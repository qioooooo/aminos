using System;

namespace System.Security
{
	// Token: 0x02000654 RID: 1620
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	public sealed class SecurityCriticalAttribute : Attribute
	{
		// Token: 0x06003AD0 RID: 15056 RVA: 0x000C8357 File Offset: 0x000C7357
		public SecurityCriticalAttribute()
		{
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x000C835F File Offset: 0x000C735F
		public SecurityCriticalAttribute(SecurityCriticalScope scope)
		{
			this._val = scope;
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06003AD2 RID: 15058 RVA: 0x000C836E File Offset: 0x000C736E
		public SecurityCriticalScope Scope
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001E46 RID: 7750
		internal SecurityCriticalScope _val;
	}
}
