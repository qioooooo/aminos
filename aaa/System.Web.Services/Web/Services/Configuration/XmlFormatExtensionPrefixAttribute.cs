using System;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000146 RID: 326
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class XmlFormatExtensionPrefixAttribute : Attribute
	{
		// Token: 0x06000A39 RID: 2617 RVA: 0x00047D29 File Offset: 0x00046D29
		public XmlFormatExtensionPrefixAttribute()
		{
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00047D31 File Offset: 0x00046D31
		public XmlFormatExtensionPrefixAttribute(string prefix, string ns)
		{
			this.prefix = prefix;
			this.ns = ns;
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00047D47 File Offset: 0x00046D47
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x00047D5D File Offset: 0x00046D5D
		public string Prefix
		{
			get
			{
				if (this.prefix != null)
				{
					return this.prefix;
				}
				return string.Empty;
			}
			set
			{
				this.prefix = value;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x00047D66 File Offset: 0x00046D66
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x00047D7C File Offset: 0x00046D7C
		public string Namespace
		{
			get
			{
				if (this.ns != null)
				{
					return this.ns;
				}
				return string.Empty;
			}
			set
			{
				this.ns = value;
			}
		}

		// Token: 0x04000649 RID: 1609
		private string prefix;

		// Token: 0x0400064A RID: 1610
		private string ns;
	}
}
