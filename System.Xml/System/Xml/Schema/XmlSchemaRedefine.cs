﻿using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	public class XmlSchemaRedefine : XmlSchemaExternal
	{
		public XmlSchemaRedefine()
		{
			base.Compositor = Compositor.Redefine;
		}

		[XmlElement("complexType", typeof(XmlSchemaComplexType))]
		[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
		[XmlElement("group", typeof(XmlSchemaGroup))]
		[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
		[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroup))]
		public XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		[XmlIgnore]
		public XmlSchemaObjectTable AttributeGroups
		{
			get
			{
				return this.attributeGroups;
			}
		}

		[XmlIgnore]
		public XmlSchemaObjectTable SchemaTypes
		{
			get
			{
				return this.types;
			}
		}

		[XmlIgnore]
		public XmlSchemaObjectTable Groups
		{
			get
			{
				return this.groups;
			}
		}

		internal override void AddAnnotation(XmlSchemaAnnotation annotation)
		{
			this.items.Add(annotation);
		}

		private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();

		private XmlSchemaObjectTable attributeGroups = new XmlSchemaObjectTable();

		private XmlSchemaObjectTable types = new XmlSchemaObjectTable();

		private XmlSchemaObjectTable groups = new XmlSchemaObjectTable();
	}
}
