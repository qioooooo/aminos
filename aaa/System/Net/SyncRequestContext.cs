using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020003C9 RID: 969
	internal class SyncRequestContext : RequestContextBase
	{
		// Token: 0x06001E77 RID: 7799 RVA: 0x00074844 File Offset: 0x00073844
		internal SyncRequestContext(int size)
		{
			base.BaseConstruction(this.Allocate(size));
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0007485C File Offset: 0x0007385C
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* Allocate(int size)
		{
			if (this.m_PinnedHandle.IsAllocated)
			{
				if (base.RequestBuffer.Length == size)
				{
					return base.RequestBlob;
				}
				this.m_PinnedHandle.Free();
			}
			base.SetBuffer(size);
			if (base.RequestBuffer == null)
			{
				return null;
			}
			this.m_PinnedHandle = GCHandle.Alloc(base.RequestBuffer, GCHandleType.Pinned);
			return (UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(base.RequestBuffer, 0);
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000748C8 File Offset: 0x000738C8
		internal void Reset(int size)
		{
			base.SetBlob(this.Allocate(size));
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000748D7 File Offset: 0x000738D7
		protected override void OnReleasePins()
		{
			if (this.m_PinnedHandle.IsAllocated)
			{
				this.m_PinnedHandle.Free();
			}
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000748F1 File Offset: 0x000738F1
		protected override void Dispose(bool disposing)
		{
			if (this.m_PinnedHandle.IsAllocated && (!NclUtilities.HasShutdownStarted || disposing))
			{
				this.m_PinnedHandle.Free();
			}
			base.Dispose(disposing);
		}

		// Token: 0x04001E4B RID: 7755
		private GCHandle m_PinnedHandle;
	}
}
