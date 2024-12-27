using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F0 RID: 1264
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
	public sealed class BestFitMappingAttribute : Attribute
	{
		// Token: 0x06003154 RID: 12628 RVA: 0x000A952A File Offset: 0x000A852A
		public BestFitMappingAttribute(bool BestFitMapping)
		{
			this._bestFitMapping = BestFitMapping;
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06003155 RID: 12629 RVA: 0x000A9539 File Offset: 0x000A8539
		public bool BestFitMapping
		{
			get
			{
				return this._bestFitMapping;
			}
		}

		// Token: 0x0400195D RID: 6493
		internal bool _bestFitMapping;

		// Token: 0x0400195E RID: 6494
		public bool ThrowOnUnmappableChar;
	}
}
