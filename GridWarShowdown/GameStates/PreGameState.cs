// Author: Noah Teitlebaum
// File Name: PreGameState.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the pre game state

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
    class PreGameState
    {
        //Store the UI Spacing
        private const int FIGHT_BUTTON_VERT = 200;
        private const int WARNING_TEXT_VERT = 270;
        private const int VERT_SPACER = 150;
        private const int HORIZ_SPACER = 250;

        private const int ITEM_TEXT_VERT = 240;
        private const int ITEM_TEXT_HORIZ = 255;
        private const int ITEM_IMG_VERT = 250;
        private const int ITEM1_IMG_HORIZ = 305;
        private const int ITEM2_IMG_HORIZ = 195;
        private const int COOLDOWN_CIRCLE_HORIZ = 35;
        private const int COOLDOWN_CIRCLE_VERT = 90;
        private const int COOLDOWN_NUMBER_SPACER = 5;

        private const int QUEUE_GRID_HORIZ = 15;
        private const int QUEUE_GRID_VERT = 150;
        private const int GRID_TEXT_HORIZ = 190;
        private const int GRID_TEXT_VERT = 35;

        private const int ARROW_VERT = 275;
        private const int ARROW1_LEFT_HORIZ = 10;
        private const int ARROW1_RIGHT_HORIZ = 340;
        private const int ARROW2_LEFT_HORIZ = 510;
        private const int ARROW2_RIGHT_HORIZ = 840;

        //Store the on and off image values
        private const int OFF = 0;
        private const int ON = 1;

        //Store the player values
        private const int PLAYER1 = 0;
        private const int PLAYER2 = 1;

        //Store the max items and deaths
        private const int MAX_ITEMS = 5;
        private const int MAX_AMOUNT_DEATHS = 3;
        private const int MIN_AMOUNT_DEATHS = 1;

        //Store the UI sound effects
        private SoundEffect clickSnd;
        private SoundEffect addSnd;
        private SoundEffect removeSnd;
        public static SoundEffect fightSnd;

        //Store the text fonts
        private SpriteFont itemFont;
        private SpriteFont subTitleFont;
        private SpriteFont maxDeathFont;
        private SpriteFont fightFont;

        //Store the background image
        private Texture2D bg;

        //Store the button images
        private Texture2D[] increaseDeathButtonImgs = new Texture2D[2];
        private Texture2D[] decreaseDeathButtonImgs = new Texture2D[2];
        private Texture2D[] leftArrowImgs = new Texture2D[2];
        private Texture2D[] rightArrowImgs = new Texture2D[2];
        private Texture2D[] addButtonImgs = new Texture2D[2];
        private Texture2D[] removeButtonImgs = new Texture2D[2];
        private float buttonScalar = 0.7f;
        private double buttonRadiusDivisible = 3;

        //Store the buttons
        private Button fight;
        private Button increaseDeaths;
        private Button decreaseDeaths;
        private Button[] leftArrows = new Button[2];
        private Button[] rightArrows = new Button[2];
        private Button[] addQueue = new Button[2];
        private Button[] removeQueue = new Button[2];

        //Store the cool down circle properties
        private Texture2D coolDownCircleImg;
        private Rectangle[] coolDownCirlceRecs = new Rectangle[2];

        //Store the current item display properties
        public static CirclyDoublyLinkedList<ItemNode>[] itemChoices = new CirclyDoublyLinkedList<ItemNode>[2];
        private ItemNode[] currentItemChoice = new ItemNode[2];
        private Rectangle[] itemRecs = new Rectangle[2];
        private Vector2[] itemChoiceTextLocs = new Vector2[2];
        private Vector2[] currentCooldownTextLocs = new Vector2[2];
        private float itemScalar = 2f;

        //Store the item stack properties
        private Texture2D stackGridImg;
        private Rectangle[] stackGridRec = new Rectangle[2];
        public static ItemStack[] itemStacks = new ItemStack[2];

        //Store the text locations
        private Vector2[] itemLocs = new Vector2[2];
        private Vector2[] playerLocs = new Vector2[2];
        private Vector2 warningLoc;
        private Vector2 maxDeathLoc;
        private Vector2 instLoc;

        //Store the text
        private string itemText = "ITEMS";
        private string[] playerSubtitles = new string[] { "Player 1 Items", "Player 2 Items" };
        private string warningText = "*PLAYER ITEM QUEUES MUST BE FULL*";
        private string maxDeathsText = "MAX DEATHS: ";
        private string instText = "*PRESS |ESC| TO GO BACK*";

        //Store the max amount of deaths
        public static int maxDeaths = 2;

        //Store the warning message properties
        private bool warningOn = false;
        private float warningOpacity = 1f;

        //Pre: Used to load audio, fonts, images, and graphics
        //Post: None
        //Desc: Load the pre game state
        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            //Load the UI sound effects
            clickSnd = Content.Load<SoundEffect>("Audio/Sounds/ClickSound");
            addSnd = Content.Load<SoundEffect>("Audio/Sounds/AddItemSound");
            removeSnd = Content.Load<SoundEffect>("Audio/Sounds/RemoveItemSound");
            fightSnd = Content.Load<SoundEffect>("Audio/Sounds/FightSound");

            //Load the text fonts
            itemFont = Content.Load<SpriteFont>("Fonts/ItemFont");
            subTitleFont = Content.Load<SpriteFont>("Fonts/SubTitleFont");
            maxDeathFont = Content.Load<SpriteFont>("Fonts/OptionFont");
            fightFont = Content.Load<SpriteFont>("Fonts/SmallOptionFont");

            //Load the background image
            bg = Content.Load<Texture2D>("Images/Backgrounds/PreGameBackground");

            //Load the button images
            increaseDeathButtonImgs[OFF] = Content.Load<Texture2D>("Images/Sprites/IncreaseDeathOff");
            increaseDeathButtonImgs[ON] = Content.Load<Texture2D>("Images/Sprites/IncreaseDeathOn");
            decreaseDeathButtonImgs[OFF] = Content.Load<Texture2D>("Images/Sprites/DecreaseDeathOff");
            decreaseDeathButtonImgs[ON] = Content.Load<Texture2D>("Images/Sprites/DecreaseDeathOn");
            leftArrowImgs[OFF] = Content.Load<Texture2D>("Images/Sprites/LeftArrowOff");
            leftArrowImgs[ON] = Content.Load<Texture2D>("Images/Sprites/LeftArrowOn");
            rightArrowImgs[OFF] = Content.Load<Texture2D>("Images/Sprites/RightArrowOff");
            rightArrowImgs[ON] = Content.Load<Texture2D>("Images/Sprites/RightArrowOn");
            addButtonImgs[OFF] = Content.Load<Texture2D>("Images/Sprites/AddButtonOff");
            addButtonImgs[ON] = Content.Load<Texture2D>("Images/Sprites/AddButtonOn");
            removeButtonImgs[OFF] = Content.Load<Texture2D>("Images/Sprites/RemoveButtonOff");
            removeButtonImgs[ON] = Content.Load<Texture2D>("Images/Sprites/RemoveButtonOn");

            //Load the buttons
            fight = new Button(graphicsDevice, new Vector2(Game1.screenWidth / 2, FIGHT_BUTTON_VERT), fightFont, "FIGHT");
            increaseDeaths = new Button(increaseDeathButtonImgs[OFF], increaseDeathButtonImgs[ON], buttonScalar, new Vector2(825, 0), buttonRadiusDivisible);
            decreaseDeaths = new Button(decreaseDeathButtonImgs[OFF], decreaseDeathButtonImgs[ON], buttonScalar, new Vector2(50, 0), buttonRadiusDivisible);

            leftArrows[PLAYER1] = new Button(leftArrowImgs[OFF], leftArrowImgs[ON], buttonScalar, new Vector2(ARROW1_LEFT_HORIZ, ARROW_VERT), buttonRadiusDivisible);
            rightArrows[PLAYER1] = new Button(rightArrowImgs[OFF], rightArrowImgs[ON], buttonScalar, new Vector2(ARROW1_RIGHT_HORIZ, ARROW_VERT), buttonRadiusDivisible);
            addQueue[PLAYER1] = new Button(addButtonImgs[OFF], addButtonImgs[ON], buttonScalar, new Vector2(rightArrows[PLAYER1].GetRec().X, rightArrows[PLAYER1].GetRec().Y + VERT_SPACER), buttonRadiusDivisible);
            removeQueue[PLAYER1] = new Button(removeButtonImgs[OFF], removeButtonImgs[ON], buttonScalar, new Vector2(leftArrows[PLAYER1].GetRec().X, leftArrows[PLAYER1].GetRec().Y + VERT_SPACER), buttonRadiusDivisible);

            leftArrows[PLAYER2] = new Button(leftArrowImgs[OFF], leftArrowImgs[ON], buttonScalar, new Vector2(ARROW2_LEFT_HORIZ, ARROW_VERT), buttonRadiusDivisible);
            rightArrows[PLAYER2] = new Button(rightArrowImgs[OFF], rightArrowImgs[ON], buttonScalar, new Vector2(ARROW2_RIGHT_HORIZ, ARROW_VERT), buttonRadiusDivisible);
            addQueue[PLAYER2] = new Button(addButtonImgs[OFF], addButtonImgs[ON], buttonScalar, new Vector2(rightArrows[PLAYER2].GetRec().X, rightArrows[PLAYER2].GetRec().Y + VERT_SPACER), buttonRadiusDivisible);
            removeQueue[PLAYER2] = new Button(removeButtonImgs[OFF], removeButtonImgs[ON], buttonScalar, new Vector2(leftArrows[PLAYER2].GetRec().X, leftArrows[PLAYER2].GetRec().Y + VERT_SPACER), buttonRadiusDivisible);

            //Load the cool down circle image
            coolDownCircleImg = Content.Load<Texture2D>("Images/Sprites/CoolDownCircle");

            //Iterate through the item choices per player
            for (int i = 0; i < itemChoices.Length; i++)
            {
                //Load the linked lists
                itemChoices[i] = new CirclyDoublyLinkedList<ItemNode>();

                //Add all of the items to the linked list
                itemChoices[i].AddToTail(new ItemNode(new ShieldPotion(Content), "Shield Potion", Content.Load<Texture2D>("Images/Sprites/ShieldPotion")));
                itemChoices[i].AddToTail(new ItemNode(new InstantHeal(Content), "Instant Heal", Content.Load<Texture2D>("Images/Sprites/Heart")));
                itemChoices[i].AddToTail(new ItemNode(new MedicalKit(Content), "Medical Kit", Content.Load<Texture2D>("Images/Sprites/MedicalKit")));
                itemChoices[i].AddToTail(new ItemNode(new Sword(Content), "Sword", Content.Load<Texture2D>("Images/Sprites/Sword")));
                itemChoices[i].AddToTail(new ItemNode(new Torch(Content), "Torch", Content.Load<Texture2D>("Images/Sprites/Torch")));
                itemChoices[i].AddToTail(new ItemNode(new Dynamite(Content), "Dynamite", Content.Load<Texture2D>("Images/Sprites/Dynamite")));
                itemChoices[i].AddToTail(new ItemNode(new Crossbow(Content), "Crossbow", Content.Load<Texture2D>("Images/Sprites/Crossbow")));
                itemChoices[i].AddToTail(new ItemNode(new Boomerang(Content), "Boomerang", Content.Load<Texture2D>("Images/Sprites/Boomerang")));
                itemChoices[i].AddToTail(new ItemNode(new Harpoon(Content), "Harpoon", Content.Load<Texture2D>("Images/Sprites/Harpoon")));
                itemChoices[i].AddToTail(new ItemNode(new SwiftEscape(Content), "Swift Escape", Content.Load<Texture2D>("Images/Sprites/SwiftEscape")));
                itemChoices[i].AddToTail(new ItemNode(new HorseGallop(Content), "Horse Gallop", Content.Load<Texture2D>("Images/Sprites/HorseGallop")));
                itemChoices[i].AddToTail(new ItemNode(new GuardianShield(Content), "Guardian Shield", Content.Load<Texture2D>("Images/Sprites/GuardianShield")));

                //Set the current item choice to the first element in the list
                currentItemChoice[i] = itemChoices[i].head;
            }

            //Load the stack grid image
            stackGridImg = Content.Load<Texture2D>("Images/Sprites/StackGrid");

            //Iterate through the item stackss per player
            for (int i = 0; i < itemStacks.Length; i++)
            {
                //Set the item stacks and the grid rectangle
                stackGridRec[i] = new Rectangle(removeQueue[i].GetRec().Left - QUEUE_GRID_HORIZ, removeQueue[i].GetRec().Y + QUEUE_GRID_VERT, stackGridImg.Width, stackGridImg.Height);
                itemStacks[i] = new ItemStack(MAX_ITEMS, Content);            
            }

            //Iterate through the item text locations per player
            for (int i = 0; i < itemLocs.Length; i++)
            {
                //Set the item text locations
                itemLocs[i] = new Vector2(stackGridRec[i].X + GRID_TEXT_HORIZ, stackGridRec[i].Y - GRID_TEXT_VERT);
            }

            //Set the player subtitle locations
            playerLocs[PLAYER1] = new Vector2((Game1.screenWidth - subTitleFont.MeasureString(playerSubtitles[PLAYER1]).X) / 2 - HORIZ_SPACER, VERT_SPACER);
            playerLocs[PLAYER2] = new Vector2((Game1.screenWidth - subTitleFont.MeasureString(playerSubtitles[PLAYER2]).X) / 2 + HORIZ_SPACER, VERT_SPACER);

            //Set the UI text locations
            warningLoc = new Vector2((Game1.screenWidth - itemFont.MeasureString(warningText).X) / 2, WARNING_TEXT_VERT);
            maxDeathLoc = new Vector2((Game1.screenWidth - maxDeathFont.MeasureString(maxDeathsText + maxDeaths).X) / 2, 30);
            instLoc = new Vector2((Game1.screenWidth - itemFont.MeasureString(instText).X) / 2, 100);

            //Set up the item locations
            SetUpItems();
        }

        //Pre: None
        //Post: None
        //Desc: Update the pre game state
        public void Update()
        {
            //Check if the user pressed the escape button
            if (Game1.kb.IsKeyDown(Keys.Escape) && !Game1.prevKb.IsKeyDown(Keys.Escape))
            {
                //Go back to the previous state
                Game1.gameState = Game1.prevGameState;
            }

            //Check if the warning message is on
            if (warningOn)
            {
                //Make the warning message fade out
                warningOpacity -= 0.01f;

                //Check if the fade has ended
                if (warningOpacity <= 0)
                {
                    //Reset the warning message
                    warningOn = false;
                    warningOpacity = 1f;
                }
            }

            //Check if the user pressed the left mouse button
            if (Game1.mouse.LeftButton == ButtonState.Pressed && Game1.prevMouse.LeftButton != ButtonState.Pressed)
            {
                //Check if any button is being hovered over
                if (fight.HoverStatus())
                {
                    //If the item statcks are not full
                    if (!itemStacks[PLAYER1].IsFull() || !itemStacks[PLAYER2].IsFull())
                    {
                        //Display the warning message
                        warningOn = true;
                        clickSnd.CreateInstance().Play();
                    }
                    else
                    {
                        //Set the max amount of deaths and the item stacks
                        PlayState.roundManagement.maxDeaths = maxDeaths;
                        itemStacks[PLAYER1].SetPlayGameStack(PLAYER1);
                        itemStacks[PLAYER2].SetPlayGameStack(PLAYER2);

                        //Enter the play state
                        Game1.prevGameState = Game1.gameState;
                        Game1.gameState = Game1.PLAY_STATE;

                        //Play the fighting music
                        MediaPlayer.Play(Game1.fightMusic);
                        fightSnd.CreateInstance().Play();
                    }
                }
                else if (increaseDeaths.HoverStatus() && maxDeaths != MAX_AMOUNT_DEATHS)
                {
                    //Increase the amount of deaths
                    maxDeaths++;
                    clickSnd.CreateInstance().Play();
                }
                else if (decreaseDeaths.HoverStatus() && maxDeaths != MIN_AMOUNT_DEATHS)
                {
                    //Decrease the amount of deaths
                    maxDeaths--;
                    clickSnd.CreateInstance().Play();
                }

                //Iterate through each player
                for (int i = 0; i <= PLAYER2; i++)
                {
                    //Check if any arrow button is being hovered over
                    if (leftArrows[i].HoverStatus())
                    {
                        //Go to the previous item choice
                        currentItemChoice[i] = currentItemChoice[i].prev;
                        clickSnd.CreateInstance().Play();
                    }
                    else if (rightArrows[i].HoverStatus())
                    {
                        //Go to the next item choice
                        currentItemChoice[i] = currentItemChoice[i].next;
                        clickSnd.CreateInstance().Play();
                    }

                    //Check if the add or remove buttons are being hovered over
                    if (addQueue[i].HoverStatus() && !itemStacks[i].IsFull())
                    {
                        //Add the current item choice to the item stack
                        itemStacks[i].Push(currentItemChoice[i]);
                        currentItemChoice[i] = itemChoices[i].Delete(currentItemChoice[i]);
                        addSnd.CreateInstance().Play();
                    }
                    if (removeQueue[i].HoverStatus() && !itemStacks[i].IsEmpty())
                    {
                        //Remove the current item choice to the item stack
                        itemChoices[i].AddToHead(itemStacks[i].Pop());
                        currentItemChoice[i] = itemChoices[i].head;
                        removeSnd.CreateInstance().Play();
                    }
                }
            }

            //Set up the item locations
            SetUpItems();
        }

        //Pre: None
        //Post: None
        //Desc: Set up the item locations
        private void SetUpItems()
        {
            //Set up the player 1 item bounding boxes, text location, and cool down location
            itemRecs[PLAYER1] = new Rectangle((Game1.screenWidth - currentItemChoice[PLAYER1].img.Width) / 2 - ITEM1_IMG_HORIZ, ITEM_IMG_VERT,
                                (int)(itemScalar * currentItemChoice[PLAYER1].img.Width), (int)(itemScalar * currentItemChoice[PLAYER1].img.Height));
            coolDownCirlceRecs[PLAYER1] = new Rectangle(itemRecs[PLAYER1].Center.X - COOLDOWN_CIRCLE_HORIZ, itemRecs[PLAYER1].Center.Y + COOLDOWN_CIRCLE_VERT,
                                          coolDownCircleImg.Width, coolDownCircleImg.Height);

            itemChoiceTextLocs[PLAYER1] = new Vector2((Game1.screenWidth - itemFont.MeasureString(currentItemChoice[PLAYER1].text).X) / 2 - ITEM_TEXT_HORIZ, ITEM_TEXT_VERT);
            currentCooldownTextLocs[PLAYER1] = new Vector2(coolDownCirlceRecs[PLAYER1].Center.X - COOLDOWN_NUMBER_SPACER, coolDownCirlceRecs[PLAYER1].Center.Y - COOLDOWN_NUMBER_SPACER);

            //Set up the player 2 item bounding boxes, text location, and cool down location
            itemRecs[PLAYER2] = new Rectangle((Game1.screenWidth - currentItemChoice[PLAYER2].img.Width) / 2 + ITEM2_IMG_HORIZ, ITEM_IMG_VERT,
                                (int)(itemScalar * currentItemChoice[PLAYER2].img.Width), (int)(itemScalar * currentItemChoice[PLAYER2].img.Height));
            coolDownCirlceRecs[PLAYER2] = new Rectangle(itemRecs[PLAYER2].Center.X - COOLDOWN_CIRCLE_HORIZ, itemRecs[PLAYER2].Center.Y + COOLDOWN_CIRCLE_VERT,
                          coolDownCircleImg.Width, coolDownCircleImg.Height);

            itemChoiceTextLocs[PLAYER2] = new Vector2((Game1.screenWidth - itemFont.MeasureString(currentItemChoice[PLAYER2].text).X) / 2 + ITEM_TEXT_HORIZ, ITEM_TEXT_VERT);
            currentCooldownTextLocs[PLAYER2] = new Vector2(coolDownCirlceRecs[PLAYER2].Center.X - COOLDOWN_NUMBER_SPACER, coolDownCirlceRecs[PLAYER2].Center.Y - COOLDOWN_NUMBER_SPACER);
        }

        //Pre: None
        //Post: None
        //Desc: Pop all the items from the stack back to the linked list
        public static void PopAll()
        {
            //Iterate through the itam stacks per player
            for (int i = 0; i < itemStacks.Length; i++)
            {
                //Check if there are items in the stack
                if (!itemStacks[i].IsEmpty())
                {
                    //Add the items from the stack back to the linked list
                    itemChoices[i].AddToHead(itemStacks[i].Pop());

                    //Recurse through the method again
                    PopAll();
                }
            }
        }

        //Pre: Used to draw various sprites
        //Post: None
        //Desc: Draw the pre game state
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the scolling menu background
            Effects.DrawParallaxBg(spriteBatch, bg);

            //Draw the fight and death buttons
            fight.DisplayStringButton(spriteBatch);
            increaseDeaths.DisplayButton(spriteBatch);
            decreaseDeaths.DisplayButton(spriteBatch);

            //Iterate through each player
            for (int i = 0; i <= PLAYER2; i++)
            {
                //Draw the buttons each player has access to
                leftArrows[i].DisplayButton(spriteBatch);
                rightArrows[i].DisplayButton(spriteBatch);
                addQueue[i].DisplayButton(spriteBatch);
                removeQueue[i].DisplayButton(spriteBatch);

                //Draw the current item choice and the cool down image
                spriteBatch.Draw(currentItemChoice[i].img, itemRecs[i], Color.White);
                spriteBatch.Draw(coolDownCircleImg, coolDownCirlceRecs[i], Color.White * 0.8f);

                //Draw the current item choice's name and its cool down cost
                Effects.OverlapTextDisplay(spriteBatch, itemFont, currentItemChoice[i].text, itemChoiceTextLocs[i], Color.Black, Color.MintCream);
                Effects.OverlapTextDisplay(spriteBatch, itemFont, "" + currentItemChoice[i].item.GetCoolDown(), currentCooldownTextLocs[i], Color.Black, Color.MintCream);

                //Draw the stack grid images and the items currently in the item stack
                spriteBatch.Draw(stackGridImg, stackGridRec[i], Color.White);
                itemStacks[i].DisplayPreGameStack(spriteBatch, new Vector2(stackGridRec[i].X, stackGridRec[i].Y));

                //Draw the item subtitles
                Effects.OverlapTextDisplay(spriteBatch, subTitleFont, itemText, itemLocs[i], Color.Black, Color.Chocolate);
            }

            //Draw the player subtitles
            Effects.OverlapTextDisplay(spriteBatch, subTitleFont, playerSubtitles[PLAYER1], playerLocs[PLAYER1], Color.Black, Color.Crimson);
            Effects.OverlapTextDisplay(spriteBatch, subTitleFont, playerSubtitles[PLAYER2], playerLocs[PLAYER2], Color.Black, Color.DodgerBlue);

            //Check if the warning occurred
            if (warningOn)
            {
                //Draw the warning message
                Effects.OverlapTextDisplay(spriteBatch, itemFont, warningText, warningLoc, Color.Black * warningOpacity, Color.Red * warningOpacity);
            }

            //Draw the UI texts
            Effects.OverlapTextDisplay(spriteBatch, maxDeathFont, maxDeathsText + maxDeaths, maxDeathLoc, Color.Black, Color.Chocolate);
            Effects.OverlapTextDisplay(spriteBatch, itemFont, instText, instLoc, Color.Black, Color.Yellow);
        }
    }
}