﻿using System;

namespace System.Xml
{
	public class XmlEntity : XmlNode
	{
		internal XmlEntity(string name, string strdata, string publicId, string systemId, string notationName, XmlDocument doc)
			: base(doc)
		{
			this.name = doc.NameTable.Add(name);
			this.publicId = publicId;
			this.systemId = systemId;
			this.notationName = notationName;
			this.unparsedReplacementStr = strdata;
			this.childrenFoliating = false;
		}

		public override XmlNode CloneNode(bool deep)
		{
			throw new InvalidOperationException(Res.GetString("Xdom_Node_Cloning"));
		}

		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		public override string LocalName
		{
			get
			{
				return this.name;
			}
		}

		public override string InnerText
		{
			get
			{
				return base.InnerText;
			}
			set
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Ent_Innertext"));
			}
		}

		internal override bool IsContainer
		{
			get
			{
				return true;
			}
		}

		internal override XmlLinkedNode LastNode
		{
			get
			{
				if (this.lastChild == null && !this.childrenFoliating)
				{
					this.childrenFoliating = true;
					XmlLoader xmlLoader = new XmlLoader();
					xmlLoader.ExpandEntity(this);
				}
				return this.lastChild;
			}
			set
			{
				this.lastChild = value;
			}
		}

		internal override bool IsValidChildType(XmlNodeType type)
		{
			return type == XmlNodeType.Text || type == XmlNodeType.Element || type == XmlNodeType.ProcessingInstruction || type == XmlNodeType.Comment || type == XmlNodeType.CDATA || type == XmlNodeType.Whitespace || type == XmlNodeType.SignificantWhitespace || type == XmlNodeType.EntityReference;
		}

		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Entity;
			}
		}

		public string PublicId
		{
			get
			{
				return this.publicId;
			}
		}

		public string SystemId
		{
			get
			{
				return this.systemId;
			}
		}

		public string NotationName
		{
			get
			{
				return this.notationName;
			}
		}

		public override string OuterXml
		{
			get
			{
				return string.Empty;
			}
		}

		public override string InnerXml
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Set_InnerXml"));
			}
		}

		public override void WriteTo(XmlWriter w)
		{
		}

		public override void WriteContentTo(XmlWriter w)
		{
		}

		public override string BaseURI
		{
			get
			{
				return this.baseURI;
			}
		}

		internal void SetBaseURI(string inBaseURI)
		{
			this.baseURI = inBaseURI;
		}

		private string publicId;

		private string systemId;

		private string notationName;

		private string name;

		private string unparsedReplacementStr;

		private string baseURI;

		private XmlLinkedNode lastChild;

		private bool childrenFoliating;
	}
}
