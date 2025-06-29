// Author: Noah Teitlebaum
// File Name: PlayState.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the play state

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
    class PlayState
    {
        //Store the UI Spacing
        private const int PAUSE_VERT = 10;
        private const int HEALTH_VERT = -15;
        private const int HEALTH_HORIZ1 = 5;
        private const int HEALTH_HORIZ2 = 800;
        private const int HEALTH_TEXT_HORIZ = 18;
        private const int HEALTH_TEXT_VERT = 14;
        private const int COOL_DOWN_VERT = 3;
        private const int COOL_DOWN_HORIZ = 100;
        private const int COOL_DOWN_SPACER = 10;

        //Store the on and off image values
        private const int OFF = 0;
        private const int ON = 1;

        //Store the player values
        private const int PLAYER1 = 0;
        private const int PLAYER2 = 1;

        //Store the sound effects
        private SoundEffect pauseSnd;
        private SoundEffect gameOverSnd;

        //Store the text Fonts
        private SpriteFont UIFont;
        private SpriteFont roundFont;

        //Store the background images and bounding box
        private Texture2D pausedBgImg;
        private Texture2D bgImg;
        private Rectangle bgRec;

        //Store the pause button properties
        private Texture2D[] pauseButtonImgs = new Texture2D[2];
        private Button pauseBar;

        //Store the player health properties
        private Texture2D healthImg;
        private Rectangle[] healthRecs = new Rectangle[2];
        private Vector2[] curHealthLocs = new Vector2[2];

        //Store the player cool down properties
        private Texture2D coolDownCircleImg;
        private Rectangle[] circleRecs = new Rectangle[2];
        private Vector2[] curCoolDownLocs = new Vector2[2];

        //Store the players
        private static int curPlayer;
        private static int nonCurPlayer;
        private static Player[] players = new Player[2];

        //Store the items
        private Item curItem;
        private Item movementItem;

        //Store the round and file management
        public static RoundManagement roundManagement;
        private FileManager playFileManager;

        //Store the movement only state
        private static bool movementOnly = false;

        //Pre: Used to load audio, fonts, images, and graphics
        //Post: None
        //Desc: Load the play state
        public void LoadContent(ContentManager Content)
        {
            //Load the sound effects
            pauseSnd = Content.Load<SoundEffect>("Audio/Sounds/PauseSound");
            gameOverSnd = Content.Load<SoundEffect>("Audio/Sounds/GameOverSound");

            //Load the text Fonts
            UIFont = Content.Load<SpriteFont>("Fonts/ItemFont");
            roundFont = Content.Load<SpriteFont>("Fonts/TitleFont");

            //Load the background images and bounding box
            pausedBgImg = Content.Load<Texture2D>("Images/Backgrounds/PausedBackground");
            bgImg = Content.Load<Texture2D>("Images/Backgrounds/GameBackground");
            bgRec = new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight);

            //Load the pause button properties
            pauseButtonImgs[OFF] = Content.Load<Texture2D>("Images/Sprites/PauseBarOff");
            pauseButtonImgs[ON] = Content.Load<Texture2D>("Images/Sprites/PauseBarOn");
            pauseBar = new Button(pauseButtonImgs[OFF], pauseButtonImgs[ON], 1f, new Vector2((Game1.screenWidth - pauseButtonImgs[OFF].Width) / 2, PAUSE_VERT), 1.4);

            //Load the player health properties
            healthImg = Content.Load<Texture2D>("Images/Sprites/Heart");
            healthRecs[PLAYER1] = new Rectangle(HEALTH_HORIZ1, HEALTH_VERT, healthImg.Width, healthImg.Height);
            healthRecs[PLAYER2] = new Rectangle(HEALTH_HORIZ1 + HEALTH_HORIZ2, HEALTH_VERT, healthImg.Width, healthImg.Height);
            curHealthLocs[PLAYER1] = new Vector2(healthRecs[PLAYER1].Center.X - HEALTH_TEXT_HORIZ, healthRecs[PLAYER1].Center.Y - HEALTH_TEXT_VERT);
            curHealthLocs[PLAYER2] = new Vector2(healthRecs[PLAYER2].Center.X - HEALTH_TEXT_HORIZ, healthRecs[PLAYER2].Center.Y - HEALTH_TEXT_VERT);

            //Load the player cool down properties
            coolDownCircleImg = Content.Load<Texture2D>("Images/Sprites/CoolDownCircle");
            circleRecs[PLAYER1] = new Rectangle(healthRecs[PLAYER1].X + COOL_DOWN_HORIZ, COOL_DOWN_VERT, coolDownCircleImg.Width, coolDownCircleImg.Height);
            circleRecs[PLAYER2] = new Rectangle(healthRecs[PLAYER2].X + COOL_DOWN_HORIZ, COOL_DOWN_VERT, coolDownCircleImg.Width, coolDownCircleImg.Height);
            curCoolDownLocs[PLAYER1] = new Vector2(circleRecs[PLAYER1].Center.X - COOL_DOWN_SPACER, circleRecs[PLAYER1].Center.Y - COOL_DOWN_SPACER);
            curCoolDownLocs[PLAYER2] = new Vector2(circleRecs[PLAYER2].Center.X - COOL_DOWN_SPACER, circleRecs[PLAYER2].Center.Y - COOL_DOWN_SPACER);

            //Set the grid and tiles within it for play
            Grid.SetGrid();

            //Load the players
            curPlayer = PLAYER1;
            nonCurPlayer = PLAYER2;
            players[PLAYER1] = new Player(Content, PLAYER1);
            players[PLAYER2] = new Player(Content, PLAYER2);

            //Load the items
            curItem = new Item(Content);
            movementItem = new Item(Content);

            //Load the round and file management
            roundManagement = new RoundManagement(Content);
            playFileManager = new FileManager(2);
        }

        //Pre: Keeps track of the elapsed game time
        //Post: None
        //Desc: Update the play state
        public void Update(GameTime gameTime)
        {
            //Update the game timers
            roundManagement.UpdateTimers(gameTime);

            //Check if the game is resumed
            if (roundManagement.IsGameResumed())
            {
                //Check if the user pressed the space buttom
                if (Game1.kb.IsKeyDown(Keys.Space) && !Game1.prevKb.IsKeyDown(Keys.Space))
                {
                    //Determine if the player was in the movement state
                    if (movementOnly)
                    {
                        //Set the player out of the movement state
                        movementOnly = false;
                    }
                    else
                    {
                        //Set the player into the movement state
                        movementOnly = true;
                        curItem = movementItem;
                        curItem.SetPotentialSquares(players[curPlayer]);
                    }
                }

                //Check if the player needs to do more than press enter
                if (movementOnly || curItem is SwiftEscape || curItem is HorseGallop)
                {
                    //Use the current item
                    curItem.UseItem(players[curPlayer], players[nonCurPlayer], roundManagement.itemTimer);
                }
                
                //Check if the player is not in the movement state
                if (!movementOnly)
                {
                    //Set the current item based off the users choice
                    curItem = PreGameState.itemStacks[curPlayer].CurrentItemChoice();
                    curItem.SetPotentialSquares(players[curPlayer]);

                    //Check if the item is available for use
                    if (curItem.IsItemAvailable(players[curPlayer]) && !(roundManagement.isSuddenDeath && curItem is Heal))
                    {
                        //Check if the user pressed the enter key and the item is not swift escape or horse gallop
                        if (Game1.kb.IsKeyDown(Keys.Enter) && !Game1.prevKb.IsKeyDown(Keys.Enter) && !(curItem is SwiftEscape || curItem is HorseGallop))
                        {
                            //Use the hovered item
                            UseOverTimeItems();
                            curItem.UseItem(players[curPlayer], players[nonCurPlayer], roundManagement.itemTimer);

                            //Subtract the cool down cost and switch the players
                            players[curPlayer].curCoolDown -= curItem.GetCoolDown();
                            Switch();
                        }
                    }
                }

                //Check if the round timer finished
                if (roundManagement.roundTimer.IsFinished())
                {
                    //Check where the player 1 health is realted to the player 2 health
                    if (players[PLAYER1].curHealth > players[PLAYER2].curHealth)
                    {
                        //Set player 2 to be dead
                        players[PLAYER2].curHealth = Player.DEAD;
                        PreGameState.fightSnd.CreateInstance().Play();
                    }
                    else if (players[PLAYER1].curHealth < players[PLAYER2].curHealth)
                    {
                        //Set player 1 to be dead
                        players[PLAYER1].curHealth = Player.DEAD;
                        PreGameState.fightSnd.CreateInstance().Play();
                    }
                    else
                    {
                        //Set the sudden death state
                        roundManagement.SetSuddenDeath(players[PLAYER1], players[PLAYER2]);
                    }
                }

                //Iterate through the players
                for (int i = 0; i < players.Length; i++)
                {
                    //Check if a player died
                    if (roundManagement.IsPlayerDead(players[i]))
                    {
                        //Check if the game was just in sudden death
                        if (roundManagement.isSuddenDeath)
                        {
                            //Play the fighting music
                            MediaPlayer.Play(Game1.fightMusic);
                        }

                        //Add a death to the current player who died and set the game for the next round
                        roundManagement.AddDeath(i);
                        players[PLAYER1].SetForPlay(PLAYER1);
                        players[PLAYER2].SetForPlay(PLAYER2);

                        //Check if the game is over
                        if (roundManagement.IsGameOver())
                        {
                            //Play the game over sound
                            gameOverSnd.CreateInstance().Play();
                        }

                        //Reset the round for the next one
                        roundManagement.isSuddenDeath = false;
                        roundManagement.ResetTimers();
                        movementOnly = false;
                    }
                }

                //Check if the game is over
                if (roundManagement.IsGameOver())
                {
                    //Check if the pause timer finished
                    if (roundManagement.pauseTimer.IsFinished())
                    {
                        //Pop all the items from the stack back to the linked list
                        PreGameState.PopAll();

                        //Reset the game fully
                        roundManagement.ResetTimers();
                        roundManagement.Reset();
                        curPlayer = PLAYER1;
                        nonCurPlayer = PLAYER2;

                        //Go back to the menu state while playing the lobby music
                        Game1.prevGameState = Game1.gameState;
                        Game1.gameState = Game1.MENU;
                        MediaPlayer.Play(Game1.lobbyMusic);
                    }

                    //Check which player won the game
                    if (roundManagement.player1Deaths == roundManagement.maxDeaths)
                    {
                        //Set player 2 won the game
                        playFileManager.AddPlayerWon(PLAYER2);
                        roundManagement.gameOverText = "PLAYER 2 WINS!";
                        roundManagement.gameOverTextColor = Color.DodgerBlue;
                    }
                    else if (roundManagement.player2Deaths == roundManagement.maxDeaths)
                    {
                        //Set player 1 won the game
                        playFileManager.AddPlayerWon(PLAYER1);
                        roundManagement.gameOverText = "PLAYER 1 WINS!";
                        roundManagement.gameOverTextColor = Color.Crimson;
                    }

                    //Write to the statistics file about the item and player statistics
                    playFileManager.WriteFile(false);
                    curItem.GetFileManager().WriteFile(true);
                }

                //Check if the user clicked the left mouse button
                if (Game1.mouse.LeftButton == ButtonState.Pressed && Game1.prevMouse.LeftButton != ButtonState.Pressed)
                {
                    //Check if the pause bar was hovered
                    if (pauseBar.HoverStatus())
                    {
                        //Enter the pause state
                        Game1.prevGameState = Game1.gameState;
                        Game1.gameState = Game1.PAUSE_STATE;
                        pauseSnd.CreateInstance().Play();
                    }
                }
            }
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Draw the play state
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the background image
            spriteBatch.Draw(bgImg, bgRec, Color.White);

            //If the game is in sudden death
            if (roundManagement.isSuddenDeath)
            {
                //Draw the sudden death background image
                spriteBatch.Draw(bgImg, bgRec, Color.Red * 0.6f);
            }

            //Draw the pause button and game timers
            pauseBar.DisplayButton(spriteBatch);
            roundManagement.DisplayTimers(spriteBatch, curPlayer);

            //Draw the current item the current player is hovering over
            PreGameState.itemStacks[curPlayer].DisplayCurrentItemBox(spriteBatch);

            //Iterate through each player
            for (int i = 0; i < players.Length; i++)
            {
                //Draw the player items
                PreGameState.itemStacks[i].DisplayPlayGameStack(spriteBatch);

                //Draw the heatlh image properties
                spriteBatch.Draw(healthImg, healthRecs[i], Color.Beige);
                Effects.OverlapTextDisplay(spriteBatch, UIFont, Convert.ToString(players[i].curHealth).PadLeft(3, '0'), curHealthLocs[i], Color.Black, Color.Yellow);

                //Draw the cool down image properties
                spriteBatch.Draw(coolDownCircleImg, circleRecs[i], Color.Beige * 0.5f);
                Effects.OverlapTextDisplay(spriteBatch, UIFont, Convert.ToString(players[i].curCoolDown).PadLeft(2, '0'), curCoolDownLocs[i], Color.Black, Color.Yellow);
            }

            //Check if the game is over or the pause timer is finished
            if (roundManagement.IsGameOver())
            {
                //Display the game over text
                spriteBatch.Draw(pausedBgImg, bgRec, Color.Black * 0.8f);
                Effects.OverlapTextDisplay(spriteBatch, roundFont, roundManagement.gameOverText,
                                           new Vector2((Game1.screenWidth - roundFont.MeasureString(roundManagement.gameOverText).X) / 2, (Game1.screenHeight - roundFont.MeasureString(roundManagement.gameOverText).Y) / 2),
                                           Color.MintCream, roundManagement.gameOverTextColor);
            }
            else if (!roundManagement.pauseTimer.IsFinished())
            {
                //Check if the game is in sudden death
                if (roundManagement.isSuddenDeath)
                {
                    //Display the sudden death text
                    spriteBatch.Draw(pausedBgImg, bgRec, Color.Black * 0.8f);
                    Effects.OverlapTextDisplay(spriteBatch, roundFont, "SUDDEN DEATH!",
                                               new Vector2((Game1.screenWidth - roundFont.MeasureString("SUDDEN DEATH!").X) / 2, (Game1.screenHeight - roundFont.MeasureString("SUDDEN DEATH!").Y) / 2),
                                               Color.Black, Color.MintCream);
                }
                else
                {
                    //Display the current round text
                    spriteBatch.Draw(pausedBgImg, bgRec, Color.Black * 0.8f);
                    Effects.OverlapTextDisplay(spriteBatch, roundFont, "ROUND: " + roundManagement.curRound,
                                               new Vector2((Game1.screenWidth - roundFont.MeasureString("ROUND: " + roundManagement.curRound).X) / 2, (Game1.screenHeight - roundFont.MeasureString("ROUND: " + roundManagement.curRound).Y) / 2),
                                               Color.Black, Color.MintCream);
                }
            }
            else
            {
                //Check if the item is available
                if (curItem.IsItemAvailable(players[curPlayer]) && roundManagement.IsGameResumed())
                {
                    //Check if the item is swift escape or horse gallop
                    if (curItem is SwiftEscape || curItem is HorseGallop)
                    {
                        //Display the items potential tiles before the players current tile
                        curItem.DisplayPotentialSquares(spriteBatch, players[nonCurPlayer]);
                        players[curPlayer].DisplayCurrentTile(spriteBatch);
                    }
                    else
                    {
                        //Display the players current tile
                        players[curPlayer].DisplayCurrentTile(spriteBatch);

                        //Check if the game is not sudden death and the current item is not a heal
                        if (!(roundManagement.isSuddenDeath && curItem is Heal))
                        {
                            //Display the items potential tiles
                            curItem.DisplayPotentialSquares(spriteBatch, players[nonCurPlayer]);
                        }
                    }
                }
                else
                {
                    //Display the players current tile
                    players[curPlayer].DisplayCurrentTile(spriteBatch);
                }
            }

            //Display the player deaths
            roundManagement.DisplayDeaths(spriteBatch);

            //Check which player is on the higher collumn
            if (players[PLAYER1].curCol > players[PLAYER2].curCol)
            {
                //Display player 2 before player 1
                players[PLAYER2].Display(spriteBatch);
                players[PLAYER1].Display(spriteBatch);
            }
            else
            {
                //Display player 1 before player 2
                players[PLAYER1].Display(spriteBatch);
                players[PLAYER2].Display(spriteBatch);
            }

            //Check if the item timer is active
            if (roundManagement.itemTimer.IsActive())
            {
                //Draw the item number to show how much damage the item gained / dealt
                curItem.DisplayItemNumber(spriteBatch, UIFont, players[nonCurPlayer], players[curPlayer]);
            }
        }

        //Pre: None
        //Post: None
        //Desc: Switches the players turn
        public static void Switch()
        {
            //Reset the player timer
            roundManagement.playerSwapTimer.ResetTimer(true);
            movementOnly = false;

            //Switch the players turn
            int temp = curPlayer;
            curPlayer = nonCurPlayer;
            nonCurPlayer = temp;
        }

        //Pre: None
        //Post: None
        //Desc: Uses overtime item effects
        public static void UseOverTimeItems()
        {
            //Use the overtime item effects
            ShieldPotion.UseOverTimeHealing(players[curPlayer]);
            Torch.UseOverTimeDamage(players[curPlayer]);
            GuardianShield.UseOverTimeShield(players[curPlayer]);
        }
    }
}