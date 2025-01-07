using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Globalization;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class DataGridViewColumnDesigner : ComponentDesigner
	{
		private string Name
		{
			get
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)base.Component;
				if (dataGridViewColumn.Site != null)
				{
					return dataGridViewColumn.Site.Name;
				}
				return dataGridViewColumn.Name;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)base.Component;
				if (dataGridViewColumn == null)
				{
					return;
				}
				if (string.Compare(value, dataGridViewColumn.Name, false, CultureInfo.InvariantCulture) == 0)
				{
					return;
				}
				DataGridView dataGridView = dataGridViewColumn.DataGridView;
				IDesignerHost designerHost = null;
				IContainer container = null;
				INameCreationService nameCreationService = null;
				if (dataGridView != null && dataGridView.Site != null)
				{
					designerHost = dataGridView.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
					nameCreationService = dataGridView.Site.GetService(typeof(INameCreationService)) as INameCreationService;
				}
				if (designerHost != null)
				{
					container = designerHost.Container;
				}
				string empty = string.Empty;
				if (dataGridView != null && !DataGridViewAddColumnDialog.ValidName(value, dataGridView.Columns, container, nameCreationService, (this.liveDataGridView != null) ? this.liveDataGridView.Columns : null, true, out empty))
				{
					if (dataGridView != null && dataGridView.Site != null)
					{
						IUIService iuiservice = (IUIService)dataGridView.Site.GetService(typeof(IUIService));
						DataGridViewDesigner.ShowErrorDialog(iuiservice, empty, this.liveDataGridView);
					}
					return;
				}
				if ((designerHost == null || (designerHost != null && !designerHost.Loading)) && base.Component.Site != null)
				{
					base.Component.Site.Name = value;
				}
				dataGridViewColumn.Name = value;
			}
		}

		public DataGridView LiveDataGridView
		{
			set
			{
				this.liveDataGridView = value;
			}
		}

		private bool UserAddedColumn
		{
			get
			{
				return this.userAddedColumn;
			}
			set
			{
				this.userAddedColumn = value;
			}
		}

		private int Width
		{
			get
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)base.Component;
				return dataGridViewColumn.Width;
			}
			set
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)base.Component;
				value = Math.Max(dataGridViewColumn.MinimumWidth, value);
				dataGridViewColumn.Width = value;
			}
		}

		public override void Initialize(IComponent component)
		{
			this.initializing = true;
			base.Initialize(component);
			if (component.Site != null)
			{
				this.selectionService = this.GetService(typeof(ISelectionService)) as ISelectionService;
				this.behaviorService = this.GetService(typeof(BehaviorService)) as BehaviorService;
				if (this.behaviorService != null && this.selectionService != null)
				{
					this.behavior = new DataGridViewColumnDesigner.FilterCutCopyPasteDeleteBehavior(true, this.behaviorService);
					this.UpdateBehavior();
					this.selectionService.SelectionChanged += this.selectionService_SelectionChanged;
				}
			}
			this.initializing = false;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.PopBehavior();
				if (this.selectionService != null)
				{
					this.selectionService.SelectionChanged -= this.selectionService_SelectionChanged;
				}
				this.selectionService = null;
				this.behaviorService = null;
			}
		}

		private void PushBehavior()
		{
			if (!this.behaviorPushed)
			{
				try
				{
					this.behaviorService.PushBehavior(this.behavior);
				}
				finally
				{
					this.behaviorPushed = true;
				}
			}
		}

		private void PopBehavior()
		{
			if (this.behaviorPushed)
			{
				try
				{
					this.behaviorService.PopBehavior(this.behavior);
				}
				finally
				{
					this.behaviorPushed = false;
				}
			}
		}

		private void UpdateBehavior()
		{
			if (this.selectionService != null)
			{
				if (this.selectionService.PrimarySelection != null && base.Component.Equals(this.selectionService.PrimarySelection))
				{
					this.PushBehavior();
					return;
				}
				this.PopBehavior();
			}
		}

		private void selectionService_SelectionChanged(object sender, EventArgs e)
		{
			this.UpdateBehavior();
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["Width"];
			if (propertyDescriptor != null)
			{
				properties["Width"] = TypeDescriptor.CreateProperty(typeof(DataGridViewColumnDesigner), propertyDescriptor, new Attribute[0]);
			}
			propertyDescriptor = (PropertyDescriptor)properties["Name"];
			if (propertyDescriptor != null)
			{
				if (base.Component.Site == null)
				{
					properties["Name"] = TypeDescriptor.CreateProperty(typeof(DataGridViewColumnDesigner), propertyDescriptor, new Attribute[]
					{
						BrowsableAttribute.Yes,
						CategoryAttribute.Design,
						new DescriptionAttribute(SR.GetString("DesignerPropName")),
						new ParenthesizePropertyNameAttribute(true)
					});
				}
				else
				{
					properties["Name"] = TypeDescriptor.CreateProperty(typeof(DataGridViewColumnDesigner), propertyDescriptor, new Attribute[]
					{
						new ParenthesizePropertyNameAttribute(true)
					});
				}
			}
			properties["UserAddedColumn"] = TypeDescriptor.CreateProperty(typeof(DataGridViewColumnDesigner), "UserAddedColumn", typeof(bool), new Attribute[]
			{
				new DefaultValueAttribute(false),
				BrowsableAttribute.No,
				DesignOnlyAttribute.Yes
			});
		}

		private bool ShouldSerializeWidth()
		{
			DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)base.Component;
			return dataGridViewColumn.InheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.Fill && dataGridViewColumn.Width != 100;
		}

		private bool ShouldSerializeName()
		{
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost == null)
			{
				return false;
			}
			if (!this.initializing)
			{
				return base.ShadowProperties.ShouldSerializeValue("Name", null);
			}
			return base.Component != designerHost.RootComponent;
		}

		private const int DATAGRIDVIEWCOLUMN_defaultWidth = 100;

		private bool userAddedColumn;

		private bool initializing;

		private BehaviorService behaviorService;

		private ISelectionService selectionService;

		private DataGridViewColumnDesigner.FilterCutCopyPasteDeleteBehavior behavior;

		private bool behaviorPushed;

		private DataGridView liveDataGridView;

		public class FilterCutCopyPasteDeleteBehavior : Behavior
		{
			public FilterCutCopyPasteDeleteBehavior(bool callParentBehavior, BehaviorService behaviorService)
				: base(callParentBehavior, behaviorService)
			{
			}

			public override MenuCommand FindCommand(CommandID commandId)
			{
				if (commandId.ID == StandardCommands.Copy.ID && commandId.Guid == StandardCommands.Copy.Guid)
				{
					return new MenuCommand(new EventHandler(this.handler), StandardCommands.Copy)
					{
						Enabled = false
					};
				}
				if (commandId.ID == StandardCommands.Paste.ID && commandId.Guid == StandardCommands.Paste.Guid)
				{
					return new MenuCommand(new EventHandler(this.handler), StandardCommands.Paste)
					{
						Enabled = false
					};
				}
				if (commandId.ID == StandardCommands.Delete.ID && commandId.Guid == StandardCommands.Delete.Guid)
				{
					return new MenuCommand(new EventHandler(this.handler), StandardCommands.Delete)
					{
						Enabled = false
					};
				}
				if (commandId.ID == StandardCommands.Cut.ID && commandId.Guid == StandardCommands.Cut.Guid)
				{
					return new MenuCommand(new EventHandler(this.handler), StandardCommands.Cut)
					{
						Enabled = false
					};
				}
				return base.FindCommand(commandId);
			}

			private void handler(object sender, EventArgs e)
			{
			}
		}
	}
}
