using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x02000071 RID: 113
	[Serializable]
	public class DeletedRowInaccessibleException : DataException
	{
		// Token: 0x060005A3 RID: 1443 RVA: 0x001DA114 File Offset: 0x001D9514
		protected DeletedRowInaccessibleException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x001DA12C File Offset: 0x001D952C
		public DeletedRowInaccessibleException()
			: base(Res.GetString("DataSet_DefaultDeletedRowInaccessibleException"))
		{
			base.HResult = -2146232031;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x001DA154 File Offset: 0x001D9554
		public DeletedRowInaccessibleException(string s)
			: base(s)
		{
			base.HResult = -2146232031;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x001DA174 File Offset: 0x001D9574
		public DeletedRowInaccessibleException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232031;
		}
	}
}
