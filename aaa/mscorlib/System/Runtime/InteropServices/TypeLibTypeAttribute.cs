using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004DA RID: 1242
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibTypeAttribute : Attribute
	{
		// Token: 0x0600310E RID: 12558 RVA: 0x000A8E13 File Offset: 0x000A7E13
		public TypeLibTypeAttribute(TypeLibTypeFlags flags)
		{
			this._val = flags;
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x000A8E22 File Offset: 0x000A7E22
		public TypeLibTypeAttribute(short flags)
		{
			this._val = (TypeLibTypeFlags)flags;
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06003110 RID: 12560 RVA: 0x000A8E31 File Offset: 0x000A7E31
		public TypeLibTypeFlags Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018E2 RID: 6370
		internal TypeLibTypeFlags _val;
	}
}
