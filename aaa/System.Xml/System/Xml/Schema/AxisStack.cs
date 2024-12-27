using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x0200017D RID: 381
	internal class AxisStack
	{
		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x00056C81 File Offset: 0x00055C81
		internal ForwardAxis Subtree
		{
			get
			{
				return this.subtree;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x00056C89 File Offset: 0x00055C89
		internal int Length
		{
			get
			{
				return this.stack.Count;
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00056C96 File Offset: 0x00055C96
		public AxisStack(ForwardAxis faxis, ActiveAxis parent)
		{
			this.subtree = faxis;
			this.stack = new ArrayList();
			this.parent = parent;
			if (!faxis.IsDss)
			{
				this.Push(1);
			}
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00056CC8 File Offset: 0x00055CC8
		internal void Push(int depth)
		{
			AxisElement axisElement = new AxisElement(this.subtree.RootNode, depth);
			this.stack.Add(axisElement);
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00056CF4 File Offset: 0x00055CF4
		internal void Pop()
		{
			this.stack.RemoveAt(this.Length - 1);
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00056D09 File Offset: 0x00055D09
		internal static bool Equal(string thisname, string thisURN, string name, string URN)
		{
			if (thisURN == null)
			{
				if (URN != null && URN.Length != 0)
				{
					return false;
				}
			}
			else if (thisURN.Length != 0 && thisURN != URN)
			{
				return false;
			}
			return thisname.Length == 0 || !(thisname != name);
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x00056D44 File Offset: 0x00055D44
		internal void MoveToParent(string name, string URN, int depth)
		{
			if (this.subtree.IsSelfAxis)
			{
				return;
			}
			foreach (object obj in this.stack)
			{
				AxisElement axisElement = (AxisElement)obj;
				axisElement.MoveToParent(depth, this.subtree);
			}
			if (this.subtree.IsDss && AxisStack.Equal(this.subtree.RootNode.Name, this.subtree.RootNode.Urn, name, URN))
			{
				this.Pop();
			}
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x00056DF0 File Offset: 0x00055DF0
		internal bool MoveToChild(string name, string URN, int depth)
		{
			bool flag = false;
			if (this.subtree.IsDss && AxisStack.Equal(this.subtree.RootNode.Name, this.subtree.RootNode.Urn, name, URN))
			{
				this.Push(-1);
			}
			foreach (object obj in this.stack)
			{
				AxisElement axisElement = (AxisElement)obj;
				if (axisElement.MoveToChild(name, URN, depth, this.subtree))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x00056E98 File Offset: 0x00055E98
		internal bool MoveToAttribute(string name, string URN, int depth)
		{
			if (!this.subtree.IsAttribute)
			{
				return false;
			}
			if (!AxisStack.Equal(this.subtree.TopNode.Name, this.subtree.TopNode.Urn, name, URN))
			{
				return false;
			}
			bool flag = false;
			if (this.subtree.TopNode.Input == null)
			{
				return this.subtree.IsDss || depth == 1;
			}
			foreach (object obj in this.stack)
			{
				AxisElement axisElement = (AxisElement)obj;
				if (axisElement.isMatch && axisElement.CurNode == this.subtree.TopNode.Input)
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x04000C58 RID: 3160
		private ArrayList stack;

		// Token: 0x04000C59 RID: 3161
		private ForwardAxis subtree;

		// Token: 0x04000C5A RID: 3162
		private ActiveAxis parent;
	}
}
