using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000014 RID: 20
	internal sealed class SoapAttributeInfo
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000059EB File Offset: 0x000049EB
		internal string AttributeElementName
		{
			get
			{
				return this.m_elementName;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000059F3 File Offset: 0x000049F3
		internal string AttributeTypeName
		{
			get
			{
				return this.m_typeName;
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000059FB File Offset: 0x000049FB
		internal bool IsEmbedded()
		{
			return (this.m_attributeType & SoapAttributeType.Embedded) > SoapAttributeType.None;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005A0B File Offset: 0x00004A0B
		internal bool IsXmlElement()
		{
			return (this.m_attributeType & SoapAttributeType.XmlElement) > SoapAttributeType.None;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005A1B File Offset: 0x00004A1B
		internal bool IsXmlAttribute()
		{
			return (this.m_attributeType & SoapAttributeType.XmlAttribute) > SoapAttributeType.None;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005A2B File Offset: 0x00004A2B
		internal bool IsXmlType()
		{
			return (this.m_attributeType & SoapAttributeType.XmlType) > SoapAttributeType.None;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005A3B File Offset: 0x00004A3B
		[Conditional("SER_LOGGING")]
		internal void Dump(string id)
		{
			this.IsXmlType();
			this.IsEmbedded();
			this.IsXmlElement();
			this.IsXmlAttribute();
		}

		// Token: 0x04000093 RID: 147
		internal SoapAttributeType m_attributeType;

		// Token: 0x04000094 RID: 148
		internal string m_nameSpace;

		// Token: 0x04000095 RID: 149
		internal string m_elementName;

		// Token: 0x04000096 RID: 150
		internal string m_typeName;

		// Token: 0x04000097 RID: 151
		internal string m_typeNamespace;
	}
}
