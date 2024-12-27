using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200007E RID: 126
	public class PropertyDataCollection : ICollection, IEnumerable
	{
		// Token: 0x0600037B RID: 891 RVA: 0x0000F375 File Offset: 0x0000E375
		internal PropertyDataCollection(ManagementBaseObject parent, bool isSystem)
		{
			this.parent = parent;
			this.isSystem = isSystem;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000F38C File Offset: 0x0000E38C
		public int Count
		{
			get
			{
				string[] array = null;
				object obj = null;
				int num;
				if (this.isSystem)
				{
					num = 48;
				}
				else
				{
					num = 64;
				}
				num = num;
				int names_ = this.parent.wbemObject.GetNames_(null, num, ref obj, out array);
				if (names_ < 0)
				{
					if (((long)names_ & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)names_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(names_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				return array.Length;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600037D RID: 893 RVA: 0x0000F3F6 File Offset: 0x0000E3F6
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0000F3F9 File Offset: 0x0000E3F9
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000F3FC File Offset: 0x0000E3FC
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < array.GetLowerBound(0) || index > array.GetUpperBound(0))
			{
				throw new ArgumentOutOfRangeException("index");
			}
			string[] array2 = null;
			object obj = null;
			int num = 0;
			if (this.isSystem)
			{
				num |= 48;
			}
			else
			{
				num |= 64;
			}
			num = num;
			int names_ = this.parent.wbemObject.GetNames_(null, num, ref obj, out array2);
			if (names_ >= 0)
			{
				if (index + array2.Length > array.Length)
				{
					throw new ArgumentException(null, "index");
				}
				foreach (string text in array2)
				{
					array.SetValue(new PropertyData(this.parent, text), index++);
				}
			}
			if (names_ < 0)
			{
				if (((long)names_ & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)names_);
					return;
				}
				Marshal.ThrowExceptionForHR(names_, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000F4E8 File Offset: 0x0000E4E8
		public void CopyTo(PropertyData[] propertyArray, int index)
		{
			this.CopyTo(propertyArray, index);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000F4F2 File Offset: 0x0000E4F2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new PropertyDataCollection.PropertyDataEnumerator(this.parent, this.isSystem);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000F505 File Offset: 0x0000E505
		public PropertyDataCollection.PropertyDataEnumerator GetEnumerator()
		{
			return new PropertyDataCollection.PropertyDataEnumerator(this.parent, this.isSystem);
		}

		// Token: 0x17000087 RID: 135
		public virtual PropertyData this[string propertyName]
		{
			get
			{
				if (propertyName == null)
				{
					throw new ArgumentNullException("propertyName");
				}
				return new PropertyData(this.parent, propertyName);
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000F534 File Offset: 0x0000E534
		public virtual void Remove(string propertyName)
		{
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				ManagementClass managementClass = new ManagementClass(this.parent.ClassPath);
				this.parent.SetPropertyValue(propertyName, managementClass.GetPropertyValue(propertyName));
				return;
			}
			int num = this.parent.wbemObject.Delete_(propertyName);
			if (num < 0)
			{
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000F5BC File Offset: 0x0000E5BC
		public virtual void Add(string propertyName, object propertyValue)
		{
			if (propertyValue == null)
			{
				throw new ArgumentNullException("propertyValue");
			}
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				throw new InvalidOperationException();
			}
			CimType cimType = CimType.None;
			bool flag = false;
			object obj = PropertyData.MapValueToWmiValue(propertyValue, out flag, out cimType);
			int num = (int)cimType;
			if (flag)
			{
				num |= 8192;
			}
			int num2 = this.parent.wbemObject.Put_(propertyName, 0, ref obj, num);
			if (num2 < 0)
			{
				if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000F658 File Offset: 0x0000E658
		public void Add(string propertyName, object propertyValue, CimType propertyType)
		{
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				throw new InvalidOperationException();
			}
			int num = (int)propertyType;
			bool flag = false;
			if (propertyValue != null && propertyValue.GetType().IsArray)
			{
				flag = true;
				num |= 8192;
			}
			object obj = PropertyData.MapValueToWmiValue(propertyValue, propertyType, flag);
			int num2 = this.parent.wbemObject.Put_(propertyName, 0, ref obj, num);
			if (num2 < 0)
			{
				if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000F6FC File Offset: 0x0000E6FC
		public void Add(string propertyName, CimType propertyType, bool isArray)
		{
			if (propertyName == null)
			{
				throw new ArgumentNullException(propertyName);
			}
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				throw new InvalidOperationException();
			}
			int num = (int)propertyType;
			if (isArray)
			{
				num |= 8192;
			}
			object value = DBNull.Value;
			int num2 = this.parent.wbemObject.Put_(propertyName, 0, ref value, num);
			if (num2 < 0)
			{
				if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x040001D5 RID: 469
		private ManagementBaseObject parent;

		// Token: 0x040001D6 RID: 470
		private bool isSystem;

		// Token: 0x0200007F RID: 127
		public class PropertyDataEnumerator : IEnumerator
		{
			// Token: 0x06000388 RID: 904 RVA: 0x0000F788 File Offset: 0x0000E788
			internal PropertyDataEnumerator(ManagementBaseObject parent, bool isSystem)
			{
				this.parent = parent;
				this.propertyNames = null;
				this.index = -1;
				object obj = null;
				int num;
				if (isSystem)
				{
					num = 48;
				}
				else
				{
					num = 64;
				}
				num = num;
				int names_ = parent.wbemObject.GetNames_(null, num, ref obj, out this.propertyNames);
				if (names_ < 0)
				{
					if (((long)names_ & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)names_);
						return;
					}
					Marshal.ThrowExceptionForHR(names_, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}

			// Token: 0x17000088 RID: 136
			// (get) Token: 0x06000389 RID: 905 RVA: 0x0000F801 File Offset: 0x0000E801
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x17000089 RID: 137
			// (get) Token: 0x0600038A RID: 906 RVA: 0x0000F809 File Offset: 0x0000E809
			public PropertyData Current
			{
				get
				{
					if (this.index == -1 || this.index == this.propertyNames.Length)
					{
						throw new InvalidOperationException();
					}
					return new PropertyData(this.parent, this.propertyNames[this.index]);
				}
			}

			// Token: 0x0600038B RID: 907 RVA: 0x0000F842 File Offset: 0x0000E842
			public bool MoveNext()
			{
				if (this.index == this.propertyNames.Length)
				{
					return false;
				}
				this.index++;
				return this.index != this.propertyNames.Length;
			}

			// Token: 0x0600038C RID: 908 RVA: 0x0000F877 File Offset: 0x0000E877
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x040001D7 RID: 471
			private ManagementBaseObject parent;

			// Token: 0x040001D8 RID: 472
			private string[] propertyNames;

			// Token: 0x040001D9 RID: 473
			private int index;
		}
	}
}
