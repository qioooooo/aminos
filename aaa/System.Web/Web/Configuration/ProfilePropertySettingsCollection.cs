using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x0200022B RID: 555
	[ConfigurationCollection(typeof(ProfilePropertySettings))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ProfilePropertySettingsCollection : ConfigurationElementCollection
	{
		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x00086A00 File Offset: 0x00085A00
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProfilePropertySettingsCollection._properties;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x00086A0F File Offset: 0x00085A0F
		protected virtual bool AllowClear
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001DD9 RID: 7641 RVA: 0x00086A12 File Offset: 0x00085A12
		protected override bool ThrowOnDuplicate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x00086A18 File Offset: 0x00085A18
		protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
		{
			if (!this.AllowClear && elementName == "clear")
			{
				throw new ConfigurationErrorsException(SR.GetString("Clear_not_valid"), reader);
			}
			if (elementName == "group")
			{
				throw new ConfigurationErrorsException(SR.GetString("Nested_group_not_valid"), reader);
			}
			return base.OnDeserializeUnrecognizedElement(elementName, reader);
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001DDB RID: 7643 RVA: 0x00086A71 File Offset: 0x00085A71
		public string[] AllKeys
		{
			get
			{
				return StringUtil.ObjectArrayToStringArray(base.BaseGetAllKeys());
			}
		}

		// Token: 0x17000611 RID: 1553
		public ProfilePropertySettings this[string name]
		{
			get
			{
				return (ProfilePropertySettings)base.BaseGet(name);
			}
		}

		// Token: 0x17000612 RID: 1554
		public ProfilePropertySettings this[int index]
		{
			get
			{
				return (ProfilePropertySettings)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x00086AB4 File Offset: 0x00085AB4
		protected override ConfigurationElement CreateNewElement()
		{
			return new ProfilePropertySettings();
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x00086ABB File Offset: 0x00085ABB
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((ProfilePropertySettings)element).Name;
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x00086AC8 File Offset: 0x00085AC8
		public void Add(ProfilePropertySettings propertySettings)
		{
			this.BaseAdd(propertySettings);
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x00086AD1 File Offset: 0x00085AD1
		public ProfilePropertySettings Get(int index)
		{
			return (ProfilePropertySettings)base.BaseGet(index);
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x00086ADF File Offset: 0x00085ADF
		public ProfilePropertySettings Get(string name)
		{
			return (ProfilePropertySettings)base.BaseGet(name);
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x00086AED File Offset: 0x00085AED
		public string GetKey(int index)
		{
			return (string)base.BaseGetKey(index);
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x00086AFB File Offset: 0x00085AFB
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x00086B04 File Offset: 0x00085B04
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x00086B0D File Offset: 0x00085B0D
		public void Set(ProfilePropertySettings propertySettings)
		{
			base.BaseAdd(propertySettings, false);
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x00086B17 File Offset: 0x00085B17
		public int IndexOf(ProfilePropertySettings propertySettings)
		{
			return base.BaseIndexOf(propertySettings);
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x00086B20 File Offset: 0x00085B20
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x04001992 RID: 6546
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
