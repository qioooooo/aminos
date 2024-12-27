using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000081 RID: 129
	public class QualifierDataCollection : ICollection, IEnumerable
	{
		// Token: 0x0600039C RID: 924 RVA: 0x0000FE6C File Offset: 0x0000EE6C
		internal QualifierDataCollection(ManagementBaseObject parent)
		{
			this.parent = parent;
			this.qualifierSetType = QualifierType.ObjectQualifier;
			this.propertyOrMethodName = null;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000FE89 File Offset: 0x0000EE89
		internal QualifierDataCollection(ManagementBaseObject parent, string propertyOrMethodName, QualifierType type)
		{
			this.parent = parent;
			this.propertyOrMethodName = propertyOrMethodName;
			this.qualifierSetType = type;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000FEA6 File Offset: 0x0000EEA6
		private IWbemQualifierSetFreeThreaded GetTypeQualifierSet()
		{
			return this.GetTypeQualifierSet(this.qualifierSetType);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000FEB4 File Offset: 0x0000EEB4
		private IWbemQualifierSetFreeThreaded GetTypeQualifierSet(QualifierType qualifierSetType)
		{
			IWbemQualifierSetFreeThreaded wbemQualifierSetFreeThreaded = null;
			int num;
			switch (qualifierSetType)
			{
			case QualifierType.ObjectQualifier:
				num = this.parent.wbemObject.GetQualifierSet_(out wbemQualifierSetFreeThreaded);
				break;
			case QualifierType.PropertyQualifier:
				num = this.parent.wbemObject.GetPropertyQualifierSet_(this.propertyOrMethodName, out wbemQualifierSetFreeThreaded);
				break;
			case QualifierType.MethodQualifier:
				num = this.parent.wbemObject.GetMethodQualifierSet_(this.propertyOrMethodName, out wbemQualifierSetFreeThreaded);
				break;
			default:
				throw new ManagementException(ManagementStatus.Unexpected, null, null);
			}
			if (num < 0)
			{
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
			return wbemQualifierSetFreeThreaded;
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000FF64 File Offset: 0x0000EF64
		public int Count
		{
			get
			{
				string[] array = null;
				IWbemQualifierSetFreeThreaded typeQualifierSet;
				try
				{
					typeQualifierSet = this.GetTypeQualifierSet();
				}
				catch (ManagementException ex)
				{
					if (this.qualifierSetType == QualifierType.PropertyQualifier && ex.ErrorCode == ManagementStatus.SystemProperty)
					{
						return 0;
					}
					throw;
				}
				int names_ = typeQualifierSet.GetNames_(0, out array);
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

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x0000FFE8 File Offset: 0x0000EFE8
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000FFEB File Offset: 0x0000EFEB
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000FFF0 File Offset: 0x0000EFF0
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
			IWbemQualifierSetFreeThreaded typeQualifierSet;
			try
			{
				typeQualifierSet = this.GetTypeQualifierSet();
			}
			catch (ManagementException ex)
			{
				if (this.qualifierSetType == QualifierType.PropertyQualifier && ex.ErrorCode == ManagementStatus.SystemProperty)
				{
					return;
				}
				throw;
			}
			int names_ = typeQualifierSet.GetNames_(0, out array2);
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
			if (index + array2.Length > array.Length)
			{
				throw new ArgumentException(null, "index");
			}
			foreach (string text in array2)
			{
				array.SetValue(new QualifierData(this.parent, this.propertyOrMethodName, text, this.qualifierSetType), index++);
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000100F8 File Offset: 0x0000F0F8
		public void CopyTo(QualifierData[] qualifierArray, int index)
		{
			this.CopyTo(qualifierArray, index);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00010102 File Offset: 0x0000F102
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new QualifierDataCollection.QualifierDataEnumerator(this.parent, this.propertyOrMethodName, this.qualifierSetType);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001011B File Offset: 0x0000F11B
		public QualifierDataCollection.QualifierDataEnumerator GetEnumerator()
		{
			return new QualifierDataCollection.QualifierDataEnumerator(this.parent, this.propertyOrMethodName, this.qualifierSetType);
		}

		// Token: 0x17000094 RID: 148
		public virtual QualifierData this[string qualifierName]
		{
			get
			{
				if (qualifierName == null)
				{
					throw new ArgumentNullException("qualifierName");
				}
				return new QualifierData(this.parent, this.propertyOrMethodName, qualifierName, this.qualifierSetType);
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001015C File Offset: 0x0000F15C
		public virtual void Remove(string qualifierName)
		{
			int num = this.GetTypeQualifierSet().Delete_(qualifierName);
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

		// Token: 0x060003A9 RID: 937 RVA: 0x000101A2 File Offset: 0x0000F1A2
		public virtual void Add(string qualifierName, object qualifierValue)
		{
			this.Add(qualifierName, qualifierValue, false, false, false, true);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000101B0 File Offset: 0x0000F1B0
		public virtual void Add(string qualifierName, object qualifierValue, bool isAmended, bool propagatesToInstance, bool propagatesToSubclass, bool isOverridable)
		{
			int num = 0;
			if (isAmended)
			{
				num |= 128;
			}
			if (propagatesToInstance)
			{
				num |= 1;
			}
			if (propagatesToSubclass)
			{
				num |= 2;
			}
			if (!isOverridable)
			{
				num |= 16;
			}
			int num2 = this.GetTypeQualifierSet().Put_(qualifierName, ref qualifierValue, num);
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

		// Token: 0x040001E1 RID: 481
		private ManagementBaseObject parent;

		// Token: 0x040001E2 RID: 482
		private string propertyOrMethodName;

		// Token: 0x040001E3 RID: 483
		private QualifierType qualifierSetType;

		// Token: 0x02000082 RID: 130
		public class QualifierDataEnumerator : IEnumerator
		{
			// Token: 0x060003AB RID: 939 RVA: 0x00010220 File Offset: 0x0000F220
			internal QualifierDataEnumerator(ManagementBaseObject parent, string propertyOrMethodName, QualifierType qualifierType)
			{
				this.parent = parent;
				this.propertyOrMethodName = propertyOrMethodName;
				this.qualifierType = qualifierType;
				this.qualifierNames = null;
				IWbemQualifierSetFreeThreaded wbemQualifierSetFreeThreaded = null;
				int num;
				switch (qualifierType)
				{
				case QualifierType.ObjectQualifier:
					num = parent.wbemObject.GetQualifierSet_(out wbemQualifierSetFreeThreaded);
					break;
				case QualifierType.PropertyQualifier:
					num = parent.wbemObject.GetPropertyQualifierSet_(propertyOrMethodName, out wbemQualifierSetFreeThreaded);
					break;
				case QualifierType.MethodQualifier:
					num = parent.wbemObject.GetMethodQualifierSet_(propertyOrMethodName, out wbemQualifierSetFreeThreaded);
					break;
				default:
					throw new ManagementException(ManagementStatus.Unexpected, null, null);
				}
				if (num < 0)
				{
					this.qualifierNames = new string[0];
					return;
				}
				num = wbemQualifierSetFreeThreaded.GetNames_(0, out this.qualifierNames);
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

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x060003AC RID: 940 RVA: 0x000102FA File Offset: 0x0000F2FA
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x060003AD RID: 941 RVA: 0x00010304 File Offset: 0x0000F304
			public QualifierData Current
			{
				get
				{
					if (this.index == -1 || this.index == this.qualifierNames.Length)
					{
						throw new InvalidOperationException();
					}
					return new QualifierData(this.parent, this.propertyOrMethodName, this.qualifierNames[this.index], this.qualifierType);
				}
			}

			// Token: 0x060003AE RID: 942 RVA: 0x00010354 File Offset: 0x0000F354
			public bool MoveNext()
			{
				if (this.index == this.qualifierNames.Length)
				{
					return false;
				}
				this.index++;
				return this.index != this.qualifierNames.Length;
			}

			// Token: 0x060003AF RID: 943 RVA: 0x00010389 File Offset: 0x0000F389
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x040001E4 RID: 484
			private ManagementBaseObject parent;

			// Token: 0x040001E5 RID: 485
			private string propertyOrMethodName;

			// Token: 0x040001E6 RID: 486
			private QualifierType qualifierType;

			// Token: 0x040001E7 RID: 487
			private string[] qualifierNames;

			// Token: 0x040001E8 RID: 488
			private int index = -1;
		}
	}
}
