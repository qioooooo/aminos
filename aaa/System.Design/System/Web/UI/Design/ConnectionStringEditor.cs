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
	// Token: 0x02000324 RID: 804
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ConnectionStringEditor : UITypeEditor
	{
		// Token: 0x06001E30 RID: 7728 RVA: 0x000ABBA4 File Offset: 0x000AABA4
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

		// Token: 0x06001E31 RID: 7729 RVA: 0x000ABD84 File Offset: 0x000AAD84
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

		// Token: 0x06001E32 RID: 7730 RVA: 0x000ABE20 File Offset: 0x000AAE20
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

		// Token: 0x06001E33 RID: 7731 RVA: 0x000ABE4C File Offset: 0x000AAE4C
		protected virtual string GetProviderName(object instance)
		{
			return string.Empty;
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000ABE53 File Offset: 0x000AAE53
		protected virtual void SetProviderName(object instance, DesignerDataConnection connection)
		{
		}

		// Token: 0x04001731 RID: 5937
		private ConnectionStringEditor.ConnectionStringPicker _connectionStringPicker;

		// Token: 0x02000325 RID: 805
		private sealed class ConnectionStringPicker : ListBox
		{
			// Token: 0x06001E36 RID: 7734 RVA: 0x000ABE5D File Offset: 0x000AAE5D
			public ConnectionStringPicker()
			{
				base.BorderStyle = BorderStyle.None;
			}

			// Token: 0x17000541 RID: 1345
			// (get) Token: 0x06001E37 RID: 7735 RVA: 0x000ABE6C File Offset: 0x000AAE6C
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

			// Token: 0x06001E38 RID: 7736 RVA: 0x000ABE90 File Offset: 0x000AAE90
			public void End()
			{
				base.Items.Clear();
				this._edSvc = null;
			}

			// Token: 0x06001E39 RID: 7737 RVA: 0x000ABEA4 File Offset: 0x000AAEA4
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

			// Token: 0x06001E3A RID: 7738 RVA: 0x000ABED7 File Offset: 0x000AAED7
			protected override void OnMouseDown(MouseEventArgs e)
			{
				base.OnMouseDown(e);
				this._mouseClicked = true;
			}

			// Token: 0x06001E3B RID: 7739 RVA: 0x000ABEE7 File Offset: 0x000AAEE7
			protected override void OnMouseUp(MouseEventArgs e)
			{
				base.OnMouseUp(e);
				this._mouseClicked = false;
			}

			// Token: 0x06001E3C RID: 7740 RVA: 0x000ABEF7 File Offset: 0x000AAEF7
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

			// Token: 0x06001E3D RID: 7741 RVA: 0x000ABF2C File Offset: 0x000AAF2C
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

			// Token: 0x04001732 RID: 5938
			private IWindowsFormsEditorService _edSvc;

			// Token: 0x04001733 RID: 5939
			private bool _keyDown;

			// Token: 0x04001734 RID: 5940
			private bool _mouseClicked;

			// Token: 0x02000326 RID: 806
			private sealed class DataConnectionItem
			{
				// Token: 0x06001E3E RID: 7742 RVA: 0x000ABFE0 File Offset: 0x000AAFE0
				public DataConnectionItem()
				{
				}

				// Token: 0x06001E3F RID: 7743 RVA: 0x000ABFE8 File Offset: 0x000AAFE8
				public DataConnectionItem(DesignerDataConnection designerDataConnection)
				{
					this._designerDataConnection = designerDataConnection;
				}

				// Token: 0x17000542 RID: 1346
				// (get) Token: 0x06001E40 RID: 7744 RVA: 0x000ABFF7 File Offset: 0x000AAFF7
				public DesignerDataConnection DesignerDataConnection
				{
					get
					{
						return this._designerDataConnection;
					}
				}

				// Token: 0x06001E41 RID: 7745 RVA: 0x000ABFFF File Offset: 0x000AAFFF
				public override string ToString()
				{
					if (this._designerDataConnection == null)
					{
						return SR.GetString("ConnectionStringEditor_NewConnection");
					}
					return this._designerDataConnection.Name;
				}

				// Token: 0x04001735 RID: 5941
				private DesignerDataConnection _designerDataConnection;
			}
		}

		// Token: 0x02000328 RID: 808
		private sealed class ConnectionStringEditorDialog : DesignerForm
		{
			// Token: 0x06001E4D RID: 7757 RVA: 0x000AC18A File Offset: 0x000AB18A
			public ConnectionStringEditorDialog(IServiceProvider serviceProvider, string providerName)
				: base(serviceProvider)
			{
				this.InitializeComponent();
				this.InitializeUI();
				this._providerName = providerName;
			}

			// Token: 0x17000545 RID: 1349
			// (get) Token: 0x06001E4E RID: 7758 RVA: 0x000AC1A6 File Offset: 0x000AB1A6
			// (set) Token: 0x06001E4F RID: 7759 RVA: 0x000AC1B4 File Offset: 0x000AB1B4
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

			// Token: 0x17000546 RID: 1350
			// (get) Token: 0x06001E50 RID: 7760 RVA: 0x000AC21C File Offset: 0x000AB21C
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

			// Token: 0x17000547 RID: 1351
			// (get) Token: 0x06001E51 RID: 7761 RVA: 0x000AC296 File Offset: 0x000AB296
			protected override string HelpTopic
			{
				get
				{
					return "net.Asp.ConnectionStrings.Editor";
				}
			}

			// Token: 0x06001E52 RID: 7762 RVA: 0x000AC2A0 File Offset: 0x000AB2A0
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

			// Token: 0x06001E53 RID: 7763 RVA: 0x000AC50C File Offset: 0x000AB50C
			private void InitializeUI()
			{
				this._helpLabel.Text = SR.GetString("ConnectionStringEditor_HelpLabel");
				this._okButton.Text = SR.GetString("OK");
				this._cancelButton.Text = SR.GetString("Cancel");
				this.Text = SR.GetString("ConnectionStringEditor_Title");
			}

			// Token: 0x06001E54 RID: 7764 RVA: 0x000AC568 File Offset: 0x000AB568
			private void OnCancelButtonClick(object sender, EventArgs e)
			{
				base.DialogResult = DialogResult.Cancel;
				base.Close();
			}

			// Token: 0x06001E55 RID: 7765 RVA: 0x000AC577 File Offset: 0x000AB577
			private void OnOkButtonClick(object sender, EventArgs e)
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
			}

			// Token: 0x0400173A RID: 5946
			private Label _helpLabel;

			// Token: 0x0400173B RID: 5947
			private Button _okButton;

			// Token: 0x0400173C RID: 5948
			private Button _cancelButton;

			// Token: 0x0400173D RID: 5949
			private TextBox _connectionStringTextBox;

			// Token: 0x0400173E RID: 5950
			private NameValueCollection _defaultConnectionStrings;

			// Token: 0x0400173F RID: 5951
			private string _providerName;
		}
	}
}
