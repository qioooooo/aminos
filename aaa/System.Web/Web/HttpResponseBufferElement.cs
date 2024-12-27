using System;
using System.Text;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000098 RID: 152
	internal sealed class HttpResponseBufferElement : HttpBaseMemoryResponseBufferElement, IHttpResponseElement
	{
		// Token: 0x060007D0 RID: 2000 RVA: 0x00022D33 File Offset: 0x00021D33
		internal HttpResponseBufferElement(byte[] data, int size)
		{
			this._data = data;
			this._size = size;
			this._free = 0;
			this._recycle = false;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00022D58 File Offset: 0x00021D58
		internal override HttpResponseBufferElement Clone()
		{
			int num = this._size - this._free;
			byte[] array = new byte[num];
			Buffer.BlockCopy(this._data, 0, array, 0, num);
			return new HttpResponseBufferElement(array, num);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00022D90 File Offset: 0x00021D90
		internal override void Recycle()
		{
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00022D94 File Offset: 0x00021D94
		internal override int Append(byte[] data, int offset, int size)
		{
			if (this._free == 0 || size == 0)
			{
				return 0;
			}
			int num = ((this._free >= size) ? size : this._free);
			Buffer.BlockCopy(data, offset, this._data, this._size - this._free, num);
			this._free -= num;
			return num;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00022DEC File Offset: 0x00021DEC
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

		// Token: 0x060007D5 RID: 2005 RVA: 0x00022E44 File Offset: 0x00021E44
		internal override void AppendEncodedChars(char[] data, int offset, int size, Encoder encoder, bool flushEncoder)
		{
			int bytes = encoder.GetBytes(data, offset, size, this._data, this._size - this._free, flushEncoder);
			this._free -= bytes;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00022E7F File Offset: 0x00021E7F
		long IHttpResponseElement.GetSize()
		{
			return (long)(this._size - this._free);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00022E8F File Offset: 0x00021E8F
		byte[] IHttpResponseElement.GetBytes()
		{
			return this._data;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00022E98 File Offset: 0x00021E98
		void IHttpResponseElement.Send(HttpWorkerRequest wr)
		{
			int num = this._size - this._free;
			if (num > 0)
			{
				wr.SendResponseFromMemory(this._data, num);
			}
		}

		// Token: 0x04001171 RID: 4465
		private byte[] _data;

		// Token: 0x04001172 RID: 4466
		private static UbyteBufferAllocator s_Allocator = new UbyteBufferAllocator(31744, 64);
	}
}
