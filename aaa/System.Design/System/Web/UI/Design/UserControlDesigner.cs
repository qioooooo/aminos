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
	// Token: 0x020003A7 RID: 935
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class UserControlDesigner : ControlDesigner
	{
		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600226A RID: 8810 RVA: 0x000BC1EC File Offset: 0x000BB1EC
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

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x0600226B RID: 8811 RVA: 0x000BC219 File Offset: 0x000BB219
		public override bool AllowResize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600226C RID: 8812 RVA: 0x000BC21C File Offset: 0x000BB21C
		// (set) Token: 0x0600226D RID: 8813 RVA: 0x000BC23D File Offset: 0x000BB23D
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

		// Token: 0x0600226E RID: 8814 RVA: 0x000BC248 File Offset: 0x000BB248
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

		// Token: 0x0600226F RID: 8815 RVA: 0x000BC284 File Offset: 0x000BB284
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

		// Token: 0x06002270 RID: 8816 RVA: 0x000BC2D4 File Offset: 0x000BB2D4
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

		// Token: 0x06002271 RID: 8817 RVA: 0x000BC368 File Offset: 0x000BB368
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

		// Token: 0x06002272 RID: 8818 RVA: 0x000BC95C File Offset: 0x000BB95C
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

		// Token: 0x06002273 RID: 8819 RVA: 0x000BC9F4 File Offset: 0x000BB9F4
		private void Refresh()
		{
			this.UpdateDesignTimeHtml();
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000BC9FC File Offset: 0x000BB9FC
		internal override string GetPersistInnerHtmlInternal()
		{
			if (base.Component.GetType() == typeof(UserControl))
			{
				return null;
			}
			return base.GetPersistInnerHtmlInternal();
		}

		// Token: 0x04001854 RID: 6228
		private const string UserControlCacheKey = "__aspnetUserControlCache";

		// Token: 0x04001855 RID: 6229
		private const string _dummyProtocolAndServer = "file://foo";

		// Token: 0x04001856 RID: 6230
		private static IDictionary _antiRecursionDictionary = new HybridDictionary();

		// Token: 0x04001857 RID: 6231
		private bool _userControlFound;

		// Token: 0x020003A8 RID: 936
		private class UserControlDesignerActionList : DesignerActionList
		{
			// Token: 0x06002276 RID: 8822 RVA: 0x000BCA29 File Offset: 0x000BBA29
			public UserControlDesignerActionList(UserControlDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
			}

			// Token: 0x1700065C RID: 1628
			// (get) Token: 0x06002277 RID: 8823 RVA: 0x000BCA3E File Offset: 0x000BBA3E
			// (set) Token: 0x06002278 RID: 8824 RVA: 0x000BCA41 File Offset: 0x000BBA41
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

			// Token: 0x06002279 RID: 8825 RVA: 0x000BCA43 File Offset: 0x000BBA43
			public void EditUserControl()
			{
				this._parent.EditUserControl();
			}

			// Token: 0x0600227A RID: 8826 RVA: 0x000BCA50 File Offset: 0x000BBA50
			public void Refresh()
			{
				this._parent.Refresh();
			}

			// Token: 0x0600227B RID: 8827 RVA: 0x000BCA60 File Offset: 0x000BBA60
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

			// Token: 0x04001858 RID: 6232
			private UserControlDesigner _parent;
		}

		// Token: 0x020003A9 RID: 937
		private sealed class TagNamespaceRegisterEntry
		{
			// Token: 0x0600227C RID: 8828 RVA: 0x000BCACF File Offset: 0x000BBACF
			public TagNamespaceRegisterEntry(string tagPrefix, string tagNamespace, string assemblyName)
			{
				this.TagPrefix = tagPrefix;
				this.TagNamespace = tagNamespace;
				this.AssemblyName = assemblyName;
			}

			// Token: 0x04001859 RID: 6233
			public string TagPrefix;

			// Token: 0x0400185A RID: 6234
			public string TagNamespace;

			// Token: 0x0400185B RID: 6235
			public string AssemblyName;
		}

		// Token: 0x020003AA RID: 938
		private sealed class UserControlDesignerHost : IContainer, IDesignerHost, IServiceContainer, IServiceProvider, IDisposable, IUrlResolutionService
		{
			// Token: 0x0600227D RID: 8829 RVA: 0x000BCAEC File Offset: 0x000BBAEC
			public UserControlDesignerHost(IDesignerHost host, IComponent rootComponent, string userControlPath)
			{
				this._host = host;
				this._componentTable = new Hashtable();
				this._designerTable = new Hashtable();
				this._rootComponent = rootComponent;
				this._userControlPath = userControlPath;
				this._rootComponent.Site = new UserControlDesigner.DummySite(this._rootComponent, this);
			}

			// Token: 0x0600227E RID: 8830 RVA: 0x000BCB44 File Offset: 0x000BBB44
			~UserControlDesignerHost()
			{
				this.Dispose(false);
			}

			// Token: 0x1700065D RID: 1629
			// (get) Token: 0x0600227F RID: 8831 RVA: 0x000BCB74 File Offset: 0x000BBB74
			private Hashtable ComponentTable
			{
				get
				{
					return this._componentTable;
				}
			}

			// Token: 0x1700065E RID: 1630
			// (get) Token: 0x06002280 RID: 8832 RVA: 0x000BCB7C File Offset: 0x000BBB7C
			private Hashtable DesignerTable
			{
				get
				{
					return this._designerTable;
				}
			}

			// Token: 0x06002281 RID: 8833 RVA: 0x000BCB84 File Offset: 0x000BBB84
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

			// Token: 0x06002282 RID: 8834 RVA: 0x000BCC70 File Offset: 0x000BBC70
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06002283 RID: 8835 RVA: 0x000BCC7F File Offset: 0x000BBC7F
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

			// Token: 0x06002284 RID: 8836 RVA: 0x000BCCB0 File Offset: 0x000BBCB0
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

			// Token: 0x06002285 RID: 8837 RVA: 0x000BCD30 File Offset: 0x000BBD30
			public void RegisterTagNamespace(string tagPrefix, string tagNamespace, string assemblyName)
			{
				if (this._tagNamespaceRegisterEntries == null)
				{
					this._tagNamespaceRegisterEntries = new List<UserControlDesigner.TagNamespaceRegisterEntry>();
				}
				this._tagNamespaceRegisterEntries.Add(new UserControlDesigner.TagNamespaceRegisterEntry(tagPrefix, tagNamespace, assemblyName));
			}

			// Token: 0x06002286 RID: 8838 RVA: 0x000BCD58 File Offset: 0x000BBD58
			public void RegisterUserControl(string tagPrefix, string tagName, string src)
			{
				if (this._userControlRegisterEntries == null)
				{
					this._userControlRegisterEntries = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				}
				this._userControlRegisterEntries[tagPrefix + ":" + tagName] = src;
			}

			// Token: 0x1700065F RID: 1631
			// (get) Token: 0x06002287 RID: 8839 RVA: 0x000BCD8A File Offset: 0x000BBD8A
			ComponentCollection IContainer.Components
			{
				get
				{
					return new ComponentCollection(this.GetComponents());
				}
			}

			// Token: 0x06002288 RID: 8840 RVA: 0x000BCD97 File Offset: 0x000BBD97
			void IContainer.Add(IComponent component)
			{
				((IContainer)this).Add(component, null);
			}

			// Token: 0x06002289 RID: 8841 RVA: 0x000BCDA4 File Offset: 0x000BBDA4
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

			// Token: 0x0600228A RID: 8842 RVA: 0x000BCEB4 File Offset: 0x000BBEB4
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

			// Token: 0x0600228B RID: 8843 RVA: 0x000BCF3E File Offset: 0x000BBF3E
			void IDisposable.Dispose()
			{
				this.Dispose();
			}

			// Token: 0x0600228C RID: 8844 RVA: 0x000BCF46 File Offset: 0x000BBF46
			object IServiceProvider.GetService(Type serviceType)
			{
				if (serviceType == typeof(IDesignerHost) || serviceType == typeof(IContainer) || serviceType == typeof(IUrlResolutionService))
				{
					return this;
				}
				return this._host.GetService(serviceType);
			}

			// Token: 0x0600228D RID: 8845 RVA: 0x000BCF7D File Offset: 0x000BBF7D
			void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
			{
			}

			// Token: 0x0600228E RID: 8846 RVA: 0x000BCF7F File Offset: 0x000BBF7F
			void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback)
			{
			}

			// Token: 0x0600228F RID: 8847 RVA: 0x000BCF81 File Offset: 0x000BBF81
			void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote)
			{
			}

			// Token: 0x06002290 RID: 8848 RVA: 0x000BCF83 File Offset: 0x000BBF83
			void IServiceContainer.AddService(Type serviceType, object serviceInstance)
			{
			}

			// Token: 0x06002291 RID: 8849 RVA: 0x000BCF85 File Offset: 0x000BBF85
			void IServiceContainer.RemoveService(Type serviceType, bool promote)
			{
			}

			// Token: 0x06002292 RID: 8850 RVA: 0x000BCF87 File Offset: 0x000BBF87
			void IServiceContainer.RemoveService(Type serviceType)
			{
			}

			// Token: 0x17000660 RID: 1632
			// (get) Token: 0x06002293 RID: 8851 RVA: 0x000BCF89 File Offset: 0x000BBF89
			IContainer IDesignerHost.Container
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000661 RID: 1633
			// (get) Token: 0x06002294 RID: 8852 RVA: 0x000BCF8C File Offset: 0x000BBF8C
			bool IDesignerHost.InTransaction
			{
				get
				{
					return this._host.InTransaction;
				}
			}

			// Token: 0x17000662 RID: 1634
			// (get) Token: 0x06002295 RID: 8853 RVA: 0x000BCF99 File Offset: 0x000BBF99
			bool IDesignerHost.Loading
			{
				get
				{
					return this._host.Loading;
				}
			}

			// Token: 0x17000663 RID: 1635
			// (get) Token: 0x06002296 RID: 8854 RVA: 0x000BCFA6 File Offset: 0x000BBFA6
			string IDesignerHost.TransactionDescription
			{
				get
				{
					return this._host.TransactionDescription;
				}
			}

			// Token: 0x17000664 RID: 1636
			// (get) Token: 0x06002297 RID: 8855 RVA: 0x000BCFB3 File Offset: 0x000BBFB3
			IComponent IDesignerHost.RootComponent
			{
				get
				{
					return this._rootComponent;
				}
			}

			// Token: 0x17000665 RID: 1637
			// (get) Token: 0x06002298 RID: 8856 RVA: 0x000BCFBB File Offset: 0x000BBFBB
			string IDesignerHost.RootComponentClassName
			{
				get
				{
					return this._rootComponent.GetType().Name;
				}
			}

			// Token: 0x14000033 RID: 51
			// (add) Token: 0x06002299 RID: 8857 RVA: 0x000BCFCD File Offset: 0x000BBFCD
			// (remove) Token: 0x0600229A RID: 8858 RVA: 0x000BCFCF File Offset: 0x000BBFCF
			event EventHandler IDesignerHost.Activated
			{
				add
				{
				}
				remove
				{
				}
			}

			// Token: 0x14000034 RID: 52
			// (add) Token: 0x0600229B RID: 8859 RVA: 0x000BCFD1 File Offset: 0x000BBFD1
			// (remove) Token: 0x0600229C RID: 8860 RVA: 0x000BCFD3 File Offset: 0x000BBFD3
			event EventHandler IDesignerHost.Deactivated
			{
				add
				{
				}
				remove
				{
				}
			}

			// Token: 0x14000035 RID: 53
			// (add) Token: 0x0600229D RID: 8861 RVA: 0x000BCFD5 File Offset: 0x000BBFD5
			// (remove) Token: 0x0600229E RID: 8862 RVA: 0x000BCFE3 File Offset: 0x000BBFE3
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

			// Token: 0x14000036 RID: 54
			// (add) Token: 0x0600229F RID: 8863 RVA: 0x000BCFF1 File Offset: 0x000BBFF1
			// (remove) Token: 0x060022A0 RID: 8864 RVA: 0x000BCFF3 File Offset: 0x000BBFF3
			event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosed
			{
				add
				{
				}
				remove
				{
				}
			}

			// Token: 0x14000037 RID: 55
			// (add) Token: 0x060022A1 RID: 8865 RVA: 0x000BCFF5 File Offset: 0x000BBFF5
			// (remove) Token: 0x060022A2 RID: 8866 RVA: 0x000BCFF7 File Offset: 0x000BBFF7
			event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosing
			{
				add
				{
				}
				remove
				{
				}
			}

			// Token: 0x14000038 RID: 56
			// (add) Token: 0x060022A3 RID: 8867 RVA: 0x000BCFF9 File Offset: 0x000BBFF9
			// (remove) Token: 0x060022A4 RID: 8868 RVA: 0x000BCFFB File Offset: 0x000BBFFB
			event EventHandler IDesignerHost.TransactionOpened
			{
				add
				{
				}
				remove
				{
				}
			}

			// Token: 0x14000039 RID: 57
			// (add) Token: 0x060022A5 RID: 8869 RVA: 0x000BCFFD File Offset: 0x000BBFFD
			// (remove) Token: 0x060022A6 RID: 8870 RVA: 0x000BCFFF File Offset: 0x000BBFFF
			event EventHandler IDesignerHost.TransactionOpening
			{
				add
				{
				}
				remove
				{
				}
			}

			// Token: 0x060022A7 RID: 8871 RVA: 0x000BD001 File Offset: 0x000BC001
			void IDesignerHost.Activate()
			{
			}

			// Token: 0x060022A8 RID: 8872 RVA: 0x000BD003 File Offset: 0x000BC003
			IComponent IDesignerHost.CreateComponent(Type componentType)
			{
				return null;
			}

			// Token: 0x060022A9 RID: 8873 RVA: 0x000BD006 File Offset: 0x000BC006
			IComponent IDesignerHost.CreateComponent(Type componentType, string name)
			{
				return null;
			}

			// Token: 0x060022AA RID: 8874 RVA: 0x000BD009 File Offset: 0x000BC009
			DesignerTransaction IDesignerHost.CreateTransaction()
			{
				return this._host.CreateTransaction();
			}

			// Token: 0x060022AB RID: 8875 RVA: 0x000BD016 File Offset: 0x000BC016
			DesignerTransaction IDesignerHost.CreateTransaction(string description)
			{
				return this._host.CreateTransaction(description);
			}

			// Token: 0x060022AC RID: 8876 RVA: 0x000BD024 File Offset: 0x000BC024
			void IDesignerHost.DestroyComponent(IComponent component)
			{
				((IContainer)this).Remove(component);
			}

			// Token: 0x060022AD RID: 8877 RVA: 0x000BD02D File Offset: 0x000BC02D
			Type IDesignerHost.GetType(string typeName)
			{
				return this._host.GetType(typeName);
			}

			// Token: 0x060022AE RID: 8878 RVA: 0x000BD03C File Offset: 0x000BC03C
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

			// Token: 0x060022AF RID: 8879 RVA: 0x000BD0BC File Offset: 0x000BC0BC
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

			// Token: 0x060022B0 RID: 8880 RVA: 0x000BD1CC File Offset: 0x000BC1CC
			private static bool IsRooted(string basepath)
			{
				return basepath == null || basepath.Length == 0 || basepath[0] == '/' || basepath[0] == '\\';
			}

			// Token: 0x060022B1 RID: 8881 RVA: 0x000BD1F1 File Offset: 0x000BC1F1
			private static bool IsAppRelativePath(string path)
			{
				return path.Length >= 2 && path[0] == '~' && (path[1] == '/' || path[1] == '\\');
			}

			// Token: 0x0400185C RID: 6236
			private const string dummyProtocolAndServer = "file://foo";

			// Token: 0x0400185D RID: 6237
			private const char appRelativeCharacter = '~';

			// Token: 0x0400185E RID: 6238
			private Hashtable _componentTable;

			// Token: 0x0400185F RID: 6239
			private Hashtable _designerTable;

			// Token: 0x04001860 RID: 6240
			private IDesignerHost _host;

			// Token: 0x04001861 RID: 6241
			private bool _disposed;

			// Token: 0x04001862 RID: 6242
			private IComponent _rootComponent;

			// Token: 0x04001863 RID: 6243
			private int _nameCounter;

			// Token: 0x04001864 RID: 6244
			private string _userControlPath;

			// Token: 0x04001865 RID: 6245
			private IDictionary<string, string> _userControlRegisterEntries;

			// Token: 0x04001866 RID: 6246
			private IList<UserControlDesigner.TagNamespaceRegisterEntry> _tagNamespaceRegisterEntries;
		}

		// Token: 0x020003B1 RID: 945
		private sealed class DummyRootDesigner : WebFormsRootDesigner
		{
			// Token: 0x060022F7 RID: 8951 RVA: 0x000BDB5C File Offset: 0x000BCB5C
			public DummyRootDesigner(WebFormsRootDesigner rootDesigner, IDictionary<string, string> userControlRegisterEntries, IList<UserControlDesigner.TagNamespaceRegisterEntry> tagNamespaceRegisterEntries, string documentUrl)
			{
				this._rootDesigner = rootDesigner;
				this._userControlRegisterEntries = userControlRegisterEntries;
				this._tagNamespaceRegisterEntries = tagNamespaceRegisterEntries;
				this._documentUrl = documentUrl;
			}

			// Token: 0x17000676 RID: 1654
			// (get) Token: 0x060022F8 RID: 8952 RVA: 0x000BDB81 File Offset: 0x000BCB81
			public override string DocumentUrl
			{
				get
				{
					return this._documentUrl;
				}
			}

			// Token: 0x17000677 RID: 1655
			// (get) Token: 0x060022F9 RID: 8953 RVA: 0x000BDB89 File Offset: 0x000BCB89
			public override bool IsLoading
			{
				get
				{
					return this._rootDesigner.IsLoading;
				}
			}

			// Token: 0x17000678 RID: 1656
			// (get) Token: 0x060022FA RID: 8954 RVA: 0x000BDB96 File Offset: 0x000BCB96
			public override bool IsDesignerViewLocked
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000679 RID: 1657
			// (get) Token: 0x060022FB RID: 8955 RVA: 0x000BDB99 File Offset: 0x000BCB99
			public override WebFormsReferenceManager ReferenceManager
			{
				get
				{
					return new UserControlDesigner.DummyRootDesigner.DummyWebFormsReferenceManager(this, this._rootDesigner.ReferenceManager, this._userControlRegisterEntries, this._tagNamespaceRegisterEntries);
				}
			}

			// Token: 0x1700067A RID: 1658
			// (get) Token: 0x060022FC RID: 8956 RVA: 0x000BDBB8 File Offset: 0x000BCBB8
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

			// Token: 0x060022FD RID: 8957 RVA: 0x000BDBDE File Offset: 0x000BCBDE
			public override void AddClientScriptToDocument(ClientScriptItem scriptItem)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060022FE RID: 8958 RVA: 0x000BDBE5 File Offset: 0x000BCBE5
			public override string AddControlToDocument(Control newControl, Control referenceControl, ControlLocation location)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060022FF RID: 8959 RVA: 0x000BDBEC File Offset: 0x000BCBEC
			public override ClientScriptItemCollection GetClientScriptsInDocument()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06002300 RID: 8960 RVA: 0x000BDBF3 File Offset: 0x000BCBF3
			protected internal override void GetControlViewAndTag(Control control, out IControlDesignerView view, out IControlDesignerTag tag)
			{
				view = null;
				tag = null;
			}

			// Token: 0x06002301 RID: 8961 RVA: 0x000BDBFB File Offset: 0x000BCBFB
			public override void RemoveClientScriptFromDocument(string clientScriptId)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06002302 RID: 8962 RVA: 0x000BDC02 File Offset: 0x000BCC02
			public override void RemoveControlFromDocument(Control control)
			{
				throw new NotSupportedException();
			}

			// Token: 0x04001877 RID: 6263
			internal WebFormsRootDesigner _rootDesigner;

			// Token: 0x04001878 RID: 6264
			private IDictionary<string, string> _userControlRegisterEntries;

			// Token: 0x04001879 RID: 6265
			private IList<UserControlDesigner.TagNamespaceRegisterEntry> _tagNamespaceRegisterEntries;

			// Token: 0x0400187A RID: 6266
			private string _documentUrl;

			// Token: 0x020003B3 RID: 947
			private sealed class DummyWebFormsReferenceManager : WebFormsReferenceManager
			{
				// Token: 0x06002309 RID: 8969 RVA: 0x000BDC11 File Offset: 0x000BCC11
				public DummyWebFormsReferenceManager(UserControlDesigner.DummyRootDesigner owner, WebFormsReferenceManager baseReferenceManager, IDictionary<string, string> baseUserControlRegisterEntries, IList<UserControlDesigner.TagNamespaceRegisterEntry> tagNamespaceRegisterEntries)
				{
					this._owner = owner;
					this._baseReferenceManager = baseReferenceManager;
					this._baseUserControlRegisterEntries = baseUserControlRegisterEntries;
					this._tagNamespaceRegisterEntries = tagNamespaceRegisterEntries;
				}

				// Token: 0x0600230A RID: 8970 RVA: 0x000BDC38 File Offset: 0x000BCC38
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

				// Token: 0x0600230B RID: 8971 RVA: 0x000BDCB7 File Offset: 0x000BCCB7
				public override Type GetType(string tagPrefix, string tagName)
				{
					return this._baseReferenceManager.GetType(tagPrefix, tagName);
				}

				// Token: 0x0600230C RID: 8972 RVA: 0x000BDCC8 File Offset: 0x000BCCC8
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

				// Token: 0x0600230D RID: 8973 RVA: 0x000BDD94 File Offset: 0x000BCD94
				public override string RegisterTagPrefix(Type objectType)
				{
					throw new NotSupportedException();
				}

				// Token: 0x0600230E RID: 8974 RVA: 0x000BDD9B File Offset: 0x000BCD9B
				private static bool IsRooted(string basepath)
				{
					return basepath == null || basepath.Length == 0 || basepath[0] == '/' || basepath[0] == '\\' || Path.IsPathRooted(basepath) || basepath.IndexOf(Path.VolumeSeparatorChar) >= 0;
				}

				// Token: 0x0600230F RID: 8975 RVA: 0x000BDDD9 File Offset: 0x000BCDD9
				private static bool IsAppRelativePath(string path)
				{
					return path.Length >= 2 && path[0] == '~' && (path[1] == '/' || path[1] == '\\');
				}

				// Token: 0x06002310 RID: 8976 RVA: 0x000BDE0C File Offset: 0x000BCE0C
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

				// Token: 0x06002311 RID: 8977 RVA: 0x000BDE4C File Offset: 0x000BCE4C
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

				// Token: 0x06002312 RID: 8978 RVA: 0x000BE174 File Offset: 0x000BD174
				public override string GetUserControlPath(string tagPrefix, string tagName)
				{
					return this._owner._userControlRegisterEntries[tagPrefix + ":" + tagName];
				}

				// Token: 0x06002313 RID: 8979 RVA: 0x000BE194 File Offset: 0x000BD194
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

				// Token: 0x06002314 RID: 8980 RVA: 0x000BE290 File Offset: 0x000BD290
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

				// Token: 0x0400187B RID: 6267
				private UserControlDesigner.DummyRootDesigner _owner;

				// Token: 0x0400187C RID: 6268
				private WebFormsReferenceManager _baseReferenceManager;

				// Token: 0x0400187D RID: 6269
				private Collection<string> _registerDirectives;

				// Token: 0x0400187E RID: 6270
				private IDictionary<string, string> _baseUserControlRegisterEntries;

				// Token: 0x0400187F RID: 6271
				private IList<UserControlDesigner.TagNamespaceRegisterEntry> _tagNamespaceRegisterEntries;
			}
		}

		// Token: 0x020003B4 RID: 948
		private sealed class DummySite : ISite, IServiceProvider
		{
			// Token: 0x06002315 RID: 8981 RVA: 0x000BE34F File Offset: 0x000BD34F
			public DummySite(IComponent component, UserControlDesigner.UserControlDesignerHost designerHost)
			{
				this._component = component;
				this._container = designerHost;
				this._designerHost = designerHost;
			}

			// Token: 0x1700067B RID: 1659
			// (get) Token: 0x06002316 RID: 8982 RVA: 0x000BE36C File Offset: 0x000BD36C
			IComponent ISite.Component
			{
				get
				{
					return this._component;
				}
			}

			// Token: 0x1700067C RID: 1660
			// (get) Token: 0x06002317 RID: 8983 RVA: 0x000BE374 File Offset: 0x000BD374
			IContainer ISite.Container
			{
				get
				{
					return this._container;
				}
			}

			// Token: 0x1700067D RID: 1661
			// (get) Token: 0x06002318 RID: 8984 RVA: 0x000BE37C File Offset: 0x000BD37C
			bool ISite.DesignMode
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700067E RID: 1662
			// (get) Token: 0x06002319 RID: 8985 RVA: 0x000BE37F File Offset: 0x000BD37F
			// (set) Token: 0x0600231A RID: 8986 RVA: 0x000BE387 File Offset: 0x000BD387
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

			// Token: 0x0600231B RID: 8987 RVA: 0x000BE390 File Offset: 0x000BD390
			object IServiceProvider.GetService(Type type)
			{
				return this._designerHost.GetService(type);
			}

			// Token: 0x04001880 RID: 6272
			private IComponent _component;

			// Token: 0x04001881 RID: 6273
			private IDesignerHost _designerHost;

			// Token: 0x04001882 RID: 6274
			private IContainer _container;

			// Token: 0x04001883 RID: 6275
			private string _name;
		}
	}
}
