using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Xml.Schema;

namespace System.Xml.Serialization.Advanced
{
	// Token: 0x02000347 RID: 839
	public abstract class SchemaImporterExtension
	{
		// Token: 0x060028CE RID: 10446 RVA: 0x000D1ED0 File Offset: 0x000D0ED0
		public virtual string ImportSchemaType(string name, string ns, XmlSchemaObject context, XmlSchemas schemas, XmlSchemaImporter importer, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeGenerationOptions options, CodeDomProvider codeProvider)
		{
			return null;
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x000D1ED3 File Offset: 0x000D0ED3
		public virtual string ImportSchemaType(XmlSchemaType type, XmlSchemaObject context, XmlSchemas schemas, XmlSchemaImporter importer, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeGenerationOptions options, CodeDomProvider codeProvider)
		{
			return null;
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x000D1ED6 File Offset: 0x000D0ED6
		public virtual string ImportAnyElement(XmlSchemaAny any, bool mixed, XmlSchemas schemas, XmlSchemaImporter importer, CodeCompileUnit compileUnit, CodeNamespace mainNamespace, CodeGenerationOptions options, CodeDomProvider codeProvider)
		{
			return null;
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x000D1ED9 File Offset: 0x000D0ED9
		public virtual CodeExpression ImportDefaultValue(string value, string type)
		{
			return null;
		}
	}
}
