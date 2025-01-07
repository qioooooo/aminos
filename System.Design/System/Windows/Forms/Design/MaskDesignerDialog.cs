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
	internal partial class MaskDesignerDialog : Form
	{
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

		public string Mask
		{
			get
			{
				return this.maskedTextBox.Mask;
			}
		}

		public Type ValidatingType
		{
			get
			{
				return this.mtpValidatingType;
			}
		}

		public IEnumerator MaskDescriptors
		{
			get
			{
				return this.maskDescriptors.GetEnumerator();
			}
		}

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

		private void SetSelectedMaskDescriptor(MaskDescriptor maskDex)
		{
			int maskDescriptorIndex = this.GetMaskDescriptorIndex(maskDex);
			this.SetSelectedMaskDescriptor(maskDescriptorIndex);
		}

		private void SetSelectedMaskDescriptor(int maskDexIndex)
		{
			if (maskDexIndex >= 0 && this.listViewCannedMasks.Items.Count > maskDexIndex)
			{
				this.listViewCannedMasks.Items[maskDexIndex].Selected = true;
				this.listViewCannedMasks.FocusedItem = this.listViewCannedMasks.Items[maskDexIndex];
				this.listViewCannedMasks.EnsureVisible(maskDexIndex);
			}
		}

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

		private void InsertMaskDescriptor(int index, MaskDescriptor maskDescriptor)
		{
			this.InsertMaskDescriptor(index, maskDescriptor, true);
		}

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

		private void RemoveMaskDescriptor(MaskDescriptor maskDescriptor)
		{
			int maskDescriptorIndex = this.GetMaskDescriptorIndex(maskDescriptor);
			if (maskDescriptorIndex >= 0)
			{
				this.maskDescriptors.RemoveAt(maskDescriptorIndex);
			}
		}

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

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (this.checkBoxUseValidatingType.Checked)
			{
				this.mtpValidatingType = this.maskedTextBox.ValidatingType;
				return;
			}
			this.mtpValidatingType = null;
		}

		private void listViewCannedMasks_Enter(object sender, EventArgs e)
		{
			if (this.listViewCannedMasks.FocusedItem == null && this.listViewCannedMasks.Items.Count > 0)
			{
				this.listViewCannedMasks.Items[0].Focused = true;
			}
		}

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

		private void MaskDesignerDialog_Load(object sender, EventArgs e)
		{
			this.UpdateSortedListView(MaskDescriptorComparer.SortType.ByName);
			this.SelectMtbMaskDescriptor();
			this.btnCancel.Select();
		}

		private void maskedTextBox_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			this.errorProvider.SetError(this.maskedTextBox, MaskedTextBoxDesigner.GetMaskInputRejectedErrorMessage(e));
		}

		private string HelpTopic
		{
			get
			{
				return "net.ComponentModel.MaskPropertyEditor";
			}
		}

		private void ShowHelp()
		{
			if (this.helpService != null)
			{
				this.helpService.ShowHelpFromKeyword(this.HelpTopic);
			}
		}

		private void MaskDesignerDialog_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.ShowHelp();
		}

		private void maskedTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			this.errorProvider.Clear();
		}

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

		private List<MaskDescriptor> maskDescriptors = new List<MaskDescriptor>();

		private MaskDescriptor customMaskDescriptor;

		private SortOrder listViewSortOrder = SortOrder.Ascending;

		private Type mtpValidatingType;

		private IHelpService helpService;
	}
}
