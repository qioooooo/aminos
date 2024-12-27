using System;
using System.Design;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003D0 RID: 976
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class UnsettableComboBox : ComboBox
	{
		// Token: 0x060023EA RID: 9194 RVA: 0x000C0700 File Offset: 0x000BF700
		public UnsettableComboBox()
		{
			this.notSetText = SR.GetString("UnsettableComboBox_NotSet");
			base.Items.Add(this.notSetText);
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x000C072A File Offset: 0x000BF72A
		// (set) Token: 0x060023EC RID: 9196 RVA: 0x000C0732 File Offset: 0x000BF732
		public string NotSetText
		{
			get
			{
				return this.notSetText;
			}
			set
			{
				this.notSetText = value;
				base.Items.RemoveAt(0);
				base.Items.Insert(0, this.notSetText);
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x000C0759 File Offset: 0x000BF759
		// (set) Token: 0x060023EE RID: 9198 RVA: 0x000C0778 File Offset: 0x000BF778
		public override string Text
		{
			get
			{
				if (this.SelectedIndex == 0 || this.SelectedIndex == -1)
				{
					return string.Empty;
				}
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x000C0781 File Offset: 0x000BF781
		public void AddItem(object item)
		{
			base.Items.Add(item);
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000C0790 File Offset: 0x000BF790
		public void EnsureNotSetItem()
		{
			if (base.Items.Count == 0)
			{
				base.Items.Add(this.notSetText);
			}
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x000C07B1 File Offset: 0x000BF7B1
		public bool IsSet()
		{
			return this.SelectedIndex > 0;
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000C07BC File Offset: 0x000BF7BC
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (this.SelectedIndex == 0)
			{
				this.internalChange = true;
				this.SelectedIndex = -1;
				this.internalChange = false;
			}
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000C07E2 File Offset: 0x000BF7E2
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			if (!this.internalChange)
			{
				base.OnSelectedIndexChanged(e);
			}
		}

		// Token: 0x040018CD RID: 6349
		private string notSetText;

		// Token: 0x040018CE RID: 6350
		private bool internalChange;
	}
}
