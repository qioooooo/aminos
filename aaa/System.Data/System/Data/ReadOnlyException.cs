using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x02000077 RID: 119
	[Serializable]
	public class ReadOnlyException : DataException
	{
		// Token: 0x060005BB RID: 1467 RVA: 0x001DA414 File Offset: 0x001D9814
		protected ReadOnlyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x001DA42C File Offset: 0x001D982C
		public ReadOnlyException()
			: base(Res.GetString("DataSet_DefaultReadOnlyException"))
		{
			base.HResult = -2146232025;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x001DA454 File Offset: 0x001D9854
		public ReadOnlyException(string s)
			: base(s)
		{
			base.HResult = -2146232025;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x001DA474 File Offset: 0x001D9874
		public ReadOnlyException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232025;
		}
	}
}
