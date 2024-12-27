using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000A1 RID: 161
	[DesignerCategory("code")]
	internal sealed class JSCodeGenerator : CodeCompiler
	{
		// Token: 0x0600072A RID: 1834 RVA: 0x000314A8 File Offset: 0x000304A8
		static JSCodeGenerator()
		{
			object obj = new object();
			JSCodeGenerator.keywords["abstract"] = obj;
			JSCodeGenerator.keywords["assert"] = obj;
			JSCodeGenerator.keywords["boolean"] = obj;
			JSCodeGenerator.keywords["break"] = obj;
			JSCodeGenerator.keywords["byte"] = obj;
			JSCodeGenerator.keywords["case"] = obj;
			JSCodeGenerator.keywords["catch"] = obj;
			JSCodeGenerator.keywords["char"] = obj;
			JSCodeGenerator.keywords["class"] = obj;
			JSCodeGenerator.keywords["const"] = obj;
			JSCodeGenerator.keywords["continue"] = obj;
			JSCodeGenerator.keywords["debugger"] = obj;
			JSCodeGenerator.keywords["decimal"] = obj;
			JSCodeGenerator.keywords["default"] = obj;
			JSCodeGenerator.keywords["delete"] = obj;
			JSCodeGenerator.keywords["do"] = obj;
			JSCodeGenerator.keywords["double"] = obj;
			JSCodeGenerator.keywords["else"] = obj;
			JSCodeGenerator.keywords["ensure"] = obj;
			JSCodeGenerator.keywords["enum"] = obj;
			JSCodeGenerator.keywords["event"] = obj;
			JSCodeGenerator.keywords["export"] = obj;
			JSCodeGenerator.keywords["extends"] = obj;
			JSCodeGenerator.keywords["false"] = obj;
			JSCodeGenerator.keywords["final"] = obj;
			JSCodeGenerator.keywords["finally"] = obj;
			JSCodeGenerator.keywords["float"] = obj;
			JSCodeGenerator.keywords["for"] = obj;
			JSCodeGenerator.keywords["function"] = obj;
			JSCodeGenerator.keywords["get"] = obj;
			JSCodeGenerator.keywords["goto"] = obj;
			JSCodeGenerator.keywords["if"] = obj;
			JSCodeGenerator.keywords["implements"] = obj;
			JSCodeGenerator.keywords["import"] = obj;
			JSCodeGenerator.keywords["in"] = obj;
			JSCodeGenerator.keywords["instanceof"] = obj;
			JSCodeGenerator.keywords["int"] = obj;
			JSCodeGenerator.keywords["invariant"] = obj;
			JSCodeGenerator.keywords["interface"] = obj;
			JSCodeGenerator.keywords["internal"] = obj;
			JSCodeGenerator.keywords["long"] = obj;
			JSCodeGenerator.keywords["namespace"] = obj;
			JSCodeGenerator.keywords["native"] = obj;
			JSCodeGenerator.keywords["new"] = obj;
			JSCodeGenerator.keywords["null"] = obj;
			JSCodeGenerator.keywords["package"] = obj;
			JSCodeGenerator.keywords["private"] = obj;
			JSCodeGenerator.keywords["protected"] = obj;
			JSCodeGenerator.keywords["public"] = obj;
			JSCodeGenerator.keywords["require"] = obj;
			JSCodeGenerator.keywords["return"] = obj;
			JSCodeGenerator.keywords["sbyte"] = obj;
			JSCodeGenerator.keywords["scope"] = obj;
			JSCodeGenerator.keywords["set"] = obj;
			JSCodeGenerator.keywords["short"] = obj;
			JSCodeGenerator.keywords["static"] = obj;
			JSCodeGenerator.keywords["super"] = obj;
			JSCodeGenerator.keywords["switch"] = obj;
			JSCodeGenerator.keywords["synchronized"] = obj;
			JSCodeGenerator.keywords["this"] = obj;
			JSCodeGenerator.keywords["throw"] = obj;
			JSCodeGenerator.keywords["throws"] = obj;
			JSCodeGenerator.keywords["transient"] = obj;
			JSCodeGenerator.keywords["true"] = obj;
			JSCodeGenerator.keywords["try"] = obj;
			JSCodeGenerator.keywords["typeof"] = obj;
			JSCodeGenerator.keywords["use"] = obj;
			JSCodeGenerator.keywords["uint"] = obj;
			JSCodeGenerator.keywords["ulong"] = obj;
			JSCodeGenerator.keywords["ushort"] = obj;
			JSCodeGenerator.keywords["var"] = obj;
			JSCodeGenerator.keywords["void"] = obj;
			JSCodeGenerator.keywords["volatile"] = obj;
			JSCodeGenerator.keywords["while"] = obj;
			JSCodeGenerator.keywords["with"] = obj;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0003198C File Offset: 0x0003098C
		protected override string CmdArgsFromParameters(CompilerParameters options)
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			string text = ((Path.DirectorySeparatorChar == '/') ? "-" : "/");
			stringBuilder.Append(text + "utf8output ");
			object obj = new object();
			Hashtable hashtable = new Hashtable(20);
			foreach (string text2 in options.ReferencedAssemblies)
			{
				if (hashtable[text2] == null)
				{
					hashtable[text2] = obj;
					stringBuilder.Append(text + "r:");
					stringBuilder.Append("\"");
					stringBuilder.Append(text2);
					stringBuilder.Append("\" ");
				}
			}
			stringBuilder.Append(text + "out:");
			stringBuilder.Append("\"");
			stringBuilder.Append(options.OutputAssembly);
			stringBuilder.Append("\" ");
			if (options.IncludeDebugInformation)
			{
				stringBuilder.Append(text + "d:DEBUG ");
				stringBuilder.Append(text + "debug+ ");
			}
			else
			{
				stringBuilder.Append(text + "debug- ");
			}
			if (options.TreatWarningsAsErrors)
			{
				stringBuilder.Append(text + "warnaserror ");
			}
			if (options.WarningLevel >= 0)
			{
				stringBuilder.Append(text + "w:" + options.WarningLevel.ToString(CultureInfo.InvariantCulture) + " ");
			}
			if (options.Win32Resource != null)
			{
				stringBuilder.Append(text + "win32res:\"" + options.Win32Resource + "\" ");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x00031B58 File Offset: 0x00030B58
		protected override string CompilerName
		{
			get
			{
				return "jsc.exe";
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00031B5F File Offset: 0x00030B5F
		protected override string CreateEscapedIdentifier(string name)
		{
			if (this.IsKeyword(name))
			{
				return "\\" + name;
			}
			return name;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00031B77 File Offset: 0x00030B77
		protected override string CreateValidIdentifier(string name)
		{
			if (this.IsKeyword(name))
			{
				return "$" + name;
			}
			return name;
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00031B8F File Offset: 0x00030B8F
		protected override string FileExtension
		{
			get
			{
				return ".js";
			}
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00031B98 File Offset: 0x00030B98
		protected override CompilerResults FromFileBatch(CompilerParameters options, string[] fileNames)
		{
			string text = options.TempFiles.AddExtension("out");
			CompilerResults compilerResults = new CompilerResults(options.TempFiles);
			if (options.OutputAssembly == null || options.OutputAssembly.Length == 0)
			{
				options.OutputAssembly = compilerResults.TempFiles.AddExtension("dll", !options.GenerateInMemory);
			}
			string text2 = null;
			if (options.IncludeDebugInformation)
			{
				compilerResults.TempFiles.AddExtension("pdb");
				text2 = this.CmdArgsFromParameters(options);
			}
			compilerResults.NativeCompilerReturnValue = 0;
			try
			{
				JSInProcCompiler jsinProcCompiler = new JSInProcCompiler();
				compilerResults.NativeCompilerReturnValue = jsinProcCompiler.Compile(options, text2, fileNames, text);
			}
			catch
			{
				compilerResults.NativeCompilerReturnValue = 10;
			}
			try
			{
				StreamReader streamReader = new StreamReader(text);
				try
				{
					for (string text3 = streamReader.ReadLine(); text3 != null; text3 = streamReader.ReadLine())
					{
						compilerResults.Output.Add(text3);
						this.ProcessCompilerOutputLine(compilerResults, text3);
					}
				}
				finally
				{
					streamReader.Close();
				}
			}
			catch (Exception ex)
			{
				compilerResults.Output.Add(JScriptException.Localize("No error output", CultureInfo.CurrentUICulture));
				compilerResults.Output.Add(ex.ToString());
			}
			catch
			{
				compilerResults.Output.Add(JScriptException.Localize("No error output", CultureInfo.CurrentUICulture));
			}
			if (compilerResults.NativeCompilerReturnValue == 0 && options.GenerateInMemory)
			{
				FileStream fileStream = new FileStream(options.OutputAssembly, FileMode.Open, FileAccess.Read, FileShare.Read);
				try
				{
					int num = (int)fileStream.Length;
					byte[] array = new byte[num];
					fileStream.Read(array, 0, num);
					compilerResults.CompiledAssembly = Assembly.Load(array, null, options.Evidence);
					goto IL_01A0;
				}
				finally
				{
					fileStream.Close();
				}
			}
			compilerResults.PathToAssembly = Path.GetFullPath(options.OutputAssembly);
			IL_01A0:
			compilerResults.Evidence = options.Evidence;
			return compilerResults;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00031D94 File Offset: 0x00030D94
		protected override void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
		{
			this.OutputIdentifier(e.ParameterName);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00031DA4 File Offset: 0x00030DA4
		protected override void GenerateArrayCreateExpression(CodeArrayCreateExpression e)
		{
			CodeExpressionCollection initializers = e.Initializers;
			if (initializers.Count > 0)
			{
				base.Output.Write("[");
				base.Indent++;
				this.OutputExpressionList(initializers);
				base.Indent--;
				base.Output.Write("]");
				return;
			}
			base.Output.Write("new ");
			base.Output.Write(this.GetBaseTypeOutput(e.CreateType.BaseType));
			base.Output.Write("[");
			if (e.SizeExpression != null)
			{
				base.GenerateExpression(e.SizeExpression);
			}
			else
			{
				base.Output.Write(e.Size.ToString(CultureInfo.InvariantCulture));
			}
			base.Output.Write("]");
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00031E88 File Offset: 0x00030E88
		protected override void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e)
		{
			base.GenerateExpression(e.TargetObject);
			base.Output.Write("[");
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
					base.Output.Write(", ");
				}
				base.GenerateExpression(codeExpression);
			}
			base.Output.Write("]");
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00031F28 File Offset: 0x00030F28
		private void GenerateAssemblyAttributes(CodeAttributeDeclarationCollection attributes)
		{
			if (attributes.Count == 0)
			{
				return;
			}
			IEnumerator enumerator = attributes.GetEnumerator();
			while (enumerator.MoveNext())
			{
				base.Output.Write("[");
				base.Output.Write("assembly: ");
				CodeAttributeDeclaration codeAttributeDeclaration = (CodeAttributeDeclaration)enumerator.Current;
				base.Output.Write(this.GetBaseTypeOutput(codeAttributeDeclaration.Name));
				base.Output.Write("(");
				bool flag = true;
				foreach (object obj in codeAttributeDeclaration.Arguments)
				{
					CodeAttributeArgument codeAttributeArgument = (CodeAttributeArgument)obj;
					if (flag)
					{
						flag = false;
					}
					else
					{
						base.Output.Write(", ");
					}
					this.OutputAttributeArgument(codeAttributeArgument);
				}
				base.Output.Write(")");
				base.Output.Write("]");
				base.Output.WriteLine();
			}
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00032040 File Offset: 0x00031040
		protected override void GenerateAssignStatement(CodeAssignStatement e)
		{
			base.GenerateExpression(e.Left);
			base.Output.Write(" = ");
			base.GenerateExpression(e.Right);
			if (!this.forLoopHack)
			{
				base.Output.WriteLine(";");
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00032090 File Offset: 0x00031090
		protected override void GenerateAttachEventStatement(CodeAttachEventStatement e)
		{
			base.GenerateExpression(e.Event.TargetObject);
			base.Output.Write(".add_");
			base.Output.Write(e.Event.EventName);
			base.Output.Write("(");
			base.GenerateExpression(e.Listener);
			base.Output.WriteLine(");");
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00032100 File Offset: 0x00031100
		protected override void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes)
		{
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00032102 File Offset: 0x00031102
		protected override void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes)
		{
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00032104 File Offset: 0x00031104
		protected override void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e)
		{
			base.Output.Write("super");
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00032118 File Offset: 0x00031118
		private string GetBaseTypeOutput(string baseType)
		{
			if (baseType.Length == 0)
			{
				return "void";
			}
			if (string.Compare(baseType, "System.Byte", StringComparison.Ordinal) == 0)
			{
				return "byte";
			}
			if (string.Compare(baseType, "System.Int16", StringComparison.Ordinal) == 0)
			{
				return "short";
			}
			if (string.Compare(baseType, "System.Int32", StringComparison.Ordinal) == 0)
			{
				return "int";
			}
			if (string.Compare(baseType, "System.Int64", StringComparison.Ordinal) == 0)
			{
				return "long";
			}
			if (string.Compare(baseType, "System.SByte", StringComparison.Ordinal) == 0)
			{
				return "sbyte";
			}
			if (string.Compare(baseType, "System.UInt16", StringComparison.Ordinal) == 0)
			{
				return "ushort";
			}
			if (string.Compare(baseType, "System.UInt32", StringComparison.Ordinal) == 0)
			{
				return "uint";
			}
			if (string.Compare(baseType, "System.UInt64", StringComparison.Ordinal) == 0)
			{
				return "ulong";
			}
			if (string.Compare(baseType, "System.Decimal", StringComparison.Ordinal) == 0)
			{
				return "decimal";
			}
			if (string.Compare(baseType, "System.Single", StringComparison.Ordinal) == 0)
			{
				return "float";
			}
			if (string.Compare(baseType, "System.Double", StringComparison.Ordinal) == 0)
			{
				return "double";
			}
			if (string.Compare(baseType, "System.Boolean", StringComparison.Ordinal) == 0)
			{
				return "boolean";
			}
			if (string.Compare(baseType, "System.Char", StringComparison.Ordinal) == 0)
			{
				return "char";
			}
			baseType = baseType.Replace('+', '.');
			return this.CreateEscapedIdentifier(baseType);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0003224A File Offset: 0x0003124A
		protected override void GenerateCastExpression(CodeCastExpression e)
		{
			this.OutputType(e.TargetType);
			base.Output.Write("(");
			base.GenerateExpression(e.Expression);
			base.Output.Write(")");
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00032284 File Offset: 0x00031284
		protected override void GenerateComment(CodeComment e)
		{
			string text = e.Text;
			StringBuilder stringBuilder = new StringBuilder(text.Length * 2);
			string text2 = (e.DocComment ? "///" : "//");
			stringBuilder.Append(text2);
			int i = 0;
			while (i < text.Length)
			{
				char c = text[i];
				if (c <= '\r')
				{
					if (c != '\n')
					{
						if (c != '\r')
						{
							goto IL_00FC;
						}
						if (i < text.Length - 1 && text[i + 1] == '\n')
						{
							stringBuilder.Append("\r\n" + text2);
							i++;
						}
						else
						{
							stringBuilder.Append("\r" + text2);
						}
					}
					else
					{
						stringBuilder.Append("\n" + text2);
					}
				}
				else if (c != '@')
				{
					switch (c)
					{
					case '\u2028':
						stringBuilder.Append("\u2028" + text2);
						break;
					case '\u2029':
						stringBuilder.Append("\u2029" + text2);
						break;
					default:
						goto IL_00FC;
					}
				}
				IL_010A:
				i++;
				continue;
				IL_00FC:
				stringBuilder.Append(text[i]);
				goto IL_010A;
			}
			base.Output.WriteLine(stringBuilder.ToString());
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x000323BC File Offset: 0x000313BC
		protected override void GenerateCompileUnitStart(CodeCompileUnit e)
		{
			base.Output.WriteLine("//------------------------------------------------------------------------------");
			base.Output.WriteLine("/// <autogenerated>");
			base.Output.WriteLine("///     This code was generated by a tool.");
			base.Output.WriteLine("///     Runtime Version: " + Environment.Version.ToString());
			base.Output.WriteLine("///");
			base.Output.WriteLine("///     Changes to this file may cause incorrect behavior and will be lost if ");
			base.Output.WriteLine("///     the code is regenerated.");
			base.Output.WriteLine("/// </autogenerated>");
			base.Output.WriteLine("//------------------------------------------------------------------------------");
			base.Output.WriteLine("");
			if (e.AssemblyCustomAttributes.Count > 0)
			{
				this.GenerateAssemblyAttributes(e.AssemblyCustomAttributes);
				base.Output.WriteLine("");
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000324A4 File Offset: 0x000314A4
		protected override void GenerateConditionStatement(CodeConditionStatement e)
		{
			base.Output.Write("if (");
			base.Indent += 2;
			base.GenerateExpression(e.Condition);
			base.Indent -= 2;
			base.Output.Write(")");
			this.OutputStartingBrace();
			base.Indent++;
			base.GenerateStatements(e.TrueStatements);
			base.Indent--;
			if (e.FalseStatements.Count > 0)
			{
				base.Output.Write("}");
				if (base.Options.ElseOnClosing)
				{
					base.Output.Write(" ");
				}
				else
				{
					base.Output.WriteLine("");
				}
				base.Output.Write("else");
				this.OutputStartingBrace();
				base.Indent++;
				base.GenerateStatements(e.FalseStatements);
				base.Indent--;
			}
			base.Output.WriteLine("}");
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000325C4 File Offset: 0x000315C4
		protected override void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c)
		{
			if (!base.IsCurrentClass && !base.IsCurrentStruct)
			{
				return;
			}
			this.OutputMemberAccessModifier(e.Attributes);
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributeDeclarations(e.CustomAttributes);
			}
			base.Output.Write("function ");
			this.OutputIdentifier(base.CurrentTypeName);
			base.Output.Write("(");
			this.OutputParameters(e.Parameters);
			base.Output.Write(")");
			CodeExpressionCollection baseConstructorArgs = e.BaseConstructorArgs;
			CodeExpressionCollection chainedConstructorArgs = e.ChainedConstructorArgs;
			this.OutputStartingBrace();
			base.Indent++;
			if (baseConstructorArgs.Count > 0)
			{
				base.Output.Write("super(");
				this.OutputExpressionList(baseConstructorArgs);
				base.Output.WriteLine(");");
			}
			if (chainedConstructorArgs.Count > 0)
			{
				base.Output.Write("this(");
				this.OutputExpressionList(chainedConstructorArgs);
				base.Output.WriteLine(");");
			}
			base.GenerateStatements(e.Statements);
			base.Output.WriteLine();
			base.Indent--;
			base.Output.WriteLine("}");
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00032708 File Offset: 0x00031708
		protected override void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e)
		{
			bool flag = e.DelegateType != null;
			if (flag)
			{
				this.OutputType(e.DelegateType);
				base.Output.Write("(");
			}
			base.GenerateExpression(e.TargetObject);
			base.Output.Write(".");
			this.OutputIdentifier(e.MethodName);
			if (flag)
			{
				base.Output.Write(")");
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0003277C File Offset: 0x0003177C
		protected override void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
		{
			if (e.TargetObject != null)
			{
				base.GenerateExpression(e.TargetObject);
			}
			base.Output.Write("(");
			this.OutputExpressionList(e.Parameters);
			base.Output.Write(")");
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000327CC File Offset: 0x000317CC
		protected override void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c)
		{
			base.Output.Write("public static ");
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributeDeclarations(e.CustomAttributes);
			}
			base.Output.Write("function Main()");
			this.OutputStartingBrace();
			base.Indent++;
			base.GenerateStatements(e.Statements);
			base.Indent--;
			base.Output.WriteLine("}");
			this.mainClassName = base.CurrentTypeName;
			this.mainMethodName = "Main";
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00032868 File Offset: 0x00031868
		protected override void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c)
		{
			throw new Exception(JScriptException.Localize("No event declarations", CultureInfo.CurrentUICulture));
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0003287E File Offset: 0x0003187E
		protected override void GenerateEventReferenceExpression(CodeEventReferenceExpression e)
		{
			throw new Exception(JScriptException.Localize("No event references", CultureInfo.CurrentUICulture));
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00032894 File Offset: 0x00031894
		protected override void GenerateExpressionStatement(CodeExpressionStatement e)
		{
			base.GenerateExpression(e.Expression);
			if (!this.forLoopHack)
			{
				base.Output.WriteLine(";");
			}
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x000328BC File Offset: 0x000318BC
		protected override void GenerateField(CodeMemberField e)
		{
			if (base.IsCurrentDelegate || base.IsCurrentInterface)
			{
				throw new Exception(JScriptException.Localize("Only methods on interfaces", CultureInfo.CurrentUICulture));
			}
			if (base.IsCurrentEnum)
			{
				this.OutputIdentifier(e.Name);
				if (e.InitExpression != null)
				{
					base.Output.Write(" = ");
					base.GenerateExpression(e.InitExpression);
				}
				base.Output.WriteLine(",");
				return;
			}
			this.OutputMemberAccessModifier(e.Attributes);
			if ((e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Static)
			{
				base.Output.Write("static ");
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributeDeclarations(e.CustomAttributes);
				base.Output.WriteLine("");
			}
			if ((e.Attributes & MemberAttributes.Const) == MemberAttributes.Const)
			{
				if ((e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Static)
				{
					base.Output.Write("static ");
				}
				base.Output.Write("const ");
			}
			else
			{
				base.Output.Write("var ");
			}
			this.OutputTypeNamePair(e.Type, e.Name);
			if (e.InitExpression != null)
			{
				base.Output.Write(" = ");
				base.GenerateExpression(e.InitExpression);
			}
			base.Output.WriteLine(";");
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00032A18 File Offset: 0x00031A18
		protected override void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				base.GenerateExpression(e.TargetObject);
				base.Output.Write(".");
			}
			this.OutputIdentifier(e.FieldName);
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00032A4A File Offset: 0x00031A4A
		protected override void GenerateGotoStatement(CodeGotoStatement e)
		{
			throw new Exception(JScriptException.Localize("No goto statements", CultureInfo.CurrentUICulture));
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00032A60 File Offset: 0x00031A60
		protected override void GenerateIndexerExpression(CodeIndexerExpression e)
		{
			base.GenerateExpression(e.TargetObject);
			base.Output.Write("[");
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
					base.Output.Write(", ");
				}
				base.GenerateExpression(codeExpression);
			}
			base.Output.Write("]");
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00032B00 File Offset: 0x00031B00
		protected override void GenerateIterationStatement(CodeIterationStatement e)
		{
			this.forLoopHack = true;
			base.Output.Write("for (");
			base.GenerateStatement(e.InitStatement);
			base.Output.Write("; ");
			base.GenerateExpression(e.TestExpression);
			base.Output.Write("; ");
			base.GenerateStatement(e.IncrementStatement);
			base.Output.Write(")");
			this.OutputStartingBrace();
			this.forLoopHack = false;
			base.Indent++;
			base.GenerateStatements(e.Statements);
			base.Indent--;
			base.Output.WriteLine("}");
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00032BBD File Offset: 0x00031BBD
		protected override void GenerateLabeledStatement(CodeLabeledStatement e)
		{
			throw new Exception(JScriptException.Localize("No goto statements", CultureInfo.CurrentUICulture));
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00032BD4 File Offset: 0x00031BD4
		protected override void GenerateLinePragmaStart(CodeLinePragma e)
		{
			base.Output.WriteLine("");
			base.Output.WriteLine("//@cc_on");
			base.Output.Write("//@set @position(file=\"");
			base.Output.Write(Regex.Replace(e.FileName, "\\\\", "\\\\"));
			base.Output.Write("\";line=");
			base.Output.Write(e.LineNumber.ToString(CultureInfo.InvariantCulture));
			base.Output.WriteLine(")");
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00032C6F File Offset: 0x00031C6F
		protected override void GenerateLinePragmaEnd(CodeLinePragma e)
		{
			base.Output.WriteLine("");
			base.Output.WriteLine("//@set @position(end)");
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00032C94 File Offset: 0x00031C94
		protected override void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c)
		{
			if (!base.IsCurrentInterface)
			{
				if (e.PrivateImplementationType == null)
				{
					this.OutputMemberAccessModifier(e.Attributes);
					this.OutputMemberVTableModifier(e.Attributes);
					this.OutputMemberScopeModifier(e.Attributes);
				}
			}
			else
			{
				this.OutputMemberVTableModifier(e.Attributes);
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributeDeclarations(e.CustomAttributes);
			}
			base.Output.Write("function ");
			if (e.PrivateImplementationType != null && !base.IsCurrentInterface)
			{
				base.Output.Write(e.PrivateImplementationType.BaseType);
				base.Output.Write(".");
			}
			this.OutputIdentifier(e.Name);
			base.Output.Write("(");
			this.isArgumentList = false;
			try
			{
				this.OutputParameters(e.Parameters);
			}
			finally
			{
				this.isArgumentList = true;
			}
			base.Output.Write(")");
			if (e.ReturnType.BaseType.Length > 0 && string.Compare(e.ReturnType.BaseType, typeof(void).FullName, StringComparison.Ordinal) != 0)
			{
				base.Output.Write(" : ");
				this.OutputType(e.ReturnType);
			}
			if (!base.IsCurrentInterface && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
			{
				this.OutputStartingBrace();
				base.Indent++;
				base.GenerateStatements(e.Statements);
				base.Indent--;
				base.Output.WriteLine("}");
				return;
			}
			base.Output.WriteLine(";");
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00032E50 File Offset: 0x00031E50
		protected override void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e)
		{
			this.GenerateMethodReferenceExpression(e.Method);
			base.Output.Write("(");
			this.OutputExpressionList(e.Parameters);
			base.Output.Write(")");
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00032E8C File Offset: 0x00031E8C
		protected override void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				if (e.TargetObject is CodeBinaryOperatorExpression)
				{
					base.Output.Write("(");
					base.GenerateExpression(e.TargetObject);
					base.Output.Write(")");
				}
				else
				{
					base.GenerateExpression(e.TargetObject);
				}
				base.Output.Write(".");
			}
			this.OutputIdentifier(e.MethodName);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00032F04 File Offset: 0x00031F04
		protected override void GenerateMethodReturnStatement(CodeMethodReturnStatement e)
		{
			base.Output.Write("return");
			if (e.Expression != null)
			{
				base.Output.Write(" ");
				base.GenerateExpression(e.Expression);
			}
			base.Output.WriteLine(";");
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00032F58 File Offset: 0x00031F58
		protected override void GenerateNamespace(CodeNamespace e)
		{
			base.Output.WriteLine("//@cc_on");
			base.Output.WriteLine("//@set @debug(off)");
			base.Output.WriteLine("");
			base.GenerateNamespaceImports(e);
			base.Output.WriteLine("");
			this.GenerateCommentStatements(e.Comments);
			this.GenerateNamespaceStart(e);
			base.GenerateTypes(e);
			this.GenerateNamespaceEnd(e);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00032FD0 File Offset: 0x00031FD0
		protected override void GenerateNamespaceEnd(CodeNamespace e)
		{
			if (e.Name != null && e.Name.Length > 0)
			{
				base.Indent--;
				base.Output.WriteLine("}");
			}
			if (this.mainClassName != null)
			{
				if (e.Name != null)
				{
					this.OutputIdentifier(e.Name);
					base.Output.Write(".");
				}
				this.OutputIdentifier(this.mainClassName);
				base.Output.Write(".");
				this.OutputIdentifier(this.mainMethodName);
				base.Output.WriteLine("();");
				this.mainClassName = null;
			}
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0003307C File Offset: 0x0003207C
		protected override void GenerateNamespaceImport(CodeNamespaceImport e)
		{
			base.Output.Write("import ");
			this.OutputIdentifier(e.Namespace);
			base.Output.WriteLine(";");
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000330AC File Offset: 0x000320AC
		protected override void GenerateNamespaceStart(CodeNamespace e)
		{
			if (e.Name != null && e.Name.Length > 0)
			{
				base.Output.Write("package ");
				this.OutputIdentifier(e.Name);
				this.OutputStartingBrace();
				base.Indent++;
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00033100 File Offset: 0x00032100
		protected override void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
		{
			base.Output.Write("new ");
			this.OutputType(e.CreateType);
			base.Output.Write("(");
			this.OutputExpressionList(e.Parameters);
			base.Output.Write(")");
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00033158 File Offset: 0x00032158
		protected override void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
		{
			if (e.CustomAttributes.Count > 0)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = e.CustomAttributes[0];
				if (!(codeAttributeDeclaration.Name == "ParamArrayAttribute"))
				{
					throw new Exception(JScriptException.Localize("No parameter attributes", CultureInfo.CurrentUICulture));
				}
				base.Output.Write("... ");
			}
			this.OutputDirection(e.Direction);
			this.OutputTypeNamePair(e.Type, e.Name);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000331D8 File Offset: 0x000321D8
		protected override void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c)
		{
			if (!base.IsCurrentClass && !base.IsCurrentStruct && !base.IsCurrentInterface)
			{
				return;
			}
			if (e.HasGet)
			{
				if (!base.IsCurrentInterface)
				{
					if (e.PrivateImplementationType == null)
					{
						this.OutputMemberAccessModifier(e.Attributes);
						this.OutputMemberVTableModifier(e.Attributes);
						this.OutputMemberScopeModifier(e.Attributes);
					}
				}
				else
				{
					this.OutputMemberVTableModifier(e.Attributes);
				}
				if (e.CustomAttributes.Count > 0)
				{
					if (base.IsCurrentInterface)
					{
						base.Output.Write("public ");
					}
					this.OutputAttributeDeclarations(e.CustomAttributes);
					base.Output.WriteLine("");
				}
				base.Output.Write("function get ");
				if (e.PrivateImplementationType != null && !base.IsCurrentInterface)
				{
					base.Output.Write(e.PrivateImplementationType.BaseType);
					base.Output.Write(".");
				}
				this.OutputIdentifier(e.Name);
				if (e.Parameters.Count > 0)
				{
					throw new Exception(JScriptException.Localize("No indexer declarations", CultureInfo.CurrentUICulture));
				}
				base.Output.Write("() : ");
				this.OutputType(e.Type);
				if (base.IsCurrentInterface || (e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Abstract)
				{
					base.Output.WriteLine(";");
				}
				else
				{
					this.OutputStartingBrace();
					base.Indent++;
					base.GenerateStatements(e.GetStatements);
					base.Indent--;
					base.Output.WriteLine("}");
				}
			}
			if (e.HasSet)
			{
				if (!base.IsCurrentInterface)
				{
					if (e.PrivateImplementationType == null)
					{
						this.OutputMemberAccessModifier(e.Attributes);
						this.OutputMemberVTableModifier(e.Attributes);
						this.OutputMemberScopeModifier(e.Attributes);
					}
				}
				else
				{
					this.OutputMemberVTableModifier(e.Attributes);
				}
				if (e.CustomAttributes.Count > 0 && !e.HasGet)
				{
					if (base.IsCurrentInterface)
					{
						base.Output.Write("public ");
					}
					this.OutputAttributeDeclarations(e.CustomAttributes);
					base.Output.WriteLine("");
				}
				base.Output.Write("function set ");
				if (e.PrivateImplementationType != null && !base.IsCurrentInterface)
				{
					base.Output.Write(e.PrivateImplementationType.BaseType);
					base.Output.Write(".");
				}
				this.OutputIdentifier(e.Name);
				base.Output.Write("(");
				this.OutputTypeNamePair(e.Type, "value");
				if (e.Parameters.Count > 0)
				{
					throw new Exception(JScriptException.Localize("No indexer declarations", CultureInfo.CurrentUICulture));
				}
				base.Output.Write(")");
				if (base.IsCurrentInterface || (e.Attributes & MemberAttributes.ScopeMask) == MemberAttributes.Abstract)
				{
					base.Output.WriteLine(";");
					return;
				}
				this.OutputStartingBrace();
				base.Indent++;
				base.GenerateStatements(e.SetStatements);
				base.Indent--;
				base.Output.WriteLine("}");
			}
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00033522 File Offset: 0x00032522
		protected override void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				base.GenerateExpression(e.TargetObject);
				base.Output.Write(".");
			}
			this.OutputIdentifier(e.PropertyName);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00033554 File Offset: 0x00032554
		protected override void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e)
		{
			base.Output.Write("value");
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00033568 File Offset: 0x00032568
		protected override void GenerateRemoveEventStatement(CodeRemoveEventStatement e)
		{
			base.GenerateExpression(e.Event.TargetObject);
			base.Output.Write(".remove_");
			base.Output.Write(e.Event.EventName);
			base.Output.Write("(");
			base.GenerateExpression(e.Listener);
			base.Output.WriteLine(");");
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x000335D8 File Offset: 0x000325D8
		protected override void GenerateSingleFloatValue(float s)
		{
			base.Output.Write("float(");
			base.Output.Write(s.ToString(CultureInfo.InvariantCulture));
			base.Output.Write(")");
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00033611 File Offset: 0x00032611
		protected override void GenerateSnippetExpression(CodeSnippetExpression e)
		{
			base.Output.Write(e.Value);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00033624 File Offset: 0x00032624
		protected override void GenerateSnippetMember(CodeSnippetTypeMember e)
		{
			base.Output.Write(e.Text);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00033637 File Offset: 0x00032637
		protected override void GenerateSnippetStatement(CodeSnippetStatement e)
		{
			base.Output.WriteLine(e.Value);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0003364A File Offset: 0x0003264A
		protected override void GenerateThisReferenceExpression(CodeThisReferenceExpression e)
		{
			base.Output.Write("this");
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0003365C File Offset: 0x0003265C
		protected override void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e)
		{
			base.Output.Write("throw");
			if (e.ToThrow != null)
			{
				base.Output.Write(" ");
				base.GenerateExpression(e.ToThrow);
			}
			base.Output.WriteLine(";");
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x000336B0 File Offset: 0x000326B0
		protected override void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
		{
			base.Output.Write("try");
			this.OutputStartingBrace();
			base.Indent++;
			base.GenerateStatements(e.TryStatements);
			base.Indent--;
			CodeCatchClauseCollection catchClauses = e.CatchClauses;
			if (catchClauses.Count > 0)
			{
				IEnumerator enumerator = catchClauses.GetEnumerator();
				while (enumerator.MoveNext())
				{
					base.Output.Write("}");
					if (base.Options.ElseOnClosing)
					{
						base.Output.Write(" ");
					}
					else
					{
						base.Output.WriteLine("");
					}
					CodeCatchClause codeCatchClause = (CodeCatchClause)enumerator.Current;
					base.Output.Write("catch (");
					this.OutputIdentifier(codeCatchClause.LocalName);
					base.Output.Write(" : ");
					this.OutputType(codeCatchClause.CatchExceptionType);
					base.Output.Write(")");
					this.OutputStartingBrace();
					base.Indent++;
					base.GenerateStatements(codeCatchClause.Statements);
					base.Indent--;
				}
			}
			CodeStatementCollection finallyStatements = e.FinallyStatements;
			if (finallyStatements.Count > 0)
			{
				base.Output.Write("}");
				if (base.Options.ElseOnClosing)
				{
					base.Output.Write(" ");
				}
				else
				{
					base.Output.WriteLine("");
				}
				base.Output.Write("finally");
				this.OutputStartingBrace();
				base.Indent++;
				base.GenerateStatements(finallyStatements);
				base.Indent--;
			}
			base.Output.WriteLine("}");
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00033880 File Offset: 0x00032880
		protected override void GenerateTypeConstructor(CodeTypeConstructor e)
		{
			if (!base.IsCurrentClass && !base.IsCurrentStruct)
			{
				return;
			}
			base.Output.Write("static ");
			this.OutputIdentifier(base.CurrentTypeName);
			this.OutputStartingBrace();
			base.Indent++;
			base.GenerateStatements(e.Statements);
			base.Indent--;
			base.Output.WriteLine("}");
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x000338F8 File Offset: 0x000328F8
		protected override void GenerateTypeEnd(CodeTypeDeclaration e)
		{
			if (!base.IsCurrentDelegate)
			{
				base.Indent--;
				base.Output.WriteLine("}");
			}
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00033920 File Offset: 0x00032920
		protected override void GenerateTypeOfExpression(CodeTypeOfExpression e)
		{
			this.OutputType(e.Type);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00033930 File Offset: 0x00032930
		protected override string GetTypeOutput(CodeTypeReference typeRef)
		{
			string text;
			if (typeRef.ArrayElementType != null)
			{
				text = this.GetTypeOutput(typeRef.ArrayElementType);
			}
			else
			{
				text = this.GetBaseTypeOutput(typeRef.BaseType);
			}
			if (typeRef.ArrayRank > 0)
			{
				char[] array = new char[typeRef.ArrayRank + 1];
				array[0] = '[';
				array[typeRef.ArrayRank] = ']';
				for (int i = 1; i < typeRef.ArrayRank; i++)
				{
					array[i] = ',';
				}
				text += new string(array);
			}
			return text;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x000339AC File Offset: 0x000329AC
		protected override void GenerateTypeStart(CodeTypeDeclaration e)
		{
			if (base.IsCurrentDelegate)
			{
				throw new Exception(JScriptException.Localize("No delegate declarations", CultureInfo.CurrentUICulture));
			}
			this.OutputTypeVisibility(e.TypeAttributes);
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributeDeclarations(e.CustomAttributes);
				base.Output.WriteLine("");
			}
			this.OutputTypeAttributes(e.TypeAttributes, base.IsCurrentStruct, base.IsCurrentEnum);
			this.OutputIdentifier(e.Name);
			if (base.IsCurrentEnum)
			{
				if (e.BaseTypes.Count > 1)
				{
					throw new Exception(JScriptException.Localize("Too many base types", CultureInfo.CurrentUICulture));
				}
				if (e.BaseTypes.Count == 1)
				{
					base.Output.Write(" : ");
					this.OutputType(e.BaseTypes[0]);
				}
			}
			else
			{
				bool flag = true;
				bool flag2 = false;
				foreach (object obj in e.BaseTypes)
				{
					CodeTypeReference codeTypeReference = (CodeTypeReference)obj;
					if (flag)
					{
						base.Output.Write(" extends ");
						flag = false;
						flag2 = true;
					}
					else if (flag2)
					{
						base.Output.Write(" implements ");
						flag2 = false;
					}
					else
					{
						base.Output.Write(", ");
					}
					this.OutputType(codeTypeReference);
				}
			}
			this.OutputStartingBrace();
			base.Indent++;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00033B3C File Offset: 0x00032B3C
		protected override void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
		{
			base.Output.Write("var ");
			this.OutputTypeNamePair(e.Type, e.Name);
			if (e.InitExpression != null)
			{
				base.Output.Write(" = ");
				base.GenerateExpression(e.InitExpression);
			}
			base.Output.WriteLine(";");
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00033B9F File Offset: 0x00032B9F
		protected override void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e)
		{
			this.OutputIdentifier(e.VariableName);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00033BAD File Offset: 0x00032BAD
		private bool IsKeyword(string value)
		{
			return JSCodeGenerator.keywords.ContainsKey(value);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00033BBC File Offset: 0x00032BBC
		protected override bool IsValidIdentifier(string value)
		{
			if (value == null || value.Length == 0)
			{
				return false;
			}
			VsaEngine vsaEngine = VsaEngine.CreateEngine();
			return vsaEngine.IsValidIdentifier(value);
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00033BE3 File Offset: 0x00032BE3
		protected override string NullToken
		{
			get
			{
				return "null";
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00033BEC File Offset: 0x00032BEC
		protected override void OutputAttributeDeclarations(CodeAttributeDeclarationCollection attributes)
		{
			if (attributes.Count == 0)
			{
				return;
			}
			this.GenerateAttributeDeclarationsStart(attributes);
			foreach (object obj in attributes)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = (CodeAttributeDeclaration)obj;
				base.Output.Write(this.GetBaseTypeOutput(codeAttributeDeclaration.Name));
				base.Output.Write("(");
				bool flag = true;
				foreach (object obj2 in codeAttributeDeclaration.Arguments)
				{
					CodeAttributeArgument codeAttributeArgument = (CodeAttributeArgument)obj2;
					if (flag)
					{
						flag = false;
					}
					else
					{
						base.Output.Write(", ");
					}
					this.OutputAttributeArgument(codeAttributeArgument);
				}
				base.Output.Write(") ");
			}
			this.GenerateAttributeDeclarationsEnd(attributes);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00033CD8 File Offset: 0x00032CD8
		protected override void OutputDirection(FieldDirection dir)
		{
			switch (dir)
			{
			case FieldDirection.In:
				break;
			case FieldDirection.Out:
			case FieldDirection.Ref:
				if (!this.isArgumentList)
				{
					throw new Exception(JScriptException.Localize("No parameter direction", CultureInfo.CurrentUICulture));
				}
				base.Output.Write("&");
				break;
			default:
				return;
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00033D27 File Offset: 0x00032D27
		protected override void OutputIdentifier(string ident)
		{
			base.Output.Write(this.CreateEscapedIdentifier(ident));
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00033D3C File Offset: 0x00032D3C
		protected override void OutputMemberAccessModifier(MemberAttributes attributes)
		{
			MemberAttributes memberAttributes = attributes & MemberAttributes.AccessMask;
			if (memberAttributes <= MemberAttributes.FamilyAndAssembly)
			{
				if (memberAttributes == MemberAttributes.Assembly)
				{
					base.Output.Write("internal ");
					return;
				}
				if (memberAttributes == MemberAttributes.FamilyAndAssembly)
				{
					base.Output.Write("internal ");
					return;
				}
			}
			else
			{
				if (memberAttributes == MemberAttributes.Family)
				{
					base.Output.Write("protected ");
					return;
				}
				if (memberAttributes == MemberAttributes.FamilyOrAssembly)
				{
					base.Output.Write("protected internal ");
					return;
				}
				if (memberAttributes == MemberAttributes.Public)
				{
					base.Output.Write("public ");
					return;
				}
			}
			base.Output.Write("private ");
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00033DEC File Offset: 0x00032DEC
		protected override void OutputMemberScopeModifier(MemberAttributes attributes)
		{
			switch (attributes & MemberAttributes.ScopeMask)
			{
			case MemberAttributes.Abstract:
				base.Output.Write("abstract ");
				return;
			case MemberAttributes.Final:
				base.Output.Write("final ");
				return;
			case MemberAttributes.Static:
				base.Output.Write("static ");
				return;
			case MemberAttributes.Override:
				base.Output.Write("override ");
				return;
			default:
				return;
			}
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00033E5C File Offset: 0x00032E5C
		private void OutputMemberVTableModifier(MemberAttributes attributes)
		{
			MemberAttributes memberAttributes = attributes & MemberAttributes.VTableMask;
			if (memberAttributes != MemberAttributes.New)
			{
				return;
			}
			base.Output.Write("hide ");
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00033E88 File Offset: 0x00032E88
		protected override void OutputParameters(CodeParameterDeclarationExpressionCollection parameters)
		{
			bool flag = true;
			foreach (object obj in parameters)
			{
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = (CodeParameterDeclarationExpression)obj;
				if (flag)
				{
					flag = false;
				}
				else
				{
					base.Output.Write(", ");
				}
				base.GenerateExpression(codeParameterDeclarationExpression);
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00033ED4 File Offset: 0x00032ED4
		private void OutputStartingBrace()
		{
			if (base.Options.BracingStyle == "C")
			{
				base.Output.WriteLine("");
				base.Output.WriteLine("{");
				return;
			}
			base.Output.WriteLine(" {");
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00033F29 File Offset: 0x00032F29
		protected override void OutputType(CodeTypeReference typeRef)
		{
			base.Output.Write(this.GetTypeOutput(typeRef));
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00033F40 File Offset: 0x00032F40
		protected override void OutputTypeAttributes(TypeAttributes attributes, bool isStruct, bool isEnum)
		{
			if (isEnum)
			{
				base.Output.Write("enum ");
				return;
			}
			TypeAttributes typeAttributes = attributes & TypeAttributes.ClassSemanticsMask;
			if (typeAttributes == TypeAttributes.NotPublic)
			{
				if ((attributes & TypeAttributes.Sealed) == TypeAttributes.Sealed)
				{
					base.Output.Write("final ");
				}
				if ((attributes & TypeAttributes.Abstract) == TypeAttributes.Abstract)
				{
					base.Output.Write("abstract ");
				}
				base.Output.Write("class ");
				return;
			}
			if (typeAttributes != TypeAttributes.ClassSemanticsMask)
			{
				return;
			}
			base.Output.Write("interface ");
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00033FCD File Offset: 0x00032FCD
		protected override void OutputTypeNamePair(CodeTypeReference typeRef, string name)
		{
			this.OutputIdentifier(name);
			base.Output.Write(" : ");
			this.OutputType(typeRef);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00033FF0 File Offset: 0x00032FF0
		private void OutputTypeVisibility(TypeAttributes attributes)
		{
			switch (attributes & TypeAttributes.VisibilityMask)
			{
			case TypeAttributes.NotPublic:
				base.Output.Write("internal ");
				return;
			case TypeAttributes.NestedPublic:
				base.Output.Write("public static ");
				return;
			case TypeAttributes.NestedPrivate:
				base.Output.Write("private static ");
				return;
			case TypeAttributes.NestedFamily:
				base.Output.Write("protected static ");
				return;
			case TypeAttributes.NestedAssembly:
			case TypeAttributes.NestedFamANDAssem:
				base.Output.Write("internal static ");
				return;
			case TypeAttributes.VisibilityMask:
				base.Output.Write("protected internal static ");
				return;
			}
			base.Output.Write("public ");
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x000340A0 File Offset: 0x000330A0
		protected override void ProcessCompilerOutputLine(CompilerResults results, string line)
		{
			Match match = JSCodeGenerator.outputReg.Match(line);
			if (match.Success)
			{
				CompilerError compilerError = new CompilerError();
				if (match.Groups[1].Success)
				{
					compilerError.FileName = match.Groups[2].Value;
					compilerError.Line = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);
					compilerError.Column = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);
				}
				if (string.Compare(match.Groups[7].Value, "warning", StringComparison.OrdinalIgnoreCase) == 0)
				{
					compilerError.IsWarning = true;
				}
				compilerError.ErrorNumber = match.Groups[8].Value;
				compilerError.ErrorText = match.Groups[9].Value;
				results.Errors.Add(compilerError);
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00034197 File Offset: 0x00033197
		protected override string QuoteSnippetString(string value)
		{
			return this.QuoteSnippetStringCStyle(value);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x000341A0 File Offset: 0x000331A0
		private string QuoteSnippetStringCStyle(string value)
		{
			char[] array = value.ToCharArray();
			StringBuilder stringBuilder = new StringBuilder(value.Length + 5);
			stringBuilder.Append("\"");
			int num = 80;
			int i = 0;
			while (i < array.Length)
			{
				char c = array[i];
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
							goto IL_0108;
						case '\r':
							stringBuilder.Append("\\r");
							break;
						default:
							if (c != '"')
							{
								goto IL_0108;
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
							stringBuilder.Append("\\u2028");
							break;
						case '\u2029':
							stringBuilder.Append("\\u2029");
							break;
						default:
							goto IL_0108;
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
				IL_0112:
				if (i >= num && i + 1 < array.Length && (!this.IsSurrogateStart(array[i]) || !this.IsSurrogateEnd(array[i + 1])))
				{
					num = i + 80;
					stringBuilder.Append("\" + \r\n\"");
				}
				i++;
				continue;
				IL_0108:
				stringBuilder.Append(array[i]);
				goto IL_0112;
			}
			stringBuilder.Append("\"");
			return stringBuilder.ToString();
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00034313 File Offset: 0x00033313
		private bool IsSurrogateStart(char c)
		{
			return '\ud800' <= c && c <= '\udbff';
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0003432A File Offset: 0x0003332A
		private bool IsSurrogateEnd(char c)
		{
			return '\udc00' <= c && c <= '\udfff';
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00034344 File Offset: 0x00033344
		protected override void GeneratePrimitiveExpression(CodePrimitiveExpression e)
		{
			if (e.Value == null)
			{
				base.Output.Write("undefined");
				return;
			}
			if (e.Value is DBNull)
			{
				base.Output.Write("null");
				return;
			}
			if (e.Value is char)
			{
				this.GeneratePrimitiveChar((char)e.Value);
				return;
			}
			base.GeneratePrimitiveExpression(e);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x000343B0 File Offset: 0x000333B0
		private void GeneratePrimitiveChar(char c)
		{
			base.Output.Write('\'');
			if (c <= '"')
			{
				if (c == '\0')
				{
					base.Output.Write("\\0");
					goto IL_0119;
				}
				switch (c)
				{
				case '\t':
					base.Output.Write("\\t");
					goto IL_0119;
				case '\n':
					base.Output.Write("\\n");
					goto IL_0119;
				case '\v':
				case '\f':
					break;
				case '\r':
					base.Output.Write("\\r");
					goto IL_0119;
				default:
					if (c == '"')
					{
						base.Output.Write("\\\"");
						goto IL_0119;
					}
					break;
				}
			}
			else
			{
				if (c == '\'')
				{
					base.Output.Write("\\'");
					goto IL_0119;
				}
				if (c == '\\')
				{
					base.Output.Write("\\\\");
					goto IL_0119;
				}
				switch (c)
				{
				case '\u2028':
					base.Output.Write("\\u2028");
					goto IL_0119;
				case '\u2029':
					base.Output.Write("\\u2029");
					goto IL_0119;
				}
			}
			base.Output.Write(c);
			IL_0119:
			base.Output.Write('\'');
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x000344E3 File Offset: 0x000334E3
		protected override bool Supports(GeneratorSupport support)
		{
			return (support & (GeneratorSupport.ArraysOfArrays | GeneratorSupport.EntryPointMethod | GeneratorSupport.MultidimensionalArrays | GeneratorSupport.StaticConstructors | GeneratorSupport.TryCatchStatements | GeneratorSupport.DeclareEnums | GeneratorSupport.DeclareInterfaces | GeneratorSupport.AssemblyAttributes | GeneratorSupport.PublicStaticMembers)) == support;
		}

		// Token: 0x04000324 RID: 804
		private const int MaxLineLength = 80;

		// Token: 0x04000325 RID: 805
		private const GeneratorSupport LanguageSupport = GeneratorSupport.ArraysOfArrays | GeneratorSupport.EntryPointMethod | GeneratorSupport.MultidimensionalArrays | GeneratorSupport.StaticConstructors | GeneratorSupport.TryCatchStatements | GeneratorSupport.DeclareEnums | GeneratorSupport.DeclareInterfaces | GeneratorSupport.AssemblyAttributes | GeneratorSupport.PublicStaticMembers;

		// Token: 0x04000326 RID: 806
		private bool forLoopHack;

		// Token: 0x04000327 RID: 807
		private bool isArgumentList = true;

		// Token: 0x04000328 RID: 808
		private static Hashtable keywords = new Hashtable(150);

		// Token: 0x04000329 RID: 809
		private string mainClassName;

		// Token: 0x0400032A RID: 810
		private string mainMethodName;

		// Token: 0x0400032B RID: 811
		private static Regex outputReg = new Regex("(([^(]+)(\\(([0-9]+),([0-9]+)\\))[ \\t]*:[ \\t]+)?(fatal )?(error|warning)[ \\t]+([A-Z]+[0-9]+)[ \\t]*:[ \\t]*(.*)");
	}
}
