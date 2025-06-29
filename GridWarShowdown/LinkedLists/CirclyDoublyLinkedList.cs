// Author: Noah Teitlebaum
// File Name: CirclyDoublyLinkedList.cs
// Project Name: GridWarShowdown
// Creation Date: Nov. 30, 2023
// Modified Date: Jan. 21, 2024
// Description: This class is meant to control a set of sequentially linked data types

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridWarShowdown
{
    class CirclyDoublyLinkedList<T> where T: Node<T>
    {
        //Store the head and tail of the linked list
        public T head;
        public T tail;

        /// <summary>
        /// A linked data structure that consists of a set of sequentially linked data types
        /// </summary>
        public CirclyDoublyLinkedList()
        {
            //Set the head and tail to null
            head = null;
            tail = null;
        }

        //Pre: A new node being added
        //Post: None
        //Desc: Adding a new node to the front of the list
        public void AddToHead(T newNode)
        {
            //Check if there are no elements currently in the list
            if (head == null)
            {
                //Set the first element to the new node
                head = newNode;
                newNode.next = newNode;
                newNode.prev = newNode;
            }
            else
            {
                //Set the first element in the list to the new node
                newNode.next = head;
                newNode.prev = head.prev;
                head.prev.next = newNode;
                head.prev = newNode;
                head = newNode;
            }
        }

        //Pre: A new node being added
        //Post: None
        //Desc: Adding a new node to the back of the list
        public void AddToTail(T newNode)
        {
            //Check if there are no elements currently in the list
            if (head == null)
            {
                //Set the first element to the new node
                head = newNode;
                tail = newNode;
            }
            else
            {
                //Set the last element to the new node
                tail.next = newNode;
                newNode.prev = tail;
                tail = newNode;
                tail.next = head;
                head.prev = tail;
            }
        }

        //Pre: The current node being deleted
        //Post: Receiving the current node that was just deleted
        //Desc: Deleting the current node
        public T Delete(T deleteNode)
        {
            //Check if there is nothing to delete
            if (head == null)
            {
                //Return nothing
                return null;
            }

            //Check if there is only one element in the list
            if (head == deleteNode && head.next == head)
            {
                //Delete the only element
                head = null;
                return null;
            }

            //Create a new instance of the previous and next nodes
            T prevNode = deleteNode.prev;
            T nextNode = deleteNode.next;

            //Check if the deleted node was the head
            if (head == deleteNode)
            {
                //Set the head to the next node
                head = nextNode;
            }

            //Set the next and previous node to the next / previous node
            prevNode.next = nextNode;
            nextNode.prev = prevNode;

            //Remove the delete nodes next and previous nodes
            deleteNode.next = null;
            deleteNode.prev = null;

            //Return the current deletion node
            return nextNode;
        }

        //Pre: None
        //Post: Receiving the current size of the linked list
        //Desc: Find the size of the linked list
        public int Size()
        {
            //Check if there are no elements in the list
            if (head == null)
            {
                //Return a size of zero
                return 0;
            }

            //Create a counter variable and a new instance of the head
            int count = 0;
            T current = head;

            //Check when the list is finished looping through the elements
            while (current.next != head)
            {
                //Increment the count and go on to the next node
                count++;
                current = current.next;
            }

            //Return the current size
            return count + 1;
        }
    }
}