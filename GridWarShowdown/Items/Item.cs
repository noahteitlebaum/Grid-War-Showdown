// Author: Noah Teitlebaum
// File Name: Item.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the items

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
    class Item
    {
        //Store the potential squares for movement
        private const int UP = 0;
        private const int DOWN = 1;
        private const int RIGHT = 2;
        private const int LEFT = 3;

        //Store the item properties unique to each item
        protected Texture2D squareImg;
        protected int coolDown;
        protected string itemNumberText;
        protected SoundEffect itemSnd;

        //Store the potential squares where the item can be activated
        protected Vector2[] potentialSquarePoss;
        protected Rectangle[] potentialSquareRecs;

        //Store the file manager for measuring certain data about the items
        protected static FileManager itemFileManager = new FileManager(9, 12);

        /// <summary>
        /// A certain move used by players during game play
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public Item(ContentManager Content)
        {
            //Load the movement properties
            potentialSquarePoss = new Vector2[4];
            squareImg = Content.Load<Texture2D>("Images/Sprites/MovementSquare");
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/MovementSound");
        }

        //Pre: None
        //Post: Receiving the file manager used to keep track of item data
        //Desc: Obtaining the file manager for data tracking
        public FileManager GetFileManager()
        {
            //Return the file manager used to keep track of item data
            return itemFileManager;
        }

        //Pre: The current player
        //Post: Receiving the location of where the item number should be displayed as a Vector2
        //Desc: Finding the location of where the item number should be displayed depending on what item was used
        protected Vector2 GetItemNumberLoc(Player curPlayer)
        {
            //Return the location of where the item number should be displayed as a Vector2
            return new Vector2(curPlayer.hoverTileRec.X + 5, curPlayer.hoverTileRec.Y + 5);
        }

        //Pre: The non current player being attacked
        //Post: Receiving if the non current player was hit as a bool
        //Desc: Checking if the current player hit the non current player
        protected bool IsPlayerHit(Player nonCurPlayer)
        {
            //Iterate through each potential square bounding box
            for (int i = 0; i < potentialSquareRecs.Length; i++)
            {
                //Check if the attacked player is inside any of the bounding boxes
                if (nonCurPlayer.hoverTileRec == potentialSquareRecs[i])
                {
                    //The attacked player was hit
                    return true;
                }
            }

            //The attacked player was not hit
            return false;
        }

        //Pre: The current player
        //Post: Receiving if the item is available for the current player as a bool
        //Desc: Checking if the current player has enough cool down for the item
        public bool IsItemAvailable(Player player)
        {
            //Check if the item cool down is not greater than the players cool down
            if (coolDown <= player.curCoolDown)
            {
                //The item is available
                return true;
            }

            //The item is not available
            return false;
        }

        //Pre: None
        //Post: Receiving the cool down of the item as an int
        //Desc: Finding the current cool down of the item
        public int GetCoolDown()
        {
            //Return the cool down of the item
            return coolDown;
        }

        //Pre: None
        //Post: None
        //Desc: Converting all the potential square locations into bounding boxes
        protected void SetBoundingBoxes()
        {
            //Load the bounding box length dependent on how many possible locations there are
            potentialSquareRecs = new Rectangle[potentialSquarePoss.Length];

            //Iterate through each possible location tile of the item
            for (int i = 0; i < potentialSquarePoss.Length; i++)
            {
                //Set the items bounding boxes dependent on how many possible locations there are
                potentialSquareRecs[i] = new Rectangle(Grid.GetXLoc((int)potentialSquarePoss[i].X), Grid.GetYLoc((int)potentialSquarePoss[i].Y), squareImg.Width, squareImg.Height);
            }
        }

        //Pre: The current player
        //Post: None
        //Desc: Set the potential squares of the movement item relative to the players position
        public virtual void SetPotentialSquares(Player curPlayer)
        {
            //Set the potential movement positions
            potentialSquarePoss[UP] = new Vector2(curPlayer.curRow, curPlayer.curCol - 1);
            potentialSquarePoss[DOWN] = new Vector2(curPlayer.curRow, curPlayer.curCol + 1);
            potentialSquarePoss[LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol);
            potentialSquarePoss[RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol);

            //Convert all the potential square locations into bounding boxes
            SetBoundingBoxes();
        }

        //Pre: The players and the item timer
        //Post: None
        //Desc: Use the movement item
        public virtual void UseItem(Player curPlayer, Player nonCurPlayer, Timer itemTimer)
        {
            //Check which movement key the user clicked and see if its a valid move
            if (Game1.kb.IsKeyDown(Keys.W) && !Game1.prevKb.IsKeyDown(Keys.W) && Grid.IsInBoundsUp(curPlayer.curCol) && Player.IsNotCollidingUp(curPlayer, nonCurPlayer))
            {
                //Move the player up
                curPlayer.MoveUp();
                UpdateMovementItem(curPlayer);
            }
            else if (Game1.kb.IsKeyDown(Keys.S) && !Game1.prevKb.IsKeyDown(Keys.S) && Grid.IsInBoundsDown(curPlayer.curCol) && Player.IsNotCollidingDown(curPlayer, nonCurPlayer))
            {
                //Move the player down
                curPlayer.MoveDown();
                UpdateMovementItem(curPlayer);
            }
            else if (Game1.kb.IsKeyDown(Keys.A) && !Game1.prevKb.IsKeyDown(Keys.A) && Grid.IsInBoundsLeft(curPlayer.curRow) && Player.IsNotCollidingLeft(curPlayer, nonCurPlayer))
            {
                //Move the player left
                curPlayer.MoveLeft();
                UpdateMovementItem(curPlayer);
            }
            else if (Game1.kb.IsKeyDown(Keys.D) && !Game1.prevKb.IsKeyDown(Keys.D) && Grid.IsInBoundsRight(curPlayer.curRow) && Player.IsNotCollidingRight(curPlayer, nonCurPlayer))
            {
                //Move the player right
                curPlayer.MoveRight();
                UpdateMovementItem(curPlayer);
            }
        }

        //Pre: The current player moving
        //Post: None
        //Desc: Update the current player after moving
        private void UpdateMovementItem(Player curPlayer)
        {
            //Increment the current players cool down
            curPlayer.curCoolDown = Math.Min(curPlayer.curCoolDown + 1, Player.MAX_COOL_DOWN);

            //Use any overtime items and switch turns
            PlayState.UseOverTimeItems();
            PlayState.Switch();
            itemSnd.CreateInstance().Play();
        }

        //Pre: Used to draw various sprites, and the opponent player
        //Post: None
        //Desc: Display the potential movement tiles
        public virtual void DisplayPotentialSquares(SpriteBatch spriteBatch, Player opponentPlayer)
        {
            //Iterate through each possible movement tile
            for (int i = 0; i < potentialSquareRecs.Length; i++)
            {
                //Determine if any possible square overlaps with the opponents position
                if (potentialSquareRecs[i] != opponentPlayer.hoverTileRec)
                {
                    //Draw the potential movement tiles
                    spriteBatch.Draw(squareImg, potentialSquareRecs[i], Color.White);
                }
            }
        }

        //Pre: Used to draw various sprites, the current item font, and both the players
        //Post: None
        //Desc: Determine where the item number should display based off of the current type of item
        public virtual void DisplayItemNumber(SpriteBatch spriteBatch, SpriteFont font, Player curPlayer, Player nonCurPlayer)
        {
        }
    }
}