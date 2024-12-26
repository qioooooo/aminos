using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200009B RID: 155
	public abstract class JSVariableField : JSField
	{
		// Token: 0x060006F0 RID: 1776 RVA: 0x00030C6C File Offset: 0x0002FC6C
		internal JSVariableField(string name, ScriptObject obj, FieldAttributes attributeFlags)
		{
			this.obj = obj;
			this.name = name;
			this.debuggerName = name;
			this.metaData = null;
			if ((attributeFlags & FieldAttributes.FieldAccessMask) == FieldAttributes.PrivateScope)
			{
				attributeFlags |= FieldAttributes.Public;
			}
			this.attributeFlags = attributeFlags;
			this.type = null;
			this.method = null;
			this.cons = null;
			this.value = null;
			this.originalContext = null;
			this.clsCompliance = CLSComplianceSpec.NotAttributed;
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x00030CD6 File Offset: 0x0002FCD6
		public override FieldAttributes Attributes
		{
			get
			{
				return this.attributeFlags;
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00030CE0 File Offset: 0x0002FCE0
		internal void CheckCLSCompliance(bool classIsCLSCompliant)
		{
			if (this.customAttributes != null)
			{
				CustomAttribute attribute = this.customAttributes.GetAttribute(Typeob.CLSCompliantAttribute);
				if (attribute != null)
				{
					this.clsCompliance = attribute.GetCLSComplianceValue();
					this.customAttributes.Remove(attribute);
				}
			}
			if (classIsCLSCompliant)
			{
				if (this.clsCompliance != CLSComplianceSpec.NonCLSCompliant && this.type != null && !this.type.IsCLSCompliant())
				{
					this.clsCompliance = CLSComplianceSpec.NonCLSCompliant;
					if (this.originalContext != null)
					{
						this.originalContext.HandleError(JSError.NonCLSCompliantMember);
						return;
					}
				}
			}
			else if (this.clsCompliance == CLSComplianceSpec.CLSCompliant)
			{
				this.originalContext.HandleError(JSError.MemberTypeCLSCompliantMismatch);
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x00030D7B File Offset: 0x0002FD7B
		public override Type DeclaringType
		{
			get
			{
				if (this.obj is ClassScope)
				{
					return ((ClassScope)this.obj).GetTypeBuilderOrEnumBuilder();
				}
				return null;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00030D9C File Offset: 0x0002FD9C
		public override Type FieldType
		{
			get
			{
				Type type = Typeob.Object;
				if (this.type != null)
				{
					type = this.type.ToType();
					if (type == Typeob.Void)
					{
						type = Typeob.Object;
					}
				}
				return type;
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00030DD2 File Offset: 0x0002FDD2
		internal MethodInfo GetAsMethod(object obj)
		{
			if (this.method == null)
			{
				this.method = new JSFieldMethod(this, obj);
			}
			return this.method;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00030DEF File Offset: 0x0002FDEF
		internal override string GetClassFullName()
		{
			if (this.obj is ClassScope)
			{
				return ((ClassScope)this.obj).GetFullName();
			}
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00030E16 File Offset: 0x0002FE16
		public override object[] GetCustomAttributes(bool inherit)
		{
			if (this.customAttributes != null)
			{
				return (object[])this.customAttributes.Evaluate();
			}
			return new object[0];
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00030E37 File Offset: 0x0002FE37
		internal virtual IReflect GetInferredType(JSField inference_target)
		{
			if (this.type != null)
			{
				return this.type.ToIReflect();
			}
			return Typeob.Object;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00030E52 File Offset: 0x0002FE52
		internal override object GetMetaData()
		{
			return this.metaData;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00030E5A File Offset: 0x0002FE5A
		internal override PackageScope GetPackage()
		{
			if (this.obj is ClassScope)
			{
				return ((ClassScope)this.obj).GetPackage();
			}
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x00030E81 File Offset: 0x0002FE81
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00030E8C File Offset: 0x0002FE8C
		internal void WriteCustomAttribute(bool doCRS)
		{
			if (this.metaData is FieldBuilder)
			{
				FieldBuilder fieldBuilder = (FieldBuilder)this.metaData;
				if (this.customAttributes != null)
				{
					CustomAttributeBuilder[] customAttributeBuilders = this.customAttributes.GetCustomAttributeBuilders(false);
					int i = 0;
					int num = customAttributeBuilders.Length;
					while (i < num)
					{
						fieldBuilder.SetCustomAttribute(customAttributeBuilders[i]);
						i++;
					}
				}
				if (this.clsCompliance == CLSComplianceSpec.CLSCompliant)
				{
					fieldBuilder.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[] { true }));
				}
				else if (this.clsCompliance == CLSComplianceSpec.NonCLSCompliant)
				{
					fieldBuilder.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[] { false }));
				}
				if (doCRS && base.IsStatic)
				{
					fieldBuilder.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.contextStaticAttributeCtor, new object[0]));
				}
			}
		}

		// Token: 0x04000311 RID: 785
		internal ScriptObject obj;

		// Token: 0x04000312 RID: 786
		private string name;

		// Token: 0x04000313 RID: 787
		internal string debuggerName;

		// Token: 0x04000314 RID: 788
		internal object metaData;

		// Token: 0x04000315 RID: 789
		internal TypeExpression type;

		// Token: 0x04000316 RID: 790
		internal FieldAttributes attributeFlags;

		// Token: 0x04000317 RID: 791
		private MethodInfo method;

		// Token: 0x04000318 RID: 792
		private ConstructorInfo cons;

		// Token: 0x04000319 RID: 793
		internal object value;

		// Token: 0x0400031A RID: 794
		internal CustomAttributeList customAttributes;

		// Token: 0x0400031B RID: 795
		internal Context originalContext;

		// Token: 0x0400031C RID: 796
		internal CLSComplianceSpec clsCompliance;
	}
}
