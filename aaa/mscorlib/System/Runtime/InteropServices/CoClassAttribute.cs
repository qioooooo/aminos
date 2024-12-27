using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004EC RID: 1260
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class CoClassAttribute : Attribute
	{
		// Token: 0x06003147 RID: 12615 RVA: 0x000A9482 File Offset: 0x000A8482
		public CoClassAttribute(Type coClass)
		{
			this._CoClass = coClass;
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06003148 RID: 12616 RVA: 0x000A9491 File Offset: 0x000A8491
		public Type CoClass
		{
			get
			{
				return this._CoClass;
			}
		}

		// Token: 0x04001954 RID: 6484
		internal Type _CoClass;
	}
}
