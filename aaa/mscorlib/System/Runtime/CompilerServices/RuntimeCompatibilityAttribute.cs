using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005F4 RID: 1524
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
	[Serializable]
	public sealed class RuntimeCompatibilityAttribute : Attribute
	{
		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060037B7 RID: 14263 RVA: 0x000BB9F4 File Offset: 0x000BA9F4
		// (set) Token: 0x060037B8 RID: 14264 RVA: 0x000BB9FC File Offset: 0x000BA9FC
		public bool WrapNonExceptionThrows
		{
			get
			{
				return this.m_wrapNonExceptionThrows;
			}
			set
			{
				this.m_wrapNonExceptionThrows = value;
			}
		}

		// Token: 0x04001CB8 RID: 7352
		private bool m_wrapNonExceptionThrows;
	}
}
