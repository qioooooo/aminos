using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000240 RID: 576
	public class XmlSchemaComplexContent : XmlSchemaContentModel
	{
		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06001B75 RID: 7029 RVA: 0x00081A2F File Offset: 0x00080A2F
		// (set) Token: 0x06001B76 RID: 7030 RVA: 0x00081A37 File Offset: 0x00080A37
		[XmlAttribute("mixed")]
		public bool IsMixed
		{
			get
			{
				return this.isMixed;
			}
			set
			{
				this.isMixed = value;
				this.hasMixedAttribute = true;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06001B77 RID: 7031 RVA: 0x00081A47 File Offset: 0x00080A47
		// (set) Token: 0x06001B78 RID: 7032 RVA: 0x00081A4F File Offset: 0x00080A4F
		[XmlElement("extension", typeof(XmlSchemaComplexContentExtension))]
		[XmlElement("restriction", typeof(XmlSchemaComplexContentRestriction))]
		public override XmlSchemaContent Content
		{
			get
			{
				return this.content;
			}
			set
			{
				this.content = value;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06001B79 RID: 7033 RVA: 0x00081A58 File Offset: 0x00080A58
		[XmlIgnore]
		internal bool HasMixedAttribute
		{
			get
			{
				return this.hasMixedAttribute;
			}
		}

		// Token: 0x0400110D RID: 4365
		private XmlSchemaContent content;

		// Token: 0x0400110E RID: 4366
		private bool isMixed;

		// Token: 0x0400110F RID: 4367
		private bool hasMixedAttribute;
	}
}
