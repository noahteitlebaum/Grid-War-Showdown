// Author: Noah Teitlebaum
// File Name: Crossbow.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the crossbow item

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
    class Crossbow : LongRangeAttack
    {
        //Store the crossbow damage
        private const int CROSSBOW_DAMAGE = 20;

        //Store the potential squares for the crossbow
        private const int UP = 0;
        private const int UP_LEFT = 1;
        private const int UP_RIGHT = 2;

        private const int DOWN = 3;
        private const int DOWN_LEFT = 4;
        private const int DOWN_RIGHT = 5;

        private const int MIDDLE_RIGHT = 6;
        private const int MIDDLE_LEFT = 7;

        private const int UP_UP = 8;
        private const int UP_UP_LEFT_MIDDLE = 9;
        private const int UP_UP_RIGHT_MIDDLE = 10;
        private const int UP_UP_LEFT = 11;
        private const int UP_UP_RIGHT = 12;

        private const int DOWN_DOWN = 13;
        private const int DOWN_DOWN_LEFT_MIDDLE = 14;
        private const int DOWN_DOWN_RIGHT_MIDDLE = 15;
        private const int DOWN_DOWN_LEFT = 16;
        private const int DOWN_DOWN_RIGHT = 17;

        private const int MIDDLE_LEFT_LEFT = 18;
        private const int MIDDLE_LEFT_LEFT_UP = 19;
        private const int MIDDLE_LEFT_LEFT_DOWN = 20;

        private const int MIDDLE_RIGHT_RIGHT = 21;
        private const int MIDDLE_RIGHT_RIGHT_UP = 22;
        private const int MIDDLE_RIGHT_RIGHT_DOWN = 23;

        private const int UP_UP_UP = 24;
        private const int UP_UP_UP_LEFT_RIGHT = 25;
        private const int UP_UP_UP_LEFT_MIDDLE = 26;
        private const int UP_UP_UP_LEFT_LEFT = 27;
        private const int UP_UP_UP_RIGHT_LEFT = 28;
        private const int UP_UP_UP_RIGHT_MIDDLE = 29;
        private const int UP_UP_UP_RIGHT_RIGHT = 30;

        private const int DOWN_DOWN_DOWN = 31;
        private const int DOWN_DOWN_DOWN_LEFT_RIGHT = 32;
        private const int DOWN_DOWN_DOWN_LEFT_MIDDLE = 33;
        private const int DOWN_DOWN_DOWN_LEFT_LEFT = 34;
        private const int DOWN_DOWN_DOWN_RIGHT_LEFT = 35;
        private const int DOWN_DOWN_DOWN_RIGHT_MIDDLE = 36;
        private const int DOWN_DOWN_DOWN_RIGHT_RIGHT = 37;

        private const int MIDDLE_LEFT_LEFT_LEFT = 38;
        private const int MIDDLE_LEFT_LEFT_LEFT_UP1 = 39;
        private const int MIDDLE_LEFT_LEFT_LEFT_UP2 = 40;
        private const int MIDDLE_LEFT_LEFT_LEFT_DOWN1 = 41;
        private const int MIDDLE_LEFT_LEFT_LEFT_DOWN2 = 42;

        private const int MIDDLE_RIGHT_RIGHT_RIGHT = 43;
        private const int MIDDLE_RIGHT_RIGHT_RIGHT_UP1 = 44;
        private const int MIDDLE_RIGHT_RIGHT_RIGHT_UP2 = 45;
        private const int MIDDLE_RIGHT_RIGHT_RIGHT_DOWN1 = 46;
        private const int MIDDLE_RIGHT_RIGHT_RIGHT_DOWN2 = 47;

        /// <summary>
        /// An item that does not deal a lot of damage but is easier to execute
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public Crossbow(ContentManager Content) : base(Content)
        {
            //Load the crossbow properties
            potentialSquarePoss = new Vector2[48];
            coolDown = 5;
            itemNumberText = "-" + CROSSBOW_DAMAGE;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/CrossbowSound");

            //Load the crossbow file properties
            longRangeAttackItem = FileManager.CROSSBOW;
            healthLost = CROSSBOW_DAMAGE;
        }

        //Pre: The current player
        //Post: None
        //Desc: Set the potential squares of the crossbow relative to the players position
        public override void SetPotentialSquares(Player curPlayer)
        {
            //Set the potential crossbow positions
            potentialSquarePoss[UP] = new Vector2(curPlayer.curRow, curPlayer.curCol - 1);
            potentialSquarePoss[UP_LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol - 1);
            potentialSquarePoss[UP_RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol - 1);

            potentialSquarePoss[DOWN] = new Vector2(curPlayer.curRow, curPlayer.curCol + 1);
            potentialSquarePoss[DOWN_LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol + 1);
            potentialSquarePoss[DOWN_RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol + 1);

            potentialSquarePoss[MIDDLE_LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol);
            potentialSquarePoss[MIDDLE_RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol);

            potentialSquarePoss[UP_UP] = new Vector2(curPlayer.curRow, curPlayer.curCol - 2);
            potentialSquarePoss[UP_UP_LEFT_MIDDLE] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol - 2);
            potentialSquarePoss[UP_UP_RIGHT_MIDDLE] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol - 2);
            potentialSquarePoss[UP_UP_LEFT] = new Vector2(curPlayer.curRow - 2, curPlayer.curCol - 2);
            potentialSquarePoss[UP_UP_RIGHT] = new Vector2(curPlayer.curRow + 2, curPlayer.curCol - 2);

            potentialSquarePoss[DOWN_DOWN] = new Vector2(curPlayer.curRow, curPlayer.curCol + 2);
            potentialSquarePoss[DOWN_DOWN_LEFT_MIDDLE] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol + 2);
            potentialSquarePoss[DOWN_DOWN_RIGHT_MIDDLE] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol + 2);
            potentialSquarePoss[DOWN_DOWN_LEFT] = new Vector2(curPlayer.curRow - 2, curPlayer.curCol + 2);
            potentialSquarePoss[DOWN_DOWN_RIGHT] = new Vector2(curPlayer.curRow + 2, curPlayer.curCol + 2);

            potentialSquarePoss[MIDDLE_LEFT_LEFT] = new Vector2(curPlayer.curRow - 2, curPlayer.curCol);
            potentialSquarePoss[MIDDLE_LEFT_LEFT_UP] = new Vector2(curPlayer.curRow - 2, curPlayer.curCol - 1);
            potentialSquarePoss[MIDDLE_LEFT_LEFT_DOWN] = new Vector2(curPlayer.curRow - 2, curPlayer.curCol + 1);

            potentialSquarePoss[MIDDLE_RIGHT_RIGHT] = new Vector2(curPlayer.curRow + 2, curPlayer.curCol);
            potentialSquarePoss[MIDDLE_RIGHT_RIGHT_UP] = new Vector2(curPlayer.curRow + 2, curPlayer.curCol - 1);
            potentialSquarePoss[MIDDLE_RIGHT_RIGHT_DOWN] = new Vector2(curPlayer.curRow + 2, curPlayer.curCol + 1);

            potentialSquarePoss[UP_UP_UP] = new Vector2(curPlayer.curRow, curPlayer.curCol - 3);
            potentialSquarePoss[UP_UP_UP_LEFT_RIGHT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol - 3);
            potentialSquarePoss[UP_UP_UP_LEFT_MIDDLE] = new Vector2(curPlayer.curRow - 2, curPlayer.curCol - 3);
            potentialSquarePoss[UP_UP_UP_LEFT_LEFT] = new Vector2(curPlayer.curRow - 3, curPlayer.curCol - 3);
            potentialSquarePoss[UP_UP_UP_RIGHT_LEFT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol - 3);
            potentialSquarePoss[UP_UP_UP_RIGHT_MIDDLE] = new Vector2(curPlayer.curRow + 2, curPlayer.curCol - 3);
            potentialSquarePoss[UP_UP_UP_RIGHT_RIGHT] = new Vector2(curPlayer.curRow + 3, curPlayer.curCol - 3);

            potentialSquarePoss[DOWN_DOWN_DOWN] = new Vector2(curPlayer.curRow, curPlayer.curCol + 3);
            potentialSquarePoss[DOWN_DOWN_DOWN_LEFT_RIGHT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol + 3);
            potentialSquarePoss[DOWN_DOWN_DOWN_LEFT_MIDDLE] = new Vector2(curPlayer.curRow - 2, curPlayer.curCol + 3);
            potentialSquarePoss[DOWN_DOWN_DOWN_LEFT_LEFT] = new Vector2(curPlayer.curRow - 3, curPlayer.curCol + 3);
            potentialSquarePoss[DOWN_DOWN_DOWN_RIGHT_LEFT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol + 3);
            potentialSquarePoss[DOWN_DOWN_DOWN_RIGHT_MIDDLE] = new Vector2(curPlayer.curRow + 2, curPlayer.curCol + 3);
            potentialSquarePoss[DOWN_DOWN_DOWN_RIGHT_RIGHT] = new Vector2(curPlayer.curRow + 3, curPlayer.curCol + 3);

            potentialSquarePoss[MIDDLE_LEFT_LEFT_LEFT] = new Vector2(curPlayer.curRow - 3, curPlayer.curCol);
            potentialSquarePoss[MIDDLE_LEFT_LEFT_LEFT_UP1] = new Vector2(curPlayer.curRow - 3, curPlayer.curCol - 1);
            potentialSquarePoss[MIDDLE_LEFT_LEFT_LEFT_UP2] = new Vector2(curPlayer.curRow - 3, curPlayer.curCol - 2);
            potentialSquarePoss[MIDDLE_LEFT_LEFT_LEFT_DOWN1] = new Vector2(curPlayer.curRow - 3, curPlayer.curCol + 1);
            potentialSquarePoss[MIDDLE_LEFT_LEFT_LEFT_DOWN2] = new Vector2(curPlayer.curRow - 3, curPlayer.curCol + 2);

            potentialSquarePoss[MIDDLE_RIGHT_RIGHT_RIGHT] = new Vector2(curPlayer.curRow + 3, curPlayer.curCol);
            potentialSquarePoss[MIDDLE_RIGHT_RIGHT_RIGHT_UP1] = new Vector2(curPlayer.curRow + 3, curPlayer.curCol - 1);
            potentialSquarePoss[MIDDLE_RIGHT_RIGHT_RIGHT_UP2] = new Vector2(curPlayer.curRow + 3, curPlayer.curCol - 2);
            potentialSquarePoss[MIDDLE_RIGHT_RIGHT_RIGHT_DOWN1] = new Vector2(curPlayer.curRow + 3, curPlayer.curCol + 1);
            potentialSquarePoss[MIDDLE_RIGHT_RIGHT_RIGHT_DOWN2] = new Vector2(curPlayer.curRow + 3, curPlayer.curCol + 2);

            //Convert all the potential square locations into bounding boxes
            SetBoundingBoxes();
        }
    }
}