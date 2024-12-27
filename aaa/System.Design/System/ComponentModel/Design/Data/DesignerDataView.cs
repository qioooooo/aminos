using System;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x0200014B RID: 331
	public abstract class DesignerDataView : DesignerDataTableBase
	{
		// Token: 0x06000CA8 RID: 3240 RVA: 0x00030AC8 File Offset: 0x0002FAC8
		protected DesignerDataView(string name)
			: base(name)
		{
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00030AD1 File Offset: 0x0002FAD1
		protected DesignerDataView(string name, string owner)
			: base(name, owner)
		{
		}
	}
}
