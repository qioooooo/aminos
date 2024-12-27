using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003EC RID: 1004
	internal class DataBoundControlActionList : DesignerActionList
	{
		// Token: 0x0600250C RID: 9484 RVA: 0x000C71DC File Offset: 0x000C61DC
		public DataBoundControlActionList(ControlDesigner controlDesigner, IDataSourceDesigner dataSourceDesigner)
			: base(controlDesigner.Component)
		{
			this._controlDesigner = controlDesigner;
			this._dataSourceDesigner = dataSourceDesigner;
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x0600250D RID: 9485 RVA: 0x000C71F8 File Offset: 0x000C61F8
		// (set) Token: 0x0600250E RID: 9486 RVA: 0x000C71FB File Offset: 0x000C61FB
		public override bool AutoShow
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x0600250F RID: 9487 RVA: 0x000C7200 File Offset: 0x000C6200
		// (set) Token: 0x06002510 RID: 9488 RVA: 0x000C7269 File Offset: 0x000C6269
		[TypeConverter(typeof(DataSourceIDConverter))]
		public string DataSourceID
		{
			get
			{
				string text = null;
				DataBoundControlDesigner dataBoundControlDesigner = this._controlDesigner as DataBoundControlDesigner;
				if (dataBoundControlDesigner != null)
				{
					text = dataBoundControlDesigner.DataSourceID;
				}
				else
				{
					BaseDataListDesigner baseDataListDesigner = this._controlDesigner as BaseDataListDesigner;
					if (baseDataListDesigner != null)
					{
						text = baseDataListDesigner.DataSourceID;
					}
					else
					{
						RepeaterDesigner repeaterDesigner = this._controlDesigner as RepeaterDesigner;
						if (repeaterDesigner != null)
						{
							text = repeaterDesigner.DataSourceID;
						}
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					return SR.GetString("DataSourceIDChromeConverter_NoDataSource");
				}
				return text;
			}
			set
			{
				ControlDesigner.InvokeTransactedChange(this._controlDesigner.Component, new TransactedChangeCallback(this.SetDataSourceIDCallback), value, SR.GetString("DataBoundControlActionList_SetDataSourceIDTransaction"));
			}
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x000C7294 File Offset: 0x000C6294
		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._controlDesigner.Component)["DataSourceID"];
			if (propertyDescriptor != null && propertyDescriptor.IsBrowsable)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("DataSourceID", SR.GetString("BaseDataBoundControl_ConfigureDataVerb"), SR.GetString("BaseDataBoundControl_DataActionGroup"), SR.GetString("BaseDataBoundControl_ConfigureDataVerbDesc")));
			}
			ControlDesigner controlDesigner = this._dataSourceDesigner as ControlDesigner;
			if (controlDesigner != null)
			{
				((DesignerActionPropertyItem)designerActionItemCollection[0]).RelatedComponent = controlDesigner.Component;
			}
			return designerActionItemCollection;
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x000C7324 File Offset: 0x000C6324
		private bool SetDataSourceIDCallback(object context)
		{
			string text = (string)context;
			DataBoundControlDesigner dataBoundControlDesigner = this._controlDesigner as DataBoundControlDesigner;
			if (dataBoundControlDesigner != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(dataBoundControlDesigner.Component)["DataSourceID"];
				propertyDescriptor.SetValue(dataBoundControlDesigner.Component, text);
			}
			else
			{
				BaseDataListDesigner baseDataListDesigner = this._controlDesigner as BaseDataListDesigner;
				if (baseDataListDesigner != null)
				{
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(baseDataListDesigner.Component)["DataSourceID"];
					propertyDescriptor2.SetValue(baseDataListDesigner.Component, text);
				}
				else
				{
					RepeaterDesigner repeaterDesigner = this._controlDesigner as RepeaterDesigner;
					if (repeaterDesigner != null)
					{
						PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(repeaterDesigner.Component)["DataSourceID"];
						propertyDescriptor3.SetValue(repeaterDesigner.Component, text);
					}
				}
			}
			return true;
		}

		// Token: 0x04001974 RID: 6516
		private IDataSourceDesigner _dataSourceDesigner;

		// Token: 0x04001975 RID: 6517
		private ControlDesigner _controlDesigner;
	}
}
