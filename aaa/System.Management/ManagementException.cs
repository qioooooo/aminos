using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management
{
	// Token: 0x0200001C RID: 28
	[Serializable]
	public class ManagementException : SystemException
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x00006CCC File Offset: 0x00005CCC
		internal static void ThrowWithExtendedInfo(ManagementStatus errorCode)
		{
			ManagementBaseObject managementBaseObject = null;
			string text = null;
			IWbemClassObjectFreeThreaded errorInfo = WbemErrorInfo.GetErrorInfo();
			if (errorInfo != null)
			{
				managementBaseObject = new ManagementBaseObject(errorInfo);
			}
			if ((text = ManagementException.GetMessage(errorCode)) == null && managementBaseObject != null)
			{
				try
				{
					text = (string)managementBaseObject["Description"];
				}
				catch
				{
				}
			}
			throw new ManagementException(errorCode, text, managementBaseObject);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006D28 File Offset: 0x00005D28
		internal static void ThrowWithExtendedInfo(Exception e)
		{
			ManagementBaseObject managementBaseObject = null;
			string text = null;
			IWbemClassObjectFreeThreaded errorInfo = WbemErrorInfo.GetErrorInfo();
			if (errorInfo != null)
			{
				managementBaseObject = new ManagementBaseObject(errorInfo);
			}
			if ((text = ManagementException.GetMessage(e)) == null && managementBaseObject != null)
			{
				try
				{
					text = (string)managementBaseObject["Description"];
				}
				catch
				{
				}
			}
			throw new ManagementException(e, text, managementBaseObject);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006D84 File Offset: 0x00005D84
		internal ManagementException(ManagementStatus errorCode, string msg, ManagementBaseObject errObj)
			: base(msg)
		{
			this.errorCode = errorCode;
			this.errorObject = errObj;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006D9C File Offset: 0x00005D9C
		internal ManagementException(Exception e, string msg, ManagementBaseObject errObj)
			: base(msg, e)
		{
			try
			{
				if (e is ManagementException)
				{
					this.errorCode = ((ManagementException)e).ErrorCode;
					if (this.errorObject != null)
					{
						this.errorObject = (ManagementBaseObject)((ManagementException)e).errorObject.Clone();
					}
					else
					{
						this.errorObject = null;
					}
				}
				else if (e is COMException)
				{
					this.errorCode = (ManagementStatus)((COMException)e).ErrorCode;
				}
				else
				{
					this.errorCode = (ManagementStatus)base.HResult;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006E34 File Offset: 0x00005E34
		protected ManagementException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.errorCode = (ManagementStatus)info.GetValue("errorCode", typeof(ManagementStatus));
			this.errorObject = info.GetValue("errorObject", typeof(ManagementBaseObject)) as ManagementBaseObject;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006E89 File Offset: 0x00005E89
		public ManagementException()
			: this(ManagementStatus.Failed, "", null)
		{
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006E9C File Offset: 0x00005E9C
		public ManagementException(string message)
			: this(ManagementStatus.Failed, message, null)
		{
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006EAB File Offset: 0x00005EAB
		public ManagementException(string message, Exception innerException)
			: this(innerException, message, null)
		{
			if (!(innerException is ManagementException))
			{
				this.errorCode = ManagementStatus.Failed;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006EC9 File Offset: 0x00005EC9
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("errorCode", this.errorCode);
			info.AddValue("errorObject", this.errorObject);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006EFC File Offset: 0x00005EFC
		private static string GetMessage(Exception e)
		{
			string text = null;
			if (e is COMException)
			{
				text = ManagementException.GetMessage((ManagementStatus)((COMException)e).ErrorCode);
			}
			if (text == null)
			{
				text = e.Message;
			}
			return text;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006F30 File Offset: 0x00005F30
		private static string GetMessage(ManagementStatus errorCode)
		{
			string text = null;
			IWbemStatusCodeText wbemStatusCodeText = (IWbemStatusCodeText)new WbemStatusCodeText();
			if (wbemStatusCodeText != null)
			{
				try
				{
					int num = wbemStatusCodeText.GetErrorCodeText_((int)errorCode, 0U, 1, out text);
					if (num != 0)
					{
						num = wbemStatusCodeText.GetErrorCodeText_((int)errorCode, 0U, 0, out text);
					}
				}
				catch
				{
				}
			}
			return text;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00006F80 File Offset: 0x00005F80
		public ManagementBaseObject ErrorInformation
		{
			get
			{
				return this.errorObject;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00006F88 File Offset: 0x00005F88
		public ManagementStatus ErrorCode
		{
			get
			{
				return this.errorCode;
			}
		}

		// Token: 0x04000107 RID: 263
		private ManagementBaseObject errorObject;

		// Token: 0x04000108 RID: 264
		private ManagementStatus errorCode;
	}
}
