using System;
using System.Collections;
using System.ComponentModel;
using System.Data.ProviderBase;

namespace System.Data.Common
{
	// Token: 0x02000138 RID: 312
	public class DbEnumerator : IEnumerator
	{
		// Token: 0x06001490 RID: 5264 RVA: 0x00227FE8 File Offset: 0x002273E8
		public DbEnumerator(IDataReader reader)
		{
			if (reader == null)
			{
				throw ADP.ArgumentNull("reader");
			}
			this._reader = reader;
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00228010 File Offset: 0x00227410
		public DbEnumerator(IDataReader reader, bool closeReader)
		{
			if (reader == null)
			{
				throw ADP.ArgumentNull("reader");
			}
			this._reader = reader;
			this.closeReader = closeReader;
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x00228040 File Offset: 0x00227440
		public object Current
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00228054 File Offset: 0x00227454
		public bool MoveNext()
		{
			if (this._schemaInfo == null)
			{
				this.BuildSchemaInfo();
			}
			this._current = null;
			if (this._reader.Read())
			{
				object[] array = new object[this._schemaInfo.Length];
				this._reader.GetValues(array);
				this._current = new DataRecordInternal(this._schemaInfo, array, this._descriptors, this._fieldNameLookup);
				return true;
			}
			if (this.closeReader)
			{
				this._reader.Close();
			}
			return false;
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x002280D4 File Offset: 0x002274D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Reset()
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x002280E8 File Offset: 0x002274E8
		private void BuildSchemaInfo()
		{
			int fieldCount = this._reader.FieldCount;
			string[] array = new string[fieldCount];
			for (int i = 0; i < fieldCount; i++)
			{
				array[i] = this._reader.GetName(i);
			}
			ADP.BuildSchemaTableInfoTableNames(array);
			SchemaInfo[] array2 = new SchemaInfo[fieldCount];
			PropertyDescriptor[] array3 = new PropertyDescriptor[this._reader.FieldCount];
			for (int j = 0; j < array2.Length; j++)
			{
				SchemaInfo schemaInfo = default(SchemaInfo);
				schemaInfo.name = this._reader.GetName(j);
				schemaInfo.type = this._reader.GetFieldType(j);
				schemaInfo.typeName = this._reader.GetDataTypeName(j);
				array3[j] = new DbEnumerator.DbColumnDescriptor(j, array[j], schemaInfo.type);
				array2[j] = schemaInfo;
			}
			this._schemaInfo = array2;
			this._fieldNameLookup = new FieldNameLookup(this._reader, -1);
			this._descriptors = new PropertyDescriptorCollection(array3);
		}

		// Token: 0x04000C47 RID: 3143
		internal IDataReader _reader;

		// Token: 0x04000C48 RID: 3144
		internal IDataRecord _current;

		// Token: 0x04000C49 RID: 3145
		internal SchemaInfo[] _schemaInfo;

		// Token: 0x04000C4A RID: 3146
		internal PropertyDescriptorCollection _descriptors;

		// Token: 0x04000C4B RID: 3147
		private FieldNameLookup _fieldNameLookup;

		// Token: 0x04000C4C RID: 3148
		private bool closeReader;

		// Token: 0x02000139 RID: 313
		private sealed class DbColumnDescriptor : PropertyDescriptor
		{
			// Token: 0x06001496 RID: 5270 RVA: 0x002281E0 File Offset: 0x002275E0
			internal DbColumnDescriptor(int ordinal, string name, Type type)
				: base(name, null)
			{
				this._ordinal = ordinal;
				this._type = type;
			}

			// Token: 0x170002D4 RID: 724
			// (get) Token: 0x06001497 RID: 5271 RVA: 0x00228204 File Offset: 0x00227604
			public override Type ComponentType
			{
				get
				{
					return typeof(IDataRecord);
				}
			}

			// Token: 0x170002D5 RID: 725
			// (get) Token: 0x06001498 RID: 5272 RVA: 0x0022821C File Offset: 0x0022761C
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170002D6 RID: 726
			// (get) Token: 0x06001499 RID: 5273 RVA: 0x0022822C File Offset: 0x0022762C
			public override Type PropertyType
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x0600149A RID: 5274 RVA: 0x00228240 File Offset: 0x00227640
			public override bool CanResetValue(object component)
			{
				return false;
			}

			// Token: 0x0600149B RID: 5275 RVA: 0x00228250 File Offset: 0x00227650
			public override object GetValue(object component)
			{
				return ((IDataRecord)component)[this._ordinal];
			}

			// Token: 0x0600149C RID: 5276 RVA: 0x00228270 File Offset: 0x00227670
			public override void ResetValue(object component)
			{
				throw ADP.NotSupported();
			}

			// Token: 0x0600149D RID: 5277 RVA: 0x00228284 File Offset: 0x00227684
			public override void SetValue(object component, object value)
			{
				throw ADP.NotSupported();
			}

			// Token: 0x0600149E RID: 5278 RVA: 0x00228298 File Offset: 0x00227698
			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}

			// Token: 0x04000C4D RID: 3149
			private int _ordinal;

			// Token: 0x04000C4E RID: 3150
			private Type _type;
		}
	}
}
