// Author: Noah Teitlebaum
// File Name: Harpoon.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the harpoon item

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
using System.IO;

namespace GridWarShowdown
{
    class Harpoon : LongRangeAttack
    {
        //Store the harpoon damage
        private const int HARPOON_DAMAGE = 10;

        //Store the potential squares for the harpoon
        private const int UP1 = 0;
        private const int UP2 = 1;
        private const int UP3 = 2;
        private const int UP4 = 3;
        private const int UP5 = 4;
        private const int UP6 = 5;
        private const int UP7 = 6;
        private const int UP8 = 7;
        private const int UP9 = 8;
        private const int UP10 = 9;

        private const int DOWN1 = 10;
        private const int DOWN2 = 11;
        private const int DOWN3 = 12;
        private const int DOWN4 = 13;
        private const int DOWN5 = 14;
        private const int DOWN6 = 15;
        private const int DOWN7 = 16;
        private const int DOWN8 = 17;
        private const int DOWN9 = 18;
        private const int DOWN10 = 19;

        private const int LEFT1 = 20;
        private const int LEFT2 = 21;
        private const int LEFT3 = 22;
        private const int LEFT4 = 23;
        private const int LEFT5 = 24;
        private const int LEFT6 = 25;
        private const int LEFT7 = 26;
        private const int LEFT8 = 27;
        private const int LEFT9 = 28;
        private const int LEFT10 = 29;

        private const int RIGHT1 = 30;
        private const int RIGHT2 = 31;
        private const int RIGHT3 = 32;
        private const int RIGHT4 = 33;
        private const int RIGHT5 = 34;
        private const int RIGHT6 = 35;
        private const int RIGHT7 = 36;
        private const int RIGHT8 = 37;
        private const int RIGHT9 = 38;
        private const int RIGHT10 = 39;

        /// <summary>
        /// An item with unlimited range that grapples the opponent
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public Harpoon(ContentManager Content) : base(Content)
        {
            //Load the harpoon properties
            potentialSquarePoss = new Vector2[40];
            coolDown = 4;
            itemNumberText = "-" + HARPOON_DAMAGE;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/HarpoonSound");

            //Load the harpoon file properties
            longRangeAttackItem = FileManager.HARPOON;
            healthLost = HARPOON_DAMAGE;
        }

        //Pre: The current player
        //Post: None
        //Desc: Set the potential squares of the harpoon relative to the players position
        public override void SetPotentialSquares(Player curPlayer)
        {
            //Set the potential harpoon positions
            potentialSquarePoss[UP1] = new Vector2(curPlayer.curRow, curPlayer.curCol - 1);
            potentialSquarePoss[UP2] = new Vector2(curPlayer.curRow, curPlayer.curCol - 2);
            potentialSquarePoss[UP3] = new Vector2(curPlayer.curRow, curPlayer.curCol - 3);
            potentialSquarePoss[UP4] = new Vector2(curPlayer.curRow, curPlayer.curCol - 4);
            potentialSquarePoss[UP5] = new Vector2(curPlayer.curRow, curPlayer.curCol - 5);
            potentialSquarePoss[UP6] = new Vector2(curPlayer.curRow, curPlayer.curCol - 6);
            potentialSquarePoss[UP7] = new Vector2(curPlayer.curRow, curPlayer.curCol - 7);
            potentialSquarePoss[UP8] = new Vector2(curPlayer.curRow, curPlayer.curCol - 8);
            potentialSquarePoss[UP9] = new Vector2(curPlayer.curRow, curPlayer.curCol - 9);
            potentialSquarePoss[UP10] = new Vector2(curPlayer.curRow, curPlayer.curCol - 10);

            potentialSquarePoss[DOWN1] = new Vector2(curPlayer.curRow, curPlayer.curCol + 1);
            potentialSquarePoss[DOWN2] = new Vector2(curPlayer.curRow, curPlayer.curCol + 2);
            potentialSquarePoss[DOWN3] = new Vector2(curPlayer.curRow, curPlayer.curCol + 3);
            potentialSquarePoss[DOWN4] = new Vector2(curPlayer.curRow, curPlayer.curCol + 4);
            potentialSquarePoss[DOWN5] = new Vector2(curPlayer.curRow, curPlayer.curCol + 5);
            potentialSquarePoss[DOWN6] = new Vector2(curPlayer.curRow, curPlayer.curCol + 6);
            potentialSquarePoss[DOWN7] = new Vector2(curPlayer.curRow, curPlayer.curCol + 7);
            potentialSquarePoss[DOWN8] = new Vector2(curPlayer.curRow, curPlayer.curCol + 8);
            potentialSquarePoss[DOWN9] = new Vector2(curPlayer.curRow, curPlayer.curCol + 9);
            potentialSquarePoss[DOWN10] = new Vector2(curPlayer.curRow, curPlayer.curCol + 10);

            potentialSquarePoss[LEFT1] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol);
            potentialSquarePoss[LEFT2] = new Vector2(curPlayer.curRow - 2, curPlayer.curCol);
            potentialSquarePoss[LEFT3] = new Vector2(curPlayer.curRow - 3, curPlayer.curCol);
            potentialSquarePoss[LEFT4] = new Vector2(curPlayer.curRow - 4, curPlayer.curCol);
            potentialSquarePoss[LEFT5] = new Vector2(curPlayer.curRow - 5, curPlayer.curCol);
            potentialSquarePoss[LEFT6] = new Vector2(curPlayer.curRow - 6, curPlayer.curCol);
            potentialSquarePoss[LEFT7] = new Vector2(curPlayer.curRow - 7, curPlayer.curCol);
            potentialSquarePoss[LEFT8] = new Vector2(curPlayer.curRow - 8, curPlayer.curCol);
            potentialSquarePoss[LEFT9] = new Vector2(curPlayer.curRow - 9, curPlayer.curCol);
            potentialSquarePoss[LEFT10] = new Vector2(curPlayer.curRow - 10, curPlayer.curCol);

            potentialSquarePoss[RIGHT1] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol);
            potentialSquarePoss[RIGHT2] = new Vector2(curPlayer.curRow + 2, curPlayer.curCol);
            potentialSquarePoss[RIGHT3] = new Vector2(curPlayer.curRow + 3, curPlayer.curCol);
            potentialSquarePoss[RIGHT4] = new Vector2(curPlayer.curRow + 4, curPlayer.curCol);
            potentialSquarePoss[RIGHT5] = new Vector2(curPlayer.curRow + 5, curPlayer.curCol);
            potentialSquarePoss[RIGHT6] = new Vector2(curPlayer.curRow + 6, curPlayer.curCol);
            potentialSquarePoss[RIGHT7] = new Vector2(curPlayer.curRow + 7, curPlayer.curCol);
            potentialSquarePoss[RIGHT8] = new Vector2(curPlayer.curRow + 8, curPlayer.curCol);
            potentialSquarePoss[RIGHT9] = new Vector2(curPlayer.curRow + 9, curPlayer.curCol);
            potentialSquarePoss[RIGHT10] = new Vector2(curPlayer.curRow + 10, curPlayer.curCol);

            //Convert all the potential square locations into bounding boxes
            SetBoundingBoxes();
        }

        //Pre: Both the players
        //Post: None
        //Desc: Use the harpoon grappling ability
        public static void UseGrapplingAbility(Player curPlayer, Player nonCurPlayer)
        {
            //Check where the non current player is relative to the current player
            if (nonCurPlayer.curCol < curPlayer.curCol)
            {
                //Set the non current player above the current player 
                nonCurPlayer.hoverTileRec.Y = Grid.GetYLoc(curPlayer.curCol - 1);
            }
            else if (nonCurPlayer.curCol > curPlayer.curCol)
            {
                //Set the non current player below the current player 
                nonCurPlayer.hoverTileRec.Y = Grid.GetYLoc(curPlayer.curCol + 1);
            }
            else if (nonCurPlayer.curRow < curPlayer.curRow)
            {
                //Set the non current player to the left of the current player 
                nonCurPlayer.hoverTileRec.X = Grid.GetXLoc(curPlayer.curRow - 1);
            }
            else if (nonCurPlayer.curRow > curPlayer.curRow)
            {
                //Set the non current player to the right of the current player 
                nonCurPlayer.hoverTileRec.X = Grid.GetXLoc(curPlayer.curRow + 1);
            }

            //Align the grappled player to its hover tile location
            nonCurPlayer.PlayerRectangleAlignment();
        }
    }
}