using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x0200019A RID: 410
	internal sealed class SqlUdtStorage : DataStorage
	{
		// Token: 0x06001822 RID: 6178 RVA: 0x0023621C File Offset: 0x0023561C
		public SqlUdtStorage(DataColumn column, Type type)
			: this(column, type, SqlUdtStorage.GetStaticNullForUdtType(type))
		{
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x00236238 File Offset: 0x00235638
		private SqlUdtStorage(DataColumn column, Type type, object nullValue)
			: base(column, type, nullValue, nullValue, typeof(ICloneable).IsAssignableFrom(type))
		{
			this.implementsIXmlSerializable = typeof(IXmlSerializable).IsAssignableFrom(type);
			this.implementsIComparable = typeof(IComparable).IsAssignableFrom(type);
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x0023628C File Offset: 0x0023568C
		internal static object GetStaticNullForUdtType(Type type)
		{
			object obj;
			if (!SqlUdtStorage.TypeToNull.TryGetValue(type, out obj))
			{
				PropertyInfo property = type.GetProperty("Null", BindingFlags.Static | BindingFlags.Public);
				if (property != null)
				{
					obj = property.GetValue(null, null);
				}
				else
				{
					FieldInfo field = type.GetField("Null", BindingFlags.Static | BindingFlags.Public);
					if (field == null)
					{
						throw ExceptionBuilder.INullableUDTwithoutStaticNull(type.AssemblyQualifiedName);
					}
					obj = field.GetValue(null);
				}
				lock (SqlUdtStorage.TypeToNull)
				{
					SqlUdtStorage.TypeToNull[type] = obj;
				}
			}
			return obj;
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x0023632C File Offset: 0x0023572C
		public override bool IsNull(int record)
		{
			return ((INullable)this.values[record]).IsNull;
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0023634C File Offset: 0x0023574C
		public override object Aggregate(int[] records, AggregateType kind)
		{
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x00236368 File Offset: 0x00235768
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.CompareValueTo(recordNo1, this.values[recordNo2]);
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x00236384 File Offset: 0x00235784
		public override int CompareValueTo(int recordNo1, object value)
		{
			if (DBNull.Value == value)
			{
				value = this.NullValue;
			}
			if (this.implementsIComparable)
			{
				IComparable comparable = (IComparable)this.values[recordNo1];
				return comparable.CompareTo(value);
			}
			if (this.NullValue != value)
			{
				throw ExceptionBuilder.IComparableNotImplemented(this.DataType.AssemblyQualifiedName);
			}
			INullable nullable = (INullable)this.values[recordNo1];
			if (!nullable.IsNull)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x002363F4 File Offset: 0x002357F4
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0023641C File Offset: 0x0023581C
		public override object Get(int recordNo)
		{
			return this.values[recordNo];
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x00236434 File Offset: 0x00235834
		public override void Set(int recordNo, object value)
		{
			if (DBNull.Value == value)
			{
				this.values[recordNo] = this.NullValue;
				base.SetNullBit(recordNo, true);
				return;
			}
			if (value == null)
			{
				if (this.IsValueType)
				{
					throw ExceptionBuilder.StorageSetFailed();
				}
				this.values[recordNo] = this.NullValue;
				base.SetNullBit(recordNo, true);
				return;
			}
			else
			{
				if (!this.DataType.IsInstanceOfType(value))
				{
					throw ExceptionBuilder.StorageSetFailed();
				}
				this.values[recordNo] = value;
				base.SetNullBit(recordNo, false);
				return;
			}
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x002364B0 File Offset: 0x002358B0
		public override void SetCapacity(int capacity)
		{
			object[] array = new object[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x002364F8 File Offset: 0x002358F8
		public override object ConvertXmlToObject(string s)
		{
			if (this.implementsIXmlSerializable)
			{
				object obj = Activator.CreateInstance(this.DataType, true);
				string text = "<col>" + s + "</col>";
				StringReader stringReader = new StringReader(text);
				using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
				{
					((IXmlSerializable)obj).ReadXml(xmlTextReader);
				}
				return obj;
			}
			StringReader stringReader2 = new StringReader(s);
			XmlSerializer xmlSerializer = ObjectStorage.GetXmlSerializer(this.DataType);
			return xmlSerializer.Deserialize(stringReader2);
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x00236590 File Offset: 0x00235990
		public override object ConvertXmlToObject(XmlReader xmlReader, XmlRootAttribute xmlAttrib)
		{
			if (xmlAttrib == null)
			{
				string text = xmlReader.GetAttribute("InstanceType", "urn:schemas-microsoft-com:xml-msdata");
				if (text == null)
				{
					string attribute = xmlReader.GetAttribute("InstanceType", "http://www.w3.org/2001/XMLSchema-instance");
					if (attribute != null)
					{
						text = XSDSchema.XsdtoClr(attribute).FullName;
					}
				}
				Type type = ((text == null) ? this.DataType : Type.GetType(text));
				TypeLimiter.EnsureTypeIsAllowed(type);
				object obj = Activator.CreateInstance(type, true);
				((IXmlSerializable)obj).ReadXml(xmlReader);
				return obj;
			}
			XmlSerializer xmlSerializer = ObjectStorage.GetXmlSerializer(this.DataType, xmlAttrib);
			return xmlSerializer.Deserialize(xmlReader);
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x00236618 File Offset: 0x00235A18
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			if (this.implementsIXmlSerializable)
			{
				using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
				{
					((IXmlSerializable)value).WriteXml(xmlTextWriter);
					goto IL_0047;
				}
			}
			XmlSerializer xmlSerializer = ObjectStorage.GetXmlSerializer(value.GetType());
			xmlSerializer.Serialize(stringWriter, value);
			IL_0047:
			return stringWriter.ToString();
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x00236690 File Offset: 0x00235A90
		public override void ConvertObjectToXml(object value, XmlWriter xmlWriter, XmlRootAttribute xmlAttrib)
		{
			if (xmlAttrib == null)
			{
				((IXmlSerializable)value).WriteXml(xmlWriter);
				return;
			}
			XmlSerializer xmlSerializer = ObjectStorage.GetXmlSerializer(this.DataType, xmlAttrib);
			xmlSerializer.Serialize(xmlWriter, value);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x002366C4 File Offset: 0x00235AC4
		protected override object GetEmptyStorage(int recordCount)
		{
			return new object[recordCount];
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x002366D8 File Offset: 0x00235AD8
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			object[] array = (object[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x00236708 File Offset: 0x00235B08
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (object[])store;
		}

		// Token: 0x04000D09 RID: 3337
		private object[] values;

		// Token: 0x04000D0A RID: 3338
		private readonly bool implementsIXmlSerializable;

		// Token: 0x04000D0B RID: 3339
		private readonly bool implementsIComparable;

		// Token: 0x04000D0C RID: 3340
		private static readonly Dictionary<Type, object> TypeToNull = new Dictionary<Type, object>();
	}
}
