// Author: Noah Teitlebaum
// File Name: Utility.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the utility items

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
    class Utility : Item
    {
        //Store the potential squares for the movement utility items
        protected const int TOP_LEFT = 0;
        protected const int TOP_RIGHT = 1;
        protected const int BOTTOM_LEFT = 2;
        protected const int BOTTOM_RIGHT = 3;

        //Store the current utility item and its images
        protected List<Texture2D> squareImgs = new List<Texture2D>();
        protected int utilityItem;

        /// <summary>
        /// A certain utility item used by players during game play
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public Utility(ContentManager Content) : base(Content)
        {
        }

        //Pre: The players and the item timer
        //Post: None
        //Desc: Use the utility item
        public override void UseItem(Player curPlayer, Player nonCurPlayer, Timer itemTimer)
        {
            //Check which movement key the user clicked and see if its a valid move
            if (Game1.kb.IsKeyDown(Keys.D1) && !Game1.prevKb.IsKeyDown(Keys.D1) && IsAvailableSpot(curPlayer, nonCurPlayer, TOP_LEFT))
            {
                //Move the player to the top left square
                UpdateItem(curPlayer, TOP_LEFT);
            }
            else if (Game1.kb.IsKeyDown(Keys.D2) && !Game1.prevKb.IsKeyDown(Keys.D2) && IsAvailableSpot(curPlayer, nonCurPlayer, TOP_RIGHT))
            {
                //Move the player to the top right square
                UpdateItem(curPlayer, TOP_RIGHT);
            }
            else if (Game1.kb.IsKeyDown(Keys.D3) && !Game1.prevKb.IsKeyDown(Keys.D3) && IsAvailableSpot(curPlayer, nonCurPlayer, BOTTOM_LEFT))
            {
                //Move the player to the bottom left square
                UpdateItem(curPlayer, BOTTOM_LEFT);
            }
            else if (Game1.kb.IsKeyDown(Keys.D4) && !Game1.prevKb.IsKeyDown(Keys.D4) && IsAvailableSpot(curPlayer, nonCurPlayer, BOTTOM_RIGHT))
            {
                //Move the player to the bottom right square
                UpdateItem(curPlayer, BOTTOM_RIGHT);
            }
        }

        //Pre: The current player moving and the spot they are moving towars
        //Post: None
        //Desc: Update the current player after moving
        private void UpdateItem(Player curPlayer, int curSpot)
        {
            //Set the players location based off the current spot the moved towards
            curPlayer.hoverTileRec = potentialSquareRecs[curSpot];
            curPlayer.PlayerRectangleAlignment();

            //Use the players cool down based off the movement utility item
            curPlayer.curCoolDown -= GetCoolDown();

            //Add the items use data to the file manager
            itemFileManager.AddTotalItemUse(utilityItem);

            //Use any overtime items and switch turns
            PlayState.UseOverTimeItems();
            PlayState.Switch();
            itemSnd.CreateInstance().Play();
        }

        //Pre: Used to draw various sprites, and the opponent player
        //Post: None
        //Desc: Display the potential movement utility tiles
        public override void DisplayPotentialSquares(SpriteBatch spriteBatch, Player opponentPlayer)
        {
            //Iterate through each possible movement tile
            for (int i = 0; i < potentialSquareRecs.Length; i++)
            {
                //Determine if any possible square overlaps with the opponents position
                if (potentialSquareRecs[i] != opponentPlayer.hoverTileRec)
                {
                    //Draw the potential movement tiles
                    spriteBatch.Draw(squareImgs[i], potentialSquareRecs[i], Color.White);
                }
            }
        }

        //Pre: Both the players and the current movement tile spot
        //Post: Recieve if the current movement spot is a valid location as a bool
        //Desc: Check if the current movement spot is available
        private bool IsAvailableSpot(Player curPlayer, Player nonCurPlayer, int curSpot)
        {
            //Determine if the movement spot location is in bounds, and does not overlap with any player
            if (potentialSquareRecs[curSpot] != curPlayer.hoverTileRec && potentialSquareRecs[curSpot] != nonCurPlayer.hoverTileRec && Grid.IsInBounds(potentialSquareRecs[curSpot]))
            {
                //The movement spot is available
                return true;
            }

            //The movement spot is not available
            return false;
        }
    }
}