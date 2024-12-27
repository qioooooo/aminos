using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace System.Runtime.InteropServices.TCEAdapterGen
{
	// Token: 0x020008DD RID: 2269
	internal class EventProviderWriter
	{
		// Token: 0x060052C3 RID: 21187 RVA: 0x0012B14B File Offset: 0x0012A14B
		public EventProviderWriter(ModuleBuilder OutputModule, string strDestTypeName, Type EventItfType, Type SrcItfType, Type SinkHelperType)
		{
			this.m_OutputModule = OutputModule;
			this.m_strDestTypeName = strDestTypeName;
			this.m_EventItfType = EventItfType;
			this.m_SrcItfType = SrcItfType;
			this.m_SinkHelperType = SinkHelperType;
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x0012B178 File Offset: 0x0012A178
		public Type Perform()
		{
			TypeBuilder typeBuilder = this.m_OutputModule.DefineType(this.m_strDestTypeName, TypeAttributes.Sealed, typeof(object), new Type[]
			{
				this.m_EventItfType,
				typeof(IDisposable)
			});
			FieldBuilder fieldBuilder = typeBuilder.DefineField("m_ConnectionPointContainer", typeof(IConnectionPointContainer), FieldAttributes.Private);
			FieldBuilder fieldBuilder2 = typeBuilder.DefineField("m_aEventSinkHelpers", typeof(ArrayList), FieldAttributes.Private);
			FieldBuilder fieldBuilder3 = typeBuilder.DefineField("m_ConnectionPoint", typeof(IConnectionPoint), FieldAttributes.Private);
			MethodBuilder methodBuilder = this.DefineInitSrcItfMethod(typeBuilder, this.m_SrcItfType, fieldBuilder2, fieldBuilder3, fieldBuilder);
			MethodInfo[] nonPropertyMethods = TCEAdapterGenerator.GetNonPropertyMethods(this.m_SrcItfType);
			for (int i = 0; i < nonPropertyMethods.Length; i++)
			{
				if (this.m_SrcItfType == nonPropertyMethods[i].DeclaringType)
				{
					this.DefineAddEventMethod(typeBuilder, nonPropertyMethods[i], this.m_SinkHelperType, fieldBuilder2, fieldBuilder3, methodBuilder);
					this.DefineRemoveEventMethod(typeBuilder, nonPropertyMethods[i], this.m_SinkHelperType, fieldBuilder2, fieldBuilder3);
				}
			}
			this.DefineConstructor(typeBuilder, fieldBuilder);
			MethodBuilder methodBuilder2 = this.DefineFinalizeMethod(typeBuilder, this.m_SinkHelperType, fieldBuilder2, fieldBuilder3);
			this.DefineDisposeMethod(typeBuilder, methodBuilder2);
			return typeBuilder.CreateType();
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x0012B2A8 File Offset: 0x0012A2A8
		private MethodBuilder DefineAddEventMethod(TypeBuilder OutputTypeBuilder, MethodInfo SrcItfMethod, Type SinkHelperClass, FieldBuilder fbSinkHelperArray, FieldBuilder fbEventCP, MethodBuilder mbInitSrcItf)
		{
			FieldInfo field = SinkHelperClass.GetField("m_" + SrcItfMethod.Name + "Delegate");
			FieldInfo field2 = SinkHelperClass.GetField("m_dwCookie");
			ConstructorInfo constructor = SinkHelperClass.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
			MethodInfo method = typeof(IConnectionPoint).GetMethod("Advise");
			Type[] array = new Type[] { typeof(object) };
			MethodInfo method2 = typeof(ArrayList).GetMethod("Add", array, null);
			array[0] = typeof(object);
			MethodInfo method3 = typeof(Monitor).GetMethod("Enter", array, null);
			array[0] = typeof(object);
			MethodInfo method4 = typeof(Monitor).GetMethod("Exit", array, null);
			Type[] array2 = new Type[] { field.FieldType };
			MethodBuilder methodBuilder = OutputTypeBuilder.DefineMethod("add_" + SrcItfMethod.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, null, array2);
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			Label label = ilgenerator.DefineLabel();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(SinkHelperClass);
			LocalBuilder localBuilder2 = ilgenerator.DeclareLocal(typeof(int));
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Call, method3);
			ilgenerator.BeginExceptionBlock();
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbEventCP);
			ilgenerator.Emit(OpCodes.Brtrue, label);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Call, mbInitSrcItf);
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(OpCodes.Newobj, constructor);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder);
			ilgenerator.Emit(OpCodes.Ldc_I4_0);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbEventCP);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Castclass, typeof(object));
			ilgenerator.Emit(OpCodes.Ldloca, localBuilder2);
			ilgenerator.Emit(OpCodes.Callvirt, method);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Stfld, field2);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Ldarg, 1);
			ilgenerator.Emit(OpCodes.Stfld, field);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbSinkHelperArray);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Castclass, typeof(object));
			ilgenerator.Emit(OpCodes.Callvirt, method2);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.BeginFinallyBlock();
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Call, method4);
			ilgenerator.EndExceptionBlock();
			ilgenerator.Emit(OpCodes.Ret);
			return methodBuilder;
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x0012B5BC File Offset: 0x0012A5BC
		private MethodBuilder DefineRemoveEventMethod(TypeBuilder OutputTypeBuilder, MethodInfo SrcItfMethod, Type SinkHelperClass, FieldBuilder fbSinkHelperArray, FieldBuilder fbEventCP)
		{
			FieldInfo field = SinkHelperClass.GetField("m_" + SrcItfMethod.Name + "Delegate");
			FieldInfo field2 = SinkHelperClass.GetField("m_dwCookie");
			Type[] array = new Type[] { typeof(int) };
			MethodInfo method = typeof(ArrayList).GetMethod("RemoveAt", array, null);
			PropertyInfo property = typeof(ArrayList).GetProperty("Item");
			MethodInfo getMethod = property.GetGetMethod();
			PropertyInfo property2 = typeof(ArrayList).GetProperty("Count");
			MethodInfo getMethod2 = property2.GetGetMethod();
			array[0] = typeof(Delegate);
			MethodInfo method2 = typeof(Delegate).GetMethod("Equals", array, null);
			array[0] = typeof(object);
			MethodInfo method3 = typeof(Monitor).GetMethod("Enter", array, null);
			array[0] = typeof(object);
			MethodInfo method4 = typeof(Monitor).GetMethod("Exit", array, null);
			MethodInfo method5 = typeof(IConnectionPoint).GetMethod("Unadvise");
			MethodInfo method6 = typeof(Marshal).GetMethod("ReleaseComObject");
			Type[] array2 = new Type[] { field.FieldType };
			MethodBuilder methodBuilder = OutputTypeBuilder.DefineMethod("remove_" + SrcItfMethod.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, null, array2);
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(typeof(int));
			LocalBuilder localBuilder2 = ilgenerator.DeclareLocal(typeof(int));
			LocalBuilder localBuilder3 = ilgenerator.DeclareLocal(SinkHelperClass);
			Label label = ilgenerator.DefineLabel();
			Label label2 = ilgenerator.DefineLabel();
			Label label3 = ilgenerator.DefineLabel();
			ilgenerator.DefineLabel();
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Call, method3);
			ilgenerator.BeginExceptionBlock();
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbSinkHelperArray);
			ilgenerator.Emit(OpCodes.Brfalse, label2);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbSinkHelperArray);
			ilgenerator.Emit(OpCodes.Callvirt, getMethod2);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder);
			ilgenerator.Emit(OpCodes.Ldc_I4, 0);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Ldc_I4, 0);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Bge, label2);
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbSinkHelperArray);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Callvirt, getMethod);
			ilgenerator.Emit(OpCodes.Castclass, SinkHelperClass);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder3);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder3);
			ilgenerator.Emit(OpCodes.Ldfld, field);
			ilgenerator.Emit(OpCodes.Ldnull);
			ilgenerator.Emit(OpCodes.Beq, label3);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder3);
			ilgenerator.Emit(OpCodes.Ldfld, field);
			ilgenerator.Emit(OpCodes.Ldarg, 1);
			ilgenerator.Emit(OpCodes.Castclass, typeof(object));
			ilgenerator.Emit(OpCodes.Callvirt, method2);
			ilgenerator.Emit(OpCodes.Ldc_I4, 255);
			ilgenerator.Emit(OpCodes.And);
			ilgenerator.Emit(OpCodes.Ldc_I4, 0);
			ilgenerator.Emit(OpCodes.Beq, label3);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbSinkHelperArray);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Callvirt, method);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbEventCP);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder3);
			ilgenerator.Emit(OpCodes.Ldfld, field2);
			ilgenerator.Emit(OpCodes.Callvirt, method5);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Ldc_I4, 1);
			ilgenerator.Emit(OpCodes.Bgt, label2);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbEventCP);
			ilgenerator.Emit(OpCodes.Call, method6);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldnull);
			ilgenerator.Emit(OpCodes.Stfld, fbEventCP);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldnull);
			ilgenerator.Emit(OpCodes.Stfld, fbSinkHelperArray);
			ilgenerator.Emit(OpCodes.Br, label2);
			ilgenerator.MarkLabel(label3);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Ldc_I4, 1);
			ilgenerator.Emit(OpCodes.Add);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Blt, label);
			ilgenerator.MarkLabel(label2);
			ilgenerator.BeginFinallyBlock();
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Call, method4);
			ilgenerator.EndExceptionBlock();
			ilgenerator.Emit(OpCodes.Ret);
			return methodBuilder;
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x0012BB38 File Offset: 0x0012AB38
		private MethodBuilder DefineInitSrcItfMethod(TypeBuilder OutputTypeBuilder, Type SourceInterface, FieldBuilder fbSinkHelperArray, FieldBuilder fbEventCP, FieldBuilder fbCPC)
		{
			ConstructorInfo constructor = typeof(ArrayList).GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, new Type[0], null);
			byte[] array = new byte[16];
			Type[] array2 = new Type[] { typeof(byte[]) };
			ConstructorInfo constructor2 = typeof(Guid).GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, array2, null);
			MethodInfo method = typeof(IConnectionPointContainer).GetMethod("FindConnectionPoint");
			MethodBuilder methodBuilder = OutputTypeBuilder.DefineMethod("Init", MethodAttributes.Private, null, null);
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(typeof(IConnectionPoint));
			LocalBuilder localBuilder2 = ilgenerator.DeclareLocal(typeof(Guid));
			LocalBuilder localBuilder3 = ilgenerator.DeclareLocal(typeof(byte[]));
			ilgenerator.Emit(OpCodes.Ldnull);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder);
			array = SourceInterface.GUID.ToByteArray();
			ilgenerator.Emit(OpCodes.Ldc_I4, 16);
			ilgenerator.Emit(OpCodes.Newarr, typeof(byte));
			ilgenerator.Emit(OpCodes.Stloc, localBuilder3);
			for (int i = 0; i < 16; i++)
			{
				ilgenerator.Emit(OpCodes.Ldloc, localBuilder3);
				ilgenerator.Emit(OpCodes.Ldc_I4, i);
				ilgenerator.Emit(OpCodes.Ldc_I4, (int)array[i]);
				ilgenerator.Emit(OpCodes.Stelem_I1);
			}
			ilgenerator.Emit(OpCodes.Ldloca, localBuilder2);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder3);
			ilgenerator.Emit(OpCodes.Call, constructor2);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbCPC);
			ilgenerator.Emit(OpCodes.Ldloca, localBuilder2);
			ilgenerator.Emit(OpCodes.Ldloca, localBuilder);
			ilgenerator.Emit(OpCodes.Callvirt, method);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Castclass, typeof(IConnectionPoint));
			ilgenerator.Emit(OpCodes.Stfld, fbEventCP);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Newobj, constructor);
			ilgenerator.Emit(OpCodes.Stfld, fbSinkHelperArray);
			ilgenerator.Emit(OpCodes.Ret);
			return methodBuilder;
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x0012BD80 File Offset: 0x0012AD80
		private void DefineConstructor(TypeBuilder OutputTypeBuilder, FieldBuilder fbCPC)
		{
			ConstructorInfo constructor = typeof(object).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
			MethodAttributes methodAttributes = MethodAttributes.SpecialName | (constructor.Attributes & MethodAttributes.MemberAccessMask);
			MethodBuilder methodBuilder = OutputTypeBuilder.DefineMethod(".ctor", methodAttributes, null, new Type[] { typeof(object) });
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Call, constructor);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldarg, 1);
			ilgenerator.Emit(OpCodes.Castclass, typeof(IConnectionPointContainer));
			ilgenerator.Emit(OpCodes.Stfld, fbCPC);
			ilgenerator.Emit(OpCodes.Ret);
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x0012BE40 File Offset: 0x0012AE40
		private MethodBuilder DefineFinalizeMethod(TypeBuilder OutputTypeBuilder, Type SinkHelperClass, FieldBuilder fbSinkHelper, FieldBuilder fbEventCP)
		{
			FieldInfo field = SinkHelperClass.GetField("m_dwCookie");
			PropertyInfo property = typeof(ArrayList).GetProperty("Item");
			MethodInfo getMethod = property.GetGetMethod();
			PropertyInfo property2 = typeof(ArrayList).GetProperty("Count");
			MethodInfo getMethod2 = property2.GetGetMethod();
			MethodInfo method = typeof(IConnectionPoint).GetMethod("Unadvise");
			MethodInfo method2 = typeof(Marshal).GetMethod("ReleaseComObject");
			Type[] array = new Type[] { typeof(object) };
			MethodInfo method3 = typeof(Monitor).GetMethod("Enter", array, null);
			array[0] = typeof(object);
			MethodInfo method4 = typeof(Monitor).GetMethod("Exit", array, null);
			MethodBuilder methodBuilder = OutputTypeBuilder.DefineMethod("Finalize", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, null, null);
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(typeof(int));
			LocalBuilder localBuilder2 = ilgenerator.DeclareLocal(typeof(int));
			LocalBuilder localBuilder3 = ilgenerator.DeclareLocal(SinkHelperClass);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Call, method3);
			ilgenerator.BeginExceptionBlock();
			Label label = ilgenerator.DefineLabel();
			Label label2 = ilgenerator.DefineLabel();
			Label label3 = ilgenerator.DefineLabel();
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbEventCP);
			ilgenerator.Emit(OpCodes.Brfalse, label3);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbSinkHelper);
			ilgenerator.Emit(OpCodes.Callvirt, getMethod2);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder);
			ilgenerator.Emit(OpCodes.Ldc_I4, 0);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Ldc_I4, 0);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Bge, label2);
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbSinkHelper);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Callvirt, getMethod);
			ilgenerator.Emit(OpCodes.Castclass, SinkHelperClass);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder3);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbEventCP);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder3);
			ilgenerator.Emit(OpCodes.Ldfld, field);
			ilgenerator.Emit(OpCodes.Callvirt, method);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Ldc_I4, 1);
			ilgenerator.Emit(OpCodes.Add);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder2);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Blt, label);
			ilgenerator.MarkLabel(label2);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Ldfld, fbEventCP);
			ilgenerator.Emit(OpCodes.Call, method2);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.MarkLabel(label3);
			ilgenerator.BeginCatchBlock(typeof(Exception));
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.BeginFinallyBlock();
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Call, method4);
			ilgenerator.EndExceptionBlock();
			ilgenerator.Emit(OpCodes.Ret);
			return methodBuilder;
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x0012C1D8 File Offset: 0x0012B1D8
		private void DefineDisposeMethod(TypeBuilder OutputTypeBuilder, MethodBuilder FinalizeMethod)
		{
			MethodInfo method = typeof(GC).GetMethod("SuppressFinalize");
			MethodBuilder methodBuilder = OutputTypeBuilder.DefineMethod("Dispose", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, null, null);
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Callvirt, FinalizeMethod);
			ilgenerator.Emit(OpCodes.Ldarg, 0);
			ilgenerator.Emit(OpCodes.Call, method);
			ilgenerator.Emit(OpCodes.Ret);
		}

		// Token: 0x04002AAD RID: 10925
		private const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

		// Token: 0x04002AAE RID: 10926
		public static readonly string GeneratedClassNamePostfix = "";

		// Token: 0x04002AAF RID: 10927
		private ModuleBuilder m_OutputModule;

		// Token: 0x04002AB0 RID: 10928
		private string m_strDestTypeName;

		// Token: 0x04002AB1 RID: 10929
		private Type m_EventItfType;

		// Token: 0x04002AB2 RID: 10930
		private Type m_SrcItfType;

		// Token: 0x04002AB3 RID: 10931
		private Type m_SinkHelperType;
	}
}
