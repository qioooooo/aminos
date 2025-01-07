using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls.ListControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal abstract class BaseDataListPage : ComponentEditorPage
	{
		protected abstract string HelpKeyword { get; }

		protected bool IsDataGridMode
		{
			get
			{
				return this.dataGridMode;
			}
		}

		protected BaseDataList GetBaseControl()
		{
			IComponent selectedComponent = base.GetSelectedComponent();
			return (BaseDataList)selectedComponent;
		}

		protected BaseDataListDesigner GetBaseDesigner()
		{
			BaseDataListDesigner baseDataListDesigner = null;
			IComponent selectedComponent = base.GetSelectedComponent();
			ISite site = selectedComponent.Site;
			IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				object designer = designerHost.GetDesigner(selectedComponent);
				baseDataListDesigner = (BaseDataListDesigner)designer;
			}
			return baseDataListDesigner;
		}

		public override void SetComponent(IComponent component)
		{
			base.SetComponent(component);
			this.dataGridMode = this.GetBaseControl() is global::System.Web.UI.WebControls.DataGrid;
			string @string = SR.GetString("RTL");
			if (!string.Equals(@string, "RTL_False", StringComparison.Ordinal))
			{
				this.RightToLeft = RightToLeft.Yes;
			}
		}

		public override void ShowHelp()
		{
			IComponent selectedComponent = base.GetSelectedComponent();
			ISite site = selectedComponent.Site;
			IHelpService helpService = (IHelpService)site.GetService(typeof(IHelpService));
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword(this.HelpKeyword);
			}
		}

		public override bool SupportsHelp()
		{
			return true;
		}

		private bool dataGridMode;

		protected class DataSourceItem
		{
			public DataSourceItem(string dataSourceName, IEnumerable runtimeDataSource)
			{
				this.runtimeDataSource = runtimeDataSource;
				this.dataSourceName = dataSourceName;
			}

			public PropertyDescriptorCollection Fields
			{
				get
				{
					if (this.dataFields == null)
					{
						IEnumerable enumerable = this.RuntimeDataSource;
						if (enumerable != null)
						{
							this.dataFields = DesignTimeData.GetDataFields(enumerable);
						}
					}
					if (this.dataFields == null)
					{
						this.dataFields = new PropertyDescriptorCollection(null);
					}
					return this.dataFields;
				}
			}

			public virtual bool HasDataMembers
			{
				get
				{
					return false;
				}
			}

			public string Name
			{
				get
				{
					return this.dataSourceName;
				}
			}

			protected virtual object RuntimeComponent
			{
				get
				{
					return this.runtimeDataSource;
				}
			}

			protected virtual IEnumerable RuntimeDataSource
			{
				get
				{
					return this.runtimeDataSource;
				}
			}

			protected void ClearFields()
			{
				this.dataFields = null;
			}

			public override string ToString()
			{
				return this.Name;
			}

			private IEnumerable runtimeDataSource;

			private string dataSourceName;

			private PropertyDescriptorCollection dataFields;
		}

		protected class ListSourceDataSourceItem : BaseDataListPage.DataSourceItem
		{
			public ListSourceDataSourceItem(string dataSourceName, IListSource runtimeListSource)
				: base(dataSourceName, null)
			{
				this.runtimeListSource = runtimeListSource;
			}

			public string CurrentDataMember
			{
				get
				{
					return this.currentDataMember;
				}
				set
				{
					this.currentDataMember = value;
					base.ClearFields();
				}
			}

			public override bool HasDataMembers
			{
				get
				{
					return this.runtimeListSource.ContainsListCollection;
				}
			}

			protected override object RuntimeComponent
			{
				get
				{
					return this.runtimeListSource;
				}
			}

			protected override IEnumerable RuntimeDataSource
			{
				get
				{
					if (this.HasDataMembers)
					{
						return DesignTimeData.GetDataMember(this.runtimeListSource, this.currentDataMember);
					}
					return this.runtimeListSource.GetList();
				}
			}

			private IListSource runtimeListSource;

			private string currentDataMember;
		}
	}
}
