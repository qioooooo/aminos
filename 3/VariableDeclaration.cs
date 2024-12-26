using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200012C RID: 300
	internal sealed class VariableDeclaration : AST
	{
		// Token: 0x06000DD6 RID: 3542 RVA: 0x0005DF48 File Offset: 0x0005CF48
		internal VariableDeclaration(Context context, Lookup identifier, TypeExpression type, AST initializer, FieldAttributes attributes, CustomAttributeList customAttributes)
			: base(context)
		{
			if (initializer != null)
			{
				this.context.UpdateWith(initializer.context);
			}
			else if (type != null)
			{
				this.context.UpdateWith(type.context);
			}
			this.identifier = identifier;
			this.type = type;
			this.initializer = initializer;
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject)
			{
				scriptObject = scriptObject.GetParent();
			}
			string text = this.identifier.ToString();
			if (scriptObject is ClassScope)
			{
				if (text == ((ClassScope)scriptObject).name)
				{
					identifier.context.HandleError(JSError.CannotUseNameOfClass);
					text += " var";
				}
			}
			else if (attributes != FieldAttributes.PrivateScope)
			{
				this.context.HandleError(JSError.NotInsideClass);
				attributes = FieldAttributes.Public;
			}
			else
			{
				attributes |= FieldAttributes.Public;
			}
			FieldInfo localField = ((IActivationObject)scriptObject).GetLocalField(text);
			if (localField != null)
			{
				if (localField.IsLiteral || scriptObject is ClassScope || type != null)
				{
					identifier.context.HandleError(JSError.DuplicateName, true);
				}
				type = (this.type = null);
			}
			if (scriptObject is ActivationObject)
			{
				if (localField == null || localField is JSVariableField)
				{
					this.field = ((ActivationObject)scriptObject).AddFieldOrUseExistingField(this.identifier.ToString(), Missing.Value, attributes);
				}
				else
				{
					this.field = ((ActivationObject)scriptObject).AddNewField(this.identifier.ToString(), null, attributes);
				}
			}
			else
			{
				this.field = ((StackFrame)scriptObject).AddNewField(this.identifier.ToString(), null, attributes | FieldAttributes.Static);
			}
			this.field.type = type;
			this.field.customAttributes = customAttributes;
			this.field.originalContext = context;
			if (this.field is JSLocalField)
			{
				((JSLocalField)this.field).debugOn = this.identifier.context.document.debugOn;
			}
			this.completion = new Completion();
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0005E140 File Offset: 0x0005D140
		internal override object Evaluate()
		{
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			object obj = null;
			if (this.initializer != null)
			{
				obj = this.initializer.Evaluate();
			}
			if (this.type != null)
			{
				obj = Convert.Coerce(obj, this.type);
			}
			else
			{
				while (scriptObject is BlockScope)
				{
					scriptObject = scriptObject.GetParent();
				}
				if (scriptObject is WithObject)
				{
					this.identifier.SetWithValue((WithObject)scriptObject, obj);
				}
				while (scriptObject is WithObject || scriptObject is BlockScope)
				{
					scriptObject = scriptObject.GetParent();
				}
				if (this.initializer == null && !(this.field.value is Missing))
				{
					this.completion.value = this.field.value;
					return this.completion;
				}
			}
			this.field.SetValue(scriptObject, this.completion.value = obj);
			return this.completion;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0005E228 File Offset: 0x0005D228
		internal override AST PartiallyEvaluate()
		{
			AST ast = (this.identifier = (Lookup)this.identifier.PartiallyEvaluateAsReference());
			if (this.type != null)
			{
				this.field.type = (this.type = (TypeExpression)this.type.PartiallyEvaluate());
			}
			else if (this.initializer == null && !(this.field is JSLocalField) && this.field.value is Missing)
			{
				ast.context.HandleError(JSError.VariableLeftUninitialized);
				this.field.type = (this.type = new TypeExpression(new ConstantWrapper(Typeob.Object, ast.context)));
			}
			if (this.initializer != null)
			{
				if (this.field.IsStatic)
				{
					ScriptObject scriptObject = base.Engine.ScriptObjectStackTop();
					ClassScope classScope = null;
					while (scriptObject != null && (classScope = scriptObject as ClassScope) == null)
					{
						scriptObject = scriptObject.GetParent();
					}
					if (classScope != null)
					{
						classScope.inStaticInitializerCode = true;
					}
					this.initializer = this.initializer.PartiallyEvaluate();
					if (classScope != null)
					{
						classScope.inStaticInitializerCode = false;
					}
				}
				else
				{
					this.initializer = this.initializer.PartiallyEvaluate();
				}
				ast.SetPartialValue(this.initializer);
			}
			if (this.field != null && this.field.customAttributes != null)
			{
				this.field.customAttributes.PartiallyEvaluate();
			}
			return this;
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0005E388 File Offset: 0x0005D388
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (this.initializer == null)
			{
				return;
			}
			if (this.context.document.debugOn && this.initializer.context != null)
			{
				this.context.EmitLineInfo(il);
			}
			Lookup lookup = this.identifier;
			lookup.TranslateToILPreSet(il, true);
			lookup.TranslateToILSet(il, true, this.initializer);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0005E3E6 File Offset: 0x0005D3E6
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			if (this.type != null)
			{
				this.type.TranslateToILInitializer(il);
			}
			if (this.initializer != null)
			{
				this.initializer.TranslateToILInitializer(il);
			}
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0005E410 File Offset: 0x0005D410
		internal override Context GetFirstExecutableContext()
		{
			if (this.initializer == null)
			{
				return null;
			}
			return this.context;
		}

		// Token: 0x0400077F RID: 1919
		internal Lookup identifier;

		// Token: 0x04000780 RID: 1920
		private TypeExpression type;

		// Token: 0x04000781 RID: 1921
		internal AST initializer;

		// Token: 0x04000782 RID: 1922
		internal JSVariableField field;

		// Token: 0x04000783 RID: 1923
		private Completion completion;
	}
}
