using System;

namespace System.Web.UI.Design
{
	// Token: 0x02000390 RID: 912
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SupportsPreviewControlAttribute : Attribute
	{
		// Token: 0x06002188 RID: 8584 RVA: 0x000B9D47 File Offset: 0x000B8D47
		public SupportsPreviewControlAttribute(bool supportsPreviewControl)
		{
			this._supportsPreviewControl = supportsPreviewControl;
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x000B9D56 File Offset: 0x000B8D56
		public bool SupportsPreviewControl
		{
			get
			{
				return this._supportsPreviewControl;
			}
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x000B9D5E File Offset: 0x000B8D5E
		public override int GetHashCode()
		{
			return this._supportsPreviewControl.GetHashCode();
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000B9D6B File Offset: 0x000B8D6B
		public override bool IsDefaultAttribute()
		{
			return this.Equals(SupportsPreviewControlAttribute.Default);
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000B9D78 File Offset: 0x000B8D78
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			SupportsPreviewControlAttribute supportsPreviewControlAttribute = obj as SupportsPreviewControlAttribute;
			return supportsPreviewControlAttribute != null && supportsPreviewControlAttribute.SupportsPreviewControl == this._supportsPreviewControl;
		}

		// Token: 0x04001818 RID: 6168
		private bool _supportsPreviewControl;

		// Token: 0x04001819 RID: 6169
		public static readonly SupportsPreviewControlAttribute Default = new SupportsPreviewControlAttribute(false);
	}
}
