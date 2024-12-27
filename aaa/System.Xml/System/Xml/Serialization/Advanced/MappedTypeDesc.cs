using System;
using System.CodeDom;
using System.Collections.Specialized;
using System.Xml.Schema;

namespace System.Xml.Serialization.Advanced
{
	// Token: 0x02000349 RID: 841
	internal class MappedTypeDesc
	{
		// Token: 0x060028E2 RID: 10466 RVA: 0x000D2108 File Offset: 0x000D1108
		internal MappedTypeDesc(string clrType, string name, string ns, XmlSchemaType xsdType, XmlSchemaObject context, SchemaImporterExtension extension, CodeNamespace code, StringCollection references)
		{
			this.clrType = clrType.Replace('+', '.');
			this.name = name;
			this.ns = ns;
			this.xsdType = xsdType;
			this.context = context;
			this.code = code;
			this.references = references;
			this.extension = extension;
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x060028E3 RID: 10467 RVA: 0x000D2161 File Offset: 0x000D1161
		internal SchemaImporterExtension Extension
		{
			get
			{
				return this.extension;
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x060028E4 RID: 10468 RVA: 0x000D2169 File Offset: 0x000D1169
		internal string Name
		{
			get
			{
				return this.clrType;
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x060028E5 RID: 10469 RVA: 0x000D2171 File Offset: 0x000D1171
		internal StringCollection ReferencedAssemblies
		{
			get
			{
				if (this.references == null)
				{
					this.references = new StringCollection();
				}
				return this.references;
			}
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x000D218C File Offset: 0x000D118C
		internal CodeTypeDeclaration ExportTypeDefinition(CodeNamespace codeNamespace, CodeCompileUnit codeCompileUnit)
		{
			if (this.exported)
			{
				return null;
			}
			this.exported = true;
			foreach (object obj in this.code.Imports)
			{
				CodeNamespaceImport codeNamespaceImport = (CodeNamespaceImport)obj;
				codeNamespace.Imports.Add(codeNamespaceImport);
			}
			CodeTypeDeclaration codeTypeDeclaration = null;
			string @string = Res.GetString("XmlExtensionComment", new object[] { this.extension.GetType().FullName });
			foreach (object obj2 in this.code.Types)
			{
				CodeTypeDeclaration codeTypeDeclaration2 = (CodeTypeDeclaration)obj2;
				if (this.clrType == codeTypeDeclaration2.Name)
				{
					if (codeTypeDeclaration != null)
					{
						throw new InvalidOperationException(Res.GetString("XmlExtensionDuplicateDefinition", new object[]
						{
							this.extension.GetType().FullName,
							this.clrType
						}));
					}
					codeTypeDeclaration = codeTypeDeclaration2;
				}
				codeTypeDeclaration2.Comments.Add(new CodeCommentStatement(@string, false));
				codeNamespace.Types.Add(codeTypeDeclaration2);
			}
			if (codeCompileUnit != null)
			{
				foreach (string text in this.ReferencedAssemblies)
				{
					if (!codeCompileUnit.ReferencedAssemblies.Contains(text))
					{
						codeCompileUnit.ReferencedAssemblies.Add(text);
					}
				}
			}
			return codeTypeDeclaration;
		}

		// Token: 0x0400169D RID: 5789
		private string name;

		// Token: 0x0400169E RID: 5790
		private string ns;

		// Token: 0x0400169F RID: 5791
		private XmlSchemaType xsdType;

		// Token: 0x040016A0 RID: 5792
		private XmlSchemaObject context;

		// Token: 0x040016A1 RID: 5793
		private string clrType;

		// Token: 0x040016A2 RID: 5794
		private SchemaImporterExtension extension;

		// Token: 0x040016A3 RID: 5795
		private CodeNamespace code;

		// Token: 0x040016A4 RID: 5796
		private bool exported;

		// Token: 0x040016A5 RID: 5797
		private StringCollection references;
	}
}
