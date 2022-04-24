# Structure
There are two C# projects in this repository. The most important is the MinesweeperLibrary. The other project is the MinesweeperConsole.

# MinesweeperLibrary
Minesweeper Library acts as a backend to any Minesweeper implementation someone wants to make. Any frontend dev can simply make a compatible Minesweeper UI and hook into this library for the logic of the game. This way someone who is good at making pretty UIs but, are not so good at writing logic can make a game of their own. 

Please also note that C# with .NET is cross-platform, so don't feel like you're restricted to Windows. You can make your Minesweeper game in Linux, MacOS, Xamarin, Blazor and with .NET MAUI coming up, you'll soon be able to make a single game that runs on all these platforms seemlessly.

The definitions of the functions to be used and the instructions on configuration are in the wiki.

Install the package through NuGet here:
https://www.nuget.org/packages/MinesweeperLibrary/

# MinesweeperConsole
This is a console version of Minesweeper. This is meant to be a demonstration of how to properly use the Minesweeper Library to make your own implementation of a Minesweeper game.

This shows how quickly and easily an implementation of Minesweeper can be made in any type of project you want (most people wouldn't make a text based Minesweeper but, it is possible with my library)

![image](https://media.discordapp.net/attachments/793633190572064788/903753764849209404/unknown.png?width=402&height=702)
