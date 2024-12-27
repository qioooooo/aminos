using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000130 RID: 304
	internal abstract class CompiledAction : Action
	{
		// Token: 0x06000D6D RID: 3437
		internal abstract void Compile(Compiler compiler);

		// Token: 0x06000D6E RID: 3438 RVA: 0x00045292 File Offset: 0x00044292
		internal virtual bool CompileAttribute(Compiler compiler)
		{
			return false;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00045298 File Offset: 0x00044298
		public void CompileAttributes(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			string localName = input.LocalName;
			if (input.MoveToFirstAttribute())
			{
				do
				{
					if (Keywords.Equals(input.NamespaceURI, input.Atoms.Empty))
					{
						try
						{
							if (!this.CompileAttribute(compiler))
							{
								throw XsltException.Create("Xslt_InvalidAttribute", new string[] { input.LocalName, localName });
							}
						}
						catch
						{
							if (!compiler.ForwardCompatibility)
							{
								throw;
							}
						}
					}
				}
				while (input.MoveToNextAttribute());
				input.ToParent();
			}
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0004532C File Offset: 0x0004432C
		internal static string PrecalculateAvt(ref Avt avt)
		{
			string text = null;
			if (avt != null && avt.IsConstant)
			{
				text = avt.Evaluate(null, null);
				avt = null;
			}
			return text;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00045358 File Offset: 0x00044358
		public void CheckEmpty(Compiler compiler)
		{
			string name = compiler.Input.Name;
			if (compiler.Recurse())
			{
				for (;;)
				{
					XPathNodeType nodeType = compiler.Input.NodeType;
					if (nodeType != XPathNodeType.Whitespace && nodeType != XPathNodeType.Comment && nodeType != XPathNodeType.ProcessingInstruction)
					{
						break;
					}
					if (!compiler.Advance())
					{
						goto Block_4;
					}
				}
				throw XsltException.Create("Xslt_NotEmptyContents", new string[] { name });
				Block_4:
				compiler.ToParent();
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x000453B7 File Offset: 0x000443B7
		public void CheckRequiredAttribute(Compiler compiler, object attrValue, string attrName)
		{
			this.CheckRequiredAttribute(compiler, attrValue != null, attrName);
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x000453C8 File Offset: 0x000443C8
		public void CheckRequiredAttribute(Compiler compiler, bool attr, string attrName)
		{
			if (!attr)
			{
				throw XsltException.Create("Xslt_MissingAttribute", new string[] { attrName });
			}
		}
	}
}
