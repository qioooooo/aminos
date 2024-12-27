using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x02000824 RID: 2084
	[ComDefaultInterface(typeof(_ModuleBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class ModuleBuilder : Module, _ModuleBuilder
	{
		// Token: 0x06004B19 RID: 19225 RVA: 0x001056A0 File Offset: 0x001046A0
		internal static string UnmangleTypeName(string typeName)
		{
			int num = typeName.Length - 1;
			for (;;)
			{
				num = typeName.LastIndexOf('+', num);
				if (num == -1)
				{
					break;
				}
				bool flag = true;
				int num2 = num;
				while (typeName[--num2] == '\\')
				{
					flag = !flag;
				}
				if (flag)
				{
					break;
				}
				num = num2;
			}
			return typeName.Substring(num + 1);
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x001056F0 File Offset: 0x001046F0
		internal static Module GetModuleBuilder(Module module)
		{
			ModuleBuilder moduleBuilder = module.InternalModule as ModuleBuilder;
			if (moduleBuilder == null)
			{
				return module;
			}
			ModuleBuilder moduleBuilder2 = null;
			Module module2;
			lock (ModuleBuilder.s_moduleBuilders)
			{
				if (ModuleBuilder.s_moduleBuilders.TryGetValue(moduleBuilder, out moduleBuilder2))
				{
					module2 = moduleBuilder2;
				}
				else
				{
					module2 = moduleBuilder;
				}
			}
			return module2;
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x0010574C File Offset: 0x0010474C
		internal ModuleBuilder(AssemblyBuilder assemblyBuilder, ModuleBuilder internalModuleBuilder)
		{
			this.m_internalModuleBuilder = internalModuleBuilder;
			this.m_assemblyBuilder = assemblyBuilder;
			lock (ModuleBuilder.s_moduleBuilders)
			{
				ModuleBuilder.s_moduleBuilders[internalModuleBuilder] = this;
			}
		}

		// Token: 0x06004B1C RID: 19228 RVA: 0x001057A0 File Offset: 0x001047A0
		private Type GetType(string strFormat, Type baseType)
		{
			if (strFormat == null || strFormat.Equals(string.Empty))
			{
				return baseType;
			}
			char[] array = strFormat.ToCharArray();
			return SymbolType.FormCompoundType(array, baseType, 0);
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x001057CE File Offset: 0x001047CE
		internal void CheckContext(params Type[][] typess)
		{
			((AssemblyBuilder)base.Assembly).CheckContext(typess);
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x001057E1 File Offset: 0x001047E1
		internal void CheckContext(params Type[] types)
		{
			((AssemblyBuilder)base.Assembly).CheckContext(types);
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06004B1F RID: 19231 RVA: 0x001057F4 File Offset: 0x001047F4
		private bool IsInternal
		{
			get
			{
				return this.m_internalModuleBuilder == null;
			}
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x00105800 File Offset: 0x00104800
		private void DemandGrantedAssemblyPermission()
		{
			AssemblyBuilder assemblyBuilder = (AssemblyBuilder)base.Assembly;
			assemblyBuilder.DemandGrantedPermission();
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x00105820 File Offset: 0x00104820
		internal virtual Type FindTypeBuilderWithName(string strTypeName, bool ignoreCase)
		{
			int count = base.m_TypeBuilderList.Count;
			Type type = null;
			int i;
			for (i = 0; i < count; i++)
			{
				type = (Type)base.m_TypeBuilderList[i];
				if (ignoreCase)
				{
					if (string.Compare(type.FullName, strTypeName, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0)
					{
						break;
					}
				}
				else if (type.FullName.Equals(strTypeName))
				{
					break;
				}
			}
			if (i == count)
			{
				type = null;
			}
			return type;
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x00105888 File Offset: 0x00104888
		internal Type GetRootElementType(Type type)
		{
			if (!type.IsByRef && !type.IsPointer && !type.IsArray)
			{
				return type;
			}
			return this.GetRootElementType(type.GetElementType());
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x001058B0 File Offset: 0x001048B0
		internal void SetEntryPoint(MethodInfo entryPoint)
		{
			base.m_EntryPoint = this.GetMethodTokenInternal(entryPoint);
		}

		// Token: 0x06004B24 RID: 19236 RVA: 0x001058C0 File Offset: 0x001048C0
		internal void PreSave(string fileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.PreSaveNoLock(fileName, portableExecutableKind, imageFileMachine);
					return;
				}
			}
			this.PreSaveNoLock(fileName, portableExecutableKind, imageFileMachine);
		}

		// Token: 0x06004B25 RID: 19237 RVA: 0x0010591C File Offset: 0x0010491C
		private void PreSaveNoLock(string fileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			if (base.m_moduleData.m_isSaved)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("InvalidOperation_ModuleHasBeenSaved"), new object[] { base.m_moduleData.m_strModuleName }));
			}
			if (!base.m_moduleData.m_fGlobalBeenCreated && base.m_moduleData.m_fHasGlobal)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GlobalFunctionNotBaked"));
			}
			int count = base.m_TypeBuilderList.Count;
			for (int i = 0; i < count; i++)
			{
				object obj = base.m_TypeBuilderList[i];
				TypeBuilder typeBuilder;
				if (obj is TypeBuilder)
				{
					typeBuilder = (TypeBuilder)obj;
				}
				else
				{
					EnumBuilder enumBuilder = (EnumBuilder)obj;
					typeBuilder = enumBuilder.m_typeBuilder;
				}
				if (!typeBuilder.m_hasBeenCreated && !typeBuilder.m_isHiddenType)
				{
					throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("NotSupported_NotAllTypesAreBaked"), new object[] { typeBuilder.FullName }));
				}
			}
			base.InternalPreSavePEFile((int)portableExecutableKind, (int)imageFileMachine);
		}

		// Token: 0x06004B26 RID: 19238 RVA: 0x00105A20 File Offset: 0x00104A20
		internal void Save(string fileName, bool isAssemblyFile, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.SaveNoLock(fileName, isAssemblyFile, portableExecutableKind, imageFileMachine);
					return;
				}
			}
			this.SaveNoLock(fileName, isAssemblyFile, portableExecutableKind, imageFileMachine);
		}

		// Token: 0x06004B27 RID: 19239 RVA: 0x00105A80 File Offset: 0x00104A80
		private void SaveNoLock(string fileName, bool isAssemblyFile, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
		{
			if (base.m_moduleData.m_embeddedRes != null)
			{
				ResWriterData resWriterData = base.m_moduleData.m_embeddedRes;
				int num = 0;
				while (resWriterData != null)
				{
					resWriterData = resWriterData.m_nextResWriter;
					num++;
				}
				base.InternalSetResourceCounts(num);
				for (resWriterData = base.m_moduleData.m_embeddedRes; resWriterData != null; resWriterData = resWriterData.m_nextResWriter)
				{
					if (resWriterData.m_resWriter != null)
					{
						resWriterData.m_resWriter.Generate();
					}
					byte[] array = new byte[resWriterData.m_memoryStream.Length];
					resWriterData.m_memoryStream.Flush();
					resWriterData.m_memoryStream.Position = 0L;
					resWriterData.m_memoryStream.Read(array, 0, array.Length);
					base.InternalAddResource(resWriterData.m_strName, array, array.Length, base.m_moduleData.m_tkFile, (int)resWriterData.m_attribute, (int)portableExecutableKind, (int)imageFileMachine);
				}
			}
			if (base.m_moduleData.m_strResourceFileName != null)
			{
				base.InternalDefineNativeResourceFile(base.m_moduleData.m_strResourceFileName, (int)portableExecutableKind, (int)imageFileMachine);
			}
			else if (base.m_moduleData.m_resourceBytes != null)
			{
				base.InternalDefineNativeResourceBytes(base.m_moduleData.m_resourceBytes, (int)portableExecutableKind, (int)imageFileMachine);
			}
			if (isAssemblyFile)
			{
				base.InternalSavePEFile(fileName, base.m_EntryPoint, (int)base.Assembly.m_assemblyData.m_peFileKind, true);
			}
			else
			{
				base.InternalSavePEFile(fileName, base.m_EntryPoint, 1, false);
			}
			base.m_moduleData.m_isSaved = true;
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x00105BD0 File Offset: 0x00104BD0
		internal int GetTypeRefNested(Type type, Module refedModule, string strRefedModuleFileName)
		{
			Type declaringType = type.DeclaringType;
			int num = 0;
			string text = type.FullName;
			if (declaringType != null)
			{
				num = this.GetTypeRefNested(declaringType, refedModule, strRefedModuleFileName);
				text = ModuleBuilder.UnmangleTypeName(text);
			}
			return base.InternalGetTypeToken(text, refedModule, strRefedModuleFileName, num);
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x00105C0C File Offset: 0x00104C0C
		internal MethodToken InternalGetConstructorToken(ConstructorInfo con, bool usingRef)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			int num2;
			if (con is ConstructorBuilder)
			{
				ConstructorBuilder constructorBuilder = con as ConstructorBuilder;
				if (!usingRef && constructorBuilder.ReflectedType.Module.InternalModule.Equals(this.InternalModule))
				{
					return constructorBuilder.GetToken();
				}
				int num = this.GetTypeTokenInternal(con.ReflectedType).Token;
				num2 = base.InternalGetMemberRef(con.ReflectedType.Module, num, constructorBuilder.GetToken().Token);
			}
			else if (con is ConstructorOnTypeBuilderInstantiation)
			{
				ConstructorOnTypeBuilderInstantiation constructorOnTypeBuilderInstantiation = con as ConstructorOnTypeBuilderInstantiation;
				if (usingRef)
				{
					throw new InvalidOperationException();
				}
				int num = this.GetTypeTokenInternal(con.DeclaringType).Token;
				num2 = base.InternalGetMemberRef(con.DeclaringType.Module, num, constructorOnTypeBuilderInstantiation.m_ctor.MetadataTokenInternal);
			}
			else if (con is RuntimeConstructorInfo && !con.ReflectedType.IsArray)
			{
				int num = this.GetTypeTokenInternal(con.ReflectedType).Token;
				num2 = base.InternalGetMemberRefOfMethodInfo(num, con.GetMethodHandle());
			}
			else
			{
				ParameterInfo[] parameters = con.GetParameters();
				Type[] array = new Type[parameters.Length];
				Type[][] array2 = new Type[array.Length][];
				Type[][] array3 = new Type[array.Length][];
				for (int i = 0; i < parameters.Length; i++)
				{
					array[i] = parameters[i].ParameterType;
					array2[i] = parameters[i].GetRequiredCustomModifiers();
					array3[i] = parameters[i].GetOptionalCustomModifiers();
				}
				int num = this.GetTypeTokenInternal(con.ReflectedType).Token;
				SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(this, con.CallingConvention, null, null, null, array, array2, array3);
				int num3;
				byte[] array4 = methodSigHelper.InternalGetSignature(out num3);
				num2 = base.InternalGetMemberRefFromSignature(num, con.Name, array4, num3);
			}
			return new MethodToken(num2);
		}

		// Token: 0x06004B2A RID: 19242 RVA: 0x00105DE4 File Offset: 0x00104DE4
		internal void Init(string strModuleName, string strFileName, ISymbolWriter writer)
		{
			base.m_moduleData = new ModuleBuilderData(this, strModuleName, strFileName);
			base.m_TypeBuilderList = new ArrayList();
			base.m_iSymWriter = writer;
			if (writer != null)
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				writer.SetUnderlyingWriter(base.m_pInternalSymWriter);
			}
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x00105E20 File Offset: 0x00104E20
		internal int GetMemberRefToken(MethodBase method, Type[] optionalParameterTypes)
		{
			int num = 0;
			if (method.IsGenericMethod)
			{
				if (!method.IsGenericMethodDefinition)
				{
					throw new InvalidOperationException();
				}
				num = method.GetGenericArguments().Length;
			}
			if (optionalParameterTypes != null && (method.CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAVarArgCallingConvention"));
			}
			MethodInfo methodInfo = method as MethodInfo;
			Type[] array;
			Type type;
			if (method.DeclaringType.IsGenericType)
			{
				MethodOnTypeBuilderInstantiation methodOnTypeBuilderInstantiation;
				MethodBase methodBase;
				ConstructorOnTypeBuilderInstantiation constructorOnTypeBuilderInstantiation;
				if ((methodOnTypeBuilderInstantiation = method as MethodOnTypeBuilderInstantiation) != null)
				{
					methodBase = methodOnTypeBuilderInstantiation.m_method;
				}
				else if ((constructorOnTypeBuilderInstantiation = method as ConstructorOnTypeBuilderInstantiation) != null)
				{
					methodBase = constructorOnTypeBuilderInstantiation.m_ctor;
				}
				else if (method is MethodBuilder || method is ConstructorBuilder)
				{
					methodBase = method;
				}
				else if (method.IsGenericMethod)
				{
					methodBase = methodInfo.GetGenericMethodDefinition();
					methodBase = methodBase.Module.ResolveMethod(methodBase.MetadataTokenInternal, methodBase.GetGenericArguments(), (methodBase.DeclaringType != null) ? methodBase.DeclaringType.GetGenericArguments() : null);
				}
				else
				{
					methodBase = method.Module.ResolveMethod(method.MetadataTokenInternal, null, (method.DeclaringType != null) ? method.DeclaringType.GetGenericArguments() : null);
				}
				array = methodBase.GetParameterTypes();
				type = methodBase.GetReturnType();
			}
			else
			{
				array = method.GetParameterTypes();
				type = method.GetReturnType();
			}
			int num3;
			if (method.DeclaringType.IsGenericType)
			{
				int num2;
				byte[] array2 = SignatureHelper.GetTypeSigToken(this, method.DeclaringType).InternalGetSignature(out num2);
				num3 = base.InternalGetTypeSpecTokenWithBytes(array2, num2);
			}
			else if (method.Module.InternalModule != this.InternalModule)
			{
				num3 = this.GetTypeToken(method.DeclaringType).Token;
			}
			else if (methodInfo != null)
			{
				num3 = this.GetMethodToken(method as MethodInfo).Token;
			}
			else
			{
				num3 = this.GetConstructorToken(method as ConstructorInfo).Token;
			}
			int num4;
			byte[] array3 = this.GetMemberRefSignature(method.CallingConvention, type, array, optionalParameterTypes, num).InternalGetSignature(out num4);
			return base.InternalGetMemberRefFromSignature(num3, method.Name, array3, num4);
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x0010601C File Offset: 0x0010501C
		internal SignatureHelper GetMemberRefSignature(CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes, int cGenericParameters)
		{
			int num;
			if (parameterTypes == null)
			{
				num = 0;
			}
			else
			{
				num = parameterTypes.Length;
			}
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(this, call, returnType, cGenericParameters);
			for (int i = 0; i < num; i++)
			{
				methodSigHelper.AddArgument(parameterTypes[i]);
			}
			if (optionalParameterTypes != null && optionalParameterTypes.Length != 0)
			{
				methodSigHelper.AddSentinel();
				for (int i = 0; i < optionalParameterTypes.Length; i++)
				{
					methodSigHelper.AddArgument(optionalParameterTypes[i]);
				}
			}
			return methodSigHelper;
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x0010607E File Offset: 0x0010507E
		internal override bool IsDynamic()
		{
			return true;
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06004B2E RID: 19246 RVA: 0x00106081 File Offset: 0x00105081
		internal override Module InternalModule
		{
			get
			{
				if (this.IsInternal)
				{
					return this;
				}
				return this.m_internalModuleBuilder;
			}
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x00106093 File Offset: 0x00105093
		internal override Assembly GetAssemblyInternal()
		{
			if (!this.IsInternal)
			{
				return this.m_assemblyBuilder;
			}
			return base._GetAssemblyInternal();
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x001060AC File Offset: 0x001050AC
		public override Type[] GetTypes()
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetTypesNoLock();
				}
			}
			return this.GetTypesNoLock();
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x00106108 File Offset: 0x00105108
		internal Type[] GetTypesNoLock()
		{
			int count = base.m_TypeBuilderList.Count;
			List<Type> list = new List<Type>(count);
			bool flag = false;
			if (this.IsInternal)
			{
				try
				{
					this.DemandGrantedAssemblyPermission();
					flag = true;
					goto IL_002E;
				}
				catch (SecurityException)
				{
					flag = false;
					goto IL_002E;
				}
			}
			flag = true;
			IL_002E:
			for (int i = 0; i < count; i++)
			{
				EnumBuilder enumBuilder = base.m_TypeBuilderList[i] as EnumBuilder;
				TypeBuilder typeBuilder;
				if (enumBuilder != null)
				{
					typeBuilder = enumBuilder.m_typeBuilder;
				}
				else
				{
					typeBuilder = base.m_TypeBuilderList[i] as TypeBuilder;
				}
				if (typeBuilder != null)
				{
					if (typeBuilder.m_hasBeenCreated)
					{
						list.Add(typeBuilder.UnderlyingSystemType);
					}
					else if (flag)
					{
						list.Add(typeBuilder);
					}
				}
				else
				{
					list.Add((Type)base.m_TypeBuilderList[i]);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x001061DC File Offset: 0x001051DC
		[ComVisible(true)]
		public override Type GetType(string className)
		{
			return this.GetType(className, false, false);
		}

		// Token: 0x06004B33 RID: 19251 RVA: 0x001061E7 File Offset: 0x001051E7
		[ComVisible(true)]
		public override Type GetType(string className, bool ignoreCase)
		{
			return this.GetType(className, false, ignoreCase);
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x001061F4 File Offset: 0x001051F4
		[ComVisible(true)]
		public override Type GetType(string className, bool throwOnError, bool ignoreCase)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetTypeNoLock(className, throwOnError, ignoreCase);
				}
			}
			return this.GetTypeNoLock(className, throwOnError, ignoreCase);
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x00106260 File Offset: 0x00105260
		private Type GetTypeNoLock(string className, bool throwOnError, bool ignoreCase)
		{
			Type type = base.GetType(className, throwOnError, ignoreCase);
			if (type != null)
			{
				return type;
			}
			string text = null;
			string text2 = null;
			int num;
			for (int i = 0; i <= className.Length; i = num + 1)
			{
				num = className.IndexOfAny(new char[] { '[', '*', '&' }, i);
				if (num == -1)
				{
					text = className;
					text2 = null;
					break;
				}
				int num2 = 0;
				int num3 = num - 1;
				while (num3 >= 0 && className[num3] == '\\')
				{
					num2++;
					num3--;
				}
				if (num2 % 2 != 1)
				{
					text = className.Substring(0, num);
					text2 = className.Substring(num);
					break;
				}
			}
			if (text == null)
			{
				text = className;
				text2 = null;
			}
			text = text.Replace("\\\\", "\\").Replace("\\[", "[").Replace("\\*", "*")
				.Replace("\\&", "&");
			if (text2 != null)
			{
				type = base.GetType(text, false, ignoreCase);
			}
			bool flag = false;
			if (this.IsInternal)
			{
				try
				{
					this.DemandGrantedAssemblyPermission();
					flag = true;
					goto IL_0101;
				}
				catch (SecurityException)
				{
					flag = false;
					goto IL_0101;
				}
			}
			flag = true;
			IL_0101:
			if (type == null && flag)
			{
				type = this.FindTypeBuilderWithName(text, ignoreCase);
				if (type == null && base.Assembly is AssemblyBuilder)
				{
					ArrayList moduleBuilderList = base.Assembly.m_assemblyData.m_moduleBuilderList;
					int count = moduleBuilderList.Count;
					int num4 = 0;
					while (num4 < count && type == null)
					{
						ModuleBuilder moduleBuilder = (ModuleBuilder)moduleBuilderList[num4];
						type = moduleBuilder.FindTypeBuilderWithName(text, ignoreCase);
						num4++;
					}
				}
			}
			if (type == null)
			{
				return null;
			}
			if (text2 == null)
			{
				return type;
			}
			return this.GetType(text2, type);
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06004B36 RID: 19254 RVA: 0x001063FC File Offset: 0x001053FC
		public override string FullyQualifiedName
		{
			get
			{
				string text = base.m_moduleData.m_strFileName;
				if (text == null)
				{
					return null;
				}
				if (base.Assembly.m_assemblyData.m_strDir != null)
				{
					text = Path.Combine(base.Assembly.m_assemblyData.m_strDir, text);
					text = Path.GetFullPath(text);
				}
				if (base.Assembly.m_assemblyData.m_strDir != null && text != null)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
				}
				return text;
			}
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x0010646C File Offset: 0x0010546C
		public TypeBuilder DefineType(string name)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name);
				}
			}
			return this.DefineTypeNoLock(name);
		}

		// Token: 0x06004B38 RID: 19256 RVA: 0x001064D8 File Offset: 0x001054D8
		private TypeBuilder DefineTypeNoLock(string name)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, TypeAttributes.NotPublic, null, null, this, PackingSize.Unspecified, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B39 RID: 19257 RVA: 0x00106500 File Offset: 0x00105500
		public TypeBuilder DefineType(string name, TypeAttributes attr)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr);
				}
			}
			return this.DefineTypeNoLock(name, attr);
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x0010656C File Offset: 0x0010556C
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, null, null, this, PackingSize.Unspecified, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x00106594 File Offset: 0x00105594
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr, parent);
				}
			}
			return this.DefineTypeNoLock(name, attr, parent);
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x00106604 File Offset: 0x00105604
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent)
		{
			this.CheckContext(new Type[] { parent });
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, null, this, PackingSize.Unspecified, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x00106640 File Offset: 0x00105640
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, int typesize)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr, parent, typesize);
				}
			}
			return this.DefineTypeNoLock(name, attr, parent, typesize);
		}

		// Token: 0x06004B3E RID: 19262 RVA: 0x001066B4 File Offset: 0x001056B4
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, int typesize)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, this, PackingSize.Unspecified, typesize, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B3F RID: 19263 RVA: 0x001066E0 File Offset: 0x001056E0
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr, parent, packingSize, typesize);
				}
			}
			return this.DefineTypeNoLock(name, attr, parent, packingSize, typesize);
		}

		// Token: 0x06004B40 RID: 19264 RVA: 0x00106758 File Offset: 0x00105758
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, this, packingSize, typesize, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B41 RID: 19265 RVA: 0x00106784 File Offset: 0x00105784
		[ComVisible(true)]
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr, parent, interfaces);
				}
			}
			return this.DefineTypeNoLock(name, attr, parent, interfaces);
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x001067F8 File Offset: 0x001057F8
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, interfaces, this, PackingSize.Unspecified, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x00106824 File Offset: 0x00105824
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packsize)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineTypeNoLock(name, attr, parent, packsize);
				}
			}
			return this.DefineTypeNoLock(name, attr, parent, packsize);
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x00106898 File Offset: 0x00105898
		private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, PackingSize packsize)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, null, this, packsize, null);
			base.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x001068C4 File Offset: 0x001058C4
		public EnumBuilder DefineEnum(string name, TypeAttributes visibility, Type underlyingType)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			this.CheckContext(new Type[] { underlyingType });
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineEnumNoLock(name, visibility, underlyingType);
				}
			}
			return this.DefineEnumNoLock(name, visibility, underlyingType);
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x00106944 File Offset: 0x00105944
		private EnumBuilder DefineEnumNoLock(string name, TypeAttributes visibility, Type underlyingType)
		{
			EnumBuilder enumBuilder = new EnumBuilder(name, underlyingType, visibility, this);
			base.m_TypeBuilderList.Add(enumBuilder);
			return enumBuilder;
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x00106969 File Offset: 0x00105969
		public IResourceWriter DefineResource(string name, string description)
		{
			return this.DefineResource(name, description, ResourceAttributes.Public);
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x00106974 File Offset: 0x00105974
		public IResourceWriter DefineResource(string name, string description, ResourceAttributes attribute)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineResourceNoLock(name, description, attribute);
				}
			}
			return this.DefineResourceNoLock(name, description, attribute);
		}

		// Token: 0x06004B49 RID: 19273 RVA: 0x001069E4 File Offset: 0x001059E4
		private IResourceWriter DefineResourceNoLock(string name, string description, ResourceAttributes attribute)
		{
			if (this.IsTransient())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			Assembly assembly = base.Assembly;
			if (!(assembly is AssemblyBuilder))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
			}
			AssemblyBuilder assemblyBuilder = (AssemblyBuilder)assembly;
			if (assemblyBuilder.IsPersistable())
			{
				assemblyBuilder.m_assemblyData.CheckResNameConflict(name);
				MemoryStream memoryStream = new MemoryStream();
				ResourceWriter resourceWriter = new ResourceWriter(memoryStream);
				ResWriterData resWriterData = new ResWriterData(resourceWriter, memoryStream, name, string.Empty, string.Empty, attribute);
				resWriterData.m_nextResWriter = base.m_moduleData.m_embeddedRes;
				base.m_moduleData.m_embeddedRes = resWriterData;
				return resourceWriter;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x00106AC0 File Offset: 0x00105AC0
		public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.DefineManifestResourceNoLock(name, stream, attribute);
					return;
				}
			}
			this.DefineManifestResourceNoLock(name, stream, attribute);
		}

		// Token: 0x06004B4B RID: 19275 RVA: 0x00106B48 File Offset: 0x00105B48
		private void DefineManifestResourceNoLock(string name, Stream stream, ResourceAttributes attribute)
		{
			if (this.IsTransient())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			Assembly assembly = base.Assembly;
			if (!(assembly is AssemblyBuilder))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
			}
			AssemblyBuilder assemblyBuilder = (AssemblyBuilder)assembly;
			if (assemblyBuilder.IsPersistable())
			{
				assemblyBuilder.m_assemblyData.CheckResNameConflict(name);
				ResWriterData resWriterData = new ResWriterData(null, stream, name, string.Empty, string.Empty, attribute);
				resWriterData.m_nextResWriter = base.m_moduleData.m_embeddedRes;
				base.m_moduleData.m_embeddedRes = resWriterData;
				return;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
		}

		// Token: 0x06004B4C RID: 19276 RVA: 0x00106C14 File Offset: 0x00105C14
		public void DefineUnmanagedResource(byte[] resource)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.DefineUnmanagedResourceInternalNoLock(resource);
					return;
				}
			}
			this.DefineUnmanagedResourceInternalNoLock(resource);
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x00106C7C File Offset: 0x00105C7C
		internal void DefineUnmanagedResourceInternalNoLock(byte[] resource)
		{
			if (base.m_moduleData.m_strResourceFileName != null || base.m_moduleData.m_resourceBytes != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			if (resource == null)
			{
				throw new ArgumentNullException("resource");
			}
			base.m_moduleData.m_resourceBytes = new byte[resource.Length];
			Array.Copy(resource, base.m_moduleData.m_resourceBytes, resource.Length);
		}

		// Token: 0x06004B4E RID: 19278 RVA: 0x00106CE8 File Offset: 0x00105CE8
		public void DefineUnmanagedResource(string resourceFileName)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.DefineUnmanagedResourceFileInternalNoLock(resourceFileName);
					return;
				}
			}
			this.DefineUnmanagedResourceFileInternalNoLock(resourceFileName);
		}

		// Token: 0x06004B4F RID: 19279 RVA: 0x00106D50 File Offset: 0x00105D50
		internal void DefineUnmanagedResourceFileInternalNoLock(string resourceFileName)
		{
			if (base.m_moduleData.m_resourceBytes != null || base.m_moduleData.m_strResourceFileName != null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
			}
			if (resourceFileName == null)
			{
				throw new ArgumentNullException("resourceFileName");
			}
			string fullPath = Path.GetFullPath(resourceFileName);
			new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();
			new EnvironmentPermission(PermissionState.Unrestricted).Assert();
			try
			{
				if (!File.Exists(resourceFileName))
				{
					throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[] { resourceFileName }), resourceFileName);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			base.m_moduleData.m_strResourceFileName = fullPath;
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x00106E08 File Offset: 0x00105E08
		public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return this.DefineGlobalMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x00106E18 File Offset: 0x00105E18
		public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return this.DefineGlobalMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x00106E38 File Offset: 0x00105E38
		public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineGlobalMethodNoLock(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
				}
			}
			return this.DefineGlobalMethodNoLock(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x00106EC0 File Offset: 0x00105EC0
		private MethodBuilder DefineGlobalMethodNoLock(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			this.CheckContext(new Type[] { returnType });
			this.CheckContext(new Type[][] { requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes });
			this.CheckContext(requiredParameterTypeCustomModifiers);
			this.CheckContext(optionalParameterTypeCustomModifiers);
			if (base.m_moduleData.m_fGlobalBeenCreated)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GlobalsHaveBeenCreated"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_GlobalFunctionHasToBeStatic"));
			}
			base.m_moduleData.m_fHasGlobal = true;
			return base.m_moduleData.m_globalTypeBuilder.DefineMethod(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x00106F98 File Offset: 0x00105F98
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			return this.DefinePInvokeMethod(name, dllName, name, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x00106FBC File Offset: 0x00105FBC
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefinePInvokeMethodNoLock(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
				}
			}
			return this.DefinePInvokeMethodNoLock(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x00107044 File Offset: 0x00106044
		private MethodBuilder DefinePInvokeMethodNoLock(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.CheckContext(new Type[] { returnType });
			this.CheckContext(parameterTypes);
			if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_GlobalFunctionHasToBeStatic"));
			}
			base.m_moduleData.m_fHasGlobal = true;
			return base.m_moduleData.m_globalTypeBuilder.DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x001070B0 File Offset: 0x001060B0
		public void CreateGlobalFunctions()
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.CreateGlobalFunctionsNoLock();
					return;
				}
			}
			this.CreateGlobalFunctionsNoLock();
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x00107114 File Offset: 0x00106114
		private void CreateGlobalFunctionsNoLock()
		{
			if (base.m_moduleData.m_fGlobalBeenCreated)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
			}
			base.m_moduleData.m_globalTypeBuilder.CreateType();
			base.m_moduleData.m_fGlobalBeenCreated = true;
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x00107150 File Offset: 0x00106150
		public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineInitializedDataNoLock(name, data, attributes);
				}
			}
			return this.DefineInitializedDataNoLock(name, data, attributes);
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x001071C0 File Offset: 0x001061C0
		private FieldBuilder DefineInitializedDataNoLock(string name, byte[] data, FieldAttributes attributes)
		{
			if (base.m_moduleData.m_fGlobalBeenCreated)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GlobalsHaveBeenCreated"));
			}
			base.m_moduleData.m_fHasGlobal = true;
			return base.m_moduleData.m_globalTypeBuilder.DefineInitializedData(name, data, attributes);
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x00107200 File Offset: 0x00106200
		public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineUninitializedDataNoLock(name, size, attributes);
				}
			}
			return this.DefineUninitializedDataNoLock(name, size, attributes);
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x00107270 File Offset: 0x00106270
		private FieldBuilder DefineUninitializedDataNoLock(string name, int size, FieldAttributes attributes)
		{
			if (base.m_moduleData.m_fGlobalBeenCreated)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GlobalsHaveBeenCreated"));
			}
			base.m_moduleData.m_fHasGlobal = true;
			return base.m_moduleData.m_globalTypeBuilder.DefineUninitializedData(name, size, attributes);
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x001072AE File Offset: 0x001062AE
		internal TypeToken GetTypeTokenInternal(Type type)
		{
			return this.GetTypeTokenInternal(type, false);
		}

		// Token: 0x06004B5E RID: 19294 RVA: 0x001072B8 File Offset: 0x001062B8
		internal TypeToken GetTypeTokenInternal(Type type, bool getGenericDefinition)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetTypeTokenWorkerNoLock(type, getGenericDefinition);
				}
			}
			return this.GetTypeTokenWorkerNoLock(type, getGenericDefinition);
		}

		// Token: 0x06004B5F RID: 19295 RVA: 0x00107318 File Offset: 0x00106318
		public TypeToken GetTypeToken(Type type)
		{
			return this.GetTypeTokenInternal(type, true);
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x00107324 File Offset: 0x00106324
		private TypeToken GetTypeTokenWorkerNoLock(Type type, bool getGenericDefinition)
		{
			this.CheckContext(new Type[] { type });
			string text = string.Empty;
			bool flag = false;
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Module moduleBuilder = ModuleBuilder.GetModuleBuilder(type.Module);
			bool flag2 = moduleBuilder.Equals(this);
			if (type.IsByRef)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CannotGetTypeTokenForByRef"));
			}
			if ((type.IsGenericType && (!type.IsGenericTypeDefinition || !getGenericDefinition)) || type.IsGenericParameter || type.IsArray || type.IsPointer)
			{
				int num;
				byte[] array = SignatureHelper.GetTypeSigToken(this, type).InternalGetSignature(out num);
				return new TypeToken(base.InternalGetTypeSpecTokenWithBytes(array, num));
			}
			if (flag2)
			{
				EnumBuilder enumBuilder = type as EnumBuilder;
				TypeBuilder typeBuilder;
				if (enumBuilder != null)
				{
					typeBuilder = enumBuilder.m_typeBuilder;
				}
				else
				{
					typeBuilder = type as TypeBuilder;
				}
				if (typeBuilder != null)
				{
					return typeBuilder.TypeToken;
				}
				if (type is GenericTypeParameterBuilder)
				{
					return new TypeToken(type.MetadataTokenInternal);
				}
				return new TypeToken(this.GetTypeRefNested(type, this, string.Empty));
			}
			else
			{
				ModuleBuilder moduleBuilder2 = moduleBuilder as ModuleBuilder;
				if (moduleBuilder2 != null)
				{
					if (moduleBuilder2.IsTransient())
					{
						flag = true;
					}
					text = moduleBuilder2.m_moduleData.m_strFileName;
				}
				else
				{
					text = moduleBuilder.ScopeName;
				}
				if (!this.IsTransient() && flag)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadTransientModuleReference"));
				}
				TypeToken typeToken = new TypeToken(this.GetTypeRefNested(type, moduleBuilder, text));
				return typeToken;
			}
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x00107484 File Offset: 0x00106484
		public TypeToken GetTypeToken(string name)
		{
			return this.GetTypeToken(base.GetType(name, false, true));
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x00107498 File Offset: 0x00106498
		public MethodToken GetMethodToken(MethodInfo method)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetMethodTokenNoLock(method, true);
				}
			}
			return this.GetMethodTokenNoLock(method, true);
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x001074F8 File Offset: 0x001064F8
		internal MethodToken GetMethodTokenInternal(MethodInfo method)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetMethodTokenNoLock(method, false);
				}
			}
			return this.GetMethodTokenNoLock(method, false);
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x00107558 File Offset: 0x00106558
		private MethodToken GetMethodTokenNoLock(MethodInfo method, bool getGenericTypeDefinition)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			int num2;
			if (method is MethodBuilder)
			{
				if (method.Module.InternalModule == this.InternalModule)
				{
					return new MethodToken(method.MetadataTokenInternal);
				}
				if (method.DeclaringType == null)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
				}
				int num = (getGenericTypeDefinition ? this.GetTypeToken(method.DeclaringType).Token : this.GetTypeTokenInternal(method.DeclaringType).Token);
				num2 = base.InternalGetMemberRef(method.DeclaringType.Module, num, method.MetadataTokenInternal);
			}
			else
			{
				if (method is MethodOnTypeBuilderInstantiation)
				{
					return new MethodToken(this.GetMemberRefToken(method, null));
				}
				if (method is SymbolMethod)
				{
					SymbolMethod symbolMethod = method as SymbolMethod;
					if (symbolMethod.GetModule() == this)
					{
						return symbolMethod.GetToken();
					}
					return symbolMethod.GetToken(this);
				}
				else
				{
					Type declaringType = method.DeclaringType;
					if (declaringType == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
					}
					if (declaringType.IsArray)
					{
						ParameterInfo[] parameters = method.GetParameters();
						Type[] array = new Type[parameters.Length];
						for (int i = 0; i < parameters.Length; i++)
						{
							array[i] = parameters[i].ParameterType;
						}
						return this.GetArrayMethodToken(declaringType, method.Name, method.CallingConvention, method.ReturnType, array);
					}
					if (method is RuntimeMethodInfo)
					{
						int num = (getGenericTypeDefinition ? this.GetTypeToken(method.DeclaringType).Token : this.GetTypeTokenInternal(method.DeclaringType).Token);
						num2 = base.InternalGetMemberRefOfMethodInfo(num, method.GetMethodHandle());
					}
					else
					{
						ParameterInfo[] parameters2 = method.GetParameters();
						Type[] array2 = new Type[parameters2.Length];
						Type[][] array3 = new Type[array2.Length][];
						Type[][] array4 = new Type[array2.Length][];
						for (int j = 0; j < parameters2.Length; j++)
						{
							array2[j] = parameters2[j].ParameterType;
							array3[j] = parameters2[j].GetRequiredCustomModifiers();
							array4[j] = parameters2[j].GetOptionalCustomModifiers();
						}
						int num = (getGenericTypeDefinition ? this.GetTypeToken(method.DeclaringType).Token : this.GetTypeTokenInternal(method.DeclaringType).Token);
						SignatureHelper signatureHelper;
						try
						{
							signatureHelper = SignatureHelper.GetMethodSigHelper(this, method.CallingConvention, method.ReturnType, method.ReturnParameter.GetRequiredCustomModifiers(), method.ReturnParameter.GetOptionalCustomModifiers(), array2, array3, array4);
						}
						catch (NotImplementedException)
						{
							signatureHelper = SignatureHelper.GetMethodSigHelper(this, method.ReturnType, array2);
						}
						int num3;
						byte[] array5 = signatureHelper.InternalGetSignature(out num3);
						num2 = base.InternalGetMemberRefFromSignature(num, method.Name, array5, num3);
					}
				}
			}
			return new MethodToken(num2);
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x00107814 File Offset: 0x00106814
		public MethodToken GetArrayMethodToken(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetArrayMethodTokenNoLock(arrayClass, methodName, callingConvention, returnType, parameterTypes);
				}
			}
			return this.GetArrayMethodTokenNoLock(arrayClass, methodName, callingConvention, returnType, parameterTypes);
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x0010787C File Offset: 0x0010687C
		private MethodToken GetArrayMethodTokenNoLock(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			this.CheckContext(new Type[] { returnType, arrayClass });
			this.CheckContext(parameterTypes);
			if (arrayClass == null)
			{
				throw new ArgumentNullException("arrayClass");
			}
			if (methodName == null)
			{
				throw new ArgumentNullException("methodName");
			}
			if (methodName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "methodName");
			}
			if (!arrayClass.IsArray)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_HasToBeArrayClass"));
			}
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(this, callingConvention, returnType, null, null, parameterTypes, null, null);
			int num;
			byte[] array = methodSigHelper.InternalGetSignature(out num);
			Type type = arrayClass;
			while (type.IsArray)
			{
				type = type.GetElementType();
			}
			int token = this.GetTypeTokenInternal(type).Token;
			return new MethodToken(base.nativeGetArrayMethodToken(this.GetTypeTokenInternal(arrayClass).Token, methodName, array, num, token));
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x0010795C File Offset: 0x0010695C
		public MethodInfo GetArrayMethod(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			this.CheckContext(new Type[] { returnType, arrayClass });
			this.CheckContext(parameterTypes);
			MethodToken arrayMethodToken = this.GetArrayMethodToken(arrayClass, methodName, callingConvention, returnType, parameterTypes);
			return new SymbolMethod(this, arrayMethodToken, arrayClass, methodName, callingConvention, returnType, parameterTypes);
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x001079A4 File Offset: 0x001069A4
		[ComVisible(true)]
		public MethodToken GetConstructorToken(ConstructorInfo con)
		{
			return this.InternalGetConstructorToken(con, false);
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x001079B0 File Offset: 0x001069B0
		public FieldToken GetFieldToken(FieldInfo field)
		{
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.GetFieldTokenNoLock(field);
				}
			}
			return this.GetFieldTokenNoLock(field);
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x00107A0C File Offset: 0x00106A0C
		private FieldToken GetFieldTokenNoLock(FieldInfo field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("con");
			}
			int num3;
			if (field is FieldBuilder)
			{
				FieldBuilder fieldBuilder = (FieldBuilder)field;
				if (field.DeclaringType != null && field.DeclaringType.IsGenericType)
				{
					int num;
					byte[] array = SignatureHelper.GetTypeSigToken(this, field.DeclaringType).InternalGetSignature(out num);
					int num2 = base.InternalGetTypeSpecTokenWithBytes(array, num);
					num3 = base.InternalGetMemberRef(this, num2, fieldBuilder.GetToken().Token);
				}
				else
				{
					if (fieldBuilder.GetTypeBuilder().Module.InternalModule.Equals(this.InternalModule))
					{
						return fieldBuilder.GetToken();
					}
					if (field.DeclaringType == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
					}
					int num2 = this.GetTypeTokenInternal(field.DeclaringType).Token;
					num3 = base.InternalGetMemberRef(field.ReflectedType.Module, num2, fieldBuilder.GetToken().Token);
				}
			}
			else if (field is RuntimeFieldInfo)
			{
				if (field.DeclaringType == null)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
				}
				if (field.DeclaringType != null && field.DeclaringType.IsGenericType)
				{
					int num4;
					byte[] array2 = SignatureHelper.GetTypeSigToken(this, field.DeclaringType).InternalGetSignature(out num4);
					int num2 = base.InternalGetTypeSpecTokenWithBytes(array2, num4);
					num3 = base.InternalGetMemberRefOfFieldInfo(num2, field.DeclaringType.GetTypeHandleInternal(), field.MetadataTokenInternal);
				}
				else
				{
					int num2 = this.GetTypeTokenInternal(field.DeclaringType).Token;
					num3 = base.InternalGetMemberRefOfFieldInfo(num2, field.DeclaringType.GetTypeHandleInternal(), field.MetadataTokenInternal);
				}
			}
			else if (field is FieldOnTypeBuilderInstantiation)
			{
				FieldInfo fieldInfo = ((FieldOnTypeBuilderInstantiation)field).FieldInfo;
				int num5;
				byte[] array3 = SignatureHelper.GetTypeSigToken(this, field.DeclaringType).InternalGetSignature(out num5);
				int num2 = base.InternalGetTypeSpecTokenWithBytes(array3, num5);
				num3 = base.InternalGetMemberRef(fieldInfo.ReflectedType.Module, num2, fieldInfo.MetadataTokenInternal);
			}
			else
			{
				int num2 = this.GetTypeTokenInternal(field.ReflectedType).Token;
				SignatureHelper fieldSigHelper = SignatureHelper.GetFieldSigHelper(this);
				fieldSigHelper.AddArgument(field.FieldType, field.GetRequiredCustomModifiers(), field.GetOptionalCustomModifiers());
				int num6;
				byte[] array4 = fieldSigHelper.InternalGetSignature(out num6);
				num3 = base.InternalGetMemberRefFromSignature(num2, field.Name, array4, num6);
			}
			return new FieldToken(num3, field.GetType());
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x00107C61 File Offset: 0x00106C61
		public StringToken GetStringConstant(string str)
		{
			return new StringToken(base.InternalGetStringConstant(str));
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x00107C70 File Offset: 0x00106C70
		public SignatureToken GetSignatureToken(SignatureHelper sigHelper)
		{
			if (sigHelper == null)
			{
				throw new ArgumentNullException("sigHelper");
			}
			int num;
			byte[] array = sigHelper.InternalGetSignature(out num);
			return new SignatureToken(TypeBuilder.InternalGetTokenFromSig(this, array, num), this);
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x00107CA4 File Offset: 0x00106CA4
		public SignatureToken GetSignatureToken(byte[] sigBytes, int sigLength)
		{
			if (sigBytes == null)
			{
				throw new ArgumentNullException("sigBytes");
			}
			byte[] array = new byte[sigBytes.Length];
			Array.Copy(sigBytes, array, sigBytes.Length);
			return new SignatureToken(TypeBuilder.InternalGetTokenFromSig(this, array, sigLength), this);
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x00107CE4 File Offset: 0x00106CE4
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			TypeBuilder.InternalCreateCustomAttribute(1, this.GetConstructorToken(con).Token, binaryAttribute, this, false);
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x00107D33 File Offset: 0x00106D33
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			customBuilder.CreateCustomAttribute(this, 1);
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x00107D59 File Offset: 0x00106D59
		public ISymbolWriter GetSymWriter()
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			return base.m_iSymWriter;
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x00107D70 File Offset: 0x00106D70
		public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					return this.DefineDocumentNoLock(url, language, languageVendor, documentType);
				}
			}
			return this.DefineDocumentNoLock(url, language, languageVendor, documentType);
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x00107DE4 File Offset: 0x00106DE4
		private ISymbolDocumentWriter DefineDocumentNoLock(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			if (base.m_iSymWriter == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
			}
			return base.m_iSymWriter.DefineDocument(url, language, languageVendor, documentType);
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x00107E10 File Offset: 0x00106E10
		public void SetUserEntryPoint(MethodInfo entryPoint)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.SetUserEntryPointNoLock(entryPoint);
					return;
				}
			}
			this.SetUserEntryPointNoLock(entryPoint);
		}

		// Token: 0x06004B74 RID: 19316 RVA: 0x00107E78 File Offset: 0x00106E78
		private void SetUserEntryPointNoLock(MethodInfo entryPoint)
		{
			if (entryPoint == null)
			{
				throw new ArgumentNullException("entryPoint");
			}
			if (base.m_iSymWriter == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
			}
			if (entryPoint.DeclaringType != null)
			{
				if (entryPoint.Module.InternalModule != this.InternalModule)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Argument_NotInTheSameModuleBuilder"));
				}
			}
			else
			{
				MethodBuilder methodBuilder = entryPoint as MethodBuilder;
				if (methodBuilder != null && methodBuilder.GetModule().InternalModule != this.InternalModule)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Argument_NotInTheSameModuleBuilder"));
				}
			}
			SymbolToken symbolToken = new SymbolToken(this.GetMethodTokenInternal(entryPoint).Token);
			base.m_iSymWriter.SetUserEntryPoint(symbolToken);
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x00107F28 File Offset: 0x00106F28
		public void SetSymCustomAttribute(string name, byte[] data)
		{
			if (this.IsInternal)
			{
				this.DemandGrantedAssemblyPermission();
			}
			if (base.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (base.Assembly.m_assemblyData)
				{
					this.SetSymCustomAttributeNoLock(name, data);
					return;
				}
			}
			this.SetSymCustomAttributeNoLock(name, data);
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x00107F90 File Offset: 0x00106F90
		private void SetSymCustomAttributeNoLock(string name, byte[] data)
		{
			if (base.m_iSymWriter == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
			}
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x00107FAA File Offset: 0x00106FAA
		public bool IsTransient()
		{
			return base.m_moduleData.IsTransient();
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x00107FB7 File Offset: 0x00106FB7
		void _ModuleBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x00107FBE File Offset: 0x00106FBE
		void _ModuleBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x00107FC5 File Offset: 0x00106FC5
		void _ModuleBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B7B RID: 19323 RVA: 0x00107FCC File Offset: 0x00106FCC
		void _ModuleBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002636 RID: 9782
		internal ModuleBuilder m_internalModuleBuilder;

		// Token: 0x04002637 RID: 9783
		private AssemblyBuilder m_assemblyBuilder;

		// Token: 0x04002638 RID: 9784
		private static readonly Dictionary<ModuleBuilder, ModuleBuilder> s_moduleBuilders = new Dictionary<ModuleBuilder, ModuleBuilder>();
	}
}
