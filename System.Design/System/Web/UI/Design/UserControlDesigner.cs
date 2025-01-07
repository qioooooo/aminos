using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class UserControlDesigner : ControlDesigner
	{
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new UserControlDesigner.UserControlDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		public override bool AllowResize
		{
			get
			{
				return false;
			}
		}

		internal override bool ShouldCodeSerializeInternal
		{
			get
			{
				return base.Component.GetType() != typeof(UserControl) && base.ShouldCodeSerializeInternal;
			}
			set
			{
				base.ShouldCodeSerializeInternal = value;
			}
		}

		private string GenerateUserControlCacheKey(string userControlPath, IThemeResolutionService themeService)
		{
			string text = userControlPath;
			if (themeService != null)
			{
				ThemeProvider stylesheetThemeProvider = themeService.GetStylesheetThemeProvider();
				if (stylesheetThemeProvider != null && !string.IsNullOrEmpty(stylesheetThemeProvider.ThemeName))
				{
					text = text + "|" + stylesheetThemeProvider.ThemeName;
				}
			}
			return text;
		}

		private string GenerateUserControlHashCode(string contents, IThemeResolutionService themeService)
		{
			string text = contents.GetHashCode().ToString(CultureInfo.InvariantCulture);
			if (themeService != null)
			{
				ThemeProvider stylesheetThemeProvider = themeService.GetStylesheetThemeProvider();
				if (stylesheetThemeProvider != null)
				{
					text = text + "|" + stylesheetThemeProvider.ContentHashCode.ToString(CultureInfo.InvariantCulture);
				}
			}
			return text;
		}

		private string MakeAppRelativePath(string path)
		{
			if (string.IsNullOrEmpty(path) || path.StartsWith("~", StringComparison.Ordinal))
			{
				return path;
			}
			string text = Path.GetDirectoryName(base.RootDesigner.DocumentUrl);
			if (string.IsNullOrEmpty(text))
			{
				text = "~";
			}
			text = text.Replace('\\', '/');
			text = text.Replace("~", "file://foo");
			path = path.Replace('\\', '/');
			Uri uri = new Uri(text + "/" + path);
			return uri.ToString().Replace("file://foo", "~");
		}

		public override string GetDesignTimeHtml()
		{
			if (base.Component.Site != null)
			{
				IWebApplication webApplication = (IWebApplication)base.Component.Site.GetService(typeof(IWebApplication));
				IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
				if (webApplication != null && designerHost != null && base.RootDesigner.ReferenceManager != null)
				{
					IUserControlDesignerAccessor userControlDesignerAccessor = (IUserControlDesignerAccessor)base.Component;
					string[] array = userControlDesignerAccessor.TagName.Split(new char[] { ':' });
					string text = base.RootDesigner.ReferenceManager.GetUserControlPath(array[0], array[1]);
					text = this.MakeAppRelativePath(text);
					IThemeResolutionService themeResolutionService = (IThemeResolutionService)base.Component.Site.GetService(typeof(IThemeResolutionService));
					string text2 = this.GenerateUserControlCacheKey(text, themeResolutionService);
					if (!string.IsNullOrEmpty(text))
					{
						string text3 = null;
						string text4 = string.Empty;
						bool flag = false;
						IDictionary dictionary = UserControlDesigner._antiRecursionDictionary;
						IDictionaryService dictionaryService = (IDictionaryService)webApplication.GetService(typeof(IDictionaryService));
						if (dictionaryService != null)
						{
							dictionary = (IDictionary)dictionaryService.GetValue("__aspnetUserControlCache");
							if (dictionary == null)
							{
								dictionary = new HybridDictionary();
								dictionaryService.SetValue("__aspnetUserControlCache", dictionary);
							}
							Pair pair = (Pair)dictionary[text2];
							if (pair != null)
							{
								text3 = (string)pair.First;
								text4 = (string)pair.Second;
								flag = text4.Contains("mvwres:");
							}
							else
							{
								flag = true;
							}
						}
						IDocumentProjectItem documentProjectItem = webApplication.GetProjectItemFromUrl(text) as IDocumentProjectItem;
						if (documentProjectItem != null)
						{
							this._userControlFound = true;
							StreamReader streamReader = new StreamReader(documentProjectItem.GetContents());
							string text5 = streamReader.ReadToEnd();
							string text6 = null;
							if (!flag)
							{
								text6 = this.GenerateUserControlHashCode(text5, themeResolutionService);
								flag = !string.Equals(text6, text3, StringComparison.OrdinalIgnoreCase) || text5.Contains(".ascx");
							}
							if (!flag)
							{
								goto IL_0580;
							}
							if (UserControlDesigner._antiRecursionDictionary.Contains(text2))
							{
								return base.CreateErrorDesignTimeHtml(SR.GetString("UserControlDesigner_CyclicError"));
							}
							UserControlDesigner._antiRecursionDictionary[text2] = base.CreateErrorDesignTimeHtml(SR.GetString("UserControlDesigner_CyclicError"));
							text4 = string.Empty;
							Pair pair2 = new Pair();
							if (text6 == null)
							{
								text6 = this.GenerateUserControlHashCode(text5, themeResolutionService);
							}
							pair2.First = text6;
							pair2.Second = text4;
							dictionary[text2] = pair2;
							UserControl userControl = (UserControl)base.Component;
							Page page = new Page();
							try
							{
								try
								{
									page.Controls.Add(userControl);
									IDesignerHost designerHost2 = new UserControlDesigner.UserControlDesignerHost(designerHost, page, text);
									if (!string.IsNullOrEmpty(text5))
									{
										List<Triplet> list = new List<Triplet>();
										Control[] array2 = ControlSerializer.DeserializeControlsInternal(text5, designerHost2, list);
										foreach (Control control in array2)
										{
											if (!(control is LiteralControl) && !(control is DesignerDataBoundLiteralControl) && !(control is DataBoundLiteralControl))
											{
												if (string.IsNullOrEmpty(control.ID))
												{
													throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, SR.GetString("UserControlDesigner_MissingID"), new object[] { control.GetType().Name }));
												}
												designerHost2.Container.Add(control);
											}
											userControl.Controls.Add(control);
										}
										foreach (Triplet triplet in list)
										{
											string text7 = (string)triplet.First;
											Pair pair3 = (Pair)triplet.Second;
											Pair pair4 = (Pair)triplet.Third;
											if (pair3 != null)
											{
												string text8 = (string)pair3.First;
												string text9 = (string)pair3.Second;
												((UserControlDesigner.UserControlDesignerHost)designerHost2).RegisterUserControl(text7, text8, text9);
											}
											else if (pair4 != null)
											{
												string text10 = (string)pair4.First;
												string text11 = (string)pair4.Second;
												((UserControlDesigner.UserControlDesignerHost)designerHost2).RegisterTagNamespace(text7, text10, text11);
											}
										}
										StringBuilder stringBuilder = new StringBuilder();
										foreach (Control control2 in array2)
										{
											string empty = string.Empty;
											if (control2 is LiteralControl)
											{
												stringBuilder.Append(((LiteralControl)control2).Text);
											}
											else if (control2 is DesignerDataBoundLiteralControl)
											{
												stringBuilder.Append(((DesignerDataBoundLiteralControl)control2).Text);
											}
											else if (control2 is DataBoundLiteralControl)
											{
												stringBuilder.Append(((DataBoundLiteralControl)control2).Text);
											}
											else if (control2 is HtmlControl)
											{
												StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
												DesignTimeHtmlTextWriter designTimeHtmlTextWriter = new DesignTimeHtmlTextWriter(stringWriter);
												control2.RenderControl(designTimeHtmlTextWriter);
												stringBuilder.Append(stringWriter.GetStringBuilder().ToString());
											}
											else
											{
												ControlDesigner controlDesigner = (ControlDesigner)designerHost2.GetDesigner(control2);
												ViewRendering viewRendering = controlDesigner.GetViewRendering();
												stringBuilder.Append(viewRendering.Content);
											}
										}
										text4 = stringBuilder.ToString();
									}
									pair2.Second = text4;
								}
								catch
								{
									dictionary.Remove(text2);
									throw;
								}
								goto IL_0580;
							}
							finally
							{
								UserControlDesigner._antiRecursionDictionary.Remove(text2);
								userControl.Controls.Clear();
								page.Controls.Remove(userControl);
							}
						}
						text4 = base.CreateErrorDesignTimeHtml(SR.GetString("UserControlDesigner_NotFound", new object[] { text }));
						IL_0580:
						if (text4.Trim().Length > 0)
						{
							return text4;
						}
					}
				}
			}
			return base.CreatePlaceHolderDesignTimeHtml();
		}

		private void EditUserControl()
		{
			IWebApplication webApplication = (IWebApplication)base.Component.Site.GetService(typeof(IWebApplication));
			if (webApplication != null)
			{
				IUserControlDesignerAccessor userControlDesignerAccessor = (IUserControlDesignerAccessor)base.Component;
				string[] array = userControlDesignerAccessor.TagName.Split(new char[] { ':' });
				string text = base.RootDesigner.ReferenceManager.GetUserControlPath(array[0], array[1]);
				if (!string.IsNullOrEmpty(text))
				{
					text = this.MakeAppRelativePath(text);
					IDocumentProjectItem documentProjectItem = webApplication.GetProjectItemFromUrl(text) as IDocumentProjectItem;
					if (documentProjectItem != null)
					{
						documentProjectItem.Open();
					}
				}
			}
		}

		private void Refresh()
		{
			this.UpdateDesignTimeHtml();
		}

		internal override string GetPersistInnerHtmlInternal()
		{
			if (base.Component.GetType() == typeof(UserControl))
			{
				return null;
			}
			return base.GetPersistInnerHtmlInternal();
		}

		private const string UserControlCacheKey = "__aspnetUserControlCache";

		private const string _dummyProtocolAndServer = "file://foo";

		private static IDictionary _antiRecursionDictionary = new HybridDictionary();

		private bool _userControlFound;

		private class UserControlDesignerActionList : DesignerActionList
		{
			public UserControlDesignerActionList(UserControlDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
			}

			public override bool AutoShow
			{
				get
				{
					return true;
				}
				set
				{
				}
			}

			public void EditUserControl()
			{
				this._parent.EditUserControl();
			}

			public void Refresh()
			{
				this._parent.Refresh();
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				if (this._parent._userControlFound)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "EditUserControl", SR.GetString("UserControlDesigner_EditUserControl"), string.Empty, string.Empty, true));
				}
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "Refresh", SR.GetString("UserControlDesigner_Refresh"), string.Empty, string.Empty, true));
				return designerActionItemCollection;
			}

			private UserControlDesigner _parent;
		}

		private sealed class TagNamespaceRegisterEntry
		{
			public TagNamespaceRegisterEntry(string tagPrefix, string tagNamespace, string assemblyName)
			{
				this.TagPrefix = tagPrefix;
				this.TagNamespace = tagNamespace;
				this.AssemblyName = assemblyName;
			}

			public string TagPrefix;

			public string TagNamespace;

			public string AssemblyName;
		}

		private sealed class UserControlDesignerHost : IContainer, IDesignerHost, IServiceContainer, IServiceProvider, IDisposable, IUrlResolutionService
		{
			public UserControlDesignerHost(IDesignerHost host, IComponent rootComponent, string userControlPath)
			{
				this._host = host;
				this._componentTable = new Hashtable();
				this._designerTable = new Hashtable();
				this._rootComponent = rootComponent;
				this._userControlPath = userControlPath;
				this._rootComponent.Site = new UserControlDesigner.DummySite(this._rootComponent, this);
			}

			~UserControlDesignerHost()
			{
				this.Dispose(false);
			}

			private Hashtable ComponentTable
			{
				get
				{
					return this._componentTable;
				}
			}

			private Hashtable DesignerTable
			{
				get
				{
					return this._designerTable;
				}
			}

			public void ClearComponents()
			{
				for (int i = 0; i < this.DesignerTable.Count; i++)
				{
					if (this.DesignerTable[i] != null)
					{
						IDesigner designer = (IDesigner)this.DesignerTable[i];
						try
						{
							designer.Dispose();
						}
						catch
						{
						}
					}
				}
				this.DesignerTable.Clear();
				for (int j = 0; j < this.ComponentTable.Count; j++)
				{
					if (this.ComponentTable[j] != null)
					{
						IComponent component = (IComponent)this.ComponentTable[j];
						ISite site = component.Site;
						try
						{
							component.Dispose();
						}
						catch
						{
						}
						if (component.Site != null)
						{
							((IContainer)this).Remove(component);
						}
					}
				}
				this.ComponentTable.Clear();
			}

			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			public void Dispose(bool disposing)
			{
				if (!this._disposed && disposing)
				{
					this.ClearComponents();
					this._host = null;
					this._componentTable = null;
					this._designerTable = null;
				}
				this._disposed = true;
			}

			private IComponent[] GetComponents()
			{
				int count = this.ComponentTable.Count;
				IComponent[] array = new IComponent[count];
				if (count != 0)
				{
					int num = 0;
					foreach (object obj in this.ComponentTable.Values)
					{
						IComponent component = (IComponent)obj;
						array[num++] = component;
					}
				}
				return array;
			}

			public void RegisterTagNamespace(string tagPrefix, string tagNamespace, string assemblyName)
			{
				if (this._tagNamespaceRegisterEntries == null)
				{
					this._tagNamespaceRegisterEntries = new List<UserControlDesigner.TagNamespaceRegisterEntry>();
				}
				this._tagNamespaceRegisterEntries.Add(new UserControlDesigner.TagNamespaceRegisterEntry(tagPrefix, tagNamespace, assemblyName));
			}

			public void RegisterUserControl(string tagPrefix, string tagName, string src)
			{
				if (this._userControlRegisterEntries == null)
				{
					this._userControlRegisterEntries = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				}
				this._userControlRegisterEntries[tagPrefix + ":" + tagName] = src;
			}

			ComponentCollection IContainer.Components
			{
				get
				{
					return new ComponentCollection(this.GetComponents());
				}
			}

			void IContainer.Add(IComponent component)
			{
				((IContainer)this).Add(component, null);
			}

			void IContainer.Add(IComponent component, string name)
			{
				if (component == null)
				{
					throw new ArgumentNullException("component");
				}
				if (component.Site == null)
				{
					component.Site = new UserControlDesigner.DummySite(component, this);
					if (component is Control)
					{
						component.Site.Name = ((Control)component).ID;
					}
					else
					{
						component.Site.Name = "Temp" + this._nameCounter++;
					}
				}
				if (name == null)
				{
					name = component.Site.Name;
				}
				if (this.ComponentTable[name] != null)
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, SR.GetString("UserControlDesignerHost_ComponentAlreadyExists"), new object[] { name }));
				}
				this.ComponentTable[name] = component;
				IDesigner designer = TypeDescriptor.CreateDesigner(component, typeof(IDesigner));
				designer.Initialize(component);
				this.DesignerTable[component] = designer;
				if (component is Control)
				{
					((Control)component).Page = (Page)this._rootComponent;
				}
			}

			void IContainer.Remove(IComponent component)
			{
				if (component == null)
				{
					throw new ArgumentNullException("component");
				}
				if (component.Site == null)
				{
					return;
				}
				string name = component.Site.Name;
				if (name != null && this.ComponentTable[name] == component)
				{
					if (this.DesignerTable != null)
					{
						IDesigner designer = (IDesigner)this.DesignerTable[component];
						if (designer != null)
						{
							this.DesignerTable.Remove(component);
							designer.Dispose();
						}
					}
					this.ComponentTable.Remove(name);
					component.Dispose();
					component.Site = null;
				}
			}

			void IDisposable.Dispose()
			{
				this.Dispose();
			}

			object IServiceProvider.GetService(Type serviceType)
			{
				if (serviceType == typeof(IDesignerHost) || serviceType == typeof(IContainer) || serviceType == typeof(IUrlResolutionService))
				{
					return this;
				}
				return this._host.GetService(serviceType);
			}

			void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
			{
			}

			void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback)
			{
			}

			void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote)
			{
			}

			void IServiceContainer.AddService(Type serviceType, object serviceInstance)
			{
			}

			void IServiceContainer.RemoveService(Type serviceType, bool promote)
			{
			}

			void IServiceContainer.RemoveService(Type serviceType)
			{
			}

			IContainer IDesignerHost.Container
			{
				get
				{
					return this;
				}
			}

			bool IDesignerHost.InTransaction
			{
				get
				{
					return this._host.InTransaction;
				}
			}

			bool IDesignerHost.Loading
			{
				get
				{
					return this._host.Loading;
				}
			}

			string IDesignerHost.TransactionDescription
			{
				get
				{
					return this._host.TransactionDescription;
				}
			}

			IComponent IDesignerHost.RootComponent
			{
				get
				{
					return this._rootComponent;
				}
			}

			string IDesignerHost.RootComponentClassName
			{
				get
				{
					return this._rootComponent.GetType().Name;
				}
			}

			event EventHandler IDesignerHost.Activated
			{
				add
				{
				}
				remove
				{
				}
			}

			event EventHandler IDesignerHost.Deactivated
			{
				add
				{
				}
				remove
				{
				}
			}

			event EventHandler IDesignerHost.LoadComplete
			{
				add
				{
					this._host.LoadComplete += value;
				}
				remove
				{
					this._host.LoadComplete -= value;
				}
			}

			event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosed
			{
				add
				{
				}
				remove
				{
				}
			}

			event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosing
			{
				add
				{
				}
				remove
				{
				}
			}

			event EventHandler IDesignerHost.TransactionOpened
			{
				add
				{
				}
				remove
				{
				}
			}

			event EventHandler IDesignerHost.TransactionOpening
			{
				add
				{
				}
				remove
				{
				}
			}

			void IDesignerHost.Activate()
			{
			}

			IComponent IDesignerHost.CreateComponent(Type componentType)
			{
				return null;
			}

			IComponent IDesignerHost.CreateComponent(Type componentType, string name)
			{
				return null;
			}

			DesignerTransaction IDesignerHost.CreateTransaction()
			{
				return this._host.CreateTransaction();
			}

			DesignerTransaction IDesignerHost.CreateTransaction(string description)
			{
				return this._host.CreateTransaction(description);
			}

			void IDesignerHost.DestroyComponent(IComponent component)
			{
				((IContainer)this).Remove(component);
			}

			Type IDesignerHost.GetType(string typeName)
			{
				return this._host.GetType(typeName);
			}

			IDesigner IDesignerHost.GetDesigner(IComponent component)
			{
				IDesigner designer;
				if (component == this._host.RootComponent)
				{
					designer = this._host.GetDesigner(component);
				}
				else if (component == this._rootComponent)
				{
					designer = new UserControlDesigner.DummyRootDesigner((WebFormsRootDesigner)this._host.GetDesigner(this._host.RootComponent), this._userControlRegisterEntries, this._tagNamespaceRegisterEntries, this._userControlPath);
				}
				else
				{
					designer = (IDesigner)this.DesignerTable[component];
				}
				return designer;
			}

			string IUrlResolutionService.ResolveClientUrl(string relativeUrl)
			{
				if (relativeUrl == null)
				{
					throw new ArgumentNullException("relativeUrl");
				}
				if (UserControlDesigner.UserControlDesignerHost.IsRooted(relativeUrl) || relativeUrl.Contains("mvwres:"))
				{
					return relativeUrl;
				}
				IUrlResolutionService urlResolutionService = (IUrlResolutionService)this._host.GetService(typeof(IUrlResolutionService));
				if (urlResolutionService != null)
				{
					if (UserControlDesigner.UserControlDesignerHost.IsAppRelativePath(relativeUrl))
					{
						relativeUrl = urlResolutionService.ResolveClientUrl(relativeUrl);
					}
					else
					{
						string text = this._userControlPath;
						if (text != null && text.Length != 0)
						{
							if (UserControlDesigner.UserControlDesignerHost.IsAppRelativePath(text))
							{
								text = text.Replace("~", "file://foo");
								Uri uri = new Uri(text);
								string[] segments = uri.Segments;
								StringBuilder stringBuilder = new StringBuilder("~");
								for (int i = 0; i < segments.Length - 1; i++)
								{
									stringBuilder.Append(segments[i]);
								}
								relativeUrl = urlResolutionService.ResolveClientUrl(stringBuilder.ToString() + relativeUrl);
							}
							else
							{
								string fileName = Path.GetFileName(text);
								int num = text.LastIndexOf(fileName, StringComparison.Ordinal);
								relativeUrl = Path.Combine(text.Substring(0, num), relativeUrl);
							}
						}
					}
				}
				return relativeUrl;
			}

			private static bool IsRooted(string basepath)
			{
				return basepath == null || basepath.Length == 0 || basepath[0] == '/' || basepath[0] == '\\';
			}

			private static bool IsAppRelativePath(string path)
			{
				return path.Length >= 2 && path[0] == '~' && (path[1] == '/' || path[1] == '\\');
			}

			private const string dummyProtocolAndServer = "file://foo";

			private const char appRelativeCharacter = '~';

			private Hashtable _componentTable;

			private Hashtable _designerTable;

			private IDesignerHost _host;

			private bool _disposed;

			private IComponent _rootComponent;

			private int _nameCounter;

			private string _userControlPath;

			private IDictionary<string, string> _userControlRegisterEntries;

			private IList<UserControlDesigner.TagNamespaceRegisterEntry> _tagNamespaceRegisterEntries;
		}

		private sealed class DummyRootDesigner : WebFormsRootDesigner
		{
			public DummyRootDesigner(WebFormsRootDesigner rootDesigner, IDictionary<string, string> userControlRegisterEntries, IList<UserControlDesigner.TagNamespaceRegisterEntry> tagNamespaceRegisterEntries, string documentUrl)
			{
				this._rootDesigner = rootDesigner;
				this._userControlRegisterEntries = userControlRegisterEntries;
				this._tagNamespaceRegisterEntries = tagNamespaceRegisterEntries;
				this._documentUrl = documentUrl;
			}

			public override string DocumentUrl
			{
				get
				{
					return this._documentUrl;
				}
			}

			public override bool IsLoading
			{
				get
				{
					return this._rootDesigner.IsLoading;
				}
			}

			public override bool IsDesignerViewLocked
			{
				get
				{
					return true;
				}
			}

			public override WebFormsReferenceManager ReferenceManager
			{
				get
				{
					return new UserControlDesigner.DummyRootDesigner.DummyWebFormsReferenceManager(this, this._rootDesigner.ReferenceManager, this._userControlRegisterEntries, this._tagNamespaceRegisterEntries);
				}
			}

			internal IWebApplication WebApplication
			{
				get
				{
					if (this._rootDesigner != null)
					{
						return (IWebApplication)this._rootDesigner.GetService(typeof(IWebApplication));
					}
					return null;
				}
			}

			public override void AddClientScriptToDocument(ClientScriptItem scriptItem)
			{
				throw new NotSupportedException();
			}

			public override string AddControlToDocument(Control newControl, Control referenceControl, ControlLocation location)
			{
				throw new NotSupportedException();
			}

			public override ClientScriptItemCollection GetClientScriptsInDocument()
			{
				throw new NotSupportedException();
			}

			protected internal override void GetControlViewAndTag(Control control, out IControlDesignerView view, out IControlDesignerTag tag)
			{
				view = null;
				tag = null;
			}

			public override void RemoveClientScriptFromDocument(string clientScriptId)
			{
				throw new NotSupportedException();
			}

			public override void RemoveControlFromDocument(Control control)
			{
				throw new NotSupportedException();
			}

			internal WebFormsRootDesigner _rootDesigner;

			private IDictionary<string, string> _userControlRegisterEntries;

			private IList<UserControlDesigner.TagNamespaceRegisterEntry> _tagNamespaceRegisterEntries;

			private string _documentUrl;

			private sealed class DummyWebFormsReferenceManager : WebFormsReferenceManager
			{
				public DummyWebFormsReferenceManager(UserControlDesigner.DummyRootDesigner owner, WebFormsReferenceManager baseReferenceManager, IDictionary<string, string> baseUserControlRegisterEntries, IList<UserControlDesigner.TagNamespaceRegisterEntry> tagNamespaceRegisterEntries)
				{
					this._owner = owner;
					this._baseReferenceManager = baseReferenceManager;
					this._baseUserControlRegisterEntries = baseUserControlRegisterEntries;
					this._tagNamespaceRegisterEntries = tagNamespaceRegisterEntries;
				}

				private bool GetNamespaceAndAssemblyFromType(Type objectType, out string ns, out string asmName)
				{
					if (objectType != null)
					{
						Assembly assembly = objectType.Module.Assembly;
						if (assembly.GlobalAssemblyCache)
						{
							asmName = assembly.FullName;
						}
						else
						{
							asmName = assembly.GetName().Name;
						}
						ns = objectType.Namespace;
						if (ns == null)
						{
							ns = string.Empty;
						}
						ns = ns.TrimEnd(new char[] { '.' });
						if (ns != null && asmName != null && asmName.Length > 0)
						{
							return true;
						}
					}
					ns = null;
					asmName = null;
					return false;
				}

				public override Type GetType(string tagPrefix, string tagName)
				{
					return this._baseReferenceManager.GetType(tagPrefix, tagName);
				}

				public override string GetTagPrefix(Type objectType)
				{
					string text;
					string text2;
					if (this.GetNamespaceAndAssemblyFromType(objectType, out text, out text2))
					{
						string text3 = null;
						string text4 = null;
						if (text != null && text2 != null)
						{
							foreach (UserControlDesigner.TagNamespaceRegisterEntry tagNamespaceRegisterEntry in this._tagNamespaceRegisterEntries)
							{
								if (string.Equals(text, tagNamespaceRegisterEntry.TagNamespace, StringComparison.OrdinalIgnoreCase))
								{
									string assemblyName = tagNamespaceRegisterEntry.AssemblyName;
									if (!string.IsNullOrEmpty(assemblyName))
									{
										if (string.Equals(text2, assemblyName, StringComparison.OrdinalIgnoreCase))
										{
											text3 = tagNamespaceRegisterEntry.TagPrefix;
											break;
										}
									}
									else if (text4 == null)
									{
										text4 = tagNamespaceRegisterEntry.TagPrefix;
									}
								}
							}
							if (text3 == null)
							{
								if (text4 != null)
								{
									text3 = text4;
								}
								else
								{
									text3 = string.Empty;
								}
							}
							return text3;
						}
					}
					return this._baseReferenceManager.GetTagPrefix(objectType);
				}

				public override string RegisterTagPrefix(Type objectType)
				{
					throw new NotSupportedException();
				}

				private static bool IsRooted(string basepath)
				{
					return basepath == null || basepath.Length == 0 || basepath[0] == '/' || basepath[0] == '\\' || Path.IsPathRooted(basepath) || basepath.IndexOf(Path.VolumeSeparatorChar) >= 0;
				}

				private static bool IsAppRelativePath(string path)
				{
					return path.Length >= 2 && path[0] == '~' && (path[1] == '/' || path[1] == '\\');
				}

				private static string ResolveFileUrl(string baseURL, string relativeFileUrl)
				{
					if (!UserControlDesigner.DummyRootDesigner.DummyWebFormsReferenceManager.IsRooted(relativeFileUrl) && !UserControlDesigner.DummyRootDesigner.DummyWebFormsReferenceManager.IsAppRelativePath(relativeFileUrl))
					{
						string fileName = Path.GetFileName(baseURL);
						int num = baseURL.LastIndexOf(fileName, StringComparison.Ordinal);
						string text = baseURL.Substring(0, num);
						relativeFileUrl = Path.Combine(text, relativeFileUrl);
					}
					return relativeFileUrl;
				}

				public override ICollection GetRegisterDirectives()
				{
					if (this._registerDirectives == null)
					{
						try
						{
							this._registerDirectives = new Collection<string>();
							IWebApplication webApplication = this._owner.WebApplication;
							if (webApplication != null)
							{
								Configuration configuration = webApplication.OpenWebConfiguration(true);
								if (configuration != null)
								{
									PagesSection pagesSection = (PagesSection)configuration.GetSection("system.web/pages");
									if (pagesSection != null)
									{
										string filePath = configuration.FilePath;
										IProjectItem rootProjectItem = webApplication.RootProjectItem;
										string physicalPath = rootProjectItem.PhysicalPath;
										string text = "~/" + filePath.Substring(physicalPath.Length, filePath.Length - physicalPath.Length);
										foreach (object obj in pagesSection.Controls)
										{
											TagPrefixInfo tagPrefixInfo = (TagPrefixInfo)obj;
											Dictionary<string, string> dictionary = new Dictionary<string, string>();
											tagPrefixInfo.Source = UserControlDesigner.DummyRootDesigner.DummyWebFormsReferenceManager.ResolveFileUrl(text, tagPrefixInfo.Source);
											ElementInformation elementInformation = tagPrefixInfo.ElementInformation;
											foreach (object obj2 in elementInformation.Properties)
											{
												PropertyInformation propertyInformation = (PropertyInformation)obj2;
												if (propertyInformation.Type == typeof(string))
												{
													dictionary[propertyInformation.Name] = ((propertyInformation.ValueOrigin != PropertyValueOrigin.Default) ? ((string)propertyInformation.Value) : null);
												}
											}
											this._registerDirectives.Add(this.GenerateRegisterDirective(dictionary["tagPrefix"], dictionary["tagName"], dictionary["namespace"], dictionary["assembly"], dictionary["src"]));
										}
									}
								}
							}
						}
						catch (Exception)
						{
						}
						if (this._baseUserControlRegisterEntries != null)
						{
							foreach (KeyValuePair<string, string> keyValuePair in this._baseUserControlRegisterEntries)
							{
								string text2 = this.GenerateRegisterDirective(keyValuePair.Key, keyValuePair.Value);
								if (!this._registerDirectives.Contains(text2))
								{
									this._registerDirectives.Add(text2);
								}
							}
						}
						if (this._tagNamespaceRegisterEntries != null)
						{
							foreach (UserControlDesigner.TagNamespaceRegisterEntry tagNamespaceRegisterEntry in this._tagNamespaceRegisterEntries)
							{
								string text3 = this.GenerateRegisterDirective(tagNamespaceRegisterEntry.TagPrefix, null, tagNamespaceRegisterEntry.TagNamespace, tagNamespaceRegisterEntry.AssemblyName, null);
								if (!this._registerDirectives.Contains(text3))
								{
									this._registerDirectives.Add(text3);
								}
							}
						}
					}
					return this._registerDirectives;
				}

				public override string GetUserControlPath(string tagPrefix, string tagName)
				{
					return this._owner._userControlRegisterEntries[tagPrefix + ":" + tagName];
				}

				private string GenerateRegisterDirective(string tagPrefix, string tagName, string ns, string assembly, string src)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("<%@ Register");
					if (tagPrefix != null && tagPrefix.Length > 0)
					{
						stringBuilder.Append(" TagPrefix=\"");
						stringBuilder.Append(tagPrefix);
						stringBuilder.Append("\"");
					}
					if (!string.IsNullOrEmpty(tagName))
					{
						stringBuilder.Append(" TagName=\"");
						stringBuilder.Append(tagName);
						stringBuilder.Append("\"");
					}
					if (ns != null)
					{
						stringBuilder.Append(" Namespace=\"");
						stringBuilder.Append(ns);
						stringBuilder.Append("\"");
					}
					if (!string.IsNullOrEmpty(assembly))
					{
						stringBuilder.Append(" Assembly=\"");
						stringBuilder.Append(assembly);
						stringBuilder.Append("\"");
					}
					if (!string.IsNullOrEmpty(src))
					{
						stringBuilder.Append(" Src=\"");
						stringBuilder.Append(src);
						stringBuilder.Append("\"");
					}
					stringBuilder.Append("%>");
					return stringBuilder.ToString();
				}

				private string GenerateRegisterDirective(string tagPrefixAndName, string src)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("<%@ Register");
					if (!string.IsNullOrEmpty(tagPrefixAndName))
					{
						string[] array = tagPrefixAndName.Split(new char[] { ':' });
						if (array.Length == 2)
						{
							stringBuilder.Append(" TagPrefix=\"");
							stringBuilder.Append(array[0]);
							stringBuilder.Append("\"");
							stringBuilder.Append(" TagName=\"");
							stringBuilder.Append(array[1]);
							stringBuilder.Append("\"");
						}
					}
					if (!string.IsNullOrEmpty(src))
					{
						stringBuilder.Append(" Src=\"");
						stringBuilder.Append(src);
						stringBuilder.Append("\"");
					}
					stringBuilder.Append("%>");
					return stringBuilder.ToString();
				}

				private UserControlDesigner.DummyRootDesigner _owner;

				private WebFormsReferenceManager _baseReferenceManager;

				private Collection<string> _registerDirectives;

				private IDictionary<string, string> _baseUserControlRegisterEntries;

				private IList<UserControlDesigner.TagNamespaceRegisterEntry> _tagNamespaceRegisterEntries;
			}
		}

		private sealed class DummySite : ISite, IServiceProvider
		{
			public DummySite(IComponent component, UserControlDesigner.UserControlDesignerHost designerHost)
			{
				this._component = component;
				this._container = designerHost;
				this._designerHost = designerHost;
			}

			IComponent ISite.Component
			{
				get
				{
					return this._component;
				}
			}

			IContainer ISite.Container
			{
				get
				{
					return this._container;
				}
			}

			bool ISite.DesignMode
			{
				get
				{
					return true;
				}
			}

			string ISite.Name
			{
				get
				{
					return this._name;
				}
				set
				{
					this._name = value;
				}
			}

			object IServiceProvider.GetService(Type type)
			{
				return this._designerHost.GetService(type);
			}

			private IComponent _component;

			private IDesignerHost _designerHost;

			private IContainer _container;

			private string _name;
		}
	}
}
