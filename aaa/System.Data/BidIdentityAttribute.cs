using System;

// Token: 0x02000111 RID: 273
[AttributeUsage(AttributeTargets.Module, AllowMultiple = false)]
internal sealed class BidIdentityAttribute : Attribute
{
	// Token: 0x0600115E RID: 4446 RVA: 0x0021B740 File Offset: 0x0021AB40
	internal BidIdentityAttribute(string idStr)
	{
		this._identity = idStr;
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x0600115F RID: 4447 RVA: 0x0021B75C File Offset: 0x0021AB5C
	internal string IdentityString
	{
		get
		{
			return this._identity;
		}
	}

	// Token: 0x04000B58 RID: 2904
	private string _identity;
}
