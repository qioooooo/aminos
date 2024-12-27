using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006B0 RID: 1712
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class ConnectionPoint
	{
		// Token: 0x060053E1 RID: 21473 RVA: 0x00154808 File Offset: 0x00153808
		internal ConnectionPoint(MethodInfo callbackMethod, Type interfaceType, Type controlType, string displayName, string id, bool allowsMultipleConnections)
		{
			if (callbackMethod == null)
			{
				throw new ArgumentNullException("callbackMethod");
			}
			if (interfaceType == null)
			{
				throw new ArgumentNullException("interfaceType");
			}
			if (controlType == null)
			{
				throw new ArgumentNullException("controlType");
			}
			if (!controlType.IsSubclassOf(typeof(Control)))
			{
				throw new ArgumentException(SR.GetString("ConnectionPoint_InvalidControlType"), "controlType");
			}
			if (string.IsNullOrEmpty(displayName))
			{
				throw new ArgumentNullException("displayName");
			}
			this._callbackMethod = callbackMethod;
			this._interfaceType = interfaceType;
			this._controlType = controlType;
			this._displayName = displayName;
			this._id = id;
			this._allowsMultipleConnections = allowsMultipleConnections;
		}

		// Token: 0x17001561 RID: 5473
		// (get) Token: 0x060053E2 RID: 21474 RVA: 0x001548AD File Offset: 0x001538AD
		public bool AllowsMultipleConnections
		{
			get
			{
				return this._allowsMultipleConnections;
			}
		}

		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x060053E3 RID: 21475 RVA: 0x001548B5 File Offset: 0x001538B5
		internal MethodInfo CallbackMethod
		{
			get
			{
				return this._callbackMethod;
			}
		}

		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x060053E4 RID: 21476 RVA: 0x001548BD File Offset: 0x001538BD
		public Type ControlType
		{
			get
			{
				return this._controlType;
			}
		}

		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x060053E5 RID: 21477 RVA: 0x001548C5 File Offset: 0x001538C5
		public Type InterfaceType
		{
			get
			{
				return this._interfaceType;
			}
		}

		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x060053E6 RID: 21478 RVA: 0x001548CD File Offset: 0x001538CD
		public string ID
		{
			get
			{
				if (string.IsNullOrEmpty(this._id))
				{
					return ConnectionPoint.DefaultID;
				}
				return this._id;
			}
		}

		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x060053E7 RID: 21479 RVA: 0x001548E8 File Offset: 0x001538E8
		public string DisplayName
		{
			get
			{
				return this._displayName;
			}
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x001548F0 File Offset: 0x001538F0
		public virtual bool GetEnabled(Control control)
		{
			return true;
		}

		// Token: 0x04002E97 RID: 11927
		internal const string DefaultIDInternal = "default";

		// Token: 0x04002E98 RID: 11928
		private MethodInfo _callbackMethod;

		// Token: 0x04002E99 RID: 11929
		private Type _controlType;

		// Token: 0x04002E9A RID: 11930
		private Type _interfaceType;

		// Token: 0x04002E9B RID: 11931
		private string _displayName;

		// Token: 0x04002E9C RID: 11932
		private string _id;

		// Token: 0x04002E9D RID: 11933
		private bool _allowsMultipleConnections;

		// Token: 0x04002E9E RID: 11934
		public static readonly string DefaultID = "default";
	}
}
