// Author: Noah Teitlebaum
// File Name: Statistics.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the statistics state

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
    class StatisticsState
    {
        //Store the UI Spacing
        private const int COLLUMN_LENGTH = 6;
        private const int COLLUMN_HORIZ1 = 190;
        private const int COLLUMN_HORIZ2 = 355;
        private const int COLLUMN_HORIZ_SPACER = 500;
        private const int COLLUMN_STARTING_VERT = 230;
        private const int COLLUMN_VERT_SPACER = 90;

        //Store the player values
        private const int PLAYER1 = 0;
        private const int PLAYER2 = 1;

        //Store the background properties
        private Texture2D bgImg;
        private Rectangle bgRec;

        //Store the instruction properties
        private SpriteFont instFont;
        private Vector2 instLoc;
        private string instText = "*PRESS |ESC| TO GO BACK*";

        //Store the subtitles font
        private SpriteFont subTitleFont;

        //Store the player subtitle properties
        private Vector2[] playerWinLocs = new Vector2[2];
        private string[] playerWinTexts = new string[] { "Player 1 Wins:", "Player 2 Wins:" };

        //Store the item health and use collumns
        private List<Vector2> itemHealthLocs = new List<Vector2>();
        private List<Vector2> itemUseLocs = new List<Vector2>();

        //Store the file manager properties
        private FileManager fileManager = new FileManager(2, 9, 12);
        private bool readFileOnce = true;

        //Pre: Used to load audio, fonts, and images 
        //Post: None
        //Desc: Load the statistics state
        public void LoadContent(ContentManager Content)
        {
            //Load the background properties
            bgImg = Content.Load<Texture2D>("Images/Backgrounds/StatisticsBackground");
            bgRec = new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight);

            //Load the instruction properties
            instFont = Content.Load<SpriteFont>("Fonts/ItemFont");
            instLoc = new Vector2((Game1.screenWidth - instFont.MeasureString(instText).X) / 2, 10);

            //Load the subtitle font
            subTitleFont = Content.Load<SpriteFont>("Fonts/SubTitleFont");

            //Iterate through each player
            for (int i = 0; i < playerWinLocs.Length; i++)
            {
                //Load the player win locations
                playerWinLocs[i] = new Vector2(30 + (i * 550), 70);
            }

            //Iterate through each collumn
            for (int i = 0; i < COLLUMN_LENGTH; i++)
            {
                //Load the item health and use locations for the first collumn
                itemHealthLocs.Add(new Vector2(COLLUMN_HORIZ1, COLLUMN_STARTING_VERT + (i * COLLUMN_VERT_SPACER)));
                itemUseLocs.Add(new Vector2(COLLUMN_HORIZ2, COLLUMN_STARTING_VERT + (i * COLLUMN_VERT_SPACER)));
            }
            for (int i = 0; i < COLLUMN_LENGTH; i++)
            {
                //Load the item health and use locations for the second collumn
                itemHealthLocs.Add(new Vector2(COLLUMN_HORIZ1 + COLLUMN_HORIZ_SPACER, COLLUMN_STARTING_VERT + (i * COLLUMN_VERT_SPACER)));
                itemUseLocs.Add(new Vector2(COLLUMN_HORIZ2 + COLLUMN_HORIZ_SPACER, COLLUMN_STARTING_VERT + (i * COLLUMN_VERT_SPACER)));
            }
        }

        //Pre: None
        //Post: None
        //Desc: Update the statistics state
        public void Update()
        {
            //Check to see if the file has not been read yet
            if (readFileOnce)
            {
                //Read the statistics file
                fileManager.ReadFile(true, true);
                readFileOnce = false;
            }

            //Check to see if the user has pressed the escape key
            if (Game1.kb.IsKeyDown(Keys.Escape) && !Game1.prevKb.IsKeyDown(Keys.Escape))
            {
                //Go back to the previous game state
                Game1.gameState = Game1.prevGameState;
                readFileOnce = true;
            }
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Draw the statistics state
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the error background
            spriteBatch.Draw(bgImg, bgRec, Color.White);

            //Check to see if an error with the file occured
            if (fileManager.errorOccurred)
            {
                //Set the error text and location
                string errorText = "-An Error With The File Occurred-";
                Vector2 errorLoc = new Vector2((Game1.screenWidth - subTitleFont.MeasureString(errorText).X) / 2, playerWinLocs[PLAYER1].Y);

                //Display the error text message
                Effects.OverlapTextDisplay(spriteBatch, subTitleFont, errorText, errorLoc, Color.Black, Color.Orange);
            }
            else
            {
                //Draw the player wins
                Effects.OverlapTextDisplay(spriteBatch, subTitleFont, playerWinTexts[PLAYER1] + " " + fileManager.playerWins[PLAYER1], playerWinLocs[PLAYER1], Color.Black, Color.Crimson);
                Effects.OverlapTextDisplay(spriteBatch, subTitleFont, playerWinTexts[PLAYER2] + " " + fileManager.playerWins[PLAYER2], playerWinLocs[PLAYER2], Color.Black, Color.DodgerBlue);
            }

            //Draw the instructions text
            Effects.OverlapTextDisplay(spriteBatch, instFont, instText, instLoc, Color.Black, Color.Yellow);

            //Iterate through each item health statistic
            for (int i = 0; i < itemHealthLocs.Count; i++)
            {
                //Check to see if the item health points goes out of range
                try
                {
                    //Draw the item health statistics
                    Effects.OverlapTextDisplay(spriteBatch, instFont, "" + fileManager.totalHealthPoints[i], itemHealthLocs[i], Color.Black, Color.Yellow);
                }
                catch (IndexOutOfRangeException)
                {
                    //Draw not applicable
                    Effects.OverlapTextDisplay(spriteBatch, instFont, "N / A", itemHealthLocs[i], Color.Black, Color.Yellow);
                }
            }

            //Iterate through each item use statistic
            for (int i = 0; i < itemUseLocs.Count; i++)
            {
                //Draw the item use statistics
                Effects.OverlapTextDisplay(spriteBatch, instFont, "" + fileManager.totalItemUse[i], itemUseLocs[i], Color.Black, Color.Yellow);
            }
        }
    }
}