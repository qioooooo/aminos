using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000076 RID: 118
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0C66F299-E08E-48c5-9264-7CCBEB4D5CBB")]
	[ComImport]
	internal interface IFileAssociationEntry
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600024F RID: 591
		FileAssociationEntry AllData { get; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000250 RID: 592
		string Extension
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000251 RID: 593
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000252 RID: 594
		string ProgID
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000253 RID: 595
		string DefaultIcon
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000254 RID: 596
		string Parameter
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
