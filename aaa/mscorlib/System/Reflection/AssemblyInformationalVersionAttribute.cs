using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002D1 RID: 721
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyInformationalVersionAttribute : Attribute
	{
		// Token: 0x06001CB8 RID: 7352 RVA: 0x000499BC File Offset: 0x000489BC
		public AssemblyInformationalVersionAttribute(string informationalVersion)
		{
			this.m_informationalVersion = informationalVersion;
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001CB9 RID: 7353 RVA: 0x000499CB File Offset: 0x000489CB
		public string InformationalVersion
		{
			get
			{
				return this.m_informationalVersion;
			}
		}

		// Token: 0x04000A82 RID: 2690
		private string m_informationalVersion;
	}
}
