using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000503 RID: 1283
	[ComVisible(true)]
	public class RuntimeEnvironment
	{
		// Token: 0x06003261 RID: 12897
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetModuleFileName();

		// Token: 0x06003262 RID: 12898
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetDeveloperPath();

		// Token: 0x06003263 RID: 12899
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetHostBindingFile();

		// Token: 0x06003264 RID: 12900
		[DllImport("mscoree.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		private static extern int GetCORVersion(StringBuilder sb, int BufferLength, ref int retLength);

		// Token: 0x06003265 RID: 12901
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool FromGlobalAccessCache(Assembly a);

		// Token: 0x06003266 RID: 12902 RVA: 0x000AB0B8 File Offset: 0x000AA0B8
		public static string GetSystemVersion()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			int num = 0;
			if (RuntimeEnvironment.GetCORVersion(stringBuilder, 256, ref num) == 0)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x000AB0EC File Offset: 0x000AA0EC
		public static string GetRuntimeDirectory()
		{
			string runtimeDirectoryImpl = RuntimeEnvironment.GetRuntimeDirectoryImpl();
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, runtimeDirectoryImpl).Demand();
			return runtimeDirectoryImpl;
		}

		// Token: 0x06003268 RID: 12904
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetRuntimeDirectoryImpl();

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06003269 RID: 12905 RVA: 0x000AB10C File Offset: 0x000AA10C
		public static string SystemConfigurationFile
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				stringBuilder.Append(RuntimeEnvironment.GetRuntimeDirectory());
				stringBuilder.Append(AppDomainSetup.RuntimeConfigurationFile);
				string text = stringBuilder.ToString();
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
				return text;
			}
		}
	}
}
