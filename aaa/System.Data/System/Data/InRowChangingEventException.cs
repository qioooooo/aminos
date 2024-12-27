using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public class InRowChangingEventException : DataException
	{
		// Token: 0x060005AB RID: 1451 RVA: 0x001DA214 File Offset: 0x001D9614
		protected InRowChangingEventException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x001DA22C File Offset: 0x001D962C
		public InRowChangingEventException()
			: base(Res.GetString("DataSet_DefaultInRowChangingEventException"))
		{
			base.HResult = -2146232029;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x001DA254 File Offset: 0x001D9654
		public InRowChangingEventException(string s)
			: base(s)
		{
			base.HResult = -2146232029;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x001DA274 File Offset: 0x001D9674
		public InRowChangingEventException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232029;
		}
	}
}
