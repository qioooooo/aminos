using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000AD RID: 173
	internal sealed class JSGlobalField : JSVariableField
	{
		// Token: 0x060007E8 RID: 2024 RVA: 0x00035E4E File Offset: 0x00034E4E
		internal JSGlobalField(ScriptObject obj, string name, object value, FieldAttributes attributeFlags)
			: base(name, obj, attributeFlags)
		{
			this.value = value;
			this.ILField = null;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00035E68 File Offset: 0x00034E68
		public override object GetValue(object obj)
		{
			if (this.ILField == null)
			{
				return this.value;
			}
			return this.ILField.GetValue(null);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00035E88 File Offset: 0x00034E88
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			if (this.ILField != null)
			{
				this.ILField.SetValue(null, value, invokeAttr, binder, culture);
				return;
			}
			if ((base.IsLiteral || base.IsInitOnly) && !(this.value is Missing))
			{
				if (this.value is FunctionObject && value is FunctionObject && this.Name.Equals(((FunctionObject)value).name))
				{
					this.value = value;
					return;
				}
				throw new JScriptException(JSError.AssignmentToReadOnly);
			}
			else
			{
				if (this.type != null)
				{
					this.value = Convert.Coerce(value, this.type);
					return;
				}
				this.value = value;
				return;
			}
		}

		// Token: 0x04000443 RID: 1091
		internal FieldInfo ILField;
	}
}
