using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000049 RID: 73
	public enum ResultCode
	{
		// Token: 0x04000131 RID: 305
		Success,
		// Token: 0x04000132 RID: 306
		OperationsError,
		// Token: 0x04000133 RID: 307
		ProtocolError,
		// Token: 0x04000134 RID: 308
		TimeLimitExceeded,
		// Token: 0x04000135 RID: 309
		SizeLimitExceeded,
		// Token: 0x04000136 RID: 310
		CompareFalse,
		// Token: 0x04000137 RID: 311
		CompareTrue,
		// Token: 0x04000138 RID: 312
		AuthMethodNotSupported,
		// Token: 0x04000139 RID: 313
		StrongAuthRequired,
		// Token: 0x0400013A RID: 314
		ReferralV2,
		// Token: 0x0400013B RID: 315
		Referral,
		// Token: 0x0400013C RID: 316
		AdminLimitExceeded,
		// Token: 0x0400013D RID: 317
		UnavailableCriticalExtension,
		// Token: 0x0400013E RID: 318
		ConfidentialityRequired,
		// Token: 0x0400013F RID: 319
		SaslBindInProgress,
		// Token: 0x04000140 RID: 320
		NoSuchAttribute = 16,
		// Token: 0x04000141 RID: 321
		UndefinedAttributeType,
		// Token: 0x04000142 RID: 322
		InappropriateMatching,
		// Token: 0x04000143 RID: 323
		ConstraintViolation,
		// Token: 0x04000144 RID: 324
		AttributeOrValueExists,
		// Token: 0x04000145 RID: 325
		InvalidAttributeSyntax,
		// Token: 0x04000146 RID: 326
		NoSuchObject = 32,
		// Token: 0x04000147 RID: 327
		AliasProblem,
		// Token: 0x04000148 RID: 328
		InvalidDNSyntax,
		// Token: 0x04000149 RID: 329
		AliasDereferencingProblem = 36,
		// Token: 0x0400014A RID: 330
		InappropriateAuthentication = 48,
		// Token: 0x0400014B RID: 331
		InsufficientAccessRights = 50,
		// Token: 0x0400014C RID: 332
		Busy,
		// Token: 0x0400014D RID: 333
		Unavailable,
		// Token: 0x0400014E RID: 334
		UnwillingToPerform,
		// Token: 0x0400014F RID: 335
		LoopDetect,
		// Token: 0x04000150 RID: 336
		SortControlMissing = 60,
		// Token: 0x04000151 RID: 337
		OffsetRangeError,
		// Token: 0x04000152 RID: 338
		NamingViolation = 64,
		// Token: 0x04000153 RID: 339
		ObjectClassViolation,
		// Token: 0x04000154 RID: 340
		NotAllowedOnNonLeaf,
		// Token: 0x04000155 RID: 341
		NotAllowedOnRdn,
		// Token: 0x04000156 RID: 342
		EntryAlreadyExists,
		// Token: 0x04000157 RID: 343
		ObjectClassModificationsProhibited,
		// Token: 0x04000158 RID: 344
		ResultsTooLarge,
		// Token: 0x04000159 RID: 345
		AffectsMultipleDsas,
		// Token: 0x0400015A RID: 346
		VirtualListViewError = 76,
		// Token: 0x0400015B RID: 347
		Other = 80
	}
}
