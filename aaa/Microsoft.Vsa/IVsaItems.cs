using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x0200000C RID: 12
	[Guid("0AB1EB6A-12BD-44d0-B941-0580ADFC73DE")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	public interface IVsaItems : IEnumerable
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000060 RID: 96
		int Count
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x1700002A RID: 42
		IVsaItem this[string name]
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x1700002B RID: 43
		IVsaItem this[int index]
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x06000063 RID: 99
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		IVsaItem CreateItem(string name, VsaItemType itemType, VsaItemFlag itemFlag);

		// Token: 0x06000064 RID: 100
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Remove(string name);

		// Token: 0x06000065 RID: 101
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Remove(int index);
	}
}
