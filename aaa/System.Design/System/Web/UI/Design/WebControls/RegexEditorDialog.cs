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
	// Token: 0x020004A3 RID: 1187
	[ToolboxItem(false)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public partial class RegexEditorDialog : Form
	{
		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002AFD RID: 11005 RVA: 0x000EDDE5 File Offset: 0x000ECDE5
		// (set) Token: 0x06002AFE RID: 11006 RVA: 0x000EDDED File Offset: 0x000ECDED
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

		// Token: 0x06002AFF RID: 11007 RVA: 0x000EDDF6 File Offset: 0x000ECDF6
		public RegexEditorDialog(ISite site)
		{
			this.site = site;
			this.InitializeComponent();
			this.settingValue = false;
			this.regularExpression = string.Empty;
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000EE466 File Offset: 0x000ED466
		protected void txtExpression_TextChanged(object sender, EventArgs e)
		{
			if (this.settingValue || this.firstActivate)
			{
				return;
			}
			this.lblTestResult.Text = string.Empty;
			this.UpdateExpressionList();
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000EE490 File Offset: 0x000ED490
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

		// Token: 0x06002B04 RID: 11012 RVA: 0x000EE4E4 File Offset: 0x000ED4E4
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

		// Token: 0x06002B05 RID: 11013 RVA: 0x000EE510 File Offset: 0x000ED510
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

		// Token: 0x06002B06 RID: 11014 RVA: 0x000EE594 File Offset: 0x000ED594
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

		// Token: 0x06002B07 RID: 11015 RVA: 0x000EE678 File Offset: 0x000ED678
		private void ShowHelp()
		{
			IHelpService helpService = (IHelpService)this.site.GetService(typeof(IHelpService));
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword("net.Asp.RegularExpressionEditor");
			}
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x000EE6AE File Offset: 0x000ED6AE
		protected void cmdHelp_Click(object sender, EventArgs e)
		{
			this.ShowHelp();
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x000EE6B6 File Offset: 0x000ED6B6
		private void Form_HelpRequested(object sender, HelpEventArgs e)
		{
			this.ShowHelp();
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x000EE6BE File Offset: 0x000ED6BE
		private void HelpButton_Click(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.ShowHelp();
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x000EE6CD File Offset: 0x000ED6CD
		protected void cmdOK_Click(object sender, EventArgs e)
		{
			this.RegularExpression = this.txtExpression.Text;
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002B0C RID: 11020 RVA: 0x000EE6E0 File Offset: 0x000ED6E0
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

		// Token: 0x04001D64 RID: 7524
		private string regularExpression;

		// Token: 0x04001D65 RID: 7525
		private bool settingValue;

		// Token: 0x04001D66 RID: 7526
		private bool firstActivate = true;

		// Token: 0x04001D68 RID: 7528
		private static object[] cannedExpressions;

		// Token: 0x04001D69 RID: 7529
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

		// Token: 0x020004A4 RID: 1188
		private class CannedExpression
		{
			// Token: 0x06002B0E RID: 11022 RVA: 0x000EE8A2 File Offset: 0x000ED8A2
			public CannedExpression(string description, string expression)
			{
				this.Description = description;
				this.Expression = expression;
			}

			// Token: 0x06002B0F RID: 11023 RVA: 0x000EE8B8 File Offset: 0x000ED8B8
			public override string ToString()
			{
				return this.Description;
			}

			// Token: 0x04001D6A RID: 7530
			public string Description;

			// Token: 0x04001D6B RID: 7531
			public string Expression;
		}

		// Token: 0x020004A5 RID: 1189
		private class RegExpEntry
		{
			// Token: 0x06002B10 RID: 11024 RVA: 0x000EE8C0 File Offset: 0x000ED8C0
			public RegExpEntry(string name, string format)
			{
				this.Name = name;
				this.Format = format;
			}

			// Token: 0x04001D6C RID: 7532
			public string Name;

			// Token: 0x04001D6D RID: 7533
			public string Format;
		}
	}
}
