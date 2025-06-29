// Author: Noah Teitlebaum
// File Name: HorseGallop.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the horse gallop item

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
    class HorseGallop : Utility
    {
        /// <summary>
        /// An item that lets the player move in a unique shape
        /// </summary>
        /// <param name="Content">Used to load audio, fonts, and images</param>
        public HorseGallop(ContentManager Content) : base(Content)
        {
            //Load the horse gallop tile images
            squareImgs.Add(Content.Load<Texture2D>("Images/Sprites/UtilSquare1"));
            squareImgs.Add(Content.Load<Texture2D>("Images/Sprites/UtilSquare2"));
            squareImgs.Add(Content.Load<Texture2D>("Images/Sprites/UtilSquare3"));
            squareImgs.Add(Content.Load<Texture2D>("Images/Sprites/UtilSquare4"));

            //Load the horse gallop properties
            potentialSquarePoss = new Vector2[4];
            coolDown = 5;
            itemSnd = Content.Load<SoundEffect>("Audio/Sounds/HorseGallopSound");
            utilityItem = FileManager.HORSE_GALLOP;
        }

        //Pre: The current player
        //Post: None
        //Desc: Set the potential squares of the horse gallop
        public override void SetPotentialSquares(Player curPlayer)
        {
            //Set the potential horse gallop positions
            potentialSquarePoss[TOP_LEFT] = new Vector2(curPlayer.curRow - 1, curPlayer.curCol - 3);
            potentialSquarePoss[TOP_RIGHT] = new Vector2(curPlayer.curRow + 3, curPlayer.curCol - 1);
            potentialSquarePoss[BOTTOM_LEFT] = new Vector2(curPlayer.curRow - 3, curPlayer.curCol + 1);
            potentialSquarePoss[BOTTOM_RIGHT] = new Vector2(curPlayer.curRow + 1, curPlayer.curCol + 3);

            //Convert all the potential square locations into bounding boxes
            SetBoundingBoxes();
        }
    }
}