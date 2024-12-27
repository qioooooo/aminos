using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Threading
{
	// Token: 0x02000136 RID: 310
	[Serializable]
	internal sealed class DomainCompressedStack
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060011CB RID: 4555 RVA: 0x00032511 File Offset: 0x00031511
		internal PermissionListSet PLS
		{
			get
			{
				return this.m_pls;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x00032519 File Offset: 0x00031519
		internal bool ConstructionHalted
		{
			get
			{
				return this.m_bHaltConstruction;
			}
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00032524 File Offset: 0x00031524
		private static DomainCompressedStack CreateManagedObject(IntPtr unmanagedDCS)
		{
			DomainCompressedStack domainCompressedStack = new DomainCompressedStack();
			domainCompressedStack.m_pls = PermissionListSet.CreateCompressedState(unmanagedDCS, out domainCompressedStack.m_bHaltConstruction);
			return domainCompressedStack;
		}

		// Token: 0x060011CE RID: 4558
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetDescCount(IntPtr dcs);

		// Token: 0x060011CF RID: 4559
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetDomainPermissionSets(IntPtr dcs, out PermissionSet granted, out PermissionSet refused);

		// Token: 0x060011D0 RID: 4560
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetDescriptorInfo(IntPtr dcs, int index, out PermissionSet granted, out PermissionSet refused, out Assembly assembly, out FrameSecurityDescriptor fsd);

		// Token: 0x060011D1 RID: 4561
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IgnoreDomain(IntPtr dcs);

		// Token: 0x040005E4 RID: 1508
		private PermissionListSet m_pls;

		// Token: 0x040005E5 RID: 1509
		private bool m_bHaltConstruction;
	}
}
