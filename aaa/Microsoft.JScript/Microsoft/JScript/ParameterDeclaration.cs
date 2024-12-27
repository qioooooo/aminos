using System;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000F7 RID: 247
	internal sealed class ParameterDeclaration : ParameterInfo
	{
		// Token: 0x06000AB0 RID: 2736 RVA: 0x00051CD4 File Offset: 0x00050CD4
		internal ParameterDeclaration(Context context, string identifier, TypeExpression type, CustomAttributeList customAttributes)
		{
			this.identifier = identifier;
			this.type = ((type == null) ? new TypeExpression(new ConstantWrapper(Typeob.Object, context)) : type);
			this.context = context;
			ActivationObject activationObject = (ActivationObject)context.document.engine.Globals.ScopeStack.Peek();
			if (activationObject.name_table[this.identifier] != null)
			{
				context.HandleError(JSError.DuplicateName, this.identifier, activationObject is ClassScope || activationObject.fast || type != null);
			}
			else
			{
				JSVariableField jsvariableField = activationObject.AddNewField(this.identifier, null, FieldAttributes.PrivateScope);
				jsvariableField.originalContext = context;
			}
			this.customAttributes = customAttributes;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00051D8F File Offset: 0x00050D8F
		internal ParameterDeclaration(Type type, string identifier)
		{
			this.identifier = identifier;
			this.type = new TypeExpression(new ConstantWrapper(type, null));
			this.customAttributes = null;
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x00051DB7 File Offset: 0x00050DB7
		public override object DefaultValue
		{
			get
			{
				return Convert.DBNull;
			}
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00051DBE File Offset: 0x00050DBE
		public override object[] GetCustomAttributes(bool inherit)
		{
			return new FieldInfo[0];
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00051DC6 File Offset: 0x00050DC6
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return new FieldInfo[0];
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00051DCE File Offset: 0x00050DCE
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.customAttributes != null && this.customAttributes.GetAttribute(attributeType) != null;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00051DEC File Offset: 0x00050DEC
		public override string Name
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x00051DF4 File Offset: 0x00050DF4
		internal IReflect ParameterIReflect
		{
			get
			{
				return this.type.ToIReflect();
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00051E04 File Offset: 0x00050E04
		public override Type ParameterType
		{
			get
			{
				Type type = this.type.ToType();
				if (type == Typeob.Void)
				{
					type = Typeob.Object;
				}
				return type;
			}
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00051E2C File Offset: 0x00050E2C
		internal void PartiallyEvaluate()
		{
			if (this.type != null)
			{
				this.type.PartiallyEvaluate();
			}
			if (this.customAttributes != null)
			{
				this.customAttributes.PartiallyEvaluate();
				if (CustomAttribute.IsDefined(this, typeof(ParamArrayAttribute), false))
				{
					if (this.type != null)
					{
						IReflect reflect = this.type.ToIReflect();
						if ((reflect is Type && ((Type)reflect).IsArray) || reflect is TypedArray)
						{
							return;
						}
					}
					this.customAttributes.context.HandleError(JSError.IllegalParamArrayAttribute);
				}
			}
		}

		// Token: 0x04000698 RID: 1688
		internal string identifier;

		// Token: 0x04000699 RID: 1689
		internal TypeExpression type;

		// Token: 0x0400069A RID: 1690
		internal Context context;

		// Token: 0x0400069B RID: 1691
		internal CustomAttributeList customAttributes;
	}
}
