using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	[ComplexBindingProperties("DataSource", "DataMember")]
	internal class ListControlBoundActionList : DesignerActionList
	{
		public ListControlBoundActionList(ControlDesigner owner)
			: base(owner.Component)
		{
			this._owner = owner;
			ListControl listControl = (ListControl)base.Component;
			if (listControl.DataSource != null)
			{
				this._boundMode = true;
			}
			this.uiService = base.GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
		}

		private void RefreshPanelContent()
		{
			if (this.uiService != null)
			{
				this.uiService.Refresh(this._owner.Component);
			}
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			designerActionItemCollection.Add(new DesignerActionPropertyItem("BoundMode", SR.GetString("BoundModeDisplayName"), SR.GetString("DataCategoryName"), SR.GetString("BoundModeDescription")));
			ListControl listControl = base.Component as ListControl;
			if (this._boundMode || (listControl != null && listControl.DataSource != null))
			{
				this._boundMode = true;
				designerActionItemCollection.Add(new DesignerActionHeaderItem(SR.GetString("BoundModeHeader"), SR.GetString("DataCategoryName")));
				designerActionItemCollection.Add(new DesignerActionPropertyItem("DataSource", SR.GetString("DataSourceDisplayName"), SR.GetString("DataCategoryName"), SR.GetString("DataSourceDescription")));
				designerActionItemCollection.Add(new DesignerActionPropertyItem("DisplayMember", SR.GetString("DisplayMemberDisplayName"), SR.GetString("DataCategoryName"), SR.GetString("DisplayMemberDescription")));
				designerActionItemCollection.Add(new DesignerActionPropertyItem("ValueMember", SR.GetString("ValueMemberDisplayName"), SR.GetString("DataCategoryName"), SR.GetString("ValueMemberDescription")));
				designerActionItemCollection.Add(new DesignerActionPropertyItem("BoundSelectedValue", SR.GetString("BoundSelectedValueDisplayName"), SR.GetString("DataCategoryName"), SR.GetString("BoundSelectedValueDescription")));
				return designerActionItemCollection;
			}
			designerActionItemCollection.Add(new DesignerActionHeaderItem(SR.GetString("UnBoundModeHeader"), SR.GetString("DataCategoryName")));
			designerActionItemCollection.Add(new DesignerActionMethodItem(this, "InvokeItemsDialog", SR.GetString("EditItemDisplayName"), SR.GetString("DataCategoryName"), SR.GetString("EditItemDescription"), true));
			return designerActionItemCollection;
		}

		public bool BoundMode
		{
			get
			{
				return this._boundMode;
			}
			set
			{
				if (!value)
				{
					this.DataSource = null;
				}
				if (this.DataSource == null)
				{
					this._boundMode = value;
				}
				this.RefreshPanelContent();
			}
		}

		public void InvokeItemsDialog()
		{
			EditorServiceContext.EditValue(this._owner, base.Component, "Items");
		}

		[AttributeProvider(typeof(IListSource))]
		public object DataSource
		{
			get
			{
				return ((ListControl)base.Component).DataSource;
			}
			set
			{
				ListControl listControl = (ListControl)base.Component;
				IDesignerHost designerHost = base.GetService(typeof(IDesignerHost)) as IDesignerHost;
				IComponentChangeService componentChangeService = base.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(listControl)["DataSource"];
				if (designerHost != null && componentChangeService != null)
				{
					using (DesignerTransaction designerTransaction = designerHost.CreateTransaction("DGV DataSource TX Name"))
					{
						componentChangeService.OnComponentChanging(base.Component, propertyDescriptor);
						listControl.DataSource = value;
						if (value == null)
						{
							listControl.DisplayMember = "";
							listControl.ValueMember = "";
						}
						componentChangeService.OnComponentChanged(base.Component, propertyDescriptor, null, null);
						designerTransaction.Commit();
						this.RefreshPanelContent();
					}
				}
			}
		}

		private Binding GetSelectedValueBinding()
		{
			ListControl listControl = (ListControl)base.Component;
			Binding binding = null;
			if (listControl.DataBindings != null)
			{
				foreach (object obj in listControl.DataBindings)
				{
					Binding binding2 = (Binding)obj;
					if (binding2.PropertyName == "SelectedValue")
					{
						binding = binding2;
					}
				}
			}
			return binding;
		}

		private void SetSelectedValueBinding(object dataSource, string dataMember)
		{
			ListControl listControl = (ListControl)base.Component;
			IDesignerHost designerHost = base.GetService(typeof(IDesignerHost)) as IDesignerHost;
			IComponentChangeService componentChangeService = base.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(listControl)["DataBindings"];
			if (designerHost != null && componentChangeService != null)
			{
				using (DesignerTransaction designerTransaction = designerHost.CreateTransaction("TextBox DataSource RESX"))
				{
					componentChangeService.OnComponentChanging(this._owner.Component, propertyDescriptor);
					Binding selectedValueBinding = this.GetSelectedValueBinding();
					if (selectedValueBinding != null)
					{
						listControl.DataBindings.Remove(selectedValueBinding);
					}
					if (listControl.DataBindings != null && dataSource != null && !string.IsNullOrEmpty(dataMember))
					{
						listControl.DataBindings.Add("SelectedValue", dataSource, dataMember);
					}
					componentChangeService.OnComponentChanged(this._owner.Component, propertyDescriptor, null, null);
					designerTransaction.Commit();
				}
			}
		}

		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string DisplayMember
		{
			get
			{
				return ((ListControl)base.Component).DisplayMember;
			}
			set
			{
				ListControl listControl = (ListControl)base.Component;
				IDesignerHost designerHost = base.GetService(typeof(IDesignerHost)) as IDesignerHost;
				IComponentChangeService componentChangeService = base.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(listControl)["DisplayMember"];
				if (designerHost != null && componentChangeService != null)
				{
					using (DesignerTransaction designerTransaction = designerHost.CreateTransaction("DGV DataSource TX Name"))
					{
						componentChangeService.OnComponentChanging(base.Component, propertyDescriptor);
						listControl.DisplayMember = value;
						componentChangeService.OnComponentChanged(base.Component, propertyDescriptor, null, null);
						designerTransaction.Commit();
					}
				}
			}
		}

		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string ValueMember
		{
			get
			{
				return ((ListControl)base.Component).ValueMember;
			}
			set
			{
				ListControl listControl = (ListControl)this._owner.Component;
				IDesignerHost designerHost = base.GetService(typeof(IDesignerHost)) as IDesignerHost;
				IComponentChangeService componentChangeService = base.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(listControl)["ValueMember"];
				if (designerHost != null && componentChangeService != null)
				{
					using (DesignerTransaction designerTransaction = designerHost.CreateTransaction("DGV DataSource TX Name"))
					{
						componentChangeService.OnComponentChanging(base.Component, propertyDescriptor);
						listControl.ValueMember = value;
						componentChangeService.OnComponentChanged(base.Component, propertyDescriptor, null, null);
						designerTransaction.Commit();
					}
				}
			}
		}

		[TypeConverter("System.Windows.Forms.Design.DesignBindingConverter")]
		[Editor("System.Windows.Forms.Design.DesignBindingEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public object BoundSelectedValue
		{
			get
			{
				Binding selectedValueBinding = this.GetSelectedValueBinding();
				string text;
				object obj;
				if (selectedValueBinding == null)
				{
					text = null;
					obj = null;
				}
				else
				{
					text = selectedValueBinding.BindingMemberInfo.BindingMember;
					obj = selectedValueBinding.DataSource;
				}
				string text2 = string.Format(CultureInfo.InvariantCulture, "System.Windows.Forms.Design.DesignBinding, {0}", new object[] { typeof(ControlDesigner).Assembly.FullName });
				this._boundSelectedValue = TypeDescriptor.CreateInstance(null, Type.GetType(text2), new Type[]
				{
					typeof(object),
					typeof(string)
				}, new object[] { obj, text });
				return this._boundSelectedValue;
			}
			set
			{
				if (value is string)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["BoundSelectedValue"];
					TypeConverter converter = propertyDescriptor.Converter;
					this._boundSelectedValue = converter.ConvertFrom(new EditorServiceContext(this._owner), CultureInfo.InvariantCulture, value);
					return;
				}
				this._boundSelectedValue = value;
				if (value != null)
				{
					object value2 = TypeDescriptor.GetProperties(this._boundSelectedValue)["DataSource"].GetValue(this._boundSelectedValue);
					string text = (string)TypeDescriptor.GetProperties(this._boundSelectedValue)["DataMember"].GetValue(this._boundSelectedValue);
					this.SetSelectedValueBinding(value2, text);
				}
			}
		}

		private ControlDesigner _owner;

		private bool _boundMode;

		private object _boundSelectedValue;

		private DesignerActionUIService uiService;
	}
}
