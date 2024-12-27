using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x02000059 RID: 89
	[Guid("6EB22876-8A19-11D0-81B6-00A0C9231C29")]
	[ComImport]
	internal interface IRoleAssociationUtil
	{
		// Token: 0x060001AA RID: 426
		[DispId(1)]
		void AssociateRole([MarshalAs(UnmanagedType.BStr)] [In] string bstrRoleID);

		// Token: 0x060001AB RID: 427
		[DispId(2)]
		void AssociateRoleByName([MarshalAs(UnmanagedType.BStr)] [In] string bstrRoleName);
	}
}
