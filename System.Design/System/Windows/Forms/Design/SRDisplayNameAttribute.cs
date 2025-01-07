using System;
using System.ComponentModel;
using System.Design;

namespace System.Windows.Forms.Design
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
	internal sealed class SRDisplayNameAttribute : DisplayNameAttribute
	{
		public SRDisplayNameAttribute(string displayName)
			: base(displayName)
		{
		}

		public override string DisplayName
		{
			get
			{
				if (!this.replaced)
				{
					this.replaced = true;
					base.DisplayNameValue = SR.GetString(base.DisplayName);
				}
				return base.DisplayName;
			}
		}

		private bool replaced;
	}
}
