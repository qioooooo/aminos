using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001DA RID: 474
	internal class DataGridDesigner : ControlDesigner
	{
		// Token: 0x06001243 RID: 4675 RVA: 0x0005ACC8 File Offset: 0x00059CC8
		private DataGridDesigner()
		{
			this.designerVerbs = new DesignerVerbCollection();
			this.designerVerbs.Add(new DesignerVerb(SR.GetString("DataGridAutoFormatString"), new EventHandler(this.OnAutoFormat)));
			base.AutoResizeHandles = true;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0005AD14 File Offset: 0x00059D14
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

		// Token: 0x06001245 RID: 4677 RVA: 0x0005AD7C File Offset: 0x00059D7C
		private void DataSource_ComponentRemoved(object sender, ComponentEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)base.Component;
			if (e.Component == dataGrid.DataSource)
			{
				dataGrid.DataSource = null;
			}
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0005ADAA File Offset: 0x00059DAA
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.changeNotificationService != null)
			{
				this.changeNotificationService.ComponentRemoved -= this.DataSource_ComponentRemoved;
			}
			base.Dispose(disposing);
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001247 RID: 4679 RVA: 0x0005ADD5 File Offset: 0x00059DD5
		public override DesignerVerbCollection Verbs
		{
			get
			{
				return this.designerVerbs;
			}
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0005ADE0 File Offset: 0x00059DE0
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

		// Token: 0x04001111 RID: 4369
		protected DesignerVerbCollection designerVerbs;

		// Token: 0x04001112 RID: 4370
		private IComponentChangeService changeNotificationService;
	}
}
