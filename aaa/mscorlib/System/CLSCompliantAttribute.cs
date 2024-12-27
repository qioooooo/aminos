using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000085 RID: 133
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	[Serializable]
	public sealed class CLSCompliantAttribute : Attribute
	{
		// Token: 0x06000766 RID: 1894 RVA: 0x00018261 File Offset: 0x00017261
		public CLSCompliantAttribute(bool isCompliant)
		{
			this.m_compliant = isCompliant;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x00018270 File Offset: 0x00017270
		public bool IsCompliant
		{
			get
			{
				return this.m_compliant;
			}
		}

		// Token: 0x04000287 RID: 647
		private bool m_compliant;
	}
}
