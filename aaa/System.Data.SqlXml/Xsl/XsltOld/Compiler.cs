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
using System.Security.Policy;
using System.Text;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;
using System.Xml.Xsl.XsltOld.Debugger;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000141 RID: 321
	internal class Compiler
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x00047C19 File Offset: 0x00046C19
		internal Keywords Atoms
		{
			get
			{
				return this.atoms;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x00047C21 File Offset: 0x00046C21
		// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x00047C29 File Offset: 0x00046C29
		internal int Stylesheetid
		{
			get
			{
				return this.stylesheetid;
			}
			set
			{
				this.stylesheetid = value;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x00047C32 File Offset: 0x00046C32
		internal NavigatorInput Document
		{
			get
			{
				return this.input;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x00047C3A File Offset: 0x00046C3A
		internal NavigatorInput Input
		{
			get
			{
				return this.input;
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00047C42 File Offset: 0x00046C42
		internal bool Advance()
		{
			return this.Document.Advance();
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00047C4F File Offset: 0x00046C4F
		internal bool Recurse()
		{
			return this.Document.Recurse();
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00047C5C File Offset: 0x00046C5C
		internal bool ToParent()
		{
			return this.Document.ToParent();
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x00047C69 File Offset: 0x00046C69
		internal Stylesheet CompiledStylesheet
		{
			get
			{
				return this.stylesheet;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00047C71 File Offset: 0x00046C71
		// (set) Token: 0x06000DEA RID: 3562 RVA: 0x00047C79 File Offset: 0x00046C79
		internal RootAction RootAction
		{
			get
			{
				return this.rootAction;
			}
			set
			{
				this.rootAction = value;
				this.currentTemplate = this.rootAction;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x00047C8E File Offset: 0x00046C8E
		internal List<TheQuery> QueryStore
		{
			get
			{
				return this.queryStore;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x00047C96 File Offset: 0x00046C96
		public virtual IXsltDebugger Debugger
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00047C99 File Offset: 0x00046C99
		internal string GetUnicRtfId()
		{
			this.rtfCount++;
			return this.rtfCount.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00047CBC File Offset: 0x00046CBC
		internal void Compile(NavigatorInput input, XmlResolver xmlResolver, Evidence evidence)
		{
			this.xmlResolver = xmlResolver;
			this.PushInputDocument(input);
			this.rootScope = this.scopeManager.PushScope();
			this.queryStore = new List<TheQuery>();
			try
			{
				this.rootStylesheet = new Stylesheet();
				this.PushStylesheet(this.rootStylesheet);
				try
				{
					this.CreateRootAction();
				}
				catch (XsltCompileException)
				{
					throw;
				}
				catch (Exception ex)
				{
					throw new XsltCompileException(ex, this.Input.BaseURI, this.Input.LineNumber, this.Input.LinePosition);
				}
				this.stylesheet.ProcessTemplates();
				this.rootAction.PorcessAttributeSets(this.rootStylesheet);
				this.stylesheet.SortWhiteSpace();
				this.CompileScript(evidence);
				if (evidence != null)
				{
					this.rootAction.permissions = SecurityManager.ResolvePolicy(evidence);
				}
				if (this.globalNamespaceAliasTable != null)
				{
					this.stylesheet.ReplaceNamespaceAlias(this);
					this.rootAction.ReplaceNamespaceAlias(this);
				}
			}
			finally
			{
				this.PopInputDocument();
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x00047DD4 File Offset: 0x00046DD4
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x00047DE6 File Offset: 0x00046DE6
		internal bool ForwardCompatibility
		{
			get
			{
				return this.scopeManager.CurrentScope.ForwardCompatibility;
			}
			set
			{
				this.scopeManager.CurrentScope.ForwardCompatibility = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x00047DF9 File Offset: 0x00046DF9
		// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x00047E0B File Offset: 0x00046E0B
		internal bool CanHaveApplyImports
		{
			get
			{
				return this.scopeManager.CurrentScope.CanHaveApplyImports;
			}
			set
			{
				this.scopeManager.CurrentScope.CanHaveApplyImports = value;
			}
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00047E20 File Offset: 0x00046E20
		internal void InsertExtensionNamespace(string value)
		{
			string[] array = this.ResolvePrefixes(value);
			if (array != null)
			{
				this.scopeManager.InsertExtensionNamespaces(array);
			}
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00047E44 File Offset: 0x00046E44
		internal void InsertExcludedNamespace(string value)
		{
			string[] array = this.ResolvePrefixes(value);
			if (array != null)
			{
				this.scopeManager.InsertExcludedNamespaces(array);
			}
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00047E68 File Offset: 0x00046E68
		internal void InsertExtensionNamespace()
		{
			this.InsertExtensionNamespace(this.Input.Navigator.GetAttribute(this.Input.Atoms.ExtensionElementPrefixes, this.Input.Atoms.XsltNamespace));
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00047EA0 File Offset: 0x00046EA0
		internal void InsertExcludedNamespace()
		{
			this.InsertExcludedNamespace(this.Input.Navigator.GetAttribute(this.Input.Atoms.ExcludeResultPrefixes, this.Input.Atoms.XsltNamespace));
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00047ED8 File Offset: 0x00046ED8
		internal bool IsExtensionNamespace(string nspace)
		{
			return this.scopeManager.IsExtensionNamespace(nspace);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00047EE6 File Offset: 0x00046EE6
		internal bool IsExcludedNamespace(string nspace)
		{
			return this.scopeManager.IsExcludedNamespace(nspace);
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00047EF4 File Offset: 0x00046EF4
		internal void PushLiteralScope()
		{
			this.PushNamespaceScope();
			string attribute = this.Input.Navigator.GetAttribute(this.Atoms.Version, this.Atoms.XsltNamespace);
			if (attribute.Length != 0)
			{
				this.ForwardCompatibility = attribute != "1.0";
			}
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00047F48 File Offset: 0x00046F48
		internal void PushNamespaceScope()
		{
			this.scopeManager.PushScope();
			NavigatorInput navigatorInput = this.Input;
			if (navigatorInput.MoveToFirstNamespace())
			{
				do
				{
					this.scopeManager.PushNamespace(navigatorInput.LocalName, navigatorInput.Value);
				}
				while (navigatorInput.MoveToNextNamespace());
				navigatorInput.ToParent();
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000DFB RID: 3579 RVA: 0x00047F96 File Offset: 0x00046F96
		protected InputScopeManager ScopeManager
		{
			get
			{
				return this.scopeManager;
			}
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00047F9E File Offset: 0x00046F9E
		internal virtual void PopScope()
		{
			this.currentTemplate.ReleaseVariableSlots(this.scopeManager.CurrentScope.GetVeriablesCount());
			this.scopeManager.PopScope();
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00047FC6 File Offset: 0x00046FC6
		internal InputScopeManager CloneScopeManager()
		{
			return this.scopeManager.Clone();
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00047FD4 File Offset: 0x00046FD4
		internal int InsertVariable(VariableAction variable)
		{
			InputScope variableScope;
			if (variable.IsGlobal)
			{
				variableScope = this.rootScope;
			}
			else
			{
				variableScope = this.scopeManager.VariableScope;
			}
			VariableAction variableAction = variableScope.ResolveVariable(variable.Name);
			if (variableAction != null)
			{
				if (!variableAction.IsGlobal)
				{
					throw XsltException.Create("Xslt_DupVarName", new string[] { variable.NameStr });
				}
				if (variable.IsGlobal)
				{
					if (variable.Stylesheetid == variableAction.Stylesheetid)
					{
						throw XsltException.Create("Xslt_DupVarName", new string[] { variable.NameStr });
					}
					if (variable.Stylesheetid < variableAction.Stylesheetid)
					{
						variableScope.InsertVariable(variable);
						return variableAction.VarKey;
					}
					return -1;
				}
			}
			variableScope.InsertVariable(variable);
			return this.currentTemplate.AllocateVariableSlot();
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00048094 File Offset: 0x00047094
		internal void AddNamespaceAlias(string StylesheetURI, NamespaceInfo AliasInfo)
		{
			if (this.globalNamespaceAliasTable == null)
			{
				this.globalNamespaceAliasTable = new Hashtable();
			}
			NamespaceInfo namespaceInfo = this.globalNamespaceAliasTable[StylesheetURI] as NamespaceInfo;
			if (namespaceInfo == null || AliasInfo.stylesheetId <= namespaceInfo.stylesheetId)
			{
				this.globalNamespaceAliasTable[StylesheetURI] = AliasInfo;
			}
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x000480E4 File Offset: 0x000470E4
		internal bool IsNamespaceAlias(string StylesheetURI)
		{
			return this.globalNamespaceAliasTable != null && this.globalNamespaceAliasTable.Contains(StylesheetURI);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x000480FC File Offset: 0x000470FC
		internal NamespaceInfo FindNamespaceAlias(string StylesheetURI)
		{
			if (this.globalNamespaceAliasTable != null)
			{
				return (NamespaceInfo)this.globalNamespaceAliasTable[StylesheetURI];
			}
			return null;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00048119 File Offset: 0x00047119
		internal string ResolveXmlNamespace(string prefix)
		{
			return this.scopeManager.ResolveXmlNamespace(prefix);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00048127 File Offset: 0x00047127
		internal string ResolveXPathNamespace(string prefix)
		{
			return this.scopeManager.ResolveXPathNamespace(prefix);
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x00048135 File Offset: 0x00047135
		internal string DefaultNamespace
		{
			get
			{
				return this.scopeManager.DefaultNamespace;
			}
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00048142 File Offset: 0x00047142
		internal void InsertKey(XmlQualifiedName name, int MatchKey, int UseKey)
		{
			this.rootAction.InsertKey(name, MatchKey, UseKey);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00048152 File Offset: 0x00047152
		internal void AddDecimalFormat(XmlQualifiedName name, DecimalFormat formatinfo)
		{
			this.rootAction.AddDecimalFormat(name, formatinfo);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00048164 File Offset: 0x00047164
		private string[] ResolvePrefixes(string tokens)
		{
			if (tokens == null || tokens.Length == 0)
			{
				return null;
			}
			string[] array = XmlConvert.SplitString(tokens);
			try
			{
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					array[i] = this.scopeManager.ResolveXmlNamespace((text == "#default") ? string.Empty : text);
				}
			}
			catch (XsltException)
			{
				if (!this.ForwardCompatibility)
				{
					throw;
				}
				return null;
			}
			return array;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x000481E0 File Offset: 0x000471E0
		internal bool GetYesNo(string value)
		{
			if (value.Equals(this.Atoms.Yes))
			{
				return true;
			}
			if (value.Equals(this.Atoms.No))
			{
				return false;
			}
			throw XsltException.Create("Xslt_InvalidAttrValue", new string[]
			{
				this.Input.LocalName,
				value
			});
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0004823C File Offset: 0x0004723C
		internal string GetSingleAttribute(string attributeAtom)
		{
			NavigatorInput navigatorInput = this.Input;
			string localName = navigatorInput.LocalName;
			string text = null;
			if (navigatorInput.MoveToFirstAttribute())
			{
				string localName2;
				for (;;)
				{
					string namespaceURI = navigatorInput.NamespaceURI;
					localName2 = navigatorInput.LocalName;
					if (Keywords.Equals(namespaceURI, this.Atoms.Empty))
					{
						if (Keywords.Equals(localName2, attributeAtom))
						{
							text = navigatorInput.Value;
						}
						else if (!this.ForwardCompatibility)
						{
							break;
						}
					}
					if (!navigatorInput.MoveToNextAttribute())
					{
						goto Block_4;
					}
				}
				throw XsltException.Create("Xslt_InvalidAttribute", new string[] { localName2, localName });
				Block_4:
				navigatorInput.ToParent();
			}
			if (text == null)
			{
				throw XsltException.Create("Xslt_MissingAttribute", new string[] { attributeAtom });
			}
			return text;
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x000482EC File Offset: 0x000472EC
		internal XmlQualifiedName CreateXPathQName(string qname)
		{
			string text;
			string text2;
			PrefixQName.ParseQualifiedName(qname, out text, out text2);
			return new XmlQualifiedName(text2, this.scopeManager.ResolveXPathNamespace(text));
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00048318 File Offset: 0x00047318
		internal XmlQualifiedName CreateXmlQName(string qname)
		{
			string text;
			string text2;
			PrefixQName.ParseQualifiedName(qname, out text, out text2);
			return new XmlQualifiedName(text2, this.scopeManager.ResolveXmlNamespace(text));
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00048344 File Offset: 0x00047344
		internal static XPathDocument LoadDocument(XmlTextReaderImpl reader)
		{
			reader.EntityHandling = EntityHandling.ExpandEntities;
			reader.XmlValidatingReaderCompatibilityMode = true;
			XPathDocument xpathDocument;
			try
			{
				xpathDocument = new XPathDocument(reader, XmlSpace.Preserve);
			}
			finally
			{
				reader.Close();
			}
			return xpathDocument;
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00048384 File Offset: 0x00047384
		private void AddDocumentURI(string href)
		{
			this.documentURIs.Add(href, null);
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00048393 File Offset: 0x00047393
		private void RemoveDocumentURI(string href)
		{
			this.documentURIs.Remove(href);
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x000483A1 File Offset: 0x000473A1
		internal bool IsCircularReference(string href)
		{
			return this.documentURIs.Contains(href);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x000483B0 File Offset: 0x000473B0
		internal Uri ResolveUri(string relativeUri)
		{
			string baseURI = this.Input.BaseURI;
			Uri uri = this.xmlResolver.ResolveUri((baseURI.Length != 0) ? this.xmlResolver.ResolveUri(null, baseURI) : null, relativeUri);
			if (uri == null)
			{
				throw XsltException.Create("Xslt_CantResolve", new string[] { relativeUri });
			}
			return uri;
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00048410 File Offset: 0x00047410
		internal NavigatorInput ResolveDocument(Uri absoluteUri)
		{
			object entity = this.xmlResolver.GetEntity(absoluteUri, null, null);
			string text = absoluteUri.ToString();
			if (entity is Stream)
			{
				return new NavigatorInput(Compiler.LoadDocument(new XmlTextReaderImpl(text, (Stream)entity)
				{
					XmlResolver = this.xmlResolver
				}).CreateNavigator(), text, this.rootScope);
			}
			if (entity is XPathNavigator)
			{
				return new NavigatorInput((XPathNavigator)entity, text, this.rootScope);
			}
			throw XsltException.Create("Xslt_CantResolve", new string[] { text });
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x000484A0 File Offset: 0x000474A0
		internal void PushInputDocument(NavigatorInput newInput)
		{
			string href = newInput.Href;
			this.AddDocumentURI(href);
			newInput.Next = this.input;
			this.input = newInput;
			this.atoms = this.input.Atoms;
			this.scopeManager = this.input.InputScopeManager;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000484F0 File Offset: 0x000474F0
		internal void PopInputDocument()
		{
			NavigatorInput navigatorInput = this.input;
			this.input = navigatorInput.Next;
			navigatorInput.Next = null;
			if (this.input != null)
			{
				this.atoms = this.input.Atoms;
				this.scopeManager = this.input.InputScopeManager;
			}
			else
			{
				this.atoms = null;
				this.scopeManager = null;
			}
			this.RemoveDocumentURI(navigatorInput.Href);
			navigatorInput.Close();
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00048563 File Offset: 0x00047563
		internal void PushStylesheet(Stylesheet stylesheet)
		{
			if (this.stylesheets == null)
			{
				this.stylesheets = new Stack();
			}
			this.stylesheets.Push(stylesheet);
			this.stylesheet = stylesheet;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0004858C File Offset: 0x0004758C
		internal Stylesheet PopStylesheet()
		{
			Stylesheet stylesheet = (Stylesheet)this.stylesheets.Pop();
			this.stylesheet = (Stylesheet)this.stylesheets.Peek();
			return stylesheet;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x000485C1 File Offset: 0x000475C1
		internal void AddAttributeSet(AttributeSetAction attributeSet)
		{
			this.stylesheet.AddAttributeSet(attributeSet);
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x000485CF File Offset: 0x000475CF
		internal void AddTemplate(TemplateAction template)
		{
			this.stylesheet.AddTemplate(template);
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x000485DD File Offset: 0x000475DD
		internal void BeginTemplate(TemplateAction template)
		{
			this.currentTemplate = template;
			this.currentMode = template.Mode;
			this.CanHaveApplyImports = template.MatchKey != -1;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00048604 File Offset: 0x00047604
		internal void EndTemplate()
		{
			this.currentTemplate = this.rootAction;
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x00048612 File Offset: 0x00047612
		internal XmlQualifiedName CurrentMode
		{
			get
			{
				return this.currentMode;
			}
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0004861A File Offset: 0x0004761A
		internal int AddQuery(string xpathQuery)
		{
			return this.AddQuery(xpathQuery, true, true, false);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00048628 File Offset: 0x00047628
		internal int AddQuery(string xpathQuery, bool allowVar, bool allowKey, bool isPattern)
		{
			CompiledXpathExpr compiledXpathExpr;
			try
			{
				compiledXpathExpr = new CompiledXpathExpr(isPattern ? this.queryBuilder.BuildPatternQuery(xpathQuery, allowVar, allowKey) : this.queryBuilder.Build(xpathQuery, allowVar, allowKey), xpathQuery, false);
			}
			catch (XPathException ex)
			{
				if (!this.ForwardCompatibility)
				{
					throw XsltException.Create("Xslt_InvalidXPath", new string[] { xpathQuery }, ex);
				}
				compiledXpathExpr = new Compiler.ErrorXPathExpression(xpathQuery, this.Input.BaseURI, this.Input.LineNumber, this.Input.LinePosition);
			}
			this.queryStore.Add(new TheQuery(compiledXpathExpr, this.scopeManager));
			return this.queryStore.Count - 1;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x000486E0 File Offset: 0x000476E0
		internal int AddStringQuery(string xpathQuery)
		{
			string text = (XmlCharType.Instance.IsOnlyWhitespace(xpathQuery) ? xpathQuery : ("string(" + xpathQuery + ")"));
			return this.AddQuery(text);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00048718 File Offset: 0x00047718
		internal int AddBooleanQuery(string xpathQuery)
		{
			string text = (XmlCharType.Instance.IsOnlyWhitespace(xpathQuery) ? xpathQuery : ("boolean(" + xpathQuery + ")"));
			return this.AddQuery(text);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00048750 File Offset: 0x00047750
		private static string GenerateUniqueClassName()
		{
			return "ScriptClass_" + ++Compiler.scriptClassCounter;
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00048770 File Offset: 0x00047770
		internal void AddScript(string source, ScriptingLanguage lang, string ns, string fileName, int lineNumber)
		{
			Compiler.ValidateExtensionNamespace(ns);
			for (ScriptingLanguage scriptingLanguage = ScriptingLanguage.JScript; scriptingLanguage <= ScriptingLanguage.CSharp; scriptingLanguage++)
			{
				Hashtable hashtable = this._typeDeclsByLang[(int)scriptingLanguage];
				if (lang == scriptingLanguage)
				{
					CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)hashtable[ns];
					if (codeTypeDeclaration == null)
					{
						codeTypeDeclaration = new CodeTypeDeclaration(Compiler.GenerateUniqueClassName());
						codeTypeDeclaration.TypeAttributes = TypeAttributes.Public;
						hashtable.Add(ns, codeTypeDeclaration);
					}
					CodeSnippetTypeMember codeSnippetTypeMember = new CodeSnippetTypeMember(source);
					if (lineNumber > 0)
					{
						codeSnippetTypeMember.LinePragma = new CodeLinePragma(fileName, lineNumber);
						this.scriptFiles.Add(fileName);
					}
					codeTypeDeclaration.Members.Add(codeSnippetTypeMember);
				}
				else if (hashtable.Contains(ns))
				{
					throw XsltException.Create("Xslt_ScriptMixedLanguages", new string[] { ns });
				}
			}
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00048827 File Offset: 0x00047827
		private static void ValidateExtensionNamespace(string nsUri)
		{
			if (nsUri.Length == 0 || nsUri == "http://www.w3.org/1999/XSL/Transform")
			{
				throw XsltException.Create("Xslt_InvalidExtensionNamespace", new string[0]);
			}
			XmlConvert.ToUri(nsUri);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00048858 File Offset: 0x00047858
		private void FixCompilerError(CompilerError e)
		{
			foreach (object obj in this.scriptFiles)
			{
				string text = (string)obj;
				if (e.FileName == text)
				{
					return;
				}
			}
			e.FileName = string.Empty;
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x000488C8 File Offset: 0x000478C8
		private CodeDomProvider ChooseCodeDomProvider(ScriptingLanguage lang)
		{
			if (lang == ScriptingLanguage.JScript)
			{
				return (CodeDomProvider)Activator.CreateInstance(Type.GetType("Microsoft.JScript.JScriptCodeProvider, Microsoft.JScript, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
			}
			if (lang != ScriptingLanguage.VisualBasic)
			{
				return new CSharpCodeProvider();
			}
			return new VBCodeProvider();
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x000488FC File Offset: 0x000478FC
		private void CompileScript(Evidence evidence)
		{
			for (ScriptingLanguage scriptingLanguage = ScriptingLanguage.JScript; scriptingLanguage <= ScriptingLanguage.CSharp; scriptingLanguage++)
			{
				int num = (int)scriptingLanguage;
				if (this._typeDeclsByLang[num].Count > 0)
				{
					this.CompileAssembly(scriptingLanguage, this._typeDeclsByLang[num], scriptingLanguage.ToString(), evidence);
				}
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00048944 File Offset: 0x00047944
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void CompileAssembly(ScriptingLanguage lang, Hashtable typeDecls, string nsName, Evidence evidence)
		{
			nsName = "Microsoft.Xslt.CompiledScripts." + nsName;
			CodeNamespace codeNamespace = new CodeNamespace(nsName);
			foreach (string text in Compiler._defaultNamespaces)
			{
				codeNamespace.Imports.Add(new CodeNamespaceImport(text));
			}
			if (lang == ScriptingLanguage.VisualBasic)
			{
				codeNamespace.Imports.Add(new CodeNamespaceImport("Microsoft.VisualBasic"));
			}
			foreach (object obj in typeDecls.Values)
			{
				CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)obj;
				codeNamespace.Types.Add(codeTypeDeclaration);
			}
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
			codeCompileUnit.Namespaces.Add(codeNamespace);
			codeCompileUnit.UserData["AllowLateBound"] = true;
			codeCompileUnit.UserData["RequireVariableDeclaration"] = false;
			CompilerParameters compilerParameters = new CompilerParameters();
			try
			{
				new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Assert();
				try
				{
					compilerParameters.GenerateInMemory = true;
					compilerParameters.Evidence = evidence;
					compilerParameters.ReferencedAssemblies.Add(typeof(XPathNavigator).Module.FullyQualifiedName);
					compilerParameters.ReferencedAssemblies.Add("system.dll");
					if (lang == ScriptingLanguage.VisualBasic)
					{
						compilerParameters.ReferencedAssemblies.Add("microsoft.visualbasic.dll");
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			catch
			{
				throw;
			}
			CompilerResults compilerResults = this.ChooseCodeDomProvider(lang).CompileAssemblyFromDom(compilerParameters, new CodeCompileUnit[] { codeCompileUnit });
			if (compilerResults.Errors.HasErrors)
			{
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				foreach (object obj2 in compilerResults.Errors)
				{
					CompilerError compilerError = (CompilerError)obj2;
					this.FixCompilerError(compilerError);
					stringWriter.WriteLine(compilerError.ToString());
				}
				throw XsltException.Create("Xslt_ScriptCompileErrors", new string[] { stringWriter.ToString() });
			}
			Assembly compiledAssembly = compilerResults.CompiledAssembly;
			foreach (object obj3 in typeDecls)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
				string text2 = (string)dictionaryEntry.Key;
				CodeTypeDeclaration codeTypeDeclaration2 = (CodeTypeDeclaration)dictionaryEntry.Value;
				this.stylesheet.ScriptObjectTypes.Add(text2, compiledAssembly.GetType(nsName + "." + codeTypeDeclaration2.Name));
			}
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00048C2C File Offset: 0x00047C2C
		public string GetNsAlias(ref string prefix)
		{
			if (Keywords.Compare(this.input.Atoms.HashDefault, prefix))
			{
				prefix = string.Empty;
				return this.DefaultNamespace;
			}
			if (!PrefixQName.ValidatePrefix(prefix))
			{
				throw XsltException.Create("Xslt_InvalidAttrValue", new string[]
				{
					this.input.LocalName,
					prefix
				});
			}
			return this.ResolveXPathNamespace(prefix);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00048C98 File Offset: 0x00047C98
		private static void getTextLex(string avt, ref int start, StringBuilder lex)
		{
			int length = avt.Length;
			int i;
			for (i = start; i < length; i++)
			{
				char c = avt[i];
				if (c == '{')
				{
					if (i + 1 >= length || avt[i + 1] != '{')
					{
						break;
					}
					i++;
				}
				else if (c == '}')
				{
					if (i + 1 >= length || avt[i + 1] != '}')
					{
						throw XsltException.Create("Xslt_SingleRightAvt", new string[] { avt });
					}
					i++;
				}
				lex.Append(c);
			}
			start = i;
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00048D20 File Offset: 0x00047D20
		private static void getXPathLex(string avt, ref int start, StringBuilder lex)
		{
			int length = avt.Length;
			int num = 0;
			for (int i = start + 1; i < length; i++)
			{
				char c = avt[i];
				switch (num)
				{
				case 0:
				{
					char c2 = c;
					if (c2 != '"')
					{
						if (c2 != '\'')
						{
							switch (c2)
							{
							case '{':
								throw XsltException.Create("Xslt_NestedAvt", new string[] { avt });
							case '}':
								i++;
								if (i == start + 2)
								{
									throw XsltException.Create("Xslt_EmptyAvtExpr", new string[] { avt });
								}
								lex.Append(avt, start + 1, i - start - 2);
								start = i;
								return;
							}
						}
						else
						{
							num = 1;
						}
					}
					else
					{
						num = 2;
					}
					break;
				}
				case 1:
					if (c == '\'')
					{
						num = 0;
					}
					break;
				case 2:
					if (c == '"')
					{
						num = 0;
					}
					break;
				}
			}
			throw XsltException.Create((num == 0) ? "Xslt_OpenBracesAvt" : "Xslt_OpenLiteralAvt", new string[] { avt });
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00048E25 File Offset: 0x00047E25
		private static bool GetNextAvtLex(string avt, ref int start, StringBuilder lex, out bool isAvt)
		{
			isAvt = false;
			if (start == avt.Length)
			{
				return false;
			}
			lex.Length = 0;
			Compiler.getTextLex(avt, ref start, lex);
			if (lex.Length == 0)
			{
				isAvt = true;
				Compiler.getXPathLex(avt, ref start, lex);
			}
			return true;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00048E5C File Offset: 0x00047E5C
		internal ArrayList CompileAvt(string avtText, out bool constant)
		{
			ArrayList arrayList = new ArrayList();
			constant = true;
			int num = 0;
			bool flag;
			while (Compiler.GetNextAvtLex(avtText, ref num, this.AvtStringBuilder, out flag))
			{
				string text = this.AvtStringBuilder.ToString();
				if (flag)
				{
					arrayList.Add(new AvtEvent(this.AddStringQuery(text)));
					constant = false;
				}
				else
				{
					arrayList.Add(new TextEvent(text));
				}
			}
			return arrayList;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00048EC0 File Offset: 0x00047EC0
		internal ArrayList CompileAvt(string avtText)
		{
			bool flag;
			return this.CompileAvt(avtText, out flag);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00048ED8 File Offset: 0x00047ED8
		public virtual ApplyImportsAction CreateApplyImportsAction()
		{
			ApplyImportsAction applyImportsAction = new ApplyImportsAction();
			applyImportsAction.Compile(this);
			return applyImportsAction;
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00048EF4 File Offset: 0x00047EF4
		public virtual ApplyTemplatesAction CreateApplyTemplatesAction()
		{
			ApplyTemplatesAction applyTemplatesAction = new ApplyTemplatesAction();
			applyTemplatesAction.Compile(this);
			return applyTemplatesAction;
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00048F10 File Offset: 0x00047F10
		public virtual AttributeAction CreateAttributeAction()
		{
			AttributeAction attributeAction = new AttributeAction();
			attributeAction.Compile(this);
			return attributeAction;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00048F2C File Offset: 0x00047F2C
		public virtual AttributeSetAction CreateAttributeSetAction()
		{
			AttributeSetAction attributeSetAction = new AttributeSetAction();
			attributeSetAction.Compile(this);
			return attributeSetAction;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00048F48 File Offset: 0x00047F48
		public virtual CallTemplateAction CreateCallTemplateAction()
		{
			CallTemplateAction callTemplateAction = new CallTemplateAction();
			callTemplateAction.Compile(this);
			return callTemplateAction;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00048F64 File Offset: 0x00047F64
		public virtual ChooseAction CreateChooseAction()
		{
			ChooseAction chooseAction = new ChooseAction();
			chooseAction.Compile(this);
			return chooseAction;
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00048F80 File Offset: 0x00047F80
		public virtual CommentAction CreateCommentAction()
		{
			CommentAction commentAction = new CommentAction();
			commentAction.Compile(this);
			return commentAction;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00048F9C File Offset: 0x00047F9C
		public virtual CopyAction CreateCopyAction()
		{
			CopyAction copyAction = new CopyAction();
			copyAction.Compile(this);
			return copyAction;
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00048FB8 File Offset: 0x00047FB8
		public virtual CopyOfAction CreateCopyOfAction()
		{
			CopyOfAction copyOfAction = new CopyOfAction();
			copyOfAction.Compile(this);
			return copyOfAction;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00048FD4 File Offset: 0x00047FD4
		public virtual ElementAction CreateElementAction()
		{
			ElementAction elementAction = new ElementAction();
			elementAction.Compile(this);
			return elementAction;
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00048FF0 File Offset: 0x00047FF0
		public virtual ForEachAction CreateForEachAction()
		{
			ForEachAction forEachAction = new ForEachAction();
			forEachAction.Compile(this);
			return forEachAction;
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0004900C File Offset: 0x0004800C
		public virtual IfAction CreateIfAction(IfAction.ConditionType type)
		{
			IfAction ifAction = new IfAction(type);
			ifAction.Compile(this);
			return ifAction;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00049028 File Offset: 0x00048028
		public virtual MessageAction CreateMessageAction()
		{
			MessageAction messageAction = new MessageAction();
			messageAction.Compile(this);
			return messageAction;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00049044 File Offset: 0x00048044
		public virtual NewInstructionAction CreateNewInstructionAction()
		{
			NewInstructionAction newInstructionAction = new NewInstructionAction();
			newInstructionAction.Compile(this);
			return newInstructionAction;
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00049060 File Offset: 0x00048060
		public virtual NumberAction CreateNumberAction()
		{
			NumberAction numberAction = new NumberAction();
			numberAction.Compile(this);
			return numberAction;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0004907C File Offset: 0x0004807C
		public virtual ProcessingInstructionAction CreateProcessingInstructionAction()
		{
			ProcessingInstructionAction processingInstructionAction = new ProcessingInstructionAction();
			processingInstructionAction.Compile(this);
			return processingInstructionAction;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00049097 File Offset: 0x00048097
		public virtual void CreateRootAction()
		{
			this.RootAction = new RootAction();
			this.RootAction.Compile(this);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x000490B0 File Offset: 0x000480B0
		public virtual SortAction CreateSortAction()
		{
			SortAction sortAction = new SortAction();
			sortAction.Compile(this);
			return sortAction;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x000490CC File Offset: 0x000480CC
		public virtual TemplateAction CreateTemplateAction()
		{
			TemplateAction templateAction = new TemplateAction();
			templateAction.Compile(this);
			return templateAction;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x000490E8 File Offset: 0x000480E8
		public virtual TemplateAction CreateSingleTemplateAction()
		{
			TemplateAction templateAction = new TemplateAction();
			templateAction.CompileSingle(this);
			return templateAction;
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00049104 File Offset: 0x00048104
		public virtual TextAction CreateTextAction()
		{
			TextAction textAction = new TextAction();
			textAction.Compile(this);
			return textAction;
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00049120 File Offset: 0x00048120
		public virtual UseAttributeSetsAction CreateUseAttributeSetsAction()
		{
			UseAttributeSetsAction useAttributeSetsAction = new UseAttributeSetsAction();
			useAttributeSetsAction.Compile(this);
			return useAttributeSetsAction;
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0004913C File Offset: 0x0004813C
		public virtual ValueOfAction CreateValueOfAction()
		{
			ValueOfAction valueOfAction = new ValueOfAction();
			valueOfAction.Compile(this);
			return valueOfAction;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00049158 File Offset: 0x00048158
		public virtual VariableAction CreateVariableAction(VariableType type)
		{
			VariableAction variableAction = new VariableAction(type);
			variableAction.Compile(this);
			if (variableAction.VarKey != -1)
			{
				return variableAction;
			}
			return null;
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00049180 File Offset: 0x00048180
		public virtual WithParamAction CreateWithParamAction()
		{
			WithParamAction withParamAction = new WithParamAction();
			withParamAction.Compile(this);
			return withParamAction;
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0004919B File Offset: 0x0004819B
		public virtual BeginEvent CreateBeginEvent()
		{
			return new BeginEvent(this);
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x000491A3 File Offset: 0x000481A3
		public virtual TextEvent CreateTextEvent()
		{
			return new TextEvent(this);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x000491AC File Offset: 0x000481AC
		public XsltException UnexpectedKeyword()
		{
			XPathNavigator xpathNavigator = this.Input.Navigator.Clone();
			string name = xpathNavigator.Name;
			xpathNavigator.MoveToParent();
			string name2 = xpathNavigator.Name;
			return XsltException.Create("Xslt_UnexpectedKeyword", new string[] { name, name2 });
		}

		// Token: 0x04000924 RID: 2340
		internal const int InvalidQueryKey = -1;

		// Token: 0x04000925 RID: 2341
		internal const double RootPriority = 0.5;

		// Token: 0x04000926 RID: 2342
		internal StringBuilder AvtStringBuilder = new StringBuilder();

		// Token: 0x04000927 RID: 2343
		private int stylesheetid;

		// Token: 0x04000928 RID: 2344
		private InputScope rootScope;

		// Token: 0x04000929 RID: 2345
		private XmlResolver xmlResolver;

		// Token: 0x0400092A RID: 2346
		private TemplateBaseAction currentTemplate;

		// Token: 0x0400092B RID: 2347
		private XmlQualifiedName currentMode;

		// Token: 0x0400092C RID: 2348
		private Hashtable globalNamespaceAliasTable;

		// Token: 0x0400092D RID: 2349
		private Stack stylesheets;

		// Token: 0x0400092E RID: 2350
		private HybridDictionary documentURIs = new HybridDictionary();

		// Token: 0x0400092F RID: 2351
		private NavigatorInput input;

		// Token: 0x04000930 RID: 2352
		private Keywords atoms;

		// Token: 0x04000931 RID: 2353
		private InputScopeManager scopeManager;

		// Token: 0x04000932 RID: 2354
		internal Stylesheet stylesheet;

		// Token: 0x04000933 RID: 2355
		internal Stylesheet rootStylesheet;

		// Token: 0x04000934 RID: 2356
		private RootAction rootAction;

		// Token: 0x04000935 RID: 2357
		private List<TheQuery> queryStore;

		// Token: 0x04000936 RID: 2358
		private QueryBuilder queryBuilder = new QueryBuilder();

		// Token: 0x04000937 RID: 2359
		private int rtfCount;

		// Token: 0x04000938 RID: 2360
		public bool AllowBuiltInMode;

		// Token: 0x04000939 RID: 2361
		public static XmlQualifiedName BuiltInMode = new XmlQualifiedName("*", string.Empty);

		// Token: 0x0400093A RID: 2362
		private Hashtable[] _typeDeclsByLang = new Hashtable[]
		{
			new Hashtable(),
			new Hashtable(),
			new Hashtable()
		};

		// Token: 0x0400093B RID: 2363
		private ArrayList scriptFiles = new ArrayList();

		// Token: 0x0400093C RID: 2364
		private static string[] _defaultNamespaces = new string[] { "System", "System.Collections", "System.Text", "System.Text.RegularExpressions", "System.Xml", "System.Xml.Xsl", "System.Xml.XPath" };

		// Token: 0x0400093D RID: 2365
		private static int scriptClassCounter = 0;

		// Token: 0x02000142 RID: 322
		internal class ErrorXPathExpression : CompiledXpathExpr
		{
			// Token: 0x06000E4A RID: 3658 RVA: 0x000492CD File Offset: 0x000482CD
			public ErrorXPathExpression(string expression, string baseUri, int lineNumber, int linePosition)
				: base(null, expression, false)
			{
				this.baseUri = baseUri;
				this.lineNumber = lineNumber;
				this.linePosition = linePosition;
			}

			// Token: 0x06000E4B RID: 3659 RVA: 0x000492EE File Offset: 0x000482EE
			public override XPathExpression Clone()
			{
				return this;
			}

			// Token: 0x06000E4C RID: 3660 RVA: 0x000492F4 File Offset: 0x000482F4
			public override void CheckErrors()
			{
				throw new XsltException("Xslt_InvalidXPath", new string[] { this.Expression }, this.baseUri, this.linePosition, this.lineNumber, null);
			}

			// Token: 0x0400093E RID: 2366
			private string baseUri;

			// Token: 0x0400093F RID: 2367
			private int lineNumber;

			// Token: 0x04000940 RID: 2368
			private int linePosition;
		}
	}
}
