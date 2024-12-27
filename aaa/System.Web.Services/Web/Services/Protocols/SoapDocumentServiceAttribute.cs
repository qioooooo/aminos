using System;
using System.Web.Services.Description;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000065 RID: 101
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SoapDocumentServiceAttribute : Attribute
	{
		// Token: 0x06000290 RID: 656 RVA: 0x0000CF5A File Offset: 0x0000BF5A
		public SoapDocumentServiceAttribute()
		{
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000CF62 File Offset: 0x0000BF62
		public SoapDocumentServiceAttribute(SoapBindingUse use)
		{
			this.use = use;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000CF71 File Offset: 0x0000BF71
		public SoapDocumentServiceAttribute(SoapBindingUse use, SoapParameterStyle paramStyle)
		{
			this.use = use;
			this.paramStyle = paramStyle;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000CF87 File Offset: 0x0000BF87
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0000CF8F File Offset: 0x0000BF8F
		public SoapBindingUse Use
		{
			get
			{
				return this.use;
			}
			set
			{
				this.use = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000CF98 File Offset: 0x0000BF98
		// (set) Token: 0x06000296 RID: 662 RVA: 0x0000CFA0 File Offset: 0x0000BFA0
		public SoapParameterStyle ParameterStyle
		{
			get
			{
				return this.paramStyle;
			}
			set
			{
				this.paramStyle = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000CFA9 File Offset: 0x0000BFA9
		// (set) Token: 0x06000298 RID: 664 RVA: 0x0000CFB1 File Offset: 0x0000BFB1
		public SoapServiceRoutingStyle RoutingStyle
		{
			get
			{
				return this.routingStyle;
			}
			set
			{
				this.routingStyle = value;
			}
		}

		// Token: 0x04000304 RID: 772
		private SoapBindingUse use;

		// Token: 0x04000305 RID: 773
		private SoapParameterStyle paramStyle;

		// Token: 0x04000306 RID: 774
		private SoapServiceRoutingStyle routingStyle;
	}
}
