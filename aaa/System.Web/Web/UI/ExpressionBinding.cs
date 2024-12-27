using System;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x020003F1 RID: 1009
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ExpressionBinding
	{
		// Token: 0x060031E2 RID: 12770 RVA: 0x000DBC66 File Offset: 0x000DAC66
		public ExpressionBinding(string propertyName, Type propertyType, string expressionPrefix, string expression)
			: this(propertyName, propertyType, expressionPrefix, expression, false, null)
		{
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x000DBC75 File Offset: 0x000DAC75
		internal ExpressionBinding(string propertyName, Type propertyType, string expressionPrefix, string expression, bool generated, object parsedExpressionData)
		{
			this._propertyName = propertyName;
			this._propertyType = propertyType;
			this._expression = expression;
			this._expressionPrefix = expressionPrefix;
			this._generated = generated;
			this._parsedExpressionData = parsedExpressionData;
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x060031E4 RID: 12772 RVA: 0x000DBCAA File Offset: 0x000DACAA
		// (set) Token: 0x060031E5 RID: 12773 RVA: 0x000DBCB2 File Offset: 0x000DACB2
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

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x060031E6 RID: 12774 RVA: 0x000DBCBB File Offset: 0x000DACBB
		// (set) Token: 0x060031E7 RID: 12775 RVA: 0x000DBCC3 File Offset: 0x000DACC3
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

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x060031E8 RID: 12776 RVA: 0x000DBCCC File Offset: 0x000DACCC
		public bool Generated
		{
			get
			{
				return this._generated;
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x060031E9 RID: 12777 RVA: 0x000DBCD4 File Offset: 0x000DACD4
		public object ParsedExpressionData
		{
			get
			{
				return this._parsedExpressionData;
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x060031EA RID: 12778 RVA: 0x000DBCDC File Offset: 0x000DACDC
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x060031EB RID: 12779 RVA: 0x000DBCE4 File Offset: 0x000DACE4
		public Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x000DBCEC File Offset: 0x000DACEC
		public override int GetHashCode()
		{
			return this._propertyName.ToLower(CultureInfo.InvariantCulture).GetHashCode();
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x000DBD04 File Offset: 0x000DAD04
		public override bool Equals(object obj)
		{
			if (obj != null && obj is ExpressionBinding)
			{
				ExpressionBinding expressionBinding = (ExpressionBinding)obj;
				return StringUtil.EqualsIgnoreCase(this._propertyName, expressionBinding.PropertyName);
			}
			return false;
		}

		// Token: 0x040022E4 RID: 8932
		private string _propertyName;

		// Token: 0x040022E5 RID: 8933
		private Type _propertyType;

		// Token: 0x040022E6 RID: 8934
		private string _expression;

		// Token: 0x040022E7 RID: 8935
		private string _expressionPrefix;

		// Token: 0x040022E8 RID: 8936
		private bool _generated;

		// Token: 0x040022E9 RID: 8937
		private object _parsedExpressionData;
	}
}
