using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200025C RID: 604
	public sealed class ButtonRenderer
	{
		// Token: 0x06001FB7 RID: 8119 RVA: 0x00042F41 File Offset: 0x00041F41
		private ButtonRenderer()
		{
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001FB8 RID: 8120 RVA: 0x00042F49 File Offset: 0x00041F49
		// (set) Token: 0x06001FB9 RID: 8121 RVA: 0x00042F50 File Offset: 0x00041F50
		public static bool RenderMatchingApplicationState
		{
			get
			{
				return ButtonRenderer.renderMatchingApplicationState;
			}
			set
			{
				ButtonRenderer.renderMatchingApplicationState = value;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001FBA RID: 8122 RVA: 0x00042F58 File Offset: 0x00041F58
		private static bool RenderWithVisualStyles
		{
			get
			{
				return !ButtonRenderer.renderMatchingApplicationState || Application.RenderWithVisualStyles;
			}
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x00042F68 File Offset: 0x00041F68
		public static bool IsBackgroundPartiallyTransparent(PushButtonState state)
		{
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				return ButtonRenderer.visualStyleRenderer.IsBackgroundPartiallyTransparent();
			}
			return false;
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x00042F83 File Offset: 0x00041F83
		public static void DrawParentBackground(Graphics g, Rectangle bounds, Control childControl)
		{
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer(0);
				ButtonRenderer.visualStyleRenderer.DrawParentBackground(g, bounds, childControl);
			}
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x00042F9F File Offset: 0x00041F9F
		public static void DrawButton(Graphics g, Rectangle bounds, PushButtonState state)
		{
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				ButtonRenderer.visualStyleRenderer.DrawBackground(g, bounds);
				return;
			}
			ControlPaint.DrawButton(g, bounds, ButtonRenderer.ConvertToButtonState(state));
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x00042FC8 File Offset: 0x00041FC8
		public static void DrawButton(Graphics g, Rectangle bounds, bool focused, PushButtonState state)
		{
			Rectangle rectangle;
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				ButtonRenderer.visualStyleRenderer.DrawBackground(g, bounds);
				rectangle = ButtonRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
			}
			else
			{
				ControlPaint.DrawButton(g, bounds, ButtonRenderer.ConvertToButtonState(state));
				rectangle = Rectangle.Inflate(bounds, -3, -3);
			}
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0004301F File Offset: 0x0004201F
		public static void DrawButton(Graphics g, Rectangle bounds, string buttonText, Font font, bool focused, PushButtonState state)
		{
			ButtonRenderer.DrawButton(g, bounds, buttonText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, focused, state);
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x00043030 File Offset: 0x00042030
		public static void DrawButton(Graphics g, Rectangle bounds, string buttonText, Font font, TextFormatFlags flags, bool focused, PushButtonState state)
		{
			Rectangle rectangle;
			Color color;
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				ButtonRenderer.visualStyleRenderer.DrawBackground(g, bounds);
				rectangle = ButtonRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
				color = ButtonRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				ControlPaint.DrawButton(g, bounds, ButtonRenderer.ConvertToButtonState(state));
				rectangle = Rectangle.Inflate(bounds, -3, -3);
				color = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, buttonText, font, rectangle, color, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x000430AC File Offset: 0x000420AC
		public static void DrawButton(Graphics g, Rectangle bounds, Image image, Rectangle imageBounds, bool focused, PushButtonState state)
		{
			Rectangle rectangle;
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				ButtonRenderer.visualStyleRenderer.DrawBackground(g, bounds);
				ButtonRenderer.visualStyleRenderer.DrawImage(g, imageBounds, image);
				rectangle = ButtonRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
			}
			else
			{
				ControlPaint.DrawButton(g, bounds, ButtonRenderer.ConvertToButtonState(state));
				g.DrawImage(image, imageBounds);
				rectangle = Rectangle.Inflate(bounds, -3, -3);
			}
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0004311C File Offset: 0x0004211C
		public static void DrawButton(Graphics g, Rectangle bounds, string buttonText, Font font, Image image, Rectangle imageBounds, bool focused, PushButtonState state)
		{
			ButtonRenderer.DrawButton(g, bounds, buttonText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, image, imageBounds, focused, state);
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x0004313C File Offset: 0x0004213C
		public static void DrawButton(Graphics g, Rectangle bounds, string buttonText, Font font, TextFormatFlags flags, Image image, Rectangle imageBounds, bool focused, PushButtonState state)
		{
			Rectangle rectangle;
			Color color;
			if (ButtonRenderer.RenderWithVisualStyles)
			{
				ButtonRenderer.InitializeRenderer((int)state);
				ButtonRenderer.visualStyleRenderer.DrawBackground(g, bounds);
				ButtonRenderer.visualStyleRenderer.DrawImage(g, imageBounds, image);
				rectangle = ButtonRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
				color = ButtonRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				ControlPaint.DrawButton(g, bounds, ButtonRenderer.ConvertToButtonState(state));
				g.DrawImage(image, imageBounds);
				rectangle = Rectangle.Inflate(bounds, -3, -3);
				color = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, buttonText, font, rectangle, color, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x000431D4 File Offset: 0x000421D4
		internal static ButtonState ConvertToButtonState(PushButtonState state)
		{
			switch (state)
			{
			case PushButtonState.Pressed:
				return ButtonState.Pushed;
			case PushButtonState.Disabled:
				return ButtonState.Inactive;
			default:
				return ButtonState.Normal;
			}
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x00043204 File Offset: 0x00042204
		private static void InitializeRenderer(int state)
		{
			if (ButtonRenderer.visualStyleRenderer == null)
			{
				ButtonRenderer.visualStyleRenderer = new VisualStyleRenderer(ButtonRenderer.ButtonElement.ClassName, ButtonRenderer.ButtonElement.Part, state);
				return;
			}
			ButtonRenderer.visualStyleRenderer.SetParameters(ButtonRenderer.ButtonElement.ClassName, ButtonRenderer.ButtonElement.Part, state);
		}

		// Token: 0x0400145C RID: 5212
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x0400145D RID: 5213
		private static readonly VisualStyleElement ButtonElement = VisualStyleElement.Button.PushButton.Normal;

		// Token: 0x0400145E RID: 5214
		private static bool renderMatchingApplicationState = true;
	}
}
