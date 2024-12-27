using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003F2 RID: 1010
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ListControlDesigner : DataBoundControlDesigner
	{
		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002549 RID: 9545 RVA: 0x000C81CC File Offset: 0x000C71CC
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new ListControlActionList(this, base.DataSourceDesigner));
				return designerActionListCollection;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x0600254A RID: 9546 RVA: 0x000C81FF File Offset: 0x000C71FF
		// (set) Token: 0x0600254B RID: 9547 RVA: 0x000C8211 File Offset: 0x000C7211
		public string DataValueField
		{
			get
			{
				return ((global::System.Web.UI.WebControls.ListControl)base.Component).DataValueField;
			}
			set
			{
				((global::System.Web.UI.WebControls.ListControl)base.Component).DataValueField = value;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x0600254C RID: 9548 RVA: 0x000C8224 File Offset: 0x000C7224
		// (set) Token: 0x0600254D RID: 9549 RVA: 0x000C8236 File Offset: 0x000C7236
		public string DataTextField
		{
			get
			{
				return ((global::System.Web.UI.WebControls.ListControl)base.Component).DataTextField;
			}
			set
			{
				((global::System.Web.UI.WebControls.ListControl)base.Component).DataTextField = value;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x0600254E RID: 9550 RVA: 0x000C8249 File Offset: 0x000C7249
		protected override bool UseDataSourcePickerActionList
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x000C824C File Offset: 0x000C724C
		internal void ConnectToDataSourceAction()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConnectToDataSourceCallback), null, SR.GetString("ListControlDesigner_ConnectToDataSource"));
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x000C8270 File Offset: 0x000C7270
		private bool ConnectToDataSourceCallback(object context)
		{
			ListControlConnectToDataSourceDialog listControlConnectToDataSourceDialog = new ListControlConnectToDataSourceDialog(this);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(base.Component.Site, listControlConnectToDataSourceDialog);
			return dialogResult == DialogResult.OK;
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000C829A File Offset: 0x000C729A
		protected override void DataBind(BaseDataBoundControl dataBoundControl)
		{
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000C829C File Offset: 0x000C729C
		internal void EditItems()
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Items"];
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditItemsCallback), propertyDescriptor, SR.GetString("ListControlDesigner_EditItems"), propertyDescriptor);
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000C82E4 File Offset: 0x000C72E4
		private bool EditItemsCallback(object context)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)context;
			ListItemsCollectionEditor listItemsCollectionEditor = new ListItemsCollectionEditor(typeof(ListItemCollection));
			listItemsCollectionEditor.EditValue(new TypeDescriptorContext(designerHost, propertyDescriptor, base.Component), new WindowsFormsEditorServiceHelper(this), propertyDescriptor.GetValue(base.Component));
			return true;
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000C8348 File Offset: 0x000C7348
		public override string GetDesignTimeHtml()
		{
			string text;
			try
			{
				global::System.Web.UI.WebControls.ListControl listControl = (global::System.Web.UI.WebControls.ListControl)base.ViewControl;
				ListItemCollection items = listControl.Items;
				bool flag = this.IsDataBound();
				if (items.Count == 0 || flag)
				{
					if (flag)
					{
						items.Clear();
						items.Add(SR.GetString("Sample_Databound_Text"));
					}
					else
					{
						items.Add(SR.GetString("Sample_Unbound_Text"));
					}
				}
				text = base.GetDesignTimeHtml();
			}
			catch (Exception ex)
			{
				text = this.GetErrorDesignTimeHtml(ex);
			}
			return text;
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000C83D0 File Offset: 0x000C73D0
		public IEnumerable GetResolvedSelectedDataSource()
		{
			return ((IDataSourceProvider)this).GetResolvedSelectedDataSource();
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000C83D8 File Offset: 0x000C73D8
		public object GetSelectedDataSource()
		{
			return ((IDataSourceProvider)this).GetSelectedDataSource();
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000C83E0 File Offset: 0x000C73E0
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(global::System.Web.UI.WebControls.ListControl));
			base.Initialize(component);
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000C83FC File Offset: 0x000C73FC
		private bool IsDataBound()
		{
			return base.DataBindings["DataSource"] != null || base.DataSourceID.Length > 0;
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000C842D File Offset: 0x000C742D
		public virtual void OnDataSourceChanged()
		{
			base.OnDataSourceChanged(true);
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000C8436 File Offset: 0x000C7436
		protected override void OnDataSourceChanged(bool forceUpdateView)
		{
			this.OnDataSourceChanged();
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000C8440 File Offset: 0x000C7440
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			Attribute[] array = new Attribute[]
			{
				new TypeConverterAttribute(typeof(DataFieldConverter))
			};
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["DataTextField"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, array);
			properties["DataTextField"] = propertyDescriptor;
			propertyDescriptor = (PropertyDescriptor)properties["DataValueField"];
			propertyDescriptor = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, array);
			properties["DataValueField"] = propertyDescriptor;
		}
	}
}
