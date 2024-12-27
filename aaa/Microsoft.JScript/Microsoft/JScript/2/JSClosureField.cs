using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x0200009C RID: 156
	internal sealed class JSClosureField : JSVariableField
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x00030F60 File Offset: 0x0002FF60
		internal JSClosureField(FieldInfo field)
			: base(field.Name, null, field.Attributes | FieldAttributes.Static)
		{
			if (field is JSFieldInfo)
			{
				field = ((JSFieldInfo)field).field;
			}
			this.field = field;
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x00030F94 File Offset: 0x0002FF94
		public override Type DeclaringType
		{
			get
			{
				return this.field.DeclaringType;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x00030FA1 File Offset: 0x0002FFA1
		public override Type FieldType
		{
			get
			{
				return this.field.FieldType;
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00030FAE File Offset: 0x0002FFAE
		internal override IReflect GetInferredType(JSField inference_target)
		{
			if (this.field is JSMemberField)
			{
				return ((JSMemberField)this.field).GetInferredType(inference_target);
			}
			return this.field.FieldType;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00030FDA File Offset: 0x0002FFDA
		internal override object GetMetaData()
		{
			if (this.field is JSField)
			{
				return ((JSField)this.field).GetMetaData();
			}
			return this.field;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00031000 File Offset: 0x00030000
		public override object GetValue(object obj)
		{
			if (obj is StackFrame)
			{
				return this.field.GetValue(((StackFrame)((StackFrame)obj).engine.ScriptObjectStackTop()).closureInstance);
			}
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00031037 File Offset: 0x00030037
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo locale)
		{
			if (obj is StackFrame)
			{
				this.field.SetValue(((StackFrame)((StackFrame)obj).engine.ScriptObjectStackTop()).closureInstance, value, invokeAttr, binder, locale);
				return;
			}
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x0400031D RID: 797
		internal FieldInfo field;
	}
}
