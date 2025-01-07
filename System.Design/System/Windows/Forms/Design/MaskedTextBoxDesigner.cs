using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	internal class MaskedTextBoxDesigner : TextBoxBaseDesigner
	{
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

		[Obsolete("This method has been deprecated. Use InitializeNewComponent instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public override void OnSetComponentDefaults()
		{
		}

		private void OnVerbSetMask(object sender, EventArgs e)
		{
			MaskedTextBoxDesignerActionList maskedTextBoxDesignerActionList = new MaskedTextBoxDesignerActionList(this);
			maskedTextBoxDesignerActionList.SetMask();
		}

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

		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				return selectionRules & ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
			}
		}

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

		private DesignerVerbCollection verbs;

		private DesignerActionListCollection actions;
	}
}
