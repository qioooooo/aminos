using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x0200029E RID: 670
	[ComVisible(true)]
	public sealed class Debugger
	{
		// Token: 0x06001ABD RID: 6845 RVA: 0x000469E8 File Offset: 0x000459E8
		public static void Break()
		{
			if (!Debugger.IsDebuggerAttached())
			{
				try
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				}
				catch (SecurityException)
				{
					return;
				}
			}
			Debugger.BreakInternal();
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x00046A24 File Offset: 0x00045A24
		private static void BreakCanThrow()
		{
			if (!Debugger.IsDebuggerAttached())
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			}
			Debugger.BreakInternal();
		}

		// Token: 0x06001ABF RID: 6847
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void BreakInternal();

		// Token: 0x06001AC0 RID: 6848 RVA: 0x00046A40 File Offset: 0x00045A40
		public static bool Launch()
		{
			if (Debugger.IsDebuggerAttached())
			{
				return true;
			}
			try
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			}
			catch (SecurityException)
			{
				return false;
			}
			return Debugger.LaunchInternal();
		}

		// Token: 0x06001AC1 RID: 6849
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool LaunchInternal();

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x00046A80 File Offset: 0x00045A80
		public static bool IsAttached
		{
			get
			{
				return Debugger.IsDebuggerAttached();
			}
		}

		// Token: 0x06001AC3 RID: 6851
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsDebuggerAttached();

		// Token: 0x06001AC4 RID: 6852
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Log(int level, string category, string message);

		// Token: 0x06001AC5 RID: 6853
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsLogging();

		// Token: 0x04000A01 RID: 2561
		public static readonly string DefaultCategory;
	}
}
