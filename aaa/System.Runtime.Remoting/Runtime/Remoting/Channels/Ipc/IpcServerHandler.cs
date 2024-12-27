using System;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting.Channels.Tcp;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x02000057 RID: 87
	internal class IpcServerHandler : TcpSocketHandler
	{
		// Token: 0x060002BC RID: 700 RVA: 0x0000D7AC File Offset: 0x0000C7AC
		internal IpcServerHandler(IpcPort port, RequestQueue requestQueue, Stream stream)
			: base(null, requestQueue, stream)
		{
			this._requestQueue = requestQueue;
			this._port = port;
			this._stream = stream;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000D7CC File Offset: 0x0000C7CC
		internal Stream GetRequestStream()
		{
			this._requestStream = new TcpFixedLengthReadingStream(this, this._contentLength);
			return this._requestStream;
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000D7E6 File Offset: 0x0000C7E6
		internal IpcPort Port
		{
			get
			{
				return this._port;
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000D7F0 File Offset: 0x0000C7F0
		internal ITransportHeaders ReadHeaders()
		{
			BaseTransportHeaders baseTransportHeaders = new BaseTransportHeaders();
			ushort num;
			base.ReadVersionAndOperation(out num);
			if (num == 1)
			{
				this._bOneWayRequest = true;
			}
			bool flag = false;
			base.ReadContentLength(out flag, out this._contentLength);
			this.ReadToEndOfHeaders(baseTransportHeaders);
			return baseTransportHeaders;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000D830 File Offset: 0x0000C830
		protected new void ReadToEndOfHeaders(BaseTransportHeaders headers)
		{
			bool flag = false;
			string text = null;
			for (ushort num = base.ReadUInt16(); num != 0; num = base.ReadUInt16())
			{
				if (num == 1)
				{
					string text2 = base.ReadCountedString();
					string text3 = base.ReadCountedString();
					headers[text2] = text3;
				}
				else if (num == 4)
				{
					this.ReadAndVerifyHeaderFormat("RequestUri", 1);
					string text4 = base.ReadCountedString();
					string text5;
					if (IpcChannelHelper.ParseURL(text4, out text5) == null)
					{
						text5 = text4;
					}
					headers.RequestUri = text5;
				}
				else if (num == 2)
				{
					this.ReadAndVerifyHeaderFormat("StatusCode", 3);
					ushort num2 = base.ReadUInt16();
					if (num2 != 0)
					{
						flag = true;
					}
				}
				else if (num == 3)
				{
					this.ReadAndVerifyHeaderFormat("StatusPhrase", 1);
					text = base.ReadCountedString();
				}
				else if (num == 6)
				{
					this.ReadAndVerifyHeaderFormat("Content-Type", 1);
					string text6 = base.ReadCountedString();
					headers.ContentType = text6;
				}
				else
				{
					byte b = (byte)base.ReadByte();
					switch (b)
					{
					case 0:
						break;
					case 1:
						base.ReadCountedString();
						break;
					case 2:
						base.ReadByte();
						break;
					case 3:
						base.ReadUInt16();
						break;
					case 4:
						base.ReadInt32();
						break;
					default:
						throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_UnknownHeaderType"), new object[] { num, b }));
					}
				}
			}
			if (flag)
			{
				if (text == null)
				{
					text = "";
				}
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_GenericServerError"), new object[] { text }));
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000D9D8 File Offset: 0x0000C9D8
		private void ReadAndVerifyHeaderFormat(string headerName, byte expectedFormat)
		{
			byte b = (byte)base.ReadByte();
			if (b != expectedFormat)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_IncorrectHeaderFormat"), new object[] { expectedFormat, headerName }));
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000DA20 File Offset: 0x0000CA20
		protected override void PrepareForNewMessage()
		{
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000DA24 File Offset: 0x0000CA24
		protected override void SendErrorMessageIfPossible(Exception e)
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
			base.WriteCountedString(e.ToString(), chunkedMemoryStream);
			base.WriteUInt16(5, chunkedMemoryStream);
			base.WriteByte(0, chunkedMemoryStream);
			base.WriteUInt16(0, chunkedMemoryStream);
			chunkedMemoryStream.WriteTo(this.NetStream);
			chunkedMemoryStream.Close();
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000DAC4 File Offset: 0x0000CAC4
		internal void SendResponse(ITransportHeaders headers, Stream contentStream)
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

		// Token: 0x040001F5 RID: 501
		private Stream _stream;

		// Token: 0x040001F6 RID: 502
		protected Stream _requestStream;

		// Token: 0x040001F7 RID: 503
		protected IpcPort _port;

		// Token: 0x040001F8 RID: 504
		private RequestQueue _requestQueue;

		// Token: 0x040001F9 RID: 505
		private bool _bOneWayRequest;

		// Token: 0x040001FA RID: 506
		private int _contentLength;
	}
}
