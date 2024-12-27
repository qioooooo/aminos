using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002F5 RID: 757
	internal enum TypeFlags
	{
		// Token: 0x04001506 RID: 5382
		None,
		// Token: 0x04001507 RID: 5383
		Abstract,
		// Token: 0x04001508 RID: 5384
		Reference,
		// Token: 0x04001509 RID: 5385
		Special = 4,
		// Token: 0x0400150A RID: 5386
		CanBeAttributeValue = 8,
		// Token: 0x0400150B RID: 5387
		CanBeTextValue = 16,
		// Token: 0x0400150C RID: 5388
		CanBeElementValue = 32,
		// Token: 0x0400150D RID: 5389
		HasCustomFormatter = 64,
		// Token: 0x0400150E RID: 5390
		AmbiguousDataType = 128,
		// Token: 0x0400150F RID: 5391
		IgnoreDefault = 512,
		// Token: 0x04001510 RID: 5392
		HasIsEmpty = 1024,
		// Token: 0x04001511 RID: 5393
		HasDefaultConstructor = 2048,
		// Token: 0x04001512 RID: 5394
		XmlEncodingNotRequired = 4096,
		// Token: 0x04001513 RID: 5395
		UseReflection = 16384,
		// Token: 0x04001514 RID: 5396
		CollapseWhitespace = 32768,
		// Token: 0x04001515 RID: 5397
		OptionalValue = 65536,
		// Token: 0x04001516 RID: 5398
		CtorInaccessible = 131072,
		// Token: 0x04001517 RID: 5399
		UsePrivateImplementation = 262144,
		// Token: 0x04001518 RID: 5400
		GenericInterface = 524288,
		// Token: 0x04001519 RID: 5401
		Unsupported = 1048576
	}
}
