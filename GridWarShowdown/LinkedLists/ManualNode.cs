// Author: Noah Teitlebaum
// File Name: ManualNode.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the manual nodes for any linked lists

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
    class ManualNode : Node<ManualNode>
    {
        //Store the manual node properties
        public Texture2D page;
        public Rectangle pageRec;

        /// <summary>
        /// Represents a current page for the user when reading through information
        /// </summary>
        /// <param name="page">Information for the user being stored as an image</param>
        public ManualNode(Texture2D page)
        {
            //Set the manual node properties
            this.page = page;
            pageRec = new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight);
        }
    }
}