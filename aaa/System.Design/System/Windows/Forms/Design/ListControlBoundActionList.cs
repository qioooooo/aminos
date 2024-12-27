using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000260 RID: 608
	[ComplexBindingProperties("DataSource", "DataMember")]
	internal class ListControlBoundActionList : DesignerActionList
	{
		// Token: 0x06001709 RID: 5897 RVA: 0x00076B58 File Offset: 0x00075B58
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

		// Token: 0x0600170A RID: 5898 RVA: 0x00076BAE File Offset: 0x00075BAE
		private void RefreshPanelContent()
		{
			if (this.uiService != null)
			{
				this.uiService.Refresh(this._owner.Component);
			}
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x00076BD0 File Offset: 0x00075BD0
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

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x00076D6E File Offset: 0x00075D6E
		// (set) Token: 0x0600170D RID: 5901 RVA: 0x00076D76 File Offset: 0x00075D76
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

		// Token: 0x0600170E RID: 5902 RVA: 0x00076D97 File Offset: 0x00075D97
		public void InvokeItemsDialog()
		{
			EditorServiceContext.EditValue(this._owner, base.Component, "Items");
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x00076DB0 File Offset: 0x00075DB0
		// (set) Token: 0x06001710 RID: 5904 RVA: 0x00076DC4 File Offset: 0x00075DC4
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

		// Token: 0x06001711 RID: 5905 RVA: 0x00076E94 File Offset: 0x00075E94
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

		// Token: 0x06001712 RID: 5906 RVA: 0x00076F14 File Offset: 0x00075F14
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

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x00077010 File Offset: 0x00076010
		// (set) Token: 0x06001714 RID: 5908 RVA: 0x00077024 File Offset: 0x00076024
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

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001715 RID: 5909 RVA: 0x000770D8 File Offset: 0x000760D8
		// (set) Token: 0x06001716 RID: 5910 RVA: 0x000770EC File Offset: 0x000760EC
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

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001717 RID: 5911 RVA: 0x000771A4 File Offset: 0x000761A4
		// (set) Token: 0x06001718 RID: 5912 RVA: 0x0007725C File Offset: 0x0007625C
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

		// Token: 0x04001314 RID: 4884
		private ControlDesigner _owner;

		// Token: 0x04001315 RID: 4885
		private bool _boundMode;

		// Token: 0x04001316 RID: 4886
		private object _boundSelectedValue;

		// Token: 0x04001317 RID: 4887
		private DesignerActionUIService uiService;
	}
}
