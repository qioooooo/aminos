using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000174 RID: 372
	internal class WithParamAction : VariableAction
	{
		// Token: 0x06000F68 RID: 3944 RVA: 0x0004D370 File Offset: 0x0004C370
		internal WithParamAction()
			: base(VariableType.WithParameter)
		{
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0004D37C File Offset: 0x0004C37C
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			base.CheckRequiredAttribute(compiler, this.name, "name");
			if (compiler.Recurse())
			{
				base.CompileTemplate(compiler);
				compiler.ToParent();
				if (this.selectKey != -1 && this.containedActions != null)
				{
					throw XsltException.Create("Xslt_VariableCntSel2", new string[] { this.nameStr });
				}
			}
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0004D3E8 File Offset: 0x0004C3E8
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
			{
				if (this.selectKey != -1)
				{
					object obj = processor.RunQuery(frame, this.selectKey);
					processor.SetParameter(this.name, obj);
					frame.Finished();
					return;
				}
				if (this.containedActions == null)
				{
					processor.SetParameter(this.name, string.Empty);
					frame.Finished();
					return;
				}
				NavigatorOutput navigatorOutput = new NavigatorOutput(this.baseUri);
				processor.PushOutput(navigatorOutput);
				processor.PushActionFrame(frame);
				frame.State = 1;
				return;
			}
			case 1:
			{
				RecordOutput recordOutput = processor.PopOutput();
				processor.SetParameter(this.name, ((NavigatorOutput)recordOutput).Navigator);
				frame.Finished();
				return;
			}
			default:
				return;
			}
		}
	}
}
