using System;
using System.ComponentModel;
using System.Data.ProviderBase;

namespace System.Data.Common
{
	// Token: 0x0200011D RID: 285
	internal sealed class DataRecordInternal : DbDataRecord, ICustomTypeDescriptor
	{
		// Token: 0x06001258 RID: 4696 RVA: 0x0021E8D8 File Offset: 0x0021DCD8
		internal DataRecordInternal(SchemaInfo[] schemaInfo, object[] values, PropertyDescriptorCollection descriptors, FieldNameLookup fieldNameLookup)
		{
			this._schemaInfo = schemaInfo;
			this._values = values;
			this._propertyDescriptors = descriptors;
			this._fieldNameLookup = fieldNameLookup;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0021E908 File Offset: 0x0021DD08
		internal DataRecordInternal(object[] values, PropertyDescriptorCollection descriptors, FieldNameLookup fieldNameLookup)
		{
			this._values = values;
			this._propertyDescriptors = descriptors;
			this._fieldNameLookup = fieldNameLookup;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0021E930 File Offset: 0x0021DD30
		internal void SetSchemaInfo(SchemaInfo[] schemaInfo)
		{
			this._schemaInfo = schemaInfo;
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600125B RID: 4699 RVA: 0x0021E944 File Offset: 0x0021DD44
		public override int FieldCount
		{
			get
			{
				return this._schemaInfo.Length;
			}
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0021E95C File Offset: 0x0021DD5C
		public override int GetValues(object[] values)
		{
			if (values == null)
			{
				throw ADP.ArgumentNull("values");
			}
			int num = ((values.Length < this._schemaInfo.Length) ? values.Length : this._schemaInfo.Length);
			for (int i = 0; i < num; i++)
			{
				values[i] = this._values[i];
			}
			return num;
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x0021E9AC File Offset: 0x0021DDAC
		public override string GetName(int i)
		{
			return this._schemaInfo[i].name;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0021E9CC File Offset: 0x0021DDCC
		public override object GetValue(int i)
		{
			return this._values[i];
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0021E9E4 File Offset: 0x0021DDE4
		public override string GetDataTypeName(int i)
		{
			return this._schemaInfo[i].typeName;
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0021EA04 File Offset: 0x0021DE04
		public override Type GetFieldType(int i)
		{
			return this._schemaInfo[i].type;
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x0021EA24 File Offset: 0x0021DE24
		public override int GetOrdinal(string name)
		{
			return this._fieldNameLookup.GetOrdinal(name);
		}

		// Token: 0x17000261 RID: 609
		public override object this[int i]
		{
			get
			{
				return this.GetValue(i);
			}
		}

		// Token: 0x17000262 RID: 610
		public override object this[string name]
		{
			get
			{
				return this.GetValue(this.GetOrdinal(name));
			}
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0021EA70 File Offset: 0x0021DE70
		public override bool GetBoolean(int i)
		{
			return (bool)this._values[i];
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0021EA8C File Offset: 0x0021DE8C
		public override byte GetByte(int i)
		{
			return (byte)this._values[i];
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0021EAA8 File Offset: 0x0021DEA8
		public override long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length)
		{
			int num = 0;
			byte[] array = (byte[])this._values[i];
			num = array.Length;
			if (dataIndex > 2147483647L)
			{
				throw ADP.InvalidSourceBufferIndex(num, dataIndex, "dataIndex");
			}
			int num2 = (int)dataIndex;
			if (buffer == null)
			{
				return (long)num;
			}
			try
			{
				if (num2 < num)
				{
					if (num2 + length > num)
					{
						num -= num2;
					}
					else
					{
						num = length;
					}
				}
				Array.Copy(array, num2, buffer, bufferIndex, num);
			}
			catch (Exception ex)
			{
				if (ADP.IsCatchableExceptionType(ex))
				{
					num = array.Length;
					if (length < 0)
					{
						throw ADP.InvalidDataLength((long)length);
					}
					if (bufferIndex < 0 || bufferIndex >= buffer.Length)
					{
						throw ADP.InvalidDestinationBufferIndex(length, bufferIndex, "bufferIndex");
					}
					if (dataIndex < 0L || dataIndex >= (long)num)
					{
						throw ADP.InvalidSourceBufferIndex(length, dataIndex, "dataIndex");
					}
					if (num + bufferIndex > buffer.Length)
					{
						throw ADP.InvalidBufferSizeOrIndex(num, bufferIndex);
					}
				}
				throw;
			}
			return (long)num;
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x0021EB8C File Offset: 0x0021DF8C
		public override char GetChar(int i)
		{
			string text = (string)this._values[i];
			char[] array = text.ToCharArray();
			return array[0];
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0021EBB4 File Offset: 0x0021DFB4
		public override long GetChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length)
		{
			int num = 0;
			string text = (string)this._values[i];
			char[] array = text.ToCharArray();
			num = array.Length;
			if (dataIndex > 2147483647L)
			{
				throw ADP.InvalidSourceBufferIndex(num, dataIndex, "dataIndex");
			}
			int num2 = (int)dataIndex;
			if (buffer == null)
			{
				return (long)num;
			}
			try
			{
				if (num2 < num)
				{
					if (num2 + length > num)
					{
						num -= num2;
					}
					else
					{
						num = length;
					}
				}
				Array.Copy(array, num2, buffer, bufferIndex, num);
			}
			catch (Exception ex)
			{
				if (ADP.IsCatchableExceptionType(ex))
				{
					num = array.Length;
					if (length < 0)
					{
						throw ADP.InvalidDataLength((long)length);
					}
					if (bufferIndex < 0 || bufferIndex >= buffer.Length)
					{
						throw ADP.InvalidDestinationBufferIndex(buffer.Length, bufferIndex, "bufferIndex");
					}
					if (num2 < 0 || num2 >= num)
					{
						throw ADP.InvalidSourceBufferIndex(num, dataIndex, "dataIndex");
					}
					if (num + bufferIndex > buffer.Length)
					{
						throw ADP.InvalidBufferSizeOrIndex(num, bufferIndex);
					}
				}
				throw;
			}
			return (long)num;
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0021ECA0 File Offset: 0x0021E0A0
		public override Guid GetGuid(int i)
		{
			return (Guid)this._values[i];
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0021ECBC File Offset: 0x0021E0BC
		public override short GetInt16(int i)
		{
			return (short)this._values[i];
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x0021ECD8 File Offset: 0x0021E0D8
		public override int GetInt32(int i)
		{
			return (int)this._values[i];
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0021ECF4 File Offset: 0x0021E0F4
		public override long GetInt64(int i)
		{
			return (long)this._values[i];
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0021ED10 File Offset: 0x0021E110
		public override float GetFloat(int i)
		{
			return (float)this._values[i];
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0021ED2C File Offset: 0x0021E12C
		public override double GetDouble(int i)
		{
			return (double)this._values[i];
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0021ED48 File Offset: 0x0021E148
		public override string GetString(int i)
		{
			return (string)this._values[i];
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0021ED64 File Offset: 0x0021E164
		public override decimal GetDecimal(int i)
		{
			return (decimal)this._values[i];
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0021ED80 File Offset: 0x0021E180
		public override DateTime GetDateTime(int i)
		{
			return (DateTime)this._values[i];
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0021ED9C File Offset: 0x0021E19C
		public override bool IsDBNull(int i)
		{
			object obj = this._values[i];
			return obj == null || Convert.IsDBNull(obj);
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0021EDC0 File Offset: 0x0021E1C0
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return new AttributeCollection(null);
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0021EDD4 File Offset: 0x0021E1D4
		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x0021EDE4 File Offset: 0x0021E1E4
		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0021EDF4 File Offset: 0x0021E1F4
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0021EE04 File Offset: 0x0021E204
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x0021EE14 File Offset: 0x0021E214
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x0021EE24 File Offset: 0x0021E224
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0021EE34 File Offset: 0x0021E234
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x0021EE48 File Offset: 0x0021E248
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0021EE5C File Offset: 0x0021E25C
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0021EE70 File Offset: 0x0021E270
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			if (this._propertyDescriptors == null)
			{
				this._propertyDescriptors = new PropertyDescriptorCollection(null);
			}
			return this._propertyDescriptors;
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0021EE98 File Offset: 0x0021E298
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		// Token: 0x04000B7D RID: 2941
		private SchemaInfo[] _schemaInfo;

		// Token: 0x04000B7E RID: 2942
		private object[] _values;

		// Token: 0x04000B7F RID: 2943
		private PropertyDescriptorCollection _propertyDescriptors;

		// Token: 0x04000B80 RID: 2944
		private FieldNameLookup _fieldNameLookup;
	}
}
