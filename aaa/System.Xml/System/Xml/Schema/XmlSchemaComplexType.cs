using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000245 RID: 581
	public class XmlSchemaComplexType : XmlSchemaType
	{
		// Token: 0x06001BB2 RID: 7090 RVA: 0x00081E04 File Offset: 0x00080E04
		static XmlSchemaComplexType()
		{
			XmlSchemaComplexType.untypedAnyType.SetQualifiedName(new XmlQualifiedName("untypedAny", "http://www.w3.org/2003/11/xpath-datatypes"));
			XmlSchemaComplexType.untypedAnyType.IsMixed = true;
			XmlSchemaComplexType.untypedAnyType.SetContentTypeParticle(XmlSchemaComplexType.anyTypeLax.ContentTypeParticle);
			XmlSchemaComplexType.untypedAnyType.SetContentType(XmlSchemaContentType.Mixed);
			XmlSchemaComplexType.untypedAnyType.ElementDecl = SchemaElementDecl.CreateAnyTypeElementDecl();
			XmlSchemaComplexType.untypedAnyType.ElementDecl.SchemaType = XmlSchemaComplexType.untypedAnyType;
			XmlSchemaComplexType.untypedAnyType.ElementDecl.ContentValidator = XmlSchemaComplexType.AnyTypeContentValidator;
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x00081EAC File Offset: 0x00080EAC
		private static XmlSchemaComplexType CreateAnyType(XmlSchemaContentProcessing processContents)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			xmlSchemaComplexType.SetQualifiedName(DatatypeImplementation.QnAnyType);
			XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
			xmlSchemaAny.MinOccurs = 0m;
			xmlSchemaAny.MaxOccurs = decimal.MaxValue;
			xmlSchemaAny.ProcessContents = processContents;
			xmlSchemaAny.BuildNamespaceList(null);
			xmlSchemaComplexType.SetContentTypeParticle(new XmlSchemaSequence
			{
				Items = { xmlSchemaAny }
			});
			xmlSchemaComplexType.SetContentType(XmlSchemaContentType.Mixed);
			xmlSchemaComplexType.ElementDecl = SchemaElementDecl.CreateAnyTypeElementDecl();
			xmlSchemaComplexType.ElementDecl.SchemaType = xmlSchemaComplexType;
			ParticleContentValidator particleContentValidator = new ParticleContentValidator(XmlSchemaContentType.Mixed);
			particleContentValidator.Start();
			particleContentValidator.OpenGroup();
			particleContentValidator.AddNamespaceList(xmlSchemaAny.NamespaceList, xmlSchemaAny);
			particleContentValidator.AddStar();
			particleContentValidator.CloseGroup();
			ContentValidator contentValidator = particleContentValidator.Finish(true);
			xmlSchemaComplexType.ElementDecl.ContentValidator = contentValidator;
			XmlSchemaAnyAttribute xmlSchemaAnyAttribute = new XmlSchemaAnyAttribute();
			xmlSchemaAnyAttribute.ProcessContents = processContents;
			xmlSchemaAnyAttribute.BuildNamespaceList(null);
			xmlSchemaComplexType.SetAttributeWildcard(xmlSchemaAnyAttribute);
			xmlSchemaComplexType.ElementDecl.AnyAttribute = xmlSchemaAnyAttribute;
			return xmlSchemaComplexType;
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06001BB5 RID: 7093 RVA: 0x00081FBF File Offset: 0x00080FBF
		[XmlIgnore]
		internal static XmlSchemaComplexType AnyType
		{
			get
			{
				return XmlSchemaComplexType.anyTypeLax;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x00081FC6 File Offset: 0x00080FC6
		[XmlIgnore]
		internal static XmlSchemaComplexType UntypedAnyType
		{
			get
			{
				return XmlSchemaComplexType.untypedAnyType;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x00081FCD File Offset: 0x00080FCD
		[XmlIgnore]
		internal static XmlSchemaComplexType AnyTypeSkip
		{
			get
			{
				return XmlSchemaComplexType.anyTypeSkip;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x00081FD4 File Offset: 0x00080FD4
		internal static ContentValidator AnyTypeContentValidator
		{
			get
			{
				return XmlSchemaComplexType.anyTypeLax.ElementDecl.ContentValidator;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x00081FE5 File Offset: 0x00080FE5
		// (set) Token: 0x06001BBA RID: 7098 RVA: 0x00081FF5 File Offset: 0x00080FF5
		[XmlAttribute("abstract")]
		[DefaultValue(false)]
		public bool IsAbstract
		{
			get
			{
				return (this.pvFlags & 8) != 0;
			}
			set
			{
				if (value)
				{
					this.pvFlags |= 8;
					return;
				}
				this.pvFlags = (byte)((int)this.pvFlags & -9);
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06001BBB RID: 7099 RVA: 0x0008201A File Offset: 0x0008101A
		// (set) Token: 0x06001BBC RID: 7100 RVA: 0x00082022 File Offset: 0x00081022
		[XmlAttribute("block")]
		[DefaultValue(XmlSchemaDerivationMethod.None)]
		public XmlSchemaDerivationMethod Block
		{
			get
			{
				return this.block;
			}
			set
			{
				this.block = value;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06001BBD RID: 7101 RVA: 0x0008202B File Offset: 0x0008102B
		// (set) Token: 0x06001BBE RID: 7102 RVA: 0x0008203B File Offset: 0x0008103B
		[DefaultValue(false)]
		[XmlAttribute("mixed")]
		public override bool IsMixed
		{
			get
			{
				return (this.pvFlags & 4) != 0;
			}
			set
			{
				if (value)
				{
					this.pvFlags |= 4;
					return;
				}
				this.pvFlags = (byte)((int)this.pvFlags & -5);
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06001BBF RID: 7103 RVA: 0x00082060 File Offset: 0x00081060
		// (set) Token: 0x06001BC0 RID: 7104 RVA: 0x00082068 File Offset: 0x00081068
		[XmlElement("complexContent", typeof(XmlSchemaComplexContent))]
		[XmlElement("simpleContent", typeof(XmlSchemaSimpleContent))]
		public XmlSchemaContentModel ContentModel
		{
			get
			{
				return this.contentModel;
			}
			set
			{
				this.contentModel = value;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x00082071 File Offset: 0x00081071
		// (set) Token: 0x06001BC2 RID: 7106 RVA: 0x00082079 File Offset: 0x00081079
		[XmlElement("sequence", typeof(XmlSchemaSequence))]
		[XmlElement("all", typeof(XmlSchemaAll))]
		[XmlElement("choice", typeof(XmlSchemaChoice))]
		[XmlElement("group", typeof(XmlSchemaGroupRef))]
		public XmlSchemaParticle Particle
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

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x00082082 File Offset: 0x00081082
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroupRef))]
		[XmlElement("attribute", typeof(XmlSchemaAttribute))]
		public XmlSchemaObjectCollection Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new XmlSchemaObjectCollection();
				}
				return this.attributes;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x0008209D File Offset: 0x0008109D
		// (set) Token: 0x06001BC5 RID: 7109 RVA: 0x000820A5 File Offset: 0x000810A5
		[XmlElement("anyAttribute")]
		public XmlSchemaAnyAttribute AnyAttribute
		{
			get
			{
				return this.anyAttribute;
			}
			set
			{
				this.anyAttribute = value;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x000820AE File Offset: 0x000810AE
		[XmlIgnore]
		public XmlSchemaContentType ContentType
		{
			get
			{
				return base.SchemaContentType;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x000820B6 File Offset: 0x000810B6
		[XmlIgnore]
		public XmlSchemaParticle ContentTypeParticle
		{
			get
			{
				return this.contentTypeParticle;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x000820BE File Offset: 0x000810BE
		[XmlIgnore]
		public XmlSchemaDerivationMethod BlockResolved
		{
			get
			{
				return this.blockResolved;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x000820C6 File Offset: 0x000810C6
		[XmlIgnore]
		public XmlSchemaObjectTable AttributeUses
		{
			get
			{
				if (this.attributeUses == null)
				{
					this.attributeUses = new XmlSchemaObjectTable();
				}
				return this.attributeUses;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06001BCA RID: 7114 RVA: 0x000820E1 File Offset: 0x000810E1
		[XmlIgnore]
		public XmlSchemaAnyAttribute AttributeWildcard
		{
			get
			{
				return this.attributeWildcard;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x000820E9 File Offset: 0x000810E9
		[XmlIgnore]
		internal XmlSchemaObjectTable LocalElements
		{
			get
			{
				if (this.localElements == null)
				{
					this.localElements = new XmlSchemaObjectTable();
				}
				return this.localElements;
			}
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x00082104 File Offset: 0x00081104
		internal void SetContentTypeParticle(XmlSchemaParticle value)
		{
			this.contentTypeParticle = value;
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x0008210D File Offset: 0x0008110D
		internal void SetBlockResolved(XmlSchemaDerivationMethod value)
		{
			this.blockResolved = value;
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x00082116 File Offset: 0x00081116
		internal void SetAttributeWildcard(XmlSchemaAnyAttribute value)
		{
			this.attributeWildcard = value;
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x0008211F File Offset: 0x0008111F
		// (set) Token: 0x06001BD0 RID: 7120 RVA: 0x0008212F File Offset: 0x0008112F
		internal bool HasWildCard
		{
			get
			{
				return (this.pvFlags & 1) != 0;
			}
			set
			{
				if (value)
				{
					this.pvFlags |= 1;
					return;
				}
				this.pvFlags = (byte)((int)this.pvFlags & -2);
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x00082154 File Offset: 0x00081154
		// (set) Token: 0x06001BD2 RID: 7122 RVA: 0x00082164 File Offset: 0x00081164
		internal bool HasDuplicateDecls
		{
			get
			{
				return (this.pvFlags & 2) != 0;
			}
			set
			{
				if (value)
				{
					this.pvFlags |= 2;
					return;
				}
				this.pvFlags = (byte)((int)this.pvFlags & -3);
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x0008218C File Offset: 0x0008118C
		internal override XmlQualifiedName DerivedFrom
		{
			get
			{
				if (this.contentModel == null)
				{
					return XmlQualifiedName.Empty;
				}
				if (this.contentModel.Content is XmlSchemaComplexContentRestriction)
				{
					return ((XmlSchemaComplexContentRestriction)this.contentModel.Content).BaseTypeName;
				}
				if (this.contentModel.Content is XmlSchemaComplexContentExtension)
				{
					return ((XmlSchemaComplexContentExtension)this.contentModel.Content).BaseTypeName;
				}
				if (this.contentModel.Content is XmlSchemaSimpleContentRestriction)
				{
					return ((XmlSchemaSimpleContentRestriction)this.contentModel.Content).BaseTypeName;
				}
				if (this.contentModel.Content is XmlSchemaSimpleContentExtension)
				{
					return ((XmlSchemaSimpleContentExtension)this.contentModel.Content).BaseTypeName;
				}
				return XmlQualifiedName.Empty;
			}
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0008224C File Offset: 0x0008124C
		internal void SetAttributes(XmlSchemaObjectCollection newAttributes)
		{
			this.attributes = newAttributes;
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x00082258 File Offset: 0x00081258
		internal bool ContainsIdAttribute(bool findAll)
		{
			int num = 0;
			foreach (object obj in this.AttributeUses.Values)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)obj;
				if (xmlSchemaAttribute.Use != XmlSchemaUse.Prohibited)
				{
					XmlSchemaDatatype datatype = xmlSchemaAttribute.Datatype;
					if (datatype != null && datatype.TypeCode == XmlTypeCode.Id)
					{
						num++;
						if (num > 1)
						{
							break;
						}
					}
				}
			}
			if (!findAll)
			{
				return num > 0;
			}
			return num > 1;
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x000822E8 File Offset: 0x000812E8
		internal override XmlSchemaObject Clone()
		{
			XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)base.MemberwiseClone();
			if (xmlSchemaComplexType.ContentModel != null)
			{
				XmlSchemaSimpleContent xmlSchemaSimpleContent = xmlSchemaComplexType.ContentModel as XmlSchemaSimpleContent;
				if (xmlSchemaSimpleContent != null)
				{
					XmlSchemaSimpleContent xmlSchemaSimpleContent2 = (XmlSchemaSimpleContent)xmlSchemaSimpleContent.Clone();
					XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = xmlSchemaSimpleContent.Content as XmlSchemaSimpleContentExtension;
					if (xmlSchemaSimpleContentExtension != null)
					{
						XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension2 = (XmlSchemaSimpleContentExtension)xmlSchemaSimpleContentExtension.Clone();
						xmlSchemaSimpleContentExtension2.BaseTypeName = xmlSchemaSimpleContentExtension.BaseTypeName.Clone();
						xmlSchemaSimpleContentExtension2.SetAttributes(XmlSchemaComplexType.CloneAttributes(xmlSchemaSimpleContentExtension.Attributes));
						xmlSchemaSimpleContent2.Content = xmlSchemaSimpleContentExtension2;
					}
					else
					{
						XmlSchemaSimpleContentRestriction xmlSchemaSimpleContentRestriction = (XmlSchemaSimpleContentRestriction)xmlSchemaSimpleContent.Content;
						XmlSchemaSimpleContentRestriction xmlSchemaSimpleContentRestriction2 = (XmlSchemaSimpleContentRestriction)xmlSchemaSimpleContentRestriction.Clone();
						xmlSchemaSimpleContentRestriction2.BaseTypeName = xmlSchemaSimpleContentRestriction.BaseTypeName.Clone();
						xmlSchemaSimpleContentRestriction2.SetAttributes(XmlSchemaComplexType.CloneAttributes(xmlSchemaSimpleContentRestriction.Attributes));
						xmlSchemaSimpleContent2.Content = xmlSchemaSimpleContentRestriction2;
					}
					xmlSchemaComplexType.ContentModel = xmlSchemaSimpleContent2;
				}
				else
				{
					XmlSchemaComplexContent xmlSchemaComplexContent = (XmlSchemaComplexContent)xmlSchemaComplexType.ContentModel;
					XmlSchemaComplexContent xmlSchemaComplexContent2 = (XmlSchemaComplexContent)xmlSchemaComplexContent.Clone();
					XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = xmlSchemaComplexContent.Content as XmlSchemaComplexContentExtension;
					if (xmlSchemaComplexContentExtension != null)
					{
						XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension2 = (XmlSchemaComplexContentExtension)xmlSchemaComplexContentExtension.Clone();
						xmlSchemaComplexContentExtension2.BaseTypeName = xmlSchemaComplexContentExtension.BaseTypeName.Clone();
						xmlSchemaComplexContentExtension2.SetAttributes(XmlSchemaComplexType.CloneAttributes(xmlSchemaComplexContentExtension.Attributes));
						if (XmlSchemaComplexType.HasParticleRef(xmlSchemaComplexContentExtension.Particle))
						{
							xmlSchemaComplexContentExtension2.Particle = XmlSchemaComplexType.CloneParticle(xmlSchemaComplexContentExtension.Particle);
						}
						xmlSchemaComplexContent2.Content = xmlSchemaComplexContentExtension2;
					}
					else
					{
						XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = xmlSchemaComplexContent.Content as XmlSchemaComplexContentRestriction;
						XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction2 = (XmlSchemaComplexContentRestriction)xmlSchemaComplexContentRestriction.Clone();
						xmlSchemaComplexContentRestriction2.BaseTypeName = xmlSchemaComplexContentRestriction.BaseTypeName.Clone();
						xmlSchemaComplexContentRestriction2.SetAttributes(XmlSchemaComplexType.CloneAttributes(xmlSchemaComplexContentRestriction.Attributes));
						if (XmlSchemaComplexType.HasParticleRef(xmlSchemaComplexContentRestriction2.Particle))
						{
							xmlSchemaComplexContentRestriction2.Particle = XmlSchemaComplexType.CloneParticle(xmlSchemaComplexContentRestriction2.Particle);
						}
						xmlSchemaComplexContent2.Content = xmlSchemaComplexContentRestriction2;
					}
					xmlSchemaComplexType.ContentModel = xmlSchemaComplexContent2;
				}
			}
			else
			{
				if (XmlSchemaComplexType.HasParticleRef(xmlSchemaComplexType.Particle))
				{
					xmlSchemaComplexType.Particle = XmlSchemaComplexType.CloneParticle(xmlSchemaComplexType.Particle);
				}
				xmlSchemaComplexType.SetAttributes(XmlSchemaComplexType.CloneAttributes(xmlSchemaComplexType.Attributes));
			}
			xmlSchemaComplexType.ClearCompiledState();
			return xmlSchemaComplexType;
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x00082502 File Offset: 0x00081502
		private void ClearCompiledState()
		{
			this.attributeUses = null;
			this.localElements = null;
			this.attributeWildcard = null;
			this.contentTypeParticle = XmlSchemaParticle.Empty;
			this.blockResolved = XmlSchemaDerivationMethod.None;
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x00082530 File Offset: 0x00081530
		internal static XmlSchemaObjectCollection CloneAttributes(XmlSchemaObjectCollection attributes)
		{
			if (XmlSchemaComplexType.HasAttributeQNameRef(attributes))
			{
				XmlSchemaObjectCollection xmlSchemaObjectCollection = attributes.Clone();
				for (int i = 0; i < attributes.Count; i++)
				{
					XmlSchemaObject xmlSchemaObject = attributes[i];
					XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = xmlSchemaObject as XmlSchemaAttributeGroupRef;
					if (xmlSchemaAttributeGroupRef != null)
					{
						XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef2 = (XmlSchemaAttributeGroupRef)xmlSchemaAttributeGroupRef.Clone();
						xmlSchemaAttributeGroupRef2.RefName = xmlSchemaAttributeGroupRef.RefName.Clone();
						xmlSchemaObjectCollection[i] = xmlSchemaAttributeGroupRef2;
					}
					else
					{
						XmlSchemaAttribute xmlSchemaAttribute = xmlSchemaObject as XmlSchemaAttribute;
						if (!xmlSchemaAttribute.RefName.IsEmpty || !xmlSchemaAttribute.SchemaTypeName.IsEmpty)
						{
							xmlSchemaObjectCollection[i] = xmlSchemaAttribute.Clone();
						}
					}
				}
				return xmlSchemaObjectCollection;
			}
			return attributes;
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000825D8 File Offset: 0x000815D8
		private static XmlSchemaObjectCollection CloneGroupBaseParticles(XmlSchemaObjectCollection groupBaseParticles)
		{
			XmlSchemaObjectCollection xmlSchemaObjectCollection = groupBaseParticles.Clone();
			for (int i = 0; i < groupBaseParticles.Count; i++)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)groupBaseParticles[i];
				xmlSchemaObjectCollection[i] = XmlSchemaComplexType.CloneParticle(xmlSchemaParticle);
			}
			return xmlSchemaObjectCollection;
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x00082618 File Offset: 0x00081618
		internal static XmlSchemaParticle CloneParticle(XmlSchemaParticle particle)
		{
			XmlSchemaGroupBase xmlSchemaGroupBase = particle as XmlSchemaGroupBase;
			if (xmlSchemaGroupBase != null && !(xmlSchemaGroupBase is XmlSchemaAll))
			{
				XmlSchemaObjectCollection xmlSchemaObjectCollection = XmlSchemaComplexType.CloneGroupBaseParticles(xmlSchemaGroupBase.Items);
				XmlSchemaGroupBase xmlSchemaGroupBase2 = (XmlSchemaGroupBase)xmlSchemaGroupBase.Clone();
				xmlSchemaGroupBase2.SetItems(xmlSchemaObjectCollection);
				return xmlSchemaGroupBase2;
			}
			if (particle is XmlSchemaGroupRef)
			{
				XmlSchemaGroupRef xmlSchemaGroupRef = (XmlSchemaGroupRef)particle.Clone();
				xmlSchemaGroupRef.RefName = xmlSchemaGroupRef.RefName.Clone();
				return xmlSchemaGroupRef;
			}
			XmlSchemaElement xmlSchemaElement = particle as XmlSchemaElement;
			if (xmlSchemaElement != null && (!xmlSchemaElement.RefName.IsEmpty || !xmlSchemaElement.SchemaTypeName.IsEmpty))
			{
				return (XmlSchemaElement)xmlSchemaElement.Clone();
			}
			return particle;
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000826BC File Offset: 0x000816BC
		internal static bool HasParticleRef(XmlSchemaParticle particle)
		{
			XmlSchemaGroupBase xmlSchemaGroupBase = particle as XmlSchemaGroupBase;
			if (xmlSchemaGroupBase != null && !(xmlSchemaGroupBase is XmlSchemaAll))
			{
				bool flag = false;
				int num = 0;
				while (num < xmlSchemaGroupBase.Items.Count && !flag)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaGroupBase.Items[num++];
					if (xmlSchemaParticle is XmlSchemaGroupRef)
					{
						flag = true;
					}
					else
					{
						XmlSchemaElement xmlSchemaElement = xmlSchemaParticle as XmlSchemaElement;
						flag = (xmlSchemaElement != null && (!xmlSchemaElement.RefName.IsEmpty || !xmlSchemaElement.SchemaTypeName.IsEmpty)) || XmlSchemaComplexType.HasParticleRef(xmlSchemaParticle);
					}
				}
				return flag;
			}
			return particle is XmlSchemaGroupRef;
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x00082754 File Offset: 0x00081754
		internal static bool HasAttributeQNameRef(XmlSchemaObjectCollection attributes)
		{
			foreach (XmlSchemaObject xmlSchemaObject in attributes)
			{
				if (xmlSchemaObject is XmlSchemaAttributeGroupRef)
				{
					return true;
				}
				XmlSchemaAttribute xmlSchemaAttribute = xmlSchemaObject as XmlSchemaAttribute;
				if (!xmlSchemaAttribute.RefName.IsEmpty || !xmlSchemaAttribute.SchemaTypeName.IsEmpty)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001122 RID: 4386
		private const byte wildCardMask = 1;

		// Token: 0x04001123 RID: 4387
		private const byte dupDeclMask = 2;

		// Token: 0x04001124 RID: 4388
		private const byte isMixedMask = 4;

		// Token: 0x04001125 RID: 4389
		private const byte isAbstractMask = 8;

		// Token: 0x04001126 RID: 4390
		private XmlSchemaDerivationMethod block = XmlSchemaDerivationMethod.None;

		// Token: 0x04001127 RID: 4391
		private XmlSchemaContentModel contentModel;

		// Token: 0x04001128 RID: 4392
		private XmlSchemaParticle particle;

		// Token: 0x04001129 RID: 4393
		private XmlSchemaObjectCollection attributes;

		// Token: 0x0400112A RID: 4394
		private XmlSchemaAnyAttribute anyAttribute;

		// Token: 0x0400112B RID: 4395
		private XmlSchemaParticle contentTypeParticle = XmlSchemaParticle.Empty;

		// Token: 0x0400112C RID: 4396
		private XmlSchemaDerivationMethod blockResolved;

		// Token: 0x0400112D RID: 4397
		private XmlSchemaObjectTable localElements;

		// Token: 0x0400112E RID: 4398
		private XmlSchemaObjectTable attributeUses;

		// Token: 0x0400112F RID: 4399
		private XmlSchemaAnyAttribute attributeWildcard;

		// Token: 0x04001130 RID: 4400
		private static XmlSchemaComplexType anyTypeLax = XmlSchemaComplexType.CreateAnyType(XmlSchemaContentProcessing.Lax);

		// Token: 0x04001131 RID: 4401
		private static XmlSchemaComplexType anyTypeSkip = XmlSchemaComplexType.CreateAnyType(XmlSchemaContentProcessing.Skip);

		// Token: 0x04001132 RID: 4402
		private static XmlSchemaComplexType untypedAnyType = new XmlSchemaComplexType();

		// Token: 0x04001133 RID: 4403
		private byte pvFlags;
	}
}
