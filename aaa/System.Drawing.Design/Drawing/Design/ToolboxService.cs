using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Drawing.Design
{
	// Token: 0x0200001E RID: 30
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class ToolboxService : IToolboxService, IComponentDiscoveryService
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000C3 RID: 195
		protected abstract CategoryNameCollection CategoryNames { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000C4 RID: 196
		// (set) Token: 0x060000C5 RID: 197
		protected abstract string SelectedCategory { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C6 RID: 198
		// (set) Token: 0x060000C7 RID: 199
		protected abstract ToolboxItemContainer SelectedItemContainer { get; set; }

		// Token: 0x060000C8 RID: 200 RVA: 0x00005BC7 File Offset: 0x00004BC7
		protected virtual ToolboxItemContainer CreateItemContainer(ToolboxItem item, IDesignerHost link)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (link != null)
			{
				return null;
			}
			return new ToolboxItemContainer(item);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005BE2 File Offset: 0x00004BE2
		protected virtual ToolboxItemContainer CreateItemContainer(IDataObject dataObject)
		{
			if (dataObject == null)
			{
				throw new ArgumentNullException("dataObject");
			}
			return new ToolboxItemContainer(dataObject);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005BF8 File Offset: 0x00004BF8
		protected virtual void FilterChanged()
		{
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005BFC File Offset: 0x00004BFC
		private ICollection GetCreatorCollection(IDesignerHost host)
		{
			if (host == null)
			{
				return this._globalCreators;
			}
			if (host != this._lastMergedHost)
			{
				ICollection collection = this._globalCreators;
				if (this._designerCreators != null)
				{
					ICollection collection2 = this._designerCreators[host] as ICollection;
					if (collection2 != null)
					{
						int num = collection2.Count;
						if (collection != null)
						{
							num += collection.Count;
						}
						ToolboxItemCreator[] array = new ToolboxItemCreator[num];
						collection2.CopyTo(array, 0);
						if (collection != null)
						{
							collection.CopyTo(array, collection2.Count);
						}
						collection = array;
					}
				}
				this._lastMergedCreators = collection;
				this._lastMergedHost = host;
			}
			return this._lastMergedCreators;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005C8C File Offset: 0x00004C8C
		private static ToolboxService.FilterSupport GetFilterSupport(ICollection itemFilter, ICollection targetFilter)
		{
			ToolboxService.FilterSupport filterSupport = ToolboxService.FilterSupport.Supported;
			int num = 0;
			int num2 = 0;
			foreach (object obj in itemFilter)
			{
				ToolboxItemFilterAttribute toolboxItemFilterAttribute = (ToolboxItemFilterAttribute)obj;
				if (filterSupport == ToolboxService.FilterSupport.NotSupported)
				{
					break;
				}
				if (toolboxItemFilterAttribute.FilterType == ToolboxItemFilterType.Require)
				{
					num++;
					using (IEnumerator enumerator2 = targetFilter.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							ToolboxItemFilterAttribute toolboxItemFilterAttribute2 = obj2 as ToolboxItemFilterAttribute;
							if (toolboxItemFilterAttribute2 != null && toolboxItemFilterAttribute.Match(toolboxItemFilterAttribute2))
							{
								num2++;
								break;
							}
						}
						continue;
					}
				}
				if (toolboxItemFilterAttribute.FilterType == ToolboxItemFilterType.Prevent)
				{
					using (IEnumerator enumerator3 = targetFilter.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							object obj3 = enumerator3.Current;
							ToolboxItemFilterAttribute toolboxItemFilterAttribute3 = obj3 as ToolboxItemFilterAttribute;
							if (toolboxItemFilterAttribute3 != null && toolboxItemFilterAttribute.Match(toolboxItemFilterAttribute3))
							{
								filterSupport = ToolboxService.FilterSupport.NotSupported;
								break;
							}
						}
						continue;
					}
				}
				if (filterSupport != ToolboxService.FilterSupport.Custom && toolboxItemFilterAttribute.FilterType == ToolboxItemFilterType.Custom)
				{
					if (toolboxItemFilterAttribute.FilterString.Length == 0)
					{
						filterSupport = ToolboxService.FilterSupport.Custom;
					}
					else
					{
						foreach (object obj4 in targetFilter)
						{
							ToolboxItemFilterAttribute toolboxItemFilterAttribute4 = (ToolboxItemFilterAttribute)obj4;
							if (toolboxItemFilterAttribute.FilterString.Equals(toolboxItemFilterAttribute4.FilterString))
							{
								filterSupport = ToolboxService.FilterSupport.Custom;
								break;
							}
						}
					}
				}
			}
			if (filterSupport != ToolboxService.FilterSupport.NotSupported && num > 0 && num2 == 0)
			{
				filterSupport = ToolboxService.FilterSupport.NotSupported;
			}
			if (filterSupport != ToolboxService.FilterSupport.NotSupported)
			{
				num = 0;
				num2 = 0;
				foreach (object obj5 in targetFilter)
				{
					ToolboxItemFilterAttribute toolboxItemFilterAttribute5 = (ToolboxItemFilterAttribute)obj5;
					if (filterSupport == ToolboxService.FilterSupport.NotSupported)
					{
						break;
					}
					if (toolboxItemFilterAttribute5.FilterType == ToolboxItemFilterType.Require)
					{
						num++;
						using (IEnumerator enumerator6 = itemFilter.GetEnumerator())
						{
							while (enumerator6.MoveNext())
							{
								object obj6 = enumerator6.Current;
								ToolboxItemFilterAttribute toolboxItemFilterAttribute6 = (ToolboxItemFilterAttribute)obj6;
								if (toolboxItemFilterAttribute5.Match(toolboxItemFilterAttribute6))
								{
									num2++;
									break;
								}
							}
							continue;
						}
					}
					if (toolboxItemFilterAttribute5.FilterType == ToolboxItemFilterType.Prevent)
					{
						using (IEnumerator enumerator7 = itemFilter.GetEnumerator())
						{
							while (enumerator7.MoveNext())
							{
								object obj7 = enumerator7.Current;
								ToolboxItemFilterAttribute toolboxItemFilterAttribute7 = (ToolboxItemFilterAttribute)obj7;
								if (toolboxItemFilterAttribute5.Match(toolboxItemFilterAttribute7))
								{
									filterSupport = ToolboxService.FilterSupport.NotSupported;
									break;
								}
							}
							continue;
						}
					}
					if (filterSupport != ToolboxService.FilterSupport.Custom && toolboxItemFilterAttribute5.FilterType == ToolboxItemFilterType.Custom)
					{
						if (toolboxItemFilterAttribute5.FilterString.Length == 0)
						{
							filterSupport = ToolboxService.FilterSupport.Custom;
						}
						else
						{
							foreach (object obj8 in itemFilter)
							{
								ToolboxItemFilterAttribute toolboxItemFilterAttribute8 = (ToolboxItemFilterAttribute)obj8;
								if (toolboxItemFilterAttribute5.FilterString.Equals(toolboxItemFilterAttribute8.FilterString))
								{
									filterSupport = ToolboxService.FilterSupport.Custom;
									break;
								}
							}
						}
					}
				}
				if (filterSupport != ToolboxService.FilterSupport.NotSupported && num > 0 && num2 == 0)
				{
					filterSupport = ToolboxService.FilterSupport.NotSupported;
				}
			}
			return filterSupport;
		}

		// Token: 0x060000CD RID: 205
		protected abstract IList GetItemContainers();

		// Token: 0x060000CE RID: 206
		protected abstract IList GetItemContainers(string categoryName);

		// Token: 0x060000CF RID: 207 RVA: 0x00006054 File Offset: 0x00005054
		public static ToolboxItem GetToolboxItem(Type toolType)
		{
			return ToolboxService.GetToolboxItem(toolType, false);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006060 File Offset: 0x00005060
		public static ToolboxItem GetToolboxItem(Type toolType, bool nonPublic)
		{
			ToolboxItem toolboxItem = null;
			if (toolType == null)
			{
				throw new ArgumentNullException("toolType");
			}
			if ((nonPublic || toolType.IsPublic || toolType.IsNestedPublic) && typeof(IComponent).IsAssignableFrom(toolType) && !toolType.IsAbstract)
			{
				ToolboxItemAttribute toolboxItemAttribute = (ToolboxItemAttribute)TypeDescriptor.GetAttributes(toolType)[typeof(ToolboxItemAttribute)];
				if (!toolboxItemAttribute.IsDefaultAttribute())
				{
					Type toolboxItemType = toolboxItemAttribute.ToolboxItemType;
					if (toolboxItemType != null)
					{
						ConstructorInfo constructorInfo = toolboxItemType.GetConstructor(new Type[] { typeof(Type) });
						if (constructorInfo != null && toolType != null)
						{
							toolboxItem = (ToolboxItem)constructorInfo.Invoke(new object[] { toolType });
						}
						else
						{
							constructorInfo = toolboxItemType.GetConstructor(new Type[0]);
							if (constructorInfo != null)
							{
								toolboxItem = (ToolboxItem)constructorInfo.Invoke(new object[0]);
								toolboxItem.Initialize(toolType);
							}
						}
					}
				}
				else if (!toolboxItemAttribute.Equals(ToolboxItemAttribute.None) && !toolType.ContainsGenericParameters)
				{
					toolboxItem = new ToolboxItem(toolType);
				}
			}
			else if (typeof(ToolboxItem).IsAssignableFrom(toolType))
			{
				toolboxItem = (ToolboxItem)Activator.CreateInstance(toolType, true);
			}
			return toolboxItem;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000618E File Offset: 0x0000518E
		public static ICollection GetToolboxItems(Assembly a, string newCodeBase)
		{
			return ToolboxService.GetToolboxItems(a, newCodeBase, false);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006198 File Offset: 0x00005198
		public static ICollection GetToolboxItems(Assembly a, string newCodeBase, bool throwOnError)
		{
			if (a == null)
			{
				throw new ArgumentNullException("a");
			}
			ArrayList arrayList = new ArrayList();
			AssemblyName assemblyName;
			if (a.GlobalAssemblyCache)
			{
				assemblyName = a.GetName();
				assemblyName.CodeBase = newCodeBase;
			}
			else
			{
				assemblyName = null;
			}
			try
			{
				foreach (Type type in a.GetTypes())
				{
					if (typeof(IComponent).IsAssignableFrom(type))
					{
						ConstructorInfo constructorInfo = type.GetConstructor(new Type[0]);
						if (constructorInfo == null)
						{
							constructorInfo = type.GetConstructor(new Type[] { typeof(IContainer) });
						}
						if (constructorInfo != null)
						{
							try
							{
								ToolboxItem toolboxItem = ToolboxService.GetToolboxItem(type);
								if (toolboxItem != null)
								{
									if (assemblyName != null)
									{
										toolboxItem.AssemblyName = assemblyName;
									}
									arrayList.Add(toolboxItem);
								}
							}
							catch
							{
								if (throwOnError)
								{
									throw;
								}
							}
						}
					}
				}
			}
			catch
			{
				if (throwOnError)
				{
					throw;
				}
			}
			return arrayList;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000628C File Offset: 0x0000528C
		public static ICollection GetToolboxItems(AssemblyName an)
		{
			return ToolboxService.GetToolboxItems(an, false);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006298 File Offset: 0x00005298
		public static ICollection GetToolboxItems(AssemblyName an, bool throwOnError)
		{
			if (ToolboxService._domainObject == null)
			{
				ToolboxService._domain = AppDomain.CreateDomain("Assembly Enumeration Domain");
				ToolboxService._domainObject = (ToolboxService.DomainProxyObject)ToolboxService._domain.CreateInstanceAndUnwrap(typeof(ToolboxService.DomainProxyObject).Assembly.FullName, typeof(ToolboxService.DomainProxyObject).FullName);
				ToolboxService._domainObjectSponsor = new ClientSponsor(new TimeSpan(0, 5, 0));
				ToolboxService._domainObjectSponsor.Register(ToolboxService._domainObject);
			}
			byte[] toolboxItems = ToolboxService._domainObject.GetToolboxItems(an, throwOnError);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			return (ICollection)binaryFormatter.Deserialize(new MemoryStream(toolboxItems));
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000633C File Offset: 0x0000533C
		protected virtual bool IsItemContainer(IDataObject dataObject, IDesignerHost host)
		{
			if (dataObject == null)
			{
				throw new ArgumentNullException("dataObject");
			}
			if (ToolboxItemContainer.ContainsFormat(dataObject))
			{
				return true;
			}
			ICollection creatorCollection = this.GetCreatorCollection(host);
			if (creatorCollection != null)
			{
				foreach (object obj in creatorCollection)
				{
					ToolboxItemCreator toolboxItemCreator = (ToolboxItemCreator)obj;
					if (dataObject.GetDataPresent(toolboxItemCreator.Format))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000063C4 File Offset: 0x000053C4
		protected bool IsItemContainerSupported(ToolboxItemContainer container, IDesignerHost host)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			ICollection creatorCollection = this.GetCreatorCollection(host);
			this._lastState = host.GetService(typeof(DesignerToolboxInfo)) as DesignerToolboxInfo;
			if (this._lastState == null)
			{
				this._lastState = new DesignerToolboxInfo(this, host);
				host.AddService(typeof(DesignerToolboxInfo), this._lastState);
			}
			switch (ToolboxService.GetFilterSupport(container.GetFilter(creatorCollection), this._lastState.Filter))
			{
			case ToolboxService.FilterSupport.NotSupported:
				return false;
			case ToolboxService.FilterSupport.Supported:
				return true;
			case ToolboxService.FilterSupport.Custom:
				if (this._lastState.ToolboxUser != null)
				{
					return this._lastState.ToolboxUser.GetToolSupported(container.GetToolboxItem(creatorCollection));
				}
				break;
			}
			return false;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006494 File Offset: 0x00005494
		internal void OnDesignerInfoChanged(DesignerToolboxInfo state)
		{
			if (this._designerEventService == null)
			{
				this._designerEventService = state.DesignerHost.GetService(typeof(IDesignerEventService)) as IDesignerEventService;
			}
			if (this._designerEventService != null && this._designerEventService.ActiveDesigner == state.DesignerHost)
			{
				this.FilterChanged();
			}
		}

		// Token: 0x060000D8 RID: 216
		protected abstract void Refresh();

		// Token: 0x060000D9 RID: 217 RVA: 0x000064EA File Offset: 0x000054EA
		protected virtual void SelectedItemContainerUsed()
		{
			this.SelectedItemContainer = null;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000064F3 File Offset: 0x000054F3
		protected virtual bool SetCursor()
		{
			if (this.SelectedItemContainer != null)
			{
				Cursor.Current = Cursors.Cross;
				return true;
			}
			return false;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000650C File Offset: 0x0000550C
		public static void UnloadToolboxItems()
		{
			if (ToolboxService._domain != null)
			{
				AppDomain domain = ToolboxService._domain;
				ToolboxService._domainObjectSponsor.Close();
				ToolboxService._domainObjectSponsor = null;
				ToolboxService._domainObject = null;
				ToolboxService._domain = null;
				AppDomain.Unload(domain);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00006548 File Offset: 0x00005548
		CategoryNameCollection IToolboxService.CategoryNames
		{
			get
			{
				return this.CategoryNames;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00006550 File Offset: 0x00005550
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00006558 File Offset: 0x00005558
		string IToolboxService.SelectedCategory
		{
			get
			{
				return this.SelectedCategory;
			}
			set
			{
				this.SelectedCategory = value;
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006564 File Offset: 0x00005564
		void IToolboxService.AddCreator(ToolboxItemCreatorCallback creator, string format)
		{
			if (creator == null)
			{
				throw new ArgumentNullException("creator");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (this._globalCreators == null)
			{
				this._globalCreators = new ArrayList();
			}
			this._globalCreators.Add(new ToolboxItemCreator(creator, format));
			this._lastMergedHost = null;
			this._lastMergedCreators = null;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000065C4 File Offset: 0x000055C4
		void IToolboxService.AddCreator(ToolboxItemCreatorCallback creator, string format, IDesignerHost host)
		{
			if (creator == null)
			{
				throw new ArgumentNullException("creator");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (this._designerCreators == null)
			{
				this._designerCreators = new Hashtable();
			}
			ArrayList arrayList = this._designerCreators[host] as ArrayList;
			if (arrayList == null)
			{
				arrayList = new ArrayList(4);
				this._designerCreators[host] = arrayList;
			}
			arrayList.Add(new ToolboxItemCreator(creator, format));
			this._lastMergedHost = null;
			this._lastMergedCreators = null;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006654 File Offset: 0x00005654
		void IToolboxService.AddLinkedToolboxItem(ToolboxItem toolboxItem, IDesignerHost host)
		{
			if (toolboxItem == null)
			{
				throw new ArgumentNullException("toolboxItem");
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			ToolboxItemContainer toolboxItemContainer = this.CreateItemContainer(toolboxItem, host);
			if (toolboxItemContainer != null)
			{
				this.GetItemContainers(this.SelectedCategory).Add(toolboxItemContainer);
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000669C File Offset: 0x0000569C
		void IToolboxService.AddLinkedToolboxItem(ToolboxItem toolboxItem, string category, IDesignerHost host)
		{
			if (toolboxItem == null)
			{
				throw new ArgumentNullException("toolboxItem");
			}
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			ToolboxItemContainer toolboxItemContainer = this.CreateItemContainer(toolboxItem, host);
			if (toolboxItemContainer != null)
			{
				this.GetItemContainers(category).Add(toolboxItemContainer);
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000066F0 File Offset: 0x000056F0
		void IToolboxService.AddToolboxItem(ToolboxItem toolboxItem)
		{
			if (toolboxItem == null)
			{
				throw new ArgumentNullException("toolboxItem");
			}
			ToolboxItemContainer toolboxItemContainer = this.CreateItemContainer(toolboxItem, null);
			if (toolboxItemContainer != null)
			{
				this.GetItemContainers(this.SelectedCategory).Add(toolboxItemContainer);
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000672C File Offset: 0x0000572C
		void IToolboxService.AddToolboxItem(ToolboxItem toolboxItem, string category)
		{
			if (toolboxItem == null)
			{
				throw new ArgumentNullException("toolboxItem");
			}
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			ToolboxItemContainer toolboxItemContainer = this.CreateItemContainer(toolboxItem, null);
			if (toolboxItemContainer != null)
			{
				this.GetItemContainers(category).Add(toolboxItemContainer);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006770 File Offset: 0x00005770
		ToolboxItem IToolboxService.DeserializeToolboxItem(object serializedObject)
		{
			if (serializedObject == null)
			{
				throw new ArgumentNullException("serializedObject");
			}
			IDataObject dataObject = serializedObject as IDataObject;
			if (dataObject == null)
			{
				dataObject = new DataObject(serializedObject);
			}
			ToolboxItemContainer toolboxItemContainer = this.CreateItemContainer(dataObject);
			if (toolboxItemContainer != null)
			{
				return toolboxItemContainer.GetToolboxItem(this.GetCreatorCollection(null));
			}
			return null;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000067B8 File Offset: 0x000057B8
		ToolboxItem IToolboxService.DeserializeToolboxItem(object serializedObject, IDesignerHost host)
		{
			if (serializedObject == null)
			{
				throw new ArgumentNullException("serializedObject");
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			IDataObject dataObject = serializedObject as IDataObject;
			if (dataObject == null)
			{
				dataObject = new DataObject(serializedObject);
			}
			ToolboxItemContainer toolboxItemContainer = this.CreateItemContainer(dataObject);
			if (toolboxItemContainer != null)
			{
				return toolboxItemContainer.GetToolboxItem(this.GetCreatorCollection(host));
			}
			return null;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000680C File Offset: 0x0000580C
		ToolboxItem IToolboxService.GetSelectedToolboxItem()
		{
			ToolboxItemContainer selectedItemContainer = this.SelectedItemContainer;
			if (selectedItemContainer != null)
			{
				return selectedItemContainer.GetToolboxItem(this.GetCreatorCollection(null));
			}
			return null;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006834 File Offset: 0x00005834
		ToolboxItem IToolboxService.GetSelectedToolboxItem(IDesignerHost host)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			ToolboxItemContainer selectedItemContainer = this.SelectedItemContainer;
			if (selectedItemContainer != null)
			{
				return selectedItemContainer.GetToolboxItem(this.GetCreatorCollection(host));
			}
			return null;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006868 File Offset: 0x00005868
		ToolboxItemCollection IToolboxService.GetToolboxItems()
		{
			IList itemContainers = this.GetItemContainers();
			ArrayList arrayList = new ArrayList(itemContainers.Count);
			ICollection creatorCollection = this.GetCreatorCollection(null);
			foreach (object obj in itemContainers)
			{
				ToolboxItemContainer toolboxItemContainer = (ToolboxItemContainer)obj;
				ToolboxItem toolboxItem = toolboxItemContainer.GetToolboxItem(creatorCollection);
				if (toolboxItem != null)
				{
					arrayList.Add(toolboxItem);
				}
			}
			ToolboxItem[] array = new ToolboxItem[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return new ToolboxItemCollection(array);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000690C File Offset: 0x0000590C
		ToolboxItemCollection IToolboxService.GetToolboxItems(IDesignerHost host)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			IList itemContainers = this.GetItemContainers();
			ArrayList arrayList = new ArrayList(itemContainers.Count);
			ICollection creatorCollection = this.GetCreatorCollection(host);
			foreach (object obj in itemContainers)
			{
				ToolboxItemContainer toolboxItemContainer = (ToolboxItemContainer)obj;
				ToolboxItem toolboxItem = toolboxItemContainer.GetToolboxItem(creatorCollection);
				if (toolboxItem != null)
				{
					arrayList.Add(toolboxItem);
				}
			}
			ToolboxItem[] array = new ToolboxItem[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return new ToolboxItemCollection(array);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000069BC File Offset: 0x000059BC
		ToolboxItemCollection IToolboxService.GetToolboxItems(string category)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			IList itemContainers = this.GetItemContainers(category);
			ArrayList arrayList = new ArrayList(itemContainers.Count);
			ICollection creatorCollection = this.GetCreatorCollection(null);
			foreach (object obj in itemContainers)
			{
				ToolboxItemContainer toolboxItemContainer = (ToolboxItemContainer)obj;
				ToolboxItem toolboxItem = toolboxItemContainer.GetToolboxItem(creatorCollection);
				if (toolboxItem != null)
				{
					arrayList.Add(toolboxItem);
				}
			}
			ToolboxItem[] array = new ToolboxItem[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return new ToolboxItemCollection(array);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006A70 File Offset: 0x00005A70
		ToolboxItemCollection IToolboxService.GetToolboxItems(string category, IDesignerHost host)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			IList itemContainers = this.GetItemContainers(category);
			ArrayList arrayList = new ArrayList(itemContainers.Count);
			ICollection creatorCollection = this.GetCreatorCollection(host);
			foreach (object obj in itemContainers)
			{
				ToolboxItemContainer toolboxItemContainer = (ToolboxItemContainer)obj;
				ToolboxItem toolboxItem = toolboxItemContainer.GetToolboxItem(creatorCollection);
				if (toolboxItem != null)
				{
					arrayList.Add(toolboxItem);
				}
			}
			ToolboxItem[] array = new ToolboxItem[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return new ToolboxItemCollection(array);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006B30 File Offset: 0x00005B30
		bool IToolboxService.IsSupported(object serializedObject, IDesignerHost host)
		{
			if (serializedObject == null)
			{
				throw new ArgumentNullException("serializedObject");
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			IDataObject dataObject = serializedObject as IDataObject;
			if (dataObject == null)
			{
				dataObject = new DataObject(serializedObject);
			}
			if (!this.IsItemContainer(dataObject, host))
			{
				return false;
			}
			ToolboxItemContainer toolboxItemContainer = this.CreateItemContainer(dataObject);
			return this.IsItemContainerSupported(toolboxItemContainer, host);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006B88 File Offset: 0x00005B88
		bool IToolboxService.IsSupported(object serializedObject, ICollection filterAttributes)
		{
			if (serializedObject == null)
			{
				throw new ArgumentNullException("serializedObject");
			}
			if (filterAttributes == null)
			{
				throw new ArgumentNullException("filterAttributes");
			}
			IDataObject dataObject = serializedObject as IDataObject;
			if (dataObject == null)
			{
				dataObject = new DataObject(serializedObject);
			}
			if (!this.IsItemContainer(dataObject, null))
			{
				return false;
			}
			ToolboxItemContainer toolboxItemContainer = this.CreateItemContainer(dataObject);
			return ToolboxService.GetFilterSupport(toolboxItemContainer.GetFilter(this.GetCreatorCollection(null)), filterAttributes) == ToolboxService.FilterSupport.Supported;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006BEC File Offset: 0x00005BEC
		bool IToolboxService.IsToolboxItem(object serializedObject)
		{
			if (serializedObject == null)
			{
				throw new ArgumentNullException("serializedObject");
			}
			IDataObject dataObject = serializedObject as IDataObject;
			if (dataObject == null)
			{
				dataObject = new DataObject(serializedObject);
			}
			return this.IsItemContainer(dataObject, null);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006C20 File Offset: 0x00005C20
		bool IToolboxService.IsToolboxItem(object serializedObject, IDesignerHost host)
		{
			if (serializedObject == null)
			{
				throw new ArgumentNullException("serializedObject");
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			IDataObject dataObject = serializedObject as IDataObject;
			if (dataObject == null)
			{
				dataObject = new DataObject(serializedObject);
			}
			return this.IsItemContainer(dataObject, host);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006C62 File Offset: 0x00005C62
		void IToolboxService.Refresh()
		{
			this.Refresh();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006C6C File Offset: 0x00005C6C
		void IToolboxService.RemoveCreator(string format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (this._globalCreators != null)
			{
				for (int i = 0; i < this._globalCreators.Count; i++)
				{
					ToolboxItemCreator toolboxItemCreator = this._globalCreators[i] as ToolboxItemCreator;
					if (toolboxItemCreator.Format.Equals(format))
					{
						this._globalCreators.RemoveAt(i);
						this._lastMergedHost = null;
						this._lastMergedCreators = null;
						return;
					}
				}
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006CE0 File Offset: 0x00005CE0
		void IToolboxService.RemoveCreator(string format, IDesignerHost host)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (this._designerCreators != null)
			{
				ArrayList arrayList = this._designerCreators[host] as ArrayList;
				if (arrayList != null)
				{
					for (int i = 0; i < arrayList.Count; i++)
					{
						ToolboxItemCreator toolboxItemCreator = arrayList[i] as ToolboxItemCreator;
						if (toolboxItemCreator.Format.Equals(format))
						{
							arrayList.RemoveAt(i);
							this._lastMergedHost = null;
							this._lastMergedCreators = null;
							return;
						}
					}
				}
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00006D68 File Offset: 0x00005D68
		void IToolboxService.RemoveToolboxItem(ToolboxItem toolboxItem)
		{
			if (toolboxItem == null)
			{
				throw new ArgumentNullException("toolboxItem");
			}
			this.GetItemContainers().Remove(this.CreateItemContainer(toolboxItem, null));
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006D8B File Offset: 0x00005D8B
		void IToolboxService.RemoveToolboxItem(ToolboxItem toolboxItem, string category)
		{
			if (toolboxItem == null)
			{
				throw new ArgumentNullException("toolboxItem");
			}
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			this.GetItemContainers(category).Remove(this.CreateItemContainer(toolboxItem, null));
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006DBD File Offset: 0x00005DBD
		void IToolboxService.SelectedToolboxItemUsed()
		{
			this.SelectedItemContainerUsed();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006DC5 File Offset: 0x00005DC5
		object IToolboxService.SerializeToolboxItem(ToolboxItem toolboxItem)
		{
			if (toolboxItem == null)
			{
				throw new ArgumentNullException("toolboxItem");
			}
			return this.CreateItemContainer(toolboxItem, null).ToolboxData;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006DE2 File Offset: 0x00005DE2
		bool IToolboxService.SetCursor()
		{
			return this.SetCursor();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006DEA File Offset: 0x00005DEA
		void IToolboxService.SetSelectedToolboxItem(ToolboxItem toolboxItem)
		{
			if (toolboxItem != null)
			{
				this.SelectedItemContainer = this.CreateItemContainer(toolboxItem, null);
				return;
			}
			this.SelectedItemContainer = null;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006E08 File Offset: 0x00005E08
		ICollection IComponentDiscoveryService.GetComponentTypes(IDesignerHost designerHost, Type baseType)
		{
			Hashtable hashtable = new Hashtable();
			ToolboxItemCollection toolboxItems = ((IToolboxService)this).GetToolboxItems();
			if (toolboxItems != null)
			{
				Type typeFromHandle = typeof(IComponent);
				foreach (object obj in toolboxItems)
				{
					ToolboxItem toolboxItem = (ToolboxItem)obj;
					Type type = toolboxItem.GetType(designerHost);
					if (type != null && typeFromHandle.IsAssignableFrom(type) && (baseType == null || baseType.IsAssignableFrom(type)))
					{
						hashtable[type] = type;
					}
				}
			}
			return hashtable.Values;
		}

		// Token: 0x040000D4 RID: 212
		private IDesignerEventService _designerEventService;

		// Token: 0x040000D5 RID: 213
		private ArrayList _globalCreators;

		// Token: 0x040000D6 RID: 214
		private Hashtable _designerCreators;

		// Token: 0x040000D7 RID: 215
		private IDesignerHost _lastMergedHost;

		// Token: 0x040000D8 RID: 216
		private ICollection _lastMergedCreators;

		// Token: 0x040000D9 RID: 217
		private DesignerToolboxInfo _lastState;

		// Token: 0x040000DA RID: 218
		private static ToolboxService.DomainProxyObject _domainObject;

		// Token: 0x040000DB RID: 219
		private static AppDomain _domain;

		// Token: 0x040000DC RID: 220
		private static ClientSponsor _domainObjectSponsor;

		// Token: 0x0200001F RID: 31
		private class DomainProxyObject : MarshalByRefObject
		{
			// Token: 0x060000FC RID: 252 RVA: 0x00006EB4 File Offset: 0x00005EB4
			internal byte[] GetToolboxItems(AssemblyName an, bool throwOnError)
			{
				Assembly assembly = null;
				try
				{
					assembly = Assembly.Load(an);
				}
				catch (FileNotFoundException)
				{
				}
				catch (BadImageFormatException)
				{
				}
				catch (IOException)
				{
				}
				if (assembly == null && an.CodeBase != null)
				{
					assembly = Assembly.LoadFrom(new Uri(an.CodeBase).LocalPath);
				}
				if (assembly == null)
				{
					throw new ArgumentException(SR.GetString("ToolboxServiceAssemblyNotFound", new object[] { an.FullName }));
				}
				ICollection collection = null;
				try
				{
					collection = ToolboxService.GetToolboxItems(assembly, null, throwOnError);
				}
				catch (Exception ex)
				{
					ReflectionTypeLoadException ex2 = ex as ReflectionTypeLoadException;
					if (ex2 != null)
					{
						throw new ReflectionTypeLoadException(null, ex2.LoaderExceptions, ex2.Message);
					}
					throw;
				}
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				MemoryStream memoryStream = new MemoryStream();
				binaryFormatter.Serialize(memoryStream, collection);
				memoryStream.Close();
				return memoryStream.GetBuffer();
			}
		}

		// Token: 0x02000020 RID: 32
		private enum FilterSupport
		{
			// Token: 0x040000DE RID: 222
			NotSupported,
			// Token: 0x040000DF RID: 223
			Supported,
			// Token: 0x040000E0 RID: 224
			Custom
		}
	}
}
