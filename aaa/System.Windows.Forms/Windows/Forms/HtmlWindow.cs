using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200043B RID: 1083
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class HtmlWindow
	{
		// Token: 0x060040FA RID: 16634 RVA: 0x000E980C File Offset: 0x000E880C
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		internal HtmlWindow(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLWindow2 win)
		{
			this.htmlWindow2 = win;
			this.shimManager = shimManager;
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x060040FB RID: 16635 RVA: 0x000E9822 File Offset: 0x000E8822
		internal UnsafeNativeMethods.IHTMLWindow2 NativeHtmlWindow
		{
			get
			{
				return this.htmlWindow2;
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x060040FC RID: 16636 RVA: 0x000E982A File Offset: 0x000E882A
		private HtmlShimManager ShimManager
		{
			get
			{
				return this.shimManager;
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x060040FD RID: 16637 RVA: 0x000E9834 File Offset: 0x000E8834
		private HtmlWindow.HtmlWindowShim WindowShim
		{
			get
			{
				if (this.ShimManager != null)
				{
					HtmlWindow.HtmlWindowShim htmlWindowShim = this.ShimManager.GetWindowShim(this);
					if (htmlWindowShim == null)
					{
						this.shimManager.AddWindowShim(this);
						htmlWindowShim = this.ShimManager.GetWindowShim(this);
					}
					return htmlWindowShim;
				}
				return null;
			}
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x060040FE RID: 16638 RVA: 0x000E9878 File Offset: 0x000E8878
		public HtmlDocument Document
		{
			get
			{
				UnsafeNativeMethods.IHTMLDocument ihtmldocument = this.NativeHtmlWindow.GetDocument() as UnsafeNativeMethods.IHTMLDocument;
				if (ihtmldocument == null)
				{
					return null;
				}
				return new HtmlDocument(this.ShimManager, ihtmldocument);
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x060040FF RID: 16639 RVA: 0x000E98A7 File Offset: 0x000E88A7
		public object DomWindow
		{
			get
			{
				return this.NativeHtmlWindow;
			}
		}

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x06004100 RID: 16640 RVA: 0x000E98B0 File Offset: 0x000E88B0
		public HtmlWindowCollection Frames
		{
			get
			{
				UnsafeNativeMethods.IHTMLFramesCollection2 frames = this.NativeHtmlWindow.GetFrames();
				if (frames == null)
				{
					return null;
				}
				return new HtmlWindowCollection(this.ShimManager, frames);
			}
		}

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06004101 RID: 16641 RVA: 0x000E98DC File Offset: 0x000E88DC
		public HtmlHistory History
		{
			get
			{
				UnsafeNativeMethods.IOmHistory history = this.NativeHtmlWindow.GetHistory();
				if (history == null)
				{
					return null;
				}
				return new HtmlHistory(history);
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06004102 RID: 16642 RVA: 0x000E9900 File Offset: 0x000E8900
		public bool IsClosed
		{
			get
			{
				return this.NativeHtmlWindow.GetClosed();
			}
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06004103 RID: 16643 RVA: 0x000E990D File Offset: 0x000E890D
		// (set) Token: 0x06004104 RID: 16644 RVA: 0x000E991A File Offset: 0x000E891A
		public string Name
		{
			get
			{
				return this.NativeHtmlWindow.GetName();
			}
			set
			{
				this.NativeHtmlWindow.SetName(value);
			}
		}

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06004105 RID: 16645 RVA: 0x000E9928 File Offset: 0x000E8928
		public HtmlWindow Opener
		{
			get
			{
				UnsafeNativeMethods.IHTMLWindow2 ihtmlwindow = this.NativeHtmlWindow.GetOpener() as UnsafeNativeMethods.IHTMLWindow2;
				if (ihtmlwindow == null)
				{
					return null;
				}
				return new HtmlWindow(this.ShimManager, ihtmlwindow);
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06004106 RID: 16646 RVA: 0x000E9958 File Offset: 0x000E8958
		public HtmlWindow Parent
		{
			get
			{
				UnsafeNativeMethods.IHTMLWindow2 parent = this.NativeHtmlWindow.GetParent();
				if (parent == null)
				{
					return null;
				}
				return new HtmlWindow(this.ShimManager, parent);
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06004107 RID: 16647 RVA: 0x000E9982 File Offset: 0x000E8982
		public Point Position
		{
			get
			{
				return new Point(((UnsafeNativeMethods.IHTMLWindow3)this.NativeHtmlWindow).GetScreenLeft(), ((UnsafeNativeMethods.IHTMLWindow3)this.NativeHtmlWindow).GetScreenTop());
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06004108 RID: 16648 RVA: 0x000E99AC File Offset: 0x000E89AC
		// (set) Token: 0x06004109 RID: 16649 RVA: 0x000E99DB File Offset: 0x000E89DB
		public Size Size
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement body = this.NativeHtmlWindow.GetDocument().GetBody();
				return new Size(body.GetOffsetWidth(), body.GetOffsetHeight());
			}
			set
			{
				this.ResizeTo(value.Width, value.Height);
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x0600410A RID: 16650 RVA: 0x000E99F1 File Offset: 0x000E89F1
		// (set) Token: 0x0600410B RID: 16651 RVA: 0x000E99FE File Offset: 0x000E89FE
		public string StatusBarText
		{
			get
			{
				return this.NativeHtmlWindow.GetStatus();
			}
			set
			{
				this.NativeHtmlWindow.SetStatus(value);
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x0600410C RID: 16652 RVA: 0x000E9A0C File Offset: 0x000E8A0C
		public Uri Url
		{
			get
			{
				UnsafeNativeMethods.IHTMLLocation location = this.NativeHtmlWindow.GetLocation();
				string text = ((location == null) ? "" : location.GetHref());
				if (!string.IsNullOrEmpty(text))
				{
					return new Uri(text);
				}
				return null;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x0600410D RID: 16653 RVA: 0x000E9A48 File Offset: 0x000E8A48
		public HtmlElement WindowFrameElement
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement ihtmlelement = ((UnsafeNativeMethods.IHTMLWindow4)this.NativeHtmlWindow).frameElement() as UnsafeNativeMethods.IHTMLElement;
				if (ihtmlelement == null)
				{
					return null;
				}
				return new HtmlElement(this.ShimManager, ihtmlelement);
			}
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x000E9A7C File Offset: 0x000E8A7C
		public void Alert(string message)
		{
			this.NativeHtmlWindow.Alert(message);
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x000E9A8A File Offset: 0x000E8A8A
		public void AttachEventHandler(string eventName, EventHandler eventHandler)
		{
			this.WindowShim.AttachEventHandler(eventName, eventHandler);
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x000E9A99 File Offset: 0x000E8A99
		public void Close()
		{
			this.NativeHtmlWindow.Close();
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x000E9AA6 File Offset: 0x000E8AA6
		public bool Confirm(string message)
		{
			return this.NativeHtmlWindow.Confirm(message);
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x000E9AB4 File Offset: 0x000E8AB4
		public void DetachEventHandler(string eventName, EventHandler eventHandler)
		{
			this.WindowShim.DetachEventHandler(eventName, eventHandler);
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x000E9AC3 File Offset: 0x000E8AC3
		public void Focus()
		{
			this.NativeHtmlWindow.Focus();
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x000E9AD0 File Offset: 0x000E8AD0
		public void MoveTo(int x, int y)
		{
			this.NativeHtmlWindow.MoveTo(x, y);
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x000E9ADF File Offset: 0x000E8ADF
		public void MoveTo(Point point)
		{
			this.NativeHtmlWindow.MoveTo(point.X, point.Y);
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x000E9AFA File Offset: 0x000E8AFA
		public void Navigate(Uri url)
		{
			this.NativeHtmlWindow.Navigate(url.ToString());
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x000E9B0D File Offset: 0x000E8B0D
		public void Navigate(string urlString)
		{
			this.NativeHtmlWindow.Navigate(urlString);
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x000E9B1C File Offset: 0x000E8B1C
		public HtmlWindow Open(string urlString, string target, string windowOptions, bool replaceEntry)
		{
			UnsafeNativeMethods.IHTMLWindow2 ihtmlwindow = this.NativeHtmlWindow.Open(urlString, target, windowOptions, replaceEntry);
			if (ihtmlwindow == null)
			{
				return null;
			}
			return new HtmlWindow(this.ShimManager, ihtmlwindow);
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x000E9B4B File Offset: 0x000E8B4B
		public HtmlWindow Open(Uri url, string target, string windowOptions, bool replaceEntry)
		{
			return this.Open(url.ToString(), target, windowOptions, replaceEntry);
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x000E9B60 File Offset: 0x000E8B60
		public HtmlWindow OpenNew(string urlString, string windowOptions)
		{
			UnsafeNativeMethods.IHTMLWindow2 ihtmlwindow = this.NativeHtmlWindow.Open(urlString, "_blank", windowOptions, true);
			if (ihtmlwindow == null)
			{
				return null;
			}
			return new HtmlWindow(this.ShimManager, ihtmlwindow);
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x000E9B92 File Offset: 0x000E8B92
		public HtmlWindow OpenNew(Uri url, string windowOptions)
		{
			return this.OpenNew(url.ToString(), windowOptions);
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x000E9BA1 File Offset: 0x000E8BA1
		public string Prompt(string message, string defaultInputValue)
		{
			return this.NativeHtmlWindow.Prompt(message, defaultInputValue).ToString();
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x000E9BB5 File Offset: 0x000E8BB5
		public void RemoveFocus()
		{
			this.NativeHtmlWindow.Blur();
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x000E9BC2 File Offset: 0x000E8BC2
		public void ResizeTo(int width, int height)
		{
			this.NativeHtmlWindow.ResizeTo(width, height);
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x000E9BD1 File Offset: 0x000E8BD1
		public void ResizeTo(Size size)
		{
			this.NativeHtmlWindow.ResizeTo(size.Width, size.Height);
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x000E9BEC File Offset: 0x000E8BEC
		public void ScrollTo(int x, int y)
		{
			this.NativeHtmlWindow.ScrollTo(x, y);
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x000E9BFB File Offset: 0x000E8BFB
		public void ScrollTo(Point point)
		{
			this.NativeHtmlWindow.ScrollTo(point.X, point.Y);
		}

		// Token: 0x1400024E RID: 590
		// (add) Token: 0x06004122 RID: 16674 RVA: 0x000E9C16 File Offset: 0x000E8C16
		// (remove) Token: 0x06004123 RID: 16675 RVA: 0x000E9C29 File Offset: 0x000E8C29
		public event HtmlElementErrorEventHandler Error
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventError, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventError, value);
			}
		}

		// Token: 0x1400024F RID: 591
		// (add) Token: 0x06004124 RID: 16676 RVA: 0x000E9C3C File Offset: 0x000E8C3C
		// (remove) Token: 0x06004125 RID: 16677 RVA: 0x000E9C4F File Offset: 0x000E8C4F
		public event HtmlElementEventHandler GotFocus
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventGotFocus, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventGotFocus, value);
			}
		}

		// Token: 0x14000250 RID: 592
		// (add) Token: 0x06004126 RID: 16678 RVA: 0x000E9C62 File Offset: 0x000E8C62
		// (remove) Token: 0x06004127 RID: 16679 RVA: 0x000E9C75 File Offset: 0x000E8C75
		public event HtmlElementEventHandler Load
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventLoad, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventLoad, value);
			}
		}

		// Token: 0x14000251 RID: 593
		// (add) Token: 0x06004128 RID: 16680 RVA: 0x000E9C88 File Offset: 0x000E8C88
		// (remove) Token: 0x06004129 RID: 16681 RVA: 0x000E9C9B File Offset: 0x000E8C9B
		public event HtmlElementEventHandler LostFocus
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventLostFocus, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventLostFocus, value);
			}
		}

		// Token: 0x14000252 RID: 594
		// (add) Token: 0x0600412A RID: 16682 RVA: 0x000E9CAE File Offset: 0x000E8CAE
		// (remove) Token: 0x0600412B RID: 16683 RVA: 0x000E9CC1 File Offset: 0x000E8CC1
		public event HtmlElementEventHandler Resize
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventResize, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventResize, value);
			}
		}

		// Token: 0x14000253 RID: 595
		// (add) Token: 0x0600412C RID: 16684 RVA: 0x000E9CD4 File Offset: 0x000E8CD4
		// (remove) Token: 0x0600412D RID: 16685 RVA: 0x000E9CE7 File Offset: 0x000E8CE7
		public event HtmlElementEventHandler Scroll
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventScroll, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventScroll, value);
			}
		}

		// Token: 0x14000254 RID: 596
		// (add) Token: 0x0600412E RID: 16686 RVA: 0x000E9CFA File Offset: 0x000E8CFA
		// (remove) Token: 0x0600412F RID: 16687 RVA: 0x000E9D0D File Offset: 0x000E8D0D
		public event HtmlElementEventHandler Unload
		{
			add
			{
				this.WindowShim.AddHandler(HtmlWindow.EventUnload, value);
			}
			remove
			{
				this.WindowShim.RemoveHandler(HtmlWindow.EventUnload, value);
			}
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x000E9D20 File Offset: 0x000E8D20
		public static bool operator ==(HtmlWindow left, HtmlWindow right)
		{
			if (object.ReferenceEquals(left, null) != object.ReferenceEquals(right, null))
			{
				return false;
			}
			if (object.ReferenceEquals(left, null))
			{
				return true;
			}
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			bool flag;
			try
			{
				intPtr = Marshal.GetIUnknownForObject(left.NativeHtmlWindow);
				intPtr2 = Marshal.GetIUnknownForObject(right.NativeHtmlWindow);
				flag = intPtr == intPtr2;
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.Release(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.Release(intPtr2);
				}
			}
			return flag;
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x000E9DB4 File Offset: 0x000E8DB4
		public static bool operator !=(HtmlWindow left, HtmlWindow right)
		{
			return !(left == right);
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x000E9DC0 File Offset: 0x000E8DC0
		public override int GetHashCode()
		{
			if (this.htmlWindow2 != null)
			{
				return this.htmlWindow2.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x000E9DD7 File Offset: 0x000E8DD7
		public override bool Equals(object obj)
		{
			return this == (HtmlWindow)obj;
		}

		// Token: 0x04001F6D RID: 8045
		internal static readonly object EventError = new object();

		// Token: 0x04001F6E RID: 8046
		internal static readonly object EventGotFocus = new object();

		// Token: 0x04001F6F RID: 8047
		internal static readonly object EventLoad = new object();

		// Token: 0x04001F70 RID: 8048
		internal static readonly object EventLostFocus = new object();

		// Token: 0x04001F71 RID: 8049
		internal static readonly object EventResize = new object();

		// Token: 0x04001F72 RID: 8050
		internal static readonly object EventScroll = new object();

		// Token: 0x04001F73 RID: 8051
		internal static readonly object EventUnload = new object();

		// Token: 0x04001F74 RID: 8052
		private HtmlShimManager shimManager;

		// Token: 0x04001F75 RID: 8053
		private UnsafeNativeMethods.IHTMLWindow2 htmlWindow2;

		// Token: 0x0200043C RID: 1084
		[ClassInterface(ClassInterfaceType.None)]
		private class HTMLWindowEvents2 : StandardOleMarshalObject, UnsafeNativeMethods.DHTMLWindowEvents2
		{
			// Token: 0x06004135 RID: 16693 RVA: 0x000E9E3B File Offset: 0x000E8E3B
			public HTMLWindowEvents2(HtmlWindow htmlWindow)
			{
				this.parent = htmlWindow;
			}

			// Token: 0x06004136 RID: 16694 RVA: 0x000E9E4A File Offset: 0x000E8E4A
			private void FireEvent(object key, EventArgs e)
			{
				if (this.parent != null)
				{
					this.parent.WindowShim.FireEvent(key, e);
				}
			}

			// Token: 0x06004137 RID: 16695 RVA: 0x000E9E6C File Offset: 0x000E8E6C
			public void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventGotFocus, htmlElementEventArgs);
			}

			// Token: 0x06004138 RID: 16696 RVA: 0x000E9E98 File Offset: 0x000E8E98
			public void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventLostFocus, htmlElementEventArgs);
			}

			// Token: 0x06004139 RID: 16697 RVA: 0x000E9EC4 File Offset: 0x000E8EC4
			public bool onerror(string description, string urlString, int line)
			{
				HtmlElementErrorEventArgs htmlElementErrorEventArgs = new HtmlElementErrorEventArgs(description, urlString, line);
				this.FireEvent(HtmlWindow.EventError, htmlElementErrorEventArgs);
				return htmlElementErrorEventArgs.Handled;
			}

			// Token: 0x0600413A RID: 16698 RVA: 0x000E9EEC File Offset: 0x000E8EEC
			public void onload(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventLoad, htmlElementEventArgs);
			}

			// Token: 0x0600413B RID: 16699 RVA: 0x000E9F18 File Offset: 0x000E8F18
			public void onunload(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventUnload, htmlElementEventArgs);
				if (this.parent != null)
				{
					this.parent.WindowShim.OnWindowUnload();
				}
			}

			// Token: 0x0600413C RID: 16700 RVA: 0x000E9F64 File Offset: 0x000E8F64
			public void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventScroll, htmlElementEventArgs);
			}

			// Token: 0x0600413D RID: 16701 RVA: 0x000E9F90 File Offset: 0x000E8F90
			public void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlWindow.EventResize, htmlElementEventArgs);
			}

			// Token: 0x0600413E RID: 16702 RVA: 0x000E9FBC File Offset: 0x000E8FBC
			public bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x0600413F RID: 16703 RVA: 0x000E9FE1 File Offset: 0x000E8FE1
			public void onbeforeunload(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004140 RID: 16704 RVA: 0x000E9FE3 File Offset: 0x000E8FE3
			public void onbeforeprint(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004141 RID: 16705 RVA: 0x000E9FE5 File Offset: 0x000E8FE5
			public void onafterprint(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x04001F76 RID: 8054
			private HtmlWindow parent;
		}

		// Token: 0x0200043D RID: 1085
		internal class HtmlWindowShim : HtmlShim
		{
			// Token: 0x06004142 RID: 16706 RVA: 0x000E9FE7 File Offset: 0x000E8FE7
			public HtmlWindowShim(HtmlWindow window)
			{
				this.htmlWindow = window;
			}

			// Token: 0x17000CA1 RID: 3233
			// (get) Token: 0x06004143 RID: 16707 RVA: 0x000E9FF6 File Offset: 0x000E8FF6
			public UnsafeNativeMethods.IHTMLWindow2 NativeHtmlWindow
			{
				get
				{
					return this.htmlWindow.NativeHtmlWindow;
				}
			}

			// Token: 0x17000CA2 RID: 3234
			// (get) Token: 0x06004144 RID: 16708 RVA: 0x000EA003 File Offset: 0x000E9003
			public override UnsafeNativeMethods.IHTMLWindow2 AssociatedWindow
			{
				get
				{
					return this.htmlWindow.NativeHtmlWindow;
				}
			}

			// Token: 0x06004145 RID: 16709 RVA: 0x000EA010 File Offset: 0x000E9010
			public override void AttachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy htmlToClrEventProxy = base.AddEventProxy(eventName, eventHandler);
				((UnsafeNativeMethods.IHTMLWindow3)this.NativeHtmlWindow).AttachEvent(eventName, htmlToClrEventProxy);
			}

			// Token: 0x06004146 RID: 16710 RVA: 0x000EA03C File Offset: 0x000E903C
			public override void ConnectToEvents()
			{
				if (this.cookie == null || !this.cookie.Connected)
				{
					this.cookie = new AxHost.ConnectionPointCookie(this.NativeHtmlWindow, new HtmlWindow.HTMLWindowEvents2(this.htmlWindow), typeof(UnsafeNativeMethods.DHTMLWindowEvents2), false);
					if (!this.cookie.Connected)
					{
						this.cookie = null;
					}
				}
			}

			// Token: 0x06004147 RID: 16711 RVA: 0x000EA09C File Offset: 0x000E909C
			public override void DetachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy htmlToClrEventProxy = base.RemoveEventProxy(eventHandler);
				if (htmlToClrEventProxy != null)
				{
					((UnsafeNativeMethods.IHTMLWindow3)this.NativeHtmlWindow).DetachEvent(eventName, htmlToClrEventProxy);
				}
			}

			// Token: 0x06004148 RID: 16712 RVA: 0x000EA0C6 File Offset: 0x000E90C6
			public override void DisconnectFromEvents()
			{
				if (this.cookie != null)
				{
					this.cookie.Disconnect();
					this.cookie = null;
				}
			}

			// Token: 0x06004149 RID: 16713 RVA: 0x000EA0E2 File Offset: 0x000E90E2
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
				if (disposing)
				{
					if (this.htmlWindow != null && this.htmlWindow.NativeHtmlWindow != null)
					{
						Marshal.FinalReleaseComObject(this.htmlWindow.NativeHtmlWindow);
					}
					this.htmlWindow = null;
				}
			}

			// Token: 0x0600414A RID: 16714 RVA: 0x000EA121 File Offset: 0x000E9121
			protected override object GetEventSender()
			{
				return this.htmlWindow;
			}

			// Token: 0x0600414B RID: 16715 RVA: 0x000EA129 File Offset: 0x000E9129
			public void OnWindowUnload()
			{
				if (this.htmlWindow != null)
				{
					this.htmlWindow.ShimManager.OnWindowUnloaded(this.htmlWindow);
				}
			}

			// Token: 0x04001F77 RID: 8055
			private AxHost.ConnectionPointCookie cookie;

			// Token: 0x04001F78 RID: 8056
			private HtmlWindow htmlWindow;
		}
	}
}
