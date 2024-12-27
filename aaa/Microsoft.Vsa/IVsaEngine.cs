using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;

namespace Microsoft.Vsa
{
	// Token: 0x02000002 RID: 2
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[Guid("E0C0FFE1-7eea-4ee2-b7e4-0080c7eb0b74")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IVsaEngine
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		// (set) Token: 0x06000002 RID: 2
		IVsaSite Site
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3
		// (set) Token: 0x06000004 RID: 4
		string Name
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5
		// (set) Token: 0x06000006 RID: 6
		string RootMoniker
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7
		// (set) Token: 0x06000008 RID: 8
		string RootNamespace
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9
		// (set) Token: 0x0600000A RID: 10
		int LCID
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11
		// (set) Token: 0x0600000C RID: 12
		bool GenerateDebugInfo
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000D RID: 13
		// (set) Token: 0x0600000E RID: 14
		Evidence Evidence
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000F RID: 15
		IVsaItems Items
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000010 RID: 16
		bool IsDirty
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000011 RID: 17
		string Language
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000012 RID: 18
		string Version
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x06000013 RID: 19
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		object GetOption(string name);

		// Token: 0x06000014 RID: 20
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void SetOption(string name, object value);

		// Token: 0x06000015 RID: 21
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		bool Compile();

		// Token: 0x06000016 RID: 22
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Run();

		// Token: 0x06000017 RID: 23
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Reset();

		// Token: 0x06000018 RID: 24
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Close();

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000019 RID: 25
		bool IsRunning
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001A RID: 26
		bool IsCompiled
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x0600001B RID: 27
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void RevokeCache();

		// Token: 0x0600001C RID: 28
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void SaveSourceState(IVsaPersistSite site);

		// Token: 0x0600001D RID: 29
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void LoadSourceState(IVsaPersistSite site);

		// Token: 0x0600001E RID: 30
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void SaveCompiledState(out byte[] pe, out byte[] pdb);

		// Token: 0x0600001F RID: 31
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void InitNew();

		// Token: 0x06000020 RID: 32
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		bool IsValidIdentifier(string identifier);

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000021 RID: 33
		Assembly Assembly { get; }
	}
}
