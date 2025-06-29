// Author: Noah Teitlebaum
// File Name: MedicalKit.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the medical kit item

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
    class MedicalKit : Heal
    {
        //Store the medical kit health
        private const int MEDICAL_KIT_HEALTH = 80;

        /// <summary>
        /// An item with a large cool down cost that heals a lot
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public MedicalKit(ContentManager Content) : base(Content)
        {
            //Load the medical kit properties
            coolDown = 9;
            itemNumberText = "+" + MEDICAL_KIT_HEALTH;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/MedicalKitSound");

            //Load the medical kit file properties
            healthItem = FileManager.MEDICAL_KIT;
            healthGained = MEDICAL_KIT_HEALTH;
        }
    }
}