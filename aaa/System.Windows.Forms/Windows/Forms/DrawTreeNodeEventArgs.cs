using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020003D5 RID: 981
	public class DrawTreeNodeEventArgs : EventArgs
	{
		// Token: 0x06003AF7 RID: 15095 RVA: 0x000D5BC9 File Offset: 0x000D4BC9
		public DrawTreeNodeEventArgs(Graphics graphics, TreeNode node, Rectangle bounds, TreeNodeStates state)
		{
			this.graphics = graphics;
			this.node = node;
			this.bounds = bounds;
			this.state = state;
			this.drawDefault = false;
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06003AF8 RID: 15096 RVA: 0x000D5BF5 File Offset: 0x000D4BF5
		// (set) Token: 0x06003AF9 RID: 15097 RVA: 0x000D5BFD File Offset: 0x000D4BFD
		public bool DrawDefault
		{
			get
			{
				return this.drawDefault;
			}
			set
			{
				this.drawDefault = value;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06003AFA RID: 15098 RVA: 0x000D5C06 File Offset: 0x000D4C06
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06003AFB RID: 15099 RVA: 0x000D5C0E File Offset: 0x000D4C0E
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06003AFC RID: 15100 RVA: 0x000D5C16 File Offset: 0x000D4C16
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06003AFD RID: 15101 RVA: 0x000D5C1E File Offset: 0x000D4C1E
		public TreeNodeStates State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x04001D73 RID: 7539
		private readonly Graphics graphics;

		// Token: 0x04001D74 RID: 7540
		private readonly TreeNode node;

		// Token: 0x04001D75 RID: 7541
		private readonly Rectangle bounds;

		// Token: 0x04001D76 RID: 7542
		private readonly TreeNodeStates state;

		// Token: 0x04001D77 RID: 7543
		private bool drawDefault;
	}
}
