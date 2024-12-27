using System;
using System.Drawing;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003C6 RID: 966
	internal sealed class AutoSizeComboBox : ComboBox
	{
		// Token: 0x06002365 RID: 9061 RVA: 0x000BED24 File Offset: 0x000BDD24
		private void AutoSizeComboBoxDropDown()
		{
			int num = 0;
			using (Graphics graphics = Graphics.FromImage(new Bitmap(1, 1)))
			{
				foreach (object obj in base.Items)
				{
					if (obj != null)
					{
						num = Math.Max(num, graphics.MeasureString(obj.ToString(), this.Font, 0, new StringFormat(StringFormatFlags.FitBlackBox | StringFormatFlags.NoWrap)).ToSize().Width);
						if (num >= 600)
						{
							num = 600;
							break;
						}
					}
				}
			}
			int num2 = num + SystemInformation.VerticalScrollBarWidth + 2 * SystemInformation.BorderSize.Width;
			base.DropDownWidth = num2 + 1;
			base.DropDownWidth = num2;
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x000BEE14 File Offset: 0x000BDE14
		public void InvalidateDropDownWidth()
		{
			this._dropDownWidthValid = false;
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x000BEE1D File Offset: 0x000BDE1D
		protected override void OnDropDown(EventArgs e)
		{
			if (!this._dropDownWidthValid)
			{
				this.AutoSizeComboBoxDropDown();
				this._dropDownWidthValid = true;
			}
			base.OnDropDown(e);
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x000BEE3B File Offset: 0x000BDE3B
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this._dropDownWidthValid = false;
		}

		// Token: 0x0400189E RID: 6302
		private const int MaxDropDownWidth = 600;

		// Token: 0x0400189F RID: 6303
		private bool _dropDownWidthValid;
	}
}
