using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000697 RID: 1687
	public class LinkedResource : AttachmentBase
	{
		// Token: 0x06003403 RID: 13315 RVA: 0x000DB7A8 File Offset: 0x000DA7A8
		internal LinkedResource()
		{
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x000DB7B0 File Offset: 0x000DA7B0
		public LinkedResource(string fileName)
			: base(fileName)
		{
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x000DB7B9 File Offset: 0x000DA7B9
		public LinkedResource(string fileName, string mediaType)
			: base(fileName, mediaType)
		{
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x000DB7C3 File Offset: 0x000DA7C3
		public LinkedResource(string fileName, ContentType contentType)
			: base(fileName, contentType)
		{
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x000DB7CD File Offset: 0x000DA7CD
		public LinkedResource(Stream contentStream)
			: base(contentStream)
		{
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x000DB7D6 File Offset: 0x000DA7D6
		public LinkedResource(Stream contentStream, string mediaType)
			: base(contentStream, mediaType)
		{
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x000DB7E0 File Offset: 0x000DA7E0
		public LinkedResource(Stream contentStream, ContentType contentType)
			: base(contentStream, contentType)
		{
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x0600340A RID: 13322 RVA: 0x000DB7EA File Offset: 0x000DA7EA
		// (set) Token: 0x0600340B RID: 13323 RVA: 0x000DB7F2 File Offset: 0x000DA7F2
		public Uri ContentLink
		{
			get
			{
				return base.ContentLocation;
			}
			set
			{
				base.ContentLocation = value;
			}
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x000DB7FC File Offset: 0x000DA7FC
		public static LinkedResource CreateLinkedResourceFromString(string content)
		{
			LinkedResource linkedResource = new LinkedResource();
			linkedResource.SetContentFromString(content, null, string.Empty);
			return linkedResource;
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x000DB820 File Offset: 0x000DA820
		public static LinkedResource CreateLinkedResourceFromString(string content, Encoding contentEncoding, string mediaType)
		{
			LinkedResource linkedResource = new LinkedResource();
			linkedResource.SetContentFromString(content, contentEncoding, mediaType);
			return linkedResource;
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x000DB840 File Offset: 0x000DA840
		public static LinkedResource CreateLinkedResourceFromString(string content, ContentType contentType)
		{
			LinkedResource linkedResource = new LinkedResource();
			linkedResource.SetContentFromString(content, contentType);
			return linkedResource;
		}
	}
}
