using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x020000AC RID: 172
	[ComVisible(true)]
	[Serializable]
	public class TypeLoadException : SystemException, ISerializable
	{
		// Token: 0x06000A56 RID: 2646 RVA: 0x0001F959 File Offset: 0x0001E959
		public TypeLoadException()
			: base(Environment.GetResourceString("Arg_TypeLoadException"))
		{
			base.SetErrorCode(-2146233054);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0001F976 File Offset: 0x0001E976
		public TypeLoadException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233054);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0001F98A File Offset: 0x0001E98A
		public TypeLoadException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233054);
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0001F99F File Offset: 0x0001E99F
		public override string Message
		{
			get
			{
				this.SetMessageField();
				return this._message;
			}
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0001F9B0 File Offset: 0x0001E9B0
		private void SetMessageField()
		{
			if (this._message == null)
			{
				if (this.ClassName == null && this.ResourceId == 0)
				{
					this._message = Environment.GetResourceString("Arg_TypeLoadException");
					return;
				}
				if (this.AssemblyName == null)
				{
					this.AssemblyName = Environment.GetResourceString("IO_UnknownFileName");
				}
				if (this.ClassName == null)
				{
					this.ClassName = Environment.GetResourceString("IO_UnknownFileName");
				}
				this._message = string.Format(CultureInfo.CurrentCulture, TypeLoadException.GetTypeLoadExceptionMessage(this.ResourceId), new object[] { this.ClassName, this.AssemblyName, this.MessageArg });
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0001FA57 File Offset: 0x0001EA57
		public string TypeName
		{
			get
			{
				if (this.ClassName == null)
				{
					return string.Empty;
				}
				return this.ClassName;
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0001FA6D File Offset: 0x0001EA6D
		private TypeLoadException(string className, string assemblyName, string messageArg, int resourceId)
			: base(null)
		{
			base.SetErrorCode(-2146233054);
			this.ClassName = className;
			this.AssemblyName = assemblyName;
			this.MessageArg = messageArg;
			this.ResourceId = resourceId;
			this.SetMessageField();
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0001FAA4 File Offset: 0x0001EAA4
		protected TypeLoadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.ClassName = info.GetString("TypeLoadClassName");
			this.AssemblyName = info.GetString("TypeLoadAssemblyName");
			this.MessageArg = info.GetString("TypeLoadMessageArg");
			this.ResourceId = info.GetInt32("TypeLoadResourceID");
		}

		// Token: 0x06000A5E RID: 2654
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetTypeLoadExceptionMessage(int resourceId);

		// Token: 0x06000A5F RID: 2655 RVA: 0x0001FB0C File Offset: 0x0001EB0C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("TypeLoadClassName", this.ClassName, typeof(string));
			info.AddValue("TypeLoadAssemblyName", this.AssemblyName, typeof(string));
			info.AddValue("TypeLoadMessageArg", this.MessageArg, typeof(string));
			info.AddValue("TypeLoadResourceID", this.ResourceId);
		}

		// Token: 0x040003A2 RID: 930
		private string ClassName;

		// Token: 0x040003A3 RID: 931
		private string AssemblyName;

		// Token: 0x040003A4 RID: 932
		private string MessageArg;

		// Token: 0x040003A5 RID: 933
		internal int ResourceId;
	}
}
