using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000264 RID: 612
	public class XmlSchemaImport : XmlSchemaExternal
	{
		// Token: 0x06001C7F RID: 7295 RVA: 0x000832B5 File Offset: 0x000822B5
		public XmlSchemaImport()
		{
			base.Compositor = Compositor.Import;
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x000832C4 File Offset: 0x000822C4
		// (set) Token: 0x06001C81 RID: 7297 RVA: 0x000832CC File Offset: 0x000822CC
		[XmlAttribute("namespace", DataType = "anyURI")]
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

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x000832D5 File Offset: 0x000822D5
		// (set) Token: 0x06001C83 RID: 7299 RVA: 0x000832DD File Offset: 0x000822DD
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

		// Token: 0x06001C84 RID: 7300 RVA: 0x000832E6 File Offset: 0x000822E6
		internal override void AddAnnotation(XmlSchemaAnnotation annotation)
		{
			this.annotation = annotation;
		}

		// Token: 0x04001192 RID: 4498
		private string ns;

		// Token: 0x04001193 RID: 4499
		private XmlSchemaAnnotation annotation;
	}
}
