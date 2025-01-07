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
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class LoginStatusDesigner : CompositeControlDesigner
	{
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

		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

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

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(LoginStatus));
			this._loginStatus = (LoginStatus)component;
			base.Initialize(component);
		}

		private bool _loggedIn;

		private LoginStatus _loginStatus;

		private class LoginStatusDesignerActionList : DesignerActionList
		{
			public LoginStatusDesignerActionList(LoginStatusDesigner designer)
				: base(designer.Component)
			{
				this._designer = designer;
			}

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

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				return new DesignerActionItemCollection
				{
					new DesignerActionPropertyItem("View", SR.GetString("WebControls_Views"), string.Empty, SR.GetString("WebControls_ViewsDescription"))
				};
			}

			private LoginStatusDesigner _designer;

			private class LoginStatusViewTypeConverter : TypeConverter
			{
				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return new TypeConverter.StandardValuesCollection(new string[]
					{
						SR.GetString("LoginStatus_LoggedOutView"),
						SR.GetString("LoginStatus_LoggedInView")
					});
				}

				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return true;
				}

				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
			}
		}
	}
}
