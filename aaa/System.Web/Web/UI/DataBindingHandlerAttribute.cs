using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003D2 RID: 978
	[AttributeUsage(AttributeTargets.Class)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DataBindingHandlerAttribute : Attribute
	{
		// Token: 0x06002FC0 RID: 12224 RVA: 0x000D447B File Offset: 0x000D347B
		public DataBindingHandlerAttribute()
		{
			this._typeName = string.Empty;
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x000D448E File Offset: 0x000D348E
		public DataBindingHandlerAttribute(Type type)
		{
			this._typeName = type.AssemblyQualifiedName;
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x000D44A2 File Offset: 0x000D34A2
		public DataBindingHandlerAttribute(string typeName)
		{
			this._typeName = typeName;
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06002FC3 RID: 12227 RVA: 0x000D44B1 File Offset: 0x000D34B1
		public string HandlerTypeName
		{
			get
			{
				if (this._typeName == null)
				{
					return string.Empty;
				}
				return this._typeName;
			}
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x000D44C8 File Offset: 0x000D34C8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DataBindingHandlerAttribute dataBindingHandlerAttribute = obj as DataBindingHandlerAttribute;
			return dataBindingHandlerAttribute != null && string.Compare(this.HandlerTypeName, dataBindingHandlerAttribute.HandlerTypeName, StringComparison.Ordinal) == 0;
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x000D44FC File Offset: 0x000D34FC
		public override int GetHashCode()
		{
			return this.HandlerTypeName.GetHashCode();
		}

		// Token: 0x040021F2 RID: 8690
		private string _typeName;

		// Token: 0x040021F3 RID: 8691
		public static readonly DataBindingHandlerAttribute Default = new DataBindingHandlerAttribute();
	}
}
