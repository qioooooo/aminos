using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006B8 RID: 1720
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ConsumerConnectionPoint : ConnectionPoint
	{
		// Token: 0x06005464 RID: 21604 RVA: 0x00157FAC File Offset: 0x00156FAC
		static ConsumerConnectionPoint()
		{
			ConstructorInfo constructorInfo = typeof(ConsumerConnectionPoint).GetConstructors()[0];
			ConsumerConnectionPoint.ConstructorTypes = WebPartUtil.GetTypesForConstructor(constructorInfo);
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x00157FD6 File Offset: 0x00156FD6
		public ConsumerConnectionPoint(MethodInfo callbackMethod, Type interfaceType, Type controlType, string displayName, string id, bool allowsMultipleConnections)
			: base(callbackMethod, interfaceType, controlType, displayName, id, allowsMultipleConnections)
		{
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x00157FE8 File Offset: 0x00156FE8
		public virtual void SetObject(Control control, object data)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			base.CallbackMethod.Invoke(control, new object[] { data });
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x0015801C File Offset: 0x0015701C
		public virtual bool SupportsConnection(Control control, ConnectionInterfaceCollection secondaryInterfaces)
		{
			return true;
		}

		// Token: 0x04002EE0 RID: 12000
		internal static readonly Type[] ConstructorTypes;
	}
}
