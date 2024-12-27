using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004D4 RID: 1236
	[ComVisible(true)]
	[Obsolete("This attribute is deprecated and will be removed in a future version.", false)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, Inherited = false)]
	public sealed class IDispatchImplAttribute : Attribute
	{
		// Token: 0x06003104 RID: 12548 RVA: 0x000A8CDF File Offset: 0x000A7CDF
		public IDispatchImplAttribute(IDispatchImplType implType)
		{
			this._val = implType;
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x000A8CEE File Offset: 0x000A7CEE
		public IDispatchImplAttribute(short implType)
		{
			this._val = (IDispatchImplType)implType;
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06003106 RID: 12550 RVA: 0x000A8CFD File Offset: 0x000A7CFD
		public IDispatchImplType Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018B5 RID: 6325
		internal IDispatchImplType _val;
	}
}
