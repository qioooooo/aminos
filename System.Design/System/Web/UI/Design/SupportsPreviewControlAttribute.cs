using System;

namespace System.Web.UI.Design
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SupportsPreviewControlAttribute : Attribute
	{
		public SupportsPreviewControlAttribute(bool supportsPreviewControl)
		{
			this._supportsPreviewControl = supportsPreviewControl;
		}

		public bool SupportsPreviewControl
		{
			get
			{
				return this._supportsPreviewControl;
			}
		}

		public override int GetHashCode()
		{
			return this._supportsPreviewControl.GetHashCode();
		}

		public override bool IsDefaultAttribute()
		{
			return this.Equals(SupportsPreviewControlAttribute.Default);
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			SupportsPreviewControlAttribute supportsPreviewControlAttribute = obj as SupportsPreviewControlAttribute;
			return supportsPreviewControlAttribute != null && supportsPreviewControlAttribute.SupportsPreviewControl == this._supportsPreviewControl;
		}

		private bool _supportsPreviewControl;

		public static readonly SupportsPreviewControlAttribute Default = new SupportsPreviewControlAttribute(false);
	}
}
