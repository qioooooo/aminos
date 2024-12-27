using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000062 RID: 98
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class LoaderOptimizationAttribute : Attribute
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x00014D66 File Offset: 0x00013D66
		public LoaderOptimizationAttribute(byte value)
		{
			this._val = value;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00014D75 File Offset: 0x00013D75
		public LoaderOptimizationAttribute(LoaderOptimization value)
		{
			this._val = (byte)value;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x00014D85 File Offset: 0x00013D85
		public LoaderOptimization Value
		{
			get
			{
				return (LoaderOptimization)this._val;
			}
		}

		// Token: 0x040001D5 RID: 469
		internal byte _val;
	}
}
