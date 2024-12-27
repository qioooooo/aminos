using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x020003C8 RID: 968
	internal class AsyncRequestContext : RequestContextBase
	{
		// Token: 0x06001E71 RID: 7793 RVA: 0x00074706 File Offset: 0x00073706
		internal AsyncRequestContext(ListenerAsyncResult result)
		{
			this.m_Result = result;
			base.BaseConstruction(this.Allocate(0U));
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x00074724 File Offset: 0x00073724
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* Allocate(uint size)
		{
			uint num = ((size != 0U) ? size : ((base.RequestBuffer == null) ? 4096U : base.Size));
			if (this.m_NativeOverlapped != null && (ulong)num != (ulong)((long)base.RequestBuffer.Length))
			{
				NativeOverlapped* nativeOverlapped = this.m_NativeOverlapped;
				this.m_NativeOverlapped = null;
				Overlapped.Free(nativeOverlapped);
			}
			if (this.m_NativeOverlapped == null)
			{
				base.SetBuffer(checked((int)num));
				this.m_NativeOverlapped = new Overlapped
				{
					AsyncResult = this.m_Result
				}.Pack(ListenerAsyncResult.IOCallback, base.RequestBuffer);
				return (UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(base.RequestBuffer, 0);
			}
			return base.RequestBlob;
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000747CC File Offset: 0x000737CC
		internal unsafe void Reset(ulong requestId, uint size)
		{
			base.SetBlob(this.Allocate(size));
			base.RequestBlob->RequestId = requestId;
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x000747E8 File Offset: 0x000737E8
		protected unsafe override void OnReleasePins()
		{
			if (this.m_NativeOverlapped != null)
			{
				NativeOverlapped* nativeOverlapped = this.m_NativeOverlapped;
				this.m_NativeOverlapped = null;
				Overlapped.Free(nativeOverlapped);
			}
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x00074814 File Offset: 0x00073814
		protected override void Dispose(bool disposing)
		{
			if (this.m_NativeOverlapped != null && (!NclUtilities.HasShutdownStarted || disposing))
			{
				Overlapped.Free(this.m_NativeOverlapped);
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001E76 RID: 7798 RVA: 0x0007483C File Offset: 0x0007383C
		internal unsafe NativeOverlapped* NativeOverlapped
		{
			get
			{
				return this.m_NativeOverlapped;
			}
		}

		// Token: 0x04001E49 RID: 7753
		private unsafe NativeOverlapped* m_NativeOverlapped;

		// Token: 0x04001E4A RID: 7754
		private ListenerAsyncResult m_Result;
	}
}
