using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200017D RID: 381
	internal class InputScopeManager
	{
		// Token: 0x06000FA0 RID: 4000 RVA: 0x0004E13A File Offset: 0x0004D13A
		public InputScopeManager(XPathNavigator navigator, InputScope rootScope)
		{
			this.navigator = navigator;
			this.scopeStack = rootScope;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0004E15B File Offset: 0x0004D15B
		internal InputScope CurrentScope
		{
			get
			{
				return this.scopeStack;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0004E163 File Offset: 0x0004D163
		internal InputScope VariableScope
		{
			get
			{
				return this.scopeStack.Parent;
			}
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0004E170 File Offset: 0x0004D170
		internal InputScopeManager Clone()
		{
			return new InputScopeManager(this.navigator, null)
			{
				scopeStack = this.scopeStack,
				defaultNS = this.defaultNS
			};
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x0004E1A3 File Offset: 0x0004D1A3
		public XPathNavigator Navigator
		{
			get
			{
				return this.navigator;
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0004E1AB File Offset: 0x0004D1AB
		internal InputScope PushScope()
		{
			this.scopeStack = new InputScope(this.scopeStack);
			return this.scopeStack;
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0004E1C4 File Offset: 0x0004D1C4
		internal void PopScope()
		{
			if (this.scopeStack == null)
			{
				return;
			}
			for (NamespaceDecl namespaceDecl = this.scopeStack.Scopes; namespaceDecl != null; namespaceDecl = namespaceDecl.Next)
			{
				this.defaultNS = namespaceDecl.PrevDefaultNsUri;
			}
			this.scopeStack = this.scopeStack.Parent;
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0004E20F File Offset: 0x0004D20F
		internal void PushNamespace(string prefix, string nspace)
		{
			this.scopeStack.AddNamespace(prefix, nspace, this.defaultNS);
			if (prefix == null || prefix.Length == 0)
			{
				this.defaultNS = nspace;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x0004E237 File Offset: 0x0004D237
		public string DefaultNamespace
		{
			get
			{
				return this.defaultNS;
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0004E240 File Offset: 0x0004D240
		private string ResolveNonEmptyPrefix(string prefix)
		{
			if (prefix == "xml")
			{
				return "http://www.w3.org/XML/1998/namespace";
			}
			if (prefix == "xmlns")
			{
				return "http://www.w3.org/2000/xmlns/";
			}
			for (InputScope parent = this.scopeStack; parent != null; parent = parent.Parent)
			{
				string text = parent.ResolveNonAtom(prefix);
				if (text != null)
				{
					return text;
				}
			}
			throw XsltException.Create("Xslt_InvalidPrefix", new string[] { prefix });
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0004E2A9 File Offset: 0x0004D2A9
		public string ResolveXmlNamespace(string prefix)
		{
			if (prefix.Length == 0)
			{
				return this.defaultNS;
			}
			return this.ResolveNonEmptyPrefix(prefix);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0004E2C1 File Offset: 0x0004D2C1
		public string ResolveXPathNamespace(string prefix)
		{
			if (prefix.Length == 0)
			{
				return string.Empty;
			}
			return this.ResolveNonEmptyPrefix(prefix);
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0004E2D8 File Offset: 0x0004D2D8
		internal void InsertExtensionNamespaces(string[] nsList)
		{
			for (int i = 0; i < nsList.Length; i++)
			{
				this.scopeStack.InsertExtensionNamespace(nsList[i]);
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0004E304 File Offset: 0x0004D304
		internal bool IsExtensionNamespace(string nspace)
		{
			for (InputScope parent = this.scopeStack; parent != null; parent = parent.Parent)
			{
				if (parent.IsExtensionNamespace(nspace))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0004E330 File Offset: 0x0004D330
		internal void InsertExcludedNamespaces(string[] nsList)
		{
			for (int i = 0; i < nsList.Length; i++)
			{
				this.scopeStack.InsertExcludedNamespace(nsList[i]);
			}
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x0004E35C File Offset: 0x0004D35C
		internal bool IsExcludedNamespace(string nspace)
		{
			for (InputScope parent = this.scopeStack; parent != null; parent = parent.Parent)
			{
				if (parent.IsExcludedNamespace(nspace))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000A08 RID: 2568
		private InputScope scopeStack;

		// Token: 0x04000A09 RID: 2569
		private string defaultNS = string.Empty;

		// Token: 0x04000A0A RID: 2570
		private XPathNavigator navigator;
	}
}
