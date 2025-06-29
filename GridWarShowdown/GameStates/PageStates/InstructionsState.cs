// Author: Noah Teitlebaum
// File Name: Instructions.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the instructions state

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
    class InstructionsState
    {
        //Store the instruction pages
        private CirclyDoublyLinkedList<ManualNode> instructionPages = new CirclyDoublyLinkedList<ManualNode>();
        private Pages pages;

        //Pre: Used to load audio, fonts, and images 
        //Post: None
        //Desc: Load the instructions state
        public void LoadContent(ContentManager Content)
        {
            //Add the instruction pages to the linked list
            instructionPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/WelcomeInstruction")));
            instructionPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/ItemSelectionInstruction")));
            instructionPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/Game1Instruction")));
            instructionPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/Game2Instruction")));
            instructionPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/SuddenDeathInstruction")));
            instructionPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/PlayerMovementInstruction")));
            instructionPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/PlayerItemUseInstruction")));
            instructionPages.AddToTail(new ManualNode(Content.Load<Texture2D>("Images/Backgrounds/OvertimeItemsInstruction")));

            //Load the various amount of pages
            pages = new Pages(instructionPages, Content, 610);
        }

        //Pre: None
        //Post: None
        //Desc: Update the instructions state
        public void Update()
        {
            //Update the various amount of pages
            pages.Update();
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Draw the instructions state
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the various amount of pages
            pages.Display(spriteBatch);   
        }
    }
}