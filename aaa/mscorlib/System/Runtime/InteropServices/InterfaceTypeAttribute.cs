using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004C8 RID: 1224
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class InterfaceTypeAttribute : Attribute
	{
		// Token: 0x060030F0 RID: 12528 RVA: 0x000A8BF4 File Offset: 0x000A7BF4
		public InterfaceTypeAttribute(ComInterfaceType interfaceType)
		{
			this._val = interfaceType;
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000A8C03 File Offset: 0x000A7C03
		public InterfaceTypeAttribute(short interfaceType)
		{
			this._val = (ComInterfaceType)interfaceType;
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x060030F2 RID: 12530 RVA: 0x000A8C12 File Offset: 0x000A7C12
		public ComInterfaceType Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018A5 RID: 6309
		internal ComInterfaceType _val;
	}
}
