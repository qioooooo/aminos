using System;

// Token: 0x0200000E RID: 14
[AttributeUsage(AttributeTargets.Module, AllowMultiple = true)]
internal sealed class BidMetaTextAttribute : Attribute
{
	// Token: 0x0600009D RID: 157 RVA: 0x000559D8 File Offset: 0x00054DD8
	internal BidMetaTextAttribute(string str)
	{
		this._metaText = str;
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600009E RID: 158 RVA: 0x000559F4 File Offset: 0x00054DF4
	internal string MetaText
	{
		get
		{
			return this._metaText;
		}
	}

	// Token: 0x040000FD RID: 253
	private string _metaText;
}
