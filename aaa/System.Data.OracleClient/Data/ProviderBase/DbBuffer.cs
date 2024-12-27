using System;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.ProviderBase
{
	// Token: 0x0200001C RID: 28
	internal abstract class DbBuffer : SafeHandle
	{
		// Token: 0x06000196 RID: 406 RVA: 0x000597CC File Offset: 0x00058BCC
		protected DbBuffer(int initialSize, bool zeroBuffer)
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

		// Token: 0x06000197 RID: 407 RVA: 0x00059848 File Offset: 0x00058C48
		protected DbBuffer(int initialSize)
			: this(initialSize, true)
		{
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00059860 File Offset: 0x00058C60
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00059874 File Offset: 0x00058C74
		protected int BaseOffset
		{
			get
			{
				return this._baseOffset;
			}
			set
			{
				this._baseOffset = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00059888 File Offset: 0x00058C88
		public override bool IsInvalid
		{
			get
			{
				return IntPtr.Zero == this.handle;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600019B RID: 411 RVA: 0x000598A8 File Offset: 0x00058CA8
		internal int Length
		{
			get
			{
				return this._bufferLength;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000598BC File Offset: 0x00058CBC
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

		// Token: 0x0600019D RID: 413 RVA: 0x00059940 File Offset: 0x00058D40
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

		// Token: 0x0600019E RID: 414 RVA: 0x000599B0 File Offset: 0x00058DB0
		internal byte[] ReadBytes(int offset, int length)
		{
			byte[] array = new byte[length];
			return this.ReadBytes(offset, array, 0, length);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000599D0 File Offset: 0x00058DD0
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

		// Token: 0x060001A0 RID: 416 RVA: 0x00059A40 File Offset: 0x00058E40
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

		// Token: 0x060001A1 RID: 417 RVA: 0x00059AA0 File Offset: 0x00058EA0
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

		// Token: 0x060001A2 RID: 418 RVA: 0x00059B00 File Offset: 0x00058F00
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

		// Token: 0x060001A3 RID: 419 RVA: 0x00059B60 File Offset: 0x00058F60
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

		// Token: 0x060001A4 RID: 420 RVA: 0x00059B94 File Offset: 0x00058F94
		internal void StructureToPtr(int offset, object structure)
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

		// Token: 0x060001A5 RID: 421 RVA: 0x00059BF8 File Offset: 0x00058FF8
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

		// Token: 0x060001A6 RID: 422 RVA: 0x00059C68 File Offset: 0x00059068
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

		// Token: 0x060001A7 RID: 423 RVA: 0x00059CC8 File Offset: 0x000590C8
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

		// Token: 0x060001A8 RID: 424 RVA: 0x00059D28 File Offset: 0x00059128
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

		// Token: 0x060001A9 RID: 425 RVA: 0x00059D88 File Offset: 0x00059188
		[Conditional("DEBUG")]
		protected void ValidateCheck(int offset, int count)
		{
			this.Validate(offset, count);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00059DA0 File Offset: 0x000591A0
		protected void Validate(int offset, int count)
		{
			if (offset < 0 || count < 0 || this.Length < checked(offset + count))
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidBuffer);
			}
		}

		// Token: 0x040001AD RID: 429
		internal const int LMEM_FIXED = 0;

		// Token: 0x040001AE RID: 430
		internal const int LMEM_MOVEABLE = 2;

		// Token: 0x040001AF RID: 431
		internal const int LMEM_ZEROINIT = 64;

		// Token: 0x040001B0 RID: 432
		private readonly int _bufferLength;

		// Token: 0x040001B1 RID: 433
		private int _baseOffset;
	}
}
