using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading;

namespace System.Data.Common
{
	// Token: 0x0200012D RID: 301
	public class DbConnectionStringBuilder : IDictionary, ICollection, IEnumerable, ICustomTypeDescriptor
	{
		// Token: 0x060013BE RID: 5054 RVA: 0x00224040 File Offset: 0x00223440
		public DbConnectionStringBuilder()
		{
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00224078 File Offset: 0x00223478
		public DbConnectionStringBuilder(bool useOdbcRules)
		{
			this.UseOdbcRules = useOdbcRules;
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x002240B4 File Offset: 0x002234B4
		private ICollection Collection
		{
			get
			{
				return this.CurrentValues;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x002240C8 File Offset: 0x002234C8
		private IDictionary Dictionary
		{
			get
			{
				return this.CurrentValues;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x002240DC File Offset: 0x002234DC
		private Dictionary<string, object> CurrentValues
		{
			get
			{
				Dictionary<string, object> dictionary = this._currentValues;
				if (dictionary == null)
				{
					dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
					this._currentValues = dictionary;
				}
				return dictionary;
			}
		}

		// Token: 0x170002AB RID: 683
		object IDictionary.this[object keyword]
		{
			get
			{
				return this[this.ObjectToString(keyword)];
			}
			set
			{
				this[this.ObjectToString(keyword)] = value;
			}
		}

		// Token: 0x170002AC RID: 684
		[Browsable(false)]
		public virtual object this[string keyword]
		{
			get
			{
				Bid.Trace("<comm.DbConnectionStringBuilder.get_Item|API> %d#, keyword='%ls'\n", this.ObjectID, keyword);
				ADP.CheckArgumentNull(keyword, "keyword");
				object obj;
				if (this.CurrentValues.TryGetValue(keyword, out obj))
				{
					return obj;
				}
				throw ADP.KeywordNotSupported(keyword);
			}
			set
			{
				ADP.CheckArgumentNull(keyword, "keyword");
				bool flag;
				if (value != null)
				{
					string text = DbConnectionStringBuilderUtil.ConvertToString(value);
					DbConnectionOptions.ValidateKeyValuePair(keyword, text);
					flag = this.CurrentValues.ContainsKey(keyword);
					this.CurrentValues[keyword] = text;
				}
				else
				{
					flag = this.Remove(keyword);
				}
				this._connectionString = null;
				if (flag)
				{
					this._propertyDescriptors = null;
				}
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060013C7 RID: 5063 RVA: 0x002241E4 File Offset: 0x002235E4
		// (set) Token: 0x060013C8 RID: 5064 RVA: 0x002241F8 File Offset: 0x002235F8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignOnly(true)]
		public bool BrowsableConnectionString
		{
			get
			{
				return this._browsableConnectionString;
			}
			set
			{
				this._browsableConnectionString = value;
				this._propertyDescriptors = null;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x00224214 File Offset: 0x00223614
		// (set) Token: 0x060013CA RID: 5066 RVA: 0x002242E0 File Offset: 0x002236E0
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Data")]
		[ResDescription("DbConnectionString_ConnectionString")]
		public string ConnectionString
		{
			get
			{
				Bid.Trace("<comm.DbConnectionStringBuilder.get_ConnectionString|API> %d#\n", this.ObjectID);
				string text = this._connectionString;
				if (text == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (object obj in this.Keys)
					{
						string text2 = (string)obj;
						object obj2;
						if (this.ShouldSerialize(text2) && this.TryGetValue(text2, out obj2))
						{
							string text3 = ((obj2 != null) ? Convert.ToString(obj2, CultureInfo.InvariantCulture) : null);
							DbConnectionStringBuilder.AppendKeyValuePair(stringBuilder, text2, text3, this.UseOdbcRules);
						}
					}
					text = stringBuilder.ToString();
					this._connectionString = text;
				}
				return text;
			}
			set
			{
				Bid.Trace("<comm.DbConnectionStringBuilder.set_ConnectionString|API> %d#\n", this.ObjectID);
				DbConnectionOptions dbConnectionOptions = new DbConnectionOptions(value, null, this.UseOdbcRules);
				string connectionString = this.ConnectionString;
				this.Clear();
				try
				{
					for (NameValuePair nameValuePair = dbConnectionOptions.KeyChain; nameValuePair != null; nameValuePair = nameValuePair.Next)
					{
						if (nameValuePair.Value != null)
						{
							this[nameValuePair.Name] = nameValuePair.Value;
						}
						else
						{
							this.Remove(nameValuePair.Name);
						}
					}
					this._connectionString = null;
				}
				catch (ArgumentException)
				{
					this.ConnectionString = connectionString;
					this._connectionString = connectionString;
					throw;
				}
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060013CB RID: 5067 RVA: 0x0022438C File Offset: 0x0022378C
		[Browsable(false)]
		public virtual int Count
		{
			get
			{
				return this.CurrentValues.Count;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x002243A4 File Offset: 0x002237A4
		[Browsable(false)]
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060013CD RID: 5069 RVA: 0x002243B4 File Offset: 0x002237B4
		[Browsable(false)]
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x002243C4 File Offset: 0x002237C4
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.Collection.IsSynchronized;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060013CF RID: 5071 RVA: 0x002243DC File Offset: 0x002237DC
		[Browsable(false)]
		public virtual ICollection Keys
		{
			get
			{
				Bid.Trace("<comm.DbConnectionStringBuilder.Keys|API> %d#\n", this.ObjectID);
				return this.Dictionary.Keys;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060013D0 RID: 5072 RVA: 0x00224404 File Offset: 0x00223804
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060013D1 RID: 5073 RVA: 0x00224418 File Offset: 0x00223818
		object ICollection.SyncRoot
		{
			get
			{
				return this.Collection.SyncRoot;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x00224430 File Offset: 0x00223830
		[Browsable(false)]
		public virtual ICollection Values
		{
			get
			{
				Bid.Trace("<comm.DbConnectionStringBuilder.Values|API> %d#\n", this.ObjectID);
				ICollection<string> collection = (ICollection<string>)this.Keys;
				IEnumerator<string> enumerator = collection.GetEnumerator();
				object[] array = new object[collection.Count];
				for (int i = 0; i < array.Length; i++)
				{
					enumerator.MoveNext();
					array[i] = this[enumerator.Current];
				}
				return new ReadOnlyCollection<object>(array);
			}
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00224498 File Offset: 0x00223898
		void IDictionary.Add(object keyword, object value)
		{
			this.Add(this.ObjectToString(keyword), value);
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x002244B4 File Offset: 0x002238B4
		public void Add(string keyword, object value)
		{
			this[keyword] = value;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x002244CC File Offset: 0x002238CC
		public static void AppendKeyValuePair(StringBuilder builder, string keyword, string value)
		{
			DbConnectionOptions.AppendKeyValuePairBuilder(builder, keyword, value, false);
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x002244E4 File Offset: 0x002238E4
		public static void AppendKeyValuePair(StringBuilder builder, string keyword, string value, bool useOdbcRules)
		{
			DbConnectionOptions.AppendKeyValuePairBuilder(builder, keyword, value, useOdbcRules);
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x002244FC File Offset: 0x002238FC
		public virtual void Clear()
		{
			Bid.Trace("<comm.DbConnectionStringBuilder.Clear|API>\n");
			this._connectionString = "";
			this._propertyDescriptors = null;
			this.CurrentValues.Clear();
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00224530 File Offset: 0x00223930
		protected internal void ClearPropertyDescriptors()
		{
			this._propertyDescriptors = null;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00224544 File Offset: 0x00223944
		bool IDictionary.Contains(object keyword)
		{
			return this.ContainsKey(this.ObjectToString(keyword));
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00224560 File Offset: 0x00223960
		public virtual bool ContainsKey(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			return this.CurrentValues.ContainsKey(keyword);
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x00224584 File Offset: 0x00223984
		void ICollection.CopyTo(Array array, int index)
		{
			Bid.Trace("<comm.DbConnectionStringBuilder.ICollection.CopyTo|API> %d#\n", this.ObjectID);
			this.Collection.CopyTo(array, index);
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x002245B0 File Offset: 0x002239B0
		public virtual bool EquivalentTo(DbConnectionStringBuilder connectionStringBuilder)
		{
			ADP.CheckArgumentNull(connectionStringBuilder, "connectionStringBuilder");
			Bid.Trace("<comm.DbConnectionStringBuilder.EquivalentTo|API> %d#, connectionStringBuilder=%d#\n", this.ObjectID, connectionStringBuilder.ObjectID);
			if (base.GetType() != connectionStringBuilder.GetType() || this.CurrentValues.Count != connectionStringBuilder.CurrentValues.Count)
			{
				return false;
			}
			foreach (KeyValuePair<string, object> keyValuePair in this.CurrentValues)
			{
				object obj;
				if (!connectionStringBuilder.CurrentValues.TryGetValue(keyValuePair.Key, out obj) || !keyValuePair.Value.Equals(obj))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x00224680 File Offset: 0x00223A80
		IEnumerator IEnumerable.GetEnumerator()
		{
			Bid.Trace("<comm.DbConnectionStringBuilder.IEnumerable.GetEnumerator|API> %d#\n", this.ObjectID);
			return this.Collection.GetEnumerator();
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x002246A8 File Offset: 0x00223AA8
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			Bid.Trace("<comm.DbConnectionStringBuilder.IDictionary.GetEnumerator|API> %d#\n", this.ObjectID);
			return this.Dictionary.GetEnumerator();
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x002246D0 File Offset: 0x00223AD0
		private string ObjectToString(object keyword)
		{
			string text;
			try
			{
				text = (string)keyword;
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException("keyword", "not a string");
			}
			return text;
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x00224718 File Offset: 0x00223B18
		void IDictionary.Remove(object keyword)
		{
			this.Remove(this.ObjectToString(keyword));
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00224734 File Offset: 0x00223B34
		public virtual bool Remove(string keyword)
		{
			Bid.Trace("<comm.DbConnectionStringBuilder.Remove|API> %d#, keyword='%ls'\n", this.ObjectID, keyword);
			ADP.CheckArgumentNull(keyword, "keyword");
			if (this.CurrentValues.Remove(keyword))
			{
				this._connectionString = null;
				this._propertyDescriptors = null;
				return true;
			}
			return false;
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0022477C File Offset: 0x00223B7C
		public virtual bool ShouldSerialize(string keyword)
		{
			Bid.Trace("<comm.DbConnectionStringBuilder.ShouldSerialize|API> keyword='%ls'\n", keyword);
			ADP.CheckArgumentNull(keyword, "keyword");
			return this.CurrentValues.ContainsKey(keyword);
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x002247AC File Offset: 0x00223BAC
		public override string ToString()
		{
			return this.ConnectionString;
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x002247C0 File Offset: 0x00223BC0
		public virtual bool TryGetValue(string keyword, out object value)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			return this.CurrentValues.TryGetValue(keyword, out value);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x002247E8 File Offset: 0x00223BE8
		internal Attribute[] GetAttributesFromCollection(AttributeCollection collection)
		{
			Attribute[] array = new Attribute[collection.Count];
			collection.CopyTo(array, 0);
			return array;
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0022480C File Offset: 0x00223C0C
		private PropertyDescriptorCollection GetProperties()
		{
			PropertyDescriptorCollection propertyDescriptorCollection = this._propertyDescriptors;
			if (propertyDescriptorCollection == null)
			{
				IntPtr intPtr;
				Bid.ScopeEnter(out intPtr, "<comm.DbConnectionStringBuilder.GetProperties|INFO> %d#", this.ObjectID);
				try
				{
					Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
					this.GetProperties(hashtable);
					PropertyDescriptor[] array = new PropertyDescriptor[hashtable.Count];
					hashtable.Values.CopyTo(array, 0);
					propertyDescriptorCollection = new PropertyDescriptorCollection(array);
					this._propertyDescriptors = propertyDescriptorCollection;
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00224898 File Offset: 0x00223C98
		protected virtual void GetProperties(Hashtable propertyDescriptors)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbConnectionStringBuilder.GetProperties|API> %d#", this.ObjectID);
			try
			{
				foreach (object obj in TypeDescriptor.GetProperties(this, true))
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
					if ("ConnectionString" != propertyDescriptor.Name)
					{
						string displayName = propertyDescriptor.DisplayName;
						if (!propertyDescriptors.ContainsKey(displayName))
						{
							Attribute[] array = this.GetAttributesFromCollection(propertyDescriptor.Attributes);
							PropertyDescriptor propertyDescriptor2 = new DbConnectionStringBuilderDescriptor(propertyDescriptor.Name, propertyDescriptor.ComponentType, propertyDescriptor.PropertyType, propertyDescriptor.IsReadOnly, array);
							propertyDescriptors[displayName] = propertyDescriptor2;
						}
					}
					else if (this.BrowsableConnectionString)
					{
						propertyDescriptors["ConnectionString"] = propertyDescriptor;
					}
					else
					{
						propertyDescriptors.Remove("ConnectionString");
					}
				}
				if (!this.IsFixedSize)
				{
					Attribute[] array = null;
					foreach (object obj2 in this.Keys)
					{
						string text = (string)obj2;
						if (!propertyDescriptors.ContainsKey(text))
						{
							object obj3 = this[text];
							Type type;
							if (obj3 != null)
							{
								type = obj3.GetType();
								if (typeof(string) == type)
								{
									int num;
									bool flag;
									if (int.TryParse((string)obj3, out num))
									{
										type = typeof(int);
									}
									else if (bool.TryParse((string)obj3, out flag))
									{
										type = typeof(bool);
									}
								}
							}
							else
							{
								type = typeof(string);
							}
							Attribute[] array2 = array;
							if (StringComparer.OrdinalIgnoreCase.Equals("Password", text) || StringComparer.OrdinalIgnoreCase.Equals("pwd", text))
							{
								array2 = new Attribute[]
								{
									BrowsableAttribute.Yes,
									PasswordPropertyTextAttribute.Yes,
									new ResCategoryAttribute("DataCategory_Security"),
									RefreshPropertiesAttribute.All
								};
							}
							else if (array == null)
							{
								array = new Attribute[]
								{
									BrowsableAttribute.Yes,
									RefreshPropertiesAttribute.All
								};
								array2 = array;
							}
							PropertyDescriptor propertyDescriptor3 = new DbConnectionStringBuilderDescriptor(text, base.GetType(), type, false, array2);
							propertyDescriptors[text] = propertyDescriptor3;
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00224B2C File Offset: 0x00223F2C
		private PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = this.GetProperties();
			if (attributes == null || attributes.Length == 0)
			{
				return properties;
			}
			PropertyDescriptor[] array = new PropertyDescriptor[properties.Count];
			int num = 0;
			foreach (object obj in properties)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				bool flag = true;
				foreach (Attribute attribute in attributes)
				{
					Attribute attribute2 = propertyDescriptor.Attributes[attribute.GetType()];
					if ((attribute2 == null && !attribute.IsDefaultAttribute()) || !attribute2.Match(attribute))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					array[num] = propertyDescriptor;
					num++;
				}
			}
			PropertyDescriptor[] array2 = new PropertyDescriptor[num];
			Array.Copy(array, array2, num);
			return new PropertyDescriptorCollection(array2);
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x00224C20 File Offset: 0x00224020
		string ICustomTypeDescriptor.GetClassName()
		{
			return TypeDescriptor.GetClassName(this, true);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00224C34 File Offset: 0x00224034
		string ICustomTypeDescriptor.GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x00224C48 File Offset: 0x00224048
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this, true);
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00224C5C File Offset: 0x0022405C
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00224C74 File Offset: 0x00224074
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00224C88 File Offset: 0x00224088
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00224C9C File Offset: 0x0022409C
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return this.GetProperties();
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x00224CB0 File Offset: 0x002240B0
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			return this.GetProperties(attributes);
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x00224CC4 File Offset: 0x002240C4
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x00224CD8 File Offset: 0x002240D8
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00224CEC File Offset: 0x002240EC
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x00224D04 File Offset: 0x00224104
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		// Token: 0x04000C1D RID: 3101
		private Dictionary<string, object> _currentValues;

		// Token: 0x04000C1E RID: 3102
		private string _connectionString = "";

		// Token: 0x04000C1F RID: 3103
		private PropertyDescriptorCollection _propertyDescriptors;

		// Token: 0x04000C20 RID: 3104
		private bool _browsableConnectionString = true;

		// Token: 0x04000C21 RID: 3105
		private readonly bool UseOdbcRules;

		// Token: 0x04000C22 RID: 3106
		private static int _objectTypeCount;

		// Token: 0x04000C23 RID: 3107
		internal readonly int _objectID = Interlocked.Increment(ref DbConnectionStringBuilder._objectTypeCount);
	}
}
