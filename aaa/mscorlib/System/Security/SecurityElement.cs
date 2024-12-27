using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Util;
using System.Text;

namespace System.Security
{
	// Token: 0x020005FF RID: 1535
	[ComVisible(true)]
	[Serializable]
	public sealed class SecurityElement : ISecurityElementFactory
	{
		// Token: 0x060037E7 RID: 14311 RVA: 0x000BC502 File Offset: 0x000BB502
		internal SecurityElement()
		{
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x000BC50A File Offset: 0x000BB50A
		SecurityElement ISecurityElementFactory.CreateSecurityElement()
		{
			return this;
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x000BC50D File Offset: 0x000BB50D
		string ISecurityElementFactory.GetTag()
		{
			return this.Tag;
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x000BC515 File Offset: 0x000BB515
		object ISecurityElementFactory.Copy()
		{
			return this.Copy();
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x000BC51D File Offset: 0x000BB51D
		string ISecurityElementFactory.Attribute(string attributeName)
		{
			return this.Attribute(attributeName);
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x000BC526 File Offset: 0x000BB526
		public static SecurityElement FromString(string xml)
		{
			if (xml == null)
			{
				throw new ArgumentNullException("xml");
			}
			return new Parser(xml).GetTopElement();
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x000BC544 File Offset: 0x000BB544
		public SecurityElement(string tag)
		{
			if (tag == null)
			{
				throw new ArgumentNullException("tag");
			}
			if (!SecurityElement.IsValidTag(tag))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidElementTag"), new object[] { tag }));
			}
			this.m_strTag = tag;
			this.m_strText = null;
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x000BC5A4 File Offset: 0x000BB5A4
		public SecurityElement(string tag, string text)
		{
			if (tag == null)
			{
				throw new ArgumentNullException("tag");
			}
			if (!SecurityElement.IsValidTag(tag))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidElementTag"), new object[] { tag }));
			}
			if (text != null && !SecurityElement.IsValidText(text))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidElementText"), new object[] { text }));
			}
			this.m_strTag = tag;
			this.m_strText = text;
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060037EF RID: 14319 RVA: 0x000BC632 File Offset: 0x000BB632
		// (set) Token: 0x060037F0 RID: 14320 RVA: 0x000BC63C File Offset: 0x000BB63C
		public string Tag
		{
			get
			{
				return this.m_strTag;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Tag");
				}
				if (!SecurityElement.IsValidTag(value))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidElementTag"), new object[] { value }));
				}
				this.m_strTag = value;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x060037F1 RID: 14321 RVA: 0x000BC68C File Offset: 0x000BB68C
		// (set) Token: 0x060037F2 RID: 14322 RVA: 0x000BC6FC File Offset: 0x000BB6FC
		public Hashtable Attributes
		{
			get
			{
				if (this.m_lAttributes == null || this.m_lAttributes.Count == 0)
				{
					return null;
				}
				Hashtable hashtable = new Hashtable(this.m_lAttributes.Count);
				int count = this.m_lAttributes.Count;
				for (int i = 0; i < count; i += 2)
				{
					hashtable.Add(this.m_lAttributes[i], this.m_lAttributes[i + 1]);
				}
				return hashtable;
			}
			set
			{
				if (value == null || value.Count == 0)
				{
					this.m_lAttributes = null;
					return;
				}
				ArrayList arrayList = new ArrayList(value.Count);
				IDictionaryEnumerator enumerator = value.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Key;
					string text2 = (string)enumerator.Value;
					if (!SecurityElement.IsValidAttributeName(text))
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidElementName"), new object[] { (string)enumerator.Current }));
					}
					if (!SecurityElement.IsValidAttributeValue(text2))
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidElementValue"), new object[] { (string)enumerator.Value }));
					}
					arrayList.Add(text);
					arrayList.Add(text2);
				}
				this.m_lAttributes = arrayList;
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x060037F3 RID: 14323 RVA: 0x000BC7E4 File Offset: 0x000BB7E4
		// (set) Token: 0x060037F4 RID: 14324 RVA: 0x000BC7F4 File Offset: 0x000BB7F4
		public string Text
		{
			get
			{
				return SecurityElement.Unescape(this.m_strText);
			}
			set
			{
				if (value == null)
				{
					this.m_strText = null;
					return;
				}
				if (!SecurityElement.IsValidText(value))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidElementTag"), new object[] { value }));
				}
				this.m_strText = value;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x060037F5 RID: 14325 RVA: 0x000BC841 File Offset: 0x000BB841
		// (set) Token: 0x060037F6 RID: 14326 RVA: 0x000BC850 File Offset: 0x000BB850
		public ArrayList Children
		{
			get
			{
				this.ConvertSecurityElementFactories();
				return this.m_lChildren;
			}
			set
			{
				if (value != null)
				{
					IEnumerator enumerator = value.GetEnumerator();
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == null)
						{
							throw new ArgumentException(Environment.GetResourceString("ArgumentNull_Child"));
						}
					}
				}
				this.m_lChildren = value;
			}
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x000BC890 File Offset: 0x000BB890
		internal void ConvertSecurityElementFactories()
		{
			if (this.m_lChildren == null)
			{
				return;
			}
			for (int i = 0; i < this.m_lChildren.Count; i++)
			{
				ISecurityElementFactory securityElementFactory = this.m_lChildren[i] as ISecurityElementFactory;
				if (securityElementFactory != null && !(this.m_lChildren[i] is SecurityElement))
				{
					this.m_lChildren[i] = securityElementFactory.CreateSecurityElement();
				}
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x060037F8 RID: 14328 RVA: 0x000BC8F6 File Offset: 0x000BB8F6
		internal ArrayList InternalChildren
		{
			get
			{
				return this.m_lChildren;
			}
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x000BC900 File Offset: 0x000BB900
		internal void AddAttributeSafe(string name, string value)
		{
			if (this.m_lAttributes == null)
			{
				this.m_lAttributes = new ArrayList(8);
			}
			else
			{
				int count = this.m_lAttributes.Count;
				for (int i = 0; i < count; i += 2)
				{
					string text = (string)this.m_lAttributes[i];
					if (string.Equals(text, name))
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_AttributeNamesMustBeUnique"));
					}
				}
			}
			this.m_lAttributes.Add(name);
			this.m_lAttributes.Add(value);
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x000BC980 File Offset: 0x000BB980
		public void AddAttribute(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!SecurityElement.IsValidAttributeName(name))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidElementName"), new object[] { name }));
			}
			if (!SecurityElement.IsValidAttributeValue(value))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidElementValue"), new object[] { value }));
			}
			this.AddAttributeSafe(name, value);
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x000BCA0D File Offset: 0x000BBA0D
		public void AddChild(SecurityElement child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			if (this.m_lChildren == null)
			{
				this.m_lChildren = new ArrayList(1);
			}
			this.m_lChildren.Add(child);
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x000BCA3E File Offset: 0x000BBA3E
		internal void AddChild(ISecurityElementFactory child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			if (this.m_lChildren == null)
			{
				this.m_lChildren = new ArrayList(1);
			}
			this.m_lChildren.Add(child);
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x000BCA70 File Offset: 0x000BBA70
		internal void AddChildNoDuplicates(ISecurityElementFactory child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			if (this.m_lChildren == null)
			{
				this.m_lChildren = new ArrayList(1);
				this.m_lChildren.Add(child);
				return;
			}
			for (int i = 0; i < this.m_lChildren.Count; i++)
			{
				if (this.m_lChildren[i] == child)
				{
					return;
				}
			}
			this.m_lChildren.Add(child);
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x000BCAE0 File Offset: 0x000BBAE0
		public bool Equal(SecurityElement other)
		{
			if (other == null)
			{
				return false;
			}
			if (!string.Equals(this.m_strTag, other.m_strTag))
			{
				return false;
			}
			if (!string.Equals(this.m_strText, other.m_strText))
			{
				return false;
			}
			if (this.m_lAttributes == null || other.m_lAttributes == null)
			{
				if (this.m_lAttributes != other.m_lAttributes)
				{
					return false;
				}
			}
			else
			{
				int count = this.m_lAttributes.Count;
				if (count != other.m_lAttributes.Count)
				{
					return false;
				}
				for (int i = 0; i < count; i++)
				{
					string text = (string)this.m_lAttributes[i];
					string text2 = (string)other.m_lAttributes[i];
					if (!string.Equals(text, text2))
					{
						return false;
					}
				}
			}
			if (this.m_lChildren == null || other.m_lChildren == null)
			{
				if (this.m_lChildren != other.m_lChildren)
				{
					return false;
				}
			}
			else
			{
				if (this.m_lChildren.Count != other.m_lChildren.Count)
				{
					return false;
				}
				this.ConvertSecurityElementFactories();
				other.ConvertSecurityElementFactories();
				IEnumerator enumerator = this.m_lChildren.GetEnumerator();
				IEnumerator enumerator2 = other.m_lChildren.GetEnumerator();
				while (enumerator.MoveNext())
				{
					enumerator2.MoveNext();
					SecurityElement securityElement = (SecurityElement)enumerator.Current;
					SecurityElement securityElement2 = (SecurityElement)enumerator2.Current;
					if (securityElement == null || !securityElement.Equal(securityElement2))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x000BCC38 File Offset: 0x000BBC38
		[ComVisible(false)]
		public SecurityElement Copy()
		{
			return new SecurityElement(this.m_strTag, this.m_strText)
			{
				m_lChildren = ((this.m_lChildren == null) ? null : new ArrayList(this.m_lChildren)),
				m_lAttributes = ((this.m_lAttributes == null) ? null : new ArrayList(this.m_lAttributes))
			};
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x000BCC90 File Offset: 0x000BBC90
		public static bool IsValidTag(string tag)
		{
			return tag != null && tag.IndexOfAny(SecurityElement.s_tagIllegalCharacters) == -1;
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x000BCCA5 File Offset: 0x000BBCA5
		public static bool IsValidText(string text)
		{
			return text != null && text.IndexOfAny(SecurityElement.s_textIllegalCharacters) == -1;
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x000BCCBA File Offset: 0x000BBCBA
		public static bool IsValidAttributeName(string name)
		{
			return SecurityElement.IsValidTag(name);
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x000BCCC2 File Offset: 0x000BBCC2
		public static bool IsValidAttributeValue(string value)
		{
			return value != null && value.IndexOfAny(SecurityElement.s_valueIllegalCharacters) == -1;
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x000BCCD8 File Offset: 0x000BBCD8
		private static string GetEscapeSequence(char c)
		{
			int num = SecurityElement.s_escapeStringPairs.Length;
			for (int i = 0; i < num; i += 2)
			{
				string text = SecurityElement.s_escapeStringPairs[i];
				string text2 = SecurityElement.s_escapeStringPairs[i + 1];
				if (text[0] == c)
				{
					return text2;
				}
			}
			return c.ToString();
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x000BCD20 File Offset: 0x000BBD20
		public static string Escape(string str)
		{
			if (str == null)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			int length = str.Length;
			int num = 0;
			for (;;)
			{
				int num2 = str.IndexOfAny(SecurityElement.s_escapeChars, num);
				if (num2 == -1)
				{
					break;
				}
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder();
				}
				stringBuilder.Append(str, num, num2 - num);
				stringBuilder.Append(SecurityElement.GetEscapeSequence(str[num2]));
				num = num2 + 1;
			}
			if (stringBuilder == null)
			{
				return str;
			}
			stringBuilder.Append(str, num, length - num);
			return stringBuilder.ToString();
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x000BCD94 File Offset: 0x000BBD94
		private static string GetUnescapeSequence(string str, int index, out int newIndex)
		{
			int num = str.Length - index;
			int num2 = SecurityElement.s_escapeStringPairs.Length;
			for (int i = 0; i < num2; i += 2)
			{
				string text = SecurityElement.s_escapeStringPairs[i];
				string text2 = SecurityElement.s_escapeStringPairs[i + 1];
				int length = text2.Length;
				if (length <= num && string.Compare(text2, 0, str, index, length, StringComparison.Ordinal) == 0)
				{
					newIndex = index + text2.Length;
					return text;
				}
			}
			newIndex = index + 1;
			return str[index].ToString();
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x000BCE10 File Offset: 0x000BBE10
		private static string Unescape(string str)
		{
			if (str == null)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			int length = str.Length;
			int num = 0;
			for (;;)
			{
				int num2 = str.IndexOf('&', num);
				if (num2 == -1)
				{
					break;
				}
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder();
				}
				stringBuilder.Append(str, num, num2 - num);
				stringBuilder.Append(SecurityElement.GetUnescapeSequence(str, num2, out num));
			}
			if (stringBuilder == null)
			{
				return str;
			}
			stringBuilder.Append(str, num, length - num);
			return stringBuilder.ToString();
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x000BCE79 File Offset: 0x000BBE79
		private static void ToStringHelperStringBuilder(object obj, string str)
		{
			((StringBuilder)obj).Append(str);
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x000BCE88 File Offset: 0x000BBE88
		private static void ToStringHelperStreamWriter(object obj, string str)
		{
			((StreamWriter)obj).Write(str);
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x000BCE98 File Offset: 0x000BBE98
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.ToString("", stringBuilder, new SecurityElement.ToStringHelperFunc(SecurityElement.ToStringHelperStringBuilder));
			return stringBuilder.ToString();
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x000BCEC9 File Offset: 0x000BBEC9
		internal void ToWriter(StreamWriter writer)
		{
			this.ToString("", writer, new SecurityElement.ToStringHelperFunc(SecurityElement.ToStringHelperStreamWriter));
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x000BCEE4 File Offset: 0x000BBEE4
		private void ToString(string indent, object obj, SecurityElement.ToStringHelperFunc func)
		{
			func(obj, "<");
			switch (this.m_type)
			{
			case SecurityElementType.Format:
				func(obj, "?");
				break;
			case SecurityElementType.Comment:
				func(obj, "!");
				break;
			}
			func(obj, this.m_strTag);
			if (this.m_lAttributes != null && this.m_lAttributes.Count > 0)
			{
				func(obj, " ");
				int count = this.m_lAttributes.Count;
				for (int i = 0; i < count; i += 2)
				{
					string text = (string)this.m_lAttributes[i];
					string text2 = (string)this.m_lAttributes[i + 1];
					func(obj, text);
					func(obj, "=\"");
					func(obj, text2);
					func(obj, "\"");
					if (i != this.m_lAttributes.Count - 2)
					{
						if (this.m_type == SecurityElementType.Regular)
						{
							func(obj, Environment.NewLine);
						}
						else
						{
							func(obj, " ");
						}
					}
				}
			}
			if (this.m_strText == null && (this.m_lChildren == null || this.m_lChildren.Count == 0))
			{
				switch (this.m_type)
				{
				case SecurityElementType.Format:
					func(obj, " ?>");
					break;
				case SecurityElementType.Comment:
					func(obj, ">");
					break;
				default:
					func(obj, "/>");
					break;
				}
				func(obj, Environment.NewLine);
				return;
			}
			func(obj, ">");
			func(obj, this.m_strText);
			if (this.m_lChildren != null)
			{
				this.ConvertSecurityElementFactories();
				func(obj, Environment.NewLine);
				for (int j = 0; j < this.m_lChildren.Count; j++)
				{
					((SecurityElement)this.m_lChildren[j]).ToString("", obj, func);
				}
			}
			func(obj, "</");
			func(obj, this.m_strTag);
			func(obj, ">");
			func(obj, Environment.NewLine);
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x000BD110 File Offset: 0x000BC110
		public string Attribute(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this.m_lAttributes == null)
			{
				return null;
			}
			int count = this.m_lAttributes.Count;
			for (int i = 0; i < count; i += 2)
			{
				string text = (string)this.m_lAttributes[i];
				if (string.Equals(text, name))
				{
					string text2 = (string)this.m_lAttributes[i + 1];
					return SecurityElement.Unescape(text2);
				}
			}
			return null;
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x000BD184 File Offset: 0x000BC184
		public SecurityElement SearchForChildByTag(string tag)
		{
			if (tag == null)
			{
				throw new ArgumentNullException("tag");
			}
			if (this.m_lChildren == null)
			{
				return null;
			}
			foreach (object obj in this.m_lChildren)
			{
				SecurityElement securityElement = (SecurityElement)obj;
				if (securityElement != null && string.Equals(securityElement.Tag, tag))
				{
					return securityElement;
				}
			}
			return null;
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x000BD1E0 File Offset: 0x000BC1E0
		internal IPermission ToPermission(bool ignoreTypeLoadFailures)
		{
			IPermission permission = XMLUtil.CreatePermission(this, PermissionState.None, ignoreTypeLoadFailures);
			if (permission == null)
			{
				return null;
			}
			permission.FromXml(this);
			PermissionToken.GetToken(permission);
			return permission;
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x000BD20C File Offset: 0x000BC20C
		internal object ToSecurityObject()
		{
			string strTag;
			if ((strTag = this.m_strTag) != null && strTag == "PermissionSet")
			{
				PermissionSet permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.FromXml(this);
				return permissionSet;
			}
			return this.ToPermission(false);
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x000BD248 File Offset: 0x000BC248
		internal string SearchForTextOfLocalName(string strLocalName)
		{
			if (strLocalName == null)
			{
				throw new ArgumentNullException("strLocalName");
			}
			if (this.m_strTag == null)
			{
				return null;
			}
			if (this.m_strTag.Equals(strLocalName) || this.m_strTag.EndsWith(":" + strLocalName, StringComparison.Ordinal))
			{
				return SecurityElement.Unescape(this.m_strText);
			}
			if (this.m_lChildren == null)
			{
				return null;
			}
			foreach (object obj in this.m_lChildren)
			{
				string text = ((SecurityElement)obj).SearchForTextOfLocalName(strLocalName);
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x000BD2D8 File Offset: 0x000BC2D8
		public string SearchForTextOfTag(string tag)
		{
			if (tag == null)
			{
				throw new ArgumentNullException("tag");
			}
			if (string.Equals(this.m_strTag, tag))
			{
				return SecurityElement.Unescape(this.m_strText);
			}
			if (this.m_lChildren == null)
			{
				return null;
			}
			IEnumerator enumerator = this.m_lChildren.GetEnumerator();
			this.ConvertSecurityElementFactories();
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				string text = ((SecurityElement)obj).SearchForTextOfTag(tag);
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x04001CD2 RID: 7378
		private const string s_strIndent = "   ";

		// Token: 0x04001CD3 RID: 7379
		private const int c_AttributesTypical = 8;

		// Token: 0x04001CD4 RID: 7380
		private const int c_ChildrenTypical = 1;

		// Token: 0x04001CD5 RID: 7381
		internal string m_strTag;

		// Token: 0x04001CD6 RID: 7382
		internal string m_strText;

		// Token: 0x04001CD7 RID: 7383
		private ArrayList m_lChildren;

		// Token: 0x04001CD8 RID: 7384
		internal ArrayList m_lAttributes;

		// Token: 0x04001CD9 RID: 7385
		internal SecurityElementType m_type;

		// Token: 0x04001CDA RID: 7386
		private static readonly char[] s_tagIllegalCharacters = new char[] { ' ', '<', '>' };

		// Token: 0x04001CDB RID: 7387
		private static readonly char[] s_textIllegalCharacters = new char[] { '<', '>' };

		// Token: 0x04001CDC RID: 7388
		private static readonly char[] s_valueIllegalCharacters = new char[] { '<', '>', '"' };

		// Token: 0x04001CDD RID: 7389
		private static readonly string[] s_escapeStringPairs = new string[] { "<", "&lt;", ">", "&gt;", "\"", "&quot;", "'", "&apos;", "&", "&amp;" };

		// Token: 0x04001CDE RID: 7390
		private static readonly char[] s_escapeChars = new char[] { '<', '>', '"', '\'', '&' };

		// Token: 0x02000600 RID: 1536
		// (Invoke) Token: 0x06003815 RID: 14357
		private delegate void ToStringHelperFunc(object obj, string str);
	}
}
