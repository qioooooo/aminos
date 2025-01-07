using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Web.UI.Design.WebControls
{
	internal class DataBoundControlActionList : DesignerActionList
	{
		public DataBoundControlActionList(ControlDesigner controlDesigner, IDataSourceDesigner dataSourceDesigner)
			: base(controlDesigner.Component)
		{
			this._controlDesigner = controlDesigner;
			this._dataSourceDesigner = dataSourceDesigner;
		}

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

		private IDataSourceDesigner _dataSourceDesigner;

		private ControlDesigner _controlDesigner;
	}
}
