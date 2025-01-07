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
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ListControlDesigner : DataBoundControlDesigner
	{
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

		protected override bool UseDataSourcePickerActionList
		{
			get
			{
				return false;
			}
		}

		internal void ConnectToDataSourceAction()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConnectToDataSourceCallback), null, SR.GetString("ListControlDesigner_ConnectToDataSource"));
		}

		private bool ConnectToDataSourceCallback(object context)
		{
			ListControlConnectToDataSourceDialog listControlConnectToDataSourceDialog = new ListControlConnectToDataSourceDialog(this);
			DialogResult dialogResult = UIServiceHelper.ShowDialog(base.Component.Site, listControlConnectToDataSourceDialog);
			return dialogResult == DialogResult.OK;
		}

		protected override void DataBind(BaseDataBoundControl dataBoundControl)
		{
		}

		internal void EditItems()
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Items"];
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditItemsCallback), propertyDescriptor, SR.GetString("ListControlDesigner_EditItems"), propertyDescriptor);
		}

		private bool EditItemsCallback(object context)
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)context;
			ListItemsCollectionEditor listItemsCollectionEditor = new ListItemsCollectionEditor(typeof(ListItemCollection));
			listItemsCollectionEditor.EditValue(new TypeDescriptorContext(designerHost, propertyDescriptor, base.Component), new WindowsFormsEditorServiceHelper(this), propertyDescriptor.GetValue(base.Component));
			return true;
		}

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

		public IEnumerable GetResolvedSelectedDataSource()
		{
			return ((IDataSourceProvider)this).GetResolvedSelectedDataSource();
		}

		public object GetSelectedDataSource()
		{
			return ((IDataSourceProvider)this).GetSelectedDataSource();
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(global::System.Web.UI.WebControls.ListControl));
			base.Initialize(component);
		}

		private bool IsDataBound()
		{
			return base.DataBindings["DataSource"] != null || base.DataSourceID.Length > 0;
		}

		public virtual void OnDataSourceChanged()
		{
			base.OnDataSourceChanged(true);
		}

		protected override void OnDataSourceChanged(bool forceUpdateView)
		{
			this.OnDataSourceChanged();
		}

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
