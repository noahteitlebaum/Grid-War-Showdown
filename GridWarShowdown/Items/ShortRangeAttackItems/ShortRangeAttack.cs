// Author: Noah Teitlebaum
// File Name: ShortRangeAttack.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the short range attack items

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
    class ShortRangeAttack : Item
    {
        //Store the current short range item and how much health was dealt
        protected int shortRangeAttackItem;
        protected int healthLost;

        /// <summary>
        /// A certain short range attack item used by players during game play
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public ShortRangeAttack(ContentManager Content) : base(Content)
        {
            squareImg = Content.Load<Texture2D>("Images/Sprites/ShortRangeSquare");
        }

        //Pre: The players and the item timer
        //Post: None
        //Desc: Use the short range item
        public override void UseItem(Player curPlayer, Player nonCurPlayer, Timer itemTimer)
        {
            //Check if the player was hit
            if (IsPlayerHit(nonCurPlayer))
            {
                //Check if the guardian shield effect is not active
                if (!nonCurPlayer.isGuardianShieldActive)
                {
                    //Check if the long rang item is the torch or dynamite
                    if (shortRangeAttackItem == FileManager.TORCH)
                    {
                        //Active the torch overtime effect
                        nonCurPlayer.isTorchDamage = true;
                    }
                    else if (shortRangeAttackItem == FileManager.DYNAMITE)
                    {
                        //Use the dynamite items flinging ability
                        Dynamite.UseFlingingAbility(curPlayer, nonCurPlayer);
                    }

                    //Decrease the opposing players health
                    nonCurPlayer.curHealth -= healthLost;

                    //Activate the item timer
                    itemTimer.Activate();
                    itemSnd.CreateInstance().Play();

                    //Add the items dealt data to the file manager
                    itemFileManager.AddTotalHealthPoints(shortRangeAttackItem, healthLost);
                }
                else
                {
                    //Activate the guardian shields sound effect when hit
                    GuardianShield.guardianShieldAccumulationSnd.CreateInstance().Play();
                }
            }

            //Add the items use data to the file manager
            itemFileManager.AddTotalItemUse(shortRangeAttackItem);
        }

        //Pre: Used to draw various sprites, and the opponent player
        //Post: None
        //Desc: Display the potential short range tiles
        public override void DisplayPotentialSquares(SpriteBatch spriteBatch, Player opponentPlayer)
        {
            //Iterate through each possible short range tile
            for (int i = 0; i < potentialSquareRecs.Length; i++)
            {
                //Draw the potential short range tiles
                spriteBatch.Draw(squareImg, potentialSquareRecs[i], Color.White);
            }
        }

        //Pre: Used to draw various sprites, the current item font, and both the players
        //Post: None
        //Desc: Determine where the item number should display based off of the short range item
        public override void DisplayItemNumber(SpriteBatch spriteBatch, SpriteFont font, Player curPlayer, Player nonCurPLayer)
        {
            //Draw the short range item number overtop the opposing player
            Effects.OverlapTextDisplay(spriteBatch, font, itemNumberText, GetItemNumberLoc(nonCurPLayer), Color.White, Color.Crimson);
        }
    }
}