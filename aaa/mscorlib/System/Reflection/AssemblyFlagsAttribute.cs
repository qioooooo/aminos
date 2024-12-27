using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002D8 RID: 728
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyFlagsAttribute : Attribute
	{
		// Token: 0x06001CC7 RID: 7367 RVA: 0x00049A7A File Offset: 0x00048A7A
		[CLSCompliant(false)]
		[Obsolete("This constructor has been deprecated. Please use AssemblyFlagsAttribute(AssemblyNameFlags) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public AssemblyFlagsAttribute(uint flags)
		{
			this.m_flags = (AssemblyNameFlags)flags;
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x00049A89 File Offset: 0x00048A89
		[Obsolete("This property has been deprecated. Please use AssemblyFlags instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[CLSCompliant(false)]
		public uint Flags
		{
			get
			{
				return (uint)this.m_flags;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001CC9 RID: 7369 RVA: 0x00049A91 File Offset: 0x00048A91
		public int AssemblyFlags
		{
			get
			{
				return (int)this.m_flags;
			}
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x00049A99 File Offset: 0x00048A99
		[Obsolete("This constructor has been deprecated. Please use AssemblyFlagsAttribute(AssemblyNameFlags) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public AssemblyFlagsAttribute(int assemblyFlags)
		{
			this.m_flags = (AssemblyNameFlags)assemblyFlags;
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x00049AA8 File Offset: 0x00048AA8
		public AssemblyFlagsAttribute(AssemblyNameFlags assemblyFlags)
		{
			this.m_flags = assemblyFlags;
		}

		// Token: 0x04000A89 RID: 2697
		private AssemblyNameFlags m_flags;
	}
}
