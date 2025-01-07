using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class FormDocumentDesigner : DocumentDesigner
	{
		private IButtonControl AcceptButton
		{
			get
			{
				return base.ShadowProperties["AcceptButton"] as IButtonControl;
			}
			set
			{
				((Form)base.Component).AcceptButton = value;
				base.ShadowProperties["AcceptButton"] = value;
			}
		}

		private IButtonControl CancelButton
		{
			get
			{
				return base.ShadowProperties["CancelButton"] as IButtonControl;
			}
			set
			{
				((Form)base.Component).CancelButton = value;
				base.ShadowProperties["CancelButton"] = value;
			}
		}

		private Size AutoScaleBaseSize
		{
			get
			{
				SizeF autoScaleSize = Form.GetAutoScaleSize(((Form)base.Component).Font);
				return new Size((int)Math.Round((double)autoScaleSize.Width), (int)Math.Round((double)autoScaleSize.Height));
			}
			set
			{
				this.autoScaleBaseSize = value;
				base.ShadowProperties["AutoScaleBaseSize"] = value;
			}
		}

		private bool AutoSize
		{
			get
			{
				return this.autoSize;
			}
			set
			{
				this.autoSize = value;
			}
		}

		private bool ShouldSerializeAutoScaleBaseSize()
		{
			return !this.initializing && ((Form)base.Component).AutoScale && base.ShadowProperties.Contains("AutoScaleBaseSize");
		}

		private Size ClientSize
		{
			get
			{
				if (this.initializing)
				{
					return new Size(-1, -1);
				}
				Size clientSize = new Size(-1, -1);
				Form form = base.Component as Form;
				if (form != null)
				{
					clientSize = form.ClientSize;
					if (form.HorizontalScroll.Visible)
					{
						clientSize.Height += SystemInformation.HorizontalScrollBarHeight;
					}
					if (form.VerticalScroll.Visible)
					{
						clientSize.Width += SystemInformation.VerticalScrollBarWidth;
					}
				}
				return clientSize;
			}
			set
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null && designerHost.Loading)
				{
					this.heightDelta = this.GetMenuHeight();
				}
				((Form)base.Component).ClientSize = value;
			}
		}

		private bool IsMdiContainer
		{
			get
			{
				return ((Form)this.Control).IsMdiContainer;
			}
			set
			{
				if (!value)
				{
					base.UnhookChildControls(this.Control);
				}
				((Form)this.Control).IsMdiContainer = value;
				if (value)
				{
					base.HookChildControls(this.Control);
				}
			}
		}

		private bool IsMenuInherited
		{
			get
			{
				if (this.inheritanceAttribute == null && this.Menu != null)
				{
					this.inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(this.Menu)[typeof(InheritanceAttribute)];
					if (this.inheritanceAttribute.Equals(InheritanceAttribute.NotInherited))
					{
						this.isMenuInherited = false;
					}
					else
					{
						this.isMenuInherited = true;
					}
				}
				return this.isMenuInherited;
			}
		}

		internal MainMenu Menu
		{
			get
			{
				return (MainMenu)base.ShadowProperties["Menu"];
			}
			set
			{
				if (value == base.ShadowProperties["Menu"])
				{
					return;
				}
				base.ShadowProperties["Menu"] = value;
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null && !designerHost.Loading)
				{
					this.EnsureMenuEditorService(value);
					if (this.menuEditorService != null)
					{
						this.menuEditorService.SetMenu(value);
					}
				}
				if (this.heightDelta == 0)
				{
					this.heightDelta = this.GetMenuHeight();
				}
			}
		}

		private double Opacity
		{
			get
			{
				return (double)base.ShadowProperties["Opacity"];
			}
			set
			{
				if (value < 0.0 || value > 1.0)
				{
					throw new ArgumentException(SR.GetString("InvalidBoundArgument", new object[]
					{
						"value",
						value.ToString(CultureInfo.CurrentCulture),
						0f.ToString(CultureInfo.CurrentCulture),
						1f.ToString(CultureInfo.CurrentCulture)
					}), "value");
				}
				base.ShadowProperties["Opacity"] = value;
			}
		}

		public override IList SnapLines
		{
			get
			{
				ArrayList arrayList = null;
				base.AddPaddingSnapLines(ref arrayList);
				if (arrayList == null)
				{
					arrayList = new ArrayList(4);
				}
				if (this.Control.Padding == Padding.Empty && arrayList != null)
				{
					int num = 0;
					for (int i = 0; i < arrayList.Count; i++)
					{
						SnapLine snapLine = arrayList[i] as SnapLine;
						if (snapLine != null && snapLine.Filter != null && snapLine.Filter.StartsWith("Padding"))
						{
							if (snapLine.Filter.Equals("Padding.Left") || snapLine.Filter.Equals("Padding.Top"))
							{
								snapLine.AdjustOffset(DesignerUtils.DEFAULTFORMPADDING);
								num++;
							}
							if (snapLine.Filter.Equals("Padding.Right") || snapLine.Filter.Equals("Padding.Bottom"))
							{
								snapLine.AdjustOffset(-DesignerUtils.DEFAULTFORMPADDING);
								num++;
							}
							if (num == 4)
							{
								break;
							}
						}
					}
				}
				return arrayList;
			}
		}

		private Size Size
		{
			get
			{
				return this.Control.Size;
			}
			set
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.Component);
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanging(base.Component, properties["ClientSize"]);
				}
				this.Control.Size = value;
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanged(base.Component, properties["ClientSize"], null, null);
				}
			}
		}

		private bool ShowInTaskbar
		{
			get
			{
				return (bool)base.ShadowProperties["ShowInTaskbar"];
			}
			set
			{
				base.ShadowProperties["ShowInTaskbar"] = value;
			}
		}

		private FormWindowState WindowState
		{
			get
			{
				return (FormWindowState)base.ShadowProperties["WindowState"];
			}
			set
			{
				base.ShadowProperties["WindowState"] = value;
			}
		}

		private void ApplyAutoScaling(SizeF baseVar, Form form)
		{
			if (!baseVar.IsEmpty)
			{
				SizeF autoScaleSize = Form.GetAutoScaleSize(form.Font);
				Size size = new Size((int)Math.Round((double)autoScaleSize.Width), (int)Math.Round((double)autoScaleSize.Height));
				if (baseVar.Equals(size))
				{
					return;
				}
				float num = (float)size.Height / baseVar.Height;
				float num2 = (float)size.Width / baseVar.Width;
				try
				{
					this.inAutoscale = true;
					form.Scale(num2, num);
				}
				finally
				{
					this.inAutoscale = false;
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					designerHost.LoadComplete -= this.OnLoadComplete;
					designerHost.Activated -= this.OnDesignerActivate;
					designerHost.Deactivated -= this.OnDesignerDeactivate;
				}
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentAdded -= this.OnComponentAdded;
					componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
				}
			}
			base.Dispose(disposing);
		}

		internal override void DoProperMenuSelection(ICollection selComponents)
		{
			foreach (object obj in selComponents)
			{
				Menu menu = obj as Menu;
				if (menu != null)
				{
					MenuItem menuItem = menu as MenuItem;
					if (menuItem != null)
					{
						Menu menu2 = this.menuEditorService.GetMenu();
						MenuItem menuItem2 = menuItem;
						while (menuItem2.Parent is MenuItem)
						{
							menuItem2 = (MenuItem)menuItem2.Parent;
						}
						if (menu2 != menuItem2.Parent)
						{
							this.menuEditorService.SetMenu(menuItem2.Parent);
						}
						if (selComponents.Count == 1)
						{
							this.menuEditorService.SetSelection(menuItem);
						}
					}
					else
					{
						this.menuEditorService.SetMenu(menu);
					}
					break;
				}
				if (this.Menu != null && this.Menu.MenuItems.Count == 0)
				{
					this.menuEditorService.SetMenu(null);
				}
				else
				{
					this.menuEditorService.SetMenu(this.Menu);
				}
				NativeMethods.SendMessage(this.Control.Handle, 134, 1, 0);
			}
		}

		protected override void EnsureMenuEditorService(IComponent c)
		{
			if (this.menuEditorService == null && c is Menu)
			{
				this.menuEditorService = (IMenuEditorService)this.GetService(typeof(IMenuEditorService));
			}
		}

		private void EnsureToolStripWindowAdornerService()
		{
			if (this.toolStripAdornerWindowService == null)
			{
				this.toolStripAdornerWindowService = (ToolStripAdornerWindowService)this.GetService(typeof(ToolStripAdornerWindowService));
			}
		}

		private int GetMenuHeight()
		{
			if (this.Menu == null || (this.IsMenuInherited && this.initializing))
			{
				return 0;
			}
			if (this.menuEditorService != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this.menuEditorService)["MenuHeight"];
				if (propertyDescriptor != null)
				{
					return (int)propertyDescriptor.GetValue(this.menuEditorService);
				}
			}
			return SystemInformation.MenuHeight;
		}

		public override void Initialize(IComponent component)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component.GetType())["WindowState"];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(FormWindowState))
			{
				this.WindowState = (FormWindowState)propertyDescriptor.GetValue(component);
			}
			this.initializing = true;
			base.Initialize(component);
			this.initializing = false;
			base.AutoResizeHandles = true;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				designerHost.LoadComplete += this.OnLoadComplete;
				designerHost.Activated += this.OnDesignerActivate;
				designerHost.Deactivated += this.OnDesignerDeactivate;
			}
			Form form = (Form)this.Control;
			form.WindowState = FormWindowState.Normal;
			base.ShadowProperties["AcceptButton"] = form.AcceptButton;
			base.ShadowProperties["CancelButton"] = form.CancelButton;
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentAdded += this.OnComponentAdded;
				componentChangeService.ComponentRemoved += this.OnComponentRemoved;
			}
		}

		private void OnComponentAdded(object source, ComponentEventArgs ce)
		{
			if (ce.Component is Menu)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null && !designerHost.Loading && ce.Component is MainMenu && !this.hasMenu)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Menu"];
					propertyDescriptor.SetValue(base.Component, ce.Component);
					this.hasMenu = true;
				}
			}
			if (ce.Component is ToolStrip && this.toolStripAdornerWindowService == null)
			{
				IDesignerHost designerHost2 = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost2 != null)
				{
					this.EnsureToolStripWindowAdornerService();
				}
			}
		}

		private void OnComponentRemoved(object source, ComponentEventArgs ce)
		{
			if (ce.Component is Menu)
			{
				if (ce.Component == this.Menu)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Menu"];
					propertyDescriptor.SetValue(base.Component, null);
					this.hasMenu = false;
				}
				else if (this.menuEditorService != null && ce.Component == this.menuEditorService.GetMenu())
				{
					this.menuEditorService.SetMenu(this.Menu);
				}
			}
			if (ce.Component is ToolStrip && this.toolStripAdornerWindowService != null)
			{
				this.toolStripAdornerWindowService = null;
			}
			if (ce.Component is IButtonControl)
			{
				if (ce.Component == base.ShadowProperties["AcceptButton"])
				{
					this.AcceptButton = null;
				}
				if (ce.Component == base.ShadowProperties["CancelButton"])
				{
					this.CancelButton = null;
				}
			}
		}

		protected override void OnCreateHandle()
		{
			if (this.Menu != null && this.menuEditorService != null)
			{
				this.menuEditorService.SetMenu(null);
				this.menuEditorService.SetMenu(this.Menu);
			}
			if (this.heightDelta != 0)
			{
				((Form)base.Component).Height += this.heightDelta;
				this.heightDelta = 0;
			}
		}

		private void OnDesignerActivate(object source, EventArgs evevent)
		{
			Control control = this.Control;
			if (control != null && control.IsHandleCreated)
			{
				NativeMethods.SendMessage(control.Handle, 134, 1, 0);
				SafeNativeMethods.RedrawWindow(control.Handle, null, IntPtr.Zero, 1024);
			}
		}

		private void OnDesignerDeactivate(object sender, EventArgs e)
		{
			Control control = this.Control;
			if (control != null && control.IsHandleCreated)
			{
				NativeMethods.SendMessage(control.Handle, 134, 0, 0);
				SafeNativeMethods.RedrawWindow(control.Handle, null, IntPtr.Zero, 1024);
			}
		}

		private void OnLoadComplete(object source, EventArgs evevent)
		{
			Form form = this.Control as Form;
			if (form != null)
			{
				int num = form.ClientSize.Width;
				int num2 = form.ClientSize.Height;
				if (form.HorizontalScroll.Visible && form.AutoScroll)
				{
					num2 += SystemInformation.HorizontalScrollBarHeight;
				}
				if (form.VerticalScroll.Visible && form.AutoScroll)
				{
					num += SystemInformation.VerticalScrollBarWidth;
				}
				this.ApplyAutoScaling(this.autoScaleBaseSize, form);
				this.ClientSize = new Size(num, num2);
				BehaviorService behaviorService = (BehaviorService)this.GetService(typeof(BehaviorService));
				if (behaviorService != null)
				{
					behaviorService.SyncSelection();
				}
				if (this.heightDelta == 0)
				{
					this.heightDelta = this.GetMenuHeight();
				}
				if (this.heightDelta != 0)
				{
					form.Height += this.heightDelta;
					this.heightDelta = 0;
				}
				if (!form.ControlBox && !form.ShowInTaskbar && !string.IsNullOrEmpty(form.Text) && this.Menu != null && !this.IsMenuInherited)
				{
					form.Height += SystemInformation.CaptionHeight + 1;
				}
				form.PerformLayout();
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "Opacity", "Menu", "IsMdiContainer", "Size", "ShowInTaskBar", "WindowState", "AutoSize", "AcceptButton", "CancelButton" };
			Attribute[] array2 = new Attribute[0];
			PropertyDescriptor propertyDescriptor;
			for (int i = 0; i < array.Length; i++)
			{
				propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(FormDocumentDesigner), propertyDescriptor, array2);
				}
			}
			propertyDescriptor = (PropertyDescriptor)properties["AutoScaleBaseSize"];
			if (propertyDescriptor != null)
			{
				properties["AutoScaleBaseSize"] = TypeDescriptor.CreateProperty(typeof(FormDocumentDesigner), propertyDescriptor, new Attribute[] { DesignerSerializationVisibilityAttribute.Visible });
			}
			propertyDescriptor = (PropertyDescriptor)properties["ClientSize"];
			if (propertyDescriptor != null)
			{
				properties["ClientSize"] = TypeDescriptor.CreateProperty(typeof(FormDocumentDesigner), propertyDescriptor, new Attribute[]
				{
					new DefaultValueAttribute(new Size(-1, -1))
				});
			}
		}

		private unsafe void WmWindowPosChanging(ref Message m)
		{
			NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)(void*)m.LParam;
			bool loading = this.inAutoscale;
			if (!loading)
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					loading = designerHost.Loading;
				}
			}
			if (loading && this.Menu != null && (ptr->flags & 1) == 0 && (this.IsMenuInherited || this.inAutoscale))
			{
				this.heightDelta = this.GetMenuHeight();
			}
		}

		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 70)
			{
				this.WmWindowPosChanging(ref m);
			}
			base.WndProc(ref m);
		}

		private Size autoScaleBaseSize = Size.Empty;

		private bool inAutoscale;

		private int heightDelta;

		private bool isMenuInherited;

		private bool hasMenu;

		private InheritanceAttribute inheritanceAttribute;

		private bool initializing;

		private bool autoSize;

		private ToolStripAdornerWindowService toolStripAdornerWindowService;
	}
}
