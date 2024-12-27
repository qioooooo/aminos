using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Microsoft.CSharp;

namespace System.Management.Instrumentation
{
	// Token: 0x020000A4 RID: 164
	internal class InstrumentedAssembly
	{
		// Token: 0x060004D0 RID: 1232 RVA: 0x00022EFC File Offset: 0x00021EFC
		private void InitEventSource(object param)
		{
			InstrumentedAssembly instrumentedAssembly = (InstrumentedAssembly)param;
			instrumentedAssembly.source = new EventSource(instrumentedAssembly.naming.NamespaceName, instrumentedAssembly.naming.DecoupledProviderInstanceName, this);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00022F34 File Offset: 0x00021F34
		public void FindReferences(Type type, CompilerParameters parameters)
		{
			if (!parameters.ReferencedAssemblies.Contains(type.Assembly.Location))
			{
				parameters.ReferencedAssemblies.Add(type.Assembly.Location);
			}
			if (type.BaseType != null)
			{
				this.FindReferences(type.BaseType, parameters);
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				if (type2.Assembly != type.Assembly)
				{
					this.FindReferences(type2, parameters);
				}
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00022FB4 File Offset: 0x00021FB4
		public bool IsInstrumentedType(Type type)
		{
			if (type.GetInterface("System.Management.Instrumentation.IEvent", false) != null || type.GetInterface("System.Management.Instrumentation.IInstance", false) != null)
			{
				return true;
			}
			object[] customAttributes = type.GetCustomAttributes(typeof(InstrumentationClassAttribute), true);
			return customAttributes != null && customAttributes.Length != 0;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00022FFC File Offset: 0x00021FFC
		public InstrumentedAssembly(Assembly assembly, SchemaNaming naming)
		{
			SecurityHelper.UnmanagedCode.Demand();
			this.naming = naming;
			Assembly assembly2 = naming.PrecompiledAssembly;
			if (assembly2 == null)
			{
				CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider();
				CompilerParameters compilerParameters = new CompilerParameters();
				compilerParameters.GenerateInMemory = true;
				compilerParameters.ReferencedAssemblies.Add(assembly.Location);
				compilerParameters.ReferencedAssemblies.Add(typeof(BaseEvent).Assembly.Location);
				compilerParameters.ReferencedAssemblies.Add(typeof(Component).Assembly.Location);
				foreach (Type type in assembly.GetTypes())
				{
					if (this.IsInstrumentedType(type))
					{
						this.FindReferences(type, compilerParameters);
					}
				}
				CompilerResults compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, new string[] { naming.Code });
				foreach (object obj in compilerResults.Errors)
				{
					CompilerError compilerError = (CompilerError)obj;
					Console.WriteLine(compilerError.ToString());
				}
				if (compilerResults.Errors.HasErrors)
				{
					throw new Exception(RC.GetString("FAILED_TO_BUILD_GENERATED_ASSEMBLY"));
				}
				assembly2 = compilerResults.CompiledAssembly;
			}
			Type type2 = assembly2.GetType("WMINET_Converter");
			this.mapTypeToConverter = (Hashtable)type2.GetField("mapTypeToConverter").GetValue(null);
			if (!MTAHelper.IsNoContextMTA())
			{
				new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethodWithParam(this.InitEventSource))
				{
					Parameter = this
				}.Start();
				return;
			}
			this.InitEventSource(this);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000231C8 File Offset: 0x000221C8
		public void Fire(object o)
		{
			SecurityHelper.UnmanagedCode.Demand();
			this.Fire(o.GetType(), o);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x000231E4 File Offset: 0x000221E4
		public void Publish(object o)
		{
			SecurityHelper.UnmanagedCode.Demand();
			try
			{
				InstrumentedAssembly.readerWriterLock.AcquireWriterLock(-1);
				if (!InstrumentedAssembly.mapPublishedObjectToID.ContainsKey(o))
				{
					InstrumentedAssembly.mapIDToPublishedObject.Add(InstrumentedAssembly.upcountId.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int))), o);
					InstrumentedAssembly.mapPublishedObjectToID.Add(o, InstrumentedAssembly.upcountId);
					InstrumentedAssembly.upcountId++;
				}
			}
			finally
			{
				InstrumentedAssembly.readerWriterLock.ReleaseWriterLock();
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00023284 File Offset: 0x00022284
		public void Revoke(object o)
		{
			SecurityHelper.UnmanagedCode.Demand();
			try
			{
				InstrumentedAssembly.readerWriterLock.AcquireWriterLock(-1);
				object obj = InstrumentedAssembly.mapPublishedObjectToID[o];
				if (obj != null)
				{
					int num = (int)obj;
					InstrumentedAssembly.mapPublishedObjectToID.Remove(o);
					InstrumentedAssembly.mapIDToPublishedObject.Remove(num.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int))));
				}
			}
			finally
			{
				InstrumentedAssembly.readerWriterLock.ReleaseWriterLock();
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00023310 File Offset: 0x00022310
		public void SetBatchSize(Type t, int batchSize)
		{
			this.GetTypeInfo(t).SetBatchSize(batchSize);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00023320 File Offset: 0x00022320
		private InstrumentedAssembly.TypeInfo GetTypeInfo(Type t)
		{
			InstrumentedAssembly.TypeInfo typeInfo;
			lock (this.mapTypeToTypeInfo)
			{
				if (this.lastType == t)
				{
					typeInfo = this.lastTypeInfo;
				}
				else
				{
					this.lastType = t;
					InstrumentedAssembly.TypeInfo typeInfo2 = (InstrumentedAssembly.TypeInfo)this.mapTypeToTypeInfo[t];
					if (typeInfo2 == null)
					{
						typeInfo2 = new InstrumentedAssembly.TypeInfo(this.source, this.naming, (Type)this.mapTypeToConverter[t]);
						this.mapTypeToTypeInfo.Add(t, typeInfo2);
					}
					this.lastTypeInfo = typeInfo2;
					typeInfo = typeInfo2;
				}
			}
			return typeInfo;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000233BC File Offset: 0x000223BC
		public void Fire(Type t, object o)
		{
			InstrumentedAssembly.TypeInfo typeInfo = this.GetTypeInfo(t);
			typeInfo.Fire(o);
		}

		// Token: 0x04000289 RID: 649
		private SchemaNaming naming;

		// Token: 0x0400028A RID: 650
		public EventSource source;

		// Token: 0x0400028B RID: 651
		public Hashtable mapTypeToConverter;

		// Token: 0x0400028C RID: 652
		public static ReaderWriterLock readerWriterLock = new ReaderWriterLock();

		// Token: 0x0400028D RID: 653
		public static Hashtable mapIDToPublishedObject = new Hashtable();

		// Token: 0x0400028E RID: 654
		private static Hashtable mapPublishedObjectToID = new Hashtable();

		// Token: 0x0400028F RID: 655
		private static int upcountId = 3839;

		// Token: 0x04000290 RID: 656
		private Hashtable mapTypeToTypeInfo = new Hashtable();

		// Token: 0x04000291 RID: 657
		private InstrumentedAssembly.TypeInfo lastTypeInfo;

		// Token: 0x04000292 RID: 658
		private Type lastType;

		// Token: 0x020000A5 RID: 165
		private class TypeInfo
		{
			// Token: 0x060004DB RID: 1243 RVA: 0x00023404 File Offset: 0x00022404
			public void Fire(object o)
			{
				if (this.source.Any())
				{
					return;
				}
				if (!this.batchEvents)
				{
					lock (this)
					{
						this.convertFunctionNoBatch(o);
						this.wbemObjects[0] = (IntPtr)this.fieldInfo.GetValue(this.convertFunctionNoBatch.Target);
						this.source.IndicateEvents(1, this.wbemObjects);
						return;
					}
				}
				lock (this)
				{
					this.convertFunctionsBatch[this.currentIndex++](o);
					this.wbemObjects[this.currentIndex - 1] = (IntPtr)this.fieldInfo.GetValue(this.convertFunctionsBatch[this.currentIndex - 1].Target);
					if (this.cleanupThread == null)
					{
						int tickCount = Environment.TickCount;
						if (tickCount - this.lastFire < 1000)
						{
							this.lastFire = Environment.TickCount;
							this.cleanupThread = new Thread(new ThreadStart(this.Cleanup));
							this.cleanupThread.SetApartmentState(ApartmentState.MTA);
							this.cleanupThread.Start();
						}
						else
						{
							this.source.IndicateEvents(this.currentIndex, this.wbemObjects);
							this.currentIndex = 0;
							this.lastFire = tickCount;
						}
					}
					else if (this.currentIndex == this.batchSize)
					{
						this.source.IndicateEvents(this.currentIndex, this.wbemObjects);
						this.currentIndex = 0;
						this.lastFire = Environment.TickCount;
					}
				}
			}

			// Token: 0x060004DC RID: 1244 RVA: 0x000235DC File Offset: 0x000225DC
			public void SetBatchSize(int batchSize)
			{
				if (batchSize <= 0)
				{
					throw new ArgumentOutOfRangeException("batchSize");
				}
				if (!WMICapabilities.MultiIndicateSupported)
				{
					batchSize = 1;
				}
				lock (this)
				{
					if (this.currentIndex > 0)
					{
						this.source.IndicateEvents(this.currentIndex, this.wbemObjects);
						this.currentIndex = 0;
						this.lastFire = Environment.TickCount;
					}
					this.wbemObjects = new IntPtr[batchSize];
					if (batchSize > 1)
					{
						this.batchEvents = true;
						this.batchSize = batchSize;
						this.convertFunctionsBatch = new ConvertToWMI[batchSize];
						for (int i = 0; i < batchSize; i++)
						{
							object obj = Activator.CreateInstance(this.converterType);
							this.convertFunctionsBatch[i] = (ConvertToWMI)Delegate.CreateDelegate(typeof(ConvertToWMI), obj, "ToWMI");
							this.wbemObjects[i] = this.ExtractIntPtr(obj);
						}
						this.fieldInfo = this.convertFunctionsBatch[0].Target.GetType().GetField("instWbemObjectAccessIP");
					}
					else
					{
						this.fieldInfo = this.convertFunctionNoBatch.Target.GetType().GetField("instWbemObjectAccessIP");
						this.wbemObjects[0] = this.ExtractIntPtr(this.convertFunctionNoBatch.Target);
						this.batchEvents = false;
					}
				}
			}

			// Token: 0x060004DD RID: 1245 RVA: 0x00023750 File Offset: 0x00022750
			public IntPtr ExtractIntPtr(object o)
			{
				return (IntPtr)o.GetType().GetField("instWbemObjectAccessIP").GetValue(o);
			}

			// Token: 0x060004DE RID: 1246 RVA: 0x00023770 File Offset: 0x00022770
			public void Cleanup()
			{
				int i = 0;
				while (i < 20)
				{
					Thread.Sleep(100);
					if (this.currentIndex == 0)
					{
						i++;
					}
					else
					{
						i = 0;
						if (Environment.TickCount - this.lastFire >= 100)
						{
							lock (this)
							{
								if (this.currentIndex > 0)
								{
									this.source.IndicateEvents(this.currentIndex, this.wbemObjects);
									this.currentIndex = 0;
									this.lastFire = Environment.TickCount;
								}
							}
						}
					}
				}
				this.cleanupThread = null;
			}

			// Token: 0x060004DF RID: 1247 RVA: 0x00023808 File Offset: 0x00022808
			public TypeInfo(EventSource source, SchemaNaming naming, Type converterType)
			{
				this.converterType = converterType;
				this.source = source;
				object obj = Activator.CreateInstance(converterType);
				this.convertFunctionNoBatch = (ConvertToWMI)Delegate.CreateDelegate(typeof(ConvertToWMI), obj, "ToWMI");
				this.SetBatchSize(this.batchSize);
			}

			// Token: 0x04000293 RID: 659
			private FieldInfo fieldInfo;

			// Token: 0x04000294 RID: 660
			private int batchSize = 64;

			// Token: 0x04000295 RID: 661
			private bool batchEvents = true;

			// Token: 0x04000296 RID: 662
			private ConvertToWMI[] convertFunctionsBatch;

			// Token: 0x04000297 RID: 663
			private ConvertToWMI convertFunctionNoBatch;

			// Token: 0x04000298 RID: 664
			private IntPtr[] wbemObjects;

			// Token: 0x04000299 RID: 665
			private Type converterType;

			// Token: 0x0400029A RID: 666
			private int currentIndex;

			// Token: 0x0400029B RID: 667
			public EventSource source;

			// Token: 0x0400029C RID: 668
			public int lastFire;

			// Token: 0x0400029D RID: 669
			public Thread cleanupThread;
		}
	}
}
