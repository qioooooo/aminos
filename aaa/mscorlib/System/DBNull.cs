using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000A0 RID: 160
	[ComVisible(true)]
	[Serializable]
	public sealed class DBNull : ISerializable, IConvertible
	{
		// Token: 0x06000966 RID: 2406 RVA: 0x0001CBF4 File Offset: 0x0001BBF4
		private DBNull()
		{
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0001CBFC File Offset: 0x0001BBFC
		private DBNull(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DBNullSerial"));
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0001CC13 File Offset: 0x0001BC13
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			UnitySerializationHolder.GetUnitySerializationInfo(info, 2, null, null);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0001CC1E File Offset: 0x0001BC1E
		public override string ToString()
		{
			return string.Empty;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001CC25 File Offset: 0x0001BC25
		public string ToString(IFormatProvider provider)
		{
			return string.Empty;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001CC2C File Offset: 0x0001BC2C
		public TypeCode GetTypeCode()
		{
			return TypeCode.DBNull;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0001CC2F File Offset: 0x0001BC2F
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0001CC40 File Offset: 0x0001BC40
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0001CC51 File Offset: 0x0001BC51
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0001CC62 File Offset: 0x0001BC62
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0001CC73 File Offset: 0x0001BC73
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0001CC84 File Offset: 0x0001BC84
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0001CC95 File Offset: 0x0001BC95
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0001CCA6 File Offset: 0x0001BCA6
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0001CCB7 File Offset: 0x0001BCB7
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0001CCC8 File Offset: 0x0001BCC8
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0001CCD9 File Offset: 0x0001BCD9
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0001CCEA File Offset: 0x0001BCEA
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0001CCFB File Offset: 0x0001BCFB
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0001CD0C File Offset: 0x0001BD0C
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromDBNull"));
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0001CD1D File Offset: 0x0001BD1D
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0400037D RID: 893
		public static readonly DBNull Value = new DBNull();
	}
}
