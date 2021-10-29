/*
------------------------------------------------------------------------------------------
File:    Cell.cs
Purpose: Holds state and logic of the Minesweeper cell. Should not be directly interacted
with by user of this library.
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
    /// This class shall represent the state of the cell. For people who use this 
    /// library, you shouldn't ever need to interact with this file. Instead you should be
    /// interacting with the public facing methods of Board.
    /// </summary>
    class Cell
    {
        //Properties
        public bool IsBomb { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsVisible { get; set; }
        public bool IsHighlighted { get; set; }
        public uint NumberOfSurroundingBombs { get; set; }

        //Constructors
        public Cell()
        {
            IsBomb = false;
            IsFlagged = false;
            IsVisible = false;
            IsHighlighted = false;
            NumberOfSurroundingBombs = 0;
        }

    }
}