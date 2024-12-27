using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x02000100 RID: 256
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[Serializable]
	public class ToolboxItem : ISerializable
	{
		// Token: 0x06000DC8 RID: 3528 RVA: 0x0002820C File Offset: 0x0002720C
		public ToolboxItem()
		{
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x00028214 File Offset: 0x00027214
		public ToolboxItem(Type toolType)
		{
			this.Initialize(toolType);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00028223 File Offset: 0x00027223
		private ToolboxItem(SerializationInfo info, StreamingContext context)
		{
			this.Deserialize(info, context);
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x00028233 File Offset: 0x00027233
		// (set) Token: 0x06000DCC RID: 3532 RVA: 0x0002824A File Offset: 0x0002724A
		public AssemblyName AssemblyName
		{
			get
			{
				return (AssemblyName)this.Properties["AssemblyName"];
			}
			set
			{
				this.Properties["AssemblyName"] = value;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x00028260 File Offset: 0x00027260
		// (set) Token: 0x06000DCE RID: 3534 RVA: 0x00028293 File Offset: 0x00027293
		public AssemblyName[] DependentAssemblies
		{
			get
			{
				AssemblyName[] array = (AssemblyName[])this.Properties["DependentAssemblies"];
				if (array != null)
				{
					return (AssemblyName[])array.Clone();
				}
				return null;
			}
			set
			{
				this.Properties["DependentAssemblies"] = value.Clone();
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x000282AB File Offset: 0x000272AB
		// (set) Token: 0x06000DD0 RID: 3536 RVA: 0x000282C2 File Offset: 0x000272C2
		public Bitmap Bitmap
		{
			get
			{
				return (Bitmap)this.Properties["Bitmap"];
			}
			set
			{
				this.Properties["Bitmap"] = value;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x000282D5 File Offset: 0x000272D5
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x000282EC File Offset: 0x000272EC
		public string Company
		{
			get
			{
				return (string)this.Properties["Company"];
			}
			set
			{
				this.Properties["Company"] = value;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x000282FF File Offset: 0x000272FF
		public virtual string ComponentType
		{
			get
			{
				return SR.GetString("DotNET_ComponentType");
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x0002830B File Offset: 0x0002730B
		// (set) Token: 0x06000DD5 RID: 3541 RVA: 0x00028322 File Offset: 0x00027322
		public string Description
		{
			get
			{
				return (string)this.Properties["Description"];
			}
			set
			{
				this.Properties["Description"] = value;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x00028335 File Offset: 0x00027335
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x0002834C File Offset: 0x0002734C
		public string DisplayName
		{
			get
			{
				return (string)this.Properties["DisplayName"];
			}
			set
			{
				this.Properties["DisplayName"] = value;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x0002835F File Offset: 0x0002735F
		// (set) Token: 0x06000DD9 RID: 3545 RVA: 0x00028376 File Offset: 0x00027376
		public ICollection Filter
		{
			get
			{
				return (ICollection)this.Properties["Filter"];
			}
			set
			{
				this.Properties["Filter"] = value;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x00028389 File Offset: 0x00027389
		// (set) Token: 0x06000DDB RID: 3547 RVA: 0x000283A0 File Offset: 0x000273A0
		public bool IsTransient
		{
			get
			{
				return (bool)this.Properties["IsTransient"];
			}
			set
			{
				this.Properties["IsTransient"] = value;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x000283B8 File Offset: 0x000273B8
		public virtual bool Locked
		{
			get
			{
				return this.locked;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x000283C0 File Offset: 0x000273C0
		public IDictionary Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new ToolboxItem.LockableDictionary(this, 8);
				}
				return this.properties;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000DDE RID: 3550 RVA: 0x000283DD File Offset: 0x000273DD
		// (set) Token: 0x06000DDF RID: 3551 RVA: 0x000283F4 File Offset: 0x000273F4
		public string TypeName
		{
			get
			{
				return (string)this.Properties["TypeName"];
			}
			set
			{
				this.Properties["TypeName"] = value;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x00028407 File Offset: 0x00027407
		public virtual string Version
		{
			get
			{
				if (this.AssemblyName != null)
				{
					return this.AssemblyName.Version.ToString();
				}
				return string.Empty;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000DE1 RID: 3553 RVA: 0x00028427 File Offset: 0x00027427
		// (remove) Token: 0x06000DE2 RID: 3554 RVA: 0x00028440 File Offset: 0x00027440
		public event ToolboxComponentsCreatedEventHandler ComponentsCreated
		{
			add
			{
				this.componentsCreatedEvent = (ToolboxComponentsCreatedEventHandler)Delegate.Combine(this.componentsCreatedEvent, value);
			}
			remove
			{
				this.componentsCreatedEvent = (ToolboxComponentsCreatedEventHandler)Delegate.Remove(this.componentsCreatedEvent, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000DE3 RID: 3555 RVA: 0x00028459 File Offset: 0x00027459
		// (remove) Token: 0x06000DE4 RID: 3556 RVA: 0x00028472 File Offset: 0x00027472
		public event ToolboxComponentsCreatingEventHandler ComponentsCreating
		{
			add
			{
				this.componentsCreatingEvent = (ToolboxComponentsCreatingEventHandler)Delegate.Combine(this.componentsCreatingEvent, value);
			}
			remove
			{
				this.componentsCreatingEvent = (ToolboxComponentsCreatingEventHandler)Delegate.Remove(this.componentsCreatingEvent, value);
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0002848B File Offset: 0x0002748B
		protected void CheckUnlocked()
		{
			if (this.Locked)
			{
				throw new InvalidOperationException(SR.GetString("ToolboxItemLocked"));
			}
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x000284A5 File Offset: 0x000274A5
		public IComponent[] CreateComponents()
		{
			return this.CreateComponents(null);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x000284B0 File Offset: 0x000274B0
		public IComponent[] CreateComponents(IDesignerHost host)
		{
			this.OnComponentsCreating(new ToolboxComponentsCreatingEventArgs(host));
			IComponent[] array = this.CreateComponentsCore(host, new Hashtable());
			if (array != null && array.Length > 0)
			{
				this.OnComponentsCreated(new ToolboxComponentsCreatedEventArgs(array));
			}
			return array;
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x000284EC File Offset: 0x000274EC
		public IComponent[] CreateComponents(IDesignerHost host, IDictionary defaultValues)
		{
			this.OnComponentsCreating(new ToolboxComponentsCreatingEventArgs(host));
			IComponent[] array = this.CreateComponentsCore(host, defaultValues);
			if (array != null && array.Length > 0)
			{
				this.OnComponentsCreated(new ToolboxComponentsCreatedEventArgs(array));
			}
			return array;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00028524 File Offset: 0x00027524
		protected virtual IComponent[] CreateComponentsCore(IDesignerHost host)
		{
			ArrayList arrayList = new ArrayList();
			Type type = this.GetType(host, this.AssemblyName, this.TypeName, true);
			if (type != null)
			{
				if (host != null)
				{
					arrayList.Add(host.CreateComponent(type));
				}
				else if (typeof(IComponent).IsAssignableFrom(type))
				{
					arrayList.Add(TypeDescriptor.CreateInstance(null, type, null, null));
				}
			}
			IComponent[] array = new IComponent[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0002859C File Offset: 0x0002759C
		protected virtual IComponent[] CreateComponentsCore(IDesignerHost host, IDictionary defaultValues)
		{
			IComponent[] array = this.CreateComponentsCore(host);
			if (host != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					IComponentInitializer componentInitializer = host.GetDesigner(array[i]) as IComponentInitializer;
					if (componentInitializer != null)
					{
						bool flag = true;
						try
						{
							componentInitializer.InitializeNewComponent(defaultValues);
							flag = false;
						}
						finally
						{
							if (flag)
							{
								for (int j = 0; j < array.Length; j++)
								{
									host.DestroyComponent(array[j]);
								}
							}
						}
					}
				}
			}
			return array;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00028614 File Offset: 0x00027614
		protected virtual void Deserialize(SerializationInfo info, StreamingContext context)
		{
			string[] array = null;
			foreach (SerializationEntry serializationEntry in info)
			{
				if (serializationEntry.Name.Equals("PropertyNames"))
				{
					array = serializationEntry.Value as string[];
					break;
				}
			}
			if (array == null)
			{
				array = new string[] { "AssemblyName", "Bitmap", "DisplayName", "Filter", "IsTransient", "TypeName" };
			}
			foreach (SerializationEntry serializationEntry2 in info)
			{
				foreach (string text in array)
				{
					if (text.Equals(serializationEntry2.Name))
					{
						this.Properties[serializationEntry2.Name] = serializationEntry2.Value;
						break;
					}
				}
			}
			bool boolean = info.GetBoolean("Locked");
			if (boolean)
			{
				this.Lock();
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0002871C File Offset: 0x0002771C
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			ToolboxItem toolboxItem = (ToolboxItem)obj;
			if (this.TypeName != toolboxItem.TypeName)
			{
				if (this.TypeName == null || toolboxItem.TypeName == null)
				{
					return false;
				}
				if (!this.TypeName.Equals(toolboxItem.TypeName))
				{
					return false;
				}
			}
			if (this.AssemblyName != toolboxItem.AssemblyName)
			{
				if (this.AssemblyName == null || toolboxItem.AssemblyName == null)
				{
					return false;
				}
				if (!this.AssemblyName.FullName.Equals(toolboxItem.AssemblyName.FullName))
				{
					return false;
				}
			}
			if (this.DisplayName != toolboxItem.DisplayName)
			{
				if (this.DisplayName == null || toolboxItem.DisplayName == null)
				{
					return false;
				}
				if (!this.DisplayName.Equals(toolboxItem.DisplayName))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00028800 File Offset: 0x00027800
		public override int GetHashCode()
		{
			int num = 0;
			if (this.TypeName != null)
			{
				num ^= this.TypeName.GetHashCode();
			}
			return num ^ this.DisplayName.GetHashCode();
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00028834 File Offset: 0x00027834
		protected virtual object FilterPropertyValue(string propertyName, object value)
		{
			if (propertyName != null)
			{
				if (!(propertyName == "AssemblyName"))
				{
					if (!(propertyName == "DisplayName") && !(propertyName == "TypeName"))
					{
						if (!(propertyName == "Filter"))
						{
							if (propertyName == "IsTransient")
							{
								if (value == null)
								{
									value = false;
								}
							}
						}
						else if (value == null)
						{
							value = new ToolboxItemFilterAttribute[0];
						}
					}
					else if (value == null)
					{
						value = string.Empty;
					}
				}
				else if (value != null)
				{
					value = ((AssemblyName)value).Clone();
				}
			}
			return value;
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x000288C0 File Offset: 0x000278C0
		public Type GetType(IDesignerHost host)
		{
			return this.GetType(host, this.AssemblyName, this.TypeName, false);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x000288D8 File Offset: 0x000278D8
		protected virtual Type GetType(IDesignerHost host, AssemblyName assemblyName, string typeName, bool reference)
		{
			ITypeResolutionService typeResolutionService = null;
			Type type = null;
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (host != null)
			{
				typeResolutionService = (ITypeResolutionService)host.GetService(typeof(ITypeResolutionService));
			}
			if (typeResolutionService != null)
			{
				if (reference)
				{
					if (assemblyName != null)
					{
						typeResolutionService.ReferenceAssembly(assemblyName);
						type = typeResolutionService.GetType(typeName);
					}
					else
					{
						type = typeResolutionService.GetType(typeName);
						if (type == null)
						{
							type = Type.GetType(typeName);
						}
						if (type != null)
						{
							typeResolutionService.ReferenceAssembly(type.Assembly.GetName());
						}
					}
				}
				else
				{
					if (assemblyName != null)
					{
						Assembly assembly = typeResolutionService.GetAssembly(assemblyName);
						if (assembly != null)
						{
							type = assembly.GetType(typeName);
						}
					}
					if (type == null)
					{
						type = typeResolutionService.GetType(typeName);
					}
				}
			}
			else if (!string.IsNullOrEmpty(typeName))
			{
				if (assemblyName != null)
				{
					Assembly assembly2 = null;
					try
					{
						assembly2 = Assembly.Load(assemblyName);
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
					if (assembly2 == null && assemblyName.CodeBase != null && assemblyName.CodeBase.Length > 0)
					{
						try
						{
							assembly2 = Assembly.LoadFrom(assemblyName.CodeBase);
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
					}
					if (assembly2 != null)
					{
						type = assembly2.GetType(typeName);
					}
				}
				if (type == null)
				{
					type = Type.GetType(typeName, false);
				}
			}
			return type;
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00028A34 File Offset: 0x00027A34
		private AssemblyName GetNonRetargetedAssemblyName(Type type, AssemblyName policiedAssemblyName)
		{
			if (type == null || policiedAssemblyName == null)
			{
				return null;
			}
			if (type.Assembly.FullName == policiedAssemblyName.FullName)
			{
				return policiedAssemblyName;
			}
			foreach (AssemblyName assemblyName in type.Assembly.GetReferencedAssemblies())
			{
				if (assemblyName.FullName == policiedAssemblyName.FullName)
				{
					return assemblyName;
				}
			}
			foreach (AssemblyName assemblyName2 in type.Assembly.GetReferencedAssemblies())
			{
				if (assemblyName2.Name == policiedAssemblyName.Name)
				{
					return assemblyName2;
				}
			}
			foreach (AssemblyName assemblyName3 in type.Assembly.GetReferencedAssemblies())
			{
				try
				{
					Assembly assembly = Assembly.Load(assemblyName3);
					if (assembly != null && assembly.FullName == policiedAssemblyName.FullName)
					{
						return assemblyName3;
					}
				}
				catch
				{
				}
			}
			return null;
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00028B48 File Offset: 0x00027B48
		public virtual void Initialize(Type type)
		{
			this.CheckUnlocked();
			if (type != null)
			{
				this.TypeName = type.FullName;
				AssemblyName name = type.Assembly.GetName(true);
				if (type.Assembly.GlobalAssemblyCache)
				{
					name.CodeBase = null;
				}
				Dictionary<string, AssemblyName> dictionary = new Dictionary<string, AssemblyName>();
				for (Type type2 = type; type2 != null; type2 = type2.BaseType)
				{
					AssemblyName name2 = type2.Assembly.GetName(true);
					AssemblyName nonRetargetedAssemblyName = this.GetNonRetargetedAssemblyName(type, name2);
					if (nonRetargetedAssemblyName != null && !dictionary.ContainsKey(nonRetargetedAssemblyName.FullName))
					{
						dictionary[nonRetargetedAssemblyName.FullName] = nonRetargetedAssemblyName;
					}
				}
				AssemblyName[] array = new AssemblyName[dictionary.Count];
				int num = 0;
				foreach (AssemblyName assemblyName in dictionary.Values)
				{
					array[num++] = assemblyName;
				}
				this.DependentAssemblies = array;
				this.AssemblyName = name;
				this.DisplayName = type.Name;
				if (!type.Assembly.ReflectionOnly)
				{
					object[] customAttributes = type.Assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						AssemblyCompanyAttribute assemblyCompanyAttribute = customAttributes[0] as AssemblyCompanyAttribute;
						if (assemblyCompanyAttribute != null && assemblyCompanyAttribute.Company != null)
						{
							this.Company = assemblyCompanyAttribute.Company;
						}
					}
					DescriptionAttribute descriptionAttribute = (DescriptionAttribute)TypeDescriptor.GetAttributes(type)[typeof(DescriptionAttribute)];
					if (descriptionAttribute != null)
					{
						this.Description = descriptionAttribute.Description;
					}
					ToolboxBitmapAttribute toolboxBitmapAttribute = (ToolboxBitmapAttribute)TypeDescriptor.GetAttributes(type)[typeof(ToolboxBitmapAttribute)];
					if (toolboxBitmapAttribute != null)
					{
						Bitmap bitmap = toolboxBitmapAttribute.GetImage(type, false) as Bitmap;
						if (bitmap != null && (bitmap.Width != 16 || bitmap.Height != 16))
						{
							bitmap = new Bitmap(bitmap, new Size(16, 16));
						}
						this.Bitmap = bitmap;
					}
					bool flag = false;
					ArrayList arrayList = new ArrayList();
					foreach (object obj in TypeDescriptor.GetAttributes(type))
					{
						Attribute attribute = (Attribute)obj;
						ToolboxItemFilterAttribute toolboxItemFilterAttribute = attribute as ToolboxItemFilterAttribute;
						if (toolboxItemFilterAttribute != null)
						{
							if (toolboxItemFilterAttribute.FilterString.Equals(this.TypeName))
							{
								flag = true;
							}
							arrayList.Add(toolboxItemFilterAttribute);
						}
					}
					if (!flag)
					{
						arrayList.Add(new ToolboxItemFilterAttribute(this.TypeName));
					}
					this.Filter = (ToolboxItemFilterAttribute[])arrayList.ToArray(typeof(ToolboxItemFilterAttribute));
				}
			}
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00028DF0 File Offset: 0x00027DF0
		public virtual void Lock()
		{
			this.locked = true;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00028DF9 File Offset: 0x00027DF9
		protected virtual void OnComponentsCreated(ToolboxComponentsCreatedEventArgs args)
		{
			if (this.componentsCreatedEvent != null)
			{
				this.componentsCreatedEvent(this, args);
			}
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00028E10 File Offset: 0x00027E10
		protected virtual void OnComponentsCreating(ToolboxComponentsCreatingEventArgs args)
		{
			if (this.componentsCreatingEvent != null)
			{
				this.componentsCreatingEvent(this, args);
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00028E28 File Offset: 0x00027E28
		protected virtual void Serialize(SerializationInfo info, StreamingContext context)
		{
			bool traceVerbose = ToolboxItem.ToolboxItemPersist.TraceVerbose;
			info.AddValue("Locked", this.Locked);
			ArrayList arrayList = new ArrayList(this.Properties.Count);
			foreach (object obj in this.Properties)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				arrayList.Add(dictionaryEntry.Key);
				info.AddValue((string)dictionaryEntry.Key, dictionaryEntry.Value);
			}
			info.AddValue("PropertyNames", (string[])arrayList.ToArray(typeof(string)));
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00028EF0 File Offset: 0x00027EF0
		public override string ToString()
		{
			return this.DisplayName;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00028EF8 File Offset: 0x00027EF8
		protected void ValidatePropertyType(string propertyName, object value, Type expectedType, bool allowNull)
		{
			if (value == null)
			{
				if (!allowNull)
				{
					throw new ArgumentNullException("value");
				}
			}
			else if (!expectedType.IsInstanceOfType(value))
			{
				throw new ArgumentException(SR.GetString("ToolboxItemInvalidPropertyType", new object[] { propertyName, expectedType.FullName }), "value");
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00028F4C File Offset: 0x00027F4C
		protected virtual object ValidatePropertyValue(string propertyName, object value)
		{
			switch (propertyName)
			{
			case "AssemblyName":
				this.ValidatePropertyType(propertyName, value, typeof(AssemblyName), true);
				break;
			case "Bitmap":
				this.ValidatePropertyType(propertyName, value, typeof(Bitmap), true);
				break;
			case "Company":
			case "Description":
			case "DisplayName":
			case "TypeName":
				this.ValidatePropertyType(propertyName, value, typeof(string), true);
				if (value == null)
				{
					value = string.Empty;
				}
				break;
			case "Filter":
			{
				this.ValidatePropertyType(propertyName, value, typeof(ICollection), true);
				int num2 = 0;
				ICollection collection = (ICollection)value;
				if (collection != null)
				{
					foreach (object obj in collection)
					{
						if (obj is ToolboxItemFilterAttribute)
						{
							num2++;
						}
					}
				}
				ToolboxItemFilterAttribute[] array = new ToolboxItemFilterAttribute[num2];
				if (collection != null)
				{
					num2 = 0;
					foreach (object obj2 in collection)
					{
						ToolboxItemFilterAttribute toolboxItemFilterAttribute = obj2 as ToolboxItemFilterAttribute;
						if (toolboxItemFilterAttribute != null)
						{
							array[num2++] = toolboxItemFilterAttribute;
						}
					}
				}
				value = array;
				break;
			}
			case "IsTransient":
				this.ValidatePropertyType(propertyName, value, typeof(bool), false);
				break;
			}
			return value;
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00029158 File Offset: 0x00028158
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			IntSecurity.UnmanagedCode.Demand();
			this.Serialize(info, context);
		}

		// Token: 0x04000B6F RID: 2927
		private static TraceSwitch ToolboxItemPersist = new TraceSwitch("ToolboxPersisting", "ToolboxItem: write data");

		// Token: 0x04000B70 RID: 2928
		private static object EventComponentsCreated = new object();

		// Token: 0x04000B71 RID: 2929
		private static object EventComponentsCreating = new object();

		// Token: 0x04000B72 RID: 2930
		private bool locked;

		// Token: 0x04000B73 RID: 2931
		private ToolboxItem.LockableDictionary properties;

		// Token: 0x04000B74 RID: 2932
		private ToolboxComponentsCreatedEventHandler componentsCreatedEvent;

		// Token: 0x04000B75 RID: 2933
		private ToolboxComponentsCreatingEventHandler componentsCreatingEvent;

		// Token: 0x02000101 RID: 257
		private class LockableDictionary : Hashtable
		{
			// Token: 0x06000DFC RID: 3580 RVA: 0x00029196 File Offset: 0x00028196
			internal LockableDictionary(ToolboxItem item, int capacity)
				: base(capacity)
			{
				this._item = item;
			}

			// Token: 0x1700038A RID: 906
			// (get) Token: 0x06000DFD RID: 3581 RVA: 0x000291A6 File Offset: 0x000281A6
			public override bool IsFixedSize
			{
				get
				{
					return this._item.Locked;
				}
			}

			// Token: 0x1700038B RID: 907
			// (get) Token: 0x06000DFE RID: 3582 RVA: 0x000291B3 File Offset: 0x000281B3
			public override bool IsReadOnly
			{
				get
				{
					return this._item.Locked;
				}
			}

			// Token: 0x1700038C RID: 908
			public override object this[object key]
			{
				get
				{
					string propertyName = this.GetPropertyName(key);
					object obj = base[propertyName];
					return this._item.FilterPropertyValue(propertyName, obj);
				}
				set
				{
					string propertyName = this.GetPropertyName(key);
					value = this._item.ValidatePropertyValue(propertyName, value);
					this.CheckSerializable(value);
					this._item.CheckUnlocked();
					base[propertyName] = value;
				}
			}

			// Token: 0x06000E01 RID: 3585 RVA: 0x0002922C File Offset: 0x0002822C
			public override void Add(object key, object value)
			{
				string propertyName = this.GetPropertyName(key);
				value = this._item.ValidatePropertyValue(propertyName, value);
				this.CheckSerializable(value);
				this._item.CheckUnlocked();
				base.Add(propertyName, value);
			}

			// Token: 0x06000E02 RID: 3586 RVA: 0x0002926C File Offset: 0x0002826C
			private void CheckSerializable(object value)
			{
				if (value != null && !value.GetType().IsSerializable)
				{
					throw new ArgumentException(SR.GetString("ToolboxItemValueNotSerializable", new object[] { value.GetType().FullName }));
				}
			}

			// Token: 0x06000E03 RID: 3587 RVA: 0x000292AF File Offset: 0x000282AF
			public override void Clear()
			{
				this._item.CheckUnlocked();
				base.Clear();
			}

			// Token: 0x06000E04 RID: 3588 RVA: 0x000292C4 File Offset: 0x000282C4
			private string GetPropertyName(object key)
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				string text = key as string;
				if (text == null || text.Length == 0)
				{
					throw new ArgumentException(SR.GetString("ToolboxItemInvalidKey"), "key");
				}
				return text;
			}

			// Token: 0x06000E05 RID: 3589 RVA: 0x00029307 File Offset: 0x00028307
			public override void Remove(object key)
			{
				this._item.CheckUnlocked();
				base.Remove(key);
			}

			// Token: 0x04000B76 RID: 2934
			private ToolboxItem _item;
		}
	}
}
