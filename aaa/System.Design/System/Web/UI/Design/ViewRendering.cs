using System;

namespace System.Web.UI.Design
{
	// Token: 0x020003BA RID: 954
	public class ViewRendering
	{
		// Token: 0x06002329 RID: 9001 RVA: 0x000BE41B File Offset: 0x000BD41B
		public ViewRendering(string content, DesignerRegionCollection regions)
			: this(content, regions, true)
		{
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x000BE426 File Offset: 0x000BD426
		public ViewRendering(string content, DesignerRegionCollection regions, bool visible)
		{
			this._content = content;
			this._regions = regions;
			this._visible = visible;
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x0600232B RID: 9003 RVA: 0x000BE443 File Offset: 0x000BD443
		public string Content
		{
			get
			{
				if (this._content == null)
				{
					return string.Empty;
				}
				return this._content;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600232C RID: 9004 RVA: 0x000BE459 File Offset: 0x000BD459
		public DesignerRegionCollection Regions
		{
			get
			{
				if (this._regions == null)
				{
					this._regions = new DesignerRegionCollection();
				}
				return this._regions;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600232D RID: 9005 RVA: 0x000BE474 File Offset: 0x000BD474
		public bool Visible
		{
			get
			{
				return this._visible;
			}
		}

		// Token: 0x0400188E RID: 6286
		private string _content;

		// Token: 0x0400188F RID: 6287
		private DesignerRegionCollection _regions;

		// Token: 0x04001890 RID: 6288
		private bool _visible;
	}
}
