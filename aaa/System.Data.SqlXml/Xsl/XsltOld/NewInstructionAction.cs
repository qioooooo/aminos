using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200015D RID: 349
	internal class NewInstructionAction : ContainerAction
	{
		// Token: 0x06000ED9 RID: 3801 RVA: 0x0004A958 File Offset: 0x00049958
		internal override void Compile(Compiler compiler)
		{
			XPathNavigator xpathNavigator = compiler.Input.Navigator.Clone();
			this.name = xpathNavigator.Name;
			xpathNavigator.MoveToParent();
			this.parent = xpathNavigator.Name;
			if (compiler.Recurse())
			{
				this.CompileSelectiveTemplate(compiler);
				compiler.ToParent();
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0004A9AC File Offset: 0x000499AC
		internal void CompileSelectiveTemplate(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			do
			{
				if (Keywords.Equals(input.NamespaceURI, input.Atoms.XsltNamespace) && Keywords.Equals(input.LocalName, input.Atoms.Fallback))
				{
					this.fallback = true;
					if (compiler.Recurse())
					{
						base.CompileTemplate(compiler);
						compiler.ToParent();
					}
				}
			}
			while (compiler.Advance());
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0004AA18 File Offset: 0x00049A18
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				if (!this.fallback)
				{
					throw XsltException.Create("Xslt_UnknownExtensionElement", new string[] { this.name });
				}
				if (this.containedActions != null && this.containedActions.Count > 0)
				{
					processor.PushActionFrame(frame);
					frame.State = 1;
					return;
				}
				break;
			case 1:
				break;
			default:
				return;
			}
			frame.Finished();
		}

		// Token: 0x0400098A RID: 2442
		private string name;

		// Token: 0x0400098B RID: 2443
		private string parent;

		// Token: 0x0400098C RID: 2444
		private bool fallback;
	}
}
