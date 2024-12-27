using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.EnterpriseServices
{
	// Token: 0x02000038 RID: 56
	[ComVisible(false)]
	[Serializable]
	public sealed class ServicedComponentException : SystemException
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00004FEA File Offset: 0x00003FEA
		private static string DefaultMessage
		{
			get
			{
				if (ServicedComponentException._default == null)
				{
					ServicedComponentException._default = Resource.FormatString("ServicedComponentException_Default");
				}
				return ServicedComponentException._default;
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005007 File Offset: 0x00004007
		public ServicedComponentException()
			: base(ServicedComponentException.DefaultMessage)
		{
			base.HResult = -2146233073;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000501F File Offset: 0x0000401F
		public ServicedComponentException(string message)
			: base(message)
		{
			base.HResult = -2146233073;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005033 File Offset: 0x00004033
		public ServicedComponentException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146233073;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005048 File Offset: 0x00004048
		private ServicedComponentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0400007A RID: 122
		private const int COR_E_SERVICEDCOMPONENT = -2146233073;

		// Token: 0x0400007B RID: 123
		private static string _default;
	}
}
