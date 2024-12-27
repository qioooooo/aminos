using System;
using System.ComponentModel;
using System.Design;
using System.Globalization;
using System.Threading;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000269 RID: 617
	public abstract class MaskDescriptor
	{
		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001747 RID: 5959
		public abstract string Mask { get; }

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001748 RID: 5960
		public abstract string Name { get; }

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001749 RID: 5961
		public abstract string Sample { get; }

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x0600174A RID: 5962
		public abstract Type ValidatingType { get; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x000780F0 File Offset: 0x000770F0
		public virtual CultureInfo Culture
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture;
			}
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x000780FC File Offset: 0x000770FC
		public static bool IsValidMaskDescriptor(MaskDescriptor maskDescriptor)
		{
			string text;
			return MaskDescriptor.IsValidMaskDescriptor(maskDescriptor, out text);
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x00078114 File Offset: 0x00077114
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

		// Token: 0x0600174E RID: 5966 RVA: 0x00078228 File Offset: 0x00077228
		private static void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			maskedTextBox.Tag = MaskedTextBoxDesigner.GetMaskInputRejectedErrorMessage(e);
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x00078248 File Offset: 0x00077248
		private static void maskedTextBox1_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
		{
			if (!e.IsValidInput)
			{
				MaskedTextBox maskedTextBox = sender as MaskedTextBox;
				maskedTextBox.Tag = e.Message;
			}
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x00078270 File Offset: 0x00077270
		public override bool Equals(object maskDescriptor)
		{
			MaskDescriptor maskDescriptor2 = maskDescriptor as MaskDescriptor;
			if (!MaskDescriptor.IsValidMaskDescriptor(maskDescriptor2) || !MaskDescriptor.IsValidMaskDescriptor(this))
			{
				return this == maskDescriptor;
			}
			return this.Mask == maskDescriptor2.Mask && this.ValidatingType == maskDescriptor2.ValidatingType;
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x000782BC File Offset: 0x000772BC
		public override int GetHashCode()
		{
			string text = this.Mask;
			if (this.ValidatingType != null)
			{
				text += this.ValidatingType.ToString();
			}
			return text.GetHashCode();
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x000782F0 File Offset: 0x000772F0
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
