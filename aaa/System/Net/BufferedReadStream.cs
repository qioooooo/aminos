using System;
using System.IO;

namespace System.Net
{
	// Token: 0x02000683 RID: 1667
	internal class BufferedReadStream : DelegatedStream
	{
		// Token: 0x0600339B RID: 13211 RVA: 0x000D9E72 File Offset: 0x000D8E72
		internal BufferedReadStream(Stream stream)
			: this(stream, false)
		{
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x000D9E7C File Offset: 0x000D8E7C
		internal BufferedReadStream(Stream stream, bool readMore)
			: base(stream)
		{
			this.readMore = readMore;
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x0600339D RID: 13213 RVA: 0x000D9E8C File Offset: 0x000D8E8C
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x0600339E RID: 13214 RVA: 0x000D9E8F File Offset: 0x000D8E8F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x000D9E94 File Offset: 0x000D8E94
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			BufferedReadStream.ReadAsyncResult readAsyncResult = new BufferedReadStream.ReadAsyncResult(this, callback, state);
			readAsyncResult.Read(buffer, offset, count);
			return readAsyncResult;
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x000D9EB8 File Offset: 0x000D8EB8
		public override int EndRead(IAsyncResult asyncResult)
		{
			return BufferedReadStream.ReadAsyncResult.End(asyncResult);
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x000D9ED0 File Offset: 0x000D8ED0
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			if (this.storedOffset < this.storedLength)
			{
				num = Math.Min(count, this.storedLength - this.storedOffset);
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, buffer, offset, num);
				this.storedOffset += num;
				if (num == count || !this.readMore)
				{
					return num;
				}
				offset += num;
				count -= num;
			}
			return num + base.Read(buffer, offset, count);
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x000D9F48 File Offset: 0x000D8F48
		public override int ReadByte()
		{
			if (this.storedOffset < this.storedLength)
			{
				return (int)this.storedBuffer[this.storedOffset++];
			}
			return base.ReadByte();
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x000D9F84 File Offset: 0x000D8F84
		internal void Push(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			if (this.storedOffset == this.storedLength)
			{
				if (this.storedBuffer == null || this.storedBuffer.Length < count)
				{
					this.storedBuffer = new byte[count];
				}
				this.storedOffset = 0;
				this.storedLength = count;
			}
			else if (count <= this.storedOffset)
			{
				this.storedOffset -= count;
			}
			else if (count <= this.storedBuffer.Length - this.storedLength + this.storedOffset)
			{
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, this.storedBuffer, count, this.storedLength - this.storedOffset);
				this.storedLength += count - this.storedOffset;
				this.storedOffset = 0;
			}
			else
			{
				byte[] array = new byte[count + this.storedLength - this.storedOffset];
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, array, count, this.storedLength - this.storedOffset);
				this.storedLength += count - this.storedOffset;
				this.storedOffset = 0;
				this.storedBuffer = array;
			}
			Buffer.BlockCopy(buffer, offset, this.storedBuffer, this.storedOffset, count);
		}

		// Token: 0x04002FA1 RID: 12193
		private byte[] storedBuffer;

		// Token: 0x04002FA2 RID: 12194
		private int storedLength;

		// Token: 0x04002FA3 RID: 12195
		private int storedOffset;

		// Token: 0x04002FA4 RID: 12196
		private bool readMore;

		// Token: 0x02000684 RID: 1668
		private class ReadAsyncResult : LazyAsyncResult
		{
			// Token: 0x060033A4 RID: 13220 RVA: 0x000DA0B9 File Offset: 0x000D90B9
			internal ReadAsyncResult(BufferedReadStream parent, AsyncCallback callback, object state)
				: base(null, state, callback)
			{
				this.parent = parent;
			}

			// Token: 0x060033A5 RID: 13221 RVA: 0x000DA0CC File Offset: 0x000D90CC
			internal void Read(byte[] buffer, int offset, int count)
			{
				if (this.parent.storedOffset < this.parent.storedLength)
				{
					this.read = Math.Min(count, this.parent.storedLength - this.parent.storedOffset);
					Buffer.BlockCopy(this.parent.storedBuffer, this.parent.storedOffset, buffer, offset, this.read);
					this.parent.storedOffset += this.read;
					if (this.read == count || !this.parent.readMore)
					{
						base.InvokeCallback();
						return;
					}
					count -= this.read;
					offset += this.read;
				}
				IAsyncResult asyncResult = this.parent.BaseStream.BeginRead(buffer, offset, count, BufferedReadStream.ReadAsyncResult.onRead, this);
				if (asyncResult.CompletedSynchronously)
				{
					this.read += this.parent.BaseStream.EndRead(asyncResult);
					base.InvokeCallback();
				}
			}

			// Token: 0x060033A6 RID: 13222 RVA: 0x000DA1CC File Offset: 0x000D91CC
			internal static int End(IAsyncResult result)
			{
				BufferedReadStream.ReadAsyncResult readAsyncResult = (BufferedReadStream.ReadAsyncResult)result;
				readAsyncResult.InternalWaitForCompletion();
				return readAsyncResult.read;
			}

			// Token: 0x060033A7 RID: 13223 RVA: 0x000DA1F0 File Offset: 0x000D91F0
			private static void OnRead(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					BufferedReadStream.ReadAsyncResult readAsyncResult = (BufferedReadStream.ReadAsyncResult)result.AsyncState;
					try
					{
						readAsyncResult.read += readAsyncResult.parent.BaseStream.EndRead(result);
						readAsyncResult.InvokeCallback();
					}
					catch (Exception ex)
					{
						if (readAsyncResult.IsCompleted)
						{
							throw;
						}
						readAsyncResult.InvokeCallback(ex);
					}
					catch
					{
						if (readAsyncResult.IsCompleted)
						{
							throw;
						}
						readAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
					}
				}
			}

			// Token: 0x04002FA5 RID: 12197
			private BufferedReadStream parent;

			// Token: 0x04002FA6 RID: 12198
			private int read;

			// Token: 0x04002FA7 RID: 12199
			private static AsyncCallback onRead = new AsyncCallback(BufferedReadStream.ReadAsyncResult.OnRead);
		}
	}
}
