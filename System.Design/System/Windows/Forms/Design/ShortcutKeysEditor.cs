﻿using System;
using System.ComponentModel;
using System.Design;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Windows.Forms.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class ShortcutKeysEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.shortcutKeysUI == null)
					{
						this.shortcutKeysUI = new ShortcutKeysEditor.ShortcutKeysUI(this);
					}
					this.shortcutKeysUI.Start(windowsFormsEditorService, value);
					windowsFormsEditorService.DropDownControl(this.shortcutKeysUI);
					if (this.shortcutKeysUI.Value != null)
					{
						value = this.shortcutKeysUI.Value;
					}
					this.shortcutKeysUI.End();
				}
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		private ShortcutKeysEditor.ShortcutKeysUI shortcutKeysUI;

		private class ShortcutKeysUI : UserControl
		{
			public ShortcutKeysUI(ShortcutKeysEditor editor)
			{
				this.editor = editor;
				this.keysConverter = null;
				this.End();
				this.InitializeComponent();
			}

			public IWindowsFormsEditorService EditorService
			{
				get
				{
					return this.edSvc;
				}
			}

			private TypeConverter KeysConverter
			{
				get
				{
					if (this.keysConverter == null)
					{
						this.keysConverter = TypeDescriptor.GetConverter(typeof(Keys));
					}
					return this.keysConverter;
				}
			}

			public object Value
			{
				get
				{
					if (((Keys)this.currentValue & Keys.KeyCode) == Keys.None)
					{
						return Keys.None;
					}
					return this.currentValue;
				}
			}

			private void btnReset_Click(object sender, EventArgs e)
			{
				this.chkCtrl.Checked = false;
				this.chkAlt.Checked = false;
				this.chkShift.Checked = false;
				this.cmbKey.SelectedIndex = -1;
			}

			private void chkModifier_CheckedChanged(object sender, EventArgs e)
			{
				this.UpdateCurrentValue();
			}

			private void cmbKey_SelectedIndexChanged(object sender, EventArgs e)
			{
				this.UpdateCurrentValue();
			}

			public void End()
			{
				this.edSvc = null;
				this.originalValue = null;
				this.currentValue = null;
				this.updateCurrentValue = false;
				if (this.unknownKeyCode != Keys.None)
				{
					this.cmbKey.Items.RemoveAt(0);
					this.unknownKeyCode = Keys.None;
				}
			}

			private void InitializeComponent()
			{
				ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ShortcutKeysEditor));
				this.tlpOuter = new TableLayoutPanel();
				this.lblModifiers = new Label();
				this.chkCtrl = new CheckBox();
				this.chkAlt = new CheckBox();
				this.chkShift = new CheckBox();
				this.tlpInner = new TableLayoutPanel();
				this.lblKey = new Label();
				this.cmbKey = new ComboBox();
				this.btnReset = new Button();
				this.tlpOuter.SuspendLayout();
				this.tlpInner.SuspendLayout();
				base.SuspendLayout();
				componentResourceManager.ApplyResources(this.tlpOuter, "tlpOuter");
				this.tlpOuter.ColumnCount = 3;
				this.tlpOuter.ColumnStyles.Add(new ColumnStyle());
				this.tlpOuter.ColumnStyles.Add(new ColumnStyle());
				this.tlpOuter.ColumnStyles.Add(new ColumnStyle());
				this.tlpOuter.Controls.Add(this.lblModifiers, 0, 0);
				this.tlpOuter.Controls.Add(this.chkCtrl, 0, 1);
				this.tlpOuter.Controls.Add(this.chkShift, 1, 1);
				this.tlpOuter.Controls.Add(this.chkAlt, 2, 1);
				this.tlpOuter.Name = "tlpOuter";
				this.tlpOuter.RowCount = 2;
				this.tlpOuter.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
				this.tlpOuter.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));
				componentResourceManager.ApplyResources(this.lblModifiers, "lblModifiers");
				this.tlpOuter.SetColumnSpan(this.lblModifiers, 3);
				this.lblModifiers.Name = "lblModifiers";
				componentResourceManager.ApplyResources(this.chkCtrl, "chkCtrl");
				this.chkCtrl.Name = "chkCtrl";
				this.chkCtrl.Margin = new Padding(12, 3, 3, 3);
				this.chkCtrl.CheckedChanged += this.chkModifier_CheckedChanged;
				componentResourceManager.ApplyResources(this.chkAlt, "chkAlt");
				this.chkAlt.Name = "chkAlt";
				this.chkAlt.CheckedChanged += this.chkModifier_CheckedChanged;
				componentResourceManager.ApplyResources(this.chkShift, "chkShift");
				this.chkShift.Name = "chkShift";
				this.chkShift.CheckedChanged += this.chkModifier_CheckedChanged;
				componentResourceManager.ApplyResources(this.tlpInner, "tlpInner");
				this.tlpInner.ColumnCount = 2;
				this.tlpInner.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
				this.tlpInner.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
				this.tlpInner.Controls.Add(this.lblKey, 0, 0);
				this.tlpInner.Controls.Add(this.cmbKey, 0, 1);
				this.tlpInner.Controls.Add(this.btnReset, 1, 1);
				this.tlpInner.Name = "tlpInner";
				this.tlpInner.RowCount = 2;
				this.tlpInner.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
				this.tlpInner.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				componentResourceManager.ApplyResources(this.lblKey, "lblKey");
				this.tlpInner.SetColumnSpan(this.lblKey, 2);
				this.lblKey.Name = "lblKey";
				componentResourceManager.ApplyResources(this.cmbKey, "cmbKey");
				this.cmbKey.DropDownStyle = ComboBoxStyle.DropDownList;
				this.cmbKey.Name = "cmbKey";
				this.cmbKey.Margin = new Padding(9, 4, 3, 3);
				this.cmbKey.Padding = this.cmbKey.Margin;
				foreach (Keys keys in ShortcutKeysEditor.ShortcutKeysUI.validKeys)
				{
					this.cmbKey.Items.Add(this.KeysConverter.ConvertToString(keys));
				}
				this.cmbKey.SelectedIndexChanged += this.cmbKey_SelectedIndexChanged;
				componentResourceManager.ApplyResources(this.btnReset, "btnReset");
				this.btnReset.Name = "btnReset";
				this.btnReset.Click += this.btnReset_Click;
				componentResourceManager.ApplyResources(this, "$this");
				base.Controls.AddRange(new Control[] { this.tlpInner, this.tlpOuter });
				base.Name = "ShortcutKeysUI";
				base.Padding = new Padding(4);
				this.tlpOuter.ResumeLayout(false);
				this.tlpOuter.PerformLayout();
				this.tlpInner.ResumeLayout(false);
				this.tlpInner.PerformLayout();
				base.ResumeLayout(false);
				base.PerformLayout();
			}

			private static bool IsValidKey(Keys keyCode)
			{
				foreach (Keys keys in ShortcutKeysEditor.ShortcutKeysUI.validKeys)
				{
					if (keys == keyCode)
					{
						return true;
					}
				}
				return false;
			}

			protected override void OnGotFocus(EventArgs e)
			{
				base.OnGotFocus(e);
				this.chkCtrl.Focus();
			}

			protected override bool ProcessDialogKey(Keys keyData)
			{
				Keys keys = keyData & Keys.KeyCode;
				Keys keys2 = keyData & Keys.Modifiers;
				Keys keys3 = keys;
				if (keys3 != Keys.Tab)
				{
					if (keys3 != Keys.Escape)
					{
						switch (keys3)
						{
						case Keys.Left:
							if ((keys2 & (Keys.Control | Keys.Alt)) == Keys.None && this.chkCtrl.Focused)
							{
								this.btnReset.Focus();
								return true;
							}
							break;
						case Keys.Right:
							if ((keys2 & (Keys.Control | Keys.Alt)) == Keys.None)
							{
								if (this.chkShift.Focused)
								{
									this.cmbKey.Focus();
									return true;
								}
								if (this.btnReset.Focused)
								{
									this.chkCtrl.Focus();
									return true;
								}
							}
							break;
						}
					}
					else if (!this.cmbKey.Focused || (keys2 & (Keys.Control | Keys.Alt)) != Keys.None || !this.cmbKey.DroppedDown)
					{
						this.currentValue = this.originalValue;
					}
				}
				else if (keys2 == Keys.Shift && this.chkCtrl.Focused)
				{
					this.btnReset.Focus();
					return true;
				}
				return base.ProcessDialogKey(keyData);
			}

			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.currentValue = value;
				this.originalValue = value;
				Keys keys = (Keys)value;
				this.chkCtrl.Checked = (keys & Keys.Control) != Keys.None;
				this.chkAlt.Checked = (keys & Keys.Alt) != Keys.None;
				this.chkShift.Checked = (keys & Keys.Shift) != Keys.None;
				Keys keys2 = keys & Keys.KeyCode;
				if (keys2 == Keys.None)
				{
					this.cmbKey.SelectedIndex = -1;
				}
				else if (ShortcutKeysEditor.ShortcutKeysUI.IsValidKey(keys2))
				{
					this.cmbKey.SelectedItem = this.KeysConverter.ConvertToString(keys2);
				}
				else
				{
					this.cmbKey.Items.Insert(0, SR.GetString("ShortcutKeys_InvalidKey"));
					this.cmbKey.SelectedIndex = 0;
					this.unknownKeyCode = keys2;
				}
				this.updateCurrentValue = true;
			}

			private void UpdateCurrentValue()
			{
				if (!this.updateCurrentValue)
				{
					return;
				}
				int selectedIndex = this.cmbKey.SelectedIndex;
				Keys keys = Keys.None;
				if (this.chkCtrl.Checked)
				{
					keys |= Keys.Control;
				}
				if (this.chkAlt.Checked)
				{
					keys |= Keys.Alt;
				}
				if (this.chkShift.Checked)
				{
					keys |= Keys.Shift;
				}
				if (this.unknownKeyCode != Keys.None && selectedIndex == 0)
				{
					keys |= this.unknownKeyCode;
				}
				else if (selectedIndex != -1)
				{
					keys |= ShortcutKeysEditor.ShortcutKeysUI.validKeys[(this.unknownKeyCode == Keys.None) ? selectedIndex : (selectedIndex - 1)];
				}
				this.currentValue = keys;
			}

			private ShortcutKeysEditor editor;

			private IWindowsFormsEditorService edSvc;

			private object originalValue;

			private object currentValue;

			private TypeConverter keysConverter;

			private Keys unknownKeyCode;

			private bool updateCurrentValue;

			private TableLayoutPanel tlpOuter;

			private TableLayoutPanel tlpInner;

			private Label lblModifiers;

			private Label lblKey;

			private CheckBox chkCtrl;

			private CheckBox chkAlt;

			private CheckBox chkShift;

			private ComboBox cmbKey;

			private Button btnReset;

			private static Keys[] validKeys = new Keys[]
			{
				Keys.A,
				Keys.B,
				Keys.C,
				Keys.D,
				Keys.D0,
				Keys.D1,
				Keys.D2,
				Keys.D3,
				Keys.D4,
				Keys.D5,
				Keys.D6,
				Keys.D7,
				Keys.D8,
				Keys.D9,
				Keys.Delete,
				Keys.Down,
				Keys.E,
				Keys.End,
				Keys.F,
				Keys.F1,
				Keys.F10,
				Keys.F11,
				Keys.F12,
				Keys.F13,
				Keys.F14,
				Keys.F15,
				Keys.F16,
				Keys.F17,
				Keys.F18,
				Keys.F19,
				Keys.F2,
				Keys.F20,
				Keys.F21,
				Keys.F22,
				Keys.F23,
				Keys.F24,
				Keys.F3,
				Keys.F4,
				Keys.F5,
				Keys.F6,
				Keys.F7,
				Keys.F8,
				Keys.F9,
				Keys.G,
				Keys.H,
				Keys.I,
				Keys.Insert,
				Keys.J,
				Keys.K,
				Keys.L,
				Keys.Left,
				Keys.M,
				Keys.N,
				Keys.NumLock,
				Keys.NumPad0,
				Keys.NumPad1,
				Keys.NumPad2,
				Keys.NumPad3,
				Keys.NumPad4,
				Keys.NumPad5,
				Keys.NumPad6,
				Keys.NumPad7,
				Keys.NumPad8,
				Keys.NumPad9,
				Keys.O,
				Keys.OemBackslash,
				Keys.OemClear,
				Keys.OemCloseBrackets,
				Keys.Oemcomma,
				Keys.OemMinus,
				Keys.OemOpenBrackets,
				Keys.OemPeriod,
				Keys.OemPipe,
				Keys.Oemplus,
				Keys.OemQuestion,
				Keys.OemQuotes,
				Keys.OemSemicolon,
				Keys.Oemtilde,
				Keys.P,
				Keys.Pause,
				Keys.Q,
				Keys.R,
				Keys.Right,
				Keys.S,
				Keys.Space,
				Keys.T,
				Keys.Tab,
				Keys.U,
				Keys.Up,
				Keys.V,
				Keys.W,
				Keys.X,
				Keys.Y,
				Keys.Z
			};
		}
	}
}