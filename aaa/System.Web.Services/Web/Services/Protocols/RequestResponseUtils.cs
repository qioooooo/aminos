using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Services.Diagnostics;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000053 RID: 83
	internal class RequestResponseUtils
	{
		// Token: 0x060001D9 RID: 473 RVA: 0x000087BA File Offset: 0x000077BA
		private RequestResponseUtils()
		{
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000087C4 File Offset: 0x000077C4
		internal static Encoding GetEncoding(string contentType)
		{
			string charset = ContentType.GetCharset(contentType);
			Encoding encoding = null;
			try
			{
				if (charset != null && charset.Length > 0)
				{
					encoding = Encoding.GetEncoding(charset);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, typeof(RequestResponseUtils), "GetEncoding", ex);
				}
			}
			catch
			{
			}
			if (encoding != null)
			{
				return encoding;
			}
			return new ASCIIEncoding();
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00008854 File Offset: 0x00007854
		internal static Encoding GetEncoding2(string contentType)
		{
			if (!ContentType.IsApplication(contentType))
			{
				return RequestResponseUtils.GetEncoding(contentType);
			}
			string charset = ContentType.GetCharset(contentType);
			Encoding encoding = null;
			try
			{
				if (charset != null && charset.Length > 0)
				{
					encoding = Encoding.GetEncoding(charset);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, typeof(RequestResponseUtils), "GetEncoding2", ex);
				}
			}
			catch
			{
			}
			return encoding;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000088EC File Offset: 0x000078EC
		internal static string ReadResponse(WebResponse response)
		{
			return RequestResponseUtils.ReadResponse(response, response.GetResponseStream());
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000088FC File Offset: 0x000078FC
		internal static string ReadResponse(WebResponse response, Stream stream)
		{
			Encoding encoding = RequestResponseUtils.GetEncoding(response.ContentType);
			if (encoding == null)
			{
				encoding = Encoding.Default;
			}
			StreamReader streamReader = new StreamReader(stream, encoding, true);
			string text;
			try
			{
				text = streamReader.ReadToEnd();
			}
			finally
			{
				stream.Close();
			}
			return text;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00008948 File Offset: 0x00007948
		internal static Stream StreamToMemoryStream(Stream stream)
		{
			MemoryStream memoryStream = new MemoryStream(1024);
			byte[] array = new byte[1024];
			int num;
			while ((num = stream.Read(array, 0, array.Length)) != 0)
			{
				memoryStream.Write(array, 0, num);
			}
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000898E File Offset: 0x0000798E
		internal static string CreateResponseExceptionString(WebResponse response)
		{
			return RequestResponseUtils.CreateResponseExceptionString(response, response.GetResponseStream());
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000899C File Offset: 0x0000799C
		internal static string CreateResponseExceptionString(WebResponse response, Stream stream)
		{
			if (response is HttpWebResponse)
			{
				HttpWebResponse httpWebResponse = (HttpWebResponse)response;
				int statusCode = (int)httpWebResponse.StatusCode;
				if (statusCode >= 400 && statusCode != 500)
				{
					return Res.GetString("WebResponseKnownError", new object[] { statusCode, httpWebResponse.StatusDescription });
				}
			}
			string text = ((stream != null) ? RequestResponseUtils.ReadResponse(response, stream) : string.Empty);
			if (text.Length > 0)
			{
				text = RequestResponseUtils.HttpUtility.HtmlDecode(text);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Res.GetString("WebResponseUnknownError"));
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append("--");
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(text);
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append("--");
				stringBuilder.Append(".");
				return stringBuilder.ToString();
			}
			return Res.GetString("WebResponseUnknownErrorEmptyBody");
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008A94 File Offset: 0x00007A94
		internal static int GetBufferSize(int contentLength)
		{
			int num;
			if (contentLength == -1)
			{
				num = 8000;
			}
			else if (contentLength <= 16000)
			{
				num = contentLength;
			}
			else
			{
				num = 16000;
			}
			return num;
		}

		// Token: 0x02000054 RID: 84
		private static class HttpUtility
		{
			// Token: 0x060001E2 RID: 482 RVA: 0x00008AC0 File Offset: 0x00007AC0
			internal static string HtmlDecode(string s)
			{
				if (s == null)
				{
					return null;
				}
				if (s.IndexOf('&') < 0)
				{
					return s;
				}
				StringBuilder stringBuilder = new StringBuilder();
				StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
				RequestResponseUtils.HttpUtility.HtmlDecode(s, stringWriter);
				return stringBuilder.ToString();
			}

			// Token: 0x060001E3 RID: 483 RVA: 0x00008B00 File Offset: 0x00007B00
			public static void HtmlDecode(string s, TextWriter output)
			{
				if (s == null)
				{
					return;
				}
				if (s.IndexOf('&') < 0)
				{
					output.Write(s);
					return;
				}
				int length = s.Length;
				int i = 0;
				while (i < length)
				{
					char c = s[i];
					if (c != '&')
					{
						goto IL_014C;
					}
					int num = s.IndexOfAny(RequestResponseUtils.HttpUtility.s_entityEndingChars, i + 1);
					if (num <= 0 || s[num] != ';')
					{
						goto IL_014C;
					}
					string text = s.Substring(i + 1, num - i - 1);
					if (text.Length > 1 && text[0] == '#')
					{
						try
						{
							if (text[1] == 'x' || text[1] == 'X')
							{
								c = (char)int.Parse(text.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
							}
							else
							{
								c = (char)int.Parse(text.Substring(1), CultureInfo.InvariantCulture);
							}
							i = num;
							goto IL_014C;
						}
						catch (FormatException ex)
						{
							i++;
							if (Tracing.On)
							{
								Tracing.ExceptionCatch(TraceEventType.Warning, typeof(RequestResponseUtils.HttpUtility), "HtmlDecode", ex);
							}
							goto IL_014C;
						}
						catch (ArgumentException ex2)
						{
							i++;
							if (Tracing.On)
							{
								Tracing.ExceptionCatch(TraceEventType.Warning, typeof(RequestResponseUtils.HttpUtility), "HtmlDecode", ex2);
							}
							goto IL_014C;
						}
					}
					i = num;
					char c2 = RequestResponseUtils.HttpUtility.HtmlEntities.Lookup(text);
					if (c2 != '\0')
					{
						c = c2;
						goto IL_014C;
					}
					output.Write('&');
					output.Write(text);
					output.Write(';');
					IL_0153:
					i++;
					continue;
					IL_014C:
					output.Write(c);
					goto IL_0153;
				}
			}

			// Token: 0x040002C2 RID: 706
			private static char[] s_entityEndingChars = new char[] { ';', '&' };

			// Token: 0x02000055 RID: 85
			private static class HtmlEntities
			{
				// Token: 0x060001E5 RID: 485 RVA: 0x00008CAC File Offset: 0x00007CAC
				internal static char Lookup(string entity)
				{
					if (RequestResponseUtils.HttpUtility.HtmlEntities._entitiesLookupTable == null)
					{
						lock (RequestResponseUtils.HttpUtility.HtmlEntities._lookupLockObject)
						{
							if (RequestResponseUtils.HttpUtility.HtmlEntities._entitiesLookupTable == null)
							{
								Hashtable hashtable = new Hashtable();
								foreach (string text in RequestResponseUtils.HttpUtility.HtmlEntities._entitiesList)
								{
									hashtable[text.Substring(2)] = text[0];
								}
								RequestResponseUtils.HttpUtility.HtmlEntities._entitiesLookupTable = hashtable;
							}
						}
					}
					object obj = RequestResponseUtils.HttpUtility.HtmlEntities._entitiesLookupTable[entity];
					if (obj != null)
					{
						return (char)obj;
					}
					return '\0';
				}

				// Token: 0x040002C3 RID: 707
				private static object _lookupLockObject = new object();

				// Token: 0x040002C4 RID: 708
				private static string[] _entitiesList = new string[]
				{
					"\"-quot", "&-amp", "<-lt", ">-gt", "\u00a0-nbsp", "¡-iexcl", "¢-cent", "£-pound", "¤-curren", "¥-yen",
					"¦-brvbar", "§-sect", "\u00a8-uml", "©-copy", "ª-ordf", "«-laquo", "¬-not", "\u00ad-shy", "®-reg", "\u00af-macr",
					"°-deg", "±-plusmn", "²-sup2", "³-sup3", "\u00b4-acute", "µ-micro", "¶-para", "·-middot", "\u00b8-cedil", "¹-sup1",
					"º-ordm", "»-raquo", "¼-frac14", "½-frac12", "¾-frac34", "¿-iquest", "À-Agrave", "Á-Aacute", "Â-Acirc", "Ã-Atilde",
					"Ä-Auml", "Å-Aring", "Æ-AElig", "Ç-Ccedil", "È-Egrave", "É-Eacute", "Ê-Ecirc", "Ë-Euml", "Ì-Igrave", "Í-Iacute",
					"Î-Icirc", "Ï-Iuml", "Ð-ETH", "Ñ-Ntilde", "Ò-Ograve", "Ó-Oacute", "Ô-Ocirc", "Õ-Otilde", "Ö-Ouml", "×-times",
					"Ø-Oslash", "Ù-Ugrave", "Ú-Uacute", "Û-Ucirc", "Ü-Uuml", "Ý-Yacute", "Þ-THORN", "ß-szlig", "à-agrave", "á-aacute",
					"â-acirc", "ã-atilde", "ä-auml", "å-aring", "æ-aelig", "ç-ccedil", "è-egrave", "é-eacute", "ê-ecirc", "ë-euml",
					"ì-igrave", "í-iacute", "î-icirc", "ï-iuml", "ð-eth", "ñ-ntilde", "ò-ograve", "ó-oacute", "ô-ocirc", "õ-otilde",
					"ö-ouml", "÷-divide", "ø-oslash", "ù-ugrave", "ú-uacute", "û-ucirc", "ü-uuml", "ý-yacute", "þ-thorn", "ÿ-yuml",
					"Œ-OElig", "œ-oelig", "Š-Scaron", "š-scaron", "Ÿ-Yuml", "ƒ-fnof", "ˆ-circ", "\u02dc-tilde", "Α-Alpha", "Β-Beta",
					"Γ-Gamma", "Δ-Delta", "Ε-Epsilon", "Ζ-Zeta", "Η-Eta", "Θ-Theta", "Ι-Iota", "Κ-Kappa", "Λ-Lambda", "Μ-Mu",
					"Ν-Nu", "Ξ-Xi", "Ο-Omicron", "Π-Pi", "Ρ-Rho", "Σ-Sigma", "Τ-Tau", "Υ-Upsilon", "Φ-Phi", "Χ-Chi",
					"Ψ-Psi", "Ω-Omega", "α-alpha", "β-beta", "γ-gamma", "δ-delta", "ε-epsilon", "ζ-zeta", "η-eta", "θ-theta",
					"ι-iota", "κ-kappa", "λ-lambda", "μ-mu", "ν-nu", "ξ-xi", "ο-omicron", "π-pi", "ρ-rho", "ς-sigmaf",
					"σ-sigma", "τ-tau", "υ-upsilon", "φ-phi", "χ-chi", "ψ-psi", "ω-omega", "ϑ-thetasym", "ϒ-upsih", "ϖ-piv",
					"\u2002-ensp", "\u2003-emsp", "\u2009-thinsp", "\u200c-zwnj", "\u200d-zwj", "\u200e-lrm", "\u200f-rlm", "–-ndash", "—-mdash", "‘-lsquo",
					"’-rsquo", "‚-sbquo", "“-ldquo", "”-rdquo", "„-bdquo", "†-dagger", "‡-Dagger", "•-bull", "…-hellip", "‰-permil",
					"′-prime", "″-Prime", "‹-lsaquo", "›-rsaquo", "‾-oline", "⁄-frasl", "€-euro", "ℑ-image", "℘-weierp", "ℜ-real",
					"™-trade", "ℵ-alefsym", "←-larr", "↑-uarr", "→-rarr", "↓-darr", "↔-harr", "↵-crarr", "⇐-lArr", "⇑-uArr",
					"⇒-rArr", "⇓-dArr", "⇔-hArr", "∀-forall", "∂-part", "∃-exist", "∅-empty", "∇-nabla", "∈-isin", "∉-notin",
					"∋-ni", "∏-prod", "∑-sum", "−-minus", "∗-lowast", "√-radic", "∝-prop", "∞-infin", "∠-ang", "∧-and",
					"∨-or", "∩-cap", "∪-cup", "∫-int", "∴-there4", "∼-sim", "≅-cong", "≈-asymp", "≠-ne", "≡-equiv",
					"≤-le", "≥-ge", "⊂-sub", "⊃-sup", "⊄-nsub", "⊆-sube", "⊇-supe", "⊕-oplus", "⊗-otimes", "⊥-perp",
					"⋅-sdot", "⌈-lceil", "⌉-rceil", "⌊-lfloor", "⌋-rfloor", "〈-lang", "〉-rang", "◊-loz", "♠-spades", "♣-clubs",
					"♥-hearts", "♦-diams"
				};

				// Token: 0x040002C5 RID: 709
				private static Hashtable _entitiesLookupTable;
			}
		}
	}
}
