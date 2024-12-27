using System;
using System.Web.Services.Protocols;

namespace System.Web.Services.Description
{
	// Token: 0x020000C2 RID: 194
	internal class HttpPostProtocolImporter : HttpProtocolImporter
	{
		// Token: 0x0600054B RID: 1355 RVA: 0x0001B01E File Offset: 0x0001A01E
		public HttpPostProtocolImporter()
			: base(true)
		{
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0001B027 File Offset: 0x0001A027
		public override string ProtocolName
		{
			get
			{
				return "HttpPost";
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0001B02E File Offset: 0x0001A02E
		internal override Type BaseClass
		{
			get
			{
				if (base.Style == ServiceDescriptionImportStyle.Client)
				{
					return typeof(HttpPostClientProtocol);
				}
				return typeof(WebService);
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001B050 File Offset: 0x0001A050
		protected override bool IsBindingSupported()
		{
			HttpBinding httpBinding = (HttpBinding)base.Binding.Extensions.Find(typeof(HttpBinding));
			return httpBinding != null && !(httpBinding.Verb != "POST");
		}
	}
}
