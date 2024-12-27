using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace System.Data.OracleClient
{
	// Token: 0x0200005E RID: 94
	[DefaultProperty("DataSource")]
	[TypeConverter(typeof(OracleConnectionStringBuilder.OracleConnectionStringBuilderConverter))]
	public sealed class OracleConnectionStringBuilder : DbConnectionStringBuilder
	{
		// Token: 0x060003C6 RID: 966 RVA: 0x00063634 File Offset: 0x00062A34
		static OracleConnectionStringBuilder()
		{
			string[] array = new string[12];
			array[0] = "Data Source";
			array[5] = "Enlist";
			array[2] = "Integrated Security";
			array[10] = "Load Balance Timeout";
			array[8] = "Max Pool Size";
			array[7] = "Min Pool Size";
			array[4] = "Password";
			array[1] = "Persist Security Info";
			array[6] = "Pooling";
			array[9] = "Unicode";
			array[3] = "User ID";
			array[11] = "Omit Oracle Connection Name";
			OracleConnectionStringBuilder._validKeywords = array;
			OracleConnectionStringBuilder._keywords = new Dictionary<string, OracleConnectionStringBuilder.Keywords>(19, StringComparer.OrdinalIgnoreCase)
			{
				{
					"Data Source",
					OracleConnectionStringBuilder.Keywords.DataSource
				},
				{
					"Enlist",
					OracleConnectionStringBuilder.Keywords.Enlist
				},
				{
					"Integrated Security",
					OracleConnectionStringBuilder.Keywords.IntegratedSecurity
				},
				{
					"Load Balance Timeout",
					OracleConnectionStringBuilder.Keywords.LoadBalanceTimeout
				},
				{
					"Max Pool Size",
					OracleConnectionStringBuilder.Keywords.MaxPoolSize
				},
				{
					"Min Pool Size",
					OracleConnectionStringBuilder.Keywords.MinPoolSize
				},
				{
					"Omit Oracle Connection Name",
					OracleConnectionStringBuilder.Keywords.OmitOracleConnectionName
				},
				{
					"Password",
					OracleConnectionStringBuilder.Keywords.Password
				},
				{
					"Persist Security Info",
					OracleConnectionStringBuilder.Keywords.PersistSecurityInfo
				},
				{
					"Pooling",
					OracleConnectionStringBuilder.Keywords.Pooling
				},
				{
					"Unicode",
					OracleConnectionStringBuilder.Keywords.Unicode
				},
				{
					"User ID",
					OracleConnectionStringBuilder.Keywords.UserID
				},
				{
					"server",
					OracleConnectionStringBuilder.Keywords.DataSource
				},
				{
					"connection lifetime",
					OracleConnectionStringBuilder.Keywords.LoadBalanceTimeout
				},
				{
					"pwd",
					OracleConnectionStringBuilder.Keywords.Password
				},
				{
					"persistsecurityinfo",
					OracleConnectionStringBuilder.Keywords.PersistSecurityInfo
				},
				{
					"uid",
					OracleConnectionStringBuilder.Keywords.UserID
				},
				{
					"user",
					OracleConnectionStringBuilder.Keywords.UserID
				},
				{
					"Workaround Oracle Bug 914652",
					OracleConnectionStringBuilder.Keywords.OmitOracleConnectionName
				}
			};
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000637B0 File Offset: 0x00062BB0
		public OracleConnectionStringBuilder()
			: this(null)
		{
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000637C4 File Offset: 0x00062BC4
		public OracleConnectionStringBuilder(string connectionString)
		{
			if (!ADP.IsEmpty(connectionString))
			{
				base.ConnectionString = connectionString;
			}
		}

		// Token: 0x170000AE RID: 174
		public override object this[string keyword]
		{
			get
			{
				OracleConnectionStringBuilder.Keywords index = this.GetIndex(keyword);
				return this.GetAt(index);
			}
			set
			{
				Bid.Trace("<comm.OracleConnectionStringBuilder.set_Item|API> keyword='%ls'\n", keyword);
				if (value == null)
				{
					this.Remove(keyword);
					return;
				}
				switch (this.GetIndex(keyword))
				{
				case OracleConnectionStringBuilder.Keywords.DataSource:
					this.DataSource = OracleConnectionStringBuilder.ConvertToString(value);
					return;
				case OracleConnectionStringBuilder.Keywords.PersistSecurityInfo:
					this.PersistSecurityInfo = OracleConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case OracleConnectionStringBuilder.Keywords.IntegratedSecurity:
					this.IntegratedSecurity = OracleConnectionStringBuilder.ConvertToIntegratedSecurity(value);
					return;
				case OracleConnectionStringBuilder.Keywords.UserID:
					this.UserID = OracleConnectionStringBuilder.ConvertToString(value);
					return;
				case OracleConnectionStringBuilder.Keywords.Password:
					this.Password = OracleConnectionStringBuilder.ConvertToString(value);
					return;
				case OracleConnectionStringBuilder.Keywords.Enlist:
					this.Enlist = OracleConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case OracleConnectionStringBuilder.Keywords.Pooling:
					this.Pooling = OracleConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case OracleConnectionStringBuilder.Keywords.MinPoolSize:
					this.MinPoolSize = OracleConnectionStringBuilder.ConvertToInt32(value);
					return;
				case OracleConnectionStringBuilder.Keywords.MaxPoolSize:
					this.MaxPoolSize = OracleConnectionStringBuilder.ConvertToInt32(value);
					return;
				case OracleConnectionStringBuilder.Keywords.Unicode:
					this.Unicode = OracleConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case OracleConnectionStringBuilder.Keywords.LoadBalanceTimeout:
					this.LoadBalanceTimeout = OracleConnectionStringBuilder.ConvertToInt32(value);
					return;
				case OracleConnectionStringBuilder.Keywords.OmitOracleConnectionName:
					this.OmitOracleConnectionName = OracleConnectionStringBuilder.ConvertToBoolean(value);
					return;
				default:
					throw ADP.KeywordNotSupported(keyword);
				}
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0006394C File Offset: 0x00062D4C
		// (set) Token: 0x060003CC RID: 972 RVA: 0x00063960 File Offset: 0x00062D60
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Source")]
		[ResDescription("DbConnectionString_DataSource")]
		[DisplayName("Data Source")]
		public string DataSource
		{
			get
			{
				return this._dataSource;
			}
			set
			{
				if (value != null && 128 < value.Length)
				{
					throw ADP.InvalidConnectionOptionLength("Data Source", 128);
				}
				this.SetValue("Data Source", value);
				this._dataSource = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060003CD RID: 973 RVA: 0x000639A0 File Offset: 0x00062DA0
		// (set) Token: 0x060003CE RID: 974 RVA: 0x000639B4 File Offset: 0x00062DB4
		[ResCategory("DataCategory_Pooling")]
		[DisplayName("Enlist")]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_Enlist")]
		public bool Enlist
		{
			get
			{
				return this._enlist;
			}
			set
			{
				this.SetValue("Enlist", value);
				this._enlist = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003CF RID: 975 RVA: 0x000639D4 File Offset: 0x00062DD4
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x000639E8 File Offset: 0x00062DE8
		[ResCategory("DataCategory_Security")]
		[ResDescription("DbConnectionString_IntegratedSecurity")]
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Integrated Security")]
		public bool IntegratedSecurity
		{
			get
			{
				return this._integratedSecurity;
			}
			set
			{
				this.SetValue("Integrated Security", value);
				this._integratedSecurity = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00063A08 File Offset: 0x00062E08
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x00063A1C File Offset: 0x00062E1C
		[DisplayName("Load Balance Timeout")]
		[ResDescription("DbConnectionString_LoadBalanceTimeout")]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Pooling")]
		public int LoadBalanceTimeout
		{
			get
			{
				return this._loadBalanceTimeout;
			}
			set
			{
				if (value < 0)
				{
					throw ADP.InvalidConnectionOptionValue("Load Balance Timeout");
				}
				this.SetValue("Load Balance Timeout", value);
				this._loadBalanceTimeout = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00063A4C File Offset: 0x00062E4C
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x00063A60 File Offset: 0x00062E60
		[DisplayName("Max Pool Size")]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Pooling")]
		[ResDescription("DbConnectionString_MaxPoolSize")]
		public int MaxPoolSize
		{
			get
			{
				return this._maxPoolSize;
			}
			set
			{
				if (value < 1)
				{
					throw ADP.InvalidConnectionOptionValue("Max Pool Size");
				}
				this.SetValue("Max Pool Size", value);
				this._maxPoolSize = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00063A90 File Offset: 0x00062E90
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x00063AA4 File Offset: 0x00062EA4
		[DisplayName("Min Pool Size")]
		[ResCategory("DataCategory_Pooling")]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_MinPoolSize")]
		public int MinPoolSize
		{
			get
			{
				return this._minPoolSize;
			}
			set
			{
				if (value < 0)
				{
					throw ADP.InvalidConnectionOptionValue("Min Pool Size");
				}
				this.SetValue("Min Pool Size", value);
				this._minPoolSize = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00063AD4 File Offset: 0x00062ED4
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x00063AE8 File Offset: 0x00062EE8
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_OmitOracleConnectionName")]
		[DisplayName("Omit Oracle Connection Name")]
		[ResCategory("DataCategory_Initialization")]
		public bool OmitOracleConnectionName
		{
			get
			{
				return this._omitOracleConnectionName;
			}
			set
			{
				this.SetValue("Omit Oracle Connection Name", value);
				this._omitOracleConnectionName = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00063B08 File Offset: 0x00062F08
		// (set) Token: 0x060003DA RID: 986 RVA: 0x00063B1C File Offset: 0x00062F1C
		[PasswordPropertyText(true)]
		[DisplayName("Password")]
		[ResCategory("DataCategory_Security")]
		[ResDescription("DbConnectionString_Password")]
		[RefreshProperties(RefreshProperties.All)]
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				if (value != null && 30 < value.Length)
				{
					throw ADP.InvalidConnectionOptionLength("Password", 30);
				}
				this.SetValue("Password", value);
				this._password = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003DB RID: 987 RVA: 0x00063B58 File Offset: 0x00062F58
		// (set) Token: 0x060003DC RID: 988 RVA: 0x00063B6C File Offset: 0x00062F6C
		[ResDescription("DbConnectionString_PersistSecurityInfo")]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Security")]
		[DisplayName("Persist Security Info")]
		public bool PersistSecurityInfo
		{
			get
			{
				return this._persistSecurityInfo;
			}
			set
			{
				this.SetValue("Persist Security Info", value);
				this._persistSecurityInfo = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003DD RID: 989 RVA: 0x00063B8C File Offset: 0x00062F8C
		// (set) Token: 0x060003DE RID: 990 RVA: 0x00063BA0 File Offset: 0x00062FA0
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Pooling")]
		[ResCategory("DataCategory_Pooling")]
		[ResDescription("DbConnectionString_Pooling")]
		public bool Pooling
		{
			get
			{
				return this._pooling;
			}
			set
			{
				this.SetValue("Pooling", value);
				this._pooling = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00063BC0 File Offset: 0x00062FC0
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x00063BD4 File Offset: 0x00062FD4
		[DisplayName("Unicode")]
		[ResDescription("DbConnectionString_Unicode")]
		[ResCategory("DataCategory_Initialization")]
		[RefreshProperties(RefreshProperties.All)]
		public bool Unicode
		{
			get
			{
				return this._unicode;
			}
			set
			{
				this.SetValue("Unicode", value);
				this._unicode = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x00063BF4 File Offset: 0x00062FF4
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x00063C08 File Offset: 0x00063008
		[DisplayName("User ID")]
		[ResDescription("DbConnectionString_UserID")]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Security")]
		public string UserID
		{
			get
			{
				return this._userID;
			}
			set
			{
				if (value != null && 30 < value.Length)
				{
					throw ADP.InvalidConnectionOptionLength("User ID", 30);
				}
				this.SetValue("User ID", value);
				this._userID = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00063C44 File Offset: 0x00063044
		public override bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x00063C54 File Offset: 0x00063054
		public override ICollection Keys
		{
			get
			{
				return new ReadOnlyCollection<string>(OracleConnectionStringBuilder._validKeywords);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00063C6C File Offset: 0x0006306C
		public override ICollection Values
		{
			get
			{
				object[] array = new object[OracleConnectionStringBuilder._validKeywords.Length];
				for (int i = 0; i < OracleConnectionStringBuilder._validKeywords.Length; i++)
				{
					array[i] = this.GetAt((OracleConnectionStringBuilder.Keywords)i);
				}
				return new ReadOnlyCollection<object>(array);
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00063CA8 File Offset: 0x000630A8
		public override void Clear()
		{
			base.Clear();
			for (int i = 0; i < OracleConnectionStringBuilder._validKeywords.Length; i++)
			{
				this.Reset((OracleConnectionStringBuilder.Keywords)i);
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00063CD4 File Offset: 0x000630D4
		internal new void ClearPropertyDescriptors()
		{
			base.ClearPropertyDescriptors();
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00063CE8 File Offset: 0x000630E8
		public override bool ContainsKey(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			return OracleConnectionStringBuilder._keywords.ContainsKey(keyword);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00063D0C File Offset: 0x0006310C
		private static bool ConvertToBoolean(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToBoolean(value);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00063D20 File Offset: 0x00063120
		private static int ConvertToInt32(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToInt32(value);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00063D34 File Offset: 0x00063134
		private static bool ConvertToIntegratedSecurity(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToIntegratedSecurity(value);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00063D48 File Offset: 0x00063148
		private static string ConvertToString(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToString(value);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00063D5C File Offset: 0x0006315C
		private object GetAt(OracleConnectionStringBuilder.Keywords index)
		{
			switch (index)
			{
			case OracleConnectionStringBuilder.Keywords.DataSource:
				return this.DataSource;
			case OracleConnectionStringBuilder.Keywords.PersistSecurityInfo:
				return this.PersistSecurityInfo;
			case OracleConnectionStringBuilder.Keywords.IntegratedSecurity:
				return this.IntegratedSecurity;
			case OracleConnectionStringBuilder.Keywords.UserID:
				return this.UserID;
			case OracleConnectionStringBuilder.Keywords.Password:
				return this.Password;
			case OracleConnectionStringBuilder.Keywords.Enlist:
				return this.Enlist;
			case OracleConnectionStringBuilder.Keywords.Pooling:
				return this.Pooling;
			case OracleConnectionStringBuilder.Keywords.MinPoolSize:
				return this.MinPoolSize;
			case OracleConnectionStringBuilder.Keywords.MaxPoolSize:
				return this.MaxPoolSize;
			case OracleConnectionStringBuilder.Keywords.Unicode:
				return this.Unicode;
			case OracleConnectionStringBuilder.Keywords.LoadBalanceTimeout:
				return this.LoadBalanceTimeout;
			case OracleConnectionStringBuilder.Keywords.OmitOracleConnectionName:
				return this.OmitOracleConnectionName;
			default:
				throw ADP.KeywordNotSupported(OracleConnectionStringBuilder._validKeywords[(int)index]);
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00063E34 File Offset: 0x00063234
		private OracleConnectionStringBuilder.Keywords GetIndex(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			OracleConnectionStringBuilder.Keywords keywords;
			if (OracleConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
			{
				return keywords;
			}
			throw ADP.KeywordNotSupported(keyword);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00063E64 File Offset: 0x00063264
		private Attribute[] GetAttributesFromCollection(AttributeCollection collection)
		{
			Attribute[] array = new Attribute[collection.Count];
			collection.CopyTo(array, 0);
			return array;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00063E88 File Offset: 0x00063288
		protected override void GetProperties(Hashtable propertyDescriptors)
		{
			foreach (object obj in TypeDescriptor.GetProperties(this, true))
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				bool flag = false;
				string displayName = propertyDescriptor.DisplayName;
				bool flag2;
				if ("Integrated Security" == displayName)
				{
					flag = true;
					flag2 = propertyDescriptor.IsReadOnly;
				}
				else
				{
					if (!("Password" == displayName) && !("User ID" == displayName))
					{
						continue;
					}
					flag2 = this.IntegratedSecurity;
				}
				Attribute[] attributesFromCollection = this.GetAttributesFromCollection(propertyDescriptor.Attributes);
				propertyDescriptors[displayName] = new DbConnectionStringBuilderDescriptor(propertyDescriptor.Name, propertyDescriptor.ComponentType, propertyDescriptor.PropertyType, flag2, attributesFromCollection)
				{
					RefreshOnChange = flag
				};
			}
			base.GetProperties(propertyDescriptors);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00063F7C File Offset: 0x0006337C
		public override bool Remove(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			OracleConnectionStringBuilder.Keywords keywords;
			if (OracleConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
			{
				base.Remove(OracleConnectionStringBuilder._validKeywords[(int)keywords]);
				this.Reset(keywords);
				return true;
			}
			return false;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00063FBC File Offset: 0x000633BC
		private void Reset(OracleConnectionStringBuilder.Keywords index)
		{
			switch (index)
			{
			case OracleConnectionStringBuilder.Keywords.DataSource:
				this._dataSource = "";
				return;
			case OracleConnectionStringBuilder.Keywords.PersistSecurityInfo:
				this._persistSecurityInfo = false;
				return;
			case OracleConnectionStringBuilder.Keywords.IntegratedSecurity:
				this._integratedSecurity = false;
				return;
			case OracleConnectionStringBuilder.Keywords.UserID:
				this._userID = "";
				return;
			case OracleConnectionStringBuilder.Keywords.Password:
				this._password = "";
				return;
			case OracleConnectionStringBuilder.Keywords.Enlist:
				this._enlist = true;
				return;
			case OracleConnectionStringBuilder.Keywords.Pooling:
				this._pooling = true;
				return;
			case OracleConnectionStringBuilder.Keywords.MinPoolSize:
				this._minPoolSize = 0;
				return;
			case OracleConnectionStringBuilder.Keywords.MaxPoolSize:
				this._maxPoolSize = 100;
				return;
			case OracleConnectionStringBuilder.Keywords.Unicode:
				this._unicode = false;
				return;
			case OracleConnectionStringBuilder.Keywords.LoadBalanceTimeout:
				this._loadBalanceTimeout = 0;
				return;
			case OracleConnectionStringBuilder.Keywords.OmitOracleConnectionName:
				this._omitOracleConnectionName = false;
				return;
			default:
				throw ADP.KeywordNotSupported(OracleConnectionStringBuilder._validKeywords[(int)index]);
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0006407C File Offset: 0x0006347C
		private void SetValue(string keyword, bool value)
		{
			base[keyword] = value.ToString(null);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00064098 File Offset: 0x00063498
		private void SetValue(string keyword, int value)
		{
			base[keyword] = value.ToString(null);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000640B4 File Offset: 0x000634B4
		private void SetValue(string keyword, string value)
		{
			ADP.CheckArgumentNull(value, keyword);
			base[keyword] = value;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000640D0 File Offset: 0x000634D0
		public override bool ShouldSerialize(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			OracleConnectionStringBuilder.Keywords keywords;
			return OracleConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords) && base.ShouldSerialize(OracleConnectionStringBuilder._validKeywords[(int)keywords]);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00064108 File Offset: 0x00063508
		public override bool TryGetValue(string keyword, out object value)
		{
			OracleConnectionStringBuilder.Keywords keywords;
			if (OracleConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
			{
				value = this.GetAt(keywords);
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x040003E4 RID: 996
		private static readonly string[] _validKeywords;

		// Token: 0x040003E5 RID: 997
		private static readonly Dictionary<string, OracleConnectionStringBuilder.Keywords> _keywords;

		// Token: 0x040003E6 RID: 998
		private string _dataSource = "";

		// Token: 0x040003E7 RID: 999
		private string _password = "";

		// Token: 0x040003E8 RID: 1000
		private string _userID = "";

		// Token: 0x040003E9 RID: 1001
		private int _loadBalanceTimeout;

		// Token: 0x040003EA RID: 1002
		private int _maxPoolSize = 100;

		// Token: 0x040003EB RID: 1003
		private int _minPoolSize;

		// Token: 0x040003EC RID: 1004
		private bool _enlist = true;

		// Token: 0x040003ED RID: 1005
		private bool _integratedSecurity;

		// Token: 0x040003EE RID: 1006
		private bool _persistSecurityInfo;

		// Token: 0x040003EF RID: 1007
		private bool _pooling = true;

		// Token: 0x040003F0 RID: 1008
		private bool _unicode;

		// Token: 0x040003F1 RID: 1009
		private bool _omitOracleConnectionName;

		// Token: 0x0200005F RID: 95
		private enum Keywords
		{
			// Token: 0x040003F3 RID: 1011
			DataSource,
			// Token: 0x040003F4 RID: 1012
			PersistSecurityInfo,
			// Token: 0x040003F5 RID: 1013
			IntegratedSecurity,
			// Token: 0x040003F6 RID: 1014
			UserID,
			// Token: 0x040003F7 RID: 1015
			Password,
			// Token: 0x040003F8 RID: 1016
			Enlist,
			// Token: 0x040003F9 RID: 1017
			Pooling,
			// Token: 0x040003FA RID: 1018
			MinPoolSize,
			// Token: 0x040003FB RID: 1019
			MaxPoolSize,
			// Token: 0x040003FC RID: 1020
			Unicode,
			// Token: 0x040003FD RID: 1021
			LoadBalanceTimeout,
			// Token: 0x040003FE RID: 1022
			OmitOracleConnectionName
		}

		// Token: 0x02000060 RID: 96
		internal sealed class OracleConnectionStringBuilderConverter : ExpandableObjectConverter
		{
			// Token: 0x060003F9 RID: 1017 RVA: 0x00064148 File Offset: 0x00063548
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return typeof(InstanceDescriptor) == destinationType || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x060003FA RID: 1018 RVA: 0x0006416C File Offset: 0x0006356C
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw ADP.ArgumentNull("destinationType");
				}
				if (typeof(InstanceDescriptor) == destinationType)
				{
					OracleConnectionStringBuilder oracleConnectionStringBuilder = value as OracleConnectionStringBuilder;
					if (oracleConnectionStringBuilder != null)
					{
						return this.ConvertToInstanceDescriptor(oracleConnectionStringBuilder);
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			// Token: 0x060003FB RID: 1019 RVA: 0x000641B4 File Offset: 0x000635B4
			private InstanceDescriptor ConvertToInstanceDescriptor(OracleConnectionStringBuilder options)
			{
				Type[] array = new Type[] { typeof(string) };
				object[] array2 = new object[] { options.ConnectionString };
				ConstructorInfo constructor = typeof(OracleConnectionStringBuilder).GetConstructor(array);
				return new InstanceDescriptor(constructor, array2);
			}
		}
	}
}
