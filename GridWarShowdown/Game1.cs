// Author: Noah Teitlebaum
// File Name: Game1.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This driver class is meant to control all the various game states that occur while playing

// 2D Arrays: Used when creating the grid board. Tracks where each tile is and is able to locate certain X and Y values given a specific row or column.

// Lists: Used when keeping track of the player deaths. Each time a player death occurs, the list constantly adds a new player death which leads to a scalable interface when choosing the max amount of deaths. When the game is over,
//        the list loops through itself until all the deaths are removed.

// File I/O: Used when keeping track of player statistics. It keeps track of player 1 and 2 wins, the amount of times any certain type of item was used, and how many health points the item has gained / dealt. 
//           If an error occurs within the file, an error message will appear with a newly created file.

// OOP: Inheritance, Polymorphism, and Encapsulation all used for the game items. With a parent item class, it branches onto the various types of items, which then branches down even further to the actual item being used.
//      In this case, overriding methods are used with displaying and activating different items, creating a scalable option when making a new item. Items may also have their own unique functionalities that they hide in their own classes.

// Stacks: Used during the item selection. Used for setting the max amount of items that the users can play with, and also checking certain features such as if the stack is full or empty. Used when displaying the items during the play state,
//         and works great with the next concept: Linked Lists. Able to push items from the linked list into the stack, while also popping the last item added, back to the linked list.

// Linked Lists: Used during item selection, the instructions state, and item manual state. Used during the instructions and item manual state through displaying various pages one at a time, leading to a less cluttered space when the user
//               intakes any information. During item selection, the items are displayed one at a time in the linked list, and the nodes hold the current item properties for play use. The linked list is a circly doubly version, for features
//               such as a previous node, and an infinite chain when scrolling through the list.

// Searching: Used when looking at the grid board. Each time a player moves through a utility item or just normally, a searching algorithm takes place for finding the current player row and another algorithm takes place for finding the
//            current player column. This is done through keeping track of the tiles X and Y values, leading to an efficient strategy.

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
    public class Game1 : Game
    {
        //Store the random number generator
        public static Random rng = new Random();

        //Store the various game states
        public const int MENU = 0;
        public const int PRE_GAME_STATE = 1;
        public const int PLAY_STATE = 2;
        public const int INSTRUCTIONS_STATE = 3;
        public const int ITEM_MANUAL_STATE = 4;
        public const int STATISTICS_STATE = 5;
        public const int PAUSE_STATE = 6;
        public const int END_GAME_STATE = 7;

        //Store image display devices
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Store the current and previous game state
        public static int gameState = MENU;
        public static int prevGameState = gameState;

        //Store screen dimensions
        public static int screenWidth;
        public static int screenHeight;

        //Store the keyboard and mouse
        public static KeyboardState kb;
        public static KeyboardState prevKb;
        public static MouseState mouse;
        public static MouseState prevMouse;

        //Store the various game state objects
        private Menu menu = new Menu();
        private PreGameState preGameState = new PreGameState();
        private PlayState playState = new PlayState();
        private InstructionsState instructionsState = new InstructionsState();
        private ItemManualState itemManualState = new ItemManualState();
        private StatisticsState statisticsState = new StatisticsState();
        private PauseState pauseState = new PauseState();

        //Store the various game sounds
        public static Song lobbyMusic;
        public static Song fightMusic;
        public static Song suddenDeathMusic;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Set the mouse to be visible during the game
            IsMouseVisible = true;

            //Set the screen dimensions
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load all the various game states
            menu.LoadContent(Content);
            preGameState.LoadContent(Content, GraphicsDevice);
            playState.LoadContent(Content);
            instructionsState.LoadContent(Content);
            itemManualState.LoadContent(Content);
            statisticsState.LoadContent(Content);
            pauseState.LoadContent(Content, GraphicsDevice);

            //Load the various game sounds
            lobbyMusic = Content.Load<Song>("Audio/Music/LobbyMusic");
            fightMusic = Content.Load<Song>("Audio/Music/FightMusic");
            suddenDeathMusic = Content.Load<Song>("Audio/Music/SuddenDeathMusic");
            MediaPlayer.Play(lobbyMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //Update the current and previous mouse and keyboard
            prevKb = kb;
            kb = Keyboard.GetState();
            prevMouse = mouse;
            mouse = Mouse.GetState();

            //Update the appropriate game state based on user’s choice
            switch (gameState)
            {
                case MENU:
                    //Update the menu
                    menu.Update();
                    break;
                case PRE_GAME_STATE:
                    //Update the pre game state
                    preGameState.Update();
                    break;
                case PLAY_STATE:
                    //Update the play state
                    playState.Update(gameTime);
                    break;
                case INSTRUCTIONS_STATE:
                    //Update the instructions state
                    instructionsState.Update();
                    break;
                case ITEM_MANUAL_STATE:
                    //Update the item manual state
                    itemManualState.Update();
                    break;
                case STATISTICS_STATE:
                    //Update the statistics state
                    statisticsState.Update();
                    break;
                case PAUSE_STATE:
                    //Update the pause state
                    pauseState.Update();
                    break;
                default:
                    //Exit the game
                    Exit();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Start drawing the images and texts
            spriteBatch.Begin();

            //Draw the appropriate game state based on user’s choice
            switch (gameState)
            {
                case MENU:
                    //Draw the menu
                    menu.Draw(spriteBatch);
                    break;
                case PRE_GAME_STATE:
                    //Draw the pre game state
                    preGameState.Draw(spriteBatch);
                    break;
                case PLAY_STATE:
                    //Draw the play state
                    playState.Draw(spriteBatch);
                    break;
                case INSTRUCTIONS_STATE:
                    //Draw the instructions state
                    instructionsState.Draw(spriteBatch);
                    break;
                case ITEM_MANUAL_STATE:
                    //Draw the item manual state
                    itemManualState.Draw(spriteBatch);
                    break;
                case STATISTICS_STATE:
                    //Draw the statistics state
                    statisticsState.Draw(spriteBatch);
                    break;
                case PAUSE_STATE:
                    //Draw the pause state
                    pauseState.Draw(spriteBatch, playState);
                    break;
            }

            //End drawing the images and texts
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}