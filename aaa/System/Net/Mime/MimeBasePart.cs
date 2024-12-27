using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x020006A9 RID: 1705
	internal class MimeBasePart
	{
		// Token: 0x060034A1 RID: 13473 RVA: 0x000DF6B3 File Offset: 0x000DE6B3
		internal MimeBasePart()
		{
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000DF6BB File Offset: 0x000DE6BB
		internal static bool ShouldUseBase64Encoding(Encoding encoding)
		{
			return encoding == Encoding.Unicode || encoding == Encoding.UTF8 || encoding == Encoding.UTF32 || encoding == Encoding.BigEndianUnicode;
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000DF6E0 File Offset: 0x000DE6E0
		internal static string EncodeHeaderValue(string value, Encoding encoding, bool base64Encoding)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (encoding == null && MimeBasePart.IsAscii(value, false))
			{
				return value;
			}
			if (encoding == null)
			{
				encoding = Encoding.GetEncoding("utf-8");
			}
			string text = encoding.BodyName;
			if (encoding == Encoding.BigEndianUnicode)
			{
				text = "utf-16be";
			}
			stringBuilder.Append("=?");
			stringBuilder.Append(text);
			stringBuilder.Append("?");
			stringBuilder.Append(base64Encoding ? "B" : "Q");
			stringBuilder.Append("?");
			byte[] bytes = encoding.GetBytes(value);
			if (base64Encoding)
			{
				Base64Stream base64Stream = new Base64Stream(-1);
				base64Stream.EncodeBytes(bytes, 0, bytes.Length, true);
				stringBuilder.Append(Encoding.ASCII.GetString(base64Stream.WriteState.Buffer, 0, base64Stream.WriteState.Length));
			}
			else
			{
				QuotedPrintableStream quotedPrintableStream = new QuotedPrintableStream(-1);
				quotedPrintableStream.EncodeBytes(bytes, 0, bytes.Length);
				stringBuilder.Append(Encoding.ASCII.GetString(quotedPrintableStream.WriteState.Buffer, 0, quotedPrintableStream.WriteState.Length));
			}
			stringBuilder.Append("?=");
			return stringBuilder.ToString();
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000DF804 File Offset: 0x000DE804
		internal static string DecodeHeaderValue(string value)
		{
			if (value == null || value.Length == 0)
			{
				return string.Empty;
			}
			string[] array = value.Split(new char[] { '?' });
			if (array.Length != 5 || array[0] != "=" || array[4] != "=")
			{
				return value;
			}
			string text = array[1];
			bool flag = array[2] == "B";
			byte[] bytes = Encoding.ASCII.GetBytes(array[3]);
			int num;
			if (flag)
			{
				Base64Stream base64Stream = new Base64Stream();
				num = base64Stream.DecodeBytes(bytes, 0, bytes.Length);
			}
			else
			{
				QuotedPrintableStream quotedPrintableStream = new QuotedPrintableStream();
				num = quotedPrintableStream.DecodeBytes(bytes, 0, bytes.Length);
			}
			Encoding encoding = Encoding.GetEncoding(text);
			return encoding.GetString(bytes, 0, num);
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000DF8C8 File Offset: 0x000DE8C8
		internal static Encoding DecodeEncoding(string value)
		{
			if (value == null || value.Length == 0)
			{
				return null;
			}
			string[] array = value.Split(new char[] { '?' });
			if (array.Length != 5 || array[0] != "=" || array[4] != "=")
			{
				return null;
			}
			string text = array[1];
			return Encoding.GetEncoding(text);
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000DF928 File Offset: 0x000DE928
		internal static bool IsAscii(string value, bool permitCROrLF)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				bool flag;
				if (c > '\u007f')
				{
					flag = false;
				}
				else
				{
					if (permitCROrLF || (c != '\r' && c != '\n'))
					{
						i++;
						continue;
					}
					flag = false;
				}
				return flag;
			}
			return true;
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x000DF97C File Offset: 0x000DE97C
		internal static bool IsAnsi(string value, bool permitCROrLF)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				bool flag;
				if (c > 'ÿ')
				{
					flag = false;
				}
				else
				{
					if (permitCROrLF || (c != '\r' && c != '\n'))
					{
						i++;
						continue;
					}
					flag = false;
				}
				return flag;
			}
			return true;
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x060034A8 RID: 13480 RVA: 0x000DF9D2 File Offset: 0x000DE9D2
		// (set) Token: 0x060034A9 RID: 13481 RVA: 0x000DF9E5 File Offset: 0x000DE9E5
		internal string ContentID
		{
			get
			{
				return this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentID)];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.ContentID));
					return;
				}
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentID)] = value;
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x060034AA RID: 13482 RVA: 0x000DFA13 File Offset: 0x000DEA13
		// (set) Token: 0x060034AB RID: 13483 RVA: 0x000DFA26 File Offset: 0x000DEA26
		internal string ContentLocation
		{
			get
			{
				return this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentLocation)];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.ContentLocation));
					return;
				}
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentLocation)] = value;
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x060034AC RID: 13484 RVA: 0x000DFA54 File Offset: 0x000DEA54
		internal NameValueCollection Headers
		{
			get
			{
				if (this.headers == null)
				{
					this.headers = new HeaderCollection();
				}
				if (this.contentType == null)
				{
					this.contentType = new ContentType();
				}
				this.contentType.PersistIfNeeded(this.headers, false);
				if (this.contentDisposition != null)
				{
					this.contentDisposition.PersistIfNeeded(this.headers, false);
				}
				return this.headers;
			}
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x060034AD RID: 13485 RVA: 0x000DFAB9 File Offset: 0x000DEAB9
		// (set) Token: 0x060034AE RID: 13486 RVA: 0x000DFAD4 File Offset: 0x000DEAD4
		internal ContentType ContentType
		{
			get
			{
				if (this.contentType == null)
				{
					this.contentType = new ContentType();
				}
				return this.contentType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.contentType = value;
				this.contentType.PersistIfNeeded((HeaderCollection)this.Headers, true);
			}
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000DFB02 File Offset: 0x000DEB02
		internal virtual void Send(BaseWriter writer)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000DFB09 File Offset: 0x000DEB09
		internal virtual IAsyncResult BeginSend(BaseWriter writer, AsyncCallback callback, object state)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000DFB10 File Offset: 0x000DEB10
		internal void EndSend(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as MimeBasePart.MimePartAsyncResult;
			if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (lazyAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndSend" }));
			}
			lazyAsyncResult.InternalWaitForCompletion();
			lazyAsyncResult.EndCalled = true;
			if (lazyAsyncResult.Result is Exception)
			{
				throw (Exception)lazyAsyncResult.Result;
			}
		}

		// Token: 0x04003066 RID: 12390
		internal const string defaultCharSet = "utf-8";

		// Token: 0x04003067 RID: 12391
		protected ContentType contentType;

		// Token: 0x04003068 RID: 12392
		protected ContentDisposition contentDisposition;

		// Token: 0x04003069 RID: 12393
		private HeaderCollection headers;

		// Token: 0x020006AA RID: 1706
		internal class MimePartAsyncResult : LazyAsyncResult
		{
			// Token: 0x060034B2 RID: 13490 RVA: 0x000DFBA2 File Offset: 0x000DEBA2
			internal MimePartAsyncResult(MimeBasePart part, object state, AsyncCallback callback)
				: base(part, state, callback)
			{
			}
		}
	}
}
