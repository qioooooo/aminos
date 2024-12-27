using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace System.Data.Odbc
{
	// Token: 0x020001E1 RID: 481
	[TypeConverter(typeof(OdbcConnectionStringBuilder.OdbcConnectionStringBuilderConverter))]
	[DefaultProperty("Driver")]
	public sealed class OdbcConnectionStringBuilder : DbConnectionStringBuilder
	{
		// Token: 0x06001AE4 RID: 6884 RVA: 0x00244550 File Offset: 0x00243950
		static OdbcConnectionStringBuilder()
		{
			string[] array = new string[] { null, "Driver" };
			array[0] = "Dsn";
			OdbcConnectionStringBuilder._validKeywords = array;
			OdbcConnectionStringBuilder._keywords = new Dictionary<string, OdbcConnectionStringBuilder.Keywords>(2, StringComparer.OrdinalIgnoreCase)
			{
				{
					"Driver",
					OdbcConnectionStringBuilder.Keywords.Driver
				},
				{
					"Dsn",
					OdbcConnectionStringBuilder.Keywords.Dsn
				}
			};
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x002445A4 File Offset: 0x002439A4
		public OdbcConnectionStringBuilder()
			: this(null)
		{
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x002445B8 File Offset: 0x002439B8
		public OdbcConnectionStringBuilder(string connectionString)
			: base(true)
		{
			if (!ADP.IsEmpty(connectionString))
			{
				base.ConnectionString = connectionString;
			}
		}

		// Token: 0x1700038B RID: 907
		public override object this[string keyword]
		{
			get
			{
				Bid.Trace("<comm.OdbcConnectionStringBuilder.get_Item|API> keyword='%ls'\n", keyword);
				ADP.CheckArgumentNull(keyword, "keyword");
				OdbcConnectionStringBuilder.Keywords keywords;
				if (OdbcConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
				{
					return this.GetAt(keywords);
				}
				return base[keyword];
			}
			set
			{
				Bid.Trace("<comm.OdbcConnectionStringBuilder.set_Item|API> keyword='%ls'\n", keyword);
				ADP.CheckArgumentNull(keyword, "keyword");
				if (value == null)
				{
					this.Remove(keyword);
					return;
				}
				OdbcConnectionStringBuilder.Keywords keywords;
				if (!OdbcConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
				{
					base[keyword] = value;
					base.ClearPropertyDescriptors();
					this._knownKeywords = null;
					return;
				}
				switch (keywords)
				{
				case OdbcConnectionStringBuilder.Keywords.Dsn:
					this.Dsn = OdbcConnectionStringBuilder.ConvertToString(value);
					return;
				case OdbcConnectionStringBuilder.Keywords.Driver:
					this.Driver = OdbcConnectionStringBuilder.ConvertToString(value);
					return;
				default:
					throw ADP.KeywordNotSupported(keyword);
				}
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x002446C0 File Offset: 0x00243AC0
		// (set) Token: 0x06001AEA RID: 6890 RVA: 0x002446D4 File Offset: 0x00243AD4
		[ResCategory("DataCategory_Source")]
		[DisplayName("Driver")]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_Driver")]
		public string Driver
		{
			get
			{
				return this._driver;
			}
			set
			{
				this.SetValue("Driver", value);
				this._driver = value;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001AEB RID: 6891 RVA: 0x002446F4 File Offset: 0x00243AF4
		// (set) Token: 0x06001AEC RID: 6892 RVA: 0x00244708 File Offset: 0x00243B08
		[DisplayName("Dsn")]
		[ResDescription("DbConnectionString_DSN")]
		[ResCategory("DataCategory_NamedConnectionString")]
		[RefreshProperties(RefreshProperties.All)]
		public string Dsn
		{
			get
			{
				return this._dsn;
			}
			set
			{
				this.SetValue("Dsn", value);
				this._dsn = value;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x00244728 File Offset: 0x00243B28
		public override ICollection Keys
		{
			get
			{
				string[] array = this._knownKeywords;
				if (array == null)
				{
					array = OdbcConnectionStringBuilder._validKeywords;
					int num = 0;
					foreach (object obj in base.Keys)
					{
						string text = (string)obj;
						bool flag = true;
						foreach (string text2 in array)
						{
							if (text2 == text)
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
								if (text4 == text3)
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

		// Token: 0x06001AEE RID: 6894 RVA: 0x00244898 File Offset: 0x00243C98
		public override void Clear()
		{
			base.Clear();
			for (int i = 0; i < OdbcConnectionStringBuilder._validKeywords.Length; i++)
			{
				this.Reset((OdbcConnectionStringBuilder.Keywords)i);
			}
			this._knownKeywords = OdbcConnectionStringBuilder._validKeywords;
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x002448D0 File Offset: 0x00243CD0
		public override bool ContainsKey(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			return OdbcConnectionStringBuilder._keywords.ContainsKey(keyword) || base.ContainsKey(keyword);
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x00244900 File Offset: 0x00243D00
		private static string ConvertToString(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToString(value);
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x00244914 File Offset: 0x00243D14
		private object GetAt(OdbcConnectionStringBuilder.Keywords index)
		{
			switch (index)
			{
			case OdbcConnectionStringBuilder.Keywords.Dsn:
				return this.Dsn;
			case OdbcConnectionStringBuilder.Keywords.Driver:
				return this.Driver;
			default:
				throw ADP.KeywordNotSupported(OdbcConnectionStringBuilder._validKeywords[(int)index]);
			}
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x00244950 File Offset: 0x00243D50
		public override bool Remove(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			if (base.Remove(keyword))
			{
				OdbcConnectionStringBuilder.Keywords keywords;
				if (OdbcConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
				{
					this.Reset(keywords);
				}
				else
				{
					base.ClearPropertyDescriptors();
					this._knownKeywords = null;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x0024499C File Offset: 0x00243D9C
		private void Reset(OdbcConnectionStringBuilder.Keywords index)
		{
			switch (index)
			{
			case OdbcConnectionStringBuilder.Keywords.Dsn:
				this._dsn = "";
				return;
			case OdbcConnectionStringBuilder.Keywords.Driver:
				this._driver = "";
				return;
			default:
				throw ADP.KeywordNotSupported(OdbcConnectionStringBuilder._validKeywords[(int)index]);
			}
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x002449E0 File Offset: 0x00243DE0
		private void SetValue(string keyword, string value)
		{
			ADP.CheckArgumentNull(value, keyword);
			base[keyword] = value;
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x002449FC File Offset: 0x00243DFC
		public override bool TryGetValue(string keyword, out object value)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			OdbcConnectionStringBuilder.Keywords keywords;
			if (OdbcConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
			{
				value = this.GetAt(keywords);
				return true;
			}
			return base.TryGetValue(keyword, out value);
		}

		// Token: 0x04000FC8 RID: 4040
		private static readonly string[] _validKeywords;

		// Token: 0x04000FC9 RID: 4041
		private static readonly Dictionary<string, OdbcConnectionStringBuilder.Keywords> _keywords;

		// Token: 0x04000FCA RID: 4042
		private string[] _knownKeywords;

		// Token: 0x04000FCB RID: 4043
		private string _dsn = "";

		// Token: 0x04000FCC RID: 4044
		private string _driver = "";

		// Token: 0x020001E2 RID: 482
		private enum Keywords
		{
			// Token: 0x04000FCE RID: 4046
			Dsn,
			// Token: 0x04000FCF RID: 4047
			Driver
		}

		// Token: 0x020001E3 RID: 483
		internal sealed class OdbcConnectionStringBuilderConverter : ExpandableObjectConverter
		{
			// Token: 0x06001AF7 RID: 6903 RVA: 0x00244A4C File Offset: 0x00243E4C
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return typeof(InstanceDescriptor) == destinationType || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x06001AF8 RID: 6904 RVA: 0x00244A70 File Offset: 0x00243E70
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw ADP.ArgumentNull("destinationType");
				}
				if (typeof(InstanceDescriptor) == destinationType)
				{
					OdbcConnectionStringBuilder odbcConnectionStringBuilder = value as OdbcConnectionStringBuilder;
					if (odbcConnectionStringBuilder != null)
					{
						return this.ConvertToInstanceDescriptor(odbcConnectionStringBuilder);
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			// Token: 0x06001AF9 RID: 6905 RVA: 0x00244AB8 File Offset: 0x00243EB8
			private InstanceDescriptor ConvertToInstanceDescriptor(OdbcConnectionStringBuilder options)
			{
				Type[] array = new Type[] { typeof(string) };
				object[] array2 = new object[] { options.ConnectionString };
				ConstructorInfo constructor = typeof(OdbcConnectionStringBuilder).GetConstructor(array);
				return new InstanceDescriptor(constructor, array2);
			}
		}
	}
}
