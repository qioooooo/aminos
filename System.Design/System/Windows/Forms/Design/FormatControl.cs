using System;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	internal class FormatControl : UserControl
	{
		public FormatControl()
		{
			this.InitializeComponent();
		}

		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
			set
			{
				this.dirty = value;
			}
		}

		public string FormatType
		{
			get
			{
				FormatControl.FormatTypeClass formatTypeClass = this.formatTypeListBox.SelectedItem as FormatControl.FormatTypeClass;
				if (formatTypeClass != null)
				{
					return formatTypeClass.ToString();
				}
				return string.Empty;
			}
			set
			{
				this.formatTypeListBox.SelectedIndex = 0;
				for (int i = 0; i < this.formatTypeListBox.Items.Count; i++)
				{
					FormatControl.FormatTypeClass formatTypeClass = this.formatTypeListBox.Items[i] as FormatControl.FormatTypeClass;
					if (formatTypeClass.ToString().Equals(value))
					{
						this.formatTypeListBox.SelectedIndex = i;
					}
				}
			}
		}

		public FormatControl.FormatTypeClass FormatTypeItem
		{
			get
			{
				return this.formatTypeListBox.SelectedItem as FormatControl.FormatTypeClass;
			}
		}

		public string NullValue
		{
			get
			{
				string text = this.nullValueTextBox.Text.Trim();
				if (text.Length != 0)
				{
					return text;
				}
				return null;
			}
			set
			{
				this.nullValueTextBox.TextChanged -= this.nullValueTextBox_TextChanged;
				this.nullValueTextBox.Text = value;
				this.nullValueTextBox.TextChanged += this.nullValueTextBox_TextChanged;
			}
		}

		public bool NullValueTextBoxEnabled
		{
			set
			{
				this.nullValueTextBox.Enabled = value;
			}
		}

		private void customStringTextBox_TextChanged(object sender, EventArgs e)
		{
			FormatControl.CustomFormatType customFormatType = this.formatTypeListBox.SelectedItem as FormatControl.CustomFormatType;
			this.sampleLabel.Text = customFormatType.SampleString;
			this.dirty = true;
		}

		private void dateTimeFormatsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			FormatControl.FormatTypeClass formatTypeClass = this.formatTypeListBox.SelectedItem as FormatControl.FormatTypeClass;
			this.sampleLabel.Text = formatTypeClass.SampleString;
			this.dirty = true;
		}

		private void decimalPlacesUpDown_ValueChanged(object sender, EventArgs e)
		{
			FormatControl.FormatTypeClass formatTypeClass = this.formatTypeListBox.SelectedItem as FormatControl.FormatTypeClass;
			this.sampleLabel.Text = formatTypeClass.SampleString;
			this.dirty = true;
		}

		private void formatGroupBox_Enter(object sender, EventArgs e)
		{
		}

		private void formatTypeListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			FormatControl.FormatTypeClass formatTypeClass = this.formatTypeListBox.SelectedItem as FormatControl.FormatTypeClass;
			this.UpdateControlVisibility(formatTypeClass);
			this.sampleLabel.Text = formatTypeClass.SampleString;
			this.explanationLabel.Text = formatTypeClass.TopLabelString;
			this.dirty = true;
		}

		public static string FormatTypeStringFromFormatString(string formatString)
		{
			if (string.IsNullOrEmpty(formatString))
			{
				return SR.GetString("BindingFormattingDialogFormatTypeNoFormatting");
			}
			if (FormatControl.NumericFormatType.ParseStatic(formatString))
			{
				return SR.GetString("BindingFormattingDialogFormatTypeNumeric");
			}
			if (FormatControl.CurrencyFormatType.ParseStatic(formatString))
			{
				return SR.GetString("BindingFormattingDialogFormatTypeCurrency");
			}
			if (FormatControl.DateTimeFormatType.ParseStatic(formatString))
			{
				return SR.GetString("BindingFormattingDialogFormatTypeDateTime");
			}
			if (FormatControl.ScientificFormatType.ParseStatic(formatString))
			{
				return SR.GetString("BindingFormattingDialogFormatTypeScientific");
			}
			return SR.GetString("BindingFormattingDialogFormatTypeCustom");
		}

		protected override bool ProcessMnemonic(char charCode)
		{
			if (Control.IsMnemonic(charCode, this.formatTypeLabel.Text))
			{
				this.formatTypeListBox.Focus();
				return true;
			}
			if (Control.IsMnemonic(charCode, this.nullValueLabel.Text))
			{
				this.nullValueTextBox.Focus();
				return true;
			}
			switch (this.formatTypeListBox.SelectedIndex)
			{
			case 0:
				return false;
			case 1:
			case 2:
			case 4:
				if (Control.IsMnemonic(charCode, this.secondRowLabel.Text))
				{
					this.decimalPlacesUpDown.Focus();
					return true;
				}
				return false;
			case 3:
				if (Control.IsMnemonic(charCode, this.secondRowLabel.Text))
				{
					this.dateTimeFormatsListBox.Focus();
					return true;
				}
				return false;
			case 5:
				if (Control.IsMnemonic(charCode, this.secondRowLabel.Text))
				{
					this.customStringTextBox.Focus();
					return true;
				}
				return false;
			default:
				return false;
			}
		}

		public void ResetFormattingInfo()
		{
			this.decimalPlacesUpDown.ValueChanged -= this.decimalPlacesUpDown_ValueChanged;
			this.customStringTextBox.TextChanged -= this.customStringTextBox_TextChanged;
			this.dateTimeFormatsListBox.SelectedIndexChanged -= this.dateTimeFormatsListBox_SelectedIndexChanged;
			this.formatTypeListBox.SelectedIndexChanged -= this.formatTypeListBox_SelectedIndexChanged;
			this.decimalPlacesUpDown.Value = 2m;
			this.nullValueTextBox.Text = string.Empty;
			this.dateTimeFormatsListBox.SelectedIndex = -1;
			this.formatTypeListBox.SelectedIndex = -1;
			this.customStringTextBox.Text = string.Empty;
			this.decimalPlacesUpDown.ValueChanged += this.decimalPlacesUpDown_ValueChanged;
			this.customStringTextBox.TextChanged += this.customStringTextBox_TextChanged;
			this.dateTimeFormatsListBox.SelectedIndexChanged += this.dateTimeFormatsListBox_SelectedIndexChanged;
			this.formatTypeListBox.SelectedIndexChanged += this.formatTypeListBox_SelectedIndexChanged;
		}

		private void UpdateControlVisibility(FormatControl.FormatTypeClass formatType)
		{
			if (formatType == null)
			{
				this.explanationLabel.Visible = false;
				this.sampleLabel.Visible = false;
				this.nullValueLabel.Visible = false;
				this.secondRowLabel.Visible = false;
				this.nullValueTextBox.Visible = false;
				this.thirdRowLabel.Visible = false;
				this.dateTimeFormatsListBox.Visible = false;
				this.customStringTextBox.Visible = false;
				this.decimalPlacesUpDown.Visible = false;
				return;
			}
			this.tableLayoutPanel1.SuspendLayout();
			this.secondRowLabel.Text = "";
			if (formatType.DropDownVisible)
			{
				this.secondRowLabel.Text = SR.GetString("BindingFormattingDialogDecimalPlaces");
				this.decimalPlacesUpDown.Visible = true;
			}
			else
			{
				this.decimalPlacesUpDown.Visible = false;
			}
			if (formatType.FormatStringTextBoxVisible)
			{
				this.secondRowLabel.Text = SR.GetString("BindingFormattingDialogCustomFormat");
				this.thirdRowLabel.Visible = true;
				this.tableLayoutPanel1.SetColumn(this.thirdRowLabel, 0);
				this.tableLayoutPanel1.SetColumnSpan(this.thirdRowLabel, 2);
				this.customStringTextBox.Visible = true;
				if (this.tableLayoutPanel1.Controls.Contains(this.dateTimeFormatsListBox))
				{
					this.tableLayoutPanel1.Controls.Remove(this.dateTimeFormatsListBox);
				}
				this.tableLayoutPanel1.Controls.Add(this.customStringTextBox, 1, 1);
			}
			else
			{
				this.thirdRowLabel.Visible = false;
				this.customStringTextBox.Visible = false;
			}
			if (formatType.ListBoxVisible)
			{
				this.secondRowLabel.Text = SR.GetString("BindingFormattingDialogType");
				if (this.tableLayoutPanel1.Controls.Contains(this.customStringTextBox))
				{
					this.tableLayoutPanel1.Controls.Remove(this.customStringTextBox);
				}
				this.dateTimeFormatsListBox.Visible = true;
				this.tableLayoutPanel1.Controls.Add(this.dateTimeFormatsListBox, 0, 2);
				this.tableLayoutPanel1.SetColumn(this.dateTimeFormatsListBox, 0);
				this.tableLayoutPanel1.SetColumnSpan(this.dateTimeFormatsListBox, 2);
			}
			else
			{
				this.dateTimeFormatsListBox.Visible = false;
			}
			this.tableLayoutPanel1.ResumeLayout(true);
		}

		private void UpdateCustomStringTextBox()
		{
			this.customStringTextBox = new TextBox();
			this.customStringTextBox.AccessibleDescription = SR.GetString("BindingFormattingDialogCustomFormatAccessibleDescription");
			this.customStringTextBox.Margin = new Padding(0, 3, 0, 3);
			this.customStringTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			this.customStringTextBox.TabIndex = 3;
			this.customStringTextBox.TextChanged += this.customStringTextBox_TextChanged;
		}

		private void UpdateFormatTypeListBoxHeight()
		{
			this.formatTypeListBox.Height = this.tableLayoutPanel1.Bottom - this.formatTypeListBox.Top;
		}

		private void UpdateFormatTypeListBoxItems()
		{
			this.dateTimeFormatsListBox.SelectedIndexChanged -= this.dateTimeFormatsListBox_SelectedIndexChanged;
			this.dateTimeFormatsListBox.Items.Clear();
			this.dateTimeFormatsListBox.Items.Add(new FormatControl.DateTimeFormatsListBoxItem(FormatControl.dateTimeFormatValue, "d"));
			this.dateTimeFormatsListBox.Items.Add(new FormatControl.DateTimeFormatsListBoxItem(FormatControl.dateTimeFormatValue, "D"));
			this.dateTimeFormatsListBox.Items.Add(new FormatControl.DateTimeFormatsListBoxItem(FormatControl.dateTimeFormatValue, "f"));
			this.dateTimeFormatsListBox.Items.Add(new FormatControl.DateTimeFormatsListBoxItem(FormatControl.dateTimeFormatValue, "F"));
			this.dateTimeFormatsListBox.Items.Add(new FormatControl.DateTimeFormatsListBoxItem(FormatControl.dateTimeFormatValue, "g"));
			this.dateTimeFormatsListBox.Items.Add(new FormatControl.DateTimeFormatsListBoxItem(FormatControl.dateTimeFormatValue, "G"));
			this.dateTimeFormatsListBox.Items.Add(new FormatControl.DateTimeFormatsListBoxItem(FormatControl.dateTimeFormatValue, "t"));
			this.dateTimeFormatsListBox.Items.Add(new FormatControl.DateTimeFormatsListBoxItem(FormatControl.dateTimeFormatValue, "T"));
			this.dateTimeFormatsListBox.Items.Add(new FormatControl.DateTimeFormatsListBoxItem(FormatControl.dateTimeFormatValue, "M"));
			this.dateTimeFormatsListBox.SelectedIndex = 0;
			this.dateTimeFormatsListBox.SelectedIndexChanged += this.dateTimeFormatsListBox_SelectedIndexChanged;
		}

		private void UpdateTBLHeight()
		{
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel1.Controls.Add(this.customStringTextBox, 1, 1);
			this.customStringTextBox.Visible = false;
			this.thirdRowLabel.MaximumSize = new Size(this.tableLayoutPanel1.Width, 0);
			this.dateTimeFormatsListBox.Visible = false;
			this.tableLayoutPanel1.SetColumn(this.thirdRowLabel, 0);
			this.tableLayoutPanel1.SetColumnSpan(this.thirdRowLabel, 2);
			this.thirdRowLabel.AutoSize = true;
			this.tableLayoutPanel1.ResumeLayout(true);
			this.tableLayoutPanel1.MinimumSize = new Size(this.tableLayoutPanel1.Width, this.tableLayoutPanel1.Height);
		}

		private void FormatControl_Load(object sender, EventArgs e)
		{
			if (this.loaded)
			{
				return;
			}
			this.nullValueLabel.Text = SR.GetString("BindingFormattingDialogNullValue");
			int num = this.nullValueLabel.Width;
			int num2 = this.nullValueLabel.Height;
			this.secondRowLabel.Text = SR.GetString("BindingFormattingDialogDecimalPlaces");
			num = Math.Max(num, this.secondRowLabel.Width);
			num2 = Math.Max(num2, this.secondRowLabel.Height);
			this.secondRowLabel.Text = SR.GetString("BindingFormattingDialogCustomFormat");
			num = Math.Max(num, this.secondRowLabel.Width);
			num2 = Math.Max(num2, this.secondRowLabel.Height);
			this.nullValueLabel.MinimumSize = new Size(num, num2);
			this.secondRowLabel.MinimumSize = new Size(num, num2);
			this.formatTypeListBox.SelectedIndexChanged -= this.formatTypeListBox_SelectedIndexChanged;
			this.formatTypeListBox.Items.Clear();
			this.formatTypeListBox.Items.Add(new FormatControl.NoFormattingFormatType());
			this.formatTypeListBox.Items.Add(new FormatControl.NumericFormatType(this));
			this.formatTypeListBox.Items.Add(new FormatControl.CurrencyFormatType(this));
			this.formatTypeListBox.Items.Add(new FormatControl.DateTimeFormatType(this));
			this.formatTypeListBox.Items.Add(new FormatControl.ScientificFormatType(this));
			this.formatTypeListBox.Items.Add(new FormatControl.CustomFormatType(this));
			this.formatTypeListBox.SelectedIndex = 0;
			this.formatTypeListBox.SelectedIndexChanged += this.formatTypeListBox_SelectedIndexChanged;
			this.UpdateCustomStringTextBox();
			this.UpdateTBLHeight();
			this.UpdateFormatTypeListBoxHeight();
			this.UpdateFormatTypeListBoxItems();
			this.UpdateControlVisibility(this.formatTypeListBox.SelectedItem as FormatControl.FormatTypeClass);
			this.sampleLabel.Text = (this.formatTypeListBox.SelectedItem as FormatControl.FormatTypeClass).SampleString;
			this.explanationLabel.Size = new Size(this.formatGroupBox.Width - 10, 30);
			this.explanationLabel.Text = (this.formatTypeListBox.SelectedItem as FormatControl.FormatTypeClass).TopLabelString;
			this.dirty = false;
			this.FormatControlFinishedLoading();
			this.loaded = true;
		}

		private void FormatControlFinishedLoading()
		{
			FormatStringDialog formatStringDialog = null;
			for (Control control = base.Parent; control != null; control = control.Parent)
			{
				BindingFormattingDialog bindingFormattingDialog = control as BindingFormattingDialog;
				formatStringDialog = control as FormatStringDialog;
				if (bindingFormattingDialog != null || formatStringDialog != null)
				{
					break;
				}
			}
			if (formatStringDialog != null)
			{
				formatStringDialog.FormatControlFinishedLoading();
			}
		}

		private void nullValueTextBox_TextChanged(object sender, EventArgs e)
		{
			this.dirty = true;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormatControl));
			this.formatGroupBox = new GroupBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.secondRowLabel = new Label();
			this.nullValueLabel = new Label();
			this.nullValueTextBox = new TextBox();
			this.decimalPlacesUpDown = new NumericUpDown();
			this.thirdRowLabel = new Label();
			this.dateTimeFormatsListBox = new ListBox();
			this.sampleGroupBox = new GroupBox();
			this.sampleLabel = new Label();
			this.formatTypeListBox = new ListBox();
			this.formatTypeLabel = new Label();
			this.explanationLabel = new Label();
			this.formatGroupBox.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((ISupportInitialize)this.decimalPlacesUpDown).BeginInit();
			this.sampleGroupBox.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.formatGroupBox, "formatGroupBox");
			this.formatGroupBox.Margin = new Padding(0);
			this.formatGroupBox.Controls.Add(this.tableLayoutPanel1);
			this.formatGroupBox.Controls.Add(this.sampleGroupBox);
			this.formatGroupBox.Controls.Add(this.formatTypeListBox);
			this.formatGroupBox.Controls.Add(this.formatTypeLabel);
			this.formatGroupBox.Controls.Add(this.explanationLabel);
			this.formatGroupBox.Name = "formatGroupBox";
			this.formatGroupBox.TabStop = false;
			this.formatGroupBox.Enter += this.formatGroupBox_Enter;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			this.tableLayoutPanel1.Controls.Add(this.secondRowLabel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.nullValueLabel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.nullValueTextBox, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.decimalPlacesUpDown, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.thirdRowLabel, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.dateTimeFormatsListBox, 0, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
			componentResourceManager.ApplyResources(this.secondRowLabel, "secondRowLabel");
			this.secondRowLabel.MinimumSize = new Size(81, 14);
			this.secondRowLabel.Name = "secondRowLabel";
			componentResourceManager.ApplyResources(this.nullValueLabel, "nullValueLabel");
			this.nullValueLabel.MinimumSize = new Size(81, 14);
			this.nullValueLabel.Name = "nullValueLabel";
			componentResourceManager.ApplyResources(this.nullValueTextBox, "nullValueTextBox");
			this.nullValueTextBox.Margin = new Padding(0, 3, 0, 3);
			this.nullValueTextBox.Name = "nullValueTextBox";
			this.nullValueTextBox.TextChanged += this.nullValueTextBox_TextChanged;
			componentResourceManager.ApplyResources(this.decimalPlacesUpDown, "decimalPlacesUpDown");
			this.decimalPlacesUpDown.Margin = new Padding(0, 3, 0, 3);
			NumericUpDown numericUpDown = this.decimalPlacesUpDown;
			int[] array = new int[4];
			array[0] = 6;
			numericUpDown.Maximum = new decimal(array);
			NumericUpDown numericUpDown2 = this.decimalPlacesUpDown;
			int[] array2 = new int[4];
			array2[0] = 2;
			numericUpDown2.Value = new decimal(array2);
			this.decimalPlacesUpDown.Name = "decimalPlacesUpDown";
			this.decimalPlacesUpDown.ValueChanged += this.decimalPlacesUpDown_ValueChanged;
			componentResourceManager.ApplyResources(this.thirdRowLabel, "thirdRowLabel");
			this.thirdRowLabel.Name = "thirdRowLabel";
			componentResourceManager.ApplyResources(this.dateTimeFormatsListBox, "dateTimeFormatsListBox");
			this.dateTimeFormatsListBox.FormattingEnabled = true;
			this.dateTimeFormatsListBox.Margin = new Padding(3, 0, 0, 0);
			this.dateTimeFormatsListBox.Name = "dateTimeFormatsListBox";
			componentResourceManager.ApplyResources(this.sampleGroupBox, "sampleGroupBox");
			this.sampleGroupBox.Controls.Add(this.sampleLabel);
			this.sampleGroupBox.MinimumSize = new Size(250, 38);
			this.sampleGroupBox.Name = "sampleGroupBox";
			this.sampleGroupBox.Padding = new Padding(0);
			this.sampleGroupBox.TabStop = false;
			componentResourceManager.ApplyResources(this.sampleLabel, "sampleLabel");
			this.sampleLabel.Name = "sampleLabel";
			componentResourceManager.ApplyResources(this.formatTypeListBox, "formatTypeListBox");
			this.formatTypeListBox.FormattingEnabled = true;
			this.formatTypeListBox.Name = "formatTypeListBox";
			this.formatTypeListBox.SelectedIndexChanged += this.formatTypeListBox_SelectedIndexChanged;
			componentResourceManager.ApplyResources(this.formatTypeLabel, "formatTypeLabel");
			this.formatTypeLabel.Name = "formatTypeLabel";
			componentResourceManager.ApplyResources(this.explanationLabel, "explanationLabel");
			this.explanationLabel.MinimumSize = new Size(0, 30);
			this.explanationLabel.Name = "explanationLabel";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.formatGroupBox);
			this.MinimumSize = new Size(390, 237);
			base.Name = "FormatControl";
			base.Load += this.FormatControl_Load;
			this.formatGroupBox.ResumeLayout(false);
			this.formatGroupBox.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((ISupportInitialize)this.decimalPlacesUpDown).EndInit();
			this.sampleGroupBox.ResumeLayout(false);
			this.sampleGroupBox.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private const int NoFormattingIndex = 0;

		private const int NumericIndex = 1;

		private const int CurrencyIndex = 2;

		private const int DateTimeIndex = 3;

		private const int ScientificIndex = 4;

		private const int CustomIndex = 5;

		private TextBox customStringTextBox = new TextBox();

		private static DateTime dateTimeFormatValue = DateTime.Now;

		private bool dirty;

		private bool loaded;

		private IContainer components;

		private GroupBox formatGroupBox;

		private Label explanationLabel;

		private Label formatTypeLabel;

		private ListBox formatTypeListBox;

		private GroupBox sampleGroupBox;

		private Label sampleLabel;

		private TableLayoutPanel tableLayoutPanel1;

		private Label nullValueLabel;

		private Label secondRowLabel;

		private TextBox nullValueTextBox;

		private Label thirdRowLabel;

		private ListBox dateTimeFormatsListBox;

		private NumericUpDown decimalPlacesUpDown;

		private class DateTimeFormatsListBoxItem
		{
			public DateTimeFormatsListBoxItem(DateTime value, string formatString)
			{
				this.value = value;
				this.formatString = formatString;
			}

			public string FormatString
			{
				get
				{
					return this.formatString;
				}
			}

			public override string ToString()
			{
				return this.value.ToString(this.formatString, CultureInfo.CurrentCulture);
			}

			private DateTime value;

			private string formatString;
		}

		internal abstract class FormatTypeClass
		{
			public abstract string TopLabelString { get; }

			public abstract string SampleString { get; }

			public abstract bool DropDownVisible { get; }

			public abstract bool ListBoxVisible { get; }

			public abstract bool FormatStringTextBoxVisible { get; }

			public abstract bool FormatLabelVisible { get; }

			public abstract string FormatString { get; }

			public abstract bool Parse(string formatString);

			public abstract void PushFormatStringIntoFormatType(string formatString);
		}

		private class NoFormattingFormatType : FormatControl.FormatTypeClass
		{
			public override string TopLabelString
			{
				get
				{
					return SR.GetString("BindingFormattingDialogFormatTypeNoFormattingExplanation");
				}
			}

			public override string SampleString
			{
				get
				{
					return "-1234.5";
				}
			}

			public override bool DropDownVisible
			{
				get
				{
					return false;
				}
			}

			public override bool ListBoxVisible
			{
				get
				{
					return false;
				}
			}

			public override bool FormatLabelVisible
			{
				get
				{
					return false;
				}
			}

			public override string FormatString
			{
				get
				{
					return "";
				}
			}

			public override bool FormatStringTextBoxVisible
			{
				get
				{
					return false;
				}
			}

			public override bool Parse(string formatString)
			{
				return false;
			}

			public override void PushFormatStringIntoFormatType(string formatString)
			{
			}

			public override string ToString()
			{
				return SR.GetString("BindingFormattingDialogFormatTypeNoFormatting");
			}
		}

		private class NumericFormatType : FormatControl.FormatTypeClass
		{
			public NumericFormatType(FormatControl owner)
			{
				this.owner = owner;
			}

			public override string TopLabelString
			{
				get
				{
					return SR.GetString("BindingFormattingDialogFormatTypeNumericExplanation");
				}
			}

			public override string SampleString
			{
				get
				{
					return (-1234.5678).ToString(this.FormatString, CultureInfo.CurrentCulture);
				}
			}

			public override bool DropDownVisible
			{
				get
				{
					return true;
				}
			}

			public override bool ListBoxVisible
			{
				get
				{
					return false;
				}
			}

			public override bool FormatLabelVisible
			{
				get
				{
					return false;
				}
			}

			public override string FormatString
			{
				get
				{
					switch ((int)this.owner.decimalPlacesUpDown.Value)
					{
					case 0:
						return "N0";
					case 1:
						return "N1";
					case 2:
						return "N2";
					case 3:
						return "N3";
					case 4:
						return "N4";
					case 5:
						return "N5";
					case 6:
						return "N6";
					default:
						return "";
					}
				}
			}

			public override bool FormatStringTextBoxVisible
			{
				get
				{
					return false;
				}
			}

			public static bool ParseStatic(string formatString)
			{
				return formatString.Equals("N0") || formatString.Equals("N1") || formatString.Equals("N2") || formatString.Equals("N3") || formatString.Equals("N4") || formatString.Equals("N5") || formatString.Equals("N6");
			}

			public override bool Parse(string formatString)
			{
				return FormatControl.NumericFormatType.ParseStatic(formatString);
			}

			public override void PushFormatStringIntoFormatType(string formatString)
			{
				if (formatString.Equals("N0"))
				{
					this.owner.decimalPlacesUpDown.Value = 0m;
					return;
				}
				if (formatString.Equals("N1"))
				{
					this.owner.decimalPlacesUpDown.Value = 1m;
					return;
				}
				if (formatString.Equals("N2"))
				{
					this.owner.decimalPlacesUpDown.Value = 2m;
					return;
				}
				if (formatString.Equals("N3"))
				{
					this.owner.decimalPlacesUpDown.Value = 3m;
					return;
				}
				if (formatString.Equals("N4"))
				{
					this.owner.decimalPlacesUpDown.Value = 4m;
					return;
				}
				if (formatString.Equals("N5"))
				{
					this.owner.decimalPlacesUpDown.Value = 5m;
					return;
				}
				if (formatString.Equals("N6"))
				{
					this.owner.decimalPlacesUpDown.Value = 6m;
				}
			}

			public override string ToString()
			{
				return SR.GetString("BindingFormattingDialogFormatTypeNumeric");
			}

			private FormatControl owner;
		}

		private class CurrencyFormatType : FormatControl.FormatTypeClass
		{
			public CurrencyFormatType(FormatControl owner)
			{
				this.owner = owner;
			}

			public override string TopLabelString
			{
				get
				{
					return SR.GetString("BindingFormattingDialogFormatTypeCurrencyExplanation");
				}
			}

			public override string SampleString
			{
				get
				{
					return (-1234.5678).ToString(this.FormatString, CultureInfo.CurrentCulture);
				}
			}

			public override bool DropDownVisible
			{
				get
				{
					return true;
				}
			}

			public override bool ListBoxVisible
			{
				get
				{
					return false;
				}
			}

			public override bool FormatLabelVisible
			{
				get
				{
					return false;
				}
			}

			public override string FormatString
			{
				get
				{
					switch ((int)this.owner.decimalPlacesUpDown.Value)
					{
					case 0:
						return "C0";
					case 1:
						return "C1";
					case 2:
						return "C2";
					case 3:
						return "C3";
					case 4:
						return "C4";
					case 5:
						return "C5";
					case 6:
						return "C6";
					default:
						return "";
					}
				}
			}

			public override bool FormatStringTextBoxVisible
			{
				get
				{
					return false;
				}
			}

			public static bool ParseStatic(string formatString)
			{
				return formatString.Equals("C0") || formatString.Equals("C1") || formatString.Equals("C2") || formatString.Equals("C3") || formatString.Equals("C4") || formatString.Equals("C5") || formatString.Equals("C6");
			}

			public override bool Parse(string formatString)
			{
				return FormatControl.CurrencyFormatType.ParseStatic(formatString);
			}

			public override void PushFormatStringIntoFormatType(string formatString)
			{
				if (formatString.Equals("C0"))
				{
					this.owner.decimalPlacesUpDown.Value = 0m;
					return;
				}
				if (formatString.Equals("C1"))
				{
					this.owner.decimalPlacesUpDown.Value = 1m;
					return;
				}
				if (formatString.Equals("C2"))
				{
					this.owner.decimalPlacesUpDown.Value = 2m;
					return;
				}
				if (formatString.Equals("C3"))
				{
					this.owner.decimalPlacesUpDown.Value = 3m;
					return;
				}
				if (formatString.Equals("C4"))
				{
					this.owner.decimalPlacesUpDown.Value = 4m;
					return;
				}
				if (formatString.Equals("C5"))
				{
					this.owner.decimalPlacesUpDown.Value = 5m;
					return;
				}
				if (formatString.Equals("C6"))
				{
					this.owner.decimalPlacesUpDown.Value = 6m;
				}
			}

			public override string ToString()
			{
				return SR.GetString("BindingFormattingDialogFormatTypeCurrency");
			}

			private FormatControl owner;
		}

		private class DateTimeFormatType : FormatControl.FormatTypeClass
		{
			public DateTimeFormatType(FormatControl owner)
			{
				this.owner = owner;
			}

			public override string TopLabelString
			{
				get
				{
					return SR.GetString("BindingFormattingDialogFormatTypeDateTimeExplanation");
				}
			}

			public override string SampleString
			{
				get
				{
					if (this.owner.dateTimeFormatsListBox.SelectedItem == null)
					{
						return "";
					}
					return FormatControl.dateTimeFormatValue.ToString(this.FormatString, CultureInfo.CurrentCulture);
				}
			}

			public override bool DropDownVisible
			{
				get
				{
					return false;
				}
			}

			public override bool ListBoxVisible
			{
				get
				{
					return true;
				}
			}

			public override bool FormatLabelVisible
			{
				get
				{
					return false;
				}
			}

			public override string FormatString
			{
				get
				{
					FormatControl.DateTimeFormatsListBoxItem dateTimeFormatsListBoxItem = this.owner.dateTimeFormatsListBox.SelectedItem as FormatControl.DateTimeFormatsListBoxItem;
					return dateTimeFormatsListBoxItem.FormatString;
				}
			}

			public override bool FormatStringTextBoxVisible
			{
				get
				{
					return false;
				}
			}

			public static bool ParseStatic(string formatString)
			{
				return formatString.Equals("d") || formatString.Equals("D") || formatString.Equals("f") || formatString.Equals("F") || formatString.Equals("g") || formatString.Equals("G") || formatString.Equals("t") || formatString.Equals("T") || formatString.Equals("M");
			}

			public override bool Parse(string formatString)
			{
				return FormatControl.DateTimeFormatType.ParseStatic(formatString);
			}

			public override void PushFormatStringIntoFormatType(string formatString)
			{
				int num = -1;
				if (formatString.Equals("d"))
				{
					num = 0;
				}
				else if (formatString.Equals("D"))
				{
					num = 1;
				}
				else if (formatString.Equals("f"))
				{
					num = 2;
				}
				else if (formatString.Equals("F"))
				{
					num = 3;
				}
				else if (formatString.Equals("g"))
				{
					num = 4;
				}
				else if (formatString.Equals("G"))
				{
					num = 5;
				}
				else if (formatString.Equals("t"))
				{
					num = 6;
				}
				else if (formatString.Equals("T"))
				{
					num = 7;
				}
				else if (formatString.Equals("M"))
				{
					num = 8;
				}
				this.owner.dateTimeFormatsListBox.SelectedIndex = num;
			}

			public override string ToString()
			{
				return SR.GetString("BindingFormattingDialogFormatTypeDateTime");
			}

			private FormatControl owner;
		}

		private class ScientificFormatType : FormatControl.FormatTypeClass
		{
			public ScientificFormatType(FormatControl owner)
			{
				this.owner = owner;
			}

			public override string TopLabelString
			{
				get
				{
					return SR.GetString("BindingFormattingDialogFormatTypeScientificExplanation");
				}
			}

			public override string SampleString
			{
				get
				{
					return (-1234.5678).ToString(this.FormatString, CultureInfo.CurrentCulture);
				}
			}

			public override bool DropDownVisible
			{
				get
				{
					return true;
				}
			}

			public override bool ListBoxVisible
			{
				get
				{
					return false;
				}
			}

			public override bool FormatLabelVisible
			{
				get
				{
					return false;
				}
			}

			public override string FormatString
			{
				get
				{
					switch ((int)this.owner.decimalPlacesUpDown.Value)
					{
					case 0:
						return "E0";
					case 1:
						return "E1";
					case 2:
						return "E2";
					case 3:
						return "E3";
					case 4:
						return "E4";
					case 5:
						return "E5";
					case 6:
						return "E6";
					default:
						return "";
					}
				}
			}

			public override bool FormatStringTextBoxVisible
			{
				get
				{
					return false;
				}
			}

			public static bool ParseStatic(string formatString)
			{
				return formatString.Equals("E0") || formatString.Equals("E1") || formatString.Equals("E2") || formatString.Equals("E3") || formatString.Equals("E4") || formatString.Equals("E5") || formatString.Equals("E6");
			}

			public override bool Parse(string formatString)
			{
				return FormatControl.ScientificFormatType.ParseStatic(formatString);
			}

			public override void PushFormatStringIntoFormatType(string formatString)
			{
				if (formatString.Equals("E0"))
				{
					this.owner.decimalPlacesUpDown.Value = 0m;
					return;
				}
				if (formatString.Equals("E1"))
				{
					this.owner.decimalPlacesUpDown.Value = 1m;
					return;
				}
				if (formatString.Equals("E2"))
				{
					this.owner.decimalPlacesUpDown.Value = 2m;
					return;
				}
				if (formatString.Equals("E3"))
				{
					this.owner.decimalPlacesUpDown.Value = 3m;
					return;
				}
				if (formatString.Equals("E4"))
				{
					this.owner.decimalPlacesUpDown.Value = 4m;
					return;
				}
				if (formatString.Equals("E5"))
				{
					this.owner.decimalPlacesUpDown.Value = 5m;
					return;
				}
				if (formatString.Equals("E6"))
				{
					this.owner.decimalPlacesUpDown.Value = 6m;
				}
			}

			public override string ToString()
			{
				return SR.GetString("BindingFormattingDialogFormatTypeScientific");
			}

			private FormatControl owner;
		}

		private class CustomFormatType : FormatControl.FormatTypeClass
		{
			public CustomFormatType(FormatControl owner)
			{
				this.owner = owner;
			}

			public override string TopLabelString
			{
				get
				{
					return SR.GetString("BindingFormattingDialogFormatTypeCustomExplanation");
				}
			}

			public override string SampleString
			{
				get
				{
					string formatString = this.FormatString;
					if (string.IsNullOrEmpty(formatString))
					{
						return "";
					}
					string text = "";
					if (FormatControl.DateTimeFormatType.ParseStatic(formatString))
					{
						text = FormatControl.dateTimeFormatValue.ToString(formatString, CultureInfo.CurrentCulture);
					}
					if (text.Equals(""))
					{
						try
						{
							text = (-1234.5678).ToString(formatString, CultureInfo.CurrentCulture);
						}
						catch (FormatException)
						{
							text = "";
						}
					}
					if (text.Equals(""))
					{
						try
						{
							text = (-1234).ToString(formatString, CultureInfo.CurrentCulture);
						}
						catch (FormatException)
						{
							text = "";
						}
					}
					if (text.Equals(""))
					{
						try
						{
							text = FormatControl.dateTimeFormatValue.ToString(formatString, CultureInfo.CurrentCulture);
						}
						catch (FormatException)
						{
							text = "";
						}
					}
					if (text.Equals(""))
					{
						text = SR.GetString("BindingFormattingDialogFormatTypeCustomInvalidFormat");
					}
					return text;
				}
			}

			public override bool DropDownVisible
			{
				get
				{
					return false;
				}
			}

			public override bool ListBoxVisible
			{
				get
				{
					return false;
				}
			}

			public override bool FormatStringTextBoxVisible
			{
				get
				{
					return true;
				}
			}

			public override bool FormatLabelVisible
			{
				get
				{
					return false;
				}
			}

			public override string FormatString
			{
				get
				{
					return this.owner.customStringTextBox.Text;
				}
			}

			public static bool ParseStatic(string formatString)
			{
				return true;
			}

			public override bool Parse(string formatString)
			{
				return FormatControl.CustomFormatType.ParseStatic(formatString);
			}

			public override void PushFormatStringIntoFormatType(string formatString)
			{
				this.owner.customStringTextBox.Text = formatString;
			}

			public override string ToString()
			{
				return SR.GetString("BindingFormattingDialogFormatTypeCustom");
			}

			private FormatControl owner;
		}
	}
}
