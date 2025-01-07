using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Text;
using System.Web.Compilation;

namespace System.Web.UI.Design
{
	internal static class ControlLocalizer
	{
		private static bool IsPropertyLocalizable(PropertyDescriptor propertyDescriptor)
		{
			DesignerSerializationVisibilityAttribute designerSerializationVisibilityAttribute = (DesignerSerializationVisibilityAttribute)propertyDescriptor.Attributes[typeof(DesignerSerializationVisibilityAttribute)];
			if (designerSerializationVisibilityAttribute != null && designerSerializationVisibilityAttribute.Visibility == DesignerSerializationVisibility.Hidden)
			{
				return false;
			}
			LocalizableAttribute localizableAttribute = (LocalizableAttribute)propertyDescriptor.Attributes[typeof(LocalizableAttribute)];
			return localizableAttribute != null && localizableAttribute.IsLocalizable;
		}

		public static string LocalizeControl(Control control, IDesignTimeResourceWriter resourceWriter, out string newInnerContent)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (resourceWriter == null)
			{
				throw new ArgumentNullException("resourceWriter");
			}
			if (control.Site == null)
			{
				throw new InvalidOperationException();
			}
			IDesignerHost designerHost = (IDesignerHost)control.Site.GetService(typeof(IDesignerHost));
			IDesignerHost designerHost2 = new ControlLocalizer.LocalizationDesignerHost(designerHost);
			ControlDesigner controlDesigner = designerHost.GetDesigner(control) as ControlDesigner;
			Control control2 = controlDesigner.CreateClonedControl(designerHost2, false);
			((IControlDesignerAccessor)control2).SetOwnerControl(control);
			bool flag = ControlLocalizer.ShouldLocalizeInnerContents(control.Site, control);
			string text = ControlLocalizer.LocalizeControl(control2, designerHost2, resourceWriter, flag);
			if (flag)
			{
				newInnerContent = ControlSerializer.SerializeInnerContents(control2, designerHost2);
			}
			else
			{
				newInnerContent = null;
			}
			return text;
		}

		private static string LocalizeControl(Control control, IServiceProvider serviceProvider, IDesignTimeResourceWriter resourceWriter, bool shouldLocalizeInnerContent)
		{
			ResourceExpressionEditor resourceExpressionEditor = (ResourceExpressionEditor)ExpressionEditor.GetExpressionEditor("resources", serviceProvider);
			ControlBuilder controlBuilder = ((IControlBuilderAccessor)control).ControlBuilder;
			ObjectPersistData objectPersistData = controlBuilder.GetObjectPersistData();
			string resourceKey = controlBuilder.GetResourceKey();
			string text = ControlLocalizer.LocalizeObject(serviceProvider, control, objectPersistData, resourceExpressionEditor, resourceWriter, resourceKey, string.Empty, control, string.Empty, shouldLocalizeInnerContent, false, false);
			if (!string.Equals(resourceKey, text, StringComparison.OrdinalIgnoreCase))
			{
				controlBuilder.SetResourceKey(text);
			}
			if (objectPersistData != null)
			{
				foreach (object obj in objectPersistData.AllPropertyEntries)
				{
					PropertyEntry propertyEntry = (PropertyEntry)obj;
					BoundPropertyEntry boundPropertyEntry = propertyEntry as BoundPropertyEntry;
					if (boundPropertyEntry != null && !boundPropertyEntry.Generated)
					{
						string[] array = boundPropertyEntry.Name.Split(new char[] { '.' });
						if (array.Length > 1)
						{
							object obj2 = control;
							string[] array2 = array;
							int i = 0;
							while (i < array2.Length)
							{
								string text2 = array2[i];
								PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj2)[text2];
								if (propertyDescriptor == null)
								{
									break;
								}
								PersistenceModeAttribute persistenceModeAttribute = propertyDescriptor.Attributes[typeof(PersistenceModeAttribute)] as PersistenceModeAttribute;
								if (persistenceModeAttribute != PersistenceModeAttribute.Attribute)
								{
									if (!string.Equals(boundPropertyEntry.ExpressionPrefix, "resources", StringComparison.OrdinalIgnoreCase))
									{
										break;
									}
									ResourceExpressionFields resourceExpressionFields = boundPropertyEntry.ParsedExpressionData as ResourceExpressionFields;
									if (resourceExpressionFields != null && string.IsNullOrEmpty(resourceExpressionFields.ClassKey))
									{
										object obj3 = resourceExpressionEditor.EvaluateExpression(boundPropertyEntry.Expression, boundPropertyEntry.ParsedExpressionData, boundPropertyEntry.PropertyInfo.PropertyType, serviceProvider);
										if (obj3 == null)
										{
											object obj4;
											PropertyDescriptor complexProperty = ControlDesigner.GetComplexProperty(control, boundPropertyEntry.Name, out obj4);
											obj3 = complexProperty.GetValue(obj4);
										}
										resourceWriter.AddResource(resourceExpressionFields.ResourceKey, obj3);
										break;
									}
									break;
								}
								else
								{
									obj2 = propertyDescriptor.GetValue(obj2);
									i++;
								}
							}
						}
					}
				}
			}
			return text;
		}

		private static bool ShouldLocalizeInnerContents(IServiceProvider serviceProvider, object obj)
		{
			Control control = obj as Control;
			if (control == null)
			{
				return false;
			}
			IDesignerHost designerHost = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
			if (designerHost == null)
			{
				return false;
			}
			ControlDesigner controlDesigner = designerHost.GetDesigner(control) as ControlDesigner;
			return controlDesigner == null || controlDesigner.ReadOnlyInternal;
		}

		private static bool ParseChildren(Type controlType)
		{
			object[] customAttributes = controlType.GetCustomAttributes(typeof(ParseChildrenAttribute), true);
			return customAttributes != null && customAttributes.Length > 0 && ((ParseChildrenAttribute)customAttributes[0]).ChildrenAsProperties;
		}

		private static string LocalizeObject(IServiceProvider serviceProvider, object obj, ObjectPersistData persistData, ResourceExpressionEditor resEditor, IDesignTimeResourceWriter resourceWriter, string resourceKey, string objectModelName, object topLevelObject, string filter, bool shouldLocalizeInnerContent, bool isComplexProperty, bool implicitlyLocalizeComplexProperty)
		{
			bool flag;
			if (isComplexProperty)
			{
				flag = implicitlyLocalizeComplexProperty;
			}
			else
			{
				flag = persistData == null || persistData.Localize;
			}
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
			for (int i = 0; i < properties.Count; i++)
			{
				try
				{
					PropertyDescriptor propertyDescriptor = properties[i];
					if (string.Equals(propertyDescriptor.Name, "Controls", StringComparison.Ordinal))
					{
						Control control = obj as Control;
						if (control != null && shouldLocalizeInnerContent)
						{
							if (!ControlLocalizer.ParseChildren(control.GetType()))
							{
								ControlCollection controls = control.Controls;
								foreach (object obj2 in controls)
								{
									Control control2 = (Control)obj2;
									IControlBuilderAccessor controlBuilderAccessor = control2;
									ControlBuilder controlBuilder = controlBuilderAccessor.ControlBuilder;
									if (controlBuilder != null)
									{
										string resourceKey2 = controlBuilder.GetResourceKey();
										string text = ControlLocalizer.LocalizeObject(serviceProvider, control2, controlBuilder.GetObjectPersistData(), resEditor, resourceWriter, resourceKey2, string.Empty, control2, string.Empty, true, false, false);
										if (!string.Equals(resourceKey2, text, StringComparison.OrdinalIgnoreCase))
										{
											controlBuilder.SetResourceKey(text);
										}
									}
								}
							}
							goto IL_07C1;
						}
					}
					PersistenceModeAttribute persistenceModeAttribute = (PersistenceModeAttribute)propertyDescriptor.Attributes[typeof(PersistenceModeAttribute)];
					string text2 = ((objectModelName.Length > 0) ? (objectModelName + '.' + propertyDescriptor.Name) : propertyDescriptor.Name);
					if (persistenceModeAttribute.Mode == PersistenceMode.Attribute && propertyDescriptor.SerializationVisibility == DesignerSerializationVisibility.Content)
					{
						resourceKey = ControlLocalizer.LocalizeObject(serviceProvider, propertyDescriptor.GetValue(obj), persistData, resEditor, resourceWriter, resourceKey, text2, topLevelObject, filter, true, true, flag);
					}
					else if (persistenceModeAttribute.Mode == PersistenceMode.Attribute || propertyDescriptor.PropertyType == typeof(string))
					{
						bool flag2 = false;
						bool flag3 = false;
						object obj3 = null;
						string text3 = string.Empty;
						if (persistData != null)
						{
							PropertyEntry filteredProperty = persistData.GetFilteredProperty(string.Empty, text2);
							if (filteredProperty is BoundPropertyEntry)
							{
								BoundPropertyEntry boundPropertyEntry = (BoundPropertyEntry)filteredProperty;
								if (!boundPropertyEntry.Generated)
								{
									if (string.Equals(boundPropertyEntry.ExpressionPrefix, "resources", StringComparison.OrdinalIgnoreCase))
									{
										ResourceExpressionFields resourceExpressionFields = boundPropertyEntry.ParsedExpressionData as ResourceExpressionFields;
										if (resourceExpressionFields != null && string.IsNullOrEmpty(resourceExpressionFields.ClassKey))
										{
											text3 = resourceExpressionFields.ResourceKey;
											obj3 = resEditor.EvaluateExpression(boundPropertyEntry.Expression, boundPropertyEntry.ParsedExpressionData, propertyDescriptor.PropertyType, serviceProvider);
											if (obj3 != null)
											{
												flag3 = true;
											}
											flag2 = true;
										}
									}
								}
								else
								{
									flag2 = true;
								}
							}
							else
							{
								flag2 = flag && ControlLocalizer.IsPropertyLocalizable(propertyDescriptor);
							}
						}
						else
						{
							flag2 = flag && ControlLocalizer.IsPropertyLocalizable(propertyDescriptor);
						}
						if (flag2)
						{
							if (!flag3)
							{
								obj3 = propertyDescriptor.GetValue(obj);
							}
							if (text3.Length == 0)
							{
								if (string.IsNullOrEmpty(resourceKey))
								{
									resourceKey = resourceWriter.CreateResourceKey(null, topLevelObject);
								}
								text3 = resourceKey + '.' + text2;
								if (filter.Length != 0)
								{
									text3 = filter + ':' + text3;
								}
							}
							resourceWriter.AddResource(text3, obj3);
						}
						if (persistData != null)
						{
							ICollection propertyAllFilters = persistData.GetPropertyAllFilters(text2);
							foreach (object obj4 in propertyAllFilters)
							{
								PropertyEntry propertyEntry = (PropertyEntry)obj4;
								if (propertyEntry.Filter.Length > 0)
								{
									if (propertyEntry is SimplePropertyEntry)
									{
										if (flag && ControlLocalizer.IsPropertyLocalizable(propertyDescriptor))
										{
											if (text3.Length == 0)
											{
												if (string.IsNullOrEmpty(resourceKey))
												{
													resourceKey = resourceWriter.CreateResourceKey(null, topLevelObject);
												}
												text3 = resourceKey + '.' + text2;
											}
											string text4 = propertyEntry.Filter + ':' + text3;
											resourceWriter.AddResource(text4, ((SimplePropertyEntry)propertyEntry).Value);
										}
									}
									else if (!(propertyEntry is ComplexPropertyEntry) && propertyEntry is BoundPropertyEntry)
									{
										BoundPropertyEntry boundPropertyEntry2 = (BoundPropertyEntry)propertyEntry;
										if (!boundPropertyEntry2.Generated && string.Equals(boundPropertyEntry2.ExpressionPrefix, "resources", StringComparison.OrdinalIgnoreCase))
										{
											ResourceExpressionFields resourceExpressionFields2 = boundPropertyEntry2.ParsedExpressionData as ResourceExpressionFields;
											if (resourceExpressionFields2 != null && string.IsNullOrEmpty(resourceExpressionFields2.ClassKey))
											{
												object obj5 = resEditor.EvaluateExpression(boundPropertyEntry2.Expression, boundPropertyEntry2.ParsedExpressionData, propertyEntry.PropertyInfo.PropertyType, serviceProvider);
												if (obj5 == null)
												{
													obj5 = string.Empty;
												}
												resourceWriter.AddResource(resourceExpressionFields2.ResourceKey, obj5);
											}
										}
									}
								}
							}
						}
					}
					else if (shouldLocalizeInnerContent)
					{
						if (typeof(ICollection).IsAssignableFrom(propertyDescriptor.PropertyType))
						{
							if (persistData != null)
							{
								ICollection propertyAllFilters2 = persistData.GetPropertyAllFilters(propertyDescriptor.Name);
								foreach (object obj6 in propertyAllFilters2)
								{
									ComplexPropertyEntry complexPropertyEntry = (ComplexPropertyEntry)obj6;
									ObjectPersistData objectPersistData = complexPropertyEntry.Builder.GetObjectPersistData();
									foreach (object obj7 in objectPersistData.CollectionItems)
									{
										ComplexPropertyEntry complexPropertyEntry2 = (ComplexPropertyEntry)obj7;
										ControlBuilder builder = complexPropertyEntry2.Builder;
										object obj8 = builder.BuildObject();
										string resourceKey3 = builder.GetResourceKey();
										string text5 = ControlLocalizer.LocalizeObject(serviceProvider, obj8, builder.GetObjectPersistData(), resEditor, resourceWriter, resourceKey3, string.Empty, obj8, string.Empty, true, false, false);
										if (!string.Equals(resourceKey3, text5, StringComparison.OrdinalIgnoreCase))
										{
											builder.SetResourceKey(text5);
										}
									}
								}
							}
						}
						else if (typeof(ITemplate).IsAssignableFrom(propertyDescriptor.PropertyType))
						{
							if (persistData != null)
							{
								ICollection propertyAllFilters3 = persistData.GetPropertyAllFilters(propertyDescriptor.Name);
								foreach (object obj9 in propertyAllFilters3)
								{
									TemplatePropertyEntry templatePropertyEntry = (TemplatePropertyEntry)obj9;
									TemplateBuilder templateBuilder = (TemplateBuilder)templatePropertyEntry.Builder;
									IDesignerHost designerHost = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
									Control[] array = ControlParser.ParseControls(designerHost, templateBuilder.Text);
									for (int j = 0; j < array.Length; j++)
									{
										if (!(array[j] is LiteralControl) && !(array[j] is DesignerDataBoundLiteralControl))
										{
											ControlLocalizer.LocalizeControl(array[j], serviceProvider, resourceWriter, true);
										}
									}
									StringBuilder stringBuilder = new StringBuilder();
									for (int k = 0; k < array.Length; k++)
									{
										if (array[k] is LiteralControl)
										{
											stringBuilder.Append(((LiteralControl)array[k]).Text);
										}
										else
										{
											stringBuilder.Append(ControlPersister.PersistControl(array[k], designerHost));
										}
									}
									templateBuilder.Text = stringBuilder.ToString();
								}
							}
						}
						else if (persistData != null)
						{
							object obj10 = propertyDescriptor.GetValue(obj);
							ObjectPersistData objectPersistData2 = null;
							ComplexPropertyEntry complexPropertyEntry3 = (ComplexPropertyEntry)persistData.GetFilteredProperty(string.Empty, propertyDescriptor.Name);
							if (complexPropertyEntry3 != null)
							{
								objectPersistData2 = complexPropertyEntry3.Builder.GetObjectPersistData();
							}
							resourceKey = ControlLocalizer.LocalizeObject(serviceProvider, obj10, objectPersistData2, resEditor, resourceWriter, resourceKey, text2, topLevelObject, string.Empty, true, true, flag);
							ICollection propertyAllFilters4 = persistData.GetPropertyAllFilters(propertyDescriptor.Name);
							foreach (object obj11 in propertyAllFilters4)
							{
								ComplexPropertyEntry complexPropertyEntry4 = (ComplexPropertyEntry)obj11;
								if (complexPropertyEntry4.Filter.Length > 0)
								{
									ControlBuilder builder2 = complexPropertyEntry4.Builder;
									objectPersistData2 = builder2.GetObjectPersistData();
									obj10 = builder2.BuildObject();
									resourceKey = ControlLocalizer.LocalizeObject(serviceProvider, obj10, objectPersistData2, resEditor, resourceWriter, resourceKey, text2, topLevelObject, complexPropertyEntry4.Filter, true, true, flag);
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					if (serviceProvider != null)
					{
						IComponentDesignerDebugService componentDesignerDebugService = serviceProvider.GetService(typeof(IComponentDesignerDebugService)) as IComponentDesignerDebugService;
						if (componentDesignerDebugService != null)
						{
							componentDesignerDebugService.Fail(ex.Message);
						}
					}
				}
				IL_07C1:;
			}
			return resourceKey;
		}

		private const string LocalizationResourceExpressionPrefix = "resources";

		private const char filterDelimiter = ':';

		private const char objectDelimiter = '.';

		private const char OMDelimiter = '.';

		private sealed class LocalizationDesignerHost : IDesignerHost, IServiceContainer, IServiceProvider
		{
			internal LocalizationDesignerHost(IDesignerHost parentHost)
			{
				this._parentHost = parentHost;
			}

			IContainer IDesignerHost.Container
			{
				get
				{
					return this._parentHost.Container;
				}
			}

			bool IDesignerHost.InTransaction
			{
				get
				{
					return this._parentHost.InTransaction;
				}
			}

			bool IDesignerHost.Loading
			{
				get
				{
					return this._parentHost.Loading;
				}
			}

			string IDesignerHost.TransactionDescription
			{
				get
				{
					return this._parentHost.TransactionDescription;
				}
			}

			IComponent IDesignerHost.RootComponent
			{
				get
				{
					return this._parentHost.RootComponent;
				}
			}

			string IDesignerHost.RootComponentClassName
			{
				get
				{
					return this._parentHost.RootComponentClassName;
				}
			}

			event EventHandler IDesignerHost.Activated
			{
				add
				{
					this._parentHost.Activated += value;
				}
				remove
				{
					this._parentHost.Activated -= value;
				}
			}

			event EventHandler IDesignerHost.Deactivated
			{
				add
				{
					this._parentHost.Deactivated += value;
				}
				remove
				{
					this._parentHost.Deactivated -= value;
				}
			}

			event EventHandler IDesignerHost.LoadComplete
			{
				add
				{
					this._parentHost.LoadComplete += value;
				}
				remove
				{
					this._parentHost.LoadComplete -= value;
				}
			}

			event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosed
			{
				add
				{
					this._parentHost.TransactionClosed += value;
				}
				remove
				{
					this._parentHost.TransactionClosed -= value;
				}
			}

			event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosing
			{
				add
				{
					this._parentHost.TransactionClosing += value;
				}
				remove
				{
					this._parentHost.TransactionClosing -= value;
				}
			}

			event EventHandler IDesignerHost.TransactionOpened
			{
				add
				{
					this._parentHost.TransactionOpened += value;
				}
				remove
				{
					this._parentHost.TransactionOpened -= value;
				}
			}

			event EventHandler IDesignerHost.TransactionOpening
			{
				add
				{
					this._parentHost.TransactionOpening += value;
				}
				remove
				{
					this._parentHost.TransactionOpening -= value;
				}
			}

			void IDesignerHost.Activate()
			{
			}

			IComponent IDesignerHost.CreateComponent(Type componentType)
			{
				return this._parentHost.CreateComponent(componentType);
			}

			IComponent IDesignerHost.CreateComponent(Type componentType, string name)
			{
				return this._parentHost.CreateComponent(componentType, name);
			}

			DesignerTransaction IDesignerHost.CreateTransaction()
			{
				return this._parentHost.CreateTransaction();
			}

			DesignerTransaction IDesignerHost.CreateTransaction(string description)
			{
				return this._parentHost.CreateTransaction(description);
			}

			void IDesignerHost.DestroyComponent(IComponent component)
			{
				this._parentHost.DestroyComponent(component);
			}

			Type IDesignerHost.GetType(string typeName)
			{
				return this._parentHost.GetType(typeName);
			}

			IDesigner IDesignerHost.GetDesigner(IComponent component)
			{
				return this._parentHost.GetDesigner(component);
			}

			void IServiceContainer.RemoveService(Type serviceType, bool promote)
			{
				this._parentHost.RemoveService(serviceType, promote);
			}

			void IServiceContainer.RemoveService(Type serviceType)
			{
				this._parentHost.RemoveService(serviceType);
			}

			void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
			{
				this._parentHost.AddService(serviceType, callback, promote);
			}

			void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback)
			{
				this._parentHost.AddService(serviceType, callback);
			}

			void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote)
			{
				this._parentHost.AddService(serviceType, serviceInstance, promote);
			}

			void IServiceContainer.AddService(Type serviceType, object serviceInstance)
			{
				this._parentHost.AddService(serviceType, serviceInstance);
			}

			object IServiceProvider.GetService(Type serviceType)
			{
				if (serviceType == typeof(IFilterResolutionService))
				{
					if (this._localizationFilterService == null)
					{
						IFilterResolutionService filterResolutionService = (IFilterResolutionService)this._parentHost.GetService(typeof(IFilterResolutionService));
						if (filterResolutionService == null)
						{
							throw new InvalidOperationException(SR.GetString("ControlLocalizer_RequiresFilterService"));
						}
						this._localizationFilterService = new ControlLocalizer.LocalizationDesignerHost.LocalizationFilterResolutionService(filterResolutionService);
					}
					return this._localizationFilterService;
				}
				return this._parentHost.GetService(serviceType);
			}

			private IDesignerHost _parentHost;

			private ControlLocalizer.LocalizationDesignerHost.LocalizationFilterResolutionService _localizationFilterService;

			private sealed class LocalizationFilterResolutionService : IFilterResolutionService
			{
				internal LocalizationFilterResolutionService(IFilterResolutionService realFilterService)
				{
					this._realFilterService = realFilterService;
				}

				int IFilterResolutionService.CompareFilters(string filter1, string filter2)
				{
					return this._realFilterService.CompareFilters(filter1, filter2);
				}

				bool IFilterResolutionService.EvaluateFilter(string filterName)
				{
					return filterName == null || filterName.Length == 0 || string.Equals(filterName, "default", StringComparison.OrdinalIgnoreCase);
				}

				private IFilterResolutionService _realFilterService;
			}
		}
	}
}
