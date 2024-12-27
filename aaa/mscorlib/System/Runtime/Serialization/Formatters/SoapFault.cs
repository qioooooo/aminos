using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007A6 RID: 1958
	[SoapType(Embedded = true)]
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapFault : ISerializable
	{
		// Token: 0x0600463A RID: 17978 RVA: 0x000F04E4 File Offset: 0x000EF4E4
		public SoapFault()
		{
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x000F04EC File Offset: 0x000EF4EC
		public SoapFault(string faultCode, string faultString, string faultActor, ServerFault serverFault)
		{
			this.faultCode = faultCode;
			this.faultString = faultString;
			this.faultActor = faultActor;
			this.detail = serverFault;
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x000F0514 File Offset: 0x000EF514
		internal SoapFault(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name = enumerator.Name;
				object value = enumerator.Value;
				if (string.Compare(name, "faultCode", true, CultureInfo.InvariantCulture) == 0)
				{
					int num = ((string)value).IndexOf(':');
					if (num > -1)
					{
						this.faultCode = ((string)value).Substring(num + 1);
					}
					else
					{
						this.faultCode = (string)value;
					}
				}
				else if (string.Compare(name, "faultString", true, CultureInfo.InvariantCulture) == 0)
				{
					this.faultString = (string)value;
				}
				else if (string.Compare(name, "faultActor", true, CultureInfo.InvariantCulture) == 0)
				{
					this.faultActor = (string)value;
				}
				else if (string.Compare(name, "detail", true, CultureInfo.InvariantCulture) == 0)
				{
					this.detail = value;
				}
			}
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x000F05F4 File Offset: 0x000EF5F4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("faultcode", "SOAP-ENV:" + this.faultCode);
			info.AddValue("faultstring", this.faultString);
			if (this.faultActor != null)
			{
				info.AddValue("faultactor", this.faultActor);
			}
			info.AddValue("detail", this.detail, typeof(object));
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x0600463E RID: 17982 RVA: 0x000F0661 File Offset: 0x000EF661
		// (set) Token: 0x0600463F RID: 17983 RVA: 0x000F0669 File Offset: 0x000EF669
		public string FaultCode
		{
			get
			{
				return this.faultCode;
			}
			set
			{
				this.faultCode = value;
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06004640 RID: 17984 RVA: 0x000F0672 File Offset: 0x000EF672
		// (set) Token: 0x06004641 RID: 17985 RVA: 0x000F067A File Offset: 0x000EF67A
		public string FaultString
		{
			get
			{
				return this.faultString;
			}
			set
			{
				this.faultString = value;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06004642 RID: 17986 RVA: 0x000F0683 File Offset: 0x000EF683
		// (set) Token: 0x06004643 RID: 17987 RVA: 0x000F068B File Offset: 0x000EF68B
		public string FaultActor
		{
			get
			{
				return this.faultActor;
			}
			set
			{
				this.faultActor = value;
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06004644 RID: 17988 RVA: 0x000F0694 File Offset: 0x000EF694
		// (set) Token: 0x06004645 RID: 17989 RVA: 0x000F069C File Offset: 0x000EF69C
		public object Detail
		{
			get
			{
				return this.detail;
			}
			set
			{
				this.detail = value;
			}
		}

		// Token: 0x040022C4 RID: 8900
		private string faultCode;

		// Token: 0x040022C5 RID: 8901
		private string faultString;

		// Token: 0x040022C6 RID: 8902
		private string faultActor;

		// Token: 0x040022C7 RID: 8903
		[SoapField(Embedded = true)]
		private object detail;
	}
}
