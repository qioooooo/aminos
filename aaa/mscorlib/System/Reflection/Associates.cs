using System;
using System.Collections;

namespace System.Reflection
{
	// Token: 0x0200033D RID: 829
	internal static class Associates
	{
		// Token: 0x060020EC RID: 8428 RVA: 0x000527C8 File Offset: 0x000517C8
		internal static bool IncludeAccessor(MethodInfo associate, bool nonPublic)
		{
			return associate != null && (nonPublic || associate.IsPublic);
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000527E0 File Offset: 0x000517E0
		internal static RuntimeMethodInfo AssignAssociates(int tkMethod, RuntimeTypeHandle declaredTypeHandle, RuntimeTypeHandle reflectedTypeHandle)
		{
			if (MetadataToken.IsNullToken(tkMethod))
			{
				return null;
			}
			bool flag = !declaredTypeHandle.Equals(reflectedTypeHandle);
			RuntimeMethodHandle runtimeMethodHandle = declaredTypeHandle.GetModuleHandle().ResolveMethodHandle(tkMethod, declaredTypeHandle.GetInstantiation(), new RuntimeTypeHandle[0]);
			MethodAttributes attributes = runtimeMethodHandle.GetAttributes();
			bool flag2 = (attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;
			bool flag3 = (attributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope;
			if (flag)
			{
				if (flag2)
				{
					return null;
				}
				if (flag3)
				{
					bool flag4 = (declaredTypeHandle.GetAttributes() & TypeAttributes.ClassSemanticsMask) == TypeAttributes.NotPublic;
					if (flag4)
					{
						int slot = runtimeMethodHandle.GetSlot();
						runtimeMethodHandle = reflectedTypeHandle.GetMethodAt(slot);
					}
				}
			}
			MethodAttributes methodAttributes = attributes & MethodAttributes.MemberAccessMask;
			RuntimeMethodInfo runtimeMethodInfo = RuntimeType.GetMethodBase(reflectedTypeHandle, runtimeMethodHandle) as RuntimeMethodInfo;
			if (runtimeMethodInfo == null)
			{
				runtimeMethodInfo = reflectedTypeHandle.GetRuntimeType().Module.ResolveMethod(tkMethod, null, null) as RuntimeMethodInfo;
			}
			return runtimeMethodInfo;
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x000528A4 File Offset: 0x000518A4
		internal unsafe static void AssignAssociates(AssociateRecord* associates, int cAssociates, RuntimeTypeHandle declaringTypeHandle, RuntimeTypeHandle reflectedTypeHandle, out RuntimeMethodInfo addOn, out RuntimeMethodInfo removeOn, out RuntimeMethodInfo fireOn, out RuntimeMethodInfo getter, out RuntimeMethodInfo setter, out MethodInfo[] other, out bool composedOfAllPrivateMethods, out BindingFlags bindingFlags)
		{
			RuntimeMethodInfo runtimeMethodInfo;
			setter = (runtimeMethodInfo = null);
			RuntimeMethodInfo runtimeMethodInfo2;
			getter = (runtimeMethodInfo2 = runtimeMethodInfo);
			RuntimeMethodInfo runtimeMethodInfo3;
			fireOn = (runtimeMethodInfo3 = runtimeMethodInfo2);
			RuntimeMethodInfo runtimeMethodInfo4;
			removeOn = (runtimeMethodInfo4 = runtimeMethodInfo3);
			addOn = runtimeMethodInfo4;
			other = null;
			Associates.Attributes attributes = Associates.Attributes.ComposedOfAllVirtualMethods | Associates.Attributes.ComposedOfAllPrivateMethods | Associates.Attributes.ComposedOfNoPublicMembers | Associates.Attributes.ComposedOfNoStaticMembers;
			while (reflectedTypeHandle.IsGenericVariable())
			{
				reflectedTypeHandle = reflectedTypeHandle.GetRuntimeType().BaseType.GetTypeHandleInternal();
			}
			bool flag = !declaringTypeHandle.Equals(reflectedTypeHandle);
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < cAssociates; i++)
			{
				RuntimeMethodInfo runtimeMethodInfo5 = Associates.AssignAssociates(associates[i].MethodDefToken, declaringTypeHandle, reflectedTypeHandle);
				if (runtimeMethodInfo5 != null)
				{
					MethodAttributes attributes2 = runtimeMethodInfo5.Attributes;
					bool flag2 = (attributes2 & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;
					bool flag3 = (attributes2 & MethodAttributes.Virtual) != MethodAttributes.PrivateScope;
					MethodAttributes methodAttributes = attributes2 & MethodAttributes.MemberAccessMask;
					bool flag4 = methodAttributes == MethodAttributes.Public;
					bool flag5 = (attributes2 & MethodAttributes.Static) != MethodAttributes.PrivateScope;
					if (flag4)
					{
						attributes &= ~Associates.Attributes.ComposedOfNoPublicMembers;
						attributes &= ~Associates.Attributes.ComposedOfAllPrivateMethods;
					}
					else if (!flag2)
					{
						attributes &= ~Associates.Attributes.ComposedOfAllPrivateMethods;
					}
					if (flag5)
					{
						attributes &= ~Associates.Attributes.ComposedOfNoStaticMembers;
					}
					if (!flag3)
					{
						attributes &= ~Associates.Attributes.ComposedOfAllVirtualMethods;
					}
					if (associates[i].Semantics == MethodSemanticsAttributes.Setter)
					{
						setter = runtimeMethodInfo5;
					}
					else if (associates[i].Semantics == MethodSemanticsAttributes.Getter)
					{
						getter = runtimeMethodInfo5;
					}
					else if (associates[i].Semantics == MethodSemanticsAttributes.Fire)
					{
						fireOn = runtimeMethodInfo5;
					}
					else if (associates[i].Semantics == MethodSemanticsAttributes.AddOn)
					{
						addOn = runtimeMethodInfo5;
					}
					else if (associates[i].Semantics == MethodSemanticsAttributes.RemoveOn)
					{
						removeOn = runtimeMethodInfo5;
					}
					else
					{
						arrayList.Add(runtimeMethodInfo5);
					}
				}
			}
			bool flag6 = (attributes & Associates.Attributes.ComposedOfNoPublicMembers) == (Associates.Attributes)0;
			bool flag7 = (attributes & Associates.Attributes.ComposedOfNoStaticMembers) == (Associates.Attributes)0;
			bindingFlags = RuntimeType.FilterPreCalculate(flag6, flag, flag7);
			composedOfAllPrivateMethods = (attributes & Associates.Attributes.ComposedOfAllPrivateMethods) != (Associates.Attributes)0;
			other = (MethodInfo[])arrayList.ToArray(typeof(MethodInfo));
		}

		// Token: 0x0200033E RID: 830
		[Flags]
		internal enum Attributes
		{
			// Token: 0x04000DBA RID: 3514
			ComposedOfAllVirtualMethods = 1,
			// Token: 0x04000DBB RID: 3515
			ComposedOfAllPrivateMethods = 2,
			// Token: 0x04000DBC RID: 3516
			ComposedOfNoPublicMembers = 4,
			// Token: 0x04000DBD RID: 3517
			ComposedOfNoStaticMembers = 8
		}
	}
}
