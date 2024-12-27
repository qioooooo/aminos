using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Permissions
{
	// Token: 0x0200073C RID: 1852
	[SecurityPermission(SecurityAction.InheritanceDemand, ControlEvidence = true, ControlPolicy = true)]
	[Serializable]
	public abstract class ResourcePermissionBase : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x06003867 RID: 14439 RVA: 0x000EDEE8 File Offset: 0x000ECEE8
		protected ResourcePermissionBase()
		{
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x000EDEFB File Offset: 0x000ECEFB
		protected ResourcePermissionBase(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.isUnrestricted = true;
				return;
			}
			if (state == PermissionState.None)
			{
				this.isUnrestricted = false;
				return;
			}
			throw new ArgumentException(SR.GetString("InvalidPermissionState"), "state");
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x000EDF39 File Offset: 0x000ECF39
		private static Hashtable CreateHashtable()
		{
			return new Hashtable(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x0600386A RID: 14442 RVA: 0x000EDF48 File Offset: 0x000ECF48
		private string ComputerName
		{
			get
			{
				if (ResourcePermissionBase.computerName == null)
				{
					lock (typeof(ResourcePermissionBase))
					{
						if (ResourcePermissionBase.computerName == null)
						{
							StringBuilder stringBuilder = new StringBuilder(256);
							int capacity = stringBuilder.Capacity;
							ResourcePermissionBase.UnsafeNativeMethods.GetComputerName(stringBuilder, ref capacity);
							ResourcePermissionBase.computerName = stringBuilder.ToString();
						}
					}
				}
				return ResourcePermissionBase.computerName;
			}
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x0600386B RID: 14443 RVA: 0x000EDFB8 File Offset: 0x000ECFB8
		private bool IsEmpty
		{
			get
			{
				return !this.isUnrestricted && this.rootTable.Count == 0;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x0600386C RID: 14444 RVA: 0x000EDFD2 File Offset: 0x000ECFD2
		// (set) Token: 0x0600386D RID: 14445 RVA: 0x000EDFDA File Offset: 0x000ECFDA
		protected Type PermissionAccessType
		{
			get
			{
				return this.permissionAccessType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!value.IsEnum)
				{
					throw new ArgumentException(SR.GetString("PermissionBadParameterEnum"), "value");
				}
				this.permissionAccessType = value;
			}
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x0600386E RID: 14446 RVA: 0x000EE00E File Offset: 0x000ED00E
		// (set) Token: 0x0600386F RID: 14447 RVA: 0x000EE018 File Offset: 0x000ED018
		protected string[] TagNames
		{
			get
			{
				return this.tagNames;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("PermissionInvalidLength", new object[] { "0" }), "value");
				}
				this.tagNames = value;
			}
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x000EE064 File Offset: 0x000ED064
		protected void AddPermissionAccess(ResourcePermissionBaseEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			if (entry.PermissionAccessPath.Length != this.TagNames.Length)
			{
				throw new InvalidOperationException(SR.GetString("PermissionNumberOfElements"));
			}
			Hashtable hashtable = this.rootTable;
			string[] permissionAccessPath = entry.PermissionAccessPath;
			for (int i = 0; i < permissionAccessPath.Length - 1; i++)
			{
				if (hashtable.ContainsKey(permissionAccessPath[i]))
				{
					hashtable = (Hashtable)hashtable[permissionAccessPath[i]];
				}
				else
				{
					Hashtable hashtable2 = ResourcePermissionBase.CreateHashtable();
					hashtable[permissionAccessPath[i]] = hashtable2;
					hashtable = hashtable2;
				}
			}
			if (hashtable.ContainsKey(permissionAccessPath[permissionAccessPath.Length - 1]))
			{
				throw new InvalidOperationException(SR.GetString("PermissionItemExists"));
			}
			hashtable[permissionAccessPath[permissionAccessPath.Length - 1]] = entry.PermissionAccess;
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x000EE124 File Offset: 0x000ED124
		protected void Clear()
		{
			this.rootTable.Clear();
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x000EE134 File Offset: 0x000ED134
		public override IPermission Copy()
		{
			ResourcePermissionBase resourcePermissionBase = this.CreateInstance();
			resourcePermissionBase.tagNames = this.tagNames;
			resourcePermissionBase.permissionAccessType = this.permissionAccessType;
			resourcePermissionBase.isUnrestricted = this.isUnrestricted;
			resourcePermissionBase.rootTable = this.CopyChildren(this.rootTable, 0);
			return resourcePermissionBase;
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x000EE180 File Offset: 0x000ED180
		private Hashtable CopyChildren(object currentContent, int tagIndex)
		{
			IDictionaryEnumerator enumerator = ((Hashtable)currentContent).GetEnumerator();
			Hashtable hashtable = ResourcePermissionBase.CreateHashtable();
			while (enumerator.MoveNext())
			{
				if (tagIndex < this.TagNames.Length - 1)
				{
					hashtable[enumerator.Key] = this.CopyChildren(enumerator.Value, tagIndex + 1);
				}
				else
				{
					hashtable[enumerator.Key] = enumerator.Value;
				}
			}
			return hashtable;
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x000EE1E6 File Offset: 0x000ED1E6
		private ResourcePermissionBase CreateInstance()
		{
			new PermissionSet(PermissionState.Unrestricted).Assert();
			return (ResourcePermissionBase)Activator.CreateInstance(base.GetType(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x000EE20B File Offset: 0x000ED20B
		protected ResourcePermissionBaseEntry[] GetPermissionEntries()
		{
			return this.GetChildrenAccess(this.rootTable, 0);
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x000EE21C File Offset: 0x000ED21C
		private ResourcePermissionBaseEntry[] GetChildrenAccess(object currentContent, int tagIndex)
		{
			IDictionaryEnumerator enumerator = ((Hashtable)currentContent).GetEnumerator();
			ArrayList arrayList = new ArrayList();
			while (enumerator.MoveNext())
			{
				if (tagIndex < this.TagNames.Length - 1)
				{
					ResourcePermissionBaseEntry[] childrenAccess = this.GetChildrenAccess(enumerator.Value, tagIndex + 1);
					for (int i = 0; i < childrenAccess.Length; i++)
					{
						childrenAccess[i].PermissionAccessPath[tagIndex] = (string)enumerator.Key;
					}
					arrayList.AddRange(childrenAccess);
				}
				else
				{
					ResourcePermissionBaseEntry resourcePermissionBaseEntry = new ResourcePermissionBaseEntry((int)enumerator.Value, new string[this.TagNames.Length]);
					resourcePermissionBaseEntry.PermissionAccessPath[tagIndex] = (string)enumerator.Key;
					arrayList.Add(resourcePermissionBaseEntry);
				}
			}
			return (ResourcePermissionBaseEntry[])arrayList.ToArray(typeof(ResourcePermissionBaseEntry));
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x000EE2E8 File Offset: 0x000ED2E8
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			if (!securityElement.Tag.Equals("Permission") && !securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("Argument_NotAPermissionElement"));
			}
			string text = securityElement.Attribute("version");
			if (text != null && !text.Equals("1"))
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidXMLBadVersion"));
			}
			string text2 = securityElement.Attribute("Unrestricted");
			if (text2 != null && string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.isUnrestricted = true;
				return;
			}
			this.isUnrestricted = false;
			this.rootTable = (Hashtable)this.ReadChildren(securityElement, 0);
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x000EE3A4 File Offset: 0x000ED3A4
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (target.GetType() != base.GetType())
			{
				throw new ArgumentException(SR.GetString("PermissionTypeMismatch"), "target");
			}
			ResourcePermissionBase resourcePermissionBase = (ResourcePermissionBase)target;
			if (this.IsUnrestricted())
			{
				return resourcePermissionBase.Copy();
			}
			if (resourcePermissionBase.IsUnrestricted())
			{
				return this.Copy();
			}
			ResourcePermissionBase resourcePermissionBase2 = null;
			Hashtable hashtable = (Hashtable)this.IntersectContents(this.rootTable, resourcePermissionBase.rootTable);
			if (hashtable != null)
			{
				resourcePermissionBase2 = this.CreateInstance();
				resourcePermissionBase2.rootTable = hashtable;
			}
			return resourcePermissionBase2;
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x000EE42C File Offset: 0x000ED42C
		private object IntersectContents(object currentContent, object targetContent)
		{
			if (currentContent is int)
			{
				int num = (int)currentContent;
				int num2 = (int)targetContent;
				return num & num2;
			}
			Hashtable hashtable = ResourcePermissionBase.CreateHashtable();
			object obj = ((Hashtable)currentContent)["."];
			object obj2 = ((Hashtable)currentContent)[this.ComputerName];
			if (obj != null || obj2 != null)
			{
				object obj3 = ((Hashtable)targetContent)["."];
				object obj4 = ((Hashtable)targetContent)[this.ComputerName];
				if (obj3 != null || obj4 != null)
				{
					object obj5 = obj;
					if (obj != null && obj2 != null)
					{
						obj5 = this.UnionOfContents(obj, obj2);
					}
					else if (obj2 != null)
					{
						obj5 = obj2;
					}
					object obj6 = obj3;
					if (obj3 != null && obj4 != null)
					{
						obj6 = this.UnionOfContents(obj3, obj4);
					}
					else if (obj4 != null)
					{
						obj6 = obj4;
					}
					object obj7 = this.IntersectContents(obj5, obj6);
					if (this.HasContent(obj7))
					{
						if (obj2 != null || obj4 != null)
						{
							hashtable[this.ComputerName] = obj7;
						}
						else
						{
							hashtable["."] = obj7;
						}
					}
				}
			}
			IDictionaryEnumerator dictionaryEnumerator;
			Hashtable hashtable2;
			if (((Hashtable)currentContent).Count < ((Hashtable)targetContent).Count)
			{
				dictionaryEnumerator = ((Hashtable)currentContent).GetEnumerator();
				hashtable2 = (Hashtable)targetContent;
			}
			else
			{
				dictionaryEnumerator = ((Hashtable)targetContent).GetEnumerator();
				hashtable2 = (Hashtable)currentContent;
			}
			while (dictionaryEnumerator.MoveNext())
			{
				string text = (string)dictionaryEnumerator.Key;
				if (hashtable2.ContainsKey(text) && text != "." && text != this.ComputerName)
				{
					object value = dictionaryEnumerator.Value;
					object obj8 = hashtable2[text];
					object obj9 = this.IntersectContents(value, obj8);
					if (this.HasContent(obj9))
					{
						hashtable[text] = obj9;
					}
				}
			}
			if (hashtable.Count <= 0)
			{
				return null;
			}
			return hashtable;
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x000EE600 File Offset: 0x000ED600
		private bool HasContent(object value)
		{
			return value != null && (!(value is int) || (int)value != 0);
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x000EE628 File Offset: 0x000ED628
		private bool IsContentSubset(object currentContent, object targetContent)
		{
			if (currentContent is int)
			{
				int num = (int)currentContent;
				int num2 = (int)targetContent;
				return (num & num2) == num;
			}
			Hashtable hashtable = (Hashtable)currentContent;
			Hashtable hashtable2 = (Hashtable)targetContent;
			object obj = hashtable2["*"];
			if (obj != null)
			{
				foreach (object obj2 in hashtable)
				{
					if (!this.IsContentSubset(((DictionaryEntry)obj2).Value, obj))
					{
						return false;
					}
				}
				return true;
			}
			foreach (object obj3 in hashtable)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
				string text = (string)dictionaryEntry.Key;
				if (text != "." && text != this.ComputerName)
				{
					if (!hashtable2.ContainsKey(text))
					{
						return false;
					}
					if (!this.IsContentSubset(dictionaryEntry.Value, hashtable2[text]))
					{
						return false;
					}
				}
			}
			object obj4 = this.MergeContents(hashtable["."], hashtable[this.ComputerName]);
			if (obj4 != null)
			{
				object obj5 = this.MergeContents(hashtable2["."], hashtable2[this.ComputerName]);
				if (obj5 != null)
				{
					return this.IsContentSubset(obj4, obj5);
				}
				if (!this.IsEmpty)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x000EE7D4 File Offset: 0x000ED7D4
		private object MergeContents(object content1, object content2)
		{
			if (content1 == null)
			{
				if (content2 == null)
				{
					return null;
				}
				return content2;
			}
			else
			{
				if (content2 == null)
				{
					return content1;
				}
				return this.UnionOfContents(content1, content2);
			}
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x000EE7F0 File Offset: 0x000ED7F0
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.IsEmpty;
			}
			if (target.GetType() != base.GetType())
			{
				return false;
			}
			ResourcePermissionBase resourcePermissionBase = (ResourcePermissionBase)target;
			return resourcePermissionBase.IsUnrestricted() || (!this.IsUnrestricted() && this.IsContentSubset(this.rootTable, resourcePermissionBase.rootTable));
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x000EE844 File Offset: 0x000ED844
		public bool IsUnrestricted()
		{
			return this.isUnrestricted;
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x000EE84C File Offset: 0x000ED84C
		private object ReadChildren(SecurityElement securityElement, int tagIndex)
		{
			Hashtable hashtable = ResourcePermissionBase.CreateHashtable();
			if (securityElement.Children != null)
			{
				for (int i = 0; i < securityElement.Children.Count; i++)
				{
					SecurityElement securityElement2 = (SecurityElement)securityElement.Children[i];
					if (securityElement2.Tag == this.TagNames[tagIndex])
					{
						string text = securityElement2.Attribute("name");
						if (tagIndex < this.TagNames.Length - 1)
						{
							hashtable[text] = this.ReadChildren(securityElement2, tagIndex + 1);
						}
						else
						{
							string text2 = securityElement2.Attribute("access");
							int num = 0;
							if (text2 != null)
							{
								num = (int)Enum.Parse(this.PermissionAccessType, text2);
							}
							hashtable[text] = num;
						}
					}
				}
			}
			return hashtable;
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x000EE910 File Offset: 0x000ED910
		protected void RemovePermissionAccess(ResourcePermissionBaseEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			if (entry.PermissionAccessPath.Length != this.TagNames.Length)
			{
				throw new InvalidOperationException(SR.GetString("PermissionNumberOfElements"));
			}
			Hashtable hashtable = this.rootTable;
			string[] permissionAccessPath = entry.PermissionAccessPath;
			for (int i = 0; i < permissionAccessPath.Length; i++)
			{
				if (hashtable == null || !hashtable.ContainsKey(permissionAccessPath[i]))
				{
					throw new InvalidOperationException(SR.GetString("PermissionItemDoesntExist"));
				}
				Hashtable hashtable2 = hashtable;
				if (i < permissionAccessPath.Length - 1)
				{
					hashtable = (Hashtable)hashtable[permissionAccessPath[i]];
					if (hashtable.Count == 1)
					{
						hashtable2.Remove(permissionAccessPath[i]);
					}
				}
				else
				{
					hashtable = null;
					hashtable2.Remove(permissionAccessPath[i]);
				}
			}
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x000EE9C0 File Offset: 0x000ED9C0
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			Type type = base.GetType();
			securityElement.AddAttribute("class", type.FullName + ", " + type.Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (this.isUnrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
				return securityElement;
			}
			this.WriteChildren(securityElement, this.rootTable, 0);
			return securityElement;
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x000EEA4C File Offset: 0x000EDA4C
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (target.GetType() != base.GetType())
			{
				throw new ArgumentException(SR.GetString("PermissionTypeMismatch"), "target");
			}
			ResourcePermissionBase resourcePermissionBase = (ResourcePermissionBase)target;
			ResourcePermissionBase resourcePermissionBase2 = null;
			if (this.IsUnrestricted() || resourcePermissionBase.IsUnrestricted())
			{
				resourcePermissionBase2 = this.CreateInstance();
				resourcePermissionBase2.isUnrestricted = true;
			}
			else
			{
				Hashtable hashtable = (Hashtable)this.UnionOfContents(this.rootTable, resourcePermissionBase.rootTable);
				if (hashtable != null)
				{
					resourcePermissionBase2 = this.CreateInstance();
					resourcePermissionBase2.rootTable = hashtable;
				}
			}
			return resourcePermissionBase2;
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x000EEADC File Offset: 0x000EDADC
		private object UnionOfContents(object currentContent, object targetContent)
		{
			if (currentContent is int)
			{
				int num = (int)currentContent;
				int num2 = (int)targetContent;
				return num | num2;
			}
			Hashtable hashtable = ResourcePermissionBase.CreateHashtable();
			IDictionaryEnumerator enumerator = ((Hashtable)currentContent).GetEnumerator();
			IDictionaryEnumerator enumerator2 = ((Hashtable)targetContent).GetEnumerator();
			while (enumerator.MoveNext())
			{
				hashtable[(string)enumerator.Key] = enumerator.Value;
			}
			while (enumerator2.MoveNext())
			{
				if (!hashtable.ContainsKey(enumerator2.Key))
				{
					hashtable[enumerator2.Key] = enumerator2.Value;
				}
				else
				{
					object obj = hashtable[enumerator2.Key];
					object value = enumerator2.Value;
					hashtable[enumerator2.Key] = this.UnionOfContents(obj, value);
				}
			}
			if (hashtable.Count <= 0)
			{
				return null;
			}
			return hashtable;
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x000EEBB4 File Offset: 0x000EDBB4
		private void WriteChildren(SecurityElement currentElement, object currentContent, int tagIndex)
		{
			IDictionaryEnumerator enumerator = ((Hashtable)currentContent).GetEnumerator();
			while (enumerator.MoveNext())
			{
				SecurityElement securityElement = new SecurityElement(this.TagNames[tagIndex]);
				currentElement.AddChild(securityElement);
				securityElement.AddAttribute("name", (string)enumerator.Key);
				if (tagIndex < this.TagNames.Length - 1)
				{
					this.WriteChildren(securityElement, enumerator.Value, tagIndex + 1);
				}
				else
				{
					int num = (int)enumerator.Value;
					if (this.PermissionAccessType != null && num != 0)
					{
						string text = Enum.Format(this.PermissionAccessType, num, "g");
						securityElement.AddAttribute("access", text);
					}
				}
			}
		}

		// Token: 0x04003242 RID: 12866
		public const string Any = "*";

		// Token: 0x04003243 RID: 12867
		public const string Local = ".";

		// Token: 0x04003244 RID: 12868
		private static string computerName;

		// Token: 0x04003245 RID: 12869
		private string[] tagNames;

		// Token: 0x04003246 RID: 12870
		private Type permissionAccessType;

		// Token: 0x04003247 RID: 12871
		private bool isUnrestricted;

		// Token: 0x04003248 RID: 12872
		private Hashtable rootTable = ResourcePermissionBase.CreateHashtable();

		// Token: 0x0200073D RID: 1853
		[SuppressUnmanagedCodeSecurity]
		private static class UnsafeNativeMethods
		{
			// Token: 0x06003885 RID: 14469
			[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
			internal static extern bool GetComputerName(StringBuilder lpBuffer, ref int nSize);
		}
	}
}
