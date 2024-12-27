using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200046A RID: 1130
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class LoginStatusDesigner : CompositeControlDesigner
	{
		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x0600290F RID: 10511 RVA: 0x000E13E8 File Offset: 0x000E03E8
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new LoginStatusDesigner.LoginStatusDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06002910 RID: 10512 RVA: 0x000E1415 File Offset: 0x000E0415
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x000E1418 File Offset: 0x000E0418
		public override string GetDesignTimeHtml()
		{
			IDictionary dictionary = new HybridDictionary(2);
			dictionary["LoggedIn"] = this._loggedIn;
			LoginStatus loginStatus = (LoginStatus)base.ViewControl;
			((IControlDesignerAccessor)loginStatus).SetDesignModeState(dictionary);
			if (this._loggedIn)
			{
				string text = loginStatus.LogoutText;
				bool flag = text == null || text.Length == 0 || text == " ";
				if (flag)
				{
					loginStatus.LogoutText = "[" + loginStatus.ID + "]";
				}
			}
			else
			{
				string text = loginStatus.LoginText;
				bool flag = text == null || text.Length == 0 || text == " ";
				if (flag)
				{
					loginStatus.LoginText = "[" + loginStatus.ID + "]";
				}
			}
			return base.GetDesignTimeHtml();
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x000E14E3 File Offset: 0x000E04E3
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(LoginStatus));
			this._loginStatus = (LoginStatus)component;
			base.Initialize(component);
		}

		// Token: 0x04001C58 RID: 7256
		private bool _loggedIn;

		// Token: 0x04001C59 RID: 7257
		private LoginStatus _loginStatus;

		// Token: 0x0200046B RID: 1131
		private class LoginStatusDesignerActionList : DesignerActionList
		{
			// Token: 0x06002914 RID: 10516 RVA: 0x000E1510 File Offset: 0x000E0510
			public LoginStatusDesignerActionList(LoginStatusDesigner designer)
				: base(designer.Component)
			{
				this._designer = designer;
			}

			// Token: 0x1700079D RID: 1949
			// (get) Token: 0x06002915 RID: 10517 RVA: 0x000E1525 File Offset: 0x000E0525
			// (set) Token: 0x06002916 RID: 10518 RVA: 0x000E1528 File Offset: 0x000E0528
			public override bool AutoShow
			{
				get
				{
					return true;
				}
				set
				{
				}
			}

			// Token: 0x1700079E RID: 1950
			// (get) Token: 0x06002917 RID: 10519 RVA: 0x000E152A File Offset: 0x000E052A
			// (set) Token: 0x06002918 RID: 10520 RVA: 0x000E1550 File Offset: 0x000E0550
			[TypeConverter(typeof(LoginStatusDesigner.LoginStatusDesignerActionList.LoginStatusViewTypeConverter))]
			public string View
			{
				get
				{
					if (this._designer._loggedIn)
					{
						return SR.GetString("LoginStatus_LoggedInView");
					}
					return SR.GetString("LoginStatus_LoggedOutView");
				}
				set
				{
					if (string.Compare(value, SR.GetString("LoginStatus_LoggedInView"), StringComparison.Ordinal) == 0)
					{
						this._designer._loggedIn = true;
					}
					else if (string.Compare(value, SR.GetString("LoginStatus_LoggedOutView"), StringComparison.Ordinal) == 0)
					{
						this._designer._loggedIn = false;
					}
					this._designer.UpdateDesignTimeHtml();
				}
			}

			// Token: 0x06002919 RID: 10521 RVA: 0x000E15A8 File Offset: 0x000E05A8
			public override DesignerActionItemCollection GetSortedActionItems()
			{
				return new DesignerActionItemCollection
				{
					new DesignerActionPropertyItem("View", SR.GetString("WebControls_Views"), string.Empty, SR.GetString("WebControls_ViewsDescription"))
				};
			}

			// Token: 0x04001C5A RID: 7258
			private LoginStatusDesigner _designer;

			// Token: 0x0200046C RID: 1132
			private class LoginStatusViewTypeConverter : TypeConverter
			{
				// Token: 0x0600291A RID: 10522 RVA: 0x000E15E8 File Offset: 0x000E05E8
				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return new TypeConverter.StandardValuesCollection(new string[]
					{
						SR.GetString("LoginStatus_LoggedOutView"),
						SR.GetString("LoginStatus_LoggedInView")
					});
				}

				// Token: 0x0600291B RID: 10523 RVA: 0x000E161C File Offset: 0x000E061C
				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return true;
				}

				// Token: 0x0600291C RID: 10524 RVA: 0x000E161F File Offset: 0x000E061F
				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
			}
		}
	}
}
