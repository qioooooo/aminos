using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200004C RID: 76
	internal sealed class Constant : AST
	{
		// Token: 0x060003A4 RID: 932 RVA: 0x00016A18 File Offset: 0x00015A18
		internal Constant(Context context, Lookup identifier, TypeExpression type, AST value, FieldAttributes attributes, CustomAttributeList customAttributes)
			: base(context)
		{
			this.attributes = attributes | FieldAttributes.InitOnly;
			this.customAttributes = customAttributes;
			this.completion = new Completion();
			this.identifier = identifier;
			this.name = identifier.ToString();
			this.value = value;
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject)
			{
				scriptObject = scriptObject.GetParent();
			}
			if (scriptObject is ClassScope)
			{
				if (this.name == ((ClassScope)scriptObject).name)
				{
					identifier.context.HandleError(JSError.CannotUseNameOfClass);
					this.name += " const";
				}
				if (attributes == FieldAttributes.PrivateScope)
				{
					attributes = FieldAttributes.Public;
				}
			}
			else
			{
				if (attributes != FieldAttributes.PrivateScope)
				{
					this.context.HandleError(JSError.NotInsideClass);
				}
				attributes = FieldAttributes.Public;
			}
			FieldInfo localField = ((IActivationObject)scriptObject).GetLocalField(this.name);
			if (localField != null)
			{
				identifier.context.HandleError(JSError.DuplicateName, true);
				this.name += " const";
			}
			if (scriptObject is ActivationObject)
			{
				this.field = ((ActivationObject)scriptObject).AddNewField(this.identifier.ToString(), value, attributes);
			}
			else
			{
				this.field = ((StackFrame)scriptObject).AddNewField(this.identifier.ToString(), value, attributes | FieldAttributes.Static);
			}
			this.field.type = type;
			this.field.customAttributes = customAttributes;
			this.field.originalContext = context;
			if (this.field is JSLocalField)
			{
				((JSLocalField)this.field).debugOn = this.identifier.context.document.debugOn;
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00016BCD File Offset: 0x00015BCD
		internal override object Evaluate()
		{
			if (this.value == null)
			{
				this.completion.value = this.field.value;
			}
			else
			{
				this.completion.value = this.value.Evaluate();
			}
			return this.completion;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00016C0C File Offset: 0x00015C0C
		internal override AST PartiallyEvaluate()
		{
			this.field.attributeFlags &= ~FieldAttributes.InitOnly;
			this.identifier.PartiallyEvaluateAsReference();
			if (this.field.type != null)
			{
				this.field.type.PartiallyEvaluate();
			}
			base.Globals.ScopeStack.Peek();
			if (this.value != null)
			{
				this.value = this.value.PartiallyEvaluate();
				this.identifier.SetPartialValue(this.value);
				if (this.value is ConstantWrapper)
				{
					object obj = (this.field.value = this.value.Evaluate());
					if (this.field.type != null)
					{
						this.field.value = Convert.Coerce(obj, this.field.type, true);
					}
					if (this.field.IsStatic && (obj is Type || obj is ClassScope || obj is TypedArray || Convert.GetTypeCode(obj) != TypeCode.Object))
					{
						this.field.attributeFlags |= FieldAttributes.Literal;
						goto IL_0128;
					}
				}
				this.field.attributeFlags |= FieldAttributes.InitOnly;
				IL_0128:
				if (this.field.type == null)
				{
					this.field.type = new TypeExpression(new ConstantWrapper(this.value.InferType(null), null));
				}
			}
			else
			{
				this.value = new ConstantWrapper(null, this.context);
				this.field.attributeFlags |= FieldAttributes.InitOnly;
			}
			if (this.field != null && this.field.customAttributes != null)
			{
				this.field.customAttributes.PartiallyEvaluate();
			}
			return this;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00016DC0 File Offset: 0x00015DC0
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if ((this.field.attributeFlags & FieldAttributes.Literal) != FieldAttributes.PrivateScope)
			{
				object obj = this.field.value;
				if (obj is Type || obj is ClassScope || obj is TypedArray)
				{
					this.field.attributeFlags &= ~FieldAttributes.Literal;
					this.identifier.TranslateToILPreSet(il);
					this.identifier.TranslateToILSet(il, new ConstantWrapper(obj, null));
					this.field.attributeFlags |= FieldAttributes.Literal;
					return;
				}
			}
			else
			{
				if (!this.field.IsStatic)
				{
					FieldBuilder fieldBuilder = (this.valueField = this.field.metaData as FieldBuilder);
					if (fieldBuilder != null)
					{
						this.field.metaData = ((TypeBuilder)fieldBuilder.DeclaringType).DefineField(this.name + " value", this.field.type.ToType(), FieldAttributes.Private);
					}
				}
				this.field.attributeFlags &= ~FieldAttributes.InitOnly;
				this.identifier.TranslateToILPreSet(il);
				this.identifier.TranslateToILSet(il, this.value);
				this.field.attributeFlags |= FieldAttributes.InitOnly;
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00016EF8 File Offset: 0x00015EF8
		internal void TranslateToILInitOnlyInitializers(ILGenerator il)
		{
			FieldBuilder fieldBuilder = this.valueField;
			if (fieldBuilder != null)
			{
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Ldfld, (FieldBuilder)this.field.metaData);
				il.Emit(OpCodes.Stfld, fieldBuilder);
				this.valueField = (FieldBuilder)this.field.metaData;
				this.field.metaData = fieldBuilder;
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00016F6E File Offset: 0x00015F6E
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			if (this.value != null)
			{
				this.value.TranslateToILInitializer(il);
			}
		}

		// Token: 0x040001D3 RID: 467
		internal FieldAttributes attributes;

		// Token: 0x040001D4 RID: 468
		private Completion completion;

		// Token: 0x040001D5 RID: 469
		internal CustomAttributeList customAttributes;

		// Token: 0x040001D6 RID: 470
		internal JSVariableField field;

		// Token: 0x040001D7 RID: 471
		private FieldBuilder valueField;

		// Token: 0x040001D8 RID: 472
		private Lookup identifier;

		// Token: 0x040001D9 RID: 473
		internal string name;

		// Token: 0x040001DA RID: 474
		internal AST value;
	}
}
