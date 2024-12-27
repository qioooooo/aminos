using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000091 RID: 145
	internal class ServiceConfigThunk
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x000043E4 File Offset: 0x000037E4
		public unsafe ServiceConfigThunk()
		{
			this.m_pUnkSC = null;
			this.m_tracker = 0;
			IUnknown* ptr;
			int num = <Module>.CoCreateInstance(ref <Module>.CLSID_CServiceConfig, null, 1, ref <Module>.IID_IUnknown, (void**)(&ptr));
			if (num == -2147221008)
			{
				int num2 = <Module>.CoInitializeEx(null, 0);
				if (num2 < 0)
				{
					Marshal.ThrowExceptionForHR(num2);
				}
				int num3 = <Module>.CoCreateInstance(ref <Module>.CLSID_CServiceConfig, null, 1, ref <Module>.IID_IUnknown, (void**)(&ptr));
				if (num3 < 0)
				{
					Marshal.ThrowExceptionForHR(num3);
				}
			}
			else if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			IntPtr intPtr = Marshal.StringToCoTaskMemUni(new string((sbyte*)(&<Module>.?A0x74e5459c.unnamed-global-0)));
			this.m_pTrackerAppName = intPtr;
			IntPtr intPtr2 = Marshal.StringToCoTaskMemUni(new string((sbyte*)(&<Module>.?A0x74e5459c.unnamed-global-1)));
			this.m_pTrackerCtxName = intPtr2;
			this.m_pUnkSC = ptr;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004498 File Offset: 0x00003898
		protected unsafe override void Finalize()
		{
			IUnknown* pUnkSC = this.m_pUnkSC;
			if (pUnkSC != null)
			{
				IUnknown* ptr = pUnkSC;
				uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr, (IntPtr)(*(*(int*)ptr + 8)));
				this.m_pUnkSC = null;
			}
			if (this.m_pTrackerAppName != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.m_pTrackerAppName);
			}
			if (this.m_pTrackerCtxName != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.m_pTrackerCtxName);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00004510 File Offset: 0x00003910
		public unsafe IUnknown* ServiceConfigUnknown
		{
			get
			{
				IUnknown* pUnkSC = this.m_pUnkSC;
				if (pUnkSC != null)
				{
					IUnknown* ptr = pUnkSC;
					uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr, (IntPtr)(*(*(int*)ptr + 4)));
				}
				return this.m_pUnkSC;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000EC RID: 236 RVA: 0x0000491C File Offset: 0x00003D1C
		public unsafe bool SupportsSysTxn
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				IUnknown* ptr = null;
				if (!<Module>.?A0x74e5459c.?fInitialized@?1??get_SupportsSysTxn@ServiceConfigThunk@Thunk@EnterpriseServices@System@@Q$AAM_NXZ@4_NA)
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					<Module>.?A0x74e5459c.?fSupportsSysTxn@?1??get_SupportsSysTxn@ServiceConfigThunk@Thunk@EnterpriseServices@System@@Q$AAM_NXZ@4_NA = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>._GUID_33caf1a1_fcb8_472b_b45e_967448ded6d8, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC))) >= 0;
					<Module>.?A0x74e5459c.?fInitialized@?1??get_SupportsSysTxn@ServiceConfigThunk@Thunk@EnterpriseServices@System@@Q$AAM_NXZ@4_NA = true;
					if (ptr != null)
					{
						IUnknown* ptr2 = ptr;
						uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
				return <Module>.?A0x74e5459c.?fSupportsSysTxn@?1??get_SupportsSysTxn@ServiceConfigThunk@Thunk@EnterpriseServices@System@@Q$AAM_NXZ@4_NA;
			}
		}

		// Token: 0x17000016 RID: 22
		// (set) Token: 0x060000ED RID: 237 RVA: 0x0000453C File Offset: 0x0000393C
		public unsafe int Inheritance
		{
			set
			{
				IServiceInheritanceConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceInheritanceConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, value, (IntPtr)(*(*(int*)ptr + 12)));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServiceInheritanceConfig* ptr2 = ptr;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (set) Token: 0x060000EE RID: 238 RVA: 0x000045AC File Offset: 0x000039AC
		public unsafe int ThreadPool
		{
			set
			{
				IServiceThreadPoolConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceThreadPoolConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, value, (IntPtr)(*(*(int*)ptr + 12)));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServiceThreadPoolConfig* ptr2 = ptr;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (set) Token: 0x060000EF RID: 239 RVA: 0x0000461C File Offset: 0x00003A1C
		public unsafe int Binding
		{
			set
			{
				IServiceThreadPoolConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceThreadPoolConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, value, (IntPtr)(*(*(int*)ptr + 16)));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServiceThreadPoolConfig* ptr2 = ptr;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x17000013 RID: 19
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x0000468C File Offset: 0x00003A8C
		public unsafe int Transaction
		{
			set
			{
				IServiceTransactionConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceTransactionConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					if (value > 0)
					{
						value--;
					}
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, value, (IntPtr)(*(*(int*)ptr + 12)));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServiceTransactionConfig* ptr2 = ptr;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004704 File Offset: 0x00003B04
		public unsafe int TxIsolationLevel
		{
			set
			{
				IServiceTransactionConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceTransactionConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, value, (IntPtr)(*(*(int*)ptr + 16)));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServiceTransactionConfig* ptr2 = ptr;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00004774 File Offset: 0x00003B74
		public unsafe int TxTimeout
		{
			set
			{
				IServiceTransactionConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceTransactionConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)), ptr, value, (IntPtr)(*(*(int*)ptr + 20)));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServiceTransactionConfig* ptr2 = ptr;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x17000010 RID: 16
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x000047E4 File Offset: 0x00003BE4
		public unsafe string TipUrl
		{
			set
			{
				IServiceTransactionConfig* ptr = null;
				IntPtr intPtr = 0;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceTransactionConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					intPtr = Marshal.StringToCoTaskMemUni(value);
					int num2 = *(int*)ptr + 24;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Char modopt(System.Runtime.CompilerServices.IsConst)*), ptr, (char*)(void*)intPtr, (IntPtr)(*num2));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.FreeCoTaskMem(intPtr);
					}
					if (ptr != null)
					{
						IServiceTransactionConfig* ptr2 = ptr;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00004880 File Offset: 0x00003C80
		public unsafe string TxDesc
		{
			set
			{
				IServiceTransactionConfig* ptr = null;
				IntPtr intPtr = 0;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceTransactionConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					intPtr = Marshal.StringToCoTaskMemUni(value);
					int num2 = *(int*)ptr + 28;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Char modopt(System.Runtime.CompilerServices.IsConst)*), ptr, (char*)(void*)intPtr, (IntPtr)(*num2));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.FreeCoTaskMem(intPtr);
					}
					if (ptr != null)
					{
						IServiceTransactionConfig* ptr2 = ptr;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x1700000E RID: 14
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004A30 File Offset: 0x00003E30
		public unsafe object Byot
		{
			set
			{
				IUnknown* ptr = null;
				ITransaction* ptr2 = null;
				IServiceTransactionConfig* ptr3 = null;
				try
				{
					int num;
					if (value != null)
					{
						ptr = (IUnknown*)(void*)Marshal.GetIUnknownForObject(value);
						num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>._GUID_0fb15084_af41_11ce_bd2b_204c4f4f5020, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr)));
						Marshal.ThrowExceptionForHR(num);
					}
					IUnknown* pUnkSC = this.m_pUnkSC;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceTransactionConfig, (void**)(&ptr3), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.EnterpriseServices.Thunk.ITransaction*), ptr3, ptr2, (IntPtr)(*(*(int*)ptr3 + 32)));
					Marshal.ThrowExceptionForHR(num);
					GC.KeepAlive(value);
				}
				finally
				{
					if (ptr3 != null)
					{
						IServiceTransactionConfig* ptr4 = ptr3;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
					}
					if (ptr != null)
					{
						IUnknown* ptr5 = ptr;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr5, (IntPtr)(*(*(int*)ptr5 + 8)));
					}
					if (ptr2 != null)
					{
						ITransaction* ptr6 = ptr2;
						uint num4 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr6, (IntPtr)(*(*(int*)ptr6 + 8)));
					}
				}
			}
		}

		// Token: 0x1700000D RID: 13
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00004970 File Offset: 0x00003D70
		public unsafe object ByotSysTxn
		{
			set
			{
				IUnknown* ptr = null;
				IUnknown* ptr2 = null;
				IServiceSysTxnConfigInternal* ptr3 = null;
				try
				{
					int num;
					if (value != null)
					{
						ptr = (IUnknown*)(void*)Marshal.GetIUnknownForObject(value);
						num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>._GUID_02558374_df2e_4dae_bd6b_1d5c994f9bdc, (void**)(&ptr2), (IntPtr)(*(*(int*)ptr)));
						Marshal.ThrowExceptionForHR(num);
					}
					IUnknown* pUnkSC = this.m_pUnkSC;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>._GUID_33caf1a1_fcb8_472b_b45e_967448ded6d8, (void**)(&ptr3), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,IUnknown*), ptr3, ptr2, (IntPtr)(*(*(int*)ptr3 + 36)));
					Marshal.ThrowExceptionForHR(num);
					GC.KeepAlive(value);
				}
				finally
				{
					if (ptr3 != null)
					{
						IServiceSysTxnConfigInternal* ptr4 = ptr3;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
					}
					if (ptr != null)
					{
						IUnknown* ptr5 = ptr;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr5, (IntPtr)(*(*(int*)ptr5 + 8)));
					}
					if (ptr2 != null)
					{
						IUnknown* ptr6 = ptr2;
						uint num4 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr6, (IntPtr)(*(*(int*)ptr6 + 8)));
					}
				}
			}
		}

		// Token: 0x1700000C RID: 12
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00004AF0 File Offset: 0x00003EF0
		public unsafe int Synchronization
		{
			set
			{
				IServiceSynchronizationConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceSynchronizationConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					if (value > 0)
					{
						value--;
					}
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, value, (IntPtr)(*(*(int*)ptr + 12)));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServiceSynchronizationConfig* ptr2 = ptr;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x1700000B RID: 11
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00004B68 File Offset: 0x00003F68
		public unsafe bool IISIntrinsics
		{
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				IServiceIISIntrinsicsConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceIISIntrinsicsConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					int num2 = (value ? 1 : 0);
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, num2, (IntPtr)(*(*(int*)ptr + 12)));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServiceIISIntrinsicsConfig* ptr2 = ptr;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x1700000A RID: 10
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00004BE0 File Offset: 0x00003FE0
		public unsafe bool COMTIIntrinsics
		{
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				IServiceComTIIntrinsicsConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceComTIIntrinsicsConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					int num2 = (value ? 1 : 0);
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, num2, (IntPtr)(*(*(int*)ptr + 12)));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServiceComTIIntrinsicsConfig* ptr2 = ptr;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x17000009 RID: 9
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00004C58 File Offset: 0x00004058
		public unsafe bool Tracker
		{
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				IServiceTrackerConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceTrackerConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					int num2 = (value ? 1 : 0);
					int num3 = *(int*)ptr + 12;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32,System.Char modopt(System.Runtime.CompilerServices.IsConst)*,System.Char modopt(System.Runtime.CompilerServices.IsConst)*), ptr, num2, (char*)(void*)this.m_pTrackerAppName, (char*)(void*)this.m_pTrackerCtxName, (IntPtr)(*num3));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServiceTrackerConfig* ptr2 = ptr;
						uint num4 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
				int num5 = (value ? 1 : 0);
				this.m_tracker = num5;
				GC.KeepAlive(this);
			}
		}

		// Token: 0x17000008 RID: 8
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00004D0C File Offset: 0x0000410C
		public unsafe string TrackerAppName
		{
			set
			{
				IServiceTrackerConfig* ptr = null;
				IntPtr intPtr = 0;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceTrackerConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					intPtr = Marshal.StringToCoTaskMemUni(value);
					int num2 = *(int*)ptr + 12;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32,System.Char modopt(System.Runtime.CompilerServices.IsConst)*,System.Char modopt(System.Runtime.CompilerServices.IsConst)*), ptr, this.m_tracker, (char*)(void*)intPtr, (char*)(void*)this.m_pTrackerCtxName, (IntPtr)(*num2));
					if (num < 0)
					{
						Marshal.FreeCoTaskMem(intPtr);
						intPtr = Marshal.StringToCoTaskMemUni(new string((sbyte*)(&<Module>.?A0x74e5459c.unnamed-global-2)));
						Marshal.ThrowExceptionForHR(num);
					}
				}
				finally
				{
					if (this.m_pTrackerAppName != IntPtr.Zero)
					{
						Marshal.FreeCoTaskMem(this.m_pTrackerAppName);
					}
					if (ptr != null)
					{
						IServiceTrackerConfig* ptr2 = ptr;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
					this.m_pTrackerAppName = intPtr;
				}
				GC.KeepAlive(this);
			}
		}

		// Token: 0x17000007 RID: 7
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00004DF8 File Offset: 0x000041F8
		public unsafe string TrackerCtxName
		{
			set
			{
				IServiceTrackerConfig* ptr = null;
				IntPtr intPtr = 0;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceTrackerConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					intPtr = Marshal.StringToCoTaskMemUni(value);
					int num2 = *(int*)ptr + 12;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32,System.Char modopt(System.Runtime.CompilerServices.IsConst)*,System.Char modopt(System.Runtime.CompilerServices.IsConst)*), ptr, this.m_tracker, (char*)(void*)this.m_pTrackerAppName, (char*)(void*)intPtr, (IntPtr)(*num2));
					if (num < 0)
					{
						Marshal.FreeCoTaskMem(intPtr);
						intPtr = Marshal.StringToCoTaskMemUni(new string((sbyte*)(&<Module>.?A0x74e5459c.unnamed-global-3)));
						Marshal.ThrowExceptionForHR(num);
					}
				}
				finally
				{
					if (this.m_pTrackerCtxName != IntPtr.Zero)
					{
						Marshal.FreeCoTaskMem(this.m_pTrackerCtxName);
					}
					if (ptr != null)
					{
						IServiceTrackerConfig* ptr2 = ptr;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
					this.m_pTrackerCtxName = intPtr;
				}
				GC.KeepAlive(this);
			}
		}

		// Token: 0x17000006 RID: 6
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00004EE4 File Offset: 0x000042E4
		public unsafe int Sxs
		{
			set
			{
				IServiceSxsConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceSxsConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, value, (IntPtr)(*(*(int*)ptr + 12)));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServiceSxsConfig* ptr2 = ptr;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00004F54 File Offset: 0x00004354
		public unsafe string SxsName
		{
			set
			{
				IServiceSxsConfig* ptr = null;
				IntPtr intPtr = 0;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceSxsConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					intPtr = Marshal.StringToCoTaskMemUni(value);
					int num2 = *(int*)ptr + 16;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Char modopt(System.Runtime.CompilerServices.IsConst)*), ptr, (char*)(void*)intPtr, (IntPtr)(*num2));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.FreeCoTaskMem(intPtr);
					}
					if (ptr != null)
					{
						IServiceSxsConfig* ptr2 = ptr;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x17000004 RID: 4
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00004FF0 File Offset: 0x000043F0
		public unsafe string SxsDirectory
		{
			set
			{
				IServiceSxsConfig* ptr = null;
				IntPtr intPtr = 0;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServiceSxsConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					intPtr = Marshal.StringToCoTaskMemUni(value);
					int num2 = *(int*)ptr + 20;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Char modopt(System.Runtime.CompilerServices.IsConst)*), ptr, (char*)(void*)intPtr, (IntPtr)(*num2));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.FreeCoTaskMem(intPtr);
					}
					if (ptr != null)
					{
						IServiceSxsConfig* ptr2 = ptr;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x17000003 RID: 3
		// (set) Token: 0x06000100 RID: 256 RVA: 0x0000508C File Offset: 0x0000448C
		public unsafe int Partition
		{
			set
			{
				IServicePartitionConfig* ptr = null;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServicePartitionConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, value, (IntPtr)(*(*(int*)ptr + 12)));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (ptr != null)
					{
						IServicePartitionConfig* ptr2 = ptr;
						uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x17000002 RID: 2
		// (set) Token: 0x06000101 RID: 257 RVA: 0x000050FC File Offset: 0x000044FC
		public unsafe Guid PartitionId
		{
			set
			{
				IServicePartitionConfig* ptr = null;
				IntPtr intPtr = 0;
				try
				{
					IUnknown* pUnkSC = this.m_pUnkSC;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), pUnkSC, ref <Module>.IID_IServicePartitionConfig, (void**)(&ptr), (IntPtr)(*(*(int*)pUnkSC)));
					Marshal.ThrowExceptionForHR(num);
					intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(value));
					Marshal.StructureToPtr(value, intPtr, false);
					int num2 = *(int*)ptr + 16;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), ptr, (void*)intPtr, (IntPtr)(*num2));
					Marshal.ThrowExceptionForHR(num);
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.FreeCoTaskMem(intPtr);
					}
					if (ptr != null)
					{
						IServicePartitionConfig* ptr2 = ptr;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
					}
				}
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005484 File Offset: 0x00004884
		public void {dtor}()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x040000B5 RID: 181
		private unsafe IUnknown* m_pUnkSC;

		// Token: 0x040000B6 RID: 182
		private int m_tracker;

		// Token: 0x040000B7 RID: 183
		private IntPtr m_pTrackerAppName;

		// Token: 0x040000B8 RID: 184
		private IntPtr m_pTrackerCtxName;
	}
}
