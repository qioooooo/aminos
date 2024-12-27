using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x0200007E RID: 126
	public abstract class FieldAccessor
	{
		// Token: 0x060005AE RID: 1454
		[DebuggerStepThrough]
		[DebuggerHidden]
		public abstract object GetValue(object thisob);

		// Token: 0x060005AF RID: 1455
		[DebuggerHidden]
		[DebuggerStepThrough]
		public abstract void SetValue(object thisob, object value);

		// Token: 0x060005B0 RID: 1456 RVA: 0x000278C0 File Offset: 0x000268C0
		internal static FieldAccessor GetAccessorFor(FieldInfo field)
		{
			FieldAccessor fieldAccessor = FieldAccessor.accessorFor[field] as FieldAccessor;
			if (fieldAccessor != null)
			{
				return fieldAccessor;
			}
			lock (FieldAccessor.accessorFor)
			{
				fieldAccessor = FieldAccessor.accessorFor[field] as FieldAccessor;
				if (fieldAccessor != null)
				{
					return fieldAccessor;
				}
				fieldAccessor = FieldAccessor.SpitAndInstantiateClassFor(field);
				FieldAccessor.accessorFor[field] = fieldAccessor;
			}
			return fieldAccessor;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00027938 File Offset: 0x00026938
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		private static FieldAccessor SpitAndInstantiateClassFor(FieldInfo field)
		{
			Type fieldType = field.FieldType;
			TypeBuilder typeBuilder = Runtime.ThunkModuleBuilder.DefineType("accessor" + FieldAccessor.count++, TypeAttributes.Public, typeof(FieldAccessor));
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetValue", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, typeof(object), new Type[] { typeof(object) });
			methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(Runtime.TypeRefs.debuggerStepThroughAttributeCtor, new object[0]));
			methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(Runtime.TypeRefs.debuggerHiddenAttributeCtor, new object[0]));
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			if (field.IsLiteral)
			{
				new ConstantWrapper(TypeReferences.GetConstantValue(field), null).TranslateToIL(ilgenerator, fieldType);
			}
			else if (field.IsStatic)
			{
				ilgenerator.Emit(OpCodes.Ldsfld, field);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Ldfld, field);
			}
			if (fieldType.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Box, fieldType);
			}
			ilgenerator.Emit(OpCodes.Ret);
			methodBuilder = typeBuilder.DefineMethod("SetValue", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, typeof(void), new Type[]
			{
				typeof(object),
				typeof(object)
			});
			methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(Runtime.TypeRefs.debuggerStepThroughAttributeCtor, new object[0]));
			methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(Runtime.TypeRefs.debuggerHiddenAttributeCtor, new object[0]));
			ilgenerator = methodBuilder.GetILGenerator();
			if (!field.IsLiteral)
			{
				if (!field.IsStatic)
				{
					ilgenerator.Emit(OpCodes.Ldarg_1);
				}
				ilgenerator.Emit(OpCodes.Ldarg_2);
				if (fieldType.IsValueType)
				{
					Convert.EmitUnbox(ilgenerator, fieldType, Type.GetTypeCode(fieldType));
				}
				if (field.IsStatic)
				{
					ilgenerator.Emit(OpCodes.Stsfld, field);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Stfld, field);
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			Type type = typeBuilder.CreateType();
			return (FieldAccessor)Activator.CreateInstance(type);
		}

		// Token: 0x04000273 RID: 627
		private static SimpleHashtable accessorFor = new SimpleHashtable(32U);

		// Token: 0x04000274 RID: 628
		private static int count = 0;
	}
}
