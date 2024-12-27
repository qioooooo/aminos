using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200025E RID: 606
	public class XmlSchemaGroupRef : XmlSchemaParticle
	{
		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06001C64 RID: 7268 RVA: 0x00083187 File Offset: 0x00082187
		// (set) Token: 0x06001C65 RID: 7269 RVA: 0x0008318F File Offset: 0x0008218F
		[XmlAttribute("ref")]
		public XmlQualifiedName RefName
		{
			get
			{
				return this.refName;
			}
			set
			{
				this.refName = ((value == null) ? XmlQualifiedName.Empty : value);
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06001C66 RID: 7270 RVA: 0x000831A8 File Offset: 0x000821A8
		[XmlIgnore]
		public XmlSchemaGroupBase Particle
		{
			get
			{
				return this.particle;
			}
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000831B0 File Offset: 0x000821B0
		internal void SetParticle(XmlSchemaGroupBase value)
		{
			this.particle = value;
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06001C68 RID: 7272 RVA: 0x000831B9 File Offset: 0x000821B9
		// (set) Token: 0x06001C69 RID: 7273 RVA: 0x000831C1 File Offset: 0x000821C1
		[XmlIgnore]
		internal XmlSchemaGroup Redefined
		{
			get
			{
				return this.refined;
			}
			set
			{
				this.refined = value;
			}
		}

		// Token: 0x04001188 RID: 4488
		private XmlQualifiedName refName = XmlQualifiedName.Empty;

		// Token: 0x04001189 RID: 4489
		private XmlSchemaGroupBase particle;

		// Token: 0x0400118A RID: 4490
		private XmlSchemaGroup refined;
	}
}
