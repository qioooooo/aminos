using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.Adapters;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000054 RID: 84
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HttpCapabilitiesBase : IFilterResolutionService
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000CEBC File Offset: 0x0000BEBC
		internal static HttpCapabilitiesBase EmptyHttpCapabilitiesBase
		{
			get
			{
				if (HttpCapabilitiesBase._emptyHttpCapabilitiesBase != null)
				{
					return HttpCapabilitiesBase._emptyHttpCapabilitiesBase;
				}
				lock (HttpCapabilitiesBase._emptyHttpCapabilitiesBaseLock)
				{
					if (HttpCapabilitiesBase._emptyHttpCapabilitiesBase != null)
					{
						return HttpCapabilitiesBase._emptyHttpCapabilitiesBase;
					}
					HttpCapabilitiesBase._emptyHttpCapabilitiesBase = new HttpCapabilitiesBase();
				}
				return HttpCapabilitiesBase._emptyHttpCapabilitiesBase;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000CF1C File Offset: 0x0000BF1C
		public bool UseOptimizedCacheKey
		{
			get
			{
				return this._useOptimizedCacheKey;
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000CF24 File Offset: 0x0000BF24
		public void DisableOptimizedCacheKey()
		{
			this._useOptimizedCacheKey = false;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000CF30 File Offset: 0x0000BF30
		[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
		public static HttpCapabilitiesBase GetConfigCapabilities(string configKey, HttpRequest request)
		{
			HttpCapabilitiesBase httpCapabilitiesBase = null;
			if (configKey == "system.web/browserCaps")
			{
				httpCapabilitiesBase = HttpCapabilitiesBase.GetBrowserCapabilities(request);
			}
			else
			{
				HttpCapabilitiesEvaluator httpCapabilitiesEvaluator = (HttpCapabilitiesEvaluator)request.Context.GetSection(configKey);
				if (httpCapabilitiesEvaluator != null)
				{
					httpCapabilitiesBase = httpCapabilitiesEvaluator.Evaluate(request);
				}
			}
			if (httpCapabilitiesBase == null)
			{
				httpCapabilitiesBase = HttpCapabilitiesBase.EmptyHttpCapabilitiesBase;
			}
			return httpCapabilitiesBase;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000CF7C File Offset: 0x0000BF7C
		internal static HttpBrowserCapabilities GetBrowserCapabilities(HttpRequest request)
		{
			HttpCapabilitiesBase httpCapabilitiesBase = null;
			HttpCapabilitiesEvaluator browserCaps = RuntimeConfig.GetConfig(request.Context).BrowserCaps;
			if (browserCaps != null)
			{
				httpCapabilitiesBase = browserCaps.Evaluate(request);
			}
			return (HttpBrowserCapabilities)httpCapabilitiesBase;
		}

		// Token: 0x170000C1 RID: 193
		public virtual string this[string key]
		{
			get
			{
				return (string)this._items[key];
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000CFC0 File Offset: 0x0000BFC0
		public HtmlTextWriter CreateHtmlTextWriter(TextWriter w)
		{
			string htmlTextWriter = this.HtmlTextWriter;
			if (htmlTextWriter != null && htmlTextWriter.Length != 0)
			{
				try
				{
					Type type = BuildManager.GetType(htmlTextWriter, true, false);
					HtmlTextWriter htmlTextWriter2 = (HtmlTextWriter)Activator.CreateInstance(type, new object[] { w });
					if (htmlTextWriter2 != null)
					{
						return htmlTextWriter2;
					}
				}
				catch
				{
					throw new Exception(SR.GetString("Could_not_create_type_instance", new object[] { htmlTextWriter }));
				}
			}
			return this.CreateHtmlTextWriterInternal(w);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000D048 File Offset: 0x0000C048
		internal HtmlTextWriter CreateHtmlTextWriterInternal(TextWriter tw)
		{
			Type tagWriter = this.TagWriter;
			if (tagWriter != null)
			{
				return Page.CreateHtmlTextWriterFromType(tw, tagWriter);
			}
			return new Html32TextWriter(tw);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000D06D File Offset: 0x0000C06D
		protected virtual void Init()
		{
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000D070 File Offset: 0x0000C070
		internal void InitInternal(HttpBrowserCapabilities browserCaps)
		{
			if (this._items != null)
			{
				throw new ArgumentException(SR.GetString("Caps_cannot_be_inited_twice"));
			}
			this._items = browserCaps._items;
			this._adapters = browserCaps._adapters;
			this._browsers = browserCaps._browsers;
			this._htmlTextWriter = browserCaps._htmlTextWriter;
			this._useOptimizedCacheKey = browserCaps._useOptimizedCacheKey;
			this.Init();
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000D0D8 File Offset: 0x0000C0D8
		internal ControlAdapter GetAdapter(Control control)
		{
			if (this._adapters == null || this._adapters.Count == 0)
			{
				return null;
			}
			if (control == null)
			{
				return null;
			}
			Type type = control.GetType();
			object obj = this.AdapterTypes[type];
			if (object.ReferenceEquals(obj, HttpCapabilitiesBase.s_nullAdapterSingleton))
			{
				return null;
			}
			Type type2 = (Type)obj;
			if (type2 == null)
			{
				Type type3 = type;
				string text = null;
				while (text == null && type3 != typeof(Control))
				{
					string text2 = type3.AssemblyQualifiedName;
					text = (string)this.Adapters[text2];
					if (text == null)
					{
						text2 = type3.FullName;
						text = (string)this.Adapters[text2];
					}
					if (text != null)
					{
						break;
					}
					type3 = type3.BaseType;
				}
				if (string.IsNullOrEmpty(text))
				{
					this.AdapterTypes[type] = HttpCapabilitiesBase.s_nullAdapterSingleton;
					return null;
				}
				type2 = BuildManager.GetType(text, false, false);
				if (type2 == null)
				{
					throw new Exception(SR.GetString("ControlAdapters_TypeNotFound", new object[] { text }));
				}
				this.AdapterTypes[type] = type2;
			}
			IWebObjectFactory adapterFactory = this.GetAdapterFactory(type2);
			ControlAdapter controlAdapter = (ControlAdapter)adapterFactory.CreateInstance();
			controlAdapter._control = control;
			return controlAdapter;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000D20C File Offset: 0x0000C20C
		private IWebObjectFactory GetAdapterFactory(Type adapterType)
		{
			if (HttpCapabilitiesBase._controlAdapterFactoryGenerator == null)
			{
				lock (HttpCapabilitiesBase._staticLock)
				{
					if (HttpCapabilitiesBase._controlAdapterFactoryGenerator == null)
					{
						HttpCapabilitiesBase._controlAdapterFactoryTable = new Hashtable();
						HttpCapabilitiesBase._controlAdapterFactoryGenerator = new FactoryGenerator();
					}
				}
			}
			IWebObjectFactory webObjectFactory = (IWebObjectFactory)HttpCapabilitiesBase._controlAdapterFactoryTable[adapterType];
			if (webObjectFactory == null)
			{
				lock (HttpCapabilitiesBase._controlAdapterFactoryTable.SyncRoot)
				{
					webObjectFactory = (IWebObjectFactory)HttpCapabilitiesBase._controlAdapterFactoryTable[adapterType];
					if (webObjectFactory == null)
					{
						try
						{
							webObjectFactory = HttpCapabilitiesBase._controlAdapterFactoryGenerator.CreateFactory(adapterType);
						}
						catch
						{
							throw new Exception(SR.GetString("Could_not_create_type_instance", new object[] { adapterType.ToString() }));
						}
						HttpCapabilitiesBase._controlAdapterFactoryTable[adapterType] = webObjectFactory;
					}
				}
			}
			return webObjectFactory;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000D2F8 File Offset: 0x0000C2F8
		// (set) Token: 0x060002AD RID: 685 RVA: 0x0000D300 File Offset: 0x0000C300
		public IDictionary Capabilities
		{
			get
			{
				return this._items;
			}
			set
			{
				this._items = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000D30C File Offset: 0x0000C30C
		public IDictionary Adapters
		{
			get
			{
				if (this._adapters == null)
				{
					lock (HttpCapabilitiesBase._staticLock)
					{
						if (this._adapters == null)
						{
							this._adapters = new Hashtable(StringComparer.OrdinalIgnoreCase);
						}
					}
				}
				return this._adapters;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000D364 File Offset: 0x0000C364
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000D36C File Offset: 0x0000C36C
		public string HtmlTextWriter
		{
			get
			{
				return this._htmlTextWriter;
			}
			set
			{
				this._htmlTextWriter = value;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000D378 File Offset: 0x0000C378
		private Hashtable AdapterTypes
		{
			get
			{
				if (this._adapterTypes == null)
				{
					lock (HttpCapabilitiesBase._staticLock)
					{
						if (this._adapterTypes == null)
						{
							this._adapterTypes = Hashtable.Synchronized(new Hashtable());
						}
					}
				}
				return this._adapterTypes;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000D3D0 File Offset: 0x0000C3D0
		public string Id
		{
			get
			{
				if (this._browsers != null)
				{
					return (string)this._browsers[this._browsers.Count - 1];
				}
				return string.Empty;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000D3FD File Offset: 0x0000C3FD
		public ArrayList Browsers
		{
			get
			{
				return this._browsers;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000D408 File Offset: 0x0000C408
		public Version ClrVersion
		{
			get
			{
				Version[] clrVersions = this.GetClrVersions();
				if (clrVersions != null)
				{
					return clrVersions[clrVersions.Length - 1];
				}
				return null;
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000D428 File Offset: 0x0000C428
		public Version[] GetClrVersions()
		{
			string userAgent = HttpCapabilitiesEvaluator.GetUserAgent(HttpContext.Current.Request);
			if (string.IsNullOrEmpty(userAgent))
			{
				return null;
			}
			Regex regex = new Regex("\\.NET CLR (?'clrVersion'[0-9\\.]*)");
			MatchCollection matchCollection = regex.Matches(userAgent);
			if (matchCollection.Count == 0)
			{
				return new Version[]
				{
					new Version()
				};
			}
			ArrayList arrayList = new ArrayList();
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				try
				{
					Version version = new Version(match.Groups["clrVersion"].Value);
					arrayList.Add(version);
				}
				catch (FormatException)
				{
				}
			}
			arrayList.Sort();
			return (Version[])arrayList.ToArray(typeof(Version));
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000D520 File Offset: 0x0000C520
		public string Type
		{
			get
			{
				if (!this._havetype)
				{
					this._type = this["type"];
					this._havetype = true;
				}
				return this._type;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000D550 File Offset: 0x0000C550
		public string Browser
		{
			get
			{
				if (!this._havebrowser)
				{
					this._browser = this["browser"];
					this._havebrowser = true;
				}
				return this._browser;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000D580 File Offset: 0x0000C580
		public string Version
		{
			get
			{
				if (!this._haveversion)
				{
					this._version = this["version"];
					this._haveversion = true;
				}
				return this._version;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000D5B0 File Offset: 0x0000C5B0
		public int MajorVersion
		{
			get
			{
				if (!this._havemajorversion)
				{
					try
					{
						this._majorversion = int.Parse(this["majorversion"], CultureInfo.InvariantCulture);
						this._havemajorversion = true;
					}
					catch (FormatException ex)
					{
						throw this.BuildParseError(ex, "majorversion");
					}
				}
				return this._majorversion;
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000D618 File Offset: 0x0000C618
		private Exception BuildParseError(Exception e, string capsKey)
		{
			string @string = SR.GetString("Invalid_string_from_browser_caps", new object[]
			{
				e.Message,
				capsKey,
				this[capsKey]
			});
			ConfigurationErrorsException ex = new ConfigurationErrorsException(@string, e);
			HttpUnhandledException ex2 = new HttpUnhandledException(null, null);
			ex2.SetFormatter(new UseLastUnhandledErrorFormatter(ex));
			return ex2;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000D66C File Offset: 0x0000C66C
		private bool CapsParseBoolDefault(string capsKey, bool defaultValue)
		{
			string text = this[capsKey];
			if (text == null)
			{
				return defaultValue;
			}
			bool flag;
			try
			{
				flag = bool.Parse(text);
			}
			catch (FormatException)
			{
				flag = defaultValue;
			}
			return flag;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000D6A8 File Offset: 0x0000C6A8
		private bool CapsParseBool(string capsKey)
		{
			bool flag;
			try
			{
				flag = bool.Parse(this[capsKey]);
			}
			catch (FormatException ex)
			{
				throw this.BuildParseError(ex, capsKey);
			}
			return flag;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000D6E0 File Offset: 0x0000C6E0
		public string MinorVersionString
		{
			get
			{
				return this["minorversion"];
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000D6F0 File Offset: 0x0000C6F0
		public double MinorVersion
		{
			get
			{
				if (!this._haveminorversion)
				{
					lock (HttpCapabilitiesBase._staticLock)
					{
						if (!this._haveminorversion)
						{
							try
							{
								this._minorversion = double.Parse(this["minorversion"], NumberStyles.Float, NumberFormatInfo.InvariantInfo);
								this._haveminorversion = true;
							}
							catch (FormatException ex)
							{
								string text = this["minorversion"];
								int num = text.IndexOf('.');
								if (num != -1)
								{
									int num2 = text.IndexOf('.', num + 1);
									if (num2 != -1)
									{
										try
										{
											this._minorversion = double.Parse(text.Substring(0, num2), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
											Thread.MemoryBarrier();
											this._haveminorversion = true;
										}
										catch (FormatException)
										{
										}
									}
								}
								if (!this._haveminorversion)
								{
									throw this.BuildParseError(ex, "minorversion");
								}
							}
						}
					}
				}
				return this._minorversion;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000D7FC File Offset: 0x0000C7FC
		public string Platform
		{
			get
			{
				if (!this._haveplatform)
				{
					this._platform = this["platform"];
					this._haveplatform = true;
				}
				return this._platform;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000D82C File Offset: 0x0000C82C
		public Type TagWriter
		{
			get
			{
				try
				{
					if (!this._havetagwriter)
					{
						string text = this["tagwriter"];
						if (string.IsNullOrEmpty(text))
						{
							this._tagwriter = null;
						}
						else if (string.Compare(text, typeof(HtmlTextWriter).FullName, StringComparison.Ordinal) == 0)
						{
							this._tagwriter = typeof(HtmlTextWriter);
						}
						else
						{
							this._tagwriter = BuildManager.GetType(text, true);
						}
						this._havetagwriter = true;
					}
				}
				catch (Exception ex)
				{
					throw this.BuildParseError(ex, "tagwriter");
				}
				return this._tagwriter;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000D8D0 File Offset: 0x0000C8D0
		public Version EcmaScriptVersion
		{
			get
			{
				if (!this._haveecmascriptversion)
				{
					this._ecmascriptversion = new Version(this["ecmascriptversion"]);
					this._haveecmascriptversion = true;
				}
				return this._ecmascriptversion;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000D905 File Offset: 0x0000C905
		public Version MSDomVersion
		{
			get
			{
				if (!this._havemsdomversion)
				{
					this._msdomversion = new Version(this["msdomversion"]);
					this._havemsdomversion = true;
				}
				return this._msdomversion;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000D93A File Offset: 0x0000C93A
		public Version W3CDomVersion
		{
			get
			{
				if (!this._havew3cdomversion)
				{
					this._w3cdomversion = new Version(this["w3cdomversion"]);
					this._havew3cdomversion = true;
				}
				return this._w3cdomversion;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000D96F File Offset: 0x0000C96F
		public bool Beta
		{
			get
			{
				if (!this._havebeta)
				{
					this._beta = this.CapsParseBool("beta");
					this._havebeta = true;
				}
				return this._beta;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000D99F File Offset: 0x0000C99F
		public bool Crawler
		{
			get
			{
				if (!this._havecrawler)
				{
					this._crawler = this.CapsParseBool("crawler");
					this._havecrawler = true;
				}
				return this._crawler;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000D9CF File Offset: 0x0000C9CF
		public bool AOL
		{
			get
			{
				if (!this._haveaol)
				{
					this._aol = this.CapsParseBool("aol");
					this._haveaol = true;
				}
				return this._aol;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000D9FF File Offset: 0x0000C9FF
		public bool Win16
		{
			get
			{
				if (!this._havewin16)
				{
					this._win16 = this.CapsParseBool("win16");
					this._havewin16 = true;
				}
				return this._win16;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000DA2F File Offset: 0x0000CA2F
		public bool Win32
		{
			get
			{
				if (!this._havewin32)
				{
					this._win32 = this.CapsParseBool("win32");
					this._havewin32 = true;
				}
				return this._win32;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000DA5F File Offset: 0x0000CA5F
		public bool Frames
		{
			get
			{
				if (!this._haveframes)
				{
					this._frames = this.CapsParseBool("frames");
					this._haveframes = true;
				}
				return this._frames;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000DA8F File Offset: 0x0000CA8F
		public bool RequiresControlStateInSession
		{
			get
			{
				if (!this._haverequiresControlStateInSession)
				{
					if (this["requiresControlStateInSession"] != null)
					{
						this._requiresControlStateInSession = this.CapsParseBoolDefault("requiresControlStateInSession", false);
					}
					this._haverequiresControlStateInSession = true;
				}
				return this._requiresControlStateInSession;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000DACD File Offset: 0x0000CACD
		public bool Tables
		{
			get
			{
				if (!this._havetables)
				{
					this._tables = this.CapsParseBool("tables");
					this._havetables = true;
				}
				return this._tables;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000DAFD File Offset: 0x0000CAFD
		public bool Cookies
		{
			get
			{
				if (!this._havecookies)
				{
					this._cookies = this.CapsParseBool("cookies");
					this._havecookies = true;
				}
				return this._cookies;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000DB2D File Offset: 0x0000CB2D
		public bool VBScript
		{
			get
			{
				if (!this._havevbscript)
				{
					this._vbscript = this.CapsParseBool("vbscript");
					this._havevbscript = true;
				}
				return this._vbscript;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000DB5D File Offset: 0x0000CB5D
		[Obsolete("The recommended alternative is the EcmaScriptVersion property. A Major version value greater than or equal to 1 implies JavaScript support. http://go.microsoft.com/fwlink/?linkid=14202")]
		public bool JavaScript
		{
			get
			{
				if (!this._havejavascript)
				{
					this._javascript = this.CapsParseBool("javascript");
					this._havejavascript = true;
				}
				return this._javascript;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000DB8D File Offset: 0x0000CB8D
		public bool JavaApplets
		{
			get
			{
				if (!this._havejavaapplets)
				{
					this._javaapplets = this.CapsParseBool("javaapplets");
					this._havejavaapplets = true;
				}
				return this._javaapplets;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000DBBD File Offset: 0x0000CBBD
		public Version JScriptVersion
		{
			get
			{
				if (!this._havejscriptversion)
				{
					this._jscriptversion = new Version(this["jscriptversion"]);
					this._havejscriptversion = true;
				}
				return this._jscriptversion;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000DBF2 File Offset: 0x0000CBF2
		public bool ActiveXControls
		{
			get
			{
				if (!this._haveactivexcontrols)
				{
					this._activexcontrols = this.CapsParseBool("activexcontrols");
					this._haveactivexcontrols = true;
				}
				return this._activexcontrols;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000DC22 File Offset: 0x0000CC22
		public bool BackgroundSounds
		{
			get
			{
				if (!this._havebackgroundsounds)
				{
					this._backgroundsounds = this.CapsParseBool("backgroundsounds");
					this._havebackgroundsounds = true;
				}
				return this._backgroundsounds;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000DC52 File Offset: 0x0000CC52
		public bool CDF
		{
			get
			{
				if (!this._havecdf)
				{
					this._cdf = this.CapsParseBool("cdf");
					this._havecdf = true;
				}
				return this._cdf;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000DC82 File Offset: 0x0000CC82
		public virtual string MobileDeviceManufacturer
		{
			get
			{
				if (!this._haveMobileDeviceManufacturer)
				{
					this._mobileDeviceManufacturer = this["mobileDeviceManufacturer"];
					this._haveMobileDeviceManufacturer = true;
				}
				return this._mobileDeviceManufacturer;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000DCB2 File Offset: 0x0000CCB2
		public virtual string MobileDeviceModel
		{
			get
			{
				if (!this._haveMobileDeviceModel)
				{
					this._mobileDeviceModel = this["mobileDeviceModel"];
					this._haveMobileDeviceModel = true;
				}
				return this._mobileDeviceModel;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000DCE2 File Offset: 0x0000CCE2
		public virtual string GatewayVersion
		{
			get
			{
				if (!this._haveGatewayVersion)
				{
					this._gatewayVersion = this["gatewayVersion"];
					this._haveGatewayVersion = true;
				}
				return this._gatewayVersion;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000DD12 File Offset: 0x0000CD12
		public virtual int GatewayMajorVersion
		{
			get
			{
				if (!this._haveGatewayMajorVersion)
				{
					this._gatewayMajorVersion = Convert.ToInt32(this["gatewayMajorVersion"], CultureInfo.InvariantCulture);
					this._haveGatewayMajorVersion = true;
				}
				return this._gatewayMajorVersion;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000DD4C File Offset: 0x0000CD4C
		public virtual double GatewayMinorVersion
		{
			get
			{
				if (!this._haveGatewayMinorVersion)
				{
					this._gatewayMinorVersion = double.Parse(this["gatewayMinorVersion"], NumberStyles.Float, NumberFormatInfo.InvariantInfo);
					this._haveGatewayMinorVersion = true;
				}
				return this._gatewayMinorVersion;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000DD87 File Offset: 0x0000CD87
		public virtual string PreferredRenderingType
		{
			get
			{
				if (!this._havePreferredRenderingType)
				{
					this._preferredRenderingType = this["preferredRenderingType"];
					this._havePreferredRenderingType = true;
				}
				return this._preferredRenderingType;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000DDB7 File Offset: 0x0000CDB7
		public virtual string PreferredRequestEncoding
		{
			get
			{
				if (!this._havePreferredRequestEncoding)
				{
					this._preferredRequestEncoding = this["preferredRequestEncoding"];
					Thread.MemoryBarrier();
					this._havePreferredRequestEncoding = true;
				}
				return this._preferredRequestEncoding;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000DDEC File Offset: 0x0000CDEC
		public virtual string PreferredResponseEncoding
		{
			get
			{
				if (!this._havePreferredResponseEncoding)
				{
					this._preferredResponseEncoding = this["preferredResponseEncoding"];
					this._havePreferredResponseEncoding = true;
				}
				return this._preferredResponseEncoding;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000DE1C File Offset: 0x0000CE1C
		public virtual string PreferredRenderingMime
		{
			get
			{
				if (!this._havePreferredRenderingMime)
				{
					this._preferredRenderingMime = this["preferredRenderingMime"];
					this._havePreferredRenderingMime = true;
				}
				return this._preferredRenderingMime;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000DE4C File Offset: 0x0000CE4C
		public virtual string PreferredImageMime
		{
			get
			{
				if (!this._havePreferredImageMime)
				{
					this._preferredImageMime = this["preferredImageMime"];
					this._havePreferredImageMime = true;
				}
				return this._preferredImageMime;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000DE7C File Offset: 0x0000CE7C
		public virtual int ScreenCharactersWidth
		{
			get
			{
				if (!this._haveScreenCharactersWidth)
				{
					if (this["screenCharactersWidth"] == null)
					{
						int num = 640;
						int num2 = 8;
						if (this["screenPixelsWidth"] != null && this["characterWidth"] != null)
						{
							num = Convert.ToInt32(this["screenPixelsWidth"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["characterWidth"], CultureInfo.InvariantCulture);
						}
						else if (this["screenPixelsWidth"] != null)
						{
							num = Convert.ToInt32(this["screenPixelsWidth"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["defaultCharacterWidth"], CultureInfo.InvariantCulture);
						}
						else if (this["characterWidth"] != null)
						{
							num = Convert.ToInt32(this["defaultScreenPixelsWidth"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["characterWidth"], CultureInfo.InvariantCulture);
						}
						else if (this["defaultScreenCharactersWidth"] != null)
						{
							num = Convert.ToInt32(this["defaultScreenCharactersWidth"], CultureInfo.InvariantCulture);
							num2 = 1;
						}
						this._screenCharactersWidth = num / num2;
					}
					else
					{
						this._screenCharactersWidth = Convert.ToInt32(this["screenCharactersWidth"], CultureInfo.InvariantCulture);
					}
					this._haveScreenCharactersWidth = true;
				}
				return this._screenCharactersWidth;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000DFD0 File Offset: 0x0000CFD0
		public virtual int ScreenCharactersHeight
		{
			get
			{
				if (!this._haveScreenCharactersHeight)
				{
					if (this["screenCharactersHeight"] == null)
					{
						int num = 480;
						int num2 = 12;
						if (this["screenPixelsHeight"] != null && this["characterHeight"] != null)
						{
							num = Convert.ToInt32(this["screenPixelsHeight"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["characterHeight"], CultureInfo.InvariantCulture);
						}
						else if (this["screenPixelsHeight"] != null)
						{
							num = Convert.ToInt32(this["screenPixelsHeight"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["defaultCharacterHeight"], CultureInfo.InvariantCulture);
						}
						else if (this["characterHeight"] != null)
						{
							num = Convert.ToInt32(this["defaultScreenPixelsHeight"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["characterHeight"], CultureInfo.InvariantCulture);
						}
						else if (this["defaultScreenCharactersHeight"] != null)
						{
							num = Convert.ToInt32(this["defaultScreenCharactersHeight"], CultureInfo.InvariantCulture);
							num2 = 1;
						}
						this._screenCharactersHeight = num / num2;
					}
					else
					{
						this._screenCharactersHeight = Convert.ToInt32(this["screenCharactersHeight"], CultureInfo.InvariantCulture);
					}
					this._haveScreenCharactersHeight = true;
				}
				return this._screenCharactersHeight;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000E124 File Offset: 0x0000D124
		public virtual int ScreenPixelsWidth
		{
			get
			{
				if (!this._haveScreenPixelsWidth)
				{
					if (this["screenPixelsWidth"] == null)
					{
						int num = 80;
						int num2 = 8;
						if (this["screenCharactersWidth"] != null && this["characterWidth"] != null)
						{
							num = Convert.ToInt32(this["screenCharactersWidth"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["characterWidth"], CultureInfo.InvariantCulture);
						}
						else if (this["screenCharactersWidth"] != null)
						{
							num = Convert.ToInt32(this["screenCharactersWidth"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["defaultCharacterWidth"], CultureInfo.InvariantCulture);
						}
						else if (this["characterWidth"] != null)
						{
							num = Convert.ToInt32(this["defaultScreenCharactersWidth"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["characterWidth"], CultureInfo.InvariantCulture);
						}
						else if (this["defaultScreenPixelsWidth"] != null)
						{
							num = Convert.ToInt32(this["defaultScreenPixelsWidth"], CultureInfo.InvariantCulture);
							num2 = 1;
						}
						this._screenPixelsWidth = num * num2;
					}
					else
					{
						this._screenPixelsWidth = Convert.ToInt32(this["screenPixelsWidth"], CultureInfo.InvariantCulture);
					}
					this._haveScreenPixelsWidth = true;
				}
				return this._screenPixelsWidth;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000E274 File Offset: 0x0000D274
		public virtual int ScreenPixelsHeight
		{
			get
			{
				if (!this._haveScreenPixelsHeight)
				{
					if (this["screenPixelsHeight"] == null)
					{
						int num = 40;
						int num2 = 12;
						if (this["screenCharactersHeight"] != null && this["characterHeight"] != null)
						{
							num = Convert.ToInt32(this["screenCharactersHeight"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["characterHeight"], CultureInfo.InvariantCulture);
						}
						else if (this["screenCharactersHeight"] != null)
						{
							num = Convert.ToInt32(this["screenCharactersHeight"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["defaultCharacterHeight"], CultureInfo.InvariantCulture);
						}
						else if (this["characterHeight"] != null)
						{
							num = Convert.ToInt32(this["defaultScreenCharactersHeight"], CultureInfo.InvariantCulture);
							num2 = Convert.ToInt32(this["characterHeight"], CultureInfo.InvariantCulture);
						}
						else if (this["defaultScreenPixelsHeight"] != null)
						{
							num = Convert.ToInt32(this["defaultScreenPixelsHeight"], CultureInfo.InvariantCulture);
							num2 = 1;
						}
						this._screenPixelsHeight = num * num2;
					}
					else
					{
						this._screenPixelsHeight = Convert.ToInt32(this["screenPixelsHeight"], CultureInfo.InvariantCulture);
					}
					this._haveScreenPixelsHeight = true;
				}
				return this._screenPixelsHeight;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000E3C5 File Offset: 0x0000D3C5
		public virtual int ScreenBitDepth
		{
			get
			{
				if (!this._haveScreenBitDepth)
				{
					this._screenBitDepth = Convert.ToInt32(this["screenBitDepth"], CultureInfo.InvariantCulture);
					this._haveScreenBitDepth = true;
				}
				return this._screenBitDepth;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000E400 File Offset: 0x0000D400
		public virtual bool IsColor
		{
			get
			{
				if (!this._haveIsColor)
				{
					if (this["isColor"] == null)
					{
						this._isColor = false;
					}
					else
					{
						this._isColor = Convert.ToBoolean(this["isColor"], CultureInfo.InvariantCulture);
					}
					this._haveIsColor = true;
				}
				return this._isColor;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000E45F File Offset: 0x0000D45F
		public virtual string InputType
		{
			get
			{
				if (!this._haveInputType)
				{
					this._inputType = this["inputType"];
					this._haveInputType = true;
				}
				return this._inputType;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000E48F File Offset: 0x0000D48F
		public virtual int NumberOfSoftkeys
		{
			get
			{
				if (!this._haveNumberOfSoftkeys)
				{
					this._numberOfSoftkeys = Convert.ToInt32(this["numberOfSoftkeys"], CultureInfo.InvariantCulture);
					this._haveNumberOfSoftkeys = true;
				}
				return this._numberOfSoftkeys;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000E4C9 File Offset: 0x0000D4C9
		public virtual int MaximumSoftkeyLabelLength
		{
			get
			{
				if (!this._haveMaximumSoftkeyLabelLength)
				{
					this._maximumSoftkeyLabelLength = Convert.ToInt32(this["maximumSoftkeyLabelLength"], CultureInfo.InvariantCulture);
					this._haveMaximumSoftkeyLabelLength = true;
				}
				return this._maximumSoftkeyLabelLength;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000E503 File Offset: 0x0000D503
		public virtual bool CanInitiateVoiceCall
		{
			get
			{
				if (!this._haveCanInitiateVoiceCall)
				{
					this._canInitiateVoiceCall = this.CapsParseBoolDefault("canInitiateVoiceCall", false);
					this._haveCanInitiateVoiceCall = true;
				}
				return this._canInitiateVoiceCall;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000E534 File Offset: 0x0000D534
		public virtual bool CanSendMail
		{
			get
			{
				if (!this._haveCanSendMail)
				{
					this._canSendMail = this.CapsParseBoolDefault("canSendMail", true);
					this._haveCanSendMail = true;
				}
				return this._canSendMail;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000E565 File Offset: 0x0000D565
		public virtual bool HasBackButton
		{
			get
			{
				if (!this._haveHasBackButton)
				{
					this._hasBackButton = this.CapsParseBoolDefault("hasBackButton", true);
					this._haveHasBackButton = true;
				}
				return this._hasBackButton;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000E596 File Offset: 0x0000D596
		public virtual bool RendersWmlDoAcceptsInline
		{
			get
			{
				if (!this._haveRendersWmlDoAcceptsInline)
				{
					this._rendersWmlDoAcceptsInline = this.CapsParseBoolDefault("rendersWmlDoAcceptsInline", true);
					this._haveRendersWmlDoAcceptsInline = true;
				}
				return this._rendersWmlDoAcceptsInline;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000E5C7 File Offset: 0x0000D5C7
		public virtual bool RendersWmlSelectsAsMenuCards
		{
			get
			{
				if (!this._haveRendersWmlSelectsAsMenuCards)
				{
					this._rendersWmlSelectsAsMenuCards = this.CapsParseBoolDefault("rendersWmlSelectsAsMenuCards", false);
					this._haveRendersWmlSelectsAsMenuCards = true;
				}
				return this._rendersWmlSelectsAsMenuCards;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000E5F8 File Offset: 0x0000D5F8
		public virtual bool RendersBreaksAfterWmlAnchor
		{
			get
			{
				if (!this._haveRendersBreaksAfterWmlAnchor)
				{
					this._rendersBreaksAfterWmlAnchor = this.CapsParseBoolDefault("rendersBreaksAfterWmlAnchor", true);
					this._haveRendersBreaksAfterWmlAnchor = true;
				}
				return this._rendersBreaksAfterWmlAnchor;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000E629 File Offset: 0x0000D629
		public virtual bool RendersBreaksAfterWmlInput
		{
			get
			{
				if (!this._haveRendersBreaksAfterWmlInput)
				{
					this._rendersBreaksAfterWmlInput = this.CapsParseBoolDefault("rendersBreaksAfterWmlInput", true);
					this._haveRendersBreaksAfterWmlInput = true;
				}
				return this._rendersBreaksAfterWmlInput;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000E65A File Offset: 0x0000D65A
		public virtual bool RendersBreakBeforeWmlSelectAndInput
		{
			get
			{
				if (!this._haveRendersBreakBeforeWmlSelectAndInput)
				{
					this._rendersBreakBeforeWmlSelectAndInput = this.CapsParseBoolDefault("rendersBreakBeforeWmlSelectAndInput", false);
					this._haveRendersBreakBeforeWmlSelectAndInput = true;
				}
				return this._rendersBreakBeforeWmlSelectAndInput;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000E68B File Offset: 0x0000D68B
		public virtual bool RequiresPhoneNumbersAsPlainText
		{
			get
			{
				if (!this._haveRequiresPhoneNumbersAsPlainText)
				{
					this._requiresPhoneNumbersAsPlainText = this.CapsParseBoolDefault("requiresPhoneNumbersAsPlainText", false);
					this._haveRequiresPhoneNumbersAsPlainText = true;
				}
				return this._requiresPhoneNumbersAsPlainText;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000E6BC File Offset: 0x0000D6BC
		public virtual bool RequiresUrlEncodedPostfieldValues
		{
			get
			{
				if (!this._haveRequiresUrlEncodedPostfieldValues)
				{
					this._requiresUrlEncodedPostfieldValues = this.CapsParseBoolDefault("requiresUrlEncodedPostfieldValues", true);
					this._haveRequiresUrlEncodedPostfieldValues = true;
				}
				return this._requiresUrlEncodedPostfieldValues;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000E6F0 File Offset: 0x0000D6F0
		public virtual string RequiredMetaTagNameValue
		{
			get
			{
				if (!this._haveRequiredMetaTagNameValue)
				{
					string text = this["requiredMetaTagNameValue"];
					if (!string.IsNullOrEmpty(text))
					{
						this._requiredMetaTagNameValue = text;
					}
					else
					{
						this._requiredMetaTagNameValue = null;
					}
					this._haveRequiredMetaTagNameValue = true;
				}
				return this._requiredMetaTagNameValue;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000E740 File Offset: 0x0000D740
		public virtual bool RendersBreaksAfterHtmlLists
		{
			get
			{
				if (!this._haveRendersBreaksAfterHtmlLists)
				{
					this._rendersBreaksAfterHtmlLists = this.CapsParseBoolDefault("rendersBreaksAfterHtmlLists", true);
					this._haveRendersBreaksAfterHtmlLists = true;
				}
				return this._rendersBreaksAfterHtmlLists;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000E771 File Offset: 0x0000D771
		public virtual bool RequiresUniqueHtmlInputNames
		{
			get
			{
				if (!this._haveRequiresUniqueHtmlInputNames)
				{
					this._requiresUniqueHtmlInputNames = this.CapsParseBoolDefault("requiresUniqueHtmlInputNames", false);
					this._haveRequiresUniqueHtmlInputNames = true;
				}
				return this._requiresUniqueHtmlInputNames;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000E7A2 File Offset: 0x0000D7A2
		public virtual bool RequiresUniqueHtmlCheckboxNames
		{
			get
			{
				if (!this._haveRequiresUniqueHtmlCheckboxNames)
				{
					this._requiresUniqueHtmlCheckboxNames = this.CapsParseBoolDefault("requiresUniqueHtmlCheckboxNames", false);
					this._haveRequiresUniqueHtmlCheckboxNames = true;
				}
				return this._requiresUniqueHtmlCheckboxNames;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000E7D3 File Offset: 0x0000D7D3
		public virtual bool SupportsCss
		{
			get
			{
				if (!this._haveSupportsCss)
				{
					this._supportsCss = this.CapsParseBoolDefault("supportsCss", false);
					this._haveSupportsCss = true;
				}
				return this._supportsCss;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000E804 File Offset: 0x0000D804
		public virtual bool HidesRightAlignedMultiselectScrollbars
		{
			get
			{
				if (!this._haveHidesRightAlignedMultiselectScrollbars)
				{
					this._hidesRightAlignedMultiselectScrollbars = this.CapsParseBoolDefault("hidesRightAlignedMultiselectScrollbars", false);
					this._haveHidesRightAlignedMultiselectScrollbars = true;
				}
				return this._hidesRightAlignedMultiselectScrollbars;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000E835 File Offset: 0x0000D835
		public virtual bool IsMobileDevice
		{
			get
			{
				if (!this._haveIsMobileDevice)
				{
					this._isMobileDevice = this.CapsParseBoolDefault("isMobileDevice", false);
					this._haveIsMobileDevice = true;
				}
				return this._isMobileDevice;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000E866 File Offset: 0x0000D866
		public virtual bool RequiresAttributeColonSubstitution
		{
			get
			{
				if (!this._haveRequiresAttributeColonSubstitution)
				{
					this._requiresAttributeColonSubstitution = this.CapsParseBoolDefault("requiresAttributeColonSubstitution", false);
					this._haveRequiresAttributeColonSubstitution = true;
				}
				return this._requiresAttributeColonSubstitution;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x0000E897 File Offset: 0x0000D897
		public virtual bool CanRenderOneventAndPrevElementsTogether
		{
			get
			{
				if (!this._haveCanRenderOneventAndPrevElementsTogether)
				{
					this._canRenderOneventAndPrevElementsTogether = this.CapsParseBoolDefault("canRenderOneventAndPrevElementsTogether", true);
					this._haveCanRenderOneventAndPrevElementsTogether = true;
				}
				return this._canRenderOneventAndPrevElementsTogether;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000E8C8 File Offset: 0x0000D8C8
		public virtual bool CanRenderInputAndSelectElementsTogether
		{
			get
			{
				if (!this._haveCanRenderInputAndSelectElementsTogether)
				{
					this._canRenderInputAndSelectElementsTogether = this.CapsParseBoolDefault("canRenderInputAndSelectElementsTogether", true);
					this._haveCanRenderInputAndSelectElementsTogether = true;
				}
				return this._canRenderInputAndSelectElementsTogether;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000E8F9 File Offset: 0x0000D8F9
		public virtual bool CanRenderAfterInputOrSelectElement
		{
			get
			{
				if (!this._haveCanRenderAfterInputOrSelectElement)
				{
					this._canRenderAfterInputOrSelectElement = this.CapsParseBoolDefault("canRenderAfterInputOrSelectElement", true);
					this._haveCanRenderAfterInputOrSelectElement = true;
				}
				return this._canRenderAfterInputOrSelectElement;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000E92A File Offset: 0x0000D92A
		public virtual bool CanRenderPostBackCards
		{
			get
			{
				if (!this._haveCanRenderPostBackCards)
				{
					this._canRenderPostBackCards = this.CapsParseBoolDefault("canRenderPostBackCards", true);
					this._haveCanRenderPostBackCards = true;
				}
				return this._canRenderPostBackCards;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000E95B File Offset: 0x0000D95B
		public virtual bool CanRenderMixedSelects
		{
			get
			{
				if (!this._haveCanRenderMixedSelects)
				{
					this._canRenderMixedSelects = this.CapsParseBoolDefault("canRenderMixedSelects", true);
					this._haveCanRenderMixedSelects = true;
				}
				return this._canRenderMixedSelects;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000E98C File Offset: 0x0000D98C
		public virtual bool CanCombineFormsInDeck
		{
			get
			{
				if (!this._haveCanCombineFormsInDeck)
				{
					this._canCombineFormsInDeck = this.CapsParseBoolDefault("canCombineFormsInDeck", true);
					this._haveCanCombineFormsInDeck = true;
				}
				return this._canCombineFormsInDeck;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000E9BD File Offset: 0x0000D9BD
		public virtual bool CanRenderSetvarZeroWithMultiSelectionList
		{
			get
			{
				if (!this._haveCanRenderSetvarZeroWithMultiSelectionList)
				{
					this._canRenderSetvarZeroWithMultiSelectionList = this.CapsParseBoolDefault("canRenderSetvarZeroWithMultiSelectionList", true);
					this._haveCanRenderSetvarZeroWithMultiSelectionList = true;
				}
				return this._canRenderSetvarZeroWithMultiSelectionList;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000E9EE File Offset: 0x0000D9EE
		public virtual bool SupportsImageSubmit
		{
			get
			{
				if (!this._haveSupportsImageSubmit)
				{
					this._supportsImageSubmit = this.CapsParseBoolDefault("supportsImageSubmit", false);
					this._haveSupportsImageSubmit = true;
				}
				return this._supportsImageSubmit;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000EA1F File Offset: 0x0000DA1F
		public virtual bool RequiresUniqueFilePathSuffix
		{
			get
			{
				if (!this._haveRequiresUniqueFilePathSuffix)
				{
					this._requiresUniqueFilePathSuffix = this.CapsParseBoolDefault("requiresUniqueFilePathSuffix", false);
					this._haveRequiresUniqueFilePathSuffix = true;
				}
				return this._requiresUniqueFilePathSuffix;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000EA50 File Offset: 0x0000DA50
		public virtual bool RequiresNoBreakInFormatting
		{
			get
			{
				if (!this._haveRequiresNoBreakInFormatting)
				{
					this._requiresNoBreakInFormatting = this.CapsParseBoolDefault("requiresNoBreakInFormatting", false);
					this._haveRequiresNoBreakInFormatting = true;
				}
				return this._requiresNoBreakInFormatting;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000EA81 File Offset: 0x0000DA81
		public virtual bool RequiresLeadingPageBreak
		{
			get
			{
				if (!this._haveRequiresLeadingPageBreak)
				{
					this._requiresLeadingPageBreak = this.CapsParseBoolDefault("requiresLeadingPageBreak", false);
					this._haveRequiresLeadingPageBreak = true;
				}
				return this._requiresLeadingPageBreak;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000EAB2 File Offset: 0x0000DAB2
		public virtual bool SupportsSelectMultiple
		{
			get
			{
				if (!this._haveSupportsSelectMultiple)
				{
					this._supportsSelectMultiple = this.CapsParseBoolDefault("supportsSelectMultiple", false);
					this._haveSupportsSelectMultiple = true;
				}
				return this._supportsSelectMultiple;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000EAE3 File Offset: 0x0000DAE3
		public virtual bool SupportsBold
		{
			get
			{
				if (!this._haveSupportsBold)
				{
					this._supportsBold = this.CapsParseBoolDefault("supportsBold", true);
					this._haveSupportsBold = true;
				}
				return this._supportsBold;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000EB14 File Offset: 0x0000DB14
		public virtual bool SupportsItalic
		{
			get
			{
				if (!this._haveSupportsItalic)
				{
					this._supportsItalic = this.CapsParseBoolDefault("supportsItalic", true);
					this._haveSupportsItalic = true;
				}
				return this._supportsItalic;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000EB45 File Offset: 0x0000DB45
		public virtual bool SupportsFontSize
		{
			get
			{
				if (!this._haveSupportsFontSize)
				{
					this._supportsFontSize = this.CapsParseBoolDefault("supportsFontSize", false);
					this._haveSupportsFontSize = true;
				}
				return this._supportsFontSize;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000EB76 File Offset: 0x0000DB76
		public virtual bool SupportsFontName
		{
			get
			{
				if (!this._haveSupportsFontName)
				{
					this._supportsFontName = this.CapsParseBoolDefault("supportsFontName", false);
					this._haveSupportsFontName = true;
				}
				return this._supportsFontName;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000EBA7 File Offset: 0x0000DBA7
		public virtual bool SupportsFontColor
		{
			get
			{
				if (!this._haveSupportsFontColor)
				{
					this._supportsFontColor = this.CapsParseBoolDefault("supportsFontColor", false);
					this._haveSupportsFontColor = true;
				}
				return this._supportsFontColor;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0000EBD8 File Offset: 0x0000DBD8
		public virtual bool SupportsBodyColor
		{
			get
			{
				if (!this._haveSupportsBodyColor)
				{
					this._supportsBodyColor = this.CapsParseBoolDefault("supportsBodyColor", false);
					this._haveSupportsBodyColor = true;
				}
				return this._supportsBodyColor;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600030B RID: 779 RVA: 0x0000EC09 File Offset: 0x0000DC09
		public virtual bool SupportsDivAlign
		{
			get
			{
				if (!this._haveSupportsDivAlign)
				{
					this._supportsDivAlign = this.CapsParseBoolDefault("supportsDivAlign", false);
					this._haveSupportsDivAlign = true;
				}
				return this._supportsDivAlign;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0000EC3A File Offset: 0x0000DC3A
		public virtual bool SupportsDivNoWrap
		{
			get
			{
				if (!this._haveSupportsDivNoWrap)
				{
					this._supportsDivNoWrap = this.CapsParseBoolDefault("supportsDivNoWrap", false);
					this._haveSupportsDivNoWrap = true;
				}
				return this._supportsDivNoWrap;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000EC6B File Offset: 0x0000DC6B
		internal bool SupportsMaintainScrollPositionOnPostback
		{
			get
			{
				if (!this._haveSupportsMaintainScrollPositionOnPostback)
				{
					this._supportsMaintainScrollPositionOnPostback = this.CapsParseBoolDefault("supportsMaintainScrollPositionOnPostback", false);
					this._haveSupportsMaintainScrollPositionOnPostback = true;
				}
				return this._supportsMaintainScrollPositionOnPostback;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000EC9C File Offset: 0x0000DC9C
		public virtual bool RequiresContentTypeMetaTag
		{
			get
			{
				if (!this._haveRequiresContentTypeMetaTag)
				{
					this._requiresContentTypeMetaTag = this.CapsParseBoolDefault("requiresContentTypeMetaTag", false);
					this._haveRequiresContentTypeMetaTag = true;
				}
				return this._requiresContentTypeMetaTag;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000ECCD File Offset: 0x0000DCCD
		public virtual bool RequiresDBCSCharacter
		{
			get
			{
				if (!this._haveRequiresDBCSCharacter)
				{
					this._requiresDBCSCharacter = this.CapsParseBoolDefault("requiresDBCSCharacter", false);
					this._haveRequiresDBCSCharacter = true;
				}
				return this._requiresDBCSCharacter;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000ECFE File Offset: 0x0000DCFE
		public virtual bool RequiresHtmlAdaptiveErrorReporting
		{
			get
			{
				if (!this._haveRequiresHtmlAdaptiveErrorReporting)
				{
					this._requiresHtmlAdaptiveErrorReporting = this.CapsParseBoolDefault("requiresHtmlAdaptiveErrorReporting", false);
					this._haveRequiresHtmlAdaptiveErrorReporting = true;
				}
				return this._requiresHtmlAdaptiveErrorReporting;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0000ED2F File Offset: 0x0000DD2F
		public virtual bool RequiresOutputOptimization
		{
			get
			{
				if (!this._haveRequiresOutputOptimization)
				{
					this._requiresOutputOptimization = this.CapsParseBoolDefault("requiresOutputOptimization", false);
					this._haveRequiresOutputOptimization = true;
				}
				return this._requiresOutputOptimization;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0000ED60 File Offset: 0x0000DD60
		public virtual bool SupportsAccesskeyAttribute
		{
			get
			{
				if (!this._haveSupportsAccesskeyAttribute)
				{
					this._supportsAccesskeyAttribute = this.CapsParseBoolDefault("supportsAccesskeyAttribute", false);
					this._haveSupportsAccesskeyAttribute = true;
				}
				return this._supportsAccesskeyAttribute;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000ED91 File Offset: 0x0000DD91
		public virtual bool SupportsInputIStyle
		{
			get
			{
				if (!this._haveSupportsInputIStyle)
				{
					this._supportsInputIStyle = this.CapsParseBoolDefault("supportsInputIStyle", false);
					this._haveSupportsInputIStyle = true;
				}
				return this._supportsInputIStyle;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000EDC2 File Offset: 0x0000DDC2
		public virtual bool SupportsInputMode
		{
			get
			{
				if (!this._haveSupportsInputMode)
				{
					this._supportsInputMode = this.CapsParseBoolDefault("supportsInputMode", false);
					this._haveSupportsInputMode = true;
				}
				return this._supportsInputMode;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000EDF3 File Offset: 0x0000DDF3
		public virtual bool SupportsIModeSymbols
		{
			get
			{
				if (!this._haveSupportsIModeSymbols)
				{
					this._supportsIModeSymbols = this.CapsParseBoolDefault("supportsIModeSymbols", false);
					this._haveSupportsIModeSymbols = true;
				}
				return this._supportsIModeSymbols;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0000EE24 File Offset: 0x0000DE24
		public virtual bool SupportsJPhoneSymbols
		{
			get
			{
				if (!this._haveSupportsJPhoneSymbols)
				{
					this._supportsJPhoneSymbols = this.CapsParseBoolDefault("supportsJPhoneSymbols", false);
					this._haveSupportsJPhoneSymbols = true;
				}
				return this._supportsJPhoneSymbols;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0000EE55 File Offset: 0x0000DE55
		public virtual bool SupportsJPhoneMultiMediaAttributes
		{
			get
			{
				if (!this._haveSupportsJPhoneMultiMediaAttributes)
				{
					this._supportsJPhoneMultiMediaAttributes = this.CapsParseBoolDefault("supportsJPhoneMultiMediaAttributes", false);
					this._haveSupportsJPhoneMultiMediaAttributes = true;
				}
				return this._supportsJPhoneMultiMediaAttributes;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000EE86 File Offset: 0x0000DE86
		public virtual int MaximumRenderedPageSize
		{
			get
			{
				if (!this._haveMaximumRenderedPageSize)
				{
					this._maximumRenderedPageSize = Convert.ToInt32(this["maximumRenderedPageSize"], CultureInfo.InvariantCulture);
					this._haveMaximumRenderedPageSize = true;
				}
				return this._maximumRenderedPageSize;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000EEC0 File Offset: 0x0000DEC0
		public virtual bool RequiresSpecialViewStateEncoding
		{
			get
			{
				if (!this._haveRequiresSpecialViewStateEncoding)
				{
					this._requiresSpecialViewStateEncoding = this.CapsParseBoolDefault("requiresSpecialViewStateEncoding", false);
					this._haveRequiresSpecialViewStateEncoding = true;
				}
				return this._requiresSpecialViewStateEncoding;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000EEF1 File Offset: 0x0000DEF1
		public virtual bool SupportsQueryStringInFormAction
		{
			get
			{
				if (!this._haveSupportsQueryStringInFormAction)
				{
					this._supportsQueryStringInFormAction = this.CapsParseBoolDefault("supportsQueryStringInFormAction", true);
					this._haveSupportsQueryStringInFormAction = true;
				}
				return this._supportsQueryStringInFormAction;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000EF22 File Offset: 0x0000DF22
		public virtual bool SupportsCacheControlMetaTag
		{
			get
			{
				if (!this._haveSupportsCacheControlMetaTag)
				{
					this._supportsCacheControlMetaTag = this.CapsParseBoolDefault("supportsCacheControlMetaTag", true);
					this._haveSupportsCacheControlMetaTag = true;
				}
				return this._supportsCacheControlMetaTag;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000EF53 File Offset: 0x0000DF53
		public virtual bool SupportsUncheck
		{
			get
			{
				if (!this._haveSupportsUncheck)
				{
					this._supportsUncheck = this.CapsParseBoolDefault("supportsUncheck", true);
					this._haveSupportsUncheck = true;
				}
				return this._supportsUncheck;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000EF84 File Offset: 0x0000DF84
		public virtual bool CanRenderEmptySelects
		{
			get
			{
				if (!this._haveCanRenderEmptySelects)
				{
					this._canRenderEmptySelects = this.CapsParseBoolDefault("canRenderEmptySelects", true);
					this._haveCanRenderEmptySelects = true;
				}
				return this._canRenderEmptySelects;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0000EFB5 File Offset: 0x0000DFB5
		public virtual bool SupportsRedirectWithCookie
		{
			get
			{
				if (!this._haveSupportsRedirectWithCookie)
				{
					this._supportsRedirectWithCookie = this.CapsParseBoolDefault("supportsRedirectWithCookie", true);
					this._haveSupportsRedirectWithCookie = true;
				}
				return this._supportsRedirectWithCookie;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600031F RID: 799 RVA: 0x0000EFE6 File Offset: 0x0000DFE6
		public virtual bool SupportsEmptyStringInCookieValue
		{
			get
			{
				if (!this._haveSupportsEmptyStringInCookieValue)
				{
					this._supportsEmptyStringInCookieValue = this.CapsParseBoolDefault("supportsEmptyStringInCookieValue", true);
					this._haveSupportsEmptyStringInCookieValue = true;
				}
				return this._supportsEmptyStringInCookieValue;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000F018 File Offset: 0x0000E018
		public virtual int DefaultSubmitButtonLimit
		{
			get
			{
				if (!this._haveDefaultSubmitButtonLimit)
				{
					this._defaultSubmitButtonLimit = ((this["defaultSubmitButtonLimit"] != null) ? Convert.ToInt32(this["defaultSubmitButtonLimit"], CultureInfo.InvariantCulture) : 1);
					this._haveDefaultSubmitButtonLimit = true;
				}
				return this._defaultSubmitButtonLimit;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0000F06F File Offset: 0x0000E06F
		public virtual bool SupportsXmlHttp
		{
			get
			{
				if (!this._haveSupportsXmlHttp)
				{
					this._supportsXmlHttp = this.CapsParseBoolDefault("supportsXmlHttp", false);
					this._haveSupportsXmlHttp = true;
				}
				return this._supportsXmlHttp;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000F0A0 File Offset: 0x0000E0A0
		public virtual bool SupportsCallback
		{
			get
			{
				if (!this._haveSupportsCallback)
				{
					this._supportsCallback = this.CapsParseBoolDefault("supportsCallback", false);
					this._haveSupportsCallback = true;
				}
				return this._supportsCallback;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000F0D1 File Offset: 0x0000E0D1
		public virtual int MaximumHrefLength
		{
			get
			{
				if (!this._haveMaximumHrefLength)
				{
					this._maximumHrefLength = Convert.ToInt32(this["maximumHrefLength"], CultureInfo.InvariantCulture);
					this._haveMaximumHrefLength = true;
				}
				return this._maximumHrefLength;
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000F10C File Offset: 0x0000E10C
		public bool IsBrowser(string browserName)
		{
			if (string.IsNullOrEmpty(browserName))
			{
				return false;
			}
			if (this._browsers == null)
			{
				return false;
			}
			for (int i = 0; i < this._browsers.Count; i++)
			{
				if (string.Equals(browserName, (string)this._browsers[i], StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000F160 File Offset: 0x0000E160
		public void AddBrowser(string browserName)
		{
			if (this._browsers == null)
			{
				lock (HttpCapabilitiesBase._staticLock)
				{
					if (this._browsers == null)
					{
						this._browsers = new ArrayList(6);
					}
				}
			}
			this._browsers.Add(browserName.ToLower(CultureInfo.InvariantCulture));
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000F1C8 File Offset: 0x0000E1C8
		bool IFilterResolutionService.EvaluateFilter(string filterName)
		{
			return this.IsBrowser(filterName);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000F1D1 File Offset: 0x0000E1D1
		int IFilterResolutionService.CompareFilters(string filter1, string filter2)
		{
			return BrowserCapabilitiesCompiler.BrowserCapabilitiesFactory.CompareFilters(filter1, filter2);
		}

		// Token: 0x04000E73 RID: 3699
		private static FactoryGenerator _controlAdapterFactoryGenerator;

		// Token: 0x04000E74 RID: 3700
		private static Hashtable _controlAdapterFactoryTable;

		// Token: 0x04000E75 RID: 3701
		private static object _staticLock = new object();

		// Token: 0x04000E76 RID: 3702
		private static object s_nullAdapterSingleton = new object();

		// Token: 0x04000E77 RID: 3703
		private bool _useOptimizedCacheKey = true;

		// Token: 0x04000E78 RID: 3704
		private static object _emptyHttpCapabilitiesBaseLock = new object();

		// Token: 0x04000E79 RID: 3705
		private static HttpCapabilitiesBase _emptyHttpCapabilitiesBase;

		// Token: 0x04000E7A RID: 3706
		private Hashtable _adapterTypes;

		// Token: 0x04000E7B RID: 3707
		private IDictionary _adapters;

		// Token: 0x04000E7C RID: 3708
		private string _htmlTextWriter;

		// Token: 0x04000E7D RID: 3709
		private IDictionary _items;

		// Token: 0x04000E7E RID: 3710
		private ArrayList _browsers;

		// Token: 0x04000E7F RID: 3711
		private volatile string _type;

		// Token: 0x04000E80 RID: 3712
		private volatile string _browser;

		// Token: 0x04000E81 RID: 3713
		private volatile string _version;

		// Token: 0x04000E82 RID: 3714
		private volatile int _majorversion;

		// Token: 0x04000E83 RID: 3715
		private double _minorversion;

		// Token: 0x04000E84 RID: 3716
		private volatile string _platform;

		// Token: 0x04000E85 RID: 3717
		private volatile Type _tagwriter;

		// Token: 0x04000E86 RID: 3718
		private volatile Version _ecmascriptversion;

		// Token: 0x04000E87 RID: 3719
		private volatile Version _jscriptversion;

		// Token: 0x04000E88 RID: 3720
		private volatile Version _msdomversion;

		// Token: 0x04000E89 RID: 3721
		private volatile Version _w3cdomversion;

		// Token: 0x04000E8A RID: 3722
		private volatile bool _beta;

		// Token: 0x04000E8B RID: 3723
		private volatile bool _crawler;

		// Token: 0x04000E8C RID: 3724
		private volatile bool _aol;

		// Token: 0x04000E8D RID: 3725
		private volatile bool _win16;

		// Token: 0x04000E8E RID: 3726
		private volatile bool _win32;

		// Token: 0x04000E8F RID: 3727
		private volatile bool _requiresControlStateInSession;

		// Token: 0x04000E90 RID: 3728
		private volatile bool _frames;

		// Token: 0x04000E91 RID: 3729
		private volatile bool _tables;

		// Token: 0x04000E92 RID: 3730
		private volatile bool _cookies;

		// Token: 0x04000E93 RID: 3731
		private volatile bool _vbscript;

		// Token: 0x04000E94 RID: 3732
		private volatile bool _javascript;

		// Token: 0x04000E95 RID: 3733
		private volatile bool _javaapplets;

		// Token: 0x04000E96 RID: 3734
		private volatile bool _activexcontrols;

		// Token: 0x04000E97 RID: 3735
		private volatile bool _backgroundsounds;

		// Token: 0x04000E98 RID: 3736
		private volatile bool _cdf;

		// Token: 0x04000E99 RID: 3737
		private volatile bool _havetype;

		// Token: 0x04000E9A RID: 3738
		private volatile bool _havebrowser;

		// Token: 0x04000E9B RID: 3739
		private volatile bool _haveversion;

		// Token: 0x04000E9C RID: 3740
		private volatile bool _havemajorversion;

		// Token: 0x04000E9D RID: 3741
		private volatile bool _haveminorversion;

		// Token: 0x04000E9E RID: 3742
		private volatile bool _haveplatform;

		// Token: 0x04000E9F RID: 3743
		private volatile bool _havetagwriter;

		// Token: 0x04000EA0 RID: 3744
		private volatile bool _haveecmascriptversion;

		// Token: 0x04000EA1 RID: 3745
		private volatile bool _havemsdomversion;

		// Token: 0x04000EA2 RID: 3746
		private volatile bool _havew3cdomversion;

		// Token: 0x04000EA3 RID: 3747
		private volatile bool _havebeta;

		// Token: 0x04000EA4 RID: 3748
		private volatile bool _havecrawler;

		// Token: 0x04000EA5 RID: 3749
		private volatile bool _haveaol;

		// Token: 0x04000EA6 RID: 3750
		private volatile bool _havewin16;

		// Token: 0x04000EA7 RID: 3751
		private volatile bool _havewin32;

		// Token: 0x04000EA8 RID: 3752
		private volatile bool _haveframes;

		// Token: 0x04000EA9 RID: 3753
		private volatile bool _haverequiresControlStateInSession;

		// Token: 0x04000EAA RID: 3754
		private volatile bool _havetables;

		// Token: 0x04000EAB RID: 3755
		private volatile bool _havecookies;

		// Token: 0x04000EAC RID: 3756
		private volatile bool _havevbscript;

		// Token: 0x04000EAD RID: 3757
		private volatile bool _havejavascript;

		// Token: 0x04000EAE RID: 3758
		private volatile bool _havejavaapplets;

		// Token: 0x04000EAF RID: 3759
		private volatile bool _haveactivexcontrols;

		// Token: 0x04000EB0 RID: 3760
		private volatile bool _havebackgroundsounds;

		// Token: 0x04000EB1 RID: 3761
		private volatile bool _havecdf;

		// Token: 0x04000EB2 RID: 3762
		private volatile string _mobileDeviceManufacturer;

		// Token: 0x04000EB3 RID: 3763
		private volatile string _mobileDeviceModel;

		// Token: 0x04000EB4 RID: 3764
		private volatile string _gatewayVersion;

		// Token: 0x04000EB5 RID: 3765
		private volatile int _gatewayMajorVersion;

		// Token: 0x04000EB6 RID: 3766
		private double _gatewayMinorVersion;

		// Token: 0x04000EB7 RID: 3767
		private volatile string _preferredRenderingType;

		// Token: 0x04000EB8 RID: 3768
		private volatile string _preferredRenderingMime;

		// Token: 0x04000EB9 RID: 3769
		private volatile string _preferredImageMime;

		// Token: 0x04000EBA RID: 3770
		private volatile string _requiredMetaTagNameValue;

		// Token: 0x04000EBB RID: 3771
		private volatile string _preferredRequestEncoding;

		// Token: 0x04000EBC RID: 3772
		private volatile string _preferredResponseEncoding;

		// Token: 0x04000EBD RID: 3773
		private volatile int _screenCharactersWidth;

		// Token: 0x04000EBE RID: 3774
		private volatile int _screenCharactersHeight;

		// Token: 0x04000EBF RID: 3775
		private volatile int _screenPixelsWidth;

		// Token: 0x04000EC0 RID: 3776
		private volatile int _screenPixelsHeight;

		// Token: 0x04000EC1 RID: 3777
		private volatile int _screenBitDepth;

		// Token: 0x04000EC2 RID: 3778
		private volatile bool _isColor;

		// Token: 0x04000EC3 RID: 3779
		private volatile string _inputType;

		// Token: 0x04000EC4 RID: 3780
		private volatile int _numberOfSoftkeys;

		// Token: 0x04000EC5 RID: 3781
		private volatile int _maximumSoftkeyLabelLength;

		// Token: 0x04000EC6 RID: 3782
		private volatile bool _canInitiateVoiceCall;

		// Token: 0x04000EC7 RID: 3783
		private volatile bool _canSendMail;

		// Token: 0x04000EC8 RID: 3784
		private volatile bool _hasBackButton;

		// Token: 0x04000EC9 RID: 3785
		private volatile bool _rendersWmlDoAcceptsInline;

		// Token: 0x04000ECA RID: 3786
		private volatile bool _rendersWmlSelectsAsMenuCards;

		// Token: 0x04000ECB RID: 3787
		private volatile bool _rendersBreaksAfterWmlAnchor;

		// Token: 0x04000ECC RID: 3788
		private volatile bool _rendersBreaksAfterWmlInput;

		// Token: 0x04000ECD RID: 3789
		private volatile bool _rendersBreakBeforeWmlSelectAndInput;

		// Token: 0x04000ECE RID: 3790
		private volatile bool _requiresPhoneNumbersAsPlainText;

		// Token: 0x04000ECF RID: 3791
		private volatile bool _requiresAttributeColonSubstitution;

		// Token: 0x04000ED0 RID: 3792
		private volatile bool _requiresUrlEncodedPostfieldValues;

		// Token: 0x04000ED1 RID: 3793
		private volatile bool _rendersBreaksAfterHtmlLists;

		// Token: 0x04000ED2 RID: 3794
		private volatile bool _requiresUniqueHtmlCheckboxNames;

		// Token: 0x04000ED3 RID: 3795
		private volatile bool _requiresUniqueHtmlInputNames;

		// Token: 0x04000ED4 RID: 3796
		private volatile bool _supportsCss;

		// Token: 0x04000ED5 RID: 3797
		private volatile bool _hidesRightAlignedMultiselectScrollbars;

		// Token: 0x04000ED6 RID: 3798
		private volatile bool _isMobileDevice;

		// Token: 0x04000ED7 RID: 3799
		private volatile bool _canRenderOneventAndPrevElementsTogether;

		// Token: 0x04000ED8 RID: 3800
		private volatile bool _canRenderInputAndSelectElementsTogether;

		// Token: 0x04000ED9 RID: 3801
		private volatile bool _canRenderAfterInputOrSelectElement;

		// Token: 0x04000EDA RID: 3802
		private volatile bool _canRenderPostBackCards;

		// Token: 0x04000EDB RID: 3803
		private volatile bool _canRenderMixedSelects;

		// Token: 0x04000EDC RID: 3804
		private volatile bool _canCombineFormsInDeck;

		// Token: 0x04000EDD RID: 3805
		private volatile bool _canRenderSetvarZeroWithMultiSelectionList;

		// Token: 0x04000EDE RID: 3806
		private volatile bool _supportsImageSubmit;

		// Token: 0x04000EDF RID: 3807
		private volatile bool _requiresUniqueFilePathSuffix;

		// Token: 0x04000EE0 RID: 3808
		private volatile bool _requiresNoBreakInFormatting;

		// Token: 0x04000EE1 RID: 3809
		private volatile bool _requiresLeadingPageBreak;

		// Token: 0x04000EE2 RID: 3810
		private volatile bool _supportsSelectMultiple;

		// Token: 0x04000EE3 RID: 3811
		private volatile bool _supportsBold;

		// Token: 0x04000EE4 RID: 3812
		private volatile bool _supportsItalic;

		// Token: 0x04000EE5 RID: 3813
		private volatile bool _supportsFontSize;

		// Token: 0x04000EE6 RID: 3814
		private volatile bool _supportsFontName;

		// Token: 0x04000EE7 RID: 3815
		private volatile bool _supportsFontColor;

		// Token: 0x04000EE8 RID: 3816
		private volatile bool _supportsBodyColor;

		// Token: 0x04000EE9 RID: 3817
		private volatile bool _supportsDivAlign;

		// Token: 0x04000EEA RID: 3818
		private volatile bool _supportsDivNoWrap;

		// Token: 0x04000EEB RID: 3819
		private volatile bool _requiresHtmlAdaptiveErrorReporting;

		// Token: 0x04000EEC RID: 3820
		private volatile bool _requiresContentTypeMetaTag;

		// Token: 0x04000EED RID: 3821
		private volatile bool _requiresDBCSCharacter;

		// Token: 0x04000EEE RID: 3822
		private volatile bool _requiresOutputOptimization;

		// Token: 0x04000EEF RID: 3823
		private volatile bool _supportsAccesskeyAttribute;

		// Token: 0x04000EF0 RID: 3824
		private volatile bool _supportsInputIStyle;

		// Token: 0x04000EF1 RID: 3825
		private volatile bool _supportsInputMode;

		// Token: 0x04000EF2 RID: 3826
		private volatile bool _supportsIModeSymbols;

		// Token: 0x04000EF3 RID: 3827
		private volatile bool _supportsJPhoneSymbols;

		// Token: 0x04000EF4 RID: 3828
		private volatile bool _supportsJPhoneMultiMediaAttributes;

		// Token: 0x04000EF5 RID: 3829
		private volatile int _maximumRenderedPageSize;

		// Token: 0x04000EF6 RID: 3830
		private volatile bool _requiresSpecialViewStateEncoding;

		// Token: 0x04000EF7 RID: 3831
		private volatile bool _supportsQueryStringInFormAction;

		// Token: 0x04000EF8 RID: 3832
		private volatile bool _supportsCacheControlMetaTag;

		// Token: 0x04000EF9 RID: 3833
		private volatile bool _supportsUncheck;

		// Token: 0x04000EFA RID: 3834
		private volatile bool _canRenderEmptySelects;

		// Token: 0x04000EFB RID: 3835
		private volatile bool _supportsRedirectWithCookie;

		// Token: 0x04000EFC RID: 3836
		private volatile bool _supportsEmptyStringInCookieValue;

		// Token: 0x04000EFD RID: 3837
		private volatile int _defaultSubmitButtonLimit;

		// Token: 0x04000EFE RID: 3838
		private volatile bool _supportsXmlHttp;

		// Token: 0x04000EFF RID: 3839
		private volatile bool _supportsCallback;

		// Token: 0x04000F00 RID: 3840
		private volatile bool _supportsMaintainScrollPositionOnPostback;

		// Token: 0x04000F01 RID: 3841
		private volatile int _maximumHrefLength;

		// Token: 0x04000F02 RID: 3842
		private volatile bool _haveMobileDeviceManufacturer;

		// Token: 0x04000F03 RID: 3843
		private volatile bool _haveMobileDeviceModel;

		// Token: 0x04000F04 RID: 3844
		private volatile bool _haveGatewayVersion;

		// Token: 0x04000F05 RID: 3845
		private volatile bool _haveGatewayMajorVersion;

		// Token: 0x04000F06 RID: 3846
		private volatile bool _haveGatewayMinorVersion;

		// Token: 0x04000F07 RID: 3847
		private volatile bool _havePreferredRenderingType;

		// Token: 0x04000F08 RID: 3848
		private volatile bool _havePreferredRenderingMime;

		// Token: 0x04000F09 RID: 3849
		private volatile bool _havePreferredImageMime;

		// Token: 0x04000F0A RID: 3850
		private volatile bool _havePreferredRequestEncoding;

		// Token: 0x04000F0B RID: 3851
		private volatile bool _havePreferredResponseEncoding;

		// Token: 0x04000F0C RID: 3852
		private volatile bool _haveScreenCharactersWidth;

		// Token: 0x04000F0D RID: 3853
		private volatile bool _haveScreenCharactersHeight;

		// Token: 0x04000F0E RID: 3854
		private volatile bool _haveScreenPixelsWidth;

		// Token: 0x04000F0F RID: 3855
		private volatile bool _haveScreenPixelsHeight;

		// Token: 0x04000F10 RID: 3856
		private volatile bool _haveScreenBitDepth;

		// Token: 0x04000F11 RID: 3857
		private volatile bool _haveIsColor;

		// Token: 0x04000F12 RID: 3858
		private volatile bool _haveInputType;

		// Token: 0x04000F13 RID: 3859
		private volatile bool _haveNumberOfSoftkeys;

		// Token: 0x04000F14 RID: 3860
		private volatile bool _haveMaximumSoftkeyLabelLength;

		// Token: 0x04000F15 RID: 3861
		private volatile bool _haveCanInitiateVoiceCall;

		// Token: 0x04000F16 RID: 3862
		private volatile bool _haveCanSendMail;

		// Token: 0x04000F17 RID: 3863
		private volatile bool _haveHasBackButton;

		// Token: 0x04000F18 RID: 3864
		private volatile bool _haveRendersWmlDoAcceptsInline;

		// Token: 0x04000F19 RID: 3865
		private volatile bool _haveRendersWmlSelectsAsMenuCards;

		// Token: 0x04000F1A RID: 3866
		private volatile bool _haveRendersBreaksAfterWmlAnchor;

		// Token: 0x04000F1B RID: 3867
		private volatile bool _haveRendersBreaksAfterWmlInput;

		// Token: 0x04000F1C RID: 3868
		private volatile bool _haveRendersBreakBeforeWmlSelectAndInput;

		// Token: 0x04000F1D RID: 3869
		private volatile bool _haveRequiresPhoneNumbersAsPlainText;

		// Token: 0x04000F1E RID: 3870
		private volatile bool _haveRequiresUrlEncodedPostfieldValues;

		// Token: 0x04000F1F RID: 3871
		private volatile bool _haveRequiredMetaTagNameValue;

		// Token: 0x04000F20 RID: 3872
		private volatile bool _haveRendersBreaksAfterHtmlLists;

		// Token: 0x04000F21 RID: 3873
		private volatile bool _haveRequiresUniqueHtmlCheckboxNames;

		// Token: 0x04000F22 RID: 3874
		private volatile bool _haveRequiresUniqueHtmlInputNames;

		// Token: 0x04000F23 RID: 3875
		private volatile bool _haveSupportsCss;

		// Token: 0x04000F24 RID: 3876
		private volatile bool _haveHidesRightAlignedMultiselectScrollbars;

		// Token: 0x04000F25 RID: 3877
		private volatile bool _haveIsMobileDevice;

		// Token: 0x04000F26 RID: 3878
		private volatile bool _haveCanRenderOneventAndPrevElementsTogether;

		// Token: 0x04000F27 RID: 3879
		private volatile bool _haveCanRenderInputAndSelectElementsTogether;

		// Token: 0x04000F28 RID: 3880
		private volatile bool _haveCanRenderAfterInputOrSelectElement;

		// Token: 0x04000F29 RID: 3881
		private volatile bool _haveCanRenderPostBackCards;

		// Token: 0x04000F2A RID: 3882
		private volatile bool _haveCanCombineFormsInDeck;

		// Token: 0x04000F2B RID: 3883
		private volatile bool _haveCanRenderMixedSelects;

		// Token: 0x04000F2C RID: 3884
		private volatile bool _haveCanRenderSetvarZeroWithMultiSelectionList;

		// Token: 0x04000F2D RID: 3885
		private volatile bool _haveSupportsImageSubmit;

		// Token: 0x04000F2E RID: 3886
		private volatile bool _haveRequiresUniqueFilePathSuffix;

		// Token: 0x04000F2F RID: 3887
		private volatile bool _haveRequiresNoBreakInFormatting;

		// Token: 0x04000F30 RID: 3888
		private volatile bool _haveRequiresLeadingPageBreak;

		// Token: 0x04000F31 RID: 3889
		private volatile bool _haveSupportsSelectMultiple;

		// Token: 0x04000F32 RID: 3890
		private volatile bool _haveRequiresAttributeColonSubstitution;

		// Token: 0x04000F33 RID: 3891
		private volatile bool _haveRequiresHtmlAdaptiveErrorReporting;

		// Token: 0x04000F34 RID: 3892
		private volatile bool _haveRequiresContentTypeMetaTag;

		// Token: 0x04000F35 RID: 3893
		private volatile bool _haveRequiresDBCSCharacter;

		// Token: 0x04000F36 RID: 3894
		private volatile bool _haveRequiresOutputOptimization;

		// Token: 0x04000F37 RID: 3895
		private volatile bool _haveSupportsAccesskeyAttribute;

		// Token: 0x04000F38 RID: 3896
		private volatile bool _haveSupportsInputIStyle;

		// Token: 0x04000F39 RID: 3897
		private volatile bool _haveSupportsInputMode;

		// Token: 0x04000F3A RID: 3898
		private volatile bool _haveSupportsIModeSymbols;

		// Token: 0x04000F3B RID: 3899
		private volatile bool _haveSupportsJPhoneSymbols;

		// Token: 0x04000F3C RID: 3900
		private volatile bool _haveSupportsJPhoneMultiMediaAttributes;

		// Token: 0x04000F3D RID: 3901
		private volatile bool _haveSupportsRedirectWithCookie;

		// Token: 0x04000F3E RID: 3902
		private volatile bool _haveSupportsEmptyStringInCookieValue;

		// Token: 0x04000F3F RID: 3903
		private volatile bool _haveSupportsBold;

		// Token: 0x04000F40 RID: 3904
		private volatile bool _haveSupportsItalic;

		// Token: 0x04000F41 RID: 3905
		private volatile bool _haveSupportsFontSize;

		// Token: 0x04000F42 RID: 3906
		private volatile bool _haveSupportsFontName;

		// Token: 0x04000F43 RID: 3907
		private volatile bool _haveSupportsFontColor;

		// Token: 0x04000F44 RID: 3908
		private volatile bool _haveSupportsBodyColor;

		// Token: 0x04000F45 RID: 3909
		private volatile bool _haveSupportsDivAlign;

		// Token: 0x04000F46 RID: 3910
		private volatile bool _haveSupportsDivNoWrap;

		// Token: 0x04000F47 RID: 3911
		private volatile bool _haveMaximumRenderedPageSize;

		// Token: 0x04000F48 RID: 3912
		private volatile bool _haveRequiresSpecialViewStateEncoding;

		// Token: 0x04000F49 RID: 3913
		private volatile bool _haveSupportsQueryStringInFormAction;

		// Token: 0x04000F4A RID: 3914
		private volatile bool _haveSupportsCacheControlMetaTag;

		// Token: 0x04000F4B RID: 3915
		private volatile bool _haveSupportsUncheck;

		// Token: 0x04000F4C RID: 3916
		private volatile bool _haveCanRenderEmptySelects;

		// Token: 0x04000F4D RID: 3917
		private volatile bool _haveDefaultSubmitButtonLimit;

		// Token: 0x04000F4E RID: 3918
		private volatile bool _haveSupportsXmlHttp;

		// Token: 0x04000F4F RID: 3919
		private volatile bool _haveSupportsCallback;

		// Token: 0x04000F50 RID: 3920
		private volatile bool _haveSupportsMaintainScrollPositionOnPostback;

		// Token: 0x04000F51 RID: 3921
		private volatile bool _haveMaximumHrefLength;

		// Token: 0x04000F52 RID: 3922
		private volatile bool _havejscriptversion;
	}
}
