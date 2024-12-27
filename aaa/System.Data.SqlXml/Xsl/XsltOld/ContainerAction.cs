using System;
using System.Collections;
using System.Globalization;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000132 RID: 306
	internal class ContainerAction : CompiledAction
	{
		// Token: 0x06000D78 RID: 3448 RVA: 0x00045489 File Offset: 0x00044489
		internal override void Compile(Compiler compiler)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00045490 File Offset: 0x00044490
		internal void CompileStylesheetAttributes(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			string localName = input.LocalName;
			string text = null;
			string text2 = null;
			if (input.MoveToFirstAttribute())
			{
				for (;;)
				{
					string namespaceURI = input.NamespaceURI;
					string localName2 = input.LocalName;
					if (Keywords.Equals(namespaceURI, input.Atoms.Empty))
					{
						if (Keywords.Equals(localName2, input.Atoms.Version))
						{
							text2 = input.Value;
							if (1.0 <= XmlConvert.ToXPathDouble(text2))
							{
								compiler.ForwardCompatibility = text2 != "1.0";
							}
							else if (!compiler.ForwardCompatibility)
							{
								break;
							}
						}
						else if (Keywords.Equals(localName2, input.Atoms.ExtensionElementPrefixes))
						{
							compiler.InsertExtensionNamespace(input.Value);
						}
						else if (Keywords.Equals(localName2, input.Atoms.ExcludeResultPrefixes))
						{
							compiler.InsertExcludedNamespace(input.Value);
						}
						else if (!Keywords.Equals(localName2, input.Atoms.Id))
						{
							text = localName2;
						}
					}
					if (!input.MoveToNextAttribute())
					{
						goto Block_8;
					}
				}
				throw XsltException.Create("Xslt_InvalidAttrValue", new string[] { "version", text2 });
				Block_8:
				input.ToParent();
			}
			if (text2 == null)
			{
				throw XsltException.Create("Xslt_MissingAttribute", new string[] { "version" });
			}
			if (text != null && !compiler.ForwardCompatibility)
			{
				throw XsltException.Create("Xslt_InvalidAttribute", new string[] { text, localName });
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00045608 File Offset: 0x00044608
		internal void CompileSingleTemplate(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			string text = null;
			if (input.MoveToFirstAttribute())
			{
				do
				{
					string namespaceURI = input.NamespaceURI;
					string localName = input.LocalName;
					if (Keywords.Equals(namespaceURI, input.Atoms.XsltNamespace) && Keywords.Equals(localName, input.Atoms.Version))
					{
						text = input.Value;
					}
				}
				while (input.MoveToNextAttribute());
				input.ToParent();
			}
			if (text != null)
			{
				compiler.AddTemplate(compiler.CreateSingleTemplateAction());
				return;
			}
			if (Keywords.Equals(input.LocalName, input.Atoms.Stylesheet) && input.NamespaceURI == "http://www.w3.org/TR/WD-xsl")
			{
				throw XsltException.Create("Xslt_WdXslNamespace", new string[0]);
			}
			throw XsltException.Create("Xslt_WrongStylesheetElement", new string[0]);
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x000456CC File Offset: 0x000446CC
		protected void CompileDocument(Compiler compiler, bool inInclude)
		{
			NavigatorInput input = compiler.Input;
			while (input.NodeType != XPathNodeType.Element)
			{
				if (!compiler.Advance())
				{
					throw XsltException.Create("Xslt_WrongStylesheetElement", new string[0]);
				}
			}
			if (Keywords.Equals(input.NamespaceURI, input.Atoms.XsltNamespace))
			{
				if (!Keywords.Equals(input.LocalName, input.Atoms.Stylesheet) && !Keywords.Equals(input.LocalName, input.Atoms.Transform))
				{
					throw XsltException.Create("Xslt_WrongStylesheetElement", new string[0]);
				}
				compiler.PushNamespaceScope();
				this.CompileStylesheetAttributes(compiler);
				this.CompileTopLevelElements(compiler);
				if (!inInclude)
				{
					this.CompileImports(compiler);
				}
			}
			else
			{
				compiler.PushLiteralScope();
				this.CompileSingleTemplate(compiler);
			}
			compiler.PopScope();
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00045790 File Offset: 0x00044790
		internal Stylesheet CompileImport(Compiler compiler, Uri uri, int id)
		{
			NavigatorInput navigatorInput = compiler.ResolveDocument(uri);
			compiler.PushInputDocument(navigatorInput);
			try
			{
				compiler.PushStylesheet(new Stylesheet());
				compiler.Stylesheetid = id;
				this.CompileDocument(compiler, false);
			}
			catch (XsltCompileException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new XsltCompileException(ex, navigatorInput.BaseURI, navigatorInput.LineNumber, navigatorInput.LinePosition);
			}
			finally
			{
				compiler.PopInputDocument();
			}
			return compiler.PopStylesheet();
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0004581C File Offset: 0x0004481C
		private void CompileImports(Compiler compiler)
		{
			ArrayList imports = compiler.CompiledStylesheet.Imports;
			int stylesheetid = compiler.Stylesheetid;
			int num = imports.Count - 1;
			while (0 <= num)
			{
				Uri uri = imports[num] as Uri;
				imports[num] = this.CompileImport(compiler, uri, ++this.maxid);
				num--;
			}
			compiler.Stylesheetid = stylesheetid;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00045888 File Offset: 0x00044888
		private void CompileInclude(Compiler compiler)
		{
			Uri uri = compiler.ResolveUri(compiler.GetSingleAttribute(compiler.Input.Atoms.Href));
			string text = uri.ToString();
			if (compiler.IsCircularReference(text))
			{
				throw XsltException.Create("Xslt_CircularInclude", new string[] { text });
			}
			NavigatorInput navigatorInput = compiler.ResolveDocument(uri);
			compiler.PushInputDocument(navigatorInput);
			try
			{
				this.CompileDocument(compiler, true);
			}
			catch (XsltCompileException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new XsltCompileException(ex, navigatorInput.BaseURI, navigatorInput.LineNumber, navigatorInput.LinePosition);
			}
			finally
			{
				compiler.PopInputDocument();
			}
			base.CheckEmpty(compiler);
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0004594C File Offset: 0x0004494C
		internal void CompileNamespaceAlias(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			string localName = input.LocalName;
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			if (input.MoveToFirstAttribute())
			{
				string localName2;
				for (;;)
				{
					string namespaceURI = input.NamespaceURI;
					localName2 = input.LocalName;
					if (Keywords.Equals(namespaceURI, input.Atoms.Empty))
					{
						if (Keywords.Equals(localName2, input.Atoms.StylesheetPrefix))
						{
							text3 = input.Value;
							text = compiler.GetNsAlias(ref text3);
						}
						else if (Keywords.Equals(localName2, input.Atoms.ResultPrefix))
						{
							text4 = input.Value;
							text2 = compiler.GetNsAlias(ref text4);
						}
						else if (!compiler.ForwardCompatibility)
						{
							break;
						}
					}
					if (!input.MoveToNextAttribute())
					{
						goto Block_5;
					}
				}
				throw XsltException.Create("Xslt_InvalidAttribute", new string[] { localName2, localName });
				Block_5:
				input.ToParent();
			}
			base.CheckRequiredAttribute(compiler, text, "stylesheet-prefix");
			base.CheckRequiredAttribute(compiler, text2, "result-prefix");
			base.CheckEmpty(compiler);
			compiler.AddNamespaceAlias(text, new NamespaceInfo(text4, text2, compiler.Stylesheetid));
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00045A60 File Offset: 0x00044A60
		internal void CompileKey(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			string localName = input.LocalName;
			int num = -1;
			int num2 = -1;
			XmlQualifiedName xmlQualifiedName = null;
			if (input.MoveToFirstAttribute())
			{
				string localName2;
				for (;;)
				{
					string namespaceURI = input.NamespaceURI;
					localName2 = input.LocalName;
					string value = input.Value;
					if (Keywords.Equals(namespaceURI, input.Atoms.Empty))
					{
						if (Keywords.Equals(localName2, input.Atoms.Name))
						{
							xmlQualifiedName = compiler.CreateXPathQName(value);
						}
						else if (Keywords.Equals(localName2, input.Atoms.Match))
						{
							num = compiler.AddQuery(value, false, false, true);
						}
						else if (Keywords.Equals(localName2, input.Atoms.Use))
						{
							num2 = compiler.AddQuery(value, false, false, false);
						}
						else if (!compiler.ForwardCompatibility)
						{
							break;
						}
					}
					if (!input.MoveToNextAttribute())
					{
						goto Block_6;
					}
				}
				throw XsltException.Create("Xslt_InvalidAttribute", new string[] { localName2, localName });
				Block_6:
				input.ToParent();
			}
			base.CheckRequiredAttribute(compiler, num != -1, "match");
			base.CheckRequiredAttribute(compiler, num2 != -1, "use");
			base.CheckRequiredAttribute(compiler, xmlQualifiedName != null, "name");
			compiler.InsertKey(xmlQualifiedName, num, num2);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00045BA0 File Offset: 0x00044BA0
		protected void CompileDecimalFormat(Compiler compiler)
		{
			NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
			DecimalFormat decimalFormat = new DecimalFormat(numberFormatInfo, '#', '0', ';');
			XmlQualifiedName xmlQualifiedName = null;
			NavigatorInput input = compiler.Input;
			if (input.MoveToFirstAttribute())
			{
				do
				{
					if (Keywords.Equals(input.Prefix, input.Atoms.Empty))
					{
						string localName = input.LocalName;
						string value = input.Value;
						if (Keywords.Equals(localName, input.Atoms.Name))
						{
							xmlQualifiedName = compiler.CreateXPathQName(value);
						}
						else if (Keywords.Equals(localName, input.Atoms.DecimalSeparator))
						{
							numberFormatInfo.NumberDecimalSeparator = value;
						}
						else if (Keywords.Equals(localName, input.Atoms.GroupingSeparator))
						{
							numberFormatInfo.NumberGroupSeparator = value;
						}
						else if (Keywords.Equals(localName, input.Atoms.Infinity))
						{
							numberFormatInfo.PositiveInfinitySymbol = value;
						}
						else if (Keywords.Equals(localName, input.Atoms.MinusSign))
						{
							numberFormatInfo.NegativeSign = value;
						}
						else if (Keywords.Equals(localName, input.Atoms.NaN))
						{
							numberFormatInfo.NaNSymbol = value;
						}
						else if (Keywords.Equals(localName, input.Atoms.Percent))
						{
							numberFormatInfo.PercentSymbol = value;
						}
						else if (Keywords.Equals(localName, input.Atoms.PerMille))
						{
							numberFormatInfo.PerMilleSymbol = value;
						}
						else if (Keywords.Equals(localName, input.Atoms.Digit))
						{
							if (this.CheckAttribute(value.Length == 1, compiler))
							{
								decimalFormat.digit = value[0];
							}
						}
						else if (Keywords.Equals(localName, input.Atoms.ZeroDigit))
						{
							if (this.CheckAttribute(value.Length == 1, compiler))
							{
								decimalFormat.zeroDigit = value[0];
							}
						}
						else if (Keywords.Equals(localName, input.Atoms.PatternSeparator) && this.CheckAttribute(value.Length == 1, compiler))
						{
							decimalFormat.patternSeparator = value[0];
						}
					}
				}
				while (input.MoveToNextAttribute());
				input.ToParent();
			}
			numberFormatInfo.NegativeInfinitySymbol = numberFormatInfo.NegativeSign + numberFormatInfo.PositiveInfinitySymbol;
			if (xmlQualifiedName == null)
			{
				xmlQualifiedName = new XmlQualifiedName();
			}
			compiler.AddDecimalFormat(xmlQualifiedName, decimalFormat);
			base.CheckEmpty(compiler);
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00045DF4 File Offset: 0x00044DF4
		internal bool CheckAttribute(bool valid, Compiler compiler)
		{
			if (valid)
			{
				return true;
			}
			if (!compiler.ForwardCompatibility)
			{
				throw XsltException.Create("Xslt_InvalidAttrValue", new string[]
				{
					compiler.Input.LocalName,
					compiler.Input.Value
				});
			}
			return false;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x00045E40 File Offset: 0x00044E40
		protected void CompileSpace(Compiler compiler, bool preserve)
		{
			string singleAttribute = compiler.GetSingleAttribute(compiler.Input.Atoms.Elements);
			string[] array = XmlConvert.SplitString(singleAttribute);
			for (int i = 0; i < array.Length; i++)
			{
				double num = this.NameTest(array[i]);
				compiler.CompiledStylesheet.AddSpace(compiler, array[i], num, preserve);
			}
			base.CheckEmpty(compiler);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x00045E9C File Offset: 0x00044E9C
		private double NameTest(string name)
		{
			if (name == "*")
			{
				return -0.5;
			}
			int num = name.Length - 2;
			if (0 > num || name[num] != ':' || name[num + 1] != '*')
			{
				string text;
				string text2;
				PrefixQName.ParseQualifiedName(name, out text, out text2);
				return 0.0;
			}
			if (!PrefixQName.ValidatePrefix(name.Substring(0, num)))
			{
				throw XsltException.Create("Xslt_InvalidAttrValue", new string[] { "elements", name });
			}
			return -0.25;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00045F30 File Offset: 0x00044F30
		protected void CompileTopLevelElements(Compiler compiler)
		{
			if (!compiler.Recurse())
			{
				return;
			}
			NavigatorInput input = compiler.Input;
			bool flag = false;
			string text;
			for (;;)
			{
				switch (input.NodeType)
				{
				case XPathNodeType.Element:
				{
					string localName = input.LocalName;
					string namespaceURI = input.NamespaceURI;
					if (Keywords.Equals(namespaceURI, input.Atoms.XsltNamespace))
					{
						if (Keywords.Equals(localName, input.Atoms.Import))
						{
							if (flag)
							{
								goto Block_5;
							}
							Uri uri = compiler.ResolveUri(compiler.GetSingleAttribute(compiler.Input.Atoms.Href));
							text = uri.ToString();
							if (compiler.IsCircularReference(text))
							{
								goto Block_6;
							}
							compiler.CompiledStylesheet.Imports.Add(uri);
							base.CheckEmpty(compiler);
							goto IL_0312;
						}
						else
						{
							if (Keywords.Equals(localName, input.Atoms.Include))
							{
								flag = true;
								this.CompileInclude(compiler);
								goto IL_0312;
							}
							flag = true;
							compiler.PushNamespaceScope();
							if (Keywords.Equals(localName, input.Atoms.StripSpace))
							{
								this.CompileSpace(compiler, false);
							}
							else if (Keywords.Equals(localName, input.Atoms.PreserveSpace))
							{
								this.CompileSpace(compiler, true);
							}
							else if (Keywords.Equals(localName, input.Atoms.Output))
							{
								this.CompileOutput(compiler);
							}
							else if (Keywords.Equals(localName, input.Atoms.Key))
							{
								this.CompileKey(compiler);
							}
							else if (Keywords.Equals(localName, input.Atoms.DecimalFormat))
							{
								this.CompileDecimalFormat(compiler);
							}
							else if (Keywords.Equals(localName, input.Atoms.NamespaceAlias))
							{
								this.CompileNamespaceAlias(compiler);
							}
							else if (Keywords.Equals(localName, input.Atoms.AttributeSet))
							{
								compiler.AddAttributeSet(compiler.CreateAttributeSetAction());
							}
							else if (Keywords.Equals(localName, input.Atoms.Variable))
							{
								VariableAction variableAction = compiler.CreateVariableAction(VariableType.GlobalVariable);
								if (variableAction != null)
								{
									this.AddAction(variableAction);
								}
							}
							else if (Keywords.Equals(localName, input.Atoms.Param))
							{
								VariableAction variableAction2 = compiler.CreateVariableAction(VariableType.GlobalParameter);
								if (variableAction2 != null)
								{
									this.AddAction(variableAction2);
								}
							}
							else if (Keywords.Equals(localName, input.Atoms.Template))
							{
								compiler.AddTemplate(compiler.CreateTemplateAction());
							}
							else if (!compiler.ForwardCompatibility)
							{
								goto Block_20;
							}
							compiler.PopScope();
							goto IL_0312;
						}
					}
					else
					{
						if (namespaceURI == input.Atoms.MsXsltNamespace && localName == input.Atoms.Script)
						{
							this.AddScript(compiler);
							goto IL_0312;
						}
						if (Keywords.Equals(namespaceURI, input.Atoms.Empty))
						{
							goto Block_23;
						}
						goto IL_0312;
					}
					break;
				}
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
				case XPathNodeType.ProcessingInstruction:
				case XPathNodeType.Comment:
					goto IL_0312;
				}
				break;
				IL_0312:
				if (!compiler.Advance())
				{
					goto Block_24;
				}
			}
			goto IL_02F4;
			Block_5:
			throw XsltException.Create("Xslt_NotFirstImport", new string[0]);
			Block_6:
			throw XsltException.Create("Xslt_CircularInclude", new string[] { text });
			Block_20:
			throw compiler.UnexpectedKeyword();
			Block_23:
			throw XsltException.Create("Xslt_NullNsAtTopLevel", new string[] { input.Name });
			IL_02F4:
			throw XsltException.Create("Xslt_InvalidContents", new string[] { "stylesheet" });
			Block_24:
			compiler.ToParent();
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00046261 File Offset: 0x00045261
		protected void CompileTemplate(Compiler compiler)
		{
			do
			{
				this.CompileOnceTemplate(compiler);
			}
			while (compiler.Advance());
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00046274 File Offset: 0x00045274
		protected void CompileOnceTemplate(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			if (input.NodeType != XPathNodeType.Element)
			{
				this.CompileLiteral(compiler);
				return;
			}
			string namespaceURI = input.NamespaceURI;
			if (Keywords.Equals(namespaceURI, input.Atoms.XsltNamespace))
			{
				compiler.PushNamespaceScope();
				this.CompileInstruction(compiler);
				compiler.PopScope();
				return;
			}
			compiler.PushLiteralScope();
			compiler.InsertExtensionNamespace();
			if (compiler.IsExtensionNamespace(namespaceURI))
			{
				this.AddAction(compiler.CreateNewInstructionAction());
			}
			else
			{
				this.CompileLiteral(compiler);
			}
			compiler.PopScope();
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x000462F8 File Offset: 0x000452F8
		private void CompileInstruction(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			string localName = input.LocalName;
			CompiledAction compiledAction;
			if (Keywords.Equals(localName, input.Atoms.ApplyImports))
			{
				compiledAction = compiler.CreateApplyImportsAction();
			}
			else if (Keywords.Equals(localName, input.Atoms.ApplyTemplates))
			{
				compiledAction = compiler.CreateApplyTemplatesAction();
			}
			else if (Keywords.Equals(localName, input.Atoms.Attribute))
			{
				compiledAction = compiler.CreateAttributeAction();
			}
			else if (Keywords.Equals(localName, input.Atoms.CallTemplate))
			{
				compiledAction = compiler.CreateCallTemplateAction();
			}
			else if (Keywords.Equals(localName, input.Atoms.Choose))
			{
				compiledAction = compiler.CreateChooseAction();
			}
			else if (Keywords.Equals(localName, input.Atoms.Comment))
			{
				compiledAction = compiler.CreateCommentAction();
			}
			else if (Keywords.Equals(localName, input.Atoms.Copy))
			{
				compiledAction = compiler.CreateCopyAction();
			}
			else if (Keywords.Equals(localName, input.Atoms.CopyOf))
			{
				compiledAction = compiler.CreateCopyOfAction();
			}
			else if (Keywords.Equals(localName, input.Atoms.Element))
			{
				compiledAction = compiler.CreateElementAction();
			}
			else
			{
				if (Keywords.Equals(localName, input.Atoms.Fallback))
				{
					return;
				}
				if (Keywords.Equals(localName, input.Atoms.ForEach))
				{
					compiledAction = compiler.CreateForEachAction();
				}
				else if (Keywords.Equals(localName, input.Atoms.If))
				{
					compiledAction = compiler.CreateIfAction(IfAction.ConditionType.ConditionIf);
				}
				else if (Keywords.Equals(localName, input.Atoms.Message))
				{
					compiledAction = compiler.CreateMessageAction();
				}
				else if (Keywords.Equals(localName, input.Atoms.Number))
				{
					compiledAction = compiler.CreateNumberAction();
				}
				else if (Keywords.Equals(localName, input.Atoms.ProcessingInstruction))
				{
					compiledAction = compiler.CreateProcessingInstructionAction();
				}
				else if (Keywords.Equals(localName, input.Atoms.Text))
				{
					compiledAction = compiler.CreateTextAction();
				}
				else if (Keywords.Equals(localName, input.Atoms.ValueOf))
				{
					compiledAction = compiler.CreateValueOfAction();
				}
				else if (Keywords.Equals(localName, input.Atoms.Variable))
				{
					compiledAction = compiler.CreateVariableAction(VariableType.LocalVariable);
				}
				else
				{
					if (!compiler.ForwardCompatibility)
					{
						throw compiler.UnexpectedKeyword();
					}
					compiledAction = compiler.CreateNewInstructionAction();
				}
			}
			this.AddAction(compiledAction);
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00046550 File Offset: 0x00045550
		private void CompileLiteral(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			switch (input.NodeType)
			{
			case XPathNodeType.Element:
				this.AddEvent(compiler.CreateBeginEvent());
				this.CompileLiteralAttributesAndNamespaces(compiler);
				if (compiler.Recurse())
				{
					this.CompileTemplate(compiler);
					compiler.ToParent();
				}
				this.AddEvent(new EndEvent(XPathNodeType.Element));
				return;
			case XPathNodeType.Attribute:
			case XPathNodeType.Namespace:
			case XPathNodeType.Whitespace:
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
				break;
			case XPathNodeType.Text:
			case XPathNodeType.SignificantWhitespace:
				this.AddEvent(compiler.CreateTextEvent());
				break;
			default:
				return;
			}
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x000465D8 File Offset: 0x000455D8
		private void CompileLiteralAttributesAndNamespaces(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			if (input.Navigator.MoveToAttribute("use-attribute-sets", input.Atoms.XsltNamespace))
			{
				this.AddAction(compiler.CreateUseAttributeSetsAction());
				input.Navigator.MoveToParent();
			}
			compiler.InsertExcludedNamespace();
			if (input.MoveToFirstNamespace())
			{
				do
				{
					string value = input.Value;
					if (!Keywords.Compare(value, input.Atoms.XsltNamespace) && !compiler.IsExcludedNamespace(value) && !compiler.IsExtensionNamespace(value) && !compiler.IsNamespaceAlias(value))
					{
						this.AddEvent(new NamespaceEvent(input));
					}
				}
				while (input.MoveToNextNamespace());
				input.ToParent();
			}
			if (input.MoveToFirstAttribute())
			{
				do
				{
					if (!Keywords.Equals(input.NamespaceURI, input.Atoms.XsltNamespace))
					{
						this.AddEvent(compiler.CreateBeginEvent());
						this.AddEvents(compiler.CompileAvt(input.Value));
						this.AddEvent(new EndEvent(XPathNodeType.Attribute));
					}
				}
				while (input.MoveToNextAttribute());
				input.ToParent();
			}
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x000466D8 File Offset: 0x000456D8
		private void CompileOutput(Compiler compiler)
		{
			compiler.RootAction.Output.Compile(compiler);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000466EB File Offset: 0x000456EB
		internal void AddAction(Action action)
		{
			if (this.containedActions == null)
			{
				this.containedActions = new ArrayList();
			}
			this.containedActions.Add(action);
			this.lastCopyCodeAction = null;
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x00046714 File Offset: 0x00045714
		private void EnsureCopyCodeAction()
		{
			if (this.lastCopyCodeAction == null)
			{
				CopyCodeAction copyCodeAction = new CopyCodeAction();
				this.AddAction(copyCodeAction);
				this.lastCopyCodeAction = copyCodeAction;
			}
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0004673D File Offset: 0x0004573D
		protected void AddEvent(Event copyEvent)
		{
			this.EnsureCopyCodeAction();
			this.lastCopyCodeAction.AddEvent(copyEvent);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00046751 File Offset: 0x00045751
		protected void AddEvents(ArrayList copyEvents)
		{
			this.EnsureCopyCodeAction();
			this.lastCopyCodeAction.AddEvents(copyEvents);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00046768 File Offset: 0x00045768
		private void AddScript(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			ScriptingLanguage scriptingLanguage = ScriptingLanguage.JScript;
			string text = null;
			if (input.MoveToFirstAttribute())
			{
				string value;
				for (;;)
				{
					if (input.LocalName == input.Atoms.Language)
					{
						value = input.Value;
						if (string.Compare(value, "jscript", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(value, "javascript", StringComparison.OrdinalIgnoreCase) == 0)
						{
							scriptingLanguage = ScriptingLanguage.JScript;
						}
						else if (string.Compare(value, "c#", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(value, "csharp", StringComparison.OrdinalIgnoreCase) == 0)
						{
							scriptingLanguage = ScriptingLanguage.CSharp;
						}
						else
						{
							if (string.Compare(value, "vb", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(value, "visualbasic", StringComparison.OrdinalIgnoreCase) != 0)
							{
								break;
							}
							scriptingLanguage = ScriptingLanguage.VisualBasic;
						}
					}
					else if (input.LocalName == input.Atoms.ImplementsPrefix)
					{
						if (!PrefixQName.ValidatePrefix(input.Value))
						{
							goto Block_6;
						}
						text = compiler.ResolveXmlNamespace(input.Value);
					}
					if (!input.MoveToNextAttribute())
					{
						goto Block_7;
					}
				}
				throw XsltException.Create("Xslt_ScriptInvalidLanguage", new string[] { value });
				Block_6:
				throw XsltException.Create("Xslt_InvalidAttrValue", new string[] { input.LocalName, input.Value });
				Block_7:
				input.ToParent();
			}
			if (text == null)
			{
				throw XsltException.Create("Xslt_MissingAttribute", new string[] { input.Atoms.ImplementsPrefix });
			}
			if (!input.Recurse() || input.NodeType != XPathNodeType.Text)
			{
				throw XsltException.Create("Xslt_ScriptEmpty", new string[0]);
			}
			compiler.AddScript(input.Value, scriptingLanguage, text, input.BaseURI, input.LineNumber);
			input.ToParent();
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00046904 File Offset: 0x00045904
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				if (this.containedActions != null && this.containedActions.Count > 0)
				{
					processor.PushActionFrame(frame);
					frame.State = 1;
					return;
				}
				frame.Finished();
				return;
			case 1:
				frame.Finished();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00046959 File Offset: 0x00045959
		internal Action GetAction(int actionIndex)
		{
			if (this.containedActions != null && actionIndex < this.containedActions.Count)
			{
				return (Action)this.containedActions[actionIndex];
			}
			return null;
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00046984 File Offset: 0x00045984
		internal void CheckDuplicateParams(XmlQualifiedName name)
		{
			if (this.containedActions != null)
			{
				foreach (object obj in this.containedActions)
				{
					CompiledAction compiledAction = (CompiledAction)obj;
					WithParamAction withParamAction = compiledAction as WithParamAction;
					if (withParamAction != null && withParamAction.Name == name)
					{
						throw XsltException.Create("Xslt_DuplicateWithParam", new string[] { name.ToString() });
					}
				}
			}
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00046A18 File Offset: 0x00045A18
		internal override void ReplaceNamespaceAlias(Compiler compiler)
		{
			if (this.containedActions == null)
			{
				return;
			}
			int count = this.containedActions.Count;
			for (int i = 0; i < this.containedActions.Count; i++)
			{
				((Action)this.containedActions[i]).ReplaceNamespaceAlias(compiler);
			}
		}

		// Token: 0x040008EF RID: 2287
		protected const int ProcessingChildren = 1;

		// Token: 0x040008F0 RID: 2288
		internal ArrayList containedActions;

		// Token: 0x040008F1 RID: 2289
		internal CopyCodeAction lastCopyCodeAction;

		// Token: 0x040008F2 RID: 2290
		private int maxid;
	}
}
