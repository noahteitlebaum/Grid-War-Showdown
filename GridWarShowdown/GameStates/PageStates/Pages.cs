// Author: Noah Teitlebaum
// File Name: Pages.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant for the manual node linked lists and its images

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
    class Pages
    {
        //Store the UI Spacing
        private const int LEFT_ARROW_HORIZ = 10;
        private const int RIGHT_ARROW_HORIZ = 875;
        private const int INST_VERT = 50;

        //Store the ON and OFF image values
        private const int OFF = 0;
        private const int ON = 1;

        //Store the on and off arrow images
        private Texture2D[] leftArrowImgs = new Texture2D[2];
        private Texture2D[] rightArrowImgs = new Texture2D[2];

        //Store the arrow buttons
        private Button leftArrow;
        private Button rightArrow;
        private float buttonScalar = 0.55f;
        private double buttonRadiusDivisible = 3.15;

        //Store the instruction text, location, and font
        private SpriteFont instFont;
        private Vector2 instLoc;
        private string instText = "*PRESS |ESC| TO GO BACK*";

        //Store the linked list and the current page being displayed
        private ManualNode curPage;

        /// <summary>
        /// Used to display various images on the screen in an ordered format
        /// </summary>
        /// <param name="pages">The pages being displayed in a linked list</param>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        /// <param name="arrowVert">How high the left and right arrows should be placed</param>
        public Pages(CirclyDoublyLinkedList<ManualNode> pages, ContentManager Content, int arrowVert)
        {
            //Set the current page
            curPage = pages.head;

            //Load the arrow images
            leftArrowImgs[OFF] = Content.Load<Texture2D>("Images/Sprites/LeftArrowOff");
            leftArrowImgs[ON] = Content.Load<Texture2D>("Images/Sprites/LeftArrowOn");
            rightArrowImgs[OFF] = Content.Load<Texture2D>("Images/Sprites/RightArrowOff");
            rightArrowImgs[ON] = Content.Load<Texture2D>("Images/Sprites/RightArrowOn");

            //Load the arrow buttons
            leftArrow = new Button(leftArrowImgs[OFF], leftArrowImgs[ON], buttonScalar, new Vector2(LEFT_ARROW_HORIZ, arrowVert), buttonRadiusDivisible);
            rightArrow = new Button(rightArrowImgs[OFF], rightArrowImgs[ON], buttonScalar, new Vector2(RIGHT_ARROW_HORIZ, arrowVert), buttonRadiusDivisible);

            //Load the instruction location and font
            instFont = Content.Load<SpriteFont>("Fonts/ItemFont");
            instLoc = new Vector2((Game1.screenWidth - instFont.MeasureString(instText).X) / 2, arrowVert + INST_VERT);
        }

        //Pre: None
        //Post: None
        //Desc: Update the pages
        public void Update()
        {
            //Check to see if the user pressed the escape key
            if (Game1.kb.IsKeyDown(Keys.Escape) && !Game1.prevKb.IsKeyDown(Keys.Escape))
            {
                //Go back to the previous game state
                Game1.gameState = Game1.prevGameState;
            }

            //Check to see if the user clicked the left mouse button
            if (Game1.mouse.LeftButton == ButtonState.Pressed && Game1.prevMouse.LeftButton != ButtonState.Pressed)
            {
                //Check to see if any arrow is being hovered over
                if (leftArrow.HoverStatus())
                {
                    //Set the current page to the previous page in the list
                    curPage = curPage.prev;
                    Menu.UISnd.CreateInstance().Play();
                }
                else if (rightArrow.HoverStatus())
                {
                    //Set the current page to the next page in the list
                    curPage = curPage.next;
                    Menu.UISnd.CreateInstance().Play();
                }
            }

            //Check to see if the user clicked the left arrow key
            if (Game1.kb.IsKeyDown(Keys.Left) && !Game1.prevKb.IsKeyDown(Keys.Left))
            {
                //Set the current page to the previous page in the list
                curPage = curPage.prev;
                Menu.UISnd.CreateInstance().Play();
            }
            else if (Game1.kb.IsKeyDown(Keys.Right) && !Game1.prevKb.IsKeyDown(Keys.Right))
            {
                //Set the current page to the next page in the list
                curPage = curPage.next;
                Menu.UISnd.CreateInstance().Play();
            }
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Draw the pages
        public void Display(SpriteBatch spriteBatch)
        {
            //Draw the current manual page
            spriteBatch.Draw(curPage.page, curPage.pageRec, Color.White);

            //Draw the arrows
            leftArrow.DisplayButton(spriteBatch);
            rightArrow.DisplayButton(spriteBatch);

            //Draw the instruction text
            Effects.OverlapTextDisplay(spriteBatch, instFont, instText, instLoc, Color.Black, Color.Yellow);
        }
    }
}