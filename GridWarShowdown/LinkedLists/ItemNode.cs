// Author: Noah Teitlebaum
// File Name: ItemNode.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the item nodes for any linked lists

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
    class ItemNode : Node<ItemNode>
    {
        //Store the item node properties
        public Item item;
        public string text;
        public Texture2D img;

        /// <summary>
        /// Represents an item during the item selection phase
        /// </summary>
        /// <param name="item">Properties of the current item</param>
        /// <param name="text">The text display of the item name</param>
        /// <param name="img">The image display of what the item looks like</param>
        public ItemNode(Item item, string text, Texture2D img)
        {
            //Set the item node properties
            this.item = item;
            this.text = text;
            this.img = img;
        }
    }
}