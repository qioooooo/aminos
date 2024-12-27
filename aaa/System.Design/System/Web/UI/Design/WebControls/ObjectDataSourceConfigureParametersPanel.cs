using System;
using System.Design;
using System.Drawing;
using System.Reflection;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000485 RID: 1157
	internal sealed class ObjectDataSourceConfigureParametersPanel : WizardPanel
	{
		// Token: 0x060029F9 RID: 10745 RVA: 0x000E6AD9 File Offset: 0x000E5AD9
		public ObjectDataSourceConfigureParametersPanel(ObjectDataSourceDesigner objectDataSourceDesigner)
		{
			this._objectDataSourceDesigner = objectDataSourceDesigner;
			this._objectDataSource = (ObjectDataSource)this._objectDataSourceDesigner.Component;
			this.InitializeComponent();
			this.InitializeUI();
			this._parameterEditorUserControl.SetAllowCollectionChanges(false);
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x000E6B18 File Offset: 0x000E5B18
		private void InitializeComponent()
		{
			this._helpLabel = new global::System.Windows.Forms.Label();
			this._parameterEditorUserControl = new ParameterEditorUserControl(this._objectDataSource.Site, this._objectDataSource);
			this._signatureLabel = new global::System.Windows.Forms.Label();
			this._signatureTextBox = new global::System.Windows.Forms.TextBox();
			base.SuspendLayout();
			this._helpLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._helpLabel.Location = new Point(0, 0);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new Size(544, 45);
			this._helpLabel.TabIndex = 10;
			this._parameterEditorUserControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._parameterEditorUserControl.Location = new Point(0, 38);
			this._parameterEditorUserControl.Name = "_parameterEditorUserControl";
			this._parameterEditorUserControl.Size = new Size(544, 152);
			this._parameterEditorUserControl.TabIndex = 20;
			this._parameterEditorUserControl.ParametersChanged += this.OnParameterEditorUserControlParametersChanged;
			this._signatureLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._signatureLabel.Location = new Point(0, 214);
			this._signatureLabel.Name = "_signatureLabel";
			this._signatureLabel.Size = new Size(544, 16);
			this._signatureLabel.TabIndex = 30;
			this._signatureTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._signatureTextBox.BackColor = SystemColors.Control;
			this._signatureTextBox.Location = new Point(0, 232);
			this._signatureTextBox.Multiline = true;
			this._signatureTextBox.Name = "_signatureTextBox";
			this._signatureTextBox.ReadOnly = true;
			this._signatureTextBox.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this._signatureTextBox.Size = new Size(544, 42);
			this._signatureTextBox.TabIndex = 40;
			this._signatureTextBox.Text = "";
			base.Controls.Add(this._signatureTextBox);
			base.Controls.Add(this._signatureLabel);
			base.Controls.Add(this._parameterEditorUserControl);
			base.Controls.Add(this._helpLabel);
			base.Name = "ObjectDataSourceConfigureParametersPanel";
			base.Size = new Size(544, 274);
			base.ResumeLayout(false);
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x000E6D88 File Offset: 0x000E5D88
		public void InitializeParameters(ParameterCollection selectParameters)
		{
			Parameter[] array = new Parameter[selectParameters.Count];
			selectParameters.CopyTo(array, 0);
			this._parameterEditorUserControl.AddParameters(array);
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x000E6DB5 File Offset: 0x000E5DB5
		private void InitializeUI()
		{
			base.Caption = SR.GetString("ObjectDataSourceConfigureParametersPanel_PanelCaption");
			this._helpLabel.Text = SR.GetString("ObjectDataSourceConfigureParametersPanel_HelpLabel");
			this._signatureLabel.Text = SR.GetString("ObjectDataSource_General_MethodSignatureLabel");
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x000E6DF4 File Offset: 0x000E5DF4
		protected internal override void OnComplete()
		{
			this._objectDataSource.SelectParameters.Clear();
			Parameter[] parameters = this._parameterEditorUserControl.GetParameters();
			foreach (Parameter parameter in parameters)
			{
				this._objectDataSource.SelectParameters.Add(parameter);
			}
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x000E6E43 File Offset: 0x000E5E43
		public override bool OnNext()
		{
			return true;
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x000E6E46 File Offset: 0x000E5E46
		private void OnParameterEditorUserControlParametersChanged(object sender, EventArgs e)
		{
			this.UpdateUI();
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x000E6E4E File Offset: 0x000E5E4E
		public override void OnPrevious()
		{
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x000E6E50 File Offset: 0x000E5E50
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				base.ParentWizard.NextButton.Enabled = false;
				this.UpdateUI();
			}
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000E6E78 File Offset: 0x000E5E78
		public void ResetUI()
		{
			this._parameterEditorUserControl.ClearParameters();
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x000E6E88 File Offset: 0x000E5E88
		public void SetMethod(MethodInfo selectMethodInfo)
		{
			this._signatureTextBox.Text = ObjectDataSourceMethodEditor.GetMethodSignature(selectMethodInfo);
			Parameter[] array = ObjectDataSourceDesigner.MergeParameters(this._parameterEditorUserControl.GetParameters(), selectMethodInfo);
			this._parameterEditorUserControl.ClearParameters();
			this._parameterEditorUserControl.AddParameters(array);
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000E6ECF File Offset: 0x000E5ECF
		private void UpdateUI()
		{
			base.ParentWizard.FinishButton.Enabled = this._parameterEditorUserControl.ParametersConfigured;
		}

		// Token: 0x04001CC1 RID: 7361
		private global::System.Windows.Forms.Label _helpLabel;

		// Token: 0x04001CC2 RID: 7362
		private global::System.Windows.Forms.Label _signatureLabel;

		// Token: 0x04001CC3 RID: 7363
		private ParameterEditorUserControl _parameterEditorUserControl;

		// Token: 0x04001CC4 RID: 7364
		private global::System.Windows.Forms.TextBox _signatureTextBox;

		// Token: 0x04001CC5 RID: 7365
		private ObjectDataSource _objectDataSource;

		// Token: 0x04001CC6 RID: 7366
		private ObjectDataSourceDesigner _objectDataSourceDesigner;
	}
}
