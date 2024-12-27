using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000154 RID: 340
	internal class ElementAction : ContainerAction
	{
		// Token: 0x06000EB7 RID: 3767 RVA: 0x0004A163 File Offset: 0x00049163
		internal ElementAction()
		{
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0004A16C File Offset: 0x0004916C
		private static PrefixQName CreateElementQName(string name, string nsUri, InputScopeManager manager)
		{
			if (nsUri == "http://www.w3.org/2000/xmlns/")
			{
				throw XsltException.Create("Xslt_ReservedNS", new string[] { nsUri });
			}
			PrefixQName prefixQName = new PrefixQName();
			prefixQName.SetQName(name);
			if (nsUri == null)
			{
				prefixQName.Namespace = manager.ResolveXmlNamespace(prefixQName.Prefix);
			}
			else
			{
				prefixQName.Namespace = nsUri;
			}
			return prefixQName;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0004A1CC File Offset: 0x000491CC
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
					this.qname = ElementAction.CreateElementQName(this.name, this.nsUri, compiler.CloneScopeManager());
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
			this.empty = this.containedActions == null;
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0004A288 File Offset: 0x00049288
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Name))
			{
				this.nameAvt = Avt.CompileAvt(compiler, value);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.Namespace))
			{
				this.nsAvt = Avt.CompileAvt(compiler, value);
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

		// Token: 0x06000EBB RID: 3771 RVA: 0x0004A318 File Offset: 0x00049318
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
					frame.CalulatedName = ElementAction.CreateElementQName((this.nameAvt == null) ? this.name : this.nameAvt.Evaluate(processor, frame), (this.nsAvt == null) ? this.nsUri : this.nsAvt.Evaluate(processor, frame), this.manager);
				}
				break;
			case 1:
				goto IL_00C2;
			case 2:
				break;
			default:
				return;
			}
			PrefixQName calulatedName = frame.CalulatedName;
			if (!processor.BeginEvent(XPathNodeType.Element, calulatedName.Prefix, calulatedName.Name, calulatedName.Namespace, this.empty))
			{
				frame.State = 2;
				return;
			}
			if (!this.empty)
			{
				processor.PushActionFrame(frame);
				frame.State = 1;
				return;
			}
			IL_00C2:
			if (!processor.EndEvent(XPathNodeType.Element))
			{
				frame.State = 1;
				return;
			}
			frame.Finished();
		}

		// Token: 0x04000971 RID: 2417
		private const int NameDone = 2;

		// Token: 0x04000972 RID: 2418
		private Avt nameAvt;

		// Token: 0x04000973 RID: 2419
		private Avt nsAvt;

		// Token: 0x04000974 RID: 2420
		private bool empty;

		// Token: 0x04000975 RID: 2421
		private InputScopeManager manager;

		// Token: 0x04000976 RID: 2422
		private string name;

		// Token: 0x04000977 RID: 2423
		private string nsUri;

		// Token: 0x04000978 RID: 2424
		private PrefixQName qname;
	}
}
