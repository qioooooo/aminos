using System;
using System.Drawing;

namespace System.Web.UI.Design
{
	// Token: 0x0200035E RID: 862
	public class DesignerRegion : DesignerObject
	{
		// Token: 0x06002054 RID: 8276 RVA: 0x000B6767 File Offset: 0x000B5767
		public DesignerRegion(ControlDesigner designer, string name)
			: this(designer, name, false)
		{
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x000B6772 File Offset: 0x000B5772
		public DesignerRegion(ControlDesigner designer, string name, bool selectable)
			: base(designer, name)
		{
			this._selectable = selectable;
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x000B6783 File Offset: 0x000B5783
		// (set) Token: 0x06002057 RID: 8279 RVA: 0x000B6799 File Offset: 0x000B5799
		public virtual string Description
		{
			get
			{
				if (this._description == null)
				{
					return string.Empty;
				}
				return this._description;
			}
			set
			{
				this._description = value;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06002058 RID: 8280 RVA: 0x000B67A2 File Offset: 0x000B57A2
		// (set) Token: 0x06002059 RID: 8281 RVA: 0x000B67B8 File Offset: 0x000B57B8
		public virtual string DisplayName
		{
			get
			{
				if (this._displayName == null)
				{
					return string.Empty;
				}
				return this._displayName;
			}
			set
			{
				this._displayName = value;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x0600205A RID: 8282 RVA: 0x000B67C1 File Offset: 0x000B57C1
		// (set) Token: 0x0600205B RID: 8283 RVA: 0x000B67C9 File Offset: 0x000B57C9
		public bool EnsureSize
		{
			get
			{
				return this._ensureSize;
			}
			set
			{
				this._ensureSize = value;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x0600205C RID: 8284 RVA: 0x000B67D2 File Offset: 0x000B57D2
		// (set) Token: 0x0600205D RID: 8285 RVA: 0x000B67DA File Offset: 0x000B57DA
		public virtual bool Highlight
		{
			get
			{
				return this._highlight;
			}
			set
			{
				this._highlight = value;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x000B67E3 File Offset: 0x000B57E3
		// (set) Token: 0x0600205F RID: 8287 RVA: 0x000B67EB File Offset: 0x000B57EB
		public virtual bool Selectable
		{
			get
			{
				return this._selectable;
			}
			set
			{
				this._selectable = value;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06002060 RID: 8288 RVA: 0x000B67F4 File Offset: 0x000B57F4
		// (set) Token: 0x06002061 RID: 8289 RVA: 0x000B67FC File Offset: 0x000B57FC
		public virtual bool Selected
		{
			get
			{
				return this._selected;
			}
			set
			{
				this._selected = value;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06002062 RID: 8290 RVA: 0x000B6805 File Offset: 0x000B5805
		// (set) Token: 0x06002063 RID: 8291 RVA: 0x000B680D File Offset: 0x000B580D
		public object UserData
		{
			get
			{
				return this._userData;
			}
			set
			{
				this._userData = value;
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000B6816 File Offset: 0x000B5816
		public Rectangle GetBounds()
		{
			return base.Designer.View.GetBounds(this);
		}

		// Token: 0x040017D4 RID: 6100
		public static readonly string DesignerRegionAttributeName = "_designerRegion";

		// Token: 0x040017D5 RID: 6101
		private string _displayName;

		// Token: 0x040017D6 RID: 6102
		private string _description;

		// Token: 0x040017D7 RID: 6103
		private object _userData;

		// Token: 0x040017D8 RID: 6104
		private bool _selectable;

		// Token: 0x040017D9 RID: 6105
		private bool _selected;

		// Token: 0x040017DA RID: 6106
		private bool _highlight;

		// Token: 0x040017DB RID: 6107
		private bool _ensureSize;
	}
}
