using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Design;
using System.Drawing;
using System.Web.Configuration;
using System.Web.UI.Design.Util;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design
{
	// Token: 0x02000366 RID: 870
	internal sealed partial class ExpressionBindingsDialog : DesignerForm
	{
		// Token: 0x060020B7 RID: 8375 RVA: 0x000B770A File Offset: 0x000B670A
		public ExpressionBindingsDialog(IServiceProvider serviceProvider, Control control)
			: base(serviceProvider)
		{
			this._control = control;
			this._controlID = control.ID;
			this.InitializeComponent();
			this.InitializeUserInterface();
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x060020B8 RID: 8376 RVA: 0x000B7732 File Offset: 0x000B6732
		private ExpressionBindingsDialog.ExpressionItem NoneItem
		{
			get
			{
				if (this._noneItem == null)
				{
					this._noneItem = new ExpressionBindingsDialog.ExpressionItem(SR.GetString("ExpressionBindingsDialog_None"));
				}
				return this._noneItem;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x060020B9 RID: 8377 RVA: 0x000B7757 File Offset: 0x000B6757
		private Control Control
		{
			get
			{
				return this._control;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x060020BA RID: 8378 RVA: 0x000B775F File Offset: 0x000B675F
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.Expressions.BindingsDialog";
			}
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x000B7CF0 File Offset: 0x000B6CF0
		private void InitializeUserInterface()
		{
			string text = string.Empty;
			if (this.Control != null && this.Control.Site != null)
			{
				text = this.Control.Site.Name;
			}
			this.Text = SR.GetString("ExpressionBindingsDialog_Text", new object[] { text });
			this._instructionLabel.Text = SR.GetString("ExpressionBindingsDialog_Inst");
			this._bindablePropsLabels.Text = SR.GetString("ExpressionBindingsDialog_BindableProps");
			this._okButton.Text = SR.GetString("ExpressionBindingsDialog_OK");
			this._cancelButton.Text = SR.GetString("ExpressionBindingsDialog_Cancel");
			this._expressionBuilderLabel.Text = SR.GetString("ExpressionBindingsDialog_ExpressionType");
			this._propertyGridLabel.Text = SR.GetString("ExpressionBindingsDialog_Properties");
			this._generatedHelpLabel.Text = SR.GetString("ExpressionBindingsDialog_GeneratedExpression");
			ImageList imageList = new ImageList();
			imageList.TransparentColor = Color.Fuchsia;
			imageList.ColorDepth = ColorDepth.Depth32Bit;
			imageList.Images.AddStrip(new Bitmap(typeof(ExpressionBindingsDialog), "ExpressionBindableProperties.bmp"));
			this._bindablePropsTree.ImageList = imageList;
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x000B7E20 File Offset: 0x000B6E20
		private void LoadBindableProperties()
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.Control.GetType(), ExpressionBindingsDialog.BindablePropertiesFilter);
			string text = null;
			PropertyDescriptor defaultProperty = TypeDescriptor.GetDefaultProperty(this.Control.GetType());
			if (defaultProperty != null)
			{
				text = defaultProperty.Name;
			}
			TreeNodeCollection nodes = this._bindablePropsTree.Nodes;
			ExpressionBindingCollection expressions = ((IExpressionsAccessor)this.Control).Expressions;
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			foreach (object obj in expressions)
			{
				ExpressionBinding expressionBinding = (ExpressionBinding)obj;
				hashtable[expressionBinding.PropertyName] = expressionBinding;
			}
			TreeNode treeNode = null;
			foreach (object obj2 in properties)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj2;
				if (string.Compare(propertyDescriptor.Name, "ID", StringComparison.OrdinalIgnoreCase) != 0)
				{
					ExpressionBinding expressionBinding2 = null;
					if (hashtable.Contains(propertyDescriptor.Name))
					{
						expressionBinding2 = (ExpressionBinding)hashtable[propertyDescriptor.Name];
						hashtable.Remove(propertyDescriptor.Name);
					}
					TreeNode treeNode2 = new ExpressionBindingsDialog.BindablePropertyNode(propertyDescriptor, expressionBinding2);
					if (propertyDescriptor.Name.Equals(text, StringComparison.OrdinalIgnoreCase))
					{
						treeNode = treeNode2;
					}
					nodes.Add(treeNode2);
				}
			}
			this._complexBindings = hashtable;
			if (treeNode == null && nodes.Count != 0)
			{
				int count = nodes.Count;
				for (int i = 0; i < count; i++)
				{
					ExpressionBindingsDialog.BindablePropertyNode bindablePropertyNode = (ExpressionBindingsDialog.BindablePropertyNode)nodes[i];
					if (bindablePropertyNode.IsBound)
					{
						treeNode = bindablePropertyNode;
						break;
					}
				}
				if (treeNode == null)
				{
					treeNode = nodes[0];
				}
			}
			if (treeNode != null)
			{
				this._bindablePropsTree.SelectedNode = treeNode;
			}
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x000B8008 File Offset: 0x000B7008
		private void LoadExpressionEditors()
		{
			this._expressionEditors = new HybridDictionary(true);
			IWebApplication webApplication = (IWebApplication)base.ServiceProvider.GetService(typeof(IWebApplication));
			if (webApplication != null)
			{
				try
				{
					Configuration configuration = webApplication.OpenWebConfiguration(true);
					if (configuration != null)
					{
						CompilationSection compilationSection = (CompilationSection)configuration.GetSection("system.web/compilation");
						ExpressionBuilderCollection expressionBuilders = compilationSection.ExpressionBuilders;
						foreach (object obj in expressionBuilders)
						{
							ExpressionBuilder expressionBuilder = (ExpressionBuilder)obj;
							string expressionPrefix = expressionBuilder.ExpressionPrefix;
							ExpressionEditor expressionEditor = ExpressionEditor.GetExpressionEditor(expressionPrefix, base.ServiceProvider);
							if (expressionEditor != null)
							{
								this._expressionEditors[expressionPrefix] = expressionEditor;
								this._expressionBuilderComboBox.Items.Add(new ExpressionBindingsDialog.ExpressionItem(expressionPrefix));
							}
						}
					}
				}
				catch
				{
				}
				this._expressionBuilderComboBox.InvalidateDropDownWidth();
			}
			this._expressionBuilderComboBox.Items.Add(this.NoneItem);
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000B8128 File Offset: 0x000B7128
		private void OnBindablePropsTreeAfterSelect(object sender, TreeViewEventArgs e)
		{
			ExpressionBindingsDialog.BindablePropertyNode bindablePropertyNode = (ExpressionBindingsDialog.BindablePropertyNode)this._bindablePropsTree.SelectedNode;
			if (this._currentNode != bindablePropertyNode)
			{
				this._currentNode = bindablePropertyNode;
				if (this._currentNode != null && this._currentNode.IsBound)
				{
					ExpressionBinding binding = this._currentNode.Binding;
					if (this._currentNode.IsGenerated)
					{
						goto IL_0199;
					}
					ExpressionEditor expressionEditor = (ExpressionEditor)this._expressionEditors[binding.ExpressionPrefix];
					if (expressionEditor == null)
					{
						UIServiceHelper.ShowMessage(base.ServiceProvider, SR.GetString("ExpressionBindingsDialog_UndefinedExpressionPrefix", new object[] { binding.ExpressionPrefix }), SR.GetString("ExpressionBindingsDialog_Text", new object[] { this.Control.Site.Name }), MessageBoxButtons.OK);
						expressionEditor = new ExpressionBindingsDialog.GenericExpressionEditor();
					}
					this._currentEditor = expressionEditor;
					this._currentSheet = this._currentEditor.GetExpressionEditorSheet(binding.Expression, base.ServiceProvider);
					this._internalChange = true;
					try
					{
						foreach (object obj in this._expressionBuilderComboBox.Items)
						{
							ExpressionBindingsDialog.ExpressionItem expressionItem = (ExpressionBindingsDialog.ExpressionItem)obj;
							if (string.Equals(expressionItem.ToString(), binding.ExpressionPrefix, StringComparison.OrdinalIgnoreCase))
							{
								this._expressionBuilderComboBox.SelectedItem = expressionItem;
							}
						}
						this._currentNode.IsValid = this._currentSheet.IsValid;
						goto IL_0199;
					}
					finally
					{
						this._internalChange = false;
					}
				}
				this._expressionBuilderComboBox.SelectedItem = this.NoneItem;
				this._currentEditor = null;
				this._currentSheet = null;
				IL_0199:
				this._expressionBuilderPropertyGrid.SelectedObject = this._currentSheet;
				this.UpdateUIState();
			}
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x000B8304 File Offset: 0x000B7304
		private void OnExpressionBuilderComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._internalChange)
			{
				return;
			}
			this._currentSheet = null;
			if (this._expressionBuilderComboBox.SelectedItem != this.NoneItem)
			{
				this._currentEditor = (ExpressionEditor)this._expressionEditors[this._expressionBuilderComboBox.SelectedItem.ToString()];
				if (this._currentNode != null)
				{
					if (this._currentNode.IsBound)
					{
						ExpressionBinding binding = this._currentNode.Binding;
						if (this._expressionEditors[binding.ExpressionPrefix] == this._currentEditor)
						{
							this._currentSheet = this._currentEditor.GetExpressionEditorSheet(binding.Expression, base.ServiceProvider);
						}
					}
					if (this._currentSheet == null)
					{
						this._currentSheet = this._currentEditor.GetExpressionEditorSheet(string.Empty, base.ServiceProvider);
					}
					this._currentNode.IsValid = this._currentSheet.IsValid;
				}
			}
			this.SaveCurrentExpressionBinding();
			this._expressionBuilderPropertyGrid.SelectedObject = this._currentSheet;
			this.UpdateUIState();
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x000B840E File Offset: 0x000B740E
		private void OnExpressionBuilderPropertyGridPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			this.SaveCurrentExpressionBinding();
			this.UpdateUIState();
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x000B841C File Offset: 0x000B741C
		protected override void OnInitialActivated(EventArgs e)
		{
			base.OnInitialActivated(e);
			this.LoadExpressionEditors();
			this.LoadBindableProperties();
			this.UpdateUIState();
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x000B8438 File Offset: 0x000B7438
		private void OnOKButtonClick(object sender, EventArgs e)
		{
			if (this._bindingsDirty)
			{
				ExpressionBindingCollection expressions = ((IExpressionsAccessor)this.Control).Expressions;
				DataBindingCollection dataBindings = ((IDataBindingsAccessor)this.Control).DataBindings;
				expressions.Clear();
				foreach (object obj in this._bindablePropsTree.Nodes)
				{
					ExpressionBindingsDialog.BindablePropertyNode bindablePropertyNode = (ExpressionBindingsDialog.BindablePropertyNode)obj;
					if (bindablePropertyNode.IsBound)
					{
						expressions.Add(bindablePropertyNode.Binding);
						if (dataBindings.Contains(bindablePropertyNode.Binding.PropertyName))
						{
							dataBindings.Remove(bindablePropertyNode.Binding.PropertyName);
						}
					}
				}
				foreach (object obj2 in this._complexBindings.Values)
				{
					ExpressionBinding expressionBinding = (ExpressionBinding)obj2;
					expressions.Add(expressionBinding);
				}
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x000B8558 File Offset: 0x000B7558
		private void SaveCurrentExpressionBinding()
		{
			if (this._expressionBuilderComboBox.SelectedItem == this.NoneItem)
			{
				this._currentNode.Binding = null;
				this._currentNode.IsValid = true;
			}
			else
			{
				string expression = this._currentSheet.GetExpression();
				PropertyDescriptor propertyDescriptor = this._currentNode.PropertyDescriptor;
				string name = propertyDescriptor.Name;
				ExpressionBinding expressionBinding = new ExpressionBinding(name, propertyDescriptor.PropertyType, this._expressionBuilderComboBox.SelectedItem.ToString(), expression);
				this._currentNode.Binding = expressionBinding;
				this._currentNode.IsValid = this._currentSheet.IsValid;
			}
			this._bindingsDirty = true;
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000B85F8 File Offset: 0x000B75F8
		private void UpdateUIState()
		{
			if (this._currentNode == null)
			{
				this._expressionBuilderComboBox.Enabled = false;
				this._expressionBuilderPropertyGrid.Enabled = false;
				this._propertiesPanel.Visible = true;
				this._generatedHelpLabel.Visible = false;
			}
			else
			{
				this._expressionBuilderComboBox.Enabled = true;
				bool flag = this._expressionBuilderComboBox.SelectedItem == this.NoneItem;
				this._expressionBuilderPropertyGrid.Enabled = !flag;
				this._propertyGridLabel.Enabled = !flag;
				this._propertiesPanel.Visible = !this._currentNode.IsGenerated;
				this._generatedHelpLabel.Visible = this._currentNode.IsGenerated;
			}
			this._okButton.Enabled = true;
			foreach (object obj in this._bindablePropsTree.Nodes)
			{
				ExpressionBindingsDialog.BindablePropertyNode bindablePropertyNode = (ExpressionBindingsDialog.BindablePropertyNode)obj;
				if (!bindablePropertyNode.IsValid)
				{
					this._okButton.Enabled = false;
					break;
				}
			}
		}

		// Token: 0x040017EC RID: 6124
		private const int UnboundImageIndex = 0;

		// Token: 0x040017ED RID: 6125
		private const int BoundImageIndex = 1;

		// Token: 0x040017EE RID: 6126
		private const int ImplicitBoundImageIndex = 2;

		// Token: 0x040017EF RID: 6127
		private static readonly Attribute[] BindablePropertiesFilter = new Attribute[]
		{
			BrowsableAttribute.Yes,
			ReadOnlyAttribute.No
		};

		// Token: 0x040017FC RID: 6140
		private string _controlID;

		// Token: 0x040017FD RID: 6141
		private bool _bindingsDirty;

		// Token: 0x040017FE RID: 6142
		private ExpressionBindingsDialog.ExpressionItem _noneItem;

		// Token: 0x040017FF RID: 6143
		private ExpressionBindingsDialog.BindablePropertyNode _currentNode;

		// Token: 0x04001800 RID: 6144
		private ExpressionEditor _currentEditor;

		// Token: 0x04001801 RID: 6145
		private ExpressionEditorSheet _currentSheet;

		// Token: 0x04001802 RID: 6146
		private IDictionary _expressionEditors;

		// Token: 0x04001803 RID: 6147
		private bool _internalChange;

		// Token: 0x04001804 RID: 6148
		private IDictionary _complexBindings;

		// Token: 0x02000367 RID: 871
		private sealed class ExpressionItem
		{
			// Token: 0x060020C7 RID: 8391 RVA: 0x000B8742 File Offset: 0x000B7742
			public ExpressionItem(string prefix)
			{
				this._prefix = prefix;
			}

			// Token: 0x060020C8 RID: 8392 RVA: 0x000B8751 File Offset: 0x000B7751
			public override string ToString()
			{
				return this._prefix;
			}

			// Token: 0x04001805 RID: 6149
			private string _prefix;
		}

		// Token: 0x02000368 RID: 872
		private sealed class BindablePropertyNode : TreeNode
		{
			// Token: 0x060020C9 RID: 8393 RVA: 0x000B875C File Offset: 0x000B775C
			public BindablePropertyNode(PropertyDescriptor propDesc, ExpressionBinding binding)
			{
				this._binding = binding;
				this._propDesc = propDesc;
				this._isValid = true;
				base.Text = propDesc.Name;
				base.ImageIndex = (base.SelectedImageIndex = (this.IsBound ? (this.IsGenerated ? 2 : 1) : 0));
			}

			// Token: 0x170005E1 RID: 1505
			// (get) Token: 0x060020CA RID: 8394 RVA: 0x000B87B6 File Offset: 0x000B77B6
			public bool IsBound
			{
				get
				{
					return this._binding != null;
				}
			}

			// Token: 0x170005E2 RID: 1506
			// (get) Token: 0x060020CB RID: 8395 RVA: 0x000B87C4 File Offset: 0x000B77C4
			public bool IsGenerated
			{
				get
				{
					return this._binding != null && this._binding.Generated;
				}
			}

			// Token: 0x170005E3 RID: 1507
			// (get) Token: 0x060020CC RID: 8396 RVA: 0x000B87DB File Offset: 0x000B77DB
			// (set) Token: 0x060020CD RID: 8397 RVA: 0x000B87E3 File Offset: 0x000B77E3
			public bool IsValid
			{
				get
				{
					return this._isValid;
				}
				set
				{
					this._isValid = value;
				}
			}

			// Token: 0x170005E4 RID: 1508
			// (get) Token: 0x060020CE RID: 8398 RVA: 0x000B87EC File Offset: 0x000B77EC
			// (set) Token: 0x060020CF RID: 8399 RVA: 0x000B87F4 File Offset: 0x000B77F4
			public ExpressionBinding Binding
			{
				get
				{
					return this._binding;
				}
				set
				{
					this._binding = value;
					base.ImageIndex = (base.SelectedImageIndex = (this.IsBound ? 1 : 0));
				}
			}

			// Token: 0x170005E5 RID: 1509
			// (get) Token: 0x060020D0 RID: 8400 RVA: 0x000B8823 File Offset: 0x000B7823
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return this._propDesc;
				}
			}

			// Token: 0x04001806 RID: 6150
			private PropertyDescriptor _propDesc;

			// Token: 0x04001807 RID: 6151
			private ExpressionBinding _binding;

			// Token: 0x04001808 RID: 6152
			private bool _isValid;
		}

		// Token: 0x02000369 RID: 873
		private sealed class GenericExpressionEditor : ExpressionEditor
		{
			// Token: 0x060020D1 RID: 8401 RVA: 0x000B882B File Offset: 0x000B782B
			public override object EvaluateExpression(string expression, object parsedExpressionData, Type propertyType, IServiceProvider serviceProvider)
			{
				return string.Empty;
			}
		}
	}
}
