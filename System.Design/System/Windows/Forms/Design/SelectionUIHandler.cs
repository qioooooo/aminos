using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	internal abstract class SelectionUIHandler
	{
		public virtual bool BeginDrag(object[] components, SelectionRules rules, int initialX, int initialY)
		{
			this.dragOffset = default(Rectangle);
			this.originalCoords = null;
			this.rules = rules;
			this.dragControls = new Control[components.Length];
			for (int i = 0; i < components.Length; i++)
			{
				this.dragControls[i] = this.GetControl((IComponent)components[i]);
			}
			bool flag = false;
			IComponent component = this.GetComponent();
			for (int j = 0; j < components.Length; j++)
			{
				if (components[j] == component)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Control control = this.GetControl();
				Size currentSnapSize = this.GetCurrentSnapSize();
				Rectangle rectangle = control.RectangleToScreen(control.ClientRectangle);
				rectangle.Inflate(currentSnapSize.Width, currentSnapSize.Height);
				ScrollableControl scrollableControl = this.GetControl() as ScrollableControl;
				if (scrollableControl != null && scrollableControl.AutoScroll)
				{
					Rectangle virtualScreen = SystemInformation.VirtualScreen;
					rectangle.Width = virtualScreen.Width;
					rectangle.Height = virtualScreen.Height;
				}
			}
			return true;
		}

		private void CancelControlMove(Control[] controls, SelectionUIHandler.BoundsInfo[] bounds)
		{
			Rectangle rectangle = default(Rectangle);
			for (int i = 0; i < controls.Length; i++)
			{
				Control parent = controls[i].Parent;
				if (parent != null)
				{
					parent.SuspendLayout();
				}
				rectangle.X = bounds[i].X;
				rectangle.Y = bounds[i].Y;
				rectangle.Width = bounds[i].Width;
				rectangle.Height = bounds[i].Height;
				controls[i].Bounds = rectangle;
			}
			for (int j = 0; j < controls.Length; j++)
			{
				Control parent2 = controls[j].Parent;
				if (parent2 != null)
				{
					parent2.ResumeLayout();
				}
			}
		}

		public virtual void DragMoved(object[] components, Rectangle offset)
		{
			this.dragOffset = offset;
			this.MoveControls(components, false, false);
		}

		public virtual void EndDrag(object[] components, bool cancel)
		{
			try
			{
				this.MoveControls(components, cancel, true);
			}
			catch (CheckoutException ex)
			{
				if (ex != CheckoutException.Canceled)
				{
					throw ex;
				}
				this.MoveControls(components, true, false);
			}
		}

		protected abstract IComponent GetComponent();

		protected abstract Control GetControl();

		protected abstract Control GetControl(IComponent component);

		protected abstract Size GetCurrentSnapSize();

		protected abstract object GetService(Type serviceType);

		protected abstract bool GetShouldSnapToGrid();

		public abstract Rectangle GetUpdatedRect(Rectangle orignalRect, Rectangle dragRect, bool updateSize);

		private void MoveControls(object[] components, bool cancel, bool finalMove)
		{
			Control[] array = this.dragControls;
			Rectangle rectangle = this.dragOffset;
			SelectionUIHandler.BoundsInfo[] array2 = this.originalCoords;
			Point point = default(Point);
			if (finalMove)
			{
				Cursor.Clip = Rectangle.Empty;
				this.dragOffset = Rectangle.Empty;
				this.dragControls = null;
				this.originalCoords = null;
			}
			if (rectangle.IsEmpty)
			{
				return;
			}
			if (finalMove && rectangle.X == 0 && rectangle.Y == 0 && rectangle.Width == 0 && rectangle.Height == 0)
			{
				return;
			}
			if (cancel)
			{
				this.CancelControlMove(array, array2);
				return;
			}
			if (this.originalCoords == null && !finalMove)
			{
				this.originalCoords = new SelectionUIHandler.BoundsInfo[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					this.originalCoords[i] = new SelectionUIHandler.BoundsInfo(array[i]);
				}
				array2 = this.originalCoords;
			}
			for (int j = 0; j < array.Length; j++)
			{
				Control parent = array[j].Parent;
				if (parent != null)
				{
					parent.SuspendLayout();
				}
				SelectionUIHandler.BoundsInfo boundsInfo = array2[j];
				point.X = boundsInfo.lastRequestedX;
				point.Y = boundsInfo.lastRequestedY;
				if (!finalMove)
				{
					boundsInfo.lastRequestedX += rectangle.X;
					boundsInfo.lastRequestedY += rectangle.Y;
					boundsInfo.lastRequestedWidth += rectangle.Width;
					boundsInfo.lastRequestedHeight += rectangle.Height;
				}
				int num = boundsInfo.lastRequestedX;
				int num2 = boundsInfo.lastRequestedY;
				int num3 = boundsInfo.lastRequestedWidth;
				int num4 = boundsInfo.lastRequestedHeight;
				Rectangle bounds = array[j].Bounds;
				if ((this.rules & SelectionRules.Moveable) == SelectionRules.None)
				{
					Size currentSnapSize;
					if (this.GetShouldSnapToGrid())
					{
						currentSnapSize = this.GetCurrentSnapSize();
					}
					else
					{
						currentSnapSize = new Size(1, 1);
					}
					if (num3 < currentSnapSize.Width)
					{
						num3 = currentSnapSize.Width;
						num = bounds.X;
					}
					if (num4 < currentSnapSize.Height)
					{
						num4 = currentSnapSize.Height;
						num2 = bounds.Y;
					}
				}
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (array[j] == designerHost.RootComponent)
				{
					num = 0;
					num2 = 0;
				}
				Rectangle updatedRect = this.GetUpdatedRect(bounds, new Rectangle(num, num2, num3, num4), true);
				Rectangle rectangle2 = bounds;
				if ((this.rules & SelectionRules.Moveable) != SelectionRules.None)
				{
					rectangle2.X = updatedRect.X;
					rectangle2.Y = updatedRect.Y;
				}
				else
				{
					if ((this.rules & SelectionRules.TopSizeable) != SelectionRules.None)
					{
						rectangle2.Y = updatedRect.Y;
						rectangle2.Height = updatedRect.Height;
					}
					if ((this.rules & SelectionRules.BottomSizeable) != SelectionRules.None)
					{
						rectangle2.Height = updatedRect.Height;
					}
					if ((this.rules & SelectionRules.LeftSizeable) != SelectionRules.None)
					{
						rectangle2.X = updatedRect.X;
						rectangle2.Width = updatedRect.Width;
					}
					if ((this.rules & SelectionRules.RightSizeable) != SelectionRules.None)
					{
						rectangle2.Width = updatedRect.Width;
					}
				}
				bool flag = rectangle.X != 0 || rectangle.Y != 0;
				bool flag2 = rectangle.Width != 0 || rectangle.Height != 0;
				if (flag && flag2)
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(components[j])["Bounds"];
					if (propertyDescriptor != null && !propertyDescriptor.IsReadOnly)
					{
						if (finalMove)
						{
							object obj = components[j];
							propertyDescriptor.SetValue(obj, rectangle2);
						}
						else
						{
							array[j].Bounds = rectangle2;
						}
						flag2 = (flag = false);
					}
				}
				if (flag)
				{
					point.X = rectangle2.X;
					point.Y = rectangle2.Y;
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(components[j])["TrayLocation"];
					if (propertyDescriptor2 != null && !propertyDescriptor2.IsReadOnly)
					{
						propertyDescriptor2.SetValue(components[j], point);
					}
					else
					{
						PropertyDescriptor propertyDescriptor3 = TypeDescriptor.GetProperties(components[j])["Left"];
						PropertyDescriptor propertyDescriptor4 = TypeDescriptor.GetProperties(components[j])["Top"];
						if (propertyDescriptor4 != null && !propertyDescriptor4.IsReadOnly)
						{
							if (finalMove)
							{
								object obj2 = components[j];
								propertyDescriptor4.SetValue(obj2, point.Y);
							}
							else
							{
								array[j].Top = point.Y;
							}
						}
						if (propertyDescriptor3 != null && !propertyDescriptor3.IsReadOnly)
						{
							if (finalMove)
							{
								object obj3 = components[j];
								propertyDescriptor3.SetValue(obj3, point.X);
							}
							else
							{
								array[j].Left = point.X;
							}
						}
						if (propertyDescriptor3 == null || propertyDescriptor4 == null)
						{
							PropertyDescriptor propertyDescriptor5 = TypeDescriptor.GetProperties(components[j])["Location"];
							if (propertyDescriptor5 != null && !propertyDescriptor5.IsReadOnly)
							{
								propertyDescriptor5.SetValue(components[j], point);
							}
						}
					}
				}
				if (flag2)
				{
					Size size = new Size(Math.Max(3, rectangle2.Width), Math.Max(3, rectangle2.Height));
					PropertyDescriptor propertyDescriptor6 = TypeDescriptor.GetProperties(components[j])["Width"];
					PropertyDescriptor propertyDescriptor7 = TypeDescriptor.GetProperties(components[j])["Height"];
					if (propertyDescriptor6 != null && !propertyDescriptor6.IsReadOnly && size.Width != (int)propertyDescriptor6.GetValue(components[j]))
					{
						if (finalMove)
						{
							object obj4 = components[j];
							propertyDescriptor6.SetValue(obj4, size);
						}
						else
						{
							array[j].Width = size.Width;
						}
					}
					if (propertyDescriptor7 != null && !propertyDescriptor7.IsReadOnly && size.Height != (int)propertyDescriptor7.GetValue(components[j]))
					{
						if (finalMove)
						{
							object obj5 = components[j];
							propertyDescriptor7.SetValue(obj5, size);
						}
						else
						{
							array[j].Height = size.Height;
						}
					}
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				Control parent2 = array[k].Parent;
				if (parent2 != null)
				{
					parent2.ResumeLayout();
					parent2.Update();
				}
				array[k].Update();
			}
		}

		public bool QueryBeginDrag(object[] components, SelectionRules rules, int initialX, int initialY)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				try
				{
					if (components != null && components.Length > 0)
					{
						foreach (object obj in components)
						{
							componentChangeService.OnComponentChanging(obj, TypeDescriptor.GetProperties(obj)["Location"]);
							PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj)["Size"];
							if (propertyDescriptor != null && propertyDescriptor.Attributes.Contains(DesignerSerializationVisibilityAttribute.Hidden))
							{
								propertyDescriptor = TypeDescriptor.GetProperties(obj)["ClientSize"];
							}
							componentChangeService.OnComponentChanging(obj, propertyDescriptor);
						}
					}
					else
					{
						componentChangeService.OnComponentChanging(this.GetComponent(), null);
					}
				}
				catch (CheckoutException ex)
				{
					if (ex == CheckoutException.Canceled)
					{
						return false;
					}
					throw ex;
				}
				catch (InvalidOperationException)
				{
					return false;
				}
			}
			return components != null && components.Length > 0;
		}

		public abstract void SetCursor();

		public virtual void OleDragEnter(DragEventArgs de)
		{
		}

		public virtual void OleDragDrop(DragEventArgs de)
		{
		}

		public virtual void OleDragOver(DragEventArgs de)
		{
		}

		public virtual void OleDragLeave()
		{
		}

		private const int MinControlWidth = 3;

		private const int MinControlHeight = 3;

		private Rectangle dragOffset = Rectangle.Empty;

		private Control[] dragControls;

		private SelectionUIHandler.BoundsInfo[] originalCoords;

		private SelectionRules rules;

		private class BoundsInfo
		{
			public BoundsInfo(Control control)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control)["Size"];
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(control)["Location"];
				Size size;
				if (propertyDescriptor != null)
				{
					size = (Size)propertyDescriptor.GetValue(control);
				}
				else
				{
					size = control.Size;
				}
				Point point;
				if (propertyDescriptor2 != null)
				{
					point = (Point)propertyDescriptor2.GetValue(control);
				}
				else
				{
					point = control.Location;
				}
				this.X = point.X;
				this.Y = point.Y;
				this.Width = size.Width;
				this.Height = size.Height;
				this.lastRequestedX = this.X;
				this.lastRequestedY = this.Y;
				this.lastRequestedWidth = this.Width;
				this.lastRequestedHeight = this.Height;
			}

			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"{X=",
					this.X.ToString(CultureInfo.CurrentCulture),
					", Y=",
					this.Y.ToString(CultureInfo.CurrentCulture),
					", Width=",
					this.Width.ToString(CultureInfo.CurrentCulture),
					", Height=",
					this.Height.ToString(CultureInfo.CurrentCulture),
					"}"
				});
			}

			public int X;

			public int Y;

			public int Width;

			public int Height;

			public int lastRequestedX = -1;

			public int lastRequestedY = -1;

			public int lastRequestedWidth = -1;

			public int lastRequestedHeight = -1;
		}
	}
}
