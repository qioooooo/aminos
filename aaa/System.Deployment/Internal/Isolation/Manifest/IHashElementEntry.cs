using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000182 RID: 386
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9D46FB70-7B54-4f4f-9331-BA9E87833FF5")]
	[ComImport]
	internal interface IHashElementEntry
	{
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600077F RID: 1919
		HashElementEntry AllData { get; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000780 RID: 1920
		uint index { get; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000781 RID: 1921
		byte Transform { get; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000782 RID: 1922
		object TransformMetadata
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000783 RID: 1923
		byte DigestMethod { get; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000784 RID: 1924
		object DigestValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000785 RID: 1925
		string Xml
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
