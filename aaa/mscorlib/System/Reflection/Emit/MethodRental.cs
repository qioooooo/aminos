using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection.Emit
{
	// Token: 0x02000822 RID: 2082
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_MethodRental))]
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class MethodRental : _MethodRental
	{
		// Token: 0x06004B0A RID: 19210 RVA: 0x001054F4 File Offset: 0x001044F4
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void SwapMethodBody(Type cls, int methodtoken, IntPtr rgIL, int methodSize, int flags)
		{
			if (methodSize <= 0 || methodSize >= 4128768)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadSizeForData"), "methodSize");
			}
			if (cls == null)
			{
				throw new ArgumentNullException("cls");
			}
			if (!(cls.Module is ModuleBuilder))
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NotDynamicModule"));
			}
			RuntimeType runtimeType;
			if (cls is TypeBuilder)
			{
				TypeBuilder typeBuilder = (TypeBuilder)cls;
				if (!typeBuilder.m_hasBeenCreated)
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("NotSupported_NotAllTypesAreBaked"), new object[] { typeBuilder.Name }));
				}
				runtimeType = typeBuilder.m_runtimeType;
			}
			else
			{
				runtimeType = cls as RuntimeType;
			}
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "cls");
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			if (runtimeType.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (runtimeType.Assembly.m_assemblyData)
				{
					MethodRental.SwapMethodBodyHelper(runtimeType, methodtoken, rgIL, methodSize, flags, ref stackCrawlMark);
					return;
				}
			}
			MethodRental.SwapMethodBodyHelper(runtimeType, methodtoken, rgIL, methodSize, flags, ref stackCrawlMark);
		}

		// Token: 0x06004B0B RID: 19211
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SwapMethodBodyHelper(RuntimeType cls, int methodtoken, IntPtr rgIL, int methodSize, int flags, ref StackCrawlMark stackMark);

		// Token: 0x06004B0C RID: 19212 RVA: 0x00105614 File Offset: 0x00104614
		private MethodRental()
		{
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x0010561C File Offset: 0x0010461C
		void _MethodRental.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x00105623 File Offset: 0x00104623
		void _MethodRental.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x0010562A File Offset: 0x0010462A
		void _MethodRental.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x00105631 File Offset: 0x00104631
		void _MethodRental.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002632 RID: 9778
		public const int JitOnDemand = 0;

		// Token: 0x04002633 RID: 9779
		public const int JitImmediate = 1;
	}
}
