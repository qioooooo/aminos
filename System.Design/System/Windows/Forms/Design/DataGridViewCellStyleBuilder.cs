using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal partial class DataGridViewCellStyleBuilder : Form
	{
		public DataGridViewCellStyleBuilder(IServiceProvider serviceProvider, IComponent comp)
		{
			this.InitializeComponent();
			this.InitializeGrids();
			this.listenerDataGridView = new DataGridView();
			this.serviceProvider = serviceProvider;
			this.comp = comp;
			if (this.serviceProvider != null)
			{
				this.helpService = (IHelpService)serviceProvider.GetService(typeof(IHelpService));
			}
			this.cellStyleProperties.Site = new DataGridViewComponentPropertyGridSite(serviceProvider, comp);
		}

		private void InitializeGrids()
		{
			this.sampleDataGridViewSelected.Size = new Size(100, this.Font.Height + 9);
			this.sampleDataGridView.Size = new Size(100, this.Font.Height + 9);
			this.sampleDataGridView.AccessibilityObject.Name = SR.GetString("CellStyleBuilderNormalPreviewAccName");
			DataGridViewRow dataGridViewRow = new DataGridViewRow();
			dataGridViewRow.Cells.Add(new DataGridViewCellStyleBuilder.DialogDataGridViewCell());
			dataGridViewRow.Cells[0].Value = "####";
			dataGridViewRow.Cells[0].AccessibilityObject.Name = SR.GetString("CellStyleBuilderSelectedPreviewAccName");
			this.sampleDataGridViewSelected.Columns.Add(new DataGridViewTextBoxColumn());
			this.sampleDataGridViewSelected.Rows.Add(dataGridViewRow);
			this.sampleDataGridViewSelected.Rows[0].Selected = true;
			this.sampleDataGridViewSelected.AccessibilityObject.Name = SR.GetString("CellStyleBuilderSelectedPreviewAccName");
			dataGridViewRow = new DataGridViewRow();
			dataGridViewRow.Cells.Add(new DataGridViewCellStyleBuilder.DialogDataGridViewCell());
			dataGridViewRow.Cells[0].Value = "####";
			dataGridViewRow.Cells[0].AccessibilityObject.Name = SR.GetString("CellStyleBuilderNormalPreviewAccName");
			this.sampleDataGridView.Columns.Add(new DataGridViewTextBoxColumn());
			this.sampleDataGridView.Rows.Add(dataGridViewRow);
		}

		public DataGridViewCellStyle CellStyle
		{
			get
			{
				return this.cellStyle;
			}
			set
			{
				this.cellStyle = new DataGridViewCellStyle(value);
				this.cellStyleProperties.SelectedObject = this.cellStyle;
				this.ListenerDataGridViewDefaultCellStyleChanged(null, EventArgs.Empty);
				this.listenerDataGridView.DefaultCellStyle = this.cellStyle;
				this.listenerDataGridView.DefaultCellStyleChanged += this.ListenerDataGridViewDefaultCellStyleChanged;
			}
		}

		public ITypeDescriptorContext Context
		{
			set
			{
				this.context = value;
			}
		}

		private void ListenerDataGridViewDefaultCellStyleChanged(object sender, EventArgs e)
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle(this.cellStyle);
			this.sampleDataGridView.DefaultCellStyle = dataGridViewCellStyle;
			this.sampleDataGridViewSelected.DefaultCellStyle = dataGridViewCellStyle;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & Keys.Modifiers) == Keys.None && (keyData & Keys.KeyCode) == Keys.Escape)
			{
				base.Close();
				return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		private void DataGridViewCellStyleBuilder_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.DataGridViewCellStyleBuilder_HelpRequestHandled();
		}

		private void DataGridViewCellStyleBuilder_HelpRequested(object sender, HelpEventArgs e)
		{
			e.Handled = true;
			this.DataGridViewCellStyleBuilder_HelpRequestHandled();
		}

		private void DataGridViewCellStyleBuilder_HelpRequestHandled()
		{
			IHelpService helpService = this.context.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword("vs.CellStyleDialog");
			}
		}

		private void DataGridViewCellStyleBuilder_Load(object sender, EventArgs e)
		{
			this.sampleDataGridView.ClearSelection();
			this.sampleDataGridView.Rows[0].Height = this.sampleDataGridView.Height;
			this.sampleDataGridView.Columns[0].Width = this.sampleDataGridView.Width;
			this.sampleDataGridViewSelected.Rows[0].Height = this.sampleDataGridViewSelected.Height;
			this.sampleDataGridViewSelected.Columns[0].Width = this.sampleDataGridViewSelected.Width;
			this.sampleDataGridView.Layout += this.sampleDataGridView_Layout;
			this.sampleDataGridViewSelected.Layout += this.sampleDataGridView_Layout;
		}

		private void sampleDataGridView_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
		{
			if ((e.StateChanged & DataGridViewElementStates.Selected) != DataGridViewElementStates.None && (e.Cell.State & DataGridViewElementStates.Selected) != DataGridViewElementStates.None)
			{
				this.sampleDataGridView.ClearSelection();
			}
		}

		private void sampleDataGridView_Layout(object sender, LayoutEventArgs e)
		{
			DataGridView dataGridView = (DataGridView)sender;
			dataGridView.Rows[0].Height = dataGridView.Height;
			dataGridView.Columns[0].Width = dataGridView.Width;
		}

		private DataGridView listenerDataGridView;

		private IHelpService helpService;

		private IComponent comp;

		private IServiceProvider serviceProvider;

		private DataGridViewCellStyle cellStyle;

		private ITypeDescriptorContext context;

		private class DialogDataGridViewCell : DataGridViewTextBoxCell
		{
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				if (this.accObj == null)
				{
					this.accObj = new DataGridViewCellStyleBuilder.DialogDataGridViewCell.DialogDataGridViewCellAccessibleObject(this);
				}
				return this.accObj;
			}

			private DataGridViewCellStyleBuilder.DialogDataGridViewCell.DialogDataGridViewCellAccessibleObject accObj;

			private class DialogDataGridViewCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
			{
				public DialogDataGridViewCellAccessibleObject(DataGridViewCell owner)
					: base(owner)
				{
				}

				public override string Name
				{
					get
					{
						return this.name;
					}
					set
					{
						this.name = value;
					}
				}

				private string name = "";
			}
		}
	}
}
