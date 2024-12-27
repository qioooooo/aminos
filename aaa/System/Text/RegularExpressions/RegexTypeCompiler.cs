using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200001B RID: 27
	internal class RegexTypeCompiler : RegexCompiler
	{
		// Token: 0x06000125 RID: 293 RVA: 0x0000A0DC File Offset: 0x000090DC
		internal RegexTypeCompiler(AssemblyName an, CustomAttributeBuilder[] attribs, string resourceFile, Evidence evidence)
		{
			new ReflectionPermission(PermissionState.Unrestricted).Assert();
			try
			{
				this._assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndSave, evidence);
				this._module = this._assembly.DefineDynamicModule(an.Name + ".dll");
				if (attribs != null)
				{
					for (int i = 0; i < attribs.Length; i++)
					{
						this._assembly.SetCustomAttribute(attribs[i]);
					}
				}
				if (resourceFile != null)
				{
					this._assembly.DefineUnmanagedResource(resourceFile);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000A178 File Offset: 0x00009178
		internal Type FactoryTypeFromCode(RegexCode code, RegexOptions options, string typeprefix)
		{
			this._code = code;
			this._codes = code._codes;
			this._strings = code._strings;
			this._fcPrefix = code._fcPrefix;
			this._bmPrefix = code._bmPrefix;
			this._anchors = code._anchors;
			this._trackcount = code._trackcount;
			this._options = options;
			string text = Interlocked.Increment(ref RegexTypeCompiler._typeCount).ToString(CultureInfo.InvariantCulture);
			string text2 = typeprefix + "Runner" + text;
			string text3 = typeprefix + "Factory" + text;
			this.DefineType(text2, false, typeof(RegexRunner));
			this.DefineMethod("Go", null);
			base.GenerateGo();
			this.BakeMethod();
			this.DefineMethod("FindFirstChar", typeof(bool));
			base.GenerateFindFirstChar();
			this.BakeMethod();
			this.DefineMethod("InitTrackCount", null);
			base.GenerateInitTrackCount();
			this.BakeMethod();
			Type type = this.BakeType();
			this.DefineType(text3, false, typeof(RegexRunnerFactory));
			this.DefineMethod("CreateInstance", typeof(RegexRunner));
			this.GenerateCreateInstance(type);
			this.BakeMethod();
			return this.BakeType();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000A2B8 File Offset: 0x000092B8
		internal void GenerateRegexType(string pattern, RegexOptions opts, string name, bool ispublic, RegexCode code, RegexTree tree, Type factory)
		{
			FieldInfo fieldInfo = this.RegexField("pattern");
			FieldInfo fieldInfo2 = this.RegexField("roptions");
			FieldInfo fieldInfo3 = this.RegexField("factory");
			FieldInfo fieldInfo4 = this.RegexField("caps");
			FieldInfo fieldInfo5 = this.RegexField("capnames");
			FieldInfo fieldInfo6 = this.RegexField("capslist");
			FieldInfo fieldInfo7 = this.RegexField("capsize");
			Type[] array = new Type[0];
			this.DefineType(name, ispublic, typeof(Regex));
			this._methbuilder = null;
			MethodAttributes methodAttributes = MethodAttributes.Public;
			ConstructorBuilder constructorBuilder = this._typebuilder.DefineConstructor(methodAttributes, CallingConventions.Standard, array);
			this._ilg = constructorBuilder.GetILGenerator();
			base.Ldthis();
			this._ilg.Emit(OpCodes.Call, typeof(Regex).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], new ParameterModifier[0]));
			base.Ldthis();
			base.Ldstr(pattern);
			base.Stfld(fieldInfo);
			base.Ldthis();
			base.Ldc((int)opts);
			base.Stfld(fieldInfo2);
			base.Ldthis();
			base.Newobj(factory.GetConstructor(array));
			base.Stfld(fieldInfo3);
			if (code._caps != null)
			{
				this.GenerateCreateHashtable(fieldInfo4, code._caps);
			}
			if (tree._capnames != null)
			{
				this.GenerateCreateHashtable(fieldInfo5, tree._capnames);
			}
			if (tree._capslist != null)
			{
				base.Ldthis();
				base.Ldc(tree._capslist.Length);
				this._ilg.Emit(OpCodes.Newarr, typeof(string));
				base.Stfld(fieldInfo6);
				for (int i = 0; i < tree._capslist.Length; i++)
				{
					base.Ldthisfld(fieldInfo6);
					base.Ldc(i);
					base.Ldstr(tree._capslist[i]);
					this._ilg.Emit(OpCodes.Stelem_Ref);
				}
			}
			base.Ldthis();
			base.Ldc(code._capsize);
			base.Stfld(fieldInfo7);
			base.Ldthis();
			base.Call(typeof(Regex).GetMethod("InitializeReferences", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
			base.Ret();
			this._typebuilder.CreateType();
			this._ilg = null;
			this._typebuilder = null;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000A4F0 File Offset: 0x000094F0
		internal void GenerateCreateHashtable(FieldInfo field, Hashtable ht)
		{
			MethodInfo method = typeof(Hashtable).GetMethod("Add", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			base.Ldthis();
			base.Newobj(typeof(Hashtable).GetConstructor(new Type[0]));
			base.Stfld(field);
			IDictionaryEnumerator enumerator = ht.GetEnumerator();
			while (enumerator.MoveNext())
			{
				base.Ldthisfld(field);
				if (enumerator.Key is int)
				{
					base.Ldc((int)enumerator.Key);
					this._ilg.Emit(OpCodes.Box, typeof(int));
				}
				else
				{
					base.Ldstr((string)enumerator.Key);
				}
				base.Ldc((int)enumerator.Value);
				this._ilg.Emit(OpCodes.Box, typeof(int));
				base.Callvirt(method);
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000A5D7 File Offset: 0x000095D7
		private FieldInfo RegexField(string fieldname)
		{
			return typeof(Regex).GetField(fieldname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000A5EB File Offset: 0x000095EB
		internal void Save()
		{
			this._assembly.Save(this._assembly.GetName().Name + ".dll");
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000A612 File Offset: 0x00009612
		internal void GenerateCreateInstance(Type newtype)
		{
			base.Newobj(newtype.GetConstructor(new Type[0]));
			base.Ret();
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000A62C File Offset: 0x0000962C
		internal void DefineType(string typename, bool ispublic, Type inheritfromclass)
		{
			if (ispublic)
			{
				this._typebuilder = this._module.DefineType(typename, TypeAttributes.Public, inheritfromclass);
				return;
			}
			this._typebuilder = this._module.DefineType(typename, TypeAttributes.NotPublic, inheritfromclass);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000A65C File Offset: 0x0000965C
		internal void DefineMethod(string methname, Type returntype)
		{
			MethodAttributes methodAttributes = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual;
			this._methbuilder = this._typebuilder.DefineMethod(methname, methodAttributes, returntype, null);
			this._ilg = this._methbuilder.GetILGenerator();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000A692 File Offset: 0x00009692
		internal void BakeMethod()
		{
			this._methbuilder = null;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000A69C File Offset: 0x0000969C
		internal Type BakeType()
		{
			Type type = this._typebuilder.CreateType();
			this._typebuilder = null;
			return type;
		}

		// Token: 0x0400070A RID: 1802
		private static int _typeCount = 0;

		// Token: 0x0400070B RID: 1803
		private static LocalDataStoreSlot _moduleSlot = Thread.AllocateDataSlot();

		// Token: 0x0400070C RID: 1804
		private AssemblyBuilder _assembly;

		// Token: 0x0400070D RID: 1805
		private ModuleBuilder _module;

		// Token: 0x0400070E RID: 1806
		private TypeBuilder _typebuilder;

		// Token: 0x0400070F RID: 1807
		private MethodBuilder _methbuilder;
	}
}
