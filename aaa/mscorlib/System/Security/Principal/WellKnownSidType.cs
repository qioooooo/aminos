using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x0200092F RID: 2351
	[ComVisible(false)]
	public enum WellKnownSidType
	{
		// Token: 0x04002C2D RID: 11309
		NullSid,
		// Token: 0x04002C2E RID: 11310
		WorldSid,
		// Token: 0x04002C2F RID: 11311
		LocalSid,
		// Token: 0x04002C30 RID: 11312
		CreatorOwnerSid,
		// Token: 0x04002C31 RID: 11313
		CreatorGroupSid,
		// Token: 0x04002C32 RID: 11314
		CreatorOwnerServerSid,
		// Token: 0x04002C33 RID: 11315
		CreatorGroupServerSid,
		// Token: 0x04002C34 RID: 11316
		NTAuthoritySid,
		// Token: 0x04002C35 RID: 11317
		DialupSid,
		// Token: 0x04002C36 RID: 11318
		NetworkSid,
		// Token: 0x04002C37 RID: 11319
		BatchSid,
		// Token: 0x04002C38 RID: 11320
		InteractiveSid,
		// Token: 0x04002C39 RID: 11321
		ServiceSid,
		// Token: 0x04002C3A RID: 11322
		AnonymousSid,
		// Token: 0x04002C3B RID: 11323
		ProxySid,
		// Token: 0x04002C3C RID: 11324
		EnterpriseControllersSid,
		// Token: 0x04002C3D RID: 11325
		SelfSid,
		// Token: 0x04002C3E RID: 11326
		AuthenticatedUserSid,
		// Token: 0x04002C3F RID: 11327
		RestrictedCodeSid,
		// Token: 0x04002C40 RID: 11328
		TerminalServerSid,
		// Token: 0x04002C41 RID: 11329
		RemoteLogonIdSid,
		// Token: 0x04002C42 RID: 11330
		LogonIdsSid,
		// Token: 0x04002C43 RID: 11331
		LocalSystemSid,
		// Token: 0x04002C44 RID: 11332
		LocalServiceSid,
		// Token: 0x04002C45 RID: 11333
		NetworkServiceSid,
		// Token: 0x04002C46 RID: 11334
		BuiltinDomainSid,
		// Token: 0x04002C47 RID: 11335
		BuiltinAdministratorsSid,
		// Token: 0x04002C48 RID: 11336
		BuiltinUsersSid,
		// Token: 0x04002C49 RID: 11337
		BuiltinGuestsSid,
		// Token: 0x04002C4A RID: 11338
		BuiltinPowerUsersSid,
		// Token: 0x04002C4B RID: 11339
		BuiltinAccountOperatorsSid,
		// Token: 0x04002C4C RID: 11340
		BuiltinSystemOperatorsSid,
		// Token: 0x04002C4D RID: 11341
		BuiltinPrintOperatorsSid,
		// Token: 0x04002C4E RID: 11342
		BuiltinBackupOperatorsSid,
		// Token: 0x04002C4F RID: 11343
		BuiltinReplicatorSid,
		// Token: 0x04002C50 RID: 11344
		BuiltinPreWindows2000CompatibleAccessSid,
		// Token: 0x04002C51 RID: 11345
		BuiltinRemoteDesktopUsersSid,
		// Token: 0x04002C52 RID: 11346
		BuiltinNetworkConfigurationOperatorsSid,
		// Token: 0x04002C53 RID: 11347
		AccountAdministratorSid,
		// Token: 0x04002C54 RID: 11348
		AccountGuestSid,
		// Token: 0x04002C55 RID: 11349
		AccountKrbtgtSid,
		// Token: 0x04002C56 RID: 11350
		AccountDomainAdminsSid,
		// Token: 0x04002C57 RID: 11351
		AccountDomainUsersSid,
		// Token: 0x04002C58 RID: 11352
		AccountDomainGuestsSid,
		// Token: 0x04002C59 RID: 11353
		AccountComputersSid,
		// Token: 0x04002C5A RID: 11354
		AccountControllersSid,
		// Token: 0x04002C5B RID: 11355
		AccountCertAdminsSid,
		// Token: 0x04002C5C RID: 11356
		AccountSchemaAdminsSid,
		// Token: 0x04002C5D RID: 11357
		AccountEnterpriseAdminsSid,
		// Token: 0x04002C5E RID: 11358
		AccountPolicyAdminsSid,
		// Token: 0x04002C5F RID: 11359
		AccountRasAndIasServersSid,
		// Token: 0x04002C60 RID: 11360
		NtlmAuthenticationSid,
		// Token: 0x04002C61 RID: 11361
		DigestAuthenticationSid,
		// Token: 0x04002C62 RID: 11362
		SChannelAuthenticationSid,
		// Token: 0x04002C63 RID: 11363
		ThisOrganizationSid,
		// Token: 0x04002C64 RID: 11364
		OtherOrganizationSid,
		// Token: 0x04002C65 RID: 11365
		BuiltinIncomingForestTrustBuildersSid,
		// Token: 0x04002C66 RID: 11366
		BuiltinPerformanceMonitoringUsersSid,
		// Token: 0x04002C67 RID: 11367
		BuiltinPerformanceLoggingUsersSid,
		// Token: 0x04002C68 RID: 11368
		BuiltinAuthorizationAccessSid,
		// Token: 0x04002C69 RID: 11369
		WinBuiltinTerminalServerLicenseServersSid,
		// Token: 0x04002C6A RID: 11370
		MaxDefined = 60
	}
}
