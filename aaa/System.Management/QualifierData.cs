using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000080 RID: 128
	public class QualifierData
	{
		// Token: 0x0600038D RID: 909 RVA: 0x0000F880 File Offset: 0x0000E880
		internal QualifierData(ManagementBaseObject parent, string propName, string qualName, QualifierType type)
		{
			this.parent = parent;
			this.propertyOrMethodName = propName;
			this.qualifierName = qualName;
			this.qualifierType = type;
			this.RefreshQualifierInfo();
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000F8AC File Offset: 0x0000E8AC
		private void RefreshQualifierInfo()
		{
			this.qualifierSet = null;
			int num;
			switch (this.qualifierType)
			{
			case QualifierType.ObjectQualifier:
				num = this.parent.wbemObject.GetQualifierSet_(out this.qualifierSet);
				break;
			case QualifierType.PropertyQualifier:
				num = this.parent.wbemObject.GetPropertyQualifierSet_(this.propertyOrMethodName, out this.qualifierSet);
				break;
			case QualifierType.MethodQualifier:
				num = this.parent.wbemObject.GetMethodQualifierSet_(this.propertyOrMethodName, out this.qualifierSet);
				break;
			default:
				throw new ManagementException(ManagementStatus.Unexpected, null, null);
			}
			if (((long)num & (long)((ulong)(-2147483648))) == 0L)
			{
				this.qualifierValue = null;
				if (this.qualifierSet != null)
				{
					num = this.qualifierSet.Get_(this.qualifierName, 0, ref this.qualifierValue, ref this.qualifierFlavor);
				}
			}
			if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				return;
			}
			if (((long)num & (long)((ulong)(-2147483648))) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000F9B8 File Offset: 0x0000E9B8
		private static object MapQualValueToWmiValue(object qualVal)
		{
			object obj = DBNull.Value;
			if (qualVal != null)
			{
				if (qualVal is Array)
				{
					if (qualVal is int[] || qualVal is double[] || qualVal is string[] || qualVal is bool[])
					{
						obj = qualVal;
					}
					else
					{
						Array array = (Array)qualVal;
						int length = array.Length;
						Type type = ((length > 0) ? array.GetValue(0).GetType() : null);
						if (type == typeof(int))
						{
							obj = new int[length];
							for (int i = 0; i < length; i++)
							{
								((int[])obj)[i] = Convert.ToInt32(array.GetValue(i), (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int)));
							}
						}
						else if (type == typeof(double))
						{
							obj = new double[length];
							for (int j = 0; j < length; j++)
							{
								((double[])obj)[j] = Convert.ToDouble(array.GetValue(j), (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(double)));
							}
						}
						else if (type == typeof(string))
						{
							obj = new string[length];
							for (int k = 0; k < length; k++)
							{
								((string[])obj)[k] = array.GetValue(k).ToString();
							}
						}
						else if (type == typeof(bool))
						{
							obj = new bool[length];
							for (int l = 0; l < length; l++)
							{
								((bool[])obj)[l] = Convert.ToBoolean(array.GetValue(l), (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(bool)));
							}
						}
						else
						{
							obj = array;
						}
					}
				}
				else
				{
					obj = qualVal;
				}
			}
			return obj;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000FB6D File Offset: 0x0000EB6D
		public string Name
		{
			get
			{
				if (this.qualifierName == null)
				{
					return "";
				}
				return this.qualifierName;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000FB83 File Offset: 0x0000EB83
		// (set) Token: 0x06000392 RID: 914 RVA: 0x0000FB98 File Offset: 0x0000EB98
		public object Value
		{
			get
			{
				this.RefreshQualifierInfo();
				return ValueTypeSafety.GetSafeObject(this.qualifierValue);
			}
			set
			{
				this.RefreshQualifierInfo();
				object obj = QualifierData.MapQualValueToWmiValue(value);
				int num = this.qualifierSet.Put_(this.qualifierName, ref obj, this.qualifierFlavor & -97);
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				if (((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000FC06 File Offset: 0x0000EC06
		// (set) Token: 0x06000394 RID: 916 RVA: 0x0000FC24 File Offset: 0x0000EC24
		public bool IsAmended
		{
			get
			{
				this.RefreshQualifierInfo();
				return 128 == (this.qualifierFlavor & 128);
			}
			set
			{
				this.RefreshQualifierInfo();
				int num = this.qualifierFlavor & -97;
				if (value)
				{
					num |= 128;
				}
				else
				{
					num &= -129;
				}
				int num2 = this.qualifierSet.Put_(this.qualifierName, ref this.qualifierValue, num);
				if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				if (((long)num2 & (long)((ulong)(-2147483648))) != 0L)
				{
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0000FCA6 File Offset: 0x0000ECA6
		public bool IsLocal
		{
			get
			{
				this.RefreshQualifierInfo();
				return 0 == (this.qualifierFlavor & 96);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000FCBA File Offset: 0x0000ECBA
		// (set) Token: 0x06000397 RID: 919 RVA: 0x0000FCD0 File Offset: 0x0000ECD0
		public bool PropagatesToInstance
		{
			get
			{
				this.RefreshQualifierInfo();
				return 1 == (this.qualifierFlavor & 1);
			}
			set
			{
				this.RefreshQualifierInfo();
				int num = this.qualifierFlavor & -97;
				if (value)
				{
					num |= 1;
				}
				else
				{
					num &= -2;
				}
				int num2 = this.qualifierSet.Put_(this.qualifierName, ref this.qualifierValue, num);
				if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				if (((long)num2 & (long)((ulong)(-2147483648))) != 0L)
				{
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0000FD4B File Offset: 0x0000ED4B
		// (set) Token: 0x06000399 RID: 921 RVA: 0x0000FD60 File Offset: 0x0000ED60
		public bool PropagatesToSubclass
		{
			get
			{
				this.RefreshQualifierInfo();
				return 2 == (this.qualifierFlavor & 2);
			}
			set
			{
				this.RefreshQualifierInfo();
				int num = this.qualifierFlavor & -97;
				if (value)
				{
					num |= 2;
				}
				else
				{
					num &= -3;
				}
				int num2 = this.qualifierSet.Put_(this.qualifierName, ref this.qualifierValue, num);
				if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				if (((long)num2 & (long)((ulong)(-2147483648))) != 0L)
				{
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000FDDB File Offset: 0x0000EDDB
		// (set) Token: 0x0600039B RID: 923 RVA: 0x0000FDF0 File Offset: 0x0000EDF0
		public bool IsOverridable
		{
			get
			{
				this.RefreshQualifierInfo();
				return 0 == (this.qualifierFlavor & 16);
			}
			set
			{
				this.RefreshQualifierInfo();
				int num = this.qualifierFlavor & -97;
				if (value)
				{
					num &= -17;
				}
				else
				{
					num |= 16;
				}
				int num2 = this.qualifierSet.Put_(this.qualifierName, ref this.qualifierValue, num);
				if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
					return;
				}
				if (((long)num2 & (long)((ulong)(-2147483648))) != 0L)
				{
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}
		}

		// Token: 0x040001DA RID: 474
		private ManagementBaseObject parent;

		// Token: 0x040001DB RID: 475
		private string propertyOrMethodName;

		// Token: 0x040001DC RID: 476
		private string qualifierName;

		// Token: 0x040001DD RID: 477
		private QualifierType qualifierType;

		// Token: 0x040001DE RID: 478
		private object qualifierValue;

		// Token: 0x040001DF RID: 479
		private int qualifierFlavor;

		// Token: 0x040001E0 RID: 480
		private IWbemQualifierSetFreeThreaded qualifierSet;
	}
}
