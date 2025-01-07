using System;
using System.Design;
using System.Drawing;
using System.Reflection;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class ObjectDataSourceConfigureParametersPanel : WizardPanel
	{
		public ObjectDataSourceConfigureParametersPanel(ObjectDataSourceDesigner objectDataSourceDesigner)
		{
			this._objectDataSourceDesigner = objectDataSourceDesigner;
			this._objectDataSource = (ObjectDataSource)this._objectDataSourceDesigner.Component;
			this.InitializeComponent();
			this.InitializeUI();
			this._parameterEditorUserControl.SetAllowCollectionChanges(false);
		}

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

		public void InitializeParameters(ParameterCollection selectParameters)
		{
			Parameter[] array = new Parameter[selectParameters.Count];
			selectParameters.CopyTo(array, 0);
			this._parameterEditorUserControl.AddParameters(array);
		}

		private void InitializeUI()
		{
			base.Caption = SR.GetString("ObjectDataSourceConfigureParametersPanel_PanelCaption");
			this._helpLabel.Text = SR.GetString("ObjectDataSourceConfigureParametersPanel_HelpLabel");
			this._signatureLabel.Text = SR.GetString("ObjectDataSource_General_MethodSignatureLabel");
		}

		protected internal override void OnComplete()
		{
			this._objectDataSource.SelectParameters.Clear();
			Parameter[] parameters = this._parameterEditorUserControl.GetParameters();
			foreach (Parameter parameter in parameters)
			{
				this._objectDataSource.SelectParameters.Add(parameter);
			}
		}

		public override bool OnNext()
		{
			return true;
		}

		private void OnParameterEditorUserControlParametersChanged(object sender, EventArgs e)
		{
			this.UpdateUI();
		}

		public override void OnPrevious()
		{
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				base.ParentWizard.NextButton.Enabled = false;
				this.UpdateUI();
			}
		}

		public void ResetUI()
		{
			this._parameterEditorUserControl.ClearParameters();
		}

		public void SetMethod(MethodInfo selectMethodInfo)
		{
			this._signatureTextBox.Text = ObjectDataSourceMethodEditor.GetMethodSignature(selectMethodInfo);
			Parameter[] array = ObjectDataSourceDesigner.MergeParameters(this._parameterEditorUserControl.GetParameters(), selectMethodInfo);
			this._parameterEditorUserControl.ClearParameters();
			this._parameterEditorUserControl.AddParameters(array);
		}

		private void UpdateUI()
		{
			base.ParentWizard.FinishButton.Enabled = this._parameterEditorUserControl.ParametersConfigured;
		}

		private global::System.Windows.Forms.Label _helpLabel;

		private global::System.Windows.Forms.Label _signatureLabel;

		private ParameterEditorUserControl _parameterEditorUserControl;

		private global::System.Windows.Forms.TextBox _signatureTextBox;

		private ObjectDataSource _objectDataSource;

		private ObjectDataSourceDesigner _objectDataSourceDesigner;
	}
}
