using System;

namespace System.Web.UI
{
	// Token: 0x0200038A RID: 906
	internal class TagNamespaceRegisterEntry : RegisterDirectiveEntry
	{
		// Token: 0x06002C41 RID: 11329 RVA: 0x000C5CC1 File Offset: 0x000C4CC1
		internal TagNamespaceRegisterEntry(string tagPrefix, string namespaceName, string assemblyName)
			: base(tagPrefix)
		{
			this._ns = namespaceName;
			this._assemblyName = assemblyName;
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06002C42 RID: 11330 RVA: 0x000C5CD8 File Offset: 0x000C4CD8
		internal string Namespace
		{
			get
			{
				return this._ns;
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06002C43 RID: 11331 RVA: 0x000C5CE0 File Offset: 0x000C4CE0
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x04002086 RID: 8326
		private string _ns;

		// Token: 0x04002087 RID: 8327
		private string _assemblyName;
	}
}
