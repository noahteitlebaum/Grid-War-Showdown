// Author: Noah Teitlebaum
// File Name: RoundManagement.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the rounds while the game is being played

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
    class RoundManagement
    {
        //Store the UI Spacing
        private const int DEATH_VERT = 10;
        private const int DEATH_HORIZ1 = 200;
        private const int DEATH_HORIZ2 = 730;
        private const int DEATH_HORIZ_SPACER = 65;
        private const int ROUND_TIMER_HORIZ = 100;
        private const int SWAP_TIMER_HORIZ = 55;

        //Store the player values
        private const int PLAYER1 = 0;
        private const int PLAYER2 = 1;

        //Store the timer durations in seconds
        private const int PAUSE_DURATION = 2;
        private const int ITEM_TIMER_DURATION = 1;
        private const int ROUND_DURATION = 250;
        private const int WARNING_DURATION = 30;
        private const int PLAYER_SWAP_DURATION = 20;

        //Store the switch sound effect
        private SoundEffect switchSnd;

        //Store the timers and the font to display them
        private SpriteFont timerFont;
        public Timer pauseTimer;
        public Timer itemTimer;
        public Timer roundTimer;
        public Timer playerSwapTimer;

        //Store the round and swap timer locations
        private Vector2 roundTimerLoc;
        private Vector2 playerSwapTimerLoc;

        //Store the round and swap timer colors
        private Color roundTimerColor = Color.MintCream;
        private Color[] playerColors = new Color[] { Color.Crimson, Color.DodgerBlue };

        //Store the player deaths
        public int maxDeaths;
        public int player1Deaths;
        public int player2Deaths;

        //Store the current round
        public int curRound;

        //Store the death image properties
        private Texture2D deathImg;
        private List<Rectangle> deathRecs = new List<Rectangle>();

        //Store the game over text properties
        public string gameOverText;
        public Color gameOverTextColor;

        //Store if the game is sudden death
        public bool isSuddenDeath;

        /// <summary>
        /// Used to manage the game rounds and properties when the game is being played
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public RoundManagement(ContentManager Content)
        {
            //Load the switch sound effect
            switchSnd = Content.Load<SoundEffect>("Audio/Sounds/SwitchSound");

            //Load the timers and the font to display them
            timerFont = Content.Load<SpriteFont>("Fonts/SubTitleFont");
            pauseTimer = new Timer(PAUSE_DURATION, true);
            itemTimer = new Timer(ITEM_TIMER_DURATION, false);
            roundTimer = new Timer(ROUND_DURATION, true);
            playerSwapTimer = new Timer(PLAYER_SWAP_DURATION, true);

            //Load the round and swap timer locations
            roundTimerLoc = new Vector2(Game1.screenWidth / 2 - ROUND_TIMER_HORIZ, 10);
            playerSwapTimerLoc = new Vector2(Game1.screenWidth / 2 + SWAP_TIMER_HORIZ, 10);

            //Load the death image
            deathImg = Content.Load<Texture2D>("Images/Sprites/Death");

            //Load the game over text properties
            gameOverText = "";
            gameOverTextColor = Color.White;

            //Reset the game for play use
            Reset();
        }

        //Pre: None
        //Post: None
        //Desc: Reset thhe round for play use
        public void Reset()
        {
            //Set the size equal to the amount of deaths
            int size = deathRecs.Count - 1;

            //Increment through the amount of deaths
            for (int i = size; i >= 0; i--)
            {
                //Remove every death setting it back to zero
                deathRecs.RemoveAt(i);
            }

            //Reset the player deaths
            maxDeaths = 2;
            player1Deaths = 0;
            player2Deaths = 0;

            //Reset the current round
            curRound = 1;
        }

        //Pre: The current player
        //Post: Receiving if the current player is dead as a bool
        //Desc: Check if the current player is dead
        public bool IsPlayerDead(Player player)
        {
            //Return if the current player is dead
            return player.curHealth <= Player.DEAD;
        }

        //Pre: None
        //Post: Receiving if any of the players have reached the max deaths as a bool
        //Desc: Check if the game is over
        public bool IsGameOver()
        {
            //Return if any of the players have reached the max deaths
            return player1Deaths == maxDeaths || player2Deaths == maxDeaths;
        }

        //Pre: None
        //Post: Receiving if the pause timer is finished and the item timer is inactive as a bool
        //Desc: Check if the game is resumed
        public bool IsGameResumed()
        {
            //Return if the pause timer is finished and the item timer is inactive
            return pauseTimer.IsFinished() && itemTimer.IsInactive();
        }

        //Pre: The current dead player
        //Post: None
        //Desc: Add a death to the current player who died
        public void AddDeath(int deadPlayer)
        {
            //Check which player died
            if (deadPlayer == PLAYER1)
            {
                //Add a death for player1
                deathRecs.Add(new Rectangle(DEATH_HORIZ1 + (DEATH_HORIZ_SPACER * player1Deaths), DEATH_VERT, deathImg.Width, deathImg.Height));
                player1Deaths++;
            }
            else if (deadPlayer == PLAYER2)
            {
                //Add a death for player2
                deathRecs.Add(new Rectangle(DEATH_HORIZ2 - (DEATH_HORIZ_SPACER * player2Deaths), DEATH_VERT, deathImg.Width, deathImg.Height));
                player2Deaths++;
            }

            //Increment the current round
            curRound++;
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Display the amount of deaths
        public void DisplayDeaths(SpriteBatch spriteBatch)
        {
            //Increment through the amount of rounds that have passed
            for (int i = 0; i < curRound - 1; i++)
            {
                //Draw the amount of deaths per player
                spriteBatch.Draw(deathImg, deathRecs[i], Color.White);
            }
        }

        //Pre: Player 1 and player 2
        //Post: None
        //Desc: Set the game for sudden death
        public void SetSuddenDeath(Player player1, Player player2)
        {
            //Activate sudden death
            isSuddenDeath = true;

            //Set the game for sudden death
            player1.SetForPlay(PLAYER1);
            player2.SetForPlay(PLAYER2);
            player1.curHealth = 1;
            player2.curHealth = 1;

            //Switch the player turns
            PlayState.Switch();

            //Set the timers
            pauseTimer.ResetTimer(true);
            roundTimer.ResetTimer(false);

            //Play the sudden death music
            MediaPlayer.Play(Game1.suddenDeathMusic);
            PreGameState.fightSnd.CreateInstance().Play();
            MediaPlayer.Volume = 0.6f;
        }

        //Pre: Keeps track of the elapsed game time
        //Post: None
        //Desc: Update the various timers
        public void UpdateTimers(GameTime gameTime)
        {
            //Update the pause and item timers
            pauseTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);
            itemTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);

            //Check if the item timer finished
            if (itemTimer.IsFinished())
            {
                //Reset the item timer
                itemTimer.ResetTimer(false);
            }

            //Check if the game is resumed
            if (IsGameResumed())
            {
                //Update the round and player swap timers
                roundTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);
                playerSwapTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);

                //Check if the player swap timer finished
                if (playerSwapTimer.IsFinished())
                {
                    //Switch the current player turn
                    PlayState.UseOverTimeItems();
                    PlayState.Switch();
                    switchSnd.CreateInstance().Play();
                }
            }

            //Check if the round timer has reached the warning duration
            if (roundTimer.GetTimeRemaining() <= WARNING_DURATION)
            {
                //Set the new swap timer duration
                playerSwapTimer.SetTargetTime(PLAYER_SWAP_DURATION / 2);

                //Check if the remaining round time is even or not
                if (roundTimer.GetTimeRemainingInt() % 2 == 0)
                {
                    //Set the color display of the round time to yellow
                    roundTimerColor = Color.Yellow;
                }
                else
                {
                    //Set the color display of the round time to orange
                    roundTimerColor = Color.Orange;
                }
            }
            else
            {
                //Set the swap timer duration and the round timer color display to normal
                playerSwapTimer.SetTargetTime(PLAYER_SWAP_DURATION);
                roundTimerColor = Color.White;
            }
        }

        //Pre: None
        //Post: None
        //Desc: Reset the timers
        public void ResetTimers()
        {
            pauseTimer.ResetTimer(true);
            roundTimer.ResetTimer(true);
            playerSwapTimer.ResetTimer(true);
        }

        //Pre: Used to draw various sprites, and the current player turn
        //Post: None
        //Desc: Display the round and player swap timers, with their corresponding colors
        public void DisplayTimers(SpriteBatch spriteBatch, int curPlayer)
        {
            //Draw the timers
            Effects.OverlapTextDisplay(spriteBatch, timerFont, roundTimer.GetTimeRemainingAsString(Timer.FORMAT_MIL), roundTimerLoc, Color.Black, roundTimerColor);
            Effects.OverlapTextDisplay(spriteBatch, timerFont, playerSwapTimer.GetTimeRemainingAsString(Timer.FORMAT_MIL), playerSwapTimerLoc, Color.Black, playerColors[curPlayer]);
        }
    }
}