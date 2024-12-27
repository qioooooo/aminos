using System;
using System.Runtime.InteropServices;
using System.Web.Services.Description;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200007D RID: 125
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class SoapRpcMethodAttribute : Attribute
	{
		// Token: 0x0600033D RID: 829 RVA: 0x0000F818 File Offset: 0x0000E818
		public SoapRpcMethodAttribute()
		{
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000F827 File Offset: 0x0000E827
		public SoapRpcMethodAttribute(string action)
		{
			this.action = action;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000F83D File Offset: 0x0000E83D
		// (set) Token: 0x06000340 RID: 832 RVA: 0x0000F845 File Offset: 0x0000E845
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

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000F84E File Offset: 0x0000E84E
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0000F864 File Offset: 0x0000E864
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

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000F86D File Offset: 0x0000E86D
		// (set) Token: 0x06000344 RID: 836 RVA: 0x0000F875 File Offset: 0x0000E875
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

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000F87E File Offset: 0x0000E87E
		// (set) Token: 0x06000346 RID: 838 RVA: 0x0000F886 File Offset: 0x0000E886
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0000F88F File Offset: 0x0000E88F
		// (set) Token: 0x06000348 RID: 840 RVA: 0x0000F897 File Offset: 0x0000E897
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

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000F8A0 File Offset: 0x0000E8A0
		// (set) Token: 0x0600034A RID: 842 RVA: 0x0000F8B6 File Offset: 0x0000E8B6
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

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000F8BF File Offset: 0x0000E8BF
		// (set) Token: 0x0600034C RID: 844 RVA: 0x0000F8D5 File Offset: 0x0000E8D5
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

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000F8DE File Offset: 0x0000E8DE
		// (set) Token: 0x0600034E RID: 846 RVA: 0x0000F8E6 File Offset: 0x0000E8E6
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

		// Token: 0x04000366 RID: 870
		private string action;

		// Token: 0x04000367 RID: 871
		private string requestName;

		// Token: 0x04000368 RID: 872
		private string responseName;

		// Token: 0x04000369 RID: 873
		private string requestNamespace;

		// Token: 0x0400036A RID: 874
		private string responseNamespace;

		// Token: 0x0400036B RID: 875
		private bool oneWay;

		// Token: 0x0400036C RID: 876
		private string binding;

		// Token: 0x0400036D RID: 877
		private SoapBindingUse use = SoapBindingUse.Encoded;
	}
}
