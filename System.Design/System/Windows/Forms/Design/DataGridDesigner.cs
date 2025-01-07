using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class DataGridDesigner : ControlDesigner
	{
		private DataGridDesigner()
		{
			this.designerVerbs = new DesignerVerbCollection();
			this.designerVerbs.Add(new DesignerVerb(SR.GetString("DataGridAutoFormatString"), new EventHandler(this.OnAutoFormat)));
			base.AutoResizeHandles = true;
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				this.changeNotificationService = (IComponentChangeService)designerHost.GetService(typeof(IComponentChangeService));
				if (this.changeNotificationService != null)
				{
					this.changeNotificationService.ComponentRemoved += this.DataSource_ComponentRemoved;
				}
			}
		}

		private void DataSource_ComponentRemoved(object sender, ComponentEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)base.Component;
			if (e.Component == dataGrid.DataSource)
			{
				dataGrid.DataSource = null;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.changeNotificationService != null)
			{
				this.changeNotificationService.ComponentRemoved -= this.DataSource_ComponentRemoved;
			}
			base.Dispose(disposing);
		}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				return this.designerVerbs;
			}
		}

		private void OnAutoFormat(object sender, EventArgs e)
		{
			object component = base.Component;
			DataGrid dataGrid = component as DataGrid;
			DataGridAutoFormatDialog dataGridAutoFormatDialog = new DataGridAutoFormatDialog(dataGrid);
			if (dataGridAutoFormatDialog.ShowDialog() == DialogResult.OK)
			{
				DataRow selectedData = dataGridAutoFormatDialog.SelectedData;
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				DesignerTransaction designerTransaction = designerHost.CreateTransaction(SR.GetString("DataGridAutoFormatUndoTitle", new object[] { base.Component.Site.Name }));
				try
				{
					if (selectedData != null)
					{
						PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(DataGrid));
						foreach (object obj in selectedData.Table.Columns)
						{
							DataColumn dataColumn = (DataColumn)obj;
							object obj2 = selectedData[dataColumn];
							PropertyDescriptor propertyDescriptor = properties[dataColumn.ColumnName];
							if (propertyDescriptor != null)
							{
								if (Convert.IsDBNull(obj2) || obj2.ToString().Length == 0)
								{
									propertyDescriptor.ResetValue(dataGrid);
								}
								else
								{
									try
									{
										TypeConverter converter = propertyDescriptor.Converter;
										object obj3 = converter.ConvertFromString(obj2.ToString());
										propertyDescriptor.SetValue(dataGrid, obj3);
									}
									catch
									{
									}
								}
							}
						}
					}
				}
				finally
				{
					designerTransaction.Commit();
				}
				dataGrid.Invalidate();
			}
		}

		protected DesignerVerbCollection designerVerbs;

		private IComponentChangeService changeNotificationService;
	}
}
