using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.Util;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x0200021D RID: 541
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PagesSection : ConfigurationSection
	{
		// Token: 0x06001CF0 RID: 7408 RVA: 0x00083B7C File Offset: 0x00082B7C
		static PagesSection()
		{
			PagesSection._properties.Add(PagesSection._propBuffer);
			PagesSection._properties.Add(PagesSection._propEnableSessionState);
			PagesSection._properties.Add(PagesSection._propEnableViewState);
			PagesSection._properties.Add(PagesSection._propEnableViewStateMac);
			PagesSection._properties.Add(PagesSection._propEnableEventValidation);
			PagesSection._properties.Add(PagesSection._propSmartNavigation);
			PagesSection._properties.Add(PagesSection._propAutoEventWireup);
			PagesSection._properties.Add(PagesSection._propPageBaseType);
			PagesSection._properties.Add(PagesSection._propUserControlBaseType);
			PagesSection._properties.Add(PagesSection._propValidateRequest);
			PagesSection._properties.Add(PagesSection._propMasterPageFile);
			PagesSection._properties.Add(PagesSection._propTheme);
			PagesSection._properties.Add(PagesSection._propStyleSheetTheme);
			PagesSection._properties.Add(PagesSection._propNamespaces);
			PagesSection._properties.Add(PagesSection._propControls);
			PagesSection._properties.Add(PagesSection._propTagMapping);
			PagesSection._properties.Add(PagesSection._propMaxPageStateFieldLength);
			PagesSection._properties.Add(PagesSection._propCompilationMode);
			PagesSection._properties.Add(PagesSection._propPageParserFilterType);
			PagesSection._properties.Add(PagesSection._propViewStateEncryptionMode);
			PagesSection._properties.Add(PagesSection._propMaintainScrollPosition);
			PagesSection._properties.Add(PagesSection._propAsyncTimeout);
			PagesSection._properties.Add(PagesSection._propRenderAllHiddenFieldsAtTopOfForm);
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x00083FD6 File Offset: 0x00082FD6
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return PagesSection._properties;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x00083FDD File Offset: 0x00082FDD
		// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x00083FEF File Offset: 0x00082FEF
		[ConfigurationProperty("buffer", DefaultValue = true)]
		public bool Buffer
		{
			get
			{
				return (bool)base[PagesSection._propBuffer];
			}
			set
			{
				base[PagesSection._propBuffer] = value;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x00084004 File Offset: 0x00083004
		// (set) Token: 0x06001CF6 RID: 7414 RVA: 0x00084094 File Offset: 0x00083094
		[ConfigurationProperty("enableSessionState", DefaultValue = "true")]
		public PagesEnableSessionState EnableSessionState
		{
			get
			{
				string text;
				if ((text = (string)base[PagesSection._propEnableSessionState]) != null)
				{
					PagesEnableSessionState pagesEnableSessionState;
					if (!(text == "true"))
					{
						if (!(text == "false"))
						{
							if (!(text == "ReadOnly"))
							{
								goto IL_004B;
							}
							pagesEnableSessionState = PagesEnableSessionState.ReadOnly;
						}
						else
						{
							pagesEnableSessionState = PagesEnableSessionState.False;
						}
					}
					else
					{
						pagesEnableSessionState = PagesEnableSessionState.True;
					}
					return pagesEnableSessionState;
				}
				IL_004B:
				string name = PagesSection._propEnableSessionState.Name;
				string text2 = "true, false, ReadOnly";
				throw new ConfigurationErrorsException(SR.GetString("Invalid_enum_attribute", new object[] { name, text2 }));
			}
			set
			{
				string text;
				switch (value)
				{
				case PagesEnableSessionState.False:
					text = "false";
					break;
				case PagesEnableSessionState.ReadOnly:
					text = "ReadOnly";
					break;
				case PagesEnableSessionState.True:
					text = "true";
					break;
				default:
					text = "true";
					break;
				}
				base[PagesSection._propEnableSessionState] = text;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x000840E7 File Offset: 0x000830E7
		// (set) Token: 0x06001CF8 RID: 7416 RVA: 0x000840F9 File Offset: 0x000830F9
		[ConfigurationProperty("enableViewState", DefaultValue = true)]
		public bool EnableViewState
		{
			get
			{
				return (bool)base[PagesSection._propEnableViewState];
			}
			set
			{
				base[PagesSection._propEnableViewState] = value;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x0008410C File Offset: 0x0008310C
		// (set) Token: 0x06001CFA RID: 7418 RVA: 0x0008411E File Offset: 0x0008311E
		[ConfigurationProperty("enableViewStateMac", DefaultValue = true)]
		public bool EnableViewStateMac
		{
			get
			{
				return (bool)base[PagesSection._propEnableViewStateMac];
			}
			set
			{
				base[PagesSection._propEnableViewStateMac] = value;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001CFB RID: 7419 RVA: 0x00084131 File Offset: 0x00083131
		// (set) Token: 0x06001CFC RID: 7420 RVA: 0x00084143 File Offset: 0x00083143
		[ConfigurationProperty("enableEventValidation", DefaultValue = true)]
		public bool EnableEventValidation
		{
			get
			{
				return (bool)base[PagesSection._propEnableEventValidation];
			}
			set
			{
				base[PagesSection._propEnableEventValidation] = value;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001CFD RID: 7421 RVA: 0x00084156 File Offset: 0x00083156
		// (set) Token: 0x06001CFE RID: 7422 RVA: 0x00084168 File Offset: 0x00083168
		[ConfigurationProperty("smartNavigation", DefaultValue = false)]
		public bool SmartNavigation
		{
			get
			{
				return (bool)base[PagesSection._propSmartNavigation];
			}
			set
			{
				base[PagesSection._propSmartNavigation] = value;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001CFF RID: 7423 RVA: 0x0008417B File Offset: 0x0008317B
		// (set) Token: 0x06001D00 RID: 7424 RVA: 0x0008418D File Offset: 0x0008318D
		[ConfigurationProperty("autoEventWireup", DefaultValue = true)]
		public bool AutoEventWireup
		{
			get
			{
				return (bool)base[PagesSection._propAutoEventWireup];
			}
			set
			{
				base[PagesSection._propAutoEventWireup] = value;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x000841A0 File Offset: 0x000831A0
		// (set) Token: 0x06001D02 RID: 7426 RVA: 0x000841B2 File Offset: 0x000831B2
		[ConfigurationProperty("maintainScrollPositionOnPostBack", DefaultValue = false)]
		public bool MaintainScrollPositionOnPostBack
		{
			get
			{
				return (bool)base[PagesSection._propMaintainScrollPosition];
			}
			set
			{
				base[PagesSection._propMaintainScrollPosition] = value;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x000841C5 File Offset: 0x000831C5
		// (set) Token: 0x06001D04 RID: 7428 RVA: 0x000841D7 File Offset: 0x000831D7
		[ConfigurationProperty("pageBaseType", DefaultValue = "System.Web.UI.Page")]
		public string PageBaseType
		{
			get
			{
				return (string)base[PagesSection._propPageBaseType];
			}
			set
			{
				base[PagesSection._propPageBaseType] = value;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x000841E5 File Offset: 0x000831E5
		// (set) Token: 0x06001D06 RID: 7430 RVA: 0x000841F7 File Offset: 0x000831F7
		[ConfigurationProperty("userControlBaseType", DefaultValue = "System.Web.UI.UserControl")]
		public string UserControlBaseType
		{
			get
			{
				return (string)base[PagesSection._propUserControlBaseType];
			}
			set
			{
				base[PagesSection._propUserControlBaseType] = value;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x00084208 File Offset: 0x00083208
		internal Type PageBaseTypeInternal
		{
			get
			{
				if (this._pageBaseType == null && base.ElementInformation.Properties[PagesSection._propPageBaseType.Name].ValueOrigin != PropertyValueOrigin.Default)
				{
					lock (this)
					{
						if (this._pageBaseType == null)
						{
							Type type = ConfigUtil.GetType(this.PageBaseType, "pageBaseType", this);
							ConfigUtil.CheckBaseType(typeof(Page), type, "pageBaseType", this);
							this._pageBaseType = type;
						}
					}
				}
				return this._pageBaseType;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x0008429C File Offset: 0x0008329C
		internal Type UserControlBaseTypeInternal
		{
			get
			{
				if (this._userControlBaseType == null && base.ElementInformation.Properties[PagesSection._propUserControlBaseType.Name].ValueOrigin != PropertyValueOrigin.Default)
				{
					lock (this)
					{
						if (this._userControlBaseType == null)
						{
							Type type = ConfigUtil.GetType(this.UserControlBaseType, "userControlBaseType", this);
							ConfigUtil.CheckBaseType(typeof(UserControl), type, "userControlBaseType", this);
							this._userControlBaseType = type;
						}
					}
				}
				return this._userControlBaseType;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x00084330 File Offset: 0x00083330
		// (set) Token: 0x06001D0A RID: 7434 RVA: 0x00084342 File Offset: 0x00083342
		[ConfigurationProperty("pageParserFilterType", DefaultValue = "")]
		public string PageParserFilterType
		{
			get
			{
				return (string)base[PagesSection._propPageParserFilterType];
			}
			set
			{
				base[PagesSection._propPageParserFilterType] = value;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x00084350 File Offset: 0x00083350
		internal Type PageParserFilterTypeInternal
		{
			get
			{
				if (this._pageParserFilterType == null && !string.IsNullOrEmpty(this.PageParserFilterType))
				{
					Type type = ConfigUtil.GetType(this.PageParserFilterType, "pageParserFilterType", this);
					ConfigUtil.CheckBaseType(typeof(PageParserFilter), type, "pageParserFilterType", this);
					this._pageParserFilterType = type;
				}
				return this._pageParserFilterType;
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000843A8 File Offset: 0x000833A8
		internal PageParserFilter CreateControlTypeFilter()
		{
			Type pageParserFilterTypeInternal = this.PageParserFilterTypeInternal;
			if (pageParserFilterTypeInternal == null)
			{
				return null;
			}
			return (PageParserFilter)HttpRuntime.CreateNonPublicInstance(pageParserFilterTypeInternal);
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x000843CC File Offset: 0x000833CC
		// (set) Token: 0x06001D0E RID: 7438 RVA: 0x000843DE File Offset: 0x000833DE
		[ConfigurationProperty("validateRequest", DefaultValue = true)]
		public bool ValidateRequest
		{
			get
			{
				return (bool)base[PagesSection._propValidateRequest];
			}
			set
			{
				base[PagesSection._propValidateRequest] = value;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001D0F RID: 7439 RVA: 0x000843F1 File Offset: 0x000833F1
		// (set) Token: 0x06001D10 RID: 7440 RVA: 0x00084403 File Offset: 0x00083403
		[ConfigurationProperty("masterPageFile", DefaultValue = "")]
		public string MasterPageFile
		{
			get
			{
				return (string)base[PagesSection._propMasterPageFile];
			}
			set
			{
				base[PagesSection._propMasterPageFile] = value;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x00084414 File Offset: 0x00083414
		internal string MasterPageFileInternal
		{
			get
			{
				if (this._masterPageFile == null)
				{
					string text = this.MasterPageFile;
					if (!string.IsNullOrEmpty(text))
					{
						if (UrlPath.IsAbsolutePhysicalPath(text))
						{
							throw new ConfigurationErrorsException(SR.GetString("Physical_path_not_allowed", new object[] { text }), base.ElementInformation.Properties["masterPageFile"].Source, base.ElementInformation.Properties["masterPageFile"].LineNumber);
						}
						VirtualPath virtualPath;
						try
						{
							virtualPath = VirtualPath.CreateNonRelative(text);
						}
						catch (Exception ex)
						{
							throw new ConfigurationErrorsException(ex.Message, ex, base.ElementInformation.Properties["masterPageFile"].Source, base.ElementInformation.Properties["masterPageFile"].LineNumber);
						}
						if (!Util.VirtualFileExistsWithAssert(virtualPath))
						{
							throw new ConfigurationErrorsException(SR.GetString("FileName_does_not_exist", new object[] { text }), base.ElementInformation.Properties["masterPageFile"].Source, base.ElementInformation.Properties["masterPageFile"].LineNumber);
						}
						string extension = UrlPath.GetExtension(text);
						Type buildProviderTypeFromExtension = CompilationUtil.GetBuildProviderTypeFromExtension(this._virtualPath, extension, BuildProviderAppliesTo.Web, false);
						if (!typeof(MasterPageBuildProvider).IsAssignableFrom(buildProviderTypeFromExtension))
						{
							throw new ConfigurationErrorsException(SR.GetString("Bad_masterPage_ext"), base.ElementInformation.Properties["masterPageFile"].Source, base.ElementInformation.Properties["masterPageFile"].LineNumber);
						}
						text = virtualPath.AppRelativeVirtualPathString;
					}
					else
					{
						text = string.Empty;
					}
					this._masterPageFile = text;
				}
				return this._masterPageFile;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001D12 RID: 7442 RVA: 0x000845DC File Offset: 0x000835DC
		// (set) Token: 0x06001D13 RID: 7443 RVA: 0x000845EE File Offset: 0x000835EE
		[ConfigurationProperty("theme", DefaultValue = "")]
		public string Theme
		{
			get
			{
				return (string)base[PagesSection._propTheme];
			}
			set
			{
				base[PagesSection._propTheme] = value;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001D14 RID: 7444 RVA: 0x000845FC File Offset: 0x000835FC
		internal string ThemeInternal
		{
			get
			{
				string theme = this.Theme;
				if (!this._themeChecked)
				{
					if (!string.IsNullOrEmpty(theme) && !Util.ThemeExists(theme))
					{
						throw new ConfigurationErrorsException(SR.GetString("Page_theme_not_found", new object[] { theme }), base.ElementInformation.Properties["theme"].Source, base.ElementInformation.Properties["theme"].LineNumber);
					}
					this._themeChecked = true;
				}
				return theme;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001D15 RID: 7445 RVA: 0x00084680 File Offset: 0x00083680
		// (set) Token: 0x06001D16 RID: 7446 RVA: 0x00084692 File Offset: 0x00083692
		[ConfigurationProperty("styleSheetTheme", DefaultValue = "")]
		public string StyleSheetTheme
		{
			get
			{
				return (string)base[PagesSection._propStyleSheetTheme];
			}
			set
			{
				base[PagesSection._propStyleSheetTheme] = value;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x000846A0 File Offset: 0x000836A0
		internal string StyleSheetThemeInternal
		{
			get
			{
				string styleSheetTheme = this.StyleSheetTheme;
				if (!this._styleSheetThemeChecked)
				{
					if (!string.IsNullOrEmpty(styleSheetTheme) && !Util.ThemeExists(styleSheetTheme))
					{
						throw new ConfigurationErrorsException(SR.GetString("Page_theme_not_found", new object[] { styleSheetTheme }), base.ElementInformation.Properties["styleSheetTheme"].Source, base.ElementInformation.Properties["styleSheetTheme"].LineNumber);
					}
					this._styleSheetThemeChecked = true;
				}
				return styleSheetTheme;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x00084724 File Offset: 0x00083724
		[ConfigurationProperty("namespaces")]
		public NamespaceCollection Namespaces
		{
			get
			{
				return (NamespaceCollection)base[PagesSection._propNamespaces];
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x00084736 File Offset: 0x00083736
		[ConfigurationProperty("controls")]
		public TagPrefixCollection Controls
		{
			get
			{
				return (TagPrefixCollection)base[PagesSection._propControls];
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001D1A RID: 7450 RVA: 0x00084748 File Offset: 0x00083748
		// (set) Token: 0x06001D1B RID: 7451 RVA: 0x0008475A File Offset: 0x0008375A
		[ConfigurationProperty("maxPageStateFieldLength", DefaultValue = -1)]
		public int MaxPageStateFieldLength
		{
			get
			{
				return (int)base[PagesSection._propMaxPageStateFieldLength];
			}
			set
			{
				base[PagesSection._propMaxPageStateFieldLength] = value;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001D1C RID: 7452 RVA: 0x0008476D File Offset: 0x0008376D
		[ConfigurationProperty("tagMapping")]
		public TagMapCollection TagMapping
		{
			get
			{
				return (TagMapCollection)base[PagesSection._propTagMapping];
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001D1D RID: 7453 RVA: 0x0008477F File Offset: 0x0008377F
		// (set) Token: 0x06001D1E RID: 7454 RVA: 0x00084791 File Offset: 0x00083791
		[ConfigurationProperty("compilationMode", DefaultValue = CompilationMode.Always)]
		public CompilationMode CompilationMode
		{
			get
			{
				return (CompilationMode)base[PagesSection._propCompilationMode];
			}
			set
			{
				base[PagesSection._propCompilationMode] = value;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x000847A4 File Offset: 0x000837A4
		// (set) Token: 0x06001D20 RID: 7456 RVA: 0x000847B6 File Offset: 0x000837B6
		[ConfigurationProperty("viewStateEncryptionMode", DefaultValue = ViewStateEncryptionMode.Auto)]
		public ViewStateEncryptionMode ViewStateEncryptionMode
		{
			get
			{
				return (ViewStateEncryptionMode)base[PagesSection._propViewStateEncryptionMode];
			}
			set
			{
				base[PagesSection._propViewStateEncryptionMode] = value;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001D21 RID: 7457 RVA: 0x000847C9 File Offset: 0x000837C9
		// (set) Token: 0x06001D22 RID: 7458 RVA: 0x000847DB File Offset: 0x000837DB
		[TypeConverter(typeof(TimeSpanSecondsConverter))]
		[ConfigurationProperty("asyncTimeout", DefaultValue = "00:00:45")]
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "10675199.02:48:05.4775807")]
		public TimeSpan AsyncTimeout
		{
			get
			{
				return (TimeSpan)base[PagesSection._propAsyncTimeout];
			}
			set
			{
				base[PagesSection._propAsyncTimeout] = value;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001D23 RID: 7459 RVA: 0x000847EE File Offset: 0x000837EE
		// (set) Token: 0x06001D24 RID: 7460 RVA: 0x00084800 File Offset: 0x00083800
		[ConfigurationProperty("renderAllHiddenFieldsAtTopOfForm", DefaultValue = true)]
		public bool RenderAllHiddenFieldsAtTopOfForm
		{
			get
			{
				return (bool)base[PagesSection._propRenderAllHiddenFieldsAtTopOfForm];
			}
			set
			{
				base[PagesSection._propRenderAllHiddenFieldsAtTopOfForm] = value;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x00084814 File Offset: 0x00083814
		internal TagNamespaceRegisterEntryTable TagNamespaceRegisterEntriesInternal
		{
			get
			{
				if (this._tagNamespaceRegisterEntries == null)
				{
					lock (this)
					{
						if (this._tagNamespaceRegisterEntries == null)
						{
							this.FillInRegisterEntries();
						}
					}
				}
				return this._tagNamespaceRegisterEntries;
			}
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x00084860 File Offset: 0x00083860
		internal void FillInRegisterEntries()
		{
			TagNamespaceRegisterEntryTable tagNamespaceRegisterEntryTable = new TagNamespaceRegisterEntryTable();
			foreach (object obj in PagesSection.DefaultTagNamespaceRegisterEntries)
			{
				TagNamespaceRegisterEntry tagNamespaceRegisterEntry = (TagNamespaceRegisterEntry)obj;
				tagNamespaceRegisterEntryTable[tagNamespaceRegisterEntry.TagPrefix] = new ArrayList(new object[] { tagNamespaceRegisterEntry });
			}
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			foreach (object obj2 in this.Controls)
			{
				TagPrefixInfo tagPrefixInfo = (TagPrefixInfo)obj2;
				if (!string.IsNullOrEmpty(tagPrefixInfo.TagName))
				{
					UserControlRegisterEntry userControlRegisterEntry = new UserControlRegisterEntry(tagPrefixInfo.TagPrefix, tagPrefixInfo.TagName);
					userControlRegisterEntry.ComesFromConfig = true;
					try
					{
						userControlRegisterEntry.UserControlSource = VirtualPath.CreateNonRelative(tagPrefixInfo.Source);
					}
					catch (Exception ex)
					{
						throw new ConfigurationErrorsException(ex.Message, ex, tagPrefixInfo.ElementInformation.Properties["src"].Source, tagPrefixInfo.ElementInformation.Properties["src"].LineNumber);
					}
					hashtable[userControlRegisterEntry.Key] = userControlRegisterEntry;
				}
				else if (!string.IsNullOrEmpty(tagPrefixInfo.Namespace))
				{
					TagNamespaceRegisterEntry tagNamespaceRegisterEntry2 = new TagNamespaceRegisterEntry(tagPrefixInfo.TagPrefix, tagPrefixInfo.Namespace, tagPrefixInfo.Assembly);
					ArrayList arrayList = (ArrayList)tagNamespaceRegisterEntryTable[tagPrefixInfo.TagPrefix];
					if (arrayList == null)
					{
						arrayList = new ArrayList();
						tagNamespaceRegisterEntryTable[tagPrefixInfo.TagPrefix] = arrayList;
					}
					arrayList.Add(tagNamespaceRegisterEntry2);
				}
			}
			this._tagNamespaceRegisterEntries = tagNamespaceRegisterEntryTable;
			this._userControlRegisterEntries = hashtable;
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001D27 RID: 7463 RVA: 0x00084A6C File Offset: 0x00083A6C
		internal static ICollection DefaultTagNamespaceRegisterEntries
		{
			get
			{
				TagNamespaceRegisterEntry tagNamespaceRegisterEntry = new TagNamespaceRegisterEntry("asp", "System.Web.UI.WebControls", "System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				TagNamespaceRegisterEntry tagNamespaceRegisterEntry2 = new TagNamespaceRegisterEntry("mobile", "System.Web.UI.MobileControls", "System.Web.Mobile, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				return new TagNamespaceRegisterEntry[] { tagNamespaceRegisterEntry, tagNamespaceRegisterEntry2 };
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001D28 RID: 7464 RVA: 0x00084AB4 File Offset: 0x00083AB4
		internal Hashtable UserControlRegisterEntriesInternal
		{
			get
			{
				if (this._userControlRegisterEntries == null)
				{
					lock (this)
					{
						if (this._userControlRegisterEntries == null)
						{
							this.FillInRegisterEntries();
						}
					}
				}
				return this._userControlRegisterEntries;
			}
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x00084B00 File Offset: 0x00083B00
		protected override void DeserializeSection(XmlReader reader)
		{
			base.DeserializeSection(reader);
			WebContext webContext = base.EvaluationContext.HostingContext as WebContext;
			if (webContext != null)
			{
				this._virtualPath = VirtualPath.CreateNonRelativeTrailingSlashAllowNull(webContext.Path);
			}
		}

		// Token: 0x04001921 RID: 6433
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001922 RID: 6434
		private static readonly ConfigurationProperty _propBuffer = new ConfigurationProperty("buffer", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001923 RID: 6435
		private static readonly ConfigurationProperty _propEnableSessionState = new ConfigurationProperty("enableSessionState", typeof(string), "true", ConfigurationPropertyOptions.None);

		// Token: 0x04001924 RID: 6436
		private static readonly ConfigurationProperty _propEnableViewState = new ConfigurationProperty("enableViewState", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001925 RID: 6437
		private static readonly ConfigurationProperty _propEnableViewStateMac = new ConfigurationProperty("enableViewStateMac", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001926 RID: 6438
		private static readonly ConfigurationProperty _propEnableEventValidation = new ConfigurationProperty("enableEventValidation", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001927 RID: 6439
		private static readonly ConfigurationProperty _propSmartNavigation = new ConfigurationProperty("smartNavigation", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001928 RID: 6440
		private static readonly ConfigurationProperty _propAutoEventWireup = new ConfigurationProperty("autoEventWireup", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001929 RID: 6441
		private static readonly ConfigurationProperty _propPageBaseType = new ConfigurationProperty("pageBaseType", typeof(string), "System.Web.UI.Page", ConfigurationPropertyOptions.None);

		// Token: 0x0400192A RID: 6442
		private static readonly ConfigurationProperty _propUserControlBaseType = new ConfigurationProperty("userControlBaseType", typeof(string), "System.Web.UI.UserControl", ConfigurationPropertyOptions.None);

		// Token: 0x0400192B RID: 6443
		private static readonly ConfigurationProperty _propValidateRequest = new ConfigurationProperty("validateRequest", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x0400192C RID: 6444
		private static readonly ConfigurationProperty _propMasterPageFile = new ConfigurationProperty("masterPageFile", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x0400192D RID: 6445
		private static readonly ConfigurationProperty _propTheme = new ConfigurationProperty("theme", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x0400192E RID: 6446
		private static readonly ConfigurationProperty _propNamespaces = new ConfigurationProperty("namespaces", typeof(NamespaceCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x0400192F RID: 6447
		private static readonly ConfigurationProperty _propControls = new ConfigurationProperty("controls", typeof(TagPrefixCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x04001930 RID: 6448
		private static readonly ConfigurationProperty _propTagMapping = new ConfigurationProperty("tagMapping", typeof(TagMapCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x04001931 RID: 6449
		private static readonly ConfigurationProperty _propMaxPageStateFieldLength = new ConfigurationProperty("maxPageStateFieldLength", typeof(int), Page.DefaultMaxPageStateFieldLength, ConfigurationPropertyOptions.None);

		// Token: 0x04001932 RID: 6450
		private static readonly ConfigurationProperty _propCompilationMode = new ConfigurationProperty("compilationMode", typeof(CompilationMode), CompilationMode.Always, ConfigurationPropertyOptions.None);

		// Token: 0x04001933 RID: 6451
		private static readonly ConfigurationProperty _propStyleSheetTheme = new ConfigurationProperty("styleSheetTheme", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001934 RID: 6452
		private static readonly ConfigurationProperty _propPageParserFilterType = new ConfigurationProperty("pageParserFilterType", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001935 RID: 6453
		private static readonly ConfigurationProperty _propViewStateEncryptionMode = new ConfigurationProperty("viewStateEncryptionMode", typeof(ViewStateEncryptionMode), ViewStateEncryptionMode.Auto, ConfigurationPropertyOptions.None);

		// Token: 0x04001936 RID: 6454
		private static readonly ConfigurationProperty _propMaintainScrollPosition = new ConfigurationProperty("maintainScrollPositionOnPostBack", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001937 RID: 6455
		private static readonly ConfigurationProperty _propAsyncTimeout = new ConfigurationProperty("asyncTimeout", typeof(TimeSpan), TimeSpan.FromSeconds((double)Page.DefaultAsyncTimeoutSeconds), StdValidatorsAndConverters.TimeSpanSecondsConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.None);

		// Token: 0x04001938 RID: 6456
		private static readonly ConfigurationProperty _propRenderAllHiddenFieldsAtTopOfForm = new ConfigurationProperty("renderAllHiddenFieldsAtTopOfForm", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001939 RID: 6457
		private VirtualPath _virtualPath;

		// Token: 0x0400193A RID: 6458
		private string _masterPageFile;

		// Token: 0x0400193B RID: 6459
		private Type _pageBaseType;

		// Token: 0x0400193C RID: 6460
		private Type _userControlBaseType;

		// Token: 0x0400193D RID: 6461
		private Type _pageParserFilterType;

		// Token: 0x0400193E RID: 6462
		private bool _themeChecked;

		// Token: 0x0400193F RID: 6463
		private bool _styleSheetThemeChecked;

		// Token: 0x04001940 RID: 6464
		private TagNamespaceRegisterEntryTable _tagNamespaceRegisterEntries;

		// Token: 0x04001941 RID: 6465
		private Hashtable _userControlRegisterEntries;
	}
}
