using System;
using System.Collections;
using System.DirectoryServices.Interop;

namespace System.DirectoryServices
{
	// Token: 0x02000039 RID: 57
	public class SchemaNameCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x0600018F RID: 399 RVA: 0x00006EB9 File Offset: 0x00005EB9
		internal SchemaNameCollection(SchemaNameCollection.VariantPropGetter propGetter, SchemaNameCollection.VariantPropSetter propSetter)
		{
			this.propGetter = propGetter;
			this.propSetter = propSetter;
		}

		// Token: 0x1700006A RID: 106
		public string this[int index]
		{
			get
			{
				object[] value = this.GetValue();
				return (string)value[index];
			}
			set
			{
				object[] value2 = this.GetValue();
				value2[index] = value;
				this.propSetter(value2);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00006F10 File Offset: 0x00005F10
		public int Count
		{
			get
			{
				object[] value = this.GetValue();
				return value.Length;
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006F28 File Offset: 0x00005F28
		public int Add(string value)
		{
			object[] value2 = this.GetValue();
			object[] array = new object[value2.Length + 1];
			for (int i = 0; i < value2.Length; i++)
			{
				array[i] = value2[i];
			}
			array[array.Length - 1] = value;
			this.propSetter(array);
			return array.Length - 1;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006F74 File Offset: 0x00005F74
		public void AddRange(string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			object[] value2 = this.GetValue();
			object[] array = new object[value2.Length + value.Length];
			for (int i = 0; i < value2.Length; i++)
			{
				array[i] = value2[i];
			}
			for (int j = value2.Length; j < array.Length; j++)
			{
				array[j] = value[j - value2.Length];
			}
			this.propSetter(array);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006FE0 File Offset: 0x00005FE0
		public void AddRange(SchemaNameCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			object[] value2 = this.GetValue();
			object[] array = new object[value2.Length + value.Count];
			for (int i = 0; i < value2.Length; i++)
			{
				array[i] = value2[i];
			}
			for (int j = value2.Length; j < array.Length; j++)
			{
				array[j] = value[j - value2.Length];
			}
			this.propSetter(array);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00007050 File Offset: 0x00006050
		public void Clear()
		{
			object[] array = new object[0];
			this.propSetter(array);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007070 File Offset: 0x00006070
		public bool Contains(string value)
		{
			return this.IndexOf(value) != -1;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007080 File Offset: 0x00006080
		public void CopyTo(string[] stringArray, int index)
		{
			object[] value = this.GetValue();
			value.CopyTo(stringArray, index);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000709C File Offset: 0x0000609C
		public IEnumerator GetEnumerator()
		{
			object[] value = this.GetValue();
			return value.GetEnumerator();
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000070B8 File Offset: 0x000060B8
		private object[] GetValue()
		{
			object obj = this.propGetter();
			if (obj == null)
			{
				return new object[0];
			}
			return (object[])obj;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000070E4 File Offset: 0x000060E4
		public int IndexOf(string value)
		{
			object[] value2 = this.GetValue();
			for (int i = 0; i < value2.Length; i++)
			{
				if (value == (string)value2[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000711C File Offset: 0x0000611C
		public void Insert(int index, string value)
		{
			ArrayList arrayList = new ArrayList(this.GetValue());
			arrayList.Insert(index, value);
			this.propSetter(arrayList.ToArray());
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007150 File Offset: 0x00006150
		public void Remove(string value)
		{
			int num = this.IndexOf(value);
			this.RemoveAt(num);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000716C File Offset: 0x0000616C
		public void RemoveAt(int index)
		{
			object[] value = this.GetValue();
			if (index >= value.Length || index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			object[] array = new object[value.Length - 1];
			for (int i = 0; i < index; i++)
			{
				array[i] = value[i];
			}
			for (int j = index + 1; j < value.Length; j++)
			{
				array[j - 1] = value[j];
			}
			this.propSetter(array);
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000071D6 File Offset: 0x000061D6
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x000071D9 File Offset: 0x000061D9
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000071DC File Offset: 0x000061DC
		void ICollection.CopyTo(Array array, int index)
		{
			object[] value = this.GetValue();
			value.CopyTo(array, index);
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x000071F8 File Offset: 0x000061F8
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x000071FB File Offset: 0x000061FB
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000070 RID: 112
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (string)value;
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007216 File Offset: 0x00006216
		int IList.Add(object value)
		{
			return this.Add((string)value);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007224 File Offset: 0x00006224
		bool IList.Contains(object value)
		{
			return this.Contains((string)value);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007232 File Offset: 0x00006232
		int IList.IndexOf(object value)
		{
			return this.IndexOf((string)value);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007240 File Offset: 0x00006240
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (string)value);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000724F File Offset: 0x0000624F
		void IList.Remove(object value)
		{
			this.Remove((string)value);
		}

		// Token: 0x040001DA RID: 474
		private SchemaNameCollection.VariantPropGetter propGetter;

		// Token: 0x040001DB RID: 475
		private SchemaNameCollection.VariantPropSetter propSetter;

		// Token: 0x0200003A RID: 58
		// (Invoke) Token: 0x060001AC RID: 428
		internal delegate object VariantPropGetter();

		// Token: 0x0200003B RID: 59
		// (Invoke) Token: 0x060001B0 RID: 432
		internal delegate void VariantPropSetter(object value);

		// Token: 0x0200003C RID: 60
		internal class FilterDelegateWrapper
		{
			// Token: 0x060001B3 RID: 435 RVA: 0x0000725D File Offset: 0x0000625D
			internal FilterDelegateWrapper(UnsafeNativeMethods.IAdsContainer wrapped)
			{
				this.obj = wrapped;
			}

			// Token: 0x17000071 RID: 113
			// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000726C File Offset: 0x0000626C
			public SchemaNameCollection.VariantPropGetter Getter
			{
				get
				{
					return new SchemaNameCollection.VariantPropGetter(this.GetFilter);
				}
			}

			// Token: 0x17000072 RID: 114
			// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000727A File Offset: 0x0000627A
			public SchemaNameCollection.VariantPropSetter Setter
			{
				get
				{
					return new SchemaNameCollection.VariantPropSetter(this.SetFilter);
				}
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x00007288 File Offset: 0x00006288
			private object GetFilter()
			{
				return this.obj.Filter;
			}

			// Token: 0x060001B7 RID: 439 RVA: 0x00007295 File Offset: 0x00006295
			private void SetFilter(object value)
			{
				this.obj.Filter = value;
			}

			// Token: 0x040001DC RID: 476
			private UnsafeNativeMethods.IAdsContainer obj;
		}
	}
}
