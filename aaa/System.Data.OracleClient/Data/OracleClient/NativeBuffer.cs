using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.OracleClient
{
	// Token: 0x0200001D RID: 29
	internal class NativeBuffer : DbBuffer
	{
		// Token: 0x060001AB RID: 427 RVA: 0x00059DC8 File Offset: 0x000591C8
		public NativeBuffer(int initialSize, bool zeroBuffer)
			: base(initialSize, zeroBuffer)
		{
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00059DE0 File Offset: 0x000591E0
		public NativeBuffer(int initialSize)
			: base(initialSize, false)
		{
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00059DF8 File Offset: 0x000591F8
		internal IntPtr DangerousGetDataPtr()
		{
			return base.DangerousGetHandle();
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00059E0C File Offset: 0x0005920C
		internal IntPtr DangerousGetDataPtr(int offset)
		{
			return ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00059E28 File Offset: 0x00059228
		internal IntPtr DangerousGetDataPtrWithBaseOffset(int offset)
		{
			return ADP.IntPtrOffset(base.DangerousGetHandle(), offset + base.BaseOffset);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00059E48 File Offset: 0x00059248
		internal static IntPtr HandleValueToTrace(NativeBuffer buffer)
		{
			return buffer.DangerousGetHandle();
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00059E5C File Offset: 0x0005925C
		internal string PtrToStringAnsi(int offset)
		{
			offset += base.BaseOffset;
			base.Validate(offset, 1);
			string text = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				int num = UnsafeNativeMethods.lstrlenA(intPtr);
				text = Marshal.PtrToStringAnsi(intPtr, num);
				base.Validate(offset, num + 1);
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

		// Token: 0x060001B2 RID: 434 RVA: 0x00059EDC File Offset: 0x000592DC
		internal string PtrToStringAnsi(int offset, int length)
		{
			offset += base.BaseOffset;
			base.Validate(offset, length);
			string text = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				text = Marshal.PtrToStringAnsi(intPtr, length);
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

		// Token: 0x060001B3 RID: 435 RVA: 0x00059F4C File Offset: 0x0005934C
		internal object PtrToStructure(int offset, Type oftype)
		{
			offset += base.BaseOffset;
			object obj = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				obj = Marshal.PtrToStructure(intPtr, oftype);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return obj;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00059FB4 File Offset: 0x000593B4
		internal static void SafeDispose(ref NativeBuffer_LongColumnData handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}
	}
}
