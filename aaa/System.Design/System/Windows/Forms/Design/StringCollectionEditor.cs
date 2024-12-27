using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000261 RID: 609
	internal class StringCollectionEditor : CollectionEditor
	{
		// Token: 0x06001719 RID: 5913 RVA: 0x00077300 File Offset: 0x00076300
		public StringCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x00077309 File Offset: 0x00076309
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			return new StringCollectionEditor.StringCollectionForm(this);
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x0600171B RID: 5915 RVA: 0x00077311 File Offset: 0x00076311
		protected override string HelpTopic
		{
			get
			{
				return "net.ComponentModel.StringCollectionEditor";
			}
		}

		// Token: 0x02000262 RID: 610
		private class StringCollectionForm : CollectionEditor.CollectionForm
		{
			// Token: 0x0600171C RID: 5916 RVA: 0x00077318 File Offset: 0x00076318
			public StringCollectionForm(CollectionEditor editor)
				: base(editor)
			{
				this.editor = (StringCollectionEditor)editor;
				this.InitializeComponent();
				this.HookEvents();
			}

			// Token: 0x0600171D RID: 5917 RVA: 0x00077339 File Offset: 0x00076339
			private void Edit1_keyDown(object sender, KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Escape)
				{
					this.cancelButton.PerformClick();
					e.Handled = true;
				}
			}

			// Token: 0x0600171E RID: 5918 RVA: 0x00077357 File Offset: 0x00076357
			private void StringCollectionEditor_HelpButtonClicked(object sender, CancelEventArgs e)
			{
				e.Cancel = true;
				this.editor.ShowHelp();
			}

			// Token: 0x0600171F RID: 5919 RVA: 0x0007736B File Offset: 0x0007636B
			private void Form_HelpRequested(object sender, HelpEventArgs e)
			{
				this.editor.ShowHelp();
			}

			// Token: 0x06001720 RID: 5920 RVA: 0x00077378 File Offset: 0x00076378
			private void HookEvents()
			{
				this.textEntry.KeyDown += this.Edit1_keyDown;
				this.okButton.Click += this.OKButton_click;
				base.HelpButtonClicked += this.StringCollectionEditor_HelpButtonClicked;
			}

			// Token: 0x06001721 RID: 5921 RVA: 0x000773C8 File Offset: 0x000763C8
			private void InitializeComponent()
			{
				ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StringCollectionEditor));
				this.instruction = new Label();
				this.textEntry = new TextBox();
				this.okButton = new Button();
				this.cancelButton = new Button();
				this.okCancelTableLayoutPanel = new TableLayoutPanel();
				this.okCancelTableLayoutPanel.SuspendLayout();
				base.SuspendLayout();
				componentResourceManager.ApplyResources(this.instruction, "instruction");
				this.instruction.Margin = new Padding(3, 1, 3, 0);
				this.instruction.Name = "instruction";
				this.textEntry.AcceptsTab = true;
				this.textEntry.AcceptsReturn = true;
				componentResourceManager.ApplyResources(this.textEntry, "textEntry");
				this.textEntry.Name = "textEntry";
				componentResourceManager.ApplyResources(this.okButton, "okButton");
				this.okButton.DialogResult = DialogResult.OK;
				this.okButton.Margin = new Padding(0, 0, 3, 0);
				this.okButton.Name = "okButton";
				componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
				this.cancelButton.DialogResult = DialogResult.Cancel;
				this.cancelButton.Margin = new Padding(3, 0, 0, 0);
				this.cancelButton.Name = "cancelButton";
				componentResourceManager.ApplyResources(this.okCancelTableLayoutPanel, "okCancelTableLayoutPanel");
				this.okCancelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.okCancelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.okCancelTableLayoutPanel.Controls.Add(this.okButton, 0, 0);
				this.okCancelTableLayoutPanel.Controls.Add(this.cancelButton, 1, 0);
				this.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel";
				this.okCancelTableLayoutPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this, "$this");
				base.AutoScaleMode = AutoScaleMode.Font;
				base.Controls.Add(this.okCancelTableLayoutPanel);
				base.Controls.Add(this.instruction);
				base.Controls.Add(this.textEntry);
				base.HelpButton = true;
				base.MaximizeBox = false;
				base.MinimizeBox = false;
				base.Name = "StringCollectionEditor";
				base.ShowIcon = false;
				base.ShowInTaskbar = false;
				this.okCancelTableLayoutPanel.ResumeLayout(false);
				this.okCancelTableLayoutPanel.PerformLayout();
				base.HelpRequested += this.Form_HelpRequested;
				base.ResumeLayout(false);
				base.PerformLayout();
			}

			// Token: 0x06001722 RID: 5922 RVA: 0x00077668 File Offset: 0x00076668
			private void OKButton_click(object sender, EventArgs e)
			{
				char[] array = new char[] { '\n' };
				char[] array2 = new char[] { '\r' };
				string[] array3 = this.textEntry.Text.Split(array);
				object[] items = base.Items;
				int num = array3.Length;
				for (int i = 0; i < num; i++)
				{
					array3[i] = array3[i].Trim(array2);
				}
				bool flag = true;
				if (num == items.Length)
				{
					int num2 = 0;
					while (num2 < num && array3[num2].Equals((string)items[num2]))
					{
						num2++;
					}
					if (num2 == num)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					base.DialogResult = DialogResult.Cancel;
					return;
				}
				if (array3.Length > 0 && array3[array3.Length - 1].Length == 0)
				{
					num--;
				}
				object[] array4 = new object[num];
				for (int j = 0; j < num; j++)
				{
					array4[j] = array3[j];
				}
				base.Items = array4;
			}

			// Token: 0x06001723 RID: 5923 RVA: 0x00077760 File Offset: 0x00076760
			protected override void OnEditValueChanged()
			{
				object[] items = base.Items;
				string text = string.Empty;
				for (int i = 0; i < items.Length; i++)
				{
					if (items[i] is string)
					{
						text += (string)items[i];
						if (i != items.Length - 1)
						{
							text += "\r\n";
						}
					}
				}
				this.textEntry.Text = text;
			}

			// Token: 0x04001318 RID: 4888
			private Label instruction;

			// Token: 0x04001319 RID: 4889
			private TextBox textEntry;

			// Token: 0x0400131A RID: 4890
			private Button okButton;

			// Token: 0x0400131B RID: 4891
			private Button cancelButton;

			// Token: 0x0400131C RID: 4892
			private TableLayoutPanel okCancelTableLayoutPanel;

			// Token: 0x0400131D RID: 4893
			private StringCollectionEditor editor;
		}
	}
}
