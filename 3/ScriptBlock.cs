using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000111 RID: 273
	public class ScriptBlock : AST
	{
		// Token: 0x06000B60 RID: 2912 RVA: 0x00056DE5 File Offset: 0x00055DE5
		internal ScriptBlock(Context context, Block statement_block)
			: base(context)
		{
			this.statement_block = statement_block;
			this.own_scope = (GlobalScope)base.Engine.ScriptObjectStackTop();
			this.fields = null;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00056E14 File Offset: 0x00055E14
		internal override object Evaluate()
		{
			if (this.fields == null)
			{
				this.fields = this.own_scope.GetFields();
			}
			int i = 0;
			int num = this.fields.Length;
			while (i < num)
			{
				FieldInfo fieldInfo = this.fields[i];
				if (!(fieldInfo is JSExpandoField))
				{
					object value = fieldInfo.GetValue(this.own_scope);
					if (value is FunctionObject)
					{
						((FunctionObject)value).engine = base.Engine;
						this.own_scope.AddFieldOrUseExistingField(fieldInfo.Name, new Closure((FunctionObject)value), fieldInfo.Attributes);
					}
					else if (value is ClassScope)
					{
						this.own_scope.AddFieldOrUseExistingField(fieldInfo.Name, value, fieldInfo.Attributes);
					}
					else
					{
						this.own_scope.AddFieldOrUseExistingField(fieldInfo.Name, Missing.Value, fieldInfo.Attributes);
					}
				}
				i++;
			}
			object obj = this.statement_block.Evaluate();
			if (obj is Completion)
			{
				obj = ((Completion)obj).value;
			}
			return obj;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00056F1A File Offset: 0x00055F1A
		internal void ProcessAssemblyAttributeLists()
		{
			this.statement_block.ProcessAssemblyAttributeLists();
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00056F28 File Offset: 0x00055F28
		internal override AST PartiallyEvaluate()
		{
			this.statement_block.PartiallyEvaluate();
			if (base.Engine.PEFileKind == PEFileKinds.Dll && base.Engine.doSaveAfterCompile)
			{
				this.statement_block.ComplainAboutAnythingOtherThanClassOrPackage();
			}
			this.fields = this.own_scope.GetFields();
			return this;
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00056F7C File Offset: 0x00055F7C
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			Expression expression = this.statement_block.ToExpression();
			if (expression != null)
			{
				expression.TranslateToIL(il, rtype);
				return;
			}
			this.statement_block.TranslateToIL(il, Typeob.Void);
			new ConstantWrapper(null, this.context).TranslateToIL(il, rtype);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00056FC5 File Offset: 0x00055FC5
		internal TypeBuilder TranslateToILClass(CompilerGlobals compilerGlobals)
		{
			return this.TranslateToILClass(compilerGlobals, true);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00056FD0 File Offset: 0x00055FD0
		internal TypeBuilder TranslateToILClass(CompilerGlobals compilerGlobals, bool pushScope)
		{
			TypeBuilder typeBuilder = (compilerGlobals.classwriter = compilerGlobals.module.DefineType("JScript " + base.Engine.classCounter++.ToString(CultureInfo.InvariantCulture), TypeAttributes.Public, Typeob.GlobalScope, null));
			compilerGlobals.classwriter.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.compilerGlobalScopeAttributeCtor, new object[0]));
			if (compilerGlobals.globalScopeClassWriter == null)
			{
				compilerGlobals.globalScopeClassWriter = typeBuilder;
			}
			ConstructorBuilder constructorBuilder = compilerGlobals.classwriter.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { Typeob.GlobalScope });
			ILGenerator ilgenerator = constructorBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Dup);
			ilgenerator.Emit(OpCodes.Ldfld, CompilerGlobals.engineField);
			ilgenerator.Emit(OpCodes.Call, CompilerGlobals.globalScopeConstructor);
			ilgenerator.Emit(OpCodes.Ret);
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("Global Code", MethodAttributes.Public, Typeob.Object, null);
			ilgenerator = methodBuilder.GetILGenerator();
			if (base.Engine.GenerateDebugInfo)
			{
				for (ScriptObject scriptObject = this.own_scope.GetParent(); scriptObject != null; scriptObject = scriptObject.GetParent())
				{
					if (scriptObject is WrappedNamespace && !((WrappedNamespace)scriptObject).name.Equals(""))
					{
						ilgenerator.UsingNamespace(((WrappedNamespace)scriptObject).name);
					}
				}
			}
			int startLine = this.context.StartLine;
			int startColumn = this.context.StartColumn;
			Context firstExecutableContext = this.GetFirstExecutableContext();
			if (firstExecutableContext != null)
			{
				firstExecutableContext.EmitFirstLineInfo(ilgenerator);
			}
			if (pushScope)
			{
				base.EmitILToLoadEngine(ilgenerator);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Call, CompilerGlobals.pushScriptObjectMethod);
			}
			this.TranslateToILInitializer(ilgenerator);
			this.TranslateToIL(ilgenerator, Typeob.Object);
			if (pushScope)
			{
				base.EmitILToLoadEngine(ilgenerator);
				ilgenerator.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
				ilgenerator.Emit(OpCodes.Pop);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return typeBuilder;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x000571D4 File Offset: 0x000561D4
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			int num = this.fields.Length;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					JSGlobalField jsglobalField = this.fields[i] as JSGlobalField;
					if (jsglobalField != null)
					{
						Type fieldType = jsglobalField.FieldType;
						if ((jsglobalField.IsLiteral && fieldType != Typeob.ScriptFunction && fieldType != Typeob.Type) || jsglobalField.metaData != null)
						{
							if ((fieldType.IsPrimitive || fieldType == Typeob.String || fieldType.IsEnum) && jsglobalField.metaData == null)
							{
								FieldBuilder fieldBuilder = base.compilerGlobals.classwriter.DefineField(jsglobalField.Name, fieldType, jsglobalField.Attributes);
								fieldBuilder.SetConstant(jsglobalField.value);
							}
						}
						else if (!(jsglobalField.value is FunctionObject) || !((FunctionObject)jsglobalField.value).suppressIL)
						{
							FieldBuilder fieldBuilder2 = base.compilerGlobals.classwriter.DefineField(jsglobalField.Name, fieldType, (jsglobalField.Attributes & ~(FieldAttributes.InitOnly | FieldAttributes.Literal)) | FieldAttributes.Static);
							jsglobalField.metaData = fieldBuilder2;
							jsglobalField.WriteCustomAttribute(base.Engine.doCRS);
						}
					}
				}
			}
			this.statement_block.TranslateToILInitializer(il);
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000572FB File Offset: 0x000562FB
		internal override Context GetFirstExecutableContext()
		{
			return this.statement_block.GetFirstExecutableContext();
		}

		// Token: 0x040006DC RID: 1756
		private Block statement_block;

		// Token: 0x040006DD RID: 1757
		private JSField[] fields;

		// Token: 0x040006DE RID: 1758
		private GlobalScope own_scope;
	}
}
