using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	internal partial class BindingFormattingDialog : Form
	{
		public BindingFormattingDialog()
		{
			this.InitializeComponent();
		}

		public ControlBindingsCollection Bindings
		{
			set
			{
				this.bindings = value;
			}
		}

		private static Bitmap BoundBitmap
		{
			get
			{
				if (BindingFormattingDialog.boundBitmap == null)
				{
					BindingFormattingDialog.boundBitmap = new Bitmap(typeof(BindingFormattingDialog), "BindingFormattingDialog.Bound.bmp");
					BindingFormattingDialog.boundBitmap.MakeTransparent(Color.Red);
				}
				return BindingFormattingDialog.boundBitmap;
			}
		}

		public ITypeDescriptorContext Context
		{
			get
			{
				return this.context;
			}
			set
			{
				this.context = value;
				this.dataSourcePicker.Context = value;
			}
		}

		public bool Dirty
		{
			get
			{
				return this.dirty || this.formatControl1.Dirty;
			}
		}

		public IDesignerHost Host
		{
			set
			{
				this.host = value;
			}
		}

		private static Bitmap UnboundBitmap
		{
			get
			{
				if (BindingFormattingDialog.unboundBitmap == null)
				{
					BindingFormattingDialog.unboundBitmap = new Bitmap(typeof(BindingFormattingDialog), "BindingFormattingDialog.Unbound.bmp");
					BindingFormattingDialog.unboundBitmap.MakeTransparent(Color.Red);
				}
				return BindingFormattingDialog.unboundBitmap;
			}
		}

		private void BindingFormattingDialog_Closing(object sender, CancelEventArgs e)
		{
			this.currentBindingTreeNode = null;
			this.dataSourcePicker.OwnerComponent = null;
			this.formatControl1.ResetFormattingInfo();
		}

		private void BindingFormattingDialog_HelpRequested(object sender, HelpEventArgs e)
		{
			this.BindingFormattingDialog_HelpRequestHandled();
			e.Handled = true;
		}

		private void BindingFormattingDialog_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			this.BindingFormattingDialog_HelpRequestHandled();
			e.Cancel = true;
		}

		private void BindingFormattingDialog_HelpRequestHandled()
		{
			IHelpService helpService = this.context.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword("vs.BindingFormattingDialog");
			}
		}

		private void BindingFormattingDialog_Load(object sender, EventArgs e)
		{
			this.inLoad = true;
			try
			{
				this.dirty = false;
				Font font = Control.DefaultFont;
				IUIService iuiservice = null;
				if (this.bindings.BindableComponent.Site != null)
				{
					iuiservice = (IUIService)this.bindings.BindableComponent.Site.GetService(typeof(IUIService));
				}
				if (iuiservice != null)
				{
					font = (Font)iuiservice.Styles["DialogFont"];
				}
				this.Font = font;
				if (this.propertiesTreeView.ImageList == null)
				{
					ImageList imageList = new ImageList();
					imageList.Images.Add(BindingFormattingDialog.BoundBitmap);
					imageList.Images.Add(BindingFormattingDialog.UnboundBitmap);
					this.propertiesTreeView.ImageList = imageList;
				}
				BindingFormattingDialog.BindingTreeNode bindingTreeNode = null;
				BindingFormattingDialog.BindingTreeNode bindingTreeNode2 = null;
				string text = null;
				string text2 = null;
				AttributeCollection attributes = TypeDescriptor.GetAttributes(this.bindings.BindableComponent);
				foreach (object obj in attributes)
				{
					Attribute attribute = (Attribute)obj;
					if (attribute is DefaultBindingPropertyAttribute)
					{
						text = ((DefaultBindingPropertyAttribute)attribute).Name;
						break;
					}
					if (attribute is DefaultPropertyAttribute)
					{
						text2 = ((DefaultPropertyAttribute)attribute).Name;
					}
				}
				this.propertiesTreeView.Nodes.Clear();
				TreeNode treeNode = new TreeNode(SR.GetString("BindingFormattingDialogCommonTreeNode"));
				TreeNode treeNode2 = new TreeNode(SR.GetString("BindingFormattingDialogAllTreeNode"));
				this.propertiesTreeView.Nodes.Add(treeNode);
				this.propertiesTreeView.Nodes.Add(treeNode2);
				IBindableComponent bindableComponent = this.bindings.BindableComponent;
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(bindableComponent);
				for (int i = 0; i < properties.Count; i++)
				{
					if (!properties[i].IsReadOnly)
					{
						BindableAttribute bindableAttribute = (BindableAttribute)properties[i].Attributes[typeof(BindableAttribute)];
						BrowsableAttribute browsableAttribute = (BrowsableAttribute)properties[i].Attributes[typeof(BrowsableAttribute)];
						if (browsableAttribute == null || browsableAttribute.Browsable || (bindableAttribute != null && bindableAttribute.Bindable))
						{
							BindingFormattingDialog.BindingTreeNode bindingTreeNode3 = new BindingFormattingDialog.BindingTreeNode(properties[i].Name);
							bindingTreeNode3.Binding = this.FindBinding(properties[i].Name);
							if (bindingTreeNode3.Binding != null)
							{
								bindingTreeNode3.FormatType = FormatControl.FormatTypeStringFromFormatString(bindingTreeNode3.Binding.FormatString);
							}
							else
							{
								bindingTreeNode3.FormatType = SR.GetString("BindingFormattingDialogFormatTypeNoFormatting");
							}
							if (bindableAttribute != null && bindableAttribute.Bindable)
							{
								treeNode.Nodes.Add(bindingTreeNode3);
							}
							else
							{
								treeNode2.Nodes.Add(bindingTreeNode3);
							}
							if (bindingTreeNode == null && !string.IsNullOrEmpty(text) && string.Compare(properties[i].Name, text, false, CultureInfo.CurrentCulture) == 0)
							{
								bindingTreeNode = bindingTreeNode3;
							}
							else if (bindingTreeNode2 == null && !string.IsNullOrEmpty(text2) && string.Compare(properties[i].Name, text2, false, CultureInfo.CurrentCulture) == 0)
							{
								bindingTreeNode2 = bindingTreeNode3;
							}
						}
					}
				}
				treeNode.Expand();
				treeNode2.Expand();
				this.propertiesTreeView.Sort();
				BindingFormattingDialog.BindingTreeNode bindingTreeNode4;
				if (bindingTreeNode != null)
				{
					bindingTreeNode4 = bindingTreeNode;
				}
				else if (bindingTreeNode2 != null)
				{
					bindingTreeNode4 = bindingTreeNode2;
				}
				else if (treeNode.Nodes.Count > 0)
				{
					bindingTreeNode4 = BindingFormattingDialog.FirstNodeInAlphabeticalOrder(treeNode.Nodes) as BindingFormattingDialog.BindingTreeNode;
				}
				else if (treeNode2.Nodes.Count > 0)
				{
					bindingTreeNode4 = BindingFormattingDialog.FirstNodeInAlphabeticalOrder(treeNode2.Nodes) as BindingFormattingDialog.BindingTreeNode;
				}
				else
				{
					bindingTreeNode4 = null;
				}
				this.propertiesTreeView.SelectedNode = bindingTreeNode4;
				if (bindingTreeNode4 != null)
				{
					bindingTreeNode4.EnsureVisible();
				}
				this.dataSourcePicker.PropertyName = bindingTreeNode4.Text;
				this.dataSourcePicker.Binding = ((bindingTreeNode4 != null) ? bindingTreeNode4.Binding : null);
				this.dataSourcePicker.Enabled = true;
				this.dataSourcePicker.OwnerComponent = this.bindings.BindableComponent;
				this.dataSourcePicker.DefaultDataSourceUpdateMode = this.bindings.DefaultDataSourceUpdateMode;
				if (bindingTreeNode4 != null && bindingTreeNode4.Binding != null)
				{
					this.bindingUpdateDropDown.Enabled = true;
					this.bindingUpdateDropDown.SelectedItem = bindingTreeNode4.Binding.DataSourceUpdateMode;
					this.updateModeLabel.Enabled = true;
					this.formatControl1.Enabled = true;
					this.formatControl1.FormatType = bindingTreeNode4.FormatType;
					FormatControl.FormatTypeClass formatTypeItem = this.formatControl1.FormatTypeItem;
					formatTypeItem.PushFormatStringIntoFormatType(bindingTreeNode4.Binding.FormatString);
					if (bindingTreeNode4.Binding.NullValue != null)
					{
						this.formatControl1.NullValue = bindingTreeNode4.Binding.NullValue.ToString();
					}
					else
					{
						this.formatControl1.NullValue = string.Empty;
					}
				}
				else
				{
					this.bindingUpdateDropDown.Enabled = false;
					this.bindingUpdateDropDown.SelectedItem = this.bindings.DefaultDataSourceUpdateMode;
					this.updateModeLabel.Enabled = false;
					this.formatControl1.Enabled = false;
					this.formatControl1.FormatType = string.Empty;
				}
				this.formatControl1.Dirty = false;
				this.currentBindingTreeNode = this.propertiesTreeView.SelectedNode as BindingFormattingDialog.BindingTreeNode;
			}
			finally
			{
				this.inLoad = false;
			}
		}

		private Binding FindBinding(string propertyName)
		{
			for (int i = 0; i < this.bindings.Count; i++)
			{
				if (string.Equals(propertyName, this.bindings[i].PropertyName, StringComparison.OrdinalIgnoreCase))
				{
					return this.bindings[i];
				}
			}
			return null;
		}

		private static TreeNode FirstNodeInAlphabeticalOrder(TreeNodeCollection nodes)
		{
			if (nodes.Count == 0)
			{
				return null;
			}
			TreeNode treeNode = nodes[0];
			for (int i = 1; i < nodes.Count; i++)
			{
				if (string.Compare(treeNode.Text, nodes[i].Text, false, CultureInfo.CurrentCulture) > 0)
				{
					treeNode = nodes[i];
				}
			}
			return treeNode;
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.dirty = false;
		}

		private void ConsolidateBindingInformation()
		{
			Binding binding = this.dataSourcePicker.Binding;
			if (binding == null)
			{
				return;
			}
			binding.FormattingEnabled = true;
			this.currentBindingTreeNode.Binding = binding;
			this.currentBindingTreeNode.FormatType = this.formatControl1.FormatType;
			FormatControl.FormatTypeClass formatTypeItem = this.formatControl1.FormatTypeItem;
			if (formatTypeItem != null)
			{
				binding.FormatString = formatTypeItem.FormatString;
				binding.NullValue = this.formatControl1.NullValue;
			}
			binding.DataSourceUpdateMode = (DataSourceUpdateMode)this.bindingUpdateDropDown.SelectedItem;
		}

		private void dataSourcePicker_PropertyValueChanged(object sender, EventArgs e)
		{
			if (this.inLoad)
			{
				return;
			}
			BindingFormattingDialog.BindingTreeNode bindingTreeNode = this.propertiesTreeView.SelectedNode as BindingFormattingDialog.BindingTreeNode;
			if (this.dataSourcePicker.Binding == bindingTreeNode.Binding)
			{
				return;
			}
			Binding binding = this.dataSourcePicker.Binding;
			if (binding != null)
			{
				binding.FormattingEnabled = true;
				Binding binding2 = bindingTreeNode.Binding;
				if (binding2 != null)
				{
					binding.FormatString = binding2.FormatString;
					binding.NullValue = binding2.NullValue;
					binding.FormatInfo = binding2.FormatInfo;
				}
			}
			bindingTreeNode.Binding = binding;
			if (binding != null)
			{
				this.formatControl1.Enabled = true;
				this.updateModeLabel.Enabled = true;
				this.bindingUpdateDropDown.Enabled = true;
				this.bindingUpdateDropDown.SelectedItem = binding.DataSourceUpdateMode;
				if (!string.IsNullOrEmpty(this.formatControl1.FormatType))
				{
					this.formatControl1.FormatType = this.formatControl1.FormatType;
				}
				else
				{
					this.formatControl1.FormatType = SR.GetString("BindingFormattingDialogFormatTypeNoFormatting");
				}
			}
			else
			{
				this.formatControl1.Enabled = false;
				this.updateModeLabel.Enabled = false;
				this.bindingUpdateDropDown.Enabled = false;
				this.bindingUpdateDropDown.SelectedItem = this.bindings.DefaultDataSourceUpdateMode;
				this.formatControl1.FormatType = SR.GetString("BindingFormattingDialogFormatTypeNoFormatting");
			}
			this.dirty = true;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (this.currentBindingTreeNode != null)
			{
				this.ConsolidateBindingInformation();
			}
			this.PushChanges();
		}

		private void propertiesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.inLoad)
			{
				return;
			}
			BindingFormattingDialog.BindingTreeNode bindingTreeNode = e.Node as BindingFormattingDialog.BindingTreeNode;
			if (bindingTreeNode == null)
			{
				this.dataSourcePicker.Binding = null;
				this.bindingLabel.Enabled = (this.dataSourcePicker.Enabled = false);
				this.updateModeLabel.Enabled = (this.bindingUpdateDropDown.Enabled = false);
				this.formatControl1.Enabled = false;
				return;
			}
			this.bindingLabel.Enabled = (this.dataSourcePicker.Enabled = true);
			this.dataSourcePicker.PropertyName = bindingTreeNode.Text;
			this.updateModeLabel.Enabled = (this.bindingUpdateDropDown.Enabled = false);
			this.formatControl1.Enabled = false;
			if (bindingTreeNode.Binding != null)
			{
				this.formatControl1.Enabled = true;
				this.formatControl1.FormatType = bindingTreeNode.FormatType;
				FormatControl.FormatTypeClass formatTypeItem = this.formatControl1.FormatTypeItem;
				this.dataSourcePicker.Binding = bindingTreeNode.Binding;
				formatTypeItem.PushFormatStringIntoFormatType(bindingTreeNode.Binding.FormatString);
				if (bindingTreeNode.Binding.NullValue != null)
				{
					this.formatControl1.NullValue = bindingTreeNode.Binding.NullValue.ToString();
				}
				else
				{
					this.formatControl1.NullValue = string.Empty;
				}
				this.bindingUpdateDropDown.SelectedItem = bindingTreeNode.Binding.DataSourceUpdateMode;
				this.updateModeLabel.Enabled = (this.bindingUpdateDropDown.Enabled = true);
			}
			else
			{
				bool flag = this.dirty;
				this.dataSourcePicker.Binding = null;
				this.formatControl1.FormatType = bindingTreeNode.FormatType;
				this.bindingUpdateDropDown.SelectedItem = this.bindings.DefaultDataSourceUpdateMode;
				this.formatControl1.NullValue = null;
				this.dirty = flag;
			}
			this.formatControl1.Dirty = false;
			this.currentBindingTreeNode = bindingTreeNode;
		}

		private void propertiesTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (this.inLoad)
			{
				return;
			}
			if (this.currentBindingTreeNode == null)
			{
				return;
			}
			if (this.dataSourcePicker.Binding == null)
			{
				return;
			}
			if (!this.formatControl1.Enabled)
			{
				return;
			}
			this.ConsolidateBindingInformation();
			this.dirty = this.dirty || this.formatControl1.Dirty;
		}

		private void PushChanges()
		{
			if (!this.Dirty)
			{
				return;
			}
			IComponentChangeService componentChangeService = this.host.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			PropertyDescriptor propertyDescriptor = null;
			IBindableComponent bindableComponent = this.bindings.BindableComponent;
			if (componentChangeService != null && bindableComponent != null)
			{
				propertyDescriptor = TypeDescriptor.GetProperties(bindableComponent)["DataBindings"];
				if (propertyDescriptor != null)
				{
					componentChangeService.OnComponentChanging(bindableComponent, propertyDescriptor);
				}
			}
			this.bindings.Clear();
			TreeNode treeNode = this.propertiesTreeView.Nodes[0];
			for (int i = 0; i < treeNode.Nodes.Count; i++)
			{
				BindingFormattingDialog.BindingTreeNode bindingTreeNode = treeNode.Nodes[i] as BindingFormattingDialog.BindingTreeNode;
				if (bindingTreeNode.Binding != null)
				{
					this.bindings.Add(bindingTreeNode.Binding);
				}
			}
			TreeNode treeNode2 = this.propertiesTreeView.Nodes[1];
			for (int j = 0; j < treeNode2.Nodes.Count; j++)
			{
				BindingFormattingDialog.BindingTreeNode bindingTreeNode2 = treeNode2.Nodes[j] as BindingFormattingDialog.BindingTreeNode;
				if (bindingTreeNode2.Binding != null)
				{
					this.bindings.Add(bindingTreeNode2.Binding);
				}
			}
			if (componentChangeService != null && bindableComponent != null && propertyDescriptor != null)
			{
				componentChangeService.OnComponentChanged(bindableComponent, propertyDescriptor, null, null);
			}
		}

		private void bindingUpdateDropDown_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.inLoad)
			{
				return;
			}
			this.dirty = true;
		}

		private const int BOUNDIMAGEINDEX = 0;

		private const int UNBOUNDIMAGEINDEX = 1;

		private ITypeDescriptorContext context;

		private ControlBindingsCollection bindings;

		private bool inLoad;

		private bool dirty;

		private static Bitmap boundBitmap;

		private static Bitmap unboundBitmap;

		private BindingFormattingDialog.BindingTreeNode currentBindingTreeNode;

		private IDesignerHost host;

		private class BindingTreeNode : TreeNode
		{
			public BindingTreeNode(string name)
				: base(name)
			{
			}

			public Binding Binding
			{
				get
				{
					return this.binding;
				}
				set
				{
					this.binding = value;
					base.ImageIndex = ((this.binding != null) ? 0 : 1);
					base.SelectedImageIndex = ((this.binding != null) ? 0 : 1);
				}
			}

			public string FormatType
			{
				get
				{
					return this.formatType;
				}
				set
				{
					this.formatType = value;
				}
			}

			private Binding binding;

			private string formatType;
		}

		private class TreeNodeComparer : IComparer
		{
			int IComparer.Compare(object o1, object o2)
			{
				TreeNode treeNode = o1 as TreeNode;
				TreeNode treeNode2 = o2 as TreeNode;
				BindingFormattingDialog.BindingTreeNode bindingTreeNode = treeNode as BindingFormattingDialog.BindingTreeNode;
				BindingFormattingDialog.BindingTreeNode bindingTreeNode2 = treeNode2 as BindingFormattingDialog.BindingTreeNode;
				if (bindingTreeNode != null)
				{
					return string.Compare(bindingTreeNode.Text, bindingTreeNode2.Text, false, CultureInfo.CurrentCulture);
				}
				if (string.Compare(treeNode.Text, SR.GetString("BindingFormattingDialogAllTreeNode"), false, CultureInfo.CurrentCulture) == 0)
				{
					if (string.Compare(treeNode2.Text, SR.GetString("BindingFormattingDialogAllTreeNode"), false, CultureInfo.CurrentCulture) == 0)
					{
						return 0;
					}
					return 1;
				}
				else
				{
					if (string.Compare(treeNode2.Text, SR.GetString("BindingFormattingDialogCommonTreeNode"), false, CultureInfo.CurrentCulture) == 0)
					{
						return 0;
					}
					return -1;
				}
			}
		}
	}
}
