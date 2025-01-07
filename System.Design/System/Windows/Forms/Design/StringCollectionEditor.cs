using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	internal class StringCollectionEditor : CollectionEditor
	{
		public StringCollectionEditor(Type type)
			: base(type)
		{
		}

		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			return new StringCollectionEditor.StringCollectionForm(this);
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.ComponentModel.StringCollectionEditor";
			}
		}

		private class StringCollectionForm : CollectionEditor.CollectionForm
		{
			public StringCollectionForm(CollectionEditor editor)
				: base(editor)
			{
				this.editor = (StringCollectionEditor)editor;
				this.InitializeComponent();
				this.HookEvents();
			}

			private void Edit1_keyDown(object sender, KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Escape)
				{
					this.cancelButton.PerformClick();
					e.Handled = true;
				}
			}

			private void StringCollectionEditor_HelpButtonClicked(object sender, CancelEventArgs e)
			{
				e.Cancel = true;
				this.editor.ShowHelp();
			}

			private void Form_HelpRequested(object sender, HelpEventArgs e)
			{
				this.editor.ShowHelp();
			}

			private void HookEvents()
			{
				this.textEntry.KeyDown += this.Edit1_keyDown;
				this.okButton.Click += this.OKButton_click;
				base.HelpButtonClicked += this.StringCollectionEditor_HelpButtonClicked;
			}

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

			private Label instruction;

			private TextBox textEntry;

			private Button okButton;

			private Button cancelButton;

			private TableLayoutPanel okCancelTableLayoutPanel;

			private StringCollectionEditor editor;
		}
	}
}
