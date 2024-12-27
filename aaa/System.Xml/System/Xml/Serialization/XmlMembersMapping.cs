using System;
using System.Text;

namespace System.Xml.Serialization
{
	// Token: 0x02000312 RID: 786
	public class XmlMembersMapping : XmlMapping
	{
		// Token: 0x0600252D RID: 9517 RVA: 0x000AE1C4 File Offset: 0x000AD1C4
		internal XmlMembersMapping(TypeScope scope, ElementAccessor accessor, XmlMappingAccess access)
			: base(scope, accessor, access)
		{
			MembersMapping membersMapping = (MembersMapping)accessor.Mapping;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(":");
			this.mappings = new XmlMemberMapping[membersMapping.Members.Length];
			for (int i = 0; i < this.mappings.Length; i++)
			{
				if (membersMapping.Members[i].TypeDesc.Type != null)
				{
					stringBuilder.Append(XmlMapping.GenerateKey(membersMapping.Members[i].TypeDesc.Type, null, null));
					stringBuilder.Append(":");
				}
				this.mappings[i] = new XmlMemberMapping(membersMapping.Members[i]);
			}
			base.SetKeyInternal(stringBuilder.ToString());
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x0600252E RID: 9518 RVA: 0x000AE27F File Offset: 0x000AD27F
		public string TypeName
		{
			get
			{
				return base.Accessor.Mapping.TypeName;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x0600252F RID: 9519 RVA: 0x000AE291 File Offset: 0x000AD291
		public string TypeNamespace
		{
			get
			{
				return base.Accessor.Mapping.Namespace;
			}
		}

		// Token: 0x17000934 RID: 2356
		public XmlMemberMapping this[int index]
		{
			get
			{
				return this.mappings[index];
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06002531 RID: 9521 RVA: 0x000AE2AD File Offset: 0x000AD2AD
		public int Count
		{
			get
			{
				return this.mappings.Length;
			}
		}

		// Token: 0x0400158C RID: 5516
		private XmlMemberMapping[] mappings;
	}
}
