using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Calculator.WPF.Models.Editing
{
    public class Cursor
    {
        public Row Row { get; private set; }
        public int Index { get; private set; }

        public Node NodeBefore => Index > 0 ? Row.Nodes[Index-1 ] : null;
        public Node NodeAfter => Index < Row.Nodes.Count ? Row.Nodes[Index] : null;


        public Cursor(Row row)
        {
            Row = row;
            Index = 0;
        }

        public void Insert(Node node)
        {
            Row.Insert(Index, node);
            Index++;

            if (node.Slots.Count > 0)
            {
                Row = node.Slots[0];
                Index = 0;
            }
        }

        public void Backspace()   
        {
            if (Index > 0)
            {
                Node prev = Row.Nodes[Index - 1];
                if (prev.Slots.Count > 0) EnterFromRight(prev);      // step in, don't delete
                else { Row.Nodes.RemoveAt(Index - 1); Index--; }
            }
            else if (Row.Owner != null)
            {
                Node owner = Row.Owner;
                if (AllSlotsEmpty(owner))
                {
                    Row parent = owner.ParentRow;
                    int at = parent.Nodes.IndexOf(owner);
                    parent.Nodes.RemoveAt(at);
                    Row = parent;
                    Index = at;
                }
                else MoveLeft();
            }
        }

        public void MoveLeft()
        {
            if (Index > 0)
            {
                Node prev = Row.Nodes[Index - 1];
                if (prev.Slots.Count > 0) EnterFromRight(prev);
                else Index--;
            }
            else if (Row.Owner != null)
            {
                Node owner = Row.Owner;
                int slot = SlotIndex(owner, Row);
                if (slot > 0) { Row = owner.Slots[slot - 1]; Index = Row.Nodes.Count; }
                else LeaveBefore(owner);
            }
        }

        public void MoveRight()
        {
            if (Index < Row.Nodes.Count)
            {
                Node next = Row.Nodes[Index];
                if (next.Slots.Count > 0)
                {
                    EnterFromLeft(next);
                }
                else
                {
                    Index++;
                }
            }

            else if (Row.Owner != null)
            {
                Node owner = Row.Owner;
                int slot = SlotIndex(owner, Row);
                if (slot + 1 < owner.Slots.Count)
                {
                    Row = owner.Slots[slot + 1];
                    Index = 0;
                }
                else
                {
                    LeaveAfter(owner);
                }
            }
        }

        public void MoveHome()
        {
            GoToRoot();
            Index = 0;
        }

        public void MoveEnd()
        {
            GoToRoot();
            Index = Row.Nodes.Count;
        }

        private void GoToRoot() { while (Row.Owner != null) Row = Row.Owner.ParentRow; }

        private void EnterFromLeft(Node c) { Row = c.Slots[0]; Index = 0; }
        private void EnterFromRight(Node c) { Row = c.Slots[c.Slots.Count - 1]; Index = Row.Nodes.Count; }
        private void LeaveAfter(Node c) { Row = c.ParentRow; Index = Row.Nodes.IndexOf(c) + 1; }
        private void LeaveBefore(Node c) { Row = c.ParentRow; Index = Row.Nodes.IndexOf(c); }

        private static int SlotIndex(Node owner, Row row)
        {
            for (int i = 0; i < owner.Slots.Count; i++)
                if (ReferenceEquals(owner.Slots[i], row)) return i;
            return -1;
        }

        private static bool AllSlotsEmpty(Node owner)
        {
            foreach (var slot in owner.Slots)
                if (slot.Nodes.Count > 0) return false;
            return true;
        }

    }
}
