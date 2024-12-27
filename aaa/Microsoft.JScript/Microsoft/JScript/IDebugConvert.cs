using System;
using System.Runtime.InteropServices;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000062 RID: 98
	[Guid("AA51516D-C0F2-49fe-9D38-61D20456904C")]
	[ComVisible(true)]
	public interface IDebugConvert
	{
		// Token: 0x060004D9 RID: 1241
		object ToPrimitive(object value, TypeCode typeCode, bool truncationPermitted);

		// Token: 0x060004DA RID: 1242
		string ByteToString(byte value, int radix);

		// Token: 0x060004DB RID: 1243
		string SByteToString(sbyte value, int radix);

		// Token: 0x060004DC RID: 1244
		string Int16ToString(short value, int radix);

		// Token: 0x060004DD RID: 1245
		string UInt16ToString(ushort value, int radix);

		// Token: 0x060004DE RID: 1246
		string Int32ToString(int value, int radix);

		// Token: 0x060004DF RID: 1247
		string UInt32ToString(uint value, int radix);

		// Token: 0x060004E0 RID: 1248
		string Int64ToString(long value, int radix);

		// Token: 0x060004E1 RID: 1249
		string UInt64ToString(ulong value, int radix);

		// Token: 0x060004E2 RID: 1250
		string SingleToString(float value);

		// Token: 0x060004E3 RID: 1251
		string DoubleToString(double value);

		// Token: 0x060004E4 RID: 1252
		string BooleanToString(bool value);

		// Token: 0x060004E5 RID: 1253
		string DoubleToDateString(double value);

		// Token: 0x060004E6 RID: 1254
		string RegexpToString(string source, bool ignoreCase, bool global, bool multiline);

		// Token: 0x060004E7 RID: 1255
		string StringToPrintable(string source);

		// Token: 0x060004E8 RID: 1256
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetManagedObject(object value);

		// Token: 0x060004E9 RID: 1257
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetManagedInt64Object(long i);

		// Token: 0x060004EA RID: 1258
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetManagedUInt64Object(ulong i);

		// Token: 0x060004EB RID: 1259
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetManagedCharObject(ushort i);

		// Token: 0x060004EC RID: 1260
		string GetErrorMessageForHR(int hr, IVsaEngine engine);
	}
}
