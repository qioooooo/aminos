using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x0200006F RID: 111
	[Serializable]
	public class DataException : SystemException
	{
		// Token: 0x0600059B RID: 1435 RVA: 0x001DA01C File Offset: 0x001D941C
		protected DataException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x001DA034 File Offset: 0x001D9434
		public DataException()
			: base(Res.GetString("DataSet_DefaultDataException"))
		{
			base.HResult = -2146232032;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x001DA05C File Offset: 0x001D945C
		public DataException(string s)
			: base(s)
		{
			base.HResult = -2146232032;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x001DA07C File Offset: 0x001D947C
		public DataException(string s, Exception innerException)
			: base(s, innerException)
		{
		}
	}
}
