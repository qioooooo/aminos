using System;
using System.Web.Services.Description;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000064 RID: 100
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class SoapDocumentMethodAttribute : Attribute
	{
		// Token: 0x0600027C RID: 636 RVA: 0x0000CE80 File Offset: 0x0000BE80
		public SoapDocumentMethodAttribute()
		{
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000CE88 File Offset: 0x0000BE88
		public SoapDocumentMethodAttribute(string action)
		{
			this.action = action;
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000CE97 File Offset: 0x0000BE97
		// (set) Token: 0x0600027F RID: 639 RVA: 0x0000CE9F File Offset: 0x0000BE9F
		public string Action
		{
			get
			{
				return this.action;
			}
			set
			{
				this.action = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000CEA8 File Offset: 0x0000BEA8
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000CEB0 File Offset: 0x0000BEB0
		public bool OneWay
		{
			get
			{
				return this.oneWay;
			}
			set
			{
				this.oneWay = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000CEB9 File Offset: 0x0000BEB9
		// (set) Token: 0x06000283 RID: 643 RVA: 0x0000CEC1 File Offset: 0x0000BEC1
		public string RequestNamespace
		{
			get
			{
				return this.requestNamespace;
			}
			set
			{
				this.requestNamespace = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000CECA File Offset: 0x0000BECA
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000CED2 File Offset: 0x0000BED2
		public string ResponseNamespace
		{
			get
			{
				return this.responseNamespace;
			}
			set
			{
				this.responseNamespace = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000CEDB File Offset: 0x0000BEDB
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000CEF1 File Offset: 0x0000BEF1
		public string RequestElementName
		{
			get
			{
				if (this.requestName != null)
				{
					return this.requestName;
				}
				return string.Empty;
			}
			set
			{
				this.requestName = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000CEFA File Offset: 0x0000BEFA
		// (set) Token: 0x06000289 RID: 649 RVA: 0x0000CF10 File Offset: 0x0000BF10
		public string ResponseElementName
		{
			get
			{
				if (this.responseName != null)
				{
					return this.responseName;
				}
				return string.Empty;
			}
			set
			{
				this.responseName = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000CF19 File Offset: 0x0000BF19
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000CF21 File Offset: 0x0000BF21
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

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000CF2A File Offset: 0x0000BF2A
		// (set) Token: 0x0600028D RID: 653 RVA: 0x0000CF32 File Offset: 0x0000BF32
		public SoapParameterStyle ParameterStyle
		{
			get
			{
				return this.style;
			}
			set
			{
				this.style = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000CF3B File Offset: 0x0000BF3B
		// (set) Token: 0x0600028F RID: 655 RVA: 0x0000CF51 File Offset: 0x0000BF51
		public string Binding
		{
			get
			{
				if (this.binding != null)
				{
					return this.binding;
				}
				return string.Empty;
			}
			set
			{
				this.binding = value;
			}
		}

		// Token: 0x040002FB RID: 763
		private string action;

		// Token: 0x040002FC RID: 764
		private string requestName;

		// Token: 0x040002FD RID: 765
		private string responseName;

		// Token: 0x040002FE RID: 766
		private string requestNamespace;

		// Token: 0x040002FF RID: 767
		private string responseNamespace;

		// Token: 0x04000300 RID: 768
		private bool oneWay;

		// Token: 0x04000301 RID: 769
		private SoapBindingUse use;

		// Token: 0x04000302 RID: 770
		private SoapParameterStyle style;

		// Token: 0x04000303 RID: 771
		private string binding;
	}
}
