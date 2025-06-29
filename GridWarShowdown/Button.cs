// Author: Noah Teitlebaum
// File Name: Button.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control activated buttons throughout the game

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
    class Button
    {
        //Store the text button properties
        private GameCircle circle;
        private SpriteFont spriteFont;
        private string text;

        //Store the image button properties
        private Texture2D[] imgs;
        private Rectangle rec;
        private double buttonRadius;

        /// <summary>
        /// Used to create a button with any text format
        /// </summary>
        /// <param name="gd">Used to create the game circle</param>
        /// <param name="loc">Where the button is placed</param>
        /// <param name="spriteFont">Font of the button text</param>
        /// <param name="text">Text displayed on the button</param>
        public Button(GraphicsDevice gd, Vector2 loc, SpriteFont spriteFont, string text)
        {
            //Load the text button properties
            circle = new GameCircle(gd, loc, (int)spriteFont.MeasureString(text).Y, 2);
            this.spriteFont = spriteFont;
            this.text = text;

            //Load the buttons bounding box and radius
            rec = new Rectangle((int)circle.GetCentre().X, (int)circle.GetCentre().Y, 0, 0);
            buttonRadius = circle.GetRadius();
        }

        /// <summary>
        /// Used to create a button with an on and off image
        /// </summary>
        /// <param name="imgOff">Button display when not hovered</param>
        /// <param name="imgOn">Button display when hovered</param>
        /// <param name="scalar">How large the button is</param>
        /// <param name="loc">Where the button is placed</param>
        /// <param name="buttonRadiusDivisible">Mouse location for the button to be considered hovered</param>
        public Button(Texture2D imgOff, Texture2D imgOn, float scalar, Vector2 loc, double buttonRadiusDivisible)
        {
            //Load the image button properties
            imgs = new Texture2D[] { imgOff, imgOn };
            rec = new Rectangle((int)loc.X, (int)loc.Y, (int)(scalar * imgOff.Width), (int)(scalar * imgOn.Height));
            buttonRadius = rec.Width / buttonRadiusDivisible;
        }

        //Pre: None
        //Post: The buttons bounding box as a Rectangle
        //Desc: Receive the buttons position
        public Rectangle GetRec()
        {
            //Receive the buttons position
            return rec;
        }

        //Pre: None
        //Post: The buttons hover status as a bool
        //Desc: Receive the hover status
        public bool HoverStatus()
        {
            //Calculate the distance between the buttons and the mouse
            int distanceX = rec.Center.X - Game1.mouse.Position.X;
            int distanceY = rec.Center.Y - Game1.mouse.Position.Y;
            int totalDistance = (int)Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));

            //Check to see if the distance between the arrows and the mouse is less than the radius of the arrows
            if (totalDistance <= buttonRadius)
            {
                //Set the hover affect to true
                return true;
            }

            //Set the hover affect to false
            return false;
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Draw the image buttons
        public void DisplayButton(SpriteBatch spriteBatch)
        {
            //Draw the image button
            spriteBatch.Draw(imgs[Convert.ToInt32(HoverStatus())], rec, Color.White);
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Draw the text buttons
        public void DisplayStringButton(SpriteBatch spriteBatch)
        {
            //Check the hover status of the button
            if (HoverStatus())
            {
                //Display the hovered version of the text button
                circle.Draw(spriteBatch, Color.White, Color.Chocolate);
                spriteBatch.DrawString(spriteFont, text, new Vector2(circle.GetBoundingBox().X - 25, circle.GetBoundingBox().Y + circle.GetRadius() / 2), Color.Chocolate);
            }
            else
            {
                //Display the non hovered version of the text button
                circle.Draw(spriteBatch, Color.White, Color.Gray);
                spriteBatch.DrawString(spriteFont, text, new Vector2(circle.GetBoundingBox().X - 25, circle.GetBoundingBox().Y + circle.GetRadius() / 2), Color.Gray);
            }
        }
    }
}