using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004FE RID: 1278
	[SuppressUnmanagedCodeSecurity]
	public static class Marshal
	{
		// Token: 0x06003198 RID: 12696 RVA: 0x000A9EC8 File Offset: 0x000A8EC8
		private static bool IsWin32Atom(IntPtr ptr)
		{
			long num = (long)ptr;
			return 0L == (num & (long)Marshal.HIWORDMASK);
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x000A9EEC File Offset: 0x000A8EEC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private static bool IsNotWin32Atom(IntPtr ptr)
		{
			long num = (long)ptr;
			return 0L != (num & (long)Marshal.HIWORDMASK);
		}

		// Token: 0x0600319A RID: 12698
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetSystemMaxDBCSCharSize();

		// Token: 0x0600319B RID: 12699 RVA: 0x000A9F14 File Offset: 0x000A8F14
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static string PtrToStringAnsi(IntPtr ptr)
		{
			if (Win32Native.NULL == ptr)
			{
				return null;
			}
			if (Marshal.IsWin32Atom(ptr))
			{
				return null;
			}
			int num = Win32Native.lstrlenA(ptr);
			if (num == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(num);
			Win32Native.CopyMemoryAnsi(stringBuilder, ptr, new IntPtr(1 + num));
			return stringBuilder.ToString();
		}

		// Token: 0x0600319C RID: 12700
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string PtrToStringAnsi(IntPtr ptr, int len);

		// Token: 0x0600319D RID: 12701
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string PtrToStringUni(IntPtr ptr, int len);

		// Token: 0x0600319E RID: 12702 RVA: 0x000A9F66 File Offset: 0x000A8F66
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static string PtrToStringAuto(IntPtr ptr, int len)
		{
			if (Marshal.SystemDefaultCharSize != 1)
			{
				return Marshal.PtrToStringUni(ptr, len);
			}
			return Marshal.PtrToStringAnsi(ptr, len);
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x000A9F80 File Offset: 0x000A8F80
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static string PtrToStringUni(IntPtr ptr)
		{
			if (Win32Native.NULL == ptr)
			{
				return null;
			}
			if (Marshal.IsWin32Atom(ptr))
			{
				return null;
			}
			int num = Win32Native.lstrlenW(ptr);
			StringBuilder stringBuilder = new StringBuilder(num);
			Win32Native.CopyMemoryUni(stringBuilder, ptr, new IntPtr(2 * (1 + num)));
			return stringBuilder.ToString();
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x000A9FCC File Offset: 0x000A8FCC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static string PtrToStringAuto(IntPtr ptr)
		{
			if (Win32Native.NULL == ptr)
			{
				return null;
			}
			if (Marshal.IsWin32Atom(ptr))
			{
				return null;
			}
			int num = Win32Native.lstrlen(ptr);
			StringBuilder stringBuilder = new StringBuilder(num);
			Win32Native.lstrcpy(stringBuilder, ptr);
			return stringBuilder.ToString();
		}

		// Token: 0x060031A1 RID: 12705
		[ComVisible(true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int SizeOf(object structure);

		// Token: 0x060031A2 RID: 12706
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int SizeOf(Type t);

		// Token: 0x060031A3 RID: 12707 RVA: 0x000AA010 File Offset: 0x000A9010
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr OffsetOf(Type t, string fieldName)
		{
			if (t == null)
			{
				throw new ArgumentNullException("t");
			}
			FieldInfo field = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_OffsetOfFieldNotFound", new object[] { t.FullName }), "fieldName");
			}
			if (!(field is RuntimeFieldInfo))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeFieldInfo"), "fieldName");
			}
			return Marshal.OffsetOfHelper(((RuntimeFieldInfo)field).GetFieldHandle().Value);
		}

		// Token: 0x060031A4 RID: 12708
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr OffsetOfHelper(IntPtr f);

		// Token: 0x060031A5 RID: 12709
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr UnsafeAddrOfPinnedArrayElement(Array arr, int index);

		// Token: 0x060031A6 RID: 12710 RVA: 0x000AA093 File Offset: 0x000A9093
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(int[] source, int startIndex, IntPtr destination, int length)
		{
			Marshal.CopyToNative(source, startIndex, destination, length);
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x000AA09E File Offset: 0x000A909E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(char[] source, int startIndex, IntPtr destination, int length)
		{
			Marshal.CopyToNative(source, startIndex, destination, length);
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x000AA0A9 File Offset: 0x000A90A9
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(short[] source, int startIndex, IntPtr destination, int length)
		{
			Marshal.CopyToNative(source, startIndex, destination, length);
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x000AA0B4 File Offset: 0x000A90B4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(long[] source, int startIndex, IntPtr destination, int length)
		{
			Marshal.CopyToNative(source, startIndex, destination, length);
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x000AA0BF File Offset: 0x000A90BF
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(float[] source, int startIndex, IntPtr destination, int length)
		{
			Marshal.CopyToNative(source, startIndex, destination, length);
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x000AA0CA File Offset: 0x000A90CA
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(double[] source, int startIndex, IntPtr destination, int length)
		{
			Marshal.CopyToNative(source, startIndex, destination, length);
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x000AA0D5 File Offset: 0x000A90D5
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(byte[] source, int startIndex, IntPtr destination, int length)
		{
			Marshal.CopyToNative(source, startIndex, destination, length);
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x000AA0E0 File Offset: 0x000A90E0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(IntPtr[] source, int startIndex, IntPtr destination, int length)
		{
			Marshal.CopyToNative(source, startIndex, destination, length);
		}

		// Token: 0x060031AE RID: 12718
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyToNative(object source, int startIndex, IntPtr destination, int length);

		// Token: 0x060031AF RID: 12719 RVA: 0x000AA0EB File Offset: 0x000A90EB
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(IntPtr source, int[] destination, int startIndex, int length)
		{
			Marshal.CopyToManaged(source, destination, startIndex, length);
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000AA0F6 File Offset: 0x000A90F6
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(IntPtr source, char[] destination, int startIndex, int length)
		{
			Marshal.CopyToManaged(source, destination, startIndex, length);
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x000AA101 File Offset: 0x000A9101
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(IntPtr source, short[] destination, int startIndex, int length)
		{
			Marshal.CopyToManaged(source, destination, startIndex, length);
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x000AA10C File Offset: 0x000A910C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(IntPtr source, long[] destination, int startIndex, int length)
		{
			Marshal.CopyToManaged(source, destination, startIndex, length);
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x000AA117 File Offset: 0x000A9117
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(IntPtr source, float[] destination, int startIndex, int length)
		{
			Marshal.CopyToManaged(source, destination, startIndex, length);
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x000AA122 File Offset: 0x000A9122
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(IntPtr source, double[] destination, int startIndex, int length)
		{
			Marshal.CopyToManaged(source, destination, startIndex, length);
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x000AA12D File Offset: 0x000A912D
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(IntPtr source, byte[] destination, int startIndex, int length)
		{
			Marshal.CopyToManaged(source, destination, startIndex, length);
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x000AA138 File Offset: 0x000A9138
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Copy(IntPtr source, IntPtr[] destination, int startIndex, int length)
		{
			Marshal.CopyToManaged(source, destination, startIndex, length);
		}

		// Token: 0x060031B7 RID: 12727
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyToManaged(IntPtr source, object destination, int startIndex, int length);

		// Token: 0x060031B8 RID: 12728
		[DllImport("mscoree.dll", EntryPoint = "ND_RU1")]
		public static extern byte ReadByte([MarshalAs(UnmanagedType.AsAny)] [In] object ptr, int ofs);

		// Token: 0x060031B9 RID: 12729
		[DllImport("mscoree.dll", EntryPoint = "ND_RU1")]
		public static extern byte ReadByte(IntPtr ptr, int ofs);

		// Token: 0x060031BA RID: 12730 RVA: 0x000AA143 File Offset: 0x000A9143
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static byte ReadByte(IntPtr ptr)
		{
			return Marshal.ReadByte(ptr, 0);
		}

		// Token: 0x060031BB RID: 12731
		[DllImport("mscoree.dll", EntryPoint = "ND_RI2")]
		public static extern short ReadInt16([MarshalAs(UnmanagedType.AsAny)] [In] object ptr, int ofs);

		// Token: 0x060031BC RID: 12732
		[DllImport("mscoree.dll", EntryPoint = "ND_RI2")]
		public static extern short ReadInt16(IntPtr ptr, int ofs);

		// Token: 0x060031BD RID: 12733 RVA: 0x000AA14C File Offset: 0x000A914C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static short ReadInt16(IntPtr ptr)
		{
			return Marshal.ReadInt16(ptr, 0);
		}

		// Token: 0x060031BE RID: 12734
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("mscoree.dll", EntryPoint = "ND_RI4")]
		public static extern int ReadInt32([MarshalAs(UnmanagedType.AsAny)] [In] object ptr, int ofs);

		// Token: 0x060031BF RID: 12735
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("mscoree.dll", EntryPoint = "ND_RI4")]
		public static extern int ReadInt32(IntPtr ptr, int ofs);

		// Token: 0x060031C0 RID: 12736 RVA: 0x000AA155 File Offset: 0x000A9155
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static int ReadInt32(IntPtr ptr)
		{
			return Marshal.ReadInt32(ptr, 0);
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x000AA15E File Offset: 0x000A915E
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr ReadIntPtr([MarshalAs(UnmanagedType.AsAny)] [In] object ptr, int ofs)
		{
			return (IntPtr)Marshal.ReadInt32(ptr, ofs);
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x000AA16C File Offset: 0x000A916C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr ReadIntPtr(IntPtr ptr, int ofs)
		{
			return (IntPtr)Marshal.ReadInt32(ptr, ofs);
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x000AA17A File Offset: 0x000A917A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr ReadIntPtr(IntPtr ptr)
		{
			return (IntPtr)Marshal.ReadInt32(ptr, 0);
		}

		// Token: 0x060031C4 RID: 12740
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("mscoree.dll", EntryPoint = "ND_RI8")]
		public static extern long ReadInt64([MarshalAs(UnmanagedType.AsAny)] [In] object ptr, int ofs);

		// Token: 0x060031C5 RID: 12741
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("mscoree.dll", EntryPoint = "ND_RI8")]
		public static extern long ReadInt64(IntPtr ptr, int ofs);

		// Token: 0x060031C6 RID: 12742 RVA: 0x000AA188 File Offset: 0x000A9188
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static long ReadInt64(IntPtr ptr)
		{
			return Marshal.ReadInt64(ptr, 0);
		}

		// Token: 0x060031C7 RID: 12743
		[DllImport("mscoree.dll", EntryPoint = "ND_WU1")]
		public static extern void WriteByte(IntPtr ptr, int ofs, byte val);

		// Token: 0x060031C8 RID: 12744
		[DllImport("mscoree.dll", EntryPoint = "ND_WU1")]
		public static extern void WriteByte([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object ptr, int ofs, byte val);

		// Token: 0x060031C9 RID: 12745 RVA: 0x000AA191 File Offset: 0x000A9191
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void WriteByte(IntPtr ptr, byte val)
		{
			Marshal.WriteByte(ptr, 0, val);
		}

		// Token: 0x060031CA RID: 12746
		[DllImport("mscoree.dll", EntryPoint = "ND_WI2")]
		public static extern void WriteInt16(IntPtr ptr, int ofs, short val);

		// Token: 0x060031CB RID: 12747
		[DllImport("mscoree.dll", EntryPoint = "ND_WI2")]
		public static extern void WriteInt16([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object ptr, int ofs, short val);

		// Token: 0x060031CC RID: 12748 RVA: 0x000AA19B File Offset: 0x000A919B
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void WriteInt16(IntPtr ptr, short val)
		{
			Marshal.WriteInt16(ptr, 0, val);
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x000AA1A5 File Offset: 0x000A91A5
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void WriteInt16(IntPtr ptr, int ofs, char val)
		{
			Marshal.WriteInt16(ptr, ofs, (short)val);
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x000AA1B0 File Offset: 0x000A91B0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void WriteInt16([In] [Out] object ptr, int ofs, char val)
		{
			Marshal.WriteInt16(ptr, ofs, (short)val);
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x000AA1BB File Offset: 0x000A91BB
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void WriteInt16(IntPtr ptr, char val)
		{
			Marshal.WriteInt16(ptr, 0, (short)val);
		}

		// Token: 0x060031D0 RID: 12752
		[DllImport("mscoree.dll", EntryPoint = "ND_WI4")]
		public static extern void WriteInt32(IntPtr ptr, int ofs, int val);

		// Token: 0x060031D1 RID: 12753
		[DllImport("mscoree.dll", EntryPoint = "ND_WI4")]
		public static extern void WriteInt32([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object ptr, int ofs, int val);

		// Token: 0x060031D2 RID: 12754 RVA: 0x000AA1C6 File Offset: 0x000A91C6
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void WriteInt32(IntPtr ptr, int val)
		{
			Marshal.WriteInt32(ptr, 0, val);
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x000AA1D0 File Offset: 0x000A91D0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void WriteIntPtr(IntPtr ptr, int ofs, IntPtr val)
		{
			Marshal.WriteInt32(ptr, ofs, (int)val);
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x000AA1DF File Offset: 0x000A91DF
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void WriteIntPtr([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object ptr, int ofs, IntPtr val)
		{
			Marshal.WriteInt32(ptr, ofs, (int)val);
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x000AA1EE File Offset: 0x000A91EE
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void WriteIntPtr(IntPtr ptr, IntPtr val)
		{
			Marshal.WriteInt32(ptr, 0, (int)val);
		}

		// Token: 0x060031D6 RID: 12758
		[DllImport("mscoree.dll", EntryPoint = "ND_WI8")]
		public static extern void WriteInt64(IntPtr ptr, int ofs, long val);

		// Token: 0x060031D7 RID: 12759
		[DllImport("mscoree.dll", EntryPoint = "ND_WI8")]
		public static extern void WriteInt64([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object ptr, int ofs, long val);

		// Token: 0x060031D8 RID: 12760 RVA: 0x000AA1FD File Offset: 0x000A91FD
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void WriteInt64(IntPtr ptr, long val)
		{
			Marshal.WriteInt64(ptr, 0, val);
		}

		// Token: 0x060031D9 RID: 12761
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetLastWin32Error();

		// Token: 0x060031DA RID: 12762
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetLastWin32Error(int error);

		// Token: 0x060031DB RID: 12763 RVA: 0x000AA208 File Offset: 0x000A9208
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static int GetHRForLastWin32Error()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (((long)lastWin32Error & (long)((ulong)(-2147483648))) == (long)((ulong)(-2147483648)))
			{
				return lastWin32Error;
			}
			return (lastWin32Error & 65535) | -2147024896;
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000AA23C File Offset: 0x000A923C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Prelink(MethodInfo m)
		{
			if (m == null)
			{
				throw new ArgumentNullException("m");
			}
			if (!(m is RuntimeMethodInfo))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"));
			}
			Marshal.InternalPrelink(m.MethodHandle.Value);
		}

		// Token: 0x060031DD RID: 12765
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalPrelink(IntPtr m);

		// Token: 0x060031DE RID: 12766 RVA: 0x000AA284 File Offset: 0x000A9284
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void PrelinkAll(Type c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			MethodInfo[] methods = c.GetMethods();
			if (methods != null)
			{
				for (int i = 0; i < methods.Length; i++)
				{
					Marshal.Prelink(methods[i]);
				}
			}
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x000AA2C0 File Offset: 0x000A92C0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static int NumParamBytes(MethodInfo m)
		{
			if (m == null)
			{
				throw new ArgumentNullException("m");
			}
			if (!(m is RuntimeMethodInfo))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"));
			}
			return Marshal.InternalNumParamBytes(m.GetMethodHandle().Value);
		}

		// Token: 0x060031E0 RID: 12768
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int InternalNumParamBytes(IntPtr m);

		// Token: 0x060031E1 RID: 12769
		[ComVisible(true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetExceptionPointers();

		// Token: 0x060031E2 RID: 12770
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetExceptionCode();

		// Token: 0x060031E3 RID: 12771
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[ComVisible(true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StructureToPtr(object structure, IntPtr ptr, bool fDeleteOld);

		// Token: 0x060031E4 RID: 12772 RVA: 0x000AA306 File Offset: 0x000A9306
		[ComVisible(true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void PtrToStructure(IntPtr ptr, object structure)
		{
			Marshal.PtrToStructureHelper(ptr, structure, false);
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x000AA310 File Offset: 0x000A9310
		[ComVisible(true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static object PtrToStructure(IntPtr ptr, Type structureType)
		{
			if (ptr == Win32Native.NULL)
			{
				return null;
			}
			if (structureType == null)
			{
				throw new ArgumentNullException("structureType");
			}
			if (structureType.IsGenericType)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "structureType");
			}
			object obj = Activator.InternalCreateInstanceWithNoMemberAccessCheck(structureType, true);
			Marshal.PtrToStructureHelper(ptr, obj, true);
			return obj;
		}

		// Token: 0x060031E6 RID: 12774
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PtrToStructureHelper(IntPtr ptr, object structure, bool allowValueClasses);

		// Token: 0x060031E7 RID: 12775
		[ComVisible(true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DestroyStructure(IntPtr ptr, Type structuretype);

		// Token: 0x060031E8 RID: 12776 RVA: 0x000AA368 File Offset: 0x000A9368
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr GetHINSTANCE(Module m)
		{
			if (m == null)
			{
				throw new ArgumentNullException("m");
			}
			return m.GetHINSTANCE();
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x000AA37E File Offset: 0x000A937E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void ThrowExceptionForHR(int errorCode)
		{
			if (errorCode < 0)
			{
				Marshal.ThrowExceptionForHRInternal(errorCode, Win32Native.NULL);
			}
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x000AA38F File Offset: 0x000A938F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void ThrowExceptionForHR(int errorCode, IntPtr errorInfo)
		{
			if (errorCode < 0)
			{
				Marshal.ThrowExceptionForHRInternal(errorCode, errorInfo);
			}
		}

		// Token: 0x060031EB RID: 12779
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ThrowExceptionForHRInternal(int errorCode, IntPtr errorInfo);

		// Token: 0x060031EC RID: 12780 RVA: 0x000AA39C File Offset: 0x000A939C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Exception GetExceptionForHR(int errorCode)
		{
			if (errorCode < 0)
			{
				return Marshal.GetExceptionForHRInternal(errorCode, Win32Native.NULL);
			}
			return null;
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x000AA3AF File Offset: 0x000A93AF
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Exception GetExceptionForHR(int errorCode, IntPtr errorInfo)
		{
			if (errorCode < 0)
			{
				return Marshal.GetExceptionForHRInternal(errorCode, errorInfo);
			}
			return null;
		}

		// Token: 0x060031EE RID: 12782
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Exception GetExceptionForHRInternal(int errorCode, IntPtr errorInfo);

		// Token: 0x060031EF RID: 12783
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetHRForException(Exception e);

		// Token: 0x060031F0 RID: 12784
		[Obsolete("The GetUnmanagedThunkForManagedMethodPtr method has been deprecated and will be removed in a future release.", false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetUnmanagedThunkForManagedMethodPtr(IntPtr pfnMethodToWrap, IntPtr pbSignature, int cbSignature);

		// Token: 0x060031F1 RID: 12785
		[Obsolete("The GetManagedThunkForUnmanagedMethodPtr method has been deprecated and will be removed in a future release.", false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetManagedThunkForUnmanagedMethodPtr(IntPtr pfnMethodToWrap, IntPtr pbSignature, int cbSignature);

		// Token: 0x060031F2 RID: 12786 RVA: 0x000AA3BE File Offset: 0x000A93BE
		[Obsolete("The GetThreadFromFiberCookie method has been deprecated.  Use the hosting API to perform this operation.", false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Thread GetThreadFromFiberCookie(int cookie)
		{
			if (cookie == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ArgumentZero"), "cookie");
			}
			return Marshal.InternalGetThreadFromFiberCookie(cookie);
		}

		// Token: 0x060031F3 RID: 12787
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Thread InternalGetThreadFromFiberCookie(int cookie);

		// Token: 0x060031F4 RID: 12788 RVA: 0x000AA3E0 File Offset: 0x000A93E0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr AllocHGlobal(IntPtr cb)
		{
			IntPtr intPtr = Win32Native.LocalAlloc_NoSafeHandle(0, cb);
			if (intPtr == Win32Native.NULL)
			{
				throw new OutOfMemoryException();
			}
			return intPtr;
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x000AA409 File Offset: 0x000A9409
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr AllocHGlobal(int cb)
		{
			return Marshal.AllocHGlobal((IntPtr)cb);
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x000AA416 File Offset: 0x000A9416
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void FreeHGlobal(IntPtr hglobal)
		{
			if (Marshal.IsNotWin32Atom(hglobal) && Win32Native.NULL != Win32Native.LocalFree(hglobal))
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x000AA43C File Offset: 0x000A943C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr ReAllocHGlobal(IntPtr pv, IntPtr cb)
		{
			IntPtr intPtr = Win32Native.LocalReAlloc(pv, cb, 2);
			if (intPtr == Win32Native.NULL)
			{
				throw new OutOfMemoryException();
			}
			return intPtr;
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x000AA468 File Offset: 0x000A9468
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr StringToHGlobalAnsi(string s)
		{
			if (s == null)
			{
				return Win32Native.NULL;
			}
			int num = (s.Length + 1) * Marshal.SystemMaxDBCSCharSize;
			if (num < s.Length)
			{
				throw new ArgumentOutOfRangeException("s");
			}
			IntPtr intPtr = new IntPtr(num);
			IntPtr intPtr2 = Win32Native.LocalAlloc_NoSafeHandle(0, intPtr);
			if (intPtr2 == Win32Native.NULL)
			{
				throw new OutOfMemoryException();
			}
			Win32Native.CopyMemoryAnsi(intPtr2, s, intPtr);
			return intPtr2;
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x000AA4D0 File Offset: 0x000A94D0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr StringToCoTaskMemAnsi(string s)
		{
			if (s == null)
			{
				return Win32Native.NULL;
			}
			int num = (s.Length + 1) * Marshal.SystemMaxDBCSCharSize;
			if (num < s.Length)
			{
				throw new ArgumentOutOfRangeException("s");
			}
			IntPtr intPtr = Win32Native.CoTaskMemAlloc(num);
			if (intPtr == Win32Native.NULL)
			{
				throw new OutOfMemoryException();
			}
			Win32Native.CopyMemoryAnsi(intPtr, s, new IntPtr(num));
			return intPtr;
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x000AA534 File Offset: 0x000A9534
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr StringToHGlobalUni(string s)
		{
			if (s == null)
			{
				return Win32Native.NULL;
			}
			int num = (s.Length + 1) * 2;
			if (num < s.Length)
			{
				throw new ArgumentOutOfRangeException("s");
			}
			IntPtr intPtr = new IntPtr(num);
			IntPtr intPtr2 = Win32Native.LocalAlloc_NoSafeHandle(0, intPtr);
			if (intPtr2 == Win32Native.NULL)
			{
				throw new OutOfMemoryException();
			}
			Win32Native.CopyMemoryUni(intPtr2, s, intPtr);
			return intPtr2;
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x000AA595 File Offset: 0x000A9595
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr StringToHGlobalAuto(string s)
		{
			if (Marshal.SystemDefaultCharSize != 1)
			{
				return Marshal.StringToHGlobalUni(s);
			}
			return Marshal.StringToHGlobalAnsi(s);
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x000AA5AC File Offset: 0x000A95AC
		[Obsolete("Use System.Runtime.InteropServices.Marshal.GetTypeLibName(ITypeLib pTLB) instead. http://go.microsoft.com/fwlink/?linkid=14202&ID=0000011.", false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static string GetTypeLibName(UCOMITypeLib pTLB)
		{
			return Marshal.GetTypeLibName((ITypeLib)pTLB);
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x000AA5BC File Offset: 0x000A95BC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static string GetTypeLibName(ITypeLib typelib)
		{
			string text = null;
			string text2 = null;
			int num = 0;
			string text3 = null;
			if (typelib == null)
			{
				throw new ArgumentNullException("typelib");
			}
			typelib.GetDocumentation(-1, out text, out text2, out num, out text3);
			return text;
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x000AA5EF File Offset: 0x000A95EF
		[Obsolete("Use System.Runtime.InteropServices.Marshal.GetTypeLibGuid(ITypeLib pTLB) instead. http://go.microsoft.com/fwlink/?linkid=14202&ID=0000011.", false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Guid GetTypeLibGuid(UCOMITypeLib pTLB)
		{
			return Marshal.GetTypeLibGuid((ITypeLib)pTLB);
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x000AA5FC File Offset: 0x000A95FC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Guid GetTypeLibGuid(ITypeLib typelib)
		{
			Guid guid = default(Guid);
			Marshal.FCallGetTypeLibGuid(ref guid, typelib);
			return guid;
		}

		// Token: 0x06003200 RID: 12800
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallGetTypeLibGuid(ref Guid result, ITypeLib pTLB);

		// Token: 0x06003201 RID: 12801 RVA: 0x000AA61A File Offset: 0x000A961A
		[Obsolete("Use System.Runtime.InteropServices.Marshal.GetTypeLibLcid(ITypeLib pTLB) instead. http://go.microsoft.com/fwlink/?linkid=14202&ID=0000011.", false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static int GetTypeLibLcid(UCOMITypeLib pTLB)
		{
			return Marshal.GetTypeLibLcid((ITypeLib)pTLB);
		}

		// Token: 0x06003202 RID: 12802
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetTypeLibLcid(ITypeLib typelib);

		// Token: 0x06003203 RID: 12803
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetTypeLibVersion(ITypeLib typeLibrary, out int major, out int minor);

		// Token: 0x06003204 RID: 12804 RVA: 0x000AA628 File Offset: 0x000A9628
		internal static Guid GetTypeInfoGuid(ITypeInfo typeInfo)
		{
			Guid guid = default(Guid);
			Marshal.FCallGetTypeInfoGuid(ref guid, typeInfo);
			return guid;
		}

		// Token: 0x06003205 RID: 12805
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallGetTypeInfoGuid(ref Guid result, ITypeInfo typeInfo);

		// Token: 0x06003206 RID: 12806 RVA: 0x000AA648 File Offset: 0x000A9648
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Guid GetTypeLibGuidForAssembly(Assembly asm)
		{
			Guid guid = default(Guid);
			Marshal.FCallGetTypeLibGuidForAssembly(ref guid, (asm == null) ? null : asm.InternalAssembly);
			return guid;
		}

		// Token: 0x06003207 RID: 12807
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallGetTypeLibGuidForAssembly(ref Guid result, Assembly asm);

		// Token: 0x06003208 RID: 12808
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetTypeLibVersionForAssembly(Assembly inputAssembly, out int majorVersion, out int minorVersion);

		// Token: 0x06003209 RID: 12809 RVA: 0x000AA671 File Offset: 0x000A9671
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void GetTypeLibVersionForAssembly(Assembly inputAssembly, out int majorVersion, out int minorVersion)
		{
			Marshal._GetTypeLibVersionForAssembly((inputAssembly == null) ? null : inputAssembly.InternalAssembly, out majorVersion, out minorVersion);
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x000AA686 File Offset: 0x000A9686
		[Obsolete("Use System.Runtime.InteropServices.Marshal.GetTypeInfoName(ITypeInfo pTLB) instead. http://go.microsoft.com/fwlink/?linkid=14202&ID=0000011.", false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static string GetTypeInfoName(UCOMITypeInfo pTI)
		{
			return Marshal.GetTypeInfoName((ITypeInfo)pTI);
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x000AA694 File Offset: 0x000A9694
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static string GetTypeInfoName(ITypeInfo typeInfo)
		{
			string text = null;
			string text2 = null;
			int num = 0;
			string text3 = null;
			if (typeInfo == null)
			{
				throw new ArgumentNullException("typeInfo");
			}
			typeInfo.GetDocumentation(-1, out text, out text2, out num, out text3);
			return text;
		}

		// Token: 0x0600320C RID: 12812
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type GetLoadedTypeForGUID(ref Guid guid);

		// Token: 0x0600320D RID: 12813 RVA: 0x000AA6C8 File Offset: 0x000A96C8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Type GetTypeForITypeInfo(IntPtr piTypeInfo)
		{
			ITypeInfo typeInfo = null;
			ITypeLib typeLib = null;
			Assembly assembly = null;
			int num = 0;
			if (piTypeInfo == Win32Native.NULL)
			{
				return null;
			}
			typeInfo = (ITypeInfo)Marshal.GetObjectForIUnknown(piTypeInfo);
			Guid typeInfoGuid = Marshal.GetTypeInfoGuid(typeInfo);
			Type type = Marshal.GetLoadedTypeForGUID(ref typeInfoGuid);
			if (type != null)
			{
				return type;
			}
			try
			{
				typeInfo.GetContainingTypeLib(out typeLib, out num);
			}
			catch (COMException)
			{
				typeLib = null;
			}
			if (typeLib != null)
			{
				AssemblyName assemblyNameFromTypelib = TypeLibConverter.GetAssemblyNameFromTypelib(typeLib, null, null, null, null, AssemblyNameFlags.None);
				string fullName = assemblyNameFromTypelib.FullName;
				Assembly[] assemblies = Thread.GetDomain().GetAssemblies();
				int num2 = assemblies.Length;
				for (int i = 0; i < num2; i++)
				{
					if (string.Compare(assemblies[i].FullName, fullName, StringComparison.Ordinal) == 0)
					{
						assembly = assemblies[i];
					}
				}
				if (assembly == null)
				{
					TypeLibConverter typeLibConverter = new TypeLibConverter();
					assembly = typeLibConverter.ConvertTypeLibToAssembly(typeLib, Marshal.GetTypeLibName(typeLib) + ".dll", TypeLibImporterFlags.None, new ImporterCallback(), null, null, null, null);
				}
				type = assembly.GetType(Marshal.GetTypeLibName(typeLib) + "." + Marshal.GetTypeInfoName(typeInfo), true, false);
				if (type != null && !type.IsVisible)
				{
					type = null;
				}
			}
			else
			{
				type = typeof(object);
			}
			return type;
		}

		// Token: 0x0600320E RID: 12814
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetITypeInfoForType(Type t);

		// Token: 0x0600320F RID: 12815 RVA: 0x000AA7F4 File Offset: 0x000A97F4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr GetIUnknownForObject(object o)
		{
			return Marshal.GetIUnknownForObjectNative(o, false);
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x000AA7FD File Offset: 0x000A97FD
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr GetIUnknownForObjectInContext(object o)
		{
			return Marshal.GetIUnknownForObjectNative(o, true);
		}

		// Token: 0x06003211 RID: 12817
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetIUnknownForObjectNative(object o, bool onlyInContext);

		// Token: 0x06003212 RID: 12818 RVA: 0x000AA806 File Offset: 0x000A9806
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr GetIDispatchForObject(object o)
		{
			return Marshal.GetIDispatchForObjectNative(o, false);
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x000AA80F File Offset: 0x000A980F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr GetIDispatchForObjectInContext(object o)
		{
			return Marshal.GetIDispatchForObjectNative(o, true);
		}

		// Token: 0x06003214 RID: 12820
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetIDispatchForObjectNative(object o, bool onlyInContext);

		// Token: 0x06003215 RID: 12821 RVA: 0x000AA818 File Offset: 0x000A9818
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr GetComInterfaceForObject(object o, Type T)
		{
			return Marshal.GetComInterfaceForObjectNative(o, T, false);
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x000AA822 File Offset: 0x000A9822
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr GetComInterfaceForObjectInContext(object o, Type t)
		{
			return Marshal.GetComInterfaceForObjectNative(o, t, true);
		}

		// Token: 0x06003217 RID: 12823
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetComInterfaceForObjectNative(object o, Type t, bool onlyInContext);

		// Token: 0x06003218 RID: 12824
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetObjectForIUnknown(IntPtr pUnk);

		// Token: 0x06003219 RID: 12825
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetUniqueObjectForIUnknown(IntPtr unknown);

		// Token: 0x0600321A RID: 12826
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetTypedObjectForIUnknown(IntPtr pUnk, Type t);

		// Token: 0x0600321B RID: 12827
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr CreateAggregatedObject(IntPtr pOuter, object o);

		// Token: 0x0600321C RID: 12828
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsComObject(object o);

		// Token: 0x0600321D RID: 12829 RVA: 0x000AA82C File Offset: 0x000A982C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static int ReleaseComObject(object o)
		{
			__ComObject _ComObject = null;
			try
			{
				_ComObject = (__ComObject)o;
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ObjNotComObject"), "o");
			}
			return _ComObject.ReleaseSelf();
		}

		// Token: 0x0600321E RID: 12830
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int InternalReleaseComObject(object o);

		// Token: 0x0600321F RID: 12831 RVA: 0x000AA870 File Offset: 0x000A9870
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static int FinalReleaseComObject(object o)
		{
			__ComObject _ComObject = null;
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			try
			{
				_ComObject = (__ComObject)o;
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ObjNotComObject"), "o");
			}
			_ComObject.FinalReleaseSelf();
			return 0;
		}

		// Token: 0x06003220 RID: 12832
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalFinalReleaseComObject(object o);

		// Token: 0x06003221 RID: 12833 RVA: 0x000AA8C4 File Offset: 0x000A98C4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static object GetComObjectData(object obj, object key)
		{
			__ComObject _ComObject = null;
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			try
			{
				_ComObject = (__ComObject)obj;
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ObjNotComObject"), "obj");
			}
			return _ComObject.GetData(key);
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x000AA928 File Offset: 0x000A9928
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static bool SetComObjectData(object obj, object key, object data)
		{
			__ComObject _ComObject = null;
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			try
			{
				_ComObject = (__ComObject)obj;
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ObjNotComObject"), "obj");
			}
			return _ComObject.SetData(key, data);
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000AA98C File Offset: 0x000A998C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static object CreateWrapperOfType(object o, Type t)
		{
			if (t == null)
			{
				throw new ArgumentNullException("t");
			}
			if (!t.IsCOMObject)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_TypeNotComObject"), "t");
			}
			if (t.IsGenericType)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "t");
			}
			if (o == null)
			{
				return null;
			}
			if (!o.GetType().IsCOMObject)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ObjNotComObject"), "o");
			}
			if (o.GetType() == t)
			{
				return o;
			}
			object obj = Marshal.GetComObjectData(o, t);
			if (obj == null)
			{
				obj = Marshal.InternalCreateWrapperOfType(o, t);
				if (!Marshal.SetComObjectData(o, t, obj))
				{
					obj = Marshal.GetComObjectData(o, t);
				}
			}
			return obj;
		}

		// Token: 0x06003224 RID: 12836
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object InternalCreateWrapperOfType(object o, Type t);

		// Token: 0x06003225 RID: 12837 RVA: 0x000AAA39 File Offset: 0x000A9A39
		[Obsolete("This API did not perform any operation and will be removed in future versions of the CLR.", false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void ReleaseThreadCache()
		{
		}

		// Token: 0x06003226 RID: 12838
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsTypeVisibleFromCom(Type t);

		// Token: 0x06003227 RID: 12839
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int QueryInterface(IntPtr pUnk, ref Guid iid, out IntPtr ppv);

		// Token: 0x06003228 RID: 12840
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int AddRef(IntPtr pUnk);

		// Token: 0x06003229 RID: 12841
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Release(IntPtr pUnk);

		// Token: 0x0600322A RID: 12842 RVA: 0x000AAA3C File Offset: 0x000A9A3C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr AllocCoTaskMem(int cb)
		{
			IntPtr intPtr = Win32Native.CoTaskMemAlloc(cb);
			if (intPtr == Win32Native.NULL)
			{
				throw new OutOfMemoryException();
			}
			return intPtr;
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x000AAA64 File Offset: 0x000A9A64
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr ReAllocCoTaskMem(IntPtr pv, int cb)
		{
			IntPtr intPtr = Win32Native.CoTaskMemRealloc(pv, cb);
			if (intPtr == Win32Native.NULL && cb != 0)
			{
				throw new OutOfMemoryException();
			}
			return intPtr;
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x000AAA90 File Offset: 0x000A9A90
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void FreeCoTaskMem(IntPtr ptr)
		{
			if (Marshal.IsNotWin32Atom(ptr))
			{
				Win32Native.CoTaskMemFree(ptr);
			}
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x000AAAA0 File Offset: 0x000A9AA0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void FreeBSTR(IntPtr ptr)
		{
			if (Marshal.IsNotWin32Atom(ptr))
			{
				Win32Native.SysFreeString(ptr);
			}
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x000AAAB0 File Offset: 0x000A9AB0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr StringToCoTaskMemUni(string s)
		{
			if (s == null)
			{
				return Win32Native.NULL;
			}
			int num = (s.Length + 1) * 2;
			if (num < s.Length)
			{
				throw new ArgumentOutOfRangeException("s");
			}
			IntPtr intPtr = Win32Native.CoTaskMemAlloc(num);
			if (intPtr == Win32Native.NULL)
			{
				throw new OutOfMemoryException();
			}
			Win32Native.CopyMemoryUni(intPtr, s, new IntPtr(num));
			return intPtr;
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x000AAB10 File Offset: 0x000A9B10
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr StringToCoTaskMemAuto(string s)
		{
			if (s == null)
			{
				return Win32Native.NULL;
			}
			int num = (s.Length + 1) * Marshal.SystemDefaultCharSize;
			if (num < s.Length)
			{
				throw new ArgumentOutOfRangeException("s");
			}
			IntPtr intPtr = Win32Native.CoTaskMemAlloc(num);
			if (intPtr == Win32Native.NULL)
			{
				throw new OutOfMemoryException();
			}
			Win32Native.lstrcpy(intPtr, s);
			return intPtr;
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x000AAB6C File Offset: 0x000A9B6C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr StringToBSTR(string s)
		{
			if (s == null)
			{
				return Win32Native.NULL;
			}
			if (s.Length + 1 < s.Length)
			{
				throw new ArgumentOutOfRangeException("s");
			}
			IntPtr intPtr = Win32Native.SysAllocStringLen(s, s.Length);
			if (intPtr == Win32Native.NULL)
			{
				throw new OutOfMemoryException();
			}
			return intPtr;
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x000AABBE File Offset: 0x000A9BBE
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static string PtrToStringBSTR(IntPtr ptr)
		{
			return Marshal.PtrToStringUni(ptr, Win32Native.SysStringLen(ptr));
		}

		// Token: 0x06003232 RID: 12850
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetNativeVariantForObject(object obj, IntPtr pDstNativeVariant);

		// Token: 0x06003233 RID: 12851
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetObjectForNativeVariant(IntPtr pSrcNativeVariant);

		// Token: 0x06003234 RID: 12852
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object[] GetObjectsForNativeVariants(IntPtr aSrcNativeVariant, int cVars);

		// Token: 0x06003235 RID: 12853
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetStartComSlot(Type t);

		// Token: 0x06003236 RID: 12854
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetEndComSlot(Type t);

		// Token: 0x06003237 RID: 12855
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern MemberInfo GetMethodInfoForComSlot(Type t, int slot, ref ComMemberType memberType);

		// Token: 0x06003238 RID: 12856 RVA: 0x000AABCC File Offset: 0x000A9BCC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static int GetComSlotForMethodInfo(MemberInfo m)
		{
			if (m == null)
			{
				throw new ArgumentNullException("m");
			}
			if (!(m is RuntimeMethodInfo))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"), "m");
			}
			if (!m.DeclaringType.IsInterface)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeInterfaceMethod"), "m");
			}
			if (m.DeclaringType.IsGenericType)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "m");
			}
			RuntimeMethodHandle methodHandle = ((RuntimeMethodInfo)m).GetMethodHandle();
			return Marshal.InternalGetComSlotForMethodInfo(methodHandle);
		}

		// Token: 0x06003239 RID: 12857
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int InternalGetComSlotForMethodInfo(RuntimeMethodHandle m);

		// Token: 0x0600323A RID: 12858 RVA: 0x000AAC5C File Offset: 0x000A9C5C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Guid GenerateGuidForType(Type type)
		{
			Guid guid = default(Guid);
			Marshal.FCallGenerateGuidForType(ref guid, type);
			return guid;
		}

		// Token: 0x0600323B RID: 12859
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallGenerateGuidForType(ref Guid result, Type type);

		// Token: 0x0600323C RID: 12860 RVA: 0x000AAC7C File Offset: 0x000A9C7C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static string GenerateProgIdForType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!RegistrationServices.TypeRequiresRegistrationHelper(type))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_TypeMustBeComCreatable"), "type");
			}
			if (type.IsImport)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_TypeMustNotBeComImport"), "type");
			}
			if (type.IsGenericType)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "type");
			}
			IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(type);
			for (int i = 0; i < customAttributes.Count; i++)
			{
				if (customAttributes[i].Constructor.DeclaringType == typeof(ProgIdAttribute))
				{
					IList<CustomAttributeTypedArgument> constructorArguments = customAttributes[i].ConstructorArguments;
					string text = (string)constructorArguments[0].Value;
					if (text == null)
					{
						text = string.Empty;
					}
					return text;
				}
			}
			return type.FullName;
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x000AAD5C File Offset: 0x000A9D5C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static object BindToMoniker(string monikerName)
		{
			object obj = null;
			IBindCtx bindCtx = null;
			Marshal.CreateBindCtx(0U, out bindCtx);
			IMoniker moniker = null;
			uint num;
			Marshal.MkParseDisplayName(bindCtx, monikerName, out num, out moniker);
			Marshal.BindMoniker(moniker, 0U, ref Marshal.IID_IUnknown, out obj);
			return obj;
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x000AAD94 File Offset: 0x000A9D94
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static object GetActiveObject(string progID)
		{
			object obj = null;
			Guid guid;
			try
			{
				Marshal.CLSIDFromProgIDEx(progID, out guid);
			}
			catch (Exception)
			{
				Marshal.CLSIDFromProgID(progID, out guid);
			}
			Marshal.GetActiveObject(ref guid, IntPtr.Zero, out obj);
			return obj;
		}

		// Token: 0x0600323F RID: 12863
		[DllImport("ole32.dll", PreserveSig = false)]
		private static extern void CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);

		// Token: 0x06003240 RID: 12864
		[DllImport("ole32.dll", PreserveSig = false)]
		private static extern void CLSIDFromProgID([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);

		// Token: 0x06003241 RID: 12865
		[DllImport("ole32.dll", PreserveSig = false)]
		private static extern void CreateBindCtx(uint reserved, out IBindCtx ppbc);

		// Token: 0x06003242 RID: 12866
		[DllImport("ole32.dll", PreserveSig = false)]
		private static extern void MkParseDisplayName(IBindCtx pbc, [MarshalAs(UnmanagedType.LPWStr)] string szUserName, out uint pchEaten, out IMoniker ppmk);

		// Token: 0x06003243 RID: 12867
		[DllImport("ole32.dll", PreserveSig = false)]
		private static extern void BindMoniker(IMoniker pmk, uint grfOpt, ref Guid iidResult, [MarshalAs(UnmanagedType.Interface)] out object ppvResult);

		// Token: 0x06003244 RID: 12868
		[DllImport("oleaut32.dll", PreserveSig = false)]
		private static extern void GetActiveObject(ref Guid rclsid, IntPtr reserved, [MarshalAs(UnmanagedType.Interface)] out object ppunk);

		// Token: 0x06003245 RID: 12869
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool InternalSwitchCCW(object oldtp, object newtp);

		// Token: 0x06003246 RID: 12870
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object InternalWrapIUnknownWithComObject(IntPtr i);

		// Token: 0x06003247 RID: 12871 RVA: 0x000AADD8 File Offset: 0x000A9DD8
		private static RuntimeTypeHandle LoadLicenseManager()
		{
			Assembly assembly = Assembly.Load("System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			Type type = assembly.GetType("System.ComponentModel.LicenseManager");
			if (type == null || !type.IsVisible)
			{
				return RuntimeTypeHandle.EmptyHandle;
			}
			return type.TypeHandle;
		}

		// Token: 0x06003248 RID: 12872
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ChangeWrapperHandleStrength(object otp, bool fIsWeak);

		// Token: 0x06003249 RID: 12873 RVA: 0x000AAE14 File Offset: 0x000A9E14
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Delegate GetDelegateForFunctionPointer(IntPtr ptr, Type t)
		{
			if (ptr == IntPtr.Zero)
			{
				throw new ArgumentNullException("ptr");
			}
			if (t == null)
			{
				throw new ArgumentNullException("t");
			}
			if (!(t is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "t");
			}
			if (t.IsGenericType)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "t");
			}
			Type baseType = t.BaseType;
			if (baseType == null || (baseType != typeof(Delegate) && baseType != typeof(MulticastDelegate)))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDelegate"), "t");
			}
			return Marshal.GetDelegateForFunctionPointerInternal(ptr, t);
		}

		// Token: 0x0600324A RID: 12874
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Delegate GetDelegateForFunctionPointerInternal(IntPtr ptr, Type t);

		// Token: 0x0600324B RID: 12875 RVA: 0x000AAEC1 File Offset: 0x000A9EC1
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr GetFunctionPointerForDelegate(Delegate d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return Marshal.GetFunctionPointerForDelegateInternal(d);
		}

		// Token: 0x0600324C RID: 12876
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetFunctionPointerForDelegateInternal(Delegate d);

		// Token: 0x0600324D RID: 12877 RVA: 0x000AAED7 File Offset: 0x000A9ED7
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr SecureStringToBSTR(SecureString s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return s.ToBSTR();
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x000AAEED File Offset: 0x000A9EED
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr SecureStringToCoTaskMemAnsi(SecureString s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return s.ToAnsiStr(false);
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x000AAF04 File Offset: 0x000A9F04
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr SecureStringToGlobalAllocAnsi(SecureString s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return s.ToAnsiStr(true);
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x000AAF1B File Offset: 0x000A9F1B
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr SecureStringToCoTaskMemUnicode(SecureString s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return s.ToUniStr(false);
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x000AAF32 File Offset: 0x000A9F32
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IntPtr SecureStringToGlobalAllocUnicode(SecureString s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return s.ToUniStr(true);
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x000AAF49 File Offset: 0x000A9F49
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void ZeroFreeBSTR(IntPtr s)
		{
			Win32Native.ZeroMemory(s, (uint)(Win32Native.SysStringLen(s) * 2));
			Marshal.FreeBSTR(s);
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x000AAF5F File Offset: 0x000A9F5F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void ZeroFreeCoTaskMemAnsi(IntPtr s)
		{
			Win32Native.ZeroMemory(s, (uint)Win32Native.lstrlenA(s));
			Marshal.FreeCoTaskMem(s);
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x000AAF73 File Offset: 0x000A9F73
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void ZeroFreeGlobalAllocAnsi(IntPtr s)
		{
			Win32Native.ZeroMemory(s, (uint)Win32Native.lstrlenA(s));
			Marshal.FreeHGlobal(s);
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x000AAF87 File Offset: 0x000A9F87
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void ZeroFreeCoTaskMemUnicode(IntPtr s)
		{
			Win32Native.ZeroMemory(s, (uint)(Win32Native.lstrlenW(s) * 2));
			Marshal.FreeCoTaskMem(s);
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x000AAF9D File Offset: 0x000A9F9D
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void ZeroFreeGlobalAllocUnicode(IntPtr s)
		{
			Win32Native.ZeroMemory(s, (uint)(Win32Native.lstrlenW(s) * 2));
			Marshal.FreeHGlobal(s);
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x000AAFB4 File Offset: 0x000A9FB4
		// Note: this type is marked as 'beforefieldinit'.
		static Marshal()
		{
			int num = 3;
			sbyte[] array = new sbyte[4];
			array[0] = 65;
			array[1] = 65;
			Marshal.SystemDefaultCharSize = num - Win32Native.lstrlen(array);
			Marshal.SystemMaxDBCSCharSize = Marshal.GetSystemMaxDBCSCharSize();
		}

		// Token: 0x04001981 RID: 6529
		private const int LMEM_FIXED = 0;

		// Token: 0x04001982 RID: 6530
		private const int LMEM_MOVEABLE = 2;

		// Token: 0x04001983 RID: 6531
		private const string s_strConvertedTypeInfoAssemblyName = "InteropDynamicTypes";

		// Token: 0x04001984 RID: 6532
		private const string s_strConvertedTypeInfoAssemblyTitle = "Interop Dynamic Types";

		// Token: 0x04001985 RID: 6533
		private const string s_strConvertedTypeInfoAssemblyDesc = "Type dynamically generated from ITypeInfo's";

		// Token: 0x04001986 RID: 6534
		private const string s_strConvertedTypeInfoNameSpace = "InteropDynamicTypes";

		// Token: 0x04001987 RID: 6535
		private static readonly IntPtr HIWORDMASK = new IntPtr(-65536L);

		// Token: 0x04001988 RID: 6536
		private static Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

		// Token: 0x04001989 RID: 6537
		public static readonly int SystemDefaultCharSize;

		// Token: 0x0400198A RID: 6538
		public static readonly int SystemMaxDBCSCharSize;
	}
}
