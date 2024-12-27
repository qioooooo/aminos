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
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.VisualBasic
{
	// Token: 0x020002BE RID: 702
	internal class VBCodeGenerator : CodeCompiler
	{
		// Token: 0x0600178D RID: 6029 RVA: 0x0004E6D1 File Offset: 0x0004D6D1
		internal VBCodeGenerator()
		{
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0004E6D9 File Offset: 0x0004D6D9
		internal VBCodeGenerator(IDictionary<string, string> providerOptions)
		{
			this.provOptions = providerOptions;
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x0600178F RID: 6031 RVA: 0x0004E6E8 File Offset: 0x0004D6E8
		protected override string FileExtension
		{
			get
			{
				return ".vb";
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x0004E6EF File Offset: 0x0004D6EF
		protected override string CompilerName
		{
			get
			{
				return "vbc.exe";
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x0004E6F6 File Offset: 0x0004D6F6
		private bool IsCurrentModule
		{
			get
			{
				return base.IsCurrentClass && this.GetUserData(base.CurrentClass, "Module", false);
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001792 RID: 6034 RVA: 0x0004E714 File Offset: 0x0004D714
		protected override string NullToken
		{
			get
			{
				return "Nothing";
			}
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0004E71B File Offset: 0x0004D71B
		private void EnsureInDoubleQuotes(ref bool fInDoubleQuotes, StringBuilder b)
		{
			if (fInDoubleQuotes)
			{
				return;
			}
			b.Append("&\"");
			fInDoubleQuotes = true;
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0004E731 File Offset: 0x0004D731
		private void EnsureNotInDoubleQuotes(ref bool fInDoubleQuotes, StringBuilder b)
		{
			if (!fInDoubleQuotes)
			{
				return;
			}
			b.Append("\"");
			fInDoubleQuotes = false;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0004E748 File Offset: 0x0004D748
		protected override string QuoteSnippetString(string value)
		{
			StringBuilder stringBuilder = new StringBuilder(value.Length + 5);
			bool flag = true;
			Indentation indentation = new Indentation((IndentedTextWriter)base.Output, base.Indent + 1);
			stringBuilder.Append("\"");
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				char c2 = c;
				if (c2 <= '"')
				{
					if (c2 != '\0')
					{
						switch (c2)
						{
						case '\t':
							this.EnsureNotInDoubleQuotes(ref flag, stringBuilder);
							stringBuilder.Append("&Global.Microsoft.VisualBasic.ChrW(9)");
							break;
						case '\n':
							this.EnsureNotInDoubleQuotes(ref flag, stringBuilder);
							stringBuilder.Append("&Global.Microsoft.VisualBasic.ChrW(10)");
							break;
						case '\v':
						case '\f':
							goto IL_016F;
						case '\r':
							this.EnsureNotInDoubleQuotes(ref flag, stringBuilder);
							if (i < value.Length - 1 && value[i + 1] == '\n')
							{
								stringBuilder.Append("&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)");
								i++;
							}
							else
							{
								stringBuilder.Append("&Global.Microsoft.VisualBasic.ChrW(13)");
							}
							break;
						default:
							if (c2 != '"')
							{
								goto IL_016F;
							}
							goto IL_00B6;
						}
					}
					else
					{
						this.EnsureNotInDoubleQuotes(ref flag, stringBuilder);
						stringBuilder.Append("&Global.Microsoft.VisualBasic.ChrW(0)");
					}
				}
				else
				{
					switch (c2)
					{
					case '“':
					case '”':
						goto IL_00B6;
					default:
						switch (c2)
						{
						case '\u2028':
						case '\u2029':
							this.EnsureNotInDoubleQuotes(ref flag, stringBuilder);
							VBCodeGenerator.AppendEscapedChar(stringBuilder, c);
							break;
						default:
							if (c2 == '＂')
							{
								goto IL_00B6;
							}
							goto IL_016F;
						}
						break;
					}
				}
				IL_0186:
				if (i > 0 && i % 80 == 0)
				{
					if (char.IsHighSurrogate(value[i]) && i < value.Length - 1 && char.IsLowSurrogate(value[i + 1]))
					{
						stringBuilder.Append(value[++i]);
					}
					if (flag)
					{
						stringBuilder.Append("\"");
					}
					flag = true;
					stringBuilder.Append("& _ \r\n");
					stringBuilder.Append(indentation.IndentationString);
					stringBuilder.Append('"');
				}
				i++;
				continue;
				IL_00B6:
				this.EnsureInDoubleQuotes(ref flag, stringBuilder);
				stringBuilder.Append(c);
				stringBuilder.Append(c);
				goto IL_0186;
				IL_016F:
				this.EnsureInDoubleQuotes(ref flag, stringBuilder);
				stringBuilder.Append(value[i]);
				goto IL_0186;
			}
			if (flag)
			{
				stringBuilder.Append("\"");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x0004E978 File Offset: 0x0004D978
		private static void AppendEscapedChar(StringBuilder b, char value)
		{
			b.Append("&Global.Microsoft.VisualBasic.ChrW(");
			int num = (int)value;
			b.Append(num.ToString(CultureInfo.InvariantCulture));
			b.Append(")");
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x0004E9B4 File Offset: 0x0004D9B4
		protected override void ProcessCompilerOutputLine(CompilerResults results, string line)
		{
			if (VBCodeGenerator.outputReg == null)
			{
				VBCodeGenerator.outputReg = new Regex("^([^(]*)\\(?([0-9]*)\\)? ?:? ?(error|warning) ([A-Z]+[0-9]+) ?: (.*)");
			}
			Match match = VBCodeGenerator.outputReg.Match(line);
			if (match.Success)
			{
				CompilerError compilerError = new CompilerError();
				compilerError.FileName = match.Groups[1].Value;
				string value = match.Groups[2].Value;
				if (value != null && value.Length > 0)
				{
					compilerError.Line = int.Parse(value, CultureInfo.InvariantCulture);
				}
				if (string.Compare(match.Groups[3].Value, "warning", StringComparison.OrdinalIgnoreCase) == 0)
				{
					compilerError.IsWarning = true;
				}
				compilerError.ErrorNumber = match.Groups[4].Value;
				compilerError.ErrorText = match.Groups[5].Value;
				results.Errors.Add(compilerError);
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x0004EA9C File Offset: 0x0004DA9C
		protected override string CmdArgsFromParameters(CompilerParameters options)
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
				string fileName = Path.GetFileName(text);
				if (string.Compare(fileName, "Microsoft.VisualBasic.dll", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(fileName, "mscorlib.dll", StringComparison.OrdinalIgnoreCase) != 0)
				{
					stringBuilder.Append("/R:");
					stringBuilder.Append("\"");
					stringBuilder.Append(text);
					stringBuilder.Append("\"");
					stringBuilder.Append(" ");
				}
			}
			stringBuilder.Append("/out:");
			stringBuilder.Append("\"");
			stringBuilder.Append(options.OutputAssembly);
			stringBuilder.Append("\"");
			stringBuilder.Append(" ");
			if (options.IncludeDebugInformation)
			{
				stringBuilder.Append("/D:DEBUG=1 ");
				stringBuilder.Append("/debug+ ");
			}
			else
			{
				stringBuilder.Append("/debug- ");
			}
			if (options.Win32Resource != null)
			{
				stringBuilder.Append("/win32resource:\"" + options.Win32Resource + "\" ");
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
				stringBuilder.Append("/warnaserror+ ");
			}
			if (options.CompilerOptions != null)
			{
				stringBuilder.Append(options.CompilerOptions + " ");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x0004ED50 File Offset: 0x0004DD50
		protected override void OutputAttributeArgument(CodeAttributeArgument arg)
		{
			if (arg.Name != null && arg.Name.Length > 0)
			{
				this.OutputIdentifier(arg.Name);
				base.Output.Write(":=");
			}
			((ICodeGenerator)this).GenerateCodeFromExpression(arg.Value, ((IndentedTextWriter)base.Output).InnerWriter, base.Options);
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x0004EDB1 File Offset: 0x0004DDB1
		private void OutputAttributes(CodeAttributeDeclarationCollection attributes, bool inLine)
		{
			this.OutputAttributes(attributes, inLine, null, false);
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x0004EDC0 File Offset: 0x0004DDC0
		private void OutputAttributes(CodeAttributeDeclarationCollection attributes, bool inLine, string prefix, bool closingLine)
		{
			if (attributes.Count == 0)
			{
				return;
			}
			IEnumerator enumerator = attributes.GetEnumerator();
			bool flag = true;
			this.GenerateAttributeDeclarationsStart(attributes);
			while (enumerator.MoveNext())
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					base.Output.Write(", ");
					if (!inLine)
					{
						this.ContinueOnNewLine("");
						base.Output.Write(" ");
					}
				}
				if (prefix != null && prefix.Length > 0)
				{
					base.Output.Write(prefix);
				}
				CodeAttributeDeclaration codeAttributeDeclaration = (CodeAttributeDeclaration)enumerator.Current;
				if (codeAttributeDeclaration.AttributeType != null)
				{
					base.Output.Write(this.GetTypeOutput(codeAttributeDeclaration.AttributeType));
				}
				base.Output.Write("(");
				bool flag2 = true;
				foreach (object obj in codeAttributeDeclaration.Arguments)
				{
					CodeAttributeArgument codeAttributeArgument = (CodeAttributeArgument)obj;
					if (flag2)
					{
						flag2 = false;
					}
					else
					{
						base.Output.Write(", ");
					}
					this.OutputAttributeArgument(codeAttributeArgument);
				}
				base.Output.Write(")");
			}
			this.GenerateAttributeDeclarationsEnd(attributes);
			base.Output.Write(" ");
			if (!inLine)
			{
				if (closingLine)
				{
					base.Output.WriteLine();
					return;
				}
				this.ContinueOnNewLine("");
			}
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x0004EF34 File Offset: 0x0004DF34
		protected override void OutputDirection(FieldDirection dir)
		{
			switch (dir)
			{
			case FieldDirection.In:
				base.Output.Write("ByVal ");
				return;
			case FieldDirection.Out:
			case FieldDirection.Ref:
				base.Output.Write("ByRef ");
				return;
			default:
				return;
			}
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x0004EF77 File Offset: 0x0004DF77
		protected override void GenerateDefaultValueExpression(CodeDefaultValueExpression e)
		{
			base.Output.Write("CType(Nothing, " + this.GetTypeOutput(e.Type) + ")");
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x0004EF9F File Offset: 0x0004DF9F
		protected override void GenerateDirectionExpression(CodeDirectionExpression e)
		{
			base.GenerateExpression(e.Expression);
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x0004EFB0 File Offset: 0x0004DFB0
		protected override void OutputFieldScopeModifier(MemberAttributes attributes)
		{
			switch (attributes & MemberAttributes.ScopeMask)
			{
			case MemberAttributes.Final:
				base.Output.Write("");
				return;
			case MemberAttributes.Static:
				if (!this.IsCurrentModule)
				{
					base.Output.Write("Shared ");
					return;
				}
				return;
			case MemberAttributes.Const:
				base.Output.Write("Const ");
				return;
			}
			base.Output.Write("");
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0004F028 File Offset: 0x0004E028
		protected override void OutputMemberAccessModifier(MemberAttributes attributes)
		{
			MemberAttributes memberAttributes = attributes & MemberAttributes.AccessMask;
			if (memberAttributes <= MemberAttributes.Family)
			{
				if (memberAttributes == MemberAttributes.Assembly)
				{
					base.Output.Write("Friend ");
					return;
				}
				if (memberAttributes == MemberAttributes.FamilyAndAssembly)
				{
					base.Output.Write("Friend ");
					return;
				}
				if (memberAttributes != MemberAttributes.Family)
				{
					return;
				}
				base.Output.Write("Protected ");
				return;
			}
			else
			{
				if (memberAttributes == MemberAttributes.FamilyOrAssembly)
				{
					base.Output.Write("Protected Friend ");
					return;
				}
				if (memberAttributes == MemberAttributes.Private)
				{
					base.Output.Write("Private ");
					return;
				}
				if (memberAttributes != MemberAttributes.Public)
				{
					return;
				}
				base.Output.Write("Public ");
				return;
			}
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x0004F0DC File Offset: 0x0004E0DC
		private void OutputVTableModifier(MemberAttributes attributes)
		{
			MemberAttributes memberAttributes = attributes & MemberAttributes.VTableMask;
			if (memberAttributes != MemberAttributes.New)
			{
				return;
			}
			base.Output.Write("Shadows ");
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x0004F108 File Offset: 0x0004E108
		protected override void OutputMemberScopeModifier(MemberAttributes attributes)
		{
			MemberAttributes memberAttributes = attributes & MemberAttributes.ScopeMask;
			switch (memberAttributes)
			{
			case MemberAttributes.Abstract:
				base.Output.Write("MustOverride ");
				return;
			case MemberAttributes.Final:
				base.Output.Write("");
				return;
			case MemberAttributes.Static:
				if (!this.IsCurrentModule)
				{
					base.Output.Write("Shared ");
					return;
				}
				break;
			case MemberAttributes.Override:
				base.Output.Write("Overrides ");
				return;
			default:
			{
				if (memberAttributes == MemberAttributes.Private)
				{
					base.Output.Write("Private ");
					return;
				}
				MemberAttributes memberAttributes2 = attributes & MemberAttributes.AccessMask;
				if (memberAttributes2 != MemberAttributes.Assembly && memberAttributes2 != MemberAttributes.Family && memberAttributes2 != MemberAttributes.Public)
				{
					return;
				}
				base.Output.Write("Overridable ");
				break;
			}
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x0004F1CC File Offset: 0x0004E1CC
		protected override void OutputOperator(CodeBinaryOperatorType op)
		{
			switch (op)
			{
			case CodeBinaryOperatorType.Modulus:
				base.Output.Write("Mod");
				return;
			case CodeBinaryOperatorType.IdentityInequality:
				base.Output.Write("<>");
				return;
			case CodeBinaryOperatorType.IdentityEquality:
				base.Output.Write("Is");
				return;
			case CodeBinaryOperatorType.ValueEquality:
				base.Output.Write("=");
				return;
			case CodeBinaryOperatorType.BitwiseOr:
				base.Output.Write("Or");
				return;
			case CodeBinaryOperatorType.BitwiseAnd:
				base.Output.Write("And");
				return;
			case CodeBinaryOperatorType.BooleanOr:
				base.Output.Write("OrElse");
				return;
			case CodeBinaryOperatorType.BooleanAnd:
				base.Output.Write("AndAlso");
				return;
			}
			base.OutputOperator(op);
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x0004F29C File Offset: 0x0004E29C
		private void GenerateNotIsNullExpression(CodeExpression e)
		{
			base.Output.Write("(Not (");
			base.GenerateExpression(e);
			base.Output.Write(") Is ");
			base.Output.Write(this.NullToken);
			base.Output.Write(")");
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x0004F2F4 File Offset: 0x0004E2F4
		protected override void GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression e)
		{
			if (e.Operator != CodeBinaryOperatorType.IdentityInequality)
			{
				base.GenerateBinaryOperatorExpression(e);
				return;
			}
			if (e.Right is CodePrimitiveExpression && ((CodePrimitiveExpression)e.Right).Value == null)
			{
				this.GenerateNotIsNullExpression(e.Left);
				return;
			}
			if (e.Left is CodePrimitiveExpression && ((CodePrimitiveExpression)e.Left).Value == null)
			{
				this.GenerateNotIsNullExpression(e.Right);
				return;
			}
			base.GenerateBinaryOperatorExpression(e);
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x0004F371 File Offset: 0x0004E371
		protected override string GetResponseFileCmdArgs(CompilerParameters options, string cmdArgs)
		{
			return "/noconfig " + base.GetResponseFileCmdArgs(options, cmdArgs);
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x0004F385 File Offset: 0x0004E385
		protected override void OutputIdentifier(string ident)
		{
			base.Output.Write(this.CreateEscapedIdentifier(ident));
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x0004F399 File Offset: 0x0004E399
		protected override void OutputType(CodeTypeReference typeRef)
		{
			base.Output.Write(this.GetTypeOutputWithoutArrayPostFix(typeRef));
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x0004F3B0 File Offset: 0x0004E3B0
		private void OutputTypeAttributes(CodeTypeDeclaration e)
		{
			if ((e.Attributes & MemberAttributes.New) != (MemberAttributes)0)
			{
				base.Output.Write("Shadows ");
			}
			TypeAttributes typeAttributes = e.TypeAttributes;
			if (e.IsPartial)
			{
				base.Output.Write("Partial ");
			}
			switch (typeAttributes & TypeAttributes.VisibilityMask)
			{
			case TypeAttributes.NotPublic:
			case TypeAttributes.NestedAssembly:
			case TypeAttributes.NestedFamANDAssem:
				base.Output.Write("Friend ");
				break;
			case TypeAttributes.Public:
			case TypeAttributes.NestedPublic:
				base.Output.Write("Public ");
				break;
			case TypeAttributes.NestedPrivate:
				base.Output.Write("Private ");
				break;
			case TypeAttributes.NestedFamily:
				base.Output.Write("Protected ");
				break;
			case TypeAttributes.VisibilityMask:
				base.Output.Write("Protected Friend ");
				break;
			}
			if (e.IsStruct)
			{
				base.Output.Write("Structure ");
				return;
			}
			if (e.IsEnum)
			{
				base.Output.Write("Enum ");
				return;
			}
			TypeAttributes typeAttributes2 = typeAttributes & TypeAttributes.ClassSemanticsMask;
			if (typeAttributes2 != TypeAttributes.NotPublic)
			{
				if (typeAttributes2 != TypeAttributes.ClassSemanticsMask)
				{
					return;
				}
				base.Output.Write("Interface ");
				return;
			}
			else
			{
				if (this.IsCurrentModule)
				{
					base.Output.Write("Module ");
					return;
				}
				if ((typeAttributes & TypeAttributes.Sealed) == TypeAttributes.Sealed)
				{
					base.Output.Write("NotInheritable ");
				}
				if ((typeAttributes & TypeAttributes.Abstract) == TypeAttributes.Abstract)
				{
					base.Output.Write("MustInherit ");
				}
				base.Output.Write("Class ");
				return;
			}
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x0004F532 File Offset: 0x0004E532
		protected override void OutputTypeNamePair(CodeTypeReference typeRef, string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				name = "__exception";
			}
			this.OutputIdentifier(name);
			this.OutputArrayPostfix(typeRef);
			base.Output.Write(" As ");
			this.OutputType(typeRef);
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x0004F568 File Offset: 0x0004E568
		private string GetArrayPostfix(CodeTypeReference typeRef)
		{
			string text = "";
			if (typeRef.ArrayElementType != null)
			{
				text = this.GetArrayPostfix(typeRef.ArrayElementType);
			}
			if (typeRef.ArrayRank > 0)
			{
				char[] array = new char[typeRef.ArrayRank + 1];
				array[0] = '(';
				array[typeRef.ArrayRank] = ')';
				for (int i = 1; i < typeRef.ArrayRank; i++)
				{
					array[i] = ',';
				}
				text = new string(array) + text;
			}
			return text;
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x0004F5DA File Offset: 0x0004E5DA
		private void OutputArrayPostfix(CodeTypeReference typeRef)
		{
			if (typeRef.ArrayRank > 0)
			{
				base.Output.Write(this.GetArrayPostfix(typeRef));
			}
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x0004F5F8 File Offset: 0x0004E5F8
		protected override void GenerateIterationStatement(CodeIterationStatement e)
		{
			base.GenerateStatement(e.InitStatement);
			base.Output.Write("Do While ");
			base.GenerateExpression(e.TestExpression);
			base.Output.WriteLine("");
			base.Indent++;
			this.GenerateVBStatements(e.Statements);
			base.GenerateStatement(e.IncrementStatement);
			base.Indent--;
			base.Output.WriteLine("Loop");
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0004F684 File Offset: 0x0004E684
		protected override void GeneratePrimitiveExpression(CodePrimitiveExpression e)
		{
			if (e.Value is char)
			{
				base.Output.Write("Global.Microsoft.VisualBasic.ChrW(" + ((IConvertible)e.Value).ToInt32(CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture) + ")");
				return;
			}
			if (e.Value is sbyte)
			{
				base.Output.Write("CSByte(");
				base.Output.Write(((sbyte)e.Value).ToString(CultureInfo.InvariantCulture));
				base.Output.Write(")");
				return;
			}
			if (e.Value is ushort)
			{
				base.Output.Write(((ushort)e.Value).ToString(CultureInfo.InvariantCulture));
				base.Output.Write("US");
				return;
			}
			if (e.Value is uint)
			{
				base.Output.Write(((uint)e.Value).ToString(CultureInfo.InvariantCulture));
				base.Output.Write("UI");
				return;
			}
			if (e.Value is ulong)
			{
				base.Output.Write(((ulong)e.Value).ToString(CultureInfo.InvariantCulture));
				base.Output.Write("UL");
				return;
			}
			base.GeneratePrimitiveExpression(e);
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x0004F7F8 File Offset: 0x0004E7F8
		protected override void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e)
		{
			base.Output.Write("Throw");
			if (e.ToThrow != null)
			{
				base.Output.Write(" ");
				base.GenerateExpression(e.ToThrow);
			}
			base.Output.WriteLine("");
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x0004F84C File Offset: 0x0004E84C
		protected override void GenerateArrayCreateExpression(CodeArrayCreateExpression e)
		{
			base.Output.Write("New ");
			CodeExpressionCollection initializers = e.Initializers;
			if (initializers.Count > 0)
			{
				string typeOutput = this.GetTypeOutput(e.CreateType);
				base.Output.Write(typeOutput);
				if (typeOutput.IndexOf('(') == -1)
				{
					base.Output.Write("()");
				}
				base.Output.Write(" {");
				base.Indent++;
				this.OutputExpressionList(initializers);
				base.Indent--;
				base.Output.Write("}");
				return;
			}
			string typeOutput2 = this.GetTypeOutput(e.CreateType);
			int num = typeOutput2.IndexOf('(');
			if (num == -1)
			{
				base.Output.Write(typeOutput2);
				base.Output.Write('(');
			}
			else
			{
				base.Output.Write(typeOutput2.Substring(0, num + 1));
			}
			if (e.SizeExpression != null)
			{
				base.Output.Write("(");
				base.GenerateExpression(e.SizeExpression);
				base.Output.Write(") - 1");
			}
			else
			{
				base.Output.Write(e.Size - 1);
			}
			if (num == -1)
			{
				base.Output.Write(')');
			}
			else
			{
				base.Output.Write(typeOutput2.Substring(num + 1));
			}
			base.Output.Write(" {}");
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x0004F9BB File Offset: 0x0004E9BB
		protected override void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e)
		{
			base.Output.Write("MyBase");
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x0004F9D0 File Offset: 0x0004E9D0
		protected override void GenerateCastExpression(CodeCastExpression e)
		{
			base.Output.Write("CType(");
			base.GenerateExpression(e.Expression);
			base.Output.Write(",");
			this.OutputType(e.TargetType);
			this.OutputArrayPostfix(e.TargetType);
			base.Output.Write(")");
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x0004FA31 File Offset: 0x0004EA31
		protected override void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e)
		{
			base.Output.Write("AddressOf ");
			base.GenerateExpression(e.TargetObject);
			base.Output.Write(".");
			this.OutputIdentifier(e.MethodName);
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0004FA6B File Offset: 0x0004EA6B
		protected override void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				base.GenerateExpression(e.TargetObject);
				base.Output.Write(".");
			}
			this.OutputIdentifier(e.FieldName);
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x0004FAA0 File Offset: 0x0004EAA0
		protected override void GenerateSingleFloatValue(float s)
		{
			if (float.IsNaN(s))
			{
				base.Output.Write("Single.NaN");
				return;
			}
			if (float.IsNegativeInfinity(s))
			{
				base.Output.Write("Single.NegativeInfinity");
				return;
			}
			if (float.IsPositiveInfinity(s))
			{
				base.Output.Write("Single.PositiveInfinity");
				return;
			}
			base.Output.Write(s.ToString(CultureInfo.InvariantCulture));
			base.Output.Write('!');
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0004FB1C File Offset: 0x0004EB1C
		protected override void GenerateDoubleValue(double d)
		{
			if (double.IsNaN(d))
			{
				base.Output.Write("Double.NaN");
				return;
			}
			if (double.IsNegativeInfinity(d))
			{
				base.Output.Write("Double.NegativeInfinity");
				return;
			}
			if (double.IsPositiveInfinity(d))
			{
				base.Output.Write("Double.PositiveInfinity");
				return;
			}
			base.Output.Write(d.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0004FB90 File Offset: 0x0004EB90
		protected override void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
		{
			this.OutputIdentifier(e.ParameterName);
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0004FB9E File Offset: 0x0004EB9E
		protected override void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e)
		{
			this.OutputIdentifier(e.VariableName);
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x0004FBAC File Offset: 0x0004EBAC
		protected override void GenerateIndexerExpression(CodeIndexerExpression e)
		{
			base.GenerateExpression(e.TargetObject);
			if (e.TargetObject is CodeBaseReferenceExpression)
			{
				base.Output.Write(".Item");
			}
			base.Output.Write("(");
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
			base.Output.Write(")");
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x0004FC68 File Offset: 0x0004EC68
		protected override void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e)
		{
			base.GenerateExpression(e.TargetObject);
			base.Output.Write("(");
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
			base.Output.Write(")");
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x0004FD08 File Offset: 0x0004ED08
		protected override void GenerateSnippetExpression(CodeSnippetExpression e)
		{
			base.Output.Write(e.Value);
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x0004FD1C File Offset: 0x0004ED1C
		protected override void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e)
		{
			this.GenerateMethodReferenceExpression(e.Method);
			CodeExpressionCollection parameters = e.Parameters;
			if (parameters.Count > 0)
			{
				base.Output.Write("(");
				this.OutputExpressionList(e.Parameters);
				base.Output.Write(")");
			}
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x0004FD74 File Offset: 0x0004ED74
		protected override void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				base.GenerateExpression(e.TargetObject);
				base.Output.Write(".");
				base.Output.Write(e.MethodName);
			}
			else
			{
				this.OutputIdentifier(e.MethodName);
			}
			if (e.TypeArguments.Count > 0)
			{
				base.Output.Write(this.GetTypeArgumentsOutput(e.TypeArguments));
			}
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x0004FDEC File Offset: 0x0004EDEC
		protected override void GenerateEventReferenceExpression(CodeEventReferenceExpression e)
		{
			if (e.TargetObject == null)
			{
				this.OutputIdentifier(e.EventName + "Event");
				return;
			}
			bool flag = e.TargetObject is CodeThisReferenceExpression;
			base.GenerateExpression(e.TargetObject);
			base.Output.Write(".");
			if (flag)
			{
				base.Output.Write(e.EventName + "Event");
				return;
			}
			base.Output.Write(e.EventName);
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0004FE73 File Offset: 0x0004EE73
		private void GenerateFormalEventReferenceExpression(CodeEventReferenceExpression e)
		{
			if (e.TargetObject != null && !(e.TargetObject is CodeThisReferenceExpression))
			{
				base.GenerateExpression(e.TargetObject);
				base.Output.Write(".");
			}
			this.OutputIdentifier(e.EventName);
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0004FEB4 File Offset: 0x0004EEB4
		protected override void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
		{
			if (e.TargetObject != null)
			{
				if (e.TargetObject is CodeEventReferenceExpression)
				{
					base.Output.Write("RaiseEvent ");
					this.GenerateFormalEventReferenceExpression((CodeEventReferenceExpression)e.TargetObject);
				}
				else
				{
					base.GenerateExpression(e.TargetObject);
				}
			}
			CodeExpressionCollection parameters = e.Parameters;
			if (parameters.Count > 0)
			{
				base.Output.Write("(");
				this.OutputExpressionList(e.Parameters);
				base.Output.Write(")");
			}
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x0004FF44 File Offset: 0x0004EF44
		protected override void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
		{
			base.Output.Write("New ");
			this.OutputType(e.CreateType);
			CodeExpressionCollection parameters = e.Parameters;
			if (parameters.Count > 0)
			{
				base.Output.Write("(");
				this.OutputExpressionList(parameters);
				base.Output.Write(")");
			}
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0004FFA4 File Offset: 0x0004EFA4
		protected override void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributes(e.CustomAttributes, true);
			}
			this.OutputDirection(e.Direction);
			this.OutputTypeNamePair(e.Type, e.Name);
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x0004FFDF File Offset: 0x0004EFDF
		protected override void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e)
		{
			base.Output.Write("value");
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0004FFF1 File Offset: 0x0004EFF1
		protected override void GenerateThisReferenceExpression(CodeThisReferenceExpression e)
		{
			base.Output.Write("Me");
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x00050003 File Offset: 0x0004F003
		protected override void GenerateExpressionStatement(CodeExpressionStatement e)
		{
			base.GenerateExpression(e.Expression);
			base.Output.WriteLine("");
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x00050021 File Offset: 0x0004F021
		private bool IsDocComment(CodeCommentStatement comment)
		{
			return comment != null && comment.Comment != null && comment.Comment.DocComment;
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x0005003C File Offset: 0x0004F03C
		protected override void GenerateCommentStatements(CodeCommentStatementCollection e)
		{
			foreach (object obj in e)
			{
				CodeCommentStatement codeCommentStatement = (CodeCommentStatement)obj;
				if (!this.IsDocComment(codeCommentStatement))
				{
					this.GenerateCommentStatement(codeCommentStatement);
				}
			}
			foreach (object obj2 in e)
			{
				CodeCommentStatement codeCommentStatement2 = (CodeCommentStatement)obj2;
				if (this.IsDocComment(codeCommentStatement2))
				{
					this.GenerateCommentStatement(codeCommentStatement2);
				}
			}
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x000500EC File Offset: 0x0004F0EC
		protected override void GenerateComment(CodeComment e)
		{
			string text = (e.DocComment ? "'''" : "'");
			base.Output.Write(text);
			string text2 = e.Text;
			for (int i = 0; i < text2.Length; i++)
			{
				base.Output.Write(text2[i]);
				if (text2[i] == '\r')
				{
					if (i < text2.Length - 1 && text2[i + 1] == '\n')
					{
						base.Output.Write('\n');
						i++;
					}
					((IndentedTextWriter)base.Output).InternalOutputTabs();
					base.Output.Write(text);
				}
				else if (text2[i] == '\n')
				{
					((IndentedTextWriter)base.Output).InternalOutputTabs();
					base.Output.Write(text);
				}
				else if (text2[i] == '\u2028' || text2[i] == '\u2029' || text2[i] == '\u0085')
				{
					base.Output.Write(text);
				}
			}
			base.Output.WriteLine();
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00050208 File Offset: 0x0004F208
		protected override void GenerateMethodReturnStatement(CodeMethodReturnStatement e)
		{
			if (e.Expression != null)
			{
				base.Output.Write("Return ");
				base.GenerateExpression(e.Expression);
				base.Output.WriteLine("");
				return;
			}
			base.Output.WriteLine("Return");
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x0005025C File Offset: 0x0004F25C
		protected override void GenerateConditionStatement(CodeConditionStatement e)
		{
			base.Output.Write("If ");
			base.GenerateExpression(e.Condition);
			base.Output.WriteLine(" Then");
			base.Indent++;
			this.GenerateVBStatements(e.TrueStatements);
			base.Indent--;
			CodeStatementCollection falseStatements = e.FalseStatements;
			if (falseStatements.Count > 0)
			{
				base.Output.Write("Else");
				base.Output.WriteLine("");
				base.Indent++;
				this.GenerateVBStatements(e.FalseStatements);
				base.Indent--;
			}
			base.Output.WriteLine("End If");
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x00050328 File Offset: 0x0004F328
		protected override void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
		{
			base.Output.WriteLine("Try ");
			base.Indent++;
			this.GenerateVBStatements(e.TryStatements);
			base.Indent--;
			CodeCatchClauseCollection catchClauses = e.CatchClauses;
			if (catchClauses.Count > 0)
			{
				foreach (object obj in catchClauses)
				{
					CodeCatchClause codeCatchClause = (CodeCatchClause)obj;
					base.Output.Write("Catch ");
					this.OutputTypeNamePair(codeCatchClause.CatchExceptionType, codeCatchClause.LocalName);
					base.Output.WriteLine("");
					base.Indent++;
					this.GenerateVBStatements(codeCatchClause.Statements);
					base.Indent--;
				}
			}
			CodeStatementCollection finallyStatements = e.FinallyStatements;
			if (finallyStatements.Count > 0)
			{
				base.Output.WriteLine("Finally");
				base.Indent++;
				this.GenerateVBStatements(finallyStatements);
				base.Indent--;
			}
			base.Output.WriteLine("End Try");
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x00050447 File Offset: 0x0004F447
		protected override void GenerateAssignStatement(CodeAssignStatement e)
		{
			base.GenerateExpression(e.Left);
			base.Output.Write(" = ");
			base.GenerateExpression(e.Right);
			base.Output.WriteLine("");
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00050484 File Offset: 0x0004F484
		protected override void GenerateAttachEventStatement(CodeAttachEventStatement e)
		{
			base.Output.Write("AddHandler ");
			this.GenerateFormalEventReferenceExpression(e.Event);
			base.Output.Write(", ");
			base.GenerateExpression(e.Listener);
			base.Output.WriteLine("");
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x000504DC File Offset: 0x0004F4DC
		protected override void GenerateRemoveEventStatement(CodeRemoveEventStatement e)
		{
			base.Output.Write("RemoveHandler ");
			this.GenerateFormalEventReferenceExpression(e.Event);
			base.Output.Write(", ");
			base.GenerateExpression(e.Listener);
			base.Output.WriteLine("");
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x00050531 File Offset: 0x0004F531
		protected override void GenerateSnippetStatement(CodeSnippetStatement e)
		{
			base.Output.WriteLine(e.Value);
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00050544 File Offset: 0x0004F544
		protected override void GenerateGotoStatement(CodeGotoStatement e)
		{
			base.Output.Write("goto ");
			base.Output.WriteLine(e.Label);
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x00050568 File Offset: 0x0004F568
		protected override void GenerateLabeledStatement(CodeLabeledStatement e)
		{
			base.Indent--;
			base.Output.Write(e.Label);
			base.Output.WriteLine(":");
			base.Indent++;
			if (e.Statement != null)
			{
				base.GenerateStatement(e.Statement);
			}
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x000505C8 File Offset: 0x0004F5C8
		protected override void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
		{
			bool flag = true;
			base.Output.Write("Dim ");
			CodeTypeReference type = e.Type;
			if (type.ArrayRank == 1 && e.InitExpression != null)
			{
				CodeArrayCreateExpression codeArrayCreateExpression = e.InitExpression as CodeArrayCreateExpression;
				if (codeArrayCreateExpression != null && codeArrayCreateExpression.Initializers.Count == 0)
				{
					flag = false;
					this.OutputIdentifier(e.Name);
					base.Output.Write("(");
					if (codeArrayCreateExpression.SizeExpression != null)
					{
						base.Output.Write("(");
						base.GenerateExpression(codeArrayCreateExpression.SizeExpression);
						base.Output.Write(") - 1");
					}
					else
					{
						base.Output.Write(codeArrayCreateExpression.Size - 1);
					}
					base.Output.Write(")");
					if (type.ArrayElementType != null)
					{
						this.OutputArrayPostfix(type.ArrayElementType);
					}
					base.Output.Write(" As ");
					this.OutputType(type);
				}
				else
				{
					this.OutputTypeNamePair(e.Type, e.Name);
				}
			}
			else
			{
				this.OutputTypeNamePair(e.Type, e.Name);
			}
			if (flag && e.InitExpression != null)
			{
				base.Output.Write(" = ");
				base.GenerateExpression(e.InitExpression);
			}
			base.Output.WriteLine("");
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x00050728 File Offset: 0x0004F728
		protected override void GenerateLinePragmaStart(CodeLinePragma e)
		{
			base.Output.WriteLine("");
			base.Output.Write("#ExternalSource(\"");
			base.Output.Write(e.FileName);
			base.Output.Write("\",");
			base.Output.Write(e.LineNumber);
			base.Output.WriteLine(")");
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00050797 File Offset: 0x0004F797
		protected override void GenerateLinePragmaEnd(CodeLinePragma e)
		{
			base.Output.WriteLine("");
			base.Output.WriteLine("#End ExternalSource");
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x000507BC File Offset: 0x0004F7BC
		protected override void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c)
		{
			if (base.IsCurrentDelegate || base.IsCurrentEnum)
			{
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributes(e.CustomAttributes, false);
			}
			string name = e.Name;
			if (e.PrivateImplementationType != null)
			{
				string text = e.PrivateImplementationType.BaseType;
				text = text.Replace('.', '_');
				e.Name = text + "_" + e.Name;
			}
			this.OutputMemberAccessModifier(e.Attributes);
			base.Output.Write("Event ");
			this.OutputTypeNamePair(e.Type, e.Name);
			if (e.ImplementationTypes.Count > 0)
			{
				base.Output.Write(" Implements ");
				bool flag = true;
				using (IEnumerator enumerator = e.ImplementationTypes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						CodeTypeReference codeTypeReference = (CodeTypeReference)obj;
						if (flag)
						{
							flag = false;
						}
						else
						{
							base.Output.Write(" , ");
						}
						this.OutputType(codeTypeReference);
						base.Output.Write(".");
						this.OutputIdentifier(name);
					}
					goto IL_0166;
				}
			}
			if (e.PrivateImplementationType != null)
			{
				base.Output.Write(" Implements ");
				this.OutputType(e.PrivateImplementationType);
				base.Output.Write(".");
				this.OutputIdentifier(name);
			}
			IL_0166:
			base.Output.WriteLine("");
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00050950 File Offset: 0x0004F950
		protected override void GenerateField(CodeMemberField e)
		{
			if (base.IsCurrentDelegate || base.IsCurrentInterface)
			{
				return;
			}
			if (base.IsCurrentEnum)
			{
				if (e.CustomAttributes.Count > 0)
				{
					this.OutputAttributes(e.CustomAttributes, false);
				}
				this.OutputIdentifier(e.Name);
				if (e.InitExpression != null)
				{
					base.Output.Write(" = ");
					base.GenerateExpression(e.InitExpression);
				}
				base.Output.WriteLine("");
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributes(e.CustomAttributes, false);
			}
			this.OutputMemberAccessModifier(e.Attributes);
			this.OutputVTableModifier(e.Attributes);
			this.OutputFieldScopeModifier(e.Attributes);
			if (this.GetUserData(e, "WithEvents", false))
			{
				base.Output.Write("WithEvents ");
			}
			this.OutputTypeNamePair(e.Type, e.Name);
			if (e.InitExpression != null)
			{
				base.Output.Write(" = ");
				base.GenerateExpression(e.InitExpression);
			}
			base.Output.WriteLine("");
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00050A78 File Offset: 0x0004FA78
		private bool MethodIsOverloaded(CodeMemberMethod e, CodeTypeDeclaration c)
		{
			if ((e.Attributes & MemberAttributes.Overloaded) != (MemberAttributes)0)
			{
				return true;
			}
			IEnumerator enumerator = c.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeMemberMethod)
				{
					CodeMemberMethod codeMemberMethod = (CodeMemberMethod)enumerator.Current;
					if (!(enumerator.Current is CodeTypeConstructor) && !(enumerator.Current is CodeConstructor) && codeMemberMethod != e && codeMemberMethod.Name.Equals(e.Name, StringComparison.OrdinalIgnoreCase) && codeMemberMethod.PrivateImplementationType == null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00050B01 File Offset: 0x0004FB01
		protected override void GenerateSnippetMember(CodeSnippetTypeMember e)
		{
			base.Output.Write(e.Text);
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00050B14 File Offset: 0x0004FB14
		protected override void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c)
		{
			if (!base.IsCurrentClass && !base.IsCurrentStruct && !base.IsCurrentInterface)
			{
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributes(e.CustomAttributes, false);
			}
			string name = e.Name;
			if (e.PrivateImplementationType != null)
			{
				string text = e.PrivateImplementationType.BaseType;
				text = text.Replace('.', '_');
				e.Name = text + "_" + e.Name;
			}
			if (!base.IsCurrentInterface)
			{
				if (e.PrivateImplementationType == null)
				{
					this.OutputMemberAccessModifier(e.Attributes);
					if (this.MethodIsOverloaded(e, c))
					{
						base.Output.Write("Overloads ");
					}
				}
				this.OutputVTableModifier(e.Attributes);
				this.OutputMemberScopeModifier(e.Attributes);
			}
			else
			{
				this.OutputVTableModifier(e.Attributes);
			}
			bool flag = false;
			if (e.ReturnType.BaseType.Length == 0 || string.Compare(e.ReturnType.BaseType, typeof(void).FullName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				flag = true;
			}
			if (flag)
			{
				base.Output.Write("Sub ");
			}
			else
			{
				base.Output.Write("Function ");
			}
			this.OutputIdentifier(e.Name);
			this.OutputTypeParameters(e.TypeParameters);
			base.Output.Write("(");
			this.OutputParameters(e.Parameters);
			base.Output.Write(")");
			if (!flag)
			{
				base.Output.Write(" As ");
				if (e.ReturnTypeCustomAttributes.Count > 0)
				{
					this.OutputAttributes(e.ReturnTypeCustomAttributes, true);
				}
				this.OutputType(e.ReturnType);
				this.OutputArrayPostfix(e.ReturnType);
			}
			if (e.ImplementationTypes.Count > 0)
			{
				base.Output.Write(" Implements ");
				bool flag2 = true;
				using (IEnumerator enumerator = e.ImplementationTypes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						CodeTypeReference codeTypeReference = (CodeTypeReference)obj;
						if (flag2)
						{
							flag2 = false;
						}
						else
						{
							base.Output.Write(" , ");
						}
						this.OutputType(codeTypeReference);
						base.Output.Write(".");
						this.OutputIdentifier(name);
					}
					goto IL_0285;
				}
			}
			if (e.PrivateImplementationType != null)
			{
				base.Output.Write(" Implements ");
				this.OutputType(e.PrivateImplementationType);
				base.Output.Write(".");
				this.OutputIdentifier(name);
			}
			IL_0285:
			base.Output.WriteLine("");
			if (!base.IsCurrentInterface && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
			{
				base.Indent++;
				this.GenerateVBStatements(e.Statements);
				base.Indent--;
				if (flag)
				{
					base.Output.WriteLine("End Sub");
				}
				else
				{
					base.Output.WriteLine("End Function");
				}
			}
			e.Name = name;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x00050E30 File Offset: 0x0004FE30
		protected override void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c)
		{
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributes(e.CustomAttributes, false);
			}
			base.Output.WriteLine("Public Shared Sub Main()");
			base.Indent++;
			this.GenerateVBStatements(e.Statements);
			base.Indent--;
			base.Output.WriteLine("End Sub");
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00050EA0 File Offset: 0x0004FEA0
		private bool PropertyIsOverloaded(CodeMemberProperty e, CodeTypeDeclaration c)
		{
			if ((e.Attributes & MemberAttributes.Overloaded) != (MemberAttributes)0)
			{
				return true;
			}
			IEnumerator enumerator = c.Members.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current is CodeMemberProperty)
				{
					CodeMemberProperty codeMemberProperty = (CodeMemberProperty)enumerator.Current;
					if (codeMemberProperty != e && codeMemberProperty.Name.Equals(e.Name, StringComparison.OrdinalIgnoreCase) && codeMemberProperty.PrivateImplementationType == null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00050F10 File Offset: 0x0004FF10
		protected override void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c)
		{
			if (!base.IsCurrentClass && !base.IsCurrentStruct && !base.IsCurrentInterface)
			{
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributes(e.CustomAttributes, false);
			}
			string name = e.Name;
			if (e.PrivateImplementationType != null)
			{
				string text = e.PrivateImplementationType.BaseType;
				text = text.Replace('.', '_');
				e.Name = text + "_" + e.Name;
			}
			if (!base.IsCurrentInterface)
			{
				if (e.PrivateImplementationType == null)
				{
					this.OutputMemberAccessModifier(e.Attributes);
					if (this.PropertyIsOverloaded(e, c))
					{
						base.Output.Write("Overloads ");
					}
				}
				this.OutputVTableModifier(e.Attributes);
				this.OutputMemberScopeModifier(e.Attributes);
			}
			else
			{
				this.OutputVTableModifier(e.Attributes);
			}
			if (e.Parameters.Count > 0 && string.Compare(e.Name, "Item", StringComparison.OrdinalIgnoreCase) == 0)
			{
				base.Output.Write("Default ");
			}
			if (e.HasGet)
			{
				if (!e.HasSet)
				{
					base.Output.Write("ReadOnly ");
				}
			}
			else if (e.HasSet)
			{
				base.Output.Write("WriteOnly ");
			}
			base.Output.Write("Property ");
			this.OutputIdentifier(e.Name);
			base.Output.Write("(");
			if (e.Parameters.Count > 0)
			{
				this.OutputParameters(e.Parameters);
			}
			base.Output.Write(")");
			base.Output.Write(" As ");
			this.OutputType(e.Type);
			this.OutputArrayPostfix(e.Type);
			if (e.ImplementationTypes.Count > 0)
			{
				base.Output.Write(" Implements ");
				bool flag = true;
				using (IEnumerator enumerator = e.ImplementationTypes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						CodeTypeReference codeTypeReference = (CodeTypeReference)obj;
						if (flag)
						{
							flag = false;
						}
						else
						{
							base.Output.Write(" , ");
						}
						this.OutputType(codeTypeReference);
						base.Output.Write(".");
						this.OutputIdentifier(name);
					}
					goto IL_0285;
				}
			}
			if (e.PrivateImplementationType != null)
			{
				base.Output.Write(" Implements ");
				this.OutputType(e.PrivateImplementationType);
				base.Output.Write(".");
				this.OutputIdentifier(name);
			}
			IL_0285:
			base.Output.WriteLine("");
			if (!c.IsInterface)
			{
				base.Indent++;
				if (e.HasGet)
				{
					base.Output.WriteLine("Get");
					if (!base.IsCurrentInterface && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
					{
						base.Indent++;
						this.GenerateVBStatements(e.GetStatements);
						e.Name = name;
						base.Indent--;
						base.Output.WriteLine("End Get");
					}
				}
				if (e.HasSet)
				{
					base.Output.WriteLine("Set");
					if (!base.IsCurrentInterface && (e.Attributes & MemberAttributes.ScopeMask) != MemberAttributes.Abstract)
					{
						base.Indent++;
						this.GenerateVBStatements(e.SetStatements);
						base.Indent--;
						base.Output.WriteLine("End Set");
					}
				}
				base.Indent--;
				base.Output.WriteLine("End Property");
			}
			e.Name = name;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x000512D0 File Offset: 0x000502D0
		protected override void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e)
		{
			if (e.TargetObject != null)
			{
				base.GenerateExpression(e.TargetObject);
				base.Output.Write(".");
				base.Output.Write(e.PropertyName);
				return;
			}
			this.OutputIdentifier(e.PropertyName);
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00051320 File Offset: 0x00050320
		protected override void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c)
		{
			if (!base.IsCurrentClass && !base.IsCurrentStruct)
			{
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributes(e.CustomAttributes, false);
			}
			this.OutputMemberAccessModifier(e.Attributes);
			base.Output.Write("Sub New(");
			this.OutputParameters(e.Parameters);
			base.Output.WriteLine(")");
			base.Indent++;
			CodeExpressionCollection baseConstructorArgs = e.BaseConstructorArgs;
			CodeExpressionCollection chainedConstructorArgs = e.ChainedConstructorArgs;
			if (chainedConstructorArgs.Count > 0)
			{
				base.Output.Write("Me.New(");
				this.OutputExpressionList(chainedConstructorArgs);
				base.Output.Write(")");
				base.Output.WriteLine("");
			}
			else if (baseConstructorArgs.Count > 0)
			{
				base.Output.Write("MyBase.New(");
				this.OutputExpressionList(baseConstructorArgs);
				base.Output.Write(")");
				base.Output.WriteLine("");
			}
			else if (base.IsCurrentClass)
			{
				base.Output.WriteLine("MyBase.New");
			}
			this.GenerateVBStatements(e.Statements);
			base.Indent--;
			base.Output.WriteLine("End Sub");
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x00051474 File Offset: 0x00050474
		protected override void GenerateTypeConstructor(CodeTypeConstructor e)
		{
			if (!base.IsCurrentClass && !base.IsCurrentStruct)
			{
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributes(e.CustomAttributes, false);
			}
			base.Output.WriteLine("Shared Sub New()");
			base.Indent++;
			this.GenerateVBStatements(e.Statements);
			base.Indent--;
			base.Output.WriteLine("End Sub");
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x000514F5 File Offset: 0x000504F5
		protected override void GenerateTypeOfExpression(CodeTypeOfExpression e)
		{
			base.Output.Write("GetType(");
			base.Output.Write(this.GetTypeOutput(e.Type));
			base.Output.Write(")");
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00051530 File Offset: 0x00050530
		protected override void GenerateTypeStart(CodeTypeDeclaration e)
		{
			if (base.IsCurrentDelegate)
			{
				if (e.CustomAttributes.Count > 0)
				{
					this.OutputAttributes(e.CustomAttributes, false);
				}
				switch (e.TypeAttributes & TypeAttributes.VisibilityMask)
				{
				case TypeAttributes.Public:
					base.Output.Write("Public ");
					break;
				}
				CodeTypeDelegate codeTypeDelegate = (CodeTypeDelegate)e;
				if (codeTypeDelegate.ReturnType.BaseType.Length > 0 && string.Compare(codeTypeDelegate.ReturnType.BaseType, "System.Void", StringComparison.OrdinalIgnoreCase) != 0)
				{
					base.Output.Write("Delegate Function ");
				}
				else
				{
					base.Output.Write("Delegate Sub ");
				}
				this.OutputIdentifier(e.Name);
				base.Output.Write("(");
				this.OutputParameters(codeTypeDelegate.Parameters);
				base.Output.Write(")");
				if (codeTypeDelegate.ReturnType.BaseType.Length > 0 && string.Compare(codeTypeDelegate.ReturnType.BaseType, "System.Void", StringComparison.OrdinalIgnoreCase) != 0)
				{
					base.Output.Write(" As ");
					this.OutputType(codeTypeDelegate.ReturnType);
					this.OutputArrayPostfix(codeTypeDelegate.ReturnType);
				}
				base.Output.WriteLine("");
				return;
			}
			if (e.IsEnum)
			{
				if (e.CustomAttributes.Count > 0)
				{
					this.OutputAttributes(e.CustomAttributes, false);
				}
				this.OutputTypeAttributes(e);
				this.OutputIdentifier(e.Name);
				if (e.BaseTypes.Count > 0)
				{
					base.Output.Write(" As ");
					this.OutputType(e.BaseTypes[0]);
				}
				base.Output.WriteLine("");
				base.Indent++;
				return;
			}
			if (e.CustomAttributes.Count > 0)
			{
				this.OutputAttributes(e.CustomAttributes, false);
			}
			this.OutputTypeAttributes(e);
			this.OutputIdentifier(e.Name);
			this.OutputTypeParameters(e.TypeParameters);
			bool flag = false;
			bool flag2 = false;
			if (e.IsStruct)
			{
				flag = true;
			}
			if (e.IsInterface)
			{
				flag2 = true;
			}
			base.Indent++;
			foreach (object obj in e.BaseTypes)
			{
				CodeTypeReference codeTypeReference = (CodeTypeReference)obj;
				if (!flag && !codeTypeReference.IsInterface)
				{
					base.Output.WriteLine("");
					base.Output.Write("Inherits ");
					flag = true;
				}
				else if (!flag2)
				{
					base.Output.WriteLine("");
					base.Output.Write("Implements ");
					flag2 = true;
				}
				else
				{
					base.Output.Write(", ");
				}
				this.OutputType(codeTypeReference);
			}
			base.Output.WriteLine("");
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00051830 File Offset: 0x00050830
		private void OutputTypeParameters(CodeTypeParameterCollection typeParameters)
		{
			if (typeParameters.Count == 0)
			{
				return;
			}
			base.Output.Write("(Of ");
			bool flag = true;
			for (int i = 0; i < typeParameters.Count; i++)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					base.Output.Write(", ");
				}
				base.Output.Write(typeParameters[i].Name);
				this.OutputTypeParameterConstraints(typeParameters[i]);
			}
			base.Output.Write(')');
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x000518B4 File Offset: 0x000508B4
		private void OutputTypeParameterConstraints(CodeTypeParameter typeParameter)
		{
			CodeTypeReferenceCollection constraints = typeParameter.Constraints;
			int num = constraints.Count;
			if (typeParameter.HasConstructorConstraint)
			{
				num++;
			}
			if (num == 0)
			{
				return;
			}
			base.Output.Write(" As ");
			if (num > 1)
			{
				base.Output.Write(" {");
			}
			bool flag = true;
			foreach (object obj in constraints)
			{
				CodeTypeReference codeTypeReference = (CodeTypeReference)obj;
				if (flag)
				{
					flag = false;
				}
				else
				{
					base.Output.Write(", ");
				}
				base.Output.Write(this.GetTypeOutput(codeTypeReference));
			}
			if (typeParameter.HasConstructorConstraint)
			{
				if (!flag)
				{
					base.Output.Write(", ");
				}
				base.Output.Write("New");
			}
			if (num > 1)
			{
				base.Output.Write('}');
			}
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x000519B4 File Offset: 0x000509B4
		protected override void GenerateTypeEnd(CodeTypeDeclaration e)
		{
			if (!base.IsCurrentDelegate)
			{
				base.Indent--;
				string text;
				if (e.IsEnum)
				{
					text = "End Enum";
				}
				else if (e.IsInterface)
				{
					text = "End Interface";
				}
				else if (e.IsStruct)
				{
					text = "End Structure";
				}
				else if (this.IsCurrentModule)
				{
					text = "End Module";
				}
				else
				{
					text = "End Class";
				}
				base.Output.WriteLine(text);
			}
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00051A2C File Offset: 0x00050A2C
		protected override void GenerateNamespace(CodeNamespace e)
		{
			if (this.GetUserData(e, "GenerateImports", true))
			{
				base.GenerateNamespaceImports(e);
			}
			base.Output.WriteLine();
			this.GenerateCommentStatements(e.Comments);
			this.GenerateNamespaceStart(e);
			base.GenerateTypes(e);
			this.GenerateNamespaceEnd(e);
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00051A7C File Offset: 0x00050A7C
		protected bool AllowLateBound(CodeCompileUnit e)
		{
			object obj = e.UserData["AllowLateBound"];
			return obj == null || !(obj is bool) || (bool)obj;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x00051AB0 File Offset: 0x00050AB0
		protected bool RequireVariableDeclaration(CodeCompileUnit e)
		{
			object obj = e.UserData["RequireVariableDeclaration"];
			return obj == null || !(obj is bool) || (bool)obj;
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00051AE4 File Offset: 0x00050AE4
		private bool GetUserData(CodeObject e, string property, bool defaultValue)
		{
			object obj = e.UserData[property];
			if (obj != null && obj is bool)
			{
				return (bool)obj;
			}
			return defaultValue;
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x00051B14 File Offset: 0x00050B14
		protected override void GenerateCompileUnitStart(CodeCompileUnit e)
		{
			base.GenerateCompileUnitStart(e);
			base.Output.WriteLine("'------------------------------------------------------------------------------");
			base.Output.Write("' <");
			base.Output.WriteLine(SR.GetString("AutoGen_Comment_Line1"));
			base.Output.Write("'     ");
			base.Output.WriteLine(SR.GetString("AutoGen_Comment_Line2"));
			base.Output.Write("'     ");
			base.Output.Write(SR.GetString("AutoGen_Comment_Line3"));
			base.Output.WriteLine(Environment.Version.ToString());
			base.Output.WriteLine("'");
			base.Output.Write("'     ");
			base.Output.WriteLine(SR.GetString("AutoGen_Comment_Line4"));
			base.Output.Write("'     ");
			base.Output.WriteLine(SR.GetString("AutoGen_Comment_Line5"));
			base.Output.Write("' </");
			base.Output.WriteLine(SR.GetString("AutoGen_Comment_Line1"));
			base.Output.WriteLine("'------------------------------------------------------------------------------");
			base.Output.WriteLine("");
			if (this.AllowLateBound(e))
			{
				base.Output.WriteLine("Option Strict Off");
			}
			else
			{
				base.Output.WriteLine("Option Strict On");
			}
			if (!this.RequireVariableDeclaration(e))
			{
				base.Output.WriteLine("Option Explicit Off");
			}
			else
			{
				base.Output.WriteLine("Option Explicit On");
			}
			base.Output.WriteLine();
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00051CBC File Offset: 0x00050CBC
		protected override void GenerateCompileUnit(CodeCompileUnit e)
		{
			this.GenerateCompileUnitStart(e);
			SortedList sortedList = new SortedList(StringComparer.OrdinalIgnoreCase);
			foreach (object obj in e.Namespaces)
			{
				CodeNamespace codeNamespace = (CodeNamespace)obj;
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
			foreach (object obj3 in sortedList.Keys)
			{
				string text = (string)obj3;
				base.Output.Write("Imports ");
				this.OutputIdentifier(text);
				base.Output.WriteLine("");
			}
			if (e.AssemblyCustomAttributes.Count > 0)
			{
				this.OutputAttributes(e.AssemblyCustomAttributes, false, "Assembly: ", true);
			}
			base.GenerateNamespaces(e);
			this.GenerateCompileUnitEnd(e);
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00051E48 File Offset: 0x00050E48
		protected override void GenerateDirectives(CodeDirectiveCollection directives)
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

		// Token: 0x060017EC RID: 6124 RVA: 0x00051E98 File Offset: 0x00050E98
		private void GenerateChecksumPragma(CodeChecksumPragma checksumPragma)
		{
			base.Output.Write("#ExternalChecksum(\"");
			base.Output.Write(checksumPragma.FileName);
			base.Output.Write("\",\"");
			base.Output.Write(checksumPragma.ChecksumAlgorithmId.ToString("B", CultureInfo.InvariantCulture));
			base.Output.Write("\",\"");
			if (checksumPragma.ChecksumData != null)
			{
				foreach (byte b in checksumPragma.ChecksumData)
				{
					base.Output.Write(b.ToString("X2", CultureInfo.InvariantCulture));
				}
			}
			base.Output.WriteLine("\")");
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x00051F58 File Offset: 0x00050F58
		private void GenerateCodeRegionDirective(CodeRegionDirective regionDirective)
		{
			if (this.IsGeneratingStatements())
			{
				return;
			}
			if (regionDirective.RegionMode == CodeRegionMode.Start)
			{
				base.Output.Write("#Region \"");
				base.Output.Write(regionDirective.RegionText);
				base.Output.WriteLine("\"");
				return;
			}
			if (regionDirective.RegionMode == CodeRegionMode.End)
			{
				base.Output.WriteLine("#End Region");
			}
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00051FC4 File Offset: 0x00050FC4
		protected override void GenerateNamespaceStart(CodeNamespace e)
		{
			if (e.Name != null && e.Name.Length > 0)
			{
				base.Output.Write("Namespace ");
				string[] array = e.Name.Split(new char[] { '.' });
				this.OutputIdentifier(array[0]);
				for (int i = 1; i < array.Length; i++)
				{
					base.Output.Write(".");
					this.OutputIdentifier(array[i]);
				}
				base.Output.WriteLine();
				base.Indent++;
			}
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0005205C File Offset: 0x0005105C
		protected override void GenerateNamespaceEnd(CodeNamespace e)
		{
			if (e.Name != null && e.Name.Length > 0)
			{
				base.Indent--;
				base.Output.WriteLine("End Namespace");
			}
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00052092 File Offset: 0x00051092
		protected override void GenerateNamespaceImport(CodeNamespaceImport e)
		{
			base.Output.Write("Imports ");
			this.OutputIdentifier(e.Namespace);
			base.Output.WriteLine("");
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x000520C0 File Offset: 0x000510C0
		protected override void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes)
		{
			base.Output.Write("<");
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x000520D2 File Offset: 0x000510D2
		protected override void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes)
		{
			base.Output.Write(">");
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x000520E4 File Offset: 0x000510E4
		public static bool IsKeyword(string value)
		{
			return FixedStringLookup.Contains(VBCodeGenerator.keywords, value, true);
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x000520F2 File Offset: 0x000510F2
		protected override bool Supports(GeneratorSupport support)
		{
			return (support & (GeneratorSupport.ArraysOfArrays | GeneratorSupport.EntryPointMethod | GeneratorSupport.GotoStatements | GeneratorSupport.MultidimensionalArrays | GeneratorSupport.StaticConstructors | GeneratorSupport.TryCatchStatements | GeneratorSupport.ReturnTypeAttributes | GeneratorSupport.DeclareValueTypes | GeneratorSupport.DeclareEnums | GeneratorSupport.DeclareDelegates | GeneratorSupport.DeclareInterfaces | GeneratorSupport.DeclareEvents | GeneratorSupport.AssemblyAttributes | GeneratorSupport.ParameterAttributes | GeneratorSupport.ReferenceParameters | GeneratorSupport.ChainedConstructorArguments | GeneratorSupport.NestedTypes | GeneratorSupport.MultipleInterfaceMembers | GeneratorSupport.PublicStaticMembers | GeneratorSupport.ComplexExpressions | GeneratorSupport.Win32Resources | GeneratorSupport.Resources | GeneratorSupport.PartialTypes | GeneratorSupport.GenericTypeReference | GeneratorSupport.GenericTypeDeclaration | GeneratorSupport.DeclareIndexerProperties)) == support;
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x00052100 File Offset: 0x00051100
		protected override bool IsValidIdentifier(string value)
		{
			if (value == null || value.Length == 0)
			{
				return false;
			}
			if (value.Length > 1023)
			{
				return false;
			}
			if (value[0] != '[' || value[value.Length - 1] != ']')
			{
				if (VBCodeGenerator.IsKeyword(value))
				{
					return false;
				}
			}
			else
			{
				value = value.Substring(1, value.Length - 2);
			}
			return (value.Length != 1 || value[0] != '_') && CodeGenerator.IsValidLanguageIndependentIdentifier(value);
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x0005217D File Offset: 0x0005117D
		protected override string CreateValidIdentifier(string name)
		{
			if (VBCodeGenerator.IsKeyword(name))
			{
				return "_" + name;
			}
			return name;
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00052194 File Offset: 0x00051194
		protected override string CreateEscapedIdentifier(string name)
		{
			if (VBCodeGenerator.IsKeyword(name))
			{
				return "[" + name + "]";
			}
			return name;
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x000521B0 File Offset: 0x000511B0
		private string GetBaseTypeOutput(CodeTypeReference typeRef)
		{
			string baseType = typeRef.BaseType;
			if (baseType.Length == 0)
			{
				return "Void";
			}
			if (string.Compare(baseType, "System.Byte", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Byte";
			}
			if (string.Compare(baseType, "System.SByte", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "SByte";
			}
			if (string.Compare(baseType, "System.Int16", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Short";
			}
			if (string.Compare(baseType, "System.Int32", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Integer";
			}
			if (string.Compare(baseType, "System.Int64", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Long";
			}
			if (string.Compare(baseType, "System.UInt16", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "UShort";
			}
			if (string.Compare(baseType, "System.UInt32", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "UInteger";
			}
			if (string.Compare(baseType, "System.UInt64", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "ULong";
			}
			if (string.Compare(baseType, "System.String", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "String";
			}
			if (string.Compare(baseType, "System.DateTime", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Date";
			}
			if (string.Compare(baseType, "System.Decimal", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Decimal";
			}
			if (string.Compare(baseType, "System.Single", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Single";
			}
			if (string.Compare(baseType, "System.Double", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Double";
			}
			if (string.Compare(baseType, "System.Boolean", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Boolean";
			}
			if (string.Compare(baseType, "System.Char", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Char";
			}
			if (string.Compare(baseType, "System.Object", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Object";
			}
			StringBuilder stringBuilder = new StringBuilder(baseType.Length + 10);
			if (typeRef.Options == CodeTypeReferenceOptions.GlobalReference)
			{
				stringBuilder.Append("Global.");
			}
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < baseType.Length; i++)
			{
				char c = baseType[i];
				if (c != '+' && c != '.')
				{
					if (c == '`')
					{
						stringBuilder.Append(this.CreateEscapedIdentifier(baseType.Substring(num, i - num)));
						i++;
						int num3 = 0;
						while (i < baseType.Length && baseType[i] >= '0' && baseType[i] <= '9')
						{
							num3 = num3 * 10 + (int)(baseType[i] - '0');
							i++;
						}
						this.GetTypeArgumentsOutput(typeRef.TypeArguments, num2, num3, stringBuilder);
						num2 += num3;
						if (i < baseType.Length && (baseType[i] == '+' || baseType[i] == '.'))
						{
							stringBuilder.Append('.');
							i++;
						}
						num = i;
					}
				}
				else
				{
					stringBuilder.Append(this.CreateEscapedIdentifier(baseType.Substring(num, i - num)));
					stringBuilder.Append('.');
					i++;
					num = i;
				}
			}
			if (num < baseType.Length)
			{
				stringBuilder.Append(this.CreateEscapedIdentifier(baseType.Substring(num)));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00052470 File Offset: 0x00051470
		private string GetTypeOutputWithoutArrayPostFix(CodeTypeReference typeRef)
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (typeRef.ArrayElementType != null)
			{
				typeRef = typeRef.ArrayElementType;
			}
			stringBuilder.Append(this.GetBaseTypeOutput(typeRef));
			return stringBuilder.ToString();
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x000524AC File Offset: 0x000514AC
		private string GetTypeArgumentsOutput(CodeTypeReferenceCollection typeArguments)
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			this.GetTypeArgumentsOutput(typeArguments, 0, typeArguments.Count, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x000524DC File Offset: 0x000514DC
		private void GetTypeArgumentsOutput(CodeTypeReferenceCollection typeArguments, int start, int length, StringBuilder sb)
		{
			sb.Append("(Of ");
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
			sb.Append(')');
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00052544 File Offset: 0x00051544
		protected override string GetTypeOutput(CodeTypeReference typeRef)
		{
			string text = string.Empty;
			text += this.GetTypeOutputWithoutArrayPostFix(typeRef);
			if (typeRef.ArrayRank > 0)
			{
				text += this.GetArrayPostfix(typeRef);
			}
			return text;
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x0005257D File Offset: 0x0005157D
		protected override void ContinueOnNewLine(string st)
		{
			base.Output.Write(st);
			base.Output.WriteLine(" _");
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x0005259B File Offset: 0x0005159B
		private bool IsGeneratingStatements()
		{
			return this.statementDepth > 0;
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x000525A8 File Offset: 0x000515A8
		private void GenerateVBStatements(CodeStatementCollection stms)
		{
			this.statementDepth++;
			try
			{
				base.GenerateStatements(stms);
			}
			finally
			{
				this.statementDepth--;
			}
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x000525EC File Offset: 0x000515EC
		protected override CompilerResults FromFileBatch(CompilerParameters options, string[] fileNames)
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
			if (options.CompilerOptions != null && options.CompilerOptions.IndexOf("/debug:pdbonly", StringComparison.OrdinalIgnoreCase) != -1)
			{
				compilerResults.TempFiles.AddExtension(text3, true);
			}
			else
			{
				compilerResults.TempFiles.AddExtension(text3);
			}
			string text4 = this.CmdArgsFromParameters(options) + " " + CodeCompiler.JoinStringArray(fileNames, " ");
			string responseFileCmdArgs = this.GetResponseFileCmdArgs(options, text4);
			string text5 = null;
			if (responseFileCmdArgs != null)
			{
				text5 = text4;
				text4 = responseFileCmdArgs;
			}
			base.Compile(options, RedistVersionInfo.GetCompilerPath(this.provOptions, this.CompilerName), this.CompilerName, text4, ref text, ref num, text5);
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

		// Token: 0x04001615 RID: 5653
		private const int MaxLineLength = 80;

		// Token: 0x04001616 RID: 5654
		private const GeneratorSupport LanguageSupport = GeneratorSupport.ArraysOfArrays | GeneratorSupport.EntryPointMethod | GeneratorSupport.GotoStatements | GeneratorSupport.MultidimensionalArrays | GeneratorSupport.StaticConstructors | GeneratorSupport.TryCatchStatements | GeneratorSupport.ReturnTypeAttributes | GeneratorSupport.DeclareValueTypes | GeneratorSupport.DeclareEnums | GeneratorSupport.DeclareDelegates | GeneratorSupport.DeclareInterfaces | GeneratorSupport.DeclareEvents | GeneratorSupport.AssemblyAttributes | GeneratorSupport.ParameterAttributes | GeneratorSupport.ReferenceParameters | GeneratorSupport.ChainedConstructorArguments | GeneratorSupport.NestedTypes | GeneratorSupport.MultipleInterfaceMembers | GeneratorSupport.PublicStaticMembers | GeneratorSupport.ComplexExpressions | GeneratorSupport.Win32Resources | GeneratorSupport.Resources | GeneratorSupport.PartialTypes | GeneratorSupport.GenericTypeReference | GeneratorSupport.GenericTypeDeclaration | GeneratorSupport.DeclareIndexerProperties;

		// Token: 0x04001617 RID: 5655
		private static Regex outputReg;

		// Token: 0x04001618 RID: 5656
		private int statementDepth;

		// Token: 0x04001619 RID: 5657
		private IDictionary<string, string> provOptions;

		// Token: 0x0400161A RID: 5658
		private static readonly string[][] keywords = new string[][]
		{
			null,
			new string[] { "as", "do", "if", "in", "is", "me", "of", "on", "or", "to" },
			new string[]
			{
				"and", "dim", "end", "for", "get", "let", "lib", "mod", "new", "not",
				"rem", "set", "sub", "try", "xor"
			},
			new string[]
			{
				"ansi", "auto", "byte", "call", "case", "cdbl", "cdec", "char", "cint", "clng",
				"cobj", "csng", "cstr", "date", "each", "else", "enum", "exit", "goto", "like",
				"long", "loop", "next", "step", "stop", "then", "true", "wend", "when", "with"
			},
			new string[]
			{
				"alias", "byref", "byval", "catch", "cbool", "cbyte", "cchar", "cdate", "class", "const",
				"ctype", "cuint", "culng", "endif", "erase", "error", "event", "false", "gosub", "isnot",
				"redim", "sbyte", "short", "throw", "ulong", "until", "using", "while"
			},
			new string[]
			{
				"csbyte", "cshort", "double", "elseif", "friend", "global", "module", "mybase", "object", "option",
				"orelse", "public", "resume", "return", "select", "shared", "single", "static", "string", "typeof",
				"ushort"
			},
			new string[]
			{
				"andalso", "boolean", "cushort", "decimal", "declare", "default", "finally", "gettype", "handles", "imports",
				"integer", "myclass", "nothing", "partial", "private", "shadows", "trycast", "unicode", "variant"
			},
			new string[]
			{
				"assembly", "continue", "delegate", "function", "inherits", "operator", "optional", "preserve", "property", "readonly",
				"synclock", "uinteger", "widening"
			},
			new string[] { "addressof", "interface", "namespace", "narrowing", "overloads", "overrides", "protected", "structure", "writeonly" },
			new string[] { "addhandler", "directcast", "implements", "paramarray", "raiseevent", "withevents" },
			new string[] { "mustinherit", "overridable" },
			new string[] { "mustoverride" },
			new string[] { "removehandler" },
			new string[] { "class_finalize", "notinheritable", "notoverridable" },
			null,
			new string[] { "class_initialize" }
		};
	}
}
