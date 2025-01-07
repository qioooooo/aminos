using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Microsoft.VisualBasic.CompilerServices
{
	internal sealed partial class VBInputBox : Form
	{
		internal VBInputBox()
		{
			this.Output = "";
			this.InitializeComponent();
		}

		internal VBInputBox(string Prompt, string Title, string DefaultResponse, int XPos, int YPos)
		{
			this.Output = "";
			this.InitializeComponent();
			this.InitializeInputBox(Prompt, Title, DefaultResponse, XPos, YPos);
		}

		private void InitializeInputBox(string Prompt, string Title, string DefaultResponse, int XPos, int YPos)
		{
			this.Text = Title;
			this.Label.Text = Prompt;
			this.TextBox.Text = DefaultResponse;
			this.OKButton.Click += this.OKButton_Click;
			this.MyCancelButton.Click += this.MyCancelButton_Click;
			Graphics graphics = this.Label.CreateGraphics();
			SizeF sizeF = graphics.MeasureString(Prompt, this.Label.Font, this.Label.Width);
			graphics.Dispose();
			checked
			{
				if (sizeF.Height > (float)this.Label.Height)
				{
					int num = (int)Math.Round((double)sizeF.Height) - this.Label.Height;
					Label label = this.Label;
					label.Height += num;
					TextBox textBox = this.TextBox;
					textBox.Top += num;
					this.Height += num;
				}
				if (XPos == -1 && YPos == -1)
				{
					this.StartPosition = FormStartPosition.CenterScreen;
				}
				else
				{
					if (XPos == -1)
					{
						XPos = 600;
					}
					if (YPos == -1)
					{
						YPos = 350;
					}
					this.StartPosition = FormStartPosition.Manual;
					Point point = new Point(XPos, YPos);
					this.DesktopLocation = point;
				}
			}
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			this.Output = this.TextBox.Text;
			this.Close();
		}

		private void MyCancelButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		public string Output;
	}
}
