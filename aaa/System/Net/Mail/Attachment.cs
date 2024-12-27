using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000679 RID: 1657
	public class Attachment : AttachmentBase
	{
		// Token: 0x06003331 RID: 13105 RVA: 0x000D8581 File Offset: 0x000D7581
		internal Attachment()
		{
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x000D8589 File Offset: 0x000D7589
		public Attachment(string fileName)
			: base(fileName)
		{
			this.Name = AttachmentBase.ShortNameFromFile(fileName);
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x000D859E File Offset: 0x000D759E
		public Attachment(string fileName, string mediaType)
			: base(fileName, mediaType)
		{
			this.Name = AttachmentBase.ShortNameFromFile(fileName);
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x000D85B4 File Offset: 0x000D75B4
		public Attachment(string fileName, ContentType contentType)
			: base(fileName, contentType)
		{
			if (contentType.Name == null || contentType.Name == string.Empty)
			{
				this.Name = AttachmentBase.ShortNameFromFile(fileName);
				return;
			}
			this.Name = contentType.Name;
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x000D85F1 File Offset: 0x000D75F1
		public Attachment(Stream contentStream, string name)
			: base(contentStream, null, null)
		{
			this.Name = name;
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x000D8603 File Offset: 0x000D7603
		public Attachment(Stream contentStream, string name, string mediaType)
			: base(contentStream, null, mediaType)
		{
			this.Name = name;
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x000D8615 File Offset: 0x000D7615
		public Attachment(Stream contentStream, ContentType contentType)
			: base(contentStream, contentType)
		{
			this.Name = contentType.Name;
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x000D862C File Offset: 0x000D762C
		internal void SetContentTypeName()
		{
			if (this.name != null && this.name.Length != 0 && !MimeBasePart.IsAscii(this.name, false))
			{
				Encoding encoding = this.NameEncoding;
				if (encoding == null)
				{
					encoding = Encoding.GetEncoding("utf-8");
				}
				base.MimePart.ContentType.Name = MimeBasePart.EncodeHeaderValue(this.name, encoding, MimeBasePart.ShouldUseBase64Encoding(encoding));
				return;
			}
			base.MimePart.ContentType.Name = this.name;
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06003339 RID: 13113 RVA: 0x000D86AA File Offset: 0x000D76AA
		// (set) Token: 0x0600333A RID: 13114 RVA: 0x000D86B4 File Offset: 0x000D76B4
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				Encoding encoding = MimeBasePart.DecodeEncoding(value);
				if (encoding != null)
				{
					this.nameEncoding = encoding;
					this.name = MimeBasePart.DecodeHeaderValue(value);
					base.MimePart.ContentType.Name = value;
					return;
				}
				this.name = value;
				this.SetContentTypeName();
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x0600333B RID: 13115 RVA: 0x000D86FD File Offset: 0x000D76FD
		// (set) Token: 0x0600333C RID: 13116 RVA: 0x000D8705 File Offset: 0x000D7705
		public Encoding NameEncoding
		{
			get
			{
				return this.nameEncoding;
			}
			set
			{
				this.nameEncoding = value;
				if (this.name != null && this.name != string.Empty)
				{
					this.SetContentTypeName();
				}
			}
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x0600333D RID: 13117 RVA: 0x000D8730 File Offset: 0x000D7730
		public ContentDisposition ContentDisposition
		{
			get
			{
				ContentDisposition contentDisposition = base.MimePart.ContentDisposition;
				if (contentDisposition == null)
				{
					contentDisposition = new ContentDisposition();
					base.MimePart.ContentDisposition = contentDisposition;
				}
				return contentDisposition;
			}
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x000D875F File Offset: 0x000D775F
		internal override void PrepareForSending()
		{
			if (this.name != null && this.name != string.Empty)
			{
				this.SetContentTypeName();
			}
			base.PrepareForSending();
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x000D8788 File Offset: 0x000D7788
		public static Attachment CreateAttachmentFromString(string content, string name)
		{
			Attachment attachment = new Attachment();
			attachment.SetContentFromString(content, null, string.Empty);
			attachment.Name = name;
			return attachment;
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x000D87B0 File Offset: 0x000D77B0
		public static Attachment CreateAttachmentFromString(string content, string name, Encoding contentEncoding, string mediaType)
		{
			Attachment attachment = new Attachment();
			attachment.SetContentFromString(content, contentEncoding, mediaType);
			attachment.Name = name;
			return attachment;
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x000D87D4 File Offset: 0x000D77D4
		public static Attachment CreateAttachmentFromString(string content, ContentType contentType)
		{
			Attachment attachment = new Attachment();
			attachment.SetContentFromString(content, contentType);
			attachment.Name = contentType.Name;
			return attachment;
		}

		// Token: 0x04002F81 RID: 12161
		private string name;

		// Token: 0x04002F82 RID: 12162
		private Encoding nameEncoding;
	}
}
