using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Drawing.Design
{
	// Token: 0x02000022 RID: 34
	[Serializable]
	public class ToolboxItemContainer : ISerializable
	{
		// Token: 0x06000101 RID: 257 RVA: 0x00006FE0 File Offset: 0x00005FE0
		protected ToolboxItemContainer(SerializationInfo info, StreamingContext context)
		{
			string[] array = (string[])info.GetValue("TbxIC_DataObjectFormats", typeof(string[]));
			object[] array2 = (object[])info.GetValue("TbxIC_DataObjectValues", typeof(object[]));
			DataObject dataObject = new DataObject();
			for (int i = 0; i < array.Length; i++)
			{
				dataObject.SetData(array[i], array2[i]);
			}
			this._dataObject = dataObject;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00007050 File Offset: 0x00006050
		public ToolboxItemContainer(ToolboxItem item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this._toolboxItem = item;
			this.UpdateFilter(item);
			this._hashCode = item.DisplayName.GetHashCode();
			if (item.AssemblyName != null)
			{
				this._hashCode ^= item.AssemblyName.GetHashCode();
			}
			if (item.TypeName != null)
			{
				this._hashCode ^= item.TypeName.GetHashCode();
			}
			if (this._hashCode == 0)
			{
				this._hashCode = item.DisplayName.GetHashCode();
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000070E9 File Offset: 0x000060E9
		public ToolboxItemContainer(IDataObject data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this._dataObject = data;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00007106 File Offset: 0x00006106
		public bool IsCreated
		{
			get
			{
				return this._toolboxItem != null;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00007114 File Offset: 0x00006114
		public bool IsTransient
		{
			get
			{
				return this._toolboxItem != null && this._toolboxItem.IsTransient;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000712C File Offset: 0x0000612C
		public virtual IDataObject ToolboxData
		{
			get
			{
				if (this._dataObject == null)
				{
					MemoryStream memoryStream = new MemoryStream();
					DataObject dataObject = new DataObject();
					BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
					binaryWriter.Write(1);
					binaryWriter.Write((short)this._filter.Count);
					foreach (object obj in this._filter)
					{
						ToolboxItemFilterAttribute toolboxItemFilterAttribute = (ToolboxItemFilterAttribute)obj;
						binaryWriter.Write(toolboxItemFilterAttribute.FilterString);
						binaryWriter.Write((short)toolboxItemFilterAttribute.FilterType);
					}
					binaryWriter.Flush();
					memoryStream.Close();
					dataObject.SetData("CF_TOOLBOXITEMCONTAINER", memoryStream.GetBuffer());
					dataObject.SetData("CF_TOOLBOXITEMCONTAINER_HASH", this._hashCode);
					dataObject.SetData("CF_TOOLBOXITEMCONTAINER_CONTENTS", new ToolboxItemContainer.ToolboxItemSerializer(this._toolboxItem));
					this._dataObject = dataObject;
				}
				return this._dataObject;
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000722C File Offset: 0x0000622C
		public void UpdateFilter(ToolboxItem item)
		{
			this._filter = ToolboxItemContainer.MergeFilter(item);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000723A File Offset: 0x0000623A
		internal static bool ContainsFormat(IDataObject dataObject)
		{
			return dataObject.GetDataPresent("CF_TOOLBOXITEMCONTAINER");
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00007248 File Offset: 0x00006248
		public override bool Equals(object obj)
		{
			ToolboxItemContainer toolboxItemContainer = obj as ToolboxItemContainer;
			if (toolboxItemContainer == this)
			{
				return true;
			}
			if (toolboxItemContainer == null)
			{
				return false;
			}
			if (this._toolboxItem != null && toolboxItemContainer._toolboxItem != null && this._toolboxItem.Equals(toolboxItemContainer._toolboxItem))
			{
				return true;
			}
			if (this._dataObject != null && toolboxItemContainer._dataObject != null && this._dataObject.Equals(toolboxItemContainer._dataObject))
			{
				return true;
			}
			ToolboxItem toolboxItem = this.GetToolboxItem(null);
			ToolboxItem toolboxItem2 = toolboxItemContainer.GetToolboxItem(null);
			return toolboxItem != null && toolboxItem2 != null && toolboxItem.Equals(toolboxItem2);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000072D0 File Offset: 0x000062D0
		public virtual ICollection GetFilter(ICollection creators)
		{
			ICollection collection = this._filter;
			if (this._filter == null)
			{
				if (this._dataObject.GetDataPresent("CF_TOOLBOXITEMCONTAINER"))
				{
					byte[] array = (byte[])this._dataObject.GetData("CF_TOOLBOXITEMCONTAINER");
					if (array != null)
					{
						BinaryReader binaryReader = new BinaryReader(new MemoryStream(array));
						short num = binaryReader.ReadInt16();
						if (num != 1)
						{
							this._filter = new ToolboxItemFilterAttribute[0];
						}
						else
						{
							short num2 = binaryReader.ReadInt16();
							ToolboxItemFilterAttribute[] array2 = new ToolboxItemFilterAttribute[(int)num2];
							for (short num3 = 0; num3 < num2; num3 += 1)
							{
								string text = binaryReader.ReadString();
								short num4 = binaryReader.ReadInt16();
								array2[(int)num3] = new ToolboxItemFilterAttribute(text, (ToolboxItemFilterType)num4);
							}
							this._filter = array2;
						}
					}
					else
					{
						this._filter = new ToolboxItemFilterAttribute[0];
					}
					collection = this._filter;
				}
				else if (creators != null)
				{
					foreach (object obj in creators)
					{
						ToolboxItemCreator toolboxItemCreator = (ToolboxItemCreator)obj;
						if (this._dataObject.GetDataPresent(toolboxItemCreator.Format))
						{
							ToolboxItem toolboxItem = toolboxItemCreator.Create(this._dataObject);
							if (toolboxItem != null)
							{
								collection = ToolboxItemContainer.MergeFilter(toolboxItem);
								break;
							}
						}
					}
				}
			}
			return collection;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00007420 File Offset: 0x00006420
		public override int GetHashCode()
		{
			if (this._hashCode == 0 && this._dataObject != null && this._dataObject.GetDataPresent("CF_TOOLBOXITEMCONTAINER_HASH"))
			{
				this._hashCode = (int)this._dataObject.GetData("CF_TOOLBOXITEMCONTAINER_HASH");
			}
			if (this._hashCode == 0)
			{
				this._hashCode = base.GetHashCode();
			}
			return this._hashCode;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00007484 File Offset: 0x00006484
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			IDataObject toolboxData = this.ToolboxData;
			string[] formats = toolboxData.GetFormats();
			object[] array = new object[formats.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = toolboxData.GetData(formats[i]);
			}
			info.AddValue("TbxIC_DataObjectFormats", formats);
			info.AddValue("TbxIC_DataObjectValues", array);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000074DC File Offset: 0x000064DC
		public virtual ToolboxItem GetToolboxItem(ICollection creators)
		{
			ToolboxItem toolboxItem = this._toolboxItem;
			if (this._toolboxItem == null)
			{
				if (this._dataObject.GetDataPresent("CF_TOOLBOXITEMCONTAINER_CONTENTS"))
				{
					string text = null;
					try
					{
						ToolboxItemContainer.ToolboxItemSerializer toolboxItemSerializer = (ToolboxItemContainer.ToolboxItemSerializer)this._dataObject.GetData("CF_TOOLBOXITEMCONTAINER_CONTENTS");
						this._toolboxItem = toolboxItemSerializer.ToolboxItem;
					}
					catch (Exception ex)
					{
						text = ex.Message;
					}
					catch
					{
					}
					if (this._toolboxItem == null)
					{
						this._toolboxItem = new ToolboxItemContainer.BrokenToolboxItem(text);
					}
					toolboxItem = this._toolboxItem;
				}
				else if (creators != null)
				{
					foreach (object obj in creators)
					{
						ToolboxItemCreator toolboxItemCreator = (ToolboxItemCreator)obj;
						if (this._dataObject.GetDataPresent(toolboxItemCreator.Format))
						{
							toolboxItem = toolboxItemCreator.Create(this._dataObject);
							if (toolboxItem != null)
							{
								break;
							}
						}
					}
				}
			}
			return toolboxItem;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000075E8 File Offset: 0x000065E8
		private static ICollection MergeFilter(ToolboxItem item)
		{
			ICollection filter = item.Filter;
			ArrayList arrayList = new ArrayList();
			foreach (object obj in TypeDescriptor.GetAttributes(item))
			{
				Attribute attribute = (Attribute)obj;
				if (attribute is ToolboxItemFilterAttribute)
				{
					arrayList.Add(attribute);
				}
			}
			ICollection collection;
			if (filter == null || filter.Count == 0)
			{
				collection = arrayList;
			}
			else if (arrayList.Count > 0)
			{
				Hashtable hashtable = new Hashtable(arrayList.Count + filter.Count);
				foreach (object obj2 in arrayList)
				{
					Attribute attribute2 = (Attribute)obj2;
					hashtable[attribute2.TypeId] = attribute2;
				}
				foreach (object obj3 in filter)
				{
					Attribute attribute3 = (Attribute)obj3;
					hashtable[attribute3.TypeId] = attribute3;
				}
				ToolboxItemFilterAttribute[] array = new ToolboxItemFilterAttribute[hashtable.Values.Count];
				hashtable.Values.CopyTo(array, 0);
				collection = array;
			}
			else
			{
				collection = filter;
			}
			return collection;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00007764 File Offset: 0x00006764
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			this.GetObjectData(info, context);
		}

		// Token: 0x040000E3 RID: 227
		private const string _localClipboardFormat = "CF_TOOLBOXITEMCONTAINER";

		// Token: 0x040000E4 RID: 228
		private const string _itemClipboardFormat = "CF_TOOLBOXITEMCONTAINER_CONTENTS";

		// Token: 0x040000E5 RID: 229
		private const string _hashClipboardFormat = "CF_TOOLBOXITEMCONTAINER_HASH";

		// Token: 0x040000E6 RID: 230
		private const string _serializationFormats = "TbxIC_DataObjectFormats";

		// Token: 0x040000E7 RID: 231
		private const string _serializationValues = "TbxIC_DataObjectValues";

		// Token: 0x040000E8 RID: 232
		private const short _clipboardVersion = 1;

		// Token: 0x040000E9 RID: 233
		private int _hashCode;

		// Token: 0x040000EA RID: 234
		private ToolboxItem _toolboxItem;

		// Token: 0x040000EB RID: 235
		private IDataObject _dataObject;

		// Token: 0x040000EC RID: 236
		private ICollection _filter;

		// Token: 0x02000023 RID: 35
		private class BrokenToolboxItem : ToolboxItem
		{
			// Token: 0x06000110 RID: 272 RVA: 0x0000776E File Offset: 0x0000676E
			public BrokenToolboxItem(string exceptionString)
				: base(typeof(Component))
			{
				this._exceptionString = exceptionString;
				this.Lock();
			}

			// Token: 0x06000111 RID: 273 RVA: 0x00007790 File Offset: 0x00006790
			protected override IComponent[] CreateComponentsCore(IDesignerHost host)
			{
				if (this._exceptionString != null)
				{
					throw new InvalidOperationException(SR.GetString("ToolboxServiceBadToolboxItemWithException", new object[] { this._exceptionString }));
				}
				throw new InvalidOperationException(SR.GetString("ToolboxServiceBadToolboxItem"));
			}

			// Token: 0x040000ED RID: 237
			private string _exceptionString;
		}

		// Token: 0x02000024 RID: 36
		[Serializable]
		private sealed class ToolboxItemSerializer : ISerializable
		{
			// Token: 0x06000112 RID: 274 RVA: 0x000077D5 File Offset: 0x000067D5
			internal ToolboxItemSerializer(ToolboxItem toolboxItem)
			{
				this._toolboxItem = toolboxItem;
			}

			// Token: 0x06000113 RID: 275 RVA: 0x000077E4 File Offset: 0x000067E4
			private ToolboxItemSerializer(SerializationInfo info, StreamingContext context)
			{
				AssemblyName assemblyName = (AssemblyName)info.GetValue("AssemblyName", typeof(AssemblyName));
				byte[] array = (byte[])info.GetValue("Stream", typeof(byte[]));
				if (ToolboxItemContainer.ToolboxItemSerializer._formatter == null)
				{
					ToolboxItemContainer.ToolboxItemSerializer._formatter = new BinaryFormatter();
				}
				SerializationBinder binder = ToolboxItemContainer.ToolboxItemSerializer._formatter.Binder;
				ToolboxItemContainer.ToolboxItemSerializer._formatter.Binder = new ToolboxItemContainer.ToolboxSerializationBinder(assemblyName);
				try
				{
					this._toolboxItem = (ToolboxItem)ToolboxItemContainer.ToolboxItemSerializer._formatter.Deserialize(new MemoryStream(array));
				}
				finally
				{
					ToolboxItemContainer.ToolboxItemSerializer._formatter.Binder = binder;
				}
			}

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x06000114 RID: 276 RVA: 0x00007894 File Offset: 0x00006894
			internal ToolboxItem ToolboxItem
			{
				get
				{
					return this._toolboxItem;
				}
			}

			// Token: 0x06000115 RID: 277 RVA: 0x0000789C File Offset: 0x0000689C
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (ToolboxItemContainer.ToolboxItemSerializer._formatter == null)
				{
					ToolboxItemContainer.ToolboxItemSerializer._formatter = new BinaryFormatter();
				}
				MemoryStream memoryStream = new MemoryStream();
				ToolboxItemContainer.ToolboxItemSerializer._formatter.Serialize(memoryStream, this._toolboxItem);
				memoryStream.Close();
				info.AddValue("AssemblyName", this._toolboxItem.GetType().Assembly.GetName());
				info.AddValue("Stream", memoryStream.GetBuffer());
			}

			// Token: 0x040000EE RID: 238
			private const string _assemblyNameKey = "AssemblyName";

			// Token: 0x040000EF RID: 239
			private const string _streamKey = "Stream";

			// Token: 0x040000F0 RID: 240
			private static BinaryFormatter _formatter;

			// Token: 0x040000F1 RID: 241
			private ToolboxItem _toolboxItem;
		}

		// Token: 0x02000025 RID: 37
		private class ToolboxSerializationBinder : SerializationBinder
		{
			// Token: 0x06000116 RID: 278 RVA: 0x00007908 File Offset: 0x00006908
			public ToolboxSerializationBinder(AssemblyName name)
			{
				this._assemblies = new Hashtable();
				this._name = name;
				this._namePart = name.Name + ",";
			}

			// Token: 0x06000117 RID: 279 RVA: 0x00007938 File Offset: 0x00006938
			public override Type BindToType(string assemblyName, string typeName)
			{
				Assembly assembly = (Assembly)this._assemblies[assemblyName];
				if (assembly == null)
				{
					try
					{
						assembly = Assembly.Load(assemblyName);
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
					if (assembly == null)
					{
						AssemblyName assemblyName2;
						if (assemblyName.StartsWith(this._namePart))
						{
							assemblyName2 = this._name;
							try
							{
								assembly = Assembly.Load(assemblyName2);
								goto IL_0058;
							}
							catch (FileNotFoundException)
							{
								goto IL_0058;
							}
							catch (BadImageFormatException)
							{
								goto IL_0058;
							}
							catch (IOException)
							{
								goto IL_0058;
							}
						}
						assemblyName2 = new AssemblyName(assemblyName);
						IL_0058:
						if (assembly == null)
						{
							string codeBase = assemblyName2.CodeBase;
							if (codeBase != null && codeBase.Length > 0 && File.Exists(codeBase))
							{
								assembly = Assembly.LoadFrom(codeBase);
							}
						}
					}
					if (assembly != null)
					{
						this._assemblies[assemblyName] = assembly;
					}
				}
				if (assembly != null)
				{
					return assembly.GetType(typeName);
				}
				return null;
			}

			// Token: 0x040000F2 RID: 242
			private Hashtable _assemblies;

			// Token: 0x040000F3 RID: 243
			private AssemblyName _name;

			// Token: 0x040000F4 RID: 244
			private string _namePart;
		}
	}
}
