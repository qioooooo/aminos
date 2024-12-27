using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class Regex : ISerializable
	{
		// Token: 0x0600002B RID: 43 RVA: 0x0000257D File Offset: 0x0000157D
		protected Regex()
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002585 File Offset: 0x00001585
		public Regex(string pattern)
			: this(pattern, RegexOptions.None, false)
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002590 File Offset: 0x00001590
		public Regex(string pattern, RegexOptions options)
			: this(pattern, options, false)
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000259C File Offset: 0x0000159C
		private Regex(string pattern, RegexOptions options, bool useCache)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}
			if (options < RegexOptions.None || options >> 10 != RegexOptions.None)
			{
				throw new ArgumentOutOfRangeException("options");
			}
			if ((options & RegexOptions.ECMAScript) != RegexOptions.None && (options & ~(RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.ECMAScript | RegexOptions.CultureInvariant)) != RegexOptions.None)
			{
				throw new ArgumentOutOfRangeException("options");
			}
			string text;
			if ((options & RegexOptions.CultureInvariant) != RegexOptions.None)
			{
				text = CultureInfo.InvariantCulture.ThreeLetterWindowsLanguageName;
			}
			else
			{
				text = CultureInfo.CurrentCulture.ThreeLetterWindowsLanguageName;
			}
			string[] array = new string[5];
			string[] array2 = array;
			int num = 0;
			int num2 = (int)options;
			array2[num] = num2.ToString(NumberFormatInfo.InvariantInfo);
			array[1] = ":";
			array[2] = text;
			array[3] = ":";
			array[4] = pattern;
			string text2 = string.Concat(array);
			CachedCodeEntry cachedCodeEntry = Regex.LookupCachedAndUpdate(text2);
			this.pattern = pattern;
			this.roptions = options;
			if (cachedCodeEntry == null)
			{
				RegexTree regexTree = RegexParser.Parse(pattern, this.roptions);
				this.capnames = regexTree._capnames;
				this.capslist = regexTree._capslist;
				this.code = RegexWriter.Write(regexTree);
				this.caps = this.code._caps;
				this.capsize = this.code._capsize;
				this.InitializeReferences();
				if (useCache)
				{
					cachedCodeEntry = this.CacheCode(text2);
				}
			}
			else
			{
				this.caps = cachedCodeEntry._caps;
				this.capnames = cachedCodeEntry._capnames;
				this.capslist = cachedCodeEntry._capslist;
				this.capsize = cachedCodeEntry._capsize;
				this.code = cachedCodeEntry._code;
				this.factory = cachedCodeEntry._factory;
				this.runnerref = cachedCodeEntry._runnerref;
				this.replref = cachedCodeEntry._replref;
				this.refsInitialized = true;
			}
			if (this.UseOptionC() && this.factory == null)
			{
				this.factory = this.Compile(this.code, this.roptions);
				if (useCache && cachedCodeEntry != null)
				{
					cachedCodeEntry.AddCompiled(this.factory);
				}
				this.code = null;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000277B File Offset: 0x0000177B
		protected Regex(SerializationInfo info, StreamingContext context)
			: this(info.GetString("pattern"), (RegexOptions)info.GetInt32("options"))
		{
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002799 File Offset: 0x00001799
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			si.AddValue("pattern", this.ToString());
			si.AddValue("options", this.Options);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000027C2 File Offset: 0x000017C2
		[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal RegexRunnerFactory Compile(RegexCode code, RegexOptions roptions)
		{
			return RegexCompiler.Compile(code, roptions);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000027CB File Offset: 0x000017CB
		public static string Escape(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return RegexParser.Escape(str);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000027E1 File Offset: 0x000017E1
		public static string Unescape(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return RegexParser.Unescape(str);
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000027F7 File Offset: 0x000017F7
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002800 File Offset: 0x00001800
		public static int CacheSize
		{
			get
			{
				return Regex.cacheSize;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				Regex.cacheSize = value;
				if (Regex.livecode.Count > Regex.cacheSize)
				{
					lock (Regex.livecode)
					{
						while (Regex.livecode.Count > Regex.cacheSize)
						{
							Regex.livecode.RemoveLast();
						}
					}
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002878 File Offset: 0x00001878
		public RegexOptions Options
		{
			get
			{
				return this.roptions;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002880 File Offset: 0x00001880
		public bool RightToLeft
		{
			get
			{
				return this.UseOptionR();
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002888 File Offset: 0x00001888
		public override string ToString()
		{
			return this.pattern;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002890 File Offset: 0x00001890
		public string[] GetGroupNames()
		{
			string[] array;
			if (this.capslist == null)
			{
				int num = this.capsize;
				array = new string[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = Convert.ToString(i, CultureInfo.InvariantCulture);
				}
			}
			else
			{
				array = new string[this.capslist.Length];
				Array.Copy(this.capslist, 0, array, 0, this.capslist.Length);
			}
			return array;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000028F4 File Offset: 0x000018F4
		public int[] GetGroupNumbers()
		{
			int[] array;
			if (this.caps == null)
			{
				int num = this.capsize;
				array = new int[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = i;
				}
			}
			else
			{
				array = new int[this.caps.Count];
				IDictionaryEnumerator enumerator = this.caps.GetEnumerator();
				while (enumerator.MoveNext())
				{
					array[(int)enumerator.Value] = (int)enumerator.Key;
				}
			}
			return array;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000296C File Offset: 0x0000196C
		public string GroupNameFromNumber(int i)
		{
			if (this.capslist == null)
			{
				if (i >= 0 && i < this.capsize)
				{
					return i.ToString(CultureInfo.InvariantCulture);
				}
				return string.Empty;
			}
			else
			{
				if (this.caps != null)
				{
					object obj = this.caps[i];
					if (obj == null)
					{
						return string.Empty;
					}
					i = (int)obj;
				}
				if (i >= 0 && i < this.capslist.Length)
				{
					return this.capslist[i];
				}
				return string.Empty;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000029EC File Offset: 0x000019EC
		public int GroupNumberFromName(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this.capnames != null)
			{
				object obj = this.capnames[name];
				if (obj == null)
				{
					return -1;
				}
				return (int)obj;
			}
			else
			{
				int num = 0;
				foreach (char c in name)
				{
					if (c > '9' || c < '0')
					{
						return -1;
					}
					num *= 10;
					num += (int)(c - '0');
				}
				if (num >= 0 && num < this.capsize)
				{
					return num;
				}
				return -1;
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002A6D File Offset: 0x00001A6D
		public static bool IsMatch(string input, string pattern)
		{
			return new Regex(pattern, RegexOptions.None, true).IsMatch(input);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002A7D File Offset: 0x00001A7D
		public static bool IsMatch(string input, string pattern, RegexOptions options)
		{
			return new Regex(pattern, options, true).IsMatch(input);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002A90 File Offset: 0x00001A90
		public bool IsMatch(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return null == this.Run(true, -1, input, 0, input.Length, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002ACF File Offset: 0x00001ACF
		public bool IsMatch(string input, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return null == this.Run(true, -1, input, 0, input.Length, startat);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002AF3 File Offset: 0x00001AF3
		public static Match Match(string input, string pattern)
		{
			return new Regex(pattern, RegexOptions.None, true).Match(input);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002B03 File Offset: 0x00001B03
		public static Match Match(string input, string pattern, RegexOptions options)
		{
			return new Regex(pattern, options, true).Match(input);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002B13 File Offset: 0x00001B13
		public Match Match(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Run(false, -1, input, 0, input.Length, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002B44 File Offset: 0x00001B44
		public Match Match(string input, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Run(false, -1, input, 0, input.Length, startat);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002B68 File Offset: 0x00001B68
		public Match Match(string input, int beginning, int length)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Run(false, -1, input, beginning, length, this.UseOptionR() ? (beginning + length) : beginning);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002B9C File Offset: 0x00001B9C
		public static MatchCollection Matches(string input, string pattern)
		{
			return new Regex(pattern, RegexOptions.None, true).Matches(input);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002BAC File Offset: 0x00001BAC
		public static MatchCollection Matches(string input, string pattern, RegexOptions options)
		{
			return new Regex(pattern, options, true).Matches(input);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002BBC File Offset: 0x00001BBC
		public MatchCollection Matches(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return new MatchCollection(this, input, 0, input.Length, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002BEB File Offset: 0x00001BEB
		public MatchCollection Matches(string input, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return new MatchCollection(this, input, 0, input.Length, startat);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C0A File Offset: 0x00001C0A
		public static string Replace(string input, string pattern, string replacement)
		{
			return new Regex(pattern, RegexOptions.None, true).Replace(input, replacement);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002C1B File Offset: 0x00001C1B
		public static string Replace(string input, string pattern, string replacement, RegexOptions options)
		{
			return new Regex(pattern, options, true).Replace(input, replacement);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002C2C File Offset: 0x00001C2C
		public string Replace(string input, string replacement)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Replace(input, replacement, -1, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002C56 File Offset: 0x00001C56
		public string Replace(string input, string replacement, int count)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Replace(input, replacement, count, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002C80 File Offset: 0x00001C80
		public string Replace(string input, string replacement, int count, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (replacement == null)
			{
				throw new ArgumentNullException("replacement");
			}
			RegexReplacement regexReplacement = (RegexReplacement)this.replref.Get();
			if (regexReplacement == null || !regexReplacement.Pattern.Equals(replacement))
			{
				regexReplacement = RegexParser.ParseReplacement(replacement, this.caps, this.capsize, this.capnames, this.roptions);
				this.replref.Cache(regexReplacement);
			}
			return regexReplacement.Replace(this, input, count, startat);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002D01 File Offset: 0x00001D01
		public static string Replace(string input, string pattern, MatchEvaluator evaluator)
		{
			return new Regex(pattern, RegexOptions.None, true).Replace(input, evaluator);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002D12 File Offset: 0x00001D12
		public static string Replace(string input, string pattern, MatchEvaluator evaluator, RegexOptions options)
		{
			return new Regex(pattern, options, true).Replace(input, evaluator);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002D23 File Offset: 0x00001D23
		public string Replace(string input, MatchEvaluator evaluator)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Replace(input, evaluator, -1, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002D4D File Offset: 0x00001D4D
		public string Replace(string input, MatchEvaluator evaluator, int count)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Replace(input, evaluator, count, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002D77 File Offset: 0x00001D77
		public string Replace(string input, MatchEvaluator evaluator, int count, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return RegexReplacement.Replace(evaluator, this, input, count, startat);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002D92 File Offset: 0x00001D92
		public static string[] Split(string input, string pattern)
		{
			return new Regex(pattern, RegexOptions.None, true).Split(input);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002DA2 File Offset: 0x00001DA2
		public static string[] Split(string input, string pattern, RegexOptions options)
		{
			return new Regex(pattern, options, true).Split(input);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002DB2 File Offset: 0x00001DB2
		public string[] Split(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Split(input, 0, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002DDB File Offset: 0x00001DDB
		public string[] Split(string input, int count)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return RegexReplacement.Split(this, input, count, this.UseOptionR() ? input.Length : 0);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002E04 File Offset: 0x00001E04
		public string[] Split(string input, int count, int startat)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return RegexReplacement.Split(this, input, count, startat);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002E1D File Offset: 0x00001E1D
		[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
		public static void CompileToAssembly(RegexCompilationInfo[] regexinfos, AssemblyName assemblyname)
		{
			Regex.CompileToAssemblyInternal(regexinfos, assemblyname, null, null, Assembly.GetCallingAssembly().Evidence);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002E32 File Offset: 0x00001E32
		[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
		public static void CompileToAssembly(RegexCompilationInfo[] regexinfos, AssemblyName assemblyname, CustomAttributeBuilder[] attributes)
		{
			Regex.CompileToAssemblyInternal(regexinfos, assemblyname, attributes, null, Assembly.GetCallingAssembly().Evidence);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002E47 File Offset: 0x00001E47
		[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
		public static void CompileToAssembly(RegexCompilationInfo[] regexinfos, AssemblyName assemblyname, CustomAttributeBuilder[] attributes, string resourceFile)
		{
			Regex.CompileToAssemblyInternal(regexinfos, assemblyname, attributes, resourceFile, Assembly.GetCallingAssembly().Evidence);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002E5C File Offset: 0x00001E5C
		private static void CompileToAssemblyInternal(RegexCompilationInfo[] regexinfos, AssemblyName assemblyname, CustomAttributeBuilder[] attributes, string resourceFile, Evidence evidence)
		{
			if (assemblyname == null)
			{
				throw new ArgumentNullException("assemblyname");
			}
			if (regexinfos == null)
			{
				throw new ArgumentNullException("regexinfos");
			}
			RegexCompiler.CompileToAssembly(regexinfos, assemblyname, attributes, resourceFile, evidence);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002E85 File Offset: 0x00001E85
		protected void InitializeReferences()
		{
			if (this.refsInitialized)
			{
				throw new NotSupportedException(SR.GetString("OnlyAllowedOnce"));
			}
			this.refsInitialized = true;
			this.runnerref = new ExclusiveReference();
			this.replref = new SharedReference();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002EBC File Offset: 0x00001EBC
		internal Match Run(bool quick, int prevlen, string input, int beginning, int length, int startat)
		{
			if (startat < 0 || startat > input.Length)
			{
				throw new ArgumentOutOfRangeException("start", SR.GetString("BeginIndexNotNegative"));
			}
			if (length < 0 || length > input.Length)
			{
				throw new ArgumentOutOfRangeException("length", SR.GetString("LengthNotNegative"));
			}
			RegexRunner regexRunner = (RegexRunner)this.runnerref.Get();
			if (regexRunner == null)
			{
				if (this.factory != null)
				{
					regexRunner = this.factory.CreateInstance();
				}
				else
				{
					regexRunner = new RegexInterpreter(this.code, this.UseOptionInvariant() ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture);
				}
			}
			Match match = regexRunner.Scan(this, input, beginning, beginning + length, startat, prevlen, quick);
			this.runnerref.Release(regexRunner);
			return match;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002F80 File Offset: 0x00001F80
		private static CachedCodeEntry LookupCachedAndUpdate(string key)
		{
			lock (Regex.livecode)
			{
				for (LinkedListNode<CachedCodeEntry> linkedListNode = Regex.livecode.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					if (linkedListNode.Value._key == key)
					{
						Regex.livecode.Remove(linkedListNode);
						Regex.livecode.AddFirst(linkedListNode);
						return linkedListNode.Value;
					}
				}
			}
			return null;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003000 File Offset: 0x00002000
		private CachedCodeEntry CacheCode(string key)
		{
			CachedCodeEntry cachedCodeEntry = null;
			lock (Regex.livecode)
			{
				for (LinkedListNode<CachedCodeEntry> linkedListNode = Regex.livecode.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					if (linkedListNode.Value._key == key)
					{
						Regex.livecode.Remove(linkedListNode);
						Regex.livecode.AddFirst(linkedListNode);
						return linkedListNode.Value;
					}
				}
				if (Regex.cacheSize != 0)
				{
					cachedCodeEntry = new CachedCodeEntry(key, this.capnames, this.capslist, this.code, this.caps, this.capsize, this.runnerref, this.replref);
					Regex.livecode.AddFirst(cachedCodeEntry);
					if (Regex.livecode.Count > Regex.cacheSize)
					{
						Regex.livecode.RemoveLast();
					}
				}
			}
			return cachedCodeEntry;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000030E0 File Offset: 0x000020E0
		protected bool UseOptionC()
		{
			return (this.roptions & RegexOptions.Compiled) != RegexOptions.None;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000030F0 File Offset: 0x000020F0
		protected bool UseOptionR()
		{
			return (this.roptions & RegexOptions.RightToLeft) != RegexOptions.None;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003101 File Offset: 0x00002101
		internal bool UseOptionInvariant()
		{
			return (this.roptions & RegexOptions.CultureInvariant) != RegexOptions.None;
		}

		// Token: 0x04000626 RID: 1574
		internal const int MaxOptionShift = 10;

		// Token: 0x04000627 RID: 1575
		protected internal string pattern;

		// Token: 0x04000628 RID: 1576
		protected internal RegexRunnerFactory factory;

		// Token: 0x04000629 RID: 1577
		protected internal RegexOptions roptions;

		// Token: 0x0400062A RID: 1578
		protected internal Hashtable caps;

		// Token: 0x0400062B RID: 1579
		protected internal Hashtable capnames;

		// Token: 0x0400062C RID: 1580
		protected internal string[] capslist;

		// Token: 0x0400062D RID: 1581
		protected internal int capsize;

		// Token: 0x0400062E RID: 1582
		internal ExclusiveReference runnerref;

		// Token: 0x0400062F RID: 1583
		internal SharedReference replref;

		// Token: 0x04000630 RID: 1584
		internal RegexCode code;

		// Token: 0x04000631 RID: 1585
		internal bool refsInitialized;

		// Token: 0x04000632 RID: 1586
		internal static LinkedList<CachedCodeEntry> livecode = new LinkedList<CachedCodeEntry>();

		// Token: 0x04000633 RID: 1587
		internal static int cacheSize = 15;
	}
}
