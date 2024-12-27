using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x02000835 RID: 2101
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_TypeBuilder))]
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class TypeBuilder : Type, _TypeBuilder
	{
		// Token: 0x06004C1F RID: 19487 RVA: 0x0010B7C0 File Offset: 0x0010A7C0
		public static MethodInfo GetMethod(Type type, MethodInfo method)
		{
			if (!(type is TypeBuilder) && !(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeTypeBuilder"));
			}
			if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedGenericMethodDefinition"), "method");
			}
			if (method.DeclaringType == null || !method.DeclaringType.IsGenericTypeDefinition)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MethodNeedGenericDeclaringType"), "method");
			}
			if (type.GetGenericTypeDefinition() != method.DeclaringType)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidMethodDeclaringType"), "type");
			}
			if (type.IsGenericTypeDefinition)
			{
				type = type.MakeGenericType(type.GetGenericArguments());
			}
			if (!(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "type");
			}
			return MethodOnTypeBuilderInstantiation.GetMethod(method, type as TypeBuilderInstantiation);
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x0010B8A0 File Offset: 0x0010A8A0
		public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor)
		{
			if (!(type is TypeBuilder) && !(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeTypeBuilder"));
			}
			if (!constructor.DeclaringType.IsGenericTypeDefinition)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ConstructorNeedGenericDeclaringType"), "constructor");
			}
			if (!(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "type");
			}
			if (type is TypeBuilder && type.IsGenericTypeDefinition)
			{
				type = type.MakeGenericType(type.GetGenericArguments());
			}
			if (type.GetGenericTypeDefinition() != constructor.DeclaringType)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidConstructorDeclaringType"), "type");
			}
			return ConstructorOnTypeBuilderInstantiation.GetConstructor(constructor, type as TypeBuilderInstantiation);
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x0010B95C File Offset: 0x0010A95C
		public static FieldInfo GetField(Type type, FieldInfo field)
		{
			if (!(type is TypeBuilder) && !(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustBeTypeBuilder"));
			}
			if (!field.DeclaringType.IsGenericTypeDefinition)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_FieldNeedGenericDeclaringType"), "field");
			}
			if (!(type is TypeBuilderInstantiation))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "type");
			}
			if (type is TypeBuilder && type.IsGenericTypeDefinition)
			{
				type = type.MakeGenericType(type.GetGenericArguments());
			}
			if (type.GetGenericTypeDefinition() != field.DeclaringType)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFieldDeclaringType"), "type");
			}
			return FieldOnTypeBuilderInstantiation.GetField(field, type as TypeBuilderInstantiation);
		}

		// Token: 0x06004C22 RID: 19490
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetParentType(int tdTypeDef, int tkParent, Module module);

		// Token: 0x06004C23 RID: 19491 RVA: 0x0010BA15 File Offset: 0x0010AA15
		private static void InternalSetParentType(int tdTypeDef, int tkParent, Module module)
		{
			TypeBuilder._InternalSetParentType(tdTypeDef, tkParent, module.InternalModule);
		}

		// Token: 0x06004C24 RID: 19492
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalAddInterfaceImpl(int tdTypeDef, int tkInterface, Module module);

		// Token: 0x06004C25 RID: 19493 RVA: 0x0010BA24 File Offset: 0x0010AA24
		private static void InternalAddInterfaceImpl(int tdTypeDef, int tkInterface, Module module)
		{
			TypeBuilder._InternalAddInterfaceImpl(tdTypeDef, tkInterface, module.InternalModule);
		}

		// Token: 0x06004C26 RID: 19494
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalDefineMethod(int handle, string name, byte[] signature, int sigLength, MethodAttributes attributes, Module module);

		// Token: 0x06004C27 RID: 19495 RVA: 0x0010BA33 File Offset: 0x0010AA33
		internal static int InternalDefineMethod(int handle, string name, byte[] signature, int sigLength, MethodAttributes attributes, Module module)
		{
			return TypeBuilder._InternalDefineMethod(handle, name, signature, sigLength, attributes, module.InternalModule);
		}

		// Token: 0x06004C28 RID: 19496
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalDefineMethodSpec(int handle, byte[] signature, int sigLength, Module module);

		// Token: 0x06004C29 RID: 19497 RVA: 0x0010BA47 File Offset: 0x0010AA47
		internal static int InternalDefineMethodSpec(int handle, byte[] signature, int sigLength, Module module)
		{
			return TypeBuilder._InternalDefineMethodSpec(handle, signature, sigLength, module.InternalModule);
		}

		// Token: 0x06004C2A RID: 19498
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalDefineField(int handle, string name, byte[] signature, int sigLength, FieldAttributes attributes, Module module);

		// Token: 0x06004C2B RID: 19499 RVA: 0x0010BA57 File Offset: 0x0010AA57
		internal static int InternalDefineField(int handle, string name, byte[] signature, int sigLength, FieldAttributes attributes, Module module)
		{
			return TypeBuilder._InternalDefineField(handle, name, signature, sigLength, attributes, module.InternalModule);
		}

		// Token: 0x06004C2C RID: 19500
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetMethodIL(int methodHandle, bool isInitLocals, byte[] body, byte[] LocalSig, int sigLength, int maxStackSize, int numExceptions, __ExceptionInstance[] exceptions, int[] tokenFixups, int[] rvaFixups, Module module);

		// Token: 0x06004C2D RID: 19501 RVA: 0x0010BA6C File Offset: 0x0010AA6C
		internal static void InternalSetMethodIL(int methodHandle, bool isInitLocals, byte[] body, byte[] LocalSig, int sigLength, int maxStackSize, int numExceptions, __ExceptionInstance[] exceptions, int[] tokenFixups, int[] rvaFixups, Module module)
		{
			TypeBuilder._InternalSetMethodIL(methodHandle, isInitLocals, body, LocalSig, sigLength, maxStackSize, numExceptions, exceptions, tokenFixups, rvaFixups, module.InternalModule);
		}

		// Token: 0x06004C2E RID: 19502
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalCreateCustomAttribute(int tkAssociate, int tkConstructor, byte[] attr, Module module, bool toDisk, bool updateCompilerFlags);

		// Token: 0x06004C2F RID: 19503 RVA: 0x0010BA95 File Offset: 0x0010AA95
		internal static void InternalCreateCustomAttribute(int tkAssociate, int tkConstructor, byte[] attr, Module module, bool toDisk, bool updateCompilerFlags)
		{
			TypeBuilder._InternalCreateCustomAttribute(tkAssociate, tkConstructor, attr, module.InternalModule, toDisk, updateCompilerFlags);
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x0010BAAC File Offset: 0x0010AAAC
		internal static void InternalCreateCustomAttribute(int tkAssociate, int tkConstructor, byte[] attr, Module module, bool toDisk)
		{
			byte[] array = null;
			if (attr != null)
			{
				array = new byte[attr.Length];
				Array.Copy(attr, array, attr.Length);
			}
			TypeBuilder.InternalCreateCustomAttribute(tkAssociate, tkConstructor, array, module.InternalModule, toDisk, false);
		}

		// Token: 0x06004C31 RID: 19505
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetPInvokeData(Module module, string DllName, string name, int token, int linkType, int linkFlags);

		// Token: 0x06004C32 RID: 19506 RVA: 0x0010BAE2 File Offset: 0x0010AAE2
		internal static void InternalSetPInvokeData(Module module, string DllName, string name, int token, int linkType, int linkFlags)
		{
			TypeBuilder._InternalSetPInvokeData(module.InternalModule, DllName, name, token, linkType, linkFlags);
		}

		// Token: 0x06004C33 RID: 19507
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalDefineProperty(Module module, int handle, string name, int attributes, byte[] signature, int sigLength, int notifyChanging, int notifyChanged);

		// Token: 0x06004C34 RID: 19508 RVA: 0x0010BAF6 File Offset: 0x0010AAF6
		internal static int InternalDefineProperty(Module module, int handle, string name, int attributes, byte[] signature, int sigLength, int notifyChanging, int notifyChanged)
		{
			return TypeBuilder._InternalDefineProperty(module.InternalModule, handle, name, attributes, signature, sigLength, notifyChanging, notifyChanged);
		}

		// Token: 0x06004C35 RID: 19509
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalDefineEvent(Module module, int handle, string name, int attributes, int tkEventType);

		// Token: 0x06004C36 RID: 19510 RVA: 0x0010BB0E File Offset: 0x0010AB0E
		internal static int InternalDefineEvent(Module module, int handle, string name, int attributes, int tkEventType)
		{
			return TypeBuilder._InternalDefineEvent(module.InternalModule, handle, name, attributes, tkEventType);
		}

		// Token: 0x06004C37 RID: 19511
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalDefineMethodSemantics(Module module, int tkAssociation, MethodSemanticsAttributes semantics, int tkMethod);

		// Token: 0x06004C38 RID: 19512 RVA: 0x0010BB20 File Offset: 0x0010AB20
		internal static void InternalDefineMethodSemantics(Module module, int tkAssociation, MethodSemanticsAttributes semantics, int tkMethod)
		{
			TypeBuilder._InternalDefineMethodSemantics(module.InternalModule, tkAssociation, semantics, tkMethod);
		}

		// Token: 0x06004C39 RID: 19513
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalDefineMethodImpl(Module module, int tkType, int tkBody, int tkDecl);

		// Token: 0x06004C3A RID: 19514 RVA: 0x0010BB30 File Offset: 0x0010AB30
		internal static void InternalDefineMethodImpl(Module module, int tkType, int tkBody, int tkDecl)
		{
			TypeBuilder._InternalDefineMethodImpl(module.InternalModule, tkType, tkBody, tkDecl);
		}

		// Token: 0x06004C3B RID: 19515
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetMethodImpl(Module module, int tkMethod, MethodImplAttributes MethodImplAttributes);

		// Token: 0x06004C3C RID: 19516 RVA: 0x0010BB40 File Offset: 0x0010AB40
		internal static void InternalSetMethodImpl(Module module, int tkMethod, MethodImplAttributes MethodImplAttributes)
		{
			TypeBuilder._InternalSetMethodImpl(module.InternalModule, tkMethod, MethodImplAttributes);
		}

		// Token: 0x06004C3D RID: 19517
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalSetParamInfo(Module module, int tkMethod, int iSequence, ParameterAttributes iParamAttributes, string strParamName);

		// Token: 0x06004C3E RID: 19518 RVA: 0x0010BB4F File Offset: 0x0010AB4F
		internal static int InternalSetParamInfo(Module module, int tkMethod, int iSequence, ParameterAttributes iParamAttributes, string strParamName)
		{
			return TypeBuilder._InternalSetParamInfo(module.InternalModule, tkMethod, iSequence, iParamAttributes, strParamName);
		}

		// Token: 0x06004C3F RID: 19519
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _InternalGetTokenFromSig(Module module, byte[] signature, int sigLength);

		// Token: 0x06004C40 RID: 19520 RVA: 0x0010BB61 File Offset: 0x0010AB61
		internal static int InternalGetTokenFromSig(Module module, byte[] signature, int sigLength)
		{
			return TypeBuilder._InternalGetTokenFromSig(module.InternalModule, signature, sigLength);
		}

		// Token: 0x06004C41 RID: 19521
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetFieldOffset(Module module, int fdToken, int iOffset);

		// Token: 0x06004C42 RID: 19522 RVA: 0x0010BB70 File Offset: 0x0010AB70
		internal static void InternalSetFieldOffset(Module module, int fdToken, int iOffset)
		{
			TypeBuilder._InternalSetFieldOffset(module.InternalModule, fdToken, iOffset);
		}

		// Token: 0x06004C43 RID: 19523
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetClassLayout(Module module, int tdToken, PackingSize iPackingSize, int iTypeSize);

		// Token: 0x06004C44 RID: 19524 RVA: 0x0010BB7F File Offset: 0x0010AB7F
		internal static void InternalSetClassLayout(Module module, int tdToken, PackingSize iPackingSize, int iTypeSize)
		{
			TypeBuilder._InternalSetClassLayout(module.InternalModule, tdToken, iPackingSize, iTypeSize);
		}

		// Token: 0x06004C45 RID: 19525
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetMarshalInfo(Module module, int tk, byte[] ubMarshal, int ubSize);

		// Token: 0x06004C46 RID: 19526 RVA: 0x0010BB8F File Offset: 0x0010AB8F
		internal static void InternalSetMarshalInfo(Module module, int tk, byte[] ubMarshal, int ubSize)
		{
			TypeBuilder._InternalSetMarshalInfo(module.InternalModule, tk, ubMarshal, ubSize);
		}

		// Token: 0x06004C47 RID: 19527
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalSetConstantValue(Module module, int tk, ref Variant var);

		// Token: 0x06004C48 RID: 19528 RVA: 0x0010BB9F File Offset: 0x0010AB9F
		private static void InternalSetConstantValue(Module module, int tk, ref Variant var)
		{
			TypeBuilder._InternalSetConstantValue(module.InternalModule, tk, ref var);
		}

		// Token: 0x06004C49 RID: 19529
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _InternalAddDeclarativeSecurity(Module module, int parent, SecurityAction action, byte[] blob);

		// Token: 0x06004C4A RID: 19530 RVA: 0x0010BBAE File Offset: 0x0010ABAE
		internal static void InternalAddDeclarativeSecurity(Module module, int parent, SecurityAction action, byte[] blob)
		{
			TypeBuilder._InternalAddDeclarativeSecurity(module.InternalModule, parent, action, blob);
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x0010BBC0 File Offset: 0x0010ABC0
		private static bool IsPublicComType(Type type)
		{
			Type declaringType = type.DeclaringType;
			if (declaringType != null)
			{
				if (TypeBuilder.IsPublicComType(declaringType) && (type.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic)
				{
					return true;
				}
			}
			else if ((type.Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.Public)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06004C4C RID: 19532 RVA: 0x0010BBFC File Offset: 0x0010ABFC
		internal static bool IsTypeEqual(Type t1, Type t2)
		{
			if (t1 == t2)
			{
				return true;
			}
			TypeBuilder typeBuilder = null;
			TypeBuilder typeBuilder2 = null;
			Type type;
			if (t1 is TypeBuilder)
			{
				typeBuilder = (TypeBuilder)t1;
				type = typeBuilder.m_runtimeType;
			}
			else
			{
				type = t1;
			}
			Type type2;
			if (t2 is TypeBuilder)
			{
				typeBuilder2 = (TypeBuilder)t2;
				type2 = typeBuilder2.m_runtimeType;
			}
			else
			{
				type2 = t2;
			}
			return (typeBuilder != null && typeBuilder2 != null && typeBuilder == typeBuilder2) || (type != null && type2 != null && type == type2);
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x0010BC64 File Offset: 0x0010AC64
		internal static void SetConstantValue(Module module, int tk, Type destType, object value)
		{
			if (value == null)
			{
				if (destType.IsValueType)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_ConstantNull"));
				}
			}
			else
			{
				Type type = value.GetType();
				if (!destType.IsEnum)
				{
					if (destType != type)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_ConstantDoesntMatch"));
					}
					switch (Type.GetTypeCode(type))
					{
					case TypeCode.Boolean:
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
					case TypeCode.String:
						break;
					default:
						if (type != typeof(DateTime))
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_ConstantNotSupported"));
						}
						break;
					}
				}
				else if (destType.UnderlyingSystemType != type)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_ConstantDoesntMatch"));
				}
			}
			Variant variant = new Variant(value);
			TypeBuilder.InternalSetConstantValue(module.InternalModule, tk, ref variant);
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x0010BD58 File Offset: 0x0010AD58
		private TypeBuilder(TypeBuilder genTypeDef, GenericTypeParameterBuilder[] inst)
		{
			this.m_genTypeDef = genTypeDef;
			this.m_DeclaringType = genTypeDef.m_DeclaringType;
			this.m_typeParent = genTypeDef.m_typeParent;
			this.m_runtimeType = genTypeDef.m_runtimeType;
			this.m_tdType = genTypeDef.m_tdType;
			this.m_strName = genTypeDef.m_strName;
			this.m_bIsGenParam = false;
			this.m_bIsGenTypeDef = false;
			this.m_module = genTypeDef.m_module;
			this.m_inst = inst;
			this.m_hasBeenCreated = true;
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x0010BDD6 File Offset: 0x0010ADD6
		internal TypeBuilder(string szName, int genParamPos, MethodBuilder declMeth)
		{
			this.m_declMeth = declMeth;
			this.m_DeclaringType = (TypeBuilder)this.m_declMeth.DeclaringType;
			this.m_module = (ModuleBuilder)declMeth.Module;
			this.InitAsGenericParam(szName, genParamPos);
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x0010BE14 File Offset: 0x0010AE14
		private TypeBuilder(string szName, int genParamPos, TypeBuilder declType)
		{
			this.m_DeclaringType = declType;
			this.m_module = (ModuleBuilder)declType.Module;
			this.InitAsGenericParam(szName, genParamPos);
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x0010BE3C File Offset: 0x0010AE3C
		private void InitAsGenericParam(string szName, int genParamPos)
		{
			this.m_strName = szName;
			this.m_genParamPos = genParamPos;
			this.m_bIsGenParam = true;
			this.m_bIsGenTypeDef = false;
			this.m_typeInterfaces = new Type[0];
		}

		// Token: 0x06004C52 RID: 19538 RVA: 0x0010BE68 File Offset: 0x0010AE68
		internal TypeBuilder(string name, TypeAttributes attr, Type parent, Module module, PackingSize iPackingSize, int iTypeSize, TypeBuilder enclosingType)
		{
			this.Init(name, attr, parent, null, module, iPackingSize, iTypeSize, enclosingType);
		}

		// Token: 0x06004C53 RID: 19539 RVA: 0x0010BE90 File Offset: 0x0010AE90
		internal TypeBuilder(string name, TypeAttributes attr, Type parent, Type[] interfaces, Module module, PackingSize iPackingSize, TypeBuilder enclosingType)
		{
			this.Init(name, attr, parent, interfaces, module, iPackingSize, 0, enclosingType);
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x0010BEB5 File Offset: 0x0010AEB5
		internal TypeBuilder(ModuleBuilder module)
		{
			this.m_tdType = new TypeToken(33554432);
			this.m_isHiddenGlobalType = true;
			this.m_module = module;
			this.m_listMethods = new ArrayList();
		}

		// Token: 0x06004C55 RID: 19541 RVA: 0x0010BEE8 File Offset: 0x0010AEE8
		private void Init(string fullname, TypeAttributes attr, Type parent, Type[] interfaces, Module module, PackingSize iPackingSize, int iTypeSize, TypeBuilder enclosingType)
		{
			this.m_bIsGenTypeDef = false;
			int[] array = null;
			this.m_bIsGenParam = false;
			this.m_hasBeenCreated = false;
			this.m_runtimeType = null;
			this.m_isHiddenGlobalType = false;
			this.m_isHiddenType = false;
			this.m_module = (ModuleBuilder)module;
			this.m_DeclaringType = enclosingType;
			Assembly assembly = this.m_module.Assembly;
			this.m_underlyingSystemType = null;
			if (fullname == null)
			{
				throw new ArgumentNullException("fullname");
			}
			if (fullname.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "fullname");
			}
			if (fullname[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalName"), "fullname");
			}
			if (fullname.Length > 1023)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_TypeNameTooLong"), "fullname");
			}
			assembly.m_assemblyData.CheckTypeNameConflict(fullname, enclosingType);
			if (enclosingType != null && ((attr & TypeAttributes.VisibilityMask) == TypeAttributes.Public || (attr & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadNestedTypeFlags"), "attr");
			}
			if (interfaces != null)
			{
				for (int i = 0; i < interfaces.Length; i++)
				{
					if (interfaces[i] == null)
					{
						throw new ArgumentNullException("interfaces");
					}
				}
				array = new int[interfaces.Length];
				for (int i = 0; i < interfaces.Length; i++)
				{
					array[i] = this.m_module.GetTypeTokenInternal(interfaces[i]).Token;
				}
			}
			int num = fullname.LastIndexOf('.');
			if (num == -1 || num == 0)
			{
				this.m_strNameSpace = string.Empty;
				this.m_strName = fullname;
			}
			else
			{
				this.m_strNameSpace = fullname.Substring(0, num);
				this.m_strName = fullname.Substring(num + 1);
			}
			this.VerifyTypeAttributes(attr);
			this.m_iAttr = attr;
			this.SetParent(parent);
			this.m_listMethods = new ArrayList();
			this.SetInterfaces(interfaces);
			this.m_constructorCount = 0;
			int num2 = 0;
			if (this.m_typeParent != null)
			{
				num2 = this.m_module.GetTypeTokenInternal(this.m_typeParent).Token;
			}
			int num3 = 0;
			if (enclosingType != null)
			{
				num3 = enclosingType.m_tdType.Token;
			}
			this.m_tdType = new TypeToken(this.InternalDefineClass(fullname, num2, array, this.m_iAttr, this.m_module, Guid.Empty, num3, 0));
			this.m_iPackingSize = iPackingSize;
			this.m_iTypeSize = iTypeSize;
			if (this.m_iPackingSize != PackingSize.Unspecified || this.m_iTypeSize != 0)
			{
				TypeBuilder.InternalSetClassLayout(this.Module, this.m_tdType.Token, this.m_iPackingSize, this.m_iTypeSize);
			}
			if (TypeBuilder.IsPublicComType(this) && assembly is AssemblyBuilder)
			{
				AssemblyBuilder assemblyBuilder = (AssemblyBuilder)assembly;
				if (assemblyBuilder.IsPersistable() && !this.m_module.IsTransient())
				{
					assemblyBuilder.m_assemblyData.AddPublicComType(this);
				}
			}
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x0010C190 File Offset: 0x0010B190
		private MethodBuilder DefinePInvokeMethodHelper(string name, string dllName, string importName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.CheckContext(new Type[] { returnType });
			this.CheckContext(new Type[][] { returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes });
			this.CheckContext(parameterTypeRequiredCustomModifiers);
			this.CheckContext(parameterTypeOptionalCustomModifiers);
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefinePInvokeMethodHelperNoLock(name, dllName, importName, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, nativeCallConv, nativeCharSet);
				}
			}
			return this.DefinePInvokeMethodHelperNoLock(name, dllName, importName, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x0010C264 File Offset: 0x0010B264
		private MethodBuilder DefinePInvokeMethodHelperNoLock(string name, string dllName, string importName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.ThrowIfCreated();
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (dllName == null)
			{
				throw new ArgumentNullException("dllName");
			}
			if (dllName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "dllName");
			}
			if (importName == null)
			{
				throw new ArgumentNullException("importName");
			}
			if (importName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "importName");
			}
			if ((this.m_iAttr & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadPInvokeOnInterface"));
			}
			if ((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadPInvokeMethod"));
			}
			attributes |= MethodAttributes.PinvokeImpl;
			MethodBuilder methodBuilder = new MethodBuilder(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, this.m_module, this, false);
			int num;
			methodBuilder.GetMethodSignature().InternalGetSignature(out num);
			if (this.m_listMethods.Contains(methodBuilder))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MethodRedefined"));
			}
			this.m_listMethods.Add(methodBuilder);
			MethodToken token = methodBuilder.GetToken();
			int num2 = 0;
			switch (nativeCallConv)
			{
			case CallingConvention.Winapi:
				num2 = 256;
				break;
			case CallingConvention.Cdecl:
				num2 = 512;
				break;
			case CallingConvention.StdCall:
				num2 = 768;
				break;
			case CallingConvention.ThisCall:
				num2 = 1024;
				break;
			case CallingConvention.FastCall:
				num2 = 1280;
				break;
			}
			switch (nativeCharSet)
			{
			case CharSet.None:
				num2 = num2;
				break;
			case CharSet.Ansi:
				num2 |= 2;
				break;
			case CharSet.Unicode:
				num2 |= 4;
				break;
			case CharSet.Auto:
				num2 |= 6;
				break;
			}
			TypeBuilder.InternalSetPInvokeData(this.m_module, dllName, importName, token.Token, 0, num2);
			methodBuilder.SetToken(token);
			return methodBuilder;
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x0010C434 File Offset: 0x0010B434
		private FieldBuilder DefineDataHelper(string name, byte[] data, int size, FieldAttributes attributes)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (size <= 0 || size >= 4128768)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadSizeForData"));
			}
			this.ThrowIfCreated();
			string text = "$ArrayType$" + size;
			Type type = this.m_module.FindTypeBuilderWithName(text, false);
			TypeBuilder typeBuilder = type as TypeBuilder;
			if (typeBuilder == null)
			{
				TypeAttributes typeAttributes = TypeAttributes.Public | TypeAttributes.ExplicitLayout | TypeAttributes.Sealed;
				typeBuilder = this.m_module.DefineType(text, typeAttributes, typeof(ValueType), PackingSize.Size1, size);
				typeBuilder.m_isHiddenType = true;
				typeBuilder.CreateType();
			}
			FieldBuilder fieldBuilder = this.DefineField(name, typeBuilder, attributes | FieldAttributes.Static);
			fieldBuilder.SetData(data, size);
			return fieldBuilder;
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x0010C500 File Offset: 0x0010B500
		private void VerifyTypeAttributes(TypeAttributes attr)
		{
			if (this.DeclaringType == null)
			{
				if ((attr & TypeAttributes.VisibilityMask) != TypeAttributes.NotPublic && (attr & TypeAttributes.VisibilityMask) != TypeAttributes.Public)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrNestedVisibilityOnNonNestedType"));
				}
			}
			else if ((attr & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic || (attr & TypeAttributes.VisibilityMask) == TypeAttributes.Public)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrNonNestedVisibilityNestedType"));
			}
			if ((attr & TypeAttributes.LayoutMask) != TypeAttributes.NotPublic && (attr & TypeAttributes.LayoutMask) != TypeAttributes.SequentialLayout && (attr & TypeAttributes.LayoutMask) != TypeAttributes.ExplicitLayout)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrInvalidLayout"));
			}
			if ((attr & TypeAttributes.ReservedMask) != TypeAttributes.NotPublic)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrReservedBitsSet"));
			}
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x0010C589 File Offset: 0x0010B589
		public bool IsCreated()
		{
			return this.m_hasBeenCreated;
		}

		// Token: 0x06004C5B RID: 19547
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalDefineClass(string fullname, int tkParent, int[] interfaceTokens, TypeAttributes attr, Module module, Guid guid, int tkEnclosingType, int tkTypeDef);

		// Token: 0x06004C5C RID: 19548 RVA: 0x0010C594 File Offset: 0x0010B594
		private int InternalDefineClass(string fullname, int tkParent, int[] interfaceTokens, TypeAttributes attr, Module module, Guid guid, int tkEnclosingType, int tkTypeDef)
		{
			return this._InternalDefineClass(fullname, tkParent, interfaceTokens, attr, module.InternalModule, guid, tkEnclosingType, tkTypeDef);
		}

		// Token: 0x06004C5D RID: 19549
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int _InternalDefineGenParam(string name, int tkParent, int position, int attributes, int[] interfaceTokens, Module module, int tkTypeDef);

		// Token: 0x06004C5E RID: 19550 RVA: 0x0010C5B9 File Offset: 0x0010B5B9
		private int InternalDefineGenParam(string name, int tkParent, int position, int attributes, int[] interfaceTokens, Module module, int tkTypeDef)
		{
			return this._InternalDefineGenParam(name, tkParent, position, attributes, interfaceTokens, module.InternalModule, tkTypeDef);
		}

		// Token: 0x06004C5F RID: 19551
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Type _TermCreateClass(int handle, Module module);

		// Token: 0x06004C60 RID: 19552 RVA: 0x0010C5D1 File Offset: 0x0010B5D1
		private Type TermCreateClass(int handle, Module module)
		{
			return this._TermCreateClass(handle, module.InternalModule);
		}

		// Token: 0x06004C61 RID: 19553 RVA: 0x0010C5E0 File Offset: 0x0010B5E0
		internal void ThrowIfCreated()
		{
			if (this.IsCreated())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TypeHasBeenCreated"));
			}
		}

		// Token: 0x06004C62 RID: 19554 RVA: 0x0010C5FA File Offset: 0x0010B5FA
		public override string ToString()
		{
			return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.ToString);
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06004C63 RID: 19555 RVA: 0x0010C603 File Offset: 0x0010B603
		public override Type DeclaringType
		{
			get
			{
				return this.m_DeclaringType;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06004C64 RID: 19556 RVA: 0x0010C60B File Offset: 0x0010B60B
		public override Type ReflectedType
		{
			get
			{
				return this.m_DeclaringType;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x06004C65 RID: 19557 RVA: 0x0010C613 File Offset: 0x0010B613
		public override string Name
		{
			get
			{
				return this.m_strName;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x06004C66 RID: 19558 RVA: 0x0010C61B File Offset: 0x0010B61B
		public override Module Module
		{
			get
			{
				return this.m_module;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x06004C67 RID: 19559 RVA: 0x0010C623 File Offset: 0x0010B623
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_tdType.Token;
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06004C68 RID: 19560 RVA: 0x0010C630 File Offset: 0x0010B630
		public override Guid GUID
		{
			get
			{
				if (this.m_runtimeType == null)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
				}
				return this.m_runtimeType.GUID;
			}
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x0010C658 File Offset: 0x0010B658
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06004C6A RID: 19562 RVA: 0x0010C695 File Offset: 0x0010B695
		public override Assembly Assembly
		{
			get
			{
				return this.m_module.Assembly;
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06004C6B RID: 19563 RVA: 0x0010C6A2 File Offset: 0x0010B6A2
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06004C6C RID: 19564 RVA: 0x0010C6B3 File Offset: 0x0010B6B3
		public override string FullName
		{
			get
			{
				if (this.m_strFullQualName == null)
				{
					this.m_strFullQualName = TypeNameBuilder.ToString(this, TypeNameBuilder.Format.FullName);
				}
				return this.m_strFullQualName;
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06004C6D RID: 19565 RVA: 0x0010C6D0 File Offset: 0x0010B6D0
		public override string Namespace
		{
			get
			{
				return this.m_strNameSpace;
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06004C6E RID: 19566 RVA: 0x0010C6D8 File Offset: 0x0010B6D8
		public override string AssemblyQualifiedName
		{
			get
			{
				return TypeNameBuilder.ToString(this, TypeNameBuilder.Format.AssemblyQualifiedName);
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06004C6F RID: 19567 RVA: 0x0010C6E1 File Offset: 0x0010B6E1
		public override Type BaseType
		{
			get
			{
				return this.m_typeParent;
			}
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x0010C6E9 File Offset: 0x0010B6E9
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x0010C715 File Offset: 0x0010B715
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetConstructors(bindingAttr);
		}

		// Token: 0x06004C72 RID: 19570 RVA: 0x0010C73B File Offset: 0x0010B73B
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			if (types == null)
			{
				return this.m_runtimeType.GetMethod(name, bindingAttr);
			}
			return this.m_runtimeType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004C73 RID: 19571 RVA: 0x0010C77B File Offset: 0x0010B77B
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetMethods(bindingAttr);
		}

		// Token: 0x06004C74 RID: 19572 RVA: 0x0010C7A1 File Offset: 0x0010B7A1
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetField(name, bindingAttr);
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x0010C7C8 File Offset: 0x0010B7C8
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetFields(bindingAttr);
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x0010C7EE File Offset: 0x0010B7EE
		public override Type GetInterface(string name, bool ignoreCase)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetInterface(name, ignoreCase);
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x0010C818 File Offset: 0x0010B818
		public override Type[] GetInterfaces()
		{
			if (this.m_runtimeType != null)
			{
				return this.m_runtimeType.GetInterfaces();
			}
			if (this.m_typeInterfaces == null)
			{
				return new Type[0];
			}
			Type[] array = new Type[this.m_typeInterfaces.Length];
			Array.Copy(this.m_typeInterfaces, array, this.m_typeInterfaces.Length);
			return array;
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x0010C86B File Offset: 0x0010B86B
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetEvent(name, bindingAttr);
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x0010C892 File Offset: 0x0010B892
		public override EventInfo[] GetEvents()
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetEvents();
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x0010C8B7 File Offset: 0x0010B8B7
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x0010C8C8 File Offset: 0x0010B8C8
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetProperties(bindingAttr);
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x0010C8EE File Offset: 0x0010B8EE
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetNestedTypes(bindingAttr);
		}

		// Token: 0x06004C7D RID: 19581 RVA: 0x0010C914 File Offset: 0x0010B914
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetNestedType(name, bindingAttr);
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x0010C93B File Offset: 0x0010B93B
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetMember(name, type, bindingAttr);
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x0010C963 File Offset: 0x0010B963
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetInterfaceMap(interfaceType);
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x0010C989 File Offset: 0x0010B989
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetEvents(bindingAttr);
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x0010C9AF File Offset: 0x0010B9AF
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return this.m_runtimeType.GetMembers(bindingAttr);
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x0010C9D8 File Offset: 0x0010B9D8
		public override bool IsAssignableFrom(Type c)
		{
			if (TypeBuilder.IsTypeEqual(c, this))
			{
				return true;
			}
			RuntimeType runtimeType = c as RuntimeType;
			TypeBuilder typeBuilder = c as TypeBuilder;
			if (typeBuilder != null && typeBuilder.m_runtimeType != null)
			{
				runtimeType = typeBuilder.m_runtimeType;
			}
			if (runtimeType != null)
			{
				return this.m_runtimeType != null && this.m_runtimeType.IsAssignableFrom(runtimeType);
			}
			if (typeBuilder == null)
			{
				return false;
			}
			if (typeBuilder.IsSubclassOf(this))
			{
				return true;
			}
			if (!base.IsInterface)
			{
				return false;
			}
			Type[] interfaces = typeBuilder.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				if (TypeBuilder.IsTypeEqual(interfaces[i], this))
				{
					return true;
				}
				if (interfaces[i].IsSubclassOf(this))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x0010CA74 File Offset: 0x0010BA74
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.m_iAttr;
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x0010CA7C File Offset: 0x0010BA7C
		protected override bool IsArrayImpl()
		{
			return false;
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x0010CA7F File Offset: 0x0010BA7F
		protected override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x0010CA82 File Offset: 0x0010BA82
		protected override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x06004C87 RID: 19591 RVA: 0x0010CA85 File Offset: 0x0010BA85
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x06004C88 RID: 19592 RVA: 0x0010CA88 File Offset: 0x0010BA88
		protected override bool IsCOMObjectImpl()
		{
			return (this.GetAttributeFlagsImpl() & TypeAttributes.Import) != TypeAttributes.NotPublic;
		}

		// Token: 0x06004C89 RID: 19593 RVA: 0x0010CA9B File Offset: 0x0010BA9B
		public override Type GetElementType()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x0010CAAC File Offset: 0x0010BAAC
		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x0010CAB0 File Offset: 0x0010BAB0
		[ComVisible(true)]
		public override bool IsSubclassOf(Type c)
		{
			if (TypeBuilder.IsTypeEqual(this, c))
			{
				return false;
			}
			for (Type type = this.BaseType; type != null; type = type.BaseType)
			{
				if (TypeBuilder.IsTypeEqual(type, c))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06004C8C RID: 19596 RVA: 0x0010CAE9 File Offset: 0x0010BAE9
		public override Type UnderlyingSystemType
		{
			get
			{
				if (this.m_runtimeType != null)
				{
					return this.m_runtimeType.UnderlyingSystemType;
				}
				if (!base.IsEnum)
				{
					return this;
				}
				if (this.m_underlyingSystemType == null)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoUnderlyingTypeOnEnum"));
				}
				return this.m_underlyingSystemType;
			}
		}

		// Token: 0x06004C8D RID: 19597 RVA: 0x0010CB27 File Offset: 0x0010BB27
		public override Type MakePointerType()
		{
			return SymbolType.FormCompoundType("*".ToCharArray(), this, 0);
		}

		// Token: 0x06004C8E RID: 19598 RVA: 0x0010CB3A File Offset: 0x0010BB3A
		public override Type MakeByRefType()
		{
			return SymbolType.FormCompoundType("&".ToCharArray(), this, 0);
		}

		// Token: 0x06004C8F RID: 19599 RVA: 0x0010CB4D File Offset: 0x0010BB4D
		public override Type MakeArrayType()
		{
			return SymbolType.FormCompoundType("[]".ToCharArray(), this, 0);
		}

		// Token: 0x06004C90 RID: 19600 RVA: 0x0010CB60 File Offset: 0x0010BB60
		public override Type MakeArrayType(int rank)
		{
			if (rank <= 0)
			{
				throw new IndexOutOfRangeException();
			}
			string text = "";
			if (rank == 1)
			{
				text = "*";
			}
			else
			{
				for (int i = 1; i < rank; i++)
				{
					text += ",";
				}
			}
			string text2 = string.Format(CultureInfo.InvariantCulture, "[{0}]", new object[] { text });
			return SymbolType.FormCompoundType(text2.ToCharArray(), this, 0);
		}

		// Token: 0x06004C91 RID: 19601 RVA: 0x0010CBCA File Offset: 0x0010BBCA
		public override object[] GetCustomAttributes(bool inherit)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			return CustomAttribute.GetCustomAttributes(this.m_runtimeType, typeof(object) as RuntimeType, inherit);
		}

		// Token: 0x06004C92 RID: 19602 RVA: 0x0010CC00 File Offset: 0x0010BC00
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this.m_runtimeType, runtimeType, inherit);
		}

		// Token: 0x06004C93 RID: 19603 RVA: 0x0010CC64 File Offset: 0x0010BC64
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			if (this.m_runtimeType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "caType");
			}
			return CustomAttribute.IsDefined(this.m_runtimeType, runtimeType, inherit);
		}

		// Token: 0x06004C94 RID: 19604 RVA: 0x0010CCBA File Offset: 0x0010BCBA
		internal void ThrowIfGeneric()
		{
			if (this.IsGenericType && !this.IsGenericTypeDefinition)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06004C95 RID: 19605 RVA: 0x0010CCD2 File Offset: 0x0010BCD2
		public override GenericParameterAttributes GenericParameterAttributes
		{
			get
			{
				return this.m_genParamAttributes;
			}
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x0010CCDA File Offset: 0x0010BCDA
		internal void SetInterfaces(params Type[] interfaces)
		{
			this.ThrowIfCreated();
			if (interfaces == null)
			{
				this.m_typeInterfaces = new Type[0];
				return;
			}
			this.m_typeInterfaces = new Type[interfaces.Length];
			Array.Copy(interfaces, this.m_typeInterfaces, interfaces.Length);
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x0010CD10 File Offset: 0x0010BD10
		public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
		{
			if (this.m_inst != null)
			{
				throw new InvalidOperationException();
			}
			if (names == null)
			{
				throw new ArgumentNullException("names");
			}
			for (int i = 0; i < names.Length; i++)
			{
				if (names[i] == null)
				{
					throw new ArgumentNullException("names");
				}
			}
			if (names.Length == 0)
			{
				throw new ArgumentException();
			}
			this.m_bIsGenTypeDef = true;
			this.m_inst = new GenericTypeParameterBuilder[names.Length];
			for (int j = 0; j < names.Length; j++)
			{
				this.m_inst[j] = new GenericTypeParameterBuilder(new TypeBuilder(names[j], j, this));
			}
			return this.m_inst;
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x0010CDA2 File Offset: 0x0010BDA2
		public override Type MakeGenericType(params Type[] typeArguments)
		{
			this.CheckContext(typeArguments);
			if (!this.IsGenericTypeDefinition)
			{
				throw new InvalidOperationException();
			}
			return new TypeBuilderInstantiation(this, typeArguments);
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x0010CDC0 File Offset: 0x0010BDC0
		public override Type[] GetGenericArguments()
		{
			return this.m_inst;
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06004C9A RID: 19610 RVA: 0x0010CDC8 File Offset: 0x0010BDC8
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return this.m_bIsGenTypeDef;
			}
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06004C9B RID: 19611 RVA: 0x0010CDD0 File Offset: 0x0010BDD0
		public override bool IsGenericType
		{
			get
			{
				return this.m_inst != null;
			}
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06004C9C RID: 19612 RVA: 0x0010CDDE File Offset: 0x0010BDDE
		public override bool IsGenericParameter
		{
			get
			{
				return this.m_bIsGenParam;
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06004C9D RID: 19613 RVA: 0x0010CDE6 File Offset: 0x0010BDE6
		public override int GenericParameterPosition
		{
			get
			{
				return this.m_genParamPos;
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06004C9E RID: 19614 RVA: 0x0010CDEE File Offset: 0x0010BDEE
		public override MethodBase DeclaringMethod
		{
			get
			{
				return this.m_declMeth;
			}
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x0010CDF6 File Offset: 0x0010BDF6
		public override Type GetGenericTypeDefinition()
		{
			if (this.IsGenericTypeDefinition)
			{
				return this;
			}
			if (this.m_genTypeDef == null)
			{
				throw new InvalidOperationException();
			}
			return this.m_genTypeDef;
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x0010CE18 File Offset: 0x0010BE18
		public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					this.DefineMethodOverrideNoLock(methodInfoBody, methodInfoDeclaration);
					return;
				}
			}
			this.DefineMethodOverrideNoLock(methodInfoBody, methodInfoDeclaration);
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x0010CE7C File Offset: 0x0010BE7C
		private void DefineMethodOverrideNoLock(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
		{
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			if (methodInfoBody == null)
			{
				throw new ArgumentNullException("methodInfoBody");
			}
			if (methodInfoDeclaration == null)
			{
				throw new ArgumentNullException("methodInfoDeclaration");
			}
			if (methodInfoBody.DeclaringType != this)
			{
				throw new ArgumentException(Environment.GetResourceString("ArgumentException_BadMethodImplBody"));
			}
			MethodToken methodTokenInternal = this.m_module.GetMethodTokenInternal(methodInfoBody);
			MethodToken methodTokenInternal2 = this.m_module.GetMethodTokenInternal(methodInfoDeclaration);
			TypeBuilder.InternalDefineMethodImpl(this.m_module, this.m_tdType.Token, methodTokenInternal.Token, methodTokenInternal2.Token);
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x0010CF08 File Offset: 0x0010BF08
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return this.DefineMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x0010CF16 File Offset: 0x0010BF16
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes)
		{
			return this.DefineMethod(name, attributes, CallingConventions.Standard, null, null);
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x0010CF23 File Offset: 0x0010BF23
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention)
		{
			return this.DefineMethod(name, attributes, callingConvention, null, null);
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x0010CF30 File Offset: 0x0010BF30
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return this.DefineMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x0010CF50 File Offset: 0x0010BF50
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineMethodNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
				}
			}
			return this.DefineMethodNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x0010CFD4 File Offset: 0x0010BFD4
		private MethodBuilder DefineMethodNoLock(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			this.CheckContext(new Type[] { returnType });
			this.CheckContext(new Type[][] { returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes });
			this.CheckContext(parameterTypeRequiredCustomModifiers);
			this.CheckContext(parameterTypeOptionalCustomModifiers);
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (parameterTypes != null)
			{
				if (parameterTypeOptionalCustomModifiers != null && parameterTypeOptionalCustomModifiers.Length != parameterTypes.Length)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[] { "parameterTypeOptionalCustomModifiers", "parameterTypes" }));
				}
				if (parameterTypeRequiredCustomModifiers != null && parameterTypeRequiredCustomModifiers.Length != parameterTypes.Length)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[] { "parameterTypeRequiredCustomModifiers", "parameterTypes" }));
				}
			}
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			if (!this.m_isHiddenGlobalType && (this.m_iAttr & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask && (attributes & MethodAttributes.Abstract) == MethodAttributes.PrivateScope && (attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadAttributeOnInterfaceMethod"));
			}
			MethodBuilder methodBuilder = new MethodBuilder(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, this.m_module, this, false);
			if (!this.m_isHiddenGlobalType && (methodBuilder.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope && methodBuilder.Name.Equals(ConstructorInfo.ConstructorName))
			{
				this.m_constructorCount++;
			}
			this.m_listMethods.Add(methodBuilder);
			return methodBuilder;
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x0010D164 File Offset: 0x0010C164
		[ComVisible(true)]
		public ConstructorBuilder DefineTypeInitializer()
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineTypeInitializerNoLock();
				}
			}
			return this.DefineTypeInitializerNoLock();
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x0010D1C8 File Offset: 0x0010C1C8
		private ConstructorBuilder DefineTypeInitializerNoLock()
		{
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			MethodAttributes methodAttributes = MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.SpecialName;
			return new ConstructorBuilder(ConstructorInfo.TypeConstructorName, methodAttributes, CallingConventions.Standard, null, this.m_module, this);
		}

		// Token: 0x06004CAA RID: 19626 RVA: 0x0010D200 File Offset: 0x0010C200
		[ComVisible(true)]
		public ConstructorBuilder DefineDefaultConstructor(MethodAttributes attributes)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineDefaultConstructorNoLock(attributes);
				}
			}
			return this.DefineDefaultConstructorNoLock(attributes);
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x0010D268 File Offset: 0x0010C268
		private ConstructorBuilder DefineDefaultConstructorNoLock(MethodAttributes attributes)
		{
			this.ThrowIfGeneric();
			ConstructorInfo constructorInfo = null;
			if (this.m_typeParent is TypeBuilderInstantiation)
			{
				Type type = this.m_typeParent.GetGenericTypeDefinition();
				if (type is TypeBuilder)
				{
					type = ((TypeBuilder)type).m_runtimeType;
				}
				if (type == null)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
				}
				Type type2 = type.MakeGenericType(this.m_typeParent.GetGenericArguments());
				if (type2 is TypeBuilderInstantiation)
				{
					constructorInfo = TypeBuilder.GetConstructor(type2, type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
				}
				else
				{
					constructorInfo = type2.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
				}
			}
			if (constructorInfo == null)
			{
				constructorInfo = this.m_typeParent.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
			}
			if (constructorInfo == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoParentDefaultConstructor"));
			}
			ConstructorBuilder constructorBuilder = this.DefineConstructor(attributes, CallingConventions.Standard, null);
			this.m_constructorCount++;
			ILGenerator ilgenerator = constructorBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Call, constructorInfo);
			ilgenerator.Emit(OpCodes.Ret);
			constructorBuilder.m_ReturnILGen = false;
			return constructorBuilder;
		}

		// Token: 0x06004CAC RID: 19628 RVA: 0x0010D377 File Offset: 0x0010C377
		[ComVisible(true)]
		public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes)
		{
			return this.DefineConstructor(attributes, callingConvention, parameterTypes, null, null);
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x0010D384 File Offset: 0x0010C384
		[ComVisible(true)]
		public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineConstructorNoLock(attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
				}
			}
			return this.DefineConstructorNoLock(attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x0010D3F8 File Offset: 0x0010C3F8
		private ConstructorBuilder DefineConstructorNoLock(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
		{
			this.CheckContext(parameterTypes);
			this.CheckContext(requiredCustomModifiers);
			this.CheckContext(optionalCustomModifiers);
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			string text;
			if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				text = ConstructorInfo.ConstructorName;
			}
			else
			{
				text = ConstructorInfo.TypeConstructorName;
			}
			attributes |= MethodAttributes.SpecialName;
			ConstructorBuilder constructorBuilder = new ConstructorBuilder(text, attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers, this.m_module, this);
			this.m_constructorCount++;
			return constructorBuilder;
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x0010D46C File Offset: 0x0010C46C
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.ThrowIfGeneric();
			return this.DefinePInvokeMethodHelper(name, dllName, name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x0010D49C File Offset: 0x0010C49C
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			return this.DefinePInvokeMethodHelper(name, dllName, entryName, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x0010D4C4 File Offset: 0x0010C4C4
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.ThrowIfGeneric();
			return this.DefinePInvokeMethodHelper(name, dllName, entryName, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, nativeCallConv, nativeCharSet);
		}

		// Token: 0x06004CB2 RID: 19634 RVA: 0x0010D4F8 File Offset: 0x0010C4F8
		public TypeBuilder DefineNestedType(string name)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name);
				}
			}
			return this.DefineNestedTypeNoLock(name);
		}

		// Token: 0x06004CB3 RID: 19635 RVA: 0x0010D560 File Offset: 0x0010C560
		private TypeBuilder DefineNestedTypeNoLock(string name)
		{
			this.ThrowIfGeneric();
			TypeBuilder typeBuilder = new TypeBuilder(name, TypeAttributes.NestedPrivate, null, null, this.m_module, PackingSize.Unspecified, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004CB4 RID: 19636 RVA: 0x0010D598 File Offset: 0x0010C598
		[ComVisible(true)]
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name, attr, parent, interfaces);
				}
			}
			return this.DefineNestedTypeNoLock(name, attr, parent, interfaces);
		}

		// Token: 0x06004CB5 RID: 19637 RVA: 0x0010D608 File Offset: 0x0010C608
		private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			this.CheckContext(new Type[] { parent });
			this.CheckContext(interfaces);
			this.ThrowIfGeneric();
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, interfaces, this.m_module, PackingSize.Unspecified, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x0010D65C File Offset: 0x0010C65C
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name, attr, parent);
				}
			}
			return this.DefineNestedTypeNoLock(name, attr, parent);
		}

		// Token: 0x06004CB7 RID: 19639 RVA: 0x0010D6C8 File Offset: 0x0010C6C8
		private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent)
		{
			this.ThrowIfGeneric();
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, null, this.m_module, PackingSize.Unspecified, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004CB8 RID: 19640 RVA: 0x0010D700 File Offset: 0x0010C700
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name, attr);
				}
			}
			return this.DefineNestedTypeNoLock(name, attr);
		}

		// Token: 0x06004CB9 RID: 19641 RVA: 0x0010D768 File Offset: 0x0010C768
		private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr)
		{
			this.ThrowIfGeneric();
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, null, null, this.m_module, PackingSize.Unspecified, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004CBA RID: 19642 RVA: 0x0010D7A0 File Offset: 0x0010C7A0
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, int typeSize)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name, attr, parent, typeSize);
				}
			}
			return this.DefineNestedTypeNoLock(name, attr, parent, typeSize);
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x0010D810 File Offset: 0x0010C810
		private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent, int typeSize)
		{
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, this.m_module, PackingSize.Unspecified, typeSize, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x0010D844 File Offset: 0x0010C844
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineNestedTypeNoLock(name, attr, parent, packSize);
				}
			}
			return this.DefineNestedTypeNoLock(name, attr, parent, packSize);
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x0010D8B4 File Offset: 0x0010C8B4
		private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent, PackingSize packSize)
		{
			this.ThrowIfGeneric();
			TypeBuilder typeBuilder = new TypeBuilder(name, attr, parent, null, this.m_module, packSize, this);
			this.m_module.m_TypeBuilderList.Add(typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x0010D8ED File Offset: 0x0010C8ED
		public FieldBuilder DefineField(string fieldName, Type type, FieldAttributes attributes)
		{
			return this.DefineField(fieldName, type, null, null, attributes);
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x0010D8FC File Offset: 0x0010C8FC
		public FieldBuilder DefineField(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineFieldNoLock(fieldName, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
				}
			}
			return this.DefineFieldNoLock(fieldName, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x0010D970 File Offset: 0x0010C970
		private FieldBuilder DefineFieldNoLock(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
		{
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			this.CheckContext(new Type[] { type });
			this.CheckContext(requiredCustomModifiers);
			if (this.m_underlyingSystemType == null && base.IsEnum && (attributes & FieldAttributes.Static) == FieldAttributes.PrivateScope)
			{
				this.m_underlyingSystemType = type;
			}
			return new FieldBuilder(this, fieldName, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x0010D9D0 File Offset: 0x0010C9D0
		public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineInitializedDataNoLock(name, data, attributes);
				}
			}
			return this.DefineInitializedDataNoLock(name, data, attributes);
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x0010DA3C File Offset: 0x0010CA3C
		private FieldBuilder DefineInitializedDataNoLock(string name, byte[] data, FieldAttributes attributes)
		{
			this.ThrowIfGeneric();
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			return this.DefineDataHelper(name, data, data.Length, attributes);
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x0010DA60 File Offset: 0x0010CA60
		public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineUninitializedDataNoLock(name, size, attributes);
				}
			}
			return this.DefineUninitializedDataNoLock(name, size, attributes);
		}

		// Token: 0x06004CC4 RID: 19652 RVA: 0x0010DACC File Offset: 0x0010CACC
		private FieldBuilder DefineUninitializedDataNoLock(string name, int size, FieldAttributes attributes)
		{
			this.ThrowIfGeneric();
			return this.DefineDataHelper(name, null, size, attributes);
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x0010DAE0 File Offset: 0x0010CAE0
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return this.DefineProperty(name, attributes, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x0010DAFC File Offset: 0x0010CAFC
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			return this.DefineProperty(name, attributes, (CallingConventions)0, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x0010DB20 File Offset: 0x0010CB20
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefinePropertyNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
				}
			}
			return this.DefinePropertyNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x0010DBA4 File Offset: 0x0010CBA4
		private PropertyBuilder DefinePropertyNoLock(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			this.ThrowIfGeneric();
			this.CheckContext(new Type[] { returnType });
			this.CheckContext(new Type[][] { returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes });
			this.CheckContext(parameterTypeRequiredCustomModifiers);
			this.CheckContext(parameterTypeOptionalCustomModifiers);
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			this.ThrowIfCreated();
			SignatureHelper propertySigHelper = SignatureHelper.GetPropertySigHelper(this.m_module, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
			int num;
			byte[] array = propertySigHelper.InternalGetSignature(out num);
			PropertyToken propertyToken = new PropertyToken(TypeBuilder.InternalDefineProperty(this.m_module, this.m_tdType.Token, name, (int)attributes, array, num, 0, 0));
			return new PropertyBuilder(this.m_module, name, propertySigHelper, attributes, returnType, propertyToken, this);
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x0010DC88 File Offset: 0x0010CC88
		public EventBuilder DefineEvent(string name, EventAttributes attributes, Type eventtype)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.DefineEventNoLock(name, attributes, eventtype);
				}
			}
			return this.DefineEventNoLock(name, attributes, eventtype);
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x0010DCF4 File Offset: 0x0010CCF4
		private EventBuilder DefineEventNoLock(string name, EventAttributes attributes, Type eventtype)
		{
			this.CheckContext(new Type[] { eventtype });
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
			}
			if (name[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalName"), "name");
			}
			int token = this.m_module.GetTypeTokenInternal(eventtype).Token;
			EventToken eventToken = new EventToken(TypeBuilder.InternalDefineEvent(this.m_module, this.m_tdType.Token, name, (int)attributes, token));
			return new EventBuilder(this.m_module, name, attributes, token, this, eventToken);
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x0010DDB0 File Offset: 0x0010CDB0
		public Type CreateType()
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					return this.CreateTypeNoLock();
				}
			}
			return this.CreateTypeNoLock();
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x0010DE14 File Offset: 0x0010CE14
		internal void CheckContext(params Type[][] typess)
		{
			((AssemblyBuilder)this.Module.Assembly).CheckContext(typess);
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x0010DE2C File Offset: 0x0010CE2C
		internal void CheckContext(params Type[] types)
		{
			((AssemblyBuilder)this.Module.Assembly).CheckContext(types);
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x0010DE44 File Offset: 0x0010CE44
		private Type CreateTypeNoLock()
		{
			if (this.IsCreated())
			{
				return this.m_runtimeType;
			}
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			if (this.m_typeInterfaces == null)
			{
				this.m_typeInterfaces = new Type[0];
			}
			int[] array = new int[this.m_typeInterfaces.Length];
			for (int i = 0; i < this.m_typeInterfaces.Length; i++)
			{
				array[i] = this.m_module.GetTypeTokenInternal(this.m_typeInterfaces[i]).Token;
			}
			int num = 0;
			if (this.m_typeParent != null)
			{
				num = this.m_module.GetTypeTokenInternal(this.m_typeParent).Token;
			}
			if (this.IsGenericParameter)
			{
				int[] array2 = new int[this.m_typeInterfaces.Length];
				if (this.m_typeParent != null)
				{
					array2 = new int[this.m_typeInterfaces.Length + 1];
					array2[array2.Length - 1] = num;
				}
				for (int j = 0; j < this.m_typeInterfaces.Length; j++)
				{
					array2[j] = this.m_module.GetTypeTokenInternal(this.m_typeInterfaces[j]).Token;
				}
				int num2 = ((this.m_declMeth == null) ? this.m_DeclaringType.m_tdType.Token : this.m_declMeth.GetToken().Token);
				this.m_tdType = new TypeToken(this.InternalDefineGenParam(this.m_strName, num2, this.m_genParamPos, (int)this.m_genParamAttributes, array2, this.m_module, 0));
				if (this.m_ca != null)
				{
					foreach (object obj in this.m_ca)
					{
						TypeBuilder.CustAttr custAttr = (TypeBuilder.CustAttr)obj;
						custAttr.Bake(this.m_module, this.MetadataTokenInternal);
					}
				}
				this.m_hasBeenCreated = true;
				return this;
			}
			if ((this.m_tdType.Token & 16777215) != 0 && (num & 16777215) != 0)
			{
				TypeBuilder.InternalSetParentType(this.m_tdType.Token, num, this.m_module);
			}
			if (this.m_inst != null)
			{
				foreach (GenericTypeParameterBuilder type in this.m_inst)
				{
					if (type is GenericTypeParameterBuilder)
					{
						((GenericTypeParameterBuilder)type).m_type.CreateType();
					}
				}
			}
			if (!this.m_isHiddenGlobalType && this.m_constructorCount == 0 && (this.m_iAttr & TypeAttributes.ClassSemanticsMask) == TypeAttributes.NotPublic && !base.IsValueType && (this.m_iAttr & (TypeAttributes.Abstract | TypeAttributes.Sealed)) != (TypeAttributes.Abstract | TypeAttributes.Sealed))
			{
				this.DefineDefaultConstructor(MethodAttributes.Public);
			}
			int count = this.m_listMethods.Count;
			for (int l = 0; l < count; l++)
			{
				MethodBuilder methodBuilder = (MethodBuilder)this.m_listMethods[l];
				if (methodBuilder.IsGenericMethodDefinition)
				{
					methodBuilder.GetToken();
				}
				MethodAttributes attributes = methodBuilder.Attributes;
				if ((methodBuilder.GetMethodImplementationFlags() & (MethodImplAttributes)135) == MethodImplAttributes.IL && (attributes & MethodAttributes.PinvokeImpl) == MethodAttributes.PrivateScope)
				{
					int num3;
					byte[] array3 = methodBuilder.GetLocalsSignature().InternalGetSignature(out num3);
					if ((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope && (this.m_iAttr & TypeAttributes.Abstract) == TypeAttributes.NotPublic)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadTypeAttributesNotAbstract"));
					}
					byte[] array4 = methodBuilder.GetBody();
					if ((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
					{
						if (array4 != null)
						{
							throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadMethodBody"));
						}
					}
					else if (array4 == null || array4.Length == 0)
					{
						if (methodBuilder.m_ilGenerator != null)
						{
							methodBuilder.CreateMethodBodyHelper(methodBuilder.GetILGenerator());
						}
						array4 = methodBuilder.GetBody();
						if ((array4 == null || array4.Length == 0) && !methodBuilder.m_canBeRuntimeImpl)
						{
							throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_BadEmptyMethodBody"), new object[] { methodBuilder.Name }));
						}
					}
					int num4;
					if (methodBuilder.m_ilGenerator != null)
					{
						num4 = methodBuilder.m_ilGenerator.GetMaxStackSize();
					}
					else
					{
						num4 = 16;
					}
					__ExceptionInstance[] exceptionInstances = methodBuilder.GetExceptionInstances();
					int[] tokenFixups = methodBuilder.GetTokenFixups();
					int[] rvafixups = methodBuilder.GetRVAFixups();
					__ExceptionInstance[] array5 = null;
					int[] array6 = null;
					int[] array7 = null;
					if (exceptionInstances != null)
					{
						array5 = new __ExceptionInstance[exceptionInstances.Length];
						Array.Copy(exceptionInstances, array5, exceptionInstances.Length);
					}
					if (tokenFixups != null)
					{
						array6 = new int[tokenFixups.Length];
						Array.Copy(tokenFixups, array6, tokenFixups.Length);
					}
					if (rvafixups != null)
					{
						array7 = new int[rvafixups.Length];
						Array.Copy(rvafixups, array7, rvafixups.Length);
					}
					TypeBuilder.InternalSetMethodIL(methodBuilder.GetToken().Token, methodBuilder.InitLocals, array4, array3, num3, num4, methodBuilder.GetNumberOfExceptions(), array5, array6, array7, this.m_module);
					if (this.Assembly.m_assemblyData.m_access == AssemblyBuilderAccess.Run)
					{
						methodBuilder.ReleaseBakedStructures();
					}
				}
			}
			this.m_hasBeenCreated = true;
			Type type2 = this.TermCreateClass(this.m_tdType.Token, this.m_module);
			if (!this.m_isHiddenGlobalType)
			{
				this.m_runtimeType = (RuntimeType)type2;
				if (this.m_DeclaringType != null && this.m_DeclaringType.m_runtimeType != null)
				{
					this.m_DeclaringType.m_runtimeType.InvalidateCachedNestedType();
				}
				return type2;
			}
			return null;
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06004CCF RID: 19663 RVA: 0x0010E35C File Offset: 0x0010D35C
		public int Size
		{
			get
			{
				return this.m_iTypeSize;
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06004CD0 RID: 19664 RVA: 0x0010E364 File Offset: 0x0010D364
		public PackingSize PackingSize
		{
			get
			{
				return this.m_iPackingSize;
			}
		}

		// Token: 0x06004CD1 RID: 19665 RVA: 0x0010E36C File Offset: 0x0010D36C
		public void SetParent(Type parent)
		{
			this.ThrowIfGeneric();
			this.ThrowIfCreated();
			this.CheckContext(new Type[] { parent });
			if (parent != null)
			{
				this.m_typeParent = parent;
				return;
			}
			if ((this.m_iAttr & TypeAttributes.ClassSemanticsMask) != TypeAttributes.ClassSemanticsMask)
			{
				this.m_typeParent = typeof(object);
				return;
			}
			if ((this.m_iAttr & TypeAttributes.Abstract) == TypeAttributes.NotPublic)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadInterfaceNotAbstract"));
			}
			this.m_typeParent = null;
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x0010E3E8 File Offset: 0x0010D3E8
		[ComVisible(true)]
		public void AddInterfaceImplementation(Type interfaceType)
		{
			this.ThrowIfGeneric();
			this.CheckContext(new Type[] { interfaceType });
			if (interfaceType == null)
			{
				throw new ArgumentNullException("interfaceType");
			}
			this.ThrowIfCreated();
			TypeToken typeTokenInternal = this.m_module.GetTypeTokenInternal(interfaceType);
			TypeBuilder.InternalAddInterfaceImpl(this.m_tdType.Token, typeTokenInternal.Token, this.m_module);
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x0010E44C File Offset: 0x0010D44C
		public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
		{
			if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
			{
				lock (this.Module.Assembly.m_assemblyData)
				{
					this.AddDeclarativeSecurityNoLock(action, pset);
					return;
				}
			}
			this.AddDeclarativeSecurityNoLock(action, pset);
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x0010E4B0 File Offset: 0x0010D4B0
		private void AddDeclarativeSecurityNoLock(SecurityAction action, PermissionSet pset)
		{
			this.ThrowIfGeneric();
			if (pset == null)
			{
				throw new ArgumentNullException("pset");
			}
			if (!Enum.IsDefined(typeof(SecurityAction), action) || action == SecurityAction.RequestMinimum || action == SecurityAction.RequestOptional || action == SecurityAction.RequestRefuse)
			{
				throw new ArgumentOutOfRangeException("action");
			}
			this.ThrowIfCreated();
			byte[] array = null;
			if (!pset.IsEmpty())
			{
				array = pset.EncodeXml();
			}
			TypeBuilder.InternalAddDeclarativeSecurity(this.m_module, this.m_tdType.Token, action, array);
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06004CD5 RID: 19669 RVA: 0x0010E530 File Offset: 0x0010D530
		public TypeToken TypeToken
		{
			get
			{
				if (this.IsGenericParameter)
				{
					this.ThrowIfCreated();
				}
				return this.m_tdType;
			}
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x0010E548 File Offset: 0x0010D548
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.ThrowIfGeneric();
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			TypeBuilder.InternalCreateCustomAttribute(this.m_tdType.Token, this.m_module.GetConstructorToken(con).Token, binaryAttribute, this.m_module, false);
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x0010E5A3 File Offset: 0x0010D5A3
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			this.ThrowIfGeneric();
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			customBuilder.CreateCustomAttribute(this.m_module, this.m_tdType.Token);
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x0010E5D0 File Offset: 0x0010D5D0
		void _TypeBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x0010E5D7 File Offset: 0x0010D5D7
		void _TypeBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x0010E5DE File Offset: 0x0010D5DE
		void _TypeBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x0010E5E5 File Offset: 0x0010D5E5
		void _TypeBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040027D7 RID: 10199
		public const int UnspecifiedTypeSize = 0;

		// Token: 0x040027D8 RID: 10200
		internal ArrayList m_ca;

		// Token: 0x040027D9 RID: 10201
		internal MethodBuilder m_currentMethod;

		// Token: 0x040027DA RID: 10202
		private TypeToken m_tdType;

		// Token: 0x040027DB RID: 10203
		private ModuleBuilder m_module;

		// Token: 0x040027DC RID: 10204
		internal string m_strName;

		// Token: 0x040027DD RID: 10205
		private string m_strNameSpace;

		// Token: 0x040027DE RID: 10206
		private string m_strFullQualName;

		// Token: 0x040027DF RID: 10207
		private Type m_typeParent;

		// Token: 0x040027E0 RID: 10208
		private Type[] m_typeInterfaces;

		// Token: 0x040027E1 RID: 10209
		internal TypeAttributes m_iAttr;

		// Token: 0x040027E2 RID: 10210
		internal GenericParameterAttributes m_genParamAttributes;

		// Token: 0x040027E3 RID: 10211
		internal ArrayList m_listMethods;

		// Token: 0x040027E4 RID: 10212
		private int m_constructorCount;

		// Token: 0x040027E5 RID: 10213
		private int m_iTypeSize;

		// Token: 0x040027E6 RID: 10214
		private PackingSize m_iPackingSize;

		// Token: 0x040027E7 RID: 10215
		private TypeBuilder m_DeclaringType;

		// Token: 0x040027E8 RID: 10216
		private Type m_underlyingSystemType;

		// Token: 0x040027E9 RID: 10217
		internal bool m_isHiddenGlobalType;

		// Token: 0x040027EA RID: 10218
		internal bool m_isHiddenType;

		// Token: 0x040027EB RID: 10219
		internal bool m_hasBeenCreated;

		// Token: 0x040027EC RID: 10220
		internal RuntimeType m_runtimeType;

		// Token: 0x040027ED RID: 10221
		private int m_genParamPos;

		// Token: 0x040027EE RID: 10222
		private GenericTypeParameterBuilder[] m_inst;

		// Token: 0x040027EF RID: 10223
		private bool m_bIsGenParam;

		// Token: 0x040027F0 RID: 10224
		private bool m_bIsGenTypeDef;

		// Token: 0x040027F1 RID: 10225
		private MethodBuilder m_declMeth;

		// Token: 0x040027F2 RID: 10226
		private TypeBuilder m_genTypeDef;

		// Token: 0x02000836 RID: 2102
		internal class CustAttr
		{
			// Token: 0x06004CDC RID: 19676 RVA: 0x0010E5EC File Offset: 0x0010D5EC
			public CustAttr(ConstructorInfo con, byte[] binaryAttribute)
			{
				if (con == null)
				{
					throw new ArgumentNullException("con");
				}
				if (binaryAttribute == null)
				{
					throw new ArgumentNullException("binaryAttribute");
				}
				this.m_con = con;
				this.m_binaryAttribute = binaryAttribute;
			}

			// Token: 0x06004CDD RID: 19677 RVA: 0x0010E61E File Offset: 0x0010D61E
			public CustAttr(CustomAttributeBuilder customBuilder)
			{
				if (customBuilder == null)
				{
					throw new ArgumentNullException("customBuilder");
				}
				this.m_customBuilder = customBuilder;
			}

			// Token: 0x06004CDE RID: 19678 RVA: 0x0010E63C File Offset: 0x0010D63C
			public void Bake(ModuleBuilder module, int token)
			{
				if (this.m_customBuilder == null)
				{
					TypeBuilder.InternalCreateCustomAttribute(token, module.GetConstructorToken(this.m_con).Token, this.m_binaryAttribute, module, false);
					return;
				}
				this.m_customBuilder.CreateCustomAttribute(module, token);
			}

			// Token: 0x040027F3 RID: 10227
			private ConstructorInfo m_con;

			// Token: 0x040027F4 RID: 10228
			private byte[] m_binaryAttribute;

			// Token: 0x040027F5 RID: 10229
			private CustomAttributeBuilder m_customBuilder;
		}
	}
}
