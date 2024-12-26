using System;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x02000072 RID: 114
	internal abstract class EnumWrapper : IConvertible
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600055B RID: 1371
		internal abstract object value { get; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600055C RID: 1372
		internal abstract Type type { get; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600055D RID: 1373
		internal abstract string name { get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x00025E61 File Offset: 0x00024E61
		internal virtual IReflect classScopeOrType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00025E69 File Offset: 0x00024E69
		TypeCode IConvertible.GetTypeCode()
		{
			return Convert.GetTypeCode(this.value);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00025E76 File Offset: 0x00024E76
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToBoolean(provider);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00025E89 File Offset: 0x00024E89
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToChar(provider);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00025E9C File Offset: 0x00024E9C
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToSByte(provider);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00025EAF File Offset: 0x00024EAF
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToByte(provider);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00025EC2 File Offset: 0x00024EC2
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToInt16(provider);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00025ED5 File Offset: 0x00024ED5
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToUInt16(provider);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00025EE8 File Offset: 0x00024EE8
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToInt32(provider);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00025EFB File Offset: 0x00024EFB
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToUInt32(provider);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00025F0E File Offset: 0x00024F0E
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToInt64(provider);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00025F21 File Offset: 0x00024F21
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToUInt64(provider);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00025F34 File Offset: 0x00024F34
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToSingle(provider);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00025F47 File Offset: 0x00024F47
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToDouble(provider);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00025F5A File Offset: 0x00024F5A
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToDecimal(provider);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00025F6D File Offset: 0x00024F6D
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToDateTime(provider);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00025F80 File Offset: 0x00024F80
		string IConvertible.ToString(IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToString(provider);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00025F93 File Offset: 0x00024F93
		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return ((IConvertible)this.value).ToType(conversionType, provider);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00025FA7 File Offset: 0x00024FA7
		internal object ToNumericValue()
		{
			return this.value;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00025FB0 File Offset: 0x00024FB0
		public override string ToString()
		{
			if (this.name != null)
			{
				return this.name;
			}
			FieldInfo[] fields = this.type.GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (StrictEquality.JScriptStrictEquals(this.value, fieldInfo.GetValue(null)))
				{
					return fieldInfo.Name;
				}
			}
			return Convert.ToString(this.value);
		}
	}
}
