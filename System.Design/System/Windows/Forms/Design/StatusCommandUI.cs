using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal class StatusCommandUI
	{
		public StatusCommandUI(IServiceProvider provider)
		{
			this.serviceProvider = provider;
		}

		private IMenuCommandService MenuService
		{
			get
			{
				if (this.menuService == null)
				{
					this.menuService = (IMenuCommandService)this.serviceProvider.GetService(typeof(IMenuCommandService));
				}
				return this.menuService;
			}
		}

		private MenuCommand StatusRectCommand
		{
			get
			{
				if (this.statusRectCommand == null && this.MenuService != null)
				{
					this.statusRectCommand = this.MenuService.FindCommand(MenuCommands.SetStatusRectangle);
				}
				return this.statusRectCommand;
			}
		}

		public void SetStatusInformation(Component selectedComponent, Point location)
		{
			if (selectedComponent == null)
			{
				return;
			}
			Rectangle rectangle = Rectangle.Empty;
			Control control = selectedComponent as Control;
			if (control != null)
			{
				rectangle = control.Bounds;
			}
			else
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(selectedComponent)["Bounds"];
				if (propertyDescriptor != null && typeof(Rectangle).IsAssignableFrom(propertyDescriptor.PropertyType))
				{
					rectangle = (Rectangle)propertyDescriptor.GetValue(selectedComponent);
				}
			}
			if (location != Point.Empty)
			{
				rectangle.X = location.X;
				rectangle.Y = location.Y;
			}
			if (this.StatusRectCommand != null)
			{
				this.StatusRectCommand.Invoke(rectangle);
			}
		}

		public void SetStatusInformation(Component selectedComponent)
		{
			if (selectedComponent == null)
			{
				return;
			}
			Rectangle rectangle = Rectangle.Empty;
			Control control = selectedComponent as Control;
			if (control != null)
			{
				rectangle = control.Bounds;
			}
			else
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(selectedComponent)["Bounds"];
				if (propertyDescriptor != null && typeof(Rectangle).IsAssignableFrom(propertyDescriptor.PropertyType))
				{
					rectangle = (Rectangle)propertyDescriptor.GetValue(selectedComponent);
				}
			}
			if (this.StatusRectCommand != null)
			{
				this.StatusRectCommand.Invoke(rectangle);
			}
		}

		public void SetStatusInformation(Rectangle bounds)
		{
			if (this.StatusRectCommand != null)
			{
				this.StatusRectCommand.Invoke(bounds);
			}
		}

		private MenuCommand statusRectCommand;

		private IMenuCommandService menuService;

		private IServiceProvider serviceProvider;
	}
}
