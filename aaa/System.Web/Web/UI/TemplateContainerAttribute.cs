using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200046F RID: 1135
	[AttributeUsage(AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TemplateContainerAttribute : Attribute
	{
		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x060035A6 RID: 13734 RVA: 0x000E7E36 File Offset: 0x000E6E36
		public BindingDirection BindingDirection
		{
			get
			{
				return this._bindingDirection;
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x060035A7 RID: 13735 RVA: 0x000E7E3E File Offset: 0x000E6E3E
		public Type ContainerType
		{
			get
			{
				return this._containerType;
			}
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x000E7E46 File Offset: 0x000E6E46
		public TemplateContainerAttribute(Type containerType)
			: this(containerType, BindingDirection.OneWay)
		{
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x000E7E50 File Offset: 0x000E6E50
		public TemplateContainerAttribute(Type containerType, BindingDirection bindingDirection)
		{
			this._containerType = containerType;
			this._bindingDirection = bindingDirection;
		}

		// Token: 0x04002542 RID: 9538
		private Type _containerType;

		// Token: 0x04002543 RID: 9539
		private BindingDirection _bindingDirection;
	}
}
