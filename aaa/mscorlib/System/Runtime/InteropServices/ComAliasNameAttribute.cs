using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E9 RID: 1257
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComAliasNameAttribute : Attribute
	{
		// Token: 0x06003140 RID: 12608 RVA: 0x000A942E File Offset: 0x000A842E
		public ComAliasNameAttribute(string alias)
		{
			this._val = alias;
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06003141 RID: 12609 RVA: 0x000A943D File Offset: 0x000A843D
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001950 RID: 6480
		internal string _val;
	}
}
