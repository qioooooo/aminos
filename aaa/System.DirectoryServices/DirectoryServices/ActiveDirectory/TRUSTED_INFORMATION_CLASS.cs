using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000F4 RID: 244
	internal enum TRUSTED_INFORMATION_CLASS
	{
		// Token: 0x040005D4 RID: 1492
		TrustedDomainNameInformation = 1,
		// Token: 0x040005D5 RID: 1493
		TrustedControllersInformation,
		// Token: 0x040005D6 RID: 1494
		TrustedPosixOffsetInformation,
		// Token: 0x040005D7 RID: 1495
		TrustedPasswordInformation,
		// Token: 0x040005D8 RID: 1496
		TrustedDomainInformationBasic,
		// Token: 0x040005D9 RID: 1497
		TrustedDomainInformationEx,
		// Token: 0x040005DA RID: 1498
		TrustedDomainAuthInformation,
		// Token: 0x040005DB RID: 1499
		TrustedDomainFullInformation,
		// Token: 0x040005DC RID: 1500
		TrustedDomainAuthInformationInternal,
		// Token: 0x040005DD RID: 1501
		TrustedDomainFullInformationInternal,
		// Token: 0x040005DE RID: 1502
		TrustedDomainInformationEx2Internal,
		// Token: 0x040005DF RID: 1503
		TrustedDomainFullInformation2Internal
	}
}
