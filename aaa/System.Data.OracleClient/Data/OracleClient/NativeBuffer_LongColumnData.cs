using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System.Data.OracleClient
{
	// Token: 0x0200001F RID: 31
	internal sealed class NativeBuffer_LongColumnData : NativeBuffer
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x00059FE4 File Offset: 0x000593E4
		internal NativeBuffer_LongColumnData()
			: base(NativeBuffer_LongColumnData.ReservedSize)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this._currentChunk = this.handle;
				Marshal.WriteIntPtr(this._currentChunk, 0, IntPtr.Zero);
				Marshal.WriteInt32(this._currentChunk, NativeBuffer_LongColumnData.LengthOrIndicatorOffset, -2);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0005A05C File Offset: 0x0005945C
		internal int TotalLengthInBytes
		{
			get
			{
				IntPtr intPtr = this.handle;
				int num = 0;
				for (int i = 0; i < this._chunkCount; i++)
				{
					intPtr = Marshal.ReadIntPtr(intPtr);
					if (intPtr == IntPtr.Zero)
					{
						throw ADP.InternalError(ADP.InternalErrorCode.InvalidLongBuffer);
					}
					int num2 = Marshal.ReadInt32(intPtr, NativeBuffer_LongColumnData.LengthOrIndicatorOffset);
					if (num2 <= 0)
					{
						throw ADP.InternalError(ADP.InternalErrorCode.InvalidLongBuffer);
					}
					if (num2 > NativeBuffer_LongColumnData.MaxChunkSize)
					{
						throw ADP.InternalError(ADP.InternalErrorCode.InvalidLongBuffer);
					}
					checked
					{
						num += num2;
					}
				}
				return num;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0005A0CC File Offset: 0x000594CC
		internal static void CopyOutOfLineBytes(IntPtr longBuffer, int cbSourceOffset, byte[] destinationBuffer, int cbDestinationOffset, int cbCount)
		{
			if (IntPtr.Zero == longBuffer)
			{
				throw ADP.ArgumentNull("longBuffer");
			}
			int num = 0;
			int i = cbCount;
			while (i > 0)
			{
				longBuffer = Marshal.ReadIntPtr(longBuffer);
				if (IntPtr.Zero == longBuffer)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.InvalidLongBuffer);
				}
				int num2 = Marshal.ReadInt32(longBuffer, NativeBuffer_LongColumnData.LengthOrIndicatorOffset);
				if (num2 <= 0 || num2 > NativeBuffer_LongColumnData.MaxChunkSize)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.InvalidLongBuffer);
				}
				int num3 = cbSourceOffset - num;
				if (num3 < num2)
				{
					int num4 = Math.Min(i, num + num2 - cbSourceOffset);
					Marshal.Copy(ADP.IntPtrOffset(longBuffer, num3 + NativeBuffer_LongColumnData.ReservedSize), destinationBuffer, cbDestinationOffset, num4);
					cbSourceOffset += num4;
					cbDestinationOffset += num4;
					i -= num4;
				}
				num += num2;
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0005A17C File Offset: 0x0005957C
		internal static void CopyOutOfLineChars(IntPtr longBuffer, int cchSourceOffset, char[] destinationBuffer, int cchDestinationOffset, int cchCount)
		{
			if (IntPtr.Zero == longBuffer)
			{
				throw ADP.ArgumentNull("longBuffer");
			}
			int num = 0;
			int i = cchCount;
			while (i > 0)
			{
				longBuffer = Marshal.ReadIntPtr(longBuffer);
				if (IntPtr.Zero == longBuffer)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.InvalidLongBuffer);
				}
				int num2 = Marshal.ReadInt32(longBuffer, NativeBuffer_LongColumnData.LengthOrIndicatorOffset);
				if (num2 <= 0 || num2 > NativeBuffer_LongColumnData.MaxChunkSize)
				{
					throw ADP.InternalError(ADP.InternalErrorCode.InvalidLongBuffer);
				}
				if ((num2 & 1) != 0)
				{
					throw ADP.InvalidCast();
				}
				int num3 = num2 / 2;
				int num4 = cchSourceOffset - num;
				if (num4 < num3)
				{
					int num5 = Math.Min(i, num + num3 - cchSourceOffset);
					Marshal.Copy(ADP.IntPtrOffset(longBuffer, num4 * ADP.CharSize + NativeBuffer_LongColumnData.ReservedSize), destinationBuffer, cchDestinationOffset, num5);
					cchSourceOffset += num5;
					cchDestinationOffset += num5;
					i -= num5;
				}
				num += num3;
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0005A248 File Offset: 0x00059648
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal IntPtr GetChunk(out IntPtr lengthPtr)
		{
			IntPtr intPtr = Marshal.ReadIntPtr(this._currentChunk);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (IntPtr.Zero == intPtr)
				{
					intPtr = SafeNativeMethods.LocalAlloc(0, (IntPtr)NativeBuffer_LongColumnData.AllocationSize);
					if (IntPtr.Zero != intPtr)
					{
						Marshal.WriteIntPtr(intPtr, IntPtr.Zero);
						Marshal.WriteIntPtr(this._currentChunk, intPtr);
					}
				}
				if (IntPtr.Zero != intPtr)
				{
					Marshal.WriteInt32(intPtr, NativeBuffer_LongColumnData.LengthOrIndicatorOffset, -1);
					this._currentChunk = intPtr;
					this._chunkCount++;
				}
			}
			if (IntPtr.Zero == intPtr)
			{
				throw new OutOfMemoryException();
			}
			lengthPtr = ADP.IntPtrOffset(intPtr, NativeBuffer_LongColumnData.LengthOrIndicatorOffset);
			return ADP.IntPtrOffset(intPtr, NativeBuffer_LongColumnData.ReservedSize);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0005A324 File Offset: 0x00059724
		protected override bool ReleaseHandle()
		{
			IntPtr intPtr = this.handle;
			while (IntPtr.Zero != intPtr)
			{
				IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr);
				SafeNativeMethods.LocalFree(intPtr);
				intPtr = intPtr2;
			}
			return true;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0005A358 File Offset: 0x00059758
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Reset()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					IntPtr intPtr = (this._currentChunk = this.handle);
					while (IntPtr.Zero != intPtr)
					{
						IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr);
						Marshal.WriteInt32(intPtr, NativeBuffer_LongColumnData.LengthOrIndicatorOffset, -2);
						intPtr = intPtr2;
					}
					this._chunkCount = 0;
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x040001B2 RID: 434
		private const int ChunkIsFree = -2;

		// Token: 0x040001B3 RID: 435
		private const int ChunkToBeFilled = -1;

		// Token: 0x040001B4 RID: 436
		private IntPtr _currentChunk = IntPtr.Zero;

		// Token: 0x040001B5 RID: 437
		private int _chunkCount;

		// Token: 0x040001B6 RID: 438
		private static readonly int AllocationSize = 8184;

		// Token: 0x040001B7 RID: 439
		private static readonly int ReservedSize = 2 * IntPtr.Size;

		// Token: 0x040001B8 RID: 440
		internal static readonly int MaxChunkSize = NativeBuffer_LongColumnData.AllocationSize - NativeBuffer_LongColumnData.ReservedSize;

		// Token: 0x040001B9 RID: 441
		private static readonly int LengthOrIndicatorOffset = IntPtr.Size;

		// Token: 0x040001BA RID: 442
		private static readonly OutOfMemoryException OutOfMemory = new OutOfMemoryException();
	}
}
