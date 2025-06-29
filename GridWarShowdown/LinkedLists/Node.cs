// Author: Noah Teitlebaum
// File Name: Node.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control the pointers for any linked lists

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridWarShowdown
{
    class Node<T> where T: Node<T>
    {
        //Store the next and previous nodes
        public T next;
        public T prev;

        /// <summary>
        /// An individual part of a larger data structure
        /// </summary>
        public Node()
        {
            //Set the next and previous nodes to nothing
            next = null;
            prev = null;
        }
    }
}