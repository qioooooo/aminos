using System;
using System.Globalization;
using System.IO;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200015B RID: 347
	internal class MessageAction : ContainerAction
	{
		// Token: 0x06000ED1 RID: 3793 RVA: 0x0004A80F File Offset: 0x0004980F
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			if (compiler.Recurse())
			{
				base.CompileTemplate(compiler);
				compiler.ToParent();
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0004A830 File Offset: 0x00049830
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Terminate))
			{
				this._Terminate = compiler.GetYesNo(value);
				return true;
			}
			return false;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0004A87C File Offset: 0x0004987C
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
			{
				TextOnlyOutput textOnlyOutput = new TextOnlyOutput(processor, new StringWriter(CultureInfo.InvariantCulture));
				processor.PushOutput(textOnlyOutput);
				processor.PushActionFrame(frame);
				frame.State = 1;
				return;
			}
			case 1:
			{
				TextOnlyOutput textOnlyOutput2 = processor.PopOutput() as TextOnlyOutput;
				Console.WriteLine(textOnlyOutput2.Writer.ToString());
				if (this._Terminate)
				{
					throw XsltException.Create("Xslt_Terminate", new string[] { textOnlyOutput2.Writer.ToString() });
				}
				frame.Finished();
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x04000988 RID: 2440
		private bool _Terminate;
	}
}
