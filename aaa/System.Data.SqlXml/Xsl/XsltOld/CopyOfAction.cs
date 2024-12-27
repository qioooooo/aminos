using System;
using System.Xml.XPath;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000149 RID: 329
	internal class CopyOfAction : CompiledAction
	{
		// Token: 0x06000E6A RID: 3690 RVA: 0x0004995E File Offset: 0x0004895E
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			base.CheckRequiredAttribute(compiler, this.selectKey != -1, "select");
			base.CheckEmpty(compiler);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00049988 File Offset: 0x00048988
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Select))
			{
				this.selectKey = compiler.AddQuery(value);
				return true;
			}
			return false;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000499D4 File Offset: 0x000489D4
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
			{
				Query valueQuery = processor.GetValueQuery(this.selectKey);
				object obj = valueQuery.Evaluate(frame.NodeSet);
				if (obj is XPathNodeIterator)
				{
					processor.PushActionFrame(CopyNodeSetAction.GetAction(), new XPathArrayIterator(valueQuery));
					frame.State = 3;
					return;
				}
				XPathNavigator xpathNavigator = obj as XPathNavigator;
				if (xpathNavigator != null)
				{
					processor.PushActionFrame(CopyNodeSetAction.GetAction(), new XPathSingletonIterator(xpathNavigator));
					frame.State = 3;
					return;
				}
				string text = XmlConvert.ToXPathString(obj);
				if (processor.TextEvent(text))
				{
					frame.Finished();
					return;
				}
				frame.StoredOutput = text;
				frame.State = 2;
				return;
			}
			case 1:
				break;
			case 2:
				processor.TextEvent(frame.StoredOutput);
				frame.Finished();
				return;
			case 3:
				frame.Finished();
				break;
			default:
				return;
			}
		}

		// Token: 0x0400095E RID: 2398
		private const int ResultStored = 2;

		// Token: 0x0400095F RID: 2399
		private const int NodeSetCopied = 3;

		// Token: 0x04000960 RID: 2400
		private int selectKey = -1;
	}
}
