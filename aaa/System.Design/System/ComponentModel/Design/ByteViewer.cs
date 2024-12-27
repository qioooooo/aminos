using System;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace System.ComponentModel.Design
{
	// Token: 0x020000F6 RID: 246
	[DesignTimeVisible(false)]
	[ToolboxItem(false)]
	public class ByteViewer : TableLayoutPanel
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x00026D48 File Offset: 0x00025D48
		public ByteViewer()
		{
			base.SuspendLayout();
			base.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
			base.ColumnCount = 1;
			base.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			base.RowCount = 1;
			base.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
			this.InitUI();
			base.ResumeLayout();
			this.displayMode = DisplayMode.Hexdump;
			this.realDisplayMode = DisplayMode.Hexdump;
			this.DoubleBuffered = true;
			base.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00027084 File Offset: 0x00026084
		private static int AnalizeByteOrderMark(byte[] buffer, int index)
		{
			int num = ((int)buffer[index] << 8) | (int)buffer[index + 1];
			int num2 = ((int)buffer[index + 2] << 8) | (int)buffer[index + 3];
			int encodingIndex = ByteViewer.GetEncodingIndex(num);
			int encodingIndex2 = ByteViewer.GetEncodingIndex(num2);
			int[,] array = new int[,]
			{
				{
					1, 5, 1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1
				},
				{
					1, 1, 1, 11, 1, 10, 4, 1, 1, 1,
					1, 1, 1
				},
				{
					2, 9, 5, 2, 2, 2, 2, 2, 2, 2,
					2, 2, 2
				},
				{
					3, 7, 3, 7, 3, 3, 3, 3, 3, 3,
					3, 3, 3
				},
				{
					14, 1, 1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1
				},
				{
					1, 6, 1, 1, 1, 1, 1, 3, 1, 1,
					1, 1, 1
				},
				{
					1, 8, 1, 1, 1, 1, 1, 1, 2, 1,
					1, 1, 1
				},
				{
					1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1
				},
				{
					1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1
				},
				{
					1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
					13, 1, 1
				},
				{
					1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1
				},
				{
					1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 12
				},
				{
					1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1
				}
			};
			return array[encodingIndex, encodingIndex2];
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x000270D8 File Offset: 0x000260D8
		private int CellToIndex(int column, int row)
		{
			return row * this.columnCount + column;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x000270E4 File Offset: 0x000260E4
		private byte[] ComposeLineBuffer(int startLine, int line)
		{
			int num = startLine * this.columnCount;
			byte[] array;
			if (num + (line + 1) * this.columnCount > this.dataBuf.Length)
			{
				array = new byte[this.dataBuf.Length % this.columnCount];
			}
			else
			{
				array = new byte[this.columnCount];
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.dataBuf[num + this.CellToIndex(i, line)];
			}
			return array;
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00027158 File Offset: 0x00026158
		private void DrawAddress(Graphics g, int startLine, int line)
		{
			Font address_FONT = ByteViewer.ADDRESS_FONT;
			Brush control = SystemBrushes.Control;
			string text = ((startLine + line) * this.columnCount).ToString("X8", CultureInfo.InvariantCulture);
			using (Brush brush = new SolidBrush(this.ForeColor))
			{
				g.DrawString(text, address_FONT, brush, 5f, (float)(7 + line * 21));
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x000271CC File Offset: 0x000261CC
		private void DrawClient(Graphics g)
		{
			using (Brush brush = new SolidBrush(SystemColors.ControlLightLight))
			{
				g.FillRectangle(brush, new Rectangle(74, 5, 538, this.rowCount * 21));
			}
			using (Pen pen = new Pen(SystemColors.ControlDark))
			{
				g.DrawRectangle(pen, new Rectangle(74, 5, 537, this.rowCount * 21 - 1));
				g.DrawLine(pen, 474, 5, 474, 5 + this.rowCount * 21 - 1);
			}
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00027280 File Offset: 0x00026280
		private static bool CharIsPrintable(char c)
		{
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
			return unicodeCategory != UnicodeCategory.Control || unicodeCategory == UnicodeCategory.Format || unicodeCategory == UnicodeCategory.LineSeparator || unicodeCategory == UnicodeCategory.ParagraphSeparator || unicodeCategory == UnicodeCategory.OtherNotAssigned;
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x000272B0 File Offset: 0x000262B0
		private void DrawDump(Graphics g, byte[] lineBuffer, int line)
		{
			StringBuilder stringBuilder = new StringBuilder(lineBuffer.Length);
			for (int i = 0; i < lineBuffer.Length; i++)
			{
				char c = Convert.ToChar(lineBuffer[i]);
				if (ByteViewer.CharIsPrintable(c))
				{
					stringBuilder.Append(c);
				}
				else
				{
					stringBuilder.Append('.');
				}
			}
			Font hexdump_FONT = ByteViewer.HEXDUMP_FONT;
			Color window = SystemColors.Window;
			using (Brush brush = new SolidBrush(this.ForeColor))
			{
				g.DrawString(stringBuilder.ToString(), hexdump_FONT, brush, 479f, (float)(7 + line * 21));
			}
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0002734C File Offset: 0x0002634C
		private void DrawHex(Graphics g, byte[] lineBuffer, int line)
		{
			Color controlLightLight = SystemColors.ControlLightLight;
			Font hexdump_FONT = ByteViewer.HEXDUMP_FONT;
			StringBuilder stringBuilder = new StringBuilder(lineBuffer.Length * 3 + 1);
			for (int i = 0; i < lineBuffer.Length; i++)
			{
				stringBuilder.Append(lineBuffer[i].ToString("X2", CultureInfo.InvariantCulture));
				stringBuilder.Append(" ");
				if (i == this.columnCount / 2 - 1)
				{
					stringBuilder.Append(" ");
				}
			}
			using (Brush brush = new SolidBrush(this.ForeColor))
			{
				g.DrawString(stringBuilder.ToString(), hexdump_FONT, brush, 76f, (float)(7 + line * 21));
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00027404 File Offset: 0x00026404
		private void DrawLines(Graphics g, int startLine, int linesCount)
		{
			for (int i = 0; i < linesCount; i++)
			{
				byte[] array = this.ComposeLineBuffer(startLine, i);
				this.DrawAddress(g, startLine, i);
				this.DrawHex(g, array, i);
				this.DrawDump(g, array, i);
			}
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00027444 File Offset: 0x00026444
		private DisplayMode GetAutoDisplayMode()
		{
			int num = 0;
			int num2 = 0;
			if (this.dataBuf == null || (this.dataBuf.Length >= 0 && this.dataBuf.Length < 8))
			{
				return DisplayMode.Hexdump;
			}
			switch (ByteViewer.AnalizeByteOrderMark(this.dataBuf, 0))
			{
			case 2:
				return DisplayMode.Hexdump;
			case 3:
				return DisplayMode.Unicode;
			case 4:
			case 5:
				return DisplayMode.Hexdump;
			case 6:
			case 7:
				return DisplayMode.Hexdump;
			case 8:
			case 9:
				return DisplayMode.Hexdump;
			case 10:
			case 11:
				return DisplayMode.Hexdump;
			case 12:
				return DisplayMode.Hexdump;
			case 13:
				return DisplayMode.Ansi;
			case 14:
				return DisplayMode.Ansi;
			default:
			{
				int num3;
				if (this.dataBuf.Length > 1024)
				{
					num3 = 512;
				}
				else
				{
					num3 = this.dataBuf.Length / 2;
				}
				for (int i = 0; i < num3; i++)
				{
					char c = (char)this.dataBuf[i];
					if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
					{
						num++;
					}
				}
				for (int j = 0; j < num3; j += 2)
				{
					char[] array = new char[1];
					Encoding.Unicode.GetChars(this.dataBuf, j, 2, array, 0);
					if (ByteViewer.CharIsPrintable(array[0]))
					{
						num2++;
					}
				}
				if (num2 * 100 / (num3 / 2) > 80)
				{
					return DisplayMode.Unicode;
				}
				if (num * 100 / num3 > 80)
				{
					return DisplayMode.Ansi;
				}
				return DisplayMode.Hexdump;
			}
			}
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0002757B File Offset: 0x0002657B
		public virtual byte[] GetBytes()
		{
			return this.dataBuf;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00027583 File Offset: 0x00026583
		public virtual DisplayMode GetDisplayMode()
		{
			return this.displayMode;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0002758C File Offset: 0x0002658C
		private static int GetEncodingIndex(int c1)
		{
			if (c1 <= 16128)
			{
				if (c1 <= 63)
				{
					if (c1 == 0)
					{
						return 1;
					}
					if (c1 == 60)
					{
						return 6;
					}
					if (c1 == 63)
					{
						return 8;
					}
				}
				else
				{
					if (c1 == 15360)
					{
						return 5;
					}
					if (c1 == 15423)
					{
						return 9;
					}
					if (c1 == 16128)
					{
						return 7;
					}
				}
			}
			else if (c1 <= 42900)
			{
				if (c1 == 19567)
				{
					return 11;
				}
				if (c1 == 30829)
				{
					return 10;
				}
				if (c1 == 42900)
				{
					return 12;
				}
			}
			else
			{
				if (c1 == 61371)
				{
					return 4;
				}
				if (c1 == 65279)
				{
					return 2;
				}
				if (c1 == 65534)
				{
					return 3;
				}
			}
			return 0;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0002762C File Offset: 0x0002662C
		private void InitAnsi()
		{
			int num = this.dataBuf.Length;
			char[] array = new char[num + 1];
			num = NativeMethods.MultiByteToWideChar(0, 0, this.dataBuf, num, array, num);
			array[num] = '\0';
			for (int i = 0; i < num; i++)
			{
				if (array[i] == '\0')
				{
					array[i] = '\v';
				}
			}
			this.edit.Text = new string(array);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00027688 File Offset: 0x00026688
		private void InitUnicode()
		{
			char[] array = new char[this.dataBuf.Length / 2 + 1];
			Encoding.Unicode.GetChars(this.dataBuf, 0, this.dataBuf.Length, array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == '\0')
				{
					array[i] = '\v';
				}
			}
			array[array.Length - 1] = '\0';
			this.edit.Text = new string(array);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x000276F4 File Offset: 0x000266F4
		private void InitUI()
		{
			this.SCROLLBAR_HEIGHT = SystemInformation.HorizontalScrollBarHeight;
			this.SCROLLBAR_WIDTH = SystemInformation.VerticalScrollBarWidth;
			base.Size = new Size(612 + this.SCROLLBAR_WIDTH + 2 + 3, 10 + this.rowCount * 21);
			this.scrollBar = new VScrollBar();
			this.scrollBar.ValueChanged += this.ScrollChanged;
			this.scrollBar.TabStop = true;
			this.scrollBar.TabIndex = 0;
			this.scrollBar.Dock = DockStyle.Right;
			this.scrollBar.Visible = false;
			this.edit = new TextBox();
			this.edit.AutoSize = false;
			this.edit.BorderStyle = BorderStyle.None;
			this.edit.Multiline = true;
			this.edit.ReadOnly = true;
			this.edit.ScrollBars = ScrollBars.Both;
			this.edit.AcceptsTab = true;
			this.edit.AcceptsReturn = true;
			this.edit.Dock = DockStyle.Fill;
			this.edit.Margin = Padding.Empty;
			this.edit.WordWrap = false;
			this.edit.Visible = false;
			base.Controls.Add(this.scrollBar, 0, 0);
			base.Controls.Add(this.edit, 0, 0);
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0002784C File Offset: 0x0002684C
		private void InitState()
		{
			this.linesCount = (this.dataBuf.Length + this.columnCount - 1) / this.columnCount;
			this.startLine = 0;
			if (this.linesCount > this.rowCount)
			{
				this.displayLinesCount = this.rowCount;
				this.scrollBar.Hide();
				this.scrollBar.Maximum = this.linesCount - 1;
				this.scrollBar.LargeChange = this.rowCount;
				this.scrollBar.Show();
				this.scrollBar.Enabled = true;
			}
			else
			{
				this.displayLinesCount = this.linesCount;
				this.scrollBar.Hide();
				this.scrollBar.Maximum = this.rowCount;
				this.scrollBar.LargeChange = this.rowCount;
				this.scrollBar.Show();
				this.scrollBar.Enabled = false;
			}
			this.scrollBar.Select();
			base.Invalidate();
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00027941 File Offset: 0x00026941
		protected override void OnKeyDown(KeyEventArgs e)
		{
			this.scrollBar.Select();
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x00027950 File Offset: 0x00026950
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			switch (this.realDisplayMode)
			{
			case DisplayMode.Hexdump:
				base.SuspendLayout();
				this.edit.Hide();
				this.scrollBar.Show();
				base.ResumeLayout();
				this.DrawClient(graphics);
				this.DrawLines(graphics, this.startLine, this.displayLinesCount);
				return;
			case DisplayMode.Ansi:
				this.edit.Invalidate();
				return;
			case DisplayMode.Unicode:
				this.edit.Invalidate();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000279DC File Offset: 0x000269DC
		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);
			int num = (base.ClientSize.Height - 10) / 21;
			if (num >= 0 && num != this.rowCount)
			{
				this.rowCount = num;
				if (this.Dock == DockStyle.None)
				{
					base.Size = new Size(612 + this.SCROLLBAR_WIDTH + 2 + 3, 10 + this.rowCount * 21);
				}
				if (this.scrollBar != null)
				{
					if (this.linesCount > this.rowCount)
					{
						this.scrollBar.Hide();
						this.scrollBar.Maximum = this.linesCount - 1;
						this.scrollBar.LargeChange = this.rowCount;
						this.scrollBar.Show();
						this.scrollBar.Enabled = true;
						this.scrollBar.Select();
					}
					else
					{
						this.scrollBar.Enabled = false;
					}
				}
				this.displayLinesCount = ((this.startLine + this.rowCount < this.linesCount) ? this.rowCount : (this.linesCount - this.startLine));
				return;
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00027AF0 File Offset: 0x00026AF0
		public virtual void SaveToFile(string path)
		{
			if (this.dataBuf != null)
			{
				FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
				try
				{
					fileStream.Write(this.dataBuf, 0, this.dataBuf.Length);
					fileStream.Close();
				}
				catch
				{
					fileStream.Close();
					throw;
				}
			}
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00027B48 File Offset: 0x00026B48
		protected virtual void ScrollChanged(object source, EventArgs e)
		{
			this.startLine = this.scrollBar.Value;
			base.Invalidate();
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00027B61 File Offset: 0x00026B61
		public virtual void SetBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (this.dataBuf != null)
			{
				this.dataBuf = null;
			}
			this.dataBuf = bytes;
			this.InitState();
			this.SetDisplayMode(this.displayMode);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00027B9C File Offset: 0x00026B9C
		public virtual void SetDisplayMode(DisplayMode mode)
		{
			if (!ClientUtils.IsEnumValid(mode, (int)mode, 1, 4))
			{
				throw new InvalidEnumArgumentException("mode", (int)mode, typeof(DisplayMode));
			}
			this.displayMode = mode;
			this.realDisplayMode = ((mode == DisplayMode.Auto) ? this.GetAutoDisplayMode() : mode);
			switch (this.realDisplayMode)
			{
			case DisplayMode.Hexdump:
				base.SuspendLayout();
				this.edit.Hide();
				if (this.linesCount <= this.rowCount)
				{
					base.ResumeLayout();
					return;
				}
				if (!this.scrollBar.Visible)
				{
					this.scrollBar.Show();
					base.ResumeLayout();
					this.scrollBar.Invalidate();
					this.scrollBar.Select();
					return;
				}
				base.ResumeLayout();
				return;
			case DisplayMode.Ansi:
				this.InitAnsi();
				base.SuspendLayout();
				this.edit.Show();
				this.scrollBar.Hide();
				base.ResumeLayout();
				base.Invalidate();
				return;
			case DisplayMode.Unicode:
				this.InitUnicode();
				base.SuspendLayout();
				this.edit.Show();
				this.scrollBar.Hide();
				base.ResumeLayout();
				base.Invalidate();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00027CC4 File Offset: 0x00026CC4
		public virtual void SetFile(string path)
		{
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
			try
			{
				int num = (int)fileStream.Length;
				byte[] array = new byte[num + 1];
				fileStream.Read(array, 0, num);
				this.SetBytes(array);
				fileStream.Close();
			}
			catch
			{
				fileStream.Close();
				throw;
			}
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x00027D20 File Offset: 0x00026D20
		public virtual void SetStartLine(int line)
		{
			if (line < 0 || line >= this.linesCount || line > this.dataBuf.Length / this.columnCount)
			{
				this.startLine = 0;
				return;
			}
			this.startLine = line;
		}

		// Token: 0x04000D61 RID: 3425
		private const int DEFAULT_COLUMN_COUNT = 16;

		// Token: 0x04000D62 RID: 3426
		private const int DEFAULT_ROW_COUNT = 25;

		// Token: 0x04000D63 RID: 3427
		private const int COLUMN_COUNT = 16;

		// Token: 0x04000D64 RID: 3428
		private const int BORDER_GAP = 2;

		// Token: 0x04000D65 RID: 3429
		private const int INSET_GAP = 3;

		// Token: 0x04000D66 RID: 3430
		private const int CELL_HEIGHT = 21;

		// Token: 0x04000D67 RID: 3431
		private const int CELL_WIDTH = 25;

		// Token: 0x04000D68 RID: 3432
		private const int CHAR_WIDTH = 8;

		// Token: 0x04000D69 RID: 3433
		private const int ADDRESS_WIDTH = 69;

		// Token: 0x04000D6A RID: 3434
		private const int HEX_WIDTH = 400;

		// Token: 0x04000D6B RID: 3435
		private const int DUMP_WIDTH = 128;

		// Token: 0x04000D6C RID: 3436
		private const int HEX_DUMP_GAP = 5;

		// Token: 0x04000D6D RID: 3437
		private const int ADDRESS_START_X = 5;

		// Token: 0x04000D6E RID: 3438
		private const int CLIENT_START_Y = 5;

		// Token: 0x04000D6F RID: 3439
		private const int LINE_START_Y = 7;

		// Token: 0x04000D70 RID: 3440
		private const int HEX_START_X = 74;

		// Token: 0x04000D71 RID: 3441
		private const int DUMP_START_X = 479;

		// Token: 0x04000D72 RID: 3442
		private const int SCROLLBAR_START_X = 612;

		// Token: 0x04000D73 RID: 3443
		private int SCROLLBAR_HEIGHT;

		// Token: 0x04000D74 RID: 3444
		private int SCROLLBAR_WIDTH;

		// Token: 0x04000D75 RID: 3445
		private static readonly Font ADDRESS_FONT = new Font("Microsoft Sans Serif", 8f);

		// Token: 0x04000D76 RID: 3446
		private static readonly Font HEXDUMP_FONT = new Font("Courier New", 8f);

		// Token: 0x04000D77 RID: 3447
		private VScrollBar scrollBar;

		// Token: 0x04000D78 RID: 3448
		private TextBox edit;

		// Token: 0x04000D79 RID: 3449
		private int columnCount = 16;

		// Token: 0x04000D7A RID: 3450
		private int rowCount = 25;

		// Token: 0x04000D7B RID: 3451
		private byte[] dataBuf;

		// Token: 0x04000D7C RID: 3452
		private int startLine;

		// Token: 0x04000D7D RID: 3453
		private int displayLinesCount;

		// Token: 0x04000D7E RID: 3454
		private int linesCount;

		// Token: 0x04000D7F RID: 3455
		private DisplayMode displayMode;

		// Token: 0x04000D80 RID: 3456
		private DisplayMode realDisplayMode;
	}
}
