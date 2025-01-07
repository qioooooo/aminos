using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.Design
{
	public class DesignerActionList
	{
		public DesignerActionList(IComponent component)
		{
			this._component = component;
		}

		public virtual bool AutoShow
		{
			get
			{
				return this._autoShow;
			}
			set
			{
				if (this._autoShow != value)
				{
					this._autoShow = value;
				}
			}
		}

		public IComponent Component
		{
			get
			{
				return this._component;
			}
		}

		public object GetService(Type serviceType)
		{
			if (this._component != null && this._component.Site != null)
			{
				return this._component.Site.GetService(serviceType);
			}
			return null;
		}

		private object GetCustomAttribute(MemberInfo info, Type attributeType)
		{
			object[] customAttributes = info.GetCustomAttributes(attributeType, true);
			if (customAttributes.Length > 0)
			{
				return customAttributes[0];
			}
			return null;
		}

		private void GetMemberDisplayProperties(MemberInfo info, out string displayName, out string description, out string category)
		{
			string text;
			category = (text = "");
			string text2;
			description = (text2 = text);
			displayName = text2;
			DescriptionAttribute descriptionAttribute = this.GetCustomAttribute(info, typeof(DescriptionAttribute)) as DescriptionAttribute;
			if (descriptionAttribute != null)
			{
				description = descriptionAttribute.Description;
			}
			DisplayNameAttribute displayNameAttribute = this.GetCustomAttribute(info, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
			if (displayNameAttribute != null)
			{
				displayName = displayNameAttribute.DisplayName;
			}
			CategoryAttribute categoryAttribute = this.GetCustomAttribute(info, typeof(CategoryAttribute)) as CategoryAttribute;
			if (displayNameAttribute != null)
			{
				category = categoryAttribute.Category;
			}
			if (string.IsNullOrEmpty(displayName))
			{
				displayName = info.Name;
			}
		}

		public virtual DesignerActionItemCollection GetSortedActionItems()
		{
			SortedList<string, DesignerActionItem> sortedList = new SortedList<string, DesignerActionItem>();
			IList<MethodInfo> list = Array.AsReadOnly<MethodInfo>(typeof(DesignerActionList).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod));
			IList<PropertyInfo> list2 = Array.AsReadOnly<PropertyInfo>(typeof(DesignerActionList).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod));
			MethodInfo[] methods = base.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
			foreach (MethodInfo methodInfo in methods)
			{
				if (!list.Contains(methodInfo) && methodInfo.GetParameters().Length == 0 && !methodInfo.IsSpecialName)
				{
					string text;
					string text2;
					string text3;
					this.GetMemberDisplayProperties(methodInfo, out text, out text2, out text3);
					sortedList.Add(methodInfo.Name, new DesignerActionMethodItem(this, methodInfo.Name, text, text3, text2));
				}
			}
			PropertyInfo[] properties = base.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (!list2.Contains(propertyInfo))
				{
					string text;
					string text2;
					string text3;
					this.GetMemberDisplayProperties(propertyInfo, out text, out text2, out text3);
					sortedList.Add(text, new DesignerActionPropertyItem(propertyInfo.Name, text, text3, text2));
				}
			}
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			foreach (DesignerActionItem designerActionItem in sortedList.Values)
			{
				designerActionItemCollection.Add(designerActionItem);
			}
			return designerActionItemCollection;
		}

		private bool _autoShow;

		private IComponent _component;
	}
}
