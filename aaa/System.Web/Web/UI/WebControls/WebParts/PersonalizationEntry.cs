using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006DF RID: 1759
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PersonalizationEntry
	{
		// Token: 0x06005660 RID: 22112 RVA: 0x0015CB21 File Offset: 0x0015BB21
		public PersonalizationEntry(object value, PersonalizationScope scope)
			: this(value, scope, false)
		{
		}

		// Token: 0x06005661 RID: 22113 RVA: 0x0015CB2C File Offset: 0x0015BB2C
		public PersonalizationEntry(object value, PersonalizationScope scope, bool isSensitive)
		{
			PersonalizationProviderHelper.CheckPersonalizationScope(scope);
			this._value = value;
			this._scope = scope;
			this._isSensitive = isSensitive;
		}

		// Token: 0x1700164D RID: 5709
		// (get) Token: 0x06005662 RID: 22114 RVA: 0x0015CB4F File Offset: 0x0015BB4F
		// (set) Token: 0x06005663 RID: 22115 RVA: 0x0015CB57 File Offset: 0x0015BB57
		public PersonalizationScope Scope
		{
			get
			{
				return this._scope;
			}
			set
			{
				if (value < PersonalizationScope.User || value > PersonalizationScope.Shared)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._scope = value;
			}
		}

		// Token: 0x1700164E RID: 5710
		// (get) Token: 0x06005664 RID: 22116 RVA: 0x0015CB73 File Offset: 0x0015BB73
		// (set) Token: 0x06005665 RID: 22117 RVA: 0x0015CB7B File Offset: 0x0015BB7B
		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x1700164F RID: 5711
		// (get) Token: 0x06005666 RID: 22118 RVA: 0x0015CB84 File Offset: 0x0015BB84
		// (set) Token: 0x06005667 RID: 22119 RVA: 0x0015CB8C File Offset: 0x0015BB8C
		public bool IsSensitive
		{
			get
			{
				return this._isSensitive;
			}
			set
			{
				this._isSensitive = value;
			}
		}

		// Token: 0x04002F57 RID: 12119
		private PersonalizationScope _scope;

		// Token: 0x04002F58 RID: 12120
		private object _value;

		// Token: 0x04002F59 RID: 12121
		private bool _isSensitive;
	}
}
