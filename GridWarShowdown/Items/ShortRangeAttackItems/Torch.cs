// Author: Noah Teitlebaum
// File Name: Torch.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the torch item

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
    class Torch : ShortRangeAttack
    {
        //Store the torch damages
        private const int TORCH_DAMAGE_INSTANT = 20;
        private const int TORCH_DAMAGE_ACCUMULATION = 10;
        private const int TORCH_DURATION = 5;

        //Store the potential squares for the torch
        private const int TOP_LEFT = 0;
        private const int TOP_RIGHT = 1;
        private const int BOTTOM_LEFT = 2;
        private const int BOTTOM_RIGHT = 3;

        //Store the sound effect for the torch
        private static SoundEffect torchAccumulationSnd;

        /// <summary>
        /// An item that lets the player attack with accumulative damage
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public Torch(ContentManager Content) : base(Content)
        {
            //Load the torch properties
            potentialSquarePoss = new Vector2[4];
            coolDown = 8;
            itemNumberText = "-" + TORCH_DAMAGE_INSTANT;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/TorchInstantSound");

            //Load the torch file properties
            shortRangeAttackItem = FileManager.TORCH;
            healthLost = TORCH_DAMAGE_INSTANT;

            //Load the sound effect for the torch
            torchAccumulationSnd = Content.Load<SoundEffect>("Audio/Sounds/TorchAccumulationSound");

        }

        //Pre: The current player
        //Post: None
        //Desc: Set the potential squares of the torch relative to the players position
        public override void SetPotentialSquares(Player curPlayer)
        {
            //Set the potential torch positions
            potentialSquarePoss[TOP_LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol - 1);
            potentialSquarePoss[TOP_RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol - 1);
            potentialSquarePoss[BOTTOM_LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol + 1);
            potentialSquarePoss[BOTTOM_RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol + 1);

            //Convert all the potential square locations into bounding boxes
            SetBoundingBoxes();
        }

        //Pre: The current player
        //Post: None
        //Desc: Use the torch overtime effect
        public static void UseOverTimeDamage(Player curPlayer)
        {
            //Check if the torch is active
            if (curPlayer.isTorchDamage)
            {
                //Check if the guardian shield effect is not active
                if (!curPlayer.isGuardianShieldActive)
                {
                    //Decrease the opposing players health
                    curPlayer.curHealth = curPlayer.curHealth - TORCH_DAMAGE_ACCUMULATION;
                    torchAccumulationSnd.CreateInstance().Play();

                    //Add the items dealt data to the file manager
                    itemFileManager.AddTotalHealthPoints(FileManager.TORCH, TORCH_DAMAGE_ACCUMULATION);
                }
                else
                {
                    //Activate the guardian shields sound effect when hit
                    GuardianShield.guardianShieldAccumulationSnd.CreateInstance().Play();
                }

                //Increment the torch counter
                curPlayer.torchDamageCounter++;
            }

            //Check if the torch has reached its duration
            if (curPlayer.torchDamageCounter == TORCH_DURATION)
            {
                //Reset the torch use
                curPlayer.isTorchDamage = false;
                curPlayer.torchDamageCounter = 0;
            }
        }
    }
}