using System;
using System.Threading;
using System.Web.Compilation;
using System.Web.Hosting;
using Microsoft.Win32;

namespace System.Web.Configuration
{
	// Token: 0x02000247 RID: 583
	internal static class ServerConfig
	{
		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001ED7 RID: 7895 RVA: 0x00089C10 File Offset: 0x00088C10
		internal static bool UseMetabase
		{
			get
			{
				if (ServerConfig.s_iisMajorVersion == 0)
				{
					int num;
					try
					{
						object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\InetStp", "MajorVersion", 0);
						num = ((value != null) ? ((int)value) : (-1));
					}
					catch (ArgumentException)
					{
						num = -1;
					}
					Interlocked.CompareExchange(ref ServerConfig.s_iisMajorVersion, num, 0);
				}
				return ServerConfig.s_iisMajorVersion <= 6;
			}
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x00089C78 File Offset: 0x00088C78
		internal static IServerConfig GetInstance()
		{
			if (ServerConfig.UseMetabase)
			{
				return MetabaseServerConfig.GetInstance();
			}
			return ProcessHostServerConfig.GetInstance();
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x00089C8C File Offset: 0x00088C8C
		internal static bool UseServerConfig
		{
			get
			{
				if (ServerConfig.s_useServerConfig == -1)
				{
					int num = 0;
					if (!HostingEnvironment.IsHosted)
					{
						num = 1;
					}
					else if (HostingEnvironment.ApplicationHost is ISAPIApplicationHost)
					{
						num = 1;
					}
					else if (HostingEnvironment.IsUnderIISProcess && !BuildManagerHost.InClientBuildManager)
					{
						num = 1;
					}
					Interlocked.CompareExchange(ref ServerConfig.s_useServerConfig, num, -1);
				}
				return ServerConfig.s_useServerConfig == 1;
			}
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x00089CE3 File Offset: 0x00088CE3
		// Note: this type is marked as 'beforefieldinit'.
		static ServerConfig()
		{
			/*
An exception occurred when decompiling this method (06001EDA)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.Configuration.ServerConfig::.cctor()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.YieldReturnDecompiler.TranslateFieldsToLocalAccess(List`1 newBody, FieldToVariableMap variableMap, ILVariable cachedThisField, Boolean calculateILSpans, Boolean fixLocals) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\YieldReturnDecompiler.cs:line 284
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 126
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x04001A1D RID: 6685
		private static int s_iisMajorVersion;

		// Token: 0x04001A1E RID: 6686
		private static int s_useServerConfig;
	}
}
