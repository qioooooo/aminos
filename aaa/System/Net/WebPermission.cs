using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace System.Net
{
	// Token: 0x020004AA RID: 1194
	[Serializable]
	public sealed class WebPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06002487 RID: 9351 RVA: 0x0008F994 File Offset: 0x0008E994
		internal static Regex MatchAllRegex
		{
			get
			{
				if (WebPermission.s_MatchAllRegex == null)
				{
					WebPermission.s_MatchAllRegex = new Regex(".*");
				}
				return WebPermission.s_MatchAllRegex;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06002488 RID: 9352 RVA: 0x0008F9B4 File Offset: 0x0008E9B4
		public IEnumerator ConnectList
		{
			get
			{
				if (this.m_UnrestrictedConnect)
				{
					return new Regex[] { WebPermission.MatchAllRegex }.GetEnumerator();
				}
				ArrayList arrayList = new ArrayList(this.m_connectList.Count);
				for (int i = 0; i < this.m_connectList.Count; i++)
				{
					arrayList.Add((this.m_connectList[i] is DelayedRegex) ? ((DelayedRegex)this.m_connectList[i]).AsRegex : ((this.m_connectList[i] is Uri) ? ((Uri)this.m_connectList[i]).GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped) : this.m_connectList[i]));
				}
				return arrayList.GetEnumerator();
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06002489 RID: 9353 RVA: 0x0008FA78 File Offset: 0x0008EA78
		public IEnumerator AcceptList
		{
			get
			{
				if (this.m_UnrestrictedAccept)
				{
					return new Regex[] { WebPermission.MatchAllRegex }.GetEnumerator();
				}
				ArrayList arrayList = new ArrayList(this.m_acceptList.Count);
				for (int i = 0; i < this.m_acceptList.Count; i++)
				{
					arrayList.Add((this.m_acceptList[i] is DelayedRegex) ? ((DelayedRegex)this.m_acceptList[i]).AsRegex : ((this.m_acceptList[i] is Uri) ? ((Uri)this.m_acceptList[i]).GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped) : this.m_acceptList[i]));
				}
				return arrayList.GetEnumerator();
			}
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x0008FB3C File Offset: 0x0008EB3C
		public WebPermission(PermissionState state)
		{
			this.m_noRestriction = state == PermissionState.Unrestricted;
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x0008FB64 File Offset: 0x0008EB64
		internal WebPermission(bool unrestricted)
		{
			this.m_noRestriction = unrestricted;
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x0008FB89 File Offset: 0x0008EB89
		public WebPermission()
		{
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x0008FBA8 File Offset: 0x0008EBA8
		internal WebPermission(NetworkAccess access)
		{
			this.m_UnrestrictedConnect = (access & NetworkAccess.Connect) != (NetworkAccess)0;
			this.m_UnrestrictedAccept = (access & NetworkAccess.Accept) != (NetworkAccess)0;
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x0008FBF4 File Offset: 0x0008EBF4
		public WebPermission(NetworkAccess access, Regex uriRegex)
		{
			this.AddPermission(access, uriRegex);
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x0008FC1A File Offset: 0x0008EC1A
		public WebPermission(NetworkAccess access, string uriString)
		{
			this.AddPermission(access, uriString);
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x0008FC40 File Offset: 0x0008EC40
		internal WebPermission(NetworkAccess access, Uri uri)
		{
			this.AddPermission(access, uri);
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x0008FC68 File Offset: 0x0008EC68
		public void AddPermission(NetworkAccess access, string uriString)
		{
			if (uriString == null)
			{
				throw new ArgumentNullException("uriString");
			}
			if (this.m_noRestriction)
			{
				return;
			}
			Uri uri;
			if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
			{
				this.AddPermission(access, uri);
				return;
			}
			ArrayList arrayList = new ArrayList();
			if ((access & NetworkAccess.Connect) != (NetworkAccess)0 && !this.m_UnrestrictedConnect)
			{
				arrayList.Add(this.m_connectList);
			}
			if ((access & NetworkAccess.Accept) != (NetworkAccess)0 && !this.m_UnrestrictedAccept)
			{
				arrayList.Add(this.m_acceptList);
			}
			foreach (object obj in arrayList)
			{
				ArrayList arrayList2 = (ArrayList)obj;
				bool flag = false;
				foreach (object obj2 in arrayList2)
				{
					string text = obj2 as string;
					if (text != null && string.Compare(text, uriString, StringComparison.OrdinalIgnoreCase) == 0)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					arrayList2.Add(uriString);
				}
			}
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x0008FD90 File Offset: 0x0008ED90
		internal void AddPermission(NetworkAccess access, Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (this.m_noRestriction)
			{
				return;
			}
			ArrayList arrayList = new ArrayList();
			if ((access & NetworkAccess.Connect) != (NetworkAccess)0 && !this.m_UnrestrictedConnect)
			{
				arrayList.Add(this.m_connectList);
			}
			if ((access & NetworkAccess.Accept) != (NetworkAccess)0 && !this.m_UnrestrictedAccept)
			{
				arrayList.Add(this.m_acceptList);
			}
			foreach (object obj in arrayList)
			{
				ArrayList arrayList2 = (ArrayList)obj;
				bool flag = false;
				foreach (object obj2 in arrayList2)
				{
					if (obj2 is Uri && uri.Equals(obj2))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					arrayList2.Add(uri);
				}
			}
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x0008FEA0 File Offset: 0x0008EEA0
		public void AddPermission(NetworkAccess access, Regex uriRegex)
		{
			if (uriRegex == null)
			{
				throw new ArgumentNullException("uriRegex");
			}
			if (this.m_noRestriction)
			{
				return;
			}
			if (uriRegex.ToString() == ".*")
			{
				if (!this.m_UnrestrictedConnect && (access & NetworkAccess.Connect) != (NetworkAccess)0)
				{
					this.m_UnrestrictedConnect = true;
					this.m_connectList.Clear();
				}
				if (!this.m_UnrestrictedAccept && (access & NetworkAccess.Accept) != (NetworkAccess)0)
				{
					this.m_UnrestrictedAccept = true;
					this.m_acceptList.Clear();
				}
				return;
			}
			this.AddAsPattern(access, new DelayedRegex(uriRegex));
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x0008FF28 File Offset: 0x0008EF28
		internal void AddAsPattern(NetworkAccess access, DelayedRegex uriRegexPattern)
		{
			ArrayList arrayList = new ArrayList();
			if ((access & NetworkAccess.Connect) != (NetworkAccess)0 && !this.m_UnrestrictedConnect)
			{
				arrayList.Add(this.m_connectList);
			}
			if ((access & NetworkAccess.Accept) != (NetworkAccess)0 && !this.m_UnrestrictedAccept)
			{
				arrayList.Add(this.m_acceptList);
			}
			foreach (object obj in arrayList)
			{
				ArrayList arrayList2 = (ArrayList)obj;
				bool flag = false;
				foreach (object obj2 in arrayList2)
				{
					if (obj2 is DelayedRegex && string.Compare(uriRegexPattern.ToString(), obj2.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					arrayList2.Add(uriRegexPattern);
				}
			}
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x00090028 File Offset: 0x0008F028
		public bool IsUnrestricted()
		{
			return this.m_noRestriction;
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x00090030 File Offset: 0x0008F030
		public override IPermission Copy()
		{
			if (this.m_noRestriction)
			{
				return new WebPermission(true);
			}
			return new WebPermission((this.m_UnrestrictedConnect ? NetworkAccess.Connect : ((NetworkAccess)0)) | (this.m_UnrestrictedAccept ? NetworkAccess.Accept : ((NetworkAccess)0)))
			{
				m_acceptList = (ArrayList)this.m_acceptList.Clone(),
				m_connectList = (ArrayList)this.m_connectList.Clone()
			};
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000900A0 File Offset: 0x0008F0A0
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return !this.m_noRestriction && !this.m_UnrestrictedConnect && !this.m_UnrestrictedAccept && this.m_connectList.Count == 0 && this.m_acceptList.Count == 0;
			}
			WebPermission webPermission = target as WebPermission;
			if (webPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (webPermission.m_noRestriction)
			{
				return true;
			}
			if (this.m_noRestriction)
			{
				return false;
			}
			if (!webPermission.m_UnrestrictedAccept)
			{
				if (this.m_UnrestrictedAccept)
				{
					return false;
				}
				if (this.m_acceptList.Count != 0)
				{
					if (webPermission.m_acceptList.Count == 0)
					{
						return false;
					}
					foreach (object obj in this.m_acceptList)
					{
						DelayedRegex delayedRegex = obj as DelayedRegex;
						if (delayedRegex != null)
						{
							if (!WebPermission.isSpecialSubsetCase(obj.ToString(), webPermission.m_acceptList))
							{
								throw new NotSupportedException(SR.GetString("net_perm_both_regex"));
							}
						}
						else if (!WebPermission.isMatchedURI(obj, webPermission.m_acceptList))
						{
							return false;
						}
					}
				}
			}
			if (!webPermission.m_UnrestrictedConnect)
			{
				if (this.m_UnrestrictedConnect)
				{
					return false;
				}
				if (this.m_connectList.Count != 0)
				{
					if (webPermission.m_connectList.Count == 0)
					{
						return false;
					}
					foreach (object obj2 in this.m_connectList)
					{
						DelayedRegex delayedRegex = obj2 as DelayedRegex;
						if (delayedRegex != null)
						{
							if (!WebPermission.isSpecialSubsetCase(obj2.ToString(), webPermission.m_connectList))
							{
								throw new NotSupportedException(SR.GetString("net_perm_both_regex"));
							}
						}
						else if (!WebPermission.isMatchedURI(obj2, webPermission.m_connectList))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x00090294 File Offset: 0x0008F294
		private static bool isSpecialSubsetCase(string regexToCheck, ArrayList permList)
		{
			foreach (object obj in permList)
			{
				DelayedRegex delayedRegex = obj as DelayedRegex;
				Uri uri;
				if (delayedRegex != null)
				{
					if (string.Compare(regexToCheck, delayedRegex.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
					{
						return true;
					}
				}
				else if ((uri = obj as Uri) != null)
				{
					if (string.Compare(regexToCheck, Regex.Escape(uri.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped)), StringComparison.OrdinalIgnoreCase) == 0)
					{
						return true;
					}
				}
				else if (string.Compare(regexToCheck, Regex.Escape(obj.ToString()), StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x00090348 File Offset: 0x0008F348
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			WebPermission webPermission = target as WebPermission;
			if (webPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (this.m_noRestriction || webPermission.m_noRestriction)
			{
				return new WebPermission(true);
			}
			WebPermission webPermission2 = new WebPermission();
			if (this.m_UnrestrictedConnect || webPermission.m_UnrestrictedConnect)
			{
				webPermission2.m_UnrestrictedConnect = true;
			}
			else
			{
				webPermission2.m_connectList = (ArrayList)webPermission.m_connectList.Clone();
				for (int i = 0; i < this.m_connectList.Count; i++)
				{
					DelayedRegex delayedRegex = this.m_connectList[i] as DelayedRegex;
					if (delayedRegex == null)
					{
						if (this.m_connectList[i] is string)
						{
							webPermission2.AddPermission(NetworkAccess.Connect, (string)this.m_connectList[i]);
						}
						else
						{
							webPermission2.AddPermission(NetworkAccess.Connect, (Uri)this.m_connectList[i]);
						}
					}
					else
					{
						webPermission2.AddAsPattern(NetworkAccess.Connect, delayedRegex);
					}
				}
			}
			if (this.m_UnrestrictedAccept || webPermission.m_UnrestrictedAccept)
			{
				webPermission2.m_UnrestrictedAccept = true;
			}
			else
			{
				webPermission2.m_acceptList = (ArrayList)webPermission.m_acceptList.Clone();
				for (int j = 0; j < this.m_acceptList.Count; j++)
				{
					DelayedRegex delayedRegex2 = this.m_acceptList[j] as DelayedRegex;
					if (delayedRegex2 == null)
					{
						if (this.m_acceptList[j] is string)
						{
							webPermission2.AddPermission(NetworkAccess.Accept, (string)this.m_acceptList[j]);
						}
						else
						{
							webPermission2.AddPermission(NetworkAccess.Accept, (Uri)this.m_acceptList[j]);
						}
					}
					else
					{
						webPermission2.AddAsPattern(NetworkAccess.Accept, delayedRegex2);
					}
				}
			}
			return webPermission2;
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x00090514 File Offset: 0x0008F514
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			WebPermission webPermission = target as WebPermission;
			if (webPermission == null)
			{
				throw new ArgumentException(SR.GetString("net_perm_target"), "target");
			}
			if (this.m_noRestriction)
			{
				return webPermission.Copy();
			}
			if (webPermission.m_noRestriction)
			{
				return this.Copy();
			}
			WebPermission webPermission2 = new WebPermission();
			if (this.m_UnrestrictedConnect && webPermission.m_UnrestrictedConnect)
			{
				webPermission2.m_UnrestrictedConnect = true;
			}
			else if (this.m_UnrestrictedConnect || webPermission.m_UnrestrictedConnect)
			{
				webPermission2.m_connectList = (ArrayList)(this.m_UnrestrictedConnect ? webPermission : this).m_connectList.Clone();
			}
			else
			{
				WebPermission.intersectList(this.m_connectList, webPermission.m_connectList, webPermission2.m_connectList);
			}
			if (this.m_UnrestrictedAccept && webPermission.m_UnrestrictedAccept)
			{
				webPermission2.m_UnrestrictedAccept = true;
			}
			else if (this.m_UnrestrictedAccept || webPermission.m_UnrestrictedAccept)
			{
				webPermission2.m_acceptList = (ArrayList)(this.m_UnrestrictedAccept ? webPermission : this).m_acceptList.Clone();
			}
			else
			{
				WebPermission.intersectList(this.m_acceptList, webPermission.m_acceptList, webPermission2.m_acceptList);
			}
			if (!webPermission2.m_UnrestrictedConnect && !webPermission2.m_UnrestrictedAccept && webPermission2.m_connectList.Count == 0 && webPermission2.m_acceptList.Count == 0)
			{
				return null;
			}
			return webPermission2;
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x0009065C File Offset: 0x0008F65C
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			if (!securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("net_not_ipermission"), "securityElement");
			}
			string text = securityElement.Attribute("class");
			if (text == null)
			{
				throw new ArgumentException(SR.GetString("net_no_classname"), "securityElement");
			}
			if (text.IndexOf(base.GetType().FullName) < 0)
			{
				throw new ArgumentException(SR.GetString("net_no_typename"), "securityElement");
			}
			string text2 = securityElement.Attribute("Unrestricted");
			this.m_connectList = new ArrayList();
			this.m_acceptList = new ArrayList();
			this.m_UnrestrictedAccept = (this.m_UnrestrictedConnect = false);
			if (text2 != null && string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.m_noRestriction = true;
				return;
			}
			this.m_noRestriction = false;
			SecurityElement securityElement2 = securityElement.SearchForChildByTag("ConnectAccess");
			if (securityElement2 != null)
			{
				foreach (object obj in securityElement2.Children)
				{
					SecurityElement securityElement3 = (SecurityElement)obj;
					if (securityElement3.Tag.Equals("URI"))
					{
						string text3;
						try
						{
							text3 = securityElement3.Attribute("uri");
						}
						catch
						{
							text3 = null;
						}
						if (text3 == null)
						{
							throw new ArgumentException(SR.GetString("net_perm_invalid_val_in_element"), "ConnectAccess");
						}
						if (text3 == ".*")
						{
							this.m_UnrestrictedConnect = true;
							this.m_connectList = new ArrayList();
							break;
						}
						this.AddAsPattern(NetworkAccess.Connect, new DelayedRegex(text3));
					}
				}
			}
			securityElement2 = securityElement.SearchForChildByTag("AcceptAccess");
			if (securityElement2 != null)
			{
				foreach (object obj2 in securityElement2.Children)
				{
					SecurityElement securityElement4 = (SecurityElement)obj2;
					if (securityElement4.Tag.Equals("URI"))
					{
						string text3;
						try
						{
							text3 = securityElement4.Attribute("uri");
						}
						catch
						{
							text3 = null;
						}
						if (text3 == null)
						{
							throw new ArgumentException(SR.GetString("net_perm_invalid_val_in_element"), "AcceptAccess");
						}
						if (text3 == ".*")
						{
							this.m_UnrestrictedAccept = true;
							this.m_acceptList = new ArrayList();
							break;
						}
						this.AddAsPattern(NetworkAccess.Accept, new DelayedRegex(text3));
					}
				}
			}
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000908F8 File Offset: 0x0008F8F8
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (!this.IsUnrestricted())
			{
				if (this.m_UnrestrictedConnect || this.m_connectList.Count > 0)
				{
					SecurityElement securityElement2 = new SecurityElement("ConnectAccess");
					if (this.m_UnrestrictedConnect)
					{
						SecurityElement securityElement3 = new SecurityElement("URI");
						securityElement3.AddAttribute("uri", SecurityElement.Escape(".*"));
						securityElement2.AddChild(securityElement3);
					}
					else
					{
						foreach (object obj in this.m_connectList)
						{
							Uri uri = obj as Uri;
							string text;
							if (uri != null)
							{
								text = Regex.Escape(uri.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped));
							}
							else
							{
								text = obj.ToString();
							}
							if (obj is string)
							{
								text = Regex.Escape(text);
							}
							SecurityElement securityElement4 = new SecurityElement("URI");
							securityElement4.AddAttribute("uri", SecurityElement.Escape(text));
							securityElement2.AddChild(securityElement4);
						}
					}
					securityElement.AddChild(securityElement2);
				}
				if (this.m_UnrestrictedAccept || this.m_acceptList.Count > 0)
				{
					SecurityElement securityElement5 = new SecurityElement("AcceptAccess");
					if (this.m_UnrestrictedAccept)
					{
						SecurityElement securityElement6 = new SecurityElement("URI");
						securityElement6.AddAttribute("uri", SecurityElement.Escape(".*"));
						securityElement5.AddChild(securityElement6);
					}
					else
					{
						foreach (object obj2 in this.m_acceptList)
						{
							Uri uri2 = obj2 as Uri;
							string text;
							if (uri2 != null)
							{
								text = Regex.Escape(uri2.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped));
							}
							else
							{
								text = obj2.ToString();
							}
							if (obj2 is string)
							{
								text = Regex.Escape(text);
							}
							SecurityElement securityElement7 = new SecurityElement("URI");
							securityElement7.AddAttribute("uri", SecurityElement.Escape(text));
							securityElement5.AddChild(securityElement7);
						}
					}
					securityElement.AddChild(securityElement5);
				}
			}
			else
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x00090B98 File Offset: 0x0008FB98
		private static bool isMatchedURI(object uriToCheck, ArrayList uriPatternList)
		{
			string text = uriToCheck as string;
			foreach (object obj in uriPatternList)
			{
				DelayedRegex delayedRegex = obj as DelayedRegex;
				if (delayedRegex == null)
				{
					if (uriToCheck.GetType() == obj.GetType())
					{
						if (text != null && string.Compare(text, (string)obj, StringComparison.OrdinalIgnoreCase) == 0)
						{
							return true;
						}
						if (text == null && uriToCheck.Equals(obj))
						{
							return true;
						}
					}
				}
				else
				{
					string text2 = ((text != null) ? text : ((Uri)uriToCheck).GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped));
					Match match = delayedRegex.AsRegex.Match(text2);
					if (match != null && match.Index == 0 && match.Length == text2.Length)
					{
						return true;
					}
					if (text == null)
					{
						text2 = ((Uri)uriToCheck).GetComponents(UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped);
						match = delayedRegex.AsRegex.Match(text2);
						if (match != null && match.Index == 0 && match.Length == text2.Length)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x00090CCC File Offset: 0x0008FCCC
		private static void intersectList(ArrayList A, ArrayList B, ArrayList result)
		{
			bool[] array = new bool[A.Count];
			bool[] array2 = new bool[B.Count];
			int num = 0;
			foreach (object obj in A)
			{
				int num2 = 0;
				foreach (object obj2 in B)
				{
					if (!array2[num2] && obj.GetType() == obj2.GetType())
					{
						if (obj is Uri)
						{
							if (obj.Equals(obj2))
							{
								result.Add(obj);
								array[num] = (array2[num2] = true);
								break;
							}
						}
						else if (string.Compare(obj.ToString(), obj2.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
						{
							result.Add(obj);
							array[num] = (array2[num2] = true);
							break;
						}
					}
					num2++;
				}
				num++;
			}
			num = 0;
			foreach (object obj3 in A)
			{
				if (!array[num])
				{
					int num2 = 0;
					foreach (object obj4 in B)
					{
						if (!array2[num2])
						{
							bool flag;
							object obj5 = WebPermission.intersectPair(obj3, obj4, out flag);
							if (obj5 != null)
							{
								bool flag2 = false;
								foreach (object obj6 in result)
								{
									if (flag == obj6 is Uri && (flag ? obj5.Equals(obj6) : (string.Compare(obj6.ToString(), obj5.ToString(), StringComparison.OrdinalIgnoreCase) == 0)))
									{
										flag2 = true;
										break;
									}
								}
								if (!flag2)
								{
									result.Add(obj5);
								}
							}
						}
						num2++;
					}
				}
				num++;
			}
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x00090F38 File Offset: 0x0008FF38
		private static object intersectPair(object L, object R, out bool isUri)
		{
			isUri = false;
			DelayedRegex delayedRegex = L as DelayedRegex;
			DelayedRegex delayedRegex2 = R as DelayedRegex;
			if (delayedRegex != null && delayedRegex2 != null)
			{
				return new DelayedRegex(string.Concat(new string[]
				{
					"(?=(",
					delayedRegex.ToString(),
					"))(",
					delayedRegex2.ToString(),
					")"
				}));
			}
			if (delayedRegex != null && delayedRegex2 == null)
			{
				isUri = R is Uri;
				string text = (isUri ? ((Uri)R).GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped) : R.ToString());
				Match match = delayedRegex.AsRegex.Match(text);
				if (match != null && match.Index == 0 && match.Length == text.Length)
				{
					return R;
				}
				return null;
			}
			else if (delayedRegex == null && delayedRegex2 != null)
			{
				isUri = L is Uri;
				string text2 = (isUri ? ((Uri)L).GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped) : L.ToString());
				Match match2 = delayedRegex2.AsRegex.Match(text2);
				if (match2 != null && match2.Index == 0 && match2.Length == text2.Length)
				{
					return L;
				}
				return null;
			}
			else
			{
				isUri = L is Uri;
				if (isUri)
				{
					if (!L.Equals(R))
					{
						return null;
					}
					return L;
				}
				else
				{
					if (string.Compare(L.ToString(), R.ToString(), StringComparison.OrdinalIgnoreCase) != 0)
					{
						return null;
					}
					return L;
				}
			}
		}

		// Token: 0x040024C1 RID: 9409
		internal const string MatchAll = ".*";

		// Token: 0x040024C2 RID: 9410
		private bool m_noRestriction;

		// Token: 0x040024C3 RID: 9411
		[OptionalField]
		private bool m_UnrestrictedConnect;

		// Token: 0x040024C4 RID: 9412
		[OptionalField]
		private bool m_UnrestrictedAccept;

		// Token: 0x040024C5 RID: 9413
		private ArrayList m_connectList = new ArrayList();

		// Token: 0x040024C6 RID: 9414
		private ArrayList m_acceptList = new ArrayList();

		// Token: 0x040024C7 RID: 9415
		private static Regex s_MatchAllRegex;
	}
}
