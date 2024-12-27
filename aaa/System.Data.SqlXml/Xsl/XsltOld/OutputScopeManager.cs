using System;
using System.Globalization;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000186 RID: 390
	internal class OutputScopeManager
	{
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0004F7CA File Offset: 0x0004E7CA
		internal string DefaultNamespace
		{
			get
			{
				return this.defaultNS;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x0004F7D2 File Offset: 0x0004E7D2
		internal OutputScope CurrentElementScope
		{
			get
			{
				return (OutputScope)this.elementScopesStack.Peek();
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x0004F7E4 File Offset: 0x0004E7E4
		internal XmlSpace XmlSpace
		{
			get
			{
				return this.CurrentElementScope.Space;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x0004F7F1 File Offset: 0x0004E7F1
		internal string XmlLang
		{
			get
			{
				return this.CurrentElementScope.Lang;
			}
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x0004F800 File Offset: 0x0004E800
		internal OutputScopeManager(XmlNameTable nameTable, OutKeywords atoms)
		{
			this.elementScopesStack = new HWStack(10);
			this.nameTable = nameTable;
			this.atoms = atoms;
			this.defaultNS = this.atoms.Empty;
			OutputScope outputScope = (OutputScope)this.elementScopesStack.Push();
			if (outputScope == null)
			{
				outputScope = new OutputScope();
				this.elementScopesStack.AddToTop(outputScope);
			}
			outputScope.Init(string.Empty, string.Empty, string.Empty, XmlSpace.None, string.Empty, false);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0004F881 File Offset: 0x0004E881
		internal void PushNamespace(string prefix, string nspace)
		{
			this.CurrentElementScope.AddNamespace(prefix, nspace, this.defaultNS);
			if (prefix == null || prefix.Length == 0)
			{
				this.defaultNS = nspace;
			}
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0004F8AC File Offset: 0x0004E8AC
		internal void PushScope(string name, string nspace, string prefix)
		{
			OutputScope currentElementScope = this.CurrentElementScope;
			OutputScope outputScope = (OutputScope)this.elementScopesStack.Push();
			if (outputScope == null)
			{
				outputScope = new OutputScope();
				this.elementScopesStack.AddToTop(outputScope);
			}
			outputScope.Init(name, nspace, prefix, currentElementScope.Space, currentElementScope.Lang, currentElementScope.Mixed);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0004F904 File Offset: 0x0004E904
		internal void PopScope()
		{
			OutputScope outputScope = (OutputScope)this.elementScopesStack.Pop();
			for (NamespaceDecl namespaceDecl = outputScope.Scopes; namespaceDecl != null; namespaceDecl = namespaceDecl.Next)
			{
				this.defaultNS = namespaceDecl.PrevDefaultNsUri;
			}
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0004F944 File Offset: 0x0004E944
		internal string ResolveNamespace(string prefix)
		{
			bool flag;
			return this.ResolveNamespace(prefix, out flag);
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0004F95C File Offset: 0x0004E95C
		internal string ResolveNamespace(string prefix, out bool thisScope)
		{
			thisScope = true;
			if (prefix == null || prefix.Length == 0)
			{
				return this.defaultNS;
			}
			if (Keywords.Equals(prefix, this.atoms.Xml))
			{
				return this.atoms.XmlNamespace;
			}
			if (Keywords.Equals(prefix, this.atoms.Xmlns))
			{
				return this.atoms.XmlnsNamespace;
			}
			for (int i = this.elementScopesStack.Length - 1; i >= 0; i--)
			{
				OutputScope outputScope = (OutputScope)this.elementScopesStack[i];
				string text = outputScope.ResolveAtom(prefix);
				if (text != null)
				{
					thisScope = i == this.elementScopesStack.Length - 1;
					return text;
				}
			}
			return null;
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0004FA08 File Offset: 0x0004EA08
		internal bool FindPrefix(string nspace, out string prefix)
		{
			int num = this.elementScopesStack.Length - 1;
			while (0 <= num)
			{
				OutputScope outputScope = (OutputScope)this.elementScopesStack[num];
				string text = null;
				if (outputScope.FindPrefix(nspace, out text))
				{
					string text2 = this.ResolveNamespace(text);
					if (text2 != null && Keywords.Equals(text2, nspace))
					{
						prefix = text;
						return true;
					}
					break;
				}
				else
				{
					num--;
				}
			}
			prefix = null;
			return false;
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0004FA6C File Offset: 0x0004EA6C
		internal string GeneratePrefix(string format)
		{
			string text;
			do
			{
				text = string.Format(CultureInfo.InvariantCulture, format, new object[] { this.prefixIndex++ });
			}
			while (this.nameTable.Get(text) != null);
			return this.nameTable.Add(text);
		}

		// Token: 0x04000AF6 RID: 2806
		private const int STACK_INCREMENT = 10;

		// Token: 0x04000AF7 RID: 2807
		private HWStack elementScopesStack;

		// Token: 0x04000AF8 RID: 2808
		private string defaultNS;

		// Token: 0x04000AF9 RID: 2809
		private OutKeywords atoms;

		// Token: 0x04000AFA RID: 2810
		private XmlNameTable nameTable;

		// Token: 0x04000AFB RID: 2811
		private int prefixIndex;
	}
}
