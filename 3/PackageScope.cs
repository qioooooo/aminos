using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000F6 RID: 246
	internal class PackageScope : ActivationObject
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x00051B39 File Offset: 0x00050B39
		public PackageScope(ScriptObject parent)
			: base(parent)
		{
			this.fast = true;
			this.name = null;
			this.owner = null;
			this.isKnownAtCompileTime = true;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00051B5E File Offset: 0x00050B5E
		internal override JSVariableField AddNewField(string name, object value, FieldAttributes attributeFlags)
		{
			base.AddNewField(this.name + "." + name, value, attributeFlags);
			return base.AddNewField(name, value, attributeFlags);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00051B84 File Offset: 0x00050B84
		internal void AddOwnName()
		{
			string text = this.name;
			int num = text.IndexOf('.');
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			base.AddNewField(text, Namespace.GetNamespace(text, this.engine), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Literal);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00051BC4 File Offset: 0x00050BC4
		protected override JSVariableField CreateField(string name, FieldAttributes attributeFlags, object value)
		{
			return new JSGlobalField(this, name, value, attributeFlags);
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00051BCF File Offset: 0x00050BCF
		internal override string GetName()
		{
			return this.name;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x00051BD8 File Offset: 0x00050BD8
		internal void MergeWith(PackageScope p)
		{
			foreach (object obj in p.field_table)
			{
				JSGlobalField jsglobalField = (JSGlobalField)obj;
				ClassScope classScope = jsglobalField.value as ClassScope;
				if (this.name_table[jsglobalField.Name] != null)
				{
					if (classScope != null)
					{
						classScope.owner.context.HandleError(JSError.DuplicateName, jsglobalField.Name, true);
						Class @class = classScope.owner;
						@class.name += p.GetHashCode().ToString(CultureInfo.InvariantCulture);
					}
				}
				else
				{
					this.field_table.Add(jsglobalField);
					this.name_table[jsglobalField.Name] = jsglobalField;
					if (classScope != null)
					{
						classScope.owner.enclosingScope = this;
						classScope.package = this;
					}
				}
			}
		}

		// Token: 0x04000696 RID: 1686
		internal string name;

		// Token: 0x04000697 RID: 1687
		internal Package owner;
	}
}
