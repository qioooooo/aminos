using System;

namespace System.Deployment.Application
{
	// Token: 0x0200004E RID: 78
	internal enum ExceptionTypes
	{
		// Token: 0x040001E4 RID: 484
		Unknown,
		// Token: 0x040001E5 RID: 485
		Activation,
		// Token: 0x040001E6 RID: 486
		ComponentStore,
		// Token: 0x040001E7 RID: 487
		ActivationInProgress,
		// Token: 0x040001E8 RID: 488
		InvalidShortcut,
		// Token: 0x040001E9 RID: 489
		InvalidARPEntry,
		// Token: 0x040001EA RID: 490
		LockTimeout,
		// Token: 0x040001EB RID: 491
		Subscription,
		// Token: 0x040001EC RID: 492
		SubscriptionState,
		// Token: 0x040001ED RID: 493
		ActivationLimitExceeded,
		// Token: 0x040001EE RID: 494
		DiskIsFull,
		// Token: 0x040001EF RID: 495
		GroupMultipleMatch,
		// Token: 0x040001F0 RID: 496
		InvalidManifest,
		// Token: 0x040001F1 RID: 497
		Manifest,
		// Token: 0x040001F2 RID: 498
		ManifestLoad,
		// Token: 0x040001F3 RID: 499
		ManifestParse,
		// Token: 0x040001F4 RID: 500
		ManifestSemanticValidation,
		// Token: 0x040001F5 RID: 501
		ManifestComponentSemanticValidation,
		// Token: 0x040001F6 RID: 502
		UnsupportedElevetaionRequest,
		// Token: 0x040001F7 RID: 503
		SubscriptionSemanticValidation,
		// Token: 0x040001F8 RID: 504
		UriSchemeNotSupported,
		// Token: 0x040001F9 RID: 505
		Zone,
		// Token: 0x040001FA RID: 506
		DeploymentUriDifferent,
		// Token: 0x040001FB RID: 507
		SizeLimitForPartialTrustOnlineAppExceeded,
		// Token: 0x040001FC RID: 508
		Validation,
		// Token: 0x040001FD RID: 509
		HashValidation,
		// Token: 0x040001FE RID: 510
		SignatureValidation,
		// Token: 0x040001FF RID: 511
		RefDefValidation,
		// Token: 0x04000200 RID: 512
		ClrValidation,
		// Token: 0x04000201 RID: 513
		StronglyNamedAssemblyVerification,
		// Token: 0x04000202 RID: 514
		IdentityMatchValidationForMixedModeAssembly,
		// Token: 0x04000203 RID: 515
		AppFileLocationValidation,
		// Token: 0x04000204 RID: 516
		FileSizeValidation
	}
}
