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
	public class ParameterEditorUserControl : UserControl
	{
		public ParameterEditorUserControl(IServiceProvider serviceProvider)
			: this(serviceProvider, null)
		{
		}

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

		internal static string GetControlDefaultValuePropertyName(string controlID, IServiceProvider serviceProvider, Control control)
		{
			Control control2 = ControlHelper.FindControl(serviceProvider, control, controlID);
			if (control2 != null)
			{
				return ParameterEditorUserControl.GetDefaultValuePropertyName(control2);
			}
			return string.Empty;
		}

		private static string GetDefaultValuePropertyName(Control control)
		{
			ControlValuePropertyAttribute controlValuePropertyAttribute = (ControlValuePropertyAttribute)TypeDescriptor.GetAttributes(control)[typeof(ControlValuePropertyAttribute)];
			if (controlValuePropertyAttribute != null && !string.IsNullOrEmpty(controlValuePropertyAttribute.Name))
			{
				return controlValuePropertyAttribute.Name;
			}
			return string.Empty;
		}

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

		private void IgnoreParameterChanges(bool ignoreChanges)
		{
			this._ignoreParameterChangesCount += (ignoreChanges ? 1 : (-1));
			if (this._ignoreParameterChangesCount == 0)
			{
				this.UpdateUI(false);
			}
		}

		private void OnAddParameterButtonClick(object sender, EventArgs e)
		{
			this.AddParameter(new Parameter("newparameter"));
		}

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

		private void OnParametersListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateUI(false);
		}

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

		public void SetAllowCollectionChanges(bool allowChanges)
		{
			this._moveUpButton.Visible = allowChanges;
			this._moveDownButton.Visible = allowChanges;
			this._deleteParameterButton.Visible = allowChanges;
			this._addParameterButton.Visible = allowChanges;
		}

		private void ToggleAdvancedMode(object sender, EventArgs e)
		{
			this._inAdvancedMode = !this._inAdvancedMode;
			this.UpdateUI(true);
		}

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

		private static readonly object EventParametersChanged = new object();

		private global::System.Windows.Forms.Label _parametersLabel;

		private ListView _parametersListView;

		private AutoSizeComboBox _parameterTypeComboBox;

		private ColumnHeader _nameColumnHeader;

		private ColumnHeader _valueColumnHeader;

		private global::System.Windows.Forms.Button _moveUpButton;

		private global::System.Windows.Forms.Button _moveDownButton;

		private global::System.Windows.Forms.Button _deleteParameterButton;

		private global::System.Windows.Forms.Button _addParameterButton;

		private global::System.Windows.Forms.Panel _addButtonPanel;

		private global::System.Windows.Forms.Label _sourceLabel;

		private global::System.Windows.Forms.Panel _editorPanel;

		private ListDictionary _parameterTypes;

		private IServiceProvider _serviceProvider;

		private ParameterEditorUserControl.ParameterEditor _parameterEditor;

		private bool _inAdvancedMode;

		private int _ignoreParameterChangesCount;

		private ParameterEditorUserControl.AdvancedParameterEditor _advancedParameterEditor;

		private ParameterEditorUserControl.ControlParameterEditor _controlParameterEditor;

		private ParameterEditorUserControl.CookieParameterEditor _cookieParameterEditor;

		private ParameterEditorUserControl.FormParameterEditor _formParameterEditor;

		private ParameterEditorUserControl.QueryStringParameterEditor _queryStringParameterEditor;

		private ParameterEditorUserControl.SessionParameterEditor _sessionParameterEditor;

		private ParameterEditorUserControl.StaticParameterEditor _staticParameterEditor;

		private ParameterEditorUserControl.ProfileParameterEditor _profileParameterEditor;

		private Control _control;

		internal sealed class ControlItem
		{
			public ControlItem(string controlID, string propertyName)
			{
				this._controlID = controlID;
				this._propertyName = propertyName;
			}

			public string ControlID
			{
				get
				{
					return this._controlID;
				}
			}

			public string PropertyName
			{
				get
				{
					return this._propertyName;
				}
			}

			private static bool IsValidComponent(IComponent component)
			{
				Control control = component as Control;
				return control != null && !string.IsNullOrEmpty(control.ID);
			}

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

			public override string ToString()
			{
				return this._controlID;
			}

			private string _controlID;

			private string _propertyName;
		}

		private class ParameterListViewItem : ListViewItem
		{
			public ParameterListViewItem(Parameter parameter)
			{
				this._parameter = parameter;
				this._isConfigured = true;
			}

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

			public bool IsConfigured
			{
				get
				{
					return this._isConfigured;
				}
			}

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

			private Parameter _parameter;

			private bool _isConfigured;
		}

		private class PropertyGridSite : ISite, IServiceProvider
		{
			public PropertyGridSite(IServiceProvider sp, IComponent comp)
			{
				this._sp = sp;
				this._comp = comp;
			}

			public IComponent Component
			{
				get
				{
					return this._comp;
				}
			}

			public IContainer Container
			{
				get
				{
					return null;
				}
			}

			public bool DesignMode
			{
				get
				{
					return false;
				}
			}

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

			private IServiceProvider _sp;

			private IComponent _comp;

			private bool _inGetService;
		}

		private abstract class ParameterEditor : global::System.Windows.Forms.Panel
		{
			protected ParameterEditor(IServiceProvider serviceProvider)
			{
				this._serviceProvider = serviceProvider;
			}

			protected ParameterEditorUserControl.ParameterListViewItem ParameterItem
			{
				get
				{
					return this._parameterItem;
				}
			}

			protected IServiceProvider ServiceProvider
			{
				get
				{
					return this._serviceProvider;
				}
			}

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

			public virtual void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				this._parameterItem = parameterItem;
			}

			protected void OnParameterChanged()
			{
				this.ParameterItem.Refresh();
				EventHandler eventHandler = base.Events[ParameterEditorUserControl.ParameterEditor.EventParameterChanged] as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(this, EventArgs.Empty);
				}
			}

			protected void OnRequestModeChange()
			{
				EventHandler eventHandler = base.Events[ParameterEditorUserControl.ParameterEditor.EventRequestModeChange] as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(this, EventArgs.Empty);
				}
			}

			public virtual void SetDefaultFocus()
			{
			}

			private static readonly object EventParameterChanged = new object();

			private static readonly object EventRequestModeChange = new object();

			private IServiceProvider _serviceProvider;

			private ParameterEditorUserControl.ParameterListViewItem _parameterItem;
		}

		private sealed class AdvancedParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
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

			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._parameterPropertyGrid.SelectedObject = base.ParameterItem.Parameter;
			}

			private void OnHideAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

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

			public override void SetDefaultFocus()
			{
				this._parameterPropertyGrid.Focus();
			}

			private global::System.Windows.Forms.Label _advancedlabel;

			private PropertyGrid _parameterPropertyGrid;

			private LinkLabel _hideAdvancedLinkLabel;

			private Control _control;
		}

		private sealed class ControlParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
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

			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

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

			public override void SetDefaultFocus()
			{
				this._controlIDComboBox.Focus();
			}

			private global::System.Windows.Forms.Label _controlIDLabel;

			private AutoSizeComboBox _controlIDComboBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private LinkLabel _showAdvancedLinkLabel;

			private Control _control;
		}

		private sealed class CookieParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
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

			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
				this._cookieNameTextBox.Text = ((CookieParameter)base.ParameterItem.Parameter).CookieName;
			}

			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			private void OnCookieNameTextBoxTextChanged(object s, EventArgs e)
			{
				if (((CookieParameter)base.ParameterItem.Parameter).CookieName != this._cookieNameTextBox.Text)
				{
					((CookieParameter)base.ParameterItem.Parameter).CookieName = this._cookieNameTextBox.Text;
					base.OnParameterChanged();
				}
			}

			public override void SetDefaultFocus()
			{
				this._cookieNameTextBox.Focus();
			}

			private global::System.Windows.Forms.Label _cookieNameLabel;

			private global::System.Windows.Forms.TextBox _cookieNameTextBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private LinkLabel _showAdvancedLinkLabel;
		}

		private sealed class FormParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
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

			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
				this._formFieldTextBox.Text = ((FormParameter)base.ParameterItem.Parameter).FormField;
			}

			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			private void OnFormFieldTextBoxTextChanged(object s, EventArgs e)
			{
				if (((FormParameter)base.ParameterItem.Parameter).FormField != this._formFieldTextBox.Text)
				{
					((FormParameter)base.ParameterItem.Parameter).FormField = this._formFieldTextBox.Text;
					base.OnParameterChanged();
				}
			}

			public override void SetDefaultFocus()
			{
				this._formFieldTextBox.Focus();
			}

			private global::System.Windows.Forms.Label _formFieldLabel;

			private global::System.Windows.Forms.TextBox _formFieldTextBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private LinkLabel _showAdvancedLinkLabel;
		}

		private sealed class SessionParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
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

			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
				this._sessionFieldTextBox.Text = ((SessionParameter)base.ParameterItem.Parameter).SessionField;
			}

			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			private void OnSessionFieldTextBoxTextChanged(object s, EventArgs e)
			{
				if (((SessionParameter)base.ParameterItem.Parameter).SessionField != this._sessionFieldTextBox.Text)
				{
					((SessionParameter)base.ParameterItem.Parameter).SessionField = this._sessionFieldTextBox.Text;
					base.OnParameterChanged();
				}
			}

			public override void SetDefaultFocus()
			{
				this._sessionFieldTextBox.Focus();
			}

			private global::System.Windows.Forms.Label _sessionFieldLabel;

			private global::System.Windows.Forms.TextBox _sessionFieldTextBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private LinkLabel _showAdvancedLinkLabel;
		}

		private sealed class StaticParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
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

			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
			}

			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			public override void SetDefaultFocus()
			{
				this._defaultValueTextBox.Focus();
			}

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private LinkLabel _showAdvancedLinkLabel;
		}

		private sealed class QueryStringParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
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

			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
				this._queryStringFieldTextBox.Text = ((QueryStringParameter)base.ParameterItem.Parameter).QueryStringField;
			}

			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			private void OnQueryStringFieldTextBoxTextChanged(object s, EventArgs e)
			{
				if (((QueryStringParameter)base.ParameterItem.Parameter).QueryStringField != this._queryStringFieldTextBox.Text)
				{
					((QueryStringParameter)base.ParameterItem.Parameter).QueryStringField = this._queryStringFieldTextBox.Text;
					base.OnParameterChanged();
				}
			}

			public override void SetDefaultFocus()
			{
				this._queryStringFieldTextBox.Focus();
			}

			private global::System.Windows.Forms.Label _queryStringFieldLabel;

			private global::System.Windows.Forms.TextBox _queryStringFieldTextBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private LinkLabel _showAdvancedLinkLabel;
		}

		private sealed class ProfileParameterEditor : ParameterEditorUserControl.ParameterEditor
		{
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

			public override void InitializeParameter(ParameterEditorUserControl.ParameterListViewItem parameterItem)
			{
				base.InitializeParameter(parameterItem);
				this._defaultValueTextBox.Text = base.ParameterItem.Parameter.DefaultValue;
				this._propertyNameTextBox.Text = ((ProfileParameter)base.ParameterItem.Parameter).PropertyName;
			}

			private void OnShowAdvancedLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
			{
				base.OnRequestModeChange();
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				if (base.ParameterItem.Parameter.DefaultValue != this._defaultValueTextBox.Text)
				{
					base.ParameterItem.Parameter.DefaultValue = this._defaultValueTextBox.Text;
					base.OnParameterChanged();
				}
			}

			private void OnPropertyNameTextBoxTextChanged(object s, EventArgs e)
			{
				if (((ProfileParameter)base.ParameterItem.Parameter).PropertyName != this._propertyNameTextBox.Text)
				{
					((ProfileParameter)base.ParameterItem.Parameter).PropertyName = this._propertyNameTextBox.Text;
					base.OnParameterChanged();
				}
			}

			public override void SetDefaultFocus()
			{
				this._propertyNameTextBox.Focus();
			}

			private global::System.Windows.Forms.Label _propertyNameLabel;

			private global::System.Windows.Forms.TextBox _propertyNameTextBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private LinkLabel _showAdvancedLinkLabel;
		}
	}
}
