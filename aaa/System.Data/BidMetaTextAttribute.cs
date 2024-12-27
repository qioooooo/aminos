using System;

// Token: 0x02000112 RID: 274
[AttributeUsage(AttributeTargets.Module, AllowMultiple = true)]
internal sealed class BidMetaTextAttribute : Attribute
{
	// Token: 0x06001160 RID: 4448 RVA: 0x0021B770 File Offset: 0x0021AB70
	internal BidMetaTextAttribute(string str)
	{
		this._metaText = str;
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06001161 RID: 4449 RVA: 0x0021B78C File Offset: 0x0021AB8C
	internal string MetaText
	{
		get
		{
			return this._metaText;
		}
	}

	// Token: 0x04000B59 RID: 2905
	private string _metaText;
}
