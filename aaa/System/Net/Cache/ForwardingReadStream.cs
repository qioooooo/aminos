using System;
using System.IO;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x0200057C RID: 1404
	internal class ForwardingReadStream : Stream, ICloseEx
	{
		// Token: 0x06002ACE RID: 10958 RVA: 0x000B6124 File Offset: 0x000B5124
		internal ForwardingReadStream(Stream originalStream, Stream shadowStream, long bytesToSkip, bool throwOnWriteError)
		{
			if (!shadowStream.CanWrite)
			{
				throw new ArgumentException(SR.GetString("net_cache_shadowstream_not_writable"), "shadowStream");
			}
			this.m_OriginalStream = originalStream;
			this.m_ShadowStream = shadowStream;
			this.m_BytesToSkip = bytesToSkip;
			this.m_ThrowOnWriteError = throwOnWriteError;
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002ACF RID: 10959 RVA: 0x000B6171 File Offset: 0x000B5171
		public override bool CanRead
		{
			get
			{
				return this.m_OriginalStream.CanRead;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06002AD0 RID: 10960 RVA: 0x000B617E File Offset: 0x000B517E
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x000B6181 File Offset: 0x000B5181
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06002AD2 RID: 10962 RVA: 0x000B6184 File Offset: 0x000B5184
		public override long Length
		{
			get
			{
				return this.m_OriginalStream.Length - this.m_BytesToSkip;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06002AD3 RID: 10963 RVA: 0x000B6198 File Offset: 0x000B5198
		// (set) Token: 0x06002AD4 RID: 10964 RVA: 0x000B61AC File Offset: 0x000B51AC
		public override long Position
		{
			get
			{
				return this.m_OriginalStream.Position - this.m_BytesToSkip;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000B61BD File Offset: 0x000B51BD
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x000B61CE File Offset: 0x000B51CE
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x000B61DF File Offset: 0x000B51DF
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x000B61F0 File Offset: 0x000B51F0
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x000B6201 File Offset: 0x000B5201
		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x000B6212 File Offset: 0x000B5212
		public override void Flush()
		{
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x000B6214 File Offset: 0x000B5214
		public override int Read(byte[] buffer, int offset, int count)
		{
			bool flag = false;
			int num = -1;
			if (Interlocked.Increment(ref this.m_ReadNesting) != 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[] { "Read", "read" }));
			}
			int num3;
			try
			{
				if (this.m_BytesToSkip != 0L)
				{
					byte[] array = new byte[4096];
					while (this.m_BytesToSkip != 0L)
					{
						int num2 = this.m_OriginalStream.Read(array, 0, (this.m_BytesToSkip < (long)array.Length) ? ((int)this.m_BytesToSkip) : array.Length);
						if (num2 == 0)
						{
							this.m_SeenReadEOF = true;
						}
						this.m_BytesToSkip -= (long)num2;
						if (!this.m_ShadowStreamIsDead)
						{
							this.m_ShadowStream.Write(array, 0, num2);
						}
					}
				}
				num = this.m_OriginalStream.Read(buffer, offset, count);
				if (num == 0)
				{
					this.m_SeenReadEOF = true;
				}
				if (this.m_ShadowStreamIsDead)
				{
					num3 = num;
				}
				else
				{
					flag = true;
					this.m_ShadowStream.Write(buffer, offset, num);
					num3 = num;
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!this.m_ShadowStreamIsDead)
				{
					this.m_ShadowStreamIsDead = true;
					try
					{
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception)
					{
						if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
						{
							throw;
						}
					}
					catch
					{
					}
				}
				if (!flag || this.m_ThrowOnWriteError)
				{
					throw;
				}
				num3 = num;
			}
			catch
			{
				if (!this.m_ShadowStreamIsDead)
				{
					this.m_ShadowStreamIsDead = true;
					try
					{
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception ex2)
					{
						if (NclUtilities.IsFatal(ex2))
						{
							throw;
						}
					}
					catch
					{
					}
				}
				if (!flag || this.m_ThrowOnWriteError)
				{
					throw;
				}
				num3 = num;
			}
			finally
			{
				Interlocked.Decrement(ref this.m_ReadNesting);
			}
			return num3;
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x000B64C4 File Offset: 0x000B54C4
		private void ReadCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			object asyncState = transportResult.AsyncState;
			this.ReadComplete(transportResult);
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x000B64E0 File Offset: 0x000B54E0
		private void ReadComplete(IAsyncResult transportResult)
		{
			for (;;)
			{
				ForwardingReadStream.InnerAsyncResult innerAsyncResult = transportResult.AsyncState as ForwardingReadStream.InnerAsyncResult;
				try
				{
					if (!innerAsyncResult.IsWriteCompletion)
					{
						innerAsyncResult.Count = this.m_OriginalStream.EndRead(transportResult);
						if (innerAsyncResult.Count == 0)
						{
							this.m_SeenReadEOF = true;
						}
						if (!this.m_ShadowStreamIsDead)
						{
							innerAsyncResult.IsWriteCompletion = true;
							transportResult = this.m_ShadowStream.BeginWrite(innerAsyncResult.Buffer, innerAsyncResult.Offset, innerAsyncResult.Count, this.m_ReadCallback, innerAsyncResult);
							if (transportResult.CompletedSynchronously)
							{
								continue;
							}
							break;
						}
					}
					else
					{
						this.m_ShadowStream.EndWrite(transportResult);
						innerAsyncResult.IsWriteCompletion = false;
					}
				}
				catch (Exception ex)
				{
					if (innerAsyncResult.InternalPeekCompleted)
					{
						throw;
					}
					try
					{
						this.m_ShadowStreamIsDead = true;
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception)
					{
					}
					catch
					{
					}
					if (!innerAsyncResult.IsWriteCompletion || this.m_ThrowOnWriteError)
					{
						if (transportResult.CompletedSynchronously)
						{
							throw;
						}
						innerAsyncResult.InvokeCallback(ex);
						break;
					}
				}
				catch
				{
					if (innerAsyncResult.InternalPeekCompleted)
					{
						throw;
					}
					try
					{
						this.m_ShadowStreamIsDead = true;
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception)
					{
					}
					catch
					{
					}
					if (!innerAsyncResult.IsWriteCompletion || this.m_ThrowOnWriteError)
					{
						if (transportResult.CompletedSynchronously)
						{
							throw;
						}
						innerAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
						break;
					}
				}
				try
				{
					if (this.m_BytesToSkip != 0L)
					{
						this.m_BytesToSkip -= (long)innerAsyncResult.Count;
						innerAsyncResult.Count = ((this.m_BytesToSkip < (long)innerAsyncResult.Buffer.Length) ? ((int)this.m_BytesToSkip) : innerAsyncResult.Buffer.Length);
						if (this.m_BytesToSkip == 0L)
						{
							transportResult = innerAsyncResult;
							innerAsyncResult = innerAsyncResult.AsyncState as ForwardingReadStream.InnerAsyncResult;
						}
						transportResult = this.m_OriginalStream.BeginRead(innerAsyncResult.Buffer, innerAsyncResult.Offset, innerAsyncResult.Count, this.m_ReadCallback, innerAsyncResult);
						if (transportResult.CompletedSynchronously)
						{
							continue;
						}
					}
					else
					{
						innerAsyncResult.InvokeCallback(innerAsyncResult.Count);
					}
				}
				catch (Exception ex2)
				{
					if (innerAsyncResult.InternalPeekCompleted)
					{
						throw;
					}
					try
					{
						this.m_ShadowStreamIsDead = true;
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception)
					{
					}
					catch
					{
					}
					if (transportResult.CompletedSynchronously)
					{
						throw;
					}
					innerAsyncResult.InvokeCallback(ex2);
				}
				catch
				{
					if (innerAsyncResult.InternalPeekCompleted)
					{
						throw;
					}
					try
					{
						this.m_ShadowStreamIsDead = true;
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception)
					{
					}
					catch
					{
					}
					if (transportResult.CompletedSynchronously)
					{
						throw;
					}
					innerAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
				break;
			}
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000B6868 File Offset: 0x000B5868
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (Interlocked.Increment(ref this.m_ReadNesting) != 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[] { "BeginRead", "read" }));
			}
			IAsyncResult asyncResult;
			try
			{
				if (this.m_ReadCallback == null)
				{
					this.m_ReadCallback = new AsyncCallback(this.ReadCallback);
				}
				if (this.m_ShadowStreamIsDead && this.m_BytesToSkip == 0L)
				{
					asyncResult = this.m_OriginalStream.BeginRead(buffer, offset, count, callback, state);
				}
				else
				{
					ForwardingReadStream.InnerAsyncResult innerAsyncResult = new ForwardingReadStream.InnerAsyncResult(state, callback, buffer, offset, count);
					if (this.m_BytesToSkip != 0L)
					{
						ForwardingReadStream.InnerAsyncResult innerAsyncResult2 = innerAsyncResult;
						innerAsyncResult = new ForwardingReadStream.InnerAsyncResult(innerAsyncResult2, null, new byte[4096], 0, (this.m_BytesToSkip < (long)buffer.Length) ? ((int)this.m_BytesToSkip) : buffer.Length);
					}
					IAsyncResult asyncResult2 = this.m_OriginalStream.BeginRead(innerAsyncResult.Buffer, innerAsyncResult.Offset, innerAsyncResult.Count, this.m_ReadCallback, innerAsyncResult);
					if (asyncResult2.CompletedSynchronously)
					{
						this.ReadComplete(asyncResult2);
					}
					asyncResult = innerAsyncResult;
				}
			}
			catch
			{
				Interlocked.Decrement(ref this.m_ReadNesting);
				throw;
			}
			return asyncResult;
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x000B6990 File Offset: 0x000B5990
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (Interlocked.Decrement(ref this.m_ReadNesting) != 0)
			{
				Interlocked.Increment(ref this.m_ReadNesting);
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndRead" }));
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			ForwardingReadStream.InnerAsyncResult innerAsyncResult = asyncResult as ForwardingReadStream.InnerAsyncResult;
			if (innerAsyncResult == null && this.m_OriginalStream.EndRead(asyncResult) == 0)
			{
				this.m_SeenReadEOF = true;
			}
			bool flag = false;
			try
			{
				innerAsyncResult.InternalWaitForCompletion();
				if (innerAsyncResult.Result is Exception)
				{
					throw (Exception)innerAsyncResult.Result;
				}
				flag = true;
			}
			finally
			{
				if (!flag && !this.m_ShadowStreamIsDead)
				{
					this.m_ShadowStreamIsDead = true;
					if (this.m_ShadowStream is ICloseEx)
					{
						((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
					}
					else
					{
						this.m_ShadowStream.Close();
					}
				}
			}
			return (int)innerAsyncResult.Result;
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000B6A84 File Offset: 0x000B5A84
		protected sealed override void Dispose(bool disposing)
		{
			this.Dispose(disposing, CloseExState.Normal);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000B6A94 File Offset: 0x000B5A94
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			if (Interlocked.Increment(ref this._Disposed) == 1)
			{
				if (closeState == CloseExState.Silent)
				{
					try
					{
						int num = 0;
						int num2;
						while (num < ConnectStream.s_DrainingBuffer.Length && (num2 = this.Read(ConnectStream.s_DrainingBuffer, 0, ConnectStream.s_DrainingBuffer.Length)) > 0)
						{
							num += num2;
						}
					}
					catch (Exception ex)
					{
						if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
						{
							throw;
						}
					}
					catch
					{
					}
				}
				this.Dispose(true, closeState);
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000B6B2C File Offset: 0x000B5B2C
		protected virtual void Dispose(bool disposing, CloseExState closeState)
		{
			try
			{
				ICloseEx closeEx = this.m_OriginalStream as ICloseEx;
				if (closeEx != null)
				{
					closeEx.CloseEx(closeState);
				}
				else
				{
					this.m_OriginalStream.Close();
				}
			}
			finally
			{
				if (!this.m_SeenReadEOF)
				{
					closeState |= CloseExState.Abort;
				}
				if (this.m_ShadowStream is ICloseEx)
				{
					((ICloseEx)this.m_ShadowStream).CloseEx(closeState);
				}
				else
				{
					this.m_ShadowStream.Close();
				}
			}
			if (!disposing)
			{
				this.m_OriginalStream = null;
				this.m_ShadowStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x000B6BC0 File Offset: 0x000B5BC0
		public override bool CanTimeout
		{
			get
			{
				return this.m_OriginalStream.CanTimeout && this.m_ShadowStream.CanTimeout;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06002AE4 RID: 10980 RVA: 0x000B6BDC File Offset: 0x000B5BDC
		// (set) Token: 0x06002AE5 RID: 10981 RVA: 0x000B6BEC File Offset: 0x000B5BEC
		public override int ReadTimeout
		{
			get
			{
				return this.m_OriginalStream.ReadTimeout;
			}
			set
			{
				Stream originalStream = this.m_OriginalStream;
				this.m_ShadowStream.ReadTimeout = value;
				originalStream.ReadTimeout = value;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x000B6C13 File Offset: 0x000B5C13
		// (set) Token: 0x06002AE7 RID: 10983 RVA: 0x000B6C20 File Offset: 0x000B5C20
		public override int WriteTimeout
		{
			get
			{
				return this.m_ShadowStream.WriteTimeout;
			}
			set
			{
				Stream originalStream = this.m_OriginalStream;
				this.m_ShadowStream.WriteTimeout = value;
				originalStream.WriteTimeout = value;
			}
		}

		// Token: 0x04002986 RID: 10630
		private Stream m_OriginalStream;

		// Token: 0x04002987 RID: 10631
		private Stream m_ShadowStream;

		// Token: 0x04002988 RID: 10632
		private int m_ReadNesting;

		// Token: 0x04002989 RID: 10633
		private bool m_ShadowStreamIsDead;

		// Token: 0x0400298A RID: 10634
		private AsyncCallback m_ReadCallback;

		// Token: 0x0400298B RID: 10635
		private long m_BytesToSkip;

		// Token: 0x0400298C RID: 10636
		private bool m_ThrowOnWriteError;

		// Token: 0x0400298D RID: 10637
		private bool m_SeenReadEOF;

		// Token: 0x0400298E RID: 10638
		private int _Disposed;

		// Token: 0x0200057D RID: 1405
		private class InnerAsyncResult : LazyAsyncResult
		{
			// Token: 0x06002AE8 RID: 10984 RVA: 0x000B6C47 File Offset: 0x000B5C47
			public InnerAsyncResult(object userState, AsyncCallback userCallback, byte[] buffer, int offset, int count)
				: base(null, userState, userCallback)
			{
				this.Buffer = buffer;
				this.Offset = offset;
				this.Count = count;
			}

			// Token: 0x0400298F RID: 10639
			public byte[] Buffer;

			// Token: 0x04002990 RID: 10640
			public int Offset;

			// Token: 0x04002991 RID: 10641
			public int Count;

			// Token: 0x04002992 RID: 10642
			public bool IsWriteCompletion;
		}
	}
}
