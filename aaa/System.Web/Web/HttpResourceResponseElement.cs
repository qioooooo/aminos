using System;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200009A RID: 154
	internal sealed class HttpResourceResponseElement : IHttpResponseElement
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x00023279 File Offset: 0x00022279
		internal HttpResourceResponseElement(IntPtr data, int offset, int size)
		{
			this._data = data;
			this._offset = offset;
			this._size = size;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00023296 File Offset: 0x00022296
		long IHttpResponseElement.GetSize()
		{
			return (long)this._size;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x000232A0 File Offset: 0x000222A0
		byte[] IHttpResponseElement.GetBytes()
		{
			if (this._size > 0)
			{
				byte[] array = new byte[this._size];
				Misc.CopyMemory(this._data, this._offset, array, 0, this._size);
				return array;
			}
			return null;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000232DE File Offset: 0x000222DE
		void IHttpResponseElement.Send(HttpWorkerRequest wr)
		{
			if (this._size > 0)
			{
				wr.SendResponseFromMemory(new IntPtr(this._data.ToInt64() + (long)this._offset), this._size);
			}
		}

		// Token: 0x04001175 RID: 4469
		private IntPtr _data;

		// Token: 0x04001176 RID: 4470
		private int _offset;

		// Token: 0x04001177 RID: 4471
		private int _size;
	}
}
