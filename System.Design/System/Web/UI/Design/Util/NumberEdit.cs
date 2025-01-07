using System;
using System.Globalization;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class NumberEdit : TextBox
	{
		public bool AllowDecimal
		{
			get
			{
				return this.allowDecimal;
			}
			set
			{
				this.allowDecimal = value;
			}
		}

		public bool AllowNegative
		{
			get
			{
				return this.allowNegative;
			}
			set
			{
				this.allowNegative = value;
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 258)
			{
				char c = (char)(int)m.WParam;
				if ((c < '0' || c > '9') && (!NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.Contains(c.ToString(CultureInfo.CurrentCulture)) || !this.allowDecimal) && (!NumberFormatInfo.CurrentInfo.NegativeSign.Contains(c.ToString(CultureInfo.CurrentCulture)) || !this.allowNegative) && c != '\b')
				{
					Console.Beep();
					return;
				}
			}
			base.WndProc(ref m);
		}

		private bool allowNegative = true;

		private bool allowDecimal = true;
	}
}
