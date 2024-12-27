using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x0200038D RID: 909
	internal abstract class DependencyParser : BaseParser
	{
		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06002C4D RID: 11341 RVA: 0x000C5DBC File Offset: 0x000C4DBC
		protected PagesSection PagesConfig
		{
			get
			{
				return this._pagesConfig;
			}
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x000C5DC4 File Offset: 0x000C4DC4
		internal void Init(VirtualPath virtualPath)
		{
			base.CurrentVirtualPath = virtualPath;
			this._virtualPath = virtualPath;
			this._pagesConfig = RuntimeConfig.GetConfig(virtualPath).Pages;
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x000C5DE8 File Offset: 0x000C4DE8
		internal ICollection GetVirtualPathDependencies()
		{
			Thread currentThread = Thread.CurrentThread;
			CultureInfo currentCulture = currentThread.CurrentCulture;
			currentThread.CurrentCulture = CultureInfo.InvariantCulture;
			try
			{
				try
				{
					this.PrepareParse();
					this.ParseFile();
				}
				finally
				{
					currentThread.CurrentCulture = currentCulture;
				}
			}
			catch
			{
				throw;
			}
			return this._virtualPathDependencies;
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x000C5E4C File Offset: 0x000C4E4C
		protected void AddDependency(VirtualPath virtualPath)
		{
			virtualPath = base.ResolveVirtualPath(virtualPath);
			if (this._virtualPathDependencies == null)
			{
				this._virtualPathDependencies = new CaseInsensitiveStringSet();
			}
			this._virtualPathDependencies.Add(virtualPath.VirtualPathString);
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06002C51 RID: 11345
		internal abstract string DefaultDirectiveName { get; }

		// Token: 0x06002C52 RID: 11346 RVA: 0x000C5E7B File Offset: 0x000C4E7B
		protected virtual void PrepareParse()
		{
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x000C5E7D File Offset: 0x000C4E7D
		private void ParseFile()
		{
			this.ParseFile(null, this._virtualPath);
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x000C5E8C File Offset: 0x000C4E8C
		private void ParseFile(string physicalPath, VirtualPath virtualPath)
		{
			string text = ((physicalPath != null) ? physicalPath : virtualPath.VirtualPathString);
			if (this._circularReferenceChecker.Contains(text))
			{
				throw new HttpException(SR.GetString("Circular_include"));
			}
			this._circularReferenceChecker.Add(text);
			try
			{
				if (physicalPath != null)
				{
					TextReader textReader2;
					TextReader textReader = (textReader2 = Util.ReaderFromFile(physicalPath, virtualPath));
					try
					{
						this.ParseReader(textReader);
						goto IL_0079;
					}
					finally
					{
						if (textReader2 != null)
						{
							((IDisposable)textReader2).Dispose();
						}
					}
				}
				using (Stream stream = virtualPath.OpenFile())
				{
					TextReader textReader = Util.ReaderFromStream(stream, virtualPath);
					this.ParseReader(textReader);
				}
				IL_0079:;
			}
			finally
			{
				this._circularReferenceChecker.Remove(text);
			}
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x000C5F4C File Offset: 0x000C4F4C
		private void ParseReader(TextReader input)
		{
			this.ParseString(input.ReadToEnd());
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x000C5F5C File Offset: 0x000C4F5C
		private void ParseString(string text)
		{
			int num = 0;
			for (;;)
			{
				Match match;
				if ((match = BaseParser.textRegex.Match(text, num)).Success)
				{
					num = match.Index + match.Length;
				}
				if (num == text.Length)
				{
					break;
				}
				if ((match = BaseParser.DirectiveRegex.Match(text, num)).Success)
				{
					IDictionary dictionary = CollectionsUtil.CreateCaseInsensitiveSortedList();
					string text2 = this.ProcessAttributes(match, dictionary);
					this.ProcessDirective(text2, dictionary);
					num = match.Index + match.Length;
				}
				else if ((match = BaseParser.includeRegex.Match(text, num)).Success)
				{
					this.ProcessServerInclude(match);
					num = match.Index + match.Length;
				}
				else if ((match = BaseParser.commentRegex.Match(text, num)).Success)
				{
					num = match.Index + match.Length;
				}
				else
				{
					int num2 = text.IndexOf("<%@", num, StringComparison.Ordinal);
					if (num2 == -1 || num2 == num)
					{
						return;
					}
					num = num2;
				}
				if (num == text.Length)
				{
					return;
				}
			}
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x000C6050 File Offset: 0x000C5050
		private void ProcessServerInclude(Match match)
		{
			string value = match.Groups["pathtype"].Value;
			string value2 = match.Groups["filename"].Value;
			if (value2.Length == 0)
			{
				return;
			}
			string text = null;
			VirtualPath virtualPath;
			if (StringUtil.EqualsIgnoreCase(value, "file"))
			{
				if (UrlPath.IsAbsolutePhysicalPath(value2))
				{
					text = value2;
					virtualPath = base.CurrentVirtualPath;
				}
				else
				{
					virtualPath = base.ResolveVirtualPath(VirtualPath.Create(value2));
				}
			}
			else
			{
				if (!StringUtil.EqualsIgnoreCase(value, "virtual"))
				{
					return;
				}
				virtualPath = base.ResolveVirtualPath(VirtualPath.Create(value2));
			}
			VirtualPath virtualPath2 = this._virtualPath;
			try
			{
				this._virtualPath = virtualPath;
				this.ParseFile(text, virtualPath);
			}
			finally
			{
				this._virtualPath = virtualPath2;
			}
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x000C6114 File Offset: 0x000C5114
		internal virtual void ProcessDirective(string directiveName, IDictionary directive)
		{
			if (directiveName == null || StringUtil.EqualsIgnoreCase(directiveName, this.DefaultDirectiveName))
			{
				this.ProcessMainDirective(directive);
				return;
			}
			if (StringUtil.EqualsIgnoreCase(directiveName, "register"))
			{
				VirtualPath andRemoveVirtualPathAttribute = Util.GetAndRemoveVirtualPathAttribute(directive, "src");
				if (andRemoveVirtualPathAttribute != null)
				{
					this.AddDependency(andRemoveVirtualPathAttribute);
					return;
				}
			}
			else if (StringUtil.EqualsIgnoreCase(directiveName, "reference"))
			{
				VirtualPath andRemoveVirtualPathAttribute2 = Util.GetAndRemoveVirtualPathAttribute(directive, "virtualpath");
				if (andRemoveVirtualPathAttribute2 != null)
				{
					this.AddDependency(andRemoveVirtualPathAttribute2);
				}
				VirtualPath andRemoveVirtualPathAttribute3 = Util.GetAndRemoveVirtualPathAttribute(directive, "page");
				if (andRemoveVirtualPathAttribute3 != null)
				{
					this.AddDependency(andRemoveVirtualPathAttribute3);
				}
				VirtualPath andRemoveVirtualPathAttribute4 = Util.GetAndRemoveVirtualPathAttribute(directive, "control");
				if (andRemoveVirtualPathAttribute4 != null)
				{
					this.AddDependency(andRemoveVirtualPathAttribute4);
					return;
				}
			}
			else if (StringUtil.EqualsIgnoreCase(directiveName, "assembly"))
			{
				VirtualPath andRemoveVirtualPathAttribute5 = Util.GetAndRemoveVirtualPathAttribute(directive, "src");
				if (andRemoveVirtualPathAttribute5 != null)
				{
					this.AddDependency(andRemoveVirtualPathAttribute5);
				}
			}
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x000C61F8 File Offset: 0x000C51F8
		private void ProcessMainDirective(IDictionary mainDirective)
		{
			foreach (object obj in mainDirective)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = ((string)dictionaryEntry.Key).ToLower(CultureInfo.InvariantCulture);
				string text3;
				string text2 = Util.ParsePropertyDeviceFilter(text, out text3);
				this.ProcessMainDirectiveAttribute(text2, text3, (string)dictionaryEntry.Value);
			}
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x000C6280 File Offset: 0x000C5280
		internal virtual void ProcessMainDirectiveAttribute(string deviceName, string name, string value)
		{
			if (name == "src")
			{
				string nonEmptyAttribute = Util.GetNonEmptyAttribute(name, value);
				this.AddDependency(VirtualPath.Create(nonEmptyAttribute));
			}
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x000C62B0 File Offset: 0x000C52B0
		private string ProcessAttributes(Match match, IDictionary attribs)
		{
			string text = null;
			CaptureCollection captures = match.Groups["attrname"].Captures;
			CaptureCollection captures2 = match.Groups["attrval"].Captures;
			CaptureCollection captures3 = match.Groups["equal"].Captures;
			for (int i = 0; i < captures.Count; i++)
			{
				string text2 = captures[i].ToString();
				string text3 = captures2[i].ToString();
				bool flag = captures3[i].ToString().Length > 0;
				if (text2 != null && !flag && text == null)
				{
					text = text2;
				}
				else
				{
					try
					{
						if (attribs != null)
						{
							attribs.Add(text2, text3);
						}
					}
					catch (ArgumentException)
					{
					}
				}
			}
			return text;
		}

		// Token: 0x0400208B RID: 8331
		private VirtualPath _virtualPath;

		// Token: 0x0400208C RID: 8332
		private StringSet _virtualPathDependencies;

		// Token: 0x0400208D RID: 8333
		private StringSet _circularReferenceChecker = new CaseInsensitiveStringSet();

		// Token: 0x0400208E RID: 8334
		private PagesSection _pagesConfig;
	}
}
