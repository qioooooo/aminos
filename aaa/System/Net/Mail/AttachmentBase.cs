using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000676 RID: 1654
	public abstract class AttachmentBase : IDisposable
	{
		// Token: 0x06003303 RID: 13059 RVA: 0x000D7D87 File Offset: 0x000D6D87
		internal AttachmentBase()
		{
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x000D7D9A File Offset: 0x000D6D9A
		protected AttachmentBase(string fileName)
		{
			this.SetContentFromFile(fileName, string.Empty);
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x000D7DB9 File Offset: 0x000D6DB9
		protected AttachmentBase(string fileName, string mediaType)
		{
			this.SetContentFromFile(fileName, mediaType);
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x000D7DD4 File Offset: 0x000D6DD4
		protected AttachmentBase(string fileName, ContentType contentType)
		{
			this.SetContentFromFile(fileName, contentType);
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x000D7DEF File Offset: 0x000D6DEF
		protected AttachmentBase(Stream contentStream)
		{
			this.part.SetContent(contentStream);
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x000D7E0E File Offset: 0x000D6E0E
		protected AttachmentBase(Stream contentStream, string mediaType)
		{
			this.part.SetContent(contentStream, null, mediaType);
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x000D7E2F File Offset: 0x000D6E2F
		internal AttachmentBase(Stream contentStream, string name, string mediaType)
		{
			this.part.SetContent(contentStream, name, mediaType);
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x000D7E50 File Offset: 0x000D6E50
		protected AttachmentBase(Stream contentStream, ContentType contentType)
		{
			this.part.SetContent(contentStream, contentType);
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x000D7E70 File Offset: 0x000D6E70
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x000D7E79 File Offset: 0x000D6E79
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this.disposed)
			{
				this.disposed = true;
				this.part.Dispose();
			}
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x000D7E98 File Offset: 0x000D6E98
		internal static string ShortNameFromFile(string fileName)
		{
			int num = fileName.LastIndexOfAny(new char[] { '\\', ':' }, fileName.Length - 1, fileName.Length);
			string text;
			if (num > 0)
			{
				text = fileName.Substring(num + 1, fileName.Length - num - 1);
			}
			else
			{
				text = fileName;
			}
			return text;
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x000D7EEC File Offset: 0x000D6EEC
		internal void SetContentFromFile(string fileName, ContentType contentType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "fileName" }), "fileName");
			}
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.part.SetContent(stream, contentType);
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x000D7F50 File Offset: 0x000D6F50
		internal void SetContentFromFile(string fileName, string mediaType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "fileName" }), "fileName");
			}
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.part.SetContent(stream, null, mediaType);
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x000D7FB8 File Offset: 0x000D6FB8
		internal void SetContentFromString(string contentString, ContentType contentType)
		{
			if (contentString == null)
			{
				throw new ArgumentNullException("content");
			}
			if (this.part.Stream != null)
			{
				this.part.Stream.Close();
			}
			Encoding encoding;
			if (contentType != null && contentType.CharSet != null)
			{
				encoding = Encoding.GetEncoding(contentType.CharSet);
			}
			else if (MimeBasePart.IsAscii(contentString, false))
			{
				encoding = Encoding.ASCII;
			}
			else
			{
				encoding = Encoding.GetEncoding("utf-8");
			}
			byte[] bytes = encoding.GetBytes(contentString);
			this.part.SetContent(new MemoryStream(bytes), contentType);
			if (MimeBasePart.ShouldUseBase64Encoding(encoding))
			{
				this.part.TransferEncoding = TransferEncoding.Base64;
				return;
			}
			this.part.TransferEncoding = TransferEncoding.QuotedPrintable;
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x000D8060 File Offset: 0x000D7060
		internal void SetContentFromString(string contentString, Encoding encoding, string mediaType)
		{
			if (contentString == null)
			{
				throw new ArgumentNullException("content");
			}
			if (this.part.Stream != null)
			{
				this.part.Stream.Close();
			}
			if (mediaType == null || mediaType == string.Empty)
			{
				mediaType = "text/plain";
			}
			int num = 0;
			try
			{
				string text = MailBnfHelper.ReadToken(mediaType, ref num, null);
				if (text.Length == 0 || num >= mediaType.Length || mediaType[num++] != '/')
				{
					throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
				}
				text = MailBnfHelper.ReadToken(mediaType, ref num, null);
				if (text.Length == 0 || num < mediaType.Length)
				{
					throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
				}
			}
			catch (FormatException)
			{
				throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
			}
			ContentType contentType = new ContentType(mediaType);
			if (encoding == null)
			{
				if (MimeBasePart.IsAscii(contentString, false))
				{
					encoding = Encoding.ASCII;
				}
				else
				{
					encoding = Encoding.GetEncoding("utf-8");
				}
			}
			contentType.CharSet = encoding.BodyName;
			byte[] bytes = encoding.GetBytes(contentString);
			this.part.SetContent(new MemoryStream(bytes), contentType);
			if (MimeBasePart.ShouldUseBase64Encoding(encoding))
			{
				this.part.TransferEncoding = TransferEncoding.Base64;
				return;
			}
			this.part.TransferEncoding = TransferEncoding.QuotedPrintable;
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x000D81B8 File Offset: 0x000D71B8
		internal virtual void PrepareForSending()
		{
			this.part.ResetStream();
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06003313 RID: 13075 RVA: 0x000D81C5 File Offset: 0x000D71C5
		public Stream ContentStream
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				return this.part.Stream;
			}
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x000D81EC File Offset: 0x000D71EC
		// (set) Token: 0x06003315 RID: 13077 RVA: 0x000D8264 File Offset: 0x000D7264
		public string ContentId
		{
			get
			{
				string text = this.part.ContentID;
				if (string.IsNullOrEmpty(text))
				{
					text = Guid.NewGuid().ToString();
					this.ContentId = text;
					return text;
				}
				if (text.Length >= 2 && text[0] == '<' && text[text.Length - 1] == '>')
				{
					return text.Substring(1, text.Length - 2);
				}
				return text;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.part.ContentID = null;
					return;
				}
				if (value.IndexOfAny(new char[] { '<', '>' }) != -1)
				{
					throw new ArgumentException(SR.GetString("MailHeaderInvalidCID"), "value");
				}
				this.part.ContentID = "<" + value + ">";
			}
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06003316 RID: 13078 RVA: 0x000D82D1 File Offset: 0x000D72D1
		// (set) Token: 0x06003317 RID: 13079 RVA: 0x000D82DE File Offset: 0x000D72DE
		public ContentType ContentType
		{
			get
			{
				return this.part.ContentType;
			}
			set
			{
				this.part.ContentType = value;
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06003318 RID: 13080 RVA: 0x000D82EC File Offset: 0x000D72EC
		// (set) Token: 0x06003319 RID: 13081 RVA: 0x000D82F9 File Offset: 0x000D72F9
		public TransferEncoding TransferEncoding
		{
			get
			{
				return this.part.TransferEncoding;
			}
			set
			{
				this.part.TransferEncoding = value;
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x0600331A RID: 13082 RVA: 0x000D8308 File Offset: 0x000D7308
		// (set) Token: 0x0600331B RID: 13083 RVA: 0x000D832D File Offset: 0x000D732D
		internal Uri ContentLocation
		{
			get
			{
				Uri uri;
				if (!Uri.TryCreate(this.part.ContentLocation, UriKind.RelativeOrAbsolute, out uri))
				{
					return null;
				}
				return uri;
			}
			set
			{
				this.part.ContentLocation = ((value == null) ? null : (value.IsAbsoluteUri ? value.AbsoluteUri : value.OriginalString));
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x0600331C RID: 13084 RVA: 0x000D835C File Offset: 0x000D735C
		internal MimePart MimePart
		{
			get
			{
				return this.part;
			}
		}

		// Token: 0x04002F7D RID: 12157
		internal bool disposed;

		// Token: 0x04002F7E RID: 12158
		private MimePart part = new MimePart();
	}
}
