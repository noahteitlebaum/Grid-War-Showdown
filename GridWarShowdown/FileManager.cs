// Author: Noah Teitlebaum
// File Name: FileManager.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control all the functionality behind the game statistics

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GridWarShowdown
{
    class FileManager
    {
        //Store the player values
        private const int PLAYER1 = 0;
        private const int PLAYER2 = 1;

        //Store the item values
        public const int SHIELD_POTION = 0;
        public const int INSTANT_HEAL = 1;
        public const int MEDICAL_KIT = 2;
        public const int SWORD = 3;
        public const int TORCH = 4;
        public const int DYNAMITE = 5;
        public const int CROSSBOW = 6;
        public const int HARPOON = 7;
        public const int BOOMERANG = 8;
        public const int SWIFT_ESCAPE = 9;
        public const int HORSE_GALLOP = 10;
        public const int GUARDIAN_SHIELD = 11;

        //Store the writing to file variable
        private StreamWriter outFile;

        //Store the Reading to file variable
        private StreamReader inFile;
        private string[] data;

        //Store the file properties
        public int[] playerWins;
        public int[] totalHealthPoints;
        public int[] totalItemUse;

        //Store if an error occurred
        public bool errorOccurred;

        /// <summary>
        /// Used to store the player file properties
        /// </summary>
        /// <param name="maxPlayerWins">Store the max amount of players that keep track of the wins</param>
        public FileManager(int maxPlayerWins)
        {
            //Set the bounds of the player win array
            playerWins = new int[maxPlayerWins];

            //Read the file for only player statistics
            ReadFile(true, false);
        }

        /// <summary>
        /// Used to store the item file properties
        /// </summary>
        /// <param name="maxTotalHealthPoints">Store the max amount of items that keep track of the total health points dealt / gained</param>
        /// <param name="maxTotalItemUse">Store the max amount of items that keep track of the total use</param>
        public FileManager(int maxTotalHealthPoints, int maxTotalItemUse)
        {
            //Set the bounds of the item arrays
            totalHealthPoints = new int[maxTotalHealthPoints];
            totalItemUse = new int[maxTotalItemUse];

            //Read the file for only item statistics
            ReadFile(false, true);
        }

        /// <summary>
        /// Used to store both the item and player file properties
        /// </summary>
        /// <param name="maxPlayerWins">Store the max amount of players that keep track of the wins</param>
        /// <param name="maxTotalHealthPoints">Store the max amount of items that keep track of the total health points dealt / gained</param>
        /// <param name="maxTotalItemUse">Store the max amount of items that keep track of the total use</param>
        public FileManager(int maxPlayerWins, int maxTotalHealthPoints, int maxTotalItemUse)
        {
            //Set the bounds of the item and player arrays
            playerWins = new int[maxPlayerWins];
            totalHealthPoints = new int[maxTotalHealthPoints];
            totalItemUse = new int[maxTotalItemUse];
        }

        //Pre: The current player who won the game
        //Post: None
        //Desc: Increment the amount of wins of the current winner
        public void AddPlayerWon(int playerWon)
        {
            playerWins[playerWon]++;
        }

        //Pre: The current item and how much health it gained / dealt
        //Post: None
        //Desc: Increment the amount of health gained / dealt for the current item
        public void AddTotalHealthPoints(int curItem, int itemHealth)
        {
            totalHealthPoints[curItem] += itemHealth;
        }

        //Pre: The current item
        //Post: None
        //Desc: Increment the amount of use for the current item
        public void AddTotalItemUse(int curItem)
        {
            totalItemUse[curItem]++;
        }

        //Pre: What sections of the file is available to read
        //Post: None
        //Desc: Read the current file
        public void ReadFile(bool isPlayerWins, bool isItemStats)
        {
            //Try to execute if an error does not occur
            try
            {
                //Set an error occurred to false
                errorOccurred = false;

                //Open the text file
                inFile = File.OpenText("Stats.txt");
                data = inFile.ReadLine().Split(',');

                //Check if the player win section of the file is available to read
                if (isPlayerWins)
                {
                    //Iterate through each player
                    for (int i = 0; i < playerWins.Length; i++)
                    {
                        //Read the player win data
                        playerWins[i] = Convert.ToInt32(data[i]);
                    }
                }

                //Check if the item section of the file is available to read
                if (isItemStats)
                {
                    //Read onto the next line
                    data = inFile.ReadLine().Split(',');

                    //Iterate through each available item health gained / dealt
                    for (int i = 0; i < totalHealthPoints.Length; i++)
                    {
                        //Read the item health gained / dealt data
                        totalHealthPoints[i] = Convert.ToInt32(data[i]);
                    }

                    //Read onto the next line
                    data = inFile.ReadLine().Split(',');

                    //Iterate through each available item use
                    for (int i = 0; i < totalItemUse.Length; i++)
                    {
                        //Read the item use data
                        totalItemUse[i] = Convert.ToInt32(data[i]);
                    }
                }

                //Close reading in the data
                inFile.Close();
            }
            catch (Exception)
            {
                //Set an error occurred to true
                errorOccurred = true;
            }
        }

        //Pre: If the file should be created or appended
        //Post: None
        //Desc: Write to a file
        public void WriteFile(bool append)
        {
            //Checck if the file should be created or appended
            if (!append)
            {
                //Write the current player win data
                outFile = File.CreateText("Stats.txt");
                outFile.WriteLine(playerWins[PLAYER1] + "," + playerWins[PLAYER2]);
            }
            else
            {
                //Append the text file
                outFile = File.AppendText("Stats.txt");

                //Iterate through each available item health gained / dealt
                for (int i = 0; i < totalHealthPoints.Length; i++)
                {
                    //Check to see if the iteration is not on the last rotation
                    if (i < totalHealthPoints.Length - 1)
                    {
                        //Write the item health gained / dealt data with a split
                        outFile.Write(totalHealthPoints[i] + ",");
                    }
                    else
                    {
                        //Write the item health gained / dealt data without a split
                        outFile.WriteLine(totalHealthPoints[i]);
                    }
                }

                //Iterate through each available item use
                for (int i = 0; i < totalItemUse.Length; i++)
                {
                    //Check to see if the iteration is not on the last rotation
                    if (i < totalItemUse.Length - 1)
                    {
                        //Write the item use data with a split
                        outFile.Write(totalItemUse[i] + ",");
                    }
                    else
                    {
                        //Write the item use data without a split
                        outFile.WriteLine(totalItemUse[i]);
                    }
                }
            }

            //Close writing the data
            outFile.Close();
        }
    }
}