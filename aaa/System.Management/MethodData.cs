using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000083 RID: 131
	public class MethodData
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x00010392 File Offset: 0x0000F392
		internal MethodData(ManagementObject parent, string methodName)
		{
			this.parent = parent;
			this.methodName = methodName;
			this.RefreshMethodInfo();
			this.qualifiers = null;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000103B8 File Offset: 0x0000F3B8
		private void RefreshMethodInfo()
		{
			int num = -2147217407;
			try
			{
				num = this.parent.wbemObject.GetMethod_(this.methodName, 0, out this.wmiInParams, out this.wmiOutParams);
			}
			catch (COMException ex)
			{
				ManagementException.ThrowWithExtendedInfo(ex);
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

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00010440 File Offset: 0x0000F440
		public string Name
		{
			get
			{
				if (this.methodName == null)
				{
					return "";
				}
				return this.methodName;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00010456 File Offset: 0x0000F456
		public ManagementBaseObject InParameters
		{
			get
			{
				this.RefreshMethodInfo();
				if (this.wmiInParams != null)
				{
					return new ManagementBaseObject(this.wmiInParams);
				}
				return null;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x00010473 File Offset: 0x0000F473
		public ManagementBaseObject OutParameters
		{
			get
			{
				this.RefreshMethodInfo();
				if (this.wmiOutParams != null)
				{
					return new ManagementBaseObject(this.wmiOutParams);
				}
				return null;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00010490 File Offset: 0x0000F490
		public string Origin
		{
			get
			{
				string text = null;
				int methodOrigin_ = this.parent.wbemObject.GetMethodOrigin_(this.methodName, out text);
				if (methodOrigin_ < 0)
				{
					if (methodOrigin_ == -2147217393)
					{
						text = string.Empty;
					}
					else if (((long)methodOrigin_ & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)methodOrigin_);
					}
					else
					{
						Marshal.ThrowExceptionForHR(methodOrigin_, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
				return text;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x000104F6 File Offset: 0x0000F4F6
		public QualifierDataCollection Qualifiers
		{
			get
			{
				if (this.qualifiers == null)
				{
					this.qualifiers = new QualifierDataCollection(this.parent, this.methodName, QualifierType.MethodQualifier);
				}
				return this.qualifiers;
			}
		}

		// Token: 0x040001E9 RID: 489
		private ManagementObject parent;

		// Token: 0x040001EA RID: 490
		private string methodName;

		// Token: 0x040001EB RID: 491
		private IWbemClassObjectFreeThreaded wmiInParams;

		// Token: 0x040001EC RID: 492
		private IWbemClassObjectFreeThreaded wmiOutParams;

		// Token: 0x040001ED RID: 493
		private QualifierDataCollection qualifiers;
	}
}
