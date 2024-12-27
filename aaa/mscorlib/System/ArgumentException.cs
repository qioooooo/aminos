using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x0200006D RID: 109
	[ComVisible(true)]
	[Serializable]
	public class ArgumentException : SystemException, ISerializable
	{
		// Token: 0x06000646 RID: 1606 RVA: 0x00015924 File Offset: 0x00014924
		public ArgumentException()
			: base(Environment.GetResourceString("Arg_ArgumentException"))
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00015941 File Offset: 0x00014941
		public ArgumentException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00015955 File Offset: 0x00014955
		public ArgumentException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001596A File Offset: 0x0001496A
		public ArgumentException(string message, string paramName, Exception innerException)
			: base(message, innerException)
		{
			this.m_paramName = paramName;
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00015986 File Offset: 0x00014986
		public ArgumentException(string message, string paramName)
			: base(message)
		{
			this.m_paramName = paramName;
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x000159A1 File Offset: 0x000149A1
		protected ArgumentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.m_paramName = info.GetString("ParamName");
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x000159BC File Offset: 0x000149BC
		public override string Message
		{
			get
			{
				string message = base.Message;
				if (this.m_paramName != null && this.m_paramName.Length != 0)
				{
					return message + Environment.NewLine + string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_ParamName_Name"), new object[] { this.m_paramName });
				}
				return message;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00015A17 File Offset: 0x00014A17
		public virtual string ParamName
		{
			get
			{
				return this.m_paramName;
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00015A1F File Offset: 0x00014A1F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ParamName", this.m_paramName, typeof(string));
		}

		// Token: 0x040001F6 RID: 502
		private string m_paramName;
	}
}
