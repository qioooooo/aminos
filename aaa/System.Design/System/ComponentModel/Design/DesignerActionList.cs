using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.Design
{
	// Token: 0x02000101 RID: 257
	public class DesignerActionList
	{
		// Token: 0x06000A8B RID: 2699 RVA: 0x0002906A File Offset: 0x0002806A
		public DesignerActionList(IComponent component)
		{
			this._component = component;
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x00029079 File Offset: 0x00028079
		// (set) Token: 0x06000A8D RID: 2701 RVA: 0x00029081 File Offset: 0x00028081
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

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x00029093 File Offset: 0x00028093
		public IComponent Component
		{
			get
			{
				return this._component;
			}
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0002909B File Offset: 0x0002809B
		public object GetService(Type serviceType)
		{
			if (this._component != null && this._component.Site != null)
			{
				return this._component.Site.GetService(serviceType);
			}
			return null;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x000290C8 File Offset: 0x000280C8
		private object GetCustomAttribute(MemberInfo info, Type attributeType)
		{
			object[] customAttributes = info.GetCustomAttributes(attributeType, true);
			if (customAttributes.Length > 0)
			{
				return customAttributes[0];
			}
			return null;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x000290EC File Offset: 0x000280EC
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

		// Token: 0x06000A92 RID: 2706 RVA: 0x00029188 File Offset: 0x00028188
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

		// Token: 0x04000D95 RID: 3477
		private bool _autoShow;

		// Token: 0x04000D96 RID: 3478
		private IComponent _component;
	}
}
