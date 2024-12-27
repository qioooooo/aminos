using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000134 RID: 308
	internal class AttributeAction : ContainerAction
	{
		// Token: 0x06000D9F RID: 3487 RVA: 0x00046D88 File Offset: 0x00045D88
		private static PrefixQName CreateAttributeQName(string name, string nsUri, InputScopeManager manager)
		{
			if (name == "xmlns")
			{
				return null;
			}
			if (nsUri == "http://www.w3.org/2000/xmlns/")
			{
				throw XsltException.Create("Xslt_ReservedNS", new string[] { nsUri });
			}
			PrefixQName prefixQName = new PrefixQName();
			prefixQName.SetQName(name);
			prefixQName.Namespace = ((nsUri != null) ? nsUri : manager.ResolveXPathNamespace(prefixQName.Prefix));
			if (prefixQName.Prefix.StartsWith("xml", StringComparison.Ordinal))
			{
				if (prefixQName.Prefix.Length == 3)
				{
					if (!(prefixQName.Namespace == "http://www.w3.org/XML/1998/namespace") || (!(prefixQName.Name == "lang") && !(prefixQName.Name == "space")))
					{
						prefixQName.ClearPrefix();
					}
				}
				else if (prefixQName.Prefix == "xmlns")
				{
					if (prefixQName.Namespace == "http://www.w3.org/2000/xmlns/")
					{
						throw XsltException.Create("Xslt_InvalidPrefix", new string[] { prefixQName.Prefix });
					}
					prefixQName.ClearPrefix();
				}
			}
			return prefixQName;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00046E98 File Offset: 0x00045E98
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			base.CheckRequiredAttribute(compiler, this.nameAvt, "name");
			this.name = CompiledAction.PrecalculateAvt(ref this.nameAvt);
			this.nsUri = CompiledAction.PrecalculateAvt(ref this.nsAvt);
			if (this.nameAvt == null && this.nsAvt == null)
			{
				if (this.name != "xmlns")
				{
					this.qname = AttributeAction.CreateAttributeQName(this.name, this.nsUri, compiler.CloneScopeManager());
				}
			}
			else
			{
				this.manager = compiler.CloneScopeManager();
			}
			if (compiler.Recurse())
			{
				base.CompileTemplate(compiler);
				compiler.ToParent();
			}
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00046F44 File Offset: 0x00045F44
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Name))
			{
				this.nameAvt = Avt.CompileAvt(compiler, value);
			}
			else
			{
				if (!Keywords.Equals(localName, compiler.Atoms.Namespace))
				{
					return false;
				}
				this.nsAvt = Avt.CompileAvt(compiler, value);
			}
			return true;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00046FB0 File Offset: 0x00045FB0
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				if (this.qname != null)
				{
					frame.CalulatedName = this.qname;
				}
				else
				{
					frame.CalulatedName = AttributeAction.CreateAttributeQName((this.nameAvt == null) ? this.name : this.nameAvt.Evaluate(processor, frame), (this.nsAvt == null) ? this.nsUri : this.nsAvt.Evaluate(processor, frame), this.manager);
					if (frame.CalulatedName == null)
					{
						frame.Finished();
						return;
					}
				}
				break;
			case 1:
				if (!processor.EndEvent(XPathNodeType.Attribute))
				{
					frame.State = 1;
					return;
				}
				frame.Finished();
				return;
			case 2:
				break;
			default:
				return;
			}
			PrefixQName calulatedName = frame.CalulatedName;
			if (!processor.BeginEvent(XPathNodeType.Attribute, calulatedName.Prefix, calulatedName.Name, calulatedName.Namespace, false))
			{
				frame.State = 2;
				return;
			}
			processor.PushActionFrame(frame);
			frame.State = 1;
		}

		// Token: 0x040008FA RID: 2298
		private const int NameDone = 2;

		// Token: 0x040008FB RID: 2299
		private Avt nameAvt;

		// Token: 0x040008FC RID: 2300
		private Avt nsAvt;

		// Token: 0x040008FD RID: 2301
		private InputScopeManager manager;

		// Token: 0x040008FE RID: 2302
		private string name;

		// Token: 0x040008FF RID: 2303
		private string nsUri;

		// Token: 0x04000900 RID: 2304
		private PrefixQName qname;
	}
}
