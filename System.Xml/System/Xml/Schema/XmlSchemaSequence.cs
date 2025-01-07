﻿using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	public class XmlSchemaSequence : XmlSchemaGroupBase
	{
		[XmlElement("group", typeof(XmlSchemaGroupRef))]
		[XmlElement("choice", typeof(XmlSchemaChoice))]
		[XmlElement("any", typeof(XmlSchemaAny))]
		[XmlElement("element", typeof(XmlSchemaElement))]
		[XmlElement("sequence", typeof(XmlSchemaSequence))]
		public override XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		internal override bool IsEmpty
		{
			get
			{
				return base.IsEmpty || this.items.Count == 0;
			}
		}

		internal override void SetItems(XmlSchemaObjectCollection newItems)
		{
			this.items = newItems;
		}

		private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();
	}
}
