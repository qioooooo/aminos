using System;

namespace System.Web.Services
{
	// Token: 0x02000015 RID: 21
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class WebServiceAttribute : Attribute
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002A72 File Offset: 0x00001A72
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002A88 File Offset: 0x00001A88
		public string Description
		{
			get
			{
				if (this.description != null)
				{
					return this.description;
				}
				return string.Empty;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002A91 File Offset: 0x00001A91
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002A99 File Offset: 0x00001A99
		public string Namespace
		{
			get
			{
				return this.ns;
			}
			set
			{
				this.ns = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002AA2 File Offset: 0x00001AA2
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002AB8 File Offset: 0x00001AB8
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x04000219 RID: 537
		public const string DefaultNamespace = "http://tempuri.org/";

		// Token: 0x0400021A RID: 538
		private string description;

		// Token: 0x0400021B RID: 539
		private string ns = "http://tempuri.org/";

		// Token: 0x0400021C RID: 540
		private string name;
	}
}
