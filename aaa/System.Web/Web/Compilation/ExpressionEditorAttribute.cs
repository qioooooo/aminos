using System;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x0200016E RID: 366
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ExpressionEditorAttribute : Attribute
	{
		// Token: 0x0600105E RID: 4190 RVA: 0x00048EEB File Offset: 0x00047EEB
		public ExpressionEditorAttribute(Type type)
			: this((type != null) ? type.AssemblyQualifiedName : null)
		{
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x00048EFF File Offset: 0x00047EFF
		public ExpressionEditorAttribute(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}
			this._editorTypeName = typeName;
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x00048F21 File Offset: 0x00047F21
		public string EditorTypeName
		{
			get
			{
				return this._editorTypeName;
			}
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x00048F2C File Offset: 0x00047F2C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ExpressionEditorAttribute expressionEditorAttribute = obj as ExpressionEditorAttribute;
			return expressionEditorAttribute != null && expressionEditorAttribute.EditorTypeName == this.EditorTypeName;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00048F5C File Offset: 0x00047F5C
		public override int GetHashCode()
		{
			return this.EditorTypeName.GetHashCode();
		}

		// Token: 0x04001650 RID: 5712
		private string _editorTypeName;
	}
}
