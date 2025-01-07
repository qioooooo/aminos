using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.ComponentModel.Design
{
	public class InheritanceService : IInheritanceService, IDisposable
	{
		public InheritanceService()
		{
			this.inheritedComponents = new Hashtable();
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.inheritedComponents != null)
			{
				this.inheritedComponents.Clear();
				this.inheritedComponents = null;
			}
		}

		public void AddInheritedComponents(IComponent component, IContainer container)
		{
			this.AddInheritedComponents(component.GetType(), component, container);
		}

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

		public InheritanceAttribute GetInheritanceAttribute(IComponent component)
		{
			InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)this.inheritedComponents[component];
			if (inheritanceAttribute == null)
			{
				inheritanceAttribute = InheritanceAttribute.Default;
			}
			return inheritanceAttribute;
		}

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

		private static TraceSwitch InheritanceServiceSwitch = new TraceSwitch("InheritanceService", "InheritanceService : Debug inheritance scan.");

		private Hashtable inheritedComponents;

		private IComponent addingComponent;

		private InheritanceAttribute addingAttribute;
	}
}
