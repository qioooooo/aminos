using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace System.ComponentModel.Design
{
	// Token: 0x020000FE RID: 254
	public abstract class DesignerActionItem
	{
		// Token: 0x06000A80 RID: 2688 RVA: 0x00028FCD File Offset: 0x00027FCD
		public DesignerActionItem(string displayName, string category, string description)
		{
			this.category = category;
			this.description = description;
			this.displayName = ((displayName == null) ? null : Regex.Replace(displayName, "\\(\\&.\\)", ""));
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00028FFF File Offset: 0x00027FFF
		internal DesignerActionItem()
		{
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x00029007 File Offset: 0x00028007
		// (set) Token: 0x06000A83 RID: 2691 RVA: 0x0002900F File Offset: 0x0002800F
		public bool AllowAssociate
		{
			get
			{
				return this.allowAssociate;
			}
			set
			{
				this.allowAssociate = value;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x00029018 File Offset: 0x00028018
		public virtual string Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x00029020 File Offset: 0x00028020
		public virtual string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x00029028 File Offset: 0x00028028
		public virtual string DisplayName
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x00029030 File Offset: 0x00028030
		public IDictionary Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new HybridDictionary();
				}
				return this.properties;
			}
		}

		// Token: 0x04000D90 RID: 3472
		private bool allowAssociate;

		// Token: 0x04000D91 RID: 3473
		private string displayName;

		// Token: 0x04000D92 RID: 3474
		private string description;

		// Token: 0x04000D93 RID: 3475
		private string category;

		// Token: 0x04000D94 RID: 3476
		private IDictionary properties;
	}
}
