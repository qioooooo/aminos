using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.ComponentModel.Design
{
	// Token: 0x0200012F RID: 303
	public class InheritanceService : IInheritanceService, IDisposable
	{
		// Token: 0x06000BCF RID: 3023 RVA: 0x0002E0E4 File Offset: 0x0002D0E4
		public InheritanceService()
		{
			this.inheritedComponents = new Hashtable();
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0002E0F7 File Offset: 0x0002D0F7
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x0002E100 File Offset: 0x0002D100
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.inheritedComponents != null)
			{
				this.inheritedComponents.Clear();
				this.inheritedComponents = null;
			}
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0002E11F File Offset: 0x0002D11F
		public void AddInheritedComponents(IComponent component, IContainer container)
		{
			this.AddInheritedComponents(component.GetType(), component, container);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0002E130 File Offset: 0x0002D130
		protected virtual void AddInheritedComponents(Type type, IComponent component, IContainer container)
		{
			if (type == null || !typeof(IComponent).IsAssignableFrom(type))
			{
				return;
			}
			ISite site = component.Site;
			IComponentChangeService componentChangeService = null;
			INameCreationService nameCreationService = null;
			if (site != null)
			{
				nameCreationService = (INameCreationService)site.GetService(typeof(INameCreationService));
				componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentAdding += this.OnComponentAdding;
				}
			}
			try
			{
				while (type != typeof(object))
				{
					Type reflectionType = TypeDescriptor.GetReflectionType(type);
					foreach (FieldInfo fieldInfo in reflectionType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					{
						string text = fieldInfo.Name;
						if (typeof(IComponent).IsAssignableFrom(fieldInfo.FieldType))
						{
							object value = fieldInfo.GetValue(component);
							if (value != null)
							{
								MemberInfo memberInfo = fieldInfo;
								object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(AccessedThroughPropertyAttribute), false);
								if (customAttributes != null && customAttributes.Length > 0)
								{
									AccessedThroughPropertyAttribute accessedThroughPropertyAttribute = (AccessedThroughPropertyAttribute)customAttributes[0];
									PropertyInfo property = reflectionType.GetProperty(accessedThroughPropertyAttribute.PropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
									if (property != null && property.PropertyType == fieldInfo.FieldType)
									{
										if (!property.CanRead)
										{
											goto IL_0214;
										}
										memberInfo = property.GetGetMethod(true);
										text = accessedThroughPropertyAttribute.PropertyName;
									}
								}
								bool flag = this.IgnoreInheritedMember(memberInfo, component);
								bool flag2 = false;
								if (flag)
								{
									flag2 = true;
								}
								else if (memberInfo is FieldInfo)
								{
									FieldInfo fieldInfo2 = (FieldInfo)memberInfo;
									flag2 = fieldInfo2.IsPrivate | fieldInfo2.IsAssembly;
								}
								else if (memberInfo is MethodInfo)
								{
									MethodInfo methodInfo = (MethodInfo)memberInfo;
									flag2 = methodInfo.IsPrivate | methodInfo.IsAssembly;
								}
								InheritanceAttribute inheritanceAttribute;
								if (flag2)
								{
									inheritanceAttribute = InheritanceAttribute.InheritedReadOnly;
								}
								else
								{
									inheritanceAttribute = InheritanceAttribute.Inherited;
								}
								bool flag3 = this.inheritedComponents[value] == null;
								this.inheritedComponents[value] = inheritanceAttribute;
								if (!flag && flag3)
								{
									try
									{
										this.addingComponent = (IComponent)value;
										this.addingAttribute = inheritanceAttribute;
										if (nameCreationService != null)
										{
											if (!nameCreationService.IsValidName(text))
											{
												goto IL_0203;
											}
										}
										try
										{
											container.Add((IComponent)value, text);
										}
										catch
										{
										}
										IL_0203:;
									}
									finally
									{
										this.addingComponent = null;
										this.addingAttribute = null;
									}
								}
							}
						}
						IL_0214:;
					}
					type = type.BaseType;
				}
			}
			finally
			{
				if (componentChangeService != null)
				{
					componentChangeService.ComponentAdding -= this.OnComponentAdding;
				}
			}
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0002E3E0 File Offset: 0x0002D3E0
		protected virtual bool IgnoreInheritedMember(MemberInfo member, IComponent component)
		{
			if (member is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				return fieldInfo.IsPrivate || fieldInfo.IsAssembly;
			}
			if (member is MethodInfo)
			{
				MethodInfo methodInfo = (MethodInfo)member;
				return methodInfo.IsPrivate || methodInfo.IsAssembly;
			}
			return true;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0002E430 File Offset: 0x0002D430
		public InheritanceAttribute GetInheritanceAttribute(IComponent component)
		{
			InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)this.inheritedComponents[component];
			if (inheritanceAttribute == null)
			{
				inheritanceAttribute = InheritanceAttribute.Default;
			}
			return inheritanceAttribute;
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0002E45C File Offset: 0x0002D45C
		private void OnComponentAdding(object sender, ComponentEventArgs ce)
		{
			if (this.addingComponent != null && this.addingComponent != ce.Component)
			{
				this.inheritedComponents[ce.Component] = InheritanceAttribute.InheritedReadOnly;
				INestedContainer nestedContainer = sender as INestedContainer;
				if (nestedContainer != null && nestedContainer.Owner == this.addingComponent)
				{
					this.inheritedComponents[ce.Component] = this.addingAttribute;
				}
			}
		}

		// Token: 0x04000E61 RID: 3681
		private static TraceSwitch InheritanceServiceSwitch = new TraceSwitch("InheritanceService", "InheritanceService : Debug inheritance scan.");

		// Token: 0x04000E62 RID: 3682
		private Hashtable inheritedComponents;

		// Token: 0x04000E63 RID: 3683
		private IComponent addingComponent;

		// Token: 0x04000E64 RID: 3684
		private InheritanceAttribute addingAttribute;
	}
}
