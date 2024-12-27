using System;
using System.Runtime.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000372 RID: 882
	[Serializable]
	public class SqlTypeException : SystemException
	{
		// Token: 0x06002F39 RID: 12089 RVA: 0x002AFD90 File Offset: 0x002AF190
		public SqlTypeException()
			: this(Res.GetString("SqlMisc_SqlTypeMessage"), null)
		{
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x002AFDB0 File Offset: 0x002AF1B0
		public SqlTypeException(string message)
			: this(message, null)
		{
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x002AFDC8 File Offset: 0x002AF1C8
		public SqlTypeException(string message, Exception e)
			: base(message, e)
		{
			base.HResult = -2146232016;
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x002AFDE8 File Offset: 0x002AF1E8
		protected SqlTypeException(SerializationInfo si, StreamingContext sc)
			: base(SqlTypeException.SqlTypeExceptionSerialization(si, sc), sc)
		{
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x002AFE04 File Offset: 0x002AF204
		private static SerializationInfo SqlTypeExceptionSerialization(SerializationInfo si, StreamingContext sc)
		{
			if (si != null && 1 == si.MemberCount)
			{
				string @string = si.GetString("SqlTypeExceptionMessage");
				SqlTypeException ex = new SqlTypeException(@string);
				ex.GetObjectData(si, sc);
			}
			return si;
		}
	}
}
