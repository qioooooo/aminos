using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x0200017E RID: 382
	internal class ActiveAxis
	{
		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x00056F70 File Offset: 0x00055F70
		public int CurrentDepth
		{
			get
			{
				return this.currentDepth;
			}
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x00056F78 File Offset: 0x00055F78
		internal void Reactivate()
		{
			this.isActive = true;
			this.currentDepth = -1;
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x00056F88 File Offset: 0x00055F88
		internal ActiveAxis(Asttree axisTree)
		{
			this.axisTree = axisTree;
			this.currentDepth = -1;
			this.axisStack = new ArrayList(axisTree.SubtreeArray.Count);
			foreach (object obj in axisTree.SubtreeArray)
			{
				ForwardAxis forwardAxis = (ForwardAxis)obj;
				AxisStack axisStack = new AxisStack(forwardAxis, this);
				this.axisStack.Add(axisStack);
			}
			this.isActive = true;
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x00057020 File Offset: 0x00056020
		public bool MoveToStartElement(string localname, string URN)
		{
			if (!this.isActive)
			{
				return false;
			}
			this.currentDepth++;
			bool flag = false;
			foreach (object obj in this.axisStack)
			{
				AxisStack axisStack = (AxisStack)obj;
				if (axisStack.Subtree.IsSelfAxis)
				{
					if (axisStack.Subtree.IsDss || this.CurrentDepth == 0)
					{
						flag = true;
					}
				}
				else if (this.CurrentDepth != 0 && axisStack.MoveToChild(localname, URN, this.currentDepth))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x000570D0 File Offset: 0x000560D0
		public virtual bool EndElement(string localname, string URN)
		{
			if (this.currentDepth == 0)
			{
				this.isActive = false;
				this.currentDepth--;
			}
			if (!this.isActive)
			{
				return false;
			}
			foreach (object obj in this.axisStack)
			{
				AxisStack axisStack = (AxisStack)obj;
				axisStack.MoveToParent(localname, URN, this.currentDepth);
			}
			this.currentDepth--;
			return false;
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x00057168 File Offset: 0x00056168
		public bool MoveToAttribute(string localname, string URN)
		{
			if (!this.isActive)
			{
				return false;
			}
			bool flag = false;
			foreach (object obj in this.axisStack)
			{
				AxisStack axisStack = (AxisStack)obj;
				if (axisStack.MoveToAttribute(localname, URN, this.currentDepth + 1))
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x04000C5B RID: 3163
		private int currentDepth;

		// Token: 0x04000C5C RID: 3164
		private bool isActive;

		// Token: 0x04000C5D RID: 3165
		private Asttree axisTree;

		// Token: 0x04000C5E RID: 3166
		private ArrayList axisStack;
	}
}
