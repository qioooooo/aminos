using System;
using System.ComponentModel;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005B4 RID: 1460
	[TypeConverter(typeof(HorizontalAlignConverter))]
	public enum HorizontalAlign
	{
		// Token: 0x04002A9D RID: 10909
		NotSet,
		// Token: 0x04002A9E RID: 10910
		Left,
		// Token: 0x04002A9F RID: 10911
		Center,
		// Token: 0x04002AA0 RID: 10912
		Right,
		// Token: 0x04002AA1 RID: 10913
		Justify
	}
}
