using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Metadata;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000030 RID: 48
	internal static class Attr
	{
		// Token: 0x06000155 RID: 341 RVA: 0x0000FAA4 File Offset: 0x0000EAA4
		internal static SoapAttributeInfo GetMemberAttributeInfo(MemberInfo memberInfo, string name, Type type)
		{
			SoapAttributeInfo soapAttributeInfo = new SoapAttributeInfo();
			Attr.ProcessTypeAttribute(type, soapAttributeInfo);
			Attr.ProcessMemberInfoAttribute(memberInfo, soapAttributeInfo);
			return soapAttributeInfo;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000FAC8 File Offset: 0x0000EAC8
		internal static void ProcessTypeAttribute(Type type, SoapAttributeInfo attributeInfo)
		{
			SoapTypeAttribute soapTypeAttribute = (SoapTypeAttribute)InternalRemotingServices.GetCachedSoapAttribute(type);
			if (soapTypeAttribute.Embedded)
			{
				attributeInfo.m_attributeType |= SoapAttributeType.Embedded;
			}
			string text;
			string text2;
			if (SoapServices.GetXmlElementForInteropType(type, out text, out text2))
			{
				attributeInfo.m_attributeType |= SoapAttributeType.XmlElement;
				attributeInfo.m_elementName = text;
				attributeInfo.m_nameSpace = text2;
			}
			if (SoapServices.GetXmlTypeForInteropType(type, out text, out text2))
			{
				attributeInfo.m_attributeType |= SoapAttributeType.XmlType;
				attributeInfo.m_typeName = text;
				attributeInfo.m_typeNamespace = text2;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000FB48 File Offset: 0x0000EB48
		internal static void ProcessMemberInfoAttribute(MemberInfo memberInfo, SoapAttributeInfo attributeInfo)
		{
			SoapAttribute cachedSoapAttribute = InternalRemotingServices.GetCachedSoapAttribute(memberInfo);
			if (cachedSoapAttribute.Embedded)
			{
				attributeInfo.m_attributeType |= SoapAttributeType.Embedded;
			}
			if (cachedSoapAttribute is SoapFieldAttribute)
			{
				SoapFieldAttribute soapFieldAttribute = (SoapFieldAttribute)cachedSoapAttribute;
				if (soapFieldAttribute.UseAttribute)
				{
					attributeInfo.m_attributeType |= SoapAttributeType.XmlAttribute;
					attributeInfo.m_elementName = soapFieldAttribute.XmlElementName;
					attributeInfo.m_nameSpace = soapFieldAttribute.XmlNamespace;
					return;
				}
				if (soapFieldAttribute.IsInteropXmlElement())
				{
					attributeInfo.m_attributeType |= SoapAttributeType.XmlElement;
					attributeInfo.m_elementName = soapFieldAttribute.XmlElementName;
					attributeInfo.m_nameSpace = soapFieldAttribute.XmlNamespace;
				}
			}
		}
	}
}
