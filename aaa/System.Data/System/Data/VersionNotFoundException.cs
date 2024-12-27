using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x02000079 RID: 121
	[Serializable]
	public class VersionNotFoundException : DataException
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x001DA514 File Offset: 0x001D9914
		protected VersionNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x001DA52C File Offset: 0x001D992C
		public VersionNotFoundException()
			: base(Res.GetString("DataSet_DefaultVersionNotFoundException"))
		{
			base.HResult = -2146232023;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x001DA554 File Offset: 0x001D9954
		public VersionNotFoundException(string s)
			: base(s)
		{
			base.HResult = -2146232023;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x001DA574 File Offset: 0x001D9974
		public VersionNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232023;
		}
	}
}
