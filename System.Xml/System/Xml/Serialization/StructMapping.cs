using System;

namespace System.Xml.Serialization
{
	internal class StructMapping : TypeMapping, INameScope
	{
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

		internal StructMapping DerivedMappings
		{
			get
			{
				return this.derivedMappings;
			}
		}

		internal bool IsFullyInitialized
		{
			get
			{
				return this.baseMapping != null && this.Members != null;
			}
		}

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

		internal StructMapping NextDerivedMapping
		{
			get
			{
				return this.nextDerivedMapping;
			}
		}

		internal bool HasSimpleContent
		{
			get
			{
				return this.hasSimpleContent;
			}
		}

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

		internal bool Declares(MemberMapping member, string parent)
		{
			StructMapping structMapping;
			return this.FindDeclaringMapping(member, out structMapping, parent) != null;
		}

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

		internal bool HasElements
		{
			get
			{
				return this.elements != null && this.elements.Values.Count > 0;
			}
		}

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

		private MemberMapping[] members;

		private StructMapping baseMapping;

		private StructMapping derivedMappings;

		private StructMapping nextDerivedMapping;

		private MemberMapping xmlnsMember;

		private bool hasSimpleContent;

		private bool openModel;

		private bool isSequence;

		private NameTable elements;

		private NameTable attributes;

		private CodeIdentifiers scope;
	}
}
