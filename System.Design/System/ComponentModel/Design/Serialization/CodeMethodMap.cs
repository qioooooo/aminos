using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	internal class CodeMethodMap
	{
		internal CodeMethodMap(CodeMemberMethod method)
			: this(null, method)
		{
		}

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

		internal CodeMemberMethod Method
		{
			get
			{
				return this._method;
			}
		}

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

		private CodeStatementCollection _container;

		private CodeStatementCollection _begin;

		private CodeStatementCollection _end;

		private CodeStatementCollection _statements;

		private CodeStatementCollection _locals;

		private CodeStatementCollection _fields;

		private CodeStatementCollection _variables;

		private CodeStatementCollection _targetStatements;

		private CodeMemberMethod _method;
	}
}
