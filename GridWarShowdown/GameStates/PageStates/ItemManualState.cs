// Author: Noah Teitlebaum
// File Name: ItemManualState.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the item manual state

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
    class ItemManualState
    {
        //Store the item manual pages
        private CirclyDoublyLinkedList<ManualNode> itemManualPages = new CirclyDoublyLinkedList<ManualNode>();
        private Pages pages;

        //Pre: Used to load audio, fonts, and images 
        //Post: None
        //Desc: Load the item manual state
        public void LoadContent(ContentManager Content)
        {
            //Add the item manual pages to the linked list
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/ShieldPotionManual")));
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/InstantHealManual")));
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/MedicalKitManual")));
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/SwordManual")));
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/TorchManual")));
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/DynamiteManual")));
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/CrossbowManual")));
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/BoomerangManual")));
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/HarpoonManual")));
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/SwiftEscapeManual")));
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/HorseGallopManual")));
            itemManualPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/GuardianShieldManual")));

            //Load the various amount of pages
            pages = new Pages(itemManualPages, Content, 10);
        }

        //Pre: None
        //Post: None
        //Desc: Update the item manual state
        public void Update()
        {
            //Update the various amount of pages
            pages.Update();
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Draw the item manual state
        public void Draw(SpriteBatch spriteBatch)
        {
            //Display the various amount of pages
            pages.Display(spriteBatch);
        }
    }
}