using System;
using System.CodeDom;
using System.Data;
using System.Data.Design;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000D8 RID: 216
	internal class MimeXmlImporter : MimeImporter
	{
		// Token: 0x060005BE RID: 1470 RVA: 0x0001BC00 File Offset: 0x0001AC00
		internal override MimeParameterCollection ImportParameters()
		{
			return null;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001BC04 File Offset: 0x0001AC04
		internal override MimeReturn ImportReturn()
		{
			MimeContentBinding mimeContentBinding = (MimeContentBinding)base.ImportContext.OperationBinding.Output.Extensions.Find(typeof(MimeContentBinding));
			if (mimeContentBinding != null)
			{
				if (!ContentType.MatchesBase(mimeContentBinding.Type, "text/xml"))
				{
					return null;
				}
				return new MimeReturn
				{
					TypeName = typeof(XmlElement).FullName,
					ReaderType = typeof(XmlReturnReader)
				};
			}
			else
			{
				MimeXmlBinding mimeXmlBinding = (MimeXmlBinding)base.ImportContext.OperationBinding.Output.Extensions.Find(typeof(MimeXmlBinding));
				if (mimeXmlBinding != null)
				{
					MimeXmlReturn mimeXmlReturn = new MimeXmlReturn();
					MessagePart messagePart;
					switch (base.ImportContext.OutputMessage.Parts.Count)
					{
					case 0:
						throw new InvalidOperationException(Res.GetString("MessageHasNoParts1", new object[] { base.ImportContext.InputMessage.Name }));
					case 1:
						if (mimeXmlBinding.Part == null || mimeXmlBinding.Part.Length == 0)
						{
							messagePart = base.ImportContext.OutputMessage.Parts[0];
						}
						else
						{
							messagePart = base.ImportContext.OutputMessage.FindPartByName(mimeXmlBinding.Part);
						}
						break;
					default:
						messagePart = base.ImportContext.OutputMessage.FindPartByName(mimeXmlBinding.Part);
						break;
					}
					mimeXmlReturn.TypeMapping = this.Importer.ImportTypeMapping(messagePart.Element);
					mimeXmlReturn.TypeName = mimeXmlReturn.TypeMapping.TypeFullName;
					mimeXmlReturn.ReaderType = typeof(XmlReturnReader);
					this.Exporter.AddMappingMetadata(mimeXmlReturn.Attributes, mimeXmlReturn.TypeMapping, string.Empty);
					return mimeXmlReturn;
				}
				return null;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0001BDC4 File Offset: 0x0001ADC4
		private XmlSchemaImporter Importer
		{
			get
			{
				if (this.importer == null)
				{
					this.importer = new XmlSchemaImporter(base.ImportContext.ConcreteSchemas, base.ImportContext.ServiceImporter.CodeGenerationOptions, base.ImportContext.ServiceImporter.CodeGenerator, base.ImportContext.ImportContext);
					foreach (Type type in base.ImportContext.ServiceImporter.Extensions)
					{
						this.importer.Extensions.Add(type.FullName, type);
					}
					this.importer.Extensions.Add(new TypedDataSetSchemaImporterExtension());
					this.importer.Extensions.Add(new DataSetSchemaImporterExtension());
				}
				return this.importer;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0001BEB0 File Offset: 0x0001AEB0
		private XmlCodeExporter Exporter
		{
			get
			{
				if (this.exporter == null)
				{
					this.exporter = new XmlCodeExporter(base.ImportContext.CodeNamespace, base.ImportContext.ServiceImporter.CodeCompileUnit, base.ImportContext.ServiceImporter.CodeGenerator, base.ImportContext.ServiceImporter.CodeGenerationOptions, base.ImportContext.ExportContext);
				}
				return this.exporter;
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001BF1C File Offset: 0x0001AF1C
		internal override void GenerateCode(MimeReturn[] importedReturns, MimeParameterCollection[] importedParameters)
		{
			for (int i = 0; i < importedReturns.Length; i++)
			{
				if (importedReturns[i] is MimeXmlReturn)
				{
					this.GenerateCode((MimeXmlReturn)importedReturns[i]);
				}
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001BF4F File Offset: 0x0001AF4F
		private void GenerateCode(MimeXmlReturn importedReturn)
		{
			this.Exporter.ExportTypeMapping(importedReturn.TypeMapping);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001BF64 File Offset: 0x0001AF64
		internal override void AddClassMetadata(CodeTypeDeclaration codeClass)
		{
			foreach (object obj in this.Exporter.IncludeMetadata)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = (CodeAttributeDeclaration)obj;
				codeClass.CustomAttributes.Add(codeAttributeDeclaration);
			}
		}

		// Token: 0x04000430 RID: 1072
		private XmlSchemaImporter importer;

		// Token: 0x04000431 RID: 1073
		private XmlCodeExporter exporter;
	}
}
