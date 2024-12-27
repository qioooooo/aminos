using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200032E RID: 814
	[ComVisible(true)]
	[Serializable]
	public class TypeDelegator : Type
	{
		// Token: 0x06001F86 RID: 8070 RVA: 0x0004FBDB File Offset: 0x0004EBDB
		protected TypeDelegator()
		{
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x0004FBE3 File Offset: 0x0004EBE3
		public TypeDelegator(Type delegatingType)
		{
			if (delegatingType == null)
			{
				throw new ArgumentNullException("delegatingType");
			}
			this.typeImpl = delegatingType;
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001F88 RID: 8072 RVA: 0x0004FC00 File Offset: 0x0004EC00
		public override Guid GUID
		{
			get
			{
				return this.typeImpl.GUID;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001F89 RID: 8073 RVA: 0x0004FC0D File Offset: 0x0004EC0D
		public override int MetadataToken
		{
			get
			{
				return this.typeImpl.MetadataToken;
			}
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x0004FC1C File Offset: 0x0004EC1C
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			return this.typeImpl.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001F8B RID: 8075 RVA: 0x0004FC41 File Offset: 0x0004EC41
		public override Module Module
		{
			get
			{
				return this.typeImpl.Module;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001F8C RID: 8076 RVA: 0x0004FC4E File Offset: 0x0004EC4E
		public override Assembly Assembly
		{
			get
			{
				return this.typeImpl.Assembly;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001F8D RID: 8077 RVA: 0x0004FC5B File Offset: 0x0004EC5B
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				return this.typeImpl.TypeHandle;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x0004FC68 File Offset: 0x0004EC68
		public override string Name
		{
			get
			{
				return this.typeImpl.Name;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x0004FC75 File Offset: 0x0004EC75
		public override string FullName
		{
			get
			{
				return this.typeImpl.FullName;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x0004FC82 File Offset: 0x0004EC82
		public override string Namespace
		{
			get
			{
				return this.typeImpl.Namespace;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x0004FC8F File Offset: 0x0004EC8F
		public override string AssemblyQualifiedName
		{
			get
			{
				return this.typeImpl.AssemblyQualifiedName;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001F92 RID: 8082 RVA: 0x0004FC9C File Offset: 0x0004EC9C
		public override Type BaseType
		{
			get
			{
				return this.typeImpl.BaseType;
			}
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x0004FCA9 File Offset: 0x0004ECA9
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			return this.typeImpl.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0004FCBD File Offset: 0x0004ECBD
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetConstructors(bindingAttr);
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x0004FCCB File Offset: 0x0004ECCB
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (types == null)
			{
				return this.typeImpl.GetMethod(name, bindingAttr);
			}
			return this.typeImpl.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x0004FCF3 File Offset: 0x0004ECF3
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMethods(bindingAttr);
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x0004FD01 File Offset: 0x0004ED01
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetField(name, bindingAttr);
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x0004FD10 File Offset: 0x0004ED10
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetFields(bindingAttr);
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x0004FD1E File Offset: 0x0004ED1E
		public override Type GetInterface(string name, bool ignoreCase)
		{
			return this.typeImpl.GetInterface(name, ignoreCase);
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x0004FD2D File Offset: 0x0004ED2D
		public override Type[] GetInterfaces()
		{
			return this.typeImpl.GetInterfaces();
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x0004FD3A File Offset: 0x0004ED3A
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetEvent(name, bindingAttr);
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x0004FD49 File Offset: 0x0004ED49
		public override EventInfo[] GetEvents()
		{
			return this.typeImpl.GetEvents();
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x0004FD56 File Offset: 0x0004ED56
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			if (returnType == null && types == null)
			{
				return this.typeImpl.GetProperty(name, bindingAttr);
			}
			return this.typeImpl.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x0004FD82 File Offset: 0x0004ED82
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetProperties(bindingAttr);
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x0004FD90 File Offset: 0x0004ED90
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetEvents(bindingAttr);
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x0004FD9E File Offset: 0x0004ED9E
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetNestedTypes(bindingAttr);
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x0004FDAC File Offset: 0x0004EDAC
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetNestedType(name, bindingAttr);
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x0004FDBB File Offset: 0x0004EDBB
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMember(name, type, bindingAttr);
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0004FDCB File Offset: 0x0004EDCB
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMembers(bindingAttr);
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0004FDD9 File Offset: 0x0004EDD9
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.typeImpl.Attributes;
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0004FDE6 File Offset: 0x0004EDE6
		protected override bool IsArrayImpl()
		{
			return this.typeImpl.IsArray;
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0004FDF3 File Offset: 0x0004EDF3
		protected override bool IsPrimitiveImpl()
		{
			return this.typeImpl.IsPrimitive;
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0004FE00 File Offset: 0x0004EE00
		protected override bool IsByRefImpl()
		{
			return this.typeImpl.IsByRef;
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x0004FE0D File Offset: 0x0004EE0D
		protected override bool IsPointerImpl()
		{
			return this.typeImpl.IsPointer;
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x0004FE1A File Offset: 0x0004EE1A
		protected override bool IsValueTypeImpl()
		{
			return this.typeImpl.IsValueType;
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0004FE27 File Offset: 0x0004EE27
		protected override bool IsCOMObjectImpl()
		{
			return this.typeImpl.IsCOMObject;
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0004FE34 File Offset: 0x0004EE34
		public override Type GetElementType()
		{
			return this.typeImpl.GetElementType();
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0004FE41 File Offset: 0x0004EE41
		protected override bool HasElementTypeImpl()
		{
			return this.typeImpl.HasElementType;
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001FAD RID: 8109 RVA: 0x0004FE4E File Offset: 0x0004EE4E
		public override Type UnderlyingSystemType
		{
			get
			{
				return this.typeImpl.UnderlyingSystemType;
			}
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x0004FE5B File Offset: 0x0004EE5B
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.typeImpl.GetCustomAttributes(inherit);
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x0004FE69 File Offset: 0x0004EE69
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.typeImpl.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0004FE78 File Offset: 0x0004EE78
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.typeImpl.IsDefined(attributeType, inherit);
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x0004FE87 File Offset: 0x0004EE87
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			return this.typeImpl.GetInterfaceMap(interfaceType);
		}

		// Token: 0x04000D91 RID: 3473
		protected Type typeImpl;
	}
}
