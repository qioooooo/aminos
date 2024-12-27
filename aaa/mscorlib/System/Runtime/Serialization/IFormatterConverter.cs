using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000347 RID: 839
	[ComVisible(true)]
	[CLSCompliant(false)]
	public interface IFormatterConverter
	{
		// Token: 0x0600217B RID: 8571
		object Convert(object value, Type type);

		// Token: 0x0600217C RID: 8572
		object Convert(object value, TypeCode typeCode);

		// Token: 0x0600217D RID: 8573
		bool ToBoolean(object value);

		// Token: 0x0600217E RID: 8574
		char ToChar(object value);

		// Token: 0x0600217F RID: 8575
		sbyte ToSByte(object value);

		// Token: 0x06002180 RID: 8576
		byte ToByte(object value);

		// Token: 0x06002181 RID: 8577
		short ToInt16(object value);

		// Token: 0x06002182 RID: 8578
		ushort ToUInt16(object value);

		// Token: 0x06002183 RID: 8579
		int ToInt32(object value);

		// Token: 0x06002184 RID: 8580
		uint ToUInt32(object value);

		// Token: 0x06002185 RID: 8581
		long ToInt64(object value);

		// Token: 0x06002186 RID: 8582
		ulong ToUInt64(object value);

		// Token: 0x06002187 RID: 8583
		float ToSingle(object value);

		// Token: 0x06002188 RID: 8584
		double ToDouble(object value);

		// Token: 0x06002189 RID: 8585
		decimal ToDecimal(object value);

		// Token: 0x0600218A RID: 8586
		DateTime ToDateTime(object value);

		// Token: 0x0600218B RID: 8587
		string ToString(object value);
	}
}
