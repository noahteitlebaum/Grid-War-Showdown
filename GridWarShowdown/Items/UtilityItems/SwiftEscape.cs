// Author: Noah Teitlebaum
// File Name: SwiftEscape.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the swift escape item

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
    class SwiftEscape : Utility
    {
        /// <summary>
        /// An item that lets the player move to any corner of the grid
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public SwiftEscape(ContentManager Content) : base(Content)
        {
            //Load the swift escape tile images
            squareImgs.Add(Content.Load<Texture2D>("Images/Sprites/UtilSquare1"));
            squareImgs.Add(Content.Load<Texture2D>("Images/Sprites/UtilSquare2"));
            squareImgs.Add(Content.Load<Texture2D>("Images/Sprites/UtilSquare3"));
            squareImgs.Add(Content.Load<Texture2D>("Images/Sprites/UtilSquare4"));

            //Load the swift escape properties
            potentialSquarePoss = new Vector2[4];
            coolDown = 7;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/SwiftEscapeSound");
            utilityItem = FileManager.SWIFT_ESCAPE;
        }

        //Pre: The current player
        //Post: None
        //Desc: Set the potential squares of the swift escape
        public override void SetPotentialSquares(Player curPlayer)
        {
            //Set the potential swift escape positions
            potentialSquarePoss[TOP_LEFT] = new Vector2(Grid.MIN, Grid.MIN);
            potentialSquarePoss[TOP_RIGHT] = new Vector2(Grid.MAX, Grid.MIN);
            potentialSquarePoss[BOTTOM_LEFT] = new Vector2(Grid.MIN, Grid.MAX);
            potentialSquarePoss[BOTTOM_RIGHT] = new Vector2(Grid.MAX, Grid.MAX);

            //Convert all the potential square locations into bounding boxes
            SetBoundingBoxes();
        }
    }
}