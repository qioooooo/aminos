using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Drawing.Design
{
	// Token: 0x02000008 RID: 8
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ColorEditor : UITypeEditor
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002648 File Offset: 0x00001648
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.colorUI == null)
					{
						this.colorUI = new ColorEditor.ColorUI(this);
					}
					this.colorUI.Start(windowsFormsEditorService, value);
					windowsFormsEditorService.DropDownControl(this.colorUI);
					if (this.colorUI.Value != null && (Color)this.colorUI.Value != Color.Empty)
					{
						value = this.colorUI.Value;
					}
					this.colorUI.End();
				}
			}
			return value;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026E3 File Offset: 0x000016E3
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026E6 File Offset: 0x000016E6
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026EC File Offset: 0x000016EC
		public override void PaintValue(PaintValueEventArgs e)
		{
			if (e.Value is Color)
			{
				Color color = (Color)e.Value;
				SolidBrush solidBrush = new SolidBrush(color);
				e.Graphics.FillRectangle(solidBrush, e.Bounds);
				solidBrush.Dispose();
			}
		}

		// Token: 0x04000041 RID: 65
		private ColorEditor.ColorUI colorUI;

		// Token: 0x02000009 RID: 9
		private class ColorPalette : Control
		{
			// Token: 0x06000022 RID: 34 RVA: 0x0000273C File Offset: 0x0000173C
			public ColorPalette(ColorEditor.ColorUI colorUI, Color[] customColors)
			{
				this.colorUI = colorUI;
				base.SetStyle(ControlStyles.Opaque, true);
				this.BackColor = SystemColors.Control;
				base.Size = new Size(202, 202);
				this.staticColors = new Color[48];
				for (int i = 0; i < ColorEditor.ColorPalette.staticCells.Length; i++)
				{
					this.staticColors[i] = ColorTranslator.FromOle(ColorEditor.ColorPalette.staticCells[i]);
				}
				this.customColors = customColors;
			}

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x06000023 RID: 35 RVA: 0x000027CE File Offset: 0x000017CE
			public Color[] CustomColors
			{
				get
				{
					return this.customColors;
				}
			}

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000024 RID: 36 RVA: 0x000027D6 File Offset: 0x000017D6
			internal int FocusedCell
			{
				get
				{
					return ColorEditor.ColorPalette.Get1DFrom2D(this.focus);
				}
			}

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000025 RID: 37 RVA: 0x000027E3 File Offset: 0x000017E3
			// (set) Token: 0x06000026 RID: 38 RVA: 0x000027EB File Offset: 0x000017EB
			public Color SelectedColor
			{
				get
				{
					return this.selectedColor;
				}
				set
				{
					if (!value.Equals(this.selectedColor))
					{
						this.InvalidateSelection();
						this.selectedColor = value;
						this.SetFocus(this.GetCellFromColor(value));
						this.InvalidateSelection();
					}
				}
			}

			// Token: 0x14000001 RID: 1
			// (add) Token: 0x06000027 RID: 39 RVA: 0x00002827 File Offset: 0x00001827
			// (remove) Token: 0x06000028 RID: 40 RVA: 0x00002840 File Offset: 0x00001840
			public event EventHandler Picked
			{
				add
				{
					this.onPicked = (EventHandler)Delegate.Combine(this.onPicked, value);
				}
				remove
				{
					this.onPicked = (EventHandler)Delegate.Remove(this.onPicked, value);
				}
			}

			// Token: 0x06000029 RID: 41 RVA: 0x00002859 File Offset: 0x00001859
			protected override AccessibleObject CreateAccessibilityInstance()
			{
				return new ColorEditor.ColorPalette.ColorPaletteAccessibleObject(this);
			}

			// Token: 0x0600002A RID: 42 RVA: 0x00002861 File Offset: 0x00001861
			protected void OnPicked(EventArgs e)
			{
				if (this.onPicked != null)
				{
					this.onPicked(this, e);
				}
			}

			// Token: 0x0600002B RID: 43 RVA: 0x00002878 File Offset: 0x00001878
			private static void FillRectWithCellBounds(int across, int down, ref Rectangle rect)
			{
				rect.X = 8 + across * 24;
				rect.Y = 8 + down * 24;
				rect.Width = 16;
				rect.Height = 16;
			}

			// Token: 0x0600002C RID: 44 RVA: 0x000028A4 File Offset: 0x000018A4
			private Point GetCellFromColor(Color c)
			{
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						if (this.GetColorFromCell(j, i).Equals(c))
						{
							return new Point(j, i);
						}
					}
				}
				return Point.Empty;
			}

			// Token: 0x0600002D RID: 45 RVA: 0x000028F4 File Offset: 0x000018F4
			private Color GetColorFromCell(int across, int down)
			{
				return this.GetColorFromCell(ColorEditor.ColorPalette.Get1DFrom2D(across, down));
			}

			// Token: 0x0600002E RID: 46 RVA: 0x00002903 File Offset: 0x00001903
			private Color GetColorFromCell(int index)
			{
				if (index < 48)
				{
					return this.staticColors[index];
				}
				return this.customColors[index - 64 + 16];
			}

			// Token: 0x0600002F RID: 47 RVA: 0x00002934 File Offset: 0x00001934
			private static Point GetCell2DFromLocationMouse(int x, int y)
			{
				int num = x / 24;
				int num2 = y / 24;
				if (num < 0 || num2 < 0 || num >= 8 || num2 >= 8)
				{
					return new Point(-1, -1);
				}
				if (x - 24 * num < 8 || y - 24 * num2 < 8)
				{
					return new Point(-1, -1);
				}
				return new Point(num, num2);
			}

			// Token: 0x06000030 RID: 48 RVA: 0x00002984 File Offset: 0x00001984
			private static int GetCellFromLocationMouse(int x, int y)
			{
				return ColorEditor.ColorPalette.Get1DFrom2D(ColorEditor.ColorPalette.GetCell2DFromLocationMouse(x, y));
			}

			// Token: 0x06000031 RID: 49 RVA: 0x00002992 File Offset: 0x00001992
			private static int Get1DFrom2D(Point pt)
			{
				return ColorEditor.ColorPalette.Get1DFrom2D(pt.X, pt.Y);
			}

			// Token: 0x06000032 RID: 50 RVA: 0x000029A7 File Offset: 0x000019A7
			private static int Get1DFrom2D(int x, int y)
			{
				if (x == -1 || y == -1)
				{
					return -1;
				}
				return x + 8 * y;
			}

			// Token: 0x06000033 RID: 51 RVA: 0x000029B8 File Offset: 0x000019B8
			internal static Point Get2DFrom1D(int cell)
			{
				int num = cell % 8;
				int num2 = cell / 8;
				return new Point(num, num2);
			}

			// Token: 0x06000034 RID: 52 RVA: 0x000029D4 File Offset: 0x000019D4
			private void InvalidateSelection()
			{
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						if (this.SelectedColor.Equals(this.GetColorFromCell(j, i)))
						{
							Rectangle rectangle = default(Rectangle);
							ColorEditor.ColorPalette.FillRectWithCellBounds(j, i, ref rectangle);
							base.Invalidate(Rectangle.Inflate(rectangle, 5, 5));
							break;
						}
					}
				}
			}

			// Token: 0x06000035 RID: 53 RVA: 0x00002A40 File Offset: 0x00001A40
			private void InvalidateFocus()
			{
				Rectangle rectangle = default(Rectangle);
				ColorEditor.ColorPalette.FillRectWithCellBounds(this.focus.X, this.focus.Y, ref rectangle);
				base.Invalidate(Rectangle.Inflate(rectangle, 5, 5));
				UnsafeNativeMethods.NotifyWinEvent(32773, new HandleRef(this, base.Handle), -4, 1 + ColorEditor.ColorPalette.Get1DFrom2D(this.focus.X, this.focus.Y));
			}

			// Token: 0x06000036 RID: 54 RVA: 0x00002AB8 File Offset: 0x00001AB8
			protected override bool IsInputKey(Keys keyData)
			{
				if (keyData != Keys.Return)
				{
					switch (keyData)
					{
					case Keys.Left:
					case Keys.Up:
					case Keys.Right:
					case Keys.Down:
						break;
					default:
						return keyData != Keys.F2 && base.IsInputKey(keyData);
					}
				}
				return true;
			}

			// Token: 0x06000037 RID: 55 RVA: 0x00002AF8 File Offset: 0x00001AF8
			protected virtual void LaunchDialog(int customIndex)
			{
				base.Invalidate();
				this.colorUI.EditorService.CloseDropDown();
				ColorEditor.CustomColorDialog customColorDialog = new ColorEditor.CustomColorDialog();
				IntPtr intPtr = UnsafeNativeMethods.GetFocus();
				try
				{
					DialogResult dialogResult = customColorDialog.ShowDialog();
					if (dialogResult != DialogResult.Cancel)
					{
						Color color = customColorDialog.Color;
						this.customColors[customIndex] = customColorDialog.Color;
						this.SelectedColor = this.customColors[customIndex];
						this.OnPicked(EventArgs.Empty);
					}
					customColorDialog.Dispose();
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						UnsafeNativeMethods.SetFocus(new HandleRef(null, intPtr));
					}
				}
			}

			// Token: 0x06000038 RID: 56 RVA: 0x00002BA8 File Offset: 0x00001BA8
			protected override void OnGotFocus(EventArgs e)
			{
				base.OnGotFocus(e);
				this.InvalidateFocus();
			}

			// Token: 0x06000039 RID: 57 RVA: 0x00002BB8 File Offset: 0x00001BB8
			protected override void OnKeyDown(KeyEventArgs e)
			{
				base.OnKeyDown(e);
				Keys keyCode = e.KeyCode;
				if (keyCode != Keys.Return)
				{
					switch (keyCode)
					{
					case Keys.Space:
						this.SelectedColor = this.GetColorFromCell(this.focus.X, this.focus.Y);
						this.InvalidateFocus();
						return;
					case Keys.Prior:
					case Keys.Next:
					case Keys.End:
					case Keys.Home:
						break;
					case Keys.Left:
						this.SetFocus(new Point(this.focus.X - 1, this.focus.Y));
						return;
					case Keys.Up:
						this.SetFocus(new Point(this.focus.X, this.focus.Y - 1));
						return;
					case Keys.Right:
						this.SetFocus(new Point(this.focus.X + 1, this.focus.Y));
						return;
					case Keys.Down:
						this.SetFocus(new Point(this.focus.X, this.focus.Y + 1));
						break;
					default:
						return;
					}
					return;
				}
				this.SelectedColor = this.GetColorFromCell(this.focus.X, this.focus.Y);
				this.InvalidateFocus();
				this.OnPicked(EventArgs.Empty);
			}

			// Token: 0x0600003A RID: 58 RVA: 0x00002CF2 File Offset: 0x00001CF2
			protected override void OnLostFocus(EventArgs e)
			{
				base.OnLostFocus(e);
				this.InvalidateFocus();
			}

			// Token: 0x0600003B RID: 59 RVA: 0x00002D04 File Offset: 0x00001D04
			protected override void OnMouseDown(MouseEventArgs me)
			{
				base.OnMouseDown(me);
				if (me.Button == MouseButtons.Left)
				{
					Point cell2DFromLocationMouse = ColorEditor.ColorPalette.GetCell2DFromLocationMouse(me.X, me.Y);
					if (cell2DFromLocationMouse.X != -1 && cell2DFromLocationMouse.Y != -1 && cell2DFromLocationMouse != this.focus)
					{
						this.SetFocus(cell2DFromLocationMouse);
					}
				}
			}

			// Token: 0x0600003C RID: 60 RVA: 0x00002D60 File Offset: 0x00001D60
			protected override void OnMouseMove(MouseEventArgs me)
			{
				base.OnMouseMove(me);
				if (me.Button == MouseButtons.Left && base.Bounds.Contains(me.X, me.Y))
				{
					Point cell2DFromLocationMouse = ColorEditor.ColorPalette.GetCell2DFromLocationMouse(me.X, me.Y);
					if (cell2DFromLocationMouse.X != -1 && cell2DFromLocationMouse.Y != -1 && cell2DFromLocationMouse != this.focus)
					{
						this.SetFocus(cell2DFromLocationMouse);
					}
				}
			}

			// Token: 0x0600003D RID: 61 RVA: 0x00002DD8 File Offset: 0x00001DD8
			protected override void OnMouseUp(MouseEventArgs me)
			{
				base.OnMouseUp(me);
				if (me.Button == MouseButtons.Left)
				{
					Point cell2DFromLocationMouse = ColorEditor.ColorPalette.GetCell2DFromLocationMouse(me.X, me.Y);
					if (cell2DFromLocationMouse.X != -1 && cell2DFromLocationMouse.Y != -1)
					{
						this.SetFocus(cell2DFromLocationMouse);
						this.SelectedColor = this.GetColorFromCell(this.focus.X, this.focus.Y);
						this.OnPicked(EventArgs.Empty);
						return;
					}
				}
				else if (me.Button == MouseButtons.Right)
				{
					int cellFromLocationMouse = ColorEditor.ColorPalette.GetCellFromLocationMouse(me.X, me.Y);
					if (cellFromLocationMouse != -1 && cellFromLocationMouse >= 48 && cellFromLocationMouse < 64)
					{
						this.LaunchDialog(cellFromLocationMouse - 64 + 16);
					}
				}
			}

			// Token: 0x0600003E RID: 62 RVA: 0x00002E90 File Offset: 0x00001E90
			protected override void OnPaint(PaintEventArgs pe)
			{
				Graphics graphics = pe.Graphics;
				graphics.FillRectangle(new SolidBrush(this.BackColor), base.ClientRectangle);
				Rectangle rectangle = default(Rectangle);
				rectangle.Width = 16;
				rectangle.Height = 16;
				rectangle.X = 8;
				rectangle.Y = 8;
				bool flag = false;
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						Color colorFromCell = this.GetColorFromCell(ColorEditor.ColorPalette.Get1DFrom2D(j, i));
						ColorEditor.ColorPalette.FillRectWithCellBounds(j, i, ref rectangle);
						if (colorFromCell.Equals(this.SelectedColor) && !flag)
						{
							ControlPaint.DrawBorder(graphics, Rectangle.Inflate(rectangle, 3, 3), SystemColors.ControlText, ButtonBorderStyle.Solid);
							flag = true;
						}
						if (this.focus.X == j && this.focus.Y == i && this.Focused)
						{
							ControlPaint.DrawFocusRectangle(graphics, Rectangle.Inflate(rectangle, 5, 5), SystemColors.ControlText, SystemColors.Control);
						}
						ControlPaint.DrawBorder(graphics, Rectangle.Inflate(rectangle, 2, 2), SystemColors.Control, 2, ButtonBorderStyle.Inset, SystemColors.Control, 2, ButtonBorderStyle.Inset, SystemColors.Control, 2, ButtonBorderStyle.Inset, SystemColors.Control, 2, ButtonBorderStyle.Inset);
						ColorEditor.ColorPalette.PaintValue(colorFromCell, graphics, rectangle);
					}
				}
			}

			// Token: 0x0600003F RID: 63 RVA: 0x00002FCC File Offset: 0x00001FCC
			private static void PaintValue(Color color, Graphics g, Rectangle rect)
			{
				g.FillRectangle(new SolidBrush(color), rect);
			}

			// Token: 0x06000040 RID: 64 RVA: 0x00002FDC File Offset: 0x00001FDC
			protected override bool ProcessDialogKey(Keys keyData)
			{
				if (keyData == Keys.F2)
				{
					int num = ColorEditor.ColorPalette.Get1DFrom2D(this.focus.X, this.focus.Y);
					if (num >= 48 && num < 64)
					{
						this.LaunchDialog(num - 64 + 16);
						return true;
					}
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x06000041 RID: 65 RVA: 0x0000302C File Offset: 0x0000202C
			private void SetFocus(Point newFocus)
			{
				if (newFocus.X < 0)
				{
					newFocus.X = 0;
				}
				if (newFocus.Y < 0)
				{
					newFocus.Y = 0;
				}
				if (newFocus.X >= 8)
				{
					newFocus.X = 7;
				}
				if (newFocus.Y >= 8)
				{
					newFocus.Y = 7;
				}
				if (this.focus != newFocus)
				{
					this.InvalidateFocus();
					this.focus = newFocus;
					this.InvalidateFocus();
				}
			}

			// Token: 0x04000042 RID: 66
			public const int CELLS_ACROSS = 8;

			// Token: 0x04000043 RID: 67
			public const int CELLS_DOWN = 8;

			// Token: 0x04000044 RID: 68
			public const int CELLS_CUSTOM = 16;

			// Token: 0x04000045 RID: 69
			public const int CELLS = 64;

			// Token: 0x04000046 RID: 70
			public const int CELL_SIZE = 16;

			// Token: 0x04000047 RID: 71
			public const int MARGIN = 8;

			// Token: 0x04000048 RID: 72
			private static readonly int[] staticCells = new int[]
			{
				16777215, 12632319, 12640511, 12648447, 12648384, 16777152, 16761024, 16761087, 14737632, 8421631,
				8438015, 8454143, 8454016, 16777088, 16744576, 16744703, 12632256, 255, 33023, 65535,
				65280, 16776960, 16711680, 16711935, 8421504, 192, 16576, 49344, 49152, 12632064,
				12582912, 12583104, 4210752, 128, 16512, 32896, 32768, 8421376, 8388608, 8388736,
				0, 64, 4210816, 16448, 16384, 4210688, 4194304, 4194368
			};

			// Token: 0x04000049 RID: 73
			private Color[] staticColors;

			// Token: 0x0400004A RID: 74
			private Color selectedColor;

			// Token: 0x0400004B RID: 75
			private Point focus = new Point(0, 0);

			// Token: 0x0400004C RID: 76
			private Color[] customColors;

			// Token: 0x0400004D RID: 77
			private EventHandler onPicked;

			// Token: 0x0400004E RID: 78
			private ColorEditor.ColorUI colorUI;

			// Token: 0x0200000A RID: 10
			[ComVisible(true)]
			public class ColorPaletteAccessibleObject : Control.ControlAccessibleObject
			{
				// Token: 0x06000043 RID: 67 RVA: 0x00003181 File Offset: 0x00002181
				public ColorPaletteAccessibleObject(ColorEditor.ColorPalette owner)
					: base(owner)
				{
					this.cells = new ColorEditor.ColorPalette.ColorPaletteAccessibleObject.ColorCellAccessibleObject[64];
				}

				// Token: 0x17000008 RID: 8
				// (get) Token: 0x06000044 RID: 68 RVA: 0x00003197 File Offset: 0x00002197
				internal ColorEditor.ColorPalette ColorPalette
				{
					get
					{
						return (ColorEditor.ColorPalette)base.Owner;
					}
				}

				// Token: 0x06000045 RID: 69 RVA: 0x000031A4 File Offset: 0x000021A4
				public override int GetChildCount()
				{
					return 64;
				}

				// Token: 0x06000046 RID: 70 RVA: 0x000031A8 File Offset: 0x000021A8
				public override AccessibleObject GetChild(int id)
				{
					if (id < 0 || id >= 64)
					{
						return null;
					}
					if (this.cells[id] == null)
					{
						this.cells[id] = new ColorEditor.ColorPalette.ColorPaletteAccessibleObject.ColorCellAccessibleObject(this, this.ColorPalette.GetColorFromCell(id), id);
					}
					return this.cells[id];
				}

				// Token: 0x06000047 RID: 71 RVA: 0x000031E4 File Offset: 0x000021E4
				public override AccessibleObject HitTest(int x, int y)
				{
					NativeMethods.POINT point = new NativeMethods.POINT(x, y);
					UnsafeNativeMethods.ScreenToClient(new HandleRef(this.ColorPalette, this.ColorPalette.Handle), point);
					int cellFromLocationMouse = ColorEditor.ColorPalette.GetCellFromLocationMouse(point.x, point.y);
					if (cellFromLocationMouse != -1)
					{
						return this.GetChild(cellFromLocationMouse);
					}
					return base.HitTest(x, y);
				}

				// Token: 0x0400004F RID: 79
				private ColorEditor.ColorPalette.ColorPaletteAccessibleObject.ColorCellAccessibleObject[] cells;

				// Token: 0x0200000B RID: 11
				[ComVisible(true)]
				public class ColorCellAccessibleObject : AccessibleObject
				{
					// Token: 0x06000048 RID: 72 RVA: 0x0000323C File Offset: 0x0000223C
					public ColorCellAccessibleObject(ColorEditor.ColorPalette.ColorPaletteAccessibleObject parent, Color color, int cell)
					{
						this.color = color;
						this.parent = parent;
						this.cell = cell;
					}

					// Token: 0x17000009 RID: 9
					// (get) Token: 0x06000049 RID: 73 RVA: 0x0000325C File Offset: 0x0000225C
					public override Rectangle Bounds
					{
						get
						{
							Point point = ColorEditor.ColorPalette.Get2DFrom1D(this.cell);
							Rectangle rectangle = default(Rectangle);
							ColorEditor.ColorPalette.FillRectWithCellBounds(point.X, point.Y, ref rectangle);
							NativeMethods.POINT point2 = new NativeMethods.POINT(rectangle.X, rectangle.Y);
							UnsafeNativeMethods.ClientToScreen(new HandleRef(this.parent.ColorPalette, this.parent.ColorPalette.Handle), point2);
							return new Rectangle(point2.x, point2.y, rectangle.Width, rectangle.Height);
						}
					}

					// Token: 0x1700000A RID: 10
					// (get) Token: 0x0600004A RID: 74 RVA: 0x000032EC File Offset: 0x000022EC
					public override string Name
					{
						get
						{
							return this.color.ToString();
						}
					}

					// Token: 0x1700000B RID: 11
					// (get) Token: 0x0600004B RID: 75 RVA: 0x000032FF File Offset: 0x000022FF
					public override AccessibleObject Parent
					{
						get
						{
							return this.parent;
						}
					}

					// Token: 0x1700000C RID: 12
					// (get) Token: 0x0600004C RID: 76 RVA: 0x00003307 File Offset: 0x00002307
					public override AccessibleRole Role
					{
						get
						{
							return AccessibleRole.Cell;
						}
					}

					// Token: 0x1700000D RID: 13
					// (get) Token: 0x0600004D RID: 77 RVA: 0x0000330C File Offset: 0x0000230C
					public override AccessibleStates State
					{
						get
						{
							AccessibleStates accessibleStates = base.State;
							if (this.cell == this.parent.ColorPalette.FocusedCell)
							{
								accessibleStates |= AccessibleStates.Focused;
							}
							return accessibleStates;
						}
					}

					// Token: 0x1700000E RID: 14
					// (get) Token: 0x0600004E RID: 78 RVA: 0x0000333D File Offset: 0x0000233D
					public override string Value
					{
						get
						{
							return this.color.ToString();
						}
					}

					// Token: 0x04000050 RID: 80
					private Color color;

					// Token: 0x04000051 RID: 81
					private ColorEditor.ColorPalette.ColorPaletteAccessibleObject parent;

					// Token: 0x04000052 RID: 82
					private int cell;
				}
			}
		}

		// Token: 0x0200000C RID: 12
		private class ColorUI : Control
		{
			// Token: 0x0600004F RID: 79 RVA: 0x00003350 File Offset: 0x00002350
			public ColorUI(ColorEditor editor)
			{
				this.editor = editor;
				this.InitializeComponent();
				this.AdjustListBoxItemHeight();
			}

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x06000050 RID: 80 RVA: 0x0000336B File Offset: 0x0000236B
			private object[] ColorValues
			{
				get
				{
					if (this.colorConstants == null)
					{
						this.colorConstants = ColorEditor.ColorUI.GetConstants(typeof(Color));
					}
					return this.colorConstants;
				}
			}

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x06000051 RID: 81 RVA: 0x00003390 File Offset: 0x00002390
			// (set) Token: 0x06000052 RID: 82 RVA: 0x000033DB File Offset: 0x000023DB
			private Color[] CustomColors
			{
				get
				{
					if (this.customColors == null)
					{
						this.customColors = new Color[16];
						for (int i = 0; i < 16; i++)
						{
							this.customColors[i] = Color.White;
						}
					}
					return this.customColors;
				}
				set
				{
					this.customColors = value;
					this.pal = null;
				}
			}

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x06000053 RID: 83 RVA: 0x000033EB File Offset: 0x000023EB
			public IWindowsFormsEditorService EditorService
			{
				get
				{
					return this.edSvc;
				}
			}

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x06000054 RID: 84 RVA: 0x000033F3 File Offset: 0x000023F3
			private object[] SystemColorValues
			{
				get
				{
					if (this.systemColorConstants == null)
					{
						this.systemColorConstants = ColorEditor.ColorUI.GetConstants(typeof(SystemColors));
					}
					return this.systemColorConstants;
				}
			}

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x06000055 RID: 85 RVA: 0x00003418 File Offset: 0x00002418
			public object Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x06000056 RID: 86 RVA: 0x00003420 File Offset: 0x00002420
			public void End()
			{
				this.edSvc = null;
				this.value = null;
			}

			// Token: 0x06000057 RID: 87 RVA: 0x00003430 File Offset: 0x00002430
			private void AdjustColorUIHeight()
			{
				Size size = this.pal.Size;
				Rectangle tabRect = this.tabControl.GetTabRect(0);
				int num = 0;
				base.Size = new Size(size.Width + 2 * num, size.Height + 2 * num + tabRect.Height);
				this.tabControl.Size = base.Size;
			}

			// Token: 0x06000058 RID: 88 RVA: 0x00003492 File Offset: 0x00002492
			private void AdjustListBoxItemHeight()
			{
				this.lbSystem.ItemHeight = this.Font.Height + 2;
				this.lbCommon.ItemHeight = this.Font.Height + 2;
			}

			// Token: 0x06000059 RID: 89 RVA: 0x000034C4 File Offset: 0x000024C4
			private Color GetBestColor(Color color)
			{
				object[] colorValues = this.ColorValues;
				int num = color.ToArgb();
				for (int i = 0; i < colorValues.Length; i++)
				{
					if (((Color)colorValues[i]).ToArgb() == num)
					{
						return (Color)colorValues[i];
					}
				}
				return color;
			}

			// Token: 0x0600005A RID: 90 RVA: 0x0000350C File Offset: 0x0000250C
			private static object[] GetConstants(Type enumType)
			{
				MethodAttributes methodAttributes = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static;
				PropertyInfo[] properties = enumType.GetProperties();
				ArrayList arrayList = new ArrayList();
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (propertyInfo.PropertyType == typeof(Color))
					{
						MethodInfo getMethod = propertyInfo.GetGetMethod();
						if (getMethod != null && (getMethod.Attributes & methodAttributes) == methodAttributes)
						{
							object[] array = null;
							arrayList.Add(propertyInfo.GetValue(null, array));
						}
					}
				}
				return arrayList.ToArray();
			}

			// Token: 0x0600005B RID: 91 RVA: 0x00003584 File Offset: 0x00002584
			private void InitializeComponent()
			{
				this.paletteTabPage = new TabPage(SR.GetString("ColorEditorPaletteTab"));
				this.commonTabPage = new TabPage(SR.GetString("ColorEditorStandardTab"));
				this.systemTabPage = new TabPage(SR.GetString("ColorEditorSystemTab"));
				base.AccessibleName = SR.GetString("ColorEditorAccName");
				this.tabControl = new ColorEditor.ColorUI.ColorEditorTabControl();
				this.tabControl.TabPages.Add(this.paletteTabPage);
				this.tabControl.TabPages.Add(this.commonTabPage);
				this.tabControl.TabPages.Add(this.systemTabPage);
				this.tabControl.TabStop = false;
				this.tabControl.SelectedTab = this.systemTabPage;
				this.tabControl.SelectedIndexChanged += this.OnTabControlSelChange;
				this.tabControl.Dock = DockStyle.Fill;
				this.tabControl.Resize += this.OnTabControlResize;
				this.lbSystem = new ColorEditor.ColorUI.ColorEditorListBox();
				this.lbSystem.DrawMode = DrawMode.OwnerDrawFixed;
				this.lbSystem.BorderStyle = BorderStyle.FixedSingle;
				this.lbSystem.IntegralHeight = false;
				this.lbSystem.Sorted = false;
				this.lbSystem.Click += this.OnListClick;
				this.lbSystem.DrawItem += this.OnListDrawItem;
				this.lbSystem.KeyDown += this.OnListKeyDown;
				this.lbSystem.Dock = DockStyle.Fill;
				this.lbSystem.FontChanged += this.OnFontChanged;
				this.lbCommon = new ColorEditor.ColorUI.ColorEditorListBox();
				this.lbCommon.DrawMode = DrawMode.OwnerDrawFixed;
				this.lbCommon.BorderStyle = BorderStyle.FixedSingle;
				this.lbCommon.IntegralHeight = false;
				this.lbCommon.Sorted = false;
				this.lbCommon.Click += this.OnListClick;
				this.lbCommon.DrawItem += this.OnListDrawItem;
				this.lbCommon.KeyDown += this.OnListKeyDown;
				this.lbCommon.Dock = DockStyle.Fill;
				Array.Sort(this.ColorValues, new ColorEditor.StandardColorComparer());
				Array.Sort(this.SystemColorValues, new ColorEditor.SystemColorComparer());
				this.lbCommon.Items.Clear();
				foreach (object obj in this.ColorValues)
				{
					this.lbCommon.Items.Add(obj);
				}
				this.lbSystem.Items.Clear();
				foreach (object obj2 in this.SystemColorValues)
				{
					this.lbSystem.Items.Add(obj2);
				}
				this.pal = new ColorEditor.ColorPalette(this, this.CustomColors);
				this.pal.Picked += this.OnPalettePick;
				this.paletteTabPage.Controls.Add(this.pal);
				this.systemTabPage.Controls.Add(this.lbSystem);
				this.commonTabPage.Controls.Add(this.lbCommon);
				base.Controls.Add(this.tabControl);
			}

			// Token: 0x0600005C RID: 92 RVA: 0x000038CD File Offset: 0x000028CD
			protected override void OnGotFocus(EventArgs e)
			{
				base.OnGotFocus(e);
				this.OnTabControlSelChange(this, EventArgs.Empty);
			}

			// Token: 0x0600005D RID: 93 RVA: 0x000038E4 File Offset: 0x000028E4
			private void OnFontChanged(object sender, EventArgs e)
			{
				this.commonHeightSet = (this.systemHeightSet = false);
			}

			// Token: 0x0600005E RID: 94 RVA: 0x00003904 File Offset: 0x00002904
			private void OnListClick(object sender, EventArgs e)
			{
				ListBox listBox = (ListBox)sender;
				if (listBox.SelectedItem != null)
				{
					this.value = (Color)listBox.SelectedItem;
				}
				this.edSvc.CloseDropDown();
			}

			// Token: 0x0600005F RID: 95 RVA: 0x00003944 File Offset: 0x00002944
			private void OnListDrawItem(object sender, DrawItemEventArgs die)
			{
				ListBox listBox = (ListBox)sender;
				object obj = listBox.Items[die.Index];
				Font font = this.Font;
				if (listBox == this.lbCommon && !this.commonHeightSet)
				{
					listBox.ItemHeight = listBox.Font.Height;
					this.commonHeightSet = true;
				}
				else if (listBox == this.lbSystem && !this.systemHeightSet)
				{
					listBox.ItemHeight = listBox.Font.Height;
					this.systemHeightSet = true;
				}
				Graphics graphics = die.Graphics;
				die.DrawBackground();
				this.editor.PaintValue(obj, graphics, new Rectangle(die.Bounds.X + 2, die.Bounds.Y + 2, 22, die.Bounds.Height - 4));
				graphics.DrawRectangle(SystemPens.WindowText, new Rectangle(die.Bounds.X + 2, die.Bounds.Y + 2, 21, die.Bounds.Height - 4 - 1));
				Brush brush = new SolidBrush(die.ForeColor);
				graphics.DrawString(((Color)obj).Name, font, brush, (float)(die.Bounds.X + 26), (float)die.Bounds.Y);
				brush.Dispose();
			}

			// Token: 0x06000060 RID: 96 RVA: 0x00003AAF File Offset: 0x00002AAF
			private void OnListKeyDown(object sender, KeyEventArgs ke)
			{
				if (ke.KeyCode == Keys.Return)
				{
					this.OnListClick(sender, EventArgs.Empty);
				}
			}

			// Token: 0x06000061 RID: 97 RVA: 0x00003AC8 File Offset: 0x00002AC8
			private void OnPalettePick(object sender, EventArgs e)
			{
				ColorEditor.ColorPalette colorPalette = (ColorEditor.ColorPalette)sender;
				this.value = this.GetBestColor(colorPalette.SelectedColor);
				this.edSvc.CloseDropDown();
			}

			// Token: 0x06000062 RID: 98 RVA: 0x00003AFE File Offset: 0x00002AFE
			protected override void OnFontChanged(EventArgs e)
			{
				base.OnFontChanged(e);
				this.AdjustListBoxItemHeight();
				this.AdjustColorUIHeight();
			}

			// Token: 0x06000063 RID: 99 RVA: 0x00003B14 File Offset: 0x00002B14
			private void OnTabControlResize(object sender, EventArgs e)
			{
				Rectangle clientRectangle = this.tabControl.TabPages[0].ClientRectangle;
				Rectangle tabRect = this.tabControl.GetTabRect(1);
				clientRectangle.Y = 0;
				clientRectangle.Height -= clientRectangle.Y;
				int num = 2;
				this.lbSystem.SetBounds(num, clientRectangle.Y + 2 * num, clientRectangle.Width - num, this.pal.Size.Height - tabRect.Height + 2 * num);
				this.lbCommon.Bounds = this.lbSystem.Bounds;
				this.pal.Location = new Point(0, clientRectangle.Y);
			}

			// Token: 0x06000064 RID: 100 RVA: 0x00003BD4 File Offset: 0x00002BD4
			private void OnTabControlSelChange(object sender, EventArgs e)
			{
				TabPage selectedTab = this.tabControl.SelectedTab;
				if (selectedTab != null && selectedTab.Controls.Count > 0)
				{
					selectedTab.Controls[0].Focus();
				}
			}

			// Token: 0x06000065 RID: 101 RVA: 0x00003C10 File Offset: 0x00002C10
			protected override bool ProcessDialogKey(Keys keyData)
			{
				if ((keyData & Keys.Alt) == Keys.None && (keyData & Keys.Control) == Keys.None && (keyData & Keys.KeyCode) == Keys.Tab)
				{
					bool flag = (keyData & Keys.Shift) == Keys.None;
					int num = this.tabControl.SelectedIndex;
					if (num != -1)
					{
						int count = this.tabControl.TabPages.Count;
						if (flag)
						{
							num = (num + 1) % count;
						}
						else
						{
							num = (num + count - 1) % count;
						}
						this.tabControl.SelectedTab = this.tabControl.TabPages[num];
						return true;
					}
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x06000066 RID: 102 RVA: 0x00003CA0 File Offset: 0x00002CA0
			public void Start(IWindowsFormsEditorService edSvc, object value)
			{
				this.edSvc = edSvc;
				this.value = value;
				this.AdjustColorUIHeight();
				if (value != null)
				{
					object[] array = this.ColorValues;
					TabPage tabPage = this.paletteTabPage;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].Equals(value))
						{
							this.lbCommon.SelectedItem = value;
							tabPage = this.commonTabPage;
							break;
						}
					}
					if (tabPage == this.paletteTabPage)
					{
						array = this.SystemColorValues;
						for (int j = 0; j < array.Length; j++)
						{
							if (array[j].Equals(value))
							{
								this.lbSystem.SelectedItem = value;
								tabPage = this.systemTabPage;
								break;
							}
						}
					}
					this.tabControl.SelectedTab = tabPage;
				}
			}

			// Token: 0x04000053 RID: 83
			private ColorEditor editor;

			// Token: 0x04000054 RID: 84
			private IWindowsFormsEditorService edSvc;

			// Token: 0x04000055 RID: 85
			private object value;

			// Token: 0x04000056 RID: 86
			private ColorEditor.ColorUI.ColorEditorTabControl tabControl;

			// Token: 0x04000057 RID: 87
			private TabPage systemTabPage;

			// Token: 0x04000058 RID: 88
			private TabPage commonTabPage;

			// Token: 0x04000059 RID: 89
			private TabPage paletteTabPage;

			// Token: 0x0400005A RID: 90
			private ListBox lbSystem;

			// Token: 0x0400005B RID: 91
			private ListBox lbCommon;

			// Token: 0x0400005C RID: 92
			private ColorEditor.ColorPalette pal;

			// Token: 0x0400005D RID: 93
			private object[] systemColorConstants;

			// Token: 0x0400005E RID: 94
			private object[] colorConstants;

			// Token: 0x0400005F RID: 95
			private Color[] customColors;

			// Token: 0x04000060 RID: 96
			private bool commonHeightSet;

			// Token: 0x04000061 RID: 97
			private bool systemHeightSet;

			// Token: 0x0200000D RID: 13
			private class ColorEditorListBox : ListBox
			{
				// Token: 0x06000067 RID: 103 RVA: 0x00003D50 File Offset: 0x00002D50
				protected override bool IsInputKey(Keys keyData)
				{
					return keyData == Keys.Return || base.IsInputKey(keyData);
				}
			}

			// Token: 0x0200000E RID: 14
			private class ColorEditorTabControl : TabControl
			{
				// Token: 0x0600006A RID: 106 RVA: 0x00003D80 File Offset: 0x00002D80
				protected override void OnGotFocus(EventArgs e)
				{
					TabPage selectedTab = base.SelectedTab;
					if (selectedTab != null && selectedTab.Controls.Count > 0)
					{
						selectedTab.Controls[0].Focus();
					}
				}
			}
		}

		// Token: 0x0200000F RID: 15
		private class CustomColorDialog : ColorDialog
		{
			// Token: 0x0600006B RID: 107 RVA: 0x00003DB8 File Offset: 0x00002DB8
			public CustomColorDialog()
			{
				Stream manifestResourceStream = typeof(ColorEditor).Module.Assembly.GetManifestResourceStream(typeof(ColorEditor), "colordlg.data");
				int num = (int)(manifestResourceStream.Length - manifestResourceStream.Position);
				byte[] array = new byte[num];
				manifestResourceStream.Read(array, 0, num);
				this.hInstance = Marshal.AllocHGlobal(num);
				Marshal.Copy(array, 0, this.hInstance, num);
			}

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x0600006C RID: 108 RVA: 0x00003E2E File Offset: 0x00002E2E
			protected override IntPtr Instance
			{
				get
				{
					return this.hInstance;
				}
			}

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x0600006D RID: 109 RVA: 0x00003E36 File Offset: 0x00002E36
			protected override int Options
			{
				get
				{
					return 66;
				}
			}

			// Token: 0x0600006E RID: 110 RVA: 0x00003E3C File Offset: 0x00002E3C
			protected override void Dispose(bool disposing)
			{
				try
				{
					if (this.hInstance != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.hInstance);
						this.hInstance = IntPtr.Zero;
					}
				}
				finally
				{
					base.Dispose(disposing);
				}
			}

			// Token: 0x0600006F RID: 111 RVA: 0x00003E8C File Offset: 0x00002E8C
			protected override IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
			{
				switch (msg)
				{
				case 272:
				{
					NativeMethods.SendDlgItemMessage(hwnd, 703, 211, (IntPtr)3, IntPtr.Zero);
					NativeMethods.SendDlgItemMessage(hwnd, 704, 211, (IntPtr)3, IntPtr.Zero);
					NativeMethods.SendDlgItemMessage(hwnd, 705, 211, (IntPtr)3, IntPtr.Zero);
					NativeMethods.SendDlgItemMessage(hwnd, 706, 211, (IntPtr)3, IntPtr.Zero);
					NativeMethods.SendDlgItemMessage(hwnd, 707, 211, (IntPtr)3, IntPtr.Zero);
					NativeMethods.SendDlgItemMessage(hwnd, 708, 211, (IntPtr)3, IntPtr.Zero);
					IntPtr intPtr = NativeMethods.GetDlgItem(hwnd, 719);
					NativeMethods.EnableWindow(intPtr, false);
					NativeMethods.SetWindowPos(intPtr, IntPtr.Zero, 0, 0, 0, 0, 128);
					intPtr = NativeMethods.GetDlgItem(hwnd, 1);
					NativeMethods.EnableWindow(intPtr, false);
					NativeMethods.SetWindowPos(intPtr, IntPtr.Zero, 0, 0, 0, 0, 128);
					base.Color = Color.Empty;
					break;
				}
				case 273:
				{
					int num = NativeMethods.Util.LOWORD((int)wParam);
					if (num == 712)
					{
						bool[] array = new bool[1];
						byte b = (byte)NativeMethods.GetDlgItemInt(hwnd, 706, array, false);
						byte b2 = (byte)NativeMethods.GetDlgItemInt(hwnd, 707, array, false);
						byte b3 = (byte)NativeMethods.GetDlgItemInt(hwnd, 708, array, false);
						base.Color = Color.FromArgb((int)b, (int)b2, (int)b3);
						NativeMethods.PostMessage(hwnd, 273, (IntPtr)NativeMethods.Util.MAKELONG(1, 0), NativeMethods.GetDlgItem(hwnd, 1));
					}
					break;
				}
				}
				return base.HookProc(hwnd, msg, wParam, lParam);
			}

			// Token: 0x04000062 RID: 98
			private const int COLOR_HUE = 703;

			// Token: 0x04000063 RID: 99
			private const int COLOR_SAT = 704;

			// Token: 0x04000064 RID: 100
			private const int COLOR_LUM = 705;

			// Token: 0x04000065 RID: 101
			private const int COLOR_RED = 706;

			// Token: 0x04000066 RID: 102
			private const int COLOR_GREEN = 707;

			// Token: 0x04000067 RID: 103
			private const int COLOR_BLUE = 708;

			// Token: 0x04000068 RID: 104
			private const int COLOR_ADD = 712;

			// Token: 0x04000069 RID: 105
			private const int COLOR_MIX = 719;

			// Token: 0x0400006A RID: 106
			private IntPtr hInstance;
		}

		// Token: 0x02000010 RID: 16
		private class SystemColorComparer : IComparer
		{
			// Token: 0x06000070 RID: 112 RVA: 0x00004040 File Offset: 0x00003040
			public int Compare(object x, object y)
			{
				return string.Compare(((Color)x).Name, ((Color)y).Name, false, CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x02000011 RID: 17
		private class StandardColorComparer : IComparer
		{
			// Token: 0x06000072 RID: 114 RVA: 0x0000407C File Offset: 0x0000307C
			public int Compare(object x, object y)
			{
				Color color = (Color)x;
				Color color2 = (Color)y;
				if (color.A < color2.A)
				{
					return -1;
				}
				if (color.A > color2.A)
				{
					return 1;
				}
				if (color.GetHue() < color2.GetHue())
				{
					return -1;
				}
				if (color.GetHue() > color2.GetHue())
				{
					return 1;
				}
				if (color.GetSaturation() < color2.GetSaturation())
				{
					return -1;
				}
				if (color.GetSaturation() > color2.GetSaturation())
				{
					return 1;
				}
				if (color.GetBrightness() < color2.GetBrightness())
				{
					return -1;
				}
				if (color.GetBrightness() > color2.GetBrightness())
				{
					return 1;
				}
				return 0;
			}
		}
	}
}
