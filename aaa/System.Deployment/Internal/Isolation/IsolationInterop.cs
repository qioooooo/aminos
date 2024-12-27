using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000150 RID: 336
	internal static class IsolationInterop
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x000202AC File Offset: 0x0001F2AC
		public static Store UserStore
		{
			get
			{
				if (IsolationInterop._userStore == null)
				{
					lock (IsolationInterop._synchObject)
					{
						if (IsolationInterop._userStore == null)
						{
							IsolationInterop._userStore = new Store(IsolationInterop.GetUserStore(0U, IntPtr.Zero, ref IsolationInterop.IID_IStore) as IStore);
						}
					}
				}
				return IsolationInterop._userStore;
			}
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00020310 File Offset: 0x0001F310
		public static Store GetUserStore()
		{
			return new Store(IsolationInterop.GetUserStore(0U, IntPtr.Zero, ref IsolationInterop.IID_IStore) as IStore);
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x0002032C File Offset: 0x0001F32C
		public static Store SystemStore
		{
			get
			{
				if (IsolationInterop._systemStore == null)
				{
					lock (IsolationInterop._synchObject)
					{
						if (IsolationInterop._systemStore == null)
						{
							IsolationInterop._systemStore = new Store(IsolationInterop.GetSystemStore(0U, ref IsolationInterop.IID_IStore) as IStore);
						}
					}
				}
				return IsolationInterop._systemStore;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0002038C File Offset: 0x0001F38C
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

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x000203DC File Offset: 0x0001F3DC
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

		// Token: 0x06000717 RID: 1815 RVA: 0x0002042C File Offset: 0x0001F42C
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

		// Token: 0x06000718 RID: 1816 RVA: 0x0002057C File Offset: 0x0001F57C
		internal static IActContext CreateActContext(IReferenceAppId AppId)
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
			createActContextParametersSource.SourceType = 2U;
			createActContextParametersSource.Data = IntPtr.Zero;
			IsolationInterop.CreateActContextParametersSourceReferenceAppid createActContextParametersSourceReferenceAppid;
			createActContextParametersSourceReferenceAppid.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParametersSourceReferenceAppid));
			createActContextParametersSourceReferenceAppid.Flags = 0U;
			createActContextParametersSourceReferenceAppid.AppId = AppId;
			IActContext actContext;
			try
			{
				createActContextParametersSource.Data = createActContextParametersSourceReferenceAppid.ToIntPtr();
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

		// Token: 0x06000719 RID: 1817
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object CreateActContext(ref IsolationInterop.CreateActContextParameters Params);

		// Token: 0x0600071A RID: 1818
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object CreateCMSFromXml([In] byte[] buffer, [In] uint bufferSize, [In] IManifestParseErrorCallback Callback, [In] ref Guid riid);

		// Token: 0x0600071B RID: 1819
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object ParseManifest([MarshalAs(UnmanagedType.LPWStr)] [In] string pszManifestPath, [In] IManifestParseErrorCallback pIManifestParseErrorCallback, [In] ref Guid riid);

		// Token: 0x0600071C RID: 1820
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		private static extern object GetUserStore([In] uint Flags, [In] IntPtr hToken, [In] ref Guid riid);

		// Token: 0x0600071D RID: 1821
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		private static extern object GetSystemStore([In] uint Flags, [In] ref Guid riid);

		// Token: 0x0600071E RID: 1822
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		private static extern IIdentityAuthority GetIdentityAuthority();

		// Token: 0x0600071F RID: 1823
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		private static extern IAppIdAuthority GetAppIdAuthority();

		// Token: 0x06000720 RID: 1824
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object GetUserStateManager([In] uint Flags, [In] IntPtr hToken, [In] ref Guid riid);

		// Token: 0x06000721 RID: 1825 RVA: 0x000206CC File Offset: 0x0001F6CC
		internal static Guid GetGuidOfType(Type type)
		{
			GuidAttribute guidAttribute = (GuidAttribute)Attribute.GetCustomAttribute(type, typeof(GuidAttribute), false);
			return new Guid(guidAttribute.Value);
		}

		// Token: 0x040005BF RID: 1471
		public const string IsolationDllName = "mscorwks.dll";

		// Token: 0x040005C0 RID: 1472
		private static object _synchObject = new object();

		// Token: 0x040005C1 RID: 1473
		private static Store _userStore = null;

		// Token: 0x040005C2 RID: 1474
		private static Store _systemStore = null;

		// Token: 0x040005C3 RID: 1475
		private static IIdentityAuthority _idAuth = null;

		// Token: 0x040005C4 RID: 1476
		private static IAppIdAuthority _appIdAuth = null;

		// Token: 0x040005C5 RID: 1477
		public static Guid IID_ICMS = IsolationInterop.GetGuidOfType(typeof(ICMS));

		// Token: 0x040005C6 RID: 1478
		public static Guid IID_IDefinitionIdentity = IsolationInterop.GetGuidOfType(typeof(IDefinitionIdentity));

		// Token: 0x040005C7 RID: 1479
		public static Guid IID_IManifestInformation = IsolationInterop.GetGuidOfType(typeof(IManifestInformation));

		// Token: 0x040005C8 RID: 1480
		public static Guid IID_IEnumSTORE_ASSEMBLY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY));

		// Token: 0x040005C9 RID: 1481
		public static Guid IID_IEnumSTORE_ASSEMBLY_FILE = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));

		// Token: 0x040005CA RID: 1482
		public static Guid IID_IEnumSTORE_CATEGORY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY));

		// Token: 0x040005CB RID: 1483
		public static Guid IID_IEnumSTORE_CATEGORY_INSTANCE = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_INSTANCE));

		// Token: 0x040005CC RID: 1484
		public static Guid IID_IEnumSTORE_DEPLOYMENT_METADATA = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_DEPLOYMENT_METADATA));

		// Token: 0x040005CD RID: 1485
		public static Guid IID_IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY));

		// Token: 0x040005CE RID: 1486
		public static Guid IID_IStore = IsolationInterop.GetGuidOfType(typeof(IStore));

		// Token: 0x040005CF RID: 1487
		public static Guid GUID_SXS_INSTALL_REFERENCE_SCHEME_OPAQUESTRING = new Guid("2ec93463-b0c3-45e1-8364-327e96aea856");

		// Token: 0x040005D0 RID: 1488
		public static Guid SXS_INSTALL_REFERENCE_SCHEME_SXS_STRONGNAME_SIGNED_PRIVATE_ASSEMBLY = new Guid("3ab20ac0-67e8-4512-8385-a487e35df3da");

		// Token: 0x02000151 RID: 337
		internal struct CreateActContextParameters
		{
			// Token: 0x040005D1 RID: 1489
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x040005D2 RID: 1490
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x040005D3 RID: 1491
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr CustomStoreList;

			// Token: 0x040005D4 RID: 1492
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr CultureFallbackList;

			// Token: 0x040005D5 RID: 1493
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr ProcessorArchitectureList;

			// Token: 0x040005D6 RID: 1494
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr Source;

			// Token: 0x040005D7 RID: 1495
			[MarshalAs(UnmanagedType.U2)]
			public ushort ProcArch;

			// Token: 0x02000152 RID: 338
			[Flags]
			public enum CreateFlags
			{
				// Token: 0x040005D9 RID: 1497
				Nothing = 0,
				// Token: 0x040005DA RID: 1498
				StoreListValid = 1,
				// Token: 0x040005DB RID: 1499
				CultureListValid = 2,
				// Token: 0x040005DC RID: 1500
				ProcessorFallbackListValid = 4,
				// Token: 0x040005DD RID: 1501
				ProcessorValid = 8,
				// Token: 0x040005DE RID: 1502
				SourceValid = 16,
				// Token: 0x040005DF RID: 1503
				IgnoreVisibility = 32
			}
		}

		// Token: 0x02000153 RID: 339
		internal struct CreateActContextParametersSource
		{
			// Token: 0x06000723 RID: 1827 RVA: 0x00020814 File Offset: 0x0001F814
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x06000724 RID: 1828 RVA: 0x0002084A File Offset: 0x0001F84A
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSource));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x040005E0 RID: 1504
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x040005E1 RID: 1505
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x040005E2 RID: 1506
			[MarshalAs(UnmanagedType.U4)]
			public uint SourceType;

			// Token: 0x040005E3 RID: 1507
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr Data;

			// Token: 0x02000154 RID: 340
			[Flags]
			public enum SourceFlags
			{
				// Token: 0x040005E5 RID: 1509
				Definition = 1,
				// Token: 0x040005E6 RID: 1510
				Reference = 2
			}
		}

		// Token: 0x02000155 RID: 341
		internal struct CreateActContextParametersSourceReferenceAppid
		{
			// Token: 0x06000725 RID: 1829 RVA: 0x00020864 File Offset: 0x0001F864
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x06000726 RID: 1830 RVA: 0x0002089A File Offset: 0x0001F89A
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSourceReferenceAppid));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x040005E7 RID: 1511
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x040005E8 RID: 1512
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x040005E9 RID: 1513
			public IReferenceAppId AppId;
		}

		// Token: 0x02000156 RID: 342
		internal struct CreateActContextParametersSourceDefinitionAppid
		{
			// Token: 0x06000727 RID: 1831 RVA: 0x000208B4 File Offset: 0x0001F8B4
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x06000728 RID: 1832 RVA: 0x000208EA File Offset: 0x0001F8EA
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSourceDefinitionAppid));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x040005EA RID: 1514
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x040005EB RID: 1515
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x040005EC RID: 1516
			public IDefinitionAppId AppId;
		}
	}
}
