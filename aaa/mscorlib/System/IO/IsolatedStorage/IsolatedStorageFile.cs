using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO.IsolatedStorage
{
	// Token: 0x02000796 RID: 1942
	[ComVisible(true)]
	public sealed class IsolatedStorageFile : IsolatedStorage, IDisposable
	{
		// Token: 0x0600459D RID: 17821 RVA: 0x000EDE31 File Offset: 0x000ECE31
		internal IsolatedStorageFile()
		{
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x000EDE39 File Offset: 0x000ECE39
		public static IsolatedStorageFile GetUserStoreForDomain()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x000EDE43 File Offset: 0x000ECE43
		public static IsolatedStorageFile GetUserStoreForAssembly()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x000EDE4D File Offset: 0x000ECE4D
		public static IsolatedStorageFile GetUserStoreForApplication()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Application, null);
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x000EDE57 File Offset: 0x000ECE57
		public static IsolatedStorageFile GetMachineStoreForDomain()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine, null, null);
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x000EDE62 File Offset: 0x000ECE62
		public static IsolatedStorageFile GetMachineStoreForAssembly()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine, null, null);
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x000EDE6D File Offset: 0x000ECE6D
		public static IsolatedStorageFile GetMachineStoreForApplication()
		{
			return IsolatedStorageFile.GetStore(IsolatedStorageScope.Machine | IsolatedStorageScope.Application, null);
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x000EDE78 File Offset: 0x000ECE78
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type domainEvidenceType, Type assemblyEvidenceType)
		{
			if (domainEvidenceType != null)
			{
				IsolatedStorageFile.DemandAdminPermission();
			}
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile();
			isolatedStorageFile.InitStore(scope, domainEvidenceType, assemblyEvidenceType);
			isolatedStorageFile.Init(scope);
			return isolatedStorageFile;
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x000EDEA4 File Offset: 0x000ECEA4
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object domainIdentity, object assemblyIdentity)
		{
			if (IsolatedStorage.IsDomain(scope) && domainIdentity == null)
			{
				throw new ArgumentNullException("domainIdentity");
			}
			if (assemblyIdentity == null)
			{
				throw new ArgumentNullException("assemblyIdentity");
			}
			IsolatedStorageFile.DemandAdminPermission();
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile();
			isolatedStorageFile.InitStore(scope, domainIdentity, assemblyIdentity, null);
			isolatedStorageFile.Init(scope);
			return isolatedStorageFile;
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x000EDEF4 File Offset: 0x000ECEF4
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Evidence domainEvidence, Type domainEvidenceType, Evidence assemblyEvidence, Type assemblyEvidenceType)
		{
			if (IsolatedStorage.IsDomain(scope) && domainEvidence == null)
			{
				throw new ArgumentNullException("domainEvidence");
			}
			if (assemblyEvidence == null)
			{
				throw new ArgumentNullException("assemblyEvidence");
			}
			IsolatedStorageFile.DemandAdminPermission();
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile();
			isolatedStorageFile.InitStore(scope, domainEvidence, domainEvidenceType, assemblyEvidence, assemblyEvidenceType, null, null);
			isolatedStorageFile.Init(scope);
			return isolatedStorageFile;
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x000EDF48 File Offset: 0x000ECF48
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type applicationEvidenceType)
		{
			if (applicationEvidenceType != null)
			{
				IsolatedStorageFile.DemandAdminPermission();
			}
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile();
			isolatedStorageFile.InitStore(scope, applicationEvidenceType);
			isolatedStorageFile.Init(scope);
			return isolatedStorageFile;
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x000EDF74 File Offset: 0x000ECF74
		public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object applicationIdentity)
		{
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			IsolatedStorageFile.DemandAdminPermission();
			IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile();
			isolatedStorageFile.InitStore(scope, null, null, applicationIdentity);
			isolatedStorageFile.Init(scope);
			return isolatedStorageFile;
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x060045A9 RID: 17833 RVA: 0x000EDFAC File Offset: 0x000ECFAC
		[CLSCompliant(false)]
		public override ulong CurrentSize
		{
			get
			{
				if (base.IsRoaming())
				{
					throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_CurrentSizeUndefined"));
				}
				ulong num;
				lock (this)
				{
					if (this.m_bDisposed)
					{
						throw new ObjectDisposedException(null, Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
					}
					if (this.m_closed)
					{
						throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
					}
					if (this.m_handle == Win32Native.NULL)
					{
						this.m_handle = IsolatedStorageFile.nOpen(this.m_InfoFile, this.GetSyncObjectName());
					}
					num = IsolatedStorageFile.nGetUsage(this.m_handle);
				}
				return num;
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x060045AA RID: 17834 RVA: 0x000EE05C File Offset: 0x000ED05C
		[CLSCompliant(false)]
		public override ulong MaximumSize
		{
			get
			{
				if (base.IsRoaming())
				{
					return 9223372036854775807UL;
				}
				return base.MaximumSize;
			}
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x000EE078 File Offset: 0x000ED078
		internal unsafe void Reserve(ulong lReserve)
		{
			if (base.IsRoaming())
			{
				return;
			}
			ulong maximumSize = this.MaximumSize;
			ulong num = lReserve;
			lock (this)
			{
				if (this.m_bDisposed)
				{
					throw new ObjectDisposedException(null, Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_closed)
				{
					throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_handle == Win32Native.NULL)
				{
					this.m_handle = IsolatedStorageFile.nOpen(this.m_InfoFile, this.GetSyncObjectName());
				}
				IsolatedStorageFile.nReserve(this.m_handle, &maximumSize, &num, false);
			}
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x000EE124 File Offset: 0x000ED124
		internal unsafe void Unreserve(ulong lFree)
		{
			if (base.IsRoaming())
			{
				return;
			}
			ulong maximumSize = this.MaximumSize;
			ulong num = lFree;
			lock (this)
			{
				if (this.m_bDisposed)
				{
					throw new ObjectDisposedException(null, Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_closed)
				{
					throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_handle == Win32Native.NULL)
				{
					this.m_handle = IsolatedStorageFile.nOpen(this.m_InfoFile, this.GetSyncObjectName());
				}
				IsolatedStorageFile.nReserve(this.m_handle, &maximumSize, &num, true);
			}
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x000EE1D0 File Offset: 0x000ED1D0
		public void DeleteFile(string file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			this.m_fiop.Assert();
			this.m_fiop.PermitOnly();
			FileInfo fileInfo = new FileInfo(this.GetFullPath(file));
			long num = 0L;
			this.Lock();
			try
			{
				try
				{
					num = fileInfo.Length;
					fileInfo.Delete();
				}
				catch
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DeleteFile"));
				}
				this.Unreserve(IsolatedStorageFile.RoundToBlockSize((ulong)num));
			}
			finally
			{
				this.Unlock();
			}
			CodeAccessPermission.RevertAll();
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x000EE270 File Offset: 0x000ED270
		public void CreateDirectory(string dir)
		{
			if (dir == null)
			{
				throw new ArgumentNullException("dir");
			}
			string fullPath = this.GetFullPath(dir);
			string fullPathInternal = Path.GetFullPathInternal(fullPath);
			string[] array = this.DirectoriesToCreate(fullPathInternal);
			if (array != null && array.Length != 0)
			{
				this.Reserve((ulong)(1024L * (long)array.Length));
				this.m_fiop.Assert();
				this.m_fiop.PermitOnly();
				try
				{
					Directory.CreateDirectory(array[array.Length - 1]);
				}
				catch
				{
					this.Unreserve((ulong)(1024L * (long)array.Length));
					Directory.Delete(array[0], true);
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_CreateDirectory"));
				}
				CodeAccessPermission.RevertAll();
				return;
			}
			if (Directory.Exists(fullPath))
			{
				return;
			}
			throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_CreateDirectory"));
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x000EE33C File Offset: 0x000ED33C
		private string[] DirectoriesToCreate(string fullPath)
		{
			ArrayList arrayList = new ArrayList();
			int num = fullPath.Length;
			if (num >= 2 && fullPath[num - 1] == this.SeparatorExternal)
			{
				num--;
			}
			int i = Path.GetRootLength(fullPath);
			while (i < num)
			{
				i++;
				while (i < num && fullPath[i] != this.SeparatorExternal)
				{
					i++;
				}
				string text = fullPath.Substring(0, i);
				if (!Directory.InternalExists(text))
				{
					arrayList.Add(text);
				}
			}
			if (arrayList.Count != 0)
			{
				return (string[])arrayList.ToArray(typeof(string));
			}
			return null;
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x000EE3D4 File Offset: 0x000ED3D4
		public void DeleteDirectory(string dir)
		{
			if (dir == null)
			{
				throw new ArgumentNullException("dir");
			}
			this.m_fiop.Assert();
			this.m_fiop.PermitOnly();
			this.Lock();
			try
			{
				try
				{
					new DirectoryInfo(this.GetFullPath(dir)).Delete(false);
				}
				catch
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DeleteDirectory"));
				}
				this.Unreserve(1024UL);
			}
			finally
			{
				this.Unlock();
			}
			CodeAccessPermission.RevertAll();
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x000EE468 File Offset: 0x000ED468
		public string[] GetFileNames(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			this.m_fiop.Assert();
			this.m_fiop.PermitOnly();
			string[] fileDirectoryNames = IsolatedStorageFile.GetFileDirectoryNames(this.GetFullPath(searchPattern), searchPattern, true);
			CodeAccessPermission.RevertAll();
			return fileDirectoryNames;
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x000EE4B0 File Offset: 0x000ED4B0
		public string[] GetDirectoryNames(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			this.m_fiop.Assert();
			this.m_fiop.PermitOnly();
			string[] fileDirectoryNames = IsolatedStorageFile.GetFileDirectoryNames(this.GetFullPath(searchPattern), searchPattern, false);
			CodeAccessPermission.RevertAll();
			return fileDirectoryNames;
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x000EE4F8 File Offset: 0x000ED4F8
		public override void Remove()
		{
			string text = null;
			this.RemoveLogicalDir();
			this.Close();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(IsolatedStorageFile.GetRootDir(base.Scope));
			if (base.IsApp())
			{
				stringBuilder.Append(base.AppName);
				stringBuilder.Append(this.SeparatorExternal);
			}
			else
			{
				if (base.IsDomain())
				{
					stringBuilder.Append(base.DomainName);
					stringBuilder.Append(this.SeparatorExternal);
					text = stringBuilder.ToString();
				}
				stringBuilder.Append(base.AssemName);
				stringBuilder.Append(this.SeparatorExternal);
			}
			string text2 = stringBuilder.ToString();
			new FileIOPermission(FileIOPermissionAccess.AllAccess, text2).Assert();
			if (this.ContainsUnknownFiles(text2))
			{
				return;
			}
			try
			{
				Directory.Delete(text2, true);
			}
			catch
			{
				return;
			}
			if (base.IsDomain())
			{
				CodeAccessPermission.RevertAssert();
				new FileIOPermission(FileIOPermissionAccess.AllAccess, text).Assert();
				if (!this.ContainsUnknownFiles(text))
				{
					try
					{
						Directory.Delete(text, true);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x000EE608 File Offset: 0x000ED608
		private void RemoveLogicalDir()
		{
			this.m_fiop.Assert();
			this.Lock();
			try
			{
				ulong num = (base.IsRoaming() ? 0UL : this.CurrentSize);
				try
				{
					Directory.Delete(this.RootDirectory, true);
				}
				catch
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DeleteDirectories"));
				}
				this.Unreserve(num);
			}
			finally
			{
				this.Unlock();
			}
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x000EE684 File Offset: 0x000ED684
		private bool ContainsUnknownFiles(string rootDir)
		{
			string[] fileDirectoryNames;
			string[] fileDirectoryNames2;
			try
			{
				fileDirectoryNames = IsolatedStorageFile.GetFileDirectoryNames(rootDir + "*", "*", true);
				fileDirectoryNames2 = IsolatedStorageFile.GetFileDirectoryNames(rootDir + "*", "*", false);
			}
			catch
			{
				throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DeleteDirectories"));
			}
			if (fileDirectoryNames2 != null && fileDirectoryNames2.Length > 0)
			{
				if (fileDirectoryNames2.Length > 1)
				{
					return true;
				}
				if (base.IsApp())
				{
					if (IsolatedStorageFile.NotAppFilesDir(fileDirectoryNames2[0]))
					{
						return true;
					}
				}
				else if (base.IsDomain())
				{
					if (IsolatedStorageFile.NotFilesDir(fileDirectoryNames2[0]))
					{
						return true;
					}
				}
				else if (IsolatedStorageFile.NotAssemFilesDir(fileDirectoryNames2[0]))
				{
					return true;
				}
			}
			if (fileDirectoryNames == null || fileDirectoryNames.Length == 0)
			{
				return false;
			}
			if (base.IsRoaming())
			{
				return fileDirectoryNames.Length > 1 || IsolatedStorageFile.NotIDFile(fileDirectoryNames[0]);
			}
			return fileDirectoryNames.Length > 2 || (IsolatedStorageFile.NotIDFile(fileDirectoryNames[0]) && IsolatedStorageFile.NotInfoFile(fileDirectoryNames[0])) || (fileDirectoryNames.Length == 2 && IsolatedStorageFile.NotIDFile(fileDirectoryNames[1]) && IsolatedStorageFile.NotInfoFile(fileDirectoryNames[1]));
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x000EE784 File Offset: 0x000ED784
		public void Close()
		{
			if (base.IsRoaming())
			{
				return;
			}
			lock (this)
			{
				if (!this.m_closed)
				{
					this.m_closed = true;
					IntPtr handle = this.m_handle;
					this.m_handle = Win32Native.NULL;
					IsolatedStorageFile.nClose(handle);
					GC.nativeSuppressFinalize(this);
				}
			}
		}

		// Token: 0x060045B7 RID: 17847 RVA: 0x000EE7E8 File Offset: 0x000ED7E8
		public void Dispose()
		{
			this.Close();
			this.m_bDisposed = true;
		}

		// Token: 0x060045B8 RID: 17848 RVA: 0x000EE7F8 File Offset: 0x000ED7F8
		~IsolatedStorageFile()
		{
			this.Dispose();
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x000EE824 File Offset: 0x000ED824
		private static bool NotIDFile(string file)
		{
			return string.Compare(file, "identity.dat", StringComparison.Ordinal) != 0;
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x000EE838 File Offset: 0x000ED838
		private static bool NotInfoFile(string file)
		{
			return string.Compare(file, "info.dat", StringComparison.Ordinal) != 0 && string.Compare(file, "appinfo.dat", StringComparison.Ordinal) != 0;
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x000EE85C File Offset: 0x000ED85C
		private static bool NotFilesDir(string dir)
		{
			return string.Compare(dir, "Files", StringComparison.Ordinal) != 0;
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x000EE870 File Offset: 0x000ED870
		internal static bool NotAssemFilesDir(string dir)
		{
			return string.Compare(dir, "AssemFiles", StringComparison.Ordinal) != 0;
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x000EE884 File Offset: 0x000ED884
		internal static bool NotAppFilesDir(string dir)
		{
			return string.Compare(dir, "AppFiles", StringComparison.Ordinal) != 0;
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x000EE898 File Offset: 0x000ED898
		public static void Remove(IsolatedStorageScope scope)
		{
			IsolatedStorageFile.VerifyGlobalScope(scope);
			IsolatedStorageFile.DemandAdminPermission();
			string rootDir = IsolatedStorageFile.GetRootDir(scope);
			new FileIOPermission(FileIOPermissionAccess.Write, rootDir).Assert();
			try
			{
				Directory.Delete(rootDir, true);
				Directory.CreateDirectory(rootDir);
			}
			catch
			{
				throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_DeleteDirectories"));
			}
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x000EE8F4 File Offset: 0x000ED8F4
		public static IEnumerator GetEnumerator(IsolatedStorageScope scope)
		{
			IsolatedStorageFile.VerifyGlobalScope(scope);
			IsolatedStorageFile.DemandAdminPermission();
			return new IsolatedStorageFileEnumerator(scope);
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x060045C0 RID: 17856 RVA: 0x000EE907 File Offset: 0x000ED907
		internal string RootDirectory
		{
			get
			{
				return this.m_RootDir;
			}
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x000EE910 File Offset: 0x000ED910
		internal string GetFullPath(string path)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.RootDirectory);
			if (path[0] == this.SeparatorExternal)
			{
				stringBuilder.Append(path.Substring(1));
			}
			else
			{
				stringBuilder.Append(path);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x000EE960 File Offset: 0x000ED960
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static string GetDataDirectoryFromActivationContext()
		{
			if (IsolatedStorageFile.s_appDataDir == null)
			{
				ActivationContext activationContext = AppDomain.CurrentDomain.ActivationContext;
				if (activationContext == null)
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_ApplicationMissingIdentity"));
				}
				string text = activationContext.DataDirectory;
				if (text != null && text[text.Length - 1] != '\\')
				{
					text += "\\";
				}
				IsolatedStorageFile.s_appDataDir = text;
			}
			return IsolatedStorageFile.s_appDataDir;
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x000EE9C8 File Offset: 0x000ED9C8
		internal void Init(IsolatedStorageScope scope)
		{
			IsolatedStorageFile.GetGlobalFileIOPerm(scope).Assert();
			StringBuilder stringBuilder = new StringBuilder();
			if (IsolatedStorage.IsApp(scope))
			{
				stringBuilder.Append(IsolatedStorageFile.GetRootDir(scope));
				if (IsolatedStorageFile.s_appDataDir == null)
				{
					stringBuilder.Append(base.AppName);
					stringBuilder.Append(this.SeparatorExternal);
				}
				try
				{
					Directory.CreateDirectory(stringBuilder.ToString());
				}
				catch
				{
				}
				this.CreateIDFile(stringBuilder.ToString(), scope);
				this.m_InfoFile = stringBuilder.ToString() + "appinfo.dat";
				stringBuilder.Append("AppFiles");
			}
			else
			{
				stringBuilder.Append(IsolatedStorageFile.GetRootDir(scope));
				if (IsolatedStorage.IsDomain(scope))
				{
					stringBuilder.Append(base.DomainName);
					stringBuilder.Append(this.SeparatorExternal);
					try
					{
						Directory.CreateDirectory(stringBuilder.ToString());
						this.CreateIDFile(stringBuilder.ToString(), scope);
					}
					catch
					{
					}
					this.m_InfoFile = stringBuilder.ToString() + "info.dat";
				}
				stringBuilder.Append(base.AssemName);
				stringBuilder.Append(this.SeparatorExternal);
				try
				{
					Directory.CreateDirectory(stringBuilder.ToString());
					this.CreateIDFile(stringBuilder.ToString(), scope);
				}
				catch
				{
				}
				if (IsolatedStorage.IsDomain(scope))
				{
					stringBuilder.Append("Files");
				}
				else
				{
					this.m_InfoFile = stringBuilder.ToString() + "info.dat";
					stringBuilder.Append("AssemFiles");
				}
			}
			stringBuilder.Append(this.SeparatorExternal);
			string text = stringBuilder.ToString();
			try
			{
				Directory.CreateDirectory(text);
			}
			catch
			{
			}
			this.m_RootDir = text;
			this.m_fiop = new FileIOPermission(FileIOPermissionAccess.AllAccess, text);
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x000EEBA0 File Offset: 0x000EDBA0
		internal bool InitExistingStore(IsolatedStorageScope scope)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(IsolatedStorageFile.GetRootDir(scope));
			if (IsolatedStorage.IsApp(scope))
			{
				stringBuilder.Append(base.AppName);
				stringBuilder.Append(this.SeparatorExternal);
				this.m_InfoFile = stringBuilder.ToString() + "appinfo.dat";
				stringBuilder.Append("AppFiles");
			}
			else
			{
				if (IsolatedStorage.IsDomain(scope))
				{
					stringBuilder.Append(base.DomainName);
					stringBuilder.Append(this.SeparatorExternal);
					this.m_InfoFile = stringBuilder.ToString() + "info.dat";
				}
				stringBuilder.Append(base.AssemName);
				stringBuilder.Append(this.SeparatorExternal);
				if (IsolatedStorage.IsDomain(scope))
				{
					stringBuilder.Append("Files");
				}
				else
				{
					this.m_InfoFile = stringBuilder.ToString() + "info.dat";
					stringBuilder.Append("AssemFiles");
				}
			}
			stringBuilder.Append(this.SeparatorExternal);
			FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, stringBuilder.ToString());
			fileIOPermission.Assert();
			if (!Directory.Exists(stringBuilder.ToString()))
			{
				return false;
			}
			this.m_RootDir = stringBuilder.ToString();
			this.m_fiop = fileIOPermission;
			return true;
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x000EECD7 File Offset: 0x000EDCD7
		protected override IsolatedStoragePermission GetPermission(PermissionSet ps)
		{
			if (ps == null)
			{
				return null;
			}
			if (ps.IsUnrestricted())
			{
				return new IsolatedStorageFilePermission(PermissionState.Unrestricted);
			}
			return (IsolatedStoragePermission)ps.GetPermission(typeof(IsolatedStorageFilePermission));
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x000EED02 File Offset: 0x000EDD02
		internal void UndoReserveOperation(ulong oldLen, ulong newLen)
		{
			oldLen = IsolatedStorageFile.RoundToBlockSize(oldLen);
			if (newLen > oldLen)
			{
				this.Unreserve(IsolatedStorageFile.RoundToBlockSize(newLen - oldLen));
			}
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x000EED1E File Offset: 0x000EDD1E
		internal void Reserve(ulong oldLen, ulong newLen)
		{
			oldLen = IsolatedStorageFile.RoundToBlockSize(oldLen);
			if (newLen > oldLen)
			{
				this.Reserve(IsolatedStorageFile.RoundToBlockSize(newLen - oldLen));
			}
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x000EED3A File Offset: 0x000EDD3A
		internal void ReserveOneBlock()
		{
			this.Reserve(1024UL);
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x000EED48 File Offset: 0x000EDD48
		internal void UnreserveOneBlock()
		{
			this.Unreserve(1024UL);
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x000EED58 File Offset: 0x000EDD58
		internal static ulong RoundToBlockSize(ulong num)
		{
			if (num < 1024UL)
			{
				return 1024UL;
			}
			ulong num2 = num % 1024UL;
			if (num2 != 0UL)
			{
				num += 1024UL - num2;
			}
			return num;
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x000EED90 File Offset: 0x000EDD90
		internal static string GetRootDir(IsolatedStorageScope scope)
		{
			if (IsolatedStorage.IsRoaming(scope))
			{
				if (IsolatedStorageFile.s_RootDirRoaming == null)
				{
					IsolatedStorageFile.s_RootDirRoaming = IsolatedStorageFile.nGetRootDir(scope);
				}
				return IsolatedStorageFile.s_RootDirRoaming;
			}
			if (IsolatedStorage.IsMachine(scope))
			{
				if (IsolatedStorageFile.s_RootDirMachine == null)
				{
					IsolatedStorageFile.InitGlobalsMachine(scope);
				}
				return IsolatedStorageFile.s_RootDirMachine;
			}
			if (IsolatedStorageFile.s_RootDirUser == null)
			{
				IsolatedStorageFile.InitGlobalsNonRoamingUser(scope);
			}
			return IsolatedStorageFile.s_RootDirUser;
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x000EEDEC File Offset: 0x000EDDEC
		private static void InitGlobalsMachine(IsolatedStorageScope scope)
		{
			string text = IsolatedStorageFile.nGetRootDir(scope);
			new FileIOPermission(FileIOPermissionAccess.AllAccess, text).Assert();
			string text2 = IsolatedStorageFile.GetMachineRandomDirectory(text);
			if (text2 == null)
			{
				Mutex mutex = IsolatedStorageFile.CreateMutexNotOwned(text);
				if (!mutex.WaitOne())
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_Init"));
				}
				try
				{
					text2 = IsolatedStorageFile.GetMachineRandomDirectory(text);
					if (text2 == null)
					{
						string randomFileName = Path.GetRandomFileName();
						string randomFileName2 = Path.GetRandomFileName();
						try
						{
							IsolatedStorageFile.nCreateDirectoryWithDacl(text + randomFileName);
							IsolatedStorageFile.nCreateDirectoryWithDacl(text + randomFileName + "\\" + randomFileName2);
						}
						catch
						{
							throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_Init"));
						}
						text2 = randomFileName + "\\" + randomFileName2;
					}
				}
				finally
				{
					mutex.ReleaseMutex();
				}
			}
			IsolatedStorageFile.s_RootDirMachine = text + text2 + "\\";
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x000EEEC4 File Offset: 0x000EDEC4
		private static void InitGlobalsNonRoamingUser(IsolatedStorageScope scope)
		{
			string text = null;
			if (scope == (IsolatedStorageScope.User | IsolatedStorageScope.Application))
			{
				text = IsolatedStorageFile.GetDataDirectoryFromActivationContext();
				if (text != null)
				{
					IsolatedStorageFile.s_RootDirUser = text;
					return;
				}
			}
			text = IsolatedStorageFile.nGetRootDir(scope);
			new FileIOPermission(FileIOPermissionAccess.AllAccess, text).Assert();
			bool flag = false;
			string text2 = null;
			string text3 = IsolatedStorageFile.GetRandomDirectory(text, out flag, out text2);
			if (text3 == null)
			{
				Mutex mutex = IsolatedStorageFile.CreateMutexNotOwned(text);
				if (!mutex.WaitOne())
				{
					throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_Init"));
				}
				try
				{
					text3 = IsolatedStorageFile.GetRandomDirectory(text, out flag, out text2);
					if (text3 == null)
					{
						if (flag)
						{
							text3 = IsolatedStorageFile.MigrateOldIsoStoreDirectory(text, text2);
						}
						else
						{
							text3 = IsolatedStorageFile.CreateRandomDirectory(text);
						}
					}
				}
				finally
				{
					mutex.ReleaseMutex();
				}
			}
			IsolatedStorageFile.s_RootDirUser = text + text3 + "\\";
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x000EEF7C File Offset: 0x000EDF7C
		internal static string MigrateOldIsoStoreDirectory(string rootDir, string oldRandomDirectory)
		{
			string randomFileName = Path.GetRandomFileName();
			string randomFileName2 = Path.GetRandomFileName();
			string text = rootDir + randomFileName;
			string text2 = text + "\\" + randomFileName2;
			try
			{
				Directory.CreateDirectory(text);
				Directory.Move(rootDir + oldRandomDirectory, text2);
			}
			catch
			{
				throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_Init"));
			}
			return randomFileName + "\\" + randomFileName2;
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x000EEFF0 File Offset: 0x000EDFF0
		internal static string CreateRandomDirectory(string rootDir)
		{
			string text = Path.GetRandomFileName() + "\\" + Path.GetRandomFileName();
			try
			{
				Directory.CreateDirectory(rootDir + text);
			}
			catch
			{
				throw new IsolatedStorageException(Environment.GetResourceString("IsolatedStorage_Init"));
			}
			return text;
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x000EF044 File Offset: 0x000EE044
		internal static string GetRandomDirectory(string rootDir, out bool bMigrateNeeded, out string sOldStoreLocation)
		{
			bMigrateNeeded = false;
			sOldStoreLocation = null;
			string[] fileDirectoryNames = IsolatedStorageFile.GetFileDirectoryNames(rootDir + "*", "*", false);
			for (int i = 0; i < fileDirectoryNames.Length; i++)
			{
				if (fileDirectoryNames[i].Length == 12)
				{
					string[] fileDirectoryNames2 = IsolatedStorageFile.GetFileDirectoryNames(rootDir + fileDirectoryNames[i] + "\\*", "*", false);
					for (int j = 0; j < fileDirectoryNames2.Length; j++)
					{
						if (fileDirectoryNames2[j].Length == 12)
						{
							return fileDirectoryNames[i] + "\\" + fileDirectoryNames2[j];
						}
					}
				}
			}
			for (int k = 0; k < fileDirectoryNames.Length; k++)
			{
				if (fileDirectoryNames[k].Length == 24)
				{
					bMigrateNeeded = true;
					sOldStoreLocation = fileDirectoryNames[k];
					return null;
				}
			}
			return null;
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x000EF0F8 File Offset: 0x000EE0F8
		internal static string GetMachineRandomDirectory(string rootDir)
		{
			string[] fileDirectoryNames = IsolatedStorageFile.GetFileDirectoryNames(rootDir + "*", "*", false);
			for (int i = 0; i < fileDirectoryNames.Length; i++)
			{
				if (fileDirectoryNames[i].Length == 12)
				{
					string[] fileDirectoryNames2 = IsolatedStorageFile.GetFileDirectoryNames(rootDir + fileDirectoryNames[i] + "\\*", "*", false);
					for (int j = 0; j < fileDirectoryNames2.Length; j++)
					{
						if (fileDirectoryNames2[j].Length == 12)
						{
							return fileDirectoryNames[i] + "\\" + fileDirectoryNames2[j];
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x000EF17C File Offset: 0x000EE17C
		internal static Mutex CreateMutexNotOwned(string pathName)
		{
			return new Mutex(false, "Global\\" + IsolatedStorageFile.GetStrongHashSuitableForObjectName(pathName));
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x000EF194 File Offset: 0x000EE194
		internal static string GetStrongHashSuitableForObjectName(string name)
		{
			MemoryStream memoryStream = new MemoryStream();
			new BinaryWriter(memoryStream).Write(name.ToUpper(CultureInfo.InvariantCulture));
			memoryStream.Position = 0L;
			return IsolatedStorage.ToBase32StringSuitableForDirName(new SHA1CryptoServiceProvider().ComputeHash(memoryStream));
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x000EF1D5 File Offset: 0x000EE1D5
		private string GetSyncObjectName()
		{
			if (this.m_SyncObjectName == null)
			{
				this.m_SyncObjectName = IsolatedStorageFile.GetStrongHashSuitableForObjectName(this.m_InfoFile);
			}
			return this.m_SyncObjectName;
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x000EF1F8 File Offset: 0x000EE1F8
		internal void Lock()
		{
			if (base.IsRoaming())
			{
				return;
			}
			lock (this)
			{
				if (this.m_bDisposed)
				{
					throw new ObjectDisposedException(null, Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_closed)
				{
					throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_handle == Win32Native.NULL)
				{
					this.m_handle = IsolatedStorageFile.nOpen(this.m_InfoFile, this.GetSyncObjectName());
				}
				IsolatedStorageFile.nLock(this.m_handle, true);
			}
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x000EF298 File Offset: 0x000EE298
		internal void Unlock()
		{
			if (base.IsRoaming())
			{
				return;
			}
			lock (this)
			{
				if (this.m_bDisposed)
				{
					throw new ObjectDisposedException(null, Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_closed)
				{
					throw new InvalidOperationException(Environment.GetResourceString("IsolatedStorage_StoreNotOpen"));
				}
				if (this.m_handle == Win32Native.NULL)
				{
					this.m_handle = IsolatedStorageFile.nOpen(this.m_InfoFile, this.GetSyncObjectName());
				}
				IsolatedStorageFile.nLock(this.m_handle, false);
			}
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x000EF338 File Offset: 0x000EE338
		internal static FileIOPermission GetGlobalFileIOPerm(IsolatedStorageScope scope)
		{
			if (IsolatedStorage.IsRoaming(scope))
			{
				if (IsolatedStorageFile.s_PermRoaming == null)
				{
					IsolatedStorageFile.s_PermRoaming = new FileIOPermission(FileIOPermissionAccess.AllAccess, IsolatedStorageFile.GetRootDir(scope));
				}
				return IsolatedStorageFile.s_PermRoaming;
			}
			if (IsolatedStorage.IsMachine(scope))
			{
				if (IsolatedStorageFile.s_PermMachine == null)
				{
					IsolatedStorageFile.s_PermMachine = new FileIOPermission(FileIOPermissionAccess.AllAccess, IsolatedStorageFile.GetRootDir(scope));
				}
				return IsolatedStorageFile.s_PermMachine;
			}
			if (IsolatedStorageFile.s_PermUser == null)
			{
				IsolatedStorageFile.s_PermUser = new FileIOPermission(FileIOPermissionAccess.AllAccess, IsolatedStorageFile.GetRootDir(scope));
			}
			return IsolatedStorageFile.s_PermUser;
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x000EF3B1 File Offset: 0x000EE3B1
		private static void DemandAdminPermission()
		{
			if (IsolatedStorageFile.s_PermAdminUser == null)
			{
				IsolatedStorageFile.s_PermAdminUser = new IsolatedStorageFilePermission(IsolatedStorageContainment.AdministerIsolatedStorageByUser, 0L, false);
			}
			IsolatedStorageFile.s_PermAdminUser.Demand();
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x000EF3D3 File Offset: 0x000EE3D3
		internal static void VerifyGlobalScope(IsolatedStorageScope scope)
		{
			if (scope != IsolatedStorageScope.User && scope != (IsolatedStorageScope.User | IsolatedStorageScope.Roaming) && scope != IsolatedStorageScope.Machine)
			{
				throw new ArgumentException(Environment.GetResourceString("IsolatedStorage_Scope_U_R_M"));
			}
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x000EF3F4 File Offset: 0x000EE3F4
		internal void CreateIDFile(string path, IsolatedStorageScope scope)
		{
			try
			{
				using (FileStream fileStream = new FileStream(path + "identity.dat", FileMode.OpenOrCreate))
				{
					MemoryStream identityStream = base.GetIdentityStream(scope);
					byte[] buffer = identityStream.GetBuffer();
					fileStream.Write(buffer, 0, (int)identityStream.Length);
					identityStream.Close();
				}
			}
			catch
			{
			}
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x000EF464 File Offset: 0x000EE464
		private static string[] GetFileDirectoryNames(string path, string msg, bool file)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path", Environment.GetResourceString("ArgumentNull_Path"));
			}
			bool flag = false;
			char c = path[path.Length - 1];
			if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == '.')
			{
				flag = true;
			}
			string text = Path.GetFullPathInternal(path);
			if (flag && text[text.Length - 1] != c)
			{
				text += "\\*";
			}
			string text2 = Path.GetDirectoryName(text);
			if (text2 != null)
			{
				text2 += "\\";
			}
			new FileIOPermission(FileIOPermissionAccess.Read, (text2 == null) ? text : text2).Demand();
			string[] array = new string[10];
			int num = 0;
			Win32Native.WIN32_FIND_DATA win32_FIND_DATA = new Win32Native.WIN32_FIND_DATA();
			SafeFindHandle safeFindHandle = Win32Native.FindFirstFile(text, win32_FIND_DATA);
			int num2;
			if (safeFindHandle.IsInvalid)
			{
				num2 = Marshal.GetLastWin32Error();
				if (num2 == 2)
				{
					return new string[0];
				}
				__Error.WinIOError(num2, msg);
			}
			int num3 = 0;
			do
			{
				bool flag2;
				if (file)
				{
					flag2 = 0 == (win32_FIND_DATA.dwFileAttributes & 16);
				}
				else
				{
					flag2 = 0 != (win32_FIND_DATA.dwFileAttributes & 16);
					if (flag2 && (win32_FIND_DATA.cFileName.Equals(".") || win32_FIND_DATA.cFileName.Equals("..")))
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					num3++;
					if (num == array.Length)
					{
						string[] array2 = new string[array.Length * 2];
						Array.Copy(array, 0, array2, 0, num);
						array = array2;
					}
					array[num++] = win32_FIND_DATA.cFileName;
				}
			}
			while (Win32Native.FindNextFile(safeFindHandle, win32_FIND_DATA));
			num2 = Marshal.GetLastWin32Error();
			safeFindHandle.Close();
			if (num2 != 0 && num2 != 18)
			{
				__Error.WinIOError(num2, msg);
			}
			if (!file && num3 == 1 && (win32_FIND_DATA.dwFileAttributes & 16) != 0)
			{
				return new string[] { win32_FIND_DATA.cFileName };
			}
			if (num == array.Length)
			{
				return array;
			}
			string[] array3 = new string[num];
			Array.Copy(array, 0, array3, 0, num);
			return array3;
		}

		// Token: 0x060045DC RID: 17884
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong nGetUsage(IntPtr handle);

		// Token: 0x060045DD RID: 17885
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr nOpen(string infoFile, string syncName);

		// Token: 0x060045DE RID: 17886
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void nClose(IntPtr handle);

		// Token: 0x060045DF RID: 17887
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void nReserve(IntPtr handle, ulong* plQuota, ulong* plReserve, bool fFree);

		// Token: 0x060045E0 RID: 17888
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nGetRootDir(IsolatedStorageScope scope);

		// Token: 0x060045E1 RID: 17889
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void nLock(IntPtr handle, bool fLock);

		// Token: 0x060045E2 RID: 17890
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void nCreateDirectoryWithDacl(string path);

		// Token: 0x04002283 RID: 8835
		private const int s_BlockSize = 1024;

		// Token: 0x04002284 RID: 8836
		private const int s_DirSize = 1024;

		// Token: 0x04002285 RID: 8837
		private const string s_name = "file.store";

		// Token: 0x04002286 RID: 8838
		internal const string s_Files = "Files";

		// Token: 0x04002287 RID: 8839
		internal const string s_AssemFiles = "AssemFiles";

		// Token: 0x04002288 RID: 8840
		internal const string s_AppFiles = "AppFiles";

		// Token: 0x04002289 RID: 8841
		internal const string s_IDFile = "identity.dat";

		// Token: 0x0400228A RID: 8842
		internal const string s_InfoFile = "info.dat";

		// Token: 0x0400228B RID: 8843
		internal const string s_AppInfoFile = "appinfo.dat";

		// Token: 0x0400228C RID: 8844
		private static string s_RootDirUser;

		// Token: 0x0400228D RID: 8845
		private static string s_RootDirMachine;

		// Token: 0x0400228E RID: 8846
		private static string s_RootDirRoaming;

		// Token: 0x0400228F RID: 8847
		private static string s_appDataDir;

		// Token: 0x04002290 RID: 8848
		private static FileIOPermission s_PermUser;

		// Token: 0x04002291 RID: 8849
		private static FileIOPermission s_PermMachine;

		// Token: 0x04002292 RID: 8850
		private static FileIOPermission s_PermRoaming;

		// Token: 0x04002293 RID: 8851
		private static IsolatedStorageFilePermission s_PermAdminUser;

		// Token: 0x04002294 RID: 8852
		private FileIOPermission m_fiop;

		// Token: 0x04002295 RID: 8853
		private string m_RootDir;

		// Token: 0x04002296 RID: 8854
		private string m_InfoFile;

		// Token: 0x04002297 RID: 8855
		private string m_SyncObjectName;

		// Token: 0x04002298 RID: 8856
		private IntPtr m_handle;

		// Token: 0x04002299 RID: 8857
		private bool m_closed;

		// Token: 0x0400229A RID: 8858
		private bool m_bDisposed;
	}
}
