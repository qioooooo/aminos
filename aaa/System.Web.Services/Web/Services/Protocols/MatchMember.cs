using System;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000051 RID: 81
	internal class MatchMember
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x000076A4 File Offset: 0x000066A4
		internal void Match(object target, string text)
		{
			if (this.memberInfo is FieldInfo)
			{
				((FieldInfo)this.memberInfo).SetValue(target, (this.matchType == null) ? this.MatchString(text) : this.MatchClass(text));
				return;
			}
			if (this.memberInfo is PropertyInfo)
			{
				((PropertyInfo)this.memberInfo).SetValue(target, (this.matchType == null) ? this.MatchString(text) : this.MatchClass(text), new object[0]);
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00007724 File Offset: 0x00006724
		private object MatchString(string text)
		{
			Match match = this.regex.Match(text);
			Type type = ((this.memberInfo is FieldInfo) ? ((FieldInfo)this.memberInfo).FieldType : ((PropertyInfo)this.memberInfo).PropertyType);
			if (type.IsArray)
			{
				ArrayList arrayList = new ArrayList();
				int num = 0;
				while (match.Success && num < this.maxRepeats)
				{
					if (match.Groups.Count <= this.group)
					{
						throw MatchMember.BadGroupIndexException(this.group, this.memberInfo.Name, match.Groups.Count - 1);
					}
					Group group = match.Groups[this.group];
					foreach (object obj in group.Captures)
					{
						Capture capture = (Capture)obj;
						arrayList.Add(text.Substring(capture.Index, capture.Length));
					}
					match = match.NextMatch();
					num++;
				}
				return arrayList.ToArray(typeof(string));
			}
			if (match.Success)
			{
				if (match.Groups.Count <= this.group)
				{
					throw MatchMember.BadGroupIndexException(this.group, this.memberInfo.Name, match.Groups.Count - 1);
				}
				Group group2 = match.Groups[this.group];
				if (group2.Captures.Count > 0)
				{
					if (group2.Captures.Count <= this.capture)
					{
						throw MatchMember.BadCaptureIndexException(this.capture, this.memberInfo.Name, group2.Captures.Count - 1);
					}
					Capture capture2 = group2.Captures[this.capture];
					return text.Substring(capture2.Index, capture2.Length);
				}
			}
			return null;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00007930 File Offset: 0x00006930
		private object MatchClass(string text)
		{
			Match match = this.regex.Match(text);
			Type type = ((this.memberInfo is FieldInfo) ? ((FieldInfo)this.memberInfo).FieldType : ((PropertyInfo)this.memberInfo).PropertyType);
			if (type.IsArray)
			{
				ArrayList arrayList = new ArrayList();
				int num = 0;
				while (match.Success && num < this.maxRepeats)
				{
					if (match.Groups.Count <= this.group)
					{
						throw MatchMember.BadGroupIndexException(this.group, this.memberInfo.Name, match.Groups.Count - 1);
					}
					Group group = match.Groups[this.group];
					foreach (object obj in group.Captures)
					{
						Capture capture = (Capture)obj;
						arrayList.Add(this.matchType.Match(text.Substring(capture.Index, capture.Length)));
					}
					match = match.NextMatch();
					num++;
				}
				return arrayList.ToArray(this.matchType.Type);
			}
			if (match.Success)
			{
				if (match.Groups.Count <= this.group)
				{
					throw MatchMember.BadGroupIndexException(this.group, this.memberInfo.Name, match.Groups.Count - 1);
				}
				Group group2 = match.Groups[this.group];
				if (group2.Captures.Count > 0)
				{
					if (group2.Captures.Count <= this.capture)
					{
						throw MatchMember.BadCaptureIndexException(this.capture, this.memberInfo.Name, group2.Captures.Count - 1);
					}
					Capture capture2 = group2.Captures[this.capture];
					return this.matchType.Match(text.Substring(capture2.Index, capture2.Length));
				}
			}
			return null;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00007B54 File Offset: 0x00006B54
		private static Exception BadCaptureIndexException(int index, string matchName, int highestIndex)
		{
			return new Exception(Res.GetString("WebTextMatchBadCaptureIndex", new object[] { index, matchName, highestIndex }));
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00007B90 File Offset: 0x00006B90
		private static Exception BadGroupIndexException(int index, string matchName, int highestIndex)
		{
			return new Exception(Res.GetString("WebTextMatchBadGroupIndex", new object[] { index, matchName, highestIndex }));
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00007BCC File Offset: 0x00006BCC
		internal static MatchMember Reflect(MemberInfo memberInfo)
		{
			Type type = null;
			if (memberInfo is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				if (!propertyInfo.CanRead)
				{
					return null;
				}
				if (!propertyInfo.CanWrite)
				{
					return null;
				}
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				if (getMethod.IsStatic)
				{
					return null;
				}
				ParameterInfo[] parameters = getMethod.GetParameters();
				if (parameters.Length > 0)
				{
					return null;
				}
				type = propertyInfo.PropertyType;
			}
			if (memberInfo is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				if (!fieldInfo.IsPublic)
				{
					return null;
				}
				if (fieldInfo.IsStatic)
				{
					return null;
				}
				if (fieldInfo.IsSpecialName)
				{
					return null;
				}
				type = fieldInfo.FieldType;
			}
			object[] customAttributes = memberInfo.GetCustomAttributes(typeof(MatchAttribute), false);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			MatchAttribute matchAttribute = (MatchAttribute)customAttributes[0];
			MatchMember matchMember = new MatchMember();
			matchMember.regex = new Regex(matchAttribute.Pattern, RegexOptions.Singleline | (matchAttribute.IgnoreCase ? (RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) : RegexOptions.None));
			matchMember.group = matchAttribute.Group;
			matchMember.capture = matchAttribute.Capture;
			matchMember.maxRepeats = matchAttribute.MaxRepeats;
			matchMember.memberInfo = memberInfo;
			if (matchMember.maxRepeats < 0)
			{
				matchMember.maxRepeats = (type.IsArray ? int.MaxValue : 1);
			}
			if (type.IsArray)
			{
				type = type.GetElementType();
			}
			if (type != typeof(string))
			{
				matchMember.matchType = MatchType.Reflect(type);
			}
			return matchMember;
		}

		// Token: 0x040002B2 RID: 690
		private MemberInfo memberInfo;

		// Token: 0x040002B3 RID: 691
		private Regex regex;

		// Token: 0x040002B4 RID: 692
		private int group;

		// Token: 0x040002B5 RID: 693
		private int capture;

		// Token: 0x040002B6 RID: 694
		private int maxRepeats;

		// Token: 0x040002B7 RID: 695
		private MatchType matchType;
	}
}
