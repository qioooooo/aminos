using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002CC RID: 716
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCompanyAttribute : Attribute
	{
		// Token: 0x06001CAE RID: 7342 RVA: 0x00049949 File Offset: 0x00048949
		public AssemblyCompanyAttribute(string company)
		{
			this.m_company = company;
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001CAF RID: 7343 RVA: 0x00049958 File Offset: 0x00048958
		public string Company
		{
			get
			{
				return this.m_company;
			}
		}

		// Token: 0x04000A7D RID: 2685
		private string m_company;
	}
}
