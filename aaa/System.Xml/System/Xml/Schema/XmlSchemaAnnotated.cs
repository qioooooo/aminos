using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200022E RID: 558
	public class XmlSchemaAnnotated : XmlSchemaObject
	{
		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x0008093C File Offset: 0x0007F93C
		// (set) Token: 0x06001AC4 RID: 6852 RVA: 0x00080944 File Offset: 0x0007F944
		[XmlAttribute("id", DataType = "ID")]
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x0008094D File Offset: 0x0007F94D
		// (set) Token: 0x06001AC6 RID: 6854 RVA: 0x00080955 File Offset: 0x0007F955
		[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
		public XmlSchemaAnnotation Annotation
		{
			get
			{
				return this.annotation;
			}
			set
			{
				this.annotation = value;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x0008095E File Offset: 0x0007F95E
		// (set) Token: 0x06001AC8 RID: 6856 RVA: 0x00080966 File Offset: 0x0007F966
		[XmlAnyAttribute]
		public XmlAttribute[] UnhandledAttributes
		{
			get
			{
				return this.moreAttributes;
			}
			set
			{
				this.moreAttributes = value;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06001AC9 RID: 6857 RVA: 0x0008096F File Offset: 0x0007F96F
		// (set) Token: 0x06001ACA RID: 6858 RVA: 0x00080977 File Offset: 0x0007F977
		[XmlIgnore]
		internal override string IdAttribute
		{
			get
			{
				return this.Id;
			}
			set
			{
				this.Id = value;
			}
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x00080980 File Offset: 0x0007F980
		internal override void SetUnhandledAttributes(XmlAttribute[] moreAttributes)
		{
			this.moreAttributes = moreAttributes;
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00080989 File Offset: 0x0007F989
		internal override void AddAnnotation(XmlSchemaAnnotation annotation)
		{
			this.annotation = annotation;
		}

		// Token: 0x040010D4 RID: 4308
		private string id;

		// Token: 0x040010D5 RID: 4309
		private XmlSchemaAnnotation annotation;

		// Token: 0x040010D6 RID: 4310
		private XmlAttribute[] moreAttributes;
	}
}
