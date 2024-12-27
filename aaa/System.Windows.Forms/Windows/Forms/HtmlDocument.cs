using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200042A RID: 1066
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class HtmlDocument
	{
		// Token: 0x06003F65 RID: 16229 RVA: 0x000E6128 File Offset: 0x000E5128
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		internal HtmlDocument(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLDocument doc)
		{
			this.htmlDocument2 = (UnsafeNativeMethods.IHTMLDocument2)doc;
			this.shimManager = shimManager;
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06003F66 RID: 16230 RVA: 0x000E6143 File Offset: 0x000E5143
		internal UnsafeNativeMethods.IHTMLDocument2 NativeHtmlDocument2
		{
			get
			{
				return this.htmlDocument2;
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x06003F67 RID: 16231 RVA: 0x000E614C File Offset: 0x000E514C
		private HtmlDocument.HtmlDocumentShim DocumentShim
		{
			get
			{
				if (this.ShimManager != null)
				{
					HtmlDocument.HtmlDocumentShim htmlDocumentShim = this.ShimManager.GetDocumentShim(this);
					if (htmlDocumentShim == null)
					{
						this.shimManager.AddDocumentShim(this);
						htmlDocumentShim = this.ShimManager.GetDocumentShim(this);
					}
					return htmlDocumentShim;
				}
				return null;
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x06003F68 RID: 16232 RVA: 0x000E618D File Offset: 0x000E518D
		private HtmlShimManager ShimManager
		{
			get
			{
				return this.shimManager;
			}
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06003F69 RID: 16233 RVA: 0x000E6198 File Offset: 0x000E5198
		public HtmlElement ActiveElement
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement activeElement = this.NativeHtmlDocument2.GetActiveElement();
				if (activeElement == null)
				{
					return null;
				}
				return new HtmlElement(this.ShimManager, activeElement);
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06003F6A RID: 16234 RVA: 0x000E61C4 File Offset: 0x000E51C4
		public HtmlElement Body
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement body = this.NativeHtmlDocument2.GetBody();
				if (body == null)
				{
					return null;
				}
				return new HtmlElement(this.ShimManager, body);
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06003F6B RID: 16235 RVA: 0x000E61EE File Offset: 0x000E51EE
		// (set) Token: 0x06003F6C RID: 16236 RVA: 0x000E61FC File Offset: 0x000E51FC
		public string Domain
		{
			get
			{
				return this.NativeHtmlDocument2.GetDomain();
			}
			set
			{
				try
				{
					this.NativeHtmlDocument2.SetDomain(value);
				}
				catch (ArgumentException)
				{
					throw new ArgumentException(SR.GetString("HtmlDocumentInvalidDomain"));
				}
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06003F6D RID: 16237 RVA: 0x000E6238 File Offset: 0x000E5238
		// (set) Token: 0x06003F6E RID: 16238 RVA: 0x000E6245 File Offset: 0x000E5245
		public string Title
		{
			get
			{
				return this.NativeHtmlDocument2.GetTitle();
			}
			set
			{
				this.NativeHtmlDocument2.SetTitle(value);
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06003F6F RID: 16239 RVA: 0x000E6254 File Offset: 0x000E5254
		public Uri Url
		{
			get
			{
				UnsafeNativeMethods.IHTMLLocation location = this.NativeHtmlDocument2.GetLocation();
				string text = ((location == null) ? "" : location.GetHref());
				if (!string.IsNullOrEmpty(text))
				{
					return new Uri(text);
				}
				return null;
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06003F70 RID: 16240 RVA: 0x000E6290 File Offset: 0x000E5290
		public HtmlWindow Window
		{
			get
			{
				UnsafeNativeMethods.IHTMLWindow2 parentWindow = this.NativeHtmlDocument2.GetParentWindow();
				if (parentWindow == null)
				{
					return null;
				}
				return new HtmlWindow(this.ShimManager, parentWindow);
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06003F71 RID: 16241 RVA: 0x000E62BC File Offset: 0x000E52BC
		// (set) Token: 0x06003F72 RID: 16242 RVA: 0x000E6304 File Offset: 0x000E5304
		public Color BackColor
		{
			get
			{
				Color color = Color.Empty;
				try
				{
					color = this.ColorFromObject(this.NativeHtmlDocument2.GetBgColor());
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return color;
			}
			set
			{
				int num = ((int)value.R << 16) | ((int)value.G << 8) | (int)value.B;
				this.NativeHtmlDocument2.SetBgColor(num);
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06003F73 RID: 16243 RVA: 0x000E6340 File Offset: 0x000E5340
		// (set) Token: 0x06003F74 RID: 16244 RVA: 0x000E6388 File Offset: 0x000E5388
		public Color ForeColor
		{
			get
			{
				Color color = Color.Empty;
				try
				{
					color = this.ColorFromObject(this.NativeHtmlDocument2.GetFgColor());
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return color;
			}
			set
			{
				int num = ((int)value.R << 16) | ((int)value.G << 8) | (int)value.B;
				this.NativeHtmlDocument2.SetFgColor(num);
			}
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06003F75 RID: 16245 RVA: 0x000E63C4 File Offset: 0x000E53C4
		// (set) Token: 0x06003F76 RID: 16246 RVA: 0x000E640C File Offset: 0x000E540C
		public Color LinkColor
		{
			get
			{
				Color color = Color.Empty;
				try
				{
					color = this.ColorFromObject(this.NativeHtmlDocument2.GetLinkColor());
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return color;
			}
			set
			{
				int num = ((int)value.R << 16) | ((int)value.G << 8) | (int)value.B;
				this.NativeHtmlDocument2.SetLinkColor(num);
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06003F77 RID: 16247 RVA: 0x000E6448 File Offset: 0x000E5448
		// (set) Token: 0x06003F78 RID: 16248 RVA: 0x000E6490 File Offset: 0x000E5490
		public Color ActiveLinkColor
		{
			get
			{
				Color color = Color.Empty;
				try
				{
					color = this.ColorFromObject(this.NativeHtmlDocument2.GetAlinkColor());
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return color;
			}
			set
			{
				int num = ((int)value.R << 16) | ((int)value.G << 8) | (int)value.B;
				this.NativeHtmlDocument2.SetAlinkColor(num);
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06003F79 RID: 16249 RVA: 0x000E64CC File Offset: 0x000E54CC
		// (set) Token: 0x06003F7A RID: 16250 RVA: 0x000E6514 File Offset: 0x000E5514
		public Color VisitedLinkColor
		{
			get
			{
				Color color = Color.Empty;
				try
				{
					color = this.ColorFromObject(this.NativeHtmlDocument2.GetVlinkColor());
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				return color;
			}
			set
			{
				int num = ((int)value.R << 16) | ((int)value.G << 8) | (int)value.B;
				this.NativeHtmlDocument2.SetVlinkColor(num);
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06003F7B RID: 16251 RVA: 0x000E654F File Offset: 0x000E554F
		public bool Focused
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLDocument4)this.NativeHtmlDocument2).HasFocus();
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06003F7C RID: 16252 RVA: 0x000E6561 File Offset: 0x000E5561
		public object DomDocument
		{
			get
			{
				return this.NativeHtmlDocument2;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06003F7D RID: 16253 RVA: 0x000E6569 File Offset: 0x000E5569
		// (set) Token: 0x06003F7E RID: 16254 RVA: 0x000E6576 File Offset: 0x000E5576
		public string Cookie
		{
			get
			{
				return this.NativeHtmlDocument2.GetCookie();
			}
			set
			{
				this.NativeHtmlDocument2.SetCookie(value);
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06003F7F RID: 16255 RVA: 0x000E6584 File Offset: 0x000E5584
		// (set) Token: 0x06003F80 RID: 16256 RVA: 0x000E65A0 File Offset: 0x000E55A0
		public bool RightToLeft
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).GetDir() == "rtl";
			}
			set
			{
				((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).SetDir(value ? "rtl" : "ltr");
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06003F81 RID: 16257 RVA: 0x000E65C1 File Offset: 0x000E55C1
		// (set) Token: 0x06003F82 RID: 16258 RVA: 0x000E65CE File Offset: 0x000E55CE
		public string Encoding
		{
			get
			{
				return this.NativeHtmlDocument2.GetCharset();
			}
			set
			{
				this.NativeHtmlDocument2.SetCharset(value);
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06003F83 RID: 16259 RVA: 0x000E65DC File Offset: 0x000E55DC
		public string DefaultEncoding
		{
			get
			{
				return this.NativeHtmlDocument2.GetDefaultCharset();
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06003F84 RID: 16260 RVA: 0x000E65EC File Offset: 0x000E55EC
		public HtmlElementCollection All
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection all = this.NativeHtmlDocument2.GetAll();
				if (all == null)
				{
					return new HtmlElementCollection(this.ShimManager);
				}
				return new HtmlElementCollection(this.ShimManager, all);
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06003F85 RID: 16261 RVA: 0x000E6620 File Offset: 0x000E5620
		public HtmlElementCollection Links
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection links = this.NativeHtmlDocument2.GetLinks();
				if (links == null)
				{
					return new HtmlElementCollection(this.ShimManager);
				}
				return new HtmlElementCollection(this.ShimManager, links);
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06003F86 RID: 16262 RVA: 0x000E6654 File Offset: 0x000E5654
		public HtmlElementCollection Images
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection images = this.NativeHtmlDocument2.GetImages();
				if (images == null)
				{
					return new HtmlElementCollection(this.ShimManager);
				}
				return new HtmlElementCollection(this.ShimManager, images);
			}
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06003F87 RID: 16263 RVA: 0x000E6688 File Offset: 0x000E5688
		public HtmlElementCollection Forms
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection forms = this.NativeHtmlDocument2.GetForms();
				if (forms == null)
				{
					return new HtmlElementCollection(this.ShimManager);
				}
				return new HtmlElementCollection(this.ShimManager, forms);
			}
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x000E66BC File Offset: 0x000E56BC
		public void Write(string text)
		{
			object[] array = new object[] { text };
			this.NativeHtmlDocument2.Write(array);
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x000E66E3 File Offset: 0x000E56E3
		public void ExecCommand(string command, bool showUI, object value)
		{
			this.NativeHtmlDocument2.ExecCommand(command, showUI, value);
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x000E66F4 File Offset: 0x000E56F4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void Focus()
		{
			((UnsafeNativeMethods.IHTMLDocument4)this.NativeHtmlDocument2).Focus();
			((UnsafeNativeMethods.IHTMLDocument4)this.NativeHtmlDocument2).Focus();
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x000E6718 File Offset: 0x000E5718
		public HtmlElement GetElementById(string id)
		{
			UnsafeNativeMethods.IHTMLElement elementById = ((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).GetElementById(id);
			if (elementById == null)
			{
				return null;
			}
			return new HtmlElement(this.ShimManager, elementById);
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x000E6748 File Offset: 0x000E5748
		public HtmlElement GetElementFromPoint(Point point)
		{
			UnsafeNativeMethods.IHTMLElement ihtmlelement = this.NativeHtmlDocument2.ElementFromPoint(point.X, point.Y);
			if (ihtmlelement == null)
			{
				return null;
			}
			return new HtmlElement(this.ShimManager, ihtmlelement);
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x000E6780 File Offset: 0x000E5780
		public HtmlElementCollection GetElementsByTagName(string tagName)
		{
			UnsafeNativeMethods.IHTMLElementCollection elementsByTagName = ((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).GetElementsByTagName(tagName);
			if (elementsByTagName == null)
			{
				return new HtmlElementCollection(this.ShimManager);
			}
			return new HtmlElementCollection(this.ShimManager, elementsByTagName);
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x000E67BC File Offset: 0x000E57BC
		public HtmlDocument OpenNew(bool replaceInHistory)
		{
			object obj = (replaceInHistory ? "replace" : "");
			object obj2 = null;
			object obj3 = this.NativeHtmlDocument2.Open("text/html", obj, obj2, obj2);
			UnsafeNativeMethods.IHTMLDocument ihtmldocument = obj3 as UnsafeNativeMethods.IHTMLDocument;
			if (ihtmldocument == null)
			{
				return null;
			}
			return new HtmlDocument(this.ShimManager, ihtmldocument);
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x000E6808 File Offset: 0x000E5808
		public HtmlElement CreateElement(string elementTag)
		{
			UnsafeNativeMethods.IHTMLElement ihtmlelement = this.NativeHtmlDocument2.CreateElement(elementTag);
			if (ihtmlelement == null)
			{
				return null;
			}
			return new HtmlElement(this.ShimManager, ihtmlelement);
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x000E6834 File Offset: 0x000E5834
		public object InvokeScript(string scriptName, object[] args)
		{
			object obj = null;
			NativeMethods.tagDISPPARAMS tagDISPPARAMS = new NativeMethods.tagDISPPARAMS();
			tagDISPPARAMS.rgvarg = IntPtr.Zero;
			try
			{
				UnsafeNativeMethods.IDispatch dispatch = this.NativeHtmlDocument2.GetScript() as UnsafeNativeMethods.IDispatch;
				if (dispatch != null)
				{
					Guid empty = Guid.Empty;
					string[] array = new string[] { scriptName };
					int[] array2 = new int[] { -1 };
					int idsOfNames = dispatch.GetIDsOfNames(ref empty, array, 1, SafeNativeMethods.GetThreadLCID(), array2);
					if (NativeMethods.Succeeded(idsOfNames) && array2[0] != -1)
					{
						if (args != null)
						{
							Array.Reverse(args);
						}
						tagDISPPARAMS.rgvarg = ((args == null) ? IntPtr.Zero : HtmlDocument.ArrayToVARIANTVector(args));
						tagDISPPARAMS.cArgs = ((args == null) ? 0 : args.Length);
						tagDISPPARAMS.rgdispidNamedArgs = IntPtr.Zero;
						tagDISPPARAMS.cNamedArgs = 0;
						object[] array3 = new object[1];
						if (dispatch.Invoke(array2[0], ref empty, SafeNativeMethods.GetThreadLCID(), 1, tagDISPPARAMS, array3, new NativeMethods.tagEXCEPINFO(), null) == 0)
						{
							obj = array3[0];
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
			finally
			{
				if (tagDISPPARAMS.rgvarg != IntPtr.Zero)
				{
					HtmlDocument.FreeVARIANTVector(tagDISPPARAMS.rgvarg, args.Length);
				}
			}
			return obj;
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x000E6978 File Offset: 0x000E5978
		public object InvokeScript(string scriptName)
		{
			return this.InvokeScript(scriptName, null);
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x000E6984 File Offset: 0x000E5984
		public void AttachEventHandler(string eventName, EventHandler eventHandler)
		{
			HtmlDocument.HtmlDocumentShim documentShim = this.DocumentShim;
			if (documentShim != null)
			{
				documentShim.AttachEventHandler(eventName, eventHandler);
			}
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x000E69A4 File Offset: 0x000E59A4
		public void DetachEventHandler(string eventName, EventHandler eventHandler)
		{
			HtmlDocument.HtmlDocumentShim documentShim = this.DocumentShim;
			if (documentShim != null)
			{
				documentShim.DetachEventHandler(eventName, eventHandler);
			}
		}

		// Token: 0x14000231 RID: 561
		// (add) Token: 0x06003F94 RID: 16276 RVA: 0x000E69C3 File Offset: 0x000E59C3
		// (remove) Token: 0x06003F95 RID: 16277 RVA: 0x000E69D6 File Offset: 0x000E59D6
		public event HtmlElementEventHandler Click
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventClick, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventClick, value);
			}
		}

		// Token: 0x14000232 RID: 562
		// (add) Token: 0x06003F96 RID: 16278 RVA: 0x000E69E9 File Offset: 0x000E59E9
		// (remove) Token: 0x06003F97 RID: 16279 RVA: 0x000E69FC File Offset: 0x000E59FC
		public event HtmlElementEventHandler ContextMenuShowing
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventContextMenuShowing, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventContextMenuShowing, value);
			}
		}

		// Token: 0x14000233 RID: 563
		// (add) Token: 0x06003F98 RID: 16280 RVA: 0x000E6A0F File Offset: 0x000E5A0F
		// (remove) Token: 0x06003F99 RID: 16281 RVA: 0x000E6A22 File Offset: 0x000E5A22
		public event HtmlElementEventHandler Focusing
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventFocusing, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventFocusing, value);
			}
		}

		// Token: 0x14000234 RID: 564
		// (add) Token: 0x06003F9A RID: 16282 RVA: 0x000E6A35 File Offset: 0x000E5A35
		// (remove) Token: 0x06003F9B RID: 16283 RVA: 0x000E6A48 File Offset: 0x000E5A48
		public event HtmlElementEventHandler LosingFocus
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventLosingFocus, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventLosingFocus, value);
			}
		}

		// Token: 0x14000235 RID: 565
		// (add) Token: 0x06003F9C RID: 16284 RVA: 0x000E6A5B File Offset: 0x000E5A5B
		// (remove) Token: 0x06003F9D RID: 16285 RVA: 0x000E6A6E File Offset: 0x000E5A6E
		public event HtmlElementEventHandler MouseDown
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventMouseDown, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventMouseDown, value);
			}
		}

		// Token: 0x14000236 RID: 566
		// (add) Token: 0x06003F9E RID: 16286 RVA: 0x000E6A81 File Offset: 0x000E5A81
		// (remove) Token: 0x06003F9F RID: 16287 RVA: 0x000E6A94 File Offset: 0x000E5A94
		public event HtmlElementEventHandler MouseLeave
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventMouseLeave, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventMouseLeave, value);
			}
		}

		// Token: 0x14000237 RID: 567
		// (add) Token: 0x06003FA0 RID: 16288 RVA: 0x000E6AA7 File Offset: 0x000E5AA7
		// (remove) Token: 0x06003FA1 RID: 16289 RVA: 0x000E6ABA File Offset: 0x000E5ABA
		public event HtmlElementEventHandler MouseMove
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventMouseMove, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventMouseMove, value);
			}
		}

		// Token: 0x14000238 RID: 568
		// (add) Token: 0x06003FA2 RID: 16290 RVA: 0x000E6ACD File Offset: 0x000E5ACD
		// (remove) Token: 0x06003FA3 RID: 16291 RVA: 0x000E6AE0 File Offset: 0x000E5AE0
		public event HtmlElementEventHandler MouseOver
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventMouseOver, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventMouseOver, value);
			}
		}

		// Token: 0x14000239 RID: 569
		// (add) Token: 0x06003FA4 RID: 16292 RVA: 0x000E6AF3 File Offset: 0x000E5AF3
		// (remove) Token: 0x06003FA5 RID: 16293 RVA: 0x000E6B06 File Offset: 0x000E5B06
		public event HtmlElementEventHandler MouseUp
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventMouseUp, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventMouseUp, value);
			}
		}

		// Token: 0x1400023A RID: 570
		// (add) Token: 0x06003FA6 RID: 16294 RVA: 0x000E6B19 File Offset: 0x000E5B19
		// (remove) Token: 0x06003FA7 RID: 16295 RVA: 0x000E6B2C File Offset: 0x000E5B2C
		public event HtmlElementEventHandler Stop
		{
			add
			{
				this.DocumentShim.AddHandler(HtmlDocument.EventStop, value);
			}
			remove
			{
				this.DocumentShim.RemoveHandler(HtmlDocument.EventStop, value);
			}
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x000E6B40 File Offset: 0x000E5B40
		internal unsafe static IntPtr ArrayToVARIANTVector(object[] args)
		{
			int num = args.Length;
			IntPtr intPtr = Marshal.AllocCoTaskMem(num * HtmlDocument.VariantSize);
			byte* ptr = (byte*)(void*)intPtr;
			for (int i = 0; i < num; i++)
			{
				Marshal.GetNativeVariantForObject(args[i], (IntPtr)((void*)(ptr + (IntPtr)HtmlDocument.VariantSize * (IntPtr)i)));
			}
			return intPtr;
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x000E6B8C File Offset: 0x000E5B8C
		internal unsafe static void FreeVARIANTVector(IntPtr mem, int len)
		{
			byte* ptr = (byte*)(void*)mem;
			for (int i = 0; i < len; i++)
			{
				SafeNativeMethods.VariantClear(new HandleRef(null, (IntPtr)((void*)(ptr + (IntPtr)HtmlDocument.VariantSize * (IntPtr)i))));
			}
			Marshal.FreeCoTaskMem(mem);
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x000E6BCC File Offset: 0x000E5BCC
		private Color ColorFromObject(object oColor)
		{
			try
			{
				if (oColor is string)
				{
					string text = oColor as string;
					int num = text.IndexOf('#');
					if (num >= 0)
					{
						string text2 = text.Substring(num + 1);
						return Color.FromArgb(255, Color.FromArgb(int.Parse(text2, NumberStyles.HexNumber, CultureInfo.InvariantCulture)));
					}
					return Color.FromName(text);
				}
				else if (oColor is int)
				{
					return Color.FromArgb(255, Color.FromArgb((int)oColor));
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
			return Color.Empty;
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x000E6C74 File Offset: 0x000E5C74
		public static bool operator ==(HtmlDocument left, HtmlDocument right)
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
				intPtr = Marshal.GetIUnknownForObject(left.NativeHtmlDocument2);
				intPtr2 = Marshal.GetIUnknownForObject(right.NativeHtmlDocument2);
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

		// Token: 0x06003FAC RID: 16300 RVA: 0x000E6D08 File Offset: 0x000E5D08
		public static bool operator !=(HtmlDocument left, HtmlDocument right)
		{
			return !(left == right);
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x000E6D14 File Offset: 0x000E5D14
		public override int GetHashCode()
		{
			if (this.htmlDocument2 != null)
			{
				return this.htmlDocument2.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x000E6D2B File Offset: 0x000E5D2B
		public override bool Equals(object obj)
		{
			return this == (HtmlDocument)obj;
		}

		// Token: 0x04001F25 RID: 7973
		internal static object EventClick = new object();

		// Token: 0x04001F26 RID: 7974
		internal static object EventContextMenuShowing = new object();

		// Token: 0x04001F27 RID: 7975
		internal static object EventFocusing = new object();

		// Token: 0x04001F28 RID: 7976
		internal static object EventLosingFocus = new object();

		// Token: 0x04001F29 RID: 7977
		internal static object EventMouseDown = new object();

		// Token: 0x04001F2A RID: 7978
		internal static object EventMouseLeave = new object();

		// Token: 0x04001F2B RID: 7979
		internal static object EventMouseMove = new object();

		// Token: 0x04001F2C RID: 7980
		internal static object EventMouseOver = new object();

		// Token: 0x04001F2D RID: 7981
		internal static object EventMouseUp = new object();

		// Token: 0x04001F2E RID: 7982
		internal static object EventStop = new object();

		// Token: 0x04001F2F RID: 7983
		private UnsafeNativeMethods.IHTMLDocument2 htmlDocument2;

		// Token: 0x04001F30 RID: 7984
		private HtmlShimManager shimManager;

		// Token: 0x04001F31 RID: 7985
		private static readonly int VariantSize = (int)Marshal.OffsetOf(typeof(HtmlDocument.FindSizeOfVariant), "b");

		// Token: 0x0200042B RID: 1067
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct FindSizeOfVariant
		{
			// Token: 0x04001F32 RID: 7986
			[MarshalAs(UnmanagedType.Struct)]
			public object var;

			// Token: 0x04001F33 RID: 7987
			public byte b;
		}

		// Token: 0x0200042D RID: 1069
		internal class HtmlDocumentShim : HtmlShim
		{
			// Token: 0x06003FC2 RID: 16322 RVA: 0x000E7004 File Offset: 0x000E6004
			internal HtmlDocumentShim(HtmlDocument htmlDocument)
			{
				this.htmlDocument = htmlDocument;
				if (this.htmlDocument != null)
				{
					HtmlWindow window = htmlDocument.Window;
					if (window != null)
					{
						this.associatedWindow = window.NativeHtmlWindow;
					}
				}
			}

			// Token: 0x17000C53 RID: 3155
			// (get) Token: 0x06003FC3 RID: 16323 RVA: 0x000E7048 File Offset: 0x000E6048
			public override UnsafeNativeMethods.IHTMLWindow2 AssociatedWindow
			{
				get
				{
					return this.associatedWindow;
				}
			}

			// Token: 0x17000C54 RID: 3156
			// (get) Token: 0x06003FC4 RID: 16324 RVA: 0x000E7050 File Offset: 0x000E6050
			public UnsafeNativeMethods.IHTMLDocument2 NativeHtmlDocument2
			{
				get
				{
					return this.htmlDocument.NativeHtmlDocument2;
				}
			}

			// Token: 0x17000C55 RID: 3157
			// (get) Token: 0x06003FC5 RID: 16325 RVA: 0x000E705D File Offset: 0x000E605D
			internal HtmlDocument Document
			{
				get
				{
					return this.htmlDocument;
				}
			}

			// Token: 0x06003FC6 RID: 16326 RVA: 0x000E7068 File Offset: 0x000E6068
			public override void AttachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy htmlToClrEventProxy = base.AddEventProxy(eventName, eventHandler);
				((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).AttachEvent(eventName, htmlToClrEventProxy);
			}

			// Token: 0x06003FC7 RID: 16327 RVA: 0x000E7094 File Offset: 0x000E6094
			public override void DetachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy htmlToClrEventProxy = base.RemoveEventProxy(eventHandler);
				if (htmlToClrEventProxy != null)
				{
					((UnsafeNativeMethods.IHTMLDocument3)this.NativeHtmlDocument2).DetachEvent(eventName, htmlToClrEventProxy);
				}
			}

			// Token: 0x06003FC8 RID: 16328 RVA: 0x000E70C0 File Offset: 0x000E60C0
			public override void ConnectToEvents()
			{
				if (this.cookie == null || !this.cookie.Connected)
				{
					this.cookie = new AxHost.ConnectionPointCookie(this.NativeHtmlDocument2, new HtmlDocument.HTMLDocumentEvents2(this.htmlDocument), typeof(UnsafeNativeMethods.DHTMLDocumentEvents2), false);
					if (!this.cookie.Connected)
					{
						this.cookie = null;
					}
				}
			}

			// Token: 0x06003FC9 RID: 16329 RVA: 0x000E711D File Offset: 0x000E611D
			public override void DisconnectFromEvents()
			{
				if (this.cookie != null)
				{
					this.cookie.Disconnect();
					this.cookie = null;
				}
			}

			// Token: 0x06003FCA RID: 16330 RVA: 0x000E7139 File Offset: 0x000E6139
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
				if (disposing)
				{
					if (this.htmlDocument != null)
					{
						Marshal.FinalReleaseComObject(this.htmlDocument.NativeHtmlDocument2);
					}
					this.htmlDocument = null;
				}
			}

			// Token: 0x06003FCB RID: 16331 RVA: 0x000E716B File Offset: 0x000E616B
			protected override object GetEventSender()
			{
				return this.htmlDocument;
			}

			// Token: 0x04001F37 RID: 7991
			private AxHost.ConnectionPointCookie cookie;

			// Token: 0x04001F38 RID: 7992
			private HtmlDocument htmlDocument;

			// Token: 0x04001F39 RID: 7993
			private UnsafeNativeMethods.IHTMLWindow2 associatedWindow;
		}

		// Token: 0x0200042E RID: 1070
		[ClassInterface(ClassInterfaceType.None)]
		private class HTMLDocumentEvents2 : StandardOleMarshalObject, UnsafeNativeMethods.DHTMLDocumentEvents2
		{
			// Token: 0x06003FCC RID: 16332 RVA: 0x000E7173 File Offset: 0x000E6173
			public HTMLDocumentEvents2(HtmlDocument htmlDocument)
			{
				this.parent = htmlDocument;
			}

			// Token: 0x06003FCD RID: 16333 RVA: 0x000E7182 File Offset: 0x000E6182
			private void FireEvent(object key, EventArgs e)
			{
				if (this.parent != null)
				{
					this.parent.DocumentShim.FireEvent(key, e);
				}
			}

			// Token: 0x06003FCE RID: 16334 RVA: 0x000E71A4 File Offset: 0x000E61A4
			public bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventClick, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FCF RID: 16335 RVA: 0x000E71D8 File Offset: 0x000E61D8
			public bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventContextMenuShowing, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FD0 RID: 16336 RVA: 0x000E720C File Offset: 0x000E620C
			public void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventFocusing, htmlElementEventArgs);
			}

			// Token: 0x06003FD1 RID: 16337 RVA: 0x000E7238 File Offset: 0x000E6238
			public void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventLosingFocus, htmlElementEventArgs);
			}

			// Token: 0x06003FD2 RID: 16338 RVA: 0x000E7264 File Offset: 0x000E6264
			public void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventMouseMove, htmlElementEventArgs);
			}

			// Token: 0x06003FD3 RID: 16339 RVA: 0x000E7290 File Offset: 0x000E6290
			public void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventMouseDown, htmlElementEventArgs);
			}

			// Token: 0x06003FD4 RID: 16340 RVA: 0x000E72BC File Offset: 0x000E62BC
			public void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventMouseLeave, htmlElementEventArgs);
			}

			// Token: 0x06003FD5 RID: 16341 RVA: 0x000E72E8 File Offset: 0x000E62E8
			public void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventMouseOver, htmlElementEventArgs);
			}

			// Token: 0x06003FD6 RID: 16342 RVA: 0x000E7314 File Offset: 0x000E6314
			public void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventMouseUp, htmlElementEventArgs);
			}

			// Token: 0x06003FD7 RID: 16343 RVA: 0x000E7340 File Offset: 0x000E6340
			public bool onstop(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlDocument.EventStop, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FD8 RID: 16344 RVA: 0x000E7374 File Offset: 0x000E6374
			public bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FD9 RID: 16345 RVA: 0x000E739C File Offset: 0x000E639C
			public bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FDA RID: 16346 RVA: 0x000E73C1 File Offset: 0x000E63C1
			public void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FDB RID: 16347 RVA: 0x000E73C3 File Offset: 0x000E63C3
			public void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FDC RID: 16348 RVA: 0x000E73C8 File Offset: 0x000E63C8
			public bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FDD RID: 16349 RVA: 0x000E73ED File Offset: 0x000E63ED
			public void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FDE RID: 16350 RVA: 0x000E73F0 File Offset: 0x000E63F0
			public bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FDF RID: 16351 RVA: 0x000E7415 File Offset: 0x000E6415
			public void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FE0 RID: 16352 RVA: 0x000E7418 File Offset: 0x000E6418
			public bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FE1 RID: 16353 RVA: 0x000E743D File Offset: 0x000E643D
			public void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FE2 RID: 16354 RVA: 0x000E7440 File Offset: 0x000E6440
			public bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FE3 RID: 16355 RVA: 0x000E7468 File Offset: 0x000E6468
			public bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FE4 RID: 16356 RVA: 0x000E7490 File Offset: 0x000E6490
			public bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FE5 RID: 16357 RVA: 0x000E74B5 File Offset: 0x000E64B5
			public void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FE6 RID: 16358 RVA: 0x000E74B7 File Offset: 0x000E64B7
			public void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FE7 RID: 16359 RVA: 0x000E74B9 File Offset: 0x000E64B9
			public void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FE8 RID: 16360 RVA: 0x000E74BB File Offset: 0x000E64BB
			public void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FE9 RID: 16361 RVA: 0x000E74BD File Offset: 0x000E64BD
			public void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FEA RID: 16362 RVA: 0x000E74BF File Offset: 0x000E64BF
			public void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FEB RID: 16363 RVA: 0x000E74C1 File Offset: 0x000E64C1
			public void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FEC RID: 16364 RVA: 0x000E74C3 File Offset: 0x000E64C3
			public void onbeforeeditfocus(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FED RID: 16365 RVA: 0x000E74C5 File Offset: 0x000E64C5
			public void onselectionchange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FEE RID: 16366 RVA: 0x000E74C8 File Offset: 0x000E64C8
			public bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FEF RID: 16367 RVA: 0x000E74F0 File Offset: 0x000E64F0
			public bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FF0 RID: 16368 RVA: 0x000E7515 File Offset: 0x000E6515
			public void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FF1 RID: 16369 RVA: 0x000E7517 File Offset: 0x000E6517
			public void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06003FF2 RID: 16370 RVA: 0x000E751C File Offset: 0x000E651C
			public bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06003FF3 RID: 16371 RVA: 0x000E7544 File Offset: 0x000E6544
			public bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x04001F3A RID: 7994
			private HtmlDocument parent;
		}
	}
}
