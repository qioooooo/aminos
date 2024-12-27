using System;

namespace System.Xml.Schema
{
	// Token: 0x0200017C RID: 380
	internal class AxisElement
	{
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x00056AD0 File Offset: 0x00055AD0
		internal DoubleLinkAxis CurNode
		{
			get
			{
				return this.curNode;
			}
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x00056AD8 File Offset: 0x00055AD8
		internal AxisElement(DoubleLinkAxis node, int depth)
		{
			this.curNode = node;
			this.curDepth = depth;
			this.rootDepth = depth;
			this.isMatch = false;
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00056B0C File Offset: 0x00055B0C
		internal void SetDepth(int depth)
		{
			this.curDepth = depth;
			this.rootDepth = depth;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00056B2C File Offset: 0x00055B2C
		internal void MoveToParent(int depth, ForwardAxis parent)
		{
			if (depth != this.curDepth - 1)
			{
				if (depth == this.curDepth && this.isMatch)
				{
					this.isMatch = false;
				}
				return;
			}
			if (this.curNode.Input == parent.RootNode && parent.IsDss)
			{
				this.curNode = parent.RootNode;
				this.rootDepth = (this.curDepth = -1);
				return;
			}
			if (this.curNode.Input != null)
			{
				this.curNode = (DoubleLinkAxis)this.curNode.Input;
				this.curDepth--;
			}
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x00056BC8 File Offset: 0x00055BC8
		internal bool MoveToChild(string name, string URN, int depth, ForwardAxis parent)
		{
			if (Asttree.IsAttribute(this.curNode))
			{
				return false;
			}
			if (this.isMatch)
			{
				this.isMatch = false;
			}
			if (!AxisStack.Equal(this.curNode.Name, this.curNode.Urn, name, URN))
			{
				return false;
			}
			if (this.curDepth == -1)
			{
				this.SetDepth(depth);
			}
			else if (depth > this.curDepth)
			{
				return false;
			}
			if (this.curNode == parent.TopNode)
			{
				this.isMatch = true;
				return true;
			}
			DoubleLinkAxis doubleLinkAxis = (DoubleLinkAxis)this.curNode.Next;
			if (Asttree.IsAttribute(doubleLinkAxis))
			{
				this.isMatch = true;
				return false;
			}
			this.curNode = doubleLinkAxis;
			this.curDepth++;
			return false;
		}

		// Token: 0x04000C54 RID: 3156
		internal DoubleLinkAxis curNode;

		// Token: 0x04000C55 RID: 3157
		internal int rootDepth;

		// Token: 0x04000C56 RID: 3158
		internal int curDepth;

		// Token: 0x04000C57 RID: 3159
		internal bool isMatch;
	}
}
