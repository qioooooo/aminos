using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web.UI.Design.Util;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	[ToolboxItem(false)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public partial class RegexEditorDialog : Form
	{
		public string RegularExpression
		{
			get
			{
				return this.regularExpression;
			}
			set
			{
				this.regularExpression = value;
			}
		}

		public RegexEditorDialog(ISite site)
		{
			this.site = site;
			this.InitializeComponent();
			this.settingValue = false;
			this.regularExpression = string.Empty;
		}

		protected void txtExpression_TextChanged(object sender, EventArgs e)
		{
			if (this.settingValue || this.firstActivate)
			{
				return;
			}
			this.lblTestResult.Text = string.Empty;
			this.UpdateExpressionList();
		}

		protected void lstStandardExpressions_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.settingValue)
			{
				return;
			}
			if (this.lstStandardExpressions.SelectedIndex >= 1)
			{
				RegexEditorDialog.CannedExpression cannedExpression = (RegexEditorDialog.CannedExpression)this.lstStandardExpressions.SelectedItem;
				this.settingValue = true;
				this.txtExpression.Text = cannedExpression.Expression;
				this.settingValue = false;
			}
		}

		protected void RegexTypeEditor_Activated(object sender, EventArgs e)
		{
			if (!this.firstActivate)
			{
				return;
			}
			this.txtExpression.Text = this.RegularExpression;
			this.UpdateExpressionList();
			this.firstActivate = false;
		}

		private void UpdateExpressionList()
		{
			bool flag = false;
			this.settingValue = true;
			string text = this.txtExpression.Text;
			for (int i = 1; i < this.lstStandardExpressions.Items.Count; i++)
			{
				if (text == ((RegexEditorDialog.CannedExpression)this.lstStandardExpressions.Items[i]).Expression)
				{
					this.lstStandardExpressions.SelectedIndex = i;
					flag = true;
				}
			}
			if (!flag)
			{
				this.lstStandardExpressions.SelectedIndex = 0;
			}
			this.settingValue = false;
		}

		protected void cmdTestValidate_Click(object sender, EventArgs args)
		{
			try
			{
				Match match = Regex.Match(this.txtSampleInput.Text, this.txtExpression.Text);
				bool flag = match.Success && match.Index == 0 && match.Length == this.txtSampleInput.Text.Length;
				if (this.txtSampleInput.Text.Length == 0)
				{
					flag = true;
				}
				this.lblTestResult.Text = (flag ? SR.GetString("RegexEditor_InputValid") : SR.GetString("RegexEditor_InputInvalid"));
				this.lblTestResult.ForeColor = (flag ? Color.Black : Color.Red);
			}
			catch
			{
				this.lblTestResult.Text = SR.GetString("RegexEditor_BadExpression");
				this.lblTestResult.ForeColor = Color.Red;
			}
		}

		private void ShowHelp()
		{
			IHelpService helpService = (IHelpService)this.site.GetService(typeof(IHelpService));
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword("net.Asp.RegularExpressionEditor");
			}
		}

		protected void cmdHelp_Click(object sender, EventArgs e)
		{
			this.ShowHelp();
		}

		private void Form_HelpRequested(object sender, HelpEventArgs e)
		{
			this.ShowHelp();
		}

		private void HelpButton_Click(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.ShowHelp();
		}

		protected void cmdOK_Click(object sender, EventArgs e)
		{
			this.RegularExpression = this.txtExpression.Text;
		}

		private object[] CannedExpressions
		{
			get
			{
				if (RegexEditorDialog.cannedExpressions == null)
				{
					ArrayList arrayList = new ArrayList();
					arrayList.Add(SR.GetString("RegexCanned_Custom"));
					arrayList.Add(new RegexEditorDialog.CannedExpression(SR.GetString("RegexCanned_Email"), "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"));
					arrayList.Add(new RegexEditorDialog.CannedExpression(SR.GetString("RegexCanned_URL"), "http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?"));
					foreach (RegexEditorDialog.RegExpEntry regExpEntry in RegexEditorDialog._entries)
					{
						if (regExpEntry.Name.Length > 0)
						{
							arrayList.Add(new RegexEditorDialog.CannedExpression(SR.GetString(regExpEntry.Name), SR.GetString(regExpEntry.Format)));
						}
					}
					RegexEditorDialog.cannedExpressions = new object[arrayList.Count];
					arrayList.CopyTo(RegexEditorDialog.cannedExpressions);
				}
				return RegexEditorDialog.cannedExpressions;
			}
		}

		private string regularExpression;

		private bool settingValue;

		private bool firstActivate = true;

		private static object[] cannedExpressions;

		private static readonly RegexEditorDialog.RegExpEntry[] _entries = new RegexEditorDialog.RegExpEntry[]
		{
			new RegexEditorDialog.RegExpEntry("RegexCanned_SocialSecurity", "RegexCanned_SocialSecurity_Format"),
			new RegexEditorDialog.RegExpEntry("RegexCanned_USPhone", "RegexCanned_USPhone_Format"),
			new RegexEditorDialog.RegExpEntry("RegexCanned_Zip", "RegexCanned_Zip_Format"),
			new RegexEditorDialog.RegExpEntry("RegexCanned_FrZip", "RegexCanned_FrZip_Format"),
			new RegexEditorDialog.RegExpEntry("RegexCanned_FrPhone", "RegexCanned_FrPhone_Format"),
			new RegexEditorDialog.RegExpEntry("RegexCanned_DeZip", "RegexCanned_DeZip_Format"),
			new RegexEditorDialog.RegExpEntry("RegexCanned_DePhone", "RegexCanned_DePhone_Format"),
			new RegexEditorDialog.RegExpEntry("RegexCanned_JpnZip", "RegexCanned_JpnZip_Format"),
			new RegexEditorDialog.RegExpEntry("RegexCanned_JpnPhone", "RegexCanned_JpnPhone_Format"),
			new RegexEditorDialog.RegExpEntry("RegexCanned_PrcZip", "RegexCanned_PrcZip_Format"),
			new RegexEditorDialog.RegExpEntry("RegexCanned_PrcPhone", "RegexCanned_PrcPhone_Format"),
			new RegexEditorDialog.RegExpEntry("RegexCanned_PrcSocialSecurity", "RegexCanned_PrcSocialSecurity_Format")
		};

		private class CannedExpression
		{
			public CannedExpression(string description, string expression)
			{
				this.Description = description;
				this.Expression = expression;
			}

			public override string ToString()
			{
				return this.Description;
			}

			public string Description;

			public string Expression;
		}

		private class RegExpEntry
		{
			public RegExpEntry(string name, string format)
			{
				this.Name = name;
				this.Format = format;
			}

			public string Name;

			public string Format;
		}
	}
}
