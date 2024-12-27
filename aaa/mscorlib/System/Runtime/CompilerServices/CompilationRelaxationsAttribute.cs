using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005CF RID: 1487
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Method)]
	[Serializable]
	public class CompilationRelaxationsAttribute : Attribute
	{
		// Token: 0x06003791 RID: 14225 RVA: 0x000BB841 File Offset: 0x000BA841
		public CompilationRelaxationsAttribute(int relaxations)
		{
			this.m_relaxations = relaxations;
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x000BB850 File Offset: 0x000BA850
		public CompilationRelaxationsAttribute(CompilationRelaxations relaxations)
		{
			this.m_relaxations = (int)relaxations;
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003793 RID: 14227 RVA: 0x000BB85F File Offset: 0x000BA85F
		public int CompilationRelaxations
		{
			get
			{
				return this.m_relaxations;
			}
		}

		// Token: 0x04001C9A RID: 7322
		private int m_relaxations;
	}
}
