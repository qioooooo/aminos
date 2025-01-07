using System;
using System.ComponentModel;
using System.Design;
using System.Globalization;
using System.Threading;

namespace System.Windows.Forms.Design
{
	public abstract class MaskDescriptor
	{
		public abstract string Mask { get; }

		public abstract string Name { get; }

		public abstract string Sample { get; }

		public abstract Type ValidatingType { get; }

		public virtual CultureInfo Culture
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture;
			}
		}

		public static bool IsValidMaskDescriptor(MaskDescriptor maskDescriptor)
		{
			string text;
			return MaskDescriptor.IsValidMaskDescriptor(maskDescriptor, out text);
		}

		public static bool IsValidMaskDescriptor(MaskDescriptor maskDescriptor, out string validationErrorDescription)
		{
			validationErrorDescription = string.Empty;
			if (maskDescriptor == null)
			{
				validationErrorDescription = SR.GetString("MaskDescriptorNull");
				return false;
			}
			if (string.IsNullOrEmpty(maskDescriptor.Mask) || string.IsNullOrEmpty(maskDescriptor.Name) || string.IsNullOrEmpty(maskDescriptor.Sample))
			{
				validationErrorDescription = SR.GetString("MaskDescriptorNullOrEmptyRequiredProperty");
				return false;
			}
			MaskedTextProvider maskedTextProvider = new MaskedTextProvider(maskDescriptor.Mask, maskDescriptor.Culture);
			MaskedTextBox maskedTextBox = new MaskedTextBox(maskedTextProvider);
			maskedTextBox.SkipLiterals = true;
			maskedTextBox.ResetOnPrompt = true;
			maskedTextBox.ResetOnSpace = true;
			maskedTextBox.ValidatingType = maskDescriptor.ValidatingType;
			maskedTextBox.FormatProvider = maskDescriptor.Culture;
			maskedTextBox.Culture = maskDescriptor.Culture;
			maskedTextBox.TypeValidationCompleted += MaskDescriptor.maskedTextBox1_TypeValidationCompleted;
			maskedTextBox.MaskInputRejected += MaskDescriptor.maskedTextBox1_MaskInputRejected;
			maskedTextBox.Text = maskDescriptor.Sample;
			if (maskedTextBox.Tag == null && maskDescriptor.ValidatingType != null)
			{
				maskedTextBox.ValidateText();
			}
			if (maskedTextBox.Tag != null)
			{
				validationErrorDescription = maskedTextBox.Tag.ToString();
			}
			return validationErrorDescription.Length == 0;
		}

		private static void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			maskedTextBox.Tag = MaskedTextBoxDesigner.GetMaskInputRejectedErrorMessage(e);
		}

		private static void maskedTextBox1_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
		{
			if (!e.IsValidInput)
			{
				MaskedTextBox maskedTextBox = sender as MaskedTextBox;
				maskedTextBox.Tag = e.Message;
			}
		}

		public override bool Equals(object maskDescriptor)
		{
			MaskDescriptor maskDescriptor2 = maskDescriptor as MaskDescriptor;
			if (!MaskDescriptor.IsValidMaskDescriptor(maskDescriptor2) || !MaskDescriptor.IsValidMaskDescriptor(this))
			{
				return this == maskDescriptor;
			}
			return this.Mask == maskDescriptor2.Mask && this.ValidatingType == maskDescriptor2.ValidatingType;
		}

		public override int GetHashCode()
		{
			string text = this.Mask;
			if (this.ValidatingType != null)
			{
				text += this.ValidatingType.ToString();
			}
			return text.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0}<Name={1}, Mask={2}, ValidatingType={3}", new object[]
			{
				base.GetType(),
				(this.Name != null) ? this.Name : "null",
				(this.Mask != null) ? this.Mask : "null",
				(this.ValidatingType != null) ? this.ValidatingType.ToString() : "null"
			});
		}
	}
}
