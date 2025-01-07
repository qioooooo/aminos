using System;

namespace System.Xml.Schema
{
	internal sealed class SchemaEntity
	{
		internal SchemaEntity(XmlQualifiedName name, bool isParameter)
		{
			this.name = name;
			this.isParameter = isParameter;
		}

		internal static bool IsPredefinedEntity(string n)
		{
			return n == "lt" || n == "gt" || n == "amp" || n == "apos" || n == "quot";
		}

		internal XmlQualifiedName Name
		{
			get
			{
				return this.name;
			}
		}

		internal string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
				this.isExternal = true;
			}
		}

		internal string Pubid
		{
			get
			{
				return this.pubid;
			}
			set
			{
				this.pubid = value;
			}
		}

		internal bool IsProcessed
		{
			get
			{
				return this.isProcessed;
			}
			set
			{
				this.isProcessed = value;
			}
		}

		internal bool IsExternal
		{
			get
			{
				return this.isExternal;
			}
			set
			{
				this.isExternal = value;
			}
		}

		internal bool DeclaredInExternal
		{
			get
			{
				return this.isDeclaredInExternal;
			}
			set
			{
				this.isDeclaredInExternal = value;
			}
		}

		internal bool IsParEntity
		{
			get
			{
				return this.isParameter;
			}
			set
			{
				this.isParameter = value;
			}
		}

		internal XmlQualifiedName NData
		{
			get
			{
				return this.ndata;
			}
			set
			{
				this.ndata = value;
			}
		}

		internal string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
				this.isExternal = false;
			}
		}

		internal int Line
		{
			get
			{
				return this.lineNumber;
			}
			set
			{
				this.lineNumber = value;
			}
		}

		internal int Pos
		{
			get
			{
				return this.linePosition;
			}
			set
			{
				this.linePosition = value;
			}
		}

		internal string BaseURI
		{
			get
			{
				if (this.baseURI != null)
				{
					return this.baseURI;
				}
				return string.Empty;
			}
			set
			{
				this.baseURI = value;
			}
		}

		internal string DeclaredURI
		{
			get
			{
				if (this.declaredURI != null)
				{
					return this.declaredURI;
				}
				return string.Empty;
			}
			set
			{
				this.declaredURI = value;
			}
		}

		private XmlQualifiedName name;

		private string url;

		private string pubid;

		private string text;

		private XmlQualifiedName ndata = XmlQualifiedName.Empty;

		private int lineNumber;

		private int linePosition;

		private bool isParameter;

		private bool isExternal;

		private bool isProcessed;

		private bool isDeclaredInExternal;

		private string baseURI;

		private string declaredURI;
	}
}
