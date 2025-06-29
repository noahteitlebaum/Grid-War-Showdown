// Author: Noah Teitlebaum
// File Name: ItemStack.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the collection of items chosen

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
    class ItemStack
    {
        //Store the UI Spacing
        private const int ITEM_BOX_VERT = -30;
        private const int ITEM_SPACER = 5;

        //Store the player values
        private const int PLAYER1 = 0;
        private const int PLAYER2 = 1;

        //Store the stack size and list
        private int size;
        private ItemNode[] stack;

        //Store the coolDown text font
        private SpriteFont coolDownfont;

        //Store the stack images
        private Texture2D itemBoxImg;
        private Texture2D curItemBoxImg;
        private Texture2D coolDownCircleImg;

        //Store the stack bounding boxes
        private Rectangle[] itemBoxRecs;
        private Rectangle curItemBoxRec;
        private Rectangle[] circleRecs;

        //Store the stack positions for the cooldown and item images
        private Rectangle[] itemRecs;
        private Vector2[] coolDownPoss;

        /// <summary>
        /// Serves as a collection of items with a last in first out operation
        /// </summary>
        /// <param name="maxItems">Max amount of items in the stack</param>
        /// <param name="Content">Used to load audio, fonts, and images </param>
        public ItemStack(int maxItems, ContentManager Content)
        {
            //Set the stack list based of the max amount of items
            stack = new ItemNode[maxItems];
            size = 0;

            //Load the coolDown text font
            coolDownfont = Content.Load<SpriteFont>("Fonts/ItemFont");

            //Load the stack images
            itemBoxImg = Content.Load<Texture2D>("Images/Sprites/ItemBox");
            curItemBoxImg = Content.Load<Texture2D>("Images/Sprites/CurrentItemBox");
            coolDownCircleImg = Content.Load<Texture2D>("Images/Sprites/CoolDownCircle");

            //Set the stack properties based of the max amount of items
            itemBoxRecs = new Rectangle[maxItems];
            circleRecs = new Rectangle[maxItems];
            itemRecs = new Rectangle[maxItems];
            coolDownPoss = new Vector2[maxItems];
        }

        //Pre: The new node
        //Post: None
        //Desc: Adds the new node to the stack
        public void Push(ItemNode newNode)
        {
            //Check if the stack size is less than the max amount of items it can hold
            if (Size() < stack.Length)
            {
                //Add a new item to the stack
                stack[size] = newNode;
                size++;
            }
        }

        //Pre: None
        //Post: Receives the node being removed as an item node
        //Desc: Removes the current node from the stack
        public ItemNode Pop()
        {
            //Store the result variable
            ItemNode result = null;

            //Check if the stack is not empty
            if (!IsEmpty())
            {
                //Remove the current item from the stack
                result = stack[size - 1];
                size--;
            }

            //Return the removed node
            return result;
        }

        //Pre: None
        //Post: Receives the last node added as an item node
        //Desc: Checks the last item added to the stack
        public ItemNode Top()
        {
            //Store the result variable
            ItemNode result = null;

            //Check if the stack is not empty
            if (!IsEmpty())
            {
                //Receive the item last added to the stack
                result = stack[size - 1];
            }

            //Return the last added item
            return result;
        }

        //Pre: None
        //Post: Receives how many elements are in the collection as an int
        //Desc: Checks the size of the stack
        public int Size()
        {
            //Return the size of the stack
            return size;
        }

        //Pre: None
        //Post: Receives if there are zero elements in the collection
        //Desc: Checks if the stack is empty
        public bool IsEmpty()
        {
            //Return if the stack is empty
            return size == 0;
        }

        //Pre: None
        //Post: Receives if there are all the elements in the collection
        //Desc: Checks if the stack is full
        public bool IsFull()
        {
            //Return if the stack is full
            return size == stack.Length;
        }

        //Pre: None
        //Post: Receives the current item selected in the stack
        //Desc: Checks the current item choice of the user
        public Item CurrentItemChoice()
        {
            //Checks if the user pressed the up or down arrow keys
            if (Game1.kb.IsKeyDown(Keys.Down) && !Game1.prevKb.IsKeyDown(Keys.Down) && curItemBoxRec.Y != itemBoxRecs[itemBoxRecs.Length - 1].Y)
            {
                //Increase up the item stack
                curItemBoxRec.Y += curItemBoxImg.Height;
            }
            else if (Game1.kb.IsKeyDown(Keys.Up) && !Game1.prevKb.IsKeyDown(Keys.Up) && curItemBoxRec.Y != itemBoxRecs[0].Y)
            {
                //Decrease down the item stack
                curItemBoxRec.Y -= curItemBoxImg.Height;
            }

            //Iterate through the size of the item stack
            for (int i = 0; i < Size(); i++)
            {
                //Check which item is currently being hovered 
                if (curItemBoxRec.Y == itemBoxRecs[i].Y)
                {
                    //Return the current hovered item
                    return stack[i].item;
                }
            }

            //There was no item being hovered
            return null;
        }

        //Pre: Used to draw various sprites, and the current player position
        //Post: None
        //Desc: Displays the pre game stack of items
        public void DisplayPreGameStack(SpriteBatch spriteBatch, Vector2 pos)
        {
            //Set the item scalar
            float itemScalar = 0.65f;

            //Iterate through the size of the item stack
            for (int i = 0; i < size; i++)
            {
                //Draw the items in the item stack
                Vector2 itemPos = new Vector2(pos.X + 75 + (74 * i + 1), pos.Y + 19);
                spriteBatch.Draw(stack[i].img, new Rectangle((int)itemPos.X, (int)itemPos.Y, (int)(itemScalar * stack[i].img.Width), (int)(itemScalar * stack[i].img.Height)), Color.White);
            }
        }

        //Pre: The current player
        //Post: None
        //Desc: Sets the play game stack of items
        public void SetPlayGameStack(int player)
        {
            //Sets the item and cool down circle scalar
            float itemScalar = 1.2f;
            float circleScalar = 0.5f;

            //Iterate through the size of the item stack
            for (int i = 0; i < size; i++)
            {
                //Check which player is selected
                if (player == PLAYER1)
                {
                    //Set the item boxes for player 1
                    itemBoxRecs[i] = new Rectangle(20, ITEM_BOX_VERT + itemBoxImg.Height * (i + 1), itemBoxImg.Width, itemBoxImg.Height);
                }
                else if (player == PLAYER2)
                {
                    //Set the item boxes for player 2
                    itemBoxRecs[i] = new Rectangle(830, ITEM_BOX_VERT + itemBoxImg.Height * (i + 1), itemBoxImg.Width, itemBoxImg.Height);
                }

                //Set the item, cool down circle, and cool down cost locations
                itemRecs[i] = new Rectangle(itemBoxRecs[i].X + ITEM_SPACER, itemBoxRecs[i].Y - ITEM_SPACER, (int)(itemScalar * stack[i].img.Width), (int)(itemScalar * stack[i].img.Height));
                circleRecs[i] = new Rectangle(itemRecs[i].X + 85, itemRecs[i].Y + 80, (int)(circleScalar * coolDownCircleImg.Width), (int)(circleScalar * coolDownCircleImg.Height));
                coolDownPoss[i] = new Vector2(circleRecs[i].X + 13, circleRecs[i].Y + 10);
            }

            //Set the hovering item box to the first element in the stack
            curItemBoxRec = new Rectangle(itemBoxRecs[0].X, itemBoxRecs[0].Y, curItemBoxImg.Width, curItemBoxImg.Height);
        }

        //Pre: Used to draw various sprites, and the current player position
        //Post: None
        //Desc: Displays the play game stack of items
        public void DisplayPlayGameStack(SpriteBatch spriteBatch)
        {
            //Iterate through the size of the item stack
            for (int i = 0; i < size; i++)
            {
                //Draw the items in the item stack
                spriteBatch.Draw(stack[i].img, itemRecs[i], Color.White);

                //Draw the stack item boxes and cool down circle
                spriteBatch.Draw(itemBoxImg, itemBoxRecs[i], Color.White);
                spriteBatch.Draw(coolDownCircleImg, circleRecs[i], Color.White * 0.5f);

                //Draw the item cool down costs
                Effects.OverlapTextDisplay(spriteBatch, coolDownfont, "" + stack[i].item.GetCoolDown(), coolDownPoss[i], Color.Black, Color.MintCream);
            }
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Displays the hovering item box
        public void DisplayCurrentItemBox(SpriteBatch spriteBatch)
        {
            //Draw the hovering item box
            spriteBatch.Draw(curItemBoxImg, curItemBoxRec, Color.White);
        }
    }
}