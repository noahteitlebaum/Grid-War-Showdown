// Author: Noah Teitlebaum
// File Name: Player.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the player objects

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
    class Player
    {
        //Store the player values
        private const int PLAYER1 = 0;
        private const int PLAYER2 = 1;

        //Store the player health values
        public const int MAX_HEALTH = 100;
        public const int DEAD = 0;

        //Store the max amount of cool down
        public const int MAX_COOL_DOWN = 10;

        //Store the hover tile image properties
        private Texture2D hoverTileimg;
        public Rectangle hoverTileRec;

        //Store the player image properties
        private Texture2D img;
        private Rectangle rec;
        private float scalar;

        //Store the current row and collumn of the player
        public int curRow;
        public int curCol;

        //Store the current health and cool down of the player
        public int curHealth;
        public int curCoolDown;

        //Store the over time item values
        public bool isShieldPotionHeal;
        public bool isTorchDamage;
        public bool isGuardianShieldActive;

        //Store the over time item counters
        public int shieldPotionCounter;
        public int torchDamageCounter;
        public int guardianShieldCounter;

        /// <summary>
        /// Characters used when playing the game
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images </param>
        /// <param name="player">The current player</param>
        public Player(ContentManager Content, int player)
        {
            //Load the hover tile image
            hoverTileimg = Content.Load<Texture2D>("Images/Sprites/PlayerHoverSquare");

            //Check to see which player to use
            if (player == PLAYER1)
            {
                //Load the player image to player one
                img = Content.Load<Texture2D>("Images/Sprites/PlayerOne");
            }
            else if (player == PLAYER2)
            {
                //Load the player image to player two
                img = Content.Load<Texture2D>("Images/Sprites/PlayerTwo");
            }

            //Load the player scalar
            scalar = 0.7f;

            //Set the player properties before the round starts
            SetForPlay(player);
        }

        //Pre: The current player
        //Post: None
        //Desc: Set the player properties before the round starts
        public void SetForPlay(int player)
        {
            //Check to see which player to use
            if (player == PLAYER1)
            {
                //Set the player to the top left corner
                curRow = Grid.MIN;
                curCol = Grid.MIN;
            }
            else if (player == PLAYER2)
            {
                //Set the player to the bottom right corner
                curRow = Grid.MAX;
                curCol = Grid.MAX;
            }

            //Set the players bounding box and the hover tiles bounding box
            hoverTileRec = new Rectangle(Grid.GetXLoc(curRow), Grid.GetYLoc(curCol), hoverTileimg.Width, hoverTileimg.Height);
            rec = new Rectangle(hoverTileRec.Left, hoverTileRec.Top - hoverTileRec.Height / 2, (int)(scalar * img.Width), (int)(scalar * img.Height));

            //Reset the current health and cool down of the player
            curHealth = MAX_HEALTH;
            curCoolDown = MAX_COOL_DOWN;

            //Reset the over time item values
            isShieldPotionHeal = false;
            isTorchDamage = false;
            isGuardianShieldActive = false;

            //Reset the over time item counters
            shieldPotionCounter = 0;
            torchDamageCounter = 0;
            guardianShieldCounter = 0;
        }

        //Pre: None
        //Post: None
        //Desc: Align the player to the hover tile
        public void PlayerRectangleAlignment()
        {
            //Align the players bounding box to the hover tiles bounding box
            rec = new Rectangle(hoverTileRec.Left, hoverTileRec.Top - hoverTileRec.Height / 2, (int)(scalar * img.Width), (int)(scalar * img.Height));
            SetCurrentPos();
        }

        //Pre: None
        //Post: None
        //Desc: Move the player up one tile
        public void MoveUp()
        {
            //Move the players bounding box and hover tiles bounding box up one tile
            rec.Y -= Grid.TILE_SPACER;
            hoverTileRec.Y -= Grid.TILE_SPACER;
            SetCurrentPos();
        }

        //Pre: None
        //Post: None
        //Desc: Move the player down one tile
        public void MoveDown()
        {
            //Move the players bounding box and hover tiles bounding box down one tile
            rec.Y += Grid.TILE_SPACER;
            hoverTileRec.Y += Grid.TILE_SPACER;
            SetCurrentPos();
        }

        //Pre: None
        //Post: None
        //Desc: Move the player right one tile
        public void MoveRight()
        {
            //Move the players bounding box and hover tiles bounding box right one tile
            rec.X += Grid.TILE_SPACER;
            hoverTileRec.X += Grid.TILE_SPACER;
            SetCurrentPos();
        }

        //Pre: None
        //Post: None
        //Desc: Move the player left one tile
        public void MoveLeft()
        {
            //Move the players bounding box and hover tiles bounding box left one tile
            rec.X -= Grid.TILE_SPACER;
            hoverTileRec.X -= Grid.TILE_SPACER;
            SetCurrentPos();
        }

        //Pre: None
        //Post: None
        //Desc: Search for where the player is on the grid
        private void SetCurrentPos()
        {
            //Search for the players row and collumn on the grid
            curRow = Grid.BinarySearchRow(hoverTileRec, Grid.MIN, Grid.MAX);
            curCol = Grid.BinarySearchCollumn(hoverTileRec, Grid.MIN, Grid.MAX);
        }

        //Pre: The current and non current player
        //Post: Receiving if the two players are not colliding if the current player moves up as a bool
        //Desc: Check if the two players are not colliding if the current player moves upwards
        public static bool IsNotCollidingUp(Player curPlayer, Player nonCurPlayer)
        {
            //Return if the two players are not colliding if the current player moves up
            return curPlayer.curRow != nonCurPlayer.curRow || curPlayer.curCol - 1 != nonCurPlayer.curCol;
        }

        //Pre: The current and non current player
        //Post: Receiving if the two players are not colliding if the current player moves down as a bool
        //Desc: Check if the two players are not colliding if the current player moves downwards
        public static bool IsNotCollidingDown(Player curPlayer, Player nonCurPlayer)
        {
            //Return if the two players are not colliding if the current player moves down
            return curPlayer.curRow != nonCurPlayer.curRow || curPlayer.curCol + 1 != nonCurPlayer.curCol;
        }

        //Pre: The current and non current player
        //Post: Receiving if the two players are not colliding if the current player moves left as a bool
        //Desc: Check if the two players are not colliding if the current player moves left
        public static bool IsNotCollidingLeft(Player curPlayer, Player nonCurPlayer)
        {
            //Return if the two players are not colliding if the current player moves left
            return curPlayer.curCol != nonCurPlayer.curCol || curPlayer.curRow - 1 != nonCurPlayer.curRow;
        }

        //Pre: The current and non current player
        //Post: Receiving if the two players are not colliding if the current player moves right as a bool
        //Desc: Check if the two players are not colliding if the current player moves right
        public static bool IsNotCollidingRight(Player curPlayer, Player nonCurPlayer)
        {
            //Return if the two players are not colliding if the current player moves right
            return curPlayer.curCol != nonCurPlayer.curCol || curPlayer.curRow + 1 != nonCurPlayer.curRow;
        }

        //Pre: None
        //Post: Receiving the current player color hue
        //Desc: Check which color hue the player should be highlighted as
        private Color PlayerColor()
        {
            //Check which overtime item combination is valid
            if (!isTorchDamage && !isShieldPotionHeal && !isGuardianShieldActive)
            {
                //Return the color white
                return Color.White;
            }
            else if (isTorchDamage && !isShieldPotionHeal && !isGuardianShieldActive)
            {
                //Return the color orange
                return Color.Orange;
            }
            else if (!isTorchDamage && isShieldPotionHeal && !isGuardianShieldActive)
            {
                //Return the color light green
                return Color.LightGreen;
            }
            else if (!isTorchDamage && !isShieldPotionHeal && isGuardianShieldActive)
            {
                //Return the color brown
                return Color.SaddleBrown;
            }

            //Return the color yellow
            return Color.Yellow;
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Draw the current player image
        public void Display(SpriteBatch spriteBatch)
        {
            //Draw the current player image
            spriteBatch.Draw(img, rec, PlayerColor());
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Draw the current hover tile
        public void DisplayCurrentTile(SpriteBatch spriteBatch)
        {
            //Draw the current hover tile
            spriteBatch.Draw(hoverTileimg, hoverTileRec, Color.White);
        }
    }
}