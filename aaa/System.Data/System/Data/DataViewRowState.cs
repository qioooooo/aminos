using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x020000AD RID: 173
	[Flags]
	[Editor("Microsoft.VSDesigner.Data.Design.DataViewRowStateEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public enum DataViewRowState
	{
		// Token: 0x04000859 RID: 2137
		None = 0,
		// Token: 0x0400085A RID: 2138
		Unchanged = 2,
		// Token: 0x0400085B RID: 2139
		Added = 4,
		// Token: 0x0400085C RID: 2140
		Deleted = 8,
		// Token: 0x0400085D RID: 2141
		ModifiedCurrent = 16,
		// Token: 0x0400085E RID: 2142
		ModifiedOriginal = 32,
		// Token: 0x0400085F RID: 2143
		OriginalRows = 42,
		// Token: 0x04000860 RID: 2144
		CurrentRows = 22
	}
}
