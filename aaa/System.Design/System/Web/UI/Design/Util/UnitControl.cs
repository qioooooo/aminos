using System;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003CF RID: 975
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class UnitControl : Panel
	{
		// Token: 0x060023C5 RID: 9157 RVA: 0x000BFE7C File Offset: 0x000BEE7C
		public UnitControl()
		{
			this.initMode = true;
			base.Size = new Size(88, 21);
			this.InitControl();
			this.InitUI();
			this.initMode = false;
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x000BFED1 File Offset: 0x000BEED1
		// (set) Token: 0x060023C7 RID: 9159 RVA: 0x000BFEDE File Offset: 0x000BEEDE
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

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x000BFEEC File Offset: 0x000BEEEC
		// (set) Token: 0x060023C9 RID: 9161 RVA: 0x000BFEF4 File Offset: 0x000BEEF4
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

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060023CA RID: 9162 RVA: 0x000BFF5D File Offset: 0x000BEF5D
		// (set) Token: 0x060023CB RID: 9163 RVA: 0x000BFF68 File Offset: 0x000BEF68
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

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x000BFFCF File Offset: 0x000BEFCF
		// (set) Token: 0x060023CD RID: 9165 RVA: 0x000BFFD7 File Offset: 0x000BEFD7
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

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060023CE RID: 9166 RVA: 0x000BFFE0 File Offset: 0x000BEFE0
		// (set) Token: 0x060023CF RID: 9167 RVA: 0x000BFFE8 File Offset: 0x000BEFE8
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

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x000BFFF1 File Offset: 0x000BEFF1
		// (set) Token: 0x060023D1 RID: 9169 RVA: 0x000BFFF9 File Offset: 0x000BEFF9
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

		// Token: 0x060023D2 RID: 9170 RVA: 0x000C0002 File Offset: 0x000BF002
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.valueEdit.Enabled = base.Enabled;
			this.unitCombo.Enabled = base.Enabled;
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060023D3 RID: 9171 RVA: 0x000C002D File Offset: 0x000BF02D
		// (set) Token: 0x060023D4 RID: 9172 RVA: 0x000C0035 File Offset: 0x000BF035
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

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060023D5 RID: 9173 RVA: 0x000C0040 File Offset: 0x000BF040
		// (set) Token: 0x060023D6 RID: 9174 RVA: 0x000C00AC File Offset: 0x000BF0AC
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

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060023D7 RID: 9175 RVA: 0x000C01DB File Offset: 0x000BF1DB
		// (set) Token: 0x060023D8 RID: 9176 RVA: 0x000C01F6 File Offset: 0x000BF1F6
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

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060023D9 RID: 9177 RVA: 0x000C020C File Offset: 0x000BF20C
		// (set) Token: 0x060023DA RID: 9178 RVA: 0x000C0227 File Offset: 0x000BF227
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

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060023DB RID: 9179 RVA: 0x000C023D File Offset: 0x000BF23D
		// (set) Token: 0x060023DC RID: 9180 RVA: 0x000C0258 File Offset: 0x000BF258
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

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060023DD RID: 9181 RVA: 0x000C026E File Offset: 0x000BF26E
		// (set) Token: 0x060023DE RID: 9182 RVA: 0x000C0289 File Offset: 0x000BF289
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

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x060023DF RID: 9183 RVA: 0x000C029F File Offset: 0x000BF29F
		// (remove) Token: 0x060023E0 RID: 9184 RVA: 0x000C02B8 File Offset: 0x000BF2B8
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

		// Token: 0x060023E1 RID: 9185 RVA: 0x000C02D4 File Offset: 0x000BF2D4
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

		// Token: 0x060023E2 RID: 9186 RVA: 0x000C03C8 File Offset: 0x000BF3C8
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

		// Token: 0x060023E3 RID: 9187 RVA: 0x000C0560 File Offset: 0x000BF560
		private void InitUI()
		{
			this.valueEdit.Text = string.Empty;
			this.unitCombo.SelectedIndex = -1;
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x000C057E File Offset: 0x000BF57E
		private void OnChanged(EventArgs e)
		{
			if (this.onChangedHandler != null)
			{
				this.onChangedHandler(this, e);
			}
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x000C0595 File Offset: 0x000BF595
		protected override void OnGotFocus(EventArgs e)
		{
			this.valueEdit.Focus();
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x000C05A4 File Offset: 0x000BF5A4
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

		// Token: 0x060023E7 RID: 9191 RVA: 0x000C0628 File Offset: 0x000BF628
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

		// Token: 0x060023E8 RID: 9192 RVA: 0x000C0676 File Offset: 0x000BF676
		private void OnUnitSelectedIndexChanged(object source, EventArgs e)
		{
			if (this.initMode || this.internalChange)
			{
				return;
			}
			this.OnChanged(EventArgs.Empty);
		}

		// Token: 0x040018B2 RID: 6322
		private const int EDIT_X_SIZE = 44;

		// Token: 0x040018B3 RID: 6323
		private const int COMBO_X_SIZE = 40;

		// Token: 0x040018B4 RID: 6324
		private const int SEPARATOR_X_SIZE = 4;

		// Token: 0x040018B5 RID: 6325
		private const int CTL_Y_SIZE = 21;

		// Token: 0x040018B6 RID: 6326
		public const int UNIT_PX = 0;

		// Token: 0x040018B7 RID: 6327
		public const int UNIT_PT = 1;

		// Token: 0x040018B8 RID: 6328
		public const int UNIT_PC = 2;

		// Token: 0x040018B9 RID: 6329
		public const int UNIT_MM = 3;

		// Token: 0x040018BA RID: 6330
		public const int UNIT_CM = 4;

		// Token: 0x040018BB RID: 6331
		public const int UNIT_IN = 5;

		// Token: 0x040018BC RID: 6332
		public const int UNIT_EM = 6;

		// Token: 0x040018BD RID: 6333
		public const int UNIT_EX = 7;

		// Token: 0x040018BE RID: 6334
		public const int UNIT_PERCENT = 8;

		// Token: 0x040018BF RID: 6335
		public const int UNIT_NONE = 9;

		// Token: 0x040018C0 RID: 6336
		private static readonly string[] UNIT_VALUES = new string[] { "px", "pt", "pc", "mm", "cm", "in", "em", "ex", "%", "" };

		// Token: 0x040018C1 RID: 6337
		private NumberEdit valueEdit;

		// Token: 0x040018C2 RID: 6338
		private ComboBox unitCombo;

		// Token: 0x040018C3 RID: 6339
		private bool allowPercent = true;

		// Token: 0x040018C4 RID: 6340
		private bool allowNonUnit;

		// Token: 0x040018C5 RID: 6341
		private int defaultUnit = 1;

		// Token: 0x040018C6 RID: 6342
		private int minValue;

		// Token: 0x040018C7 RID: 6343
		private int maxValue = 65535;

		// Token: 0x040018C8 RID: 6344
		private bool validateMinMax;

		// Token: 0x040018C9 RID: 6345
		private EventHandler onChangedHandler;

		// Token: 0x040018CA RID: 6346
		private bool initMode;

		// Token: 0x040018CB RID: 6347
		private bool internalChange;

		// Token: 0x040018CC RID: 6348
		private bool valueChanged;
	}
}
