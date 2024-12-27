using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security.AccessControl
{
	// Token: 0x0200091B RID: 2331
	[Serializable]
	public sealed class PrivilegeNotHeldException : UnauthorizedAccessException, ISerializable
	{
		// Token: 0x0600549C RID: 21660 RVA: 0x00133F74 File Offset: 0x00132F74
		public PrivilegeNotHeldException()
			: base(Environment.GetResourceString("PrivilegeNotHeld_Default"))
		{
		}

		// Token: 0x0600549D RID: 21661 RVA: 0x00133F88 File Offset: 0x00132F88
		public PrivilegeNotHeldException(string privilege)
			: base(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("PrivilegeNotHeld_Named"), new object[] { privilege }))
		{
			this._privilegeName = privilege;
		}

		// Token: 0x0600549E RID: 21662 RVA: 0x00133FC4 File Offset: 0x00132FC4
		public PrivilegeNotHeldException(string privilege, Exception inner)
			: base(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("PrivilegeNotHeld_Named"), new object[] { privilege }), inner)
		{
			this._privilegeName = privilege;
		}

		// Token: 0x0600549F RID: 21663 RVA: 0x00133FFF File Offset: 0x00132FFF
		internal PrivilegeNotHeldException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._privilegeName = info.GetString("PrivilegeName");
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x060054A0 RID: 21664 RVA: 0x0013401A File Offset: 0x0013301A
		public string PrivilegeName
		{
			get
			{
				return this._privilegeName;
			}
		}

		// Token: 0x060054A1 RID: 21665 RVA: 0x00134022 File Offset: 0x00133022
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("PrivilegeName", this._privilegeName, typeof(string));
		}

		// Token: 0x04002BD4 RID: 11220
		private readonly string _privilegeName;
	}
}
