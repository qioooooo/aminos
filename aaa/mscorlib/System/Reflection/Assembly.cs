using System;
using System.Collections;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Reflection.Cache;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Util;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System.Reflection
{
	// Token: 0x020002C8 RID: 712
	[ComDefaultInterface(typeof(_Assembly))]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	public class Assembly : _Assembly, IEvidenceFactory, ICustomAttributeProvider, ISerializable
	{
		// Token: 0x06001BEC RID: 7148 RVA: 0x00048498 File Offset: 0x00047498
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			if (!(o is Assembly))
			{
				return false;
			}
			Assembly assembly = o as Assembly;
			assembly = assembly.InternalAssembly;
			return this.InternalAssembly == assembly;
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x000484CB File Offset: 0x000474CB
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001BEE RID: 7150 RVA: 0x000484D3 File Offset: 0x000474D3
		internal virtual Assembly InternalAssembly
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001BEF RID: 7151 RVA: 0x000484D6 File Offset: 0x000474D6
		// (set) Token: 0x06001BF0 RID: 7152 RVA: 0x000484E3 File Offset: 0x000474E3
		internal AssemblyBuilderData m_assemblyData
		{
			get
			{
				return this.InternalAssembly.m__assemblyData;
			}
			set
			{
				this.InternalAssembly.m__assemblyData = value;
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06001BF1 RID: 7153 RVA: 0x000484F1 File Offset: 0x000474F1
		// (remove) Token: 0x06001BF2 RID: 7154 RVA: 0x0004850A File Offset: 0x0004750A
		[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
		private event ModuleResolveEventHandler _ModuleResolve;

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x00048523 File Offset: 0x00047523
		private ModuleResolveEventHandler ModuleResolveEvent
		{
			get
			{
				return this.InternalAssembly._ModuleResolve;
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06001BF4 RID: 7156 RVA: 0x00048530 File Offset: 0x00047530
		// (remove) Token: 0x06001BF5 RID: 7157 RVA: 0x0004854E File Offset: 0x0004754E
		public event ModuleResolveEventHandler ModuleResolve
		{
			[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
			add
			{
				Assembly internalAssembly = this.InternalAssembly;
				internalAssembly._ModuleResolve = (ModuleResolveEventHandler)Delegate.Combine(internalAssembly._ModuleResolve, value);
			}
			[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
			remove
			{
				Assembly internalAssembly = this.InternalAssembly;
				internalAssembly._ModuleResolve = (ModuleResolveEventHandler)Delegate.Remove(internalAssembly._ModuleResolve, value);
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001BF6 RID: 7158 RVA: 0x0004856C File Offset: 0x0004756C
		// (set) Token: 0x06001BF7 RID: 7159 RVA: 0x00048579 File Offset: 0x00047579
		internal InternalCache m_cachedData
		{
			get
			{
				return this.InternalAssembly.m__cachedData;
			}
			set
			{
				this.InternalAssembly.m__cachedData = value;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001BF8 RID: 7160 RVA: 0x00048587 File Offset: 0x00047587
		// (set) Token: 0x06001BF9 RID: 7161 RVA: 0x00048594 File Offset: 0x00047594
		internal IntPtr m_assembly
		{
			get
			{
				return this.InternalAssembly.m__assembly;
			}
			set
			{
				this.InternalAssembly.m__assembly = value;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001BFA RID: 7162 RVA: 0x000485A4 File Offset: 0x000475A4
		public virtual string CodeBase
		{
			get
			{
				string text = this.nGetCodeBase(false);
				this.VerifyCodeBaseDiscovery(text);
				return text;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x000485C1 File Offset: 0x000475C1
		public virtual string EscapedCodeBase
		{
			get
			{
				return AssemblyName.EscapeCodeBase(this.CodeBase);
			}
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000485CE File Offset: 0x000475CE
		public virtual AssemblyName GetName()
		{
			return this.GetName(false);
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001BFD RID: 7165 RVA: 0x000485D7 File Offset: 0x000475D7
		internal unsafe AssemblyHandle AssemblyHandle
		{
			get
			{
				return new AssemblyHandle((void*)this.m_assembly);
			}
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000485EC File Offset: 0x000475EC
		public virtual AssemblyName GetName(bool copiedName)
		{
			AssemblyName assemblyName = new AssemblyName();
			string text = this.nGetCodeBase(copiedName);
			this.VerifyCodeBaseDiscovery(text);
			assemblyName.Init(this.nGetSimpleName(), this.nGetPublicKey(), null, this.GetVersion(), this.GetLocale(), this.nGetHashAlgorithm(), AssemblyVersionCompatibility.SameMachine, text, this.nGetFlags() | AssemblyNameFlags.PublicKey, null);
			assemblyName.ProcessorArchitecture = this.ComputeProcArchIndex();
			return assemblyName;
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001BFF RID: 7167 RVA: 0x0004864C File Offset: 0x0004764C
		public virtual string FullName
		{
			get
			{
				string text;
				if ((text = (string)this.Cache[CacheObjType.AssemblyName]) != null)
				{
					return text;
				}
				text = this.GetFullName();
				if (text != null)
				{
					this.Cache[CacheObjType.AssemblyName] = text;
				}
				return text;
			}
		}

		// Token: 0x06001C00 RID: 7168
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string CreateQualifiedName(string assemblyName, string typeName);

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001C01 RID: 7169 RVA: 0x00048688 File Offset: 0x00047688
		public virtual MethodInfo EntryPoint
		{
			get
			{
				RuntimeMethodHandle runtimeMethodHandle = this.nGetEntryPoint();
				if (!runtimeMethodHandle.IsNullHandle())
				{
					return (MethodInfo)RuntimeType.GetMethodBase(runtimeMethodHandle);
				}
				return null;
			}
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x000486B4 File Offset: 0x000476B4
		public static Assembly GetAssembly(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Module module = type.Module;
			if (module == null)
			{
				return null;
			}
			return module.Assembly;
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x000486E1 File Offset: 0x000476E1
		Type _Assembly.GetType()
		{
			return base.GetType();
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x000486E9 File Offset: 0x000476E9
		public virtual Type GetType(string name)
		{
			return this.GetType(name, false, false);
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000486F4 File Offset: 0x000476F4
		public virtual Type GetType(string name, bool throwOnError)
		{
			return this.GetType(name, throwOnError, false);
		}

		// Token: 0x06001C06 RID: 7174
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type _GetType(string name, bool throwOnError, bool ignoreCase);

		// Token: 0x06001C07 RID: 7175 RVA: 0x000486FF File Offset: 0x000476FF
		public Type GetType(string name, bool throwOnError, bool ignoreCase)
		{
			return this.InternalAssembly._GetType(name, throwOnError, ignoreCase);
		}

		// Token: 0x06001C08 RID: 7176
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type[] _GetExportedTypes();

		// Token: 0x06001C09 RID: 7177 RVA: 0x0004870F File Offset: 0x0004770F
		public virtual Type[] GetExportedTypes()
		{
			return this.InternalAssembly._GetExportedTypes();
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x0004871C File Offset: 0x0004771C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual Type[] GetTypes()
		{
			Module[] array = this.nGetModules(true, false);
			int num = array.Length;
			int num2 = 0;
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			Type[][] array2 = new Type[num][];
			for (int i = 0; i < num; i++)
			{
				array2[i] = array[i].GetTypesInternal(ref stackCrawlMark);
				num2 += array2[i].Length;
			}
			int num3 = 0;
			Type[] array3 = new Type[num2];
			for (int j = 0; j < num; j++)
			{
				int num4 = array2[j].Length;
				Array.Copy(array2[j], 0, array3, num3, num4);
				num3 += num4;
			}
			return array3;
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x000487AC File Offset: 0x000477AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual Stream GetManifestResourceStream(Type type, string name)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.GetManifestResourceStream(type, name, false, ref stackCrawlMark);
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x000487C8 File Offset: 0x000477C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual Stream GetManifestResourceStream(string name)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.GetManifestResourceStream(name, ref stackCrawlMark, false);
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x000487E1 File Offset: 0x000477E1
		public Assembly GetSatelliteAssembly(CultureInfo culture)
		{
			return this.InternalGetSatelliteAssembly(culture, null, true);
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x000487EC File Offset: 0x000477EC
		public Assembly GetSatelliteAssembly(CultureInfo culture, Version version)
		{
			return this.InternalGetSatelliteAssembly(culture, version, true);
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001C0F RID: 7183 RVA: 0x000487F7 File Offset: 0x000477F7
		public virtual Evidence Evidence
		{
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
			get
			{
				return this.nGetEvidence().Copy();
			}
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x00048804 File Offset: 0x00047804
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, 6, this.FullName, this);
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x00048824 File Offset: 0x00047824
		internal bool AptcaCheck(Assembly sourceAssembly)
		{
			return this.AssemblyHandle.AptcaCheck(sourceAssembly.AssemblyHandle);
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001C12 RID: 7186 RVA: 0x00048848 File Offset: 0x00047848
		[ComVisible(false)]
		public Module ManifestModule
		{
			get
			{
				ModuleHandle manifestModule = this.AssemblyHandle.GetManifestModule();
				if (manifestModule.IsNullHandle())
				{
					return null;
				}
				return manifestModule.GetModule();
			}
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x00048876 File Offset: 0x00047876
		public virtual object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x00048890 File Offset: 0x00047890
		public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this, runtimeType);
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x000488D8 File Offset: 0x000478D8
		public virtual bool IsDefined(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "caType");
			}
			return CustomAttribute.IsDefined(this, runtimeType);
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x00048920 File Offset: 0x00047920
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly LoadFrom(string assemblyFile)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoadFrom(assemblyFile, null, null, AssemblyHashAlgorithm.None, false, ref stackCrawlMark);
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x0004893C File Offset: 0x0004793C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly ReflectionOnlyLoadFrom(string assemblyFile)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoadFrom(assemblyFile, null, null, AssemblyHashAlgorithm.None, true, ref stackCrawlMark);
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x00048958 File Offset: 0x00047958
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly LoadFrom(string assemblyFile, Evidence securityEvidence)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoadFrom(assemblyFile, securityEvidence, null, AssemblyHashAlgorithm.None, false, ref stackCrawlMark);
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x00048974 File Offset: 0x00047974
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly LoadFrom(string assemblyFile, Evidence securityEvidence, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoadFrom(assemblyFile, securityEvidence, hashValue, hashAlgorithm, false, ref stackCrawlMark);
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x00048990 File Offset: 0x00047990
		private static Assembly InternalLoadFrom(string assemblyFile, Evidence securityEvidence, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm, bool forIntrospection, ref StackCrawlMark stackMark)
		{
			if (assemblyFile == null)
			{
				throw new ArgumentNullException("assemblyFile");
			}
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.CodeBase = assemblyFile;
			assemblyName.SetHashControl(hashValue, hashAlgorithm);
			return Assembly.InternalLoad(assemblyName, securityEvidence, ref stackMark, forIntrospection);
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x000489CC File Offset: 0x000479CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly Load(string assemblyString)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoad(assemblyString, null, ref stackCrawlMark, false);
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x000489E8 File Offset: 0x000479E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly ReflectionOnlyLoad(string assemblyString)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoad(assemblyString, null, ref stackCrawlMark, true);
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x00048A04 File Offset: 0x00047A04
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly Load(string assemblyString, Evidence assemblySecurity)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoad(assemblyString, assemblySecurity, ref stackCrawlMark, false);
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x00048A20 File Offset: 0x00047A20
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly Load(AssemblyName assemblyRef)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoad(assemblyRef, null, ref stackCrawlMark, false);
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x00048A3C File Offset: 0x00047A3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly Load(AssemblyName assemblyRef, Evidence assemblySecurity)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.InternalLoad(assemblyRef, assemblySecurity, ref stackCrawlMark, false);
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x00048A58 File Offset: 0x00047A58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static IntPtr LoadWithPartialNameHack(string partialName, bool cropPublicKey)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			Assembly assembly = null;
			AssemblyName assemblyName = new AssemblyName(partialName);
			if (!Assembly.IsSimplyNamed(assemblyName))
			{
				if (cropPublicKey)
				{
					assemblyName.SetPublicKey(null);
					assemblyName.SetPublicKeyToken(null);
				}
				AssemblyName assemblyName2 = Assembly.EnumerateCache(assemblyName);
				if (assemblyName2 != null)
				{
					assembly = Assembly.InternalLoad(assemblyName2, null, ref stackCrawlMark, false);
				}
			}
			if (assembly == null)
			{
				return (IntPtr)0;
			}
			return (IntPtr)assembly.AssemblyHandle.Value;
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x00048ABC File Offset: 0x00047ABC
		[Obsolete("This method has been deprecated. Please use Assembly.Load() instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly LoadWithPartialName(string partialName)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.LoadWithPartialNameInternal(partialName, null, ref stackCrawlMark);
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x00048AD4 File Offset: 0x00047AD4
		[Obsolete("This method has been deprecated. Please use Assembly.Load() instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly LoadWithPartialName(string partialName, Evidence securityEvidence)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.LoadWithPartialNameInternal(partialName, securityEvidence, ref stackCrawlMark);
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x00048AEC File Offset: 0x00047AEC
		internal static Assembly LoadWithPartialNameInternal(string partialName, Evidence securityEvidence, ref StackCrawlMark stackMark)
		{
			if (securityEvidence != null)
			{
				new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
			}
			Assembly assembly = null;
			AssemblyName assemblyName = new AssemblyName(partialName);
			try
			{
				assembly = Assembly.nLoad(assemblyName, null, securityEvidence, null, ref stackMark, true, false);
			}
			catch (Exception ex)
			{
				if (ex.IsTransient)
				{
					throw ex;
				}
				if (Assembly.IsSimplyNamed(assemblyName))
				{
					return null;
				}
				AssemblyName assemblyName2 = Assembly.EnumerateCache(assemblyName);
				if (assemblyName2 != null)
				{
					return Assembly.InternalLoad(assemblyName2, securityEvidence, ref stackMark, false);
				}
			}
			return assembly;
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001C24 RID: 7204 RVA: 0x00048B68 File Offset: 0x00047B68
		[ComVisible(false)]
		public virtual bool ReflectionOnly
		{
			get
			{
				return this.nReflection();
			}
		}

		// Token: 0x06001C25 RID: 7205
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool _nReflection();

		// Token: 0x06001C26 RID: 7206 RVA: 0x00048B70 File Offset: 0x00047B70
		internal bool nReflection()
		{
			return this.InternalAssembly._nReflection();
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x00048B80 File Offset: 0x00047B80
		private static AssemblyName EnumerateCache(AssemblyName partialName)
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			partialName.Version = null;
			ArrayList arrayList = new ArrayList();
			Fusion.ReadCache(arrayList, partialName.FullName, 2U);
			IEnumerator enumerator = arrayList.GetEnumerator();
			AssemblyName assemblyName = null;
			CultureInfo cultureInfo = partialName.CultureInfo;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				AssemblyName assemblyName2 = new AssemblyName((string)obj);
				if (Assembly.CulturesEqual(cultureInfo, assemblyName2.CultureInfo))
				{
					if (assemblyName == null)
					{
						assemblyName = assemblyName2;
					}
					else if (assemblyName2.Version > assemblyName.Version)
					{
						assemblyName = assemblyName2;
					}
				}
			}
			return assemblyName;
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x00048C10 File Offset: 0x00047C10
		private static bool CulturesEqual(CultureInfo refCI, CultureInfo defCI)
		{
			bool flag = defCI.Equals(CultureInfo.InvariantCulture);
			if (refCI == null || refCI.Equals(CultureInfo.InvariantCulture))
			{
				return flag;
			}
			return !flag && defCI.Equals(refCI);
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x00048C4C File Offset: 0x00047C4C
		private static bool IsSimplyNamed(AssemblyName partialName)
		{
			byte[] array = partialName.GetPublicKeyToken();
			if (array != null && array.Length == 0)
			{
				return true;
			}
			array = partialName.GetPublicKey();
			return array != null && array.Length == 0;
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x00048C7C File Offset: 0x00047C7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly Load(byte[] rawAssembly)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.nLoadImage(rawAssembly, null, null, ref stackCrawlMark, false);
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x00048C98 File Offset: 0x00047C98
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly ReflectionOnlyLoad(byte[] rawAssembly)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.nLoadImage(rawAssembly, null, null, ref stackCrawlMark, true);
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x00048CB4 File Offset: 0x00047CB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.nLoadImage(rawAssembly, rawSymbolStore, null, ref stackCrawlMark, false);
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x00048CD0 File Offset: 0x00047CD0
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.nLoadImage(rawAssembly, rawSymbolStore, securityEvidence, ref stackCrawlMark, false);
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x00048CEA File Offset: 0x00047CEA
		public static Assembly LoadFile(string path)
		{
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, path).Demand();
			return Assembly.nLoadFile(path, null);
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x00048D00 File Offset: 0x00047D00
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlEvidence)]
		public static Assembly LoadFile(string path, Evidence securityEvidence)
		{
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, path).Demand();
			return Assembly.nLoadFile(path, securityEvidence);
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x00048D16 File Offset: 0x00047D16
		public Module LoadModule(string moduleName, byte[] rawModule)
		{
			return this.nLoadModule(moduleName, rawModule, null, this.Evidence);
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x00048D27 File Offset: 0x00047D27
		public Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore)
		{
			return this.nLoadModule(moduleName, rawModule, rawSymbolStore, this.Evidence);
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x00048D38 File Offset: 0x00047D38
		public object CreateInstance(string typeName)
		{
			return this.CreateInstance(typeName, false, BindingFlags.Instance | BindingFlags.Public, null, null, null, null);
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x00048D48 File Offset: 0x00047D48
		public object CreateInstance(string typeName, bool ignoreCase)
		{
			return this.CreateInstance(typeName, ignoreCase, BindingFlags.Instance | BindingFlags.Public, null, null, null, null);
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x00048D58 File Offset: 0x00047D58
		public object CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			Type type = this.GetType(typeName, false, ignoreCase);
			if (type == null)
			{
				return null;
			}
			return Activator.CreateInstance(type, bindingAttr, binder, args, culture, activationAttributes);
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x00048D83 File Offset: 0x00047D83
		public Module[] GetLoadedModules()
		{
			return this.nGetModules(false, false);
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x00048D8D File Offset: 0x00047D8D
		public Module[] GetLoadedModules(bool getResourceModules)
		{
			return this.nGetModules(false, getResourceModules);
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x00048D97 File Offset: 0x00047D97
		public Module[] GetModules()
		{
			return this.nGetModules(true, false);
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x00048DA1 File Offset: 0x00047DA1
		public Module[] GetModules(bool getResourceModules)
		{
			return this.nGetModules(true, getResourceModules);
		}

		// Token: 0x06001C39 RID: 7225
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Module _GetModule(string name);

		// Token: 0x06001C3A RID: 7226 RVA: 0x00048DAB File Offset: 0x00047DAB
		public Module GetModule(string name)
		{
			return this.GetModuleInternal(name);
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x00048DB4 File Offset: 0x00047DB4
		internal virtual Module GetModuleInternal(string name)
		{
			return this.InternalAssembly._GetModule(name);
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x00048DC4 File Offset: 0x00047DC4
		public virtual FileStream GetFile(string name)
		{
			Module module = this.GetModule(name);
			if (module == null)
			{
				return null;
			}
			return new FileStream(module.InternalGetFullyQualifiedName(), FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x00048DEC File Offset: 0x00047DEC
		public virtual FileStream[] GetFiles()
		{
			return this.GetFiles(false);
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x00048DF8 File Offset: 0x00047DF8
		public virtual FileStream[] GetFiles(bool getResourceModules)
		{
			Module[] array = this.nGetModules(true, getResourceModules);
			int num = array.Length;
			FileStream[] array2 = new FileStream[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = new FileStream(array[i].InternalGetFullyQualifiedName(), FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			return array2;
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x00048E39 File Offset: 0x00047E39
		public virtual string[] GetManifestResourceNames()
		{
			return this.nGetManifestResourceNames();
		}

		// Token: 0x06001C40 RID: 7232
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string[] _nGetManifestResourceNames();

		// Token: 0x06001C41 RID: 7233 RVA: 0x00048E41 File Offset: 0x00047E41
		internal string[] nGetManifestResourceNames()
		{
			return this.InternalAssembly._nGetManifestResourceNames();
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x00048E50 File Offset: 0x00047E50
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly GetExecutingAssembly()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Assembly.nGetExecutingAssembly(ref stackCrawlMark);
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x00048E68 File Offset: 0x00047E68
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Assembly GetCallingAssembly()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCallersCaller;
			return Assembly.nGetExecutingAssembly(ref stackCrawlMark);
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x00048E80 File Offset: 0x00047E80
		public static Assembly GetEntryAssembly()
		{
			AppDomainManager appDomainManager = AppDomain.CurrentDomain.DomainManager;
			if (appDomainManager == null)
			{
				appDomainManager = new AppDomainManager();
			}
			return appDomainManager.EntryAssembly;
		}

		// Token: 0x06001C45 RID: 7237
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AssemblyName[] _GetReferencedAssemblies();

		// Token: 0x06001C46 RID: 7238 RVA: 0x00048EA7 File Offset: 0x00047EA7
		public AssemblyName[] GetReferencedAssemblies()
		{
			return this.InternalAssembly._GetReferencedAssemblies();
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x00048EB4 File Offset: 0x00047EB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual ManifestResourceInfo GetManifestResourceInfo(string resourceName)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			Assembly assembly;
			string text;
			int num = this.nGetManifestResourceInfo(resourceName, out assembly, out text, ref stackCrawlMark);
			if (num == -1)
			{
				return null;
			}
			return new ManifestResourceInfo(assembly, text, (ResourceLocation)num);
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x00048EE0 File Offset: 0x00047EE0
		public override string ToString()
		{
			string fullName = this.FullName;
			if (fullName == null)
			{
				return base.ToString();
			}
			return fullName;
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001C49 RID: 7241 RVA: 0x00048F00 File Offset: 0x00047F00
		public virtual string Location
		{
			get
			{
				string location = this.GetLocation();
				if (location != null)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, location).Demand();
				}
				return location;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x00048F24 File Offset: 0x00047F24
		[ComVisible(false)]
		public virtual string ImageRuntimeVersion
		{
			get
			{
				return this.nGetImageRuntimeVersion();
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001C4B RID: 7243 RVA: 0x00048F2C File Offset: 0x00047F2C
		public bool GlobalAssemblyCache
		{
			get
			{
				return this.nGlobalAssemblyCache();
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001C4C RID: 7244 RVA: 0x00048F34 File Offset: 0x00047F34
		[ComVisible(false)]
		public long HostContext
		{
			get
			{
				return this.GetHostContext();
			}
		}

		// Token: 0x06001C4D RID: 7245
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern long _GetHostContext();

		// Token: 0x06001C4E RID: 7246 RVA: 0x00048F3C File Offset: 0x00047F3C
		private long GetHostContext()
		{
			return this.InternalAssembly._GetHostContext();
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x00048F4C File Offset: 0x00047F4C
		internal static string VerifyCodeBase(string codebase)
		{
			if (codebase == null)
			{
				return null;
			}
			int length = codebase.Length;
			if (length == 0)
			{
				return null;
			}
			int num = codebase.IndexOf(':');
			if (num != -1 && num + 2 < length && (codebase[num + 1] == '/' || codebase[num + 1] == '\\') && (codebase[num + 2] == '/' || codebase[num + 2] == '\\'))
			{
				return codebase;
			}
			if (length > 2 && codebase[0] == '\\' && codebase[1] == '\\')
			{
				return "file://" + codebase;
			}
			return "file:///" + Path.GetFullPathInternal(codebase);
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x00048FEC File Offset: 0x00047FEC
		internal virtual Stream GetManifestResourceStream(Type type, string name, bool skipSecurityCheck, ref StackCrawlMark stackMark)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (type == null)
			{
				if (name == null)
				{
					throw new ArgumentNullException("type");
				}
			}
			else
			{
				string @namespace = type.Namespace;
				if (@namespace != null)
				{
					stringBuilder.Append(@namespace);
					if (name != null)
					{
						stringBuilder.Append(Type.Delimiter);
					}
				}
			}
			if (name != null)
			{
				stringBuilder.Append(name);
			}
			return this.GetManifestResourceStream(stringBuilder.ToString(), ref stackMark, skipSecurityCheck);
		}

		// Token: 0x06001C51 RID: 7249
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Module _nLoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore, Evidence securityEvidence);

		// Token: 0x06001C52 RID: 7250 RVA: 0x0004904B File Offset: 0x0004804B
		private Module nLoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore, Evidence securityEvidence)
		{
			return this.InternalAssembly._nLoadModule(moduleName, rawModule, rawSymbolStore, securityEvidence);
		}

		// Token: 0x06001C53 RID: 7251
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool _nGlobalAssemblyCache();

		// Token: 0x06001C54 RID: 7252 RVA: 0x0004905D File Offset: 0x0004805D
		private bool nGlobalAssemblyCache()
		{
			return this.InternalAssembly._nGlobalAssemblyCache();
		}

		// Token: 0x06001C55 RID: 7253
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string _nGetImageRuntimeVersion();

		// Token: 0x06001C56 RID: 7254 RVA: 0x0004906A File Offset: 0x0004806A
		private string nGetImageRuntimeVersion()
		{
			return this.InternalAssembly._nGetImageRuntimeVersion();
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00049077 File Offset: 0x00048077
		internal Assembly()
		{
		}

		// Token: 0x06001C58 RID: 7256
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Module _nDefineDynamicModule(Assembly containingAssembly, bool emitSymbolInfo, string filename, ref StackCrawlMark stackMark);

		// Token: 0x06001C59 RID: 7257 RVA: 0x0004907F File Offset: 0x0004807F
		internal static Module nDefineDynamicModule(Assembly containingAssembly, bool emitSymbolInfo, string filename, ref StackCrawlMark stackMark)
		{
			return Assembly._nDefineDynamicModule(containingAssembly.InternalAssembly, emitSymbolInfo, filename, ref stackMark);
		}

		// Token: 0x06001C5A RID: 7258
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _nPrepareForSavingManifestToDisk(Module assemblyModule);

		// Token: 0x06001C5B RID: 7259 RVA: 0x0004908F File Offset: 0x0004808F
		internal void nPrepareForSavingManifestToDisk(Module assemblyModule)
		{
			if (assemblyModule != null)
			{
				assemblyModule = assemblyModule.InternalModule;
			}
			this.InternalAssembly._nPrepareForSavingManifestToDisk(assemblyModule);
		}

		// Token: 0x06001C5C RID: 7260
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _nSaveToFileList(string strFileName);

		// Token: 0x06001C5D RID: 7261 RVA: 0x000490A8 File Offset: 0x000480A8
		internal int nSaveToFileList(string strFileName)
		{
			return this.InternalAssembly._nSaveToFileList(strFileName);
		}

		// Token: 0x06001C5E RID: 7262
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _nSetHashValue(int tkFile, string strFullFileName);

		// Token: 0x06001C5F RID: 7263 RVA: 0x000490B6 File Offset: 0x000480B6
		internal int nSetHashValue(int tkFile, string strFullFileName)
		{
			return this.InternalAssembly._nSetHashValue(tkFile, strFullFileName);
		}

		// Token: 0x06001C60 RID: 7264
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _nSaveExportedType(string strComTypeName, int tkAssemblyRef, int tkTypeDef, TypeAttributes flags);

		// Token: 0x06001C61 RID: 7265 RVA: 0x000490C5 File Offset: 0x000480C5
		internal int nSaveExportedType(string strComTypeName, int tkAssemblyRef, int tkTypeDef, TypeAttributes flags)
		{
			return this.InternalAssembly._nSaveExportedType(strComTypeName, tkAssemblyRef, tkTypeDef, flags);
		}

		// Token: 0x06001C62 RID: 7266
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _nSavePermissionRequests(byte[] required, byte[] optional, byte[] refused);

		// Token: 0x06001C63 RID: 7267 RVA: 0x000490D7 File Offset: 0x000480D7
		internal void nSavePermissionRequests(byte[] required, byte[] optional, byte[] refused)
		{
			this.InternalAssembly._nSavePermissionRequests(required, optional, refused);
		}

		// Token: 0x06001C64 RID: 7268
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _nSaveManifestToDisk(string strFileName, int entryPoint, int fileKind, int portableExecutableKind, int ImageFileMachine);

		// Token: 0x06001C65 RID: 7269 RVA: 0x000490E7 File Offset: 0x000480E7
		internal void nSaveManifestToDisk(string strFileName, int entryPoint, int fileKind, int portableExecutableKind, int ImageFileMachine)
		{
			this.InternalAssembly._nSaveManifestToDisk(strFileName, entryPoint, fileKind, portableExecutableKind, ImageFileMachine);
		}

		// Token: 0x06001C66 RID: 7270
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _nAddFileToInMemoryFileList(string strFileName, Module module);

		// Token: 0x06001C67 RID: 7271 RVA: 0x000490FB File Offset: 0x000480FB
		internal int nAddFileToInMemoryFileList(string strFileName, Module module)
		{
			if (module != null)
			{
				module = module.InternalModule;
			}
			return this.InternalAssembly._nAddFileToInMemoryFileList(strFileName, module);
		}

		// Token: 0x06001C68 RID: 7272
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Module _nGetOnDiskAssemblyModule();

		// Token: 0x06001C69 RID: 7273 RVA: 0x00049115 File Offset: 0x00048115
		internal Module nGetOnDiskAssemblyModule()
		{
			return this.InternalAssembly._nGetOnDiskAssemblyModule();
		}

		// Token: 0x06001C6A RID: 7274
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Module _nGetInMemoryAssemblyModule();

		// Token: 0x06001C6B RID: 7275 RVA: 0x00049122 File Offset: 0x00048122
		internal Module nGetInMemoryAssemblyModule()
		{
			return this.InternalAssembly._nGetInMemoryAssemblyModule();
		}

		// Token: 0x06001C6C RID: 7276
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nDefineVersionInfoResource(string filename, string title, string iconFilename, string description, string copyright, string trademark, string company, string product, string productVersion, string fileVersion, int lcid, bool isDll);

		// Token: 0x06001C6D RID: 7277 RVA: 0x00049130 File Offset: 0x00048130
		private static void DecodeSerializedEvidence(Evidence evidence, byte[] serializedEvidence)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			Evidence evidence2 = null;
			PermissionSet permissionSet = new PermissionSet(false);
			permissionSet.SetPermission(new SecurityPermission(SecurityPermissionFlag.SerializationFormatter));
			permissionSet.PermitOnly();
			permissionSet.Assert();
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(serializedEvidence))
				{
					evidence2 = (Evidence)binaryFormatter.Deserialize(memoryStream);
				}
			}
			catch
			{
			}
			if (evidence2 != null)
			{
				IEnumerator assemblyEnumerator = evidence2.GetAssemblyEnumerator();
				while (assemblyEnumerator.MoveNext())
				{
					object obj = assemblyEnumerator.Current;
					evidence.AddAssembly(obj);
				}
			}
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000491D4 File Offset: 0x000481D4
		private static void AddX509Certificate(Evidence evidence, byte[] cert)
		{
			evidence.AddHost(new Publisher(new X509Certificate(cert)));
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000491E8 File Offset: 0x000481E8
		private static void AddStrongName(Evidence evidence, byte[] blob, string strSimpleName, int major, int minor, int build, int revision, Assembly assembly)
		{
			StrongName strongName = new StrongName(new StrongNamePublicKeyBlob(blob), strSimpleName, new Version(major, minor, build, revision), assembly);
			evidence.AddHost(strongName);
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x00049218 File Offset: 0x00048218
		private static Evidence CreateSecurityIdentity(Assembly asm, string strUrl, int zone, byte[] cert, byte[] publicKeyBlob, string strSimpleName, int major, int minor, int build, int revision, byte[] serializedEvidence, Evidence additionalEvidence)
		{
			Evidence evidence = new Evidence();
			if (zone != -1)
			{
				evidence.AddHost(new Zone((SecurityZone)zone));
			}
			if (strUrl != null)
			{
				evidence.AddHost(new Url(strUrl, true));
				if (string.Compare(strUrl, 0, "file:", 0, 5, StringComparison.OrdinalIgnoreCase) != 0)
				{
					evidence.AddHost(Site.CreateFromUrl(strUrl));
				}
			}
			if (cert != null)
			{
				Assembly.AddX509Certificate(evidence, cert);
			}
			if (asm != null && RuntimeEnvironment.FromGlobalAccessCache(asm))
			{
				evidence.AddHost(new GacInstalled());
			}
			if (serializedEvidence != null)
			{
				Assembly.DecodeSerializedEvidence(evidence, serializedEvidence);
			}
			if (publicKeyBlob != null && publicKeyBlob.Length != 0)
			{
				Assembly.AddStrongName(evidence, publicKeyBlob, strSimpleName, major, minor, build, revision, asm);
			}
			if (asm != null && !asm.nIsDynamic())
			{
				evidence.AddHost(new Hash(asm));
			}
			if (additionalEvidence != null)
			{
				evidence.MergeWithNoDuplicates(additionalEvidence);
			}
			if (asm != null)
			{
				HostSecurityManager hostSecurityManager = AppDomain.CurrentDomain.HostSecurityManager;
				if ((hostSecurityManager.Flags & HostSecurityManagerOptions.HostAssemblyEvidence) == HostSecurityManagerOptions.HostAssemblyEvidence)
				{
					return hostSecurityManager.ProvideAssemblyEvidence(asm, evidence);
				}
			}
			return evidence;
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000492F8 File Offset: 0x000482F8
		[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
		private bool IsAssemblyUnderAppBase()
		{
			string location = this.GetLocation();
			if (string.IsNullOrEmpty(location))
			{
				return true;
			}
			FileIOAccess fileIOAccess = new FileIOAccess(Path.GetFullPathInternal(location));
			FileIOAccess fileIOAccess2 = new FileIOAccess(Path.GetFullPathInternal(AppDomain.CurrentDomain.BaseDirectory));
			return fileIOAccess.IsSubsetOf(fileIOAccess2);
		}

		// Token: 0x06001C72 RID: 7282
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsStrongNameVerified();

		// Token: 0x06001C73 RID: 7283
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Assembly nGetExecutingAssembly(ref StackCrawlMark stackMark);

		// Token: 0x06001C74 RID: 7284 RVA: 0x00049340 File Offset: 0x00048340
		internal unsafe virtual Stream GetManifestResourceStream(string name, ref StackCrawlMark stackMark, bool skipSecurityCheck)
		{
			ulong num = 0UL;
			byte* resource = this.GetResource(name, out num, ref stackMark, skipSecurityCheck);
			if (resource == null)
			{
				return null;
			}
			if (num > 9223372036854775807UL)
			{
				throw new NotImplementedException(Environment.GetResourceString("NotImplemented_ResourcesLongerThan2^63"));
			}
			return new UnmanagedMemoryStream(resource, (long)num, (long)num, FileAccess.Read, true);
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x0004938C File Offset: 0x0004838C
		internal Version GetVersion()
		{
			int num;
			int num2;
			int num3;
			int num4;
			this.nGetVersion(out num, out num2, out num3, out num4);
			return new Version(num, num2, num3, num4);
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000493B0 File Offset: 0x000483B0
		internal CultureInfo GetLocale()
		{
			string text = this.nGetLocale();
			if (text == null)
			{
				return CultureInfo.InvariantCulture;
			}
			return new CultureInfo(text);
		}

		// Token: 0x06001C77 RID: 7287
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string _nGetLocale();

		// Token: 0x06001C78 RID: 7288 RVA: 0x000493D3 File Offset: 0x000483D3
		private string nGetLocale()
		{
			return this.InternalAssembly._nGetLocale();
		}

		// Token: 0x06001C79 RID: 7289
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _nGetVersion(out int majVer, out int minVer, out int buildNum, out int revNum);

		// Token: 0x06001C7A RID: 7290 RVA: 0x000493E0 File Offset: 0x000483E0
		internal void nGetVersion(out int majVer, out int minVer, out int buildNum, out int revNum)
		{
			this.InternalAssembly._nGetVersion(out majVer, out minVer, out buildNum, out revNum);
		}

		// Token: 0x06001C7B RID: 7291
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool _nIsDynamic();

		// Token: 0x06001C7C RID: 7292 RVA: 0x000493F2 File Offset: 0x000483F2
		internal bool nIsDynamic()
		{
			return this.InternalAssembly._nIsDynamic();
		}

		// Token: 0x06001C7D RID: 7293
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _nGetManifestResourceInfo(string resourceName, out Assembly assemblyRef, out string fileName, ref StackCrawlMark stackMark);

		// Token: 0x06001C7E RID: 7294 RVA: 0x000493FF File Offset: 0x000483FF
		private int nGetManifestResourceInfo(string resourceName, out Assembly assemblyRef, out string fileName, ref StackCrawlMark stackMark)
		{
			return this.InternalAssembly._nGetManifestResourceInfo(resourceName, out assemblyRef, out fileName, ref stackMark);
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x00049414 File Offset: 0x00048414
		private void VerifyCodeBaseDiscovery(string codeBase)
		{
			if (codeBase != null && string.Compare(codeBase, 0, "file:", 0, 5, StringComparison.OrdinalIgnoreCase) == 0)
			{
				URLString urlstring = new URLString(codeBase, true);
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, urlstring.GetFileName()).Demand();
			}
		}

		// Token: 0x06001C80 RID: 7296
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string _GetLocation();

		// Token: 0x06001C81 RID: 7297 RVA: 0x0004944E File Offset: 0x0004844E
		internal string GetLocation()
		{
			return this.InternalAssembly._GetLocation();
		}

		// Token: 0x06001C82 RID: 7298
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern byte[] _nGetPublicKey();

		// Token: 0x06001C83 RID: 7299 RVA: 0x0004945B File Offset: 0x0004845B
		internal byte[] nGetPublicKey()
		{
			return this.InternalAssembly._nGetPublicKey();
		}

		// Token: 0x06001C84 RID: 7300
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string _nGetSimpleName();

		// Token: 0x06001C85 RID: 7301 RVA: 0x00049468 File Offset: 0x00048468
		internal string nGetSimpleName()
		{
			return this.InternalAssembly._nGetSimpleName();
		}

		// Token: 0x06001C86 RID: 7302
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string _nGetCodeBase(bool fCopiedName);

		// Token: 0x06001C87 RID: 7303 RVA: 0x00049475 File Offset: 0x00048475
		internal string nGetCodeBase(bool fCopiedName)
		{
			return this.InternalAssembly._nGetCodeBase(fCopiedName);
		}

		// Token: 0x06001C88 RID: 7304
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AssemblyHashAlgorithm _nGetHashAlgorithm();

		// Token: 0x06001C89 RID: 7305 RVA: 0x00049483 File Offset: 0x00048483
		internal AssemblyHashAlgorithm nGetHashAlgorithm()
		{
			return this.InternalAssembly._nGetHashAlgorithm();
		}

		// Token: 0x06001C8A RID: 7306
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AssemblyNameFlags _nGetFlags();

		// Token: 0x06001C8B RID: 7307 RVA: 0x00049490 File Offset: 0x00048490
		internal AssemblyNameFlags nGetFlags()
		{
			return this.InternalAssembly._nGetFlags();
		}

		// Token: 0x06001C8C RID: 7308
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void _nGetGrantSet(out PermissionSet newGrant, out PermissionSet newDenied);

		// Token: 0x06001C8D RID: 7309 RVA: 0x0004949D File Offset: 0x0004849D
		internal void nGetGrantSet(out PermissionSet newGrant, out PermissionSet newDenied)
		{
			this.InternalAssembly._nGetGrantSet(out newGrant, out newDenied);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000494AC File Offset: 0x000484AC
		internal PermissionSet GetPermissionSet()
		{
			PermissionSet permissionSet;
			PermissionSet permissionSet2;
			this.nGetGrantSet(out permissionSet, out permissionSet2);
			if (permissionSet == null)
			{
				permissionSet = new PermissionSet(PermissionState.Unrestricted);
			}
			return permissionSet;
		}

		// Token: 0x06001C8F RID: 7311
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string _GetFullName();

		// Token: 0x06001C90 RID: 7312 RVA: 0x000494CE File Offset: 0x000484CE
		internal string GetFullName()
		{
			return this.InternalAssembly._GetFullName();
		}

		// Token: 0x06001C91 RID: 7313
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _nGetEntryPoint();

		// Token: 0x06001C92 RID: 7314 RVA: 0x000494DB File Offset: 0x000484DB
		private RuntimeMethodHandle nGetEntryPoint()
		{
			return new RuntimeMethodHandle(this._nGetEntryPoint());
		}

		// Token: 0x06001C93 RID: 7315
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Evidence _nGetEvidence();

		// Token: 0x06001C94 RID: 7316 RVA: 0x000494E8 File Offset: 0x000484E8
		internal Evidence nGetEvidence()
		{
			return this.InternalAssembly._nGetEvidence();
		}

		// Token: 0x06001C95 RID: 7317
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern byte* _GetResource(string resourceName, out ulong length, ref StackCrawlMark stackMark, bool skipSecurityCheck);

		// Token: 0x06001C96 RID: 7318 RVA: 0x000494F5 File Offset: 0x000484F5
		private unsafe byte* GetResource(string resourceName, out ulong length, ref StackCrawlMark stackMark, bool skipSecurityCheck)
		{
			return this.InternalAssembly._GetResource(resourceName, out length, ref stackMark, skipSecurityCheck);
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x00049508 File Offset: 0x00048508
		internal static Assembly InternalLoad(string assemblyString, Evidence assemblySecurity, ref StackCrawlMark stackMark, bool forIntrospection)
		{
			if (assemblyString == null)
			{
				throw new ArgumentNullException("assemblyString");
			}
			if (assemblyString.Length == 0 || assemblyString[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Format_StringZeroLength"));
			}
			AssemblyName assemblyName = new AssemblyName();
			Assembly assembly = null;
			assemblyName.Name = assemblyString;
			int num = assemblyName.nInit(out assembly, forIntrospection, true);
			if (num == -2146234297)
			{
				return assembly;
			}
			return Assembly.InternalLoad(assemblyName, assemblySecurity, ref stackMark, forIntrospection);
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x00049574 File Offset: 0x00048574
		internal static Assembly InternalLoad(AssemblyName assemblyRef, Evidence assemblySecurity, ref StackCrawlMark stackMark, bool forIntrospection)
		{
			if (assemblyRef == null)
			{
				throw new ArgumentNullException("assemblyRef");
			}
			assemblyRef = (AssemblyName)assemblyRef.Clone();
			if (assemblySecurity != null)
			{
				new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
			}
			string text = Assembly.VerifyCodeBase(assemblyRef.CodeBase);
			if (text != null)
			{
				if (string.Compare(text, 0, "file:", 0, 5, StringComparison.OrdinalIgnoreCase) != 0)
				{
					IPermission permission = Assembly.CreateWebPermission(assemblyRef.EscapedCodeBase);
					permission.Demand();
				}
				else
				{
					URLString urlstring = new URLString(text, true);
					new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, urlstring.GetFileName()).Demand();
				}
			}
			return Assembly.nLoad(assemblyRef, text, assemblySecurity, null, ref stackMark, true, forIntrospection);
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x00049608 File Offset: 0x00048608
		private static void DemandPermission(string codeBase, bool havePath, int demandFlag)
		{
			FileIOPermissionAccess fileIOPermissionAccess = FileIOPermissionAccess.PathDiscovery;
			switch (demandFlag)
			{
			case 1:
				fileIOPermissionAccess = FileIOPermissionAccess.Read;
				break;
			case 2:
				fileIOPermissionAccess = FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery;
				break;
			case 3:
			{
				IPermission permission = Assembly.CreateWebPermission(AssemblyName.EscapeCodeBase(codeBase));
				permission.Demand();
				return;
			}
			}
			if (!havePath)
			{
				URLString urlstring = new URLString(codeBase, true);
				codeBase = urlstring.GetFileName();
			}
			codeBase = Path.GetFullPathInternal(codeBase);
			new FileIOPermission(fileIOPermissionAccess, codeBase).Demand();
		}

		// Token: 0x06001C9A RID: 7322
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Assembly _nLoad(AssemblyName fileName, string codeBase, Evidence assemblySecurity, Assembly locationHint, ref StackCrawlMark stackMark, bool throwOnFileNotFound, bool forIntrospection);

		// Token: 0x06001C9B RID: 7323 RVA: 0x00049674 File Offset: 0x00048674
		private static Assembly nLoad(AssemblyName fileName, string codeBase, Evidence assemblySecurity, Assembly locationHint, ref StackCrawlMark stackMark, bool throwOnFileNotFound, bool forIntrospection)
		{
			if (locationHint != null)
			{
				locationHint = locationHint.InternalAssembly;
			}
			return Assembly._nLoad(fileName, codeBase, assemblySecurity, locationHint, ref stackMark, throwOnFileNotFound, forIntrospection);
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x00049690 File Offset: 0x00048690
		private static IPermission CreateWebPermission(string codeBase)
		{
			Assembly assembly = Assembly.Load("System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			Type type = assembly.GetType("System.Net.NetworkAccess", true);
			IPermission permission = null;
			if (type.IsEnum && type.IsVisible)
			{
				object[] array = new object[2];
				array[0] = (Enum)Enum.Parse(type, "Connect", true);
				if (array[0] != null)
				{
					array[1] = codeBase;
					type = assembly.GetType("System.Net.WebPermission", true);
					if (type.IsVisible)
					{
						permission = (IPermission)Activator.CreateInstance(type, array);
					}
				}
			}
			if (permission == null)
			{
				throw new ExecutionEngineException();
			}
			return permission;
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x00049718 File Offset: 0x00048718
		private Module OnModuleResolveEvent(string moduleName)
		{
			ModuleResolveEventHandler moduleResolveEvent = this.ModuleResolveEvent;
			if (moduleResolveEvent == null)
			{
				return null;
			}
			Delegate[] invocationList = moduleResolveEvent.GetInvocationList();
			int num = invocationList.Length;
			for (int i = 0; i < num; i++)
			{
				Module module = ((ModuleResolveEventHandler)invocationList[i])(this, new ResolveEventArgs(moduleName));
				if (module != null)
				{
					return module.InternalModule;
				}
			}
			return null;
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x0004976C File Offset: 0x0004876C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal Assembly InternalGetSatelliteAssembly(CultureInfo culture, Version version, bool throwOnFileNotFound)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.SetPublicKey(this.nGetPublicKey());
			assemblyName.Flags = this.nGetFlags() | AssemblyNameFlags.PublicKey;
			if (version == null)
			{
				assemblyName.Version = this.GetVersion();
			}
			else
			{
				assemblyName.Version = version;
			}
			assemblyName.CultureInfo = culture;
			assemblyName.Name = this.nGetSimpleName() + ".resources";
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			Assembly assembly = Assembly.nLoad(assemblyName, null, null, this, ref stackCrawlMark, throwOnFileNotFound, false);
			if (assembly == this)
			{
				throw new FileNotFoundException(string.Format(culture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[] { assemblyName.Name }));
			}
			return assembly;
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001C9F RID: 7327 RVA: 0x00049820 File Offset: 0x00048820
		internal InternalCache Cache
		{
			get
			{
				InternalCache internalCache = this.m_cachedData;
				if (internalCache == null)
				{
					internalCache = new InternalCache("Assembly");
					this.m_cachedData = internalCache;
					GC.ClearCache += this.OnCacheClear;
				}
				return internalCache;
			}
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x0004985B File Offset: 0x0004885B
		internal void OnCacheClear(object sender, ClearCacheEventArgs cacheEventArgs)
		{
			this.m_cachedData = null;
		}

		// Token: 0x06001CA1 RID: 7329
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Assembly nLoadFile(string path, Evidence evidence);

		// Token: 0x06001CA2 RID: 7330
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Assembly nLoadImage(byte[] rawAssembly, byte[] rawSymbolStore, Evidence evidence, ref StackCrawlMark stackMark, bool fIntrospection);

		// Token: 0x06001CA3 RID: 7331
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _nAddStandAloneResource(string strName, string strFileName, string strFullFileName, int attribute);

		// Token: 0x06001CA4 RID: 7332 RVA: 0x00049864 File Offset: 0x00048864
		internal void nAddStandAloneResource(string strName, string strFileName, string strFullFileName, int attribute)
		{
			this.InternalAssembly._nAddStandAloneResource(strName, strFileName, strFullFileName, attribute);
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x00049876 File Offset: 0x00048876
		internal virtual Module[] nGetModules(bool loadIfNotFound, bool getResourceModules)
		{
			return this.InternalAssembly._nGetModules(loadIfNotFound, getResourceModules);
		}

		// Token: 0x06001CA6 RID: 7334
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Module[] _nGetModules(bool loadIfNotFound, bool getResourceModules);

		// Token: 0x06001CA7 RID: 7335 RVA: 0x00049888 File Offset: 0x00048888
		internal ProcessorArchitecture ComputeProcArchIndex()
		{
			Module manifestModule = this.ManifestModule;
			if (manifestModule != null && manifestModule.MDStreamVersion > 65536)
			{
				PortableExecutableKinds portableExecutableKinds;
				ImageFileMachine imageFileMachine;
				this.ManifestModule.GetPEKind(out portableExecutableKinds, out imageFileMachine);
				if ((portableExecutableKinds & PortableExecutableKinds.PE32Plus) == PortableExecutableKinds.PE32Plus)
				{
					ImageFileMachine imageFileMachine2 = imageFileMachine;
					if (imageFileMachine2 != ImageFileMachine.I386)
					{
						if (imageFileMachine2 == ImageFileMachine.IA64)
						{
							return ProcessorArchitecture.IA64;
						}
						if (imageFileMachine2 == ImageFileMachine.AMD64)
						{
							return ProcessorArchitecture.Amd64;
						}
					}
					else if ((portableExecutableKinds & PortableExecutableKinds.ILOnly) == PortableExecutableKinds.ILOnly)
					{
						return ProcessorArchitecture.MSIL;
					}
				}
				else if (imageFileMachine == ImageFileMachine.I386)
				{
					if ((portableExecutableKinds & PortableExecutableKinds.Required32Bit) == PortableExecutableKinds.Required32Bit)
					{
						return ProcessorArchitecture.X86;
					}
					if ((portableExecutableKinds & PortableExecutableKinds.ILOnly) == PortableExecutableKinds.ILOnly)
					{
						return ProcessorArchitecture.MSIL;
					}
					return ProcessorArchitecture.X86;
				}
			}
			return ProcessorArchitecture.None;
		}

		// Token: 0x04000A75 RID: 2677
		private const string s_localFilePrefix = "file:";

		// Token: 0x04000A76 RID: 2678
		internal AssemblyBuilderData m__assemblyData;

		// Token: 0x04000A78 RID: 2680
		private InternalCache m__cachedData;

		// Token: 0x04000A79 RID: 2681
		private IntPtr m__assembly;
	}
}
