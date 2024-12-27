using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000170 RID: 368
	internal class ValueOfAction : CompiledAction
	{
		// Token: 0x06000F4B RID: 3915 RVA: 0x0004CEF2 File Offset: 0x0004BEF2
		internal static Action BuiltInRule()
		{
			return ValueOfAction.s_BuiltInRule;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0004CEF9 File Offset: 0x0004BEF9
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			base.CheckRequiredAttribute(compiler, this.selectKey != -1, "select");
			base.CheckEmpty(compiler);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0004CF24 File Offset: 0x0004BF24
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Select))
			{
				this.selectKey = compiler.AddQuery(value);
			}
			else
			{
				if (!Keywords.Equals(localName, compiler.Atoms.DisableOutputEscaping))
				{
					return false;
				}
				this.disableOutputEscaping = compiler.GetYesNo(value);
			}
			return true;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0004CF90 File Offset: 0x0004BF90
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
			{
				string text = processor.ValueOf(frame, this.selectKey);
				if (processor.TextEvent(text, this.disableOutputEscaping))
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
				break;
			default:
				return;
			}
		}

		// Token: 0x040009E2 RID: 2530
		private const int ResultStored = 2;

		// Token: 0x040009E3 RID: 2531
		private int selectKey = -1;

		// Token: 0x040009E4 RID: 2532
		private bool disableOutputEscaping;

		// Token: 0x040009E5 RID: 2533
		private static Action s_BuiltInRule = new BuiltInRuleTextAction();
	}
}
