using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200005C RID: 92
	public class DsmlDirectoryIdentifier : DirectoryIdentifier
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x00007BC4 File Offset: 0x00006BC4
		public DsmlDirectoryIdentifier(Uri serverUri)
		{
			if (serverUri == null)
			{
				throw new ArgumentNullException("serverUri");
			}
			if (string.Compare(serverUri.Scheme, "http", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(serverUri.Scheme, "https", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(Res.GetString("DsmlNonHttpUri"));
			}
			this.uri = serverUri;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00007C28 File Offset: 0x00006C28
		public Uri ServerUri
		{
			get
			{
				return this.uri;
			}
		}

		// Token: 0x040001DE RID: 478
		private Uri uri;
	}
}
