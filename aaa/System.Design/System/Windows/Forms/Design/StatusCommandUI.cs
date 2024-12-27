using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000296 RID: 662
	internal class StatusCommandUI
	{
		// Token: 0x06001885 RID: 6277 RVA: 0x00081929 File Offset: 0x00080929
		public StatusCommandUI(IServiceProvider provider)
		{
			this.serviceProvider = provider;
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001886 RID: 6278 RVA: 0x00081938 File Offset: 0x00080938
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

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x00081968 File Offset: 0x00080968
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

		// Token: 0x06001888 RID: 6280 RVA: 0x00081998 File Offset: 0x00080998
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

		// Token: 0x06001889 RID: 6281 RVA: 0x00081A3C File Offset: 0x00080A3C
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

		// Token: 0x0600188A RID: 6282 RVA: 0x00081AB7 File Offset: 0x00080AB7
		public void SetStatusInformation(Rectangle bounds)
		{
			if (this.StatusRectCommand != null)
			{
				this.StatusRectCommand.Invoke(bounds);
			}
		}

		// Token: 0x0400143D RID: 5181
		private MenuCommand statusRectCommand;

		// Token: 0x0400143E RID: 5182
		private IMenuCommandService menuService;

		// Token: 0x0400143F RID: 5183
		private IServiceProvider serviceProvider;
	}
}
