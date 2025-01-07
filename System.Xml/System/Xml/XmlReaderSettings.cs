using System;
using System.Xml.Schema;
using System.Xml.XmlConfiguration;

namespace System.Xml
{
	public sealed class XmlReaderSettings
	{
		public XmlReaderSettings()
		{
			this.Reset();
		}

		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
			set
			{
				this.CheckReadOnly("NameTable");
				this.nameTable = value;
			}
		}

		internal bool IsXmlResolverSet
		{
			get
			{
				return this.isXmlResolverSet;
			}
			private set
			{
				this.isXmlResolverSet = value;
			}
		}

		public XmlResolver XmlResolver
		{
			set
			{
				this.CheckReadOnly("XmlResolver");
				this.xmlResolver = value;
				this.IsXmlResolverSet = true;
			}
		}

		internal XmlResolver GetXmlResolver()
		{
			return this.xmlResolver;
		}

		internal XmlResolver GetXmlResolver_CheckConfig()
		{
			if (XmlReaderSection.ProhibitDefaultUrlResolver && !this.IsXmlResolverSet)
			{
				return null;
			}
			return this.xmlResolver;
		}

		public int LineNumberOffset
		{
			get
			{
				return this.lineNumberOffset;
			}
			set
			{
				this.CheckReadOnly("LineNumberOffset");
				if (this.lineNumberOffset < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.lineNumberOffset = value;
			}
		}

		public int LinePositionOffset
		{
			get
			{
				return this.linePositionOffset;
			}
			set
			{
				this.CheckReadOnly("LinePositionOffset");
				if (this.linePositionOffset < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.linePositionOffset = value;
			}
		}

		public ConformanceLevel ConformanceLevel
		{
			get
			{
				return this.conformanceLevel;
			}
			set
			{
				this.CheckReadOnly("ConformanceLevel");
				if (value > ConformanceLevel.Document)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.conformanceLevel = value;
			}
		}

		public bool CheckCharacters
		{
			get
			{
				return this.checkCharacters;
			}
			set
			{
				this.CheckReadOnly("CheckCharacters");
				this.checkCharacters = value;
			}
		}

		public long MaxCharactersInDocument
		{
			get
			{
				return this.maxCharactersInDocument;
			}
			set
			{
				this.CheckReadOnly("MaxCharactersInDocument");
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.maxCharactersInDocument = value;
			}
		}

		public long MaxCharactersFromEntities
		{
			get
			{
				return this.maxCharactersFromEntities;
			}
			set
			{
				this.CheckReadOnly("MaxCharactersFromEntities");
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.maxCharactersFromEntities = value;
			}
		}

		public ValidationType ValidationType
		{
			get
			{
				return this.validationType;
			}
			set
			{
				this.CheckReadOnly("ValidationType");
				if (value > ValidationType.Schema)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.validationType = value;
			}
		}

		public XmlSchemaValidationFlags ValidationFlags
		{
			get
			{
				return this.validationFlags;
			}
			set
			{
				this.CheckReadOnly("ValidationFlags");
				if (value > (XmlSchemaValidationFlags.ProcessInlineSchema | XmlSchemaValidationFlags.ProcessSchemaLocation | XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints | XmlSchemaValidationFlags.AllowXmlAttributes))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.validationFlags = value;
			}
		}

		public XmlSchemaSet Schemas
		{
			get
			{
				if (this.schemas == null)
				{
					this.schemas = new XmlSchemaSet();
				}
				return this.schemas;
			}
			set
			{
				this.CheckReadOnly("Schemas");
				this.schemas = value;
			}
		}

		public event ValidationEventHandler ValidationEventHandler
		{
			add
			{
				this.CheckReadOnly("ValidationEventHandler");
				this.valEventHandler = (ValidationEventHandler)Delegate.Combine(this.valEventHandler, value);
			}
			remove
			{
				this.CheckReadOnly("ValidationEventHandler");
				this.valEventHandler = (ValidationEventHandler)Delegate.Remove(this.valEventHandler, value);
			}
		}

		public bool IgnoreWhitespace
		{
			get
			{
				return this.ignoreWhitespace;
			}
			set
			{
				this.CheckReadOnly("IgnoreWhitespace");
				this.ignoreWhitespace = value;
			}
		}

		public bool IgnoreProcessingInstructions
		{
			get
			{
				return this.ignorePIs;
			}
			set
			{
				this.CheckReadOnly("IgnoreProcessingInstructions");
				this.ignorePIs = value;
			}
		}

		public bool IgnoreComments
		{
			get
			{
				return this.ignoreComments;
			}
			set
			{
				this.CheckReadOnly("IgnoreComments");
				this.ignoreComments = value;
			}
		}

		public bool ProhibitDtd
		{
			get
			{
				return this.prohibitDtd;
			}
			set
			{
				this.CheckReadOnly("ProhibitDtd");
				this.prohibitDtd = value;
			}
		}

		public bool CloseInput
		{
			get
			{
				return this.closeInput;
			}
			set
			{
				this.CheckReadOnly("CloseInput");
				this.closeInput = value;
			}
		}

		public void Reset()
		{
			this.CheckReadOnly("Reset");
			this.nameTable = null;
			this.xmlResolver = XmlReaderSettings.CreateDefaultResolver();
			this.lineNumberOffset = 0;
			this.linePositionOffset = 0;
			this.checkCharacters = true;
			this.conformanceLevel = ConformanceLevel.Document;
			this.schemas = null;
			this.validationType = ValidationType.None;
			this.validationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints;
			this.validationFlags |= XmlSchemaValidationFlags.AllowXmlAttributes;
			this.ignoreWhitespace = false;
			this.ignorePIs = false;
			this.ignoreComments = false;
			this.prohibitDtd = true;
			this.closeInput = false;
			this.maxCharactersFromEntities = 0L;
			this.maxCharactersInDocument = 0L;
			this.isReadOnly = false;
			this.IsXmlResolverSet = false;
		}

		private static XmlResolver CreateDefaultResolver()
		{
			return new XmlUrlResolver();
		}

		public XmlReaderSettings Clone()
		{
			XmlReaderSettings xmlReaderSettings = base.MemberwiseClone() as XmlReaderSettings;
			xmlReaderSettings.isReadOnly = false;
			return xmlReaderSettings;
		}

		internal bool ReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
			set
			{
				this.isReadOnly = value;
			}
		}

		internal ValidationEventHandler GetEventHandler()
		{
			return this.valEventHandler;
		}

		private void CheckReadOnly(string propertyName)
		{
			if (this.isReadOnly)
			{
				throw new XmlException("Xml_ReadOnlyProperty", "XmlReaderSettings." + propertyName);
			}
		}

		internal bool CanResolveExternals
		{
			get
			{
				return !this.prohibitDtd && this.xmlResolver != null;
			}
		}

		private XmlNameTable nameTable;

		private XmlResolver xmlResolver;

		private int lineNumberOffset;

		private int linePositionOffset;

		private ConformanceLevel conformanceLevel;

		private bool checkCharacters;

		private long maxCharactersInDocument;

		private long maxCharactersFromEntities;

		private ValidationType validationType;

		private XmlSchemaValidationFlags validationFlags;

		private XmlSchemaSet schemas;

		private ValidationEventHandler valEventHandler;

		private bool ignoreWhitespace;

		private bool ignorePIs;

		private bool ignoreComments;

		private bool prohibitDtd;

		private bool closeInput;

		private bool isReadOnly;

		private bool isXmlResolverSet;
	}
}
