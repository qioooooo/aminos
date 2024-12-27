using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;

namespace System
{
	// Token: 0x02000054 RID: 84
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[Guid("05F696DC-2B29-3663-AD8B-C4389CF2A713")]
	public interface _AppDomain
	{
		// Token: 0x06000447 RID: 1095
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06000448 RID: 1096
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06000449 RID: 1097
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x0600044A RID: 1098
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x0600044B RID: 1099
		string ToString();

		// Token: 0x0600044C RID: 1100
		bool Equals(object other);

		// Token: 0x0600044D RID: 1101
		int GetHashCode();

		// Token: 0x0600044E RID: 1102
		Type GetType();

		// Token: 0x0600044F RID: 1103
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		object InitializeLifetimeService();

		// Token: 0x06000450 RID: 1104
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		object GetLifetimeService();

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000451 RID: 1105
		Evidence Evidence { get; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000452 RID: 1106
		// (remove) Token: 0x06000453 RID: 1107
		event EventHandler DomainUnload;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000454 RID: 1108
		// (remove) Token: 0x06000455 RID: 1109
		event AssemblyLoadEventHandler AssemblyLoad;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000456 RID: 1110
		// (remove) Token: 0x06000457 RID: 1111
		event EventHandler ProcessExit;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000458 RID: 1112
		// (remove) Token: 0x06000459 RID: 1113
		event ResolveEventHandler TypeResolve;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600045A RID: 1114
		// (remove) Token: 0x0600045B RID: 1115
		event ResolveEventHandler ResourceResolve;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600045C RID: 1116
		// (remove) Token: 0x0600045D RID: 1117
		event ResolveEventHandler AssemblyResolve;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600045E RID: 1118
		// (remove) Token: 0x0600045F RID: 1119
		event UnhandledExceptionEventHandler UnhandledException;

		// Token: 0x06000460 RID: 1120
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access);

		// Token: 0x06000461 RID: 1121
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir);

		// Token: 0x06000462 RID: 1122
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence);

		// Token: 0x06000463 RID: 1123
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

		// Token: 0x06000464 RID: 1124
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence);

		// Token: 0x06000465 RID: 1125
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

		// Token: 0x06000466 RID: 1126
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

		// Token: 0x06000467 RID: 1127
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

		// Token: 0x06000468 RID: 1128
		AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions, bool isSynchronized);

		// Token: 0x06000469 RID: 1129
		ObjectHandle CreateInstance(string assemblyName, string typeName);

		// Token: 0x0600046A RID: 1130
		ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName);

		// Token: 0x0600046B RID: 1131
		ObjectHandle CreateInstance(string assemblyName, string typeName, object[] activationAttributes);

		// Token: 0x0600046C RID: 1132
		ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, object[] activationAttributes);

		// Token: 0x0600046D RID: 1133
		ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes);

		// Token: 0x0600046E RID: 1134
		ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes);

		// Token: 0x0600046F RID: 1135
		Assembly Load(AssemblyName assemblyRef);

		// Token: 0x06000470 RID: 1136
		Assembly Load(string assemblyString);

		// Token: 0x06000471 RID: 1137
		Assembly Load(byte[] rawAssembly);

		// Token: 0x06000472 RID: 1138
		Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore);

		// Token: 0x06000473 RID: 1139
		Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence);

		// Token: 0x06000474 RID: 1140
		Assembly Load(AssemblyName assemblyRef, Evidence assemblySecurity);

		// Token: 0x06000475 RID: 1141
		Assembly Load(string assemblyString, Evidence assemblySecurity);

		// Token: 0x06000476 RID: 1142
		int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity);

		// Token: 0x06000477 RID: 1143
		int ExecuteAssembly(string assemblyFile);

		// Token: 0x06000478 RID: 1144
		int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity, string[] args);

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000479 RID: 1145
		string FriendlyName { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600047A RID: 1146
		string BaseDirectory { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600047B RID: 1147
		string RelativeSearchPath { get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600047C RID: 1148
		bool ShadowCopyFiles { get; }

		// Token: 0x0600047D RID: 1149
		Assembly[] GetAssemblies();

		// Token: 0x0600047E RID: 1150
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		void AppendPrivatePath(string path);

		// Token: 0x0600047F RID: 1151
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		void ClearPrivatePath();

		// Token: 0x06000480 RID: 1152
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		void SetShadowCopyPath(string s);

		// Token: 0x06000481 RID: 1153
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		void ClearShadowCopyPath();

		// Token: 0x06000482 RID: 1154
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		void SetCachePath(string s);

		// Token: 0x06000483 RID: 1155
		[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		void SetData(string name, object data);

		// Token: 0x06000484 RID: 1156
		object GetData(string name);

		// Token: 0x06000485 RID: 1157
		[SecurityPermission(SecurityAction.LinkDemand, ControlDomainPolicy = true)]
		void SetAppDomainPolicy(PolicyLevel domainPolicy);

		// Token: 0x06000486 RID: 1158
		void SetThreadPrincipal(IPrincipal principal);

		// Token: 0x06000487 RID: 1159
		void SetPrincipalPolicy(PrincipalPolicy policy);

		// Token: 0x06000488 RID: 1160
		void DoCallBack(CrossAppDomainDelegate theDelegate);

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000489 RID: 1161
		string DynamicDirectory { get; }
	}
}
