using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000490 RID: 1168
	public class ParameterEditorUserControl : UserControl
	{
		// Token: 0x06002A59 RID: 10841 RVA: 0x000E8F8D File Offset: 0x000E7F8D
		public ParameterEditorUserControl(IServiceProvider serviceProvider)
			: this(serviceProvider, null)
		{
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x000E8F98 File Offset: 0x000E7F98
		internal ParameterEditorUserControl(IServiceProvider serviceProvider, Control control)
		{
			this._serviceProvider = serviceProvider;
			this._control = control;
			this.InitializeComponent();
			this.InitializeUI();
			this.InitializeParameterEditors();
			this.CreateParameterList();
			foreach (object obj in this._parameterTypes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				this._parameterTypeComboBox.Items.Add(dictionaryEntry.Value);
			}
			this._parameterTypeComboBox.InvalidateDropDownWidth();
			this.UpdateUI(false);
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06002A5B RID: 10843 RVA: 0x000E9040 File Offset: 0x000E8040
		public bool ParametersConfigured
		{
			get
			{
				foreach (object obj in this._parametersListView.Items)
				{
					ParameterEditorUserControl.ParameterListViewItem parameterListViewItem = (ParameterEditorUserControl.ParameterListViewItem)obj;
					if (parameterListViewItem != null && !parameterListViewItem.IsConfigured)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06002A5C RID: 10844 RVA: 0x000E90AC File Offset: 0x000E80AC
		// (remove) Token: 0x06002A5D RID: 10845 RVA: 0x000E90BF File Offset: 0x000E80BF
		public event EventHandler ParametersChanged
		{
			add
			{
				base.Events.AddHandler(ParameterEditorUserControl.EventParametersChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ParameterEditorUserControl.EventParametersChanged, value);
			}
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000E90D4 File Offset: 0x000E80D4
		private void CreateParameterList()
		{
			this._parameterTypes = new ListDictionary();
			this._parameterTypes.Add(typeof(Parameter), "None");
			this._parameterTypes.Add(typeof(CookieParameter), "Cookie");
			this._parameterTypes.Add(typeof(ControlParameter), "Control");
			this._parameterTypes.Add(typeof(FormParameter), "Form");
			this._parameterTypes.Add(typeof(ProfileParameter), "Profile");
			this._parameterTypes.Add(typeof(QueryStringParameter), "QueryString");
			this._parameterTypes.Add(typeof(SessionParameter), "Session");
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000E91A4 File Offset: 0x000E81A4
		private void InitializeComponent()
		{
			this._addButtonPanel = new global::System.Windows.Forms.Panel();
			this._addParameterButton = new global::System.Windows.Forms.Button();
			this._parametersLabel = new global::System.Windows.Forms.Label();
			this._sourceLabel = new global::System.Windows.Forms.Label();
			this._parametersListView = new ListView();
			this._nameColumnHeader = new ColumnHeader("");
			this._valueColumnHeader = new ColumnHeader("");
			this._parameterTypeComboBox = new AutoSizeComboBox();
			this._moveUpButton = new global::System.Windows.Forms.Button();
			this._moveDownButton = new global::System.Windows.Forms.Button();
			this._deleteParameterButton = new global::System.Windows.Forms.Button();
			this._editorPanel = new global::System.Windows.Forms.Panel();
			this._addButtonPanel.SuspendLayout();
			base.SuspendLayout();
			this._parametersLabel.Location = new Point(0, 0);
			this._parametersLabel.Name = "_parametersLabel";
			this._parametersLabel.Size = new Size(252, 16);
			this._parametersLabel.TabIndex = 10;
			this._parametersListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			this._parametersListView.Columns.AddRange(new ColumnHeader[] { this._nameColumnHeader, this._valueColumnHeader });
			this._parametersListView.FullRowSelect = true;
			this._parametersListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this._parametersListView.HideSelection = false;
			this._parametersListView.LabelEdit = true;
			this._parametersListView.Location = new Point(0, 18);
			this._parametersListView.MultiSelect = false;
			this._parametersListView.Name = "_parametersListView";
			this._parametersListView.Size = new Size(252, 234);
			this._parametersListView.TabIndex = 20;
			this._parametersListView.View = global::System.Windows.Forms.View.Details;
			this._parametersListView.SelectedIndexChanged += this.OnParametersListViewSelectedIndexChanged;
			this._parametersListView.AfterLabelEdit += this.OnParametersListViewAfterLabelEdit;
			this._nameColumnHeader.Width = 85;
			this._valueColumnHeader.Width = 134;
			this._addButtonPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this._addButtonPanel.Controls.Add(this._addParameterButton);
			this._addButtonPanel.Location = new Point(0, 258);
			this._addButtonPanel.Name = "_addButtonPanel";
			this._addButtonPanel.Size = new Size(252, 23);
			this._addButtonPanel.TabIndex = 30;
			this._addParameterButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this._addParameterButton.AutoSize = true;
			this._addParameterButton.Location = new Point(124, 0);
			this._addParameterButton.Name = "_addParameterButton";
			this._addParameterButton.Size = new Size(128, 23);
			this._addParameterButton.TabIndex = 10;
			this._addParameterButton.Click += this.OnAddParameterButtonClick;
			this._moveUpButton.Location = new Point(258, 18);
			this._moveUpButton.Name = "_moveUpButton";
			this._moveUpButton.Size = new Size(26, 23);
			this._moveUpButton.TabIndex = 40;
			this._moveUpButton.Click += this.OnMoveUpButtonClick;
			this._moveDownButton.Location = new Point(258, 42);
			this._moveDownButton.Name = "_moveDownButton";
			this._moveDownButton.Size = new Size(26, 23);
			this._moveDownButton.TabIndex = 50;
			this._moveDownButton.Click += this.OnMoveDownButtonClick;
			this._deleteParameterButton.Location = new Point(258, 71);
			this._deleteParameterButton.Name = "_deleteParameterButton";
			this._deleteParameterButton.Size = new Size(26, 23);
			this._deleteParameterButton.TabIndex = 60;
			this._deleteParameterButton.Click += this.OnDeleteParameterButtonClick;
			this._sourceLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._sourceLabel.Location = new Point(292, 0);
			this._sourceLabel.Name = "_sourceLabel";
			this._sourceLabel.Size = new Size(300, 16);
			this._sourceLabel.TabIndex = 70;
			this._parameterTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this._parameterTypeComboBox.Location = new Point(292, 18);
			this._parameterTypeComboBox.Name = "_parameterTypeComboBox";
			this._parameterTypeComboBox.Size = new Size(163, 21);
			this._parameterTypeComboBox.TabIndex = 80;
			this._parameterTypeComboBox.SelectedIndexChanged += this.OnParameterTypeComboBoxSelectedIndexChanged;
			this._editorPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._editorPanel.Location = new Point(292, 47);
			this._editorPanel.Name = "_editorPanel";
			this._editorPanel.Size = new Size(308, 235);
			this._editorPanel.TabIndex = 90;
			base.Controls.Add(this._editorPanel);
			base.Controls.Add(this._addButtonPanel);
			base.Controls.Add(this._deleteParameterButton);
			base.Controls.Add(this._moveDownButton);
			base.Controls.Add(this._moveUpButton);
			base.Controls.Add(this._parameterTypeComboBox);
			base.Controls.Add(this._parametersListView);
			base.Controls.Add(this._sourceLabel);
			base.Controls.Add(this._parametersLabel);
			this.MinimumSize = new Size(460, 126);
			base.Name = "ParameterEditorUserControl";
			base.Size = new Size(600, 280);
			this._addButtonPanel.ResumeLayout(false);
			this._addButtonPanel.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000E97B4 File Offset: 0x000E87B4
		private void InitializeParameterEditors()
		{
			this._advancedParameterEditor = new ParameterEditorUserControl.AdvancedParameterEditor(this._serviceProvider, this._control);
			this._advancedParameterEditor.RequestModeChange += this.ToggleAdvancedMode;
			this._advancedParameterEditor.ParameterChanged += this.OnParametersChanged;
			this._advancedParameterEditor.Visible = false;
			this._editorPanel.Controls.Add(this._advancedParameterEditor);
			this._staticParameterEditor = new ParameterEditorUserControl.StaticParameterEditor(this._serviceProvider);
			this._staticParameterEditor.RequestModeChange += this.ToggleAdvancedMode;
			this._staticParameterEditor.ParameterChanged += this.OnParametersChanged;
			this._staticParameterEditor.Visible = false;
			this._editorPanel.Controls.Add(this._staticParameterEditor);
			this._controlParameterEditor = new ParameterEditorUserControl.ControlParameterEditor(this._serviceProvider, this._control);
			this._controlParameterEditor.RequestModeChange += this.ToggleAdvancedMode;
			this._controlParameterEditor.ParameterChanged += this.OnParametersChanged;
			this._controlParameterEditor.Visible = false;
			this._editorPanel.Controls.Add(this._controlParameterEditor);
			this._formParameterEditor = new ParameterEditorUserControl.FormParameterEditor(this._serviceProvider);
			this._formParameterEditor.RequestModeChange += this.ToggleAdvancedMode;
			this._formParameterEditor.ParameterChanged += this.OnParametersChanged;
			this._formParameterEditor.Visible = false;
			this._editorPanel.Controls.Add(this._formParameterEditor);
			this._queryStringParameterEditor = new ParameterEditorUserControl.QueryStringParameterEditor(this._serviceProvider);
			this._queryStringParameterEditor.RequestModeChange += this.ToggleAdvancedMode;
			this._queryStringParameterEditor.ParameterChanged += this.OnParametersChanged;
			this._queryStringParameterEditor.Visible = false;
			this._editorPanel.Controls.Add(this._queryStringParameterEditor);
			this._cookieParameterEditor = new ParameterEditorUserControl.CookieParameterEditor(this._serviceProvider);
			this._cookieParameterEditor.RequestModeChange += this.ToggleAdvancedMode;
			this._cookieParameterEditor.ParameterChanged += this.OnParametersChanged;
			this._cookieParameterEditor.Visible = false;
			this._editorPanel.Controls.Add(this._cookieParameterEditor);
			this._sessionParameterEditor = new ParameterEditorUserControl.SessionParameterEditor(this._serviceProvider);
			this._sessionParameterEditor.RequestModeChange += this.ToggleAdvancedMode;
			this._sessionParameterEditor.ParameterChanged += this.OnParametersChanged;
			this._sessionParameterEditor.Visible = false;
			this._editorPanel.Controls.Add(this._sessionParameterEditor);
			this._profileParameterEditor = new ParameterEditorUserControl.ProfileParameterEditor(this._serviceProvider);
			this._profileParameterEditor.RequestModeChange += this.ToggleAdvancedMode;
			this._profileParameterEditor.ParameterChanged += this.OnParametersChanged;
			this._profileParameterEditor.Visible = false;
			this._editorPanel.Controls.Add(this._profileParameterEditor);
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000E9AE0 File Offset: 0x000E8AE0
		private void InitializeUI()
		{
			this._parametersLabel.Text = SR.GetString("ParameterEditorUserControl_ParametersLabel");
			this._nameColumnHeader.Text = SR.GetString("ParameterEditorUserControl_ParameterNameColumnHeader");
			this._valueColumnHeader.Text = SR.GetString("ParameterEditorUserControl_ParameterValueColumnHeader");
			this._addParameterButton.Text = SR.GetString("ParameterEditorUserControl_AddButton");
			this._sourceLabel.Text = SR.GetString("ParameterEditorUserControl_SourceLabel");
			Icon icon = new Icon(typeof(ParameterEditorUserControl), "SortUp.ico");
			Bitmap bitmap = icon.ToBitmap();
			bitmap.MakeTransparent();
			this._moveUpButton.Image = bitmap;
			Icon icon2 = new Icon(typeof(ParameterEditorUserControl), "SortDown.ico");
			Bitmap bitmap2 = icon2.ToBitmap();
			bitmap2.MakeTransparent();
			this._moveDownButton.Image = bitmap2;
			Icon icon3 = new Icon(typeof(ParameterEditorUserControl), "Delete.ico");
			Bitmap bitmap3 = icon3.ToBitmap();
			bitmap3.MakeTransparent();
			this._deleteParameterButton.Image = bitmap3;
			this._moveUpButton.AccessibleName = SR.GetString("ParameterEditorUserControl_MoveParameterUp");
			this._moveDownButton.AccessibleName = SR.GetString("ParameterEditorUserControl_MoveParameterDown");
			this._deleteParameterButton.AccessibleName = SR.GetString("ParameterEditorUserControl_DeleteParameter");
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000E9C24 File Offset: 0x000E8C24
		private void AddParameter(Parameter parameter)
		{
			try
			{
				this.IgnoreParameterChanges(true);
				ParameterEditorUserControl.ParameterListViewItem parameterListViewItem = new ParameterEditorUserControl.ParameterListViewItem(parameter);
				this._parametersListView.BeginUpdate();
				try
				{
					this._parametersListView.Items.Add(parameterListViewItem);
					parameterListViewItem.Selected = true;
					parameterListViewItem.Focused = true;
					parameterListViewItem.EnsureVisible();
					this._parametersListView.Focus();
				}
				finally
				{
					this._parametersListView.EndUpdate();
				}
				parameterListViewItem.Refresh();
				parameterListViewItem.BeginEdit();
			}
			finally
			{
				this.IgnoreParameterChanges(false);
			}
			this.OnParametersChanged(this, EventArgs.Empty);
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000E9CC8 File Offset: 0x000E8CC8
		public void AddParameters(Parameter[] parameters)
		{
			try
			{
				this.IgnoreParameterChanges(true);
				this._parametersListView.BeginUpdate();
				ArrayList arrayList = new ArrayList();
				try
				{
					foreach (Parameter parameter in parameters)
					{
						ParameterEditorUserControl.ParameterListViewItem parameterListViewItem = new ParameterEditorUserControl.ParameterListViewItem(parameter);
						this._parametersListView.Items.Add(parameterListViewItem);
						arrayList.Add(parameterListViewItem);
					}
					if (this._parametersListView.Items.Count > 0)
					{
						this._parametersListView.Items[0].Selected = true;
						this._parametersListView.Items[0].Focused = true;
						this._parametersListView.Items[0].EnsureVisible();
					}
					this._parametersListView.Focus();
				}
				finally
				{
					this._parametersListView.EndUpdate();
				}
				foreach (object obj in arrayList)
				{
					ParameterEditorUserControl.ParameterListViewItem parameterListViewItem2 = (ParameterEditorUserControl.ParameterListViewItem)obj;
					parameterListViewItem2.Refresh();
				}
			}
			finally
			{
				this.IgnoreParameterChanges(false);
			}
			this.OnParametersChanged(this, EventArgs.Empty);
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000E9E3C File Offset: 0x000E8E3C
		public void ClearParameters()
		{
			try
			{
				this.IgnoreParameterChanges(true);
				this._parametersListView.Items.Clear();
				this.UpdateUI(false);
			}
			finally
			{
				this.IgnoreParameterChanges(false);
			}
			this.OnParametersChanged(this, EventArgs.Empty);
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x000E9E90 File Offset: 0x000E8E90
		internal static string GetControlDefaultValuePropertyName(string controlID, IServiceProvider serviceProvider, Control control)
		{
			Control control2 = ControlHelper.FindControl(serviceProvider, control, controlID);
			if (control2 != null)
			{
				return ParameterEditorUserControl.GetDefaultValuePropertyName(control2);
			}
			return string.Empty;
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x000E9EB8 File Offset: 0x000E8EB8
		private static string GetDefaultValuePropertyName(Control control)
		{
			ControlValuePropertyAttribute controlValuePropertyAttribute = (ControlValuePropertyAttribute)TypeDescriptor.GetAttributes(control)[typeof(ControlValuePropertyAttribute)];
			if (controlValuePropertyAttribute != null && !string.IsNullOrEmpty(controlValuePropertyAttribute.Name))
			{
				return controlValuePropertyAttribute.Name;
			}
			return string.Empty;
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x000E9EFC File Offset: 0x000E8EFC
		internal static string GetParameterExpression(IServiceProvider serviceProvider, Parameter p, Control control, out bool isHelperText)
		{
			if (p.GetType() == typeof(ControlParameter))
			{
				ControlParameter controlParameter = (ControlParameter)p;
				if (controlParameter.ControlID.Length == 0)
				{
					isHelperText = true;
					return SR.GetString("ParameterEditorUserControl_ControlParameterExpressionUnknown");
				}
				string text = controlParameter.PropertyName;
				if (text.Length == 0)
				{
					text = ParameterEditorUserControl.GetControlDefaultValuePropertyName(controlParameter.ControlID, serviceProvider, control);
				}
				if (text.Length > 0)
				{
					isHelperText = false;
					return controlParameter.ControlID + "." + text;
				}
				isHelperText = true;
				return SR.GetString("ParameterEditorUserControl_ControlParameterExpressionUnknown");
			}
			else if (p.GetType() == typeof(FormParameter))
			{
				FormParameter formParameter = (FormParameter)p;
				if (formParameter.FormField.Length > 0)
				{
					isHelperText = false;
					return string.Format(CultureInfo.InvariantCulture, "Request.Form(\"{0}\")", new object[] { formParameter.FormField });
				}
				isHelperText = true;
				return SR.GetString("ParameterEditorUserControl_FormParameterExpressionUnknown");
			}
			else if (p.GetType() == typeof(QueryStringParameter))
			{
				QueryStringParameter queryStringParameter = (QueryStringParameter)p;
				if (queryStringParameter.QueryStringField.Length > 0)
				{
					isHelperText = false;
					return string.Format(CultureInfo.InvariantCulture, "Request.QueryString(\"{0}\")", new object[] { queryStringParameter.QueryStringField });
				}
				isHelperText = true;
				return SR.GetString("ParameterEditorUserControl_QueryStringParameterExpressionUnknown");
			}
			else if (p.GetType() == typeof(CookieParameter))
			{
				CookieParameter cookieParameter = (CookieParameter)p;
				if (cookieParameter.CookieName.Length > 0)
				{
					isHelperText = false;
					return string.Format(CultureInfo.InvariantCulture, "Request.Cookies(\"{0}\").Value", new object[] { cookieParameter.CookieName });
				}
				isHelperText = true;
				return SR.GetString("ParameterEditorUserControl_CookieParameterExpressionUnknown");
			}
			else if (p.GetType() == typeof(SessionParameter))
			{
				SessionParameter sessionParameter = (SessionParameter)p;
				if (sessionParameter.SessionField.Length > 0)
				{
					isHelperText = false;
					return string.Format(CultureInfo.InvariantCulture, "Session(\"{0}\")", new object[] { sessionParameter.SessionField });
				}
				isHelperText = true;
				return SR.GetString("ParameterEditorUserControl_SessionParameterExpressionUnknown");
			}
			else if (p.GetType() == typeof(ProfileParameter))
			{
				ProfileParameter profileParameter = (ProfileParameter)p;
				if (profileParameter.PropertyName.Length > 0)
				{
					isHelperText = false;
					return string.Format(CultureInfo.InvariantCulture, "Profile(\"{0}\")", new object[] { profileParameter.PropertyName });
				}
				isHelperText = true;
				return SR.GetString("ParameterEditorUserControl_ProfileParameterExpressionUnknown");
			}
			else
			{
				if (p.GetType() != typeof(Parameter))
				{
					isHelperText = true;
					return p.GetType().Name;
				}
				if (p.DefaultValue == null)
				{
					isHelperText = false;
					return string.Empty;
				}
				isHelperText = false;
				return p.DefaultValue;
			}
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x000EA19C File Offset: 0x000E919C
		public Parameter[] GetParameters()
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this._parametersListView.Items)
			{
				ParameterEditorUserControl.ParameterListViewItem parameterListViewItem = (ParameterEditorUserControl.ParameterListViewItem)obj;
				if (parameterListViewItem.Parameter != null)
				{
					arrayList.Add(parameterListViewItem.Parameter);
				}
			}
			return (Parameter[])arrayList.ToArray(typeof(Parameter));
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x000EA224 File Offset: 0x000E9224
		private void IgnoreParameterChanges(bool ignoreChanges)
		{
			this._ignoreParameterChangesCount += (ignoreChanges ? 1 : (-1));
			if (this._ignoreParameterChangesCount == 0)
			{
				this.UpdateUI(false);
			}
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x000EA249 File Offset: 0x000E9249
		private void OnAddParameterButtonClick(object sender, EventArgs e)
		{
			this.AddParameter(new Parameter("newparameter"));
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x000EA25C File Offset: 0x000E925C
		private void OnDeleteParameterButtonClick(object sender, EventArgs e)
		{
			try
			{
				this.IgnoreParameterChanges(true);
				if (this._parametersListView.SelectedItems.Count == 0)
				{
					return;
				}
				int num = this._parametersListView.SelectedIndices[0];
				this._parametersListView.BeginUpdate();
				try
				{
					this._parametersListView.Items.RemoveAt(num);
					if (num < this._parametersListView.Items.Count)
					{
						this._parametersListView.Items[num].Selected = true;
						this._parametersListView.Items[num].Focused = true;
						this._parametersListView.Items[num].EnsureVisible();
						this._parametersListView.Focus();
					}
					else if (this._parametersListView.Items.Count > 0)
					{
						num = this._parametersListView.Items.Count - 1;
						this._parametersListView.Items[num].Selected = true;
						this._parametersListView.Items[num].Focused = true;
						this._parametersListView.Items[num].EnsureVisible();
						this._parametersListView.Focus();
					}
				}
				finally
				{
					this._parametersListView.EndUpdate();
				}
				this.UpdateUI(false);
			}
			finally
			{
				this.IgnoreParameterChanges(false);
			}
			this.OnParametersChanged(this, EventArgs.Empty);
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x000EA3F0 File Offset: 0x000E93F0
		private void OnMoveDownButtonClick(object sender, EventArgs e)
		{
			try
			{
				this.IgnoreParameterChanges(true);
				if (this._parametersListView.SelectedItems.Count == 0)
				{
					return;
				}
				int num = this._parametersListView.SelectedIndices[0];
				if (num == this._parametersListView.Items.Count - 1)
				{
					return;
				}
				this._parametersListView.BeginUpdate();
				try
				{
					ListViewItem listViewItem = this._parametersListView.Items[num];
					listViewItem.Remove();
					this._parametersListView.Items.Insert(num + 1, listViewItem);
					listViewItem.Selected = true;
					listViewItem.Focused = true;
					listViewItem.EnsureVisible();
					this._parametersListView.Focus();
				}
				finally
				{
					this._parametersListView.EndUpdate();
				}
			}
			finally
			{
				this.IgnoreParameterChanges(false);
			}
			this.OnParametersChanged(this, EventArgs.Empty);
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x000EA4DC File Offset: 0x000E94DC
		private void OnMoveUpButtonClick(object sender, EventArgs e)
		{
			try
			{
				this.IgnoreParameterChanges(true);
				if (this._parametersListView.SelectedItems.Count == 0)
				{
					return;
				}
				int num = this._parametersListView.SelectedIndices[0];
				if (num == 0)
				{
					return;
				}
				this._parametersListView.BeginUpdate();
				try
				{
					ListViewItem listViewItem = this._parametersListView.Items[num];
					listViewItem.Remove();
					this._parametersListView.Items.Insert(num - 1, listViewItem);
					listViewItem.Selected = true;
					listViewItem.Focused = true;
					listViewItem.EnsureVisible();
					this._parametersListView.Focus();
				}
				finally
				{
					this._parametersListView.EndUpdate();
				}
			}
			finally
			{
				this.IgnoreParameterChanges(false);
			}
			this.OnParametersChanged(this, EventArgs.Empty);
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x000EA5B8 File Offset: 0x000E95B8
		protected virtual void OnParametersChanged(object sender, EventArgs e)
		{
			if (this._ignoreParameterChangesCount > 0)
			{
				return;
			}
			EventHandler eventHandler = base.Events[ParameterEditorUserControl.EventParametersChanged] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x000EA5F4 File Offset: 0x000E95F4
		private void OnParametersListViewAfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if (e.Label == null || e.Label.Trim().Length == 0)
			{
				e.CancelEdit = true;
				return;
			}
			ParameterEditorUserControl.ParameterListViewItem parameterListViewItem = (ParameterEditorUserControl.ParameterListViewItem)this._parametersListView.Items[e.Item];
			parameterListViewItem.ParameterName = e.Label;
			this.UpdateUI(false);
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x000EA652 File Offset: 0x000E9652
		private void OnParametersListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateUI(false);
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x000EA65C File Offset: 0x000E965C
		private void OnParameterTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				this.IgnoreParameterChanges(true);
				if (this._parametersListView.SelectedItems.Count == 0)
				{
					return;
				}
				ParameterEditorUserControl.ParameterListViewItem parameterListViewItem = (ParameterEditorUserControl.ParameterListViewItem)this._parametersListView.SelectedItems[0];
				string text = (string)this._parameterTypeComboBox.SelectedItem;
				Type type = null;
				foreach (object obj in this._parameterTypes)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if ((string)dictionaryEntry.Value == text)
					{
						type = (Type)dictionaryEntry.Key;
					}
				}
				if (type != null && (parameterListViewItem.Parameter == null || parameterListViewItem.Parameter.GetType() != type))
				{
					parameterListViewItem.Parameter = (Parameter)Activator.CreateInstance(type);
					parameterListViewItem.Refresh();
				}
				this.SetActiveEditParameterItem(parameterListViewItem, false);
			}
			finally
			{
				this.IgnoreParameterChanges(false);
			}
			this.OnParametersChanged(this, EventArgs.Empty);
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x000EA778 File Offset: 0x000E9778
		private void SetActiveEditParameterItem(ParameterEditorUserControl.ParameterListViewItem parameterItem, bool allowFocusChange)
		{
			if (parameterItem == null)
			{
				if (this._parameterEditor != null)
				{
					this._parameterEditor.Visible = false;
					this._parameterEditor = null;
					return;
				}
			}
			else
			{
				ParameterEditorUserControl.ParameterEditor parameterEditor = null;
				if (this._inAdvancedMode)
				{
					parameterEditor = this._advancedParameterEditor;
				}
				else if (parameterItem.Parameter != null)
				{
					if (parameterItem.Parameter.GetType() == typeof(Parameter))
					{
						parameterEditor = this._staticParameterEditor;
					}
					else if (parameterItem.Parameter.GetType() == typeof(ControlParameter))
					{
						parameterEditor = this._controlParameterEditor;
					}
					else if (parameterItem.Parameter.GetType() == typeof(FormParameter))
					{
						parameterEditor = this._formParameterEditor;
					}
					else if (parameterItem.Parameter.GetType() == typeof(QueryStringParameter))
					{
						parameterEditor = this._queryStringParameterEditor;
					}
					else if (parameterItem.Parameter.GetType() == typeof(CookieParameter))
					{
						parameterEditor = this._cookieParameterEditor;
					}
					else if (parameterItem.Parameter.GetType() == typeof(SessionParameter))
					{
						parameterEditor = this._sessionParameterEditor;
					}
					else if (parameterItem.Parameter.GetType() == typeof(ProfileParameter))
					{
						parameterEditor = this._profileParameterEditor;
					}
				}
				if (this._parameterEditor != parameterEditor)
				{
					if (this._parameterEditor != null)
					{
						this._parameterEditor.Visible = false;
					}
					this._parameterEditor = parameterEditor;
				}
				if (this._parameterEditor != null)
				{
					this._parameterEditor.InitializeParameter(parameterItem);
					this._parameterEditor.Visible = true;
					if (allowFocusChange)
					{
						this._parameterEditor.SetDefaultFocus();
					}
				}
			}
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x000EA8FE File Offset: 0x000E98FE
		public void SetAllowCollectionChanges(bool allowChanges)
		{
			this._moveUpButton.Visible = allowChanges;
			this._moveDownButton.Visible = allowChanges;
			this._deleteParameterButton.Visible = allowChanges;
			this._addParameterButton.Visible = allowChanges;
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x000EA930 File Offset: 0x000E9930
		private void ToggleAdvancedMode(object sender, EventArgs e)
		{
			this._inAdvancedMode = !this._inAdvancedMode;
			this.UpdateUI(true);
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000EA948 File Offset: 0x000E9948
		private void UpdateUI(bool allowFocusChange)
		{
			if (this._parametersListView.SelectedItems.Count > 0)
			{
				ParameterEditorUserControl.ParameterListViewItem parameterListViewItem = (ParameterEditorUserControl.ParameterListViewItem)this._parametersListView.SelectedItems[0];
				this._deleteParameterButton.Enabled = true;
				this._moveUpButton.Enabled = this._parametersListView.SelectedIndices[0] > 0;
				this._moveDownButton.Enabled = this._parametersListView.SelectedIndices[0] < this._parametersListView.Items.Count - 1;
				this._sourceLabel.Enabled = true;
				this._parameterTypeComboBox.Enabled = true;
				this._editorPanel.Enabled = true;
				if (parameterListViewItem.Parameter == null)
				{
					this._parameterTypeComboBox.SelectedIndex = -1;
				}
				else
				{
					Type type = parameterListViewItem.Parameter.GetType();
					object obj = this._parameterTypes[type];
					if (obj != null)
					{
						this._parameterTypeComboBox.SelectedItem = obj;
					}
					else
					{
						this._parameterTypeComboBox.SelectedIndex = -1;
					}
				}
				this.SetActiveEditParameterItem(parameterListViewItem, allowFocusChange);
				return;
			}
			this._deleteParameterButton.Enabled = false;
			this._moveUpButton.Enabled = false;
			this._moveDownButton.Enabled = false;
			this._sourceLabel.Enabled = false;
			this._parameterTypeComboBox.Enabled = false;
			this._parameterTypeComboBox.SelectedIndex = -1;
			this._editorPanel.Enabled = false;
			this.SetActiveEditParameterItem(null, false);
		}

		// Token: 0x04001CE3 RID: 7395
		private static readonly object EventParametersChanged = new object();

		// Token: 0x04001CE4 RID: 7396
		private global::System.Windows.Forms.Label _parametersLabel;

		// Token: 0x04001CE5 RID: 7397
		private ListView _parametersListView;

		// Token: 0x04001CE6 RID: 7398
		private AutoSizeComboBox _parameterTypeComboBox;

		// Token: 0x04001CE7 RID: 7399
		private ColumnHeader _nameColumnHeader;

		// Token: 0x04001CE8 RID: 7400
		private ColumnHeader _valueColumnHeader;

		// Token: 0x04001CE9 RID: 7401
		private global::System.Windows.Forms.Button _moveUpButton;

		// Token: 0x04001CEA RID: 7402
		private global::System.Windows.Forms.Button _moveDownButton;

		// Token: 0x04001CEB RID: 7403
		private global::System.Windows.Forms.Button _deleteParameterButton;

		// Token: 0x04001CEC RID: 7404
		private global::System.Windows.Forms.Button _addParameterButton;

		// Token: 0x04001CED RID: 7405
		private global::System.Windows.Forms.Panel _addButtonPanel;

		// Token: 0x04001CEE RID: 7406
		private global::System.Windows.Forms.Label _sourceLabel;

		// Token: 0x04001CEF RID: 7407
		private global::System.Windows.Forms.Panel _editorPanel;

		// Token: 0x04001CF0 RID: 7408
		private ListDictionary _parameterTypes;

		// Token: 0x04001CF1 RID: 7409
		private IServiceProvider _serviceProvider;

		// Token: 0x04001CF2 RID: 7410
		private ParameterEditorUserControl.ParameterEditor _parameterEditor;

		// Token: 0x04001CF3 RID: 7411
		private bool _inAdvancedMode;

		// Token: 0x04001CF4 RID: 7412
		private int _ignoreParameterChangesCount;

		// Token: 0x04001CF5 RID: 7413
		private ParameterEditorUserControl.AdvancedParameterEditor _advancedParameterEditor;

		// Token: 0x04001CF6 RID: 7414
		private ParameterEditorUserControl.ControlParameterEditor _controlParameterEditor;

		// Token: 0x04001CF7 RID: 7415
		private ParameterEditorUserControl.CookieParameterEditor _cookieParameterEditor;

		// Token: 0x04001CF8 RID: 7416
		private ParameterEditorUserControl.FormParameterEditor _formParameterEditor;

		// Token: 0x04001CF9 RID: 7417
		private ParameterEditorUserControl.QueryStringParameterEditor _queryStringParameterEditor;

		// Token: 0x04001CFA RID: 7418
		private ParameterEditorUserControl.SessionParameterEditor _sessionParameterEditor;

		// Token: 0x04001CFB RID: 7419
		private ParameterEditorUserControl.StaticParameterEditor _staticParameterEditor;

		// Token: 0x04001CFC RID: 7420
		private ParameterEditorUserControl.ProfileParameterEditor _profileParameterEditor;

		// Token: 0x04001CFD RID: 7421
		private Control _control;

		// Token: 0x02000491 RID: 1169
		internal sealed class ControlItem
		{
			// Token: 0x06002A77 RID: 10871 RVA: 0x000EAABE File Offset: 0x000E9ABE
			public ControlItem(string controlID, string propertyName)
			{
				this._controlID = controlID;
				this._propertyName = propertyName;
			}

			// Token: 0x170007EB RID: 2027
			// (get) Token: 0x06002A78 RID: 10872 RVA: 0x000EAAD4 File Offset: 0x000E9AD4
			public string ControlID
			{
				get
				{
					return this._controlID;
				}
			}

			// Token: 0x170007EC RID: 2028
			// (get) Token: 0x06002A79 RID: 10873 RVA: 0x000EAADC File Offset: 0x000E9ADC
			public string PropertyName
			{
				get
				{
					return this._propertyName;
				}
			}

			// Token: 0x06002A7A RID: 10874 RVA: 0x000EAAE4 File Offset: 0x000E9AE4
			private static bool IsValidComponent(IComponent component)
			{
				Control control = component as Control;
				return control != null && !string.IsNullOrEmpty(control.ID);
			}

			// Token: 0x06002A7B RID: 10875 RVA: 0x000EAB10 File Offset: 0x000E9B10
			public static ParameterEditorUserControl.ControlItem[] GetControlItems(IDesignerHost host, Control control)
			{
				IList<IComponent> allComponents = ControlHelper.GetAllComponents(control, new ControlHelper.IsValidComponentDelegate(ParameterEditorUserControl.ControlItem.IsValidComponent));
				List<ParameterEditorUserControl.ControlItem> list = new List<ParameterEditorUserControl.ControlItem>();
				foreach (IComponent component in allComponents)
				{
					Control control2 = (Control)component;
					string defaultValuePropertyName = ParameterEditorUserControl.GetDefaultValuePropertyName(control2);
					if (!string.IsNullOrEmpty(defaultValuePropertyName))
					{
						list.Add(new ParameterEditorUserControl.ControlItem(control2.ID, defaultValuePropertyName));
					}
				}
				return list.ToArray();
			}

			// Token: 0x06002A7C RID: 10876 RVA: 0x000EAB9C File Offset: 0x000E9B9C
			public override string ToString()
			{
				return this._controlID;
			}

			// Token: 0x04001CFE RID: 7422
			private string _controlID;

			// Token: 0x04001CFF RID: 7423
			private string _propertyName;
		}

		// Token: 0x02000492 RID: 1170
		private class ParameterListViewItem : ListViewItem
		{
			// Token: 0x06002A7D RID: 10877 RVA: 0x000EABA4 File Offset: 0x000E9BA4
			public ParameterListViewItem(Parameter parameter)
			{
				this._parameter = parameter;
				this._isConfigured = true;
			}

			// Token: 0x170007ED RID: 2029
			// (get) Token: 0x06002A7E RID: 10878 RVA: 0x000EABBA File Offset: 0x000E9BBA
			// (set) Token: 0x06002A7F RID: 10879 RVA: 0x000EABC7 File Offset: 0x000E9BC7
			public DbType DbType
			{
				get
				{
					return this._parameter.DbType;
				}
				set
				{
					this._parameter.DbType = value;
				}
			}

			// Token: 0x170007EE RID: 2030
			// (get) Token: 0x06002A80 RID: 10880 RVA: 0x000EABD5 File Offset: 0x000E9BD5
			public bool IsConfigured
			{
				get
				{
					return this._isConfigured;
				}
			}

			// Token: 0x170007EF RID: 2031
			// (get) Token: 0x06002A81 RID: 10881 RVA: 0x000EABDD File Offset: 0x000E9BDD
			// (set) Token: 0x06002A82 RID: 10882 RVA: 0x000EABEA File Offset: 0x000E9BEA
			public string ParameterName
			{
				get
				{
					return this._parameter.Name;
				}
				set
				{
					this._parameter.Name = value;
				}
			}

			// Token: 0x170007F0 RID: 2032
			// (get) Token: 0x06002A83 RID: 10883 RVA: 0x000EABF8 File Offset: 0x000E9BF8
			// (set) Token: 0x06002A84 RID: 10884 RVA: 0x000EAC05 File Offset: 0x000E9C05
			public TypeCode ParameterType
			{
				get
				{
					return this._parameter.Type;
				}
				set
				{
					this._parameter.Type = value;
				}
			}

			// Token: 0x170007F1 RID: 2033
			// (get) Token: 0x06002A85 RID: 10885 RVA: 0x000EAC13 File Offset: 0x000E9C13
			// (set) Token: 0x06002A86 RID: 10886 RVA: 0x000EAC1C File Offset: 0x000E9C1C
			public Parameter Parameter
			{
				get
				{
					return this._parameter;
				}
				set
				{
					string defaultValue = this._parameter.DefaultValue;
					ParameterDirection direction = this._parameter.Direction;
					string name = this._parameter.Name;
					bool convertEmptyStringToNull = this._parameter.ConvertEmptyStringToNull;
					int size = this._parameter.Size;
					TypeCode type = this._parameter.Type;
					DbType dbType = this._parameter.DbType;
					this._parameter = value;
					this._parameter.DefaultValue = defaultValue;
					this._parameter.Direction = direction;
					this._parameter.Name = name;
					this._parameter.ConvertEmptyStringToNull = convertEmptyStringToNull;
					this._parameter.Size = size;
					this._parameter.Type = type;
					this._parameter.DbType = dbType;
				}
			}

			// Token: 0x06002A87 RID: 10887 RVA: 0x000EACE0 File Offset: 0x000E9CE0
			public void Refresh()
			{
				base.SubItems.Clear();
				base.Text = this.ParameterName;
				base.UseItemStyleForSubItems = false;
				ListView listView = base.ListView;
				IServiceProvider serviceProvider = null;
				Control control = null;
				if (listView != null)
				{
					ParameterEditorUserControl parameterEditorUserControl = (ParameterEditorUserControl)listView.Parent;
					serviceProvider = parameterEditorUserControl._serviceProvider;
					control = parameterEditorUserControl._control;
				}
				bool flag;
				string parameterExpression = ParameterEditorUserControl.GetParameterExpression(serviceProvider, this._parameter, control, out flag);
				this._isConfigured = !flag;
				ListViewItem.ListViewSubItem listViewSubItem = new ListViewItem.ListViewSubItem();
				listViewSubItem.Text = parameterExpression;
				if (flag)
				{
					listViewSubItem.ForeColor = SystemColors.GrayText;
				}
				base.SubItems.Add(listViewSubItem);
			}

			// Token: 0x04001D00 RID: 7424
			private Parameter _parameter;

			// Token: 0x04001D01 RID: 7425
			private bool _isConfigured;
		}

		// Token: 0x02000493 RID: 1171
		private class PropertyGridSite : ISite, IServiceProvider
		{
			// Token: 0x06002A88 RID: 10888 RVA: 0x000EAD7E File Offset: 0x000E9D7E
			public PropertyGridSite(IServiceProvider sp, IComponent comp)
			{
				this._sp = sp;
				this._comp = comp;
			}

			// Token: 0x170007F2 RID: 2034
			// (get) Token: 0x06002A89 RID: 10889 RVA: 0x000EAD94 File Offset: 0x000E9D94
			public IComponent Component
			{
				get
				{
					return this._comp;
				}
			}

			// Token: 0x170007F3 RID: 2035
			// (get) Token: 0x06002A8A RID: 10890 RVA: 0x000EAD9C File Offset: 0x000E9D9C
			public IContainer Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x170007F4 RID: 2036
			// (get) Token: 0x06002A8B RID: 10891 RVA: 0x000EAD9F File Offset: 0x000E9D9F
			public bool DesignMode
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170007F5 RID: 2037
			// (get) Token: 0x06002A8C RID: 10892 RVA: 0x000EADA2 File Offset: 0x000E9DA2
			// (set) Token: 0x06002A8D RID: 10893 RVA: 0x000EADA5 File Offset: 0x000E9DA5
			public string Name
			{
				get
				{
					return null;
				}
				set
				{
				}
			}

			// Token: 0x06002A8E RID: 10894 RVA: 0x000EADA8 File Offset: 0x000E9DA8
			public object GetService(Type t)
			{
				if (!this._inGetService && this._sp != null)
				{
					try
					{
						this._inGetService = true;
						return this._sp.GetService(t);
					}
					finally
					{
						this._inGetService = false;
					}
				}
				return null;
			}

			// Token: 0x04001D02 RID: 7426
			private IServiceProvider _sp;

			// Token: 0x04001D03 RID: 7427
			private IComponent _comp;

			// Token: 0x04001D04 RID: 7428
			private bool _inGetService;
		}

		// Token: 0x02000494 RID: 1172
		private abstract class ParameterEditor : global::System.Windows.Forms.Panel
		{
			// Token: 0x06002A8F RID: 10895 RVA: 0x000EADF8 File Offset: 0x000E9DF8
			protected ParameterEditor(IServiceProvider serviceProvider)
			{
				this._serviceProvider = serviceProvider;
			}

			// Token: 0x170007F6 RID: 2038
			// (get) Token: 0x06002A90 RID: 10896 RVA: 0x000EAE07 File Offset: 0x000E9E07
			protected ParameterEditorUserControl.ParameterListViewItem ParameterItem
			{
				get
				{
					return this._parameterItem;
				}
			}

			// Token: 0x170007F7 RID: 2039
			// (get) Token: 0x06002A91 RID: 10897 RVA: 0x000EAE0F File Offset: 0x000E9E0F
			protected IServiceProvider ServiceProvider
			{
				get
				{
					return this._serviceProvider;
				}
			}

			// Token: 0x1400003E RID: 62
			// (add) Token: 0x06002A92 RID: 10898 RVA: 0x000EAE17 File Offset: 0x000E9E17
			// (remove) Token: 0x06002A93 RID: 10899 RVA: 0x000EAE2A File Offset: 0x000E9E2A
			public event EventHandler ParameterChanged
			{
				add
				{
					base.Events.AddHandler(ParameterEditorUserControl.ParameterEditor.EventParameterChanged, value);
				}
				remove
				{
					base.Events.RemoveHandler(ParameterEditorUserControl.ParameterEditor.EventParameterChanged, value);
				}
			}

			// Token: 0x1400003F RID: 63
			// (add) Token: 0x06002A94 RID: 10900 RVA: 0x000EAE3D File Offset: 0x000E9E3D
			// (remove) Token: 0x06002A95 RID: 10901 RVA: 0x000EAE50 File Offset: 0x000E9E50
			public event EventHandler RequestModeChange
			{
				add
				{
					base.Events.AddHandler(ParameterEditorUserControl.ParameterEditor.EventRequestModeChange, value);
				}
				remove
				{
					base.Events.RemoveHandler(ParameterEditorUserControl.ParameterEditor.EventRequestModeChange, value);
				}
			}

			// Token: 0x06002A96 RID: 10902 RVA: 0x000EAE63 File Offset: 0x000E9E63
			public virtual void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				this._parameterItem = parameterItem;
			}

			// Token: 0x06002A97 RID: 10903 RVA: 0x000EAE6C File Offset: 0x000E9E6C
			protected void OnParameterChanged()
			{
				this.ParameterItem.Refresh();
				EventHandler eventHandler = base.Events[ParameterEditorUserControl.ParameterEditor.EventParameterChanged] as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(this, EventArgs.Empty);
				}
			}

			// Token: 0x06002A98 RID: 10904 RVA: 0x000EAEAC File Offset: 0x000E9EAC
			protected void OnRequestModeChange()
			{
				EventHandler eventHandler = base.Events[ParameterEditorUserControl.ParameterEditor.EventRequestModeChange] as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(this, EventArgs.Empty);
				}
			}

			// Token: 0x06002A99 RID: 10905 RVA: 0x000EAEDE File Offset: 0x000E9EDE
			public virtual void SetDefaultFocus()
			{
			}

			// Token: 0x04001D05 RID: 7429
			private static readonly object EventParameterChanged = new object();

			// Token: 0x04001D06 RID: 7430
			private static readonly object EventRequestModeChange = new object();

			// Token: 0x04001D07 RID: 7431
			private IServiceProvider _serviceProvider;

			// Token: 0x04001D08 RID: 7432
			private ParameterEditorUserControl.ParameterListViewItem _parameterItem;
		}

		// Token: 0x02000495 RID: 1173
		private sealed class AdvancedParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
			// Token: 0x06002A9B RID: 10907 RVA: 0x000EAEF8 File Offset: 0x000E9EF8
			public AdvancedParameterEditor(IServiceProvider serviceProvider, Control control)
				: base(serviceProvider)
			{
				this._control = control;
				base.SuspendLayout();
				base.Size = new Size(400, 400);
				this._advancedlabel = new global::System.Windows.Forms.Label();
				this._parameterPropertyGrid = new VsPropertyGrid(base.ServiceProvider);
				this._hideAdvancedLinkLabel = new LinkLabel();
				this._advancedlabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._advancedlabel.Location = new Point(0, 0);
				this._advancedlabel.Size = new Size(400, 16);
				this._advancedlabel.TabIndex = 10;
				this._advancedlabel.Text = SR.GetString("ParameterEditorUserControl_AdvancedProperties");
				this._parameterPropertyGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
				this._parameterPropertyGrid.CommandsVisibleIfAvailable = true;
				this._parameterPropertyGrid.LargeButtons = false;
				this._parameterPropertyGrid.LineColor = SystemColors.ScrollBar;
				this._parameterPropertyGrid.Location = new Point(0, 18);
				this._parameterPropertyGrid.PropertySort = PropertySort.Alphabetical;
				this._parameterPropertyGrid.Site = new ParameterEditorUserControl.PropertyGridSite(base.ServiceProvider, this._parameterPropertyGrid);
				this._parameterPropertyGrid.Size = new Size(400, 356);
				this._parameterPropertyGrid.TabIndex = 20;
				this._parameterPropertyGrid.ToolbarVisible = false;
				this._parameterPropertyGrid.ViewBackColor = SystemColors.Window;
				this._parameterPropertyGrid.ViewForeColor = SystemColors.WindowText;
				this._parameterPropertyGrid.PropertyValueChanged += this.OnParameterPropertyGridPropertyValueChanged;
				this._hideAdvancedLinkLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
				this._hideAdvancedLinkLabel.Location = new Point(0, 384);
				this._hideAdvancedLinkLabel.Size = new Size(400, 16);
				this._hideAdvancedLinkLabel.TabIndex = 30;
				this._hideAdvancedLinkLabel.TabStop = true;
				this._hideAdvancedLinkLabel.Text = SR.GetString("ParameterEditorUserControl_HideAdvancedPropertiesLabel");
				this._hideAdvancedLinkLabel.Links.Add(new LinkLabel.Link(0, this._hideAdvancedLinkLabel.Text.Length));
				this._hideAdvancedLinkLabel.LinkClicked += this.OnHideAdvancedLinkLabelLinkClicked;
				base.Controls.Add(this._advancedlabel);
				base.Controls.Add(this._parameterPropertyGrid);
				base.Controls.Add(this._hideAdvancedLinkLabel);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			// Token: 0x06002A9C RID: 10908 RVA: 0x000EB16D File Offset: 0x000EA16D
			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._parameterPropertyGrid.SelectedObject = base.ParameterItem.Parameter;
			}

			// Token: 0x06002A9D RID: 10909 RVA: 0x000EB18C File Offset: 0x000EA18C
			private void OnHideAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			// Token: 0x06002A9E RID: 10910 RVA: 0x000EB194 File Offset: 0x000EA194
			private void OnParameterPropertyGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
			{
				if (e.ChangedItem.PropertyDescriptor.Name == "ControlID")
				{
					ControlParameter controlParameter = base.ParameterItem.Parameter as ControlParameter;
					if (controlParameter != null && controlParameter.PropertyName.Length == 0 && controlParameter.ControlID != (string)e.OldValue)
					{
						controlParameter.PropertyName = ParameterEditorUserControl.GetControlDefaultValuePropertyName(controlParameter.ControlID, base.ServiceProvider, this._control);
					}
				}
				base.OnParameterChanged();
			}

			// Token: 0x06002A9F RID: 10911 RVA: 0x000EB219 File Offset: 0x000EA219
			public override void SetDefaultFocus()
			{
				this._parameterPropertyGrid.Focus();
			}

			// Token: 0x04001D09 RID: 7433
			private global::System.Windows.Forms.Label _advancedlabel;

			// Token: 0x04001D0A RID: 7434
			private PropertyGrid _parameterPropertyGrid;

			// Token: 0x04001D0B RID: 7435
			private LinkLabel _hideAdvancedLinkLabel;

			// Token: 0x04001D0C RID: 7436
			private Control _control;
		}

		// Token: 0x02000496 RID: 1174
		private sealed class ControlParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
			// Token: 0x06002AA0 RID: 10912 RVA: 0x000EB228 File Offset: 0x000EA228
			public ControlParameterEditor(IServiceProvider serviceProvider, Control control)
				: base(serviceProvider)
			{
				this._control = control;
				base.SuspendLayout();
				base.Size = new Size(400, 400);
				this._controlIDLabel = new global::System.Windows.Forms.Label();
				this._controlIDComboBox = new AutoSizeComboBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._showAdvancedLinkLabel = new LinkLabel();
				this._controlIDLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._controlIDLabel.Location = new Point(0, 0);
				this._controlIDLabel.Size = new Size(400, 16);
				this._controlIDLabel.TabIndex = 10;
				this._controlIDLabel.Text = SR.GetString("ParameterEditorUserControl_ControlParameterControlID");
				this._controlIDComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._controlIDComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				this._controlIDComboBox.Location = new Point(0, 18);
				this._controlIDComboBox.Size = new Size(400, 21);
				this._controlIDComboBox.Sorted = true;
				this._controlIDComboBox.TabIndex = 20;
				this._controlIDComboBox.SelectedIndexChanged += this.OnControlIDComboBoxSelectedIndexChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 45);
				this._defaultValueLabel.Size = new Size(400, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("ParameterEditorUserControl_ParameterDefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 63);
				this._defaultValueTextBox.Size = new Size(400, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				this._showAdvancedLinkLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._showAdvancedLinkLabel.Location = new Point(0, 87);
				this._showAdvancedLinkLabel.Size = new Size(400, 16);
				this._showAdvancedLinkLabel.TabIndex = 50;
				this._showAdvancedLinkLabel.TabStop = true;
				this._showAdvancedLinkLabel.Text = SR.GetString("ParameterEditorUserControl_ShowAdvancedProperties");
				this._showAdvancedLinkLabel.Links.Add(new LinkLabel.Link(0, this._showAdvancedLinkLabel.Text.Length));
				this._showAdvancedLinkLabel.LinkClicked += this.OnShowAdvancedLinkLabelLinkClicked;
				base.Controls.Add(this._controlIDLabel);
				base.Controls.Add(this._controlIDComboBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				base.Controls.Add(this._showAdvancedLinkLabel);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			// Token: 0x06002AA1 RID: 10913 RVA: 0x000EB51C File Offset: 0x000EA51C
			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				string controlID = ((ControlParameter)base.ParameterItem.Parameter).ControlID;
				string propertyName = ((ControlParameter)base.ParameterItem.Parameter).PropertyName;
				this._controlIDComboBox.Items.Clear();
				ParameterEditorUserControl.ControlItem controlItem = null;
				if (base.ServiceProvider != null)
				{
					IDesignerHost designerHost = (IDesignerHost)base.ServiceProvider.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						ParameterEditorUserControl.ControlItem[] controlItems = ParameterEditorUserControl.ControlItem.GetControlItems(designerHost, this._control);
						foreach (ParameterEditorUserControl.ControlItem controlItem2 in controlItems)
						{
							this._controlIDComboBox.Items.Add(controlItem2);
							if (controlItem2.ControlID == controlID && controlItem2.PropertyName == propertyName)
							{
								controlItem = controlItem2;
							}
						}
					}
				}
				if (controlItem == null && controlID.Length > 0)
				{
					ParameterEditorUserControl.ControlItem controlItem3 = new ParameterEditorUserControl.ControlItem(controlID, propertyName);
					this._controlIDComboBox.Items.Insert(0, controlItem3);
					controlItem = controlItem3;
				}
				this._controlIDComboBox.InvalidateDropDownWidth();
				this._controlIDComboBox.SelectedItem = controlItem;
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
			}

			// Token: 0x06002AA2 RID: 10914 RVA: 0x000EB651 File Offset: 0x000EA651
			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			// Token: 0x06002AA3 RID: 10915 RVA: 0x000EB65C File Offset: 0x000EA65C
			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002AA4 RID: 10916 RVA: 0x000EB6AC File Offset: 0x000EA6AC
			private void OnControlIDComboBoxSelectedIndexChanged(object s, EventArgs e)
			{
				ParameterEditorUserControl.ControlItem controlItem = this._controlIDComboBox.SelectedItem as ParameterEditorUserControl.ControlItem;
				ControlParameter controlParameter = (ControlParameter)base.ParameterItem.Parameter;
				if (controlItem == null)
				{
					controlParameter.ControlID = string.Empty;
					controlParameter.PropertyName = string.Empty;
				}
				else
				{
					controlParameter.ControlID = controlItem.ControlID;
					controlParameter.PropertyName = controlItem.PropertyName;
				}
				base.OnParameterChanged();
			}

			// Token: 0x06002AA5 RID: 10917 RVA: 0x000EB714 File Offset: 0x000EA714
			public override void SetDefaultFocus()
			{
				this._controlIDComboBox.Focus();
			}

			// Token: 0x04001D0D RID: 7437
			private global::System.Windows.Forms.Label _controlIDLabel;

			// Token: 0x04001D0E RID: 7438
			private AutoSizeComboBox _controlIDComboBox;

			// Token: 0x04001D0F RID: 7439
			private global::System.Windows.Forms.Label _defaultValueLabel;

			// Token: 0x04001D10 RID: 7440
			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			// Token: 0x04001D11 RID: 7441
			private LinkLabel _showAdvancedLinkLabel;

			// Token: 0x04001D12 RID: 7442
			private Control _control;
		}

		// Token: 0x02000497 RID: 1175
		private sealed class CookieParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
			// Token: 0x06002AA6 RID: 10918 RVA: 0x000EB724 File Offset: 0x000EA724
			public CookieParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(400, 400);
				this._cookieNameLabel = new global::System.Windows.Forms.Label();
				this._cookieNameTextBox = new global::System.Windows.Forms.TextBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._showAdvancedLinkLabel = new LinkLabel();
				this._cookieNameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._cookieNameLabel.Location = new Point(0, 0);
				this._cookieNameLabel.Size = new Size(400, 16);
				this._cookieNameLabel.TabIndex = 10;
				this._cookieNameLabel.Text = SR.GetString("ParameterEditorUserControl_CookieParameterCookieName");
				this._cookieNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._cookieNameTextBox.Location = new Point(0, 18);
				this._cookieNameTextBox.Size = new Size(400, 20);
				this._cookieNameTextBox.TabIndex = 20;
				this._cookieNameTextBox.TextChanged += this.OnCookieNameTextBoxTextChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 44);
				this._defaultValueLabel.Size = new Size(400, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("ParameterEditorUserControl_ParameterDefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 62);
				this._defaultValueTextBox.Size = new Size(400, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				this._showAdvancedLinkLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._showAdvancedLinkLabel.Location = new Point(0, 86);
				this._showAdvancedLinkLabel.Size = new Size(400, 16);
				this._showAdvancedLinkLabel.TabIndex = 50;
				this._showAdvancedLinkLabel.TabStop = true;
				this._showAdvancedLinkLabel.Text = SR.GetString("ParameterEditorUserControl_ShowAdvancedProperties");
				this._showAdvancedLinkLabel.Links.Add(new LinkLabel.Link(0, this._showAdvancedLinkLabel.Text.Length));
				this._showAdvancedLinkLabel.LinkClicked += this.OnShowAdvancedLinkLabelLinkClicked;
				base.Controls.Add(this._cookieNameLabel);
				base.Controls.Add(this._cookieNameTextBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				base.Controls.Add(this._showAdvancedLinkLabel);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			// Token: 0x06002AA7 RID: 10919 RVA: 0x000EB9F8 File Offset: 0x000EA9F8
			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
				this._cookieNameTextBox.Text = ((CookieParameter)base.ParameterItem.Parameter).CookieName;
			}

			// Token: 0x06002AA8 RID: 10920 RVA: 0x000EBA47 File Offset: 0x000EAA47
			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			// Token: 0x06002AA9 RID: 10921 RVA: 0x000EBA50 File Offset: 0x000EAA50
			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002AAA RID: 10922 RVA: 0x000EBAA0 File Offset: 0x000EAAA0
			private void OnCookieNameTextBoxTextChanged(object s, EventArgs e)
			{
				if (((CookieParameter)base.ParameterItem.Parameter).CookieName != this._cookieNameTextBox.Text)
				{
					((CookieParameter)base.ParameterItem.Parameter).CookieName = this._cookieNameTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002AAB RID: 10923 RVA: 0x000EBAFA File Offset: 0x000EAAFA
			public override void SetDefaultFocus()
			{
				this._cookieNameTextBox.Focus();
			}

			// Token: 0x04001D13 RID: 7443
			private global::System.Windows.Forms.Label _cookieNameLabel;

			// Token: 0x04001D14 RID: 7444
			private global::System.Windows.Forms.TextBox _cookieNameTextBox;

			// Token: 0x04001D15 RID: 7445
			private global::System.Windows.Forms.Label _defaultValueLabel;

			// Token: 0x04001D16 RID: 7446
			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			// Token: 0x04001D17 RID: 7447
			private LinkLabel _showAdvancedLinkLabel;
		}

		// Token: 0x02000498 RID: 1176
		private sealed class FormParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
			// Token: 0x06002AAC RID: 10924 RVA: 0x000EBB08 File Offset: 0x000EAB08
			public FormParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(400, 400);
				this._formFieldLabel = new global::System.Windows.Forms.Label();
				this._formFieldTextBox = new global::System.Windows.Forms.TextBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._showAdvancedLinkLabel = new LinkLabel();
				this._formFieldLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._formFieldLabel.Location = new Point(0, 0);
				this._formFieldLabel.Size = new Size(400, 16);
				this._formFieldLabel.TabIndex = 10;
				this._formFieldLabel.Text = SR.GetString("ParameterEditorUserControl_FormParameterFormField");
				this._formFieldTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._formFieldTextBox.Location = new Point(0, 18);
				this._formFieldTextBox.Size = new Size(400, 20);
				this._formFieldTextBox.TabIndex = 20;
				this._formFieldTextBox.TextChanged += this.OnFormFieldTextBoxTextChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 44);
				this._defaultValueLabel.Size = new Size(400, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("ParameterEditorUserControl_ParameterDefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 62);
				this._defaultValueTextBox.Size = new Size(400, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				this._showAdvancedLinkLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._showAdvancedLinkLabel.Location = new Point(0, 86);
				this._showAdvancedLinkLabel.Size = new Size(400, 16);
				this._showAdvancedLinkLabel.TabIndex = 50;
				this._showAdvancedLinkLabel.TabStop = true;
				this._showAdvancedLinkLabel.Text = SR.GetString("ParameterEditorUserControl_ShowAdvancedProperties");
				this._showAdvancedLinkLabel.Links.Add(new LinkLabel.Link(0, this._showAdvancedLinkLabel.Text.Length));
				this._showAdvancedLinkLabel.LinkClicked += this.OnShowAdvancedLinkLabelLinkClicked;
				base.Controls.Add(this._formFieldLabel);
				base.Controls.Add(this._formFieldTextBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				base.Controls.Add(this._showAdvancedLinkLabel);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			// Token: 0x06002AAD RID: 10925 RVA: 0x000EBDDC File Offset: 0x000EADDC
			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
				this._formFieldTextBox.Text = ((FormParameter)base.ParameterItem.Parameter).FormField;
			}

			// Token: 0x06002AAE RID: 10926 RVA: 0x000EBE2B File Offset: 0x000EAE2B
			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			// Token: 0x06002AAF RID: 10927 RVA: 0x000EBE34 File Offset: 0x000EAE34
			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002AB0 RID: 10928 RVA: 0x000EBE84 File Offset: 0x000EAE84
			private void OnFormFieldTextBoxTextChanged(object s, EventArgs e)
			{
				if (((FormParameter)base.ParameterItem.Parameter).FormField != this._formFieldTextBox.Text)
				{
					((FormParameter)base.ParameterItem.Parameter).FormField = this._formFieldTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002AB1 RID: 10929 RVA: 0x000EBEDE File Offset: 0x000EAEDE
			public override void SetDefaultFocus()
			{
				this._formFieldTextBox.Focus();
			}

			// Token: 0x04001D18 RID: 7448
			private global::System.Windows.Forms.Label _formFieldLabel;

			// Token: 0x04001D19 RID: 7449
			private global::System.Windows.Forms.TextBox _formFieldTextBox;

			// Token: 0x04001D1A RID: 7450
			private global::System.Windows.Forms.Label _defaultValueLabel;

			// Token: 0x04001D1B RID: 7451
			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			// Token: 0x04001D1C RID: 7452
			private LinkLabel _showAdvancedLinkLabel;
		}

		// Token: 0x02000499 RID: 1177
		private sealed class SessionParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
			// Token: 0x06002AB2 RID: 10930 RVA: 0x000EBEEC File Offset: 0x000EAEEC
			public SessionParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(400, 400);
				this._sessionFieldLabel = new global::System.Windows.Forms.Label();
				this._sessionFieldTextBox = new global::System.Windows.Forms.TextBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._showAdvancedLinkLabel = new LinkLabel();
				this._sessionFieldLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._sessionFieldLabel.Location = new Point(0, 0);
				this._sessionFieldLabel.Size = new Size(400, 16);
				this._sessionFieldLabel.TabIndex = 10;
				this._sessionFieldLabel.Text = SR.GetString("ParameterEditorUserControl_SessionParameterSessionField");
				this._sessionFieldTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._sessionFieldTextBox.Location = new Point(0, 18);
				this._sessionFieldTextBox.Size = new Size(400, 20);
				this._sessionFieldTextBox.TabIndex = 20;
				this._sessionFieldTextBox.TextChanged += this.OnSessionFieldTextBoxTextChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 44);
				this._defaultValueLabel.Size = new Size(400, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("ParameterEditorUserControl_ParameterDefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 62);
				this._defaultValueTextBox.Size = new Size(400, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				this._showAdvancedLinkLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._showAdvancedLinkLabel.Location = new Point(0, 86);
				this._showAdvancedLinkLabel.Size = new Size(400, 16);
				this._showAdvancedLinkLabel.TabIndex = 50;
				this._showAdvancedLinkLabel.TabStop = true;
				this._showAdvancedLinkLabel.Text = SR.GetString("ParameterEditorUserControl_ShowAdvancedProperties");
				this._showAdvancedLinkLabel.Links.Add(new LinkLabel.Link(0, this._showAdvancedLinkLabel.Text.Length));
				this._showAdvancedLinkLabel.LinkClicked += this.OnShowAdvancedLinkLabelLinkClicked;
				base.Controls.Add(this._sessionFieldLabel);
				base.Controls.Add(this._sessionFieldTextBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				base.Controls.Add(this._showAdvancedLinkLabel);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			// Token: 0x06002AB3 RID: 10931 RVA: 0x000EC1C0 File Offset: 0x000EB1C0
			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
				this._sessionFieldTextBox.Text = ((SessionParameter)base.ParameterItem.Parameter).SessionField;
			}

			// Token: 0x06002AB4 RID: 10932 RVA: 0x000EC20F File Offset: 0x000EB20F
			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			// Token: 0x06002AB5 RID: 10933 RVA: 0x000EC218 File Offset: 0x000EB218
			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002AB6 RID: 10934 RVA: 0x000EC268 File Offset: 0x000EB268
			private void OnSessionFieldTextBoxTextChanged(object s, EventArgs e)
			{
				if (((SessionParameter)base.ParameterItem.Parameter).SessionField != this._sessionFieldTextBox.Text)
				{
					((SessionParameter)base.ParameterItem.Parameter).SessionField = this._sessionFieldTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002AB7 RID: 10935 RVA: 0x000EC2C2 File Offset: 0x000EB2C2
			public override void SetDefaultFocus()
			{
				this._sessionFieldTextBox.Focus();
			}

			// Token: 0x04001D1D RID: 7453
			private global::System.Windows.Forms.Label _sessionFieldLabel;

			// Token: 0x04001D1E RID: 7454
			private global::System.Windows.Forms.TextBox _sessionFieldTextBox;

			// Token: 0x04001D1F RID: 7455
			private global::System.Windows.Forms.Label _defaultValueLabel;

			// Token: 0x04001D20 RID: 7456
			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			// Token: 0x04001D21 RID: 7457
			private LinkLabel _showAdvancedLinkLabel;
		}

		// Token: 0x0200049A RID: 1178
		private sealed class StaticParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
			// Token: 0x06002AB8 RID: 10936 RVA: 0x000EC2D0 File Offset: 0x000EB2D0
			public StaticParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(400, 400);
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._showAdvancedLinkLabel = new LinkLabel();
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 0);
				this._defaultValueLabel.Size = new Size(400, 16);
				this._defaultValueLabel.TabIndex = 10;
				this._defaultValueLabel.Text = SR.GetString("ParameterEditorUserControl_ParameterDefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 18);
				this._defaultValueTextBox.Size = new Size(400, 20);
				this._defaultValueTextBox.TabIndex = 20;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				this._showAdvancedLinkLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._showAdvancedLinkLabel.Location = new Point(0, 42);
				this._showAdvancedLinkLabel.Size = new Size(400, 16);
				this._showAdvancedLinkLabel.TabIndex = 30;
				this._showAdvancedLinkLabel.TabStop = true;
				this._showAdvancedLinkLabel.Text = SR.GetString("ParameterEditorUserControl_ShowAdvancedProperties");
				this._showAdvancedLinkLabel.Links.Add(new LinkLabel.Link(0, this._showAdvancedLinkLabel.Text.Length));
				this._showAdvancedLinkLabel.LinkClicked += this.OnShowAdvancedLinkLabelLinkClicked;
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				base.Controls.Add(this._showAdvancedLinkLabel);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			// Token: 0x06002AB9 RID: 10937 RVA: 0x000EC4B6 File Offset: 0x000EB4B6
			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
			}

			// Token: 0x06002ABA RID: 10938 RVA: 0x000EC4DA File Offset: 0x000EB4DA
			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			// Token: 0x06002ABB RID: 10939 RVA: 0x000EC4E4 File Offset: 0x000EB4E4
			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002ABC RID: 10940 RVA: 0x000EC534 File Offset: 0x000EB534
			public override void SetDefaultFocus()
			{
				this._defaultValueTextBox.Focus();
			}

			// Token: 0x04001D22 RID: 7458
			private global::System.Windows.Forms.Label _defaultValueLabel;

			// Token: 0x04001D23 RID: 7459
			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			// Token: 0x04001D24 RID: 7460
			private LinkLabel _showAdvancedLinkLabel;
		}

		// Token: 0x0200049B RID: 1179
		private sealed class QueryStringParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
			// Token: 0x06002ABD RID: 10941 RVA: 0x000EC544 File Offset: 0x000EB544
			public QueryStringParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(400, 400);
				this._queryStringFieldLabel = new global::System.Windows.Forms.Label();
				this._queryStringFieldTextBox = new global::System.Windows.Forms.TextBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._showAdvancedLinkLabel = new LinkLabel();
				this._queryStringFieldLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._queryStringFieldLabel.Location = new Point(0, 0);
				this._queryStringFieldLabel.Size = new Size(400, 16);
				this._queryStringFieldLabel.TabIndex = 10;
				this._queryStringFieldLabel.Text = SR.GetString("ParameterEditorUserControl_QueryStringParameterQueryStringField");
				this._queryStringFieldTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._queryStringFieldTextBox.Location = new Point(0, 18);
				this._queryStringFieldTextBox.Size = new Size(400, 20);
				this._queryStringFieldTextBox.TabIndex = 20;
				this._queryStringFieldTextBox.TextChanged += this.OnQueryStringFieldTextBoxTextChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 44);
				this._defaultValueLabel.Size = new Size(400, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("ParameterEditorUserControl_ParameterDefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 62);
				this._defaultValueTextBox.Size = new Size(400, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				this._showAdvancedLinkLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._showAdvancedLinkLabel.Location = new Point(0, 86);
				this._showAdvancedLinkLabel.Size = new Size(400, 16);
				this._showAdvancedLinkLabel.TabIndex = 50;
				this._showAdvancedLinkLabel.TabStop = true;
				this._showAdvancedLinkLabel.Text = SR.GetString("ParameterEditorUserControl_ShowAdvancedProperties");
				this._showAdvancedLinkLabel.Links.Add(new LinkLabel.Link(0, this._showAdvancedLinkLabel.Text.Length));
				this._showAdvancedLinkLabel.LinkClicked += this.OnShowAdvancedLinkLabelLinkClicked;
				base.Controls.Add(this._queryStringFieldLabel);
				base.Controls.Add(this._queryStringFieldTextBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				base.Controls.Add(this._showAdvancedLinkLabel);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			// Token: 0x06002ABE RID: 10942 RVA: 0x000EC818 File Offset: 0x000EB818
			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
				this._queryStringFieldTextBox.Text = ((QueryStringParameter)base.ParameterItem.Parameter).QueryStringField;
			}

			// Token: 0x06002ABF RID: 10943 RVA: 0x000EC867 File Offset: 0x000EB867
			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			// Token: 0x06002AC0 RID: 10944 RVA: 0x000EC870 File Offset: 0x000EB870
			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002AC1 RID: 10945 RVA: 0x000EC8C0 File Offset: 0x000EB8C0
			private void OnQueryStringFieldTextBoxTextChanged(object s, EventArgs e)
			{
				if (((QueryStringParameter)base.ParameterItem.Parameter).QueryStringField != this._queryStringFieldTextBox.Text)
				{
					((QueryStringParameter)base.ParameterItem.Parameter).QueryStringField = this._queryStringFieldTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002AC2 RID: 10946 RVA: 0x000EC91A File Offset: 0x000EB91A
			public override void SetDefaultFocus()
			{
				this._queryStringFieldTextBox.Focus();
			}

			// Token: 0x04001D25 RID: 7461
			private global::System.Windows.Forms.Label _queryStringFieldLabel;

			// Token: 0x04001D26 RID: 7462
			private global::System.Windows.Forms.TextBox _queryStringFieldTextBox;

			// Token: 0x04001D27 RID: 7463
			private global::System.Windows.Forms.Label _defaultValueLabel;

			// Token: 0x04001D28 RID: 7464
			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			// Token: 0x04001D29 RID: 7465
			private LinkLabel _showAdvancedLinkLabel;
		}

		// Token: 0x0200049C RID: 1180
		private sealed class ProfileParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
			// Token: 0x06002AC3 RID: 10947 RVA: 0x000EC928 File Offset: 0x000EB928
			public ProfileParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(400, 400);
				this._propertyNameLabel = new global::System.Windows.Forms.Label();
				this._propertyNameTextBox = new global::System.Windows.Forms.TextBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._showAdvancedLinkLabel = new LinkLabel();
				this._propertyNameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._propertyNameLabel.Location = new Point(0, 0);
				this._propertyNameLabel.Size = new Size(400, 16);
				this._propertyNameLabel.TabIndex = 10;
				this._propertyNameLabel.Text = SR.GetString("ParameterEditorUserControl_ProfilePropertyName");
				this._propertyNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._propertyNameTextBox.Location = new Point(0, 18);
				this._propertyNameTextBox.Size = new Size(400, 20);
				this._propertyNameTextBox.TabIndex = 20;
				this._propertyNameTextBox.TextChanged += this.OnPropertyNameTextBoxTextChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 44);
				this._defaultValueLabel.Size = new Size(400, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("ParameterEditorUserControl_ParameterDefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 62);
				this._defaultValueTextBox.Size = new Size(400, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				this._showAdvancedLinkLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._showAdvancedLinkLabel.Location = new Point(0, 86);
				this._showAdvancedLinkLabel.Size = new Size(400, 16);
				this._showAdvancedLinkLabel.TabIndex = 50;
				this._showAdvancedLinkLabel.TabStop = true;
				this._showAdvancedLinkLabel.Text = SR.GetString("ParameterEditorUserControl_ShowAdvancedProperties");
				this._showAdvancedLinkLabel.Links.Add(new LinkLabel.Link(0, this._showAdvancedLinkLabel.Text.Length));
				this._showAdvancedLinkLabel.LinkClicked += this.OnShowAdvancedLinkLabelLinkClicked;
				base.Controls.Add(this._propertyNameLabel);
				base.Controls.Add(this._propertyNameTextBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				base.Controls.Add(this._showAdvancedLinkLabel);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			// Token: 0x06002AC4 RID: 10948 RVA: 0x000ECBFC File Offset: 0x000EBBFC
			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
				this._propertyNameTextBox.Text = ((ProfileParameter)base.ParameterItem.Parameter).PropertyName;
			}

			// Token: 0x06002AC5 RID: 10949 RVA: 0x000ECC4B File Offset: 0x000EBC4B
			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			// Token: 0x06002AC6 RID: 10950 RVA: 0x000ECC54 File Offset: 0x000EBC54
			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002AC7 RID: 10951 RVA: 0x000ECCA4 File Offset: 0x000EBCA4
			private void OnPropertyNameTextBoxTextChanged(object s, EventArgs e)
			{
				if (((ProfileParameter)base.ParameterItem.Parameter).PropertyName != this._propertyNameTextBox.Text)
				{
					((ProfileParameter)base.ParameterItem.Parameter).PropertyName = this._propertyNameTextBox.Text;
					base.OnParameterChanged();
				}
			}

			// Token: 0x06002AC8 RID: 10952 RVA: 0x000ECCFE File Offset: 0x000EBCFE
			public override void SetDefaultFocus()
			{
				this._propertyNameTextBox.Focus();
			}

			// Token: 0x04001D2A RID: 7466
			private global::System.Windows.Forms.Label _propertyNameLabel;

			// Token: 0x04001D2B RID: 7467
			private global::System.Windows.Forms.TextBox _propertyNameTextBox;

			// Token: 0x04001D2C RID: 7468
			private global::System.Windows.Forms.Label _defaultValueLabel;

			// Token: 0x04001D2D RID: 7469
			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			// Token: 0x04001D2E RID: 7470
			private LinkLabel _showAdvancedLinkLabel;
		}
	}
}
