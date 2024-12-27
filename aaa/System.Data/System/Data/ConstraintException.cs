using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x02000070 RID: 112
	[Serializable]
	public class ConstraintException : DataException
	{
		// Token: 0x0600059F RID: 1439 RVA: 0x001DA094 File Offset: 0x001D9494
		protected ConstraintException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x001DA0AC File Offset: 0x001D94AC
		public ConstraintException()
			: base(Res.GetString("DataSet_DefaultConstraintException"))
		{
			base.HResult = -2146232022;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x001DA0D4 File Offset: 0x001D94D4
		public ConstraintException(string s)
			: base(s)
		{
			base.HResult = -2146232022;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x001DA0F4 File Offset: 0x001D94F4
		public ConstraintException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232022;
		}
	}
}
