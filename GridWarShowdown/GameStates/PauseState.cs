// Author: Noah Teitlebaum
// File Name: Pause.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the pause state

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
    class PauseState
    {
        //Store the UI Spacing
        private const int MAX_DEATH_LOC = 130;
        private const int GAMESTATE_VERT = 390;
        private const int INST_HORIZ = 200;
        private const int ITEMS_HORIZ = 750;

        //Store the player values
        private const int OFF = 0;
        private const int ON = 1;

        //Store the unpause sound effect
        private SoundEffect unpauseSnd;

        //Store the background properties
        private Texture2D bgImg;
        private Rectangle bgRec;

        //Store the max death text, location, and font
        private SpriteFont maxDeathFont;
        private Vector2 maxDeathLoc;
        private string maxDeathsText = "MAX DEATHS: ";

        //Store the pause button images
        private Texture2D[] pauseButtonImgs = new Texture2D[2];

        //Store the various buttons
        private Button pauseBar;
        private Button instructionState;
        private Button itemManualState;

        //Pre: Used to load audio, fonts, and images
        //Post: None
        //Desc: Load the pause state
        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            //Load the unpause sound effect
            unpauseSnd = Content.Load<SoundEffect>("Audio/Sounds/UnpauseSound");

            //Load the background properties
            bgImg = Content.Load<Texture2D>("Images/Backgrounds/PausedBackground");
            bgRec = new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight);

            //Load max death location, and font
            maxDeathFont = Content.Load<SpriteFont>("Fonts/OptionFont");
            maxDeathLoc = new Vector2((Game1.screenWidth - maxDeathFont.MeasureString(maxDeathsText + PreGameState.maxDeaths).X) / 2, MAX_DEATH_LOC);

            //Load the pause button images
            pauseButtonImgs[OFF] = Content.Load<Texture2D>("Images/Sprites/PauseBarOff");
            pauseButtonImgs[ON] = Content.Load<Texture2D>("Images/Sprites/PauseBarOn");

            //Load the various buttons
            pauseBar = new Button(pauseButtonImgs[OFF], pauseButtonImgs[ON], 1f, new Vector2((Game1.screenWidth - pauseButtonImgs[OFF].Width) / 2, 10), 1.4);
            instructionState = new Button(graphicsDevice, new Vector2(INST_HORIZ, GAMESTATE_VERT), maxDeathFont, "INSTS.");
            itemManualState = new Button(graphicsDevice, new Vector2(ITEMS_HORIZ, GAMESTATE_VERT), maxDeathFont, "ITEM'S");
        }

        //Pre: None
        //Post: None
        //Desc: Update the pause state
        public void Update()
        {
            //Check to see if the user clicked the left mouse button
            if (Game1.mouse.LeftButton == ButtonState.Pressed && Game1.prevMouse.LeftButton != ButtonState.Pressed)
            {
                //Set the previous game state
                Game1.prevGameState = Game1.gameState;

                //Check to see if any of the buttons are being hovered over
                if (pauseBar.HoverStatus())
                {
                    //Unpause the game
                    Game1.gameState = Game1.PLAY_STATE;
                    unpauseSnd.CreateInstance().Play();
                }
                else if (instructionState.HoverStatus())
                {
                    //Enter the instructions state
                    Game1.gameState = Game1.INSTRUCTIONS_STATE;
                    Menu.UISnd.CreateInstance().Play();
                }
                else if (itemManualState.HoverStatus())
                {
                    //Enter the item manual state
                    Game1.gameState = Game1.ITEM_MANUAL_STATE;
                    Menu.UISnd.CreateInstance().Play();
                }
            }
        }

        //Pre: Used to draw various sprites, the paused playstate
        //Post: None
        //Desc: Draw the menu state
        public void Draw(SpriteBatch spriteBatch, PlayState playState)
        {
            //Draw the paused play state with a grey background over top
            playState.Draw(spriteBatch);
            spriteBatch.Draw(bgImg, bgRec, Color.Black * 0.8f);

            //Draw the max death text
            Effects.OverlapTextDisplay(spriteBatch, maxDeathFont, maxDeathsText + PreGameState.maxDeaths, maxDeathLoc, Color.White, Color.Chocolate);

            //Draw the various buttons
            pauseBar.DisplayButton(spriteBatch);
            instructionState.DisplayStringButton(spriteBatch);
            itemManualState.DisplayStringButton(spriteBatch);
        }
    }
}