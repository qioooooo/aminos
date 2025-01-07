using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class LinkAreaEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				IHelpService helpService = (IHelpService)provider.GetService(typeof(IHelpService));
				if (windowsFormsEditorService != null)
				{
					if (this.linkAreaUI == null)
					{
						this.linkAreaUI = new LinkAreaEditor.LinkAreaUI(this, helpService);
					}
					string text = string.Empty;
					PropertyDescriptor propertyDescriptor = null;
					if (context != null && context.Instance != null)
					{
						propertyDescriptor = TypeDescriptor.GetProperties(context.Instance)["Text"];
						if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string))
						{
							text = (string)propertyDescriptor.GetValue(context.Instance);
						}
					}
					string text2 = text;
					this.linkAreaUI.SampleText = text;
					this.linkAreaUI.Start(windowsFormsEditorService, value);
					if (windowsFormsEditorService.ShowDialog(this.linkAreaUI) == DialogResult.OK)
					{
						value = this.linkAreaUI.Value;
						text = this.linkAreaUI.SampleText;
						if (!text2.Equals(text) && propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string))
						{
							propertyDescriptor.SetValue(context.Instance, text);
						}
					}
					this.linkAreaUI.End();
				}
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		private LinkAreaEditor.LinkAreaUI linkAreaUI;

		internal class LinkAreaUI : Form
		{
			public LinkAreaUI(LinkAreaEditor editor, IHelpService helpService)
			{
				this.editor = editor;
				this.helpService = helpService;
				this.InitializeComponent();
			}

			public string SampleText
			{
				get
				{
					return this.sampleEdit.Text;
				}
				set
				{
					this.sampleEdit.Text = value;
					this.UpdateSelection();
				}
			}

			public object Value
			{
				get
				{
					return this.value;
				}
			}

			public void End()
			{
				this.edSvc = null;
				this.value = null;
			}

			private void InitializeComponent()
			{
				ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LinkAreaEditor));
				this.caption = new Label();
				this.sampleEdit = new TextBox();
				this.okButton = new Button();
				this.cancelButton = new Button();
				this.okCancelTableLayoutPanel = new TableLayoutPanel();
				this.okCancelTableLayoutPanel.SuspendLayout();
				base.SuspendLayout();
				this.okButton.Click += this.okButton_click;
				componentResourceManager.ApplyResources(this.caption, "caption");
				this.caption.Margin = new Padding(3, 1, 3, 0);
				this.caption.Name = "caption";
				componentResourceManager.ApplyResources(this.sampleEdit, "sampleEdit");
				this.sampleEdit.Margin = new Padding(3, 2, 3, 3);
				this.sampleEdit.Name = "sampleEdit";
				this.sampleEdit.HideSelection = false;
				this.sampleEdit.ScrollBars = ScrollBars.Vertical;
				componentResourceManager.ApplyResources(this.okButton, "okButton");
				this.okButton.DialogResult = DialogResult.OK;
				this.okButton.Margin = new Padding(0, 0, 2, 0);
				this.okButton.Name = "okButton";
				componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
				this.cancelButton.DialogResult = DialogResult.Cancel;
				this.cancelButton.Margin = new Padding(3, 0, 0, 0);
				this.cancelButton.Name = "cancelButton";
				componentResourceManager.ApplyResources(this.okCancelTableLayoutPanel, "okCancelTableLayoutPanel");
				this.okCancelTableLayoutPanel.ColumnCount = 2;
				this.okCancelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.okCancelTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.okCancelTableLayoutPanel.Controls.Add(this.okButton, 0, 0);
				this.okCancelTableLayoutPanel.Controls.Add(this.cancelButton, 1, 0);
				this.okCancelTableLayoutPanel.Margin = new Padding(3, 1, 3, 3);
				this.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel";
				this.okCancelTableLayoutPanel.RowCount = 1;
				this.okCancelTableLayoutPanel.RowStyles.Add(new RowStyle());
				this.okCancelTableLayoutPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this, "$this");
				base.AutoScaleMode = AutoScaleMode.Font;
				base.CancelButton = this.cancelButton;
				base.Controls.Add(this.okCancelTableLayoutPanel);
				base.Controls.Add(this.sampleEdit);
				base.Controls.Add(this.caption);
				base.HelpButton = true;
				base.MaximizeBox = false;
				base.MinimizeBox = false;
				base.Name = "LinkAreaEditor";
				base.ShowIcon = false;
				base.ShowInTaskbar = false;
				base.HelpButtonClicked += this.LinkAreaEditor_HelpButtonClicked;
				this.okCancelTableLayoutPanel.ResumeLayout(false);
				this.okCancelTableLayoutPanel.PerformLayout();
				base.ResumeLayout(false);
				base.PerformLayout();
			}

			private void okButton_click(object sender, EventArgs e)
			{
				this.value = new LinkArea(this.sampleEdit.SelectionStart, this.sampleEdit.SelectionLength);
			}

			private string HelpTopic
			{
				get
				{
					return "net.ComponentModel.LinkAreaEditor";
				}
			}

			private void ShowHelp()
			{
				if (this.helpService != null)
				{
					this.helpService.ShowHelpFromKeyword(this.HelpTopic);
				}
			}

			private void LinkAreaEditor_HelpButtonClicked(object sender, CancelEventArgs e)
			{
				e.Cancel = true;
				this.ShowHelp();
			}

			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.value = value;
				this.UpdateSelection();
				base.ActiveControl = this.sampleEdit;
			}

			private void UpdateSelection()
			{
				if (this.value is LinkArea)
				{
					LinkArea linkArea = (LinkArea)this.value;
					try
					{
						this.sampleEdit.SelectionStart = linkArea.Start;
						this.sampleEdit.SelectionLength = linkArea.Length;
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
					}
					catch
					{
					}
				}
			}

			private Label caption = new Label();

			private TextBox sampleEdit = new TextBox();

			private Button okButton = new Button();

			private Button cancelButton = new Button();

			private TableLayoutPanel okCancelTableLayoutPanel;

			private LinkAreaEditor editor;

			private IWindowsFormsEditorService edSvc;

			private object value;

			private IHelpService helpService;
		}
	}
}
