using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace System.Windows.Forms
{
	// Token: 0x020002D1 RID: 721
	internal class DataGridCaption
	{
		// Token: 0x060029AF RID: 10671 RVA: 0x0006E250 File Offset: 0x0006D250
		internal DataGridCaption(DataGrid dataGrid)
		{
			this.dataGrid = dataGrid;
			this.downButtonVisible = dataGrid.ParentRowsVisible;
			DataGridCaption.colorMap[0].OldColor = Color.White;
			DataGridCaption.colorMap[0].NewColor = this.ForeColor;
			this.OnGridFontChanged();
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x0006E2F0 File Offset: 0x0006D2F0
		internal void OnGridFontChanged()
		{
			if (this.dataGridFont != null)
			{
				if (this.dataGridFont.Equals(this.dataGrid.Font))
				{
					return;
				}
			}
			try
			{
				this.dataGridFont = new Font(this.dataGrid.Font, FontStyle.Bold);
			}
			catch
			{
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x060029B1 RID: 10673 RVA: 0x0006E34C File Offset: 0x0006D34C
		// (set) Token: 0x060029B2 RID: 10674 RVA: 0x0006E354 File Offset: 0x0006D354
		internal bool BackButtonActive
		{
			get
			{
				return this.backActive;
			}
			set
			{
				if (this.backActive != value)
				{
					this.backActive = value;
					this.InvalidateCaptionRect(this.backButtonRect);
				}
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060029B3 RID: 10675 RVA: 0x0006E372 File Offset: 0x0006D372
		// (set) Token: 0x060029B4 RID: 10676 RVA: 0x0006E37A File Offset: 0x0006D37A
		internal bool DownButtonActive
		{
			get
			{
				return this.downActive;
			}
			set
			{
				if (this.downActive != value)
				{
					this.downActive = value;
					this.InvalidateCaptionRect(this.downButtonRect);
				}
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x060029B5 RID: 10677 RVA: 0x0006E398 File Offset: 0x0006D398
		internal static SolidBrush DefaultBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaption;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x0006E3A4 File Offset: 0x0006D3A4
		internal static Pen DefaultTextBorderPen
		{
			get
			{
				return new Pen(SystemColors.ActiveCaptionText);
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060029B7 RID: 10679 RVA: 0x0006E3B0 File Offset: 0x0006D3B0
		internal static SolidBrush DefaultForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaptionText;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060029B8 RID: 10680 RVA: 0x0006E3BC File Offset: 0x0006D3BC
		// (set) Token: 0x060029B9 RID: 10681 RVA: 0x0006E3CC File Offset: 0x0006D3CC
		internal Color BackColor
		{
			get
			{
				return this.backBrush.Color;
			}
			set
			{
				if (!this.backBrush.Color.Equals(value))
				{
					if (value.IsEmpty)
					{
						throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "Caption BackColor" }));
					}
					this.backBrush = new SolidBrush(value);
					this.Invalidate();
				}
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x0006E435 File Offset: 0x0006D435
		internal EventHandlerList Events
		{
			get
			{
				if (this.events == null)
				{
					this.events = new EventHandlerList();
				}
				return this.events;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060029BB RID: 10683 RVA: 0x0006E450 File Offset: 0x0006D450
		// (set) Token: 0x060029BC RID: 10684 RVA: 0x0006E468 File Offset: 0x0006D468
		internal Font Font
		{
			get
			{
				if (this.textFont == null)
				{
					return this.dataGridFont;
				}
				return this.textFont;
			}
			set
			{
				if (this.textFont == null || !this.textFont.Equals(value))
				{
					this.textFont = value;
					if (this.dataGrid.Caption != null)
					{
						this.dataGrid.RecalculateFonts();
						this.dataGrid.PerformLayout();
						this.dataGrid.Invalidate();
					}
				}
			}
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x0006E4C0 File Offset: 0x0006D4C0
		internal bool ShouldSerializeFont()
		{
			return this.textFont != null && !this.textFont.Equals(this.dataGridFont);
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x0006E4E0 File Offset: 0x0006D4E0
		internal bool ShouldSerializeBackColor()
		{
			return !this.backBrush.Equals(DataGridCaption.DefaultBackBrush);
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x0006E4F5 File Offset: 0x0006D4F5
		internal void ResetBackColor()
		{
			if (this.ShouldSerializeBackColor())
			{
				this.backBrush = DataGridCaption.DefaultBackBrush;
				this.Invalidate();
			}
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x0006E510 File Offset: 0x0006D510
		internal void ResetForeColor()
		{
			if (this.ShouldSerializeForeColor())
			{
				this.foreBrush = DataGridCaption.DefaultForeBrush;
				this.Invalidate();
			}
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x0006E52B File Offset: 0x0006D52B
		internal bool ShouldSerializeForeColor()
		{
			return !this.foreBrush.Equals(DataGridCaption.DefaultForeBrush);
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x0006E540 File Offset: 0x0006D540
		internal void ResetFont()
		{
			this.textFont = null;
			this.Invalidate();
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060029C3 RID: 10691 RVA: 0x0006E54F File Offset: 0x0006D54F
		// (set) Token: 0x060029C4 RID: 10692 RVA: 0x0006E557 File Offset: 0x0006D557
		internal string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (value == null)
				{
					this.text = "";
				}
				else
				{
					this.text = value;
				}
				this.Invalidate();
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060029C5 RID: 10693 RVA: 0x0006E576 File Offset: 0x0006D576
		// (set) Token: 0x060029C6 RID: 10694 RVA: 0x0006E57E File Offset: 0x0006D57E
		internal bool TextBorderVisible
		{
			get
			{
				return this.textBorderVisible;
			}
			set
			{
				this.textBorderVisible = value;
				this.Invalidate();
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060029C7 RID: 10695 RVA: 0x0006E58D File Offset: 0x0006D58D
		// (set) Token: 0x060029C8 RID: 10696 RVA: 0x0006E59C File Offset: 0x0006D59C
		internal Color ForeColor
		{
			get
			{
				return this.foreBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[] { "Caption ForeColor" }));
				}
				this.foreBrush = new SolidBrush(value);
				DataGridCaption.colorMap[0].NewColor = this.ForeColor;
				this.Invalidate();
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060029C9 RID: 10697 RVA: 0x0006E5F6 File Offset: 0x0006D5F6
		internal Point MinimumBounds
		{
			get
			{
				return DataGridCaption.minimumBounds;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060029CA RID: 10698 RVA: 0x0006E5FD File Offset: 0x0006D5FD
		// (set) Token: 0x060029CB RID: 10699 RVA: 0x0006E605 File Offset: 0x0006D605
		internal bool BackButtonVisible
		{
			get
			{
				return this.backButtonVisible;
			}
			set
			{
				if (this.backButtonVisible != value)
				{
					this.backButtonVisible = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060029CC RID: 10700 RVA: 0x0006E61D File Offset: 0x0006D61D
		// (set) Token: 0x060029CD RID: 10701 RVA: 0x0006E625 File Offset: 0x0006D625
		internal bool DownButtonVisible
		{
			get
			{
				return this.downButtonVisible;
			}
			set
			{
				if (this.downButtonVisible != value)
				{
					this.downButtonVisible = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x0006E640 File Offset: 0x0006D640
		protected virtual void AddEventHandler(object key, Delegate handler)
		{
			lock (this)
			{
				if (handler != null)
				{
					for (DataGridCaption.EventEntry next = this.eventList; next != null; next = next.next)
					{
						if (next.key == key)
						{
							next.handler = Delegate.Combine(next.handler, handler);
							return;
						}
					}
					this.eventList = new DataGridCaption.EventEntry(this.eventList, key, handler);
				}
			}
		}

		// Token: 0x1400012C RID: 300
		// (add) Token: 0x060029CF RID: 10703 RVA: 0x0006E6B8 File Offset: 0x0006D6B8
		// (remove) Token: 0x060029D0 RID: 10704 RVA: 0x0006E6CB File Offset: 0x0006D6CB
		internal event EventHandler BackwardClicked
		{
			add
			{
				this.Events.AddHandler(DataGridCaption.EVENT_BACKWARDCLICKED, value);
			}
			remove
			{
				this.Events.RemoveHandler(DataGridCaption.EVENT_BACKWARDCLICKED, value);
			}
		}

		// Token: 0x1400012D RID: 301
		// (add) Token: 0x060029D1 RID: 10705 RVA: 0x0006E6DE File Offset: 0x0006D6DE
		// (remove) Token: 0x060029D2 RID: 10706 RVA: 0x0006E6F1 File Offset: 0x0006D6F1
		internal event EventHandler CaptionClicked
		{
			add
			{
				this.Events.AddHandler(DataGridCaption.EVENT_CAPTIONCLICKED, value);
			}
			remove
			{
				this.Events.RemoveHandler(DataGridCaption.EVENT_CAPTIONCLICKED, value);
			}
		}

		// Token: 0x1400012E RID: 302
		// (add) Token: 0x060029D3 RID: 10707 RVA: 0x0006E704 File Offset: 0x0006D704
		// (remove) Token: 0x060029D4 RID: 10708 RVA: 0x0006E717 File Offset: 0x0006D717
		internal event EventHandler DownClicked
		{
			add
			{
				this.Events.AddHandler(DataGridCaption.EVENT_DOWNCLICKED, value);
			}
			remove
			{
				this.Events.RemoveHandler(DataGridCaption.EVENT_DOWNCLICKED, value);
			}
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x0006E72A File Offset: 0x0006D72A
		private void Invalidate()
		{
			if (this.dataGrid != null)
			{
				this.dataGrid.InvalidateCaption();
			}
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x0006E73F File Offset: 0x0006D73F
		private void InvalidateCaptionRect(Rectangle r)
		{
			if (this.dataGrid != null)
			{
				this.dataGrid.InvalidateCaptionRect(r);
			}
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x0006E758 File Offset: 0x0006D758
		private void InvalidateLocation(DataGridCaption.CaptionLocation loc)
		{
			switch (loc)
			{
			case DataGridCaption.CaptionLocation.BackButton:
			{
				Rectangle rectangle = this.backButtonRect;
				rectangle.Inflate(1, 1);
				this.InvalidateCaptionRect(rectangle);
				return;
			}
			case DataGridCaption.CaptionLocation.DownButton:
			{
				Rectangle rectangle = this.downButtonRect;
				rectangle.Inflate(1, 1);
				this.InvalidateCaptionRect(rectangle);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x0006E7A8 File Offset: 0x0006D7A8
		protected void OnBackwardClicked(EventArgs e)
		{
			if (this.backActive)
			{
				EventHandler eventHandler = (EventHandler)this.Events[DataGridCaption.EVENT_BACKWARDCLICKED];
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x0006E7E0 File Offset: 0x0006D7E0
		protected void OnCaptionClicked(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)this.Events[DataGridCaption.EVENT_CAPTIONCLICKED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x0006E810 File Offset: 0x0006D810
		protected void OnDownClicked(EventArgs e)
		{
			if (this.downActive && this.downButtonVisible)
			{
				EventHandler eventHandler = (EventHandler)this.Events[DataGridCaption.EVENT_DOWNCLICKED];
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x0006E850 File Offset: 0x0006D850
		private Bitmap GetBitmap(string bitmapName)
		{
			Bitmap bitmap = null;
			try
			{
				bitmap = new Bitmap(typeof(DataGridCaption), bitmapName);
				bitmap.MakeTransparent();
			}
			catch (Exception)
			{
			}
			return bitmap;
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x0006E88C File Offset: 0x0006D88C
		private Bitmap GetBackButtonBmp(bool alignRight)
		{
			if (alignRight)
			{
				if (DataGridCaption.leftButtonBitmap_bidi == null)
				{
					DataGridCaption.leftButtonBitmap_bidi = this.GetBitmap("DataGridCaption.backarrow_bidi.bmp");
				}
				return DataGridCaption.leftButtonBitmap_bidi;
			}
			if (DataGridCaption.leftButtonBitmap == null)
			{
				DataGridCaption.leftButtonBitmap = this.GetBitmap("DataGridCaption.backarrow.bmp");
			}
			return DataGridCaption.leftButtonBitmap;
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x0006E8CA File Offset: 0x0006D8CA
		private Bitmap GetDetailsBmp()
		{
			if (DataGridCaption.magnifyingGlassBitmap == null)
			{
				DataGridCaption.magnifyingGlassBitmap = this.GetBitmap("DataGridCaption.Details.bmp");
			}
			return DataGridCaption.magnifyingGlassBitmap;
		}

		// Token: 0x060029DE RID: 10718 RVA: 0x0006E8E8 File Offset: 0x0006D8E8
		protected virtual Delegate GetEventHandler(object key)
		{
			Delegate @delegate;
			lock (this)
			{
				for (DataGridCaption.EventEntry next = this.eventList; next != null; next = next.next)
				{
					if (next.key == key)
					{
						return next.handler;
					}
				}
				@delegate = null;
			}
			return @delegate;
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x0006E940 File Offset: 0x0006D940
		internal Rectangle GetBackButtonRect(Rectangle bounds, bool alignRight, int downButtonWidth)
		{
			Bitmap backButtonBmp = this.GetBackButtonBmp(false);
			Size size;
			lock (backButtonBmp)
			{
				size = backButtonBmp.Size;
			}
			return new Rectangle(bounds.Right - 12 - downButtonWidth - size.Width, bounds.Y + 1 + 2, size.Width, size.Height);
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x0006E9B0 File Offset: 0x0006D9B0
		internal int GetDetailsButtonWidth()
		{
			int num = 0;
			Bitmap detailsBmp = this.GetDetailsBmp();
			lock (detailsBmp)
			{
				num = detailsBmp.Size.Width;
			}
			return num;
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x0006E9F8 File Offset: 0x0006D9F8
		internal Rectangle GetDetailsButtonRect(Rectangle bounds, bool alignRight)
		{
			Bitmap detailsBmp = this.GetDetailsBmp();
			Size size;
			lock (detailsBmp)
			{
				size = detailsBmp.Size;
			}
			int width = size.Width;
			return new Rectangle(bounds.Right - 6 - width, bounds.Y + 1 + 2, width, size.Height);
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x0006EA60 File Offset: 0x0006DA60
		internal void Paint(Graphics g, Rectangle bounds, bool alignRight)
		{
			Size size = new Size((int)g.MeasureString(this.text, this.Font).Width + 2, this.Font.Height + 2);
			this.downButtonRect = this.GetDetailsButtonRect(bounds, alignRight);
			int detailsButtonWidth = this.GetDetailsButtonWidth();
			this.backButtonRect = this.GetBackButtonRect(bounds, alignRight, detailsButtonWidth);
			int num = (this.backButtonVisible ? (this.backButtonRect.Width + 3 + 4) : 0);
			int num2 = ((this.downButtonVisible && !this.dataGrid.ParentRowsIsEmpty()) ? (detailsButtonWidth + 3 + 4) : 0);
			int num3 = bounds.Width - 3 - num - num2;
			this.textRect = new Rectangle(bounds.X, bounds.Y + 1, Math.Min(num3, 4 + size.Width), 4 + size.Height);
			if (alignRight)
			{
				this.textRect.X = bounds.Right - this.textRect.Width;
				this.backButtonRect.X = bounds.X + 12 + detailsButtonWidth;
				this.downButtonRect.X = bounds.X + 6;
			}
			g.FillRectangle(this.backBrush, bounds);
			if (this.backButtonVisible)
			{
				this.PaintBackButton(g, this.backButtonRect, alignRight);
				if (this.backActive && this.lastMouseLocation == DataGridCaption.CaptionLocation.BackButton)
				{
					this.backButtonRect.Inflate(1, 1);
					ControlPaint.DrawBorder3D(g, this.backButtonRect, this.backPressed ? Border3DStyle.SunkenInner : Border3DStyle.RaisedInner);
				}
			}
			this.PaintText(g, this.textRect, alignRight);
			if (this.downButtonVisible && !this.dataGrid.ParentRowsIsEmpty())
			{
				this.PaintDownButton(g, this.downButtonRect);
				if (this.lastMouseLocation == DataGridCaption.CaptionLocation.DownButton)
				{
					this.downButtonRect.Inflate(1, 1);
					ControlPaint.DrawBorder3D(g, this.downButtonRect, this.downPressed ? Border3DStyle.SunkenInner : Border3DStyle.RaisedInner);
				}
			}
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x0006EC44 File Offset: 0x0006DC44
		private void PaintIcon(Graphics g, Rectangle bounds, Bitmap b)
		{
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetRemapTable(DataGridCaption.colorMap, ColorAdjustType.Bitmap);
			g.DrawImage(b, bounds, 0, 0, bounds.Width, bounds.Height, GraphicsUnit.Pixel, imageAttributes);
			imageAttributes.Dispose();
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x0006EC84 File Offset: 0x0006DC84
		private void PaintBackButton(Graphics g, Rectangle bounds, bool alignRight)
		{
			Bitmap backButtonBmp = this.GetBackButtonBmp(alignRight);
			lock (backButtonBmp)
			{
				this.PaintIcon(g, bounds, backButtonBmp);
			}
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x0006ECC4 File Offset: 0x0006DCC4
		private void PaintDownButton(Graphics g, Rectangle bounds)
		{
			Bitmap detailsBmp = this.GetDetailsBmp();
			lock (detailsBmp)
			{
				this.PaintIcon(g, bounds, detailsBmp);
			}
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x0006ED04 File Offset: 0x0006DD04
		private void PaintText(Graphics g, Rectangle bounds, bool alignToRight)
		{
			Rectangle rectangle = bounds;
			if (rectangle.Width <= 0 || rectangle.Height <= 0)
			{
				return;
			}
			if (this.textBorderVisible)
			{
				g.DrawRectangle(this.textBorderPen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				rectangle.Inflate(-1, -1);
			}
			Rectangle rectangle2 = rectangle;
			rectangle2.Height = 2;
			g.FillRectangle(this.backBrush, rectangle2);
			rectangle2.Y = rectangle.Bottom - 2;
			g.FillRectangle(this.backBrush, rectangle2);
			rectangle2 = new Rectangle(rectangle.X, rectangle.Y + 2, 2, rectangle.Height - 4);
			g.FillRectangle(this.backBrush, rectangle2);
			rectangle2.X = rectangle.Right - 2;
			g.FillRectangle(this.backBrush, rectangle2);
			rectangle.Inflate(-2, -2);
			g.FillRectangle(this.backBrush, rectangle);
			StringFormat stringFormat = new StringFormat();
			if (alignToRight)
			{
				stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
				stringFormat.Alignment = StringAlignment.Far;
			}
			g.DrawString(this.text, this.Font, this.foreBrush, rectangle, stringFormat);
			stringFormat.Dispose();
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x0006EE40 File Offset: 0x0006DE40
		private DataGridCaption.CaptionLocation FindLocation(int x, int y)
		{
			if (!this.backButtonRect.IsEmpty && this.backButtonRect.Contains(x, y))
			{
				return DataGridCaption.CaptionLocation.BackButton;
			}
			if (!this.downButtonRect.IsEmpty && this.downButtonRect.Contains(x, y))
			{
				return DataGridCaption.CaptionLocation.DownButton;
			}
			if (!this.textRect.IsEmpty && this.textRect.Contains(x, y))
			{
				return DataGridCaption.CaptionLocation.Text;
			}
			return DataGridCaption.CaptionLocation.Nowhere;
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060029E8 RID: 10728 RVA: 0x0006EEA8 File Offset: 0x0006DEA8
		// (set) Token: 0x060029E9 RID: 10729 RVA: 0x0006EEB0 File Offset: 0x0006DEB0
		private bool DownButtonDown
		{
			get
			{
				return this.downButtonDown;
			}
			set
			{
				if (this.downButtonDown != value)
				{
					this.downButtonDown = value;
					this.InvalidateLocation(DataGridCaption.CaptionLocation.DownButton);
				}
			}
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x0006EEC9 File Offset: 0x0006DEC9
		internal bool GetDownButtonDirection()
		{
			return this.DownButtonDown;
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x0006EED4 File Offset: 0x0006DED4
		internal void MouseDown(int x, int y)
		{
			DataGridCaption.CaptionLocation captionLocation = this.FindLocation(x, y);
			switch (captionLocation)
			{
			case DataGridCaption.CaptionLocation.BackButton:
				this.backPressed = true;
				this.InvalidateLocation(captionLocation);
				return;
			case DataGridCaption.CaptionLocation.DownButton:
				this.downPressed = true;
				this.InvalidateLocation(captionLocation);
				return;
			case DataGridCaption.CaptionLocation.Text:
				this.OnCaptionClicked(EventArgs.Empty);
				return;
			default:
				return;
			}
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x0006EF2C File Offset: 0x0006DF2C
		internal void MouseUp(int x, int y)
		{
			switch (this.FindLocation(x, y))
			{
			case DataGridCaption.CaptionLocation.BackButton:
				if (this.backPressed)
				{
					this.backPressed = false;
					this.OnBackwardClicked(EventArgs.Empty);
				}
				break;
			case DataGridCaption.CaptionLocation.DownButton:
				if (this.downPressed)
				{
					this.downPressed = false;
					this.OnDownClicked(EventArgs.Empty);
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x0006EF8C File Offset: 0x0006DF8C
		internal void MouseLeft()
		{
			DataGridCaption.CaptionLocation captionLocation = this.lastMouseLocation;
			this.lastMouseLocation = DataGridCaption.CaptionLocation.Nowhere;
			this.InvalidateLocation(captionLocation);
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x0006EFB0 File Offset: 0x0006DFB0
		internal void MouseOver(int x, int y)
		{
			DataGridCaption.CaptionLocation captionLocation = this.FindLocation(x, y);
			this.InvalidateLocation(this.lastMouseLocation);
			this.InvalidateLocation(captionLocation);
			this.lastMouseLocation = captionLocation;
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x0006EFE0 File Offset: 0x0006DFE0
		protected virtual void RaiseEvent(object key, EventArgs e)
		{
			Delegate eventHandler = this.GetEventHandler(key);
			if (eventHandler != null)
			{
				((EventHandler)eventHandler)(this, e);
			}
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x0006F008 File Offset: 0x0006E008
		protected virtual void RemoveEventHandler(object key, Delegate handler)
		{
			lock (this)
			{
				if (handler != null)
				{
					DataGridCaption.EventEntry next = this.eventList;
					DataGridCaption.EventEntry eventEntry = null;
					while (next != null)
					{
						if (next.key == key)
						{
							next.handler = Delegate.Remove(next.handler, handler);
							if (next.handler == null)
							{
								if (eventEntry == null)
								{
									this.eventList = next.next;
								}
								else
								{
									eventEntry.next = next.next;
								}
							}
							break;
						}
						eventEntry = next;
						next = next.next;
					}
				}
			}
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x0006F094 File Offset: 0x0006E094
		protected virtual void RemoveEventHandlers()
		{
			this.eventList = null;
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x0006F09D File Offset: 0x0006E09D
		internal void SetDownButtonDirection(bool pointDown)
		{
			this.DownButtonDown = pointDown;
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x0006F0A6 File Offset: 0x0006E0A6
		internal bool ToggleDownButtonDirection()
		{
			this.DownButtonDown = !this.DownButtonDown;
			return this.DownButtonDown;
		}

		// Token: 0x0400176C RID: 5996
		private const int xOffset = 3;

		// Token: 0x0400176D RID: 5997
		private const int yOffset = 1;

		// Token: 0x0400176E RID: 5998
		private const int textPadding = 2;

		// Token: 0x0400176F RID: 5999
		private const int buttonToText = 4;

		// Token: 0x04001770 RID: 6000
		internal EventHandlerList events;

		// Token: 0x04001771 RID: 6001
		private static ColorMap[] colorMap = new ColorMap[]
		{
			new ColorMap()
		};

		// Token: 0x04001772 RID: 6002
		private static readonly Point minimumBounds = new Point(50, 30);

		// Token: 0x04001773 RID: 6003
		private DataGrid dataGrid;

		// Token: 0x04001774 RID: 6004
		private bool backButtonVisible;

		// Token: 0x04001775 RID: 6005
		private bool downButtonVisible;

		// Token: 0x04001776 RID: 6006
		private SolidBrush backBrush = DataGridCaption.DefaultBackBrush;

		// Token: 0x04001777 RID: 6007
		private SolidBrush foreBrush = DataGridCaption.DefaultForeBrush;

		// Token: 0x04001778 RID: 6008
		private Pen textBorderPen = DataGridCaption.DefaultTextBorderPen;

		// Token: 0x04001779 RID: 6009
		private string text = "";

		// Token: 0x0400177A RID: 6010
		private bool textBorderVisible;

		// Token: 0x0400177B RID: 6011
		private Font textFont;

		// Token: 0x0400177C RID: 6012
		private Font dataGridFont;

		// Token: 0x0400177D RID: 6013
		private bool backActive;

		// Token: 0x0400177E RID: 6014
		private bool downActive;

		// Token: 0x0400177F RID: 6015
		private bool backPressed;

		// Token: 0x04001780 RID: 6016
		private bool downPressed;

		// Token: 0x04001781 RID: 6017
		private bool downButtonDown;

		// Token: 0x04001782 RID: 6018
		private static Bitmap leftButtonBitmap;

		// Token: 0x04001783 RID: 6019
		private static Bitmap leftButtonBitmap_bidi;

		// Token: 0x04001784 RID: 6020
		private static Bitmap magnifyingGlassBitmap;

		// Token: 0x04001785 RID: 6021
		private Rectangle backButtonRect = default(Rectangle);

		// Token: 0x04001786 RID: 6022
		private Rectangle downButtonRect = default(Rectangle);

		// Token: 0x04001787 RID: 6023
		private Rectangle textRect = default(Rectangle);

		// Token: 0x04001788 RID: 6024
		private DataGridCaption.CaptionLocation lastMouseLocation;

		// Token: 0x04001789 RID: 6025
		private DataGridCaption.EventEntry eventList;

		// Token: 0x0400178A RID: 6026
		private static readonly object EVENT_BACKWARDCLICKED = new object();

		// Token: 0x0400178B RID: 6027
		private static readonly object EVENT_DOWNCLICKED = new object();

		// Token: 0x0400178C RID: 6028
		private static readonly object EVENT_CAPTIONCLICKED = new object();

		// Token: 0x020002D2 RID: 722
		internal enum CaptionLocation
		{
			// Token: 0x0400178E RID: 6030
			Nowhere,
			// Token: 0x0400178F RID: 6031
			BackButton,
			// Token: 0x04001790 RID: 6032
			DownButton,
			// Token: 0x04001791 RID: 6033
			Text
		}

		// Token: 0x020002D3 RID: 723
		private sealed class EventEntry
		{
			// Token: 0x060029F5 RID: 10741 RVA: 0x0006F10E File Offset: 0x0006E10E
			internal EventEntry(DataGridCaption.EventEntry next, object key, Delegate handler)
			{
				this.next = next;
				this.key = key;
				this.handler = handler;
			}

			// Token: 0x04001792 RID: 6034
			internal DataGridCaption.EventEntry next;

			// Token: 0x04001793 RID: 6035
			internal object key;

			// Token: 0x04001794 RID: 6036
			internal Delegate handler;
		}
	}
}
