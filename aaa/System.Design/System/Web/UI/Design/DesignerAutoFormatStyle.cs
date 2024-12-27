using System;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x0200035A RID: 858
	public class DesignerAutoFormatStyle : Style
	{
		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x0600203B RID: 8251 RVA: 0x000B6601 File Offset: 0x000B5601
		// (set) Token: 0x0600203C RID: 8252 RVA: 0x000B6609 File Offset: 0x000B5609
		public VerticalAlign VerticalAlign
		{
			get
			{
				return this._verticalAlign;
			}
			set
			{
				this._verticalAlign = value;
			}
		}

		// Token: 0x040017CC RID: 6092
		private VerticalAlign _verticalAlign;
	}
}
