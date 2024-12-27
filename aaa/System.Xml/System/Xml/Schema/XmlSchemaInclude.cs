using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000265 RID: 613
	public class XmlSchemaInclude : XmlSchemaExternal
	{
		// Token: 0x06001C85 RID: 7301 RVA: 0x000832EF File Offset: 0x000822EF
		public XmlSchemaInclude()
		{
			base.Compositor = Compositor.Include;
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06001C86 RID: 7302 RVA: 0x000832FE File Offset: 0x000822FE
		// (set) Token: 0x06001C87 RID: 7303 RVA: 0x00083306 File Offset: 0x00082306
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

		// Token: 0x06001C88 RID: 7304 RVA: 0x0008330F File Offset: 0x0008230F
		internal override void AddAnnotation(XmlSchemaAnnotation annotation)
		{
			this.annotation = annotation;
		}

		// Token: 0x04001194 RID: 4500
		private XmlSchemaAnnotation annotation;
	}
}
