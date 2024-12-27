using System;

namespace System.Web.UI
{
	// Token: 0x020003AD RID: 941
	internal class CodeBlockBuilder : ControlBuilder
	{
		// Token: 0x06002E2A RID: 11818 RVA: 0x000CF379 File Offset: 0x000CE379
		internal CodeBlockBuilder(CodeBlockType blockType, string content, int lineNumber, int column, VirtualPath virtualPath)
		{
			this._content = content;
			this._blockType = blockType;
			this._column = column;
			base.Line = lineNumber;
			base.VirtualPath = virtualPath;
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000CF3A6 File Offset: 0x000CE3A6
		public override object BuildObject()
		{
			return null;
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06002E2C RID: 11820 RVA: 0x000CF3A9 File Offset: 0x000CE3A9
		internal string Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06002E2D RID: 11821 RVA: 0x000CF3B1 File Offset: 0x000CE3B1
		internal CodeBlockType BlockType
		{
			get
			{
				return this._blockType;
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06002E2E RID: 11822 RVA: 0x000CF3B9 File Offset: 0x000CE3B9
		internal int Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x0400216D RID: 8557
		protected CodeBlockType _blockType;

		// Token: 0x0400216E RID: 8558
		protected string _content;

		// Token: 0x0400216F RID: 8559
		private int _column;
	}
}
