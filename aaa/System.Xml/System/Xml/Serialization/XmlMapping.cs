using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000310 RID: 784
	public abstract class XmlMapping
	{
		// Token: 0x0600250C RID: 9484 RVA: 0x000ADF51 File Offset: 0x000ACF51
		internal XmlMapping(TypeScope scope, ElementAccessor accessor)
			: this(scope, accessor, XmlMappingAccess.Read | XmlMappingAccess.Write)
		{
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x000ADF5C File Offset: 0x000ACF5C
		internal XmlMapping(TypeScope scope, ElementAccessor accessor, XmlMappingAccess access)
		{
			this.scope = scope;
			this.accessor = accessor;
			this.access = access;
			this.shallow = scope == null;
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x0600250E RID: 9486 RVA: 0x000ADF83 File Offset: 0x000ACF83
		internal ElementAccessor Accessor
		{
			get
			{
				return this.accessor;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x0600250F RID: 9487 RVA: 0x000ADF8B File Offset: 0x000ACF8B
		internal TypeScope Scope
		{
			get
			{
				return this.scope;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06002510 RID: 9488 RVA: 0x000ADF93 File Offset: 0x000ACF93
		public string ElementName
		{
			get
			{
				return global::System.Xml.Serialization.Accessor.UnescapeName(this.Accessor.Name);
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06002511 RID: 9489 RVA: 0x000ADFA5 File Offset: 0x000ACFA5
		public string XsdElementName
		{
			get
			{
				return this.Accessor.Name;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06002512 RID: 9490 RVA: 0x000ADFB2 File Offset: 0x000ACFB2
		public string Namespace
		{
			get
			{
				return this.accessor.Namespace;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06002513 RID: 9491 RVA: 0x000ADFBF File Offset: 0x000ACFBF
		// (set) Token: 0x06002514 RID: 9492 RVA: 0x000ADFC7 File Offset: 0x000ACFC7
		internal bool GenerateSerializer
		{
			get
			{
				return this.generateSerializer;
			}
			set
			{
				this.generateSerializer = value;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06002515 RID: 9493 RVA: 0x000ADFD0 File Offset: 0x000ACFD0
		internal bool IsReadable
		{
			get
			{
				return (this.access & XmlMappingAccess.Read) != XmlMappingAccess.None;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06002516 RID: 9494 RVA: 0x000ADFE0 File Offset: 0x000ACFE0
		internal bool IsWriteable
		{
			get
			{
				return (this.access & XmlMappingAccess.Write) != XmlMappingAccess.None;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06002517 RID: 9495 RVA: 0x000ADFF0 File Offset: 0x000ACFF0
		// (set) Token: 0x06002518 RID: 9496 RVA: 0x000ADFF8 File Offset: 0x000ACFF8
		internal bool IsSoap
		{
			get
			{
				return this.isSoap;
			}
			set
			{
				this.isSoap = value;
			}
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x000AE001 File Offset: 0x000AD001
		public void SetKey(string key)
		{
			this.SetKeyInternal(key);
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000AE00A File Offset: 0x000AD00A
		internal void SetKeyInternal(string key)
		{
			this.key = key;
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000AE014 File Offset: 0x000AD014
		internal static string GenerateKey(Type type, XmlRootAttribute root, string ns)
		{
			if (root == null)
			{
				root = (XmlRootAttribute)XmlAttributes.GetAttr(type, typeof(XmlRootAttribute));
			}
			return string.Concat(new string[]
			{
				type.FullName,
				":",
				(root == null) ? string.Empty : root.Key,
				":",
				(ns == null) ? string.Empty : ns
			});
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x000AE082 File Offset: 0x000AD082
		internal string Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000AE08A File Offset: 0x000AD08A
		internal void CheckShallow()
		{
			if (this.shallow)
			{
				throw new InvalidOperationException(Res.GetString("XmlMelformMapping"));
			}
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x000AE0A4 File Offset: 0x000AD0A4
		internal static bool IsShallow(XmlMapping[] mappings)
		{
			for (int i = 0; i < mappings.Length; i++)
			{
				if (mappings[i] == null || mappings[i].shallow)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001584 RID: 5508
		private TypeScope scope;

		// Token: 0x04001585 RID: 5509
		private bool generateSerializer;

		// Token: 0x04001586 RID: 5510
		private bool isSoap;

		// Token: 0x04001587 RID: 5511
		private ElementAccessor accessor;

		// Token: 0x04001588 RID: 5512
		private string key;

		// Token: 0x04001589 RID: 5513
		private bool shallow;

		// Token: 0x0400158A RID: 5514
		private XmlMappingAccess access;
	}
}
