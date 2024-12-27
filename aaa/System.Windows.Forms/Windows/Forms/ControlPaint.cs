using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Internal;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x020002B4 RID: 692
	public sealed class ControlPaint
	{
		// Token: 0x060025E5 RID: 9701 RVA: 0x00058E79 File Offset: 0x00057E79
		private ControlPaint()
		{
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x00058E84 File Offset: 0x00057E84
		internal static Rectangle CalculateBackgroundImageRectangle(Rectangle bounds, Image backgroundImage, ImageLayout imageLayout)
		{
			Rectangle rectangle = bounds;
			if (backgroundImage != null)
			{
				switch (imageLayout)
				{
				case ImageLayout.None:
					rectangle.Size = backgroundImage.Size;
					break;
				case ImageLayout.Center:
				{
					rectangle.Size = backgroundImage.Size;
					Size size = bounds.Size;
					if (size.Width > rectangle.Width)
					{
						rectangle.X = (size.Width - rectangle.Width) / 2;
					}
					if (size.Height > rectangle.Height)
					{
						rectangle.Y = (size.Height - rectangle.Height) / 2;
					}
					break;
				}
				case ImageLayout.Stretch:
					rectangle.Size = bounds.Size;
					break;
				case ImageLayout.Zoom:
				{
					Size size2 = backgroundImage.Size;
					float num = (float)bounds.Width / (float)size2.Width;
					float num2 = (float)bounds.Height / (float)size2.Height;
					if (num < num2)
					{
						rectangle.Width = bounds.Width;
						rectangle.Height = (int)((double)((float)size2.Height * num) + 0.5);
						if (bounds.Y >= 0)
						{
							rectangle.Y = (bounds.Height - rectangle.Height) / 2;
						}
					}
					else
					{
						rectangle.Height = bounds.Height;
						rectangle.Width = (int)((double)((float)size2.Width * num2) + 0.5);
						if (bounds.X >= 0)
						{
							rectangle.X = (bounds.Width - rectangle.Width) / 2;
						}
					}
					break;
				}
				}
			}
			return rectangle;
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060025E7 RID: 9703 RVA: 0x0005901D File Offset: 0x0005801D
		public static Color ContrastControlDark
		{
			get
			{
				if (!SystemInformation.HighContrast)
				{
					return SystemColors.ControlDark;
				}
				return SystemColors.WindowFrame;
			}
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x00059034 File Offset: 0x00058034
		private static IntPtr CreateBitmapInfo(Bitmap bitmap, IntPtr hdcS)
		{
			NativeMethods.BITMAPINFOHEADER bitmapinfoheader = new NativeMethods.BITMAPINFOHEADER();
			bitmapinfoheader.biSize = Marshal.SizeOf(bitmapinfoheader);
			bitmapinfoheader.biWidth = bitmap.Width;
			bitmapinfoheader.biHeight = bitmap.Height;
			bitmapinfoheader.biPlanes = 1;
			bitmapinfoheader.biBitCount = 16;
			bitmapinfoheader.biCompression = 0;
			int num = 0;
			IntPtr intPtr = SafeNativeMethods.CreateHalftonePalette(new HandleRef(null, hdcS));
			UnsafeNativeMethods.GetObject(new HandleRef(null, intPtr), 2, ref num);
			int[] array = new int[num];
			SafeNativeMethods.GetPaletteEntries(new HandleRef(null, intPtr), 0, num, array);
			int[] array2 = new int[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = array[i];
				array2[i] = (num2 & -16777216) >> 6 + (num2 & 16711680) >> 4 + (num2 & 65280) >> 2;
			}
			SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
			IntPtr intPtr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(bitmapinfoheader) + num * 4);
			Marshal.StructureToPtr(bitmapinfoheader, intPtr2, false);
			Marshal.Copy(array2, 0, (IntPtr)((long)intPtr2 + (long)Marshal.SizeOf(bitmapinfoheader)), num);
			return intPtr2;
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x00059148 File Offset: 0x00058148
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr CreateHBitmap16Bit(Bitmap bitmap, Color background)
		{
			Size size = bitmap.Size;
			IntPtr intPtr2;
			using (DeviceContext screenDC = DeviceContext.ScreenDC)
			{
				IntPtr hdc = screenDC.Hdc;
				using (DeviceContext deviceContext = DeviceContext.FromCompatibleDC(hdc))
				{
					IntPtr hdc2 = deviceContext.Hdc;
					byte[] array = new byte[bitmap.Width * bitmap.Height];
					IntPtr intPtr = ControlPaint.CreateBitmapInfo(bitmap, hdc);
					intPtr2 = SafeNativeMethods.CreateDIBSection(new HandleRef(null, hdc), new HandleRef(null, intPtr), 0, array, IntPtr.Zero, 0);
					Marshal.FreeCoTaskMem(intPtr);
					if (intPtr2 == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
					try
					{
						IntPtr intPtr3 = SafeNativeMethods.SelectObject(new HandleRef(null, hdc2), new HandleRef(null, intPtr2));
						if (intPtr3 == IntPtr.Zero)
						{
							throw new Win32Exception();
						}
						SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr3));
						using (Graphics graphics = Graphics.FromHdcInternal(hdc2))
						{
							using (Brush brush = new SolidBrush(background))
							{
								graphics.FillRectangle(brush, 0, 0, size.Width, size.Height);
							}
							graphics.DrawImage(bitmap, 0, 0, size.Width, size.Height);
						}
					}
					catch
					{
						SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr2));
						throw;
					}
				}
			}
			return intPtr2;
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x00059310 File Offset: 0x00058310
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr CreateHBitmapTransparencyMask(Bitmap bitmap)
		{
			if (bitmap == null)
			{
				throw new ArgumentNullException("bitmap");
			}
			Size size = bitmap.Size;
			int width = bitmap.Width;
			int height = bitmap.Height;
			int num = width / 8;
			if (width % 8 != 0)
			{
				num++;
			}
			if (num % 2 != 0)
			{
				num++;
			}
			byte[] array = new byte[num * height];
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			for (int i = 0; i < height; i++)
			{
				IntPtr intPtr = (IntPtr)((long)bitmapData.Scan0 + (long)(i * bitmapData.Stride));
				for (int j = 0; j < width; j++)
				{
					int num2 = Marshal.ReadInt32(intPtr, j * 4);
					if (num2 >> 24 == 0)
					{
						int num3 = num * i + j / 8;
						byte[] array2 = array;
						int num4 = num3;
						array2[num4] |= (byte)(128 >> j % 8);
					}
				}
			}
			bitmap.UnlockBits(bitmapData);
			return SafeNativeMethods.CreateBitmap(size.Width, size.Height, 1, 1, array);
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x0005941C File Offset: 0x0005841C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr CreateHBitmapColorMask(Bitmap bitmap, IntPtr monochromeMask)
		{
			Size size = bitmap.Size;
			IntPtr hbitmap = bitmap.GetHbitmap();
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			IntPtr intPtr = UnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, dc));
			IntPtr intPtr2 = UnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, dc));
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			IntPtr intPtr3 = SafeNativeMethods.SelectObject(new HandleRef(null, intPtr), new HandleRef(null, monochromeMask));
			IntPtr intPtr4 = SafeNativeMethods.SelectObject(new HandleRef(null, intPtr2), new HandleRef(null, hbitmap));
			SafeNativeMethods.SetBkColor(new HandleRef(null, intPtr2), 16777215);
			SafeNativeMethods.SetTextColor(new HandleRef(null, intPtr2), 0);
			SafeNativeMethods.BitBlt(new HandleRef(null, intPtr2), 0, 0, size.Width, size.Height, new HandleRef(null, intPtr), 0, 0, 2229030);
			SafeNativeMethods.SelectObject(new HandleRef(null, intPtr), new HandleRef(null, intPtr3));
			SafeNativeMethods.SelectObject(new HandleRef(null, intPtr2), new HandleRef(null, intPtr4));
			UnsafeNativeMethods.DeleteCompatibleDC(new HandleRef(null, intPtr));
			UnsafeNativeMethods.DeleteCompatibleDC(new HandleRef(null, intPtr2));
			return global::System.Internal.HandleCollector.Add(hbitmap, NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x0005953C File Offset: 0x0005853C
		internal static IntPtr CreateHalftoneHBRUSH()
		{
			short[] array = new short[8];
			for (int i = 0; i < 8; i++)
			{
				array[i] = (short)(21845 << (i & 1));
			}
			IntPtr intPtr = SafeNativeMethods.CreateBitmap(8, 8, 1, 1, array);
			IntPtr intPtr2 = SafeNativeMethods.CreateBrushIndirect(new NativeMethods.LOGBRUSH
			{
				lbColor = ColorTranslator.ToWin32(Color.Black),
				lbStyle = 3,
				lbHatch = intPtr
			});
			SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
			return intPtr2;
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000595B4 File Offset: 0x000585B4
		internal static void CopyPixels(IntPtr sourceHwnd, IDeviceContext targetDC, Point sourceLocation, Point destinationLocation, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
			int width = blockRegionSize.Width;
			int height = blockRegionSize.Height;
			DeviceContext deviceContext = DeviceContext.FromHwnd(sourceHwnd);
			HandleRef handleRef = new HandleRef(null, targetDC.GetHdc());
			HandleRef handleRef2 = new HandleRef(null, deviceContext.Hdc);
			try
			{
				if (!SafeNativeMethods.BitBlt(handleRef, destinationLocation.X, destinationLocation.Y, width, height, handleRef2, sourceLocation.X, sourceLocation.Y, (int)copyPixelOperation))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				targetDC.ReleaseHdc();
				deviceContext.Dispose();
			}
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x00059648 File Offset: 0x00058648
		private static DashStyle BorderStyleToDashStyle(ButtonBorderStyle borderStyle)
		{
			switch (borderStyle)
			{
			case ButtonBorderStyle.Dotted:
				return DashStyle.Dot;
			case ButtonBorderStyle.Dashed:
				return DashStyle.Dash;
			case ButtonBorderStyle.Solid:
				return DashStyle.Solid;
			default:
				return DashStyle.Solid;
			}
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x00059674 File Offset: 0x00058674
		public static Color Dark(Color baseColor, float percOfDarkDark)
		{
			return new ControlPaint.HLSColor(baseColor).Darker(percOfDarkDark);
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x00059690 File Offset: 0x00058690
		public static Color Dark(Color baseColor)
		{
			return new ControlPaint.HLSColor(baseColor).Darker(0.5f);
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000596B0 File Offset: 0x000586B0
		public static Color DarkDark(Color baseColor)
		{
			return new ControlPaint.HLSColor(baseColor).Darker(1f);
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000596D0 File Offset: 0x000586D0
		internal static bool IsDarker(Color c1, Color c2)
		{
			ControlPaint.HLSColor hlscolor = new ControlPaint.HLSColor(c1);
			ControlPaint.HLSColor hlscolor2 = new ControlPaint.HLSColor(c2);
			return hlscolor.Luminosity < hlscolor2.Luminosity;
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x00059700 File Offset: 0x00058700
		internal static void PrintBorder(Graphics graphics, Rectangle bounds, BorderStyle style, Border3DStyle b3dStyle)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			switch (style)
			{
			case BorderStyle.None:
				break;
			case BorderStyle.FixedSingle:
				ControlPaint.DrawBorder(graphics, bounds, Color.FromKnownColor(KnownColor.WindowFrame), ButtonBorderStyle.Solid);
				return;
			case BorderStyle.Fixed3D:
				ControlPaint.DrawBorder3D(graphics, bounds, b3dStyle);
				break;
			default:
				return;
			}
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x00059748 File Offset: 0x00058748
		internal static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect)
		{
			ControlPaint.DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, Point.Empty, RightToLeft.No);
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x0005975D File Offset: 0x0005875D
		internal static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset)
		{
			ControlPaint.DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, scrollOffset, RightToLeft.No);
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x00059770 File Offset: 0x00058770
		internal static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset, RightToLeft rightToLeft)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (backgroundImageLayout == ImageLayout.Tile)
			{
				using (TextureBrush textureBrush = new TextureBrush(backgroundImage, WrapMode.Tile))
				{
					if (scrollOffset != Point.Empty)
					{
						Matrix transform = textureBrush.Transform;
						transform.Translate((float)scrollOffset.X, (float)scrollOffset.Y);
						textureBrush.Transform = transform;
					}
					g.FillRectangle(textureBrush, clipRect);
					return;
				}
			}
			Rectangle rectangle = ControlPaint.CalculateBackgroundImageRectangle(bounds, backgroundImage, backgroundImageLayout);
			if (rightToLeft == RightToLeft.Yes && backgroundImageLayout == ImageLayout.None)
			{
				rectangle.X += clipRect.Width - rectangle.Width;
			}
			using (SolidBrush solidBrush = new SolidBrush(backColor))
			{
				g.FillRectangle(solidBrush, clipRect);
			}
			if (!clipRect.Contains(rectangle))
			{
				if (backgroundImageLayout == ImageLayout.Stretch || backgroundImageLayout == ImageLayout.Zoom)
				{
					rectangle.Intersect(clipRect);
					g.DrawImage(backgroundImage, rectangle);
					return;
				}
				if (backgroundImageLayout == ImageLayout.None)
				{
					rectangle.Offset(clipRect.Location);
					Rectangle rectangle2 = rectangle;
					rectangle2.Intersect(clipRect);
					Rectangle rectangle3 = new Rectangle(Point.Empty, rectangle2.Size);
					g.DrawImage(backgroundImage, rectangle2, rectangle3.X, rectangle3.Y, rectangle3.Width, rectangle3.Height, GraphicsUnit.Pixel);
					return;
				}
				Rectangle rectangle4 = rectangle;
				rectangle4.Intersect(clipRect);
				Rectangle rectangle5 = new Rectangle(new Point(rectangle4.X - rectangle.X, rectangle4.Y - rectangle.Y), rectangle4.Size);
				g.DrawImage(backgroundImage, rectangle4, rectangle5.X, rectangle5.Y, rectangle5.Width, rectangle5.Height, GraphicsUnit.Pixel);
				return;
			}
			else
			{
				ImageAttributes imageAttributes = new ImageAttributes();
				imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
				g.DrawImage(backgroundImage, rectangle, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAttributes);
				imageAttributes.Dispose();
			}
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x0005995C File Offset: 0x0005895C
		public static void DrawBorder(Graphics graphics, Rectangle bounds, Color color, ButtonBorderStyle style)
		{
			switch (style)
			{
			case ButtonBorderStyle.None:
				break;
			case ButtonBorderStyle.Dotted:
			case ButtonBorderStyle.Dashed:
			case ButtonBorderStyle.Solid:
				ControlPaint.DrawBorderSimple(graphics, bounds, color, style);
				return;
			case ButtonBorderStyle.Inset:
			case ButtonBorderStyle.Outset:
				ControlPaint.DrawBorderComplex(graphics, bounds, color, style);
				break;
			default:
				return;
			}
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000599A0 File Offset: 0x000589A0
		public static void DrawBorder(Graphics graphics, Rectangle bounds, Color leftColor, int leftWidth, ButtonBorderStyle leftStyle, Color topColor, int topWidth, ButtonBorderStyle topStyle, Color rightColor, int rightWidth, ButtonBorderStyle rightStyle, Color bottomColor, int bottomWidth, ButtonBorderStyle bottomStyle)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			int[] array = new int[topWidth];
			int[] array2 = new int[topWidth];
			int[] array3 = new int[leftWidth];
			int[] array4 = new int[leftWidth];
			int[] array5 = new int[bottomWidth];
			int[] array6 = new int[bottomWidth];
			int[] array7 = new int[rightWidth];
			int[] array8 = new int[rightWidth];
			float num = 0f;
			float num2 = 0f;
			if (leftWidth > 0)
			{
				num = (float)topWidth / (float)leftWidth;
				num2 = (float)bottomWidth / (float)leftWidth;
			}
			float num3 = 0f;
			float num4 = 0f;
			if (rightWidth > 0)
			{
				num3 = (float)topWidth / (float)rightWidth;
				num4 = (float)bottomWidth / (float)rightWidth;
			}
			ControlPaint.HLSColor hlscolor = new ControlPaint.HLSColor(topColor);
			ControlPaint.HLSColor hlscolor2 = new ControlPaint.HLSColor(leftColor);
			ControlPaint.HLSColor hlscolor3 = new ControlPaint.HLSColor(bottomColor);
			ControlPaint.HLSColor hlscolor4 = new ControlPaint.HLSColor(rightColor);
			if (topWidth > 0)
			{
				int i;
				for (i = 0; i < topWidth; i++)
				{
					int num5 = 0;
					if (num > 0f)
					{
						num5 = (int)((float)i / num);
					}
					int num6 = 0;
					if (num3 > 0f)
					{
						num6 = (int)((float)i / num3);
					}
					array[i] = bounds.X + num5;
					array2[i] = bounds.X + bounds.Width - num6 - 1;
					if (leftWidth > 0)
					{
						array3[num5] = bounds.Y + i + 1;
					}
					if (rightWidth > 0)
					{
						array7[num6] = bounds.Y + i;
					}
				}
				for (int j = i; j < leftWidth; j++)
				{
					array3[j] = bounds.Y + i + 1;
				}
				for (int k = i; k < rightWidth; k++)
				{
					array7[k] = bounds.Y + i;
				}
			}
			else
			{
				for (int l = 0; l < leftWidth; l++)
				{
					array3[l] = bounds.Y;
				}
				for (int m = 0; m < rightWidth; m++)
				{
					array7[m] = bounds.Y;
				}
			}
			if (bottomWidth > 0)
			{
				int n;
				for (n = 0; n < bottomWidth; n++)
				{
					int num7 = 0;
					if (num2 > 0f)
					{
						num7 = (int)((float)n / num2);
					}
					int num8 = 0;
					if (num4 > 0f)
					{
						num8 = (int)((float)n / num4);
					}
					array5[n] = bounds.X + num7;
					array6[n] = bounds.X + bounds.Width - num8 - 1;
					if (leftWidth > 0)
					{
						array4[num7] = bounds.Y + bounds.Height - n - 1;
					}
					if (rightWidth > 0)
					{
						array8[num8] = bounds.Y + bounds.Height - n - 1;
					}
				}
				for (int num9 = n; num9 < leftWidth; num9++)
				{
					array4[num9] = bounds.Y + bounds.Height - n - 1;
				}
				for (int num10 = n; num10 < rightWidth; num10++)
				{
					array8[num10] = bounds.Y + bounds.Height - n - 1;
				}
			}
			else
			{
				for (int num11 = 0; num11 < leftWidth; num11++)
				{
					array4[num11] = bounds.Y + bounds.Height - 1;
				}
				for (int num12 = 0; num12 < rightWidth; num12++)
				{
					array8[num12] = bounds.Y + bounds.Height - 1;
				}
			}
			switch (topStyle)
			{
			case ButtonBorderStyle.Dotted:
			{
				Pen pen = new Pen(topColor);
				pen.DashStyle = DashStyle.Dot;
				for (int num13 = 0; num13 < topWidth; num13++)
				{
					graphics.DrawLine(pen, array[num13], bounds.Y + num13, array2[num13], bounds.Y + num13);
				}
				pen.Dispose();
				break;
			}
			case ButtonBorderStyle.Dashed:
			{
				Pen pen = new Pen(topColor);
				pen.DashStyle = DashStyle.Dash;
				for (int num14 = 0; num14 < topWidth; num14++)
				{
					graphics.DrawLine(pen, array[num14], bounds.Y + num14, array2[num14], bounds.Y + num14);
				}
				pen.Dispose();
				break;
			}
			case ButtonBorderStyle.Solid:
			{
				Pen pen = new Pen(topColor);
				pen.DashStyle = DashStyle.Solid;
				for (int num15 = 0; num15 < topWidth; num15++)
				{
					graphics.DrawLine(pen, array[num15], bounds.Y + num15, array2[num15], bounds.Y + num15);
				}
				pen.Dispose();
				break;
			}
			case ButtonBorderStyle.Inset:
			{
				float num16 = ControlPaint.InfinityToOne(1f / (float)(topWidth - 1));
				for (int num17 = 0; num17 < topWidth; num17++)
				{
					Pen pen = new Pen(hlscolor.Darker(1f - (float)num17 * num16));
					pen.DashStyle = DashStyle.Solid;
					graphics.DrawLine(pen, array[num17], bounds.Y + num17, array2[num17], bounds.Y + num17);
					pen.Dispose();
				}
				break;
			}
			case ButtonBorderStyle.Outset:
			{
				float num18 = ControlPaint.InfinityToOne(1f / (float)(topWidth - 1));
				for (int num19 = 0; num19 < topWidth; num19++)
				{
					Pen pen = new Pen(hlscolor.Lighter(1f - (float)num19 * num18));
					pen.DashStyle = DashStyle.Solid;
					graphics.DrawLine(pen, array[num19], bounds.Y + num19, array2[num19], bounds.Y + num19);
					pen.Dispose();
				}
				break;
			}
			}
			switch (leftStyle)
			{
			case ButtonBorderStyle.Dotted:
			{
				Pen pen = new Pen(leftColor);
				pen.DashStyle = DashStyle.Dot;
				for (int num20 = 0; num20 < leftWidth; num20++)
				{
					graphics.DrawLine(pen, bounds.X + num20, array3[num20], bounds.X + num20, array4[num20]);
				}
				pen.Dispose();
				break;
			}
			case ButtonBorderStyle.Dashed:
			{
				Pen pen = new Pen(leftColor);
				pen.DashStyle = DashStyle.Dash;
				for (int num21 = 0; num21 < leftWidth; num21++)
				{
					graphics.DrawLine(pen, bounds.X + num21, array3[num21], bounds.X + num21, array4[num21]);
				}
				pen.Dispose();
				break;
			}
			case ButtonBorderStyle.Solid:
			{
				Pen pen = new Pen(leftColor);
				pen.DashStyle = DashStyle.Solid;
				for (int num22 = 0; num22 < leftWidth; num22++)
				{
					graphics.DrawLine(pen, bounds.X + num22, array3[num22], bounds.X + num22, array4[num22]);
				}
				pen.Dispose();
				break;
			}
			case ButtonBorderStyle.Inset:
			{
				float num23 = ControlPaint.InfinityToOne(1f / (float)(leftWidth - 1));
				for (int num24 = 0; num24 < leftWidth; num24++)
				{
					Pen pen = new Pen(hlscolor2.Darker(1f - (float)num24 * num23));
					pen.DashStyle = DashStyle.Solid;
					graphics.DrawLine(pen, bounds.X + num24, array3[num24], bounds.X + num24, array4[num24]);
					pen.Dispose();
				}
				break;
			}
			case ButtonBorderStyle.Outset:
			{
				float num25 = ControlPaint.InfinityToOne(1f / (float)(leftWidth - 1));
				for (int num26 = 0; num26 < leftWidth; num26++)
				{
					Pen pen = new Pen(hlscolor2.Lighter(1f - (float)num26 * num25));
					pen.DashStyle = DashStyle.Solid;
					graphics.DrawLine(pen, bounds.X + num26, array3[num26], bounds.X + num26, array4[num26]);
					pen.Dispose();
				}
				break;
			}
			}
			switch (bottomStyle)
			{
			case ButtonBorderStyle.Dotted:
			{
				Pen pen = new Pen(bottomColor);
				pen.DashStyle = DashStyle.Dot;
				for (int num27 = 0; num27 < bottomWidth; num27++)
				{
					graphics.DrawLine(pen, array5[num27], bounds.Y + bounds.Height - 1 - num27, array6[num27], bounds.Y + bounds.Height - 1 - num27);
				}
				pen.Dispose();
				break;
			}
			case ButtonBorderStyle.Dashed:
			{
				Pen pen = new Pen(bottomColor);
				pen.DashStyle = DashStyle.Dash;
				for (int num28 = 0; num28 < bottomWidth; num28++)
				{
					graphics.DrawLine(pen, array5[num28], bounds.Y + bounds.Height - 1 - num28, array6[num28], bounds.Y + bounds.Height - 1 - num28);
				}
				pen.Dispose();
				break;
			}
			case ButtonBorderStyle.Solid:
			{
				Pen pen = new Pen(bottomColor);
				pen.DashStyle = DashStyle.Solid;
				for (int num29 = 0; num29 < bottomWidth; num29++)
				{
					graphics.DrawLine(pen, array5[num29], bounds.Y + bounds.Height - 1 - num29, array6[num29], bounds.Y + bounds.Height - 1 - num29);
				}
				pen.Dispose();
				break;
			}
			case ButtonBorderStyle.Inset:
			{
				float num30 = ControlPaint.InfinityToOne(1f / (float)(bottomWidth - 1));
				for (int num31 = 0; num31 < bottomWidth; num31++)
				{
					Pen pen = new Pen(hlscolor3.Lighter(1f - (float)num31 * num30));
					pen.DashStyle = DashStyle.Solid;
					graphics.DrawLine(pen, array5[num31], bounds.Y + bounds.Height - 1 - num31, array6[num31], bounds.Y + bounds.Height - 1 - num31);
					pen.Dispose();
				}
				break;
			}
			case ButtonBorderStyle.Outset:
			{
				float num32 = ControlPaint.InfinityToOne(1f / (float)(bottomWidth - 1));
				for (int num33 = 0; num33 < bottomWidth; num33++)
				{
					Pen pen = new Pen(hlscolor3.Darker(1f - (float)num33 * num32));
					pen.DashStyle = DashStyle.Solid;
					graphics.DrawLine(pen, array5[num33], bounds.Y + bounds.Height - 1 - num33, array6[num33], bounds.Y + bounds.Height - 1 - num33);
					pen.Dispose();
				}
				break;
			}
			}
			switch (rightStyle)
			{
			case ButtonBorderStyle.None:
				break;
			case ButtonBorderStyle.Dotted:
			{
				Pen pen = new Pen(rightColor);
				pen.DashStyle = DashStyle.Dot;
				for (int num34 = 0; num34 < rightWidth; num34++)
				{
					graphics.DrawLine(pen, bounds.X + bounds.Width - 1 - num34, array7[num34], bounds.X + bounds.Width - 1 - num34, array8[num34]);
				}
				pen.Dispose();
				return;
			}
			case ButtonBorderStyle.Dashed:
			{
				Pen pen = new Pen(rightColor);
				pen.DashStyle = DashStyle.Dash;
				for (int num35 = 0; num35 < rightWidth; num35++)
				{
					graphics.DrawLine(pen, bounds.X + bounds.Width - 1 - num35, array7[num35], bounds.X + bounds.Width - 1 - num35, array8[num35]);
				}
				pen.Dispose();
				return;
			}
			case ButtonBorderStyle.Solid:
			{
				Pen pen = new Pen(rightColor);
				pen.DashStyle = DashStyle.Solid;
				for (int num36 = 0; num36 < rightWidth; num36++)
				{
					graphics.DrawLine(pen, bounds.X + bounds.Width - 1 - num36, array7[num36], bounds.X + bounds.Width - 1 - num36, array8[num36]);
				}
				pen.Dispose();
				return;
			}
			case ButtonBorderStyle.Inset:
			{
				float num37 = ControlPaint.InfinityToOne(1f / (float)(rightWidth - 1));
				for (int num38 = 0; num38 < rightWidth; num38++)
				{
					Pen pen = new Pen(hlscolor4.Lighter(1f - (float)num38 * num37));
					pen.DashStyle = DashStyle.Solid;
					graphics.DrawLine(pen, bounds.X + bounds.Width - 1 - num38, array7[num38], bounds.X + bounds.Width - 1 - num38, array8[num38]);
					pen.Dispose();
				}
				return;
			}
			case ButtonBorderStyle.Outset:
			{
				float num39 = ControlPaint.InfinityToOne(1f / (float)(rightWidth - 1));
				for (int num40 = 0; num40 < rightWidth; num40++)
				{
					Pen pen = new Pen(hlscolor4.Darker(1f - (float)num40 * num39));
					pen.DashStyle = DashStyle.Solid;
					graphics.DrawLine(pen, bounds.X + bounds.Width - 1 - num40, array7[num40], bounds.X + bounds.Width - 1 - num40, array8[num40]);
					pen.Dispose();
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x0005A591 File Offset: 0x00059591
		public static void DrawBorder3D(Graphics graphics, Rectangle rectangle)
		{
			ControlPaint.DrawBorder3D(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, Border3DStyle.Etched, Border3DSide.Left | Border3DSide.Top | Border3DSide.Right | Border3DSide.Bottom);
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x0005A5B8 File Offset: 0x000595B8
		public static void DrawBorder3D(Graphics graphics, Rectangle rectangle, Border3DStyle style)
		{
			ControlPaint.DrawBorder3D(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, style, Border3DSide.Left | Border3DSide.Top | Border3DSide.Right | Border3DSide.Bottom);
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x0005A5DF File Offset: 0x000595DF
		public static void DrawBorder3D(Graphics graphics, Rectangle rectangle, Border3DStyle style, Border3DSide sides)
		{
			ControlPaint.DrawBorder3D(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, style, sides);
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x0005A605 File Offset: 0x00059605
		public static void DrawBorder3D(Graphics graphics, int x, int y, int width, int height)
		{
			ControlPaint.DrawBorder3D(graphics, x, y, width, height, Border3DStyle.Etched, Border3DSide.Left | Border3DSide.Top | Border3DSide.Right | Border3DSide.Bottom);
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x0005A615 File Offset: 0x00059615
		public static void DrawBorder3D(Graphics graphics, int x, int y, int width, int height, Border3DStyle style)
		{
			ControlPaint.DrawBorder3D(graphics, x, y, width, height, style, Border3DSide.Left | Border3DSide.Top | Border3DSide.Right | Border3DSide.Bottom);
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x0005A628 File Offset: 0x00059628
		public static void DrawBorder3D(Graphics graphics, int x, int y, int width, int height, Border3DStyle style, Border3DSide sides)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			int num = (int)(style & (Border3DStyle)15);
			int num2 = (int)(sides | (Border3DSide)(style & (Border3DStyle)(-16)));
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(x, y, width, height);
			if ((num2 & 8192) == 8192)
			{
				Size border3DSize = SystemInformation.Border3DSize;
				rect.left -= border3DSize.Width;
				rect.right += border3DSize.Width;
				rect.top -= border3DSize.Height;
				rect.bottom += border3DSize.Height;
				num2 &= -8193;
			}
			using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics))
			{
				SafeNativeMethods.DrawEdge(new HandleRef(windowsGraphics, windowsGraphics.DeviceContext.Hdc), ref rect, num, num2);
			}
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x0005A710 File Offset: 0x00059710
		private static void DrawBorderComplex(Graphics graphics, Rectangle bounds, Color color, ButtonBorderStyle style)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (style == ButtonBorderStyle.Inset)
			{
				ControlPaint.HLSColor hlscolor = new ControlPaint.HLSColor(color);
				Pen pen = new Pen(hlscolor.Darker(1f));
				graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.X + bounds.Width - 1, bounds.Y);
				graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.X, bounds.Y + bounds.Height - 1);
				pen.Color = hlscolor.Lighter(1f);
				graphics.DrawLine(pen, bounds.X, bounds.Y + bounds.Height - 1, bounds.X + bounds.Width - 1, bounds.Y + bounds.Height - 1);
				graphics.DrawLine(pen, bounds.X + bounds.Width - 1, bounds.Y, bounds.X + bounds.Width - 1, bounds.Y + bounds.Height - 1);
				pen.Color = hlscolor.Lighter(0.5f);
				graphics.DrawLine(pen, bounds.X + 1, bounds.Y + 1, bounds.X + bounds.Width - 2, bounds.Y + 1);
				graphics.DrawLine(pen, bounds.X + 1, bounds.Y + 1, bounds.X + 1, bounds.Y + bounds.Height - 2);
				if (color.ToKnownColor() == SystemColors.Control.ToKnownColor())
				{
					pen.Color = SystemColors.ControlLight;
					graphics.DrawLine(pen, bounds.X + 1, bounds.Y + bounds.Height - 2, bounds.X + bounds.Width - 2, bounds.Y + bounds.Height - 2);
					graphics.DrawLine(pen, bounds.X + bounds.Width - 2, bounds.Y + 1, bounds.X + bounds.Width - 2, bounds.Y + bounds.Height - 2);
				}
				pen.Dispose();
				return;
			}
			bool flag = color.ToKnownColor() == SystemColors.Control.ToKnownColor();
			ControlPaint.HLSColor hlscolor2 = new ControlPaint.HLSColor(color);
			Pen pen2 = (flag ? SystemPens.ControlLightLight : new Pen(hlscolor2.Lighter(1f)));
			graphics.DrawLine(pen2, bounds.X, bounds.Y, bounds.X + bounds.Width - 1, bounds.Y);
			graphics.DrawLine(pen2, bounds.X, bounds.Y, bounds.X, bounds.Y + bounds.Height - 1);
			if (flag)
			{
				pen2 = SystemPens.ControlDarkDark;
			}
			else
			{
				pen2.Color = hlscolor2.Darker(1f);
			}
			graphics.DrawLine(pen2, bounds.X, bounds.Y + bounds.Height - 1, bounds.X + bounds.Width - 1, bounds.Y + bounds.Height - 1);
			graphics.DrawLine(pen2, bounds.X + bounds.Width - 1, bounds.Y, bounds.X + bounds.Width - 1, bounds.Y + bounds.Height - 1);
			if (flag)
			{
				if (SystemInformation.HighContrast)
				{
					pen2 = SystemPens.ControlLight;
				}
				else
				{
					pen2 = SystemPens.Control;
				}
			}
			else
			{
				pen2.Color = color;
			}
			graphics.DrawLine(pen2, bounds.X + 1, bounds.Y + 1, bounds.X + bounds.Width - 2, bounds.Y + 1);
			graphics.DrawLine(pen2, bounds.X + 1, bounds.Y + 1, bounds.X + 1, bounds.Y + bounds.Height - 2);
			if (flag)
			{
				pen2 = SystemPens.ControlDark;
			}
			else
			{
				pen2.Color = hlscolor2.Darker(0.5f);
			}
			graphics.DrawLine(pen2, bounds.X + 1, bounds.Y + bounds.Height - 2, bounds.X + bounds.Width - 2, bounds.Y + bounds.Height - 2);
			graphics.DrawLine(pen2, bounds.X + bounds.Width - 2, bounds.Y + 1, bounds.X + bounds.Width - 2, bounds.Y + bounds.Height - 2);
			if (!flag)
			{
				pen2.Dispose();
			}
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x0005ABE0 File Offset: 0x00059BE0
		private static void DrawBorderSimple(Graphics graphics, Rectangle bounds, Color color, ButtonBorderStyle style)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			bool flag = style == ButtonBorderStyle.Solid && color.IsSystemColor;
			Pen pen;
			if (flag)
			{
				pen = SystemPens.FromSystemColor(color);
			}
			else
			{
				pen = new Pen(color);
				if (style != ButtonBorderStyle.Solid)
				{
					pen.DashStyle = ControlPaint.BorderStyleToDashStyle(style);
				}
			}
			graphics.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
			if (!flag)
			{
				pen.Dispose();
			}
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x0005AC5D File Offset: 0x00059C5D
		public static void DrawButton(Graphics graphics, Rectangle rectangle, ButtonState state)
		{
			ControlPaint.DrawButton(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, state);
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x0005AC84 File Offset: 0x00059C84
		public static void DrawButton(Graphics graphics, int x, int y, int width, int height, ButtonState state)
		{
			ControlPaint.DrawFrameControl(graphics, x, y, width, height, 4, (int)((ButtonState)16 | state), Color.Empty, Color.Empty);
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x0005ACAC File Offset: 0x00059CAC
		public static void DrawCaptionButton(Graphics graphics, Rectangle rectangle, CaptionButton button, ButtonState state)
		{
			ControlPaint.DrawCaptionButton(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, button, state);
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x0005ACD4 File Offset: 0x00059CD4
		public static void DrawCaptionButton(Graphics graphics, int x, int y, int width, int height, CaptionButton button, ButtonState state)
		{
			ControlPaint.DrawFrameControl(graphics, x, y, width, height, 1, (int)(button | (CaptionButton)state), Color.Empty, Color.Empty);
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x0005ACFC File Offset: 0x00059CFC
		public static void DrawCheckBox(Graphics graphics, Rectangle rectangle, ButtonState state)
		{
			ControlPaint.DrawCheckBox(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, state);
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x0005AD24 File Offset: 0x00059D24
		public static void DrawCheckBox(Graphics graphics, int x, int y, int width, int height, ButtonState state)
		{
			if ((state & ButtonState.Flat) == ButtonState.Flat)
			{
				ControlPaint.DrawFlatCheckBox(graphics, new Rectangle(x, y, width, height), state);
				return;
			}
			ControlPaint.DrawFrameControl(graphics, x, y, width, height, 4, (int)state, Color.Empty, Color.Empty);
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x0005AD6B File Offset: 0x00059D6B
		public static void DrawComboButton(Graphics graphics, Rectangle rectangle, ButtonState state)
		{
			ControlPaint.DrawComboButton(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, state);
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x0005AD90 File Offset: 0x00059D90
		public static void DrawComboButton(Graphics graphics, int x, int y, int width, int height, ButtonState state)
		{
			ControlPaint.DrawFrameControl(graphics, x, y, width, height, 3, (int)((ButtonState)5 | state), Color.Empty, Color.Empty);
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x0005ADB8 File Offset: 0x00059DB8
		public static void DrawContainerGrabHandle(Graphics graphics, Rectangle bounds)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			Brush white = Brushes.White;
			Pen black = Pens.Black;
			graphics.FillRectangle(white, bounds.Left + 1, bounds.Top + 1, bounds.Width - 2, bounds.Height - 2);
			graphics.DrawLine(black, bounds.X + 1, bounds.Y, bounds.Right - 2, bounds.Y);
			graphics.DrawLine(black, bounds.X + 1, bounds.Bottom - 1, bounds.Right - 2, bounds.Bottom - 1);
			graphics.DrawLine(black, bounds.X, bounds.Y + 1, bounds.X, bounds.Bottom - 2);
			graphics.DrawLine(black, bounds.Right - 1, bounds.Y + 1, bounds.Right - 1, bounds.Bottom - 2);
			int num = bounds.X + bounds.Width / 2;
			int num2 = bounds.Y + bounds.Height / 2;
			graphics.DrawLine(black, num, bounds.Y, num, bounds.Bottom - 2);
			graphics.DrawLine(black, bounds.X, num2, bounds.Right - 2, num2);
			graphics.DrawLine(black, num - 1, bounds.Y + 2, num + 1, bounds.Y + 2);
			graphics.DrawLine(black, num - 2, bounds.Y + 3, num + 2, bounds.Y + 3);
			graphics.DrawLine(black, bounds.X + 2, num2 - 1, bounds.X + 2, num2 + 1);
			graphics.DrawLine(black, bounds.X + 3, num2 - 2, bounds.X + 3, num2 + 2);
			graphics.DrawLine(black, bounds.Right - 3, num2 - 1, bounds.Right - 3, num2 + 1);
			graphics.DrawLine(black, bounds.Right - 4, num2 - 2, bounds.Right - 4, num2 + 2);
			graphics.DrawLine(black, num - 1, bounds.Bottom - 3, num + 1, bounds.Bottom - 3);
			graphics.DrawLine(black, num - 2, bounds.Bottom - 4, num + 2, bounds.Bottom - 4);
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x0005AFFC File Offset: 0x00059FFC
		private static void DrawFlatCheckBox(Graphics graphics, Rectangle rectangle, ButtonState state)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			Brush brush = (((state & ButtonState.Inactive) == ButtonState.Inactive) ? SystemBrushes.Control : SystemBrushes.Window);
			Color color = (((state & ButtonState.Inactive) == ButtonState.Inactive) ? SystemColors.ControlDark : SystemColors.ControlText);
			ControlPaint.DrawFlatCheckBox(graphics, rectangle, color, brush, state);
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x0005B058 File Offset: 0x0005A058
		private static void DrawFlatCheckBox(Graphics graphics, Rectangle rectangle, Color foreground, Brush background, ButtonState state)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (rectangle.Width < 0 || rectangle.Height < 0)
			{
				throw new ArgumentOutOfRangeException("rectangle");
			}
			Rectangle rectangle2 = new Rectangle(rectangle.X + 1, rectangle.Y + 1, rectangle.Width - 2, rectangle.Height - 2);
			graphics.FillRectangle(background, rectangle2);
			if ((state & ButtonState.Checked) == ButtonState.Checked)
			{
				if (ControlPaint.checkImage == null || ControlPaint.checkImage.Width != rectangle.Width || ControlPaint.checkImage.Height != rectangle.Height)
				{
					if (ControlPaint.checkImage != null)
					{
						ControlPaint.checkImage.Dispose();
						ControlPaint.checkImage = null;
					}
					NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(0, 0, rectangle.Width, rectangle.Height);
					Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height);
					using (Graphics graphics2 = Graphics.FromImage(bitmap))
					{
						graphics2.Clear(Color.Transparent);
						IntPtr hdc = graphics2.GetHdc();
						try
						{
							SafeNativeMethods.DrawFrameControl(new HandleRef(null, hdc), ref rect, 2, 1);
						}
						finally
						{
							graphics2.ReleaseHdcInternal(hdc);
						}
					}
					bitmap.MakeTransparent();
					ControlPaint.checkImage = bitmap;
				}
				rectangle.X++;
				ControlPaint.DrawImageColorized(graphics, ControlPaint.checkImage, rectangle, foreground);
				rectangle.X--;
			}
			Pen controlDark = SystemPens.ControlDark;
			graphics.DrawRectangle(controlDark, rectangle2.X, rectangle2.Y, rectangle2.Width - 1, rectangle2.Height - 1);
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x0005B210 File Offset: 0x0005A210
		public static void DrawFocusRectangle(Graphics graphics, Rectangle rectangle)
		{
			ControlPaint.DrawFocusRectangle(graphics, rectangle, SystemColors.ControlText, SystemColors.Control);
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x0005B224 File Offset: 0x0005A224
		public static void DrawFocusRectangle(Graphics graphics, Rectangle rectangle, Color foreColor, Color backColor)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			rectangle.Width--;
			rectangle.Height--;
			graphics.DrawRectangle(ControlPaint.GetFocusPen(backColor, (rectangle.X + rectangle.Y) % 2 == 1), rectangle);
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x0005B280 File Offset: 0x0005A280
		private static void DrawFrameControl(Graphics graphics, int x, int y, int width, int height, int kind, int state, Color foreColor, Color backColor)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (width < 0)
			{
				throw new ArgumentOutOfRangeException("width");
			}
			if (height < 0)
			{
				throw new ArgumentOutOfRangeException("height");
			}
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(0, 0, width, height);
			using (Bitmap bitmap = new Bitmap(width, height))
			{
				using (Graphics graphics2 = Graphics.FromImage(bitmap))
				{
					graphics2.Clear(Color.Transparent);
					using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics2))
					{
						SafeNativeMethods.DrawFrameControl(new HandleRef(windowsGraphics, windowsGraphics.DeviceContext.Hdc), ref rect, kind, state);
					}
					if (foreColor == Color.Empty || backColor == Color.Empty)
					{
						graphics.DrawImage(bitmap, x, y);
					}
					else
					{
						ImageAttributes imageAttributes = new ImageAttributes();
						imageAttributes.SetRemapTable(new ColorMap[]
						{
							new ColorMap
							{
								OldColor = Color.Black,
								NewColor = foreColor
							},
							new ColorMap
							{
								OldColor = Color.White,
								NewColor = backColor
							}
						}, ColorAdjustType.Bitmap);
						graphics.DrawImage(bitmap, new Rectangle(x, y, width, height), 0, 0, width, height, GraphicsUnit.Pixel, imageAttributes, null, IntPtr.Zero);
					}
				}
			}
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x0005B3F4 File Offset: 0x0005A3F4
		public static void DrawGrabHandle(Graphics graphics, Rectangle rectangle, bool primary, bool enabled)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			Pen pen;
			Brush brush;
			if (primary)
			{
				if (ControlPaint.grabPenPrimary == null)
				{
					ControlPaint.grabPenPrimary = Pens.Black;
				}
				pen = ControlPaint.grabPenPrimary;
				if (enabled)
				{
					if (ControlPaint.grabBrushPrimary == null)
					{
						ControlPaint.grabBrushPrimary = Brushes.White;
					}
					brush = ControlPaint.grabBrushPrimary;
				}
				else
				{
					brush = SystemBrushes.Control;
				}
			}
			else
			{
				if (ControlPaint.grabPenSecondary == null)
				{
					ControlPaint.grabPenSecondary = Pens.White;
				}
				pen = ControlPaint.grabPenSecondary;
				if (enabled)
				{
					if (ControlPaint.grabBrushSecondary == null)
					{
						ControlPaint.grabBrushSecondary = Brushes.Black;
					}
					brush = ControlPaint.grabBrushSecondary;
				}
				else
				{
					brush = SystemBrushes.Control;
				}
			}
			Rectangle rectangle2 = new Rectangle(rectangle.X + 1, rectangle.Y + 1, rectangle.Width - 1, rectangle.Height - 1);
			graphics.FillRectangle(brush, rectangle2);
			rectangle.Width--;
			rectangle.Height--;
			graphics.DrawRectangle(pen, rectangle);
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x0005B4E0 File Offset: 0x0005A4E0
		public static void DrawGrid(Graphics graphics, Rectangle area, Size pixelsBetweenDots, Color backColor)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (pixelsBetweenDots.Width <= 0 || pixelsBetweenDots.Height <= 0)
			{
				throw new ArgumentOutOfRangeException("pixelsBetweenDots");
			}
			float brightness = backColor.GetBrightness();
			bool flag = (double)brightness < 0.5;
			if (ControlPaint.gridBrush == null || ControlPaint.gridSize.Width != pixelsBetweenDots.Width || ControlPaint.gridSize.Height != pixelsBetweenDots.Height || flag != ControlPaint.gridInvert)
			{
				if (ControlPaint.gridBrush != null)
				{
					ControlPaint.gridBrush.Dispose();
					ControlPaint.gridBrush = null;
				}
				ControlPaint.gridSize = pixelsBetweenDots;
				int num = 16;
				ControlPaint.gridInvert = flag;
				Color color = (ControlPaint.gridInvert ? Color.White : Color.Black);
				int num2 = (num / pixelsBetweenDots.Width + 1) * pixelsBetweenDots.Width;
				int num3 = (num / pixelsBetweenDots.Height + 1) * pixelsBetweenDots.Height;
				Bitmap bitmap = new Bitmap(num2, num3);
				for (int i = 0; i < num2; i += pixelsBetweenDots.Width)
				{
					for (int j = 0; j < num3; j += pixelsBetweenDots.Height)
					{
						bitmap.SetPixel(i, j, color);
					}
				}
				ControlPaint.gridBrush = new TextureBrush(bitmap);
				bitmap.Dispose();
			}
			graphics.FillRectangle(ControlPaint.gridBrush, area);
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x0005B630 File Offset: 0x0005A630
		internal static void DrawImageColorized(Graphics graphics, Image image, Rectangle destination, Color replaceBlack)
		{
			ControlPaint.DrawImageColorized(graphics, image, destination, ControlPaint.RemapBlackAndWhitePreserveTransparentMatrix(replaceBlack, Color.White));
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x0005B645 File Offset: 0x0005A645
		internal static bool IsImageTransparent(Image backgroundImage)
		{
			return backgroundImage != null && (backgroundImage.Flags & 2) > 0;
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x0005B658 File Offset: 0x0005A658
		internal static void DrawImageReplaceColor(Graphics g, Image image, Rectangle dest, Color oldColor, Color newColor)
		{
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetRemapTable(new ColorMap[]
			{
				new ColorMap
				{
					OldColor = oldColor,
					NewColor = newColor
				}
			}, ColorAdjustType.Bitmap);
			g.DrawImage(image, dest, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes, null, IntPtr.Zero);
			imageAttributes.Dispose();
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x0005B6B8 File Offset: 0x0005A6B8
		private static void DrawImageColorized(Graphics graphics, Image image, Rectangle destination, ColorMatrix matrix)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetColorMatrix(matrix);
			graphics.DrawImage(image, destination, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes, null, IntPtr.Zero);
			imageAttributes.Dispose();
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x0005B704 File Offset: 0x0005A704
		public static void DrawImageDisabled(Graphics graphics, Image image, int x, int y, Color background)
		{
			ControlPaint.DrawImageDisabled(graphics, image, new Rectangle(x, y, image.Width, image.Height), background, false);
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x0005B784 File Offset: 0x0005A784
		internal static void DrawImageDisabled(Graphics graphics, Image image, Rectangle imageBounds, Color background, bool unscaledImage)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			Size size = image.Size;
			if (ControlPaint.disabledImageAttr == null)
			{
				float[][] array = new float[5][];
				array[0] = new float[] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f };
				array[1] = new float[] { 0.2577f, 0.2577f, 0.2577f, 0f, 0f };
				array[2] = new float[] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f };
				float[][] array2 = array;
				int num = 3;
				float[] array3 = new float[5];
				array3[3] = 1f;
				array2[num] = array3;
				array[4] = new float[] { 0.38f, 0.38f, 0.38f, 0f, 1f };
				ColorMatrix colorMatrix = new ColorMatrix(array);
				ControlPaint.disabledImageAttr = new ImageAttributes();
				ControlPaint.disabledImageAttr.ClearColorKey();
				ControlPaint.disabledImageAttr.SetColorMatrix(colorMatrix);
			}
			if (unscaledImage)
			{
				using (Bitmap bitmap = new Bitmap(image.Width, image.Height))
				{
					using (Graphics graphics2 = Graphics.FromImage(bitmap))
					{
						graphics2.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, ControlPaint.disabledImageAttr);
					}
					graphics.DrawImageUnscaled(bitmap, imageBounds);
					return;
				}
			}
			graphics.DrawImage(image, imageBounds, 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, ControlPaint.disabledImageAttr);
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x0005B900 File Offset: 0x0005A900
		public static void DrawLockedFrame(Graphics graphics, Rectangle rectangle, bool primary)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			Pen pen;
			if (primary)
			{
				pen = Pens.White;
			}
			else
			{
				pen = Pens.Black;
			}
			graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
			rectangle.Inflate(-1, -1);
			graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
			if (primary)
			{
				pen = Pens.Black;
			}
			else
			{
				pen = Pens.White;
			}
			rectangle.Inflate(-1, -1);
			graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x0005B9C4 File Offset: 0x0005A9C4
		public static void DrawMenuGlyph(Graphics graphics, Rectangle rectangle, MenuGlyph glyph)
		{
			ControlPaint.DrawMenuGlyph(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, glyph);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x0005B9E9 File Offset: 0x0005A9E9
		public static void DrawMenuGlyph(Graphics graphics, Rectangle rectangle, MenuGlyph glyph, Color foreColor, Color backColor)
		{
			ControlPaint.DrawMenuGlyph(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, glyph, foreColor, backColor);
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x0005BA14 File Offset: 0x0005AA14
		public static void DrawMenuGlyph(Graphics graphics, int x, int y, int width, int height, MenuGlyph glyph)
		{
			ControlPaint.DrawFrameControl(graphics, x, y, width, height, 2, (int)glyph, Color.Empty, Color.Empty);
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x0005BA3C File Offset: 0x0005AA3C
		public static void DrawMenuGlyph(Graphics graphics, int x, int y, int width, int height, MenuGlyph glyph, Color foreColor, Color backColor)
		{
			ControlPaint.DrawFrameControl(graphics, x, y, width, height, 2, (int)glyph, foreColor, backColor);
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x0005BA5B File Offset: 0x0005AA5B
		public static void DrawMixedCheckBox(Graphics graphics, Rectangle rectangle, ButtonState state)
		{
			ControlPaint.DrawMixedCheckBox(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, state);
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x0005BA80 File Offset: 0x0005AA80
		public static void DrawMixedCheckBox(Graphics graphics, int x, int y, int width, int height, ButtonState state)
		{
			ControlPaint.DrawFrameControl(graphics, x, y, width, height, 4, (int)((ButtonState)8 | state), Color.Empty, Color.Empty);
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x0005BAA7 File Offset: 0x0005AAA7
		public static void DrawRadioButton(Graphics graphics, Rectangle rectangle, ButtonState state)
		{
			ControlPaint.DrawRadioButton(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, state);
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x0005BACC File Offset: 0x0005AACC
		public static void DrawRadioButton(Graphics graphics, int x, int y, int width, int height, ButtonState state)
		{
			ControlPaint.DrawFrameControl(graphics, x, y, width, height, 4, (int)((ButtonState)4 | state), Color.Empty, Color.Empty);
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x0005BAF4 File Offset: 0x0005AAF4
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		public static void DrawReversibleFrame(Rectangle rectangle, Color backColor, FrameStyle style)
		{
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
			IntPtr dcex = UnsafeNativeMethods.GetDCEx(new HandleRef(null, UnsafeNativeMethods.GetDesktopWindow()), NativeMethods.NullHandleRef, 1027);
			IntPtr intPtr;
			switch (style)
			{
			case FrameStyle.Dashed:
				intPtr = SafeNativeMethods.CreatePen(2, 1, ColorTranslator.ToWin32(backColor));
				goto IL_0073;
			}
			intPtr = SafeNativeMethods.CreatePen(0, 2, ColorTranslator.ToWin32(backColor));
			IL_0073:
			int num2 = SafeNativeMethods.SetROP2(new HandleRef(null, dcex), num);
			IntPtr intPtr2 = SafeNativeMethods.SelectObject(new HandleRef(null, dcex), new HandleRef(null, UnsafeNativeMethods.GetStockObject(5)));
			IntPtr intPtr3 = SafeNativeMethods.SelectObject(new HandleRef(null, dcex), new HandleRef(null, intPtr));
			SafeNativeMethods.SetBkColor(new HandleRef(null, dcex), ColorTranslator.ToWin32(color));
			SafeNativeMethods.Rectangle(new HandleRef(null, dcex), rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
			SafeNativeMethods.SetROP2(new HandleRef(null, dcex), num2);
			SafeNativeMethods.SelectObject(new HandleRef(null, dcex), new HandleRef(null, intPtr2));
			SafeNativeMethods.SelectObject(new HandleRef(null, dcex), new HandleRef(null, intPtr3));
			if (intPtr != IntPtr.Zero)
			{
				SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
			}
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dcex));
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x0005BC54 File Offset: 0x0005AC54
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		public static void DrawReversibleLine(Point start, Point end, Color backColor)
		{
			int colorRop = ControlPaint.GetColorRop(backColor, 10, 7);
			IntPtr dcex = UnsafeNativeMethods.GetDCEx(new HandleRef(null, UnsafeNativeMethods.GetDesktopWindow()), NativeMethods.NullHandleRef, 1027);
			IntPtr intPtr = SafeNativeMethods.CreatePen(0, 1, ColorTranslator.ToWin32(backColor));
			int num = SafeNativeMethods.SetROP2(new HandleRef(null, dcex), colorRop);
			IntPtr intPtr2 = SafeNativeMethods.SelectObject(new HandleRef(null, dcex), new HandleRef(null, UnsafeNativeMethods.GetStockObject(5)));
			IntPtr intPtr3 = SafeNativeMethods.SelectObject(new HandleRef(null, dcex), new HandleRef(null, intPtr));
			SafeNativeMethods.MoveToEx(new HandleRef(null, dcex), start.X, start.Y, null);
			SafeNativeMethods.LineTo(new HandleRef(null, dcex), end.X, end.Y);
			SafeNativeMethods.SetROP2(new HandleRef(null, dcex), num);
			SafeNativeMethods.SelectObject(new HandleRef(null, dcex), new HandleRef(null, intPtr2));
			SafeNativeMethods.SelectObject(new HandleRef(null, dcex), new HandleRef(null, intPtr3));
			SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dcex));
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x0005BD5F File Offset: 0x0005AD5F
		public static void DrawScrollButton(Graphics graphics, Rectangle rectangle, ScrollButton button, ButtonState state)
		{
			ControlPaint.DrawScrollButton(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, button, state);
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x0005BD88 File Offset: 0x0005AD88
		public static void DrawScrollButton(Graphics graphics, int x, int y, int width, int height, ScrollButton button, ButtonState state)
		{
			ControlPaint.DrawFrameControl(graphics, x, y, width, height, 3, (int)(button | (ScrollButton)state), Color.Empty, Color.Empty);
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x0005BDB0 File Offset: 0x0005ADB0
		public static void DrawSelectionFrame(Graphics graphics, bool active, Rectangle outsideRect, Rectangle insideRect, Color backColor)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			Brush brush;
			if (active)
			{
				brush = ControlPaint.GetActiveBrush(backColor);
			}
			else
			{
				brush = ControlPaint.GetSelectedBrush(backColor);
			}
			Region clip = graphics.Clip;
			graphics.ExcludeClip(insideRect);
			graphics.FillRectangle(brush, outsideRect);
			graphics.Clip = clip;
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x0005BDFD File Offset: 0x0005ADFD
		public static void DrawSizeGrip(Graphics graphics, Color backColor, Rectangle bounds)
		{
			ControlPaint.DrawSizeGrip(graphics, backColor, bounds.X, bounds.Y, bounds.Width, bounds.Height);
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x0005BE24 File Offset: 0x0005AE24
		public static void DrawSizeGrip(Graphics graphics, Color backColor, int x, int y, int width, int height)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			using (Pen pen = new Pen(ControlPaint.LightLight(backColor)))
			{
				using (Pen pen2 = new Pen(ControlPaint.Dark(backColor)))
				{
					int num = Math.Min(width, height);
					int num2 = x + width - 1;
					int num3 = y + height - 2;
					for (int i = 0; i < num - 4; i += 4)
					{
						graphics.DrawLine(pen2, num2 - (i + 1) - 2, num3, num2, num3 - (i + 1) - 2);
						graphics.DrawLine(pen2, num2 - (i + 2) - 2, num3, num2, num3 - (i + 2) - 2);
						graphics.DrawLine(pen, num2 - (i + 3) - 2, num3, num2, num3 - (i + 3) - 2);
					}
				}
			}
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x0005BF08 File Offset: 0x0005AF08
		public static void DrawStringDisabled(Graphics graphics, string s, Font font, Color color, RectangleF layoutRectangle, StringFormat format)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			layoutRectangle.Offset(1f, 1f);
			SolidBrush solidBrush = new SolidBrush(ControlPaint.LightLight(color));
			try
			{
				graphics.DrawString(s, font, solidBrush, layoutRectangle, format);
				layoutRectangle.Offset(-1f, -1f);
				color = ControlPaint.Dark(color);
				solidBrush.Color = color;
				graphics.DrawString(s, font, solidBrush, layoutRectangle, format);
			}
			finally
			{
				solidBrush.Dispose();
			}
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x0005BF94 File Offset: 0x0005AF94
		public static void DrawStringDisabled(IDeviceContext dc, string s, Font font, Color color, Rectangle layoutRectangle, TextFormatFlags format)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			layoutRectangle.Offset(1, 1);
			Color color2 = ControlPaint.LightLight(color);
			TextRenderer.DrawText(dc, s, font, layoutRectangle, color2, format);
			layoutRectangle.Offset(-1, -1);
			color2 = ControlPaint.Dark(color);
			TextRenderer.DrawText(dc, s, font, layoutRectangle, color2, format);
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x0005BFEC File Offset: 0x0005AFEC
		public static void DrawVisualStyleBorder(Graphics graphics, Rectangle bounds)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			using (Pen pen = new Pen(VisualStyleInformation.TextControlBorder))
			{
				graphics.DrawRectangle(pen, bounds);
			}
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x0005C038 File Offset: 0x0005B038
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		public static void FillReversibleRectangle(Rectangle rectangle, Color backColor)
		{
			int colorRop = ControlPaint.GetColorRop(backColor, 10813541, 5898313);
			int colorRop2 = ControlPaint.GetColorRop(backColor, 6, 6);
			IntPtr dcex = UnsafeNativeMethods.GetDCEx(new HandleRef(null, UnsafeNativeMethods.GetDesktopWindow()), NativeMethods.NullHandleRef, 1027);
			IntPtr intPtr = SafeNativeMethods.CreateSolidBrush(ColorTranslator.ToWin32(backColor));
			int num = SafeNativeMethods.SetROP2(new HandleRef(null, dcex), colorRop2);
			IntPtr intPtr2 = SafeNativeMethods.SelectObject(new HandleRef(null, dcex), new HandleRef(null, intPtr));
			SafeNativeMethods.PatBlt(new HandleRef(null, dcex), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, colorRop);
			SafeNativeMethods.SetROP2(new HandleRef(null, dcex), num);
			SafeNativeMethods.SelectObject(new HandleRef(null, dcex), new HandleRef(null, intPtr2));
			SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dcex));
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x0005C117 File Offset: 0x0005B117
		internal static Font FontInPoints(Font font)
		{
			return new Font(font.FontFamily, font.SizeInPoints, font.Style, GraphicsUnit.Point, font.GdiCharSet, font.GdiVerticalFont);
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x0005C140 File Offset: 0x0005B140
		internal static bool FontToIFont(Font source, UnsafeNativeMethods.IFont target)
		{
			bool flag = false;
			string name = target.GetName();
			if (!source.Name.Equals(name))
			{
				target.SetName(source.Name);
				flag = true;
			}
			float num = (float)target.GetSize() / 10000f;
			float sizeInPoints = source.SizeInPoints;
			if (sizeInPoints != num)
			{
				target.SetSize((long)(sizeInPoints * 10000f));
				flag = true;
			}
			NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
			IntSecurity.ObjectFromWin32Handle.Assert();
			try
			{
				source.ToLogFont(logfont);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			short weight = target.GetWeight();
			if ((int)weight != logfont.lfWeight)
			{
				target.SetWeight((short)logfont.lfWeight);
				flag = true;
			}
			bool bold = target.GetBold();
			if (bold != logfont.lfWeight >= 700)
			{
				target.SetBold(logfont.lfWeight >= 700);
				flag = true;
			}
			bool italic = target.GetItalic();
			if (italic != (0 != logfont.lfItalic))
			{
				target.SetItalic(0 != logfont.lfItalic);
				flag = true;
			}
			bool underline = target.GetUnderline();
			if (underline != (0 != logfont.lfUnderline))
			{
				target.SetUnderline(0 != logfont.lfUnderline);
				flag = true;
			}
			bool strikethrough = target.GetStrikethrough();
			if (strikethrough != (0 != logfont.lfStrikeOut))
			{
				target.SetStrikethrough(0 != logfont.lfStrikeOut);
				flag = true;
			}
			short charset = target.GetCharset();
			if (charset != (short)logfont.lfCharSet)
			{
				target.SetCharset((short)logfont.lfCharSet);
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x0005C2D4 File Offset: 0x0005B2D4
		private static int GetColorRop(Color color, int darkROP, int lightROP)
		{
			if ((double)color.GetBrightness() < 0.5)
			{
				return darkROP;
			}
			return lightROP;
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x0005C2EC File Offset: 0x0005B2EC
		private static Brush GetActiveBrush(Color backColor)
		{
			Color color;
			if ((double)backColor.GetBrightness() <= 0.5)
			{
				color = SystemColors.ControlLight;
			}
			else
			{
				color = SystemColors.ControlDark;
			}
			if (ControlPaint.frameBrushActive == null || !ControlPaint.frameColorActive.Equals(color))
			{
				if (ControlPaint.frameBrushActive != null)
				{
					ControlPaint.frameBrushActive.Dispose();
					ControlPaint.frameBrushActive = null;
				}
				ControlPaint.frameColorActive = color;
				int num = 8;
				Bitmap bitmap = new Bitmap(num, num);
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num; j++)
					{
						bitmap.SetPixel(i, j, Color.Transparent);
					}
				}
				for (int k = 0; k < num; k++)
				{
					for (int l = -k; l < num; l += 4)
					{
						if (l >= 0)
						{
							bitmap.SetPixel(l, k, color);
						}
					}
				}
				ControlPaint.frameBrushActive = new TextureBrush(bitmap);
				bitmap.Dispose();
			}
			return ControlPaint.frameBrushActive;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x0005C3D8 File Offset: 0x0005B3D8
		private static Pen GetFocusPen(Color backColor, bool odds)
		{
			if (ControlPaint.focusPen == null || ((double)ControlPaint.focusPenColor.GetBrightness() <= 0.5 && (double)backColor.GetBrightness() <= 0.5) || !ControlPaint.focusPenColor.Equals(backColor))
			{
				if (ControlPaint.focusPen != null)
				{
					ControlPaint.focusPen.Dispose();
					ControlPaint.focusPen = null;
					ControlPaint.focusPenInvert.Dispose();
					ControlPaint.focusPenInvert = null;
				}
				ControlPaint.focusPenColor = backColor;
				Bitmap bitmap = new Bitmap(2, 2);
				Color color = Color.Transparent;
				Color color2 = Color.Black;
				if ((double)backColor.GetBrightness() <= 0.5)
				{
					color = color2;
					color2 = ControlPaint.InvertColor(backColor);
				}
				else if (backColor == Color.Transparent)
				{
					color = Color.White;
				}
				bitmap.SetPixel(1, 0, color2);
				bitmap.SetPixel(0, 1, color2);
				bitmap.SetPixel(0, 0, color);
				bitmap.SetPixel(1, 1, color);
				Brush brush = new TextureBrush(bitmap);
				ControlPaint.focusPen = new Pen(brush, 1f);
				brush.Dispose();
				bitmap.SetPixel(1, 0, color);
				bitmap.SetPixel(0, 1, color);
				bitmap.SetPixel(0, 0, color2);
				bitmap.SetPixel(1, 1, color2);
				brush = new TextureBrush(bitmap);
				ControlPaint.focusPenInvert = new Pen(brush, 1f);
				brush.Dispose();
				bitmap.Dispose();
			}
			if (!odds)
			{
				return ControlPaint.focusPenInvert;
			}
			return ControlPaint.focusPen;
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x0005C538 File Offset: 0x0005B538
		private static Brush GetSelectedBrush(Color backColor)
		{
			Color color;
			if ((double)backColor.GetBrightness() <= 0.5)
			{
				color = SystemColors.ControlLight;
			}
			else
			{
				color = SystemColors.ControlDark;
			}
			if (ControlPaint.frameBrushSelected == null || !ControlPaint.frameColorSelected.Equals(color))
			{
				if (ControlPaint.frameBrushSelected != null)
				{
					ControlPaint.frameBrushSelected.Dispose();
					ControlPaint.frameBrushSelected = null;
				}
				ControlPaint.frameColorSelected = color;
				int num = 8;
				Bitmap bitmap = new Bitmap(num, num);
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num; j++)
					{
						bitmap.SetPixel(i, j, Color.Transparent);
					}
				}
				int num2 = 0;
				for (int k = 0; k < num; k += 2)
				{
					for (int l = num2; l < num; l += 2)
					{
						bitmap.SetPixel(k, l, color);
					}
					num2 ^= 1;
				}
				ControlPaint.frameBrushSelected = new TextureBrush(bitmap);
				bitmap.Dispose();
			}
			return ControlPaint.frameBrushSelected;
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x0005C624 File Offset: 0x0005B624
		private static float InfinityToOne(float value)
		{
			if (value == float.NegativeInfinity || value == float.PositiveInfinity)
			{
				return 1f;
			}
			return value;
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x0005C63D File Offset: 0x0005B63D
		private static Color InvertColor(Color color)
		{
			return Color.FromArgb((int)color.A, (int)(~color.R), (int)(~color.G), (int)(~color.B));
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x0005C668 File Offset: 0x0005B668
		public static Color Light(Color baseColor, float percOfLightLight)
		{
			return new ControlPaint.HLSColor(baseColor).Lighter(percOfLightLight);
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x0005C684 File Offset: 0x0005B684
		public static Color Light(Color baseColor)
		{
			return new ControlPaint.HLSColor(baseColor).Lighter(0.5f);
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x0005C6A4 File Offset: 0x0005B6A4
		public static Color LightLight(Color baseColor)
		{
			return new ControlPaint.HLSColor(baseColor).Lighter(1f);
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x0005C6C4 File Offset: 0x0005B6C4
		internal static ColorMatrix MultiplyColorMatrix(float[][] matrix1, float[][] matrix2)
		{
			int num = 5;
			float[][] array = new float[num][];
			for (int i = 0; i < num; i++)
			{
				array[i] = new float[num];
			}
			float[] array2 = new float[num];
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					array2[k] = matrix1[k][j];
				}
				for (int l = 0; l < num; l++)
				{
					float[] array3 = matrix2[l];
					float num2 = 0f;
					for (int m = 0; m < num; m++)
					{
						num2 += array3[m] * array2[m];
					}
					array[l][j] = num2;
				}
			}
			return new ColorMatrix(array);
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x0005C76C File Offset: 0x0005B76C
		internal static void PaintTableControlBorder(TableLayoutPanelCellBorderStyle borderStyle, Graphics g, Rectangle bound)
		{
			int x = bound.X;
			int y = bound.Y;
			int right = bound.Right;
			int bottom = bound.Bottom;
			switch (borderStyle)
			{
			case TableLayoutPanelCellBorderStyle.None:
			case TableLayoutPanelCellBorderStyle.Single:
				return;
			case TableLayoutPanelCellBorderStyle.Inset:
			case TableLayoutPanelCellBorderStyle.InsetDouble:
			{
				g.DrawLine(SystemPens.ControlDark, x, y, right - 1, y);
				g.DrawLine(SystemPens.ControlDark, x, y, x, bottom - 1);
				using (Pen pen = new Pen(SystemColors.Window))
				{
					g.DrawLine(pen, right - 1, y, right - 1, bottom - 1);
					g.DrawLine(pen, x, bottom - 1, right - 1, bottom - 1);
					return;
				}
				break;
			}
			case TableLayoutPanelCellBorderStyle.Outset:
			case TableLayoutPanelCellBorderStyle.OutsetDouble:
			case TableLayoutPanelCellBorderStyle.OutsetPartial:
				break;
			default:
				return;
			}
			using (Pen pen2 = new Pen(SystemColors.Window))
			{
				g.DrawLine(pen2, x, y, right - 1, y);
				g.DrawLine(pen2, x, y, x, bottom - 1);
			}
			g.DrawLine(SystemPens.ControlDark, right - 1, y, right - 1, bottom - 1);
			g.DrawLine(SystemPens.ControlDark, x, bottom - 1, right - 1, bottom - 1);
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x0005C89C File Offset: 0x0005B89C
		internal static void PaintTableCellBorder(TableLayoutPanelCellBorderStyle borderStyle, Graphics g, Rectangle bound)
		{
			switch (borderStyle)
			{
			case TableLayoutPanelCellBorderStyle.None:
				return;
			case TableLayoutPanelCellBorderStyle.Single:
				g.DrawRectangle(SystemPens.ControlDark, bound);
				return;
			case TableLayoutPanelCellBorderStyle.Inset:
			{
				using (Pen pen = new Pen(SystemColors.Window))
				{
					g.DrawLine(pen, bound.X, bound.Y, bound.X + bound.Width - 1, bound.Y);
					g.DrawLine(pen, bound.X, bound.Y, bound.X, bound.Y + bound.Height - 1);
				}
				g.DrawLine(SystemPens.ControlDark, bound.X + bound.Width - 1, bound.Y, bound.X + bound.Width - 1, bound.Y + bound.Height - 1);
				g.DrawLine(SystemPens.ControlDark, bound.X, bound.Y + bound.Height - 1, bound.X + bound.Width - 1, bound.Y + bound.Height - 1);
				return;
			}
			case TableLayoutPanelCellBorderStyle.InsetDouble:
			{
				g.DrawRectangle(SystemPens.Control, bound);
				bound = new Rectangle(bound.X + 1, bound.Y + 1, bound.Width - 1, bound.Height - 1);
				using (Pen pen2 = new Pen(SystemColors.Window))
				{
					g.DrawLine(pen2, bound.X, bound.Y, bound.X + bound.Width - 1, bound.Y);
					g.DrawLine(pen2, bound.X, bound.Y, bound.X, bound.Y + bound.Height - 1);
				}
				g.DrawLine(SystemPens.ControlDark, bound.X + bound.Width - 1, bound.Y, bound.X + bound.Width - 1, bound.Y + bound.Height - 1);
				g.DrawLine(SystemPens.ControlDark, bound.X, bound.Y + bound.Height - 1, bound.X + bound.Width - 1, bound.Y + bound.Height - 1);
				return;
			}
			case TableLayoutPanelCellBorderStyle.Outset:
			{
				g.DrawLine(SystemPens.ControlDark, bound.X, bound.Y, bound.X + bound.Width - 1, bound.Y);
				g.DrawLine(SystemPens.ControlDark, bound.X, bound.Y, bound.X, bound.Y + bound.Height - 1);
				using (Pen pen3 = new Pen(SystemColors.Window))
				{
					g.DrawLine(pen3, bound.X + bound.Width - 1, bound.Y, bound.X + bound.Width - 1, bound.Y + bound.Height - 1);
					g.DrawLine(pen3, bound.X, bound.Y + bound.Height - 1, bound.X + bound.Width - 1, bound.Y + bound.Height - 1);
					return;
				}
				break;
			}
			case TableLayoutPanelCellBorderStyle.OutsetDouble:
			case TableLayoutPanelCellBorderStyle.OutsetPartial:
				break;
			default:
				return;
			}
			g.DrawRectangle(SystemPens.Control, bound);
			bound = new Rectangle(bound.X + 1, bound.Y + 1, bound.Width - 1, bound.Height - 1);
			g.DrawLine(SystemPens.ControlDark, bound.X, bound.Y, bound.X + bound.Width - 1, bound.Y);
			g.DrawLine(SystemPens.ControlDark, bound.X, bound.Y, bound.X, bound.Y + bound.Height - 1);
			using (Pen pen4 = new Pen(SystemColors.Window))
			{
				g.DrawLine(pen4, bound.X + bound.Width - 1, bound.Y, bound.X + bound.Width - 1, bound.Y + bound.Height - 1);
				g.DrawLine(pen4, bound.X, bound.Y + bound.Height - 1, bound.X + bound.Width - 1, bound.Y + bound.Height - 1);
			}
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x0005CD70 File Offset: 0x0005BD70
		private static ColorMatrix RemapBlackAndWhitePreserveTransparentMatrix(Color replaceBlack, Color replaceWhite)
		{
			float num = (float)replaceBlack.R / 255f;
			float num2 = (float)replaceBlack.G / 255f;
			float num3 = (float)replaceBlack.B / 255f;
			float num4 = (float)replaceBlack.A / 255f;
			float num5 = (float)replaceWhite.R / 255f;
			float num6 = (float)replaceWhite.G / 255f;
			float num7 = (float)replaceWhite.B / 255f;
			float num8 = (float)replaceWhite.A / 255f;
			return new ColorMatrix
			{
				Matrix00 = -num,
				Matrix01 = -num2,
				Matrix02 = -num3,
				Matrix10 = num5,
				Matrix11 = num6,
				Matrix12 = num7,
				Matrix33 = 1f,
				Matrix40 = num,
				Matrix41 = num2,
				Matrix42 = num3,
				Matrix44 = 1f
			};
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x0005CE68 File Offset: 0x0005BE68
		internal static TextFormatFlags TextFormatFlagsForAlignmentGDI(global::System.Drawing.ContentAlignment align)
		{
			TextFormatFlags textFormatFlags = TextFormatFlags.Default;
			textFormatFlags |= ControlPaint.TranslateAlignmentForGDI(align);
			return textFormatFlags | ControlPaint.TranslateLineAlignmentForGDI(align);
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x0005CE8C File Offset: 0x0005BE8C
		internal static StringAlignment TranslateAlignment(global::System.Drawing.ContentAlignment align)
		{
			StringAlignment stringAlignment;
			if ((align & ControlPaint.anyRight) != (global::System.Drawing.ContentAlignment)0)
			{
				stringAlignment = StringAlignment.Far;
			}
			else if ((align & ControlPaint.anyCenter) != (global::System.Drawing.ContentAlignment)0)
			{
				stringAlignment = StringAlignment.Center;
			}
			else
			{
				stringAlignment = StringAlignment.Near;
			}
			return stringAlignment;
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x0005CEB8 File Offset: 0x0005BEB8
		internal static TextFormatFlags TranslateAlignmentForGDI(global::System.Drawing.ContentAlignment align)
		{
			TextFormatFlags textFormatFlags;
			if ((align & ControlPaint.anyBottom) != (global::System.Drawing.ContentAlignment)0)
			{
				textFormatFlags = TextFormatFlags.Bottom;
			}
			else if ((align & ControlPaint.anyMiddle) != (global::System.Drawing.ContentAlignment)0)
			{
				textFormatFlags = TextFormatFlags.VerticalCenter;
			}
			else
			{
				textFormatFlags = TextFormatFlags.Default;
			}
			return textFormatFlags;
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x0005CEE4 File Offset: 0x0005BEE4
		internal static StringAlignment TranslateLineAlignment(global::System.Drawing.ContentAlignment align)
		{
			StringAlignment stringAlignment;
			if ((align & ControlPaint.anyBottom) != (global::System.Drawing.ContentAlignment)0)
			{
				stringAlignment = StringAlignment.Far;
			}
			else if ((align & ControlPaint.anyMiddle) != (global::System.Drawing.ContentAlignment)0)
			{
				stringAlignment = StringAlignment.Center;
			}
			else
			{
				stringAlignment = StringAlignment.Near;
			}
			return stringAlignment;
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x0005CF10 File Offset: 0x0005BF10
		internal static TextFormatFlags TranslateLineAlignmentForGDI(global::System.Drawing.ContentAlignment align)
		{
			TextFormatFlags textFormatFlags;
			if ((align & ControlPaint.anyRight) != (global::System.Drawing.ContentAlignment)0)
			{
				textFormatFlags = TextFormatFlags.Right;
			}
			else if ((align & ControlPaint.anyCenter) != (global::System.Drawing.ContentAlignment)0)
			{
				textFormatFlags = TextFormatFlags.HorizontalCenter;
			}
			else
			{
				textFormatFlags = TextFormatFlags.Default;
			}
			return textFormatFlags;
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x0005CF3C File Offset: 0x0005BF3C
		internal static StringFormat StringFormatForAlignment(global::System.Drawing.ContentAlignment align)
		{
			return new StringFormat
			{
				Alignment = ControlPaint.TranslateAlignment(align),
				LineAlignment = ControlPaint.TranslateLineAlignment(align)
			};
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x0005CF68 File Offset: 0x0005BF68
		internal static StringFormat CreateStringFormat(Control ctl, global::System.Drawing.ContentAlignment textAlign, bool showEllipsis, bool useMnemonic)
		{
			StringFormat stringFormat = ControlPaint.StringFormatForAlignment(textAlign);
			if (ctl.RightToLeft == RightToLeft.Yes)
			{
				stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
			}
			if (showEllipsis)
			{
				stringFormat.Trimming = StringTrimming.EllipsisCharacter;
				stringFormat.FormatFlags |= StringFormatFlags.LineLimit;
			}
			if (!useMnemonic)
			{
				stringFormat.HotkeyPrefix = HotkeyPrefix.None;
			}
			else if (ctl.ShowKeyboardCues)
			{
				stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
			}
			else
			{
				stringFormat.HotkeyPrefix = HotkeyPrefix.Hide;
			}
			if (ctl.AutoSize)
			{
				stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
			}
			return stringFormat;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x0005CFF0 File Offset: 0x0005BFF0
		internal static TextFormatFlags CreateTextFormatFlags(Control ctl, global::System.Drawing.ContentAlignment textAlign, bool showEllipsis, bool useMnemonic)
		{
			textAlign = ctl.RtlTranslateContent(textAlign);
			TextFormatFlags textFormatFlags = ControlPaint.TextFormatFlagsForAlignmentGDI(textAlign);
			textFormatFlags |= TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak;
			if (showEllipsis)
			{
				textFormatFlags |= TextFormatFlags.EndEllipsis;
			}
			if (ctl.RightToLeft == RightToLeft.Yes)
			{
				textFormatFlags |= TextFormatFlags.RightToLeft;
			}
			if (!useMnemonic)
			{
				textFormatFlags |= TextFormatFlags.NoPrefix;
			}
			else if (!ctl.ShowKeyboardCues)
			{
				textFormatFlags |= TextFormatFlags.HidePrefix;
			}
			return textFormatFlags;
		}

		// Token: 0x04001609 RID: 5641
		[ThreadStatic]
		private static Bitmap checkImage;

		// Token: 0x0400160A RID: 5642
		[ThreadStatic]
		private static Pen focusPen;

		// Token: 0x0400160B RID: 5643
		[ThreadStatic]
		private static Pen focusPenInvert;

		// Token: 0x0400160C RID: 5644
		private static Color focusPenColor;

		// Token: 0x0400160D RID: 5645
		private static Pen grabPenPrimary;

		// Token: 0x0400160E RID: 5646
		private static Pen grabPenSecondary;

		// Token: 0x0400160F RID: 5647
		private static Brush grabBrushPrimary;

		// Token: 0x04001610 RID: 5648
		private static Brush grabBrushSecondary;

		// Token: 0x04001611 RID: 5649
		[ThreadStatic]
		private static Brush frameBrushActive;

		// Token: 0x04001612 RID: 5650
		private static Color frameColorActive;

		// Token: 0x04001613 RID: 5651
		[ThreadStatic]
		private static Brush frameBrushSelected;

		// Token: 0x04001614 RID: 5652
		private static Color frameColorSelected;

		// Token: 0x04001615 RID: 5653
		[ThreadStatic]
		private static Brush gridBrush;

		// Token: 0x04001616 RID: 5654
		private static Size gridSize;

		// Token: 0x04001617 RID: 5655
		private static bool gridInvert;

		// Token: 0x04001618 RID: 5656
		[ThreadStatic]
		private static ImageAttributes disabledImageAttr;

		// Token: 0x04001619 RID: 5657
		private static readonly global::System.Drawing.ContentAlignment anyRight = (global::System.Drawing.ContentAlignment)1092;

		// Token: 0x0400161A RID: 5658
		private static readonly global::System.Drawing.ContentAlignment anyBottom = (global::System.Drawing.ContentAlignment)1792;

		// Token: 0x0400161B RID: 5659
		private static readonly global::System.Drawing.ContentAlignment anyCenter = (global::System.Drawing.ContentAlignment)546;

		// Token: 0x0400161C RID: 5660
		private static readonly global::System.Drawing.ContentAlignment anyMiddle = (global::System.Drawing.ContentAlignment)112;

		// Token: 0x020002B5 RID: 693
		private struct HLSColor
		{
			// Token: 0x06002643 RID: 9795 RVA: 0x0005D078 File Offset: 0x0005C078
			public HLSColor(Color color)
			{
				this.isSystemColors_Control = color.ToKnownColor() == SystemColors.Control.ToKnownColor();
				int r = (int)color.R;
				int g = (int)color.G;
				int b = (int)color.B;
				int num = Math.Max(Math.Max(r, g), b);
				int num2 = Math.Min(Math.Min(r, g), b);
				int num3 = num + num2;
				this.luminosity = (num3 * 240 + 255) / 510;
				int num4 = num - num2;
				if (num4 == 0)
				{
					this.saturation = 0;
					this.hue = 160;
					return;
				}
				if (this.luminosity <= 120)
				{
					this.saturation = (num4 * 240 + num3 / 2) / num3;
				}
				else
				{
					this.saturation = (num4 * 240 + (510 - num3) / 2) / (510 - num3);
				}
				int num5 = ((num - r) * 40 + num4 / 2) / num4;
				int num6 = ((num - g) * 40 + num4 / 2) / num4;
				int num7 = ((num - b) * 40 + num4 / 2) / num4;
				if (r == num)
				{
					this.hue = num7 - num6;
				}
				else if (g == num)
				{
					this.hue = 80 + num5 - num7;
				}
				else
				{
					this.hue = 160 + num6 - num5;
				}
				if (this.hue < 0)
				{
					this.hue += 240;
				}
				if (this.hue > 240)
				{
					this.hue -= 240;
				}
			}

			// Token: 0x170005FE RID: 1534
			// (get) Token: 0x06002644 RID: 9796 RVA: 0x0005D1F8 File Offset: 0x0005C1F8
			public int Luminosity
			{
				get
				{
					return this.luminosity;
				}
			}

			// Token: 0x06002645 RID: 9797 RVA: 0x0005D200 File Offset: 0x0005C200
			public Color Darker(float percDarker)
			{
				if (!this.isSystemColors_Control)
				{
					int num = 0;
					int num2 = this.NewLuma(-333, true);
					return this.ColorFromHLS(this.hue, num2 - (int)((float)(num2 - num) * percDarker), this.saturation);
				}
				if (percDarker == 0f)
				{
					return SystemColors.ControlDark;
				}
				if (percDarker == 1f)
				{
					return SystemColors.ControlDarkDark;
				}
				Color controlDark = SystemColors.ControlDark;
				Color controlDarkDark = SystemColors.ControlDarkDark;
				int num3 = (int)(controlDark.R - controlDarkDark.R);
				int num4 = (int)(controlDark.G - controlDarkDark.G);
				int num5 = (int)(controlDark.B - controlDarkDark.B);
				return Color.FromArgb((int)(controlDark.R - (byte)((float)num3 * percDarker)), (int)(controlDark.G - (byte)((float)num4 * percDarker)), (int)(controlDark.B - (byte)((float)num5 * percDarker)));
			}

			// Token: 0x06002646 RID: 9798 RVA: 0x0005D2D4 File Offset: 0x0005C2D4
			public override bool Equals(object o)
			{
				if (!(o is ControlPaint.HLSColor))
				{
					return false;
				}
				ControlPaint.HLSColor hlscolor = (ControlPaint.HLSColor)o;
				return this.hue == hlscolor.hue && this.saturation == hlscolor.saturation && this.luminosity == hlscolor.luminosity && this.isSystemColors_Control == hlscolor.isSystemColors_Control;
			}

			// Token: 0x06002647 RID: 9799 RVA: 0x0005D330 File Offset: 0x0005C330
			public static bool operator ==(ControlPaint.HLSColor a, ControlPaint.HLSColor b)
			{
				return a.Equals(b);
			}

			// Token: 0x06002648 RID: 9800 RVA: 0x0005D345 File Offset: 0x0005C345
			public static bool operator !=(ControlPaint.HLSColor a, ControlPaint.HLSColor b)
			{
				return !a.Equals(b);
			}

			// Token: 0x06002649 RID: 9801 RVA: 0x0005D35D File Offset: 0x0005C35D
			public override int GetHashCode()
			{
				return (this.hue << 6) | (this.saturation << 2) | this.luminosity;
			}

			// Token: 0x0600264A RID: 9802 RVA: 0x0005D378 File Offset: 0x0005C378
			public Color Lighter(float percLighter)
			{
				if (!this.isSystemColors_Control)
				{
					int num = this.luminosity;
					int num2 = this.NewLuma(500, true);
					return this.ColorFromHLS(this.hue, num + (int)((float)(num2 - num) * percLighter), this.saturation);
				}
				if (percLighter == 0f)
				{
					return SystemColors.ControlLight;
				}
				if (percLighter == 1f)
				{
					return SystemColors.ControlLightLight;
				}
				Color controlLight = SystemColors.ControlLight;
				Color controlLightLight = SystemColors.ControlLightLight;
				int num3 = (int)(controlLight.R - controlLightLight.R);
				int num4 = (int)(controlLight.G - controlLightLight.G);
				int num5 = (int)(controlLight.B - controlLightLight.B);
				return Color.FromArgb((int)(controlLight.R - (byte)((float)num3 * percLighter)), (int)(controlLight.G - (byte)((float)num4 * percLighter)), (int)(controlLight.B - (byte)((float)num5 * percLighter)));
			}

			// Token: 0x0600264B RID: 9803 RVA: 0x0005D44E File Offset: 0x0005C44E
			private int NewLuma(int n, bool scale)
			{
				return this.NewLuma(this.luminosity, n, scale);
			}

			// Token: 0x0600264C RID: 9804 RVA: 0x0005D460 File Offset: 0x0005C460
			private int NewLuma(int luminosity, int n, bool scale)
			{
				if (n == 0)
				{
					return luminosity;
				}
				if (!scale)
				{
					int num = luminosity + (int)((long)n * 240L / 1000L);
					if (num < 0)
					{
						num = 0;
					}
					if (num > 240)
					{
						num = 240;
					}
					return num;
				}
				if (n > 0)
				{
					return (int)(((long)(luminosity * (1000 - n)) + 241L * (long)n) / 1000L);
				}
				return luminosity * (n + 1000) / 1000;
			}

			// Token: 0x0600264D RID: 9805 RVA: 0x0005D4D4 File Offset: 0x0005C4D4
			private Color ColorFromHLS(int hue, int luminosity, int saturation)
			{
				byte b3;
				byte b2;
				byte b;
				if (saturation == 0)
				{
					b = (b2 = (b3 = (byte)(luminosity * 255 / 240)));
					if (hue != 160)
					{
					}
				}
				else
				{
					int num;
					if (luminosity <= 120)
					{
						num = (luminosity * (240 + saturation) + 120) / 240;
					}
					else
					{
						num = luminosity + saturation - (luminosity * saturation + 120) / 240;
					}
					int num2 = 2 * luminosity - num;
					b2 = (byte)((this.HueToRGB(num2, num, hue + 80) * 255 + 120) / 240);
					b = (byte)((this.HueToRGB(num2, num, hue) * 255 + 120) / 240);
					b3 = (byte)((this.HueToRGB(num2, num, hue - 80) * 255 + 120) / 240);
				}
				return Color.FromArgb((int)b2, (int)b, (int)b3);
			}

			// Token: 0x0600264E RID: 9806 RVA: 0x0005D59C File Offset: 0x0005C59C
			private int HueToRGB(int n1, int n2, int hue)
			{
				if (hue < 0)
				{
					hue += 240;
				}
				if (hue > 240)
				{
					hue -= 240;
				}
				if (hue < 40)
				{
					return n1 + ((n2 - n1) * hue + 20) / 40;
				}
				if (hue < 120)
				{
					return n2;
				}
				if (hue < 160)
				{
					return n1 + ((n2 - n1) * (160 - hue) + 20) / 40;
				}
				return n1;
			}

			// Token: 0x0400161D RID: 5661
			private const int ShadowAdj = -333;

			// Token: 0x0400161E RID: 5662
			private const int HilightAdj = 500;

			// Token: 0x0400161F RID: 5663
			private const int WatermarkAdj = -50;

			// Token: 0x04001620 RID: 5664
			private const int Range = 240;

			// Token: 0x04001621 RID: 5665
			private const int HLSMax = 240;

			// Token: 0x04001622 RID: 5666
			private const int RGBMax = 255;

			// Token: 0x04001623 RID: 5667
			private const int Undefined = 160;

			// Token: 0x04001624 RID: 5668
			private int hue;

			// Token: 0x04001625 RID: 5669
			private int saturation;

			// Token: 0x04001626 RID: 5670
			private int luminosity;

			// Token: 0x04001627 RID: 5671
			private bool isSystemColors_Control;
		}
	}
}
