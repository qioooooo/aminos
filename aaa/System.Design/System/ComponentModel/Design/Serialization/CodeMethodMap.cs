using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000151 RID: 337
	internal class CodeMethodMap
	{
		// Token: 0x06000CC0 RID: 3264 RVA: 0x00030D41 File Offset: 0x0002FD41
		internal CodeMethodMap(CodeMemberMethod method)
			: this(null, method)
		{
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00030D4B File Offset: 0x0002FD4B
		internal CodeMethodMap(CodeStatementCollection targetStatements, CodeMemberMethod method)
		{
			this._method = method;
			if (targetStatements != null)
			{
				this._targetStatements = targetStatements;
				return;
			}
			this._targetStatements = this._method.Statements;
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x00030D76 File Offset: 0x0002FD76
		internal CodeStatementCollection BeginStatements
		{
			get
			{
				if (this._begin == null)
				{
					this._begin = new CodeStatementCollection();
				}
				return this._begin;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00030D91 File Offset: 0x0002FD91
		internal CodeStatementCollection EndStatements
		{
			get
			{
				if (this._end == null)
				{
					this._end = new CodeStatementCollection();
				}
				return this._end;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x00030DAC File Offset: 0x0002FDAC
		internal CodeStatementCollection ContainerStatements
		{
			get
			{
				if (this._container == null)
				{
					this._container = new CodeStatementCollection();
				}
				return this._container;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00030DC7 File Offset: 0x0002FDC7
		internal CodeMemberMethod Method
		{
			get
			{
				return this._method;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00030DCF File Offset: 0x0002FDCF
		internal CodeStatementCollection Statements
		{
			get
			{
				if (this._statements == null)
				{
					this._statements = new CodeStatementCollection();
				}
				return this._statements;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00030DEA File Offset: 0x0002FDEA
		internal CodeStatementCollection LocalVariables
		{
			get
			{
				if (this._locals == null)
				{
					this._locals = new CodeStatementCollection();
				}
				return this._locals;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x00030E05 File Offset: 0x0002FE05
		internal CodeStatementCollection FieldAssignments
		{
			get
			{
				if (this._fields == null)
				{
					this._fields = new CodeStatementCollection();
				}
				return this._fields;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x00030E20 File Offset: 0x0002FE20
		internal CodeStatementCollection VariableAssignments
		{
			get
			{
				if (this._variables == null)
				{
					this._variables = new CodeStatementCollection();
				}
				return this._variables;
			}
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00030E3C File Offset: 0x0002FE3C
		internal void Add(CodeStatementCollection statements)
		{
			foreach (object obj in statements)
			{
				CodeStatement codeStatement = (CodeStatement)obj;
				string text = codeStatement.UserData["IContainer"] as string;
				if (text != null && text == "IContainer")
				{
					this.ContainerStatements.Add(codeStatement);
				}
				else if (codeStatement is CodeAssignStatement && ((CodeAssignStatement)codeStatement).Left is CodeFieldReferenceExpression)
				{
					this.FieldAssignments.Add(codeStatement);
				}
				else if (codeStatement is CodeAssignStatement && ((CodeAssignStatement)codeStatement).Left is CodeVariableReferenceExpression)
				{
					this.VariableAssignments.Add(codeStatement);
				}
				else if (codeStatement is CodeVariableDeclarationStatement)
				{
					this.LocalVariables.Add(codeStatement);
				}
				else
				{
					string text2 = codeStatement.UserData["statement-ordering"] as string;
					if (text2 != null)
					{
						string text3;
						if ((text3 = text2) != null)
						{
							if (text3 == "begin")
							{
								this.BeginStatements.Add(codeStatement);
								continue;
							}
							if (text3 == "end")
							{
								this.EndStatements.Add(codeStatement);
								continue;
							}
							if (!(text3 == "default"))
							{
							}
						}
						this.Statements.Add(codeStatement);
					}
					else
					{
						this.Statements.Add(codeStatement);
					}
				}
			}
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00030FD0 File Offset: 0x0002FFD0
		internal void Combine()
		{
			if (this._container != null)
			{
				this._targetStatements.AddRange(this._container);
			}
			if (this._locals != null)
			{
				this._targetStatements.AddRange(this._locals);
			}
			if (this._fields != null)
			{
				this._targetStatements.AddRange(this._fields);
			}
			if (this._variables != null)
			{
				this._targetStatements.AddRange(this._variables);
			}
			if (this._begin != null)
			{
				this._targetStatements.AddRange(this._begin);
			}
			if (this._statements != null)
			{
				this._targetStatements.AddRange(this._statements);
			}
			if (this._end != null)
			{
				this._targetStatements.AddRange(this._end);
			}
		}

		// Token: 0x04000EBE RID: 3774
		private CodeStatementCollection _container;

		// Token: 0x04000EBF RID: 3775
		private CodeStatementCollection _begin;

		// Token: 0x04000EC0 RID: 3776
		private CodeStatementCollection _end;

		// Token: 0x04000EC1 RID: 3777
		private CodeStatementCollection _statements;

		// Token: 0x04000EC2 RID: 3778
		private CodeStatementCollection _locals;

		// Token: 0x04000EC3 RID: 3779
		private CodeStatementCollection _fields;

		// Token: 0x04000EC4 RID: 3780
		private CodeStatementCollection _variables;

		// Token: 0x04000EC5 RID: 3781
		private CodeStatementCollection _targetStatements;

		// Token: 0x04000EC6 RID: 3782
		private CodeMemberMethod _method;
	}
}
