using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x02000085 RID: 133
	[StructLayout(LayoutKind.Sequential)]
	public sealed class EncoderParameter : IDisposable
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x0001CD74 File Offset: 0x0001BD74
		~EncoderParameter()
		{
			this.Dispose(false);
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x0001CDA4 File Offset: 0x0001BDA4
		// (set) Token: 0x06000791 RID: 1937 RVA: 0x0001CDB1 File Offset: 0x0001BDB1
		public Encoder Encoder
		{
			get
			{
				return new Encoder(this.parameterGuid);
			}
			set
			{
				this.parameterGuid = value.Guid;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0001CDBF File Offset: 0x0001BDBF
		public EncoderParameterValueType Type
		{
			get
			{
				return (EncoderParameterValueType)this.parameterValueType;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0001CDC7 File Offset: 0x0001BDC7
		public EncoderParameterValueType ValueType
		{
			get
			{
				return (EncoderParameterValueType)this.parameterValueType;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x0001CDCF File Offset: 0x0001BDCF
		public int NumberOfValues
		{
			get
			{
				return this.numberOfValues;
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001CDD7 File Offset: 0x0001BDD7
		public void Dispose()
		{
			this.Dispose(true);
			GC.KeepAlive(this);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001CDEC File Offset: 0x0001BDEC
		private void Dispose(bool disposing)
		{
			if (this.parameterValue != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.parameterValue);
			}
			this.parameterValue = IntPtr.Zero;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001CE18 File Offset: 0x0001BE18
		public EncoderParameter(Encoder encoder, byte value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = 1;
			this.numberOfValues = 1;
			this.parameterValue = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteByte(this.parameterValue, value);
			GC.KeepAlive(this);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001CE8C File Offset: 0x0001BE8C
		public EncoderParameter(Encoder encoder, byte value, bool undefined)
		{
			this.parameterGuid = encoder.Guid;
			if (undefined)
			{
				this.parameterValueType = 7;
			}
			else
			{
				this.parameterValueType = 1;
			}
			this.numberOfValues = 1;
			this.parameterValue = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteByte(this.parameterValue, value);
			GC.KeepAlive(this);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001CF0C File Offset: 0x0001BF0C
		public EncoderParameter(Encoder encoder, short value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = 3;
			this.numberOfValues = 1;
			this.parameterValue = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(short)));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteInt16(this.parameterValue, value);
			GC.KeepAlive(this);
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0001CF80 File Offset: 0x0001BF80
		public EncoderParameter(Encoder encoder, long value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = 4;
			this.numberOfValues = 1;
			this.parameterValue = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteInt32(this.parameterValue, (int)value);
			GC.KeepAlive(this);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0001CFF4 File Offset: 0x0001BFF4
		public EncoderParameter(Encoder encoder, int numerator, int denominator)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = 5;
			this.numberOfValues = 1;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(2 * num);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteInt32(this.parameterValue, numerator);
			Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, num), denominator);
			GC.KeepAlive(this);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001D07C File Offset: 0x0001C07C
		public EncoderParameter(Encoder encoder, long rangebegin, long rangeend)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = 6;
			this.numberOfValues = 1;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(2 * num);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteInt32(this.parameterValue, (int)rangebegin);
			Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, num), (int)rangeend);
			GC.KeepAlive(this);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0001D108 File Offset: 0x0001C108
		public EncoderParameter(Encoder encoder, int numerator1, int demoninator1, int numerator2, int demoninator2)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = 8;
			this.numberOfValues = 1;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(4 * num);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteInt32(this.parameterValue, numerator1);
			Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, num), demoninator1);
			Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, 2 * num), numerator2);
			Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, 3 * num), demoninator2);
			GC.KeepAlive(this);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0001D1BC File Offset: 0x0001C1BC
		public EncoderParameter(Encoder encoder, string value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = 2;
			this.numberOfValues = value.Length;
			this.parameterValue = Marshal.StringToHGlobalAnsi(value);
			GC.KeepAlive(this);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0001D21C File Offset: 0x0001C21C
		public EncoderParameter(Encoder encoder, byte[] value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = 1;
			this.numberOfValues = value.Length;
			this.parameterValue = Marshal.AllocHGlobal(this.numberOfValues);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.Copy(value, 0, this.parameterValue, this.numberOfValues);
			GC.KeepAlive(this);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001D290 File Offset: 0x0001C290
		public EncoderParameter(Encoder encoder, byte[] value, bool undefined)
		{
			this.parameterGuid = encoder.Guid;
			if (undefined)
			{
				this.parameterValueType = 7;
			}
			else
			{
				this.parameterValueType = 1;
			}
			this.numberOfValues = value.Length;
			this.parameterValue = Marshal.AllocHGlobal(this.numberOfValues);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.Copy(value, 0, this.parameterValue, this.numberOfValues);
			GC.KeepAlive(this);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0001D310 File Offset: 0x0001C310
		public EncoderParameter(Encoder encoder, short[] value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = 3;
			this.numberOfValues = value.Length;
			int num = Marshal.SizeOf(typeof(short));
			this.parameterValue = Marshal.AllocHGlobal(checked(this.numberOfValues * num));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.Copy(value, 0, this.parameterValue, this.numberOfValues);
			GC.KeepAlive(this);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001D394 File Offset: 0x0001C394
		public unsafe EncoderParameter(Encoder encoder, long[] value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = 4;
			this.numberOfValues = value.Length;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(checked(this.numberOfValues * num));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			int* ptr = (int*)(void*)this.parameterValue;
			fixed (long* ptr2 = value)
			{
				for (int i = 0; i < value.Length; i++)
				{
					ptr[i] = (int)ptr2[i];
				}
			}
			GC.KeepAlive(this);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001D44C File Offset: 0x0001C44C
		public EncoderParameter(Encoder encoder, int[] numerator, int[] denominator)
		{
			this.parameterGuid = encoder.Guid;
			if (numerator.Length != denominator.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			this.parameterValueType = 5;
			this.numberOfValues = numerator.Length;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(checked(this.numberOfValues * 2 * num));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			for (int i = 0; i < this.numberOfValues; i++)
			{
				Marshal.WriteInt32(EncoderParameter.Add(i * 2 * num, this.parameterValue), numerator[i]);
				Marshal.WriteInt32(EncoderParameter.Add((i * 2 + 1) * num, this.parameterValue), denominator[i]);
			}
			GC.KeepAlive(this);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001D514 File Offset: 0x0001C514
		public EncoderParameter(Encoder encoder, long[] rangebegin, long[] rangeend)
		{
			this.parameterGuid = encoder.Guid;
			if (rangebegin.Length != rangeend.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			this.parameterValueType = 6;
			this.numberOfValues = rangebegin.Length;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(checked(this.numberOfValues * 2 * num));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			for (int i = 0; i < this.numberOfValues; i++)
			{
				Marshal.WriteInt32(EncoderParameter.Add(i * 2 * num, this.parameterValue), (int)rangebegin[i]);
				Marshal.WriteInt32(EncoderParameter.Add((i * 2 + 1) * num, this.parameterValue), (int)rangeend[i]);
			}
			GC.KeepAlive(this);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0001D5DC File Offset: 0x0001C5DC
		public EncoderParameter(Encoder encoder, int[] numerator1, int[] denominator1, int[] numerator2, int[] denominator2)
		{
			this.parameterGuid = encoder.Guid;
			if (numerator1.Length != denominator1.Length || numerator1.Length != denominator2.Length || denominator1.Length != denominator2.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			this.parameterValueType = 8;
			this.numberOfValues = numerator1.Length;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(checked(this.numberOfValues * 4 * num));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			for (int i = 0; i < this.numberOfValues; i++)
			{
				Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, 4 * i * num), numerator1[i]);
				Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, (4 * i + 1) * num), denominator1[i]);
				Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, (4 * i + 2) * num), numerator2[i]);
				Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, (4 * i + 3) * num), denominator2[i]);
			}
			GC.KeepAlive(this);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0001D6EC File Offset: 0x0001C6EC
		public EncoderParameter(Encoder encoder, int NumberOfValues, int Type, int Value)
		{
			IntSecurity.UnmanagedCode.Demand();
			int num;
			switch (Type)
			{
			case 1:
			case 2:
				num = 1;
				break;
			case 3:
				num = 2;
				break;
			case 4:
				num = 4;
				break;
			case 5:
			case 6:
				num = 8;
				break;
			case 7:
				num = 1;
				break;
			case 8:
				num = 16;
				break;
			default:
				throw SafeNativeMethods.Gdip.StatusException(8);
			}
			int num2 = checked(num * NumberOfValues);
			this.parameterValue = Marshal.AllocHGlobal(num2);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			for (int i = 0; i < num2; i++)
			{
				Marshal.WriteByte(EncoderParameter.Add(this.parameterValue, i), Marshal.ReadByte((IntPtr)(Value + i)));
			}
			this.parameterValueType = Type;
			this.numberOfValues = NumberOfValues;
			this.parameterGuid = encoder.Guid;
			GC.KeepAlive(this);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0001D7C9 File Offset: 0x0001C7C9
		private static IntPtr Add(IntPtr a, int b)
		{
			return (IntPtr)((long)a + (long)b);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0001D7D9 File Offset: 0x0001C7D9
		private static IntPtr Add(int a, IntPtr b)
		{
			return (IntPtr)((long)a + (long)b);
		}

		// Token: 0x0400060E RID: 1550
		[MarshalAs(UnmanagedType.Struct)]
		private Guid parameterGuid;

		// Token: 0x0400060F RID: 1551
		private int numberOfValues;

		// Token: 0x04000610 RID: 1552
		private int parameterValueType;

		// Token: 0x04000611 RID: 1553
		private IntPtr parameterValue;
	}
}
