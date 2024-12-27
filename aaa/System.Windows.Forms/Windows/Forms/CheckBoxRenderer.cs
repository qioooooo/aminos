using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x02000264 RID: 612
	public sealed class CheckBoxRenderer
	{
		// Token: 0x06002001 RID: 8193 RVA: 0x00043AC8 File Offset: 0x00042AC8
		private CheckBoxRenderer()
		{
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06002002 RID: 8194 RVA: 0x00043AD0 File Offset: 0x00042AD0
		// (set) Token: 0x06002003 RID: 8195 RVA: 0x00043AD7 File Offset: 0x00042AD7
		public static bool RenderMatchingApplicationState
		{
			get
			{
				return CheckBoxRenderer.renderMatchingApplicationState;
			}
			set
			{
				CheckBoxRenderer.renderMatchingApplicationState = value;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06002004 RID: 8196 RVA: 0x00043ADF File Offset: 0x00042ADF
		private static bool RenderWithVisualStyles
		{
			get
			{
				return !CheckBoxRenderer.renderMatchingApplicationState || Application.RenderWithVisualStyles;
			}
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x00043AEF File Offset: 0x00042AEF
		public static bool IsBackgroundPartiallyTransparent(CheckBoxState state)
		{
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer((int)state);
				return CheckBoxRenderer.visualStyleRenderer.IsBackgroundPartiallyTransparent();
			}
			return false;
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x00043B0A File Offset: 0x00042B0A
		public static void DrawParentBackground(Graphics g, Rectangle bounds, Control childControl)
		{
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer(0);
				CheckBoxRenderer.visualStyleRenderer.DrawParentBackground(g, bounds, childControl);
			}
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x00043B28 File Offset: 0x00042B28
		public static void DrawCheckBox(Graphics g, Point glyphLocation, CheckBoxState state)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, CheckBoxRenderer.GetGlyphSize(g, state));
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer((int)state);
				CheckBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle);
				return;
			}
			if (CheckBoxRenderer.IsMixed(state))
			{
				ControlPaint.DrawMixedCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
				return;
			}
			ControlPaint.DrawCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x00043B81 File Offset: 0x00042B81
		public static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, bool focused, CheckBoxState state)
		{
			CheckBoxRenderer.DrawCheckBox(g, glyphLocation, textBounds, checkBoxText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, focused, state);
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x00043B94 File Offset: 0x00042B94
		public static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, TextFormatFlags flags, bool focused, CheckBoxState state)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, CheckBoxRenderer.GetGlyphSize(g, state));
			Color color;
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer((int)state);
				CheckBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle);
				color = CheckBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				if (CheckBoxRenderer.IsMixed(state))
				{
					ControlPaint.DrawMixedCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
				}
				else
				{
					ControlPaint.DrawCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
				}
				color = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, checkBoxText, font, textBounds, color, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, textBounds);
			}
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x00043C24 File Offset: 0x00042C24
		public static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, Image image, Rectangle imageBounds, bool focused, CheckBoxState state)
		{
			CheckBoxRenderer.DrawCheckBox(g, glyphLocation, textBounds, checkBoxText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, image, imageBounds, focused, state);
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x00043C48 File Offset: 0x00042C48
		public static void DrawCheckBox(Graphics g, Point glyphLocation, Rectangle textBounds, string checkBoxText, Font font, TextFormatFlags flags, Image image, Rectangle imageBounds, bool focused, CheckBoxState state)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, CheckBoxRenderer.GetGlyphSize(g, state));
			Color color;
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer((int)state);
				CheckBoxRenderer.visualStyleRenderer.DrawImage(g, imageBounds, image);
				CheckBoxRenderer.visualStyleRenderer.DrawBackground(g, rectangle);
				color = CheckBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				g.DrawImage(image, imageBounds);
				if (CheckBoxRenderer.IsMixed(state))
				{
					ControlPaint.DrawMixedCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
				}
				else
				{
					ControlPaint.DrawCheckBox(g, rectangle, CheckBoxRenderer.ConvertToButtonState(state));
				}
				color = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, checkBoxText, font, textBounds, color, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, textBounds);
			}
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x00043CEF File Offset: 0x00042CEF
		public static Size GetGlyphSize(Graphics g, CheckBoxState state)
		{
			if (CheckBoxRenderer.RenderWithVisualStyles)
			{
				CheckBoxRenderer.InitializeRenderer((int)state);
				return CheckBoxRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.Draw);
			}
			return new Size(13, 13);
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x00043D14 File Offset: 0x00042D14
		internal static ButtonState ConvertToButtonState(CheckBoxState state)
		{
			switch (state)
			{
			case CheckBoxState.UncheckedPressed:
				return ButtonState.Pushed;
			case CheckBoxState.UncheckedDisabled:
				return ButtonState.Inactive;
			case CheckBoxState.CheckedNormal:
			case CheckBoxState.CheckedHot:
				return ButtonState.Checked;
			case CheckBoxState.CheckedPressed:
				return ButtonState.Checked | ButtonState.Pushed;
			case CheckBoxState.CheckedDisabled:
				return ButtonState.Checked | ButtonState.Inactive;
			case CheckBoxState.MixedNormal:
			case CheckBoxState.MixedHot:
				return ButtonState.Checked;
			case CheckBoxState.MixedPressed:
				return ButtonState.Checked | ButtonState.Pushed;
			case CheckBoxState.MixedDisabled:
				return ButtonState.Checked | ButtonState.Inactive;
			default:
				return ButtonState.Normal;
			}
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x00043D88 File Offset: 0x00042D88
		internal static CheckBoxState ConvertFromButtonState(ButtonState state, bool isMixed, bool isHot)
		{
			if (isMixed)
			{
				if ((state & ButtonState.Pushed) == ButtonState.Pushed)
				{
					return CheckBoxState.MixedPressed;
				}
				if ((state & ButtonState.Inactive) == ButtonState.Inactive)
				{
					return CheckBoxState.MixedDisabled;
				}
				if (isHot)
				{
					return CheckBoxState.MixedHot;
				}
				return CheckBoxState.MixedNormal;
			}
			else if ((state & ButtonState.Checked) == ButtonState.Checked)
			{
				if ((state & ButtonState.Pushed) == ButtonState.Pushed)
				{
					return CheckBoxState.CheckedPressed;
				}
				if ((state & ButtonState.Inactive) == ButtonState.Inactive)
				{
					return CheckBoxState.CheckedDisabled;
				}
				if (isHot)
				{
					return CheckBoxState.CheckedHot;
				}
				return CheckBoxState.CheckedNormal;
			}
			else
			{
				if ((state & ButtonState.Pushed) == ButtonState.Pushed)
				{
					return CheckBoxState.UncheckedPressed;
				}
				if ((state & ButtonState.Inactive) == ButtonState.Inactive)
				{
					return CheckBoxState.UncheckedDisabled;
				}
				if (isHot)
				{
					return CheckBoxState.UncheckedHot;
				}
				return CheckBoxState.UncheckedNormal;
			}
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x00043E20 File Offset: 0x00042E20
		private static bool IsMixed(CheckBoxState state)
		{
			switch (state)
			{
			case CheckBoxState.MixedNormal:
			case CheckBoxState.MixedHot:
			case CheckBoxState.MixedPressed:
			case CheckBoxState.MixedDisabled:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x00043E50 File Offset: 0x00042E50
		private static void InitializeRenderer(int state)
		{
			if (CheckBoxRenderer.visualStyleRenderer == null)
			{
				CheckBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(CheckBoxRenderer.CheckBoxElement.ClassName, CheckBoxRenderer.CheckBoxElement.Part, state);
				return;
			}
			CheckBoxRenderer.visualStyleRenderer.SetParameters(CheckBoxRenderer.CheckBoxElement.ClassName, CheckBoxRenderer.CheckBoxElement.Part, state);
		}

		// Token: 0x0400147C RID: 5244
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x0400147D RID: 5245
		private static readonly VisualStyleElement CheckBoxElement = VisualStyleElement.Button.CheckBox.UncheckedNormal;

		// Token: 0x0400147E RID: 5246
		private static bool renderMatchingApplicationState = true;
	}
}
