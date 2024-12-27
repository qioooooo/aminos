using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

// Token: 0x02000009 RID: 9
[CLSCompliant(false)]
internal class NativeOledbWrapper
{
	// Token: 0x06000084 RID: 132 RVA: 0x001C4B2C File Offset: 0x001C3F2C
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static int IChapteredRowsetReleaseChapter(IntPtr ptr, IntPtr chapter)
	{
		int num = -2147418113;
		uint num2 = 0U;
		uint num3 = chapter.ToPointer();
		IChapteredRowset* ptr2 = null;
		IUnknown* ptr3 = (IUnknown*)ptr.ToPointer();
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
		}
		finally
		{
			num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr3, ref <Module>.IID_IChapteredRowset, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr3)));
			if (null != ptr2)
			{
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)*), ptr2, num3, (uint*)(&num2), (IntPtr)(*(*(int*)ptr2 + 16)));
				IChapteredRowset* ptr4 = ptr2;
				uint num4 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
			}
		}
		return num;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x001C4BB0 File Offset: 0x001C3FB0
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static int ITransactionAbort(IntPtr ptr)
	{
		int num = -2147418113;
		ITransactionLocal* ptr2 = null;
		IUnknown* ptr3 = (IUnknown*)ptr.ToPointer();
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
		}
		finally
		{
			num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr3, ref <Module>.IID_ITransactionLocal, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr3)));
			if (null != ptr2)
			{
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,BOID*,System.Int32,System.Int32), ptr2, null, 0, 0, (IntPtr)(*(*(int*)ptr2 + 16)));
				ITransactionLocal* ptr4 = ptr2;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
			}
		}
		return num;
	}

	// Token: 0x06000086 RID: 134 RVA: 0x001C4C2C File Offset: 0x001C402C
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static int ITransactionCommit(IntPtr ptr)
	{
		int num = -2147418113;
		ITransactionLocal* ptr2 = null;
		IUnknown* ptr3 = (IUnknown*)ptr.ToPointer();
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
		}
		finally
		{
			num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr3, ref <Module>.IID_ITransactionLocal, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr3)));
			if (null != ptr2)
			{
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)), ptr2, 0, 2, 0, (IntPtr)(*(*(int*)ptr2 + 12)));
				ITransactionLocal* ptr4 = ptr2;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
			}
		}
		return num;
	}

	// Token: 0x06000087 RID: 135 RVA: 0x001C4CA8 File Offset: 0x001C40A8
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool MemoryCompare(IntPtr buf1, IntPtr buf2, int count)
	{
		Debug.Assert(buf1 != buf2, "buf1 and buf2 are the same");
		byte b;
		if (buf1.ToInt64() >= buf2.ToInt64() && buf2.ToInt64() + (long)count > buf1.ToInt64())
		{
			b = 0;
		}
		else
		{
			b = 1;
		}
		Debug.Assert(b != 0, "overlapping region buf1");
		byte b2;
		if (buf2.ToInt64() >= buf1.ToInt64() && buf1.ToInt64() + (long)count > buf2.ToInt64())
		{
			b2 = 0;
		}
		else
		{
			b2 = 1;
		}
		Debug.Assert(b2 != 0, "overlapping region buf2");
		byte b3 = ((0 <= count) ? 1 : 0);
		Debug.Assert(b3 != 0, "negative count");
		uint num = (uint)count;
		void* ptr = buf2.ToPointer();
		void* ptr2 = buf1.ToPointer();
		int num2;
		if (count != 0)
		{
			sbyte b4 = *(sbyte*)ptr2;
			sbyte b5 = *(sbyte*)ptr;
			if (b4 >= b5)
			{
				while (b4 <= b5)
				{
					if (num == 1U)
					{
						goto IL_00DB;
					}
					num -= 1U;
					ptr2 = (void*)((byte*)ptr2 + 1);
					ptr = (void*)((byte*)ptr + 1);
					b4 = *(sbyte*)ptr2;
					b5 = *(sbyte*)ptr;
					if (b4 < b5)
					{
						break;
					}
				}
			}
			num2 = 1;
			goto IL_00DE;
		}
		IL_00DB:
		num2 = 0;
		IL_00DE:
		return (byte)num2 != 0;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x001C4D98 File Offset: 0x001C4198
	internal static void MemoryCopy(IntPtr dst, IntPtr src, int count)
	{
		Debug.Assert(dst != src, "dst and src are the same");
		byte b;
		if (dst.ToInt64() >= src.ToInt64() && src.ToInt64() + (long)count > dst.ToInt64())
		{
			b = 0;
		}
		else
		{
			b = 1;
		}
		Debug.Assert(b != 0, "overlapping region dst");
		byte b2;
		if (src.ToInt64() >= dst.ToInt64() && dst.ToInt64() + (long)count > src.ToInt64())
		{
			b2 = 0;
		}
		else
		{
			b2 = 1;
		}
		Debug.Assert(b2 != 0, "overlapping region src");
		byte b3 = ((0 <= count) ? 1 : 0);
		Debug.Assert(b3 != 0, "negative count");
		cpblk(dst.ToPointer(), src.ToPointer(), count);
	}

	// Token: 0x04000069 RID: 105
	internal static int SizeOfPROPVARIANT = 16;
}
