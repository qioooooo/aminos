using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002D4 RID: 724
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyVersionAttribute : Attribute
	{
		// Token: 0x06001CBE RID: 7358 RVA: 0x00049A0F File Offset: 0x00048A0F
		public AssemblyVersionAttribute(string version)
		{
			this.m_version = version;
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001CBF RID: 7359 RVA: 0x00049A1E File Offset: 0x00048A1E
		public string Version
		{
			get
			{
				return this.m_version;
			}
		}

		// Token: 0x04000A85 RID: 2693
		private string m_version;
	}
}
