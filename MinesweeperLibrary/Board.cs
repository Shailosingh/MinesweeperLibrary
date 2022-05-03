/*
------------------------------------------------------------------------------------------
File:    Board.cs
Purpose: Holds state and logic of the Minesweeper gameboard. This is the object that the
UI should be interacted with through the public facing methods
==========================================================================================
Program Description:
Minesweeper Library acts as a backend to any Minesweeper implementation someone wants to 
make. Any frontend dev can simply make a compatible Minesweeper UI and hook into this 
library for the logic of the game. This way someone who is good at making pretty UIs but,
are not so good at writing logic can make a game of their own. 

Please also note that C# with .NET is cross-platform, so don't feel like you're restricted to 
Windows. You can makeyour Minesweeper game in Linux, MacOS, Xamarin, Blazor and with .NET 
MAUI coming up, you'll soon be able to make a single game that runs on all these platforms 
seemlessly.

Good luck with your coding 😊!
------------------------------------------------------------------------------------------
Author:  Shailendra Singh
Version  2021-10-26
------------------------------------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperLibrary
{
    /// <summary>
    /// This class represents the state and logic of the board. This is where your UI should be
    /// hooked to and the board be interacted with.
    /// NOTE: Rows and columns indices of your minesweeper grid should both start at 0. Max Rows and
    /// Column length is 50
    /// </summary>
    public class Board
    {
        //CONSTANTS
        public static uint MIN_ROWS { get; private set; } = 1;
        public static uint MAX_ROWS { get; private set; } = 50;
        public static uint MIN_COLS { get; private set; } = 4;
        public static uint MAX_COLS { get; private set; } = 50;

        //Datafields
        private Cell[,] CellGrid;
        private bool GameComplete;
        private bool GameLost;
        private bool GameUntouched;
        public bool IsHeldDown { get; private set; }
        private uint NumberOfSquares;
        public uint NumberOfColumns { get; private set; }
        public  uint NumberOfRows { get; private set; }
        private uint NumberOfBombs;
        private uint RemainingCleanSquares;
        public int RemainingFlags { get; private set; }

        //Constructor
        public Board(int rowNumber, int columnNumber, int bombNumber)
        {
            //Load in basics
            GameComplete = false;
            GameLost = false;
            GameUntouched = true;
            IsHeldDown = false;

            //Load in column, row and bomb number. Ensure they are within constraints
            //Columns------------------------------------------------------------------------------
            if(columnNumber < MIN_COLS)
            {
                NumberOfColumns = MIN_COLS;
            }

            else if(MAX_COLS < columnNumber)
            {
                NumberOfColumns = MAX_COLS;
            }

            else
            {
                NumberOfColumns = (uint)columnNumber;
            }

            //Rows---------------------------------------------------------------------------------
            if (rowNumber < MIN_ROWS)
            {
                NumberOfRows = MIN_ROWS;
            }

            else if (MAX_ROWS < rowNumber)
            {
                NumberOfRows = MAX_ROWS;
            }

            else
            {
                NumberOfRows = (uint)rowNumber;
            }

            //Bombs and number of squares----------------------------------------------------------
            NumberOfSquares = NumberOfRows * NumberOfColumns;

            //Ensure there is at least 1 bomb
            if(bombNumber < 1)
            {
                NumberOfBombs = 1;
            }

            //Ensure there is at least 1 free square
            else if(bombNumber >= NumberOfSquares)
            {
                NumberOfBombs = NumberOfSquares - 1;
            }

            else
            {
                NumberOfBombs = (uint)bombNumber;
            }

            //Remaining flags, clean squares and cell grid-----------------------------------------
            RemainingFlags = (int)NumberOfBombs;
            RemainingCleanSquares = NumberOfSquares - NumberOfBombs;
            CellGrid = new Cell[NumberOfRows, NumberOfColumns];

            for (int rowIndex = 0; rowIndex < NumberOfRows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < NumberOfColumns; colIndex++)
                {
                    CellGrid[rowIndex, colIndex] = new Cell();
                }
            }

            //Load the mines into the grid randomly------------------------------------------------
            uint minesRemaining = NumberOfBombs;
            Random rand = new Random();
            int row;
            int col;

            do
            {
                row = rand.Next(0, (int)NumberOfRows);
                col = rand.Next(0, (int)NumberOfColumns);

                //Ensure that the space isn't already a bomb and then put the bomb in
                if(!CellGrid[row, col].IsBomb)
                {
                    CellGrid[row, col].IsBomb = true;
                    minesRemaining--;
                }

            } while (minesRemaining != 0);
        }

        //Board interactions-----------------------------------------------------------------------
        /// <summary>
        /// This resets the entire board back to default values. Hook this to your reset button. 
        /// In MS Minesweeper, that was the little yellow face up top.
        /// </summary>
        public void Reset()
        {
            //Reset basic values
            GameComplete = false;
            GameLost = false;
            GameUntouched = true;
            RemainingFlags = (int)NumberOfBombs;
            RemainingCleanSquares = NumberOfSquares - NumberOfBombs;
            CellGrid = new Cell[NumberOfRows, NumberOfColumns];

            //Create new grid
            for (int rowIndex = 0; rowIndex < NumberOfRows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < NumberOfColumns; colIndex++)
                {
                    CellGrid[rowIndex, colIndex] = new Cell();
                }
            }

            //Load the mines into the grid randomly
            uint minesRemaining = NumberOfBombs;
            Random rand = new Random();
            int row;
            int col;

            do
            {
                row = rand.Next(0, (int)NumberOfRows);
                col = rand.Next(0, (int)NumberOfColumns);

                //Ensure that the space isn't already a bomb and then put the bomb in
                if (!CellGrid[row, col].IsBomb)
                {
                    CellGrid[row, col].IsBomb = true;
                    minesRemaining--;
                }

            } while (minesRemaining != 0);
        }

        /// <summary>
        /// Handles left click events. Left clicks make visible hidden and unflagged squares.
        /// It will do nothing to other cell types
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        public void LeftClick(int row, int col)
        {
            //On first valid unflagged left click
            if(CoordinateIsValid(row, col) && !CellGrid[row,col].IsFlagged && GameUntouched)
            {
                //Set variable to show board has now been touched (clicked on)
                GameUntouched = false;

                //If the current cell is a mine, move the mine somewhere else
                if(CellGrid[row,col].IsBomb)
                {
                    Random rand = new Random();
                    int newBombRow;
                    int newBombCol;

                    do
                    {
                        newBombRow = rand.Next(0, (int)NumberOfRows);
                        newBombCol = rand.Next(0, (int)NumberOfColumns);
                    } while (row != newBombRow && col != newBombCol && CellGrid[newBombRow, newBombCol].IsBomb);

                    CellGrid[newBombRow, newBombCol].IsBomb = true;
                    CellGrid[row, col].IsBomb = false;
                }

                //Find the number of mines surrounding every cell (except for cells which are mines) and set them
                for (int rowIndex = 0; rowIndex < NumberOfRows; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < NumberOfColumns; colIndex++)
                    {
                        if(!CellGrid[rowIndex,colIndex].IsBomb)
                        {
                            CellGrid[rowIndex, colIndex].NumberOfSurroundingBombs = FindSurroundingMines(rowIndex, colIndex);
                        }
                    }
                }
            }

            //All valid left clicks on hidden, unflagged squares
            if(CoordinateIsValid(row,col) && !CellGrid[row,col].IsVisible &&!CellGrid[row,col].IsFlagged)
            {
                //If this coordinate is a bomb, lose the game
                if(CellGrid[row,col].IsBomb)
                {
                    GameComplete = true;
                    GameLost = true;
                }

                //If the cell is not a bomb, make it visible and reduce the counter for number of clean squares
                else
                {
                    CellGrid[row, col].IsVisible = true;
                    RemainingCleanSquares--;
                }

                //If the current cell (not a bomb) is not surrounded by any bombs, recursively left click surrounding squares (floodfill)
                if(!CellGrid[row, col].IsBomb && CellGrid[row,col].NumberOfSurroundingBombs==0)
                {
                    int rowOrigin = row - 1;
                    int colOrigin = col - 1;
                    int rowIndex;
                    int colIndex;

                    //Cycle through surrounding cells
                    for (int rowOffset = 0; rowOffset < 3; rowOffset++)
                    {
                        for (int colOffset = 0; colOffset < 3; colOffset++)
                        {
                            //Left click on surrounding cells
                            rowIndex = rowOrigin + rowOffset;
                            colIndex = colOrigin + colOffset;
                            LeftClick(rowIndex, colIndex);               
                        }
                    }
                }

                //Now check if the game has been won (No more safe squares)
                if(!CellGrid[row, col].IsBomb && RemainingCleanSquares == 0)
                {
                    GameComplete = true;
                    GameLost = false;
                }
            }
        }

        /// <summary>
        /// Handles right click events. Right clicks flag/unflag hidden squares and does nothing with
        /// invalid input or visible squares.
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        public void RightClick(int row, int col)
        {
            //Ensure that the grid is valid and is not visible, so it can be flagged or unflagged
            if(CoordinateIsValid(row,col) && !CellGrid[row,col].IsVisible)
            {
                if(CellGrid[row,col].IsFlagged)
                {
                    CellGrid[row, col].IsFlagged = false;
                    RemainingFlags++;
                }

                else
                {
                    CellGrid[row, col].IsFlagged = true;
                    RemainingFlags--;
                }
            }
        }

        /// <summary>
        /// Handles holding down both left and right mouse button event. This will highlight all
        /// the hidden squares surrounding square (row, col)
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        public void BothClickHoldDown(int row, int col)
        {
            //Ensure coordinate is valid
            if (CoordinateIsValid(row, col))
            {
                //Get upper left corner square of current square
                int rowIndexOrigin = row - 1;
                int colIndexOrigin = col - 1;
                int rowIndex;
                int colIndex;

                //Highlight all surrounding valid squares and the selected square, if it is not flagged
                for (int rowOffset = 0; rowOffset < 3; rowOffset++)
                {
                    for (int colOffset = 0; colOffset < 3; colOffset++)
                    {
                        //Calculate current indices
                        rowIndex = rowIndexOrigin + rowOffset;
                        colIndex = colIndexOrigin + colOffset;

                        if (CoordinateIsValid(rowIndex, colIndex) && !CellGrid[row, col].IsFlagged)
                        {
                            CellGrid[rowIndex, colIndex].IsHighlighted = true;
                        }
                    }
                }

                //Mark the board as being held down
                IsHeldDown = true;
            }
        }

        /// <summary>
        /// Handles releasing both left and right mouse button event. This will remove the highlight
        /// of every highlighted cell. If a visible cell was being held down and the number of bombs
        /// around the cell is equal to the number of flags, then click all the other cells around 
        /// it.
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        public void BothClickRelease(int row, int col)
        {
            //Ensure current cell is valid
            if (CoordinateIsValid(row, col))
            {
                //Get upper left corner square of current square
                int rowIndexOrigin = row - 1;
                int colIndexOrigin = col - 1;
                int rowIndex;
                int colIndex;

                //Unhighlight all surrounding valid squares and the selected square, if it is not flagged
                for (int rowOffset = 0; rowOffset < 3; rowOffset++)
                {
                    for (int colOffset = 0; colOffset < 3; colOffset++)
                    {
                        //Calculate current indices
                        rowIndex = rowIndexOrigin + rowOffset;
                        colIndex = colIndexOrigin + colOffset;

                        if (CoordinateIsValid(rowIndex, colIndex) && !CellGrid[row, col].IsFlagged)
                        {
                            CellGrid[rowIndex, colIndex].IsHighlighted = false;
                        }
                    }
                }

                //If the current cell is visible and has the same number of flags around it as bombs, left click other squares
                if (CellGrid[row, col].IsVisible && FindSurroundingFlags(row,col) == CellGrid[row,col].NumberOfSurroundingBombs)
                {
                    //Get upper left corner square of current square
                    rowIndexOrigin = row - 1;
                    colIndexOrigin = col - 1;

                    //Cycle through surrounding unclicked, unflagged cells and click em
                    for (int rowOffset = 0; rowOffset < 3; rowOffset++)
                    {
                        for (int colOffset = 0; colOffset < 3; colOffset++)
                        {
                            //Calculate current indices
                            rowIndex = rowIndexOrigin + rowOffset;
                            colIndex = colIndexOrigin + colOffset;

                            if(CoordinateIsValid(rowIndex, colIndex) && !(row == rowIndex && col == colIndex) && !CellGrid[rowIndex,colIndex].IsVisible &&!CellGrid[rowIndex, colIndex].IsFlagged)
                            {
                                LeftClick(rowIndex, colIndex);
                            }
                        }
                    }

                }

                //Mark the board as no longer being held down
                IsHeldDown = false;
            }
        }

        //Board monitors---------------------------------------------------------------------------
        /// <summary>
        /// Checks if the user won the game.
        /// </summary>
        /// <returns>True if the game was won, false if not</returns>
        public bool GameIsWon()
        {
            return GameComplete && !GameLost;
        }

        /// <summary>
        /// Checks if the user lost the game
        /// </summary>
        /// <returns>True if the game was lost, false otherwise</returns>
        public bool GameIsLost()
        {
            return GameComplete && GameLost;
        }

        /// <summary>
        /// Checks if the game is still running
        /// </summary>
        /// <returns>True if the game is still active. False if not</returns>
        public bool GameStillRunning()
        {
            return !GameComplete;
        }
        
        /// <summary>
        /// Checks if a cell is being held down on the board
        /// </summary>
        /// <returns>Whether a cell is being held down</returns>
        public bool BoardBeingHeldDown()
        {
            return IsHeldDown;
        }

        /// <summary>
        /// Gets number of rows in board
        /// </summary>
        /// <returns>Number of rows</returns>
        public int GetNumberOfRows()
        {
            return (int)NumberOfRows;
        }

        /// <summary>
        /// Gets number of columns in board
        /// </summary>
        /// <returns>Number of columns</returns>
        public int GetNumberOfCols()
        {
            return (int)NumberOfColumns;
        }

        /// <summary>
        /// Gets remaining number of flags. Will go into negatives if user uses too many flags
        /// (false flagging safe cells)
        /// </summary>
        /// <returns>The number of remaining flags</returns>
        public int GetRemainingFlags()
        {
            return RemainingFlags;
        }

        /// <summary>
        /// Gets a code that indicates the visual status of the specified cell. Anyone who uses 
        /// this library is advised to use a dictionary or some other similar structure to map
        /// each code to the appropriate image on the cell.
        /// </summary>
        /// <param name="row">Row Index</param>
        /// <param name="col">Column Index</param>
        /// <returns>
        /// -1: Invalid coordinate,
        /// 0-8: Number of mines around cell (should display blank square if 0)
        /// 9: Flagged,
        /// 10: Highlighted,
        /// 11: Mine (Will be displayed when game is lost)
        /// 12: Hidden cell
        /// </returns>
        public int CellVisualStatus(int row, int col)
        {
            //Check if cell coords are invalid
            if(!CoordinateIsValid(row,col))
            {
                return -1;
            }

            //Check if the coordinate appears highlighted (not visible, not flagged)
            else if(IsHighlighted(row,col))
            {
                return 10;
            }

            //If the game is over and the cell is a bomb, then the bomb should always be displayed. The game is over after all
            else if(!GameStillRunning() && CellGrid[row,col].IsBomb)
            {
                return 11;
            }

            //Check if the cell is flagged
            else if(CellGrid[row,col].IsFlagged)
            {
                return 9;
            }

            //If the cell is still not visible, it must still be displayed as hidden
            else if(!CellGrid[row,col].IsVisible)
            {
                return 12;
            }

            //Now whatever the number of surrounding bombs of the cell can be outputted
            else
            {
                return (int)CellGrid[row, col].NumberOfSurroundingBombs;
            }


        }

        //Public helpers---------------------------------------------------------------------------
        /// <summary>
        /// Checks if the given indices for row and column are valid for the grid.
        /// </summary>
        /// <param name="row">Row Index</param>
        /// <param name="col">Column Index</param>
        /// <returns>True if the coordinate is valid, false if not</returns>
        public bool CoordinateIsValid(int row, int col)
        {
            //Validate row
            if(row < 0 || row >= NumberOfRows)
            {
                return false;
            }

            //Validate column
            if (col < 0 || col >= NumberOfColumns)
            {
                return false;
            }

            //If it got this far coordinate must be valid
            return true;
        }

        //Private helpers--------------------------------------------------------------------------
        /// <summary>
        /// Checks if the grid is highlighted visibly on the grid. This is private so it can be used 
        /// internally in the method that shall eventually give the visual status of the coordinate to
        /// the user.
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        /// <returns></returns>
        private bool IsHighlighted(int row, int col)
        {
            return !CellGrid[row, col].IsVisible && !CellGrid[row,col].IsFlagged && CellGrid[row, col].IsHighlighted;
        }

        /// <summary>
        /// Find the number of mines surrounding a given coordinate for a cell
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        /// <returns>The number of mines around a coordinate</returns>
        private uint FindSurroundingMines(int row, int col)
        {
            uint surroundingMines = 0;

            //Cycle through all the cells around the selected cell and count number of mines
            int rowIndexOrigin = row - 1;
            int colIndexOrigin = col - 1;
            int rowIndex;
            int colIndex;
            for (int rowOffset = 0; rowOffset < 3; rowOffset++)
            {
                for (int colOffset = 0; colOffset < 3; colOffset++)
                {
                    //Calculate current indices to be checked
                    rowIndex = rowIndexOrigin + rowOffset;
                    colIndex = colIndexOrigin + colOffset;

                    //If the coordinate is valid, not the selected cell and a bomb, increment the surroundingMine count
                    if (CoordinateIsValid(rowIndex, colIndex) && !(row == rowIndex && col == colIndex) && CellGrid[rowIndex,colIndex].IsBomb)
                    {
                        surroundingMines++;
                    }
                }
            }

            //Return final surrounding mine count
            return surroundingMines;
        }

        /// <summary>
        /// Determines the number of flags surrounding  the selected cell
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="col">Column index</param>
        /// <returns>Number of flags around selected square</returns>
        private uint FindSurroundingFlags(int row, int col)
        {
            uint surroundingFlags = 0;

            //Cycle through all the cells around the selected cell and count number of flags
            int rowIndexOrigin = row - 1;
            int colIndexOrigin = col - 1;
            int rowIndex;
            int colIndex;
            for (int rowOffset = 0; rowOffset < 3; rowOffset++)
            {
                for (int colOffset = 0; colOffset < 3; colOffset++)
                {
                    //Calculate current indices to be checked
                    rowIndex = rowIndexOrigin + rowOffset;
                    colIndex = colIndexOrigin + colOffset;

                    //If the coordinate is valid, not the selected cell and flagged, increment the surroundingFlag count
                    if (CoordinateIsValid(rowIndex, colIndex) && !(row == rowIndex && col == colIndex) && CellGrid[rowIndex, colIndex].IsFlagged)
                    {
                        surroundingFlags++;
                    }
                }
            }

            //Return final surrounding flag count
            return surroundingFlags;
        }
    }
}
