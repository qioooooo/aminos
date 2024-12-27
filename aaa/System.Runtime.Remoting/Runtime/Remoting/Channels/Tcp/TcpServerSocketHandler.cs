using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x02000048 RID: 72
	internal sealed class TcpServerSocketHandler : TcpSocketHandler
	{
		// Token: 0x06000268 RID: 616 RVA: 0x0000C323 File Offset: 0x0000B323
		internal TcpServerSocketHandler(Socket socket, RequestQueue requestQueue, Stream stream)
			: base(socket, requestQueue, stream)
		{
			this._connectionId = Interlocked.Increment(ref TcpServerSocketHandler._connectionIdCounter);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000C33E File Offset: 0x0000B33E
		public bool CanServiceAnotherRequest()
		{
			return true;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000C341 File Offset: 0x0000B341
		protected override void PrepareForNewMessage()
		{
			if (this._requestStream != null)
			{
				if (!this._requestStream.FoundEnd)
				{
					this._requestStream.ReadToEnd();
				}
				this._requestStream = null;
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000C36C File Offset: 0x0000B36C
		protected override void SendErrorMessageIfPossible(Exception e)
		{
			try
			{
				this.SendErrorResponse(e, true);
			}
			catch
			{
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000C398 File Offset: 0x0000B398
		public ITransportHeaders ReadHeaders()
		{
			BaseTransportHeaders baseTransportHeaders = new BaseTransportHeaders();
			ushort num;
			base.ReadVersionAndOperation(out num);
			if (num == 0)
			{
				this._bOneWayRequest = false;
			}
			else
			{
				if (num != 1)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_ExpectingRequestOp"), new object[] { num.ToString(CultureInfo.CurrentCulture) }));
				}
				this._bOneWayRequest = true;
			}
			base.ReadContentLength(out this._bChunked, out this._contentLength);
			base.ReadToEndOfHeaders(baseTransportHeaders);
			baseTransportHeaders.IPAddress = ((IPEndPoint)this.NetSocket.RemoteEndPoint).Address;
			baseTransportHeaders.ConnectionId = this._connectionId;
			return baseTransportHeaders;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000C443 File Offset: 0x0000B443
		public Stream GetRequestStream()
		{
			if (!this._bChunked)
			{
				this._requestStream = new TcpFixedLengthReadingStream(this, this._contentLength);
			}
			else
			{
				this._requestStream = new TcpChunkedReadingStream(this);
			}
			return this._requestStream;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000C474 File Offset: 0x0000B474
		public void SendResponse(ITransportHeaders headers, Stream contentStream)
		{
			if (this._bOneWayRequest)
			{
				return;
			}
			ChunkedMemoryStream chunkedMemoryStream = new ChunkedMemoryStream(CoreChannel.BufferPool);
			base.WritePreambleAndVersion(chunkedMemoryStream);
			base.WriteUInt16(2, chunkedMemoryStream);
			base.WriteUInt16(0, chunkedMemoryStream);
			base.WriteInt32((int)contentStream.Length, chunkedMemoryStream);
			base.WriteHeaders(headers, chunkedMemoryStream);
			chunkedMemoryStream.WriteTo(this.NetStream);
			chunkedMemoryStream.Close();
			StreamHelper.CopyStream(contentStream, this.NetStream);
			contentStream.Close();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000C4E6 File Offset: 0x0000B4E6
		private string GenerateFaultString(Exception e)
		{
			if (!base.CustomErrorsEnabled())
			{
				return e.ToString();
			}
			return CoreChannel.GetResourceString("Remoting_InternalError");
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000C501 File Offset: 0x0000B501
		public void SendErrorResponse(Exception e, bool bCloseConnection)
		{
			this.SendErrorResponse(this.GenerateFaultString(e), bCloseConnection);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000C514 File Offset: 0x0000B514
		public void SendErrorResponse(string e, bool bCloseConnection)
		{
			if (this._bOneWayRequest)
			{
				return;
			}
			ChunkedMemoryStream chunkedMemoryStream = new ChunkedMemoryStream(CoreChannel.BufferPool);
			base.WritePreambleAndVersion(chunkedMemoryStream);
			base.WriteUInt16(2, chunkedMemoryStream);
			base.WriteUInt16(0, chunkedMemoryStream);
			base.WriteInt32(0, chunkedMemoryStream);
			base.WriteUInt16(2, chunkedMemoryStream);
			base.WriteByte(3, chunkedMemoryStream);
			base.WriteUInt16(1, chunkedMemoryStream);
			base.WriteUInt16(3, chunkedMemoryStream);
			base.WriteByte(1, chunkedMemoryStream);
			base.WriteCountedString(e, chunkedMemoryStream);
			base.WriteUInt16(5, chunkedMemoryStream);
			base.WriteByte(0, chunkedMemoryStream);
			base.WriteUInt16(0, chunkedMemoryStream);
			chunkedMemoryStream.WriteTo(this.NetStream);
			chunkedMemoryStream.Close();
		}

		// Token: 0x040001AF RID: 431
		private static byte[] s_endOfLineBytes = Encoding.ASCII.GetBytes("\r\n");

		// Token: 0x040001B0 RID: 432
		private static long _connectionIdCounter = 0L;

		// Token: 0x040001B1 RID: 433
		private long _connectionId;

		// Token: 0x040001B2 RID: 434
		private bool _bOneWayRequest;

		// Token: 0x040001B3 RID: 435
		private bool _bChunked;

		// Token: 0x040001B4 RID: 436
		private int _contentLength;

		// Token: 0x040001B5 RID: 437
		private TcpReadingStream _requestStream;
	}
}
