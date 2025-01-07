﻿using System;
using System.CodeDom;
using System.Collections.Specialized;
using System.Xml.Schema;

namespace System.Xml.Serialization.Advanced
{
	internal class MappedTypeDesc
	{
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

		internal SchemaImporterExtension Extension
		{
			get
			{
				return this.extension;
			}
		}

		internal string Name
		{
			get
			{
				return this.clrType;
			}
		}

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

		private string name;

		private string ns;

		private XmlSchemaType xsdType;

		private XmlSchemaObject context;

		private string clrType;

		private SchemaImporterExtension extension;

		private CodeNamespace code;

		private bool exported;

		private StringCollection references;
	}
}
