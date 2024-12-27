using System;
using System.ComponentModel;
using System.Reflection;

namespace System.Xml.Serialization
{
	// Token: 0x02000305 RID: 773
	public class XmlAttributes
	{
		// Token: 0x0600241E RID: 9246 RVA: 0x000AA6FB File Offset: 0x000A96FB
		public XmlAttributes()
		{
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x000AA724 File Offset: 0x000A9724
		internal XmlAttributeFlags XmlFlags
		{
			get
			{
				XmlAttributeFlags xmlAttributeFlags = (XmlAttributeFlags)0;
				if (this.xmlElements.Count > 0)
				{
					xmlAttributeFlags |= XmlAttributeFlags.Elements;
				}
				if (this.xmlArrayItems.Count > 0)
				{
					xmlAttributeFlags |= XmlAttributeFlags.ArrayItems;
				}
				if (this.xmlAnyElements.Count > 0)
				{
					xmlAttributeFlags |= XmlAttributeFlags.AnyElements;
				}
				if (this.xmlArray != null)
				{
					xmlAttributeFlags |= XmlAttributeFlags.Array;
				}
				if (this.xmlAttribute != null)
				{
					xmlAttributeFlags |= XmlAttributeFlags.Attribute;
				}
				if (this.xmlText != null)
				{
					xmlAttributeFlags |= XmlAttributeFlags.Text;
				}
				if (this.xmlEnum != null)
				{
					xmlAttributeFlags |= XmlAttributeFlags.Enum;
				}
				if (this.xmlRoot != null)
				{
					xmlAttributeFlags |= XmlAttributeFlags.Root;
				}
				if (this.xmlType != null)
				{
					xmlAttributeFlags |= XmlAttributeFlags.Type;
				}
				if (this.xmlAnyAttribute != null)
				{
					xmlAttributeFlags |= XmlAttributeFlags.AnyAttribute;
				}
				if (this.xmlChoiceIdentifier != null)
				{
					xmlAttributeFlags |= XmlAttributeFlags.ChoiceIdentifier;
				}
				if (this.xmlns)
				{
					xmlAttributeFlags |= XmlAttributeFlags.XmlnsDeclarations;
				}
				return xmlAttributeFlags;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x000AA7F0 File Offset: 0x000A97F0
		private static Type IgnoreAttribute
		{
			get
			{
				if (XmlAttributes.ignoreAttributeType == null)
				{
					XmlAttributes.ignoreAttributeType = typeof(object).Assembly.GetType("System.XmlIgnoreMemberAttribute");
					if (XmlAttributes.ignoreAttributeType == null)
					{
						XmlAttributes.ignoreAttributeType = typeof(XmlIgnoreAttribute);
					}
				}
				return XmlAttributes.ignoreAttributeType;
			}
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x000AA840 File Offset: 0x000A9840
		public XmlAttributes(ICustomAttributeProvider provider)
		{
			object[] customAttributes = provider.GetCustomAttributes(false);
			XmlAnyElementAttribute xmlAnyElementAttribute = null;
			for (int i = 0; i < customAttributes.Length; i++)
			{
				if (customAttributes[i] is XmlIgnoreAttribute || customAttributes[i] is ObsoleteAttribute || customAttributes[i].GetType() == XmlAttributes.IgnoreAttribute)
				{
					this.xmlIgnore = true;
					break;
				}
				if (customAttributes[i] is XmlElementAttribute)
				{
					this.xmlElements.Add((XmlElementAttribute)customAttributes[i]);
				}
				else if (customAttributes[i] is XmlArrayItemAttribute)
				{
					this.xmlArrayItems.Add((XmlArrayItemAttribute)customAttributes[i]);
				}
				else if (customAttributes[i] is XmlAnyElementAttribute)
				{
					XmlAnyElementAttribute xmlAnyElementAttribute2 = (XmlAnyElementAttribute)customAttributes[i];
					if ((xmlAnyElementAttribute2.Name == null || xmlAnyElementAttribute2.Name.Length == 0) && xmlAnyElementAttribute2.NamespaceSpecified && xmlAnyElementAttribute2.Namespace == null)
					{
						xmlAnyElementAttribute = xmlAnyElementAttribute2;
					}
					else
					{
						this.xmlAnyElements.Add((XmlAnyElementAttribute)customAttributes[i]);
					}
				}
				else if (customAttributes[i] is DefaultValueAttribute)
				{
					this.xmlDefaultValue = ((DefaultValueAttribute)customAttributes[i]).Value;
				}
				else if (customAttributes[i] is XmlAttributeAttribute)
				{
					this.xmlAttribute = (XmlAttributeAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is XmlArrayAttribute)
				{
					this.xmlArray = (XmlArrayAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is XmlTextAttribute)
				{
					this.xmlText = (XmlTextAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is XmlEnumAttribute)
				{
					this.xmlEnum = (XmlEnumAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is XmlRootAttribute)
				{
					this.xmlRoot = (XmlRootAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is XmlTypeAttribute)
				{
					this.xmlType = (XmlTypeAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is XmlAnyAttributeAttribute)
				{
					this.xmlAnyAttribute = (XmlAnyAttributeAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is XmlChoiceIdentifierAttribute)
				{
					this.xmlChoiceIdentifier = (XmlChoiceIdentifierAttribute)customAttributes[i];
				}
				else if (customAttributes[i] is XmlNamespaceDeclarationsAttribute)
				{
					this.xmlns = true;
				}
			}
			if (this.xmlIgnore)
			{
				this.xmlElements.Clear();
				this.xmlArrayItems.Clear();
				this.xmlAnyElements.Clear();
				this.xmlDefaultValue = null;
				this.xmlAttribute = null;
				this.xmlArray = null;
				this.xmlText = null;
				this.xmlEnum = null;
				this.xmlType = null;
				this.xmlAnyAttribute = null;
				this.xmlChoiceIdentifier = null;
				this.xmlns = false;
				return;
			}
			if (xmlAnyElementAttribute != null)
			{
				this.xmlAnyElements.Add(xmlAnyElementAttribute);
			}
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x000AAAE4 File Offset: 0x000A9AE4
		internal static object GetAttr(ICustomAttributeProvider provider, Type attrType)
		{
			object[] customAttributes = provider.GetCustomAttributes(attrType, false);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			return customAttributes[0];
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06002423 RID: 9251 RVA: 0x000AAB04 File Offset: 0x000A9B04
		public XmlElementAttributes XmlElements
		{
			get
			{
				return this.xmlElements;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x000AAB0C File Offset: 0x000A9B0C
		// (set) Token: 0x06002425 RID: 9253 RVA: 0x000AAB14 File Offset: 0x000A9B14
		public XmlAttributeAttribute XmlAttribute
		{
			get
			{
				return this.xmlAttribute;
			}
			set
			{
				this.xmlAttribute = value;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06002426 RID: 9254 RVA: 0x000AAB1D File Offset: 0x000A9B1D
		// (set) Token: 0x06002427 RID: 9255 RVA: 0x000AAB25 File Offset: 0x000A9B25
		public XmlEnumAttribute XmlEnum
		{
			get
			{
				return this.xmlEnum;
			}
			set
			{
				this.xmlEnum = value;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06002428 RID: 9256 RVA: 0x000AAB2E File Offset: 0x000A9B2E
		// (set) Token: 0x06002429 RID: 9257 RVA: 0x000AAB36 File Offset: 0x000A9B36
		public XmlTextAttribute XmlText
		{
			get
			{
				return this.xmlText;
			}
			set
			{
				this.xmlText = value;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x0600242A RID: 9258 RVA: 0x000AAB3F File Offset: 0x000A9B3F
		// (set) Token: 0x0600242B RID: 9259 RVA: 0x000AAB47 File Offset: 0x000A9B47
		public XmlArrayAttribute XmlArray
		{
			get
			{
				return this.xmlArray;
			}
			set
			{
				this.xmlArray = value;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x0600242C RID: 9260 RVA: 0x000AAB50 File Offset: 0x000A9B50
		public XmlArrayItemAttributes XmlArrayItems
		{
			get
			{
				return this.xmlArrayItems;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x000AAB58 File Offset: 0x000A9B58
		// (set) Token: 0x0600242E RID: 9262 RVA: 0x000AAB60 File Offset: 0x000A9B60
		public object XmlDefaultValue
		{
			get
			{
				return this.xmlDefaultValue;
			}
			set
			{
				this.xmlDefaultValue = value;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x000AAB69 File Offset: 0x000A9B69
		// (set) Token: 0x06002430 RID: 9264 RVA: 0x000AAB71 File Offset: 0x000A9B71
		public bool XmlIgnore
		{
			get
			{
				return this.xmlIgnore;
			}
			set
			{
				this.xmlIgnore = value;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x000AAB7A File Offset: 0x000A9B7A
		// (set) Token: 0x06002432 RID: 9266 RVA: 0x000AAB82 File Offset: 0x000A9B82
		public XmlTypeAttribute XmlType
		{
			get
			{
				return this.xmlType;
			}
			set
			{
				this.xmlType = value;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x000AAB8B File Offset: 0x000A9B8B
		// (set) Token: 0x06002434 RID: 9268 RVA: 0x000AAB93 File Offset: 0x000A9B93
		public XmlRootAttribute XmlRoot
		{
			get
			{
				return this.xmlRoot;
			}
			set
			{
				this.xmlRoot = value;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x000AAB9C File Offset: 0x000A9B9C
		public XmlAnyElementAttributes XmlAnyElements
		{
			get
			{
				return this.xmlAnyElements;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06002436 RID: 9270 RVA: 0x000AABA4 File Offset: 0x000A9BA4
		// (set) Token: 0x06002437 RID: 9271 RVA: 0x000AABAC File Offset: 0x000A9BAC
		public XmlAnyAttributeAttribute XmlAnyAttribute
		{
			get
			{
				return this.xmlAnyAttribute;
			}
			set
			{
				this.xmlAnyAttribute = value;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x000AABB5 File Offset: 0x000A9BB5
		public XmlChoiceIdentifierAttribute XmlChoiceIdentifier
		{
			get
			{
				return this.xmlChoiceIdentifier;
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x000AABBD File Offset: 0x000A9BBD
		// (set) Token: 0x0600243A RID: 9274 RVA: 0x000AABC5 File Offset: 0x000A9BC5
		public bool Xmlns
		{
			get
			{
				return this.xmlns;
			}
			set
			{
				this.xmlns = value;
			}
		}

		// Token: 0x04001560 RID: 5472
		private XmlElementAttributes xmlElements = new XmlElementAttributes();

		// Token: 0x04001561 RID: 5473
		private XmlArrayItemAttributes xmlArrayItems = new XmlArrayItemAttributes();

		// Token: 0x04001562 RID: 5474
		private XmlAnyElementAttributes xmlAnyElements = new XmlAnyElementAttributes();

		// Token: 0x04001563 RID: 5475
		private XmlArrayAttribute xmlArray;

		// Token: 0x04001564 RID: 5476
		private XmlAttributeAttribute xmlAttribute;

		// Token: 0x04001565 RID: 5477
		private XmlTextAttribute xmlText;

		// Token: 0x04001566 RID: 5478
		private XmlEnumAttribute xmlEnum;

		// Token: 0x04001567 RID: 5479
		private bool xmlIgnore;

		// Token: 0x04001568 RID: 5480
		private bool xmlns;

		// Token: 0x04001569 RID: 5481
		private object xmlDefaultValue;

		// Token: 0x0400156A RID: 5482
		private XmlRootAttribute xmlRoot;

		// Token: 0x0400156B RID: 5483
		private XmlTypeAttribute xmlType;

		// Token: 0x0400156C RID: 5484
		private XmlAnyAttributeAttribute xmlAnyAttribute;

		// Token: 0x0400156D RID: 5485
		private XmlChoiceIdentifierAttribute xmlChoiceIdentifier;

		// Token: 0x0400156E RID: 5486
		private static Type ignoreAttributeType;
	}
}
