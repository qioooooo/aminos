using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200022C RID: 556
	internal static class IsolationInterop
	{
		// Token: 0x060015B9 RID: 5561 RVA: 0x00037E2A File Offset: 0x00036E2A
		public static Store GetUserStore()
		{
			return new Store(IsolationInterop.GetUserStore(0U, IntPtr.Zero, ref IsolationInterop.IID_IStore) as IStore);
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060015BA RID: 5562 RVA: 0x00037E48 File Offset: 0x00036E48
		public static IIdentityAuthority IdentityAuthority
		{
			get
			{
				if (IsolationInterop._idAuth == null)
				{
					lock (IsolationInterop._synchObject)
					{
						if (IsolationInterop._idAuth == null)
						{
							IsolationInterop._idAuth = IsolationInterop.GetIdentityAuthority();
						}
					}
				}
				return IsolationInterop._idAuth;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x060015BB RID: 5563 RVA: 0x00037E98 File Offset: 0x00036E98
		public static IAppIdAuthority AppIdAuthority
		{
			get
			{
				if (IsolationInterop._appIdAuth == null)
				{
					lock (IsolationInterop._synchObject)
					{
						if (IsolationInterop._appIdAuth == null)
						{
							IsolationInterop._appIdAuth = IsolationInterop.GetAppIdAuthority();
						}
					}
				}
				return IsolationInterop._appIdAuth;
			}
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x00037EE8 File Offset: 0x00036EE8
		internal static IActContext CreateActContext(IDefinitionAppId AppId)
		{
			IsolationInterop.CreateActContextParameters createActContextParameters;
			createActContextParameters.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParameters));
			createActContextParameters.Flags = 16U;
			createActContextParameters.CustomStoreList = IntPtr.Zero;
			createActContextParameters.CultureFallbackList = IntPtr.Zero;
			createActContextParameters.ProcessorArchitectureList = IntPtr.Zero;
			createActContextParameters.Source = IntPtr.Zero;
			createActContextParameters.ProcArch = 0;
			IsolationInterop.CreateActContextParametersSource createActContextParametersSource;
			createActContextParametersSource.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParametersSource));
			createActContextParametersSource.Flags = 0U;
			createActContextParametersSource.SourceType = 1U;
			createActContextParametersSource.Data = IntPtr.Zero;
			IsolationInterop.CreateActContextParametersSourceDefinitionAppid createActContextParametersSourceDefinitionAppid;
			createActContextParametersSourceDefinitionAppid.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParametersSourceDefinitionAppid));
			createActContextParametersSourceDefinitionAppid.Flags = 0U;
			createActContextParametersSourceDefinitionAppid.AppId = AppId;
			IActContext actContext;
			try
			{
				createActContextParametersSource.Data = createActContextParametersSourceDefinitionAppid.ToIntPtr();
				createActContextParameters.Source = createActContextParametersSource.ToIntPtr();
				actContext = IsolationInterop.CreateActContext(ref createActContextParameters) as IActContext;
			}
			finally
			{
				if (createActContextParametersSource.Data != IntPtr.Zero)
				{
					IsolationInterop.CreateActContextParametersSourceDefinitionAppid.Destroy(createActContextParametersSource.Data);
					createActContextParametersSource.Data = IntPtr.Zero;
				}
				if (createActContextParameters.Source != IntPtr.Zero)
				{
					IsolationInterop.CreateActContextParametersSource.Destroy(createActContextParameters.Source);
					createActContextParameters.Source = IntPtr.Zero;
				}
			}
			return actContext;
		}

		// Token: 0x060015BD RID: 5565
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object CreateActContext(ref IsolationInterop.CreateActContextParameters Params);

		// Token: 0x060015BE RID: 5566
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object CreateCMSFromXml([In] byte[] buffer, [In] uint bufferSize, [In] IManifestParseErrorCallback Callback, [In] ref Guid riid);

		// Token: 0x060015BF RID: 5567
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object ParseManifest([MarshalAs(UnmanagedType.LPWStr)] [In] string pszManifestPath, [In] IManifestParseErrorCallback pIManifestParseErrorCallback, [In] ref Guid riid);

		// Token: 0x060015C0 RID: 5568
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		private static extern object GetUserStore([In] uint Flags, [In] IntPtr hToken, [In] ref Guid riid);

		// Token: 0x060015C1 RID: 5569
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		private static extern IIdentityAuthority GetIdentityAuthority();

		// Token: 0x060015C2 RID: 5570
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		private static extern IAppIdAuthority GetAppIdAuthority();

		// Token: 0x060015C3 RID: 5571 RVA: 0x00038038 File Offset: 0x00037038
		internal static Guid GetGuidOfType(Type type)
		{
			GuidAttribute guidAttribute = (GuidAttribute)Attribute.GetCustomAttribute(type, typeof(GuidAttribute), false);
			return new Guid(guidAttribute.Value);
		}

		// Token: 0x040008D9 RID: 2265
		public const string IsolationDllName = "mscorwks.dll";

		// Token: 0x040008DA RID: 2266
		private static object _synchObject = new object();

		// Token: 0x040008DB RID: 2267
		private static IIdentityAuthority _idAuth = null;

		// Token: 0x040008DC RID: 2268
		private static IAppIdAuthority _appIdAuth = null;

		// Token: 0x040008DD RID: 2269
		public static Guid IID_ICMS = IsolationInterop.GetGuidOfType(typeof(ICMS));

		// Token: 0x040008DE RID: 2270
		public static Guid IID_IDefinitionIdentity = IsolationInterop.GetGuidOfType(typeof(IDefinitionIdentity));

		// Token: 0x040008DF RID: 2271
		public static Guid IID_IManifestInformation = IsolationInterop.GetGuidOfType(typeof(IManifestInformation));

		// Token: 0x040008E0 RID: 2272
		public static Guid IID_IEnumSTORE_ASSEMBLY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY));

		// Token: 0x040008E1 RID: 2273
		public static Guid IID_IEnumSTORE_ASSEMBLY_FILE = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));

		// Token: 0x040008E2 RID: 2274
		public static Guid IID_IEnumSTORE_CATEGORY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY));

		// Token: 0x040008E3 RID: 2275
		public static Guid IID_IEnumSTORE_CATEGORY_INSTANCE = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_INSTANCE));

		// Token: 0x040008E4 RID: 2276
		public static Guid IID_IEnumSTORE_DEPLOYMENT_METADATA = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_DEPLOYMENT_METADATA));

		// Token: 0x040008E5 RID: 2277
		public static Guid IID_IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY));

		// Token: 0x040008E6 RID: 2278
		public static Guid IID_IStore = IsolationInterop.GetGuidOfType(typeof(IStore));

		// Token: 0x040008E7 RID: 2279
		public static Guid GUID_SXS_INSTALL_REFERENCE_SCHEME_OPAQUESTRING = new Guid("2ec93463-b0c3-45e1-8364-327e96aea856");

		// Token: 0x040008E8 RID: 2280
		public static Guid SXS_INSTALL_REFERENCE_SCHEME_SXS_STRONGNAME_SIGNED_PRIVATE_ASSEMBLY = new Guid("3ab20ac0-67e8-4512-8385-a487e35df3da");

		// Token: 0x0200022D RID: 557
		internal struct CreateActContextParameters
		{
			// Token: 0x040008E9 RID: 2281
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x040008EA RID: 2282
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x040008EB RID: 2283
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr CustomStoreList;

			// Token: 0x040008EC RID: 2284
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr CultureFallbackList;

			// Token: 0x040008ED RID: 2285
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr ProcessorArchitectureList;

			// Token: 0x040008EE RID: 2286
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr Source;

			// Token: 0x040008EF RID: 2287
			[MarshalAs(UnmanagedType.U2)]
			public ushort ProcArch;

			// Token: 0x0200022E RID: 558
			[Flags]
			public enum CreateFlags
			{
				// Token: 0x040008F1 RID: 2289
				Nothing = 0,
				// Token: 0x040008F2 RID: 2290
				StoreListValid = 1,
				// Token: 0x040008F3 RID: 2291
				CultureListValid = 2,
				// Token: 0x040008F4 RID: 2292
				ProcessorFallbackListValid = 4,
				// Token: 0x040008F5 RID: 2293
				ProcessorValid = 8,
				// Token: 0x040008F6 RID: 2294
				SourceValid = 16,
				// Token: 0x040008F7 RID: 2295
				IgnoreVisibility = 32
			}
		}

		// Token: 0x0200022F RID: 559
		internal struct CreateActContextParametersSource
		{
			// Token: 0x060015C5 RID: 5573 RVA: 0x00038174 File Offset: 0x00037174
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x060015C6 RID: 5574 RVA: 0x000381AA File Offset: 0x000371AA
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSource));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x040008F8 RID: 2296
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x040008F9 RID: 2297
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x040008FA RID: 2298
			[MarshalAs(UnmanagedType.U4)]
			public uint SourceType;

			// Token: 0x040008FB RID: 2299
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr Data;

			// Token: 0x02000230 RID: 560
			[Flags]
			public enum SourceFlags
			{
				// Token: 0x040008FD RID: 2301
				Definition = 1,
				// Token: 0x040008FE RID: 2302
				Reference = 2
			}
		}

		// Token: 0x02000231 RID: 561
		internal struct CreateActContextParametersSourceDefinitionAppid
		{
			// Token: 0x060015C7 RID: 5575 RVA: 0x000381C4 File Offset: 0x000371C4
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x060015C8 RID: 5576 RVA: 0x000381FA File Offset: 0x000371FA
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSourceDefinitionAppid));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x040008FF RID: 2303
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x04000900 RID: 2304
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x04000901 RID: 2305
			public IDefinitionAppId AppId;
		}
	}
}
