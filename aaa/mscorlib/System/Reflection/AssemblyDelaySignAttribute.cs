using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002D6 RID: 726
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyDelaySignAttribute : Attribute
	{
		// Token: 0x06001CC2 RID: 7362 RVA: 0x00049A3D File Offset: 0x00048A3D
		public AssemblyDelaySignAttribute(bool delaySign)
		{
			this.m_delaySign = delaySign;
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x00049A4C File Offset: 0x00048A4C
		public bool DelaySign
		{
			get
			{
				return this.m_delaySign;
			}
		}

		// Token: 0x04000A87 RID: 2695
		private bool m_delaySign;
	}
}
