using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms.Design.Behavior;
using Microsoft.Internal.Performance;

namespace System.Windows.Forms.Design
{
	internal class OleDragDropHandler
	{
		public OleDragDropHandler(SelectionUIHandler selectionHandler, IServiceProvider serviceProvider, IOleDragClient client)
		{
			this.serviceProvider = serviceProvider;
			this.selectionHandler = selectionHandler;
			this.client = client;
		}

		public static string DataFormat
		{
			get
			{
				return "CF_XMLCODE";
			}
		}

		public static string ExtraInfoFormat
		{
			get
			{
				return "CF_COMPONENTTYPES";
			}
		}

		public static string NestedToolboxItemFormat
		{
			get
			{
				return "CF_NESTEDTOOLBOXITEM";
			}
		}

		private IComponent GetDragOwnerComponent(IDataObject data)
		{
			if (OleDragDropHandler.currentDrags == null || !OleDragDropHandler.currentDrags.Contains(data))
			{
				return null;
			}
			return OleDragDropHandler.currentDrags[data] as IComponent;
		}

		private static void AddCurrentDrag(IDataObject data, IComponent component)
		{
			if (OleDragDropHandler.currentDrags == null)
			{
				OleDragDropHandler.currentDrags = new Hashtable();
			}
			OleDragDropHandler.currentDrags[data] = component;
		}

		private static void RemoveCurrentDrag(IDataObject data)
		{
			OleDragDropHandler.currentDrags.Remove(data);
		}

		internal IOleDragClient Destination
		{
			get
			{
				return this.client;
			}
		}

		protected virtual bool CanDropDataObject(IDataObject dataObj)
		{
			if (dataObj != null)
			{
				if (!(dataObj is OleDragDropHandler.ComponentDataObjectWrapper))
				{
					try
					{
						object data = dataObj.GetData(OleDragDropHandler.DataFormat, false);
						if (data == null)
						{
							return false;
						}
						IDesignerSerializationService designerSerializationService = (IDesignerSerializationService)this.GetService(typeof(IDesignerSerializationService));
						if (designerSerializationService == null)
						{
							return false;
						}
						ICollection collection = designerSerializationService.Deserialize(data);
						if (collection.Count > 0)
						{
							bool flag = true;
							foreach (object obj in collection)
							{
								if (obj is IComponent)
								{
									flag = flag && this.client.IsDropOk((IComponent)obj);
									if (!flag)
									{
										break;
									}
								}
							}
							return flag;
						}
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
					}
					return false;
				}
				object[] draggingObjects = this.GetDraggingObjects(dataObj, true);
				if (draggingObjects == null)
				{
					return false;
				}
				bool flag2 = true;
				int num = 0;
				while (flag2 && num < draggingObjects.Length)
				{
					flag2 = flag2 && draggingObjects[num] is IComponent && this.client.IsDropOk((IComponent)draggingObjects[num]);
					num++;
				}
				return flag2;
			}
			return false;
		}

		public bool Dragging
		{
			get
			{
				return this.localDrag;
			}
		}

		public static bool FreezePainting
		{
			get
			{
				return OleDragDropHandler.freezePainting;
			}
		}

		public IComponent[] CreateTool(ToolboxItem tool, Control parent, int x, int y, int width, int height, bool hasLocation, bool hasSize)
		{
			return this.CreateTool(tool, parent, x, y, width, height, hasLocation, hasSize, null);
		}

		public IComponent[] CreateTool(ToolboxItem tool, Control parent, int x, int y, int width, int height, bool hasLocation, bool hasSize, ToolboxSnapDragDropEventArgs e)
		{
			IToolboxService toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			IComponent[] array = new IComponent[0];
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			DesignerTransaction designerTransaction = null;
			try
			{
				try
				{
					if (designerHost != null)
					{
						designerTransaction = designerHost.CreateTransaction(SR.GetString("DesignerBatchCreateTool", new object[] { tool.ToString() }));
					}
				}
				catch (CheckoutException ex)
				{
					if (ex == CheckoutException.Canceled)
					{
						return array;
					}
					throw ex;
				}
				try
				{
					try
					{
						if (designerHost != null && this.CurrentlyLocalizing(designerHost.RootComponent))
						{
							IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
							if (iuiservice != null)
							{
								iuiservice.ShowMessage(SR.GetString("LocalizingCannotAdd"));
							}
							array = new IComponent[0];
							return array;
						}
						Hashtable hashtable = new Hashtable();
						if (parent != null)
						{
							hashtable["Parent"] = parent;
						}
						if (parent != null && parent.IsMirrored)
						{
							x += width;
						}
						if (hasLocation)
						{
							hashtable["Location"] = new Point(x, y);
						}
						if (hasSize)
						{
							hashtable["Size"] = new Size(width, height);
						}
						if (e != null)
						{
							hashtable["ToolboxSnapDragDropEventArgs"] = e;
						}
						array = tool.CreateComponents(designerHost, hashtable);
					}
					catch (CheckoutException ex2)
					{
						if (ex2 != CheckoutException.Canceled)
						{
							throw;
						}
						array = new IComponent[0];
					}
					catch (ArgumentException ex3)
					{
						IUIService iuiservice2 = (IUIService)this.GetService(typeof(IUIService));
						if (iuiservice2 != null)
						{
							iuiservice2.ShowError(ex3);
						}
					}
					catch (Exception ex4)
					{
						IUIService iuiservice3 = (IUIService)this.GetService(typeof(IUIService));
						string text = string.Empty;
						if (ex4.InnerException != null)
						{
							text = ex4.InnerException.ToString();
						}
						if (string.IsNullOrEmpty(text))
						{
							text = ex4.ToString();
						}
						if (ex4 is InvalidOperationException)
						{
							text = ex4.Message;
						}
						if (iuiservice3 == null)
						{
							throw;
						}
						iuiservice3.ShowError(ex4, SR.GetString("FailedToCreateComponent", new object[] { tool.DisplayName, text }));
					}
					if (array == null)
					{
						array = new IComponent[0];
					}
				}
				finally
				{
					if (toolboxService != null && tool.Equals(toolboxService.GetSelectedToolboxItem(designerHost)))
					{
						toolboxService.SelectedToolboxItemUsed();
					}
				}
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
				}
				Cursor.Current = cursor;
			}
			if (selectionService != null && array.Length > 0)
			{
				if (designerHost != null)
				{
					designerHost.Activate();
				}
				ArrayList arrayList = new ArrayList(array);
				for (int i = 0; i < array.Length; i++)
				{
					if (!TypeDescriptor.GetAttributes(array[i]).Contains(DesignTimeVisibleAttribute.Yes))
					{
						arrayList.Remove(array[i]);
					}
				}
				selectionService.SetSelectedComponents(arrayList.ToArray(), SelectionTypes.Replace);
			}
			OleDragDropHandler.codemarkers.CodeMarker(CodeMarkerEvent.perfFXDesignCreateComponentEnd);
			return array;
		}

		private bool CurrentlyLocalizing(IComponent rootComponent)
		{
			if (rootComponent != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(rootComponent)["Language"];
				if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(CultureInfo))
				{
					CultureInfo cultureInfo = (CultureInfo)propertyDescriptor.GetValue(rootComponent);
					if (!cultureInfo.Equals(CultureInfo.InvariantCulture))
					{
						return true;
					}
				}
			}
			return false;
		}

		private void DisableDragDropChildren(ICollection controls, ArrayList allowDropCache)
		{
			foreach (object obj in controls)
			{
				Control control = (Control)obj;
				if (control != null)
				{
					if (control.AllowDrop)
					{
						allowDropCache.Add(control);
						control.AllowDrop = false;
					}
					if (control.HasChildren)
					{
						this.DisableDragDropChildren(control.Controls, allowDropCache);
					}
				}
			}
		}

		private Point DrawDragFrames(object[] comps, Point oldOffset, DragDropEffects oldEffect, Point newOffset, DragDropEffects newEffect, bool drawAtNewOffset)
		{
			Rectangle rectangle = Rectangle.Empty;
			Point empty = Point.Empty;
			Control designerControl = this.client.GetDesignerControl();
			if (this.selectionHandler == null)
			{
				return Point.Empty;
			}
			if (comps == null)
			{
				return Point.Empty;
			}
			for (int i = 0; i < comps.Length; i++)
			{
				Control controlForComponent = this.client.GetControlForComponent(comps[i]);
				Color color = SystemColors.Control;
				try
				{
					color = controlForComponent.BackColor;
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
				bool flag = true;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(comps[i])["Location"];
				if (propertyDescriptor != null)
				{
					flag = propertyDescriptor.IsReadOnly;
				}
				if (!oldOffset.IsEmpty && ((oldEffect & DragDropEffects.Move) == DragDropEffects.None || !flag))
				{
					rectangle = controlForComponent.Bounds;
					if (drawAtNewOffset)
					{
						rectangle.X = oldOffset.X;
						rectangle.Y = oldOffset.Y;
					}
					else
					{
						rectangle.Offset(oldOffset.X, oldOffset.Y);
					}
					rectangle = this.selectionHandler.GetUpdatedRect(controlForComponent.Bounds, rectangle, false);
					this.DrawReversibleFrame(designerControl.Handle, rectangle, color);
				}
				if (!newOffset.IsEmpty && ((oldEffect & DragDropEffects.Move) == DragDropEffects.None || !flag))
				{
					rectangle = controlForComponent.Bounds;
					if (drawAtNewOffset)
					{
						rectangle.X = newOffset.X;
						rectangle.Y = newOffset.Y;
					}
					else
					{
						rectangle.Offset(newOffset.X, newOffset.Y);
					}
					rectangle = this.selectionHandler.GetUpdatedRect(controlForComponent.Bounds, rectangle, false);
					this.DrawReversibleFrame(designerControl.Handle, rectangle, color);
				}
			}
			return newOffset;
		}

		private void DrawReversibleFrame(IntPtr handle, Rectangle rectangle, Color backColor)
		{
			if (rectangle.Width == 0)
			{
				rectangle.Width = 5;
			}
			if (rectangle.Height == 0)
			{
				rectangle.Height = 5;
			}
			int num;
			Color color;
			if ((double)backColor.GetBrightness() < 0.5)
			{
				num = 10;
				color = Color.White;
			}
			else
			{
				num = 7;
				color = Color.Black;
			}
			IntPtr dc = UnsafeNativeMethods.GetDC(new HandleRef(null, handle));
			IntPtr intPtr = SafeNativeMethods.CreatePen(NativeMethods.PS_SOLID, 2, ColorTranslator.ToWin32(backColor));
			int num2 = SafeNativeMethods.SetROP2(new HandleRef(null, dc), num);
			IntPtr intPtr2 = SafeNativeMethods.SelectObject(new HandleRef(null, dc), new HandleRef(null, UnsafeNativeMethods.GetStockObject(5)));
			IntPtr intPtr3 = SafeNativeMethods.SelectObject(new HandleRef(null, dc), new HandleRef(null, intPtr));
			SafeNativeMethods.SetBkColor(new HandleRef(null, dc), ColorTranslator.ToWin32(color));
			SafeNativeMethods.Rectangle(new HandleRef(null, dc), rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
			SafeNativeMethods.SetROP2(new HandleRef(null, dc), num2);
			SafeNativeMethods.SelectObject(new HandleRef(null, dc), new HandleRef(null, intPtr2));
			SafeNativeMethods.SelectObject(new HandleRef(null, dc), new HandleRef(null, intPtr3));
			if (intPtr != IntPtr.Zero)
			{
				SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
			}
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
		}

		public bool DoBeginDrag(object[] components, SelectionRules rules, int initialX, int initialY)
		{
			if ((rules & SelectionRules.AllSizeable) != SelectionRules.None || Control.MouseButtons == MouseButtons.None)
			{
				return true;
			}
			Control designerControl = this.client.GetDesignerControl();
			this.localDrag = true;
			this.localDragInside = false;
			this.dragComps = components;
			this.dragBase = new Point(initialX, initialY);
			this.localDragOffset = Point.Empty;
			designerControl.PointToClient(new Point(initialX, initialY));
			DragDropEffects dragDropEffects = DragDropEffects.Copy | DragDropEffects.Move;
			for (int i = 0; i < components.Length; i++)
			{
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(components[i])[typeof(InheritanceAttribute)];
				if (!inheritanceAttribute.Equals(InheritanceAttribute.NotInherited) && !inheritanceAttribute.Equals(InheritanceAttribute.InheritedReadOnly))
				{
					dragDropEffects &= ~DragDropEffects.Move;
					dragDropEffects |= (DragDropEffects)67108864;
				}
			}
			DataObject dataObject = new OleDragDropHandler.ComponentDataObjectWrapper(new OleDragDropHandler.ComponentDataObject(this.client, this.serviceProvider, components, initialX, initialY));
			NativeMethods.MSG msg = default(NativeMethods.MSG);
			while (NativeMethods.PeekMessage(ref msg, IntPtr.Zero, 15, 15, 1))
			{
				NativeMethods.TranslateMessage(ref msg);
				NativeMethods.DispatchMessage(ref msg);
			}
			bool flag = OleDragDropHandler.freezePainting;
			OleDragDropHandler.AddCurrentDrag(dataObject, this.client.Component);
			ArrayList arrayList = new ArrayList();
			foreach (object obj in components)
			{
				Control control = obj as Control;
				if (control != null && control.HasChildren)
				{
					this.DisableDragDropChildren(control.Controls, arrayList);
				}
			}
			DragDropEffects dragDropEffects2 = DragDropEffects.None;
			IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
			DesignerTransaction designerTransaction = null;
			if (designerHost != null)
			{
				designerTransaction = designerHost.CreateTransaction(SR.GetString("DragDropDragComponents", new object[] { components.Length }));
			}
			try
			{
				dragDropEffects2 = designerControl.DoDragDrop(dataObject, dragDropEffects);
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
				}
			}
			finally
			{
				OleDragDropHandler.RemoveCurrentDrag(dataObject);
				foreach (object obj2 in arrayList)
				{
					Control control2 = (Control)obj2;
					control2.AllowDrop = true;
				}
				OleDragDropHandler.freezePainting = flag;
				if (designerTransaction != null)
				{
					((IDisposable)designerTransaction).Dispose();
				}
			}
			bool flag2 = (dragDropEffects2 & DragDropEffects.Move) != DragDropEffects.None || (dragDropEffects2 & (DragDropEffects)67108864) != DragDropEffects.None;
			bool flag3 = flag2 && this.localDragInside;
			ISelectionUIService selectionUIService = (ISelectionUIService)this.GetService(typeof(ISelectionUIService));
			if (selectionUIService != null && selectionUIService.Dragging)
			{
				selectionUIService.EndDrag(!flag3);
			}
			if (!this.localDragOffset.IsEmpty && dragDropEffects2 != DragDropEffects.None)
			{
				this.DrawDragFrames(this.dragComps, this.localDragOffset, this.localDragEffect, Point.Empty, DragDropEffects.None, false);
			}
			this.localDragOffset = Point.Empty;
			this.dragComps = null;
			this.localDrag = (this.localDragInside = false);
			this.dragBase = Point.Empty;
			return false;
		}

		public void DoEndDrag(object[] components, bool cancel)
		{
			this.dragComps = null;
			this.localDrag = false;
			this.localDragInside = false;
		}

		public void DoOleDragDrop(DragEventArgs de)
		{
			OleDragDropHandler.freezePainting = false;
			if (this.selectionHandler == null)
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			if ((this.localDrag && de.X == this.dragBase.X && de.Y == this.dragBase.Y) || de.AllowedEffect == DragDropEffects.None || (!this.localDrag && !this.dragOk))
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			bool flag = (de.AllowedEffect & (DragDropEffects)67108864) != DragDropEffects.None && this.localDragInside;
			bool flag2 = (de.AllowedEffect & DragDropEffects.Move) != DragDropEffects.None || flag;
			bool flag3 = (de.AllowedEffect & DragDropEffects.Copy) != DragDropEffects.None;
			if ((de.Effect & DragDropEffects.Move) != DragDropEffects.None && !flag2)
			{
				de.Effect = DragDropEffects.Copy;
			}
			if ((de.Effect & DragDropEffects.Copy) != DragDropEffects.None && !flag3)
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			if (flag && (de.Effect & DragDropEffects.Move) != DragDropEffects.None)
			{
				de.Effect |= (DragDropEffects)67108866;
			}
			else if ((de.Effect & DragDropEffects.Copy) != DragDropEffects.None)
			{
				de.Effect = DragDropEffects.Copy;
			}
			if (this.forceDrawFrames || this.localDragInside)
			{
				this.localDragOffset = this.DrawDragFrames(this.dragComps, this.localDragOffset, this.localDragEffect, Point.Empty, DragDropEffects.None, this.forceDrawFrames);
				this.forceDrawFrames = false;
			}
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				if (this.dragOk || (this.localDragInside && de.Effect == DragDropEffects.Copy))
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					IContainer container = designerHost.RootComponent.Site.Container;
					IDataObject dataObject = de.Data;
					bool flag4 = false;
					object[] array;
					if (dataObject is OleDragDropHandler.ComponentDataObjectWrapper)
					{
						dataObject = ((OleDragDropHandler.ComponentDataObjectWrapper)dataObject).InnerData;
						OleDragDropHandler.ComponentDataObject componentDataObject = (OleDragDropHandler.ComponentDataObject)dataObject;
						IComponent dragOwnerComponent = this.GetDragOwnerComponent(de.Data);
						bool flag5 = dragOwnerComponent == null || this.client.Component == null || dragOwnerComponent.Site.Container != this.client.Component.Site.Container;
						bool flag6 = false;
						if (de.Effect == DragDropEffects.Copy || flag5)
						{
							componentDataObject.Deserialize(this.serviceProvider, (de.Effect & DragDropEffects.Copy) == DragDropEffects.None);
						}
						else
						{
							flag6 = true;
						}
						flag4 = true;
						array = componentDataObject.Components;
						if (flag6)
						{
							array = this.GetTopLevelComponents(array);
						}
					}
					else
					{
						object data = dataObject.GetData(OleDragDropHandler.DataFormat, true);
						if (data == null)
						{
							array = new IComponent[0];
						}
						else
						{
							dataObject = new OleDragDropHandler.ComponentDataObject(this.client, this.serviceProvider, data);
							array = ((OleDragDropHandler.ComponentDataObject)dataObject).Components;
							flag4 = true;
						}
					}
					if (array != null && array.Length > 0)
					{
						DesignerTransaction designerTransaction = null;
						try
						{
							designerTransaction = designerHost.CreateTransaction(SR.GetString("DragDropDropComponents"));
							if (!this.localDrag)
							{
								designerHost.Activate();
							}
							ArrayList arrayList = new ArrayList();
							for (int i = 0; i < array.Length; i++)
							{
								IComponent component = array[i] as IComponent;
								if (component != null)
								{
									try
									{
										string text = null;
										if (component.Site != null)
										{
											text = component.Site.Name;
										}
										Control control = null;
										if (flag4)
										{
											control = this.client.GetDesignerControl();
											NativeMethods.SendMessage(control.Handle, 11, 0, 0);
										}
										Point point = this.client.GetDesignerControl().PointToClient(new Point(de.X, de.Y));
										PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["TrayLocation"];
										if (propertyDescriptor == null)
										{
											propertyDescriptor = TypeDescriptor.GetProperties(component)["Location"];
										}
										if (propertyDescriptor != null && !propertyDescriptor.IsReadOnly)
										{
											Rectangle rectangle = default(Rectangle);
											Point point2 = (Point)propertyDescriptor.GetValue(component);
											rectangle.X = point.X + point2.X;
											rectangle.Y = point.Y + point2.Y;
											rectangle = this.selectionHandler.GetUpdatedRect(Rectangle.Empty, rectangle, false);
										}
										if (!this.client.AddComponent(component, text, false))
										{
											de.Effect = DragDropEffects.None;
										}
										else if (this.client.GetControlForComponent(component) == null)
										{
											flag4 = false;
										}
										if (flag4)
										{
											ParentControlDesigner parentControlDesigner = this.client as ParentControlDesigner;
											if (parentControlDesigner != null)
											{
												Control controlForComponent = this.client.GetControlForComponent(component);
												point = parentControlDesigner.GetSnappedPoint(controlForComponent.Location);
												controlForComponent.Location = point;
											}
										}
										if (control != null)
										{
											NativeMethods.SendMessage(control.Handle, 11, 1, 0);
											control.Invalidate(true);
										}
										if (TypeDescriptor.GetAttributes(component).Contains(DesignTimeVisibleAttribute.Yes))
										{
											arrayList.Add(component);
										}
									}
									catch (CheckoutException ex)
									{
										if (ex == CheckoutException.Canceled)
										{
											break;
										}
										throw;
									}
								}
							}
							if (designerHost != null)
							{
								designerHost.Activate();
							}
							ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
							selectionService.SetSelectedComponents((object[])arrayList.ToArray(typeof(IComponent)), SelectionTypes.Replace);
							this.localDragInside = false;
						}
						finally
						{
							if (designerTransaction != null)
							{
								designerTransaction.Commit();
							}
						}
					}
				}
				if (this.localDragInside)
				{
					ISelectionUIService selectionUIService = (ISelectionUIService)this.GetService(typeof(ISelectionUIService));
					if (selectionUIService != null && selectionUIService.Dragging && flag2)
					{
						Rectangle rectangle2 = new Rectangle(de.X - this.dragBase.X, de.Y - this.dragBase.Y, 0, 0);
						selectionUIService.DragMoved(rectangle2);
					}
				}
				this.dragOk = false;
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		public void DoOleDragEnter(DragEventArgs de)
		{
			if (this.localDrag || !this.CanDropDataObject(de.Data) || de.AllowedEffect == DragDropEffects.None)
			{
				if (this.localDrag && de.AllowedEffect != DragDropEffects.None)
				{
					this.localDragInside = true;
					if ((de.KeyState & 8) != 0 && (de.AllowedEffect & DragDropEffects.Copy) != DragDropEffects.None && this.client.CanModifyComponents)
					{
						de.Effect = DragDropEffects.Copy;
					}
					bool flag = (de.AllowedEffect & (DragDropEffects)67108864) != DragDropEffects.None && this.localDragInside;
					if (flag)
					{
						de.Effect |= (DragDropEffects)67108864;
					}
					if ((de.AllowedEffect & DragDropEffects.Move) != DragDropEffects.None)
					{
						de.Effect |= DragDropEffects.Move;
						return;
					}
				}
				else
				{
					de.Effect = DragDropEffects.None;
				}
				return;
			}
			if (!this.client.CanModifyComponents)
			{
				return;
			}
			this.dragOk = true;
			if ((de.KeyState & 8) != 0 && (de.AllowedEffect & DragDropEffects.Copy) != DragDropEffects.None)
			{
				de.Effect = DragDropEffects.Copy;
				return;
			}
			if ((de.AllowedEffect & DragDropEffects.Move) != DragDropEffects.None)
			{
				de.Effect = DragDropEffects.Move;
				return;
			}
			de.Effect = DragDropEffects.None;
		}

		public void DoOleDragLeave()
		{
			if (this.localDrag || this.forceDrawFrames)
			{
				this.localDragInside = false;
				this.localDragOffset = this.DrawDragFrames(this.dragComps, this.localDragOffset, this.localDragEffect, Point.Empty, DragDropEffects.None, this.forceDrawFrames);
				if (this.forceDrawFrames && this.dragOk)
				{
					this.dragBase = Point.Empty;
					this.dragComps = null;
				}
				this.forceDrawFrames = false;
			}
			this.dragOk = false;
		}

		public void DoOleDragOver(DragEventArgs de)
		{
			if (!this.localDrag && !this.dragOk)
			{
				de.Effect = DragDropEffects.None;
				return;
			}
			bool flag = (de.KeyState & 8) != 0 && (de.AllowedEffect & DragDropEffects.Copy) != DragDropEffects.None && this.client.CanModifyComponents;
			bool flag2 = (de.AllowedEffect & (DragDropEffects)67108864) != DragDropEffects.None && this.localDragInside;
			bool flag3 = (de.AllowedEffect & DragDropEffects.Move) != DragDropEffects.None || flag2;
			if ((flag || flag3) && (this.localDrag || this.forceDrawFrames))
			{
				Point point = Point.Empty;
				Point point2 = this.client.GetDesignerControl().PointToClient(new Point(de.X, de.Y));
				if (this.forceDrawFrames)
				{
					point = point2;
				}
				else
				{
					point = new Point(de.X - this.dragBase.X, de.Y - this.dragBase.Y);
				}
				if (!this.client.GetDesignerControl().ClientRectangle.Contains(point2))
				{
					flag = false;
					flag3 = false;
					point = this.localDragOffset;
				}
				if (point != this.localDragOffset)
				{
					this.DrawDragFrames(this.dragComps, this.localDragOffset, this.localDragEffect, point, de.Effect, this.forceDrawFrames);
					this.localDragOffset = point;
					this.localDragEffect = de.Effect;
				}
			}
			if (flag)
			{
				de.Effect = DragDropEffects.Copy;
			}
			else if (flag3)
			{
				de.Effect = DragDropEffects.Move;
			}
			else
			{
				de.Effect = DragDropEffects.None;
			}
			if (flag2)
			{
				de.Effect |= (DragDropEffects)67108864;
			}
		}

		public void DoOleGiveFeedback(GiveFeedbackEventArgs e)
		{
			SelectionUIHandler selectionUIHandler = this.selectionHandler;
			e.UseDefaultCursors = (!this.localDragInside && !this.forceDrawFrames) || (e.Effect & DragDropEffects.Copy) != DragDropEffects.None || e.Effect == DragDropEffects.None;
			if (!e.UseDefaultCursors && this.selectionHandler != null)
			{
				this.selectionHandler.SetCursor();
			}
		}

		private object[] GetDraggingObjects(IDataObject dataObj, bool topLevelOnly)
		{
			object[] array = null;
			if (dataObj is OleDragDropHandler.ComponentDataObjectWrapper)
			{
				dataObj = ((OleDragDropHandler.ComponentDataObjectWrapper)dataObj).InnerData;
				OleDragDropHandler.ComponentDataObject componentDataObject = (OleDragDropHandler.ComponentDataObject)dataObj;
				array = componentDataObject.Components;
			}
			if (!topLevelOnly || array == null)
			{
				return array;
			}
			return this.GetTopLevelComponents(array);
		}

		public object[] GetDraggingObjects(IDataObject dataObj)
		{
			return this.GetDraggingObjects(dataObj, false);
		}

		public object[] GetDraggingObjects(DragEventArgs de)
		{
			return this.GetDraggingObjects(de.Data);
		}

		private object[] GetTopLevelComponents(ICollection comps)
		{
			if (!(comps is IList))
			{
				comps = new ArrayList(comps);
			}
			IList list = (IList)comps;
			ArrayList arrayList = new ArrayList();
			foreach (object obj in list)
			{
				Control control = obj as Control;
				if (control == null && obj != null)
				{
					arrayList.Add(obj);
				}
				else if (control != null && (control.Parent == null || !list.Contains(control.Parent)))
				{
					arrayList.Add(obj);
				}
			}
			return arrayList.ToArray();
		}

		protected object GetService(Type t)
		{
			return this.serviceProvider.GetService(t);
		}

		protected virtual void OnInitializeComponent(IComponent comp, int x, int y, int width, int height, bool hasLocation, bool hasSize)
		{
		}

		protected const int AllowLocalMoveOnly = 67108864;

		public const string CF_CODE = "CF_XMLCODE";

		public const string CF_COMPONENTTYPES = "CF_COMPONENTTYPES";

		public const string CF_TOOLBOXITEM = "CF_NESTEDTOOLBOXITEM";

		private SelectionUIHandler selectionHandler;

		private IServiceProvider serviceProvider;

		private IOleDragClient client;

		private bool dragOk;

		private bool forceDrawFrames;

		private bool localDrag;

		private bool localDragInside;

		private Point localDragOffset = Point.Empty;

		private DragDropEffects localDragEffect;

		private object[] dragComps;

		private Point dragBase = Point.Empty;

		private static bool freezePainting = false;

		private static Hashtable currentDrags;

		private static CodeMarkers codemarkers = CodeMarkers.Instance;

		protected class ComponentDataObjectWrapper : DataObject
		{
			public ComponentDataObjectWrapper(OleDragDropHandler.ComponentDataObject dataObject)
				: base(dataObject)
			{
				this.innerData = dataObject;
			}

			public OleDragDropHandler.ComponentDataObject InnerData
			{
				get
				{
					return this.innerData;
				}
			}

			private OleDragDropHandler.ComponentDataObject innerData;
		}

		protected class ComponentDataObject : IDataObject
		{
			public ComponentDataObject(IOleDragClient dragClient, IServiceProvider sp, object[] comps, int x, int y)
			{
				this.serviceProvider = sp;
				this.components = this.GetComponentList(comps, null, -1);
				this.initialX = x;
				this.initialY = y;
				this.dragClient = dragClient;
			}

			public ComponentDataObject(IOleDragClient dragClient, IServiceProvider sp, object serializationData)
			{
				this.serviceProvider = sp;
				this.serializationData = serializationData;
				this.dragClient = dragClient;
			}

			private Stream SerializationStream
			{
				get
				{
					if (this.serializationStream == null && this.Components != null)
					{
						IDesignerSerializationService designerSerializationService = (IDesignerSerializationService)this.serviceProvider.GetService(typeof(IDesignerSerializationService));
						if (designerSerializationService != null)
						{
							object[] array = new object[this.components.Length];
							for (int i = 0; i < this.components.Length; i++)
							{
								array[i] = (IComponent)this.components[i];
							}
							object obj = designerSerializationService.Serialize(array);
							this.serializationStream = new MemoryStream();
							BinaryFormatter binaryFormatter = new BinaryFormatter();
							binaryFormatter.Serialize(this.serializationStream, obj);
							this.serializationStream.Seek(0L, SeekOrigin.Begin);
						}
					}
					return this.serializationStream;
				}
			}

			public object[] Components
			{
				get
				{
					if (this.components == null && (this.serializationStream != null || this.serializationData != null))
					{
						this.Deserialize(null, false);
						if (this.components == null)
						{
							return new object[0];
						}
					}
					return (object[])this.components.Clone();
				}
			}

			private OleDragDropHandler.CfCodeToolboxItem NestedToolboxItem
			{
				get
				{
					if (this.toolboxitemdata == null)
					{
						this.toolboxitemdata = new OleDragDropHandler.CfCodeToolboxItem(this.GetData(OleDragDropHandler.DataFormat));
					}
					return this.toolboxitemdata;
				}
			}

			private object[] GetComponentList(object[] components, ArrayList list, int index)
			{
				if (this.serviceProvider == null)
				{
					return components;
				}
				ISelectionService selectionService = (ISelectionService)this.serviceProvider.GetService(typeof(ISelectionService));
				if (selectionService == null)
				{
					return components;
				}
				ICollection collection;
				if (components == null)
				{
					collection = selectionService.GetSelectedComponents();
				}
				else
				{
					collection = new ArrayList(components);
				}
				IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					ArrayList arrayList = new ArrayList();
					foreach (object obj in collection)
					{
						IComponent component = (IComponent)obj;
						arrayList.Add(component);
						this.GetAssociatedComponents(component, designerHost, arrayList);
					}
					collection = arrayList;
				}
				object[] array = new object[collection.Count];
				collection.CopyTo(array, 0);
				return array;
			}

			private void GetAssociatedComponents(IComponent component, IDesignerHost host, ArrayList list)
			{
				ComponentDesigner componentDesigner = host.GetDesigner(component) as ComponentDesigner;
				if (componentDesigner == null)
				{
					return;
				}
				foreach (object obj in componentDesigner.AssociatedComponents)
				{
					IComponent component2 = (IComponent)obj;
					list.Add(component2);
					this.GetAssociatedComponents(component2, host, list);
				}
			}

			public virtual object GetData(string format)
			{
				return this.GetData(format, false);
			}

			public virtual object GetData(string format, bool autoConvert)
			{
				if (format.Equals(OleDragDropHandler.DataFormat))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					this.SerializationStream.Seek(0L, SeekOrigin.Begin);
					return binaryFormatter.Deserialize(this.SerializationStream);
				}
				if (format.Equals(OleDragDropHandler.NestedToolboxItemFormat))
				{
					this.NestedToolboxItem.SetDisplayName();
					return this.NestedToolboxItem;
				}
				return null;
			}

			public virtual object GetData(Type t)
			{
				return this.GetData(t.FullName);
			}

			public bool GetDataPresent(string format, bool autoConvert)
			{
				return Array.IndexOf<string>(this.GetFormats(), format) != -1;
			}

			public bool GetDataPresent(string format)
			{
				return this.GetDataPresent(format, false);
			}

			public bool GetDataPresent(Type format)
			{
				return this.GetDataPresent(format.FullName, false);
			}

			public string[] GetFormats(bool autoConvert)
			{
				return this.GetFormats();
			}

			public string[] GetFormats()
			{
				return new string[]
				{
					OleDragDropHandler.NestedToolboxItemFormat,
					OleDragDropHandler.DataFormat,
					DataFormats.Serializable,
					OleDragDropHandler.ExtraInfoFormat
				};
			}

			public void Deserialize(IServiceProvider serviceProvider, bool removeCurrentComponents)
			{
				if (serviceProvider == null)
				{
					serviceProvider = this.serviceProvider;
				}
				IDesignerSerializationService designerSerializationService = (IDesignerSerializationService)serviceProvider.GetService(typeof(IDesignerSerializationService));
				IDesignerHost designerHost = null;
				DesignerTransaction designerTransaction = null;
				try
				{
					if (this.serializationData == null)
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						this.serializationData = binaryFormatter.Deserialize(this.SerializationStream);
					}
					if (removeCurrentComponents && this.components != null)
					{
						foreach (IComponent component in this.components)
						{
							if (designerHost == null && component.Site != null)
							{
								designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
								if (designerHost != null)
								{
									designerTransaction = designerHost.CreateTransaction(SR.GetString("DragDropMoveComponents", new object[] { this.components.Length }));
								}
							}
							if (designerHost != null)
							{
								designerHost.DestroyComponent(component);
							}
						}
						this.components = null;
					}
					ICollection collection = designerSerializationService.Deserialize(this.serializationData);
					this.components = new IComponent[collection.Count];
					IEnumerator enumerator = collection.GetEnumerator();
					int num = 0;
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						this.components[num++] = (IComponent)obj;
					}
					ArrayList arrayList = new ArrayList();
					for (int j = 0; j < this.components.Length; j++)
					{
						if (this.components[j] is Control)
						{
							Control control = (Control)this.components[j];
							if (control.Parent == null)
							{
								arrayList.Add(this.components[j]);
							}
						}
						else
						{
							arrayList.Add(this.components[j]);
						}
					}
					this.components = arrayList.ToArray();
				}
				finally
				{
					if (designerTransaction != null)
					{
						designerTransaction.Commit();
					}
				}
			}

			public void SetData(string format, bool autoConvert, object data)
			{
				this.SetData(format, data);
			}

			public void SetData(string format, object data)
			{
				throw new Exception(SR.GetString("DragDropSetDataError"));
			}

			public void SetData(Type format, object data)
			{
				this.SetData(format.FullName, data);
			}

			public void SetData(object data)
			{
				this.SetData(data.GetType(), data);
			}

			private IServiceProvider serviceProvider;

			private object[] components;

			private Stream serializationStream;

			private object serializationData;

			private int initialX;

			private int initialY;

			private IOleDragClient dragClient;

			private OleDragDropHandler.CfCodeToolboxItem toolboxitemdata;
		}

		[Serializable]
		internal class CfCodeToolboxItem : ToolboxItem
		{
			public CfCodeToolboxItem(object serializationData)
			{
				this.serializationData = serializationData;
			}

			private CfCodeToolboxItem(SerializationInfo info, StreamingContext context)
			{
				this.Deserialize(info, context);
			}

			public void SetDisplayName()
			{
				if (!this.displaynameset)
				{
					this.displaynameset = true;
					string text = "Template";
					int num = ++OleDragDropHandler.CfCodeToolboxItem.template;
					base.DisplayName = text + num.ToString(CultureInfo.CurrentCulture);
				}
			}

			protected override void Serialize(SerializationInfo info, StreamingContext context)
			{
				base.Serialize(info, context);
				if (this.serializationData != null)
				{
					info.AddValue("CfCodeToolboxItem.serializationData", this.serializationData);
				}
			}

			protected override void Deserialize(SerializationInfo info, StreamingContext context)
			{
				base.Deserialize(info, context);
				foreach (SerializationEntry serializationEntry in info)
				{
					if (serializationEntry.Name == "CfCodeToolboxItem.serializationData")
					{
						this.serializationData = serializationEntry.Value;
						return;
					}
				}
			}

			protected override IComponent[] CreateComponentsCore(IDesignerHost host, IDictionary defaultValues)
			{
				IDesignerSerializationService designerSerializationService = (IDesignerSerializationService)host.GetService(typeof(IDesignerSerializationService));
				if (designerSerializationService == null)
				{
					return null;
				}
				ICollection collection = designerSerializationService.Deserialize(this.serializationData);
				ArrayList arrayList = new ArrayList();
				foreach (object obj in collection)
				{
					if (obj != null && obj is IComponent)
					{
						arrayList.Add(obj);
					}
				}
				IComponent[] array = new IComponent[arrayList.Count];
				arrayList.CopyTo(array, 0);
				ArrayList arrayList2 = null;
				if (defaultValues == null)
				{
					defaultValues = new Hashtable();
				}
				Control control = defaultValues["Parent"] as Control;
				if (control != null)
				{
					ParentControlDesigner parentControlDesigner = host.GetDesigner(control) as ParentControlDesigner;
					if (parentControlDesigner != null)
					{
						Rectangle rectangle = Rectangle.Empty;
						foreach (IComponent component in array)
						{
							Control control2 = component as Control;
							if (control2 != null && control2 != control && control2.Parent == null)
							{
								if (rectangle.IsEmpty)
								{
									rectangle = control2.Bounds;
								}
								else
								{
									rectangle = Rectangle.Union(rectangle, control2.Bounds);
								}
							}
						}
						defaultValues.Remove("Size");
						foreach (IComponent component2 in array)
						{
							Control control3 = component2 as Control;
							Form form = control3 as Form;
							if (control3 != null && (form == null || !form.TopLevel) && control3.Parent == null)
							{
								defaultValues["Offset"] = new Size(control3.Bounds.X - rectangle.X, control3.Bounds.Y - rectangle.Y);
								parentControlDesigner.AddControl(control3, defaultValues);
							}
						}
					}
				}
				ComponentTray componentTray = (ComponentTray)host.GetService(typeof(ComponentTray));
				if (componentTray != null)
				{
					foreach (IComponent component3 in array)
					{
						ComponentTray.TrayControl trayControlFromComponent = componentTray.GetTrayControlFromComponent(component3);
						if (trayControlFromComponent != null)
						{
							if (arrayList2 == null)
							{
								arrayList2 = new ArrayList();
							}
							arrayList2.Add(trayControlFromComponent);
						}
					}
					if (arrayList2 != null)
					{
						componentTray.UpdatePastePositions(arrayList2);
					}
				}
				return array;
			}

			protected override IComponent[] CreateComponentsCore(IDesignerHost host)
			{
				return this.CreateComponentsCore(host, null);
			}

			private object serializationData;

			private static int template;

			private bool displaynameset;
		}
	}
}
