using System;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	internal class IntegerCollectionEditor : CollectionEditor
	{
		public IntegerCollectionEditor(Type type)
			: base(type)
		{
		}

		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			return new IntegerCollectionEditor.IntegerCollectionForm(this);
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.ComponentModel.IntegerCollectionEditor";
			}
		}

		private class IntegerCollectionForm : CollectionEditor.CollectionForm
		{
			public IntegerCollectionForm(CollectionEditor editor)
				: base(editor)
			{
				this.editor = (IntegerCollectionEditor)editor;
				this.InitializeComponent();
			}

			private void Edit1_keyDown(object sender, KeyEventArgs e)
			{
				if (e.KeyCode == Keys.Escape)
				{
					this.cancelButton.PerformClick();
					e.Handled = true;
				}
			}

			private void HelpButton_click(object sender, EventArgs e)
			{
				this.editor.ShowHelp();
			}

			private void Form_HelpRequested(object sender, HelpEventArgs e)
			{
				this.editor.ShowHelp();
			}

			private void InitializeComponent()
			{
				this.instruction.Location = new Point(4, 7);
				this.instruction.Size = new Size(422, 14);
				this.instruction.TabIndex = 0;
				this.instruction.TabStop = false;
				this.instruction.Text = SR.GetString("IntegerCollectionEditorInstruction");
				this.textEntry.Location = new Point(4, 22);
				this.textEntry.Size = new Size(422, 244);
				this.textEntry.TabIndex = 0;
				this.textEntry.Text = "";
				this.textEntry.AcceptsTab = false;
				this.textEntry.AcceptsReturn = true;
				this.textEntry.AutoSize = false;
				this.textEntry.Multiline = true;
				this.textEntry.ScrollBars = ScrollBars.Both;
				this.textEntry.WordWrap = false;
				this.textEntry.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
				this.textEntry.KeyDown += this.Edit1_keyDown;
				this.okButton.Location = new Point(185, 274);
				this.okButton.Size = new Size(75, 23);
				this.okButton.TabIndex = 1;
				this.okButton.Text = SR.GetString("IntegerCollectionEditorOKCaption");
				this.okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
				this.okButton.DialogResult = DialogResult.OK;
				this.okButton.Click += this.OKButton_click;
				this.cancelButton.Location = new Point(264, 274);
				this.cancelButton.Size = new Size(75, 23);
				this.cancelButton.TabIndex = 2;
				this.cancelButton.Text = SR.GetString("IntegerCollectionEditorCancelCaption");
				this.cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
				this.cancelButton.DialogResult = DialogResult.Cancel;
				this.helpButton.Location = new Point(343, 274);
				this.helpButton.Size = new Size(75, 23);
				this.helpButton.TabIndex = 3;
				this.helpButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
				this.helpButton.Text = SR.GetString("IntegerCollectionEditorHelpCaption");
				base.Location = new Point(7, 7);
				this.Text = SR.GetString("IntegerCollectionEditorTitle");
				base.AcceptButton = this.okButton;
				base.AutoScaleMode = AutoScaleMode.Font;
				base.AutoScaleDimensions = new SizeF(6f, 13f);
				base.CancelButton = this.cancelButton;
				base.ClientSize = new Size(429, 307);
				base.MaximizeBox = false;
				base.MinimizeBox = false;
				base.ControlBox = false;
				base.ShowInTaskbar = false;
				base.StartPosition = FormStartPosition.CenterScreen;
				this.MinimumSize = new Size(300, 200);
				this.helpButton.Click += this.HelpButton_click;
				base.HelpRequested += this.Form_HelpRequested;
				base.Controls.Clear();
				base.Controls.AddRange(new Control[] { this.instruction, this.textEntry, this.okButton, this.cancelButton, this.helpButton });
			}

			private void OKButton_click(object sender, EventArgs e)
			{
				char[] array = new char[] { '\n' };
				char[] array2 = new char[] { '\r' };
				string[] array3 = this.textEntry.Text.Split(array);
				object[] items = base.Items;
				int num = array3.Length;
				if (array3.Length > 0 && array3[array3.Length - 1].Length == 0)
				{
					num--;
				}
				int[] array4 = new int[num];
				for (int i = 0; i < num; i++)
				{
					array3[i] = array3[i].Trim(array2);
					try
					{
						array4[i] = int.Parse(array3[i], CultureInfo.CurrentCulture);
					}
					catch (Exception ex)
					{
						this.DisplayError(ex);
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
					}
					catch
					{
					}
				}
				bool flag = true;
				if (num == items.Length)
				{
					int num2 = 0;
					while (num2 < num && array4[num2].Equals((int)items[num2]))
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
				object[] array5 = new object[num];
				for (int j = 0; j < num; j++)
				{
					array5[j] = array4[j];
				}
				base.Items = array5;
			}

			protected override void OnEditValueChanged()
			{
				object[] items = base.Items;
				string text = string.Empty;
				for (int i = 0; i < items.Length; i++)
				{
					if (items[i] is int)
					{
						text += ((int)items[i]).ToString(CultureInfo.CurrentCulture);
						if (i != items.Length - 1)
						{
							text += "\r\n";
						}
					}
				}
				this.textEntry.Text = text;
			}

			private Label instruction = new Label();

			private TextBox textEntry = new TextBox();

			private Button okButton = new Button();

			private Button cancelButton = new Button();

			private Button helpButton = new Button();

			private IntegerCollectionEditor editor;
		}
	}
}
