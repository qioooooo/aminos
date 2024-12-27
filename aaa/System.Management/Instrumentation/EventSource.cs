using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace System.Management.Instrumentation
{
	// Token: 0x0200009E RID: 158
	internal sealed class EventSource : IWbemProviderInit, IWbemEventProvider, IWbemEventProviderQuerySink, IWbemEventProviderSecurity, IWbemServices_Old
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x00021C9C File Offset: 0x00020C9C
		public EventSource(string namespaceName, string appName, InstrumentedAssembly instrumentedAssembly)
		{
			lock (EventSource.eventSources)
			{
				if (EventSource.shutdownInProgress == 0)
				{
					this.instrumentedAssembly = instrumentedAssembly;
					int num = this.registrar.Register_(0, null, null, null, namespaceName, appName, this);
					if (num != 0)
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
					EventSource.eventSources.Add(this);
				}
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00021D5C File Offset: 0x00020D5C
		~EventSource()
		{
			this.UnRegister();
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00021D88 File Offset: 0x00020D88
		private void UnRegister()
		{
			lock (this)
			{
				if (this.registrar != null)
				{
					if (this.workerThreadInitialized)
					{
						this.alive = false;
						this.doIndicate.Set();
						GC.KeepAlive(this);
						this.workerThreadInitialized = false;
					}
					this.registrar.UnRegister_();
					this.registrar = null;
				}
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00021DFC File Offset: 0x00020DFC
		private static void ProcessExit(object o, EventArgs args)
		{
			if (EventSource.shutdownInProgress != 0)
			{
				return;
			}
			Interlocked.Increment(ref EventSource.shutdownInProgress);
			try
			{
				EventSource.preventShutdownLock.AcquireWriterLock(-1);
				lock (EventSource.eventSources)
				{
					foreach (object obj in EventSource.eventSources)
					{
						EventSource eventSource = (EventSource)obj;
						eventSource.UnRegister();
					}
				}
			}
			finally
			{
				EventSource.preventShutdownLock.ReleaseWriterLock();
				Thread.Sleep(50);
				EventSource.preventShutdownLock.AcquireWriterLock(-1);
				EventSource.preventShutdownLock.ReleaseWriterLock();
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00021EC8 File Offset: 0x00020EC8
		static EventSource()
		{
			AppDomain.CurrentDomain.ProcessExit += EventSource.ProcessExit;
			AppDomain.CurrentDomain.DomainUnload += EventSource.ProcessExit;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00021F1C File Offset: 0x00020F1C
		public void MTAWorkerThread2()
		{
			for (;;)
			{
				this.doIndicate.WaitOne();
				if (!this.alive)
				{
					break;
				}
				for (;;)
				{
					EventSource.MTARequest mtarequest = null;
					lock (this.critSec)
					{
						if (this.reqList.Count <= 0)
						{
							break;
						}
						mtarequest = (EventSource.MTARequest)this.reqList[0];
						this.reqList.RemoveAt(0);
					}
					try
					{
						if (this.pSinkMTA != null)
						{
							int num = this.pSinkMTA.Indicate_(mtarequest.lengthFromSTA, mtarequest.objectsFromSTA);
							if (num < 0)
							{
								if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
								{
									ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
								}
								else
								{
									Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
								}
							}
						}
					}
					catch (Exception ex)
					{
						mtarequest.exception = ex;
					}
					finally
					{
						mtarequest.doneIndicate.Set();
						GC.KeepAlive(this);
					}
				}
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00022020 File Offset: 0x00021020
		public void IndicateEvents(int length, IntPtr[] objects)
		{
			if (this.pSinkMTA == null)
			{
				return;
			}
			if (MTAHelper.IsNoContextMTA())
			{
				int num = this.pSinkMTA.Indicate_(length, objects);
				if (num < 0)
				{
					if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			else
			{
				EventSource.MTARequest mtarequest = new EventSource.MTARequest(length, objects);
				lock (this.critSec)
				{
					if (!this.workerThreadInitialized)
					{
						Thread thread = new Thread(new ThreadStart(this.MTAWorkerThread2));
						thread.IsBackground = true;
						thread.SetApartmentState(ApartmentState.MTA);
						thread.Start();
						this.workerThreadInitialized = true;
					}
					int num2 = this.reqList.Add(mtarequest);
					if (!this.doIndicate.Set())
					{
						this.reqList.RemoveAt(num2);
						throw new ManagementException(RC.GetString("WORKER_THREAD_WAKEUP_FAILED"));
					}
				}
				mtarequest.doneIndicate.WaitOne();
				if (mtarequest.exception != null)
				{
					throw mtarequest.exception;
				}
			}
			GC.KeepAlive(this);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00022140 File Offset: 0x00021140
		private void RelocateSinkRCWToMTA()
		{
			new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethodWithParam(this.RelocateSinkRCWToMTA_ThreadFuncion))
			{
				Parameter = this
			}.Start();
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0002216C File Offset: 0x0002116C
		private void RelocateSinkRCWToMTA_ThreadFuncion(object param)
		{
			EventSource eventSource = (EventSource)param;
			eventSource.pSinkMTA = (IWbemObjectSink)EventSource.RelocateRCWToCurrentApartment(eventSource.pSinkNA);
			eventSource.pSinkNA = null;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x000221A0 File Offset: 0x000211A0
		private void RelocateNamespaceRCWToMTA()
		{
			new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethodWithParam(this.RelocateNamespaceRCWToMTA_ThreadFuncion))
			{
				Parameter = this
			}.Start();
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x000221CC File Offset: 0x000211CC
		private void RelocateNamespaceRCWToMTA_ThreadFuncion(object param)
		{
			EventSource eventSource = (EventSource)param;
			eventSource.pNamespaceMTA = (IWbemServices)EventSource.RelocateRCWToCurrentApartment(eventSource.pNamespaceNA);
			eventSource.pNamespaceNA = null;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00022200 File Offset: 0x00021200
		private static object RelocateRCWToCurrentApartment(object comObject)
		{
			if (comObject == null)
			{
				return null;
			}
			IntPtr iunknownForObject = Marshal.GetIUnknownForObject(comObject);
			int num = Marshal.ReleaseComObject(comObject);
			if (num != 0)
			{
				throw new Exception();
			}
			comObject = Marshal.GetObjectForIUnknown(iunknownForObject);
			Marshal.Release(iunknownForObject);
			return comObject;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00022239 File Offset: 0x00021239
		public bool Any()
		{
			return this.pSinkMTA == null || this.mapQueryIdToQuery.Count == 0;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00022254 File Offset: 0x00021254
		int IWbemProviderInit.Initialize_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszUser, [In] int lFlags, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszNamespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszLocale, [MarshalAs(UnmanagedType.Interface)] [In] IWbemServices pNamespace, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemProviderInitSink pInitSink)
		{
			this.pNamespaceNA = pNamespace;
			this.RelocateNamespaceRCWToMTA();
			this.pSinkNA = null;
			this.pSinkMTA = null;
			lock (this.mapQueryIdToQuery)
			{
				this.mapQueryIdToQuery.Clear();
			}
			pInitSink.SetStatus_(0, 0);
			Marshal.ReleaseComObject(pInitSink);
			return 0;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000222C4 File Offset: 0x000212C4
		int IWbemEventProvider.ProvideEvents_([MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pSink, [In] int lFlags)
		{
			this.pSinkNA = pSink;
			this.RelocateSinkRCWToMTA();
			return 0;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000222D4 File Offset: 0x000212D4
		int IWbemEventProviderQuerySink.NewQuery_([In] uint dwId, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQueryLanguage, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQuery)
		{
			lock (this.mapQueryIdToQuery)
			{
				if (this.mapQueryIdToQuery.ContainsKey(dwId))
				{
					this.mapQueryIdToQuery.Remove(dwId);
				}
				this.mapQueryIdToQuery.Add(dwId, wszQuery);
			}
			return 0;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00022340 File Offset: 0x00021340
		int IWbemEventProviderQuerySink.CancelQuery_([In] uint dwId)
		{
			lock (this.mapQueryIdToQuery)
			{
				this.mapQueryIdToQuery.Remove(dwId);
			}
			return 0;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00022388 File Offset: 0x00021388
		int IWbemEventProviderSecurity.AccessCheck_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszQueryLanguage, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQuery, [In] int lSidLength, [In] ref byte pSid)
		{
			return 0;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0002238B File Offset: 0x0002138B
		int IWbemServices_Old.OpenNamespace_([MarshalAs(UnmanagedType.BStr)] [In] string strNamespace, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemServices ppWorkingNamespace, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00022392 File Offset: 0x00021392
		int IWbemServices_Old.CancelAsyncCall_([MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pSink)
		{
			return -2147217396;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00022399 File Offset: 0x00021399
		int IWbemServices_Old.QueryObjectSink_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemObjectSink ppResponseHandler)
		{
			ppResponseHandler = null;
			return -2147217396;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000223A3 File Offset: 0x000213A3
		int IWbemServices_Old.GetObject_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppObject, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x000223AC File Offset: 0x000213AC
		int IWbemServices_Old.GetObjectAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			Match match = Regex.Match(strObjectPath.ToLower(CultureInfo.InvariantCulture), "(.*?)\\.instanceid=\"(.*?)\",processid=\"(.*?)\"");
			if (!match.Success)
			{
				pResponseHandler.SetStatus_(0, -2147217406, null, IntPtr.Zero);
				Marshal.ReleaseComObject(pResponseHandler);
				return -2147217406;
			}
			string value = match.Groups[1].Value;
			string value2 = match.Groups[2].Value;
			string value3 = match.Groups[3].Value;
			if (Instrumentation.ProcessIdentity != value3)
			{
				pResponseHandler.SetStatus_(0, -2147217406, null, IntPtr.Zero);
				Marshal.ReleaseComObject(pResponseHandler);
				return -2147217406;
			}
			int num = ((IConvertible)value2).ToInt32((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int)));
			object obj = null;
			try
			{
				InstrumentedAssembly.readerWriterLock.AcquireReaderLock(-1);
				obj = InstrumentedAssembly.mapIDToPublishedObject[num.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int)))];
			}
			finally
			{
				InstrumentedAssembly.readerWriterLock.ReleaseReaderLock();
			}
			if (obj != null)
			{
				Type type = (Type)this.instrumentedAssembly.mapTypeToConverter[obj.GetType()];
				if (type != null)
				{
					object obj2 = Activator.CreateInstance(type);
					ConvertToWMI convertToWMI = (ConvertToWMI)Delegate.CreateDelegate(typeof(ConvertToWMI), obj2, "ToWMI");
					lock (obj)
					{
						convertToWMI(obj);
					}
					IntPtr[] array = new IntPtr[] { (IntPtr)obj2.GetType().GetField("instWbemObjectAccessIP").GetValue(obj2) };
					Marshal.AddRef(array[0]);
					IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = new IWbemClassObjectFreeThreaded(array[0]);
					object obj4 = num;
					wbemClassObjectFreeThreaded.Put_("InstanceId", 0, ref obj4, 0);
					obj4 = Instrumentation.ProcessIdentity;
					wbemClassObjectFreeThreaded.Put_("ProcessId", 0, ref obj4, 0);
					pResponseHandler.Indicate_(1, array);
					pResponseHandler.SetStatus_(0, 0, null, IntPtr.Zero);
					Marshal.ReleaseComObject(pResponseHandler);
					return 0;
				}
			}
			pResponseHandler.SetStatus_(0, -2147217406, null, IntPtr.Zero);
			Marshal.ReleaseComObject(pResponseHandler);
			return -2147217406;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00022624 File Offset: 0x00021624
		int IWbemServices_Old.PutClass_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObject, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0002262B File Offset: 0x0002162B
		int IWbemServices_Old.PutClassAsync_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObject, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00022632 File Offset: 0x00021632
		int IWbemServices_Old.DeleteClass_([MarshalAs(UnmanagedType.BStr)] [In] string strClass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00022639 File Offset: 0x00021639
		int IWbemServices_Old.DeleteClassAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strClass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00022640 File Offset: 0x00021640
		int IWbemServices_Old.CreateClassEnum_([MarshalAs(UnmanagedType.BStr)] [In] string strSuperclass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum)
		{
			ppEnum = null;
			return -2147217396;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0002264B File Offset: 0x0002164B
		int IWbemServices_Old.CreateClassEnumAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strSuperclass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00022652 File Offset: 0x00021652
		int IWbemServices_Old.PutInstance_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInst, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00022659 File Offset: 0x00021659
		int IWbemServices_Old.PutInstanceAsync_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInst, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00022660 File Offset: 0x00021660
		int IWbemServices_Old.DeleteInstance_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00022667 File Offset: 0x00021667
		int IWbemServices_Old.DeleteInstanceAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0002266E File Offset: 0x0002166E
		int IWbemServices_Old.CreateInstanceEnum_([MarshalAs(UnmanagedType.BStr)] [In] string strFilter, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum)
		{
			ppEnum = null;
			return -2147217396;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0002267C File Offset: 0x0002167C
		int IWbemServices_Old.CreateInstanceEnumAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strFilter, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			try
			{
				EventSource.preventShutdownLock.AcquireReaderLock(-1);
				if (EventSource.shutdownInProgress != 0)
				{
					return 0;
				}
				uint num = (uint)(Environment.TickCount + 100);
				Type type = null;
				foreach (object obj in this.instrumentedAssembly.mapTypeToConverter.Keys)
				{
					Type type2 = (Type)obj;
					if (string.Compare(ManagedNameAttribute.GetMemberName(type2), strFilter, StringComparison.Ordinal) == 0)
					{
						type = type2;
						break;
					}
				}
				if (type == null)
				{
					return 0;
				}
				int num2 = 64;
				IntPtr[] array = new IntPtr[num2];
				IntPtr[] array2 = new IntPtr[num2];
				ConvertToWMI[] array3 = new ConvertToWMI[num2];
				IWbemClassObjectFreeThreaded[] array4 = new IWbemClassObjectFreeThreaded[num2];
				int num3 = 0;
				int num4 = 0;
				object processIdentity = Instrumentation.ProcessIdentity;
				try
				{
					InstrumentedAssembly.readerWriterLock.AcquireReaderLock(-1);
					foreach (object obj2 in InstrumentedAssembly.mapIDToPublishedObject)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
						if (EventSource.shutdownInProgress != 0)
						{
							return 0;
						}
						if (type == dictionaryEntry.Value.GetType())
						{
							if (array3[num4] == null)
							{
								object obj3 = Activator.CreateInstance((Type)this.instrumentedAssembly.mapTypeToConverter[type]);
								array3[num4] = (ConvertToWMI)Delegate.CreateDelegate(typeof(ConvertToWMI), obj3, "ToWMI");
								lock (dictionaryEntry.Value)
								{
									array3[num4](dictionaryEntry.Value);
								}
								array[num4] = (IntPtr)obj3.GetType().GetField("instWbemObjectAccessIP").GetValue(obj3);
								Marshal.AddRef(array[num4]);
								array4[num4] = new IWbemClassObjectFreeThreaded(array[num4]);
								array4[num4].Put_("ProcessId", 0, ref processIdentity, 0);
								if (num4 == 0)
								{
									int num5;
									WmiNetUtilsHelper.GetPropertyHandle_f27(27, array4[num4], "InstanceId", out num5, out num3);
								}
							}
							else
							{
								lock (dictionaryEntry.Value)
								{
									array3[num4](dictionaryEntry.Value);
								}
								array[num4] = (IntPtr)array3[num4].Target.GetType().GetField("instWbemObjectAccessIP").GetValue(array3[num4].Target);
								Marshal.AddRef(array[num4]);
								array4[num4] = new IWbemClassObjectFreeThreaded(array[num4]);
								array4[num4].Put_("ProcessId", 0, ref processIdentity, 0);
								if (num4 == 0)
								{
									int num6;
									WmiNetUtilsHelper.GetPropertyHandle_f27(27, array4[num4], "InstanceId", out num6, out num3);
								}
							}
							string text = (string)dictionaryEntry.Key;
							WmiNetUtilsHelper.WritePropertyValue_f28(28, array4[num4], num3, (text.Length + 1) * 2, text);
							num4++;
							if (num4 == num2 || Environment.TickCount >= (int)num)
							{
								for (int i = 0; i < num4; i++)
								{
									WmiNetUtilsHelper.Clone_f(12, array[i], out array2[i]);
								}
								int num7 = pResponseHandler.Indicate_(num4, array2);
								for (int j = 0; j < num4; j++)
								{
									Marshal.Release(array2[j]);
								}
								if (num7 != 0)
								{
									return 0;
								}
								num4 = 0;
								num = (uint)(Environment.TickCount + 100);
							}
						}
					}
				}
				finally
				{
					InstrumentedAssembly.readerWriterLock.ReleaseReaderLock();
				}
				if (num4 > 0)
				{
					for (int k = 0; k < num4; k++)
					{
						WmiNetUtilsHelper.Clone_f(12, array[k], out array2[k]);
					}
					pResponseHandler.Indicate_(num4, array2);
					for (int l = 0; l < num4; l++)
					{
						Marshal.Release(array2[l]);
					}
				}
			}
			finally
			{
				pResponseHandler.SetStatus_(0, 0, null, IntPtr.Zero);
				Marshal.ReleaseComObject(pResponseHandler);
				EventSource.preventShutdownLock.ReleaseReaderLock();
			}
			return 0;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00022B7C File Offset: 0x00021B7C
		int IWbemServices_Old.ExecQuery_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum)
		{
			ppEnum = null;
			return -2147217396;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00022B87 File Offset: 0x00021B87
		int IWbemServices_Old.ExecQueryAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00022B8E File Offset: 0x00021B8E
		int IWbemServices_Old.ExecNotificationQuery_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum)
		{
			ppEnum = null;
			return -2147217396;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00022B99 File Offset: 0x00021B99
		int IWbemServices_Old.ExecNotificationQueryAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00022BA0 File Offset: 0x00021BA0
		int IWbemServices_Old.ExecMethod_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [MarshalAs(UnmanagedType.BStr)] [In] string strMethodName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInParams, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppOutParams, [In] IntPtr ppCallResult)
		{
			return -2147217396;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00022BA7 File Offset: 0x00021BA7
		int IWbemServices_Old.ExecMethodAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [MarshalAs(UnmanagedType.BStr)] [In] string strMethodName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInParams, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler)
		{
			return -2147217396;
		}

		// Token: 0x04000273 RID: 627
		private IWbemDecoupledRegistrar registrar = (IWbemDecoupledRegistrar)new WbemDecoupledRegistrar();

		// Token: 0x04000274 RID: 628
		private static ArrayList eventSources = new ArrayList();

		// Token: 0x04000275 RID: 629
		private InstrumentedAssembly instrumentedAssembly;

		// Token: 0x04000276 RID: 630
		private static int shutdownInProgress = 0;

		// Token: 0x04000277 RID: 631
		private static ReaderWriterLock preventShutdownLock = new ReaderWriterLock();

		// Token: 0x04000278 RID: 632
		private IWbemServices pNamespaceNA;

		// Token: 0x04000279 RID: 633
		private IWbemObjectSink pSinkNA;

		// Token: 0x0400027A RID: 634
		private IWbemServices pNamespaceMTA;

		// Token: 0x0400027B RID: 635
		private IWbemObjectSink pSinkMTA;

		// Token: 0x0400027C RID: 636
		private ArrayList reqList = new ArrayList(3);

		// Token: 0x0400027D RID: 637
		private object critSec = new object();

		// Token: 0x0400027E RID: 638
		private AutoResetEvent doIndicate = new AutoResetEvent(false);

		// Token: 0x0400027F RID: 639
		private bool workerThreadInitialized;

		// Token: 0x04000280 RID: 640
		private bool alive = true;

		// Token: 0x04000281 RID: 641
		private Hashtable mapQueryIdToQuery = new Hashtable();

		// Token: 0x0200009F RID: 159
		private class MTARequest
		{
			// Token: 0x060004B6 RID: 1206 RVA: 0x00022BAE File Offset: 0x00021BAE
			public MTARequest(int length, IntPtr[] objects)
			{
				this.lengthFromSTA = length;
				this.objectsFromSTA = objects;
			}

			// Token: 0x04000282 RID: 642
			public AutoResetEvent doneIndicate = new AutoResetEvent(false);

			// Token: 0x04000283 RID: 643
			public Exception exception;

			// Token: 0x04000284 RID: 644
			public int lengthFromSTA = -1;

			// Token: 0x04000285 RID: 645
			public IntPtr[] objectsFromSTA;
		}
	}
}
