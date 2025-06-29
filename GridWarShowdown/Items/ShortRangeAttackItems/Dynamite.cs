// Author: Noah Teitlebaum
// File Name: Dynamite.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the dynamite item

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
    class Dynamite : ShortRangeAttack
    {
        //Store the dynamite damage
        private const int DYNAMITE_DAMAGE = 45;

        //Store the potential squares for the dynamite
        private const int UP = 0;
        private const int DOWN = 1;
        private const int LEFT = 2;
        private const int RIGHT = 3;

        /// <summary>
        /// An item that does a lot of damage with flinging abilities
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public Dynamite(ContentManager Content) : base(Content)
        {
            //Load the dynamite properties
            potentialSquarePoss = new Vector2[4];
            coolDown = 6;
            itemNumberText = "-" + DYNAMITE_DAMAGE;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/DynamiteSound");

            //Load the dynamite file properties
            shortRangeAttackItem = FileManager.DYNAMITE;
            healthLost = DYNAMITE_DAMAGE;
        }

        //Pre: The current player
        //Post: None
        //Desc: Set the potential squares of the dynamite relative to the players position
        public override void SetPotentialSquares(Player curPlayer)
        {
            //Set the potential dynamite positions
            potentialSquarePoss[UP] = new Vector2(curPlayer.curRow, curPlayer.curCol - 1);
            potentialSquarePoss[DOWN] = new Vector2(curPlayer.curRow, curPlayer.curCol + 1);
            potentialSquarePoss[LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol);
            potentialSquarePoss[RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol);

            //Convert all the potential square locations into bounding boxes
            SetBoundingBoxes();
        }

        //Pre: Both the players
        //Post: None
        //Desc: Use the dynamite flinging ability
        public static void UseFlingingAbility(Player curPlayer, Player nonCurPlayer)
        {
            //Randomize a random row and collumn in the grid bounds
            int randRow = Game1.rng.Next(Grid.MIN, Grid.MAX + 1);
            int randCol = Game1.rng.Next(Grid.MIN, Grid.MAX + 1);

            //Check if the randomized position is on the current player
            while (curPlayer.curRow == randRow && curPlayer.curCol == randCol)
            {
                //Randomize a new random row and collumn in the grid bounds
                randRow = Game1.rng.Next(Grid.MIN, Grid.MAX + 1);
                randCol = Game1.rng.Next(Grid.MIN, Grid.MAX + 1);
            }

            //Set the position of the opposing player based off the randomized row and collumn
            nonCurPlayer.hoverTileRec.X = Grid.GetXLoc(randRow);
            nonCurPlayer.hoverTileRec.Y = Grid.GetYLoc(randCol);
            nonCurPlayer.PlayerRectangleAlignment();
        }
    }
}