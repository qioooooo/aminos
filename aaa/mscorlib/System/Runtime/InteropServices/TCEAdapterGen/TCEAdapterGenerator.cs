using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Runtime.InteropServices.TCEAdapterGen
{
	// Token: 0x020008E1 RID: 2273
	internal class TCEAdapterGenerator
	{
		// Token: 0x060052D9 RID: 21209 RVA: 0x0012C840 File Offset: 0x0012B840
		public void Process(ModuleBuilder ModBldr, ArrayList EventItfList)
		{
			this.m_Module = ModBldr;
			int count = EventItfList.Count;
			for (int i = 0; i < count; i++)
			{
				EventItfInfo eventItfInfo = (EventItfInfo)EventItfList[i];
				Type eventItfType = eventItfInfo.GetEventItfType();
				Type srcItfType = eventItfInfo.GetSrcItfType();
				string eventProviderName = eventItfInfo.GetEventProviderName();
				Type type = new EventSinkHelperWriter(this.m_Module, srcItfType, eventItfType).Perform();
				new EventProviderWriter(this.m_Module, eventProviderName, eventItfType, srcItfType, type).Perform();
			}
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x0012C8B8 File Offset: 0x0012B8B8
		internal static void SetClassInterfaceTypeToNone(TypeBuilder tb)
		{
			if (TCEAdapterGenerator.s_NoClassItfCABuilder == null)
			{
				Type[] array = new Type[] { typeof(ClassInterfaceType) };
				ConstructorInfo constructor = typeof(ClassInterfaceAttribute).GetConstructor(array);
				TCEAdapterGenerator.s_NoClassItfCABuilder = new CustomAttributeBuilder(constructor, new object[] { ClassInterfaceType.None });
			}
			tb.SetCustomAttribute(TCEAdapterGenerator.s_NoClassItfCABuilder);
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x0012C918 File Offset: 0x0012B918
		internal static TypeBuilder DefineUniqueType(string strInitFullName, TypeAttributes attrs, Type BaseType, Type[] aInterfaceTypes, ModuleBuilder mb)
		{
			string text = strInitFullName;
			int num = 2;
			while (mb.GetType(text) != null)
			{
				text = strInitFullName + "_" + num;
				num++;
			}
			return mb.DefineType(text, attrs, BaseType, aInterfaceTypes);
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x0012C958 File Offset: 0x0012B958
		internal static void SetHiddenAttribute(TypeBuilder tb)
		{
			if (TCEAdapterGenerator.s_HiddenCABuilder == null)
			{
				Type[] array = new Type[] { typeof(TypeLibTypeFlags) };
				ConstructorInfo constructor = typeof(TypeLibTypeAttribute).GetConstructor(array);
				TCEAdapterGenerator.s_HiddenCABuilder = new CustomAttributeBuilder(constructor, new object[] { TypeLibTypeFlags.FHidden });
			}
			tb.SetCustomAttribute(TCEAdapterGenerator.s_HiddenCABuilder);
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x0012C9BC File Offset: 0x0012B9BC
		internal static MethodInfo[] GetNonPropertyMethods(Type type)
		{
			MethodInfo[] methods = type.GetMethods();
			ArrayList arrayList = new ArrayList(methods);
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				MethodInfo[] accessors = propertyInfo.GetAccessors();
				foreach (MethodInfo methodInfo in accessors)
				{
					for (int k = 0; k < arrayList.Count; k++)
					{
						if ((MethodInfo)arrayList[k] == methodInfo)
						{
							arrayList.RemoveAt(k);
						}
					}
				}
			}
			MethodInfo[] array3 = new MethodInfo[arrayList.Count];
			arrayList.CopyTo(array3);
			return array3;
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x0012CA68 File Offset: 0x0012BA68
		internal static MethodInfo[] GetPropertyMethods(Type type)
		{
			type.GetMethods();
			ArrayList arrayList = new ArrayList();
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				MethodInfo[] accessors = propertyInfo.GetAccessors();
				foreach (MethodInfo methodInfo in accessors)
				{
					arrayList.Add(methodInfo);
				}
			}
			MethodInfo[] array3 = new MethodInfo[arrayList.Count];
			arrayList.CopyTo(array3);
			return array3;
		}

		// Token: 0x04002ABE RID: 10942
		private ModuleBuilder m_Module;

		// Token: 0x04002ABF RID: 10943
		private Hashtable m_SrcItfToSrcItfInfoMap = new Hashtable();

		// Token: 0x04002AC0 RID: 10944
		private static CustomAttributeBuilder s_NoClassItfCABuilder;

		// Token: 0x04002AC1 RID: 10945
		private static CustomAttributeBuilder s_HiddenCABuilder;
	}
}
