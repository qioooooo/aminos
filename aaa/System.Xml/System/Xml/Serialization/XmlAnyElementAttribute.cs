using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002FD RID: 765
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
	public class XmlAnyElementAttribute : Attribute
	{
		// Token: 0x060023D0 RID: 9168 RVA: 0x000AA24F File Offset: 0x000A924F
		public XmlAnyElementAttribute()
		{
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x000AA25E File Offset: 0x000A925E
		public XmlAnyElementAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x000AA274 File Offset: 0x000A9274
		public XmlAnyElementAttribute(string name, string ns)
		{
			this.name = name;
			this.ns = ns;
			this.nsSpecified = true;
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x060023D3 RID: 9171 RVA: 0x000AA298 File Offset: 0x000A9298
		// (set) Token: 0x060023D4 RID: 9172 RVA: 0x000AA2AE File Offset: 0x000A92AE
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

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x060023D5 RID: 9173 RVA: 0x000AA2B7 File Offset: 0x000A92B7
		// (set) Token: 0x060023D6 RID: 9174 RVA: 0x000AA2BF File Offset: 0x000A92BF
		public string Namespace
		{
			get
			{
				return this.ns;
			}
			set
			{
				this.ns = value;
				this.nsSpecified = true;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x060023D7 RID: 9175 RVA: 0x000AA2CF File Offset: 0x000A92CF
		// (set) Token: 0x060023D8 RID: 9176 RVA: 0x000AA2D7 File Offset: 0x000A92D7
		public int Order
		{
			get
			{
				return this.order;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("XmlDisallowNegativeValues"), "Order");
				}
				this.order = value;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x060023D9 RID: 9177 RVA: 0x000AA2F9 File Offset: 0x000A92F9
		internal bool NamespaceSpecified
		{
			get
			{
				return this.nsSpecified;
			}
		}

		// Token: 0x0400153C RID: 5436
		private string name;

		// Token: 0x0400153D RID: 5437
		private string ns;

		// Token: 0x0400153E RID: 5438
		private int order = -1;

		// Token: 0x0400153F RID: 5439
		private bool nsSpecified;
	}
}
