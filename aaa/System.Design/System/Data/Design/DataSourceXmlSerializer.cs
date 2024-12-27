using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Xml;

namespace System.Data.Design
{
	// Token: 0x0200007F RID: 127
	internal class DataSourceXmlSerializer
	{
		// Token: 0x0600053C RID: 1340 RVA: 0x0000937B File Offset: 0x0000837B
		internal DataSourceXmlSerializer()
		{
			this.objectNeedBeInitialized = new Queue();
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0000939C File Offset: 0x0000839C
		private Hashtable NameToType
		{
			get
			{
				if (DataSourceXmlSerializer.nameToType == null)
				{
					DataSourceXmlSerializer.nameToType = new Hashtable();
					DataSourceXmlSerializer.nameToType.Add("DbSource", typeof(DbSource));
					DataSourceXmlSerializer.nameToType.Add("Connection", typeof(DesignConnection));
					DataSourceXmlSerializer.nameToType.Add("TableAdapter", typeof(DesignTable));
					DataSourceXmlSerializer.nameToType.Add("DbCommand", typeof(DbSourceCommand));
					DataSourceXmlSerializer.nameToType.Add("Parameter", typeof(DesignParameter));
				}
				return DataSourceXmlSerializer.nameToType;
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00009440 File Offset: 0x00008440
		private object CreateObject(string tagName)
		{
			if (tagName == "DbTable")
			{
				tagName = "TableAdapter";
			}
			if (!this.NameToType.Contains(tagName))
			{
				throw new DataSourceSerializationException(SR.GetString("DTDS_CouldNotDeserializeXmlElement", new object[] { tagName }));
			}
			Type type = (Type)this.NameToType[tagName];
			return Activator.CreateInstance(type);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x000094A4 File Offset: 0x000084A4
		internal object Deserialize(XmlElement xmlElement)
		{
			object obj = this.CreateObject(xmlElement.LocalName);
			if (obj is IDataSourceXmlSerializable)
			{
				((IDataSourceXmlSerializable)obj).ReadXml(xmlElement, this);
			}
			else
			{
				this.DeserializeBody(xmlElement, obj);
			}
			if (obj is IDataSourceInitAfterLoading)
			{
				this.objectNeedBeInitialized.Enqueue(obj);
			}
			return obj;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x000094F4 File Offset: 0x000084F4
		internal void DeserializeBody(XmlElement xmlElement, object obj)
		{
			DataSourceXmlSerializer.PropertySerializationInfo serializationInfo = this.GetSerializationInfo(obj.GetType());
			IDataSourceXmlSpecialOwner dataSourceXmlSpecialOwner = obj as IDataSourceXmlSpecialOwner;
			foreach (DataSourceXmlSerializer.XmlSerializableProperty xmlSerializableProperty in serializationInfo.AttributeProperties)
			{
				DataSourceXmlAttributeAttribute dataSourceXmlAttributeAttribute = xmlSerializableProperty.SerializationAttribute as DataSourceXmlAttributeAttribute;
				if (dataSourceXmlAttributeAttribute != null)
				{
					XmlAttribute xmlAttribute = xmlElement.Attributes[xmlSerializableProperty.Name];
					if (xmlAttribute != null)
					{
						PropertyDescriptor propertyDescriptor = xmlSerializableProperty.PropertyDescriptor;
						if (dataSourceXmlAttributeAttribute.SpecialWay)
						{
							dataSourceXmlSpecialOwner.ReadSpecialItem(propertyDescriptor.Name, xmlAttribute, this);
						}
						else
						{
							Type propertyType = xmlSerializableProperty.PropertyType;
							object obj2;
							if (propertyType == typeof(string))
							{
								obj2 = xmlAttribute.InnerText;
							}
							else
							{
								obj2 = TypeDescriptor.GetConverter(propertyType).ConvertFromString(xmlAttribute.InnerText);
							}
							if (obj2 != null)
							{
								propertyDescriptor.SetValue(obj, obj2);
							}
						}
					}
				}
			}
			foreach (object obj3 in xmlElement.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj3;
				XmlElement xmlElement2 = xmlNode as XmlElement;
				if (xmlElement2 != null)
				{
					DataSourceXmlSerializer.XmlSerializableProperty serializablePropertyWithElementName = serializationInfo.GetSerializablePropertyWithElementName(xmlElement2.LocalName);
					if (serializablePropertyWithElementName != null)
					{
						PropertyDescriptor propertyDescriptor2 = serializablePropertyWithElementName.PropertyDescriptor;
						DataSourceXmlSerializationAttribute serializationAttribute = serializablePropertyWithElementName.SerializationAttribute;
						if (serializationAttribute is DataSourceXmlElementAttribute)
						{
							DataSourceXmlElementAttribute dataSourceXmlElementAttribute = (DataSourceXmlElementAttribute)serializationAttribute;
							bool specialWay = serializationAttribute.SpecialWay;
							if (specialWay)
							{
								dataSourceXmlSpecialOwner.ReadSpecialItem(propertyDescriptor2.Name, xmlElement2, this);
								continue;
							}
							if (this.NameToType.Contains(xmlElement2.LocalName))
							{
								object obj4 = this.Deserialize(xmlElement2);
								propertyDescriptor2.SetValue(obj, obj4);
								continue;
							}
							Type propertyType2 = serializablePropertyWithElementName.PropertyType;
							try
							{
								object obj2;
								if (propertyType2 == typeof(string))
								{
									obj2 = xmlElement2.InnerText;
								}
								else
								{
									obj2 = TypeDescriptor.GetConverter(propertyType2).ConvertFromString(xmlElement2.InnerText);
								}
								propertyDescriptor2.SetValue(obj, obj2);
								continue;
							}
							catch (Exception)
							{
								continue;
							}
						}
						DataSourceXmlSubItemAttribute dataSourceXmlSubItemAttribute = (DataSourceXmlSubItemAttribute)serializationAttribute;
						if (typeof(IList).IsAssignableFrom(propertyDescriptor2.PropertyType))
						{
							IList list = propertyDescriptor2.GetValue(obj) as IList;
							using (IEnumerator enumerator2 = xmlElement2.ChildNodes.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									object obj5 = enumerator2.Current;
									XmlNode xmlNode2 = (XmlNode)obj5;
									XmlElement xmlElement3 = xmlNode2 as XmlElement;
									if (xmlElement3 != null)
									{
										object obj6 = this.Deserialize(xmlElement3);
										list.Add(obj6);
									}
								}
								continue;
							}
						}
						for (XmlNode xmlNode3 = xmlElement2.FirstChild; xmlNode3 != null; xmlNode3 = xmlNode3.NextSibling)
						{
							if (xmlNode3 is XmlElement)
							{
								object obj7 = this.Deserialize((XmlElement)xmlNode3);
								propertyDescriptor2.SetValue(obj, obj7);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00009804 File Offset: 0x00008804
		private DataSourceXmlSerializer.PropertySerializationInfo GetSerializationInfo(Type type)
		{
			if (DataSourceXmlSerializer.propertySerializationInfoHash == null)
			{
				DataSourceXmlSerializer.propertySerializationInfoHash = new Hashtable();
			}
			if (DataSourceXmlSerializer.propertySerializationInfoHash.Contains(type))
			{
				return (DataSourceXmlSerializer.PropertySerializationInfo)DataSourceXmlSerializer.propertySerializationInfoHash[type];
			}
			DataSourceXmlSerializer.PropertySerializationInfo propertySerializationInfo = new DataSourceXmlSerializer.PropertySerializationInfo(type);
			DataSourceXmlSerializer.propertySerializationInfoHash.Add(type, propertySerializationInfo);
			return propertySerializationInfo;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00009854 File Offset: 0x00008854
		internal void InitializeObjects()
		{
			int count = this.objectNeedBeInitialized.Count;
			while (count-- > 0)
			{
				IDataSourceInitAfterLoading dataSourceInitAfterLoading = (IDataSourceInitAfterLoading)this.objectNeedBeInitialized.Dequeue();
				dataSourceInitAfterLoading.InitializeAfterLoading();
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00009890 File Offset: 0x00008890
		internal void Serialize(XmlWriter xmlWriter, object obj)
		{
			if (obj is IDataSourceXmlSerializable)
			{
				((IDataSourceXmlSerializable)obj).WriteXml(xmlWriter, this);
				return;
			}
			Type type = obj.GetType();
			string text = null;
			AttributeCollection attributes = TypeDescriptor.GetAttributes(type);
			DataSourceXmlClassAttribute dataSourceXmlClassAttribute = attributes[typeof(DataSourceXmlClassAttribute)] as DataSourceXmlClassAttribute;
			if (dataSourceXmlClassAttribute != null)
			{
				text = dataSourceXmlClassAttribute.Name;
			}
			if (text == null)
			{
				text = type.Name;
			}
			xmlWriter.WriteStartElement(string.Empty, text, this.nameSpace);
			this.SerializeBody(xmlWriter, obj);
			xmlWriter.WriteFullEndElement();
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00009910 File Offset: 0x00008910
		internal void SerializeBody(XmlWriter xmlWriter, object obj)
		{
			PropertyDescriptorCollection propertyDescriptorCollection;
			if (obj is ICustomTypeDescriptor)
			{
				propertyDescriptorCollection = ((ICustomTypeDescriptor)obj).GetProperties();
			}
			else
			{
				propertyDescriptorCollection = TypeDescriptor.GetProperties(obj);
			}
			propertyDescriptorCollection = propertyDescriptorCollection.Sort();
			ArrayList arrayList = new ArrayList();
			IDataSourceXmlSpecialOwner dataSourceXmlSpecialOwner = obj as IDataSourceXmlSpecialOwner;
			foreach (object obj2 in propertyDescriptorCollection)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj2;
				DataSourceXmlSerializationAttribute dataSourceXmlSerializationAttribute = (DataSourceXmlSerializationAttribute)propertyDescriptor.Attributes[typeof(DataSourceXmlSerializationAttribute)];
				if (dataSourceXmlSerializationAttribute != null)
				{
					if (dataSourceXmlSerializationAttribute is DataSourceXmlAttributeAttribute)
					{
						DataSourceXmlAttributeAttribute dataSourceXmlAttributeAttribute = (DataSourceXmlAttributeAttribute)dataSourceXmlSerializationAttribute;
						object value = propertyDescriptor.GetValue(obj);
						if (value != null)
						{
							string text = dataSourceXmlAttributeAttribute.Name;
							if (text == null)
							{
								text = propertyDescriptor.Name;
							}
							if (dataSourceXmlAttributeAttribute.SpecialWay)
							{
								xmlWriter.WriteStartAttribute(string.Empty, text, string.Empty);
								dataSourceXmlSpecialOwner.WriteSpecialItem(propertyDescriptor.Name, xmlWriter, this);
								xmlWriter.WriteEndAttribute();
							}
							else
							{
								xmlWriter.WriteAttributeString(text, value.ToString());
							}
						}
					}
					else
					{
						arrayList.Add(propertyDescriptor);
					}
				}
			}
			foreach (object obj3 in arrayList)
			{
				PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)obj3;
				object value2 = propertyDescriptor2.GetValue(obj);
				if (value2 != null)
				{
					DataSourceXmlSerializationAttribute dataSourceXmlSerializationAttribute2 = (DataSourceXmlSerializationAttribute)propertyDescriptor2.Attributes[typeof(DataSourceXmlSerializationAttribute)];
					string text2 = dataSourceXmlSerializationAttribute2.Name;
					if (text2 == null)
					{
						text2 = propertyDescriptor2.Name;
					}
					if (!(dataSourceXmlSerializationAttribute2 is DataSourceXmlElementAttribute))
					{
						DataSourceXmlSubItemAttribute dataSourceXmlSubItemAttribute = (DataSourceXmlSubItemAttribute)dataSourceXmlSerializationAttribute2;
						xmlWriter.WriteStartElement(string.Empty, text2, this.nameSpace);
						if (value2 is ICollection)
						{
							using (IEnumerator enumerator3 = ((ICollection)value2).GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									object obj4 = enumerator3.Current;
									this.Serialize(xmlWriter, obj4);
								}
								goto IL_0253;
							}
							goto IL_024A;
						}
						goto IL_024A;
						IL_0253:
						xmlWriter.WriteFullEndElement();
						continue;
						IL_024A:
						this.Serialize(xmlWriter, value2);
						goto IL_0253;
					}
					DataSourceXmlElementAttribute dataSourceXmlElementAttribute = (DataSourceXmlElementAttribute)dataSourceXmlSerializationAttribute2;
					bool specialWay = dataSourceXmlSerializationAttribute2.SpecialWay;
					if (specialWay)
					{
						xmlWriter.WriteStartElement(string.Empty, text2, this.nameSpace);
						dataSourceXmlSpecialOwner.WriteSpecialItem(propertyDescriptor2.Name, xmlWriter, this);
						xmlWriter.WriteFullEndElement();
					}
					else if (this.NameToType.Contains(text2))
					{
						this.Serialize(xmlWriter, value2);
					}
					else
					{
						xmlWriter.WriteElementString(text2, value2.ToString());
					}
				}
			}
		}

		// Token: 0x04000ACB RID: 2763
		private static Hashtable nameToType;

		// Token: 0x04000ACC RID: 2764
		private static Hashtable propertySerializationInfoHash;

		// Token: 0x04000ACD RID: 2765
		private string nameSpace = "urn:schemas-microsoft-com:xml-msdatasource";

		// Token: 0x04000ACE RID: 2766
		private Queue objectNeedBeInitialized;

		// Token: 0x02000080 RID: 128
		private class PropertySerializationInfo
		{
			// Token: 0x06000545 RID: 1349 RVA: 0x00009BE8 File Offset: 0x00008BE8
			internal PropertySerializationInfo(Type type)
			{
				ArrayList arrayList = new ArrayList();
				this.elementProperties = new Hashtable();
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
				foreach (object obj in properties)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
					DataSourceXmlSerializationAttribute dataSourceXmlSerializationAttribute = (DataSourceXmlSerializationAttribute)propertyDescriptor.Attributes[typeof(DataSourceXmlSerializationAttribute)];
					if (dataSourceXmlSerializationAttribute != null)
					{
						DataSourceXmlSerializer.XmlSerializableProperty xmlSerializableProperty = new DataSourceXmlSerializer.XmlSerializableProperty(dataSourceXmlSerializationAttribute, propertyDescriptor);
						if (dataSourceXmlSerializationAttribute is DataSourceXmlAttributeAttribute)
						{
							arrayList.Add(xmlSerializableProperty);
						}
						else
						{
							this.elementProperties.Add(xmlSerializableProperty.Name, xmlSerializableProperty);
						}
					}
				}
				this.AttributeProperties = (DataSourceXmlSerializer.XmlSerializableProperty[])arrayList.ToArray(typeof(DataSourceXmlSerializer.XmlSerializableProperty));
			}

			// Token: 0x06000546 RID: 1350 RVA: 0x00009CC4 File Offset: 0x00008CC4
			internal DataSourceXmlSerializer.XmlSerializableProperty GetSerializablePropertyWithElementName(string name)
			{
				if (this.elementProperties.Contains(name))
				{
					return (DataSourceXmlSerializer.XmlSerializableProperty)this.elementProperties[name];
				}
				return null;
			}

			// Token: 0x04000ACF RID: 2767
			internal DataSourceXmlSerializer.XmlSerializableProperty[] AttributeProperties;

			// Token: 0x04000AD0 RID: 2768
			private Hashtable elementProperties;
		}

		// Token: 0x02000081 RID: 129
		private class XmlSerializableProperty
		{
			// Token: 0x06000547 RID: 1351 RVA: 0x00009CE8 File Offset: 0x00008CE8
			internal XmlSerializableProperty(DataSourceXmlSerializationAttribute serializationAttribute, PropertyDescriptor propertyDescriptor)
			{
				this.Name = serializationAttribute.Name;
				if (this.Name == null)
				{
					this.Name = propertyDescriptor.Name;
				}
				this.SerializationAttribute = serializationAttribute;
				this.PropertyDescriptor = propertyDescriptor;
				this.PropertyType = serializationAttribute.ItemType;
				if (this.PropertyType == null)
				{
					this.PropertyType = propertyDescriptor.PropertyType;
				}
			}

			// Token: 0x04000AD1 RID: 2769
			internal string Name;

			// Token: 0x04000AD2 RID: 2770
			internal DataSourceXmlSerializationAttribute SerializationAttribute;

			// Token: 0x04000AD3 RID: 2771
			internal Type PropertyType;

			// Token: 0x04000AD4 RID: 2772
			internal PropertyDescriptor PropertyDescriptor;
		}
	}
}
