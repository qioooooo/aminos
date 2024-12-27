using System;
using System.Configuration.Assemblies;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002D7 RID: 727
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyAlgorithmIdAttribute : Attribute
	{
		// Token: 0x06001CC4 RID: 7364 RVA: 0x00049A54 File Offset: 0x00048A54
		public AssemblyAlgorithmIdAttribute(AssemblyHashAlgorithm algorithmId)
		{
			this.m_algId = (uint)algorithmId;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x00049A63 File Offset: 0x00048A63
		[CLSCompliant(false)]
		public AssemblyAlgorithmIdAttribute(uint algorithmId)
		{
			this.m_algId = algorithmId;
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x00049A72 File Offset: 0x00048A72
		[CLSCompliant(false)]
		public uint AlgorithmId
		{
			get
			{
				return this.m_algId;
			}
		}

		// Token: 0x04000A88 RID: 2696
		private uint m_algId;
	}
}
