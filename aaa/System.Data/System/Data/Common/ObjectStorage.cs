using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000159 RID: 345
	internal sealed class ObjectStorage : DataStorage
	{
		// Token: 0x060015B0 RID: 5552 RVA: 0x0022C04C File Offset: 0x0022B44C
		internal ObjectStorage(DataColumn column, Type type)
			: base(column, type, ObjectStorage.defaultValue, DBNull.Value, typeof(ICloneable).IsAssignableFrom(type))
		{
			this.implementsIXmlSerializable = typeof(IXmlSerializable).IsAssignableFrom(type);
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0022C094 File Offset: 0x0022B494
		public override object Aggregate(int[] records, AggregateType kind)
		{
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0022C0B0 File Offset: 0x0022B4B0
		public override int Compare(int recordNo1, int recordNo2)
		{
			object obj = this.values[recordNo1];
			object obj2 = this.values[recordNo2];
			if (obj == obj2)
			{
				return 0;
			}
			if (obj == null)
			{
				return -1;
			}
			if (obj2 == null)
			{
				return 1;
			}
			IComparable comparable = obj as IComparable;
			if (comparable != null)
			{
				try
				{
					return comparable.CompareTo(obj2);
				}
				catch (ArgumentException ex)
				{
					ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				}
			}
			return this.CompareWithFamilies(obj, obj2);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0022C124 File Offset: 0x0022B524
		public override int CompareValueTo(int recordNo1, object value)
		{
			object obj = this.Get(recordNo1);
			if (obj is IComparable && value.GetType() == obj.GetType())
			{
				return ((IComparable)obj).CompareTo(value);
			}
			if (obj == value)
			{
				return 0;
			}
			if (obj == null)
			{
				if (this.NullValue == value)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (this.NullValue == value || value == null)
				{
					return 1;
				}
				return this.CompareWithFamilies(obj, value);
			}
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0022C188 File Offset: 0x0022B588
		private int CompareTo(object valueNo1, object valueNo2)
		{
			if (valueNo1 == null)
			{
				return -1;
			}
			if (valueNo2 == null)
			{
				return 1;
			}
			if (valueNo1 == valueNo2)
			{
				return 0;
			}
			if (valueNo1 == this.NullValue)
			{
				return -1;
			}
			if (valueNo2 == this.NullValue)
			{
				return 1;
			}
			if (valueNo1 is IComparable)
			{
				try
				{
					return ((IComparable)valueNo1).CompareTo(valueNo2);
				}
				catch (ArgumentException ex)
				{
					ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
				}
			}
			return this.CompareWithFamilies(valueNo1, valueNo2);
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x0022C204 File Offset: 0x0022B604
		private int CompareWithFamilies(object valueNo1, object valueNo2)
		{
			ObjectStorage.Families family = this.GetFamily(valueNo1.GetType());
			ObjectStorage.Families family2 = this.GetFamily(valueNo2.GetType());
			if (family < family2)
			{
				return -1;
			}
			if (family > family2)
			{
				return 1;
			}
			switch (family)
			{
			case ObjectStorage.Families.DATETIME:
				valueNo1 = Convert.ToDateTime(valueNo1, base.FormatProvider);
				valueNo2 = Convert.ToDateTime(valueNo1, base.FormatProvider);
				goto IL_0138;
			case ObjectStorage.Families.NUMBER:
				valueNo1 = Convert.ToDouble(valueNo1, base.FormatProvider);
				valueNo2 = Convert.ToDouble(valueNo2, base.FormatProvider);
				goto IL_0138;
			case ObjectStorage.Families.BOOLEAN:
				valueNo1 = Convert.ToBoolean(valueNo1, base.FormatProvider);
				valueNo2 = Convert.ToBoolean(valueNo2, base.FormatProvider);
				goto IL_0138;
			case ObjectStorage.Families.ARRAY:
			{
				Array array = (Array)valueNo1;
				Array array2 = (Array)valueNo2;
				if (array.Length > array2.Length)
				{
					return 1;
				}
				if (array.Length < array2.Length)
				{
					return -1;
				}
				for (int i = 0; i < array.Length; i++)
				{
					int num = this.CompareTo(array.GetValue(i), array2.GetValue(i));
					if (num != 0)
					{
						return num;
					}
				}
				return 0;
			}
			}
			valueNo1 = valueNo1.ToString();
			valueNo2 = valueNo2.ToString();
			IL_0138:
			return ((IComparable)valueNo1).CompareTo(valueNo2);
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x0022C358 File Offset: 0x0022B758
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x0022C378 File Offset: 0x0022B778
		public override object Get(int recordNo)
		{
			object obj = this.values[recordNo];
			if (obj != null)
			{
				return obj;
			}
			return this.NullValue;
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0022C39C File Offset: 0x0022B79C
		private ObjectStorage.Families GetFamily(Type dataType)
		{
			switch (Type.GetTypeCode(dataType))
			{
			case TypeCode.Boolean:
				return ObjectStorage.Families.BOOLEAN;
			case TypeCode.Char:
				return ObjectStorage.Families.STRING;
			case TypeCode.SByte:
				return ObjectStorage.Families.STRING;
			case TypeCode.Byte:
				return ObjectStorage.Families.STRING;
			case TypeCode.Int16:
				return ObjectStorage.Families.NUMBER;
			case TypeCode.UInt16:
				return ObjectStorage.Families.NUMBER;
			case TypeCode.Int32:
				return ObjectStorage.Families.NUMBER;
			case TypeCode.UInt32:
				return ObjectStorage.Families.NUMBER;
			case TypeCode.Int64:
				return ObjectStorage.Families.NUMBER;
			case TypeCode.UInt64:
				return ObjectStorage.Families.NUMBER;
			case TypeCode.Single:
				return ObjectStorage.Families.NUMBER;
			case TypeCode.Double:
				return ObjectStorage.Families.NUMBER;
			case TypeCode.Decimal:
				return ObjectStorage.Families.NUMBER;
			case TypeCode.DateTime:
				return ObjectStorage.Families.DATETIME;
			case TypeCode.String:
				return ObjectStorage.Families.STRING;
			}
			if (typeof(TimeSpan) == dataType)
			{
				return ObjectStorage.Families.DATETIME;
			}
			if (dataType.IsArray)
			{
				return ObjectStorage.Families.ARRAY;
			}
			return ObjectStorage.Families.STRING;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0022C434 File Offset: 0x0022B834
		public override bool IsNull(int record)
		{
			return null == this.values[record];
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0022C44C File Offset: 0x0022B84C
		public override void Set(int recordNo, object value)
		{
			if (this.NullValue == value)
			{
				this.values[recordNo] = null;
				return;
			}
			if (this.DataType == typeof(object) || this.DataType.IsInstanceOfType(value))
			{
				this.values[recordNo] = value;
				return;
			}
			Type type = value.GetType();
			if (this.DataType == typeof(Guid) && type == typeof(string))
			{
				this.values[recordNo] = new Guid((string)value);
				return;
			}
			if (this.DataType != typeof(byte[]))
			{
				throw ExceptionBuilder.StorageSetFailed();
			}
			if (type == typeof(bool))
			{
				this.values[recordNo] = BitConverter.GetBytes((bool)value);
				return;
			}
			if (type == typeof(char))
			{
				this.values[recordNo] = BitConverter.GetBytes((char)value);
				return;
			}
			if (type == typeof(short))
			{
				this.values[recordNo] = BitConverter.GetBytes((short)value);
				return;
			}
			if (type == typeof(int))
			{
				this.values[recordNo] = BitConverter.GetBytes((int)value);
				return;
			}
			if (type == typeof(long))
			{
				this.values[recordNo] = BitConverter.GetBytes((long)value);
				return;
			}
			if (type == typeof(ushort))
			{
				this.values[recordNo] = BitConverter.GetBytes((ushort)value);
				return;
			}
			if (type == typeof(uint))
			{
				this.values[recordNo] = BitConverter.GetBytes((uint)value);
				return;
			}
			if (type == typeof(ulong))
			{
				this.values[recordNo] = BitConverter.GetBytes((ulong)value);
				return;
			}
			if (type == typeof(float))
			{
				this.values[recordNo] = BitConverter.GetBytes((float)value);
				return;
			}
			if (type == typeof(double))
			{
				this.values[recordNo] = BitConverter.GetBytes((double)value);
				return;
			}
			throw ExceptionBuilder.StorageSetFailed();
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x0022C640 File Offset: 0x0022BA40
		public override void SetCapacity(int capacity)
		{
			object[] array = new object[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0022C680 File Offset: 0x0022BA80
		public override object ConvertXmlToObject(string s)
		{
			Type dataType = this.DataType;
			if (dataType == typeof(byte[]))
			{
				return Convert.FromBase64String(s);
			}
			if (dataType == typeof(Type))
			{
				return Type.GetType(s);
			}
			if (dataType == typeof(Guid))
			{
				return new Guid(s);
			}
			if (dataType == typeof(Uri))
			{
				return new Uri(s);
			}
			if (this.implementsIXmlSerializable)
			{
				object obj = Activator.CreateInstance(this.DataType, true);
				StringReader stringReader = new StringReader(s);
				using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
				{
					((IXmlSerializable)obj).ReadXml(xmlTextReader);
				}
				return obj;
			}
			StringReader stringReader2 = new StringReader(s);
			XmlSerializer xmlSerializer = ObjectStorage.GetXmlSerializer(dataType);
			return xmlSerializer.Deserialize(stringReader2);
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0022C75C File Offset: 0x0022BB5C
		public override object ConvertXmlToObject(XmlReader xmlReader, XmlRootAttribute xmlAttrib)
		{
			bool flag = false;
			bool flag2 = false;
			object obj;
			if (xmlAttrib == null)
			{
				Type type = null;
				string attribute = xmlReader.GetAttribute("InstanceType", "urn:schemas-microsoft-com:xml-msdata");
				if (attribute == null || attribute.Length == 0)
				{
					string text = xmlReader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
					if (text != null && text.Length > 0)
					{
						string[] array = text.Split(new char[] { ':' });
						if (array.Length == 2 && xmlReader.LookupNamespace(array[0]) == "http://www.w3.org/2001/XMLSchema")
						{
							text = array[1];
						}
						type = XSDSchema.XsdtoClr(text);
						flag = true;
					}
					else if (this.DataType == typeof(object))
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					obj = xmlReader.ReadString();
				}
				else if (attribute == "Type")
				{
					obj = Type.GetType(xmlReader.ReadString());
					xmlReader.Read();
				}
				else
				{
					if (type == null)
					{
						type = ((attribute == null) ? this.DataType : Type.GetType(attribute));
					}
					if (type == typeof(char) || type == typeof(Guid))
					{
						flag = true;
					}
					if (type == typeof(object))
					{
						throw ExceptionBuilder.CanNotDeserializeObjectType();
					}
					TypeLimiter.EnsureTypeIsAllowed(type);
					if (!flag)
					{
						obj = Activator.CreateInstance(type, true);
						((IXmlSerializable)obj).ReadXml(xmlReader);
					}
					else
					{
						if (type == typeof(string) && xmlReader.NodeType == XmlNodeType.Element && xmlReader.IsEmptyElement)
						{
							obj = string.Empty;
						}
						else
						{
							obj = xmlReader.ReadString();
							if (type != typeof(byte[]))
							{
								obj = SqlConvert.ChangeTypeForXML(obj, type);
							}
							else
							{
								obj = Convert.FromBase64String(obj.ToString());
							}
						}
						xmlReader.Read();
					}
				}
			}
			else
			{
				XmlSerializer xmlSerializer = ObjectStorage.GetXmlSerializer(this.DataType, xmlAttrib);
				obj = xmlSerializer.Deserialize(xmlReader);
			}
			return obj;
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x0022C91C File Offset: 0x0022BD1C
		public override string ConvertObjectToXml(object value)
		{
			if (value == null || value == this.NullValue)
			{
				return string.Empty;
			}
			Type dataType = this.DataType;
			if (dataType == typeof(byte[]) || (dataType == typeof(object) && value is byte[]))
			{
				return Convert.ToBase64String((byte[])value);
			}
			if (dataType == typeof(Type) || (dataType == typeof(object) && value is Type))
			{
				return ((Type)value).AssemblyQualifiedName;
			}
			if (!DataStorage.IsTypeCustomType(value.GetType()))
			{
				return (string)SqlConvert.ChangeTypeForXML(value, typeof(string));
			}
			if (Type.GetTypeCode(value.GetType()) != TypeCode.Object)
			{
				return value.ToString();
			}
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			if (this.implementsIXmlSerializable)
			{
				using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
				{
					((IXmlSerializable)value).WriteXml(xmlTextWriter);
				}
				return stringWriter.ToString();
			}
			XmlSerializer xmlSerializer = ObjectStorage.GetXmlSerializer(value.GetType());
			xmlSerializer.Serialize(stringWriter, value);
			return stringWriter.ToString();
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x0022CA48 File Offset: 0x0022BE48
		public override void ConvertObjectToXml(object value, XmlWriter xmlWriter, XmlRootAttribute xmlAttrib)
		{
			if (xmlAttrib == null)
			{
				((IXmlSerializable)value).WriteXml(xmlWriter);
				return;
			}
			XmlSerializer xmlSerializer = ObjectStorage.GetXmlSerializer(value.GetType(), xmlAttrib);
			xmlSerializer.Serialize(xmlWriter, value);
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0022CA7C File Offset: 0x0022BE7C
		protected override object GetEmptyStorage(int recordCount)
		{
			return new object[recordCount];
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0022CA90 File Offset: 0x0022BE90
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			object[] array = (object[])store;
			array[storeIndex] = this.values[record];
			bool flag = this.IsNull(record);
			nullbits.Set(storeIndex, flag);
			if (!flag && array[storeIndex] is DateTime)
			{
				DateTime dateTime = (DateTime)array[storeIndex];
				if (dateTime.Kind == DateTimeKind.Local)
				{
					array[storeIndex] = DateTime.SpecifyKind(dateTime.ToUniversalTime(), DateTimeKind.Local);
				}
			}
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0022CAFC File Offset: 0x0022BEFC
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (object[])store;
			for (int i = 0; i < this.values.Length; i++)
			{
				if (this.values[i] is DateTime)
				{
					DateTime dateTime = (DateTime)this.values[i];
					if (dateTime.Kind == DateTimeKind.Local)
					{
						this.values[i] = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).ToLocalTime();
					}
				}
			}
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0022CB6C File Offset: 0x0022BF6C
		internal static XmlSerializer GetXmlSerializer(Type type)
		{
			return ObjectStorage._serializerFactory.CreateSerializer(type);
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0022CB88 File Offset: 0x0022BF88
		internal static XmlSerializer GetXmlSerializer(Type type, XmlRootAttribute attribute)
		{
			XmlSerializer xmlSerializer = null;
			KeyValuePair<Type, XmlRootAttribute> keyValuePair = new KeyValuePair<Type, XmlRootAttribute>(type, attribute);
			Dictionary<KeyValuePair<Type, XmlRootAttribute>, XmlSerializer> dictionary = ObjectStorage._tempAssemblyCache;
			if (dictionary == null || !dictionary.TryGetValue(keyValuePair, out xmlSerializer))
			{
				lock (ObjectStorage._tempAssemblyCacheLock)
				{
					dictionary = ObjectStorage._tempAssemblyCache;
					if (dictionary == null || !dictionary.TryGetValue(keyValuePair, out xmlSerializer))
					{
						if (dictionary != null)
						{
							Dictionary<KeyValuePair<Type, XmlRootAttribute>, XmlSerializer> dictionary2 = new Dictionary<KeyValuePair<Type, XmlRootAttribute>, XmlSerializer>(1 + dictionary.Count, ObjectStorage.TempAssemblyComparer.Default);
							foreach (KeyValuePair<KeyValuePair<Type, XmlRootAttribute>, XmlSerializer> keyValuePair2 in dictionary)
							{
								dictionary2.Add(keyValuePair2.Key, keyValuePair2.Value);
							}
							dictionary = dictionary2;
						}
						else
						{
							dictionary = new Dictionary<KeyValuePair<Type, XmlRootAttribute>, XmlSerializer>(ObjectStorage.TempAssemblyComparer.Default);
						}
						keyValuePair = new KeyValuePair<Type, XmlRootAttribute>(type, new XmlRootAttribute());
						keyValuePair.Value.ElementName = attribute.ElementName;
						keyValuePair.Value.Namespace = attribute.Namespace;
						keyValuePair.Value.DataType = attribute.DataType;
						keyValuePair.Value.IsNullable = attribute.IsNullable;
						xmlSerializer = ObjectStorage._serializerFactory.CreateSerializer(type, attribute);
						dictionary.Add(keyValuePair, xmlSerializer);
						ObjectStorage._tempAssemblyCache = dictionary;
					}
				}
			}
			return xmlSerializer;
		}

		// Token: 0x04000CA8 RID: 3240
		private static readonly object defaultValue = null;

		// Token: 0x04000CA9 RID: 3241
		private object[] values;

		// Token: 0x04000CAA RID: 3242
		private readonly bool implementsIXmlSerializable;

		// Token: 0x04000CAB RID: 3243
		private static readonly object _tempAssemblyCacheLock = new object();

		// Token: 0x04000CAC RID: 3244
		private static Dictionary<KeyValuePair<Type, XmlRootAttribute>, XmlSerializer> _tempAssemblyCache;

		// Token: 0x04000CAD RID: 3245
		private static readonly XmlSerializerFactory _serializerFactory = new XmlSerializerFactory();

		// Token: 0x0200015A RID: 346
		private enum Families
		{
			// Token: 0x04000CAF RID: 3247
			DATETIME,
			// Token: 0x04000CB0 RID: 3248
			NUMBER,
			// Token: 0x04000CB1 RID: 3249
			STRING,
			// Token: 0x04000CB2 RID: 3250
			BOOLEAN,
			// Token: 0x04000CB3 RID: 3251
			ARRAY
		}

		// Token: 0x0200015B RID: 347
		private class TempAssemblyComparer : IEqualityComparer<KeyValuePair<Type, XmlRootAttribute>>
		{
			// Token: 0x060015C6 RID: 5574 RVA: 0x0022CD18 File Offset: 0x0022C118
			private TempAssemblyComparer()
			{
			}

			// Token: 0x060015C7 RID: 5575 RVA: 0x0022CD2C File Offset: 0x0022C12C
			public bool Equals(KeyValuePair<Type, XmlRootAttribute> x, KeyValuePair<Type, XmlRootAttribute> y)
			{
				return x.Key == y.Key && ((x.Value == null && y.Value == null) || (x.Value != null && y.Value != null && x.Value.ElementName == y.Value.ElementName && x.Value.Namespace == y.Value.Namespace && x.Value.DataType == y.Value.DataType && x.Value.IsNullable == y.Value.IsNullable));
			}

			// Token: 0x060015C8 RID: 5576 RVA: 0x0022CDF8 File Offset: 0x0022C1F8
			public int GetHashCode(KeyValuePair<Type, XmlRootAttribute> obj)
			{
				return obj.Key.GetHashCode() + obj.Value.ElementName.GetHashCode();
			}

			// Token: 0x04000CB4 RID: 3252
			internal static readonly IEqualityComparer<KeyValuePair<Type, XmlRootAttribute>> Default = new ObjectStorage.TempAssemblyComparer();
		}
	}
}
