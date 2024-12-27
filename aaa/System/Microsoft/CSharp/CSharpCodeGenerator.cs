using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.CSharp
{
	// Token: 0x020002B9 RID: 697
	internal class CSharpCodeGenerator : ICodeCompiler, ICodeGenerator
	{
		// Token: 0x060016CD RID: 5837 RVA: 0x0004844D File Offset: 0x0004744D
		internal CSharpCodeGenerator()
		{
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00048455 File Offset: 0x00047455
		internal CSharpCodeGenerator(IDictionary<string, string> providerOptions)
		{
			this.provOptions = providerOptions;
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060016CF RID: 5839 RVA: 0x00048464 File Offset: 0x00047464
		private string FileExtension
		{
			get
			{
				return ".cs";
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060016D0 RID: 5840 RVA: 0x0004846B File Offset: 0x0004746B
		private string CompilerName
		{
			get
			{
				return "csc.exe";
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060016D1 RID: 5841 RVA: 0x00048472 File Offset: 0x00047472
		private string CurrentTypeName
		{
			get
			{
				if (this.currentClass != null)
				{
					return this.currentClass.Name;
				}
				return "<% unknown %>";
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060016D2 RID: 5842 RVA: 0x0004848D File Offset: 0x0004748D
		// (set) Token: 0x060016D3 RID: 5843 RVA: 0x0004849A File Offset: 0x0004749A
		private int Indent
		{
			get
			{
				return this.output.Indent;
			}
			set
			{
				this.output.Indent = value;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060016D4 RID: 5844 RVA: 0x000484A8 File Offset: 0x000474A8
		private bool IsCurrentInterface
		{
			get
			{
				return this.currentClass != null && !(this.currentClass is CodeTypeDelegate) && this.currentClass.IsInterface;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060016D5 RID: 5845 RVA: 0x000484CC File Offset: 0x000474CC
		private bool IsCurrentClass
		{
			get
			{
				return this.currentClass != null && !(this.currentClass is CodeTypeDelegate) && this.currentClass.IsClass;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060016D6 RID: 5846 RVA: 0x000484F0 File Offset: 0x000474F0
		private bool IsCurrentStruct
		{
			get
			{
				return this.currentClass != null && !(this.currentClass is CodeTypeDelegate) && this.currentClass.IsStruct;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060016D7 RID: 5847 RVA: 0x00048514 File Offset: 0x00047514
		private bool IsCurrentEnum
		{
			get
			{
				return this.currentClass != null && !(this.currentClass is CodeTypeDelegate) && this.currentClass.IsEnum;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060016D8 RID: 5848 RVA: 0x00048538 File Offset: 0x00047538
		private bool IsCurrentDelegate
		{
			get
			{
				return this.currentClass != null && this.currentClass is CodeTypeDelegate;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060016D9 RID: 5849 RVA: 0x00048552 File Offset: 0x00047552
		private string NullToken
		{
			get
			{
				return "null";
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060016DA RID: 5850 RVA: 0x00048559 File Offset: 0x00047559
		private CodeGeneratorOptions Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060016DB RID: 5851 RVA: 0x00048561 File Offset: 0x00047561
		private TextWriter Output
		{
			get
			{
				return this.output;
			}
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x0004856C File Offset: 0x0004756C
		private string QuoteSnippetStringCStyle(string value)
		{
			StringBuilder stringBuilder = new StringBuilder(value.Length + 5);
			Indentation indentation = new Indentation((IndentedTextWriter)this.Output, this.Indent + 1);
			stringBuilder.Append("\"");
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				if (c <= '"')
				{
					if (c != '\0')
					{
						switch (c)
						{
						case '\t':
							stringBuilder.Append("\\t");
							break;
						case '\n':
							stringBuilder.Append("\\n");
							break;
						case '\v':
						case '\f':
							goto IL_0107;
						case '\r':
							stringBuilder.Append("\\r");
							break;
						default:
							if (c != '"')
							{
								goto IL_0107;
							}
							stringBuilder.Append("\\\"");
							break;
						}
					}
					else
					{
						stringBuilder.Append("\\0");
					}
				}
				else if (c != '\'')
				{
					if (c != '\\')
					{
						switch (c)
						{
						case '\u2028':
						case '\u2029':
							this.AppendEscapedChar(stringBuilder, value[i]);
							break;
						default:
							goto IL_0107;
						}
					}
					else
					{
						stringBuilder.Append("\\\\");
					}
				}
				else
				{
					stringBuilder.Append("\\'");
				}
				IL_0115:
				if (i > 0 && i % 80 == 0)
				{
					if (char.IsHighSurrogate(value[i]) && i < value.Length - 1 && char.IsLowSurrogate(value[i + 1]))
					{
						stringBuilder.Append(value[++i]);
					}
					stringBuilder.Append("\" +\r\n");
					stringBuilder.Append(indentation.IndentationString);
					stringBuilder.Append('"');
				}
				i++;
				continue;
				IL_0107:
				stringBuilder.Append(value[i]);
				goto IL_0115;
			}
			stringBuilder.Append("\"");
			return stringBuilder.ToString();
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x00048718 File Offset: 0x00047718
		private string QuoteSnippetStringVerbatimStyle(string value)
		{
			StringBuilder stringBuilder = new StringBuilder(value.Length + 5);
			stringBuilder.Append("@\"");
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] == '"')
				{
					stringBuilder.Append("\"\"");
				}
				else
				{
					stringBuilder.Append(value[i]);
				}
			}
			stringBuilder.Append("\"");
			return stringBuilder.ToString();
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x00048789 File Offset: 0x00047789
		private string QuoteSnippetString(string value)
		{
			if (value.Length < 256 || value.Length > 1500 || value.IndexOf('\0') != -1)
			{
				return this.QuoteSnippetStringCStyle(value);
			}
			return this.QuoteSnippetStringVerbatimStyle(value);
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x000487C0 File Offset: 0x000477C0
		private void ProcessCompilerOutputLine(CompilerResults results, string line)
		{
			if (CSharpCodeGenerator.outputReg == null)
			{
				CSharpCodeGenerator.outputReg = new Regex("(^([^(]+)(\\(([0-9]+),([0-9]+)\\))?: )?(error|warning) ([A-Z]+[0-9]+) ?: (.*)");
			}
			Match match = CSharpCodeGenerator.outputReg.Match(line);
			if (match.Success)
			{
				CompilerError compilerError = new CompilerError();
				if (match.Groups[3].Success)
				{
					compilerError.FileName = match.Groups[2].Value;
					compilerError.Line = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);
					compilerError.Column = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);
				}
				if (string.Compare(match.Groups[6].Value, "warning", StringComparison.OrdinalIgnoreCase) == 0)
				{
					compilerError.IsWarning = true;
				}
				compilerError.ErrorNumber = match.Groups[7].Value;
				compilerError.ErrorText = match.Groups[8].Value;
				results.Errors.Add(compilerError);
			}
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x000488CC File Offset: 0x000478CC
		private string CmdArgsFromParameters(CompilerParameters options)
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			if (options.GenerateExecutable)
			{
				stringBuilder.Append("/t:exe ");
				if (options.MainClass != null && options.MainClass.Length > 0)
				{
					stringBuilder.Append("/main:");
					stringBuilder.Append(options.MainClass);
					stringBuilder.Append(" ");
				}
			}
			else
			{
				stringBuilder.Append("/t:library ");
			}
			stringBuilder.Append("/utf8output ");
			foreach (string text in options.ReferencedAssemblies)
			{
				stringBuilder.Append("/R:");
				stringBuilder.Append("\"");
				stringBuilder.Append(text);
				stringBuilder.Append("\"");
				stringBuilder.Append(" ");
			}
			stringBuilder.Append("/out:");
			stringBuilder.Append("\"");
			stringBuilder.Append(options.OutputAssembly);
			stringBuilder.Append("\"");
			stringBuilder.Append(" ");
			if (options.IncludeDebugInformation)
			{
				stringBuilder.Append("/D:DEBUG ");
				stringBuilder.Append("/debug+ ");
				stringBuilder.Append("/optimize- ");
			}
			else
			{
				stringBuilder.Append("/debug- ");
				stringBuilder.Append("/optimize+ ");
			}
			if (options.Win32Resource != null)
			{
				stringBuilder.Append("/win32res:\"" + options.Win32Resource + "\" ");
			}
			foreach (string text2 in options.EmbeddedResources)
			{
				stringBuilder.Append("/res:\"");
				stringBuilder.Append(text2);
				stringBuilder.Append("\" ");
			}
			foreach (string text3 in options.LinkedResources)
			{
				stringBuilder.Append("/linkres:\"");
				stringBuilder.Append(text3);
				stringBuilder.Append("\" ");
			}
			if (options.TreatWarningsAsErrors)
			{
				stringBuilder.Append("/warnaserror ");
			}
			if (options.WarningLevel >= 0)
			{
				stringBuilder.Append("/w:" + options.WarningLevel + " ");
			}
			if (options.CompilerOptions != null)
			{
				stringBuilder.Append(options.CompilerOptions + " ");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x00048B9C File Offset: 0x00047B9C
		private void ContinueOnNewLine(string st)
		{
			this.Output.WriteLine(st);
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00048BAC File Offset: 0x00047BAC
		private string GetResponseFileCmdArgs(CompilerParameters options, string cmdArgs)
		{
			string text = options.TempFiles.AddExtension("cmdline");
			Stream stream = new FileStream(text, FileMode.Create, FileAccess.Write, FileShare.Read);
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
				{
					streamWriter.Write(cmdArgs);
					streamWriter.Flush();
				}
			}
			finally
			{
				stream.Close();
			}
			return "/noconfig /fullpaths @\"" + text + "\"";
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00048C30 File Offset: 0x00047C30
		private void OutputIdentifier(string ident)
		{
			this.Output.Write(this.CreateEscapedIdentifier(ident));
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00048C44 File Offset: 0x00047C44
		private void OutputType(CodeTypeReference typeRef)
		{
			this.Output.Write(this.GetTypeOutput(typeRef));
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x00048C58 File Offset: 0x00047C58
		private void GenerateArrayCreateExpression(CodeArrayCreateExpression e)
		{
			this.Output.Write("new ");
			CodeExpressionCollection initializers = e.Initializers;
			if (initializers.Count > 0)
			{
				this.OutputType(e.CreateType);
				if (e.CreateType.ArrayRank == 0)
				{
					this.Output.Write("[]");
				}
				this.Output.WriteLine(" {");
				this.Indent++;
				this.OutputExpressionList(initializers, true);
				this.Indent--;
				this.Output.Write("}");
				return;
			}
			this.Output.Write(this.GetBaseTypeOutput(e.CreateType));
			this.Output.Write("[");
			if (e.SizeExpression != null)
			{
				this.GenerateExpression(e.SizeExpression);
			}
			else
			{
				this.Output.Write(e.Size);
			}
			this.Output.Write("]");
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x00048D51 File Offset: 0x00047D51
		private void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e)
		{
			this.Output.Write("base");
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00048D64 File Offset: 0x00047D64
		private void GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression e)
		{
			bool flag = false;
			this.Output.Write("(");
			this.GenerateExpression(e.Left);
			this.Output.Write(" ");
			if (e.Left is CodeBinaryOperatorExpression || e.Right is CodeBinaryOperatorExpression)
			{
				if (!this.inNestedBinary)
				{
					flag = true;
					this.inNestedBinary = true;
					this.Indent += 3;
				}
				this.ContinueOnNewLine("");
			}
			this.OutputOperator(e.Operator);
			this.Output.Write(" ");
			this.GenerateExpression(e.Right);
			this.Output.Write(")");
			if (flag)
			{
				this.Indent -= 3;
				this.inNestedBinary = false;
			}
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x00048E34 File Offset: 0x00047E34
		private void GenerateCastExpression(CodeCastExpression e)
		{
			this.Output.Write("((");
			this.OutputType(e.TargetType);
			this.Output.Write(")(");
			this.GenerateExpression(e.Expression);
			this.Output.Write("))");
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00048E8C File Offset: 0x00047E8C
		public void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
		{
			if (this.output != null)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenReentrance"));
			}
			this.options = ((options == null) ? new CodeGeneratorOptions() : options);
			this.output = new IndentedTextWriter(writer, this.options.IndentString);
			try
			{
				CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration();
				this.currentClass = codeTypeDeclaration;
				this.GenerateTypeMember(member, codeTypeDeclaration);
			}
			finally
			{
				this.currentClass = null;
				this.output = null;
				this.options = null;
			}
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x00048F18 File Offset: 0x00047F18
		private void GenerateDefaultValueExpression(CodeDefaultValueExpression e)
		{
			this.Output.Write("default(");
			this.OutputType(e.Type);
			this.Output.Write(")");
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00048F48 File Offset: 0x00047F48
		private void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e)
		{
			this.Output.Write("new ");
			this.OutputType(e.DelegateType);
			this.Output.Write("(");
			this.GenerateExpression(e.TargetObject);
			this.Output.Write(".");
			this.OutputIdentifier(e.MethodName);
			this.Output.Write(")");
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x00048FBC File Offset: 0x00047FBC
		private void GenerateEvents(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeMemberEvent)
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeMemberEvent codeMemberEvent = (CodeMemberEvent)enumerator.Current;
					if (codeMemberEvent.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeMemberEvent.LinePragma);
					}
					this.GenerateEvent(codeMemberEvent, e);
					if (codeMemberEvent.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeMemberEvent.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x000490B4 File Offset: 0x000480B4
		private void GenerateFields(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeMemberField)
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeMemberField codeMemberField = (CodeMemberField)enumerator.Current;
					if (codeMemberField.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeMemberField.LinePragma);
					}
					this.GenerateField(codeMemberField);
					if (codeMemberField.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeMemberField.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x000491AA File Offset: 0x000481AA
		private void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				this.GenerateExpression(e.TargetObject);
				this.Output.Write(".");
			}
			this.OutputIdentifier(e.FieldName);
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x000491DC File Offset: 0x000481DC
		private void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
		{
			this.OutputIdentifier(e.ParameterName);
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x000491EA File Offset: 0x000481EA
		private void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e)
		{
			this.OutputIdentifier(e.VariableName);
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x000491F8 File Offset: 0x000481F8
		private void GenerateIndexerExpression(CodeIndexerExpression e)
		{
			this.GenerateExpression(e.TargetObject);
			this.Output.Write("[");
			bool flag = true;
			foreach (object obj in e.Indices)
			{
				CodeExpression codeExpression = (CodeExpression)obj;
				if (flag)
				{
					flag = false;
				}
				else
				{
					this.Output.Write(", ");
				}
				this.GenerateExpression(codeExpression);
			}
			this.Output.Write("]");
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00049298 File Offset: 0x00048298
		private void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e)
		{
			this.GenerateExpression(e.TargetObject);
			this.Output.Write("[");
			bool flag = true;
			foreach (object obj in e.Indices)
			{
				CodeExpression codeExpression = (CodeExpression)obj;
				if (flag)
				{
					flag = false;
				}
				else
				{
					this.Output.Write(", ");
				}
				this.GenerateExpression(codeExpression);
			}
			this.Output.Write("]");
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00049338 File Offset: 0x00048338
		private void GenerateSnippetCompileUnit(CodeSnippetCompileUnit e)
		{
			this.GenerateDirectives(e.StartDirectives);
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaStart(e.LinePragma);
			}
			this.Output.WriteLine(e.Value);
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaEnd(e.LinePragma);
			}
			if (e.EndDirectives.Count > 0)
			{
				this.GenerateDirectives(e.EndDirectives);
			}
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x000493A4 File Offset: 0x000483A4
		private void GenerateSnippetExpression(CodeSnippetExpression e)
		{
			this.Output.Write(e.Value);
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x000493B7 File Offset: 0x000483B7
		private void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e)
		{
			this.GenerateMethodReferenceExpression(e.Method);
			this.Output.Write("(");
			this.OutputExpressionList(e.Parameters);
			this.Output.Write(")");
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x000493F4 File Offset: 0x000483F4
		private void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				if (e.TargetObject is CodeBinaryOperatorExpression)
				{
					this.Output.Write("(");
					this.GenerateExpression(e.TargetObject);
					this.Output.Write(")");
				}
				else
				{
					this.GenerateExpression(e.TargetObject);
				}
				this.Output.Write(".");
			}
			this.OutputIdentifier(e.MethodName);
			if (e.TypeArguments.Count > 0)
			{
				this.Output.Write(this.GetTypeArgumentsOutput(e.TypeArguments));
			}
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00049494 File Offset: 0x00048494
		private bool GetUserData(CodeObject e, string property, bool defaultValue)
		{
			object obj = e.UserData[property];
			if (obj != null && obj is bool)
			{
				return (bool)obj;
			}
			return defaultValue;
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x000494C4 File Offset: 0x000484C4
		private void GenerateNamespace(CodeNamespace e)
		{
			this.GenerateCommentStatements(e.Comments);
			this.GenerateNamespaceStart(e);
			if (this.GetUserData(e, "GenerateImports", true))
			{
				this.GenerateNamespaceImports(e);
			}
			this.Output.WriteLine("");
			this.GenerateTypes(e);
			this.GenerateNamespaceEnd(e);
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00049518 File Offset: 0x00048518
		private void GenerateStatement(CodeStatement e)
		{
			if (e.StartDirectives.Count > 0)
			{
				this.GenerateDirectives(e.StartDirectives);
			}
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaStart(e.LinePragma);
			}
			if (e is CodeCommentStatement)
			{
				this.GenerateCommentStatement((CodeCommentStatement)e);
			}
			else if (e is CodeMethodReturnStatement)
			{
				this.GenerateMethodReturnStatement((CodeMethodReturnStatement)e);
			}
			else if (e is CodeConditionStatement)
			{
				this.GenerateConditionStatement((CodeConditionStatement)e);
			}
			else if (e is CodeTryCatchFinallyStatement)
			{
				this.GenerateTryCatchFinallyStatement((CodeTryCatchFinallyStatement)e);
			}
			else if (e is CodeAssignStatement)
			{
				this.GenerateAssignStatement((CodeAssignStatement)e);
			}
			else if (e is CodeExpressionStatement)
			{
				this.GenerateExpressionStatement((CodeExpressionStatement)e);
			}
			else if (e is CodeIterationStatement)
			{
				this.GenerateIterationStatement((CodeIterationStatement)e);
			}
			else if (e is CodeThrowExceptionStatement)
			{
				this.GenerateThrowExceptionStatement((CodeThrowExceptionStatement)e);
			}
			else if (e is CodeSnippetStatement)
			{
				int indent = this.Indent;
				this.Indent = 0;
				this.GenerateSnippetStatement((CodeSnippetStatement)e);
				this.Indent = indent;
			}
			else if (e is CodeVariableDeclarationStatement)
			{
				this.GenerateVariableDeclarationStatement((CodeVariableDeclarationStatement)e);
			}
			else if (e is CodeAttachEventStatement)
			{
				this.GenerateAttachEventStatement((CodeAttachEventStatement)e);
			}
			else if (e is CodeRemoveEventStatement)
			{
				this.GenerateRemoveEventStatement((CodeRemoveEventStatement)e);
			}
			else if (e is CodeGotoStatement)
			{
				this.GenerateGotoStatement((CodeGotoStatement)e);
			}
			else
			{
				if (!(e is CodeLabeledStatement))
				{
					throw new ArgumentException(SR.GetString("InvalidElementType", new object[] { e.GetType().FullName }), "e");
				}
				this.GenerateLabeledStatement((CodeLabeledStatement)e);
			}
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaEnd(e.LinePragma);
			}
			if (e.EndDirectives.Count > 0)
			{
				this.GenerateDirectives(e.EndDirectives);
			}
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00049714 File Offset: 0x00048714
		private void GenerateStatements(CodeStatementCollection stms)
		{
			foreach (object obj in stms)
			{
				((ICodeGenerator)this).GenerateCodeFromStatement((CodeStatement)obj, this.output.InnerWriter, this.options);
			}
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x00049754 File Offset: 0x00048754
		private void GenerateNamespaceImports(CodeNamespace e)
		{
			foreach (object obj in e.Imports)
			{
				CodeNamespaceImport codeNamespaceImport = (CodeNamespaceImport)obj;
				if (codeNamespaceImport.LinePragma != null)
				{
					this.GenerateLinePragmaStart(codeNamespaceImport.LinePragma);
				}
				this.GenerateNamespaceImport(codeNamespaceImport);
				if (codeNamespaceImport.LinePragma != null)
				{
					this.GenerateLinePragmaEnd(codeNamespaceImport.LinePragma);
				}
			}
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x000497B2 File Offset: 0x000487B2
		private void GenerateEventReferenceExpression(CodeEventReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				this.GenerateExpression(e.TargetObject);
				this.Output.Write(".");
			}
			this.OutputIdentifier(e.EventName);
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x000497E4 File Offset: 0x000487E4
		private void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
		{
			if (e.TargetObject != null)
			{
				this.GenerateExpression(e.TargetObject);
			}
			this.Output.Write("(");
			this.OutputExpressionList(e.Parameters);
			this.Output.Write(")");
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00049834 File Offset: 0x00048834
		private void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
		{
			this.Output.Write("new ");
			this.OutputType(e.CreateType);
			this.Output.Write("(");
			this.OutputExpressionList(e.Parameters);
			this.Output.Write(")");
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x0004988C File Offset: 0x0004888C
		private void GeneratePrimitiveExpression(CodePrimitiveExpression e)
		{
			if (e.Value is char)
			{
				this.GeneratePrimitiveChar((char)e.Value);
				return;
			}
			if (e.Value is sbyte)
			{
				this.Output.Write(((sbyte)e.Value).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if (e.Value is ushort)
			{
				this.Output.Write(((ushort)e.Value).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if (e.Value is uint)
			{
				this.Output.Write(((uint)e.Value).ToString(CultureInfo.InvariantCulture));
				this.Output.Write("u");
				return;
			}
			if (e.Value is ulong)
			{
				this.Output.Write(((ulong)e.Value).ToString(CultureInfo.InvariantCulture));
				this.Output.Write("ul");
				return;
			}
			this.GeneratePrimitiveExpressionBase(e);
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x000499A4 File Offset: 0x000489A4
		private void GeneratePrimitiveExpressionBase(CodePrimitiveExpression e)
		{
			if (e.Value == null)
			{
				this.Output.Write(this.NullToken);
				return;
			}
			if (e.Value is string)
			{
				this.Output.Write(this.QuoteSnippetString((string)e.Value));
				return;
			}
			if (e.Value is char)
			{
				this.Output.Write("'" + e.Value.ToString() + "'");
				return;
			}
			if (e.Value is byte)
			{
				this.Output.Write(((byte)e.Value).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if (e.Value is short)
			{
				this.Output.Write(((short)e.Value).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if (e.Value is int)
			{
				this.Output.Write(((int)e.Value).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if (e.Value is long)
			{
				this.Output.Write(((long)e.Value).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if (e.Value is float)
			{
				this.GenerateSingleFloatValue((float)e.Value);
				return;
			}
			if (e.Value is double)
			{
				this.GenerateDoubleValue((double)e.Value);
				return;
			}
			if (e.Value is decimal)
			{
				this.GenerateDecimalValue((decimal)e.Value);
				return;
			}
			if (!(e.Value is bool))
			{
				throw new ArgumentException(SR.GetString("InvalidPrimitiveType", new object[] { e.Value.GetType().ToString() }));
			}
			if ((bool)e.Value)
			{
				this.Output.Write("true");
				return;
			}
			this.Output.Write("false");
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00049BB4 File Offset: 0x00048BB4
		private void GeneratePrimitiveChar(char c)
		{
			this.Output.Write('\'');
			if (c <= '"')
			{
				if (c == '\0')
				{
					this.Output.Write("\\0");
					goto IL_0132;
				}
				switch (c)
				{
				case '\t':
					this.Output.Write("\\t");
					goto IL_0132;
				case '\n':
					this.Output.Write("\\n");
					goto IL_0132;
				case '\v':
				case '\f':
					break;
				case '\r':
					this.Output.Write("\\r");
					goto IL_0132;
				default:
					if (c == '"')
					{
						this.Output.Write("\\\"");
						goto IL_0132;
					}
					break;
				}
			}
			else
			{
				if (c > '\\')
				{
					switch (c)
					{
					case '\u0084':
					case '\u0085':
						break;
					default:
						switch (c)
						{
						case '\u2028':
						case '\u2029':
							break;
						default:
							goto IL_0114;
						}
						break;
					}
					this.AppendEscapedChar(null, c);
					goto IL_0132;
				}
				if (c == '\'')
				{
					this.Output.Write("\\'");
					goto IL_0132;
				}
				if (c == '\\')
				{
					this.Output.Write("\\\\");
					goto IL_0132;
				}
			}
			IL_0114:
			if (char.IsSurrogate(c))
			{
				this.AppendEscapedChar(null, c);
			}
			else
			{
				this.Output.Write(c);
			}
			IL_0132:
			this.Output.Write('\'');
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00049D00 File Offset: 0x00048D00
		private void AppendEscapedChar(StringBuilder b, char value)
		{
			if (b == null)
			{
				this.Output.Write("\\u");
				TextWriter textWriter = this.Output;
				int num = (int)value;
				textWriter.Write(num.ToString("X4", CultureInfo.InvariantCulture));
				return;
			}
			b.Append("\\u");
			int num2 = (int)value;
			b.Append(num2.ToString("X4", CultureInfo.InvariantCulture));
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00049D65 File Offset: 0x00048D65
		private void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e)
		{
			this.Output.Write("value");
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00049D77 File Offset: 0x00048D77
		private void GenerateThisReferenceExpression(CodeThisReferenceExpression e)
		{
			this.Output.Write("this");
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x00049D89 File Offset: 0x00048D89
		private void GenerateExpressionStatement(CodeExpressionStatement e)
		{
			this.GenerateExpression(e.Expression);
			if (!this.generatingForLoop)
			{
				this.Output.WriteLine(";");
			}
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x00049DB0 File Offset: 0x00048DB0
		private void GenerateIterationStatement(CodeIterationStatement e)
		{
			this.generatingForLoop = true;
			this.Output.Write("for (");
			this.GenerateStatement(e.InitStatement);
			this.Output.Write("; ");
			this.GenerateExpression(e.TestExpression);
			this.Output.Write("; ");
			this.GenerateStatement(e.IncrementStatement);
			this.Output.Write(")");
			this.OutputStartingBrace();
			this.generatingForLoop = false;
			this.Indent++;
			this.GenerateStatements(e.Statements);
			this.Indent--;
			this.Output.WriteLine("}");
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x00049E70 File Offset: 0x00048E70
		private void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e)
		{
			this.Output.Write("throw");
			if (e.ToThrow != null)
			{
				this.Output.Write(" ");
				this.GenerateExpression(e.ToThrow);
			}
			this.Output.WriteLine(";");
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00049EC4 File Offset: 0x00048EC4
		private void GenerateComment(CodeComment e)
		{
			string text = (e.DocComment ? "///" : "//");
			this.Output.Write(text);
			this.Output.Write(" ");
			string text2 = e.Text;
			for (int i = 0; i < text2.Length; i++)
			{
				if (text2[i] != '\0')
				{
					this.Output.Write(text2[i]);
					if (text2[i] == '\r')
					{
						if (i < text2.Length - 1 && text2[i + 1] == '\n')
						{
							this.Output.Write('\n');
							i++;
						}
						((IndentedTextWriter)this.Output).InternalOutputTabs();
						this.Output.Write(text);
					}
					else if (text2[i] == '\n')
					{
						((IndentedTextWriter)this.Output).InternalOutputTabs();
						this.Output.Write(text);
					}
					else if (text2[i] == '\u2028' || text2[i] == '\u2029' || text2[i] == '\u0085')
					{
						this.Output.Write(text);
					}
				}
			}
			this.Output.WriteLine();
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x00049FFA File Offset: 0x00048FFA
		private void GenerateCommentStatement(CodeCommentStatement e)
		{
			this.GenerateComment(e.Comment);
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x0004A008 File Offset: 0x00049008
		private void GenerateCommentStatements(CodeCommentStatementCollection e)
		{
			foreach (object obj in e)
			{
				CodeCommentStatement codeCommentStatement = (CodeCommentStatement)obj;
				this.GenerateCommentStatement(codeCommentStatement);
			}
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x0004A05C File Offset: 0x0004905C
		private void GenerateMethodReturnStatement(CodeMethodReturnStatement e)
		{
			this.Output.Write("return");
			if (e.Expression != null)
			{
				this.Output.Write(" ");
				this.GenerateExpression(e.Expression);
			}
			this.Output.WriteLine(";");
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x0004A0B0 File Offset: 0x000490B0
		private void GenerateConditionStatement(CodeConditionStatement e)
		{
			this.Output.Write("if (");
			this.GenerateExpression(e.Condition);
			this.Output.Write(")");
			this.OutputStartingBrace();
			this.Indent++;
			this.GenerateStatements(e.TrueStatements);
			this.Indent--;
			CodeStatementCollection falseStatements = e.FalseStatements;
			if (falseStatements.Count > 0)
			{
				this.Output.Write("}");
				if (this.Options.ElseOnClosing)
				{
					this.Output.Write(" ");
				}
				else
				{
					this.Output.WriteLine("");
				}
				this.Output.Write("else");
				this.OutputStartingBrace();
				this.Indent++;
				this.GenerateStatements(e.FalseStatements);
				this.Indent--;
			}
			this.Output.WriteLine("}");
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x0004A1B4 File Offset: 0x000491B4
		private void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
		{
			this.Output.Write("try");
			this.OutputStartingBrace();
			this.Indent++;
			this.GenerateStatements(e.TryStatements);
			this.Indent--;
			CodeCatchClauseCollection catchClauses = e.CatchClauses;
			if (catchClauses.Count > 0)
			{
				IEnumerator enumerator = catchClauses.GetEnumerator();
				while (enumerator.MoveNext())
				{
					this.Output.Write("}");
					if (this.Options.ElseOnClosing)
					{
						this.Output.Write(" ");
					}
					else
					{
						this.Output.WriteLine("");
					}
					CodeCatchClause codeCatchClause = (CodeCatchClause)enumerator.Current;
					this.Output.Write("catch (");
					this.OutputType(codeCatchClause.CatchExceptionType);
					this.Output.Write(" ");
					this.OutputIdentifier(codeCatchClause.LocalName);
					this.Output.Write(")");
					this.OutputStartingBrace();
					this.Indent++;
					this.GenerateStatements(codeCatchClause.Statements);
					this.Indent--;
				}
			}
			CodeStatementCollection finallyStatements = e.FinallyStatements;
			if (finallyStatements.Count > 0)
			{
				this.Output.Write("}");
				if (this.Options.ElseOnClosing)
				{
					this.Output.Write(" ");
				}
				else
				{
					this.Output.WriteLine("");
				}
				this.Output.Write("finally");
				this.OutputStartingBrace();
				this.Indent++;
				this.GenerateStatements(finallyStatements);
				this.Indent--;
			}
			this.Output.WriteLine("}");
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0004A384 File Offset: 0x00049384
		private void GenerateAssignStatement(CodeAssignStatement e)
		{
			this.GenerateExpression(e.Left);
			this.Output.Write(" = ");
			this.GenerateExpression(e.Right);
			if (!this.generatingForLoop)
			{
				this.Output.WriteLine(";");
			}
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x0004A3D1 File Offset: 0x000493D1
		private void GenerateAttachEventStatement(CodeAttachEventStatement e)
		{
			this.GenerateEventReferenceExpression(e.Event);
			this.Output.Write(" += ");
			this.GenerateExpression(e.Listener);
			this.Output.WriteLine(";");
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x0004A40B File Offset: 0x0004940B
		private void GenerateRemoveEventStatement(CodeRemoveEventStatement e)
		{
			this.GenerateEventReferenceExpression(e.Event);
			this.Output.Write(" -= ");
			this.GenerateExpression(e.Listener);
			this.Output.WriteLine(";");
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x0004A445 File Offset: 0x00049445
		private void GenerateSnippetStatement(CodeSnippetStatement e)
		{
			this.Output.WriteLine(e.Value);
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x0004A458 File Offset: 0x00049458
		private void GenerateGotoStatement(CodeGotoStatement e)
		{
			this.Output.Write("goto ");
			this.Output.Write(e.Label);
			this.Output.WriteLine(";");
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x0004A48C File Offset: 0x0004948C
		private void GenerateLabeledStatement(CodeLabeledStatement e)
		{
			this.Indent--;
			this.Output.Write(e.Label);
			this.Output.WriteLine(":");
			this.Indent++;
			if (e.Statement != null)
			{
				this.GenerateStatement(e.Statement);
			}
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x0004A4EC File Offset: 0x000494EC
		private void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
		{
			this.OutputTypeNamePair(e.Type, e.Name);
			if (e.InitExpression != null)
			{
				this.Output.Write(" = ");
				this.GenerateExpression(e.InitExpression);
			}
			if (!this.generatingForLoop)
			{
				this.Output.WriteLine(";");
			}
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0004A548 File Offset: 0x00049548
		private void GenerateLinePragmaStart(CodeLinePragma e)
		{
			this.Output.WriteLine("");
			this.Output.Write("#line ");
			this.Output.Write(e.LineNumber);
			this.Output.Write(" \"");
			this.Output.Write(e.FileName);
			this.Output.Write("\"");
			this.Output.WriteLine("");
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x0004A5C7 File Offset: 0x000495C7
		private void GenerateLinePragmaEnd(CodeLinePragma e)
		{
			this.Output.WriteLine();
			this.Output.WriteLine("#line default");
			this.Output.WriteLine("#line hidden");
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x0004A5F4 File Offset: 0x000495F4
		private void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c)
		{
			if (this.IsCurrentDelegate || this.IsCurrentEnum)
			{
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.GenerateAttributes(e.CustomAttributes);
			}
			if (e.PrivateImplementationType == null)
			{
				this.OutputMemberAccessModifier(e.Attributes);
			}
			this.Output.Write("event ");
			string text = e.Name;
			if (e.PrivateImplementationType != null)
			{
				text = e.PrivateImplementationType.BaseType + "." + text;
			}
			this.OutputTypeNamePair(e.Type, text);
			this.Output.WriteLine(";");
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x0004A694 File Offset: 0x00049694
		private void GenerateExpression(CodeExpression e)
		{
			if (e is CodeArrayCreateExpression)
			{
				this.GenerateArrayCreateExpression((CodeArrayCreateExpression)e);
				return;
			}
			if (e is CodeBaseReferenceExpression)
			{
				this.GenerateBaseReferenceExpression((CodeBaseReferenceExpression)e);
				return;
			}
			if (e is CodeBinaryOperatorExpression)
			{
				this.GenerateBinaryOperatorExpression((CodeBinaryOperatorExpression)e);
				return;
			}
			if (e is CodeCastExpression)
			{
				this.GenerateCastExpression((CodeCastExpression)e);
				return;
			}
			if (e is CodeDelegateCreateExpression)
			{
				this.GenerateDelegateCreateExpression((CodeDelegateCreateExpression)e);
				return;
			}
			if (e is CodeFieldReferenceExpression)
			{
				this.GenerateFieldReferenceExpression((CodeFieldReferenceExpression)e);
				return;
			}
			if (e is CodeArgumentReferenceExpression)
			{
				this.GenerateArgumentReferenceExpression((CodeArgumentReferenceExpression)e);
				return;
			}
			if (e is CodeVariableReferenceExpression)
			{
				this.GenerateVariableReferenceExpression((CodeVariableReferenceExpression)e);
				return;
			}
			if (e is CodeIndexerExpression)
			{
				this.GenerateIndexerExpression((CodeIndexerExpression)e);
				return;
			}
			if (e is CodeArrayIndexerExpression)
			{
				this.GenerateArrayIndexerExpression((CodeArrayIndexerExpression)e);
				return;
			}
			if (e is CodeSnippetExpression)
			{
				this.GenerateSnippetExpression((CodeSnippetExpression)e);
				return;
			}
			if (e is CodeMethodInvokeExpression)
			{
				this.GenerateMethodInvokeExpression((CodeMethodInvokeExpression)e);
				return;
			}
			if (e is CodeMethodReferenceExpression)
			{
				this.GenerateMethodReferenceExpression((CodeMethodReferenceExpression)e);
				return;
			}
			if (e is CodeEventReferenceExpression)
			{
				this.GenerateEventReferenceExpression((CodeEventReferenceExpression)e);
				return;
			}
			if (e is CodeDelegateInvokeExpression)
			{
				this.GenerateDelegateInvokeExpression((CodeDelegateInvokeExpression)e);
				return;
			}
			if (e is CodeObjectCreateExpression)
			{
				this.GenerateObjectCreateExpression((CodeObjectCreateExpression)e);
				return;
			}
			if (e is CodeParameterDeclarationExpression)
			{
				this.GenerateParameterDeclarationExpression((CodeParameterDeclarationExpression)e);
				return;
			}
			if (e is CodeDirectionExpression)
			{
				this.GenerateDirectionExpression((CodeDirectionExpression)e);
				return;
			}
			if (e is CodePrimitiveExpression)
			{
				this.GeneratePrimitiveExpression((CodePrimitiveExpression)e);
				return;
			}
			if (e is CodePropertyReferenceExpression)
			{
				this.GeneratePropertyReferenceExpression((CodePropertyReferenceExpression)e);
				return;
			}
			if (e is CodePropertySetValueReferenceExpression)
			{
				this.GeneratePropertySetValueReferenceExpression((CodePropertySetValueReferenceExpression)e);
				return;
			}
			if (e is CodeThisReferenceExpression)
			{
				this.GenerateThisReferenceExpression((CodeThisReferenceExpression)e);
				return;
			}
			if (e is CodeTypeReferenceExpression)
			{
				this.GenerateTypeReferenceExpression((CodeTypeReferenceExpression)e);
				return;
			}
			if (e is CodeTypeOfExpression)
			{
				this.GenerateTypeOfExpression((CodeTypeOfExpression)e);
				return;
			}
			if (e is CodeDefaultValueExpression)
			{
				this.GenerateDefaultValueExpression((CodeDefaultValueExpression)e);
				return;
			}
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			throw new ArgumentException(SR.GetString("InvalidElementType", new object[] { e.GetType().FullName }), "e");
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x0004A8E8 File Offset: 0x000498E8
		private void GenerateField(CodeMemberField e)
		{
			if (this.IsCurrentDelegate || this.IsCurrentInterface)
			{
				return;
			}
			if (this.IsCurrentEnum)
			{
				if (e.CustomAttributes.Count > 0)
				{
					this.GenerateAttributes(e.CustomAttributes);
				}
				this.OutputIdentifier(e.Name);
				if (e.InitExpression != null)
				{
					this.Output.Write(" = ");
					this.GenerateExpression(e.InitExpression);
				}
				this.Output.WriteLine(",");
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.GenerateAttributes(e.CustomAttributes);
			}
			this.OutputMemberAccessModifier(e.Attributes);
			this.OutputVTableModifier(e.Attributes);
			this.OutputFieldScopeModifier(e.Attributes);
			this.OutputTypeNamePair(e.Type, e.Name);
			if (e.InitExpression != null)
			{
				this.Output.Write(" = ");
				this.GenerateExpression(e.InitExpression);
			}
			this.Output.WriteLine(";");
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x0004A9ED File Offset: 0x000499ED
		private void GenerateSnippetMember(CodeSnippetTypeMember e)
		{
			this.Output.Write(e.Text);
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x0004AA00 File Offset: 0x00049A00
		private void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.GenerateAttributes(e.CustomAttributes, null, true);
			}
			this.OutputDirection(e.Direction);
			this.OutputTypeNamePair(e.Type, e.Name);
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x0004AA3C File Offset: 0x00049A3C
		private void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.GenerateAttributes(e.CustomAttributes);
			}
			this.Output.Write("public static ");
			this.OutputType(e.ReturnType);
			this.Output.Write(" Main()");
			this.OutputStartingBrace();
			this.Indent++;
			this.GenerateStatements(e.Statements);
			this.Indent--;
			this.Output.WriteLine("}");
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x0004AAD0 File Offset: 0x00049AD0
		private void GenerateMethods(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeMemberMethod && !(enumerator.Current is CodeTypeConstructor) && !(enumerator.Current is CodeConstructor))
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeMemberMethod codeMemberMethod = (CodeMemberMethod)enumerator.Current;
					if (codeMemberMethod.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeMemberMethod.LinePragma);
					}
					if (enumerator.Current is CodeEntryPointMethod)
					{
						this.GenerateEntryPointMethod((CodeEntryPointMethod)enumerator.Current, e);
					}
					else
					{
						this.GenerateMethod(codeMemberMethod, e);
					}
					if (codeMemberMethod.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeMemberMethod.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x0004AC08 File Offset: 0x00049C08
		private void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c)
		{
			if (!this.IsCurrentClass && !this.IsCurrentStruct && !this.IsCurrentInterface)
			{
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.GenerateAttributes(e.CustomAttributes);
			}
			if (e.ReturnTypeCustomAttributes.Count > 0)
			{
				this.GenerateAttributes(e.ReturnTypeCustomAttributes, "return: ");
			}
			if (!this.IsCurrentInterface)
			{
				if (e.PrivateImplementationType == null)
				{
					this.OutputMemberAccessModifier(e.Attributes);
					this.OutputVTableModifier(e.Attributes);
					this.OutputMemberScopeModifier(e.Attributes);
				}
			}
			else
			{
				this.OutputVTableModifier(e.Attributes);
			}
			this.OutputType(e.ReturnType);
			this.Output.Write(" ");
			if (e.PrivateImplementationType != null)
			{
				this.Output.Write(e.PrivateImplementationType.BaseType);
				this.Output.Write(".");
			}
			this.OutputIdentifier(e.Name);
			this.OutputTypeParameters(e.TypeParameters);
			this.Output.Write("(");
			this.OutputParameters(e.Parameters);
			this.Output.Write(")");
			this.OutputTypeParameterConstraints(e.TypeParameters);
			if (!this.IsCurrentInterface && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
			{
				this.OutputStartingBrace();
				this.Indent++;
				this.GenerateStatements(e.Statements);
				this.Indent--;
				this.Output.WriteLine("}");
				return;
			}
			this.Output.WriteLine(";");
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x0004ADA8 File Offset: 0x00049DA8
		private void GenerateProperties(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeMemberProperty)
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeMemberProperty codeMemberProperty = (CodeMemberProperty)enumerator.Current;
					if (codeMemberProperty.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeMemberProperty.LinePragma);
					}
					this.GenerateProperty(codeMemberProperty, e);
					if (codeMemberProperty.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeMemberProperty.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x0004AEA0 File Offset: 0x00049EA0
		private void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c)
		{
			if (!this.IsCurrentClass && !this.IsCurrentStruct && !this.IsCurrentInterface)
			{
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.GenerateAttributes(e.CustomAttributes);
			}
			if (!this.IsCurrentInterface)
			{
				if (e.PrivateImplementationType == null)
				{
					this.OutputMemberAccessModifier(e.Attributes);
					this.OutputVTableModifier(e.Attributes);
					this.OutputMemberScopeModifier(e.Attributes);
				}
			}
			else
			{
				this.OutputVTableModifier(e.Attributes);
			}
			this.OutputType(e.Type);
			this.Output.Write(" ");
			if (e.PrivateImplementationType != null && !this.IsCurrentInterface)
			{
				this.Output.Write(e.PrivateImplementationType.BaseType);
				this.Output.Write(".");
			}
			if (e.Parameters.Count > 0 && string.Compare(e.Name, "Item", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.Output.Write("this[");
				this.OutputParameters(e.Parameters);
				this.Output.Write("]");
			}
			else
			{
				this.OutputIdentifier(e.Name);
			}
			this.OutputStartingBrace();
			this.Indent++;
			if (e.HasGet)
			{
				if (this.IsCurrentInterface || (e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Abstract)
				{
					this.Output.WriteLine("get;");
				}
				else
				{
					this.Output.Write("get");
					this.OutputStartingBrace();
					this.Indent++;
					this.GenerateStatements(e.GetStatements);
					this.Indent--;
					this.Output.WriteLine("}");
				}
			}
			if (e.HasSet)
			{
				if (this.IsCurrentInterface || (e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Abstract)
				{
					this.Output.WriteLine("set;");
				}
				else
				{
					this.Output.Write("set");
					this.OutputStartingBrace();
					this.Indent++;
					this.GenerateStatements(e.SetStatements);
					this.Indent--;
					this.Output.WriteLine("}");
				}
			}
			this.Indent--;
			this.Output.WriteLine("}");
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x0004B0FC File Offset: 0x0004A0FC
		private void GenerateSingleFloatValue(float s)
		{
			if (float.IsNaN(s))
			{
				this.Output.Write("float.NaN");
				return;
			}
			if (float.IsNegativeInfinity(s))
			{
				this.Output.Write("float.NegativeInfinity");
				return;
			}
			if (float.IsPositiveInfinity(s))
			{
				this.Output.Write("float.PositiveInfinity");
				return;
			}
			this.Output.Write(s.ToString(CultureInfo.InvariantCulture));
			this.Output.Write('F');
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x0004B178 File Offset: 0x0004A178
		private void GenerateDoubleValue(double d)
		{
			if (double.IsNaN(d))
			{
				this.Output.Write("double.NaN");
				return;
			}
			if (double.IsNegativeInfinity(d))
			{
				this.Output.Write("double.NegativeInfinity");
				return;
			}
			if (double.IsPositiveInfinity(d))
			{
				this.Output.Write("double.PositiveInfinity");
				return;
			}
			this.Output.Write(d.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x0004B1EC File Offset: 0x0004A1EC
		private void GenerateDecimalValue(decimal d)
		{
			this.Output.Write(d.ToString(CultureInfo.InvariantCulture));
			this.Output.Write('m');
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x0004B214 File Offset: 0x0004A214
		private void OutputVTableModifier(MemberAttributes attributes)
		{
			MemberAttributes memberAttributes = attributes & MemberAttributes.VTableMask;
			if (memberAttributes != MemberAttributes.New)
			{
				return;
			}
			this.Output.Write("new ");
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x0004B240 File Offset: 0x0004A240
		private void OutputMemberAccessModifier(MemberAttributes attributes)
		{
			MemberAttributes memberAttributes = attributes & MemberAttributes.AccessMask;
			if (memberAttributes <= MemberAttributes.Family)
			{
				if (memberAttributes == MemberAttributes.Assembly)
				{
					this.Output.Write("internal ");
					return;
				}
				if (memberAttributes == MemberAttributes.FamilyAndAssembly)
				{
					this.Output.Write("internal ");
					return;
				}
				if (memberAttributes != MemberAttributes.Family)
				{
					return;
				}
				this.Output.Write("protected ");
				return;
			}
			else
			{
				if (memberAttributes == MemberAttributes.FamilyOrAssembly)
				{
					this.Output.Write("protected internal ");
					return;
				}
				if (memberAttributes == MemberAttributes.Private)
				{
					this.Output.Write("private ");
					return;
				}
				if (memberAttributes != MemberAttributes.Public)
				{
					return;
				}
				this.Output.Write("public ");
				return;
			}
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0004B2F4 File Offset: 0x0004A2F4
		private void OutputMemberScopeModifier(MemberAttributes attributes)
		{
			switch (attributes & MemberAttributes.ScopeMask)
			{
			case MemberAttributes.Abstract:
				this.Output.Write("abstract ");
				return;
			case MemberAttributes.Final:
				this.Output.Write("");
				return;
			case MemberAttributes.Static:
				this.Output.Write("static ");
				return;
			case MemberAttributes.Override:
				this.Output.Write("override ");
				return;
			default:
			{
				MemberAttributes memberAttributes = attributes & MemberAttributes.AccessMask;
				if (memberAttributes != MemberAttributes.Assembly && memberAttributes != MemberAttributes.Family && memberAttributes != MemberAttributes.Public)
				{
					return;
				}
				this.Output.Write("virtual ");
				return;
			}
			}
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x0004B398 File Offset: 0x0004A398
		private void OutputOperator(CodeBinaryOperatorType op)
		{
			switch (op)
			{
			case CodeBinaryOperatorType.Add:
				this.Output.Write("+");
				return;
			case CodeBinaryOperatorType.Subtract:
				this.Output.Write("-");
				return;
			case CodeBinaryOperatorType.Multiply:
				this.Output.Write("*");
				return;
			case CodeBinaryOperatorType.Divide:
				this.Output.Write("/");
				return;
			case CodeBinaryOperatorType.Modulus:
				this.Output.Write("%");
				return;
			case CodeBinaryOperatorType.Assign:
				this.Output.Write("=");
				return;
			case CodeBinaryOperatorType.IdentityInequality:
				this.Output.Write("!=");
				return;
			case CodeBinaryOperatorType.IdentityEquality:
				this.Output.Write("==");
				return;
			case CodeBinaryOperatorType.ValueEquality:
				this.Output.Write("==");
				return;
			case CodeBinaryOperatorType.BitwiseOr:
				this.Output.Write("|");
				return;
			case CodeBinaryOperatorType.BitwiseAnd:
				this.Output.Write("&");
				return;
			case CodeBinaryOperatorType.BooleanOr:
				this.Output.Write("||");
				return;
			case CodeBinaryOperatorType.BooleanAnd:
				this.Output.Write("&&");
				return;
			case CodeBinaryOperatorType.LessThan:
				this.Output.Write("<");
				return;
			case CodeBinaryOperatorType.LessThanOrEqual:
				this.Output.Write("<=");
				return;
			case CodeBinaryOperatorType.GreaterThan:
				this.Output.Write(">");
				return;
			case CodeBinaryOperatorType.GreaterThanOrEqual:
				this.Output.Write(">=");
				return;
			default:
				return;
			}
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x0004B514 File Offset: 0x0004A514
		private void OutputFieldScopeModifier(MemberAttributes attributes)
		{
			switch (attributes & MemberAttributes.ScopeMask)
			{
			case MemberAttributes.Final:
			case MemberAttributes.Override:
				break;
			case MemberAttributes.Static:
				this.Output.Write("static ");
				return;
			case MemberAttributes.Const:
				this.Output.Write("const ");
				break;
			default:
				return;
			}
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x0004B560 File Offset: 0x0004A560
		private void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				this.GenerateExpression(e.TargetObject);
				this.Output.Write(".");
			}
			this.OutputIdentifier(e.PropertyName);
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x0004B594 File Offset: 0x0004A594
		private void GenerateConstructors(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeConstructor)
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeConstructor codeConstructor = (CodeConstructor)enumerator.Current;
					if (codeConstructor.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeConstructor.LinePragma);
					}
					this.GenerateConstructor(codeConstructor, e);
					if (codeConstructor.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeConstructor.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x0004B68C File Offset: 0x0004A68C
		private void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c)
		{
			if (!this.IsCurrentClass && !this.IsCurrentStruct)
			{
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.GenerateAttributes(e.CustomAttributes);
			}
			this.OutputMemberAccessModifier(e.Attributes);
			this.OutputIdentifier(this.CurrentTypeName);
			this.Output.Write("(");
			this.OutputParameters(e.Parameters);
			this.Output.Write(")");
			CodeExpressionCollection baseConstructorArgs = e.BaseConstructorArgs;
			CodeExpressionCollection chainedConstructorArgs = e.ChainedConstructorArgs;
			if (baseConstructorArgs.Count > 0)
			{
				this.Output.WriteLine(" : ");
				this.Indent++;
				this.Indent++;
				this.Output.Write("base(");
				this.OutputExpressionList(baseConstructorArgs);
				this.Output.Write(")");
				this.Indent--;
				this.Indent--;
			}
			if (chainedConstructorArgs.Count > 0)
			{
				this.Output.WriteLine(" : ");
				this.Indent++;
				this.Indent++;
				this.Output.Write("this(");
				this.OutputExpressionList(chainedConstructorArgs);
				this.Output.Write(")");
				this.Indent--;
				this.Indent--;
			}
			this.OutputStartingBrace();
			this.Indent++;
			this.GenerateStatements(e.Statements);
			this.Indent--;
			this.Output.WriteLine("}");
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x0004B844 File Offset: 0x0004A844
		private void GenerateTypeConstructor(CodeTypeConstructor e)
		{
			if (!this.IsCurrentClass && !this.IsCurrentStruct)
			{
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.GenerateAttributes(e.CustomAttributes);
			}
			this.Output.Write("static ");
			this.Output.Write(this.CurrentTypeName);
			this.Output.Write("()");
			this.OutputStartingBrace();
			this.Indent++;
			this.GenerateStatements(e.Statements);
			this.Indent--;
			this.Output.WriteLine("}");
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x0004B8EB File Offset: 0x0004A8EB
		private void GenerateTypeReferenceExpression(CodeTypeReferenceExpression e)
		{
			this.OutputType(e.Type);
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x0004B8F9 File Offset: 0x0004A8F9
		private void GenerateTypeOfExpression(CodeTypeOfExpression e)
		{
			this.Output.Write("typeof(");
			this.OutputType(e.Type);
			this.Output.Write(")");
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x0004B928 File Offset: 0x0004A928
		private void GenerateType(CodeTypeDeclaration e)
		{
			this.currentClass = e;
			if (e.StartDirectives.Count > 0)
			{
				this.GenerateDirectives(e.StartDirectives);
			}
			this.GenerateCommentStatements(e.Comments);
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaStart(e.LinePragma);
			}
			this.GenerateTypeStart(e);
			if (this.Options.VerbatimOrder)
			{
				using (IEnumerator enumerator = e.Members.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						CodeTypeMember codeTypeMember = (CodeTypeMember)obj;
						this.GenerateTypeMember(codeTypeMember, e);
					}
					goto IL_00CA;
				}
			}
			this.GenerateFields(e);
			this.GenerateSnippetMembers(e);
			this.GenerateTypeConstructors(e);
			this.GenerateConstructors(e);
			this.GenerateProperties(e);
			this.GenerateEvents(e);
			this.GenerateMethods(e);
			this.GenerateNestedTypes(e);
			IL_00CA:
			this.currentClass = e;
			this.GenerateTypeEnd(e);
			if (e.LinePragma != null)
			{
				this.GenerateLinePragmaEnd(e.LinePragma);
			}
			if (e.EndDirectives.Count > 0)
			{
				this.GenerateDirectives(e.EndDirectives);
			}
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x0004BA4C File Offset: 0x0004AA4C
		private void GenerateTypes(CodeNamespace e)
		{
			foreach (object obj in e.Types)
			{
				CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)obj;
				if (this.options.BlankLinesBetweenMembers)
				{
					this.Output.WriteLine();
				}
				((ICodeGenerator)this).GenerateCodeFromType(codeTypeDeclaration, this.output.InnerWriter, this.options);
			}
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x0004BAD0 File Offset: 0x0004AAD0
		private void GenerateTypeStart(CodeTypeDeclaration e)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.GenerateAttributes(e.CustomAttributes);
			}
			if (this.IsCurrentDelegate)
			{
				switch (e.TypeAttributes & TypeAttributes.VisibilityMask)
				{
				case TypeAttributes.Public:
					this.Output.Write("public ");
					break;
				}
				CodeTypeDelegate codeTypeDelegate = (CodeTypeDelegate)e;
				this.Output.Write("delegate ");
				this.OutputType(codeTypeDelegate.ReturnType);
				this.Output.Write(" ");
				this.OutputIdentifier(e.Name);
				this.Output.Write("(");
				this.OutputParameters(codeTypeDelegate.Parameters);
				this.Output.WriteLine(");");
				return;
			}
			this.OutputTypeAttributes(e);
			this.OutputIdentifier(e.Name);
			this.OutputTypeParameters(e.TypeParameters);
			bool flag = true;
			foreach (object obj in e.BaseTypes)
			{
				CodeTypeReference codeTypeReference = (CodeTypeReference)obj;
				if (flag)
				{
					this.Output.Write(" : ");
					flag = false;
				}
				else
				{
					this.Output.Write(", ");
				}
				this.OutputType(codeTypeReference);
			}
			this.OutputTypeParameterConstraints(e.TypeParameters);
			this.OutputStartingBrace();
			this.Indent++;
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x0004BC54 File Offset: 0x0004AC54
		private void GenerateTypeMember(CodeTypeMember member, CodeTypeDeclaration declaredType)
		{
			if (this.options.BlankLinesBetweenMembers)
			{
				this.Output.WriteLine();
			}
			if (member is CodeTypeDeclaration)
			{
				((ICodeGenerator)this).GenerateCodeFromType((CodeTypeDeclaration)member, this.output.InnerWriter, this.options);
				this.currentClass = declaredType;
				return;
			}
			if (member.StartDirectives.Count > 0)
			{
				this.GenerateDirectives(member.StartDirectives);
			}
			this.GenerateCommentStatements(member.Comments);
			if (member.LinePragma != null)
			{
				this.GenerateLinePragmaStart(member.LinePragma);
			}
			if (member is CodeMemberField)
			{
				this.GenerateField((CodeMemberField)member);
			}
			else if (member is CodeMemberProperty)
			{
				this.GenerateProperty((CodeMemberProperty)member, declaredType);
			}
			else if (member is CodeMemberMethod)
			{
				if (member is CodeConstructor)
				{
					this.GenerateConstructor((CodeConstructor)member, declaredType);
				}
				else if (member is CodeTypeConstructor)
				{
					this.GenerateTypeConstructor((CodeTypeConstructor)member);
				}
				else if (member is CodeEntryPointMethod)
				{
					this.GenerateEntryPointMethod((CodeEntryPointMethod)member, declaredType);
				}
				else
				{
					this.GenerateMethod((CodeMemberMethod)member, declaredType);
				}
			}
			else if (member is CodeMemberEvent)
			{
				this.GenerateEvent((CodeMemberEvent)member, declaredType);
			}
			else if (member is CodeSnippetTypeMember)
			{
				int indent = this.Indent;
				this.Indent = 0;
				this.GenerateSnippetMember((CodeSnippetTypeMember)member);
				this.Indent = indent;
				this.Output.WriteLine();
			}
			if (member.LinePragma != null)
			{
				this.GenerateLinePragmaEnd(member.LinePragma);
			}
			if (member.EndDirectives.Count > 0)
			{
				this.GenerateDirectives(member.EndDirectives);
			}
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x0004BDEC File Offset: 0x0004ADEC
		private void GenerateTypeConstructors(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeTypeConstructor)
				{
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeTypeConstructor codeTypeConstructor = (CodeTypeConstructor)enumerator.Current;
					if (codeTypeConstructor.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeTypeConstructor.LinePragma);
					}
					this.GenerateTypeConstructor(codeTypeConstructor);
					if (codeTypeConstructor.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeTypeConstructor.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x0004BEE4 File Offset: 0x0004AEE4
		private void GenerateSnippetMembers(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			bool flag = false;
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeSnippetTypeMember)
				{
					flag = true;
					this.currentMember = (CodeTypeMember)enumerator.Current;
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					if (this.currentMember.StartDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.StartDirectives);
					}
					this.GenerateCommentStatements(this.currentMember.Comments);
					CodeSnippetTypeMember codeSnippetTypeMember = (CodeSnippetTypeMember)enumerator.Current;
					if (codeSnippetTypeMember.LinePragma != null)
					{
						this.GenerateLinePragmaStart(codeSnippetTypeMember.LinePragma);
					}
					int indent = this.Indent;
					this.Indent = 0;
					this.GenerateSnippetMember(codeSnippetTypeMember);
					this.Indent = indent;
					if (codeSnippetTypeMember.LinePragma != null)
					{
						this.GenerateLinePragmaEnd(codeSnippetTypeMember.LinePragma);
					}
					if (this.currentMember.EndDirectives.Count > 0)
					{
						this.GenerateDirectives(this.currentMember.EndDirectives);
					}
				}
			}
			if (flag)
			{
				this.Output.WriteLine();
			}
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x0004C004 File Offset: 0x0004B004
		private void GenerateNestedTypes(CodeTypeDeclaration e)
		{
			IEnumerator enumerator = e.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeTypeDeclaration)
				{
					if (this.options.BlankLinesBetweenMembers)
					{
						this.Output.WriteLine();
					}
					CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)enumerator.Current;
					((ICodeGenerator)this).GenerateCodeFromType(codeTypeDeclaration, this.output.InnerWriter, this.options);
				}
			}
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x0004C070 File Offset: 0x0004B070
		private void GenerateNamespaces(CodeCompileUnit e)
		{
			foreach (object obj in e.Namespaces)
			{
				CodeNamespace codeNamespace = (CodeNamespace)obj;
				((ICodeGenerator)this).GenerateCodeFromNamespace(codeNamespace, this.output.InnerWriter, this.options);
			}
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x0004C0DC File Offset: 0x0004B0DC
		private void OutputAttributeArgument(CodeAttributeArgument arg)
		{
			if (arg.Name != null && arg.Name.Length > 0)
			{
				this.OutputIdentifier(arg.Name);
				this.Output.Write("=");
			}
			((ICodeGenerator)this).GenerateCodeFromExpression(arg.Value, this.output.InnerWriter, this.options);
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x0004C138 File Offset: 0x0004B138
		private void OutputDirection(FieldDirection dir)
		{
			switch (dir)
			{
			case FieldDirection.In:
				break;
			case FieldDirection.Out:
				this.Output.Write("out ");
				return;
			case FieldDirection.Ref:
				this.Output.Write("ref ");
				break;
			default:
				return;
			}
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x0004C17B File Offset: 0x0004B17B
		private void OutputExpressionList(CodeExpressionCollection expressions)
		{
			this.OutputExpressionList(expressions, false);
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x0004C188 File Offset: 0x0004B188
		private void OutputExpressionList(CodeExpressionCollection expressions, bool newlineBetweenItems)
		{
			bool flag = true;
			IEnumerator enumerator = expressions.GetEnumerator();
			this.Indent++;
			while (enumerator.MoveNext())
			{
				if (flag)
				{
					flag = false;
				}
				else if (newlineBetweenItems)
				{
					this.ContinueOnNewLine(",");
				}
				else
				{
					this.Output.Write(", ");
				}
				((ICodeGenerator)this).GenerateCodeFromExpression((CodeExpression)enumerator.Current, this.output.InnerWriter, this.options);
			}
			this.Indent--;
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x0004C210 File Offset: 0x0004B210
		private void OutputParameters(CodeParameterDeclarationExpressionCollection parameters)
		{
			bool flag = true;
			bool flag2 = parameters.Count > 15;
			if (flag2)
			{
				this.Indent += 3;
			}
			foreach (object obj in parameters)
			{
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = (CodeParameterDeclarationExpression)obj;
				if (flag)
				{
					flag = false;
				}
				else
				{
					this.Output.Write(", ");
				}
				if (flag2)
				{
					this.ContinueOnNewLine("");
				}
				this.GenerateExpression(codeParameterDeclarationExpression);
			}
			if (flag2)
			{
				this.Indent -= 3;
			}
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x0004C295 File Offset: 0x0004B295
		private void OutputTypeNamePair(CodeTypeReference typeRef, string name)
		{
			this.OutputType(typeRef);
			this.Output.Write(" ");
			this.OutputIdentifier(name);
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x0004C2B8 File Offset: 0x0004B2B8
		private void OutputTypeParameters(CodeTypeParameterCollection typeParameters)
		{
			if (typeParameters.Count == 0)
			{
				return;
			}
			this.Output.Write('<');
			bool flag = true;
			for (int i = 0; i < typeParameters.Count; i++)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					this.Output.Write(", ");
				}
				if (typeParameters[i].CustomAttributes.Count > 0)
				{
					this.GenerateAttributes(typeParameters[i].CustomAttributes, null, true);
					this.Output.Write(' ');
				}
				this.Output.Write(typeParameters[i].Name);
			}
			this.Output.Write('>');
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x0004C360 File Offset: 0x0004B360
		private void OutputTypeParameterConstraints(CodeTypeParameterCollection typeParameters)
		{
			if (typeParameters.Count == 0)
			{
				return;
			}
			for (int i = 0; i < typeParameters.Count; i++)
			{
				this.Output.WriteLine();
				this.Indent++;
				bool flag = true;
				if (typeParameters[i].Constraints.Count > 0)
				{
					foreach (object obj in typeParameters[i].Constraints)
					{
						CodeTypeReference codeTypeReference = (CodeTypeReference)obj;
						if (flag)
						{
							this.Output.Write("where ");
							this.Output.Write(typeParameters[i].Name);
							this.Output.Write(" : ");
							flag = false;
						}
						else
						{
							this.Output.Write(", ");
						}
						this.OutputType(codeTypeReference);
					}
				}
				if (typeParameters[i].HasConstructorConstraint)
				{
					if (flag)
					{
						this.Output.Write("where ");
						this.Output.Write(typeParameters[i].Name);
						this.Output.Write(" : new()");
					}
					else
					{
						this.Output.Write(", new ()");
					}
				}
				this.Indent--;
			}
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x0004C4CC File Offset: 0x0004B4CC
		private void OutputTypeAttributes(CodeTypeDeclaration e)
		{
			if ((e.Attributes & MemberAttributes.New) != (MemberAttributes)0)
			{
				this.Output.Write("new ");
			}
			TypeAttributes typeAttributes = e.TypeAttributes;
			switch (typeAttributes & TypeAttributes.VisibilityMask)
			{
			case TypeAttributes.NotPublic:
			case TypeAttributes.NestedAssembly:
			case TypeAttributes.NestedFamANDAssem:
				this.Output.Write("internal ");
				break;
			case TypeAttributes.Public:
			case TypeAttributes.NestedPublic:
				this.Output.Write("public ");
				break;
			case TypeAttributes.NestedPrivate:
				this.Output.Write("private ");
				break;
			case TypeAttributes.NestedFamily:
				this.Output.Write("protected ");
				break;
			case TypeAttributes.VisibilityMask:
				this.Output.Write("protected internal ");
				break;
			}
			if (e.IsStruct)
			{
				if (e.IsPartial)
				{
					this.Output.Write("partial ");
				}
				this.Output.Write("struct ");
				return;
			}
			if (e.IsEnum)
			{
				this.Output.Write("enum ");
				return;
			}
			TypeAttributes typeAttributes2 = typeAttributes & TypeAttributes.ClassSemanticsMask;
			if (typeAttributes2 == TypeAttributes.NotPublic)
			{
				if ((typeAttributes & TypeAttributes.Sealed) == TypeAttributes.Sealed)
				{
					this.Output.Write("sealed ");
				}
				if ((typeAttributes & TypeAttributes.Abstract) == TypeAttributes.Abstract)
				{
					this.Output.Write("abstract ");
				}
				if (e.IsPartial)
				{
					this.Output.Write("partial ");
				}
				this.Output.Write("class ");
				return;
			}
			if (typeAttributes2 != TypeAttributes.ClassSemanticsMask)
			{
				return;
			}
			if (e.IsPartial)
			{
				this.Output.Write("partial ");
			}
			this.Output.Write("interface ");
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x0004C665 File Offset: 0x0004B665
		private void GenerateTypeEnd(CodeTypeDeclaration e)
		{
			if (!this.IsCurrentDelegate)
			{
				this.Indent--;
				this.Output.WriteLine("}");
			}
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x0004C690 File Offset: 0x0004B690
		private void GenerateNamespaceStart(CodeNamespace e)
		{
			if (e.Name != null && e.Name.Length > 0)
			{
				this.Output.Write("namespace ");
				string[] array = e.Name.Split(new char[] { '.' });
				this.OutputIdentifier(array[0]);
				for (int i = 1; i < array.Length; i++)
				{
					this.Output.Write(".");
					this.OutputIdentifier(array[i]);
				}
				this.OutputStartingBrace();
				this.Indent++;
			}
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x0004C720 File Offset: 0x0004B720
		private void GenerateCompileUnit(CodeCompileUnit e)
		{
			this.GenerateCompileUnitStart(e);
			this.GenerateNamespaces(e);
			this.GenerateCompileUnitEnd(e);
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x0004C738 File Offset: 0x0004B738
		private void GenerateCompileUnitStart(CodeCompileUnit e)
		{
			if (e.StartDirectives.Count > 0)
			{
				this.GenerateDirectives(e.StartDirectives);
			}
			this.Output.WriteLine("//------------------------------------------------------------------------------");
			this.Output.Write("// <");
			this.Output.WriteLine(SR.GetString("AutoGen_Comment_Line1"));
			this.Output.Write("//     ");
			this.Output.WriteLine(SR.GetString("AutoGen_Comment_Line2"));
			this.Output.Write("//     ");
			this.Output.Write(SR.GetString("AutoGen_Comment_Line3"));
			this.Output.WriteLine(Environment.Version.ToString());
			this.Output.WriteLine("//");
			this.Output.Write("//     ");
			this.Output.WriteLine(SR.GetString("AutoGen_Comment_Line4"));
			this.Output.Write("//     ");
			this.Output.WriteLine(SR.GetString("AutoGen_Comment_Line5"));
			this.Output.Write("// </");
			this.Output.WriteLine(SR.GetString("AutoGen_Comment_Line1"));
			this.Output.WriteLine("//------------------------------------------------------------------------------");
			this.Output.WriteLine("");
			SortedList sortedList = new SortedList(StringComparer.Ordinal);
			foreach (object obj in e.Namespaces)
			{
				CodeNamespace codeNamespace = (CodeNamespace)obj;
				if (string.IsNullOrEmpty(codeNamespace.Name))
				{
					codeNamespace.UserData["GenerateImports"] = false;
					foreach (object obj2 in codeNamespace.Imports)
					{
						CodeNamespaceImport codeNamespaceImport = (CodeNamespaceImport)obj2;
						if (!sortedList.Contains(codeNamespaceImport.Namespace))
						{
							sortedList.Add(codeNamespaceImport.Namespace, codeNamespaceImport.Namespace);
						}
					}
				}
			}
			foreach (object obj3 in sortedList.Keys)
			{
				string text = (string)obj3;
				this.Output.Write("using ");
				this.OutputIdentifier(text);
				this.Output.WriteLine(";");
			}
			if (sortedList.Keys.Count > 0)
			{
				this.Output.WriteLine("");
			}
			if (e.AssemblyCustomAttributes.Count > 0)
			{
				this.GenerateAttributes(e.AssemblyCustomAttributes, "assembly: ");
				this.Output.WriteLine("");
			}
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x0004CA38 File Offset: 0x0004BA38
		private void GenerateCompileUnitEnd(CodeCompileUnit e)
		{
			if (e.EndDirectives.Count > 0)
			{
				this.GenerateDirectives(e.EndDirectives);
			}
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x0004CA54 File Offset: 0x0004BA54
		private void GenerateDirectionExpression(CodeDirectionExpression e)
		{
			this.OutputDirection(e.Direction);
			this.GenerateExpression(e.Expression);
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x0004CA70 File Offset: 0x0004BA70
		private void GenerateDirectives(CodeDirectiveCollection directives)
		{
			for (int i = 0; i < directives.Count; i++)
			{
				CodeDirective codeDirective = directives[i];
				if (codeDirective is CodeChecksumPragma)
				{
					this.GenerateChecksumPragma((CodeChecksumPragma)codeDirective);
				}
				else if (codeDirective is CodeRegionDirective)
				{
					this.GenerateCodeRegionDirective((CodeRegionDirective)codeDirective);
				}
			}
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x0004CAC0 File Offset: 0x0004BAC0
		private void GenerateChecksumPragma(CodeChecksumPragma checksumPragma)
		{
			this.Output.Write("#pragma checksum \"");
			this.Output.Write(checksumPragma.FileName);
			this.Output.Write("\" \"");
			this.Output.Write(checksumPragma.ChecksumAlgorithmId.ToString("B", CultureInfo.InvariantCulture));
			this.Output.Write("\" \"");
			if (checksumPragma.ChecksumData != null)
			{
				foreach (byte b in checksumPragma.ChecksumData)
				{
					this.Output.Write(b.ToString("X2", CultureInfo.InvariantCulture));
				}
			}
			this.Output.WriteLine("\"");
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x0004CB80 File Offset: 0x0004BB80
		private void GenerateCodeRegionDirective(CodeRegionDirective regionDirective)
		{
			if (regionDirective.RegionMode == CodeRegionMode.Start)
			{
				this.Output.Write("#region ");
				this.Output.WriteLine(regionDirective.RegionText);
				return;
			}
			if (regionDirective.RegionMode == CodeRegionMode.End)
			{
				this.Output.WriteLine("#endregion");
			}
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x0004CBD1 File Offset: 0x0004BBD1
		private void GenerateNamespaceEnd(CodeNamespace e)
		{
			if (e.Name != null && e.Name.Length > 0)
			{
				this.Indent--;
				this.Output.WriteLine("}");
			}
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x0004CC07 File Offset: 0x0004BC07
		private void GenerateNamespaceImport(CodeNamespaceImport e)
		{
			this.Output.Write("using ");
			this.OutputIdentifier(e.Namespace);
			this.Output.WriteLine(";");
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x0004CC35 File Offset: 0x0004BC35
		private void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes)
		{
			this.Output.Write("[");
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x0004CC47 File Offset: 0x0004BC47
		private void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes)
		{
			this.Output.Write("]");
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x0004CC59 File Offset: 0x0004BC59
		private void GenerateAttributes(CodeAttributeDeclarationCollection attributes)
		{
			this.GenerateAttributes(attributes, null, false);
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x0004CC64 File Offset: 0x0004BC64
		private void GenerateAttributes(CodeAttributeDeclarationCollection attributes, string prefix)
		{
			this.GenerateAttributes(attributes, prefix, false);
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x0004CC70 File Offset: 0x0004BC70
		private void GenerateAttributes(CodeAttributeDeclarationCollection attributes, string prefix, bool inLine)
		{
			if (attributes.Count == 0)
			{
				return;
			}
			IEnumerator enumerator = attributes.GetEnumerator();
			bool flag = false;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				CodeAttributeDeclaration codeAttributeDeclaration = (CodeAttributeDeclaration)obj;
				if (codeAttributeDeclaration.Name.Equals("system.paramarrayattribute", StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
				}
				else
				{
					this.GenerateAttributeDeclarationsStart(attributes);
					if (prefix != null)
					{
						this.Output.Write(prefix);
					}
					if (codeAttributeDeclaration.AttributeType != null)
					{
						this.Output.Write(this.GetTypeOutput(codeAttributeDeclaration.AttributeType));
					}
					this.Output.Write("(");
					bool flag2 = true;
					foreach (object obj2 in codeAttributeDeclaration.Arguments)
					{
						CodeAttributeArgument codeAttributeArgument = (CodeAttributeArgument)obj2;
						if (flag2)
						{
							flag2 = false;
						}
						else
						{
							this.Output.Write(", ");
						}
						this.OutputAttributeArgument(codeAttributeArgument);
					}
					this.Output.Write(")");
					this.GenerateAttributeDeclarationsEnd(attributes);
					if (inLine)
					{
						this.Output.Write(" ");
					}
					else
					{
						this.Output.WriteLine();
					}
				}
			}
			if (flag)
			{
				if (prefix != null)
				{
					this.Output.Write(prefix);
				}
				this.Output.Write("params");
				if (inLine)
				{
					this.Output.Write(" ");
					return;
				}
				this.Output.WriteLine();
			}
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x0004CDF0 File Offset: 0x0004BDF0
		private static bool IsKeyword(string value)
		{
			return FixedStringLookup.Contains(CSharpCodeGenerator.keywords, value, false);
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x0004CDFE File Offset: 0x0004BDFE
		private static bool IsPrefixTwoUnderscore(string value)
		{
			return value.Length >= 3 && (value[0] == '_' && value[1] == '_') && value[2] != '_';
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x0004CE31 File Offset: 0x0004BE31
		public bool Supports(GeneratorSupport support)
		{
			return (support & (GeneratorSupport.ArraysOfArrays | GeneratorSupport.EntryPointMethod | GeneratorSupport.GotoStatements | GeneratorSupport.MultidimensionalArrays | GeneratorSupport.StaticConstructors | GeneratorSupport.TryCatchStatements | GeneratorSupport.ReturnTypeAttributes | GeneratorSupport.DeclareValueTypes | GeneratorSupport.DeclareEnums | GeneratorSupport.DeclareDelegates | GeneratorSupport.DeclareInterfaces | GeneratorSupport.DeclareEvents | GeneratorSupport.AssemblyAttributes | GeneratorSupport.ParameterAttributes | GeneratorSupport.ReferenceParameters | GeneratorSupport.ChainedConstructorArguments | GeneratorSupport.NestedTypes | GeneratorSupport.MultipleInterfaceMembers | GeneratorSupport.PublicStaticMembers | GeneratorSupport.ComplexExpressions | GeneratorSupport.Win32Resources | GeneratorSupport.Resources | GeneratorSupport.PartialTypes | GeneratorSupport.GenericTypeReference | GeneratorSupport.GenericTypeDeclaration | GeneratorSupport.DeclareIndexerProperties)) == support;
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x0004CE40 File Offset: 0x0004BE40
		public bool IsValidIdentifier(string value)
		{
			if (value == null || value.Length == 0)
			{
				return false;
			}
			if (value.Length > 512)
			{
				return false;
			}
			if (value[0] != '@')
			{
				if (CSharpCodeGenerator.IsKeyword(value))
				{
					return false;
				}
			}
			else
			{
				value = value.Substring(1);
			}
			return CodeGenerator.IsValidLanguageIndependentIdentifier(value);
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x0004CE90 File Offset: 0x0004BE90
		public void ValidateIdentifier(string value)
		{
			if (!this.IsValidIdentifier(value))
			{
				throw new ArgumentException(SR.GetString("InvalidIdentifier", new object[] { value }));
			}
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x0004CEC2 File Offset: 0x0004BEC2
		public string CreateValidIdentifier(string name)
		{
			if (CSharpCodeGenerator.IsPrefixTwoUnderscore(name))
			{
				name = "_" + name;
			}
			while (CSharpCodeGenerator.IsKeyword(name))
			{
				name = "_" + name;
			}
			return name;
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x0004CEF1 File Offset: 0x0004BEF1
		public string CreateEscapedIdentifier(string name)
		{
			if (CSharpCodeGenerator.IsKeyword(name) || CSharpCodeGenerator.IsPrefixTwoUnderscore(name))
			{
				return "@" + name;
			}
			return name;
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x0004CF10 File Offset: 0x0004BF10
		private string GetBaseTypeOutput(CodeTypeReference typeRef)
		{
			string text = typeRef.BaseType;
			if (text.Length == 0)
			{
				return "void";
			}
			string text2 = text.ToLower(CultureInfo.InvariantCulture);
			string text3;
			if ((text3 = text2) != null)
			{
				if (<PrivateImplementationDetails>{AC337EB4-A302-4BEE-BACD-AD51F79CA690}.$$method0x6001706-1 == null)
				{
					<PrivateImplementationDetails>{AC337EB4-A302-4BEE-BACD-AD51F79CA690}.$$method0x6001706-1 = new Dictionary<string, int>(16)
					{
						{ "system.int16", 0 },
						{ "system.int32", 1 },
						{ "system.int64", 2 },
						{ "system.string", 3 },
						{ "system.object", 4 },
						{ "system.boolean", 5 },
						{ "system.void", 6 },
						{ "system.char", 7 },
						{ "system.byte", 8 },
						{ "system.uint16", 9 },
						{ "system.uint32", 10 },
						{ "system.uint64", 11 },
						{ "system.sbyte", 12 },
						{ "system.single", 13 },
						{ "system.double", 14 },
						{ "system.decimal", 15 }
					};
				}
				int num;
				if (<PrivateImplementationDetails>{AC337EB4-A302-4BEE-BACD-AD51F79CA690}.$$method0x6001706-1.TryGetValue(text3, out num))
				{
					switch (num)
					{
					case 0:
						text = "short";
						break;
					case 1:
						text = "int";
						break;
					case 2:
						text = "long";
						break;
					case 3:
						text = "string";
						break;
					case 4:
						text = "object";
						break;
					case 5:
						text = "bool";
						break;
					case 6:
						text = "void";
						break;
					case 7:
						text = "char";
						break;
					case 8:
						text = "byte";
						break;
					case 9:
						text = "ushort";
						break;
					case 10:
						text = "uint";
						break;
					case 11:
						text = "ulong";
						break;
					case 12:
						text = "sbyte";
						break;
					case 13:
						text = "float";
						break;
					case 14:
						text = "double";
						break;
					case 15:
						text = "decimal";
						break;
					default:
						goto IL_021E;
					}
					return text;
				}
			}
			IL_021E:
			StringBuilder stringBuilder = new StringBuilder(text.Length + 10);
			if (typeRef.Options == CodeTypeReferenceOptions.GlobalReference)
			{
				stringBuilder.Append("global::");
			}
			string baseType = typeRef.BaseType;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < baseType.Length; i++)
			{
				char c = baseType[i];
				if (c != '+' && c != '.')
				{
					if (c == '`')
					{
						stringBuilder.Append(this.CreateEscapedIdentifier(baseType.Substring(num2, i - num2)));
						i++;
						int num4 = 0;
						while (i < baseType.Length && baseType[i] >= '0' && baseType[i] <= '9')
						{
							num4 = num4 * 10 + (int)(baseType[i] - '0');
							i++;
						}
						this.GetTypeArgumentsOutput(typeRef.TypeArguments, num3, num4, stringBuilder);
						num3 += num4;
						if (i < baseType.Length && (baseType[i] == '+' || baseType[i] == '.'))
						{
							stringBuilder.Append('.');
							i++;
						}
						num2 = i;
					}
				}
				else
				{
					stringBuilder.Append(this.CreateEscapedIdentifier(baseType.Substring(num2, i - num2)));
					stringBuilder.Append('.');
					i++;
					num2 = i;
				}
			}
			if (num2 < baseType.Length)
			{
				stringBuilder.Append(this.CreateEscapedIdentifier(baseType.Substring(num2)));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x0004D2B0 File Offset: 0x0004C2B0
		private string GetTypeArgumentsOutput(CodeTypeReferenceCollection typeArguments)
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			this.GetTypeArgumentsOutput(typeArguments, 0, typeArguments.Count, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x0004D2E0 File Offset: 0x0004C2E0
		private void GetTypeArgumentsOutput(CodeTypeReferenceCollection typeArguments, int start, int length, StringBuilder sb)
		{
			sb.Append('<');
			bool flag = true;
			for (int i = start; i < start + length; i++)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					sb.Append(", ");
				}
				if (i < typeArguments.Count)
				{
					sb.Append(this.GetTypeOutput(typeArguments[i]));
				}
			}
			sb.Append('>');
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x0004D344 File Offset: 0x0004C344
		public string GetTypeOutput(CodeTypeReference typeRef)
		{
			string text = string.Empty;
			CodeTypeReference codeTypeReference = typeRef;
			while (codeTypeReference.ArrayElementType != null)
			{
				codeTypeReference = codeTypeReference.ArrayElementType;
			}
			text += this.GetBaseTypeOutput(codeTypeReference);
			while (typeRef != null && typeRef.ArrayRank > 0)
			{
				char[] array = new char[typeRef.ArrayRank + 1];
				array[0] = '[';
				array[typeRef.ArrayRank] = ']';
				for (int i = 1; i < typeRef.ArrayRank; i++)
				{
					array[i] = ',';
				}
				text += new string(array);
				typeRef = typeRef.ArrayElementType;
			}
			return text;
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x0004D3D0 File Offset: 0x0004C3D0
		private void OutputStartingBrace()
		{
			if (this.Options.BracingStyle == "C")
			{
				this.Output.WriteLine("");
				this.Output.WriteLine("{");
				return;
			}
			this.Output.WriteLine(" {");
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x0004D428 File Offset: 0x0004C428
		private CompilerResults FromFileBatch(CompilerParameters options, string[] fileNames)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			string text = null;
			int num = 0;
			CompilerResults compilerResults = new CompilerResults(options.TempFiles);
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.ControlEvidence);
			securityPermission.Assert();
			try
			{
				compilerResults.Evidence = options.Evidence;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			bool flag = false;
			if (options.OutputAssembly == null || options.OutputAssembly.Length == 0)
			{
				string text2 = (options.GenerateExecutable ? "exe" : "dll");
				options.OutputAssembly = compilerResults.TempFiles.AddExtension(text2, !options.GenerateInMemory);
				new FileStream(options.OutputAssembly, FileMode.Create, FileAccess.ReadWrite).Close();
				flag = true;
			}
			string text3 = "pdb";
			if (options.CompilerOptions != null && CultureInfo.InvariantCulture.CompareInfo.IndexOf(options.CompilerOptions, "/debug:pdbonly", CompareOptions.IgnoreCase) != -1)
			{
				compilerResults.TempFiles.AddExtension(text3, true);
			}
			else
			{
				compilerResults.TempFiles.AddExtension(text3);
			}
			string text4 = this.CmdArgsFromParameters(options) + " " + CSharpCodeGenerator.JoinStringArray(fileNames, " ");
			string responseFileCmdArgs = this.GetResponseFileCmdArgs(options, text4);
			string text5 = null;
			if (responseFileCmdArgs != null)
			{
				text5 = text4;
				text4 = responseFileCmdArgs;
			}
			this.Compile(options, RedistVersionInfo.GetCompilerPath(this.provOptions, this.CompilerName), this.CompilerName, text4, ref text, ref num, text5);
			compilerResults.NativeCompilerReturnValue = num;
			if (num != 0 || options.WarningLevel > 0)
			{
				FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				try
				{
					if (fileStream.Length > 0L)
					{
						StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
						string text6;
						do
						{
							text6 = streamReader.ReadLine();
							if (text6 != null)
							{
								compilerResults.Output.Add(text6);
								this.ProcessCompilerOutputLine(compilerResults, text6);
							}
						}
						while (text6 != null);
					}
				}
				finally
				{
					fileStream.Close();
				}
				if (num != 0 && flag)
				{
					File.Delete(options.OutputAssembly);
				}
			}
			if (!compilerResults.Errors.HasErrors && options.GenerateInMemory)
			{
				FileStream fileStream2 = new FileStream(options.OutputAssembly, FileMode.Open, FileAccess.Read, FileShare.Read);
				try
				{
					int num2 = (int)fileStream2.Length;
					byte[] array = new byte[num2];
					fileStream2.Read(array, 0, num2);
					SecurityPermission securityPermission2 = new SecurityPermission(SecurityPermissionFlag.ControlEvidence);
					securityPermission2.Assert();
					try
					{
						compilerResults.CompiledAssembly = Assembly.Load(array, null, options.Evidence);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					return compilerResults;
				}
				finally
				{
					fileStream2.Close();
				}
			}
			compilerResults.PathToAssembly = options.OutputAssembly;
			return compilerResults;
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x0004D6D0 File Offset: 0x0004C6D0
		CompilerResults ICodeCompiler.CompileAssemblyFromDom(CompilerParameters options, CodeCompileUnit e)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults compilerResults;
			try
			{
				compilerResults = this.FromDom(options, e);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0004D714 File Offset: 0x0004C714
		CompilerResults ICodeCompiler.CompileAssemblyFromFile(CompilerParameters options, string fileName)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults compilerResults;
			try
			{
				compilerResults = this.FromFile(options, fileName);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0004D758 File Offset: 0x0004C758
		CompilerResults ICodeCompiler.CompileAssemblyFromSource(CompilerParameters options, string source)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults compilerResults;
			try
			{
				compilerResults = this.FromSource(options, source);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x0004D79C File Offset: 0x0004C79C
		CompilerResults ICodeCompiler.CompileAssemblyFromSourceBatch(CompilerParameters options, string[] sources)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults compilerResults;
			try
			{
				compilerResults = this.FromSourceBatch(options, sources);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x0004D7E0 File Offset: 0x0004C7E0
		CompilerResults ICodeCompiler.CompileAssemblyFromFileBatch(CompilerParameters options, string[] fileNames)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			CompilerResults compilerResults;
			try
			{
				foreach (string text in fileNames)
				{
					using (File.OpenRead(text))
					{
					}
				}
				compilerResults = this.FromFileBatch(options, fileNames);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x0004D86C File Offset: 0x0004C86C
		CompilerResults ICodeCompiler.CompileAssemblyFromDomBatch(CompilerParameters options, CodeCompileUnit[] ea)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults compilerResults;
			try
			{
				compilerResults = this.FromDomBatch(options, ea);
			}
			finally
			{
				options.TempFiles.SafeDelete();
			}
			return compilerResults;
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x0004D8B0 File Offset: 0x0004C8B0
		internal void Compile(CompilerParameters options, string compilerDirectory, string compilerExe, string arguments, ref string outputFile, ref int nativeReturnValue, string trueArgs)
		{
			string text = null;
			outputFile = options.TempFiles.AddExtension("out");
			string text2 = Path.Combine(compilerDirectory, compilerExe);
			if (File.Exists(text2))
			{
				string text3 = null;
				if (trueArgs != null)
				{
					text3 = "\"" + text2 + "\" " + trueArgs;
				}
				nativeReturnValue = Executor.ExecWaitWithCapture(options.SafeUserToken, "\"" + text2 + "\" " + arguments, Environment.CurrentDirectory, options.TempFiles, ref outputFile, ref text, text3);
				return;
			}
			throw new InvalidOperationException(SR.GetString("CompilerNotFound", new object[] { text2 }));
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x0004D948 File Offset: 0x0004C948
		private CompilerResults FromDom(CompilerParameters options, CodeCompileUnit e)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			return this.FromDomBatch(options, new CodeCompileUnit[] { e });
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x0004D984 File Offset: 0x0004C984
		private CompilerResults FromFile(CompilerParameters options, string fileName)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			using (File.OpenRead(fileName))
			{
			}
			return this.FromFileBatch(options, new string[] { fileName });
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x0004D9F0 File Offset: 0x0004C9F0
		private CompilerResults FromSource(CompilerParameters options, string source)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			return this.FromSourceBatch(options, new string[] { source });
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x0004DA2C File Offset: 0x0004CA2C
		private CompilerResults FromDomBatch(CompilerParameters options, CodeCompileUnit[] ea)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (ea == null)
			{
				throw new ArgumentNullException("ea");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			string[] array = new string[ea.Length];
			CompilerResults compilerResults = null;
			try
			{
				WindowsImpersonationContext windowsImpersonationContext = Executor.RevertImpersonation();
				try
				{
					for (int i = 0; i < ea.Length; i++)
					{
						if (ea[i] != null)
						{
							this.ResolveReferencedAssemblies(options, ea[i]);
							array[i] = options.TempFiles.AddExtension(i + this.FileExtension);
							Stream stream = new FileStream(array[i], FileMode.Create, FileAccess.Write, FileShare.Read);
							try
							{
								using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
								{
									((ICodeGenerator)this).GenerateCodeFromCompileUnit(ea[i], streamWriter, this.Options);
									streamWriter.Flush();
								}
							}
							finally
							{
								stream.Close();
							}
						}
					}
					compilerResults = this.FromFileBatch(options, array);
				}
				finally
				{
					Executor.ReImpersonate(windowsImpersonationContext);
				}
			}
			catch
			{
				throw;
			}
			return compilerResults;
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x0004DB48 File Offset: 0x0004CB48
		private void ResolveReferencedAssemblies(CompilerParameters options, CodeCompileUnit e)
		{
			if (e.ReferencedAssemblies.Count > 0)
			{
				foreach (string text in e.ReferencedAssemblies)
				{
					if (!options.ReferencedAssemblies.Contains(text))
					{
						options.ReferencedAssemblies.Add(text);
					}
				}
			}
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x0004DBC0 File Offset: 0x0004CBC0
		private CompilerResults FromSourceBatch(CompilerParameters options, string[] sources)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			string[] array = new string[sources.Length];
			CompilerResults compilerResults = null;
			try
			{
				WindowsImpersonationContext windowsImpersonationContext = Executor.RevertImpersonation();
				try
				{
					for (int i = 0; i < sources.Length; i++)
					{
						string text = options.TempFiles.AddExtension(i + this.FileExtension);
						Stream stream = new FileStream(text, FileMode.Create, FileAccess.Write, FileShare.Read);
						try
						{
							using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
							{
								streamWriter.Write(sources[i]);
								streamWriter.Flush();
							}
						}
						finally
						{
							stream.Close();
						}
						array[i] = text;
					}
					compilerResults = this.FromFileBatch(options, array);
				}
				finally
				{
					Executor.ReImpersonate(windowsImpersonationContext);
				}
			}
			catch
			{
				throw;
			}
			return compilerResults;
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x0004DCC8 File Offset: 0x0004CCC8
		private static string JoinStringArray(string[] sa, string separator)
		{
			if (sa == null || sa.Length == 0)
			{
				return string.Empty;
			}
			if (sa.Length == 1)
			{
				return "\"" + sa[0] + "\"";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < sa.Length - 1; i++)
			{
				stringBuilder.Append("\"");
				stringBuilder.Append(sa[i]);
				stringBuilder.Append("\"");
				stringBuilder.Append(separator);
			}
			stringBuilder.Append("\"");
			stringBuilder.Append(sa[sa.Length - 1]);
			stringBuilder.Append("\"");
			return stringBuilder.ToString();
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x0004DD68 File Offset: 0x0004CD68
		void ICodeGenerator.GenerateCodeFromType(CodeTypeDeclaration e, TextWriter w, CodeGeneratorOptions o)
		{
			bool flag = false;
			if (this.output != null && w != this.output.InnerWriter)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenOutputWriter"));
			}
			if (this.output == null)
			{
				flag = true;
				this.options = ((o == null) ? new CodeGeneratorOptions() : o);
				this.output = new IndentedTextWriter(w, this.options.IndentString);
			}
			try
			{
				this.GenerateType(e);
			}
			finally
			{
				if (flag)
				{
					this.output = null;
					this.options = null;
				}
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0004DDFC File Offset: 0x0004CDFC
		void ICodeGenerator.GenerateCodeFromExpression(CodeExpression e, TextWriter w, CodeGeneratorOptions o)
		{
			bool flag = false;
			if (this.output != null && w != this.output.InnerWriter)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenOutputWriter"));
			}
			if (this.output == null)
			{
				flag = true;
				this.options = ((o == null) ? new CodeGeneratorOptions() : o);
				this.output = new IndentedTextWriter(w, this.options.IndentString);
			}
			try
			{
				this.GenerateExpression(e);
			}
			finally
			{
				if (flag)
				{
					this.output = null;
					this.options = null;
				}
			}
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x0004DE90 File Offset: 0x0004CE90
		void ICodeGenerator.GenerateCodeFromCompileUnit(CodeCompileUnit e, TextWriter w, CodeGeneratorOptions o)
		{
			bool flag = false;
			if (this.output != null && w != this.output.InnerWriter)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenOutputWriter"));
			}
			if (this.output == null)
			{
				flag = true;
				this.options = ((o == null) ? new CodeGeneratorOptions() : o);
				this.output = new IndentedTextWriter(w, this.options.IndentString);
			}
			try
			{
				if (e is CodeSnippetCompileUnit)
				{
					this.GenerateSnippetCompileUnit((CodeSnippetCompileUnit)e);
				}
				else
				{
					this.GenerateCompileUnit(e);
				}
			}
			finally
			{
				if (flag)
				{
					this.output = null;
					this.options = null;
				}
			}
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x0004DF38 File Offset: 0x0004CF38
		void ICodeGenerator.GenerateCodeFromNamespace(CodeNamespace e, TextWriter w, CodeGeneratorOptions o)
		{
			bool flag = false;
			if (this.output != null && w != this.output.InnerWriter)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenOutputWriter"));
			}
			if (this.output == null)
			{
				flag = true;
				this.options = ((o == null) ? new CodeGeneratorOptions() : o);
				this.output = new IndentedTextWriter(w, this.options.IndentString);
			}
			try
			{
				this.GenerateNamespace(e);
			}
			finally
			{
				if (flag)
				{
					this.output = null;
					this.options = null;
				}
			}
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x0004DFCC File Offset: 0x0004CFCC
		void ICodeGenerator.GenerateCodeFromStatement(CodeStatement e, TextWriter w, CodeGeneratorOptions o)
		{
			bool flag = false;
			if (this.output != null && w != this.output.InnerWriter)
			{
				throw new InvalidOperationException(SR.GetString("CodeGenOutputWriter"));
			}
			if (this.output == null)
			{
				flag = true;
				this.options = ((o == null) ? new CodeGeneratorOptions() : o);
				this.output = new IndentedTextWriter(w, this.options.IndentString);
			}
			try
			{
				this.GenerateStatement(e);
			}
			finally
			{
				if (flag)
				{
					this.output = null;
					this.options = null;
				}
			}
		}

		// Token: 0x04001602 RID: 5634
		private const int ParameterMultilineThreshold = 15;

		// Token: 0x04001603 RID: 5635
		private const int MaxLineLength = 80;

		// Token: 0x04001604 RID: 5636
		private const GeneratorSupport LanguageSupport = GeneratorSupport.ArraysOfArrays | GeneratorSupport.EntryPointMethod | GeneratorSupport.GotoStatements | GeneratorSupport.MultidimensionalArrays | GeneratorSupport.StaticConstructors | GeneratorSupport.TryCatchStatements | GeneratorSupport.ReturnTypeAttributes | GeneratorSupport.DeclareValueTypes | GeneratorSupport.DeclareEnums | GeneratorSupport.DeclareDelegates | GeneratorSupport.DeclareInterfaces | GeneratorSupport.DeclareEvents | GeneratorSupport.AssemblyAttributes | GeneratorSupport.ParameterAttributes | GeneratorSupport.ReferenceParameters | GeneratorSupport.ChainedConstructorArguments | GeneratorSupport.NestedTypes | GeneratorSupport.MultipleInterfaceMembers | GeneratorSupport.PublicStaticMembers | GeneratorSupport.ComplexExpressions | GeneratorSupport.Win32Resources | GeneratorSupport.Resources | GeneratorSupport.PartialTypes | GeneratorSupport.GenericTypeReference | GeneratorSupport.GenericTypeDeclaration | GeneratorSupport.DeclareIndexerProperties;

		// Token: 0x04001605 RID: 5637
		private IndentedTextWriter output;

		// Token: 0x04001606 RID: 5638
		private CodeGeneratorOptions options;

		// Token: 0x04001607 RID: 5639
		private CodeTypeDeclaration currentClass;

		// Token: 0x04001608 RID: 5640
		private CodeTypeMember currentMember;

		// Token: 0x04001609 RID: 5641
		private bool inNestedBinary;

		// Token: 0x0400160A RID: 5642
		private IDictionary<string, string> provOptions;

		// Token: 0x0400160B RID: 5643
		private static Regex outputReg;

		// Token: 0x0400160C RID: 5644
		private static readonly string[][] keywords = new string[][]
		{
			null,
			new string[] { "as", "do", "if", "in", "is" },
			new string[] { "for", "int", "new", "out", "ref", "try" },
			new string[]
			{
				"base", "bool", "byte", "case", "char", "else", "enum", "goto", "lock", "long",
				"null", "this", "true", "uint", "void"
			},
			new string[]
			{
				"break", "catch", "class", "const", "event", "false", "fixed", "float", "sbyte", "short",
				"throw", "ulong", "using", "where", "while", "yield"
			},
			new string[]
			{
				"double", "extern", "object", "params", "public", "return", "sealed", "sizeof", "static", "string",
				"struct", "switch", "typeof", "unsafe", "ushort"
			},
			new string[] { "checked", "decimal", "default", "finally", "foreach", "partial", "private", "virtual" },
			new string[] { "abstract", "continue", "delegate", "explicit", "implicit", "internal", "operator", "override", "readonly", "volatile" },
			new string[] { "__arglist", "__makeref", "__reftype", "interface", "namespace", "protected", "unchecked" },
			new string[] { "__refvalue", "stackalloc" }
		};

		// Token: 0x0400160D RID: 5645
		private bool generatingForLoop;
	}
}
