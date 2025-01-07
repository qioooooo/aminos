﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Serialization.Advanced;

namespace System.Data.Design
{
	public class TypedDataSetSchemaImporterExtension : SchemaImporterExtension
	{
		public TypedDataSetSchemaImporterExtension()
			: this(TypedDataSetGenerator.GenerateOption.None)
		{
		}

		protected TypedDataSetSchemaImporterExtension(TypedDataSetGenerator.GenerateOption dataSetGenerateOptions)
		{
			this.dataSetGenerateOptions = dataSetGenerateOptions;
		}

		public override string ImportSchemaType(string name, string namespaceName, XmlSchemaObject context, XmlSchemas schemas, XmlSchemaImporter importer, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeGenerationOptions options, CodeDomProvider codeProvider)
		{
			IList schemas2 = schemas.GetSchemas(namespaceName);
			if (schemas2.Count != 1)
			{
				return null;
			}
			XmlSchema xmlSchema = schemas2[0] as XmlSchema;
			if (xmlSchema == null)
			{
				return null;
			}
			XmlSchemaType xmlSchemaType = (XmlSchemaType)xmlSchema.SchemaTypes[new XmlQualifiedName(name, namespaceName)];
			return this.ImportSchemaType(xmlSchemaType, context, schemas, importer, compileUnit, mainNamespace, options, codeProvider);
		}

		public override string ImportSchemaType(XmlSchemaType type, XmlSchemaObject context, XmlSchemas schemas, XmlSchemaImporter importer, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeGenerationOptions options, CodeDomProvider codeProvider)
		{
			if (type == null)
			{
				return null;
			}
			if (!(context is XmlSchemaElement))
			{
				return null;
			}
			XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)context;
			if (!TypedDataSetSchemaImporterExtension.IsDataSet(xmlSchemaElement))
			{
				if (type is XmlSchemaComplexType)
				{
					XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)type;
					if (xmlSchemaComplexType.Particle is XmlSchemaSequence)
					{
						XmlSchemaObjectCollection items = ((XmlSchemaSequence)xmlSchemaComplexType.Particle).Items;
						if (items.Count == 2 && items[0] is XmlSchemaAny && items[1] is XmlSchemaAny)
						{
							XmlSchemaAny xmlSchemaAny = (XmlSchemaAny)items[0];
							XmlSchemaAny xmlSchemaAny2 = (XmlSchemaAny)items[1];
							if (xmlSchemaAny.Namespace == "http://www.w3.org/2001/XMLSchema" && xmlSchemaAny2.Namespace == "urn:schemas-microsoft-com:xml-diffgram-v1")
							{
								string text = null;
								string text2 = null;
								foreach (XmlSchemaObject xmlSchemaObject in xmlSchemaComplexType.Attributes)
								{
									XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute)xmlSchemaObject;
									if (xmlSchemaAttribute.Name == "namespace")
									{
										text = xmlSchemaAttribute.FixedValue.Trim();
									}
									else if (xmlSchemaAttribute.Name == "tableTypeName")
									{
										text2 = xmlSchemaAttribute.FixedValue.Trim();
									}
									if (text != null && text2 != null)
									{
										break;
									}
								}
								if (text == null)
								{
									return null;
								}
								IList schemas2 = schemas.GetSchemas(text);
								if (schemas2.Count != 1)
								{
									return null;
								}
								XmlSchema xmlSchema = schemas2[0] as XmlSchema;
								if (xmlSchema == null || xmlSchema.Id == null)
								{
									return null;
								}
								XmlSchemaElement xmlSchemaElement2 = this.FindDataSetElement(xmlSchema, schemas);
								if (xmlSchemaElement2 == null)
								{
									return null;
								}
								string text3 = this.ImportSchemaType(xmlSchemaElement2.SchemaType, xmlSchemaElement2, schemas, importer, compileUnit, mainNamespace, options, codeProvider);
								if (text2 == null)
								{
									return text3;
								}
								return CodeGenHelper.GetTypeName(codeProvider, text3, text2);
							}
						}
					}
					if (xmlSchemaComplexType.Particle is XmlSchemaSequence || xmlSchemaComplexType.Particle is XmlSchemaAll)
					{
						XmlSchemaObjectCollection items2 = ((XmlSchemaGroupBase)xmlSchemaComplexType.Particle).Items;
						if (items2.Count == 1)
						{
							if (!(items2[0] is XmlSchemaAny))
							{
								return null;
							}
							XmlSchemaAny xmlSchemaAny3 = (XmlSchemaAny)items2[0];
							if (xmlSchemaAny3.Namespace == null)
							{
								return null;
							}
							if (xmlSchemaAny3.Namespace.IndexOf('#') >= 0)
							{
								return null;
							}
							if (xmlSchemaAny3.Namespace.IndexOf(' ') >= 0)
							{
								return null;
							}
							IList schemas3 = schemas.GetSchemas(xmlSchemaAny3.Namespace);
							if (schemas3.Count != 1)
							{
								return null;
							}
							XmlSchema xmlSchema2 = schemas3[0] as XmlSchema;
							if (xmlSchema2 == null)
							{
								return null;
							}
							if (xmlSchema2.Id == null)
							{
								return null;
							}
							XmlSchemaElement xmlSchemaElement3 = this.FindDataSetElement(xmlSchema2, schemas);
							if (xmlSchemaElement3 != null)
							{
								return this.ImportSchemaType(xmlSchemaElement3.SchemaType, xmlSchemaElement3, schemas, importer, compileUnit, mainNamespace, options, codeProvider);
							}
							return null;
						}
					}
				}
				return null;
			}
			if (this.importedTypes[type] != null)
			{
				return (string)this.importedTypes[type];
			}
			return this.GenerateTypedDataSet(xmlSchemaElement, schemas, compileUnit, mainNamespace, codeProvider);
		}

		internal string GenerateTypedDataSet(XmlSchemaElement element, XmlSchemas schemas, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeDomProvider codeProvider)
		{
			if (element == null)
			{
				return null;
			}
			if (this.importedTypes[element.SchemaType] != null)
			{
				return (string)this.importedTypes[element.SchemaType];
			}
			IList schemas2 = schemas.GetSchemas(element.QualifiedName.Namespace);
			if (schemas2.Count != 1)
			{
				return null;
			}
			XmlSchema xmlSchema = schemas2[0] as XmlSchema;
			if (xmlSchema == null)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			xmlSchema.Write(memoryStream);
			memoryStream.Position = 0L;
			DesignDataSource designDataSource = new DesignDataSource();
			designDataSource.ReadXmlSchema(memoryStream);
			memoryStream.Close();
			string text = TypedDataSetGenerator.GenerateInternal(designDataSource, compileUnit, mainNamespace, codeProvider, this.dataSetGenerateOptions, null);
			this.importedTypes.Add(element.SchemaType, text);
			return text;
		}

		internal static bool IsDataSet(XmlSchemaElement e)
		{
			if (e.UnhandledAttributes != null)
			{
				foreach (XmlAttribute xmlAttribute in e.UnhandledAttributes)
				{
					if (xmlAttribute.LocalName == "IsDataSet" && xmlAttribute.NamespaceURI == "urn:schemas-microsoft-com:xml-msdata" && (xmlAttribute.Value == "True" || xmlAttribute.Value == "true" || xmlAttribute.Value == "1"))
					{
						return true;
					}
				}
			}
			return false;
		}

		internal XmlSchemaElement FindDataSetElement(XmlSchema schema, XmlSchemas schemas)
		{
			foreach (XmlSchemaObject xmlSchemaObject in schema.Items)
			{
				if (xmlSchemaObject is XmlSchemaElement && TypedDataSetSchemaImporterExtension.IsDataSet((XmlSchemaElement)xmlSchemaObject))
				{
					XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaObject;
					return (XmlSchemaElement)schemas.Find(xmlSchemaElement.QualifiedName, typeof(XmlSchemaElement));
				}
			}
			return null;
		}

		private Hashtable importedTypes = new Hashtable();

		private TypedDataSetGenerator.GenerateOption dataSetGenerateOptions;
	}
}
