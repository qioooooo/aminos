using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x02000709 RID: 1801
	[ComVisible(true)]
	public interface IFieldInfo
	{
		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06004111 RID: 16657
		// (set) Token: 0x06004112 RID: 16658
		string[] FieldNames
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			set;
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06004113 RID: 16659
		// (set) Token: 0x06004114 RID: 16660
		Type[] FieldTypes
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			set;
		}
	}
}
