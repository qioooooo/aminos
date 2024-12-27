using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000158 RID: 344
	internal class IfAction : ContainerAction
	{
		// Token: 0x06000EC9 RID: 3785 RVA: 0x0004A6A8 File Offset: 0x000496A8
		internal IfAction(IfAction.ConditionType type)
		{
			this.type = type;
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0004A6BE File Offset: 0x000496BE
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			if (this.type != IfAction.ConditionType.ConditionOtherwise)
			{
				base.CheckRequiredAttribute(compiler, this.testKey != -1, "test");
			}
			if (compiler.Recurse())
			{
				base.CompileTemplate(compiler);
				compiler.ToParent();
			}
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0004A700 File Offset: 0x00049700
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (!Keywords.Equals(localName, compiler.Atoms.Test))
			{
				return false;
			}
			if (this.type == IfAction.ConditionType.ConditionOtherwise)
			{
				return false;
			}
			this.testKey = compiler.AddBooleanQuery(value);
			return true;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0004A758 File Offset: 0x00049758
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				if ((this.type == IfAction.ConditionType.ConditionIf || this.type == IfAction.ConditionType.ConditionWhen) && !processor.EvaluateBoolean(frame, this.testKey))
				{
					frame.Finished();
					return;
				}
				processor.PushActionFrame(frame);
				frame.State = 1;
				return;
			case 1:
				if (this.type == IfAction.ConditionType.ConditionWhen || this.type == IfAction.ConditionType.ConditionOtherwise)
				{
					frame.Exit();
				}
				frame.Finished();
				return;
			default:
				return;
			}
		}

		// Token: 0x04000981 RID: 2433
		private IfAction.ConditionType type;

		// Token: 0x04000982 RID: 2434
		private int testKey = -1;

		// Token: 0x02000159 RID: 345
		internal enum ConditionType
		{
			// Token: 0x04000984 RID: 2436
			ConditionIf,
			// Token: 0x04000985 RID: 2437
			ConditionWhen,
			// Token: 0x04000986 RID: 2438
			ConditionOtherwise
		}
	}
}
