using System;
using System.Collections.Generic;
using System.Reflection;

namespace System
{
	// Token: 0x020000C7 RID: 199
	internal static class Internal
	{
		// Token: 0x06000B7D RID: 2941 RVA: 0x00022E4C File Offset: 0x00021E4C
		private static void CommonlyUsedGenericInstantiations_HACK()
		{
			Array.Sort<double>(null);
			Array.Sort<int>(null);
			Array.Sort<IntPtr>(null);
			new ArraySegment<byte>(new byte[1], 0, 0);
			new Dictionary<char, object>();
			new Dictionary<Guid, byte>();
			new Dictionary<Guid, object>();
			new Dictionary<short, IntPtr>();
			new Dictionary<int, byte>();
			new Dictionary<int, int>();
			new Dictionary<int, object>();
			new Dictionary<IntPtr, bool>();
			new Dictionary<IntPtr, short>();
			new Dictionary<object, bool>();
			new Dictionary<object, char>();
			new Dictionary<object, Guid>();
			new Dictionary<object, int>();
			new Dictionary<object, uint>();
			new Dictionary<uint, object>();
			Internal.NullableHelper_HACK<bool>();
			Internal.NullableHelper_HACK<byte>();
			Internal.NullableHelper_HACK<char>();
			Internal.NullableHelper_HACK<DateTime>();
			Internal.NullableHelper_HACK<decimal>();
			Internal.NullableHelper_HACK<double>();
			Internal.NullableHelper_HACK<Guid>();
			Internal.NullableHelper_HACK<short>();
			Internal.NullableHelper_HACK<int>();
			Internal.NullableHelper_HACK<long>();
			Internal.NullableHelper_HACK<float>();
			Internal.NullableHelper_HACK<TimeSpan>();
			new List<bool>();
			new List<byte>();
			new List<DateTime>();
			new List<decimal>();
			new List<double>();
			new List<Guid>();
			new List<short>();
			new List<int>();
			new List<long>();
			new List<TimeSpan>();
			new List<sbyte>();
			new List<float>();
			new List<ushort>();
			new List<uint>();
			new List<ulong>();
			new List<KeyValuePair<object, object>>();
			RuntimeType.RuntimeTypeCache.Prejitinit_HACK();
			new CerArrayList<RuntimeMethodInfo>(0);
			new CerArrayList<RuntimeConstructorInfo>(0);
			new CerArrayList<RuntimePropertyInfo>(0);
			new CerArrayList<RuntimeEventInfo>(0);
			new CerArrayList<RuntimeFieldInfo>(0);
			new CerArrayList<RuntimeType>(0);
			new KeyValuePair<char, ushort>('\0', 0);
			new KeyValuePair<ushort, double>(0, double.MinValue);
			Internal.SZArrayHelper_HACK<bool>(null);
			Internal.SZArrayHelper_HACK<byte>(null);
			Internal.SZArrayHelper_HACK<DateTime>(null);
			Internal.SZArrayHelper_HACK<decimal>(null);
			Internal.SZArrayHelper_HACK<double>(null);
			Internal.SZArrayHelper_HACK<Guid>(null);
			Internal.SZArrayHelper_HACK<short>(null);
			Internal.SZArrayHelper_HACK<int>(null);
			Internal.SZArrayHelper_HACK<long>(null);
			Internal.SZArrayHelper_HACK<TimeSpan>(null);
			Internal.SZArrayHelper_HACK<sbyte>(null);
			Internal.SZArrayHelper_HACK<float>(null);
			Internal.SZArrayHelper_HACK<ushort>(null);
			Internal.SZArrayHelper_HACK<uint>(null);
			Internal.SZArrayHelper_HACK<ulong>(null);
			Internal.SZArrayHelper_HACK<CustomAttributeTypedArgument>(null);
			Internal.SZArrayHelper_HACK<CustomAttributeNamedArgument>(null);
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x00023028 File Offset: 0x00022028
		private static T NullableHelper_HACK<T>() where T : struct
		{
			Nullable.Compare<T>(null, null);
			Nullable.Equals<T>(null, null);
			return ((T?)null).GetValueOrDefault();
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00023075 File Offset: 0x00022075
		private static void SZArrayHelper_HACK<T>(SZArrayHelper oSZArrayHelper)
		{
			int count = oSZArrayHelper.get_Count<T>();
			T t = oSZArrayHelper.get_Item<T>(0);
			oSZArrayHelper.GetEnumerator<T>();
		}
	}
}
