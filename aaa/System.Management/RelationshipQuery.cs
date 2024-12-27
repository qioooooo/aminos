using System;

namespace System.Management
{
	// Token: 0x0200003D RID: 61
	public class RelationshipQuery : WqlObjectQuery
	{
		// Token: 0x0600021E RID: 542 RVA: 0x0000B1EB File Offset: 0x0000A1EB
		public RelationshipQuery()
			: this(null)
		{
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000B1F4 File Offset: 0x0000A1F4
		public RelationshipQuery(string queryOrSourceObject)
		{
			if (queryOrSourceObject == null)
			{
				return;
			}
			if (queryOrSourceObject.TrimStart(new char[0]).StartsWith(RelationshipQuery.tokenReferences, StringComparison.OrdinalIgnoreCase))
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

		// Token: 0x06000220 RID: 544 RVA: 0x0000B273 File Offset: 0x0000A273
		public RelationshipQuery(string sourceObject, string relationshipClass)
			: this(sourceObject, relationshipClass, null, null, false)
		{
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000B280 File Offset: 0x0000A280
		public RelationshipQuery(string sourceObject, string relationshipClass, string relationshipQualifier, string thisRole, bool classDefinitionsOnly)
		{
			this.isSchemaQuery = false;
			this.sourceObject = sourceObject;
			this.relationshipClass = relationshipClass;
			this.relationshipQualifier = relationshipQualifier;
			this.thisRole = thisRole;
			this.classDefinitionsOnly = classDefinitionsOnly;
			this.BuildQuery();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B2BC File Offset: 0x0000A2BC
		public RelationshipQuery(bool isSchemaQuery, string sourceObject, string relationshipClass, string relationshipQualifier, string thisRole)
		{
			if (!isSchemaQuery)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "isSchemaQuery");
			}
			this.isSchemaQuery = true;
			this.sourceObject = sourceObject;
			this.relationshipClass = relationshipClass;
			this.relationshipQualifier = relationshipQualifier;
			this.thisRole = thisRole;
			this.classDefinitionsOnly = false;
			this.BuildQuery();
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000B319 File Offset: 0x0000A319
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000B321 File Offset: 0x0000A321
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

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000B336 File Offset: 0x0000A336
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000B34C File Offset: 0x0000A34C
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

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000B361 File Offset: 0x0000A361
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000B377 File Offset: 0x0000A377
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

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000B38C File Offset: 0x0000A38C
		// (set) Token: 0x0600022A RID: 554 RVA: 0x0000B3A2 File Offset: 0x0000A3A2
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

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000B3B7 File Offset: 0x0000A3B7
		// (set) Token: 0x0600022C RID: 556 RVA: 0x0000B3CD File Offset: 0x0000A3CD
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

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000B3E2 File Offset: 0x0000A3E2
		// (set) Token: 0x0600022E RID: 558 RVA: 0x0000B3EA File Offset: 0x0000A3EA
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

		// Token: 0x0600022F RID: 559 RVA: 0x0000B400 File Offset: 0x0000A400
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
				RelationshipQuery.tokenReferences,
				" ",
				RelationshipQuery.tokenOf,
				" {",
				this.sourceObject,
				"}"
			});
			if (this.RelationshipClass.Length != 0 || this.RelationshipQualifier.Length != 0 || this.ThisRole.Length != 0 || this.classDefinitionsOnly || this.isSchemaQuery)
			{
				text = text + " " + RelationshipQuery.tokenWhere;
				if (this.RelationshipClass.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelationshipQuery.tokenResultClass,
						" = ",
						this.relationshipClass
					});
				}
				if (this.ThisRole.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelationshipQuery.tokenRole,
						" = ",
						this.thisRole
					});
				}
				if (this.RelationshipQualifier.Length != 0)
				{
					text = string.Concat(new string[]
					{
						text,
						" ",
						RelationshipQuery.tokenRequiredQualifier,
						" = ",
						this.relationshipQualifier
					});
				}
				if (!this.isSchemaQuery)
				{
					if (this.classDefinitionsOnly)
					{
						text = text + " " + RelationshipQuery.tokenClassDefsOnly;
					}
				}
				else
				{
					text = text + " " + RelationshipQuery.tokenSchemaOnly;
				}
			}
			base.SetQueryString(text);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000B5C4 File Offset: 0x0000A5C4
		protected internal override void ParseQuery(string query)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			bool flag = false;
			bool flag2 = false;
			string text4 = query.Trim();
			if (string.Compare(text4, 0, RelationshipQuery.tokenReferences, 0, RelationshipQuery.tokenReferences.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "references");
			}
			text4 = text4.Remove(0, RelationshipQuery.tokenReferences.Length);
			if (text4.Length == 0 || !char.IsWhiteSpace(text4[0]))
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			text4 = text4.TrimStart(null);
			if (string.Compare(text4, 0, RelationshipQuery.tokenOf, 0, RelationshipQuery.tokenOf.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "of");
			}
			text4 = text4.Remove(0, RelationshipQuery.tokenOf.Length).TrimStart(null);
			if (text4.IndexOf('{') != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			text4 = text4.Remove(0, 1).TrimStart(null);
			int num;
			if (-1 == (num = text4.IndexOf('}')))
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			string text5 = text4.Substring(0, num).TrimEnd(null);
			text4 = text4.Remove(0, num + 1).TrimStart(null);
			if (0 < text4.Length)
			{
				if (string.Compare(text4, 0, RelationshipQuery.tokenWhere, 0, RelationshipQuery.tokenWhere.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "where");
				}
				text4 = text4.Remove(0, RelationshipQuery.tokenWhere.Length);
				if (text4.Length == 0 || !char.IsWhiteSpace(text4[0]))
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				text4 = text4.TrimStart(null);
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				for (;;)
				{
					if (text4.Length >= RelationshipQuery.tokenResultClass.Length && string.Compare(text4, 0, RelationshipQuery.tokenResultClass, 0, RelationshipQuery.tokenResultClass.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text4, RelationshipQuery.tokenResultClass, "=", ref flag3, ref text);
					}
					else if (text4.Length >= RelationshipQuery.tokenRole.Length && string.Compare(text4, 0, RelationshipQuery.tokenRole, 0, RelationshipQuery.tokenRole.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text4, RelationshipQuery.tokenRole, "=", ref flag4, ref text2);
					}
					else if (text4.Length >= RelationshipQuery.tokenRequiredQualifier.Length && string.Compare(text4, 0, RelationshipQuery.tokenRequiredQualifier, 0, RelationshipQuery.tokenRequiredQualifier.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text4, RelationshipQuery.tokenRequiredQualifier, "=", ref flag5, ref text3);
					}
					else if (text4.Length >= RelationshipQuery.tokenClassDefsOnly.Length && string.Compare(text4, 0, RelationshipQuery.tokenClassDefsOnly, 0, RelationshipQuery.tokenClassDefsOnly.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						ManagementQuery.ParseToken(ref text4, RelationshipQuery.tokenClassDefsOnly, ref flag6);
						flag = true;
					}
					else
					{
						if (text4.Length < RelationshipQuery.tokenSchemaOnly.Length || string.Compare(text4, 0, RelationshipQuery.tokenSchemaOnly, 0, RelationshipQuery.tokenSchemaOnly.Length, StringComparison.OrdinalIgnoreCase) != 0)
						{
							break;
						}
						ManagementQuery.ParseToken(ref text4, RelationshipQuery.tokenSchemaOnly, ref flag7);
						flag2 = true;
					}
				}
				if (text4.Length != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				if (flag && flag2)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
			}
			this.sourceObject = text5;
			this.relationshipClass = text;
			this.thisRole = text2;
			this.relationshipQualifier = text3;
			this.classDefinitionsOnly = flag;
			this.isSchemaQuery = flag2;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000B964 File Offset: 0x0000A964
		public override object Clone()
		{
			if (!this.isSchemaQuery)
			{
				return new RelationshipQuery(this.sourceObject, this.relationshipClass, this.relationshipQualifier, this.thisRole, this.classDefinitionsOnly);
			}
			return new RelationshipQuery(true, this.sourceObject, this.relationshipClass, this.relationshipQualifier, this.thisRole);
		}

		// Token: 0x0400016C RID: 364
		private static readonly string tokenReferences = "references";

		// Token: 0x0400016D RID: 365
		private static readonly string tokenOf = "of";

		// Token: 0x0400016E RID: 366
		private static readonly string tokenWhere = "where";

		// Token: 0x0400016F RID: 367
		private static readonly string tokenResultClass = "resultclass";

		// Token: 0x04000170 RID: 368
		private static readonly string tokenRole = "role";

		// Token: 0x04000171 RID: 369
		private static readonly string tokenRequiredQualifier = "requiredqualifier";

		// Token: 0x04000172 RID: 370
		private static readonly string tokenClassDefsOnly = "classdefsonly";

		// Token: 0x04000173 RID: 371
		private static readonly string tokenSchemaOnly = "schemaonly";

		// Token: 0x04000174 RID: 372
		private string sourceObject;

		// Token: 0x04000175 RID: 373
		private string relationshipClass;

		// Token: 0x04000176 RID: 374
		private string relationshipQualifier;

		// Token: 0x04000177 RID: 375
		private string thisRole;

		// Token: 0x04000178 RID: 376
		private bool classDefinitionsOnly;

		// Token: 0x04000179 RID: 377
		private bool isSchemaQuery;
	}
}
