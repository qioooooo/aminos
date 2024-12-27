using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200042F RID: 1071
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class HtmlElement
	{
		// Token: 0x06003FF4 RID: 16372 RVA: 0x000E7569 File Offset: 0x000E6569
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		internal HtmlElement(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLElement element)
		{
			this.htmlElement = element;
			this.shimManager = shimManager;
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06003FF5 RID: 16373 RVA: 0x000E7580 File Offset: 0x000E6580
		public HtmlElementCollection All
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection ihtmlelementCollection = this.NativeHtmlElement.GetAll() as UnsafeNativeMethods.IHTMLElementCollection;
				if (ihtmlelementCollection == null)
				{
					return new HtmlElementCollection(this.shimManager);
				}
				return new HtmlElementCollection(this.shimManager, ihtmlelementCollection);
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06003FF6 RID: 16374 RVA: 0x000E75BC File Offset: 0x000E65BC
		public HtmlElementCollection Children
		{
			get
			{
				UnsafeNativeMethods.IHTMLElementCollection ihtmlelementCollection = this.NativeHtmlElement.GetChildren() as UnsafeNativeMethods.IHTMLElementCollection;
				if (ihtmlelementCollection == null)
				{
					return new HtmlElementCollection(this.shimManager);
				}
				return new HtmlElementCollection(this.shimManager, ihtmlelementCollection);
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06003FF7 RID: 16375 RVA: 0x000E75F5 File Offset: 0x000E65F5
		public bool CanHaveChildren
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).CanHaveChildren();
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06003FF8 RID: 16376 RVA: 0x000E7608 File Offset: 0x000E6608
		public Rectangle ClientRectangle
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement2 ihtmlelement = (UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement;
				return new Rectangle(ihtmlelement.ClientLeft(), ihtmlelement.ClientTop(), ihtmlelement.ClientWidth(), ihtmlelement.ClientHeight());
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06003FF9 RID: 16377 RVA: 0x000E7640 File Offset: 0x000E6640
		public HtmlDocument Document
		{
			get
			{
				UnsafeNativeMethods.IHTMLDocument ihtmldocument = this.NativeHtmlElement.GetDocument() as UnsafeNativeMethods.IHTMLDocument;
				if (ihtmldocument == null)
				{
					return null;
				}
				return new HtmlDocument(this.shimManager, ihtmldocument);
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06003FFA RID: 16378 RVA: 0x000E766F File Offset: 0x000E666F
		// (set) Token: 0x06003FFB RID: 16379 RVA: 0x000E7684 File Offset: 0x000E6684
		public bool Enabled
		{
			get
			{
				return !((UnsafeNativeMethods.IHTMLElement3)this.NativeHtmlElement).GetDisabled();
			}
			set
			{
				((UnsafeNativeMethods.IHTMLElement3)this.NativeHtmlElement).SetDisabled(!value);
			}
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06003FFC RID: 16380 RVA: 0x000E769C File Offset: 0x000E669C
		private HtmlElement.HtmlElementShim ElementShim
		{
			get
			{
				if (this.ShimManager != null)
				{
					HtmlElement.HtmlElementShim htmlElementShim = this.ShimManager.GetElementShim(this);
					if (htmlElementShim == null)
					{
						this.shimManager.AddElementShim(this);
						htmlElementShim = this.ShimManager.GetElementShim(this);
					}
					return htmlElementShim;
				}
				return null;
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x06003FFD RID: 16381 RVA: 0x000E76E0 File Offset: 0x000E66E0
		public HtmlElement FirstChild
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement ihtmlelement = null;
				UnsafeNativeMethods.IHTMLDOMNode ihtmldomnode = this.NativeHtmlElement as UnsafeNativeMethods.IHTMLDOMNode;
				if (ihtmldomnode != null)
				{
					ihtmlelement = ihtmldomnode.FirstChild() as UnsafeNativeMethods.IHTMLElement;
				}
				if (ihtmlelement == null)
				{
					return null;
				}
				return new HtmlElement(this.shimManager, ihtmlelement);
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x06003FFE RID: 16382 RVA: 0x000E771B File Offset: 0x000E671B
		// (set) Token: 0x06003FFF RID: 16383 RVA: 0x000E7728 File Offset: 0x000E6728
		public string Id
		{
			get
			{
				return this.NativeHtmlElement.GetId();
			}
			set
			{
				this.NativeHtmlElement.SetId(value);
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06004000 RID: 16384 RVA: 0x000E7736 File Offset: 0x000E6736
		// (set) Token: 0x06004001 RID: 16385 RVA: 0x000E7744 File Offset: 0x000E6744
		public string InnerHtml
		{
			get
			{
				return this.NativeHtmlElement.GetInnerHTML();
			}
			set
			{
				try
				{
					this.NativeHtmlElement.SetInnerHTML(value);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode == -2146827688)
					{
						throw new NotSupportedException(SR.GetString("HtmlElementPropertyNotSupported"));
					}
					throw;
				}
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06004002 RID: 16386 RVA: 0x000E7790 File Offset: 0x000E6790
		// (set) Token: 0x06004003 RID: 16387 RVA: 0x000E77A0 File Offset: 0x000E67A0
		public string InnerText
		{
			get
			{
				return this.NativeHtmlElement.GetInnerText();
			}
			set
			{
				try
				{
					this.NativeHtmlElement.SetInnerText(value);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode == -2146827688)
					{
						throw new NotSupportedException(SR.GetString("HtmlElementPropertyNotSupported"));
					}
					throw;
				}
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06004004 RID: 16388 RVA: 0x000E77EC File Offset: 0x000E67EC
		// (set) Token: 0x06004005 RID: 16389 RVA: 0x000E77F9 File Offset: 0x000E67F9
		public string Name
		{
			get
			{
				return this.GetAttribute("Name");
			}
			set
			{
				this.SetAttribute("Name", value);
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06004006 RID: 16390 RVA: 0x000E7807 File Offset: 0x000E6807
		private UnsafeNativeMethods.IHTMLElement NativeHtmlElement
		{
			get
			{
				return this.htmlElement;
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06004007 RID: 16391 RVA: 0x000E7810 File Offset: 0x000E6810
		public HtmlElement NextSibling
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement ihtmlelement = null;
				UnsafeNativeMethods.IHTMLDOMNode ihtmldomnode = this.NativeHtmlElement as UnsafeNativeMethods.IHTMLDOMNode;
				if (ihtmldomnode != null)
				{
					ihtmlelement = ihtmldomnode.NextSibling() as UnsafeNativeMethods.IHTMLElement;
				}
				if (ihtmlelement == null)
				{
					return null;
				}
				return new HtmlElement(this.shimManager, ihtmlelement);
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06004008 RID: 16392 RVA: 0x000E784B File Offset: 0x000E684B
		public Rectangle OffsetRectangle
		{
			get
			{
				return new Rectangle(this.NativeHtmlElement.GetOffsetLeft(), this.NativeHtmlElement.GetOffsetTop(), this.NativeHtmlElement.GetOffsetWidth(), this.NativeHtmlElement.GetOffsetHeight());
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06004009 RID: 16393 RVA: 0x000E7880 File Offset: 0x000E6880
		public HtmlElement OffsetParent
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement offsetParent = this.NativeHtmlElement.GetOffsetParent();
				if (offsetParent == null)
				{
					return null;
				}
				return new HtmlElement(this.shimManager, offsetParent);
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x0600400A RID: 16394 RVA: 0x000E78AA File Offset: 0x000E68AA
		// (set) Token: 0x0600400B RID: 16395 RVA: 0x000E78B8 File Offset: 0x000E68B8
		public string OuterHtml
		{
			get
			{
				return this.NativeHtmlElement.GetOuterHTML();
			}
			set
			{
				try
				{
					this.NativeHtmlElement.SetOuterHTML(value);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode == -2146827688)
					{
						throw new NotSupportedException(SR.GetString("HtmlElementPropertyNotSupported"));
					}
					throw;
				}
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x0600400C RID: 16396 RVA: 0x000E7904 File Offset: 0x000E6904
		// (set) Token: 0x0600400D RID: 16397 RVA: 0x000E7914 File Offset: 0x000E6914
		public string OuterText
		{
			get
			{
				return this.NativeHtmlElement.GetOuterText();
			}
			set
			{
				try
				{
					this.NativeHtmlElement.SetOuterText(value);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode == -2146827688)
					{
						throw new NotSupportedException(SR.GetString("HtmlElementPropertyNotSupported"));
					}
					throw;
				}
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x0600400E RID: 16398 RVA: 0x000E7960 File Offset: 0x000E6960
		public HtmlElement Parent
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement parentElement = this.NativeHtmlElement.GetParentElement();
				if (parentElement == null)
				{
					return null;
				}
				return new HtmlElement(this.shimManager, parentElement);
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x0600400F RID: 16399 RVA: 0x000E798C File Offset: 0x000E698C
		public Rectangle ScrollRectangle
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement2 ihtmlelement = (UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement;
				return new Rectangle(ihtmlelement.GetScrollLeft(), ihtmlelement.GetScrollTop(), ihtmlelement.GetScrollWidth(), ihtmlelement.GetScrollHeight());
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06004010 RID: 16400 RVA: 0x000E79C2 File Offset: 0x000E69C2
		// (set) Token: 0x06004011 RID: 16401 RVA: 0x000E79D4 File Offset: 0x000E69D4
		public int ScrollLeft
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).GetScrollLeft();
			}
			set
			{
				((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).SetScrollLeft(value);
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06004012 RID: 16402 RVA: 0x000E79E7 File Offset: 0x000E69E7
		// (set) Token: 0x06004013 RID: 16403 RVA: 0x000E79F9 File Offset: 0x000E69F9
		public int ScrollTop
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).GetScrollTop();
			}
			set
			{
				((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).SetScrollTop(value);
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06004014 RID: 16404 RVA: 0x000E7A0C File Offset: 0x000E6A0C
		private HtmlShimManager ShimManager
		{
			get
			{
				return this.shimManager;
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06004015 RID: 16405 RVA: 0x000E7A14 File Offset: 0x000E6A14
		// (set) Token: 0x06004016 RID: 16406 RVA: 0x000E7A26 File Offset: 0x000E6A26
		public string Style
		{
			get
			{
				return this.NativeHtmlElement.GetStyle().GetCssText();
			}
			set
			{
				this.NativeHtmlElement.GetStyle().SetCssText(value);
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06004017 RID: 16407 RVA: 0x000E7A39 File Offset: 0x000E6A39
		public string TagName
		{
			get
			{
				return this.NativeHtmlElement.GetTagName();
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06004018 RID: 16408 RVA: 0x000E7A46 File Offset: 0x000E6A46
		// (set) Token: 0x06004019 RID: 16409 RVA: 0x000E7A58 File Offset: 0x000E6A58
		public short TabIndex
		{
			get
			{
				return ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).GetTabIndex();
			}
			set
			{
				((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).SetTabIndex((int)value);
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x0600401A RID: 16410 RVA: 0x000E7A6B File Offset: 0x000E6A6B
		public object DomElement
		{
			get
			{
				return this.NativeHtmlElement;
			}
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x000E7A73 File Offset: 0x000E6A73
		public HtmlElement AppendChild(HtmlElement newElement)
		{
			return this.InsertAdjacentElement(HtmlElementInsertionOrientation.BeforeEnd, newElement);
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x000E7A7D File Offset: 0x000E6A7D
		public void AttachEventHandler(string eventName, EventHandler eventHandler)
		{
			this.ElementShim.AttachEventHandler(eventName, eventHandler);
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x000E7A8C File Offset: 0x000E6A8C
		public void DetachEventHandler(string eventName, EventHandler eventHandler)
		{
			this.ElementShim.DetachEventHandler(eventName, eventHandler);
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x000E7A9C File Offset: 0x000E6A9C
		public void Focus()
		{
			try
			{
				((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).Focus();
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2146826178)
				{
					throw new NotSupportedException(SR.GetString("HtmlElementMethodNotSupported"));
				}
				throw;
			}
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x000E7AEC File Offset: 0x000E6AEC
		public string GetAttribute(string attributeName)
		{
			object attribute = this.NativeHtmlElement.GetAttribute(attributeName, 0);
			if (attribute != null)
			{
				return attribute.ToString();
			}
			return "";
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x000E7B18 File Offset: 0x000E6B18
		public HtmlElementCollection GetElementsByTagName(string tagName)
		{
			UnsafeNativeMethods.IHTMLElementCollection elementsByTagName = ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).GetElementsByTagName(tagName);
			if (elementsByTagName == null)
			{
				return new HtmlElementCollection(this.shimManager);
			}
			return new HtmlElementCollection(this.shimManager, elementsByTagName);
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x000E7B54 File Offset: 0x000E6B54
		public HtmlElement InsertAdjacentElement(HtmlElementInsertionOrientation orient, HtmlElement newElement)
		{
			UnsafeNativeMethods.IHTMLElement ihtmlelement = ((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).InsertAdjacentElement(orient.ToString(), (UnsafeNativeMethods.IHTMLElement)newElement.DomElement);
			if (ihtmlelement == null)
			{
				return null;
			}
			return new HtmlElement(this.shimManager, ihtmlelement);
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x000E7B99 File Offset: 0x000E6B99
		public object InvokeMember(string methodName)
		{
			return this.InvokeMember(methodName, null);
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x000E7BA4 File Offset: 0x000E6BA4
		public object InvokeMember(string methodName, params object[] parameter)
		{
			object obj = null;
			NativeMethods.tagDISPPARAMS tagDISPPARAMS = new NativeMethods.tagDISPPARAMS();
			tagDISPPARAMS.rgvarg = IntPtr.Zero;
			try
			{
				UnsafeNativeMethods.IDispatch dispatch = this.NativeHtmlElement as UnsafeNativeMethods.IDispatch;
				if (dispatch != null)
				{
					Guid empty = Guid.Empty;
					string[] array = new string[] { methodName };
					int[] array2 = new int[] { -1 };
					int idsOfNames = dispatch.GetIDsOfNames(ref empty, array, 1, SafeNativeMethods.GetThreadLCID(), array2);
					if (NativeMethods.Succeeded(idsOfNames) && array2[0] != -1)
					{
						if (parameter != null)
						{
							Array.Reverse(parameter);
						}
						tagDISPPARAMS.rgvarg = ((parameter == null) ? IntPtr.Zero : HtmlDocument.ArrayToVARIANTVector(parameter));
						tagDISPPARAMS.cArgs = ((parameter == null) ? 0 : parameter.Length);
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
					HtmlDocument.FreeVARIANTVector(tagDISPPARAMS.rgvarg, parameter.Length);
				}
			}
			return obj;
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x000E7CE4 File Offset: 0x000E6CE4
		public void RemoveFocus()
		{
			((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).Blur();
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x000E7CF6 File Offset: 0x000E6CF6
		public void RaiseEvent(string eventName)
		{
			((UnsafeNativeMethods.IHTMLElement3)this.NativeHtmlElement).FireEvent(eventName, null);
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x000E7D0B File Offset: 0x000E6D0B
		public void ScrollIntoView(bool alignWithTop)
		{
			this.NativeHtmlElement.ScrollIntoView(alignWithTop);
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x000E7D20 File Offset: 0x000E6D20
		public void SetAttribute(string attributeName, string value)
		{
			try
			{
				this.NativeHtmlElement.SetAttribute(attributeName, value, 0);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147352567)
				{
					throw new NotSupportedException(SR.GetString("HtmlElementAttributeNotSupported"));
				}
				throw;
			}
		}

		// Token: 0x1400023B RID: 571
		// (add) Token: 0x06004028 RID: 16424 RVA: 0x000E7D70 File Offset: 0x000E6D70
		// (remove) Token: 0x06004029 RID: 16425 RVA: 0x000E7D83 File Offset: 0x000E6D83
		public event HtmlElementEventHandler Click
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventClick, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventClick, value);
			}
		}

		// Token: 0x1400023C RID: 572
		// (add) Token: 0x0600402A RID: 16426 RVA: 0x000E7D96 File Offset: 0x000E6D96
		// (remove) Token: 0x0600402B RID: 16427 RVA: 0x000E7DA9 File Offset: 0x000E6DA9
		public event HtmlElementEventHandler DoubleClick
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventDoubleClick, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventDoubleClick, value);
			}
		}

		// Token: 0x1400023D RID: 573
		// (add) Token: 0x0600402C RID: 16428 RVA: 0x000E7DBC File Offset: 0x000E6DBC
		// (remove) Token: 0x0600402D RID: 16429 RVA: 0x000E7DCF File Offset: 0x000E6DCF
		public event HtmlElementEventHandler Drag
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventDrag, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventDrag, value);
			}
		}

		// Token: 0x1400023E RID: 574
		// (add) Token: 0x0600402E RID: 16430 RVA: 0x000E7DE2 File Offset: 0x000E6DE2
		// (remove) Token: 0x0600402F RID: 16431 RVA: 0x000E7DF5 File Offset: 0x000E6DF5
		public event HtmlElementEventHandler DragEnd
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventDragEnd, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventDragEnd, value);
			}
		}

		// Token: 0x1400023F RID: 575
		// (add) Token: 0x06004030 RID: 16432 RVA: 0x000E7E08 File Offset: 0x000E6E08
		// (remove) Token: 0x06004031 RID: 16433 RVA: 0x000E7E1B File Offset: 0x000E6E1B
		public event HtmlElementEventHandler DragLeave
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventDragLeave, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventDragLeave, value);
			}
		}

		// Token: 0x14000240 RID: 576
		// (add) Token: 0x06004032 RID: 16434 RVA: 0x000E7E2E File Offset: 0x000E6E2E
		// (remove) Token: 0x06004033 RID: 16435 RVA: 0x000E7E41 File Offset: 0x000E6E41
		public event HtmlElementEventHandler DragOver
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventDragOver, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventDragOver, value);
			}
		}

		// Token: 0x14000241 RID: 577
		// (add) Token: 0x06004034 RID: 16436 RVA: 0x000E7E54 File Offset: 0x000E6E54
		// (remove) Token: 0x06004035 RID: 16437 RVA: 0x000E7E67 File Offset: 0x000E6E67
		public event HtmlElementEventHandler Focusing
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventFocusing, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventFocusing, value);
			}
		}

		// Token: 0x14000242 RID: 578
		// (add) Token: 0x06004036 RID: 16438 RVA: 0x000E7E7A File Offset: 0x000E6E7A
		// (remove) Token: 0x06004037 RID: 16439 RVA: 0x000E7E8D File Offset: 0x000E6E8D
		public event HtmlElementEventHandler GotFocus
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventGotFocus, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventGotFocus, value);
			}
		}

		// Token: 0x14000243 RID: 579
		// (add) Token: 0x06004038 RID: 16440 RVA: 0x000E7EA0 File Offset: 0x000E6EA0
		// (remove) Token: 0x06004039 RID: 16441 RVA: 0x000E7EB3 File Offset: 0x000E6EB3
		public event HtmlElementEventHandler LosingFocus
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventLosingFocus, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventLosingFocus, value);
			}
		}

		// Token: 0x14000244 RID: 580
		// (add) Token: 0x0600403A RID: 16442 RVA: 0x000E7EC6 File Offset: 0x000E6EC6
		// (remove) Token: 0x0600403B RID: 16443 RVA: 0x000E7ED9 File Offset: 0x000E6ED9
		public event HtmlElementEventHandler LostFocus
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventLostFocus, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventLostFocus, value);
			}
		}

		// Token: 0x14000245 RID: 581
		// (add) Token: 0x0600403C RID: 16444 RVA: 0x000E7EEC File Offset: 0x000E6EEC
		// (remove) Token: 0x0600403D RID: 16445 RVA: 0x000E7EFF File Offset: 0x000E6EFF
		public event HtmlElementEventHandler KeyDown
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventKeyDown, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventKeyDown, value);
			}
		}

		// Token: 0x14000246 RID: 582
		// (add) Token: 0x0600403E RID: 16446 RVA: 0x000E7F12 File Offset: 0x000E6F12
		// (remove) Token: 0x0600403F RID: 16447 RVA: 0x000E7F25 File Offset: 0x000E6F25
		public event HtmlElementEventHandler KeyPress
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventKeyPress, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventKeyPress, value);
			}
		}

		// Token: 0x14000247 RID: 583
		// (add) Token: 0x06004040 RID: 16448 RVA: 0x000E7F38 File Offset: 0x000E6F38
		// (remove) Token: 0x06004041 RID: 16449 RVA: 0x000E7F4B File Offset: 0x000E6F4B
		public event HtmlElementEventHandler KeyUp
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventKeyUp, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventKeyUp, value);
			}
		}

		// Token: 0x14000248 RID: 584
		// (add) Token: 0x06004042 RID: 16450 RVA: 0x000E7F5E File Offset: 0x000E6F5E
		// (remove) Token: 0x06004043 RID: 16451 RVA: 0x000E7F71 File Offset: 0x000E6F71
		public event HtmlElementEventHandler MouseMove
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseMove, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseMove, value);
			}
		}

		// Token: 0x14000249 RID: 585
		// (add) Token: 0x06004044 RID: 16452 RVA: 0x000E7F84 File Offset: 0x000E6F84
		// (remove) Token: 0x06004045 RID: 16453 RVA: 0x000E7F97 File Offset: 0x000E6F97
		public event HtmlElementEventHandler MouseDown
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseDown, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseDown, value);
			}
		}

		// Token: 0x1400024A RID: 586
		// (add) Token: 0x06004046 RID: 16454 RVA: 0x000E7FAA File Offset: 0x000E6FAA
		// (remove) Token: 0x06004047 RID: 16455 RVA: 0x000E7FBD File Offset: 0x000E6FBD
		public event HtmlElementEventHandler MouseOver
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseOver, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseOver, value);
			}
		}

		// Token: 0x1400024B RID: 587
		// (add) Token: 0x06004048 RID: 16456 RVA: 0x000E7FD0 File Offset: 0x000E6FD0
		// (remove) Token: 0x06004049 RID: 16457 RVA: 0x000E7FE3 File Offset: 0x000E6FE3
		public event HtmlElementEventHandler MouseUp
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseUp, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseUp, value);
			}
		}

		// Token: 0x1400024C RID: 588
		// (add) Token: 0x0600404A RID: 16458 RVA: 0x000E7FF6 File Offset: 0x000E6FF6
		// (remove) Token: 0x0600404B RID: 16459 RVA: 0x000E8009 File Offset: 0x000E7009
		public event HtmlElementEventHandler MouseEnter
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseEnter, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseEnter, value);
			}
		}

		// Token: 0x1400024D RID: 589
		// (add) Token: 0x0600404C RID: 16460 RVA: 0x000E801C File Offset: 0x000E701C
		// (remove) Token: 0x0600404D RID: 16461 RVA: 0x000E802F File Offset: 0x000E702F
		public event HtmlElementEventHandler MouseLeave
		{
			add
			{
				this.ElementShim.AddHandler(HtmlElement.EventMouseLeave, value);
			}
			remove
			{
				this.ElementShim.RemoveHandler(HtmlElement.EventMouseLeave, value);
			}
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x000E8044 File Offset: 0x000E7044
		public static bool operator ==(HtmlElement left, HtmlElement right)
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
				intPtr = Marshal.GetIUnknownForObject(left.NativeHtmlElement);
				intPtr2 = Marshal.GetIUnknownForObject(right.NativeHtmlElement);
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

		// Token: 0x0600404F RID: 16463 RVA: 0x000E80D8 File Offset: 0x000E70D8
		public static bool operator !=(HtmlElement left, HtmlElement right)
		{
			return !(left == right);
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x000E80E4 File Offset: 0x000E70E4
		public override int GetHashCode()
		{
			if (this.htmlElement != null)
			{
				return this.htmlElement.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x000E80FB File Offset: 0x000E70FB
		public override bool Equals(object obj)
		{
			return this == obj as HtmlElement;
		}

		// Token: 0x04001F3B RID: 7995
		internal static readonly object EventClick = new object();

		// Token: 0x04001F3C RID: 7996
		internal static readonly object EventDoubleClick = new object();

		// Token: 0x04001F3D RID: 7997
		internal static readonly object EventDrag = new object();

		// Token: 0x04001F3E RID: 7998
		internal static readonly object EventDragEnd = new object();

		// Token: 0x04001F3F RID: 7999
		internal static readonly object EventDragLeave = new object();

		// Token: 0x04001F40 RID: 8000
		internal static readonly object EventDragOver = new object();

		// Token: 0x04001F41 RID: 8001
		internal static readonly object EventFocusing = new object();

		// Token: 0x04001F42 RID: 8002
		internal static readonly object EventGotFocus = new object();

		// Token: 0x04001F43 RID: 8003
		internal static readonly object EventLosingFocus = new object();

		// Token: 0x04001F44 RID: 8004
		internal static readonly object EventLostFocus = new object();

		// Token: 0x04001F45 RID: 8005
		internal static readonly object EventKeyDown = new object();

		// Token: 0x04001F46 RID: 8006
		internal static readonly object EventKeyPress = new object();

		// Token: 0x04001F47 RID: 8007
		internal static readonly object EventKeyUp = new object();

		// Token: 0x04001F48 RID: 8008
		internal static readonly object EventMouseDown = new object();

		// Token: 0x04001F49 RID: 8009
		internal static readonly object EventMouseEnter = new object();

		// Token: 0x04001F4A RID: 8010
		internal static readonly object EventMouseLeave = new object();

		// Token: 0x04001F4B RID: 8011
		internal static readonly object EventMouseMove = new object();

		// Token: 0x04001F4C RID: 8012
		internal static readonly object EventMouseOver = new object();

		// Token: 0x04001F4D RID: 8013
		internal static readonly object EventMouseUp = new object();

		// Token: 0x04001F4E RID: 8014
		private UnsafeNativeMethods.IHTMLElement htmlElement;

		// Token: 0x04001F4F RID: 8015
		private HtmlShimManager shimManager;

		// Token: 0x02000430 RID: 1072
		[ClassInterface(ClassInterfaceType.None)]
		private class HTMLElementEvents2 : StandardOleMarshalObject, UnsafeNativeMethods.DHTMLElementEvents2, UnsafeNativeMethods.DHTMLAnchorEvents2, UnsafeNativeMethods.DHTMLAreaEvents2, UnsafeNativeMethods.DHTMLButtonElementEvents2, UnsafeNativeMethods.DHTMLControlElementEvents2, UnsafeNativeMethods.DHTMLFormElementEvents2, UnsafeNativeMethods.DHTMLFrameSiteEvents2, UnsafeNativeMethods.DHTMLImgEvents2, UnsafeNativeMethods.DHTMLInputFileElementEvents2, UnsafeNativeMethods.DHTMLInputImageEvents2, UnsafeNativeMethods.DHTMLInputTextElementEvents2, UnsafeNativeMethods.DHTMLLabelEvents2, UnsafeNativeMethods.DHTMLLinkElementEvents2, UnsafeNativeMethods.DHTMLMapEvents2, UnsafeNativeMethods.DHTMLMarqueeElementEvents2, UnsafeNativeMethods.DHTMLOptionButtonElementEvents2, UnsafeNativeMethods.DHTMLSelectElementEvents2, UnsafeNativeMethods.DHTMLStyleElementEvents2, UnsafeNativeMethods.DHTMLTableEvents2, UnsafeNativeMethods.DHTMLTextContainerEvents2, UnsafeNativeMethods.DHTMLScriptEvents2
		{
			// Token: 0x06004053 RID: 16467 RVA: 0x000E81D7 File Offset: 0x000E71D7
			public HTMLElementEvents2(HtmlElement htmlElement)
			{
				this.parent = htmlElement;
			}

			// Token: 0x06004054 RID: 16468 RVA: 0x000E81E6 File Offset: 0x000E71E6
			private void FireEvent(object key, EventArgs e)
			{
				if (this.parent != null)
				{
					this.parent.ElementShim.FireEvent(key, e);
				}
			}

			// Token: 0x06004055 RID: 16469 RVA: 0x000E8208 File Offset: 0x000E7208
			public bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventClick, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004056 RID: 16470 RVA: 0x000E823C File Offset: 0x000E723C
			public bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventDoubleClick, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004057 RID: 16471 RVA: 0x000E8270 File Offset: 0x000E7270
			public bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventKeyPress, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004058 RID: 16472 RVA: 0x000E82A4 File Offset: 0x000E72A4
			public void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventKeyDown, htmlElementEventArgs);
			}

			// Token: 0x06004059 RID: 16473 RVA: 0x000E82D0 File Offset: 0x000E72D0
			public void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventKeyUp, htmlElementEventArgs);
			}

			// Token: 0x0600405A RID: 16474 RVA: 0x000E82FC File Offset: 0x000E72FC
			public void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseOver, htmlElementEventArgs);
			}

			// Token: 0x0600405B RID: 16475 RVA: 0x000E8328 File Offset: 0x000E7328
			public void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseMove, htmlElementEventArgs);
			}

			// Token: 0x0600405C RID: 16476 RVA: 0x000E8354 File Offset: 0x000E7354
			public void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseDown, htmlElementEventArgs);
			}

			// Token: 0x0600405D RID: 16477 RVA: 0x000E8380 File Offset: 0x000E7380
			public void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseUp, htmlElementEventArgs);
			}

			// Token: 0x0600405E RID: 16478 RVA: 0x000E83AC File Offset: 0x000E73AC
			public void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseEnter, htmlElementEventArgs);
			}

			// Token: 0x0600405F RID: 16479 RVA: 0x000E83D8 File Offset: 0x000E73D8
			public void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventMouseLeave, htmlElementEventArgs);
			}

			// Token: 0x06004060 RID: 16480 RVA: 0x000E8404 File Offset: 0x000E7404
			public bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004061 RID: 16481 RVA: 0x000E842C File Offset: 0x000E742C
			public void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventGotFocus, htmlElementEventArgs);
			}

			// Token: 0x06004062 RID: 16482 RVA: 0x000E8458 File Offset: 0x000E7458
			public bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventDrag, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004063 RID: 16483 RVA: 0x000E848C File Offset: 0x000E748C
			public void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventDragEnd, htmlElementEventArgs);
			}

			// Token: 0x06004064 RID: 16484 RVA: 0x000E84B8 File Offset: 0x000E74B8
			public void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventDragLeave, htmlElementEventArgs);
			}

			// Token: 0x06004065 RID: 16485 RVA: 0x000E84E4 File Offset: 0x000E74E4
			public bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventDragOver, htmlElementEventArgs);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004066 RID: 16486 RVA: 0x000E8518 File Offset: 0x000E7518
			public void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventFocusing, htmlElementEventArgs);
			}

			// Token: 0x06004067 RID: 16487 RVA: 0x000E8544 File Offset: 0x000E7544
			public void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventLosingFocus, htmlElementEventArgs);
			}

			// Token: 0x06004068 RID: 16488 RVA: 0x000E8570 File Offset: 0x000E7570
			public void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				this.FireEvent(HtmlElement.EventLostFocus, htmlElementEventArgs);
			}

			// Token: 0x06004069 RID: 16489 RVA: 0x000E859B File Offset: 0x000E759B
			public void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600406A RID: 16490 RVA: 0x000E85A0 File Offset: 0x000E75A0
			public bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x0600406B RID: 16491 RVA: 0x000E85C8 File Offset: 0x000E75C8
			public bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x0600406C RID: 16492 RVA: 0x000E85ED File Offset: 0x000E75ED
			public void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600406D RID: 16493 RVA: 0x000E85F0 File Offset: 0x000E75F0
			public bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x0600406E RID: 16494 RVA: 0x000E8615 File Offset: 0x000E7615
			public void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600406F RID: 16495 RVA: 0x000E8618 File Offset: 0x000E7618
			public bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004070 RID: 16496 RVA: 0x000E8640 File Offset: 0x000E7640
			public bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004071 RID: 16497 RVA: 0x000E8665 File Offset: 0x000E7665
			public void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004072 RID: 16498 RVA: 0x000E8668 File Offset: 0x000E7668
			public bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004073 RID: 16499 RVA: 0x000E868D File Offset: 0x000E768D
			public void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004074 RID: 16500 RVA: 0x000E868F File Offset: 0x000E768F
			public void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004075 RID: 16501 RVA: 0x000E8691 File Offset: 0x000E7691
			public void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004076 RID: 16502 RVA: 0x000E8693 File Offset: 0x000E7693
			public void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004077 RID: 16503 RVA: 0x000E8695 File Offset: 0x000E7695
			public void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004078 RID: 16504 RVA: 0x000E8697 File Offset: 0x000E7697
			public void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004079 RID: 16505 RVA: 0x000E8699 File Offset: 0x000E7699
			public void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600407A RID: 16506 RVA: 0x000E869B File Offset: 0x000E769B
			public void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600407B RID: 16507 RVA: 0x000E86A0 File Offset: 0x000E76A0
			public bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x0600407C RID: 16508 RVA: 0x000E86C8 File Offset: 0x000E76C8
			public bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x0600407D RID: 16509 RVA: 0x000E86F0 File Offset: 0x000E76F0
			public bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x0600407E RID: 16510 RVA: 0x000E8718 File Offset: 0x000E7718
			public bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x0600407F RID: 16511 RVA: 0x000E8740 File Offset: 0x000E7740
			public bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004080 RID: 16512 RVA: 0x000E8768 File Offset: 0x000E7768
			public bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004081 RID: 16513 RVA: 0x000E8790 File Offset: 0x000E7790
			public bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004082 RID: 16514 RVA: 0x000E87B8 File Offset: 0x000E77B8
			public bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004083 RID: 16515 RVA: 0x000E87E0 File Offset: 0x000E77E0
			public bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004084 RID: 16516 RVA: 0x000E8805 File Offset: 0x000E7805
			public void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004085 RID: 16517 RVA: 0x000E8807 File Offset: 0x000E7807
			public void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004086 RID: 16518 RVA: 0x000E8809 File Offset: 0x000E7809
			public void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004087 RID: 16519 RVA: 0x000E880B File Offset: 0x000E780B
			public void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004088 RID: 16520 RVA: 0x000E880D File Offset: 0x000E780D
			public void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004089 RID: 16521 RVA: 0x000E880F File Offset: 0x000E780F
			public void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600408A RID: 16522 RVA: 0x000E8811 File Offset: 0x000E7811
			public void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600408B RID: 16523 RVA: 0x000E8813 File Offset: 0x000E7813
			public void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600408C RID: 16524 RVA: 0x000E8818 File Offset: 0x000E7818
			public bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x0600408D RID: 16525 RVA: 0x000E8840 File Offset: 0x000E7840
			public bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x0600408E RID: 16526 RVA: 0x000E8865 File Offset: 0x000E7865
			public void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600408F RID: 16527 RVA: 0x000E8868 File Offset: 0x000E7868
			public bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004090 RID: 16528 RVA: 0x000E8890 File Offset: 0x000E7890
			public bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004091 RID: 16529 RVA: 0x000E88B5 File Offset: 0x000E78B5
			public void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004092 RID: 16530 RVA: 0x000E88B8 File Offset: 0x000E78B8
			public bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004093 RID: 16531 RVA: 0x000E88E0 File Offset: 0x000E78E0
			public bool onchange(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004094 RID: 16532 RVA: 0x000E8905 File Offset: 0x000E7905
			public void onselect(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004095 RID: 16533 RVA: 0x000E8907 File Offset: 0x000E7907
			public void onload(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004096 RID: 16534 RVA: 0x000E8909 File Offset: 0x000E7909
			public void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004097 RID: 16535 RVA: 0x000E890B File Offset: 0x000E790B
			public void onabort(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x06004098 RID: 16536 RVA: 0x000E8910 File Offset: 0x000E7910
			public bool onsubmit(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x06004099 RID: 16537 RVA: 0x000E8938 File Offset: 0x000E7938
			public bool onreset(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
				HtmlElementEventArgs htmlElementEventArgs = new HtmlElementEventArgs(this.parent.ShimManager, evtObj);
				return htmlElementEventArgs.ReturnValue;
			}

			// Token: 0x0600409A RID: 16538 RVA: 0x000E895D File Offset: 0x000E795D
			public void onchange_void(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600409B RID: 16539 RVA: 0x000E895F File Offset: 0x000E795F
			public void onbounce(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600409C RID: 16540 RVA: 0x000E8961 File Offset: 0x000E7961
			public void onfinish(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x0600409D RID: 16541 RVA: 0x000E8963 File Offset: 0x000E7963
			public void onstart(UnsafeNativeMethods.IHTMLEventObj evtObj)
			{
			}

			// Token: 0x04001F50 RID: 8016
			private HtmlElement parent;
		}

		// Token: 0x02000431 RID: 1073
		internal class HtmlElementShim : HtmlShim
		{
			// Token: 0x0600409E RID: 16542 RVA: 0x000E8968 File Offset: 0x000E7968
			public HtmlElementShim(HtmlElement element)
			{
				this.htmlElement = element;
				if (this.htmlElement != null)
				{
					HtmlDocument document = this.htmlElement.Document;
					if (document != null)
					{
						HtmlWindow window = document.Window;
						if (window != null)
						{
							this.associatedWindow = window.NativeHtmlWindow;
						}
					}
				}
			}

			// Token: 0x17000C71 RID: 3185
			// (get) Token: 0x0600409F RID: 16543 RVA: 0x000E89C1 File Offset: 0x000E79C1
			public UnsafeNativeMethods.IHTMLElement NativeHtmlElement
			{
				get
				{
					return this.htmlElement.NativeHtmlElement;
				}
			}

			// Token: 0x17000C72 RID: 3186
			// (get) Token: 0x060040A0 RID: 16544 RVA: 0x000E89CE File Offset: 0x000E79CE
			internal HtmlElement Element
			{
				get
				{
					return this.htmlElement;
				}
			}

			// Token: 0x17000C73 RID: 3187
			// (get) Token: 0x060040A1 RID: 16545 RVA: 0x000E89D6 File Offset: 0x000E79D6
			public override UnsafeNativeMethods.IHTMLWindow2 AssociatedWindow
			{
				get
				{
					return this.associatedWindow;
				}
			}

			// Token: 0x060040A2 RID: 16546 RVA: 0x000E89E0 File Offset: 0x000E79E0
			public override void AttachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy htmlToClrEventProxy = base.AddEventProxy(eventName, eventHandler);
				((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).AttachEvent(eventName, htmlToClrEventProxy);
			}

			// Token: 0x060040A3 RID: 16547 RVA: 0x000E8A0C File Offset: 0x000E7A0C
			public override void ConnectToEvents()
			{
				if (this.cookie == null || !this.cookie.Connected)
				{
					int num = 0;
					while (num < HtmlElement.HtmlElementShim.dispInterfaceTypes.Length && this.cookie == null)
					{
						this.cookie = new AxHost.ConnectionPointCookie(this.NativeHtmlElement, new HtmlElement.HTMLElementEvents2(this.htmlElement), HtmlElement.HtmlElementShim.dispInterfaceTypes[num], false);
						if (!this.cookie.Connected)
						{
							this.cookie = null;
						}
						num++;
					}
				}
			}

			// Token: 0x060040A4 RID: 16548 RVA: 0x000E8A80 File Offset: 0x000E7A80
			public override void DetachEventHandler(string eventName, EventHandler eventHandler)
			{
				HtmlToClrEventProxy htmlToClrEventProxy = base.RemoveEventProxy(eventHandler);
				if (htmlToClrEventProxy != null)
				{
					((UnsafeNativeMethods.IHTMLElement2)this.NativeHtmlElement).DetachEvent(eventName, htmlToClrEventProxy);
				}
			}

			// Token: 0x060040A5 RID: 16549 RVA: 0x000E8AAA File Offset: 0x000E7AAA
			public override void DisconnectFromEvents()
			{
				if (this.cookie != null)
				{
					this.cookie.Disconnect();
					this.cookie = null;
				}
			}

			// Token: 0x060040A6 RID: 16550 RVA: 0x000E8AC6 File Offset: 0x000E7AC6
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
				if (this.htmlElement != null)
				{
					Marshal.FinalReleaseComObject(this.htmlElement.NativeHtmlElement);
				}
				this.htmlElement = null;
			}

			// Token: 0x060040A7 RID: 16551 RVA: 0x000E8AF5 File Offset: 0x000E7AF5
			protected override object GetEventSender()
			{
				return this.htmlElement;
			}

			// Token: 0x04001F51 RID: 8017
			private static Type[] dispInterfaceTypes = new Type[]
			{
				typeof(UnsafeNativeMethods.DHTMLElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLAnchorEvents2),
				typeof(UnsafeNativeMethods.DHTMLAreaEvents2),
				typeof(UnsafeNativeMethods.DHTMLButtonElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLControlElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLFormElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLFrameSiteEvents2),
				typeof(UnsafeNativeMethods.DHTMLImgEvents2),
				typeof(UnsafeNativeMethods.DHTMLInputFileElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLInputImageEvents2),
				typeof(UnsafeNativeMethods.DHTMLInputTextElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLLabelEvents2),
				typeof(UnsafeNativeMethods.DHTMLLinkElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLMapEvents2),
				typeof(UnsafeNativeMethods.DHTMLMarqueeElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLOptionButtonElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLSelectElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLStyleElementEvents2),
				typeof(UnsafeNativeMethods.DHTMLTableEvents2),
				typeof(UnsafeNativeMethods.DHTMLTextContainerEvents2),
				typeof(UnsafeNativeMethods.DHTMLScriptEvents2)
			};

			// Token: 0x04001F52 RID: 8018
			private AxHost.ConnectionPointCookie cookie;

			// Token: 0x04001F53 RID: 8019
			private HtmlElement htmlElement;

			// Token: 0x04001F54 RID: 8020
			private UnsafeNativeMethods.IHTMLWindow2 associatedWindow;
		}
	}
}
