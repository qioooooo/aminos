using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x02000076 RID: 118
	[Serializable]
	public class NoNullAllowedException : DataException
	{
		// Token: 0x060005B7 RID: 1463 RVA: 0x001DA394 File Offset: 0x001D9794
		protected NoNullAllowedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x001DA3AC File Offset: 0x001D97AC
		public NoNullAllowedException()
			: base(Res.GetString("DataSet_DefaultNoNullAllowedException"))
		{
			base.HResult = -2146232026;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x001DA3D4 File Offset: 0x001D97D4
		public NoNullAllowedException(string s)
			: base(s)
		{
			base.HResult = -2146232026;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x001DA3F4 File Offset: 0x001D97F4
		public NoNullAllowedException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232026;
		}
	}
}
