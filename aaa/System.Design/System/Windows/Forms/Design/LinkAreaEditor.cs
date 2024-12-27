using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200025B RID: 603
	internal class LinkAreaEditor : UITypeEditor
	{
		// Token: 0x060016EB RID: 5867 RVA: 0x00076200 File Offset: 0x00075200
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

		// Token: 0x060016EC RID: 5868 RVA: 0x00076326 File Offset: 0x00075326
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x04001307 RID: 4871
		private LinkAreaEditor.LinkAreaUI linkAreaUI;

		// Token: 0x0200025C RID: 604
		internal class LinkAreaUI : Form
		{
			// Token: 0x060016EE RID: 5870 RVA: 0x00076334 File Offset: 0x00075334
			public LinkAreaUI(LinkAreaEditor editor, IHelpService helpService)
			{
				this.editor = editor;
				this.helpService = helpService;
				this.InitializeComponent();
			}

			// Token: 0x170003F1 RID: 1009
			// (get) Token: 0x060016EF RID: 5871 RVA: 0x00076387 File Offset: 0x00075387
			// (set) Token: 0x060016F0 RID: 5872 RVA: 0x00076394 File Offset: 0x00075394
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

			// Token: 0x170003F2 RID: 1010
			// (get) Token: 0x060016F1 RID: 5873 RVA: 0x000763A8 File Offset: 0x000753A8
			public object Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x060016F2 RID: 5874 RVA: 0x000763B0 File Offset: 0x000753B0
			public void End()
			{
				this.edSvc = null;
				this.value = null;
			}

			// Token: 0x060016F3 RID: 5875 RVA: 0x000763C0 File Offset: 0x000753C0
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

			// Token: 0x060016F4 RID: 5876 RVA: 0x000766D7 File Offset: 0x000756D7
			private void okButton_click(object sender, EventArgs e)
			{
				this.value = new LinkArea(this.sampleEdit.SelectionStart, this.sampleEdit.SelectionLength);
			}

			// Token: 0x170003F3 RID: 1011
			// (get) Token: 0x060016F5 RID: 5877 RVA: 0x000766FF File Offset: 0x000756FF
			private string HelpTopic
			{
				get
				{
					return "net.ComponentModel.LinkAreaEditor";
				}
			}

			// Token: 0x060016F6 RID: 5878 RVA: 0x00076706 File Offset: 0x00075706
			private void ShowHelp()
			{
				if (this.helpService != null)
				{
					this.helpService.ShowHelpFromKeyword(this.HelpTopic);
				}
			}

			// Token: 0x060016F7 RID: 5879 RVA: 0x00076721 File Offset: 0x00075721
			private void LinkAreaEditor_HelpButtonClicked(object sender, CancelEventArgs e)
			{
				e.Cancel = true;
				this.ShowHelp();
			}

			// Token: 0x060016F8 RID: 5880 RVA: 0x00076730 File Offset: 0x00075730
			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.value = value;
				this.UpdateSelection();
				base.ActiveControl = this.sampleEdit;
			}

			// Token: 0x060016F9 RID: 5881 RVA: 0x00076754 File Offset: 0x00075754
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

			// Token: 0x04001308 RID: 4872
			private Label caption = new Label();

			// Token: 0x04001309 RID: 4873
			private TextBox sampleEdit = new TextBox();

			// Token: 0x0400130A RID: 4874
			private Button okButton = new Button();

			// Token: 0x0400130B RID: 4875
			private Button cancelButton = new Button();

			// Token: 0x0400130C RID: 4876
			private TableLayoutPanel okCancelTableLayoutPanel;

			// Token: 0x0400130D RID: 4877
			private LinkAreaEditor editor;

			// Token: 0x0400130E RID: 4878
			private IWindowsFormsEditorService edSvc;

			// Token: 0x0400130F RID: 4879
			private object value;

			// Token: 0x04001310 RID: 4880
			private IHelpService helpService;
		}
	}
}
