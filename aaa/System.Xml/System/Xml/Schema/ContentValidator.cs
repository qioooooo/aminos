using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x0200019F RID: 415
	internal class ContentValidator
	{
		// Token: 0x06001578 RID: 5496 RVA: 0x0005F2AC File Offset: 0x0005E2AC
		public ContentValidator(XmlSchemaContentType contentType)
		{
			this.contentType = contentType;
			this.isEmptiable = true;
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0005F2C2 File Offset: 0x0005E2C2
		protected ContentValidator(XmlSchemaContentType contentType, bool isOpen, bool isEmptiable)
		{
			this.contentType = contentType;
			this.isOpen = isOpen;
			this.isEmptiable = isEmptiable;
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x0005F2DF File Offset: 0x0005E2DF
		public XmlSchemaContentType ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x0005F2E7 File Offset: 0x0005E2E7
		public bool PreserveWhitespace
		{
			get
			{
				return this.contentType == XmlSchemaContentType.TextOnly || this.contentType == XmlSchemaContentType.Mixed;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x0600157C RID: 5500 RVA: 0x0005F2FC File Offset: 0x0005E2FC
		public virtual bool IsEmptiable
		{
			get
			{
				return this.isEmptiable;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x0600157D RID: 5501 RVA: 0x0005F304 File Offset: 0x0005E304
		// (set) Token: 0x0600157E RID: 5502 RVA: 0x0005F31F File Offset: 0x0005E31F
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

		// Token: 0x0600157F RID: 5503 RVA: 0x0005F328 File Offset: 0x0005E328
		public virtual void InitValidation(ValidationState context)
		{
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0005F32A File Offset: 0x0005E32A
		public virtual object ValidateElement(XmlQualifiedName name, ValidationState context, out int errorCode)
		{
			if (this.contentType == XmlSchemaContentType.TextOnly || this.contentType == XmlSchemaContentType.Empty)
			{
				context.NeedValidateChildren = false;
			}
			errorCode = -1;
			return null;
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0005F348 File Offset: 0x0005E348
		public virtual bool CompleteValidation(ValidationState context)
		{
			return true;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0005F34B File Offset: 0x0005E34B
		public virtual ArrayList ExpectedElements(ValidationState context, bool isRequiredOnly)
		{
			return null;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0005F34E File Offset: 0x0005E34E
		public virtual ArrayList ExpectedParticles(ValidationState context, bool isRequiredOnly)
		{
			return null;
		}

		// Token: 0x04000CCC RID: 3276
		private XmlSchemaContentType contentType;

		// Token: 0x04000CCD RID: 3277
		private bool isOpen;

		// Token: 0x04000CCE RID: 3278
		private bool isEmptiable;

		// Token: 0x04000CCF RID: 3279
		public static readonly ContentValidator Empty = new ContentValidator(XmlSchemaContentType.Empty);

		// Token: 0x04000CD0 RID: 3280
		public static readonly ContentValidator TextOnly = new ContentValidator(XmlSchemaContentType.TextOnly, false, false);

		// Token: 0x04000CD1 RID: 3281
		public static readonly ContentValidator Mixed = new ContentValidator(XmlSchemaContentType.Mixed);

		// Token: 0x04000CD2 RID: 3282
		public static readonly ContentValidator Any = new ContentValidator(XmlSchemaContentType.Mixed, true, true);
	}
}
