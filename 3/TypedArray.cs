using System;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Microsoft.JScript
{
	// Token: 0x02000122 RID: 290
	public sealed class TypedArray : IReflect
	{
		// Token: 0x06000BD8 RID: 3032 RVA: 0x0005A7D8 File Offset: 0x000597D8
		public TypedArray(IReflect elementType, int rank)
		{
			this.elementType = elementType;
			this.rank = rank;
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0005A7F0 File Offset: 0x000597F0
		public override bool Equals(object obj)
		{
			if (obj is TypedArray)
			{
				return this.ToString().Equals(obj.ToString());
			}
			Type type = obj as Type;
			return type != null && type.IsArray && type.GetArrayRank() == this.rank && this.elementType.Equals(type.GetElementType());
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0005A84E File Offset: 0x0005984E
		public FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			return Typeob.Array.GetField(name, bindingAttr);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0005A85C File Offset: 0x0005985C
		public FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return Typeob.Array.GetFields(bindingAttr);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x0005A869 File Offset: 0x00059869
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x0005A876 File Offset: 0x00059876
		public MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			return Typeob.Array.GetMember(name, bindingAttr);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x0005A884 File Offset: 0x00059884
		public MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			return Typeob.Array.GetMembers(bindingAttr);
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x0005A891 File Offset: 0x00059891
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
		{
			return Typeob.Array.GetMethod(name, bindingAttr);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0005A89F File Offset: 0x0005989F
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return Typeob.Array.GetMethod(name, bindingAttr, binder, types, modifiers);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0005A8B2 File Offset: 0x000598B2
		public MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return Typeob.Array.GetMethods(bindingAttr);
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0005A8BF File Offset: 0x000598BF
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
		{
			return Typeob.Array.GetProperty(name, bindingAttr);
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0005A8CD File Offset: 0x000598CD
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return Typeob.Array.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0005A8E2 File Offset: 0x000598E2
		public PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return Typeob.Array.GetProperties(bindingAttr);
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0005A8F0 File Offset: 0x000598F0
		public object InvokeMember(string name, BindingFlags flags, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo locale, string[] namedParameters)
		{
			if ((flags & BindingFlags.CreateInstance) == BindingFlags.Default)
			{
				return LateBinding.CallValue(this.elementType, args, true, true, null, null, binder, locale, namedParameters);
			}
			return Typeob.Array.InvokeMember(name, flags, binder, target, args, modifiers, locale, namedParameters);
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0005A934 File Offset: 0x00059934
		internal static string ToRankString(int rank)
		{
			switch (rank)
			{
			case 1:
				return "[]";
			case 2:
				return "[,]";
			case 3:
				return "[,,]";
			default:
			{
				StringBuilder stringBuilder = new StringBuilder(rank + 1);
				stringBuilder.Append('[');
				for (int i = 1; i < rank; i++)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(']');
				return stringBuilder.ToString();
			}
			}
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0005A9A4 File Offset: 0x000599A4
		public override string ToString()
		{
			Type type = this.elementType as Type;
			if (type != null)
			{
				return type.FullName + TypedArray.ToRankString(this.rank);
			}
			ClassScope classScope = this.elementType as ClassScope;
			if (classScope != null)
			{
				return classScope.GetFullName() + TypedArray.ToRankString(this.rank);
			}
			TypedArray typedArray = this.elementType as TypedArray;
			if (typedArray != null)
			{
				return typedArray.ToString() + TypedArray.ToRankString(this.rank);
			}
			return Convert.ToType(this.elementType).FullName + TypedArray.ToRankString(this.rank);
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0005AA44 File Offset: 0x00059A44
		internal Type ToType()
		{
			Type type = Convert.ToType(this.elementType);
			return Convert.ToType(TypedArray.ToRankString(this.rank), type);
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x0005AA6E File Offset: 0x00059A6E
		public Type UnderlyingSystemType
		{
			get
			{
				return base.GetType();
			}
		}

		// Token: 0x04000710 RID: 1808
		internal IReflect elementType;

		// Token: 0x04000711 RID: 1809
		internal int rank;
	}
}
