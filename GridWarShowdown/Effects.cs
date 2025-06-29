// Author: Noah Teitlebaum
// File Name: Effects.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control all the functionality behind any effects

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
    static class Effects
    {
        //Store the scrolling background bounding boxes
        private static Rectangle[] bgRecs = new Rectangle[] { new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), new Rectangle(-Game1.screenWidth, 0, Game1.screenWidth, Game1.screenHeight) };

        //Pre: Used to draw various sprites and the image used for the background
        //Post: None
        //Desc: Display a parallax effect
        public static void DrawParallaxBg(SpriteBatch spriteBatch, Texture2D img)
        {
            //Iterate through each backgrounds bounding box
            for (int i = 0; i < bgRecs.Length; i++)
            {
                //Increment each bounding boxes x value to make it move
                bgRecs[i].X += 1;

                //Check if the backgrounds bounding box is out of screen view
                if (bgRecs[i].X >= Game1.screenWidth)
                {
                    //Reset the position of the backgrounds bounding box for a parallax effect
                    bgRecs[i].X = -Game1.screenWidth;
                }

                //Draw the background images
                spriteBatch.Draw(img, bgRecs[i], Color.White);
            }
        }

        //Pre: Used to draw various sprites, the text properties, and the colors to give off the overlap effect
        //Post: None
        //Desc: Display an overlap text effect
        public static void OverlapTextDisplay(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 loc, Color color, Color overlapColor)
        {
            //Display the texts
            spriteBatch.DrawString(font, text, loc, color);
            spriteBatch.DrawString(font, text, new Vector2(loc.X - 2, loc.Y - 2), overlapColor);
        }
    }
}