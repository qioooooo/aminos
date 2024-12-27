using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005DA RID: 1498
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class RequiredAttributeAttribute : Attribute
	{
		// Token: 0x060037A4 RID: 14244 RVA: 0x000BB92C File Offset: 0x000BA92C
		public RequiredAttributeAttribute(Type requiredContract)
		{
			this.requiredContract = requiredContract;
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x060037A5 RID: 14245 RVA: 0x000BB93B File Offset: 0x000BA93B
		public Type RequiredContract
		{
			get
			{
				return this.requiredContract;
			}
		}

		// Token: 0x04001CAE RID: 7342
		private Type requiredContract;
	}
}
