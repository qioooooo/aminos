using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000327 RID: 807
	[ComVisible(true)]
	[Serializable]
	public sealed class ReflectionTypeLoadException : SystemException, ISerializable
	{
		// Token: 0x06001F68 RID: 8040 RVA: 0x0004F793 File Offset: 0x0004E793
		private ReflectionTypeLoadException()
			: base(Environment.GetResourceString("ReflectionTypeLoad_LoadFailed"))
		{
			base.SetErrorCode(-2146232830);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x0004F7B0 File Offset: 0x0004E7B0
		private ReflectionTypeLoadException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146232830);
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x0004F7C4 File Offset: 0x0004E7C4
		public ReflectionTypeLoadException(Type[] classes, Exception[] exceptions)
			: base(null)
		{
			this._classes = classes;
			this._exceptions = exceptions;
			base.SetErrorCode(-2146232830);
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x0004F7E6 File Offset: 0x0004E7E6
		public ReflectionTypeLoadException(Type[] classes, Exception[] exceptions, string message)
			: base(message)
		{
			this._classes = classes;
			this._exceptions = exceptions;
			base.SetErrorCode(-2146232830);
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x0004F808 File Offset: 0x0004E808
		internal ReflectionTypeLoadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._classes = (Type[])info.GetValue("Types", typeof(Type[]));
			this._exceptions = (Exception[])info.GetValue("Exceptions", typeof(Exception[]));
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x0004F85D File Offset: 0x0004E85D
		public Type[] Types
		{
			get
			{
				return this._classes;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001F6E RID: 8046 RVA: 0x0004F865 File Offset: 0x0004E865
		public Exception[] LoaderExceptions
		{
			get
			{
				return this._exceptions;
			}
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x0004F870 File Offset: 0x0004E870
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("Types", this._classes, typeof(Type[]));
			info.AddValue("Exceptions", this._exceptions, typeof(Exception[]));
		}

		// Token: 0x04000D68 RID: 3432
		private Type[] _classes;

		// Token: 0x04000D69 RID: 3433
		private Exception[] _exceptions;
	}
}
