using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.Runtime;
using System.Xml.Xsl.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000FD RID: 253
	internal class QilGenerator : IErrorHelper, IXPathEnvironment, IFocus
	{
		// Token: 0x06000B06 RID: 2822 RVA: 0x000358E2 File Offset: 0x000348E2
		public static QilExpression CompileStylesheet(Compiler compiler)
		{
			return new QilGenerator(compiler.IsDebug).Compile(compiler);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x000358F8 File Offset: 0x000348F8
		private QilGenerator(bool debug)
		{
			this.scope = new CompilerScopeManager<QilIterator>();
			this.outputScope = new OutputScopeManager();
			this.prefixesInUse = new HybridDictionary();
			this.f = new XsltQilFactory(new QilFactory(), debug);
			this.xpathBuilder = new XPathBuilder(this);
			this.xpathParser = new XPathParser<QilNode>();
			this.ptrnBuilder = new XPathPatternBuilder(this);
			this.ptrnParser = new XPathPatternParser();
			this.refReplacer = new ReferenceReplacer(this.f.BaseFactory);
			this.invkGen = new InvokeGenerator(this.f, debug);
			this.matcherBuilder = new MatcherBuilder(this.f, this.refReplacer, this.invkGen);
			this.singlFocus = new SingletonFocus(this.f);
			this.funcFocus = default(FunctionFocus);
			this.curLoop = new LoopFocus(this.f);
			this.strConcat = new QilStrConcatenator(this.f);
			this.varHelper = new QilGenerator.VariableHelper(this.f);
			this.elementOrDocumentType = XmlQueryTypeFactory.DocumentOrElement;
			this.textOrAttributeType = XmlQueryTypeFactory.NodeChoice(XmlNodeKindFlags.Attribute | XmlNodeKindFlags.Text);
			this.nameCurrent = this.f.QName("current", "urn:schemas-microsoft-com:xslt-debug");
			this.namePosition = this.f.QName("position", "urn:schemas-microsoft-com:xslt-debug");
			this.nameLast = this.f.QName("last", "urn:schemas-microsoft-com:xslt-debug");
			this.nameNamespaces = this.f.QName("namespaces", "urn:schemas-microsoft-com:xslt-debug");
			this.formatterCnt = 0;
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00035AAB File Offset: 0x00034AAB
		private bool IsDebug
		{
			get
			{
				return this.compiler.IsDebug;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x00035AB8 File Offset: 0x00034AB8
		private bool EvaluateFuncCalls
		{
			get
			{
				return !this.IsDebug;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00035AC3 File Offset: 0x00034AC3
		private bool InferXPathTypes
		{
			get
			{
				return !this.IsDebug;
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00035AD0 File Offset: 0x00034AD0
		private QilExpression Compile(Compiler compiler)
		{
			this.compiler = compiler;
			this.functions = this.f.FunctionList();
			this.extPars = this.f.GlobalParameterList();
			this.gloVars = this.f.GlobalVariableList();
			this.nsVars = this.f.GlobalVariableList();
			compiler.Scripts.CompileScripts();
			if (!this.IsDebug)
			{
				new XslAstAnalyzer().Analyze(compiler);
			}
			this.CreateGlobalVarPars();
			try
			{
				this.CompileKeys();
				this.CompileAndSortMatches(compiler.PrincipalStylesheet);
				this.PrecompileProtoTemplatesHeaders();
				this.CompileGlobalVariables();
				foreach (ProtoTemplate protoTemplate in compiler.AllTemplates)
				{
					this.CompileProtoTemplate(protoTemplate);
				}
			}
			catch (XslLoadException ex)
			{
				ex.SetSourceLineInfo(this.lastScope.SourceLine);
				throw;
			}
			catch (Exception ex2)
			{
				if (!XmlException.IsCatchableException(ex2))
				{
					throw;
				}
				throw new XslLoadException(ex2, this.lastScope.SourceLine);
			}
			QilNode qilNode = this.CompileRootExpression(compiler.StartApplyTemplates);
			foreach (ProtoTemplate protoTemplate2 in compiler.AllTemplates)
			{
				foreach (QilNode qilNode2 in protoTemplate2.Function.Arguments)
				{
					QilParameter qilParameter = (QilParameter)qilNode2;
					if (!this.IsDebug || qilParameter.Name.Equals(this.nameNamespaces))
					{
						qilParameter.DefaultValue = null;
					}
				}
			}
			Dictionary<string, Type> scriptClasses = compiler.Scripts.ScriptClasses;
			List<EarlyBoundInfo> list = new List<EarlyBoundInfo>(scriptClasses.Count);
			foreach (KeyValuePair<string, Type> keyValuePair in scriptClasses)
			{
				if (keyValuePair.Value != null)
				{
					list.Add(new EarlyBoundInfo(keyValuePair.Key, keyValuePair.Value));
				}
			}
			QilExpression qilExpression = this.f.QilExpression(qilNode, this.f.BaseFactory);
			qilExpression.EarlyBoundTypes = list;
			qilExpression.FunctionList = this.functions;
			qilExpression.GlobalParameterList = this.extPars;
			qilExpression.GlobalVariableList = this.gloVars;
			qilExpression.WhitespaceRules = compiler.WhitespaceRules;
			qilExpression.IsDebug = this.IsDebug;
			qilExpression.DefaultWriterSettings = compiler.Output.Settings;
			QilDepthChecker.Check(qilExpression);
			return qilExpression;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00035DA8 File Offset: 0x00034DA8
		private QilNode InvokeOnCurrentNodeChanged()
		{
			return this.f.Loop(this.f.Let(this.f.InvokeOnCurrentNodeChanged(this.curLoop.GetCurrent())), this.f.Sequence());
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00035DE1 File Offset: 0x00034DE1
		[Conditional("DEBUG")]
		private void CheckSingletonFocus()
		{
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00035DE4 File Offset: 0x00034DE4
		private QilNode CompileRootExpression(XslNode applyTmpls)
		{
			QilNode qilNode = this.f.Int32(0);
			if (this.formatNumberDynamicUsed || this.IsDebug)
			{
				foreach (DecimalFormatDecl decimalFormatDecl in this.compiler.DecimalFormats)
				{
					qilNode = this.f.Add(qilNode, this.f.InvokeRegisterDecimalFormat(decimalFormatDecl));
				}
			}
			foreach (string text in this.compiler.Scripts.ScriptClasses.Keys)
			{
				qilNode = this.f.Add(qilNode, this.f.InvokeCheckScriptNamespace(text));
			}
			this.singlFocus.SetFocus(SingletonFocusType.InitialContextNode);
			QilNode qilNode2 = this.GenerateApply(null, applyTmpls);
			this.singlFocus.SetFocus(null);
			if (qilNode.NodeType == QilNodeType.Add)
			{
				qilNode2 = this.f.Conditional(this.f.Eq(qilNode, this.f.Int32(0)), qilNode2, this.f.Sequence());
			}
			return this.f.DocumentCtor(qilNode2);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00035F38 File Offset: 0x00034F38
		private QilList EnterScope(XslNode node)
		{
			this.lastScope = node;
			this.xslVersion = node.XslVersion;
			this.scope.PushScope();
			bool flag = false;
			NsDecl nsDecl = node.Namespaces;
			while (nsDecl != null)
			{
				this.scope.AddNamespace(nsDecl.Prefix, nsDecl.NsUri);
				nsDecl = nsDecl.Prev;
				flag = true;
			}
			if (flag)
			{
				return this.BuildDebuggerNamespaces();
			}
			return null;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00035F9C File Offset: 0x00034F9C
		private void ExitScope()
		{
			this.scope.PopScope();
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00035FAC File Offset: 0x00034FAC
		private QilList BuildDebuggerNamespaces()
		{
			if (this.IsDebug)
			{
				QilList qilList = this.f.BaseFactory.Sequence();
				foreach (object obj in ((IEnumerable)this.scope))
				{
					CompilerScopeManager<QilIterator>.ScopeRecord scopeRecord = (CompilerScopeManager<QilIterator>.ScopeRecord)obj;
					qilList.Add(this.f.NamespaceDecl(this.f.String(scopeRecord.ncName), this.f.String(scopeRecord.nsUri)));
				}
				return qilList;
			}
			return null;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00036050 File Offset: 0x00035050
		private QilNode GetCurrentNode()
		{
			if (this.curLoop.IsFocusSet)
			{
				return this.curLoop.GetCurrent();
			}
			if (this.funcFocus.IsFocusSet)
			{
				return this.funcFocus.GetCurrent();
			}
			return this.singlFocus.GetCurrent();
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0003608F File Offset: 0x0003508F
		private QilNode GetCurrentPosition()
		{
			if (this.curLoop.IsFocusSet)
			{
				return this.curLoop.GetPosition();
			}
			if (this.funcFocus.IsFocusSet)
			{
				return this.funcFocus.GetPosition();
			}
			return this.singlFocus.GetPosition();
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x000360CE File Offset: 0x000350CE
		private QilNode GetLastPosition()
		{
			if (this.curLoop.IsFocusSet)
			{
				return this.curLoop.GetLast();
			}
			if (this.funcFocus.IsFocusSet)
			{
				return this.funcFocus.GetLast();
			}
			return this.singlFocus.GetLast();
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00036110 File Offset: 0x00035110
		private XmlQueryType ChooseBestType(VarPar var)
		{
			if (this.IsDebug || !this.InferXPathTypes)
			{
				return XmlQueryTypeFactory.ItemS;
			}
			XslFlags xslFlags = var.Flags & XslFlags.TypeFilter;
			if (xslFlags <= (XslFlags.Node | XslFlags.Nodeset))
			{
				if (xslFlags <= XslFlags.Node)
				{
					switch (xslFlags)
					{
					case XslFlags.String:
						return XmlQueryTypeFactory.StringX;
					case XslFlags.Number:
						return XmlQueryTypeFactory.DoubleX;
					case XslFlags.String | XslFlags.Number:
						break;
					case XslFlags.Boolean:
						return XmlQueryTypeFactory.BooleanX;
					default:
						if (xslFlags == XslFlags.Node)
						{
							return XmlQueryTypeFactory.NodeNotRtf;
						}
						break;
					}
				}
				else
				{
					if (xslFlags == XslFlags.Nodeset)
					{
						return XmlQueryTypeFactory.NodeNotRtfS;
					}
					if (xslFlags == (XslFlags.Node | XslFlags.Nodeset))
					{
						return XmlQueryTypeFactory.NodeNotRtfS;
					}
				}
			}
			else if (xslFlags <= (XslFlags.Node | XslFlags.Rtf))
			{
				if (xslFlags == XslFlags.Rtf)
				{
					return XmlQueryTypeFactory.Node;
				}
				if (xslFlags == (XslFlags.Node | XslFlags.Rtf))
				{
					return XmlQueryTypeFactory.Node;
				}
			}
			else
			{
				if (xslFlags == (XslFlags.Nodeset | XslFlags.Rtf))
				{
					return XmlQueryTypeFactory.NodeS;
				}
				if (xslFlags == (XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf))
				{
					return XmlQueryTypeFactory.NodeS;
				}
			}
			return XmlQueryTypeFactory.ItemS;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x000361D0 File Offset: 0x000351D0
		private QilIterator GetNsVar(QilList nsList)
		{
			foreach (QilNode qilNode in this.nsVars)
			{
				QilIterator qilIterator = (QilIterator)qilNode;
				QilList qilList = (QilList)qilIterator.Binding;
				if (qilList.Count == nsList.Count)
				{
					bool flag = true;
					for (int i = 0; i < nsList.Count; i++)
					{
						if (((QilLiteral)((QilBinary)nsList[i]).Right).Value != ((QilLiteral)((QilBinary)qilList[i]).Right).Value || ((QilLiteral)((QilBinary)nsList[i]).Left).Value != ((QilLiteral)((QilBinary)qilList[i]).Left).Value)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return qilIterator;
					}
				}
			}
			QilIterator qilIterator2 = this.f.Let(nsList);
			qilIterator2.DebugName = this.f.QName("ns" + this.nsVars.Count, "urn:schemas-microsoft-com:xslt-debug").ToString();
			this.gloVars.Add(qilIterator2);
			this.nsVars.Add(qilIterator2);
			return qilIterator2;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00036338 File Offset: 0x00035338
		private void PrecompileProtoTemplatesHeaders()
		{
			List<VarPar> list = null;
			Dictionary<VarPar, Template> dictionary = null;
			Dictionary<VarPar, QilFunction> dictionary2 = null;
			foreach (ProtoTemplate protoTemplate in this.compiler.AllTemplates)
			{
				QilList qilList = this.f.FormalParameterList();
				XslFlags xslFlags = ((!this.IsDebug) ? protoTemplate.Flags : XslFlags.FocusFilter);
				QilList qilList2 = this.EnterScope(protoTemplate);
				if ((xslFlags & XslFlags.Current) != XslFlags.None)
				{
					qilList.Add(this.CreateXslParam(this.CloneName(this.nameCurrent), XmlQueryTypeFactory.NodeNotRtf));
				}
				if ((xslFlags & XslFlags.Position) != XslFlags.None)
				{
					qilList.Add(this.CreateXslParam(this.CloneName(this.namePosition), XmlQueryTypeFactory.DoubleX));
				}
				if ((xslFlags & XslFlags.Last) != XslFlags.None)
				{
					qilList.Add(this.CreateXslParam(this.CloneName(this.nameLast), XmlQueryTypeFactory.DoubleX));
				}
				if (this.IsDebug && qilList2 != null)
				{
					QilParameter qilParameter = this.CreateXslParam(this.CloneName(this.nameNamespaces), XmlQueryTypeFactory.NamespaceS);
					qilParameter.DefaultValue = this.GetNsVar(qilList2);
					qilList.Add(qilParameter);
				}
				Template template = protoTemplate as Template;
				if (template != null)
				{
					this.funcFocus.StartFocus(qilList, xslFlags);
					for (int i = 0; i < protoTemplate.Content.Count; i++)
					{
						XslNode xslNode = protoTemplate.Content[i];
						if (xslNode.NodeType != XslNodeType.Text)
						{
							if (xslNode.NodeType != XslNodeType.Param)
							{
								break;
							}
							VarPar varPar = (VarPar)xslNode;
							this.EnterScope(varPar);
							if (this.scope.IsLocalVariable(varPar.Name.LocalName, varPar.Name.NamespaceUri))
							{
								this.ReportError("Xslt_DupLocalVariable", new string[] { varPar.Name.QualifiedName });
							}
							QilParameter qilParameter2 = this.CreateXslParam(varPar.Name, this.ChooseBestType(varPar));
							if (this.IsDebug)
							{
								qilParameter2.Annotation = varPar;
							}
							else if ((varPar.DefValueFlags & XslFlags.HasCalls) == XslFlags.None)
							{
								qilParameter2.DefaultValue = this.CompileVarParValue(varPar);
							}
							else
							{
								QilList qilList3 = this.f.FormalParameterList();
								QilList qilList4 = this.f.ActualParameterList();
								for (int j = 0; j < qilList.Count; j++)
								{
									QilParameter qilParameter3 = this.f.Parameter(qilList[j].XmlType);
									qilParameter3.DebugName = ((QilParameter)qilList[j]).DebugName;
									qilParameter3.Name = this.CloneName(((QilParameter)qilList[j]).Name);
									QilGenerator.SetLineInfo(qilParameter3, qilList[j].SourceLine);
									qilList3.Add(qilParameter3);
									qilList4.Add(qilList[j]);
								}
								varPar.Flags |= template.Flags & XslFlags.FocusFilter;
								QilFunction qilFunction = this.f.Function(qilList3, ((varPar.DefValueFlags & XslFlags.SideEffects) == XslFlags.None) ? this.f.False() : this.f.True(), this.ChooseBestType(varPar));
								qilFunction.SourceLine = SourceLineInfo.NoSource;
								qilFunction.DebugName = "<xsl:param name=\"" + varPar.Name.QualifiedName + "\">";
								qilParameter2.DefaultValue = this.f.Invoke(qilFunction, qilList4);
								if (list == null)
								{
									list = new List<VarPar>();
									dictionary = new Dictionary<VarPar, Template>();
									dictionary2 = new Dictionary<VarPar, QilFunction>();
								}
								list.Add(varPar);
								dictionary.Add(varPar, template);
								dictionary2.Add(varPar, qilFunction);
							}
							QilGenerator.SetLineInfo(qilParameter2, varPar.SourceLine);
							this.ExitScope();
							this.scope.AddVariable(varPar.Name, qilParameter2);
							qilList.Add(qilParameter2);
						}
					}
					this.funcFocus.StopFocus();
				}
				this.ExitScope();
				protoTemplate.Function = this.f.Function(qilList, ((protoTemplate.Flags & XslFlags.SideEffects) == XslFlags.None) ? this.f.False() : this.f.True(), (protoTemplate is AttributeSet) ? XmlQueryTypeFactory.AttributeS : XmlQueryTypeFactory.NodeNotRtfS);
				protoTemplate.Function.DebugName = protoTemplate.GetDebugName();
				QilGenerator.SetLineInfo(protoTemplate.Function, protoTemplate.SourceLine ?? SourceLineInfo.NoSource);
				this.functions.Add(protoTemplate.Function);
			}
			if (list != null)
			{
				foreach (VarPar varPar2 in list)
				{
					Template template2 = dictionary[varPar2];
					QilFunction qilFunction2 = dictionary2[varPar2];
					this.funcFocus.StartFocus(qilFunction2.Arguments, varPar2.Flags);
					this.EnterScope(template2);
					this.EnterScope(varPar2);
					foreach (QilNode qilNode in qilFunction2.Arguments)
					{
						QilParameter qilParameter4 = (QilParameter)qilNode;
						this.scope.AddVariable(qilParameter4.Name, qilParameter4);
					}
					qilFunction2.Definition = this.CompileVarParValue(varPar2);
					QilGenerator.SetLineInfo(qilFunction2.Definition, varPar2.SourceLine);
					this.ExitScope();
					this.ExitScope();
					this.funcFocus.StopFocus();
					this.functions.Add(qilFunction2);
				}
			}
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00036928 File Offset: 0x00035928
		private QilParameter CreateXslParam(QilName name, XmlQueryType xt)
		{
			QilParameter qilParameter = this.f.Parameter(xt);
			qilParameter.DebugName = name.ToString();
			qilParameter.Name = name;
			return qilParameter;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00036958 File Offset: 0x00035958
		private void CompileProtoTemplate(ProtoTemplate tmpl)
		{
			this.EnterScope(tmpl);
			this.funcFocus.StartFocus(tmpl.Function.Arguments, (!this.IsDebug) ? tmpl.Flags : XslFlags.FocusFilter);
			foreach (QilNode qilNode in tmpl.Function.Arguments)
			{
				QilParameter qilParameter = (QilParameter)qilNode;
				if (qilParameter.Name.NamespaceUri != "urn:schemas-microsoft-com:xslt-debug")
				{
					if (this.IsDebug)
					{
						VarPar varPar = (VarPar)qilParameter.Annotation;
						QilList qilList = this.EnterScope(varPar);
						qilParameter.DefaultValue = this.CompileVarParValue(varPar);
						this.ExitScope();
						qilParameter.DefaultValue = this.SetDebugNs(qilParameter.DefaultValue, qilList);
					}
					this.scope.AddVariable(qilParameter.Name, qilParameter);
				}
			}
			tmpl.Function.Definition = this.CompileInstructions(tmpl.Content);
			this.funcFocus.StopFocus();
			this.ExitScope();
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00036A70 File Offset: 0x00035A70
		private QilList InstructionList()
		{
			return this.f.BaseFactory.Sequence();
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00036A82 File Offset: 0x00035A82
		private QilNode CompileInstructions(IList<XslNode> instructions)
		{
			return this.CompileInstructions(instructions, 0, this.InstructionList());
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00036A92 File Offset: 0x00035A92
		private QilNode CompileInstructions(IList<XslNode> instructions, int from)
		{
			return this.CompileInstructions(instructions, from, this.InstructionList());
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00036AA2 File Offset: 0x00035AA2
		private QilNode CompileInstructions(IList<XslNode> instructions, QilList content)
		{
			return this.CompileInstructions(instructions, 0, content);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00036AB0 File Offset: 0x00035AB0
		private QilNode CompileInstructions(IList<XslNode> instructions, int from, QilList content)
		{
			for (int i = from; i < instructions.Count; i++)
			{
				XslNode xslNode = instructions[i];
				XslNodeType nodeType = xslNode.NodeType;
				if (nodeType != XslNodeType.Param)
				{
					QilList qilList = this.EnterScope(xslNode);
					QilNode qilNode;
					switch (nodeType)
					{
					case XslNodeType.ApplyImports:
						qilNode = this.CompileApplyImports(xslNode);
						break;
					case XslNodeType.ApplyTemplates:
						qilNode = this.CompileApplyTemplates((XslNodeEx)xslNode);
						break;
					case XslNodeType.Attribute:
						qilNode = this.CompileAttribute((NodeCtor)xslNode);
						break;
					case XslNodeType.AttributeSet:
					case XslNodeType.Key:
					case XslNodeType.Otherwise:
					case XslNodeType.Param:
					case XslNodeType.Sort:
					case XslNodeType.Template:
						goto IL_0201;
					case XslNodeType.CallTemplate:
						qilNode = this.CompileCallTemplate((XslNodeEx)xslNode);
						break;
					case XslNodeType.Choose:
						qilNode = this.CompileChoose(xslNode);
						break;
					case XslNodeType.Comment:
						qilNode = this.CompileComment(xslNode);
						break;
					case XslNodeType.Copy:
						qilNode = this.CompileCopy(xslNode);
						break;
					case XslNodeType.CopyOf:
						qilNode = this.CompileCopyOf(xslNode);
						break;
					case XslNodeType.Element:
						qilNode = this.CompileElement((NodeCtor)xslNode);
						break;
					case XslNodeType.Error:
						qilNode = this.CompileError(xslNode);
						break;
					case XslNodeType.ForEach:
						qilNode = this.CompileForEach((XslNodeEx)xslNode);
						break;
					case XslNodeType.If:
						qilNode = this.CompileIf(xslNode);
						break;
					case XslNodeType.List:
						qilNode = this.CompileList(xslNode);
						break;
					case XslNodeType.LiteralAttribute:
						qilNode = this.CompileLiteralAttribute(xslNode);
						break;
					case XslNodeType.LiteralElement:
						qilNode = this.CompileLiteralElement(xslNode);
						break;
					case XslNodeType.Message:
						qilNode = this.CompileMessage(xslNode);
						break;
					case XslNodeType.Nop:
						qilNode = this.CompileNop(xslNode);
						break;
					case XslNodeType.Number:
						qilNode = this.CompileNumber((Number)xslNode);
						break;
					case XslNodeType.PI:
						qilNode = this.CompilePI(xslNode);
						break;
					case XslNodeType.Text:
						qilNode = this.CompileText((Text)xslNode);
						break;
					case XslNodeType.UseAttributeSet:
						qilNode = this.CompileUseAttributeSet(xslNode);
						break;
					case XslNodeType.ValueOf:
						qilNode = this.CompileValueOf(xslNode);
						break;
					case XslNodeType.ValueOfDoe:
						qilNode = this.CompileValueOfDoe(xslNode);
						break;
					case XslNodeType.Variable:
						qilNode = this.CompileVariable(xslNode);
						break;
					default:
						goto IL_0201;
					}
					IL_0204:
					this.ExitScope();
					if (qilNode.NodeType != QilNodeType.Sequence || qilNode.Count != 0)
					{
						if (nodeType != XslNodeType.LiteralAttribute && nodeType != XslNodeType.UseAttributeSet)
						{
							this.SetLineInfoCheck(qilNode, xslNode.SourceLine);
						}
						qilNode = this.SetDebugNs(qilNode, qilList);
						if (nodeType == XslNodeType.Variable)
						{
							QilIterator qilIterator = this.f.Let(qilNode);
							qilIterator.DebugName = xslNode.Name.ToString();
							this.scope.AddVariable(xslNode.Name, qilIterator);
							qilNode = this.f.Loop(qilIterator, this.CompileInstructions(instructions, i + 1));
							i = instructions.Count;
						}
						content.Add(qilNode);
						goto IL_02A5;
					}
					goto IL_02A5;
					IL_0201:
					qilNode = null;
					goto IL_0204;
				}
				IL_02A5:;
			}
			if (!this.IsDebug && content.Count == 1)
			{
				return content[0];
			}
			return content;
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00036D8C File Offset: 0x00035D8C
		private QilNode CompileList(XslNode node)
		{
			return this.CompileInstructions(node.Content);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00036D9A File Offset: 0x00035D9A
		private QilNode CompileNop(XslNode node)
		{
			return this.f.Nop(this.f.Sequence());
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00036DB4 File Offset: 0x00035DB4
		private void AddNsDecl(QilList content, string prefix, string nsUri)
		{
			if (this.outputScope.LookupNamespace(prefix) == nsUri)
			{
				return;
			}
			this.outputScope.AddNamespace(prefix, nsUri);
			content.Add(this.f.NamespaceDecl(this.f.String(prefix), this.f.String(nsUri)));
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00036E0C File Offset: 0x00035E0C
		private QilNode CompileLiteralElement(XslNode node)
		{
			bool flag = true;
			QilName name;
			string text;
			string namespaceUri;
			QilList qilList;
			for (;;)
			{
				IL_0002:
				this.prefixesInUse.Clear();
				name = node.Name;
				text = name.Prefix;
				namespaceUri = name.NamespaceUri;
				this.compiler.ApplyNsAliases(ref text, ref namespaceUri);
				if (flag)
				{
					this.prefixesInUse.Add(text, namespaceUri);
				}
				else
				{
					text = name.Prefix;
				}
				this.outputScope.PushScope();
				qilList = this.InstructionList();
				foreach (object obj in ((IEnumerable)this.scope))
				{
					CompilerScopeManager<QilIterator>.ScopeRecord scopeRecord = (CompilerScopeManager<QilIterator>.ScopeRecord)obj;
					string text2 = scopeRecord.ncName;
					string nsUri = scopeRecord.nsUri;
					if (nsUri != "http://www.w3.org/1999/XSL/Transform" && !this.scope.IsExNamespace(nsUri))
					{
						this.compiler.ApplyNsAliases(ref text2, ref nsUri);
						if (flag)
						{
							if (this.prefixesInUse.Contains(text2))
							{
								if ((string)this.prefixesInUse[text2] != nsUri)
								{
									this.outputScope.PopScope();
									flag = false;
									goto IL_0002;
								}
							}
							else
							{
								this.prefixesInUse.Add(text2, nsUri);
							}
						}
						else
						{
							text2 = scopeRecord.ncName;
						}
						this.AddNsDecl(qilList, text2, nsUri);
					}
				}
				break;
			}
			QilNode qilNode = this.CompileInstructions(node.Content, qilList);
			this.outputScope.PopScope();
			name.Prefix = text;
			name.NamespaceUri = namespaceUri;
			return this.f.ElementCtor(name, qilNode);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00036FA8 File Offset: 0x00035FA8
		private QilNode CompileElement(NodeCtor node)
		{
			QilNode qilNode = this.CompileStringAvt(node.NsAvt);
			QilNode qilNode2 = this.CompileStringAvt(node.NameAvt);
			QilNode qilNode3;
			if (qilNode2.NodeType == QilNodeType.LiteralString && (qilNode == null || qilNode.NodeType == QilNodeType.LiteralString))
			{
				string text = (QilLiteral)qilNode2;
				string text2;
				string text3;
				bool flag = this.compiler.ParseQName(text, out text2, out text3, this);
				string text4;
				if (qilNode == null)
				{
					text4 = (flag ? this.ResolvePrefix(false, text2) : this.compiler.CreatePhantomNamespace());
				}
				else
				{
					text4 = (QilLiteral)qilNode;
				}
				qilNode3 = this.f.QName(text3, text4, text2);
			}
			else if (qilNode != null)
			{
				qilNode3 = this.f.StrParseQName(qilNode2, qilNode);
			}
			else
			{
				qilNode3 = this.ResolveQNameDynamic(false, qilNode2);
			}
			this.outputScope.PushScope();
			this.outputScope.InvalidateAllPrefixes();
			QilNode qilNode4 = this.CompileInstructions(node.Content);
			this.outputScope.PopScope();
			return this.f.ElementCtor(qilNode3, qilNode4);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x000370A0 File Offset: 0x000360A0
		private QilNode CompileLiteralAttribute(XslNode node)
		{
			QilName name = node.Name;
			string prefix = name.Prefix;
			string namespaceUri = name.NamespaceUri;
			if (prefix.Length != 0)
			{
				this.compiler.ApplyNsAliases(ref prefix, ref namespaceUri);
			}
			name.Prefix = prefix;
			name.NamespaceUri = namespaceUri;
			return this.f.AttributeCtor(name, this.CompileTextAvt(node.Select));
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00037100 File Offset: 0x00036100
		private QilNode CompileAttribute(NodeCtor node)
		{
			QilNode qilNode = this.CompileStringAvt(node.NsAvt);
			QilNode qilNode2 = this.CompileStringAvt(node.NameAvt);
			bool flag = false;
			QilNode qilNode3;
			if (qilNode2.NodeType == QilNodeType.LiteralString && (qilNode == null || qilNode.NodeType == QilNodeType.LiteralString))
			{
				string text = (QilLiteral)qilNode2;
				string text2;
				string text3;
				bool flag2 = this.compiler.ParseQName(text, out text2, out text3, this);
				string text4;
				if (qilNode == null)
				{
					text4 = (flag2 ? this.ResolvePrefix(true, text2) : this.compiler.CreatePhantomNamespace());
				}
				else
				{
					text4 = (QilLiteral)qilNode;
					flag = true;
				}
				if (text == "xmlns" || (text3 == "xmlns" && text4.Length == 0))
				{
					this.ReportError("Xslt_XmlnsAttr", new string[] { "name", text });
				}
				qilNode3 = this.f.QName(text3, text4, text2);
			}
			else if (qilNode != null)
			{
				qilNode3 = this.f.StrParseQName(qilNode2, qilNode);
			}
			else
			{
				qilNode3 = this.ResolveQNameDynamic(true, qilNode2);
			}
			if (flag)
			{
				this.outputScope.InvalidateNonDefaultPrefixes();
			}
			return this.f.AttributeCtor(qilNode3, this.CompileInstructions(node.Content));
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00037238 File Offset: 0x00036238
		private QilNode ExtractText(string source, ref int pos)
		{
			int num = pos;
			this.unescapedText.Length = 0;
			int i;
			for (i = pos; i < source.Length; i++)
			{
				char c = source[i];
				if (c == '{' || c == '}')
				{
					if (i + 1 < source.Length && source[i + 1] == c)
					{
						i++;
						this.unescapedText.Append(source, num, i - num);
						num = i + 1;
					}
					else
					{
						if (c == '{')
						{
							break;
						}
						pos = source.Length;
						if (this.xslVersion != XslVersion.ForwardsCompatible)
						{
							this.ReportError("Xslt_SingleRightBraceInAvt", new string[] { source });
							return null;
						}
						return this.f.Error(this.lastScope.SourceLine, "Xslt_SingleRightBraceInAvt", new string[] { source });
					}
				}
			}
			pos = i;
			if (this.unescapedText.Length != 0)
			{
				this.unescapedText.Append(source, num, i - num);
				return this.f.String(this.unescapedText.ToString());
			}
			if (i <= num)
			{
				return null;
			}
			return this.f.String(source.Substring(num, i - num));
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00037360 File Offset: 0x00036360
		private QilNode CompileAvt(string source)
		{
			QilList qilList = this.f.BaseFactory.Sequence();
			int i = 0;
			while (i < source.Length)
			{
				QilNode qilNode = this.ExtractText(source, ref i);
				if (qilNode != null)
				{
					qilList.Add(qilNode);
				}
				if (i < source.Length)
				{
					i++;
					QilNode qilNode2 = this.CompileXPathExpressionWithinAvt(source, ref i);
					qilList.Add(this.f.ConvertToString(qilNode2));
				}
			}
			if (qilList.Count == 1)
			{
				return qilList[0];
			}
			return qilList;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000373DA File Offset: 0x000363DA
		private QilNode CompileStringAvt(string avt)
		{
			if (avt == null)
			{
				return null;
			}
			if (avt.IndexOfAny(QilGenerator.curlyBraces) == -1)
			{
				return this.f.String(avt);
			}
			return this.f.StrConcat(this.CompileAvt(avt));
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00037410 File Offset: 0x00036410
		private QilNode CompileTextAvt(string avt)
		{
			if (avt.IndexOfAny(QilGenerator.curlyBraces) == -1)
			{
				return this.f.TextCtor(this.f.String(avt));
			}
			QilNode qilNode = this.CompileAvt(avt);
			if (qilNode.NodeType == QilNodeType.Sequence)
			{
				QilList qilList = this.InstructionList();
				foreach (QilNode qilNode2 in qilNode)
				{
					qilList.Add(this.f.TextCtor(qilNode2));
				}
				return qilList;
			}
			return this.f.TextCtor(qilNode);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x000374B0 File Offset: 0x000364B0
		private QilNode CompileText(Text node)
		{
			if (node.Hints == SerializationHints.None)
			{
				return this.f.TextCtor(this.f.String(node.Select));
			}
			return this.f.RawTextCtor(this.f.String(node.Select));
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00037500 File Offset: 0x00036500
		private QilNode CompilePI(XslNode node)
		{
			QilNode qilNode = this.CompileStringAvt(node.Select);
			if (qilNode.NodeType == QilNodeType.LiteralString)
			{
				string text = (QilLiteral)qilNode;
				this.compiler.ValidatePiName(text, this);
			}
			return this.f.PICtor(qilNode, this.CompileInstructions(node.Content));
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00037555 File Offset: 0x00036555
		private QilNode CompileComment(XslNode node)
		{
			return this.f.CommentCtor(this.CompileInstructions(node.Content));
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0003756E File Offset: 0x0003656E
		private QilNode CompileError(XslNode node)
		{
			return this.f.Error(this.f.String(node.Select));
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0003758C File Offset: 0x0003658C
		private QilNode WrapLoopBody(ISourceLineInfo before, QilNode expr, ISourceLineInfo after)
		{
			if (this.IsDebug)
			{
				return this.f.Sequence(new QilNode[]
				{
					QilGenerator.SetLineInfo(this.InvokeOnCurrentNodeChanged(), before),
					expr,
					QilGenerator.SetLineInfo(this.f.Nop(this.f.Sequence()), after)
				});
			}
			return expr;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000375E8 File Offset: 0x000365E8
		private QilNode CompileForEach(XslNodeEx node)
		{
			IList<XslNode> content = node.Content;
			LoopFocus loopFocus = this.curLoop;
			QilIterator qilIterator = this.f.For(this.CompileNodeSetExpression(node.Select));
			this.curLoop.SetFocus(qilIterator);
			int num = this.varHelper.StartVariables();
			this.curLoop.Sort(this.CompileSorts(content, ref loopFocus));
			QilNode qilNode = this.CompileInstructions(content);
			qilNode = this.WrapLoopBody(node.ElemNameLi, qilNode, node.EndTagLi);
			qilNode = this.AddCurrentPositionLast(qilNode);
			qilNode = this.curLoop.ConstructLoop(qilNode);
			qilNode = this.varHelper.FinishVariables(qilNode, num);
			this.curLoop = loopFocus;
			return qilNode;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00037690 File Offset: 0x00036690
		private QilNode CompileApplyTemplates(XslNodeEx node)
		{
			IList<XslNode> content = node.Content;
			int num = this.varHelper.StartVariables();
			QilIterator qilIterator = this.f.Let(this.CompileNodeSetExpression(node.Select));
			this.varHelper.AddVariable(qilIterator);
			for (int i = 0; i < content.Count; i++)
			{
				VarPar varPar = content[i] as VarPar;
				if (varPar != null)
				{
					this.CompileWithParam(varPar);
					QilNode value = varPar.Value;
					if (this.IsDebug || (!(value is QilIterator) && !(value is QilLiteral)))
					{
						QilIterator qilIterator2 = this.f.Let(value);
						qilIterator2.DebugName = this.f.QName("with-param " + varPar.Name.QualifiedName, "urn:schemas-microsoft-com:xslt-debug").ToString();
						this.varHelper.AddVariable(qilIterator2);
						varPar.Value = qilIterator2;
					}
				}
			}
			LoopFocus loopFocus = this.curLoop;
			QilIterator qilIterator3 = this.f.For(qilIterator);
			this.curLoop.SetFocus(qilIterator3);
			this.curLoop.Sort(this.CompileSorts(content, ref loopFocus));
			QilNode qilNode = this.GenerateApply(null, node);
			qilNode = this.WrapLoopBody(node.ElemNameLi, qilNode, node.EndTagLi);
			qilNode = this.AddCurrentPositionLast(qilNode);
			qilNode = this.curLoop.ConstructLoop(qilNode);
			this.curLoop = loopFocus;
			return this.varHelper.FinishVariables(qilNode, num);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00037808 File Offset: 0x00036808
		private QilNode CompileNodeSetExpression(string expr)
		{
			QilNode qilNode = this.CompileXPathExpression(expr);
			if (this.f.CannotBeNodeSet(qilNode))
			{
				XPathCompileException ex = new XPathCompileException(expr, 0, expr.Length, "XPath_NodeSetExpected", null);
				if (this.xslVersion != XslVersion.ForwardsCompatible)
				{
					this.ReportErrorInXPath(ex);
				}
				return this.f.Error(this.f.String(ex.Message));
			}
			return this.f.EnsureNodeSet(qilNode);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00037878 File Offset: 0x00036878
		private QilNode CompileApplyImports(XslNode node)
		{
			return this.GenerateApply((Stylesheet)node.Arg, node);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0003788C File Offset: 0x0003688C
		private QilNode CompileCallTemplate(XslNodeEx node)
		{
			int num = this.varHelper.StartVariables();
			IList<XslNode> content = node.Content;
			foreach (XslNode xslNode in content)
			{
				VarPar varPar = (VarPar)xslNode;
				this.CompileWithParam(varPar);
				if (this.IsDebug)
				{
					QilNode value = varPar.Value;
					QilIterator qilIterator = this.f.Let(value);
					qilIterator.DebugName = this.f.QName("with-param " + varPar.Name.QualifiedName, "urn:schemas-microsoft-com:xslt-debug").ToString();
					this.varHelper.AddVariable(qilIterator);
					varPar.Value = qilIterator;
				}
			}
			Template template;
			QilNode qilNode;
			if (this.compiler.NamedTemplates.TryGetValue(node.Name, out template))
			{
				qilNode = this.GenerateCall(template.Function, node);
			}
			else
			{
				if (!this.compiler.IsPhantomName(node.Name))
				{
					this.compiler.ReportError(node.SourceLine, "Xslt_InvalidCallTemplate", new string[] { node.Name.QualifiedName });
				}
				qilNode = this.f.Sequence();
			}
			if (content.Count > 0)
			{
				qilNode = QilGenerator.SetLineInfo(qilNode, node.ElemNameLi);
			}
			qilNode = this.varHelper.FinishVariables(qilNode, num);
			if (this.IsDebug)
			{
				return this.f.Nop(qilNode);
			}
			return qilNode;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00037A10 File Offset: 0x00036A10
		private QilNode CompileUseAttributeSet(XslNode node)
		{
			this.outputScope.InvalidateAllPrefixes();
			AttributeSet attributeSet;
			if (this.compiler.AttributeSets.TryGetValue(node.Name, out attributeSet))
			{
				return this.GenerateCall(attributeSet.Function, node);
			}
			if (!this.compiler.IsPhantomName(node.Name))
			{
				this.compiler.ReportError(node.SourceLine, "Xslt_NoAttributeSet", new string[] { node.Name.QualifiedName });
			}
			return this.f.Sequence();
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00037A9C File Offset: 0x00036A9C
		private QilNode CompileCopy(XslNode copy)
		{
			QilNode currentNode = this.GetCurrentNode();
			if ((currentNode.XmlType.NodeKinds & (XmlNodeKindFlags.Attribute | XmlNodeKindFlags.Namespace)) != XmlNodeKindFlags.None)
			{
				this.outputScope.InvalidateAllPrefixes();
			}
			if (currentNode.XmlType.NodeKinds == XmlNodeKindFlags.Element)
			{
				QilList qilList = this.InstructionList();
				qilList.Add(this.f.XPathNamespace(currentNode));
				this.outputScope.PushScope();
				this.outputScope.InvalidateAllPrefixes();
				QilNode qilNode = this.CompileInstructions(copy.Content, qilList);
				this.outputScope.PopScope();
				return this.f.ElementCtor(this.f.NameOf(currentNode), qilNode);
			}
			if (currentNode.XmlType.NodeKinds == XmlNodeKindFlags.Document)
			{
				return this.CompileInstructions(copy.Content);
			}
			if ((currentNode.XmlType.NodeKinds & (XmlNodeKindFlags.Document | XmlNodeKindFlags.Element)) == XmlNodeKindFlags.None)
			{
				return currentNode;
			}
			return this.f.XsltCopy(currentNode, this.CompileInstructions(copy.Content));
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00037B80 File Offset: 0x00036B80
		private QilNode CompileCopyOf(XslNode node)
		{
			QilNode qilNode = this.CompileXPathExpression(node.Select);
			if (qilNode.XmlType.IsNode)
			{
				if ((qilNode.XmlType.NodeKinds & (XmlNodeKindFlags.Attribute | XmlNodeKindFlags.Namespace)) != XmlNodeKindFlags.None)
				{
					this.outputScope.InvalidateAllPrefixes();
				}
				if (qilNode.XmlType.IsNotRtf && (qilNode.XmlType.NodeKinds & XmlNodeKindFlags.Document) == XmlNodeKindFlags.None)
				{
					return qilNode;
				}
				if (qilNode.XmlType.IsSingleton)
				{
					return this.f.XsltCopyOf(qilNode);
				}
				QilIterator qilIterator;
				return this.f.Loop(qilIterator = this.f.For(qilNode), this.f.XsltCopyOf(qilIterator));
			}
			else
			{
				if (qilNode.XmlType.IsAtomicValue)
				{
					return this.f.TextCtor(this.f.ConvertToString(qilNode));
				}
				this.outputScope.InvalidateAllPrefixes();
				QilIterator qilIterator2;
				return this.f.Loop(qilIterator2 = this.f.For(qilNode), this.f.Conditional(this.f.IsType(qilIterator2, XmlQueryTypeFactory.Node), this.f.XsltCopyOf(this.f.TypeAssert(qilIterator2, XmlQueryTypeFactory.Node)), this.f.TextCtor(this.f.XsltConvert(qilIterator2, XmlQueryTypeFactory.StringX))));
			}
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00037CBD File Offset: 0x00036CBD
		private QilNode CompileValueOf(XslNode valueOf)
		{
			return this.f.TextCtor(this.f.ConvertToString(this.CompileXPathExpression(valueOf.Select)));
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00037CE1 File Offset: 0x00036CE1
		private QilNode CompileValueOfDoe(XslNode valueOf)
		{
			return this.f.RawTextCtor(this.f.ConvertToString(this.CompileXPathExpression(valueOf.Select)));
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00037D05 File Offset: 0x00036D05
		private QilNode CompileWhen(XslNode whenNode, QilNode otherwise)
		{
			return this.f.Conditional(this.f.ConvertToBoolean(this.CompileXPathExpression(whenNode.Select)), this.CompileInstructions(whenNode.Content), otherwise);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00037D36 File Offset: 0x00036D36
		private QilNode CompileIf(XslNode ifNode)
		{
			return this.CompileWhen(ifNode, this.InstructionList());
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00037D48 File Offset: 0x00036D48
		private QilNode CompileChoose(XslNode node)
		{
			IList<XslNode> content = node.Content;
			QilNode qilNode = null;
			int num = content.Count - 1;
			while (0 <= num)
			{
				XslNode xslNode = content[num];
				QilList qilList = this.EnterScope(xslNode);
				if (xslNode.NodeType == XslNodeType.Otherwise)
				{
					qilNode = this.CompileInstructions(xslNode.Content);
				}
				else
				{
					qilNode = this.CompileWhen(xslNode, qilNode ?? this.InstructionList());
				}
				this.ExitScope();
				this.SetLineInfoCheck(qilNode, xslNode.SourceLine);
				qilNode = this.SetDebugNs(qilNode, qilList);
				num--;
			}
			if (qilNode == null)
			{
				return this.f.Sequence();
			}
			if (!this.IsDebug)
			{
				return qilNode;
			}
			return this.f.Sequence(qilNode);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00037DF0 File Offset: 0x00036DF0
		private QilNode CompileMessage(XslNode node)
		{
			string uri = this.lastScope.SourceLine.Uri;
			QilNode qilNode = this.f.RtfCtor(this.CompileInstructions(node.Content), this.f.String(uri));
			qilNode = this.f.InvokeOuterXml(qilNode);
			if (!(bool)node.Arg)
			{
				return this.f.Warning(qilNode);
			}
			QilIterator qilIterator;
			return this.f.Loop(qilIterator = this.f.Let(qilNode), this.f.Sequence(this.f.Warning(qilIterator), this.f.Error(qilIterator)));
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00037E98 File Offset: 0x00036E98
		private QilNode CompileVariable(XslNode node)
		{
			if (this.scope.IsLocalVariable(node.Name.LocalName, node.Name.NamespaceUri))
			{
				this.ReportError("Xslt_DupLocalVariable", new string[] { node.Name.QualifiedName });
			}
			return this.CompileVarParValue(node);
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00037EF0 File Offset: 0x00036EF0
		private QilNode CompileVarParValue(XslNode node)
		{
			string uri = this.lastScope.SourceLine.Uri;
			IList<XslNode> content = node.Content;
			string select = node.Select;
			QilNode qilNode;
			if (select != null)
			{
				QilList qilList = this.InstructionList();
				qilList.Add(this.CompileXPathExpression(select));
				qilNode = this.CompileInstructions(content, qilList);
			}
			else if (content.Count != 0)
			{
				this.outputScope.PushScope();
				this.outputScope.InvalidateAllPrefixes();
				qilNode = this.f.RtfCtor(this.CompileInstructions(content), this.f.String(uri));
				this.outputScope.PopScope();
			}
			else
			{
				qilNode = this.f.String(string.Empty);
			}
			if (this.IsDebug)
			{
				qilNode = this.f.TypeAssert(qilNode, XmlQueryTypeFactory.ItemS);
			}
			return qilNode;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00037FB8 File Offset: 0x00036FB8
		private void CompileWithParam(VarPar withParam)
		{
			QilList qilList = this.EnterScope(withParam);
			QilNode qilNode = this.CompileVarParValue(withParam);
			this.ExitScope();
			QilGenerator.SetLineInfo(qilNode, withParam.SourceLine);
			qilNode = this.SetDebugNs(qilNode, qilList);
			withParam.Value = qilNode;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00037FF8 File Offset: 0x00036FF8
		private QilNode CompileSorts(IList<XslNode> content, ref LoopFocus parentLoop)
		{
			QilList qilList = this.f.BaseFactory.SortKeyList();
			int i = 0;
			while (i < content.Count)
			{
				Sort sort = content[i] as Sort;
				if (sort != null)
				{
					this.CompileSort(sort, qilList, ref parentLoop);
					content.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			if (qilList.Count == 0)
			{
				return null;
			}
			return qilList;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00038054 File Offset: 0x00037054
		private QilNode CompileLangAttribute(string attValue, bool fwdCompat)
		{
			QilNode qilNode = this.CompileStringAvt(attValue);
			if (qilNode != null)
			{
				if (qilNode.NodeType == QilNodeType.LiteralString)
				{
					string text = (QilLiteral)qilNode;
					int num = XsltLibrary.LangToLcidInternal(text, fwdCompat, this);
					if (num == 127)
					{
						qilNode = null;
					}
				}
				else
				{
					QilIterator qilIterator;
					qilNode = this.f.Loop(qilIterator = this.f.Let(qilNode), this.f.Conditional(this.f.Eq(this.f.InvokeLangToLcid(qilIterator, fwdCompat), this.f.Int32(127)), this.f.String(string.Empty), qilIterator));
				}
			}
			return qilNode;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x000380F3 File Offset: 0x000370F3
		private QilNode CompileLangAttributeToLcid(string attValue, bool fwdCompat)
		{
			return this.CompileLangToLcid(this.CompileStringAvt(attValue), fwdCompat);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00038104 File Offset: 0x00037104
		private QilNode CompileLangToLcid(QilNode lang, bool fwdCompat)
		{
			if (lang == null)
			{
				return this.f.Double(127.0);
			}
			if (lang.NodeType == QilNodeType.LiteralString)
			{
				return this.f.Double((double)XsltLibrary.LangToLcidInternal((QilLiteral)lang, fwdCompat, this));
			}
			return this.f.XsltConvert(this.f.InvokeLangToLcid(lang, fwdCompat), XmlQueryTypeFactory.DoubleX);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00038170 File Offset: 0x00037170
		private void CompileDataTypeAttribute(string attValue, bool fwdCompat, ref QilNode select, out QilNode select2)
		{
			QilNode qilNode = this.CompileStringAvt(attValue);
			if (qilNode != null)
			{
				if (qilNode.NodeType != QilNodeType.LiteralString)
				{
					QilIterator qilIterator;
					qilNode = this.f.Loop(qilIterator = this.f.Let(qilNode), this.f.Conditional(this.f.Eq(qilIterator, this.f.String("number")), this.f.False(), this.f.Conditional(this.f.Eq(qilIterator, this.f.String("text")), this.f.True(), fwdCompat ? this.f.True() : this.f.Loop(this.f.Let(this.ResolveQNameDynamic(true, qilIterator)), this.f.Error(this.lastScope.SourceLine, "Xslt_BistateAttribute", new string[] { "data-type", "text", "number" })))));
					QilIterator qilIterator2 = this.f.Let(qilNode);
					this.varHelper.AddVariable(qilIterator2);
					select2 = select.DeepClone(this.f.BaseFactory);
					select = this.f.Conditional(qilIterator2, this.f.ConvertToString(select), this.f.String(string.Empty));
					select2 = this.f.Conditional(qilIterator2, this.f.Double(0.0), this.f.ConvertToNumber(select2));
					return;
				}
				string text = (QilLiteral)qilNode;
				if (text == "number")
				{
					select = this.f.ConvertToNumber(select);
					select2 = null;
					return;
				}
				if (!(text == "text") && !fwdCompat)
				{
					string text3;
					string text4;
					string text2 = (this.compiler.ParseQName(text, out text3, out text4, this) ? this.ResolvePrefix(true, text3) : this.compiler.CreatePhantomNamespace());
					int length = text2.Length;
					this.ReportError("Xslt_BistateAttribute", new string[] { "data-type", "text", "number" });
				}
			}
			select = this.f.ConvertToString(select);
			select2 = null;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x000383D8 File Offset: 0x000373D8
		private QilNode CompileOrderAttribute(string attName, string attValue, string value0, string value1, bool fwdCompat)
		{
			QilNode qilNode = this.CompileStringAvt(attValue);
			if (qilNode != null)
			{
				if (qilNode.NodeType == QilNodeType.LiteralString)
				{
					string text = (QilLiteral)qilNode;
					if (text == value1)
					{
						qilNode = this.f.String("1");
					}
					else
					{
						if (text != value0 && !fwdCompat)
						{
							this.ReportError("Xslt_BistateAttribute", new string[] { attName, value0, value1 });
						}
						qilNode = this.f.String("0");
					}
				}
				else
				{
					QilIterator qilIterator;
					qilNode = this.f.Loop(qilIterator = this.f.Let(qilNode), this.f.Conditional(this.f.Eq(qilIterator, this.f.String(value1)), this.f.String("1"), fwdCompat ? this.f.String("0") : this.f.Conditional(this.f.Eq(qilIterator, this.f.String(value0)), this.f.String("0"), this.f.Error(this.lastScope.SourceLine, "Xslt_BistateAttribute", new string[] { attName, value0, value1 }))));
				}
			}
			return qilNode;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0003853C File Offset: 0x0003753C
		private void CompileSort(Sort sort, QilList keyList, ref LoopFocus parentLoop)
		{
			this.EnterScope(sort);
			bool forwardsCompatible = sort.ForwardsCompatible;
			QilNode qilNode = this.CompileXPathExpression(sort.Select);
			QilNode qilNode2;
			QilNode qilNode3;
			QilNode qilNode4;
			QilNode qilNode5;
			if (sort.Lang != null || sort.DataType != null || sort.Order != null || sort.CaseOrder != null)
			{
				LoopFocus loopFocus = this.curLoop;
				this.curLoop = parentLoop;
				qilNode2 = this.CompileLangAttribute(sort.Lang, forwardsCompatible);
				this.CompileDataTypeAttribute(sort.DataType, forwardsCompatible, ref qilNode, out qilNode3);
				qilNode4 = this.CompileOrderAttribute("order", sort.Order, "ascending", "descending", forwardsCompatible);
				qilNode5 = this.CompileOrderAttribute("case-order", sort.CaseOrder, "lower-first", "upper-first", forwardsCompatible);
				this.curLoop = loopFocus;
			}
			else
			{
				qilNode = this.f.ConvertToString(qilNode);
				qilNode2 = (qilNode3 = (qilNode4 = (qilNode5 = null)));
			}
			this.strConcat.Reset();
			this.strConcat.Append("http://collations.microsoft.com");
			this.strConcat.Append('/');
			this.strConcat.Append(qilNode2);
			char c = '?';
			if (qilNode4 != null)
			{
				this.strConcat.Append(c);
				this.strConcat.Append("descendingOrder=");
				this.strConcat.Append(qilNode4);
				c = '&';
			}
			if (qilNode5 != null)
			{
				this.strConcat.Append(c);
				this.strConcat.Append("upperFirst=");
				this.strConcat.Append(qilNode5);
			}
			QilNode qilNode6 = this.strConcat.ToQil();
			QilSortKey qilSortKey = this.f.SortKey(qilNode, qilNode6);
			keyList.Add(qilSortKey);
			if (qilNode3 != null)
			{
				qilSortKey = this.f.SortKey(qilNode3, qilNode6.DeepClone(this.f.BaseFactory));
				keyList.Add(qilSortKey);
			}
			this.ExitScope();
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00038708 File Offset: 0x00037708
		private QilNode MatchPattern(QilNode pattern, QilIterator testNode)
		{
			if (pattern.NodeType == QilNodeType.Error)
			{
				return pattern;
			}
			QilList qilList;
			if (pattern.NodeType == QilNodeType.Sequence)
			{
				qilList = (QilList)pattern;
			}
			else
			{
				qilList = this.f.BaseFactory.Sequence();
				qilList.Add(pattern);
			}
			QilNode qilNode = this.f.False();
			int num = qilList.Count - 1;
			while (0 <= num)
			{
				QilLoop qilLoop = (QilLoop)qilList[num];
				qilNode = this.f.Or(this.refReplacer.Replace(qilLoop.Body, qilLoop.Variable, testNode), qilNode);
				num--;
			}
			return qilNode;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x000387A0 File Offset: 0x000377A0
		private QilNode MatchCountPattern(QilNode countPattern, QilIterator testNode)
		{
			if (countPattern != null)
			{
				return this.MatchPattern(countPattern, testNode);
			}
			QilNode currentNode = this.GetCurrentNode();
			XmlNodeKindFlags nodeKinds = currentNode.XmlType.NodeKinds;
			if ((nodeKinds & (nodeKinds - 1)) != XmlNodeKindFlags.None)
			{
				return this.f.InvokeIsSameNodeSort(testNode, currentNode);
			}
			XmlNodeKindFlags xmlNodeKindFlags = nodeKinds;
			if (xmlNodeKindFlags <= XmlNodeKindFlags.Text)
			{
				QilNode qilNode;
				switch (xmlNodeKindFlags)
				{
				case XmlNodeKindFlags.Document:
					return this.f.IsType(testNode, XmlQueryTypeFactory.Document);
				case XmlNodeKindFlags.Element:
					qilNode = this.f.IsType(testNode, XmlQueryTypeFactory.Element);
					break;
				case XmlNodeKindFlags.Document | XmlNodeKindFlags.Element:
					goto IL_0156;
				case XmlNodeKindFlags.Attribute:
					qilNode = this.f.IsType(testNode, XmlQueryTypeFactory.Attribute);
					break;
				default:
					if (xmlNodeKindFlags != XmlNodeKindFlags.Text)
					{
						goto IL_0156;
					}
					return this.f.IsType(testNode, XmlQueryTypeFactory.Text);
				}
				return this.f.And(qilNode, this.f.And(this.f.Eq(this.f.LocalNameOf(testNode), this.f.LocalNameOf(currentNode)), this.f.Eq(this.f.NamespaceUriOf(testNode), this.f.NamespaceUriOf(this.GetCurrentNode()))));
			}
			if (xmlNodeKindFlags == XmlNodeKindFlags.Comment)
			{
				return this.f.IsType(testNode, XmlQueryTypeFactory.Comment);
			}
			if (xmlNodeKindFlags == XmlNodeKindFlags.PI)
			{
				return this.f.And(this.f.IsType(testNode, XmlQueryTypeFactory.PI), this.f.Eq(this.f.LocalNameOf(testNode), this.f.LocalNameOf(currentNode)));
			}
			if (xmlNodeKindFlags == XmlNodeKindFlags.Namespace)
			{
				return this.f.And(this.f.IsType(testNode, XmlQueryTypeFactory.Namespace), this.f.Eq(this.f.LocalNameOf(testNode), this.f.LocalNameOf(currentNode)));
			}
			IL_0156:
			return this.f.False();
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00038974 File Offset: 0x00037974
		private QilNode PlaceMarker(QilNode countPattern, QilNode fromPattern, bool multiple)
		{
			QilNode qilNode = ((countPattern != null) ? countPattern.DeepClone(this.f.BaseFactory) : null);
			QilIterator qilIterator;
			QilNode qilNode2 = this.f.Filter(qilIterator = this.f.For(this.f.AncestorOrSelf(this.GetCurrentNode())), this.MatchCountPattern(countPattern, qilIterator));
			QilNode qilNode3;
			if (multiple)
			{
				qilNode3 = this.f.DocOrderDistinct(qilNode2);
			}
			else
			{
				qilNode3 = this.f.Filter(qilIterator = this.f.For(qilNode2), this.f.Eq(this.f.PositionOf(qilIterator), this.f.Int32(1)));
			}
			QilNode qilNode4;
			QilIterator qilIterator2;
			if (fromPattern == null)
			{
				qilNode4 = qilNode3;
			}
			else
			{
				QilNode qilNode5 = this.f.Filter(qilIterator = this.f.For(this.f.AncestorOrSelf(this.GetCurrentNode())), this.MatchPattern(fromPattern, qilIterator));
				QilNode qilNode6 = this.f.Filter(qilIterator = this.f.For(qilNode5), this.f.Eq(this.f.PositionOf(qilIterator), this.f.Int32(1)));
				qilNode4 = this.f.Loop(qilIterator = this.f.For(qilNode6), this.f.Filter(qilIterator2 = this.f.For(qilNode3), this.f.Before(qilIterator, qilIterator2)));
			}
			return this.f.Loop(qilIterator2 = this.f.For(qilNode4), this.f.Add(this.f.Int32(1), this.f.Length(this.f.Filter(qilIterator = this.f.For(this.f.PrecedingSibling(qilIterator2)), this.MatchCountPattern(qilNode, qilIterator)))));
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00038B50 File Offset: 0x00037B50
		private QilNode PlaceMarkerAny(QilNode countPattern, QilNode fromPattern)
		{
			QilNode qilNode2;
			QilIterator qilIterator3;
			if (fromPattern == null)
			{
				QilNode qilNode = this.f.NodeRange(this.f.Root(this.GetCurrentNode()), this.GetCurrentNode());
				QilIterator qilIterator;
				qilNode2 = this.f.Filter(qilIterator = this.f.For(qilNode), this.MatchCountPattern(countPattern, qilIterator));
			}
			else
			{
				QilIterator qilIterator;
				QilNode qilNode3 = this.f.Filter(qilIterator = this.f.For(this.f.Preceding(this.GetCurrentNode())), this.MatchPattern(fromPattern, qilIterator));
				QilNode qilNode4 = this.f.Filter(qilIterator = this.f.For(qilNode3), this.f.Eq(this.f.PositionOf(qilIterator), this.f.Int32(1)));
				QilIterator qilIterator2;
				qilNode2 = this.f.Loop(qilIterator = this.f.For(qilNode4), this.f.Filter(qilIterator2 = this.f.For(this.f.Filter(qilIterator3 = this.f.For(this.f.NodeRange(qilIterator, this.GetCurrentNode())), this.MatchCountPattern(countPattern, qilIterator3))), this.f.Not(this.f.Is(qilIterator, qilIterator2))));
			}
			return this.f.Loop(qilIterator3 = this.f.Let(this.f.Length(qilNode2)), this.f.Conditional(this.f.Eq(qilIterator3, this.f.Int32(0)), this.f.Sequence(), qilIterator3));
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00038CF8 File Offset: 0x00037CF8
		private QilNode CompileLetterValueAttribute(string attValue, bool fwdCompat)
		{
			QilNode qilNode = this.CompileStringAvt(attValue);
			if (qilNode == null)
			{
				return this.f.String("default");
			}
			if (qilNode.NodeType == QilNodeType.LiteralString)
			{
				string text = (QilLiteral)qilNode;
				if (text != "alphabetic" && text != "traditional")
				{
					if (fwdCompat)
					{
						return this.f.String("default");
					}
					this.ReportError("Xslt_BistateAttribute", new string[] { "letter-value", "alphabetic", "traditional" });
				}
				return qilNode;
			}
			QilIterator qilIterator = this.f.Let(qilNode);
			return this.f.Loop(qilIterator, this.f.Conditional(this.f.Or(this.f.Eq(qilIterator, this.f.String("alphabetic")), this.f.Eq(qilIterator, this.f.String("traditional"))), qilIterator, fwdCompat ? this.f.String("default") : this.f.Error(this.lastScope.SourceLine, "Xslt_BistateAttribute", new string[] { "letter-value", "alphabetic", "traditional" })));
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00038E54 File Offset: 0x00037E54
		private QilNode CompileGroupingSeparatorAttribute(string attValue, bool fwdCompat)
		{
			QilNode qilNode = this.CompileStringAvt(attValue);
			if (qilNode == null)
			{
				qilNode = this.f.String(string.Empty);
			}
			else if (qilNode.NodeType == QilNodeType.LiteralString)
			{
				string text = (QilLiteral)qilNode;
				if (text.Length != 1)
				{
					if (!fwdCompat)
					{
						this.ReportError("Xslt_CharAttribute", new string[] { "grouping-separator" });
					}
					qilNode = this.f.String(string.Empty);
				}
			}
			else
			{
				QilIterator qilIterator = this.f.Let(qilNode);
				qilNode = this.f.Loop(qilIterator, this.f.Conditional(this.f.Eq(this.f.StrLength(qilIterator), this.f.Int32(1)), qilIterator, fwdCompat ? this.f.String(string.Empty) : this.f.Error(this.lastScope.SourceLine, "Xslt_CharAttribute", new string[] { "grouping-separator" })));
			}
			return qilNode;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00038F68 File Offset: 0x00037F68
		private QilNode CompileGroupingSizeAttribute(string attValue, bool fwdCompat)
		{
			QilNode qilNode = this.CompileStringAvt(attValue);
			if (qilNode == null)
			{
				return this.f.Double(0.0);
			}
			if (qilNode.NodeType != QilNodeType.LiteralString)
			{
				QilIterator qilIterator = this.f.Let(this.f.ConvertToNumber(qilNode));
				return this.f.Loop(qilIterator, this.f.Conditional(this.f.And(this.f.Lt(this.f.Double(0.0), qilIterator), this.f.Lt(qilIterator, this.f.Double(2147483647.0))), qilIterator, this.f.Double(0.0)));
			}
			string text = (QilLiteral)qilNode;
			double num = XsltFunctions.Round(XPathConvert.StringToDouble(text));
			if (0.0 <= num && num <= 2147483647.0)
			{
				return this.f.Double(num);
			}
			return this.f.Double(0.0);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00039080 File Offset: 0x00038080
		private QilNode CompileNumber(Number num)
		{
			QilNode qilNode;
			if (num.Value != null)
			{
				qilNode = this.f.ConvertToNumber(this.CompileXPathExpression(num.Value));
			}
			else
			{
				QilNode qilNode2 = ((num.Count != null) ? this.CompileNumberPattern(num.Count) : null);
				QilNode qilNode3 = ((num.From != null) ? this.CompileNumberPattern(num.From) : null);
				switch (num.Level)
				{
				case NumberLevel.Single:
					qilNode = this.PlaceMarker(qilNode2, qilNode3, false);
					break;
				case NumberLevel.Multiple:
					qilNode = this.PlaceMarker(qilNode2, qilNode3, true);
					break;
				default:
					qilNode = this.PlaceMarkerAny(qilNode2, qilNode3);
					break;
				}
			}
			bool forwardsCompatible = num.ForwardsCompatible;
			return this.f.TextCtor(this.f.InvokeNumberFormat(qilNode, this.CompileStringAvt(num.Format), this.CompileLangAttributeToLcid(num.Lang, forwardsCompatible), this.CompileLetterValueAttribute(num.LetterValue, forwardsCompatible), this.CompileGroupingSeparatorAttribute(num.GroupingSeparator, forwardsCompatible), this.CompileGroupingSizeAttribute(num.GroupingSize, forwardsCompatible)));
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00039178 File Offset: 0x00038178
		private void CompileAndSortMatches(Stylesheet sheet)
		{
			foreach (Template template in sheet.Templates)
			{
				if (template.Match != null)
				{
					this.EnterScope(template);
					QilNode qilNode = this.CompileMatchPattern(template.Match);
					if (qilNode.NodeType == QilNodeType.Sequence)
					{
						QilList qilList = (QilList)qilNode;
						for (int i = 0; i < qilList.Count; i++)
						{
							sheet.AddTemplateMatch(template, (QilLoop)qilList[i]);
						}
					}
					else
					{
						sheet.AddTemplateMatch(template, (QilLoop)qilNode);
					}
					this.ExitScope();
				}
			}
			sheet.SortTemplateMatches();
			foreach (Stylesheet stylesheet in sheet.Imports)
			{
				this.CompileAndSortMatches(stylesheet);
			}
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0003925C File Offset: 0x0003825C
		private void CompileKeys()
		{
			for (int i = 0; i < this.compiler.Keys.Count; i++)
			{
				foreach (Key key in this.compiler.Keys[i])
				{
					this.EnterScope(key);
					QilParameter qilParameter = this.f.Parameter(XmlQueryTypeFactory.NodeNotRtf);
					this.singlFocus.SetFocus(qilParameter);
					QilIterator qilIterator = this.f.For(this.f.OptimizeBarrier(this.CompileKeyMatch(key.Match)));
					this.singlFocus.SetFocus(qilIterator);
					QilIterator qilIterator2 = this.f.For(this.CompileKeyUse(key.Use));
					qilIterator2 = this.f.For(this.f.OptimizeBarrier(this.f.Loop(qilIterator2, this.f.ConvertToString(qilIterator2))));
					QilParameter qilParameter2 = this.f.Parameter(XmlQueryTypeFactory.StringX);
					QilFunction qilFunction = this.f.Function(this.f.FormalParameterList(qilParameter, qilParameter2), this.f.Filter(qilIterator, this.f.Not(this.f.IsEmpty(this.f.Filter(qilIterator2, this.f.Eq(qilIterator2, qilParameter2))))), this.f.False());
					qilFunction.DebugName = key.GetDebugName();
					QilGenerator.SetLineInfo(qilFunction, key.SourceLine);
					key.Function = qilFunction;
					this.functions.Add(qilFunction);
					this.ExitScope();
				}
			}
			this.singlFocus.SetFocus(null);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00039440 File Offset: 0x00038440
		private void CreateGlobalVarPars()
		{
			foreach (VarPar varPar in this.compiler.ExternalPars)
			{
				this.CreateGlobalVarPar(varPar);
			}
			foreach (VarPar varPar2 in this.compiler.GlobalVars)
			{
				this.CreateGlobalVarPar(varPar2);
			}
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x000394E0 File Offset: 0x000384E0
		private void CreateGlobalVarPar(VarPar varPar)
		{
			XmlQueryType xmlQueryType = this.ChooseBestType(varPar);
			QilIterator qilIterator;
			if (varPar.NodeType == XslNodeType.Variable)
			{
				qilIterator = this.f.Let(this.f.Unknown(xmlQueryType));
			}
			else
			{
				qilIterator = this.f.Parameter(null, varPar.Name, xmlQueryType);
			}
			qilIterator.DebugName = varPar.Name.ToString();
			varPar.Value = qilIterator;
			QilGenerator.SetLineInfo(qilIterator, varPar.SourceLine);
			this.scope.AddVariable(varPar.Name, qilIterator);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00039564 File Offset: 0x00038564
		private void CompileGlobalVariables()
		{
			this.singlFocus.SetFocus(SingletonFocusType.InitialDocumentNode);
			foreach (VarPar varPar in this.compiler.ExternalPars)
			{
				this.extPars.Add(this.CompileGlobalVarPar(varPar));
			}
			foreach (VarPar varPar2 in this.compiler.GlobalVars)
			{
				this.gloVars.Add(this.CompileGlobalVarPar(varPar2));
			}
			this.singlFocus.SetFocus(null);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00039634 File Offset: 0x00038634
		private QilIterator CompileGlobalVarPar(VarPar varPar)
		{
			QilIterator qilIterator = (QilIterator)varPar.Value;
			QilList qilList = this.EnterScope(varPar);
			QilNode qilNode = this.CompileVarParValue(varPar);
			QilGenerator.SetLineInfo(qilNode, qilIterator.SourceLine);
			qilNode = this.AddCurrentPositionLast(qilNode);
			qilNode = this.SetDebugNs(qilNode, qilList);
			qilIterator.SourceLine = SourceLineInfo.NoSource;
			qilIterator.Binding = qilNode;
			this.ExitScope();
			return qilIterator;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00039694 File Offset: 0x00038694
		private void ReportErrorInXPath(XslLoadException e)
		{
			XPathCompileException ex = e as XPathCompileException;
			string text = ((ex != null) ? ex.FormatDetailedMessage() : e.Message);
			this.compiler.ReportError(this.lastScope.SourceLine, "Xml_UserException", new string[] { text });
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x000396E1 File Offset: 0x000386E1
		private QilNode PhantomXPathExpression()
		{
			return this.f.TypeAssert(this.f.Sequence(), XmlQueryTypeFactory.ItemS);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x000396FE File Offset: 0x000386FE
		private QilNode PhantomKeyMatch()
		{
			return this.f.TypeAssert(this.f.Sequence(), XmlQueryTypeFactory.NodeNotRtfS);
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0003971C File Offset: 0x0003871C
		private QilNode CompileXPathExpression(string expr)
		{
			this.SetEnvironmentFlags(true, true, true);
			QilNode qilNode;
			if (expr == null)
			{
				qilNode = this.PhantomXPathExpression();
			}
			else
			{
				try
				{
					XPathScanner xpathScanner = new XPathScanner(expr);
					qilNode = this.xpathParser.Parse(xpathScanner, this.xpathBuilder, LexKind.Eof);
				}
				catch (XslLoadException ex)
				{
					if (this.xslVersion != XslVersion.ForwardsCompatible)
					{
						this.ReportErrorInXPath(ex);
					}
					qilNode = this.f.Error(this.f.String(ex.Message));
				}
			}
			if (qilNode is QilIterator)
			{
				qilNode = this.f.Nop(qilNode);
			}
			return qilNode;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x000397B4 File Offset: 0x000387B4
		private QilNode CompileXPathExpressionWithinAvt(string expr, ref int pos)
		{
			this.SetEnvironmentFlags(true, true, true);
			QilNode qilNode;
			try
			{
				XPathScanner xpathScanner = new XPathScanner(expr, pos);
				qilNode = this.xpathParser.Parse(xpathScanner, this.xpathBuilder, LexKind.RBrace);
				pos = xpathScanner.LexStart + 1;
			}
			catch (XslLoadException ex)
			{
				if (this.xslVersion != XslVersion.ForwardsCompatible)
				{
					this.ReportErrorInXPath(ex);
				}
				qilNode = this.f.Error(this.f.String(ex.Message));
				pos = expr.Length;
			}
			if (qilNode is QilIterator)
			{
				qilNode = this.f.Nop(qilNode);
			}
			return qilNode;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00039854 File Offset: 0x00038854
		private QilNode CompileMatchPattern(string pttrn)
		{
			this.SetEnvironmentFlags(false, false, true);
			QilNode qilNode;
			try
			{
				XPathScanner xpathScanner = new XPathScanner(pttrn);
				qilNode = this.ptrnParser.Parse(xpathScanner, this.ptrnBuilder);
			}
			catch (XslLoadException ex)
			{
				if (this.xslVersion != XslVersion.ForwardsCompatible)
				{
					this.ReportErrorInXPath(ex);
				}
				qilNode = this.f.Loop(this.f.For(this.ptrnBuilder.FixupNode), this.f.Error(this.f.String(ex.Message)));
				XPathPatternBuilder.SetPriority(qilNode, 0.5);
			}
			return qilNode;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x000398F8 File Offset: 0x000388F8
		private QilNode CompileNumberPattern(string pttrn)
		{
			this.SetEnvironmentFlags(true, false, true);
			QilNode qilNode;
			try
			{
				XPathScanner xpathScanner = new XPathScanner(pttrn);
				qilNode = this.ptrnParser.Parse(xpathScanner, this.ptrnBuilder);
			}
			catch (XslLoadException ex)
			{
				if (this.xslVersion != XslVersion.ForwardsCompatible)
				{
					this.ReportErrorInXPath(ex);
				}
				qilNode = this.f.Error(this.f.String(ex.Message));
			}
			return qilNode;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0003996C File Offset: 0x0003896C
		private QilNode CompileKeyMatch(string pttrn)
		{
			if (this.keyMatchBuilder == null)
			{
				this.keyMatchBuilder = new KeyMatchBuilder(this);
			}
			this.SetEnvironmentFlags(false, false, false);
			QilNode qilNode;
			if (pttrn == null)
			{
				qilNode = this.PhantomKeyMatch();
			}
			else
			{
				try
				{
					XPathScanner xpathScanner = new XPathScanner(pttrn);
					qilNode = this.ptrnParser.Parse(xpathScanner, this.keyMatchBuilder);
				}
				catch (XslLoadException ex)
				{
					if (this.xslVersion != XslVersion.ForwardsCompatible)
					{
						this.ReportErrorInXPath(ex);
					}
					qilNode = this.f.Error(this.f.String(ex.Message));
				}
			}
			return qilNode;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00039A00 File Offset: 0x00038A00
		private QilNode CompileKeyUse(string expr)
		{
			this.SetEnvironmentFlags(false, true, false);
			QilNode qilNode;
			if (expr == null)
			{
				qilNode = this.PhantomXPathExpression();
			}
			else
			{
				try
				{
					XPathScanner xpathScanner = new XPathScanner(expr);
					qilNode = this.xpathParser.Parse(xpathScanner, this.xpathBuilder, LexKind.Eof);
				}
				catch (XslLoadException ex)
				{
					if (this.xslVersion != XslVersion.ForwardsCompatible)
					{
						this.ReportErrorInXPath(ex);
					}
					qilNode = this.f.Error(this.f.String(ex.Message));
				}
			}
			if (qilNode is QilIterator)
			{
				qilNode = this.f.Nop(qilNode);
			}
			return qilNode;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00039A98 File Offset: 0x00038A98
		private QilNode ResolveQNameDynamic(bool ignoreDefaultNs, QilNode qilName)
		{
			QilList qilList = this.f.BaseFactory.Sequence();
			if (ignoreDefaultNs)
			{
				qilList.Add(this.f.NamespaceDecl(this.f.String(string.Empty), this.f.String(string.Empty)));
			}
			foreach (object obj in ((IEnumerable)this.scope))
			{
				CompilerScopeManager<QilIterator>.ScopeRecord scopeRecord = (CompilerScopeManager<QilIterator>.ScopeRecord)obj;
				string ncName = scopeRecord.ncName;
				string nsUri = scopeRecord.nsUri;
				if (!ignoreDefaultNs || ncName.Length != 0)
				{
					qilList.Add(this.f.NamespaceDecl(this.f.String(ncName), this.f.String(nsUri)));
				}
			}
			return this.f.StrParseQName(qilName, qilList);
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00039B88 File Offset: 0x00038B88
		private QilNode GenerateCall(QilFunction func, XslNode node)
		{
			this.AddImplicitArgs(node);
			return this.invkGen.GenerateInvoke(func, node.Content);
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00039BA3 File Offset: 0x00038BA3
		private QilNode GenerateApply(Stylesheet sheet, XslNode node)
		{
			if (this.compiler.Settings.CheckOnly)
			{
				return this.f.Sequence();
			}
			this.AddImplicitArgs(node);
			return this.InvokeApplyFunction(sheet, node.Name, node.Content);
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00039BE0 File Offset: 0x00038BE0
		private void AddImplicitArgs(XslNode node)
		{
			XslFlags xslFlags = XslFlags.None;
			if (this.IsDebug)
			{
				xslFlags = XslFlags.FocusFilter;
			}
			else if (node.NodeType == XslNodeType.CallTemplate)
			{
				Template template;
				if (this.compiler.NamedTemplates.TryGetValue(node.Name, out template))
				{
					xslFlags = template.Flags;
				}
			}
			else if (node.NodeType == XslNodeType.UseAttributeSet)
			{
				AttributeSet attributeSet;
				if (this.compiler.AttributeSets.TryGetValue(node.Name, out attributeSet))
				{
					xslFlags = attributeSet.Flags;
				}
			}
			else
			{
				if (!this.compiler.ModeFlags.TryGetValue(node.Name, out xslFlags))
				{
					xslFlags = XslFlags.None;
				}
				xslFlags |= XslFlags.Current;
			}
			List<XslNode> list = new List<XslNode>();
			if ((xslFlags & XslFlags.Current) != XslFlags.None)
			{
				list.Add(QilGenerator.CreateWithParam(this.nameCurrent, this.GetCurrentNode()));
			}
			if ((xslFlags & XslFlags.Position) != XslFlags.None)
			{
				list.Add(QilGenerator.CreateWithParam(this.namePosition, this.GetCurrentPosition()));
			}
			if ((xslFlags & XslFlags.Last) != XslFlags.None)
			{
				list.Add(QilGenerator.CreateWithParam(this.nameLast, this.GetLastPosition()));
			}
			node.InsertContent(list);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00039CEC File Offset: 0x00038CEC
		public static VarPar CreateWithParam(QilName name, QilNode value)
		{
			VarPar varPar = AstFactory.WithParam(name);
			varPar.Value = value;
			return varPar;
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00039D08 File Offset: 0x00038D08
		private bool FillupInvokeArgs(IList<QilNode> formalArgs, IList<XslNode> actualArgs, QilList invokeArgs)
		{
			if (actualArgs.Count != formalArgs.Count)
			{
				return false;
			}
			invokeArgs.Clear();
			for (int i = 0; i < formalArgs.Count; i++)
			{
				QilName name = ((QilParameter)formalArgs[i]).Name;
				XmlQueryType xmlType = formalArgs[i].XmlType;
				QilNode qilNode = null;
				int j = 0;
				while (j < actualArgs.Count)
				{
					VarPar varPar = (VarPar)actualArgs[j];
					if (name.Equals(varPar.Name))
					{
						QilNode value = varPar.Value;
						XmlQueryType xmlType2 = value.XmlType;
						if (xmlType2 != xmlType && (!xmlType2.IsNode || !xmlType.IsNode || !xmlType2.IsSubtypeOf(xmlType)))
						{
							return false;
						}
						qilNode = value;
						break;
					}
					else
					{
						j++;
					}
				}
				if (qilNode == null)
				{
					return false;
				}
				invokeArgs.Add(qilNode);
			}
			return true;
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00039DE4 File Offset: 0x00038DE4
		private QilNode InvokeApplyFunction(Stylesheet sheet, QilName mode, IList<XslNode> actualArgs)
		{
			XslFlags xslFlags;
			if (!this.compiler.ModeFlags.TryGetValue(mode, out xslFlags))
			{
				xslFlags = XslFlags.None;
			}
			if (this.IsDebug)
			{
				xslFlags = XslFlags.FocusFilter;
			}
			xslFlags |= XslFlags.Current;
			QilList qilList = this.f.ActualParameterList();
			QilFunction qilFunction = null;
			Dictionary<QilName, List<QilFunction>> dictionary = ((sheet == null) ? this.compiler.ApplyTemplatesFunctions : sheet.ApplyImportsFunctions);
			List<QilFunction> list;
			if (!dictionary.TryGetValue(mode, out list))
			{
				list = (dictionary[mode] = new List<QilFunction>());
			}
			foreach (QilFunction qilFunction2 in list)
			{
				if (this.FillupInvokeArgs(qilFunction2.Arguments, actualArgs, qilList))
				{
					qilFunction = qilFunction2;
					break;
				}
			}
			if (qilFunction == null)
			{
				qilList.Clear();
				QilList qilList2 = this.f.FormalParameterList();
				for (int i = 0; i < actualArgs.Count; i++)
				{
					VarPar varPar = (VarPar)actualArgs[i];
					qilList.Add(varPar.Value);
					QilParameter qilParameter = this.f.Parameter((i == 0) ? XmlQueryTypeFactory.NodeNotRtf : varPar.Value.XmlType);
					qilParameter.Name = this.CloneName(varPar.Name);
					qilList2.Add(qilParameter);
					varPar.Value = qilParameter;
				}
				qilFunction = this.f.Function(qilList2, this.f.False(), XmlQueryTypeFactory.NodeNotRtfS);
				string text = ((mode.LocalName.Length == 0) ? string.Empty : (" mode=\"" + mode.QualifiedName + '"'));
				qilFunction.DebugName = ((sheet == null) ? "<xsl:apply-templates" : "<xsl:apply-imports") + text + '>';
				list.Add(qilFunction);
				this.functions.Add(qilFunction);
				QilIterator qilIterator = (QilIterator)qilList2[0];
				QilTernary qilTernary = this.f.BaseFactory.Conditional(this.f.IsType(qilIterator, this.elementOrDocumentType), this.f.BaseFactory.Nop(this.f.BaseFactory.Unknown(XmlQueryTypeFactory.NodeNotRtfS)), this.f.Conditional(this.f.IsType(qilIterator, this.textOrAttributeType), this.f.TextCtor(this.f.XPathNodeValue(qilIterator)), this.f.Sequence()));
				this.matcherBuilder.CollectPatterns(sheet ?? this.compiler.PrincipalStylesheet, mode, sheet != null);
				qilFunction.Definition = this.matcherBuilder.BuildMatcher(qilIterator, actualArgs, qilTernary);
				QilIterator qilIterator2 = this.f.For(this.f.Content(qilIterator));
				QilNode qilNode = this.f.Filter(qilIterator2, this.f.IsType(qilIterator2, XmlQueryTypeFactory.Content));
				qilNode.XmlType = XmlQueryTypeFactory.ContentS;
				LoopFocus loopFocus = this.curLoop;
				this.curLoop.SetFocus(this.f.For(qilNode));
				if ((xslFlags & XslFlags.Last) != XslFlags.None)
				{
					this.curLoop.GetLast();
				}
				List<XslNode> list2 = new List<XslNode>(3);
				int num = 0;
				if ((xslFlags & XslFlags.Current) != XslFlags.None)
				{
					list2.Add(actualArgs[num++]);
				}
				if ((xslFlags & XslFlags.Position) != XslFlags.None)
				{
					list2.Add(actualArgs[num++]);
				}
				if ((xslFlags & XslFlags.Last) != XslFlags.None)
				{
					list2.Add(actualArgs[num++]);
				}
				actualArgs = list2;
				int num2 = 0;
				if ((xslFlags & XslFlags.Current) != XslFlags.None)
				{
					((VarPar)actualArgs[num2++]).Value = this.GetCurrentNode();
				}
				if ((xslFlags & XslFlags.Position) != XslFlags.None)
				{
					((VarPar)actualArgs[num2++]).Value = this.GetCurrentPosition();
				}
				if ((xslFlags & XslFlags.Last) != XslFlags.None)
				{
					((VarPar)actualArgs[num2++]).Value = this.GetLastPosition();
				}
				QilNode qilNode2 = this.InvokeApplyFunction(null, mode, actualArgs);
				if (this.IsDebug)
				{
					qilNode2 = this.f.Sequence(this.InvokeOnCurrentNodeChanged(), qilNode2);
				}
				QilLoop qilLoop = this.curLoop.ConstructLoop(qilNode2);
				this.curLoop = loopFocus;
				((QilUnary)qilTernary.Center).Child = qilLoop;
			}
			return this.f.Invoke(qilFunction, qilList);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0003A254 File Offset: 0x00039254
		public void ReportError(string res, params string[] args)
		{
			this.compiler.ReportError(this.lastScope.SourceLine, res, args);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0003A26E File Offset: 0x0003926E
		public void ReportWarning(string res, params string[] args)
		{
			this.compiler.ReportWarning(this.lastScope.SourceLine, res, args);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0003A288 File Offset: 0x00039288
		[Conditional("DEBUG")]
		private void VerifyXPathQName(QilName qname)
		{
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0003A28C File Offset: 0x0003928C
		private string ResolvePrefix(bool ignoreDefaultNs, string prefix)
		{
			if (ignoreDefaultNs && prefix.Length == 0)
			{
				return string.Empty;
			}
			string text = this.scope.LookupNamespace(prefix);
			if (text == null)
			{
				if (prefix.Length == 0)
				{
					text = string.Empty;
				}
				else
				{
					this.ReportError("Xslt_InvalidPrefix", new string[] { prefix });
					text = this.compiler.CreatePhantomNamespace();
				}
			}
			return text;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0003A2EE File Offset: 0x000392EE
		private void SetLineInfoCheck(QilNode n, ISourceLineInfo lineInfo)
		{
			if (n.SourceLine == null)
			{
				QilGenerator.SetLineInfo(n, lineInfo);
			}
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0003A300 File Offset: 0x00039300
		private static QilNode SetLineInfo(QilNode n, ISourceLineInfo lineInfo)
		{
			if (lineInfo != null && 0 < lineInfo.StartLine && lineInfo.StartLine <= lineInfo.EndLine)
			{
				n.SourceLine = lineInfo;
			}
			return n;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0003A324 File Offset: 0x00039324
		private QilNode AddDebugVariable(QilName name, QilNode value, QilNode content)
		{
			QilIterator qilIterator = this.f.Let(value);
			qilIterator.DebugName = name.ToString();
			return this.f.Loop(qilIterator, content);
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0003A358 File Offset: 0x00039358
		private QilNode SetDebugNs(QilNode n, QilList nsList)
		{
			if (n != null && nsList != null)
			{
				QilNode qilNode = this.GetNsVar(nsList);
				if (qilNode.XmlType.Cardinality == XmlQueryCardinality.One)
				{
					qilNode = this.f.TypeAssert(qilNode, XmlQueryTypeFactory.NamespaceS);
				}
				n = this.AddDebugVariable(this.CloneName(this.nameNamespaces), qilNode, n);
			}
			return n;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0003A3B4 File Offset: 0x000393B4
		private QilNode AddCurrentPositionLast(QilNode content)
		{
			if (this.IsDebug)
			{
				content = this.AddDebugVariable(this.CloneName(this.nameLast), this.GetLastPosition(), content);
				content = this.AddDebugVariable(this.CloneName(this.namePosition), this.GetCurrentPosition(), content);
				content = this.AddDebugVariable(this.CloneName(this.nameCurrent), this.GetCurrentNode(), content);
			}
			return content;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0003A41B File Offset: 0x0003941B
		private QilName CloneName(QilName name)
		{
			return (QilName)name.ShallowClone(this.f.BaseFactory);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0003A433 File Offset: 0x00039433
		private void SetEnvironmentFlags(bool allowVariables, bool allowCurrent, bool allowKey)
		{
			this.allowVariables = allowVariables;
			this.allowCurrent = allowCurrent;
			this.allowKey = allowKey;
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x0003A44A File Offset: 0x0003944A
		XPathQilFactory IXPathEnvironment.Factory
		{
			get
			{
				return this.f;
			}
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0003A452 File Offset: 0x00039452
		QilNode IFocus.GetCurrent()
		{
			return this.GetCurrentNode();
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0003A45A File Offset: 0x0003945A
		QilNode IFocus.GetPosition()
		{
			return this.GetCurrentPosition();
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0003A462 File Offset: 0x00039462
		QilNode IFocus.GetLast()
		{
			return this.GetLastPosition();
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0003A46A File Offset: 0x0003946A
		string IXPathEnvironment.ResolvePrefix(string prefix)
		{
			return this.ResolvePrefixThrow(true, prefix);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0003A474 File Offset: 0x00039474
		QilNode IXPathEnvironment.ResolveVariable(string prefix, string name)
		{
			if (!this.allowVariables)
			{
				throw new XslLoadException("Xslt_VariablesNotAllowed", new string[0]);
			}
			string text = this.ResolvePrefixThrow(true, prefix);
			QilNode qilNode = this.scope.LookupVariable(name, text);
			if (qilNode == null)
			{
				throw new XslLoadException("Xslt_InvalidVariable", new string[] { Compiler.ConstructQName(prefix, name) });
			}
			XmlQueryType xmlType = qilNode.XmlType;
			if (qilNode.NodeType == QilNodeType.Parameter && xmlType.IsNode && xmlType.IsNotRtf && xmlType.MaybeMany && !xmlType.IsDod)
			{
				qilNode = this.f.TypeAssert(qilNode, XmlQueryTypeFactory.NodeDodS);
			}
			return qilNode;
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0003A518 File Offset: 0x00039518
		QilNode IXPathEnvironment.ResolveFunction(string prefix, string name, IList<QilNode> args, IFocus env)
		{
			if (prefix.Length != 0)
			{
				string text = this.ResolvePrefixThrow(true, prefix);
				if (text == "urn:schemas-microsoft-com:xslt")
				{
					if (name == "node-set")
					{
						XPathBuilder.FunctionInfo<QilGenerator.FuncId>.CheckArity(1, 1, name, args.Count);
						return this.CompileMsNodeSet(args[0]);
					}
					if (name == "string-compare")
					{
						XPathBuilder.FunctionInfo<QilGenerator.FuncId>.CheckArity(2, 4, name, args.Count);
						return this.f.InvokeMsStringCompare(this.f.ConvertToString(args[0]), this.f.ConvertToString(args[1]), (2 < args.Count) ? this.f.ConvertToString(args[2]) : this.f.String(string.Empty), (3 < args.Count) ? this.f.ConvertToString(args[3]) : this.f.String(string.Empty));
					}
					if (name == "utc")
					{
						XPathBuilder.FunctionInfo<QilGenerator.FuncId>.CheckArity(1, 1, name, args.Count);
						return this.f.InvokeMsUtc(this.f.ConvertToString(args[0]));
					}
					if (name == "format-date" || name == "format-time")
					{
						XPathBuilder.FunctionInfo<QilGenerator.FuncId>.CheckArity(1, 3, name, args.Count);
						return this.f.InvokeMsFormatDateTime(this.f.ConvertToString(args[0]), (1 < args.Count) ? this.f.ConvertToString(args[1]) : this.f.String(string.Empty), (2 < args.Count) ? this.f.ConvertToString(args[2]) : this.f.String(string.Empty), this.f.Boolean(name == "format-date"));
					}
					if (name == "local-name")
					{
						XPathBuilder.FunctionInfo<QilGenerator.FuncId>.CheckArity(1, 1, name, args.Count);
						return this.f.InvokeMsLocalName(this.f.ConvertToString(args[0]));
					}
					if (name == "namespace-uri")
					{
						XPathBuilder.FunctionInfo<QilGenerator.FuncId>.CheckArity(1, 1, name, args.Count);
						return this.f.InvokeMsNamespaceUri(this.f.ConvertToString(args[0]), env.GetCurrent());
					}
					if (name == "number")
					{
						XPathBuilder.FunctionInfo<QilGenerator.FuncId>.CheckArity(1, 1, name, args.Count);
						return this.f.InvokeMsNumber(args[0]);
					}
				}
				if (text == "http://exslt.org/common")
				{
					if (name == "node-set")
					{
						XPathBuilder.FunctionInfo<QilGenerator.FuncId>.CheckArity(1, 1, name, args.Count);
						return this.CompileMsNodeSet(args[0]);
					}
					if (name == "object-type")
					{
						XPathBuilder.FunctionInfo<QilGenerator.FuncId>.CheckArity(1, 1, name, args.Count);
						return this.EXslObjectType(args[0]);
					}
				}
				for (int i = 0; i < args.Count; i++)
				{
					args[i] = this.f.SafeDocOrderDistinct(args[i]);
				}
				if (this.compiler.Settings.EnableScript)
				{
					XmlExtensionFunction xmlExtensionFunction = this.compiler.Scripts.ResolveFunction(name, text, args.Count, this);
					if (xmlExtensionFunction != null)
					{
						return this.GenerateScriptCall(this.f.QName(name, text, prefix), xmlExtensionFunction, args);
					}
				}
				else if (this.compiler.Scripts.ScriptClasses.ContainsKey(text))
				{
					this.ReportWarning("Xslt_ScriptsProhibited", new string[0]);
					return this.f.Error(this.lastScope.SourceLine, "Xslt_ScriptsProhibited", new string[0]);
				}
				return this.f.XsltInvokeLateBound(this.f.QName(name, text, prefix), args);
			}
			XPathBuilder.FunctionInfo<QilGenerator.FuncId> functionInfo;
			if (!QilGenerator.FunctionTable.TryGetValue(name, out functionInfo))
			{
				throw new XslLoadException("Xslt_UnknownXsltFunction", new string[] { Compiler.ConstructQName(prefix, name) });
			}
			functionInfo.CastArguments(args, name, this.f);
			switch (functionInfo.id)
			{
			case QilGenerator.FuncId.Current:
				if (!this.allowCurrent)
				{
					throw new XslLoadException("Xslt_CurrentNotAllowed", new string[0]);
				}
				return ((IFocus)this).GetCurrent();
			case QilGenerator.FuncId.Document:
				return this.CompileFnDocument(args[0], (args.Count > 1) ? args[1] : null);
			case QilGenerator.FuncId.Key:
				if (!this.allowKey)
				{
					throw new XslLoadException("Xslt_KeyNotAllowed", new string[0]);
				}
				return this.CompileFnKey(args[0], args[1], env);
			case QilGenerator.FuncId.FormatNumber:
				return this.CompileFormatNumber(args[0], args[1], (args.Count > 2) ? args[2] : null);
			case QilGenerator.FuncId.UnparsedEntityUri:
				return this.CompileUnparsedEntityUri(args[0]);
			case QilGenerator.FuncId.GenerateId:
				return this.CompileGenerateId((args.Count > 0) ? args[0] : env.GetCurrent());
			case QilGenerator.FuncId.SystemProperty:
				return this.CompileSystemProperty(args[0]);
			case QilGenerator.FuncId.ElementAvailable:
				return this.CompileElementAvailable(args[0]);
			case QilGenerator.FuncId.FunctionAvailable:
				return this.CompileFunctionAvailable(args[0]);
			default:
				return null;
			}
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0003AA54 File Offset: 0x00039A54
		private QilNode GenerateScriptCall(QilName name, XmlExtensionFunction scrFunc, IList<QilNode> args)
		{
			for (int i = 0; i < args.Count; i++)
			{
				XmlQueryType xmlArgumentType = scrFunc.GetXmlArgumentType(i);
				XmlTypeCode typeCode = xmlArgumentType.TypeCode;
				switch (typeCode)
				{
				case XmlTypeCode.Item:
					break;
				case XmlTypeCode.Node:
					args[i] = (xmlArgumentType.IsSingleton ? this.f.ConvertToNode(args[i]) : this.f.ConvertToNodeSet(args[i]));
					break;
				default:
					switch (typeCode)
					{
					case XmlTypeCode.String:
						args[i] = this.f.ConvertToString(args[i]);
						break;
					case XmlTypeCode.Boolean:
						args[i] = this.f.ConvertToBoolean(args[i]);
						break;
					case XmlTypeCode.Double:
						args[i] = this.f.ConvertToNumber(args[i]);
						break;
					}
					break;
				}
			}
			return this.f.XsltInvokeEarlyBound(name, scrFunc.Method, scrFunc.XmlReturnType, args);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0003AB58 File Offset: 0x00039B58
		private string ResolvePrefixThrow(bool ignoreDefaultNs, string prefix)
		{
			if (ignoreDefaultNs && prefix.Length == 0)
			{
				return string.Empty;
			}
			string text = this.scope.LookupNamespace(prefix);
			if (text == null)
			{
				if (prefix.Length != 0)
				{
					throw new XslLoadException("Xslt_InvalidPrefix", new string[] { prefix });
				}
				text = string.Empty;
			}
			return text;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0003ABAC File Offset: 0x00039BAC
		private static Dictionary<string, XPathBuilder.FunctionInfo<QilGenerator.FuncId>> CreateFunctionTable()
		{
			return new Dictionary<string, XPathBuilder.FunctionInfo<QilGenerator.FuncId>>(16)
			{
				{
					"current",
					new XPathBuilder.FunctionInfo<QilGenerator.FuncId>(QilGenerator.FuncId.Current, 0, 0, null)
				},
				{
					"document",
					new XPathBuilder.FunctionInfo<QilGenerator.FuncId>(QilGenerator.FuncId.Document, 1, 2, QilGenerator.argFnDocument)
				},
				{
					"key",
					new XPathBuilder.FunctionInfo<QilGenerator.FuncId>(QilGenerator.FuncId.Key, 2, 2, QilGenerator.argFnKey)
				},
				{
					"format-number",
					new XPathBuilder.FunctionInfo<QilGenerator.FuncId>(QilGenerator.FuncId.FormatNumber, 2, 3, QilGenerator.argFnFormatNumber)
				},
				{
					"unparsed-entity-uri",
					new XPathBuilder.FunctionInfo<QilGenerator.FuncId>(QilGenerator.FuncId.UnparsedEntityUri, 1, 1, XPathBuilder.argString)
				},
				{
					"generate-id",
					new XPathBuilder.FunctionInfo<QilGenerator.FuncId>(QilGenerator.FuncId.GenerateId, 0, 1, XPathBuilder.argNodeSet)
				},
				{
					"system-property",
					new XPathBuilder.FunctionInfo<QilGenerator.FuncId>(QilGenerator.FuncId.SystemProperty, 1, 1, XPathBuilder.argString)
				},
				{
					"element-available",
					new XPathBuilder.FunctionInfo<QilGenerator.FuncId>(QilGenerator.FuncId.ElementAvailable, 1, 1, XPathBuilder.argString)
				},
				{
					"function-available",
					new XPathBuilder.FunctionInfo<QilGenerator.FuncId>(QilGenerator.FuncId.FunctionAvailable, 1, 1, XPathBuilder.argString)
				}
			};
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0003AC98 File Offset: 0x00039C98
		public static bool IsFunctionAvailable(string localName, string nsUri)
		{
			if (XPathBuilder.IsFunctionAvailable(localName, nsUri))
			{
				return true;
			}
			if (nsUri.Length == 0)
			{
				return QilGenerator.FunctionTable.ContainsKey(localName) && localName != "unparsed-entity-uri";
			}
			if (nsUri == "urn:schemas-microsoft-com:xslt")
			{
				return localName == "node-set" || localName == "format-date" || localName == "format-time" || localName == "local-name" || localName == "namespace-uri" || localName == "number" || localName == "string-compare" || localName == "utc";
			}
			return nsUri == "http://exslt.org/common" && (localName == "node-set" || localName == "object-type");
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0003AD74 File Offset: 0x00039D74
		public static bool IsElementAvailable(XmlQualifiedName name)
		{
			if (name.Namespace == "http://www.w3.org/1999/XSL/Transform")
			{
				string name2 = name.Name;
				return name2 == "apply-imports" || name2 == "apply-templates" || name2 == "attribute" || name2 == "call-template" || name2 == "choose" || name2 == "comment" || name2 == "copy" || name2 == "copy-of" || name2 == "element" || name2 == "fallback" || name2 == "for-each" || name2 == "if" || name2 == "message" || name2 == "number" || name2 == "processing-instruction" || name2 == "text" || name2 == "value-of" || name2 == "variable";
			}
			return false;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0003AEA4 File Offset: 0x00039EA4
		private QilNode CompileFnKey(QilNode name, QilNode keys, IFocus env)
		{
			QilNode qilNode;
			if (keys.XmlType.IsNode)
			{
				if (keys.XmlType.IsSingleton)
				{
					qilNode = this.CompileSingleKey(name, this.f.ConvertToString(keys), env);
				}
				else
				{
					QilIterator qilIterator;
					qilNode = this.f.Loop(qilIterator = this.f.For(keys), this.CompileSingleKey(name, this.f.ConvertToString(qilIterator), env));
				}
			}
			else if (keys.XmlType.IsAtomicValue)
			{
				qilNode = this.CompileSingleKey(name, this.f.ConvertToString(keys), env);
			}
			else
			{
				QilIterator qilIterator;
				QilIterator qilIterator2;
				QilIterator qilIterator3;
				qilNode = this.f.Loop(qilIterator2 = this.f.Let(name), this.f.Loop(qilIterator3 = this.f.Let(keys), this.f.Conditional(this.f.Not(this.f.IsType(qilIterator3, XmlQueryTypeFactory.AnyAtomicType)), this.f.Loop(qilIterator = this.f.For(this.f.TypeAssert(qilIterator3, XmlQueryTypeFactory.NodeS)), this.CompileSingleKey(qilIterator2, this.f.ConvertToString(qilIterator), env)), this.CompileSingleKey(qilIterator2, this.f.XsltConvert(qilIterator3, XmlQueryTypeFactory.StringX), env))));
			}
			return this.f.DocOrderDistinct(qilNode);
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0003AFFC File Offset: 0x00039FFC
		private QilNode CompileSingleKey(QilNode name, QilNode key, IFocus env)
		{
			QilNode qilNode;
			if (name.NodeType == QilNodeType.LiteralString)
			{
				string text = (QilLiteral)name;
				string text2;
				string text3;
				this.compiler.ParseQName(text, out text2, out text3, default(QilGenerator.ThrowErrorHelper));
				string text4 = this.ResolvePrefixThrow(true, text2);
				QilName qilName = this.f.QName(text3, text4, text2);
				if (!this.compiler.Keys.Contains(qilName))
				{
					throw new XslLoadException("Xslt_UndefinedKey", new string[] { text });
				}
				qilNode = this.CompileSingleKey(this.compiler.Keys[qilName], key, env);
			}
			else
			{
				if (this.generalKey == null)
				{
					this.generalKey = this.CreateGeneralKeyFunction();
				}
				QilIterator qilIterator = this.f.Let(name);
				QilNode qilNode2 = this.ResolveQNameDynamic(true, qilIterator);
				qilNode = this.f.Invoke(this.generalKey, this.f.ActualParameterList(new QilNode[]
				{
					qilIterator,
					qilNode2,
					key,
					env.GetCurrent()
				}));
				qilNode = this.f.Loop(qilIterator, qilNode);
			}
			return qilNode;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0003B128 File Offset: 0x0003A128
		private QilNode CompileSingleKey(List<Key> defList, QilNode key, IFocus env)
		{
			if (defList.Count == 1)
			{
				return this.f.Invoke(defList[0].Function, this.f.ActualParameterList(env.GetCurrent(), key));
			}
			QilIterator qilIterator = this.f.Let(key);
			QilNode qilNode = this.f.Sequence();
			foreach (Key key2 in defList)
			{
				qilNode.Add(this.f.Invoke(key2.Function, this.f.ActualParameterList(env.GetCurrent(), qilIterator)));
			}
			return this.f.Loop(qilIterator, qilNode);
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0003B1F4 File Offset: 0x0003A1F4
		private QilNode CompileSingleKey(List<Key> defList, QilIterator key, QilIterator context)
		{
			QilList qilList = this.f.BaseFactory.Sequence();
			QilNode qilNode = null;
			foreach (Key key2 in defList)
			{
				qilNode = this.f.Invoke(key2.Function, this.f.ActualParameterList(context, key));
				qilList.Add(qilNode);
			}
			if (defList.Count != 1)
			{
				return qilList;
			}
			return qilNode;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0003B280 File Offset: 0x0003A280
		private QilFunction CreateGeneralKeyFunction()
		{
			QilIterator qilIterator = this.f.Parameter(XmlQueryTypeFactory.StringX);
			QilIterator qilIterator2 = this.f.Parameter(XmlQueryTypeFactory.QNameX);
			QilIterator qilIterator3 = this.f.Parameter(XmlQueryTypeFactory.StringX);
			QilIterator qilIterator4 = this.f.Parameter(XmlQueryTypeFactory.NodeNotRtf);
			QilNode qilNode = this.f.Error("Xslt_UndefinedKey", qilIterator);
			for (int i = 0; i < this.compiler.Keys.Count; i++)
			{
				qilNode = this.f.Conditional(this.f.Eq(qilIterator2, this.compiler.Keys[i][0].Name.DeepClone(this.f.BaseFactory)), this.CompileSingleKey(this.compiler.Keys[i], qilIterator3, qilIterator4), qilNode);
			}
			QilFunction qilFunction = this.f.Function(this.f.FormalParameterList(new QilNode[] { qilIterator, qilIterator2, qilIterator3, qilIterator4 }), qilNode, this.f.False());
			qilFunction.DebugName = "key";
			this.functions.Add(qilFunction);
			return qilFunction;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0003B3C4 File Offset: 0x0003A3C4
		private QilNode CompileFnDocument(QilNode uris, QilNode baseNode)
		{
			if (!this.compiler.Settings.EnableDocumentFunction)
			{
				this.ReportWarning("Xslt_DocumentFuncProhibited", new string[0]);
				return this.f.Error(this.lastScope.SourceLine, "Xslt_DocumentFuncProhibited", new string[0]);
			}
			QilNode qilNode;
			if (uris.XmlType.IsNode)
			{
				QilIterator qilIterator;
				qilNode = this.f.DocOrderDistinct(this.f.Loop(qilIterator = this.f.For(uris), this.CompileSingleDocument(this.f.ConvertToString(qilIterator), baseNode ?? qilIterator)));
			}
			else if (uris.XmlType.IsAtomicValue)
			{
				qilNode = this.CompileSingleDocument(this.f.ConvertToString(uris), baseNode);
			}
			else
			{
				QilIterator qilIterator2 = this.f.Let(uris);
				QilIterator qilIterator3 = ((baseNode != null) ? this.f.Let(baseNode) : null);
				QilIterator qilIterator;
				qilNode = this.f.Conditional(this.f.Not(this.f.IsType(qilIterator2, XmlQueryTypeFactory.AnyAtomicType)), this.f.DocOrderDistinct(this.f.Loop(qilIterator = this.f.For(this.f.TypeAssert(qilIterator2, XmlQueryTypeFactory.NodeS)), this.CompileSingleDocument(this.f.ConvertToString(qilIterator), qilIterator3 ?? qilIterator))), this.CompileSingleDocument(this.f.XsltConvert(qilIterator2, XmlQueryTypeFactory.StringX), qilIterator3));
				qilNode = ((baseNode != null) ? this.f.Loop(qilIterator3, qilNode) : qilNode);
				qilNode = this.f.Loop(qilIterator2, qilNode);
			}
			return qilNode;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0003B55C File Offset: 0x0003A55C
		private QilNode CompileSingleDocument(QilNode uri, QilNode baseNode)
		{
			QilNode qilNode;
			if (baseNode == null)
			{
				qilNode = this.f.String(this.lastScope.SourceLine.Uri);
			}
			else if (baseNode.XmlType.IsSingleton)
			{
				qilNode = this.f.InvokeBaseUri(baseNode);
			}
			else
			{
				QilIterator qilIterator;
				qilNode = this.f.StrConcat(this.f.Loop(qilIterator = this.f.FirstNode(baseNode), this.f.InvokeBaseUri(qilIterator)));
			}
			return this.f.DataSource(uri, qilNode);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0003B5E4 File Offset: 0x0003A5E4
		private QilNode CompileFormatNumber(QilNode value, QilNode formatPicture, QilNode formatName)
		{
			XmlQualifiedName xmlQualifiedName;
			if (formatName == null)
			{
				xmlQualifiedName = new XmlQualifiedName();
				formatName = this.f.String(string.Empty);
			}
			else if (formatName.NodeType == QilNodeType.LiteralString)
			{
				xmlQualifiedName = this.ResolveQNameThrow(true, formatName);
			}
			else
			{
				xmlQualifiedName = null;
			}
			if (!(xmlQualifiedName != null))
			{
				this.formatNumberDynamicUsed = true;
				QilIterator qilIterator = this.f.Let(formatName);
				QilNode qilNode = this.ResolveQNameDynamic(true, qilIterator);
				return this.f.Loop(qilIterator, this.f.InvokeFormatNumberDynamic(value, formatPicture, qilNode, qilIterator));
			}
			DecimalFormatDecl decimalFormatDecl;
			if (this.compiler.DecimalFormats.Contains(xmlQualifiedName))
			{
				decimalFormatDecl = this.compiler.DecimalFormats[xmlQualifiedName];
			}
			else
			{
				if (xmlQualifiedName != DecimalFormatDecl.Default.Name)
				{
					throw new XslLoadException("Xslt_NoDecimalFormat", new string[] { (QilLiteral)formatName });
				}
				decimalFormatDecl = DecimalFormatDecl.Default;
			}
			if (formatPicture.NodeType == QilNodeType.LiteralString)
			{
				QilIterator qilIterator2 = this.f.Let(this.f.InvokeRegisterDecimalFormatter(formatPicture, decimalFormatDecl));
				qilIterator2.DebugName = this.f.QName("formatter" + this.formatterCnt++, "urn:schemas-microsoft-com:xslt-debug").ToString();
				this.gloVars.Add(qilIterator2);
				return this.f.InvokeFormatNumberStatic(value, qilIterator2);
			}
			this.formatNumberDynamicUsed = true;
			QilNode qilNode2 = this.f.QName(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
			return this.f.InvokeFormatNumberDynamic(value, formatPicture, qilNode2, formatName);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0003B77C File Offset: 0x0003A77C
		private QilNode CompileUnparsedEntityUri(QilNode n)
		{
			return this.f.Error(this.lastScope.SourceLine, "Xslt_UnsupportedXsltFunction", new string[] { "unparsed-entity-uri" });
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0003B7B4 File Offset: 0x0003A7B4
		private QilNode CompileGenerateId(QilNode n)
		{
			if (n.XmlType.IsSingleton)
			{
				return this.f.XsltGenerateId(n);
			}
			QilIterator qilIterator;
			return this.f.StrConcat(this.f.Loop(qilIterator = this.f.FirstNode(n), this.f.XsltGenerateId(qilIterator)));
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0003B80C File Offset: 0x0003A80C
		private XmlQualifiedName ResolveQNameThrow(bool ignoreDefaultNs, QilNode qilName)
		{
			string text = (QilLiteral)qilName;
			string text2;
			string text3;
			this.compiler.ParseQName(text, out text2, out text3, default(QilGenerator.ThrowErrorHelper));
			string text4 = this.ResolvePrefixThrow(ignoreDefaultNs, text2);
			return new XmlQualifiedName(text3, text4);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0003B858 File Offset: 0x0003A858
		private QilNode CompileSystemProperty(QilNode name)
		{
			if (name.NodeType == QilNodeType.LiteralString)
			{
				XmlQualifiedName xmlQualifiedName = this.ResolveQNameThrow(true, name);
				if (this.EvaluateFuncCalls)
				{
					XPathItem xpathItem = XsltFunctions.SystemProperty(xmlQualifiedName);
					if (xpathItem.ValueType == XsltConvert.StringType)
					{
						return this.f.String(xpathItem.Value);
					}
					return this.f.Double(xpathItem.ValueAsDouble);
				}
				else
				{
					name = this.f.QName(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
				}
			}
			else
			{
				name = this.ResolveQNameDynamic(true, name);
			}
			return this.f.InvokeSystemProperty(name);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0003B8EC File Offset: 0x0003A8EC
		private QilNode CompileElementAvailable(QilNode name)
		{
			if (name.NodeType == QilNodeType.LiteralString)
			{
				XmlQualifiedName xmlQualifiedName = this.ResolveQNameThrow(false, name);
				if (this.EvaluateFuncCalls)
				{
					return this.f.Boolean(QilGenerator.IsElementAvailable(xmlQualifiedName));
				}
				name = this.f.QName(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
			}
			else
			{
				name = this.ResolveQNameDynamic(false, name);
			}
			return this.f.InvokeElementAvailable(name);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0003B958 File Offset: 0x0003A958
		private QilNode CompileFunctionAvailable(QilNode name)
		{
			if (name.NodeType == QilNodeType.LiteralString)
			{
				XmlQualifiedName xmlQualifiedName = this.ResolveQNameThrow(true, name);
				if (this.EvaluateFuncCalls && (xmlQualifiedName.Namespace.Length == 0 || xmlQualifiedName.Namespace == "http://www.w3.org/1999/XSL/Transform"))
				{
					return this.f.Boolean(QilGenerator.IsFunctionAvailable(xmlQualifiedName.Name, xmlQualifiedName.Namespace));
				}
				name = this.f.QName(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
			}
			else
			{
				name = this.ResolveQNameDynamic(true, name);
			}
			return this.f.InvokeFunctionAvailable(name);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0003B9ED File Offset: 0x0003A9ED
		private QilNode CompileMsNodeSet(QilNode n)
		{
			if (n.XmlType.IsNode && n.XmlType.IsNotRtf)
			{
				return n;
			}
			return this.f.XsltConvert(n, XmlQueryTypeFactory.NodeDodS);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0003BA1C File Offset: 0x0003AA1C
		private QilNode EXslObjectType(QilNode n)
		{
			if (this.EvaluateFuncCalls)
			{
				switch (n.XmlType.TypeCode)
				{
				case XmlTypeCode.String:
					return this.f.String("string");
				case XmlTypeCode.Boolean:
					return this.f.String("boolean");
				case XmlTypeCode.Double:
					return this.f.String("number");
				}
				if (n.XmlType.IsNode && n.XmlType.IsNotRtf)
				{
					return this.f.String("node-set");
				}
			}
			return this.f.InvokeEXslObjectType(n);
		}

		// Token: 0x040007C1 RID: 1985
		private const XmlNodeKindFlags InvalidatingNodes = XmlNodeKindFlags.Attribute | XmlNodeKindFlags.Namespace;

		// Token: 0x040007C2 RID: 1986
		private CompilerScopeManager<QilIterator> scope;

		// Token: 0x040007C3 RID: 1987
		private OutputScopeManager outputScope;

		// Token: 0x040007C4 RID: 1988
		private HybridDictionary prefixesInUse;

		// Token: 0x040007C5 RID: 1989
		private XsltQilFactory f;

		// Token: 0x040007C6 RID: 1990
		private XPathBuilder xpathBuilder;

		// Token: 0x040007C7 RID: 1991
		private XPathParser<QilNode> xpathParser;

		// Token: 0x040007C8 RID: 1992
		private XPathPatternBuilder ptrnBuilder;

		// Token: 0x040007C9 RID: 1993
		private XPathPatternParser ptrnParser;

		// Token: 0x040007CA RID: 1994
		private ReferenceReplacer refReplacer;

		// Token: 0x040007CB RID: 1995
		private KeyMatchBuilder keyMatchBuilder;

		// Token: 0x040007CC RID: 1996
		private InvokeGenerator invkGen;

		// Token: 0x040007CD RID: 1997
		private MatcherBuilder matcherBuilder;

		// Token: 0x040007CE RID: 1998
		private QilStrConcatenator strConcat;

		// Token: 0x040007CF RID: 1999
		private QilGenerator.VariableHelper varHelper;

		// Token: 0x040007D0 RID: 2000
		private Compiler compiler;

		// Token: 0x040007D1 RID: 2001
		private QilList functions;

		// Token: 0x040007D2 RID: 2002
		private QilFunction generalKey;

		// Token: 0x040007D3 RID: 2003
		private bool formatNumberDynamicUsed;

		// Token: 0x040007D4 RID: 2004
		private QilList extPars;

		// Token: 0x040007D5 RID: 2005
		private QilList gloVars;

		// Token: 0x040007D6 RID: 2006
		private QilList nsVars;

		// Token: 0x040007D7 RID: 2007
		private XmlQueryType elementOrDocumentType;

		// Token: 0x040007D8 RID: 2008
		private XmlQueryType textOrAttributeType;

		// Token: 0x040007D9 RID: 2009
		private XslNode lastScope;

		// Token: 0x040007DA RID: 2010
		private XslVersion xslVersion;

		// Token: 0x040007DB RID: 2011
		private QilName nameCurrent;

		// Token: 0x040007DC RID: 2012
		private QilName namePosition;

		// Token: 0x040007DD RID: 2013
		private QilName nameLast;

		// Token: 0x040007DE RID: 2014
		private QilName nameNamespaces;

		// Token: 0x040007DF RID: 2015
		private SingletonFocus singlFocus;

		// Token: 0x040007E0 RID: 2016
		private FunctionFocus funcFocus;

		// Token: 0x040007E1 RID: 2017
		private LoopFocus curLoop;

		// Token: 0x040007E2 RID: 2018
		private int formatterCnt;

		// Token: 0x040007E3 RID: 2019
		private readonly StringBuilder unescapedText = new StringBuilder();

		// Token: 0x040007E4 RID: 2020
		private static readonly char[] curlyBraces = new char[] { '{', '}' };

		// Token: 0x040007E5 RID: 2021
		private bool allowVariables = true;

		// Token: 0x040007E6 RID: 2022
		private bool allowCurrent = true;

		// Token: 0x040007E7 RID: 2023
		private bool allowKey = true;

		// Token: 0x040007E8 RID: 2024
		private static readonly XmlTypeCode[] argFnDocument = new XmlTypeCode[]
		{
			XmlTypeCode.Item,
			XmlTypeCode.Node
		};

		// Token: 0x040007E9 RID: 2025
		private static readonly XmlTypeCode[] argFnKey = new XmlTypeCode[]
		{
			XmlTypeCode.String,
			XmlTypeCode.Item
		};

		// Token: 0x040007EA RID: 2026
		private static readonly XmlTypeCode[] argFnFormatNumber = new XmlTypeCode[]
		{
			XmlTypeCode.Double,
			XmlTypeCode.String,
			XmlTypeCode.String
		};

		// Token: 0x040007EB RID: 2027
		public static Dictionary<string, XPathBuilder.FunctionInfo<QilGenerator.FuncId>> FunctionTable = QilGenerator.CreateFunctionTable();

		// Token: 0x020000FE RID: 254
		private class VariableHelper
		{
			// Token: 0x06000B8D RID: 2957 RVA: 0x0003BB41 File Offset: 0x0003AB41
			public VariableHelper(XPathQilFactory f)
			{
				this.f = f;
			}

			// Token: 0x06000B8E RID: 2958 RVA: 0x0003BB5B File Offset: 0x0003AB5B
			public int StartVariables()
			{
				return this.vars.Count;
			}

			// Token: 0x06000B8F RID: 2959 RVA: 0x0003BB68 File Offset: 0x0003AB68
			public void AddVariable(QilIterator let)
			{
				this.vars.Push(let);
			}

			// Token: 0x06000B90 RID: 2960 RVA: 0x0003BB78 File Offset: 0x0003AB78
			public QilNode FinishVariables(QilNode node, int varScope)
			{
				int num = this.vars.Count - varScope;
				while (num-- != 0)
				{
					node = this.f.Loop(this.vars.Pop(), node);
				}
				return node;
			}

			// Token: 0x06000B91 RID: 2961 RVA: 0x0003BBB6 File Offset: 0x0003ABB6
			[Conditional("DEBUG")]
			public void CheckEmpty()
			{
			}

			// Token: 0x040007EC RID: 2028
			private Stack<QilIterator> vars = new Stack<QilIterator>();

			// Token: 0x040007ED RID: 2029
			private XPathQilFactory f;
		}

		// Token: 0x020000FF RID: 255
		private struct ThrowErrorHelper : IErrorHelper
		{
			// Token: 0x06000B92 RID: 2962 RVA: 0x0003BBB8 File Offset: 0x0003ABB8
			public void ReportError(string res, params string[] args)
			{
				throw new XslLoadException("Xml_UserException", new string[] { res });
			}

			// Token: 0x06000B93 RID: 2963 RVA: 0x0003BBDB File Offset: 0x0003ABDB
			public void ReportWarning(string res, params string[] args)
			{
			}
		}

		// Token: 0x02000100 RID: 256
		public enum FuncId
		{
			// Token: 0x040007EF RID: 2031
			Current,
			// Token: 0x040007F0 RID: 2032
			Document,
			// Token: 0x040007F1 RID: 2033
			Key,
			// Token: 0x040007F2 RID: 2034
			FormatNumber,
			// Token: 0x040007F3 RID: 2035
			UnparsedEntityUri,
			// Token: 0x040007F4 RID: 2036
			GenerateId,
			// Token: 0x040007F5 RID: 2037
			SystemProperty,
			// Token: 0x040007F6 RID: 2038
			ElementAvailable,
			// Token: 0x040007F7 RID: 2039
			FunctionAvailable
		}
	}
}
