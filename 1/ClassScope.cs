using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000039 RID: 57
	internal sealed class ClassScope : ActivationObject, IComparable
	{
		// Token: 0x06000264 RID: 612 RVA: 0x000146A8 File Offset: 0x000136A8
		internal ClassScope(AST name, GlobalScope scope)
			: base(scope)
		{
			this.name = name.ToString();
			this.engine = scope.engine;
			this.fast = scope.fast;
			this.noExpando = true;
			this.isKnownAtCompileTime = true;
			this.owner = null;
			this.constructors = new JSConstructor[0];
			ScriptObject scriptObject = this.engine.ScriptObjectStackTop();
			while (scriptObject is WithObject)
			{
				scriptObject = scriptObject.GetParent();
			}
			if (scriptObject is ClassScope)
			{
				this.package = ((ClassScope)scriptObject).GetPackage();
			}
			else if (scriptObject is PackageScope)
			{
				this.package = (PackageScope)scriptObject;
			}
			else
			{
				this.package = null;
			}
			this.itemProp = null;
			this.outerClassField = null;
			this.inStaticInitializerCode = false;
			this.staticInitializerUsesEval = false;
			this.instanceInitializerUsesEval = false;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0001477C File Offset: 0x0001377C
		internal void AddClassesFromInheritanceChain(string name, ArrayList result)
		{
			IReflect reflect = this;
			bool flag = true;
			while (reflect is ClassScope)
			{
				if (reflect.GetMember(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length > 0)
				{
					result.Add(reflect);
					flag = false;
				}
				if (reflect is ClassScope)
				{
					reflect = ((ClassScope)reflect).GetSuperType();
				}
				else
				{
					reflect = ((Type)reflect).BaseType;
				}
			}
			if (flag && reflect is Type && reflect.GetMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length > 0)
			{
				result.Add(reflect);
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000147F4 File Offset: 0x000137F4
		internal static ClassScope ScopeOfClassMemberInitializer(ScriptObject scope)
		{
			while (scope != null)
			{
				if (scope is FunctionScope)
				{
					return null;
				}
				ClassScope classScope = scope as ClassScope;
				if (classScope != null)
				{
					return classScope;
				}
				scope = scope.GetParent();
			}
			return null;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00014825 File Offset: 0x00013825
		public int CompareTo(object ob)
		{
			if (ob == this)
			{
				return 0;
			}
			if (ob is ClassScope && ((ClassScope)ob).IsSameOrDerivedFrom(this))
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00014846 File Offset: 0x00013846
		protected override JSVariableField CreateField(string name, FieldAttributes attributeFlags, object value)
		{
			return new JSMemberField(this, name, value, attributeFlags);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00014854 File Offset: 0x00013854
		internal object FakeCallToTypeMethod(MethodInfo method, object[] arguments, Exception e)
		{
			ParameterInfo[] parameters = method.GetParameters();
			int num = parameters.Length;
			Type[] array = new Type[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			MethodInfo method2 = typeof(ClassScope).GetMethod(method.Name, array);
			if (method2 != null)
			{
				return method2.Invoke(this, arguments);
			}
			throw e;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000148B4 File Offset: 0x000138B4
		public object[] GetCustomAttributes(bool inherit)
		{
			CustomAttributeList customAttributes = this.owner.customAttributes;
			if (customAttributes == null)
			{
				return new object[0];
			}
			return (object[])customAttributes.Evaluate();
		}

		// Token: 0x0600026B RID: 619 RVA: 0x000148E2 File Offset: 0x000138E2
		public ConstructorInfo[] GetConstructors()
		{
			return this.constructors;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x000148EA File Offset: 0x000138EA
		public FieldInfo GetField(string name)
		{
			return base.GetField(name, BindingFlags.Instance | BindingFlags.Public);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x000148F5 File Offset: 0x000138F5
		public MethodInfo GetMethod(string name)
		{
			return base.GetMethod(name, BindingFlags.Instance | BindingFlags.Public);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00014900 File Offset: 0x00013900
		public PropertyInfo GetProperty(string name)
		{
			return base.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0001490B File Offset: 0x0001390B
		internal override object GetDefaultValue(PreferredType preferred_type)
		{
			return this.GetFullName();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00014913 File Offset: 0x00013913
		internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00014918 File Offset: 0x00013918
		internal string GetFullName()
		{
			PackageScope packageScope = this.GetPackage();
			if (packageScope != null)
			{
				return packageScope.GetName() + "." + this.name;
			}
			if (this.owner.enclosingScope is ClassScope)
			{
				return ((ClassScope)this.owner.enclosingScope).GetFullName() + "." + this.name;
			}
			return this.name;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00014984 File Offset: 0x00013984
		public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			MemberInfoList memberInfoList = new MemberInfoList();
			FieldInfo fieldInfo = (FieldInfo)this.name_table[name];
			if (fieldInfo != null)
			{
				if (fieldInfo.IsPublic)
				{
					if ((bindingAttr & BindingFlags.Public) == BindingFlags.Default)
					{
						goto IL_012D;
					}
				}
				else if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
				{
					goto IL_012D;
				}
				if (fieldInfo.IsLiteral)
				{
					object value = ((JSMemberField)fieldInfo).value;
					if (value is FunctionObject)
					{
						FunctionObject functionObject = (FunctionObject)value;
						if (functionObject.isConstructor)
						{
							return new MemberInfo[0];
						}
						if (!functionObject.isExpandoMethod)
						{
							((JSMemberField)fieldInfo).AddOverloadedMembers(memberInfoList, this, bindingAttr | BindingFlags.DeclaredOnly);
							goto IL_012D;
						}
						if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
						{
							memberInfoList.Add(fieldInfo);
							goto IL_012D;
						}
						goto IL_012D;
					}
					else
					{
						if (value is JSProperty)
						{
							JSProperty jsproperty = (JSProperty)value;
							MethodInfo methodInfo = ((jsproperty.getter != null) ? jsproperty.getter : jsproperty.setter);
							if (methodInfo.IsStatic)
							{
								if ((bindingAttr & BindingFlags.Static) == BindingFlags.Default)
								{
									goto IL_012D;
								}
							}
							else if ((bindingAttr & BindingFlags.Instance) == BindingFlags.Default)
							{
								goto IL_012D;
							}
							memberInfoList.Add(jsproperty);
							goto IL_012D;
						}
						if (value is ClassScope && (bindingAttr & BindingFlags.Instance) != BindingFlags.Default && !((ClassScope)value).owner.isStatic)
						{
							memberInfoList.Add(fieldInfo);
							goto IL_012D;
						}
					}
				}
				if (fieldInfo.IsStatic)
				{
					if ((bindingAttr & BindingFlags.Static) == BindingFlags.Default)
					{
						goto IL_012D;
					}
				}
				else if ((bindingAttr & BindingFlags.Instance) == BindingFlags.Default)
				{
					goto IL_012D;
				}
				memberInfoList.Add(fieldInfo);
			}
			IL_012D:
			if (this.owner != null && this.owner.isInterface && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default)
			{
				return this.owner.GetInterfaceMember(name);
			}
			if (this.parent != null && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default)
			{
				MemberInfo[] member = this.parent.GetMember(name, bindingAttr);
				if (member != null)
				{
					foreach (MemberInfo memberInfo in member)
					{
						if (memberInfo.MemberType == MemberTypes.Field)
						{
							fieldInfo = (FieldInfo)memberInfo;
							if (!fieldInfo.IsStatic && !fieldInfo.IsLiteral && !(fieldInfo is JSWrappedField))
							{
								fieldInfo = new JSWrappedField(fieldInfo, this.parent);
							}
							memberInfoList.Add(fieldInfo);
						}
						else
						{
							memberInfoList.Add(ScriptObject.WrapMember(memberInfo, this.parent));
						}
					}
				}
			}
			return memberInfoList.ToArray();
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00014B84 File Offset: 0x00013B84
		internal bool HasInstance(object ob)
		{
			if (!(ob is JSObject))
			{
				return false;
			}
			for (ScriptObject scriptObject = ((JSObject)ob).GetParent(); scriptObject != null; scriptObject = scriptObject.GetParent())
			{
				if (scriptObject == this)
				{
					return true;
				}
				if (scriptObject is WithObject)
				{
					object contained_object = ((WithObject)scriptObject).contained_object;
					if (contained_object == this)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00014BD4 File Offset: 0x00013BD4
		internal JSMemberField[] GetMemberFields()
		{
			int count = this.field_table.Count;
			JSMemberField[] array = new JSMemberField[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = (JSMemberField)this.field_table[i];
			}
			return array;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00014C18 File Offset: 0x00013C18
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			MemberInfoList memberInfoList = new MemberInfoList();
			foreach (object obj in this.field_table)
			{
				FieldInfo fieldInfo = (FieldInfo)obj;
				if (fieldInfo.IsLiteral && fieldInfo is JSMemberField)
				{
					object value;
					if ((value = ((JSMemberField)fieldInfo).value) is FunctionObject)
					{
						if (!((FunctionObject)value).isConstructor)
						{
							((JSMemberField)fieldInfo).AddOverloadedMembers(memberInfoList, this, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						}
					}
					else if (value is JSProperty)
					{
						memberInfoList.Add((MemberInfo)value);
					}
					else
					{
						memberInfoList.Add(fieldInfo);
					}
				}
				else
				{
					memberInfoList.Add(fieldInfo);
				}
			}
			if (this.parent != null)
			{
				memberInfoList.AddRange(this.parent.GetMembers(bindingAttr));
			}
			return memberInfoList.ToArray();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00014CD7 File Offset: 0x00013CD7
		internal override string GetName()
		{
			return this.name;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00014CE0 File Offset: 0x00013CE0
		internal Type GetBakedSuperType()
		{
			this.owner.PartiallyEvaluate();
			if (this.owner is EnumDeclaration)
			{
				return ((EnumDeclaration)this.owner).baseType.ToType();
			}
			object contained_object = ((WithObject)this.parent).contained_object;
			if (contained_object is ClassScope)
			{
				return ((ClassScope)contained_object).GetBakedSuperType();
			}
			if (contained_object is Type)
			{
				return (Type)contained_object;
			}
			return Globals.TypeRefs.ToReferenceContext(contained_object.GetType());
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00014D60 File Offset: 0x00013D60
		internal PackageScope GetPackage()
		{
			return this.package;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00014D68 File Offset: 0x00013D68
		internal IReflect GetSuperType()
		{
			this.owner.PartiallyEvaluate();
			return (IReflect)((WithObject)this.parent).contained_object;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00014D8B File Offset: 0x00013D8B
		internal Type GetTypeBuilderOrEnumBuilder()
		{
			if (this.classwriter == null)
			{
				this.classwriter = this.owner.GetTypeBuilderOrEnumBuilder();
			}
			return this.classwriter;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00014DAC File Offset: 0x00013DAC
		internal TypeBuilder GetTypeBuilder()
		{
			return (TypeBuilder)this.GetTypeBuilderOrEnumBuilder();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00014DB9 File Offset: 0x00013DB9
		internal IReflect GetUnderlyingTypeIfEnum()
		{
			if (this.owner is EnumDeclaration)
			{
				return ((EnumDeclaration)this.owner.PartiallyEvaluate()).baseType.ToIReflect();
			}
			return this;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00014DE4 File Offset: 0x00013DE4
		internal bool ImplementsInterface(IReflect iface)
		{
			this.owner.PartiallyEvaluate();
			object contained_object = ((WithObject)this.parent).contained_object;
			if (contained_object is ClassScope)
			{
				return ((ClassScope)contained_object).ImplementsInterface(iface) || this.owner.ImplementsInterface(iface);
			}
			if (contained_object is Type && iface is Type)
			{
				return ((Type)iface).IsAssignableFrom((Type)contained_object) || this.owner.ImplementsInterface(iface);
			}
			return this.owner.ImplementsInterface(iface);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00014E74 File Offset: 0x00013E74
		internal bool IsCLSCompliant()
		{
			this.owner.PartiallyEvaluate();
			TypeAttributes typeAttributes = this.owner.attributes & TypeAttributes.VisibilityMask;
			if (typeAttributes != TypeAttributes.Public && typeAttributes != TypeAttributes.NestedPublic)
			{
				return false;
			}
			if (this.owner.clsCompliance == CLSComplianceSpec.NotAttributed)
			{
				return this.owner.Engine.isCLSCompliant;
			}
			return this.owner.clsCompliance == CLSComplianceSpec.CLSCompliant;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00014ED4 File Offset: 0x00013ED4
		internal bool IsNestedIn(ClassScope other, bool isStatic)
		{
			if (this.parent == null)
			{
				return false;
			}
			this.owner.PartiallyEvaluate();
			if (this.owner.enclosingScope == other)
			{
				return isStatic || !this.owner.isStatic;
			}
			return this.owner.enclosingScope is ClassScope && ((ClassScope)this.owner.enclosingScope).IsNestedIn(other, isStatic);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00014F44 File Offset: 0x00013F44
		internal bool IsSameOrDerivedFrom(ClassScope other)
		{
			if (this == other)
			{
				return true;
			}
			if (other.owner.isInterface)
			{
				return this.ImplementsInterface(other);
			}
			if (this.parent == null)
			{
				return false;
			}
			this.owner.PartiallyEvaluate();
			object contained_object = ((WithObject)this.parent).contained_object;
			return contained_object is ClassScope && ((ClassScope)contained_object).IsSameOrDerivedFrom(other);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00014FAC File Offset: 0x00013FAC
		internal bool IsSameOrDerivedFrom(Type other)
		{
			if (this.owner.GetTypeBuilder() == other)
			{
				return true;
			}
			if (this.parent == null)
			{
				return false;
			}
			this.owner.PartiallyEvaluate();
			object contained_object = ((WithObject)this.parent).contained_object;
			if (contained_object is ClassScope)
			{
				return ((ClassScope)contained_object).IsSameOrDerivedFrom(other);
			}
			return other.IsAssignableFrom((Type)contained_object);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00015014 File Offset: 0x00014014
		internal bool IsPromotableTo(Type other)
		{
			Type bakedSuperType = this.GetBakedSuperType();
			if (other.IsAssignableFrom(bakedSuperType))
			{
				return true;
			}
			if (other.IsInterface && this.ImplementsInterface(other))
			{
				return true;
			}
			EnumDeclaration enumDeclaration = this.owner as EnumDeclaration;
			return enumDeclaration != null && Convert.IsPromotableTo(enumDeclaration.baseType.ToType(), other);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0001506C File Offset: 0x0001406C
		internal bool ParentIsInSamePackage()
		{
			object contained_object = ((WithObject)this.parent).contained_object;
			return contained_object is ClassScope && ((ClassScope)contained_object).package == this.package;
		}

		// Token: 0x04000172 RID: 370
		internal string name;

		// Token: 0x04000173 RID: 371
		internal Type classwriter;

		// Token: 0x04000174 RID: 372
		internal Class owner;

		// Token: 0x04000175 RID: 373
		internal ConstructorInfo[] constructors;

		// Token: 0x04000176 RID: 374
		internal bool noExpando;

		// Token: 0x04000177 RID: 375
		internal PackageScope package;

		// Token: 0x04000178 RID: 376
		internal JSProperty itemProp;

		// Token: 0x04000179 RID: 377
		internal FieldInfo outerClassField;

		// Token: 0x0400017A RID: 378
		internal bool inStaticInitializerCode;

		// Token: 0x0400017B RID: 379
		internal bool staticInitializerUsesEval;

		// Token: 0x0400017C RID: 380
		internal bool instanceInitializerUsesEval;
	}
}
