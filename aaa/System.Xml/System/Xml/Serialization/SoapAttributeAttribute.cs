using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002E6 RID: 742
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public class SoapAttributeAttribute : Attribute
	{
		// Token: 0x060022C2 RID: 8898 RVA: 0x000A3A43 File Offset: 0x000A2A43
		public SoapAttributeAttribute()
		{
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x000A3A4B File Offset: 0x000A2A4B
		public SoapAttributeAttribute(string attributeName)
		{
			this.attributeName = attributeName;
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x060022C4 RID: 8900 RVA: 0x000A3A5A File Offset: 0x000A2A5A
		// (set) Token: 0x060022C5 RID: 8901 RVA: 0x000A3A70 File Offset: 0x000A2A70
		public string AttributeName
		{
			get
			{
				if (this.attributeName != null)
				{
					return this.attributeName;
				}
				return string.Empty;
			}
			set
			{
				this.attributeName = value;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x000A3A79 File Offset: 0x000A2A79
		// (set) Token: 0x060022C7 RID: 8903 RVA: 0x000A3A81 File Offset: 0x000A2A81
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

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x000A3A8A File Offset: 0x000A2A8A
		// (set) Token: 0x060022C9 RID: 8905 RVA: 0x000A3AA0 File Offset: 0x000A2AA0
		public string DataType
		{
			get
			{
				if (this.dataType != null)
				{
					return this.dataType;
				}
				return string.Empty;
			}
			set
			{
				this.dataType = value;
			}
		}

		// Token: 0x040014D0 RID: 5328
		private string attributeName;

		// Token: 0x040014D1 RID: 5329
		private string ns;

		// Token: 0x040014D2 RID: 5330
		private string dataType;
	}
}
