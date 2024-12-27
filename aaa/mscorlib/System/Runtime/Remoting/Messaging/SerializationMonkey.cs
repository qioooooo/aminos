using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200070A RID: 1802
	[Serializable]
	internal class SerializationMonkey : ISerializable, IFieldInfo
	{
		// Token: 0x06004115 RID: 16661 RVA: 0x000DEA4C File Offset: 0x000DDA4C
		internal SerializationMonkey(SerializationInfo info, StreamingContext ctx)
		{
			this._obj.RootSetObjectData(info, ctx);
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x000DEA61 File Offset: 0x000DDA61
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06004117 RID: 16663 RVA: 0x000DEA72 File Offset: 0x000DDA72
		// (set) Token: 0x06004118 RID: 16664 RVA: 0x000DEA7A File Offset: 0x000DDA7A
		public string[] FieldNames
		{
			get
			{
				return this.fieldNames;
			}
			set
			{
				this.fieldNames = value;
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06004119 RID: 16665 RVA: 0x000DEA83 File Offset: 0x000DDA83
		// (set) Token: 0x0600411A RID: 16666 RVA: 0x000DEA8B File Offset: 0x000DDA8B
		public Type[] FieldTypes
		{
			get
			{
				return this.fieldTypes;
			}
			set
			{
				this.fieldTypes = value;
			}
		}

		// Token: 0x04002091 RID: 8337
		internal ISerializationRootObject _obj;

		// Token: 0x04002092 RID: 8338
		internal string[] fieldNames;

		// Token: 0x04002093 RID: 8339
		internal Type[] fieldTypes;
	}
}
