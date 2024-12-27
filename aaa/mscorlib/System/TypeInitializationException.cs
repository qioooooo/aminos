using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x0200011A RID: 282
	[ComVisible(true)]
	[Serializable]
	public sealed class TypeInitializationException : SystemException
	{
		// Token: 0x0600106D RID: 4205 RVA: 0x0002EC01 File Offset: 0x0002DC01
		private TypeInitializationException()
			: base(Environment.GetResourceString("TypeInitialization_Default"))
		{
			base.SetErrorCode(-2146233036);
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0002EC1E File Offset: 0x0002DC1E
		private TypeInitializationException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233036);
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0002EC34 File Offset: 0x0002DC34
		public TypeInitializationException(string fullTypeName, Exception innerException)
			: base(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("TypeInitialization_Type"), new object[] { fullTypeName }), innerException)
		{
			this._typeName = fullTypeName;
			base.SetErrorCode(-2146233036);
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0002EC7A File Offset: 0x0002DC7A
		internal TypeInitializationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._typeName = info.GetString("TypeName");
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x0002EC95 File Offset: 0x0002DC95
		public string TypeName
		{
			get
			{
				if (this._typeName == null)
				{
					return string.Empty;
				}
				return this._typeName;
			}
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0002ECAB File Offset: 0x0002DCAB
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("TypeName", this.TypeName, typeof(string));
		}

		// Token: 0x04000568 RID: 1384
		private string _typeName;
	}
}
