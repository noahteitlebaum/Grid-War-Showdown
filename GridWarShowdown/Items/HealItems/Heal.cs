// Author: Noah Teitlebaum
// File Name: Heal.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the heal items

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
    class Heal : Item
    {
        //Store the potential squares for heal items
        private const int CURRENT = 0;

        //Store the current health item and how much health was gained
        protected int healthItem;
        protected int healthGained;

        /// <summary>
        /// A certain heal item used by players during game play
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public Heal(ContentManager Content) : base(Content)
        {
            //Load the heal properties
            potentialSquarePoss = new Vector2[1];
            squareImg = Content.Load<Texture2D>("Images/Sprites/HealSquare");
        }

        //Pre: The current player
        //Post: None
        //Desc: Set the potential squares of the heal item relative to the players position
        public override void SetPotentialSquares(Player curPlayer)
        {
            //Set the potential heal positions
            potentialSquarePoss[CURRENT] = new Vector2(curPlayer.curRow, curPlayer.curCol);
            SetBoundingBoxes();
        }

        //Pre: The players and the item timer
        //Post: None
        //Desc: Use the healing item
        public override void UseItem(Player curPlayer, Player nonCurPlayer, Timer itemTimer)
        {
            //Check if the health item activated was the shield potion
            if (healthItem == FileManager.SHIELD_POTION)
            {
                //Active the shield potion overtime effect
                curPlayer.isShieldPotionHeal = true;
            }

            //Increase the current players health
            curPlayer.curHealth = Math.Min(curPlayer.curHealth + healthGained, Player.MAX_HEALTH);

            //Activate the item timer
            itemTimer.Activate();
            itemSnd.CreateInstance().Play();

            //Add the items data to the file manager
            itemFileManager.AddTotalHealthPoints(healthItem, healthGained);
            itemFileManager.AddTotalItemUse(healthItem);
        }

        //Pre: Used to draw various sprites, and the opponent player
        //Post: None
        //Desc: Display the potential heal tiles
        public override void DisplayPotentialSquares(SpriteBatch spriteBatch, Player opponentPlayer)
        {
            //Draw the potential heal tile
            spriteBatch.Draw(squareImg, potentialSquareRecs[CURRENT], Color.White);
        }

        //Pre: Used to draw various sprites, the current item font, and both the players
        //Post: None
        //Desc: Determine where the item number should display based off of the heal item
        public override void DisplayItemNumber(SpriteBatch spriteBatch, SpriteFont font, Player curPlayer, Player nonCurPLayer)
        {
            //Draw the heal item number overtop the current player
            Effects.OverlapTextDisplay(spriteBatch, font, itemNumberText, GetItemNumberLoc(curPlayer), Color.White, Color.Green);
        }
    }
}