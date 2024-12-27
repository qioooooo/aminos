using System;
using System.Runtime.InteropServices;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200005F RID: 95
	public sealed class SoapClientMessage : SoapMessage
	{
		// Token: 0x06000252 RID: 594 RVA: 0x0000B347 File Offset: 0x0000A347
		internal SoapClientMessage(SoapHttpClientProtocol protocol, SoapClientMethod method, string url)
		{
			this.method = method;
			this.protocol = protocol;
			this.url = url;
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000B364 File Offset: 0x0000A364
		public override bool OneWay
		{
			get
			{
				return this.method.oneWay;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000B371 File Offset: 0x0000A371
		public SoapHttpClientProtocol Client
		{
			get
			{
				return this.protocol;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000B379 File Offset: 0x0000A379
		public override LogicalMethodInfo MethodInfo
		{
			get
			{
				return this.method.methodInfo;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000B386 File Offset: 0x0000A386
		public override string Url
		{
			get
			{
				return this.url;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000B38E File Offset: 0x0000A38E
		public override string Action
		{
			get
			{
				return this.method.action;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000B39B File Offset: 0x0000A39B
		[ComVisible(false)]
		public override SoapProtocolVersion SoapVersion
		{
			get
			{
				if (this.protocol.SoapVersion != SoapProtocolVersion.Default)
				{
					return this.protocol.SoapVersion;
				}
				return SoapProtocolVersion.Soap11;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000B3B7 File Offset: 0x0000A3B7
		internal SoapClientMethod Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000B3BF File Offset: 0x0000A3BF
		protected override void EnsureOutStage()
		{
			base.EnsureStage(SoapMessageStage.AfterDeserialize);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000B3C8 File Offset: 0x0000A3C8
		protected override void EnsureInStage()
		{
			base.EnsureStage(SoapMessageStage.BeforeSerialize);
		}

		// Token: 0x040002DC RID: 732
		private SoapClientMethod method;

		// Token: 0x040002DD RID: 733
		private SoapHttpClientProtocol protocol;

		// Token: 0x040002DE RID: 734
		private string url;

		// Token: 0x040002DF RID: 735
		internal SoapExtension[] initializedExtensions;
	}
}
