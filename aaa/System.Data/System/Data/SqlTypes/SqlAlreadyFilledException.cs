using System;
using System.Runtime.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000376 RID: 886
	[Serializable]
	public sealed class SqlAlreadyFilledException : SqlTypeException
	{
		// Token: 0x06002F4C RID: 12108 RVA: 0x002AFFF8 File Offset: 0x002AF3F8
		public SqlAlreadyFilledException()
			: this(SQLResource.AlreadyFilledMessage, null)
		{
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x002B0014 File Offset: 0x002AF414
		public SqlAlreadyFilledException(string message)
			: this(message, null)
		{
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x002B002C File Offset: 0x002AF42C
		public SqlAlreadyFilledException(string message, Exception e)
			: base(message, e)
		{
			base.HResult = -2146232015;
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x002B004C File Offset: 0x002AF44C
		private SqlAlreadyFilledException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}
	}
}
