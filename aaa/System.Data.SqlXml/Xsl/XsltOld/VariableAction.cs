using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000172 RID: 370
	internal class VariableAction : ContainerAction, IXsltContextVariable
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x0004D055 File Offset: 0x0004C055
		internal int Stylesheetid
		{
			get
			{
				return this.stylesheetid;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0004D05D File Offset: 0x0004C05D
		internal XmlQualifiedName Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000F57 RID: 3927 RVA: 0x0004D065 File Offset: 0x0004C065
		internal string NameStr
		{
			get
			{
				return this.nameStr;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000F58 RID: 3928 RVA: 0x0004D06D File Offset: 0x0004C06D
		internal VariableType VarType
		{
			get
			{
				return this.varType;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0004D075 File Offset: 0x0004C075
		internal int VarKey
		{
			get
			{
				return this.varKey;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000F5A RID: 3930 RVA: 0x0004D07D File Offset: 0x0004C07D
		internal bool IsGlobal
		{
			get
			{
				return this.varType == VariableType.GlobalVariable || this.varType == VariableType.GlobalParameter;
			}
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0004D092 File Offset: 0x0004C092
		internal VariableAction(VariableType type)
		{
			this.varType = type;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0004D0A8 File Offset: 0x0004C0A8
		internal override void Compile(Compiler compiler)
		{
			this.stylesheetid = compiler.Stylesheetid;
			this.baseUri = compiler.Input.BaseURI;
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
			if (this.containedActions != null)
			{
				this.baseUri = this.baseUri + '#' + compiler.GetUnicRtfId();
			}
			else
			{
				this.baseUri = null;
			}
			this.varKey = compiler.InsertVariable(this);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0004D16C File Offset: 0x0004C16C
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Name))
			{
				this.nameStr = value;
				this.name = compiler.CreateXPathQName(this.nameStr);
			}
			else
			{
				if (!Keywords.Equals(localName, compiler.Atoms.Select))
				{
					return false;
				}
				this.selectKey = compiler.AddQuery(value);
			}
			return true;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0004D1E4 File Offset: 0x0004C1E4
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			object obj = null;
			switch (frame.State)
			{
			case 0:
				if (this.IsGlobal)
				{
					if (frame.GetVariable(this.varKey) != null)
					{
						frame.Finished();
						return;
					}
					frame.SetVariable(this.varKey, VariableAction.BeingComputedMark);
				}
				if (this.varType == VariableType.GlobalParameter)
				{
					obj = processor.GetGlobalParameter(this.name);
				}
				else if (this.varType == VariableType.LocalParameter)
				{
					obj = processor.GetParameter(this.name);
				}
				if (obj == null)
				{
					if (this.selectKey != -1)
					{
						obj = processor.RunQuery(frame, this.selectKey);
					}
					else
					{
						if (this.containedActions != null)
						{
							NavigatorOutput navigatorOutput = new NavigatorOutput(this.baseUri);
							processor.PushOutput(navigatorOutput);
							processor.PushActionFrame(frame);
							frame.State = 1;
							return;
						}
						obj = string.Empty;
					}
				}
				break;
			case 1:
			{
				RecordOutput recordOutput = processor.PopOutput();
				obj = ((NavigatorOutput)recordOutput).Navigator;
				break;
			}
			case 2:
				break;
			default:
				return;
			}
			frame.SetVariable(this.varKey, obj);
			frame.Finished();
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0004D2DD File Offset: 0x0004C2DD
		XPathResultType IXsltContextVariable.VariableType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0004D2E0 File Offset: 0x0004C2E0
		object IXsltContextVariable.Evaluate(XsltContext xsltContext)
		{
			return ((XsltCompileContext)xsltContext).EvaluateVariable(this);
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0004D2EE File Offset: 0x0004C2EE
		bool IXsltContextVariable.IsLocal
		{
			get
			{
				return this.varType == VariableType.LocalVariable || this.varType == VariableType.LocalParameter;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0004D304 File Offset: 0x0004C304
		bool IXsltContextVariable.IsParam
		{
			get
			{
				return this.varType == VariableType.LocalParameter || this.varType == VariableType.GlobalParameter;
			}
		}

		// Token: 0x040009E7 RID: 2535
		private const int ValueCalculated = 2;

		// Token: 0x040009E8 RID: 2536
		public static object BeingComputedMark = new object();

		// Token: 0x040009E9 RID: 2537
		protected XmlQualifiedName name;

		// Token: 0x040009EA RID: 2538
		protected string nameStr;

		// Token: 0x040009EB RID: 2539
		protected string baseUri;

		// Token: 0x040009EC RID: 2540
		protected int selectKey = -1;

		// Token: 0x040009ED RID: 2541
		protected int stylesheetid;

		// Token: 0x040009EE RID: 2542
		protected VariableType varType;

		// Token: 0x040009EF RID: 2543
		private int varKey;
	}
}
