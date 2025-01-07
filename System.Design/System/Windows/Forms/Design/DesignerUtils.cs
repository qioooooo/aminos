using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal static class DesignerUtils
	{
		public static Image BoxImage
		{
			get
			{
				if (DesignerUtils.boxImage == null)
				{
					DesignerUtils.boxImage = new Bitmap(DesignerUtils.BOXIMAGESIZE, DesignerUtils.BOXIMAGESIZE, PixelFormat.Format32bppPArgb);
					using (Graphics graphics = Graphics.FromImage(DesignerUtils.boxImage))
					{
						graphics.FillRectangle(new SolidBrush(SystemColors.InactiveBorder), 0, 0, DesignerUtils.BOXIMAGESIZE, DesignerUtils.BOXIMAGESIZE);
						graphics.DrawRectangle(new Pen(SystemColors.ControlDarkDark), 0, 0, DesignerUtils.BOXIMAGESIZE - 1, DesignerUtils.BOXIMAGESIZE - 1);
					}
				}
				return DesignerUtils.boxImage;
			}
		}

		public static Brush HoverBrush
		{
			get
			{
				return DesignerUtils.hoverBrush;
			}
		}

		public static Size MinDragSize
		{
			get
			{
				if (DesignerUtils.minDragSize == Size.Empty)
				{
					Size dragSize = SystemInformation.DragSize;
					Size doubleClickSize = SystemInformation.DoubleClickSize;
					DesignerUtils.minDragSize.Width = Math.Max(dragSize.Width, doubleClickSize.Width);
					DesignerUtils.minDragSize.Height = Math.Max(dragSize.Height, doubleClickSize.Height);
				}
				return DesignerUtils.minDragSize;
			}
		}

		public static Point LastCursorPoint
		{
			get
			{
				int messagePos = SafeNativeMethods.GetMessagePos();
				return new Point(NativeMethods.Util.SignedLOWORD(messagePos), NativeMethods.Util.SignedHIWORD(messagePos));
			}
		}

		public static void SyncBrushes()
		{
			DesignerUtils.hoverBrush.Dispose();
			DesignerUtils.hoverBrush = new SolidBrush(Color.FromArgb(50, SystemColors.Highlight));
			DesignerUtils.selectionBorderBrush.Dispose();
			DesignerUtils.selectionBorderBrush = new HatchBrush(HatchStyle.Percent50, SystemColors.ControlDarkDark, Color.Transparent);
			SafeNativeMethods.DeleteObject(new HandleRef(null, DesignerUtils.grabHandleFillBrushPrimary));
			DesignerUtils.grabHandleFillBrushPrimary = SafeNativeMethods.CreateSolidBrush(ColorTranslator.ToWin32(SystemColors.Window));
			SafeNativeMethods.DeleteObject(new HandleRef(null, DesignerUtils.grabHandleFillBrush));
			DesignerUtils.grabHandleFillBrush = SafeNativeMethods.CreateSolidBrush(ColorTranslator.ToWin32(SystemColors.ControlText));
			SafeNativeMethods.DeleteObject(new HandleRef(null, DesignerUtils.grabHandlePenPrimary));
			DesignerUtils.grabHandlePenPrimary = SafeNativeMethods.CreatePen(NativeMethods.PS_SOLID, 1, ColorTranslator.ToWin32(SystemColors.ControlText));
			SafeNativeMethods.DeleteObject(new HandleRef(null, DesignerUtils.grabHandlePen));
			DesignerUtils.grabHandlePen = SafeNativeMethods.CreatePen(NativeMethods.PS_SOLID, 1, ColorTranslator.ToWin32(SystemColors.Window));
		}

		private static void DrawDragBorder(Graphics g, Size imageSize, int borderSize, Color backColor)
		{
			Pen pen = SystemPens.ControlDarkDark;
			if (backColor != Color.Empty && (double)backColor.GetBrightness() < 0.5)
			{
				pen = SystemPens.ControlLight;
			}
			g.DrawLine(pen, 1, 0, imageSize.Width - 2, 0);
			g.DrawLine(pen, 1, imageSize.Height - 1, imageSize.Width - 2, imageSize.Height - 1);
			g.DrawLine(pen, 0, 1, 0, imageSize.Height - 2);
			g.DrawLine(pen, imageSize.Width - 1, 1, imageSize.Width - 1, imageSize.Height - 2);
			for (int i = 1; i < borderSize; i++)
			{
				g.DrawRectangle(pen, i, i, imageSize.Width - (2 + i), imageSize.Height - (2 + i));
			}
		}

		public static void DrawResizeBorder(Graphics g, Region resizeBorder, Color backColor)
		{
			Brush brush = SystemBrushes.ControlDarkDark;
			if (backColor != Color.Empty && (double)backColor.GetBrightness() < 0.5)
			{
				brush = SystemBrushes.ControlLight;
			}
			g.FillRegion(brush, resizeBorder);
		}

		public static void DrawFrame(Graphics g, Region resizeBorder, FrameStyle style, Color backColor)
		{
			Color color = SystemColors.ControlDarkDark;
			if (backColor != Color.Empty && (double)backColor.GetBrightness() < 0.5)
			{
				color = SystemColors.ControlLight;
			}
			Brush brush;
			switch (style)
			{
			case FrameStyle.Dashed:
				brush = new HatchBrush(HatchStyle.Percent50, color, Color.Transparent);
				goto IL_0055;
			}
			brush = new SolidBrush(color);
			IL_0055:
			g.FillRegion(brush, resizeBorder);
		}

		public static void DrawGrabHandle(Graphics graphics, Rectangle bounds, bool isPrimary, Glyph glyph)
		{
			IntPtr hdc = graphics.GetHdc();
			try
			{
				IntPtr intPtr = SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, isPrimary ? DesignerUtils.grabHandleFillBrushPrimary : DesignerUtils.grabHandleFillBrush));
				IntPtr intPtr2 = SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, isPrimary ? DesignerUtils.grabHandlePenPrimary : DesignerUtils.grabHandlePen));
				SafeNativeMethods.RoundRect(new HandleRef(glyph, hdc), bounds.Left, bounds.Top, bounds.Right, bounds.Bottom, 2, 2);
				SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, intPtr));
				SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, intPtr2));
			}
			finally
			{
				graphics.ReleaseHdcInternal(hdc);
			}
		}

		public static void DrawNoResizeHandle(Graphics graphics, Rectangle bounds, bool isPrimary, Glyph glyph)
		{
			IntPtr hdc = graphics.GetHdc();
			try
			{
				IntPtr intPtr = SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, isPrimary ? DesignerUtils.grabHandleFillBrushPrimary : DesignerUtils.grabHandleFillBrush));
				IntPtr intPtr2 = SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, DesignerUtils.grabHandlePenPrimary));
				SafeNativeMethods.Rectangle(new HandleRef(glyph, hdc), bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
				SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, intPtr));
				SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, intPtr2));
			}
			finally
			{
				graphics.ReleaseHdcInternal(hdc);
			}
		}

		public static void DrawLockedHandle(Graphics graphics, Rectangle bounds, bool isPrimary, Glyph glyph)
		{
			IntPtr hdc = graphics.GetHdc();
			try
			{
				IntPtr intPtr = SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, DesignerUtils.grabHandlePenPrimary));
				IntPtr intPtr2 = SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, DesignerUtils.grabHandleFillBrushPrimary));
				SafeNativeMethods.RoundRect(new HandleRef(glyph, hdc), bounds.Left + DesignerUtils.LOCKHANDLEUPPER_OFFSET, bounds.Top, bounds.Left + DesignerUtils.LOCKHANDLEUPPER_OFFSET + DesignerUtils.LOCKHANDLESIZE_UPPER, bounds.Top + DesignerUtils.LOCKHANDLESIZE_UPPER, 2, 2);
				SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, isPrimary ? DesignerUtils.grabHandleFillBrushPrimary : DesignerUtils.grabHandleFillBrush));
				SafeNativeMethods.Rectangle(new HandleRef(glyph, hdc), bounds.Left, bounds.Top + DesignerUtils.LOCKHANDLELOWER_OFFSET, bounds.Right, bounds.Bottom);
				SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, intPtr2));
				SafeNativeMethods.SelectObject(new HandleRef(glyph, hdc), new HandleRef(glyph, intPtr));
			}
			finally
			{
				graphics.ReleaseHdcInternal(hdc);
			}
		}

		public static void DrawSelectionBorder(Graphics graphics, Rectangle bounds)
		{
			graphics.FillRectangle(DesignerUtils.selectionBorderBrush, bounds);
		}

		public static void GenerateSnapShot(Control control, ref Image image, int borderSize, double opacity, Color backColor)
		{
			if (!DesignerUtils.GenerateSnapShotWithWM_PRINT(control, ref image))
			{
				DesignerUtils.GenerateSnapShotWithBitBlt(control, ref image);
			}
			if (opacity < 1.0 && opacity > 0.0)
			{
				DesignerUtils.SetImageAlpha((Bitmap)image, opacity);
			}
			if (borderSize > 0)
			{
				using (Graphics graphics = Graphics.FromImage(image))
				{
					DesignerUtils.DrawDragBorder(graphics, image.Size, borderSize, backColor);
				}
			}
		}

		public static Size GetAdornmentDimensions(AdornmentType adornmentType)
		{
			switch (adornmentType)
			{
			case AdornmentType.GrabHandle:
				return new Size(DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE);
			case AdornmentType.ContainerSelector:
			case AdornmentType.Maximum:
				return new Size(DesignerUtils.CONTAINERGRABHANDLESIZE, DesignerUtils.CONTAINERGRABHANDLESIZE);
			default:
				return new Size(0, 0);
			}
		}

		public static bool UseSnapLines(IServiceProvider provider)
		{
			bool flag = true;
			object obj = null;
			DesignerOptionService designerOptionService = provider.GetService(typeof(DesignerOptionService)) as DesignerOptionService;
			if (designerOptionService != null)
			{
				PropertyDescriptor propertyDescriptor = designerOptionService.Options.Properties["UseSnapLines"];
				if (propertyDescriptor != null)
				{
					obj = propertyDescriptor.GetValue(null);
				}
			}
			if (obj != null && obj is bool)
			{
				flag = (bool)obj;
			}
			return flag;
		}

		public static object GetOptionValue(IServiceProvider provider, string name)
		{
			object obj = null;
			if (provider != null)
			{
				DesignerOptionService designerOptionService = provider.GetService(typeof(DesignerOptionService)) as DesignerOptionService;
				if (designerOptionService != null)
				{
					PropertyDescriptor propertyDescriptor = designerOptionService.Options.Properties[name];
					if (propertyDescriptor != null)
					{
						obj = propertyDescriptor.GetValue(null);
					}
				}
				else
				{
					IDesignerOptionService designerOptionService2 = provider.GetService(typeof(IDesignerOptionService)) as IDesignerOptionService;
					if (designerOptionService2 != null)
					{
						obj = designerOptionService2.GetOptionValue("WindowsFormsDesigner\\General", name);
					}
				}
			}
			return obj;
		}

		public static void GenerateSnapShotWithBitBlt(Control control, ref Image image)
		{
			HandleRef handleRef = new HandleRef(control, control.Handle);
			IntPtr dc = UnsafeNativeMethods.GetDC(handleRef);
			image = new Bitmap(Math.Max(control.Width, DesignerUtils.MINCONTROLBITMAPSIZE), Math.Max(control.Height, DesignerUtils.MINCONTROLBITMAPSIZE), PixelFormat.Format32bppPArgb);
			using (Graphics graphics = Graphics.FromImage(image))
			{
				if (control.BackColor == Color.Transparent)
				{
					graphics.Clear(SystemColors.Control);
				}
				IntPtr hdc = graphics.GetHdc();
				SafeNativeMethods.BitBlt(hdc, 0, 0, image.Width, image.Height, dc, 0, 0, 13369376);
				graphics.ReleaseHdc(hdc);
			}
		}

		public static bool GenerateSnapShotWithWM_PRINT(Control control, ref Image image)
		{
			IntPtr handle = control.Handle;
			image = new Bitmap(Math.Max(control.Width, DesignerUtils.MINCONTROLBITMAPSIZE), Math.Max(control.Height, DesignerUtils.MINCONTROLBITMAPSIZE), PixelFormat.Format32bppPArgb);
			if (control.BackColor == Color.Transparent)
			{
				using (Graphics graphics = Graphics.FromImage(image))
				{
					graphics.Clear(SystemColors.Control);
				}
			}
			Color color = Color.FromArgb(255, 252, 186, 238);
			((Bitmap)image).SetPixel(image.Width / 2, image.Height / 2, color);
			using (Graphics graphics2 = Graphics.FromImage(image))
			{
				IntPtr hdc = graphics2.GetHdc();
				NativeMethods.SendMessage(handle, 791, hdc, (IntPtr)30);
				graphics2.ReleaseHdc(hdc);
			}
			return !((Bitmap)image).GetPixel(image.Width / 2, image.Height / 2).Equals(color);
		}

		public static Rectangle GetBoundsForSelectionType(Rectangle originalBounds, SelectionBorderGlyphType type, int borderSize)
		{
			Rectangle rectangle = Rectangle.Empty;
			switch (type)
			{
			case SelectionBorderGlyphType.Top:
				rectangle = new Rectangle(originalBounds.Left - borderSize, originalBounds.Top - borderSize, originalBounds.Width + 2 * borderSize, borderSize);
				break;
			case SelectionBorderGlyphType.Bottom:
				rectangle = new Rectangle(originalBounds.Left - borderSize, originalBounds.Bottom, originalBounds.Width + 2 * borderSize, borderSize);
				break;
			case SelectionBorderGlyphType.Left:
				rectangle = new Rectangle(originalBounds.Left - borderSize, originalBounds.Top - borderSize, borderSize, originalBounds.Height + 2 * borderSize);
				break;
			case SelectionBorderGlyphType.Right:
				rectangle = new Rectangle(originalBounds.Right, originalBounds.Top - borderSize, borderSize, originalBounds.Height + 2 * borderSize);
				break;
			case SelectionBorderGlyphType.Body:
				rectangle = originalBounds;
				break;
			}
			return rectangle;
		}

		private static Rectangle GetBoundsForSelectionType(Rectangle originalBounds, SelectionBorderGlyphType type, int bordersize, int offset)
		{
			Rectangle rectangle = DesignerUtils.GetBoundsForSelectionType(originalBounds, type, bordersize);
			if (offset != 0)
			{
				switch (type)
				{
				case SelectionBorderGlyphType.Top:
					rectangle.Offset(-offset, -offset);
					rectangle.Width += 2 * offset;
					break;
				case SelectionBorderGlyphType.Bottom:
					rectangle.Offset(-offset, offset);
					rectangle.Width += 2 * offset;
					break;
				case SelectionBorderGlyphType.Left:
					rectangle.Offset(-offset, -offset);
					rectangle.Height += 2 * offset;
					break;
				case SelectionBorderGlyphType.Right:
					rectangle.Offset(offset, -offset);
					rectangle.Height += 2 * offset;
					break;
				case SelectionBorderGlyphType.Body:
					rectangle = originalBounds;
					break;
				}
			}
			return rectangle;
		}

		public static Rectangle GetBoundsForSelectionType(Rectangle originalBounds, SelectionBorderGlyphType type)
		{
			return DesignerUtils.GetBoundsForSelectionType(originalBounds, type, DesignerUtils.SELECTIONBORDERSIZE, DesignerUtils.SELECTIONBORDEROFFSET);
		}

		public static Rectangle GetBoundsForNoResizeSelectionType(Rectangle originalBounds, SelectionBorderGlyphType type)
		{
			return DesignerUtils.GetBoundsForSelectionType(originalBounds, type, DesignerUtils.SELECTIONBORDERSIZE, DesignerUtils.NORESIZEBORDEROFFSET);
		}

		public static int GetTextBaseline(Control ctrl, ContentAlignment alignment)
		{
			Rectangle clientRectangle = ctrl.ClientRectangle;
			int num = 0;
			int num2 = 0;
			using (Graphics graphics = ctrl.CreateGraphics())
			{
				IntPtr hdc = graphics.GetHdc();
				IntPtr intPtr = ctrl.Font.ToHfont();
				try
				{
					IntPtr intPtr2 = SafeNativeMethods.SelectObject(new HandleRef(ctrl, hdc), new HandleRef(ctrl, intPtr));
					NativeMethods.TEXTMETRIC textmetric = new NativeMethods.TEXTMETRIC();
					SafeNativeMethods.GetTextMetrics(new HandleRef(ctrl, hdc), textmetric);
					num = textmetric.tmAscent + 1;
					num2 = textmetric.tmHeight;
					SafeNativeMethods.SelectObject(new HandleRef(ctrl, hdc), new HandleRef(ctrl, intPtr2));
				}
				finally
				{
					SafeNativeMethods.DeleteObject(new HandleRef(ctrl.Font, intPtr));
					graphics.ReleaseHdc(hdc);
				}
			}
			if ((alignment & DesignerUtils.anyTopAlignment) != (ContentAlignment)0)
			{
				return clientRectangle.Top + num;
			}
			if ((alignment & DesignerUtils.anyMiddleAlignment) != (ContentAlignment)0)
			{
				return clientRectangle.Top + clientRectangle.Height / 2 - num2 / 2 + num;
			}
			return clientRectangle.Bottom - num2 + num;
		}

		public static Rectangle GetBoundsFromToolboxSnapDragDropInfo(ToolboxSnapDragDropEventArgs e, Rectangle originalBounds, bool isMirrored)
		{
			Rectangle rectangle = originalBounds;
			if (e.Offset != Point.Empty)
			{
				if ((e.SnapDirections & ToolboxSnapDragDropEventArgs.SnapDirection.Top) != ToolboxSnapDragDropEventArgs.SnapDirection.None)
				{
					rectangle.Y += e.Offset.Y;
				}
				else if ((e.SnapDirections & ToolboxSnapDragDropEventArgs.SnapDirection.Bottom) != ToolboxSnapDragDropEventArgs.SnapDirection.None)
				{
					rectangle.Y = originalBounds.Y - originalBounds.Height + e.Offset.Y;
				}
				if (!isMirrored)
				{
					if ((e.SnapDirections & ToolboxSnapDragDropEventArgs.SnapDirection.Left) != ToolboxSnapDragDropEventArgs.SnapDirection.None)
					{
						rectangle.X += e.Offset.X;
					}
					else if ((e.SnapDirections & ToolboxSnapDragDropEventArgs.SnapDirection.Right) != ToolboxSnapDragDropEventArgs.SnapDirection.None)
					{
						rectangle.X = originalBounds.X - originalBounds.Width + e.Offset.X;
					}
				}
				else if ((e.SnapDirections & ToolboxSnapDragDropEventArgs.SnapDirection.Left) != ToolboxSnapDragDropEventArgs.SnapDirection.None)
				{
					rectangle.X = originalBounds.X - originalBounds.Width - e.Offset.X;
				}
				else if ((e.SnapDirections & ToolboxSnapDragDropEventArgs.SnapDirection.Right) != ToolboxSnapDragDropEventArgs.SnapDirection.None)
				{
					rectangle.X -= e.Offset.X;
				}
			}
			return rectangle;
		}

		public static string GetUniqueSiteName(IDesignerHost host, string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			INameCreationService nameCreationService = (INameCreationService)host.GetService(typeof(INameCreationService));
			if (nameCreationService == null)
			{
				return null;
			}
			if (host.Container.Components[name] != null)
			{
				string text = name;
				int num = 1;
				while (!nameCreationService.IsValidName(text))
				{
					text = name + num.ToString(CultureInfo.InvariantCulture);
					num++;
				}
				return text;
			}
			if (!nameCreationService.IsValidName(name))
			{
				return null;
			}
			return name;
		}

		private unsafe static void SetImageAlpha(Bitmap b, double opacity)
		{
			if (opacity == 1.0)
			{
				return;
			}
			byte[] array = new byte[256];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (byte)((double)i * opacity);
			}
			BitmapData bitmapData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			try
			{
				int num = bitmapData.Height * bitmapData.Width;
				int* ptr = (int*)(void*)bitmapData.Scan0;
				byte* ptr2 = (byte*)(ptr + num);
				for (byte* ptr3 = (byte*)ptr + 3; ptr3 < ptr2; ptr3 += 4)
				{
					*ptr3 = array[(int)(*ptr3)];
				}
			}
			finally
			{
				b.UnlockBits(bitmapData);
			}
		}

		public static ICollection FilterGenericTypes(ICollection types)
		{
			if (types == null || types.Count == 0)
			{
				return types;
			}
			ArrayList arrayList = new ArrayList(types.Count);
			foreach (object obj in types)
			{
				Type type = (Type)obj;
				if (!type.ContainsGenericParameters)
				{
					arrayList.Add(type);
				}
			}
			return arrayList;
		}

		public static IContainer CheckForNestedContainer(IContainer container)
		{
			NestedContainer nestedContainer = container as NestedContainer;
			if (nestedContainer != null)
			{
				return nestedContainer.Owner.Site.Container;
			}
			return container;
		}

		public static ICollection CopyDragObjects(ICollection objects, IServiceProvider svcProvider)
		{
			if (objects == null || svcProvider == null)
			{
				return null;
			}
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				ComponentSerializationService componentSerializationService = svcProvider.GetService(typeof(ComponentSerializationService)) as ComponentSerializationService;
				IDesignerHost designerHost = svcProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (componentSerializationService != null && designerHost != null)
				{
					SerializationStore serializationStore = null;
					serializationStore = componentSerializationService.CreateStore();
					ICollection collection = DesignerUtils.GetCopySelection(objects, designerHost);
					foreach (object obj in collection)
					{
						IComponent component = (IComponent)obj;
						componentSerializationService.Serialize(serializationStore, component);
					}
					serializationStore.Close();
					collection = componentSerializationService.Deserialize(serializationStore);
					ArrayList arrayList = new ArrayList(objects.Count);
					foreach (object obj2 in collection)
					{
						IComponent component2 = (IComponent)obj2;
						Control control = component2 as Control;
						if (control != null && control.Parent == null)
						{
							arrayList.Add(component2);
						}
						else if (control == null)
						{
							ToolStripItem toolStripItem = component2 as ToolStripItem;
							if (toolStripItem != null && toolStripItem.GetCurrentParent() == null)
							{
								arrayList.Add(component2);
							}
						}
					}
					return arrayList;
				}
			}
			finally
			{
				Cursor.Current = cursor;
			}
			return null;
		}

		private static ICollection GetCopySelection(ICollection objects, IDesignerHost host)
		{
			if (objects == null || host == null)
			{
				return null;
			}
			ArrayList arrayList = new ArrayList();
			foreach (object obj in objects)
			{
				IComponent component = (IComponent)obj;
				arrayList.Add(component);
				DesignerUtils.GetAssociatedComponents(component, host, arrayList);
			}
			return arrayList;
		}

		internal static void GetAssociatedComponents(IComponent component, IDesignerHost host, ArrayList list)
		{
			if (host == null)
			{
				return;
			}
			ComponentDesigner componentDesigner = host.GetDesigner(component) as ComponentDesigner;
			if (componentDesigner == null)
			{
				return;
			}
			foreach (object obj in componentDesigner.AssociatedComponents)
			{
				IComponent component2 = (IComponent)obj;
				if (component2.Site != null)
				{
					list.Add(component2);
					DesignerUtils.GetAssociatedComponents(component2, host, list);
				}
			}
		}

		private static Size minDragSize = Size.Empty;

		private static SolidBrush hoverBrush = new SolidBrush(Color.FromArgb(50, SystemColors.Highlight));

		private static HatchBrush selectionBorderBrush = new HatchBrush(HatchStyle.Percent50, SystemColors.ControlDarkDark, Color.Transparent);

		private static IntPtr grabHandleFillBrushPrimary = SafeNativeMethods.CreateSolidBrush(ColorTranslator.ToWin32(SystemColors.Window));

		private static IntPtr grabHandleFillBrush = SafeNativeMethods.CreateSolidBrush(ColorTranslator.ToWin32(SystemColors.ControlText));

		private static IntPtr grabHandlePenPrimary = SafeNativeMethods.CreatePen(NativeMethods.PS_SOLID, 1, ColorTranslator.ToWin32(SystemColors.ControlText));

		private static IntPtr grabHandlePen = SafeNativeMethods.CreatePen(NativeMethods.PS_SOLID, 1, ColorTranslator.ToWin32(SystemColors.Window));

		private static Bitmap boxImage = null;

		public static int BOXIMAGESIZE = 16;

		public static int SELECTIONBORDERSIZE = 1;

		public static int SELECTIONBORDERHITAREA = 3;

		public static int HANDLESIZE = 7;

		public static int HANDLEOVERLAP = 2;

		public static int SELECTIONBORDEROFFSET = (DesignerUtils.HANDLESIZE - DesignerUtils.SELECTIONBORDERSIZE) / 2 - DesignerUtils.HANDLEOVERLAP;

		public static int NORESIZEHANDLESIZE = 5;

		public static int NORESIZEBORDEROFFSET = (DesignerUtils.NORESIZEHANDLESIZE - DesignerUtils.SELECTIONBORDERSIZE) / 2;

		public static int LOCKHANDLEHEIGHT = 9;

		public static int LOCKHANDLEWIDTH = 7;

		public static int LOCKHANDLEOVERLAP = 2;

		public static int LOCKEDSELECTIONBORDEROFFSET_Y = (DesignerUtils.LOCKHANDLEHEIGHT - DesignerUtils.SELECTIONBORDERSIZE) / 2 - DesignerUtils.LOCKHANDLEOVERLAP;

		public static int LOCKEDSELECTIONBORDEROFFSET_X = (DesignerUtils.LOCKHANDLEWIDTH - DesignerUtils.SELECTIONBORDERSIZE) / 2 - DesignerUtils.LOCKHANDLEOVERLAP;

		public static int LOCKHANDLESIZE_UPPER = 5;

		public static int LOCKHANDLEHEIGHT_LOWER = 6;

		public static int LOCKHANDLEWIDTH_LOWER = 7;

		public static int LOCKHANDLEUPPER_OFFSET = (DesignerUtils.LOCKHANDLEWIDTH_LOWER - DesignerUtils.LOCKHANDLESIZE_UPPER) / 2;

		public static int LOCKHANDLELOWER_OFFSET = DesignerUtils.LOCKHANDLEHEIGHT - DesignerUtils.LOCKHANDLEHEIGHT_LOWER;

		public static int CONTAINERGRABHANDLESIZE = 15;

		public static int SNAPELINEDELAY = 1000;

		public static int MINIMUMSTYLESIZE = 20;

		public static int MINIMUMSTYLEPERCENT = 50;

		public static int MINCONTROLBITMAPSIZE = 1;

		public static int MINUMUMSTYLESIZEDRAG = 8;

		public static int DEFAULTROWCOUNT = 2;

		public static int DEFAULTCOLUMNCOUNT = 2;

		public static int RESIZEGLYPHSIZE = 4;

		public static int DEFAULTFORMPADDING = 9;

		public static readonly ContentAlignment anyTopAlignment = (ContentAlignment)7;

		public static readonly ContentAlignment anyMiddleAlignment = (ContentAlignment)112;
	}
}
