using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.WPF.Models.Editing
{
    public class Cursor
    {
        public Row Row { get; private set; }
        public int Index { get; private set; }

        public Cursor(Row row)
        {
            Row = row;
            Index = 0;
        }

        public void Insert(Node node)
        {
            Row.Nodes.Insert(Index, node);
            Index++;
        }

        public void Backspace()   
        {
            if (Index == 0) return;
            Row.Nodes.RemoveAt(Index - 1);
            Index--;
        }

        public void MoveLeft()
        {
            if (Index > 0)
            {
                Index--;
            }
        }

        public void MoveRight()
        {
            if (Index < Row.Nodes.Count)
            {
                Index++;
            }
        }

        public void MoveHome()
        {
            Index = 0;
        }

        public void MoveEnd()
        {
            Index = Row.Nodes.Count;
        }

    }
}
