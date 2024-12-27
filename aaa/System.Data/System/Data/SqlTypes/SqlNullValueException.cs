using System;
using System.Runtime.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000373 RID: 883
	[Serializable]
	public sealed class SqlNullValueException : SqlTypeException
	{
		// Token: 0x06002F3E RID: 12094 RVA: 0x002AFE3C File Offset: 0x002AF23C
		public SqlNullValueException()
			: this(SQLResource.NullValueMessage, null)
		{
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x002AFE58 File Offset: 0x002AF258
		public SqlNullValueException(string message)
			: this(message, null)
		{
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x002AFE70 File Offset: 0x002AF270
		public SqlNullValueException(string message, Exception e)
			: base(message, e)
		{
			base.HResult = -2146232015;
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x002AFE90 File Offset: 0x002AF290
		private SqlNullValueException(SerializationInfo si, StreamingContext sc)
			: base(SqlNullValueException.SqlNullValueExceptionSerialization(si, sc), sc)
		{
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x002AFEAC File Offset: 0x002AF2AC
		private static SerializationInfo SqlNullValueExceptionSerialization(SerializationInfo si, StreamingContext sc)
		{
			if (si != null && 1 == si.MemberCount)
			{
				string @string = si.GetString("SqlNullValueExceptionMessage");
				SqlNullValueException ex = new SqlNullValueException(@string);
				ex.GetObjectData(si, sc);
			}
			return si;
		}
	}
}
