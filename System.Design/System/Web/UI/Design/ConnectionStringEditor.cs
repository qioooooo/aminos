using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Data;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.UI.Design.Util;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ConnectionStringEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			Control control = context.Instance as Control;
			if (provider != null)
			{
				IDataEnvironment dataEnvironment = (IDataEnvironment)provider.GetService(typeof(IDataEnvironment));
				if (dataEnvironment != null)
				{
					IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
					if (windowsFormsEditorService != null && context.Instance != null)
					{
						if (this._connectionStringPicker == null)
						{
							this._connectionStringPicker = new ConnectionStringEditor.ConnectionStringPicker();
						}
						string text = (string)value;
						ExpressionEditor expressionEditor = ExpressionEditor.GetExpressionEditor(typeof(ConnectionStringsExpressionBuilder), provider);
						if (expressionEditor != null)
						{
							string expressionPrefix = expressionEditor.ExpressionPrefix;
							DesignerDataConnection currentConnection = ConnectionStringEditor.GetCurrentConnection(control, context.PropertyDescriptor.Name, text, expressionPrefix);
							this._connectionStringPicker.Start(windowsFormsEditorService, dataEnvironment.Connections, currentConnection);
							windowsFormsEditorService.DropDownControl(this._connectionStringPicker);
							if (this._connectionStringPicker.SelectedItem != null)
							{
								DesignerDataConnection designerDataConnection = this._connectionStringPicker.SelectedConnection;
								if (designerDataConnection == null)
								{
									designerDataConnection = dataEnvironment.BuildConnection(UIServiceHelper.GetDialogOwnerWindow(provider), null);
								}
								if (designerDataConnection != null)
								{
									if (designerDataConnection.IsConfigured)
									{
										ExpressionBindingCollection expressions = ((IExpressionsAccessor)control).Expressions;
										expressions.Add(new ExpressionBinding(context.PropertyDescriptor.Name, context.PropertyDescriptor.PropertyType, expressionPrefix, designerDataConnection.Name));
										this.SetProviderName(context.Instance, designerDataConnection);
										IComponentChangeService componentChangeService = (IComponentChangeService)provider.GetService(typeof(IComponentChangeService));
										if (componentChangeService != null)
										{
											componentChangeService.OnComponentChanged(control, null, null, null);
										}
									}
									else
									{
										value = designerDataConnection.ConnectionString;
										this.SetProviderName(context.Instance, designerDataConnection);
									}
								}
							}
							this._connectionStringPicker.End();
						}
					}
					return value;
				}
			}
			string providerName = this.GetProviderName(context.Instance);
			ConnectionStringEditor.ConnectionStringEditorDialog connectionStringEditorDialog = new ConnectionStringEditor.ConnectionStringEditorDialog(provider, providerName);
			connectionStringEditorDialog.ConnectionString = (string)value;
			DialogResult dialogResult = UIServiceHelper.ShowDialog(provider, connectionStringEditorDialog);
			if (dialogResult == DialogResult.OK)
			{
				value = connectionStringEditorDialog.ConnectionString;
			}
			return value;
		}

		private static DesignerDataConnection GetCurrentConnection(Control control, string propertyName, string connectionString, string expressionPrefix)
		{
			ExpressionBindingCollection expressions = ((IExpressionsAccessor)control).Expressions;
			ExpressionBinding expressionBinding = expressions[propertyName];
			string text = "." + "ConnectionString".ToLowerInvariant();
			DesignerDataConnection designerDataConnection;
			if (expressionBinding != null && string.Equals(expressionBinding.ExpressionPrefix, expressionPrefix, StringComparison.OrdinalIgnoreCase))
			{
				string expression = expressionBinding.Expression;
				if (expression.ToLowerInvariant().EndsWith(text, StringComparison.Ordinal))
				{
					expression.Substring(0, expression.Length - text.Length);
				}
				designerDataConnection = new DesignerDataConnection(expressionBinding.Expression, string.Empty, connectionString, true);
			}
			else
			{
				designerDataConnection = new DesignerDataConnection(string.Empty, string.Empty, connectionString, false);
			}
			return designerDataConnection;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context != null)
			{
				IDataEnvironment dataEnvironment = (IDataEnvironment)context.GetService(typeof(IDataEnvironment));
				if (dataEnvironment != null)
				{
					return UITypeEditorEditStyle.DropDown;
				}
			}
			return UITypeEditorEditStyle.Modal;
		}

		protected virtual string GetProviderName(object instance)
		{
			return string.Empty;
		}

		protected virtual void SetProviderName(object instance, DesignerDataConnection connection)
		{
		}

		private ConnectionStringEditor.ConnectionStringPicker _connectionStringPicker;

		private sealed class ConnectionStringPicker : ListBox
		{
			public ConnectionStringPicker()
			{
				base.BorderStyle = BorderStyle.None;
			}

			public DesignerDataConnection SelectedConnection
			{
				get
				{
					ConnectionStringEditor.ConnectionStringPicker.DataConnectionItem dataConnectionItem = base.SelectedItem as ConnectionStringEditor.ConnectionStringPicker.DataConnectionItem;
					if (dataConnectionItem != null)
					{
						return dataConnectionItem.DesignerDataConnection;
					}
					return null;
				}
			}

			public void End()
			{
				base.Items.Clear();
				this._edSvc = null;
			}

			protected override void OnKeyUp(KeyEventArgs e)
			{
				base.OnKeyUp(e);
				this._keyDown = true;
				this._mouseClicked = false;
				if (e.KeyData == Keys.Return)
				{
					this._keyDown = false;
					this._edSvc.CloseDropDown();
				}
			}

			protected override void OnMouseDown(MouseEventArgs e)
			{
				base.OnMouseDown(e);
				this._mouseClicked = true;
			}

			protected override void OnMouseUp(MouseEventArgs e)
			{
				base.OnMouseUp(e);
				this._mouseClicked = false;
			}

			protected override void OnSelectedIndexChanged(EventArgs e)
			{
				base.OnSelectedIndexChanged(e);
				if (this._mouseClicked && !this._keyDown)
				{
					this._mouseClicked = false;
					this._keyDown = false;
					this._edSvc.CloseDropDown();
				}
			}

			public void Start(IWindowsFormsEditorService edSvc, ICollection connections, DesignerDataConnection currentConnection)
			{
				this._edSvc = edSvc;
				base.Items.Clear();
				object obj = null;
				foreach (object obj2 in connections)
				{
					DesignerDataConnection designerDataConnection = (DesignerDataConnection)obj2;
					ConnectionStringEditor.ConnectionStringPicker.DataConnectionItem dataConnectionItem = new ConnectionStringEditor.ConnectionStringPicker.DataConnectionItem(designerDataConnection);
					if (designerDataConnection.ConnectionString == currentConnection.ConnectionString && designerDataConnection.IsConfigured == currentConnection.IsConfigured)
					{
						obj = dataConnectionItem;
					}
					base.Items.Add(dataConnectionItem);
				}
				base.Items.Add(new ConnectionStringEditor.ConnectionStringPicker.DataConnectionItem());
				base.SelectedItem = obj;
			}

			private IWindowsFormsEditorService _edSvc;

			private bool _keyDown;

			private bool _mouseClicked;

			private sealed class DataConnectionItem
			{
				public DataConnectionItem()
				{
				}

				public DataConnectionItem(DesignerDataConnection designerDataConnection)
				{
					this._designerDataConnection = designerDataConnection;
				}

				public DesignerDataConnection DesignerDataConnection
				{
					get
					{
						return this._designerDataConnection;
					}
				}

				public override string ToString()
				{
					if (this._designerDataConnection == null)
					{
						return SR.GetString("ConnectionStringEditor_NewConnection");
					}
					return this._designerDataConnection.Name;
				}

				private DesignerDataConnection _designerDataConnection;
			}
		}

		private sealed class ConnectionStringEditorDialog : DesignerForm
		{
			public ConnectionStringEditorDialog(IServiceProvider serviceProvider, string providerName)
				: base(serviceProvider)
			{
				this.InitializeComponent();
				this.InitializeUI();
				this._providerName = providerName;
			}

			public string ConnectionString
			{
				get
				{
					return this._connectionStringTextBox.Text;
				}
				set
				{
					if (!string.IsNullOrEmpty(value))
					{
						this._connectionStringTextBox.Text = value;
						return;
					}
					if (string.IsNullOrEmpty(this._providerName))
					{
						this._connectionStringTextBox.Text = this.DefaultConnectionStrings["System.Data.SqlClient"];
						return;
					}
					this._connectionStringTextBox.Text = this.DefaultConnectionStrings[this._providerName];
				}
			}

			private NameValueCollection DefaultConnectionStrings
			{
				get
				{
					if (this._defaultConnectionStrings == null)
					{
						this._defaultConnectionStrings = new NameValueCollection();
						this._defaultConnectionStrings.Add("System.Data.SqlClient", "server=(local); trusted_connection=true; database=[database]");
						this._defaultConnectionStrings.Add("System.Data.Odbc", "Driver=[driver]; Server=[server]; Database=[database]; Uid=[username]; Pwd=[password]");
						this._defaultConnectionStrings.Add("System.Data.OleDb", "Provider=[provider]; Data Source=[server]; Initial Catalog=[database]; User Id=[username]; Password=[password]");
						this._defaultConnectionStrings.Add("System.Data.OracleClient", "Data Source=Oracle8i; Integrated Security=SSPI");
					}
					return this._defaultConnectionStrings;
				}
			}

			protected override string HelpTopic
			{
				get
				{
					return "net.Asp.ConnectionStrings.Editor";
				}
			}

			private void InitializeComponent()
			{
				this._helpLabel = new Label();
				this._okButton = new Button();
				this._cancelButton = new Button();
				this._connectionStringTextBox = new TextBox();
				base.SuspendLayout();
				this._helpLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._helpLabel.Location = new Point(12, 12);
				this._helpLabel.Name = "_helpLabel";
				this._helpLabel.Size = new Size(369, 16);
				this._helpLabel.TabIndex = 10;
				this._okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
				this._okButton.Location = new Point(228, 233);
				this._okButton.Name = "_okButton";
				this._okButton.TabIndex = 30;
				this._okButton.Click += this.OnOkButtonClick;
				this._cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
				this._cancelButton.DialogResult = DialogResult.Cancel;
				this._cancelButton.Location = new Point(310, 233);
				this._cancelButton.Name = "_cancelButton";
				this._cancelButton.TabIndex = 40;
				this._cancelButton.Click += this.OnCancelButtonClick;
				this._connectionStringTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
				this._connectionStringTextBox.Location = new Point(12, 36);
				this._connectionStringTextBox.Multiline = true;
				this._connectionStringTextBox.Name = "_connectionStringTextBox";
				this._connectionStringTextBox.Size = new Size(369, 190);
				this._connectionStringTextBox.TabIndex = 20;
				base.AcceptButton = this._okButton;
				this.AutoSize = true;
				base.CancelButton = this._cancelButton;
				base.ClientSize = new Size(392, 266);
				base.Controls.Add(this._connectionStringTextBox);
				base.Controls.Add(this._cancelButton);
				base.Controls.Add(this._okButton);
				base.Controls.Add(this._helpLabel);
				this.MinimumSize = new Size(400, 300);
				base.Name = "Form1";
				base.SizeGripStyle = SizeGripStyle.Hide;
				base.InitializeForm();
				base.ResumeLayout(false);
				base.PerformLayout();
			}

			private void InitializeUI()
			{
				this._helpLabel.Text = SR.GetString("ConnectionStringEditor_HelpLabel");
				this._okButton.Text = SR.GetString("OK");
				this._cancelButton.Text = SR.GetString("Cancel");
				this.Text = SR.GetString("ConnectionStringEditor_Title");
			}

			private void OnCancelButtonClick(object sender, EventArgs e)
			{
				base.DialogResult = DialogResult.Cancel;
				base.Close();
			}

			private void OnOkButtonClick(object sender, EventArgs e)
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
			}

			private Label _helpLabel;

			private Button _okButton;

			private Button _cancelButton;

			private TextBox _connectionStringTextBox;

			private NameValueCollection _defaultConnectionStrings;

			private string _providerName;
		}
	}
}
