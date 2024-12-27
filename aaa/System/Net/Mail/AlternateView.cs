using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000677 RID: 1655
	public class AlternateView : AttachmentBase
	{
		// Token: 0x0600331D RID: 13085 RVA: 0x000D8364 File Offset: 0x000D7364
		internal AlternateView()
		{
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x000D836C File Offset: 0x000D736C
		public AlternateView(string fileName)
			: base(fileName)
		{
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x000D8375 File Offset: 0x000D7375
		public AlternateView(string fileName, string mediaType)
			: base(fileName, mediaType)
		{
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x000D837F File Offset: 0x000D737F
		public AlternateView(string fileName, ContentType contentType)
			: base(fileName, contentType)
		{
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x000D8389 File Offset: 0x000D7389
		public AlternateView(Stream contentStream)
			: base(contentStream)
		{
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x000D8392 File Offset: 0x000D7392
		public AlternateView(Stream contentStream, string mediaType)
			: base(contentStream, mediaType)
		{
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x000D839C File Offset: 0x000D739C
		public AlternateView(Stream contentStream, ContentType contentType)
			: base(contentStream, contentType)
		{
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06003324 RID: 13092 RVA: 0x000D83A6 File Offset: 0x000D73A6
		public LinkedResourceCollection LinkedResources
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.linkedResources == null)
				{
					this.linkedResources = new LinkedResourceCollection();
				}
				return this.linkedResources;
			}
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06003325 RID: 13093 RVA: 0x000D83DA File Offset: 0x000D73DA
		// (set) Token: 0x06003326 RID: 13094 RVA: 0x000D83E2 File Offset: 0x000D73E2
		public Uri BaseUri
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

		// Token: 0x06003327 RID: 13095 RVA: 0x000D83EC File Offset: 0x000D73EC
		public static AlternateView CreateAlternateViewFromString(string content)
		{
			AlternateView alternateView = new AlternateView();
			alternateView.SetContentFromString(content, null, string.Empty);
			return alternateView;
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x000D8410 File Offset: 0x000D7410
		public static AlternateView CreateAlternateViewFromString(string content, Encoding contentEncoding, string mediaType)
		{
			AlternateView alternateView = new AlternateView();
			alternateView.SetContentFromString(content, contentEncoding, mediaType);
			return alternateView;
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x000D8430 File Offset: 0x000D7430
		public static AlternateView CreateAlternateViewFromString(string content, ContentType contentType)
		{
			AlternateView alternateView = new AlternateView();
			alternateView.SetContentFromString(content, contentType);
			return alternateView;
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x000D844C File Offset: 0x000D744C
		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing && this.linkedResources != null)
			{
				this.linkedResources.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x04002F7F RID: 12159
		private LinkedResourceCollection linkedResources;
	}
}
