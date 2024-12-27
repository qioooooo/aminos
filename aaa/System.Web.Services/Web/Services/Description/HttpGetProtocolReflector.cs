using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000C1 RID: 193
	internal class HttpGetProtocolReflector : HttpProtocolReflector
	{
		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x0001AF67 File Offset: 0x00019F67
		public override string ProtocolName
		{
			get
			{
				return "HttpGet";
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001AF70 File Offset: 0x00019F70
		protected override void BeginClass()
		{
			if (base.IsEmptyBinding)
			{
				return;
			}
			HttpBinding httpBinding = new HttpBinding();
			httpBinding.Verb = "GET";
			base.Binding.Extensions.Add(httpBinding);
			HttpAddressBinding httpAddressBinding = new HttpAddressBinding();
			httpAddressBinding.Location = base.ServiceUrl;
			base.Port.Extensions.Add(httpAddressBinding);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001AFD0 File Offset: 0x00019FD0
		protected override bool ReflectMethod()
		{
			if (!base.ReflectUrlParameters())
			{
				return false;
			}
			if (!base.ReflectMimeReturn())
			{
				return false;
			}
			HttpOperationBinding httpOperationBinding = new HttpOperationBinding();
			httpOperationBinding.Location = base.MethodUrl;
			base.OperationBinding.Extensions.Add(httpOperationBinding);
			return true;
		}
	}
}
