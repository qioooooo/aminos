using System;
using System.ComponentModel;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006F5 RID: 1781
	internal abstract class TransformerConfigurationWizardBase : Wizard, ITransformerConfigurationControl
	{
		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x0600570F RID: 22287
		protected abstract PropertyDescriptorCollection ConsumerSchema { get; }

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x06005710 RID: 22288 RVA: 0x0015EAD2 File Offset: 0x0015DAD2
		// (set) Token: 0x06005711 RID: 22289 RVA: 0x0015EADA File Offset: 0x0015DADA
		protected string[] OldConsumerNames
		{
			get
			{
				return this._oldConsumerNames;
			}
			set
			{
				this._oldConsumerNames = value;
			}
		}

		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x06005712 RID: 22290 RVA: 0x0015EAE3 File Offset: 0x0015DAE3
		// (set) Token: 0x06005713 RID: 22291 RVA: 0x0015EAEB File Offset: 0x0015DAEB
		protected string[] OldProviderNames
		{
			get
			{
				return this._oldProviderNames;
			}
			set
			{
				this._oldProviderNames = value;
			}
		}

		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x06005714 RID: 22292
		protected abstract PropertyDescriptorCollection ProviderSchema { get; }

		// Token: 0x1400010C RID: 268
		// (add) Token: 0x06005715 RID: 22293 RVA: 0x0015EAF4 File Offset: 0x0015DAF4
		// (remove) Token: 0x06005716 RID: 22294 RVA: 0x0015EB07 File Offset: 0x0015DB07
		public event EventHandler Cancelled
		{
			add
			{
				base.Events.AddHandler(TransformerConfigurationWizardBase.EventCancelled, value);
			}
			remove
			{
				base.Events.RemoveHandler(TransformerConfigurationWizardBase.EventCancelled, value);
			}
		}

		// Token: 0x1400010D RID: 269
		// (add) Token: 0x06005717 RID: 22295 RVA: 0x0015EB1A File Offset: 0x0015DB1A
		// (remove) Token: 0x06005718 RID: 22296 RVA: 0x0015EB2D File Offset: 0x0015DB2D
		public event EventHandler Succeeded
		{
			add
			{
				base.Events.AddHandler(TransformerConfigurationWizardBase.EventSucceeded, value);
			}
			remove
			{
				base.Events.RemoveHandler(TransformerConfigurationWizardBase.EventSucceeded, value);
			}
		}

		// Token: 0x06005719 RID: 22297
		protected abstract void CreateWizardSteps();

		// Token: 0x0600571A RID: 22298 RVA: 0x0015EB40 File Offset: 0x0015DB40
		protected internal override void LoadControlState(object savedState)
		{
			if (savedState == null)
			{
				this.CreateWizardSteps();
				base.LoadControlState(null);
				return;
			}
			object[] array = (object[])savedState;
			if (array.Length != 3)
			{
				throw new ArgumentException(SR.GetString("Invalid_ControlState"));
			}
			if (array[1] != null)
			{
				this.OldProviderNames = (string[])array[1];
			}
			if (array[2] != null)
			{
				this.OldConsumerNames = (string[])array[2];
			}
			this.CreateWizardSteps();
			base.LoadControlState(array[0]);
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x0015EBB0 File Offset: 0x0015DBB0
		protected override void OnCancelButtonClick(EventArgs e)
		{
			this.OnCancelled(EventArgs.Empty);
			base.OnCancelButtonClick(e);
		}

		// Token: 0x0600571C RID: 22300 RVA: 0x0015EBC4 File Offset: 0x0015DBC4
		private void OnCancelled(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[TransformerConfigurationWizardBase.EventCancelled];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x0015EBF2 File Offset: 0x0015DBF2
		protected override void OnFinishButtonClick(WizardNavigationEventArgs e)
		{
			this.OnSucceeded(EventArgs.Empty);
			base.OnFinishButtonClick(e);
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x0015EC08 File Offset: 0x0015DC08
		protected internal override void OnInit(EventArgs e)
		{
			this.DisplayCancelButton = true;
			this.DisplaySideBar = false;
			if (this.Page != null)
			{
				this.Page.RegisterRequiresControlState(this);
				this.Page.PreRenderComplete += this.OnPagePreRenderComplete;
			}
			base.OnInit(e);
		}

		// Token: 0x0600571F RID: 22303 RVA: 0x0015EC58 File Offset: 0x0015DC58
		private void OnPagePreRenderComplete(object sender, EventArgs e)
		{
			string[] array = this.ConvertSchemaToArray(this.ProviderSchema);
			string[] array2 = this.ConvertSchemaToArray(this.ConsumerSchema);
			if (this.StringArraysDifferent(array, this.OldProviderNames) || this.StringArraysDifferent(array2, this.OldConsumerNames) || this.WizardSteps.Count == 0)
			{
				this.OldProviderNames = array;
				this.OldConsumerNames = array2;
				this.WizardSteps.Clear();
				base.ClearChildState();
				this.CreateWizardSteps();
				this.ActiveStepIndex = 0;
			}
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x0015ECD8 File Offset: 0x0015DCD8
		private string[] ConvertSchemaToArray(PropertyDescriptorCollection schema)
		{
			string[] array = null;
			if (schema != null && schema.Count > 0)
			{
				array = new string[schema.Count * 2];
				for (int i = 0; i < schema.Count; i++)
				{
					PropertyDescriptor propertyDescriptor = schema[i];
					if (propertyDescriptor != null)
					{
						array[2 * i] = propertyDescriptor.DisplayName;
						array[2 * i + 1] = propertyDescriptor.Name;
					}
				}
			}
			return array;
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x0015ED38 File Offset: 0x0015DD38
		private void OnSucceeded(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[TransformerConfigurationWizardBase.EventSucceeded];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06005722 RID: 22306 RVA: 0x0015ED68 File Offset: 0x0015DD68
		protected internal override object SaveControlState()
		{
			object[] array = new object[]
			{
				base.SaveControlState(),
				this.OldProviderNames,
				this.OldConsumerNames
			};
			for (int i = 0; i < 3; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x0015EDAC File Offset: 0x0015DDAC
		private bool StringArraysDifferent(string[] arrA, string[] arrB)
		{
			int num = ((arrA == null) ? 0 : arrA.Length);
			int num2 = ((arrB == null) ? 0 : arrB.Length);
			if (num != num2)
			{
				return true;
			}
			for (int i = 0; i < num2; i++)
			{
				if (arrA[i] != arrB[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002F7E RID: 12158
		private const int baseIndex = 0;

		// Token: 0x04002F7F RID: 12159
		private const int oldProviderNamesIndex = 1;

		// Token: 0x04002F80 RID: 12160
		private const int oldConsumerNamesIndex = 2;

		// Token: 0x04002F81 RID: 12161
		private const int controlStateArrayLength = 3;

		// Token: 0x04002F82 RID: 12162
		private string[] _oldProviderNames;

		// Token: 0x04002F83 RID: 12163
		private string[] _oldConsumerNames;

		// Token: 0x04002F84 RID: 12164
		private static readonly object EventCancelled = new object();

		// Token: 0x04002F85 RID: 12165
		private static readonly object EventSucceeded = new object();
	}
}
