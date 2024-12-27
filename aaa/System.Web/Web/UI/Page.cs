using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Configuration;
using System.EnterpriseServices;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Caching;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Management;
using System.Web.RegularExpressions;
using System.Web.SessionState;
using System.Web.UI.Adapters;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Util;
using System.Xml;

namespace System.Web.UI
{
	// Token: 0x020003ED RID: 1005
	[DefaultEvent("Load")]
	[DesignerCategory("ASPXCodeBehind")]
	[ToolboxItem(false)]
	[Designer("Microsoft.VisualStudio.Web.WebForms.WebFormDesigner, Microsoft.VisualStudio.Web, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[DesignerSerializer("Microsoft.VisualStudio.Web.WebForms.WebFormCodeDomSerializer, Microsoft.VisualStudio.Web, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.TypeCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Page : TemplateControl, IHttpHandler
	{
		// Token: 0x060030B5 RID: 12469 RVA: 0x000D63E4 File Offset: 0x000D53E4
		static Page()
		{
			Page.s_systemPostFields.Add("__EVENTTARGET");
			Page.s_systemPostFields.Add("__EVENTARGUMENT");
			Page.s_systemPostFields.Add("__VIEWSTATEFIELDCOUNT");
			Page.s_systemPostFields.Add("__VIEWSTATEGENERATOR");
			Page.s_systemPostFields.Add("__VIEWSTATE");
			Page.s_systemPostFields.Add("__VIEWSTATEENCRYPTED");
			Page.s_systemPostFields.Add("__PREVIOUSPAGE");
			Page.s_systemPostFields.Add("__CALLBACKID");
			Page.s_systemPostFields.Add("__CALLBACKPARAM");
			Page.s_systemPostFields.Add("__LASTFOCUS");
			Page.s_systemPostFields.Add(Page.UniqueFilePathSuffixID);
			Page.s_systemPostFields.Add(HttpResponse.RedirectQueryStringVariable);
			Page.s_systemPostFields.Add("__EVENTVALIDATION");
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x000D655F File Offset: 0x000D555F
		public Page()
		{
			this._page = this;
			this._enableViewStateMac = true;
			this.ID = "__Page";
			this._supportsStyleSheets = -1;
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x060030B7 RID: 12471 RVA: 0x000D6599 File Offset: 0x000D5599
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HttpApplicationState Application
		{
			get
			{
				return this._application;
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x060030B8 RID: 12472 RVA: 0x000D65A1 File Offset: 0x000D55A1
		protected internal override HttpContext Context
		{
			get
			{
				if (this._context == null)
				{
					this._context = HttpContext.Current;
				}
				return this._context;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x060030B9 RID: 12473 RVA: 0x000D65BC File Offset: 0x000D55BC
		private StringSet ControlStateLoadedControlIds
		{
			get
			{
				if (this._controlStateLoadedControlIds == null)
				{
					this._controlStateLoadedControlIds = new StringSet();
				}
				return this._controlStateLoadedControlIds;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x060030BA RID: 12474 RVA: 0x000D65D7 File Offset: 0x000D55D7
		// (set) Token: 0x060030BB RID: 12475 RVA: 0x000D65DF File Offset: 0x000D55DF
		internal string ClientState
		{
			get
			{
				return this._clientState;
			}
			set
			{
				this._clientState = value;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x060030BC RID: 12476 RVA: 0x000D65E8 File Offset: 0x000D55E8
		internal string ClientOnSubmitEvent
		{
			get
			{
				if (this.ClientScript.HasSubmitStatements || (this.Form != null && this.Form.SubmitDisabledControls && this.EnabledControls.Count > 0))
				{
					return "javascript:return WebForm_OnSubmit();";
				}
				return string.Empty;
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x060030BD RID: 12477 RVA: 0x000D6625 File Offset: 0x000D5625
		public ClientScriptManager ClientScript
		{
			get
			{
				if (this._clientScriptManager == null)
				{
					this._clientScriptManager = new ClientScriptManager(this);
				}
				return this._clientScriptManager;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x060030BE RID: 12478 RVA: 0x000D6641 File Offset: 0x000D5641
		// (set) Token: 0x060030BF RID: 12479 RVA: 0x000D6657 File Offset: 0x000D5657
		[WebSysDescription("Page_ClientTarget")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		public string ClientTarget
		{
			get
			{
				if (this._clientTarget != null)
				{
					return this._clientTarget;
				}
				return string.Empty;
			}
			set
			{
				this._clientTarget = value;
				if (this._request != null)
				{
					this._request.ClientTarget = value;
				}
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x060030C0 RID: 12480 RVA: 0x000D6674 File Offset: 0x000D5674
		public string ClientQueryString
		{
			get
			{
				if (this._clientQueryString == null)
				{
					if (this.RequestInternal != null && this.Request.HasQueryString)
					{
						Hashtable hashtable = new Hashtable();
						foreach (object obj in ((IEnumerable)Page.s_systemPostFields))
						{
							string text = (string)obj;
							hashtable.Add(text, true);
						}
						this._clientQueryString = ((HttpValueCollection)this.Request.QueryString).ToString(true, hashtable);
					}
					else
					{
						this._clientQueryString = string.Empty;
					}
				}
				return this._clientQueryString;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x060030C1 RID: 12481 RVA: 0x000D6728 File Offset: 0x000D5728
		// (set) Token: 0x060030C2 RID: 12482 RVA: 0x000D6730 File Offset: 0x000D5730
		internal bool ContainsEncryptedViewState
		{
			get
			{
				return this._containsEncryptedViewState;
			}
			set
			{
				this._containsEncryptedViewState = value;
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x060030C3 RID: 12483 RVA: 0x000D6739 File Offset: 0x000D5739
		// (set) Token: 0x060030C4 RID: 12484 RVA: 0x000D6741 File Offset: 0x000D5741
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebSysDescription("Page_ErrorPage")]
		[Browsable(false)]
		public string ErrorPage
		{
			get
			{
				return this._errorPage;
			}
			set
			{
				this._errorPage = value;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x060030C5 RID: 12485 RVA: 0x000D674A File Offset: 0x000D574A
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsCallback
		{
			get
			{
				return this._isCallback;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x060030C6 RID: 12486 RVA: 0x000D6752 File Offset: 0x000D5752
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x060030C7 RID: 12487 RVA: 0x000D6758 File Offset: 0x000D5758
		protected internal virtual string UniqueFilePathSuffix
		{
			get
			{
				if (this._uniqueFilePathSuffix != null)
				{
					return this._uniqueFilePathSuffix;
				}
				long num = DateTime.Now.Ticks % 999983L;
				this._uniqueFilePathSuffix = Page.UniqueFilePathSuffixID + "=" + num.ToString("D6", CultureInfo.InvariantCulture);
				this._uniqueFilePathSuffix = this._uniqueFilePathSuffix.PadLeft(6, '0');
				return this._uniqueFilePathSuffix;
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x060030C8 RID: 12488 RVA: 0x000D67CE File Offset: 0x000D57CE
		// (set) Token: 0x060030C9 RID: 12489 RVA: 0x000D67D6 File Offset: 0x000D57D6
		public Control AutoPostBackControl
		{
			get
			{
				return this._autoPostBackControl;
			}
			set
			{
				this._autoPostBackControl = value;
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x060030CA RID: 12490 RVA: 0x000D67E0 File Offset: 0x000D57E0
		internal bool ClientSupportsFocus
		{
			get
			{
				return this._request != null && (this._request.Browser.EcmaScriptVersion >= Page.FocusMinimumEcmaVersion || this._request.Browser.JScriptVersion >= Page.FocusMinimumJScriptVersion);
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x060030CB RID: 12491 RVA: 0x000D6830 File Offset: 0x000D5830
		internal bool ClientSupportsJavaScript
		{
			get
			{
				if (!this._clientSupportsJavaScriptChecked)
				{
					this._clientSupportsJavaScript = this._request != null && this._request.Browser.EcmaScriptVersion >= Page.JavascriptMinimumVersion;
					this._clientSupportsJavaScriptChecked = true;
				}
				return this._clientSupportsJavaScript;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x060030CC RID: 12492 RVA: 0x000D687D File Offset: 0x000D587D
		private ArrayList EnabledControls
		{
			get
			{
				if (this._enabledControls == null)
				{
					this._enabledControls = new ArrayList();
				}
				return this._enabledControls;
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x060030CD RID: 12493 RVA: 0x000D6898 File Offset: 0x000D5898
		internal string FocusedControlID
		{
			get
			{
				if (this._focusedControlID == null)
				{
					return string.Empty;
				}
				return this._focusedControlID;
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x060030CE RID: 12494 RVA: 0x000D68AE File Offset: 0x000D58AE
		internal Control FocusedControl
		{
			get
			{
				return this._focusedControl;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x060030CF RID: 12495 RVA: 0x000D68B6 File Offset: 0x000D58B6
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HtmlHead Header
		{
			get
			{
				return this._header;
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x060030D0 RID: 12496 RVA: 0x000D68BE File Offset: 0x000D58BE
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new virtual char IdSeparator
		{
			get
			{
				if (!this._haveIdSeparator)
				{
					if (this._adapter != null)
					{
						this._idSeparator = this.PageAdapter.IdSeparator;
					}
					else
					{
						this._idSeparator = base.IdSeparatorFromConfig;
					}
					this._haveIdSeparator = true;
				}
				return this._idSeparator;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x060030D1 RID: 12497 RVA: 0x000D68FC File Offset: 0x000D58FC
		internal string LastFocusedControl
		{
			[AspNetHostingPermission(SecurityAction.Assert, Level = AspNetHostingPermissionLevel.Low)]
			get
			{
				if (this.RequestInternal != null)
				{
					string text = this.Request["__LASTFOCUS"];
					if (text != null)
					{
						return text;
					}
				}
				return string.Empty;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x060030D2 RID: 12498 RVA: 0x000D692C File Offset: 0x000D592C
		// (set) Token: 0x060030D3 RID: 12499 RVA: 0x000D6950 File Offset: 0x000D5950
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool MaintainScrollPositionOnPostBack
		{
			get
			{
				return (this.RequestInternal == null || this.RequestInternal.Browser.SupportsMaintainScrollPositionOnPostback) && this._maintainScrollPosition;
			}
			set
			{
				if (this._maintainScrollPosition != value)
				{
					this._maintainScrollPosition = value;
					if (this._maintainScrollPosition)
					{
						this.LoadScrollPosition();
					}
				}
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x060030D4 RID: 12500 RVA: 0x000D6970 File Offset: 0x000D5970
		[Browsable(false)]
		[WebSysDescription("MasterPage_MasterPage")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MasterPage Master
		{
			get
			{
				if (this._master == null && !this._preInitWorkComplete)
				{
					this._master = MasterPage.CreateMaster(this, this.Context, this._masterPageFile, this._contentTemplateCollection);
				}
				return this._master;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x060030D5 RID: 12501 RVA: 0x000D69A6 File Offset: 0x000D59A6
		// (set) Token: 0x060030D6 RID: 12502 RVA: 0x000D69B4 File Offset: 0x000D59B4
		[WebSysDescription("MasterPage_MasterPageFile")]
		[DefaultValue("")]
		[WebCategory("Behavior")]
		public virtual string MasterPageFile
		{
			get
			{
				return VirtualPath.GetVirtualPathString(this._masterPageFile);
			}
			set
			{
				if (this._preInitWorkComplete)
				{
					throw new InvalidOperationException(SR.GetString("PropertySetBeforePageEvent", new object[] { "MasterPageFile", "Page_PreInit" }));
				}
				if (value != VirtualPath.GetVirtualPathString(this._masterPageFile))
				{
					this._masterPageFile = VirtualPath.CreateAllowNull(value);
					if (this._master != null && this.Controls.Contains(this._master))
					{
						this.Controls.Remove(this._master);
					}
					this._master = null;
				}
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x060030D7 RID: 12503 RVA: 0x000D6A43 File Offset: 0x000D5A43
		// (set) Token: 0x060030D8 RID: 12504 RVA: 0x000D6A4C File Offset: 0x000D5A4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int MaxPageStateFieldLength
		{
			get
			{
				return this._maxPageStateFieldLength;
			}
			set
			{
				if (base.ControlState > ControlState.FrameworkInitialized)
				{
					throw new InvalidOperationException(SR.GetString("PropertySetAfterFrameworkInitialize", new object[] { "MaxPageStateFieldLength" }));
				}
				if (value == 0 || value < -1)
				{
					throw new ArgumentException(SR.GetString("Page_Illegal_MaxPageStateFieldLength"), "MaxPageStateFieldLength");
				}
				this._maxPageStateFieldLength = value;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060030D9 RID: 12505 RVA: 0x000D6AA5 File Offset: 0x000D5AA5
		// (set) Token: 0x060030DA RID: 12506 RVA: 0x000D6AAD File Offset: 0x000D5AAD
		internal bool ContainsCrossPagePost
		{
			get
			{
				return this._containsCrossPagePost;
			}
			set
			{
				this._containsCrossPagePost = value;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x060030DB RID: 12507 RVA: 0x000D6AB6 File Offset: 0x000D5AB6
		internal bool RenderFocusScript
		{
			get
			{
				return this._requireFocusScript;
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x060030DC RID: 12508 RVA: 0x000D6ABE File Offset: 0x000D5ABE
		internal Stack PartialCachingControlStack
		{
			get
			{
				return this._partialCachingControlStack;
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x060030DD RID: 12509 RVA: 0x000D6AC8 File Offset: 0x000D5AC8
		protected virtual PageStatePersister PageStatePersister
		{
			get
			{
				if (this._persister == null)
				{
					PageAdapter pageAdapter = this.PageAdapter;
					if (pageAdapter != null)
					{
						this._persister = pageAdapter.GetStatePersister();
					}
					if (this._persister == null)
					{
						this._persister = new HiddenFieldPageStatePersister(this);
					}
				}
				return this._persister;
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x060030DE RID: 12510 RVA: 0x000D6B10 File Offset: 0x000D5B10
		internal string RequestViewStateString
		{
			get
			{
				if (!this._cachedRequestViewState)
				{
					StringBuilder stringBuilder = new StringBuilder();
					try
					{
						NameValueCollection requestValueCollection = this.RequestValueCollection;
						if (requestValueCollection != null)
						{
							string text = this.RequestValueCollection["__VIEWSTATEFIELDCOUNT"];
							if (this.MaxPageStateFieldLength == -1 || text == null)
							{
								this._cachedRequestViewState = true;
								this._requestViewState = this.RequestValueCollection["__VIEWSTATE"];
								return this._requestViewState;
							}
							int num = Convert.ToInt32(text, CultureInfo.InvariantCulture);
							if (num < 0)
							{
								throw new HttpException(SR.GetString("ViewState_InvalidViewState"));
							}
							for (int i = 0; i < num; i++)
							{
								string text2 = "__VIEWSTATE";
								if (i > 0)
								{
									text2 += i.ToString(CultureInfo.InvariantCulture);
								}
								string text3 = this.RequestValueCollection[text2];
								if (text3 == null)
								{
									throw new HttpException(SR.GetString("ViewState_MissingViewStateField", new object[] { text2 }));
								}
								stringBuilder.Append(text3);
							}
						}
						this._cachedRequestViewState = true;
						this._requestViewState = stringBuilder.ToString();
					}
					catch (Exception ex)
					{
						ViewStateException.ThrowViewStateError(ex, stringBuilder.ToString());
					}
				}
				return this._requestViewState;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x060030DF RID: 12511 RVA: 0x000D6C50 File Offset: 0x000D5C50
		internal string ValidatorInvalidControl
		{
			get
			{
				if (this._validatorInvalidControl == null)
				{
					return string.Empty;
				}
				return this._validatorInvalidControl;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x060030E0 RID: 12512 RVA: 0x000D6C66 File Offset: 0x000D5C66
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TraceContext Trace
		{
			get
			{
				return this.Context.Trace;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x060030E1 RID: 12513 RVA: 0x000D6C73 File Offset: 0x000D5C73
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public HttpRequest Request
		{
			get
			{
				if (this._request == null)
				{
					throw new HttpException(SR.GetString("Request_not_available"));
				}
				return this._request;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x060030E2 RID: 12514 RVA: 0x000D6C93 File Offset: 0x000D5C93
		internal HttpRequest RequestInternal
		{
			get
			{
				return this._request;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x060030E3 RID: 12515 RVA: 0x000D6C9B File Offset: 0x000D5C9B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HttpResponse Response
		{
			get
			{
				if (this._response == null)
				{
					throw new HttpException(SR.GetString("Response_not_available"));
				}
				return this._response;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x060030E4 RID: 12516 RVA: 0x000D6CBB File Offset: 0x000D5CBB
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HttpServerUtility Server
		{
			get
			{
				return this.Context.Server;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x060030E5 RID: 12517 RVA: 0x000D6CC8 File Offset: 0x000D5CC8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Cache Cache
		{
			get
			{
				if (this._cache == null)
				{
					throw new HttpException(SR.GetString("Cache_not_available"));
				}
				return this._cache;
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x060030E6 RID: 12518 RVA: 0x000D6CE8 File Offset: 0x000D5CE8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual HttpSessionState Session
		{
			get
			{
				if (!this._sessionRetrieved)
				{
					this._sessionRetrieved = true;
					try
					{
						this._session = this.Context.Session;
					}
					catch
					{
					}
				}
				if (this._session == null)
				{
					throw new HttpException(SR.GetString("Session_not_enabled"));
				}
				return this._session;
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x060030E7 RID: 12519 RVA: 0x000D6D48 File Offset: 0x000D5D48
		// (set) Token: 0x060030E8 RID: 12520 RVA: 0x000D6D9C File Offset: 0x000D5D9C
		[Localizable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Bindable(true)]
		public string Title
		{
			get
			{
				if (this.Page.Header == null && base.ControlState >= ControlState.ChildrenInitialized)
				{
					throw new InvalidOperationException(SR.GetString("Page_Title_Requires_Head"));
				}
				if (this._titleToBeSet != null)
				{
					return this._titleToBeSet;
				}
				return this.Page.Header.Title;
			}
			set
			{
				if (this.Page.Header != null)
				{
					this.Page.Header.Title = value;
					return;
				}
				if (base.ControlState >= ControlState.ChildrenInitialized)
				{
					throw new InvalidOperationException(SR.GetString("Page_Title_Requires_Head"));
				}
				this._titleToBeSet = value;
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x060030E9 RID: 12521 RVA: 0x000D6DE8 File Offset: 0x000D5DE8
		internal bool ContainsTheme
		{
			get
			{
				return this._theme != null;
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x060030EA RID: 12522 RVA: 0x000D6DF6 File Offset: 0x000D5DF6
		// (set) Token: 0x060030EB RID: 12523 RVA: 0x000D6E00 File Offset: 0x000D5E00
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual string Theme
		{
			get
			{
				return this._themeName;
			}
			set
			{
				if (this._preInitWorkComplete)
				{
					throw new InvalidOperationException(SR.GetString("PropertySetBeforePageEvent", new object[] { "Theme", "Page_PreInit" }));
				}
				if (!string.IsNullOrEmpty(value) && !FileUtil.IsValidDirectoryName(value))
				{
					throw new ArgumentException(SR.GetString("Page_theme_invalid_name", new object[] { value }), "Theme");
				}
				this._themeName = value;
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x060030EC RID: 12524 RVA: 0x000D6E78 File Offset: 0x000D5E78
		internal bool SupportsStyleSheets
		{
			get
			{
				if (this._supportsStyleSheets != -1)
				{
					return this._supportsStyleSheets == 1;
				}
				if (this.Header != null && this.Header.StyleSheet != null && this.RequestInternal != null && this.Request.Browser != null && this.Request.Browser["preferredRenderingType"] != "xhtml-mp" && this.Request.Browser.SupportsCss && !this.Page.IsCallback && (this.ScriptManager == null || !this.ScriptManager.IsInAsyncPostBack))
				{
					this._supportsStyleSheets = 1;
					return true;
				}
				this._supportsStyleSheets = 0;
				return false;
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x060030ED RID: 12525 RVA: 0x000D6F2E File Offset: 0x000D5F2E
		// (set) Token: 0x060030EE RID: 12526 RVA: 0x000D6F36 File Offset: 0x000D5F36
		[Filterable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual string StyleSheetTheme
		{
			get
			{
				return this._styleSheetName;
			}
			set
			{
				if (this._pageFlags[1])
				{
					throw new InvalidOperationException(SR.GetString("SetStyleSheetThemeCannotBeSet"));
				}
				this._styleSheetName = value;
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x060030EF RID: 12527 RVA: 0x000D6F5D File Offset: 0x000D5F5D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPrincipal User
		{
			get
			{
				return this.Context.User;
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x060030F0 RID: 12528 RVA: 0x000D6F6A File Offset: 0x000D5F6A
		internal XhtmlConformanceMode XhtmlConformanceMode
		{
			get
			{
				if (!this._xhtmlConformanceModeSet)
				{
					if (base.DesignMode)
					{
						this._xhtmlConformanceMode = XhtmlConformanceMode.Transitional;
					}
					else
					{
						this._xhtmlConformanceMode = base.GetXhtmlConformanceSection().Mode;
					}
					this._xhtmlConformanceModeSet = true;
				}
				return this._xhtmlConformanceMode;
			}
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000D6FA4 File Offset: 0x000D5FA4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal virtual HtmlTextWriter CreateHtmlTextWriter(TextWriter tw)
		{
			if (this.Context != null && this.Context.Request != null && this.Context.Request.Browser != null)
			{
				return this.Context.Request.Browser.CreateHtmlTextWriter(tw);
			}
			HtmlTextWriter htmlTextWriter = Page.CreateHtmlTextWriterInternal(tw, this._request);
			if (htmlTextWriter == null)
			{
				htmlTextWriter = new HtmlTextWriter(tw);
			}
			return htmlTextWriter;
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x000D7007 File Offset: 0x000D6007
		internal static HtmlTextWriter CreateHtmlTextWriterInternal(TextWriter tw, HttpRequest request)
		{
			if (request != null && request.Browser != null)
			{
				return request.Browser.CreateHtmlTextWriterInternal(tw);
			}
			return new Html32TextWriter(tw);
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x000D7028 File Offset: 0x000D6028
		public static HtmlTextWriter CreateHtmlTextWriterFromType(TextWriter tw, Type writerType)
		{
			if (writerType == typeof(HtmlTextWriter))
			{
				return new HtmlTextWriter(tw);
			}
			if (writerType == typeof(Html32TextWriter))
			{
				return new Html32TextWriter(tw);
			}
			HtmlTextWriter htmlTextWriter;
			try
			{
				Util.CheckAssignableType(typeof(HtmlTextWriter), writerType);
				htmlTextWriter = (HtmlTextWriter)HttpRuntime.CreateNonPublicInstance(writerType, new object[] { tw });
			}
			catch
			{
				throw new HttpException(SR.GetString("Invalid_HtmlTextWriter", new object[] { writerType.FullName }));
			}
			return htmlTextWriter;
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x000D70BC File Offset: 0x000D60BC
		public override Control FindControl(string id)
		{
			if (StringUtil.EqualsIgnoreCase(id, "__Page"))
			{
				return this;
			}
			return base.FindControl(id, 0);
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x000D70D5 File Offset: 0x000D60D5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual int GetTypeHashCode()
		{
			return 0;
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x000D70D8 File Offset: 0x000D60D8
		internal override string GetUniqueIDPrefix()
		{
			if (this.Parent == null)
			{
				return string.Empty;
			}
			return base.GetUniqueIDPrefix();
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x000D70F0 File Offset: 0x000D60F0
		internal uint GetClientStateIdentifier()
		{
			int hashCode = StringComparer.InvariantCultureIgnoreCase.GetHashCode(this.TemplateSourceDirectory);
			return (uint)(hashCode + StringComparer.InvariantCultureIgnoreCase.GetHashCode(base.GetType().Name));
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x000D7128 File Offset: 0x000D6128
		private bool HandleError(Exception e)
		{
			try
			{
				this.Context.TempError = e;
				this.OnError(EventArgs.Empty);
				if (this.Context.TempError == null)
				{
					return true;
				}
			}
			finally
			{
				this.Context.TempError = null;
			}
			if (!string.IsNullOrEmpty(this._errorPage) && this.Context.IsCustomErrorEnabled)
			{
				this._response.RedirectToErrorPage(this._errorPage, CustomErrorsSection.GetSettings(this.Context).RedirectMode);
				return true;
			}
			PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_UNHANDLED);
			string text = null;
			if (this.Context.TraceIsEnabled)
			{
				this.Trace.Warn(SR.GetString("Unhandled_Err_Error"), null, e);
				if (this.Trace.PageOutput)
				{
					StringWriter stringWriter = new StringWriter();
					HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
					this.BuildPageProfileTree(false);
					this.Trace.EndRequest();
					this.Trace.StopTracing();
					this.Trace.StatusCode = 500;
					this.Trace.Render(htmlTextWriter);
					text = stringWriter.ToString();
				}
			}
			if (HttpException.GetErrorFormatter(e) != null)
			{
				return false;
			}
			if (e is SecurityException)
			{
				return false;
			}
			throw new HttpUnhandledException(null, text, e);
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x060030F9 RID: 12537 RVA: 0x000D7264 File Offset: 0x000D6264
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsCrossPagePostBack
		{
			get
			{
				return this._isCrossPagePostBack;
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x060030FA RID: 12538 RVA: 0x000D726C File Offset: 0x000D626C
		internal bool IsExportingWebPart
		{
			get
			{
				return this._pageFlags[2];
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x060030FB RID: 12539 RVA: 0x000D727A File Offset: 0x000D627A
		internal bool IsExportingWebPartShared
		{
			get
			{
				return this._pageFlags[4];
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x060030FC RID: 12540 RVA: 0x000D7288 File Offset: 0x000D6288
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsPostBack
		{
			get
			{
				return this._requestValueCollection != null && (this._isCrossPagePostBack || (!this._pageFlags[8] && !this.ViewStateMacValidationErrorWasSuppressed && (this.Context.ServerExecuteDepth <= 0 || (this.Context.Handler != null && base.GetType() == this.Context.Handler.GetType())) && !this._fPageLayoutChanged));
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x000D7301 File Offset: 0x000D6301
		internal NameValueCollection RequestValueCollection
		{
			get
			{
				return this._requestValueCollection;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x060030FE RID: 12542 RVA: 0x000D7309 File Offset: 0x000D6309
		// (set) Token: 0x060030FF RID: 12543 RVA: 0x000D7314 File Offset: 0x000D6314
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DefaultValue(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool EnableEventValidation
		{
			get
			{
				return this._enableEventValidation;
			}
			set
			{
				if (base.ControlState > ControlState.FrameworkInitialized)
				{
					throw new InvalidOperationException(SR.GetString("PropertySetAfterFrameworkInitialize", new object[] { "EnableEventValidation" }));
				}
				this._enableEventValidation = value;
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06003100 RID: 12544 RVA: 0x000D7351 File Offset: 0x000D6351
		// (set) Token: 0x06003101 RID: 12545 RVA: 0x000D7359 File Offset: 0x000D6359
		[Browsable(false)]
		public override bool EnableViewState
		{
			get
			{
				return base.EnableViewState;
			}
			set
			{
				base.EnableViewState = value;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06003102 RID: 12546 RVA: 0x000D7362 File Offset: 0x000D6362
		// (set) Token: 0x06003103 RID: 12547 RVA: 0x000D736C File Offset: 0x000D636C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(ViewStateEncryptionMode.Auto)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public ViewStateEncryptionMode ViewStateEncryptionMode
		{
			get
			{
				return this._encryptionMode;
			}
			set
			{
				if (base.ControlState > ControlState.FrameworkInitialized)
				{
					throw new InvalidOperationException(SR.GetString("PropertySetAfterFrameworkInitialize", new object[] { "ViewStateEncryptionMode" }));
				}
				if (value < ViewStateEncryptionMode.Auto || value > ViewStateEncryptionMode.Never)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._encryptionMode = value;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06003104 RID: 12548 RVA: 0x000D73BC File Offset: 0x000D63BC
		// (set) Token: 0x06003105 RID: 12549 RVA: 0x000D73C4 File Offset: 0x000D63C4
		[Browsable(false)]
		public string ViewStateUserKey
		{
			get
			{
				return this._viewStateUserKey;
			}
			set
			{
				if (base.ControlState >= ControlState.Initialized)
				{
					throw new HttpException(SR.GetString("Too_late_for_ViewStateUserKey"));
				}
				this._viewStateUserKey = value;
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06003106 RID: 12550 RVA: 0x000D73E6 File Offset: 0x000D63E6
		// (set) Token: 0x06003107 RID: 12551 RVA: 0x000D73EE File Offset: 0x000D63EE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
				base.ID = value;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06003108 RID: 12552 RVA: 0x000D73F7 File Offset: 0x000D63F7
		// (set) Token: 0x06003109 RID: 12553 RVA: 0x000D73FF File Offset: 0x000D63FF
		[Browsable(false)]
		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x000D7408 File Offset: 0x000D6408
		internal static string DecryptString(string s)
		{
			return Page.DecryptStringWithIV(s, IVType.Hash);
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x000D7414 File Offset: 0x000D6414
		internal static string DecryptStringWithIV(string s, IVType ivType)
		{
			if (s == null)
			{
				return null;
			}
			byte[] array = HttpServerUtility.UrlTokenDecode(s);
			if (array != null)
			{
				array = MachineKeySection.EncryptOrDecryptData(false, array, null, 0, array.Length, ivType);
			}
			if (array == null)
			{
				throw new HttpException(SR.GetString("ViewState_InvalidViewState"));
			}
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x000D745C File Offset: 0x000D645C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void DesignerInitialize()
		{
			this.InitRecursive(null);
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x000D7468 File Offset: 0x000D6468
		internal NameValueCollection GetCollectionBasedOnMethod(bool dontReturnNull)
		{
			if (this._request.HttpVerb == HttpVerb.POST)
			{
				if (!dontReturnNull && !this._request.HasForm)
				{
					return null;
				}
				return this._request.Form;
			}
			else
			{
				if (!dontReturnNull && !this._request.HasQueryString)
				{
					return null;
				}
				return this._request.QueryString;
			}
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x000D74C0 File Offset: 0x000D64C0
		private bool DetermineIsExportingWebPart()
		{
			byte[] queryStringBytes = this.Request.QueryStringBytes;
			if (queryStringBytes == null || queryStringBytes.Length < 28)
			{
				return false;
			}
			if (queryStringBytes[0] != 95 || queryStringBytes[1] != 95 || queryStringBytes[2] != 87 || queryStringBytes[3] != 69 || queryStringBytes[4] != 66 || queryStringBytes[5] != 80 || queryStringBytes[6] != 65 || queryStringBytes[7] != 82 || queryStringBytes[8] != 84 || queryStringBytes[9] != 69 || queryStringBytes[10] != 88 || queryStringBytes[11] != 80 || queryStringBytes[12] != 79 || queryStringBytes[13] != 82 || queryStringBytes[14] != 84 || queryStringBytes[15] != 61 || queryStringBytes[16] != 116 || queryStringBytes[17] != 114 || queryStringBytes[18] != 117 || queryStringBytes[19] != 101 || queryStringBytes[20] != 38)
			{
				return false;
			}
			this._pageFlags.Set(2);
			return true;
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x000D75A0 File Offset: 0x000D65A0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal virtual NameValueCollection DeterminePostBackMode()
		{
			if (this.Context.Request == null)
			{
				return null;
			}
			if (this.Context.PreventPostback)
			{
				return null;
			}
			NameValueCollection nameValueCollection = this.GetCollectionBasedOnMethod(false);
			if (nameValueCollection == null)
			{
				return null;
			}
			bool flag = false;
			string[] values = nameValueCollection.GetValues(null);
			if (values != null)
			{
				int num = values.Length;
				for (int i = 0; i < num; i++)
				{
					if (values[i].StartsWith("__VIEWSTATE", StringComparison.Ordinal) || values[i] == "__EVENTTARGET")
					{
						flag = true;
						break;
					}
				}
			}
			if (nameValueCollection["__VIEWSTATE"] == null && nameValueCollection["__VIEWSTATEFIELDCOUNT"] == null && nameValueCollection["__EVENTTARGET"] == null && !flag)
			{
				nameValueCollection = null;
			}
			else if (this.Request.QueryStringText.IndexOf(HttpResponse.RedirectQueryStringAssignment, StringComparison.Ordinal) != -1)
			{
				nameValueCollection = null;
			}
			return nameValueCollection;
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x000D7668 File Offset: 0x000D6668
		internal static string EncryptString(string s)
		{
			return Page.EncryptStringWithIV(s, IVType.Hash);
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x000D7674 File Offset: 0x000D6674
		internal static string EncryptStringWithIV(string s, IVType ivType)
		{
			byte[] array = Encoding.UTF8.GetBytes(s);
			array = MachineKeySection.EncryptOrDecryptData(true, array, null, 0, array.Length, ivType);
			return HttpServerUtility.UrlTokenEncode(array);
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x000D76A4 File Offset: 0x000D66A4
		private void LoadAllState()
		{
			object obj = this.LoadPageStateFromPersistenceMedium();
			IDictionary dictionary = null;
			Pair pair = null;
			Pair pair2 = obj as Pair;
			if (obj != null)
			{
				dictionary = pair2.First as IDictionary;
				pair = pair2.Second as Pair;
			}
			if (dictionary != null)
			{
				this._controlsRequiringPostBack = (ArrayList)dictionary["__ControlsRequirePostBackKey__"];
				if (this._registeredControlsRequiringControlState != null)
				{
					foreach (object obj2 in ((IEnumerable)this._registeredControlsRequiringControlState))
					{
						Control control = (Control)obj2;
						control.LoadControlStateInternal(dictionary[control.UniqueID]);
					}
				}
			}
			if (pair != null)
			{
				string text = (string)pair.First;
				int num = int.Parse(text, NumberFormatInfo.InvariantInfo);
				this._fPageLayoutChanged = num != this.GetTypeHashCode();
				if (!this._fPageLayoutChanged)
				{
					base.LoadViewStateRecursive(pair.Second);
				}
			}
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x000D77A8 File Offset: 0x000D67A8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal virtual object LoadPageStateFromPersistenceMedium()
		{
			PageStatePersister pageStatePersister = this.PageStatePersister;
			try
			{
				pageStatePersister.Load();
			}
			catch (HttpException ex)
			{
				if (this._pageFlags[8])
				{
					return null;
				}
				if (this.ShouldSuppressMacValidationException(ex))
				{
					if (this.Context != null && this.Context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "Ignoring page state", ex);
					}
					this.ViewStateMacValidationErrorWasSuppressed = true;
					return null;
				}
				ex.WebEventCode = 3002;
				throw;
			}
			return new Pair(pageStatePersister.ControlState, pageStatePersister.ViewState);
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06003114 RID: 12564 RVA: 0x000D7848 File Offset: 0x000D6848
		// (set) Token: 0x06003115 RID: 12565 RVA: 0x000D7857 File Offset: 0x000D6857
		private bool ViewStateMacValidationErrorWasSuppressed
		{
			get
			{
				return this._pageFlags[64];
			}
			set
			{
				this._pageFlags[64] = value;
			}
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x000D7868 File Offset: 0x000D6868
		internal bool ShouldSuppressMacValidationException(Exception e)
		{
			if (!EnableViewStateMacRegistryHelper.SuppressMacValidationErrorsFromCrossPagePostbacks)
			{
				return false;
			}
			if (ViewStateException.IsMacValidationException(e))
			{
				if (EnableViewStateMacRegistryHelper.SuppressMacValidationErrorsAlways)
				{
					return true;
				}
				if (this._requestValueCollection == null)
				{
					return true;
				}
				if (!this.VerifyClientStateIdentifier(this._requestValueCollection["__VIEWSTATEGENERATOR"]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x000D78B4 File Offset: 0x000D68B4
		private bool VerifyClientStateIdentifier(string identifier)
		{
			uint num;
			return identifier != null && uint.TryParse(identifier, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num) && num == this.GetClientStateIdentifier();
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x000D78E4 File Offset: 0x000D68E4
		internal void LoadScrollPosition()
		{
			if (this._previousPagePath != null)
			{
				return;
			}
			if (this._requestValueCollection != null)
			{
				string text = this._requestValueCollection["__SCROLLPOSITIONX"];
				if (text != null && !int.TryParse(text, out this._scrollPositionX))
				{
					this._scrollPositionX = 0;
				}
				string text2 = this._requestValueCollection["__SCROLLPOSITIONY"];
				if (text2 != null && !int.TryParse(text2, out this._scrollPositionY))
				{
					this._scrollPositionY = 0;
				}
			}
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x000D795A File Offset: 0x000D695A
		internal IStateFormatter CreateStateFormatter()
		{
			return new ObjectStateFormatter(this, true);
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x000D7964 File Offset: 0x000D6964
		internal ICollection DecomposeViewStateIntoChunks()
		{
			string clientState = this.ClientState;
			if (clientState == null)
			{
				return null;
			}
			if (this.MaxPageStateFieldLength <= 0)
			{
				return new ArrayList(1) { clientState };
			}
			int num = this.ClientState.Length / this.MaxPageStateFieldLength;
			ArrayList arrayList = new ArrayList(num + 1);
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				arrayList.Add(clientState.Substring(num2, this.MaxPageStateFieldLength));
				num2 += this.MaxPageStateFieldLength;
			}
			if (num2 < clientState.Length)
			{
				arrayList.Add(clientState.Substring(num2));
			}
			if (arrayList.Count == 0)
			{
				arrayList.Add(string.Empty);
			}
			return arrayList;
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x000D7A14 File Offset: 0x000D6A14
		internal void RenderViewStateFields(HtmlTextWriter writer)
		{
			if (this.ClientState != null)
			{
				ICollection collection = this.DecomposeViewStateIntoChunks();
				writer.WriteLine();
				if (collection.Count > 1)
				{
					writer.Write("<input type=\"hidden\" name=\"");
					writer.Write("__VIEWSTATEFIELDCOUNT");
					writer.Write("\" id=\"");
					writer.Write("__VIEWSTATEFIELDCOUNT");
					writer.Write("\" value=\"");
					writer.Write(collection.Count.ToString(CultureInfo.InvariantCulture));
					writer.WriteLine("\" />");
				}
				int num = 0;
				foreach (object obj in collection)
				{
					string text = (string)obj;
					writer.Write("<input type=\"hidden\" name=\"");
					writer.Write("__VIEWSTATE");
					string text2 = null;
					if (num > 0)
					{
						text2 = num.ToString(CultureInfo.InvariantCulture);
						writer.Write(text2);
					}
					writer.Write("\" id=\"");
					writer.Write("__VIEWSTATE");
					if (num > 0)
					{
						writer.Write(text2);
					}
					writer.Write("\" value=\"");
					writer.Write(text);
					writer.WriteLine("\" />");
					num++;
				}
				if (EnableViewStateMacRegistryHelper.WriteViewStateGeneratorField)
				{
					this.ClientScript.RegisterHiddenField("__VIEWSTATEGENERATOR", this.GetClientStateIdentifier().ToString("X8", CultureInfo.InvariantCulture));
					return;
				}
			}
			else
			{
				writer.Write("\r\n<input type=\"hidden\" name=\"");
				writer.Write("__VIEWSTATE");
				writer.WriteLine("\" id=\"");
				writer.Write("__VIEWSTATE");
				writer.WriteLine("\" value=\"\" />");
			}
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x000D7BC4 File Offset: 0x000D6BC4
		internal void BeginFormRender(HtmlTextWriter writer, string formUniqueID)
		{
			bool flag = writer.RenderDivAroundHiddenInputs && !base.EnableLegacyRendering;
			if (flag)
			{
				writer.WriteLine();
				writer.Write("<div>");
			}
			this.ClientScript.RenderHiddenFields(writer);
			this.RenderViewStateFields(writer);
			if (flag)
			{
				writer.WriteLine("</div>");
			}
			if (this.ClientSupportsJavaScript)
			{
				if (this.MaintainScrollPositionOnPostBack && !this._requireScrollScript)
				{
					this.ClientScript.RegisterHiddenField("__SCROLLPOSITIONX", this._scrollPositionX.ToString(CultureInfo.InvariantCulture));
					this.ClientScript.RegisterHiddenField("__SCROLLPOSITIONY", this._scrollPositionY.ToString(CultureInfo.InvariantCulture));
					this.ClientScript.RegisterStartupScript(typeof(Page), "PageScrollPositionScript", "\r\ntheForm.oldSubmit = theForm.submit;\r\ntheForm.submit = WebForm_SaveScrollPositionSubmit;\r\n\r\ntheForm.oldOnSubmit = theForm.onsubmit;\r\ntheForm.onsubmit = WebForm_SaveScrollPositionOnSubmit;\r\n" + (this.IsPostBack ? "\r\ntheForm.oldOnLoad = window.onload;\r\nwindow.onload = WebForm_RestoreScrollPosition;\r\n" : string.Empty), true);
					this.RegisterWebFormsScript();
					this._requireScrollScript = true;
				}
				if (this.ClientSupportsFocus && this.Form != null && (this.RenderFocusScript || this.Form.DefaultFocus.Length > 0 || this.Form.DefaultButton.Length > 0))
				{
					string text = string.Empty;
					if (this.FocusedControlID.Length > 0)
					{
						text = this.FocusedControlID;
					}
					else if (this.FocusedControl != null)
					{
						if (this.FocusedControl.Visible)
						{
							text = this.FocusedControl.ClientID;
						}
					}
					else if (this.ValidatorInvalidControl.Length > 0)
					{
						text = this.ValidatorInvalidControl;
					}
					else if (this.LastFocusedControl.Length > 0)
					{
						text = this.LastFocusedControl;
					}
					else if (this.Form.DefaultFocus.Length > 0)
					{
						text = this.Form.DefaultFocus;
					}
					else if (this.Form.DefaultButton.Length > 0)
					{
						text = this.Form.DefaultButton;
					}
					int num;
					if (text.Length > 0 && !CrossSiteScriptingValidation.IsDangerousString(text, out num) && CrossSiteScriptingValidation.IsValidJavascriptId(text))
					{
						this.ClientScript.RegisterClientScriptResource(typeof(HtmlForm), "Focus.js");
						if (!this.ClientScript.IsClientScriptBlockRegistered(typeof(HtmlForm), "Focus"))
						{
							this.RegisterWebFormsScript();
							this.ClientScript.RegisterStartupScript(typeof(HtmlForm), "Focus", "WebForm_AutoFocus('" + Util.QuoteJScriptString(text) + "');", true);
						}
						IScriptManager scriptManager = this.ScriptManager;
						if (scriptManager != null)
						{
							scriptManager.SetFocusInternal(text);
						}
					}
				}
				if (this.Form.SubmitDisabledControls && this.EnabledControls.Count > 0 && this._request.Browser.W3CDomVersion.Major > 0)
				{
					foreach (object obj in this.EnabledControls)
					{
						Control control = (Control)obj;
						this.ClientScript.RegisterArrayDeclaration("__enabledControlArray", "'" + control.ClientID + "'");
					}
					this.ClientScript.RegisterOnSubmitStatement(typeof(Page), "PageReEnableControlsScript", "WebForm_ReEnableControls();");
					this.RegisterWebFormsScript();
				}
				if (this._fRequirePostBackScript)
				{
					this.RenderPostBackScript(writer, formUniqueID);
				}
				if (this._fRequireWebFormsScript)
				{
					this.RenderWebFormsScript(writer);
				}
			}
			this.ClientScript.RenderClientScriptBlocks(writer);
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x000D7F58 File Offset: 0x000D6F58
		internal void EndFormRenderArrayAndExpandoAttribute(HtmlTextWriter writer, string formUniqueID)
		{
			if (this.ClientSupportsJavaScript)
			{
				this.ClientScript.RenderArrayDeclares(writer);
				this.ClientScript.RenderExpandoAttribute(writer);
			}
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000D7F7C File Offset: 0x000D6F7C
		internal void EndFormRenderHiddenFields(HtmlTextWriter writer, string formUniqueID)
		{
			if (this.RequiresViewStateEncryptionInternal)
			{
				this.ClientScript.RegisterHiddenField("__VIEWSTATEENCRYPTED", string.Empty);
			}
			if (this._containsCrossPagePost)
			{
				string text = Page.EncryptString(this.Request.CurrentExecutionFilePath);
				this.ClientScript.RegisterHiddenField("__PREVIOUSPAGE", text);
			}
			if (this.EnableEventValidation)
			{
				this.ClientScript.SaveEventValidationField();
			}
			if (this.ClientScript.HasRegisteredHiddenFields)
			{
				bool flag = writer.RenderDivAroundHiddenInputs && !base.EnableLegacyRendering;
				if (flag)
				{
					writer.WriteLine();
					writer.RenderBeginTag(HtmlTextWriterTag.Div);
				}
				this.ClientScript.RenderHiddenFields(writer);
				if (flag)
				{
					writer.RenderEndTag();
				}
			}
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x000D802C File Offset: 0x000D702C
		internal void EndFormRenderPostBackAndWebFormsScript(HtmlTextWriter writer, string formUniqueID)
		{
			if (this.ClientSupportsJavaScript)
			{
				if (this._fRequirePostBackScript && !this._fPostBackScriptRendered)
				{
					this.RenderPostBackScript(writer, formUniqueID);
				}
				if (this._fRequireWebFormsScript && !this._fWebFormsScriptRendered)
				{
					this.RenderWebFormsScript(writer);
				}
			}
			this.ClientScript.RenderClientStartupScripts(writer);
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x000D807C File Offset: 0x000D707C
		internal void EndFormRender(HtmlTextWriter writer, string formUniqueID)
		{
			this.EndFormRenderArrayAndExpandoAttribute(writer, formUniqueID);
			this.EndFormRenderHiddenFields(writer, formUniqueID);
			this.EndFormRenderPostBackAndWebFormsScript(writer, formUniqueID);
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06003121 RID: 12577 RVA: 0x000D8096 File Offset: 0x000D7096
		internal bool IsInOnFormRender
		{
			get
			{
				return this._inOnFormRender;
			}
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x000D809E File Offset: 0x000D709E
		internal void OnFormRender()
		{
			if (this._fOnFormRenderCalled)
			{
				throw new HttpException(SR.GetString("Multiple_forms_not_allowed"));
			}
			this._fOnFormRenderCalled = true;
			this._inOnFormRender = true;
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x000D80C6 File Offset: 0x000D70C6
		internal void OnFormPostRender()
		{
			this._inOnFormRender = false;
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x000D80CF File Offset: 0x000D70CF
		internal void ResetOnFormRenderCalled()
		{
			this._fOnFormRenderCalled = false;
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x000D80D8 File Offset: 0x000D70D8
		public void SetFocus(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (this.Form == null)
			{
				throw new InvalidOperationException(SR.GetString("Form_Required_For_Focus"));
			}
			if (this.Form.ControlState == ControlState.PreRendered)
			{
				throw new InvalidOperationException(SR.GetString("Page_MustCallBeforeAndDuringPreRender", new object[] { "SetFocus" }));
			}
			this._focusedControl = control;
			this._focusedControlID = null;
			this.RegisterFocusScript();
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x000D8150 File Offset: 0x000D7150
		public void SetFocus(string clientID)
		{
			if (clientID == null || clientID.Trim().Length == 0)
			{
				throw new ArgumentNullException("clientID");
			}
			if (this.Form == null)
			{
				throw new InvalidOperationException(SR.GetString("Form_Required_For_Focus"));
			}
			if (this.Form.ControlState == ControlState.PreRendered)
			{
				throw new InvalidOperationException(SR.GetString("Page_MustCallBeforeAndDuringPreRender", new object[] { "SetFocus" }));
			}
			this._focusedControlID = clientID.Trim();
			this._focusedControl = null;
			this.RegisterFocusScript();
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x000D81D7 File Offset: 0x000D71D7
		internal void SetValidatorInvalidControlFocus(string clientID)
		{
			if (string.IsNullOrEmpty(this._validatorInvalidControl))
			{
				this._validatorInvalidControl = clientID;
				this.RegisterFocusScript();
			}
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x000D81F3 File Offset: 0x000D71F3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Obsolete("The recommended alternative is ClientScript.GetPostBackEventReference. http://go.microsoft.com/fwlink/?linkid=14202")]
		public string GetPostBackEventReference(Control control)
		{
			return this.ClientScript.GetPostBackEventReference(control, string.Empty);
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x000D8206 File Offset: 0x000D7206
		[Obsolete("The recommended alternative is ClientScript.GetPostBackEventReference. http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public string GetPostBackEventReference(Control control, string argument)
		{
			return this.ClientScript.GetPostBackEventReference(control, argument);
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x000D8215 File Offset: 0x000D7215
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Obsolete("The recommended alternative is ClientScript.GetPostBackEventReference. http://go.microsoft.com/fwlink/?linkid=14202")]
		public string GetPostBackClientEvent(Control control, string argument)
		{
			return this.ClientScript.GetPostBackEventReference(control, argument);
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x000D8224 File Offset: 0x000D7224
		[Obsolete("The recommended alternative is ClientScript.GetPostBackClientHyperlink. http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public string GetPostBackClientHyperlink(Control control, string argument)
		{
			return this.ClientScript.GetPostBackClientHyperlink(control, argument, false);
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000D8234 File Offset: 0x000D7234
		internal void InitializeStyleSheet()
		{
			if (this._pageFlags[1])
			{
				return;
			}
			string styleSheetTheme = this.StyleSheetTheme;
			if (!string.IsNullOrEmpty(styleSheetTheme))
			{
				BuildResultCompiledType themeBuildResultType = ThemeDirectoryCompiler.GetThemeBuildResultType(this.Context, styleSheetTheme);
				if (themeBuildResultType == null)
				{
					throw new HttpException(SR.GetString("Page_theme_not_found", new object[] { styleSheetTheme }));
				}
				this._styleSheet = (PageTheme)themeBuildResultType.CreateInstance();
				this._styleSheet.Initialize(this, true);
			}
			this._pageFlags.Set(1);
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x000D82B8 File Offset: 0x000D72B8
		private void InitializeThemes()
		{
			string theme = this.Theme;
			if (string.IsNullOrEmpty(theme))
			{
				return;
			}
			BuildResultCompiledType themeBuildResultType = ThemeDirectoryCompiler.GetThemeBuildResultType(this.Context, theme);
			if (themeBuildResultType != null)
			{
				this._theme = (PageTheme)themeBuildResultType.CreateInstance();
				this._theme.Initialize(this, false);
				return;
			}
			throw new HttpException(SR.GetString("Page_theme_not_found", new object[] { theme }));
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x000D8320 File Offset: 0x000D7320
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected internal void AddContentTemplate(string templateName, ITemplate template)
		{
			if (this._contentTemplateCollection == null)
			{
				this._contentTemplateCollection = new Hashtable(11, StringComparer.OrdinalIgnoreCase);
			}
			try
			{
				this._contentTemplateCollection.Add(templateName, template);
			}
			catch (ArgumentException)
			{
				throw new HttpException(SR.GetString("MasterPage_Multiple_content", new object[] { templateName }));
			}
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x000D8384 File Offset: 0x000D7384
		private void ApplyMasterPage()
		{
			if (this.Master != null)
			{
				ArrayList arrayList = new ArrayList();
				arrayList.Add(this._masterPageFile.VirtualPathString.ToLower(CultureInfo.InvariantCulture));
				MasterPage.ApplyMasterRecursive(this.Master, arrayList);
			}
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x000D83C7 File Offset: 0x000D73C7
		internal void ApplyControlSkin(Control ctrl)
		{
			if (this._theme != null)
			{
				this._theme.ApplyControlSkin(ctrl);
			}
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x000D83DD File Offset: 0x000D73DD
		internal bool ApplyControlStyleSheet(Control ctrl)
		{
			if (this._styleSheet != null)
			{
				this._styleSheet.ApplyControlSkin(ctrl);
				return true;
			}
			return false;
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x000D83F8 File Offset: 0x000D73F8
		internal void RegisterFocusScript()
		{
			if (this.ClientSupportsFocus && !this._requireFocusScript)
			{
				this.ClientScript.RegisterHiddenField("__LASTFOCUS", string.Empty);
				this._requireFocusScript = true;
				if (this._partialCachingControlStack != null)
				{
					foreach (object obj in this._partialCachingControlStack)
					{
						BasePartialCachingControl basePartialCachingControl = (BasePartialCachingControl)obj;
						basePartialCachingControl.RegisterFocusScript();
					}
				}
			}
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x000D8484 File Offset: 0x000D7484
		internal void RegisterPostBackScript()
		{
			if (!this.ClientSupportsJavaScript)
			{
				return;
			}
			if (this._fPostBackScriptRendered)
			{
				return;
			}
			if (!this._fRequirePostBackScript)
			{
				this.ClientScript.RegisterHiddenField("__EVENTTARGET", string.Empty);
				this.ClientScript.RegisterHiddenField("__EVENTARGUMENT", string.Empty);
				this._fRequirePostBackScript = true;
			}
			if (this._partialCachingControlStack != null)
			{
				foreach (object obj in this._partialCachingControlStack)
				{
					BasePartialCachingControl basePartialCachingControl = (BasePartialCachingControl)obj;
					basePartialCachingControl.RegisterPostBackScript();
				}
			}
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x000D8530 File Offset: 0x000D7530
		private void RenderPostBackScript(HtmlTextWriter writer, string formUniqueID)
		{
			writer.Write(base.EnableLegacyRendering ? "\r\n<script type=\"text/javascript\">\r\n<!--\r\n" : "\r\n<script type=\"text/javascript\">\r\n//<![CDATA[\r\n");
			if (this.PageAdapter != null)
			{
				writer.Write("var theForm = ");
				writer.Write(this.PageAdapter.GetPostBackFormReference(formUniqueID));
				writer.WriteLine(";");
			}
			else
			{
				writer.Write("var theForm = document.forms['");
				writer.Write(formUniqueID);
				writer.WriteLine("'];");
				writer.Write("if (!theForm) {\r\n    theForm = document.");
				writer.Write(formUniqueID);
				writer.WriteLine(";\r\n}");
			}
			writer.WriteLine("function __doPostBack(eventTarget, eventArgument) {\r\n    if (!theForm.onsubmit || (theForm.onsubmit() != false)) {\r\n        theForm.__EVENTTARGET.value = eventTarget;\r\n        theForm.__EVENTARGUMENT.value = eventArgument;\r\n        theForm.submit();\r\n    }\r\n}");
			writer.WriteLine(base.EnableLegacyRendering ? "// -->\r\n</script>\r\n" : "//]]>\r\n</script>\r\n");
			this._fPostBackScriptRendered = true;
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x000D85F0 File Offset: 0x000D75F0
		internal void RegisterWebFormsScript()
		{
			if (this.ClientSupportsJavaScript)
			{
				if (this._fWebFormsScriptRendered)
				{
					return;
				}
				this.RegisterPostBackScript();
				this._fRequireWebFormsScript = true;
				if (this._partialCachingControlStack != null)
				{
					foreach (object obj in this._partialCachingControlStack)
					{
						BasePartialCachingControl basePartialCachingControl = (BasePartialCachingControl)obj;
						basePartialCachingControl.RegisterWebFormsScript();
					}
				}
			}
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000D8670 File Offset: 0x000D7670
		private void RenderWebFormsScript(HtmlTextWriter writer)
		{
			this.ClientScript.RenderWebFormsScript(writer);
			this._fWebFormsScriptRendered = true;
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x000D8685 File Offset: 0x000D7685
		[Obsolete("The recommended alternative is ClientScript.IsClientScriptBlockRegistered(string key). http://go.microsoft.com/fwlink/?linkid=14202")]
		public bool IsClientScriptBlockRegistered(string key)
		{
			return this.ClientScript.IsClientScriptBlockRegistered(typeof(Page), key);
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x000D869D File Offset: 0x000D769D
		[Obsolete("The recommended alternative is ClientScript.IsStartupScriptRegistered(string key). http://go.microsoft.com/fwlink/?linkid=14202")]
		public bool IsStartupScriptRegistered(string key)
		{
			return this.ClientScript.IsStartupScriptRegistered(typeof(Page), key);
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x000D86B5 File Offset: 0x000D76B5
		[Obsolete("The recommended alternative is ClientScript.RegisterArrayDeclaration(string arrayName, string arrayValue). http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void RegisterArrayDeclaration(string arrayName, string arrayValue)
		{
			this.ClientScript.RegisterArrayDeclaration(arrayName, arrayValue);
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x000D86C4 File Offset: 0x000D76C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Obsolete("The recommended alternative is ClientScript.RegisterHiddenField(string hiddenFieldName, string hiddenFieldInitialValue). http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual void RegisterHiddenField(string hiddenFieldName, string hiddenFieldInitialValue)
		{
			this.ClientScript.RegisterHiddenField(hiddenFieldName, hiddenFieldInitialValue);
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x000D86D3 File Offset: 0x000D76D3
		[Obsolete("The recommended alternative is ClientScript.RegisterClientScriptBlock(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void RegisterClientScriptBlock(string key, string script)
		{
			this.ClientScript.RegisterClientScriptBlock(typeof(Page), key, script);
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x000D86EC File Offset: 0x000D76EC
		[Obsolete("The recommended alternative is ClientScript.RegisterStartupScript(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void RegisterStartupScript(string key, string script)
		{
			this.ClientScript.RegisterStartupScript(typeof(Page), key, script, false);
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x000D8706 File Offset: 0x000D7706
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Obsolete("The recommended alternative is ClientScript.RegisterOnSubmitStatement(Type type, string key, string script). http://go.microsoft.com/fwlink/?linkid=14202")]
		public void RegisterOnSubmitStatement(string key, string script)
		{
			this.ClientScript.RegisterOnSubmitStatement(typeof(Page), key, script);
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x000D871F File Offset: 0x000D771F
		internal void RegisterEnabledControl(Control control)
		{
			this.EnabledControls.Add(control);
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x000D8730 File Offset: 0x000D7730
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void RegisterRequiresControlState(Control control)
		{
			if (control == null)
			{
				throw new ArgumentException(SR.GetString("Page_ControlState_ControlCannotBeNull"));
			}
			if (control.ControlState == ControlState.PreRendered)
			{
				throw new InvalidOperationException(SR.GetString("Page_MustCallBeforeAndDuringPreRender", new object[] { "RegisterRequiresControlState" }));
			}
			if (this._registeredControlsRequiringControlState == null)
			{
				this._registeredControlsRequiringControlState = new ControlSet();
			}
			if (!this._registeredControlsRequiringControlState.Contains(control))
			{
				this._registeredControlsRequiringControlState.Add(control);
				IDictionary dictionary = (IDictionary)this.PageStatePersister.ControlState;
				if (dictionary != null)
				{
					string uniqueID = control.UniqueID;
					if (!this.ControlStateLoadedControlIds.Contains(uniqueID))
					{
						control.LoadControlStateInternal(dictionary[uniqueID]);
						this.ControlStateLoadedControlIds.Add(uniqueID);
					}
				}
			}
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000D87E8 File Offset: 0x000D77E8
		public bool RequiresControlState(Control control)
		{
			return this._registeredControlsRequiringControlState != null && this._registeredControlsRequiringControlState.Contains(control);
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x000D8800 File Offset: 0x000D7800
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void UnregisterRequiresControlState(Control control)
		{
			if (control == null)
			{
				throw new ArgumentException(SR.GetString("Page_ControlState_ControlCannotBeNull"));
			}
			if (this._registeredControlsRequiringControlState == null)
			{
				return;
			}
			this._registeredControlsRequiringControlState.Remove(control);
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x000D882C File Offset: 0x000D782C
		internal bool ShouldLoadControlState(Control control)
		{
			if (this._registeredControlsRequiringClearChildControlState == null)
			{
				return true;
			}
			foreach (object obj in this._registeredControlsRequiringClearChildControlState.Keys)
			{
				Control control2 = (Control)obj;
				if (control != control2 && control.IsDescendentOf(control2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x000D88A4 File Offset: 0x000D78A4
		internal void RegisterRequiresClearChildControlState(Control control)
		{
			if (this._registeredControlsRequiringClearChildControlState == null)
			{
				this._registeredControlsRequiringClearChildControlState = new HybridDictionary();
				this._registeredControlsRequiringClearChildControlState.Add(control, true);
			}
			else if (this._registeredControlsRequiringClearChildControlState[control] == null)
			{
				this._registeredControlsRequiringClearChildControlState.Add(control, true);
			}
			IDictionary dictionary = (IDictionary)this.PageStatePersister.ControlState;
			if (dictionary != null)
			{
				List<string> list = new List<string>(dictionary.Count);
				foreach (object obj in dictionary.Keys)
				{
					string text = (string)obj;
					Control control2 = this.FindControl(text);
					if (control2 != null && control2.IsDescendentOf(control))
					{
						list.Add(text);
					}
				}
				foreach (string text2 in list)
				{
					dictionary[text2] = null;
				}
			}
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x000D89C4 File Offset: 0x000D79C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void RegisterRequiresPostBack(Control control)
		{
			if (!(control is IPostBackDataHandler) && !(control._adapter is IPostBackDataHandler))
			{
				throw new HttpException(SR.GetString("Ctrl_not_data_handler"));
			}
			if (this._registeredControlsThatRequirePostBack == null)
			{
				this._registeredControlsThatRequirePostBack = new ArrayList();
			}
			this._registeredControlsThatRequirePostBack.Add(control.UniqueID);
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x000D8A1D File Offset: 0x000D7A1D
		internal void PushCachingControl(BasePartialCachingControl c)
		{
			if (this._partialCachingControlStack == null)
			{
				this._partialCachingControlStack = new Stack();
			}
			this._partialCachingControlStack.Push(c);
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x000D8A3E File Offset: 0x000D7A3E
		internal void PopCachingControl()
		{
			this._partialCachingControlStack.Pop();
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x000D8A4C File Offset: 0x000D7A4C
		private void ProcessPostData(NameValueCollection postData, bool fBeforeLoad)
		{
			if (this._changedPostDataConsumers == null)
			{
				this._changedPostDataConsumers = new ArrayList();
			}
			if (postData != null)
			{
				foreach (object obj in postData)
				{
					string text = (string)obj;
					if (text != null && !Page.IsSystemPostField(text))
					{
						Control control = this.FindControl(text);
						if (control == null)
						{
							if (fBeforeLoad)
							{
								if (this._leftoverPostData == null)
								{
									this._leftoverPostData = new NameValueCollection();
								}
								this._leftoverPostData.Add(text, null);
							}
						}
						else
						{
							IPostBackDataHandler postBackDataHandler = control.PostBackDataHandler;
							if (postBackDataHandler == null)
							{
								if (control.PostBackEventHandler != null)
								{
									this.RegisterRequiresRaiseEvent(control.PostBackEventHandler);
								}
							}
							else
							{
								if (postBackDataHandler != null)
								{
									bool flag = postBackDataHandler.LoadPostData(text, this._requestValueCollection);
									if (flag)
									{
										this._changedPostDataConsumers.Add(control);
									}
								}
								if (this._controlsRequiringPostBack != null)
								{
									this._controlsRequiringPostBack.Remove(text);
								}
							}
						}
					}
				}
			}
			ArrayList arrayList = null;
			if (this._controlsRequiringPostBack != null)
			{
				foreach (object obj2 in this._controlsRequiringPostBack)
				{
					string text2 = (string)obj2;
					Control control2 = this.FindControl(text2);
					if (control2 != null)
					{
						IPostBackDataHandler postBackDataHandler2 = control2._adapter as IPostBackDataHandler;
						if (postBackDataHandler2 == null)
						{
							postBackDataHandler2 = control2 as IPostBackDataHandler;
						}
						if (postBackDataHandler2 == null)
						{
							throw new HttpException(SR.GetString("Postback_ctrl_not_found", new object[] { text2 }));
						}
						bool flag2 = postBackDataHandler2.LoadPostData(text2, this._requestValueCollection);
						if (flag2)
						{
							this._changedPostDataConsumers.Add(control2);
						}
					}
					else if (fBeforeLoad)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(text2);
					}
				}
				this._controlsRequiringPostBack = arrayList;
			}
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x000D8C44 File Offset: 0x000D7C44
		internal void RaiseChangedEvents()
		{
			if (this._changedPostDataConsumers != null)
			{
				for (int i = 0; i < this._changedPostDataConsumers.Count; i++)
				{
					Control control = (Control)this._changedPostDataConsumers[i];
					if (control != null)
					{
						IPostBackDataHandler postBackDataHandler = control.PostBackDataHandler;
						if ((control == null || control.IsDescendentOf(this)) && control != null && control.PostBackDataHandler != null)
						{
							postBackDataHandler.RaisePostDataChangedEvent();
						}
					}
				}
			}
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x000D8CA8 File Offset: 0x000D7CA8
		private void RaisePostBackEvent(NameValueCollection postData)
		{
			if (this._registeredControlThatRequireRaiseEvent != null)
			{
				this.RaisePostBackEvent(this._registeredControlThatRequireRaiseEvent, null);
				return;
			}
			string text = postData["__EVENTTARGET"];
			bool flag = !string.IsNullOrEmpty(text);
			if (flag || this.AutoPostBackControl != null)
			{
				Control control = null;
				if (flag)
				{
					control = this.FindControl(text);
				}
				if (control != null && control.PostBackEventHandler != null)
				{
					string text2 = postData["__EVENTARGUMENT"];
					this.RaisePostBackEvent(control.PostBackEventHandler, text2);
					return;
				}
			}
			else
			{
				this.Validate();
			}
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x000D8D24 File Offset: 0x000D7D24
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
		{
			sourceControl.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x000D8D2D File Offset: 0x000D7D2D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void RegisterRequiresRaiseEvent(IPostBackEventHandler control)
		{
			this._registeredControlThatRequireRaiseEvent = control;
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x0600314C RID: 12620 RVA: 0x000D8D36 File Offset: 0x000D7D36
		public bool IsPostBackEventControlRegistered
		{
			get
			{
				return this._registeredControlThatRequireRaiseEvent != null;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x0600314D RID: 12621 RVA: 0x000D8D44 File Offset: 0x000D7D44
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsValid
		{
			get
			{
				if (!this._validated)
				{
					throw new HttpException(SR.GetString("IsValid_Cant_Be_Called"));
				}
				if (this._validators != null)
				{
					ValidatorCollection validators = this.Validators;
					int count = validators.Count;
					for (int i = 0; i < count; i++)
					{
						if (!validators[i].IsValid)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x0600314E RID: 12622 RVA: 0x000D8D9C File Offset: 0x000D7D9C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ValidatorCollection Validators
		{
			get
			{
				if (this._validators == null)
				{
					this._validators = new ValidatorCollection();
				}
				return this._validators;
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x0600314F RID: 12623 RVA: 0x000D8DB8 File Offset: 0x000D7DB8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Page PreviousPage
		{
			get
			{
				if (this._previousPage == null && this._previousPagePath != null)
				{
					if (!Util.IsUserAllowedToPath(this.Context, this._previousPagePath))
					{
						throw new InvalidOperationException(SR.GetString("Previous_Page_Not_Authorized"));
					}
					ITypedWebObjectFactory typedWebObjectFactory = (ITypedWebObjectFactory)BuildManager.GetVPathBuildResult(this.Context, this._previousPagePath);
					if (typeof(Page).IsAssignableFrom(typedWebObjectFactory.InstantiatedType))
					{
						this._previousPage = (Page)typedWebObjectFactory.CreateInstance();
						this._previousPage._isCrossPagePostBack = true;
						this.Server.Execute(this._previousPage, TextWriter.Null, true, false);
					}
				}
				return this._previousPage;
			}
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000D8E6D File Offset: 0x000D7E6D
		public string MapPath(string virtualPath)
		{
			return this._request.MapPath(VirtualPath.CreateAllowNull(virtualPath), base.TemplateControlVirtualDirectory, true);
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x000D8E87 File Offset: 0x000D7E87
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void InitOutputCache(int duration, string varyByHeader, string varyByCustom, OutputCacheLocation location, string varyByParam)
		{
			this.InitOutputCache(duration, null, varyByHeader, varyByCustom, location, varyByParam);
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x000D8E98 File Offset: 0x000D7E98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void InitOutputCache(int duration, string varyByContentEncoding, string varyByHeader, string varyByCustom, OutputCacheLocation location, string varyByParam)
		{
			if (this._isCrossPagePostBack)
			{
				return;
			}
			this.InitOutputCache(new OutputCacheParameters
			{
				Duration = duration,
				VaryByContentEncoding = varyByContentEncoding,
				VaryByHeader = varyByHeader,
				VaryByCustom = varyByCustom,
				Location = location,
				VaryByParam = varyByParam
			});
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x000D8EE8 File Offset: 0x000D7EE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected internal virtual void InitOutputCache(OutputCacheParameters cacheSettings)
		{
			if (this._isCrossPagePostBack)
			{
				return;
			}
			OutputCacheProfile outputCacheProfile = null;
			HttpCachePolicy cache = this.Response.Cache;
			OutputCacheLocation outputCacheLocation = (OutputCacheLocation)(-1);
			int num = 0;
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = null;
			string text6 = null;
			bool flag = false;
			RuntimeConfig appConfig = RuntimeConfig.GetAppConfig();
			OutputCacheSection outputCache = appConfig.OutputCache;
			if (!outputCache.EnableOutputCache)
			{
				return;
			}
			if (cacheSettings.CacheProfile != null && cacheSettings.CacheProfile.Length != 0)
			{
				OutputCacheSettingsSection outputCacheSettings = appConfig.OutputCacheSettings;
				outputCacheProfile = outputCacheSettings.OutputCacheProfiles[cacheSettings.CacheProfile];
				if (outputCacheProfile == null)
				{
					throw new HttpException(SR.GetString("CacheProfile_Not_Found", new object[] { cacheSettings.CacheProfile }));
				}
				if (!outputCacheProfile.Enabled)
				{
					return;
				}
			}
			if (outputCacheProfile != null)
			{
				num = outputCacheProfile.Duration;
				text = outputCacheProfile.VaryByContentEncoding;
				text2 = outputCacheProfile.VaryByHeader;
				text3 = outputCacheProfile.VaryByCustom;
				text4 = outputCacheProfile.VaryByParam;
				text5 = outputCacheProfile.SqlDependency;
				flag = outputCacheProfile.NoStore;
				text6 = outputCacheProfile.VaryByControl;
				outputCacheLocation = outputCacheProfile.Location;
				if (string.IsNullOrEmpty(text))
				{
					text = null;
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 = null;
				}
				if (string.IsNullOrEmpty(text3))
				{
					text3 = null;
				}
				if (string.IsNullOrEmpty(text4))
				{
					text4 = null;
				}
				if (string.IsNullOrEmpty(text6))
				{
					text6 = null;
				}
				if (StringUtil.EqualsIgnoreCase(text4, "none"))
				{
					text4 = null;
				}
				if (StringUtil.EqualsIgnoreCase(text6, "none"))
				{
					text6 = null;
				}
			}
			if (cacheSettings.IsParameterSet(OutputCacheParameter.Duration))
			{
				num = cacheSettings.Duration;
			}
			if (cacheSettings.IsParameterSet(OutputCacheParameter.VaryByContentEncoding))
			{
				text = cacheSettings.VaryByContentEncoding;
			}
			if (cacheSettings.IsParameterSet(OutputCacheParameter.VaryByHeader))
			{
				text2 = cacheSettings.VaryByHeader;
			}
			if (cacheSettings.IsParameterSet(OutputCacheParameter.VaryByCustom))
			{
				text3 = cacheSettings.VaryByCustom;
			}
			if (cacheSettings.IsParameterSet(OutputCacheParameter.VaryByControl))
			{
				text6 = cacheSettings.VaryByControl;
			}
			if (cacheSettings.IsParameterSet(OutputCacheParameter.VaryByParam))
			{
				text4 = cacheSettings.VaryByParam;
			}
			if (cacheSettings.IsParameterSet(OutputCacheParameter.SqlDependency))
			{
				text5 = cacheSettings.SqlDependency;
			}
			if (cacheSettings.IsParameterSet(OutputCacheParameter.NoStore))
			{
				flag = cacheSettings.NoStore;
			}
			if (cacheSettings.IsParameterSet(OutputCacheParameter.Location))
			{
				outputCacheLocation = cacheSettings.Location;
			}
			if (outputCacheLocation == (OutputCacheLocation)(-1))
			{
				outputCacheLocation = OutputCacheLocation.Any;
			}
			if (outputCacheLocation != OutputCacheLocation.None && (outputCacheProfile == null || outputCacheProfile.Enabled))
			{
				if ((outputCacheProfile == null || outputCacheProfile.Duration == -1) && !cacheSettings.IsParameterSet(OutputCacheParameter.Duration))
				{
					throw new HttpException(SR.GetString("Missing_output_cache_attr", new object[] { "duration" }));
				}
				if ((outputCacheProfile == null || (outputCacheProfile.VaryByParam == null && outputCacheProfile.VaryByControl == null)) && !cacheSettings.IsParameterSet(OutputCacheParameter.VaryByParam) && !cacheSettings.IsParameterSet(OutputCacheParameter.VaryByControl))
				{
					throw new HttpException(SR.GetString("Missing_output_cache_attr", new object[] { "varyByParam" }));
				}
			}
			if (flag)
			{
				this.Response.Cache.SetNoStore();
			}
			HttpCacheability httpCacheability;
			switch (outputCacheLocation)
			{
			case OutputCacheLocation.Any:
				httpCacheability = HttpCacheability.Public;
				break;
			case OutputCacheLocation.Client:
				httpCacheability = HttpCacheability.Private;
				break;
			case OutputCacheLocation.Downstream:
				httpCacheability = HttpCacheability.Public;
				cache.SetNoServerCaching();
				break;
			case OutputCacheLocation.Server:
				httpCacheability = HttpCacheability.Server;
				break;
			case OutputCacheLocation.None:
				httpCacheability = HttpCacheability.NoCache;
				break;
			case OutputCacheLocation.ServerAndClient:
				httpCacheability = HttpCacheability.ServerAndPrivate;
				break;
			default:
				throw new ArgumentOutOfRangeException("cacheSettings", SR.GetString("Invalid_cache_settings_location"));
			}
			cache.SetCacheability(httpCacheability);
			if (outputCacheLocation != OutputCacheLocation.None)
			{
				cache.SetExpires(this.Context.Timestamp.AddSeconds((double)num));
				cache.SetMaxAge(new TimeSpan(0, 0, num));
				cache.SetValidUntilExpires(true);
				cache.SetLastModified(this.Context.Timestamp);
				if (outputCacheLocation != OutputCacheLocation.Client)
				{
					if (text != null)
					{
						string[] array = text.Split(Page.s_varySeparator);
						foreach (string text7 in array)
						{
							cache.VaryByContentEncodings[text7.Trim()] = true;
						}
					}
					if (text2 != null)
					{
						string[] array3 = text2.Split(Page.s_varySeparator);
						foreach (string text8 in array3)
						{
							cache.VaryByHeaders[text8.Trim()] = true;
						}
					}
					if (this.PageAdapter != null)
					{
						StringCollection cacheVaryByHeaders = this.PageAdapter.CacheVaryByHeaders;
						if (cacheVaryByHeaders != null)
						{
							foreach (string text9 in cacheVaryByHeaders)
							{
								cache.VaryByHeaders[text9] = true;
							}
						}
					}
					if (outputCacheLocation != OutputCacheLocation.Downstream)
					{
						if (text3 != null)
						{
							cache.SetVaryByCustom(text3);
						}
						if (string.IsNullOrEmpty(text4) && string.IsNullOrEmpty(text6) && (this.PageAdapter == null || this.PageAdapter.CacheVaryByParams == null))
						{
							cache.VaryByParams.IgnoreParams = true;
						}
						else
						{
							if (!string.IsNullOrEmpty(text4))
							{
								string[] array5 = text4.Split(Page.s_varySeparator);
								foreach (string text10 in array5)
								{
									cache.VaryByParams[text10.Trim()] = true;
								}
							}
							if (!string.IsNullOrEmpty(text6))
							{
								string[] array7 = text6.Split(Page.s_varySeparator);
								foreach (string text11 in array7)
								{
									cache.VaryByParams[text11.Trim()] = true;
								}
							}
							if (this.PageAdapter != null)
							{
								IList cacheVaryByParams = this.PageAdapter.CacheVaryByParams;
								if (cacheVaryByParams != null)
								{
									foreach (object obj in cacheVaryByParams)
									{
										string text12 = (string)obj;
										cache.VaryByParams[text12] = true;
									}
								}
							}
						}
						if (!string.IsNullOrEmpty(text5))
						{
							this.Response.AddCacheDependency(new CacheDependency[] { SqlCacheDependency.CreateOutputCacheDependency(text5) });
						}
					}
				}
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (set) Token: 0x06003154 RID: 12628 RVA: 0x000D94C4 File Offset: 0x000D84C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("The recommended alternative is HttpResponse.AddFileDependencies. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected ArrayList FileDependencies
		{
			set
			{
				this.Response.AddFileDependencies(value);
			}
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x000D94D2 File Offset: 0x000D84D2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected object GetWrappedFileDependencies(string[] virtualFileDependencies)
		{
			return virtualFileDependencies;
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x000D94D5 File Offset: 0x000D84D5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected internal void AddWrappedFileDependencies(object virtualFileDependencies)
		{
			this.Response.AddVirtualPathDependencies((string[])virtualFileDependencies);
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06003158 RID: 12632 RVA: 0x000D94F6 File Offset: 0x000D84F6
		// (set) Token: 0x06003157 RID: 12631 RVA: 0x000D94E8 File Offset: 0x000D84E8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Buffer
		{
			get
			{
				return this.Response.BufferOutput;
			}
			set
			{
				this.Response.BufferOutput = value;
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x0600315A RID: 12634 RVA: 0x000D9511 File Offset: 0x000D8511
		// (set) Token: 0x06003159 RID: 12633 RVA: 0x000D9503 File Offset: 0x000D8503
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ContentType
		{
			get
			{
				return this.Response.ContentType;
			}
			set
			{
				this.Response.ContentType = value;
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x0600315C RID: 12636 RVA: 0x000D9531 File Offset: 0x000D8531
		// (set) Token: 0x0600315B RID: 12635 RVA: 0x000D951E File Offset: 0x000D851E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public int CodePage
		{
			get
			{
				return this.Response.ContentEncoding.CodePage;
			}
			set
			{
				this.Response.ContentEncoding = Encoding.GetEncoding(value);
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x0600315E RID: 12638 RVA: 0x000D9556 File Offset: 0x000D8556
		// (set) Token: 0x0600315D RID: 12637 RVA: 0x000D9543 File Offset: 0x000D8543
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public string ResponseEncoding
		{
			get
			{
				return this.Response.ContentEncoding.EncodingName;
			}
			set
			{
				this.Response.ContentEncoding = Encoding.GetEncoding(value);
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06003160 RID: 12640 RVA: 0x000D95F0 File Offset: 0x000D85F0
		// (set) Token: 0x0600315F RID: 12639 RVA: 0x000D9568 File Offset: 0x000D8568
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Culture
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture.DisplayName;
			}
			set
			{
				CultureInfo cultureInfo = null;
				if (StringUtil.EqualsIgnoreCase(value, HttpApplication.AutoCulture))
				{
					CultureInfo cultureInfo2 = this.CultureFromUserLanguages(true);
					if (cultureInfo2 != null)
					{
						cultureInfo = cultureInfo2;
					}
				}
				else
				{
					if (StringUtil.StringStartsWithIgnoreCase(value, HttpApplication.AutoCulture))
					{
						CultureInfo cultureInfo3 = this.CultureFromUserLanguages(true);
						if (cultureInfo3 != null)
						{
							cultureInfo = cultureInfo3;
							goto IL_0053;
						}
						try
						{
							cultureInfo = HttpServerUtility.CreateReadOnlyCultureInfo(value.Substring(5));
							goto IL_0053;
						}
						catch
						{
							goto IL_0053;
						}
					}
					cultureInfo = HttpServerUtility.CreateReadOnlyCultureInfo(value);
				}
				IL_0053:
				if (cultureInfo != null)
				{
					Thread.CurrentThread.CurrentCulture = cultureInfo;
					this._dynamicCulture = cultureInfo;
				}
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06003161 RID: 12641 RVA: 0x000D9601 File Offset: 0x000D8601
		internal CultureInfo DynamicCulture
		{
			get
			{
				return this._dynamicCulture;
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06003163 RID: 12643 RVA: 0x000D9632 File Offset: 0x000D8632
		// (set) Token: 0x06003162 RID: 12642 RVA: 0x000D960C File Offset: 0x000D860C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int LCID
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture.LCID;
			}
			set
			{
				CultureInfo cultureInfo = HttpServerUtility.CreateReadOnlyCultureInfo(value);
				Thread.CurrentThread.CurrentCulture = cultureInfo;
				this._dynamicCulture = cultureInfo;
			}
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x000D9644 File Offset: 0x000D8644
		private CultureInfo CultureFromUserLanguages(bool specific)
		{
			if (this._context != null && this._context.Request != null && this._context.Request.UserLanguages != null && this._context.Request.UserLanguages[0] != null)
			{
				try
				{
					string text = this._context.Request.UserLanguages[0];
					int num = text.IndexOf(';');
					if (num != -1)
					{
						text = text.Substring(0, num);
					}
					if (specific)
					{
						return HttpServerUtility.CreateReadOnlySpecificCultureInfo(text);
					}
					return HttpServerUtility.CreateReadOnlyCultureInfo(text);
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06003166 RID: 12646 RVA: 0x000D9768 File Offset: 0x000D8768
		// (set) Token: 0x06003165 RID: 12645 RVA: 0x000D96E0 File Offset: 0x000D86E0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public string UICulture
		{
			get
			{
				return Thread.CurrentThread.CurrentUICulture.DisplayName;
			}
			set
			{
				CultureInfo cultureInfo = null;
				if (StringUtil.EqualsIgnoreCase(value, HttpApplication.AutoCulture))
				{
					CultureInfo cultureInfo2 = this.CultureFromUserLanguages(false);
					if (cultureInfo2 != null)
					{
						cultureInfo = cultureInfo2;
					}
				}
				else
				{
					if (StringUtil.StringStartsWithIgnoreCase(value, HttpApplication.AutoCulture))
					{
						CultureInfo cultureInfo3 = this.CultureFromUserLanguages(false);
						if (cultureInfo3 != null)
						{
							cultureInfo = cultureInfo3;
							goto IL_0053;
						}
						try
						{
							cultureInfo = HttpServerUtility.CreateReadOnlyCultureInfo(value.Substring(5));
							goto IL_0053;
						}
						catch
						{
							goto IL_0053;
						}
					}
					cultureInfo = HttpServerUtility.CreateReadOnlyCultureInfo(value);
				}
				IL_0053:
				if (cultureInfo != null)
				{
					Thread.CurrentThread.CurrentUICulture = cultureInfo;
					this._dynamicUICulture = cultureInfo;
				}
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06003167 RID: 12647 RVA: 0x000D9779 File Offset: 0x000D8779
		internal CultureInfo DynamicUICulture
		{
			get
			{
				return this._dynamicUICulture;
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06003169 RID: 12649 RVA: 0x000D97B4 File Offset: 0x000D87B4
		// (set) Token: 0x06003168 RID: 12648 RVA: 0x000D9781 File Offset: 0x000D8781
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public TimeSpan AsyncTimeout
		{
			get
			{
				if (!this._asyncTimeoutSet)
				{
					if (this.Context != null)
					{
						PagesSection pages = RuntimeConfig.GetConfig(this.Context).Pages;
						if (pages != null)
						{
							this.AsyncTimeout = pages.AsyncTimeout;
						}
					}
					if (!this._asyncTimeoutSet)
					{
						this.AsyncTimeout = TimeSpan.FromSeconds((double)Page.DefaultAsyncTimeoutSeconds);
					}
				}
				return this._asyncTimeout;
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentException(SR.GetString("Page_Illegal_AsyncTimeout"), "AsyncTimeout");
				}
				this._asyncTimeout = value;
				this._asyncTimeoutSet = true;
			}
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x0600316B RID: 12651 RVA: 0x000D9819 File Offset: 0x000D8819
		// (set) Token: 0x0600316A RID: 12650 RVA: 0x000D9810 File Offset: 0x000D8810
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected int TransactionMode
		{
			get
			{
				return this._transactionMode;
			}
			set
			{
				this._transactionMode = value;
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x0600316D RID: 12653 RVA: 0x000D982A File Offset: 0x000D882A
		// (set) Token: 0x0600316C RID: 12652 RVA: 0x000D9821 File Offset: 0x000D8821
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected bool AspCompatMode
		{
			get
			{
				return this._aspCompatMode;
			}
			set
			{
				this._aspCompatMode = value;
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x0600316F RID: 12655 RVA: 0x000D983B File Offset: 0x000D883B
		// (set) Token: 0x0600316E RID: 12654 RVA: 0x000D9832 File Offset: 0x000D8832
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected bool AsyncMode
		{
			get
			{
				return this._asyncMode;
			}
			set
			{
				this._asyncMode = value;
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x000D9851 File Offset: 0x000D8851
		// (set) Token: 0x06003170 RID: 12656 RVA: 0x000D9843 File Offset: 0x000D8843
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool TraceEnabled
		{
			get
			{
				return this.Trace.IsEnabled;
			}
			set
			{
				this.Trace.IsEnabled = value;
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x000D986C File Offset: 0x000D886C
		// (set) Token: 0x06003172 RID: 12658 RVA: 0x000D985E File Offset: 0x000D885E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TraceMode TraceModeValue
		{
			get
			{
				return this.Trace.TraceMode;
			}
			set
			{
				this.Trace.TraceMode = value;
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06003174 RID: 12660 RVA: 0x000D9879 File Offset: 0x000D8879
		// (set) Token: 0x06003175 RID: 12661 RVA: 0x000D9881 File Offset: 0x000D8881
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool EnableViewStateMac
		{
			get
			{
				return this._enableViewStateMac;
			}
			set
			{
				if (!EnableViewStateMacRegistryHelper.EnforceViewStateMac)
				{
					this._enableViewStateMac = value;
				}
			}
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06003176 RID: 12662 RVA: 0x000D9894 File Offset: 0x000D8894
		// (set) Token: 0x06003177 RID: 12663 RVA: 0x000D990B File Offset: 0x000D890B
		[Browsable(false)]
		[Filterable(false)]
		[Obsolete("The recommended alternative is Page.SetFocus and Page.MaintainScrollPositionOnPostBack. http://go.microsoft.com/fwlink/?linkid=14202")]
		public bool SmartNavigation
		{
			get
			{
				if (this._smartNavSupport == SmartNavigationSupport.NotDesiredOrSupported)
				{
					return false;
				}
				if (this._smartNavSupport == SmartNavigationSupport.Desired)
				{
					HttpContext httpContext = HttpContext.Current;
					if (httpContext == null)
					{
						return false;
					}
					HttpBrowserCapabilities browser = httpContext.Request.Browser;
					if (!string.Equals(browser.Browser, "ie", StringComparison.OrdinalIgnoreCase) || browser.MajorVersion < 6 || !browser.Win32)
					{
						this._smartNavSupport = SmartNavigationSupport.NotDesiredOrSupported;
					}
					else
					{
						this._smartNavSupport = SmartNavigationSupport.IE6OrNewer;
					}
				}
				return this._smartNavSupport != SmartNavigationSupport.NotDesiredOrSupported;
			}
			set
			{
				if (value)
				{
					this._smartNavSupport = SmartNavigationSupport.Desired;
					return;
				}
				this._smartNavSupport = SmartNavigationSupport.NotDesiredOrSupported;
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06003178 RID: 12664 RVA: 0x000D991F File Offset: 0x000D891F
		internal bool IsTransacted
		{
			get
			{
				return this._transactionMode != 0;
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x000D992D File Offset: 0x000D892D
		internal bool IsInAspCompatMode
		{
			get
			{
				return this._aspCompatMode;
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x0600317A RID: 12666 RVA: 0x000D9935 File Offset: 0x000D8935
		public bool IsAsync
		{
			get
			{
				return this._asyncMode;
			}
		}

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x0600317B RID: 12667 RVA: 0x000D993D File Offset: 0x000D893D
		// (remove) Token: 0x0600317C RID: 12668 RVA: 0x000D9950 File Offset: 0x000D8950
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler LoadComplete
		{
			add
			{
				base.Events.AddHandler(Page.EventLoadComplete, value);
			}
			remove
			{
				base.Events.RemoveHandler(Page.EventLoadComplete, value);
			}
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x000D9964 File Offset: 0x000D8964
		protected virtual void OnLoadComplete(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Page.EventLoadComplete];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x000D9994 File Offset: 0x000D8994
		protected virtual void OnPreRenderComplete(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Page.EventPreRenderComplete];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x000D99C2 File Offset: 0x000D89C2
		private void PerformPreRenderComplete()
		{
			this.OnPreRenderComplete(EventArgs.Empty);
		}

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06003180 RID: 12672 RVA: 0x000D99CF File Offset: 0x000D89CF
		// (remove) Token: 0x06003181 RID: 12673 RVA: 0x000D99E2 File Offset: 0x000D89E2
		public event EventHandler PreInit
		{
			add
			{
				base.Events.AddHandler(Page.EventPreInit, value);
			}
			remove
			{
				base.Events.RemoveHandler(Page.EventPreInit, value);
			}
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06003182 RID: 12674 RVA: 0x000D99F5 File Offset: 0x000D89F5
		// (remove) Token: 0x06003183 RID: 12675 RVA: 0x000D9A08 File Offset: 0x000D8A08
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler PreLoad
		{
			add
			{
				base.Events.AddHandler(Page.EventPreLoad, value);
			}
			remove
			{
				base.Events.RemoveHandler(Page.EventPreLoad, value);
			}
		}

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06003184 RID: 12676 RVA: 0x000D9A1B File Offset: 0x000D8A1B
		// (remove) Token: 0x06003185 RID: 12677 RVA: 0x000D9A2E File Offset: 0x000D8A2E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler PreRenderComplete
		{
			add
			{
				base.Events.AddHandler(Page.EventPreRenderComplete, value);
			}
			remove
			{
				base.Events.RemoveHandler(Page.EventPreRenderComplete, value);
			}
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x000D9A41 File Offset: 0x000D8A41
		protected override void FrameworkInitialize()
		{
			base.FrameworkInitialize();
			this.InitializeStyleSheet();
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x000D9A4F File Offset: 0x000D8A4F
		protected virtual void InitializeCulture()
		{
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x000D9A51 File Offset: 0x000D8A51
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (this._theme != null)
			{
				this._theme.SetStyleSheet();
			}
			if (this._styleSheet != null)
			{
				this._styleSheet.SetStyleSheet();
			}
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x000D9A80 File Offset: 0x000D8A80
		protected virtual void OnPreInit(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Page.EventPreInit];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x000D9AAE File Offset: 0x000D8AAE
		private void PerformPreInit()
		{
			this.OnPreInit(EventArgs.Empty);
			this.InitializeThemes();
			this.ApplyMasterPage();
			this._preInitWorkComplete = true;
		}

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x0600318B RID: 12683 RVA: 0x000D9ACE File Offset: 0x000D8ACE
		// (remove) Token: 0x0600318C RID: 12684 RVA: 0x000D9AE1 File Offset: 0x000D8AE1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler InitComplete
		{
			add
			{
				base.Events.AddHandler(Page.EventInitComplete, value);
			}
			remove
			{
				base.Events.RemoveHandler(Page.EventInitComplete, value);
			}
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x000D9AF4 File Offset: 0x000D8AF4
		protected virtual void OnInitComplete(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Page.EventInitComplete];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x000D9B24 File Offset: 0x000D8B24
		protected virtual void OnPreLoad(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Page.EventPreLoad];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x000D9B52 File Offset: 0x000D8B52
		public void RegisterRequiresViewStateEncryption()
		{
			if (base.ControlState >= ControlState.PreRendered)
			{
				throw new InvalidOperationException(SR.GetString("Too_late_for_RegisterRequiresViewStateEncryption"));
			}
			this._viewStateEncryptionRequested = true;
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06003190 RID: 12688 RVA: 0x000D9B74 File Offset: 0x000D8B74
		internal bool RequiresViewStateEncryptionInternal
		{
			get
			{
				return this.ViewStateEncryptionMode == ViewStateEncryptionMode.Always || (this._viewStateEncryptionRequested && this.ViewStateEncryptionMode == ViewStateEncryptionMode.Auto);
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06003191 RID: 12689 RVA: 0x000D9B94 File Offset: 0x000D8B94
		// (remove) Token: 0x06003192 RID: 12690 RVA: 0x000D9BA7 File Offset: 0x000D8BA7
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler SaveStateComplete
		{
			add
			{
				base.Events.AddHandler(Page.EventSaveStateComplete, value);
			}
			remove
			{
				base.Events.RemoveHandler(Page.EventSaveStateComplete, value);
			}
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x000D9BBC File Offset: 0x000D8BBC
		protected virtual void OnSaveStateComplete(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Page.EventSaveStateComplete];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x000D9BEA File Offset: 0x000D8BEA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void ProcessRequest(HttpContext context)
		{
			if (HttpRuntime.NamedPermissionSet != null)
			{
				if (HttpRuntime.ProcessRequestInApplicationTrust)
				{
					if (base.NoCompile)
					{
						HttpRuntime.NamedPermissionSet.PermitOnly();
					}
				}
				else if (!base.NoCompile)
				{
					this.ProcessRequestWithAssert(context);
					return;
				}
			}
			this.ProcessRequestWithNoAssert(context);
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x000D9C25 File Offset: 0x000D8C25
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private void ProcessRequestWithAssert(HttpContext context)
		{
			this.ProcessRequestWithNoAssert(context);
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x000D9C2E File Offset: 0x000D8C2E
		private void ProcessRequestWithNoAssert(HttpContext context)
		{
			this.SetIntrinsics(context);
			this.ProcessRequest();
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x000D9C3D File Offset: 0x000D8C3D
		[SecurityPermission(SecurityAction.Assert, ControlThread = true)]
		private void SetCultureWithAssert(Thread currentThread, CultureInfo currentCulture, CultureInfo currentUICulture)
		{
			this.SetCulture(currentThread, currentCulture, currentUICulture);
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x000D9C48 File Offset: 0x000D8C48
		private void SetCulture(Thread currentThread, CultureInfo currentCulture, CultureInfo currentUICulture)
		{
			currentThread.CurrentCulture = currentCulture;
			currentThread.CurrentUICulture = currentUICulture;
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x000D9C58 File Offset: 0x000D8C58
		private void ProcessRequest()
		{
			Thread currentThread = Thread.CurrentThread;
			CultureInfo currentCulture = currentThread.CurrentCulture;
			CultureInfo currentUICulture = currentThread.CurrentUICulture;
			try
			{
				this.ProcessRequest(true, true);
			}
			finally
			{
				this.RestoreCultures(currentThread, currentCulture, currentUICulture);
			}
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x000D9CA0 File Offset: 0x000D8CA0
		private void ProcessRequest(bool includeStagesBeforeAsyncPoint, bool includeStagesAfterAsyncPoint)
		{
			if (includeStagesBeforeAsyncPoint)
			{
				this.FrameworkInitialize();
				base.ControlState = ControlState.FrameworkInitialized;
			}
			bool flag = this.Context.WorkerRequest is IIS7WorkerRequest;
			try
			{
				try
				{
					if (this.IsTransacted)
					{
						this.ProcessRequestTransacted();
					}
					else
					{
						this.ProcessRequestMain(includeStagesBeforeAsyncPoint, includeStagesAfterAsyncPoint);
					}
					if (includeStagesAfterAsyncPoint)
					{
						flag = false;
						this.ProcessRequestEndTrace();
					}
				}
				catch (ThreadAbortException)
				{
					try
					{
						if (flag)
						{
							this.ProcessRequestEndTrace();
						}
					}
					catch
					{
					}
				}
				finally
				{
					if (includeStagesAfterAsyncPoint)
					{
						this.ProcessRequestCleanup();
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x000D9D4C File Offset: 0x000D8D4C
		private void RestoreCultures(Thread currentThread, CultureInfo prevCulture, CultureInfo prevUICulture)
		{
			if (prevCulture != currentThread.CurrentCulture || prevUICulture != currentThread.CurrentUICulture)
			{
				if (HttpRuntime.IsFullTrust)
				{
					this.SetCulture(currentThread, prevCulture, prevUICulture);
					return;
				}
				this.SetCultureWithAssert(currentThread, prevCulture, prevUICulture);
			}
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x000D9D7C File Offset: 0x000D8D7C
		private void ProcessRequestTransacted()
		{
			bool flag = false;
			TransactedCallback transactedCallback = new TransactedCallback(this.ProcessRequestMain);
			Transactions.InvokeTransacted(transactedCallback, (TransactionOption)this._transactionMode, ref flag);
			try
			{
				if (flag)
				{
					this.OnAbortTransaction(EventArgs.Empty);
					WebBaseEvent.RaiseSystemEvent(this, 2002);
				}
				else
				{
					this.OnCommitTransaction(EventArgs.Empty);
					WebBaseEvent.RaiseSystemEvent(this, 2001);
				}
				this._request.ValidateRawUrl();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex)
			{
				PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_DURING_REQUEST);
				PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_TOTAL);
				if (!this.HandleError(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x000D9E20 File Offset: 0x000D8E20
		private void ProcessRequestCleanup()
		{
			this._request = null;
			this._response = null;
			if (!this.IsCrossPagePostBack)
			{
				this.UnloadRecursive(true);
			}
			if (this.Context.TraceIsEnabled)
			{
				this.Trace.StopTracing();
			}
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x000D9E58 File Offset: 0x000D8E58
		private void ProcessRequestEndTrace()
		{
			if (this.Context.TraceIsEnabled)
			{
				this.Trace.EndRequest();
				if (this.Trace.PageOutput && !this.IsCallback && (this.ScriptManager == null || !this.ScriptManager.IsInAsyncPostBack))
				{
					this.Trace.Render(this.CreateHtmlTextWriter(this.Response.Output));
					this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
				}
			}
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x000D9ED4 File Offset: 0x000D8ED4
		internal void SetPreviousPage(Page previousPage)
		{
			this._previousPage = previousPage;
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x000D9EDD File Offset: 0x000D8EDD
		private void ProcessRequestMain()
		{
			this.ProcessRequestMain(true, true);
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x000D9EE8 File Offset: 0x000D8EE8
		private void ProcessRequestMain(bool includeStagesBeforeAsyncPoint, bool includeStagesAfterAsyncPoint)
		{
			try
			{
				HttpContext context = this.Context;
				string text = null;
				if (includeStagesBeforeAsyncPoint)
				{
					if (this.IsInAspCompatMode)
					{
						AspCompatApplicationStep.OnPageStartSessionObjects();
					}
					if (this.PageAdapter != null)
					{
						this._requestValueCollection = this.PageAdapter.DeterminePostBackMode();
					}
					else
					{
						this._requestValueCollection = this.DeterminePostBackMode();
					}
					string text2 = string.Empty;
					if (this.DetermineIsExportingWebPart())
					{
						if (!RuntimeConfig.GetAppConfig().WebParts.EnableExport)
						{
							throw new InvalidOperationException(SR.GetString("WebPartExportHandler_DisabledExportHandler"));
						}
						text = this.Request.QueryString["webPart"];
						if (string.IsNullOrEmpty(text))
						{
							throw new InvalidOperationException(SR.GetString("WebPartExportHandler_InvalidArgument"));
						}
						if (string.Equals(this.Request.QueryString["scope"], "shared", StringComparison.OrdinalIgnoreCase))
						{
							this._pageFlags.Set(4);
						}
						string text3 = this.Request.QueryString["query"];
						if (text3 == null)
						{
							text3 = string.Empty;
						}
						this.Request.QueryStringText = text3;
						context.Trace.IsEnabled = false;
					}
					if (this._requestValueCollection != null)
					{
						if (this._requestValueCollection["__VIEWSTATEENCRYPTED"] != null)
						{
							this.ContainsEncryptedViewState = true;
						}
						text2 = this._requestValueCollection["__CALLBACKID"];
						if (text2 != null && this._request.HttpVerb == HttpVerb.POST)
						{
							this._isCallback = true;
						}
						else if (!this.IsCrossPagePostBack)
						{
							VirtualPath virtualPath = null;
							if (this._requestValueCollection["__PREVIOUSPAGE"] != null)
							{
								try
								{
									virtualPath = VirtualPath.CreateNonRelativeAllowNull(Page.DecryptString(this._requestValueCollection["__PREVIOUSPAGE"]));
								}
								catch
								{
									this._pageFlags[8] = true;
								}
								if (virtualPath != null && virtualPath != this.Request.CurrentExecutionFilePathObject)
								{
									this._pageFlags[8] = true;
									this._previousPagePath = virtualPath;
								}
							}
						}
					}
					if (this.MaintainScrollPositionOnPostBack)
					{
						this.LoadScrollPosition();
					}
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "Begin PreInit");
					}
					if (EtwTrace.IsTraceEnabled(5, 4))
					{
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_PRE_INIT_ENTER, this._context.WorkerRequest);
					}
					this.PerformPreInit();
					if (EtwTrace.IsTraceEnabled(5, 4))
					{
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_PRE_INIT_LEAVE, this._context.WorkerRequest);
					}
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "End PreInit");
					}
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "Begin Init");
					}
					if (EtwTrace.IsTraceEnabled(5, 4))
					{
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_INIT_ENTER, this._context.WorkerRequest);
					}
					this.InitRecursive(null);
					if (EtwTrace.IsTraceEnabled(5, 4))
					{
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_INIT_LEAVE, this._context.WorkerRequest);
					}
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "End Init");
					}
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "Begin InitComplete");
					}
					this.OnInitComplete(EventArgs.Empty);
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "End InitComplete");
					}
					if (this.IsPostBack)
					{
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "Begin LoadState");
						}
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_LOAD_VIEWSTATE_ENTER, this._context.WorkerRequest);
						}
						this.LoadAllState();
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_LOAD_VIEWSTATE_LEAVE, this._context.WorkerRequest);
						}
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "End LoadState");
							this.Trace.Write("aspx.page", "Begin ProcessPostData");
						}
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_LOAD_POSTDATA_ENTER, this._context.WorkerRequest);
						}
						this.ProcessPostData(this._requestValueCollection, true);
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_LOAD_POSTDATA_LEAVE, this._context.WorkerRequest);
						}
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "End ProcessPostData");
						}
					}
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "Begin PreLoad");
					}
					this.OnPreLoad(EventArgs.Empty);
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "End PreLoad");
					}
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "Begin Load");
					}
					if (EtwTrace.IsTraceEnabled(5, 4))
					{
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_LOAD_ENTER, this._context.WorkerRequest);
					}
					this.LoadRecursive();
					if (EtwTrace.IsTraceEnabled(5, 4))
					{
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_LOAD_LEAVE, this._context.WorkerRequest);
					}
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "End Load");
					}
					if (this.IsPostBack)
					{
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "Begin ProcessPostData Second Try");
						}
						this.ProcessPostData(this._leftoverPostData, false);
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "End ProcessPostData Second Try");
							this.Trace.Write("aspx.page", "Begin Raise ChangedEvents");
						}
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_POST_DATA_CHANGED_ENTER, this._context.WorkerRequest);
						}
						this.RaiseChangedEvents();
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_POST_DATA_CHANGED_LEAVE, this._context.WorkerRequest);
						}
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "End Raise ChangedEvents");
							this.Trace.Write("aspx.page", "Begin Raise PostBackEvent");
						}
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_RAISE_POSTBACK_ENTER, this._context.WorkerRequest);
						}
						this.RaisePostBackEvent(this._requestValueCollection);
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_RAISE_POSTBACK_LEAVE, this._context.WorkerRequest);
						}
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "End Raise PostBackEvent");
						}
					}
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "Begin LoadComplete");
					}
					this.OnLoadComplete(EventArgs.Empty);
					if (context.TraceIsEnabled)
					{
						this.Trace.Write("aspx.page", "End LoadComplete");
					}
					if (this.IsPostBack && this.IsCallback)
					{
						this.PrepareCallback(text2);
					}
					else if (!this.IsCrossPagePostBack)
					{
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "Begin PreRender");
						}
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_PRE_RENDER_ENTER, this._context.WorkerRequest);
						}
						this.PreRenderRecursiveInternal();
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_PRE_RENDER_LEAVE, this._context.WorkerRequest);
						}
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "End PreRender");
						}
					}
				}
				if (this._asyncInfo == null || this._asyncInfo.CallerIsBlocking)
				{
					this.ExecuteRegisteredAsyncTasks();
				}
				this._request.ValidateRawUrl();
				if (includeStagesAfterAsyncPoint)
				{
					if (this.IsCallback)
					{
						this.RenderCallback();
					}
					else if (!this.IsCrossPagePostBack)
					{
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "Begin PreRenderComplete");
						}
						this.PerformPreRenderComplete();
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "End PreRenderComplete");
						}
						if (context.TraceIsEnabled)
						{
							this.BuildPageProfileTree(this.EnableViewState);
							this.Trace.Write("aspx.page", "Begin SaveState");
						}
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_SAVE_VIEWSTATE_ENTER, this._context.WorkerRequest);
						}
						this.SaveAllState();
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_SAVE_VIEWSTATE_LEAVE, this._context.WorkerRequest);
						}
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "End SaveState");
							this.Trace.Write("aspx.page", "Begin SaveStateComplete");
						}
						this.OnSaveStateComplete(EventArgs.Empty);
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "End SaveStateComplete");
							this.Trace.Write("aspx.page", "Begin Render");
						}
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_RENDER_ENTER, this._context.WorkerRequest);
						}
						if (text != null)
						{
							this.ExportWebPart(text);
						}
						else
						{
							this.RenderControl(this.CreateHtmlTextWriter(this.Response.Output));
						}
						if (EtwTrace.IsTraceEnabled(5, 4))
						{
							EtwTrace.Trace(EtwTraceType.ETW_TYPE_PAGE_RENDER_LEAVE, this._context.WorkerRequest);
						}
						if (context.TraceIsEnabled)
						{
							this.Trace.Write("aspx.page", "End Render");
						}
						this.CheckRemainingAsyncTasks(false);
					}
				}
			}
			catch (ThreadAbortException ex)
			{
				HttpApplication.CancelModuleException ex2 = ex.ExceptionState as HttpApplication.CancelModuleException;
				if (!includeStagesBeforeAsyncPoint || !includeStagesAfterAsyncPoint || this._context.Handler != this || this._context.ApplicationInstance == null || ex2 == null || ex2.Timeout)
				{
					this.CheckRemainingAsyncTasks(true);
					throw;
				}
				this._context.ApplicationInstance.CompleteRequest();
				Thread.ResetAbort();
			}
			catch (ConfigurationException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_DURING_REQUEST);
				PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_TOTAL);
				if (!this.HandleError(ex3))
				{
					throw;
				}
			}
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x000DA894 File Offset: 0x000D9894
		private void BuildPageProfileTree(bool enableViewState)
		{
			if (!this._profileTreeBuilt)
			{
				this._profileTreeBuilt = true;
				base.BuildProfileTree("ROOT", enableViewState);
			}
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x000DA8B4 File Offset: 0x000D98B4
		private void ExportWebPart(string exportedWebPartID)
		{
			WebPart webPart = null;
			WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this);
			if (currentWebPartManager != null)
			{
				webPart = currentWebPartManager.WebParts[exportedWebPartID];
			}
			if (webPart == null || webPart.IsClosed || webPart is ProxyWebPart)
			{
				this.Response.Redirect(this.Request.PathWithQueryString, false);
				return;
			}
			this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			this.Response.Expires = 0;
			this.Response.ContentType = "application/mswebpart";
			string text = webPart.DisplayTitle;
			if (string.IsNullOrEmpty(text))
			{
				text = SR.GetString("Part_Untitled");
			}
			NonWordRegex nonWordRegex = new NonWordRegex();
			this.Response.AddHeader("content-disposition", "attachment; filename=" + nonWordRegex.Replace(text, "") + ".WebPart");
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(this.Response.Output))
			{
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.WriteStartDocument();
				currentWebPartManager.ExportWebPart(webPart, xmlTextWriter);
				xmlTextWriter.WriteEndDocument();
			}
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x000DA9CC File Offset: 0x000D99CC
		private void InitializeWriter(HtmlTextWriter writer)
		{
			Html32TextWriter html32TextWriter = writer as Html32TextWriter;
			if (html32TextWriter != null && this.Request.Browser != null)
			{
				html32TextWriter.ShouldPerformDivTableSubstitution = this.Request.Browser.Tables;
			}
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x000DAA06 File Offset: 0x000D9A06
		protected internal override void Render(HtmlTextWriter writer)
		{
			this.InitializeWriter(writer);
			base.Render(writer);
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x000DAA18 File Offset: 0x000D9A18
		private void PrepareCallback(string callbackControlID)
		{
			this.Response.Cache.SetNoStore();
			try
			{
				string text = this._requestValueCollection["__CALLBACKPARAM"];
				this._callbackControl = this.FindControl(callbackControlID) as ICallbackEventHandler;
				if (this._callbackControl == null)
				{
					throw new InvalidOperationException(SR.GetString("Page_CallBackTargetInvalid", new object[] { callbackControlID }));
				}
				this._callbackControl.RaiseCallbackEvent(text);
			}
			catch (Exception ex)
			{
				this.Response.Clear();
				this.Response.Write('e');
				if (this.Context.IsCustomErrorEnabled)
				{
					this.Response.Write(SR.GetString("Page_CallBackError"));
				}
				else
				{
					bool flag = !string.IsNullOrEmpty(this._requestValueCollection["__CALLBACKLOADSCRIPT"]);
					this.Response.Write(flag ? Util.QuoteJScriptString(HttpUtility.HtmlEncode(ex.Message)) : HttpUtility.HtmlEncode(ex.Message));
				}
			}
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x000DAB24 File Offset: 0x000D9B24
		private void RenderCallback()
		{
			bool flag = !string.IsNullOrEmpty(this._requestValueCollection["__CALLBACKLOADSCRIPT"]);
			try
			{
				string text = null;
				if (flag)
				{
					text = this._requestValueCollection["__CALLBACKINDEX"];
					if (string.IsNullOrEmpty(text))
					{
						throw new HttpException(SR.GetString("Page_CallBackInvalid"));
					}
					for (int i = 0; i < text.Length; i++)
					{
						if (!char.IsDigit(text, i))
						{
							throw new HttpException(SR.GetString("Page_CallBackInvalid"));
						}
					}
					this.Response.Write("<script>parent.__pendingCallbacks[");
					this.Response.Write(text);
					this.Response.Write("].xmlRequest.responseText=\"");
				}
				if (this._callbackControl != null)
				{
					string callbackResult = this._callbackControl.GetCallbackResult();
					if (this.EnableEventValidation)
					{
						string eventValidationFieldValue = this.ClientScript.GetEventValidationFieldValue();
						this.Response.Write(eventValidationFieldValue.Length.ToString(CultureInfo.InvariantCulture));
						this.Response.Write('|');
						this.Response.Write(eventValidationFieldValue);
					}
					else
					{
						this.Response.Write('s');
					}
					this.Response.Write(flag ? Util.QuoteJScriptString(callbackResult) : callbackResult);
				}
				if (flag)
				{
					this.Response.Write("\";parent.__pendingCallbacks[");
					this.Response.Write(text);
					this.Response.Write("].xmlRequest.readyState=4;parent.WebForm_CallbackComplete();</script>");
				}
			}
			catch (Exception ex)
			{
				this.Response.Clear();
				this.Response.Write('e');
				if (this.Context.IsCustomErrorEnabled)
				{
					this.Response.Write(SR.GetString("Page_CallBackError"));
				}
				else
				{
					this.Response.Write(flag ? Util.QuoteJScriptString(HttpUtility.HtmlEncode(ex.Message)) : HttpUtility.HtmlEncode(ex.Message));
				}
			}
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x000DAD14 File Offset: 0x000D9D14
		internal void SetForm(HtmlForm form)
		{
			this._form = form;
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x060031A9 RID: 12713 RVA: 0x000DAD1D File Offset: 0x000D9D1D
		public HtmlForm Form
		{
			get
			{
				return this._form;
			}
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x000DAD25 File Offset: 0x000D9D25
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void RegisterViewStateHandler()
		{
			this._needToPersistViewState = true;
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x000DAD30 File Offset: 0x000D9D30
		private void SaveAllState()
		{
			if (!this._needToPersistViewState)
			{
				return;
			}
			Pair pair = new Pair();
			IDictionary dictionary = null;
			if (this._registeredControlsRequiringControlState != null && this._registeredControlsRequiringControlState.Count > 0)
			{
				dictionary = new HybridDictionary(this._registeredControlsRequiringControlState.Count + 1);
				foreach (object obj in ((IEnumerable)this._registeredControlsRequiringControlState))
				{
					Control control = (Control)obj;
					object obj2 = control.SaveControlStateInternal();
					if (dictionary[control.UniqueID] == null && obj2 != null)
					{
						dictionary.Add(control.UniqueID, obj2);
					}
				}
			}
			if (this._registeredControlsThatRequirePostBack != null && this._registeredControlsThatRequirePostBack.Count > 0)
			{
				if (dictionary == null)
				{
					dictionary = new HybridDictionary();
				}
				dictionary.Add("__ControlsRequirePostBackKey__", this._registeredControlsThatRequirePostBack);
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				pair.First = dictionary;
			}
			Pair pair2 = new Pair(this.GetTypeHashCode().ToString(NumberFormatInfo.InvariantInfo), base.SaveViewStateRecursive());
			if (this.Context.TraceIsEnabled)
			{
				int num = 0;
				if (pair2.Second is Pair)
				{
					num = base.EstimateStateSize(((Pair)pair2.Second).First);
				}
				else if (pair2.Second is Triplet)
				{
					num = base.EstimateStateSize(((Triplet)pair2.Second).First);
				}
				this.Trace.AddControlStateSize(this.UniqueID, num, (dictionary == null) ? 0 : base.EstimateStateSize(dictionary[this.UniqueID]));
			}
			pair.Second = pair2;
			this.SavePageStateToPersistenceMedium(pair);
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x000DAEEC File Offset: 0x000D9EEC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal virtual void SavePageStateToPersistenceMedium(object state)
		{
			PageStatePersister pageStatePersister = this.PageStatePersister;
			if (state is Pair)
			{
				Pair pair = (Pair)state;
				pageStatePersister.ControlState = pair.First;
				pageStatePersister.ViewState = pair.Second;
			}
			else
			{
				pageStatePersister.ViewState = state;
			}
			pageStatePersister.Save();
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x000DAF36 File Offset: 0x000D9F36
		private void SetIntrinsics(HttpContext context)
		{
			this.SetIntrinsics(context, false);
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x000DAF40 File Offset: 0x000D9F40
		private void SetIntrinsics(HttpContext context, bool allowAsync)
		{
			this._context = context;
			this._request = context.Request;
			this._response = context.Response;
			this._application = context.Application;
			this._cache = context.Cache;
			if (!allowAsync && this._context != null && this._context.ApplicationInstance != null)
			{
				this._context.SyncContext.Disable();
			}
			if (!string.IsNullOrEmpty(this._clientTarget))
			{
				this._request.ClientTarget = this._clientTarget;
			}
			HttpCapabilitiesBase browser = this._request.Browser;
			this._response.ContentType = browser.PreferredRenderingMime;
			if (browser != null)
			{
				string preferredResponseEncoding = browser.PreferredResponseEncoding;
				string preferredRequestEncoding = browser.PreferredRequestEncoding;
				if (!string.IsNullOrEmpty(preferredResponseEncoding))
				{
					this._response.ContentEncoding = Encoding.GetEncoding(preferredResponseEncoding);
				}
				if (!string.IsNullOrEmpty(preferredRequestEncoding))
				{
					this._request.ContentEncoding = Encoding.GetEncoding(preferredRequestEncoding);
				}
			}
			base.HookUpAutomaticHandlers();
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x000DB030 File Offset: 0x000DA030
		internal void SetHeader(HtmlHead header)
		{
			this._header = header;
			if (!string.IsNullOrEmpty(this._titleToBeSet))
			{
				if (this._header == null)
				{
					throw new InvalidOperationException(SR.GetString("Page_Title_Requires_Head"));
				}
				this.Title = this._titleToBeSet;
				this._titleToBeSet = null;
			}
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000DB07C File Offset: 0x000DA07C
		internal override void UnloadRecursive(bool dispose)
		{
			base.UnloadRecursive(dispose);
			if (this._previousPage != null && this._previousPage.IsCrossPagePostBack)
			{
				this._previousPage.UnloadRecursive(dispose);
			}
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x000DB0A6 File Offset: 0x000DA0A6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected IAsyncResult AspCompatBeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
		{
			this.SetIntrinsics(context);
			this._aspCompatStep = new AspCompatApplicationStep(context, new AspCompatCallback(this.ProcessRequest));
			return this._aspCompatStep.BeginAspCompatExecution(cb, extraData);
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x000DB0D4 File Offset: 0x000DA0D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void AspCompatEndProcessRequest(IAsyncResult result)
		{
			this._aspCompatStep.EndAspCompatExecution(result);
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x000DB0E4 File Offset: 0x000DA0E4
		public void ExecuteRegisteredAsyncTasks()
		{
			if (this._asyncTaskManager == null)
			{
				return;
			}
			if (this._asyncTaskManager.TaskExecutionInProgress)
			{
				return;
			}
			HttpAsyncResult httpAsyncResult = this._asyncTaskManager.ExecuteTasks(null, null);
			if (httpAsyncResult.Error != null)
			{
				throw new HttpException(null, httpAsyncResult.Error);
			}
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x000DB12C File Offset: 0x000DA12C
		private void CheckRemainingAsyncTasks(bool isThreadAbort)
		{
			if (this._asyncTaskManager != null)
			{
				this._asyncTaskManager.DisposeTimer();
				if (isThreadAbort)
				{
					this._asyncTaskManager.CompleteAllTasksNow(true);
					return;
				}
				if (!this._asyncTaskManager.FailedToStartTasks && this._asyncTaskManager.AnyTasksRemain)
				{
					throw new HttpException(SR.GetString("Registered_async_tasks_remain"));
				}
			}
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x000DB186 File Offset: 0x000DA186
		public void RegisterAsyncTask(PageAsyncTask task)
		{
			if (task == null)
			{
				throw new ArgumentNullException("task");
			}
			if (this._asyncTaskManager == null)
			{
				this._asyncTaskManager = new PageAsyncTaskManager(this);
			}
			this._asyncTaskManager.AddTask(task);
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x000DB1B6 File Offset: 0x000DA1B6
		private void AsyncPageProcessRequestBeforeAsyncPointCancellableCallback(object state)
		{
			this.ProcessRequest(true, false);
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x000DB1C0 File Offset: 0x000DA1C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected IAsyncResult AsyncPageBeginProcessRequest(HttpContext context, AsyncCallback callback, object extraData)
		{
			this.SetIntrinsics(context, true);
			if (this._asyncInfo == null)
			{
				this._asyncInfo = new Page.PageAsyncInfo(this);
			}
			this._asyncInfo.AsyncResult = new HttpAsyncResult(callback, extraData);
			this._asyncInfo.CallerIsBlocking = callback == null;
			try
			{
				this._context.InvokeCancellableCallback(new WaitCallback(this.AsyncPageProcessRequestBeforeAsyncPointCancellableCallback), null);
			}
			catch (Exception ex)
			{
				if (this._context.SyncContext.PendingOperationsCount == 0)
				{
					throw;
				}
				this._asyncInfo.SetError(ex);
			}
			if (this._asyncTaskManager != null && !this._asyncInfo.CallerIsBlocking)
			{
				this._asyncTaskManager.RegisterHandlersForPagePreRenderCompleteAsync();
			}
			this._asyncInfo.AsyncPointReached = true;
			this._context.SyncContext.Disable();
			this._asyncInfo.CallHandlers(true);
			return this._asyncInfo.AsyncResult;
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x000DB2AC File Offset: 0x000DA2AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void AsyncPageEndProcessRequest(IAsyncResult result)
		{
			if (this._asyncInfo == null)
			{
				return;
			}
			this._asyncInfo.AsyncResult.End();
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x000DB2C8 File Offset: 0x000DA2C8
		public void AddOnPreRenderCompleteAsync(BeginEventHandler beginHandler, EndEventHandler endHandler)
		{
			this.AddOnPreRenderCompleteAsync(beginHandler, endHandler, null);
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x000DB2D4 File Offset: 0x000DA2D4
		public void AddOnPreRenderCompleteAsync(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
		{
			if (beginHandler == null)
			{
				throw new ArgumentNullException("beginHandler");
			}
			if (endHandler == null)
			{
				throw new ArgumentNullException("endHandler");
			}
			if (this._asyncInfo == null)
			{
				if (!(this is IHttpAsyncHandler))
				{
					throw new InvalidOperationException(SR.GetString("Async_required"));
				}
				this._asyncInfo = new Page.PageAsyncInfo(this);
			}
			if (this._asyncInfo.AsyncPointReached)
			{
				throw new InvalidOperationException(SR.GetString("Async_addhandler_too_late"));
			}
			this._asyncInfo.AddHandler(beginHandler, endHandler, state);
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x000DB358 File Offset: 0x000DA358
		public virtual void Validate()
		{
			this._validated = true;
			if (this._validators != null)
			{
				for (int i = 0; i < this.Validators.Count; i++)
				{
					this.Validators[i].Validate();
				}
			}
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x000DB39C File Offset: 0x000DA39C
		public virtual void Validate(string validationGroup)
		{
			this._validated = true;
			if (this._validators != null)
			{
				ValidatorCollection validators = this.GetValidators(validationGroup);
				if (string.IsNullOrEmpty(validationGroup) && this._validators.Count == validators.Count)
				{
					this.Validate();
					return;
				}
				for (int i = 0; i < validators.Count; i++)
				{
					validators[i].Validate();
				}
			}
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x000DB400 File Offset: 0x000DA400
		public ValidatorCollection GetValidators(string validationGroup)
		{
			if (validationGroup == null)
			{
				validationGroup = string.Empty;
			}
			ValidatorCollection validatorCollection = new ValidatorCollection();
			if (this._validators != null)
			{
				for (int i = 0; i < this.Validators.Count; i++)
				{
					BaseValidator baseValidator = this.Validators[i] as BaseValidator;
					if (baseValidator != null)
					{
						if (string.Compare(baseValidator.ValidationGroup, validationGroup, StringComparison.Ordinal) == 0)
						{
							validatorCollection.Add(baseValidator);
						}
					}
					else if (validationGroup.Length == 0)
					{
						validatorCollection.Add(this.Validators[i]);
					}
				}
			}
			return validatorCollection;
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x000DB484 File Offset: 0x000DA484
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void VerifyRenderingInServerForm(Control control)
		{
			if (this.Context == null || base.DesignMode)
			{
				return;
			}
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (!this._inOnFormRender && !this.IsCallback)
			{
				throw new HttpException(SR.GetString("ControlRenderedOutsideServerForm", new object[]
				{
					control.ClientID,
					control.GetType().Name
				}));
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x060031BF RID: 12735 RVA: 0x000DB4EF File Offset: 0x000DA4EF
		public PageAdapter PageAdapter
		{
			get
			{
				if (this._pageAdapter == null)
				{
					this.ResolveAdapter();
					this._pageAdapter = (PageAdapter)this._adapter;
				}
				return this._pageAdapter;
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x060031C0 RID: 12736 RVA: 0x000DB518 File Offset: 0x000DA518
		internal string RelativeFilePath
		{
			get
			{
				if (this._relativeFilePath == null)
				{
					string text = this.Context.Request.CurrentExecutionFilePath;
					string filePath = this.Context.Request.FilePath;
					if (filePath.Equals(text))
					{
						int num = text.LastIndexOf('/');
						if (num >= 0)
						{
							text = text.Substring(num + 1);
						}
						this._relativeFilePath = text;
					}
					else
					{
						this._relativeFilePath = this.Server.UrlDecode(UrlPath.MakeRelative(filePath, text));
					}
				}
				return this._relativeFilePath;
			}
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x000DB596 File Offset: 0x000DA596
		internal bool GetDesignModeInternal()
		{
			if (!this._designModeChecked)
			{
				this._designMode = base.Site != null && base.Site.DesignMode;
				this._designModeChecked = true;
			}
			return this._designMode;
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x060031C2 RID: 12738 RVA: 0x000DB5C9 File Offset: 0x000DA5C9
		[Browsable(false)]
		public IDictionary Items
		{
			get
			{
				if (this._items == null)
				{
					this._items = new HybridDictionary();
				}
				return this._items;
			}
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x000DB5E4 File Offset: 0x000DA5E4
		internal void PushDataBindingContext(object dataItem)
		{
			if (this._dataBindingContext == null)
			{
				this._dataBindingContext = new Stack();
			}
			this._dataBindingContext.Push(dataItem);
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x000DB605 File Offset: 0x000DA605
		internal void PopDataBindingContext()
		{
			this._dataBindingContext.Pop();
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x000DB613 File Offset: 0x000DA613
		public object GetDataItem()
		{
			if (this._dataBindingContext == null || this._dataBindingContext.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("Page_MissingDataBindingContext"));
			}
			return this._dataBindingContext.Peek();
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x000DB645 File Offset: 0x000DA645
		internal static bool IsSystemPostField(string field)
		{
			return Page.s_systemPostFields.Contains(field);
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x060031C7 RID: 12743 RVA: 0x000DB652 File Offset: 0x000DA652
		internal IScriptManager ScriptManager
		{
			get
			{
				return (IScriptManager)this.Items[typeof(IScriptManager)];
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x060031C8 RID: 12744 RVA: 0x000DB670 File Offset: 0x000DA670
		internal bool IsPartialRenderingSupported
		{
			get
			{
				if (!this._pageFlags[32])
				{
					Type scriptManagerType = this.ScriptManagerType;
					if (scriptManagerType != null)
					{
						object obj = this.Page.Items[scriptManagerType];
						if (obj != null)
						{
							PropertyInfo property = scriptManagerType.GetProperty("SupportsPartialRendering");
							if (property != null)
							{
								object value = property.GetValue(obj, null);
								this._pageFlags[16] = (bool)value;
							}
						}
					}
					this._pageFlags[32] = true;
				}
				return this._pageFlags[16];
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x060031C9 RID: 12745 RVA: 0x000DB6F1 File Offset: 0x000DA6F1
		internal Type ScriptManagerType
		{
			get
			{
				if (Page._scriptManagerType == null)
				{
					Page._scriptManagerType = BuildManager.GetType("System.Web.UI.ScriptManager", false);
				}
				return Page._scriptManagerType;
			}
		}

		// Token: 0x0400223D RID: 8765
		private const string PageID = "__Page";

		// Token: 0x0400223E RID: 8766
		private const string PageScrollPositionScriptKey = "PageScrollPositionScript";

		// Token: 0x0400223F RID: 8767
		private const string PageSubmitScriptKey = "PageSubmitScript";

		// Token: 0x04002240 RID: 8768
		private const string PageReEnableControlsScriptKey = "PageReEnableControlsScript";

		// Token: 0x04002241 RID: 8769
		private const string PageRegisteredControlsThatRequirePostBackKey = "__ControlsRequirePostBackKey__";

		// Token: 0x04002242 RID: 8770
		private const string EnabledControlArray = "__enabledControlArray";

		// Token: 0x04002243 RID: 8771
		internal const ViewStateEncryptionMode EncryptionModeDefault = ViewStateEncryptionMode.Auto;

		// Token: 0x04002244 RID: 8772
		internal const bool MaintainScrollPositionOnPostBackDefault = false;

		// Token: 0x04002245 RID: 8773
		internal const bool EnableViewStateMacDefault = true;

		// Token: 0x04002246 RID: 8774
		internal const bool EnableEventValidationDefault = true;

		// Token: 0x04002247 RID: 8775
		internal const string systemPostFieldPrefix = "__";

		// Token: 0x04002248 RID: 8776
		[EditorBrowsable(EditorBrowsableState.Never)]
		public const string postEventSourceID = "__EVENTTARGET";

		// Token: 0x04002249 RID: 8777
		private const string lastFocusID = "__LASTFOCUS";

		// Token: 0x0400224A RID: 8778
		private const string _scrollPositionXID = "__SCROLLPOSITIONX";

		// Token: 0x0400224B RID: 8779
		private const string _scrollPositionYID = "__SCROLLPOSITIONY";

		// Token: 0x0400224C RID: 8780
		[EditorBrowsable(EditorBrowsableState.Never)]
		public const string postEventArgumentID = "__EVENTARGUMENT";

		// Token: 0x0400224D RID: 8781
		internal const string ViewStateFieldPrefixID = "__VIEWSTATE";

		// Token: 0x0400224E RID: 8782
		internal const string ViewStateFieldCountID = "__VIEWSTATEFIELDCOUNT";

		// Token: 0x0400224F RID: 8783
		internal const string ViewStateGeneratorFieldID = "__VIEWSTATEGENERATOR";

		// Token: 0x04002250 RID: 8784
		internal const string ViewStateEncryptionID = "__VIEWSTATEENCRYPTED";

		// Token: 0x04002251 RID: 8785
		internal const string EventValidationPrefixID = "__EVENTVALIDATION";

		// Token: 0x04002252 RID: 8786
		internal const string WebPartExportID = "__WEBPARTEXPORT";

		// Token: 0x04002253 RID: 8787
		internal const string callbackID = "__CALLBACKID";

		// Token: 0x04002254 RID: 8788
		internal const string callbackParameterID = "__CALLBACKPARAM";

		// Token: 0x04002255 RID: 8789
		internal const string callbackLoadScriptID = "__CALLBACKLOADSCRIPT";

		// Token: 0x04002256 RID: 8790
		internal const string callbackIndexID = "__CALLBACKINDEX";

		// Token: 0x04002257 RID: 8791
		internal const string previousPageID = "__PREVIOUSPAGE";

		// Token: 0x04002258 RID: 8792
		private const int styleSheetInitialized = 1;

		// Token: 0x04002259 RID: 8793
		private const int isExportingWebPart = 2;

		// Token: 0x0400225A RID: 8794
		private const int isExportingWebPartShared = 4;

		// Token: 0x0400225B RID: 8795
		private const int isCrossPagePostRequest = 8;

		// Token: 0x0400225C RID: 8796
		private const int isPartialRenderingSupported = 16;

		// Token: 0x0400225D RID: 8797
		private const int isPartialRenderingSupportedSet = 32;

		// Token: 0x0400225E RID: 8798
		private const int wasViewStateMacErrorSuppressed = 64;

		// Token: 0x0400225F RID: 8799
		internal const bool BufferDefault = true;

		// Token: 0x04002260 RID: 8800
		internal const bool SmartNavigationDefault = false;

		// Token: 0x04002261 RID: 8801
		internal static readonly object EventPreRenderComplete = new object();

		// Token: 0x04002262 RID: 8802
		internal static readonly object EventPreLoad = new object();

		// Token: 0x04002263 RID: 8803
		internal static readonly object EventLoadComplete = new object();

		// Token: 0x04002264 RID: 8804
		internal static readonly object EventPreInit = new object();

		// Token: 0x04002265 RID: 8805
		internal static readonly object EventInitComplete = new object();

		// Token: 0x04002266 RID: 8806
		internal static readonly object EventSaveStateComplete = new object();

		// Token: 0x04002267 RID: 8807
		private static readonly Version FocusMinimumEcmaVersion = new Version("1.4");

		// Token: 0x04002268 RID: 8808
		private static readonly Version FocusMinimumJScriptVersion = new Version("3.0");

		// Token: 0x04002269 RID: 8809
		private static readonly Version JavascriptMinimumVersion = new Version("1.0");

		// Token: 0x0400226A RID: 8810
		private static readonly Version MSDomScrollMinimumVersion = new Version("4.0");

		// Token: 0x0400226B RID: 8811
		private static readonly string UniqueFilePathSuffixID = "__ufps";

		// Token: 0x0400226C RID: 8812
		private string _uniqueFilePathSuffix;

		// Token: 0x0400226D RID: 8813
		internal static readonly int DefaultMaxPageStateFieldLength = -1;

		// Token: 0x0400226E RID: 8814
		internal static readonly int DefaultAsyncTimeoutSeconds = 45;

		// Token: 0x0400226F RID: 8815
		private int _maxPageStateFieldLength = Page.DefaultMaxPageStateFieldLength;

		// Token: 0x04002270 RID: 8816
		private string _requestViewState;

		// Token: 0x04002271 RID: 8817
		private bool _cachedRequestViewState;

		// Token: 0x04002272 RID: 8818
		private PageAdapter _pageAdapter;

		// Token: 0x04002273 RID: 8819
		private bool _fPageLayoutChanged;

		// Token: 0x04002274 RID: 8820
		private bool _haveIdSeparator;

		// Token: 0x04002275 RID: 8821
		private char _idSeparator;

		// Token: 0x04002276 RID: 8822
		private bool _sessionRetrieved;

		// Token: 0x04002277 RID: 8823
		private HttpSessionState _session;

		// Token: 0x04002278 RID: 8824
		private int _transactionMode;

		// Token: 0x04002279 RID: 8825
		private bool _aspCompatMode;

		// Token: 0x0400227A RID: 8826
		private bool _asyncMode;

		// Token: 0x0400227B RID: 8827
		private TimeSpan _asyncTimeout;

		// Token: 0x0400227C RID: 8828
		private bool _asyncTimeoutSet;

		// Token: 0x0400227D RID: 8829
		private PageAsyncTaskManager _asyncTaskManager;

		// Token: 0x0400227E RID: 8830
		private Page.PageAsyncInfo _asyncInfo;

		// Token: 0x0400227F RID: 8831
		private CultureInfo _dynamicCulture;

		// Token: 0x04002280 RID: 8832
		private CultureInfo _dynamicUICulture;

		// Token: 0x04002281 RID: 8833
		private string _clientState;

		// Token: 0x04002282 RID: 8834
		private PageStatePersister _persister;

		// Token: 0x04002283 RID: 8835
		internal ControlSet _registeredControlsRequiringControlState;

		// Token: 0x04002284 RID: 8836
		private StringSet _controlStateLoadedControlIds;

		// Token: 0x04002285 RID: 8837
		internal HybridDictionary _registeredControlsRequiringClearChildControlState;

		// Token: 0x04002286 RID: 8838
		private ViewStateEncryptionMode _encryptionMode;

		// Token: 0x04002287 RID: 8839
		private bool _viewStateEncryptionRequested;

		// Token: 0x04002288 RID: 8840
		private ArrayList _enabledControls;

		// Token: 0x04002289 RID: 8841
		internal HttpRequest _request;

		// Token: 0x0400228A RID: 8842
		internal HttpResponse _response;

		// Token: 0x0400228B RID: 8843
		internal HttpApplicationState _application;

		// Token: 0x0400228C RID: 8844
		internal Cache _cache;

		// Token: 0x0400228D RID: 8845
		internal string _errorPage;

		// Token: 0x0400228E RID: 8846
		private string _clientTarget;

		// Token: 0x0400228F RID: 8847
		private HtmlForm _form;

		// Token: 0x04002290 RID: 8848
		private bool _inOnFormRender;

		// Token: 0x04002291 RID: 8849
		private bool _fOnFormRenderCalled;

		// Token: 0x04002292 RID: 8850
		private bool _fRequireWebFormsScript;

		// Token: 0x04002293 RID: 8851
		private bool _fWebFormsScriptRendered;

		// Token: 0x04002294 RID: 8852
		private bool _fRequirePostBackScript;

		// Token: 0x04002295 RID: 8853
		private bool _fPostBackScriptRendered;

		// Token: 0x04002296 RID: 8854
		private bool _containsCrossPagePost;

		// Token: 0x04002297 RID: 8855
		private bool _requireFocusScript;

		// Token: 0x04002298 RID: 8856
		private bool _profileTreeBuilt;

		// Token: 0x04002299 RID: 8857
		private bool _maintainScrollPosition;

		// Token: 0x0400229A RID: 8858
		private ClientScriptManager _clientScriptManager;

		// Token: 0x0400229B RID: 8859
		private static Type _scriptManagerType;

		// Token: 0x0400229C RID: 8860
		private bool _requireScrollScript;

		// Token: 0x0400229D RID: 8861
		private bool _isCallback;

		// Token: 0x0400229E RID: 8862
		private bool _isCrossPagePostBack;

		// Token: 0x0400229F RID: 8863
		private bool _containsEncryptedViewState;

		// Token: 0x040022A0 RID: 8864
		private bool _enableEventValidation = true;

		// Token: 0x040022A1 RID: 8865
		private Stack _partialCachingControlStack;

		// Token: 0x040022A2 RID: 8866
		private ArrayList _controlsRequiringPostBack;

		// Token: 0x040022A3 RID: 8867
		private ArrayList _registeredControlsThatRequirePostBack;

		// Token: 0x040022A4 RID: 8868
		private NameValueCollection _leftoverPostData;

		// Token: 0x040022A5 RID: 8869
		private IPostBackEventHandler _registeredControlThatRequireRaiseEvent;

		// Token: 0x040022A6 RID: 8870
		private ArrayList _changedPostDataConsumers;

		// Token: 0x040022A7 RID: 8871
		private bool _needToPersistViewState;

		// Token: 0x040022A8 RID: 8872
		private bool _enableViewStateMac;

		// Token: 0x040022A9 RID: 8873
		private string _viewStateUserKey;

		// Token: 0x040022AA RID: 8874
		private string _themeName;

		// Token: 0x040022AB RID: 8875
		private PageTheme _theme;

		// Token: 0x040022AC RID: 8876
		private string _styleSheetName;

		// Token: 0x040022AD RID: 8877
		private PageTheme _styleSheet;

		// Token: 0x040022AE RID: 8878
		private VirtualPath _masterPageFile;

		// Token: 0x040022AF RID: 8879
		private MasterPage _master;

		// Token: 0x040022B0 RID: 8880
		private IDictionary _contentTemplateCollection;

		// Token: 0x040022B1 RID: 8881
		private SmartNavigationSupport _smartNavSupport;

		// Token: 0x040022B2 RID: 8882
		internal HttpContext _context;

		// Token: 0x040022B3 RID: 8883
		private ValidatorCollection _validators;

		// Token: 0x040022B4 RID: 8884
		private bool _validated;

		// Token: 0x040022B5 RID: 8885
		private HtmlHead _header;

		// Token: 0x040022B6 RID: 8886
		private int _supportsStyleSheets;

		// Token: 0x040022B7 RID: 8887
		private Control _autoPostBackControl;

		// Token: 0x040022B8 RID: 8888
		private string _focusedControlID;

		// Token: 0x040022B9 RID: 8889
		private Control _focusedControl;

		// Token: 0x040022BA RID: 8890
		private string _validatorInvalidControl;

		// Token: 0x040022BB RID: 8891
		private int _scrollPositionX;

		// Token: 0x040022BC RID: 8892
		private int _scrollPositionY;

		// Token: 0x040022BD RID: 8893
		private Page _previousPage;

		// Token: 0x040022BE RID: 8894
		private VirtualPath _previousPagePath;

		// Token: 0x040022BF RID: 8895
		private bool _preInitWorkComplete;

		// Token: 0x040022C0 RID: 8896
		private bool _clientSupportsJavaScriptChecked;

		// Token: 0x040022C1 RID: 8897
		private bool _clientSupportsJavaScript;

		// Token: 0x040022C2 RID: 8898
		private string _titleToBeSet;

		// Token: 0x040022C3 RID: 8899
		private ICallbackEventHandler _callbackControl;

		// Token: 0x040022C4 RID: 8900
		private bool _xhtmlConformanceModeSet;

		// Token: 0x040022C5 RID: 8901
		private XhtmlConformanceMode _xhtmlConformanceMode;

		// Token: 0x040022C6 RID: 8902
		private SimpleBitVector32 _pageFlags;

		// Token: 0x040022C7 RID: 8903
		private NameValueCollection _requestValueCollection;

		// Token: 0x040022C8 RID: 8904
		private static StringSet s_systemPostFields = new StringSet();

		// Token: 0x040022C9 RID: 8905
		private string _clientQueryString;

		// Token: 0x040022CA RID: 8906
		private static char[] s_varySeparator = new char[] { ';' };

		// Token: 0x040022CB RID: 8907
		private AspCompatApplicationStep _aspCompatStep;

		// Token: 0x040022CC RID: 8908
		private string _relativeFilePath;

		// Token: 0x040022CD RID: 8909
		private bool _designModeChecked;

		// Token: 0x040022CE RID: 8910
		private bool _designMode;

		// Token: 0x040022CF RID: 8911
		private IDictionary _items;

		// Token: 0x040022D0 RID: 8912
		private Stack _dataBindingContext;

		// Token: 0x020003EE RID: 1006
		private class PageAsyncInfo
		{
			// Token: 0x060031CA RID: 12746 RVA: 0x000DB710 File Offset: 0x000DA710
			internal PageAsyncInfo(Page page)
			{
				this._page = page;
				this._app = page.Context.ApplicationInstance;
				this._syncContext = page.Context.SyncContext;
				this._completionCallback = new AsyncCallback(this.OnAsyncHandlerCompletion);
				this._callHandlersThreadpoolCallback = new WaitCallback(this.CallHandlersFromThreadpoolThread);
				this._callHandlersCancellableCallback = new WaitCallback(this.CallHandlersCancellableCallback);
			}

			// Token: 0x17000AEC RID: 2796
			// (get) Token: 0x060031CB RID: 12747 RVA: 0x000DB782 File Offset: 0x000DA782
			// (set) Token: 0x060031CC RID: 12748 RVA: 0x000DB78A File Offset: 0x000DA78A
			internal HttpAsyncResult AsyncResult
			{
				get
				{
					return this._asyncResult;
				}
				set
				{
					this._asyncResult = value;
				}
			}

			// Token: 0x17000AED RID: 2797
			// (get) Token: 0x060031CD RID: 12749 RVA: 0x000DB793 File Offset: 0x000DA793
			// (set) Token: 0x060031CE RID: 12750 RVA: 0x000DB79B File Offset: 0x000DA79B
			internal bool AsyncPointReached
			{
				get
				{
					return this._asyncPointReached;
				}
				set
				{
					this._asyncPointReached = value;
				}
			}

			// Token: 0x17000AEE RID: 2798
			// (get) Token: 0x060031CF RID: 12751 RVA: 0x000DB7A4 File Offset: 0x000DA7A4
			// (set) Token: 0x060031D0 RID: 12752 RVA: 0x000DB7AC File Offset: 0x000DA7AC
			internal bool CallerIsBlocking
			{
				get
				{
					return this._callerIsBlocking;
				}
				set
				{
					this._callerIsBlocking = value;
				}
			}

			// Token: 0x060031D1 RID: 12753 RVA: 0x000DB7B8 File Offset: 0x000DA7B8
			internal void AddHandler(BeginEventHandler beginHandler, EndEventHandler endHandler, object state)
			{
				if (this._handlerCount == 0)
				{
					this._beginHandlers = new ArrayList();
					this._endHandlers = new ArrayList();
					this._stateObjects = new ArrayList();
				}
				this._beginHandlers.Add(beginHandler);
				this._endHandlers.Add(endHandler);
				this._stateObjects.Add(state);
				this._handlerCount++;
			}

			// Token: 0x060031D2 RID: 12754 RVA: 0x000DB824 File Offset: 0x000DA824
			private void CallHandlersCancellableCallback(object state)
			{
				bool flag = (bool)state;
				if (this.CallerIsBlocking || flag)
				{
					this.CallHandlersPossiblyUnderLock(flag);
					return;
				}
				lock (this._app)
				{
					this.CallHandlersPossiblyUnderLock(flag);
				}
			}

			// Token: 0x060031D3 RID: 12755 RVA: 0x000DB878 File Offset: 0x000DA878
			internal void CallHandlers(bool onPageThread)
			{
				try
				{
					this._page.Context.InvokeCancellableCallback(this._callHandlersCancellableCallback, onPageThread);
				}
				catch (Exception ex)
				{
					this._error = ex;
					this._completed = true;
					this._asyncResult.Complete(onPageThread, null, this._error);
					if (!onPageThread && ex is ThreadAbortException && ((ThreadAbortException)ex).ExceptionState is HttpApplication.CancelModuleException)
					{
						Thread.ResetAbort();
					}
				}
			}

			// Token: 0x060031D4 RID: 12756 RVA: 0x000DB8FC File Offset: 0x000DA8FC
			private void CallHandlersPossiblyUnderLock(bool onPageThread)
			{
				HttpApplication.ThreadContext threadContext = null;
				if (!onPageThread)
				{
					threadContext = this._app.OnThreadEnter();
				}
				try
				{
					while (this._currentHandler < this._handlerCount && this._error == null)
					{
						try
						{
							IAsyncResult asyncResult = ((BeginEventHandler)this._beginHandlers[this._currentHandler])(this._page, EventArgs.Empty, this._completionCallback, this._stateObjects[this._currentHandler]);
							if (asyncResult == null)
							{
								throw new InvalidOperationException(SR.GetString("Async_null_asyncresult"));
							}
							if (!asyncResult.CompletedSynchronously)
							{
								return;
							}
							try
							{
								((EndEventHandler)this._endHandlers[this._currentHandler])(asyncResult);
							}
							finally
							{
								this._currentHandler++;
							}
						}
						catch (Exception ex)
						{
							if (onPageThread && this._syncContext.PendingOperationsCount == 0)
							{
								throw;
							}
							PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_DURING_REQUEST);
							PerfCounters.IncrementCounter(AppPerfCounter.ERRORS_TOTAL);
							try
							{
								if (!this._page.HandleError(ex))
								{
									this._error = ex;
								}
							}
							catch (Exception ex2)
							{
								this._error = ex2;
							}
						}
					}
					if (this._syncContext.PendingOperationsCount > 0)
					{
						this._syncContext.SetLastCompletionWorkItem(this._callHandlersThreadpoolCallback);
					}
					else
					{
						if (this._error == null && this._syncContext.Error != null)
						{
							try
							{
								if (!this._page.HandleError(this._syncContext.Error))
								{
									this._error = this._syncContext.Error;
									this._syncContext.ClearError();
								}
							}
							catch (Exception ex3)
							{
								this._error = ex3;
							}
						}
						try
						{
							this._page.ProcessRequest(false, true);
						}
						catch (Exception ex4)
						{
							if (onPageThread)
							{
								throw;
							}
							this._error = ex4;
						}
						if (threadContext != null)
						{
							try
							{
								threadContext.Leave();
							}
							finally
							{
								threadContext = null;
							}
						}
						this._completed = true;
						this._asyncResult.Complete(onPageThread, null, this._error);
					}
				}
				finally
				{
					if (threadContext != null)
					{
						threadContext.Leave();
					}
				}
			}

			// Token: 0x060031D5 RID: 12757 RVA: 0x000DBB84 File Offset: 0x000DAB84
			private void OnAsyncHandlerCompletion(IAsyncResult ar)
			{
				if (ar.CompletedSynchronously)
				{
					return;
				}
				try
				{
					((EndEventHandler)this._endHandlers[this._currentHandler])(ar);
				}
				catch (Exception ex)
				{
					this._error = ex;
				}
				if (this._completed)
				{
					return;
				}
				this._currentHandler++;
				if (Thread.CurrentThread.IsThreadPoolThread)
				{
					this.CallHandlers(false);
					return;
				}
				ThreadPool.QueueUserWorkItem(this._callHandlersThreadpoolCallback);
			}

			// Token: 0x060031D6 RID: 12758 RVA: 0x000DBC0C File Offset: 0x000DAC0C
			private void CallHandlersFromThreadpoolThread(object data)
			{
				this.CallHandlers(false);
			}

			// Token: 0x060031D7 RID: 12759 RVA: 0x000DBC15 File Offset: 0x000DAC15
			internal void SetError(Exception error)
			{
				this._error = error;
			}

			// Token: 0x040022D1 RID: 8913
			private Page _page;

			// Token: 0x040022D2 RID: 8914
			private bool _callerIsBlocking;

			// Token: 0x040022D3 RID: 8915
			private HttpApplication _app;

			// Token: 0x040022D4 RID: 8916
			private AspNetSynchronizationContext _syncContext;

			// Token: 0x040022D5 RID: 8917
			private HttpAsyncResult _asyncResult;

			// Token: 0x040022D6 RID: 8918
			private bool _asyncPointReached;

			// Token: 0x040022D7 RID: 8919
			private int _handlerCount;

			// Token: 0x040022D8 RID: 8920
			private ArrayList _beginHandlers;

			// Token: 0x040022D9 RID: 8921
			private ArrayList _endHandlers;

			// Token: 0x040022DA RID: 8922
			private ArrayList _stateObjects;

			// Token: 0x040022DB RID: 8923
			private AsyncCallback _completionCallback;

			// Token: 0x040022DC RID: 8924
			private WaitCallback _callHandlersThreadpoolCallback;

			// Token: 0x040022DD RID: 8925
			private WaitCallback _callHandlersCancellableCallback;

			// Token: 0x040022DE RID: 8926
			private int _currentHandler;

			// Token: 0x040022DF RID: 8927
			private Exception _error;

			// Token: 0x040022E0 RID: 8928
			private bool _completed;
		}
	}
}
