using System;
using System.Collections;

namespace System.Xml.Schema
{
	internal class ContentValidator
	{
		public ContentValidator(XmlSchemaContentType contentType)
		{
			this.contentType = contentType;
			this.isEmptiable = true;
		}

		protected ContentValidator(XmlSchemaContentType contentType, bool isOpen, bool isEmptiable)
		{
			this.contentType = contentType;
			this.isOpen = isOpen;
			this.isEmptiable = isEmptiable;
		}

		public XmlSchemaContentType ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		public bool PreserveWhitespace
		{
			get
			{
				return this.contentType == XmlSchemaContentType.TextOnly || this.contentType == XmlSchemaContentType.Mixed;
			}
		}

		public virtual bool IsEmptiable
		{
			get
			{
				return this.isEmptiable;
			}
		}

		public bool IsOpen
		{
			get
			{
				return this.contentType != XmlSchemaContentType.TextOnly && this.contentType != XmlSchemaContentType.Empty && this.isOpen;
			}
			set
			{
				this.isOpen = value;
			}
		}

		public virtual void InitValidation(ValidationState context)
		{
		}

		public virtual object ValidateElement(XmlQualifiedName name, ValidationState context, out int errorCode)
		{
			if (this.contentType == XmlSchemaContentType.TextOnly || this.contentType == XmlSchemaContentType.Empty)
			{
				context.NeedValidateChildren = false;
			}
			errorCode = -1;
			return null;
		}

		public virtual bool CompleteValidation(ValidationState context)
		{
			return true;
		}

		public virtual ArrayList ExpectedElements(ValidationState context, bool isRequiredOnly)
		{
			return null;
		}

		public virtual ArrayList ExpectedParticles(ValidationState context, bool isRequiredOnly)
		{
			return null;
		}

		private XmlSchemaContentType contentType;

		private bool isOpen;

		private bool isEmptiable;

		public static readonly ContentValidator Empty = new ContentValidator(XmlSchemaContentType.Empty);

		public static readonly ContentValidator TextOnly = new ContentValidator(XmlSchemaContentType.TextOnly, false, false);

		public static readonly ContentValidator Mixed = new ContentValidator(XmlSchemaContentType.Mixed);

		public static readonly ContentValidator Any = new ContentValidator(XmlSchemaContentType.Mixed, true, true);
	}
}
