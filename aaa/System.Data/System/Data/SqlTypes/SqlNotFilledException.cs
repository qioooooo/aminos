using System;
using System.Runtime.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000375 RID: 885
	[Serializable]
	public sealed class SqlNotFilledException : SqlTypeException
	{
		// Token: 0x06002F48 RID: 12104 RVA: 0x002AFF8C File Offset: 0x002AF38C
		public SqlNotFilledException()
			: this(SQLResource.NotFilledMessage, null)
		{
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x002AFFA8 File Offset: 0x002AF3A8
		public SqlNotFilledException(string message)
			: this(message, null)
		{
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x002AFFC0 File Offset: 0x002AF3C0
		public SqlNotFilledException(string message, Exception e)
			: base(message, e)
		{
			base.HResult = -2146232015;
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x002AFFE0 File Offset: 0x002AF3E0
		private SqlNotFilledException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}
	}
}
