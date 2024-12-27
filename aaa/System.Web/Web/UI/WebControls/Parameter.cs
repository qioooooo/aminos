using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000509 RID: 1289
	[DefaultProperty("DefaultValue")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Parameter : ICloneable, IStateManager
	{
		// Token: 0x06003EC1 RID: 16065 RVA: 0x00105380 File Offset: 0x00104380
		public Parameter()
		{
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x00105388 File Offset: 0x00104388
		public Parameter(string name)
		{
			this.Name = name;
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x00105397 File Offset: 0x00104397
		public Parameter(string name, DbType dbType)
		{
			this.Name = name;
			this.DbType = dbType;
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x001053AD File Offset: 0x001043AD
		public Parameter(string name, DbType dbType, string defaultValue)
		{
			this.Name = name;
			this.DbType = dbType;
			this.DefaultValue = defaultValue;
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x001053CA File Offset: 0x001043CA
		public Parameter(string name, TypeCode type)
		{
			this.Name = name;
			this.Type = type;
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x001053E0 File Offset: 0x001043E0
		public Parameter(string name, TypeCode type, string defaultValue)
		{
			this.Name = name;
			this.Type = type;
			this.DefaultValue = defaultValue;
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x00105400 File Offset: 0x00104400
		protected Parameter(Parameter original)
		{
			this.DefaultValue = original.DefaultValue;
			this.Direction = original.Direction;
			this.Name = original.Name;
			this.ConvertEmptyStringToNull = original.ConvertEmptyStringToNull;
			this.Size = original.Size;
			this.Type = original.Type;
			this.DbType = original.DbType;
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06003EC8 RID: 16072 RVA: 0x00105467 File Offset: 0x00104467
		protected bool IsTrackingViewState
		{
			get
			{
				return this._tracking;
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06003EC9 RID: 16073 RVA: 0x00105470 File Offset: 0x00104470
		// (set) Token: 0x06003ECA RID: 16074 RVA: 0x0010549A File Offset: 0x0010449A
		[DefaultValue(DbType.Object)]
		[WebCategory("Parameter")]
		[WebSysDescription("Parameter_DbType")]
		public DbType DbType
		{
			get
			{
				object obj = this.ViewState["DbType"];
				if (obj == null)
				{
					return DbType.Object;
				}
				return (DbType)obj;
			}
			set
			{
				if (value < DbType.AnsiString || value > DbType.DateTimeOffset)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.DbType != value)
				{
					this.ViewState["DbType"] = value;
					this.OnParameterChanged();
				}
			}
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06003ECB RID: 16075 RVA: 0x001054D8 File Offset: 0x001044D8
		// (set) Token: 0x06003ECC RID: 16076 RVA: 0x001054FC File Offset: 0x001044FC
		[DefaultValue(null)]
		[WebCategory("Parameter")]
		[WebSysDescription("Parameter_DefaultValue")]
		public string DefaultValue
		{
			get
			{
				object obj = this.ViewState["DefaultValue"];
				return obj as string;
			}
			set
			{
				if (this.DefaultValue != value)
				{
					this.ViewState["DefaultValue"] = value;
					this.OnParameterChanged();
				}
			}
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06003ECD RID: 16077 RVA: 0x00105524 File Offset: 0x00104524
		// (set) Token: 0x06003ECE RID: 16078 RVA: 0x0010554D File Offset: 0x0010454D
		[WebCategory("Parameter")]
		[DefaultValue(ParameterDirection.Input)]
		[WebSysDescription("Parameter_Direction")]
		public ParameterDirection Direction
		{
			get
			{
				object obj = this.ViewState["Direction"];
				if (obj == null)
				{
					return ParameterDirection.Input;
				}
				return (ParameterDirection)obj;
			}
			set
			{
				if (this.Direction != value)
				{
					this.ViewState["Direction"] = value;
					this.OnParameterChanged();
				}
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06003ECF RID: 16079 RVA: 0x00105574 File Offset: 0x00104574
		// (set) Token: 0x06003ED0 RID: 16080 RVA: 0x001055A1 File Offset: 0x001045A1
		[WebSysDescription("Parameter_Name")]
		[WebCategory("Parameter")]
		[DefaultValue("")]
		public string Name
		{
			get
			{
				object obj = this.ViewState["Name"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				if (this.Name != value)
				{
					this.ViewState["Name"] = value;
					this.OnParameterChanged();
				}
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x06003ED1 RID: 16081 RVA: 0x001055C8 File Offset: 0x001045C8
		[Browsable(false)]
		internal object ParameterValue
		{
			get
			{
				return this.GetValue(this.ViewState["ParameterValue"], false);
			}
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x001055E4 File Offset: 0x001045E4
		public DbType GetDatabaseType()
		{
			DbType dbType = this.DbType;
			if (dbType == DbType.Object)
			{
				return Parameter.ConvertTypeCodeToDbType(this.Type);
			}
			if (this.Type != TypeCode.Empty)
			{
				throw new InvalidOperationException(SR.GetString("Parameter_TypeNotSupported", new object[] { this.Name }));
			}
			return dbType;
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x00105634 File Offset: 0x00104634
		internal object GetValue(object value, bool ignoreNullableTypeChanges)
		{
			DbType dbType = this.DbType;
			if (dbType == DbType.Object)
			{
				return Parameter.GetValue(value, this.DefaultValue, this.Type, this.ConvertEmptyStringToNull, ignoreNullableTypeChanges);
			}
			if (this.Type != TypeCode.Empty)
			{
				throw new InvalidOperationException(SR.GetString("Parameter_TypeNotSupported", new object[] { this.Name }));
			}
			return Parameter.GetValue(value, this.DefaultValue, dbType, this.ConvertEmptyStringToNull, ignoreNullableTypeChanges);
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x001056A4 File Offset: 0x001046A4
		internal static object GetValue(object value, string defaultValue, DbType dbType, bool convertEmptyStringToNull, bool ignoreNullableTypeChanges)
		{
			if (dbType != DbType.DateTimeOffset && dbType != DbType.Time && dbType != DbType.Guid)
			{
				TypeCode typeCode = Parameter.ConvertDbTypeToTypeCode(dbType);
				return Parameter.GetValue(value, defaultValue, typeCode, convertEmptyStringToNull, ignoreNullableTypeChanges);
			}
			value = Parameter.HandleNullValue(value, defaultValue, convertEmptyStringToNull);
			if (value == null)
			{
				return null;
			}
			if (ignoreNullableTypeChanges && Parameter.IsNullableType(value.GetType()))
			{
				return value;
			}
			if (dbType == DbType.DateTimeOffset)
			{
				if (value is DateTimeOffset)
				{
					return value;
				}
				return DateTimeOffset.Parse(value.ToString(), CultureInfo.CurrentCulture);
			}
			else if (dbType == DbType.Time)
			{
				if (value is TimeSpan)
				{
					return value;
				}
				return TimeSpan.Parse(value.ToString());
			}
			else
			{
				if (dbType != DbType.Guid)
				{
					return null;
				}
				if (value is Guid)
				{
					return value;
				}
				return new Guid(value.ToString());
			}
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x0010575C File Offset: 0x0010475C
		internal static object GetValue(object value, string defaultValue, TypeCode type, bool convertEmptyStringToNull, bool ignoreNullableTypeChanges)
		{
			if (type == TypeCode.DBNull)
			{
				return DBNull.Value;
			}
			value = Parameter.HandleNullValue(value, defaultValue, convertEmptyStringToNull);
			if (value == null)
			{
				return null;
			}
			if (type == TypeCode.Object || type == TypeCode.Empty)
			{
				return value;
			}
			if (ignoreNullableTypeChanges && Parameter.IsNullableType(value.GetType()))
			{
				return value;
			}
			return value = Convert.ChangeType(value, type, CultureInfo.CurrentCulture);
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x001057B0 File Offset: 0x001047B0
		private static object HandleNullValue(object value, string defaultValue, bool convertEmptyStringToNull)
		{
			if (convertEmptyStringToNull)
			{
				string text = value as string;
				if (text != null && text.Length == 0)
				{
					value = null;
				}
			}
			if (value == null)
			{
				if (convertEmptyStringToNull && string.IsNullOrEmpty(defaultValue))
				{
					defaultValue = null;
				}
				if (defaultValue == null)
				{
					return null;
				}
				value = defaultValue;
			}
			return value;
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x001057EF File Offset: 0x001047EF
		private static bool IsNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06003ED8 RID: 16088 RVA: 0x00105810 File Offset: 0x00104810
		// (set) Token: 0x06003ED9 RID: 16089 RVA: 0x00105839 File Offset: 0x00104839
		[WebSysDescription("Parameter_Size")]
		[DefaultValue(0)]
		[WebCategory("Parameter")]
		public int Size
		{
			get
			{
				object obj = this.ViewState["Size"];
				if (obj == null)
				{
					return 0;
				}
				return (int)obj;
			}
			set
			{
				if (this.Size != value)
				{
					this.ViewState["Size"] = value;
					this.OnParameterChanged();
				}
			}
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06003EDA RID: 16090 RVA: 0x00105860 File Offset: 0x00104860
		// (set) Token: 0x06003EDB RID: 16091 RVA: 0x00105889 File Offset: 0x00104889
		[WebCategory("Parameter")]
		[WebSysDescription("Parameter_Type")]
		[DefaultValue(TypeCode.Empty)]
		public TypeCode Type
		{
			get
			{
				object obj = this.ViewState["Type"];
				if (obj == null)
				{
					return TypeCode.Empty;
				}
				return (TypeCode)obj;
			}
			set
			{
				if (value < TypeCode.Empty || value > TypeCode.String)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.Type != value)
				{
					this.ViewState["Type"] = value;
					this.OnParameterChanged();
				}
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06003EDC RID: 16092 RVA: 0x001058C4 File Offset: 0x001048C4
		// (set) Token: 0x06003EDD RID: 16093 RVA: 0x001058ED File Offset: 0x001048ED
		[DefaultValue(true)]
		[WebSysDescription("Parameter_ConvertEmptyStringToNull")]
		[WebCategory("Parameter")]
		public bool ConvertEmptyStringToNull
		{
			get
			{
				object obj = this.ViewState["ConvertEmptyStringToNull"];
				return obj == null || (bool)obj;
			}
			set
			{
				if (this.ConvertEmptyStringToNull != value)
				{
					this.ViewState["ConvertEmptyStringToNull"] = value;
					this.OnParameterChanged();
				}
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06003EDE RID: 16094 RVA: 0x00105914 File Offset: 0x00104914
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected StateBag ViewState
		{
			get
			{
				if (this._viewState == null)
				{
					this._viewState = new StateBag();
					if (this._tracking)
					{
						this._viewState.TrackViewState();
					}
				}
				return this._viewState;
			}
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x00105942 File Offset: 0x00104942
		protected virtual Parameter Clone()
		{
			return new Parameter(this);
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x0010594C File Offset: 0x0010494C
		public static TypeCode ConvertDbTypeToTypeCode(DbType dbType)
		{
			switch (dbType)
			{
			case DbType.AnsiString:
			case DbType.String:
			case DbType.AnsiStringFixedLength:
			case DbType.StringFixedLength:
				return TypeCode.String;
			case DbType.Byte:
				return TypeCode.Byte;
			case DbType.Boolean:
				return TypeCode.Boolean;
			case DbType.Currency:
			case DbType.Decimal:
			case DbType.VarNumeric:
				return TypeCode.Decimal;
			case DbType.Date:
			case DbType.DateTime:
			case DbType.Time:
			case DbType.DateTime2:
				return TypeCode.DateTime;
			case DbType.Double:
				return TypeCode.Double;
			case DbType.Int16:
				return TypeCode.Int16;
			case DbType.Int32:
				return TypeCode.Int32;
			case DbType.Int64:
				return TypeCode.Int64;
			case DbType.SByte:
				return TypeCode.SByte;
			case DbType.Single:
				return TypeCode.Single;
			case DbType.UInt16:
				return TypeCode.UInt16;
			case DbType.UInt32:
				return TypeCode.UInt32;
			case DbType.UInt64:
				return TypeCode.UInt64;
			}
			return TypeCode.Object;
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x001059FC File Offset: 0x001049FC
		public static DbType ConvertTypeCodeToDbType(TypeCode typeCode)
		{
			switch (typeCode)
			{
			case TypeCode.Boolean:
				return DbType.Boolean;
			case TypeCode.Char:
				return DbType.StringFixedLength;
			case TypeCode.SByte:
				return DbType.SByte;
			case TypeCode.Byte:
				return DbType.Byte;
			case TypeCode.Int16:
				return DbType.Int16;
			case TypeCode.UInt16:
				return DbType.UInt16;
			case TypeCode.Int32:
				return DbType.Int32;
			case TypeCode.UInt32:
				return DbType.UInt32;
			case TypeCode.Int64:
				return DbType.Int64;
			case TypeCode.UInt64:
				return DbType.UInt64;
			case TypeCode.Single:
				return DbType.Single;
			case TypeCode.Double:
				return DbType.Double;
			case TypeCode.Decimal:
				return DbType.Decimal;
			case TypeCode.DateTime:
				return DbType.DateTime;
			case TypeCode.String:
				return DbType.String;
			}
			return DbType.Object;
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x00105A89 File Offset: 0x00104A89
		protected virtual object Evaluate(HttpContext context, Control control)
		{
			return null;
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x00105A8C File Offset: 0x00104A8C
		protected virtual void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				this.ViewState.LoadViewState(savedState);
			}
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x00105A9D File Offset: 0x00104A9D
		protected void OnParameterChanged()
		{
			if (this._owner != null)
			{
				this._owner.CallOnParametersChanged();
			}
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x00105AB2 File Offset: 0x00104AB2
		protected virtual object SaveViewState()
		{
			if (this._viewState == null)
			{
				return null;
			}
			return this._viewState.SaveViewState();
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x00105AC9 File Offset: 0x00104AC9
		protected internal virtual void SetDirty()
		{
			this.ViewState.SetDirty(true);
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x00105AD7 File Offset: 0x00104AD7
		internal void SetOwner(ParameterCollection owner)
		{
			this._owner = owner;
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x00105AE0 File Offset: 0x00104AE0
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x00105AE8 File Offset: 0x00104AE8
		protected virtual void TrackViewState()
		{
			this._tracking = true;
			if (this._viewState != null)
			{
				this._viewState.TrackViewState();
			}
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x00105B04 File Offset: 0x00104B04
		internal void UpdateValue(HttpContext context, Control control)
		{
			object obj = this.ViewState["ParameterValue"];
			object obj2 = this.Evaluate(context, control);
			this.ViewState["ParameterValue"] = obj2;
			if ((obj2 == null && obj != null) || (obj2 != null && !obj2.Equals(obj)))
			{
				this.OnParameterChanged();
			}
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x00105B54 File Offset: 0x00104B54
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06003EEC RID: 16108 RVA: 0x00105B5C File Offset: 0x00104B5C
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this.IsTrackingViewState;
			}
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x00105B64 File Offset: 0x00104B64
		void IStateManager.LoadViewState(object savedState)
		{
			this.LoadViewState(savedState);
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x00105B6D File Offset: 0x00104B6D
		object IStateManager.SaveViewState()
		{
			return this.SaveViewState();
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x00105B75 File Offset: 0x00104B75
		void IStateManager.TrackViewState()
		{
			this.TrackViewState();
		}

		// Token: 0x040027A9 RID: 10153
		private ParameterCollection _owner;

		// Token: 0x040027AA RID: 10154
		private bool _tracking;

		// Token: 0x040027AB RID: 10155
		private StateBag _viewState;
	}
}
