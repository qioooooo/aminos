using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web.Compilation;
using System.Web.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000392 RID: 914
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ControlBuilder
	{
		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06002C68 RID: 11368 RVA: 0x000C64DC File Offset: 0x000C54DC
		public virtual Type BindingContainerType
		{
			get
			{
				if (this.NamingContainerBuilder == null)
				{
					return typeof(Control);
				}
				Type controlType = this.NamingContainerBuilder.ControlType;
				if (typeof(INonBindingContainer).IsAssignableFrom(controlType))
				{
					return this.NamingContainerBuilder.BindingContainerType;
				}
				return this.NamingContainerBuilder.ControlType;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06002C69 RID: 11369 RVA: 0x000C6531 File Offset: 0x000C5531
		internal ICollection EventEntries
		{
			get
			{
				if (this._eventEntries == null)
				{
					return EmptyCollection.Instance;
				}
				return this._eventEntries;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06002C6A RID: 11370 RVA: 0x000C6547 File Offset: 0x000C5547
		private ArrayList EventEntriesInternal
		{
			get
			{
				if (this._eventEntries == null)
				{
					this._eventEntries = new ArrayList();
				}
				return this._eventEntries;
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06002C6B RID: 11371 RVA: 0x000C6562 File Offset: 0x000C5562
		internal ICollection SimplePropertyEntries
		{
			get
			{
				if (this._simplePropertyEntries == null)
				{
					return EmptyCollection.Instance;
				}
				return this._simplePropertyEntries;
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06002C6C RID: 11372 RVA: 0x000C6578 File Offset: 0x000C5578
		internal ArrayList SimplePropertyEntriesInternal
		{
			get
			{
				if (this._simplePropertyEntries == null)
				{
					this._simplePropertyEntries = new ArrayList();
				}
				return this._simplePropertyEntries;
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06002C6D RID: 11373 RVA: 0x000C6593 File Offset: 0x000C5593
		internal ICollection ComplexPropertyEntries
		{
			get
			{
				if (this._complexPropertyEntries == null)
				{
					return EmptyCollection.Instance;
				}
				return this._complexPropertyEntries;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06002C6E RID: 11374 RVA: 0x000C65A9 File Offset: 0x000C55A9
		private ArrayList ComplexPropertyEntriesInternal
		{
			get
			{
				if (this._complexPropertyEntries == null)
				{
					this._complexPropertyEntries = new ArrayList();
				}
				return this._complexPropertyEntries;
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06002C6F RID: 11375 RVA: 0x000C65C4 File Offset: 0x000C55C4
		internal ICollection TemplatePropertyEntries
		{
			get
			{
				if (this._templatePropertyEntries == null)
				{
					return EmptyCollection.Instance;
				}
				return this._templatePropertyEntries;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06002C70 RID: 11376 RVA: 0x000C65DA File Offset: 0x000C55DA
		private ArrayList TemplatePropertyEntriesInternal
		{
			get
			{
				if (this._templatePropertyEntries == null)
				{
					this._templatePropertyEntries = new ArrayList();
				}
				return this._templatePropertyEntries;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06002C71 RID: 11377 RVA: 0x000C65F5 File Offset: 0x000C55F5
		internal ICollection BoundPropertyEntries
		{
			get
			{
				if (this._boundPropertyEntries == null)
				{
					return EmptyCollection.Instance;
				}
				return this._boundPropertyEntries;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06002C72 RID: 11378 RVA: 0x000C660B File Offset: 0x000C560B
		private ArrayList BoundPropertyEntriesInternal
		{
			get
			{
				if (this._boundPropertyEntries == null)
				{
					this._boundPropertyEntries = new ArrayList();
				}
				return this._boundPropertyEntries;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06002C73 RID: 11379 RVA: 0x000C6626 File Offset: 0x000C5626
		internal bool HasFilteredBoundEntries
		{
			get
			{
				return this.flags[512];
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06002C74 RID: 11380 RVA: 0x000C6638 File Offset: 0x000C5638
		internal bool IsNoCompile
		{
			get
			{
				return this.flags[1];
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06002C75 RID: 11381 RVA: 0x000C6646 File Offset: 0x000C5646
		// (set) Token: 0x06002C76 RID: 11382 RVA: 0x000C664E File Offset: 0x000C564E
		internal string SkinID
		{
			get
			{
				return this._skinID;
			}
			set
			{
				this._skinID = value;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06002C77 RID: 11383 RVA: 0x000C6657 File Offset: 0x000C5657
		internal IDictionary AdditionalState
		{
			get
			{
				if (this._additionalState == null)
				{
					this._additionalState = new Dictionary<object, object>();
				}
				return this._additionalState;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06002C78 RID: 11384 RVA: 0x000C6672 File Offset: 0x000C5672
		public Type ControlType
		{
			get
			{
				return this._controlType;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06002C79 RID: 11385 RVA: 0x000C667A File Offset: 0x000C567A
		public IFilterResolutionService CurrentFilterResolutionService
		{
			get
			{
				if (this.ServiceProvider != null)
				{
					return (IFilterResolutionService)this.ServiceProvider.GetService(typeof(IFilterResolutionService));
				}
				return this.TemplateControl;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06002C7A RID: 11386 RVA: 0x000C66A5 File Offset: 0x000C56A5
		public virtual Type DeclareType
		{
			get
			{
				return this._controlType;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06002C7B RID: 11387 RVA: 0x000C66B0 File Offset: 0x000C56B0
		private IDesignerHost DesignerHost
		{
			get
			{
				if (this.InDesigner && this.ParseTimeData != null)
				{
					TemplateParser parser = this.ParseTimeData.Parser;
					if (parser != null)
					{
						return parser.DesignerHost;
					}
				}
				return null;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06002C7C RID: 11388 RVA: 0x000C66E4 File Offset: 0x000C56E4
		private ControlBuilder DefaultPropertyBuilder
		{
			get
			{
				return this.ParseTimeData.DefaultPropertyBuilder;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06002C7D RID: 11389 RVA: 0x000C66F1 File Offset: 0x000C56F1
		public IThemeResolutionService ThemeResolutionService
		{
			get
			{
				if (this.ServiceProvider != null)
				{
					return (IThemeResolutionService)this.ServiceProvider.GetService(typeof(IThemeResolutionService));
				}
				return this.TemplateControl as IThemeResolutionService;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06002C7E RID: 11390 RVA: 0x000C6721 File Offset: 0x000C5721
		private EventDescriptorCollection EventDescriptors
		{
			get
			{
				if (this.ParseTimeData.EventDescriptors == null)
				{
					this.ParseTimeData.EventDescriptors = TypeDescriptor.GetEvents(this._controlType);
				}
				return this.ParseTimeData.EventDescriptors;
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06002C7F RID: 11391 RVA: 0x000C6751 File Offset: 0x000C5751
		// (set) Token: 0x06002C80 RID: 11392 RVA: 0x000C675E File Offset: 0x000C575E
		internal string Filter
		{
			get
			{
				return this.ParseTimeData.Filter;
			}
			set
			{
				this.ParseTimeData.Filter = value;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06002C81 RID: 11393 RVA: 0x000C676C File Offset: 0x000C576C
		protected bool FChildrenAsProperties
		{
			get
			{
				return this.ParseTimeData.ChildrenAsProperties;
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06002C82 RID: 11394 RVA: 0x000C6779 File Offset: 0x000C5779
		protected bool FIsNonParserAccessor
		{
			get
			{
				return this.ParseTimeData.IsNonParserAccessor;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06002C83 RID: 11395 RVA: 0x000C6786 File Offset: 0x000C5786
		public bool HasAspCode
		{
			get
			{
				return this.ParseTimeData.HasAspCode;
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06002C84 RID: 11396 RVA: 0x000C6793 File Offset: 0x000C5793
		// (set) Token: 0x06002C85 RID: 11397 RVA: 0x000C67A0 File Offset: 0x000C57A0
		public string ID
		{
			get
			{
				return this.ParseTimeData.ID;
			}
			set
			{
				this.ParseTimeData.ID = value;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06002C86 RID: 11398 RVA: 0x000C67AE File Offset: 0x000C57AE
		// (set) Token: 0x06002C87 RID: 11399 RVA: 0x000C67BB File Offset: 0x000C57BB
		internal bool IsGeneratedID
		{
			get
			{
				return this.ParseTimeData.IsGeneratedID;
			}
			set
			{
				this.ParseTimeData.IsGeneratedID = value;
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06002C88 RID: 11400 RVA: 0x000C67C9 File Offset: 0x000C57C9
		private bool IgnoreControlProperty
		{
			get
			{
				return this.ParseTimeData.IgnoreControlProperties;
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06002C89 RID: 11401 RVA: 0x000C67D6 File Offset: 0x000C57D6
		protected bool InDesigner
		{
			get
			{
				return !this.IsNoCompile && this.Parser != null && this.Parser.FInDesigner;
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06002C8A RID: 11402 RVA: 0x000C67F7 File Offset: 0x000C57F7
		protected bool InPageTheme
		{
			get
			{
				return this.Parser is PageThemeParser;
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06002C8B RID: 11403 RVA: 0x000C6807 File Offset: 0x000C5807
		internal bool IsControlSkin
		{
			get
			{
				return this.ParentBuilder is FileLevelPageThemeBuilder;
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06002C8C RID: 11404 RVA: 0x000C6817 File Offset: 0x000C5817
		private bool IsHtmlControl
		{
			get
			{
				return this.ParseTimeData.IsHtmlControl;
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06002C8D RID: 11405 RVA: 0x000C6824 File Offset: 0x000C5824
		// (set) Token: 0x06002C8E RID: 11406 RVA: 0x000C6831 File Offset: 0x000C5831
		internal int Line
		{
			get
			{
				return this.ParseTimeData.Line;
			}
			set
			{
				this.ParseTimeData.Line = value;
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06002C8F RID: 11407 RVA: 0x000C683F File Offset: 0x000C583F
		public bool Localize
		{
			get
			{
				return this.ParseTimeData == null || this.ParseTimeData.Localize;
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06002C90 RID: 11408 RVA: 0x000C6858 File Offset: 0x000C5858
		private ControlBuilder NamingContainerBuilder
		{
			get
			{
				if (this.ParseTimeData.NamingContainerSearched)
				{
					return this.ParseTimeData.NamingContainerBuilder;
				}
				if (this.ParentBuilder == null || this.ParentBuilder.ControlType == null)
				{
					this.ParseTimeData.NamingContainerBuilder = null;
				}
				else if (typeof(INamingContainer).IsAssignableFrom(this.ParentBuilder.ControlType))
				{
					this.ParseTimeData.NamingContainerBuilder = this.ParentBuilder;
				}
				else
				{
					this.ParseTimeData.NamingContainerBuilder = this.ParentBuilder.NamingContainerBuilder;
				}
				this.ParseTimeData.NamingContainerSearched = true;
				return this.ParseTimeData.NamingContainerBuilder;
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06002C91 RID: 11409 RVA: 0x000C68FD File Offset: 0x000C58FD
		public Type NamingContainerType
		{
			get
			{
				if (this.NamingContainerBuilder == null)
				{
					return typeof(Control);
				}
				return this.NamingContainerBuilder.ControlType;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06002C92 RID: 11410 RVA: 0x000C691D File Offset: 0x000C591D
		internal CompilationMode CompilationMode
		{
			get
			{
				return this.Parser.CompilationMode;
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06002C93 RID: 11411 RVA: 0x000C692A File Offset: 0x000C592A
		internal ControlBuilder ParentBuilder
		{
			get
			{
				return this.ParseTimeData.ParentBuilder;
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06002C94 RID: 11412 RVA: 0x000C6937 File Offset: 0x000C5937
		protected internal TemplateParser Parser
		{
			get
			{
				return this.ParseTimeData.Parser;
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06002C95 RID: 11413 RVA: 0x000C6944 File Offset: 0x000C5944
		private ControlBuilder.ControlBuilderParseTimeData ParseTimeData
		{
			get
			{
				if (this._parseTimeData == null)
				{
					if (this.IsNoCompile)
					{
						throw new InvalidOperationException(SR.GetString("ControlBuilder_ParseTimeDataNotAvailable"));
					}
					this._parseTimeData = new ControlBuilder.ControlBuilderParseTimeData();
				}
				return this._parseTimeData;
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06002C96 RID: 11414 RVA: 0x000C6977 File Offset: 0x000C5977
		private PropertyDescriptorCollection PropertyDescriptors
		{
			get
			{
				if (this.ParseTimeData.PropertyDescriptors == null)
				{
					this.ParseTimeData.PropertyDescriptors = TypeDescriptor.GetProperties(this._controlType);
				}
				return this.ParseTimeData.PropertyDescriptors;
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06002C97 RID: 11415 RVA: 0x000C69A7 File Offset: 0x000C59A7
		private StringSet PropertyEntries
		{
			get
			{
				if (this.ParseTimeData.PropertyEntries == null)
				{
					this.ParseTimeData.PropertyEntries = new CaseInsensitiveStringSet();
				}
				return this.ParseTimeData.PropertyEntries;
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06002C98 RID: 11416 RVA: 0x000C69D1 File Offset: 0x000C59D1
		internal ArrayList SubBuilders
		{
			get
			{
				if (this._subBuilders == null)
				{
					this._subBuilders = new ArrayList();
				}
				return this._subBuilders;
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06002C99 RID: 11417 RVA: 0x000C69EC File Offset: 0x000C59EC
		public IServiceProvider ServiceProvider
		{
			get
			{
				return this._serviceProvider;
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06002C9A RID: 11418 RVA: 0x000C69F4 File Offset: 0x000C59F4
		private bool SupportsAttributes
		{
			get
			{
				return this.ParseTimeData.SupportsAttributes;
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06002C9B RID: 11419 RVA: 0x000C6A01 File Offset: 0x000C5A01
		public string TagName
		{
			get
			{
				return this._tagName;
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06002C9C RID: 11420 RVA: 0x000C6A09 File Offset: 0x000C5A09
		// (set) Token: 0x06002C9D RID: 11421 RVA: 0x000C6A16 File Offset: 0x000C5A16
		internal VirtualPath VirtualPath
		{
			get
			{
				return this.ParseTimeData.VirtualPath;
			}
			set
			{
				this.ParseTimeData.VirtualPath = value;
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06002C9E RID: 11422 RVA: 0x000C6A24 File Offset: 0x000C5A24
		internal string VirtualPathString
		{
			get
			{
				return VirtualPath.GetVirtualPathString(this.VirtualPath);
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06002C9F RID: 11423 RVA: 0x000C6A34 File Offset: 0x000C5A34
		internal TemplateControl TemplateControl
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				if (httpContext == null)
				{
					return null;
				}
				return httpContext.TemplateControl;
			}
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x000C6A54 File Offset: 0x000C5A54
		private void AddBoundProperty(string filter, string name, string expressionPrefix, string expression, ExpressionBuilder expressionBuilder, object parsedExpressionData, string fieldName, string formatString, bool twoWayBound)
		{
			this.AddBoundProperty(filter, name, expressionPrefix, expression, expressionBuilder, parsedExpressionData, false, fieldName, formatString, twoWayBound);
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x000C6A78 File Offset: 0x000C5A78
		private void AddBoundProperty(string filter, string name, string expressionPrefix, string expression, ExpressionBuilder expressionBuilder, object parsedExpressionData, bool generated, string fieldName, string formatString, bool twoWayBound)
		{
			string id = this.ParseTimeData.ID;
			IDesignerHost designerHost = this.DesignerHost;
			if (string.IsNullOrEmpty(expressionPrefix))
			{
				if (string.IsNullOrEmpty(id))
				{
					if (this.CompilationMode == CompilationMode.Never)
					{
						throw new HttpException(SR.GetString("NoCompileBinding_requires_ID", new object[]
						{
							this._controlType.Name,
							fieldName
						}));
					}
					if (twoWayBound)
					{
						throw new HttpException(SR.GetString("TwoWayBinding_requires_ID", new object[]
						{
							this._controlType.Name,
							fieldName
						}));
					}
				}
				if (!this.flags[8192] && this.ControlType.GetEvent("DataBinding") == null)
				{
					throw new InvalidOperationException(SR.GetString("ControlBuilder_DatabindingRequiresEvent", new object[] { this._controlType.FullName }));
				}
			}
			else if (expressionBuilder == null)
			{
				expressionBuilder = ExpressionBuilder.GetExpressionBuilder(expressionPrefix, this.VirtualPath, designerHost);
			}
			BoundPropertyEntry boundPropertyEntry = new BoundPropertyEntry();
			boundPropertyEntry.Filter = filter;
			boundPropertyEntry.Expression = expression;
			boundPropertyEntry.ExpressionBuilder = expressionBuilder;
			boundPropertyEntry.ExpressionPrefix = expressionPrefix;
			boundPropertyEntry.Generated = generated;
			boundPropertyEntry.FieldName = fieldName;
			boundPropertyEntry.FormatString = formatString;
			boundPropertyEntry.ControlType = this._controlType;
			boundPropertyEntry.ControlID = id;
			boundPropertyEntry.TwoWayBound = twoWayBound;
			boundPropertyEntry.ParsedExpressionData = parsedExpressionData;
			this.FillUpBoundPropertyEntry(boundPropertyEntry, name);
			foreach (object obj in this.BoundPropertyEntriesInternal)
			{
				BoundPropertyEntry boundPropertyEntry2 = (BoundPropertyEntry)obj;
				if (string.Equals(boundPropertyEntry2.Name, boundPropertyEntry.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(boundPropertyEntry2.Filter, boundPropertyEntry.Filter, StringComparison.OrdinalIgnoreCase))
				{
					string text = boundPropertyEntry.Name;
					if (!string.IsNullOrEmpty(boundPropertyEntry.Filter))
					{
						text = boundPropertyEntry.Filter + ":" + text;
					}
					throw new InvalidOperationException(SR.GetString("ControlBuilder_CannotHaveMultipleBoundEntries", new object[] { text, this.ControlType }));
				}
			}
			this.AddBoundProperty(boundPropertyEntry);
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x000C6CB4 File Offset: 0x000C5CB4
		private void AddBoundProperty(BoundPropertyEntry entry)
		{
			this.AddEntry(this.BoundPropertyEntriesInternal, entry);
			if (entry.TwoWayBound)
			{
				this.flags[1024] = true;
			}
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x000C6CDC File Offset: 0x000C5CDC
		private void FillUpBoundPropertyEntry(BoundPropertyEntry entry, string name)
		{
			string text;
			MemberInfo memberInfo = PropertyMapper.GetMemberInfo(this._controlType, name, out text);
			entry.Name = text;
			if (memberInfo != null)
			{
				if (memberInfo is PropertyInfo)
				{
					PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
					if (propertyInfo.GetSetMethod() == null)
					{
						if (!this.SupportsAttributes)
						{
							throw new HttpException(SR.GetString("Property_readonly", new object[] { name }));
						}
						if (entry.TwoWayBound)
						{
							entry.ReadOnlyProperty = true;
						}
						else
						{
							entry.UseSetAttribute = true;
						}
					}
					else
					{
						entry.PropertyInfo = propertyInfo;
						entry.Type = propertyInfo.PropertyType;
					}
				}
				else
				{
					entry.Type = ((FieldInfo)memberInfo).FieldType;
				}
			}
			else
			{
				if (!this.SupportsAttributes)
				{
					throw new HttpException(SR.GetString("Type_doesnt_have_property", new object[]
					{
						this._controlType.FullName,
						name
					}));
				}
				if (entry.TwoWayBound)
				{
					throw new InvalidOperationException(SR.GetString("ControlBuilder_TwoWayBindingNonProperty", new object[]
					{
						name,
						this.ControlType.Name
					}));
				}
				entry.Name = name;
				entry.UseSetAttribute = true;
			}
			if (entry.ParsedExpressionData == null)
			{
				entry.ParseExpression(new ExpressionBuilderContext(this.VirtualPath));
			}
			if (!this.Parser.IgnoreParseErrors && entry.ParsedExpressionData == null && Util.IsWhiteSpaceString(entry.Expression))
			{
				throw new HttpException(SR.GetString("Empty_expression"));
			}
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000C6E50 File Offset: 0x000C5E50
		private void AddCollectionItem(ControlBuilder builder)
		{
			ComplexPropertyEntry complexPropertyEntry = new ComplexPropertyEntry(true);
			complexPropertyEntry.Builder = builder;
			complexPropertyEntry.Filter = string.Empty;
			this.AddEntry(this.ComplexPropertyEntriesInternal, complexPropertyEntry);
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000C6E84 File Offset: 0x000C5E84
		private void AddComplexProperty(string filter, string name, ControlBuilder builder)
		{
			string empty = string.Empty;
			MemberInfo memberInfo = PropertyMapper.GetMemberInfo(this._controlType, name, out empty);
			ComplexPropertyEntry complexPropertyEntry = new ComplexPropertyEntry();
			complexPropertyEntry.Builder = builder;
			complexPropertyEntry.Filter = filter;
			complexPropertyEntry.Name = empty;
			if (memberInfo != null)
			{
				Type type;
				if (memberInfo is PropertyInfo)
				{
					PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
					complexPropertyEntry.PropertyInfo = propertyInfo;
					if (propertyInfo.GetSetMethod() == null)
					{
						complexPropertyEntry.ReadOnly = true;
					}
					this.ValidatePersistable(propertyInfo, false, false, false, filter);
					type = propertyInfo.PropertyType;
				}
				else
				{
					type = ((FieldInfo)memberInfo).FieldType;
				}
				complexPropertyEntry.Type = type;
				this.AddEntry(this.ComplexPropertyEntriesInternal, complexPropertyEntry);
				return;
			}
			throw new HttpException(SR.GetString("Type_doesnt_have_property", new object[]
			{
				this._controlType.FullName,
				name
			}));
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000C6F54 File Offset: 0x000C5F54
		private void AddEntry(ArrayList entries, PropertyEntry entry)
		{
			if (string.Equals(entry.Name, "ID", StringComparison.OrdinalIgnoreCase) && this.flags[8192] && !(entry is SimplePropertyEntry))
			{
				throw new HttpException(SR.GetString("ControlBuilder_IDMustUseAttribute"));
			}
			entry.Index = entries.Count;
			entries.Add(entry);
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000C6FB4 File Offset: 0x000C5FB4
		private void AddProperty(string filter, string name, string value, bool mainDirectiveMode)
		{
			if (this.IgnoreControlProperty)
			{
				return;
			}
			string empty = string.Empty;
			MemberInfo memberInfo = null;
			if (this._controlType != null)
			{
				if (string.Equals(name, "SkinID", StringComparison.OrdinalIgnoreCase) && this.flags[8192])
				{
					if (!string.IsNullOrEmpty(filter))
					{
						throw new InvalidOperationException(SR.GetString("Illegal_Device", new object[] { "SkinID" }));
					}
					this.SkinID = value;
					return;
				}
				else
				{
					memberInfo = PropertyMapper.GetMemberInfo(this._controlType, name, out empty);
				}
			}
			if (memberInfo != null)
			{
				SimplePropertyEntry simplePropertyEntry = new SimplePropertyEntry();
				simplePropertyEntry.Filter = filter;
				simplePropertyEntry.Name = empty;
				simplePropertyEntry.PersistedValue = value;
				Type type;
				if (memberInfo is PropertyInfo)
				{
					PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
					simplePropertyEntry.PropertyInfo = propertyInfo;
					if (propertyInfo.GetSetMethod() == null)
					{
						if (!this.SupportsAttributes)
						{
							throw new HttpException(SR.GetString("Property_readonly", new object[] { name }));
						}
						simplePropertyEntry.UseSetAttribute = true;
						simplePropertyEntry.Name = name;
					}
					this.ValidatePersistable(propertyInfo, simplePropertyEntry.UseSetAttribute, mainDirectiveMode, true, filter);
					type = propertyInfo.PropertyType;
				}
				else
				{
					type = ((FieldInfo)memberInfo).FieldType;
				}
				simplePropertyEntry.Type = type;
				if (simplePropertyEntry.UseSetAttribute)
				{
					simplePropertyEntry.Value = value;
				}
				else
				{
					object obj = PropertyConverter.ObjectFromString(type, memberInfo, value);
					DesignTimePageThemeParser designTimePageThemeParser = this.Parser as DesignTimePageThemeParser;
					if (designTimePageThemeParser != null)
					{
						object[] customAttributes = memberInfo.GetCustomAttributes(typeof(UrlPropertyAttribute), true);
						if (customAttributes.Length > 0)
						{
							string text = obj.ToString();
							if (UrlPath.IsRelativeUrl(text) && !UrlPath.IsAppRelativePath(text))
							{
								obj = designTimePageThemeParser.ThemePhysicalPath + text;
							}
						}
					}
					simplePropertyEntry.Value = obj;
					if (type.IsEnum)
					{
						if (obj == null)
						{
							throw new HttpException(SR.GetString("Invalid_enum_value", new object[]
							{
								value,
								name,
								simplePropertyEntry.Type.FullName
							}));
						}
						simplePropertyEntry.PersistedValue = Enum.Format(type, obj, "G");
					}
					else if (type == typeof(bool) && obj == null)
					{
						simplePropertyEntry.Value = true;
					}
				}
				this.AddEntry(this.SimplePropertyEntriesInternal, simplePropertyEntry);
				return;
			}
			bool flag = false;
			if (StringUtil.StringStartsWithIgnoreCase(name, "on"))
			{
				string text2 = name.Substring(2);
				EventDescriptor eventDescriptor = this.EventDescriptors.Find(text2, true);
				if (eventDescriptor != null)
				{
					if (this.InPageTheme)
					{
						throw new HttpException(SR.GetString("Property_theme_disabled", new object[]
						{
							text2,
							this.ControlType.FullName
						}));
					}
					if (value != null)
					{
						value = value.Trim();
					}
					if (string.IsNullOrEmpty(value))
					{
						throw new HttpException(SR.GetString("Event_handler_cant_be_empty", new object[] { name }));
					}
					if (filter.Length > 0)
					{
						throw new HttpException(SR.GetString("Events_cant_be_filtered", new object[] { filter, name }));
					}
					flag = true;
					if (!this.Parser.PageParserFilterProcessedEventHookupAttribute(this.ID, eventDescriptor.Name, value))
					{
						this.Parser.OnFoundEventHandler(name);
						EventEntry eventEntry = new EventEntry();
						eventEntry.Name = eventDescriptor.Name;
						eventEntry.HandlerType = eventDescriptor.EventType;
						eventEntry.HandlerMethodName = value;
						this.EventEntriesInternal.Add(eventEntry);
					}
				}
			}
			if (!flag)
			{
				if (!this.SupportsAttributes && filter != ControlBuilder.DesignerFilter)
				{
					if (this._controlType != null)
					{
						throw new HttpException(SR.GetString("Type_doesnt_have_property", new object[]
						{
							this._controlType.FullName,
							name
						}));
					}
					throw new HttpException(SR.GetString("Property_doesnt_have_property", new object[] { this.TagName, name }));
				}
				else
				{
					SimplePropertyEntry simplePropertyEntry2 = new SimplePropertyEntry();
					simplePropertyEntry2.Filter = filter;
					simplePropertyEntry2.Name = name;
					simplePropertyEntry2.PersistedValue = value;
					simplePropertyEntry2.UseSetAttribute = true;
					simplePropertyEntry2.Value = value;
					this.AddEntry(this.SimplePropertyEntriesInternal, simplePropertyEntry2);
				}
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000C73C4 File Offset: 0x000C63C4
		private void AddTemplateProperty(string filter, string name, TemplateBuilder builder)
		{
			string empty = string.Empty;
			MemberInfo memberInfo = PropertyMapper.GetMemberInfo(this._controlType, name, out empty);
			bool flag = builder is BindableTemplateBuilder;
			TemplatePropertyEntry templatePropertyEntry = new TemplatePropertyEntry(flag);
			templatePropertyEntry.Builder = builder;
			templatePropertyEntry.Filter = filter;
			templatePropertyEntry.Name = empty;
			Type type = null;
			if (memberInfo != null)
			{
				if (memberInfo is PropertyInfo)
				{
					PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
					templatePropertyEntry.PropertyInfo = propertyInfo;
					this.ValidatePersistable(propertyInfo, false, false, false, filter);
					TemplateContainerAttribute templateContainerAttribute = (TemplateContainerAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(TemplateContainerAttribute), false);
					if (templateContainerAttribute != null)
					{
						if (!typeof(INamingContainer).IsAssignableFrom(templateContainerAttribute.ContainerType))
						{
							throw new HttpException(SR.GetString("Invalid_template_container", new object[]
							{
								name,
								templateContainerAttribute.ContainerType.FullName
							}));
						}
						builder.SetControlType(templateContainerAttribute.ContainerType);
					}
					templatePropertyEntry.Type = propertyInfo.PropertyType;
				}
				else
				{
					type = ((FieldInfo)memberInfo).FieldType;
				}
				templatePropertyEntry.Type = type;
				this.AddEntry(this.TemplatePropertyEntriesInternal, templatePropertyEntry);
				return;
			}
			throw new HttpException(SR.GetString("Type_doesnt_have_property", new object[]
			{
				this._controlType.FullName,
				name
			}));
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x000C750E File Offset: 0x000C650E
		internal void AddSubBuilder(object o)
		{
			this.SubBuilders.Add(o);
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06002CAA RID: 11434 RVA: 0x000C751D File Offset: 0x000C651D
		internal bool HasTwoWayBoundProperties
		{
			get
			{
				return this.flags[1024];
			}
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x000C752F File Offset: 0x000C652F
		public virtual bool AllowWhitespaceLiterals()
		{
			return true;
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x000C7534 File Offset: 0x000C6534
		public virtual void AppendLiteralString(string s)
		{
			if (s == null)
			{
				return;
			}
			if (this.FIsNonParserAccessor || this.FChildrenAsProperties)
			{
				if (this.DefaultPropertyBuilder != null)
				{
					this.DefaultPropertyBuilder.AppendLiteralString(s);
					return;
				}
				s = s.Trim();
				if (this.FChildrenAsProperties && s.StartsWith("<", StringComparison.OrdinalIgnoreCase))
				{
					throw new HttpException(SR.GetString("Literal_content_not_match_property", new object[]
					{
						this._controlType.FullName,
						s
					}));
				}
				if (s.Length != 0)
				{
					throw new HttpException(SR.GetString("Literal_content_not_allowed", new object[]
					{
						this._controlType.FullName,
						s
					}));
				}
				return;
			}
			else
			{
				if (!this.AllowWhitespaceLiterals() && Util.IsWhiteSpaceString(s))
				{
					return;
				}
				if (this.HtmlDecodeLiterals())
				{
					s = HttpUtility.HtmlDecode(s);
				}
				object lastBuilder = this.GetLastBuilder();
				DataBoundLiteralControlBuilder dataBoundLiteralControlBuilder = lastBuilder as DataBoundLiteralControlBuilder;
				if (dataBoundLiteralControlBuilder != null)
				{
					dataBoundLiteralControlBuilder.AddLiteralString(s);
					return;
				}
				this.AddSubBuilder(s);
				return;
			}
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x000C762C File Offset: 0x000C662C
		public virtual void AppendSubBuilder(ControlBuilder subBuilder)
		{
			subBuilder.OnAppendToParentBuilder(this);
			if (this.FChildrenAsProperties)
			{
				if (subBuilder is CodeBlockBuilder)
				{
					throw new HttpException(SR.GetString("Code_not_supported_on_not_controls"));
				}
				if (this.DefaultPropertyBuilder != null)
				{
					this.DefaultPropertyBuilder.AppendSubBuilder(subBuilder);
					return;
				}
				string tagName = subBuilder.TagName;
				if (subBuilder is TemplateBuilder)
				{
					TemplateBuilder templateBuilder = (TemplateBuilder)subBuilder;
					this.AddTemplateProperty(templateBuilder.Filter, tagName, templateBuilder);
					return;
				}
				if (subBuilder is CollectionBuilder)
				{
					if (subBuilder.SubBuilders != null && subBuilder.SubBuilders.Count > 0)
					{
						foreach (object obj in subBuilder.SubBuilders)
						{
							ControlBuilder controlBuilder = (ControlBuilder)obj;
							subBuilder.AddCollectionItem(controlBuilder);
						}
						subBuilder.SubBuilders.Clear();
						this.AddComplexProperty(subBuilder.Filter, tagName, subBuilder);
						return;
					}
				}
				else if (subBuilder is StringPropertyBuilder)
				{
					string text = ((StringPropertyBuilder)subBuilder).Text.Trim();
					if (!string.IsNullOrEmpty(text))
					{
						this.AddComplexProperty(subBuilder.Filter, tagName, subBuilder);
						return;
					}
				}
				else
				{
					this.AddComplexProperty(subBuilder.Filter, tagName, subBuilder);
				}
				return;
			}
			else
			{
				CodeBlockBuilder codeBlockBuilder = subBuilder as CodeBlockBuilder;
				if (codeBlockBuilder != null)
				{
					if (this.ControlType != null && !this.flags[8192])
					{
						throw new HttpException(SR.GetString("Code_not_supported_on_not_controls"));
					}
					if (codeBlockBuilder.BlockType == CodeBlockType.DataBinding)
					{
						if (ControlBuilder.bindExpressionRegex.Match(codeBlockBuilder.Content, 0).Success)
						{
							ControlBuilder controlBuilder2 = this;
							while (controlBuilder2 != null && !(controlBuilder2 is TemplateBuilder))
							{
								controlBuilder2 = controlBuilder2.ParentBuilder;
							}
							if (controlBuilder2 != null && controlBuilder2.ParentBuilder != null && controlBuilder2 is TemplateBuilder)
							{
								throw new HttpException(SR.GetString("DataBoundLiterals_cant_bind"));
							}
						}
						if (this.InDesigner)
						{
							IDictionary dictionary = new ParsedAttributeCollection();
							dictionary.Add("Text", "<%#" + codeBlockBuilder.Content + "%>");
							subBuilder = ControlBuilder.CreateBuilderFromType(this.Parser, this, typeof(DesignerDataBoundLiteralControl), null, null, dictionary, codeBlockBuilder.Line, codeBlockBuilder.VirtualPathString);
						}
						else
						{
							object lastBuilder = this.GetLastBuilder();
							DataBoundLiteralControlBuilder dataBoundLiteralControlBuilder = lastBuilder as DataBoundLiteralControlBuilder;
							bool flag = false;
							if (dataBoundLiteralControlBuilder == null)
							{
								dataBoundLiteralControlBuilder = new DataBoundLiteralControlBuilder();
								dataBoundLiteralControlBuilder.Init(this.Parser, this, typeof(DataBoundLiteralControl), null, null, null);
								dataBoundLiteralControlBuilder.Line = codeBlockBuilder.Line;
								dataBoundLiteralControlBuilder.VirtualPath = codeBlockBuilder.VirtualPath;
								flag = true;
								string text2 = lastBuilder as string;
								if (text2 != null)
								{
									this.SubBuilders.RemoveAt(this.SubBuilders.Count - 1);
									dataBoundLiteralControlBuilder.AddLiteralString(text2);
								}
							}
							dataBoundLiteralControlBuilder.AddDataBindingExpression(codeBlockBuilder);
							if (!flag)
							{
								return;
							}
							subBuilder = dataBoundLiteralControlBuilder;
						}
					}
					else
					{
						this.ParseTimeData.HasAspCode = true;
					}
				}
				if (this.FIsNonParserAccessor)
				{
					throw new HttpException(SR.GetString("Children_not_supported_on_not_controls"));
				}
				this.AddSubBuilder(subBuilder);
				return;
			}
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x000C790C File Offset: 0x000C690C
		internal virtual void BuildChildren(object parentObj)
		{
			if (this._subBuilders != null)
			{
				IEnumerator enumerator = this._subBuilders.GetEnumerator();
				int num = 0;
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					object obj2;
					if (obj is string)
					{
						obj2 = new LiteralControl((string)obj);
						goto IL_0135;
					}
					if (!(obj is CodeBlockBuilder))
					{
						ControlBuilder controlBuilder = (ControlBuilder)obj;
						controlBuilder.SetServiceProvider(this.ServiceProvider);
						try
						{
							obj2 = controlBuilder.BuildObject(this.flags[32768]);
							if (!this.InDesigner)
							{
								UserControl userControl = obj2 as UserControl;
								if (userControl != null)
								{
									Control control = parentObj as Control;
									userControl.InitializeAsUserControl(control.Page);
								}
							}
						}
						finally
						{
							controlBuilder.SetServiceProvider(null);
						}
						goto IL_0135;
					}
					if (this.InDesigner)
					{
						CodeBlockBuilder codeBlockBuilder = (CodeBlockBuilder)obj;
						string text;
						switch (codeBlockBuilder.BlockType)
						{
						case CodeBlockType.Code:
							text = "<%" + codeBlockBuilder.Content + "%>";
							break;
						case CodeBlockType.Expression:
							text = "<%=" + codeBlockBuilder.Content + "%>";
							break;
						case CodeBlockType.DataBinding:
							text = "<%#" + codeBlockBuilder.Content + "%>";
							break;
						default:
							goto IL_0141;
						}
						obj2 = new LiteralControl(text);
						goto IL_0135;
					}
					IL_0141:
					num++;
					continue;
					IL_0135:
					((IParserAccessor)parentObj).AddParsedSubObject(obj2);
					goto IL_0141;
				}
			}
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000C7A7C File Offset: 0x000C6A7C
		public virtual object BuildObject()
		{
			return this.BuildObjectInternal();
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000C7A84 File Offset: 0x000C6A84
		internal object BuildObject(bool shouldApplyTheme)
		{
			if (this.flags[32768] != shouldApplyTheme)
			{
				this.flags[32768] = shouldApplyTheme;
			}
			return this.BuildObject();
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000C7AB0 File Offset: 0x000C6AB0
		internal object BuildObjectInternal()
		{
			if (!this.flags[2])
			{
				ConstructorNeedsTagAttribute constructorNeedsTagAttribute = (ConstructorNeedsTagAttribute)TypeDescriptor.GetAttributes(this.ControlType)[typeof(ConstructorNeedsTagAttribute)];
				if (constructorNeedsTagAttribute != null && constructorNeedsTagAttribute.NeedsTag)
				{
					this.flags[4] = true;
				}
				this.flags[2] = true;
			}
			object obj;
			if (this.flags[4])
			{
				object[] array = new object[] { this.TagName };
				obj = HttpRuntime.CreatePublicInstance(this._controlType, array);
			}
			else
			{
				obj = HttpRuntime.FastCreatePublicInstance(this._controlType);
			}
			if (this.flags[32768])
			{
				obj = this.GetThemedObject(obj);
			}
			this.InitObject(obj);
			return obj;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000C7B6D File Offset: 0x000C6B6D
		public virtual void CloseControl()
		{
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x000C7B70 File Offset: 0x000C6B70
		internal static ParsedAttributeCollection ConvertDictionaryToParsedAttributeCollection(IDictionary attribs)
		{
			if (attribs is ParsedAttributeCollection)
			{
				return (ParsedAttributeCollection)attribs;
			}
			ParsedAttributeCollection parsedAttributeCollection = new ParsedAttributeCollection();
			foreach (object obj in attribs)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				parsedAttributeCollection.AddFilteredAttribute(string.Empty, dictionaryEntry.Key.ToString(), dictionaryEntry.Value.ToString());
			}
			return parsedAttributeCollection;
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x000C7BF8 File Offset: 0x000C6BF8
		internal ControlBuilder CreateChildBuilder(string filter, string tagName, IDictionary attribs, TemplateParser parser, ControlBuilder parentBuilder, string id, int line, VirtualPath virtualPath, ref Type childType, bool defaultProperty)
		{
			ControlBuilder controlBuilder;
			if (this.FChildrenAsProperties)
			{
				if (this.DefaultPropertyBuilder != null)
				{
					PropertyInfo property = this._controlType.GetProperty(tagName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
					if (property != null)
					{
						controlBuilder = this.GetChildPropertyBuilder(tagName, attribs, ref childType, parser, false);
						if (this.DefaultPropertyBuilder.SubBuilders.Count > 0)
						{
							object[] customAttributes = this.ControlType.GetCustomAttributes(typeof(ParseChildrenAttribute), true);
							ParseChildrenAttribute parseChildrenAttribute = (ParseChildrenAttribute)customAttributes[0];
							throw new HttpException(SR.GetString("Cant_use_default_items_and_filtered_collection", new object[]
							{
								this._controlType.FullName,
								parseChildrenAttribute.DefaultProperty
							}));
						}
						this.ParseTimeData.DefaultPropertyBuilder = null;
					}
					else
					{
						controlBuilder = this.DefaultPropertyBuilder.CreateChildBuilder(filter, tagName, attribs, parser, parentBuilder, id, line, virtualPath, ref childType, false);
					}
				}
				else
				{
					controlBuilder = this.GetChildPropertyBuilder(tagName, attribs, ref childType, parser, defaultProperty);
				}
			}
			else
			{
				string text = Util.CreateFilteredName(filter, tagName);
				childType = this.GetChildControlType(text, attribs);
				if (childType == null)
				{
					return null;
				}
				controlBuilder = ControlBuilder.CreateBuilderFromType(parser, parentBuilder, childType, text, id, attribs, line, this.VirtualPathString);
			}
			if (controlBuilder == null)
			{
				return null;
			}
			controlBuilder.Filter = filter;
			controlBuilder.SetParentBuilder((parentBuilder != null) ? parentBuilder : this);
			return controlBuilder;
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000C7D38 File Offset: 0x000C6D38
		public static ControlBuilder CreateBuilderFromType(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string id, IDictionary attribs, int line, string sourceFileName)
		{
			ControlBuilder controlBuilder = ControlBuilder.CreateBuilderFromType(type);
			controlBuilder.Line = line;
			controlBuilder.VirtualPath = VirtualPath.CreateAllowNull(sourceFileName);
			controlBuilder.Init(parser, parentBuilder, type, tagName, id, attribs);
			return controlBuilder;
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000C7D70 File Offset: 0x000C6D70
		private static ControlBuilder CreateBuilderFromType(Type type)
		{
			if (ControlBuilder.s_controlBuilderFactoryCache == null)
			{
				ControlBuilder.s_controlBuilderFactoryGenerator = new FactoryGenerator();
				ControlBuilder.s_controlBuilderFactoryCache = Hashtable.Synchronized(new Hashtable());
				ControlBuilder.s_controlBuilderFactoryCache[typeof(Content)] = new ContentBuilderInternalFactory();
				ControlBuilder.s_controlBuilderFactoryCache[typeof(ContentPlaceHolder)] = new ContentPlaceHolderBuilderFactory();
			}
			IWebObjectFactory webObjectFactory = (IWebObjectFactory)ControlBuilder.s_controlBuilderFactoryCache[type];
			if (webObjectFactory == null)
			{
				ControlBuilderAttribute controlBuilderAttribute = ControlBuilder.GetControlBuilderAttribute(type);
				if (controlBuilderAttribute != null)
				{
					Util.CheckAssignableType(typeof(ControlBuilder), controlBuilderAttribute.BuilderType);
					if (controlBuilderAttribute.BuilderType.IsPublic)
					{
						webObjectFactory = ControlBuilder.s_controlBuilderFactoryGenerator.CreateFactory(controlBuilderAttribute.BuilderType);
					}
					else
					{
						webObjectFactory = new ControlBuilder.ReflectionBasedControlBuilderFactory(controlBuilderAttribute.BuilderType);
					}
				}
				else
				{
					webObjectFactory = ControlBuilder.s_defaultControlBuilderFactory;
				}
				ControlBuilder.s_controlBuilderFactoryCache[type] = webObjectFactory;
			}
			return (ControlBuilder)webObjectFactory.CreateInstance();
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000C7E50 File Offset: 0x000C6E50
		private static ControlBuilderAttribute GetControlBuilderAttribute(Type controlType)
		{
			ControlBuilderAttribute controlBuilderAttribute = null;
			object[] customAttributes = controlType.GetCustomAttributes(typeof(ControlBuilderAttribute), true);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				controlBuilderAttribute = (ControlBuilderAttribute)customAttributes[0];
			}
			return controlBuilderAttribute;
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000C7E84 File Offset: 0x000C6E84
		private ControlBuilder GetChildPropertyBuilder(string tagName, IDictionary attribs, ref Type childType, TemplateParser templateParser, bool defaultProperty)
		{
			PropertyInfo property = this._controlType.GetProperty(tagName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			if (property == null)
			{
				throw new HttpException(SR.GetString("Type_doesnt_have_property", new object[]
				{
					this._controlType.FullName,
					tagName
				}));
			}
			childType = property.PropertyType;
			ControlBuilder controlBuilder = null;
			if (typeof(ICollection).IsAssignableFrom(childType))
			{
				IgnoreUnknownContentAttribute ignoreUnknownContentAttribute = (IgnoreUnknownContentAttribute)Attribute.GetCustomAttribute(property, typeof(IgnoreUnknownContentAttribute), true);
				controlBuilder = new CollectionBuilder(ignoreUnknownContentAttribute != null);
			}
			else if (typeof(ITemplate).IsAssignableFrom(childType))
			{
				bool flag = false;
				bool flag2 = true;
				object[] customAttributes = property.GetCustomAttributes(typeof(TemplateContainerAttribute), false);
				if (customAttributes != null && customAttributes.Length > 0)
				{
					flag = ((TemplateContainerAttribute)customAttributes[0]).BindingDirection == BindingDirection.TwoWay;
				}
				object[] customAttributes2 = property.GetCustomAttributes(typeof(TemplateInstanceAttribute), false);
				if (customAttributes2 != null && customAttributes2.Length > 0)
				{
					flag2 = ((TemplateInstanceAttribute)customAttributes2[0]).Instances == TemplateInstance.Multiple;
				}
				if (flag)
				{
					controlBuilder = new BindableTemplateBuilder();
				}
				else
				{
					controlBuilder = new TemplateBuilder();
				}
				if (controlBuilder is TemplateBuilder)
				{
					((TemplateBuilder)controlBuilder).AllowMultipleInstances = flag2;
					if (this.InDesigner)
					{
						((TemplateBuilder)controlBuilder).SetDesignerHost(templateParser.DesignerHost);
					}
				}
			}
			else if (childType == typeof(string))
			{
				PersistenceModeAttribute persistenceModeAttribute = (PersistenceModeAttribute)Attribute.GetCustomAttribute(property, typeof(PersistenceModeAttribute), true);
				if ((persistenceModeAttribute == null || persistenceModeAttribute.Mode == PersistenceMode.Attribute) && !defaultProperty)
				{
					throw new HttpException(SR.GetString("ControlBuilder_CannotHaveComplexString", new object[]
					{
						this._controlType.FullName,
						tagName
					}));
				}
				controlBuilder = new StringPropertyBuilder();
			}
			if (controlBuilder != null)
			{
				controlBuilder.Line = this.Line;
				controlBuilder.VirtualPath = this.VirtualPath;
				controlBuilder.Init(this.Parser, this, null, tagName, null, attribs);
				return controlBuilder;
			}
			return ControlBuilder.CreateBuilderFromType(this.Parser, this, childType, tagName, null, attribs, this.Line, this.VirtualPathString);
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x000C8090 File Offset: 0x000C7090
		public virtual Type GetChildControlType(string tagName, IDictionary attribs)
		{
			return null;
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x000C8094 File Offset: 0x000C7094
		internal ICollection GetFilteredPropertyEntrySet(ICollection entries)
		{
			IDictionary dictionary = new HybridDictionary(true);
			IFilterResolutionService currentFilterResolutionService = this.CurrentFilterResolutionService;
			if (currentFilterResolutionService != null)
			{
				using (IEnumerator enumerator = entries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						PropertyEntry propertyEntry = (PropertyEntry)obj;
						if (!dictionary.Contains(propertyEntry.Name))
						{
							string filter = propertyEntry.Filter;
							if (string.IsNullOrEmpty(filter) || currentFilterResolutionService.EvaluateFilter(filter))
							{
								dictionary[propertyEntry.Name] = propertyEntry;
							}
						}
					}
					goto IL_00D0;
				}
			}
			foreach (object obj2 in entries)
			{
				PropertyEntry propertyEntry2 = (PropertyEntry)obj2;
				if (string.IsNullOrEmpty(propertyEntry2.Filter))
				{
					dictionary[propertyEntry2.Name] = propertyEntry2;
				}
			}
			IL_00D0:
			return dictionary.Values;
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x000C8194 File Offset: 0x000C7194
		private bool HasFilteredEntries(ICollection entries)
		{
			foreach (object obj in entries)
			{
				PropertyEntry propertyEntry = (PropertyEntry)obj;
				if (propertyEntry.Filter.Length > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x000C81F8 File Offset: 0x000C71F8
		internal object GetLastBuilder()
		{
			if (this.SubBuilders.Count == 0)
			{
				return null;
			}
			return this.SubBuilders[this.SubBuilders.Count - 1];
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x000C8221 File Offset: 0x000C7221
		public ObjectPersistData GetObjectPersistData()
		{
			return new ObjectPersistData(this, this.Parser.RootBuilder.BuiltObjects);
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x000C8239 File Offset: 0x000C7239
		public virtual bool HasBody()
		{
			return true;
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x000C823C File Offset: 0x000C723C
		public virtual bool HtmlDecodeLiterals()
		{
			return false;
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x000C8240 File Offset: 0x000C7240
		public virtual void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string id, IDictionary attribs)
		{
			if (parser != null && parser.ControlBuilderInterceptor != null)
			{
				parser.ControlBuilderInterceptor.PreControlBuilderInit(this, parser, parentBuilder, type, tagName, id, attribs, this.AdditionalState);
			}
			this.ParseTimeData.Parser = parser;
			this.ParseTimeData.ParentBuilder = parentBuilder;
			if (parser != null)
			{
				this.ParseTimeData.IgnoreControlProperties = parser.IgnoreControlProperties;
			}
			this._tagName = tagName;
			if (type != null)
			{
				this._controlType = type;
				this.flags[8192] = typeof(Control).IsAssignableFrom(this._controlType);
				this.ID = id;
				ParseChildrenAttribute parseChildrenAttribute = ControlBuilder.GetParseChildrenAttribute(type);
				if (!typeof(IParserAccessor).IsAssignableFrom(type))
				{
					this.ParseTimeData.IsNonParserAccessor = true;
					this.ParseTimeData.ChildrenAsProperties = true;
				}
				else if (parseChildrenAttribute != null)
				{
					this.ParseTimeData.ChildrenAsProperties = parseChildrenAttribute.ChildrenAsProperties;
				}
				if (this.FChildrenAsProperties && parseChildrenAttribute != null && parseChildrenAttribute.DefaultProperty.Length != 0)
				{
					Type type2 = null;
					this.ParseTimeData.DefaultPropertyBuilder = this.CreateChildBuilder(string.Empty, parseChildrenAttribute.DefaultProperty, null, parser, null, null, this.Line, this.VirtualPath, ref type2, true);
				}
				this.ParseTimeData.IsHtmlControl = typeof(HtmlControl).IsAssignableFrom(this._controlType);
				this.ParseTimeData.SupportsAttributes = typeof(IAttributeAccessor).IsAssignableFrom(this._controlType);
			}
			else
			{
				this.flags[8192] = false;
			}
			if (attribs != null)
			{
				this.PreprocessAttributes(ControlBuilder.ConvertDictionaryToParsedAttributeCollection(attribs));
			}
			if (this.InPageTheme)
			{
				ControlBuilder currentSkinBuilder = ((PageThemeParser)parser).CurrentSkinBuilder;
				if (currentSkinBuilder != null && currentSkinBuilder.ControlType == this.ControlType && string.Equals(currentSkinBuilder.SkinID, this.SkinID, StringComparison.OrdinalIgnoreCase))
				{
					throw new InvalidOperationException(SR.GetString("Cannot_set_recursive_skin", new object[] { currentSkinBuilder.ControlType.Name }));
				}
			}
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x000C8434 File Offset: 0x000C7434
		private static ParseChildrenAttribute GetParseChildrenAttribute(Type controlType)
		{
			ParseChildrenAttribute parseChildrenAttribute = (ParseChildrenAttribute)ControlBuilder.s_parseChildrenAttributeCache[controlType];
			if (parseChildrenAttribute == null)
			{
				object[] customAttributes = controlType.GetCustomAttributes(typeof(ParseChildrenAttribute), true);
				if (customAttributes != null && customAttributes.Length > 0)
				{
					parseChildrenAttribute = (ParseChildrenAttribute)customAttributes[0];
				}
				if (parseChildrenAttribute == null)
				{
					parseChildrenAttribute = ControlBuilder.s_markerParseChildrenAttribute;
				}
				lock (ControlBuilder.s_parseChildrenAttributeCache.SyncRoot)
				{
					ControlBuilder.s_parseChildrenAttributeCache[controlType] = parseChildrenAttribute;
				}
			}
			if (parseChildrenAttribute == ControlBuilder.s_markerParseChildrenAttribute)
			{
				return null;
			}
			return parseChildrenAttribute;
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x000C84C4 File Offset: 0x000C74C4
		private void DoInitObjectOptimizations(object obj)
		{
			this.flags[16] = typeof(ICollection).IsAssignableFrom(this.ControlType);
			this.flags[32] = typeof(IParserAccessor).IsAssignableFrom(obj.GetType());
			if (this._simplePropertyEntries != null)
			{
				this.flags[64] = this.HasFilteredEntries(this._simplePropertyEntries);
			}
			if (this._complexPropertyEntries != null)
			{
				this.flags[128] = this.HasFilteredEntries(this._complexPropertyEntries);
			}
			if (this._templatePropertyEntries != null)
			{
				this.flags[256] = this.HasFilteredEntries(this._templatePropertyEntries);
			}
			if (this._boundPropertyEntries != null)
			{
				this.flags[512] = this.HasFilteredEntries(this._boundPropertyEntries);
			}
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x000C85A4 File Offset: 0x000C75A4
		internal virtual object GetThemedObject(object obj)
		{
			Control control = obj as Control;
			if (control == null)
			{
				return obj;
			}
			IThemeResolutionService themeResolutionService = this.ThemeResolutionService;
			if (themeResolutionService != null)
			{
				if (!string.IsNullOrEmpty(this.SkinID))
				{
					control.SkinID = this.SkinID;
				}
				ThemeProvider stylesheetThemeProvider = themeResolutionService.GetStylesheetThemeProvider();
				SkinBuilder skinBuilder = null;
				if (stylesheetThemeProvider != null)
				{
					skinBuilder = stylesheetThemeProvider.GetSkinBuilder(control);
					if (skinBuilder != null)
					{
						try
						{
							skinBuilder.SetServiceProvider(this.ServiceProvider);
							return skinBuilder.ApplyTheme();
						}
						finally
						{
							skinBuilder.SetServiceProvider(null);
						}
						return control;
					}
				}
			}
			return control;
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x000C862C File Offset: 0x000C762C
		internal virtual void InitObject(object obj)
		{
			this.EnsureEntriesSorted();
			if (!this.flags[8])
			{
				this.DoInitObjectOptimizations(obj);
				this.flags[8] = true;
			}
			Control control = obj as Control;
			if (control != null)
			{
				if (this.InDesigner)
				{
					control.SetDesignMode();
				}
				if (this.SkinID != null)
				{
					control.SkinID = this.SkinID;
				}
				if (!this.InDesigner && this.TemplateControl != null)
				{
					control.ApplyStyleSheetSkin(this.TemplateControl.Page);
				}
			}
			this.InitSimpleProperties(obj);
			if (this.flags[16])
			{
				this.InitCollectionsComplexProperties(obj);
			}
			else
			{
				this.InitComplexProperties(obj);
			}
			if (this.InDesigner)
			{
				if (control != null)
				{
					if (this.Parser.DesignTimeDataBindHandler != null)
					{
						control.DataBinding += this.Parser.DesignTimeDataBindHandler;
					}
					control.SetControlBuilder(this);
				}
				this.Parser.RootBuilder.BuiltObjects[obj] = this;
			}
			this.InitBoundProperties(obj);
			if (this.flags[32])
			{
				this.BuildChildren(obj);
			}
			this.InitTemplateProperties(obj);
			if (control != null)
			{
				this.BindFieldToControl(control);
			}
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x000C874C File Offset: 0x000C774C
		private void InitSimpleProperties(object obj)
		{
			if (this._simplePropertyEntries == null)
			{
				return;
			}
			ICollection collection;
			if (this.flags[64])
			{
				collection = this.GetFilteredPropertyEntrySet(this.SimplePropertyEntries);
			}
			else
			{
				collection = this.SimplePropertyEntries;
			}
			foreach (object obj2 in collection)
			{
				SimplePropertyEntry simplePropertyEntry = (SimplePropertyEntry)obj2;
				this.SetSimpleProperty(simplePropertyEntry, obj);
			}
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x000C87D0 File Offset: 0x000C77D0
		internal void SetSimpleProperty(SimplePropertyEntry entry, object obj)
		{
			if (entry.UseSetAttribute)
			{
				((IAttributeAccessor)obj).SetAttribute(entry.Name, entry.Value.ToString());
				return;
			}
			try
			{
				PropertyMapper.SetMappedPropertyValue(obj, entry.Name, entry.Value, this.InDesigner);
			}
			catch (Exception ex)
			{
				throw new HttpException(SR.GetString("Cannot_set_property", new object[] { entry.PersistedValue, entry.Name }), ex);
			}
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x000C8858 File Offset: 0x000C7858
		private void InitCollectionsComplexProperties(object obj)
		{
			if (this._complexPropertyEntries == null)
			{
				return;
			}
			foreach (object obj2 in this.ComplexPropertyEntries)
			{
				ComplexPropertyEntry complexPropertyEntry = (ComplexPropertyEntry)obj2;
				try
				{
					ControlBuilder builder = complexPropertyEntry.Builder;
					builder.SetServiceProvider(this.ServiceProvider);
					object obj3;
					try
					{
						obj3 = builder.BuildObject(this.flags[32768]);
					}
					finally
					{
						builder.SetServiceProvider(null);
					}
					object[] array = new object[] { obj3 };
					MethodInfo method = this.ControlType.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { obj3.GetType() }, null);
					if (method == null)
					{
						throw new InvalidOperationException(SR.GetString("ControlBuilder_CollectionHasNoAddMethod", new object[] { this.TagName }));
					}
					Util.InvokeMethod(method, obj, array);
				}
				catch (Exception ex)
				{
					throw new HttpException(SR.GetString("Cannot_add_value_not_collection", new object[] { this.TagName, ex.Message }), ex);
				}
			}
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x000C89A8 File Offset: 0x000C79A8
		private void InitComplexProperties(object obj)
		{
			if (this._complexPropertyEntries == null)
			{
				return;
			}
			ICollection collection;
			if (this.flags[128])
			{
				collection = this.GetFilteredPropertyEntrySet(this.ComplexPropertyEntries);
			}
			else
			{
				collection = this.ComplexPropertyEntries;
			}
			foreach (object obj2 in collection)
			{
				ComplexPropertyEntry complexPropertyEntry = (ComplexPropertyEntry)obj2;
				if (complexPropertyEntry.ReadOnly)
				{
					try
					{
						object property = FastPropertyAccessor.GetProperty(obj, complexPropertyEntry.Name, this.InDesigner);
						complexPropertyEntry.Builder.SetServiceProvider(this.ServiceProvider);
						try
						{
							if (complexPropertyEntry.Builder.flags[32768] != this.flags[32768])
							{
								complexPropertyEntry.Builder.flags[32768] = this.flags[32768];
							}
							complexPropertyEntry.Builder.InitObject(property);
						}
						finally
						{
							complexPropertyEntry.Builder.SetServiceProvider(null);
						}
						continue;
					}
					catch (Exception ex)
					{
						throw new HttpException(SR.GetString("Cannot_init", new object[] { complexPropertyEntry.Name, ex.Message }), ex);
					}
				}
				try
				{
					ControlBuilder builder = complexPropertyEntry.Builder;
					object obj3 = null;
					builder.SetServiceProvider(this.ServiceProvider);
					try
					{
						obj3 = builder.BuildObject(this.flags[32768]);
					}
					finally
					{
						builder.SetServiceProvider(null);
					}
					FastPropertyAccessor.SetProperty(obj, complexPropertyEntry.Name, obj3, this.InDesigner);
				}
				catch (Exception ex2)
				{
					throw new HttpException(SR.GetString("Cannot_set_property", new object[] { this.TagName, complexPropertyEntry.Name }), ex2);
				}
			}
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x000C8BF0 File Offset: 0x000C7BF0
		private void InitBoundProperties(object obj)
		{
			if (this._boundPropertyEntries == null)
			{
				return;
			}
			DataBindingCollection dataBindingCollection = null;
			IAttributeAccessor attributeAccessor = null;
			ICollection collection;
			if (this.flags[512])
			{
				collection = this.GetFilteredPropertyEntrySet(this.BoundPropertyEntries);
			}
			else
			{
				collection = this.BoundPropertyEntries;
			}
			foreach (object obj2 in collection)
			{
				BoundPropertyEntry boundPropertyEntry = (BoundPropertyEntry)obj2;
				if (!boundPropertyEntry.TwoWayBound || !(this is BindableTemplateBuilder) || !this.InDesigner)
				{
					this.InitBoundProperty(obj, boundPropertyEntry, ref dataBindingCollection, ref attributeAccessor);
				}
			}
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x000C8CA0 File Offset: 0x000C7CA0
		private void InitBoundProperty(object obj, BoundPropertyEntry entry, ref DataBindingCollection dataBindings, ref IAttributeAccessor attributeAccessor)
		{
			string text = ((entry.ExpressionPrefix == null) ? string.Empty : entry.ExpressionPrefix.Trim());
			if (this.InDesigner)
			{
				if (string.IsNullOrEmpty(text))
				{
					if (dataBindings == null && obj is IDataBindingsAccessor)
					{
						dataBindings = ((IDataBindingsAccessor)obj).DataBindings;
					}
					dataBindings.Add(new DataBinding(entry.Name, entry.Type, entry.Expression.Trim()));
					return;
				}
				if (obj is IExpressionsAccessor)
				{
					string text2 = ((entry.Expression == null) ? string.Empty : entry.Expression.Trim());
					((IExpressionsAccessor)obj).Expressions.Add(new ExpressionBinding(entry.Name, entry.Type, text, text2, entry.Generated, entry.ParsedExpressionData));
					return;
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(text))
				{
					ExpressionBuilder expressionBuilder = entry.ExpressionBuilder;
					if (!expressionBuilder.SupportsEvaluate)
					{
						return;
					}
					string name = entry.Name;
					ExpressionBuilderContext expressionBuilderContext;
					if (this.TemplateControl != null)
					{
						expressionBuilderContext = new ExpressionBuilderContext(this.TemplateControl);
					}
					else
					{
						expressionBuilderContext = new ExpressionBuilderContext(this.VirtualPath);
					}
					object obj2 = expressionBuilder.EvaluateExpression(obj, entry, entry.ParsedExpressionData, expressionBuilderContext);
					if (entry.UseSetAttribute)
					{
						if (attributeAccessor == null)
						{
							attributeAccessor = (IAttributeAccessor)obj;
						}
						attributeAccessor.SetAttribute(name, obj2.ToString());
						return;
					}
					try
					{
						PropertyMapper.SetMappedPropertyValue(obj, name, obj2, this.InDesigner);
						return;
					}
					catch (Exception ex)
					{
						throw new HttpException(SR.GetString("Cannot_set_property", new object[]
						{
							entry.ExpressionPrefix + ":" + entry.Expression,
							name
						}), ex);
					}
				}
				((Control)obj).DataBinding += this.DataBindingMethod;
			}
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x000C8E64 File Offset: 0x000C7E64
		private void DataBindingMethod(object sender, EventArgs e)
		{
			bool flag = this is BindableTemplateBuilder;
			bool flag2 = this is TemplateBuilder;
			bool flag3 = true;
			Control control = null;
			ICollection collection;
			if (!this.flags[512])
			{
				collection = this.BoundPropertyEntries;
			}
			else
			{
				ServiceContainer serviceContainer = new ServiceContainer();
				serviceContainer.AddService(typeof(IFilterResolutionService), this.TemplateControl);
				try
				{
					this.SetServiceProvider(serviceContainer);
					collection = this.GetFilteredPropertyEntrySet(this.BoundPropertyEntries);
				}
				finally
				{
					this.SetServiceProvider(null);
				}
			}
			foreach (object obj in collection)
			{
				BoundPropertyEntry boundPropertyEntry = (BoundPropertyEntry)obj;
				if ((!boundPropertyEntry.TwoWayBound || (!flag && !boundPropertyEntry.ReadOnlyProperty)) && (boundPropertyEntry.TwoWayBound || !flag2) && boundPropertyEntry.IsDataBindingEntry)
				{
					if (flag3)
					{
						flag3 = false;
						if (this._bindingContainerDescriptor == null)
						{
							this._bindingContainerDescriptor = TypeDescriptor.GetProperties(typeof(Control))["BindingContainer"];
						}
						object value = this._bindingContainerDescriptor.GetValue(sender);
						control = value as Control;
						if (control.Page.GetDataItem() == null)
						{
							break;
						}
					}
					object obj2 = control.TemplateControl.Eval(boundPropertyEntry.FieldName, boundPropertyEntry.FormatString);
					string text;
					MemberInfo memberInfo = PropertyMapper.GetMemberInfo(boundPropertyEntry.ControlType, boundPropertyEntry.Name, out text);
					if (!boundPropertyEntry.Type.IsValueType || obj2 != null)
					{
						object obj3 = obj2;
						if (boundPropertyEntry.Type == typeof(string))
						{
							obj3 = Convert.ToString(obj2, CultureInfo.CurrentCulture);
						}
						else if (obj2 != null && !boundPropertyEntry.Type.IsAssignableFrom(obj2.GetType()))
						{
							obj3 = PropertyConverter.ObjectFromString(boundPropertyEntry.Type, memberInfo, Convert.ToString(obj2, CultureInfo.CurrentCulture));
						}
						PropertyMapper.SetMappedPropertyValue(sender, text, obj3, this.InDesigner);
					}
				}
			}
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x000C908C File Offset: 0x000C808C
		private void InitTemplateProperties(object obj)
		{
			if (this._templatePropertyEntries == null)
			{
				return;
			}
			object[] array = new object[1];
			ICollection collection;
			if (this.flags[256])
			{
				collection = this.GetFilteredPropertyEntrySet(this.TemplatePropertyEntries);
			}
			else
			{
				collection = this.TemplatePropertyEntries;
			}
			foreach (object obj2 in collection)
			{
				TemplatePropertyEntry templatePropertyEntry = (TemplatePropertyEntry)obj2;
				try
				{
					ControlBuilder builder = templatePropertyEntry.Builder;
					builder.SetServiceProvider(this.ServiceProvider);
					try
					{
						array[0] = builder.BuildObject(this.flags[32768]);
					}
					finally
					{
						builder.SetServiceProvider(null);
					}
					MethodInfo setMethod = templatePropertyEntry.PropertyInfo.GetSetMethod();
					Util.InvokeMethod(setMethod, obj, array);
				}
				catch (Exception ex)
				{
					throw new HttpException(SR.GetString("Cannot_set_property", new object[] { this.TagName, templatePropertyEntry.Name }), ex);
				}
			}
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x000C91B8 File Offset: 0x000C81B8
		private void BindFieldToControl(Control control)
		{
			if (this.flags[2048] && !this.flags[4096])
			{
				return;
			}
			this.flags[2048] = true;
			TemplateControl templateControl = this.TemplateControl;
			if (templateControl == null)
			{
				return;
			}
			Type type = this.TemplateControl.GetType();
			if (!this.flags[4096])
			{
				if (this.InDesigner)
				{
					return;
				}
				if (control.ID == null)
				{
					return;
				}
				if (type.Assembly == typeof(HttpRuntime).Assembly)
				{
					return;
				}
			}
			FieldInfo field = templateControl.GetType().GetField(control.ID, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null || field.IsPrivate || !field.FieldType.IsAssignableFrom(control.GetType()))
			{
				return;
			}
			field.SetValue(templateControl, control);
			this.flags[4096] = true;
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x000C929A File Offset: 0x000C829A
		public virtual bool NeedsTagInnerText()
		{
			return false;
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x000C92A0 File Offset: 0x000C82A0
		public virtual void OnAppendToParentBuilder(ControlBuilder parentBuilder)
		{
			if (this.DefaultPropertyBuilder != null)
			{
				ControlBuilder defaultPropertyBuilder = this.DefaultPropertyBuilder;
				this.ParseTimeData.DefaultPropertyBuilder = null;
				this.AppendSubBuilder(defaultPropertyBuilder);
			}
			if (!(this is BindableTemplateBuilder))
			{
				ControlBuilder controlBuilder = this;
				while (controlBuilder != null && !(controlBuilder is BindableTemplateBuilder))
				{
					controlBuilder = controlBuilder.ParentBuilder;
				}
				if (controlBuilder != null && controlBuilder is BindableTemplateBuilder)
				{
					foreach (object obj in this.BoundPropertyEntries)
					{
						BoundPropertyEntry boundPropertyEntry = (BoundPropertyEntry)obj;
						if (boundPropertyEntry.TwoWayBound)
						{
							((BindableTemplateBuilder)controlBuilder).AddBoundProperty(boundPropertyEntry);
						}
					}
				}
			}
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x000C9354 File Offset: 0x000C8354
		internal virtual void PrepareNoCompilePageSupport()
		{
			this.flags[1] = true;
			this._parseTimeData = null;
			if (this._eventEntries != null && this._eventEntries.Count == 0)
			{
				this._eventEntries = null;
			}
			if (this._simplePropertyEntries != null && this._simplePropertyEntries.Count == 0)
			{
				this._simplePropertyEntries = null;
			}
			if (this._complexPropertyEntries != null)
			{
				if (this._complexPropertyEntries.Count == 0)
				{
					this._complexPropertyEntries = null;
				}
				else
				{
					foreach (object obj in this._complexPropertyEntries)
					{
						BuilderPropertyEntry builderPropertyEntry = (BuilderPropertyEntry)obj;
						if (builderPropertyEntry.Builder != null)
						{
							builderPropertyEntry.Builder.PrepareNoCompilePageSupport();
						}
					}
				}
			}
			if (this._templatePropertyEntries != null)
			{
				if (this._templatePropertyEntries.Count == 0)
				{
					this._templatePropertyEntries = null;
				}
				else
				{
					foreach (object obj2 in this._templatePropertyEntries)
					{
						BuilderPropertyEntry builderPropertyEntry2 = (BuilderPropertyEntry)obj2;
						if (builderPropertyEntry2.Builder != null)
						{
							builderPropertyEntry2.Builder.PrepareNoCompilePageSupport();
						}
					}
				}
			}
			if (this._boundPropertyEntries != null && this._boundPropertyEntries.Count == 0)
			{
				this._boundPropertyEntries = null;
			}
			if (this._subBuilders != null)
			{
				if (this._subBuilders.Count > 0)
				{
					using (IEnumerator enumerator3 = this._subBuilders.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							object obj3 = enumerator3.Current;
							ControlBuilder controlBuilder = obj3 as ControlBuilder;
							if (controlBuilder != null)
							{
								controlBuilder.PrepareNoCompilePageSupport();
							}
						}
						goto IL_01A6;
					}
				}
				this._subBuilders = null;
			}
			IL_01A6:
			this.EnsureEntriesSorted();
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x000C9538 File Offset: 0x000C8538
		internal void PreprocessAttribute(string filter, string attribname, string attribvalue, bool mainDirectiveMode)
		{
			if (attribvalue == null)
			{
				attribvalue = string.Empty;
			}
			Match match;
			if ((match = ControlBuilder.databindRegex.Match(attribvalue, 0)).Success)
			{
				if (BuildManager.PrecompilingForUpdatableDeployment)
				{
					return;
				}
				string value = match.Groups["code"].Value;
				bool flag = false;
				bool flag2 = false;
				if (!this.InDesigner)
				{
					if ((match = ControlBuilder.bindExpressionRegex.Match(value, 0)).Success)
					{
						flag = true;
						flag2 = true;
					}
					else if ((this.CompilationMode == CompilationMode.Never || this.InPageTheme) && (match = ControlBuilder.evalExpressionRegex.Match(value, 0)).Success)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					if (!this.Parser.PageParserFilterProcessedDataBindingAttribute(this.ID, attribname, value))
					{
						this.Parser.EnsureCodeAllowed();
						this.AddBoundProperty(filter, attribname, string.Empty, value, null, null, string.Empty, string.Empty, false);
					}
					return;
				}
				string value2 = match.Groups["params"].Value;
				if (!(match = ControlBuilder.bindParametersRegex.Match(value2, 0)).Success)
				{
					throw new HttpException(SR.GetString("BadlyFormattedBind"));
				}
				string value3 = match.Groups["fieldName"].Value;
				string text = string.Empty;
				Group group = match.Groups["formatString"];
				if (group != null)
				{
					text = group.Value;
				}
				if (text.Length > 0 && !ControlBuilder.formatStringRegex.Match(text, 0).Success)
				{
					throw new HttpException(SR.GetString("BadlyFormattedBind"));
				}
				if (this.InPageTheme && !flag2)
				{
					this.AddBoundProperty(filter, attribname, string.Empty, value, null, null, string.Empty, string.Empty, false);
					return;
				}
				this.AddBoundProperty(filter, attribname, string.Empty, value, null, null, value3, text, flag2);
				return;
			}
			else
			{
				if (!(match = ControlBuilder.expressionBuilderRegex.Match(attribvalue, 0)).Success)
				{
					this.AddProperty(filter, attribname, attribvalue, mainDirectiveMode);
					return;
				}
				if (this.InPageTheme)
				{
					throw new HttpParseException(SR.GetString("ControlBuilder_ExpressionsNotAllowedInThemes"));
				}
				if (BuildManager.PrecompilingForUpdatableDeployment)
				{
					return;
				}
				string text2 = match.Groups["code"].Value.Trim();
				int num = text2.IndexOf(':');
				if (num == -1)
				{
					throw new HttpParseException(SR.GetString("InvalidExpressionSyntax", new object[] { attribvalue }));
				}
				string text3 = text2.Substring(0, num).Trim();
				string text4 = text2.Substring(num + 1).Trim();
				if (text3.Length == 0)
				{
					throw new HttpParseException(SR.GetString("MissingExpressionPrefix", new object[] { attribvalue }));
				}
				if (text4.Length == 0)
				{
					throw new HttpParseException(SR.GetString("MissingExpressionValue", new object[] { attribvalue }));
				}
				ExpressionBuilder expressionBuilder = null;
				if (this.CompilationMode == CompilationMode.Never)
				{
					expressionBuilder = ExpressionBuilder.GetExpressionBuilder(text3, this.Parser.CurrentVirtualPath);
					if (expressionBuilder != null && !expressionBuilder.SupportsEvaluate)
					{
						throw new InvalidOperationException(SR.GetString("Cannot_evaluate_expression", new object[] { text3 + ":" + text4 }));
					}
				}
				this.AddBoundProperty(filter, attribname, text3, text4, expressionBuilder, null, string.Empty, string.Empty, false);
				return;
			}
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000C9874 File Offset: 0x000C8874
		private bool IsValidForImplicitLocalization()
		{
			if (this.flags[8192])
			{
				return true;
			}
			if (this.ParentBuilder == null)
			{
				return false;
			}
			if (this.ParentBuilder.DefaultPropertyBuilder != null)
			{
				return typeof(ICollection).IsAssignableFrom(this.ParentBuilder.DefaultPropertyBuilder.ControlType);
			}
			return typeof(ICollection).IsAssignableFrom(this.ParentBuilder.ControlType);
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000C98E8 File Offset: 0x000C88E8
		internal void ProcessImplicitResources(ParsedAttributeCollection attribs)
		{
			string text = (string)((IDictionary)attribs)["meta:localize"];
			if (text != null)
			{
				if (!this.IsValidForImplicitLocalization())
				{
					throw new InvalidOperationException(SR.GetString("meta_localize_notallowed", new object[] { this.TagName }));
				}
				bool flag;
				if (!bool.TryParse(text, out flag))
				{
					throw new HttpException(SR.GetString("ControlBuilder_InvalidLocalizeValue", new object[] { text }));
				}
				this.ParseTimeData.Localize = flag;
			}
			else
			{
				this.ParseTimeData.Localize = true;
			}
			string text2 = (string)((IDictionary)attribs)["meta:resourcekey"];
			attribs.ClearFilter("meta");
			if (text2 == null)
			{
				return;
			}
			if (!this.IsValidForImplicitLocalization())
			{
				throw new InvalidOperationException(SR.GetString("meta_reskey_notallowed", new object[] { this.TagName }));
			}
			if (!CodeGenerator.IsValidLanguageIndependentIdentifier(text2))
			{
				throw new HttpException(SR.GetString("Invalid_resourcekey", new object[] { text2 }));
			}
			if (!this.ParseTimeData.Localize)
			{
				throw new HttpException(SR.GetString("meta_localize_error"));
			}
			this.ParseTimeData.ResourceKeyPrefix = text2;
			IImplicitResourceProvider implicitResourceProvider;
			if (this.Parser.FInDesigner && this.Parser.DesignerHost != null)
			{
				implicitResourceProvider = (IImplicitResourceProvider)this.Parser.DesignerHost.GetService(typeof(IImplicitResourceProvider));
			}
			else
			{
				implicitResourceProvider = this.Parser.GetImplicitResourceProvider();
			}
			ICollection collection = null;
			if (implicitResourceProvider != null)
			{
				collection = implicitResourceProvider.GetImplicitResourceKeys(text2);
			}
			if (collection != null)
			{
				IDesignerHost designerHost = this.DesignerHost;
				ExpressionBuilder expressionBuilder = ExpressionBuilder.GetExpressionBuilder("resources", this.Parser.CurrentVirtualPath, designerHost);
				bool flag2 = typeof(ResourceExpressionBuilder) == expressionBuilder.GetType();
				foreach (object obj in collection)
				{
					ImplicitResourceKey implicitResourceKey = (ImplicitResourceKey)obj;
					string text3 = text2 + "." + implicitResourceKey.Property;
					if (implicitResourceKey.Filter.Length > 0)
					{
						text3 = implicitResourceKey.Filter + ':' + text3;
					}
					string text4 = implicitResourceKey.Property.Replace('.', '-');
					object obj2 = null;
					string text5;
					if (flag2)
					{
						obj2 = ResourceExpressionBuilder.ParseExpression(text3);
						text5 = string.Empty;
					}
					else
					{
						text5 = text3;
					}
					this.AddBoundProperty(implicitResourceKey.Filter, text4, "resources", text5, expressionBuilder, obj2, true, string.Empty, string.Empty, false);
				}
			}
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000C9B88 File Offset: 0x000C8B88
		private void PreprocessAttributes(ParsedAttributeCollection attribs)
		{
			this.ProcessImplicitResources(attribs);
			foreach (object obj in attribs.GetFilteredAttributeDictionaries())
			{
				FilteredAttributeDictionary filteredAttributeDictionary = (FilteredAttributeDictionary)obj;
				string filter = filteredAttributeDictionary.Filter;
				foreach (object obj2 in ((IEnumerable)filteredAttributeDictionary))
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
					string text = dictionaryEntry.Key.ToString();
					string text2 = dictionaryEntry.Value.ToString();
					this.PreprocessAttribute(filter, text, text2, false);
				}
			}
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000C9C58 File Offset: 0x000C8C58
		public void SetServiceProvider(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider;
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000C9C61 File Offset: 0x000C8C61
		internal void EnsureEntriesSorted()
		{
			if (!this.flags[16384])
			{
				this.flags[16384] = true;
				this.SortEntries();
			}
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000C9C8C File Offset: 0x000C8C8C
		internal virtual void SortEntries()
		{
			if (this is CollectionBuilder)
			{
				return;
			}
			ControlBuilder.FilteredPropertyEntryComparer filteredPropertyEntryComparer = null;
			this.ProcessAndSortPropertyEntries(this._boundPropertyEntries, ref filteredPropertyEntryComparer);
			this.ProcessAndSortPropertyEntries(this._complexPropertyEntries, ref filteredPropertyEntryComparer);
			this.ProcessAndSortPropertyEntries(this._simplePropertyEntries, ref filteredPropertyEntryComparer);
			this.ProcessAndSortPropertyEntries(this._templatePropertyEntries, ref filteredPropertyEntryComparer);
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000C9CDC File Offset: 0x000C8CDC
		internal void ProcessAndSortPropertyEntries(ArrayList propertyEntries, ref ControlBuilder.FilteredPropertyEntryComparer comparer)
		{
			if (propertyEntries != null && propertyEntries.Count > 1)
			{
				HybridDictionary hybridDictionary = new HybridDictionary(propertyEntries.Count, true);
				int num = 0;
				foreach (object obj in propertyEntries)
				{
					PropertyEntry propertyEntry = (PropertyEntry)obj;
					object obj2 = hybridDictionary[propertyEntry.Name];
					if (obj2 != null)
					{
						propertyEntry.Order = (int)obj2;
					}
					else
					{
						propertyEntry.Order = num;
						hybridDictionary.Add(propertyEntry.Name, num++);
					}
				}
				if (comparer == null)
				{
					comparer = new ControlBuilder.FilteredPropertyEntryComparer(this.CurrentFilterResolutionService);
				}
				propertyEntries.Sort(comparer);
			}
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000C9DA8 File Offset: 0x000C8DA8
		internal void SetControlType(Type controlType)
		{
			this._controlType = controlType;
			if (this._controlType != null)
			{
				this.flags[8192] = typeof(Control).IsAssignableFrom(this._controlType);
				return;
			}
			this.flags[8192] = false;
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000C9DFC File Offset: 0x000C8DFC
		internal virtual void SetParentBuilder(ControlBuilder parentBuilder)
		{
			this.ParseTimeData.ParentBuilder = parentBuilder;
			if (this.ParseTimeData.FirstNonThemableProperty != null && parentBuilder is FileLevelPageThemeBuilder)
			{
				throw new InvalidOperationException(SR.GetString("Property_theme_disabled", new object[]
				{
					this.ParseTimeData.FirstNonThemableProperty.Name,
					this.ControlType.FullName
				}));
			}
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000C9E63 File Offset: 0x000C8E63
		public string GetResourceKey()
		{
			return this.ParseTimeData.ResourceKeyPrefix;
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000C9E70 File Offset: 0x000C8E70
		public void SetResourceKey(string resourceKey)
		{
			SimplePropertyEntry simplePropertyEntry = new SimplePropertyEntry();
			simplePropertyEntry.Filter = "meta";
			simplePropertyEntry.Name = "resourcekey";
			simplePropertyEntry.Value = resourceKey;
			simplePropertyEntry.PersistedValue = resourceKey;
			simplePropertyEntry.UseSetAttribute = true;
			simplePropertyEntry.Type = typeof(string);
			this.AddEntry(this.SimplePropertyEntriesInternal, simplePropertyEntry);
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000C9ECB File Offset: 0x000C8ECB
		public virtual void SetTagInnerText(string text)
		{
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000C9ECD File Offset: 0x000C8ECD
		public virtual void ProcessGeneratedCode(CodeCompileUnit codeCompileUnit, CodeTypeDeclaration baseType, CodeTypeDeclaration derivedType, CodeMemberMethod buildMethod, CodeMemberMethod dataBindingMethod)
		{
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000C9ED0 File Offset: 0x000C8ED0
		private void ValidatePersistable(PropertyInfo propInfo, bool usingSetAttribute, bool mainDirectiveMode, bool simplePropertyEntry, string filter)
		{
			bool flag = propInfo.DeclaringType.IsAssignableFrom(this._controlType);
			PropertyDescriptorCollection propertyDescriptorCollection;
			if (flag)
			{
				propertyDescriptorCollection = this.PropertyDescriptors;
			}
			else
			{
				propertyDescriptorCollection = TypeDescriptor.GetProperties(propInfo.DeclaringType);
			}
			PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[propInfo.Name];
			if (propertyDescriptor != null)
			{
				if (flag)
				{
					if (this.IsHtmlControl)
					{
						if (propertyDescriptor.Attributes.Contains(HtmlControlPersistableAttribute.No))
						{
							throw new HttpException(SR.GetString("Property_Not_Persistable", new object[] { propertyDescriptor.Name }));
						}
					}
					else if (!usingSetAttribute && !mainDirectiveMode && propertyDescriptor.Attributes.Contains(DesignerSerializationVisibilityAttribute.Hidden))
					{
						throw new HttpException(SR.GetString("Property_Not_Persistable", new object[] { propertyDescriptor.Name }));
					}
				}
				if (!FilterableAttribute.IsPropertyFilterable(propertyDescriptor) && !string.IsNullOrEmpty(filter))
				{
					throw new InvalidOperationException(SR.GetString("Illegal_Device", new object[] { propertyDescriptor.Name }));
				}
				if (this.InPageTheme && this.ParseTimeData.FirstNonThemableProperty == null && (!simplePropertyEntry || !usingSetAttribute))
				{
					ThemeableAttribute themeableAttribute = (ThemeableAttribute)propertyDescriptor.Attributes[typeof(ThemeableAttribute)];
					if (themeableAttribute != null && !themeableAttribute.Themeable)
					{
						if (this.ParentBuilder != null)
						{
							if (this.ParentBuilder is FileLevelPageThemeBuilder)
							{
								throw new InvalidOperationException(SR.GetString("Property_theme_disabled", new object[]
								{
									propertyDescriptor.Name,
									this.ControlType.FullName
								}));
							}
						}
						else
						{
							this.ParseTimeData.FirstNonThemableProperty = propertyDescriptor;
						}
					}
				}
			}
		}

		// Token: 0x0400208F RID: 8335
		private const int parseComplete = 1;

		// Token: 0x04002090 RID: 8336
		private const int needsTagAttributeComputed = 2;

		// Token: 0x04002091 RID: 8337
		private const int needsTagAttribute = 4;

		// Token: 0x04002092 RID: 8338
		private const int doneInitObjectOptimizations = 8;

		// Token: 0x04002093 RID: 8339
		private const int isICollection = 16;

		// Token: 0x04002094 RID: 8340
		private const int isIParserAccessor = 32;

		// Token: 0x04002095 RID: 8341
		private const int hasFilteredSimpleProps = 64;

		// Token: 0x04002096 RID: 8342
		private const int hasFilteredComplexProps = 128;

		// Token: 0x04002097 RID: 8343
		private const int hasFilteredTemplateProps = 256;

		// Token: 0x04002098 RID: 8344
		private const int hasFilteredBoundProps = 512;

		// Token: 0x04002099 RID: 8345
		private const int hasTwoWayBoundProps = 1024;

		// Token: 0x0400209A RID: 8346
		private const int triedFieldToControlBinding = 2048;

		// Token: 0x0400209B RID: 8347
		private const int hasFieldToControlBinding = 4096;

		// Token: 0x0400209C RID: 8348
		private const int controlTypeIsControl = 8192;

		// Token: 0x0400209D RID: 8349
		private const int entriesSorted = 16384;

		// Token: 0x0400209E RID: 8350
		private const int applyTheme = 32768;

		// Token: 0x0400209F RID: 8351
		public static readonly string DesignerFilter = "__designer";

		// Token: 0x040020A0 RID: 8352
		private static readonly Regex databindRegex = new DataBindRegex();

		// Token: 0x040020A1 RID: 8353
		internal static readonly Regex expressionBuilderRegex = new ExpressionBuilderRegex();

		// Token: 0x040020A2 RID: 8354
		private static readonly Regex bindExpressionRegex = new BindExpressionRegex();

		// Token: 0x040020A3 RID: 8355
		private static readonly Regex bindParametersRegex = new BindParametersRegex();

		// Token: 0x040020A4 RID: 8356
		private static readonly Regex evalExpressionRegex = new EvalExpressionRegex();

		// Token: 0x040020A5 RID: 8357
		private static readonly Regex formatStringRegex = new FormatStringRegex();

		// Token: 0x040020A6 RID: 8358
		private Type _controlType;

		// Token: 0x040020A7 RID: 8359
		private string _tagName;

		// Token: 0x040020A8 RID: 8360
		private string _skinID;

		// Token: 0x040020A9 RID: 8361
		private ArrayList _subBuilders;

		// Token: 0x040020AA RID: 8362
		private ControlBuilder.ControlBuilderParseTimeData _parseTimeData;

		// Token: 0x040020AB RID: 8363
		private IServiceProvider _serviceProvider;

		// Token: 0x040020AC RID: 8364
		private ArrayList _eventEntries;

		// Token: 0x040020AD RID: 8365
		private ArrayList _simplePropertyEntries;

		// Token: 0x040020AE RID: 8366
		private ArrayList _complexPropertyEntries;

		// Token: 0x040020AF RID: 8367
		private ArrayList _templatePropertyEntries;

		// Token: 0x040020B0 RID: 8368
		private ArrayList _boundPropertyEntries;

		// Token: 0x040020B1 RID: 8369
		private IDictionary _additionalState;

		// Token: 0x040020B2 RID: 8370
		private PropertyDescriptor _bindingContainerDescriptor;

		// Token: 0x040020B3 RID: 8371
		private SimpleBitVector32 flags;

		// Token: 0x040020B4 RID: 8372
		private static FactoryGenerator s_controlBuilderFactoryGenerator;

		// Token: 0x040020B5 RID: 8373
		private static Hashtable s_controlBuilderFactoryCache;

		// Token: 0x040020B6 RID: 8374
		private static ParseChildrenAttribute s_markerParseChildrenAttribute = new ParseChildrenAttribute();

		// Token: 0x040020B7 RID: 8375
		private static Hashtable s_parseChildrenAttributeCache = new Hashtable();

		// Token: 0x040020B8 RID: 8376
		private static IWebObjectFactory s_defaultControlBuilderFactory = new ControlBuilder.DefaultControlBuilderFactory();

		// Token: 0x02000393 RID: 915
		private class DefaultControlBuilderFactory : IWebObjectFactory
		{
			// Token: 0x06002CE2 RID: 11490 RVA: 0x000CA0E1 File Offset: 0x000C90E1
			object IWebObjectFactory.CreateInstance()
			{
				return new ControlBuilder();
			}
		}

		// Token: 0x02000394 RID: 916
		private class ReflectionBasedControlBuilderFactory : IWebObjectFactory
		{
			// Token: 0x06002CE4 RID: 11492 RVA: 0x000CA0F0 File Offset: 0x000C90F0
			internal ReflectionBasedControlBuilderFactory(Type builderType)
			{
				this._builderType = builderType;
			}

			// Token: 0x06002CE5 RID: 11493 RVA: 0x000CA0FF File Offset: 0x000C90FF
			object IWebObjectFactory.CreateInstance()
			{
				return (ControlBuilder)HttpRuntime.CreateNonPublicInstance(this._builderType);
			}

			// Token: 0x040020B9 RID: 8377
			private Type _builderType;
		}

		// Token: 0x02000395 RID: 917
		private sealed class ControlBuilderParseTimeData
		{
			// Token: 0x170009C6 RID: 2502
			// (get) Token: 0x06002CE6 RID: 11494 RVA: 0x000CA111 File Offset: 0x000C9111
			// (set) Token: 0x06002CE7 RID: 11495 RVA: 0x000CA11F File Offset: 0x000C911F
			internal bool ChildrenAsProperties
			{
				get
				{
					return this.flags[1];
				}
				set
				{
					this.flags[1] = value;
				}
			}

			// Token: 0x170009C7 RID: 2503
			// (get) Token: 0x06002CE8 RID: 11496 RVA: 0x000CA12E File Offset: 0x000C912E
			// (set) Token: 0x06002CE9 RID: 11497 RVA: 0x000CA13C File Offset: 0x000C913C
			internal bool HasAspCode
			{
				get
				{
					return this.flags[2];
				}
				set
				{
					this.flags[2] = value;
				}
			}

			// Token: 0x170009C8 RID: 2504
			// (get) Token: 0x06002CEA RID: 11498 RVA: 0x000CA14B File Offset: 0x000C914B
			// (set) Token: 0x06002CEB RID: 11499 RVA: 0x000CA159 File Offset: 0x000C9159
			internal bool IsHtmlControl
			{
				get
				{
					return this.flags[4];
				}
				set
				{
					this.flags[4] = value;
				}
			}

			// Token: 0x170009C9 RID: 2505
			// (get) Token: 0x06002CEC RID: 11500 RVA: 0x000CA168 File Offset: 0x000C9168
			// (set) Token: 0x06002CED RID: 11501 RVA: 0x000CA17A File Offset: 0x000C917A
			internal bool IgnoreControlProperties
			{
				get
				{
					return this.flags[256];
				}
				set
				{
					this.flags[256] = value;
				}
			}

			// Token: 0x170009CA RID: 2506
			// (get) Token: 0x06002CEE RID: 11502 RVA: 0x000CA18D File Offset: 0x000C918D
			// (set) Token: 0x06002CEF RID: 11503 RVA: 0x000CA19B File Offset: 0x000C919B
			internal bool IsNonParserAccessor
			{
				get
				{
					return this.flags[8];
				}
				set
				{
					this.flags[8] = value;
				}
			}

			// Token: 0x170009CB RID: 2507
			// (get) Token: 0x06002CF0 RID: 11504 RVA: 0x000CA1AA File Offset: 0x000C91AA
			// (set) Token: 0x06002CF1 RID: 11505 RVA: 0x000CA1B9 File Offset: 0x000C91B9
			internal bool IsGeneratedID
			{
				get
				{
					return this.flags[64];
				}
				set
				{
					this.flags[64] = value;
				}
			}

			// Token: 0x170009CC RID: 2508
			// (get) Token: 0x06002CF2 RID: 11506 RVA: 0x000CA1C9 File Offset: 0x000C91C9
			// (set) Token: 0x06002CF3 RID: 11507 RVA: 0x000CA1DB File Offset: 0x000C91DB
			internal bool Localize
			{
				get
				{
					return this.flags[128];
				}
				set
				{
					this.flags[128] = value;
				}
			}

			// Token: 0x170009CD RID: 2509
			// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x000CA1EE File Offset: 0x000C91EE
			// (set) Token: 0x06002CF5 RID: 11509 RVA: 0x000CA1FD File Offset: 0x000C91FD
			internal bool NamingContainerSearched
			{
				get
				{
					return this.flags[16];
				}
				set
				{
					this.flags[16] = value;
				}
			}

			// Token: 0x170009CE RID: 2510
			// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x000CA20D File Offset: 0x000C920D
			// (set) Token: 0x06002CF7 RID: 11511 RVA: 0x000CA21C File Offset: 0x000C921C
			internal bool SupportsAttributes
			{
				get
				{
					return this.flags[32];
				}
				set
				{
					this.flags[32] = value;
				}
			}

			// Token: 0x040020BA RID: 8378
			private const int childrenAsProperties = 1;

			// Token: 0x040020BB RID: 8379
			private const int hasAspCode = 2;

			// Token: 0x040020BC RID: 8380
			private const int isHtmlControl = 4;

			// Token: 0x040020BD RID: 8381
			private const int isNonParserAccessor = 8;

			// Token: 0x040020BE RID: 8382
			private const int namingContainerSearched = 16;

			// Token: 0x040020BF RID: 8383
			private const int supportsAttributes = 32;

			// Token: 0x040020C0 RID: 8384
			private const int isGeneratedID = 64;

			// Token: 0x040020C1 RID: 8385
			private const int localize = 128;

			// Token: 0x040020C2 RID: 8386
			private const int ignoreControlProperties = 256;

			// Token: 0x040020C3 RID: 8387
			private SimpleBitVector32 flags;

			// Token: 0x040020C4 RID: 8388
			internal ControlBuilder DefaultPropertyBuilder;

			// Token: 0x040020C5 RID: 8389
			internal EventDescriptorCollection EventDescriptors;

			// Token: 0x040020C6 RID: 8390
			internal string Filter;

			// Token: 0x040020C7 RID: 8391
			internal string ID;

			// Token: 0x040020C8 RID: 8392
			internal int Line;

			// Token: 0x040020C9 RID: 8393
			internal ControlBuilder NamingContainerBuilder;

			// Token: 0x040020CA RID: 8394
			internal ControlBuilder ParentBuilder;

			// Token: 0x040020CB RID: 8395
			internal TemplateParser Parser;

			// Token: 0x040020CC RID: 8396
			internal PropertyDescriptorCollection PropertyDescriptors;

			// Token: 0x040020CD RID: 8397
			internal StringSet PropertyEntries;

			// Token: 0x040020CE RID: 8398
			internal VirtualPath VirtualPath;

			// Token: 0x040020CF RID: 8399
			internal PropertyDescriptor FirstNonThemableProperty;

			// Token: 0x040020D0 RID: 8400
			internal string ResourceKeyPrefix;
		}

		// Token: 0x02000396 RID: 918
		internal sealed class FilteredPropertyEntryComparer : IComparer
		{
			// Token: 0x06002CF9 RID: 11513 RVA: 0x000CA234 File Offset: 0x000C9234
			public FilteredPropertyEntryComparer(IFilterResolutionService filterResolutionService)
			{
				this._filterResolutionService = filterResolutionService;
			}

			// Token: 0x06002CFA RID: 11514 RVA: 0x000CA244 File Offset: 0x000C9244
			int IComparer.Compare(object o1, object o2)
			{
				if (o1 == o2)
				{
					return 0;
				}
				if (o1 == null)
				{
					return 1;
				}
				if (o2 == null)
				{
					return -1;
				}
				PropertyEntry propertyEntry = (PropertyEntry)o1;
				PropertyEntry propertyEntry2 = (PropertyEntry)o2;
				int num = propertyEntry.Order - propertyEntry2.Order;
				if (num == 0)
				{
					if (this._filterResolutionService == null)
					{
						if (string.IsNullOrEmpty(propertyEntry.Filter))
						{
							if (propertyEntry2.Filter != null && propertyEntry2.Filter.Length > 0)
							{
								num = 1;
							}
							else
							{
								num = 0;
							}
						}
						else if (string.IsNullOrEmpty(propertyEntry2.Filter))
						{
							num = -1;
						}
						else
						{
							num = 0;
						}
					}
					else
					{
						string text = ((propertyEntry.Filter.Length == 0) ? "Default" : propertyEntry.Filter);
						string text2 = ((propertyEntry2.Filter.Length == 0) ? "Default" : propertyEntry2.Filter);
						num = this._filterResolutionService.CompareFilters(text, text2);
					}
					if (num == 0)
					{
						return propertyEntry.Index - propertyEntry2.Index;
					}
				}
				return num;
			}

			// Token: 0x040020D1 RID: 8401
			private IFilterResolutionService _filterResolutionService;
		}
	}
}
