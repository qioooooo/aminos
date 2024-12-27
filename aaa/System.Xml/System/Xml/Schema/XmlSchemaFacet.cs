using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200024E RID: 590
	public abstract class XmlSchemaFacet : XmlSchemaAnnotated
	{
		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x00082FA9 File Offset: 0x00081FA9
		// (set) Token: 0x06001C41 RID: 7233 RVA: 0x00082FB1 File Offset: 0x00081FB1
		[XmlAttribute("value")]
		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x00082FBA File Offset: 0x00081FBA
		// (set) Token: 0x06001C43 RID: 7235 RVA: 0x00082FC2 File Offset: 0x00081FC2
		[DefaultValue(false)]
		[XmlAttribute("fixed")]
		public virtual bool IsFixed
		{
			get
			{
				return this.isFixed;
			}
			set
			{
				if (!(this is XmlSchemaEnumerationFacet) && !(this is XmlSchemaPatternFacet))
				{
					this.isFixed = value;
				}
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x00082FDB File Offset: 0x00081FDB
		// (set) Token: 0x06001C45 RID: 7237 RVA: 0x00082FE3 File Offset: 0x00081FE3
		internal FacetType FacetType
		{
			get
			{
				return this.facetType;
			}
			set
			{
				this.facetType = value;
			}
		}

		// Token: 0x0400117B RID: 4475
		private string value;

		// Token: 0x0400117C RID: 4476
		private bool isFixed;

		// Token: 0x0400117D RID: 4477
		private FacetType facetType;
	}
}
