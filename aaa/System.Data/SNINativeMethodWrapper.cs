using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

// Token: 0x02000002 RID: 2
[CLSCompliant(false)]
internal class SNINativeMethodWrapper
{
	// Token: 0x0600005D RID: 93 RVA: 0x001C3F64 File Offset: 0x001C3364
	internal static byte[] GetData()
	{
		byte[] array = null;
		int num;
		IntPtr intPtr = (IntPtr)<Module>.SqlDependencyProcessDispatcherStorage.NativeGetData(ref num);
		if (intPtr != IntPtr.Zero)
		{
			array = new byte[num];
			Marshal.Copy(intPtr, array, 0, num);
		}
		return array;
	}

	// Token: 0x0600005E RID: 94 RVA: 0x001C3FB8 File Offset: 0x001C33B8
	internal static void SetData(byte[] data)
	{
		ref byte ptr = ref data[0];
		<Module>.SqlDependencyProcessDispatcherStorage.NativeSetData(ref ptr, data.Length);
	}

	// Token: 0x0600005F RID: 95 RVA: 0x001C403C File Offset: 0x001C343C
	internal static _AppDomain GetDefaultAppDomain()
	{
		IntPtr intPtr = (IntPtr)<Module>.SqlDependencyProcessDispatcherStorage.NativeGetDefaultAppDomain();
		object objectForIUnknown = Marshal.GetObjectForIUnknown(intPtr);
		Marshal.Release(intPtr);
		return objectForIUnknown as _AppDomain;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x001C40B8 File Offset: 0x001C34B8
	internal static IntPtr SNIServerEnumOpen()
	{
		IntPtr intPtr = new IntPtr(<Module>.SNIServerEnumOpen(null, 1));
		return intPtr;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x001C40D4 File Offset: 0x001C34D4
	internal unsafe static int SNIServerEnumRead(IntPtr handle, char[] wStr, int pcbBuf, ref bool fMore)
	{
		ref ushort ptr = ref wStr[0];
		int num = (fMore ? 1 : 0);
		int num2 = <Module>.SNIServerEnumRead(handle.ToPointer(), ref ptr, pcbBuf, &num);
		byte b = ((num != 0) ? 1 : 0);
		fMore = b != 0;
		return num2;
	}

	// Token: 0x06000062 RID: 98 RVA: 0x001C4108 File Offset: 0x001C3508
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void SNIServerEnumClose(IntPtr handle)
	{
		delegate* unmanaged[Stdcall, Stdcall]<void*, void> _unep@?SNIServerEnumClose@@$$J14YGXPAX@Z = <Module>.__unep@?SNIServerEnumClose@@$$J14YGXPAX@Z;
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Void*), handle.ToPointer(), _unep@?SNIServerEnumClose@@$$J14YGXPAX@Z);
	}

	// Token: 0x06000063 RID: 99 RVA: 0x001C4128 File Offset: 0x001C3528
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static uint SNIClose(IntPtr pConn)
	{
		delegate* unmanaged[Stdcall, Stdcall]<SNI_Conn*, uint> _unep@?SNIClose@@$$J14YGKPAVSNI_Conn@@@Z = <Module>.__unep@?SNIClose@@$$J14YGKPAVSNI_Conn@@@Z;
		return calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(SNI_Conn*), (SNI_Conn*)pConn.ToPointer(), _unep@?SNIClose@@$$J14YGKPAVSNI_Conn@@@Z);
	}

	// Token: 0x06000064 RID: 100 RVA: 0x001C4148 File Offset: 0x001C3548
	internal static uint SNIInitialize()
	{
		return <Module>.SNIInitialize(null);
	}

	// Token: 0x06000065 RID: 101 RVA: 0x001C415C File Offset: 0x001C355C
	private unsafe static void MarshalConsumerInfo(SNINativeMethodWrapper.ConsumerInfo consumerInfo, SNI_CONSUMER_INFO* native_consumerInfo)
	{
		*native_consumerInfo = consumerInfo.defaultBufferSize;
		void* ptr;
		if (null == consumerInfo.readDelegate)
		{
			ptr = null;
		}
		else
		{
			ptr = Marshal.GetFunctionPointerForDelegate(consumerInfo.readDelegate).ToPointer();
		}
		*(native_consumerInfo + 8) = ptr;
		void* ptr2;
		if (null == consumerInfo.writeDelegate)
		{
			ptr2 = null;
		}
		else
		{
			ptr2 = Marshal.GetFunctionPointerForDelegate(consumerInfo.writeDelegate).ToPointer();
		}
		*(native_consumerInfo + 12) = ptr2;
		*(native_consumerInfo + 4) = consumerInfo.key.ToPointer();
	}

	// Token: 0x06000066 RID: 102 RVA: 0x001C41D8 File Offset: 0x001C35D8
	internal unsafe static uint SNIOpenSyncEx(SNINativeMethodWrapper.ConsumerInfo consumerInfo, string constring, ref IntPtr pConn, byte[] spnBuffer, byte[] instanceName, [MarshalAs(UnmanagedType.U1)] bool fOverrideCache, [MarshalAs(UnmanagedType.U1)] bool fSync, int timeout, [MarshalAs(UnmanagedType.U1)] bool fParallel)
	{
		SNI_CLIENT_CONSUMER_INFO sni_CLIENT_CONSUMER_INFO;
		<Module>.SNI_CLIENT_CONSUMER_INFO.{ctor}(ref sni_CLIENT_CONSUMER_INFO);
		ref ushort ptr = ref <Module>.PtrToStringChars(constring);
		byte b = ((null == pConn.ToPointer()) ? 1 : 0);
		Debug.Assert(b != 0, "Verrifying variable is really not initallized.");
		SNI_Conn* ptr2 = null;
		ref byte ptr3 = (ref spnBuffer != null ? ref spnBuffer[0] : 0);
		ref byte ptr4 = ref instanceName[0];
		SNINativeMethodWrapper.MarshalConsumerInfo(consumerInfo, ref sni_CLIENT_CONSUMER_INFO);
		*((ref sni_CLIENT_CONSUMER_INFO) + 32) = ref ptr;
		*((ref sni_CLIENT_CONSUMER_INFO) + 36) = 0;
		if (spnBuffer != null)
		{
			*((ref sni_CLIENT_CONSUMER_INFO) + 40) = ref ptr3;
			*((ref sni_CLIENT_CONSUMER_INFO) + 44) = spnBuffer.Length;
		}
		*((ref sni_CLIENT_CONSUMER_INFO) + 48) = ref ptr4;
		*((ref sni_CLIENT_CONSUMER_INFO) + 52) = instanceName.Length;
		*((ref sni_CLIENT_CONSUMER_INFO) + 56) = (fOverrideCache ? 1 : 0);
		*((ref sni_CLIENT_CONSUMER_INFO) + 60) = (fSync ? 1 : 0);
		*((ref sni_CLIENT_CONSUMER_INFO) + 64) = timeout;
		*((ref sni_CLIENT_CONSUMER_INFO) + 68) = (fParallel ? 1 : 0);
		uint num = <Module>.SNIOpenSyncEx(&sni_CLIENT_CONSUMER_INFO, &ptr2);
		IntPtr intPtr = (IntPtr)((void*)ptr2);
		pConn = intPtr;
		return num;
	}

	// Token: 0x06000067 RID: 103 RVA: 0x001C4298 File Offset: 0x001C3698
	internal unsafe static uint SNIOpen(SNINativeMethodWrapper.ConsumerInfo consumerInfo, string constring, SafeHandle parent, ref IntPtr pConn, [MarshalAs(UnmanagedType.U1)] bool fSync)
	{
		uint num = 0U;
		SNI_CONSUMER_INFO sni_CONSUMER_INFO;
		SNINativeMethodWrapper.MarshalConsumerInfo(consumerInfo, ref sni_CONSUMER_INFO);
		SNI_Conn* ptr = null;
		ref byte ptr2 = ref Encoding.ASCII.GetBytes(constring)[0];
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			parent.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			num = <Module>.SNIOpen(&sni_CONSUMER_INFO, ref ptr2, parent.DangerousGetHandle().ToPointer(), &ptr, fSync ? 1 : 0);
		}
		finally
		{
			if (flag)
			{
				parent.DangerousRelease();
			}
		}
		IntPtr intPtr = (IntPtr)((void*)ptr);
		pConn = intPtr;
		return num;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x001C4334 File Offset: 0x001C3734
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal unsafe static void SNIPacketAllocate(SafeHandle pConn, SNINativeMethodWrapper.IOType ioType, ref IntPtr ret)
	{
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			pConn.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			SNI_Conn* ptr = (SNI_Conn*)pConn.DangerousGetHandle().ToPointer();
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				delegate* unmanaged[Stdcall, Stdcall]<SNI_Conn*, uint, SNI_Packet*> _unep@?SNIPacketAllocate@@$$J18YGPAVSNI_Packet@@PAVSNI_Conn@@K@Z = <Module>.__unep@?SNIPacketAllocate@@$$J18YGPAVSNI_Packet@@PAVSNI_Conn@@K@Z;
				IntPtr intPtr = (IntPtr)calli(SNI_Packet* modopt(System.Runtime.CompilerServices.CallConvStdcall)(SNI_Conn*,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)), ptr, ioType, _unep@?SNIPacketAllocate@@$$J18YGPAVSNI_Packet@@PAVSNI_Conn@@K@Z);
				ret = intPtr;
			}
		}
		finally
		{
			if (flag)
			{
				pConn.DangerousRelease();
			}
		}
	}

	// Token: 0x06000069 RID: 105 RVA: 0x001C43D0 File Offset: 0x001C37D0
	internal static IntPtr SNIPacketGetConnection(IntPtr packet)
	{
		ref SNI_Packet ptr = packet.ToPointer();
		return (IntPtr)<Module>.SNIPacketGetConnection(ref ptr);
	}

	// Token: 0x0600006A RID: 106 RVA: 0x001C43F0 File Offset: 0x001C37F0
	internal unsafe static void SNIPacketGetData(IntPtr packet, ref IntPtr data, ref uint dataSize)
	{
		ref SNI_Packet ptr = packet.ToPointer();
		byte* ptr2 = null;
		uint num = 0U;
		<Module>.SNIPacketGetData(ref ptr, &ptr2, (uint*)(&num));
		IntPtr intPtr = (IntPtr)((void*)ptr2);
		data = intPtr;
		dataSize = num;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x001C4428 File Offset: 0x001C3828
	internal unsafe static void SNIPacketReset(SafeHandle pConn, SNINativeMethodWrapper.IOType ioType, SafeHandle packet)
	{
		bool flag = false;
		bool flag2 = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			pConn.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			packet.DangerousAddRef(ref flag2);
			Debug.Assert(flag2, "AddRef Failed!");
			SNI_Conn* ptr = (SNI_Conn*)pConn.DangerousGetHandle().ToPointer();
			SNI_Packet* ptr2 = (SNI_Packet*)packet.DangerousGetHandle().ToPointer();
			<Module>.SNIPacketReset(ptr, ioType, ptr2);
		}
		finally
		{
			if (flag)
			{
				pConn.DangerousRelease();
			}
			if (flag2)
			{
				packet.DangerousRelease();
			}
		}
	}

	// Token: 0x0600006C RID: 108 RVA: 0x001C44BC File Offset: 0x001C38BC
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void SNIPacketRelease(IntPtr packet)
	{
		ref SNI_Packet ptr = packet.ToPointer();
		delegate* unmanaged[Stdcall, Stdcall]<SNI_Packet*, void> _unep@?SNIPacketRelease@@$$J14YGXPAVSNI_Packet@@@Z = <Module>.__unep@?SNIPacketRelease@@$$J14YGXPAVSNI_Packet@@@Z;
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvStdcall)(SNI_Packet*), ref ptr, _unep@?SNIPacketRelease@@$$J14YGXPAVSNI_Packet@@@Z);
	}

	// Token: 0x0600006D RID: 109 RVA: 0x001C44E0 File Offset: 0x001C38E0
	internal unsafe static void SNIPacketSetData(SafeHandle packet, byte[] data, int length)
	{
		ref byte ptr = ref data[0];
		RuntimeHelpers.PrepareConstrainedRegions();
		bool flag = false;
		try
		{
			packet.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			<Module>.SNIPacketSetData((SNI_Packet*)packet.DangerousGetHandle().ToPointer(), ref ptr, length);
		}
		finally
		{
			if (flag)
			{
				packet.DangerousRelease();
			}
		}
	}

	// Token: 0x0600006E RID: 110 RVA: 0x001C4568 File Offset: 0x001C3968
	[ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)]
	[ResourceExposure(ResourceScope.None)]
	internal static int SNIQueryInfo(SNINativeMethodWrapper.QTypes qType, ref IntPtr qInfo)
	{
		byte b = ((qType == SNINativeMethodWrapper.QTypes.SNI_QUERY_LOCALDB_HMODULE) ? 1 : 0);
		Debug.Assert(b != 0, "qType is unsupported or unknown");
		ref IntPtr ptr = ref qInfo;
		return <Module>.SNIQueryInfo((uint)qType, ref ptr);
	}

	// Token: 0x0600006F RID: 111 RVA: 0x001C454C File Offset: 0x001C394C
	internal unsafe static int SNIQueryInfo(SNINativeMethodWrapper.QTypes qType, ref uint qInfo)
	{
		uint num = qInfo;
		int num2 = <Module>.SNIQueryInfo((uint)qType, (void*)(&num));
		qInfo = num;
		return num2;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x001C4590 File Offset: 0x001C3990
	internal unsafe static uint SNISetInfo(SafeHandle pConn, SNINativeMethodWrapper.QTypes qtype, ref uint qInfo)
	{
		uint num = qInfo;
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		uint num2;
		try
		{
			pConn.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			num2 = <Module>.SNISetInfo((SNI_Conn*)pConn.DangerousGetHandle().ToPointer(), (uint)qtype, (void*)(&num));
		}
		finally
		{
			if (flag)
			{
				pConn.DangerousRelease();
			}
		}
		qInfo = num;
		return num2;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x001C4600 File Offset: 0x001C3A00
	internal unsafe static uint SNIReadAsync(SafeHandle pConn, ref IntPtr packet)
	{
		SNI_Packet* ptr = null;
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		uint num;
		try
		{
			pConn.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			num = <Module>.SNIReadAsync((SNI_Conn*)pConn.DangerousGetHandle().ToPointer(), &ptr, null);
		}
		finally
		{
			if (flag)
			{
				pConn.DangerousRelease();
			}
		}
		IntPtr intPtr = (IntPtr)((void*)ptr);
		packet = intPtr;
		return num;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x001C467C File Offset: 0x001C3A7C
	internal unsafe static uint SNIReadSync(SafeHandle pConn, ref IntPtr packet, int timeout)
	{
		SNI_Packet* ptr = null;
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		uint num;
		try
		{
			pConn.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			num = <Module>.SNIReadSync((SNI_Conn*)pConn.DangerousGetHandle().ToPointer(), &ptr, timeout);
		}
		finally
		{
			if (flag)
			{
				pConn.DangerousRelease();
			}
		}
		IntPtr intPtr = (IntPtr)((void*)ptr);
		packet = intPtr;
		return num;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x001C46F8 File Offset: 0x001C3AF8
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static uint SNITerminate()
	{
		return calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(), <Module>.__unep@?SNITerminate@@$$J10YGKXZ);
	}

	// Token: 0x06000074 RID: 116 RVA: 0x001C4710 File Offset: 0x001C3B10
	internal unsafe static uint SNIWriteAsync(SafeHandle pConn, SafeHandle packet)
	{
		bool flag = false;
		bool flag2 = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		uint num;
		try
		{
			pConn.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			packet.DangerousAddRef(ref flag2);
			Debug.Assert(flag2, "AddRef Failed!");
			SNI_Conn* ptr = (SNI_Conn*)pConn.DangerousGetHandle().ToPointer();
			SNI_Packet* ptr2 = (SNI_Packet*)packet.DangerousGetHandle().ToPointer();
			num = <Module>.SNIWriteAsync(ptr, ptr2, null);
		}
		finally
		{
			if (flag)
			{
				pConn.DangerousRelease();
			}
			if (flag2)
			{
				packet.DangerousRelease();
			}
		}
		return num;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x001C47A8 File Offset: 0x001C3BA8
	internal unsafe static uint SNIWriteSync(SafeHandle pConn, SafeHandle packet)
	{
		bool flag = false;
		bool flag2 = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		uint num;
		try
		{
			pConn.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			packet.DangerousAddRef(ref flag2);
			Debug.Assert(flag2, "AddRef Failed!");
			SNI_Conn* ptr = (SNI_Conn*)pConn.DangerousGetHandle().ToPointer();
			SNI_Packet* ptr2 = (SNI_Packet*)packet.DangerousGetHandle().ToPointer();
			num = <Module>.SNIWriteSync(ptr, ptr2, null);
		}
		finally
		{
			if (flag)
			{
				pConn.DangerousRelease();
			}
			if (flag2)
			{
				packet.DangerousRelease();
			}
		}
		return num;
	}

	// Token: 0x06000076 RID: 118 RVA: 0x001C4840 File Offset: 0x001C3C40
	internal unsafe static uint SNIAddProvider(SafeHandle pConn, SNINativeMethodWrapper.ProviderEnum providerEnum, ref uint info)
	{
		uint num = info;
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		uint num2;
		try
		{
			pConn.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			num2 = <Module>.SNIAddProvider((SNI_Conn*)pConn.DangerousGetHandle().ToPointer(), (ProviderNum)providerEnum, (void*)(&num));
		}
		finally
		{
			if (flag)
			{
				pConn.DangerousRelease();
			}
		}
		info = num;
		return num2;
	}

	// Token: 0x06000077 RID: 119 RVA: 0x001C48B0 File Offset: 0x001C3CB0
	internal unsafe static uint SNIRemoveProvider(SafeHandle pConn, SNINativeMethodWrapper.ProviderEnum providerEnum)
	{
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		uint num;
		try
		{
			pConn.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			num = <Module>.SNIRemoveProvider((SNI_Conn*)pConn.DangerousGetHandle().ToPointer(), (ProviderNum)providerEnum);
		}
		finally
		{
			if (flag)
			{
				pConn.DangerousRelease();
			}
		}
		return num;
	}

	// Token: 0x06000078 RID: 120 RVA: 0x001C4918 File Offset: 0x001C3D18
	internal unsafe static void SNIGetLastError(SNINativeMethodWrapper.SNI_Error error)
	{
		SNI_ERROR sni_ERROR;
		<Module>.SNIGetLastError(&sni_ERROR);
		error.provider = sni_ERROR;
		error.errorMessage = new char[522];
		int num = 0;
		do
		{
			char[] errorMessage = error.errorMessage;
			int num2 = num;
			errorMessage[num2] = *(num2 * 2 + ((ref sni_ERROR) + 4));
			num++;
		}
		while (num < 261);
		error.nativeError = (uint)(*((ref sni_ERROR) + 528));
		error.sniError = (uint)(*((ref sni_ERROR) + 532));
		IntPtr intPtr = (IntPtr)(*((ref sni_ERROR) + 536));
		error.fileName = Marshal.PtrToStringUni(intPtr);
		IntPtr intPtr2 = (IntPtr)(*((ref sni_ERROR) + 540));
		error.function = Marshal.PtrToStringUni(intPtr2);
		error.lineNumber = (uint)(*((ref sni_ERROR) + 544));
	}

	// Token: 0x06000079 RID: 121 RVA: 0x001C49C8 File Offset: 0x001C3DC8
	internal unsafe static uint SNISecInitPackage(ref uint maxLength)
	{
		uint num = maxLength;
		uint num2 = <Module>.SNISecInitPackage((uint*)(&num));
		maxLength = num;
		return num2;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x001C49E4 File Offset: 0x001C3DE4
	internal unsafe static uint SNISecGenClientContext(SafeHandle pConnectionObject, byte[] inBuff, uint receivedLength, byte[] OutBuff, ref uint sendLength, byte[] serverUserName)
	{
		uint num = sendLength;
		ref byte ptr = (ref inBuff != null ? ref inBuff[0] : 0);
		ref byte ptr2 = ref OutBuff[0];
		ref byte ptr3 = ref serverUserName[0];
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		uint num3;
		try
		{
			pConnectionObject.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			SNI_Conn* ptr4 = (SNI_Conn*)pConnectionObject.DangerousGetHandle().ToPointer();
			int num2;
			if (serverUserName == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = serverUserName.Length;
			}
			int num4;
			num3 = <Module>.SNISecGenClientContext(ptr4, ref ptr, receivedLength, ref ptr2, (uint*)(&num), &num4, ref ptr3, num2, null, null);
		}
		finally
		{
			if (flag)
			{
				pConnectionObject.DangerousRelease();
			}
		}
		sendLength = num;
		return num3;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x001C4A90 File Offset: 0x001C3E90
	[ResourceExposure(ResourceScope.None)]
	[ResourceConsumption(ResourceScope.Machine, ResourceScope.Machine)]
	internal unsafe static uint SNIWaitForSSLHandshakeToComplete(SafeHandle pConn, int timeoutMilliseconds)
	{
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		uint num;
		try
		{
			pConn.DangerousAddRef(ref flag);
			Debug.Assert(flag, "AddRef Failed!");
			num = <Module>.SNIWaitForSSLHandshakeToComplete((SNI_Conn*)pConn.DangerousGetHandle().ToPointer(), timeoutMilliseconds);
		}
		finally
		{
			if (flag)
			{
				pConn.DangerousRelease();
			}
		}
		return num;
	}

	// Token: 0x04000040 RID: 64
	internal static int SNI_LocalDBErrorCode = 50;

	// Token: 0x04000041 RID: 65
	internal static int SniMaxComposedSpnLength = (int)<Module>.SNI_MAX_COMPOSED_SPN;

	// Token: 0x02000003 RID: 3
	internal enum QTypes
	{
		// Token: 0x04000043 RID: 67
		SNI_QUERY_LOCALDB_HMODULE = 24,
		// Token: 0x04000044 RID: 68
		SNI_QUERY_CONN_SECPKG = 10,
		// Token: 0x04000045 RID: 69
		SNI_QUERY_CONN_PARENTCONNID = 9,
		// Token: 0x04000046 RID: 70
		SNI_QUERY_CONN_CONNID = 8,
		// Token: 0x04000047 RID: 71
		SNI_QUERY_CONN_PROVIDERNUM = 7,
		// Token: 0x04000048 RID: 72
		SNI_QUERY_CONN_ENCRYPT = 6,
		// Token: 0x04000049 RID: 73
		SNI_QUERY_CERTIFICATE = 5,
		// Token: 0x0400004A RID: 74
		SNI_QUERY_SERVER_ENCRYPT_POSSIBLE = 4,
		// Token: 0x0400004B RID: 75
		SNI_QUERY_CLIENT_ENCRYPT_POSSIBLE = 3,
		// Token: 0x0400004C RID: 76
		SNI_QUERY_CONN_KEY = 2,
		// Token: 0x0400004D RID: 77
		SNI_QUERY_CONN_BUFSIZE = 1,
		// Token: 0x0400004E RID: 78
		SNI_QUERY_CONN_INFO = 0
	}

	// Token: 0x02000004 RID: 4
	internal enum ProviderEnum
	{
		// Token: 0x04000050 RID: 80
		INVALID_PROV = 10,
		// Token: 0x04000051 RID: 81
		MAX_PROVS = 9,
		// Token: 0x04000052 RID: 82
		VIA_PROV = 8,
		// Token: 0x04000053 RID: 83
		TCP_PROV = 7,
		// Token: 0x04000054 RID: 84
		SSL_PROV = 6,
		// Token: 0x04000055 RID: 85
		SMUX_PROV = 5,
		// Token: 0x04000056 RID: 86
		SM_PROV = 4,
		// Token: 0x04000057 RID: 87
		SIGN_PROV = 3,
		// Token: 0x04000058 RID: 88
		SESSION_PROV = 2,
		// Token: 0x04000059 RID: 89
		NP_PROV = 1,
		// Token: 0x0400005A RID: 90
		HTTP_PROV = 0
	}

	// Token: 0x02000005 RID: 5
	internal enum IOType
	{
		// Token: 0x0400005C RID: 92
		WRITE = 1,
		// Token: 0x0400005D RID: 93
		READ = 0
	}

	// Token: 0x02000006 RID: 6
	// (Invoke) Token: 0x0600007F RID: 127
	internal delegate void SqlAsyncCallbackDelegate(IntPtr ptr1, IntPtr ptr2, uint num);

	// Token: 0x02000007 RID: 7
	[CLSCompliant(false)]
	internal class ConsumerInfo
	{
		// Token: 0x0400005E RID: 94
		internal int defaultBufferSize;

		// Token: 0x0400005F RID: 95
		internal SNINativeMethodWrapper.SqlAsyncCallbackDelegate readDelegate;

		// Token: 0x04000060 RID: 96
		internal SNINativeMethodWrapper.SqlAsyncCallbackDelegate writeDelegate;

		// Token: 0x04000061 RID: 97
		internal IntPtr key;
	}

	// Token: 0x02000008 RID: 8
	[CLSCompliant(false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal class SNI_Error
	{
		// Token: 0x04000062 RID: 98
		internal SNINativeMethodWrapper.ProviderEnum provider;

		// Token: 0x04000063 RID: 99
		internal char[] errorMessage;

		// Token: 0x04000064 RID: 100
		internal uint nativeError;

		// Token: 0x04000065 RID: 101
		internal uint sniError;

		// Token: 0x04000066 RID: 102
		internal string fileName;

		// Token: 0x04000067 RID: 103
		internal string function;

		// Token: 0x04000068 RID: 104
		internal uint lineNumber;
	}
}
