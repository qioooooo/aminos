using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200025D RID: 605
	public class XmlSchemaGroup : XmlSchemaAnnotated
	{
		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x000830B4 File Offset: 0x000820B4
		// (set) Token: 0x06001C55 RID: 7253 RVA: 0x000830BC File Offset: 0x000820BC
		[XmlAttribute("name")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06001C56 RID: 7254 RVA: 0x000830C5 File Offset: 0x000820C5
		// (set) Token: 0x06001C57 RID: 7255 RVA: 0x000830CD File Offset: 0x000820CD
		[XmlElement("choice", typeof(XmlSchemaChoice))]
		[XmlElement("all", typeof(XmlSchemaAll))]
		[XmlElement("sequence", typeof(XmlSchemaSequence))]
		public XmlSchemaGroupBase Particle
		{
			get
			{
				return this.particle;
			}
			set
			{
				this.particle = value;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001C58 RID: 7256 RVA: 0x000830D6 File Offset: 0x000820D6
		[XmlIgnore]
		public XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qname;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001C59 RID: 7257 RVA: 0x000830DE File Offset: 0x000820DE
		// (set) Token: 0x06001C5A RID: 7258 RVA: 0x000830E6 File Offset: 0x000820E6
		[XmlIgnore]
		internal XmlSchemaParticle CanonicalParticle
		{
			get
			{
				return this.canonicalParticle;
			}
			set
			{
				this.canonicalParticle = value;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001C5B RID: 7259 RVA: 0x000830EF File Offset: 0x000820EF
		// (set) Token: 0x06001C5C RID: 7260 RVA: 0x000830F7 File Offset: 0x000820F7
		[XmlIgnore]
		internal XmlSchemaGroup Redefined
		{
			get
			{
				return this.redefined;
			}
			set
			{
				this.redefined = value;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06001C5D RID: 7261 RVA: 0x00083100 File Offset: 0x00082100
		// (set) Token: 0x06001C5E RID: 7262 RVA: 0x00083108 File Offset: 0x00082108
		[XmlIgnore]
		internal int SelfReferenceCount
		{
			get
			{
				return this.selfReferenceCount;
			}
			set
			{
				this.selfReferenceCount = value;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06001C5F RID: 7263 RVA: 0x00083111 File Offset: 0x00082111
		// (set) Token: 0x06001C60 RID: 7264 RVA: 0x00083119 File Offset: 0x00082119
		[XmlIgnore]
		internal override string NameAttribute
		{
			get
			{
				return this.Name;
			}
			set
			{
				this.Name = value;
			}
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x00083122 File Offset: 0x00082122
		internal void SetQualifiedName(XmlQualifiedName value)
		{
			this.qname = value;
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x0008312C File Offset: 0x0008212C
		internal override XmlSchemaObject Clone()
		{
			XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)base.MemberwiseClone();
			if (XmlSchemaComplexType.HasParticleRef(this.particle))
			{
				xmlSchemaGroup.particle = XmlSchemaComplexType.CloneParticle(this.particle) as XmlSchemaGroupBase;
			}
			xmlSchemaGroup.canonicalParticle = XmlSchemaParticle.Empty;
			return xmlSchemaGroup;
		}

		// Token: 0x04001182 RID: 4482
		private string name;

		// Token: 0x04001183 RID: 4483
		private XmlSchemaGroupBase particle;

		// Token: 0x04001184 RID: 4484
		private XmlSchemaParticle canonicalParticle;

		// Token: 0x04001185 RID: 4485
		private XmlQualifiedName qname = XmlQualifiedName.Empty;

		// Token: 0x04001186 RID: 4486
		private XmlSchemaGroup redefined;

		// Token: 0x04001187 RID: 4487
		private int selfReferenceCount;
	}
}
