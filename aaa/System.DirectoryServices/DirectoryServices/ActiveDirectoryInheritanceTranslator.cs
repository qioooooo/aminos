using System;
using System.ComponentModel;
using System.Security.AccessControl;

namespace System.DirectoryServices
{
	// Token: 0x02000010 RID: 16
	internal sealed class ActiveDirectoryInheritanceTranslator
	{
		// Token: 0x0600002F RID: 47 RVA: 0x0000269F File Offset: 0x0000169F
		internal static InheritanceFlags GetInheritanceFlags(ActiveDirectorySecurityInheritance inheritanceType)
		{
			if (inheritanceType < ActiveDirectorySecurityInheritance.None || inheritanceType > ActiveDirectorySecurityInheritance.Children)
			{
				throw new InvalidEnumArgumentException("inheritanceType", (int)inheritanceType, typeof(ActiveDirectorySecurityInheritance));
			}
			return ActiveDirectoryInheritanceTranslator.ITToIF[(int)inheritanceType];
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000026C6 File Offset: 0x000016C6
		internal static PropagationFlags GetPropagationFlags(ActiveDirectorySecurityInheritance inheritanceType)
		{
			if (inheritanceType < ActiveDirectorySecurityInheritance.None || inheritanceType > ActiveDirectorySecurityInheritance.Children)
			{
				throw new InvalidEnumArgumentException("inheritanceType", (int)inheritanceType, typeof(ActiveDirectorySecurityInheritance));
			}
			return ActiveDirectoryInheritanceTranslator.ITToPF[(int)inheritanceType];
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000026F0 File Offset: 0x000016F0
		internal static ActiveDirectorySecurityInheritance GetEffectiveInheritanceFlags(InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			ActiveDirectorySecurityInheritance activeDirectorySecurityInheritance = ActiveDirectorySecurityInheritance.None;
			if ((inheritanceFlags & InheritanceFlags.ContainerInherit) != InheritanceFlags.None)
			{
				switch (propagationFlags)
				{
				case PropagationFlags.None:
					activeDirectorySecurityInheritance = ActiveDirectorySecurityInheritance.All;
					break;
				case PropagationFlags.NoPropagateInherit:
					activeDirectorySecurityInheritance = ActiveDirectorySecurityInheritance.SelfAndChildren;
					break;
				case PropagationFlags.InheritOnly:
					activeDirectorySecurityInheritance = ActiveDirectorySecurityInheritance.Descendents;
					break;
				case PropagationFlags.NoPropagateInherit | PropagationFlags.InheritOnly:
					activeDirectorySecurityInheritance = ActiveDirectorySecurityInheritance.Children;
					break;
				default:
					throw new ArgumentException("propagationFlags");
				}
			}
			return activeDirectorySecurityInheritance;
		}

		// Token: 0x0400014E RID: 334
		internal static InheritanceFlags[] ITToIF = new InheritanceFlags[]
		{
			InheritanceFlags.None,
			InheritanceFlags.ContainerInherit,
			InheritanceFlags.ContainerInherit,
			InheritanceFlags.ContainerInherit,
			InheritanceFlags.ContainerInherit
		};

		// Token: 0x0400014F RID: 335
		internal static PropagationFlags[] ITToPF = new PropagationFlags[]
		{
			PropagationFlags.None,
			PropagationFlags.None,
			PropagationFlags.InheritOnly,
			PropagationFlags.NoPropagateInherit,
			PropagationFlags.NoPropagateInherit | PropagationFlags.InheritOnly
		};
	}
}
