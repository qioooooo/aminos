using System;
using System.Drawing;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	internal sealed class AutoSizeComboBox : ComboBox
	{
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

		public void InvalidateDropDownWidth()
		{
			this._dropDownWidthValid = false;
		}

		protected override void OnDropDown(EventArgs e)
		{
			if (!this._dropDownWidthValid)
			{
				this.AutoSizeComboBoxDropDown();
				this._dropDownWidthValid = true;
			}
			base.OnDropDown(e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this._dropDownWidthValid = false;
		}

		private const int MaxDropDownWidth = 600;

		private bool _dropDownWidthValid;
	}
}
