using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006F4 RID: 1780
	[WebPartTransformer(typeof(IWebPartRow), typeof(IWebPartField))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class RowToFieldTransformer : WebPartTransformer, IWebPartField
	{
		// Token: 0x06005704 RID: 22276 RVA: 0x0015E9D0 File Offset: 0x0015D9D0
		public override Control CreateConfigurationControl()
		{
			return new RowToFieldTransformer.RowToFieldConfigurationWizard(this);
		}

		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x06005705 RID: 22277 RVA: 0x0015E9D8 File Offset: 0x0015D9D8
		// (set) Token: 0x06005706 RID: 22278 RVA: 0x0015E9EE File Offset: 0x0015D9EE
		public string FieldName
		{
			get
			{
				if (this._fieldName == null)
				{
					return string.Empty;
				}
				return this._fieldName;
			}
			set
			{
				this._fieldName = value;
			}
		}

		// Token: 0x17001677 RID: 5751
		// (get) Token: 0x06005707 RID: 22279 RVA: 0x0015E9F7 File Offset: 0x0015D9F7
		private PropertyDescriptorCollection ProviderSchema
		{
			get
			{
				if (this._provider == null)
				{
					return null;
				}
				return this._provider.Schema;
			}
		}

		// Token: 0x06005708 RID: 22280 RVA: 0x0015EA10 File Offset: 0x0015DA10
		private void GetRowData(object rowData)
		{
			object obj = null;
			if (rowData != null)
			{
				PropertyDescriptor schema = ((IWebPartField)this).Schema;
				if (schema != null)
				{
					obj = schema.GetValue(rowData);
				}
			}
			this._callback(obj);
		}

		// Token: 0x06005709 RID: 22281 RVA: 0x0015EA40 File Offset: 0x0015DA40
		protected internal override void LoadConfigurationState(object savedState)
		{
			this._fieldName = (string)savedState;
		}

		// Token: 0x0600570A RID: 22282 RVA: 0x0015EA4E File Offset: 0x0015DA4E
		protected internal override object SaveConfigurationState()
		{
			return this._fieldName;
		}

		// Token: 0x0600570B RID: 22283 RVA: 0x0015EA56 File Offset: 0x0015DA56
		public override object Transform(object providerData)
		{
			this._provider = (IWebPartRow)providerData;
			return this;
		}

		// Token: 0x0600570C RID: 22284 RVA: 0x0015EA65 File Offset: 0x0015DA65
		void IWebPartField.GetFieldValue(FieldCallback callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (this._provider != null)
			{
				this._callback = callback;
				this._provider.GetRowData(new RowCallback(this.GetRowData));
				return;
			}
			callback(null);
		}

		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x0600570D RID: 22285 RVA: 0x0015EAA4 File Offset: 0x0015DAA4
		PropertyDescriptor IWebPartField.Schema
		{
			get
			{
				PropertyDescriptorCollection providerSchema = this.ProviderSchema;
				if (providerSchema == null)
				{
					return null;
				}
				return providerSchema.Find(this.FieldName, true);
			}
		}

		// Token: 0x04002F7B RID: 12155
		private IWebPartRow _provider;

		// Token: 0x04002F7C RID: 12156
		private string _fieldName;

		// Token: 0x04002F7D RID: 12157
		private FieldCallback _callback;

		// Token: 0x020006F6 RID: 1782
		private sealed class RowToFieldConfigurationWizard : TransformerConfigurationWizardBase
		{
			// Token: 0x06005726 RID: 22310 RVA: 0x0015EE0D File Offset: 0x0015DE0D
			public RowToFieldConfigurationWizard(RowToFieldTransformer owner)
			{
				this._owner = owner;
			}

			// Token: 0x1700167D RID: 5757
			// (get) Token: 0x06005727 RID: 22311 RVA: 0x0015EE1C File Offset: 0x0015DE1C
			protected override PropertyDescriptorCollection ConsumerSchema
			{
				get
				{
					return null;
				}
			}

			// Token: 0x1700167E RID: 5758
			// (get) Token: 0x06005728 RID: 22312 RVA: 0x0015EE1F File Offset: 0x0015DE1F
			protected override PropertyDescriptorCollection ProviderSchema
			{
				get
				{
					return this._owner.ProviderSchema;
				}
			}

			// Token: 0x06005729 RID: 22313 RVA: 0x0015EE2C File Offset: 0x0015DE2C
			protected override void CreateWizardSteps()
			{
				WizardStep wizardStep = new WizardStep();
				this._fieldName = new DropDownList();
				this._fieldName.ID = "FieldName";
				if (base.OldProviderNames != null)
				{
					for (int i = 0; i < base.OldProviderNames.Length / 2; i++)
					{
						ListItem listItem = new ListItem(base.OldProviderNames[2 * i], base.OldProviderNames[2 * i + 1]);
						if (string.Equals(listItem.Value, this._owner.FieldName, StringComparison.OrdinalIgnoreCase))
						{
							listItem.Selected = true;
						}
						this._fieldName.Items.Add(listItem);
					}
				}
				else
				{
					this._fieldName.Items.Add(new ListItem(SR.GetString("RowToFieldTransformer_NoProviderSchema")));
					this._fieldName.Enabled = false;
				}
				Label label = new Label();
				label.Text = SR.GetString("RowToFieldTransformer_FieldName");
				label.AssociatedControlID = this._fieldName.ID;
				wizardStep.Controls.Add(label);
				wizardStep.Controls.Add(new LiteralControl(" "));
				wizardStep.Controls.Add(this._fieldName);
				this.WizardSteps.Add(wizardStep);
			}

			// Token: 0x0600572A RID: 22314 RVA: 0x0015EF58 File Offset: 0x0015DF58
			protected override void OnFinishButtonClick(WizardNavigationEventArgs e)
			{
				string text = null;
				if (this._fieldName.Enabled)
				{
					text = this._fieldName.SelectedValue;
				}
				this._owner.FieldName = text;
				base.OnFinishButtonClick(e);
			}

			// Token: 0x04002F86 RID: 12166
			private const string fieldNameID = "FieldName";

			// Token: 0x04002F87 RID: 12167
			private DropDownList _fieldName;

			// Token: 0x04002F88 RID: 12168
			private RowToFieldTransformer _owner;
		}
	}
}
