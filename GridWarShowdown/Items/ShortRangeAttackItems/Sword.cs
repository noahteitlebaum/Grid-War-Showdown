// Author: Noah Teitlebaum
// File Name: Sword.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the sword item

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
    class Sword : ShortRangeAttack
    {
        //Store the sword damage
        private const int SWORD_DAMAGE = 40;

        //Store the potential squares for the sword
        private const int UP = 0;
        private const int UP_LEFT = 1;
        private const int UP_RIGHT  = 2;

        private const int DOWN = 3;
        private const int DOWN_LEFT = 4;
        private const int DOWN_RIGHT = 5;

        private const int MIDDLE_RIGHT = 6;
        private const int MIDDLE_LEFT = 7;

        /// <summary>
        /// An item that deals a lot of damage
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public Sword(ContentManager Content) : base(Content)
        {
            //Load the sword properties
            potentialSquarePoss = new Vector2[8];
            coolDown = 5;
            itemNumberText = "-" + SWORD_DAMAGE;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/SwordSound");

            //Load the sword file properties
            shortRangeAttackItem = FileManager.SWORD;
            healthLost = SWORD_DAMAGE;
        }

        //Pre: The current player
        //Post: None
        //Desc: Set the potential squares of the sword relative to the players position
        public override void SetPotentialSquares(Player curPlayer)
        {
            //Set the potential sword positions
            potentialSquarePoss[UP_LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol - 1);
            potentialSquarePoss[UP] = new Vector2(curPlayer.curRow, curPlayer.curCol - 1);
            potentialSquarePoss[UP_RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol - 1);

            potentialSquarePoss[DOWN_LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol + 1);
            potentialSquarePoss[DOWN] = new Vector2(curPlayer.curRow, curPlayer.curCol + 1);
            potentialSquarePoss[DOWN_RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol + 1);

            potentialSquarePoss[MIDDLE_LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol);
            potentialSquarePoss[MIDDLE_RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol);

            //Convert all the potential square locations into bounding boxes
            SetBoundingBoxes();
        }
    }
}