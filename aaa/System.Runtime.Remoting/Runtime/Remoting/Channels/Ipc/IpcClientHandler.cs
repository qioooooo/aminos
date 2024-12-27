using System;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x02000058 RID: 88
	internal class IpcClientHandler : IpcServerHandler
	{
		// Token: 0x060002C5 RID: 709 RVA: 0x0000DB36 File Offset: 0x0000CB36
		internal IpcClientHandler(IpcPort port, Stream stream, IpcClientTransportSink sink)
			: base(port, null, stream)
		{
			this._sink = sink;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000DB48 File Offset: 0x0000CB48
		internal Stream GetResponseStream()
		{
			this._responseStream = new TcpFixedLengthReadingStream(this, this._contentLength);
			return this._responseStream;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000DB64 File Offset: 0x0000CB64
		public new BaseTransportHeaders ReadHeaders()
		{
			BaseTransportHeaders baseTransportHeaders = new BaseTransportHeaders();
			ushort num;
			base.ReadVersionAndOperation(out num);
			if (num != 2)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_ExpectingReplyOp"), new object[] { num.ToString(CultureInfo.CurrentCulture) }));
			}
			base.ReadContentLength(out this._bChunked, out this._contentLength);
			base.ReadToEndOfHeaders(baseTransportHeaders);
			return baseTransportHeaders;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000DBCE File Offset: 0x0000CBCE
		public override void OnInputStreamClosed()
		{
			if (this._responseStream != null)
			{
				this._responseStream.ReadToEnd();
				this._responseStream = null;
			}
			this.ReturnToCache();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000DBF0 File Offset: 0x0000CBF0
		internal void ReturnToCache()
		{
			this._sink.Cache.ReleaseConnection(this._port);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000DC08 File Offset: 0x0000CC08
		internal void SendRequest(IMessage msg, ITransportHeaders headers, Stream contentStream)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			int num = (int)contentStream.Length;
			string uri = methodCallMessage.Uri;
			this._bOneWayRequest = RemotingServices.IsOneWay(methodCallMessage.MethodBase);
			ChunkedMemoryStream chunkedMemoryStream = new ChunkedMemoryStream(CoreChannel.BufferPool);
			base.WritePreambleAndVersion(chunkedMemoryStream);
			if (!this._bOneWayRequest)
			{
				base.WriteUInt16(0, chunkedMemoryStream);
			}
			else
			{
				base.WriteUInt16(1, chunkedMemoryStream);
			}
			base.WriteUInt16(0, chunkedMemoryStream);
			base.WriteInt32(num, chunkedMemoryStream);
			base.WriteUInt16(4, chunkedMemoryStream);
			base.WriteByte(1, chunkedMemoryStream);
			base.WriteCountedString(uri, chunkedMemoryStream);
			base.WriteHeaders(headers, chunkedMemoryStream);
			chunkedMemoryStream.WriteTo(this.NetStream);
			chunkedMemoryStream.Close();
			StreamHelper.CopyStream(contentStream, this.NetStream);
			contentStream.Close();
		}

		// Token: 0x040001FB RID: 507
		private bool _bOneWayRequest;

		// Token: 0x040001FC RID: 508
		private TcpReadingStream _responseStream;

		// Token: 0x040001FD RID: 509
		private int _contentLength;

		// Token: 0x040001FE RID: 510
		private bool _bChunked;

		// Token: 0x040001FF RID: 511
		private IpcClientTransportSink _sink;
	}
}
