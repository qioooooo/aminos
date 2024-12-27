using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x02000015 RID: 21
	[Guid("5f44bb1a-465a-37db-8e84-acc733bf7541")]
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	[Serializable]
	public class VsaException : ExternalException
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00002658 File Offset: 0x00001658
		public VsaException()
		{
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002660 File Offset: 0x00001660
		public VsaException(string message)
			: base(message)
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002669 File Offset: 0x00001669
		public VsaException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002674 File Offset: 0x00001674
		public VsaException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			base.HResult = (int)info.GetValue("VsaException_HResult", typeof(int));
			this.HelpLink = (string)info.GetValue("VsaException_HelpLink", typeof(string));
			this.Source = (string)info.GetValue("VsaException_Source", typeof(string));
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000026E9 File Offset: 0x000016E9
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("VsaException_HResult", base.HResult);
			info.AddValue("VsaException_HelpLink", this.HelpLink);
			info.AddValue("VsaException_Source", this.Source);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002728 File Offset: 0x00001728
		public override string ToString()
		{
			if (this.Message != null && "" != this.Message)
			{
				return string.Concat(new string[]
				{
					"Microsoft.Vsa.VsaException: ",
					Enum.GetName(((VsaError)base.HResult).GetType(), (VsaError)base.HResult),
					" (0x",
					string.Format(CultureInfo.InvariantCulture, "{0,8:X}", new object[] { base.HResult }),
					"): ",
					this.Message
				});
			}
			return string.Concat(new string[]
			{
				"Microsoft.Vsa.VsaException: ",
				Enum.GetName(((VsaError)base.HResult).GetType(), (VsaError)base.HResult),
				" (0x",
				string.Format(CultureInfo.InvariantCulture, "{0,8:X}", new object[] { base.HResult }),
				")."
			});
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000283C File Offset: 0x0000183C
		public VsaException(VsaError error)
			: base(string.Empty, (int)error)
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000284A File Offset: 0x0000184A
		public VsaException(VsaError error, string message)
			: base(message, (int)error)
		{
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00002854 File Offset: 0x00001854
		public VsaException(VsaError error, string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = (int)error;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00002865 File Offset: 0x00001865
		public new VsaError ErrorCode
		{
			get
			{
				return (VsaError)base.HResult;
			}
		}
	}
}
