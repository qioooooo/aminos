using System;
using System.Web.Services.Protocols;

namespace System.Web.Services.Description
{
	// Token: 0x020000BD RID: 189
	internal class HttpGetProtocolImporter : HttpProtocolImporter
	{
		// Token: 0x06000517 RID: 1303 RVA: 0x0001A010 File Offset: 0x00019010
		public HttpGetProtocolImporter()
			: base(false)
		{
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0001A019 File Offset: 0x00019019
		public override string ProtocolName
		{
			get
			{
				return "HttpGet";
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0001A020 File Offset: 0x00019020
		internal override Type BaseClass
		{
			get
			{
				if (base.Style == ServiceDescriptionImportStyle.Client)
				{
					return typeof(HttpGetClientProtocol);
				}
				return typeof(WebService);
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001A040 File Offset: 0x00019040
		protected override bool IsBindingSupported()
		{
			HttpBinding httpBinding = (HttpBinding)base.Binding.Extensions.Find(typeof(HttpBinding));
			return httpBinding != null && !(httpBinding.Verb != "GET");
		}
	}
}
