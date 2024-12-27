using System;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003C8 RID: 968
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class GroupLabel : Label
	{
		// Token: 0x0600236F RID: 9071 RVA: 0x000BEFC6 File Offset: 0x000BDFC6
		public GroupLabel()
		{
			base.SetStyle(ControlStyles.UserPaint, true);
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x000BEFD8 File Offset: 0x000BDFD8
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.ClientRectangle;
			string text = this.Text;
			Brush brush = new SolidBrush(this.ForeColor);
			graphics.DrawString(text, this.Font, brush, 0f, 0f);
			brush.Dispose();
			int num = clientRectangle.X;
			if (text.Length != 0)
			{
				num += 4 + Size.Ceiling(graphics.MeasureString(text, this.Font)).Width;
			}
			int num2 = clientRectangle.Height / 2;
			graphics.DrawLine(SystemPens.ControlDark, num, num2, clientRectangle.Width, num2);
			num2++;
			graphics.DrawLine(SystemPens.ControlLightLight, num, num2, clientRectangle.Width, num2);
		}
	}
}
