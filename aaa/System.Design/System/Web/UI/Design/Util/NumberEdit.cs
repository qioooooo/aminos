using System;
using System.Globalization;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003CB RID: 971
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class NumberEdit : TextBox
	{
		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060023A8 RID: 9128 RVA: 0x000BF478 File Offset: 0x000BE478
		// (set) Token: 0x060023A9 RID: 9129 RVA: 0x000BF480 File Offset: 0x000BE480
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

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060023AA RID: 9130 RVA: 0x000BF489 File Offset: 0x000BE489
		// (set) Token: 0x060023AB RID: 9131 RVA: 0x000BF491 File Offset: 0x000BE491
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

		// Token: 0x060023AC RID: 9132 RVA: 0x000BF49C File Offset: 0x000BE49C
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

		// Token: 0x040018A7 RID: 6311
		private bool allowNegative = true;

		// Token: 0x040018A8 RID: 6312
		private bool allowDecimal = true;
	}
}
