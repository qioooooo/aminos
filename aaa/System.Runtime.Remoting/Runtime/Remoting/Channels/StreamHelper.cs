using System;
using System.IO;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200001C RID: 28
	internal static class StreamHelper
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x00005180 File Offset: 0x00004180
		internal static void CopyStream(Stream source, Stream target)
		{
			if (source == null)
			{
				return;
			}
			ChunkedMemoryStream chunkedMemoryStream = source as ChunkedMemoryStream;
			if (chunkedMemoryStream != null)
			{
				chunkedMemoryStream.WriteTo(target);
				return;
			}
			MemoryStream memoryStream = source as MemoryStream;
			if (memoryStream != null)
			{
				memoryStream.WriteTo(target);
				return;
			}
			byte[] buffer = CoreChannel.BufferPool.GetBuffer();
			int num = buffer.Length;
			for (int i = source.Read(buffer, 0, num); i > 0; i = source.Read(buffer, 0, num))
			{
				target.Write(buffer, 0, i);
			}
			CoreChannel.BufferPool.ReturnBuffer(buffer);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000051F8 File Offset: 0x000041F8
		internal static void BufferCopy(byte[] source, int srcOffset, byte[] dest, int destOffset, int count)
		{
			if (count > 8)
			{
				Buffer.BlockCopy(source, srcOffset, dest, destOffset, count);
				return;
			}
			for (int i = 0; i < count; i++)
			{
				dest[destOffset + i] = source[srcOffset + i];
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005230 File Offset: 0x00004230
		internal static IAsyncResult BeginAsyncCopyStream(Stream source, Stream target, bool asyncRead, bool asyncWrite, bool closeSource, bool closeTarget, AsyncCallback callback, object state)
		{
			AsyncCopyStreamResult asyncCopyStreamResult = new AsyncCopyStreamResult(callback, state);
			byte[] buffer = CoreChannel.BufferPool.GetBuffer();
			asyncCopyStreamResult.Source = source;
			asyncCopyStreamResult.Target = target;
			asyncCopyStreamResult.Buffer = buffer;
			asyncCopyStreamResult.AsyncRead = asyncRead;
			asyncCopyStreamResult.AsyncWrite = asyncWrite;
			asyncCopyStreamResult.CloseSource = closeSource;
			asyncCopyStreamResult.CloseTarget = closeTarget;
			try
			{
				StreamHelper.AsyncCopyReadHelper(asyncCopyStreamResult);
			}
			catch (Exception ex)
			{
				asyncCopyStreamResult.SetComplete(null, ex);
			}
			catch
			{
				asyncCopyStreamResult.SetComplete(null, new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException")));
			}
			return asyncCopyStreamResult;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000052D0 File Offset: 0x000042D0
		internal static void EndAsyncCopyStream(IAsyncResult iar)
		{
			AsyncCopyStreamResult asyncCopyStreamResult = (AsyncCopyStreamResult)iar;
			if (!iar.IsCompleted)
			{
				iar.AsyncWaitHandle.WaitOne();
			}
			if (asyncCopyStreamResult.Exception != null)
			{
				throw asyncCopyStreamResult.Exception;
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005308 File Offset: 0x00004308
		private static void AsyncCopyReadHelper(AsyncCopyStreamResult streamState)
		{
			if (streamState.AsyncRead)
			{
				byte[] buffer = streamState.Buffer;
				streamState.Source.BeginRead(buffer, 0, buffer.Length, StreamHelper._asyncCopyStreamReadCallback, streamState);
				return;
			}
			byte[] buffer2 = streamState.Buffer;
			int num = streamState.Source.Read(buffer2, 0, buffer2.Length);
			if (num == 0)
			{
				streamState.SetComplete(null, null);
				return;
			}
			if (num < 0)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Stream_UnknownReadError"));
			}
			StreamHelper.AsyncCopyWriteHelper(streamState, num);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000537C File Offset: 0x0000437C
		private static void AsyncCopyWriteHelper(AsyncCopyStreamResult streamState, int bytesRead)
		{
			if (streamState.AsyncWrite)
			{
				byte[] buffer = streamState.Buffer;
				streamState.Target.BeginWrite(buffer, 0, bytesRead, StreamHelper._asyncCopyStreamWriteCallback, streamState);
				return;
			}
			byte[] buffer2 = streamState.Buffer;
			streamState.Target.Write(buffer2, 0, bytesRead);
			StreamHelper.AsyncCopyReadHelper(streamState);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000053CC File Offset: 0x000043CC
		private static void AsyncCopyStreamReadCallback(IAsyncResult iar)
		{
			AsyncCopyStreamResult asyncCopyStreamResult = (AsyncCopyStreamResult)iar.AsyncState;
			try
			{
				Stream source = asyncCopyStreamResult.Source;
				int num = source.EndRead(iar);
				if (num == 0)
				{
					asyncCopyStreamResult.SetComplete(null, null);
				}
				else
				{
					if (num < 0)
					{
						throw new RemotingException(CoreChannel.GetResourceString("Remoting_Stream_UnknownReadError"));
					}
					StreamHelper.AsyncCopyWriteHelper(asyncCopyStreamResult, num);
				}
			}
			catch (Exception ex)
			{
				asyncCopyStreamResult.SetComplete(null, ex);
			}
			catch
			{
				asyncCopyStreamResult.SetComplete(null, new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException")));
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005460 File Offset: 0x00004460
		private static void AsyncCopyStreamWriteCallback(IAsyncResult iar)
		{
			AsyncCopyStreamResult asyncCopyStreamResult = (AsyncCopyStreamResult)iar.AsyncState;
			try
			{
				asyncCopyStreamResult.Target.EndWrite(iar);
				StreamHelper.AsyncCopyReadHelper(asyncCopyStreamResult);
			}
			catch (Exception ex)
			{
				asyncCopyStreamResult.SetComplete(null, ex);
			}
			catch
			{
				asyncCopyStreamResult.SetComplete(null, new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException")));
			}
		}

		// Token: 0x040000AF RID: 175
		private static AsyncCallback _asyncCopyStreamReadCallback = new AsyncCallback(StreamHelper.AsyncCopyStreamReadCallback);

		// Token: 0x040000B0 RID: 176
		private static AsyncCallback _asyncCopyStreamWriteCallback = new AsyncCallback(StreamHelper.AsyncCopyStreamWriteCallback);
	}
}
