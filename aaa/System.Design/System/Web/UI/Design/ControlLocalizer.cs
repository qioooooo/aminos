using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Text;
using System.Web.Compilation;

namespace System.Web.UI.Design
{
	// Token: 0x02000336 RID: 822
	internal static class ControlLocalizer
	{
		// Token: 0x06001F07 RID: 7943 RVA: 0x000AF274 File Offset: 0x000AE274
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

		// Token: 0x06001F08 RID: 7944 RVA: 0x000AF2D0 File Offset: 0x000AE2D0
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

		// Token: 0x06001F09 RID: 7945 RVA: 0x000AF374 File Offset: 0x000AE374
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

		// Token: 0x06001F0A RID: 7946 RVA: 0x000AF584 File Offset: 0x000AE584
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

		// Token: 0x06001F0B RID: 7947 RVA: 0x000AF5D4 File Offset: 0x000AE5D4
		private static bool ParseChildren(Type controlType)
		{
			object[] customAttributes = controlType.GetCustomAttributes(typeof(ParseChildrenAttribute), true);
			return customAttributes != null && customAttributes.Length > 0 && ((ParseChildrenAttribute)customAttributes[0]).ChildrenAsProperties;
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x000AF60C File Offset: 0x000AE60C
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

		// Token: 0x04001762 RID: 5986
		private const string LocalizationResourceExpressionPrefix = "resources";

		// Token: 0x04001763 RID: 5987
		private const char filterDelimiter = ':';

		// Token: 0x04001764 RID: 5988
		private const char objectDelimiter = '.';

		// Token: 0x04001765 RID: 5989
		private const char OMDelimiter = '.';

		// Token: 0x02000337 RID: 823
		private sealed class LocalizationDesignerHost : IDesignerHost, IServiceContainer, IServiceProvider
		{
			// Token: 0x06001F0D RID: 7949 RVA: 0x000AFE98 File Offset: 0x000AEE98
			internal LocalizationDesignerHost(IDesignerHost parentHost)
			{
				this._parentHost = parentHost;
			}

			// Token: 0x17000577 RID: 1399
			// (get) Token: 0x06001F0E RID: 7950 RVA: 0x000AFEA7 File Offset: 0x000AEEA7
			IContainer IDesignerHost.Container
			{
				get
				{
					return this._parentHost.Container;
				}
			}

			// Token: 0x17000578 RID: 1400
			// (get) Token: 0x06001F0F RID: 7951 RVA: 0x000AFEB4 File Offset: 0x000AEEB4
			bool IDesignerHost.InTransaction
			{
				get
				{
					return this._parentHost.InTransaction;
				}
			}

			// Token: 0x17000579 RID: 1401
			// (get) Token: 0x06001F10 RID: 7952 RVA: 0x000AFEC1 File Offset: 0x000AEEC1
			bool IDesignerHost.Loading
			{
				get
				{
					return this._parentHost.Loading;
				}
			}

			// Token: 0x1700057A RID: 1402
			// (get) Token: 0x06001F11 RID: 7953 RVA: 0x000AFECE File Offset: 0x000AEECE
			string IDesignerHost.TransactionDescription
			{
				get
				{
					return this._parentHost.TransactionDescription;
				}
			}

			// Token: 0x1700057B RID: 1403
			// (get) Token: 0x06001F12 RID: 7954 RVA: 0x000AFEDB File Offset: 0x000AEEDB
			IComponent IDesignerHost.RootComponent
			{
				get
				{
					return this._parentHost.RootComponent;
				}
			}

			// Token: 0x1700057C RID: 1404
			// (get) Token: 0x06001F13 RID: 7955 RVA: 0x000AFEE8 File Offset: 0x000AEEE8
			string IDesignerHost.RootComponentClassName
			{
				get
				{
					return this._parentHost.RootComponentClassName;
				}
			}

			// Token: 0x1400001E RID: 30
			// (add) Token: 0x06001F14 RID: 7956 RVA: 0x000AFEF5 File Offset: 0x000AEEF5
			// (remove) Token: 0x06001F15 RID: 7957 RVA: 0x000AFF03 File Offset: 0x000AEF03
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

			// Token: 0x1400001F RID: 31
			// (add) Token: 0x06001F16 RID: 7958 RVA: 0x000AFF11 File Offset: 0x000AEF11
			// (remove) Token: 0x06001F17 RID: 7959 RVA: 0x000AFF1F File Offset: 0x000AEF1F
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

			// Token: 0x14000020 RID: 32
			// (add) Token: 0x06001F18 RID: 7960 RVA: 0x000AFF2D File Offset: 0x000AEF2D
			// (remove) Token: 0x06001F19 RID: 7961 RVA: 0x000AFF3B File Offset: 0x000AEF3B
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

			// Token: 0x14000021 RID: 33
			// (add) Token: 0x06001F1A RID: 7962 RVA: 0x000AFF49 File Offset: 0x000AEF49
			// (remove) Token: 0x06001F1B RID: 7963 RVA: 0x000AFF57 File Offset: 0x000AEF57
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

			// Token: 0x14000022 RID: 34
			// (add) Token: 0x06001F1C RID: 7964 RVA: 0x000AFF65 File Offset: 0x000AEF65
			// (remove) Token: 0x06001F1D RID: 7965 RVA: 0x000AFF73 File Offset: 0x000AEF73
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

			// Token: 0x14000023 RID: 35
			// (add) Token: 0x06001F1E RID: 7966 RVA: 0x000AFF81 File Offset: 0x000AEF81
			// (remove) Token: 0x06001F1F RID: 7967 RVA: 0x000AFF8F File Offset: 0x000AEF8F
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

			// Token: 0x14000024 RID: 36
			// (add) Token: 0x06001F20 RID: 7968 RVA: 0x000AFF9D File Offset: 0x000AEF9D
			// (remove) Token: 0x06001F21 RID: 7969 RVA: 0x000AFFAB File Offset: 0x000AEFAB
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

			// Token: 0x06001F22 RID: 7970 RVA: 0x000AFFB9 File Offset: 0x000AEFB9
			void IDesignerHost.Activate()
			{
			}

			// Token: 0x06001F23 RID: 7971 RVA: 0x000AFFBB File Offset: 0x000AEFBB
			IComponent IDesignerHost.CreateComponent(Type componentType)
			{
				return this._parentHost.CreateComponent(componentType);
			}

			// Token: 0x06001F24 RID: 7972 RVA: 0x000AFFC9 File Offset: 0x000AEFC9
			IComponent IDesignerHost.CreateComponent(Type componentType, string name)
			{
				return this._parentHost.CreateComponent(componentType, name);
			}

			// Token: 0x06001F25 RID: 7973 RVA: 0x000AFFD8 File Offset: 0x000AEFD8
			DesignerTransaction IDesignerHost.CreateTransaction()
			{
				return this._parentHost.CreateTransaction();
			}

			// Token: 0x06001F26 RID: 7974 RVA: 0x000AFFE5 File Offset: 0x000AEFE5
			DesignerTransaction IDesignerHost.CreateTransaction(string description)
			{
				return this._parentHost.CreateTransaction(description);
			}

			// Token: 0x06001F27 RID: 7975 RVA: 0x000AFFF3 File Offset: 0x000AEFF3
			void IDesignerHost.DestroyComponent(IComponent component)
			{
				this._parentHost.DestroyComponent(component);
			}

			// Token: 0x06001F28 RID: 7976 RVA: 0x000B0001 File Offset: 0x000AF001
			Type IDesignerHost.GetType(string typeName)
			{
				return this._parentHost.GetType(typeName);
			}

			// Token: 0x06001F29 RID: 7977 RVA: 0x000B000F File Offset: 0x000AF00F
			IDesigner IDesignerHost.GetDesigner(IComponent component)
			{
				return this._parentHost.GetDesigner(component);
			}

			// Token: 0x06001F2A RID: 7978 RVA: 0x000B001D File Offset: 0x000AF01D
			void IServiceContainer.RemoveService(Type serviceType, bool promote)
			{
				this._parentHost.RemoveService(serviceType, promote);
			}

			// Token: 0x06001F2B RID: 7979 RVA: 0x000B002C File Offset: 0x000AF02C
			void IServiceContainer.RemoveService(Type serviceType)
			{
				this._parentHost.RemoveService(serviceType);
			}

			// Token: 0x06001F2C RID: 7980 RVA: 0x000B003A File Offset: 0x000AF03A
			void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
			{
				this._parentHost.AddService(serviceType, callback, promote);
			}

			// Token: 0x06001F2D RID: 7981 RVA: 0x000B004A File Offset: 0x000AF04A
			void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback)
			{
				this._parentHost.AddService(serviceType, callback);
			}

			// Token: 0x06001F2E RID: 7982 RVA: 0x000B0059 File Offset: 0x000AF059
			void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote)
			{
				this._parentHost.AddService(serviceType, serviceInstance, promote);
			}

			// Token: 0x06001F2F RID: 7983 RVA: 0x000B0069 File Offset: 0x000AF069
			void IServiceContainer.AddService(Type serviceType, object serviceInstance)
			{
				this._parentHost.AddService(serviceType, serviceInstance);
			}

			// Token: 0x06001F30 RID: 7984 RVA: 0x000B0078 File Offset: 0x000AF078
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

			// Token: 0x04001766 RID: 5990
			private IDesignerHost _parentHost;

			// Token: 0x04001767 RID: 5991
			private ControlLocalizer.LocalizationDesignerHost.LocalizationFilterResolutionService _localizationFilterService;

			// Token: 0x02000338 RID: 824
			private sealed class LocalizationFilterResolutionService : IFilterResolutionService
			{
				// Token: 0x06001F31 RID: 7985 RVA: 0x000B00E7 File Offset: 0x000AF0E7
				internal LocalizationFilterResolutionService(IFilterResolutionService realFilterService)
				{
					this._realFilterService = realFilterService;
				}

				// Token: 0x06001F32 RID: 7986 RVA: 0x000B00F6 File Offset: 0x000AF0F6
				int IFilterResolutionService.CompareFilters(string filter1, string filter2)
				{
					return this._realFilterService.CompareFilters(filter1, filter2);
				}

				// Token: 0x06001F33 RID: 7987 RVA: 0x000B0105 File Offset: 0x000AF105
				bool IFilterResolutionService.EvaluateFilter(string filterName)
				{
					return filterName == null || filterName.Length == 0 || string.Equals(filterName, "default", StringComparison.OrdinalIgnoreCase);
				}

				// Token: 0x04001768 RID: 5992
				private IFilterResolutionService _realFilterService;
			}
		}
	}
}
