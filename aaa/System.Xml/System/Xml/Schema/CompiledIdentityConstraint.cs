using System;

namespace System.Xml.Schema
{
	// Token: 0x02000187 RID: 391
	internal class CompiledIdentityConstraint
	{
		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x000587BA File Offset: 0x000577BA
		public CompiledIdentityConstraint.ConstraintRole Role
		{
			get
			{
				return this.role;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x000587C2 File Offset: 0x000577C2
		public Asttree Selector
		{
			get
			{
				return this.selector;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x000587CA File Offset: 0x000577CA
		public Asttree[] Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x000587D2 File Offset: 0x000577D2
		private CompiledIdentityConstraint()
		{
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x000587F0 File Offset: 0x000577F0
		public CompiledIdentityConstraint(XmlSchemaIdentityConstraint constraint, XmlNamespaceManager nsmgr)
		{
			this.name = constraint.QualifiedName;
			try
			{
				this.selector = new Asttree(constraint.Selector.XPath, false, nsmgr);
			}
			catch (XmlSchemaException ex)
			{
				ex.SetSource(constraint.Selector);
				throw ex;
			}
			XmlSchemaObjectCollection xmlSchemaObjectCollection = constraint.Fields;
			this.fields = new Asttree[xmlSchemaObjectCollection.Count];
			for (int i = 0; i < xmlSchemaObjectCollection.Count; i++)
			{
				try
				{
					this.fields[i] = new Asttree(((XmlSchemaXPath)xmlSchemaObjectCollection[i]).XPath, true, nsmgr);
				}
				catch (XmlSchemaException ex2)
				{
					ex2.SetSource(constraint.Fields[i]);
					throw ex2;
				}
			}
			if (constraint is XmlSchemaUnique)
			{
				this.role = CompiledIdentityConstraint.ConstraintRole.Unique;
				return;
			}
			if (constraint is XmlSchemaKey)
			{
				this.role = CompiledIdentityConstraint.ConstraintRole.Key;
				return;
			}
			this.role = CompiledIdentityConstraint.ConstraintRole.Keyref;
			this.refer = ((XmlSchemaKeyref)constraint).Refer;
		}

		// Token: 0x04000C86 RID: 3206
		internal XmlQualifiedName name = XmlQualifiedName.Empty;

		// Token: 0x04000C87 RID: 3207
		private CompiledIdentityConstraint.ConstraintRole role;

		// Token: 0x04000C88 RID: 3208
		private Asttree selector;

		// Token: 0x04000C89 RID: 3209
		private Asttree[] fields;

		// Token: 0x04000C8A RID: 3210
		internal XmlQualifiedName refer = XmlQualifiedName.Empty;

		// Token: 0x04000C8B RID: 3211
		public static readonly CompiledIdentityConstraint Empty = new CompiledIdentityConstraint();

		// Token: 0x02000188 RID: 392
		public enum ConstraintRole
		{
			// Token: 0x04000C8D RID: 3213
			Unique,
			// Token: 0x04000C8E RID: 3214
			Key,
			// Token: 0x04000C8F RID: 3215
			Keyref
		}
	}
}
