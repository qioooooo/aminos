using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004CB RID: 1227
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, Inherited = false)]
	public sealed class ClassInterfaceAttribute : Attribute
	{
		// Token: 0x060030F5 RID: 12533 RVA: 0x000A8C31 File Offset: 0x000A7C31
		public ClassInterfaceAttribute(ClassInterfaceType classInterfaceType)
		{
			this._val = classInterfaceType;
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x000A8C40 File Offset: 0x000A7C40
		public ClassInterfaceAttribute(short classInterfaceType)
		{
			this._val = (ClassInterfaceType)classInterfaceType;
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060030F7 RID: 12535 RVA: 0x000A8C4F File Offset: 0x000A7C4F
		public ClassInterfaceType Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018AB RID: 6315
		internal ClassInterfaceType _val;
	}
}
