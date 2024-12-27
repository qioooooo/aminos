using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using System.Xml.XPath;
using System.Xml.Xsl.XsltOld.Debugger;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000189 RID: 393
	internal sealed class Processor : IXsltProcessor
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x0004FC2C File Offset: 0x0004EC2C
		internal XPathNavigator Current
		{
			get
			{
				ActionFrame actionFrame = (ActionFrame)this.actionStack.Peek();
				if (actionFrame == null)
				{
					return null;
				}
				return actionFrame.Node;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x0004FC55 File Offset: 0x0004EC55
		// (set) Token: 0x0600106C RID: 4204 RVA: 0x0004FC5D File Offset: 0x0004EC5D
		internal Processor.ExecResult ExecutionResult
		{
			get
			{
				return this.execResult;
			}
			set
			{
				this.execResult = value;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x0004FC66 File Offset: 0x0004EC66
		internal Stylesheet Stylesheet
		{
			get
			{
				return this.stylesheet;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x0004FC6E File Offset: 0x0004EC6E
		internal XmlResolver Resolver
		{
			get
			{
				return this.resolver;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x0004FC76 File Offset: 0x0004EC76
		internal ArrayList SortArray
		{
			get
			{
				return this.sortArray;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x0004FC7E File Offset: 0x0004EC7E
		internal Key[] KeyList
		{
			get
			{
				return this.keyList;
			}
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x0004FC88 File Offset: 0x0004EC88
		internal XPathNavigator GetNavigator(Uri ruri)
		{
			XPathNavigator xpathNavigator;
			if (this.documentCache != null)
			{
				xpathNavigator = this.documentCache[ruri] as XPathNavigator;
				if (xpathNavigator != null)
				{
					return xpathNavigator.Clone();
				}
			}
			else
			{
				this.documentCache = new Hashtable();
			}
			object entity = this.resolver.GetEntity(ruri, null, null);
			if (entity is Stream)
			{
				xpathNavigator = ((IXPathNavigable)Compiler.LoadDocument(new XmlTextReaderImpl(ruri.ToString(), (Stream)entity)
				{
					XmlResolver = this.resolver
				})).CreateNavigator();
			}
			else
			{
				if (!(entity is XPathNavigator))
				{
					throw XsltException.Create("Xslt_CantResolve", new string[] { ruri.ToString() });
				}
				xpathNavigator = (XPathNavigator)entity;
			}
			this.documentCache[ruri] = xpathNavigator.Clone();
			return xpathNavigator;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0004FD49 File Offset: 0x0004ED49
		internal void AddSort(Sort sortinfo)
		{
			this.sortArray.Add(sortinfo);
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x0004FD58 File Offset: 0x0004ED58
		internal void InitSortArray()
		{
			if (this.sortArray == null)
			{
				this.sortArray = new ArrayList();
				return;
			}
			this.sortArray.Clear();
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0004FD7C File Offset: 0x0004ED7C
		internal object GetGlobalParameter(XmlQualifiedName qname)
		{
			object obj = this.args.GetParam(qname.Name, qname.Namespace);
			if (obj == null)
			{
				return null;
			}
			if (!(obj is XPathNodeIterator) && !(obj is XPathNavigator) && !(obj is bool) && !(obj is double) && !(obj is string))
			{
				if (obj is short || obj is ushort || obj is int || obj is uint || obj is long || obj is ulong || obj is float || obj is decimal)
				{
					obj = XmlConvert.ToXPathDouble(obj);
				}
				else
				{
					obj = obj.ToString();
				}
			}
			return obj;
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0004FE24 File Offset: 0x0004EE24
		internal object GetExtensionObject(string nsUri)
		{
			return this.args.GetExtensionObject(nsUri);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0004FE32 File Offset: 0x0004EE32
		internal object GetScriptObject(string nsUri)
		{
			return this.scriptExtensions[nsUri];
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x0004FE40 File Offset: 0x0004EE40
		internal RootAction RootAction
		{
			get
			{
				return this.rootAction;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x0004FE48 File Offset: 0x0004EE48
		internal XPathNavigator Document
		{
			get
			{
				return this.document;
			}
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0004FE50 File Offset: 0x0004EE50
		internal StringBuilder GetSharedStringBuilder()
		{
			if (this.sharedStringBuilder == null)
			{
				this.sharedStringBuilder = new StringBuilder();
			}
			else
			{
				this.sharedStringBuilder.Length = 0;
			}
			return this.sharedStringBuilder;
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0004FE79 File Offset: 0x0004EE79
		internal void ReleaseSharedStringBuilder()
		{
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x0004FE7B File Offset: 0x0004EE7B
		internal ArrayList NumberList
		{
			get
			{
				if (this.numberList == null)
				{
					this.numberList = new ArrayList();
				}
				return this.numberList;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x0004FE96 File Offset: 0x0004EE96
		internal IXsltDebugger Debugger
		{
			get
			{
				return this.debugger;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x0004FE9E File Offset: 0x0004EE9E
		internal HWStack ActionStack
		{
			get
			{
				return this.actionStack;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x0004FEA6 File Offset: 0x0004EEA6
		internal RecordBuilder Builder
		{
			get
			{
				return this.builder;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0004FEAE File Offset: 0x0004EEAE
		internal XsltOutput Output
		{
			get
			{
				return this.output;
			}
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0004FEB8 File Offset: 0x0004EEB8
		public Processor(XPathNavigator doc, XsltArgumentList args, XmlResolver resolver, Stylesheet stylesheet, List<TheQuery> queryStore, RootAction rootAction, IXsltDebugger debugger)
		{
			this.stylesheet = stylesheet;
			this.queryStore = queryStore;
			this.rootAction = rootAction;
			this.queryList = new Query[queryStore.Count];
			for (int i = 0; i < queryStore.Count; i++)
			{
				this.queryList[i] = Query.Clone(queryStore[i].CompiledQuery.QueryTree);
			}
			this.xsm = new StateMachine();
			this.document = doc;
			this.builder = null;
			this.actionStack = new HWStack(10);
			this.output = this.rootAction.Output;
			this.permissions = this.rootAction.permissions;
			this.resolver = ((resolver != null) ? resolver : new XmlNullResolver());
			this.args = ((args != null) ? args : new XsltArgumentList());
			this.debugger = debugger;
			if (this.debugger != null)
			{
				this.debuggerStack = new HWStack(10, 1000);
				this.templateLookup = new TemplateLookupActionDbg();
			}
			if (this.rootAction.KeyList != null)
			{
				this.keyList = new Key[this.rootAction.KeyList.Count];
				for (int j = 0; j < this.keyList.Length; j++)
				{
					this.keyList[j] = this.rootAction.KeyList[j].Clone();
				}
			}
			this.scriptExtensions = new Hashtable(this.stylesheet.ScriptObjectTypes.Count);
			foreach (object obj in this.stylesheet.ScriptObjectTypes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = (string)dictionaryEntry.Key;
				if (this.GetExtensionObject(text) != null)
				{
					throw XsltException.Create("Xslt_ScriptDub", new string[] { text });
				}
				this.scriptExtensions.Add(text, Activator.CreateInstance((Type)dictionaryEntry.Value, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null));
			}
			this.PushActionFrame(this.rootAction, null);
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x000500FC File Offset: 0x0004F0FC
		public ReaderOutput StartReader()
		{
			ReaderOutput readerOutput = new ReaderOutput(this);
			this.builder = new RecordBuilder(readerOutput, this.nameTable);
			return readerOutput;
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00050124 File Offset: 0x0004F124
		public void Execute(Stream stream)
		{
			RecordOutput recordOutput = null;
			switch (this.output.Method)
			{
			case XsltOutput.OutputMethod.Xml:
			case XsltOutput.OutputMethod.Html:
			case XsltOutput.OutputMethod.Other:
			case XsltOutput.OutputMethod.Unknown:
				recordOutput = new TextOutput(this, stream);
				break;
			case XsltOutput.OutputMethod.Text:
				recordOutput = new TextOnlyOutput(this, stream);
				break;
			}
			this.builder = new RecordBuilder(recordOutput, this.nameTable);
			this.Execute();
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00050188 File Offset: 0x0004F188
		public void Execute(TextWriter writer)
		{
			RecordOutput recordOutput = null;
			switch (this.output.Method)
			{
			case XsltOutput.OutputMethod.Xml:
			case XsltOutput.OutputMethod.Html:
			case XsltOutput.OutputMethod.Other:
			case XsltOutput.OutputMethod.Unknown:
				recordOutput = new TextOutput(this, writer);
				break;
			case XsltOutput.OutputMethod.Text:
				recordOutput = new TextOnlyOutput(this, writer);
				break;
			}
			this.builder = new RecordBuilder(recordOutput, this.nameTable);
			this.Execute();
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x000501E9 File Offset: 0x0004F1E9
		public void Execute(XmlWriter writer)
		{
			this.builder = new RecordBuilder(new WriterOutput(this, writer), this.nameTable);
			this.Execute();
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0005020C File Offset: 0x0004F20C
		internal void Execute()
		{
			while (this.execResult == Processor.ExecResult.Continue)
			{
				ActionFrame actionFrame = (ActionFrame)this.actionStack.Peek();
				if (actionFrame == null)
				{
					this.builder.TheEnd();
					this.ExecutionResult = Processor.ExecResult.Done;
					break;
				}
				if (actionFrame.Execute(this))
				{
					this.actionStack.Pop();
				}
			}
			if (this.execResult == Processor.ExecResult.Interrupt)
			{
				this.execResult = Processor.ExecResult.Continue;
			}
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00050270 File Offset: 0x0004F270
		internal ActionFrame PushNewFrame()
		{
			ActionFrame actionFrame = (ActionFrame)this.actionStack.Peek();
			ActionFrame actionFrame2 = (ActionFrame)this.actionStack.Push();
			if (actionFrame2 == null)
			{
				actionFrame2 = new ActionFrame();
				this.actionStack.AddToTop(actionFrame2);
			}
			if (actionFrame != null)
			{
				actionFrame2.Inherit(actionFrame);
			}
			return actionFrame2;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x000502C0 File Offset: 0x0004F2C0
		internal void PushActionFrame(Action action, XPathNodeIterator nodeSet)
		{
			ActionFrame actionFrame = this.PushNewFrame();
			actionFrame.Init(action, nodeSet);
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x000502DC File Offset: 0x0004F2DC
		internal void PushActionFrame(ActionFrame container)
		{
			this.PushActionFrame(container, container.NodeSet);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x000502EC File Offset: 0x0004F2EC
		internal void PushActionFrame(ActionFrame container, XPathNodeIterator nodeSet)
		{
			ActionFrame actionFrame = this.PushNewFrame();
			actionFrame.Init(container, nodeSet);
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00050308 File Offset: 0x0004F308
		internal void PushTemplateLookup(XPathNodeIterator nodeSet, XmlQualifiedName mode, Stylesheet importsOf)
		{
			this.templateLookup.Initialize(mode, importsOf);
			this.PushActionFrame(this.templateLookup, nodeSet);
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00050324 File Offset: 0x0004F324
		internal string GetQueryExpression(int key)
		{
			return this.queryStore[key].CompiledQuery.Expression;
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0005033C File Offset: 0x0004F33C
		internal Query GetCompiledQuery(int key)
		{
			TheQuery theQuery = this.queryStore[key];
			theQuery.CompiledQuery.CheckErrors();
			Query query = Query.Clone(this.queryList[key]);
			query.SetXsltContext(new XsltCompileContext(theQuery._ScopeManager, this));
			return query;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00050382 File Offset: 0x0004F382
		internal Query GetValueQuery(int key)
		{
			return this.GetValueQuery(key, null);
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0005038C File Offset: 0x0004F38C
		internal Query GetValueQuery(int key, XsltCompileContext context)
		{
			TheQuery theQuery = this.queryStore[key];
			theQuery.CompiledQuery.CheckErrors();
			Query query = this.queryList[key];
			if (context == null)
			{
				context = new XsltCompileContext(theQuery._ScopeManager, this);
			}
			else
			{
				context.Reinitialize(theQuery._ScopeManager, this);
			}
			query.SetXsltContext(context);
			return query;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x000503E2 File Offset: 0x0004F3E2
		private XsltCompileContext GetValueOfContext()
		{
			if (this.valueOfContext == null)
			{
				this.valueOfContext = new XsltCompileContext();
			}
			return this.valueOfContext;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x000503FD File Offset: 0x0004F3FD
		[Conditional("DEBUG")]
		private void RecycleValueOfContext()
		{
			if (this.valueOfContext != null)
			{
				this.valueOfContext.Recycle();
			}
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00050412 File Offset: 0x0004F412
		private XsltCompileContext GetMatchesContext()
		{
			if (this.matchesContext == null)
			{
				this.matchesContext = new XsltCompileContext();
			}
			return this.matchesContext;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0005042D File Offset: 0x0004F42D
		[Conditional("DEBUG")]
		private void RecycleMatchesContext()
		{
			if (this.matchesContext != null)
			{
				this.matchesContext.Recycle();
			}
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00050444 File Offset: 0x0004F444
		internal string ValueOf(ActionFrame context, int key)
		{
			Query valueQuery = this.GetValueQuery(key, this.GetValueOfContext());
			object obj = valueQuery.Evaluate(context.NodeSet);
			string text;
			if (obj is XPathNodeIterator)
			{
				XPathNavigator xpathNavigator = valueQuery.Advance();
				text = ((xpathNavigator != null) ? this.ValueOf(xpathNavigator) : string.Empty);
			}
			else
			{
				text = XmlConvert.ToXPathString(obj);
			}
			return text;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00050498 File Offset: 0x0004F498
		internal string ValueOf(XPathNavigator n)
		{
			if (this.stylesheet.Whitespace && n.NodeType == XPathNodeType.Element)
			{
				StringBuilder stringBuilder = this.GetSharedStringBuilder();
				this.ElementValueWithoutWS(n, stringBuilder);
				this.ReleaseSharedStringBuilder();
				return stringBuilder.ToString();
			}
			return n.Value;
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x000504E0 File Offset: 0x0004F4E0
		private void ElementValueWithoutWS(XPathNavigator nav, StringBuilder builder)
		{
			bool flag = this.Stylesheet.PreserveWhiteSpace(this, nav);
			if (nav.MoveToFirstChild())
			{
				do
				{
					switch (nav.NodeType)
					{
					case XPathNodeType.Element:
						this.ElementValueWithoutWS(nav, builder);
						break;
					case XPathNodeType.Text:
					case XPathNodeType.SignificantWhitespace:
						builder.Append(nav.Value);
						break;
					case XPathNodeType.Whitespace:
						if (flag)
						{
							builder.Append(nav.Value);
						}
						break;
					}
				}
				while (nav.MoveToNext());
				nav.MoveToParent();
			}
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00050564 File Offset: 0x0004F564
		internal XPathNodeIterator StartQuery(XPathNodeIterator context, int key)
		{
			Query compiledQuery = this.GetCompiledQuery(key);
			object obj = compiledQuery.Evaluate(context);
			if (obj is XPathNodeIterator)
			{
				return new XPathSelectionIterator(context.Current, compiledQuery);
			}
			throw XsltException.Create("XPath_NodeSetExpected", new string[0]);
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x000505A6 File Offset: 0x0004F5A6
		internal object Evaluate(ActionFrame context, int key)
		{
			return this.GetValueQuery(key).Evaluate(context.NodeSet);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x000505BC File Offset: 0x0004F5BC
		internal object RunQuery(ActionFrame context, int key)
		{
			Query compiledQuery = this.GetCompiledQuery(key);
			object obj = compiledQuery.Evaluate(context.NodeSet);
			XPathNodeIterator xpathNodeIterator = obj as XPathNodeIterator;
			if (xpathNodeIterator != null)
			{
				return new XPathArrayIterator(xpathNodeIterator);
			}
			return obj;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x000505F0 File Offset: 0x0004F5F0
		internal string EvaluateString(ActionFrame context, int key)
		{
			object obj = this.Evaluate(context, key);
			string text = null;
			if (obj != null)
			{
				text = XmlConvert.ToXPathString(obj);
			}
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0005061C File Offset: 0x0004F61C
		internal bool EvaluateBoolean(ActionFrame context, int key)
		{
			object obj = this.Evaluate(context, key);
			if (obj == null)
			{
				return false;
			}
			XPathNavigator xpathNavigator = obj as XPathNavigator;
			if (xpathNavigator == null)
			{
				return Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
			}
			return Convert.ToBoolean(xpathNavigator.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00050660 File Offset: 0x0004F660
		internal bool Matches(XPathNavigator context, int key)
		{
			Query valueQuery = this.GetValueQuery(key, this.GetMatchesContext());
			bool flag2;
			try
			{
				bool flag = valueQuery.MatchNode(context) != null;
				flag2 = flag;
			}
			catch (XPathException)
			{
				throw XsltException.Create("Xslt_InvalidPattern", new string[] { this.GetQueryExpression(key) });
			}
			return flag2;
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600109C RID: 4252 RVA: 0x000506BC File Offset: 0x0004F6BC
		internal XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x0600109D RID: 4253 RVA: 0x000506C4 File Offset: 0x0004F6C4
		internal bool CanContinue
		{
			get
			{
				return this.execResult == Processor.ExecResult.Continue;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x000506CF File Offset: 0x0004F6CF
		internal bool ExecutionDone
		{
			get
			{
				return this.execResult == Processor.ExecResult.Done;
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x000506DA File Offset: 0x0004F6DA
		internal void ResetOutput()
		{
			this.builder.Reset();
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x000506E7 File Offset: 0x0004F6E7
		internal bool BeginEvent(XPathNodeType nodeType, string prefix, string name, string nspace, bool empty)
		{
			return this.BeginEvent(nodeType, prefix, name, nspace, empty, null, true);
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x000506F8 File Offset: 0x0004F6F8
		internal bool BeginEvent(XPathNodeType nodeType, string prefix, string name, string nspace, bool empty, object htmlProps, bool search)
		{
			int num = this.xsm.BeginOutlook(nodeType);
			if (this.ignoreLevel > 0 || num == 16)
			{
				this.ignoreLevel++;
				return true;
			}
			switch (this.builder.BeginEvent(num, nodeType, prefix, name, nspace, empty, htmlProps, search))
			{
			case Processor.OutputResult.Continue:
				this.xsm.Begin(nodeType);
				return true;
			case Processor.OutputResult.Interrupt:
				this.xsm.Begin(nodeType);
				this.ExecutionResult = Processor.ExecResult.Interrupt;
				return true;
			case Processor.OutputResult.Overflow:
				this.ExecutionResult = Processor.ExecResult.Interrupt;
				return false;
			case Processor.OutputResult.Error:
				this.ignoreLevel++;
				return true;
			case Processor.OutputResult.Ignore:
				return true;
			default:
				return true;
			}
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x000507A5 File Offset: 0x0004F7A5
		internal bool TextEvent(string text)
		{
			return this.TextEvent(text, false);
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x000507B0 File Offset: 0x0004F7B0
		internal bool TextEvent(string text, bool disableOutputEscaping)
		{
			if (this.ignoreLevel > 0)
			{
				return true;
			}
			int num = this.xsm.BeginOutlook(XPathNodeType.Text);
			switch (this.builder.TextEvent(num, text, disableOutputEscaping))
			{
			case Processor.OutputResult.Continue:
				this.xsm.Begin(XPathNodeType.Text);
				return true;
			case Processor.OutputResult.Interrupt:
				this.xsm.Begin(XPathNodeType.Text);
				this.ExecutionResult = Processor.ExecResult.Interrupt;
				return true;
			case Processor.OutputResult.Overflow:
				this.ExecutionResult = Processor.ExecResult.Interrupt;
				return false;
			case Processor.OutputResult.Error:
			case Processor.OutputResult.Ignore:
				return true;
			default:
				return true;
			}
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00050834 File Offset: 0x0004F834
		internal bool EndEvent(XPathNodeType nodeType)
		{
			if (this.ignoreLevel > 0)
			{
				this.ignoreLevel--;
				return true;
			}
			int num = this.xsm.EndOutlook(nodeType);
			switch (this.builder.EndEvent(num, nodeType))
			{
			case Processor.OutputResult.Continue:
				this.xsm.End(nodeType);
				return true;
			case Processor.OutputResult.Interrupt:
				this.xsm.End(nodeType);
				this.ExecutionResult = Processor.ExecResult.Interrupt;
				return true;
			case Processor.OutputResult.Overflow:
				this.ExecutionResult = Processor.ExecResult.Interrupt;
				return false;
			}
			return true;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x000508C0 File Offset: 0x0004F8C0
		internal bool CopyBeginEvent(XPathNavigator node, bool emptyflag)
		{
			switch (node.NodeType)
			{
			case XPathNodeType.Element:
			case XPathNodeType.Attribute:
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
				return this.BeginEvent(node.NodeType, node.Prefix, node.LocalName, node.NamespaceURI, emptyflag);
			case XPathNodeType.Namespace:
				return this.BeginEvent(XPathNodeType.Namespace, null, node.LocalName, node.Value, false);
			}
			return true;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0005093C File Offset: 0x0004F93C
		internal bool CopyTextEvent(XPathNavigator node)
		{
			switch (node.NodeType)
			{
			case XPathNodeType.Attribute:
			case XPathNodeType.Text:
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
			{
				string value = node.Value;
				return this.TextEvent(value);
			}
			}
			return true;
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00050990 File Offset: 0x0004F990
		internal bool CopyEndEvent(XPathNavigator node)
		{
			switch (node.NodeType)
			{
			case XPathNodeType.Element:
			case XPathNodeType.Attribute:
			case XPathNodeType.Namespace:
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
				return this.EndEvent(node.NodeType);
			}
			return true;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x000509E4 File Offset: 0x0004F9E4
		internal static bool IsRoot(XPathNavigator navigator)
		{
			if (navigator.NodeType == XPathNodeType.Root)
			{
				return true;
			}
			if (navigator.NodeType == XPathNodeType.Element)
			{
				XPathNavigator xpathNavigator = navigator.Clone();
				xpathNavigator.MoveToRoot();
				return xpathNavigator.IsSamePosition(navigator);
			}
			return false;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00050A1C File Offset: 0x0004FA1C
		internal void PushOutput(RecordOutput output)
		{
			this.builder.OutputState = this.xsm.State;
			RecordBuilder recordBuilder = this.builder;
			this.builder = new RecordBuilder(output, this.nameTable);
			this.builder.Next = recordBuilder;
			this.xsm.Reset();
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x00050A70 File Offset: 0x0004FA70
		internal RecordOutput PopOutput()
		{
			RecordBuilder recordBuilder = this.builder;
			this.builder = recordBuilder.Next;
			this.xsm.State = this.builder.OutputState;
			recordBuilder.TheEnd();
			return recordBuilder.Output;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00050AB2 File Offset: 0x0004FAB2
		internal bool SetDefaultOutput(XsltOutput.OutputMethod method)
		{
			if (this.Output.Method != method)
			{
				this.output = this.output.CreateDerivedOutput(method);
				return true;
			}
			return false;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00050AD8 File Offset: 0x0004FAD8
		internal object GetVariableValue(VariableAction variable)
		{
			int varKey = variable.VarKey;
			if (!variable.IsGlobal)
			{
				return ((ActionFrame)this.actionStack.Peek()).GetVariable(varKey);
			}
			ActionFrame actionFrame = (ActionFrame)this.actionStack[0];
			object variable2 = actionFrame.GetVariable(varKey);
			if (variable2 == VariableAction.BeingComputedMark)
			{
				throw XsltException.Create("Xslt_CircularReference", new string[] { variable.NameStr });
			}
			if (variable2 != null)
			{
				return variable2;
			}
			int length = this.actionStack.Length;
			ActionFrame actionFrame2 = this.PushNewFrame();
			actionFrame2.Inherit(actionFrame);
			actionFrame2.Init(variable, actionFrame.NodeSet);
			do
			{
				bool flag = ((ActionFrame)this.actionStack.Peek()).Execute(this);
				if (flag)
				{
					this.actionStack.Pop();
				}
			}
			while (length < this.actionStack.Length);
			return actionFrame.GetVariable(varKey);
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x00050BC0 File Offset: 0x0004FBC0
		internal void SetParameter(XmlQualifiedName name, object value)
		{
			ActionFrame actionFrame = (ActionFrame)this.actionStack[this.actionStack.Length - 2];
			actionFrame.SetParameter(name, value);
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x00050BF4 File Offset: 0x0004FBF4
		internal void ResetParams()
		{
			ActionFrame actionFrame = (ActionFrame)this.actionStack[this.actionStack.Length - 1];
			actionFrame.ResetParams();
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x00050C28 File Offset: 0x0004FC28
		internal object GetParameter(XmlQualifiedName name)
		{
			ActionFrame actionFrame = (ActionFrame)this.actionStack[this.actionStack.Length - 3];
			return actionFrame.GetParameter(name);
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x00050C5C File Offset: 0x0004FC5C
		internal void PushDebuggerStack()
		{
			Processor.DebuggerFrame debuggerFrame = (Processor.DebuggerFrame)this.debuggerStack.Push();
			if (debuggerFrame == null)
			{
				debuggerFrame = new Processor.DebuggerFrame();
				this.debuggerStack.AddToTop(debuggerFrame);
			}
			debuggerFrame.actionFrame = (ActionFrame)this.actionStack.Peek();
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00050CA5 File Offset: 0x0004FCA5
		internal void PopDebuggerStack()
		{
			this.debuggerStack.Pop();
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00050CB4 File Offset: 0x0004FCB4
		internal void OnInstructionExecute()
		{
			Processor.DebuggerFrame debuggerFrame = (Processor.DebuggerFrame)this.debuggerStack.Peek();
			debuggerFrame.actionFrame = (ActionFrame)this.actionStack.Peek();
			this.Debugger.OnInstructionExecute(this);
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00050CF4 File Offset: 0x0004FCF4
		internal XmlQualifiedName GetPrevioseMode()
		{
			return ((Processor.DebuggerFrame)this.debuggerStack[this.debuggerStack.Length - 2]).currentMode;
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00050D18 File Offset: 0x0004FD18
		internal void SetCurrentMode(XmlQualifiedName mode)
		{
			((Processor.DebuggerFrame)this.debuggerStack[this.debuggerStack.Length - 1]).currentMode = mode;
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x00050D3D File Offset: 0x0004FD3D
		int IXsltProcessor.StackDepth
		{
			get
			{
				return this.debuggerStack.Length;
			}
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x00050D4A File Offset: 0x0004FD4A
		IStackFrame IXsltProcessor.GetStackFrame(int depth)
		{
			return ((Processor.DebuggerFrame)this.debuggerStack[depth]).actionFrame;
		}

		// Token: 0x04000AFF RID: 2815
		private const int StackIncrement = 10;

		// Token: 0x04000B00 RID: 2816
		private Processor.ExecResult execResult;

		// Token: 0x04000B01 RID: 2817
		private Stylesheet stylesheet;

		// Token: 0x04000B02 RID: 2818
		private RootAction rootAction;

		// Token: 0x04000B03 RID: 2819
		private Key[] keyList;

		// Token: 0x04000B04 RID: 2820
		private List<TheQuery> queryStore;

		// Token: 0x04000B05 RID: 2821
		public PermissionSet permissions;

		// Token: 0x04000B06 RID: 2822
		private XPathNavigator document;

		// Token: 0x04000B07 RID: 2823
		private HWStack actionStack;

		// Token: 0x04000B08 RID: 2824
		private HWStack debuggerStack;

		// Token: 0x04000B09 RID: 2825
		private StringBuilder sharedStringBuilder;

		// Token: 0x04000B0A RID: 2826
		private int ignoreLevel;

		// Token: 0x04000B0B RID: 2827
		private StateMachine xsm;

		// Token: 0x04000B0C RID: 2828
		private RecordBuilder builder;

		// Token: 0x04000B0D RID: 2829
		private XsltOutput output;

		// Token: 0x04000B0E RID: 2830
		private XmlNameTable nameTable = new NameTable();

		// Token: 0x04000B0F RID: 2831
		private XmlResolver resolver;

		// Token: 0x04000B10 RID: 2832
		private XsltArgumentList args;

		// Token: 0x04000B11 RID: 2833
		private Hashtable scriptExtensions;

		// Token: 0x04000B12 RID: 2834
		private ArrayList numberList;

		// Token: 0x04000B13 RID: 2835
		private TemplateLookupAction templateLookup = new TemplateLookupAction();

		// Token: 0x04000B14 RID: 2836
		private IXsltDebugger debugger;

		// Token: 0x04000B15 RID: 2837
		private Query[] queryList;

		// Token: 0x04000B16 RID: 2838
		private ArrayList sortArray;

		// Token: 0x04000B17 RID: 2839
		private Hashtable documentCache;

		// Token: 0x04000B18 RID: 2840
		private XsltCompileContext valueOfContext;

		// Token: 0x04000B19 RID: 2841
		private XsltCompileContext matchesContext;

		// Token: 0x0200018A RID: 394
		internal enum ExecResult
		{
			// Token: 0x04000B1B RID: 2843
			Continue,
			// Token: 0x04000B1C RID: 2844
			Interrupt,
			// Token: 0x04000B1D RID: 2845
			Done
		}

		// Token: 0x0200018B RID: 395
		internal enum OutputResult
		{
			// Token: 0x04000B1F RID: 2847
			Continue,
			// Token: 0x04000B20 RID: 2848
			Interrupt,
			// Token: 0x04000B21 RID: 2849
			Overflow,
			// Token: 0x04000B22 RID: 2850
			Error,
			// Token: 0x04000B23 RID: 2851
			Ignore
		}

		// Token: 0x0200018C RID: 396
		internal class DebuggerFrame
		{
			// Token: 0x04000B24 RID: 2852
			internal ActionFrame actionFrame;

			// Token: 0x04000B25 RID: 2853
			internal XmlQualifiedName currentMode;
		}
	}
}
