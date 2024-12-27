using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200041D RID: 1053
	public sealed class GroupBoxRenderer
	{
		// Token: 0x06003EB1 RID: 16049 RVA: 0x000E3F91 File Offset: 0x000E2F91
		private GroupBoxRenderer()
		{
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06003EB2 RID: 16050 RVA: 0x000E3F99 File Offset: 0x000E2F99
		// (set) Token: 0x06003EB3 RID: 16051 RVA: 0x000E3FA0 File Offset: 0x000E2FA0
		public static bool RenderMatchingApplicationState
		{
			get
			{
				return GroupBoxRenderer.renderMatchingApplicationState;
			}
			set
			{
				GroupBoxRenderer.renderMatchingApplicationState = value;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06003EB4 RID: 16052 RVA: 0x000E3FA8 File Offset: 0x000E2FA8
		private static bool RenderWithVisualStyles
		{
			get
			{
				return !GroupBoxRenderer.renderMatchingApplicationState || Application.RenderWithVisualStyles;
			}
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x000E3FB8 File Offset: 0x000E2FB8
		public static bool IsBackgroundPartiallyTransparent(GroupBoxState state)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.InitializeRenderer((int)state);
				return GroupBoxRenderer.visualStyleRenderer.IsBackgroundPartiallyTransparent();
			}
			return false;
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x000E3FD3 File Offset: 0x000E2FD3
		public static void DrawParentBackground(Graphics g, Rectangle bounds, Control childControl)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.InitializeRenderer(0);
				GroupBoxRenderer.visualStyleRenderer.DrawParentBackground(g, bounds, childControl);
			}
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x000E3FEF File Offset: 0x000E2FEF
		public static void DrawGroupBox(Graphics g, Rectangle bounds, GroupBoxState state)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.DrawThemedGroupBoxNoText(g, bounds, state);
				return;
			}
			GroupBoxRenderer.DrawUnthemedGroupBoxNoText(g, bounds, state);
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x000E4009 File Offset: 0x000E3009
		public static void DrawGroupBox(Graphics g, Rectangle bounds, string groupBoxText, Font font, GroupBoxState state)
		{
			GroupBoxRenderer.DrawGroupBox(g, bounds, groupBoxText, font, TextFormatFlags.Default, state);
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x000E4017 File Offset: 0x000E3017
		public static void DrawGroupBox(Graphics g, Rectangle bounds, string groupBoxText, Font font, Color textColor, GroupBoxState state)
		{
			GroupBoxRenderer.DrawGroupBox(g, bounds, groupBoxText, font, textColor, TextFormatFlags.Default, state);
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x000E4027 File Offset: 0x000E3027
		public static void DrawGroupBox(Graphics g, Rectangle bounds, string groupBoxText, Font font, TextFormatFlags flags, GroupBoxState state)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.DrawThemedGroupBoxWithText(g, bounds, groupBoxText, font, GroupBoxRenderer.DefaultTextColor(state), flags, state);
				return;
			}
			GroupBoxRenderer.DrawUnthemedGroupBoxWithText(g, bounds, groupBoxText, font, GroupBoxRenderer.DefaultTextColor(state), flags, state);
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x000E4059 File Offset: 0x000E3059
		public static void DrawGroupBox(Graphics g, Rectangle bounds, string groupBoxText, Font font, Color textColor, TextFormatFlags flags, GroupBoxState state)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.DrawThemedGroupBoxWithText(g, bounds, groupBoxText, font, textColor, flags, state);
				return;
			}
			GroupBoxRenderer.DrawUnthemedGroupBoxWithText(g, bounds, groupBoxText, font, textColor, flags, state);
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x000E4081 File Offset: 0x000E3081
		private static void DrawThemedGroupBoxNoText(Graphics g, Rectangle bounds, GroupBoxState state)
		{
			GroupBoxRenderer.InitializeRenderer((int)state);
			GroupBoxRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x000E4098 File Offset: 0x000E3098
		private static void DrawThemedGroupBoxWithText(Graphics g, Rectangle bounds, string groupBoxText, Font font, Color textColor, TextFormatFlags flags, GroupBoxState state)
		{
			GroupBoxRenderer.InitializeRenderer((int)state);
			Rectangle rectangle = bounds;
			rectangle.Width -= 14;
			Size size = TextRenderer.MeasureText(g, groupBoxText, font, new Size(rectangle.Width, rectangle.Height), flags);
			rectangle.Width = size.Width;
			rectangle.Height = size.Height;
			if ((flags & TextFormatFlags.Right) == TextFormatFlags.Right)
			{
				rectangle.X = bounds.Right - rectangle.Width - 7 + 1;
			}
			else
			{
				rectangle.X += 6;
			}
			TextRenderer.DrawText(g, groupBoxText, font, rectangle, textColor, flags);
			Rectangle rectangle2 = bounds;
			rectangle2.Y += font.Height / 2;
			rectangle2.Height -= font.Height / 2;
			Rectangle rectangle3 = rectangle2;
			Rectangle rectangle4 = rectangle2;
			Rectangle rectangle5 = rectangle2;
			rectangle3.Width = 7;
			rectangle4.Width = Math.Max(0, rectangle.Width - 3);
			if ((flags & TextFormatFlags.Right) == TextFormatFlags.Right)
			{
				rectangle3.X = rectangle2.Right - 7;
				rectangle4.X = rectangle3.Left - rectangle4.Width;
				rectangle5.Width = rectangle4.X - rectangle2.X;
			}
			else
			{
				rectangle4.X = rectangle3.Right;
				rectangle5.X = rectangle4.Right;
				rectangle5.Width = rectangle2.Right - rectangle5.X;
			}
			rectangle4.Y = rectangle.Bottom;
			rectangle4.Height -= rectangle.Bottom - rectangle2.Top;
			GroupBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle2, rectangle3);
			GroupBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle2, rectangle4);
			GroupBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle2, rectangle5);
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x000E4258 File Offset: 0x000E3258
		private static void DrawUnthemedGroupBoxNoText(Graphics g, Rectangle bounds, GroupBoxState state)
		{
			Color control = SystemColors.Control;
			Pen pen = new Pen(ControlPaint.Light(control, 1f));
			Pen pen2 = new Pen(ControlPaint.Dark(control, 0f));
			try
			{
				g.DrawLine(pen, bounds.Left + 1, bounds.Top + 1, bounds.Left + 1, bounds.Height - 1);
				g.DrawLine(pen2, bounds.Left, bounds.Top + 1, bounds.Left, bounds.Height - 2);
				g.DrawLine(pen, bounds.Left, bounds.Height - 1, bounds.Width - 1, bounds.Height - 1);
				g.DrawLine(pen2, bounds.Left, bounds.Height - 2, bounds.Width - 1, bounds.Height - 2);
				g.DrawLine(pen, bounds.Left + 1, bounds.Top + 1, bounds.Width - 1, bounds.Top + 1);
				g.DrawLine(pen2, bounds.Left, bounds.Top, bounds.Width - 2, bounds.Top);
				g.DrawLine(pen, bounds.Width - 1, bounds.Top, bounds.Width - 1, bounds.Height - 1);
				g.DrawLine(pen2, bounds.Width - 2, bounds.Top, bounds.Width - 2, bounds.Height - 2);
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
				}
				if (pen2 != null)
				{
					pen2.Dispose();
				}
			}
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x000E4404 File Offset: 0x000E3404
		private static void DrawUnthemedGroupBoxWithText(Graphics g, Rectangle bounds, string groupBoxText, Font font, Color textColor, TextFormatFlags flags, GroupBoxState state)
		{
			Rectangle rectangle = bounds;
			rectangle.Width -= 8;
			Size size = TextRenderer.MeasureText(g, groupBoxText, font, new Size(rectangle.Width, rectangle.Height), flags);
			rectangle.Width = size.Width;
			rectangle.Height = size.Height;
			if ((flags & TextFormatFlags.Right) == TextFormatFlags.Right)
			{
				rectangle.X = bounds.Right - rectangle.Width - 8;
			}
			else
			{
				rectangle.X += 8;
			}
			TextRenderer.DrawText(g, groupBoxText, font, rectangle, textColor, flags);
			if (rectangle.Width > 0)
			{
				rectangle.Inflate(2, 0);
			}
			Pen pen = new Pen(SystemColors.ControlLight);
			Pen pen2 = new Pen(SystemColors.ControlDark);
			int num = bounds.Top + font.Height / 2;
			g.DrawLine(pen, bounds.Left + 1, num, bounds.Left + 1, bounds.Height - 1);
			g.DrawLine(pen2, bounds.Left, num - 1, bounds.Left, bounds.Height - 2);
			g.DrawLine(pen, bounds.Left, bounds.Height - 1, bounds.Width, bounds.Height - 1);
			g.DrawLine(pen2, bounds.Left, bounds.Height - 2, bounds.Width - 1, bounds.Height - 2);
			g.DrawLine(pen, bounds.Left + 1, num, rectangle.X - 2, num);
			g.DrawLine(pen2, bounds.Left, num - 1, rectangle.X - 3, num - 1);
			g.DrawLine(pen, rectangle.X + rectangle.Width + 1, num, bounds.Width - 1, num);
			g.DrawLine(pen2, rectangle.X + rectangle.Width + 2, num - 1, bounds.Width - 2, num - 1);
			g.DrawLine(pen, bounds.Width - 1, num, bounds.Width - 1, bounds.Height - 1);
			g.DrawLine(pen2, bounds.Width - 2, num - 1, bounds.Width - 2, bounds.Height - 2);
			pen.Dispose();
			pen2.Dispose();
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x000E464A File Offset: 0x000E364A
		private static Color DefaultTextColor(GroupBoxState state)
		{
			if (GroupBoxRenderer.RenderWithVisualStyles)
			{
				GroupBoxRenderer.InitializeRenderer((int)state);
				return GroupBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			return SystemColors.ControlText;
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x000E4670 File Offset: 0x000E3670
		private static void InitializeRenderer(int state)
		{
			if (GroupBoxRenderer.visualStyleRenderer == null)
			{
				GroupBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(GroupBoxRenderer.GroupBoxElement.ClassName, GroupBoxRenderer.GroupBoxElement.Part, state);
				return;
			}
			GroupBoxRenderer.visualStyleRenderer.SetParameters(GroupBoxRenderer.GroupBoxElement.ClassName, GroupBoxRenderer.GroupBoxElement.Part, state);
		}

		// Token: 0x04001ECD RID: 7885
		private const int textOffset = 8;

		// Token: 0x04001ECE RID: 7886
		private const int boxHeaderWidth = 7;

		// Token: 0x04001ECF RID: 7887
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x04001ED0 RID: 7888
		private static readonly VisualStyleElement GroupBoxElement = VisualStyleElement.Button.GroupBox.Normal;

		// Token: 0x04001ED1 RID: 7889
		private static bool renderMatchingApplicationState = true;
	}
}
