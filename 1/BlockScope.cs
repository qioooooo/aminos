using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200002D RID: 45
	public class BlockScope : ActivationObject
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x0000EC1C File Offset: 0x0000DC1C
		internal BlockScope(ScriptObject parent)
			: base(parent)
		{
			this.scopeId = BlockScope.counter++;
			this.isKnownAtCompileTime = true;
			this.fast = !(parent is ActivationObject) || ((ActivationObject)parent).fast;
			this.localFieldsForDebugInfo = new ArrayList();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000EC74 File Offset: 0x0000DC74
		public BlockScope(ScriptObject parent, string name, int scopeId)
			: base(parent)
		{
			this.scopeId = scopeId;
			JSField jsfield = (JSField)this.parent.GetField(name + ":" + this.scopeId, BindingFlags.Public);
			this.name_table[name] = jsfield;
			this.field_table.Add(jsfield);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000ECD2 File Offset: 0x0000DCD2
		internal void AddFieldForLocalScopeDebugInfo(JSLocalField field)
		{
			this.localFieldsForDebugInfo.Add(field);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000ECE4 File Offset: 0x0000DCE4
		protected override JSVariableField CreateField(string name, FieldAttributes attributeFlags, object value)
		{
			if (!(this.parent is ActivationObject))
			{
				return base.CreateField(name, attributeFlags, value);
			}
			JSVariableField jsvariableField = ((ActivationObject)this.parent).AddNewField(name + ":" + this.scopeId, value, attributeFlags);
			jsvariableField.debuggerName = name;
			return jsvariableField;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000ED3C File Offset: 0x0000DD3C
		internal void EmitLocalInfoForFields(ILGenerator il)
		{
			foreach (object obj in this.localFieldsForDebugInfo)
			{
				JSLocalField jslocalField = (JSLocalField)obj;
				((LocalBuilder)jslocalField.metaData).SetLocalSymInfo(jslocalField.debuggerName);
			}
			if (this.parent is GlobalScope)
			{
				LocalBuilder localBuilder = il.DeclareLocal(Typeob.Int32);
				localBuilder.SetLocalSymInfo("scopeId for catch block");
				ConstantWrapper.TranslateToILInt(il, this.scopeId);
				il.Emit(OpCodes.Stloc, localBuilder);
			}
		}

		// Token: 0x04000092 RID: 146
		private static int counter;

		// Token: 0x04000093 RID: 147
		internal bool catchHanderScope;

		// Token: 0x04000094 RID: 148
		internal int scopeId;

		// Token: 0x04000095 RID: 149
		private ArrayList localFieldsForDebugInfo;
	}
}
