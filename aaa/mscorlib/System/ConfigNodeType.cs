using System;

namespace System
{
	// Token: 0x0200007F RID: 127
	[Serializable]
	internal enum ConfigNodeType
	{
		// Token: 0x0400023A RID: 570
		Element = 1,
		// Token: 0x0400023B RID: 571
		Attribute,
		// Token: 0x0400023C RID: 572
		Pi,
		// Token: 0x0400023D RID: 573
		XmlDecl,
		// Token: 0x0400023E RID: 574
		DocType,
		// Token: 0x0400023F RID: 575
		DTDAttribute,
		// Token: 0x04000240 RID: 576
		EntityDecl,
		// Token: 0x04000241 RID: 577
		ElementDecl,
		// Token: 0x04000242 RID: 578
		AttlistDecl,
		// Token: 0x04000243 RID: 579
		Notation,
		// Token: 0x04000244 RID: 580
		Group,
		// Token: 0x04000245 RID: 581
		IncludeSect,
		// Token: 0x04000246 RID: 582
		PCData,
		// Token: 0x04000247 RID: 583
		CData,
		// Token: 0x04000248 RID: 584
		IgnoreSect,
		// Token: 0x04000249 RID: 585
		Comment,
		// Token: 0x0400024A RID: 586
		EntityRef,
		// Token: 0x0400024B RID: 587
		Whitespace,
		// Token: 0x0400024C RID: 588
		Name,
		// Token: 0x0400024D RID: 589
		NMToken,
		// Token: 0x0400024E RID: 590
		String,
		// Token: 0x0400024F RID: 591
		Peref,
		// Token: 0x04000250 RID: 592
		Model,
		// Token: 0x04000251 RID: 593
		ATTDef,
		// Token: 0x04000252 RID: 594
		ATTType,
		// Token: 0x04000253 RID: 595
		ATTPresence,
		// Token: 0x04000254 RID: 596
		DTDSubset,
		// Token: 0x04000255 RID: 597
		LastNodeType
	}
}
