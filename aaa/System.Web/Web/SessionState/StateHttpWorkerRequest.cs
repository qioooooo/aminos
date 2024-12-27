using System;
using System.Globalization;
using System.Text;

namespace System.Web.SessionState
{
	// Token: 0x02000381 RID: 897
	internal class StateHttpWorkerRequest : HttpWorkerRequest
	{
		// Token: 0x06002B70 RID: 11120 RVA: 0x000C19A0 File Offset: 0x000C09A0
		internal StateHttpWorkerRequest(IntPtr tracker, UnsafeNativeMethods.StateProtocolVerb methodIndex, string uri, UnsafeNativeMethods.StateProtocolExclusive exclusive, int extraFlags, int timeout, int lockCookieExists, int lockCookie, int contentLength, IntPtr content)
		{
			this._tracker = tracker;
			this._methodIndex = methodIndex;
			switch (this._methodIndex)
			{
			case UnsafeNativeMethods.StateProtocolVerb.GET:
				this._method = "GET";
				break;
			case UnsafeNativeMethods.StateProtocolVerb.PUT:
				this._method = "PUT";
				break;
			case UnsafeNativeMethods.StateProtocolVerb.DELETE:
				this._method = "DELETE";
				break;
			case UnsafeNativeMethods.StateProtocolVerb.HEAD:
				this._method = "HEAD";
				break;
			}
			this._uri = uri;
			if (this._uri.StartsWith("//", StringComparison.Ordinal))
			{
				this._uri = this._uri.Substring(1);
			}
			this._exclusive = exclusive;
			this._extraFlags = extraFlags;
			this._timeout = timeout;
			this._lockCookie = lockCookie;
			this._lockCookieExists = lockCookieExists != 0;
			this._contentLength = contentLength;
			if (contentLength != 0)
			{
				uint num = (uint)(int)content;
				this._content = new byte[]
				{
					(byte)(num & 255U),
					(byte)((num & 65280U) >> 8),
					(byte)((num & 16711680U) >> 16),
					(byte)((num & 4278190080U) >> 24)
				};
			}
			this._status = new StringBuilder(256);
			this._headers = new StringBuilder(256);
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x000C1AE4 File Offset: 0x000C0AE4
		public override string GetUriPath()
		{
			return HttpUtility.UrlDecode(this._uri);
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x000C1AF1 File Offset: 0x000C0AF1
		public override string GetFilePath()
		{
			return null;
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x000C1AF4 File Offset: 0x000C0AF4
		public override string GetQueryString()
		{
			return null;
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x000C1AF7 File Offset: 0x000C0AF7
		public override string GetRawUrl()
		{
			return this._uri;
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x000C1AFF File Offset: 0x000C0AFF
		public override string GetHttpVerbName()
		{
			return this._method;
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x000C1B07 File Offset: 0x000C0B07
		public override string GetHttpVersion()
		{
			return "HTTP/1.0";
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x000C1B10 File Offset: 0x000C0B10
		public override string GetRemoteAddress()
		{
			if (this._remoteAddress == null)
			{
				StringBuilder stringBuilder = new StringBuilder(15);
				UnsafeNativeMethods.STWNDGetRemoteAddress(this._tracker, stringBuilder);
				this._remoteAddress = stringBuilder.ToString();
			}
			return this._remoteAddress;
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x000C1B4B File Offset: 0x000C0B4B
		public override int GetRemotePort()
		{
			if (this._remotePort == 0)
			{
				this._remotePort = UnsafeNativeMethods.STWNDGetRemotePort(this._tracker);
			}
			return this._remotePort;
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x000C1B6C File Offset: 0x000C0B6C
		public override string GetLocalAddress()
		{
			if (this._localAddress == null)
			{
				StringBuilder stringBuilder = new StringBuilder(15);
				UnsafeNativeMethods.STWNDGetLocalAddress(this._tracker, stringBuilder);
				this._localAddress = stringBuilder.ToString();
			}
			return this._localAddress;
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x000C1BA7 File Offset: 0x000C0BA7
		public override int GetLocalPort()
		{
			if (this._localPort == 0)
			{
				this._localPort = UnsafeNativeMethods.STWNDGetLocalPort(this._tracker);
			}
			return this._localPort;
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x000C1BC8 File Offset: 0x000C0BC8
		public override byte[] GetPreloadedEntityBody()
		{
			return this._content;
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x000C1BD0 File Offset: 0x000C0BD0
		public override bool IsEntireEntityBodyIsPreloaded()
		{
			return true;
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x000C1BD3 File Offset: 0x000C0BD3
		public override string MapPath(string virtualPath)
		{
			return virtualPath;
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x000C1BD6 File Offset: 0x000C0BD6
		public override int ReadEntityBody(byte[] buffer, int size)
		{
			return 0;
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x000C1BD9 File Offset: 0x000C0BD9
		public override long GetBytesRead()
		{
			throw new NotSupportedException(SR.GetString("Not_supported"));
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x000C1BEC File Offset: 0x000C0BEC
		public override string GetKnownRequestHeader(int index)
		{
			string text = null;
			if (index == 11)
			{
				text = this._contentLength.ToString(CultureInfo.InvariantCulture);
			}
			return text;
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x000C1C14 File Offset: 0x000C0C14
		public override string GetUnknownRequestHeader(string name)
		{
			string text = null;
			if (name.Equals("Http_Exclusive"))
			{
				switch (this._exclusive)
				{
				case UnsafeNativeMethods.StateProtocolExclusive.ACQUIRE:
					text = "acquire";
					break;
				case UnsafeNativeMethods.StateProtocolExclusive.RELEASE:
					text = "release";
					break;
				}
			}
			else if (name.Equals("Http_Timeout"))
			{
				if (this._timeout != -1)
				{
					text = this._timeout.ToString(CultureInfo.InvariantCulture);
				}
			}
			else if (name.Equals("Http_LockCookie"))
			{
				if (this._lockCookieExists)
				{
					text = this._lockCookie.ToString(CultureInfo.InvariantCulture);
				}
			}
			else if (name.Equals("Http_ExtraFlags") && this._extraFlags != -1)
			{
				text = this._extraFlags.ToString(CultureInfo.InvariantCulture);
			}
			return text;
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x000C1CD8 File Offset: 0x000C0CD8
		public override string[][] GetUnknownRequestHeaders()
		{
			int num = 0;
			if (this._exclusive != (UnsafeNativeMethods.StateProtocolExclusive)(-1))
			{
				num++;
			}
			if (this._extraFlags != -1)
			{
				num++;
			}
			if (this._timeout != -1)
			{
				num++;
			}
			if (this._lockCookieExists)
			{
				num++;
			}
			if (num == 0)
			{
				return null;
			}
			string[][] array = new string[num][];
			int num2 = 0;
			if (this._exclusive != (UnsafeNativeMethods.StateProtocolExclusive)(-1))
			{
				array[0] = new string[2];
				array[0][0] = "Http_Exclusive";
				if (this._exclusive == UnsafeNativeMethods.StateProtocolExclusive.ACQUIRE)
				{
					array[0][1] = "acquire";
				}
				else
				{
					array[0][1] = "release";
				}
				num2++;
			}
			if (this._timeout != -1)
			{
				array[num2] = new string[2];
				array[num2][0] = "Http_Timeout";
				array[num2][1] = this._timeout.ToString(CultureInfo.InvariantCulture);
				num2++;
			}
			if (this._lockCookieExists)
			{
				array[num2] = new string[2];
				array[num2][0] = "Http_LockCookie";
				array[num2][1] = this._lockCookie.ToString(CultureInfo.InvariantCulture);
				num2++;
			}
			if (this._extraFlags != -1)
			{
				array[num2] = new string[2];
				array[num2][0] = "Http_ExtraFlags";
				array[num2][1] = this._extraFlags.ToString(CultureInfo.InvariantCulture);
				num2++;
			}
			return array;
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x000C1E06 File Offset: 0x000C0E06
		public override void SendStatus(int statusCode, string statusDescription)
		{
			this._statusCode = statusCode;
			this._status.Append(statusCode.ToString(CultureInfo.InvariantCulture) + " " + statusDescription + "\r\n");
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x000C1E38 File Offset: 0x000C0E38
		public override void SendKnownResponseHeader(int index, string value)
		{
			this._headers.Append(HttpWorkerRequest.GetKnownResponseHeaderName(index));
			this._headers.Append(": ");
			this._headers.Append(value);
			this._headers.Append("\r\n");
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x000C1E86 File Offset: 0x000C0E86
		public override void SendUnknownResponseHeader(string name, string value)
		{
			this._headers.Append(name);
			this._headers.Append(": ");
			this._headers.Append(value);
			this._headers.Append("\r\n");
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x000C1EC4 File Offset: 0x000C0EC4
		public override void SendCalculatedContentLength(int contentLength)
		{
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x000C1EC6 File Offset: 0x000C0EC6
		public override bool HeadersSent()
		{
			return this._sent;
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000C1ECE File Offset: 0x000C0ECE
		public override bool IsClientConnected()
		{
			return UnsafeNativeMethods.STWNDIsClientConnected(this._tracker);
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x000C1EDB File Offset: 0x000C0EDB
		public override void CloseConnection()
		{
			UnsafeNativeMethods.STWNDCloseConnection(this._tracker);
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x000C1EE8 File Offset: 0x000C0EE8
		private void SendResponse()
		{
			if (!this._sent)
			{
				this._sent = true;
				UnsafeNativeMethods.STWNDSendResponse(this._tracker, this._status, this._status.Length, this._headers, this._headers.Length, this._unmanagedState);
			}
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x000C1F38 File Offset: 0x000C0F38
		public override void SendResponseFromMemory(byte[] data, int length)
		{
			if (this._statusCode == 200)
			{
				if (IntPtr.Size == 4)
				{
					this._unmanagedState = (IntPtr)((int)data[0] | ((int)data[1] << 8) | ((int)data[2] << 16) | ((int)data[3] << 24));
				}
				else
				{
					this._unmanagedState = (IntPtr)((long)((ulong)data[0] | ((ulong)data[1] << 8) | ((ulong)data[2] << 16) | ((ulong)data[3] << 24) | ((ulong)data[4] << 32) | ((ulong)data[5] << 40) | ((ulong)data[6] << 48) | ((ulong)data[7] << 56)));
				}
			}
			this.SendResponse();
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x000C1FCA File Offset: 0x000C0FCA
		public override void SendResponseFromFile(string filename, long offset, long length)
		{
			throw new NotSupportedException(SR.GetString("Not_supported"));
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x000C1FDB File Offset: 0x000C0FDB
		public override void SendResponseFromFile(IntPtr handle, long offset, long length)
		{
			throw new NotSupportedException(SR.GetString("Not_supported"));
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x000C1FEC File Offset: 0x000C0FEC
		public override void FlushResponse(bool finalFlush)
		{
			this.SendResponse();
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000C1FF4 File Offset: 0x000C0FF4
		public override void EndOfRequest()
		{
			this.SendResponse();
			UnsafeNativeMethods.STWNDEndOfRequest(this._tracker);
		}

		// Token: 0x04002010 RID: 8208
		private const int ADDRESS_LENGTH_MAX = 15;

		// Token: 0x04002011 RID: 8209
		private IntPtr _tracker;

		// Token: 0x04002012 RID: 8210
		private string _uri;

		// Token: 0x04002013 RID: 8211
		private UnsafeNativeMethods.StateProtocolExclusive _exclusive;

		// Token: 0x04002014 RID: 8212
		private int _extraFlags;

		// Token: 0x04002015 RID: 8213
		private int _timeout;

		// Token: 0x04002016 RID: 8214
		private int _lockCookie;

		// Token: 0x04002017 RID: 8215
		private bool _lockCookieExists;

		// Token: 0x04002018 RID: 8216
		private int _contentLength;

		// Token: 0x04002019 RID: 8217
		private byte[] _content;

		// Token: 0x0400201A RID: 8218
		private UnsafeNativeMethods.StateProtocolVerb _methodIndex;

		// Token: 0x0400201B RID: 8219
		private string _method;

		// Token: 0x0400201C RID: 8220
		private string _remoteAddress;

		// Token: 0x0400201D RID: 8221
		private int _remotePort;

		// Token: 0x0400201E RID: 8222
		private string _localAddress;

		// Token: 0x0400201F RID: 8223
		private int _localPort;

		// Token: 0x04002020 RID: 8224
		private StringBuilder _status;

		// Token: 0x04002021 RID: 8225
		private int _statusCode;

		// Token: 0x04002022 RID: 8226
		private StringBuilder _headers;

		// Token: 0x04002023 RID: 8227
		private IntPtr _unmanagedState;

		// Token: 0x04002024 RID: 8228
		private bool _sent;
	}
}
