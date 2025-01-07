using System;
using System.ComponentModel;

namespace System.Design
{
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class SRDescriptionAttribute : DescriptionAttribute
	{
		public SRDescriptionAttribute(string description)
			: base(description)
		{
		}

		public override string Description
		{
			get
			{
				if (!this.replaced)
				{
					this.replaced = true;
					base.DescriptionValue = SR.GetString(base.Description);
				}
				return base.Description;
			}
		}

		private bool replaced;
	}
}
