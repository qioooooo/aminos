using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection
{
	// Token: 0x0200031C RID: 796
	[ComDefaultInterface(typeof(_Module))]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	public class Module : _Module, ISerializable, ICustomAttributeProvider
	{
		// Token: 0x06001EC1 RID: 7873
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type _GetTypeInternal(string className, bool ignoreCase, bool throwOnError);

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0004DF8E File Offset: 0x0004CF8E
		internal Type GetTypeInternal(string className, bool ignoreCase, bool throwOnError)
		{
			return this.InternalModule._GetTypeInternal(className, ignoreCase, throwOnError);
		}

		// Token: 0x06001EC3 RID: 7875
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern IntPtr _GetHINSTANCE();

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0004DF9E File Offset: 0x0004CF9E
		internal IntPtr GetHINSTANCE()
		{
			return this.InternalModule._GetHINSTANCE();
		}

		// Token: 0x06001EC5 RID: 7877
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string _InternalGetName();

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0004DFAB File Offset: 0x0004CFAB
		private string InternalGetName()
		{
			return this.InternalModule._InternalGetName();
		}

		// Token: 0x06001EC7 RID: 7879
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string _InternalGetFullyQualifiedName();

		// Token: 0x06001EC8 RID: 7880 RVA: 0x0004DFB8 File Offset: 0x0004CFB8
		internal string InternalGetFullyQualifiedName()
		{
			return this.InternalModule._InternalGetFullyQualifiedName();
		}

		// Token: 0x06001EC9 RID: 7881
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type[] _GetTypesInternal(ref StackCrawlMark stackMark);

		// Token: 0x06001ECA RID: 7882 RVA: 0x0004DFC5 File Offset: 0x0004CFC5
		internal Type[] GetTypesInternal(ref StackCrawlMark stackMark)
		{
			return this.InternalModule._GetTypesInternal(ref stackMark);
		}

		// Token: 0x06001ECB RID: 7883
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Assembly _GetAssemblyInternal();

		// Token: 0x06001ECC RID: 7884 RVA: 0x0004DFD3 File Offset: 0x0004CFD3
		internal virtual Assembly GetAssemblyInternal()
		{
			return this.InternalModule._GetAssemblyInternal();
		}

		// Token: 0x06001ECD RID: 7885
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetTypeToken(string strFullName, Module refedModule, string strRefedModuleFileName, int tkResolution);

		// Token: 0x06001ECE RID: 7886 RVA: 0x0004DFE0 File Offset: 0x0004CFE0
		internal int InternalGetTypeToken(string strFullName, Module refedModule, string strRefedModuleFileName, int tkResolution)
		{
			return this.InternalModule._InternalGetTypeToken(strFullName, refedModule.InternalModule, strRefedModuleFileName, tkResolution);
		}

		// Token: 0x06001ECF RID: 7887
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type _InternalLoadInMemoryTypeByName(string className);

		// Token: 0x06001ED0 RID: 7888 RVA: 0x0004DFF7 File Offset: 0x0004CFF7
		internal Type InternalLoadInMemoryTypeByName(string className)
		{
			return this.InternalModule._InternalLoadInMemoryTypeByName(className);
		}

		// Token: 0x06001ED1 RID: 7889
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetMemberRef(Module refedModule, int tr, int defToken);

		// Token: 0x06001ED2 RID: 7890 RVA: 0x0004E005 File Offset: 0x0004D005
		internal int InternalGetMemberRef(Module refedModule, int tr, int defToken)
		{
			return this.InternalModule._InternalGetMemberRef(refedModule.InternalModule, tr, defToken);
		}

		// Token: 0x06001ED3 RID: 7891
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetMemberRefFromSignature(int tr, string methodName, byte[] signature, int length);

		// Token: 0x06001ED4 RID: 7892 RVA: 0x0004E01A File Offset: 0x0004D01A
		internal int InternalGetMemberRefFromSignature(int tr, string methodName, byte[] signature, int length)
		{
			return this.InternalModule._InternalGetMemberRefFromSignature(tr, methodName, signature, length);
		}

		// Token: 0x06001ED5 RID: 7893
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetMemberRefOfMethodInfo(int tr, IntPtr method);

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0004E02C File Offset: 0x0004D02C
		internal int InternalGetMemberRefOfMethodInfo(int tr, RuntimeMethodHandle method)
		{
			return this.InternalModule._InternalGetMemberRefOfMethodInfo(tr, method.Value);
		}

		// Token: 0x06001ED7 RID: 7895
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetMemberRefOfFieldInfo(int tkType, IntPtr interfaceHandle, int tkField);

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0004E041 File Offset: 0x0004D041
		internal int InternalGetMemberRefOfFieldInfo(int tkType, RuntimeTypeHandle declaringType, int tkField)
		{
			return this.InternalModule._InternalGetMemberRefOfFieldInfo(tkType, declaringType.Value, tkField);
		}

		// Token: 0x06001ED9 RID: 7897
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetTypeSpecTokenWithBytes(byte[] signature, int length);

		// Token: 0x06001EDA RID: 7898 RVA: 0x0004E057 File Offset: 0x0004D057
		internal int InternalGetTypeSpecTokenWithBytes(byte[] signature, int length)
		{
			return this.InternalModule._InternalGetTypeSpecTokenWithBytes(signature, length);
		}

		// Token: 0x06001EDB RID: 7899
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _nativeGetArrayMethodToken(int tkTypeSpec, string methodName, byte[] signature, int sigLength, int baseToken);

		// Token: 0x06001EDC RID: 7900 RVA: 0x0004E066 File Offset: 0x0004D066
		internal int nativeGetArrayMethodToken(int tkTypeSpec, string methodName, byte[] signature, int sigLength, int baseToken)
		{
			return this.InternalModule._nativeGetArrayMethodToken(tkTypeSpec, methodName, signature, sigLength, baseToken);
		}

		// Token: 0x06001EDD RID: 7901
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalSetFieldRVAContent(int fdToken, byte[] data, int length);

		// Token: 0x06001EDE RID: 7902 RVA: 0x0004E07A File Offset: 0x0004D07A
		internal void InternalSetFieldRVAContent(int fdToken, byte[] data, int length)
		{
			this.InternalModule._InternalSetFieldRVAContent(fdToken, data, length);
		}

		// Token: 0x06001EDF RID: 7903
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalGetStringConstant(string str);

		// Token: 0x06001EE0 RID: 7904 RVA: 0x0004E08A File Offset: 0x0004D08A
		internal int InternalGetStringConstant(string str)
		{
			return this.InternalModule._InternalGetStringConstant(str);
		}

		// Token: 0x06001EE1 RID: 7905
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalPreSavePEFile(int portableExecutableKind, int imageFileMachine);

		// Token: 0x06001EE2 RID: 7906 RVA: 0x0004E098 File Offset: 0x0004D098
		internal void InternalPreSavePEFile(int portableExecutableKind, int imageFileMachine)
		{
			this.InternalModule._InternalPreSavePEFile(portableExecutableKind, imageFileMachine);
		}

		// Token: 0x06001EE3 RID: 7907
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalSavePEFile(string fileName, int entryPoint, int isExe, bool isManifestFile);

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0004E0A7 File Offset: 0x0004D0A7
		internal void InternalSavePEFile(string fileName, MethodToken entryPoint, int isExe, bool isManifestFile)
		{
			this.InternalModule._InternalSavePEFile(fileName, entryPoint.Token, isExe, isManifestFile);
		}

		// Token: 0x06001EE5 RID: 7909
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalSetResourceCounts(int resCount);

		// Token: 0x06001EE6 RID: 7910 RVA: 0x0004E0BF File Offset: 0x0004D0BF
		internal void InternalSetResourceCounts(int resCount)
		{
			this.InternalModule._InternalSetResourceCounts(resCount);
		}

		// Token: 0x06001EE7 RID: 7911
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalAddResource(string strName, byte[] resBytes, int resByteCount, int tkFile, int attribute, int portableExecutableKind, int imageFileMachine);

		// Token: 0x06001EE8 RID: 7912 RVA: 0x0004E0CD File Offset: 0x0004D0CD
		internal void InternalAddResource(string strName, byte[] resBytes, int resByteCount, int tkFile, int attribute, int portableExecutableKind, int imageFileMachine)
		{
			this.InternalModule._InternalAddResource(strName, resBytes, resByteCount, tkFile, attribute, portableExecutableKind, imageFileMachine);
		}

		// Token: 0x06001EE9 RID: 7913
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalSetModuleProps(string strModuleName);

		// Token: 0x06001EEA RID: 7914 RVA: 0x0004E0E5 File Offset: 0x0004D0E5
		internal void InternalSetModuleProps(string strModuleName)
		{
			this.InternalModule._InternalSetModuleProps(strModuleName);
		}

		// Token: 0x06001EEB RID: 7915
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool _IsResourceInternal();

		// Token: 0x06001EEC RID: 7916 RVA: 0x0004E0F3 File Offset: 0x0004D0F3
		internal bool IsResourceInternal()
		{
			return this.InternalModule._IsResourceInternal();
		}

		// Token: 0x06001EED RID: 7917
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern X509Certificate _GetSignerCertificateInternal();

		// Token: 0x06001EEE RID: 7918 RVA: 0x0004E100 File Offset: 0x0004D100
		internal X509Certificate GetSignerCertificateInternal()
		{
			return this.InternalModule._GetSignerCertificateInternal();
		}

		// Token: 0x06001EEF RID: 7919
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalDefineNativeResourceFile(string strFilename, int portableExecutableKind, int ImageFileMachine);

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0004E10D File Offset: 0x0004D10D
		internal void InternalDefineNativeResourceFile(string strFilename, int portableExecutableKind, int ImageFileMachine)
		{
			this.InternalModule._InternalDefineNativeResourceFile(strFilename, portableExecutableKind, ImageFileMachine);
		}

		// Token: 0x06001EF1 RID: 7921
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void _InternalDefineNativeResourceBytes(byte[] resource, int portableExecutableKind, int imageFileMachine);

		// Token: 0x06001EF2 RID: 7922 RVA: 0x0004E11D File Offset: 0x0004D11D
		internal void InternalDefineNativeResourceBytes(byte[] resource, int portableExecutableKind, int imageFileMachine)
		{
			this.InternalModule._InternalDefineNativeResourceBytes(resource, portableExecutableKind, imageFileMachine);
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x0004E130 File Offset: 0x0004D130
		static Module()
		{
			__Filters _Filters = new __Filters();
			Module.FilterTypeName = new TypeFilter(_Filters.FilterTypeName);
			Module.FilterTypeNameIgnoreCase = new TypeFilter(_Filters.FilterTypeNameIgnoreCase);
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0004E167 File Offset: 0x0004D167
		public MethodBase ResolveMethod(int metadataToken)
		{
			return this.ResolveMethod(metadataToken, null, null);
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0004E174 File Offset: 0x0004D174
		private static RuntimeTypeHandle[] ConvertToTypeHandleArray(Type[] genericArguments)
		{
			if (genericArguments == null)
			{
				return null;
			}
			int num = genericArguments.Length;
			RuntimeTypeHandle[] array = new RuntimeTypeHandle[num];
			for (int i = 0; i < num; i++)
			{
				Type type = genericArguments[i];
				if (type == null)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidGenericInstArray"));
				}
				type = type.UnderlyingSystemType;
				if (type == null)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidGenericInstArray"));
				}
				if (!(type is RuntimeType))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidGenericInstArray"));
				}
				array[i] = type.GetTypeHandleInternal();
			}
			return array;
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0004E1FC File Offset: 0x0004D1FC
		public byte[] ResolveSignature(int metadataToken)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[] { metadataToken2, this }), new object[0]));
			}
			if (!metadataToken2.IsMemberRef && !metadataToken2.IsMethodDef && !metadataToken2.IsTypeSpec && !metadataToken2.IsSignature && !metadataToken2.IsFieldDef)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[] { metadataToken2, this }), new object[0]), "metadataToken");
			}
			ConstArray constArray;
			if (metadataToken2.IsMemberRef)
			{
				constArray = this.MetadataImport.GetMemberRefProps(metadataToken);
			}
			else
			{
				constArray = this.MetadataImport.GetSignatureFromToken(metadataToken);
			}
			byte[] array = new byte[constArray.Length];
			for (int i = 0; i < constArray.Length; i++)
			{
				array[i] = constArray[i];
			}
			return array;
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0004E32C File Offset: 0x0004D32C
		public unsafe MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[] { metadataToken2, this }), new object[0]));
			}
			RuntimeTypeHandle[] array = Module.ConvertToTypeHandleArray(genericTypeArguments);
			RuntimeTypeHandle[] array2 = Module.ConvertToTypeHandleArray(genericMethodArguments);
			MethodBase methodBase;
			try
			{
				if (!metadataToken2.IsMethodDef && !metadataToken2.IsMethodSpec)
				{
					if (!metadataToken2.IsMemberRef)
					{
						throw new ArgumentException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMethod", new object[] { metadataToken2, this }), new object[0]));
					}
					if (*(byte*)this.MetadataImport.GetMemberRefProps(metadataToken2).Signature.ToPointer() == 6)
					{
						throw new ArgumentException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMethod"), new object[] { metadataToken2, this }));
					}
				}
				RuntimeMethodHandle runtimeMethodHandle = this.GetModuleHandle().ResolveMethodHandle(metadataToken2, array, array2);
				Type type = runtimeMethodHandle.GetDeclaringType().GetRuntimeType();
				if (type.IsGenericType || type.IsArray)
				{
					MetadataToken metadataToken3 = new MetadataToken(this.MetadataImport.GetParentToken(metadataToken2));
					if (metadataToken2.IsMethodSpec)
					{
						metadataToken3 = new MetadataToken(this.MetadataImport.GetParentToken(metadataToken3));
					}
					type = this.ResolveType(metadataToken3, genericTypeArguments, genericMethodArguments);
				}
				methodBase = RuntimeType.GetMethodBase(type.GetTypeHandleInternal(), runtimeMethodHandle);
			}
			catch (BadImageFormatException ex)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadImageFormatExceptionResolve"), ex);
			}
			return methodBase;
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0004E53C File Offset: 0x0004D53C
		internal FieldInfo ResolveLiteralField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Argument_InvalidToken", new object[] { metadataToken2, this }), new object[0]));
			}
			string text = this.MetadataImport.GetName(metadataToken2).ToString();
			int parentToken = this.MetadataImport.GetParentToken(metadataToken2);
			Type type = this.ResolveType(parentToken, genericTypeArguments, genericMethodArguments);
			type.GetFields();
			FieldInfo field;
			try
			{
				field = type.GetField(text, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			catch
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveField"), new object[] { metadataToken2, this }), "metadataToken");
			}
			return field;
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x0004E64C File Offset: 0x0004D64C
		public FieldInfo ResolveField(int metadataToken)
		{
			return this.ResolveField(metadataToken, null, null);
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x0004E658 File Offset: 0x0004D658
		public unsafe FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[] { metadataToken2, this }), new object[0]));
			}
			RuntimeTypeHandle[] array = Module.ConvertToTypeHandleArray(genericTypeArguments);
			RuntimeTypeHandle[] array2 = Module.ConvertToTypeHandleArray(genericMethodArguments);
			FieldInfo fieldInfo;
			try
			{
				RuntimeFieldHandle runtimeFieldHandle = default(RuntimeFieldHandle);
				if (!metadataToken2.IsFieldDef)
				{
					if (!metadataToken2.IsMemberRef)
					{
						throw new ArgumentException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveField"), new object[] { metadataToken2, this }));
					}
					if (*(byte*)this.MetadataImport.GetMemberRefProps(metadataToken2).Signature.ToPointer() != 6)
					{
						throw new ArgumentException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveField"), new object[] { metadataToken2, this }));
					}
					runtimeFieldHandle = this.GetModuleHandle().ResolveFieldHandle(metadataToken2, array, array2);
				}
				runtimeFieldHandle = this.GetModuleHandle().ResolveFieldHandle(metadataToken, array, array2);
				Type type = runtimeFieldHandle.GetApproxDeclaringType().GetRuntimeType();
				if (type.IsGenericType || type.IsArray)
				{
					int parentToken = this.GetModuleHandle().GetMetadataImport().GetParentToken(metadataToken);
					type = this.ResolveType(parentToken, genericTypeArguments, genericMethodArguments);
				}
				fieldInfo = RuntimeType.GetFieldInfo(type.GetTypeHandleInternal(), runtimeFieldHandle);
			}
			catch (MissingFieldException)
			{
				fieldInfo = this.ResolveLiteralField(metadataToken2, genericTypeArguments, genericMethodArguments);
			}
			catch (BadImageFormatException ex)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadImageFormatExceptionResolve"), ex);
			}
			return fieldInfo;
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0004E86C File Offset: 0x0004D86C
		public Type ResolveType(int metadataToken)
		{
			return this.ResolveType(metadataToken, null, null);
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x0004E878 File Offset: 0x0004D878
		public Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (metadataToken2.IsGlobalTypeDefToken)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveModuleType"), new object[] { metadataToken2 }), "metadataToken");
			}
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[] { metadataToken2, this }), new object[0]));
			}
			if (!metadataToken2.IsTypeDef && !metadataToken2.IsTypeSpec && !metadataToken2.IsTypeRef)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveType"), new object[] { metadataToken2, this }), "metadataToken");
			}
			RuntimeTypeHandle[] array = Module.ConvertToTypeHandleArray(genericTypeArguments);
			RuntimeTypeHandle[] array2 = Module.ConvertToTypeHandleArray(genericMethodArguments);
			Type type;
			try
			{
				Type runtimeType = this.GetModuleHandle().ResolveTypeHandle(metadataToken, array, array2).GetRuntimeType();
				if (runtimeType == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveType"), new object[] { metadataToken2, this }), "metadataToken");
				}
				type = runtimeType;
			}
			catch (BadImageFormatException ex)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadImageFormatExceptionResolve"), ex);
			}
			return type;
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x0004EA04 File Offset: 0x0004DA04
		public MemberInfo ResolveMember(int metadataToken)
		{
			return this.ResolveMember(metadataToken, null, null);
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x0004EA10 File Offset: 0x0004DA10
		public unsafe MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (metadataToken2.IsProperty)
			{
				throw new ArgumentException(Environment.GetResourceString("InvalidOperation_PropertyInfoNotAvailable"));
			}
			if (metadataToken2.IsEvent)
			{
				throw new ArgumentException(Environment.GetResourceString("InvalidOperation_EventInfoNotAvailable"));
			}
			if (metadataToken2.IsMethodSpec || metadataToken2.IsMethodDef)
			{
				return this.ResolveMethod(metadataToken, genericTypeArguments, genericMethodArguments);
			}
			if (metadataToken2.IsFieldDef)
			{
				return this.ResolveField(metadataToken, genericTypeArguments, genericMethodArguments);
			}
			if (metadataToken2.IsTypeRef || metadataToken2.IsTypeDef || metadataToken2.IsTypeSpec)
			{
				return this.ResolveType(metadataToken, genericTypeArguments, genericMethodArguments);
			}
			if (!metadataToken2.IsMemberRef)
			{
				throw new ArgumentException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_ResolveMember", new object[] { metadataToken2, this }), new object[0]));
			}
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[] { metadataToken2, this }), new object[0]));
			}
			if (*(byte*)this.MetadataImport.GetMemberRefProps(metadataToken2).Signature.ToPointer() == 6)
			{
				return this.ResolveField(metadataToken2, genericTypeArguments, genericMethodArguments);
			}
			return this.ResolveMethod(metadataToken2, genericTypeArguments, genericMethodArguments);
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x0004EB8C File Offset: 0x0004DB8C
		public string ResolveString(int metadataToken)
		{
			MetadataToken metadataToken2 = new MetadataToken(metadataToken);
			if (!metadataToken2.IsString)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Argument_ResolveString"), new object[]
				{
					metadataToken,
					this.ToString()
				}));
			}
			if (!this.MetadataImport.IsValidToken(metadataToken2))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Argument_InvalidToken", new object[] { metadataToken2, this }), new object[0]));
			}
			string userString = this.MetadataImport.GetUserString(metadataToken);
			if (userString == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Argument_ResolveString"), new object[]
				{
					metadataToken,
					this.ToString()
				}));
			}
			return userString;
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0004EC80 File Offset: 0x0004DC80
		public void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
		{
			this.GetModuleHandle().GetPEKind(out peKind, out machine);
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001F01 RID: 7937 RVA: 0x0004ECA0 File Offset: 0x0004DCA0
		public int MDStreamVersion
		{
			get
			{
				return this.GetModuleHandle().MDStreamVersion;
			}
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0004ECBC File Offset: 0x0004DCBC
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			if (!(o is Module))
			{
				return false;
			}
			Module module = o as Module;
			module = module.InternalModule;
			return this.InternalModule == module;
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0004ECEF File Offset: 0x0004DCEF
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x0004ECF7 File Offset: 0x0004DCF7
		internal virtual Module InternalModule
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001F05 RID: 7941 RVA: 0x0004ECFA File Offset: 0x0004DCFA
		// (set) Token: 0x06001F06 RID: 7942 RVA: 0x0004ED07 File Offset: 0x0004DD07
		internal ArrayList m_TypeBuilderList
		{
			get
			{
				return this.InternalModule.m__TypeBuilderList;
			}
			set
			{
				this.InternalModule.m__TypeBuilderList = value;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001F07 RID: 7943 RVA: 0x0004ED15 File Offset: 0x0004DD15
		// (set) Token: 0x06001F08 RID: 7944 RVA: 0x0004ED22 File Offset: 0x0004DD22
		internal ISymbolWriter m_iSymWriter
		{
			get
			{
				return this.InternalModule.m__iSymWriter;
			}
			set
			{
				this.InternalModule.m__iSymWriter = value;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001F09 RID: 7945 RVA: 0x0004ED30 File Offset: 0x0004DD30
		// (set) Token: 0x06001F0A RID: 7946 RVA: 0x0004ED3D File Offset: 0x0004DD3D
		internal ModuleBuilderData m_moduleData
		{
			get
			{
				return this.InternalModule.m__moduleData;
			}
			set
			{
				this.InternalModule.m__moduleData = value;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001F0B RID: 7947 RVA: 0x0004ED4B File Offset: 0x0004DD4B
		// (set) Token: 0x06001F0C RID: 7948 RVA: 0x0004ED58 File Offset: 0x0004DD58
		private RuntimeType m_runtimeType
		{
			get
			{
				return this.InternalModule.m__runtimeType;
			}
			set
			{
				this.InternalModule.m__runtimeType = value;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001F0D RID: 7949 RVA: 0x0004ED66 File Offset: 0x0004DD66
		private IntPtr m_pRefClass
		{
			get
			{
				return this.InternalModule.m__pRefClass;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x0004ED73 File Offset: 0x0004DD73
		internal IntPtr m_pData
		{
			get
			{
				return this.InternalModule.m__pData;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001F0F RID: 7951 RVA: 0x0004ED80 File Offset: 0x0004DD80
		internal IntPtr m_pInternalSymWriter
		{
			get
			{
				return this.InternalModule.m__pInternalSymWriter;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001F10 RID: 7952 RVA: 0x0004ED8D File Offset: 0x0004DD8D
		private IntPtr m_pGlobals
		{
			get
			{
				return this.InternalModule.m__pGlobals;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001F11 RID: 7953 RVA: 0x0004ED9A File Offset: 0x0004DD9A
		private IntPtr m_pFields
		{
			get
			{
				return this.InternalModule.m__pFields;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x0004EDA7 File Offset: 0x0004DDA7
		// (set) Token: 0x06001F13 RID: 7955 RVA: 0x0004EDB4 File Offset: 0x0004DDB4
		internal MethodToken m_EntryPoint
		{
			get
			{
				return this.InternalModule.m__EntryPoint;
			}
			set
			{
				this.InternalModule.m__EntryPoint = value;
			}
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x0004EDC2 File Offset: 0x0004DDC2
		internal Module()
		{
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x0004EDCA File Offset: 0x0004DDCA
		private FieldInfo InternalGetField(string name, BindingFlags bindingAttr)
		{
			if (this.RuntimeType == null)
			{
				return null;
			}
			return this.RuntimeType.GetField(name, bindingAttr);
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0004EDE3 File Offset: 0x0004DDE3
		internal virtual bool IsDynamic()
		{
			return false;
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001F17 RID: 7959 RVA: 0x0004EDE8 File Offset: 0x0004DDE8
		internal RuntimeType RuntimeType
		{
			get
			{
				if (this.m_runtimeType == null)
				{
					this.m_runtimeType = this.GetModuleHandle().GetModuleTypeHandle().GetRuntimeType();
				}
				return this.m_runtimeType;
			}
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x0004EE1F File Offset: 0x0004DE1F
		protected virtual MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (this.RuntimeType == null)
			{
				return null;
			}
			if (types == null)
			{
				return this.RuntimeType.GetMethod(name, bindingAttr);
			}
			return this.RuntimeType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001F19 RID: 7961 RVA: 0x0004EE54 File Offset: 0x0004DE54
		internal MetadataImport MetadataImport
		{
			get
			{
				return this.ModuleHandle.GetMetadataImport();
			}
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x0004EE6F File Offset: 0x0004DE6F
		public virtual object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0004EE88 File Offset: 0x0004DE88
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

		// Token: 0x06001F1C RID: 7964 RVA: 0x0004EED0 File Offset: 0x0004DED0
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

		// Token: 0x06001F1D RID: 7965 RVA: 0x0004EF16 File Offset: 0x0004DF16
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, 5, this.ScopeName, this.GetAssemblyInternal());
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0004EF39 File Offset: 0x0004DF39
		[ComVisible(true)]
		public virtual Type GetType(string className, bool ignoreCase)
		{
			return this.GetType(className, false, ignoreCase);
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0004EF44 File Offset: 0x0004DF44
		[ComVisible(true)]
		public virtual Type GetType(string className)
		{
			return this.GetType(className, false, false);
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x0004EF4F File Offset: 0x0004DF4F
		[ComVisible(true)]
		public virtual Type GetType(string className, bool throwOnError, bool ignoreCase)
		{
			return this.GetTypeInternal(className, throwOnError, ignoreCase);
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001F21 RID: 7969 RVA: 0x0004EF5C File Offset: 0x0004DF5C
		public virtual string FullyQualifiedName
		{
			get
			{
				string text = this.InternalGetFullyQualifiedName();
				if (text != null)
				{
					bool flag = true;
					try
					{
						Path.GetFullPathInternal(text);
					}
					catch (ArgumentException)
					{
						flag = false;
					}
					if (flag)
					{
						new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
					}
				}
				return text;
			}
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0004EFA4 File Offset: 0x0004DFA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual Type[] FindTypes(TypeFilter filter, object filterCriteria)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			Type[] typesInternal = this.GetTypesInternal(ref stackCrawlMark);
			int num = 0;
			for (int i = 0; i < typesInternal.Length; i++)
			{
				if (filter != null && !filter(typesInternal[i], filterCriteria))
				{
					typesInternal[i] = null;
				}
				else
				{
					num++;
				}
			}
			if (num == typesInternal.Length)
			{
				return typesInternal;
			}
			Type[] array = new Type[num];
			num = 0;
			for (int j = 0; j < typesInternal.Length; j++)
			{
				if (typesInternal[j] != null)
				{
					array[num++] = typesInternal[j];
				}
			}
			return array;
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x0004F020 File Offset: 0x0004E020
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual Type[] GetTypes()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.GetTypesInternal(ref stackCrawlMark);
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001F24 RID: 7972 RVA: 0x0004F038 File Offset: 0x0004E038
		public Guid ModuleVersionId
		{
			get
			{
				Guid guid;
				this.MetadataImport.GetScopeProps(out guid);
				return guid;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001F25 RID: 7973 RVA: 0x0004F058 File Offset: 0x0004E058
		public int MetadataToken
		{
			get
			{
				return this.GetModuleHandle().GetToken();
			}
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x0004F073 File Offset: 0x0004E073
		public bool IsResource()
		{
			return this.IsResourceInternal();
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x0004F07B File Offset: 0x0004E07B
		public FieldInfo[] GetFields()
		{
			if (this.RuntimeType == null)
			{
				return new FieldInfo[0];
			}
			return this.RuntimeType.GetFields();
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0004F097 File Offset: 0x0004E097
		public FieldInfo[] GetFields(BindingFlags bindingFlags)
		{
			if (this.RuntimeType == null)
			{
				return new FieldInfo[0];
			}
			return this.RuntimeType.GetFields(bindingFlags);
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x0004F0B4 File Offset: 0x0004E0B4
		public FieldInfo GetField(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0004F0CD File Offset: 0x0004E0CD
		public FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.InternalGetField(name, bindingAttr);
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x0004F0E5 File Offset: 0x0004E0E5
		public MethodInfo[] GetMethods()
		{
			if (this.RuntimeType == null)
			{
				return new MethodInfo[0];
			}
			return this.RuntimeType.GetMethods();
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x0004F101 File Offset: 0x0004E101
		public MethodInfo[] GetMethods(BindingFlags bindingFlags)
		{
			if (this.RuntimeType == null)
			{
				return new MethodInfo[0];
			}
			return this.RuntimeType.GetMethods(bindingFlags);
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x0004F120 File Offset: 0x0004E120
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x0004F17C File Offset: 0x0004E17C
		public MethodInfo GetMethod(string name, Type[] types)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetMethodImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, types, null);
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x0004F1D0 File Offset: 0x0004E1D0
		public MethodInfo GetMethod(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.GetMethodImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, null, null);
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x0004F1ED File Offset: 0x0004E1ED
		public string ScopeName
		{
			get
			{
				return this.InternalGetName();
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001F31 RID: 7985 RVA: 0x0004F1F8 File Offset: 0x0004E1F8
		public string Name
		{
			get
			{
				string text = this.InternalGetFullyQualifiedName();
				int num = text.LastIndexOf('\\');
				if (num == -1)
				{
					return text;
				}
				return new string(text.ToCharArray(), num + 1, text.Length - num - 1);
			}
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x0004F233 File Offset: 0x0004E233
		public override string ToString()
		{
			return this.ScopeName;
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001F33 RID: 7987 RVA: 0x0004F23B File Offset: 0x0004E23B
		public Assembly Assembly
		{
			get
			{
				return this.GetAssemblyInternal();
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001F34 RID: 7988 RVA: 0x0004F243 File Offset: 0x0004E243
		public unsafe ModuleHandle ModuleHandle
		{
			get
			{
				return new ModuleHandle((void*)this.m_pData);
			}
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x0004F255 File Offset: 0x0004E255
		internal unsafe ModuleHandle GetModuleHandle()
		{
			return new ModuleHandle((void*)this.m_pData);
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x0004F267 File Offset: 0x0004E267
		public X509Certificate GetSignerCertificate()
		{
			return this.GetSignerCertificateInternal();
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x0004F26F File Offset: 0x0004E26F
		void _Module.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x0004F276 File Offset: 0x0004E276
		void _Module.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x0004F27D File Offset: 0x0004E27D
		void _Module.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x0004F284 File Offset: 0x0004E284
		void _Module.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000D26 RID: 3366
		private const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

		// Token: 0x04000D27 RID: 3367
		public static readonly TypeFilter FilterTypeName;

		// Token: 0x04000D28 RID: 3368
		public static readonly TypeFilter FilterTypeNameIgnoreCase;

		// Token: 0x04000D29 RID: 3369
		internal ArrayList m__TypeBuilderList;

		// Token: 0x04000D2A RID: 3370
		internal ISymbolWriter m__iSymWriter;

		// Token: 0x04000D2B RID: 3371
		internal ModuleBuilderData m__moduleData;

		// Token: 0x04000D2C RID: 3372
		private RuntimeType m__runtimeType;

		// Token: 0x04000D2D RID: 3373
		private IntPtr m__pRefClass;

		// Token: 0x04000D2E RID: 3374
		internal IntPtr m__pData;

		// Token: 0x04000D2F RID: 3375
		internal IntPtr m__pInternalSymWriter;

		// Token: 0x04000D30 RID: 3376
		private IntPtr m__pGlobals;

		// Token: 0x04000D31 RID: 3377
		private IntPtr m__pFields;

		// Token: 0x04000D32 RID: 3378
		internal MethodToken m__EntryPoint;
	}
}
