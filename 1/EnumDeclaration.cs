using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200006E RID: 110
	internal sealed class EnumDeclaration : Class
	{
		// Token: 0x06000543 RID: 1347 RVA: 0x000257BC File Offset: 0x000247BC
		internal EnumDeclaration(Context context, IdentifierLiteral id, TypeExpression baseType, Block body, FieldAttributes attributes, CustomAttributeList customAttributes)
			: base(context, id, new TypeExpression(new ConstantWrapper(Typeob.Enum, null)), new TypeExpression[0], body, attributes, false, false, true, false, customAttributes)
		{
			this.baseType = ((baseType != null) ? baseType : new TypeExpression(new ConstantWrapper(Typeob.Int32, null)));
			this.needsEngine = false;
			this.attributes &= TypeAttributes.VisibilityMask;
			TypeExpression typeExpression = new TypeExpression(new ConstantWrapper(this.classob, this.context));
			AST ast = new ConstantWrapper(-1, null);
			AST ast2 = new ConstantWrapper(1, null);
			foreach (JSMemberField fieldInfo in this.fields)
			{
				JSVariableField jsvariableField = (JSVariableField)fieldInfo;
				jsvariableField.attributeFlags = FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static | FieldAttributes.Literal;
				jsvariableField.type = typeExpression;
				if (jsvariableField.value == null)
				{
					ast = (jsvariableField.value = new Plus(ast.context, ast, ast2));
				}
				else
				{
					ast = (AST)jsvariableField.value;
				}
				jsvariableField.value = new DeclaredEnumValue(jsvariableField.value, jsvariableField.Name, this.classob);
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000258DC File Offset: 0x000248DC
		internal override AST PartiallyEvaluate()
		{
			if (!(this.classob.GetParent() is GlobalScope))
			{
				return this;
			}
			this.baseType.PartiallyEvaluate();
			IReflect reflect = this.baseType.ToIReflect();
			Type type;
			if (!(reflect is Type) || !Convert.IsPrimitiveIntegerType(type = (Type)reflect))
			{
				this.baseType.context.HandleError(JSError.InvalidBaseTypeForEnum);
				this.baseType = new TypeExpression(new ConstantWrapper(Typeob.Int32, null));
				type = Typeob.Int32;
			}
			if (this.customAttributes != null)
			{
				this.customAttributes.PartiallyEvaluate();
			}
			if (base.NeedsToBeCheckedForCLSCompliance())
			{
				if (!TypeExpression.TypeIsCLSCompliant(reflect))
				{
					this.baseType.context.HandleError(JSError.NonCLSCompliantType);
				}
				base.CheckMemberNamesForCLSCompliance();
			}
			ScriptObject scriptObject = this.enclosingScope;
			while (!(scriptObject is GlobalScope) && !(scriptObject is PackageScope))
			{
				scriptObject = scriptObject.GetParent();
			}
			this.classob.SetParent(new WithObject(scriptObject, Typeob.Enum, true));
			base.Globals.ScopeStack.Push(this.classob);
			try
			{
				foreach (JSMemberField fieldInfo in this.fields)
				{
					JSMemberField jsmemberField = (JSMemberField)fieldInfo;
					((DeclaredEnumValue)jsmemberField.value).CoerceToBaseType(type, jsmemberField.originalContext);
				}
			}
			finally
			{
				base.Globals.ScopeStack.Pop();
			}
			return this;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00025A54 File Offset: 0x00024A54
		internal override Type GetTypeBuilderOrEnumBuilder()
		{
			if (this.classob.classwriter != null)
			{
				return this.classob.classwriter;
			}
			this.PartiallyEvaluate();
			ClassScope classScope = this.enclosingScope as ClassScope;
			if (classScope != null)
			{
				TypeBuilder typeBuilder = ((TypeBuilder)classScope.classwriter).DefineNestedType(this.name, this.attributes | TypeAttributes.Sealed, Typeob.Enum, null);
				this.classob.classwriter = typeBuilder;
				Type type = this.baseType.ToType();
				FieldBuilder fieldBuilder = typeBuilder.DefineField("value__", type, FieldAttributes.Private | FieldAttributes.SpecialName);
				if (this.customAttributes != null)
				{
					CustomAttributeBuilder[] customAttributeBuilders = this.customAttributes.GetCustomAttributeBuilders(false);
					for (int i = 0; i < customAttributeBuilders.Length; i++)
					{
						typeBuilder.SetCustomAttribute(customAttributeBuilders[i]);
					}
				}
				foreach (JSMemberField fieldInfo in this.fields)
				{
					(((JSMemberField)fieldInfo).metaData = typeBuilder.DefineField(fieldInfo.Name, typeBuilder, FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static | FieldAttributes.Literal)).SetConstant(((EnumWrapper)fieldInfo.GetValue(null)).ToNumericValue());
				}
				return typeBuilder;
			}
			EnumBuilder enumBuilder = base.compilerGlobals.module.DefineEnum(this.name, this.attributes, this.baseType.ToType());
			this.classob.classwriter = enumBuilder;
			if (this.customAttributes != null)
			{
				CustomAttributeBuilder[] customAttributeBuilders2 = this.customAttributes.GetCustomAttributeBuilders(false);
				for (int k = 0; k < customAttributeBuilders2.Length; k++)
				{
					enumBuilder.SetCustomAttribute(customAttributeBuilders2[k]);
				}
			}
			foreach (JSMemberField fieldInfo2 in this.fields)
			{
				((JSMemberField)fieldInfo2).metaData = enumBuilder.DefineLiteral(fieldInfo2.Name, ((EnumWrapper)fieldInfo2.GetValue(null)).ToNumericValue());
			}
			return enumBuilder;
		}

		// Token: 0x04000247 RID: 583
		internal TypeExpression baseType;
	}
}
