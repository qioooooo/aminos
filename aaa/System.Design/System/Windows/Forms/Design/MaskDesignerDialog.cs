using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200026E RID: 622
	internal partial class MaskDesignerDialog : Form
	{
		// Token: 0x06001763 RID: 5987 RVA: 0x00079264 File Offset: 0x00078264
		public MaskDesignerDialog(MaskedTextBox instance, IHelpService helpService)
		{
			if (instance == null)
			{
				this.maskedTextBox = new MaskedTextBox();
			}
			else
			{
				this.maskedTextBox = MaskedTextBoxDesigner.GetDesignMaskedTextBox(instance);
			}
			this.helpService = helpService;
			this.InitializeComponent();
			base.SuspendLayout();
			this.txtBoxMask.Text = this.maskedTextBox.Mask;
			this.AddDefaultMaskDescriptors(this.maskedTextBox.Culture);
			this.maskDescriptionHeader.Text = SR.GetString("MaskDesignerDialogMaskDescription");
			this.maskDescriptionHeader.Width = this.listViewCannedMasks.Width / 3;
			this.dataFormatHeader.Text = SR.GetString("MaskDesignerDialogDataFormat");
			this.dataFormatHeader.Width = this.listViewCannedMasks.Width / 3;
			this.validatingTypeHeader.Text = SR.GetString("MaskDesignerDialogValidatingType");
			this.validatingTypeHeader.Width = this.listViewCannedMasks.Width / 3 - SystemInformation.VerticalScrollBarWidth - 4;
			base.ResumeLayout(false);
			this.HookEvents();
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x0007937C File Offset: 0x0007837C
		private void HookEvents()
		{
			this.listViewCannedMasks.SelectedIndexChanged += this.listViewCannedMasks_SelectedIndexChanged;
			this.listViewCannedMasks.ColumnClick += this.listViewCannedMasks_ColumnClick;
			this.listViewCannedMasks.Enter += this.listViewCannedMasks_Enter;
			this.btnOK.Click += this.btnOK_Click;
			this.txtBoxMask.TextChanged += this.txtBoxMask_TextChanged;
			this.txtBoxMask.Validating += this.txtBoxMask_Validating;
			this.maskedTextBox.KeyDown += this.maskedTextBox_KeyDown;
			this.maskedTextBox.MaskInputRejected += this.maskedTextBox_MaskInputRejected;
			base.Load += this.MaskDesignerDialog_Load;
			base.HelpButtonClicked += this.MaskDesignerDialog_HelpButtonClicked;
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x00079C58 File Offset: 0x00078C58
		public string Mask
		{
			get
			{
				return this.maskedTextBox.Mask;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001767 RID: 5991 RVA: 0x00079C65 File Offset: 0x00078C65
		public Type ValidatingType
		{
			get
			{
				return this.mtpValidatingType;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x00079C6D File Offset: 0x00078C6D
		public IEnumerator MaskDescriptors
		{
			get
			{
				return this.maskDescriptors.GetEnumerator();
			}
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00079C80 File Offset: 0x00078C80
		private void AddDefaultMaskDescriptors(CultureInfo culture)
		{
			this.customMaskDescriptor = new MaskDescriptorTemplate(null, SR.GetString("MaskDesignerDialogCustomEntry"), null, null, null, true);
			List<MaskDescriptor> localizedMaskDescriptors = MaskDescriptorTemplate.GetLocalizedMaskDescriptors(culture);
			this.InsertMaskDescriptor(0, this.customMaskDescriptor, false);
			foreach (MaskDescriptor maskDescriptor in localizedMaskDescriptors)
			{
				this.InsertMaskDescriptor(0, maskDescriptor);
			}
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00079D00 File Offset: 0x00078D00
		private bool ContainsMaskDescriptor(MaskDescriptor maskDescriptor)
		{
			foreach (MaskDescriptor maskDescriptor2 in this.maskDescriptors)
			{
				if (maskDescriptor.Equals(maskDescriptor2) || maskDescriptor.Name.Trim() == maskDescriptor2.Name.Trim())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00079D7C File Offset: 0x00078D7C
		public void DiscoverMaskDescriptors(ITypeDiscoveryService discoveryService)
		{
			if (discoveryService != null)
			{
				ICollection collection = DesignerUtils.FilterGenericTypes(discoveryService.GetTypes(typeof(MaskDescriptor), false));
				foreach (object obj in collection)
				{
					Type type = (Type)obj;
					if (!type.IsAbstract && type.IsPublic)
					{
						try
						{
							MaskDescriptor maskDescriptor = (MaskDescriptor)Activator.CreateInstance(type);
							this.InsertMaskDescriptor(0, maskDescriptor);
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
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x00079E3C File Offset: 0x00078E3C
		private int GetMaskDescriptorIndex(MaskDescriptor maskDescriptor)
		{
			for (int i = 0; i < this.maskDescriptors.Count; i++)
			{
				MaskDescriptor maskDescriptor2 = this.maskDescriptors[i];
				if (maskDescriptor2 == maskDescriptor)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x00079E74 File Offset: 0x00078E74
		private void SelectMtbMaskDescriptor()
		{
			int num = -1;
			if (!string.IsNullOrEmpty(this.maskedTextBox.Mask))
			{
				for (int i = 0; i < this.maskDescriptors.Count; i++)
				{
					MaskDescriptor maskDescriptor = this.maskDescriptors[i];
					if (maskDescriptor.Mask == this.maskedTextBox.Mask && maskDescriptor.ValidatingType == this.maskedTextBox.ValidatingType)
					{
						num = i;
						break;
					}
				}
			}
			if (num == -1)
			{
				num = this.GetMaskDescriptorIndex(this.customMaskDescriptor);
			}
			if (num != -1)
			{
				this.SetSelectedMaskDescriptor(num);
			}
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x00079F08 File Offset: 0x00078F08
		private void SetSelectedMaskDescriptor(MaskDescriptor maskDex)
		{
			int maskDescriptorIndex = this.GetMaskDescriptorIndex(maskDex);
			this.SetSelectedMaskDescriptor(maskDescriptorIndex);
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00079F24 File Offset: 0x00078F24
		private void SetSelectedMaskDescriptor(int maskDexIndex)
		{
			if (maskDexIndex >= 0 && this.listViewCannedMasks.Items.Count > maskDexIndex)
			{
				this.listViewCannedMasks.Items[maskDexIndex].Selected = true;
				this.listViewCannedMasks.FocusedItem = this.listViewCannedMasks.Items[maskDexIndex];
				this.listViewCannedMasks.EnsureVisible(maskDexIndex);
			}
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x00079F88 File Offset: 0x00078F88
		private void UpdateSortedListView(MaskDescriptorComparer.SortType sortType)
		{
			if (!this.listViewCannedMasks.IsHandleCreated)
			{
				return;
			}
			MaskDescriptor maskDescriptor = null;
			if (this.listViewCannedMasks.SelectedItems.Count > 0)
			{
				int num = this.listViewCannedMasks.SelectedIndices[0];
				maskDescriptor = this.maskDescriptors[num];
			}
			this.maskDescriptors.RemoveAt(this.maskDescriptors.Count - 1);
			this.maskDescriptors.Sort(new MaskDescriptorComparer(sortType, this.listViewSortOrder));
			UnsafeNativeMethods.SendMessage(this.listViewCannedMasks.Handle, 11, false, 0);
			try
			{
				this.listViewCannedMasks.Items.Clear();
				string @string = SR.GetString("MaskDescriptorValidatingTypeNone");
				foreach (MaskDescriptor maskDescriptor2 in this.maskDescriptors)
				{
					string text = ((maskDescriptor2.ValidatingType != null) ? maskDescriptor2.ValidatingType.Name : @string);
					MaskedTextProvider maskedTextProvider = new MaskedTextProvider(maskDescriptor2.Mask, maskDescriptor2.Culture);
					maskedTextProvider.Add(maskDescriptor2.Sample);
					string text2 = maskedTextProvider.ToString(false, true);
					this.listViewCannedMasks.Items.Add(new ListViewItem(new string[] { maskDescriptor2.Name, text2, text }));
				}
				this.maskDescriptors.Add(this.customMaskDescriptor);
				this.listViewCannedMasks.Items.Add(new ListViewItem(new string[]
				{
					this.customMaskDescriptor.Name,
					"",
					@string
				}));
				if (maskDescriptor != null)
				{
					this.SetSelectedMaskDescriptor(maskDescriptor);
				}
			}
			finally
			{
				UnsafeNativeMethods.SendMessage(this.listViewCannedMasks.Handle, 11, true, 0);
				this.listViewCannedMasks.Invalidate();
			}
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x0007A198 File Offset: 0x00079198
		private void InsertMaskDescriptor(int index, MaskDescriptor maskDescriptor)
		{
			this.InsertMaskDescriptor(index, maskDescriptor, true);
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x0007A1A4 File Offset: 0x000791A4
		private void InsertMaskDescriptor(int index, MaskDescriptor maskDescriptor, bool validateDescriptor)
		{
			string text;
			if (validateDescriptor && !MaskDescriptor.IsValidMaskDescriptor(maskDescriptor, out text))
			{
				return;
			}
			if (!this.ContainsMaskDescriptor(maskDescriptor))
			{
				this.maskDescriptors.Insert(index, maskDescriptor);
			}
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0007A1D8 File Offset: 0x000791D8
		private void RemoveMaskDescriptor(MaskDescriptor maskDescriptor)
		{
			int maskDescriptorIndex = this.GetMaskDescriptorIndex(maskDescriptor);
			if (maskDescriptorIndex >= 0)
			{
				this.maskDescriptors.RemoveAt(maskDescriptorIndex);
			}
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x0007A200 File Offset: 0x00079200
		private void listViewCannedMasks_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			switch (this.listViewSortOrder)
			{
			case SortOrder.None:
			case SortOrder.Descending:
				this.listViewSortOrder = SortOrder.Ascending;
				break;
			case SortOrder.Ascending:
				this.listViewSortOrder = SortOrder.Descending;
				break;
			}
			this.UpdateSortedListView((MaskDescriptorComparer.SortType)e.Column);
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x0007A244 File Offset: 0x00079244
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (this.checkBoxUseValidatingType.Checked)
			{
				this.mtpValidatingType = this.maskedTextBox.ValidatingType;
				return;
			}
			this.mtpValidatingType = null;
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x0007A26C File Offset: 0x0007926C
		private void listViewCannedMasks_Enter(object sender, EventArgs e)
		{
			if (this.listViewCannedMasks.FocusedItem == null && this.listViewCannedMasks.Items.Count > 0)
			{
				this.listViewCannedMasks.Items[0].Focused = true;
			}
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x0007A2A8 File Offset: 0x000792A8
		private void listViewCannedMasks_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.listViewCannedMasks.SelectedItems.Count != 0)
			{
				int num = this.listViewCannedMasks.SelectedIndices[0];
				MaskDescriptor maskDescriptor = this.maskDescriptors[num];
				if (maskDescriptor != this.customMaskDescriptor)
				{
					this.txtBoxMask.Text = maskDescriptor.Mask;
					this.maskedTextBox.Mask = maskDescriptor.Mask;
					this.maskedTextBox.ValidatingType = maskDescriptor.ValidatingType;
					return;
				}
				this.maskedTextBox.ValidatingType = null;
			}
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x0007A32F File Offset: 0x0007932F
		private void MaskDesignerDialog_Load(object sender, EventArgs e)
		{
			this.UpdateSortedListView(MaskDescriptorComparer.SortType.ByName);
			this.SelectMtbMaskDescriptor();
			this.btnCancel.Select();
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x0007A349 File Offset: 0x00079349
		private void maskedTextBox_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			this.errorProvider.SetError(this.maskedTextBox, MaskedTextBoxDesigner.GetMaskInputRejectedErrorMessage(e));
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x0600177A RID: 6010 RVA: 0x0007A362 File Offset: 0x00079362
		private string HelpTopic
		{
			get
			{
				return "net.ComponentModel.MaskPropertyEditor";
			}
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x0007A369 File Offset: 0x00079369
		private void ShowHelp()
		{
			if (this.helpService != null)
			{
				this.helpService.ShowHelpFromKeyword(this.HelpTopic);
			}
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x0007A384 File Offset: 0x00079384
		private void MaskDesignerDialog_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.ShowHelp();
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x0007A393 File Offset: 0x00079393
		private void maskedTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			this.errorProvider.Clear();
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x0007A3A0 File Offset: 0x000793A0
		private void txtBoxMask_Validating(object sender, CancelEventArgs e)
		{
			try
			{
				this.maskedTextBox.Mask = this.txtBoxMask.Text;
			}
			catch (ArgumentException)
			{
			}
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x0007A3D8 File Offset: 0x000793D8
		private void txtBoxMask_TextChanged(object sender, EventArgs e)
		{
			MaskDescriptor maskDescriptor = null;
			if (this.listViewCannedMasks.SelectedItems.Count != 0)
			{
				int num = this.listViewCannedMasks.SelectedIndices[0];
				maskDescriptor = this.maskDescriptors[num];
			}
			if (maskDescriptor == null || (maskDescriptor != this.customMaskDescriptor && maskDescriptor.Mask != this.txtBoxMask.Text))
			{
				this.SetSelectedMaskDescriptor(this.customMaskDescriptor);
			}
		}

		// Token: 0x04001341 RID: 4929
		private List<MaskDescriptor> maskDescriptors = new List<MaskDescriptor>();

		// Token: 0x04001342 RID: 4930
		private MaskDescriptor customMaskDescriptor;

		// Token: 0x04001343 RID: 4931
		private SortOrder listViewSortOrder = SortOrder.Ascending;

		// Token: 0x04001344 RID: 4932
		private Type mtpValidatingType;

		// Token: 0x04001346 RID: 4934
		private IHelpService helpService;
	}
}
