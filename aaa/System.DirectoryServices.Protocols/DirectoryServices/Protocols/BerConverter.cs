using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000008 RID: 8
	public sealed class BerConverter
	{
		// Token: 0x0600000D RID: 13 RVA: 0x0000229D File Offset: 0x0000129D
		private BerConverter()
		{
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022A8 File Offset: 0x000012A8
		public static byte[] Encode(string format, params object[] value)
		{
			Utility.CheckOSVersion();
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			byte[] array = null;
			if (value == null)
			{
				value = new object[0];
			}
			BerSafeHandle berSafeHandle = new BerSafeHandle();
			int num = 0;
			foreach (char c in format)
			{
				int num2;
				if (c == '{' || c == '}' || c == '[' || c == ']' || c == 'n')
				{
					num2 = Wldap32.ber_printf_emptyarg(berSafeHandle, new string(c, 1));
				}
				else if (c == 't' || c == 'i' || c == 'e')
				{
					if (num >= value.Length)
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					if (!(value[num] is int))
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					num2 = Wldap32.ber_printf_int(berSafeHandle, new string(c, 1), (int)value[num]);
					num++;
				}
				else if (c == 'b')
				{
					if (num >= value.Length)
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					if (!(value[num] is bool))
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					num2 = Wldap32.ber_printf_int(berSafeHandle, new string(c, 1), ((bool)value[num]) ? 1 : 0);
					num++;
				}
				else if (c == 's')
				{
					if (num >= value.Length)
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					if (value[num] != null && !(value[num] is string))
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					byte[] array2 = null;
					if (value[num] != null)
					{
						array2 = utf8Encoding.GetBytes((string)value[num]);
					}
					num2 = BerConverter.EncodingByteArrayHelper(berSafeHandle, array2, 'o');
					num++;
				}
				else if (c == 'o' || c == 'X')
				{
					if (num >= value.Length)
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					if (value[num] != null && !(value[num] is byte[]))
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					byte[] array3 = (byte[])value[num];
					num2 = BerConverter.EncodingByteArrayHelper(berSafeHandle, array3, c);
					num++;
				}
				else if (c == 'v')
				{
					if (num >= value.Length)
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					if (value[num] != null && !(value[num] is string[]))
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					string[] array4 = (string[])value[num];
					byte[][] array5 = null;
					if (array4 != null)
					{
						array5 = new byte[array4.Length][];
						for (int j = 0; j < array4.Length; j++)
						{
							string text = array4[j];
							if (text == null)
							{
								array5[j] = null;
							}
							else
							{
								array5[j] = utf8Encoding.GetBytes(text);
							}
						}
					}
					num2 = BerConverter.EncodingMultiByteArrayHelper(berSafeHandle, array5, 'V');
					num++;
				}
				else
				{
					if (c != 'V')
					{
						throw new ArgumentException(Res.GetString("BerConverterUndefineChar"));
					}
					if (num >= value.Length)
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					if (value[num] != null && !(value[num] is byte[][]))
					{
						throw new ArgumentException(Res.GetString("BerConverterNotMatch"));
					}
					byte[][] array6 = (byte[][])value[num];
					num2 = BerConverter.EncodingMultiByteArrayHelper(berSafeHandle, array6, c);
					num++;
				}
				if (num2 == -1)
				{
					throw new BerConversionException();
				}
			}
			berval berval = new berval();
			IntPtr intPtr = (IntPtr)0;
			try
			{
				int num2 = Wldap32.ber_flatten(berSafeHandle, ref intPtr);
				if (num2 == -1)
				{
					throw new BerConversionException();
				}
				if (intPtr != (IntPtr)0)
				{
					Marshal.PtrToStructure(intPtr, berval);
				}
				if (berval == null || berval.bv_len == 0)
				{
					array = new byte[0];
				}
				else
				{
					array = new byte[berval.bv_len];
					Marshal.Copy(berval.bv_val, array, 0, berval.bv_len);
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					Wldap32.ber_bvfree(intPtr);
				}
			}
			return array;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002678 File Offset: 0x00001678
		public static object[] Decode(string format, byte[] value)
		{
			bool flag;
			object[] array = BerConverter.TryDecode(format, value, out flag);
			if (flag)
			{
				return array;
			}
			throw new BerConversionException();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000269C File Offset: 0x0000169C
		internal static object[] TryDecode(string format, byte[] value, out bool decodeSucceeded)
		{
			Utility.CheckOSVersion();
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			UTF8Encoding utf8Encoding = new UTF8Encoding(false, true);
			berval berval = new berval();
			ArrayList arrayList = new ArrayList();
			BerSafeHandle berSafeHandle = null;
			object[] array = null;
			decodeSucceeded = false;
			if (value == null)
			{
				berval.bv_len = 0;
				berval.bv_val = (IntPtr)0;
			}
			else
			{
				berval.bv_len = value.Length;
				berval.bv_val = Marshal.AllocHGlobal(value.Length);
				Marshal.Copy(value, 0, berval.bv_val, value.Length);
			}
			try
			{
				berSafeHandle = new BerSafeHandle(berval);
			}
			finally
			{
				if (berval.bv_val != (IntPtr)0)
				{
					Marshal.FreeHGlobal(berval.bv_val);
				}
			}
			int num = 0;
			foreach (char c in format)
			{
				if (c == '{' || c == '}' || c == '[' || c == ']' || c == 'n' || c == 'x')
				{
					num = Wldap32.ber_scanf(berSafeHandle, new string(c, 1));
					if (num != 0)
					{
					}
				}
				else if (c == 'i' || c == 'e' || c == 'b')
				{
					int num2 = 0;
					num = Wldap32.ber_scanf_int(berSafeHandle, new string(c, 1), ref num2);
					if (num == 0)
					{
						if (c == 'b')
						{
							bool flag = num2 != 0;
							arrayList.Add(flag);
						}
						else
						{
							arrayList.Add(num2);
						}
					}
				}
				else if (c == 'a')
				{
					byte[] array2 = BerConverter.DecodingByteArrayHelper(berSafeHandle, 'O', ref num);
					if (num == 0)
					{
						string text = null;
						if (array2 != null)
						{
							text = utf8Encoding.GetString(array2);
						}
						arrayList.Add(text);
					}
				}
				else if (c == 'O')
				{
					byte[] array3 = BerConverter.DecodingByteArrayHelper(berSafeHandle, c, ref num);
					if (num == 0)
					{
						arrayList.Add(array3);
					}
				}
				else if (c == 'B')
				{
					IntPtr intPtr = (IntPtr)0;
					int num3 = 0;
					num = Wldap32.ber_scanf_bitstring(berSafeHandle, "B", ref intPtr, ref num3);
					if (num == 0)
					{
						byte[] array4 = null;
						if (intPtr != (IntPtr)0)
						{
							array4 = new byte[num3];
							Marshal.Copy(intPtr, array4, 0, num3);
						}
						arrayList.Add(array4);
					}
				}
				else if (c == 'v')
				{
					string[] array5 = null;
					byte[][] array6 = BerConverter.DecodingMultiByteArrayHelper(berSafeHandle, 'V', ref num);
					if (num == 0)
					{
						if (array6 != null)
						{
							array5 = new string[array6.Length];
							for (int j = 0; j < array6.Length; j++)
							{
								if (array6[j] == null)
								{
									array5[j] = null;
								}
								else
								{
									array5[j] = utf8Encoding.GetString(array6[j]);
								}
							}
						}
						arrayList.Add(array5);
					}
				}
				else
				{
					if (c != 'V')
					{
						throw new ArgumentException(Res.GetString("BerConverterUndefineChar"));
					}
					byte[][] array7 = BerConverter.DecodingMultiByteArrayHelper(berSafeHandle, c, ref num);
					if (num == 0)
					{
						arrayList.Add(array7);
					}
				}
				if (num != 0)
				{
					return array;
				}
			}
			array = new object[arrayList.Count];
			for (int k = 0; k < arrayList.Count; k++)
			{
				array[k] = arrayList[k];
			}
			decodeSucceeded = true;
			return array;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000029B8 File Offset: 0x000019B8
		private static int EncodingByteArrayHelper(BerSafeHandle berElement, byte[] tempValue, char fmt)
		{
			int num;
			if (tempValue != null)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(tempValue.Length);
				Marshal.Copy(tempValue, 0, intPtr, tempValue.Length);
				HGlobalMemHandle hglobalMemHandle = new HGlobalMemHandle(intPtr);
				num = Wldap32.ber_printf_bytearray(berElement, new string(fmt, 1), hglobalMemHandle, tempValue.Length);
			}
			else
			{
				num = Wldap32.ber_printf_bytearray(berElement, new string(fmt, 1), new HGlobalMemHandle((IntPtr)0), 0);
			}
			return num;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002A14 File Offset: 0x00001A14
		private static byte[] DecodingByteArrayHelper(BerSafeHandle berElement, char fmt, ref int error)
		{
			error = 0;
			IntPtr intPtr = (IntPtr)0;
			berval berval = new berval();
			byte[] array = null;
			error = Wldap32.ber_scanf_ptr(berElement, new string(fmt, 1), ref intPtr);
			try
			{
				if (error == 0 && intPtr != (IntPtr)0)
				{
					Marshal.PtrToStructure(intPtr, berval);
					array = new byte[berval.bv_len];
					Marshal.Copy(berval.bv_val, array, 0, berval.bv_len);
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					Wldap32.ber_bvfree(intPtr);
				}
			}
			return array;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002AA8 File Offset: 0x00001AA8
		private static int EncodingMultiByteArrayHelper(BerSafeHandle berElement, byte[][] tempValue, char fmt)
		{
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)0;
			SafeBerval[] array = null;
			int num = 0;
			try
			{
				if (tempValue != null)
				{
					intPtr = Utility.AllocHGlobalIntPtrArray(tempValue.Length + 1);
					int num2 = Marshal.SizeOf(typeof(SafeBerval));
					array = new SafeBerval[tempValue.Length];
					int i;
					for (i = 0; i < tempValue.Length; i++)
					{
						byte[] array2 = tempValue[i];
						array[i] = new SafeBerval();
						if (array2 == null)
						{
							array[i].bv_len = 0;
							array[i].bv_val = (IntPtr)0;
						}
						else
						{
							array[i].bv_len = array2.Length;
							array[i].bv_val = Marshal.AllocHGlobal(array2.Length);
							Marshal.Copy(array2, 0, array[i].bv_val, array2.Length);
						}
						IntPtr intPtr3 = Marshal.AllocHGlobal(num2);
						Marshal.StructureToPtr(array[i], intPtr3, false);
						intPtr2 = (IntPtr)((long)intPtr + (long)(Marshal.SizeOf(typeof(IntPtr)) * i));
						Marshal.WriteIntPtr(intPtr2, intPtr3);
					}
					intPtr2 = (IntPtr)((long)intPtr + (long)(Marshal.SizeOf(typeof(IntPtr)) * i));
					Marshal.WriteIntPtr(intPtr2, (IntPtr)0);
				}
				num = Wldap32.ber_printf_berarray(berElement, new string(fmt, 1), intPtr);
				GC.KeepAlive(array);
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					for (int j = 0; j < tempValue.Length; j++)
					{
						IntPtr intPtr4 = Marshal.ReadIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * j);
						if (intPtr4 != (IntPtr)0)
						{
							Marshal.FreeHGlobal(intPtr4);
						}
					}
					Marshal.FreeHGlobal(intPtr);
				}
			}
			return num;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002C64 File Offset: 0x00001C64
		private static byte[][] DecodingMultiByteArrayHelper(BerSafeHandle berElement, char fmt, ref int error)
		{
			error = 0;
			IntPtr intPtr = (IntPtr)0;
			int num = 0;
			ArrayList arrayList = new ArrayList();
			IntPtr intPtr2 = (IntPtr)0;
			byte[][] array = null;
			try
			{
				error = Wldap32.ber_scanf_ptr(berElement, new string(fmt, 1), ref intPtr);
				if (error == 0 && intPtr != (IntPtr)0)
				{
					intPtr2 = Marshal.ReadIntPtr(intPtr);
					while (intPtr2 != (IntPtr)0)
					{
						berval berval = new berval();
						Marshal.PtrToStructure(intPtr2, berval);
						byte[] array2 = new byte[berval.bv_len];
						Marshal.Copy(berval.bv_val, array2, 0, berval.bv_len);
						arrayList.Add(array2);
						num++;
						intPtr2 = Marshal.ReadIntPtr(intPtr, num * Marshal.SizeOf(typeof(IntPtr)));
					}
					array = new byte[arrayList.Count][];
					for (int i = 0; i < arrayList.Count; i++)
					{
						array[i] = (byte[])arrayList[i];
					}
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					Wldap32.ber_bvecfree(intPtr);
				}
			}
			return array;
		}
	}
}
