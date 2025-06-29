// Author: Noah Teitlebaum
// File Name: GuardianShield.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the guardian shield item

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
    class GuardianShield : Utility
    {
        //Store the potential square for the guardian shield
        private const int CURRENT = 0;

        //Store the cool down for the guardian shield
        private const int GUARDIAN_SHIELD_DURATION = 3;

        //Store the sound effect for the guardian shield
        public static SoundEffect guardianShieldAccumulationSnd;

        /// <summary>
        /// An item that lets the player be invincible for a number of rounds
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public GuardianShield(ContentManager Content) : base(Content)
        {
            //Load the guardian shield tile image
            squareImgs.Add(Content.Load<Texture2D>("Images/Sprites/UtilSquare"));

            //Load the guardian shield properties
            potentialSquarePoss = new Vector2[1];
            coolDown = 6;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/GuardianShieldInstantSound");
            utilityItem = FileManager.GUARDIAN_SHIELD;

            //Load the sound effect for the guardian shield
            guardianShieldAccumulationSnd = Content.Load<SoundEffect>("Audio/Sounds/GuardianShieldAccumulationSound");
        }

        //Pre: The current player
        //Post: None
        //Desc: Set the potential squares of the guardian shield relative to the players position
        public override void SetPotentialSquares(Player curPlayer)
        {
            //Set the potential guardian shield positions
            potentialSquarePoss[CURRENT] = new Vector2(curPlayer.curRow, curPlayer.curCol);
            SetBoundingBoxes();
        }

        //Pre: The players and the item timer
        //Post: None
        //Desc: Use the guardian shield
        public override void UseItem(Player curPlayer, Player nonCurPlayer, Timer itemTimer)
        {
            //Active the guardian shield overtime effect
            curPlayer.isGuardianShieldActive = true;
            itemSnd.CreateInstance().Play();

            //Add the guardian shield data to the file manager
            itemFileManager.AddTotalItemUse(utilityItem);
        }

        //Pre: The non current player
        //Post: None
        //Desc: Use the guardian shield overtime effect
        public static void UseOverTimeShield(Player nonCurPlayer)
        {
            //Check if the guardian shield is active
            if (nonCurPlayer.isGuardianShieldActive)
            {
                //Increment the guardian shield counter
                nonCurPlayer.guardianShieldCounter++;
            }

            //Check if the guardian shield has reached its duration
            if (nonCurPlayer.guardianShieldCounter == GUARDIAN_SHIELD_DURATION)
            {
                //Reset the guardian shield use
                nonCurPlayer.isGuardianShieldActive = false;
                nonCurPlayer.guardianShieldCounter = 0;
            }
        }
    }
}