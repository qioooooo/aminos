using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F1 RID: 1265
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Module, Inherited = false)]
	public sealed class DefaultCharSetAttribute : Attribute
	{
		// Token: 0x06003156 RID: 12630 RVA: 0x000A9541 File Offset: 0x000A8541
		public DefaultCharSetAttribute(CharSet charSet)
		{
			this._CharSet = charSet;
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06003157 RID: 12631 RVA: 0x000A9550 File Offset: 0x000A8550
		public CharSet CharSet
		{
			get
			{
				return this._CharSet;
			}
		}

		// Token: 0x0400195F RID: 6495
		internal CharSet _CharSet;
	}
}
