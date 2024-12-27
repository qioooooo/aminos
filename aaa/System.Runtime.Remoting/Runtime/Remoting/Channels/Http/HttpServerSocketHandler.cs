using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000037 RID: 55
	internal sealed class HttpServerSocketHandler : HttpSocketHandler
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x00008CDF File Offset: 0x00007CDF
		internal HttpServerSocketHandler(Socket socket, RequestQueue requestQueue, Stream stream)
			: base(socket, requestQueue, stream)
		{
			this._connectionId = Interlocked.Increment(ref HttpServerSocketHandler._connectionIdCounter);
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00008CFA File Offset: 0x00007CFA
		public bool AllowChunkedResponse
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008CFD File Offset: 0x00007CFD
		public bool CanServiceAnotherRequest()
		{
			return this._keepAlive && this._requestStream != null && (this._requestStream.FoundEnd || this._requestStream.ReadToEnd());
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008D2C File Offset: 0x00007D2C
		protected override void PrepareForNewMessage()
		{
			this._requestStream = null;
			this._responseStream = null;
			this._contentLength = 0;
			this._chunkedEncoding = false;
			this._keepAlive = false;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008D51 File Offset: 0x00007D51
		private string GenerateFaultString(Exception e)
		{
			if (!base.CustomErrorsEnabled())
			{
				return e.ToString();
			}
			return CoreChannel.GetResourceString("Remoting_InternalError");
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00008D6C File Offset: 0x00007D6C
		protected override void SendErrorMessageIfPossible(Exception e)
		{
			if (this._responseStream == null && !(e is SocketException))
			{
				Stream stream = new MemoryStream();
				StreamWriter streamWriter = new StreamWriter(stream, new UTF8Encoding(false));
				streamWriter.WriteLine(this.GenerateFaultString(e));
				streamWriter.Flush();
				this.SendResponse(stream, "500", CoreChannel.GetResourceString("Remoting_InternalError"), null);
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00008DC6 File Offset: 0x00007DC6
		private static bool ValidateVerbCharacter(byte b)
		{
			return char.IsLetter((char)b) || b == 45;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008DD8 File Offset: 0x00007DD8
		public BaseTransportHeaders ReadHeaders()
		{
			bool flag = false;
			BaseTransportHeaders baseTransportHeaders = new BaseTransportHeaders();
			string text;
			string text2;
			string text3;
			this.ReadFirstLine(out text, out text2, out text3);
			if (text == null || text2 == null || text3 == null)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Http_UnableToReadFirstLine"));
			}
			if (text3.Equals("HTTP/1.1"))
			{
				this._version = HttpVersion.V1_1;
			}
			else if (text3.Equals("HTTP/1.0"))
			{
				this._version = HttpVersion.V1_0;
			}
			else
			{
				this._version = HttpVersion.V1_1;
			}
			if (this._version == HttpVersion.V1_1)
			{
				this._keepAlive = true;
			}
			else
			{
				this._keepAlive = false;
			}
			string text4;
			if (HttpChannelHelper.ParseURL(text2, out text4) == null)
			{
				text4 = text2;
			}
			baseTransportHeaders["__RequestVerb"] = text;
			baseTransportHeaders.RequestUri = text4;
			baseTransportHeaders["__HttpVersion"] = text3;
			if (this._version == HttpVersion.V1_1 && (text.Equals("POST") || text.Equals("PUT")))
			{
				flag = true;
			}
			base.ReadToEndOfHeaders(baseTransportHeaders, out this._chunkedEncoding, out this._contentLength, ref this._keepAlive, ref flag);
			if (flag && this._version != HttpVersion.V1_0)
			{
				this.SendContinue();
			}
			baseTransportHeaders["__IPAddress"] = ((IPEndPoint)this.NetSocket.RemoteEndPoint).Address;
			baseTransportHeaders["__ConnectionId"] = this._connectionId;
			return baseTransportHeaders;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008F1C File Offset: 0x00007F1C
		public Stream GetRequestStream()
		{
			if (this._chunkedEncoding)
			{
				this._requestStream = new HttpChunkedReadingStream(this);
			}
			else
			{
				this._requestStream = new HttpFixedLengthReadingStream(this, this._contentLength);
			}
			return this._requestStream;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008F4C File Offset: 0x00007F4C
		public Stream GetResponseStream(string statusCode, string reasonPhrase, ITransportHeaders headers)
		{
			bool flag = false;
			int num = 0;
			object obj = headers["__HttpStatusCode"];
			string text = headers["__HttpReasonPhrase"] as string;
			if (obj != null)
			{
				statusCode = obj.ToString();
			}
			if (text != null)
			{
				reasonPhrase = text;
			}
			if (!this.CanServiceAnotherRequest())
			{
				headers["Connection"] = "Close";
			}
			object obj2 = headers["Content-Length"];
			if (obj2 != null)
			{
				flag = true;
				if (obj2 is int)
				{
					num = (int)obj2;
				}
				else
				{
					num = Convert.ToInt32(obj2, CultureInfo.InvariantCulture);
				}
			}
			bool flag2 = this.AllowChunkedResponse && !flag;
			if (flag2)
			{
				headers["Transfer-Encoding"] = "chunked";
			}
			ChunkedMemoryStream chunkedMemoryStream = new ChunkedMemoryStream(CoreChannel.BufferPool);
			base.WriteResponseFirstLine(statusCode, reasonPhrase, chunkedMemoryStream);
			base.WriteHeaders(headers, chunkedMemoryStream);
			chunkedMemoryStream.WriteTo(this.NetStream);
			chunkedMemoryStream.Close();
			if (flag2)
			{
				this._responseStream = new HttpChunkedResponseStream(this.NetStream);
			}
			else
			{
				this._responseStream = new HttpFixedLengthResponseStream(this.NetStream, num);
			}
			return this._responseStream;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00009060 File Offset: 0x00008060
		private bool ReadFirstLine(out string verb, out string requestURI, out string version)
		{
			verb = null;
			requestURI = null;
			version = null;
			verb = base.ReadToChar(' ', HttpServerSocketHandler.s_validateVerbDelegate);
			byte[] array = base.ReadToByte(32);
			int num;
			HttpChannelHelper.DecodeUriInPlace(array, out num);
			requestURI = Encoding.UTF8.GetString(array, 0, num);
			version = base.ReadToEndOfLine();
			return true;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000090AE File Offset: 0x000080AE
		private void SendContinue()
		{
			this.NetStream.Write(HttpServerSocketHandler._bufferhttpContinue, 0, HttpServerSocketHandler._bufferhttpContinue.Length);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000090C8 File Offset: 0x000080C8
		public void SendResponse(Stream httpContentStream, string statusCode, string reasonPhrase, ITransportHeaders headers)
		{
			if (this._responseStream == null)
			{
				if (headers == null)
				{
					headers = new TransportHeaders();
				}
				string text = (string)headers["Server"];
				if (text != null)
				{
					text = HttpServerTransportSink.ServerHeader + ", " + text;
				}
				else
				{
					text = HttpServerTransportSink.ServerHeader;
				}
				headers["Server"] = text;
				if (!this.AllowChunkedResponse && httpContentStream != null)
				{
					headers["Content-Length"] = httpContentStream.Length.ToString(CultureInfo.InvariantCulture);
				}
				else if (httpContentStream == null)
				{
					headers["Content-Length"] = "0";
				}
				this.GetResponseStream(statusCode, reasonPhrase, headers);
				if (httpContentStream != null)
				{
					StreamHelper.CopyStream(httpContentStream, this._responseStream);
					this._responseStream.Close();
					httpContentStream.Close();
				}
				this._responseStream = null;
				return;
			}
			this._responseStream.Close();
			if (this._responseStream != httpContentStream)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Http_WrongResponseStream"));
			}
			this._responseStream = null;
		}

		// Token: 0x0400014C RID: 332
		private static ValidateByteDelegate s_validateVerbDelegate = new ValidateByteDelegate(HttpServerSocketHandler.ValidateVerbCharacter);

		// Token: 0x0400014D RID: 333
		private static long _connectionIdCounter = 0L;

		// Token: 0x0400014E RID: 334
		private static byte[] _bufferhttpContinue = Encoding.ASCII.GetBytes("HTTP/1.1 100 Continue\r\n\r\n");

		// Token: 0x0400014F RID: 335
		private HttpReadingStream _requestStream;

		// Token: 0x04000150 RID: 336
		private HttpServerResponseStream _responseStream;

		// Token: 0x04000151 RID: 337
		private long _connectionId;

		// Token: 0x04000152 RID: 338
		private HttpVersion _version;

		// Token: 0x04000153 RID: 339
		private int _contentLength;

		// Token: 0x04000154 RID: 340
		private bool _chunkedEncoding;

		// Token: 0x04000155 RID: 341
		private bool _keepAlive;
	}
}
