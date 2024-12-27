using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x02000072 RID: 114
	[Serializable]
	public class DuplicateNameException : DataException
	{
		// Token: 0x060005A7 RID: 1447 RVA: 0x001DA194 File Offset: 0x001D9594
		protected DuplicateNameException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x001DA1AC File Offset: 0x001D95AC
		public DuplicateNameException()
			: base(Res.GetString("DataSet_DefaultDuplicateNameException"))
		{
			base.HResult = -2146232030;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x001DA1D4 File Offset: 0x001D95D4
		public DuplicateNameException(string s)
			: base(s)
		{
			base.HResult = -2146232030;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x001DA1F4 File Offset: 0x001D95F4
		public DuplicateNameException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232030;
		}
	}
}
