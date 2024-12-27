using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000135 RID: 309
	internal class AttributeSetAction : ContainerAction
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x000470A0 File Offset: 0x000460A0
		internal XmlQualifiedName Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x000470A8 File Offset: 0x000460A8
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			base.CheckRequiredAttribute(compiler, this.name, "name");
			this.CompileContent(compiler);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x000470CC File Offset: 0x000460CC
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Name))
			{
				this.name = compiler.CreateXPathQName(value);
			}
			else
			{
				if (!Keywords.Equals(localName, compiler.Atoms.UseAttributeSets))
				{
					return false;
				}
				base.AddAction(compiler.CreateUseAttributeSetsAction());
			}
			return true;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00047138 File Offset: 0x00046138
		private void CompileContent(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			if (compiler.Recurse())
			{
				for (;;)
				{
					switch (input.NodeType)
					{
					case XPathNodeType.Element:
					{
						compiler.PushNamespaceScope();
						string namespaceURI = input.NamespaceURI;
						string localName = input.LocalName;
						if (Keywords.Equals(namespaceURI, input.Atoms.XsltNamespace) && Keywords.Equals(localName, input.Atoms.Attribute))
						{
							base.AddAction(compiler.CreateAttributeAction());
							compiler.PopScope();
							goto IL_00B8;
						}
						goto IL_008B;
					}
					case XPathNodeType.SignificantWhitespace:
					case XPathNodeType.Whitespace:
					case XPathNodeType.ProcessingInstruction:
					case XPathNodeType.Comment:
						goto IL_00B8;
					}
					break;
					IL_00B8:
					if (!compiler.Advance())
					{
						goto Block_4;
					}
				}
				goto IL_009A;
				IL_008B:
				throw compiler.UnexpectedKeyword();
				IL_009A:
				throw XsltException.Create("Xslt_InvalidContents", new string[] { "attribute-set" });
				Block_4:
				compiler.ToParent();
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00047210 File Offset: 0x00046210
		internal void Merge(AttributeSetAction attributeAction)
		{
			int num = 0;
			Action action;
			while ((action = attributeAction.GetAction(num)) != null)
			{
				base.AddAction(action);
				num++;
			}
		}

		// Token: 0x04000901 RID: 2305
		internal XmlQualifiedName name;
	}
}
