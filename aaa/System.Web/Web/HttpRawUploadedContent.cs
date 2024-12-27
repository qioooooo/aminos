using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x02000075 RID: 117
	internal class HttpRawUploadedContent : IDisposable
	{
		// Token: 0x06000509 RID: 1289 RVA: 0x00014B58 File Offset: 0x00013B58
		internal HttpRawUploadedContent(int fileThreshold, int expectedLength)
		{
			this._fileThreshold = fileThreshold;
			this._expectedLength = expectedLength;
			if (this._expectedLength >= 0 && this._expectedLength < this._fileThreshold)
			{
				this._data = new byte[this._expectedLength];
				return;
			}
			this._data = new byte[this._fileThreshold];
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00014BB3 File Offset: 0x00013BB3
		public void Dispose()
		{
			if (this._file != null)
			{
				this._file.Dispose();
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00014BC8 File Offset: 0x00013BC8
		internal void AddBytes(byte[] data, int offset, int length)
		{
			if (this._completed)
			{
				throw new InvalidOperationException();
			}
			if (length <= 0)
			{
				return;
			}
			if (this._file == null)
			{
				if (this._length + length <= this._data.Length)
				{
					Array.Copy(data, offset, this._data, this._length, length);
					this._length += length;
					return;
				}
				if (this._length + length <= this._fileThreshold)
				{
					byte[] array = new byte[this._fileThreshold];
					if (this._length > 0)
					{
						Array.Copy(this._data, 0, array, 0, this._length);
					}
					Array.Copy(data, offset, array, this._length, length);
					this._data = array;
					this._length += length;
					return;
				}
				this._file = new HttpRawUploadedContent.TempFile();
				this._file.AddBytes(this._data, 0, this._length);
			}
			this._file.AddBytes(data, offset, length);
			this._length += length;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00014CC5 File Offset: 0x00013CC5
		internal void DoneAddingBytes()
		{
			if (this._data == null)
			{
				this._data = new byte[0];
			}
			if (this._file != null)
			{
				this._file.DoneAddingBytes();
			}
			this._completed = true;
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00014CF5 File Offset: 0x00013CF5
		internal int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x170001E1 RID: 481
		internal byte this[int index]
		{
			get
			{
				if (!this._completed)
				{
					throw new InvalidOperationException();
				}
				if (this._file == null)
				{
					return this._data[index];
				}
				if (index >= this._chunkOffset && index < this._chunkOffset + this._chunkLength)
				{
					return this._data[index - this._chunkOffset];
				}
				if (index < 0 || index >= this._length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				this._chunkLength = this._file.GetBytes(index, this._data.Length, this._data, 0);
				this._chunkOffset = index;
				return this._data[0];
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00014DA0 File Offset: 0x00013DA0
		internal void CopyBytes(int offset, byte[] buffer, int bufferOffset, int length)
		{
			if (!this._completed)
			{
				throw new InvalidOperationException();
			}
			if (this._file == null)
			{
				Array.Copy(this._data, offset, buffer, bufferOffset, length);
				return;
			}
			if (offset >= this._chunkOffset && offset + length < this._chunkOffset + this._chunkLength)
			{
				Array.Copy(this._data, offset - this._chunkOffset, buffer, bufferOffset, length);
				return;
			}
			if (length <= this._data.Length)
			{
				this._chunkLength = this._file.GetBytes(offset, this._data.Length, this._data, 0);
				this._chunkOffset = offset;
				Array.Copy(this._data, offset - this._chunkOffset, buffer, bufferOffset, length);
				return;
			}
			this._file.GetBytes(offset, length, buffer, bufferOffset);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00014E68 File Offset: 0x00013E68
		internal void WriteBytes(int offset, int length, Stream stream)
		{
			if (!this._completed)
			{
				throw new InvalidOperationException();
			}
			if (this._file != null)
			{
				int num = offset;
				int i = length;
				byte[] array = new byte[(i > this._fileThreshold) ? this._fileThreshold : i];
				while (i > 0)
				{
					int num2 = ((i > this._fileThreshold) ? this._fileThreshold : i);
					int bytes = this._file.GetBytes(num, num2, array, 0);
					if (bytes == 0)
					{
						return;
					}
					stream.Write(array, 0, bytes);
					num += bytes;
					i -= bytes;
				}
				return;
			}
			stream.Write(this._data, offset, length);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00014EF9 File Offset: 0x00013EF9
		internal byte[] GetAsByteArray()
		{
			if (this._file == null && this._length == this._data.Length)
			{
				return this._data;
			}
			return this.GetAsByteArray(0, this._length);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00014F28 File Offset: 0x00013F28
		internal byte[] GetAsByteArray(int offset, int length)
		{
			if (!this._completed)
			{
				throw new InvalidOperationException();
			}
			if (length == 0)
			{
				return new byte[0];
			}
			byte[] array = new byte[length];
			this.CopyBytes(offset, array, 0, length);
			return array;
		}

		// Token: 0x0400104A RID: 4170
		private int _fileThreshold;

		// Token: 0x0400104B RID: 4171
		private int _expectedLength;

		// Token: 0x0400104C RID: 4172
		private bool _completed;

		// Token: 0x0400104D RID: 4173
		private int _length;

		// Token: 0x0400104E RID: 4174
		private byte[] _data;

		// Token: 0x0400104F RID: 4175
		private HttpRawUploadedContent.TempFile _file;

		// Token: 0x04001050 RID: 4176
		private int _chunkOffset;

		// Token: 0x04001051 RID: 4177
		private int _chunkLength;

		// Token: 0x02000076 RID: 118
		private class TempFile : IDisposable
		{
			// Token: 0x06000513 RID: 1299 RVA: 0x00014F60 File Offset: 0x00013F60
			internal TempFile()
			{
				using (new ApplicationImpersonationContext())
				{
					string text = Path.Combine(HttpRuntime.CodegenDirInternal, "uploads");
					new FileIOPermission(FileIOPermissionAccess.AllAccess, text).Assert();
					if (!Directory.Exists(text))
					{
						try
						{
							Directory.CreateDirectory(text);
						}
						catch
						{
						}
					}
					this._tempFiles = new TempFileCollection(text, false);
					this._filename = this._tempFiles.AddExtension("post", false);
					this._filestream = new FileStream(this._filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose);
				}
			}

			// Token: 0x06000514 RID: 1300 RVA: 0x00015014 File Offset: 0x00014014
			public void Dispose()
			{
				using (new ApplicationImpersonationContext())
				{
					try
					{
						if (this._filestream != null)
						{
							this._filestream.Close();
						}
						this._tempFiles.Delete();
						((IDisposable)this._tempFiles).Dispose();
					}
					catch
					{
					}
				}
			}

			// Token: 0x06000515 RID: 1301 RVA: 0x00015080 File Offset: 0x00014080
			internal void AddBytes(byte[] data, int offset, int length)
			{
				if (this._filestream == null)
				{
					throw new InvalidOperationException();
				}
				this._filestream.Write(data, offset, length);
			}

			// Token: 0x06000516 RID: 1302 RVA: 0x0001509E File Offset: 0x0001409E
			internal void DoneAddingBytes()
			{
				if (this._filestream == null)
				{
					throw new InvalidOperationException();
				}
				this._filestream.Flush();
				this._filestream.Seek(0L, SeekOrigin.Begin);
			}

			// Token: 0x06000517 RID: 1303 RVA: 0x000150C8 File Offset: 0x000140C8
			internal int GetBytes(int offset, int length, byte[] buffer, int bufferOffset)
			{
				if (this._filestream == null)
				{
					throw new InvalidOperationException();
				}
				this._filestream.Seek((long)offset, SeekOrigin.Begin);
				return this._filestream.Read(buffer, bufferOffset, length);
			}

			// Token: 0x04001052 RID: 4178
			private TempFileCollection _tempFiles;

			// Token: 0x04001053 RID: 4179
			private string _filename;

			// Token: 0x04001054 RID: 4180
			private Stream _filestream;
		}
	}
}
