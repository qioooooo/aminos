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
	internal sealed partial class ExpressionBindingsDialog : DesignerForm
	{
		public ExpressionBindingsDialog(IServiceProvider serviceProvider, Control control)
			: base(serviceProvider)
		{
			this._control = control;
			this._controlID = control.ID;
			this.InitializeComponent();
			this.InitializeUserInterface();
		}

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

		private Control Control
		{
			get
			{
				return this._control;
			}
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.Expressions.BindingsDialog";
			}
		}

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

		private void OnExpressionBuilderPropertyGridPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			this.SaveCurrentExpressionBinding();
			this.UpdateUIState();
		}

		protected override void OnInitialActivated(EventArgs e)
		{
			base.OnInitialActivated(e);
			this.LoadExpressionEditors();
			this.LoadBindableProperties();
			this.UpdateUIState();
		}

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

		private const int UnboundImageIndex = 0;

		private const int BoundImageIndex = 1;

		private const int ImplicitBoundImageIndex = 2;

		private static readonly Attribute[] BindablePropertiesFilter = new Attribute[]
		{
			BrowsableAttribute.Yes,
			ReadOnlyAttribute.No
		};

		private string _controlID;

		private bool _bindingsDirty;

		private ExpressionBindingsDialog.ExpressionItem _noneItem;

		private ExpressionBindingsDialog.BindablePropertyNode _currentNode;

		private ExpressionEditor _currentEditor;

		private ExpressionEditorSheet _currentSheet;

		private IDictionary _expressionEditors;

		private bool _internalChange;

		private IDictionary _complexBindings;

		private sealed class ExpressionItem
		{
			public ExpressionItem(string prefix)
			{
				this._prefix = prefix;
			}

			public override string ToString()
			{
				return this._prefix;
			}

			private string _prefix;
		}

		private sealed class BindablePropertyNode : TreeNode
		{
			public BindablePropertyNode(PropertyDescriptor propDesc, ExpressionBinding binding)
			{
				this._binding = binding;
				this._propDesc = propDesc;
				this._isValid = true;
				base.Text = propDesc.Name;
				base.ImageIndex = (base.SelectedImageIndex = (this.IsBound ? (this.IsGenerated ? 2 : 1) : 0));
			}

			public bool IsBound
			{
				get
				{
					return this._binding != null;
				}
			}

			public bool IsGenerated
			{
				get
				{
					return this._binding != null && this._binding.Generated;
				}
			}

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

			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return this._propDesc;
				}
			}

			private PropertyDescriptor _propDesc;

			private ExpressionBinding _binding;

			private bool _isValid;
		}

		private sealed class GenericExpressionEditor : ExpressionEditor
		{
			public override object EvaluateExpression(string expression, object parsedExpressionData, Type propertyType, IServiceProvider serviceProvider)
			{
				return string.Empty;
			}
		}
	}
}
