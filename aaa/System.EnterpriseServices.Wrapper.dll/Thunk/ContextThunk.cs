using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Thunk
{
	// Token: 0x02000036 RID: 54
	internal class ContextThunk
	{
		// Token: 0x0600008A RID: 138 RVA: 0x00001CB8 File Offset: 0x000010B8
		private ContextThunk()
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00001CCC File Offset: 0x000010CC
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool IsInTransaction()
		{
			IObjectContext* ptr = null;
			if (<Module>.GetContext(ref <Module>.IID_IObjectContext, (void**)(&ptr)) >= 0 && null != ptr)
			{
				IObjectContext* ptr2 = ptr;
				bool flag = calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 32))) != 0;
				IObjectContext* ptr3 = ptr;
				uint num = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
				return flag;
			}
			return false;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00001D14 File Offset: 0x00001114
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool IsDefaultContext()
		{
			return <Module>.IsDefaultContext() != 0;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00001D30 File Offset: 0x00001130
		public unsafe static void SetAbort()
		{
			IObjectContext* ptr = null;
			int num = <Module>.GetContext(ref <Module>.IID_IObjectContext, (void**)(&ptr));
			if (num >= 0 && null != ptr)
			{
				IObjectContext* ptr2 = ptr;
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 20)));
				IObjectContext* ptr3 = ptr;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
				if (num == 0)
				{
					return;
				}
			}
			if (num == -2147467262)
			{
				num = -2147164156;
			}
			Marshal.ThrowExceptionForHR(num);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00001D84 File Offset: 0x00001184
		public unsafe static void SetComplete()
		{
			IObjectContext* ptr = null;
			int num = <Module>.GetContext(ref <Module>.IID_IObjectContext, (void**)(&ptr));
			if (num >= 0 && null != ptr)
			{
				IObjectContext* ptr2 = ptr;
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 16)));
				IObjectContext* ptr3 = ptr;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
				if (num == 0)
				{
					return;
				}
			}
			if (num == -2147467262)
			{
				num = -2147164156;
			}
			Marshal.ThrowExceptionForHR(num);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00001DD8 File Offset: 0x000011D8
		public unsafe static void DisableCommit()
		{
			IObjectContext* ptr = null;
			int num = <Module>.GetContext(ref <Module>.IID_IObjectContext, (void**)(&ptr));
			if (num >= 0 && null != ptr)
			{
				IObjectContext* ptr2 = ptr;
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 28)));
				IObjectContext* ptr3 = ptr;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
				if (num == 0)
				{
					return;
				}
			}
			if (num == -2147467262)
			{
				num = -2147164156;
			}
			Marshal.ThrowExceptionForHR(num);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00001E2C File Offset: 0x0000122C
		public unsafe static void EnableCommit()
		{
			IObjectContext* ptr = null;
			int num = <Module>.GetContext(ref <Module>.IID_IObjectContext, (void**)(&ptr));
			if (num >= 0 && null != ptr)
			{
				IObjectContext* ptr2 = ptr;
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 24)));
				IObjectContext* ptr3 = ptr;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
				if (num == 0)
				{
					return;
				}
			}
			if (num == -2147467262)
			{
				num = -2147164156;
			}
			Marshal.ThrowExceptionForHR(num);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00001E80 File Offset: 0x00001280
		public unsafe static Guid GetTransactionId()
		{
			Guid guid = default(Guid);
			IObjectContextInfo* ptr = null;
			int num = <Module>.GetContext(ref <Module>.IID_IObjectContextInfo, (void**)(&ptr));
			if (num >= 0 && null != ptr)
			{
				_GUID guid2;
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID*), ptr, &guid2, (IntPtr)(*(*(int*)ptr + 20)));
				IObjectContextInfo* ptr2 = ptr;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
				if (num == 0)
				{
					Guid guid3 = new Guid(guid2, *((ref guid2) + 4), *((ref guid2) + 6), *((ref guid2) + 8), *((ref guid2) + 9), *((ref guid2) + 10), *((ref guid2) + 11), *((ref guid2) + 12), *((ref guid2) + 13), *((ref guid2) + 14), *((ref guid2) + 15));
					return guid3;
				}
			}
			if (num == -2147467262)
			{
				num = -2147164156;
			}
			Marshal.ThrowExceptionForHR(num);
			return guid;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00001F24 File Offset: 0x00001324
		public unsafe static int GetMyTransactionVote()
		{
			IContextState* ptr = null;
			int num = <Module>.GetContext(ref <Module>.IID_IContextState, (void**)(&ptr));
			int num2;
			if (num >= 0 && null != ptr)
			{
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32*), ptr, &num2, (IntPtr)(*(*(int*)ptr + 24)));
				IContextState* ptr2 = ptr;
				uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
				if (num >= 0)
				{
					return num2;
				}
			}
			if (num == -2147467262)
			{
				num = -2147164156;
			}
			Marshal.ThrowExceptionForHR(num);
			return num2;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00001F80 File Offset: 0x00001380
		public unsafe static void SetMyTransactionVote(int vote)
		{
			IContextState* ptr = null;
			int num = <Module>.GetContext(ref <Module>.IID_IContextState, (void**)(&ptr));
			if (num >= 0 && null != ptr)
			{
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32), ptr, vote, (IntPtr)(*(*(int*)ptr + 20)));
				IContextState* ptr2 = ptr;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
			}
			if (num == -2147467262)
			{
				num = -2147164156;
			}
			else if (num >= 0)
			{
				return;
			}
			Marshal.ThrowExceptionForHR(num);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00001FD8 File Offset: 0x000013D8
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool GetDeactivateOnReturn()
		{
			IContextState* ptr = null;
			int num = <Module>.GetContext(ref <Module>.IID_IContextState, (void**)(&ptr));
			if (num >= 0 && null != ptr)
			{
				short num2;
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int16*), ptr, &num2, (IntPtr)(*(*(int*)ptr + 16)));
				IContextState* ptr2 = ptr;
				uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
				if (num >= 0)
				{
					return -1 == num2;
				}
			}
			if (num == -2147467262)
			{
				num = -2147164156;
			}
			Marshal.ThrowExceptionForHR(num);
			return false;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00002038 File Offset: 0x00001438
		public unsafe static void SetDeactivateOnReturn([MarshalAs(UnmanagedType.U1)] bool deactivateOnReturn)
		{
			IContextState* ptr = null;
			int num = <Module>.GetContext(ref <Module>.IID_IContextState, (void**)(&ptr));
			if (num >= 0 && null != ptr)
			{
				int num3;
				int num2 = (num3 = -1);
				if (!deactivateOnReturn)
				{
					num3 = ~num2;
				}
				short num4 = (short)num3;
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int16), ptr, num4, (IntPtr)(*(*(int*)ptr + 12)));
				IContextState* ptr2 = ptr;
				uint num5 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 8)));
			}
			if (num == -2147467262)
			{
				num = -2147164156;
			}
			else if (num >= 0)
			{
				return;
			}
			Marshal.ThrowExceptionForHR(num);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002098 File Offset: 0x00001498
		public unsafe static object GetTransaction()
		{
			IUnknown* ptr = null;
			IObjectContextInfo* ptr2 = null;
			int num = <Module>.GetContext(ref <Module>.IID_IObjectContextInfo, (void**)(&ptr2));
			if (num >= 0 && null != ptr2)
			{
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,IUnknown**), ptr2, &ptr, (IntPtr)(*(*(int*)ptr2 + 16)));
				IObjectContextInfo* ptr3 = ptr2;
				uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, (IntPtr)(*(*(int*)ptr3 + 8)));
			}
			if (num == -2147467262)
			{
				num = -2147164156;
			}
			else if (num >= 0)
			{
				goto IL_004E;
			}
			Marshal.ThrowExceptionForHR(num);
			IL_004E:
			object obj = null;
			if (ptr != null)
			{
				try
				{
					IntPtr intPtr = new IntPtr((void*)ptr);
					obj = Marshal.GetObjectForIUnknown(intPtr);
				}
				finally
				{
					IUnknown* ptr4 = ptr;
					uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
				}
			}
			return obj;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002134 File Offset: 0x00001534
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool GetTransactionProxyOrTransaction(ref object ppTx, TxInfo pTxInfo)
		{
			IObjectContext* ptr = null;
			pTxInfo.isDtcTransaction = false;
			bool flag = false;
			ppTx = null;
			int num = <Module>.GetContext(ref <Module>.IID_IObjectContext, (void**)(&ptr));
			if (num >= 0 && null != ptr)
			{
				IObjectContext* ptr2 = ptr;
				if (calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, (IntPtr)(*(*(int*)ptr2 + 32))) != null)
				{
					flag = true;
					IContextTransactionInfoPrivate* ptr3 = null;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>._GUID_7d40fcc8_f81e_462e_bba1_8a99ebdc826c, (void**)(&ptr3), (IntPtr)(*(*(int*)ptr)));
					if (num == 0)
					{
						try
						{
							IUnknown* ptr4 = null;
							num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,IUnknown**), ptr3, &ptr4, (IntPtr)(*(*(int*)ptr3 + 12)));
							if (num >= 0)
							{
								if (ptr4 == null)
								{
									pTxInfo.isDtcTransaction = false;
									int num2;
									uint num3;
									num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.Int32 modopt(System.Runtime.CompilerServices.IsLong)*,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)*), ptr3, (int*)(&num2), (uint*)(&num3), (IntPtr)(*(*(int*)ptr3 + 20)));
									if (num >= 0)
									{
										pTxInfo.IsolationLevel = num2;
										pTxInfo.timeout = (int)num3;
									}
								}
								else
								{
									IUnknown* ptr5 = null;
									num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr4, ref <Module>._GUID_0fb15084_af41_11ce_bd2b_204c4f4f5020, (void**)(&ptr5), (IntPtr)(*(*(int*)ptr4)));
									if (num == 0)
									{
										pTxInfo.isDtcTransaction = true;
										IUnknown* ptr6 = ptr5;
										uint num4 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr6, (IntPtr)(*(*(int*)ptr6 + 8)));
									}
									else if (num == -2147467262)
									{
										num = 0;
									}
									try
									{
										if (num >= 0)
										{
											IntPtr intPtr = new IntPtr((void*)ptr4);
											ppTx = Marshal.GetObjectForIUnknown(intPtr);
										}
									}
									finally
									{
										IUnknown* ptr7 = ptr4;
										uint num5 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr7, (IntPtr)(*(*(int*)ptr7 + 8)));
									}
								}
							}
							goto IL_0186;
						}
						finally
						{
							IContextTransactionInfoPrivate* ptr8 = ptr3;
							uint num6 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr8, (IntPtr)(*(*(int*)ptr8 + 8)));
						}
					}
					if (num == -2147467262)
					{
						IObjectContextInfo* ptr9 = null;
						IUnknown* ptr10 = null;
						num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>.IID_IObjectContextInfo, (void**)(&ptr9), (IntPtr)(*(*(int*)ptr)));
						if (num >= 0)
						{
							try
							{
								num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,IUnknown**), ptr9, &ptr10, (IntPtr)(*(*(int*)ptr9 + 16)));
								if (num >= 0)
								{
									pTxInfo.isDtcTransaction = true;
									try
									{
										IntPtr intPtr2 = new IntPtr((void*)ptr10);
										ppTx = Marshal.GetObjectForIUnknown(intPtr2);
									}
									finally
									{
										IUnknown* ptr11 = ptr10;
										uint num7 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr11, (IntPtr)(*(*(int*)ptr11 + 8)));
									}
								}
							}
							finally
							{
								IObjectContextInfo* ptr12 = ptr9;
								uint num8 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr12, (IntPtr)(*(*(int*)ptr12 + 8)));
							}
						}
					}
				}
				else
				{
					flag = false;
				}
				IL_0186:
				IObjectContext* ptr13 = ptr;
				uint num9 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr13, (IntPtr)(*(*(int*)ptr13 + 8)));
			}
			else if (num == -2147467262)
			{
				return flag;
			}
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			return flag;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002350 File Offset: 0x00001750
		public unsafe static Guid RegisterTransactionProxy(object pTransactionProxy)
		{
			IContextTransactionInfoPrivate* ptr = null;
			int num = <Module>.GetContext(ref <Module>._GUID_7d40fcc8_f81e_462e_bba1_8a99ebdc826c, (void**)(&ptr));
			if (num >= 0)
			{
				try
				{
					IUnknown* ptr2 = (IUnknown*)Marshal.GetIUnknownForObject(pTransactionProxy).ToPointer();
					ITransactionProxyPrivate* ptr3 = null;
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr2, ref <Module>._GUID_02558374_df2e_4dae_bd6b_1d5c994f9bdc, (void**)(&ptr3), (IntPtr)(*(*(int*)ptr2)));
					IUnknown* ptr4 = ptr2;
					uint num2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, (IntPtr)(*(*(int*)ptr4 + 8)));
					if (num >= 0)
					{
						_GUID guid;
						num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.EnterpriseServices.Thunk.ITransactionProxyPrivate*,_GUID*), ptr, ptr3, &guid, (IntPtr)(*(*(int*)ptr + 16)));
						ITransactionProxyPrivate* ptr5 = ptr3;
						uint num3 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr5, (IntPtr)(*(*(int*)ptr5 + 8)));
						if (num >= 0)
						{
							Guid guid2 = new Guid(guid, *((ref guid) + 4), *((ref guid) + 6), *((ref guid) + 8), *((ref guid) + 9), *((ref guid) + 10), *((ref guid) + 11), *((ref guid) + 12), *((ref guid) + 13), *((ref guid) + 14), *((ref guid) + 15));
							return guid2;
						}
					}
				}
				finally
				{
					IContextTransactionInfoPrivate* ptr6 = ptr;
					uint num4 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr6, (IntPtr)(*(*(int*)ptr6 + 8)));
				}
				if (num < 0)
				{
					goto IL_00C5;
				}
				goto IL_00CB;
			}
			IL_00C5:
			Marshal.ThrowExceptionForHR(num);
			IL_00CB:
			return Guid.Empty;
		}
	}
}
