using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004C4 RID: 1220
	[AttributeUsage(AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	[ComVisible(true)]
	public sealed class UnmanagedFunctionPointerAttribute : Attribute
	{
		// Token: 0x060030EB RID: 12523 RVA: 0x000A8BBE File Offset: 0x000A7BBE
		public UnmanagedFunctionPointerAttribute(CallingConvention callingConvention)
		{
			this.m_callingConvention = callingConvention;
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060030EC RID: 12524 RVA: 0x000A8BCD File Offset: 0x000A7BCD
		public CallingConvention CallingConvention
		{
			get
			{
				return this.m_callingConvention;
			}
		}

		// Token: 0x0400189B RID: 6299
		private CallingConvention m_callingConvention;

		// Token: 0x0400189C RID: 6300
		public CharSet CharSet;

		// Token: 0x0400189D RID: 6301
		public bool BestFitMapping;

		// Token: 0x0400189E RID: 6302
		public bool ThrowOnUnmappableChar;

		// Token: 0x0400189F RID: 6303
		public bool SetLastError;
	}
}
