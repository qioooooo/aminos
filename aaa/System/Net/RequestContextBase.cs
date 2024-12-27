using System;

namespace System.Net
{
	// Token: 0x020003C7 RID: 967
	internal abstract class RequestContextBase : IDisposable
	{
		// Token: 0x06001E62 RID: 7778 RVA: 0x000745F6 File Offset: 0x000735F6
		protected unsafe void BaseConstruction(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* requestBlob)
		{
			if (requestBlob == null)
			{
				GC.SuppressFinalize(this);
				return;
			}
			this.m_MemoryBlob = requestBlob;
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x0007460B File Offset: 0x0007360B
		internal void ReleasePins()
		{
			this.m_OriginalBlobAddress = this.m_MemoryBlob;
			this.UnsetBlob();
			this.OnReleasePins();
		}

		// Token: 0x06001E64 RID: 7780
		protected abstract void OnReleasePins();

		// Token: 0x06001E65 RID: 7781 RVA: 0x00074625 File Offset: 0x00073625
		public void Close()
		{
			this.Dispose();
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x0007462D File Offset: 0x0007362D
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x00074636 File Offset: 0x00073636
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x00074638 File Offset: 0x00073638
		~RequestContextBase()
		{
			this.Dispose(false);
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x00074668 File Offset: 0x00073668
		internal unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* RequestBlob
		{
			get
			{
				return this.m_MemoryBlob;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001E6A RID: 7786 RVA: 0x00074670 File Offset: 0x00073670
		internal byte[] RequestBuffer
		{
			get
			{
				return this.m_BackingBuffer;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x00074678 File Offset: 0x00073678
		internal uint Size
		{
			get
			{
				return (uint)this.m_BackingBuffer.Length;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x00074684 File Offset: 0x00073684
		internal unsafe IntPtr OriginalBlobAddress
		{
			get
			{
				UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* memoryBlob = this.m_MemoryBlob;
				return (IntPtr)((void*)((memoryBlob == null) ? this.m_OriginalBlobAddress : memoryBlob));
			}
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x000746AB File Offset: 0x000736AB
		protected unsafe void SetBlob(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* requestBlob)
		{
			if (requestBlob == null)
			{
				this.UnsetBlob();
				return;
			}
			if (this.m_MemoryBlob == null)
			{
				GC.ReRegisterForFinalize(this);
			}
			this.m_MemoryBlob = requestBlob;
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x000746D0 File Offset: 0x000736D0
		protected void UnsetBlob()
		{
			if (this.m_MemoryBlob != null)
			{
				GC.SuppressFinalize(this);
			}
			this.m_MemoryBlob = null;
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x000746EA File Offset: 0x000736EA
		protected void SetBuffer(int size)
		{
			this.m_BackingBuffer = ((size == 0) ? null : new byte[size]);
		}

		// Token: 0x04001E46 RID: 7750
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* m_MemoryBlob;

		// Token: 0x04001E47 RID: 7751
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* m_OriginalBlobAddress;

		// Token: 0x04001E48 RID: 7752
		private byte[] m_BackingBuffer;
	}
}
