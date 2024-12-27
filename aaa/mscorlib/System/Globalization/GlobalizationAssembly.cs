using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace System.Globalization
{
	// Token: 0x0200039F RID: 927
	internal sealed class GlobalizationAssembly
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x0600256C RID: 9580 RVA: 0x00069004 File Offset: 0x00068004
		internal static GlobalizationAssembly DefaultInstance
		{
			get
			{
				if (GlobalizationAssembly.m_defaultInstance == null)
				{
					throw new TypeLoadException("Failure has occurred while loading a type.");
				}
				return GlobalizationAssembly.m_defaultInstance;
			}
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x00069020 File Offset: 0x00068020
		internal static GlobalizationAssembly GetGlobalizationAssembly(Assembly assembly)
		{
			GlobalizationAssembly globalizationAssembly;
			if ((globalizationAssembly = (GlobalizationAssembly)GlobalizationAssembly.m_assemblyHash[assembly]) == null)
			{
				RuntimeHelpers.TryCode tryCode = new RuntimeHelpers.TryCode(GlobalizationAssembly.CreateGlobalizationAssembly);
				RuntimeHelpers.ExecuteCodeWithLock(typeof(CultureTableRecord), tryCode, assembly);
				globalizationAssembly = (GlobalizationAssembly)GlobalizationAssembly.m_assemblyHash[assembly];
			}
			return globalizationAssembly;
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x00069074 File Offset: 0x00068074
		[PrePrepareMethod]
		private static void CreateGlobalizationAssembly(object assem)
		{
			Assembly assembly = (Assembly)assem;
			if ((GlobalizationAssembly)GlobalizationAssembly.m_assemblyHash[assembly] == null)
			{
				GlobalizationAssembly globalizationAssembly = new GlobalizationAssembly();
				globalizationAssembly.pNativeGlobalizationAssembly = GlobalizationAssembly.nativeCreateGlobalizationAssembly(assembly);
				Thread.MemoryBarrier();
				GlobalizationAssembly.m_assemblyHash[assembly] = globalizationAssembly;
			}
		}

		// Token: 0x0600256F RID: 9583
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* _nativeCreateGlobalizationAssembly(Assembly assembly);

		// Token: 0x06002570 RID: 9584 RVA: 0x000690BF File Offset: 0x000680BF
		private unsafe static void* nativeCreateGlobalizationAssembly(Assembly assembly)
		{
			return GlobalizationAssembly._nativeCreateGlobalizationAssembly(assembly.InternalAssembly);
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000690CC File Offset: 0x000680CC
		internal GlobalizationAssembly()
		{
			this.compareInfoCache = new Hashtable(4);
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000690E0 File Offset: 0x000680E0
		internal unsafe static byte* GetGlobalizationResourceBytePtr(Assembly assembly, string tableName)
		{
			Stream manifestResourceStream = assembly.GetManifestResourceStream(tableName);
			UnmanagedMemoryStream unmanagedMemoryStream = manifestResourceStream as UnmanagedMemoryStream;
			if (unmanagedMemoryStream != null)
			{
				byte* positionPointer = unmanagedMemoryStream.PositionPointer;
				if (positionPointer != null)
				{
					return positionPointer;
				}
			}
			throw new ExecutionEngineException();
		}

		// Token: 0x040010D7 RID: 4311
		private static Hashtable m_assemblyHash = Hashtable.Synchronized(new Hashtable(4));

		// Token: 0x040010D8 RID: 4312
		internal static GlobalizationAssembly m_defaultInstance = GlobalizationAssembly.GetGlobalizationAssembly(Assembly.GetAssembly(typeof(GlobalizationAssembly)));

		// Token: 0x040010D9 RID: 4313
		internal Hashtable compareInfoCache;

		// Token: 0x040010DA RID: 4314
		internal unsafe void* pNativeGlobalizationAssembly;
	}
}
