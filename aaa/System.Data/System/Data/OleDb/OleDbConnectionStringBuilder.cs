using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Text;

namespace System.Data.OleDb
{
	// Token: 0x0200021B RID: 539
	[DefaultProperty("Provider")]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(OleDbConnectionStringBuilder.OleDbConnectionStringBuilderConverter))]
	public sealed class OleDbConnectionStringBuilder : DbConnectionStringBuilder
	{
		// Token: 0x06001ECE RID: 7886 RVA: 0x002591C0 File Offset: 0x002585C0
		static OleDbConnectionStringBuilder()
		{
			string[] array = new string[5];
			array[2] = "Data Source";
			array[0] = "File Name";
			array[4] = "OLE DB Services";
			array[3] = "Persist Security Info";
			array[1] = "Provider";
			OleDbConnectionStringBuilder._validKeywords = array;
			OleDbConnectionStringBuilder._keywords = new Dictionary<string, OleDbConnectionStringBuilder.Keywords>(9, StringComparer.OrdinalIgnoreCase)
			{
				{
					"Data Source",
					OleDbConnectionStringBuilder.Keywords.DataSource
				},
				{
					"File Name",
					OleDbConnectionStringBuilder.Keywords.FileName
				},
				{
					"OLE DB Services",
					OleDbConnectionStringBuilder.Keywords.OleDbServices
				},
				{
					"Persist Security Info",
					OleDbConnectionStringBuilder.Keywords.PersistSecurityInfo
				},
				{
					"Provider",
					OleDbConnectionStringBuilder.Keywords.Provider
				}
			};
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x00259254 File Offset: 0x00258654
		public OleDbConnectionStringBuilder()
			: this(null)
		{
			this._knownKeywords = OleDbConnectionStringBuilder._validKeywords;
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x00259274 File Offset: 0x00258674
		public OleDbConnectionStringBuilder(string connectionString)
		{
			if (!ADP.IsEmpty(connectionString))
			{
				base.ConnectionString = connectionString;
			}
		}

		// Token: 0x17000430 RID: 1072
		public override object this[string keyword]
		{
			get
			{
				Bid.Trace("<comm.OleDbConnectionStringBuilder.get_Item|API> keyword='%ls'\n", keyword);
				ADP.CheckArgumentNull(keyword, "keyword");
				OleDbConnectionStringBuilder.Keywords keywords;
				object obj;
				if (OleDbConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
				{
					obj = this.GetAt(keywords);
				}
				else if (!base.TryGetValue(keyword, out obj))
				{
					Dictionary<string, OleDbPropertyInfo> providerInfo = this.GetProviderInfo(this.Provider);
					OleDbPropertyInfo oleDbPropertyInfo = providerInfo[keyword];
					obj = oleDbPropertyInfo._defaultValue;
				}
				return obj;
			}
			set
			{
				Bid.Trace("<comm.OleDbConnectionStringBuilder.set_Item|API> keyword='%ls'\n", keyword);
				if (value == null)
				{
					this.Remove(keyword);
					return;
				}
				ADP.CheckArgumentNull(keyword, "keyword");
				OleDbConnectionStringBuilder.Keywords keywords;
				if (!OleDbConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
				{
					base[keyword] = value;
					this.ClearPropertyDescriptors();
					return;
				}
				switch (keywords)
				{
				case OleDbConnectionStringBuilder.Keywords.FileName:
					this.FileName = OleDbConnectionStringBuilder.ConvertToString(value);
					return;
				case OleDbConnectionStringBuilder.Keywords.Provider:
					this.Provider = OleDbConnectionStringBuilder.ConvertToString(value);
					return;
				case OleDbConnectionStringBuilder.Keywords.DataSource:
					this.DataSource = OleDbConnectionStringBuilder.ConvertToString(value);
					return;
				case OleDbConnectionStringBuilder.Keywords.PersistSecurityInfo:
					this.PersistSecurityInfo = OleDbConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case OleDbConnectionStringBuilder.Keywords.OleDbServices:
					this.OleDbServices = OleDbConnectionStringBuilder.ConvertToInt32(value);
					return;
				default:
					throw ADP.KeywordNotSupported(keyword);
				}
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001ED3 RID: 7891 RVA: 0x002593DC File Offset: 0x002587DC
		// (set) Token: 0x06001ED4 RID: 7892 RVA: 0x002593F0 File Offset: 0x002587F0
		[ResDescription("DbConnectionString_DataSource")]
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Data Source")]
		[ResCategory("DataCategory_Source")]
		public string DataSource
		{
			get
			{
				return this._dataSource;
			}
			set
			{
				this.SetValue("Data Source", value);
				this._dataSource = value;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001ED5 RID: 7893 RVA: 0x00259410 File Offset: 0x00258810
		// (set) Token: 0x06001ED6 RID: 7894 RVA: 0x00259424 File Offset: 0x00258824
		[ResCategory("DataCategory_NamedConnectionString")]
		[Editor("System.Windows.Forms.Design.FileNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("File Name")]
		[ResDescription("DbConnectionString_FileName")]
		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				this.SetValue("File Name", value);
				this._fileName = value;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001ED7 RID: 7895 RVA: 0x00259444 File Offset: 0x00258844
		// (set) Token: 0x06001ED8 RID: 7896 RVA: 0x00259458 File Offset: 0x00258858
		[DisplayName("OLE DB Services")]
		[TypeConverter(typeof(OleDbConnectionStringBuilder.OleDbServicesConverter))]
		[ResDescription("DbConnectionString_OleDbServices")]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Pooling")]
		public int OleDbServices
		{
			get
			{
				return this._oleDbServices;
			}
			set
			{
				this.SetValue("OLE DB Services", value);
				this._oleDbServices = value;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x00259478 File Offset: 0x00258878
		// (set) Token: 0x06001EDA RID: 7898 RVA: 0x0025948C File Offset: 0x0025888C
		[ResCategory("DataCategory_Security")]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_PersistSecurityInfo")]
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

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001EDB RID: 7899 RVA: 0x002594AC File Offset: 0x002588AC
		// (set) Token: 0x06001EDC RID: 7900 RVA: 0x002594C0 File Offset: 0x002588C0
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Provider")]
		[ResCategory("DataCategory_Source")]
		[TypeConverter(typeof(OleDbConnectionStringBuilder.OleDbProviderConverter))]
		[ResDescription("DbConnectionString_Provider")]
		public string Provider
		{
			get
			{
				return this._provider;
			}
			set
			{
				this.SetValue("Provider", value);
				this._provider = value;
				this.RestartProvider();
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001EDD RID: 7901 RVA: 0x002594E8 File Offset: 0x002588E8
		public override ICollection Keys
		{
			get
			{
				string[] array = this._knownKeywords;
				if (array == null)
				{
					Dictionary<string, OleDbPropertyInfo> providerInfo = this.GetProviderInfo(this.Provider);
					if (0 < providerInfo.Count)
					{
						array = new string[OleDbConnectionStringBuilder._validKeywords.Length + providerInfo.Count];
						OleDbConnectionStringBuilder._validKeywords.CopyTo(array, 0);
						providerInfo.Keys.CopyTo(array, OleDbConnectionStringBuilder._validKeywords.Length);
					}
					else
					{
						array = OleDbConnectionStringBuilder._validKeywords;
					}
					int num = 0;
					foreach (object obj in base.Keys)
					{
						string text = (string)obj;
						bool flag = true;
						foreach (string text2 in array)
						{
							if (StringComparer.OrdinalIgnoreCase.Equals(text2, text))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							num++;
						}
					}
					if (0 < num)
					{
						string[] array3 = new string[array.Length + num];
						array.CopyTo(array3, 0);
						int num2 = array.Length;
						foreach (object obj2 in base.Keys)
						{
							string text3 = (string)obj2;
							bool flag2 = true;
							foreach (string text4 in array)
							{
								if (StringComparer.OrdinalIgnoreCase.Equals(text4, text3))
								{
									flag2 = false;
									break;
								}
							}
							if (flag2)
							{
								array3[num2++] = text3;
							}
						}
						array = array3;
					}
					this._knownKeywords = array;
				}
				return new ReadOnlyCollection<string>(array);
			}
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x002596B0 File Offset: 0x00258AB0
		public override bool ContainsKey(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			return OleDbConnectionStringBuilder._keywords.ContainsKey(keyword) || base.ContainsKey(keyword);
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x002596E0 File Offset: 0x00258AE0
		private static bool ConvertToBoolean(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToBoolean(value);
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x002596F4 File Offset: 0x00258AF4
		private static int ConvertToInt32(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToInt32(value);
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x00259708 File Offset: 0x00258B08
		private static string ConvertToString(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToString(value);
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x0025971C File Offset: 0x00258B1C
		public override void Clear()
		{
			base.Clear();
			for (int i = 0; i < OleDbConnectionStringBuilder._validKeywords.Length; i++)
			{
				this.Reset((OleDbConnectionStringBuilder.Keywords)i);
			}
			base.ClearPropertyDescriptors();
			this._knownKeywords = OleDbConnectionStringBuilder._validKeywords;
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0025975C File Offset: 0x00258B5C
		private object GetAt(OleDbConnectionStringBuilder.Keywords index)
		{
			switch (index)
			{
			case OleDbConnectionStringBuilder.Keywords.FileName:
				return this.FileName;
			case OleDbConnectionStringBuilder.Keywords.Provider:
				return this.Provider;
			case OleDbConnectionStringBuilder.Keywords.DataSource:
				return this.DataSource;
			case OleDbConnectionStringBuilder.Keywords.PersistSecurityInfo:
				return this.PersistSecurityInfo;
			case OleDbConnectionStringBuilder.Keywords.OleDbServices:
				return this.OleDbServices;
			default:
				throw ADP.KeywordNotSupported(OleDbConnectionStringBuilder._validKeywords[(int)index]);
			}
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x002597C0 File Offset: 0x00258BC0
		public override bool Remove(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			bool flag = base.Remove(keyword);
			OleDbConnectionStringBuilder.Keywords keywords;
			if (OleDbConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
			{
				this.Reset(keywords);
			}
			else if (flag)
			{
				this.ClearPropertyDescriptors();
			}
			return flag;
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x00259804 File Offset: 0x00258C04
		private void Reset(OleDbConnectionStringBuilder.Keywords index)
		{
			switch (index)
			{
			case OleDbConnectionStringBuilder.Keywords.FileName:
				this._fileName = "";
				this.RestartProvider();
				return;
			case OleDbConnectionStringBuilder.Keywords.Provider:
				this._provider = "";
				this.RestartProvider();
				return;
			case OleDbConnectionStringBuilder.Keywords.DataSource:
				this._dataSource = "";
				return;
			case OleDbConnectionStringBuilder.Keywords.PersistSecurityInfo:
				this._persistSecurityInfo = false;
				return;
			case OleDbConnectionStringBuilder.Keywords.OleDbServices:
				this._oleDbServices = -13;
				return;
			default:
				throw ADP.KeywordNotSupported(OleDbConnectionStringBuilder._validKeywords[(int)index]);
			}
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x0025987C File Offset: 0x00258C7C
		private new void ClearPropertyDescriptors()
		{
			base.ClearPropertyDescriptors();
			this._knownKeywords = null;
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x00259898 File Offset: 0x00258C98
		private void RestartProvider()
		{
			this.ClearPropertyDescriptors();
			this._propertyInfo = null;
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x002598B4 File Offset: 0x00258CB4
		private void SetValue(string keyword, bool value)
		{
			base[keyword] = value.ToString(null);
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x002598D0 File Offset: 0x00258CD0
		private void SetValue(string keyword, int value)
		{
			base[keyword] = value.ToString(null);
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x002598EC File Offset: 0x00258CEC
		private void SetValue(string keyword, string value)
		{
			ADP.CheckArgumentNull(value, keyword);
			base[keyword] = value;
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x00259908 File Offset: 0x00258D08
		public override bool TryGetValue(string keyword, out object value)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			OleDbConnectionStringBuilder.Keywords keywords;
			if (OleDbConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
			{
				value = this.GetAt(keywords);
				return true;
			}
			if (base.TryGetValue(keyword, out value))
			{
				return true;
			}
			Dictionary<string, OleDbPropertyInfo> providerInfo = this.GetProviderInfo(this.Provider);
			OleDbPropertyInfo oleDbPropertyInfo;
			if (providerInfo.TryGetValue(keyword, out oleDbPropertyInfo))
			{
				value = oleDbPropertyInfo._defaultValue;
				return true;
			}
			return false;
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x0025996C File Offset: 0x00258D6C
		private Dictionary<string, OleDbPropertyInfo> GetProviderInfo(string provider)
		{
			Dictionary<string, OleDbPropertyInfo> dictionary = this._propertyInfo;
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, OleDbPropertyInfo>(StringComparer.OrdinalIgnoreCase);
				if (!ADP.IsEmpty(provider))
				{
					Dictionary<string, OleDbPropertyInfo> dictionary2 = null;
					try
					{
						StringBuilder stringBuilder = new StringBuilder();
						DbConnectionStringBuilder.AppendKeyValuePair(stringBuilder, "Provider", provider);
						OleDbConnectionString oleDbConnectionString = new OleDbConnectionString(stringBuilder.ToString(), true);
						oleDbConnectionString.CreatePermissionSet().Demand();
						using (OleDbConnectionInternal oleDbConnectionInternal = new OleDbConnectionInternal(oleDbConnectionString, null))
						{
							dictionary2 = oleDbConnectionInternal.GetPropertyInfo(new Guid[] { OleDbPropertySetGuid.DBInitAll });
							foreach (KeyValuePair<string, OleDbPropertyInfo> keyValuePair in dictionary2)
							{
								OleDbPropertyInfo value = keyValuePair.Value;
								OleDbConnectionStringBuilder.Keywords keywords;
								if (!OleDbConnectionStringBuilder._keywords.TryGetValue(value._description, out keywords) && (!(OleDbPropertySetGuid.DBInit == value._propertySet) || (200 != value._propertyID && 60 != value._propertyID && 64 != value._propertyID)))
								{
									dictionary[value._description] = value;
								}
							}
							List<Guid> list = new List<Guid>();
							foreach (KeyValuePair<string, OleDbPropertyInfo> keyValuePair2 in dictionary2)
							{
								OleDbPropertyInfo value2 = keyValuePair2.Value;
								if (!list.Contains(value2._propertySet))
								{
									list.Add(value2._propertySet);
								}
							}
							Guid[] array = new Guid[list.Count];
							list.CopyTo(array, 0);
							using (PropertyIDSet propertyIDSet = new PropertyIDSet(array))
							{
								using (IDBPropertiesWrapper idbpropertiesWrapper = oleDbConnectionInternal.IDBProperties())
								{
									OleDbHResult oleDbHResult;
									using (DBPropSet dbpropSet = new DBPropSet(idbpropertiesWrapper.Value, propertyIDSet, out oleDbHResult))
									{
										if (OleDbHResult.S_OK <= oleDbHResult)
										{
											int propertySetCount = dbpropSet.PropertySetCount;
											for (int i = 0; i < propertySetCount; i++)
											{
												Guid guid;
												tagDBPROP[] propertySet = dbpropSet.GetPropertySet(i, out guid);
												foreach (tagDBPROP tagDBPROP in propertySet)
												{
													foreach (KeyValuePair<string, OleDbPropertyInfo> keyValuePair3 in dictionary2)
													{
														OleDbPropertyInfo value3 = keyValuePair3.Value;
														if (value3._propertyID == tagDBPROP.dwPropertyID && value3._propertySet == guid)
														{
															value3._defaultValue = tagDBPROP.vValue;
															if (value3._defaultValue == null)
															{
																if (typeof(string) == value3._type)
																{
																	value3._defaultValue = "";
																}
																else if (typeof(int) == value3._type)
																{
																	value3._defaultValue = 0;
																}
																else if (typeof(bool) == value3._type)
																{
																	value3._defaultValue = false;
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
					catch (InvalidOperationException ex)
					{
						ADP.TraceExceptionWithoutRethrow(ex);
					}
					catch (OleDbException ex2)
					{
						ADP.TraceExceptionWithoutRethrow(ex2);
					}
					catch (SecurityException ex3)
					{
						ADP.TraceExceptionWithoutRethrow(ex3);
					}
				}
				this._propertyInfo = dictionary;
			}
			return dictionary;
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x00259D98 File Offset: 0x00259198
		protected override void GetProperties(Hashtable propertyDescriptors)
		{
			Dictionary<string, OleDbPropertyInfo> providerInfo = this.GetProviderInfo(this.Provider);
			if (0 < providerInfo.Count)
			{
				foreach (OleDbPropertyInfo oleDbPropertyInfo in providerInfo.Values)
				{
					OleDbConnectionStringBuilder.Keywords keywords;
					if (!OleDbConnectionStringBuilder._keywords.TryGetValue(oleDbPropertyInfo._description, out keywords))
					{
						bool flag = false;
						bool flag2 = false;
						Attribute[] array;
						if (OleDbPropertySetGuid.DBInit == oleDbPropertyInfo._propertySet)
						{
							int propertyID = oleDbPropertyInfo._propertyID;
							if (propertyID <= 160)
							{
								switch (propertyID)
								{
								case 5:
								case 6:
								case 7:
								case 8:
								case 10:
									array = new Attribute[]
									{
										BrowsableAttribute.Yes,
										new ResCategoryAttribute("DataCategory_Security"),
										RefreshPropertiesAttribute.All
									};
									flag2 = 7 == oleDbPropertyInfo._propertyID;
									goto IL_0303;
								case 9:
									array = new Attribute[]
									{
										BrowsableAttribute.Yes,
										PasswordPropertyTextAttribute.Yes,
										new ResCategoryAttribute("DataCategory_Security"),
										RefreshPropertiesAttribute.All
									};
									flag = this.ContainsKey("Integrated Security");
									flag2 = true;
									goto IL_0303;
								case 11:
									goto IL_0265;
								case 12:
									array = new Attribute[]
									{
										BrowsableAttribute.Yes,
										new ResCategoryAttribute("DataCategory_Security"),
										RefreshPropertiesAttribute.All
									};
									flag = this.ContainsKey("Integrated Security");
									flag2 = true;
									goto IL_0303;
								default:
									switch (propertyID)
									{
									case 61:
									case 63:
									case 65:
										goto IL_0235;
									case 62:
										goto IL_0111;
									case 64:
										goto IL_0265;
									case 66:
										break;
									default:
										if (propertyID != 160)
										{
											goto IL_0265;
										}
										goto IL_0235;
									}
									break;
								}
							}
							else if (propertyID <= 233)
							{
								if (propertyID == 186)
								{
									goto IL_0235;
								}
								if (propertyID != 233)
								{
									goto IL_0265;
								}
								goto IL_0111;
							}
							else
							{
								switch (propertyID)
								{
								case 270:
								case 271:
									goto IL_0235;
								default:
									if (propertyID != 284)
									{
										goto IL_0265;
									}
									break;
								}
							}
							array = new Attribute[]
							{
								BrowsableAttribute.Yes,
								new ResCategoryAttribute("DataCategory_Initialization"),
								RefreshPropertiesAttribute.All
							};
							goto IL_0303;
							IL_0111:
							array = new Attribute[]
							{
								BrowsableAttribute.Yes,
								new ResCategoryAttribute("DataCategory_Source"),
								RefreshPropertiesAttribute.All
							};
							goto IL_0303;
							IL_0235:
							array = new Attribute[]
							{
								BrowsableAttribute.Yes,
								new ResCategoryAttribute("DataCategory_Advanced"),
								RefreshPropertiesAttribute.All
							};
							goto IL_0303;
							IL_0265:
							array = new Attribute[]
							{
								BrowsableAttribute.Yes,
								RefreshPropertiesAttribute.All
							};
						}
						else if (oleDbPropertyInfo._description.EndsWith(" Provider", StringComparison.OrdinalIgnoreCase))
						{
							array = new Attribute[]
							{
								BrowsableAttribute.Yes,
								RefreshPropertiesAttribute.All,
								new ResCategoryAttribute("DataCategory_Source"),
								new TypeConverterAttribute(typeof(OleDbConnectionStringBuilder.OleDbProviderConverter))
							};
							flag2 = true;
						}
						else
						{
							array = new Attribute[]
							{
								BrowsableAttribute.Yes,
								RefreshPropertiesAttribute.All,
								new CategoryAttribute(this.Provider)
							};
						}
						IL_0303:
						DbConnectionStringBuilderDescriptor dbConnectionStringBuilderDescriptor = new DbConnectionStringBuilderDescriptor(oleDbPropertyInfo._description, typeof(OleDbConnectionStringBuilder), oleDbPropertyInfo._type, flag, array);
						dbConnectionStringBuilderDescriptor.RefreshOnChange = flag2;
						propertyDescriptors[oleDbPropertyInfo._description] = dbConnectionStringBuilderDescriptor;
					}
				}
			}
			base.GetProperties(propertyDescriptors);
		}

		// Token: 0x0400128B RID: 4747
		private static readonly string[] _validKeywords;

		// Token: 0x0400128C RID: 4748
		private static readonly Dictionary<string, OleDbConnectionStringBuilder.Keywords> _keywords;

		// Token: 0x0400128D RID: 4749
		private string[] _knownKeywords;

		// Token: 0x0400128E RID: 4750
		private Dictionary<string, OleDbPropertyInfo> _propertyInfo;

		// Token: 0x0400128F RID: 4751
		private string _fileName = "";

		// Token: 0x04001290 RID: 4752
		private string _dataSource = "";

		// Token: 0x04001291 RID: 4753
		private string _provider = "";

		// Token: 0x04001292 RID: 4754
		private int _oleDbServices = -13;

		// Token: 0x04001293 RID: 4755
		private bool _persistSecurityInfo;

		// Token: 0x0200021C RID: 540
		private enum Keywords
		{
			// Token: 0x04001295 RID: 4757
			FileName,
			// Token: 0x04001296 RID: 4758
			Provider,
			// Token: 0x04001297 RID: 4759
			DataSource,
			// Token: 0x04001298 RID: 4760
			PersistSecurityInfo,
			// Token: 0x04001299 RID: 4761
			OleDbServices
		}

		// Token: 0x0200021D RID: 541
		private sealed class OleDbProviderConverter : StringConverter
		{
			// Token: 0x06001EEF RID: 7919 RVA: 0x0025A134 File Offset: 0x00259534
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			// Token: 0x06001EF0 RID: 7920 RVA: 0x0025A144 File Offset: 0x00259544
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}

			// Token: 0x06001EF1 RID: 7921 RVA: 0x0025A154 File Offset: 0x00259554
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				TypeConverter.StandardValuesCollection standardValuesCollection = this._standardValues;
				if (this._standardValues == null)
				{
					DataTable elements = new OleDbEnumerator().GetElements();
					DataColumn dataColumn = elements.Columns["SOURCES_NAME"];
					DataColumn dataColumn2 = elements.Columns["SOURCES_TYPE"];
					List<string> list = new List<string>(elements.Rows.Count);
					foreach (object obj in elements.Rows)
					{
						DataRow dataRow = (DataRow)obj;
						int num = (int)dataRow[dataColumn2];
						if (1 == num || 3 == num)
						{
							string text = (string)dataRow[dataColumn];
							if (!OleDbConnectionString.IsMSDASQL(text.ToLower(CultureInfo.InvariantCulture)) && 0 > list.IndexOf(text))
							{
								list.Add(text);
							}
						}
					}
					standardValuesCollection = new TypeConverter.StandardValuesCollection(list);
					this._standardValues = standardValuesCollection;
				}
				return standardValuesCollection;
			}

			// Token: 0x0400129A RID: 4762
			private const int DBSOURCETYPE_DATASOURCE_TDP = 1;

			// Token: 0x0400129B RID: 4763
			private const int DBSOURCETYPE_DATASOURCE_MDP = 3;

			// Token: 0x0400129C RID: 4764
			private TypeConverter.StandardValuesCollection _standardValues;
		}

		// Token: 0x0200021E RID: 542
		[Flags]
		internal enum OleDbServiceValues
		{
			// Token: 0x0400129E RID: 4766
			DisableAll = 0,
			// Token: 0x0400129F RID: 4767
			ResourcePooling = 1,
			// Token: 0x040012A0 RID: 4768
			TransactionEnlistment = 2,
			// Token: 0x040012A1 RID: 4769
			ClientCursor = 4,
			// Token: 0x040012A2 RID: 4770
			AggregationAfterSession = 8,
			// Token: 0x040012A3 RID: 4771
			EnableAll = -1,
			// Token: 0x040012A4 RID: 4772
			Default = -13
		}

		// Token: 0x0200021F RID: 543
		internal sealed class OleDbServicesConverter : TypeConverter
		{
			// Token: 0x06001EF3 RID: 7923 RVA: 0x0025A27C File Offset: 0x0025967C
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return typeof(string) == sourceType || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06001EF4 RID: 7924 RVA: 0x0025A2A0 File Offset: 0x002596A0
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string text = value as string;
				if (text == null)
				{
					return base.ConvertFrom(context, culture, value);
				}
				int num;
				if (int.TryParse(text, out num))
				{
					return num;
				}
				if (text.IndexOf(',') != -1)
				{
					int num2 = 0;
					string[] array = text.Split(new char[] { ',' });
					foreach (string text2 in array)
					{
						num2 |= (int)((OleDbConnectionStringBuilder.OleDbServiceValues)Enum.Parse(typeof(OleDbConnectionStringBuilder.OleDbServiceValues), text2, true));
					}
					return num2;
				}
				return (int)((OleDbConnectionStringBuilder.OleDbServiceValues)Enum.Parse(typeof(OleDbConnectionStringBuilder.OleDbServiceValues), text, true));
			}

			// Token: 0x06001EF5 RID: 7925 RVA: 0x0025A350 File Offset: 0x00259750
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return typeof(string) == destinationType || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x06001EF6 RID: 7926 RVA: 0x0025A374 File Offset: 0x00259774
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (typeof(string) == destinationType && value != null && typeof(int) == value.GetType())
				{
					return Enum.Format(typeof(OleDbConnectionStringBuilder.OleDbServiceValues), (OleDbConnectionStringBuilder.OleDbServiceValues)((int)value), "G");
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			// Token: 0x06001EF7 RID: 7927 RVA: 0x0025A3D0 File Offset: 0x002597D0
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			// Token: 0x06001EF8 RID: 7928 RVA: 0x0025A3E0 File Offset: 0x002597E0
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}

			// Token: 0x06001EF9 RID: 7929 RVA: 0x0025A3F0 File Offset: 0x002597F0
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				TypeConverter.StandardValuesCollection standardValuesCollection = this._standardValues;
				if (standardValuesCollection == null)
				{
					Array values = Enum.GetValues(typeof(OleDbConnectionStringBuilder.OleDbServiceValues));
					Array.Sort(values, 0, values.Length);
					standardValuesCollection = new TypeConverter.StandardValuesCollection(values);
					this._standardValues = standardValuesCollection;
				}
				return standardValuesCollection;
			}

			// Token: 0x06001EFA RID: 7930 RVA: 0x0025A434 File Offset: 0x00259834
			public override bool IsValid(ITypeDescriptorContext context, object value)
			{
				return true;
			}

			// Token: 0x040012A5 RID: 4773
			private TypeConverter.StandardValuesCollection _standardValues;
		}

		// Token: 0x02000220 RID: 544
		internal sealed class OleDbConnectionStringBuilderConverter : ExpandableObjectConverter
		{
			// Token: 0x06001EFC RID: 7932 RVA: 0x0025A458 File Offset: 0x00259858
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return typeof(InstanceDescriptor) == destinationType || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x06001EFD RID: 7933 RVA: 0x0025A47C File Offset: 0x0025987C
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw ADP.ArgumentNull("destinationType");
				}
				if (typeof(InstanceDescriptor) == destinationType)
				{
					OleDbConnectionStringBuilder oleDbConnectionStringBuilder = value as OleDbConnectionStringBuilder;
					if (oleDbConnectionStringBuilder != null)
					{
						return this.ConvertToInstanceDescriptor(oleDbConnectionStringBuilder);
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			// Token: 0x06001EFE RID: 7934 RVA: 0x0025A4C4 File Offset: 0x002598C4
			private InstanceDescriptor ConvertToInstanceDescriptor(OleDbConnectionStringBuilder options)
			{
				Type[] array = new Type[] { typeof(string) };
				object[] array2 = new object[] { options.ConnectionString };
				ConstructorInfo constructor = typeof(OleDbConnectionStringBuilder).GetConstructor(array);
				return new InstanceDescriptor(constructor, array2);
			}
		}
	}
}
