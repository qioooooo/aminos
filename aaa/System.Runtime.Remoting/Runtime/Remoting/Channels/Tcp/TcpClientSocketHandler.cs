using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x02000042 RID: 66
	internal class TcpClientSocketHandler : TcpSocketHandler
	{
		// Token: 0x06000229 RID: 553 RVA: 0x0000B20A File Offset: 0x0000A20A
		public TcpClientSocketHandler(Socket socket, string machinePortAndSid, Stream stream, TcpClientTransportSink sink)
			: base(socket, stream)
		{
			this._machinePortAndSid = machinePortAndSid;
			this._sink = sink;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000B223 File Offset: 0x0000A223
		protected override void PrepareForNewMessage()
		{
			this._requestStream = null;
			this._responseStream = null;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000B233 File Offset: 0x0000A233
		public override void OnInputStreamClosed()
		{
			if (this._responseStream != null)
			{
				this._responseStream.ReadToEnd();
				this._responseStream = null;
			}
			this.ReturnToCache();
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000B258 File Offset: 0x0000A258
		public BaseTransportHeaders ReadHeaders()
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

		// Token: 0x0600022D RID: 557 RVA: 0x0000B2C4 File Offset: 0x0000A2C4
		public Stream GetRequestStream(IMessage msg, int contentLength, ITransportHeaders headers)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
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
			base.WriteInt32(contentLength, chunkedMemoryStream);
			base.WriteUInt16(4, chunkedMemoryStream);
			base.WriteByte(1, chunkedMemoryStream);
			base.WriteCountedString(uri, chunkedMemoryStream);
			base.WriteHeaders(headers, chunkedMemoryStream);
			chunkedMemoryStream.WriteTo(this.NetStream);
			chunkedMemoryStream.Close();
			this._requestStream = this.NetStream;
			return this._requestStream;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000B370 File Offset: 0x0000A370
		public void SendRequest(IMessage msg, ITransportHeaders headers, Stream contentStream)
		{
			int num = (int)contentStream.Length;
			this.GetRequestStream(msg, num, headers);
			StreamHelper.CopyStream(contentStream, this.NetStream);
			contentStream.Close();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000B3A1 File Offset: 0x0000A3A1
		public Stream GetResponseStream()
		{
			if (!this._bChunked)
			{
				this._responseStream = new TcpFixedLengthReadingStream(this, this._contentLength);
			}
			else
			{
				this._responseStream = new TcpChunkedReadingStream(this);
			}
			return this._responseStream;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000B3D1 File Offset: 0x0000A3D1
		public bool OneWayRequest
		{
			get
			{
				return this._bOneWayRequest;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000B3D9 File Offset: 0x0000A3D9
		public void ReturnToCache()
		{
			this._sink.ClientSocketCache.ReleaseSocket(this._machinePortAndSid, this);
		}

		// Token: 0x0400018A RID: 394
		private static byte[] s_endOfLineBytes = Encoding.ASCII.GetBytes("\r\n");

		// Token: 0x0400018B RID: 395
		private string _machinePortAndSid;

		// Token: 0x0400018C RID: 396
		private bool _bOneWayRequest;

		// Token: 0x0400018D RID: 397
		private bool _bChunked;

		// Token: 0x0400018E RID: 398
		private int _contentLength;

		// Token: 0x0400018F RID: 399
		private Stream _requestStream;

		// Token: 0x04000190 RID: 400
		private TcpReadingStream _responseStream;

		// Token: 0x04000191 RID: 401
		private TcpClientTransportSink _sink;
	}
}
