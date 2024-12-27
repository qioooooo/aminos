using System;
using System.Collections;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200017C RID: 380
	internal class InputScope : DocumentScope
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000F90 RID: 3984 RVA: 0x0004DFA9 File Offset: 0x0004CFA9
		internal InputScope Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0004DFB1 File Offset: 0x0004CFB1
		internal Hashtable Variables
		{
			get
			{
				return this.variables;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x0004DFB9 File Offset: 0x0004CFB9
		// (set) Token: 0x06000F93 RID: 3987 RVA: 0x0004DFC1 File Offset: 0x0004CFC1
		internal bool ForwardCompatibility
		{
			get
			{
				return this.forwardCompatibility;
			}
			set
			{
				this.forwardCompatibility = value;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0004DFCA File Offset: 0x0004CFCA
		// (set) Token: 0x06000F95 RID: 3989 RVA: 0x0004DFD2 File Offset: 0x0004CFD2
		internal bool CanHaveApplyImports
		{
			get
			{
				return this.canHaveApplyImports;
			}
			set
			{
				this.canHaveApplyImports = value;
			}
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0004DFDB File Offset: 0x0004CFDB
		internal InputScope(InputScope parent)
		{
			this.Init(parent);
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0004DFEA File Offset: 0x0004CFEA
		internal void Init(InputScope parent)
		{
			this.scopes = null;
			this.parent = parent;
			if (this.parent != null)
			{
				this.forwardCompatibility = this.parent.forwardCompatibility;
				this.canHaveApplyImports = this.parent.canHaveApplyImports;
			}
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0004E024 File Offset: 0x0004D024
		internal void InsertExtensionNamespace(string nspace)
		{
			if (this.extensionNamespaces == null)
			{
				this.extensionNamespaces = new Hashtable();
			}
			this.extensionNamespaces[nspace] = null;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0004E046 File Offset: 0x0004D046
		internal bool IsExtensionNamespace(string nspace)
		{
			return this.extensionNamespaces != null && this.extensionNamespaces.Contains(nspace);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0004E05E File Offset: 0x0004D05E
		internal void InsertExcludedNamespace(string nspace)
		{
			if (this.excludedNamespaces == null)
			{
				this.excludedNamespaces = new Hashtable();
			}
			this.excludedNamespaces[nspace] = null;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0004E080 File Offset: 0x0004D080
		internal bool IsExcludedNamespace(string nspace)
		{
			return this.excludedNamespaces != null && this.excludedNamespaces.Contains(nspace);
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0004E098 File Offset: 0x0004D098
		internal void InsertVariable(VariableAction variable)
		{
			if (this.variables == null)
			{
				this.variables = new Hashtable();
			}
			this.variables[variable.Name] = variable;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0004E0BF File Offset: 0x0004D0BF
		internal int GetVeriablesCount()
		{
			if (this.variables == null)
			{
				return 0;
			}
			return this.variables.Count;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0004E0D8 File Offset: 0x0004D0D8
		public VariableAction ResolveVariable(XmlQualifiedName qname)
		{
			for (InputScope inputScope = this; inputScope != null; inputScope = inputScope.Parent)
			{
				if (inputScope.Variables != null)
				{
					VariableAction variableAction = (VariableAction)inputScope.Variables[qname];
					if (variableAction != null)
					{
						return variableAction;
					}
				}
			}
			return null;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0004E114 File Offset: 0x0004D114
		public VariableAction ResolveGlobalVariable(XmlQualifiedName qname)
		{
			InputScope inputScope = null;
			for (InputScope inputScope2 = this; inputScope2 != null; inputScope2 = inputScope2.Parent)
			{
				inputScope = inputScope2;
			}
			return inputScope.ResolveVariable(qname);
		}

		// Token: 0x04000A02 RID: 2562
		private InputScope parent;

		// Token: 0x04000A03 RID: 2563
		private bool forwardCompatibility;

		// Token: 0x04000A04 RID: 2564
		private bool canHaveApplyImports;

		// Token: 0x04000A05 RID: 2565
		private Hashtable variables;

		// Token: 0x04000A06 RID: 2566
		private Hashtable extensionNamespaces;

		// Token: 0x04000A07 RID: 2567
		private Hashtable excludedNamespaces;
	}
}
