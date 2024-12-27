using System;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Configuration.Internal
{
	// Token: 0x020000C2 RID: 194
	internal sealed class InternalConfigHost : IInternalConfigHost
	{
		// Token: 0x06000725 RID: 1829 RVA: 0x0001F914 File Offset: 0x0001E914
		internal InternalConfigHost()
		{
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001F91C File Offset: 0x0001E91C
		void IInternalConfigHost.Init(IInternalConfigRoot configRoot, params object[] hostInitParams)
		{
			this._configRoot = configRoot;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0001F925 File Offset: 0x0001E925
		void IInternalConfigHost.InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams)
		{
			this._configRoot = configRoot;
			configPath = null;
			locationConfigPath = null;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0001F935 File Offset: 0x0001E935
		bool IInternalConfigHost.IsConfigRecordRequired(string configPath)
		{
			return true;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001F938 File Offset: 0x0001E938
		bool IInternalConfigHost.IsInitDelayed(IInternalConfigRecord configRecord)
		{
			return false;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001F93B File Offset: 0x0001E93B
		void IInternalConfigHost.RequireCompleteInit(IInternalConfigRecord configRecord)
		{
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001F93D File Offset: 0x0001E93D
		public bool IsSecondaryRoot(string configPath)
		{
			return false;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001F940 File Offset: 0x0001E940
		string IInternalConfigHost.GetStreamName(string configPath)
		{
			throw ExceptionUtil.UnexpectedError("IInternalConfigHost.GetStreamName");
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001F94C File Offset: 0x0001E94C
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		internal static string StaticGetStreamNameForConfigSource(string streamName, string configSource)
		{
			if (!Path.IsPathRooted(streamName))
			{
				throw ExceptionUtil.ParameterInvalid("streamName");
			}
			streamName = Path.GetFullPath(streamName);
			string directoryOrRootName = UrlPath.GetDirectoryOrRootName(streamName);
			string text = Path.Combine(directoryOrRootName, configSource);
			text = Path.GetFullPath(text);
			string directoryOrRootName2 = UrlPath.GetDirectoryOrRootName(text);
			if (!UrlPath.IsEqualOrSubdirectory(directoryOrRootName, directoryOrRootName2))
			{
				throw new ArgumentException(SR.GetString("Config_source_not_under_config_dir", new object[] { configSource }));
			}
			return text;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001F9B7 File Offset: 0x0001E9B7
		string IInternalConfigHost.GetStreamNameForConfigSource(string streamName, string configSource)
		{
			return InternalConfigHost.StaticGetStreamNameForConfigSource(streamName, configSource);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001F9C0 File Offset: 0x0001E9C0
		internal static object StaticGetStreamVersion(string streamName)
		{
			bool flag = false;
			long num = 0L;
			DateTime dateTime = DateTime.MinValue;
			DateTime dateTime2 = DateTime.MinValue;
			UnsafeNativeMethods.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA;
			if (UnsafeNativeMethods.GetFileAttributesEx(streamName, 0, out win32_FILE_ATTRIBUTE_DATA) && (win32_FILE_ATTRIBUTE_DATA.fileAttributes & 16) == 0)
			{
				flag = true;
				num = (long)(((ulong)win32_FILE_ATTRIBUTE_DATA.fileSizeHigh << 32) | (ulong)win32_FILE_ATTRIBUTE_DATA.fileSizeLow);
				dateTime = DateTime.FromFileTimeUtc((long)(((ulong)win32_FILE_ATTRIBUTE_DATA.ftCreationTimeHigh << 32) | (ulong)win32_FILE_ATTRIBUTE_DATA.ftCreationTimeLow));
				dateTime2 = DateTime.FromFileTimeUtc((long)(((ulong)win32_FILE_ATTRIBUTE_DATA.ftLastWriteTimeHigh << 32) | (ulong)win32_FILE_ATTRIBUTE_DATA.ftLastWriteTimeLow));
			}
			return new FileVersion(flag, num, dateTime, dateTime2);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001FA49 File Offset: 0x0001EA49
		object IInternalConfigHost.GetStreamVersion(string streamName)
		{
			return InternalConfigHost.StaticGetStreamVersion(streamName);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001FA51 File Offset: 0x0001EA51
		internal static Stream StaticOpenStreamForRead(string streamName)
		{
			if (string.IsNullOrEmpty(streamName))
			{
				throw ExceptionUtil.UnexpectedError("InternalConfigHost::StaticOpenStreamForRead");
			}
			if (!FileUtil.FileExists(streamName, true))
			{
				return null;
			}
			return new FileStream(streamName, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001FA7A File Offset: 0x0001EA7A
		Stream IInternalConfigHost.OpenStreamForRead(string streamName)
		{
			return ((IInternalConfigHost)this).OpenStreamForRead(streamName, false);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001FA84 File Offset: 0x0001EA84
		Stream IInternalConfigHost.OpenStreamForRead(string streamName, bool assertPermissions)
		{
			Stream stream = null;
			bool flag = false;
			if (assertPermissions || !this._configRoot.IsDesignTime)
			{
				new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, streamName).Assert();
				flag = true;
			}
			try
			{
				stream = InternalConfigHost.StaticOpenStreamForRead(streamName);
			}
			finally
			{
				if (flag)
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			return stream;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001FAD8 File Offset: 0x0001EAD8
		internal static Stream StaticOpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext, bool assertPermissions)
		{
			bool flag = false;
			if (string.IsNullOrEmpty(streamName))
			{
				throw new ConfigurationException(SR.GetString("Config_no_stream_to_write"));
			}
			string directoryName = Path.GetDirectoryName(streamName);
			try
			{
				if (!Directory.Exists(directoryName))
				{
					if (assertPermissions)
					{
						new FileIOPermission(PermissionState.Unrestricted).Assert();
						flag = true;
					}
					Directory.CreateDirectory(directoryName);
				}
			}
			catch
			{
			}
			finally
			{
				if (flag)
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			WriteFileContext writeFileContext = null;
			flag = false;
			if (assertPermissions)
			{
				new FileIOPermission(FileIOPermissionAccess.AllAccess, directoryName).Assert();
				flag = true;
			}
			Stream stream;
			try
			{
				writeFileContext = new WriteFileContext(streamName, templateStreamName);
				if (File.Exists(streamName))
				{
					FileInfo fileInfo = new FileInfo(streamName);
					FileAttributes attributes = fileInfo.Attributes;
					if ((attributes & (FileAttributes.ReadOnly | FileAttributes.Hidden)) != (FileAttributes)0)
					{
						throw new IOException(SR.GetString("Config_invalid_attributes_for_write", new object[] { streamName }));
					}
				}
				try
				{
					stream = new FileStream(writeFileContext.TempNewFilename, FileMode.Create, FileAccess.Write, FileShare.Read);
				}
				catch (Exception ex)
				{
					throw new ConfigurationException(SR.GetString("Config_write_failed", new object[] { streamName }), ex);
				}
				catch
				{
					throw new ConfigurationException(SR.GetString("Config_write_failed", new object[] { streamName }));
				}
			}
			catch
			{
				if (writeFileContext != null)
				{
					writeFileContext.Complete(streamName, false);
				}
				throw;
			}
			finally
			{
				if (flag)
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			writeContext = writeFileContext;
			return stream;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001FC50 File Offset: 0x0001EC50
		Stream IInternalConfigHost.OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext)
		{
			return ((IInternalConfigHost)this).OpenStreamForWrite(streamName, templateStreamName, ref writeContext, false);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001FC5C File Offset: 0x0001EC5C
		Stream IInternalConfigHost.OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext, bool assertPermissions)
		{
			return InternalConfigHost.StaticOpenStreamForWrite(streamName, templateStreamName, ref writeContext, assertPermissions);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001FC68 File Offset: 0x0001EC68
		internal static void StaticWriteCompleted(string streamName, bool success, object writeContext, bool assertPermissions)
		{
			WriteFileContext writeFileContext = (WriteFileContext)writeContext;
			bool flag = false;
			if (assertPermissions)
			{
				string directoryName = Path.GetDirectoryName(streamName);
				string[] array = new string[] { streamName, writeFileContext.TempNewFilename, directoryName };
				FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.View | AccessControlActions.Change, array);
				fileIOPermission.Assert();
				flag = true;
			}
			try
			{
				writeFileContext.Complete(streamName, success);
			}
			finally
			{
				if (flag)
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001FCE0 File Offset: 0x0001ECE0
		void IInternalConfigHost.WriteCompleted(string streamName, bool success, object writeContext)
		{
			((IInternalConfigHost)this).WriteCompleted(streamName, success, writeContext, false);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001FCEC File Offset: 0x0001ECEC
		void IInternalConfigHost.WriteCompleted(string streamName, bool success, object writeContext, bool assertPermissions)
		{
			InternalConfigHost.StaticWriteCompleted(streamName, success, writeContext, assertPermissions);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001FCF8 File Offset: 0x0001ECF8
		internal static void StaticDeleteStream(string streamName)
		{
			File.Delete(streamName);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001FD00 File Offset: 0x0001ED00
		void IInternalConfigHost.DeleteStream(string streamName)
		{
			InternalConfigHost.StaticDeleteStream(streamName);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001FD08 File Offset: 0x0001ED08
		internal static bool StaticIsFile(string streamName)
		{
			return Path.IsPathRooted(streamName);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001FD10 File Offset: 0x0001ED10
		bool IInternalConfigHost.IsFile(string streamName)
		{
			return InternalConfigHost.StaticIsFile(streamName);
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001FD18 File Offset: 0x0001ED18
		bool IInternalConfigHost.SupportsChangeNotifications
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0001FD1B File Offset: 0x0001ED1B
		object IInternalConfigHost.StartMonitoringStreamForChanges(string streamName, StreamChangeCallback callback)
		{
			throw ExceptionUtil.UnexpectedError("IInternalConfigHost.StartMonitoringStreamForChanges");
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001FD27 File Offset: 0x0001ED27
		void IInternalConfigHost.StopMonitoringStreamForChanges(string streamName, StreamChangeCallback callback)
		{
			throw ExceptionUtil.UnexpectedError("IInternalConfigHost.StopMonitoringStreamForChanges");
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0001FD33 File Offset: 0x0001ED33
		bool IInternalConfigHost.SupportsRefresh
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0001FD36 File Offset: 0x0001ED36
		bool IInternalConfigHost.SupportsPath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001FD39 File Offset: 0x0001ED39
		bool IInternalConfigHost.IsDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition)
		{
			return true;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001FD3C File Offset: 0x0001ED3C
		void IInternalConfigHost.VerifyDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, IConfigErrorInfo errorInfo)
		{
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x0001FD3E File Offset: 0x0001ED3E
		bool IInternalConfigHost.SupportsLocation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0001FD41 File Offset: 0x0001ED41
		bool IInternalConfigHost.IsAboveApplication(string configPath)
		{
			throw ExceptionUtil.UnexpectedError("IInternalConfigHost.IsAboveApplication");
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001FD4D File Offset: 0x0001ED4D
		string IInternalConfigHost.GetConfigPathFromLocationSubPath(string configPath, string locationSubPath)
		{
			throw ExceptionUtil.UnexpectedError("IInternalConfigHost.GetConfigPathFromLocationSubPath");
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001FD59 File Offset: 0x0001ED59
		bool IInternalConfigHost.IsLocationApplicable(string configPath)
		{
			throw ExceptionUtil.UnexpectedError("IInternalConfigHost.IsLocationApplicable");
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001FD65 File Offset: 0x0001ED65
		bool IInternalConfigHost.IsTrustedConfigPath(string configPath)
		{
			throw ExceptionUtil.UnexpectedError("IInternalConfigHost.IsTrustedConfigPath");
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001FD71 File Offset: 0x0001ED71
		bool IInternalConfigHost.IsFullTrustSectionWithoutAptcaAllowed(IInternalConfigRecord configRecord)
		{
			return TypeUtil.IsCallerFullTrust;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001FD78 File Offset: 0x0001ED78
		void IInternalConfigHost.GetRestrictedPermissions(IInternalConfigRecord configRecord, out PermissionSet permissionSet, out bool isHostReady)
		{
			permissionSet = null;
			isHostReady = true;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001FD80 File Offset: 0x0001ED80
		IDisposable IInternalConfigHost.Impersonate()
		{
			return null;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001FD83 File Offset: 0x0001ED83
		bool IInternalConfigHost.PrefetchAll(string configPath, string streamName)
		{
			return false;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0001FD86 File Offset: 0x0001ED86
		bool IInternalConfigHost.PrefetchSection(string sectionGroupName, string sectionName)
		{
			return false;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0001FD89 File Offset: 0x0001ED89
		object IInternalConfigHost.CreateDeprecatedConfigContext(string configPath)
		{
			throw ExceptionUtil.UnexpectedError("IInternalConfigHost.CreateDeprecatedConfigContext");
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0001FD95 File Offset: 0x0001ED95
		object IInternalConfigHost.CreateConfigurationContext(string configPath, string locationSubPath)
		{
			throw ExceptionUtil.UnexpectedError("IInternalConfigHost.CreateConfigurationContext");
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001FDA1 File Offset: 0x0001EDA1
		string IInternalConfigHost.DecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection)
		{
			return ProtectedConfigurationSection.DecryptSection(encryptedXml, protectionProvider);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001FDAA File Offset: 0x0001EDAA
		string IInternalConfigHost.EncryptSection(string clearTextXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection)
		{
			return ProtectedConfigurationSection.EncryptSection(clearTextXml, protectionProvider);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001FDB3 File Offset: 0x0001EDB3
		Type IInternalConfigHost.GetConfigType(string typeName, bool throwOnError)
		{
			return Type.GetType(typeName, throwOnError);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001FDBC File Offset: 0x0001EDBC
		string IInternalConfigHost.GetConfigTypeName(Type t)
		{
			return t.AssemblyQualifiedName;
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0001FDC4 File Offset: 0x0001EDC4
		bool IInternalConfigHost.IsRemote
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000421 RID: 1057
		private const FileAttributes InvalidAttributesForWrite = FileAttributes.ReadOnly | FileAttributes.Hidden;

		// Token: 0x04000422 RID: 1058
		private IInternalConfigRoot _configRoot;
	}
}
