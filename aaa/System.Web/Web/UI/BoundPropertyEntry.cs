using System;
using System.Security.Permissions;
using System.Web.Compilation;

namespace System.Web.UI
{
	// Token: 0x0200039E RID: 926
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class BoundPropertyEntry : PropertyEntry
	{
		// Token: 0x06002D23 RID: 11555 RVA: 0x000CA7C8 File Offset: 0x000C97C8
		internal BoundPropertyEntry()
		{
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06002D24 RID: 11556 RVA: 0x000CA7D0 File Offset: 0x000C97D0
		// (set) Token: 0x06002D25 RID: 11557 RVA: 0x000CA7D8 File Offset: 0x000C97D8
		public string ControlID
		{
			get
			{
				return this._controlID;
			}
			set
			{
				this._controlID = value;
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06002D26 RID: 11558 RVA: 0x000CA7E1 File Offset: 0x000C97E1
		// (set) Token: 0x06002D27 RID: 11559 RVA: 0x000CA7E9 File Offset: 0x000C97E9
		public Type ControlType
		{
			get
			{
				return this._controlType;
			}
			set
			{
				this._controlType = value;
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06002D28 RID: 11560 RVA: 0x000CA7F2 File Offset: 0x000C97F2
		// (set) Token: 0x06002D29 RID: 11561 RVA: 0x000CA7FA File Offset: 0x000C97FA
		public string Expression
		{
			get
			{
				return this._expression;
			}
			set
			{
				this._expression = value;
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06002D2A RID: 11562 RVA: 0x000CA803 File Offset: 0x000C9803
		// (set) Token: 0x06002D2B RID: 11563 RVA: 0x000CA80B File Offset: 0x000C980B
		public ExpressionBuilder ExpressionBuilder
		{
			get
			{
				return this._expressionBuilder;
			}
			set
			{
				this._expressionBuilder = value;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06002D2C RID: 11564 RVA: 0x000CA814 File Offset: 0x000C9814
		// (set) Token: 0x06002D2D RID: 11565 RVA: 0x000CA81C File Offset: 0x000C981C
		public string ExpressionPrefix
		{
			get
			{
				return this._expressionPrefix;
			}
			set
			{
				this._expressionPrefix = value;
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06002D2E RID: 11566 RVA: 0x000CA825 File Offset: 0x000C9825
		// (set) Token: 0x06002D2F RID: 11567 RVA: 0x000CA82D File Offset: 0x000C982D
		public string FieldName
		{
			get
			{
				return this._fieldName;
			}
			set
			{
				this._fieldName = value;
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06002D30 RID: 11568 RVA: 0x000CA836 File Offset: 0x000C9836
		// (set) Token: 0x06002D31 RID: 11569 RVA: 0x000CA83E File Offset: 0x000C983E
		public string FormatString
		{
			get
			{
				return this._formatString;
			}
			set
			{
				this._formatString = value;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06002D32 RID: 11570 RVA: 0x000CA847 File Offset: 0x000C9847
		internal bool IsDataBindingEntry
		{
			get
			{
				return string.IsNullOrEmpty(this.ExpressionPrefix);
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06002D33 RID: 11571 RVA: 0x000CA854 File Offset: 0x000C9854
		// (set) Token: 0x06002D34 RID: 11572 RVA: 0x000CA85C File Offset: 0x000C985C
		public bool Generated
		{
			get
			{
				return this._generated;
			}
			set
			{
				this._generated = value;
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06002D35 RID: 11573 RVA: 0x000CA865 File Offset: 0x000C9865
		// (set) Token: 0x06002D36 RID: 11574 RVA: 0x000CA86D File Offset: 0x000C986D
		public object ParsedExpressionData
		{
			get
			{
				return this._parsedExpressionData;
			}
			set
			{
				this._parsedExpressionData = value;
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06002D37 RID: 11575 RVA: 0x000CA876 File Offset: 0x000C9876
		// (set) Token: 0x06002D38 RID: 11576 RVA: 0x000CA87E File Offset: 0x000C987E
		public bool ReadOnlyProperty
		{
			get
			{
				return this._readOnlyProperty;
			}
			set
			{
				this._readOnlyProperty = value;
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06002D39 RID: 11577 RVA: 0x000CA887 File Offset: 0x000C9887
		// (set) Token: 0x06002D3A RID: 11578 RVA: 0x000CA88F File Offset: 0x000C988F
		public bool TwoWayBound
		{
			get
			{
				return this._twoWayBound;
			}
			set
			{
				this._twoWayBound = value;
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x000CA898 File Offset: 0x000C9898
		// (set) Token: 0x06002D3C RID: 11580 RVA: 0x000CA8A0 File Offset: 0x000C98A0
		public bool UseSetAttribute
		{
			get
			{
				return this._useSetAttribute;
			}
			set
			{
				this._useSetAttribute = value;
			}
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000CA8A9 File Offset: 0x000C98A9
		internal void ParseExpression(ExpressionBuilderContext context)
		{
			if (this.Expression == null || this.ExpressionPrefix == null || this.ExpressionBuilder == null)
			{
				return;
			}
			this._parsedExpressionData = this.ExpressionBuilder.ParseExpression(this.Expression, base.Type, context);
		}

		// Token: 0x040020DE RID: 8414
		private string _expression;

		// Token: 0x040020DF RID: 8415
		private ExpressionBuilder _expressionBuilder;

		// Token: 0x040020E0 RID: 8416
		private string _expressionPrefix;

		// Token: 0x040020E1 RID: 8417
		private bool _useSetAttribute;

		// Token: 0x040020E2 RID: 8418
		private object _parsedExpressionData;

		// Token: 0x040020E3 RID: 8419
		private bool _generated;

		// Token: 0x040020E4 RID: 8420
		private string _fieldName;

		// Token: 0x040020E5 RID: 8421
		private string _formatString;

		// Token: 0x040020E6 RID: 8422
		private string _controlID;

		// Token: 0x040020E7 RID: 8423
		private Type _controlType;

		// Token: 0x040020E8 RID: 8424
		private bool _readOnlyProperty;

		// Token: 0x040020E9 RID: 8425
		private bool _twoWayBound;
	}
}
