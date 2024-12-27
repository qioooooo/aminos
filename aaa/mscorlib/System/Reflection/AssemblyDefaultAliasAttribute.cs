using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002D0 RID: 720
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyDefaultAliasAttribute : Attribute
	{
		// Token: 0x06001CB6 RID: 7350 RVA: 0x000499A5 File Offset: 0x000489A5
		public AssemblyDefaultAliasAttribute(string defaultAlias)
		{
			this.m_defaultAlias = defaultAlias;
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001CB7 RID: 7351 RVA: 0x000499B4 File Offset: 0x000489B4
		public string DefaultAlias
		{
			get
			{
				return this.m_defaultAlias;
			}
		}

		// Token: 0x04000A81 RID: 2689
		private string m_defaultAlias;
	}
}
