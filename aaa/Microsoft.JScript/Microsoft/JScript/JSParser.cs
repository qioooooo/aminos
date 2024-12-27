using System;
using System.Collections;
using System.Reflection;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace Microsoft.JScript
{
	// Token: 0x020000B6 RID: 182
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class JSParser
	{
		// Token: 0x06000837 RID: 2103 RVA: 0x00039088 File Offset: 0x00038088
		public JSParser(Context context)
		{
			this.sourceContext = context;
			this.currentToken = context.Clone();
			this.scanner = new JSScanner(this.currentToken);
			this.noSkipTokenSet = new NoSkipTokenSet();
			this.errorToken = null;
			this.program = null;
			this.blockType = new ArrayList(16);
			this.labelTable = new SimpleHashtable(16U);
			this.finallyEscaped = 0;
			this.Globals = context.document.engine.Globals;
			this.Severity = 5;
			this.demandFullTrustOnFunctionCreation = false;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00039120 File Offset: 0x00038120
		public ScriptBlock Parse()
		{
			Block block = this.ParseStatements(false);
			return new ScriptBlock(this.sourceContext.Clone(), block);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00039146 File Offset: 0x00038146
		public Block ParseEvalBody()
		{
			this.demandFullTrustOnFunctionCreation = true;
			return this.ParseStatements(true);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00039158 File Offset: 0x00038158
		internal ScriptBlock ParseExpressionItem()
		{
			int num = this.Globals.ScopeStack.Size();
			try
			{
				Block block = new Block(this.sourceContext.Clone());
				this.GetNextToken();
				block.Append(new Expression(this.sourceContext.Clone(), this.ParseExpression()));
				return new ScriptBlock(this.sourceContext.Clone(), block);
			}
			catch (EndOfFile)
			{
			}
			catch (ScannerException ex)
			{
				this.EOFError(ex.m_errorId);
			}
			catch (StackOverflowException)
			{
				this.Globals.ScopeStack.TrimToSize(num);
				this.ReportError(JSError.OutOfStack, true);
			}
			return null;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00039218 File Offset: 0x00038218
		private Block ParseStatements(bool insideEval)
		{
			int num = this.Globals.ScopeStack.Size();
			this.program = new Block(this.sourceContext.Clone());
			this.blockType.Add(JSParser.BlockType.Block);
			this.errorToken = null;
			try
			{
				this.GetNextToken();
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_TopLevelNoSkipTokenSet);
				try
				{
					while (this.currentToken.token != JSToken.EndOfFile)
					{
						AST ast = null;
						try
						{
							if (this.currentToken.token == JSToken.Package && !insideEval)
							{
								ast = this.ParsePackage(this.currentToken.Clone());
							}
							else
							{
								if (this.currentToken.token == JSToken.Import && !insideEval)
								{
									this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
									try
									{
										try
										{
											ast = this.ParseImportStatement();
										}
										catch (RecoveryTokenException ex)
										{
											if (this.IndexOfToken(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet, ex) == -1)
											{
												throw ex;
											}
											ast = ex._partiallyComputedNode;
											if (ex._token == JSToken.Semicolon)
											{
												this.GetNextToken();
											}
										}
										goto IL_0104;
									}
									finally
									{
										this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
									}
								}
								ast = this.ParseStatement();
							}
							IL_0104:;
						}
						catch (RecoveryTokenException ex2)
						{
							if (this.TokenInList(NoSkipTokenSet.s_TopLevelNoSkipTokenSet, ex2) || this.TokenInList(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, ex2))
							{
								ast = ex2._partiallyComputedNode;
							}
							else
							{
								this.errorToken = null;
								do
								{
									this.GetNextToken();
								}
								while (this.currentToken.token != JSToken.EndOfFile && !this.TokenInList(NoSkipTokenSet.s_TopLevelNoSkipTokenSet, this.currentToken.token) && !this.TokenInList(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, this.currentToken.token));
							}
						}
						if (ast != null)
						{
							this.program.Append(ast);
						}
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_TopLevelNoSkipTokenSet);
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
				}
			}
			catch (EndOfFile)
			{
			}
			catch (ScannerException ex3)
			{
				this.EOFError(ex3.m_errorId);
			}
			catch (StackOverflowException)
			{
				this.Globals.ScopeStack.TrimToSize(num);
				this.ReportError(JSError.OutOfStack, true);
			}
			return this.program;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000394C4 File Offset: 0x000384C4
		private AST ParseStatement()
		{
			AST ast = null;
			JSToken token = this.currentToken.token;
			if (token <= JSToken.Else)
			{
				switch (token)
				{
				case JSToken.EndOfFile:
					this.EOFError(JSError.ErrEOF);
					throw new EndOfFile();
				case JSToken.If:
					return this.ParseIfStatement();
				case JSToken.For:
					return this.ParseForStatement();
				case JSToken.Do:
					return this.ParseDoStatement();
				case JSToken.While:
					return this.ParseWhileStatement();
				case JSToken.Continue:
					ast = this.ParseContinueStatement();
					if (ast == null)
					{
						return new Block(this.CurrentPositionContext());
					}
					return ast;
				case JSToken.Break:
					ast = this.ParseBreakStatement();
					if (ast == null)
					{
						return new Block(this.CurrentPositionContext());
					}
					return ast;
				case JSToken.Return:
					ast = this.ParseReturnStatement();
					if (ast == null)
					{
						return new Block(this.CurrentPositionContext());
					}
					return ast;
				case JSToken.Import:
					this.ReportError(JSError.InvalidImport, true);
					ast = new Block(this.currentToken.Clone());
					try
					{
						this.ParseImportStatement();
						goto IL_04B0;
					}
					catch (RecoveryTokenException)
					{
						goto IL_04B0;
					}
					goto IL_0315;
				case JSToken.With:
					return this.ParseWithStatement();
				case JSToken.Switch:
					return this.ParseSwitchStatement();
				case JSToken.Throw:
					ast = this.ParseThrowStatement();
					if (ast == null)
					{
						return new Block(this.CurrentPositionContext());
					}
					goto IL_04B0;
				case JSToken.Try:
					return this.ParseTryStatement();
				case JSToken.Package:
				{
					Context context = this.currentToken.Clone();
					ast = this.ParsePackage(context);
					if (ast is Package)
					{
						this.ReportError(JSError.PackageInWrongContext, context, true);
						ast = new Block(context);
						goto IL_04B0;
					}
					goto IL_04B0;
				}
				case JSToken.Internal:
				case JSToken.Abstract:
				case JSToken.Public:
				case JSToken.Static:
				case JSToken.Private:
				case JSToken.Protected:
				case JSToken.Final:
				{
					bool flag;
					ast = this.ParseAttributes(null, false, false, out flag);
					if (!flag)
					{
						ast = this.ParseExpression(ast, false, true, JSToken.None);
						ast = new Expression(ast.context.Clone(), ast);
						goto IL_04B0;
					}
					return ast;
				}
				case JSToken.Event:
				case JSToken.Null:
				case JSToken.True:
				case JSToken.False:
					goto IL_0315;
				case JSToken.Var:
				case JSToken.Const:
					return this.ParseVariableStatement(FieldAttributes.PrivateScope, null, this.currentToken.token);
				case JSToken.Class:
					goto IL_028C;
				case JSToken.Function:
					return this.ParseFunction(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), false, false, false, false, null);
				case JSToken.LeftCurly:
					return this.ParseBlock();
				case JSToken.Semicolon:
					ast = new Block(this.currentToken.Clone());
					this.GetNextToken();
					return ast;
				case JSToken.This:
					break;
				default:
					switch (token)
					{
					case JSToken.Debugger:
						ast = new DebugBreak(this.currentToken.Clone());
						this.GetNextToken();
						goto IL_04B0;
					case JSToken.Default:
						goto IL_0315;
					case JSToken.Else:
						this.ReportError(JSError.InvalidElse);
						this.SkipTokensAndThrow();
						goto IL_04B0;
					default:
						goto IL_0315;
					}
					break;
				}
			}
			else
			{
				if (token == JSToken.Interface)
				{
					goto IL_028C;
				}
				switch (token)
				{
				case JSToken.Super:
					break;
				case JSToken.RightParen:
					goto IL_0315;
				case JSToken.RightCurly:
					this.ReportError(JSError.SyntaxError);
					this.SkipTokensAndThrow();
					goto IL_04B0;
				default:
					if (token != JSToken.Enum)
					{
						goto IL_0315;
					}
					return this.ParseEnum(FieldAttributes.PrivateScope, this.currentToken.Clone(), null);
				}
			}
			Context context2 = this.currentToken.Clone();
			if (JSToken.LeftParen == this.scanner.PeekToken())
			{
				ast = this.ParseConstructorCall(context2);
				goto IL_04B0;
			}
			goto IL_0315;
			IL_028C:
			return this.ParseClass(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), false, false, null);
			IL_0315:
			this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
			bool flag2 = false;
			try
			{
				bool flag3 = true;
				bool flag4;
				ast = this.ParseUnaryExpression(out flag4, ref flag3, false);
				if (flag3)
				{
					if (ast is Lookup && JSToken.Colon == this.currentToken.token)
					{
						string text = ast.ToString();
						if (this.labelTable[text] != null)
						{
							this.ReportError(JSError.BadLabel, ast.context.Clone(), true);
							this.GetNextToken();
							return new Block(this.CurrentPositionContext());
						}
						this.GetNextToken();
						this.labelTable[text] = this.blockType.Count;
						if (this.currentToken.token != JSToken.EndOfFile)
						{
							ast = this.ParseStatement();
						}
						else
						{
							ast = new Block(this.CurrentPositionContext());
						}
						this.labelTable.Remove(text);
						return ast;
					}
					else if (JSToken.Semicolon != this.currentToken.token && !this.scanner.GotEndOfLine())
					{
						bool flag5;
						ast = this.ParseAttributes(ast, false, false, out flag5);
						if (flag5)
						{
							return ast;
						}
					}
				}
				ast = this.ParseExpression(ast, false, flag4, JSToken.None);
				ast = new Expression(ast.context.Clone(), ast);
			}
			catch (RecoveryTokenException ex)
			{
				if (ex._partiallyComputedNode != null)
				{
					ast = ex._partiallyComputedNode;
				}
				if (ast == null)
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
					flag2 = true;
					this.SkipTokensAndThrow();
				}
				if (this.IndexOfToken(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet, ex) == -1)
				{
					ex._partiallyComputedNode = ast;
					throw ex;
				}
			}
			finally
			{
				if (!flag2)
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
				}
			}
			IL_04B0:
			if (JSToken.Semicolon == this.currentToken.token)
			{
				ast.context.UpdateWith(this.currentToken);
				this.GetNextToken();
			}
			else if (!this.scanner.GotEndOfLine() && JSToken.RightCurly != this.currentToken.token && this.currentToken.token != JSToken.EndOfFile)
			{
				this.ReportError(JSError.NoSemicolon, true);
			}
			return ast;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00039A30 File Offset: 0x00038A30
		private AST ParseAttributes(AST statement, bool unambiguousContext, bool isInsideClass, out bool parsedOK)
		{
			AST ast = statement;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			AST ast2 = null;
			ArrayList arrayList4 = new ArrayList();
			Context context = null;
			Context context2 = null;
			Context context3 = null;
			int num = 0;
			if (unambiguousContext)
			{
				num = 2;
			}
			FieldAttributes fieldAttributes = FieldAttributes.PrivateScope;
			FieldAttributes fieldAttributes2 = FieldAttributes.PrivateScope;
			Context context4 = null;
			if (statement != null)
			{
				ast2 = statement;
				arrayList4.Add(statement);
				arrayList.Add(this.CurrentPositionContext());
				context4 = statement.context.Clone();
				num = 1;
			}
			else
			{
				context4 = this.currentToken.Clone();
			}
			parsedOK = true;
			for (;;)
			{
				JSToken jstoken = JSToken.None;
				JSToken token = this.currentToken.token;
				if (token <= JSToken.Void)
				{
					switch (token)
					{
					case JSToken.Internal:
					case JSToken.Abstract:
					case JSToken.Public:
					case JSToken.Static:
					case JSToken.Private:
					case JSToken.Protected:
					case JSToken.Final:
						jstoken = this.currentToken.token;
						break;
					case JSToken.Event:
					case JSToken.LeftCurly:
					case JSToken.Semicolon:
					case JSToken.Null:
					case JSToken.True:
					case JSToken.False:
					case JSToken.This:
						goto IL_047E;
					case JSToken.Var:
					case JSToken.Const:
						goto IL_0159;
					case JSToken.Class:
						goto IL_02E7;
					case JSToken.Function:
						goto IL_01E2;
					case JSToken.Identifier:
						break;
					default:
						if (token != JSToken.Void)
						{
							goto IL_047E;
						}
						goto IL_0401;
					}
					bool flag = true;
					bool flag2;
					statement = this.ParseUnaryExpression(out flag2, ref flag, false, jstoken == JSToken.None);
					ast2 = statement;
					if (jstoken != JSToken.None)
					{
						if (statement is Lookup)
						{
							goto IL_07ED;
						}
						if (num != 2)
						{
							arrayList2.Add(this.currentToken.Clone());
						}
					}
					jstoken = JSToken.None;
					if (!flag)
					{
						goto IL_047E;
					}
					arrayList4.Add(statement);
				}
				else
				{
					if (token == JSToken.Interface)
					{
						goto IL_02A8;
					}
					switch (token)
					{
					case JSToken.Boolean:
					case JSToken.Byte:
					case JSToken.Char:
					case JSToken.Double:
					case JSToken.Float:
					case JSToken.Int:
					case JSToken.Long:
						goto IL_0401;
					case JSToken.Decimal:
					case JSToken.DoubleColon:
					case JSToken.Ensure:
					case JSToken.Goto:
					case JSToken.Invariant:
						goto IL_047E;
					case JSToken.Enum:
						goto IL_036F;
					default:
						if (token != JSToken.Short)
						{
							goto IL_047E;
						}
						goto IL_0401;
					}
				}
				IL_0A12:
				if (num == 2)
				{
					continue;
				}
				if (this.scanner.GotEndOfLine())
				{
					num = 0;
					continue;
				}
				num++;
				arrayList.Add(this.currentToken.Clone());
				continue;
				IL_0401:
				parsedOK = false;
				ast2 = new Lookup(this.currentToken);
				jstoken = JSToken.None;
				arrayList4.Add(ast2);
				this.GetNextToken();
				goto IL_0A12;
				IL_07ED:
				switch (jstoken)
				{
				case JSToken.Internal:
					fieldAttributes2 = FieldAttributes.Assembly;
					break;
				case JSToken.Abstract:
					if (context != null)
					{
						arrayList3.Add(JSError.SyntaxError);
						arrayList3.Add(statement.context.Clone());
						goto IL_0A12;
					}
					context = statement.context.Clone();
					goto IL_0A12;
				case JSToken.Public:
					fieldAttributes2 = FieldAttributes.Public;
					break;
				case JSToken.Static:
					if (isInsideClass)
					{
						fieldAttributes2 = FieldAttributes.Static;
						if (context2 != null)
						{
							arrayList3.Add(JSError.SyntaxError);
							arrayList3.Add(statement.context.Clone());
						}
						else
						{
							context2 = statement.context.Clone();
						}
					}
					else
					{
						arrayList3.Add(JSError.NotInsideClass);
						arrayList3.Add(statement.context.Clone());
					}
					break;
				case JSToken.Private:
					if (isInsideClass)
					{
						fieldAttributes2 = FieldAttributes.Private;
					}
					else
					{
						arrayList3.Add(JSError.NotInsideClass);
						arrayList3.Add(statement.context.Clone());
					}
					break;
				case JSToken.Protected:
					if (isInsideClass)
					{
						fieldAttributes2 = FieldAttributes.Family;
					}
					else
					{
						arrayList3.Add(JSError.NotInsideClass);
						arrayList3.Add(statement.context.Clone());
					}
					break;
				case JSToken.Final:
					if (context3 != null)
					{
						arrayList3.Add(JSError.SyntaxError);
						arrayList3.Add(statement.context.Clone());
						goto IL_0A12;
					}
					context3 = statement.context.Clone();
					goto IL_0A12;
				}
				if ((fieldAttributes & FieldAttributes.FieldAccessMask) == fieldAttributes2 && fieldAttributes2 != FieldAttributes.PrivateScope)
				{
					arrayList3.Add(JSError.DupVisibility);
					arrayList3.Add(statement.context.Clone());
					goto IL_0A12;
				}
				if ((fieldAttributes & FieldAttributes.FieldAccessMask) <= FieldAttributes.PrivateScope || (fieldAttributes2 & FieldAttributes.FieldAccessMask) <= FieldAttributes.PrivateScope)
				{
					fieldAttributes |= fieldAttributes2;
					context4.UpdateWith(statement.context);
					goto IL_0A12;
				}
				if ((fieldAttributes2 == FieldAttributes.Family && (fieldAttributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly) || (fieldAttributes2 == FieldAttributes.Assembly && (fieldAttributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family))
				{
					fieldAttributes &= ~FieldAttributes.FieldAccessMask;
					fieldAttributes |= FieldAttributes.FamORAssem;
					goto IL_0A12;
				}
				arrayList3.Add(JSError.IncompatibleVisibility);
				arrayList3.Add(statement.context.Clone());
				goto IL_0A12;
				IL_047E:
				parsedOK = false;
				if (num != 2)
				{
					goto Block_33;
				}
				if (arrayList4.Count > 0)
				{
					AST ast3 = (AST)arrayList4[arrayList4.Count - 1];
					if (ast3 is Lookup)
					{
						if (JSToken.Semicolon == this.currentToken.token || JSToken.Colon == this.currentToken.token)
						{
							this.ReportError(JSError.BadVariableDeclaration, ast3.context.Clone());
							this.SkipTokensAndThrow();
						}
					}
					else if (ast3 is Call && ((Call)ast3).CanBeFunctionDeclaration())
					{
						if (JSToken.Colon == this.currentToken.token || JSToken.LeftCurly == this.currentToken.token)
						{
							this.ReportError(JSError.BadFunctionDeclaration, ast3.context.Clone(), true);
							if (JSToken.Colon == this.currentToken.token)
							{
								this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartBlockNoSkipTokenSet);
								try
								{
									this.SkipTokensAndThrow();
								}
								catch (RecoveryTokenException)
								{
								}
								finally
								{
									this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartBlockNoSkipTokenSet);
								}
							}
							this.errorToken = null;
							if (JSToken.LeftCurly == this.currentToken.token)
							{
								FunctionScope functionScope = new FunctionScope(this.Globals.ScopeStack.Peek(), isInsideClass);
								this.Globals.ScopeStack.Push(functionScope);
								try
								{
									this.ParseBlock();
								}
								finally
								{
									this.Globals.ScopeStack.Pop();
								}
								this.SkipTokensAndThrow();
							}
						}
						else
						{
							this.ReportError(JSError.SyntaxError, ast3.context.Clone());
						}
						this.SkipTokensAndThrow();
					}
				}
				if (JSToken.LeftCurly == this.currentToken.token && isInsideClass)
				{
					goto Block_48;
				}
				this.ReportError(JSError.MissingConstructForAttributes, context4.CombineWith(this.currentToken));
				this.SkipTokensAndThrow();
				goto IL_07ED;
			}
			IL_0159:
			int i = 0;
			int count = arrayList3.Count;
			while (i < count)
			{
				this.ReportError((JSError)arrayList3[i], (Context)arrayList3[i + 1], true);
				i += 2;
			}
			if (context != null)
			{
				this.ReportError(JSError.IllegalVisibility, context, true);
			}
			if (context3 != null)
			{
				this.ReportError(JSError.IllegalVisibility, context3, true);
			}
			context4.UpdateWith(this.currentToken);
			return this.ParseVariableStatement(fieldAttributes, this.FromASTListToCustomAttributeList(arrayList4), this.currentToken.token);
			IL_01E2:
			int j = 0;
			int count2 = arrayList3.Count;
			while (j < count2)
			{
				this.ReportError((JSError)arrayList3[j], (Context)arrayList3[j + 1], true);
				j += 2;
			}
			context4.UpdateWith(this.currentToken);
			if (context2 != null)
			{
				if (context != null)
				{
					context2.HandleError(JSError.AbstractCannotBeStatic);
					context2 = null;
				}
				else if (context3 != null)
				{
					context3.HandleError(JSError.StaticIsAlreadyFinal);
					context3 = null;
				}
			}
			if (context != null)
			{
				if (context3 != null)
				{
					context3.HandleError(JSError.FinalPrecludesAbstract);
					context3 = null;
				}
				if (fieldAttributes2 == FieldAttributes.Private)
				{
					context.HandleError(JSError.AbstractCannotBePrivate);
					fieldAttributes2 = FieldAttributes.Family;
				}
			}
			return this.ParseFunction(fieldAttributes, false, context4, isInsideClass, context != null, context3 != null, false, this.FromASTListToCustomAttributeList(arrayList4));
			IL_02A8:
			if (context != null)
			{
				this.ReportError(JSError.IllegalVisibility, context, true);
				context = null;
			}
			if (context3 != null)
			{
				this.ReportError(JSError.IllegalVisibility, context3, true);
				context3 = null;
			}
			if (context2 != null)
			{
				this.ReportError(JSError.IllegalVisibility, context2, true);
				context2 = null;
			}
			IL_02E7:
			int k = 0;
			int count3 = arrayList3.Count;
			while (k < count3)
			{
				this.ReportError((JSError)arrayList3[k], (Context)arrayList3[k + 1], true);
				k += 2;
			}
			context4.UpdateWith(this.currentToken);
			if (context3 != null && context != null)
			{
				context3.HandleError(JSError.FinalPrecludesAbstract);
			}
			return this.ParseClass(fieldAttributes, context2 != null, context4, context != null, context3 != null, this.FromASTListToCustomAttributeList(arrayList4));
			IL_036F:
			int l = 0;
			int count4 = arrayList3.Count;
			while (l < count4)
			{
				this.ReportError((JSError)arrayList3[l], (Context)arrayList3[l + 1], true);
				l += 2;
			}
			context4.UpdateWith(this.currentToken);
			if (context != null)
			{
				this.ReportError(JSError.IllegalVisibility, context, true);
			}
			if (context3 != null)
			{
				this.ReportError(JSError.IllegalVisibility, context3, true);
			}
			if (context2 != null)
			{
				this.ReportError(JSError.IllegalVisibility, context2, true);
			}
			return this.ParseEnum(fieldAttributes, context4, this.FromASTListToCustomAttributeList(arrayList4));
			Block_33:
			if (ast != statement || statement == null)
			{
				statement = ast2;
				int m = 0;
				int count5 = arrayList2.Count;
				while (m < count5)
				{
					this.ForceReportInfo((Context)arrayList2[m], JSError.KeywordUsedAsIdentifier);
					m++;
				}
				int n = 0;
				int count6 = arrayList.Count;
				while (n < count6)
				{
					if (!this.currentToken.Equals((Context)arrayList[n]))
					{
						this.ReportError(JSError.NoSemicolon, (Context)arrayList[n], true);
					}
					n++;
				}
			}
			return statement;
			Block_48:
			int num2 = 0;
			int count7 = arrayList3.Count;
			while (num2 < count7)
			{
				this.ReportError((JSError)arrayList3[num2], (Context)arrayList3[num2 + 1]);
				num2 += 2;
			}
			if (context2 == null)
			{
				this.ReportError(JSError.StaticMissingInStaticInit, this.CurrentPositionContext());
			}
			string name = ((ClassScope)this.Globals.ScopeStack.Peek()).name;
			bool flag3 = true;
			foreach (object obj in arrayList4)
			{
				flag3 = false;
				if (context2 == null || !(obj is Lookup) || !(obj.ToString() == name) || ((Lookup)obj).context.StartColumn <= context2.StartColumn)
				{
					this.ReportError(JSError.SyntaxError, ((AST)obj).context);
				}
			}
			if (flag3)
			{
				this.ReportError(JSError.NoIdentifier, this.CurrentPositionContext());
			}
			this.errorToken = null;
			parsedOK = true;
			return this.ParseStaticInitializer(context4);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0003A4BC File Offset: 0x000394BC
		private Block ParseBlock()
		{
			Context context;
			return this.ParseBlock(out context);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0003A4D4 File Offset: 0x000394D4
		private Block ParseBlock(out Context closingBraceContext)
		{
			closingBraceContext = null;
			this.blockType.Add(JSParser.BlockType.Block);
			Block block = new Block(this.currentToken.Clone());
			this.GetNextToken();
			this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
			this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
			try
			{
				while (JSToken.RightCurly != this.currentToken.token)
				{
					try
					{
						block.Append(this.ParseStatement());
					}
					catch (RecoveryTokenException ex)
					{
						if (ex._partiallyComputedNode != null)
						{
							block.Append(ex._partiallyComputedNode);
						}
						if (this.IndexOfToken(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, ex) == -1)
						{
							throw ex;
						}
					}
				}
			}
			catch (RecoveryTokenException ex2)
			{
				if (this.IndexOfToken(NoSkipTokenSet.s_BlockNoSkipTokenSet, ex2) == -1)
				{
					ex2._partiallyComputedNode = block;
					throw ex2;
				}
			}
			finally
			{
				this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
				this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
				this.blockType.RemoveAt(this.blockType.Count - 1);
			}
			closingBraceContext = this.currentToken.Clone();
			block.context.UpdateWith(this.currentToken);
			this.GetNextToken();
			return block;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0003A61C File Offset: 0x0003961C
		private AST ParseVariableStatement(FieldAttributes visibility, CustomAttributeList customAttributes, JSToken kind)
		{
			Block block = new Block(this.currentToken.Clone());
			bool flag = true;
			AST ast = null;
			for (;;)
			{
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfLineToken);
				try
				{
					ast = this.ParseIdentifierInitializer(JSToken.None, visibility, customAttributes, kind);
				}
				catch (RecoveryTokenException ex)
				{
					if (ex._partiallyComputedNode != null && !flag)
					{
						block.Append(ex._partiallyComputedNode);
						block.context.UpdateWith(ex._partiallyComputedNode.context);
						ex._partiallyComputedNode = block;
					}
					if (this.IndexOfToken(NoSkipTokenSet.s_EndOfLineToken, ex) == -1)
					{
						throw ex;
					}
					if (flag)
					{
						ast = ex._partiallyComputedNode;
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfLineToken);
				}
				if (JSToken.Semicolon == this.currentToken.token || JSToken.RightCurly == this.currentToken.token)
				{
					break;
				}
				if (JSToken.Comma != this.currentToken.token)
				{
					goto IL_00FA;
				}
				flag = false;
				block.Append(ast);
			}
			if (JSToken.Semicolon == this.currentToken.token)
			{
				ast.context.UpdateWith(this.currentToken);
				this.GetNextToken();
				goto IL_0113;
			}
			goto IL_0113;
			IL_00FA:
			if (!this.scanner.GotEndOfLine())
			{
				this.ReportError(JSError.NoSemicolon, true);
			}
			IL_0113:
			if (flag)
			{
				return ast;
			}
			block.Append(ast);
			block.context.UpdateWith(ast.context);
			return block;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0003A778 File Offset: 0x00039778
		private AST ParseIdentifierInitializer(JSToken inToken, FieldAttributes visibility, CustomAttributeList customAttributes, JSToken kind)
		{
			Lookup lookup = null;
			TypeExpression typeExpression = null;
			AST ast = null;
			RecoveryTokenException ex = null;
			this.GetNextToken();
			if (JSToken.Identifier != this.currentToken.token)
			{
				string text = JSKeyword.CanBeIdentifier(this.currentToken.token);
				if (text != null)
				{
					this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
					lookup = new Lookup(text, this.currentToken.Clone());
				}
				else
				{
					this.ReportError(JSError.NoIdentifier);
					lookup = new Lookup("#_Missing Identifier_#" + JSParser.s_cDummyName++, this.CurrentPositionContext());
				}
			}
			else
			{
				lookup = new Lookup(this.scanner.GetIdentifier(), this.currentToken.Clone());
			}
			this.GetNextToken();
			Context context = lookup.context.Clone();
			this.noSkipTokenSet.Add(NoSkipTokenSet.s_VariableDeclNoSkipTokenSet);
			try
			{
				if (JSToken.Colon == this.currentToken.token)
				{
					try
					{
						typeExpression = this.ParseTypeExpression();
					}
					catch (RecoveryTokenException ex2)
					{
						typeExpression = (TypeExpression)ex2._partiallyComputedNode;
						throw ex2;
					}
					finally
					{
						if (typeExpression != null)
						{
							context.UpdateWith(typeExpression.context);
						}
					}
				}
				if (JSToken.Assign == this.currentToken.token || JSToken.Equal == this.currentToken.token)
				{
					if (JSToken.Equal == this.currentToken.token)
					{
						this.ReportError(JSError.NoEqual, true);
					}
					this.GetNextToken();
					try
					{
						ast = this.ParseExpression(true, inToken);
					}
					catch (RecoveryTokenException ex3)
					{
						ast = ex3._partiallyComputedNode;
						throw ex3;
					}
					finally
					{
						if (ast != null)
						{
							context.UpdateWith(ast.context);
						}
					}
				}
			}
			catch (RecoveryTokenException ex4)
			{
				if (this.IndexOfToken(NoSkipTokenSet.s_VariableDeclNoSkipTokenSet, ex4) == -1)
				{
					ex = ex4;
				}
			}
			finally
			{
				this.noSkipTokenSet.Remove(NoSkipTokenSet.s_VariableDeclNoSkipTokenSet);
			}
			AST ast2;
			if (JSToken.Var == kind)
			{
				ast2 = new VariableDeclaration(context, lookup, typeExpression, ast, visibility, customAttributes);
			}
			else
			{
				if (ast == null)
				{
					this.ForceReportInfo(JSError.NoEqual);
				}
				ast2 = new Constant(context, lookup, typeExpression, ast, visibility, customAttributes);
			}
			if (customAttributes != null)
			{
				customAttributes.SetTarget(ast2);
			}
			if (ex != null)
			{
				ex._partiallyComputedNode = ast2;
				throw ex;
			}
			return ast2;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0003A9BC File Offset: 0x000399BC
		private AST ParseQualifiedIdentifier(JSError error)
		{
			this.GetNextToken();
			AST ast = null;
			Context context = this.currentToken.Clone();
			if (JSToken.Identifier != this.currentToken.token)
			{
				string text = JSKeyword.CanBeIdentifier(this.currentToken.token);
				if (text != null)
				{
					JSToken token = this.currentToken.token;
					if (token != JSToken.Void)
					{
						switch (token)
						{
						case JSToken.Boolean:
						case JSToken.Byte:
						case JSToken.Char:
						case JSToken.Double:
						case JSToken.Float:
						case JSToken.Int:
						case JSToken.Long:
							goto IL_009A;
						case JSToken.Decimal:
						case JSToken.DoubleColon:
						case JSToken.Enum:
						case JSToken.Ensure:
						case JSToken.Goto:
						case JSToken.Invariant:
							break;
						default:
							if (token == JSToken.Short)
							{
								goto IL_009A;
							}
							break;
						}
						this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
					}
					IL_009A:
					ast = new Lookup(text, context);
				}
				else
				{
					this.ReportError(error, true);
					this.SkipTokensAndThrow();
				}
			}
			else
			{
				ast = new Lookup(this.scanner.GetIdentifier(), context);
			}
			this.GetNextToken();
			if (JSToken.AccessField == this.currentToken.token)
			{
				ast = this.ParseScopeSequence(ast, error);
			}
			return ast;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0003AAB0 File Offset: 0x00039AB0
		private AST ParseScopeSequence(AST qualid, JSError error)
		{
			ConstantWrapper constantWrapper = null;
			do
			{
				this.GetNextToken();
				if (JSToken.Identifier != this.currentToken.token)
				{
					string text = JSKeyword.CanBeIdentifier(this.currentToken.token);
					if (text != null)
					{
						this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
						constantWrapper = new ConstantWrapper(text, this.currentToken.Clone());
					}
					else
					{
						this.ReportError(error, true);
						this.SkipTokensAndThrow(qualid);
					}
				}
				else
				{
					constantWrapper = new ConstantWrapper(this.scanner.GetIdentifier(), this.currentToken.Clone());
				}
				qualid = new Member(qualid.context.CombineWith(this.currentToken), qualid, constantWrapper);
				this.GetNextToken();
			}
			while (JSToken.AccessField == this.currentToken.token);
			return qualid;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0003AB6C File Offset: 0x00039B6C
		private TypeExpression ParseTypeExpression()
		{
			AST ast = null;
			try
			{
				ast = this.ParseQualifiedIdentifier(JSError.NeedType);
			}
			catch (RecoveryTokenException ex)
			{
				if (ex._partiallyComputedNode != null)
				{
					ex._partiallyComputedNode = new TypeExpression(ex._partiallyComputedNode);
				}
				throw ex;
			}
			TypeExpression typeExpression = new TypeExpression(ast);
			if (typeExpression != null)
			{
				while (!this.scanner.GotEndOfLine() && JSToken.LeftBracket == this.currentToken.token)
				{
					this.GetNextToken();
					int num = 1;
					while (JSToken.Comma == this.currentToken.token)
					{
						this.GetNextToken();
						num++;
					}
					if (JSToken.RightBracket != this.currentToken.token)
					{
						this.ReportError(JSError.NoRightBracket);
					}
					this.GetNextToken();
					if (typeExpression.isArray)
					{
						typeExpression = new TypeExpression(typeExpression);
					}
					typeExpression.isArray = true;
					typeExpression.rank = num;
				}
			}
			return typeExpression;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0003AC40 File Offset: 0x00039C40
		private If ParseIfStatement()
		{
			Context context = this.currentToken.Clone();
			AST ast = null;
			AST ast2 = null;
			AST ast3 = null;
			this.blockType.Add(JSParser.BlockType.Block);
			try
			{
				this.GetNextToken();
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
				try
				{
					if (JSToken.LeftParen != this.currentToken.token)
					{
						this.ReportError(JSError.NoLeftParen);
					}
					this.GetNextToken();
					ast = this.ParseExpression();
					if (JSToken.RightParen != this.currentToken.token)
					{
						context.UpdateWith(ast.context);
						this.ReportError(JSError.NoRightParen);
					}
					else
					{
						context.UpdateWith(this.currentToken);
					}
					this.GetNextToken();
				}
				catch (RecoveryTokenException ex)
				{
					if (ex._partiallyComputedNode != null)
					{
						ast = ex._partiallyComputedNode;
					}
					else
					{
						ast = new ConstantWrapper(true, this.CurrentPositionContext());
					}
					if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1)
					{
						ex._partiallyComputedNode = null;
						throw ex;
					}
					if (ex._token == JSToken.RightParen)
					{
						this.GetNextToken();
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
				}
				if (ast is Assign)
				{
					ast.context.HandleError(JSError.SuspectAssignment);
				}
				if (JSToken.Semicolon == this.currentToken.token)
				{
					this.ForceReportInfo(JSError.SuspectSemicolon);
				}
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_IfBodyNoSkipTokenSet);
				try
				{
					ast2 = this.ParseStatement();
				}
				catch (RecoveryTokenException ex2)
				{
					if (ex2._partiallyComputedNode != null)
					{
						ast2 = ex2._partiallyComputedNode;
					}
					else
					{
						ast2 = new Block(this.CurrentPositionContext());
					}
					if (this.IndexOfToken(NoSkipTokenSet.s_IfBodyNoSkipTokenSet, ex2) == -1)
					{
						ex2._partiallyComputedNode = new If(context, ast, ast2, ast3);
						throw ex2;
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_IfBodyNoSkipTokenSet);
				}
				if (JSToken.Else == this.currentToken.token)
				{
					this.GetNextToken();
					if (JSToken.Semicolon == this.currentToken.token)
					{
						this.ForceReportInfo(JSError.SuspectSemicolon);
					}
					try
					{
						ast3 = this.ParseStatement();
					}
					catch (RecoveryTokenException ex3)
					{
						if (ex3._partiallyComputedNode != null)
						{
							ast3 = ex3._partiallyComputedNode;
						}
						else
						{
							ast3 = new Block(this.CurrentPositionContext());
						}
						ex3._partiallyComputedNode = new If(context, ast, ast2, ast3);
						throw ex3;
					}
				}
			}
			finally
			{
				this.blockType.RemoveAt(this.blockType.Count - 1);
			}
			return new If(context, ast, ast2, ast3);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0003AF1C File Offset: 0x00039F1C
		private AST ParseForStatement()
		{
			this.blockType.Add(JSParser.BlockType.Loop);
			AST ast = null;
			try
			{
				Context context = this.currentToken.Clone();
				this.GetNextToken();
				if (JSToken.LeftParen != this.currentToken.token)
				{
					this.ReportError(JSError.NoLeftParen);
				}
				this.GetNextToken();
				bool flag = false;
				bool flag2 = false;
				AST ast2 = null;
				AST ast3 = null;
				AST ast4 = null;
				AST ast5 = null;
				try
				{
					if (JSToken.Var == this.currentToken.token)
					{
						flag = true;
						ast3 = this.ParseIdentifierInitializer(JSToken.In, FieldAttributes.PrivateScope, null, JSToken.Var);
						while (JSToken.Comma == this.currentToken.token)
						{
							flag = false;
							AST ast6 = this.ParseIdentifierInitializer(JSToken.In, FieldAttributes.PrivateScope, null, JSToken.Var);
							ast3 = new Comma(ast3.context.CombineWith(ast6.context), ast3, ast6);
						}
						if (flag)
						{
							if (JSToken.In == this.currentToken.token)
							{
								this.GetNextToken();
								ast4 = this.ParseExpression();
							}
							else
							{
								flag = false;
							}
						}
					}
					else if (JSToken.Semicolon != this.currentToken.token)
					{
						bool flag3;
						ast3 = this.ParseUnaryExpression(out flag3, false);
						if (flag3 && JSToken.In == this.currentToken.token)
						{
							flag = true;
							ast2 = ast3;
							ast3 = null;
							this.GetNextToken();
							this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
							try
							{
								try
								{
									ast4 = this.ParseExpression();
								}
								catch (RecoveryTokenException ex)
								{
									if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1)
									{
										ex._partiallyComputedNode = null;
										throw ex;
									}
									if (ex._partiallyComputedNode == null)
									{
										ast4 = new ConstantWrapper(true, this.CurrentPositionContext());
									}
									else
									{
										ast4 = ex._partiallyComputedNode;
									}
									if (ex._token == JSToken.RightParen)
									{
										this.GetNextToken();
										flag2 = true;
									}
								}
								goto IL_01D2;
							}
							finally
							{
								this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
							}
						}
						ast3 = this.ParseExpression(ast3, false, flag3, JSToken.In);
					}
					else
					{
						ast3 = new EmptyLiteral(this.CurrentPositionContext());
					}
					IL_01D2:;
				}
				catch (RecoveryTokenException ex2)
				{
					ex2._partiallyComputedNode = null;
					throw ex2;
				}
				if (flag)
				{
					if (!flag2)
					{
						if (JSToken.RightParen != this.currentToken.token)
						{
							this.ReportError(JSError.NoRightParen);
						}
						context.UpdateWith(this.currentToken);
						this.GetNextToken();
					}
					AST ast7 = null;
					try
					{
						ast7 = this.ParseStatement();
					}
					catch (RecoveryTokenException ex3)
					{
						if (ex3._partiallyComputedNode == null)
						{
							ast7 = new Block(this.CurrentPositionContext());
						}
						else
						{
							ast7 = ex3._partiallyComputedNode;
						}
						ex3._partiallyComputedNode = new ForIn(context, ast2, ast3, ast4, ast7);
						throw ex3;
					}
					ast = new ForIn(context, ast2, ast3, ast4, ast7);
				}
				else
				{
					this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
					try
					{
						if (JSToken.Semicolon != this.currentToken.token)
						{
							this.ReportError(JSError.NoSemicolon);
							if (JSToken.Colon == this.currentToken.token)
							{
								this.noSkipTokenSet.Add(NoSkipTokenSet.s_VariableDeclNoSkipTokenSet);
								try
								{
									this.SkipTokensAndThrow();
								}
								catch (RecoveryTokenException ex4)
								{
									if (JSToken.Semicolon != this.currentToken.token)
									{
										throw ex4;
									}
									this.errorToken = null;
								}
								finally
								{
									this.noSkipTokenSet.Remove(NoSkipTokenSet.s_VariableDeclNoSkipTokenSet);
								}
							}
						}
						this.GetNextToken();
						if (JSToken.Semicolon != this.currentToken.token)
						{
							ast4 = this.ParseExpression();
							if (JSToken.Semicolon != this.currentToken.token)
							{
								this.ReportError(JSError.NoSemicolon);
							}
						}
						else
						{
							ast4 = new ConstantWrapper(true, this.CurrentPositionContext());
						}
						this.GetNextToken();
						if (JSToken.RightParen != this.currentToken.token)
						{
							ast5 = this.ParseExpression();
						}
						else
						{
							ast5 = new EmptyLiteral(this.CurrentPositionContext());
						}
						if (JSToken.RightParen != this.currentToken.token)
						{
							this.ReportError(JSError.NoRightParen);
						}
						context.UpdateWith(this.currentToken);
						this.GetNextToken();
					}
					catch (RecoveryTokenException ex5)
					{
						if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex5) == -1)
						{
							ex5._partiallyComputedNode = null;
							throw ex5;
						}
						ex5._partiallyComputedNode = null;
						if (ast4 == null)
						{
							ast4 = new ConstantWrapper(true, this.CurrentPositionContext());
						}
						if (ast5 == null)
						{
							ast5 = new EmptyLiteral(this.CurrentPositionContext());
						}
						if (ex5._token == JSToken.RightParen)
						{
							this.GetNextToken();
						}
					}
					finally
					{
						this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
					}
					AST ast8 = null;
					try
					{
						ast8 = this.ParseStatement();
					}
					catch (RecoveryTokenException ex6)
					{
						if (ex6._partiallyComputedNode == null)
						{
							ast8 = new Block(this.CurrentPositionContext());
						}
						else
						{
							ast8 = ex6._partiallyComputedNode;
						}
						ex6._partiallyComputedNode = new For(context, ast3, ast4, ast5, ast8);
						throw ex6;
					}
					ast = new For(context, ast3, ast4, ast5, ast8);
				}
			}
			finally
			{
				this.blockType.RemoveAt(this.blockType.Count - 1);
			}
			return ast;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0003B4A0 File Offset: 0x0003A4A0
		private DoWhile ParseDoStatement()
		{
			Context context = null;
			AST ast = null;
			AST ast2 = null;
			this.blockType.Add(JSParser.BlockType.Loop);
			try
			{
				this.GetNextToken();
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_DoWhileBodyNoSkipTokenSet);
				try
				{
					ast = this.ParseStatement();
				}
				catch (RecoveryTokenException ex)
				{
					if (ex._partiallyComputedNode != null)
					{
						ast = ex._partiallyComputedNode;
					}
					else
					{
						ast = new Block(this.CurrentPositionContext());
					}
					if (this.IndexOfToken(NoSkipTokenSet.s_DoWhileBodyNoSkipTokenSet, ex) == -1)
					{
						ex._partiallyComputedNode = new DoWhile(this.CurrentPositionContext(), ast, new ConstantWrapper(false, this.CurrentPositionContext()));
						throw ex;
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_DoWhileBodyNoSkipTokenSet);
				}
				if (JSToken.While != this.currentToken.token)
				{
					this.ReportError(JSError.NoWhile);
				}
				context = this.currentToken.Clone();
				this.GetNextToken();
				if (JSToken.LeftParen != this.currentToken.token)
				{
					this.ReportError(JSError.NoLeftParen);
				}
				this.GetNextToken();
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
				try
				{
					ast2 = this.ParseExpression();
					if (JSToken.RightParen != this.currentToken.token)
					{
						this.ReportError(JSError.NoRightParen);
						context.UpdateWith(ast2.context);
					}
					else
					{
						context.UpdateWith(this.currentToken);
					}
					this.GetNextToken();
				}
				catch (RecoveryTokenException ex2)
				{
					if (ex2._partiallyComputedNode != null)
					{
						ast2 = ex2._partiallyComputedNode;
					}
					else
					{
						ast2 = new ConstantWrapper(false, this.CurrentPositionContext());
					}
					if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex2) == -1)
					{
						ex2._partiallyComputedNode = new DoWhile(context, ast, ast2);
						throw ex2;
					}
					if (JSToken.RightParen == this.currentToken.token)
					{
						this.GetNextToken();
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
				}
				if (JSToken.Semicolon == this.currentToken.token)
				{
					context.UpdateWith(this.currentToken);
					this.GetNextToken();
				}
			}
			finally
			{
				this.blockType.RemoveAt(this.blockType.Count - 1);
			}
			return new DoWhile(context, ast, ast2);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0003B71C File Offset: 0x0003A71C
		private While ParseWhileStatement()
		{
			Context context = this.currentToken.Clone();
			AST ast = null;
			AST ast2 = null;
			this.blockType.Add(JSParser.BlockType.Loop);
			try
			{
				this.GetNextToken();
				if (JSToken.LeftParen != this.currentToken.token)
				{
					this.ReportError(JSError.NoLeftParen);
				}
				this.GetNextToken();
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
				try
				{
					ast = this.ParseExpression();
					if (JSToken.RightParen != this.currentToken.token)
					{
						this.ReportError(JSError.NoRightParen);
						context.UpdateWith(ast.context);
					}
					else
					{
						context.UpdateWith(this.currentToken);
					}
					this.GetNextToken();
				}
				catch (RecoveryTokenException ex)
				{
					if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1)
					{
						ex._partiallyComputedNode = null;
						throw ex;
					}
					if (ex._partiallyComputedNode != null)
					{
						ast = ex._partiallyComputedNode;
					}
					else
					{
						ast = new ConstantWrapper(false, this.CurrentPositionContext());
					}
					if (JSToken.RightParen == this.currentToken.token)
					{
						this.GetNextToken();
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
				}
				try
				{
					ast2 = this.ParseStatement();
				}
				catch (RecoveryTokenException ex2)
				{
					if (ex2._partiallyComputedNode != null)
					{
						ast2 = ex2._partiallyComputedNode;
					}
					else
					{
						ast2 = new Block(this.CurrentPositionContext());
					}
					ex2._partiallyComputedNode = new While(context, ast, ast2);
					throw ex2;
				}
			}
			finally
			{
				this.blockType.RemoveAt(this.blockType.Count - 1);
			}
			return new While(context, ast, ast2);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0003B8F0 File Offset: 0x0003A8F0
		private Continue ParseContinueStatement()
		{
			Context context = this.currentToken.Clone();
			this.GetNextToken();
			string text = null;
			int num;
			if (!this.scanner.GotEndOfLine() && (JSToken.Identifier == this.currentToken.token || (text = JSKeyword.CanBeIdentifier(this.currentToken.token)) != null))
			{
				context.UpdateWith(this.currentToken);
				if (text != null)
				{
					this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
				}
				else
				{
					text = this.scanner.GetIdentifier();
				}
				object obj = this.labelTable[text];
				if (obj == null)
				{
					this.ReportError(JSError.NoLabel, true);
					this.GetNextToken();
					return null;
				}
				num = (int)obj;
				if ((JSParser.BlockType)this.blockType[num] != JSParser.BlockType.Loop)
				{
					this.ReportError(JSError.BadContinue, context.Clone(), true);
				}
				this.GetNextToken();
			}
			else
			{
				num = this.blockType.Count - 1;
				while (num >= 0 && (JSParser.BlockType)this.blockType[num] != JSParser.BlockType.Loop)
				{
					num--;
				}
				if (num < 0)
				{
					this.ReportError(JSError.BadContinue, context, true);
					return null;
				}
			}
			if (JSToken.Semicolon == this.currentToken.token)
			{
				context.UpdateWith(this.currentToken);
				this.GetNextToken();
			}
			else if (JSToken.RightCurly != this.currentToken.token && !this.scanner.GotEndOfLine())
			{
				this.ReportError(JSError.NoSemicolon, true);
			}
			int num2 = 0;
			int i = num;
			int count = this.blockType.Count;
			while (i < count)
			{
				if ((JSParser.BlockType)this.blockType[i] == JSParser.BlockType.Finally)
				{
					num++;
					num2++;
				}
				i++;
			}
			if (num2 > this.finallyEscaped)
			{
				this.finallyEscaped = num2;
			}
			return new Continue(context, this.blockType.Count - num, num2 > 0);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0003BAC0 File Offset: 0x0003AAC0
		private Break ParseBreakStatement()
		{
			Context context = this.currentToken.Clone();
			this.GetNextToken();
			string text = null;
			int num;
			if (!this.scanner.GotEndOfLine() && (JSToken.Identifier == this.currentToken.token || (text = JSKeyword.CanBeIdentifier(this.currentToken.token)) != null))
			{
				context.UpdateWith(this.currentToken);
				if (text != null)
				{
					this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
				}
				else
				{
					text = this.scanner.GetIdentifier();
				}
				object obj = this.labelTable[text];
				if (obj == null)
				{
					this.ReportError(JSError.NoLabel, true);
					this.GetNextToken();
					return null;
				}
				num = (int)obj - 1;
				this.GetNextToken();
			}
			else
			{
				num = this.blockType.Count - 1;
				while (((JSParser.BlockType)this.blockType[num] == JSParser.BlockType.Block || (JSParser.BlockType)this.blockType[num] == JSParser.BlockType.Finally) && --num >= 0)
				{
				}
				num--;
				if (num < 0)
				{
					this.ReportError(JSError.BadBreak, context, true);
					return null;
				}
			}
			if (JSToken.Semicolon == this.currentToken.token)
			{
				context.UpdateWith(this.currentToken);
				this.GetNextToken();
			}
			else if (JSToken.RightCurly != this.currentToken.token && !this.scanner.GotEndOfLine())
			{
				this.ReportError(JSError.NoSemicolon, true);
			}
			int num2 = 0;
			int i = num;
			int count = this.blockType.Count;
			while (i < count)
			{
				if ((JSParser.BlockType)this.blockType[i] == JSParser.BlockType.Finally)
				{
					num++;
					num2++;
				}
				i++;
			}
			if (num2 > this.finallyEscaped)
			{
				this.finallyEscaped = num2;
			}
			return new Break(context, this.blockType.Count - num - 1, num2 > 0);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0003BC80 File Offset: 0x0003AC80
		private bool CheckForReturnFromFinally()
		{
			int num = 0;
			for (int i = this.blockType.Count - 1; i >= 0; i--)
			{
				if ((JSParser.BlockType)this.blockType[i] == JSParser.BlockType.Finally)
				{
					num++;
				}
			}
			if (num > this.finallyEscaped)
			{
				this.finallyEscaped = num;
			}
			return num > 0;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0003BCD4 File Offset: 0x0003ACD4
		private Return ParseReturnStatement()
		{
			Context context = this.currentToken.Clone();
			if (this.Globals.ScopeStack.Peek() is FunctionScope)
			{
				AST ast = null;
				this.GetNextToken();
				if (!this.scanner.GotEndOfLine())
				{
					if (JSToken.Semicolon != this.currentToken.token && JSToken.RightCurly != this.currentToken.token)
					{
						this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
						try
						{
							ast = this.ParseExpression();
						}
						catch (RecoveryTokenException ex)
						{
							ast = ex._partiallyComputedNode;
							if (this.IndexOfToken(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet, ex) == -1)
							{
								if (ast != null)
								{
									context.UpdateWith(ast.context);
								}
								ex._partiallyComputedNode = new Return(context, ast, this.CheckForReturnFromFinally());
								throw ex;
							}
						}
						finally
						{
							this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
						}
						if (JSToken.Semicolon != this.currentToken.token && JSToken.RightCurly != this.currentToken.token && !this.scanner.GotEndOfLine())
						{
							this.ReportError(JSError.NoSemicolon, true);
						}
					}
					if (JSToken.Semicolon == this.currentToken.token)
					{
						context.UpdateWith(this.currentToken);
						this.GetNextToken();
					}
					else if (ast != null)
					{
						context.UpdateWith(ast.context);
					}
				}
				return new Return(context, ast, this.CheckForReturnFromFinally());
			}
			this.ReportError(JSError.BadReturn, context, true);
			this.GetNextToken();
			return null;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0003BE54 File Offset: 0x0003AE54
		private Import ParseImportStatement()
		{
			Context context = this.currentToken.Clone();
			AST ast = null;
			try
			{
				ast = this.ParseQualifiedIdentifier(JSError.PackageExpected);
			}
			catch (RecoveryTokenException ex)
			{
				if (ex._partiallyComputedNode != null)
				{
					ex._partiallyComputedNode = new Import(context, ex._partiallyComputedNode);
				}
			}
			if (this.currentToken.token != JSToken.Semicolon && !this.scanner.GotEndOfLine())
			{
				this.ReportError(JSError.NoSemicolon, this.currentToken.Clone());
			}
			return new Import(context, ast);
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0003BEE4 File Offset: 0x0003AEE4
		private With ParseWithStatement()
		{
			Context context = this.currentToken.Clone();
			AST ast = null;
			AST ast2 = null;
			this.blockType.Add(JSParser.BlockType.Block);
			try
			{
				this.GetNextToken();
				if (JSToken.LeftParen != this.currentToken.token)
				{
					this.ReportError(JSError.NoLeftParen);
				}
				this.GetNextToken();
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
				try
				{
					ast = this.ParseExpression();
					if (JSToken.RightParen != this.currentToken.token)
					{
						context.UpdateWith(ast.context);
						this.ReportError(JSError.NoRightParen);
					}
					else
					{
						context.UpdateWith(this.currentToken);
					}
					this.GetNextToken();
				}
				catch (RecoveryTokenException ex)
				{
					if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1)
					{
						ex._partiallyComputedNode = null;
						throw ex;
					}
					if (ex._partiallyComputedNode == null)
					{
						ast = new ConstantWrapper(true, this.CurrentPositionContext());
					}
					else
					{
						ast = ex._partiallyComputedNode;
					}
					context.UpdateWith(ast.context);
					if (ex._token == JSToken.RightParen)
					{
						this.GetNextToken();
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
				}
				try
				{
					ast2 = this.ParseStatement();
				}
				catch (RecoveryTokenException ex2)
				{
					if (ex2._partiallyComputedNode == null)
					{
						ast2 = new Block(this.CurrentPositionContext());
					}
					else
					{
						ast2 = ex2._partiallyComputedNode;
					}
					ex2._partiallyComputedNode = new With(context, ast, ast2);
				}
			}
			finally
			{
				this.blockType.RemoveAt(this.blockType.Count - 1);
			}
			return new With(context, ast, ast2);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0003C0BC File Offset: 0x0003B0BC
		private AST ParseSwitchStatement()
		{
			Context context = this.currentToken.Clone();
			AST ast = null;
			ASTList astlist = null;
			this.blockType.Add(JSParser.BlockType.Switch);
			try
			{
				this.GetNextToken();
				if (JSToken.LeftParen != this.currentToken.token)
				{
					this.ReportError(JSError.NoLeftParen);
				}
				this.GetNextToken();
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_SwitchNoSkipTokenSet);
				try
				{
					ast = this.ParseExpression();
					if (JSToken.RightParen != this.currentToken.token)
					{
						this.ReportError(JSError.NoRightParen);
					}
					this.GetNextToken();
					if (JSToken.LeftCurly != this.currentToken.token)
					{
						this.ReportError(JSError.NoLeftCurly);
					}
					this.GetNextToken();
				}
				catch (RecoveryTokenException ex)
				{
					if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) == -1 && this.IndexOfToken(NoSkipTokenSet.s_SwitchNoSkipTokenSet, ex) == -1)
					{
						ex._partiallyComputedNode = null;
						throw ex;
					}
					if (ex._partiallyComputedNode == null)
					{
						ast = new ConstantWrapper(true, this.CurrentPositionContext());
					}
					else
					{
						ast = ex._partiallyComputedNode;
					}
					if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex) != -1)
					{
						if (ex._token == JSToken.RightParen)
						{
							this.GetNextToken();
						}
						if (JSToken.LeftCurly != this.currentToken.token)
						{
							this.ReportError(JSError.NoLeftCurly);
						}
						this.GetNextToken();
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_SwitchNoSkipTokenSet);
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
				}
				astlist = new ASTList(this.currentToken.Clone());
				bool flag = false;
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
				try
				{
					while (JSToken.RightCurly != this.currentToken.token)
					{
						AST ast2 = null;
						Context context2 = this.currentToken.Clone();
						this.noSkipTokenSet.Add(NoSkipTokenSet.s_CaseNoSkipTokenSet);
						try
						{
							if (JSToken.Case == this.currentToken.token)
							{
								this.GetNextToken();
								ast2 = this.ParseExpression();
							}
							else if (JSToken.Default == this.currentToken.token)
							{
								if (flag)
								{
									this.ReportError(JSError.DupDefault, true);
								}
								else
								{
									flag = true;
								}
								this.GetNextToken();
							}
							else
							{
								flag = true;
								this.ReportError(JSError.BadSwitch);
							}
							if (JSToken.Colon != this.currentToken.token)
							{
								this.ReportError(JSError.NoColon);
							}
							this.GetNextToken();
						}
						catch (RecoveryTokenException ex2)
						{
							if (this.IndexOfToken(NoSkipTokenSet.s_CaseNoSkipTokenSet, ex2) == -1)
							{
								ex2._partiallyComputedNode = null;
								throw ex2;
							}
							ast2 = ex2._partiallyComputedNode;
							if (ex2._token == JSToken.Colon)
							{
								this.GetNextToken();
							}
						}
						finally
						{
							this.noSkipTokenSet.Remove(NoSkipTokenSet.s_CaseNoSkipTokenSet);
						}
						this.blockType.Add(JSParser.BlockType.Block);
						try
						{
							Block block = new Block(this.currentToken.Clone());
							this.noSkipTokenSet.Add(NoSkipTokenSet.s_SwitchNoSkipTokenSet);
							this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
							SwitchCase switchCase;
							try
							{
								while (JSToken.RightCurly != this.currentToken.token && JSToken.Case != this.currentToken.token && JSToken.Default != this.currentToken.token)
								{
									try
									{
										block.Append(this.ParseStatement());
									}
									catch (RecoveryTokenException ex3)
									{
										if (ex3._partiallyComputedNode != null)
										{
											block.Append(ex3._partiallyComputedNode);
											ex3._partiallyComputedNode = null;
										}
										if (this.IndexOfToken(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, ex3) == -1)
										{
											throw ex3;
										}
									}
								}
							}
							catch (RecoveryTokenException ex4)
							{
								if (this.IndexOfToken(NoSkipTokenSet.s_SwitchNoSkipTokenSet, ex4) == -1)
								{
									if (ast2 == null)
									{
										switchCase = new SwitchCase(context2, block);
									}
									else
									{
										switchCase = new SwitchCase(context2, ast2, block);
									}
									astlist.Append(switchCase);
									throw ex4;
								}
							}
							finally
							{
								this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
								this.noSkipTokenSet.Remove(NoSkipTokenSet.s_SwitchNoSkipTokenSet);
							}
							if (JSToken.RightCurly == this.currentToken.token)
							{
								block.context.UpdateWith(this.currentToken);
							}
							if (ast2 == null)
							{
								context2.UpdateWith(block.context);
								switchCase = new SwitchCase(context2, block);
							}
							else
							{
								context2.UpdateWith(block.context);
								switchCase = new SwitchCase(context2, ast2, block);
							}
							astlist.Append(switchCase);
						}
						finally
						{
							this.blockType.RemoveAt(this.blockType.Count - 1);
						}
					}
				}
				catch (RecoveryTokenException ex5)
				{
					if (this.IndexOfToken(NoSkipTokenSet.s_BlockNoSkipTokenSet, ex5) == -1)
					{
						context.UpdateWith(this.CurrentPositionContext());
						ex5._partiallyComputedNode = new Switch(context, ast, astlist);
						throw ex5;
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
				}
				context.UpdateWith(this.currentToken);
				this.GetNextToken();
			}
			finally
			{
				this.blockType.RemoveAt(this.blockType.Count - 1);
			}
			return new Switch(context, ast, astlist);
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0003C674 File Offset: 0x0003B674
		private AST ParseThrowStatement()
		{
			Context context = this.currentToken.Clone();
			this.GetNextToken();
			AST ast = null;
			if (!this.scanner.GotEndOfLine() && JSToken.Semicolon != this.currentToken.token)
			{
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
				try
				{
					ast = this.ParseExpression();
				}
				catch (RecoveryTokenException ex)
				{
					ast = ex._partiallyComputedNode;
					if (this.IndexOfToken(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet, ex) == -1)
					{
						if (ast != null)
						{
							ex._partiallyComputedNode = new Throw(context, ex._partiallyComputedNode);
						}
						throw ex;
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
				}
			}
			if (ast != null)
			{
				context.UpdateWith(ast.context);
			}
			return new Throw(context, ast);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0003C740 File Offset: 0x0003B740
		private AST ParseTryStatement()
		{
			Context context = this.currentToken.Clone();
			Context context2 = null;
			AST ast = null;
			AST ast2 = null;
			AST ast3 = null;
			AST ast4 = null;
			RecoveryTokenException ex = null;
			TypeExpression typeExpression = null;
			this.blockType.Add(JSParser.BlockType.Block);
			try
			{
				bool flag = false;
				bool flag2 = false;
				this.GetNextToken();
				if (JSToken.LeftCurly != this.currentToken.token)
				{
					this.ReportError(JSError.NoLeftCurly);
				}
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_NoTrySkipTokenSet);
				try
				{
					try
					{
						ast = this.ParseBlock(out context2);
					}
					catch (RecoveryTokenException ex2)
					{
						if (this.IndexOfToken(NoSkipTokenSet.s_NoTrySkipTokenSet, ex2) == -1)
						{
							throw ex2;
						}
						ast = ex2._partiallyComputedNode;
					}
					goto IL_02E5;
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_NoTrySkipTokenSet);
				}
				IL_00A6:
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_NoTrySkipTokenSet);
				try
				{
					if (ast3 != null)
					{
						ast = new Try(context, ast, ast2, typeExpression, ast3, null, false, context2);
						ast2 = null;
						typeExpression = null;
						ast3 = null;
					}
					flag = true;
					this.GetNextToken();
					if (JSToken.LeftParen != this.currentToken.token)
					{
						this.ReportError(JSError.NoLeftParen);
					}
					this.GetNextToken();
					if (JSToken.Identifier != this.currentToken.token)
					{
						string text = JSKeyword.CanBeIdentifier(this.currentToken.token);
						if (text != null)
						{
							this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
							ast2 = new Lookup(text, this.currentToken.Clone());
						}
						else
						{
							this.ReportError(JSError.NoIdentifier);
							ast2 = new Lookup("##Exc##" + JSParser.s_cDummyName++, this.CurrentPositionContext());
						}
					}
					else
					{
						ast2 = new Lookup(this.scanner.GetIdentifier(), this.currentToken.Clone());
					}
					this.GetNextToken();
					this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
					try
					{
						if (JSToken.Colon == this.currentToken.token)
						{
							typeExpression = this.ParseTypeExpression();
						}
						else
						{
							if (flag2)
							{
								this.ForceReportInfo(ast2.context, JSError.UnreachableCatch);
							}
							flag2 = true;
						}
						if (JSToken.RightParen != this.currentToken.token)
						{
							this.ReportError(JSError.NoRightParen);
						}
						this.GetNextToken();
					}
					catch (RecoveryTokenException ex3)
					{
						if (this.IndexOfToken(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet, ex3) == -1)
						{
							ex3._partiallyComputedNode = null;
							throw ex3;
						}
						typeExpression = (TypeExpression)ex3._partiallyComputedNode;
						if (this.currentToken.token == JSToken.RightParen)
						{
							this.GetNextToken();
						}
					}
					finally
					{
						this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockConditionNoSkipTokenSet);
					}
					if (JSToken.LeftCurly != this.currentToken.token)
					{
						this.ReportError(JSError.NoLeftCurly);
					}
					ast3 = this.ParseBlock();
					context.UpdateWith(ast3.context);
				}
				catch (RecoveryTokenException ex4)
				{
					if (ex4._partiallyComputedNode == null)
					{
						ast3 = new Block(this.CurrentPositionContext());
					}
					else
					{
						ast3 = ex4._partiallyComputedNode;
					}
					if (this.IndexOfToken(NoSkipTokenSet.s_NoTrySkipTokenSet, ex4) == -1)
					{
						if (typeExpression != null)
						{
							ex4._partiallyComputedNode = new Try(context, ast, ast2, typeExpression, ast3, null, false, context2);
						}
						throw ex4;
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_NoTrySkipTokenSet);
				}
				IL_02E5:
				if (JSToken.Catch == this.currentToken.token)
				{
					goto IL_00A6;
				}
				try
				{
					if (JSToken.Finally == this.currentToken.token)
					{
						this.GetNextToken();
						this.blockType.Add(JSParser.BlockType.Finally);
						try
						{
							ast4 = this.ParseBlock();
							flag = true;
						}
						finally
						{
							this.blockType.RemoveAt(this.blockType.Count - 1);
						}
						context.UpdateWith(ast4.context);
					}
				}
				catch (RecoveryTokenException ex5)
				{
					ex = ex5;
				}
				if (!flag)
				{
					this.ReportError(JSError.NoCatch, true);
					ast4 = new Block(this.CurrentPositionContext());
				}
			}
			finally
			{
				this.blockType.RemoveAt(this.blockType.Count - 1);
			}
			bool flag3 = false;
			if (this.finallyEscaped > 0)
			{
				this.finallyEscaped--;
				flag3 = true;
			}
			if (ex != null)
			{
				ex._partiallyComputedNode = new Try(context, ast, ast2, typeExpression, ast3, ast4, flag3, context2);
				throw ex;
			}
			return new Try(context, ast, ast2, typeExpression, ast3, ast4, flag3, context2);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0003CC0C File Offset: 0x0003BC0C
		private AST ParseClass(FieldAttributes visibilitySpec, bool isStatic, Context classCtx, bool isAbstract, bool isFinal, CustomAttributeList customAttributes)
		{
			AST ast = null;
			AST ast2 = null;
			TypeExpression typeExpression = null;
			ArrayList arrayList = new ArrayList();
			bool flag = JSToken.Interface == this.currentToken.token;
			this.GetNextToken();
			if (JSToken.Identifier == this.currentToken.token)
			{
				ast = new IdentifierLiteral(this.scanner.GetIdentifier(), this.currentToken.Clone());
			}
			else
			{
				this.ReportError(JSError.NoIdentifier);
				if (JSToken.Extends != this.currentToken.token && JSToken.Implements != this.currentToken.token && JSToken.LeftCurly != this.currentToken.token)
				{
					this.SkipTokensAndThrow();
				}
				ast = new IdentifierLiteral("##Missing Class Name##" + JSParser.s_cDummyName++, this.CurrentPositionContext());
			}
			this.GetNextToken();
			if (JSToken.Extends == this.currentToken.token || JSToken.Implements == this.currentToken.token)
			{
				if (flag && JSToken.Extends == this.currentToken.token)
				{
					this.currentToken.token = JSToken.Implements;
				}
				if (JSToken.Extends == this.currentToken.token)
				{
					this.noSkipTokenSet.Add(NoSkipTokenSet.s_ClassExtendsNoSkipTokenSet);
					try
					{
						ast2 = this.ParseQualifiedIdentifier(JSError.NeedType);
					}
					catch (RecoveryTokenException ex)
					{
						if (this.IndexOfToken(NoSkipTokenSet.s_ClassExtendsNoSkipTokenSet, ex) == -1)
						{
							ex._partiallyComputedNode = null;
							throw ex;
						}
						ast2 = ex._partiallyComputedNode;
					}
					finally
					{
						this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ClassExtendsNoSkipTokenSet);
					}
				}
				if (JSToken.Implements == this.currentToken.token)
				{
					do
					{
						this.noSkipTokenSet.Add(NoSkipTokenSet.s_ClassImplementsNoSkipTokenSet);
						try
						{
							AST ast3 = this.ParseQualifiedIdentifier(JSError.NeedType);
							arrayList.Add(new TypeExpression(ast3));
						}
						catch (RecoveryTokenException ex2)
						{
							if (this.IndexOfToken(NoSkipTokenSet.s_ClassImplementsNoSkipTokenSet, ex2) == -1)
							{
								ex2._partiallyComputedNode = null;
								throw ex2;
							}
							if (ex2._partiallyComputedNode != null)
							{
								arrayList.Add(new TypeExpression(ex2._partiallyComputedNode));
							}
						}
						finally
						{
							this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ClassImplementsNoSkipTokenSet);
						}
					}
					while (JSToken.Comma == this.currentToken.token);
				}
			}
			if (ast2 != null)
			{
				typeExpression = new TypeExpression(ast2);
			}
			if (JSToken.LeftCurly != this.currentToken.token)
			{
				this.ReportError(JSError.NoLeftCurly);
			}
			ArrayList arrayList2 = this.blockType;
			this.blockType = new ArrayList(16);
			SimpleHashtable simpleHashtable = this.labelTable;
			this.labelTable = new SimpleHashtable(16U);
			this.Globals.ScopeStack.Push(new ClassScope(ast, ((IActivationObject)this.Globals.ScopeStack.Peek()).GetGlobalScope()));
			AST ast4;
			try
			{
				Block block = this.ParseClassBody(false, flag);
				classCtx.UpdateWith(block.context);
				TypeExpression[] array = new TypeExpression[arrayList.Count];
				arrayList.CopyTo(array);
				Class @class = new Class(classCtx, ast, typeExpression, array, block, visibilitySpec, isAbstract, isFinal, isStatic, flag, customAttributes);
				if (customAttributes != null)
				{
					customAttributes.SetTarget(@class);
				}
				ast4 = @class;
			}
			catch (RecoveryTokenException ex3)
			{
				classCtx.UpdateWith(ex3._partiallyComputedNode.context);
				TypeExpression[] array = new TypeExpression[arrayList.Count];
				arrayList.CopyTo(array);
				ex3._partiallyComputedNode = new Class(classCtx, ast, typeExpression, array, (Block)ex3._partiallyComputedNode, visibilitySpec, isAbstract, isFinal, isStatic, flag, customAttributes);
				if (customAttributes != null)
				{
					customAttributes.SetTarget(ex3._partiallyComputedNode);
				}
				throw ex3;
			}
			finally
			{
				this.Globals.ScopeStack.Pop();
				this.blockType = arrayList2;
				this.labelTable = simpleHashtable;
			}
			return ast4;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0003CFD4 File Offset: 0x0003BFD4
		private Block ParseClassBody(bool isEnum, bool isInterface)
		{
			this.blockType.Add(JSParser.BlockType.Block);
			Block block = new Block(this.currentToken.Clone());
			try
			{
				this.GetNextToken();
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
				JSToken[] array = null;
				if (isEnum)
				{
					array = NoSkipTokenSet.s_EnumBodyNoSkipTokenSet;
				}
				else if (isInterface)
				{
					array = NoSkipTokenSet.s_InterfaceBodyNoSkipTokenSet;
				}
				else
				{
					array = NoSkipTokenSet.s_ClassBodyNoSkipTokenSet;
				}
				try
				{
					while (JSToken.RightCurly != this.currentToken.token)
					{
						if (this.currentToken.token == JSToken.EndOfFile)
						{
							this.ReportError(JSError.NoRightCurly, true);
							this.SkipTokensAndThrow();
						}
						this.noSkipTokenSet.Add(array);
						try
						{
							AST ast = (isEnum ? this.ParseEnumMember() : this.ParseClassMember(isInterface));
							if (ast != null)
							{
								block.Append(ast);
							}
						}
						catch (RecoveryTokenException ex)
						{
							if (ex._partiallyComputedNode != null)
							{
								block.Append(ex._partiallyComputedNode);
							}
							if (this.IndexOfToken(array, ex) == -1)
							{
								ex._partiallyComputedNode = null;
								throw ex;
							}
						}
						finally
						{
							this.noSkipTokenSet.Remove(array);
						}
					}
				}
				catch (RecoveryTokenException ex2)
				{
					ex2._partiallyComputedNode = block;
					throw ex2;
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
				}
				block.context.UpdateWith(this.currentToken);
				this.GetNextToken();
			}
			finally
			{
				this.blockType.RemoveAt(this.blockType.Count - 1);
			}
			return block;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0003D1A4 File Offset: 0x0003C1A4
		private AST ParseClassMember(bool isInterface)
		{
			bool flag = false;
			if (isInterface && this.currentToken.token == JSToken.Public)
			{
				this.GetNextToken();
			}
			JSToken token = this.currentToken.token;
			if (token <= JSToken.Interface)
			{
				switch (token)
				{
				case JSToken.Import:
					this.ReportError(JSError.InvalidImport, true);
					try
					{
						this.ParseImportStatement();
					}
					catch (RecoveryTokenException)
					{
					}
					return null;
				case JSToken.With:
				case JSToken.Switch:
				case JSToken.Throw:
				case JSToken.Try:
				case JSToken.Event:
				case JSToken.LeftCurly:
				case JSToken.Null:
				case JSToken.True:
				case JSToken.False:
				case JSToken.This:
					break;
				case JSToken.Package:
				{
					Context context = this.currentToken.Clone();
					AST ast = this.ParsePackage(context);
					if (ast is Package)
					{
						this.ReportError(JSError.PackageInWrongContext, context, true);
					}
					return null;
				}
				case JSToken.Internal:
				case JSToken.Abstract:
				case JSToken.Public:
				case JSToken.Static:
				case JSToken.Private:
				case JSToken.Protected:
				case JSToken.Final:
					if (isInterface)
					{
						this.ReportError(JSError.BadModifierInInterface, true);
						this.GetNextToken();
						this.SkipTokensAndThrow();
					}
					return this.ParseAttributes(null, true, true, out flag);
				case JSToken.Var:
				case JSToken.Const:
					if (isInterface)
					{
						this.ReportError(JSError.VarIllegalInInterface, true);
						this.GetNextToken();
						this.SkipTokensAndThrow();
					}
					return this.ParseVariableStatement(FieldAttributes.PrivateScope, null, this.currentToken.token);
				case JSToken.Class:
					if (isInterface)
					{
						this.ReportError(JSError.SyntaxError, true);
						this.GetNextToken();
						this.SkipTokensAndThrow();
					}
					return this.ParseClass(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), false, false, null);
				case JSToken.Function:
					return this.ParseFunction(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), true, isInterface, false, isInterface, null);
				case JSToken.Semicolon:
					this.GetNextToken();
					return this.ParseClassMember(isInterface);
				case JSToken.Identifier:
				{
					if (isInterface)
					{
						this.ReportError(JSError.SyntaxError, true);
						this.GetNextToken();
						this.SkipTokensAndThrow();
					}
					bool flag2 = true;
					bool flag3;
					AST ast2 = this.ParseUnaryExpression(out flag3, ref flag2, false);
					if (flag2)
					{
						ast2 = this.ParseAttributes(ast2, true, true, out flag);
						if (flag)
						{
							return ast2;
						}
					}
					this.ReportError(JSError.SyntaxError, ast2.context.Clone(), true);
					this.SkipTokensAndThrow();
					return null;
				}
				default:
					if (token == JSToken.Interface)
					{
						if (isInterface)
						{
							this.ReportError(JSError.InterfaceIllegalInInterface, true);
							this.GetNextToken();
							this.SkipTokensAndThrow();
						}
						return this.ParseClass(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), false, false, null);
					}
					break;
				}
			}
			else
			{
				if (token == JSToken.RightCurly)
				{
					return null;
				}
				if (token == JSToken.Enum)
				{
					return this.ParseEnum(FieldAttributes.PrivateScope, this.currentToken.Clone(), null);
				}
			}
			this.ReportError(JSError.SyntaxError, true);
			this.GetNextToken();
			this.SkipTokensAndThrow();
			return null;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0003D430 File Offset: 0x0003C430
		private AST ParseEnum(FieldAttributes visibilitySpec, Context enumCtx, CustomAttributeList customAttributes)
		{
			IdentifierLiteral identifierLiteral = null;
			AST ast = null;
			TypeExpression typeExpression = null;
			this.GetNextToken();
			if (JSToken.Identifier == this.currentToken.token)
			{
				identifierLiteral = new IdentifierLiteral(this.scanner.GetIdentifier(), this.currentToken.Clone());
			}
			else
			{
				this.ReportError(JSError.NoIdentifier);
				if (JSToken.Colon != this.currentToken.token && JSToken.LeftCurly != this.currentToken.token)
				{
					this.SkipTokensAndThrow();
				}
				identifierLiteral = new IdentifierLiteral("##Missing Enum Name##" + JSParser.s_cDummyName++, this.CurrentPositionContext());
			}
			this.GetNextToken();
			if (JSToken.Colon == this.currentToken.token)
			{
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_EnumBaseTypeNoSkipTokenSet);
				try
				{
					ast = this.ParseQualifiedIdentifier(JSError.NeedType);
				}
				catch (RecoveryTokenException ex)
				{
					if (this.IndexOfToken(NoSkipTokenSet.s_ClassExtendsNoSkipTokenSet, ex) == -1)
					{
						ex._partiallyComputedNode = null;
						throw ex;
					}
					ast = ex._partiallyComputedNode;
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EnumBaseTypeNoSkipTokenSet);
				}
			}
			if (ast != null)
			{
				typeExpression = new TypeExpression(ast);
			}
			if (JSToken.LeftCurly != this.currentToken.token)
			{
				this.ReportError(JSError.NoLeftCurly);
			}
			ArrayList arrayList = this.blockType;
			this.blockType = new ArrayList(16);
			SimpleHashtable simpleHashtable = this.labelTable;
			this.labelTable = new SimpleHashtable(16U);
			this.Globals.ScopeStack.Push(new ClassScope(identifierLiteral, ((IActivationObject)this.Globals.ScopeStack.Peek()).GetGlobalScope()));
			AST ast2;
			try
			{
				Block block = this.ParseClassBody(true, false);
				enumCtx.UpdateWith(block.context);
				EnumDeclaration enumDeclaration = new EnumDeclaration(enumCtx, identifierLiteral, typeExpression, block, visibilitySpec, customAttributes);
				if (customAttributes != null)
				{
					customAttributes.SetTarget(enumDeclaration);
				}
				ast2 = enumDeclaration;
			}
			catch (RecoveryTokenException ex2)
			{
				enumCtx.UpdateWith(ex2._partiallyComputedNode.context);
				ex2._partiallyComputedNode = new EnumDeclaration(enumCtx, identifierLiteral, typeExpression, (Block)ex2._partiallyComputedNode, visibilitySpec, customAttributes);
				if (customAttributes != null)
				{
					customAttributes.SetTarget(ex2._partiallyComputedNode);
				}
				throw ex2;
			}
			finally
			{
				this.Globals.ScopeStack.Pop();
				this.blockType = arrayList;
				this.labelTable = simpleHashtable;
			}
			return ast2;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0003D68C File Offset: 0x0003C68C
		private AST ParseEnumMember()
		{
			AST ast = null;
			AST ast2 = null;
			JSToken token = this.currentToken.token;
			if (token == JSToken.Var)
			{
				this.ReportError(JSError.NoVarInEnum, true);
				this.GetNextToken();
				return this.ParseEnumMember();
			}
			if (token == JSToken.Semicolon)
			{
				this.GetNextToken();
				return this.ParseEnumMember();
			}
			if (token != JSToken.Identifier)
			{
				this.ReportError(JSError.SyntaxError, true);
				this.SkipTokensAndThrow();
				return ast;
			}
			Lookup lookup = new Lookup(this.currentToken.Clone());
			Context context = this.currentToken.Clone();
			this.GetNextToken();
			if (JSToken.Assign == this.currentToken.token)
			{
				this.GetNextToken();
				ast2 = this.ParseExpression(true);
			}
			if (JSToken.Comma == this.currentToken.token)
			{
				this.GetNextToken();
			}
			else if (JSToken.RightCurly != this.currentToken.token)
			{
				this.ReportError(JSError.NoComma, true);
			}
			return new Constant(context, lookup, null, ast2, FieldAttributes.Public, null);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0003D780 File Offset: 0x0003C780
		private bool GuessIfAbstract()
		{
			JSToken token = this.currentToken.token;
			if (token <= JSToken.Interface)
			{
				switch (token)
				{
				case JSToken.Package:
				case JSToken.Internal:
				case JSToken.Abstract:
				case JSToken.Public:
				case JSToken.Static:
				case JSToken.Private:
				case JSToken.Protected:
				case JSToken.Final:
				case JSToken.Const:
				case JSToken.Class:
				case JSToken.Function:
					break;
				case JSToken.Event:
				case JSToken.Var:
				case JSToken.LeftCurly:
					return false;
				case JSToken.Semicolon:
					this.GetNextToken();
					return true;
				default:
					if (token != JSToken.Interface)
					{
						return false;
					}
					break;
				}
			}
			else if (token != JSToken.RightCurly && token != JSToken.Enum)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0003D800 File Offset: 0x0003C800
		private AST ParseFunction(FieldAttributes visibilitySpec, bool inExpression, Context fncCtx, bool isMethod, bool isAbstract, bool isFinal, bool isInterface, CustomAttributeList customAttributes)
		{
			return this.ParseFunction(visibilitySpec, inExpression, fncCtx, isMethod, isAbstract, isFinal, isInterface, customAttributes, null);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0003D824 File Offset: 0x0003C824
		private AST ParseFunction(FieldAttributes visibilitySpec, bool inExpression, Context fncCtx, bool isMethod, bool isAbstract, bool isFinal, bool isInterface, CustomAttributeList customAttributes, Call function)
		{
			if (this.demandFullTrustOnFunctionCreation)
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			}
			IdentifierLiteral identifierLiteral = null;
			AST ast = null;
			ArrayList arrayList = null;
			TypeExpression typeExpression = null;
			Block block = null;
			bool flag = false;
			bool flag2 = false;
			if (function == null)
			{
				this.GetNextToken();
				if (isMethod)
				{
					if (JSToken.Get == this.currentToken.token)
					{
						flag = true;
						this.GetNextToken();
					}
					else if (JSToken.Set == this.currentToken.token)
					{
						flag2 = true;
						this.GetNextToken();
					}
				}
				if (JSToken.Identifier == this.currentToken.token)
				{
					identifierLiteral = new IdentifierLiteral(this.scanner.GetIdentifier(), this.currentToken.Clone());
					this.GetNextToken();
					if (JSToken.AccessField == this.currentToken.token)
					{
						if (isInterface)
						{
							this.ReportError(JSError.SyntaxError, true);
						}
						this.GetNextToken();
						if (JSToken.Identifier == this.currentToken.token)
						{
							ast = new Lookup(identifierLiteral.context);
							identifierLiteral = new IdentifierLiteral(this.scanner.GetIdentifier(), this.currentToken.Clone());
							this.GetNextToken();
							while (JSToken.AccessField == this.currentToken.token)
							{
								this.GetNextToken();
								if (JSToken.Identifier == this.currentToken.token)
								{
									ast = new Member(ast.context.CombineWith(this.currentToken), ast, new ConstantWrapper(identifierLiteral.ToString(), identifierLiteral.context));
									identifierLiteral = new IdentifierLiteral(this.scanner.GetIdentifier(), this.currentToken.Clone());
									this.GetNextToken();
								}
								else
								{
									this.ReportError(JSError.NoIdentifier, true);
								}
							}
						}
						else
						{
							this.ReportError(JSError.NoIdentifier, true);
						}
					}
				}
				else
				{
					string text = JSKeyword.CanBeIdentifier(this.currentToken.token);
					if (text != null)
					{
						this.ForceReportInfo(JSError.KeywordUsedAsIdentifier, isMethod);
						identifierLiteral = new IdentifierLiteral(text, this.currentToken.Clone());
						this.GetNextToken();
					}
					else
					{
						if (!inExpression)
						{
							text = this.currentToken.GetCode();
							this.ReportError(JSError.NoIdentifier, true);
							this.GetNextToken();
						}
						else
						{
							text = "";
						}
						identifierLiteral = new IdentifierLiteral(text, this.CurrentPositionContext());
					}
				}
			}
			else
			{
				identifierLiteral = function.GetName();
			}
			ArrayList arrayList2 = this.blockType;
			this.blockType = new ArrayList(16);
			SimpleHashtable simpleHashtable = this.labelTable;
			this.labelTable = new SimpleHashtable(16U);
			FunctionScope functionScope = new FunctionScope(this.Globals.ScopeStack.Peek(), isMethod);
			this.Globals.ScopeStack.Push(functionScope);
			try
			{
				arrayList = new ArrayList();
				Context context = null;
				if (function == null)
				{
					if (JSToken.LeftParen != this.currentToken.token)
					{
						this.ReportError(JSError.NoLeftParen);
					}
					this.GetNextToken();
					while (JSToken.RightParen != this.currentToken.token)
					{
						if (context != null)
						{
							this.ReportError(JSError.ParamListNotLast, context, true);
							context = null;
						}
						string text2 = null;
						TypeExpression typeExpression2 = null;
						this.noSkipTokenSet.Add(NoSkipTokenSet.s_FunctionDeclNoSkipTokenSet);
						try
						{
							if (JSToken.ParamArray == this.currentToken.token)
							{
								context = this.currentToken.Clone();
								this.GetNextToken();
							}
							if (JSToken.Identifier != this.currentToken.token && (text2 = JSKeyword.CanBeIdentifier(this.currentToken.token)) == null)
							{
								if (JSToken.LeftCurly == this.currentToken.token)
								{
									this.ReportError(JSError.NoRightParen);
									break;
								}
								if (JSToken.Comma == this.currentToken.token)
								{
									this.ReportError(JSError.SyntaxError, true);
								}
								else
								{
									this.ReportError(JSError.SyntaxError, true);
									this.SkipTokensAndThrow();
								}
							}
							else
							{
								if (text2 == null)
								{
									text2 = this.scanner.GetIdentifier();
								}
								else
								{
									this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
								}
								Context context2 = this.currentToken.Clone();
								this.GetNextToken();
								if (JSToken.Colon == this.currentToken.token)
								{
									typeExpression2 = this.ParseTypeExpression();
									if (typeExpression2 != null)
									{
										context2.UpdateWith(typeExpression2.context);
									}
								}
								CustomAttributeList customAttributeList = null;
								if (context != null)
								{
									customAttributeList = new CustomAttributeList(context);
									customAttributeList.Append(new CustomAttribute(context, new Lookup("...", context), new ASTList(null)));
								}
								arrayList.Add(new ParameterDeclaration(context2, text2, typeExpression2, customAttributeList));
							}
							if (JSToken.RightParen == this.currentToken.token)
							{
								break;
							}
							if (JSToken.Comma != this.currentToken.token)
							{
								if (JSToken.LeftCurly == this.currentToken.token)
								{
									this.ReportError(JSError.NoRightParen);
									break;
								}
								if (JSToken.Identifier == this.currentToken.token && typeExpression2 == null)
								{
									this.ReportError(JSError.NoCommaOrTypeDefinitionError);
								}
								else
								{
									this.ReportError(JSError.NoComma);
								}
							}
							this.GetNextToken();
						}
						catch (RecoveryTokenException ex)
						{
							if (this.IndexOfToken(NoSkipTokenSet.s_FunctionDeclNoSkipTokenSet, ex) == -1)
							{
								throw ex;
							}
						}
						finally
						{
							this.noSkipTokenSet.Remove(NoSkipTokenSet.s_FunctionDeclNoSkipTokenSet);
						}
					}
					fncCtx.UpdateWith(this.currentToken);
					if (flag && arrayList.Count != 0)
					{
						this.ReportError(JSError.BadPropertyDeclaration, true);
						flag = false;
					}
					else if (flag2 && arrayList.Count != 1)
					{
						this.ReportError(JSError.BadPropertyDeclaration, true);
						flag2 = false;
					}
					this.GetNextToken();
					if (JSToken.Colon == this.currentToken.token)
					{
						if (flag2)
						{
							this.ReportError(JSError.SyntaxError);
						}
						this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartBlockNoSkipTokenSet);
						try
						{
							typeExpression = this.ParseTypeExpression();
						}
						catch (RecoveryTokenException ex2)
						{
							if (this.IndexOfToken(NoSkipTokenSet.s_StartBlockNoSkipTokenSet, ex2) == -1)
							{
								ex2._partiallyComputedNode = null;
								throw ex2;
							}
							if (ex2._partiallyComputedNode != null)
							{
								typeExpression = (TypeExpression)ex2._partiallyComputedNode;
							}
						}
						finally
						{
							this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartBlockNoSkipTokenSet);
						}
						if (flag2)
						{
							typeExpression = null;
						}
					}
				}
				else
				{
					function.GetParameters(arrayList);
				}
				if (JSToken.LeftCurly != this.currentToken.token && (isAbstract || (isMethod && this.GuessIfAbstract())))
				{
					if (!isAbstract)
					{
						isAbstract = true;
						this.ReportError(JSError.ShouldBeAbstract, fncCtx, true);
					}
					block = new Block(this.currentToken.Clone());
				}
				else
				{
					if (JSToken.LeftCurly != this.currentToken.token)
					{
						this.ReportError(JSError.NoLeftCurly, true);
					}
					else if (isAbstract)
					{
						this.ReportError(JSError.AbstractWithBody, fncCtx, true);
					}
					this.blockType.Add(JSParser.BlockType.Block);
					this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
					this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
					try
					{
						block = new Block(this.currentToken.Clone());
						this.GetNextToken();
						while (JSToken.RightCurly != this.currentToken.token)
						{
							try
							{
								block.Append(this.ParseStatement());
							}
							catch (RecoveryTokenException ex3)
							{
								if (ex3._partiallyComputedNode != null)
								{
									block.Append(ex3._partiallyComputedNode);
								}
								if (this.IndexOfToken(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, ex3) == -1)
								{
									throw ex3;
								}
							}
						}
						block.context.UpdateWith(this.currentToken);
						fncCtx.UpdateWith(this.currentToken);
					}
					catch (RecoveryTokenException ex4)
					{
						if (this.IndexOfToken(NoSkipTokenSet.s_BlockNoSkipTokenSet, ex4) == -1)
						{
							this.Globals.ScopeStack.Pop();
							try
							{
								ParameterDeclaration[] array = new ParameterDeclaration[arrayList.Count];
								arrayList.CopyTo(array);
								if (inExpression)
								{
									ex4._partiallyComputedNode = new FunctionExpression(fncCtx, identifierLiteral, array, typeExpression, block, functionScope, visibilitySpec);
								}
								else
								{
									ex4._partiallyComputedNode = new FunctionDeclaration(fncCtx, ast, identifierLiteral, array, typeExpression, block, functionScope, visibilitySpec, isMethod, flag, flag2, isAbstract, isFinal, customAttributes);
								}
								if (customAttributes != null)
								{
									customAttributes.SetTarget(ex4._partiallyComputedNode);
								}
							}
							finally
							{
								this.Globals.ScopeStack.Push(functionScope);
							}
							throw ex4;
						}
					}
					finally
					{
						this.blockType.RemoveAt(this.blockType.Count - 1);
						this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
						this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
					}
					this.GetNextToken();
				}
			}
			finally
			{
				this.blockType = arrayList2;
				this.labelTable = simpleHashtable;
				this.Globals.ScopeStack.Pop();
			}
			ParameterDeclaration[] array2 = new ParameterDeclaration[arrayList.Count];
			arrayList.CopyTo(array2);
			AST ast2;
			if (inExpression)
			{
				ast2 = new FunctionExpression(fncCtx, identifierLiteral, array2, typeExpression, block, functionScope, visibilitySpec);
			}
			else
			{
				ast2 = new FunctionDeclaration(fncCtx, ast, identifierLiteral, array2, typeExpression, block, functionScope, visibilitySpec, isMethod, flag, flag2, isAbstract, isFinal, customAttributes);
			}
			if (customAttributes != null)
			{
				customAttributes.SetTarget(ast2);
			}
			return ast2;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0003E138 File Offset: 0x0003D138
		internal AST ParseFunctionExpression()
		{
			this.demandFullTrustOnFunctionCreation = true;
			this.GetNextToken();
			return this.ParseFunction(FieldAttributes.PrivateScope, true, this.currentToken.Clone(), false, false, false, false, null);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0003E16C File Offset: 0x0003D16C
		internal string[] ParseNamedBreakpoint(out int argNumber)
		{
			argNumber = 0;
			AST ast = this.ParseQualifiedIdentifier(JSError.SyntaxError);
			if (ast == null)
			{
				return null;
			}
			string[] array = new string[4];
			array[0] = ast.ToString();
			if (JSToken.LeftParen == this.currentToken.token)
			{
				array[1] = "";
				this.GetNextToken();
				while (JSToken.RightParen != this.currentToken.token)
				{
					string text = null;
					if (JSToken.Identifier != this.currentToken.token && (text = JSKeyword.CanBeIdentifier(this.currentToken.token)) == null)
					{
						return null;
					}
					if (text == null)
					{
						text = this.scanner.GetIdentifier();
					}
					AST ast2 = new Lookup(text, this.currentToken.Clone());
					this.GetNextToken();
					string text2;
					if (JSToken.AccessField == this.currentToken.token)
					{
						ast2 = this.ParseScopeSequence(ast2, JSError.SyntaxError);
						text2 = ast2.ToString();
						while (JSToken.LeftBracket == this.currentToken.token)
						{
							this.GetNextToken();
							if (JSToken.RightBracket != this.currentToken.token)
							{
								return null;
							}
							text2 += "[]";
							this.GetNextToken();
						}
					}
					else if (JSToken.Colon == this.currentToken.token)
					{
						this.GetNextToken();
						if (JSToken.RightParen == this.currentToken.token)
						{
							return null;
						}
						continue;
					}
					else
					{
						text2 = ast2.ToString();
					}
					string[] array2;
					(array2 = array)[1] = array2[1] + text2 + " ";
					argNumber++;
					if (JSToken.Comma == this.currentToken.token)
					{
						this.GetNextToken();
						if (JSToken.RightParen == this.currentToken.token)
						{
							return null;
						}
					}
				}
				this.GetNextToken();
				if (JSToken.Colon == this.currentToken.token)
				{
					this.GetNextToken();
					string text = null;
					if (JSToken.Identifier != this.currentToken.token && (text = JSKeyword.CanBeIdentifier(this.currentToken.token)) == null)
					{
						return null;
					}
					if (text == null)
					{
						text = this.scanner.GetIdentifier();
					}
					AST ast2 = new Lookup(text, this.currentToken.Clone());
					this.GetNextToken();
					if (JSToken.AccessField == this.currentToken.token)
					{
						ast2 = this.ParseScopeSequence(ast2, JSError.SyntaxError);
						array[2] = ast2.ToString();
						while (JSToken.LeftBracket == this.currentToken.token)
						{
							this.GetNextToken();
							if (JSToken.RightBracket != this.currentToken.token)
							{
								return null;
							}
							string[] array3;
							(array3 = array)[2] = array3[2] + "[]";
							this.GetNextToken();
						}
					}
					else
					{
						array[2] = ast2.ToString();
					}
				}
			}
			if (JSToken.FirstBinaryOp == this.currentToken.token)
			{
				this.GetNextToken();
				if (JSToken.IntegerLiteral != this.currentToken.token)
				{
					return null;
				}
				array[3] = this.currentToken.GetCode();
				this.GetNextToken();
			}
			if (this.currentToken.token != JSToken.EndOfFile)
			{
				return null;
			}
			return array;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0003E42C File Offset: 0x0003D42C
		private AST ParsePackage(Context packageContext)
		{
			this.GetNextToken();
			AST ast = null;
			bool flag = this.scanner.GotEndOfLine();
			if (JSToken.Identifier != this.currentToken.token)
			{
				if (JSScanner.CanParseAsExpression(this.currentToken.token))
				{
					this.ReportError(JSError.KeywordUsedAsIdentifier, packageContext.Clone(), true);
					ast = new Lookup("package", packageContext);
					ast = this.MemberExpression(ast, null);
					bool flag2;
					ast = this.ParsePostfixExpression(ast, out flag2);
					ast = this.ParseExpression(ast, false, flag2, JSToken.None);
					return new Expression(ast.context.Clone(), ast);
				}
				if (flag)
				{
					this.ReportError(JSError.KeywordUsedAsIdentifier, packageContext.Clone(), true);
					return new Lookup("package", packageContext);
				}
				if (JSToken.Increment == this.currentToken.token || JSToken.Decrement == this.currentToken.token)
				{
					this.ReportError(JSError.KeywordUsedAsIdentifier, packageContext.Clone(), true);
					ast = new Lookup("package", packageContext);
					bool flag3;
					ast = this.ParsePostfixExpression(ast, out flag3);
					ast = this.ParseExpression(ast, false, false, JSToken.None);
					return new Expression(ast.context.Clone(), ast);
				}
			}
			else
			{
				this.errorToken = this.currentToken;
				ast = this.ParseQualifiedIdentifier(JSError.NoIdentifier);
			}
			Context context = null;
			if (JSToken.LeftCurly != this.currentToken.token && ast == null)
			{
				context = this.currentToken.Clone();
				this.GetNextToken();
			}
			if (JSToken.LeftCurly == this.currentToken.token)
			{
				if (ast == null)
				{
					if (context == null)
					{
						context = this.currentToken.Clone();
					}
					this.ReportError(JSError.NoIdentifier, context, true);
				}
			}
			else if (ast == null)
			{
				this.ReportError(JSError.SyntaxError, packageContext);
				if (JSScanner.CanStartStatement(context.token))
				{
					this.currentToken = context;
					return this.ParseStatement();
				}
				if (JSScanner.CanStartStatement(this.currentToken.token))
				{
					this.errorToken = null;
					return this.ParseStatement();
				}
				this.ReportError(JSError.SyntaxError);
				this.SkipTokensAndThrow();
			}
			else
			{
				if (flag)
				{
					this.ReportError(JSError.KeywordUsedAsIdentifier, packageContext.Clone(), true);
					Block block = new Block(packageContext.Clone());
					block.Append(new Lookup("package", packageContext));
					ast = this.MemberExpression(ast, null);
					bool flag4;
					ast = this.ParsePostfixExpression(ast, out flag4);
					ast = this.ParseExpression(ast, false, true, JSToken.None);
					block.Append(new Expression(ast.context.Clone(), ast));
					block.context.UpdateWith(ast.context);
					return block;
				}
				this.ReportError(JSError.NoLeftCurly);
			}
			PackageScope packageScope = new PackageScope(this.Globals.ScopeStack.Peek());
			this.Globals.ScopeStack.Push(packageScope);
			AST ast4;
			try
			{
				string text = ((ast != null) ? ast.ToString() : "anonymous package");
				packageScope.name = text;
				packageContext.UpdateWith(this.currentToken);
				ASTList astlist = new ASTList(packageContext);
				this.GetNextToken();
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_PackageBodyNoSkipTokenSet);
				try
				{
					while (this.currentToken.token != JSToken.RightCurly)
					{
						AST ast2 = null;
						try
						{
							JSToken token = this.currentToken.token;
							if (token <= JSToken.Semicolon)
							{
								if (token == JSToken.EndOfFile)
								{
									this.EOFError(JSError.ErrEOF);
									throw new EndOfFile();
								}
								switch (token)
								{
								case JSToken.Import:
									this.ReportError(JSError.InvalidImport, true);
									try
									{
										this.ParseImportStatement();
										continue;
									}
									catch (RecoveryTokenException)
									{
										continue;
									}
									break;
								case JSToken.With:
								case JSToken.Switch:
								case JSToken.Throw:
								case JSToken.Try:
								case JSToken.Event:
								case JSToken.Var:
								case JSToken.Const:
								case JSToken.Function:
								case JSToken.LeftCurly:
									goto IL_04D9;
								case JSToken.Package:
									break;
								case JSToken.Internal:
								case JSToken.Abstract:
								case JSToken.Public:
								case JSToken.Static:
								case JSToken.Private:
								case JSToken.Protected:
								case JSToken.Final:
								{
									bool flag5;
									ast2 = this.ParseAttributes(null, true, false, out flag5);
									if (flag5 && ast2 is Class)
									{
										astlist.Append(ast2);
										continue;
									}
									this.ReportError(JSError.OnlyClassesAllowed, ast2.context.Clone(), true);
									this.SkipTokensAndThrow();
									continue;
								}
								case JSToken.Class:
									goto IL_0388;
								case JSToken.Semicolon:
									this.GetNextToken();
									continue;
								default:
									goto IL_04D9;
								}
								Context context2 = this.currentToken.Clone();
								AST ast3 = this.ParsePackage(context2);
								if (ast3 is Package)
								{
									this.ReportError(JSError.PackageInWrongContext, context2, true);
									continue;
								}
								continue;
							}
							else
							{
								if (token == JSToken.Identifier)
								{
									bool flag6 = true;
									bool flag7;
									ast2 = this.ParseUnaryExpression(out flag7, ref flag6, false);
									if (flag6)
									{
										bool flag8;
										ast2 = this.ParseAttributes(ast2, true, false, out flag8);
										if (flag8 && ast2 is Class)
										{
											astlist.Append(ast2);
											continue;
										}
									}
									this.ReportError(JSError.OnlyClassesAllowed, ast2.context.Clone(), true);
									this.SkipTokensAndThrow();
									continue;
								}
								if (token != JSToken.Interface)
								{
									if (token != JSToken.Enum)
									{
										goto IL_04D9;
									}
									astlist.Append(this.ParseEnum(FieldAttributes.PrivateScope, this.currentToken.Clone(), null));
									continue;
								}
							}
							IL_0388:
							astlist.Append(this.ParseClass(FieldAttributes.PrivateScope, false, this.currentToken.Clone(), false, false, null));
							continue;
							IL_04D9:
							this.ReportError(JSError.OnlyClassesAllowed, (ast2 != null) ? ast2.context.Clone() : this.CurrentPositionContext(), true);
							this.SkipTokensAndThrow();
						}
						catch (RecoveryTokenException ex)
						{
							if (ex._partiallyComputedNode != null && ex._partiallyComputedNode is Class)
							{
								astlist.Append((Class)ex._partiallyComputedNode);
								ex._partiallyComputedNode = null;
							}
							if (this.IndexOfToken(NoSkipTokenSet.s_PackageBodyNoSkipTokenSet, ex) == -1)
							{
								throw ex;
							}
						}
					}
				}
				catch (RecoveryTokenException ex2)
				{
					if (this.IndexOfToken(NoSkipTokenSet.s_BlockNoSkipTokenSet, ex2) == -1)
					{
						this.ReportError(JSError.NoRightCurly, this.CurrentPositionContext());
						ex2._partiallyComputedNode = new Package(text, ast, astlist, packageContext);
						throw ex2;
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_PackageBodyNoSkipTokenSet);
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
				}
				this.GetNextToken();
				ast4 = new Package(text, ast, astlist, packageContext);
			}
			finally
			{
				this.Globals.ScopeStack.Pop();
			}
			return ast4;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0003EAA0 File Offset: 0x0003DAA0
		private AST ParseStaticInitializer(Context initContext)
		{
			if (this.demandFullTrustOnFunctionCreation)
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			}
			Block block = null;
			FunctionScope functionScope = new FunctionScope(this.Globals.ScopeStack.Peek());
			functionScope.isStatic = true;
			ArrayList arrayList = this.blockType;
			this.blockType = new ArrayList(16);
			SimpleHashtable simpleHashtable = this.labelTable;
			this.labelTable = new SimpleHashtable(16U);
			this.blockType.Add(JSParser.BlockType.Block);
			this.noSkipTokenSet.Add(NoSkipTokenSet.s_BlockNoSkipTokenSet);
			this.noSkipTokenSet.Add(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
			try
			{
				this.Globals.ScopeStack.Push(functionScope);
				block = new Block(this.currentToken.Clone());
				this.GetNextToken();
				while (JSToken.RightCurly != this.currentToken.token)
				{
					try
					{
						block.Append(this.ParseStatement());
					}
					catch (RecoveryTokenException ex)
					{
						if (ex._partiallyComputedNode != null)
						{
							block.Append(ex._partiallyComputedNode);
						}
						if (this.IndexOfToken(NoSkipTokenSet.s_StartStatementNoSkipTokenSet, ex) == -1)
						{
							throw ex;
						}
					}
				}
			}
			catch (RecoveryTokenException ex2)
			{
				if (this.IndexOfToken(NoSkipTokenSet.s_BlockNoSkipTokenSet, ex2) == -1)
				{
					ex2._partiallyComputedNode = new StaticInitializer(initContext, block, functionScope);
					throw ex2;
				}
			}
			finally
			{
				this.noSkipTokenSet.Remove(NoSkipTokenSet.s_StartStatementNoSkipTokenSet);
				this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BlockNoSkipTokenSet);
				this.blockType = arrayList;
				this.labelTable = simpleHashtable;
				this.Globals.ScopeStack.Pop();
			}
			block.context.UpdateWith(this.currentToken);
			initContext.UpdateWith(this.currentToken);
			this.GetNextToken();
			return new StaticInitializer(initContext, block, functionScope);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0003EC6C File Offset: 0x0003DC6C
		private AST ParseExpression()
		{
			bool flag;
			AST ast = this.ParseUnaryExpression(out flag, false);
			return this.ParseExpression(ast, false, flag, JSToken.None);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0003EC90 File Offset: 0x0003DC90
		private AST ParseExpression(bool single)
		{
			bool flag;
			AST ast = this.ParseUnaryExpression(out flag, false);
			return this.ParseExpression(ast, single, flag, JSToken.None);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0003ECB4 File Offset: 0x0003DCB4
		private AST ParseExpression(bool single, JSToken inToken)
		{
			bool flag;
			AST ast = this.ParseUnaryExpression(out flag, false);
			return this.ParseExpression(ast, single, flag, inToken);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0003ECD8 File Offset: 0x0003DCD8
		private AST ParseExpression(AST leftHandSide, bool single, bool bCanAssign, JSToken inToken)
		{
			OpListItem opListItem = new OpListItem(JSToken.None, OpPrec.precNone, null);
			AstListItem astListItem = new AstListItem(leftHandSide, null);
			AST term2;
			try
			{
				while (JSScanner.IsProcessableOperator(this.currentToken.token) && inToken != this.currentToken.token)
				{
					OpPrec operatorPrecedence = JSScanner.GetOperatorPrecedence(this.currentToken.token);
					bool flag = JSScanner.IsRightAssociativeOperator(this.currentToken.token);
					while (operatorPrecedence < opListItem._prec || (operatorPrecedence == opListItem._prec && !flag))
					{
						AST ast = this.CreateExpressionNode(opListItem._operator, astListItem._prev._term, astListItem._term);
						opListItem = opListItem._prev;
						astListItem = astListItem._prev._prev;
						astListItem = new AstListItem(ast, astListItem);
					}
					if (JSToken.ConditionalIf == this.currentToken.token)
					{
						AST term = astListItem._term;
						astListItem = astListItem._prev;
						this.GetNextToken();
						AST ast2 = this.ParseExpression(true);
						if (JSToken.Colon != this.currentToken.token)
						{
							this.ReportError(JSError.NoColon);
						}
						this.GetNextToken();
						AST ast3 = this.ParseExpression(true, inToken);
						AST ast = new Conditional(term.context.CombineWith(ast3.context), term, ast2, ast3);
						astListItem = new AstListItem(ast, astListItem);
					}
					else
					{
						if (JSScanner.IsAssignmentOperator(this.currentToken.token))
						{
							if (!bCanAssign)
							{
								this.ReportError(JSError.IllegalAssignment);
								this.SkipTokensAndThrow();
							}
						}
						else
						{
							bCanAssign = false;
						}
						opListItem = new OpListItem(this.currentToken.token, operatorPrecedence, opListItem);
						this.GetNextToken();
						if (bCanAssign)
						{
							astListItem = new AstListItem(this.ParseUnaryExpression(out bCanAssign, false), astListItem);
						}
						else
						{
							bool flag2;
							astListItem = new AstListItem(this.ParseUnaryExpression(out flag2, false), astListItem);
							flag2 = flag2;
						}
					}
				}
				while (opListItem._operator != JSToken.None)
				{
					AST ast = this.CreateExpressionNode(opListItem._operator, astListItem._prev._term, astListItem._term);
					opListItem = opListItem._prev;
					astListItem = astListItem._prev._prev;
					astListItem = new AstListItem(ast, astListItem);
				}
				if (!single && JSToken.Comma == this.currentToken.token)
				{
					this.GetNextToken();
					AST ast4 = this.ParseExpression(false, inToken);
					astListItem._term = new Comma(astListItem._term.context.CombineWith(ast4.context), astListItem._term, ast4);
				}
				term2 = astListItem._term;
			}
			catch (RecoveryTokenException ex)
			{
				ex._partiallyComputedNode = leftHandSide;
				throw ex;
			}
			return term2;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0003EF50 File Offset: 0x0003DF50
		private AST ParseUnaryExpression(out bool isLeftHandSideExpr, bool isMinus)
		{
			bool flag = false;
			return this.ParseUnaryExpression(out isLeftHandSideExpr, ref flag, isMinus, false);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0003EF6A File Offset: 0x0003DF6A
		private AST ParseUnaryExpression(out bool isLeftHandSideExpr, ref bool canBeAttribute, bool isMinus)
		{
			return this.ParseUnaryExpression(out isLeftHandSideExpr, ref canBeAttribute, isMinus, true);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0003EF78 File Offset: 0x0003DF78
		private AST ParseUnaryExpression(out bool isLeftHandSideExpr, ref bool canBeAttribute, bool isMinus, bool warnForKeyword)
		{
			AST ast = null;
			isLeftHandSideExpr = false;
			bool flag = false;
			switch (this.currentToken.token)
			{
			case JSToken.FirstOp:
			{
				Context context = this.currentToken.Clone();
				this.GetNextToken();
				canBeAttribute = false;
				AST ast2 = this.ParseUnaryExpression(out flag, ref canBeAttribute, false);
				context.UpdateWith(ast2.context);
				ast = new NumericUnary(context, ast2, JSToken.FirstOp);
				break;
			}
			case JSToken.BitwiseNot:
			{
				Context context = this.currentToken.Clone();
				this.GetNextToken();
				canBeAttribute = false;
				AST ast2 = this.ParseUnaryExpression(out flag, ref canBeAttribute, false);
				context.UpdateWith(ast2.context);
				ast = new NumericUnary(context, ast2, JSToken.BitwiseNot);
				break;
			}
			case JSToken.Delete:
			{
				Context context = this.currentToken.Clone();
				this.GetNextToken();
				canBeAttribute = false;
				AST ast2 = this.ParseUnaryExpression(out flag, ref canBeAttribute, false);
				context.UpdateWith(ast2.context);
				ast = new Delete(context, ast2);
				break;
			}
			case JSToken.Void:
			{
				Context context = this.currentToken.Clone();
				this.GetNextToken();
				canBeAttribute = false;
				AST ast2 = this.ParseUnaryExpression(out flag, ref canBeAttribute, false);
				context.UpdateWith(ast2.context);
				ast = new VoidOp(context, ast2);
				break;
			}
			case JSToken.Typeof:
			{
				Context context = this.currentToken.Clone();
				this.GetNextToken();
				canBeAttribute = false;
				AST ast2 = this.ParseUnaryExpression(out flag, ref canBeAttribute, false);
				context.UpdateWith(ast2.context);
				ast = new Typeof(context, ast2);
				break;
			}
			case JSToken.Increment:
			{
				Context context = this.currentToken.Clone();
				this.GetNextToken();
				canBeAttribute = false;
				AST ast2 = this.ParseUnaryExpression(out flag, ref canBeAttribute, false);
				context.UpdateWith(ast2.context);
				ast = new PostOrPrefixOperator(context, ast2, PostOrPrefix.PrefixIncrement);
				break;
			}
			case JSToken.Decrement:
			{
				Context context = this.currentToken.Clone();
				this.GetNextToken();
				canBeAttribute = false;
				AST ast2 = this.ParseUnaryExpression(out flag, ref canBeAttribute, false);
				context.UpdateWith(ast2.context);
				ast = new PostOrPrefixOperator(context, ast2, PostOrPrefix.PrefixDecrement);
				break;
			}
			case JSToken.FirstBinaryOp:
			{
				Context context = this.currentToken.Clone();
				this.GetNextToken();
				canBeAttribute = false;
				AST ast2 = this.ParseUnaryExpression(out flag, ref canBeAttribute, false);
				context.UpdateWith(ast2.context);
				ast = new NumericUnary(context, ast2, JSToken.FirstBinaryOp);
				break;
			}
			case JSToken.Minus:
			{
				Context context = this.currentToken.Clone();
				this.GetNextToken();
				canBeAttribute = false;
				JSToken token = this.currentToken.token;
				AST ast2 = this.ParseUnaryExpression(out flag, ref canBeAttribute, true);
				if (token == JSToken.NumericLiteral)
				{
					context.UpdateWith(ast2.context);
					ast2.context = context;
					ast = ast2;
				}
				else
				{
					context.UpdateWith(ast2.context);
					ast = new NumericUnary(context, ast2, JSToken.Minus);
				}
				break;
			}
			default:
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_PostfixExpressionNoSkipTokenSet);
				try
				{
					ast = this.ParseLeftHandSideExpression(isMinus, ref canBeAttribute, warnForKeyword);
				}
				catch (RecoveryTokenException ex)
				{
					if (this.IndexOfToken(NoSkipTokenSet.s_PostfixExpressionNoSkipTokenSet, ex) == -1)
					{
						throw ex;
					}
					if (ex._partiallyComputedNode == null)
					{
						this.SkipTokensAndThrow();
					}
					else
					{
						ast = ex._partiallyComputedNode;
					}
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_PostfixExpressionNoSkipTokenSet);
				}
				ast = this.ParsePostfixExpression(ast, out isLeftHandSideExpr, ref canBeAttribute);
				break;
			}
			flag = flag;
			return ast;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0003F290 File Offset: 0x0003E290
		private AST ParsePostfixExpression(AST ast, out bool isLeftHandSideExpr)
		{
			bool flag = false;
			return this.ParsePostfixExpression(ast, out isLeftHandSideExpr, ref flag);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0003F2AC File Offset: 0x0003E2AC
		private AST ParsePostfixExpression(AST ast, out bool isLeftHandSideExpr, ref bool canBeAttribute)
		{
			isLeftHandSideExpr = true;
			if (ast != null && !this.scanner.GotEndOfLine())
			{
				if (JSToken.Increment == this.currentToken.token)
				{
					isLeftHandSideExpr = false;
					Context context = ast.context.Clone();
					context.UpdateWith(this.currentToken);
					canBeAttribute = false;
					ast = new PostOrPrefixOperator(context, ast, PostOrPrefix.PostfixIncrement);
					this.GetNextToken();
				}
				else if (JSToken.Decrement == this.currentToken.token)
				{
					isLeftHandSideExpr = false;
					Context context = ast.context.Clone();
					context.UpdateWith(this.currentToken);
					canBeAttribute = false;
					ast = new PostOrPrefixOperator(context, ast, PostOrPrefix.PostfixDecrement);
					this.GetNextToken();
				}
			}
			return ast;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0003F34E File Offset: 0x0003E34E
		private AST ParseLeftHandSideExpression()
		{
			return this.ParseLeftHandSideExpression(false);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0003F358 File Offset: 0x0003E358
		private AST ParseLeftHandSideExpression(bool isMinus)
		{
			bool flag = false;
			return this.ParseLeftHandSideExpression(isMinus, ref flag, false);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0003F374 File Offset: 0x0003E374
		private AST ParseLeftHandSideExpression(bool isMinus, ref bool canBeAttribute, bool warnForKeyword)
		{
			AST ast = null;
			bool flag = false;
			ArrayList arrayList = null;
			while (JSToken.New == this.currentToken.token)
			{
				if (arrayList == null)
				{
					arrayList = new ArrayList(4);
				}
				arrayList.Add(this.currentToken.Clone());
				this.GetNextToken();
			}
			JSToken token = this.currentToken.token;
			JSToken jstoken = token;
			if (jstoken <= JSToken.Divide)
			{
				switch (jstoken)
				{
				case JSToken.Function:
					canBeAttribute = false;
					ast = this.ParseFunction(FieldAttributes.PrivateScope, true, this.currentToken.Clone(), false, false, false, false, null);
					flag = true;
					goto IL_0956;
				case JSToken.LeftCurly:
				{
					canBeAttribute = false;
					Context context = this.currentToken.Clone();
					this.GetNextToken();
					ASTList astlist = new ASTList(this.currentToken.Clone());
					if (JSToken.RightCurly != this.currentToken.token)
					{
						for (;;)
						{
							AST ast2 = null;
							if (JSToken.Identifier == this.currentToken.token)
							{
								ast2 = new ConstantWrapper(this.scanner.GetIdentifier(), this.currentToken.Clone());
							}
							else if (JSToken.StringLiteral == this.currentToken.token)
							{
								ast2 = new ConstantWrapper(this.scanner.GetStringLiteral(), this.currentToken.Clone());
							}
							else if (JSToken.IntegerLiteral == this.currentToken.token || JSToken.NumericLiteral == this.currentToken.token)
							{
								string code = this.currentToken.GetCode();
								double num = Convert.ToNumber(code, true, true, Missing.Value);
								ast2 = new ConstantWrapper(num, this.currentToken.Clone());
								((ConstantWrapper)ast2).isNumericLiteral = true;
							}
							else
							{
								this.ReportError(JSError.NoMemberIdentifier);
								ast2 = new IdentifierLiteral("_#Missing_Field#_" + JSParser.s_cDummyName++, this.CurrentPositionContext());
							}
							ASTList astlist2 = new ASTList(ast2.context.Clone());
							this.GetNextToken();
							this.noSkipTokenSet.Add(NoSkipTokenSet.s_ObjectInitNoSkipTokenSet);
							try
							{
								try
								{
									AST ast3;
									if (JSToken.Colon != this.currentToken.token)
									{
										this.ReportError(JSError.NoColon, true);
										ast3 = this.ParseExpression(true);
									}
									else
									{
										this.GetNextToken();
										ast3 = this.ParseExpression(true);
									}
									astlist2.Append(ast2);
									astlist2.Append(ast3);
									astlist.Append(astlist2);
									if (JSToken.RightCurly == this.currentToken.token)
									{
										break;
									}
									if (JSToken.Comma == this.currentToken.token)
									{
										this.GetNextToken();
									}
									else
									{
										if (this.scanner.GotEndOfLine())
										{
											this.ReportError(JSError.NoRightCurly);
										}
										else
										{
											this.ReportError(JSError.NoComma, true);
										}
										this.SkipTokensAndThrow();
									}
								}
								catch (RecoveryTokenException ex)
								{
									if (ex._partiallyComputedNode != null)
									{
										AST ast3 = ex._partiallyComputedNode;
										astlist2.Append(ast2);
										astlist2.Append(ast3);
										astlist.Append(astlist2);
									}
									if (this.IndexOfToken(NoSkipTokenSet.s_ObjectInitNoSkipTokenSet, ex) == -1)
									{
										ex._partiallyComputedNode = new ObjectLiteral(context, astlist);
										throw ex;
									}
									if (JSToken.Comma == this.currentToken.token)
									{
										this.GetNextToken();
									}
									if (JSToken.RightCurly == this.currentToken.token)
									{
										break;
									}
								}
								continue;
							}
							finally
							{
								this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ObjectInitNoSkipTokenSet);
							}
							break;
						}
					}
					astlist.context.UpdateWith(this.currentToken);
					context.UpdateWith(this.currentToken);
					ast = new ObjectLiteral(context, astlist);
					goto IL_0956;
				}
				case JSToken.Semicolon:
					break;
				case JSToken.Null:
					canBeAttribute = false;
					ast = new NullLiteral(this.currentToken.Clone());
					goto IL_0956;
				case JSToken.True:
					canBeAttribute = false;
					ast = new ConstantWrapper(true, this.currentToken.Clone());
					goto IL_0956;
				case JSToken.False:
					canBeAttribute = false;
					ast = new ConstantWrapper(false, this.currentToken.Clone());
					goto IL_0956;
				case JSToken.This:
					canBeAttribute = false;
					ast = new ThisLiteral(this.currentToken.Clone(), false);
					goto IL_0956;
				case JSToken.Identifier:
					ast = new Lookup(this.scanner.GetIdentifier(), this.currentToken.Clone());
					goto IL_0956;
				case JSToken.StringLiteral:
					canBeAttribute = false;
					ast = new ConstantWrapper(this.scanner.GetStringLiteral(), this.currentToken.Clone());
					goto IL_0956;
				case JSToken.IntegerLiteral:
				{
					canBeAttribute = false;
					string code2 = this.currentToken.GetCode();
					object obj = Convert.LiteralToNumber(code2, this.currentToken);
					if (obj == null)
					{
						obj = 0;
					}
					ast = new ConstantWrapper(obj, this.currentToken.Clone());
					((ConstantWrapper)ast).isNumericLiteral = true;
					goto IL_0956;
				}
				case JSToken.NumericLiteral:
				{
					canBeAttribute = false;
					string text = (isMinus ? ("-" + this.currentToken.GetCode()) : this.currentToken.GetCode());
					double num2 = Convert.ToNumber(text, false, false, Missing.Value);
					ast = new ConstantWrapper(num2, this.currentToken.Clone());
					((ConstantWrapper)ast).isNumericLiteral = true;
					goto IL_0956;
				}
				case JSToken.LeftParen:
					canBeAttribute = false;
					this.GetNextToken();
					this.noSkipTokenSet.Add(NoSkipTokenSet.s_ParenExpressionNoSkipToken);
					try
					{
						ast = this.ParseExpression();
						if (JSToken.RightParen != this.currentToken.token)
						{
							this.ReportError(JSError.NoRightParen);
						}
					}
					catch (RecoveryTokenException ex2)
					{
						if (this.IndexOfToken(NoSkipTokenSet.s_ParenExpressionNoSkipToken, ex2) == -1)
						{
							throw ex2;
						}
						ast = ex2._partiallyComputedNode;
					}
					finally
					{
						this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ParenExpressionNoSkipToken);
					}
					if (ast == null)
					{
						this.SkipTokensAndThrow();
						goto IL_0956;
					}
					goto IL_0956;
				case JSToken.LeftBracket:
				{
					canBeAttribute = false;
					Context context2 = this.currentToken.Clone();
					ASTList astlist3 = new ASTList(this.currentToken.Clone());
					this.GetNextToken();
					if (this.currentToken.token != JSToken.Identifier || this.scanner.PeekToken() != JSToken.Colon)
					{
						goto IL_0561;
					}
					this.noSkipTokenSet.Add(NoSkipTokenSet.s_BracketToken);
					try
					{
						try
						{
							if (this.currentToken.GetCode() == "assembly")
							{
								this.GetNextToken();
								this.GetNextToken();
								return new AssemblyCustomAttributeList(this.ParseCustomAttributeList());
							}
							this.ReportError(JSError.ExpectedAssembly);
							this.SkipTokensAndThrow();
						}
						catch (RecoveryTokenException ex3)
						{
							ex3._partiallyComputedNode = new Block(context2);
							return ex3._partiallyComputedNode;
						}
						goto IL_0561;
					}
					finally
					{
						if (this.currentToken.token == JSToken.RightBracket)
						{
							this.errorToken = null;
							this.GetNextToken();
						}
						this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BracketToken);
					}
					IL_046D:
					if (JSToken.Comma != this.currentToken.token)
					{
						this.noSkipTokenSet.Add(NoSkipTokenSet.s_ArrayInitNoSkipTokenSet);
						try
						{
							try
							{
								astlist3.Append(this.ParseExpression(true));
								if (JSToken.Comma != this.currentToken.token)
								{
									if (JSToken.RightBracket != this.currentToken.token)
									{
										this.ReportError(JSError.NoRightBracket);
									}
									goto IL_0573;
								}
							}
							catch (RecoveryTokenException ex4)
							{
								if (ex4._partiallyComputedNode != null)
								{
									astlist3.Append(ex4._partiallyComputedNode);
								}
								if (this.IndexOfToken(NoSkipTokenSet.s_ArrayInitNoSkipTokenSet, ex4) == -1)
								{
									context2.UpdateWith(this.CurrentPositionContext());
									ex4._partiallyComputedNode = new ArrayLiteral(context2, astlist3);
									throw ex4;
								}
								if (JSToken.RightBracket == this.currentToken.token)
								{
									goto IL_0573;
								}
							}
							goto IL_055B;
						}
						finally
						{
							this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ArrayInitNoSkipTokenSet);
						}
					}
					astlist3.Append(new ConstantWrapper(Missing.Value, this.currentToken.Clone()));
					IL_055B:
					this.GetNextToken();
					IL_0561:
					if (JSToken.RightBracket != this.currentToken.token)
					{
						goto IL_046D;
					}
					IL_0573:
					context2.UpdateWith(this.currentToken);
					ast = new ArrayLiteral(context2, astlist3);
					goto IL_0956;
				}
				default:
					if (jstoken == JSToken.Divide)
					{
						canBeAttribute = false;
						string text2 = this.scanner.ScanRegExp();
						if (text2 != null)
						{
							bool flag2 = false;
							try
							{
								new Regex(text2, RegexOptions.ECMAScript);
							}
							catch (ArgumentException)
							{
								text2 = "";
								flag2 = true;
							}
							string text3 = this.scanner.ScanRegExpFlags();
							if (text3 == null)
							{
								ast = new RegExpLiteral(text2, null, this.currentToken.Clone());
							}
							else
							{
								try
								{
									ast = new RegExpLiteral(text2, text3, this.currentToken.Clone());
								}
								catch (JScriptException)
								{
									ast = new RegExpLiteral(text2, null, this.currentToken.Clone());
									flag2 = true;
								}
							}
							if (flag2)
							{
								this.ReportError(JSError.RegExpSyntax, true);
								goto IL_0956;
							}
							goto IL_0956;
						}
					}
					break;
				}
			}
			else
			{
				if (jstoken == JSToken.Super)
				{
					canBeAttribute = false;
					ast = new ThisLiteral(this.currentToken.Clone(), true);
					goto IL_0956;
				}
				if (jstoken == JSToken.PreProcessorConstant)
				{
					canBeAttribute = false;
					ast = new ConstantWrapper(this.scanner.GetPreProcessorValue(), this.currentToken.Clone());
					goto IL_0956;
				}
			}
			string text4 = JSKeyword.CanBeIdentifier(this.currentToken.token);
			if (text4 != null)
			{
				if (warnForKeyword)
				{
					JSToken token2 = this.currentToken.token;
					if (token2 != JSToken.Void)
					{
						switch (token2)
						{
						case JSToken.Boolean:
						case JSToken.Byte:
						case JSToken.Char:
						case JSToken.Double:
						case JSToken.Float:
						case JSToken.Int:
						case JSToken.Long:
							goto IL_08FC;
						case JSToken.Decimal:
						case JSToken.DoubleColon:
						case JSToken.Enum:
						case JSToken.Ensure:
						case JSToken.Goto:
						case JSToken.Invariant:
							break;
						default:
							if (token2 == JSToken.Short)
							{
								goto IL_08FC;
							}
							break;
						}
						this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
					}
				}
				IL_08FC:
				canBeAttribute = false;
				ast = new Lookup(text4, this.currentToken.Clone());
			}
			else
			{
				if (this.currentToken.token == JSToken.BitwiseAnd)
				{
					this.ReportError(JSError.WrongUseOfAddressOf);
					this.errorToken = null;
					this.GetNextToken();
					return this.ParseLeftHandSideExpression(isMinus, ref canBeAttribute, warnForKeyword);
				}
				this.ReportError(JSError.ExpressionExpected);
				this.SkipTokensAndThrow();
			}
			IL_0956:
			if (!flag)
			{
				this.GetNextToken();
			}
			return this.MemberExpression(ast, arrayList, ref canBeAttribute);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0003FDE0 File Offset: 0x0003EDE0
		private AST ParseConstructorCall(Context superCtx)
		{
			bool flag = JSToken.Super == this.currentToken.token;
			this.GetNextToken();
			Context context = this.currentToken.Clone();
			ASTList astlist = new ASTList(context);
			this.noSkipTokenSet.Add(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
			this.noSkipTokenSet.Add(NoSkipTokenSet.s_ParenToken);
			try
			{
				astlist = this.ParseExpressionList(JSToken.RightParen);
				this.GetNextToken();
			}
			catch (RecoveryTokenException ex)
			{
				if (ex._partiallyComputedNode != null)
				{
					astlist = (ASTList)ex._partiallyComputedNode;
				}
				if (this.IndexOfToken(NoSkipTokenSet.s_ParenToken, ex) == -1 && this.IndexOfToken(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet, ex) == -1)
				{
					ex._partiallyComputedNode = new ConstructorCall(superCtx, astlist, flag);
					throw ex;
				}
				if (ex._token == JSToken.RightParen)
				{
					this.GetNextToken();
				}
			}
			finally
			{
				this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ParenToken);
				this.noSkipTokenSet.Remove(NoSkipTokenSet.s_EndOfStatementNoSkipTokenSet);
			}
			superCtx.UpdateWith(context);
			return new ConstructorCall(superCtx, astlist, flag);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0003FEEC File Offset: 0x0003EEEC
		private CustomAttributeList ParseCustomAttributeList()
		{
			CustomAttributeList customAttributeList = new CustomAttributeList(this.currentToken.Clone());
			for (;;)
			{
				Context context = this.currentToken.Clone();
				bool flag = true;
				bool flag2;
				AST ast = this.ParseUnaryExpression(out flag2, ref flag, false, false);
				if (flag)
				{
					if (ast is Lookup || ast is Member)
					{
						customAttributeList.Append(new CustomAttribute(ast.context, ast, new ASTList(null)));
					}
					else
					{
						customAttributeList.Append(((Call)ast).ToCustomAttribute());
					}
				}
				else if (this.tokensSkipped == 0)
				{
					this.ReportError(JSError.SyntaxError, context);
				}
				if (this.currentToken.token == JSToken.RightBracket)
				{
					break;
				}
				if (this.currentToken.token == JSToken.Comma)
				{
					this.GetNextToken();
				}
				else
				{
					this.ReportError(JSError.NoRightBracketOrComma);
					this.SkipTokensAndThrow();
				}
			}
			return customAttributeList;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0003FFC0 File Offset: 0x0003EFC0
		private AST MemberExpression(AST expression, ArrayList newContexts)
		{
			bool flag = false;
			return this.MemberExpression(expression, newContexts, ref flag);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0003FFDC File Offset: 0x0003EFDC
		private AST MemberExpression(AST expression, ArrayList newContexts, ref bool canBeAttribute)
		{
			bool flag;
			return this.MemberExpression(expression, newContexts, out flag, ref canBeAttribute);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0003FFF4 File Offset: 0x0003EFF4
		private AST MemberExpression(AST expression, ArrayList newContexts, out bool canBeQualid, ref bool canBeAttribute)
		{
			bool flag = false;
			canBeQualid = true;
			for (;;)
			{
				this.noSkipTokenSet.Add(NoSkipTokenSet.s_MemberExprNoSkipTokenSet);
				try
				{
					try
					{
						switch (this.currentToken.token)
						{
						case JSToken.LeftParen:
						{
							if (flag)
							{
								canBeAttribute = false;
							}
							else
							{
								flag = true;
							}
							canBeQualid = false;
							ASTList astlist = null;
							RecoveryTokenException ex = null;
							this.noSkipTokenSet.Add(NoSkipTokenSet.s_ParenToken);
							try
							{
								astlist = this.ParseExpressionList(JSToken.RightParen);
							}
							catch (RecoveryTokenException ex2)
							{
								astlist = (ASTList)ex2._partiallyComputedNode;
								if (this.IndexOfToken(NoSkipTokenSet.s_ParenToken, ex2) == -1)
								{
									ex = ex2;
								}
							}
							finally
							{
								this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ParenToken);
							}
							if (expression is Lookup)
							{
								string text = expression.ToString();
								if (text.Equals("eval"))
								{
									expression.context.UpdateWith(astlist.context);
									if (astlist.count == 1)
									{
										expression = new Eval(expression.context, astlist[0], null);
									}
									else if (astlist.count > 1)
									{
										expression = new Eval(expression.context, astlist[0], astlist[1]);
									}
									else
									{
										expression = new Eval(expression.context, new ConstantWrapper("", this.CurrentPositionContext()), null);
									}
									canBeAttribute = false;
								}
								else if (this.Globals.engine.doPrint && text.Equals("print"))
								{
									expression.context.UpdateWith(astlist.context);
									expression = new Print(expression.context, astlist);
									canBeAttribute = false;
								}
								else
								{
									expression = new Call(expression.context.CombineWith(astlist.context), expression, astlist, false);
								}
							}
							else
							{
								expression = new Call(expression.context.CombineWith(astlist.context), expression, astlist, false);
							}
							if (newContexts != null && newContexts.Count > 0)
							{
								((Context)newContexts[newContexts.Count - 1]).UpdateWith(expression.context);
								if (!(expression is Call))
								{
									expression = new Call((Context)newContexts[newContexts.Count - 1], expression, new ASTList(this.CurrentPositionContext()), false);
								}
								else
								{
									expression.context = (Context)newContexts[newContexts.Count - 1];
								}
								((Call)expression).isConstructor = true;
								newContexts.RemoveAt(newContexts.Count - 1);
							}
							if (ex != null)
							{
								ex._partiallyComputedNode = expression;
								throw ex;
							}
							this.GetNextToken();
							break;
						}
						case JSToken.LeftBracket:
						{
							canBeQualid = false;
							canBeAttribute = false;
							this.noSkipTokenSet.Add(NoSkipTokenSet.s_BracketToken);
							ASTList astlist;
							try
							{
								astlist = this.ParseExpressionList(JSToken.RightBracket);
							}
							catch (RecoveryTokenException ex3)
							{
								if (this.IndexOfToken(NoSkipTokenSet.s_BracketToken, ex3) == -1)
								{
									if (ex3._partiallyComputedNode != null)
									{
										ex3._partiallyComputedNode = new Call(expression.context.CombineWith(this.currentToken.Clone()), expression, (ASTList)ex3._partiallyComputedNode, true);
									}
									else
									{
										ex3._partiallyComputedNode = expression;
									}
									throw ex3;
								}
								astlist = (ASTList)ex3._partiallyComputedNode;
							}
							finally
							{
								this.noSkipTokenSet.Remove(NoSkipTokenSet.s_BracketToken);
							}
							expression = new Call(expression.context.CombineWith(this.currentToken.Clone()), expression, astlist, true);
							if (newContexts != null && newContexts.Count > 0)
							{
								((Context)newContexts[newContexts.Count - 1]).UpdateWith(expression.context);
								expression.context = (Context)newContexts[newContexts.Count - 1];
								((Call)expression).isConstructor = true;
								newContexts.RemoveAt(newContexts.Count - 1);
							}
							this.GetNextToken();
							break;
						}
						case JSToken.AccessField:
						{
							if (flag)
							{
								canBeAttribute = false;
							}
							ConstantWrapper constantWrapper = null;
							this.GetNextToken();
							if (JSToken.Identifier != this.currentToken.token)
							{
								string text2 = JSKeyword.CanBeIdentifier(this.currentToken.token);
								if (text2 != null)
								{
									this.ForceReportInfo(JSError.KeywordUsedAsIdentifier);
									constantWrapper = new ConstantWrapper(text2, this.currentToken.Clone());
								}
								else
								{
									this.ReportError(JSError.NoIdentifier);
									this.SkipTokensAndThrow(expression);
								}
							}
							else
							{
								constantWrapper = new ConstantWrapper(this.scanner.GetIdentifier(), this.currentToken.Clone());
							}
							this.GetNextToken();
							expression = new Member(expression.context.CombineWith(constantWrapper.context), expression, constantWrapper);
							break;
						}
						default:
							if (newContexts != null)
							{
								while (newContexts.Count > 0)
								{
									((Context)newContexts[newContexts.Count - 1]).UpdateWith(expression.context);
									expression = new Call((Context)newContexts[newContexts.Count - 1], expression, new ASTList(this.CurrentPositionContext()), false);
									((Call)expression).isConstructor = true;
									newContexts.RemoveAt(newContexts.Count - 1);
								}
							}
							return expression;
						}
					}
					catch (RecoveryTokenException ex4)
					{
						if (this.IndexOfToken(NoSkipTokenSet.s_MemberExprNoSkipTokenSet, ex4) == -1)
						{
							throw ex4;
						}
						expression = ex4._partiallyComputedNode;
					}
					continue;
				}
				finally
				{
					this.noSkipTokenSet.Remove(NoSkipTokenSet.s_MemberExprNoSkipTokenSet);
				}
				break;
			}
			AST ast;
			return ast;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00040574 File Offset: 0x0003F574
		private ASTList ParseExpressionList(JSToken terminator)
		{
			Context context = this.currentToken.Clone();
			this.scanner.GetCurrentLine();
			this.GetNextToken();
			ASTList astlist = new ASTList(context);
			if (terminator != this.currentToken.token)
			{
				for (;;)
				{
					this.noSkipTokenSet.Add(NoSkipTokenSet.s_ExpressionListNoSkipTokenSet);
					try
					{
						if (JSToken.BitwiseAnd == this.currentToken.token)
						{
							Context context2 = this.currentToken.Clone();
							this.GetNextToken();
							AST ast = this.ParseLeftHandSideExpression();
							if (ast is Member || ast is Lookup)
							{
								context2.UpdateWith(ast.context);
								astlist.Append(new AddressOf(context2, ast));
							}
							else
							{
								this.ReportError(JSError.DoesNotHaveAnAddress, context2.Clone());
								astlist.Append(ast);
							}
						}
						else if (JSToken.Comma == this.currentToken.token)
						{
							astlist.Append(new ConstantWrapper(Missing.Value, this.currentToken.Clone()));
						}
						else
						{
							if (terminator == this.currentToken.token)
							{
								break;
							}
							astlist.Append(this.ParseExpression(true));
						}
						if (terminator == this.currentToken.token)
						{
							break;
						}
						if (JSToken.Comma != this.currentToken.token)
						{
							if (terminator == JSToken.RightParen)
							{
								if (JSToken.Semicolon == this.currentToken.token && JSToken.RightParen == this.scanner.PeekToken())
								{
									this.ReportError(JSError.UnexpectedSemicolon, true);
									this.GetNextToken();
									break;
								}
								this.ReportError(JSError.NoRightParenOrComma);
							}
							else
							{
								this.ReportError(JSError.NoRightBracketOrComma);
							}
							this.SkipTokensAndThrow();
						}
					}
					catch (RecoveryTokenException ex)
					{
						if (ex._partiallyComputedNode != null)
						{
							astlist.Append(ex._partiallyComputedNode);
						}
						if (this.IndexOfToken(NoSkipTokenSet.s_ExpressionListNoSkipTokenSet, ex) == -1)
						{
							ex._partiallyComputedNode = astlist;
							throw ex;
						}
					}
					finally
					{
						this.noSkipTokenSet.Remove(NoSkipTokenSet.s_ExpressionListNoSkipTokenSet);
					}
					this.GetNextToken();
				}
			}
			context.UpdateWith(this.currentToken);
			return astlist;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00040794 File Offset: 0x0003F794
		private AST CreateExpressionNode(JSToken op, AST operand1, AST operand2)
		{
			Context context = operand1.context.CombineWith(operand2.context);
			switch (op)
			{
			case JSToken.FirstBinaryOp:
				return new Plus(context, operand1, operand2);
			case JSToken.Minus:
				return new NumericBinary(context, operand1, operand2, JSToken.Minus);
			case JSToken.LogicalOr:
				return new Logical_or(context, operand1, operand2);
			case JSToken.LogicalAnd:
				return new Logical_and(context, operand1, operand2);
			case JSToken.BitwiseOr:
				return new BitwiseBinary(context, operand1, operand2, JSToken.BitwiseOr);
			case JSToken.BitwiseXor:
				return new BitwiseBinary(context, operand1, operand2, JSToken.BitwiseXor);
			case JSToken.BitwiseAnd:
				return new BitwiseBinary(context, operand1, operand2, JSToken.BitwiseAnd);
			case JSToken.Equal:
				return new Equality(context, operand1, operand2, JSToken.Equal);
			case JSToken.NotEqual:
				return new Equality(context, operand1, operand2, JSToken.NotEqual);
			case JSToken.StrictEqual:
				return new StrictEquality(context, operand1, operand2, JSToken.StrictEqual);
			case JSToken.StrictNotEqual:
				return new StrictEquality(context, operand1, operand2, JSToken.StrictNotEqual);
			case JSToken.GreaterThan:
				return new Relational(context, operand1, operand2, JSToken.GreaterThan);
			case JSToken.LessThan:
				return new Relational(context, operand1, operand2, JSToken.LessThan);
			case JSToken.LessThanEqual:
				return new Relational(context, operand1, operand2, JSToken.LessThanEqual);
			case JSToken.GreaterThanEqual:
				return new Relational(context, operand1, operand2, JSToken.GreaterThanEqual);
			case JSToken.LeftShift:
				return new BitwiseBinary(context, operand1, operand2, JSToken.LeftShift);
			case JSToken.RightShift:
				return new BitwiseBinary(context, operand1, operand2, JSToken.RightShift);
			case JSToken.UnsignedRightShift:
				return new BitwiseBinary(context, operand1, operand2, JSToken.UnsignedRightShift);
			case JSToken.Multiply:
				return new NumericBinary(context, operand1, operand2, JSToken.Multiply);
			case JSToken.Divide:
				return new NumericBinary(context, operand1, operand2, JSToken.Divide);
			case JSToken.Modulo:
				return new NumericBinary(context, operand1, operand2, JSToken.Modulo);
			case JSToken.Instanceof:
				return new Instanceof(context, operand1, operand2);
			case JSToken.In:
				return new In(context, operand1, operand2);
			case JSToken.Assign:
				return new Assign(context, operand1, operand2);
			case JSToken.PlusAssign:
				return new PlusAssign(context, operand1, operand2);
			case JSToken.MinusAssign:
				return new NumericBinaryAssign(context, operand1, operand2, JSToken.Minus);
			case JSToken.MultiplyAssign:
				return new NumericBinaryAssign(context, operand1, operand2, JSToken.Multiply);
			case JSToken.DivideAssign:
				return new NumericBinaryAssign(context, operand1, operand2, JSToken.Divide);
			case JSToken.BitwiseAndAssign:
				return new BitwiseBinaryAssign(context, operand1, operand2, JSToken.BitwiseAnd);
			case JSToken.BitwiseOrAssign:
				return new BitwiseBinaryAssign(context, operand1, operand2, JSToken.BitwiseOr);
			case JSToken.BitwiseXorAssign:
				return new BitwiseBinaryAssign(context, operand1, operand2, JSToken.BitwiseXor);
			case JSToken.ModuloAssign:
				return new NumericBinaryAssign(context, operand1, operand2, JSToken.Modulo);
			case JSToken.LeftShiftAssign:
				return new BitwiseBinaryAssign(context, operand1, operand2, JSToken.LeftShift);
			case JSToken.RightShiftAssign:
				return new BitwiseBinaryAssign(context, operand1, operand2, JSToken.RightShift);
			case JSToken.UnsignedRightShiftAssign:
				return new BitwiseBinaryAssign(context, operand1, operand2, JSToken.UnsignedRightShift);
			case JSToken.Comma:
				return new Comma(context, operand1, operand2);
			}
			return null;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x000409D8 File Offset: 0x0003F9D8
		private void GetNextToken()
		{
			if (this.errorToken == null)
			{
				this.goodTokensProcessed += 1L;
				this.breakRecursion = 0;
				this.scanner.GetNextToken();
				return;
			}
			if (this.breakRecursion > 10)
			{
				this.errorToken = null;
				this.scanner.GetNextToken();
				return;
			}
			this.breakRecursion++;
			this.currentToken = this.errorToken;
			this.errorToken = null;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00040A50 File Offset: 0x0003FA50
		private Context CurrentPositionContext()
		{
			Context context = this.currentToken.Clone();
			context.endPos = ((context.startPos < context.source_string.Length) ? (context.startPos + 1) : context.startPos);
			return context;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00040A93 File Offset: 0x0003FA93
		private void ReportError(JSError errorId)
		{
			this.ReportError(errorId, false);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00040AA0 File Offset: 0x0003FAA0
		private void ReportError(JSError errorId, bool skipToken)
		{
			Context context = this.currentToken.Clone();
			context.endPos = context.startPos + 1;
			this.ReportError(errorId, context, skipToken);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00040AD0 File Offset: 0x0003FAD0
		private void ReportError(JSError errorId, Context context)
		{
			this.ReportError(errorId, context, false);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00040ADC File Offset: 0x0003FADC
		private void ReportError(JSError errorId, Context context, bool skipToken)
		{
			int severity = this.Severity;
			this.Severity = new JScriptException(errorId).Severity;
			if (context.token == JSToken.EndOfFile)
			{
				this.EOFError(errorId);
				return;
			}
			if (this.goodTokensProcessed > 0L || this.Severity < severity)
			{
				context.HandleError(errorId);
			}
			if (skipToken)
			{
				this.goodTokensProcessed = -1L;
				return;
			}
			this.errorToken = this.currentToken;
			this.goodTokensProcessed = 0L;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00040B4B File Offset: 0x0003FB4B
		private void ForceReportInfo(JSError errorId)
		{
			this.ForceReportInfo(this.currentToken.Clone(), errorId);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00040B5F File Offset: 0x0003FB5F
		private void ForceReportInfo(Context context, JSError errorId)
		{
			context.HandleError(errorId);
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00040B68 File Offset: 0x0003FB68
		private void ForceReportInfo(JSError errorId, bool treatAsError)
		{
			this.currentToken.Clone().HandleError(errorId, treatAsError);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00040B7C File Offset: 0x0003FB7C
		private void EOFError(JSError errorId)
		{
			Context context = this.sourceContext.Clone();
			context.lineNumber = this.scanner.GetCurrentLine();
			context.endLineNumber = context.lineNumber;
			context.startLinePos = this.scanner.GetStartLinePosition();
			context.endLinePos = context.startLinePos;
			context.startPos = this.sourceContext.endPos;
			context.endPos++;
			context.HandleError(errorId);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00040BF5 File Offset: 0x0003FBF5
		private void SkipTokensAndThrow()
		{
			this.SkipTokensAndThrow(null);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00040C00 File Offset: 0x0003FC00
		private void SkipTokensAndThrow(AST partialAST)
		{
			this.errorToken = null;
			bool flag = this.noSkipTokenSet.HasToken(JSToken.EndOfLine);
			while (!this.noSkipTokenSet.HasToken(this.currentToken.token))
			{
				if (flag && this.scanner.GotEndOfLine())
				{
					this.errorToken = this.currentToken;
					throw new RecoveryTokenException(JSToken.EndOfLine, partialAST);
				}
				this.GetNextToken();
				if (++this.tokensSkipped > 50)
				{
					this.ForceReportInfo(JSError.TooManyTokensSkipped);
					throw new EndOfFile();
				}
				if (this.currentToken.token == JSToken.EndOfFile)
				{
					throw new EndOfFile();
				}
			}
			this.errorToken = this.currentToken;
			throw new RecoveryTokenException(this.currentToken.token, partialAST);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00040CC3 File Offset: 0x0003FCC3
		private int IndexOfToken(JSToken[] tokens, RecoveryTokenException exc)
		{
			return this.IndexOfToken(tokens, exc._token);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00040CD4 File Offset: 0x0003FCD4
		private int IndexOfToken(JSToken[] tokens, JSToken token)
		{
			int num = 0;
			int num2 = tokens.Length;
			while (num < num2 && tokens[num] != token)
			{
				num++;
			}
			if (num >= num2)
			{
				num = -1;
			}
			else
			{
				this.errorToken = null;
			}
			return num;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00040D07 File Offset: 0x0003FD07
		private bool TokenInList(JSToken[] tokens, JSToken token)
		{
			return -1 != this.IndexOfToken(tokens, token);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00040D17 File Offset: 0x0003FD17
		private bool TokenInList(JSToken[] tokens, RecoveryTokenException exc)
		{
			return -1 != this.IndexOfToken(tokens, exc._token);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00040D2C File Offset: 0x0003FD2C
		private CustomAttributeList FromASTListToCustomAttributeList(ArrayList attributes)
		{
			CustomAttributeList customAttributeList = null;
			if (attributes != null && attributes.Count > 0)
			{
				customAttributeList = new CustomAttributeList(((AST)attributes[0]).context);
			}
			int i = 0;
			int count = attributes.Count;
			while (i < count)
			{
				ASTList astlist = new ASTList(null);
				if (attributes[i] is Lookup || attributes[i] is Member)
				{
					customAttributeList.Append(new CustomAttribute(((AST)attributes[i]).context, (AST)attributes[i], astlist));
				}
				else
				{
					customAttributeList.Append(((Call)attributes[i]).ToCustomAttribute());
				}
				i++;
			}
			return customAttributeList;
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x00040DD7 File Offset: 0x0003FDD7
		internal bool HasAborted
		{
			get
			{
				return this.tokensSkipped > 50;
			}
		}

		// Token: 0x04000466 RID: 1126
		private const int c_MaxSkippedTokenNumber = 50;

		// Token: 0x04000467 RID: 1127
		private bool demandFullTrustOnFunctionCreation;

		// Token: 0x04000468 RID: 1128
		private Context sourceContext;

		// Token: 0x04000469 RID: 1129
		private JSScanner scanner;

		// Token: 0x0400046A RID: 1130
		private Context currentToken;

		// Token: 0x0400046B RID: 1131
		private Context errorToken;

		// Token: 0x0400046C RID: 1132
		private int tokensSkipped;

		// Token: 0x0400046D RID: 1133
		private NoSkipTokenSet noSkipTokenSet;

		// Token: 0x0400046E RID: 1134
		private long goodTokensProcessed;

		// Token: 0x0400046F RID: 1135
		private Block program;

		// Token: 0x04000470 RID: 1136
		private ArrayList blockType;

		// Token: 0x04000471 RID: 1137
		private SimpleHashtable labelTable;

		// Token: 0x04000472 RID: 1138
		private int finallyEscaped;

		// Token: 0x04000473 RID: 1139
		private int breakRecursion;

		// Token: 0x04000474 RID: 1140
		private static int s_cDummyName;

		// Token: 0x04000475 RID: 1141
		private Globals Globals;

		// Token: 0x04000476 RID: 1142
		private int Severity;

		// Token: 0x020000B7 RID: 183
		private enum BlockType
		{
			// Token: 0x04000478 RID: 1144
			Block,
			// Token: 0x04000479 RID: 1145
			Loop,
			// Token: 0x0400047A RID: 1146
			Switch,
			// Token: 0x0400047B RID: 1147
			Finally
		}
	}
}
