using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace System.ServiceProcess.Telemetry
{
	// Token: 0x02000031 RID: 49
	internal static class ServiceProcessTraceLogger
	{
		// Token: 0x060000FE RID: 254 RVA: 0x00006078 File Offset: 0x00005078
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void TraceServiceProcessStart()
		{
			ServiceProcessTraceLogger.TraceServiceStartMethod traceServiceStartMethod = null;
			try
			{
				try
				{
					traceServiceStartMethod = ServiceProcessTraceLogger.GetMethod<ServiceProcessTraceLogger.TraceServiceStartMethod>("netfxperf.dll", "TraceServiceStart");
				}
				catch (EntryPointNotFoundException)
				{
				}
				try
				{
					if (traceServiceStartMethod != null)
					{
						traceServiceStartMethod();
					}
				}
				catch (TargetInvocationException)
				{
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000060D8 File Offset: 0x000050D8
		[SecurityCritical]
		private static TDelegate GetMethod<TDelegate>(string system32Module, string entryPoint) where TDelegate : class
		{
			try
			{
				ServiceProcessTraceLogger.ValidateGetMethodArgs<TDelegate>(ref system32Module, ref entryPoint);
			}
			catch (ArgumentException ex)
			{
				throw new EntryPointNotFoundException(string.Empty, ex);
			}
			catch (NotSupportedException ex2)
			{
				throw new EntryPointNotFoundException(string.Empty, ex2);
			}
			Type typeFromHandle = typeof(TDelegate);
			IntPtr intPtr = NativeMethods.LoadLibraryHelper.SecureLoadLibraryEx(system32Module, IntPtr.Zero, NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_SEARCH_SYSTEM32);
			if (intPtr == IntPtr.Zero)
			{
				throw new EntryPointNotFoundException("Failed to load " + system32Module, new Win32Exception());
			}
			IntPtr procAddress = NativeMethods.GetProcAddress(intPtr, entryPoint);
			if (procAddress == IntPtr.Zero)
			{
				throw new EntryPointNotFoundException("Failed to get entrypoint " + entryPoint + " from " + system32Module, new Win32Exception());
			}
			Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(procAddress, typeFromHandle);
			if (delegateForFunctionPointer == null)
			{
				throw new EntryPointNotFoundException("Failed to get managed delegate (" + typeFromHandle.Name + ") for function pointer " + entryPoint);
			}
			TDelegate tdelegate = delegateForFunctionPointer as TDelegate;
			if (tdelegate == null)
			{
				string text = string.Format("{0}!{1}", system32Module, entryPoint);
				throw new EntryPointNotFoundException("Delegate for " + text + " is not of type " + typeFromHandle.Name);
			}
			return tdelegate;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00006208 File Offset: 0x00005208
		private static void ValidateGetMethodArgs<TDelegate>(ref string system32Module, ref string entryPoint)
		{
			if (system32Module == null)
			{
				throw new ArgumentNullException("system32Module");
			}
			if (entryPoint == null)
			{
				throw new ArgumentNullException("entryPoint");
			}
			system32Module = system32Module.Trim();
			entryPoint = entryPoint.Trim();
			if (system32Module.Length == 0)
			{
				throw new ArgumentException("system32Module");
			}
			if (entryPoint.Length == 0)
			{
				throw new ArgumentException("entryPoint");
			}
			Type typeFromHandle = typeof(TDelegate);
			if (!typeof(Delegate).IsAssignableFrom(typeFromHandle))
			{
				throw new NotSupportedException(typeFromHandle.Name + " is not a Delegate");
			}
		}

		// Token: 0x04000226 RID: 550
		private const string TraceServiceStartMethodName = "TraceServiceStart";

		// Token: 0x04000227 RID: 551
		private const string TraceServiceStartSourceModule = "netfxperf.dll";

		// Token: 0x02000032 RID: 50
		// (Invoke) Token: 0x06000102 RID: 258
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		internal delegate void TraceServiceStartMethod();
	}
}
