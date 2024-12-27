using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security
{
	// Token: 0x02000660 RID: 1632
	[ComVisible(true)]
	[Serializable]
	public sealed class NamedPermissionSet : PermissionSet
	{
		// Token: 0x06003B7C RID: 15228 RVA: 0x000CBE4E File Offset: 0x000CAE4E
		internal NamedPermissionSet()
		{
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x000CBE56 File Offset: 0x000CAE56
		public NamedPermissionSet(string name)
		{
			NamedPermissionSet.CheckName(name);
			this.m_name = name;
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x000CBE6B File Offset: 0x000CAE6B
		public NamedPermissionSet(string name, PermissionState state)
			: base(state)
		{
			NamedPermissionSet.CheckName(name);
			this.m_name = name;
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x000CBE81 File Offset: 0x000CAE81
		public NamedPermissionSet(string name, PermissionSet permSet)
			: base(permSet)
		{
			NamedPermissionSet.CheckName(name);
			this.m_name = name;
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x000CBE97 File Offset: 0x000CAE97
		public NamedPermissionSet(NamedPermissionSet permSet)
			: base(permSet)
		{
			this.m_name = permSet.m_name;
			this.m_description = permSet.Description;
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06003B81 RID: 15233 RVA: 0x000CBEB8 File Offset: 0x000CAEB8
		// (set) Token: 0x06003B82 RID: 15234 RVA: 0x000CBEC0 File Offset: 0x000CAEC0
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				NamedPermissionSet.CheckName(value);
				this.m_name = value;
			}
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x000CBECF File Offset: 0x000CAECF
		private static void CheckName(string name)
		{
			if (name == null || name.Equals(""))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NPMSInvalidName"));
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06003B84 RID: 15236 RVA: 0x000CBEF1 File Offset: 0x000CAEF1
		// (set) Token: 0x06003B85 RID: 15237 RVA: 0x000CBF19 File Offset: 0x000CAF19
		public string Description
		{
			get
			{
				if (this.m_descrResource != null)
				{
					this.m_description = Environment.GetResourceString(this.m_descrResource);
					this.m_descrResource = null;
				}
				return this.m_description;
			}
			set
			{
				this.m_description = value;
				this.m_descrResource = null;
			}
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x000CBF29 File Offset: 0x000CAF29
		public override PermissionSet Copy()
		{
			return new NamedPermissionSet(this);
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x000CBF34 File Offset: 0x000CAF34
		public NamedPermissionSet Copy(string name)
		{
			return new NamedPermissionSet(this)
			{
				Name = name
			};
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x000CBF50 File Offset: 0x000CAF50
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.ToXml("System.Security.NamedPermissionSet");
			if (this.m_name != null && !this.m_name.Equals(""))
			{
				securityElement.AddAttribute("Name", SecurityElement.Escape(this.m_name));
			}
			if (this.Description != null && !this.Description.Equals(""))
			{
				securityElement.AddAttribute("Description", SecurityElement.Escape(this.Description));
			}
			return securityElement;
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x000CBFCA File Offset: 0x000CAFCA
		public override void FromXml(SecurityElement et)
		{
			this.FromXml(et, false, false);
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x000CBFD8 File Offset: 0x000CAFD8
		internal override void FromXml(SecurityElement et, bool allowInternalOnly, bool ignoreTypeLoadFailures)
		{
			if (et == null)
			{
				throw new ArgumentNullException("et");
			}
			string text = et.Attribute("Name");
			this.m_name = ((text == null) ? null : text);
			text = et.Attribute("Description");
			this.m_description = ((text == null) ? "" : text);
			this.m_descrResource = null;
			base.FromXml(et, allowInternalOnly, ignoreTypeLoadFailures);
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x000CC03C File Offset: 0x000CB03C
		internal void FromXmlNameOnly(SecurityElement et)
		{
			string text = et.Attribute("Name");
			this.m_name = ((text == null) ? null : text);
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x000CC062 File Offset: 0x000CB062
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x000CC06B File Offset: 0x000CB06B
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04001E8A RID: 7818
		private string m_name;

		// Token: 0x04001E8B RID: 7819
		private string m_description;

		// Token: 0x04001E8C RID: 7820
		[OptionalField(VersionAdded = 2)]
		internal string m_descrResource;
	}
}
