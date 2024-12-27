using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000127 RID: 295
	internal static class IsolationInterop
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x00009088 File Offset: 0x00008088
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

		// Token: 0x06000439 RID: 1081 RVA: 0x000090EC File Offset: 0x000080EC
		public static Store GetUserStore()
		{
			return new Store(IsolationInterop.GetUserStore(0U, IntPtr.Zero, ref IsolationInterop.IID_IStore) as IStore);
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00009108 File Offset: 0x00008108
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

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00009168 File Offset: 0x00008168
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

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x000091B8 File Offset: 0x000081B8
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

		// Token: 0x0600043D RID: 1085 RVA: 0x00009208 File Offset: 0x00008208
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

		// Token: 0x0600043E RID: 1086 RVA: 0x00009358 File Offset: 0x00008358
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

		// Token: 0x0600043F RID: 1087
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object CreateActContext(ref IsolationInterop.CreateActContextParameters Params);

		// Token: 0x06000440 RID: 1088
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object CreateCMSFromXml([In] byte[] buffer, [In] uint bufferSize, [In] IManifestParseErrorCallback Callback, [In] ref Guid riid);

		// Token: 0x06000441 RID: 1089
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object ParseManifest([MarshalAs(UnmanagedType.LPWStr)] [In] string pszManifestPath, [In] IManifestParseErrorCallback pIManifestParseErrorCallback, [In] ref Guid riid);

		// Token: 0x06000442 RID: 1090
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		private static extern object GetUserStore([In] uint Flags, [In] IntPtr hToken, [In] ref Guid riid);

		// Token: 0x06000443 RID: 1091
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		private static extern object GetSystemStore([In] uint Flags, [In] ref Guid riid);

		// Token: 0x06000444 RID: 1092
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		private static extern IIdentityAuthority GetIdentityAuthority();

		// Token: 0x06000445 RID: 1093
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		private static extern IAppIdAuthority GetAppIdAuthority();

		// Token: 0x06000446 RID: 1094
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object GetUserStateManager([In] uint Flags, [In] IntPtr hToken, [In] ref Guid riid);

		// Token: 0x06000447 RID: 1095 RVA: 0x000094A8 File Offset: 0x000084A8
		internal static Guid GetGuidOfType(Type type)
		{
			GuidAttribute guidAttribute = (GuidAttribute)Attribute.GetCustomAttribute(type, typeof(GuidAttribute), false);
			return new Guid(guidAttribute.Value);
		}

		// Token: 0x04000E4D RID: 3661
		public const string IsolationDllName = "mscorwks.dll";

		// Token: 0x04000E4E RID: 3662
		private static object _synchObject = new object();

		// Token: 0x04000E4F RID: 3663
		private static Store _userStore = null;

		// Token: 0x04000E50 RID: 3664
		private static Store _systemStore = null;

		// Token: 0x04000E51 RID: 3665
		private static IIdentityAuthority _idAuth = null;

		// Token: 0x04000E52 RID: 3666
		private static IAppIdAuthority _appIdAuth = null;

		// Token: 0x04000E53 RID: 3667
		public static Guid IID_ICMS = IsolationInterop.GetGuidOfType(typeof(ICMS));

		// Token: 0x04000E54 RID: 3668
		public static Guid IID_IDefinitionIdentity = IsolationInterop.GetGuidOfType(typeof(IDefinitionIdentity));

		// Token: 0x04000E55 RID: 3669
		public static Guid IID_IManifestInformation = IsolationInterop.GetGuidOfType(typeof(IManifestInformation));

		// Token: 0x04000E56 RID: 3670
		public static Guid IID_IEnumSTORE_ASSEMBLY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY));

		// Token: 0x04000E57 RID: 3671
		public static Guid IID_IEnumSTORE_ASSEMBLY_FILE = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));

		// Token: 0x04000E58 RID: 3672
		public static Guid IID_IEnumSTORE_CATEGORY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY));

		// Token: 0x04000E59 RID: 3673
		public static Guid IID_IEnumSTORE_CATEGORY_INSTANCE = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_INSTANCE));

		// Token: 0x04000E5A RID: 3674
		public static Guid IID_IEnumSTORE_DEPLOYMENT_METADATA = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_DEPLOYMENT_METADATA));

		// Token: 0x04000E5B RID: 3675
		public static Guid IID_IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY));

		// Token: 0x04000E5C RID: 3676
		public static Guid IID_IStore = IsolationInterop.GetGuidOfType(typeof(IStore));

		// Token: 0x04000E5D RID: 3677
		public static Guid GUID_SXS_INSTALL_REFERENCE_SCHEME_OPAQUESTRING = new Guid("2ec93463-b0c3-45e1-8364-327e96aea856");

		// Token: 0x04000E5E RID: 3678
		public static Guid SXS_INSTALL_REFERENCE_SCHEME_SXS_STRONGNAME_SIGNED_PRIVATE_ASSEMBLY = new Guid("3ab20ac0-67e8-4512-8385-a487e35df3da");

		// Token: 0x02000128 RID: 296
		internal struct CreateActContextParameters
		{
			// Token: 0x04000E5F RID: 3679
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x04000E60 RID: 3680
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x04000E61 RID: 3681
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr CustomStoreList;

			// Token: 0x04000E62 RID: 3682
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr CultureFallbackList;

			// Token: 0x04000E63 RID: 3683
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr ProcessorArchitectureList;

			// Token: 0x04000E64 RID: 3684
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr Source;

			// Token: 0x04000E65 RID: 3685
			[MarshalAs(UnmanagedType.U2)]
			public ushort ProcArch;

			// Token: 0x02000129 RID: 297
			[Flags]
			public enum CreateFlags
			{
				// Token: 0x04000E67 RID: 3687
				Nothing = 0,
				// Token: 0x04000E68 RID: 3688
				StoreListValid = 1,
				// Token: 0x04000E69 RID: 3689
				CultureListValid = 2,
				// Token: 0x04000E6A RID: 3690
				ProcessorFallbackListValid = 4,
				// Token: 0x04000E6B RID: 3691
				ProcessorValid = 8,
				// Token: 0x04000E6C RID: 3692
				SourceValid = 16,
				// Token: 0x04000E6D RID: 3693
				IgnoreVisibility = 32
			}
		}

		// Token: 0x0200012A RID: 298
		internal struct CreateActContextParametersSource
		{
			// Token: 0x06000449 RID: 1097 RVA: 0x000095F0 File Offset: 0x000085F0
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x0600044A RID: 1098 RVA: 0x00009626 File Offset: 0x00008626
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSource));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x04000E6E RID: 3694
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x04000E6F RID: 3695
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x04000E70 RID: 3696
			[MarshalAs(UnmanagedType.U4)]
			public uint SourceType;

			// Token: 0x04000E71 RID: 3697
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr Data;

			// Token: 0x0200012B RID: 299
			[Flags]
			public enum SourceFlags
			{
				// Token: 0x04000E73 RID: 3699
				Definition = 1,
				// Token: 0x04000E74 RID: 3700
				Reference = 2
			}
		}

		// Token: 0x0200012C RID: 300
		internal struct CreateActContextParametersSourceReferenceAppid
		{
			// Token: 0x0600044B RID: 1099 RVA: 0x00009640 File Offset: 0x00008640
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x0600044C RID: 1100 RVA: 0x00009676 File Offset: 0x00008676
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSourceReferenceAppid));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x04000E75 RID: 3701
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x04000E76 RID: 3702
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x04000E77 RID: 3703
			public IReferenceAppId AppId;
		}

		// Token: 0x0200012D RID: 301
		internal struct CreateActContextParametersSourceDefinitionAppid
		{
			// Token: 0x0600044D RID: 1101 RVA: 0x00009690 File Offset: 0x00008690
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x0600044E RID: 1102 RVA: 0x000096C6 File Offset: 0x000086C6
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSourceDefinitionAppid));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x04000E78 RID: 3704
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x04000E79 RID: 3705
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x04000E7A RID: 3706
			public IDefinitionAppId AppId;
		}
	}
}
