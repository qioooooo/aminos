using System;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000D9 RID: 217
	internal class MimeXmlReflector : MimeReflector
	{
		// Token: 0x060005C6 RID: 1478 RVA: 0x0001BFD0 File Offset: 0x0001AFD0
		internal override bool ReflectParameters()
		{
			return false;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001BFD4 File Offset: 0x0001AFD4
		internal override bool ReflectReturn()
		{
			MessagePart messagePart = new MessagePart();
			messagePart.Name = "Body";
			base.ReflectionContext.OutputMessage.Parts.Add(messagePart);
			if (typeof(XmlNode).IsAssignableFrom(base.ReflectionContext.Method.ReturnType))
			{
				MimeContentBinding mimeContentBinding = new MimeContentBinding();
				mimeContentBinding.Type = "text/xml";
				mimeContentBinding.Part = messagePart.Name;
				base.ReflectionContext.OperationBinding.Output.Extensions.Add(mimeContentBinding);
			}
			else
			{
				MimeXmlBinding mimeXmlBinding = new MimeXmlBinding();
				mimeXmlBinding.Part = messagePart.Name;
				LogicalMethodInfo method = base.ReflectionContext.Method;
				XmlAttributes xmlAttributes = new XmlAttributes(method.ReturnTypeCustomAttributeProvider);
				XmlTypeMapping xmlTypeMapping = base.ReflectionContext.ReflectionImporter.ImportTypeMapping(method.ReturnType, xmlAttributes.XmlRoot);
				xmlTypeMapping.SetKey(method.GetKey() + ":Return");
				base.ReflectionContext.SchemaExporter.ExportTypeMapping(xmlTypeMapping);
				messagePart.Element = new XmlQualifiedName(xmlTypeMapping.XsdElementName, xmlTypeMapping.Namespace);
				base.ReflectionContext.OperationBinding.Output.Extensions.Add(mimeXmlBinding);
			}
			return true;
		}
	}
}
