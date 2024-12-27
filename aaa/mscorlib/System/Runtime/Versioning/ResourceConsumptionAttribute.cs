using System;
using System.Diagnostics;

namespace System.Runtime.Versioning
{
	// Token: 0x02000934 RID: 2356
	[Conditional("RESOURCE_ANNOTATION_WORK")]
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
	public sealed class ResourceConsumptionAttribute : Attribute
	{
		// Token: 0x0600557E RID: 21886 RVA: 0x0013723C File Offset: 0x0013623C
		public ResourceConsumptionAttribute(ResourceScope resourceScope)
		{
			this._resourceScope = resourceScope;
			this._consumptionScope = this._resourceScope;
		}

		// Token: 0x0600557F RID: 21887 RVA: 0x00137257 File Offset: 0x00136257
		public ResourceConsumptionAttribute(ResourceScope resourceScope, ResourceScope consumptionScope)
		{
			this._resourceScope = resourceScope;
			this._consumptionScope = consumptionScope;
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06005580 RID: 21888 RVA: 0x0013726D File Offset: 0x0013626D
		public ResourceScope ResourceScope
		{
			get
			{
				return this._resourceScope;
			}
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06005581 RID: 21889 RVA: 0x00137275 File Offset: 0x00136275
		public ResourceScope ConsumptionScope
		{
			get
			{
				return this._consumptionScope;
			}
		}

		// Token: 0x04002C8A RID: 11402
		private ResourceScope _consumptionScope;

		// Token: 0x04002C8B RID: 11403
		private ResourceScope _resourceScope;
	}
}
