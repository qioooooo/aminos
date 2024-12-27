using System;

namespace System
{
	// Token: 0x02000077 RID: 119
	public static class BitConverter
	{
		// Token: 0x0600068D RID: 1677 RVA: 0x000160B8 File Offset: 0x000150B8
		public static byte[] GetBytes(bool value)
		{
			return new byte[] { value ? 1 : 0 };
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000160D7 File Offset: 0x000150D7
		public static byte[] GetBytes(char value)
		{
			return BitConverter.GetBytes((short)value);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x000160E0 File Offset: 0x000150E0
		public unsafe static byte[] GetBytes(short value)
		{
			byte[] array = new byte[2];
			fixed (byte* ptr = array)
			{
				*(short*)ptr = value;
			}
			return array;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00016114 File Offset: 0x00015114
		public unsafe static byte[] GetBytes(int value)
		{
			byte[] array = new byte[4];
			fixed (byte* ptr = array)
			{
				*(int*)ptr = value;
			}
			return array;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00016148 File Offset: 0x00015148
		public unsafe static byte[] GetBytes(long value)
		{
			byte[] array = new byte[8];
			fixed (byte* ptr = array)
			{
				*(long*)ptr = value;
			}
			return array;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001617B File Offset: 0x0001517B
		[CLSCompliant(false)]
		public static byte[] GetBytes(ushort value)
		{
			return BitConverter.GetBytes((short)value);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00016184 File Offset: 0x00015184
		[CLSCompliant(false)]
		public static byte[] GetBytes(uint value)
		{
			return BitConverter.GetBytes((int)value);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001618C File Offset: 0x0001518C
		[CLSCompliant(false)]
		public static byte[] GetBytes(ulong value)
		{
			return BitConverter.GetBytes((long)value);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00016194 File Offset: 0x00015194
		public unsafe static byte[] GetBytes(float value)
		{
			return BitConverter.GetBytes(*(int*)(&value));
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001619F File Offset: 0x0001519F
		public unsafe static byte[] GetBytes(double value)
		{
			return BitConverter.GetBytes(*(long*)(&value));
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000161AA File Offset: 0x000151AA
		public static char ToChar(byte[] value, int startIndex)
		{
			return (char)BitConverter.ToInt16(value, startIndex);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000161B4 File Offset: 0x000151B4
		public unsafe static short ToInt16(byte[] value, int startIndex)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if ((ulong)startIndex >= (ulong)((long)value.Length))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 2)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			fixed (byte* ptr = &value[startIndex])
			{
				short num;
				if (startIndex % 2 == 0)
				{
					num = *(short*)ptr;
				}
				else if (BitConverter.IsLittleEndian)
				{
					num = (short)((int)(*ptr) | ((int)ptr[1] << 8));
				}
				else
				{
					num = (short)(((int)(*ptr) << 8) | (int)ptr[1]);
				}
				return num;
			}
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00016228 File Offset: 0x00015228
		public unsafe static int ToInt32(byte[] value, int startIndex)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if ((ulong)startIndex >= (ulong)((long)value.Length))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 4)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			fixed (byte* ptr = &value[startIndex])
			{
				int num;
				if (startIndex % 4 == 0)
				{
					num = *(int*)ptr;
				}
				else if (BitConverter.IsLittleEndian)
				{
					num = (int)(*ptr) | ((int)ptr[1] << 8) | ((int)ptr[2] << 16) | ((int)ptr[3] << 24);
				}
				else
				{
					num = ((int)(*ptr) << 24) | ((int)ptr[1] << 16) | ((int)ptr[2] << 8) | (int)ptr[3];
				}
				return num;
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000162C0 File Offset: 0x000152C0
		public unsafe static long ToInt64(byte[] value, int startIndex)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if ((ulong)startIndex >= (ulong)((long)value.Length))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 8)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			fixed (byte* ptr = &value[startIndex])
			{
				long num;
				if (startIndex % 8 == 0)
				{
					num = *(long*)ptr;
				}
				else if (BitConverter.IsLittleEndian)
				{
					int num2 = (int)(*ptr) | ((int)ptr[1] << 8) | ((int)ptr[2] << 16) | ((int)ptr[3] << 24);
					int num3 = (int)ptr[4] | ((int)ptr[5] << 8) | ((int)ptr[6] << 16) | ((int)ptr[7] << 24);
					num = (long)((ulong)num2 | (ulong)((ulong)((long)num3) << 32));
				}
				else
				{
					int num4 = ((int)(*ptr) << 24) | ((int)ptr[1] << 16) | ((int)ptr[2] << 8) | (int)ptr[3];
					int num5 = ((int)ptr[4] << 24) | ((int)ptr[5] << 16) | ((int)ptr[6] << 8) | (int)ptr[7];
					num = (long)((ulong)num5 | (ulong)((ulong)((long)num4) << 32));
				}
				return num;
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x000163BA File Offset: 0x000153BA
		[CLSCompliant(false)]
		public static ushort ToUInt16(byte[] value, int startIndex)
		{
			return (ushort)BitConverter.ToInt16(value, startIndex);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000163C4 File Offset: 0x000153C4
		[CLSCompliant(false)]
		public static uint ToUInt32(byte[] value, int startIndex)
		{
			return (uint)BitConverter.ToInt32(value, startIndex);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x000163CD File Offset: 0x000153CD
		[CLSCompliant(false)]
		public static ulong ToUInt64(byte[] value, int startIndex)
		{
			return (ulong)BitConverter.ToInt64(value, startIndex);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000163D8 File Offset: 0x000153D8
		public unsafe static float ToSingle(byte[] value, int startIndex)
		{
			int num = BitConverter.ToInt32(value, startIndex);
			return *(float*)(&num);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x000163F4 File Offset: 0x000153F4
		public unsafe static double ToDouble(byte[] value, int startIndex)
		{
			long num = BitConverter.ToInt64(value, startIndex);
			return *(double*)(&num);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001640D File Offset: 0x0001540D
		private static char GetHexValue(int i)
		{
			if (i < 10)
			{
				return (char)(i + 48);
			}
			return (char)(i - 10 + 65);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00016424 File Offset: 0x00015424
		public static string ToString(byte[] value, int startIndex, int length)
		{
			if (value == null)
			{
				throw new ArgumentNullException("byteArray");
			}
			int num = value.Length;
			if (startIndex < 0 || (startIndex >= num && startIndex > 0))
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			if (startIndex > num - length)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayPlusOffTooSmall"));
			}
			if (length == 0)
			{
				return string.Empty;
			}
			char[] array = new char[length * 3];
			int num2 = startIndex;
			for (int i = 0; i < length * 3; i += 3)
			{
				byte b = value[num2++];
				array[i] = BitConverter.GetHexValue((int)(b / 16));
				array[i + 1] = BitConverter.GetHexValue((int)(b % 16));
				array[i + 2] = '-';
			}
			return new string(array, 0, array.Length - 1);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x000164F4 File Offset: 0x000154F4
		public static string ToString(byte[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return BitConverter.ToString(value, 0, value.Length);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001650E File Offset: 0x0001550E
		public static string ToString(byte[] value, int startIndex)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return BitConverter.ToString(value, startIndex, value.Length - startIndex);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001652C File Offset: 0x0001552C
		public static bool ToBoolean(byte[] value, int startIndex)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (startIndex > value.Length - 1)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return value[startIndex] != 0;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00016585 File Offset: 0x00015585
		public unsafe static long DoubleToInt64Bits(double value)
		{
			return *(long*)(&value);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001658B File Offset: 0x0001558B
		public unsafe static double Int64BitsToDouble(long value)
		{
			return *(double*)(&value);
		}

		// Token: 0x04000215 RID: 533
		public static readonly bool IsLittleEndian = true;
	}
}
