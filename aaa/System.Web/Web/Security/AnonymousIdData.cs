using System;

namespace System.Web.Security
{
	// Token: 0x02000325 RID: 805
	[Serializable]
	internal class AnonymousIdData
	{
		// Token: 0x060027A1 RID: 10145 RVA: 0x000AD875 File Offset: 0x000AC875
		internal AnonymousIdData(string id, DateTime dt)
		{
			this.ExpireDate = dt;
			this.AnonymousId = ((dt > DateTime.UtcNow) ? id : null);
		}

		// Token: 0x04001E5F RID: 7775
		internal string AnonymousId;

		// Token: 0x04001E60 RID: 7776
		internal DateTime ExpireDate;
	}
}
