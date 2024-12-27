using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004E9 RID: 1257
	internal class HttpResponseStreamAsyncResult : LazyAsyncResult
	{
		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002721 RID: 10017 RVA: 0x000A2034 File Offset: 0x000A1034
		internal ushort dataChunkCount
		{
			get
			{
				return (ushort)this.m_DataChunks.Length;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002722 RID: 10018 RVA: 0x000A203F File Offset: 0x000A103F
		internal unsafe UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK* pDataChunks
		{
			get
			{
				return (UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(this.m_DataChunks, 0);
			}
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x000A2052 File Offset: 0x000A1052
		internal HttpResponseStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback)
			: base(asyncObject, userState, callback)
		{
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000A2060 File Offset: 0x000A1060
		internal unsafe HttpResponseStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback, byte[] buffer, int offset, int size, bool chunked, bool sentHeaders)
			: base(asyncObject, userState, callback)
		{
			this.m_SentHeaders = sentHeaders;
			Overlapped overlapped = new Overlapped();
			overlapped.AsyncResult = this;
			this.m_DataChunks = new UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK[chunked ? 3 : 1];
			object[] array = new object[1 + this.m_DataChunks.Length];
			array[this.m_DataChunks.Length] = this.m_DataChunks;
			int num = 0;
			byte[] array2 = null;
			if (chunked)
			{
				array2 = ConnectStream.GetChunkHeader(size, out num);
				this.m_DataChunks[0] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[0].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[0].BufferLength = (uint)(array2.Length - num);
				array[0] = array2;
				this.m_DataChunks[1] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[1].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[1].BufferLength = (uint)size;
				array[1] = buffer;
				this.m_DataChunks[2] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[2].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[2].BufferLength = (uint)NclConstants.CRLF.Length;
				array[2] = NclConstants.CRLF;
			}
			else
			{
				this.m_DataChunks[0] = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
				this.m_DataChunks[0].DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
				this.m_DataChunks[0].BufferLength = (uint)size;
				array[0] = buffer;
			}
			this.m_pOverlapped = overlapped.Pack(HttpResponseStreamAsyncResult.s_IOCallback, array);
			if (chunked)
			{
				this.m_DataChunks[0].pBuffer = (byte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(array2, num);
				this.m_DataChunks[1].pBuffer = (byte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
				this.m_DataChunks[2].pBuffer = (byte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(NclConstants.CRLF, 0);
				return;
			}
			this.m_DataChunks[0].pBuffer = (byte*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x000A2264 File Offset: 0x000A1264
		private unsafe static void Callback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
			HttpResponseStreamAsyncResult httpResponseStreamAsyncResult = overlapped.AsyncResult as HttpResponseStreamAsyncResult;
			object obj = null;
			try
			{
				if (errorCode != 0U && errorCode != 38U)
				{
					httpResponseStreamAsyncResult.ErrorCode = (int)errorCode;
					obj = new HttpListenerException((int)errorCode);
				}
				else
				{
					obj = ((httpResponseStreamAsyncResult.m_DataChunks.Length == 1) ? httpResponseStreamAsyncResult.m_DataChunks[0].BufferLength : 0U);
					if (Logging.On)
					{
						for (int i = 0; i < httpResponseStreamAsyncResult.m_DataChunks.Length; i++)
						{
							Logging.Dump(Logging.HttpListener, httpResponseStreamAsyncResult, "Callback", (IntPtr)((void*)httpResponseStreamAsyncResult.m_DataChunks[0].pBuffer), (int)httpResponseStreamAsyncResult.m_DataChunks[0].BufferLength);
						}
					}
				}
			}
			catch (Exception ex)
			{
				obj = ex;
			}
			catch
			{
				obj = new Exception(SR.GetString("net_nonClsCompliantException"));
			}
			httpResponseStreamAsyncResult.InvokeCallback(obj);
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x000A2354 File Offset: 0x000A1354
		protected override void Cleanup()
		{
			base.Cleanup();
			if (this.m_pOverlapped != null)
			{
				Overlapped.Free(this.m_pOverlapped);
			}
		}

		// Token: 0x040026A0 RID: 9888
		internal unsafe NativeOverlapped* m_pOverlapped;

		// Token: 0x040026A1 RID: 9889
		private UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK[] m_DataChunks;

		// Token: 0x040026A2 RID: 9890
		internal bool m_SentHeaders;

		// Token: 0x040026A3 RID: 9891
		private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(HttpResponseStreamAsyncResult.Callback);
	}
}
