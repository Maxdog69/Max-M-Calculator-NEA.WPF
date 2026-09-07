using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace Calculator.WPF.Models
{
    // files includes: nodes, stacks, queue



    public class MyNode<T>
    {
        public T data;
        public MyNode<T>? Next;

        public MyNode(T value)
        {
            data = value;
            Next = null;
        }

    }


    public class MyStack<T>
    {
        private MyNode<T>? Top;

        public void Push(T item)
        {
            if (Top == null)
            {
                Top = new MyNode<T>(item);
            }
            else
            {
                MyNode<T> temp = Top;
                Top = new MyNode<T>(item);
                Top.Next = temp;

            }
        }

        public T Pop()
        {

            if (Top == null)
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            var temp = Top;
            Top = Top.Next;
            return temp.data;
        }

        public void displayStack()
        {
            if (Top == null)
            {
                Console.WriteLine("null");
            }
            else
            {
                var current = Top;
                while (current.Next != null)
                {

                    Console.WriteLine(current.data.ToString());
                    current = current.Next;
                }
                Console.WriteLine(current.data.ToString());

            }

        }

        public T Peek()
        {
            if (Top == null)
            {
                throw new InvalidOperationException("Stack is empty");
            }

            return Top.data;
        }

        public bool isEmpty()
        {
            return Top == null;
        }

    }


    /*
     * We need a queue to store the previously held expressions in the display of the main screen. Need to think about more
     */
    public class MyQueue<T>
    {
        private MyNode<T>? _head;
        private MyNode<T>? _tail;
        private int _count;

        public void Enqueue(T value)
        {
            MyNode<T> newNode = new MyNode<T>(value);

            if (_tail == null) // tail is empty
            {
                _head = newNode;
                _tail = newNode;
            }

            else
            {
                _tail.Next = newNode;
                _tail = newNode;
            }

            _count++;

        }

        public int length()
        {
            return _count;
        }

        public T Dequeue()
        {

            if (_head == null)
            {
                throw new InvalidOperationException("queue is empty");
            }

            MyNode<T> temp = _head;
            _head = _head.Next;

            if (_head == null)
            {
                _tail = null;
            }

            _count--;

            return temp.data;

        }

        public void displayQueue()
        {
            if (_head == null)
            {
                Console.WriteLine("queue is empty");
            }
            else
            {
                var current = _head;
                while (current.Next != null)
                {

                    Console.WriteLine(current.data.ToString());
                    current = current.Next;
                }
                Console.WriteLine(current.data.ToString());
            }
        }

    }


}
