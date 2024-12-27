using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Xml.Serialization
{
	// Token: 0x020002B8 RID: 696
	public class ImportContext
	{
		// Token: 0x0600214A RID: 8522 RVA: 0x0009E01F File Offset: 0x0009D01F
		public ImportContext(CodeIdentifiers identifiers, bool shareTypes)
		{
			this.typeIdentifiers = identifiers;
			this.shareTypes = shareTypes;
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x0009E035 File Offset: 0x0009D035
		internal ImportContext()
			: this(null, false)
		{
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x0600214C RID: 8524 RVA: 0x0009E03F File Offset: 0x0009D03F
		internal SchemaObjectCache Cache
		{
			get
			{
				if (this.cache == null)
				{
					this.cache = new SchemaObjectCache();
				}
				return this.cache;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600214D RID: 8525 RVA: 0x0009E05A File Offset: 0x0009D05A
		internal Hashtable Elements
		{
			get
			{
				if (this.elements == null)
				{
					this.elements = new Hashtable();
				}
				return this.elements;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x0600214E RID: 8526 RVA: 0x0009E075 File Offset: 0x0009D075
		internal Hashtable Mappings
		{
			get
			{
				if (this.mappings == null)
				{
					this.mappings = new Hashtable();
				}
				return this.mappings;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600214F RID: 8527 RVA: 0x0009E090 File Offset: 0x0009D090
		public CodeIdentifiers TypeIdentifiers
		{
			get
			{
				if (this.typeIdentifiers == null)
				{
					this.typeIdentifiers = new CodeIdentifiers();
				}
				return this.typeIdentifiers;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002150 RID: 8528 RVA: 0x0009E0AB File Offset: 0x0009D0AB
		public bool ShareTypes
		{
			get
			{
				return this.shareTypes;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002151 RID: 8529 RVA: 0x0009E0B3 File Offset: 0x0009D0B3
		public StringCollection Warnings
		{
			get
			{
				return this.Cache.Warnings;
			}
		}

		// Token: 0x0400144A RID: 5194
		private bool shareTypes;

		// Token: 0x0400144B RID: 5195
		private SchemaObjectCache cache;

		// Token: 0x0400144C RID: 5196
		private Hashtable mappings;

		// Token: 0x0400144D RID: 5197
		private Hashtable elements;

		// Token: 0x0400144E RID: 5198
		private CodeIdentifiers typeIdentifiers;
	}
}
