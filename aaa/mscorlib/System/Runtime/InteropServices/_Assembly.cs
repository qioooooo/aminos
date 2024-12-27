using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002C7 RID: 711
	[TypeLibImportClass(typeof(Assembly))]
	[Guid("17156360-2f1a-384a-bc52-fde93c215c5b")]
	[CLSCompliant(false)]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface _Assembly
	{
		// Token: 0x06001BC0 RID: 7104
		string ToString();

		// Token: 0x06001BC1 RID: 7105
		bool Equals(object other);

		// Token: 0x06001BC2 RID: 7106
		int GetHashCode();

		// Token: 0x06001BC3 RID: 7107
		Type GetType();

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001BC4 RID: 7108
		string CodeBase { get; }

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001BC5 RID: 7109
		string EscapedCodeBase { get; }

		// Token: 0x06001BC6 RID: 7110
		AssemblyName GetName();

		// Token: 0x06001BC7 RID: 7111
		AssemblyName GetName(bool copiedName);

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001BC8 RID: 7112
		string FullName { get; }

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001BC9 RID: 7113
		MethodInfo EntryPoint { get; }

		// Token: 0x06001BCA RID: 7114
		Type GetType(string name);

		// Token: 0x06001BCB RID: 7115
		Type GetType(string name, bool throwOnError);

		// Token: 0x06001BCC RID: 7116
		Type[] GetExportedTypes();

		// Token: 0x06001BCD RID: 7117
		Type[] GetTypes();

		// Token: 0x06001BCE RID: 7118
		Stream GetManifestResourceStream(Type type, string name);

		// Token: 0x06001BCF RID: 7119
		Stream GetManifestResourceStream(string name);

		// Token: 0x06001BD0 RID: 7120
		FileStream GetFile(string name);

		// Token: 0x06001BD1 RID: 7121
		FileStream[] GetFiles();

		// Token: 0x06001BD2 RID: 7122
		FileStream[] GetFiles(bool getResourceModules);

		// Token: 0x06001BD3 RID: 7123
		string[] GetManifestResourceNames();

		// Token: 0x06001BD4 RID: 7124
		ManifestResourceInfo GetManifestResourceInfo(string resourceName);

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001BD5 RID: 7125
		string Location { get; }

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001BD6 RID: 7126
		Evidence Evidence { get; }

		// Token: 0x06001BD7 RID: 7127
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001BD8 RID: 7128
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001BD9 RID: 7129
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06001BDA RID: 7130
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void GetObjectData(SerializationInfo info, StreamingContext context);

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06001BDB RID: 7131
		// (remove) Token: 0x06001BDC RID: 7132
		event ModuleResolveEventHandler ModuleResolve;

		// Token: 0x06001BDD RID: 7133
		Type GetType(string name, bool throwOnError, bool ignoreCase);

		// Token: 0x06001BDE RID: 7134
		Assembly GetSatelliteAssembly(CultureInfo culture);

		// Token: 0x06001BDF RID: 7135
		Assembly GetSatelliteAssembly(CultureInfo culture, Version version);

		// Token: 0x06001BE0 RID: 7136
		Module LoadModule(string moduleName, byte[] rawModule);

		// Token: 0x06001BE1 RID: 7137
		Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore);

		// Token: 0x06001BE2 RID: 7138
		object CreateInstance(string typeName);

		// Token: 0x06001BE3 RID: 7139
		object CreateInstance(string typeName, bool ignoreCase);

		// Token: 0x06001BE4 RID: 7140
		object CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes);

		// Token: 0x06001BE5 RID: 7141
		Module[] GetLoadedModules();

		// Token: 0x06001BE6 RID: 7142
		Module[] GetLoadedModules(bool getResourceModules);

		// Token: 0x06001BE7 RID: 7143
		Module[] GetModules();

		// Token: 0x06001BE8 RID: 7144
		Module[] GetModules(bool getResourceModules);

		// Token: 0x06001BE9 RID: 7145
		Module GetModule(string name);

		// Token: 0x06001BEA RID: 7146
		AssemblyName[] GetReferencedAssemblies();

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001BEB RID: 7147
		bool GlobalAssemblyCache { get; }
	}
}
