using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x0200003B RID: 59
	internal class HttpHandlerTransportSink : IServerChannelSink, IChannelSinkBase
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x00009B33 File Offset: 0x00008B33
		public HttpHandlerTransportSink(IServerChannelSink nextSink)
		{
			this._nextSink = nextSink;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009B44 File Offset: 0x00008B44
		public void HandleRequest(HttpContext context)
		{
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			BaseTransportHeaders baseTransportHeaders = new BaseTransportHeaders();
			NameValueCollection headers = request.Headers;
			string[] allKeys = headers.AllKeys;
			if (AppSettings.LateHttpHeaderParsing)
			{
				baseTransportHeaders["__RequestVerb"] = request.HttpMethod;
				baseTransportHeaders["__CustomErrorsEnabled"] = HttpRemotingHandler.CustomErrorsEnabled(context);
				baseTransportHeaders.RequestUri = (string)context.Items["__requestUri"];
			}
			foreach (string text in allKeys)
			{
				string text2 = headers[text];
				baseTransportHeaders[text] = text2;
			}
			if (!AppSettings.LateHttpHeaderParsing)
			{
				baseTransportHeaders["__RequestVerb"] = request.HttpMethod;
				baseTransportHeaders["__CustomErrorsEnabled"] = HttpRemotingHandler.CustomErrorsEnabled(context);
				baseTransportHeaders.RequestUri = (string)context.Items["__requestUri"];
			}
			baseTransportHeaders.IPAddress = IPAddress.Parse(request.UserHostAddress);
			Stream inputStream = request.InputStream;
			ServerChannelSinkStack serverChannelSinkStack = new ServerChannelSinkStack();
			serverChannelSinkStack.Push(this, null);
			IMessage message;
			ITransportHeaders transportHeaders;
			Stream stream;
			switch (this._nextSink.ProcessMessage(serverChannelSinkStack, null, baseTransportHeaders, inputStream, out message, out transportHeaders, out stream))
			{
			case ServerProcessing.Complete:
				this.SendResponse(response, 200, transportHeaders, stream);
				return;
			case ServerProcessing.OneWay:
				this.SendResponse(response, 202, transportHeaders, stream);
				break;
			case ServerProcessing.Async:
				break;
			default:
				return;
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009CB0 File Offset: 0x00008CB0
		private void SendResponse(HttpResponse httpResponse, int statusCode, ITransportHeaders responseHeaders, Stream httpContentStream)
		{
			if (responseHeaders != null)
			{
				string text = (string)responseHeaders["Server"];
				if (text != null)
				{
					text = HttpServerTransportSink.ServerHeader + ", " + text;
				}
				else
				{
					text = HttpServerTransportSink.ServerHeader;
				}
				responseHeaders["Server"] = text;
				object obj = responseHeaders["__HttpStatusCode"];
				if (obj != null)
				{
					statusCode = Convert.ToInt32(obj, CultureInfo.InvariantCulture);
				}
				if (httpContentStream != null)
				{
					int num = -1;
					try
					{
						if (httpContentStream != null)
						{
							num = (int)httpContentStream.Length;
						}
					}
					catch
					{
					}
					if (num != -1)
					{
						responseHeaders["Content-Length"] = num;
					}
				}
				else
				{
					responseHeaders["Content-Length"] = 0;
				}
				foreach (object obj2 in responseHeaders)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
					string text2 = (string)dictionaryEntry.Key;
					if (!text2.StartsWith("__", StringComparison.Ordinal))
					{
						httpResponse.AppendHeader(text2, dictionaryEntry.Value.ToString());
					}
				}
			}
			HttpRemotingHandler.SetHttpResponseStatusCode(httpResponse, statusCode);
			Stream outputStream = httpResponse.OutputStream;
			if (httpContentStream != null)
			{
				StreamHelper.CopyStream(httpContentStream, outputStream);
				httpContentStream.Close();
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009E00 File Offset: 0x00008E00
		public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00009E07 File Offset: 0x00008E07
		public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00009E0E File Offset: 0x00008E0E
		public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
		{
			return null;
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00009E11 File Offset: 0x00008E11
		public IServerChannelSink NextChannelSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00009E19 File Offset: 0x00008E19
		public IDictionary Properties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0400015E RID: 350
		private const int _defaultChunkSize = 2048;

		// Token: 0x0400015F RID: 351
		public IServerChannelSink _nextSink;
	}
}
