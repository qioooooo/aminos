using System;
using System.IO;
using System.IO.Compression;

namespace System.Net
{
	// Token: 0x020003E6 RID: 998
	internal class DeflateWrapperStream : DeflateStream, ICloseEx
	{
		// Token: 0x06002061 RID: 8289 RVA: 0x0007F9A0 File Offset: 0x0007E9A0
		public DeflateWrapperStream(Stream stream, CompressionMode mode)
			: base(stream, mode, false)
		{
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x0007F9AC File Offset: 0x0007E9AC
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			ICloseEx closeEx = base.BaseStream as ICloseEx;
			if (closeEx != null)
			{
				closeEx.CloseEx(closeState);
				return;
			}
			this.Close();
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x0007F9D8 File Offset: 0x0007E9D8
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			IAsyncResult asyncResult;
			try
			{
				asyncResult = base.BeginRead(buffer, offset, size, callback, state);
			}
			catch (Exception ex)
			{
				try
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					if (ex is InvalidDataException || ex is InvalidOperationException || ex is IndexOutOfRangeException)
					{
						this.Close();
					}
				}
				catch
				{
				}
				throw ex;
			}
			return asyncResult;
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x0007FA7C File Offset: 0x0007EA7C
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			int num;
			try
			{
				num = base.EndRead(asyncResult);
			}
			catch (Exception ex)
			{
				try
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					if (ex is InvalidDataException || ex is InvalidOperationException || ex is IndexOutOfRangeException)
					{
						this.Close();
					}
				}
				catch
				{
				}
				throw ex;
			}
			return num;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x0007FAF0 File Offset: 0x0007EAF0
		public override int Read(byte[] buffer, int offset, int size)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			int num;
			try
			{
				num = base.Read(buffer, offset, size);
			}
			catch (Exception ex)
			{
				try
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					if (ex is InvalidDataException || ex is InvalidOperationException || ex is IndexOutOfRangeException)
					{
						this.Close();
					}
				}
				catch
				{
				}
				throw ex;
			}
			return num;
		}
	}
}
