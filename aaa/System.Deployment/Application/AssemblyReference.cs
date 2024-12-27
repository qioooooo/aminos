using System;
using System.Reflection;

namespace System.Deployment.Application
{
	// Token: 0x02000079 RID: 121
	internal class AssemblyReference
	{
		// Token: 0x060003A0 RID: 928 RVA: 0x000151C0 File Offset: 0x000141C0
		public AssemblyReference(AssemblyName name)
		{
			this._name = name;
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x000151CF File Offset: 0x000141CF
		public AssemblyName Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x040002A4 RID: 676
		private AssemblyName _name;
	}
}
