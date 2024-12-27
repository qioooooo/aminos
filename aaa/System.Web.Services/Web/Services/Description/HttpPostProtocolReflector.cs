using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000C3 RID: 195
	internal class HttpPostProtocolReflector : HttpProtocolReflector
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0001B097 File Offset: 0x0001A097
		public override string ProtocolName
		{
			get
			{
				return "HttpPost";
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001B0A0 File Offset: 0x0001A0A0
		protected override void BeginClass()
		{
			if (base.IsEmptyBinding)
			{
				return;
			}
			HttpBinding httpBinding = new HttpBinding();
			httpBinding.Verb = "POST";
			base.Binding.Extensions.Add(httpBinding);
			HttpAddressBinding httpAddressBinding = new HttpAddressBinding();
			httpAddressBinding.Location = base.ServiceUrl;
			base.Port.Extensions.Add(httpAddressBinding);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001B100 File Offset: 0x0001A100
		protected override bool ReflectMethod()
		{
			if (!base.ReflectMimeParameters())
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
