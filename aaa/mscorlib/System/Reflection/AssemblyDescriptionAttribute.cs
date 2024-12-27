using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002CD RID: 717
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyDescriptionAttribute : Attribute
	{
		// Token: 0x06001CB0 RID: 7344 RVA: 0x00049960 File Offset: 0x00048960
		public AssemblyDescriptionAttribute(string description)
		{
			this.m_description = description;
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x0004996F File Offset: 0x0004896F
		public string Description
		{
			get
			{
				return this.m_description;
			}
		}

		// Token: 0x04000A7E RID: 2686
		private string m_description;
	}
}
