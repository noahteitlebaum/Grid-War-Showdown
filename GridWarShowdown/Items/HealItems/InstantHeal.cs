// Author: Noah Teitlebaum
// File Name: InstantHeal.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the instant heal item

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
    class InstantHeal : Heal
    {
        //Store the instant heal health
        private const int INSTANT_HEAL = 15;

        /// <summary>
        /// An item with a small cool down cost that does not heal a lot
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public InstantHeal(ContentManager Content) : base(Content)
        {
            //Load the instant heal properties
            coolDown = 3;
            itemNumberText = "+" + INSTANT_HEAL;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/InstantHealSound");

            //Load the instant heal file properties
            healthItem = FileManager.INSTANT_HEAL;
            healthGained = INSTANT_HEAL;
        }
    }
}