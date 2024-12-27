using System;

namespace System.Management
{
	// Token: 0x0200003C RID: 60
	public class RelatedObjectQuery : WqlObjectQuery
	{
		// Token: 0x06000203 RID: 515 RVA: 0x0000A6AE File Offset: 0x000096AE
		public RelatedObjectQuery()
			: this(null)
		{
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A6B8 File Offset: 0x000096B8
		public RelatedObjectQuery(string queryOrSourceObject)
		{
			if (queryOrSourceObject == null)
			{
				return;
			}
			if (queryOrSourceObject.TrimStart(new char[0]).StartsWith(RelatedObjectQuery.tokenAssociators, StringComparison.OrdinalIgnoreCase))
			{
				this.QueryString = queryOrSourceObject;
				return;
			}
			ManagementPath managementPath = new ManagementPath(queryOrSourceObject);
			if ((managementPath.IsClass || managementPath.IsInstance) && managementPath.NamespacePath.Length == 0)
			{
				this.SourceObject = queryOrSourceObject;
				this.isSchemaQuery = false;
				return;
			}
			throw new ArgumentException(RC.GetString("INVALID_QUERY"), "queryOrSourceObject");
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000A738 File Offset: 0x00009738
		public RelatedObjectQuery(string sourceObject, string relatedClass)
			: this(sourceObject, relatedClass, null, null, null, null, null, false)
		{
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000A754 File Offset: 0x00009754
		public RelatedObjectQuery(string sourceObject, string relatedClass, string relationshipClass, string relatedQualifier, string relationshipQualifier, string relatedRole, string thisRole, bool classDefinitionsOnly)
		{
			this.isSchemaQuery = false;
			this.sourceObject = sourceObject;
			this.relatedClass = relatedClass;
			this.relationshipClass = relationshipClass;
			this.relatedQualifier = relatedQualifier;
			this.relationshipQualifier = relationshipQualifier;
			this.relatedRole = relatedRole;
			this.thisRole = thisRole;
			this.classDefinitionsOnly = classDefinitionsOnly;
			this.BuildQuery();
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000A7B4 File Offset: 0x000097B4
		public RelatedObjectQuery(bool isSchemaQuery, string sourceObject, string relatedClass, string relationshipClass, string relatedQualifier, string relationshipQualifier, string relatedRole, string thisRole)
		{
			if (!isSchemaQuery)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "isSchemaQuery");
			}
			this.isSchemaQuery = true;
			this.sourceObject = sourceObject;
			this.relatedClass = relatedClass;
			this.relationshipClass = relationshipClass;
			this.relatedQualifier = relatedQualifier;
			this.relationshipQualifier = relationshipQualifier;
			this.relatedRole = relatedRole;
			this.thisRole = thisRole;
			this.classDefinitionsOnly = false;
			this.BuildQuery();
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000A829 File Offset: 0x00009829
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0000A831 File Offset: 0x00009831
		public bool IsSchemaQuery
		{
			get
			{
				return this.isSchemaQuery;
			}
			set
			{
				this.isSchemaQuery = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000A846 File Offset: 0x00009846
		// (set) Token: 0x0600020B RID: 523 RVA: 0x0000A85C File Offset: 0x0000985C
		public string SourceObject
		{
			get
			{
				if (this.sourceObject == null)
				{
					return string.Empty;
				}
				return this.sourceObject;
			}
			set
			{
				this.sourceObject = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000A871 File Offset: 0x00009871
		// (set) Token: 0x0600020D RID: 525 RVA: 0x0000A887 File Offset: 0x00009887
		public string RelatedClass
		{
			get
			{
				if (this.relatedClass == null)
				{
					return string.Empty;
				}
				return this.relatedClass;
			}
			set
			{
				this.relatedClass = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000A89C File Offset: 0x0000989C
		// (set) Token: 0x0600020F RID: 527 RVA: 0x0000A8B2 File Offset: 0x000098B2
		public string RelationshipClass
		{
			get
			{
				if (this.relationshipClass == null)
				{
					return string.Empty;
				}
				return this.relationshipClass;
			}
			set
			{
				this.relationshipClass = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000A8C7 File Offset: 0x000098C7
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000A8DD File Offset: 0x000098DD
		public string RelatedQualifier
		{
			get
			{
				if (this.relatedQualifier == null)
				{
					return string.Empty;
				}
				return this.relatedQualifier;
			}
			set
			{
				this.relatedQualifier = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000A8F2 File Offset: 0x000098F2
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000A908 File Offset: 0x00009908
		public string RelationshipQualifier
		{
			get
			{
				if (this.relationshipQualifier == null)
				{
					return string.Empty;
				}
				return this.relationshipQualifier;
			}
			set
			{
				this.relationshipQualifier = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000A91D File Offset: 0x0000991D
		// (set) Token: 0x06000215 RID: 533 RVA: 0x0000A933 File Offset: 0x00009933
		public string RelatedRole
		{
			get
			{
				if (this.relatedRole == null)
				{
					return string.Empty;
				}
				return this.relatedRole;
			}
			set
			{
				this.relatedRole = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000A948 File Offset: 0x00009948
		// (set) Token: 0x06000217 RID: 535 RVA: 0x0000A95E File Offset: 0x0000995E
		public string ThisRole
		{
			get
			{
				if (this.thisRole == null)
				{
					return string.Empty;
				}
				return this.thisRole;
			}
			set
			{
				this.thisRole = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000A973 File Offset: 0x00009973
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0000A97B File Offset: 0x0000997B
		public bool ClassDefinitionsOnly
		{
			get
			{
				return this.classDefinitionsOnly;
			}
			set
			{
				this.classDefinitionsOnly = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000A990 File Offset: 0x00009990
		protected internal void BuildQuery()
		{
			if (this.sourceObject == null)
			{
				base.SetQueryString(string.Empty);
			}
			if (this.sourceObject == null || this.sourceObject.Length == 0)
			{
				return;
			}
			string text = string.Concat(new string[]
			{
				RelatedObjectQuery.tokenAssociators,
				" ",
				RelatedObjectQuery.tokenOf,
				" {",
				this.sourceObject,
				"}"
			});
			if (this.RelatedClass.Length != 0 || this.RelationshipClass.Length != 0 || this.RelatedQualifier.Length != 0 || this.RelationshipQualifier.Length != 0 || this.RelatedRole.Length != 0 || this.ThisRole.Length != 0 || this.classDefinitionsOnly || this.isSchemaQuery)
			{
				text = text + " " + RelatedObjectQuery.tokenWhere;
				if (this.RelatedClass.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenResultClass,
						" = ",
						this.relatedClass
					});
				}
				if (this.RelationshipClass.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenAssocClass,
						" = ",
						this.relationshipClass
					});
				}
				if (this.RelatedRole.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenResultRole,
						" = ",
						this.relatedRole
					});
				}
				if (this.ThisRole.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenRole,
						" = ",
						this.thisRole
					});
				}
				if (this.RelatedQualifier.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenRequiredQualifier,
						" = ",
						this.relatedQualifier
					});
				}
				if (this.RelationshipQualifier.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelatedObjectQuery.tokenRequiredAssocQualifier,
						" = ",
						this.relationshipQualifier
					});
				}
				if (!this.isSchemaQuery)
				{
					if (this.classDefinitionsOnly)
					{
						text = text + " " + RelatedObjectQuery.tokenClassDefsOnly;
					}
				}
				else
				{
					text = text + " " + RelatedObjectQuery.tokenSchemaOnly;
				}
			}
			base.SetQueryString(text);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000AC50 File Offset: 0x00009C50
		protected internal override void ParseQuery(string query)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = null;
			string text6 = null;
			bool flag = false;
			bool flag2 = false;
			string text7 = query.Trim();
			if (string.Compare(text7, 0, RelatedObjectQuery.tokenAssociators, 0, RelatedObjectQuery.tokenAssociators.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "associators");
			}
			text7 = text7.Remove(0, RelatedObjectQuery.tokenAssociators.Length);
			if (text7.Length == 0 || !char.IsWhiteSpace(text7[0]))
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			text7 = text7.TrimStart(null);
			if (string.Compare(text7, 0, RelatedObjectQuery.tokenOf, 0, RelatedObjectQuery.tokenOf.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "of");
			}
			text7 = text7.Remove(0, RelatedObjectQuery.tokenOf.Length).TrimStart(null);
			if (text7.IndexOf('{') != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			text7 = text7.Remove(0, 1).TrimStart(null);
			int num;
			if (-1 == (num = text7.IndexOf('}')))
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			string text8 = text7.Substring(0, num).TrimEnd(null);
			text7 = text7.Remove(0, num + 1).TrimStart(null);
			if (0 < text7.Length)
			{
				if (string.Compare(text7, 0, RelatedObjectQuery.tokenWhere, 0, RelatedObjectQuery.tokenWhere.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "where");
				}
				text7 = text7.Remove(0, RelatedObjectQuery.tokenWhere.Length);
				if (text7.Length == 0 || !char.IsWhiteSpace(text7[0]))
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				text7 = text7.TrimStart(null);
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				bool flag8 = false;
				bool flag9 = false;
				bool flag10 = false;
				for (;;)
				{
					if (text7.Length >= RelatedObjectQuery.tokenResultClass.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenResultClass, 0, RelatedObjectQuery.tokenResultClass.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenResultClass, "=", ref flag3, ref text);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenAssocClass.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenAssocClass, 0, RelatedObjectQuery.tokenAssocClass.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenAssocClass, "=", ref flag4, ref text2);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenResultRole.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenResultRole, 0, RelatedObjectQuery.tokenResultRole.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenResultRole, "=", ref flag5, ref text3);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenRole.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenRole, 0, RelatedObjectQuery.tokenRole.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenRole, "=", ref flag6, ref text4);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenRequiredQualifier.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenRequiredQualifier, 0, RelatedObjectQuery.tokenRequiredQualifier.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenRequiredQualifier, "=", ref flag7, ref text5);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenRequiredAssocQualifier.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenRequiredAssocQualifier, 0, RelatedObjectQuery.tokenRequiredAssocQualifier.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenRequiredAssocQualifier, "=", ref flag8, ref text6);
					}
					else if (text7.Length >= RelatedObjectQuery.tokenSchemaOnly.Length && string.Compare(text7, 0, RelatedObjectQuery.tokenSchemaOnly, 0, RelatedObjectQuery.tokenSchemaOnly.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenSchemaOnly, ref flag10);
						flag2 = true;
					}
					else
					{
						if (text7.Length < RelatedObjectQuery.tokenClassDefsOnly.Length || string.Compare(text7, 0, RelatedObjectQuery.tokenClassDefsOnly, 0, RelatedObjectQuery.tokenClassDefsOnly.Length, StringComparison.OrdinalIgnoreCase) != 0)
						{
							break;
						}
						ManagementQuery.ParseToken(ref text7, RelatedObjectQuery.tokenClassDefsOnly, ref flag9);
						flag = true;
					}
				}
				if (text7.Length != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				if (flag10 && flag9)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
			}
			this.sourceObject = text8;
			this.relatedClass = text;
			this.relationshipClass = text2;
			this.relatedRole = text3;
			this.thisRole = text4;
			this.relatedQualifier = text5;
			this.relationshipQualifier = text6;
			this.classDefinitionsOnly = flag;
			this.isSchemaQuery = flag2;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000B0F4 File Offset: 0x0000A0F4
		public override object Clone()
		{
			if (!this.isSchemaQuery)
			{
				return new RelatedObjectQuery(this.sourceObject, this.relatedClass, this.relationshipClass, this.relatedQualifier, this.relationshipQualifier, this.relatedRole, this.thisRole, this.classDefinitionsOnly);
			}
			return new RelatedObjectQuery(true, this.sourceObject, this.relatedClass, this.relationshipClass, this.relatedQualifier, this.relationshipQualifier, this.relatedRole, this.thisRole);
		}

		// Token: 0x04000158 RID: 344
		private static readonly string tokenAssociators = "associators";

		// Token: 0x04000159 RID: 345
		private static readonly string tokenOf = "of";

		// Token: 0x0400015A RID: 346
		private static readonly string tokenWhere = "where";

		// Token: 0x0400015B RID: 347
		private static readonly string tokenResultClass = "resultclass";

		// Token: 0x0400015C RID: 348
		private static readonly string tokenAssocClass = "assocclass";

		// Token: 0x0400015D RID: 349
		private static readonly string tokenResultRole = "resultrole";

		// Token: 0x0400015E RID: 350
		private static readonly string tokenRole = "role";

		// Token: 0x0400015F RID: 351
		private static readonly string tokenRequiredQualifier = "requiredqualifier";

		// Token: 0x04000160 RID: 352
		private static readonly string tokenRequiredAssocQualifier = "requiredassocqualifier";

		// Token: 0x04000161 RID: 353
		private static readonly string tokenClassDefsOnly = "classdefsonly";

		// Token: 0x04000162 RID: 354
		private static readonly string tokenSchemaOnly = "schemaonly";

		// Token: 0x04000163 RID: 355
		private bool isSchemaQuery;

		// Token: 0x04000164 RID: 356
		private string sourceObject;

		// Token: 0x04000165 RID: 357
		private string relatedClass;

		// Token: 0x04000166 RID: 358
		private string relationshipClass;

		// Token: 0x04000167 RID: 359
		private string relatedQualifier;

		// Token: 0x04000168 RID: 360
		private string relationshipQualifier;

		// Token: 0x04000169 RID: 361
		private string relatedRole;

		// Token: 0x0400016A RID: 362
		private string thisRole;

		// Token: 0x0400016B RID: 363
		private bool classDefinitionsOnly;
	}
}
