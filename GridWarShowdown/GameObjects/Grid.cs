// Author: Noah Teitlebaum
// File Name: Grid.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the grid functionality and tile spacing

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using GameUtility;

namespace GridWarShowdown
{
    public static class Grid
    {
        //Store the grids tile spacer
        public const int TILE_SPACER = 62;

        //Store the minimum and maximum tiles in the row and collumn
        public const int MIN = 0;
        public const int MAX = 9;

        //Store the board grid as a 2D array
        static private Point[,] boardGrid;

        //Pre: None
        //Post: None
        //Desc: Set up the grid and tiles within it
        public static void SetGrid()
        {
            //Set the board to be a 10 by 10 grid
            boardGrid = new Point[10, 10];

            //Iterate through the length of the grids rows
            for (int i = 0; i < boardGrid.GetLength(0); i++)
            {
                //Iterate through the length of the grids collumns
                for (int j = 0; j < boardGrid.GetLength(1); j++)
                {
                    //Set the board grid locations
                    boardGrid[i, j] = new Point(190 + (i * TILE_SPACER), 79 + (j * TILE_SPACER));
                }
            }
        }

        //Pre: The current row
        //Post: Receiving the X location of the board grid depending on the row as an int
        //Desc: Finding the specific X location of any board grid tile
        public static int GetXLoc(int row)
        {
            //Check to see if the row is out of bounds
            if (row < Grid.MIN || row > Grid.MAX)
            {
                //Return a non existing value (null)
                return Int32.MinValue;
            }

            //Return the X location of the board grid depending on the row
            return boardGrid[row, 0].X;
        }

        //Pre: The current collumn
        //Post: Receiving the Y location of the board grid depending on the collumn as an int
        //Desc: Finding the specific Y location of any board grid tile
        public static int GetYLoc(int collumn)     
        {
            //Check to see if the collumn is out of bounds
            if (collumn < Grid.MIN || collumn > Grid.MAX)
            {
                //Return a non existing value (null)
                return Int32.MinValue;
            }

            //Return the Y location of the board grid depending on the collumn
            return boardGrid[0, collumn].Y;
        }

        //Pre: A bounding box representing a tile on the grid
        //Post: Receiving if the bounding box is inside the bounds of the grid as a bool
        //Desc: Check if the bounding box representing a tile is inside the bounds of the grid
        public static bool IsInBounds(Rectangle rec)
        {
            //Return if the bounding box representing a tile is inside the bounds of the grid
            return rec.X >= GetXLoc(Grid.MIN) && rec.X <= GetXLoc(Grid.MAX) && rec.Y >= GetYLoc(Grid.MIN) && rec.Y <= GetYLoc(Grid.MAX);
        }

        //Pre: The current collumn
        //Post: Receiving if the collumn is inside the bounds of the grid as a bool
        //Desc: Check if the collumn representing a vertical tile is inside the bounds of the grid
        public static bool IsInBoundsUp(int collumn)
        {
            //Return if the collumn is inside the bounds of the grid
            return collumn - 1 >= 0;
        }

        //Pre: The current collumn
        //Post: Receiving if the collumn is inside the bounds of the grid as a bool
        //Desc: Check if the collumn representing a vertical tile is inside the bounds of the grid
        public static bool IsInBoundsDown(int collumn)
        {
            //Return if the collumn is inside the bounds of the grid
            return collumn + 1 <= boardGrid.GetLength(1) - 1;
        }

        //Pre: The current row
        //Post: Receiving if the row is inside the bounds of the grid as a bool
        //Desc: Check if the row representing a horizontal tile is inside the bounds of the grid
        public static bool IsInBoundsLeft(int row)
        {
            //Return if the row is inside the bounds of the grid
            return row - 1 >= 0;
        }

        //Pre: The current row
        //Post: Receiving if the row is inside the bounds of the grid as a bool
        //Desc: Check if the row representing a horizontal tile is inside the bounds of the grid
        public static bool IsInBoundsRight(int row)
        {
            //Return if the row is inside the bounds of the grid
            return row + 1 <= boardGrid.GetLength(0) - 1;
        }

        //Pre: The target bounding box, with a range of a left and right index
        //Post: Receiving where the current row of the bounding box is as an int
        //Desc: Check where the current row is located
        public static int BinarySearchRow(Rectangle val, int left, int right)
        {
            //Check if the left and right have overlapped
            if (left > right)
            {
                //Return the target row as not found
                return -1;
            }

            //Find the middle index
            int mid = (left + right) / 2;

            //Check to see where the target row is placed
            if (val.X == GetXLoc(mid))
            {
                //Return the row of the target value
                return mid;
            }
            else if (val.X < GetXLoc(mid))
            {
                //The target row is in the left half
                return BinarySearchRow(val, left, mid - 1);
            }
            else
            {
                //The target row is in the right half
                return BinarySearchRow(val, mid + 1, right);
            }
        }

        //Pre: The target bounding box, with a range of a left and right index
        //Post: Receiving where the current collumn of the bounding box is as an int
        //Desc: Check where the current collumn is located
        public static int BinarySearchCollumn(Rectangle val, int left, int right)
        {
            //Check if the left and right have overlapped
            if (left > right)
            {
                //Return the target collumn as not found
                return -1;
            }

            //Find the middle index
            int mid = (left + right) / 2;

            //Check to see where the target collumn is placed
            if (val.Y == GetYLoc(mid))
            {
                //Return the collumn of the target value
                return mid;
            }
            else if (val.Y < GetYLoc(mid))
            {
                //The target collumn is in the left half
                return BinarySearchCollumn(val, left, mid - 1);
            }
            else
            {
                //The target collumn is in the right half
                return BinarySearchCollumn(val, mid + 1, right);
            }
        }
    }
}