using System;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.ProviderBase
{
	// Token: 0x02000209 RID: 521
	internal abstract class DbBuffer : SafeHandle
	{
		// Token: 0x06001CB8 RID: 7352 RVA: 0x0024DE40 File Offset: 0x0024D240
		private DbBuffer(int initialSize, bool zeroBuffer)
			: base(IntPtr.Zero, true)
		{
			if (0 < initialSize)
			{
				int num = (zeroBuffer ? 64 : 0);
				this._bufferLength = initialSize;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					this.handle = SafeNativeMethods.LocalAlloc(num, (IntPtr)initialSize);
				}
				if (IntPtr.Zero == this.handle)
				{
					throw new OutOfMemoryException();
				}
			}
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x0024DEBC File Offset: 0x0024D2BC
		protected DbBuffer(int initialSize)
			: this(initialSize, true)
		{
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x0024DED4 File Offset: 0x0024D2D4
		protected DbBuffer(IntPtr invalidHandleValue, bool ownsHandle)
			: base(invalidHandleValue, ownsHandle)
		{
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001CBB RID: 7355 RVA: 0x0024DEEC File Offset: 0x0024D2EC
		private int BaseOffset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001CBC RID: 7356 RVA: 0x0024DEFC File Offset: 0x0024D2FC
		public override bool IsInvalid
		{
			get
			{
				return IntPtr.Zero == this.handle;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x0024DF1C File Offset: 0x0024D31C
		internal int Length
		{
			get
			{
				return this._bufferLength;
			}
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x0024DF30 File Offset: 0x0024D330
		internal string PtrToStringUni(int offset)
		{
			offset += this.BaseOffset;
			this.Validate(offset, 2);
			string text = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				int num = UnsafeNativeMethods.lstrlenW(intPtr);
				this.Validate(offset, 2 * (num + 1));
				text = Marshal.PtrToStringUni(intPtr, num);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return text;
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x0024DFB4 File Offset: 0x0024D3B4
		internal string PtrToStringUni(int offset, int length)
		{
			offset += this.BaseOffset;
			this.Validate(offset, 2 * length);
			string text = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				text = Marshal.PtrToStringUni(intPtr, length);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return text;
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x0024E024 File Offset: 0x0024D424
		internal byte ReadByte(int offset)
		{
			offset += this.BaseOffset;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			byte b;
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				b = Marshal.ReadByte(intPtr, offset);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return b;
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x0024E084 File Offset: 0x0024D484
		internal byte[] ReadBytes(int offset, int length)
		{
			byte[] array = new byte[length];
			return this.ReadBytes(offset, array, 0, length);
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x0024E0A4 File Offset: 0x0024D4A4
		internal byte[] ReadBytes(int offset, byte[] destination, int startIndex, int length)
		{
			offset += this.BaseOffset;
			this.Validate(offset, length);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				Marshal.Copy(intPtr, destination, startIndex, length);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return destination;
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x0024E114 File Offset: 0x0024D514
		internal char ReadChar(int offset)
		{
			short num = this.ReadInt16(offset);
			return (char)num;
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x0024E12C File Offset: 0x0024D52C
		internal char[] ReadChars(int offset, char[] destination, int startIndex, int length)
		{
			offset += this.BaseOffset;
			this.Validate(offset, 2 * length);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				Marshal.Copy(intPtr, destination, startIndex, length);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return destination;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x0024E1A0 File Offset: 0x0024D5A0
		internal double ReadDouble(int offset)
		{
			long num = this.ReadInt64(offset);
			return BitConverter.Int64BitsToDouble(num);
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x0024E1BC File Offset: 0x0024D5BC
		internal short ReadInt16(int offset)
		{
			offset += this.BaseOffset;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			short num;
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				num = Marshal.ReadInt16(intPtr, offset);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x0024E21C File Offset: 0x0024D61C
		internal void ReadInt16Array(int offset, short[] destination, int startIndex, int length)
		{
			offset += this.BaseOffset;
			this.Validate(offset, 2 * length);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				Marshal.Copy(intPtr, destination, startIndex, length);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x0024E28C File Offset: 0x0024D68C
		internal int ReadInt32(int offset)
		{
			offset += this.BaseOffset;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			int num;
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				num = Marshal.ReadInt32(intPtr, offset);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x0024E2EC File Offset: 0x0024D6EC
		internal void ReadInt32Array(int offset, int[] destination, int startIndex, int length)
		{
			offset += this.BaseOffset;
			this.Validate(offset, 4 * length);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				Marshal.Copy(intPtr, destination, startIndex, length);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x0024E35C File Offset: 0x0024D75C
		internal long ReadInt64(int offset)
		{
			offset += this.BaseOffset;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			long num;
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				num = Marshal.ReadInt64(intPtr, offset);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x0024E3BC File Offset: 0x0024D7BC
		internal IntPtr ReadIntPtr(int offset)
		{
			offset += this.BaseOffset;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			IntPtr intPtr2;
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				intPtr2 = Marshal.ReadIntPtr(intPtr, offset);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return intPtr2;
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x0024E41C File Offset: 0x0024D81C
		internal unsafe float ReadSingle(int offset)
		{
			int num = this.ReadInt32(offset);
			return *(float*)(&num);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x0024E438 File Offset: 0x0024D838
		protected override bool ReleaseHandle()
		{
			IntPtr handle = this.handle;
			this.handle = IntPtr.Zero;
			if (IntPtr.Zero != handle)
			{
				SafeNativeMethods.LocalFree(handle);
			}
			return true;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x0024E46C File Offset: 0x0024D86C
		private void StructureToPtr(int offset, object structure)
		{
			offset += this.BaseOffset;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				Marshal.StructureToPtr(structure, intPtr, false);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x0024E4D0 File Offset: 0x0024D8D0
		internal void WriteByte(int offset, byte value)
		{
			offset += this.BaseOffset;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				Marshal.WriteByte(intPtr, offset, value);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x0024E530 File Offset: 0x0024D930
		internal void WriteBytes(int offset, byte[] source, int startIndex, int length)
		{
			offset += this.BaseOffset;
			this.Validate(offset, length);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				Marshal.Copy(source, startIndex, intPtr, length);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x0024E5A0 File Offset: 0x0024D9A0
		internal void WriteCharArray(int offset, char[] source, int startIndex, int length)
		{
			offset += this.BaseOffset;
			this.Validate(offset, 2 * length);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				Marshal.Copy(source, startIndex, intPtr, length);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x0024E610 File Offset: 0x0024DA10
		internal void WriteDouble(int offset, double value)
		{
			this.WriteInt64(offset, BitConverter.DoubleToInt64Bits(value));
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x0024E62C File Offset: 0x0024DA2C
		internal void WriteInt16(int offset, short value)
		{
			offset += this.BaseOffset;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				Marshal.WriteInt16(intPtr, offset, value);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x0024E68C File Offset: 0x0024DA8C
		internal void WriteInt16Array(int offset, short[] source, int startIndex, int length)
		{
			offset += this.BaseOffset;
			this.Validate(offset, 2 * length);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				Marshal.Copy(source, startIndex, intPtr, length);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x0024E6FC File Offset: 0x0024DAFC
		internal void WriteInt32(int offset, int value)
		{
			offset += this.BaseOffset;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				Marshal.WriteInt32(intPtr, offset, value);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x0024E75C File Offset: 0x0024DB5C
		internal void WriteInt32Array(int offset, int[] source, int startIndex, int length)
		{
			offset += this.BaseOffset;
			this.Validate(offset, 4 * length);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				Marshal.Copy(source, startIndex, intPtr, length);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x0024E7CC File Offset: 0x0024DBCC
		internal void WriteInt64(int offset, long value)
		{
			offset += this.BaseOffset;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				Marshal.WriteInt64(intPtr, offset, value);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x0024E82C File Offset: 0x0024DC2C
		internal void WriteIntPtr(int offset, IntPtr value)
		{
			offset += this.BaseOffset;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				Marshal.WriteIntPtr(intPtr, offset, value);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x0024E88C File Offset: 0x0024DC8C
		internal unsafe void WriteSingle(int offset, float value)
		{
			this.WriteInt32(offset, *(int*)(&value));
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x0024E8A4 File Offset: 0x0024DCA4
		internal void ZeroMemory()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				SafeNativeMethods.ZeroMemory(intPtr, (IntPtr)this.Length);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x0024E900 File Offset: 0x0024DD00
		internal Guid ReadGuid(int offset)
		{
			byte[] array = new byte[16];
			this.ReadBytes(offset, array, 0, 16);
			return new Guid(array);
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x0024E928 File Offset: 0x0024DD28
		internal void WriteGuid(int offset, Guid value)
		{
			this.StructureToPtr(offset, value);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x0024E944 File Offset: 0x0024DD44
		internal DateTime ReadDate(int offset)
		{
			short[] array = new short[3];
			this.ReadInt16Array(offset, array, 0, 3);
			return new DateTime((int)((ushort)array[0]), (int)((ushort)array[1]), (int)((ushort)array[2]));
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x0024E974 File Offset: 0x0024DD74
		internal void WriteDate(int offset, DateTime value)
		{
			short[] array = new short[]
			{
				(short)value.Year,
				(short)value.Month,
				(short)value.Day
			};
			this.WriteInt16Array(offset, array, 0, 3);
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x0024E9B8 File Offset: 0x0024DDB8
		internal TimeSpan ReadTime(int offset)
		{
			short[] array = new short[3];
			this.ReadInt16Array(offset, array, 0, 3);
			return new TimeSpan((int)((ushort)array[0]), (int)((ushort)array[1]), (int)((ushort)array[2]));
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x0024E9E8 File Offset: 0x0024DDE8
		internal void WriteTime(int offset, TimeSpan value)
		{
			short[] array = new short[]
			{
				(short)value.Hours,
				(short)value.Minutes,
				(short)value.Seconds
			};
			this.WriteInt16Array(offset, array, 0, 3);
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x0024EA2C File Offset: 0x0024DE2C
		internal DateTime ReadDateTime(int offset)
		{
			short[] array = new short[6];
			this.ReadInt16Array(offset, array, 0, 6);
			int num = this.ReadInt32(offset + 12);
			DateTime dateTime = new DateTime((int)((ushort)array[0]), (int)((ushort)array[1]), (int)((ushort)array[2]), (int)((ushort)array[3]), (int)((ushort)array[4]), (int)((ushort)array[5]));
			return dateTime.AddTicks((long)(num / 100));
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x0024EA80 File Offset: 0x0024DE80
		internal void WriteDateTime(int offset, DateTime value)
		{
			int num = (int)(value.Ticks % 10000000L) * 100;
			short[] array = new short[]
			{
				(short)value.Year,
				(short)value.Month,
				(short)value.Day,
				(short)value.Hour,
				(short)value.Minute,
				(short)value.Second
			};
			this.WriteInt16Array(offset, array, 0, 6);
			this.WriteInt32(offset + 12, num);
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x0024EB00 File Offset: 0x0024DF00
		internal decimal ReadNumeric(int offset)
		{
			byte[] array = new byte[20];
			this.ReadBytes(offset, array, 1, 19);
			int[] array2 = new int[]
			{
				0,
				0,
				0,
				(int)array[2] << 16
			};
			if (array[3] == 0)
			{
				array2[3] |= int.MinValue;
			}
			array2[0] = BitConverter.ToInt32(array, 4);
			array2[1] = BitConverter.ToInt32(array, 8);
			array2[2] = BitConverter.ToInt32(array, 12);
			if (BitConverter.ToInt32(array, 16) != 0)
			{
				throw ADP.NumericToDecimalOverflow();
			}
			return new decimal(array2);
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x0024EB84 File Offset: 0x0024DF84
		internal void WriteNumeric(int offset, decimal value, byte precision)
		{
			int[] bits = decimal.GetBits(value);
			byte[] array = new byte[20];
			array[1] = precision;
			Buffer.BlockCopy(bits, 14, array, 2, 2);
			array[3] = ((array[3] == 0) ? 1 : 0);
			Buffer.BlockCopy(bits, 0, array, 4, 12);
			array[16] = 0;
			array[17] = 0;
			array[18] = 0;
			array[19] = 0;
			this.WriteBytes(offset, array, 1, 19);
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x0024EBE8 File Offset: 0x0024DFE8
		[Conditional("DEBUG")]
		protected void ValidateCheck(int offset, int count)
		{
			this.Validate(offset, count);
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x0024EC00 File Offset: 0x0024E000
		protected void Validate(int offset, int count)
		{
			if (offset < 0 || count < 0 || this.Length < checked(offset + count))
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidBuffer);
			}
		}

		// Token: 0x04001085 RID: 4229
		internal const int LMEM_FIXED = 0;

		// Token: 0x04001086 RID: 4230
		internal const int LMEM_MOVEABLE = 2;

		// Token: 0x04001087 RID: 4231
		internal const int LMEM_ZEROINIT = 64;

		// Token: 0x04001088 RID: 4232
		private readonly int _bufferLength;
	}
}
