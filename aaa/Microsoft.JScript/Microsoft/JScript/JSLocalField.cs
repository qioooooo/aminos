using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000B1 RID: 177
	public sealed class JSLocalField : JSVariableField
	{
		// Token: 0x06000802 RID: 2050 RVA: 0x000380AB File Offset: 0x000370AB
		public JSLocalField(string name, RuntimeTypeHandle handle, int slotNumber)
			: this(name, null, slotNumber, Missing.Value)
		{
			this.type = new TypeExpression(new ConstantWrapper(Type.GetTypeFromHandle(handle), null));
			this.isDefined = true;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x000380DC File Offset: 0x000370DC
		internal JSLocalField(string name, FunctionScope scope, int slotNumber, object value)
			: base(name, scope, FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static)
		{
			this.slotNumber = slotNumber;
			this.inferred_type = null;
			this.dependents = null;
			this.value = value;
			this.debugOn = false;
			this.outerField = null;
			this.isDefined = false;
			this.isUsedBeforeDefinition = false;
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0003812C File Offset: 0x0003712C
		public override Type FieldType
		{
			get
			{
				if (this.type != null)
				{
					return base.FieldType;
				}
				return Convert.ToType(this.GetInferredType(null));
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0003814C File Offset: 0x0003714C
		internal override IReflect GetInferredType(JSField inference_target)
		{
			if (this.outerField != null)
			{
				return this.outerField.GetInferredType(inference_target);
			}
			if (this.type != null)
			{
				return base.GetInferredType(inference_target);
			}
			if (this.inferred_type == null || this.inferred_type == Typeob.Object)
			{
				return Typeob.Object;
			}
			if (inference_target != null && inference_target != this)
			{
				if (this.dependents == null)
				{
					this.dependents = new ArrayList();
				}
				this.dependents.Add(inference_target);
			}
			return this.inferred_type;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x000381C8 File Offset: 0x000371C8
		public override object GetValue(object obj)
		{
			if ((this.attributeFlags & FieldAttributes.Literal) != FieldAttributes.PrivateScope && !(this.value is FunctionObject))
			{
				return this.value;
			}
			while (obj is BlockScope)
			{
				obj = ((BlockScope)obj).GetParent();
			}
			StackFrame stackFrame = (StackFrame)obj;
			JSLocalField jslocalField = this.outerField;
			int num = this.slotNumber;
			while (jslocalField != null)
			{
				num = jslocalField.slotNumber;
				stackFrame = (StackFrame)stackFrame.GetParent();
				jslocalField = jslocalField.outerField;
			}
			return stackFrame.localVars[num];
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00038248 File Offset: 0x00037248
		internal void SetInferredType(IReflect ir, AST expr)
		{
			this.isDefined = true;
			if (this.type != null)
			{
				return;
			}
			if (this.outerField != null)
			{
				this.outerField.SetInferredType(ir, expr);
				return;
			}
			if (Convert.IsPrimitiveNumericTypeFitForDouble(ir))
			{
				ir = Typeob.Double;
			}
			else if (ir == Typeob.Void)
			{
				ir = Typeob.Object;
			}
			if (this.inferred_type == null)
			{
				this.inferred_type = ir;
				return;
			}
			if (ir == this.inferred_type)
			{
				return;
			}
			if (!Convert.IsPrimitiveNumericType(this.inferred_type) || !Convert.IsPrimitiveNumericType(ir) || !Convert.IsPromotableTo(ir, this.inferred_type))
			{
				this.inferred_type = Typeob.Object;
				if (this.dependents != null)
				{
					int i = 0;
					int count = this.dependents.Count;
					while (i < count)
					{
						((JSLocalField)this.dependents[i]).SetInferredType(Typeob.Object, null);
						i++;
					}
				}
			}
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00038320 File Offset: 0x00037320
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo locale)
		{
			if (this.type != null)
			{
				value = Convert.Coerce(value, this.type);
			}
			while (obj is BlockScope)
			{
				obj = ((BlockScope)obj).GetParent();
			}
			StackFrame stackFrame = (StackFrame)obj;
			JSLocalField jslocalField = this.outerField;
			int num = this.slotNumber;
			while (jslocalField != null)
			{
				num = jslocalField.slotNumber;
				stackFrame = (StackFrame)stackFrame.GetParent();
				jslocalField = jslocalField.outerField;
			}
			if (stackFrame.localVars == null)
			{
				return;
			}
			stackFrame.localVars[num] = value;
		}

		// Token: 0x0400044D RID: 1101
		internal int slotNumber;

		// Token: 0x0400044E RID: 1102
		internal IReflect inferred_type;

		// Token: 0x0400044F RID: 1103
		private ArrayList dependents;

		// Token: 0x04000450 RID: 1104
		internal bool debugOn;

		// Token: 0x04000451 RID: 1105
		internal JSLocalField outerField;

		// Token: 0x04000452 RID: 1106
		internal bool isDefined;

		// Token: 0x04000453 RID: 1107
		internal bool isUsedBeforeDefinition;
	}
}
