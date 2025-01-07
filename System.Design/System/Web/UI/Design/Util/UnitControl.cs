using System;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class UnitControl : Panel
	{
		public UnitControl()
		{
			this.initMode = true;
			base.Size = new Size(88, 21);
			this.InitControl();
			this.InitUI();
			this.initMode = false;
		}

		public bool AllowNegativeValues
		{
			get
			{
				return this.valueEdit.AllowNegative;
			}
			set
			{
				this.valueEdit.AllowNegative = value;
			}
		}

		public bool AllowNonUnitValues
		{
			get
			{
				return this.allowNonUnit;
			}
			set
			{
				if (value == this.allowNonUnit)
				{
					return;
				}
				if (value && !this.allowPercent)
				{
					throw new Exception();
				}
				this.allowNonUnit = value;
				if (this.allowNonUnit)
				{
					this.unitCombo.Items.Add(UnitControl.UNIT_VALUES[9]);
					return;
				}
				this.unitCombo.Items.Remove(UnitControl.UNIT_VALUES[9]);
			}
		}

		public bool AllowPercentValues
		{
			get
			{
				return this.allowPercent;
			}
			set
			{
				if (value == this.allowPercent)
				{
					return;
				}
				if (!value && this.allowNonUnit)
				{
					throw new Exception();
				}
				this.allowPercent = value;
				if (this.allowPercent)
				{
					this.unitCombo.Items.Add(UnitControl.UNIT_VALUES[8]);
					return;
				}
				this.unitCombo.Items.Remove(UnitControl.UNIT_VALUES[8]);
			}
		}

		public int DefaultUnit
		{
			get
			{
				return this.defaultUnit;
			}
			set
			{
				this.defaultUnit = value;
			}
		}

		public int MaxValue
		{
			get
			{
				return this.maxValue;
			}
			set
			{
				this.maxValue = value;
			}
		}

		public int MinValue
		{
			get
			{
				return this.minValue;
			}
			set
			{
				this.minValue = value;
			}
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.valueEdit.Enabled = base.Enabled;
			this.unitCombo.Enabled = base.Enabled;
		}

		public bool ValidateMinMax
		{
			get
			{
				return this.validateMinMax;
			}
			set
			{
				this.validateMinMax = value;
			}
		}

		public string Value
		{
			get
			{
				string text = this.GetValidatedValue();
				if (text == null)
				{
					text = this.valueEdit.Text;
				}
				else
				{
					this.valueEdit.Text = text;
					this.OnValueTextChanged(this.valueEdit, EventArgs.Empty);
				}
				int selectedIndex = this.unitCombo.SelectedIndex;
				if (text.Length == 0 || selectedIndex == -1)
				{
					return null;
				}
				return text + UnitControl.UNIT_VALUES[selectedIndex];
			}
			set
			{
				this.initMode = true;
				this.InitUI();
				if (value != null)
				{
					string text = value.Trim().ToLower(CultureInfo.InvariantCulture);
					int length = text.Length;
					int num = -1;
					int num2 = -1;
					for (int i = 0; i < length; i++)
					{
						char c = text[i];
						if ((c < '0' || c > '9') && !NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.Contains(c.ToString(CultureInfo.CurrentCulture)) && (!NumberFormatInfo.CurrentInfo.NegativeSign.Contains(c.ToString(CultureInfo.CurrentCulture)) || !this.valueEdit.AllowNegative))
						{
							break;
						}
						num2 = i;
					}
					if (num2 != -1)
					{
						if (num2 + 1 < length)
						{
							int num3 = (this.allowPercent ? 8 : 7);
							string text2 = text.Substring(num2 + 1);
							for (int j = 0; j <= num3; j++)
							{
								if (UnitControl.UNIT_VALUES[j].Equals(text2))
								{
									num = j;
									break;
								}
							}
						}
						else if (this.allowNonUnit)
						{
							num = 9;
						}
						if (num != -1)
						{
							this.valueEdit.Text = text.Substring(0, num2 + 1);
							this.unitCombo.SelectedIndex = num;
						}
					}
				}
				this.initMode = false;
			}
		}

		public string UnitAccessibleName
		{
			get
			{
				if (this.unitCombo != null)
				{
					return this.unitCombo.AccessibleName;
				}
				return string.Empty;
			}
			set
			{
				if (this.unitCombo != null)
				{
					this.unitCombo.AccessibleName = value;
				}
			}
		}

		public string UnitAccessibleDescription
		{
			get
			{
				if (this.unitCombo != null)
				{
					return this.unitCombo.AccessibleDescription;
				}
				return string.Empty;
			}
			set
			{
				if (this.unitCombo != null)
				{
					this.unitCombo.AccessibleDescription = value;
				}
			}
		}

		public string ValueAccessibleName
		{
			get
			{
				if (this.valueEdit != null)
				{
					return this.valueEdit.AccessibleName;
				}
				return string.Empty;
			}
			set
			{
				if (this.valueEdit != null)
				{
					this.valueEdit.AccessibleName = value;
				}
			}
		}

		public string ValueAccessibleDescription
		{
			get
			{
				if (this.valueEdit != null)
				{
					return this.valueEdit.AccessibleDescription;
				}
				return string.Empty;
			}
			set
			{
				if (this.valueEdit != null)
				{
					this.valueEdit.AccessibleDescription = value;
				}
			}
		}

		public event EventHandler Changed
		{
			add
			{
				this.onChangedHandler = (EventHandler)Delegate.Combine(this.onChangedHandler, value);
			}
			remove
			{
				this.onChangedHandler = (EventHandler)Delegate.Remove(this.onChangedHandler, value);
			}
		}

		private string GetValidatedValue()
		{
			string text = null;
			if (this.validateMinMax)
			{
				string text2 = this.valueEdit.Text;
				if (text2.Length != 0)
				{
					try
					{
						if (!text2.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
						{
							int num = int.Parse(text2, CultureInfo.CurrentCulture);
							if (num < this.minValue)
							{
								text = this.minValue.ToString(NumberFormatInfo.CurrentInfo);
							}
							else if (num > this.maxValue)
							{
								text = this.maxValue.ToString(NumberFormatInfo.CurrentInfo);
							}
						}
						else
						{
							float num2 = float.Parse(text2, CultureInfo.CurrentCulture);
							if (num2 < (float)this.minValue)
							{
								text = this.minValue.ToString(NumberFormatInfo.CurrentInfo);
							}
							else if (num2 > (float)this.maxValue)
							{
								text = this.maxValue.ToString(NumberFormatInfo.CurrentInfo);
							}
						}
					}
					catch
					{
						text = this.maxValue.ToString(NumberFormatInfo.CurrentInfo);
					}
				}
			}
			return text;
		}

		private void InitControl()
		{
			int num = base.Width - 44;
			if (num < 0)
			{
				num = 0;
			}
			this.valueEdit = new NumberEdit();
			this.valueEdit.Location = new Point(0, 0);
			this.valueEdit.Size = new Size(num, 21);
			this.valueEdit.TabIndex = 0;
			this.valueEdit.MaxLength = 10;
			this.valueEdit.TextChanged += this.OnValueTextChanged;
			this.valueEdit.LostFocus += this.OnValueLostFocus;
			this.unitCombo = new ComboBox();
			this.unitCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			this.unitCombo.Location = new Point(num + 4, 0);
			this.unitCombo.Size = new Size(40, 21);
			this.unitCombo.TabIndex = 1;
			this.unitCombo.MaxDropDownItems = 9;
			this.unitCombo.SelectedIndexChanged += this.OnUnitSelectedIndexChanged;
			base.Controls.Clear();
			base.Controls.AddRange(new Control[] { this.unitCombo, this.valueEdit });
			for (int i = 0; i <= 7; i++)
			{
				this.unitCombo.Items.Add(UnitControl.UNIT_VALUES[i]);
			}
			if (this.allowPercent)
			{
				this.unitCombo.Items.Add(UnitControl.UNIT_VALUES[8]);
			}
			if (this.allowNonUnit)
			{
				this.unitCombo.Items.Add(UnitControl.UNIT_VALUES[9]);
			}
		}

		private void InitUI()
		{
			this.valueEdit.Text = string.Empty;
			this.unitCombo.SelectedIndex = -1;
		}

		private void OnChanged(EventArgs e)
		{
			if (this.onChangedHandler != null)
			{
				this.onChangedHandler(this, e);
			}
		}

		protected override void OnGotFocus(EventArgs e)
		{
			this.valueEdit.Focus();
		}

		private void OnValueTextChanged(object source, EventArgs e)
		{
			if (this.initMode)
			{
				return;
			}
			string text = this.valueEdit.Text;
			if (text.Length == 0)
			{
				this.internalChange = true;
				this.unitCombo.SelectedIndex = -1;
				this.internalChange = false;
			}
			else if (this.unitCombo.SelectedIndex == -1)
			{
				this.internalChange = true;
				this.unitCombo.SelectedIndex = this.defaultUnit;
				this.internalChange = false;
			}
			this.valueChanged = true;
			this.OnChanged(null);
		}

		private void OnValueLostFocus(object source, EventArgs e)
		{
			if (this.valueChanged)
			{
				string validatedValue = this.GetValidatedValue();
				if (validatedValue != null)
				{
					this.valueEdit.Text = validatedValue;
					this.OnValueTextChanged(this.valueEdit, EventArgs.Empty);
				}
				this.valueChanged = false;
				this.OnChanged(EventArgs.Empty);
			}
		}

		private void OnUnitSelectedIndexChanged(object source, EventArgs e)
		{
			if (this.initMode || this.internalChange)
			{
				return;
			}
			this.OnChanged(EventArgs.Empty);
		}

		private const int EDIT_X_SIZE = 44;

		private const int COMBO_X_SIZE = 40;

		private const int SEPARATOR_X_SIZE = 4;

		private const int CTL_Y_SIZE = 21;

		public const int UNIT_PX = 0;

		public const int UNIT_PT = 1;

		public const int UNIT_PC = 2;

		public const int UNIT_MM = 3;

		public const int UNIT_CM = 4;

		public const int UNIT_IN = 5;

		public const int UNIT_EM = 6;

		public const int UNIT_EX = 7;

		public const int UNIT_PERCENT = 8;

		public const int UNIT_NONE = 9;

		private static readonly string[] UNIT_VALUES = new string[] { "px", "pt", "pc", "mm", "cm", "in", "em", "ex", "%", "" };

		private NumberEdit valueEdit;

		private ComboBox unitCombo;

		private bool allowPercent = true;

		private bool allowNonUnit;

		private int defaultUnit = 1;

		private int minValue;

		private int maxValue = 65535;

		private bool validateMinMax;

		private EventHandler onChangedHandler;

		private bool initMode;

		private bool internalChange;

		private bool valueChanged;
	}
}
