using System;
using System.Runtime.InteropServices;
using System.Web.Services.Description;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200007E RID: 126
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SoapRpcServiceAttribute : Attribute
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0000F8FE File Offset: 0x0000E8FE
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0000F906 File Offset: 0x0000E906
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

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000F90F File Offset: 0x0000E90F
		// (set) Token: 0x06000353 RID: 851 RVA: 0x0000F917 File Offset: 0x0000E917
		[ComVisible(false)]
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

		// Token: 0x0400036E RID: 878
		private SoapServiceRoutingStyle routingStyle;

		// Token: 0x0400036F RID: 879
		private SoapBindingUse use = SoapBindingUse.Encoded;
	}
}
