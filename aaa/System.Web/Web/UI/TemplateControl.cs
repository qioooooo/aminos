using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.Util;
using System.Xml;

namespace System.Web.UI
{
	// Token: 0x020003EA RID: 1002
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class TemplateControl : Control, INamingContainer, IFilterResolutionService
	{
		// Token: 0x06003071 RID: 12401 RVA: 0x000D58BC File Offset: 0x000D48BC
		static TemplateControl()
		{
			TemplateControl._eventObjects.Add("Page_PreInit", Page.EventPreInit);
			TemplateControl._eventObjects.Add("Page_Init", Control.EventInit);
			TemplateControl._eventObjects.Add("Page_InitComplete", Page.EventInitComplete);
			TemplateControl._eventObjects.Add("Page_Load", Control.EventLoad);
			TemplateControl._eventObjects.Add("Page_PreLoad", Page.EventPreLoad);
			TemplateControl._eventObjects.Add("Page_LoadComplete", Page.EventLoadComplete);
			TemplateControl._eventObjects.Add("Page_PreRenderComplete", Page.EventPreRenderComplete);
			TemplateControl._eventObjects.Add("Page_DataBind", Control.EventDataBinding);
			TemplateControl._eventObjects.Add("Page_PreRender", Control.EventPreRender);
			TemplateControl._eventObjects.Add("Page_SaveStateComplete", Page.EventSaveStateComplete);
			TemplateControl._eventObjects.Add("Page_Unload", Control.EventUnload);
			TemplateControl._eventObjects.Add("Page_Error", TemplateControl.EventError);
			TemplateControl._eventObjects.Add("Page_AbortTransaction", TemplateControl.EventAbortTransaction);
			TemplateControl._eventObjects.Add("OnTransactionAbort", TemplateControl.EventAbortTransaction);
			TemplateControl._eventObjects.Add("Page_CommitTransaction", TemplateControl.EventCommitTransaction);
			TemplateControl._eventObjects.Add("OnTransactionCommit", TemplateControl.EventCommitTransaction);
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x000D5A51 File Offset: 0x000D4A51
		protected TemplateControl()
		{
			this.Construct();
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x000D5A5F File Offset: 0x000D4A5F
		protected virtual void Construct()
		{
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06003074 RID: 12404 RVA: 0x000D5A61 File Offset: 0x000D4A61
		// (remove) Token: 0x06003075 RID: 12405 RVA: 0x000D5A74 File Offset: 0x000D4A74
		[WebSysDescription("Page_OnCommitTransaction")]
		public event EventHandler CommitTransaction
		{
			add
			{
				base.Events.AddHandler(TemplateControl.EventCommitTransaction, value);
			}
			remove
			{
				base.Events.RemoveHandler(TemplateControl.EventCommitTransaction, value);
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06003076 RID: 12406 RVA: 0x000D5A87 File Offset: 0x000D4A87
		// (set) Token: 0x06003077 RID: 12407 RVA: 0x000D5A8F File Offset: 0x000D4A8F
		[Browsable(true)]
		public override bool EnableTheming
		{
			get
			{
				return base.EnableTheming;
			}
			set
			{
				base.EnableTheming = value;
			}
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x000D5A98 File Offset: 0x000D4A98
		protected virtual void OnCommitTransaction(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[TemplateControl.EventCommitTransaction];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06003079 RID: 12409 RVA: 0x000D5AC6 File Offset: 0x000D4AC6
		// (remove) Token: 0x0600307A RID: 12410 RVA: 0x000D5AD9 File Offset: 0x000D4AD9
		[WebSysDescription("Page_OnAbortTransaction")]
		public event EventHandler AbortTransaction
		{
			add
			{
				base.Events.AddHandler(TemplateControl.EventAbortTransaction, value);
			}
			remove
			{
				base.Events.RemoveHandler(TemplateControl.EventAbortTransaction, value);
			}
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x000D5AEC File Offset: 0x000D4AEC
		protected virtual void OnAbortTransaction(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[TemplateControl.EventAbortTransaction];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x0600307C RID: 12412 RVA: 0x000D5B1A File Offset: 0x000D4B1A
		// (remove) Token: 0x0600307D RID: 12413 RVA: 0x000D5B2D File Offset: 0x000D4B2D
		[WebSysDescription("Page_Error")]
		public event EventHandler Error
		{
			add
			{
				base.Events.AddHandler(TemplateControl.EventError, value);
			}
			remove
			{
				base.Events.RemoveHandler(TemplateControl.EventError, value);
			}
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x000D5B40 File Offset: 0x000D4B40
		protected virtual void OnError(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[TemplateControl.EventError];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x000D5B6E File Offset: 0x000D4B6E
		internal void SetNoCompileBuildResult(BuildResultNoCompileTemplateControl noCompileBuildResult)
		{
			this._noCompileBuildResult = noCompileBuildResult;
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x000D5B77 File Offset: 0x000D4B77
		internal bool NoCompile
		{
			get
			{
				return this._noCompileBuildResult != null;
			}
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x000D5B85 File Offset: 0x000D4B85
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void FrameworkInitialize()
		{
			if (this.NoCompile)
			{
				if (HttpRuntime.NamedPermissionSet != null && !HttpRuntime.ProcessRequestInApplicationTrust)
				{
					HttpRuntime.NamedPermissionSet.PermitOnly();
				}
				this._noCompileBuildResult.FrameworkInitialize(this);
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x000D5BB3 File Offset: 0x000D4BB3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual bool SupportAutoEvents
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003083 RID: 12419 RVA: 0x000D5BB6 File Offset: 0x000D4BB6
		internal IntPtr StringResourcePointer
		{
			get
			{
				return this._stringResourcePointer;
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x000D5BBE File Offset: 0x000D4BBE
		internal int MaxResourceOffset
		{
			get
			{
				return this._maxResourceOffset;
			}
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x000D5BC6 File Offset: 0x000D4BC6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static object ReadStringResource(Type t)
		{
			return StringResourceManager.ReadSafeStringResource(t);
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x000D5BCE File Offset: 0x000D4BCE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public object ReadStringResource()
		{
			return StringResourceManager.ReadSafeStringResource(base.GetType());
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x000D5BDB File Offset: 0x000D4BDB
		protected LiteralControl CreateResourceBasedLiteralControl(int offset, int size, bool fAsciiOnly)
		{
			return new ResourceBasedLiteralControl(this, offset, size, fAsciiOnly);
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x000D5BE8 File Offset: 0x000D4BE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void SetStringResourcePointer(object stringResourcePointer, int maxResourceOffset)
		{
			SafeStringResource safeStringResource = (SafeStringResource)stringResourcePointer;
			this._stringResourcePointer = safeStringResource.StringResourcePointer;
			this._maxResourceOffset = safeStringResource.ResourceSize;
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06003089 RID: 12425 RVA: 0x000D5C14 File Offset: 0x000D4C14
		internal VirtualPath VirtualPath
		{
			get
			{
				return this._virtualPath;
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x0600308A RID: 12426 RVA: 0x000D5C1C File Offset: 0x000D4C1C
		// (set) Token: 0x0600308B RID: 12427 RVA: 0x000D5C29 File Offset: 0x000D4C29
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public string AppRelativeVirtualPath
		{
			get
			{
				return VirtualPath.GetAppRelativeVirtualPathString(this.TemplateControlVirtualPath);
			}
			set
			{
				this.TemplateControlVirtualPath = VirtualPath.CreateNonRelative(value);
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x0600308C RID: 12428 RVA: 0x000D5C37 File Offset: 0x000D4C37
		// (set) Token: 0x0600308D RID: 12429 RVA: 0x000D5C3F File Offset: 0x000D4C3F
		internal VirtualPath TemplateControlVirtualPath
		{
			get
			{
				return this._virtualPath;
			}
			set
			{
				this._virtualPath = value;
				base.TemplateControlVirtualDirectory = this._virtualPath.Parent;
			}
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x000D5C59 File Offset: 0x000D4C59
		public virtual bool TestDeviceFilter(string filterName)
		{
			return this.Context.Request.Browser.IsBrowser(filterName);
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x000D5C71 File Offset: 0x000D4C71
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected void WriteUTF8ResourceString(HtmlTextWriter output, int offset, int size, bool fAsciiOnly)
		{
			if (offset < 0 || size < 0 || checked(offset + size) > this._maxResourceOffset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			output.WriteUTF8ResourceString(this.StringResourcePointer, offset, size, fAsciiOnly);
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003090 RID: 12432 RVA: 0x000D5CA1 File Offset: 0x000D4CA1
		// (set) Token: 0x06003091 RID: 12433 RVA: 0x000D5CA4 File Offset: 0x000D4CA4
		[Obsolete("Use of this property is not recommended because it is no longer useful. http://go.microsoft.com/fwlink/?linkid=14202")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual int AutoHandlers
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x000D5CA6 File Offset: 0x000D4CA6
		internal override TemplateControl GetTemplateControl()
		{
			return this;
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x000D5CAC File Offset: 0x000D4CAC
		internal void HookUpAutomaticHandlers()
		{
			if (!this.SupportAutoEvents)
			{
				return;
			}
			object obj = TemplateControl._eventListCache[base.GetType()];
			IDictionary dictionary;
			if (obj == null)
			{
				lock (TemplateControl._lockObject)
				{
					obj = TemplateControl._eventListCache[base.GetType()];
					if (obj == null)
					{
						dictionary = new ListDictionary();
						this.GetDelegateInformation(dictionary);
						if (dictionary.Count == 0)
						{
							obj = TemplateControl._emptyEventSingleton;
						}
						else
						{
							obj = dictionary;
						}
						TemplateControl._eventListCache[base.GetType()] = obj;
					}
				}
			}
			if (obj == TemplateControl._emptyEventSingleton)
			{
				return;
			}
			dictionary = (IDictionary)obj;
			foreach (object obj2 in dictionary.Keys)
			{
				string text = (string)obj2;
				TemplateControl.EventMethodInfo eventMethodInfo = (TemplateControl.EventMethodInfo)dictionary[text];
				bool flag = false;
				MethodInfo methodInfo = eventMethodInfo.MethodInfo;
				Delegate @delegate = base.Events[TemplateControl._eventObjects[text]];
				if (@delegate != null)
				{
					foreach (Delegate delegate2 in @delegate.GetInvocationList())
					{
						if (delegate2.Method.Equals(methodInfo))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					IntPtr functionPointer = methodInfo.MethodHandle.GetFunctionPointer();
					EventHandler handler = new CalliEventHandlerDelegateProxy(this, functionPointer, eventMethodInfo.IsArgless).Handler;
					base.Events.AddHandler(TemplateControl._eventObjects[text], handler);
				}
			}
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x000D5E50 File Offset: 0x000D4E50
		private void GetDelegateInformation(IDictionary dictionary)
		{
			if (HttpRuntime.IsFullTrust)
			{
				this.GetDelegateInformationWithNoAssert(dictionary);
				return;
			}
			this.GetDelegateInformationWithAssert(dictionary);
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x000D5E68 File Offset: 0x000D4E68
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
		private void GetDelegateInformationWithAssert(IDictionary dictionary)
		{
			this.GetDelegateInformationWithNoAssert(dictionary);
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x000D5E74 File Offset: 0x000D4E74
		private void GetDelegateInformationWithNoAssert(IDictionary dictionary)
		{
			if (this is Page)
			{
				this.GetDelegateInformationFromMethod("Page_PreInit", dictionary);
				this.GetDelegateInformationFromMethod("Page_PreLoad", dictionary);
				this.GetDelegateInformationFromMethod("Page_LoadComplete", dictionary);
				this.GetDelegateInformationFromMethod("Page_PreRenderComplete", dictionary);
				this.GetDelegateInformationFromMethod("Page_InitComplete", dictionary);
				this.GetDelegateInformationFromMethod("Page_SaveStateComplete", dictionary);
			}
			this.GetDelegateInformationFromMethod("Page_Init", dictionary);
			this.GetDelegateInformationFromMethod("Page_Load", dictionary);
			this.GetDelegateInformationFromMethod("Page_DataBind", dictionary);
			this.GetDelegateInformationFromMethod("Page_PreRender", dictionary);
			this.GetDelegateInformationFromMethod("Page_Unload", dictionary);
			this.GetDelegateInformationFromMethod("Page_Error", dictionary);
			if (!this.GetDelegateInformationFromMethod("Page_AbortTransaction", dictionary))
			{
				this.GetDelegateInformationFromMethod("OnTransactionAbort", dictionary);
			}
			if (!this.GetDelegateInformationFromMethod("Page_CommitTransaction", dictionary))
			{
				this.GetDelegateInformationFromMethod("OnTransactionCommit", dictionary);
			}
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x000D5F5C File Offset: 0x000D4F5C
		private bool GetDelegateInformationFromMethod(string methodName, IDictionary dictionary)
		{
			EventHandler eventHandler = (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), this, methodName, true, false);
			if (eventHandler != null)
			{
				dictionary[methodName] = new TemplateControl.EventMethodInfo(eventHandler.Method, false);
				return true;
			}
			VoidMethod voidMethod = (VoidMethod)Delegate.CreateDelegate(typeof(VoidMethod), this, methodName, true, false);
			if (voidMethod != null)
			{
				dictionary[methodName] = new TemplateControl.EventMethodInfo(voidMethod.Method, true);
				return true;
			}
			return false;
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x000D5FCC File Offset: 0x000D4FCC
		public Control LoadControl(string virtualPath)
		{
			return this.LoadControl(VirtualPath.Create(virtualPath));
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x000D5FDC File Offset: 0x000D4FDC
		internal Control LoadControl(VirtualPath virtualPath)
		{
			virtualPath = VirtualPath.Combine(base.TemplateControlVirtualDirectory, virtualPath);
			BuildResult vpathBuildResult = BuildManager.GetVPathBuildResult(this.Context, virtualPath);
			return this.LoadControl((IWebObjectFactory)vpathBuildResult, virtualPath, null, null);
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x000D6014 File Offset: 0x000D5014
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
		private void AddStackContextToHashCode(HashCodeCombiner combinedHashCode)
		{
			StackTrace stackTrace = new StackTrace();
			int num = 2;
			for (;;)
			{
				StackFrame frame = stackTrace.GetFrame(num);
				if (frame.GetMethod().DeclaringType != typeof(TemplateControl))
				{
					break;
				}
				num++;
			}
			for (int i = num; i < num + 2; i++)
			{
				StackFrame frame2 = stackTrace.GetFrame(i);
				combinedHashCode.AddObject(frame2.GetMethod());
				combinedHashCode.AddObject(frame2.GetNativeOffset());
			}
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x000D607F File Offset: 0x000D507F
		public Control LoadControl(Type t, object[] parameters)
		{
			return this.LoadControl(null, null, t, parameters);
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x000D608C File Offset: 0x000D508C
		private Control LoadControl(IWebObjectFactory objectFactory, VirtualPath virtualPath, Type t, object[] parameters)
		{
			BuildResultNoCompileUserControl buildResultNoCompileUserControl = null;
			if (objectFactory != null)
			{
				BuildResultCompiledType buildResultCompiledType = objectFactory as BuildResultCompiledType;
				if (buildResultCompiledType != null)
				{
					t = buildResultCompiledType.ResultType;
					Util.CheckAssignableType(typeof(UserControl), t);
				}
				else
				{
					buildResultNoCompileUserControl = (BuildResultNoCompileUserControl)objectFactory;
				}
			}
			else if (t != null)
			{
				Util.CheckAssignableType(typeof(Control), t);
			}
			PartialCachingAttribute partialCachingAttribute;
			if (t != null)
			{
				partialCachingAttribute = (PartialCachingAttribute)TypeDescriptor.GetAttributes(t)[typeof(PartialCachingAttribute)];
			}
			else
			{
				partialCachingAttribute = buildResultNoCompileUserControl.CachingAttribute;
			}
			if (partialCachingAttribute == null)
			{
				Control control;
				if (objectFactory != null)
				{
					control = (Control)objectFactory.CreateInstance();
				}
				else
				{
					control = (Control)HttpRuntime.CreatePublicInstance(t, parameters);
				}
				UserControl userControl = control as UserControl;
				if (userControl != null)
				{
					if (virtualPath != null)
					{
						userControl.TemplateControlVirtualPath = virtualPath;
					}
					userControl.InitializeAsUserControl(this.Page);
				}
				return control;
			}
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			if (objectFactory != null)
			{
				hashCodeCombiner.AddObject(objectFactory);
			}
			else
			{
				hashCodeCombiner.AddObject(t);
			}
			if (!partialCachingAttribute.Shared)
			{
				this.AddStackContextToHashCode(hashCodeCombiner);
			}
			string combinedHashString = hashCodeCombiner.CombinedHashString;
			return new PartialCachingControl(objectFactory, t, partialCachingAttribute, "_" + combinedHashString, parameters);
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x000D61A0 File Offset: 0x000D51A0
		public ITemplate LoadTemplate(string virtualPath)
		{
			return this.LoadTemplate(VirtualPath.Create(virtualPath));
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x000D61B0 File Offset: 0x000D51B0
		internal ITemplate LoadTemplate(VirtualPath virtualPath)
		{
			virtualPath = VirtualPath.Combine(base.TemplateControlVirtualDirectory, virtualPath);
			ITypedWebObjectFactory typedWebObjectFactory = (ITypedWebObjectFactory)BuildManager.GetVPathBuildResult(this.Context, virtualPath);
			return new TemplateControl.SimpleTemplate(typedWebObjectFactory);
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x000D61E3 File Offset: 0x000D51E3
		public Control ParseControl(string content)
		{
			return this.ParseControl(content, true);
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x000D61ED File Offset: 0x000D51ED
		public Control ParseControl(string content, bool ignoreParserFilter)
		{
			return TemplateParser.ParseControl(content, VirtualPath.Create(this.AppRelativeVirtualPath), ignoreParserFilter);
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000D6201 File Offset: 0x000D5201
		private void CheckPageExists()
		{
			if (this.Page == null)
			{
				throw new InvalidOperationException(SR.GetString("TemplateControl_DataBindingRequiresPage"));
			}
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x000D621B File Offset: 0x000D521B
		protected internal object Eval(string expression)
		{
			this.CheckPageExists();
			return DataBinder.Eval(this.Page.GetDataItem(), expression);
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x000D6234 File Offset: 0x000D5234
		protected internal string Eval(string expression, string format)
		{
			this.CheckPageExists();
			return DataBinder.Eval(this.Page.GetDataItem(), expression, format);
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x000D624E File Offset: 0x000D524E
		protected internal object XPath(string xPathExpression)
		{
			this.CheckPageExists();
			return XPathBinder.Eval(this.Page.GetDataItem(), xPathExpression);
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x000D6267 File Offset: 0x000D5267
		protected internal object XPath(string xPathExpression, IXmlNamespaceResolver resolver)
		{
			this.CheckPageExists();
			return XPathBinder.Eval(this.Page.GetDataItem(), xPathExpression, resolver);
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x000D6281 File Offset: 0x000D5281
		protected internal string XPath(string xPathExpression, string format)
		{
			this.CheckPageExists();
			return XPathBinder.Eval(this.Page.GetDataItem(), xPathExpression, format);
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x000D629B File Offset: 0x000D529B
		protected internal string XPath(string xPathExpression, string format, IXmlNamespaceResolver resolver)
		{
			this.CheckPageExists();
			return XPathBinder.Eval(this.Page.GetDataItem(), xPathExpression, format, resolver);
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x000D62B6 File Offset: 0x000D52B6
		protected internal IEnumerable XPathSelect(string xPathExpression)
		{
			this.CheckPageExists();
			return XPathBinder.Select(this.Page.GetDataItem(), xPathExpression);
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x000D62CF File Offset: 0x000D52CF
		protected internal IEnumerable XPathSelect(string xPathExpression, IXmlNamespaceResolver resolver)
		{
			this.CheckPageExists();
			return XPathBinder.Select(this.Page.GetDataItem(), xPathExpression, resolver);
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x000D62E9 File Offset: 0x000D52E9
		protected object GetLocalResourceObject(string resourceKey)
		{
			if (this._resourceProvider == null)
			{
				this._resourceProvider = ResourceExpressionBuilder.GetLocalResourceProvider(this);
			}
			return ResourceExpressionBuilder.GetResourceObject(this._resourceProvider, resourceKey, null);
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x000D630C File Offset: 0x000D530C
		protected object GetLocalResourceObject(string resourceKey, Type objType, string propName)
		{
			if (this._resourceProvider == null)
			{
				this._resourceProvider = ResourceExpressionBuilder.GetLocalResourceProvider(this);
			}
			return ResourceExpressionBuilder.GetResourceObject(this._resourceProvider, resourceKey, null, objType, propName);
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x000D6331 File Offset: 0x000D5331
		protected object GetGlobalResourceObject(string className, string resourceKey)
		{
			return ResourceExpressionBuilder.GetGlobalResourceObject(className, resourceKey, null, null, null);
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x000D633D File Offset: 0x000D533D
		protected object GetGlobalResourceObject(string className, string resourceKey, Type objType, string propName)
		{
			return ResourceExpressionBuilder.GetGlobalResourceObject(className, resourceKey, objType, propName, null);
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x000D634A File Offset: 0x000D534A
		bool IFilterResolutionService.EvaluateFilter(string filterName)
		{
			return this.TestDeviceFilter(filterName);
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x000D6353 File Offset: 0x000D5353
		int IFilterResolutionService.CompareFilters(string filter1, string filter2)
		{
			return BrowserCapabilitiesCompiler.BrowserCapabilitiesFactory.CompareFilters(filter1, filter2);
		}

		// Token: 0x0400221E RID: 8734
		private const string _pagePreInitEventName = "Page_PreInit";

		// Token: 0x0400221F RID: 8735
		private const string _pageInitEventName = "Page_Init";

		// Token: 0x04002220 RID: 8736
		private const string _pageInitCompleteEventName = "Page_InitComplete";

		// Token: 0x04002221 RID: 8737
		private const string _pageLoadEventName = "Page_Load";

		// Token: 0x04002222 RID: 8738
		private const string _pagePreLoadEventName = "Page_PreLoad";

		// Token: 0x04002223 RID: 8739
		private const string _pageLoadCompleteEventName = "Page_LoadComplete";

		// Token: 0x04002224 RID: 8740
		private const string _pagePreRenderCompleteEventName = "Page_PreRenderComplete";

		// Token: 0x04002225 RID: 8741
		private const string _pageDataBindEventName = "Page_DataBind";

		// Token: 0x04002226 RID: 8742
		private const string _pagePreRenderEventName = "Page_PreRender";

		// Token: 0x04002227 RID: 8743
		private const string _pageSaveStateCompleteEventName = "Page_SaveStateComplete";

		// Token: 0x04002228 RID: 8744
		private const string _pageUnloadEventName = "Page_Unload";

		// Token: 0x04002229 RID: 8745
		private const string _pageErrorEventName = "Page_Error";

		// Token: 0x0400222A RID: 8746
		private const string _pageAbortTransactionEventName = "Page_AbortTransaction";

		// Token: 0x0400222B RID: 8747
		private const string _onTransactionAbortEventName = "OnTransactionAbort";

		// Token: 0x0400222C RID: 8748
		private const string _pageCommitTransactionEventName = "Page_CommitTransaction";

		// Token: 0x0400222D RID: 8749
		private const string _onTransactionCommitEventName = "OnTransactionCommit";

		// Token: 0x0400222E RID: 8750
		private IntPtr _stringResourcePointer;

		// Token: 0x0400222F RID: 8751
		private int _maxResourceOffset;

		// Token: 0x04002230 RID: 8752
		private static object _lockObject = new object();

		// Token: 0x04002231 RID: 8753
		private static Hashtable _eventListCache = new Hashtable();

		// Token: 0x04002232 RID: 8754
		private static object _emptyEventSingleton = new object();

		// Token: 0x04002233 RID: 8755
		private VirtualPath _virtualPath;

		// Token: 0x04002234 RID: 8756
		private IResourceProvider _resourceProvider;

		// Token: 0x04002235 RID: 8757
		private static IDictionary _eventObjects = new Hashtable(16);

		// Token: 0x04002236 RID: 8758
		private BuildResultNoCompileTemplateControl _noCompileBuildResult;

		// Token: 0x04002237 RID: 8759
		private static readonly object EventCommitTransaction = new object();

		// Token: 0x04002238 RID: 8760
		private static readonly object EventAbortTransaction = new object();

		// Token: 0x04002239 RID: 8761
		private static readonly object EventError = new object();

		// Token: 0x020003EB RID: 1003
		internal class SimpleTemplate : ITemplate
		{
			// Token: 0x060030B0 RID: 12464 RVA: 0x000D6361 File Offset: 0x000D5361
			internal SimpleTemplate(ITypedWebObjectFactory objectFactory)
			{
				Util.CheckAssignableType(typeof(UserControl), objectFactory.InstantiatedType);
				this._objectFactory = objectFactory;
			}

			// Token: 0x060030B1 RID: 12465 RVA: 0x000D6388 File Offset: 0x000D5388
			public virtual void InstantiateIn(Control control)
			{
				UserControl userControl = (UserControl)this._objectFactory.CreateInstance();
				userControl.InitializeAsUserControl(control.Page);
				control.Controls.Add(userControl);
			}

			// Token: 0x0400223A RID: 8762
			private IWebObjectFactory _objectFactory;
		}

		// Token: 0x020003EC RID: 1004
		private class EventMethodInfo
		{
			// Token: 0x060030B2 RID: 12466 RVA: 0x000D63BE File Offset: 0x000D53BE
			internal EventMethodInfo(MethodInfo methodInfo, bool isArgless)
			{
				this._isArgless = isArgless;
				this._methodInfo = methodInfo;
			}

			// Token: 0x17000A8E RID: 2702
			// (get) Token: 0x060030B3 RID: 12467 RVA: 0x000D63D4 File Offset: 0x000D53D4
			internal bool IsArgless
			{
				get
				{
					return this._isArgless;
				}
			}

			// Token: 0x17000A8F RID: 2703
			// (get) Token: 0x060030B4 RID: 12468 RVA: 0x000D63DC File Offset: 0x000D53DC
			internal MethodInfo MethodInfo
			{
				get
				{
					return this._methodInfo;
				}
			}

			// Token: 0x0400223B RID: 8763
			private bool _isArgless;

			// Token: 0x0400223C RID: 8764
			private MethodInfo _methodInfo;
		}
	}
}
