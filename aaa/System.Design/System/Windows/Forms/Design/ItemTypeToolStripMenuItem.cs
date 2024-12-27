using System;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000259 RID: 601
	internal class ItemTypeToolStripMenuItem : ToolStripMenuItem
	{
		// Token: 0x060016DD RID: 5853 RVA: 0x00075F66 File Offset: 0x00074F66
		public ItemTypeToolStripMenuItem(Type t)
		{
			this._itemType = t;
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x060016DE RID: 5854 RVA: 0x00075F80 File Offset: 0x00074F80
		public Type ItemType
		{
			get
			{
				return this._itemType;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x00075F88 File Offset: 0x00074F88
		// (set) Token: 0x060016E0 RID: 5856 RVA: 0x00075F90 File Offset: 0x00074F90
		public bool ConvertTo
		{
			get
			{
				return this.convertTo;
			}
			set
			{
				this.convertTo = value;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x00075F99 File Offset: 0x00074F99
		// (set) Token: 0x060016E2 RID: 5858 RVA: 0x00075FBA File Offset: 0x00074FBA
		public override Image Image
		{
			get
			{
				if (this._image == null)
				{
					this._image = ToolStripDesignerUtils.GetToolboxBitmap(this.ItemType);
				}
				return this._image;
			}
			set
			{
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x060016E3 RID: 5859 RVA: 0x00075FBC File Offset: 0x00074FBC
		// (set) Token: 0x060016E4 RID: 5860 RVA: 0x00075FC9 File Offset: 0x00074FC9
		public override string Text
		{
			get
			{
				return ToolStripDesignerUtils.GetToolboxDescription(this.ItemType);
			}
			set
			{
			}
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x00075FCB File Offset: 0x00074FCB
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.tbxItem = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04001301 RID: 4865
		private static string systemWindowsFormsNamespace = typeof(ToolStripItem).Namespace;

		// Token: 0x04001302 RID: 4866
		private static ToolboxItem invalidToolboxItem = new ToolboxItem();

		// Token: 0x04001303 RID: 4867
		private Type _itemType;

		// Token: 0x04001304 RID: 4868
		private bool convertTo;

		// Token: 0x04001305 RID: 4869
		private ToolboxItem tbxItem = ItemTypeToolStripMenuItem.invalidToolboxItem;

		// Token: 0x04001306 RID: 4870
		private Image _image;
	}
}
