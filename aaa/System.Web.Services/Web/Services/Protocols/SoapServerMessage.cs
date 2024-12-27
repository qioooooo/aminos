using System;
using System.Runtime.InteropServices;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200007F RID: 127
	public sealed class SoapServerMessage : SoapMessage
	{
		// Token: 0x06000354 RID: 852 RVA: 0x0000F920 File Offset: 0x0000E920
		internal SoapServerMessage(SoapServerProtocol protocol)
		{
			this.protocol = protocol;
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000F92F File Offset: 0x0000E92F
		public override bool OneWay
		{
			get
			{
				return this.protocol.ServerMethod.oneWay;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000F941 File Offset: 0x0000E941
		public override string Url
		{
			get
			{
				return this.protocol.Request.Url.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped).Replace("#", "%23");
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000F96A File Offset: 0x0000E96A
		public override string Action
		{
			get
			{
				return this.protocol.ServerMethod.action;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000F97C File Offset: 0x0000E97C
		[ComVisible(false)]
		public override SoapProtocolVersion SoapVersion
		{
			get
			{
				return this.protocol.Version;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000F989 File Offset: 0x0000E989
		public object Server
		{
			get
			{
				base.EnsureStage((SoapMessageStage)9);
				return this.protocol.Target;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0000F99E File Offset: 0x0000E99E
		public override LogicalMethodInfo MethodInfo
		{
			get
			{
				return this.protocol.MethodInfo;
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000F9AB File Offset: 0x0000E9AB
		protected override void EnsureOutStage()
		{
			base.EnsureStage(SoapMessageStage.BeforeSerialize);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000F9B4 File Offset: 0x0000E9B4
		protected override void EnsureInStage()
		{
			base.EnsureStage(SoapMessageStage.AfterDeserialize);
		}

		// Token: 0x04000370 RID: 880
		private SoapServerProtocol protocol;

		// Token: 0x04000371 RID: 881
		internal SoapExtension[] highPriConfigExtensions;

		// Token: 0x04000372 RID: 882
		internal SoapExtension[] otherExtensions;

		// Token: 0x04000373 RID: 883
		internal SoapExtension[] allExtensions;
	}
}
