using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002CC RID: 716
	internal class StructMapping : TypeMapping, INameScope
	{
		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x060021D3 RID: 8659 RVA: 0x0009F3BC File Offset: 0x0009E3BC
		// (set) Token: 0x060021D4 RID: 8660 RVA: 0x0009F3C4 File Offset: 0x0009E3C4
		internal StructMapping BaseMapping
		{
			get
			{
				return this.baseMapping;
			}
			set
			{
				this.baseMapping = value;
				if (!base.IsAnonymousType && this.baseMapping != null)
				{
					this.nextDerivedMapping = this.baseMapping.derivedMappings;
					this.baseMapping.derivedMappings = this;
				}
				if (value.isSequence && !this.isSequence)
				{
					this.isSequence = true;
					if (this.baseMapping.IsSequence)
					{
						for (StructMapping structMapping = this.derivedMappings; structMapping != null; structMapping = structMapping.NextDerivedMapping)
						{
							structMapping.SetSequence();
						}
					}
				}
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x060021D5 RID: 8661 RVA: 0x0009F442 File Offset: 0x0009E442
		internal StructMapping DerivedMappings
		{
			get
			{
				return this.derivedMappings;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x060021D6 RID: 8662 RVA: 0x0009F44A File Offset: 0x0009E44A
		internal bool IsFullyInitialized
		{
			get
			{
				return this.baseMapping != null && this.Members != null;
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x060021D7 RID: 8663 RVA: 0x0009F462 File Offset: 0x0009E462
		internal NameTable LocalElements
		{
			get
			{
				if (this.elements == null)
				{
					this.elements = new NameTable();
				}
				return this.elements;
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x060021D8 RID: 8664 RVA: 0x0009F47D File Offset: 0x0009E47D
		internal NameTable LocalAttributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new NameTable();
				}
				return this.attributes;
			}
		}

		// Token: 0x1700082D RID: 2093
		object INameScope.this[string name, string ns]
		{
			get
			{
				object obj = this.LocalElements[name, ns];
				if (obj != null)
				{
					return obj;
				}
				if (this.baseMapping != null)
				{
					return ((INameScope)this.baseMapping)[name, ns];
				}
				return null;
			}
			set
			{
				this.LocalElements[name, ns] = value;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x060021DB RID: 8667 RVA: 0x0009F4DF File Offset: 0x0009E4DF
		internal StructMapping NextDerivedMapping
		{
			get
			{
				return this.nextDerivedMapping;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x0009F4E7 File Offset: 0x0009E4E7
		internal bool HasSimpleContent
		{
			get
			{
				return this.hasSimpleContent;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x060021DD RID: 8669 RVA: 0x0009F4F0 File Offset: 0x0009E4F0
		internal bool HasXmlnsMember
		{
			get
			{
				for (StructMapping structMapping = this; structMapping != null; structMapping = structMapping.BaseMapping)
				{
					if (structMapping.XmlnsMember != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x0009F516 File Offset: 0x0009E516
		// (set) Token: 0x060021DF RID: 8671 RVA: 0x0009F51E File Offset: 0x0009E51E
		internal MemberMapping[] Members
		{
			get
			{
				return this.members;
			}
			set
			{
				this.members = value;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x060021E0 RID: 8672 RVA: 0x0009F527 File Offset: 0x0009E527
		// (set) Token: 0x060021E1 RID: 8673 RVA: 0x0009F52F File Offset: 0x0009E52F
		internal MemberMapping XmlnsMember
		{
			get
			{
				return this.xmlnsMember;
			}
			set
			{
				this.xmlnsMember = value;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x060021E2 RID: 8674 RVA: 0x0009F538 File Offset: 0x0009E538
		// (set) Token: 0x060021E3 RID: 8675 RVA: 0x0009F540 File Offset: 0x0009E540
		internal bool IsOpenModel
		{
			get
			{
				return this.openModel;
			}
			set
			{
				this.openModel = value;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x060021E4 RID: 8676 RVA: 0x0009F549 File Offset: 0x0009E549
		// (set) Token: 0x060021E5 RID: 8677 RVA: 0x0009F564 File Offset: 0x0009E564
		internal CodeIdentifiers Scope
		{
			get
			{
				if (this.scope == null)
				{
					this.scope = new CodeIdentifiers();
				}
				return this.scope;
			}
			set
			{
				this.scope = value;
			}
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x0009F570 File Offset: 0x0009E570
		internal MemberMapping FindDeclaringMapping(MemberMapping member, out StructMapping declaringMapping, string parent)
		{
			declaringMapping = null;
			if (this.BaseMapping != null)
			{
				MemberMapping memberMapping = this.BaseMapping.FindDeclaringMapping(member, out declaringMapping, parent);
				if (memberMapping != null)
				{
					return memberMapping;
				}
			}
			if (this.members == null)
			{
				return null;
			}
			int i = 0;
			while (i < this.members.Length)
			{
				if (this.members[i].Name == member.Name)
				{
					if (this.members[i].TypeDesc != member.TypeDesc)
					{
						throw new InvalidOperationException(Res.GetString("XmlHiddenMember", new object[]
						{
							parent,
							member.Name,
							member.TypeDesc.FullName,
							base.TypeName,
							this.members[i].Name,
							this.members[i].TypeDesc.FullName
						}));
					}
					if (!this.members[i].Match(member))
					{
						throw new InvalidOperationException(Res.GetString("XmlInvalidXmlOverride", new object[]
						{
							parent,
							member.Name,
							base.TypeName,
							this.members[i].Name
						}));
					}
					declaringMapping = this;
					return this.members[i];
				}
				else
				{
					i++;
				}
			}
			return null;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x0009F6B0 File Offset: 0x0009E6B0
		internal bool Declares(MemberMapping member, string parent)
		{
			StructMapping structMapping;
			return this.FindDeclaringMapping(member, out structMapping, parent) != null;
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x0009F6D0 File Offset: 0x0009E6D0
		internal void SetContentModel(TextAccessor text, bool hasElements)
		{
			if (this.BaseMapping == null || this.BaseMapping.TypeDesc.IsRoot)
			{
				this.hasSimpleContent = !hasElements && text != null && !text.Mapping.IsList;
			}
			else if (this.BaseMapping.HasSimpleContent)
			{
				if (text != null || hasElements)
				{
					throw new InvalidOperationException(Res.GetString("XmlIllegalSimpleContentExtension", new object[]
					{
						base.TypeDesc.FullName,
						this.BaseMapping.TypeDesc.FullName
					}));
				}
				this.hasSimpleContent = true;
			}
			else
			{
				this.hasSimpleContent = false;
			}
			if (!this.hasSimpleContent && text != null && !text.Mapping.TypeDesc.CanBeTextValue)
			{
				throw new InvalidOperationException(Res.GetString("XmlIllegalTypedTextAttribute", new object[]
				{
					base.TypeDesc.FullName,
					text.Name,
					text.Mapping.TypeDesc.FullName
				}));
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x060021E9 RID: 8681 RVA: 0x0009F7D1 File Offset: 0x0009E7D1
		internal bool HasElements
		{
			get
			{
				return this.elements != null && this.elements.Values.Count > 0;
			}
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x0009F7F0 File Offset: 0x0009E7F0
		internal bool HasExplicitSequence()
		{
			if (this.members != null)
			{
				for (int i = 0; i < this.members.Length; i++)
				{
					if (this.members[i].IsParticle && this.members[i].IsSequence)
					{
						return true;
					}
				}
			}
			return this.baseMapping != null && this.baseMapping.HasExplicitSequence();
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x0009F850 File Offset: 0x0009E850
		internal void SetSequence()
		{
			if (base.TypeDesc.IsRoot)
			{
				return;
			}
			StructMapping structMapping = this;
			while (!structMapping.BaseMapping.IsSequence && structMapping.BaseMapping != null && !structMapping.BaseMapping.TypeDesc.IsRoot)
			{
				structMapping = structMapping.BaseMapping;
			}
			structMapping.IsSequence = true;
			for (StructMapping structMapping2 = structMapping.DerivedMappings; structMapping2 != null; structMapping2 = structMapping2.NextDerivedMapping)
			{
				structMapping2.SetSequence();
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x0009F8BD File Offset: 0x0009E8BD
		// (set) Token: 0x060021ED RID: 8685 RVA: 0x0009F8D7 File Offset: 0x0009E8D7
		internal bool IsSequence
		{
			get
			{
				return this.isSequence && !base.TypeDesc.IsRoot;
			}
			set
			{
				this.isSequence = value;
			}
		}

		// Token: 0x04001480 RID: 5248
		private MemberMapping[] members;

		// Token: 0x04001481 RID: 5249
		private StructMapping baseMapping;

		// Token: 0x04001482 RID: 5250
		private StructMapping derivedMappings;

		// Token: 0x04001483 RID: 5251
		private StructMapping nextDerivedMapping;

		// Token: 0x04001484 RID: 5252
		private MemberMapping xmlnsMember;

		// Token: 0x04001485 RID: 5253
		private bool hasSimpleContent;

		// Token: 0x04001486 RID: 5254
		private bool openModel;

		// Token: 0x04001487 RID: 5255
		private bool isSequence;

		// Token: 0x04001488 RID: 5256
		private NameTable elements;

		// Token: 0x04001489 RID: 5257
		private NameTable attributes;

		// Token: 0x0400148A RID: 5258
		private CodeIdentifiers scope;
	}
}
