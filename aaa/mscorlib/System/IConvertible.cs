using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200001E RID: 30
	[ComVisible(true)]
	[CLSCompliant(false)]
	public interface IConvertible
	{
		// Token: 0x060000EC RID: 236
		TypeCode GetTypeCode();

		// Token: 0x060000ED RID: 237
		bool ToBoolean(IFormatProvider provider);

		// Token: 0x060000EE RID: 238
		char ToChar(IFormatProvider provider);

		// Token: 0x060000EF RID: 239
		sbyte ToSByte(IFormatProvider provider);

		// Token: 0x060000F0 RID: 240
		byte ToByte(IFormatProvider provider);

		// Token: 0x060000F1 RID: 241
		short ToInt16(IFormatProvider provider);

		// Token: 0x060000F2 RID: 242
		ushort ToUInt16(IFormatProvider provider);

		// Token: 0x060000F3 RID: 243
		int ToInt32(IFormatProvider provider);

		// Token: 0x060000F4 RID: 244
		uint ToUInt32(IFormatProvider provider);

		// Token: 0x060000F5 RID: 245
		long ToInt64(IFormatProvider provider);

		// Token: 0x060000F6 RID: 246
		ulong ToUInt64(IFormatProvider provider);

		// Token: 0x060000F7 RID: 247
		float ToSingle(IFormatProvider provider);

		// Token: 0x060000F8 RID: 248
		double ToDouble(IFormatProvider provider);

		// Token: 0x060000F9 RID: 249
		decimal ToDecimal(IFormatProvider provider);

		// Token: 0x060000FA RID: 250
		DateTime ToDateTime(IFormatProvider provider);

		// Token: 0x060000FB RID: 251
		string ToString(IFormatProvider provider);

		// Token: 0x060000FC RID: 252
		object ToType(Type conversionType, IFormatProvider provider);
	}
}
