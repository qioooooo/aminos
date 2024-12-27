using System;
using System.Drawing;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000536 RID: 1334
	[SupportsEventValidation]
	internal class DataControlPagerLinkButton : DataControlLinkButton
	{
		// Token: 0x060041BB RID: 16827 RVA: 0x00110171 File Offset: 0x0010F171
		internal DataControlPagerLinkButton(IPostBackContainer container)
			: base(container)
		{
		}

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x060041BC RID: 16828 RVA: 0x0011017A File Offset: 0x0010F17A
		// (set) Token: 0x060041BD RID: 16829 RVA: 0x0011017D File Offset: 0x0010F17D
		public override bool CausesValidation
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("CannotSetValidationOnPagerButtons"));
			}
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x00110190 File Offset: 0x0010F190
		protected override void SetForeColor()
		{
			if (!base.ControlStyle.IsSet(4))
			{
				Control control = this;
				for (int i = 0; i < 6; i++)
				{
					control = control.Parent;
					Color foreColor = ((WebControl)control).ForeColor;
					if (foreColor != Color.Empty)
					{
						this.ForeColor = foreColor;
						return;
					}
				}
			}
		}
	}
}
