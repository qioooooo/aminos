using System;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000DC RID: 220
	internal class SchemaCompiler
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0001C2F2 File Offset: 0x0001B2F2
		internal static StringCollection Warnings
		{
			get
			{
				if (SchemaCompiler.warnings == null)
				{
					SchemaCompiler.warnings = new StringCollection();
				}
				return SchemaCompiler.warnings;
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001C30A File Offset: 0x0001B30A
		internal static StringCollection Compile(XmlSchemas schemas)
		{
			SchemaCompiler.AddImports(schemas);
			SchemaCompiler.Warnings.Clear();
			schemas.Compile(new ValidationEventHandler(SchemaCompiler.ValidationCallbackWithErrorCode), true);
			return SchemaCompiler.Warnings;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001C334 File Offset: 0x0001B334
		private static void AddImport(XmlSchema schema, string ns)
		{
			if (schema.TargetNamespace == ns)
			{
				return;
			}
			foreach (XmlSchemaObject xmlSchemaObject in schema.Includes)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)xmlSchemaObject;
				XmlSchemaImport xmlSchemaImport = xmlSchemaExternal as XmlSchemaImport;
				if (xmlSchemaImport != null && xmlSchemaImport.Namespace == ns)
				{
					return;
				}
			}
			XmlSchemaImport xmlSchemaImport2 = new XmlSchemaImport();
			xmlSchemaImport2.Namespace = ns;
			schema.Includes.Add(xmlSchemaImport2);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001C3CC File Offset: 0x0001B3CC
		private static void AddImports(XmlSchemas schemas)
		{
			foreach (object obj in schemas)
			{
				XmlSchema xmlSchema = (XmlSchema)obj;
				SchemaCompiler.AddImport(xmlSchema, "http://schemas.xmlsoap.org/soap/encoding/");
				SchemaCompiler.AddImport(xmlSchema, "http://schemas.xmlsoap.org/wsdl/");
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001C430 File Offset: 0x0001B430
		internal static string WarningDetails(XmlSchemaException exception, string message)
		{
			XmlSchemaObject xmlSchemaObject = exception.SourceSchemaObject;
			string text;
			if (exception.LineNumber == 0 && exception.LinePosition == 0)
			{
				text = SchemaCompiler.GetSchemaItem(xmlSchemaObject, null, message);
			}
			else
			{
				string text2 = null;
				if (xmlSchemaObject != null)
				{
					while (xmlSchemaObject.Parent != null)
					{
						xmlSchemaObject = xmlSchemaObject.Parent;
					}
					if (xmlSchemaObject is XmlSchema)
					{
						text2 = ((XmlSchema)xmlSchemaObject).TargetNamespace;
					}
				}
				text = Res.GetString("SchemaSyntaxErrorDetails", new object[] { text2, message, exception.LineNumber, exception.LinePosition });
			}
			return text;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001C4C4 File Offset: 0x0001B4C4
		private static string GetSchemaItem(XmlSchemaObject o, string ns, string details)
		{
			if (o == null)
			{
				return null;
			}
			while (o.Parent != null && !(o.Parent is XmlSchema))
			{
				o = o.Parent;
			}
			if (ns == null || ns.Length == 0)
			{
				XmlSchemaObject xmlSchemaObject = o;
				while (xmlSchemaObject.Parent != null)
				{
					xmlSchemaObject = xmlSchemaObject.Parent;
				}
				if (xmlSchemaObject is XmlSchema)
				{
					ns = ((XmlSchema)xmlSchemaObject).TargetNamespace;
				}
			}
			string text;
			if (o is XmlSchemaNotation)
			{
				text = Res.GetString("XmlSchemaNamedItem", new object[]
				{
					ns,
					"notation",
					((XmlSchemaNotation)o).Name,
					details
				});
			}
			else if (o is XmlSchemaGroup)
			{
				text = Res.GetString("XmlSchemaNamedItem", new object[]
				{
					ns,
					"group",
					((XmlSchemaGroup)o).Name,
					details
				});
			}
			else if (o is XmlSchemaElement)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)o;
				if (xmlSchemaElement.Name == null || xmlSchemaElement.Name.Length == 0)
				{
					XmlQualifiedName parentName = SchemaCompiler.GetParentName(o);
					text = Res.GetString("XmlSchemaElementReference", new object[]
					{
						xmlSchemaElement.RefName.ToString(),
						parentName.Name,
						parentName.Namespace
					});
				}
				else
				{
					text = Res.GetString("XmlSchemaNamedItem", new object[] { ns, "element", xmlSchemaElement.Name, details });
				}
			}
			else if (o is XmlSchemaType)
			{
				text = Res.GetString("XmlSchemaNamedItem", new object[]
				{
					ns,
					(o.GetType() == typeof(XmlSchemaSimpleType)) ? "simpleType" : "complexType",
					((XmlSchemaType)o).Name,
					details
				});
			}
			else if (o is XmlSchemaAttributeGroup)
			{
				text = Res.GetString("XmlSchemaNamedItem", new object[]
				{
					ns,
					"attributeGroup",
					((XmlSchemaAttributeGroup)o).Name,
					details
				});
			}
			else if (o is XmlSchemaAttribute)
			{
				XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)o;
				if (xmlSchemaAttribute.Name == null || xmlSchemaAttribute.Name.Length == 0)
				{
					XmlQualifiedName parentName2 = SchemaCompiler.GetParentName(o);
					return Res.GetString("XmlSchemaAttributeReference", new object[]
					{
						xmlSchemaAttribute.RefName.ToString(),
						parentName2.Name,
						parentName2.Namespace
					});
				}
				text = Res.GetString("XmlSchemaNamedItem", new object[] { ns, "attribute", xmlSchemaAttribute.Name, details });
			}
			else if (o is XmlSchemaContent)
			{
				XmlQualifiedName parentName3 = SchemaCompiler.GetParentName(o);
				text = Res.GetString("XmlSchemaContentDef", new object[] { parentName3.Name, parentName3.Namespace, details });
			}
			else if (o is XmlSchemaExternal)
			{
				string text2 = ((o is XmlSchemaImport) ? "import" : ((o is XmlSchemaInclude) ? "include" : ((o is XmlSchemaRedefine) ? "redefine" : o.GetType().Name)));
				text = Res.GetString("XmlSchemaItem", new object[] { ns, text2, details });
			}
			else if (o is XmlSchema)
			{
				text = Res.GetString("XmlSchema", new object[] { ns, details });
			}
			else
			{
				text = Res.GetString("XmlSchemaNamedItem", new object[]
				{
					ns,
					o.GetType().Name,
					null,
					details
				});
			}
			return text;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001C8A0 File Offset: 0x0001B8A0
		internal static XmlQualifiedName GetParentName(XmlSchemaObject item)
		{
			while (item.Parent != null)
			{
				if (item.Parent is XmlSchemaType)
				{
					XmlSchemaType xmlSchemaType = (XmlSchemaType)item.Parent;
					if (xmlSchemaType.Name != null && xmlSchemaType.Name.Length != 0)
					{
						return xmlSchemaType.QualifiedName;
					}
				}
				item = item.Parent;
			}
			return XmlQualifiedName.Empty;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001C8FC File Offset: 0x0001B8FC
		private static void ValidationCallbackWithErrorCode(object sender, ValidationEventArgs args)
		{
			SchemaCompiler.Warnings.Add(Res.GetString((args.Severity == XmlSeverityType.Error) ? "SchemaValidationError" : "SchemaValidationWarning", new object[] { SchemaCompiler.WarningDetails(args.Exception, args.Message) }));
		}

		// Token: 0x04000434 RID: 1076
		private static StringCollection warnings;
	}
}
