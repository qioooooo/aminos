using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000064 RID: 100
	[ComVisible(true)]
	[Guid("432D76CE-8C9E-4eed-ADDD-91737F27A8CB")]
	public class DebugConvert : IDebugConvert, IDebugConvert2
	{
		// Token: 0x060004EE RID: 1262 RVA: 0x0002477F File Offset: 0x0002377F
		public object ToPrimitive(object value, TypeCode typeCode, bool truncationPermitted)
		{
			return Convert.Coerce2(value, typeCode, truncationPermitted);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00024789 File Offset: 0x00023789
		public string ByteToString(byte value, int radix)
		{
			return Convert.ToString(value, radix);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00024792 File Offset: 0x00023792
		public string SByteToString(sbyte value, int radix)
		{
			if (radix == 10)
			{
				return value.ToString(CultureInfo.InvariantCulture);
			}
			return Convert.ToString((byte)value, radix);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x000247AE File Offset: 0x000237AE
		public string Int16ToString(short value, int radix)
		{
			return Convert.ToString((short)Convert.ToInteger((double)value), radix);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000247BE File Offset: 0x000237BE
		public string UInt16ToString(ushort value, int radix)
		{
			if (radix == 10)
			{
				return value.ToString(CultureInfo.InvariantCulture);
			}
			return Convert.ToString((short)Convert.ToInteger((double)value), radix);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x000247E0 File Offset: 0x000237E0
		public string Int32ToString(int value, int radix)
		{
			return Convert.ToString((int)Convert.ToInteger((double)value), radix);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x000247F0 File Offset: 0x000237F0
		public string UInt32ToString(uint value, int radix)
		{
			if (radix == 10)
			{
				return value.ToString(CultureInfo.InvariantCulture);
			}
			return Convert.ToString((int)Convert.ToInteger(value), radix);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00024813 File Offset: 0x00023813
		public string Int64ToString(long value, int radix)
		{
			return Convert.ToString((long)Convert.ToInteger((double)value), radix);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00024823 File Offset: 0x00023823
		public string UInt64ToString(ulong value, int radix)
		{
			if (radix == 10)
			{
				return value.ToString(CultureInfo.InvariantCulture);
			}
			return Convert.ToString((long)Convert.ToInteger(value), radix);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00024846 File Offset: 0x00023846
		public string SingleToString(float value)
		{
			return Convert.ToString((double)value);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0002484F File Offset: 0x0002384F
		public string DoubleToString(double value)
		{
			return Convert.ToString(value);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00024857 File Offset: 0x00023857
		public string BooleanToString(bool value)
		{
			return Convert.ToString(value);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0002485F File Offset: 0x0002385F
		public string DoubleToDateString(double value)
		{
			return DatePrototype.DateToString(value);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00024868 File Offset: 0x00023868
		public string RegexpToString(string source, bool ignoreCase, bool global, bool multiline)
		{
			object obj = RegExpConstructor.ob.Construct(source, ignoreCase, global, multiline);
			return obj.ToString();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0002488B File Offset: 0x0002388B
		public string DecimalToString(decimal value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0002489C File Offset: 0x0002389C
		public string StringToPrintable(string source)
		{
			int length = source.Length;
			StringBuilder stringBuilder = new StringBuilder(length);
			int i = 0;
			while (i < length)
			{
				int num = (int)source[i];
				if (num <= 13)
				{
					if (num != 0)
					{
						switch (num)
						{
						case 8:
							stringBuilder.Append("\\b");
							break;
						case 9:
							stringBuilder.Append("\\t");
							break;
						case 10:
							stringBuilder.Append("\\n");
							break;
						case 11:
							stringBuilder.Append("\\v");
							break;
						case 12:
							stringBuilder.Append("\\f");
							break;
						case 13:
							stringBuilder.Append("\\r");
							break;
						default:
							goto IL_00FF;
						}
					}
					else
					{
						stringBuilder.Append("\\0");
					}
				}
				else if (num != 34)
				{
					if (num != 92)
					{
						goto IL_00FF;
					}
					stringBuilder.Append("\\\\");
				}
				else
				{
					stringBuilder.Append("\"");
				}
				IL_0184:
				i++;
				continue;
				IL_00FF:
				if (char.GetUnicodeCategory(source[i]) != UnicodeCategory.Control)
				{
					stringBuilder.Append(source[i]);
					goto IL_0184;
				}
				stringBuilder.Append("\\u");
				int num2 = (int)source[i];
				char[] array = new char[4];
				for (int j = 0; j < 4; j++)
				{
					int num3 = num2 % 16;
					if (num3 <= 9)
					{
						array[3 - j] = (char)(48 + num3);
					}
					else
					{
						array[3 - j] = (char)(65 + num3 - 10);
					}
					num2 /= 16;
				}
				stringBuilder.Append(array);
				goto IL_0184;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00024A3E File Offset: 0x00023A3E
		[return: MarshalAs(UnmanagedType.Interface)]
		public object GetManagedObject(object value)
		{
			return value;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00024A41 File Offset: 0x00023A41
		[return: MarshalAs(UnmanagedType.Interface)]
		public object GetManagedInt64Object(long i)
		{
			return i;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00024A49 File Offset: 0x00023A49
		[return: MarshalAs(UnmanagedType.Interface)]
		public object GetManagedUInt64Object(ulong i)
		{
			return i;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00024A51 File Offset: 0x00023A51
		[return: MarshalAs(UnmanagedType.Interface)]
		public object GetManagedCharObject(ushort i)
		{
			return (char)i;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00024A5C File Offset: 0x00023A5C
		public string GetErrorMessageForHR(int hr, IVsaEngine engine)
		{
			CultureInfo cultureInfo = null;
			VsaEngine vsaEngine = engine as VsaEngine;
			if (vsaEngine != null)
			{
				cultureInfo = vsaEngine.ErrorCultureInfo;
			}
			if (((long)hr & (long)((ulong)(-65536))) == (long)((ulong)(-2146828288)) && Enum.IsDefined(typeof(JSError), hr & 65535))
			{
				return JScriptException.Localize((hr & 65535).ToString(CultureInfo.InvariantCulture), cultureInfo);
			}
			return JScriptException.Localize(6011.ToString(CultureInfo.InvariantCulture), "0x" + hr.ToString("X", CultureInfo.InvariantCulture), cultureInfo);
		}
	}
}
