using System;
using System.Collections;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000055 RID: 85
	internal class ADSubstringFilter
	{
		// Token: 0x060001A3 RID: 419 RVA: 0x00007A6B File Offset: 0x00006A6B
		public ADSubstringFilter()
		{
			this.Initial = null;
			this.Final = null;
			this.Any = new ArrayList();
		}

		// Token: 0x0400018F RID: 399
		public string Name;

		// Token: 0x04000190 RID: 400
		public ADValue Initial;

		// Token: 0x04000191 RID: 401
		public ADValue Final;

		// Token: 0x04000192 RID: 402
		public ArrayList Any;
	}
}
