using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x02000078 RID: 120
	[Serializable]
	public class RowNotInTableException : DataException
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x001DA494 File Offset: 0x001D9894
		protected RowNotInTableException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x001DA4AC File Offset: 0x001D98AC
		public RowNotInTableException()
			: base(Res.GetString("DataSet_DefaultRowNotInTableException"))
		{
			base.HResult = -2146232024;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x001DA4D4 File Offset: 0x001D98D4
		public RowNotInTableException(string s)
			: base(s)
		{
			base.HResult = -2146232024;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x001DA4F4 File Offset: 0x001D98F4
		public RowNotInTableException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232024;
		}
	}
}
