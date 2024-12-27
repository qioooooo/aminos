using System;
using System.Collections;
using System.Configuration;
using System.Configuration.Internal;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000237 RID: 567
	[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
	internal sealed class RemoteWebConfigurationHost : DelegatingConfigHost
	{
		// Token: 0x06001E46 RID: 7750 RVA: 0x00087ABB File Offset: 0x00086ABB
		internal RemoteWebConfigurationHost()
		{
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x00087AC3 File Offset: 0x00086AC3
		public override void Init(IInternalConfigRoot configRoot, params object[] hostInitParams)
		{
			throw ExceptionUtil.UnexpectedError("RemoteWebConfigurationHost::Init");
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x00087AD0 File Offset: 0x00086AD0
		public override void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot root, params object[] hostInitConfigurationParams)
		{
			WebLevel webLevel = (WebLevel)hostInitConfigurationParams[0];
			string text = (string)hostInitConfigurationParams[2];
			string text2 = (string)hostInitConfigurationParams[3];
			if (locationSubPath == null)
			{
				locationSubPath = (string)hostInitConfigurationParams[4];
			}
			string text3 = (string)hostInitConfigurationParams[5];
			string text4 = (string)hostInitConfigurationParams[6];
			string text5 = (string)hostInitConfigurationParams[7];
			IntPtr intPtr = (IntPtr)hostInitConfigurationParams[8];
			configPath = null;
			locationConfigPath = null;
			this._Server = text3;
			this._Username = RemoteWebConfigurationHost.GetUserNameFromFullName(text4);
			this._Domain = RemoteWebConfigurationHost.GetDomainFromFullName(text4);
			this._Password = text5;
			this._Identity = ((intPtr == IntPtr.Zero) ? null : new WindowsIdentity(intPtr));
			this._PathMap = new Hashtable(StringComparer.OrdinalIgnoreCase);
			string filePaths;
			try
			{
				WindowsImpersonationContext windowsImpersonationContext = ((this._Identity != null) ? this._Identity.Impersonate() : null);
				try
				{
					IRemoteWebConfigurationHostServer remoteWebConfigurationHostServer = RemoteWebConfigurationHost.CreateRemoteObject(text3, this._Username, this._Domain, text5);
					try
					{
						filePaths = remoteWebConfigurationHostServer.GetFilePaths((int)webLevel, text, text2, locationSubPath);
					}
					finally
					{
						while (Marshal.ReleaseComObject(remoteWebConfigurationHostServer) > 0)
						{
						}
					}
				}
				finally
				{
					if (windowsImpersonationContext != null)
					{
						windowsImpersonationContext.Undo();
					}
				}
			}
			catch
			{
				throw;
			}
			if (filePaths == null)
			{
				throw ExceptionUtil.UnexpectedError("RemoteWebConfigurationHost::InitForConfiguration");
			}
			string[] array = filePaths.Split(RemoteWebConfigurationHostServer.FilePathsSeparatorParams);
			if (array.Length < 7 || (array.Length - 5) % 2 != 0)
			{
				throw ExceptionUtil.UnexpectedError("RemoteWebConfigurationHost::InitForConfiguration");
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length == 0)
				{
					array[i] = null;
				}
			}
			string text6 = array[0];
			string text7 = array[1];
			string text8 = array[2];
			configPath = array[3];
			locationConfigPath = array[4];
			this._ConfigPath = configPath;
			WebConfigurationFileMap webConfigurationFileMap = new WebConfigurationFileMap();
			VirtualPath virtualPath = VirtualPath.CreateAbsoluteAllowNull(text6);
			webConfigurationFileMap.Site = text8;
			for (int j = 5; j < array.Length; j += 2)
			{
				string text9 = array[j];
				string text10 = array[j + 1];
				this._PathMap.Add(text9, text10);
				if (WebConfigurationHost.IsMachineConfigPath(text9))
				{
					webConfigurationFileMap.MachineConfigFilename = text10;
				}
				else
				{
					string text11;
					bool flag;
					if (WebConfigurationHost.IsRootWebConfigPath(text9))
					{
						text11 = null;
						flag = false;
					}
					else
					{
						string text12;
						VirtualPath virtualPath2;
						WebConfigurationHost.GetSiteIDAndVPathFromConfigPath(text9, out text12, out virtualPath2);
						text11 = VirtualPath.GetVirtualPathString(virtualPath2);
						flag = virtualPath2 == virtualPath;
					}
					webConfigurationFileMap.VirtualDirectories.Add(text11, new VirtualDirectoryMapping(Path.GetDirectoryName(text10), flag));
				}
			}
			WebConfigurationHost webConfigurationHost = new WebConfigurationHost();
			webConfigurationHost.Init(root, new object[]
			{
				true,
				new UserMapPath(webConfigurationFileMap, false),
				null,
				text6,
				text7,
				text8
			});
			base.Host = webConfigurationHost;
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x00087D98 File Offset: 0x00086D98
		public override bool IsConfigRecordRequired(string configPath)
		{
			return configPath.Length <= this._ConfigPath.Length;
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x00087DB0 File Offset: 0x00086DB0
		public override string GetStreamName(string configPath)
		{
			return (string)this._PathMap[configPath];
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x00087DC4 File Offset: 0x00086DC4
		public override object GetStreamVersion(string streamName)
		{
			WindowsImpersonationContext windowsImpersonationContext = null;
			bool flag;
			long num;
			long num2;
			long num3;
			try
			{
				if (this._Identity != null)
				{
					windowsImpersonationContext = this._Identity.Impersonate();
				}
				try
				{
					IRemoteWebConfigurationHostServer remoteWebConfigurationHostServer = RemoteWebConfigurationHost.CreateRemoteObject(this._Server, this._Username, this._Domain, this._Password);
					try
					{
						remoteWebConfigurationHostServer.GetFileDetails(streamName, out flag, out num, out num2, out num3);
					}
					finally
					{
						while (Marshal.ReleaseComObject(remoteWebConfigurationHostServer) > 0)
						{
						}
					}
				}
				finally
				{
					if (windowsImpersonationContext != null)
					{
						windowsImpersonationContext.Undo();
					}
				}
			}
			catch
			{
				throw;
			}
			return new FileDetails(flag, num, DateTime.FromFileTimeUtc(num2), DateTime.FromFileTimeUtc(num3));
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x00087E74 File Offset: 0x00086E74
		public override Stream OpenStreamForRead(string streamName)
		{
			RemoteWebConfigurationHostStream remoteWebConfigurationHostStream = new RemoteWebConfigurationHostStream(false, this._Server, streamName, null, this._Username, this._Domain, this._Password, this._Identity);
			if (remoteWebConfigurationHostStream == null || remoteWebConfigurationHostStream.Length < 1L)
			{
				return null;
			}
			return remoteWebConfigurationHostStream;
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x00087EB8 File Offset: 0x00086EB8
		public override Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext)
		{
			RemoteWebConfigurationHostStream remoteWebConfigurationHostStream = new RemoteWebConfigurationHostStream(true, this._Server, streamName, templateStreamName, this._Username, this._Domain, this._Password, this._Identity);
			writeContext = remoteWebConfigurationHostStream;
			return remoteWebConfigurationHostStream;
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x00087EF0 File Offset: 0x00086EF0
		public override void DeleteStream(string StreamName)
		{
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x00087EF4 File Offset: 0x00086EF4
		public override void WriteCompleted(string streamName, bool success, object writeContext)
		{
			if (success)
			{
				RemoteWebConfigurationHostStream remoteWebConfigurationHostStream = (RemoteWebConfigurationHostStream)writeContext;
				remoteWebConfigurationHostStream.FlushForWriteCompleted();
			}
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x00087F11 File Offset: 0x00086F11
		public override bool IsFile(string StreamName)
		{
			return false;
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x00087F14 File Offset: 0x00086F14
		public override bool PrefetchAll(string configPath, string StreamName)
		{
			return true;
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x00087F17 File Offset: 0x00086F17
		public override bool PrefetchSection(string sectionGroupName, string sectionName)
		{
			return true;
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x00087F1A File Offset: 0x00086F1A
		public override void GetRestrictedPermissions(IInternalConfigRecord configRecord, out PermissionSet permissionSet, out bool isHostReady)
		{
			WebConfigurationHost.StaticGetRestrictedPermissions(configRecord, out permissionSet, out isHostReady);
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x00087F24 File Offset: 0x00086F24
		public override bool IsRemote
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x00087F27 File Offset: 0x00086F27
		public override string DecryptSection(string encryptedXmlString, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection)
		{
			return this.CallEncryptOrDecrypt(false, encryptedXmlString, protectionProvider, protectedConfigSection);
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x00087F33 File Offset: 0x00086F33
		public override string EncryptSection(string clearTextXmlString, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection)
		{
			return this.CallEncryptOrDecrypt(true, clearTextXmlString, protectionProvider, protectedConfigSection);
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x00087F40 File Offset: 0x00086F40
		private string CallEncryptOrDecrypt(bool doEncrypt, string xmlString, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection)
		{
			/*
An exception occurred when decompiling this method (06001E57)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.String System.Web.Configuration.RemoteWebConfigurationHost::CallEncryptOrDecrypt(System.Boolean,System.String,System.Configuration.ProtectedConfigurationProvider,System.Configuration.ProtectedConfigurationSection)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.VariableSlot.CloneVariableState(VariableSlot[] state) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 78
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 407
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x00088054 File Offset: 0x00087054
		private static string GetUserNameFromFullName(string fullUserName)
		{
			if (string.IsNullOrEmpty(fullUserName))
			{
				return null;
			}
			if (fullUserName.Contains("@"))
			{
				return fullUserName;
			}
			string[] array = fullUserName.Split(new char[] { '\\' });
			if (array.Length == 1)
			{
				return fullUserName;
			}
			return array[1];
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x0008809C File Offset: 0x0008709C
		private static string GetDomainFromFullName(string fullUserName)
		{
			if (string.IsNullOrEmpty(fullUserName))
			{
				return null;
			}
			if (fullUserName.Contains("@"))
			{
				return null;
			}
			string[] array = fullUserName.Split(new char[] { '\\' });
			if (array.Length == 1)
			{
				return ".";
			}
			return array[0];
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x000880E8 File Offset: 0x000870E8
		internal static IRemoteWebConfigurationHostServer CreateRemoteObject(string server, string username, string domain, string password)
		{
			IRemoteWebConfigurationHostServer remoteWebConfigurationHostServer;
			try
			{
				if (string.IsNullOrEmpty(username))
				{
					remoteWebConfigurationHostServer = RemoteWebConfigurationHost.CreateRemoteObjectUsingGetTypeFromCLSID(server);
				}
				else if (IntPtr.Size == 8)
				{
					remoteWebConfigurationHostServer = RemoteWebConfigurationHost.CreateRemoteObjectOn64BitPlatform(server, username, domain, password);
				}
				else
				{
					remoteWebConfigurationHostServer = RemoteWebConfigurationHost.CreateRemoteObjectOn32BitPlatform(server, username, domain, password);
				}
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147221164)
				{
					throw new Exception(SR.GetString("Make_sure_remote_server_is_enabled_for_config_access"));
				}
				throw;
			}
			return remoteWebConfigurationHostServer;
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x00088158 File Offset: 0x00087158
		private static IRemoteWebConfigurationHostServer CreateRemoteObjectUsingGetTypeFromCLSID(string server)
		{
			Type typeFromCLSID = Type.GetTypeFromCLSID(typeof(RemoteWebConfigurationHostServer).GUID, server, true);
			return (IRemoteWebConfigurationHostServer)Activator.CreateInstance(typeFromCLSID);
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x00088188 File Offset: 0x00087188
		private static IRemoteWebConfigurationHostServer CreateRemoteObjectOn32BitPlatform(string server, string username, string domain, string password)
		{
			MULTI_QI[] array = new MULTI_QI[1];
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			Guid guid = typeof(RemoteWebConfigurationHostServer).GUID;
			IntPtr intPtr3 = IntPtr.Zero;
			IRemoteWebConfigurationHostServer remoteWebConfigurationHostServer;
			try
			{
				intPtr = Marshal.AllocCoTaskMem(16);
				Marshal.StructureToPtr(typeof(IRemoteWebConfigurationHostServer).GUID, intPtr, false);
				array[0] = new MULTI_QI(intPtr);
				COAUTHIDENTITY coauthidentity = new COAUTHIDENTITY(username, domain, password);
				intPtr3 = Marshal.AllocCoTaskMem(Marshal.SizeOf(coauthidentity));
				Marshal.StructureToPtr(coauthidentity, intPtr3, false);
				COAUTHINFO coauthinfo = new COAUTHINFO(RpcAuthent.WinNT, RpcAuthor.None, null, RpcLevel.Default, RpcImpers.Impersonate, intPtr3);
				intPtr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(coauthinfo));
				Marshal.StructureToPtr(coauthinfo, intPtr2, false);
				COSERVERINFO coserverinfo = new COSERVERINFO(server, intPtr2);
				int num = UnsafeNativeMethods.CoCreateInstanceEx(ref guid, IntPtr.Zero, 16, coserverinfo, 1, array);
				if (num == -2147221164)
				{
					throw new Exception(SR.GetString("Make_sure_remote_server_is_enabled_for_config_access"));
				}
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				if (array[0].hr < 0)
				{
					Marshal.ThrowExceptionForHR(array[0].hr);
				}
				num = UnsafeNativeMethods.CoSetProxyBlanket(array[0].pItf, RpcAuthent.WinNT, RpcAuthor.None, null, RpcLevel.Default, RpcImpers.Impersonate, intPtr3, 0);
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				remoteWebConfigurationHostServer = (IRemoteWebConfigurationHostServer)Marshal.GetObjectForIUnknown(array[0].pItf);
			}
			finally
			{
				if (array[0].pItf != IntPtr.Zero)
				{
					Marshal.Release(array[0].pItf);
					array[0].pItf = IntPtr.Zero;
				}
				array[0].piid = IntPtr.Zero;
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.DestroyStructure(intPtr2, typeof(COAUTHINFO));
					Marshal.FreeCoTaskMem(intPtr2);
				}
				if (intPtr3 != IntPtr.Zero)
				{
					Marshal.DestroyStructure(intPtr3, typeof(COAUTHIDENTITY));
					Marshal.FreeCoTaskMem(intPtr3);
				}
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
			return remoteWebConfigurationHostServer;
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x000883B4 File Offset: 0x000873B4
		private static IRemoteWebConfigurationHostServer CreateRemoteObjectOn64BitPlatform(string server, string username, string domain, string password)
		{
			MULTI_QI_X64[] array = new MULTI_QI_X64[1];
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			Guid guid = typeof(RemoteWebConfigurationHostServer).GUID;
			IntPtr intPtr3 = IntPtr.Zero;
			IRemoteWebConfigurationHostServer remoteWebConfigurationHostServer;
			try
			{
				intPtr = Marshal.AllocCoTaskMem(16);
				Marshal.StructureToPtr(typeof(IRemoteWebConfigurationHostServer).GUID, intPtr, false);
				array[0] = new MULTI_QI_X64(intPtr);
				COAUTHIDENTITY_X64 coauthidentity_X = new COAUTHIDENTITY_X64(username, domain, password);
				intPtr3 = Marshal.AllocCoTaskMem(Marshal.SizeOf(coauthidentity_X));
				Marshal.StructureToPtr(coauthidentity_X, intPtr3, false);
				COAUTHINFO_X64 coauthinfo_X = new COAUTHINFO_X64(RpcAuthent.WinNT, RpcAuthor.None, null, RpcLevel.Default, RpcImpers.Impersonate, intPtr3);
				intPtr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(coauthinfo_X));
				Marshal.StructureToPtr(coauthinfo_X, intPtr2, false);
				COSERVERINFO_X64 coserverinfo_X = new COSERVERINFO_X64(server, intPtr2);
				int num = UnsafeNativeMethods.CoCreateInstanceEx(ref guid, IntPtr.Zero, 16, coserverinfo_X, 1, array);
				if (num == -2147221164)
				{
					throw new Exception(SR.GetString("Make_sure_remote_server_is_enabled_for_config_access"));
				}
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				if (array[0].hr < 0)
				{
					Marshal.ThrowExceptionForHR(array[0].hr);
				}
				num = UnsafeNativeMethods.CoSetProxyBlanket(array[0].pItf, RpcAuthent.WinNT, RpcAuthor.None, null, RpcLevel.Default, RpcImpers.Impersonate, intPtr3, 0);
				if (num < 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				remoteWebConfigurationHostServer = (IRemoteWebConfigurationHostServer)Marshal.GetObjectForIUnknown(array[0].pItf);
			}
			finally
			{
				if (array[0].pItf != IntPtr.Zero)
				{
					Marshal.Release(array[0].pItf);
					array[0].pItf = IntPtr.Zero;
				}
				array[0].piid = IntPtr.Zero;
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.DestroyStructure(intPtr2, typeof(COAUTHINFO_X64));
					Marshal.FreeCoTaskMem(intPtr2);
				}
				if (intPtr3 != IntPtr.Zero)
				{
					Marshal.DestroyStructure(intPtr3, typeof(COAUTHIDENTITY_X64));
					Marshal.FreeCoTaskMem(intPtr3);
				}
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
			return remoteWebConfigurationHostServer;
		}

		// Token: 0x040019B7 RID: 6583
		private const string KEY_MACHINE = "MACHINE";

		// Token: 0x040019B8 RID: 6584
		private static object s_version = new object();

		// Token: 0x040019B9 RID: 6585
		private string _Server;

		// Token: 0x040019BA RID: 6586
		private string _Username;

		// Token: 0x040019BB RID: 6587
		private string _Domain;

		// Token: 0x040019BC RID: 6588
		private string _Password;

		// Token: 0x040019BD RID: 6589
		private WindowsIdentity _Identity;

		// Token: 0x040019BE RID: 6590
		private Hashtable _PathMap;

		// Token: 0x040019BF RID: 6591
		private string _ConfigPath;
	}
}
