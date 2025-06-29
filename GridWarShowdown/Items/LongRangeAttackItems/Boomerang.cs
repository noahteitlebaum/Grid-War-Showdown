// Author: Noah Teitlebaum
// File Name: Boomerang.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the boomerang item

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
    class Boomerang : LongRangeAttack
    {
        //Store the boomerang damage
        private const int BOOMERANG_DAMAGE = 35;

        //Store the potential squares for the boomerang
        private const int LEFT = 0;
        private const int LEFT_LEFT = 1;
        private const int LEFT_LEFT_UP_LEFT = 2;
        private const int LEFT_LEFT_UP_LEFT_UP = 3;

        private const int RIGHT = 4;
        private const int RIGHT_RIGHT = 5;
        private const int RIGHT_RIGHT_UP_RIGHT = 6;
        private const int RIGHT_RIGHT_UP_RIGHT_UP = 7;

        private const int TOP_LEFT_CORNER = 8;
        private const int TOP_RIGHT_CORNER = 9;

        private const int TOP_LEFT = 10;
        private const int TOP_MIDDLE = 11;
        private const int TOP_RIGHT = 12;

        /// <summary>
        /// An item that deals a lot of damage but is more difficult to execute
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public Boomerang(ContentManager Content) : base(Content)
        {
            //Load the boomerang properties
            potentialSquarePoss = new Vector2[13];
            coolDown = 4;
            itemNumberText = "-" + BOOMERANG_DAMAGE;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/BoomerangSound");

            //Load the boomerang file properties
            longRangeAttackItem = FileManager.BOOMERANG;
            healthLost = BOOMERANG_DAMAGE;
        }

        //Pre: The current player
        //Post: None
        //Desc: Set the potential squares of the boomerang relative to the players position
        public override void SetPotentialSquares(Player curPlayer)
        {
            //Set the potential boomerang positions
            potentialSquarePoss[LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol);
            potentialSquarePoss[LEFT_LEFT] = new Vector2(curPlayer.curRow - 2, curPlayer.curCol);
            potentialSquarePoss[LEFT_LEFT_UP_LEFT] = new Vector2(curPlayer.curRow - 3, curPlayer.curCol - 1);
            potentialSquarePoss[LEFT_LEFT_UP_LEFT_UP] = new Vector2(curPlayer.curRow - 3, curPlayer.curCol - 2);

            potentialSquarePoss[RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol);
            potentialSquarePoss[RIGHT_RIGHT] = new Vector2(curPlayer.curRow + 2, curPlayer.curCol);
            potentialSquarePoss[RIGHT_RIGHT_UP_RIGHT] = new Vector2(curPlayer.curRow + 3, curPlayer.curCol - 1);
            potentialSquarePoss[RIGHT_RIGHT_UP_RIGHT_UP] = new Vector2(curPlayer.curRow + 3, curPlayer.curCol - 2);

            potentialSquarePoss[TOP_LEFT_CORNER] = new Vector2(curPlayer.curRow - 2, curPlayer.curCol - 3);
            potentialSquarePoss[TOP_RIGHT_CORNER] = new Vector2(curPlayer.curRow + 2, curPlayer.curCol - 3);

            potentialSquarePoss[TOP_LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol - 4);
            potentialSquarePoss[TOP_MIDDLE] = new Vector2(curPlayer.curRow, curPlayer.curCol - 4);
            potentialSquarePoss[TOP_RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol - 4);

            //Convert all the potential square locations into bounding boxes
            SetBoundingBoxes();
        }
    }
}