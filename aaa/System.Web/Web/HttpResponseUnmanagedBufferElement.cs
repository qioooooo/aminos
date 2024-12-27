using System;
using System.Text;
using System.Threading;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000099 RID: 153
	internal sealed class HttpResponseUnmanagedBufferElement : HttpBaseMemoryResponseBufferElement, IHttpResponseElement
	{
		// Token: 0x060007DA RID: 2010 RVA: 0x00022ED7 File Offset: 0x00021ED7
		static HttpResponseUnmanagedBufferElement()
		{
			if (HttpRuntime.UseIntegratedPipeline)
			{
				HttpResponseUnmanagedBufferElement.s_Pool = UnsafeIISMethods.MgdGetBufferPool(BufferingParams.INTEGRATED_MODE_BUFFER_SIZE);
				return;
			}
			HttpResponseUnmanagedBufferElement.s_Pool = UnsafeNativeMethods.BufferPoolGetPool(31744, 64);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00022F04 File Offset: 0x00021F04
		internal HttpResponseUnmanagedBufferElement()
		{
			if (HttpRuntime.UseIntegratedPipeline)
			{
				this._data = UnsafeIISMethods.MgdGetBuffer(HttpResponseUnmanagedBufferElement.s_Pool);
				this._size = BufferingParams.INTEGRATED_MODE_BUFFER_SIZE;
			}
			else
			{
				this._data = UnsafeNativeMethods.BufferPoolGetBuffer(HttpResponseUnmanagedBufferElement.s_Pool);
				this._size = 31744;
			}
			if (this._data == IntPtr.Zero)
			{
				throw new OutOfMemoryException();
			}
			this._free = this._size;
			this._recycle = true;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00022F84 File Offset: 0x00021F84
		protected override void Finalize()
		{
			try
			{
				IntPtr intPtr = Interlocked.Exchange(ref this._data, IntPtr.Zero);
				if (intPtr != IntPtr.Zero)
				{
					if (HttpRuntime.UseIntegratedPipeline)
					{
						UnsafeIISMethods.MgdReturnBuffer(intPtr);
					}
					else
					{
						UnsafeNativeMethods.BufferPoolReleaseBuffer(intPtr);
					}
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00022FE0 File Offset: 0x00021FE0
		internal override HttpResponseBufferElement Clone()
		{
			int num = this._size - this._free;
			byte[] array = new byte[num];
			Misc.CopyMemory(this._data, 0, array, 0, num);
			return new HttpResponseBufferElement(array, num);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00023018 File Offset: 0x00022018
		internal override void Recycle()
		{
			if (this._recycle)
			{
				this.ForceRecycle();
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00023028 File Offset: 0x00022028
		private void ForceRecycle()
		{
			IntPtr intPtr = Interlocked.Exchange(ref this._data, IntPtr.Zero);
			if (intPtr != IntPtr.Zero)
			{
				this._free = 0;
				this._recycle = false;
				if (HttpRuntime.UseIntegratedPipeline)
				{
					UnsafeIISMethods.MgdReturnBuffer(intPtr);
				}
				else
				{
					UnsafeNativeMethods.BufferPoolReleaseBuffer(intPtr);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00023080 File Offset: 0x00022080
		internal override int Append(byte[] data, int offset, int size)
		{
			if (this._free == 0 || size == 0)
			{
				return 0;
			}
			int num = ((this._free >= size) ? size : this._free);
			Misc.CopyMemory(data, offset, this._data, this._size - this._free, num);
			this._free -= num;
			return num;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000230D8 File Offset: 0x000220D8
		internal override int Append(IntPtr data, int offset, int size)
		{
			if (this._free == 0 || size == 0)
			{
				return 0;
			}
			int num = ((this._free >= size) ? size : this._free);
			Misc.CopyMemory(data, offset, this._data, this._size - this._free, num);
			this._free -= num;
			return num;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0002312F File Offset: 0x0002212F
		internal void AdjustSize(int size)
		{
			this._free -= size;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00023140 File Offset: 0x00022140
		internal override void AppendEncodedChars(char[] data, int offset, int size, Encoder encoder, bool flushEncoder)
		{
			int num = HttpResponseUnmanagedBufferElement.UnsafeAppendEncodedChars(data, offset, size, this._data, this._size - this._free, this._free, encoder, flushEncoder);
			this._free -= num;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00023184 File Offset: 0x00022184
		private unsafe static int UnsafeAppendEncodedChars(char[] src, int srcOffset, int srcSize, IntPtr dest, int destOffset, int destSize, Encoder encoder, bool flushEncoder)
		{
			byte* ptr = (byte*)(void*)dest + destOffset;
			int bytes;
			fixed (char* ptr2 = src)
			{
				bytes = encoder.GetBytes(ptr2 + srcOffset, srcSize, ptr, destSize, flushEncoder);
			}
			return bytes;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x000231CD File Offset: 0x000221CD
		long IHttpResponseElement.GetSize()
		{
			return (long)(this._size - this._free);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x000231E0 File Offset: 0x000221E0
		byte[] IHttpResponseElement.GetBytes()
		{
			int num = this._size - this._free;
			if (num > 0)
			{
				byte[] array = new byte[num];
				Misc.CopyMemory(this._data, 0, array, 0, num);
				return array;
			}
			return null;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00023218 File Offset: 0x00022218
		void IHttpResponseElement.Send(HttpWorkerRequest wr)
		{
			int num = this._size - this._free;
			if (num > 0)
			{
				wr.SendResponseFromMemory(this._data, num, true);
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00023248 File Offset: 0x00022248
		internal unsafe IntPtr FreeLocation
		{
			get
			{
				int num = this._size - this._free;
				byte* ptr = (byte*)this._data.ToPointer();
				ptr += num;
				return new IntPtr((void*)ptr);
			}
		}

		// Token: 0x04001173 RID: 4467
		private IntPtr _data;

		// Token: 0x04001174 RID: 4468
		private static IntPtr s_Pool;
	}
}
