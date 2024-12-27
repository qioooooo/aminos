using System;
using System.Runtime.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000374 RID: 884
	[Serializable]
	public sealed class SqlTruncateException : SqlTypeException
	{
		// Token: 0x06002F43 RID: 12099 RVA: 0x002AFEE4 File Offset: 0x002AF2E4
		public SqlTruncateException()
			: this(SQLResource.TruncationMessage, null)
		{
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x002AFF00 File Offset: 0x002AF300
		public SqlTruncateException(string message)
			: this(message, null)
		{
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x002AFF18 File Offset: 0x002AF318
		public SqlTruncateException(string message, Exception e)
			: base(message, e)
		{
			base.HResult = -2146232014;
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x002AFF38 File Offset: 0x002AF338
		private SqlTruncateException(SerializationInfo si, StreamingContext sc)
			: base(SqlTruncateException.SqlTruncateExceptionSerialization(si, sc), sc)
		{
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x002AFF54 File Offset: 0x002AF354
		private static SerializationInfo SqlTruncateExceptionSerialization(SerializationInfo si, StreamingContext sc)
		{
			if (si != null && 1 == si.MemberCount)
			{
				string @string = si.GetString("SqlTruncateExceptionMessage");
				SqlTruncateException ex = new SqlTruncateException(@string);
				ex.GetObjectData(si, sc);
			}
			return si;
		}
	}
}
