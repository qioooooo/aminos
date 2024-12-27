using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004DB RID: 1243
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class TypeLibFuncAttribute : Attribute
	{
		// Token: 0x06003111 RID: 12561 RVA: 0x000A8E39 File Offset: 0x000A7E39
		public TypeLibFuncAttribute(TypeLibFuncFlags flags)
		{
			this._val = flags;
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x000A8E48 File Offset: 0x000A7E48
		public TypeLibFuncAttribute(short flags)
		{
			this._val = (TypeLibFuncFlags)flags;
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06003113 RID: 12563 RVA: 0x000A8E57 File Offset: 0x000A7E57
		public TypeLibFuncFlags Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018E3 RID: 6371
		internal TypeLibFuncFlags _val;
	}
}
