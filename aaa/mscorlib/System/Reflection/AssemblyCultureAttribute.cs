using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002D3 RID: 723
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyCultureAttribute : Attribute
	{
		// Token: 0x06001CBC RID: 7356 RVA: 0x000499F8 File Offset: 0x000489F8
		public AssemblyCultureAttribute(string culture)
		{
			this.m_culture = culture;
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x00049A07 File Offset: 0x00048A07
		public string Culture
		{
			get
			{
				return this.m_culture;
			}
		}

		// Token: 0x04000A84 RID: 2692
		private string m_culture;
	}
}
