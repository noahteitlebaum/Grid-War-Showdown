// Author: Noah Teitlebaum
// File Name: LongRangeAttack.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the long range attack items

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
    class LongRangeAttack : Item
    {
        //Store the current long range item and how much health was dealt
        protected int longRangeAttackItem;
        protected int healthLost;

        /// <summary>
        /// A certain long range attack item used by players during game play
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public LongRangeAttack(ContentManager Content) : base(Content)
        {
            //Load the long range attack square image
            squareImg = Content.Load<Texture2D>("Images/Sprites/LongRangeSquare");
        }

        //Pre: The players and the item timer
        //Post: None
        //Desc: Use the long range item
        public override void UseItem(Player curPlayer, Player nonCurPlayer, Timer itemTimer)
        {
            //Check if the player was hit
            if (IsPlayerHit(nonCurPlayer))
            {
                //Check if the guardian shield effect is not active
                if (!nonCurPlayer.isGuardianShieldActive)
                {
                    //Check if the long rang item is the harpoon
                    if (longRangeAttackItem == FileManager.HARPOON)
                    {
                        //Use the harpoon items grappling ability
                        Harpoon.UseGrapplingAbility(curPlayer, nonCurPlayer);
                    }

                    //Decrease the opposing players health
                    nonCurPlayer.curHealth -= healthLost;

                    //Activate the item timer
                    itemTimer.Activate();
                    itemSnd.CreateInstance().Play();

                    //Add the items dealt data to the file manager
                    itemFileManager.AddTotalHealthPoints(longRangeAttackItem, healthLost);
                }
                else
                {
                    //Activate the guardian shields sound effect when hit
                    GuardianShield.guardianShieldAccumulationSnd.CreateInstance().Play();
                }
            }

            //Add the items use data to the file manager
            itemFileManager.AddTotalItemUse(longRangeAttackItem);
        }

        //Pre: Used to draw various sprites, and the opponent player
        //Post: None
        //Desc: Display the potential long range tiles
        public override void DisplayPotentialSquares(SpriteBatch spriteBatch, Player opponentPlayer)
        {
            //Iterate through each possible long range tile
            for (int i = 0; i < potentialSquareRecs.Length; i++)
            {
                //Draw the potential long range tiles
                spriteBatch.Draw(squareImg, potentialSquareRecs[i], Color.White);
            }
        }

        //Pre: Used to draw various sprites, the current item font, and both the players
        //Post: None
        //Desc: Determine where the item number should display based off of the long range item
        public override void DisplayItemNumber(SpriteBatch spriteBatch, SpriteFont font, Player curPlayer, Player nonCurPLayer)
        {
            //Draw the long range item number overtop the opposing player
            Effects.OverlapTextDisplay(spriteBatch, font, itemNumberText, GetItemNumberLoc(nonCurPLayer), Color.White, Color.Magenta);
        }
    }
}