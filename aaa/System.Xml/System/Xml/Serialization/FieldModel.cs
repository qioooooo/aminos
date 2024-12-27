using System;
using System.Reflection;

namespace System.Xml.Serialization
{
	// Token: 0x020002DB RID: 731
	internal class FieldModel
	{
		// Token: 0x06002252 RID: 8786 RVA: 0x000A0C3C File Offset: 0x0009FC3C
		internal FieldModel(string name, Type fieldType, TypeDesc fieldTypeDesc, bool checkSpecified, bool checkShouldPersist)
			: this(name, fieldType, fieldTypeDesc, checkSpecified, checkShouldPersist, false)
		{
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000A0C4C File Offset: 0x0009FC4C
		internal FieldModel(string name, Type fieldType, TypeDesc fieldTypeDesc, bool checkSpecified, bool checkShouldPersist, bool readOnly)
		{
			this.fieldTypeDesc = fieldTypeDesc;
			this.name = name;
			this.fieldType = fieldType;
			this.checkSpecified = (checkSpecified ? SpecifiedAccessor.ReadWrite : SpecifiedAccessor.None);
			this.checkShouldPersist = checkShouldPersist;
			this.readOnly = readOnly;
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000A0C88 File Offset: 0x0009FC88
		internal FieldModel(MemberInfo memberInfo, Type fieldType, TypeDesc fieldTypeDesc)
		{
			this.name = memberInfo.Name;
			this.fieldType = fieldType;
			this.fieldTypeDesc = fieldTypeDesc;
			this.checkShouldPersist = memberInfo.DeclaringType.GetMethod("ShouldSerialize" + memberInfo.Name, new Type[0]) != null;
			FieldInfo field = memberInfo.DeclaringType.GetField(memberInfo.Name + "Specified");
			if (field != null)
			{
				if (field.FieldType != typeof(bool))
				{
					throw new InvalidOperationException(Res.GetString("XmlInvalidSpecifiedType", new object[]
					{
						field.Name,
						field.FieldType.FullName,
						typeof(bool).FullName
					}));
				}
				this.checkSpecified = (field.IsInitOnly ? SpecifiedAccessor.ReadOnly : SpecifiedAccessor.ReadWrite);
			}
			else
			{
				PropertyInfo property = memberInfo.DeclaringType.GetProperty(memberInfo.Name + "Specified");
				if (property != null)
				{
					if (StructModel.CheckPropertyRead(property))
					{
						this.checkSpecified = (property.CanWrite ? SpecifiedAccessor.ReadWrite : SpecifiedAccessor.ReadOnly);
					}
					if (this.checkSpecified != SpecifiedAccessor.None && property.PropertyType != typeof(bool))
					{
						throw new InvalidOperationException(Res.GetString("XmlInvalidSpecifiedType", new object[]
						{
							property.Name,
							property.PropertyType.FullName,
							typeof(bool).FullName
						}));
					}
				}
			}
			if (memberInfo is PropertyInfo)
			{
				this.readOnly = !((PropertyInfo)memberInfo).CanWrite;
				this.isProperty = true;
				return;
			}
			if (memberInfo is FieldInfo)
			{
				this.readOnly = ((FieldInfo)memberInfo).IsInitOnly;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002255 RID: 8789 RVA: 0x000A0E3C File Offset: 0x0009FE3C
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002256 RID: 8790 RVA: 0x000A0E44 File Offset: 0x0009FE44
		internal Type FieldType
		{
			get
			{
				return this.fieldType;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x000A0E4C File Offset: 0x0009FE4C
		internal TypeDesc FieldTypeDesc
		{
			get
			{
				return this.fieldTypeDesc;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x000A0E54 File Offset: 0x0009FE54
		internal bool CheckShouldPersist
		{
			get
			{
				return this.checkShouldPersist;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002259 RID: 8793 RVA: 0x000A0E5C File Offset: 0x0009FE5C
		internal SpecifiedAccessor CheckSpecified
		{
			get
			{
				return this.checkSpecified;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x0600225A RID: 8794 RVA: 0x000A0E64 File Offset: 0x0009FE64
		internal bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x0600225B RID: 8795 RVA: 0x000A0E6C File Offset: 0x0009FE6C
		internal bool IsProperty
		{
			get
			{
				return this.isProperty;
			}
		}

		// Token: 0x040014B6 RID: 5302
		private SpecifiedAccessor checkSpecified;

		// Token: 0x040014B7 RID: 5303
		private bool checkShouldPersist;

		// Token: 0x040014B8 RID: 5304
		private bool readOnly;

		// Token: 0x040014B9 RID: 5305
		private bool isProperty;

		// Token: 0x040014BA RID: 5306
		private Type fieldType;

		// Token: 0x040014BB RID: 5307
		private string name;

		// Token: 0x040014BC RID: 5308
		private TypeDesc fieldTypeDesc;
	}
}
