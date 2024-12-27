using System;
using System.IO;

namespace System.Web
{
	// Token: 0x0200009B RID: 155
	internal sealed class HttpFileResponseElement : IHttpResponseElement
	{
		// Token: 0x060007ED RID: 2029 RVA: 0x0002330D File Offset: 0x0002230D
		internal HttpFileResponseElement(string filename, long offset, long size, bool isImpersonating, bool supportsLongTransmitFile)
			: this(filename, offset, size, isImpersonating, true, supportsLongTransmitFile)
		{
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0002331D File Offset: 0x0002231D
		internal HttpFileResponseElement(string filename, long offset, long size)
			: this(filename, offset, size, false, false, false)
		{
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0002332C File Offset: 0x0002232C
		private HttpFileResponseElement(string filename, long offset, long size, bool isImpersonating, bool useTransmitFile, bool supportsLongTransmitFile)
		{
			if ((!supportsLongTransmitFile && size > 2147483647L) || size < 0L)
			{
				throw new ArgumentOutOfRangeException("size", size, SR.GetString("Invalid_size"));
			}
			if ((!supportsLongTransmitFile && offset > 2147483647L) || offset < 0L)
			{
				throw new ArgumentOutOfRangeException("offset", offset, SR.GetString("Invalid_size"));
			}
			this._filename = filename;
			this._offset = offset;
			this._size = size;
			this._isImpersonating = isImpersonating;
			this._useTransmitFile = useTransmitFile;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x000233BE File Offset: 0x000223BE
		long IHttpResponseElement.GetSize()
		{
			return this._size;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x000233C8 File Offset: 0x000223C8
		byte[] IHttpResponseElement.GetBytes()
		{
			if (this._size == 0L)
			{
				return null;
			}
			byte[] array = null;
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(this._filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				long length = fileStream.Length;
				if (this._offset < 0L || this._size > length - this._offset)
				{
					throw new HttpException(SR.GetString("Invalid_range"));
				}
				if (this._offset > 0L)
				{
					fileStream.Seek(this._offset, SeekOrigin.Begin);
				}
				int num = (int)this._size;
				array = new byte[num];
				int num2 = 0;
				do
				{
					int num3 = fileStream.Read(array, num2, num);
					if (num3 == 0)
					{
						break;
					}
					num2 += num3;
					num -= num3;
				}
				while (num > 0);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
			return array;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0002348C File Offset: 0x0002248C
		void IHttpResponseElement.Send(HttpWorkerRequest wr)
		{
			if (this._size > 0L)
			{
				if (this._useTransmitFile)
				{
					wr.TransmitFile(this._filename, this._offset, this._size, this._isImpersonating);
					return;
				}
				wr.SendResponseFromFile(this._filename, this._offset, this._size);
			}
		}

		// Token: 0x04001178 RID: 4472
		private string _filename;

		// Token: 0x04001179 RID: 4473
		private long _offset;

		// Token: 0x0400117A RID: 4474
		private long _size;

		// Token: 0x0400117B RID: 4475
		private bool _isImpersonating;

		// Token: 0x0400117C RID: 4476
		private bool _useTransmitFile;
	}
}
