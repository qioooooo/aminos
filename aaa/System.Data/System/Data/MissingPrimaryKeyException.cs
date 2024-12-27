using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x02000075 RID: 117
	[Serializable]
	public class MissingPrimaryKeyException : DataException
	{
		// Token: 0x060005B3 RID: 1459 RVA: 0x001DA314 File Offset: 0x001D9714
		protected MissingPrimaryKeyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x001DA32C File Offset: 0x001D972C
		public MissingPrimaryKeyException()
			: base(Res.GetString("DataSet_DefaultMissingPrimaryKeyException"))
		{
			base.HResult = -2146232027;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x001DA354 File Offset: 0x001D9754
		public MissingPrimaryKeyException(string s)
			: base(s)
		{
			base.HResult = -2146232027;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x001DA374 File Offset: 0x001D9774
		public MissingPrimaryKeyException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232027;
		}
	}
}
