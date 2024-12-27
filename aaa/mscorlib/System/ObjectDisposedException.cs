using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x020000E0 RID: 224
	[ComVisible(true)]
	[Serializable]
	public class ObjectDisposedException : InvalidOperationException
	{
		// Token: 0x06000C5A RID: 3162 RVA: 0x0002525E File Offset: 0x0002425E
		private ObjectDisposedException()
			: this(null, Environment.GetResourceString("ObjectDisposed_Generic"))
		{
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00025271 File Offset: 0x00024271
		public ObjectDisposedException(string objectName)
			: this(objectName, Environment.GetResourceString("ObjectDisposed_Generic"))
		{
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00025284 File Offset: 0x00024284
		public ObjectDisposedException(string objectName, string message)
			: base(message)
		{
			base.SetErrorCode(-2146232798);
			this.objectName = objectName;
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0002529F File Offset: 0x0002429F
		public ObjectDisposedException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146232798);
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x000252B4 File Offset: 0x000242B4
		public override string Message
		{
			get
			{
				string text = this.ObjectName;
				if (text == null || text.Length == 0)
				{
					return base.Message;
				}
				return base.Message + Environment.NewLine + string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ObjectDisposed_ObjectName_Name"), new object[] { text });
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x0002530A File Offset: 0x0002430A
		public string ObjectName
		{
			get
			{
				if (this.objectName == null)
				{
					return string.Empty;
				}
				return this.objectName;
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00025320 File Offset: 0x00024320
		protected ObjectDisposedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.objectName = info.GetString("ObjectName");
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0002533B File Offset: 0x0002433B
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("ObjectName", this.ObjectName, typeof(string));
		}

		// Token: 0x0400042A RID: 1066
		private string objectName;
	}
}
