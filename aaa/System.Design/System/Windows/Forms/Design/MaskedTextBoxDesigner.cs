using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000270 RID: 624
	internal class MaskedTextBoxDesigner : TextBoxBaseDesigner
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x0007A690 File Offset: 0x00079690
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this.actions == null)
				{
					this.actions = new DesignerActionListCollection();
					this.actions.Add(new MaskedTextBoxDesignerActionList(this));
				}
				return this.actions;
			}
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x0007A6C0 File Offset: 0x000796C0
		internal static MaskedTextBox GetDesignMaskedTextBox(MaskedTextBox mtb)
		{
			MaskedTextBox maskedTextBox;
			if (mtb == null)
			{
				maskedTextBox = new MaskedTextBox();
			}
			else
			{
				if (mtb.MaskedTextProvider == null)
				{
					maskedTextBox = new MaskedTextBox();
					maskedTextBox.Text = mtb.Text;
				}
				else
				{
					maskedTextBox = new MaskedTextBox(mtb.MaskedTextProvider);
				}
				maskedTextBox.ValidatingType = mtb.ValidatingType;
				maskedTextBox.BeepOnError = mtb.BeepOnError;
				maskedTextBox.InsertKeyMode = mtb.InsertKeyMode;
				maskedTextBox.RejectInputOnFirstFailure = mtb.RejectInputOnFirstFailure;
				maskedTextBox.CutCopyMaskFormat = mtb.CutCopyMaskFormat;
				maskedTextBox.Culture = mtb.Culture;
			}
			maskedTextBox.UseSystemPasswordChar = false;
			maskedTextBox.PasswordChar = '\0';
			maskedTextBox.ReadOnly = false;
			maskedTextBox.HidePromptOnLeave = false;
			return maskedTextBox;
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x0007A76C File Offset: 0x0007976C
		internal static string GetMaskInputRejectedErrorMessage(MaskInputRejectedEventArgs e)
		{
			MaskedTextResultHint rejectionHint = e.RejectionHint;
			string text;
			switch (rejectionHint)
			{
			case MaskedTextResultHint.PositionOutOfRange:
				text = SR.GetString("MaskedTextBoxHintPositionOutOfRange");
				goto IL_00C7;
			case MaskedTextResultHint.NonEditPosition:
				text = SR.GetString("MaskedTextBoxHintNonEditPosition");
				goto IL_00C7;
			case MaskedTextResultHint.UnavailableEditPosition:
				text = SR.GetString("MaskedTextBoxHintUnavailableEditPosition");
				goto IL_00C7;
			case MaskedTextResultHint.PromptCharNotAllowed:
				text = SR.GetString("MaskedTextBoxHintPromptCharNotAllowed");
				goto IL_00C7;
			case MaskedTextResultHint.InvalidInput:
				break;
			default:
				switch (rejectionHint)
				{
				case MaskedTextResultHint.SignedDigitExpected:
					text = SR.GetString("MaskedTextBoxHintSignedDigitExpected");
					goto IL_00C7;
				case MaskedTextResultHint.LetterExpected:
					text = SR.GetString("MaskedTextBoxHintLetterExpected");
					goto IL_00C7;
				case MaskedTextResultHint.DigitExpected:
					text = SR.GetString("MaskedTextBoxHintDigitExpected");
					goto IL_00C7;
				case MaskedTextResultHint.AlphanumericCharacterExpected:
					text = SR.GetString("MaskedTextBoxHintAlphanumericCharacterExpected");
					goto IL_00C7;
				case MaskedTextResultHint.AsciiCharacterExpected:
					text = SR.GetString("MaskedTextBoxHintAsciiCharacterExpected");
					goto IL_00C7;
				}
				break;
			}
			text = SR.GetString("MaskedTextBoxHintInvalidInput");
			IL_00C7:
			return string.Format(CultureInfo.CurrentCulture, SR.GetString("MaskedTextBoxTextEditorErrorFormatString"), new object[] { e.Position, text });
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x0007A86E File Offset: 0x0007986E
		[Obsolete("This method has been deprecated. Use InitializeNewComponent instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public override void OnSetComponentDefaults()
		{
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x0007A870 File Offset: 0x00079870
		private void OnVerbSetMask(object sender, EventArgs e)
		{
			MaskedTextBoxDesignerActionList maskedTextBoxDesignerActionList = new MaskedTextBoxDesignerActionList(this);
			maskedTextBoxDesignerActionList.SetMask();
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0007A88C File Offset: 0x0007988C
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "Text", "PasswordChar" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(MaskedTextBoxDesigner), propertyDescriptor, array2);
				}
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600178F RID: 6031 RVA: 0x0007A900 File Offset: 0x00079900
		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				return selectionRules & ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x0007A91C File Offset: 0x0007991C
		// (set) Token: 0x06001791 RID: 6033 RVA: 0x0007A95C File Offset: 0x0007995C
		private char PasswordChar
		{
			get
			{
				MaskedTextBox maskedTextBox = this.Control as MaskedTextBox;
				if (maskedTextBox.UseSystemPasswordChar)
				{
					maskedTextBox.UseSystemPasswordChar = false;
					char passwordChar = maskedTextBox.PasswordChar;
					maskedTextBox.UseSystemPasswordChar = true;
					return passwordChar;
				}
				return maskedTextBox.PasswordChar;
			}
			set
			{
				MaskedTextBox maskedTextBox = this.Control as MaskedTextBox;
				maskedTextBox.PasswordChar = value;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001792 RID: 6034 RVA: 0x0007A97C File Offset: 0x0007997C
		// (set) Token: 0x06001793 RID: 6035 RVA: 0x0007A9B8 File Offset: 0x000799B8
		private string Text
		{
			get
			{
				MaskedTextBox maskedTextBox = this.Control as MaskedTextBox;
				if (string.IsNullOrEmpty(maskedTextBox.Mask))
				{
					return maskedTextBox.Text;
				}
				return maskedTextBox.MaskedTextProvider.ToString(false, false);
			}
			set
			{
				MaskedTextBox maskedTextBox = this.Control as MaskedTextBox;
				if (string.IsNullOrEmpty(maskedTextBox.Mask))
				{
					maskedTextBox.Text = value;
					return;
				}
				bool resetOnSpace = maskedTextBox.ResetOnSpace;
				bool resetOnPrompt = maskedTextBox.ResetOnPrompt;
				bool skipLiterals = maskedTextBox.SkipLiterals;
				maskedTextBox.ResetOnSpace = true;
				maskedTextBox.ResetOnPrompt = true;
				maskedTextBox.SkipLiterals = true;
				maskedTextBox.Text = value;
				maskedTextBox.ResetOnSpace = resetOnSpace;
				maskedTextBox.ResetOnPrompt = resetOnPrompt;
				maskedTextBox.SkipLiterals = skipLiterals;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001794 RID: 6036 RVA: 0x0007AA2C File Offset: 0x00079A2C
		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (this.verbs == null)
				{
					this.verbs = new DesignerVerbCollection();
					this.verbs.Add(new DesignerVerb(SR.GetString("MaskedTextBoxDesignerVerbsSetMaskDesc"), new EventHandler(this.OnVerbSetMask)));
				}
				return this.verbs;
			}
		}

		// Token: 0x04001347 RID: 4935
		private DesignerVerbCollection verbs;

		// Token: 0x04001348 RID: 4936
		private DesignerActionListCollection actions;
	}
}
