// Author: Noah Teitlebaum
// File Name: ShieldPotion.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the shield potion item

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
    class ShieldPotion : Heal
    {
        //Store the  shield potion damages
        private const int SHIELD_POTION_INSTANT = 20;
        private const int SHIELD_POTION_ACCUMULATION = 10;
        private const int SHIELD_POTION_DURATION = 3;

        //Store the sound effect for the shield potion
        private static SoundEffect shieldPotionAccumulationSnd;

        /// <summary>
        /// An item that lets the player heal with accumulative health
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public ShieldPotion(ContentManager Content) : base(Content)
        {
            //Load the shield potion properties
            coolDown = 6;
            itemNumberText = "+" + SHIELD_POTION_INSTANT;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/ShieldPotionInstantSound");

            //Load the shield potion file properties
            healthItem = FileManager.SHIELD_POTION;
            healthGained = SHIELD_POTION_INSTANT;

            //Load the sound effect for the shield potion
            shieldPotionAccumulationSnd = Content.Load<SoundEffect>("Audio/Sounds/ShieldPotionAccumulationSound");
        }

        //Pre: The current player
        //Post: None
        //Desc: Use the shield potion ovetime effect
        public static void UseOverTimeHealing(Player curPlayer)
        {
            //Check if the shield potion is active
            if (curPlayer.isShieldPotionHeal)
            {
                //Increase the current players health
                curPlayer.curHealth = Math.Min(curPlayer.curHealth + SHIELD_POTION_ACCUMULATION, Player.MAX_HEALTH);
                shieldPotionAccumulationSnd.CreateInstance().Play();

                //Add the items gained data to the file manager
                itemFileManager.AddTotalHealthPoints(FileManager.SHIELD_POTION, SHIELD_POTION_ACCUMULATION);

                //Increment the shield potion counter
                curPlayer.shieldPotionCounter++;
            }

            //Check if the shield potion has reached its duration
            if (curPlayer.shieldPotionCounter == SHIELD_POTION_DURATION)
            {
                //Reset the shield potion use
                curPlayer.isShieldPotionHeal = false;
                curPlayer.shieldPotionCounter = 0;
            }
        }
    }
}