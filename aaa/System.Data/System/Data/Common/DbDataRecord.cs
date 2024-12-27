using System;
using System.ComponentModel;

namespace System.Data.Common
{
	// Token: 0x0200011C RID: 284
	public abstract class DbDataRecord : ICustomTypeDescriptor, IDataRecord
	{
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06001232 RID: 4658
		public abstract int FieldCount { get; }

		// Token: 0x1700025E RID: 606
		public abstract object this[int i] { get; }

		// Token: 0x1700025F RID: 607
		public abstract object this[string name] { get; }

		// Token: 0x06001235 RID: 4661
		public abstract bool GetBoolean(int i);

		// Token: 0x06001236 RID: 4662
		public abstract byte GetByte(int i);

		// Token: 0x06001237 RID: 4663
		public abstract long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length);

		// Token: 0x06001238 RID: 4664
		public abstract char GetChar(int i);

		// Token: 0x06001239 RID: 4665
		public abstract long GetChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length);

		// Token: 0x0600123A RID: 4666 RVA: 0x0021E7DC File Offset: 0x0021DBDC
		public IDataReader GetData(int i)
		{
			return this.GetDbDataReader(i);
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0021E7F0 File Offset: 0x0021DBF0
		protected virtual DbDataReader GetDbDataReader(int i)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x0600123C RID: 4668
		public abstract string GetDataTypeName(int i);

		// Token: 0x0600123D RID: 4669
		public abstract DateTime GetDateTime(int i);

		// Token: 0x0600123E RID: 4670
		public abstract decimal GetDecimal(int i);

		// Token: 0x0600123F RID: 4671
		public abstract double GetDouble(int i);

		// Token: 0x06001240 RID: 4672
		public abstract Type GetFieldType(int i);

		// Token: 0x06001241 RID: 4673
		public abstract float GetFloat(int i);

		// Token: 0x06001242 RID: 4674
		public abstract Guid GetGuid(int i);

		// Token: 0x06001243 RID: 4675
		public abstract short GetInt16(int i);

		// Token: 0x06001244 RID: 4676
		public abstract int GetInt32(int i);

		// Token: 0x06001245 RID: 4677
		public abstract long GetInt64(int i);

		// Token: 0x06001246 RID: 4678
		public abstract string GetName(int i);

		// Token: 0x06001247 RID: 4679
		public abstract int GetOrdinal(string name);

		// Token: 0x06001248 RID: 4680
		public abstract string GetString(int i);

		// Token: 0x06001249 RID: 4681
		public abstract object GetValue(int i);

		// Token: 0x0600124A RID: 4682
		public abstract int GetValues(object[] values);

		// Token: 0x0600124B RID: 4683
		public abstract bool IsDBNull(int i);

		// Token: 0x0600124C RID: 4684 RVA: 0x0021E804 File Offset: 0x0021DC04
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return new AttributeCollection(null);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0021E818 File Offset: 0x0021DC18
		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0021E828 File Offset: 0x0021DC28
		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0021E838 File Offset: 0x0021DC38
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0021E848 File Offset: 0x0021DC48
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0021E858 File Offset: 0x0021DC58
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0021E868 File Offset: 0x0021DC68
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0021E878 File Offset: 0x0021DC78
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0021E88C File Offset: 0x0021DC8C
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0021E8A0 File Offset: 0x0021DCA0
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0021E8B4 File Offset: 0x0021DCB4
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			return new PropertyDescriptorCollection(null);
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0021E8C8 File Offset: 0x0021DCC8
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}
	}
}
