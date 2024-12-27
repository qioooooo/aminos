using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004C9 RID: 1225
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComDefaultInterfaceAttribute : Attribute
	{
		// Token: 0x060030F3 RID: 12531 RVA: 0x000A8C1A File Offset: 0x000A7C1A
		public ComDefaultInterfaceAttribute(Type defaultInterface)
		{
			this._val = defaultInterface;
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x060030F4 RID: 12532 RVA: 0x000A8C29 File Offset: 0x000A7C29
		public Type Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018A6 RID: 6310
		internal Type _val;
	}
}
