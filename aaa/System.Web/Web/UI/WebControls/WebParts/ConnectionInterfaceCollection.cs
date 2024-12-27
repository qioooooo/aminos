using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006AF RID: 1711
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ConnectionInterfaceCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060053D8 RID: 21464 RVA: 0x0015469B File Offset: 0x0015369B
		public ConnectionInterfaceCollection()
		{
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x001546A3 File Offset: 0x001536A3
		public ConnectionInterfaceCollection(ICollection connectionInterfaces)
		{
			this.Initialize(null, connectionInterfaces);
		}

		// Token: 0x060053DA RID: 21466 RVA: 0x001546B3 File Offset: 0x001536B3
		public ConnectionInterfaceCollection(ConnectionInterfaceCollection existingConnectionInterfaces, ICollection connectionInterfaces)
		{
			this.Initialize(existingConnectionInterfaces, connectionInterfaces);
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x001546C4 File Offset: 0x001536C4
		private void Initialize(ConnectionInterfaceCollection existingConnectionInterfaces, ICollection connectionInterfaces)
		{
			if (existingConnectionInterfaces != null)
			{
				foreach (object obj in existingConnectionInterfaces)
				{
					Type type = (Type)obj;
					base.InnerList.Add(type);
				}
			}
			if (connectionInterfaces != null)
			{
				foreach (object obj2 in connectionInterfaces)
				{
					if (obj2 == null)
					{
						throw new ArgumentException(SR.GetString("Collection_CantAddNull"), "connectionInterfaces");
					}
					if (!(obj2 is Type))
					{
						throw new ArgumentException(SR.GetString("Collection_InvalidType", new object[] { "Type" }), "connectionInterfaces");
					}
					base.InnerList.Add(obj2);
				}
			}
		}

		// Token: 0x060053DC RID: 21468 RVA: 0x001547BC File Offset: 0x001537BC
		public bool Contains(Type value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x001547CA File Offset: 0x001537CA
		public int IndexOf(Type value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x17001560 RID: 5472
		public Type this[int index]
		{
			get
			{
				return (Type)base.InnerList[index];
			}
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x001547EB File Offset: 0x001537EB
		public void CopyTo(Type[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x04002E96 RID: 11926
		public static readonly ConnectionInterfaceCollection Empty = new ConnectionInterfaceCollection();
	}
}
