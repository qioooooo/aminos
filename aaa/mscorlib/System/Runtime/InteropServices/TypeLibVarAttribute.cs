using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004DC RID: 1244
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public sealed class TypeLibVarAttribute : Attribute
	{
		// Token: 0x06003114 RID: 12564 RVA: 0x000A8E5F File Offset: 0x000A7E5F
		public TypeLibVarAttribute(TypeLibVarFlags flags)
		{
			this._val = flags;
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x000A8E6E File Offset: 0x000A7E6E
		public TypeLibVarAttribute(short flags)
		{
			this._val = (TypeLibVarFlags)flags;
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06003116 RID: 12566 RVA: 0x000A8E7D File Offset: 0x000A7E7D
		public TypeLibVarFlags Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018E4 RID: 6372
		internal TypeLibVarFlags _val;
	}
}
