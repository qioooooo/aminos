using System;

namespace System.Data.Odbc
{
	// Token: 0x02000205 RID: 517
	internal struct SQLLEN
	{
		// Token: 0x06001C8E RID: 7310 RVA: 0x0024D634 File Offset: 0x0024CA34
		internal SQLLEN(int value)
		{
			this._value = value;
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x0024D648 File Offset: 0x0024CA48
		internal SQLLEN(long value)
		{
			this._value = checked((int)value);
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x0024D660 File Offset: 0x0024CA60
		internal SQLLEN(IntPtr value)
		{
			this._value = value.ToInt32();
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x0024D67C File Offset: 0x0024CA7C
		public static implicit operator SQLLEN(int value)
		{
			return new SQLLEN(value);
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x0024D690 File Offset: 0x0024CA90
		public static explicit operator SQLLEN(long value)
		{
			return new SQLLEN(value);
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x0024D6A4 File Offset: 0x0024CAA4
		public static implicit operator int(SQLLEN value)
		{
			return value._value;
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x0024D6B8 File Offset: 0x0024CAB8
		public static explicit operator long(SQLLEN value)
		{
			return (long)value._value;
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x0024D6D0 File Offset: 0x0024CAD0
		public long ToInt64()
		{
			return (long)this._value;
		}

		// Token: 0x04001068 RID: 4200
		internal int _value;
	}
}
