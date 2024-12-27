using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x02000436 RID: 1078
	public sealed class HtmlElementEventArgs : EventArgs
	{
		// Token: 0x060040BF RID: 16575 RVA: 0x000E8F38 File Offset: 0x000E7F38
		internal HtmlElementEventArgs(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLEventObj eventObj)
		{
			this.htmlEventObj = eventObj;
			this.shimManager = shimManager;
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060040C0 RID: 16576 RVA: 0x000E8F4E File Offset: 0x000E7F4E
		private UnsafeNativeMethods.IHTMLEventObj NativeHTMLEventObj
		{
			get
			{
				return this.htmlEventObj;
			}
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x060040C1 RID: 16577 RVA: 0x000E8F58 File Offset: 0x000E7F58
		public MouseButtons MouseButtonsPressed
		{
			get
			{
				MouseButtons mouseButtons = MouseButtons.None;
				int button = this.NativeHTMLEventObj.GetButton();
				if ((button & 1) != 0)
				{
					mouseButtons |= MouseButtons.Left;
				}
				if ((button & 2) != 0)
				{
					mouseButtons |= MouseButtons.Right;
				}
				if ((button & 4) != 0)
				{
					mouseButtons |= MouseButtons.Middle;
				}
				return mouseButtons;
			}
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x060040C2 RID: 16578 RVA: 0x000E8F9B File Offset: 0x000E7F9B
		public Point ClientMousePosition
		{
			get
			{
				return new Point(this.NativeHTMLEventObj.GetClientX(), this.NativeHTMLEventObj.GetClientY());
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x060040C3 RID: 16579 RVA: 0x000E8FB8 File Offset: 0x000E7FB8
		public Point OffsetMousePosition
		{
			get
			{
				return new Point(this.NativeHTMLEventObj.GetOffsetX(), this.NativeHTMLEventObj.GetOffsetY());
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x000E8FD5 File Offset: 0x000E7FD5
		public Point MousePosition
		{
			get
			{
				return new Point(this.NativeHTMLEventObj.GetX(), this.NativeHTMLEventObj.GetY());
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x060040C5 RID: 16581 RVA: 0x000E8FF2 File Offset: 0x000E7FF2
		// (set) Token: 0x060040C6 RID: 16582 RVA: 0x000E9002 File Offset: 0x000E8002
		public bool BubbleEvent
		{
			get
			{
				return !this.NativeHTMLEventObj.GetCancelBubble();
			}
			set
			{
				this.NativeHTMLEventObj.SetCancelBubble(!value);
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x060040C7 RID: 16583 RVA: 0x000E9013 File Offset: 0x000E8013
		public int KeyPressedCode
		{
			get
			{
				return this.NativeHTMLEventObj.GetKeyCode();
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x060040C8 RID: 16584 RVA: 0x000E9020 File Offset: 0x000E8020
		public bool AltKeyPressed
		{
			get
			{
				return this.NativeHTMLEventObj.GetAltKey();
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x060040C9 RID: 16585 RVA: 0x000E902D File Offset: 0x000E802D
		public bool CtrlKeyPressed
		{
			get
			{
				return this.NativeHTMLEventObj.GetCtrlKey();
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x060040CA RID: 16586 RVA: 0x000E903A File Offset: 0x000E803A
		public bool ShiftKeyPressed
		{
			get
			{
				return this.NativeHTMLEventObj.GetShiftKey();
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x060040CB RID: 16587 RVA: 0x000E9047 File Offset: 0x000E8047
		public string EventType
		{
			get
			{
				return this.NativeHTMLEventObj.GetEventType();
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x060040CC RID: 16588 RVA: 0x000E9054 File Offset: 0x000E8054
		// (set) Token: 0x060040CD RID: 16589 RVA: 0x000E9078 File Offset: 0x000E8078
		public bool ReturnValue
		{
			get
			{
				object returnValue = this.NativeHTMLEventObj.GetReturnValue();
				return returnValue == null || (bool)returnValue;
			}
			set
			{
				object obj = value;
				this.NativeHTMLEventObj.SetReturnValue(obj);
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x060040CE RID: 16590 RVA: 0x000E9098 File Offset: 0x000E8098
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public HtmlElement FromElement
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement fromElement = this.NativeHTMLEventObj.GetFromElement();
				if (fromElement != null)
				{
					return new HtmlElement(this.shimManager, fromElement);
				}
				return null;
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x060040CF RID: 16591 RVA: 0x000E90C4 File Offset: 0x000E80C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public HtmlElement ToElement
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement toElement = this.NativeHTMLEventObj.GetToElement();
				if (toElement != null)
				{
					return new HtmlElement(this.shimManager, toElement);
				}
				return null;
			}
		}

		// Token: 0x04001F62 RID: 8034
		private UnsafeNativeMethods.IHTMLEventObj htmlEventObj;

		// Token: 0x04001F63 RID: 8035
		private HtmlShimManager shimManager;
	}
}
