// Author: Noah Teitlebaum
// File Name: Menu.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the menu state

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
    class Menu
    {
        //Store the UI Spacing
        private const int OPTION_SPACER = 115;
        private const int ARROW_HORIZ = 200;
        private const int ARROW_VERT = 80;
        private const int INST_VERT = 675;

        //Store the possible game options
        private const int PLAY = 0;
        private const int INSTRUCTIONS = 1;
        private const int ITEM_MANUAL = 2;
        private const int STATISTICS = 3;
        private const int EXIT = 4;

        //Store the current game option
        private int gameOption = PLAY;

        //Store the UI sound effect
        public static SoundEffect UISnd;

        //Store the background image
        private Texture2D bgImg;

        //Store the arrow properties
        private Texture2D arrowImg;
        private Rectangle arrowRec;
        private float arrowScalar = 1.3f;

        //Store the fonts
        private SpriteFont titleFont;
        private SpriteFont optionFont;
        private SpriteFont instFont;

        //Store the text locations
        private Vector2 titleLoc;
        private Vector2[] optionLocs = new Vector2[5];
        private Vector2 instLoc;

        //Store the text
        private string titleText = "GRIDWAR SHOWDOWN!";
        private string[] optionTexts = new string[] { "FIGHT!", "INSTRUCTIONS", "ITEM MANUAL", "STATISTICS", "EXIT" };
        private string instText = "*Use |UP| And |DOWN| Arrow Keys. Press |ENTER| To Continue*";

        //Pre: Used to load audio, fonts, and images 
        //Post: None
        //Desc: Load the menu state
        public void LoadContent(ContentManager Content)
        {
            //Load the UI sound effect
            UISnd = Content.Load<SoundEffect>("Audio/Sounds/UISound");

            //Load the background image
            bgImg = Content.Load<Texture2D>("Images/Backgrounds/MenuBackground");

            //Load the arrow properties
            arrowImg = Content.Load<Texture2D>("Images/Sprites/BarbarianArrow");
            arrowRec = new Rectangle(ARROW_HORIZ, ARROW_VERT, (int)(arrowScalar * arrowImg.Width), (int)(arrowScalar * arrowImg.Height));

            //Load the fonts
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");
            optionFont = Content.Load<SpriteFont>("Fonts/OptionFont");
            instFont = Content.Load<SpriteFont>("Fonts/ItemFont");

            //Load the title location
            titleLoc = new Vector2((Game1.screenWidth - titleFont.MeasureString(titleText).X) / 2, 10);

            //Iterate through each game option
            for (int i = 0; i < optionLocs.Length; i++)
            {
                //Load the possible game option locations
                optionLocs[i] = new Vector2(Game1.screenWidth / 2 - OPTION_SPACER, OPTION_SPACER + (OPTION_SPACER * i)); 
            }

            //Load the instruction location
            instLoc = new Vector2((Game1.screenWidth - instFont.MeasureString(instText).X) / 2, INST_VERT);
        }

        //Pre: None
        //Post: None
        //Desc: Update the menu state
        public void Update()
        {
            //Check to see if the user pressed the down or up arrow key
            if (Game1.kb.IsKeyDown(Keys.Down) && !Game1.prevKb.IsKeyDown(Keys.Down) && gameOption != EXIT)
            {
                //Go to the next possible game option
                arrowRec.Y += OPTION_SPACER;
                gameOption++;
            }
            else if (Game1.kb.IsKeyDown(Keys.Up) && !Game1.prevKb.IsKeyDown(Keys.Up) && gameOption != PLAY)
            {
                //Go to the previous possible game option
                arrowRec.Y -= OPTION_SPACER;
                gameOption--;
            }

            //Check to see if the user pressed the enter key
            if (Game1.kb.IsKeyDown(Keys.Enter) && !Game1.prevKb.IsKeyDown(Keys.Enter))
            {
                //Set the previous game state
                Game1.prevGameState = Game1.gameState;
                UISnd.CreateInstance().Play();

                //Set the appropriate game option based on user’s choice
                switch (gameOption)
                {
                    case PLAY:
                        //Set the game state to the pre game
                        Game1.gameState = Game1.PRE_GAME_STATE;
                        break;
                    case INSTRUCTIONS:
                        //Set the game state to the instructions
                        Game1.gameState = Game1.INSTRUCTIONS_STATE;
                        break;
                    case ITEM_MANUAL:
                        //Set the game state to the item manual
                        Game1.gameState = Game1.ITEM_MANUAL_STATE;
                        break;
                    case STATISTICS:
                        //Set the game state to the statistics
                        Game1.gameState = Game1.STATISTICS_STATE;
                        break;
                    case EXIT:
                        //Exit the game
                        Game1.gameState = -1;
                        break;
                }
            }

        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Draw the menu state
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the scolling menu background
            Effects.DrawParallaxBg(spriteBatch, bgImg);

            //Draw the arrow images
            spriteBatch.Draw(arrowImg, arrowRec, Color.White);

            //Draw the title text
            Effects.OverlapTextDisplay(spriteBatch, titleFont, titleText, titleLoc, Color.Black, Color.DarkOrange);

            //Iterate through each game option
            for (int i = 0; i < optionTexts.Length; i++)
            {
                //Draw the possible game option texts
                Effects.OverlapTextDisplay(spriteBatch, optionFont, optionTexts[i], optionLocs[i], Color.Goldenrod, Color.FloralWhite);
            }

            //Draw the instruction text
            Effects.OverlapTextDisplay(spriteBatch, instFont, instText, instLoc, Color.Black, Color.Yellow);
        }
    }
}