using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x02000074 RID: 116
	[Serializable]
	public class InvalidConstraintException : DataException
	{
		// Token: 0x060005AF RID: 1455 RVA: 0x001DA294 File Offset: 0x001D9694
		protected InvalidConstraintException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x001DA2AC File Offset: 0x001D96AC
		public InvalidConstraintException()
			: base(Res.GetString("DataSet_DefaultInvalidConstraintException"))
		{
			base.HResult = -2146232028;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x001DA2D4 File Offset: 0x001D96D4
		public InvalidConstraintException(string s)
			: base(s)
		{
			base.HResult = -2146232028;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x001DA2F4 File Offset: 0x001D96F4
		public InvalidConstraintException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232028;
		}
	}
}
