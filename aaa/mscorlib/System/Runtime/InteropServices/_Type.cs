using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020000F1 RID: 241
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[Guid("BCA8B44D-AAD6-3A86-8AB7-03349F4F2DA2")]
	[TypeLibImportClass(typeof(Type))]
	[ComVisible(true)]
	public interface _Type
	{
		// Token: 0x06000CC0 RID: 3264
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06000CC1 RID: 3265
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06000CC2 RID: 3266
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06000CC3 RID: 3267
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06000CC4 RID: 3268
		string ToString();

		// Token: 0x06000CC5 RID: 3269
		bool Equals(object other);

		// Token: 0x06000CC6 RID: 3270
		int GetHashCode();

		// Token: 0x06000CC7 RID: 3271
		Type GetType();

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000CC8 RID: 3272
		MemberTypes MemberType { get; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000CC9 RID: 3273
		string Name { get; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000CCA RID: 3274
		Type DeclaringType { get; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000CCB RID: 3275
		Type ReflectedType { get; }

		// Token: 0x06000CCC RID: 3276
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06000CCD RID: 3277
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06000CCE RID: 3278
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000CCF RID: 3279
		Guid GUID { get; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000CD0 RID: 3280
		Module Module { get; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000CD1 RID: 3281
		Assembly Assembly { get; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000CD2 RID: 3282
		RuntimeTypeHandle TypeHandle { get; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000CD3 RID: 3283
		string FullName { get; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000CD4 RID: 3284
		string Namespace { get; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000CD5 RID: 3285
		string AssemblyQualifiedName { get; }

		// Token: 0x06000CD6 RID: 3286
		int GetArrayRank();

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000CD7 RID: 3287
		Type BaseType { get; }

		// Token: 0x06000CD8 RID: 3288
		ConstructorInfo[] GetConstructors(BindingFlags bindingAttr);

		// Token: 0x06000CD9 RID: 3289
		Type GetInterface(string name, bool ignoreCase);

		// Token: 0x06000CDA RID: 3290
		Type[] GetInterfaces();

		// Token: 0x06000CDB RID: 3291
		Type[] FindInterfaces(TypeFilter filter, object filterCriteria);

		// Token: 0x06000CDC RID: 3292
		EventInfo GetEvent(string name, BindingFlags bindingAttr);

		// Token: 0x06000CDD RID: 3293
		EventInfo[] GetEvents();

		// Token: 0x06000CDE RID: 3294
		EventInfo[] GetEvents(BindingFlags bindingAttr);

		// Token: 0x06000CDF RID: 3295
		Type[] GetNestedTypes(BindingFlags bindingAttr);

		// Token: 0x06000CE0 RID: 3296
		Type GetNestedType(string name, BindingFlags bindingAttr);

		// Token: 0x06000CE1 RID: 3297
		MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr);

		// Token: 0x06000CE2 RID: 3298
		MemberInfo[] GetDefaultMembers();

		// Token: 0x06000CE3 RID: 3299
		MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria);

		// Token: 0x06000CE4 RID: 3300
		Type GetElementType();

		// Token: 0x06000CE5 RID: 3301
		bool IsSubclassOf(Type c);

		// Token: 0x06000CE6 RID: 3302
		bool IsInstanceOfType(object o);

		// Token: 0x06000CE7 RID: 3303
		bool IsAssignableFrom(Type c);

		// Token: 0x06000CE8 RID: 3304
		InterfaceMapping GetInterfaceMap(Type interfaceType);

		// Token: 0x06000CE9 RID: 3305
		MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000CEA RID: 3306
		MethodInfo GetMethod(string name, BindingFlags bindingAttr);

		// Token: 0x06000CEB RID: 3307
		MethodInfo[] GetMethods(BindingFlags bindingAttr);

		// Token: 0x06000CEC RID: 3308
		FieldInfo GetField(string name, BindingFlags bindingAttr);

		// Token: 0x06000CED RID: 3309
		FieldInfo[] GetFields(BindingFlags bindingAttr);

		// Token: 0x06000CEE RID: 3310
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr);

		// Token: 0x06000CEF RID: 3311
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000CF0 RID: 3312
		PropertyInfo[] GetProperties(BindingFlags bindingAttr);

		// Token: 0x06000CF1 RID: 3313
		MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

		// Token: 0x06000CF2 RID: 3314
		MemberInfo[] GetMembers(BindingFlags bindingAttr);

		// Token: 0x06000CF3 RID: 3315
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000CF4 RID: 3316
		Type UnderlyingSystemType { get; }

		// Token: 0x06000CF5 RID: 3317
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, CultureInfo culture);

		// Token: 0x06000CF6 RID: 3318
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args);

		// Token: 0x06000CF7 RID: 3319
		ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000CF8 RID: 3320
		ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000CF9 RID: 3321
		ConstructorInfo GetConstructor(Type[] types);

		// Token: 0x06000CFA RID: 3322
		ConstructorInfo[] GetConstructors();

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000CFB RID: 3323
		ConstructorInfo TypeInitializer { get; }

		// Token: 0x06000CFC RID: 3324
		MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000CFD RID: 3325
		MethodInfo GetMethod(string name, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000CFE RID: 3326
		MethodInfo GetMethod(string name, Type[] types);

		// Token: 0x06000CFF RID: 3327
		MethodInfo GetMethod(string name);

		// Token: 0x06000D00 RID: 3328
		MethodInfo[] GetMethods();

		// Token: 0x06000D01 RID: 3329
		FieldInfo GetField(string name);

		// Token: 0x06000D02 RID: 3330
		FieldInfo[] GetFields();

		// Token: 0x06000D03 RID: 3331
		Type GetInterface(string name);

		// Token: 0x06000D04 RID: 3332
		EventInfo GetEvent(string name);

		// Token: 0x06000D05 RID: 3333
		PropertyInfo GetProperty(string name, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06000D06 RID: 3334
		PropertyInfo GetProperty(string name, Type returnType, Type[] types);

		// Token: 0x06000D07 RID: 3335
		PropertyInfo GetProperty(string name, Type[] types);

		// Token: 0x06000D08 RID: 3336
		PropertyInfo GetProperty(string name, Type returnType);

		// Token: 0x06000D09 RID: 3337
		PropertyInfo GetProperty(string name);

		// Token: 0x06000D0A RID: 3338
		PropertyInfo[] GetProperties();

		// Token: 0x06000D0B RID: 3339
		Type[] GetNestedTypes();

		// Token: 0x06000D0C RID: 3340
		Type GetNestedType(string name);

		// Token: 0x06000D0D RID: 3341
		MemberInfo[] GetMember(string name);

		// Token: 0x06000D0E RID: 3342
		MemberInfo[] GetMembers();

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000D0F RID: 3343
		TypeAttributes Attributes { get; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000D10 RID: 3344
		bool IsNotPublic { get; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000D11 RID: 3345
		bool IsPublic { get; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000D12 RID: 3346
		bool IsNestedPublic { get; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000D13 RID: 3347
		bool IsNestedPrivate { get; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000D14 RID: 3348
		bool IsNestedFamily { get; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000D15 RID: 3349
		bool IsNestedAssembly { get; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000D16 RID: 3350
		bool IsNestedFamANDAssem { get; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000D17 RID: 3351
		bool IsNestedFamORAssem { get; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000D18 RID: 3352
		bool IsAutoLayout { get; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000D19 RID: 3353
		bool IsLayoutSequential { get; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000D1A RID: 3354
		bool IsExplicitLayout { get; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000D1B RID: 3355
		bool IsClass { get; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000D1C RID: 3356
		bool IsInterface { get; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000D1D RID: 3357
		bool IsValueType { get; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000D1E RID: 3358
		bool IsAbstract { get; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000D1F RID: 3359
		bool IsSealed { get; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000D20 RID: 3360
		bool IsEnum { get; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000D21 RID: 3361
		bool IsSpecialName { get; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000D22 RID: 3362
		bool IsImport { get; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000D23 RID: 3363
		bool IsSerializable { get; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000D24 RID: 3364
		bool IsAnsiClass { get; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000D25 RID: 3365
		bool IsUnicodeClass { get; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000D26 RID: 3366
		bool IsAutoClass { get; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000D27 RID: 3367
		bool IsArray { get; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000D28 RID: 3368
		bool IsByRef { get; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000D29 RID: 3369
		bool IsPointer { get; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000D2A RID: 3370
		bool IsPrimitive { get; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000D2B RID: 3371
		bool IsCOMObject { get; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000D2C RID: 3372
		bool HasElementType { get; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000D2D RID: 3373
		bool IsContextful { get; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000D2E RID: 3374
		bool IsMarshalByRef { get; }

		// Token: 0x06000D2F RID: 3375
		bool Equals(Type o);
	}
}
