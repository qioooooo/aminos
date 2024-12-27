using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x020000D5 RID: 213
	[ComVisible(true)]
	[Serializable]
	public class MissingMemberException : MemberAccessException, ISerializable
	{
		// Token: 0x06000BFE RID: 3070 RVA: 0x00023CE5 File Offset: 0x00022CE5
		public MissingMemberException()
			: base(Environment.GetResourceString("Arg_MissingMemberException"))
		{
			base.SetErrorCode(-2146233070);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00023D02 File Offset: 0x00022D02
		public MissingMemberException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233070);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00023D16 File Offset: 0x00022D16
		public MissingMemberException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233070);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00023D2C File Offset: 0x00022D2C
		protected MissingMemberException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.ClassName = info.GetString("MMClassName");
			this.MemberName = info.GetString("MMMemberName");
			this.Signature = (byte[])info.GetValue("MMSignature", typeof(byte[]));
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x00023D84 File Offset: 0x00022D84
		public override string Message
		{
			get
			{
				if (this.ClassName == null)
				{
					return base.Message;
				}
				return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingMember_Name", new object[] { this.ClassName + "." + this.MemberName + ((this.Signature != null) ? (" " + MissingMemberException.FormatSignature(this.Signature)) : "") }), new object[0]);
			}
		}

		// Token: 0x06000C03 RID: 3075
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string FormatSignature(byte[] signature);

		// Token: 0x06000C04 RID: 3076 RVA: 0x00023DFF File Offset: 0x00022DFF
		private MissingMemberException(string className, string memberName, byte[] signature)
		{
			this.ClassName = className;
			this.MemberName = memberName;
			this.Signature = signature;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00023E1C File Offset: 0x00022E1C
		public MissingMemberException(string className, string memberName)
		{
			this.ClassName = className;
			this.MemberName = memberName;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00023E34 File Offset: 0x00022E34
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("MMClassName", this.ClassName, typeof(string));
			info.AddValue("MMMemberName", this.MemberName, typeof(string));
			info.AddValue("MMSignature", this.Signature, typeof(byte[]));
		}

		// Token: 0x0400041B RID: 1051
		protected string ClassName;

		// Token: 0x0400041C RID: 1052
		protected string MemberName;

		// Token: 0x0400041D RID: 1053
		protected byte[] Signature;
	}
}
