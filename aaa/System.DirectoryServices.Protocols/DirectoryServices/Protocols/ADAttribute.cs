using System;
using System.Collections;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000056 RID: 86
	internal class ADAttribute
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x00007A8C File Offset: 0x00006A8C
		public ADAttribute()
		{
			this.Values = new ArrayList();
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007A9F File Offset: 0x00006A9F
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		// Token: 0x04000193 RID: 403
		public string Name;

		// Token: 0x04000194 RID: 404
		public ArrayList Values;
	}
}
