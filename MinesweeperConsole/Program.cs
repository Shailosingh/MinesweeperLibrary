/*
------------------------------------------------------------------------------------------
File:    Program.cs
Purpose: Console minesweeper program
==========================================================================================
Program Description:
This is a console version of minesweeper. This is meant to be a demonstration of how to
properly use the Minesweeper Library to make your own implementation of a Minesweeper game.

This shows how quickly and easily an implementation of Minesweeper can be made in any type
of project you want (most people wouldn't make a text based Minesweeper but, it is possible
with my library)
------------------------------------------------------------------------------------------
Author:  Shailendra Singh
Version  2021-10-29
------------------------------------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using MinesweeperLibrary;

namespace MinesweeperConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set up symbol dictionary to interpret visual codes from board to be printed
            Dictionary<int, char> SYMBOL_DICTIONARY = new Dictionary<int, char>();
            SYMBOL_DICTIONARY.Add(0, ' ');
            for (int number = 1; number < 9; number++)
            {
                SYMBOL_DICTIONARY.Add(number, Convert.ToChar(number.ToString()));
            }
            SYMBOL_DICTIONARY.Add(9, 'F');
            SYMBOL_DICTIONARY.Add(10, 'H');
            SYMBOL_DICTIONARY.Add(11, 'X');
            SYMBOL_DICTIONARY.Add(12, 'O');

            //Setup colour for symbols on the board
            Dictionary<char, ConsoleColor> COLOUR_DICTIONARY = new Dictionary<char, ConsoleColor>();
            COLOUR_DICTIONARY.Add(' ', ConsoleColor.Black);
            COLOUR_DICTIONARY.Add('1', ConsoleColor.Blue);
            COLOUR_DICTIONARY.Add('2', ConsoleColor.Green);
            COLOUR_DICTIONARY.Add('3', ConsoleColor.Red);
            COLOUR_DICTIONARY.Add('4', ConsoleColor.DarkBlue);
            COLOUR_DICTIONARY.Add('5', ConsoleColor.DarkRed);
            COLOUR_DICTIONARY.Add('6', ConsoleColor.DarkCyan);
            COLOUR_DICTIONARY.Add('7', ConsoleColor.Magenta);
            COLOUR_DICTIONARY.Add('8', ConsoleColor.Gray);
            COLOUR_DICTIONARY.Add('F', ConsoleColor.Red);
            COLOUR_DICTIONARY.Add('H', ConsoleColor.Yellow);
            COLOUR_DICTIONARY.Add('X', ConsoleColor.DarkYellow);
            COLOUR_DICTIONARY.Add('O', ConsoleColor.White);

            //Get gameboard info from user
            bool menuValid = false;
            int rowCount = 0;
            int colCount = 0;
            int bombCount = 0;
            do
            {
                Console.Write("Number of rows: ");
                string rowString = Console.ReadLine();
                Console.Write("Number of columns: ");
                string colString = Console.ReadLine();
                Console.Write("Number of bombs: ");
                string bombString = Console.ReadLine();
                Console.WriteLine();

                if(int.TryParse(rowString, out rowCount) && int.TryParse(colString, out colCount) && int.TryParse(bombString, out bombCount))
                {
                    menuValid = true;
                }

            } while (!menuValid);

            //Create board and setup game
            Board gameBoard = new Board(rowCount, colCount, bombCount);
            int[] coordinates = new int[2];
            string menuOption;

            //Game loop
            do
            {
                //If the user is holding down a cell
                if (gameBoard.BoardBeingHeldDown())
                {
                    PrintBoard(gameBoard, COLOUR_DICTIONARY, SYMBOL_DICTIONARY);
                    Console.WriteLine("Press enter to release buttons!");
                    Console.ReadLine();
                    gameBoard.BothClickRelease(coordinates[0], coordinates[1]);
                }

                //Skips this if user just released cell. This is to solve bug where if you win by releasing the cell, it will not go to win screen
                else
                {
                    PrintBoard(gameBoard, COLOUR_DICTIONARY, SYMBOL_DICTIONARY);

                    Console.WriteLine("Choose Action");
                    Console.WriteLine("_____________");
                    Console.WriteLine("(1) Left Click");
                    Console.WriteLine("(2) Right Click");
                    Console.WriteLine("(3) Left Right Hold");
                    Console.WriteLine("(4) Reset\n");

                    do
                    {
                        menuValid = true;
                        Console.Write("Action: ");
                        menuOption = Console.ReadLine();

                        //Check if option is invalid
                        if (!(menuOption.Equals("1") || menuOption.Equals("2") || menuOption.Equals("3") || menuOption.Equals("4")))
                        {
                            menuValid = false;
                        }
                        Console.WriteLine();
                    } while (!menuValid);

                    //Menu options
                    //Left click
                    if (menuOption.Equals("1"))
                    {
                        coordinates = RowColInput(gameBoard);
                        gameBoard.LeftClick(coordinates[0], coordinates[1]);
                        Console.WriteLine("Left Click!");
                    }

                    //Right click
                    else if (menuOption.Equals("2"))
                    {
                        coordinates = RowColInput(gameBoard);
                        gameBoard.RightClick(coordinates[0], coordinates[1]);
                        Console.WriteLine("Right Click!");
                    }

                    //Left Right hold
                    else if (menuOption.Equals("3"))
                    {
                        coordinates = RowColInput(gameBoard);
                        gameBoard.BothClickHoldDown(coordinates[0], coordinates[1]);
                        Console.WriteLine("Buttons held!");
                    }

                    //Reset
                    else
                    {
                        gameBoard.Reset();
                        Console.WriteLine("Game reset");
                    }
                }

            } while (gameBoard.GameStillRunning());

            //Check if you won or lost the game
            PrintBoard(gameBoard, COLOUR_DICTIONARY, SYMBOL_DICTIONARY);
            if(gameBoard.GameIsWon())
            {
                Console.WriteLine("Woohoo you won!");
                Console.ReadLine();
            }

            else
            {
                Console.WriteLine("You lost, poor you!");
                Console.ReadLine();
            }

        }

        /// <summary>
        /// This method allows the user to input the row and columns to be selected on the board.
        /// </summary>
        /// <param name="gameBoard">The game board object</param>
        /// <returns>Array where index 0 is the row index and index 1 is the column index</returns>
        static int[] RowColInput(Board gameBoard)
        {
            //Initialize variables
            bool menuValid = false;
            string rowInputString;
            int rowInput = -1;
            string colInputString;
            int colInput = -1;

            //Ask for row and column until the inputs are valid
            do
            {
                Console.Write("Row: ");
                rowInputString = Console.ReadLine();
                Console.Write("Col: ");
                colInputString = Console.ReadLine();

                if (int.TryParse(rowInputString, out rowInput) && int.TryParse(colInputString, out colInput))
                {
                    menuValid = true;
                }
                Console.WriteLine();

            } while (!(menuValid && gameBoard.CoordinateIsValid(rowInput, colInput)));

            int[] coordinates = { rowInput, colInput };
            return coordinates;
        }

        /// <summary>
        /// This takes in the game board and the dictionaries of constants to draw a colour coded
        /// minesweeper grid and a label telling the user how many flags are left
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="COLOUR_DICTIONARY">Maps each character to a specific colour</param>
        /// <param name="SYMBOL_DICTIONARY">Maps each integer given from the game board to its appropriate character</param>
        static void PrintBoard(Board gameBoard, Dictionary<char, ConsoleColor> COLOUR_DICTIONARY, Dictionary<int, char> SYMBOL_DICTIONARY)
        {
            //Print top number label (x-axis labelling)
            Console.WriteLine();
            Console.Write("   ");
            for (int row = 0; row < gameBoard.GetNumberOfCols(); row++)
            {
                Console.Write(row.ToString().PadRight(3));
            }
            Console.WriteLine();

            for (int row = 0; row < gameBoard.GetNumberOfRows(); row++)
            {
                //Print the spacing above each row and the number labels next to them (y-axis labeling)
                Console.Write("  ");
                for (int wallIndex = 0; wallIndex < gameBoard.GetNumberOfCols(); wallIndex++)
                {
                    Console.Write("---");
                }

                Console.WriteLine();
                Console.Write(row.ToString().PadRight(2));

                //Print character and bars between them
                for (int col = 0; col < gameBoard.GetNumberOfCols(); col++)
                {
                    Console.Write("|");
                    Console.ForegroundColor = COLOUR_DICTIONARY[SYMBOL_DICTIONARY[gameBoard.CellVisualStatus(row, col)]];
                    Console.Write(SYMBOL_DICTIONARY[gameBoard.CellVisualStatus(row, col)]);
                    Console.ResetColor();
                    Console.Write("|");
                }
                Console.WriteLine();
            }

            //Prints very bottom bar of grid
            Console.Write("  ");
            for (int wallIndex = 0; wallIndex < gameBoard.GetNumberOfCols(); wallIndex++)
            {
                Console.Write("---");
            }

            //Print flag tally
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Number of flags remaining: {gameBoard.GetRemainingFlags()}");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}