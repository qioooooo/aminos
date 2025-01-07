using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class GlobalDataBindingHandler
	{
		private GlobalDataBindingHandler()
		{
		}

		private static Hashtable DataBindingHandlerTable
		{
			get
			{
				if (GlobalDataBindingHandler.dataBindingHandlerTable == null)
				{
					GlobalDataBindingHandler.dataBindingHandlerTable = new Hashtable();
				}
				return GlobalDataBindingHandler.dataBindingHandlerTable;
			}
		}

		public static void OnDataBind(object sender, EventArgs e)
		{
			Control control = (Control)sender;
			IDataBindingsAccessor dataBindingsAccessor = (IDataBindingsAccessor)sender;
			if (!dataBindingsAccessor.HasDataBindings)
			{
				return;
			}
			DataBindingHandlerAttribute dataBindingHandlerAttribute = (DataBindingHandlerAttribute)TypeDescriptor.GetAttributes(sender)[typeof(DataBindingHandlerAttribute)];
			if (dataBindingHandlerAttribute == null || dataBindingHandlerAttribute.HandlerTypeName.Length == 0)
			{
				return;
			}
			ISite site = control.Site;
			IDesignerHost designerHost = null;
			if (site == null)
			{
				Page page = control.Page;
				if (page != null)
				{
					site = page.Site;
				}
				else
				{
					Control control2 = control.Parent;
					while (site == null && control2 != null)
					{
						if (control2.Site != null)
						{
							site = control2.Site;
						}
						control2 = control2.Parent;
					}
				}
			}
			if (site != null)
			{
				designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
			}
			if (designerHost == null)
			{
				return;
			}
			IDesigner designer = designerHost.GetDesigner(control);
			if (designer != null)
			{
				return;
			}
			DataBindingHandler dataBindingHandler = null;
			try
			{
				string handlerTypeName = dataBindingHandlerAttribute.HandlerTypeName;
				dataBindingHandler = (DataBindingHandler)GlobalDataBindingHandler.DataBindingHandlerTable[handlerTypeName];
				if (dataBindingHandler == null)
				{
					Type type = Type.GetType(handlerTypeName);
					if (type != null)
					{
						dataBindingHandler = (DataBindingHandler)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null);
						GlobalDataBindingHandler.DataBindingHandlerTable[handlerTypeName] = dataBindingHandler;
					}
				}
			}
			catch (Exception)
			{
				return;
			}
			if (dataBindingHandler != null)
			{
				dataBindingHandler.DataBindControl(designerHost, control);
			}
		}

		public static readonly EventHandler Handler = new EventHandler(GlobalDataBindingHandler.OnDataBind);

		private static Hashtable dataBindingHandlerTable;
	}
}
