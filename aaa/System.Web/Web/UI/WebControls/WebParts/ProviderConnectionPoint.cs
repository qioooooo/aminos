using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006EE RID: 1774
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ProviderConnectionPoint : ConnectionPoint
	{
		// Token: 0x060056D0 RID: 22224 RVA: 0x0015E360 File Offset: 0x0015D360
		static ProviderConnectionPoint()
		{
			ConstructorInfo constructorInfo = typeof(ProviderConnectionPoint).GetConstructors()[0];
			ProviderConnectionPoint.ConstructorTypes = WebPartUtil.GetTypesForConstructor(constructorInfo);
		}

		// Token: 0x060056D1 RID: 22225 RVA: 0x0015E38A File Offset: 0x0015D38A
		public ProviderConnectionPoint(MethodInfo callbackMethod, Type interfaceType, Type controlType, string displayName, string id, bool allowsMultipleConnections)
			: base(callbackMethod, interfaceType, controlType, displayName, id, allowsMultipleConnections)
		{
		}

		// Token: 0x060056D2 RID: 22226 RVA: 0x0015E39B File Offset: 0x0015D39B
		public virtual ConnectionInterfaceCollection GetSecondaryInterfaces(Control control)
		{
			return ConnectionInterfaceCollection.Empty;
		}

		// Token: 0x060056D3 RID: 22227 RVA: 0x0015E3A2 File Offset: 0x0015D3A2
		public virtual object GetObject(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			return base.CallbackMethod.Invoke(control, null);
		}

		// Token: 0x04002F77 RID: 12151
		internal static readonly Type[] ConstructorTypes;
	}
}
