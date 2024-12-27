using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000163 RID: 355
	internal class ProcessingInstructionAction : ContainerAction
	{
		// Token: 0x06000EFE RID: 3838 RVA: 0x0004B9CC File Offset: 0x0004A9CC
		internal ProcessingInstructionAction()
		{
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0004B9D4 File Offset: 0x0004A9D4
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			base.CheckRequiredAttribute(compiler, this.nameAvt, "name");
			if (this.nameAvt.IsConstant)
			{
				this.name = this.nameAvt.Evaluate(null, null);
				this.nameAvt = null;
				if (!ProcessingInstructionAction.IsProcessingInstructionName(this.name))
				{
					this.name = null;
				}
			}
			if (compiler.Recurse())
			{
				base.CompileTemplate(compiler);
				compiler.ToParent();
			}
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0004BA4C File Offset: 0x0004AA4C
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Name))
			{
				this.nameAvt = Avt.CompileAvt(compiler, value);
				return true;
			}
			return false;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0004BA98 File Offset: 0x0004AA98
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				if (this.nameAvt == null)
				{
					frame.StoredOutput = this.name;
					if (this.name == null)
					{
						frame.Finished();
						return;
					}
				}
				else
				{
					frame.StoredOutput = this.nameAvt.Evaluate(processor, frame);
					if (!ProcessingInstructionAction.IsProcessingInstructionName(frame.StoredOutput))
					{
						frame.Finished();
						return;
					}
				}
				break;
			case 1:
				if (!processor.EndEvent(XPathNodeType.ProcessingInstruction))
				{
					frame.State = 1;
					return;
				}
				frame.Finished();
				return;
			case 2:
				goto IL_00B5;
			case 3:
				break;
			default:
				goto IL_00B5;
			}
			if (!processor.BeginEvent(XPathNodeType.ProcessingInstruction, string.Empty, frame.StoredOutput, string.Empty, false))
			{
				frame.State = 3;
				return;
			}
			processor.PushActionFrame(frame);
			frame.State = 1;
			return;
			IL_00B5:
			frame.Finished();
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0004BB60 File Offset: 0x0004AB60
		internal static bool IsProcessingInstructionName(string name)
		{
			if (name == null)
			{
				return false;
			}
			int length = name.Length;
			int i = 0;
			XmlCharType instance = XmlCharType.Instance;
			while (i < length && instance.IsWhiteSpace(name[i]))
			{
				i++;
			}
			if (i >= length)
			{
				return false;
			}
			if (i < length && !instance.IsStartNCNameChar(name[i]))
			{
				return false;
			}
			while (i < length)
			{
				if (!instance.IsNCNameChar(name[i]))
				{
					break;
				}
				i++;
			}
			while (i < length && instance.IsWhiteSpace(name[i]))
			{
				i++;
			}
			return i >= length && (length != 3 || (name[0] != 'X' && name[0] != 'x') || (name[1] != 'M' && name[1] != 'm') || (name[2] != 'L' && name[2] != 'l'));
		}

		// Token: 0x040009B1 RID: 2481
		private const int NameEvaluated = 2;

		// Token: 0x040009B2 RID: 2482
		private const int NameReady = 3;

		// Token: 0x040009B3 RID: 2483
		private const char CharX = 'X';

		// Token: 0x040009B4 RID: 2484
		private const char Charx = 'x';

		// Token: 0x040009B5 RID: 2485
		private const char CharM = 'M';

		// Token: 0x040009B6 RID: 2486
		private const char Charm = 'm';

		// Token: 0x040009B7 RID: 2487
		private const char CharL = 'L';

		// Token: 0x040009B8 RID: 2488
		private const char Charl = 'l';

		// Token: 0x040009B9 RID: 2489
		private Avt nameAvt;

		// Token: 0x040009BA RID: 2490
		private string name;
	}
}
