using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005D7 RID: 1495
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class MethodImplAttribute : Attribute
	{
		// Token: 0x0600379D RID: 14237 RVA: 0x000BB8CC File Offset: 0x000BA8CC
		internal MethodImplAttribute(MethodImplAttributes methodImplAttributes)
		{
			MethodImplOptions methodImplOptions = MethodImplOptions.Unmanaged | MethodImplOptions.ForwardRef | MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall | MethodImplOptions.Synchronized | MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization;
			this._val = (MethodImplOptions)(methodImplAttributes & (MethodImplAttributes)methodImplOptions);
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x000BB8EE File Offset: 0x000BA8EE
		public MethodImplAttribute(MethodImplOptions methodImplOptions)
		{
			this._val = methodImplOptions;
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x000BB8FD File Offset: 0x000BA8FD
		public MethodImplAttribute(short value)
		{
			this._val = (MethodImplOptions)value;
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x000BB90C File Offset: 0x000BA90C
		public MethodImplAttribute()
		{
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x060037A1 RID: 14241 RVA: 0x000BB914 File Offset: 0x000BA914
		public MethodImplOptions Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001CAC RID: 7340
		internal MethodImplOptions _val;

		// Token: 0x04001CAD RID: 7341
		public MethodCodeType MethodCodeType;
	}
}
