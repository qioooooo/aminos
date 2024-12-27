using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.UI;

namespace System.Web.Configuration
{
	// Token: 0x020001A7 RID: 423
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class BrowserCapabilitiesFactory : BrowserCapabilitiesFactoryBase
	{
		// Token: 0x060011BC RID: 4540 RVA: 0x0004EDA0 File Offset: 0x0004DDA0
		public override void ConfigureBrowserCapabilities(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			this.DefaultProcess(headers, browserCaps);
			if (!base.IsBrowserUnknown(browserCaps))
			{
				return;
			}
			this.DefaultDefaultProcess(headers, browserCaps);
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0004EDBE File Offset: 0x0004DDBE
		protected virtual void IeProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0004EDC0 File Offset: 0x0004DDC0
		protected virtual void IeProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0004EDC4 File Offset: 0x0004DDC4
		private bool IeProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Mozilla[^(]*\\([C|c]ompatible;\\s*MSIE (?'version'(?'major'\\d+)(?'minor'\\.\\d+)(?'letters'\\w*))(?'extra'[^)]*)"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			bool flag = regexWorker.ProcessRegex(text, "Opera|Go\\.Web|Windows CE|EudoraWeb");
			if (flag)
			{
				return false;
			}
			capabilities["browser"] = "IE";
			capabilities["extra"] = regexWorker["${extra}"];
			capabilities["isColor"] = "true";
			capabilities["letters"] = regexWorker["${letters}"];
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["screenBitDepth"] = "8";
			capabilities["type"] = regexWorker["IE${major}"];
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.AddBrowser("IE");
			this.IeProcessGateways(headers, browserCaps);
			this.IeaolProcess(headers, browserCaps);
			this.IebetaProcess(headers, browserCaps);
			this.IeupdateProcess(headers, browserCaps);
			bool flag2 = true;
			if (!this.Ie5to9Process(headers, browserCaps) && !this.Ie4Process(headers, browserCaps) && !this.Ie3Process(headers, browserCaps) && !this.Ie2Process(headers, browserCaps) && !this.Ie1minor5Process(headers, browserCaps))
			{
				flag2 = false;
			}
			this.IeProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0004EF40 File Offset: 0x0004DF40
		protected virtual void Ie5to9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0004EF42 File Offset: 0x0004DF42
		protected virtual void Ie5to9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0004EF44 File Offset: 0x0004DF44
		private bool Ie5to9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^[5-9]|[1-9]\\d+"))
			{
				return false;
			}
			capabilities["activexcontrols"] = "true";
			capabilities["backgroundsounds"] = "true";
			capabilities["cookies"] = "true";
			capabilities["css1"] = "true";
			capabilities["css2"] = "true";
			capabilities["ecmascriptversion"] = "1.2";
			capabilities["frames"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["javascript"] = "true";
			capabilities["jscriptversion"] = "5.0";
			capabilities["msdomversion"] = regexWorker["${majorversion}${minorversion}"];
			capabilities["supportsCallback"] = "true";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["supportsMaintainScrollPositionOnPostback"] = "true";
			capabilities["supportsVCard"] = "true";
			capabilities["supportsXmlHttp"] = "true";
			capabilities["tables"] = "true";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["vbscript"] = "true";
			capabilities["w3cdomversion"] = "1.0";
			capabilities["xml"] = "true";
			browserCaps.AddBrowser("IE5to9");
			this.Ie5to9ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Ie6to9Process(headers, browserCaps) && !this.Ie5Process(headers, browserCaps) && !this.Ie5to9macProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Ie5to9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0004F12A File Offset: 0x0004E12A
		protected virtual void Ie6to9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0004F12C File Offset: 0x0004E12C
		protected virtual void Ie6to9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0004F130 File Offset: 0x0004E130
		private bool Ie6to9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "[6-9]|[1-9]\\d+"))
			{
				return false;
			}
			capabilities["jscriptversion"] = "5.6";
			capabilities["ExchangeOmaSupported"] = "true";
			browserCaps.AddBrowser("IE6to9");
			this.Ie6to9ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Treo600Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Ie6to9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0004F1BC File Offset: 0x0004E1BC
		protected virtual void Treo600ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0004F1BE File Offset: 0x0004E1BE
		protected virtual void Treo600ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0004F1C0 File Offset: 0x0004E1C0
		private bool Treo600Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "PalmSource; Blazer"))
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "PalmSource; Blazer 3\\.0\\)\\s\\d+;(?'screenPixelsHeight'\\d+)x(?'screenPixelsWidth'\\d+)$");
			capabilities["browser"] = "Blazer 3.0";
			capabilities["cachesAllResponsesWithExpires"] = "false";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["canRenderEmptySelects"] = "true";
			capabilities["canSendMail"] = "true";
			capabilities["cookies"] = "true";
			capabilities["ecmascriptversion"] = "1.1";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "false";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["javascript"] = "true";
			capabilities["jscriptversion"] = "0.0";
			capabilities["maximumHrefLength"] = "10000";
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["mobileDeviceManufacturer"] = "";
			capabilities["mobileDeviceModel"] = "";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "text/html";
			capabilities["preferredRenderingType"] = "html32";
			capabilities["preferredRequestEncoding"] = "utf-8";
			capabilities["preferredResponseEncoding"] = "utf-8";
			capabilities["rendersBreaksAfterHtmlLists"] = "true";
			capabilities["requiredMetaTagNameValue"] = "PalmComputingPlatform";
			capabilities["requiresAttributeColonSubstitution"] = "false";
			capabilities["requiresContentTypeMetaTag"] = "false";
			capabilities["requiresControlStateInSession"] = "false";
			capabilities["requiresDBCSCharacter"] = "false";
			capabilities["requiresFullyQualifiedRedirectUrl"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["requiresLeadingPageBreak"] = "false";
			capabilities["requiresNoBreakInFormatting"] = "false";
			capabilities["requiresOutputOptimization"] = "false";
			capabilities["requiresPostRedirectionHandling"] = "false";
			capabilities["requiresPragmaNoCacheHeader"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["requiresUniqueHtmlCheckboxNames"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "13";
			capabilities["screenCharactersWidth"] = "32";
			capabilities["screenPixelsHeight"] = regexWorker["${screenPixelsHeight}"];
			capabilities["screenPixelsWidth"] = regexWorker["${screenPixelsWidth}"];
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyColor"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCharacterEntityEncoding"] = "true";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivAlign"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsEmptyStringInCookieValue"] = "true";
			capabilities["supportsFileUpload"] = "false";
			capabilities["supportsFontColor"] = "true";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsIModeSymbols"] = "false";
			capabilities["supportsInputIStyle"] = "false";
			capabilities["supportsInputMode"] = "false";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsJPhoneMultiMediaAttributes"] = "false";
			capabilities["supportsJPhoneSymbols"] = "false";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["supportsQueryStringInFormAction"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["supportsSelectMultiple"] = "true";
			capabilities["supportsUncheck"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = "Handspring Treo 600";
			browserCaps.AddBrowser("Treo600");
			this.Treo600ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Treo600ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x0004F66D File Offset: 0x0004E66D
		protected virtual void Ie5ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0004F66F File Offset: 0x0004E66F
		protected virtual void Ie5ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0004F674 File Offset: 0x0004E674
		private bool Ie5Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^5$"))
			{
				return false;
			}
			browserCaps.AddBrowser("IE5");
			this.Ie5ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Ie50Process(headers, browserCaps) && !this.Ie55Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Ie5ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0004F6EA File Offset: 0x0004E6EA
		protected virtual void Ie50ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0004F6EC File Offset: 0x0004E6EC
		protected virtual void Ie50ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0004F6F0 File Offset: 0x0004E6F0
		private bool Ie50Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["minorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^\\.0"))
			{
				return false;
			}
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("IE50");
			this.Ie50ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie50ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0004F764 File Offset: 0x0004E764
		protected virtual void Ie55ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0004F766 File Offset: 0x0004E766
		protected virtual void Ie55ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0004F768 File Offset: 0x0004E768
		private bool Ie55Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["minorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^\\.5"))
			{
				return false;
			}
			capabilities["jscriptversion"] = "5.5";
			capabilities["ExchangeOmaSupported"] = "true";
			browserCaps.AddBrowser("IE55");
			this.Ie55ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie55ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0004F7E7 File Offset: 0x0004E7E7
		protected virtual void Ie5to9macProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0004F7E9 File Offset: 0x0004E7E9
		protected virtual void Ie5to9macProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0004F7EC File Offset: 0x0004E7EC
		private bool Ie5to9macProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["platform"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(MacPPC|Mac68K)"))
			{
				return false;
			}
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("IE5to9Mac");
			this.Ie5to9macProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie5to9macProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0004F860 File Offset: 0x0004E860
		protected virtual void Ie4ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0004F862 File Offset: 0x0004E862
		protected virtual void Ie4ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0004F864 File Offset: 0x0004E864
		private bool Ie4Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MSIE 4"))
			{
				return false;
			}
			capabilities["activexcontrols"] = "true";
			capabilities["backgroundsounds"] = "true";
			capabilities["cdf"] = "true";
			capabilities["cookies"] = "true";
			capabilities["css1"] = "true";
			capabilities["ecmascriptversion"] = "1.2";
			capabilities["frames"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["javascript"] = "true";
			capabilities["jscriptversion"] = "3.0";
			capabilities["msdomversion"] = "4.0";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "false";
			capabilities["supportsMaintainScrollPositionOnPostback"] = "true";
			capabilities["tables"] = "true";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["vbscript"] = "true";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("IE4");
			this.Ie4ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie4ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0004F9E3 File Offset: 0x0004E9E3
		protected virtual void Ie3ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0004F9E5 File Offset: 0x0004E9E5
		protected virtual void Ie3ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0004F9E8 File Offset: 0x0004E9E8
		private bool Ie3Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^3"))
			{
				return false;
			}
			capabilities["activexcontrols"] = "true";
			capabilities["backgroundsounds"] = "true";
			capabilities["cookies"] = "true";
			capabilities["css1"] = "true";
			capabilities["ecmascriptversion"] = "1.0";
			capabilities["frames"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["javascript"] = "true";
			capabilities["jscriptversion"] = "1.0";
			capabilities["supportsMultilineTextBoxDisplay"] = "false";
			capabilities["tables"] = "true";
			capabilities["vbscript"] = "true";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("IE3");
			this.Ie3ProcessGateways(headers, browserCaps);
			this.Ie3akProcess(headers, browserCaps);
			this.Ie3skProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Ie3win16Process(headers, browserCaps) && !this.Ie3macProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Ie3ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x0004FB45 File Offset: 0x0004EB45
		protected virtual void Ie3win16ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x0004FB47 File Offset: 0x0004EB47
		protected virtual void Ie3win16ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0004FB4C File Offset: 0x0004EB4C
		private bool Ie3win16Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "16bit|Win(dows 3\\.1|16)"))
			{
				return false;
			}
			capabilities["activexcontrols"] = "false";
			capabilities["javaapplets"] = "false";
			browserCaps.AddBrowser("IE3win16");
			this.Ie3win16ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Ie3win16aProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Ie3win16ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0004FBD3 File Offset: 0x0004EBD3
		protected virtual void Ie3win16aProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0004FBD5 File Offset: 0x0004EBD5
		protected virtual void Ie3win16aProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0004FBD8 File Offset: 0x0004EBD8
		private bool Ie3win16aProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["extra"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^a"))
			{
				return false;
			}
			capabilities["beta"] = "true";
			capabilities["javascript"] = "false";
			capabilities["vbscript"] = "false";
			browserCaps.AddBrowser("IE3win16a");
			this.Ie3win16aProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie3win16aProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0004FC67 File Offset: 0x0004EC67
		protected virtual void Ie3macProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0004FC69 File Offset: 0x0004EC69
		protected virtual void Ie3macProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0004FC6C File Offset: 0x0004EC6C
		private bool Ie3macProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "PPC Mac|Macintosh.*(68K|PPC)|Mac_(PowerPC|PPC|68(K|000))"))
			{
				return false;
			}
			capabilities["activexcontrols"] = "false";
			capabilities["vbscript"] = "false";
			browserCaps.AddBrowser("IE3Mac");
			this.Ie3macProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie3macProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0004FCE6 File Offset: 0x0004ECE6
		protected virtual void Ie3akProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0004FCE8 File Offset: 0x0004ECE8
		protected virtual void Ie3akProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0004FCEC File Offset: 0x0004ECEC
		private bool Ie3akProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["extra"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "; AK;"))
			{
				return false;
			}
			capabilities["ak"] = "true";
			this.Ie3akProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie3akProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x0004FD50 File Offset: 0x0004ED50
		protected virtual void Ie3skProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0004FD52 File Offset: 0x0004ED52
		protected virtual void Ie3skProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0004FD54 File Offset: 0x0004ED54
		private bool Ie3skProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["extra"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "; SK;"))
			{
				return false;
			}
			capabilities["sk"] = "true";
			this.Ie3skProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie3skProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0004FDB8 File Offset: 0x0004EDB8
		protected virtual void Ie2ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0004FDBA File Offset: 0x0004EDBA
		protected virtual void Ie2ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0004FDBC File Offset: 0x0004EDBC
		private bool Ie2Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^2"))
			{
				return false;
			}
			capabilities["backgroundsounds"] = "true";
			capabilities["cookies"] = "true";
			capabilities["tables"] = "true";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("IE2");
			this.Ie2ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.WebtvProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Ie2ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0004FE6D File Offset: 0x0004EE6D
		protected virtual void Ie1minor5ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x0004FE6F File Offset: 0x0004EE6F
		protected virtual void Ie1minor5ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0004FE74 File Offset: 0x0004EE74
		private bool Ie1minor5Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["version"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^1\\.5"))
			{
				return false;
			}
			capabilities["cookies"] = "true";
			capabilities["tables"] = "true";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("IE1minor5");
			this.Ie1minor5ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie1minor5ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0004FF08 File Offset: 0x0004EF08
		protected virtual void IeaolProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x0004FF0A File Offset: 0x0004EF0A
		protected virtual void IeaolProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0004FF0C File Offset: 0x0004EF0C
		private bool IeaolProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["extra"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "; AOL"))
			{
				return false;
			}
			capabilities["aol"] = "true";
			capabilities["frames"] = "true";
			this.IeaolProcessGateways(headers, browserCaps);
			bool flag = false;
			this.IeaolProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0004FF80 File Offset: 0x0004EF80
		protected virtual void IebetaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0004FF82 File Offset: 0x0004EF82
		protected virtual void IebetaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0004FF84 File Offset: 0x0004EF84
		private bool IebetaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["letters"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^([bB]|ab)"))
			{
				return false;
			}
			capabilities["beta"] = "true";
			this.IebetaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.IebetaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0004FFE8 File Offset: 0x0004EFE8
		protected virtual void IeupdateProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0004FFEA File Offset: 0x0004EFEA
		protected virtual void IeupdateProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0004FFEC File Offset: 0x0004EFEC
		private bool IeupdateProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["extra"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "; Update a;"))
			{
				return false;
			}
			capabilities["authenticodeupdate"] = "true";
			this.IeupdateProcessGateways(headers, browserCaps);
			bool flag = false;
			this.IeupdateProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00050050 File Offset: 0x0004F050
		protected virtual void MozillaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00050052 File Offset: 0x0004F052
		protected virtual void MozillaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00050054 File Offset: 0x0004F054
		private bool MozillaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Mozilla"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			bool flag = regexWorker.ProcessRegex(text, "MME|Opera");
			if (flag)
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "Mozilla/(?'version'(?'major'\\d+)(?'minor'\\.\\d+)\\w*)");
			regexWorker.ProcessRegex(browserCaps[string.Empty], " (?'screenWidth'\\d*)x(?'screenHeight'\\d*)");
			capabilities["browser"] = "Mozilla";
			capabilities["cookies"] = "false";
			capabilities["defaultScreenCharactersHeight"] = "40";
			capabilities["defaultScreenCharactersWidth"] = "80";
			capabilities["defaultScreenPixelsHeight"] = "480";
			capabilities["defaultScreenPixelsWidth"] = "640";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "false";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["screenBitDepth"] = "8";
			capabilities["screenPixelsHeight"] = regexWorker["${screenHeight}"];
			capabilities["screenPixelsWidth"] = regexWorker["${screenWidth}"];
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsDivNoWrap"] = "true";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["type"] = "Mozilla";
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.AddBrowser("Mozilla");
			this.MozillaProcessGateways(headers, browserCaps);
			this.MozillabetaProcess(headers, browserCaps);
			this.MozillagoldProcess(headers, browserCaps);
			bool flag2 = true;
			if (!this.IeProcess(headers, browserCaps) && !this.PowerbrowserProcess(headers, browserCaps) && !this.GeckoProcess(headers, browserCaps) && !this.AvantgoProcess(headers, browserCaps) && !this.GoamericaProcess(headers, browserCaps) && !this.Netscape3Process(headers, browserCaps) && !this.Netscape4Process(headers, browserCaps) && !this.MypalmProcess(headers, browserCaps) && !this.EudorawebProcess(headers, browserCaps) && !this.WinceProcess(headers, browserCaps) && !this.MspieProcess(headers, browserCaps))
			{
				flag2 = false;
			}
			this.MozillaProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0005031B File Offset: 0x0004F31B
		protected virtual void MozillabetaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0005031D File Offset: 0x0004F31D
		protected virtual void MozillabetaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00050320 File Offset: 0x0004F320
		private bool MozillabetaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Mozilla/\\d+\\.\\d+b"))
			{
				return false;
			}
			capabilities["beta"] = "true";
			this.MozillabetaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MozillabetaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0005037F File Offset: 0x0004F37F
		protected virtual void MozillagoldProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00050381 File Offset: 0x0004F381
		protected virtual void MozillagoldProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00050384 File Offset: 0x0004F384
		private bool MozillagoldProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Mozilla/\\d+\\.\\d+\\w*Gold"))
			{
				return false;
			}
			capabilities["gold"] = "true";
			this.MozillagoldProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MozillagoldProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x000503E3 File Offset: 0x0004F3E3
		protected virtual void PowerbrowserProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x000503E5 File Offset: 0x0004F3E5
		protected virtual void PowerbrowserProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x000503E8 File Offset: 0x0004F3E8
		private bool PowerbrowserProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Mozilla/2\\.01 \\(Compatible\\) Oracle\\(tm\\) PowerBrowser\\(tm\\)/1\\.0a"))
			{
				return false;
			}
			capabilities["browser"] = "PowerBrowser";
			capabilities["cookies"] = "true";
			capabilities["ecmascriptversion"] = "1.0";
			capabilities["frames"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["javascript"] = "true";
			capabilities["majorversion"] = "1";
			capabilities["minorversion"] = ".5";
			capabilities["platform"] = "Win95";
			capabilities["tables"] = "true";
			capabilities["vbscript"] = "true";
			capabilities["version"] = "1.5";
			capabilities["type"] = "PowerBrowser1";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("PowerBrowser");
			this.PowerbrowserProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PowerbrowserProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00050527 File Offset: 0x0004F527
		protected virtual void GeckoProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00050529 File Offset: 0x0004F529
		protected virtual void GeckoProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x0005052C File Offset: 0x0004F52C
		private bool GeckoProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Gecko"))
			{
				return false;
			}
			capabilities["browser"] = "Mozilla";
			capabilities["css1"] = "true";
			capabilities["css2"] = "true";
			capabilities["cookies"] = "true";
			capabilities["ecmascriptversion"] = "1.5";
			capabilities["frames"] = "true";
			capabilities["isColor"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["javascript"] = "true";
			capabilities["maximumRenderedPageSize"] = "20000";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["screenBitDepth"] = "32";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFileUpload"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = "desktop";
			capabilities["version"] = regexWorker["${version}"];
			capabilities["w3cdomversion"] = "1.0";
			browserCaps.AddBrowser("Gecko");
			this.GeckoProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.MozillarvProcess(headers, browserCaps) && !this.SafariProcess(headers, browserCaps) && !this.Netscape5Process(headers, browserCaps))
			{
				flag = false;
			}
			this.GeckoProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x000506CD File Offset: 0x0004F6CD
		protected virtual void MozillarvProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x000506CF File Offset: 0x0004F6CF
		protected virtual void MozillarvProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x000506D4 File Offset: 0x0004F6D4
		private bool MozillarvProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "rv\\:(?'version'(?'major'\\d+)(?'minor'\\.[.\\d]*))"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			bool flag = regexWorker.ProcessRegex(text, "Netscape");
			if (flag)
			{
				return false;
			}
			capabilities["ecmascriptversion"] = "1.4";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["supportsCallback"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsMaintainScrollPositionOnPostback"] = "true";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["type"] = regexWorker["Mozilla${major}"];
			capabilities["version"] = regexWorker["${version}"];
			capabilities["w3cdomversion"] = "1.0";
			browserCaps.AddBrowser("MozillaRV");
			this.MozillarvProcessGateways(headers, browserCaps);
			this.Mozillav14plusProcess(headers, browserCaps);
			bool flag2 = true;
			if (!this.MozillafirebirdProcess(headers, browserCaps) && !this.MozillafirefoxProcess(headers, browserCaps))
			{
				flag2 = false;
			}
			this.MozillarvProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00050844 File Offset: 0x0004F844
		protected virtual void Mozillav14plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x00050846 File Offset: 0x0004F846
		protected virtual void Mozillav14plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00050848 File Offset: 0x0004F848
		private bool Mozillav14plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^1$"))
			{
				return false;
			}
			text = (string)capabilities["minorversion"];
			if (!regexWorker.ProcessRegex(text, "^\\.[4-9]"))
			{
				return false;
			}
			capabilities["backgroundsounds"] = "true";
			capabilities["css1"] = "true";
			capabilities["css2"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["maximumRenderedPageSize"] = "2000000";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["screenBitDepth"] = "32";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["type"] = regexWorker["Mozilla${version}"];
			capabilities["xml"] = "true";
			this.Mozillav14plusProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mozillav14plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00050965 File Offset: 0x0004F965
		protected virtual void MozillafirebirdProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00050967 File Offset: 0x0004F967
		protected virtual void MozillafirebirdProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0005096C File Offset: 0x0004F96C
		private bool MozillafirebirdProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Gecko\\/\\d+ Firebird\\/(?'version'(?'major'\\d+)(?'minor'\\.[.\\d]*))"))
			{
				return false;
			}
			capabilities["browser"] = "Firebird";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["version"] = regexWorker["${version}"];
			capabilities["type"] = regexWorker["Firebird${version}"];
			browserCaps.AddBrowser("MozillaFirebird");
			this.MozillafirebirdProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MozillafirebirdProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00050A2E File Offset: 0x0004FA2E
		protected virtual void MozillafirefoxProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00050A30 File Offset: 0x0004FA30
		protected virtual void MozillafirefoxProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x00050A34 File Offset: 0x0004FA34
		private bool MozillafirefoxProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Gecko\\/\\d+ Firefox\\/(?'version'(?'major'\\d+)(?'minor'\\.[.\\d]*))"))
			{
				return false;
			}
			capabilities["browser"] = "Firefox";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["version"] = regexWorker["${version}"];
			capabilities["type"] = regexWorker["Firefox${version}"];
			browserCaps.AddBrowser("MozillaFirefox");
			this.MozillafirefoxProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MozillafirefoxProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00050AF6 File Offset: 0x0004FAF6
		protected virtual void SafariProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00050AF8 File Offset: 0x0004FAF8
		protected virtual void SafariProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00050AFC File Offset: 0x0004FAFC
		private bool SafariProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "AppleWebKit/(?'webversion'\\d+)"))
			{
				return false;
			}
			capabilities["appleWebTechnologyVersion"] = regexWorker["${webversion}"];
			capabilities["backgroundsounds"] = "true";
			capabilities["browser"] = "AppleMAC-Safari";
			capabilities["css1"] = "true";
			capabilities["css2"] = "true";
			capabilities["ecmascriptversion"] = "0.0";
			capabilities["futureBrowser"] = "Apple Safari";
			capabilities["screenBitDepth"] = "24";
			capabilities["tables"] = "true";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["type"] = "Desktop";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("Safari");
			this.SafariProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Safari60Process(headers, browserCaps) && !this.Safari85Process(headers, browserCaps) && !this.Safari1plusProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.SafariProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00050C42 File Offset: 0x0004FC42
		protected virtual void Safari60ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x00050C44 File Offset: 0x0004FC44
		protected virtual void Safari60ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x00050C48 File Offset: 0x0004FC48
		private bool Safari60Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["appleWebTechnologyVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "60"))
			{
				return false;
			}
			capabilities["ecmascriptversion"] = "1.0";
			browserCaps.AddBrowser("Safari60");
			this.Safari60ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Safari60ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00050CB7 File Offset: 0x0004FCB7
		protected virtual void Safari85ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00050CB9 File Offset: 0x0004FCB9
		protected virtual void Safari85ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00050CBC File Offset: 0x0004FCBC
		private bool Safari85Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["appleWebTechnologyVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "85"))
			{
				return false;
			}
			capabilities["ecmascriptversion"] = "1.4";
			browserCaps.AddBrowser("Safari85");
			this.Safari85ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Safari85ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00050D2B File Offset: 0x0004FD2B
		protected virtual void Safari1plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00050D2D File Offset: 0x0004FD2D
		protected virtual void Safari1plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00050D30 File Offset: 0x0004FD30
		private bool Safari1plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["appleWebTechnologyVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "\\d\\d\\d"))
			{
				return false;
			}
			capabilities["ecmascriptversion"] = "1.4";
			capabilities["w3cdomversion"] = "1.0";
			capabilities["supportsCallback"] = "true";
			browserCaps.AddBrowser("Safari1Plus");
			this.Safari1plusProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Safari1plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00050DBF File Offset: 0x0004FDBF
		protected virtual void AvantgoProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00050DC1 File Offset: 0x0004FDC1
		protected virtual void AvantgoProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00050DC4 File Offset: 0x0004FDC4
		private bool AvantgoProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "\\(compatible; AvantGo .*\\)"))
			{
				return false;
			}
			regexWorker.ProcessRegex(headers["X-AVANTGO-VERSION"], "(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*)");
			capabilities["browser"] = "AvantGo";
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "true";
			capabilities["defaultScreenCharactersHeight"] = "6";
			capabilities["defaultScreenCharactersWidth"] = "12";
			capabilities["defaultScreenPixelsHeight"] = "72";
			capabilities["defaultScreenPixelsWidth"] = "96";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["javascript"] = "false";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "2560";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiredMetaTagNameValue"] = "HandheldFriendly";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["requiresLeadingPageBreak"] = "true";
			capabilities["requiresUniqueHtmlCheckboxNames"] = "true";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "13";
			capabilities["screenCharactersWidth"] = "30";
			capabilities["screenPixelsHeight"] = "150";
			capabilities["screenPixelsWidth"] = "150";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsFontSize"] = "false";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsItalic"] = "false";
			capabilities["tables"] = "true";
			capabilities["type"] = regexWorker["AvantGo${browserMajorVersion}"];
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("AvantGo");
			this.AvantgoProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.TmobilesidekickProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.AvantgoProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x000510B5 File Offset: 0x000500B5
		protected virtual void TmobilesidekickProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x000510B7 File Offset: 0x000500B7
		protected virtual void TmobilesidekickProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x000510BC File Offset: 0x000500BC
		private bool TmobilesidekickProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Danger hiptop"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["css1"] = "true";
			capabilities["ecmaScriptVersion"] = "1.3";
			capabilities["frames"] = "true";
			capabilities["inputType"] = "telephoneKeypad";
			capabilities["javaapplets"] = "true";
			capabilities["majorVersion"] = "5";
			capabilities["maximumRenderedPageSize"] = "7000";
			capabilities["minorVersion"] = ".0";
			capabilities["mobileDeviceManufacturer"] = "T-Mobile";
			capabilities["mobileDeviceModel"] = "SideKick";
			capabilities["preferredRenderingType"] = "html32";
			capabilities["requiresLeadingPageBreak"] = "false";
			capabilities["requiresUniqueHtmlCheckboxNames"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "11";
			capabilities["screenCharactersWidth"] = "57";
			capabilities["screenPixelsHeight"] = "136";
			capabilities["screenPixelsWidth"] = "236";
			capabilities["supportsCss"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["type"] = "AvantGo 3";
			capabilities["version"] = "5.0";
			browserCaps.AddBrowser("TMobileSidekick");
			this.TmobilesidekickProcessGateways(headers, browserCaps);
			bool flag = false;
			this.TmobilesidekickProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x00051286 File Offset: 0x00050286
		protected virtual void CasiopeiaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00051288 File Offset: 0x00050288
		protected virtual void CasiopeiaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0005128C File Offset: 0x0005028C
		private bool CasiopeiaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "CASSIOPEIA BE"))
			{
				return false;
			}
			capabilities["browser"] = "CASSIOPEIA";
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["cookies"] = "true";
			capabilities["css1"] = "false";
			capabilities["defaultScreenCharactersHeight"] = "6";
			capabilities["defaultScreenCharactersWidth"] = "12";
			capabilities["defaultScreenPixelsHeight"] = "72";
			capabilities["defaultScreenPixelsWidth"] = "96";
			capabilities["ecmascriptversion"] = "1.3";
			capabilities["frames"] = "true";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isMobileDevice"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["javascript"] = "true";
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["mobileDeviceManufacturer"] = "Casio";
			capabilities["mobileDeviceModel"] = "Casio BE-500";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["requiresContentTypeMetaTag"] = "true";
			capabilities["requiresLeadingPageBreak"] = "true";
			capabilities["requiresNoBreakInFormatting"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "50";
			capabilities["screenCharactersWidth"] = "38";
			capabilities["screenPixelsHeight"] = "320";
			capabilities["screenPixelsWidth"] = "240";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCharacterEntityEncoding"] = "false";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivNoWrap"] = "true";
			capabilities["supportsFileUpload"] = "false";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "false";
			capabilities["supportsItalic"] = "false";
			capabilities["tables"] = "true";
			capabilities["type"] = "CASSIOPEIA";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("Casiopeia");
			this.CasiopeiaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.CasiopeiaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00051571 File Offset: 0x00050571
		protected virtual void DefaultProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00051573 File Offset: 0x00050573
		protected virtual void DefaultProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x00051578 File Offset: 0x00050578
		private bool DefaultProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			capabilities["activexcontrols"] = "false";
			capabilities["ak"] = "false";
			capabilities["aol"] = "false";
			capabilities["authenticodeupdate"] = "false";
			capabilities["backgroundsounds"] = "false";
			capabilities["beta"] = "false";
			capabilities["browser"] = "Unknown";
			capabilities["cachesAllResponsesWithExpires"] = "false";
			capabilities["canCombineFormsInDeck"] = "true";
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["canRenderAfterInputOrSelectElement"] = "true";
			capabilities["canRenderEmptySelects"] = "true";
			capabilities["canRenderInputAndSelectElementsTogether"] = "true";
			capabilities["canRenderMixedSelects"] = "true";
			capabilities["canRenderOneventAndPrevElementsTogether"] = "true";
			capabilities["canRenderPostBackCards"] = "true";
			capabilities["canRenderSetvarZeroWithMultiSelectionList"] = "true";
			capabilities["canSendMail"] = "true";
			capabilities["cdf"] = "false";
			capabilities["cookies"] = "true";
			capabilities["crawler"] = "false";
			capabilities["css1"] = "false";
			capabilities["css2"] = "false";
			capabilities["defaultCharacterHeight"] = "12";
			capabilities["defaultCharacterWidth"] = "8";
			capabilities["defaultScreenCharactersHeight"] = "6";
			capabilities["defaultScreenCharactersWidth"] = "12";
			capabilities["defaultScreenPixelsHeight"] = "72";
			capabilities["defaultScreenPixelsWidth"] = "96";
			capabilities["defaultSubmitButtonLimit"] = "1";
			capabilities["ecmascriptversion"] = "0.0";
			capabilities["frames"] = "false";
			capabilities["gatewayMajorVersion"] = "0";
			capabilities["gatewayMinorVersion"] = "0";
			capabilities["gatewayVersion"] = "None";
			capabilities["gold"] = "false";
			capabilities["hasBackButton"] = "true";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "false";
			capabilities["inputType"] = "telephoneKeypad";
			capabilities["isColor"] = "false";
			capabilities["isMobileDevice"] = "false";
			capabilities["javaapplets"] = "false";
			capabilities["javascript"] = "false";
			capabilities["jscriptversion"] = "0.0";
			capabilities["majorversion"] = "0";
			capabilities["maximumHrefLength"] = "10000";
			capabilities["maximumRenderedPageSize"] = "2000";
			capabilities["maximumSoftkeyLabelLength"] = "5";
			capabilities["minorversion"] = "0";
			capabilities["mobileDeviceManufacturer"] = "Unknown";
			capabilities["mobileDeviceModel"] = "Unknown";
			capabilities["msdomversion"] = "0.0";
			capabilities["numberOfSoftkeys"] = "0";
			capabilities["platform"] = "Unknown";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "text/html";
			capabilities["preferredRenderingType"] = "html32";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["rendersBreaksAfterHtmlLists"] = "true";
			capabilities["rendersBreaksAfterWmlAnchor"] = "false";
			capabilities["rendersBreaksAfterWmlInput"] = "false";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["rendersWmlSelectsAsMenuCards"] = "false";
			capabilities["requiredMetaTagNameValue"] = "";
			capabilities["requiresAdaptiveErrorReporting"] = "false";
			capabilities["requiresAttributeColonSubstitution"] = "false";
			capabilities["requiresContentTypeMetaTag"] = "false";
			capabilities["requiresDBCSCharacter"] = "false";
			capabilities["requiresFullyQualifiedRedirectUrl"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["requiresLeadingPageBreak"] = "false";
			capabilities["requiresNoBreakInFormatting"] = "false";
			capabilities["requiresNoescapedPostUrl"] = "true";
			capabilities["requiresNoSoftkeyLabels"] = "false";
			capabilities["requiresOutputOptimization"] = "false";
			capabilities["requiresPhoneNumbersAsPlainText"] = "false";
			capabilities["requiresPostRedirectionHandling"] = "false";
			capabilities["requiresSpecialViewStateEncoding"] = "false";
			capabilities["requiresUniqueFilePathSuffix"] = "false";
			capabilities["requiresUniqueHtmlCheckboxNames"] = "false";
			capabilities["requiresUniqueHtmlInputNames"] = "false";
			capabilities["requiresUrlEncodedPostfieldValues"] = "false";
			capabilities["screenBitDepth"] = "1";
			capabilities["sk"] = "false";
			capabilities["supportsAccesskeyAttribute"] = "false";
			capabilities["supportsBodyColor"] = "true";
			capabilities["supportsBold"] = "false";
			capabilities["supportsCacheControlMetaTag"] = "true";
			capabilities["supportsCharacterEntityEncoding"] = "true";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivAlign"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsEmptyStringInCookieValue"] = "true";
			capabilities["supportsFileUpload"] = "false";
			capabilities["supportsFontColor"] = "true";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsFontSize"] = "false";
			capabilities["supportsImageSubmit"] = "false";
			capabilities["supportsIModeSymbols"] = "false";
			capabilities["supportsInputIStyle"] = "false";
			capabilities["supportsInputMode"] = "false";
			capabilities["supportsItalic"] = "false";
			capabilities["supportsJPhoneMultiMediaAttributes"] = "false";
			capabilities["supportsJPhoneSymbols"] = "false";
			capabilities["supportsMaintainScrollPositionOnPostback"] = "false";
			capabilities["supportsMultilineTextBoxDisplay"] = "false";
			capabilities["supportsQueryStringInFormAction"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["supportsSelectMultiple"] = "true";
			capabilities["supportsUncheck"] = "true";
			capabilities["supportsVCard"] = "false";
			capabilities["tables"] = "false";
			capabilities["tagwriter"] = "System.Web.UI.Html32TextWriter";
			capabilities["type"] = "Unknown";
			capabilities["vbscript"] = "false";
			capabilities["version"] = "0.0";
			capabilities["w3cdomversion"] = "0.0";
			capabilities["win16"] = "false";
			capabilities["win32"] = "false";
			capabilities["xml"] = "false";
			browserCaps.AddBrowser("Default");
			this.DefaultProcessGateways(headers, browserCaps);
			this.NokiagatewayProcess(headers, browserCaps);
			this.UpgatewayProcess(headers, browserCaps);
			this.CrawlerProcess(headers, browserCaps);
			this.ColorProcess(headers, browserCaps);
			this.MonoProcess(headers, browserCaps);
			this.PixelsProcess(headers, browserCaps);
			this.VoiceProcess(headers, browserCaps);
			this.CharsetProcess(headers, browserCaps);
			this.PlatformProcess(headers, browserCaps);
			this.WinProcess(headers, browserCaps);
			bool flag = true;
			if (!this.MozillaProcess(headers, browserCaps) && !this.DocomoProcess(headers, browserCaps) && !this.Ericssonr380Process(headers, browserCaps) && !this.EricssonProcess(headers, browserCaps) && !this.EzwapProcess(headers, browserCaps) && !this.GenericdownlevelProcess(headers, browserCaps) && !this.JataayuProcess(headers, browserCaps) && !this.JphoneProcess(headers, browserCaps) && !this.LegendProcess(headers, browserCaps) && !this.MmeProcess(headers, browserCaps) && !this.NokiaProcess(headers, browserCaps) && !this.NokiamobilebrowserrainbowProcess(headers, browserCaps) && !this.Nokiaepoc32wtlProcess(headers, browserCaps) && !this.UpProcess(headers, browserCaps) && !this.OperaProcess(headers, browserCaps) && !this.PalmscapeProcess(headers, browserCaps) && !this.AuspalmProcess(headers, browserCaps) && !this.SharppdaProcess(headers, browserCaps) && !this.PanasonicProcess(headers, browserCaps) && !this.Mspie06Process(headers, browserCaps) && !this.SktdevicesProcess(headers, browserCaps) && !this.WinwapProcess(headers, browserCaps) && !this.XiinoProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.DefaultProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00051E8B File Offset: 0x00050E8B
		protected virtual void DocomoProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00051E8D File Offset: 0x00050E8D
		protected virtual void DocomoProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00051E90 File Offset: 0x00050E90
		private bool DocomoProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^DoCoMo/"))
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "^DoCoMo/(?'httpVersion'[^/ ]*)[/ ](?'deviceID'[^/\\x28]*)");
			capabilities["browser"] = "i-mode";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["cookies"] = "false";
			capabilities["defaultScreenCharactersHeight"] = "6";
			capabilities["defaultScreenCharactersWidth"] = "16";
			capabilities["defaultScreenPixelsHeight"] = "70";
			capabilities["defaultScreenPixelsWidth"] = "90";
			capabilities["deviceID"] = regexWorker["${deviceID}"];
			capabilities["inputType"] = "telephoneKeypad";
			capabilities["isColor"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["javaapplets"] = "false";
			capabilities["javascript"] = "false";
			capabilities["maximumHrefLength"] = "524";
			capabilities["maximumRenderedPageSize"] = "5120";
			capabilities["mobileDeviceModel"] = regexWorker["${deviceID}"];
			capabilities["optimumPageWeight"] = "700";
			capabilities["preferredRenderingType"] = "chtml10";
			capabilities["preferredRequestEncoding"] = "shift_jis";
			capabilities["preferredResponseEncoding"] = "shift_jis";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["requiresFullyQualifiedRedirectUrl"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOutputOptimization"] = "true";
			capabilities["screenBitDepth"] = "1";
			capabilities["supportsAccesskeyAttribute"] = "true";
			capabilities["supportsCharacterEntityEncoding"] = "false";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsIModeSymbols"] = "true";
			capabilities["supportsInputIStyle"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["tables"] = "false";
			capabilities["type"] = "i-mode";
			capabilities["vbscript"] = "false";
			browserCaps.HtmlTextWriter = "System.Web.UI.ChtmlTextWriter";
			browserCaps.AddBrowser("Docomo");
			this.DocomoProcessGateways(headers, browserCaps);
			this.DocomorenderingsizeProcess(headers, browserCaps);
			this.DocomodefaultrenderingsizeProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Docomosh251iProcess(headers, browserCaps) && !this.Docomon251iProcess(headers, browserCaps) && !this.Docomop211iProcess(headers, browserCaps) && !this.Docomof212iProcess(headers, browserCaps) && !this.Docomod501iProcess(headers, browserCaps) && !this.Docomof501iProcess(headers, browserCaps) && !this.Docomon501iProcess(headers, browserCaps) && !this.Docomop501iProcess(headers, browserCaps) && !this.Docomod502iProcess(headers, browserCaps) && !this.Docomof502iProcess(headers, browserCaps) && !this.Docomon502iProcess(headers, browserCaps) && !this.Docomop502iProcess(headers, browserCaps) && !this.Docomonm502iProcess(headers, browserCaps) && !this.Docomoso502iProcess(headers, browserCaps) && !this.Docomof502itProcess(headers, browserCaps) && !this.Docomon502itProcess(headers, browserCaps) && !this.Docomoso502iwmProcess(headers, browserCaps) && !this.Docomof504iProcess(headers, browserCaps) && !this.Docomon504iProcess(headers, browserCaps) && !this.Docomop504iProcess(headers, browserCaps) && !this.Docomon821iProcess(headers, browserCaps) && !this.Docomop821iProcess(headers, browserCaps) && !this.Docomod209iProcess(headers, browserCaps) && !this.Docomoer209iProcess(headers, browserCaps) && !this.Docomof209iProcess(headers, browserCaps) && !this.Docomoko209iProcess(headers, browserCaps) && !this.Docomon209iProcess(headers, browserCaps) && !this.Docomop209iProcess(headers, browserCaps) && !this.Docomop209isProcess(headers, browserCaps) && !this.Docomor209iProcess(headers, browserCaps) && !this.Docomor691iProcess(headers, browserCaps) && !this.Docomof503iProcess(headers, browserCaps) && !this.Docomof503isProcess(headers, browserCaps) && !this.Docomod503iProcess(headers, browserCaps) && !this.Docomod503isProcess(headers, browserCaps) && !this.Docomod210iProcess(headers, browserCaps) && !this.Docomof210iProcess(headers, browserCaps) && !this.Docomon210iProcess(headers, browserCaps) && !this.Docomon2001Process(headers, browserCaps) && !this.Docomod211iProcess(headers, browserCaps) && !this.Docomon211iProcess(headers, browserCaps) && !this.Docomop210iProcess(headers, browserCaps) && !this.Docomoko210iProcess(headers, browserCaps) && !this.Docomop2101vProcess(headers, browserCaps) && !this.Docomop2102vProcess(headers, browserCaps) && !this.Docomof211iProcess(headers, browserCaps) && !this.Docomof671iProcess(headers, browserCaps) && !this.Docomon503isProcess(headers, browserCaps) && !this.Docomon503iProcess(headers, browserCaps) && !this.Docomoso503iProcess(headers, browserCaps) && !this.Docomop503isProcess(headers, browserCaps) && !this.Docomop503iProcess(headers, browserCaps) && !this.Docomoso210iProcess(headers, browserCaps) && !this.Docomoso503isProcess(headers, browserCaps) && !this.Docomosh821iProcess(headers, browserCaps) && !this.Docomon2002Process(headers, browserCaps) && !this.Docomoso505iProcess(headers, browserCaps) && !this.Docomop505iProcess(headers, browserCaps) && !this.Docomon505iProcess(headers, browserCaps) && !this.Docomod505iProcess(headers, browserCaps) && !this.Docomoisim60Process(headers, browserCaps))
			{
				flag = false;
			}
			this.DocomoProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0005243F File Offset: 0x0005143F
		protected virtual void DocomorenderingsizeProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00052441 File Offset: 0x00051441
		protected virtual void DocomorenderingsizeProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00052444 File Offset: 0x00051444
		private bool DocomorenderingsizeProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^DoCoMo/([^/ ]*)[/ ]([^/\\x28]*)([/\\x28]c(?'cacheSize'\\d+))"))
			{
				return false;
			}
			text = (string)capabilities["maximumRenderedPageSize"];
			if (!regexWorker.ProcessRegex(text, "^5120$"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = regexWorker["${cacheSize}000"];
			this.DocomorenderingsizeProcessGateways(headers, browserCaps);
			bool flag = false;
			this.DocomorenderingsizeProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x000524CC File Offset: 0x000514CC
		protected virtual void DocomodefaultrenderingsizeProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x000524CE File Offset: 0x000514CE
		protected virtual void DocomodefaultrenderingsizeProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x000524D0 File Offset: 0x000514D0
		private bool DocomodefaultrenderingsizeProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["maximumRenderedPageSize"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^(0|00|000)$"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "800";
			this.DocomodefaultrenderingsizeProcessGateways(headers, browserCaps);
			bool flag = false;
			this.DocomodefaultrenderingsizeProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00052534 File Offset: 0x00051534
		protected virtual void Docomosh251iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00052536 File Offset: 0x00051536
		protected virtual void Docomosh251iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x00052538 File Offset: 0x00051538
		private bool Docomosh251iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SH251i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Sharp";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["requiresLeadingPageBreak"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "130";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoSH251i");
			this.Docomosh251iProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Docomosh251isProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Docomosh251iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x00052644 File Offset: 0x00051644
		protected virtual void Docomosh251isProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00052646 File Offset: 0x00051646
		protected virtual void Docomosh251isProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00052648 File Offset: 0x00051648
		private bool Docomosh251isProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SH251iS"))
			{
				return false;
			}
			capabilities["requiresLeadingPageBreak"] = "false";
			capabilities["screenCharactersHeight"] = "11";
			capabilities["screenCharactersWidth"] = "22";
			capabilities["screenPixelsHeight"] = "220";
			capabilities["screenPixelsWidth"] = "176";
			browserCaps.AddBrowser("DocomoSH251iS");
			this.Docomosh251isProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomosh251isProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x000526F7 File Offset: 0x000516F7
		protected virtual void Docomon251iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x000526F9 File Offset: 0x000516F9
		protected virtual void Docomon251iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x000526FC File Offset: 0x000516FC
		private bool Docomon251iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N251i"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "22";
			capabilities["screenPixelsHeight"] = "140";
			capabilities["screenPixelsWidth"] = "132";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoN251i");
			this.Docomon251iProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Docomon251isProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Docomon251iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00052808 File Offset: 0x00051808
		protected virtual void Docomon251isProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0005280A File Offset: 0x0005180A
		protected virtual void Docomon251isProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0005280C File Offset: 0x0005180C
		private bool Docomon251isProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N251iS"))
			{
				return false;
			}
			browserCaps.AddBrowser("DocomoN251iS");
			this.Docomon251isProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon251isProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x0005286B File Offset: 0x0005186B
		protected virtual void Docomop211iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x0005286D File Offset: 0x0005186D
		protected virtual void Docomop211iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x00052870 File Offset: 0x00051870
		private bool Docomop211iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P211i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["MobileDeviceManufacturer"] = "Panasonic";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "130";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoP211i");
			this.Docomop211iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop211iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0005296F File Offset: 0x0005196F
		protected virtual void Docomof212iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00052971 File Offset: 0x00051971
		protected virtual void Docomof212iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00052974 File Offset: 0x00051974
		private bool Docomof212iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "F212i"))
			{
				return false;
			}
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Fujitsu";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenPixelsHeight"] = "136";
			capabilities["screenPixelsWidth"] = "132";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoF212i");
			this.Docomof212iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomof212iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x00052A63 File Offset: 0x00051A63
		protected virtual void Docomod501iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00052A65 File Offset: 0x00051A65
		protected virtual void Docomod501iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00052A68 File Offset: 0x00051A68
		private bool Docomod501iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "D501i"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "Mitsubishi";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "72";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoD501i");
			this.Docomod501iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomod501iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00052B37 File Offset: 0x00051B37
		protected virtual void Docomof501iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00052B39 File Offset: 0x00051B39
		protected virtual void Docomof501iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00052B3C File Offset: 0x00051B3C
		private bool Docomof501iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "F501i"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "Fujitsu";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "84";
			capabilities["screenPixelsWidth"] = "112";
			browserCaps.AddBrowser("DocomoF501i");
			this.Docomof501iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomof501iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00052C0B File Offset: 0x00051C0B
		protected virtual void Docomon501iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00052C0D File Offset: 0x00051C0D
		protected virtual void Docomon501iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00052C10 File Offset: 0x00051C10
		private bool Docomon501iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N501i"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "118";
			browserCaps.AddBrowser("DocomoN501i");
			this.Docomon501iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon501iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00052CDF File Offset: 0x00051CDF
		protected virtual void Docomop501iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00052CE1 File Offset: 0x00051CE1
		protected virtual void Docomop501iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00052CE4 File Offset: 0x00051CE4
		private bool Docomop501iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P501i"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "120";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoP501i");
			this.Docomop501iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop501iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x00052DB3 File Offset: 0x00051DB3
		protected virtual void Docomod502iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00052DB5 File Offset: 0x00051DB5
		protected virtual void Docomod502iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00052DB8 File Offset: 0x00051DB8
		private bool Docomod502iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "D502i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Mitsubishi";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "90";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoD502i");
			this.Docomod502iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomod502iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00052E87 File Offset: 0x00051E87
		protected virtual void Docomof502iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00052E89 File Offset: 0x00051E89
		protected virtual void Docomof502iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00052E8C File Offset: 0x00051E8C
		private bool Docomof502iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "F502i$"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Fujitsu";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "91";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoF502i");
			this.Docomof502iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomof502iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00052F5B File Offset: 0x00051F5B
		protected virtual void Docomon502iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00052F5D File Offset: 0x00051F5D
		protected virtual void Docomon502iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00052F60 File Offset: 0x00051F60
		private bool Docomon502iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N502i$"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "118";
			browserCaps.AddBrowser("DocomoN502i");
			this.Docomon502iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon502iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0005302F File Offset: 0x0005202F
		protected virtual void Docomop502iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00053031 File Offset: 0x00052031
		protected virtual void Docomop502iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00053034 File Offset: 0x00052034
		private bool Docomop502iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P502i"))
			{
				return false;
			}
			capabilities["canRenderEmptySelects"] = "false";
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "117";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoP502i");
			this.Docomop502iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop502iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00053113 File Offset: 0x00052113
		protected virtual void Docomonm502iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00053115 File Offset: 0x00052115
		protected virtual void Docomonm502iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00053118 File Offset: 0x00052118
		private bool Docomonm502iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "NM502i"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "Nokia";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "106";
			capabilities["screenPixelsWidth"] = "111";
			browserCaps.AddBrowser("DocomoNm502i");
			this.Docomonm502iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomonm502iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x000531E7 File Offset: 0x000521E7
		protected virtual void Docomoso502iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x000531E9 File Offset: 0x000521E9
		protected virtual void Docomoso502iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x000531EC File Offset: 0x000521EC
		private bool Docomoso502iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SO502i$"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "120";
			capabilities["screenPixelsWidth"] = "120";
			browserCaps.AddBrowser("DocomoSo502i");
			this.Docomoso502iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomoso502iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x000532BB File Offset: 0x000522BB
		protected virtual void Docomof502itProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x000532BD File Offset: 0x000522BD
		protected virtual void Docomof502itProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x000532C0 File Offset: 0x000522C0
		private bool Docomof502itProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "F502it"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Fujitsu";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "91";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoF502it");
			this.Docomof502itProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomof502itProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0005338F File Offset: 0x0005238F
		protected virtual void Docomon502itProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00053391 File Offset: 0x00052391
		protected virtual void Docomon502itProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x00053394 File Offset: 0x00052394
		private bool Docomon502itProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N502it"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "118";
			browserCaps.AddBrowser("DocomoN502it");
			this.Docomon502itProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon502itProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00053463 File Offset: 0x00052463
		protected virtual void Docomoso502iwmProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00053465 File Offset: 0x00052465
		protected virtual void Docomoso502iwmProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00053468 File Offset: 0x00052468
		private bool Docomoso502iwmProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SO502iWM"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "113";
			capabilities["screenPixelsWidth"] = "120";
			browserCaps.AddBrowser("DocomoSo502iwm");
			this.Docomoso502iwmProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomoso502iwmProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00053537 File Offset: 0x00052537
		protected virtual void Docomof504iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00053539 File Offset: 0x00052539
		protected virtual void Docomof504iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0005353C File Offset: 0x0005253C
		private bool Docomof504iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "F504i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Fujitsu";
			capabilities["requiresContentTypeMetaTag"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "136";
			capabilities["screenPixelsWidth"] = "132";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoF504i");
			this.Docomof504iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomof504iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0005363B File Offset: 0x0005263B
		protected virtual void Docomon504iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x0005363D File Offset: 0x0005263D
		protected virtual void Docomon504iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x00053640 File Offset: 0x00052640
		private bool Docomon504iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N504i"))
			{
				return false;
			}
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "180";
			capabilities["screenPixelsWidth"] = "160";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoN504i");
			this.Docomon504iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon504iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0005373F File Offset: 0x0005273F
		protected virtual void Docomop504iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00053741 File Offset: 0x00052741
		protected virtual void Docomop504iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00053744 File Offset: 0x00052744
		private bool Docomop504iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P504i"))
			{
				return false;
			}
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["requiresContentTypeMetaTag"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "9";
			capabilities["screenCharactersWidth"] = "22";
			capabilities["screenPixelsHeight"] = "144";
			capabilities["screenPixelsWidth"] = "132";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoP504i");
			this.Docomop504iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop504iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00053863 File Offset: 0x00052863
		protected virtual void Docomon821iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00053865 File Offset: 0x00052865
		protected virtual void Docomon821iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00053868 File Offset: 0x00052868
		private bool Docomon821iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N821i"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "119";
			browserCaps.AddBrowser("DocomoN821i");
			this.Docomon821iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon821iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00053937 File Offset: 0x00052937
		protected virtual void Docomop821iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00053939 File Offset: 0x00052939
		protected virtual void Docomop821iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0005393C File Offset: 0x0005293C
		private bool Docomop821iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P821i"))
			{
				return false;
			}
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["maximumRenderedPageSize"] = "5000";
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "118";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoP821i");
			this.Docomop821iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop821iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00053A6B File Offset: 0x00052A6B
		protected virtual void Docomod209iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00053A6D File Offset: 0x00052A6D
		protected virtual void Docomod209iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00053A70 File Offset: 0x00052A70
		private bool Docomod209iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "D209i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Mitsubishi";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "90";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoD209i");
			this.Docomod209iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomod209iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00053B3F File Offset: 0x00052B3F
		protected virtual void Docomoer209iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00053B41 File Offset: 0x00052B41
		protected virtual void Docomoer209iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00053B44 File Offset: 0x00052B44
		private bool Docomoer209iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "ER209i"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "Ericsson";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "72";
			capabilities["screenPixelsWidth"] = "120";
			browserCaps.AddBrowser("DocomoEr209i");
			this.Docomoer209iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomoer209iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00053C13 File Offset: 0x00052C13
		protected virtual void Docomof209iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00053C15 File Offset: 0x00052C15
		protected virtual void Docomof209iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00053C18 File Offset: 0x00052C18
		private bool Docomof209iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "F209i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Fujitsu";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "91";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoF209i");
			this.Docomof209iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomof209iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00053CE7 File Offset: 0x00052CE7
		protected virtual void Docomoko209iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00053CE9 File Offset: 0x00052CE9
		protected virtual void Docomoko209iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00053CEC File Offset: 0x00052CEC
		private bool Docomoko209iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KO209i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Kokusai";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "96";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoKo209i");
			this.Docomoko209iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomoko209iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00053DBB File Offset: 0x00052DBB
		protected virtual void Docomon209iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00053DBD File Offset: 0x00052DBD
		protected virtual void Docomon209iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x00053DC0 File Offset: 0x00052DC0
		private bool Docomon209iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N209i"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "82";
			capabilities["screenPixelsWidth"] = "108";
			browserCaps.AddBrowser("DocomoN209i");
			this.Docomon209iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon209iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00053E8F File Offset: 0x00052E8F
		protected virtual void Docomop209iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00053E91 File Offset: 0x00052E91
		protected virtual void Docomop209iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00053E94 File Offset: 0x00052E94
		private bool Docomop209iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P209i$"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "87";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoP209i");
			this.Docomop209iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop209iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00053F63 File Offset: 0x00052F63
		protected virtual void Docomop209isProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00053F65 File Offset: 0x00052F65
		protected virtual void Docomop209isProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00053F68 File Offset: 0x00052F68
		private bool Docomop209isProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P209iS"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "87";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoP209is");
			this.Docomop209isProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop209isProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00054037 File Offset: 0x00053037
		protected virtual void Docomor209iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00054039 File Offset: 0x00053039
		protected virtual void Docomor209iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0005403C File Offset: 0x0005303C
		private bool Docomor209iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "R209i"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "JRC";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "72";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoR209i");
			this.Docomor209iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomor209iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0005410B File Offset: 0x0005310B
		protected virtual void Docomor691iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0005410D File Offset: 0x0005310D
		protected virtual void Docomor691iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00054110 File Offset: 0x00053110
		private bool Docomor691iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "R691i"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "JRC";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "72";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoR691i");
			this.Docomor691iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomor691iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x000541DF File Offset: 0x000531DF
		protected virtual void Docomof503iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x000541E1 File Offset: 0x000531E1
		protected virtual void Docomof503iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x000541E4 File Offset: 0x000531E4
		private bool Docomof503iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "F503i$"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Fujitsu";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "130";
			capabilities["screenPixelsWidth"] = "120";
			browserCaps.AddBrowser("DocomoF503i");
			this.Docomof503iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomof503iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x000542B3 File Offset: 0x000532B3
		protected virtual void Docomof503isProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x000542B5 File Offset: 0x000532B5
		protected virtual void Docomof503isProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x000542B8 File Offset: 0x000532B8
		private bool Docomof503isProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "F503iS"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Fujitsu";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "12";
			capabilities["screenCharactersWidth"] = "24";
			capabilities["screenPixelsHeight"] = "130";
			capabilities["screenPixelsWidth"] = "120";
			browserCaps.AddBrowser("DocomoF503is");
			this.Docomof503isProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomof503isProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00054387 File Offset: 0x00053387
		protected virtual void Docomod503iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x00054389 File Offset: 0x00053389
		protected virtual void Docomod503iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0005438C File Offset: 0x0005338C
		private bool Docomod503iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "D503i$"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Mitsubishi";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "126";
			capabilities["screenPixelsWidth"] = "132";
			browserCaps.AddBrowser("DocomoD503i");
			this.Docomod503iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomod503iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x0005445B File Offset: 0x0005345B
		protected virtual void Docomod503isProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0005445D File Offset: 0x0005345D
		protected virtual void Docomod503isProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00054460 File Offset: 0x00053460
		private bool Docomod503isProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "D503iS$"))
			{
				return false;
			}
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Mitsubishi";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "126";
			capabilities["screenPixelsWidth"] = "132";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoD503is");
			this.Docomod503isProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomod503isProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0005456F File Offset: 0x0005356F
		protected virtual void Docomod210iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00054571 File Offset: 0x00053571
		protected virtual void Docomod210iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00054574 File Offset: 0x00053574
		private bool Docomod210iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "D210i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Mitsubishi";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "91";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoD210i");
			this.Docomod210iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomod210iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00054643 File Offset: 0x00053643
		protected virtual void Docomof210iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00054645 File Offset: 0x00053645
		protected virtual void Docomof210iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00054648 File Offset: 0x00053648
		private bool Docomof210iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "F210i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Fujitsu";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "113";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoF210i");
			this.Docomof210iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomof210iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00054717 File Offset: 0x00053717
		protected virtual void Docomon210iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00054719 File Offset: 0x00053719
		protected virtual void Docomon210iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0005471C File Offset: 0x0005371C
		private bool Docomon210iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N210i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "113";
			capabilities["screenPixelsWidth"] = "118";
			browserCaps.AddBrowser("DocomoN210i");
			this.Docomon210iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon210iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x000547EB File Offset: 0x000537EB
		protected virtual void Docomon2001ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x000547ED File Offset: 0x000537ED
		protected virtual void Docomon2001ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x000547F0 File Offset: 0x000537F0
		private bool Docomon2001Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N2001"))
			{
				return false;
			}
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "118";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoN2001");
			this.Docomon2001ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon2001ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x000548FF File Offset: 0x000538FF
		protected virtual void Docomod211iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00054901 File Offset: 0x00053901
		protected virtual void Docomod211iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00054904 File Offset: 0x00053904
		private bool Docomod211iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "D211i"))
			{
				return false;
			}
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Mitsubishi";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "91";
			capabilities["screenPixelsWidth"] = "100";
			browserCaps.AddBrowser("DocomoD211i");
			this.Docomod211iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomod211iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00054A03 File Offset: 0x00053A03
		protected virtual void Docomon211iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00054A05 File Offset: 0x00053A05
		protected virtual void Docomon211iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00054A08 File Offset: 0x00053A08
		private bool Docomon211iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N211i"))
			{
				return false;
			}
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "118";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoN211i");
			this.Docomon211iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon211iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00054B17 File Offset: 0x00053B17
		protected virtual void Docomop210iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00054B19 File Offset: 0x00053B19
		protected virtual void Docomop210iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00054B1C File Offset: 0x00053B1C
		private bool Docomop210iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P210i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "91";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoP210i");
			this.Docomop210iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop210iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00054BEB File Offset: 0x00053BEB
		protected virtual void Docomoko210iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00054BED File Offset: 0x00053BED
		protected virtual void Docomoko210iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00054BF0 File Offset: 0x00053BF0
		private bool Docomoko210iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KO210i"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Kokusai";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "96";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("DocomoKo210i");
			this.Docomoko210iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomoko210iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00054CBF File Offset: 0x00053CBF
		protected virtual void Docomop2101vProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00054CC1 File Offset: 0x00053CC1
		protected virtual void Docomop2101vProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00054CC4 File Offset: 0x00053CC4
		private bool Docomop2101vProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P2101V"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["screenBitDepth"] = "18";
			capabilities["screenCharactersHeight"] = "9";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "182";
			capabilities["screenPixelsWidth"] = "163";
			browserCaps.AddBrowser("DocomoP2101v");
			this.Docomop2101vProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop2101vProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00054D93 File Offset: 0x00053D93
		protected virtual void Docomop2102vProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00054D95 File Offset: 0x00053D95
		protected virtual void Docomop2102vProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00054D98 File Offset: 0x00053D98
		private bool Docomop2102vProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P2102V"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			browserCaps.AddBrowser("DocomoP2102v");
			this.Docomop2102vProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop2102vProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00054E07 File Offset: 0x00053E07
		protected virtual void Docomof211iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x00054E09 File Offset: 0x00053E09
		protected virtual void Docomof211iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00054E0C File Offset: 0x00053E0C
		private bool Docomof211iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "F211i$"))
			{
				return false;
			}
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Fujitsu";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "113";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoF211i");
			this.Docomof211iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomof211iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00054F1B File Offset: 0x00053F1B
		protected virtual void Docomof671iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00054F1D File Offset: 0x00053F1D
		protected virtual void Docomof671iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00054F20 File Offset: 0x00053F20
		private bool Docomof671iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "F671i$"))
			{
				return false;
			}
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Fujitsu";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "9";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "126";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoF671i");
			this.Docomof671iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomof671iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0005502F File Offset: 0x0005402F
		protected virtual void Docomon503isProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00055031 File Offset: 0x00054031
		protected virtual void Docomon503isProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00055034 File Offset: 0x00054034
		private bool Docomon503isProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N503iS"))
			{
				return false;
			}
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "118";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoN503is");
			this.Docomon503isProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon503isProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00055143 File Offset: 0x00054143
		protected virtual void Docomon503iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00055145 File Offset: 0x00054145
		protected virtual void Docomon503iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00055148 File Offset: 0x00054148
		private bool Docomon503iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N503i$"))
			{
				return false;
			}
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "118";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoN503i");
			this.Docomon503iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon503iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00055257 File Offset: 0x00054257
		protected virtual void Docomoso503iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00055259 File Offset: 0x00054259
		protected virtual void Docomoso503iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0005525C File Offset: 0x0005425C
		private bool Docomoso503iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SO503i$"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "113";
			capabilities["screenPixelsWidth"] = "120";
			browserCaps.AddBrowser("DocomoSo503i");
			this.Docomoso503iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomoso503iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0005532B File Offset: 0x0005432B
		protected virtual void Docomop503isProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0005532D File Offset: 0x0005432D
		protected virtual void Docomop503isProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00055330 File Offset: 0x00054330
		private bool Docomop503isProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P503iS"))
			{
				return false;
			}
			capabilities["canRenderEmptySelects"] = "false";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "130";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoP503is");
			this.Docomop503isProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop503isProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0005546F File Offset: 0x0005446F
		protected virtual void Docomop503iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00055471 File Offset: 0x00054471
		protected virtual void Docomop503iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00055474 File Offset: 0x00054474
		private bool Docomop503iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P503i$"))
			{
				return false;
			}
			capabilities["canRenderEmptySelects"] = "false";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["rendersBreaksAfterHtmlLists"] = "false";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "130";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoP503i");
			this.Docomop503iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop503iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x000555B3 File Offset: 0x000545B3
		protected virtual void Docomoso210iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x000555B5 File Offset: 0x000545B5
		protected virtual void Docomoso210iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x000555B8 File Offset: 0x000545B8
		private bool Docomoso210iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SO210i$"))
			{
				return false;
			}
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "17";
			capabilities["screenPixelsHeight"] = "113";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoSo210i");
			this.Docomoso210iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomoso210iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x000556C7 File Offset: 0x000546C7
		protected virtual void Docomoso503isProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x000556C9 File Offset: 0x000546C9
		protected virtual void Docomoso503isProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x000556CC File Offset: 0x000546CC
		private bool Docomoso503isProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SO503iS"))
			{
				return false;
			}
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "17";
			capabilities["screenPixelsHeight"] = "113";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoSo503is");
			this.Docomoso503isProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomoso503isProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x000557DB File Offset: 0x000547DB
		protected virtual void Docomosh821iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x000557DD File Offset: 0x000547DD
		protected virtual void Docomosh821iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x000557E0 File Offset: 0x000547E0
		private bool Docomosh821iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SH821i"))
			{
				return false;
			}
			capabilities["canRenderEmptySelects"] = "false";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Sharp";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "78";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoSh821i");
			this.Docomosh821iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomosh821iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x000558DF File Offset: 0x000548DF
		protected virtual void Docomon2002ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x000558E1 File Offset: 0x000548E1
		protected virtual void Docomon2002ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x000558E4 File Offset: 0x000548E4
		private bool Docomon2002Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N2002"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "NEC";
			capabilities["requiresContentTypeMetaTag"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "118";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("DocomoN2002");
			this.Docomon2002ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon2002ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x000559E3 File Offset: 0x000549E3
		protected virtual void Docomoso505iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x000559E5 File Offset: 0x000549E5
		protected virtual void Docomoso505iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x000559E8 File Offset: 0x000549E8
		private bool Docomoso505iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SO505i"))
			{
				return false;
			}
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "SonyEricsson";
			capabilities["screenBitDepth"] = "18";
			capabilities["screenCharactersHeight"] = "9";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "240";
			capabilities["screenPixelsWidth"] = "256";
			browserCaps.AddBrowser("DocomoSo505i");
			this.Docomoso505iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomoso505iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x00055AC7 File Offset: 0x00054AC7
		protected virtual void Docomop505iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x00055AC9 File Offset: 0x00054AC9
		protected virtual void Docomop505iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00055ACC File Offset: 0x00054ACC
		private bool Docomop505iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P505i"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			browserCaps.AddBrowser("DocomoP505i");
			this.Docomop505iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomop505iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x00055B3B File Offset: 0x00054B3B
		protected virtual void Docomon505iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00055B3D File Offset: 0x00054B3D
		protected virtual void Docomon505iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00055B40 File Offset: 0x00054B40
		private bool Docomon505iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "N505i"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "NEC";
			browserCaps.AddBrowser("DocomoN505i");
			this.Docomon505iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomon505iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x00055BAF File Offset: 0x00054BAF
		protected virtual void Docomod505iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x00055BB1 File Offset: 0x00054BB1
		protected virtual void Docomod505iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x00055BB4 File Offset: 0x00054BB4
		private bool Docomod505iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "D505i"))
			{
				return false;
			}
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Mitsubishi";
			capabilities["screenBitDepth"] = "18";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "270";
			capabilities["screenPixelsWidth"] = "240";
			browserCaps.AddBrowser("DocomoD505i");
			this.Docomod505iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomod505iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x00055C93 File Offset: 0x00054C93
		protected virtual void Docomoisim60ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x00055C95 File Offset: 0x00054C95
		protected virtual void Docomoisim60ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x00055C98 File Offset: 0x00054C98
		private bool Docomoisim60Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "ISIM60"))
			{
				return false;
			}
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "true";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "NTT DoCoMo";
			capabilities["mobileDeviceModel"] = "i-mode HTML Simulator";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "180";
			capabilities["screenPixelsWidth"] = "160";
			capabilities["supportsEmptyStringInCookieValue"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["supportsSelectMultiple"] = "false";
			browserCaps.AddBrowser("DocomoISIM60");
			this.Docomoisim60ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Docomoisim60ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x00055DE7 File Offset: 0x00054DE7
		protected virtual void Ericssonr380ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x00055DE9 File Offset: 0x00054DE9
		protected virtual void Ericssonr380ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00055DEC File Offset: 0x00054DEC
		private bool Ericssonr380Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "R380 (?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*) WAP1\\.1"))
			{
				return false;
			}
			capabilities["browser"] = "Ericsson";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["cookies"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "3000";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceManufacturer"] = "Ericsson";
			capabilities["mobileDeviceModel"] = "R380";
			capabilities["requiresNoescapedPostUrl"] = "false";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml11";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenPixelsHeight"] = "100";
			capabilities["screenPixelsWidth"] = "310";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["type"] = "Ericsson R380";
			capabilities["version"] = regexWorker["${browserMajorVersion}.${browserMinorVersion}"];
			browserCaps.AddBrowser("EricssonR380");
			this.Ericssonr380ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssonr380ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00055FD8 File Offset: 0x00054FD8
		protected virtual void EricssonProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00055FDA File Offset: 0x00054FDA
		protected virtual void EricssonProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00055FDC File Offset: 0x00054FDC
		private bool EricssonProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Ericsson(?'deviceID'[^/]+)/(?'deviceVer'.*)"))
			{
				return false;
			}
			capabilities["browser"] = "Ericsson";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["cookies"] = "false";
			capabilities["defaultScreenCharactersHeight"] = "4";
			capabilities["defaultScreenCharactersWidth"] = "20";
			capabilities["defaultScreenPixelsHeight"] = "52";
			capabilities["defaultScreenPixelsWidth"] = "101";
			capabilities["inputType"] = "telephoneKeypad";
			capabilities["isColor"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["maximumRenderedPageSize"] = "1600";
			capabilities["mobileDeviceManufacturer"] = "Ericsson";
			capabilities["mobileDeviceModel"] = regexWorker["${deviceID}"];
			capabilities["mobileDeviceVersion"] = regexWorker["${deviceVer}"];
			capabilities["numberOfSoftkeys"] = "2";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml11";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["screenBitDepth"] = "1";
			capabilities["type"] = regexWorker["Ericsson ${deviceID}"];
			browserCaps.HtmlTextWriter = "System.Web.UI.XhtmlTextWriter";
			browserCaps.AddBrowser("Ericsson");
			this.EricssonProcessGateways(headers, browserCaps);
			this.SonyericssonProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Ericssonr320Process(headers, browserCaps) && !this.Ericssont20Process(headers, browserCaps) && !this.Ericssont65Process(headers, browserCaps) && !this.Ericssont68Process(headers, browserCaps) && !this.Ericssont300Process(headers, browserCaps) && !this.Ericssonp800Process(headers, browserCaps) && !this.Ericssont61Process(headers, browserCaps) && !this.Ericssont31Process(headers, browserCaps) && !this.Ericssonr520Process(headers, browserCaps) && !this.Ericssona2628Process(headers, browserCaps) && !this.Ericssont39Process(headers, browserCaps))
			{
				flag = false;
			}
			this.EricssonProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0005621D File Offset: 0x0005521D
		protected virtual void SonyericssonProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0005621F File Offset: 0x0005521F
		protected virtual void SonyericssonProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x00056224 File Offset: 0x00055224
		private bool SonyericssonProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^SonyEricsson"))
			{
				return false;
			}
			capabilities["browser"] = "Sony Ericsson";
			capabilities["mobileDeviceManufacturer"] = "Sony Ericsson";
			capabilities["type"] = regexWorker["Sony Ericsson ${mobileDeviceModel}"];
			this.SonyericssonProcessGateways(headers, browserCaps);
			bool flag = false;
			this.SonyericssonProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x000562A9 File Offset: 0x000552A9
		protected virtual void Ericssonr320ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x000562AB File Offset: 0x000552AB
		protected virtual void Ericssonr320ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x000562B0 File Offset: 0x000552B0
		private bool Ericssonr320Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "R320"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "3000";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "52";
			capabilities["screenPixelsWidth"] = "101";
			browserCaps.AddBrowser("EricssonR320");
			this.Ericssonr320ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssonr320ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x0005635F File Offset: 0x0005535F
		protected virtual void Ericssont20ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00056361 File Offset: 0x00055361
		protected virtual void Ericssont20ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00056364 File Offset: 0x00055364
		private bool Ericssont20Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "T20"))
			{
				return false;
			}
			capabilities["canSendMail"] = "false";
			capabilities["maximumRenderedPageSize"] = "1400";
			capabilities["maximumSoftkeyLabelLength"] = "21";
			capabilities["mobileDeviceModel"] = "T20, T20e, T29s";
			capabilities["numberOfSoftkeys"] = "1";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenCharactersHeight"] = "3";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "33";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("EricssonT20");
			this.Ericssont20ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssont20ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00056493 File Offset: 0x00055493
		protected virtual void Ericssont65ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x00056495 File Offset: 0x00055495
		protected virtual void Ericssont65ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x00056498 File Offset: 0x00055498
		private bool Ericssont65Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "T65"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "3000";
			capabilities["maximumSoftkeyLabelLength"] = "21";
			capabilities["mobileDeviceModel"] = "Ericsson T65";
			capabilities["numberOfSoftkeys"] = "1";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "67";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("EricssonT65");
			this.Ericssont65ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssont65ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000565E7 File Offset: 0x000555E7
		protected virtual void Ericssont68ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x000565E9 File Offset: 0x000555E9
		protected virtual void Ericssont68ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x000565EC File Offset: 0x000555EC
		private bool Ericssont68Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "T68"))
			{
				return false;
			}
			capabilities["canSendMail"] = "false";
			capabilities["isColor"] = "true";
			capabilities["numberOfSoftkeys"] = "1";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["rendersWmlDoAcceptsInline"] = "false";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "80";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontSize"] = "true";
			browserCaps.AddBrowser("EricssonT68");
			this.Ericssont68ProcessGateways(headers, browserCaps);
			this.Ericssont68upgatewayProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Ericsson301aProcess(headers, browserCaps) && !this.Ericssont68r1aProcess(headers, browserCaps) && !this.Ericssont68r101Process(headers, browserCaps) && !this.Ericssont68r201aProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Ericssont68ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0005674F File Offset: 0x0005574F
		protected virtual void Ericssont68upgatewayProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00056751 File Offset: 0x00055751
		protected virtual void Ericssont68upgatewayProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00056754 File Offset: 0x00055754
		private bool Ericssont68upgatewayProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "UP\\.Link"))
			{
				return false;
			}
			capabilities["cookies"] = "true";
			this.Ericssont68upgatewayProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssont68upgatewayProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x000567B3 File Offset: 0x000557B3
		protected virtual void Ericsson301aProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000567B5 File Offset: 0x000557B5
		protected virtual void Ericsson301aProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x000567B8 File Offset: 0x000557B8
		private bool Ericsson301aProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "R301A"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["canSendMail"] = "true";
			capabilities["displaysAccessKeysAutomatically"] = "true";
			capabilities["maximumRenderedPageSize"] = "2800";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresNewLineSuppression"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "true";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenPixelsHeight"] = "72";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsBold"] = "false";
			capabilities["supportsDivAlign"] = "false";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsFontSize"] = "false";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "false";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["usePOverDiv"] = "true";
			browserCaps.AddBrowser("Ericsson301A");
			this.Ericsson301aProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericsson301aProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x000569E7 File Offset: 0x000559E7
		protected virtual void Ericssont300ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x000569E9 File Offset: 0x000559E9
		protected virtual void Ericssont300ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x000569EC File Offset: 0x000559EC
		private bool Ericssont300Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "T300"))
			{
				return false;
			}
			capabilities["breaksOnBlockElements"] = "false";
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["cookies"] = "true";
			capabilities["displaysAccessKeysAutomatically"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "2800";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "true";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "15";
			capabilities["screenPixelsHeight"] = "72";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsDivAlign"] = "false";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "false";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["supportsWtai"] = "true";
			capabilities["tables"] = "false";
			browserCaps.AddBrowser("EricssonT300");
			this.Ericssont300ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssont300ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x00056C4B File Offset: 0x00055C4B
		protected virtual void Ericssonp800ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x00056C4D File Offset: 0x00055C4D
		protected virtual void Ericssonp800ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x00056C50 File Offset: 0x00055C50
		private bool Ericssonp800Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P800"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "Sony Ericsson";
			capabilities["cookies"] = "true";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Sony Ericsson";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "16";
			capabilities["screenCharactersWidth"] = "28";
			capabilities["screenPixelsHeight"] = "320";
			capabilities["screenPixelsWidth"] = "208";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("EricssonP800");
			this.Ericssonp800ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Ericssonp800r101Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Ericssonp800ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00056E9C File Offset: 0x00055E9C
		protected virtual void Ericssonp800r101ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00056E9E File Offset: 0x00055E9E
		protected virtual void Ericssonp800r101ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00056EA0 File Offset: 0x00055EA0
		private bool Ericssonp800r101Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "R101"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "10000";
			browserCaps.AddBrowser("EricssonP800R101");
			this.Ericssonp800r101ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssonp800r101ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00056F0F File Offset: 0x00055F0F
		protected virtual void Ericssont61ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00056F11 File Offset: 0x00055F11
		protected virtual void Ericssont61ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00056F14 File Offset: 0x00055F14
		private bool Ericssont61Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "T610|T616|T618"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["cookies"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "9800";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "true";
			capabilities["requiredOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "128";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsNoWrapStyle"] = "false";
			capabilities["supportsSelectFollowingTable"] = "false";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			browserCaps.AddBrowser("EricssonT61");
			this.Ericssont61ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssont61ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00057163 File Offset: 0x00056163
		protected virtual void Ericssont31ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00057165 File Offset: 0x00056165
		protected virtual void Ericssont31ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x00057168 File Offset: 0x00056168
		private bool Ericssont31Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "T310|T312|T316"))
			{
				return false;
			}
			capabilities["breaksOnBlockElements"] = "false";
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["cookies"] = "true";
			capabilities["displaysAccessKeysAutomatically"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "2800";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "true";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "15";
			capabilities["screenPixelsHeight"] = "80";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsDivAlign"] = "false";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsNoWrapStyle"] = "false";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "false";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			browserCaps.AddBrowser("EricssonT31");
			this.Ericssont31ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssont31ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x000573B7 File Offset: 0x000563B7
		protected virtual void Ericssont68r1aProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x000573B9 File Offset: 0x000563B9
		protected virtual void Ericssont68r1aProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x000573BC File Offset: 0x000563BC
		private bool Ericssont68r1aProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "R1A"))
			{
				return false;
			}
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["maximumRenderedPageSize"] = "5000";
			capabilities["maximumSoftkeyLabelLength"] = "14";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			browserCaps.AddBrowser("EricssonT68R1A");
			this.Ericssont68r1aProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssont68r1aProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0005746B File Offset: 0x0005646B
		protected virtual void Ericssont68r101ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0005746D File Offset: 0x0005646D
		protected virtual void Ericssont68r101ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00057470 File Offset: 0x00056470
		private bool Ericssont68r101Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "R101"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2900";
			capabilities["maximumSoftkeyLabelLength"] = "21";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["requiresNoSoftkeyLabels"] = "true";
			browserCaps.AddBrowser("EricssonT68R101");
			this.Ericssont68r101ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssont68r101ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0005750F File Offset: 0x0005650F
		protected virtual void Ericssont68r201aProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00057511 File Offset: 0x00056511
		protected virtual void Ericssont68r201aProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00057514 File Offset: 0x00056514
		private bool Ericssont68r201aProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "R201A"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["maximumRenderedPageSize"] = "2900";
			capabilities["maximumSoftkeyLabelLength"] = "21";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["screenBitDepth"] = "24";
			browserCaps.AddBrowser("EricssonT68R201A");
			this.Ericssont68r201aProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssont68r201aProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x000575D3 File Offset: 0x000565D3
		protected virtual void Ericssonr520ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x000575D5 File Offset: 0x000565D5
		protected virtual void Ericssonr520ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x000575D8 File Offset: 0x000565D8
		private bool Ericssonr520Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "R520"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "1600";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "67";
			capabilities["screenPixelsWidth"] = "101";
			browserCaps.AddBrowser("EricssonR520");
			this.Ericssonr520ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssonr520ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x00057697 File Offset: 0x00056697
		protected virtual void Ericssona2628ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00057699 File Offset: 0x00056699
		protected virtual void Ericssona2628ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0005769C File Offset: 0x0005669C
		private bool Ericssona2628Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "A2628"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "1600";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "54";
			capabilities["screenPixelsWidth"] = "101";
			browserCaps.AddBrowser("EricssonA2628");
			this.Ericssona2628ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssona2628ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0005774B File Offset: 0x0005674B
		protected virtual void Ericssont39ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0005774D File Offset: 0x0005674D
		protected virtual void Ericssont39ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00057750 File Offset: 0x00056750
		private bool Ericssont39Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "T39"))
			{
				return false;
			}
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["inputType"] = "telephoneKeypad";
			capabilities["maximumRenderedPageSize"] = "3000";
			capabilities["maximumSoftkeyLabelLength"] = "21";
			capabilities["mobileDeviceManufacturer"] = "Ericsson";
			capabilities["mobileDeviceModel"] = "Ericsson T39";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "3";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "54";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "false";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("EricssonT39");
			this.Ericssont39ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ericssont39ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x000578BF File Offset: 0x000568BF
		protected virtual void EzwapProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x000578C1 File Offset: 0x000568C1
		protected virtual void EzwapProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x000578C4 File Offset: 0x000568C4
		private bool EzwapProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "EzWAP (?'browserMajorVersion'\\d+)(?'browserMinorVersion'\\.\\d+)"))
			{
				return false;
			}
			capabilities["browser"] = "EzWAP";
			capabilities["canSendMail"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["requiresXhtmlCssSuppression"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "12";
			capabilities["screenCharactersWidth"] = "33";
			capabilities["screenPixelsHeight"] = "320";
			capabilities["screenPixelsWidth"] = "240";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsDivAlign"] = "false";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsStyleElement"] = "false";
			capabilities["supportsUrlAttributeEncoding"] = "false";
			capabilities["tables"] = "true";
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.AddBrowser("EzWAP");
			this.EzwapProcessGateways(headers, browserCaps);
			bool flag = false;
			this.EzwapProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00057AA0 File Offset: 0x00056AA0
		protected virtual void NokiagatewayProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00057AA2 File Offset: 0x00056AA2
		protected virtual void NokiagatewayProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00057AA4 File Offset: 0x00056AA4
		private bool NokiagatewayProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["VIA"];
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			text = headers["VIA"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'nokiaVersion'Nokia\\D*(?'gatewayMajorVersion'\\d+)(?'gatewayMinorVersion'\\.\\d+)[^,]*)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["gatewayMajorVersion"] = regexWorker["${gatewayMajorVersion}"];
			capabilities["gatewayMinorVersion"] = regexWorker["${gatewayMinorVersion}"];
			capabilities["gatewayVersion"] = regexWorker["${nokiaVersion}"];
			this.NokiagatewayProcessGateways(headers, browserCaps);
			bool flag = false;
			this.NokiagatewayProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x00057B51 File Offset: 0x00056B51
		protected virtual void UpgatewayProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00057B53 File Offset: 0x00056B53
		protected virtual void UpgatewayProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00057B58 File Offset: 0x00056B58
		private bool UpgatewayProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "UP\\.Link/"))
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "(?'goWebUPGateway'Go\\.Web)");
			capabilities["isGoWebUpGateway"] = regexWorker["${goWebUPGateway}"];
			this.UpgatewayProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.UpnongogatewayProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.UpgatewayProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00057BE1 File Offset: 0x00056BE1
		protected virtual void UpnongogatewayProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00057BE3 File Offset: 0x00056BE3
		protected virtual void UpnongogatewayProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00057BE8 File Offset: 0x00056BE8
		private bool UpnongogatewayProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "UP\\.Link/(?'gatewayMajorVersion'\\d*)(?'gatewayMinorVersion'\\.\\d*)(?'other'\\S*)"))
			{
				return false;
			}
			text = (string)capabilities["isGoWebUpGateway"];
			if (!regexWorker.ProcessRegex(text, "^$"))
			{
				return false;
			}
			capabilities["gatewayMajorVersion"] = regexWorker["${gatewayMajorVersion}"];
			capabilities["gatewayMinorVersion"] = regexWorker["${gatewayMinorVersion}"];
			capabilities["gatewayVersion"] = regexWorker["UP.Link/${gatewayMajorVersion}${gatewayMinorVersion}${other}"];
			this.UpnongogatewayProcessGateways(headers, browserCaps);
			bool flag = false;
			this.UpnongogatewayProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00057C9C File Offset: 0x00056C9C
		protected virtual void CrawlerProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x00057C9E File Offset: 0x00056C9E
		protected virtual void CrawlerProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00057CA0 File Offset: 0x00056CA0
		private bool CrawlerProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "crawler|Crawler|Googlebot|msnbot"))
			{
				return false;
			}
			capabilities["crawler"] = "true";
			this.CrawlerProcessGateways(headers, browserCaps);
			bool flag = false;
			this.CrawlerProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x00057CFF File Offset: 0x00056CFF
		protected virtual void ColorProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00057D01 File Offset: 0x00056D01
		protected virtual void ColorProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00057D04 File Offset: 0x00056D04
		private bool ColorProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["UA-COLOR"];
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			text = headers["UA-COLOR"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "color(?'colorDepth'\\d+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["isColor"] = "true";
			capabilities["screenBitDepth"] = regexWorker["${colorDepth}"];
			this.ColorProcessGateways(headers, browserCaps);
			bool flag = false;
			this.ColorProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00057D95 File Offset: 0x00056D95
		protected virtual void MonoProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00057D97 File Offset: 0x00056D97
		protected virtual void MonoProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00057D9C File Offset: 0x00056D9C
		private bool MonoProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["UA-COLOR"];
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			text = headers["UA-COLOR"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "mono(?'colorDepth'\\d+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["isColor"] = "false";
			capabilities["screenBitDepth"] = regexWorker["${colorDepth}"];
			this.MonoProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MonoProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00057E2D File Offset: 0x00056E2D
		protected virtual void PixelsProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x00057E2F File Offset: 0x00056E2F
		protected virtual void PixelsProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00057E34 File Offset: 0x00056E34
		private bool PixelsProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["UA-PIXELS"];
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			text = headers["UA-PIXELS"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'screenWidth'\\d+)x(?'screenHeight'\\d+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["screenPixelsHeight"] = regexWorker["${screenHeight}"];
			capabilities["screenPixelsWidth"] = regexWorker["${screenWidth}"];
			this.PixelsProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PixelsProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00057ECB File Offset: 0x00056ECB
		protected virtual void VoiceProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x00057ECD File Offset: 0x00056ECD
		protected virtual void VoiceProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x00057ED0 File Offset: 0x00056ED0
		private bool VoiceProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["UA-VOICE"];
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			text = headers["UA-VOICE"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?i:TRUE)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["canInitiateVoiceCall"] = "true";
			this.VoiceProcessGateways(headers, browserCaps);
			bool flag = false;
			this.VoiceProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x00057F4B File Offset: 0x00056F4B
		protected virtual void CharsetProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00057F4D File Offset: 0x00056F4D
		protected virtual void CharsetProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00057F50 File Offset: 0x00056F50
		private bool CharsetProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-CHARSET"];
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			text = headers["X-UP-DEVCAP-CHARSET"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?i)^Shift_JIS$"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			if (!regexWorker.ProcessRegex(text, "(UP/\\S* UP\\.Browser/3\\.\\[3-9]d*)|(UP\\.Browser/3\\.\\[3-9]d*)|(UP\\.Browser/3\\.\\[3-9]d*)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["canSendMail"] = "true";
			this.CharsetProcessGateways(headers, browserCaps);
			bool flag = false;
			this.CharsetProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00057FE9 File Offset: 0x00056FE9
		protected virtual void PlatformProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00057FEB File Offset: 0x00056FEB
		protected virtual void PlatformProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00057FF0 File Offset: 0x00056FF0
		private bool PlatformProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			this.PlatformProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.PlatformwinntProcess(headers, browserCaps) && !this.Platformwin2000bProcess(headers, browserCaps) && !this.Platformwin95Process(headers, browserCaps) && !this.Platformwin98Process(headers, browserCaps) && !this.Platformwin16Process(headers, browserCaps) && !this.PlatformwinceProcess(headers, browserCaps) && !this.Platformmac68kProcess(headers, browserCaps) && !this.PlatformmacppcProcess(headers, browserCaps) && !this.PlatformunixProcess(headers, browserCaps) && !this.PlatformwebtvProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.PlatformProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00058094 File Offset: 0x00057094
		protected virtual void PlatformwinntProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00058096 File Offset: 0x00057096
		protected virtual void PlatformwinntProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x00058098 File Offset: 0x00057098
		private bool PlatformwinntProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Windows NT|WinNT|Windows XP"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			bool flag = regexWorker.ProcessRegex(text, "WinCE|Windows CE");
			if (flag)
			{
				return false;
			}
			capabilities["platform"] = "WinNT";
			this.PlatformwinntProcessGateways(headers, browserCaps);
			bool flag2 = true;
			if (!this.PlatformwinxpProcess(headers, browserCaps) && !this.Platformwin2000aProcess(headers, browserCaps))
			{
				flag2 = false;
			}
			this.PlatformwinntProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0005812C File Offset: 0x0005712C
		protected virtual void PlatformwinxpProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0005812E File Offset: 0x0005712E
		protected virtual void PlatformwinxpProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00058130 File Offset: 0x00057130
		private bool PlatformwinxpProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Windows (NT 5\\.1|XP)"))
			{
				return false;
			}
			capabilities["platform"] = "WinXP";
			this.PlatformwinxpProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PlatformwinxpProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0005818F File Offset: 0x0005718F
		protected virtual void Platformwin2000aProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00058191 File Offset: 0x00057191
		protected virtual void Platformwin2000aProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00058194 File Offset: 0x00057194
		private bool Platformwin2000aProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Windows NT 5\\.0"))
			{
				return false;
			}
			capabilities["platform"] = "Win2000";
			this.Platformwin2000aProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Platformwin2000aProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x000581F3 File Offset: 0x000571F3
		protected virtual void Platformwin2000bProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x000581F5 File Offset: 0x000571F5
		protected virtual void Platformwin2000bProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x000581F8 File Offset: 0x000571F8
		private bool Platformwin2000bProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Windows 2000"))
			{
				return false;
			}
			capabilities["platform"] = "Win2000";
			this.Platformwin2000bProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Platformwin2000bProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00058257 File Offset: 0x00057257
		protected virtual void Platformwin95ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00058259 File Offset: 0x00057259
		protected virtual void Platformwin95ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x0005825C File Offset: 0x0005725C
		private bool Platformwin95Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Win(dows )?95"))
			{
				return false;
			}
			capabilities["platform"] = "Win95";
			this.Platformwin95ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Platformwin95ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x000582BB File Offset: 0x000572BB
		protected virtual void Platformwin98ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x000582BD File Offset: 0x000572BD
		protected virtual void Platformwin98ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x000582C0 File Offset: 0x000572C0
		private bool Platformwin98Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Win(dows )?98"))
			{
				return false;
			}
			capabilities["platform"] = "Win98";
			this.Platformwin98ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Platformwin98ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0005831F File Offset: 0x0005731F
		protected virtual void Platformwin16ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00058321 File Offset: 0x00057321
		protected virtual void Platformwin16ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00058324 File Offset: 0x00057324
		private bool Platformwin16Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Win(dows 3\\.1|16)"))
			{
				return false;
			}
			capabilities["platform"] = "Win16";
			this.Platformwin16ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Platformwin16ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00058383 File Offset: 0x00057383
		protected virtual void PlatformwinceProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00058385 File Offset: 0x00057385
		protected virtual void PlatformwinceProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00058388 File Offset: 0x00057388
		private bool PlatformwinceProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Win(dows )?CE"))
			{
				return false;
			}
			capabilities["platform"] = "WinCE";
			this.PlatformwinceProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PlatformwinceProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x000583E7 File Offset: 0x000573E7
		protected virtual void Platformmac68kProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x000583E9 File Offset: 0x000573E9
		protected virtual void Platformmac68kProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x000583EC File Offset: 0x000573EC
		private bool Platformmac68kProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Mac(_68(000|K)|intosh.*68K)"))
			{
				return false;
			}
			capabilities["platform"] = "Mac68K";
			this.Platformmac68kProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Platformmac68kProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0005844B File Offset: 0x0005744B
		protected virtual void PlatformmacppcProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0005844D File Offset: 0x0005744D
		protected virtual void PlatformmacppcProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00058450 File Offset: 0x00057450
		private bool PlatformmacppcProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Mac(_PowerPC|intosh.*PPC|_PPC)|PPC Mac"))
			{
				return false;
			}
			capabilities["platform"] = "MacPPC";
			this.PlatformmacppcProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PlatformmacppcProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x000584AF File Offset: 0x000574AF
		protected virtual void PlatformunixProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x000584B1 File Offset: 0x000574B1
		protected virtual void PlatformunixProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x000584B4 File Offset: 0x000574B4
		private bool PlatformunixProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "X11"))
			{
				return false;
			}
			capabilities["platform"] = "UNIX";
			this.PlatformunixProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PlatformunixProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00058513 File Offset: 0x00057513
		protected virtual void PlatformwebtvProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00058515 File Offset: 0x00057515
		protected virtual void PlatformwebtvProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x00058518 File Offset: 0x00057518
		private bool PlatformwebtvProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "WebTV"))
			{
				return false;
			}
			capabilities["platform"] = "WebTV";
			this.PlatformwebtvProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PlatformwebtvProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00058577 File Offset: 0x00057577
		protected virtual void WinProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00058579 File Offset: 0x00057579
		protected virtual void WinProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0005857C File Offset: 0x0005757C
		private bool WinProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			this.WinProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Win32Process(headers, browserCaps) && !this.Win16Process(headers, browserCaps))
			{
				flag = false;
			}
			this.WinProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x000585D0 File Offset: 0x000575D0
		protected virtual void Win32ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x000585D2 File Offset: 0x000575D2
		protected virtual void Win32ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x000585D4 File Offset: 0x000575D4
		private bool Win32Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Win(dows )?(9[58]|NT|32)"))
			{
				return false;
			}
			capabilities["win32"] = "true";
			this.Win32ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Win32ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x00058633 File Offset: 0x00057633
		protected virtual void Win16ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00058635 File Offset: 0x00057635
		protected virtual void Win16ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00058638 File Offset: 0x00057638
		private bool Win16Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "16bit|Win(dows 3\\.1|16)"))
			{
				return false;
			}
			capabilities["win16"] = "true";
			this.Win16ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Win16ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00058697 File Offset: 0x00057697
		protected virtual void GenericdownlevelProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00058699 File Offset: 0x00057699
		protected virtual void GenericdownlevelProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0005869C File Offset: 0x0005769C
		private bool GenericdownlevelProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Generic Downlevel$"))
			{
				return false;
			}
			capabilities["cookies"] = "false";
			capabilities["ecmascriptversion"] = "1.0";
			capabilities["tables"] = "true";
			capabilities["type"] = "Downlevel";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("GenericDownlevel");
			this.GenericdownlevelProcessGateways(headers, browserCaps);
			bool flag = false;
			this.GenericdownlevelProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0005874B File Offset: 0x0005774B
		protected virtual void GoamericaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0005874D File Offset: 0x0005774D
		protected virtual void GoamericaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00058750 File Offset: 0x00057750
		private bool GoamericaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Go\\.Web/(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*)"))
			{
				return false;
			}
			capabilities["BackgroundSounds"] = "true";
			capabilities["browser"] = "Go.Web";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "true";
			capabilities["defaultScreenCharactersHeight"] = "6";
			capabilities["defaultScreenCharactersWidth"] = "12";
			capabilities["defaultScreenPixelsHeight"] = "72";
			capabilities["defaultScreenPixelsWidth"] = "96";
			capabilities["isMobileDevice"] = "true";
			capabilities["javaapplets"] = "false";
			capabilities["javascript"] = "false";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "6000";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["rendersBreaksAfterHtmlLists"] = "false";
			capabilities["requiredMetaTagNameValue"] = "HandheldFriendly";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresNoBreakInFormatting"] = "true";
			capabilities["requiresUniqueHtmlCheckboxNames"] = "true";
			capabilities["screenBitDepth"] = "2";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivAlign"] = "false";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsFontSize"] = "false";
			capabilities["SupportsDivNoWrap"] = "false";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsItalic"] = "false";
			capabilities["supportsSelectMultiple"] = "false";
			capabilities["type"] = "Go.Web";
			capabilities["vbscript"] = "false";
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("GoAmerica");
			this.GoamericaProcessGateways(headers, browserCaps);
			this.GoamericaupProcess(headers, browserCaps);
			this.GatableProcess(headers, browserCaps);
			this.MaxpagesizeProcess(headers, browserCaps);
			bool flag = true;
			if (!this.GoamericawinceProcess(headers, browserCaps) && !this.GoamericapalmProcess(headers, browserCaps) && !this.GoamericarimProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.GoamericaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00058A48 File Offset: 0x00057A48
		protected virtual void GoamericaupProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00058A4A File Offset: 0x00057A4A
		protected virtual void GoamericaupProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00058A4C File Offset: 0x00057A4C
		private bool GoamericaupProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "UP\\.Browser"))
			{
				return false;
			}
			capabilities["ecmascriptversion"] = "0.0";
			capabilities["frames"] = "false";
			this.GoamericaupProcessGateways(headers, browserCaps);
			bool flag = false;
			this.GoamericaupProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00058ABB File Offset: 0x00057ABB
		protected virtual void GoamericawinceProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00058ABD File Offset: 0x00057ABD
		protected virtual void GoamericawinceProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00058AC0 File Offset: 0x00057AC0
		private bool GoamericawinceProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "WinCE"))
			{
				return false;
			}
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["defaultScreenCharactersHeight"] = "14";
			capabilities["defaultScreenCharactersWidth"] = "30";
			capabilities["defaultScreenPixelsHeight"] = "320";
			capabilities["defaultScreenPixelsWidth"] = "240";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["mobileDeviceModel"] = "Pocket PC";
			capabilities["platform"] = "WinCE";
			capabilities["screenBitDepth"] = "16";
			capabilities["supportsDivAlign"] = "true";
			capabilities["supportsFontColor"] = "true";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsSelectMultiple"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("GoAmericaWinCE");
			this.GoamericawinceProcessGateways(headers, browserCaps);
			bool flag = false;
			this.GoamericawinceProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00058C3A File Offset: 0x00057C3A
		protected virtual void GoamericapalmProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00058C3C File Offset: 0x00057C3C
		protected virtual void GoamericapalmProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x00058C40 File Offset: 0x00057C40
		private bool GoamericapalmProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Palm"))
			{
				return false;
			}
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "PalmOS-licensee";
			capabilities["screenCharactersHeight"] = "12";
			capabilities["screenCharactersWidth"] = "36";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "160";
			capabilities["supportsUncheck"] = "false";
			browserCaps.AddBrowser("GoAmericaPalm");
			this.GoamericapalmProcessGateways(headers, browserCaps);
			bool flag = false;
			this.GoamericapalmProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x00058D2A File Offset: 0x00057D2A
		protected virtual void GoamericarimProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00058D2C File Offset: 0x00057D2C
		protected virtual void GoamericarimProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00058D30 File Offset: 0x00057D30
		private bool GoamericarimProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "RIM"))
			{
				return false;
			}
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "false";
			capabilities["mobileDeviceManufacturer"] = "RIM";
			capabilities["screenBitDepth"] = "1";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivAlign"] = "false";
			capabilities["supportsDivWrap"] = "false";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsFontSize"] = "false";
			capabilities["supportsFontItalic"] = "false";
			browserCaps.AddBrowser("GoAmericaRIM");
			this.GoamericarimProcessGateways(headers, browserCaps);
			this.GoamericanonuprimProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Goamericarim950Process(headers, browserCaps) && !this.Goamericarim850Process(headers, browserCaps) && !this.Goamericarim957Process(headers, browserCaps) && !this.Goamericarim857Process(headers, browserCaps))
			{
				flag = false;
			}
			this.GoamericarimProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00058E7E File Offset: 0x00057E7E
		protected virtual void GoamericanonuprimProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00058E80 File Offset: 0x00057E80
		protected virtual void GoamericanonuprimProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00058E84 File Offset: 0x00057E84
		private bool GoamericanonuprimProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			bool flag = regexWorker.ProcessRegex(text, "UP\\.Browser");
			if (flag)
			{
				return false;
			}
			capabilities["ecmascriptversion"] = "1.1";
			capabilities["frames"] = "true";
			this.GoamericanonuprimProcessGateways(headers, browserCaps);
			bool flag2 = false;
			this.GoamericanonuprimProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00058EF3 File Offset: 0x00057EF3
		protected virtual void Goamericarim950ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00058EF5 File Offset: 0x00057EF5
		protected virtual void Goamericarim950ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00058EF8 File Offset: 0x00057EF8
		private bool Goamericarim950Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "RIM950"))
			{
				return false;
			}
			capabilities["BackgroundSounds"] = "false";
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["mobileDeviceModel"] = "950";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "25";
			capabilities["screenPixelsHeight"] = "64";
			capabilities["screenPixelsWidth"] = "132";
			browserCaps.AddBrowser("GoAmericaRIM950");
			this.Goamericarim950ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Goamericarim950ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00058FD2 File Offset: 0x00057FD2
		protected virtual void Goamericarim850ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00058FD4 File Offset: 0x00057FD4
		protected virtual void Goamericarim850ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00058FD8 File Offset: 0x00057FD8
		private bool Goamericarim850Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "RIM850"))
			{
				return false;
			}
			capabilities["mobileDeviceModel"] = "850";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "25";
			capabilities["screenPixelsHeight"] = "64";
			capabilities["screenPixelsWidth"] = "132";
			browserCaps.AddBrowser("GoAmericaRIM850");
			this.Goamericarim850ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Goamericarim850ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00059082 File Offset: 0x00058082
		protected virtual void Goamericarim957ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00059084 File Offset: 0x00058084
		protected virtual void Goamericarim957ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00059088 File Offset: 0x00058088
		private bool Goamericarim957Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "RIM957"))
			{
				return false;
			}
			capabilities["BackgroundSounds"] = "false";
			capabilities["mobileDeviceModel"] = "957";
			capabilities["screenCharactersHeight"] = "15";
			capabilities["screenCharactersWidth"] = "32";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "160";
			browserCaps.AddBrowser("GoAmericaRIM957");
			this.Goamericarim957ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Goamericarim957major6minor2Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Goamericarim957ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0005914F File Offset: 0x0005814F
		protected virtual void Goamericarim957major6minor2ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00059151 File Offset: 0x00058151
		protected virtual void Goamericarim957major6minor2ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00059154 File Offset: 0x00058154
		private bool Goamericarim957major6minor2Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["version"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "6\\.2"))
			{
				return false;
			}
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["maximumRenderedPageSize"] = "7168";
			capabilities["requiresLeadingPageBreak"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["requiresUniqueHtmlCheckboxNames"] = "true";
			capabilities["screenCharactersHeight"] = "13";
			capabilities["screenCharactersWidth"] = "30";
			capabilities["supportsSelectMultiple"] = "true";
			capabilities["supportsUncheck"] = "true";
			browserCaps.AddBrowser("GoAmericaRIM957major6minor2");
			this.Goamericarim957major6minor2ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Goamericarim957major6minor2ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00059243 File Offset: 0x00058243
		protected virtual void Goamericarim857ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00059245 File Offset: 0x00058245
		protected virtual void Goamericarim857ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00059248 File Offset: 0x00058248
		private bool Goamericarim857Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "RIM857"))
			{
				return false;
			}
			capabilities["BackgroundSounds"] = "false";
			capabilities["ecmascriptversion"] = "0.0";
			capabilities["frames"] = "false";
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["mobileDeviceModel"] = "857";
			capabilities["screenCharactersHeight"] = "15";
			capabilities["screenCharactersWidth"] = "32";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "160";
			browserCaps.AddBrowser("GoAmericaRIM857");
			this.Goamericarim857ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Goamericarim857major6Process(headers, browserCaps) && !this.Goamerica7to9Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Goamericarim857ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00059349 File Offset: 0x00058349
		protected virtual void Goamericarim857major6ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0005934B File Offset: 0x0005834B
		protected virtual void Goamericarim857major6ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00059350 File Offset: 0x00058350
		private bool Goamericarim857major6Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["version"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "6\\."))
			{
				return false;
			}
			capabilities["ecmascriptversion"] = "1.1";
			capabilities["frames"] = "true";
			browserCaps.AddBrowser("GoAmericaRIM857major6");
			this.Goamericarim857major6ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Goamericarim857major6minor2to9Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Goamericarim857major6ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x000593DC File Offset: 0x000583DC
		protected virtual void Goamericarim857major6minor2to9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x000593DE File Offset: 0x000583DE
		protected virtual void Goamericarim857major6minor2to9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x000593E0 File Offset: 0x000583E0
		private bool Goamericarim857major6minor2to9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["version"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "6\\.[2-9]"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["requiresAttributeColonSubstitution"] = "false";
			capabilities["requiresLeadingPageBreak"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenCharactersHeight"] = "16";
			capabilities["screenCharactersWidth"] = "31";
			capabilities["supportsUncheck"] = "false";
			browserCaps.AddBrowser("GoAmericaRIM857major6minor2to9");
			this.Goamericarim857major6minor2to9ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Goamericarim857major6minor2to9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x000594BF File Offset: 0x000584BF
		protected virtual void Goamerica7to9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x000594C1 File Offset: 0x000584C1
		protected virtual void Goamerica7to9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x000594C4 File Offset: 0x000584C4
		private bool Goamerica7to9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^[7-9]$"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["requiresAttributeColonSubstitution"] = "false";
			capabilities["requiresLeadingPageBreak"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenCharactersHeight"] = "16";
			capabilities["screenCharactersWidth"] = "31";
			capabilities["supportsUncheck"] = "false";
			browserCaps.AddBrowser("GoAmerica7to9");
			this.Goamerica7to9ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Goamerica7to9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x000595A3 File Offset: 0x000585A3
		protected virtual void GatableProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x000595A5 File Offset: 0x000585A5
		protected virtual void GatableProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x000595A8 File Offset: 0x000585A8
		private bool GatableProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-GA-TABLES"];
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			this.GatableProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.GatablefalseProcess(headers, browserCaps) && !this.GatabletrueProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.GatableProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00059602 File Offset: 0x00058602
		protected virtual void GatablefalseProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00059604 File Offset: 0x00058604
		protected virtual void GatablefalseProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00059608 File Offset: 0x00058608
		private bool GatablefalseProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-GA-TABLES"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?i:FALSE)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["tables"] = "false";
			this.GatablefalseProcessGateways(headers, browserCaps);
			bool flag = false;
			this.GatablefalseProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x0005966D File Offset: 0x0005866D
		protected virtual void GatabletrueProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x0005966F File Offset: 0x0005866F
		protected virtual void GatabletrueProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00059674 File Offset: 0x00058674
		private bool GatabletrueProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-GA-TABLES"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?i:TRUE)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["tables"] = "true";
			this.GatabletrueProcessGateways(headers, browserCaps);
			bool flag = false;
			this.GatabletrueProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x000596D9 File Offset: 0x000586D9
		protected virtual void MaxpagesizeProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x000596DB File Offset: 0x000586DB
		protected virtual void MaxpagesizeProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x000596E0 File Offset: 0x000586E0
		private bool MaxpagesizeProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-GA-MAX-TRANSFER"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'maxPageSize'\\d+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["maximumRenderedPageSize"] = regexWorker["${maxPageSize}"];
			this.MaxpagesizeProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MaxpagesizeProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0005974B File Offset: 0x0005874B
		protected virtual void JataayuProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0005974D File Offset: 0x0005874D
		protected virtual void JataayuProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00059750 File Offset: 0x00058750
		private bool JataayuProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^jBrowser"))
			{
				return false;
			}
			capabilities["browser"] = "Jataayu jBrowser";
			capabilities["canSendMail"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["mobileDeviceManufacturer"] = "Jataayu";
			capabilities["mobileDeviceModel"] = "jBrowser";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "wml20";
			capabilities["requiresCommentInStyleElement"] = "true";
			capabilities["requiresHiddenFieldValues"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "true";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "17";
			capabilities["screenCharactersWidth"] = "42";
			capabilities["screenPixelsHeight"] = "265";
			capabilities["screenPixelsWidth"] = "248";
			capabilities["supportsBodyClassAttribute"] = "false";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["tables"] = "true";
			capabilities["type"] = "Jataayu jBrowser";
			browserCaps.AddBrowser("Jataayu");
			this.JataayuProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.JataayuppcProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.JataayuProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00059987 File Offset: 0x00058987
		protected virtual void JataayuppcProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00059989 File Offset: 0x00058989
		protected virtual void JataayuppcProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0005998C File Offset: 0x0005898C
		private bool JataayuppcProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(PPC)"))
			{
				return false;
			}
			capabilities["cookies"] = "true";
			capabilities["screenCharactersHeight"] = "14";
			capabilities["screenCharactersWidth"] = "31";
			capabilities["screenPixelsHeight"] = "320";
			capabilities["screenPixelsWidth"] = "240";
			capabilities["supportsStyleElement"] = "true";
			browserCaps.AddBrowser("JataayuPPC");
			this.JataayuppcProcessGateways(headers, browserCaps);
			bool flag = false;
			this.JataayuppcProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00059A46 File Offset: 0x00058A46
		protected virtual void JphoneProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00059A48 File Offset: 0x00058A48
		protected virtual void JphoneProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00059A4C File Offset: 0x00058A4C
		private bool JphoneProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^J-PHONE/"))
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "J-PHONE/(?'majorVersion'\\d+)(?'minorVersion'\\.\\d+)/(?'deviceModel'.*)");
			capabilities["browser"] = "J-Phone";
			capabilities["cookies"] = "false";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["defaultCharacterHeight"] = "12";
			capabilities["defaultCharacterWidth"] = "12";
			capabilities["defaultScreenCharactersHeight"] = "7";
			capabilities["defaultScreenCharactersWidth"] = "16";
			capabilities["defaultScreenPixelsHeight"] = "84";
			capabilities["defaultScreenPixelsWidth"] = "96";
			capabilities["isColor"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["javaapplets"] = "false";
			capabilities["javascript"] = "false";
			capabilities["majorVersion"] = regexWorker["${majorVersion}"];
			capabilities["maximumRenderedPageSize"] = "6000";
			capabilities["minorVersion"] = regexWorker["${minorVersion}"];
			capabilities["mobileDeviceModel"] = regexWorker["${deviceModel}"];
			capabilities["optimumPageWeight"] = "700";
			capabilities["preferredImageMime"] = "image/png";
			capabilities["preferredRenderingType"] = "html32";
			capabilities["preferredRequestEncoding"] = "shift_jis";
			capabilities["preferredResponseEncoding"] = "shift_jis";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["requiresContentTypeMetaTag"] = "true";
			capabilities["requiresFullyQualifiedRedirectUrl"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOutputOptimization"] = "true";
			capabilities["screenBitDepth"] = "2";
			capabilities["supportsAccesskeyAttribute"] = "true";
			capabilities["supportsBodyColor"] = "true";
			capabilities["supportsBold"] = "false";
			capabilities["supportsCharacterEntityEncoding"] = "false";
			capabilities["supportsDivAlign"] = "true";
			capabilities["supportsDivNoWrap"] = "true";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsFontColor"] = "true";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsFontSize"] = "false";
			capabilities["supportsInputMode"] = "true";
			capabilities["supportsItalic"] = "false";
			capabilities["supportsJPhoneMultiMediaAttributes"] = "true";
			capabilities["supportsJPhoneSymbols"] = "true";
			capabilities["supportsQueryStringInFormAction"] = "false";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["tables"] = "true";
			capabilities["type"] = "J-Phone";
			capabilities["vbscript"] = "false";
			capabilities["version"] = regexWorker["${majorVersion}${minorVersion}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.ChtmlTextWriter";
			browserCaps.AddBrowser("Jphone");
			this.JphoneProcessGateways(headers, browserCaps);
			this.Jphone4Process(headers, browserCaps);
			this.JphonecolorProcess(headers, browserCaps);
			this.JphonedisplayProcess(headers, browserCaps);
			bool flag = true;
			if (!this.JphonemitsubishiProcess(headers, browserCaps) && !this.JphonedensoProcess(headers, browserCaps) && !this.JphonekenwoodProcess(headers, browserCaps) && !this.JphonenecProcess(headers, browserCaps) && !this.JphonepanasonicProcess(headers, browserCaps) && !this.JphonepioneerProcess(headers, browserCaps) && !this.JphonesanyoProcess(headers, browserCaps) && !this.JphonesharpProcess(headers, browserCaps) && !this.JphonetoshibaProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.JphoneProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00059E6D File Offset: 0x00058E6D
		protected virtual void JphonemitsubishiProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00059E6F File Offset: 0x00058E6F
		protected virtual void JphonemitsubishiProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00059E74 File Offset: 0x00058E74
		private bool JphonemitsubishiProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^J-D\\d"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Mitsubishi";
			browserCaps.AddBrowser("JphoneMitsubishi");
			this.JphonemitsubishiProcessGateways(headers, browserCaps);
			bool flag = false;
			this.JphonemitsubishiProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x00059EE3 File Offset: 0x00058EE3
		protected virtual void JphonedensoProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00059EE5 File Offset: 0x00058EE5
		protected virtual void JphonedensoProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00059EE8 File Offset: 0x00058EE8
		private bool JphonedensoProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "J-DN\\d"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Denso";
			browserCaps.AddBrowser("JphoneDenso");
			this.JphonedensoProcessGateways(headers, browserCaps);
			bool flag = false;
			this.JphonedensoProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x00059F57 File Offset: 0x00058F57
		protected virtual void JphonekenwoodProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00059F59 File Offset: 0x00058F59
		protected virtual void JphonekenwoodProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00059F5C File Offset: 0x00058F5C
		private bool JphonekenwoodProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "J-K\\d"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kenwood";
			browserCaps.AddBrowser("JphoneKenwood");
			this.JphonekenwoodProcessGateways(headers, browserCaps);
			bool flag = false;
			this.JphonekenwoodProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00059FCB File Offset: 0x00058FCB
		protected virtual void JphonenecProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00059FCD File Offset: 0x00058FCD
		protected virtual void JphonenecProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00059FD0 File Offset: 0x00058FD0
		private bool JphonenecProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "J-N\\d"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "NEC";
			browserCaps.AddBrowser("JphoneNec");
			this.JphonenecProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Jphonenecn51Process(headers, browserCaps))
			{
				flag = false;
			}
			this.JphonenecProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0005A04C File Offset: 0x0005904C
		protected virtual void Jphonenecn51ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0005A04E File Offset: 0x0005904E
		protected virtual void Jphonenecn51ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0005A050 File Offset: 0x00059050
		private bool Jphonenecn51Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "J-N51"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "12000";
			capabilities["mobileDeviceModel"] = "J-N51";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["requiresContentTypeMetaTag"] = "false";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["supportsCharacterEntityEncoding"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsInputIStyle"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("JphoneNecN51");
			this.Jphonenecn51ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphonenecn51ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0005A16F File Offset: 0x0005916F
		protected virtual void JphonepanasonicProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0005A171 File Offset: 0x00059171
		protected virtual void JphonepanasonicProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0005A174 File Offset: 0x00059174
		private bool JphonepanasonicProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "J-P\\d"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			browserCaps.AddBrowser("JphonePanasonic");
			this.JphonepanasonicProcessGateways(headers, browserCaps);
			bool flag = false;
			this.JphonepanasonicProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0005A1E3 File Offset: 0x000591E3
		protected virtual void JphonepioneerProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0005A1E5 File Offset: 0x000591E5
		protected virtual void JphonepioneerProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0005A1E8 File Offset: 0x000591E8
		private bool JphonepioneerProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "J-PE\\d"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Pioneer";
			browserCaps.AddBrowser("JphonePioneer");
			this.JphonepioneerProcessGateways(headers, browserCaps);
			bool flag = false;
			this.JphonepioneerProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0005A257 File Offset: 0x00059257
		protected virtual void JphonesanyoProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0005A259 File Offset: 0x00059259
		protected virtual void JphonesanyoProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0005A25C File Offset: 0x0005925C
		private bool JphonesanyoProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "J-SA\\d"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			browserCaps.AddBrowser("JphoneSanyo");
			this.JphonesanyoProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Jphonesa51Process(headers, browserCaps))
			{
				flag = false;
			}
			this.JphonesanyoProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0005A2D8 File Offset: 0x000592D8
		protected virtual void Jphonesa51ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0005A2DA File Offset: 0x000592DA
		protected virtual void Jphonesa51ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0005A2DC File Offset: 0x000592DC
		private bool Jphonesa51Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^J-SA51\\D*"))
			{
				return false;
			}
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["maximumRenderedPageSize"] = "12000";
			capabilities["mobileDeviceModel"] = "J-SA51";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenCharactersWidth"] = "22";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsInputIStyle"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("JphoneSA51");
			this.Jphonesa51ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphonesa51ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0005A3CB File Offset: 0x000593CB
		protected virtual void JphonesharpProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x0005A3CD File Offset: 0x000593CD
		protected virtual void JphonesharpProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x0005A3D0 File Offset: 0x000593D0
		private bool JphonesharpProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "J-SH\\d"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sharp";
			browserCaps.AddBrowser("JphoneSharp");
			this.JphonesharpProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Jphonesharpsh53Process(headers, browserCaps) && !this.Jphonesharpsh07Process(headers, browserCaps) && !this.Jphonesharpsh08Process(headers, browserCaps) && !this.Jphonesharpsh51Process(headers, browserCaps) && !this.Jphonesharpsh52Process(headers, browserCaps))
			{
				flag = false;
			}
			this.JphonesharpProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0005A474 File Offset: 0x00059474
		protected virtual void Jphonesharpsh53ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0005A476 File Offset: 0x00059476
		protected virtual void Jphonesharpsh53ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0005A478 File Offset: 0x00059478
		private bool Jphonesharpsh53Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "J-SH53"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "12000";
			capabilities["mobileDeviceModel"] = "J-SH53";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["requiresContentTypeMetaTag"] = "false";
			capabilities["screenBitDepth"] = "18";
			capabilities["screenCharactersHeight"] = "13";
			capabilities["screenCharactersWidth"] = "24";
			capabilities["supportsCharacterEntityEncoding"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsInputIStyle"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("JphoneSharpSh53");
			this.Jphonesharpsh53ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphonesharpsh53ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0005A5A7 File Offset: 0x000595A7
		protected virtual void Jphonesharpsh07ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0005A5A9 File Offset: 0x000595A9
		protected virtual void Jphonesharpsh07ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x0005A5AC File Offset: 0x000595AC
		private bool Jphonesharpsh07Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^J-SH07"))
			{
				return false;
			}
			capabilities["canRenderEmptySelects"] = "false";
			capabilities["maximumRenderedPageSize"] = "12000";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["requiresLeadingPageBreak"] = "false";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("JphoneSharpSh07");
			this.Jphonesharpsh07ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphonesharpsh07ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0005A68B File Offset: 0x0005968B
		protected virtual void Jphonesharpsh08ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0005A68D File Offset: 0x0005968D
		protected virtual void Jphonesharpsh08ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0005A690 File Offset: 0x00059690
		private bool Jphonesharpsh08Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^J-SH08"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["supportsInputIStyle"] = "false";
			capabilities["requiresLeadingPageBreak"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "117";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("JphoneSharpSh08");
			this.Jphonesharpsh08ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphonesharpsh08ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0005A79F File Offset: 0x0005979F
		protected virtual void Jphonesharpsh51ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0005A7A1 File Offset: 0x000597A1
		protected virtual void Jphonesharpsh51ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0005A7A4 File Offset: 0x000597A4
		private bool Jphonesharpsh51Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^J-SH51"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "12000";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["supportsInputIStyle"] = "true";
			capabilities["requiresLeadingPageBreak"] = "false";
			capabilities["requiresUniqueHtmlCheckboxNames"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "130";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("JphoneSharpSh51");
			this.Jphonesharpsh51ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphonesharpsh51ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0005A8D3 File Offset: 0x000598D3
		protected virtual void Jphonesharpsh52ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0005A8D5 File Offset: 0x000598D5
		protected virtual void Jphonesharpsh52ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0005A8D8 File Offset: 0x000598D8
		private bool Jphonesharpsh52Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^J-SH52\\D*"))
			{
				return false;
			}
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["maximumRenderedPageSize"] = "12000";
			capabilities["mobileDeviceModel"] = "J-SH52";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsInputIStyle"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("JphoneSharpSh52");
			this.Jphonesharpsh52ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphonesharpsh52ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0005A9B7 File Offset: 0x000599B7
		protected virtual void JphonetoshibaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0005A9B9 File Offset: 0x000599B9
		protected virtual void JphonetoshibaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0005A9BC File Offset: 0x000599BC
		private bool JphonetoshibaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "J-T\\d"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Toshiba";
			browserCaps.AddBrowser("JphoneToshiba");
			this.JphonetoshibaProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Jphonetoshibat06aProcess(headers, browserCaps) && !this.Jphonetoshibat08Process(headers, browserCaps) && !this.Jphonetoshibat51Process(headers, browserCaps))
			{
				flag = false;
			}
			this.JphonetoshibaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0005AA4C File Offset: 0x00059A4C
		protected virtual void Jphonetoshibat06aProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0005AA4E File Offset: 0x00059A4E
		protected virtual void Jphonetoshibat06aProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0005AA50 File Offset: 0x00059A50
		private bool Jphonetoshibat06aProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^J-T06_a"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "12000";
			capabilities["mobileDeviceModel"] = "J-T06";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("JphoneToshibaT06a");
			this.Jphonetoshibat06aProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphonetoshibat06aProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0005AB1F File Offset: 0x00059B1F
		protected virtual void Jphonetoshibat08ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0005AB21 File Offset: 0x00059B21
		protected virtual void Jphonetoshibat08ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0005AB24 File Offset: 0x00059B24
		private bool Jphonetoshibat08Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^J-T08\\D*"))
			{
				return false;
			}
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "22";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("JphoneToshibaT08");
			this.Jphonetoshibat08ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphonetoshibat08ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0005ABD3 File Offset: 0x00059BD3
		protected virtual void Jphonetoshibat51ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0005ABD5 File Offset: 0x00059BD5
		protected virtual void Jphonetoshibat51ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0005ABD8 File Offset: 0x00059BD8
		private bool Jphonetoshibat51Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^J-T51"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "12000";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "false";
			capabilities["requiresLeadingPageBreak"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "24";
			capabilities["screenPixelsHeight"] = "144";
			capabilities["screenPixelsWidth"] = "144";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsInputIStyle"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			browserCaps.AddBrowser("JphoneToshibaT51");
			this.Jphonetoshibat51ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphonetoshibat51ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0005ACF7 File Offset: 0x00059CF7
		protected virtual void Jphone4ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0005ACF9 File Offset: 0x00059CF9
		protected virtual void Jphone4ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0005ACFC File Offset: 0x00059CFC
		private bool Jphone4Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "4"))
			{
				return false;
			}
			capabilities["supportsQueryStringInFormAction"] = "true";
			this.Jphone4ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphone4ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0005AD60 File Offset: 0x00059D60
		protected virtual void JphonecolorProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0005AD62 File Offset: 0x00059D62
		protected virtual void JphonecolorProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0005AD64 File Offset: 0x00059D64
		private bool JphonecolorProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-JPHONE-COLOR"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'colorIndicator'[CG])(?'bitDepth'\\d+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["bitDepth"] = regexWorker["${bitDepth}"];
			capabilities["colorIndicator"] = regexWorker["${colorIndicator}"];
			this.JphonecolorProcessGateways(headers, browserCaps);
			this.JphonecoloriscolorProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Jphone16bitcolorProcess(headers, browserCaps) && !this.Jphone8bitcolorProcess(headers, browserCaps) && !this.Jphone2bitcolorProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.JphonecolorProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0005AE0F File Offset: 0x00059E0F
		protected virtual void JphonecoloriscolorProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0005AE11 File Offset: 0x00059E11
		protected virtual void JphonecoloriscolorProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0005AE14 File Offset: 0x00059E14
		private bool JphonecoloriscolorProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["colorIndicator"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "C"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			this.JphonecoloriscolorProcessGateways(headers, browserCaps);
			bool flag = false;
			this.JphonecoloriscolorProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0005AE78 File Offset: 0x00059E78
		protected virtual void Jphone16bitcolorProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0005AE7A File Offset: 0x00059E7A
		protected virtual void Jphone16bitcolorProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0005AE7C File Offset: 0x00059E7C
		private bool Jphone16bitcolorProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["bitDepth"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^65536$"))
			{
				return false;
			}
			capabilities["screenBitDepth"] = "16";
			this.Jphone16bitcolorProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphone16bitcolorProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0005AEE0 File Offset: 0x00059EE0
		protected virtual void Jphone8bitcolorProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0005AEE2 File Offset: 0x00059EE2
		protected virtual void Jphone8bitcolorProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0005AEE4 File Offset: 0x00059EE4
		private bool Jphone8bitcolorProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["bitDepth"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^256$"))
			{
				return false;
			}
			capabilities["screenBitDepth"] = "8";
			this.Jphone8bitcolorProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphone8bitcolorProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0005AF48 File Offset: 0x00059F48
		protected virtual void Jphone2bitcolorProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0005AF4A File Offset: 0x00059F4A
		protected virtual void Jphone2bitcolorProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0005AF4C File Offset: 0x00059F4C
		private bool Jphone2bitcolorProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["bitDepth"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^4$"))
			{
				return false;
			}
			capabilities["screenBitDepth"] = "2";
			this.Jphone2bitcolorProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Jphone2bitcolorProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0005AFB0 File Offset: 0x00059FB0
		protected virtual void JphonedisplayProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0005AFB2 File Offset: 0x00059FB2
		protected virtual void JphonedisplayProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0005AFB4 File Offset: 0x00059FB4
		private bool JphonedisplayProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-JPHONE-DISPLAY"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'screenWidth'\\d+)\\*(?'screenHeight'\\d+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["screenPixelsHeight"] = regexWorker["${screenHeight}"];
			capabilities["screenPixelsWidth"] = regexWorker["${screenWidth}"];
			this.JphonedisplayProcessGateways(headers, browserCaps);
			bool flag = false;
			this.JphonedisplayProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0005B035 File Offset: 0x0005A035
		protected virtual void LegendProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0005B037 File Offset: 0x0005A037
		protected virtual void LegendProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0005B03C File Offset: 0x0005A03C
		private bool LegendProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^(?'deviceID'LG\\S*) AU\\/(?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*).*"))
			{
				return false;
			}
			capabilities["browser"] = "AU-System";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "false";
			capabilities["hasBackButton"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["numberOfSoftkeys"] = "2";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["rendersWmlDoAcceptsInline"] = "false";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["type"] = regexWorker["AU ${browserMajorVersion}"];
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.AddBrowser("Legend");
			this.LegendProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Lgg5200Process(headers, browserCaps))
			{
				flag = false;
			}
			this.LegendProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0005B1DB File Offset: 0x0005A1DB
		protected virtual void Lgg5200ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0005B1DD File Offset: 0x0005A1DD
		protected virtual void Lgg5200ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0005B1E0 File Offset: 0x0005A1E0
		private bool Lgg5200Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^LG-G5200"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "3500";
			capabilities["maximumSoftkeyLabelLength"] = "6";
			capabilities["mobileDeviceManufacturer"] = "LEGEND";
			capabilities["mobileDeviceModel"] = "G808";
			capabilities["screenBitDepth"] = "4";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "17";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "128";
			browserCaps.AddBrowser("LGG5200");
			this.Lgg5200ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Lgg5200ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0005B2CA File Offset: 0x0005A2CA
		protected virtual void MmeProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0005B2CC File Offset: 0x0005A2CC
		protected virtual void MmeProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0005B2D0 File Offset: 0x0005A2D0
		private bool MmeProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MMEF"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			if (!regexWorker.ProcessRegex(text, "Mozilla/(?'version'(?'major'\\d+)(?'minor'\\.\\d+)\\w*).*"))
			{
				return false;
			}
			capabilities["browser"] = "Microsoft Mobile Explorer";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["inputType"] = "telephoneKeypad";
			capabilities["isMobileDevice"] = "true";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["mobileDeviceManufacturer"] = "Microsoft";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["type"] = "Microsoft Mobile Explorer";
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("MME");
			this.MmeProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Mmef20Process(headers, browserCaps) && !this.MmemobileexplorerProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.MmeProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0005B420 File Offset: 0x0005A420
		protected virtual void Mmef20ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0005B422 File Offset: 0x0005A422
		protected virtual void Mmef20ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0005B424 File Offset: 0x0005A424
		private bool Mmef20Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MMEF20"))
			{
				return false;
			}
			capabilities["canRenderSetvarZeroWithMultiSelectionList"] = "false";
			capabilities["defaultCharacterHeight"] = "15";
			capabilities["defaultCharacterWidth"] = "5";
			capabilities["defaultScreenPixelsHeight"] = "160";
			capabilities["defaultScreenPixelsWidth"] = "120";
			capabilities["isColor"] = "false";
			capabilities["maximumRenderedPageSize"] = "4000";
			capabilities["mobileDeviceModel"] = "Simulator";
			capabilities["numberOfSoftkeys"] = "2";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml11";
			capabilities["screenBitDepth"] = "1";
			browserCaps.AddBrowser("MMEF20");
			this.Mmef20ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.MmecellphoneProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Mmef20ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0005B55B File Offset: 0x0005A55B
		protected virtual void MmemobileexplorerProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0005B55D File Offset: 0x0005A55D
		protected virtual void MmemobileexplorerProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0005B560 File Offset: 0x0005A560
		private bool MmemobileexplorerProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^MobileExplorer/(?'majorVersion'\\d*)(?'minorVersion'\\.\\d*) \\(Mozilla/1\\.22; compatible; MMEF\\d+; (?'manufacturer'[^;]*); (?'model'[^;)]*)(; (?'deviceID'[^)]*))?"))
			{
				return false;
			}
			capabilities["canRenderSetvarZeroWithMultiSelectionList"] = "false";
			capabilities["defaultCharacterHeight"] = "15";
			capabilities["defaultCharacterWidth"] = "5";
			capabilities["defaultScreenPixelsHeight"] = "160";
			capabilities["defaultScreenPixelsWidth"] = "120";
			capabilities["deviceID"] = regexWorker["${deviceID}"];
			capabilities["isColor"] = "true";
			capabilities["majorVersion"] = regexWorker["${majorVersion}"];
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["minorVersion"] = regexWorker["${minorVersion}"];
			capabilities["mobileDeviceManufacturer"] = regexWorker["${manufacturer}"];
			capabilities["mobileDeviceModel"] = regexWorker["${model}"];
			capabilities["numberOfSoftkeys"] = "2";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "text/html";
			capabilities["preferredRenderingType"] = "html32";
			capabilities["screenBitDepth"] = "8";
			capabilities["supportsBodyColor"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsDivAlign"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFontColor"] = "true";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["version"] = regexWorker["${majorVersion}${minorVersion}"];
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("MMEMobileExplorer");
			this.MmemobileexplorerProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MmemobileexplorerProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0005B799 File Offset: 0x0005A799
		protected virtual void MmecellphoneProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0005B79B File Offset: 0x0005A79B
		protected virtual void MmecellphoneProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0005B7A0 File Offset: 0x0005A7A0
		private bool MmecellphoneProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Mozilla/.*\\(compatible; MMEF(?'majorVersion'\\d)(?'minorVersion'\\d); Cell[pP]hone(([;,] (?'deviceID'[^;]*))(;(?'buildInfo'.*))*)*\\)"))
			{
				return false;
			}
			capabilities["canCombineFormsInDeck"] = "false";
			capabilities["canRenderPostBackCards"] = "false";
			capabilities["deviceID"] = regexWorker["${deviceID}"];
			capabilities["majorVersion"] = regexWorker["${majorVersion}"];
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["minorVersion"] = regexWorker[".${minorVersion}"];
			capabilities["version"] = regexWorker["${majorVersion}.${minorVersion}"];
			browserCaps.AddBrowser("MMECellphone");
			this.MmecellphoneProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.MmebenefonqProcess(headers, browserCaps) && !this.Mmesonycmdz5Process(headers, browserCaps) && !this.Mmesonycmdj5Process(headers, browserCaps) && !this.Mmesonycmdj7Process(headers, browserCaps) && !this.MmegenericsmallProcess(headers, browserCaps) && !this.MmegenericlargeProcess(headers, browserCaps) && !this.MmegenericflipProcess(headers, browserCaps) && !this.Mmegeneric3dProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.MmecellphoneProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0005B8D5 File Offset: 0x0005A8D5
		protected virtual void MmebenefonqProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0005B8D7 File Offset: 0x0005A8D7
		protected virtual void MmebenefonqProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0005B8DC File Offset: 0x0005A8DC
		private bool MmebenefonqProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Benefon Q"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Benefon";
			capabilities["mobileDeviceModel"] = "Q";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "48";
			capabilities["screenPixelsWidth"] = "100";
			browserCaps.AddBrowser("MMEBenefonQ");
			this.MmebenefonqProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MmebenefonqProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0005B9AB File Offset: 0x0005A9AB
		protected virtual void Mmesonycmdz5ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0005B9AD File Offset: 0x0005A9AD
		protected virtual void Mmesonycmdz5ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0005B9B0 File Offset: 0x0005A9B0
		private bool Mmesonycmdz5Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Sony CMD-Z5"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["mobileDeviceModel"] = "CMD-Z5";
			capabilities["requiresOutputOptimization"] = "true";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "60";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("MMESonyCMDZ5");
			this.Mmesonycmdz5ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Mmesonycmdz5pj020eProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Mmesonycmdz5ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0005BA9C File Offset: 0x0005AA9C
		protected virtual void Mmesonycmdz5pj020eProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0005BA9E File Offset: 0x0005AA9E
		protected virtual void Mmesonycmdz5pj020eProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0005BAA0 File Offset: 0x0005AAA0
		private bool Mmesonycmdz5pj020eProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Pj020e"))
			{
				return false;
			}
			capabilities["screenPixelsHeight"] = "65";
			browserCaps.AddBrowser("MMESonyCMDZ5Pj020e");
			this.Mmesonycmdz5pj020eProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mmesonycmdz5pj020eProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0005BB0A File Offset: 0x0005AB0A
		protected virtual void Mmesonycmdj5ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0005BB0C File Offset: 0x0005AB0C
		protected virtual void Mmesonycmdj5ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0005BB10 File Offset: 0x0005AB10
		private bool Mmesonycmdj5Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Sony CMD-J5"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["mobileDeviceModel"] = "CMD-J5";
			capabilities["requiresOutputOptimization"] = "true";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "65";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("MMESonyCMDJ5");
			this.Mmesonycmdj5ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mmesonycmdj5ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0005BBEF File Offset: 0x0005ABEF
		protected virtual void Mmesonycmdj7ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0005BBF1 File Offset: 0x0005ABF1
		protected virtual void Mmesonycmdj7ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0005BBF4 File Offset: 0x0005ABF4
		private bool Mmesonycmdj7Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Sony CMD-J7/J70"))
			{
				return false;
			}
			capabilities["canCombineFormsInDeck"] = "true";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["canRenderPostBackCards"] = "true";
			capabilities["canRenderSetvarZeroWithMultiSelectionList"] = "true";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "2900";
			capabilities["maximumSoftkeyLabelLength"] = "21";
			capabilities["mobileDeviceManufacturer"] = "Ericsson";
			capabilities["mobileDeviceModel"] = "T68";
			capabilities["numberOfSoftkeys"] = "1";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["rendersWmlDoAcceptsInline"] = "false";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "80";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontSize"] = "true";
			browserCaps.AddBrowser("MMESonyCMDJ7");
			this.Mmesonycmdj7ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mmesonycmdj7ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0005BDB3 File Offset: 0x0005ADB3
		protected virtual void MmegenericsmallProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0005BDB5 File Offset: 0x0005ADB5
		protected virtual void MmegenericsmallProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0005BDB8 File Offset: 0x0005ADB8
		private bool MmegenericsmallProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "GenericSmall"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Microsoft";
			capabilities["mobileDeviceModel"] = "Generic Small Skin";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenPixelsHeight"] = "60";
			capabilities["screenPixelsWidth"] = "100";
			browserCaps.AddBrowser("MMEGenericSmall");
			this.MmegenericsmallProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MmegenericsmallProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0005BE67 File Offset: 0x0005AE67
		protected virtual void MmegenericlargeProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0005BE69 File Offset: 0x0005AE69
		protected virtual void MmegenericlargeProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0005BE6C File Offset: 0x0005AE6C
		private bool MmegenericlargeProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "GenericLarge"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Microsoft";
			capabilities["mobileDeviceModel"] = "Generic Large Skin";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenPixelsHeight"] = "240";
			capabilities["screenPixelsWidth"] = "160";
			browserCaps.AddBrowser("MMEGenericLarge");
			this.MmegenericlargeProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MmegenericlargeProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0005BF1B File Offset: 0x0005AF1B
		protected virtual void MmegenericflipProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0005BF1D File Offset: 0x0005AF1D
		protected virtual void MmegenericflipProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0005BF20 File Offset: 0x0005AF20
		private bool MmegenericflipProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "GenericFlip"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Microsoft";
			capabilities["mobileDeviceModel"] = "Generic Flip Skin";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenPixelsHeight"] = "200";
			capabilities["screenPixelsWidth"] = "160";
			browserCaps.AddBrowser("MMEGenericFlip");
			this.MmegenericflipProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MmegenericflipProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0005BFCF File Offset: 0x0005AFCF
		protected virtual void Mmegeneric3dProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0005BFD1 File Offset: 0x0005AFD1
		protected virtual void Mmegeneric3dProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0005BFD4 File Offset: 0x0005AFD4
		private bool Mmegeneric3dProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Generic3D"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Microsoft";
			capabilities["mobileDeviceModel"] = "Generic 3D Skin";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "128";
			browserCaps.AddBrowser("MMEGeneric3D");
			this.Mmegeneric3dProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mmegeneric3dProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0005C083 File Offset: 0x0005B083
		protected virtual void Netscape3ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0005C085 File Offset: 0x0005B085
		protected virtual void Netscape3ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0005C088 File Offset: 0x0005B088
		private bool Netscape3Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Mozilla/3"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			bool flag = regexWorker.ProcessRegex(text, "Opera");
			if (flag)
			{
				return false;
			}
			text = browserCaps[string.Empty];
			flag = regexWorker.ProcessRegex(text, "AvantGo");
			if (flag)
			{
				return false;
			}
			text = browserCaps[string.Empty];
			flag = regexWorker.ProcessRegex(text, "MSIE");
			if (flag)
			{
				return false;
			}
			capabilities["browser"] = "Netscape";
			capabilities["cookies"] = "true";
			capabilities["css1"] = "true";
			capabilities["ecmascriptversion"] = "1.1";
			capabilities["frames"] = "true";
			capabilities["isColor"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["javascript"] = "true";
			capabilities["screenBitDepth"] = "8";
			capabilities["supportsCss"] = "false";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = "Netscape3";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("Netscape3");
			this.Netscape3ProcessGateways(headers, browserCaps);
			bool flag2 = false;
			this.Netscape3ProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0005C231 File Offset: 0x0005B231
		protected virtual void Netscape4ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0005C233 File Offset: 0x0005B233
		protected virtual void Netscape4ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0005C238 File Offset: 0x0005B238
		private bool Netscape4Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Mozilla/4"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			bool flag = regexWorker.ProcessRegex(text, "Opera");
			if (flag)
			{
				return false;
			}
			text = browserCaps[string.Empty];
			flag = regexWorker.ProcessRegex(text, "MSIE");
			if (flag)
			{
				return false;
			}
			capabilities["browser"] = "Netscape";
			capabilities["cookies"] = "true";
			capabilities["css1"] = "true";
			capabilities["ecmascriptversion"] = "1.3";
			capabilities["frames"] = "true";
			capabilities["isColor"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["javascript"] = "true";
			capabilities["screenBitDepth"] = "8";
			capabilities["supportsCss"] = "false";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = "Netscape4";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("Netscape4");
			this.Netscape4ProcessGateways(headers, browserCaps);
			bool flag2 = true;
			if (!this.CasiopeiaProcess(headers, browserCaps) && !this.PalmwebproProcess(headers, browserCaps) && !this.NetfrontProcess(headers, browserCaps))
			{
				flag2 = false;
			}
			this.Netscape4ProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0005C3E4 File Offset: 0x0005B3E4
		protected virtual void Netscape5ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x0005C3E6 File Offset: 0x0005B3E6
		protected virtual void Netscape5ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0005C3E8 File Offset: 0x0005B3E8
		private bool Netscape5Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Mozilla/5\\.0 \\([^)]*\\) (Gecko/[-\\d]+ )?Netscape\\d?/(?'version'(?'major'\\d+)(?'minor'\\.\\d+)(?'letters'\\w*))"))
			{
				return false;
			}
			capabilities["browser"] = "Netscape";
			capabilities["cookies"] = "true";
			capabilities["css1"] = "true";
			capabilities["css2"] = "true";
			capabilities["ecmascriptversion"] = "1.5";
			capabilities["frames"] = "true";
			capabilities["isColor"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["javascript"] = "true";
			capabilities["letters"] = regexWorker["${letters}"];
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["screenBitDepth"] = "8";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["supportsVCard"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = regexWorker["Netscape${major}"];
			capabilities["version"] = regexWorker["${version}"];
			capabilities["w3cdomversion"] = "1.0";
			capabilities["xml"] = "true";
			browserCaps.AddBrowser("Netscape5");
			this.Netscape5ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Netscape6to9Process(headers, browserCaps) && !this.NetscapebetaProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Netscape5ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0005C5E7 File Offset: 0x0005B5E7
		protected virtual void Netscape6to9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0005C5E9 File Offset: 0x0005B5E9
		protected virtual void Netscape6to9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0005C5EC File Offset: 0x0005B5EC
		private bool Netscape6to9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["version"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "[6-9]\\."))
			{
				return false;
			}
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsXmlHttp"] = "true";
			capabilities["supportsCallback"] = "true";
			capabilities["supportsMaintainScrollPositionOnPostback"] = "true";
			browserCaps.AddBrowser("Netscape6to9");
			this.Netscape6to9ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Netscape6to9betaProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Netscape6to9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0005C6A8 File Offset: 0x0005B6A8
		protected virtual void Netscape6to9betaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0005C6AA File Offset: 0x0005B6AA
		protected virtual void Netscape6to9betaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0005C6AC File Offset: 0x0005B6AC
		private bool Netscape6to9betaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["letters"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^b"))
			{
				return false;
			}
			capabilities["beta"] = "true";
			browserCaps.AddBrowser("Netscape6to9Beta");
			this.Netscape6to9betaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Netscape6to9betaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0005C71B File Offset: 0x0005B71B
		protected virtual void NetscapebetaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0005C71D File Offset: 0x0005B71D
		protected virtual void NetscapebetaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0005C720 File Offset: 0x0005B720
		private bool NetscapebetaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["letters"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^b"))
			{
				return false;
			}
			capabilities["beta"] = "true";
			browserCaps.AddBrowser("NetscapeBeta");
			this.NetscapebetaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.NetscapebetaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0005C78F File Offset: 0x0005B78F
		protected virtual void NokiaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0005C791 File Offset: 0x0005B791
		protected virtual void NokiaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0005C794 File Offset: 0x0005B794
		private bool NokiaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia"))
			{
				return false;
			}
			capabilities["browser"] = "Nokia";
			capabilities["cookies"] = "false";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["canRenderOneventAndPrevElementsTogether"] = "false";
			capabilities["canRenderPostBackCards"] = "false";
			capabilities["canSendMail"] = "false";
			capabilities["defaultScreenCharactersHeight"] = "4";
			capabilities["defaultScreenCharactersWidth"] = "20";
			capabilities["defaultScreenPixelsHeight"] = "40";
			capabilities["defaultScreenPixelsWidth"] = "90";
			capabilities["hasBackButton"] = "false";
			capabilities["inputType"] = "telephoneKeypad";
			capabilities["isColor"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["maximumRenderedPageSize"] = "1397";
			capabilities["mobileDeviceManufacturer"] = "Nokia";
			capabilities["numberOfSoftkeys"] = "2";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml11";
			capabilities["rendersBreaksAfterWmlAnchor"] = "true";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["rendersWmlDoAcceptsInline"] = "false";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["requiresPhoneNumbersAsPlainText"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "1";
			capabilities["type"] = "Nokia";
			browserCaps.AddBrowser("Nokia");
			this.NokiaProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.NokiablueprintProcess(headers, browserCaps) && !this.NokiawapsimulatorProcess(headers, browserCaps) && !this.NokiamobilebrowserProcess(headers, browserCaps) && !this.Nokia7110Process(headers, browserCaps) && !this.Nokia6220Process(headers, browserCaps) && !this.Nokia6250Process(headers, browserCaps) && !this.Nokia6310Process(headers, browserCaps) && !this.Nokia6510Process(headers, browserCaps) && !this.Nokia8310Process(headers, browserCaps) && !this.Nokia9110iProcess(headers, browserCaps) && !this.Nokia9110Process(headers, browserCaps) && !this.Nokia3330Process(headers, browserCaps) && !this.Nokia9210Process(headers, browserCaps) && !this.Nokia9210htmlProcess(headers, browserCaps) && !this.Nokia3590Process(headers, browserCaps) && !this.Nokia3595Process(headers, browserCaps) && !this.Nokia3560Process(headers, browserCaps) && !this.Nokia3650Process(headers, browserCaps) && !this.Nokia5100Process(headers, browserCaps) && !this.Nokia6200Process(headers, browserCaps) && !this.Nokia6590Process(headers, browserCaps) && !this.Nokia6800Process(headers, browserCaps) && !this.Nokia7650Process(headers, browserCaps))
			{
				flag = false;
			}
			this.NokiaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0005CAB5 File Offset: 0x0005BAB5
		protected virtual void NokiablueprintProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0005CAB7 File Offset: 0x0005BAB7
		protected virtual void NokiablueprintProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0005CABC File Offset: 0x0005BABC
		private bool NokiablueprintProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia\\-WAP\\-Toolkit\\/(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*)"))
			{
				return false;
			}
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["cookies"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "65536";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "Blueprint Simulator";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersBreaksAfterWmlAnchor"] = "false";
			capabilities["type"] = "Nokia WAP Toolkit";
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.AddBrowser("NokiaBlueprint");
			this.NokiablueprintProcessGateways(headers, browserCaps);
			bool flag = false;
			this.NokiablueprintProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0005CBC8 File Offset: 0x0005BBC8
		protected virtual void NokiawapsimulatorProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0005CBCA File Offset: 0x0005BBCA
		protected virtual void NokiawapsimulatorProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0005CBCC File Offset: 0x0005BBCC
		private bool NokiawapsimulatorProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia\\-MIT\\-Browser\\/(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*)"))
			{
				return false;
			}
			capabilities["canRenderOnEventAndPrevElementsTogether"] = "true";
			capabilities["canRenderPostBackCards"] = "true";
			capabilities["cookies"] = "true";
			capabilities["hasBackButton"] = "true";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "3584";
			capabilities["maximumSoftkeyLabelLength"] = "21";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "WAP Simulator";
			capabilities["rendersBreaksAfterWmlAnchor"] = "false";
			capabilities["requiresPhoneNumbersAsPlainText"] = "false";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "25";
			capabilities["screenCharactersWidth"] = "32";
			capabilities["screenPixelsHeight"] = "512";
			capabilities["screenPixelsWidth"] = "384";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["type"] = "Nokia Mobile Internet Toolkit";
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.AddBrowser("NokiaWapSimulator");
			this.NokiawapsimulatorProcessGateways(headers, browserCaps);
			bool flag = false;
			this.NokiawapsimulatorProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0005CDA8 File Offset: 0x0005BDA8
		protected virtual void NokiamobilebrowserProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0005CDAA File Offset: 0x0005BDAA
		protected virtual void NokiamobilebrowserProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0005CDAC File Offset: 0x0005BDAC
		private bool NokiamobilebrowserProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia Mobile Browser (?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*)"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "25000";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = regexWorker["Mobile Browser ${browserMajorVersion}${browserMinorVersion}"];
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "application/vnd.wap.xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["requiresAbsolutePostbackUrl"] = "true";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresPostRedirectionHandling"] = "true";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "14";
			capabilities["screenCharactersWidth"] = "24";
			capabilities["screenPixelsHeight"] = "255";
			capabilities["screenPixelsWidth"] = "180";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["supportsStyleElement"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = regexWorker["Nokia Mobile Browser ${browserMajorVersion}${browserMinorVersion}"];
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.HtmlTextWriter = "System.Web.UI.XhtmlTextWriter";
			browserCaps.AddBrowser("NokiaMobileBrowser");
			this.NokiamobilebrowserProcessGateways(headers, browserCaps);
			bool flag = false;
			this.NokiamobilebrowserProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0005D03F File Offset: 0x0005C03F
		protected virtual void Nokia7110ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0005D041 File Offset: 0x0005C041
		protected virtual void Nokia7110ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0005D044 File Offset: 0x0005C044
		private bool Nokia7110Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia7110/1\\.0 \\((?'versionString'(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*).*)\\)"))
			{
				return false;
			}
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "7110";
			capabilities["optimumPageWeight"] = "800";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "22";
			capabilities["screenPixelsHeight"] = "44";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["type"] = "Nokia 7110";
			capabilities["version"] = regexWorker["${versionString}"];
			browserCaps.AddBrowser("Nokia7110");
			this.Nokia7110ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia7110ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0005D150 File Offset: 0x0005C150
		protected virtual void Nokia6220ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0005D152 File Offset: 0x0005C152
		protected virtual void Nokia6220ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0005D154 File Offset: 0x0005C154
		private bool Nokia6220Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia6210/1\\.0 \\((?'versionString'(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*).*)\\)"))
			{
				return false;
			}
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "6210";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "22";
			capabilities["screenPixelsHeight"] = "41";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["type"] = "Nokia 6210";
			capabilities["version"] = regexWorker["${versionString}"];
			browserCaps.AddBrowser("Nokia6220");
			this.Nokia6220ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia6220ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0005D250 File Offset: 0x0005C250
		protected virtual void Nokia6250ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0005D252 File Offset: 0x0005C252
		protected virtual void Nokia6250ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0005D254 File Offset: 0x0005C254
		private bool Nokia6250Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia6250/1\\.0 \\((?'versionString'(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*).*)\\)"))
			{
				return false;
			}
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "6250";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "22";
			capabilities["screenPixelsHeight"] = "41";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["type"] = "Nokia 6250";
			capabilities["version"] = regexWorker["${versionString}"];
			browserCaps.AddBrowser("Nokia6250");
			this.Nokia6250ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia6250ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0005D350 File Offset: 0x0005C350
		protected virtual void Nokia6310ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0005D352 File Offset: 0x0005C352
		protected virtual void Nokia6310ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0005D354 File Offset: 0x0005C354
		private bool Nokia6310Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia6310/1\\.0 \\((?'versionString'(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*).*)\\)"))
			{
				return false;
			}
			capabilities["canRenderOneventAndPrevElementsTogether"] = "true";
			capabilities["canRenderPostBackCards"] = "true";
			capabilities["cookies"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "2800";
			capabilities["maximumSoftkeyLabelLength"] = "21";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "6310";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersBreaksAfterWmlAnchor"] = "false";
			capabilities["rendersBreaksAfterWmlInput"] = "false";
			capabilities["requiresPhoneNumbersAsPlainText"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "45";
			capabilities["screenPixelsWidth"] = "92";
			capabilities["type"] = "Nokia 6310";
			capabilities["version"] = regexWorker["${versionString}"];
			browserCaps.AddBrowser("Nokia6310");
			this.Nokia6310ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia6310ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0005D4F0 File Offset: 0x0005C4F0
		protected virtual void Nokia6510ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0005D4F2 File Offset: 0x0005C4F2
		protected virtual void Nokia6510ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0005D4F4 File Offset: 0x0005C4F4
		private bool Nokia6510Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia6510/1\\.0 \\((?'versionString'(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*).*)\\)"))
			{
				return false;
			}
			capabilities["canRenderOnEventAndPrevElementsTogether"] = "true";
			capabilities["canRenderPostBackCards"] = "true";
			capabilities["cookies"] = "true";
			capabilities["hasBackButton"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "2800";
			capabilities["maximumSoftkeyLabelLength"] = "21";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "6510";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["requiresPhoneNumbersAsPlainText"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "45";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["type"] = "Nokia 6510";
			capabilities["version"] = regexWorker["${versionString}"];
			browserCaps.AddBrowser("Nokia6510");
			this.Nokia6510ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia6510ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0005D6A0 File Offset: 0x0005C6A0
		protected virtual void Nokia8310ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0005D6A2 File Offset: 0x0005C6A2
		protected virtual void Nokia8310ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0005D6A4 File Offset: 0x0005C6A4
		private bool Nokia8310Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia8310/1\\.0 \\((?'versionString'(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*).*)\\)"))
			{
				return false;
			}
			capabilities["canRenderOneventAndPrevElementsTogether"] = "true";
			capabilities["canRenderPostBackCards"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "2700";
			capabilities["maximumSoftkeyLabelLength"] = "21";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "8310";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersBreaksAfterWmlAnchor"] = "false";
			capabilities["rendersBreaksAfterWmlInput"] = "false";
			capabilities["requiresPhoneNumbersAsPlainText"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "3";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "39";
			capabilities["screenPixelsWidth"] = "78";
			capabilities["type"] = "Nokia 8310";
			capabilities["version"] = regexWorker["${versionString}"];
			browserCaps.AddBrowser("Nokia8310");
			this.Nokia8310ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia8310ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0005D840 File Offset: 0x0005C840
		protected virtual void Nokia9110iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0005D842 File Offset: 0x0005C842
		protected virtual void Nokia9110iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0005D844 File Offset: 0x0005C844
		private bool Nokia9110iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia9110/1\\.0"))
			{
				return false;
			}
			capabilities["cookies"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["maximumRenderedPageSize"] = "8192";
			capabilities["mobileDeviceModel"] = "9110i Communicator";
			capabilities["rendersBreaksAfterWmlAnchor"] = "false";
			capabilities["screenBitDepth"] = "4";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "60";
			capabilities["screenPixelsHeight"] = "180";
			capabilities["screenPixelsWidth"] = "400";
			capabilities["type"] = "Nokia 9110";
			browserCaps.AddBrowser("Nokia9110i");
			this.Nokia9110iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia9110iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0005D94E File Offset: 0x0005C94E
		protected virtual void Nokia9110ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0005D950 File Offset: 0x0005C950
		protected virtual void Nokia9110ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0005D954 File Offset: 0x0005C954
		private bool Nokia9110Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia-9110"))
			{
				return false;
			}
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["maximumRenderedPageSize"] = "150000";
			capabilities["mobileDeviceModel"] = "Nokia 9110";
			capabilities["numberOfSoftkeys"] = "0";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "text/html";
			capabilities["preferredRenderingType"] = "html32";
			capabilities["rendersBreaksAfterHtmlLists"] = "false";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "11";
			capabilities["screenCharactersWidth"] = "57";
			capabilities["screenPixelsHeight"] = "200";
			capabilities["screenPixelsWidth"] = "540";
			capabilities["supportsAccesskeyAttribute"] = "true";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsSelectMultiple"] = "false";
			capabilities["tables"] = "true";
			capabilities["type"] = "Nokia 9110";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("Nokia9110");
			this.Nokia9110ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia9110ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0005DB59 File Offset: 0x0005CB59
		protected virtual void Nokia3330ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0005DB5B File Offset: 0x0005CB5B
		protected virtual void Nokia3330ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0005DB60 File Offset: 0x0005CB60
		private bool Nokia3330Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia3330/1\\.0 \\((?'versionString'(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*).*)\\)"))
			{
				return false;
			}
			capabilities["hasBackButton"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "2800";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "3330";
			capabilities["screenCharactersHeight"] = "3";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "39";
			capabilities["screenPixelsWidth"] = "78";
			capabilities["type"] = "Nokia 3330";
			capabilities["version"] = regexWorker["${versionString}"];
			browserCaps.AddBrowser("Nokia3330");
			this.Nokia3330ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia3330ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0005DC7C File Offset: 0x0005CC7C
		protected virtual void Nokia9210ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0005DC7E File Offset: 0x0005CC7E
		protected virtual void Nokia9210ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0005DC80 File Offset: 0x0005CC80
		private bool Nokia9210Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia9210/1\\.0"))
			{
				return false;
			}
			capabilities["cookies"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "8192";
			capabilities["mobileDeviceModel"] = "9210 Communicator";
			capabilities["rendersBreaksAfterWmlAnchor"] = "false";
			capabilities["rendersBreaksAfterWmlInput"] = "false";
			capabilities["requiresNoSoftkeyLabels"] = "true";
			capabilities["requiresSpecialViewStateEncoding"] = "true";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "75";
			capabilities["screenPixelsHeight"] = "165";
			capabilities["screenPixelsWidth"] = "490";
			capabilities["supportsCacheControlMetaTag"] = "false";
			capabilities["type"] = "Nokia 9210";
			browserCaps.AddBrowser("Nokia9210");
			this.Nokia9210ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia9210ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0005DDDA File Offset: 0x0005CDDA
		protected virtual void Nokia9210htmlProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0005DDDC File Offset: 0x0005CDDC
		protected virtual void Nokia9210htmlProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0005DDE0 File Offset: 0x0005CDE0
		private bool Nokia9210htmlProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "EPOC32-WTL/2\\.2 Crystal/6\\.0 STNC-WTL/"))
			{
				return false;
			}
			capabilities["ExchangeOmaSupported"] = "true";
			browserCaps.AddBrowser("Nokia9210HTML");
			this.Nokia9210htmlProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia9210htmlProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0005DE4A File Offset: 0x0005CE4A
		protected virtual void Nokia3590ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0005DE4C File Offset: 0x0005CE4C
		protected virtual void Nokia3590ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0005DE50 File Offset: 0x0005CE50
		private bool Nokia3590Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Nokia3590/(?'version'(?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*)"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["cookies"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "3200";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "3590";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["requiresAbsolutePostbackUrl"] = "true";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "72";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = "Nokia 3590";
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.AddBrowser("Nokia3590");
			this.Nokia3590ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Nokia3590v1Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Nokia3590ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0005E0D9 File Offset: 0x0005D0D9
		protected virtual void Nokia3590v1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0005E0DB File Offset: 0x0005D0DB
		protected virtual void Nokia3590v1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0005E0E0 File Offset: 0x0005D0E0
		private bool Nokia3590v1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia3590/1\\.0\\(7\\."))
			{
				return false;
			}
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["maximumRenderedPageSize"] = "8020";
			capabilities["preferredRequestEncoding"] = "iso-8859-1";
			capabilities["preferredResponseEncoding"] = "utf-8";
			capabilities["screenPixelsHeight"] = "65";
			capabilities["supportsCss"] = "true";
			capabilities["supportsNoWrapStyle"] = "false";
			capabilities["supportsTitleElement"] = "true";
			capabilities["tables"] = "false";
			browserCaps.AddBrowser("Nokia3590V1");
			this.Nokia3590v1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia3590v1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0005E1CA File Offset: 0x0005D1CA
		protected virtual void Nokia3595ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0005E1CC File Offset: 0x0005D1CC
		protected virtual void Nokia3595ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0005E1D0 File Offset: 0x0005D1D0
		private bool Nokia3595Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia3595/(?'version'(?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*)"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["cookies"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "15700";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "3595";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["preferredRequestEncoding"] = "iso-8859-1";
			capabilities["preferredResponseEncoding"] = "utf-8";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersWidth"] = "17";
			capabilities["screenPixelsHeight"] = "132";
			capabilities["screenPixelsWidth"] = "176";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsNoWrapStyle"] = "false";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = "Nokia 3595";
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.AddBrowser("Nokia3595");
			this.Nokia3595ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia3595ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0005E47C File Offset: 0x0005D47C
		protected virtual void Nokia3560ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0005E47E File Offset: 0x0005D47E
		protected virtual void Nokia3560ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0005E480 File Offset: 0x0005D480
		private bool Nokia3560Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia3560"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["cookies"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "28";
			capabilities["screenPixelsHeight"] = "176";
			capabilities["screenPixelsWidth"] = "208";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsNoWrapStyle"] = "false";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("Nokia3560");
			this.Nokia3560ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia3560ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0005E6AA File Offset: 0x0005D6AA
		protected virtual void Nokia3650ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0005E6AC File Offset: 0x0005D6AC
		protected virtual void Nokia3650ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0005E6B0 File Offset: 0x0005D6B0
		private bool Nokia3650Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia3650/(?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*).* Series60/(?'platformVersion'\\S*)"))
			{
				return false;
			}
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "3650";
			capabilities["mobilePlatformVersion"] = regexWorker["${platformVersion}"];
			capabilities["type"] = "Nokia 3650";
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.AddBrowser("Nokia3650");
			this.Nokia3650ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Nokia3650p12plusProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Nokia3650ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0005E78F File Offset: 0x0005D78F
		protected virtual void Nokia3650p12plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0005E791 File Offset: 0x0005D791
		protected virtual void Nokia3650p12plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0005E794 File Offset: 0x0005D794
		private bool Nokia3650p12plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobilePlatformVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "[^01]\\.|1\\.[^01]"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["cookies"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "28";
			capabilities["screenPixelsHeight"] = "176";
			capabilities["screenPixelsWidth"] = "208";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsNoWrapStyle"] = "false";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			capabilities["tagwriter"] = "System.Web.UI.XhtmlTextWriter";
			browserCaps.AddBrowser("Nokia3650P12Plus");
			this.Nokia3650p12plusProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia3650p12plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0005E9E3 File Offset: 0x0005D9E3
		protected virtual void Nokia5100ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0005E9E5 File Offset: 0x0005D9E5
		protected virtual void Nokia5100ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0005E9E8 File Offset: 0x0005D9E8
		private bool Nokia5100Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia5100/(?'version'(?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*)"))
			{
				return false;
			}
			capabilities["canRenderOnEventAndPrevElementsTogether"] = "true";
			capabilities["canRenderPostBackCards"] = "true";
			capabilities["isColor"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "3500";
			capabilities["maximumSoftkeyLabelLength"] = "14";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "5100";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["rendersWmlSelectsAsMenuCards"] = "true";
			capabilities["requiresPhoneNumbersAsPlainText"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "15";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "128";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["type"] = "Nokia 5100";
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.AddBrowser("Nokia5100");
			this.Nokia5100ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia5100ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0005EBB4 File Offset: 0x0005DBB4
		protected virtual void Nokia6200ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0005EBB6 File Offset: 0x0005DBB6
		protected virtual void Nokia6200ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x0005EBB8 File Offset: 0x0005DBB8
		private bool Nokia6200Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia6200/(?'version'(?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*)"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["cookies"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "6200";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["preferredRequestEncoding"] = "iso-8859-1";
			capabilities["preferredResponseEncoding"] = "utf-8";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "19";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "128";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsNoWrapStyle"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = "Nokia 6200";
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.AddBrowser("Nokia6200");
			this.Nokia6200ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia6200ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x0005EE64 File Offset: 0x0005DE64
		protected virtual void Nokia6590ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0005EE66 File Offset: 0x0005DE66
		protected virtual void Nokia6590ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0005EE68 File Offset: 0x0005DE68
		private bool Nokia6590Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Nokia6590/(?'versionString'(?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*) "))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["cookies"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "9800";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "6590";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["requiresAbsolutePostbackUrl"] = "true";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "72";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = "Nokia 6590";
			capabilities["version"] = regexWorker["${versionString}"];
			browserCaps.AddBrowser("Nokia6590");
			this.Nokia6590ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia6590ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0005F0F4 File Offset: 0x0005E0F4
		protected virtual void Nokia6800ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0005F0F6 File Offset: 0x0005E0F6
		protected virtual void Nokia6800ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0005F0F8 File Offset: 0x0005E0F8
		private bool Nokia6800Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Nokia6800/(?'version'(?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*)"))
			{
				return false;
			}
			capabilities["canRenderOnEventAndPrevElementsTogether"] = "true";
			capabilities["canRenderPostBackCards"] = "true";
			capabilities["hasBackButton"] = "true";
			capabilities["isColor"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "3500";
			capabilities["maximumSoftkeyLabelLength"] = "14";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "6800";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["requiresPhoneNumbersAsPlainText"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "15";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "128";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["type"] = "Nokia 6800";
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.AddBrowser("Nokia6800");
			this.Nokia6800ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia6800ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0005F2B4 File Offset: 0x0005E2B4
		protected virtual void Nokia7650ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0005F2B6 File Offset: 0x0005E2B6
		protected virtual void Nokia7650ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0005F2B8 File Offset: 0x0005E2B8
		private bool Nokia7650Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Nokia7650/(?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*).*"))
			{
				return false;
			}
			capabilities["canRenderOnEventAndPrevElementsTogether"] = "true";
			capabilities["canRenderPostBackCards"] = "true";
			capabilities["cookies"] = "true";
			capabilities["hasBackButton"] = "true";
			capabilities["isColor"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "3500";
			capabilities["maximumSoftkeyLabelLength"] = "18";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceModel"] = "7650";
			capabilities["numberOfSoftkeys"] = "3";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["requiresPhoneNumbersAsPlainText"] = "false";
			capabilities["requiresSpecialViewStateEncoding"] = "true";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "28";
			capabilities["screenPixelsHeight"] = "208";
			capabilities["screenPixelsWidth"] = "176";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["type"] = "Nokia 7650";
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.AddBrowser("Nokia7650");
			this.Nokia7650ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokia7650ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0005F4B4 File Offset: 0x0005E4B4
		protected virtual void NokiamobilebrowserrainbowProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0005F4B6 File Offset: 0x0005E4B6
		protected virtual void NokiamobilebrowserrainbowProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0005F4B8 File Offset: 0x0005E4B8
		private bool NokiamobilebrowserrainbowProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Rainbow/(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*)"))
			{
				return false;
			}
			capabilities["browser"] = "Nokia";
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["canSendMail"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["javascript"] = "false";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "25000";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceManufacturer"] = "Nokia";
			capabilities["mobileDeviceModel"] = regexWorker["Mobile Browser ${browserMajorVersion}${browserMinorVersion}"];
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "application/vnd.wap.xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["requiresAbsolutePostbackUrl"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresInputTypeAttribute"] = "true";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "14";
			capabilities["screenCharactersWidth"] = "24";
			capabilities["screenPixelsHeight"] = "255";
			capabilities["screenPixelsWidth"] = "180";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyColor"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsDivAlign"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFontColor"] = "true";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsQueryStringInFormAction"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["supportsStyleElement"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = regexWorker["Nokia Mobile Browser ${browserMajorVersion}${browserMinorVersion}"];
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.XhtmlTextWriter";
			browserCaps.AddBrowser("NokiaMobileBrowserRainbow");
			this.NokiamobilebrowserrainbowProcessGateways(headers, browserCaps);
			bool flag = false;
			this.NokiamobilebrowserrainbowProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0005F7C0 File Offset: 0x0005E7C0
		protected virtual void Nokiaepoc32wtlProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x0005F7C2 File Offset: 0x0005E7C2
		protected virtual void Nokiaepoc32wtlProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0005F7C4 File Offset: 0x0005E7C4
		private bool Nokiaepoc32wtlProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "EPOC32-WTL/(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*)"))
			{
				return false;
			}
			capabilities["browser"] = "EPOC";
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["canSendMail"] = "false";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["maximumRenderedPageSize"] = "150000";
			capabilities["mobileDeviceManufacturer"] = "Nokia";
			capabilities["mobileDeviceModel"] = "Nokia 9210";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["rendersBreaksAfterHtmlLists"] = "false";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "54";
			capabilities["screenPixelsHeight"] = "170";
			capabilities["screenPixelsWidth"] = "478";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsSelectMultiple"] = "false";
			capabilities["supportsEmptyStringInCookieValue"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = "Nokia Epoc";
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("NokiaEpoc32wtl");
			this.Nokiaepoc32wtlProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Nokiaepoc32wtl20Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Nokiaepoc32wtlProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0005FA11 File Offset: 0x0005EA11
		protected virtual void Nokiaepoc32wtl20ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0005FA13 File Offset: 0x0005EA13
		protected virtual void Nokiaepoc32wtl20ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0005FA18 File Offset: 0x0005EA18
		private bool Nokiaepoc32wtl20Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["version"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "2\\.0"))
			{
				return false;
			}
			capabilities["canRenderEmptySelects"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "false";
			capabilities["maximumRenderedPageSize"] = "7168";
			capabilities["mobileDeviceManufacturer"] = "Psion";
			capabilities["mobileDeviceModel"] = "Series 7";
			capabilities["rendersBreaksAfterHtmlLists"] = "true";
			capabilities["requiresAttributeColonSubstitution"] = "false";
			capabilities["requiresLeadingPageBreak"] = "true";
			capabilities["requiresUniqueHtmlCheckboxNames"] = "true";
			capabilities["screenCharactersHeight"] = "31";
			capabilities["screenCharactersWidth"] = "69";
			capabilities["screenPixelsHeight"] = "72";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["SupportsEmptyStringInCookieValue"] = "true";
			browserCaps.AddBrowser("NokiaEpoc32wtl20");
			this.Nokiaepoc32wtl20ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nokiaepoc32wtl20ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x0005FB67 File Offset: 0x0005EB67
		protected virtual void UpProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x0005FB69 File Offset: 0x0005EB69
		protected virtual void UpProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x0005FB6C File Offset: 0x0005EB6C
		private bool UpProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(UP\\.Browser)|(UP/)"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			bool flag = regexWorker.ProcessRegex(text, "Go\\.Web");
			if (flag)
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "((?'deviceID'\\S*) UP/\\S* UP\\.Browser/((?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*) UP\\.Link/)|((?'deviceID'\\S*)/\\S* UP(\\.Browser)*/((?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*))|(UP\\.Browser/((?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*)-(?'deviceID'\\S*) UP\\.Link/)|((?'deviceID'\\S*) UP\\.Browser/((?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*) UP\\.Link/)|((?'deviceID'\\S*)/(?'DeviceVersion'\\S*) UP/((?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*))|((?'deviceID'\\S*)/(?'DeviceVersion'\\S*) UP.Browser/((?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*))|((?'deviceID'\\S*) UP.Browser/((?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*)\\S*))");
			capabilities["browser"] = "Phone.com";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["canSendMail"] = "false";
			capabilities["deviceID"] = regexWorker["${deviceID}"];
			capabilities["deviceVersion"] = regexWorker["${deviceVersion}"];
			capabilities["inputType"] = "telephoneKeypad";
			capabilities["isMobileDevice"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "1492";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["numberOfSoftkeys"] = "2";
			capabilities["optimumPageWeight"] = "700";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml11";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "true";
			capabilities["rendersWmlDoAcceptsInline"] = "false";
			capabilities["rendersWmlSelectsAsMenuCards"] = "true";
			capabilities["requiresFullyQualifiedRedirectUrl"] = "true";
			capabilities["requiresNoescapedPostUrl"] = "true";
			capabilities["requiresPostRedirectionHandling"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["type"] = regexWorker["Phone.com${browserMajorVersion}"];
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.AddBrowser("Up");
			this.UpProcessGateways(headers, browserCaps);
			this.UpdefaultscreencharactersProcess(headers, browserCaps);
			this.UpdefaultscreenpixelsProcess(headers, browserCaps);
			this.UpscreendepthProcess(headers, browserCaps);
			this.UpscreencharsProcess(headers, browserCaps);
			this.UpscreenpixelsProcess(headers, browserCaps);
			this.UpmsizeProcess(headers, browserCaps);
			this.IscolorProcess(headers, browserCaps);
			this.UpnumsoftkeysProcess(headers, browserCaps);
			this.UpsoftkeysizeProcess(headers, browserCaps);
			this.UpmaxpduProcess(headers, browserCaps);
			this.UpversionProcess(headers, browserCaps);
			bool flag2 = true;
			if (!this.AumicProcess(headers, browserCaps) && !this.Alcatelbe4Process(headers, browserCaps) && !this.Alcatelbe5Process(headers, browserCaps) && !this.Alcatelbe3Process(headers, browserCaps) && !this.Alcatelbf3Process(headers, browserCaps) && !this.Alcatelbf4Process(headers, browserCaps) && !this.MotcbProcess(headers, browserCaps) && !this.Motf5Process(headers, browserCaps) && !this.Motd8Process(headers, browserCaps) && !this.MotcfProcess(headers, browserCaps) && !this.Motf6Process(headers, browserCaps) && !this.MotbcProcess(headers, browserCaps) && !this.MotdcProcess(headers, browserCaps) && !this.MotpancProcess(headers, browserCaps) && !this.Motc4Process(headers, browserCaps) && !this.MccaProcess(headers, browserCaps) && !this.Mot2000Process(headers, browserCaps) && !this.Motp2kcProcess(headers, browserCaps) && !this.MotafProcess(headers, browserCaps) && !this.Motc2Process(headers, browserCaps) && !this.XeniumProcess(headers, browserCaps) && !this.Sagem959Process(headers, browserCaps) && !this.Sgha300Process(headers, browserCaps) && !this.Sghn100Process(headers, browserCaps) && !this.C304saProcess(headers, browserCaps) && !this.Sy11Process(headers, browserCaps) && !this.St12Process(headers, browserCaps) && !this.Sy14Process(headers, browserCaps) && !this.Sies40Process(headers, browserCaps) && !this.Siesl45Process(headers, browserCaps) && !this.Sies35Process(headers, browserCaps) && !this.Sieme45Process(headers, browserCaps) && !this.Sies45Process(headers, browserCaps) && !this.Gm832Process(headers, browserCaps) && !this.Gm910iProcess(headers, browserCaps) && !this.Mot32Process(headers, browserCaps) && !this.Mot28Process(headers, browserCaps) && !this.D2Process(headers, browserCaps) && !this.PpatProcess(headers, browserCaps) && !this.AlazProcess(headers, browserCaps) && !this.Cdm9100Process(headers, browserCaps) && !this.Cdm135Process(headers, browserCaps) && !this.Cdm9000Process(headers, browserCaps) && !this.C303caProcess(headers, browserCaps) && !this.C311caProcess(headers, browserCaps) && !this.C202deProcess(headers, browserCaps) && !this.C409caProcess(headers, browserCaps) && !this.C402deProcess(headers, browserCaps) && !this.Ds15Process(headers, browserCaps) && !this.Tp2200Process(headers, browserCaps) && !this.Tp120Process(headers, browserCaps) && !this.Ds10Process(headers, browserCaps) && !this.R280Process(headers, browserCaps) && !this.C201hProcess(headers, browserCaps) && !this.S71Process(headers, browserCaps) && !this.C302hProcess(headers, browserCaps) && !this.C309hProcess(headers, browserCaps) && !this.C407hProcess(headers, browserCaps) && !this.C451hProcess(headers, browserCaps) && !this.R201Process(headers, browserCaps) && !this.P21Process(headers, browserCaps) && !this.Kyocera702gProcess(headers, browserCaps) && !this.Kyocera703gProcess(headers, browserCaps) && !this.Kyocerac307kProcess(headers, browserCaps) && !this.Tk01Process(headers, browserCaps) && !this.Tk02Process(headers, browserCaps) && !this.Tk03Process(headers, browserCaps) && !this.Tk04Process(headers, browserCaps) && !this.Tk05Process(headers, browserCaps) && !this.D303kProcess(headers, browserCaps) && !this.D304kProcess(headers, browserCaps) && !this.Qcp2035Process(headers, browserCaps) && !this.Qcp3035Process(headers, browserCaps) && !this.D512Process(headers, browserCaps) && !this.Dm110Process(headers, browserCaps) && !this.Tm510Process(headers, browserCaps) && !this.Lg13Process(headers, browserCaps) && !this.P100Process(headers, browserCaps) && !this.Lgc875fProcess(headers, browserCaps) && !this.Lgp680fProcess(headers, browserCaps) && !this.Lgp7800fProcess(headers, browserCaps) && !this.Lgc840fProcess(headers, browserCaps) && !this.Lgi2100Process(headers, browserCaps) && !this.Lgp7300fProcess(headers, browserCaps) && !this.Sd500Process(headers, browserCaps) && !this.Tp1100Process(headers, browserCaps) && !this.Tp3000Process(headers, browserCaps) && !this.T250Process(headers, browserCaps) && !this.Mo01Process(headers, browserCaps) && !this.Mo02Process(headers, browserCaps) && !this.Mc01Process(headers, browserCaps) && !this.McccProcess(headers, browserCaps) && !this.Mcc9Process(headers, browserCaps) && !this.Nk00Process(headers, browserCaps) && !this.Mai12Process(headers, browserCaps) && !this.Ma112Process(headers, browserCaps) && !this.Ma13Process(headers, browserCaps) && !this.Mac1Process(headers, browserCaps) && !this.Mat1Process(headers, browserCaps) && !this.Sc01Process(headers, browserCaps) && !this.Sc03Process(headers, browserCaps) && !this.Sc02Process(headers, browserCaps) && !this.Sc04Process(headers, browserCaps) && !this.Sg08Process(headers, browserCaps) && !this.Sc13Process(headers, browserCaps) && !this.Sc11Process(headers, browserCaps) && !this.Sec01Process(headers, browserCaps) && !this.Sc10Process(headers, browserCaps) && !this.Sy12Process(headers, browserCaps) && !this.St11Process(headers, browserCaps) && !this.Sy13Process(headers, browserCaps) && !this.Syc1Process(headers, browserCaps) && !this.Sy01Process(headers, browserCaps) && !this.Syt1Process(headers, browserCaps) && !this.Sty2Process(headers, browserCaps) && !this.Sy02Process(headers, browserCaps) && !this.Sy03Process(headers, browserCaps) && !this.Si01Process(headers, browserCaps) && !this.Sni1Process(headers, browserCaps) && !this.Sn11Process(headers, browserCaps) && !this.Sn12Process(headers, browserCaps) && !this.Sn134Process(headers, browserCaps) && !this.Sn156Process(headers, browserCaps) && !this.Snc1Process(headers, browserCaps) && !this.Tsc1Process(headers, browserCaps) && !this.Tsi1Process(headers, browserCaps) && !this.Ts11Process(headers, browserCaps) && !this.Ts12Process(headers, browserCaps) && !this.Ts13Process(headers, browserCaps) && !this.Tst1Process(headers, browserCaps) && !this.Tst2Process(headers, browserCaps) && !this.Tst3Process(headers, browserCaps) && !this.Ig01Process(headers, browserCaps) && !this.Ig02Process(headers, browserCaps) && !this.Ig03Process(headers, browserCaps) && !this.Qc31Process(headers, browserCaps) && !this.Qc12Process(headers, browserCaps) && !this.Qc32Process(headers, browserCaps) && !this.Sp01Process(headers, browserCaps) && !this.ShProcess(headers, browserCaps) && !this.Upg1Process(headers, browserCaps) && !this.Opwv1Process(headers, browserCaps) && !this.AlavProcess(headers, browserCaps) && !this.Im1kProcess(headers, browserCaps) && !this.Nt95Process(headers, browserCaps) && !this.Mot2001Process(headers, browserCaps) && !this.Motv200Process(headers, browserCaps) && !this.Mot72Process(headers, browserCaps) && !this.Mot76Process(headers, browserCaps) && !this.Scp6000Process(headers, browserCaps) && !this.Motd5Process(headers, browserCaps) && !this.Motf0Process(headers, browserCaps) && !this.Sgha400Process(headers, browserCaps) && !this.Sec03Process(headers, browserCaps) && !this.Siec3iProcess(headers, browserCaps) && !this.Sn17Process(headers, browserCaps) && !this.Scp4700Process(headers, browserCaps) && !this.Sec02Process(headers, browserCaps) && !this.Sy15Process(headers, browserCaps) && !this.Db520Process(headers, browserCaps) && !this.L430v03j02Process(headers, browserCaps) && !this.OpwvsdkProcess(headers, browserCaps) && !this.Kddica21Process(headers, browserCaps) && !this.Kddits21Process(headers, browserCaps) && !this.Kddisa21Process(headers, browserCaps) && !this.Km100Process(headers, browserCaps) && !this.Lgelx5350Process(headers, browserCaps) && !this.Hitachip300Process(headers, browserCaps) && !this.Sies46Process(headers, browserCaps) && !this.Motorolav60gProcess(headers, browserCaps) && !this.Motorolav708Process(headers, browserCaps) && !this.Motorolav708aProcess(headers, browserCaps) && !this.Motorolae360Process(headers, browserCaps) && !this.Sonyericssona1101sProcess(headers, browserCaps) && !this.Philipsfisio820Process(headers, browserCaps) && !this.Casioa5302Process(headers, browserCaps) && !this.Tcll668Process(headers, browserCaps) && !this.Kddits24Process(headers, browserCaps) && !this.Sies55Process(headers, browserCaps) && !this.Sharpgx10Process(headers, browserCaps) && !this.BenqathenaProcess(headers, browserCaps))
			{
				flag2 = false;
			}
			this.UpProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x0006071F File Offset: 0x0005F71F
		protected virtual void UpdefaultscreencharactersProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x00060721 File Offset: 0x0005F721
		protected virtual void UpdefaultscreencharactersProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x00060724 File Offset: 0x0005F724
		private bool UpdefaultscreencharactersProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-SCREENCHARS"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^$"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["defaultScreenCharactersWidth"] = "15";
			capabilities["defaultScreenCharactersHeight"] = "4";
			this.UpdefaultscreencharactersProcessGateways(headers, browserCaps);
			bool flag = false;
			this.UpdefaultscreencharactersProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00060799 File Offset: 0x0005F799
		protected virtual void UpdefaultscreenpixelsProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0006079B File Offset: 0x0005F79B
		protected virtual void UpdefaultscreenpixelsProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x000607A0 File Offset: 0x0005F7A0
		private bool UpdefaultscreenpixelsProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-SCREENPIXELS"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^$"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["defaultScreenPixelsWidth"] = "120";
			capabilities["defaultScreenPixelsHeight"] = "40";
			this.UpdefaultscreenpixelsProcessGateways(headers, browserCaps);
			bool flag = false;
			this.UpdefaultscreenpixelsProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x00060815 File Offset: 0x0005F815
		protected virtual void UpscreendepthProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x00060817 File Offset: 0x0005F817
		protected virtual void UpscreendepthProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0006081C File Offset: 0x0005F81C
		private bool UpscreendepthProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-SCREENDEPTH"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'screenDepth'\\d+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["screenBitDepth"] = regexWorker["${screenDepth}"];
			this.UpscreendepthProcessGateways(headers, browserCaps);
			bool flag = false;
			this.UpscreendepthProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00060887 File Offset: 0x0005F887
		protected virtual void UpscreencharsProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00060889 File Offset: 0x0005F889
		protected virtual void UpscreencharsProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x0006088C File Offset: 0x0005F88C
		private bool UpscreencharsProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-SCREENCHARS"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'screenCharsWidth'[1-9]\\d*),(?'screenCharsHeight'[1-9]\\d*)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["screenCharactersHeight"] = regexWorker["${screenCharsHeight}"];
			capabilities["screenCharactersWidth"] = regexWorker["${screenCharsWidth}"];
			this.UpscreencharsProcessGateways(headers, browserCaps);
			bool flag = false;
			this.UpscreencharsProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0006090D File Offset: 0x0005F90D
		protected virtual void UpscreenpixelsProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x0006090F File Offset: 0x0005F90F
		protected virtual void UpscreenpixelsProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00060914 File Offset: 0x0005F914
		private bool UpscreenpixelsProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-SCREENPIXELS"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'screenPixWidth'[1-9]\\d*),(?'screenPixHeight'[1-9]\\d*)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["screenPixelsHeight"] = regexWorker["${screenPixHeight}"];
			capabilities["screenPixelsWidth"] = regexWorker["${screenPixWidth}"];
			this.UpscreenpixelsProcessGateways(headers, browserCaps);
			bool flag = false;
			this.UpscreenpixelsProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x00060995 File Offset: 0x0005F995
		protected virtual void UpmsizeProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x00060997 File Offset: 0x0005F997
		protected virtual void UpmsizeProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0006099C File Offset: 0x0005F99C
		private bool UpmsizeProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-MSIZE"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'width'\\d+),(?'height'\\d+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["characterHeight"] = regexWorker["${height}"];
			capabilities["characterWidth"] = regexWorker["${width}"];
			this.UpmsizeProcessGateways(headers, browserCaps);
			bool flag = false;
			this.UpmsizeProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x00060A1D File Offset: 0x0005FA1D
		protected virtual void IscolorProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x00060A1F File Offset: 0x0005FA1F
		protected virtual void IscolorProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x00060A24 File Offset: 0x0005FA24
		private bool IscolorProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-ISCOLOR"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, ".+"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			this.IscolorProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.IscolortrueProcess(headers, browserCaps) && !this.IscolorfalseProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.IscolorProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00060A8D File Offset: 0x0005FA8D
		protected virtual void IscolortrueProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x00060A8F File Offset: 0x0005FA8F
		protected virtual void IscolortrueProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00060A94 File Offset: 0x0005FA94
		private bool IscolortrueProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-ISCOLOR"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "1"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["isColor"] = "true";
			this.IscolortrueProcessGateways(headers, browserCaps);
			bool flag = false;
			this.IscolortrueProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x00060AF9 File Offset: 0x0005FAF9
		protected virtual void IscolorfalseProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x00060AFB File Offset: 0x0005FAFB
		protected virtual void IscolorfalseProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x00060B00 File Offset: 0x0005FB00
		private bool IscolorfalseProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-ISCOLOR"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "0"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["isColor"] = "false";
			this.IscolorfalseProcessGateways(headers, browserCaps);
			bool flag = false;
			this.IscolorfalseProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x00060B65 File Offset: 0x0005FB65
		protected virtual void UpnumsoftkeysProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x00060B67 File Offset: 0x0005FB67
		protected virtual void UpnumsoftkeysProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x00060B6C File Offset: 0x0005FB6C
		private bool UpnumsoftkeysProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-NUMSOFTKEYS"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'softkeys'\\d+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["numberOfSoftkeys"] = regexWorker["${softkeys}"];
			this.UpnumsoftkeysProcessGateways(headers, browserCaps);
			bool flag = false;
			this.UpnumsoftkeysProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00060BD7 File Offset: 0x0005FBD7
		protected virtual void UpsoftkeysizeProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00060BD9 File Offset: 0x0005FBD9
		protected virtual void UpsoftkeysizeProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00060BDC File Offset: 0x0005FBDC
		private bool UpsoftkeysizeProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-SOFTKEYSIZE"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'softkeySize'\\d+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["maximumSoftkeyLabelLength"] = regexWorker["${softkeySize}"];
			this.UpsoftkeysizeProcessGateways(headers, browserCaps);
			bool flag = false;
			this.UpsoftkeysizeProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x00060C47 File Offset: 0x0005FC47
		protected virtual void UpmaxpduProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00060C49 File Offset: 0x0005FC49
		protected virtual void UpmaxpduProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x00060C4C File Offset: 0x0005FC4C
		private bool UpmaxpduProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["X-UP-DEVCAP-MAX-PDU"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'maxDeckSize'\\d+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["maximumRenderedPageSize"] = regexWorker["${maxDeckSize}"];
			this.UpmaxpduProcessGateways(headers, browserCaps);
			bool flag = false;
			this.UpmaxpduProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00060CB7 File Offset: 0x0005FCB7
		protected virtual void UpversionProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00060CB9 File Offset: 0x0005FCB9
		protected virtual void UpversionProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00060CBC File Offset: 0x0005FCBC
		private bool UpversionProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			capabilities["type"] = regexWorker["Phone.com ${majorVersion}.x Browser"];
			this.UpversionProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.BlazerProcess(headers, browserCaps) && !this.Up4Process(headers, browserCaps) && !this.Up5Process(headers, browserCaps) && !this.Up6Process(headers, browserCaps) && !this.Up3Process(headers, browserCaps))
			{
				flag = false;
			}
			this.UpversionProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00060D35 File Offset: 0x0005FD35
		protected virtual void AumicProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00060D37 File Offset: 0x0005FD37
		protected virtual void AumicProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00060D3C File Offset: 0x0005FD3C
		private bool AumicProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "AU-MIC/(?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*).*"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "AU MIC";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "true";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "true";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "17";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsSelectFollowingTable"] = "false";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = regexWorker["AU MIC ${browserMajorVersion}"];
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.AddBrowser("AuMic");
			this.AumicProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Aumicv2Process(headers, browserCaps) && !this.A500Process(headers, browserCaps) && !this.N400Process(headers, browserCaps))
			{
				flag = false;
			}
			this.AumicProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x00060FDF File Offset: 0x0005FFDF
		protected virtual void Aumicv2ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x00060FE1 File Offset: 0x0005FFE1
		protected virtual void Aumicv2ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x00060FE4 File Offset: 0x0005FFE4
		private bool Aumicv2Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "2"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "8600";
			capabilities["screenCharactersHeight"] = "9";
			capabilities["supportsBold"] = "false";
			browserCaps.AddBrowser("AuMicV2");
			this.Aumicv2ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Aumicv2ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00061073 File Offset: 0x00060073
		protected virtual void A500ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00061075 File Offset: 0x00060075
		protected virtual void A500ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x00061078 File Offset: 0x00060078
		private bool A500Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["HTTP_X_WAP_PROFILE"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "\\/SPH-A500\\/"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "SPH-A500";
			capabilities["maximumRenderedPageSize"] = "8850";
			capabilities["preferredREnderingType"] = "xhtml-basic";
			capabilities["supportsNoWrapStyle"] = "false";
			capabilities["supportsTitleElement"] = "true";
			browserCaps.AddBrowser("a500");
			this.A500ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.A500ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00061138 File Offset: 0x00060138
		protected virtual void N400ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0006113A File Offset: 0x0006013A
		protected virtual void N400ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0006113C File Offset: 0x0006013C
		private bool N400Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["HTTP_X_WAP_PROFILE"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "\\/SPH-N400\\/"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "N400";
			capabilities["maximumRenderedPageSize"] = "46750";
			capabilities["preferredREnderingType"] = "xhtml-basic";
			capabilities["screenBitDepth"] = "24";
			capabilities["supportsNoWrapStyle"] = "false";
			capabilities["supportsTitleElement"] = "false";
			browserCaps.AddBrowser("n400");
			this.N400ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.N400ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0006120C File Offset: 0x0006020C
		protected virtual void BlazerProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0006120E File Offset: 0x0006020E
		protected virtual void BlazerProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x00061210 File Offset: 0x00060210
		private bool BlazerProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Blazer"))
			{
				return false;
			}
			capabilities["browser"] = "Handspring Blazer";
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["cookies"] = "true";
			capabilities["defaultScreenCharactersHeight"] = "6";
			capabilities["defaultScreenCharactersWidth"] = "12";
			capabilities["defaultScreenPixelsHeight"] = "72";
			capabilities["defaultScreenPixelsWidth"] = "96";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["maximumRenderedPageSize"] = "7000";
			capabilities["mobileDeviceManufacturer"] = "PalmOS-licensee";
			capabilities["numberOfSoftkeys"] = "0";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "text/html";
			capabilities["preferredRenderingType"] = "html32";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["rendersWmlSelectsAsMenuCards"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "12";
			capabilities["screenCharactersWidth"] = "36";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "160";
			capabilities["supportsBold"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["type"] = "Handspring Blazer";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			this.BlazerProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Blazerupg1Process(headers, browserCaps))
			{
				flag = false;
			}
			this.BlazerProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x00061467 File Offset: 0x00060467
		protected virtual void Blazerupg1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x00061469 File Offset: 0x00060469
		protected virtual void Blazerupg1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0006146C File Offset: 0x0006046C
		private bool Blazerupg1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "UPG1 UP/\\S* \\(compatible; Blazer (?'browserMajorVersion'\\d+)(?'browserMinorVersion'\\.\\d+)"))
			{
				return false;
			}
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			this.Blazerupg1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Blazerupg1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x000614FD File Offset: 0x000604FD
		protected virtual void Up4ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x000614FF File Offset: 0x000604FF
		protected virtual void Up4ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00061504 File Offset: 0x00060504
		private bool Up4Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "4"))
			{
				return false;
			}
			capabilities["requiresAbsolutePostbackUrl"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			this.Up4ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Up4ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00061578 File Offset: 0x00060578
		protected virtual void Up5ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0006157A File Offset: 0x0006057A
		protected virtual void Up5ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0006157C File Offset: 0x0006057C
		private bool Up5Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "5"))
			{
				return false;
			}
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			this.Up5ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Up5ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x000615E0 File Offset: 0x000605E0
		protected virtual void Up6ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x000615E2 File Offset: 0x000605E2
		protected virtual void Up6ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x000615E4 File Offset: 0x000605E4
		private bool Up6Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "6"))
			{
				return false;
			}
			capabilities["cookies"] = "true";
			capabilities["preferredRenderingMime"] = "text/html";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["supportsStyleElement"] = "true";
			capabilities["type"] = regexWorker["Openwave ${majorVersion}.x Browser"];
			browserCaps.HtmlTextWriter = "System.Web.UI.XhtmlTextWriter";
			this.Up6ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Up61plusProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Up6ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x000616A6 File Offset: 0x000606A6
		protected virtual void Up61plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x000616A8 File Offset: 0x000606A8
		protected virtual void Up61plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x000616AC File Offset: 0x000606AC
		private bool Up61plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["minorVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^\\.[^0]"))
			{
				return false;
			}
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			this.Up61plusProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Up61plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00061710 File Offset: 0x00060710
		protected virtual void Up3ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00061712 File Offset: 0x00060712
		protected virtual void Up3ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00061714 File Offset: 0x00060714
		private bool Up3Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "3"))
			{
				return false;
			}
			capabilities["canRenderInputAndSelectElementsTogether"] = "false";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["requiresAbsolutePostbackUrl"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["requiresUrlEncodedPostfieldValues"] = "true";
			capabilities["type"] = "Phone.com 3.x Browser";
			this.Up3ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Up3ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x000617C8 File Offset: 0x000607C8
		protected virtual void Alcatelbe4ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x000617CA File Offset: 0x000607CA
		protected virtual void Alcatelbe4ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x000617CC File Offset: 0x000607CC
		private bool Alcatelbe4Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Alcatel-BE4"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Alcatel";
			capabilities["mobileDeviceModel"] = "301";
			browserCaps.AddBrowser("AlcatelBe4");
			this.Alcatelbe4ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Alcatelbe4ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0006184B File Offset: 0x0006084B
		protected virtual void Alcatelbe5ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0006184D File Offset: 0x0006084D
		protected virtual void Alcatelbe5ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00061850 File Offset: 0x00060850
		private bool Alcatelbe5Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Alcatel-BE5"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Alcatel";
			capabilities["mobileDeviceModel"] = "501, 701";
			browserCaps.AddBrowser("AlcatelBe5");
			this.Alcatelbe5ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Alcatelbe5v2Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Alcatelbe5ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x000618DC File Offset: 0x000608DC
		protected virtual void Alcatelbe5v2ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x000618DE File Offset: 0x000608DE
		protected virtual void Alcatelbe5v2ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x000618E0 File Offset: 0x000608E0
		private bool Alcatelbe5v2Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "2\\.0"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "1900";
			capabilities["maximumSoftkeyLabelLength"] = "0";
			capabilities["mobileDeviceModel"] = "Alcatel One Touch 501";
			capabilities["numberOfSoftkeys"] = "10";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["requiresNoSoftkeyLabels"] = "true";
			capabilities["screenBitDepth"] = "0";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "14";
			capabilities["screenPixelsHeight"] = "60";
			capabilities["screenPixelsWidth"] = "91";
			capabilities["supportsBold"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("AlcatelBe5v2");
			this.Alcatelbe5v2ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Alcatelbe5v2ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00061A0A File Offset: 0x00060A0A
		protected virtual void Alcatelbe3ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00061A0C File Offset: 0x00060A0C
		protected virtual void Alcatelbe3ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00061A10 File Offset: 0x00060A10
		private bool Alcatelbe3Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Alcatel-BE3"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Alcatel";
			capabilities["mobileDeviceModel"] = "OneTouchDB";
			browserCaps.AddBrowser("AlcatelBe3");
			this.Alcatelbe3ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Alcatelbe3ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00061A8F File Offset: 0x00060A8F
		protected virtual void Alcatelbf3ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00061A91 File Offset: 0x00060A91
		protected virtual void Alcatelbf3ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00061A94 File Offset: 0x00060A94
		private bool Alcatelbf3Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Alcatel-BF3"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "1900";
			capabilities["maximumSoftkeyLabelLength"] = "13";
			capabilities["mobileDeviceManufacturer"] = "Alcatel";
			capabilities["mobileDeviceModel"] = "Alcatel One Touch 311";
			capabilities["numberOfSoftkeys"] = "10";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "11";
			capabilities["screenPixelsHeight"] = "65";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("AlcatelBf3");
			this.Alcatelbf3ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Alcatelbf3ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00061B93 File Offset: 0x00060B93
		protected virtual void Alcatelbf4ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00061B95 File Offset: 0x00060B95
		protected virtual void Alcatelbf4ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00061B98 File Offset: 0x00060B98
		private bool Alcatelbf4Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Alcatel-BF4"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "1900";
			capabilities["maximumSoftkeyLabelLength"] = "13";
			capabilities["mobileDeviceManufacturer"] = "Alcatel";
			capabilities["mobileDeviceModel"] = "Alcatel One Touch 511";
			capabilities["numberOfSoftkeys"] = "10";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "11";
			capabilities["screenPixelsHeight"] = "60";
			capabilities["screenPixelsWidth"] = "89";
			browserCaps.AddBrowser("AlcatelBf4");
			this.Alcatelbf4ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Alcatelbf4ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00061C97 File Offset: 0x00060C97
		protected virtual void MotcbProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x00061C99 File Offset: 0x00060C99
		protected virtual void MotcbProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00061C9C File Offset: 0x00060C9C
		private bool MotcbProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-CB"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Timeport P7389";
			capabilities["numberOfSoftkeys"] = "1";
			browserCaps.AddBrowser("MotCb");
			this.MotcbProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MotcbProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00061D2B File Offset: 0x00060D2B
		protected virtual void Motf5ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x00061D2D File Offset: 0x00060D2D
		protected virtual void Motf5ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x00061D30 File Offset: 0x00060D30
		private bool Motf5Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-F5"))
			{
				return false;
			}
			capabilities["cookies"] = "false";
			capabilities["maximumRenderedPageSize"] = "1900";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Talkabout 192";
			capabilities["numberOfSoftkeys"] = "3";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "40";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("MotF5");
			this.Motf5ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motf5ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00061E2F File Offset: 0x00060E2F
		protected virtual void Motd8ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x00061E31 File Offset: 0x00060E31
		protected virtual void Motd8ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x00061E34 File Offset: 0x00060E34
		private bool Motd8Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-D8"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Timeport 250/P7689";
			browserCaps.AddBrowser("MotD8");
			this.Motd8ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motd8ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x00061EB3 File Offset: 0x00060EB3
		protected virtual void MotcfProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x00061EB5 File Offset: 0x00060EB5
		protected virtual void MotcfProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00061EB8 File Offset: 0x00060EB8
		private bool MotcfProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-CF"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Accompli 6188";
			browserCaps.AddBrowser("MotCf");
			this.MotcfProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MotcfProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x00061F37 File Offset: 0x00060F37
		protected virtual void Motf6ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00061F39 File Offset: 0x00060F39
		protected virtual void Motf6ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00061F3C File Offset: 0x00060F3C
		private bool Motf6Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-F6"))
			{
				return false;
			}
			capabilities["cookies"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["maximumRenderedPageSize"] = "5000";
			capabilities["maximumSoftkeyLabelLength"] = "6";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Accompli 008/6288";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["rendersWmlSelectsAsMenuCards"] = "false";
			capabilities["screenBitDepth"] = "4";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "320";
			capabilities["screenPixelsWidth"] = "240";
			browserCaps.AddBrowser("MotF6");
			this.Motf6ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motf6ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0006206B File Offset: 0x0006106B
		protected virtual void MotbcProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0006206D File Offset: 0x0006106D
		protected virtual void MotbcProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00062070 File Offset: 0x00061070
		private bool MotbcProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-BC"))
			{
				return false;
			}
			capabilities["inputType"] = "keyboard";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Accompli 009";
			browserCaps.AddBrowser("MotBc");
			this.MotbcProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MotbcProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x000620FF File Offset: 0x000610FF
		protected virtual void MotdcProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x00062101 File Offset: 0x00061101
		protected virtual void MotdcProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00062104 File Offset: 0x00061104
		private bool MotdcProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-DC"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "V3682, V50";
			browserCaps.AddBrowser("MotDc");
			this.MotdcProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MotdcProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x00062183 File Offset: 0x00061183
		protected virtual void MotpancProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x00062185 File Offset: 0x00061185
		protected virtual void MotpancProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00062188 File Offset: 0x00061188
		private bool MotpancProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-PAN-C"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Timeport 270c";
			browserCaps.AddBrowser("MotPanC");
			this.MotpancProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MotpancProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00062207 File Offset: 0x00061207
		protected virtual void Motc4ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00062209 File Offset: 0x00061209
		protected virtual void Motc4ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0006220C File Offset: 0x0006120C
		private bool Motc4Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-C4"))
			{
				return false;
			}
			capabilities["canRenderMixedSelects"] = "false";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "V2288, V2282";
			capabilities["supportsCacheControlMetaTag"] = "false";
			browserCaps.AddBrowser("MotC4");
			this.Motc4ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motc4ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x000622AB File Offset: 0x000612AB
		protected virtual void MccaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x000622AD File Offset: 0x000612AD
		protected virtual void MccaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x000622B0 File Offset: 0x000612B0
		private bool MccaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MCCA"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Timeport 8767/ST7868";
			browserCaps.AddBrowser("Mcca");
			this.MccaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.MccaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0006232F File Offset: 0x0006132F
		protected virtual void Mot2000ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x00062331 File Offset: 0x00061331
		protected virtual void Mot2000ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x00062334 File Offset: 0x00061334
		private bool Mot2000Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-2000"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "V60c";
			browserCaps.AddBrowser("Mot2000");
			this.Mot2000ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mot2000ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x000623B3 File Offset: 0x000613B3
		protected virtual void Motp2kcProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x000623B5 File Offset: 0x000613B5
		protected virtual void Motp2kcProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x000623B8 File Offset: 0x000613B8
		private bool Motp2kcProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-P2K-C"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "V120c";
			browserCaps.AddBrowser("MotP2kC");
			this.Motp2kcProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motp2kcProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x00062437 File Offset: 0x00061437
		protected virtual void MotafProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x00062439 File Offset: 0x00061439
		protected virtual void MotafProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0006243C File Offset: 0x0006143C
		private bool MotafProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-AF"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Timeport 260/P7382i/P7389i";
			capabilities["screenCharactersHeight"] = "4";
			browserCaps.AddBrowser("MotAf");
			this.MotafProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Motaf418Process(headers, browserCaps))
			{
				flag = false;
			}
			this.MotafProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x000624D8 File Offset: 0x000614D8
		protected virtual void Motaf418ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x000624DA File Offset: 0x000614DA
		protected virtual void Motaf418ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x000624DC File Offset: 0x000614DC
		private bool Motaf418Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["DeviceVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "4\\.1\\.8"))
			{
				return false;
			}
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["maximumRenderedPageSize"] = "1900";
			capabilities["maximumSoftkeyLabelLength"] = "5";
			capabilities["mobileDeviceModel"] = "Timeport 260";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "64";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsCacheControlMetaTag"] = "false";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("MotAf418");
			this.Motaf418ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motaf418ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x000625DB File Offset: 0x000615DB
		protected virtual void Motc2ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x000625DD File Offset: 0x000615DD
		protected virtual void Motc2ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x000625E0 File Offset: 0x000615E0
		private bool Motc2Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-C2"))
			{
				return false;
			}
			capabilities["inputType"] = "keyboard";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "V100, V.Box";
			browserCaps.AddBrowser("MotC2");
			this.Motc2ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motc2ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0006266F File Offset: 0x0006166F
		protected virtual void XeniumProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00062671 File Offset: 0x00061671
		protected virtual void XeniumProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x00062674 File Offset: 0x00061674
		private bool XeniumProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Philips-Xenium9@9"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Philips";
			capabilities["mobileDeviceModel"] = "Xenium 9";
			browserCaps.AddBrowser("Xenium");
			this.XeniumProcessGateways(headers, browserCaps);
			bool flag = false;
			this.XeniumProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x000626F3 File Offset: 0x000616F3
		protected virtual void Sagem959ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x000626F5 File Offset: 0x000616F5
		protected virtual void Sagem959ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x000626F8 File Offset: 0x000616F8
		private bool Sagem959Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Sagem-959"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sagem";
			capabilities["mobileDeviceModel"] = "MW-959";
			browserCaps.AddBrowser("Sagem959");
			this.Sagem959ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sagem959ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x00062777 File Offset: 0x00061777
		protected virtual void Sgha300ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x00062779 File Offset: 0x00061779
		protected virtual void Sgha300ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0006277C File Offset: 0x0006177C
		private bool Sgha300Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SAMSUNG-SGH-A300"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2000";
			capabilities["maximumSoftkeyLabelLength"] = "19";
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "SGH-A300";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "13";
			capabilities["screenPixelsHeight"] = "128";
			capabilities["screenPixelsWidth"] = "128";
			browserCaps.AddBrowser("SghA300");
			this.Sgha300ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sgha300ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0006285B File Offset: 0x0006185B
		protected virtual void Sghn100ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x0006285D File Offset: 0x0006185D
		protected virtual void Sghn100ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x00062860 File Offset: 0x00061860
		private bool Sghn100Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Samsung-SGH-N100/"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "SGH-N100";
			browserCaps.AddBrowser("SghN100");
			this.Sghn100ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sghn100ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x000628DF File Offset: 0x000618DF
		protected virtual void C304saProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x000628E1 File Offset: 0x000618E1
		protected virtual void C304saProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x000628E4 File Offset: 0x000618E4
		private bool C304saProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Sanyo-C304SA/"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "C304SA";
			browserCaps.AddBrowser("C304sa");
			this.C304saProcessGateways(headers, browserCaps);
			bool flag = false;
			this.C304saProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x00062963 File Offset: 0x00061963
		protected virtual void Sy11ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x00062965 File Offset: 0x00061965
		protected virtual void Sy11ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x00062968 File Offset: 0x00061968
		private bool Sy11Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SY11"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "C304SA";
			browserCaps.AddBrowser("Sy11");
			this.Sy11ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sy11ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x000629E7 File Offset: 0x000619E7
		protected virtual void St12ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x000629E9 File Offset: 0x000619E9
		protected virtual void St12ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x000629EC File Offset: 0x000619EC
		private bool St12Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "ST12"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "C411ST";
			browserCaps.AddBrowser("St12");
			this.St12ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.St12ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x00062A6B File Offset: 0x00061A6B
		protected virtual void Sy14ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x00062A6D File Offset: 0x00061A6D
		protected virtual void Sy14ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00062A70 File Offset: 0x00061A70
		private bool Sy14Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SY14"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "C412SA";
			browserCaps.AddBrowser("Sy14");
			this.Sy14ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sy14ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00062AEF File Offset: 0x00061AEF
		protected virtual void Sies40ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00062AF1 File Offset: 0x00061AF1
		protected virtual void Sies40ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x00062AF4 File Offset: 0x00061AF4
		private bool Sies40Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SIE-S40"))
			{
				return false;
			}
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["maximumRenderedPageSize"] = "2048";
			capabilities["mobileDeviceManufacturer"] = "Siemens";
			capabilities["mobileDeviceModel"] = "S40, S42";
			browserCaps.AddBrowser("SieS40");
			this.Sies40ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sies40ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x00062B93 File Offset: 0x00061B93
		protected virtual void Siesl45ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x00062B95 File Offset: 0x00061B95
		protected virtual void Siesl45ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x00062B98 File Offset: 0x00061B98
		private bool Siesl45Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SIE-SL45"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Siemens";
			capabilities["mobileDeviceModel"] = "SL-45";
			browserCaps.AddBrowser("SieSl45");
			this.Siesl45ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Siesl45ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x00062C17 File Offset: 0x00061C17
		protected virtual void Sies35ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x00062C19 File Offset: 0x00061C19
		protected virtual void Sies35ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00062C1C File Offset: 0x00061C1C
		private bool Sies35Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SIE-S35"))
			{
				return false;
			}
			capabilities["canRenderMixedSelects"] = "false";
			capabilities["mobileDeviceManufacturer"] = "Siemens";
			capabilities["mobileDeviceModel"] = "S35";
			browserCaps.AddBrowser("SieS35");
			this.Sies35ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sies35ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x00062CAB File Offset: 0x00061CAB
		protected virtual void Sieme45ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x00062CAD File Offset: 0x00061CAD
		protected virtual void Sieme45ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x00062CB0 File Offset: 0x00061CB0
		private bool Sieme45Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SIE-ME45"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2800";
			capabilities["maximumSoftkeyLabelLength"] = "7";
			capabilities["mobileDeviceManufacturer"] = "Siemens";
			capabilities["mobileDeviceModel"] = "ME45";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "65";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontSize"] = "true";
			browserCaps.AddBrowser("SieMe45");
			this.Sieme45ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sieme45ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x00062DEF File Offset: 0x00061DEF
		protected virtual void Sies45ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00062DF1 File Offset: 0x00061DF1
		protected virtual void Sies45ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00062DF4 File Offset: 0x00061DF4
		private bool Sies45Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SIE-S45"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2765";
			capabilities["maximumSoftkeyLabelLength"] = "7";
			capabilities["mobileDeviceManufacturer"] = "Siemens";
			capabilities["mobileDeviceModel"] = "S45";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "64";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontSize"] = "true";
			browserCaps.AddBrowser("SieS45");
			this.Sies45ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sies45ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00062F33 File Offset: 0x00061F33
		protected virtual void Gm832ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x00062F35 File Offset: 0x00061F35
		protected virtual void Gm832ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00062F38 File Offset: 0x00061F38
		private bool Gm832Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "GM832"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Telit";
			capabilities["mobileDeviceModel"] = "GM832";
			browserCaps.AddBrowser("Gm832");
			this.Gm832ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Gm832ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00062FB7 File Offset: 0x00061FB7
		protected virtual void Gm910iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00062FB9 File Offset: 0x00061FB9
		protected virtual void Gm910iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00062FBC File Offset: 0x00061FBC
		private bool Gm910iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Telit-GM910i"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Telit";
			capabilities["mobileDeviceModel"] = "GM910i";
			browserCaps.AddBrowser("Gm910i");
			this.Gm910iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Gm910iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x0006303B File Offset: 0x0006203B
		protected virtual void Mot32ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0006303D File Offset: 0x0006203D
		protected virtual void Mot32ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x00063040 File Offset: 0x00062040
		private bool Mot32Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-32"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "i85s, i50sx";
			browserCaps.AddBrowser("Mot32");
			this.Mot32ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mot32ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x000630BF File Offset: 0x000620BF
		protected virtual void Mot28ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x000630C1 File Offset: 0x000620C1
		protected virtual void Mot28ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x000630C4 File Offset: 0x000620C4
		private bool Mot28Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-28"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "i700+, i1000+";
			browserCaps.AddBrowser("Mot28");
			this.Mot28ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mot28ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x00063143 File Offset: 0x00062143
		protected virtual void D2ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x00063145 File Offset: 0x00062145
		protected virtual void D2ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x00063148 File Offset: 0x00062148
		private bool D2Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "D2"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["mobileDeviceModel"] = "D2";
			browserCaps.AddBrowser("D2");
			this.D2ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.D2ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x000631C7 File Offset: 0x000621C7
		protected virtual void PpatProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x000631C9 File Offset: 0x000621C9
		protected virtual void PpatProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x000631CC File Offset: 0x000621CC
		private bool PpatProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "P-PAT"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["mobileDeviceModel"] = "P-PAT";
			browserCaps.AddBrowser("PPat");
			this.PpatProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PpatProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0006324B File Offset: 0x0006224B
		protected virtual void AlazProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0006324D File Offset: 0x0006224D
		protected virtual void AlazProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x00063250 File Offset: 0x00062250
		private bool AlazProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "ALAZ"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Alcatel";
			capabilities["mobileDeviceModel"] = "OneTouch";
			browserCaps.AddBrowser("Alaz");
			this.AlazProcessGateways(headers, browserCaps);
			bool flag = false;
			this.AlazProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x000632CF File Offset: 0x000622CF
		protected virtual void Cdm9100ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x000632D1 File Offset: 0x000622D1
		protected virtual void Cdm9100ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x000632D4 File Offset: 0x000622D4
		private bool Cdm9100Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "AUDIOVOX-CDM9100"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Audiovox";
			capabilities["mobileDeviceModel"] = "CDM-9100";
			browserCaps.AddBrowser("Cdm9100");
			this.Cdm9100ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Cdm9100ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00063353 File Offset: 0x00062353
		protected virtual void Cdm135ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x00063355 File Offset: 0x00062355
		protected virtual void Cdm135ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x00063358 File Offset: 0x00062358
		private bool Cdm135Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "HD-MMD1010"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Audiovox";
			capabilities["mobileDeviceModel"] = "CDM-135";
			browserCaps.AddBrowser("Cdm135");
			this.Cdm135ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Cdm135ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x000633D7 File Offset: 0x000623D7
		protected virtual void Cdm9000ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x000633D9 File Offset: 0x000623D9
		protected virtual void Cdm9000ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x000633DC File Offset: 0x000623DC
		private bool Cdm9000Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "TSCA"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Audiovox";
			capabilities["mobileDeviceModel"] = "CDM-9000";
			browserCaps.AddBrowser("Cdm9000");
			this.Cdm9000ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Cdm9000ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0006345B File Offset: 0x0006245B
		protected virtual void C303caProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0006345D File Offset: 0x0006245D
		protected virtual void C303caProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00063460 File Offset: 0x00062460
		private bool C303caProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "CA11"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Casio";
			capabilities["mobileDeviceModel"] = "C303CA";
			browserCaps.AddBrowser("C303ca");
			this.C303caProcessGateways(headers, browserCaps);
			bool flag = false;
			this.C303caProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x000634DF File Offset: 0x000624DF
		protected virtual void C311caProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x000634E1 File Offset: 0x000624E1
		protected virtual void C311caProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x000634E4 File Offset: 0x000624E4
		private bool C311caProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "CA12"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Casio";
			capabilities["mobileDeviceModel"] = "C311CA";
			browserCaps.AddBrowser("C311ca");
			this.C311caProcessGateways(headers, browserCaps);
			bool flag = false;
			this.C311caProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00063563 File Offset: 0x00062563
		protected virtual void C202deProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x00063565 File Offset: 0x00062565
		protected virtual void C202deProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x00063568 File Offset: 0x00062568
		private bool C202deProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "DN01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Casio";
			capabilities["mobileDeviceModel"] = "C202DE";
			browserCaps.AddBrowser("C202de");
			this.C202deProcessGateways(headers, browserCaps);
			bool flag = false;
			this.C202deProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x000635E7 File Offset: 0x000625E7
		protected virtual void C409caProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x000635E9 File Offset: 0x000625E9
		protected virtual void C409caProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x000635EC File Offset: 0x000625EC
		private bool C409caProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "CA13"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Casio";
			capabilities["mobileDeviceModel"] = "C409CA";
			browserCaps.AddBrowser("C409ca");
			this.C409caProcessGateways(headers, browserCaps);
			bool flag = false;
			this.C409caProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0006366B File Offset: 0x0006266B
		protected virtual void C402deProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0006366D File Offset: 0x0006266D
		protected virtual void C402deProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x00063670 File Offset: 0x00062670
		private bool C402deProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "DN11"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Denso";
			capabilities["mobileDeviceModel"] = "C402DE";
			browserCaps.AddBrowser("C402de");
			this.C402deProcessGateways(headers, browserCaps);
			bool flag = false;
			this.C402deProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x000636EF File Offset: 0x000626EF
		protected virtual void Ds15ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x000636F1 File Offset: 0x000626F1
		protected virtual void Ds15ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x000636F4 File Offset: 0x000626F4
		private bool Ds15Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "DS15"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Denso";
			capabilities["mobileDeviceModel"] = "Touchpoint DS15";
			browserCaps.AddBrowser("Ds15");
			this.Ds15ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ds15ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00063773 File Offset: 0x00062773
		protected virtual void Tp2200ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00063775 File Offset: 0x00062775
		protected virtual void Tp2200ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x00063778 File Offset: 0x00062778
		private bool Tp2200Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "DS1[34]"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Denso";
			capabilities["mobileDeviceModel"] = "TouchPoint TP2200";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "15";
			browserCaps.AddBrowser("Tp2200");
			this.Tp2200ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tp2200ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x00063817 File Offset: 0x00062817
		protected virtual void Tp120ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00063819 File Offset: 0x00062819
		protected virtual void Tp120ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0006381C File Offset: 0x0006281C
		private bool Tp120Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "DS12"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Denso";
			capabilities["mobileDeviceModel"] = "TouchPoint TP120";
			browserCaps.AddBrowser("Tp120");
			this.Tp120ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tp120ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0006389B File Offset: 0x0006289B
		protected virtual void Ds10ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x0006389D File Offset: 0x0006289D
		protected virtual void Ds10ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x000638A0 File Offset: 0x000628A0
		private bool Ds10Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "DS10"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Denso";
			capabilities["mobileDeviceModel"] = "Eagle 10";
			browserCaps.AddBrowser("Ds10");
			this.Ds10ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ds10ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0006391F File Offset: 0x0006291F
		protected virtual void R280ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x00063921 File Offset: 0x00062921
		protected virtual void R280ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x00063924 File Offset: 0x00062924
		private bool R280Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "ERK0"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Ericsson";
			capabilities["mobileDeviceModel"] = "R280";
			browserCaps.AddBrowser("R280");
			this.R280ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.R280ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x000639A3 File Offset: 0x000629A3
		protected virtual void C201hProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x000639A5 File Offset: 0x000629A5
		protected virtual void C201hProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x000639A8 File Offset: 0x000629A8
		private bool C201hProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "HI01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Hitachi";
			capabilities["mobileDeviceModel"] = "C201H";
			browserCaps.AddBrowser("C201h");
			this.C201hProcessGateways(headers, browserCaps);
			bool flag = false;
			this.C201hProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00063A27 File Offset: 0x00062A27
		protected virtual void S71ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x00063A29 File Offset: 0x00062A29
		protected virtual void S71ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x00063A2C File Offset: 0x00062A2C
		private bool S71Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "HW01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Hanwha";
			capabilities["mobileDeviceModel"] = "S71";
			browserCaps.AddBrowser("S71");
			this.S71ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.S71ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x00063AAB File Offset: 0x00062AAB
		protected virtual void C302hProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x00063AAD File Offset: 0x00062AAD
		protected virtual void C302hProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x00063AB0 File Offset: 0x00062AB0
		private bool C302hProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "HI11"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Hitachi";
			capabilities["mobileDeviceModel"] = "C302H";
			browserCaps.AddBrowser("C302h");
			this.C302hProcessGateways(headers, browserCaps);
			bool flag = false;
			this.C302hProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x00063B2F File Offset: 0x00062B2F
		protected virtual void C309hProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00063B31 File Offset: 0x00062B31
		protected virtual void C309hProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x00063B34 File Offset: 0x00062B34
		private bool C309hProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "HI12"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Hitachi";
			capabilities["mobileDeviceModel"] = "C309H";
			browserCaps.AddBrowser("C309h");
			this.C309hProcessGateways(headers, browserCaps);
			bool flag = false;
			this.C309hProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00063BB3 File Offset: 0x00062BB3
		protected virtual void C407hProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x00063BB5 File Offset: 0x00062BB5
		protected virtual void C407hProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x00063BB8 File Offset: 0x00062BB8
		private bool C407hProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "HI13"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Hitachi";
			capabilities["mobileDeviceModel"] = "C407H";
			browserCaps.AddBrowser("C407h");
			this.C407hProcessGateways(headers, browserCaps);
			bool flag = false;
			this.C407hProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x00063C47 File Offset: 0x00062C47
		protected virtual void C451hProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x00063C49 File Offset: 0x00062C49
		protected virtual void C451hProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x00063C4C File Offset: 0x00062C4C
		private bool C451hProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "HI14"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Hitachi";
			capabilities["mobileDeviceModel"] = "C451H";
			browserCaps.AddBrowser("C451h");
			this.C451hProcessGateways(headers, browserCaps);
			bool flag = false;
			this.C451hProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00063CCB File Offset: 0x00062CCB
		protected virtual void R201ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x00063CCD File Offset: 0x00062CCD
		protected virtual void R201ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x00063CD0 File Offset: 0x00062CD0
		private bool R201Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "HD03"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Hyundai";
			capabilities["mobileDeviceModel"] = "HGC-R201";
			browserCaps.AddBrowser("R201");
			this.R201ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.R201ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x00063D4F File Offset: 0x00062D4F
		protected virtual void P21ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00063D51 File Offset: 0x00062D51
		protected virtual void P21ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x00063D54 File Offset: 0x00062D54
		private bool P21Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "HD02"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Hyundai";
			capabilities["mobileDeviceModel"] = "P-21";
			browserCaps.AddBrowser("P21");
			this.P21ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.P21ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x00063DD3 File Offset: 0x00062DD3
		protected virtual void Kyocera702gProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x00063DD5 File Offset: 0x00062DD5
		protected virtual void Kyocera702gProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x00063DD8 File Offset: 0x00062DD8
		private bool Kyocera702gProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KCI1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "702G";
			browserCaps.AddBrowser("Kyocera702g");
			this.Kyocera702gProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Kyocera702gProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x00063E57 File Offset: 0x00062E57
		protected virtual void Kyocera703gProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x00063E59 File Offset: 0x00062E59
		protected virtual void Kyocera703gProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x00063E5C File Offset: 0x00062E5C
		private bool Kyocera703gProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KCI2"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "703G";
			browserCaps.AddBrowser("Kyocera703g");
			this.Kyocera703gProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Kyocera703gProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00063EDB File Offset: 0x00062EDB
		protected virtual void Kyocerac307kProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00063EDD File Offset: 0x00062EDD
		protected virtual void Kyocerac307kProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00063EE0 File Offset: 0x00062EE0
		private bool Kyocerac307kProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KC11"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "C307K";
			browserCaps.AddBrowser("KyoceraC307k");
			this.Kyocerac307kProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Kyocerac307kProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00063F5F File Offset: 0x00062F5F
		protected virtual void Tk01ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00063F61 File Offset: 0x00062F61
		protected virtual void Tk01ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00063F64 File Offset: 0x00062F64
		private bool Tk01Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KCT1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "TK01";
			browserCaps.AddBrowser("Tk01");
			this.Tk01ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tk01ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x00063FE3 File Offset: 0x00062FE3
		protected virtual void Tk02ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x00063FE5 File Offset: 0x00062FE5
		protected virtual void Tk02ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x00063FE8 File Offset: 0x00062FE8
		private bool Tk02Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KCT2"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "TK02";
			browserCaps.AddBrowser("Tk02");
			this.Tk02ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tk02ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x00064067 File Offset: 0x00063067
		protected virtual void Tk03ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x00064069 File Offset: 0x00063069
		protected virtual void Tk03ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0006406C File Offset: 0x0006306C
		private bool Tk03Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KCT4"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "TK03";
			browserCaps.AddBrowser("Tk03");
			this.Tk03ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tk03ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x000640EB File Offset: 0x000630EB
		protected virtual void Tk04ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x000640ED File Offset: 0x000630ED
		protected virtual void Tk04ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x000640F0 File Offset: 0x000630F0
		private bool Tk04Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KCT5"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "TK04";
			browserCaps.AddBrowser("Tk04");
			this.Tk04ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tk04ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0006416F File Offset: 0x0006316F
		protected virtual void Tk05ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x00064171 File Offset: 0x00063171
		protected virtual void Tk05ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x00064174 File Offset: 0x00063174
		private bool Tk05Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KCT6"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "TK05";
			browserCaps.AddBrowser("Tk05");
			this.Tk05ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tk05ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x000641F3 File Offset: 0x000631F3
		protected virtual void D303kProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x000641F5 File Offset: 0x000631F5
		protected virtual void D303kProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x000641F8 File Offset: 0x000631F8
		private bool D303kProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KCC1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "D303K";
			browserCaps.AddBrowser("D303k");
			this.D303kProcessGateways(headers, browserCaps);
			bool flag = false;
			this.D303kProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x00064277 File Offset: 0x00063277
		protected virtual void D304kProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00064279 File Offset: 0x00063279
		protected virtual void D304kProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0006427C File Offset: 0x0006327C
		private bool D304kProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KCC2"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "D304K";
			browserCaps.AddBrowser("D304k");
			this.D304kProcessGateways(headers, browserCaps);
			bool flag = false;
			this.D304kProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x000642FB File Offset: 0x000632FB
		protected virtual void Qcp2035ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x000642FD File Offset: 0x000632FD
		protected virtual void Qcp2035ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00064300 File Offset: 0x00063300
		private bool Qcp2035Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "QC06"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "QCP2035/2037";
			browserCaps.AddBrowser("Qcp2035");
			this.Qcp2035ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Qcp2035ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0006437F File Offset: 0x0006337F
		protected virtual void Qcp3035ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x00064381 File Offset: 0x00063381
		protected virtual void Qcp3035ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x00064384 File Offset: 0x00063384
		private bool Qcp3035Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "QC07"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "QCP3035";
			browserCaps.AddBrowser("Qcp3035");
			this.Qcp3035ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Qcp3035ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x00064403 File Offset: 0x00063403
		protected virtual void D512ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x00064405 File Offset: 0x00063405
		protected virtual void D512ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x00064408 File Offset: 0x00063408
		private bool D512Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG22"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "D-512";
			browserCaps.AddBrowser("D512");
			this.D512ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.D512ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x00064487 File Offset: 0x00063487
		protected virtual void Dm110ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x00064489 File Offset: 0x00063489
		protected virtual void Dm110ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0006448C File Offset: 0x0006348C
		private bool Dm110Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG05"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "DM-110";
			browserCaps.AddBrowser("Dm110");
			this.Dm110ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Dm110ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x0006450B File Offset: 0x0006350B
		protected virtual void Tm510ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x0006450D File Offset: 0x0006350D
		protected virtual void Tm510ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x00064510 File Offset: 0x00063510
		private bool Tm510Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG21"))
			{
				return false;
			}
			capabilities["canRenderPostBackCards"] = "false";
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "TM-510";
			browserCaps.AddBrowser("Tm510");
			this.Tm510ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tm510ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0006459F File Offset: 0x0006359F
		protected virtual void Lg13ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x000645A1 File Offset: 0x000635A1
		protected virtual void Lg13ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x000645A4 File Offset: 0x000635A4
		private bool Lg13Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG13"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "DM-510";
			browserCaps.AddBrowser("Lg13");
			this.Lg13ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Lg13ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00064623 File Offset: 0x00063623
		protected virtual void P100ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x00064625 File Offset: 0x00063625
		protected virtual void P100ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00064628 File Offset: 0x00063628
		private bool P100Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG11"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "P-100";
			browserCaps.AddBrowser("P100");
			this.P100ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.P100ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x000646A7 File Offset: 0x000636A7
		protected virtual void Lgc875fProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x000646A9 File Offset: 0x000636A9
		protected virtual void Lgc875fProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x000646AC File Offset: 0x000636AC
		private bool Lgc875fProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG07"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "LGC-875F";
			browserCaps.AddBrowser("Lgc875f");
			this.Lgc875fProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Lgc875fProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x0006472B File Offset: 0x0006372B
		protected virtual void Lgp680fProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0006472D File Offset: 0x0006372D
		protected virtual void Lgp680fProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x00064730 File Offset: 0x00063730
		private bool Lgp680fProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG03"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "LGP-6800F";
			browserCaps.AddBrowser("Lgp680f");
			this.Lgp680fProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Lgp680fProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x000647AF File Offset: 0x000637AF
		protected virtual void Lgp7800fProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x000647B1 File Offset: 0x000637B1
		protected virtual void Lgp7800fProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x000647B4 File Offset: 0x000637B4
		private bool Lgp7800fProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG04"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "LGP-7800F";
			browserCaps.AddBrowser("Lgp7800f");
			this.Lgp7800fProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Lgp7800fProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00064833 File Offset: 0x00063833
		protected virtual void Lgc840fProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00064835 File Offset: 0x00063835
		protected virtual void Lgc840fProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x00064838 File Offset: 0x00063838
		private bool Lgc840fProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG09"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "LGC-840F";
			browserCaps.AddBrowser("Lgc840f");
			this.Lgc840fProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Lgc840fProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x000648B7 File Offset: 0x000638B7
		protected virtual void Lgi2100ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x000648B9 File Offset: 0x000638B9
		protected virtual void Lgi2100ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x000648BC File Offset: 0x000638BC
		private bool Lgi2100Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG02"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "LGI-2100";
			browserCaps.AddBrowser("Lgi2100");
			this.Lgi2100ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Lgi2100ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x0006493B File Offset: 0x0006393B
		protected virtual void Lgp7300fProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x0006493D File Offset: 0x0006393D
		protected virtual void Lgp7300fProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x00064940 File Offset: 0x00063940
		private bool Lgp7300fProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "LGP-7300F";
			browserCaps.AddBrowser("Lgp7300f");
			this.Lgp7300fProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Lgp7300fProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x000649BF File Offset: 0x000639BF
		protected virtual void Sd500ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x000649C1 File Offset: 0x000639C1
		protected virtual void Sd500ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x000649C4 File Offset: 0x000639C4
		private bool Sd500Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG10"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "SD-500";
			browserCaps.AddBrowser("Sd500");
			this.Sd500ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sd500ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x00064A43 File Offset: 0x00063A43
		protected virtual void Tp1100ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00064A45 File Offset: 0x00063A45
		protected virtual void Tp1100ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x00064A48 File Offset: 0x00063A48
		private bool Tp1100Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG06"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "Touchpoint TP1100";
			browserCaps.AddBrowser("Tp1100");
			this.Tp1100ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tp1100ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00064AC7 File Offset: 0x00063AC7
		protected virtual void Tp3000ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x00064AC9 File Offset: 0x00063AC9
		protected virtual void Tp3000ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x00064ACC File Offset: 0x00063ACC
		private bool Tp3000Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG08"))
			{
				return false;
			}
			capabilities["canRenderAfterInputOrSelectElement"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "Touchpoint TP3000";
			browserCaps.AddBrowser("Tp3000");
			this.Tp3000ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tp3000ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x00064B6B File Offset: 0x00063B6B
		protected virtual void T250ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x00064B6D File Offset: 0x00063B6D
		protected virtual void T250ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x00064B70 File Offset: 0x00063B70
		private bool T250Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "T250"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Mitsubishi";
			capabilities["mobileDeviceModel"] = "T250";
			browserCaps.AddBrowser("T250");
			this.T250ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.T250ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00064BEF File Offset: 0x00063BEF
		protected virtual void Mo01ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00064BF1 File Offset: 0x00063BF1
		protected virtual void Mo01ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00064BF4 File Offset: 0x00063BF4
		private bool Mo01Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MO01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "i500+, i700+, i1000+";
			browserCaps.AddBrowser("Mo01");
			this.Mo01ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mo01ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00064C73 File Offset: 0x00063C73
		protected virtual void Mo02ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00064C75 File Offset: 0x00063C75
		protected virtual void Mo02ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x00064C78 File Offset: 0x00063C78
		private bool Mo02Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MO02"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "i2000+";
			browserCaps.AddBrowser("Mo02");
			this.Mo02ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mo02ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00064CF7 File Offset: 0x00063CF7
		protected virtual void Mc01ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x00064CF9 File Offset: 0x00063CF9
		protected virtual void Mc01ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00064CFC File Offset: 0x00063CFC
		private bool Mc01Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MC01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "StarTac ST786x, Talkabout T816x, Timeport P816x";
			browserCaps.AddBrowser("Mc01");
			this.Mc01ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mc01ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x00064D7B File Offset: 0x00063D7B
		protected virtual void McccProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x00064D7D File Offset: 0x00063D7D
		protected virtual void McccProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00064D80 File Offset: 0x00063D80
		private bool McccProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MCCC"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Talkabout V2267";
			browserCaps.AddBrowser("Mccc");
			this.McccProcessGateways(headers, browserCaps);
			bool flag = false;
			this.McccProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x00064DFF File Offset: 0x00063DFF
		protected virtual void Mcc9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x00064E01 File Offset: 0x00063E01
		protected virtual void Mcc9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x00064E04 File Offset: 0x00063E04
		private bool Mcc9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MCC9"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Talkabout V8162";
			browserCaps.AddBrowser("Mcc9");
			this.Mcc9ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mcc9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x00064E83 File Offset: 0x00063E83
		protected virtual void Nk00ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x00064E85 File Offset: 0x00063E85
		protected virtual void Nk00ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x00064E88 File Offset: 0x00063E88
		private bool Nk00Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "NK00"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2252";
			capabilities["maximumSoftkeyLabelLength"] = "6";
			capabilities["mobileDeviceManufacturer"] = "Nokia";
			capabilities["mobileDeviceModel"] = "nokia 3285";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["screenCharactersWidth"] = "15";
			capabilities["supportsBold"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("Nk00");
			this.Nk00ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nk00ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00064F77 File Offset: 0x00063F77
		protected virtual void Mai12ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00064F79 File Offset: 0x00063F79
		protected virtual void Mai12ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x00064F7C File Offset: 0x00063F7C
		private bool Mai12Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MAI[12]"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["mobileDeviceModel"] = "704G";
			browserCaps.AddBrowser("Mai12");
			this.Mai12ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mai12ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x00064FFB File Offset: 0x00063FFB
		protected virtual void Ma112ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00064FFD File Offset: 0x00063FFD
		protected virtual void Ma112ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x00065000 File Offset: 0x00064000
		private bool Ma112Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MA1[12]"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["mobileDeviceModel"] = "C308P";
			browserCaps.AddBrowser("Ma112");
			this.Ma112ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ma112ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x0006507F File Offset: 0x0006407F
		protected virtual void Ma13ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x00065081 File Offset: 0x00064081
		protected virtual void Ma13ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x00065084 File Offset: 0x00064084
		private bool Ma13Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MA13"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["mobileDeviceModel"] = "C408P";
			browserCaps.AddBrowser("Ma13");
			this.Ma13ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ma13ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00065103 File Offset: 0x00064103
		protected virtual void Mac1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00065105 File Offset: 0x00064105
		protected virtual void Mac1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00065108 File Offset: 0x00064108
		private bool Mac1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MAC1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["mobileDeviceModel"] = "D305P";
			browserCaps.AddBrowser("Mac1");
			this.Mac1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mac1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00065187 File Offset: 0x00064187
		protected virtual void Mat1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00065189 File Offset: 0x00064189
		protected virtual void Mat1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x0006518C File Offset: 0x0006418C
		private bool Mat1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MAT1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["mobileDeviceModel"] = "TP01";
			browserCaps.AddBrowser("Mat1");
			this.Mat1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mat1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x0006520B File Offset: 0x0006420B
		protected virtual void Sc01ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x0006520D File Offset: 0x0006420D
		protected virtual void Sc01ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x00065210 File Offset: 0x00064210
		private bool Sc01Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SC01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "SCH-3500";
			browserCaps.AddBrowser("Sc01");
			this.Sc01ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sc01ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x0006528F File Offset: 0x0006428F
		protected virtual void Sc03ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x00065291 File Offset: 0x00064291
		protected virtual void Sc03ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x00065294 File Offset: 0x00064294
		private bool Sc03Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SC03"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "SCH-6100";
			browserCaps.AddBrowser("Sc03");
			this.Sc03ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sc03ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00065313 File Offset: 0x00064313
		protected virtual void Sc02ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00065315 File Offset: 0x00064315
		protected virtual void Sc02ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00065318 File Offset: 0x00064318
		private bool Sc02Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SC02"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "SCH-8500";
			browserCaps.AddBrowser("Sc02");
			this.Sc02ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sc02ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00065397 File Offset: 0x00064397
		protected virtual void Sc04ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00065399 File Offset: 0x00064399
		protected virtual void Sc04ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0006539C File Offset: 0x0006439C
		private bool Sc04Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SC04"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "SCH-850";
			browserCaps.AddBrowser("Sc04");
			this.Sc04ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sc04ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0006541B File Offset: 0x0006441B
		protected virtual void Sg08ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0006541D File Offset: 0x0006441D
		protected virtual void Sg08ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00065420 File Offset: 0x00064420
		private bool Sg08Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SG08"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "SGH-800";
			browserCaps.AddBrowser("Sg08");
			this.Sg08ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sg08ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x0006549F File Offset: 0x0006449F
		protected virtual void Sc13ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x000654A1 File Offset: 0x000644A1
		protected virtual void Sc13ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x000654A4 File Offset: 0x000644A4
		private bool Sc13Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SC13"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "Uproar M100";
			browserCaps.AddBrowser("Sc13");
			this.Sc13ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sc13ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00065523 File Offset: 0x00064523
		protected virtual void Sc11ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00065525 File Offset: 0x00064525
		protected virtual void Sc11ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00065528 File Offset: 0x00064528
		private bool Sc11Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SC11"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "SCH-N105";
			browserCaps.AddBrowser("Sc11");
			this.Sc11ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sc11ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x000655A7 File Offset: 0x000645A7
		protected virtual void Sec01ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x000655A9 File Offset: 0x000645A9
		protected virtual void Sec01ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x000655AC File Offset: 0x000645AC
		private bool Sec01Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SEC01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "SCH-U03";
			browserCaps.AddBrowser("Sec01");
			this.Sec01ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sec01ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0006562B File Offset: 0x0006462B
		protected virtual void Sc10ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x0006562D File Offset: 0x0006462D
		protected virtual void Sc10ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00065630 File Offset: 0x00064630
		private bool Sc10Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SC10"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "SCH-U02";
			browserCaps.AddBrowser("Sc10");
			this.Sc10ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sc10ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x000656AF File Offset: 0x000646AF
		protected virtual void Sy12ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x000656B1 File Offset: 0x000646B1
		protected virtual void Sy12ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x000656B4 File Offset: 0x000646B4
		private bool Sy12Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SY12"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "C401SA";
			browserCaps.AddBrowser("Sy12");
			this.Sy12ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sy12ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x00065743 File Offset: 0x00064743
		protected virtual void St11ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x00065745 File Offset: 0x00064745
		protected virtual void St11ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00065748 File Offset: 0x00064748
		private bool St11Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "ST11"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "C403ST";
			browserCaps.AddBrowser("St11");
			this.St11ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.St11ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x000657C7 File Offset: 0x000647C7
		protected virtual void Sy13ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x000657C9 File Offset: 0x000647C9
		protected virtual void Sy13ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x000657CC File Offset: 0x000647CC
		private bool Sy13Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SY13"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "C405SA";
			browserCaps.AddBrowser("Sy13");
			this.Sy13ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sy13ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x0006584B File Offset: 0x0006484B
		protected virtual void Syc1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0006584D File Offset: 0x0006484D
		protected virtual void Syc1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x00065850 File Offset: 0x00064850
		private bool Syc1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SYC1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "D301SA";
			browserCaps.AddBrowser("Syc1");
			this.Syc1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Syc1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x000658CF File Offset: 0x000648CF
		protected virtual void Sy01ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x000658D1 File Offset: 0x000648D1
		protected virtual void Sy01ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x000658D4 File Offset: 0x000648D4
		private bool Sy01Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SY01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "SCP-4000";
			browserCaps.AddBrowser("Sy01");
			this.Sy01ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sy01ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x00065953 File Offset: 0x00064953
		protected virtual void Syt1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00065955 File Offset: 0x00064955
		protected virtual void Syt1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00065958 File Offset: 0x00064958
		private bool Syt1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SYT1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "TS01";
			browserCaps.AddBrowser("Syt1");
			this.Syt1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Syt1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x000659D7 File Offset: 0x000649D7
		protected virtual void Sty2ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x000659D9 File Offset: 0x000649D9
		protected virtual void Sty2ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x000659DC File Offset: 0x000649DC
		private bool Sty2Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SYT2"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "TS02";
			browserCaps.AddBrowser("Sty2");
			this.Sty2ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sty2ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x00065A5B File Offset: 0x00064A5B
		protected virtual void Sy02ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00065A5D File Offset: 0x00064A5D
		protected virtual void Sy02ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00065A60 File Offset: 0x00064A60
		private bool Sy02Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SY02"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "SCP-4500";
			browserCaps.AddBrowser("Sy02");
			this.Sy02ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sy02ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00065ADF File Offset: 0x00064ADF
		protected virtual void Sy03ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00065AE1 File Offset: 0x00064AE1
		protected virtual void Sy03ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00065AE4 File Offset: 0x00064AE4
		private bool Sy03Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SY03"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "SCP-5000";
			browserCaps.AddBrowser("Sy03");
			this.Sy03ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sy03ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00065B63 File Offset: 0x00064B63
		protected virtual void Si01ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x00065B65 File Offset: 0x00064B65
		protected virtual void Si01ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00065B68 File Offset: 0x00064B68
		private bool Si01Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SI01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Siemens";
			capabilities["mobileDeviceModel"] = "S25";
			browserCaps.AddBrowser("Si01");
			this.Si01ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Si01ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00065BE7 File Offset: 0x00064BE7
		protected virtual void Sni1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x00065BE9 File Offset: 0x00064BE9
		protected virtual void Sni1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00065BEC File Offset: 0x00064BEC
		private bool Sni1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SNI1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["mobileDeviceModel"] = "705G";
			browserCaps.AddBrowser("Sni1");
			this.Sni1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sni1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00065C6B File Offset: 0x00064C6B
		protected virtual void Sn11ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00065C6D File Offset: 0x00064C6D
		protected virtual void Sn11ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00065C70 File Offset: 0x00064C70
		private bool Sn11Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SN11"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["mobileDeviceModel"] = "C305SN";
			browserCaps.AddBrowser("Sn11");
			this.Sn11ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sn11ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00065CEF File Offset: 0x00064CEF
		protected virtual void Sn12ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00065CF1 File Offset: 0x00064CF1
		protected virtual void Sn12ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00065CF4 File Offset: 0x00064CF4
		private bool Sn12Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SN12"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["mobileDeviceModel"] = "C404S";
			browserCaps.AddBrowser("Sn12");
			this.Sn12ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sn12ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00065D73 File Offset: 0x00064D73
		protected virtual void Sn134ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00065D75 File Offset: 0x00064D75
		protected virtual void Sn134ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00065D78 File Offset: 0x00064D78
		private bool Sn134Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SN1[34]"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["mobileDeviceModel"] = "C406S";
			browserCaps.AddBrowser("Sn134");
			this.Sn134ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sn134ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00065DF7 File Offset: 0x00064DF7
		protected virtual void Sn156ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00065DF9 File Offset: 0x00064DF9
		protected virtual void Sn156ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00065DFC File Offset: 0x00064DFC
		private bool Sn156Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SN1[56]"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["mobileDeviceModel"] = "C413S";
			browserCaps.AddBrowser("Sn156");
			this.Sn156ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sn156ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00065E7B File Offset: 0x00064E7B
		protected virtual void Snc1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00065E7D File Offset: 0x00064E7D
		protected virtual void Snc1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00065E80 File Offset: 0x00064E80
		private bool Snc1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SNC1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["mobileDeviceModel"] = "D306S";
			browserCaps.AddBrowser("Snc1");
			this.Snc1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Snc1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00065EFF File Offset: 0x00064EFF
		protected virtual void Tsc1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00065F01 File Offset: 0x00064F01
		protected virtual void Tsc1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00065F04 File Offset: 0x00064F04
		private bool Tsc1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "TSC1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Toshiba";
			capabilities["mobileDeviceModel"] = "D302T";
			browserCaps.AddBrowser("Tsc1");
			this.Tsc1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tsc1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00065F83 File Offset: 0x00064F83
		protected virtual void Tsi1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x00065F85 File Offset: 0x00064F85
		protected virtual void Tsi1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x00065F88 File Offset: 0x00064F88
		private bool Tsi1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "TSI1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Toshiba";
			capabilities["mobileDeviceModel"] = "701G";
			browserCaps.AddBrowser("Tsi1");
			this.Tsi1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tsi1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x00066007 File Offset: 0x00065007
		protected virtual void Ts11ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00066009 File Offset: 0x00065009
		protected virtual void Ts11ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x0006600C File Offset: 0x0006500C
		private bool Ts11Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "TS11"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Toshiba";
			capabilities["mobileDeviceModel"] = "C301T";
			browserCaps.AddBrowser("Ts11");
			this.Ts11ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ts11ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x0006608B File Offset: 0x0006508B
		protected virtual void Ts12ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x0006608D File Offset: 0x0006508D
		protected virtual void Ts12ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00066090 File Offset: 0x00065090
		private bool Ts12Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "TS12"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Toshiba";
			capabilities["mobileDeviceModel"] = "C310T";
			browserCaps.AddBrowser("Ts12");
			this.Ts12ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ts12ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x0006610F File Offset: 0x0006510F
		protected virtual void Ts13ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00066111 File Offset: 0x00065111
		protected virtual void Ts13ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00066114 File Offset: 0x00065114
		private bool Ts13Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "TS13"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Toshiba";
			capabilities["mobileDeviceModel"] = "C410T";
			browserCaps.AddBrowser("Ts13");
			this.Ts13ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ts13ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00066193 File Offset: 0x00065193
		protected virtual void Tst1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00066195 File Offset: 0x00065195
		protected virtual void Tst1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00066198 File Offset: 0x00065198
		private bool Tst1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "TST1"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Toshiba";
			capabilities["mobileDeviceModel"] = "TT01";
			browserCaps.AddBrowser("Tst1");
			this.Tst1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tst1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00066217 File Offset: 0x00065217
		protected virtual void Tst2ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00066219 File Offset: 0x00065219
		protected virtual void Tst2ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x0006621C File Offset: 0x0006521C
		private bool Tst2Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "TST2"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Toshiba";
			capabilities["mobileDeviceModel"] = "TT02";
			browserCaps.AddBrowser("Tst2");
			this.Tst2ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tst2ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x0006629B File Offset: 0x0006529B
		protected virtual void Tst3ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x0006629D File Offset: 0x0006529D
		protected virtual void Tst3ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x000662A0 File Offset: 0x000652A0
		private bool Tst3Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "TST3"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Toshiba";
			capabilities["mobileDeviceModel"] = "TT03";
			browserCaps.AddBrowser("Tst3");
			this.Tst3ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tst3ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x0006631F File Offset: 0x0006531F
		protected virtual void Ig01ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00066321 File Offset: 0x00065321
		protected virtual void Ig01ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00066324 File Offset: 0x00065324
		private bool Ig01Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "IG01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "NeoPoint";
			capabilities["mobileDeviceModel"] = "NP1000";
			browserCaps.AddBrowser("Ig01");
			this.Ig01ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ig01ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x000663A3 File Offset: 0x000653A3
		protected virtual void Ig02ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x000663A5 File Offset: 0x000653A5
		protected virtual void Ig02ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x000663A8 File Offset: 0x000653A8
		private bool Ig02Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "IG02"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "NeoPoint";
			capabilities["mobileDeviceModel"] = "NP1660";
			browserCaps.AddBrowser("Ig02");
			this.Ig02ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ig02ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00066427 File Offset: 0x00065427
		protected virtual void Ig03ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x00066429 File Offset: 0x00065429
		protected virtual void Ig03ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x0006642C File Offset: 0x0006542C
		private bool Ig03Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "IG03"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "NeoPoint";
			capabilities["mobileDeviceModel"] = "NP2000";
			browserCaps.AddBrowser("Ig03");
			this.Ig03ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ig03ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x000664AB File Offset: 0x000654AB
		protected virtual void Qc31ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x000664AD File Offset: 0x000654AD
		protected virtual void Qc31ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x000664B0 File Offset: 0x000654B0
		private bool Qc31Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "QC31"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Qualcomm";
			capabilities["mobileDeviceModel"] = "QCP-860, QCP-1960";
			browserCaps.AddBrowser("Qc31");
			this.Qc31ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Qc31ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x0006652F File Offset: 0x0006552F
		protected virtual void Qc12ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x00066531 File Offset: 0x00065531
		protected virtual void Qc12ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00066534 File Offset: 0x00065534
		private bool Qc12Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "QC12"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Qualcomm";
			capabilities["mobileDeviceModel"] = "QCP-1900, QCP-2700";
			browserCaps.AddBrowser("Qc12");
			this.Qc12ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Qc12ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x000665B3 File Offset: 0x000655B3
		protected virtual void Qc32ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x000665B5 File Offset: 0x000655B5
		protected virtual void Qc32ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x000665B8 File Offset: 0x000655B8
		private bool Qc32Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "QC32"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Qualcomm";
			capabilities["mobileDeviceModel"] = "QCP-2760";
			browserCaps.AddBrowser("Qc32");
			this.Qc32ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Qc32ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00066637 File Offset: 0x00065637
		protected virtual void Sp01ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00066639 File Offset: 0x00065639
		protected virtual void Sp01ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x0006663C File Offset: 0x0006563C
		private bool Sp01Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SP01"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Mitsubishi";
			capabilities["mobileDeviceModel"] = "MA120";
			browserCaps.AddBrowser("Sp01");
			this.Sp01ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sp01ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x000666BB File Offset: 0x000656BB
		protected virtual void ShProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x000666BD File Offset: 0x000656BD
		protected virtual void ShProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x000666C0 File Offset: 0x000656C0
		private bool ShProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SH"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "Duette";
			browserCaps.AddBrowser("Sh");
			this.ShProcessGateways(headers, browserCaps);
			bool flag = false;
			this.ShProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0006673F File Offset: 0x0006573F
		protected virtual void Upg1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00066741 File Offset: 0x00065741
		protected virtual void Upg1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x00066744 File Offset: 0x00065744
		private bool Upg1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "UPG1"))
			{
				return false;
			}
			capabilities["canSendMail"] = "false";
			capabilities["mobileDeviceManufacturer"] = "OpenWave";
			capabilities["mobileDeviceModel"] = "Generic Simulator";
			browserCaps.AddBrowser("Upg1");
			this.Upg1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Upg1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x000667D3 File Offset: 0x000657D3
		protected virtual void Opwv1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x000667D5 File Offset: 0x000657D5
		protected virtual void Opwv1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x000667D8 File Offset: 0x000657D8
		private bool Opwv1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "OPWV1"))
			{
				return false;
			}
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["inputType"] = "keyboard";
			capabilities["maximumRenderedPageSize"] = "3584";
			capabilities["maximumSoftkeyLabelLength"] = "9";
			capabilities["mobileDeviceManufacturer"] = "Openwave";
			capabilities["mobileDeviceModel"] = "Openwave 5.0 emulator";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "19";
			capabilities["screenPixelsHeight"] = "188";
			capabilities["screenPixelsWidth"] = "144";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			browserCaps.AddBrowser("Opwv1");
			this.Opwv1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Opwv1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00066917 File Offset: 0x00065917
		protected virtual void AlavProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x00066919 File Offset: 0x00065919
		protected virtual void AlavProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x0006691C File Offset: 0x0006591C
		private bool AlavProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "ALAV"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Alcatel";
			capabilities["mobileDeviceModel"] = "OneTouch";
			browserCaps.AddBrowser("Alav");
			this.AlavProcessGateways(headers, browserCaps);
			bool flag = false;
			this.AlavProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x0006699B File Offset: 0x0006599B
		protected virtual void Im1kProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x0006699D File Offset: 0x0006599D
		protected virtual void Im1kProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x000669A0 File Offset: 0x000659A0
		private bool Im1kProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "IM1K"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "iDEN";
			browserCaps.AddBrowser("Im1k");
			this.Im1kProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Im1kProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x00066A1F File Offset: 0x00065A1F
		protected virtual void Nt95ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00066A21 File Offset: 0x00065A21
		protected virtual void Nt95ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00066A24 File Offset: 0x00065A24
		private bool Nt95Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "NT95"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["mobileDeviceModel"] = "cdmaOne";
			browserCaps.AddBrowser("Nt95");
			this.Nt95ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Nt95ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x00066AA3 File Offset: 0x00065AA3
		protected virtual void Mot2001ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00066AA5 File Offset: 0x00065AA5
		protected virtual void Mot2001ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x00066AA8 File Offset: 0x00065AA8
		private bool Mot2001Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-2001"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "1946";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Timeport 270c";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["requiresSpecialViewStateEncoding"] = "true";
			capabilities["requiresUrlEncodedPostfieldValues"] = "true";
			capabilities["screenCharactersWidth"] = "19";
			browserCaps.AddBrowser("Mot2001");
			this.Mot2001ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mot2001ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00066B77 File Offset: 0x00065B77
		protected virtual void Motv200ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00066B79 File Offset: 0x00065B79
		protected virtual void Motv200ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x00066B7C File Offset: 0x00065B7C
		private bool Motv200Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-v200"))
			{
				return false;
			}
			capabilities["hasBackButton"] = "false";
			capabilities["inputType"] = "keyboard";
			capabilities["maximumRenderedPageSize"] = "2000";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Motorola v200";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["requiresSpecialViewStateEncoding"] = "true";
			capabilities["requiresUrlEncodedPostfieldValues"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("Motv200");
			this.Motv200ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motv200ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x00066C7B File Offset: 0x00065C7B
		protected virtual void Mot72ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x00066C7D File Offset: 0x00065C7D
		protected virtual void Mot72ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x00066C80 File Offset: 0x00065C80
		private bool Mot72Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-72"))
			{
				return false;
			}
			capabilities["hasBackButton"] = "false";
			capabilities["maximumRenderedPageSize"] = "2900";
			capabilities["maximumSoftkeyLabelLength"] = "7";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Motorola i80s";
			capabilities["numberOfSoftkeys"] = "4";
			capabilities["rendersBreaksAfterWmlAnchor"] = "true";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["requiresSpecialViewStateEncoding"] = "true";
			capabilities["requiresUrlEncodedPostfieldValues"] = "true";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "13";
			browserCaps.AddBrowser("Mot72");
			this.Mot72ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mot72ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x00066D9F File Offset: 0x00065D9F
		protected virtual void Mot76ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x00066DA1 File Offset: 0x00065DA1
		protected virtual void Mot76ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x00066DA4 File Offset: 0x00065DA4
		private bool Mot76Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-76"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2969";
			capabilities["maximumSoftkeyLabelLength"] = "7";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Motorola i90c";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["screenCharactersWidth"] = "14";
			browserCaps.AddBrowser("Mot76");
			this.Mot76ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mot76ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x00066E83 File Offset: 0x00065E83
		protected virtual void Scp6000ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x00066E85 File Offset: 0x00065E85
		protected virtual void Scp6000ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x00066E88 File Offset: 0x00065E88
		private bool Scp6000Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Sanyo-SCP6000"))
			{
				return false;
			}
			capabilities["canRenderInputAndSelectElementsTogether"] = "false";
			capabilities["hasBackButton"] = "false";
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "Sanyo SCP-6000";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenPixelsHeight"] = "120";
			capabilities["screenPixelsWidth"] = "128";
			capabilities["supportsBold"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("Scp6000");
			this.Scp6000ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Scp6000ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x00066F97 File Offset: 0x00065F97
		protected virtual void Motd5ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x00066F99 File Offset: 0x00065F99
		protected virtual void Motd5ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x00066F9C File Offset: 0x00065F9C
		private bool Motd5Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-D5"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2000";
			capabilities["maximumSoftkeyLabelLength"] = "6";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Motorola Talkabout 191/192/193";
			capabilities["numberOfSoftkeys"] = "3";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "13";
			capabilities["screenPixelsHeight"] = "51";
			capabilities["screenPixelsWidth"] = "91";
			browserCaps.AddBrowser("MotD5");
			this.Motd5ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motd5ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x0006708B File Offset: 0x0006608B
		protected virtual void Motf0ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x0006708D File Offset: 0x0006608D
		protected virtual void Motf0ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x00067090 File Offset: 0x00066090
		private bool Motf0Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-F0"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2000";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "Motorola v50";
			capabilities["numberOfSoftkeys"] = "3";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["requiresSpecialViewStateEncoding"] = "true";
			capabilities["requiresUrlEncodedPostfieldValues"] = "true";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "40";
			capabilities["screenPixelsWidth"] = "96";
			browserCaps.AddBrowser("MotF0");
			this.Motf0ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motf0ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x0006719F File Offset: 0x0006619F
		protected virtual void Sgha400ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x000671A1 File Offset: 0x000661A1
		protected virtual void Sgha400ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x000671A4 File Offset: 0x000661A4
		private bool Sgha400Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SAMSUNG-SGH-A400"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2000";
			capabilities["maximumSoftkeyLabelLength"] = "6";
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "Samsung SGH-A400";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["requiresNoSoftkeyLabels"] = "true";
			capabilities["screenCharactersHeight"] = "3";
			capabilities["screenCharactersWidth"] = "13";
			capabilities["screenPixelsHeight"] = "96";
			capabilities["screenPixelsWidth"] = "128";
			browserCaps.AddBrowser("SghA400");
			this.Sgha400ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sgha400ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x000672A3 File Offset: 0x000662A3
		protected virtual void Sec03ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x000672A5 File Offset: 0x000662A5
		protected virtual void Sec03ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x000672A8 File Offset: 0x000662A8
		private bool Sec03Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SEC03"))
			{
				return false;
			}
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "false";
			capabilities["maximumRenderedPageSize"] = "3000";
			capabilities["maximumSoftkeyLabelLength"] = "7";
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "Samsung SPH-i300";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "38";
			capabilities["screenPixelsHeight"] = "240";
			capabilities["screenPixelsWidth"] = "160";
			capabilities["supportsBold"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("Sec03");
			this.Sec03ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sec03ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000673F7 File Offset: 0x000663F7
		protected virtual void Siec3iProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x000673F9 File Offset: 0x000663F9
		protected virtual void Siec3iProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x000673FC File Offset: 0x000663FC
		private bool Siec3iProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SIE-C3I"))
			{
				return false;
			}
			capabilities["canRenderMixedSelects"] = "false";
			capabilities["maximumRenderedPageSize"] = "1900";
			capabilities["maximumSoftkeyLabelLength"] = "7";
			capabilities["mobileDeviceManufacturer"] = "Siemens";
			capabilities["mobileDeviceModel"] = "C35/M35";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "true";
			capabilities["rendersWmlDoAcceptsInline"] = "false";
			capabilities["requiresSpecialViewStateEncoding"] = "false";
			capabilities["requiresUrlEncodedPostfieldValues"] = "false";
			capabilities["screenCharactersHeight"] = "3";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "54";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsBold"] = "true";
			browserCaps.AddBrowser("SieC3i");
			this.Siec3iProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Siec3iProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x0006754B File Offset: 0x0006654B
		protected virtual void Sn17ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x0006754D File Offset: 0x0006654D
		protected virtual void Sn17ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00067550 File Offset: 0x00066550
		private bool Sn17Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SN17"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["maximumRenderedPageSize"] = "12000";
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["mobileDeviceModel"] = "C1002S";
			capabilities["numberOfSoftkeys"] = "3";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["requiresSpecialViewStateEncoding"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "120";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("Sn17");
			this.Sn17ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sn17ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x0006767F File Offset: 0x0006667F
		protected virtual void Scp4700ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x00067681 File Offset: 0x00066681
		protected virtual void Scp4700ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x00067684 File Offset: 0x00066684
		private bool Scp4700Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Sanyo-SCP4700"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "3072";
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "Sanyo SCP 4700";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "15";
			capabilities["screenPixelsHeight"] = "32";
			capabilities["screenPixelsWidth"] = "91";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("Scp4700");
			this.Scp4700ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Scp4700ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00067773 File Offset: 0x00066773
		protected virtual void Sec02ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x00067775 File Offset: 0x00066775
		protected virtual void Sec02ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x00067778 File Offset: 0x00066778
		private bool Sec02Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SEC02"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2867";
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			capabilities["mobileDeviceModel"] = "Samsung SPH-N200";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["rendersBreaksAfterWmlAnchor"] = "true";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "15";
			capabilities["screenPixelsHeight"] = "96";
			capabilities["screenPixelsWidth"] = "128";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("Sec02");
			this.Sec02ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sec02ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x000678A7 File Offset: 0x000668A7
		protected virtual void Sy15ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x000678A9 File Offset: 0x000668A9
		protected virtual void Sy15ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x000678AC File Offset: 0x000668AC
		private bool Sy15Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SY15"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["maximumRenderedPageSize"] = "7500";
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "Sanyo C1001SA";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["requiresSpecialViewStateEncoding"] = "true";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("Sy15");
			this.Sy15ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sy15ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x000679AB File Offset: 0x000669AB
		protected virtual void Db520ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x000679AD File Offset: 0x000669AD
		protected virtual void Db520ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x000679B0 File Offset: 0x000669B0
		private bool Db520Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LGE-DB520"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "3072";
			capabilities["mobileDeviceManufacturer"] = "Sprint";
			capabilities["mobileDeviceModel"] = "TP5200";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["preferredRenderingMime"] = "text/wnd.wap.wml";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("Db520");
			this.Db520ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Db520ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00067A8F File Offset: 0x00066A8F
		protected virtual void L430v03j02ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00067A91 File Offset: 0x00066A91
		protected virtual void L430v03j02ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00067A94 File Offset: 0x00066A94
		private bool L430v03j02Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LGE-L430V03J02"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2900";
			capabilities["maximumSoftkeyLabelLength"] = "10";
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "LG-LP9000";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["requiresNoSoftkeyLabels"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "19";
			capabilities["screenPixelsHeight"] = "133";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsBold"] = "true";
			browserCaps.AddBrowser("L430V03J02");
			this.L430v03j02ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.L430v03j02ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00067BD3 File Offset: 0x00066BD3
		protected virtual void OpwvsdkProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00067BD5 File Offset: 0x00066BD5
		protected virtual void OpwvsdkProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00067BD8 File Offset: 0x00066BD8
		private bool OpwvsdkProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^OPWV-SDK"))
			{
				return false;
			}
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["mobileDeviceManufacturer"] = "Openwave";
			capabilities["mobileDeviceModel"] = "Mobile Browser";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "9";
			capabilities["screenCharactersWidth"] = "19";
			capabilities["screenPixelsHeight"] = "188";
			capabilities["screenPixelsWidth"] = "144";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("OPWVSDK");
			this.OpwvsdkProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Opwvsdk6Process(headers, browserCaps) && !this.Opwvsdk6plusProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.OpwvsdkProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00067D89 File Offset: 0x00066D89
		protected virtual void Opwvsdk6ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00067D8B File Offset: 0x00066D8B
		protected virtual void Opwvsdk6ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x00067D90 File Offset: 0x00066D90
		private bool Opwvsdk6Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^OPWV-SDK/61"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "UP.Browser";
			capabilities["canSendMail"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["mobileDeviceModel"] = "WAP Simulator 6.1";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "15";
			capabilities["screenPixelsHeight"] = "108";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			browserCaps.AddBrowser("OPWVSDK6");
			this.Opwvsdk6ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Opwvsdk6ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00067F2A File Offset: 0x00066F2A
		protected virtual void Opwvsdk6plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00067F2C File Offset: 0x00066F2C
		protected virtual void Opwvsdk6plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00067F30 File Offset: 0x00066F30
		private bool Opwvsdk6plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^OPWV-SDK/(?'modelVersion'[7-9]|6[^01])"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "Openwave";
			capabilities["canSendMail"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["maximumRenderedPageSize"] = "66000";
			capabilities["mobileDeviceModel"] = regexWorker["Mobile Browser ${version}"];
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresPragmaNoCacheHeader"] = "true";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "72";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsNoWrapStyle"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			browserCaps.AddBrowser("OPWVSDK6Plus");
			this.Opwvsdk6plusProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Opwvsdk6plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x00068110 File Offset: 0x00067110
		protected virtual void Kddica21ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x00068112 File Offset: 0x00067112
		protected virtual void Kddica21ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x00068114 File Offset: 0x00067114
		private bool Kddica21Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^KDDI-CA21$"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "9000";
			capabilities["mobileDeviceManufacturer"] = "Casio";
			capabilities["mobileDeviceModel"] = "A3012CA";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "147";
			capabilities["screenPixelsWidth"] = "125";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("KDDICA21");
			this.Kddica21ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Kddica21ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x00068263 File Offset: 0x00067263
		protected virtual void Kddits21ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x00068265 File Offset: 0x00067265
		protected virtual void Kddits21ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x00068268 File Offset: 0x00067268
		private bool Kddits21Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KDDI-TS21"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "9000";
			capabilities["mobileDeviceManufacturer"] = "Toshiba";
			capabilities["mobileDeviceModel"] = "C5001T";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "135";
			capabilities["screenPixelsWidth"] = "144";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("KDDITS21");
			this.Kddits21ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Kddits21ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x000683B7 File Offset: 0x000673B7
		protected virtual void Kddisa21ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x000683B9 File Offset: 0x000673B9
		protected virtual void Kddisa21ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x000683BC File Offset: 0x000673BC
		private bool Kddisa21Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KDDI-SA21"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "9000";
			capabilities["mobileDeviceManufacturer"] = "Sanyo";
			capabilities["mobileDeviceModel"] = "A3011SA";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "176";
			capabilities["screenPixelsWidth"] = "132";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsItalic"] = "false";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("KDDISA21");
			this.Kddisa21ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Kddisa21ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x0006851B File Offset: 0x0006751B
		protected virtual void Km100ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x0006851D File Offset: 0x0006751D
		protected virtual void Km100ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x00068520 File Offset: 0x00067520
		private bool Km100Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KM100"))
			{
				return false;
			}
			capabilities["cookies"] = "false";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "2900";
			capabilities["maximumSoftkeyLabelLength"] = "12";
			capabilities["mobileDeviceManufacturer"] = "OKWap";
			capabilities["mobileDeviceModel"] = "i108";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["rendersWmlSelectsAsMenuCards"] = "false";
			capabilities["requiresNoSoftkeyLabels"] = "true";
			capabilities["requiresPhoneNumbersAsPlainText"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("KM100");
			this.Km100ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Km100ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x0006869F File Offset: 0x0006769F
		protected virtual void Lgelx5350ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x000686A1 File Offset: 0x000676A1
		protected virtual void Lgelx5350ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x000686A4 File Offset: 0x000676A4
		private bool Lgelx5350Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LGE-LX5350"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["mobileDeviceManufacturer"] = "LG";
			capabilities["mobileDeviceModel"] = "LX5350";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "5";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "108";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("LGELX5350");
			this.Lgelx5350ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Lgelx5350ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x000688A3 File Offset: 0x000678A3
		protected virtual void Hitachip300ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x000688A5 File Offset: 0x000678A5
		protected virtual void Hitachip300ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x000688A8 File Offset: 0x000678A8
		private bool Hitachip300Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Hitachi-P300"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["mobileDeviceManufacturer"] = "Hitachi";
			capabilities["mobileDeviceModel"] = "SH-P300";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "17";
			capabilities["screenPixelsHeight"] = "130";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("HitachiP300");
			this.Hitachip300ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Hitachip300ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x00068AC7 File Offset: 0x00067AC7
		protected virtual void Sies46ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00068AC9 File Offset: 0x00067AC9
		protected virtual void Sies46ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00068ACC File Offset: 0x00067ACC
		private bool Sies46Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SIE-S46"))
			{
				return false;
			}
			capabilities["hasBackButton"] = "false";
			capabilities["maximumRenderedPageSize"] = "2700";
			capabilities["mobileDeviceManufacturer"] = "Siemens";
			capabilities["mobileDeviceModel"] = "S46";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "19";
			capabilities["screenPixelsHeight"] = "72";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsBold"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("SIES46");
			this.Sies46ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sies46ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x00068BFB File Offset: 0x00067BFB
		protected virtual void Motorolav60gProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00068BFD File Offset: 0x00067BFD
		protected virtual void Motorolav60gProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00068C00 File Offset: 0x00067C00
		private bool Motorolav60gProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-PHX4_"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "2000";
			capabilities["maximumSoftkeyLabelLength"] = "7";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "V60G";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["requiresSpecialViewStateEncoding"] = "true";
			capabilities["requiresUrlEncodedPostfieldValues"] = "true";
			capabilities["screenCharactersHeight"] = "3";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "36";
			capabilities["screenPixelsWidth"] = "90";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("MotorolaV60G");
			this.Motorolav60gProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motorolav60gProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00068D1F File Offset: 0x00067D1F
		protected virtual void Motorolav708ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00068D21 File Offset: 0x00067D21
		protected virtual void Motorolav708ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00068D24 File Offset: 0x00067D24
		private bool Motorolav708Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-V708_"))
			{
				return false;
			}
			capabilities["isColor"] = "false";
			capabilities["maximumRenderedPageSize"] = "1900";
			capabilities["maximumSoftkeyLabelLength"] = "7";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "V70";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "2";
			capabilities["screenCharactersWidth"] = "15";
			capabilities["screenPixelsHeight"] = "34";
			capabilities["screenPixelsWidth"] = "90";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("MotorolaV708");
			this.Motorolav708ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motorolav708ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00068E53 File Offset: 0x00067E53
		protected virtual void Motorolav708aProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x00068E55 File Offset: 0x00067E55
		protected virtual void Motorolav708aProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00068E58 File Offset: 0x00067E58
		private bool Motorolav708aProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MOT-V708A"))
			{
				return false;
			}
			capabilities["canRenderInputAndSelectElementsTogether"] = "false";
			capabilities["hasBackButton"] = "false";
			capabilities["maximumRenderedPageSize"] = "2000";
			capabilities["maximumSoftkeyLabelLength"] = "7";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "V70";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["screenCharactersHeight"] = "3";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "94";
			capabilities["screenPixelsWidth"] = "96";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("MotorolaV708A");
			this.Motorolav708aProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motorolav708aProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00068F77 File Offset: 0x00067F77
		protected virtual void Motorolae360ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x00068F79 File Offset: 0x00067F79
		protected virtual void Motorolae360ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x00068F7C File Offset: 0x00067F7C
		private bool Motorolae360Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Motorola-T33"))
			{
				return false;
			}
			capabilities["canSendMail"] = "true";
			capabilities["hasBackButton"] = "false";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "3500";
			capabilities["maximumSoftkeyLabelLength"] = "8";
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			capabilities["mobileDeviceModel"] = "E360";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["rendersWmlDoAcceptsInline"] = "true";
			capabilities["screenBitDepth"] = "12";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "27";
			capabilities["screenPixelsHeight"] = "96";
			capabilities["screenPixelsWidth"] = "128";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("MotorolaE360");
			this.Motorolae360ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Motorolae360ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x000690EB File Offset: 0x000680EB
		protected virtual void Sonyericssona1101sProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x000690ED File Offset: 0x000680ED
		protected virtual void Sonyericssona1101sProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x000690F0 File Offset: 0x000680F0
		private bool Sonyericssona1101sProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KDDI-SN22"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["maximumRenderedPageSize"] = "9000";
			capabilities["mobileDeviceManufacturer"] = "SonyEricsson";
			capabilities["mobileDeviceModel"] = "A1101S";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "9";
			capabilities["screenPixelsHeight"] = "123";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("SonyericssonA1101S");
			this.Sonyericssona1101sProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sonyericssona1101sProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x000692BF File Offset: 0x000682BF
		protected virtual void Philipsfisio820ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x000692C1 File Offset: 0x000682C1
		protected virtual void Philipsfisio820ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x000692C4 File Offset: 0x000682C4
		private bool Philipsfisio820Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "PHILIPS-FISIO 820"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "3000";
			capabilities["maximumSoftkeyLabelLength"] = "6";
			capabilities["mobileDeviceManufacturer"] = "Philips";
			capabilities["mobileDeviceModel"] = "Fisio 820";
			capabilities["numberOfSoftkeys"] = "3";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["requiresNoSoftkeyLabels"] = "true";
			capabilities["requiresSpecialViewStateEncoding"] = "true";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "20";
			capabilities["screenPixelsHeight"] = "112";
			capabilities["screenPixelsWidth"] = "112";
			capabilities["supportsBold"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			browserCaps.AddBrowser("PhilipsFisio820");
			this.Philipsfisio820ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Philipsfisio820ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x0006940E File Offset: 0x0006840E
		protected virtual void Casioa5302ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00069410 File Offset: 0x00068410
		protected virtual void Casioa5302ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x00069414 File Offset: 0x00068414
		private bool Casioa5302Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KDDI-CA22"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["maximumRenderedPageSize"] = "9000";
			capabilities["mobileDeviceManufacturer"] = "Casio";
			capabilities["mobileDeviceModel"] = "A5302CA";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenPixelsHeight"] = "147";
			capabilities["screenPixelsWidth"] = "132";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("CasioA5302");
			this.Casioa5302ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Casioa5302ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x000695D3 File Offset: 0x000685D3
		protected virtual void Tcll668ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x000695D5 File Offset: 0x000685D5
		protected virtual void Tcll668ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x000695D8 File Offset: 0x000685D8
		private bool Tcll668Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Compal-Seville"))
			{
				return false;
			}
			capabilities["browser"] = "Openwave";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "3800";
			capabilities["maximumSoftkeyLabelLength"] = "7";
			capabilities["mobileDeviceManufacturer"] = "TCL";
			capabilities["mobileDeviceModel"] = "L668";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "false";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "14";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "128";
			capabilities["supportsBold"] = "true";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["type"] = regexWorker["Openwave ${majorVersion}.x Browser"];
			browserCaps.AddBrowser("TCLL668");
			this.Tcll668ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Tcll668ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x0006976D File Offset: 0x0006876D
		protected virtual void Kddits24ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x0006976F File Offset: 0x0006876F
		protected virtual void Kddits24ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x00069774 File Offset: 0x00068774
		private bool Kddits24Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "KDDI-TS24"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "Openwave";
			capabilities["canSendMail"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["maximumRenderedPageSize"] = "9000";
			capabilities["mobileDeviceManufacturer"] = "Toshiba";
			capabilities["mobileDeviceModel"] = "A5304T";
			capabilities["PreferredImageMime"] = "image/jpeg";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenPixelsHeight"] = "176";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsNoWrapStyle"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsTitleElement"] = "false";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("KDDITS24");
			this.Kddits24ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Kddits24ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00069973 File Offset: 0x00068973
		protected virtual void Sies55ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x00069975 File Offset: 0x00068975
		protected virtual void Sies55ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x00069978 File Offset: 0x00068978
		private bool Sies55Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SIE-S55"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "Openwave";
			capabilities["canSendMail"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["isColor"] = "true";
			capabilities["maximumHrefLength"] = "980";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["mobileDeviceManufacturer"] = "Siemens";
			capabilities["mobileDeviceModel"] = "S55";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "4";
			capabilities["screenCharactersWidth"] = "19";
			capabilities["screenPixelsHeight"] = "80";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsFontSize"] = "false";
			capabilities["supportsNoWrapStyle"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("SIES55");
			this.Sies55ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sies55ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x00069BE7 File Offset: 0x00068BE7
		protected virtual void Sharpgx10ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00069BE9 File Offset: 0x00068BE9
		protected virtual void Sharpgx10ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x00069BEC File Offset: 0x00068BEC
		private bool Sharpgx10Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^SHARP\\-TQ\\-GX10"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "Openwave";
			capabilities["canSendMail"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["mobileDeviceManufacturer"] = "Sharp";
			capabilities["mobileDeviceModel"] = "GX10";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "9";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "120";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsNoWrapStyle"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsTitleElement"] = "false";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("SHARPGx10");
			this.Sharpgx10ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sharpgx10ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x00069E26 File Offset: 0x00068E26
		protected virtual void BenqathenaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x00069E28 File Offset: 0x00068E28
		protected virtual void BenqathenaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x00069E2C File Offset: 0x00068E2C
		private bool BenqathenaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^BENQ\\-Athena"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "Openwave";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["mobileDeviceManufacturer"] = "BenQ";
			capabilities["mobileDeviceModel"] = "S830c";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "8";
			capabilities["screenCharactersWidth"] = "18";
			capabilities["screenPixelsHeight"] = "0";
			capabilities["screenPixelsWidth"] = "0";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsNoWrapStyle"] = "true";
			capabilities["supportsRedirectWithCookie"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("BenQAthena");
			this.BenqathenaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.BenqathenaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0006A026 File Offset: 0x00069026
		protected virtual void OperaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x0006A028 File Offset: 0x00069028
		protected virtual void OperaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x0006A02C File Offset: 0x0006902C
		private bool OperaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Opera[ /](?'version'(?'major'\\d+)(?'minor'\\.\\d+)(?'letters'\\w*))"))
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], " (?'screenWidth'\\d*)x(?'screenHeight'\\d*)");
			capabilities["browser"] = "Opera";
			capabilities["cookies"] = "true";
			capabilities["css1"] = "true";
			capabilities["css2"] = "true";
			capabilities["defaultScreenCharactersHeight"] = "40";
			capabilities["defaultScreenCharactersWidth"] = "80";
			capabilities["defaultScreenPixelsHeight"] = "480";
			capabilities["defaultScreenPixelsWidth"] = "640";
			capabilities["ecmascriptversion"] = "1.1";
			capabilities["frames"] = "true";
			capabilities["javascript"] = "true";
			capabilities["letters"] = regexWorker["${letters}"];
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "false";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["screenBitDepth"] = "8";
			capabilities["screenPixelsHeight"] = regexWorker["${screenHeight}"];
			capabilities["screenPixelsWidth"] = regexWorker["${screenWidth}"];
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsDivNoWrap"] = "true";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["tables"] = "true";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["type"] = regexWorker["Opera${major}"];
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.Adapters["System.Web.UI.WebControls.CheckBox, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.HideDisabledControlAdapter";
			browserCaps.Adapters["System.Web.UI.WebControls.RadioButton, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.HideDisabledControlAdapter";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("Opera");
			this.OperaProcessGateways(headers, browserCaps);
			this.OperamobileProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Opera1to3betaProcess(headers, browserCaps) && !this.Opera4Process(headers, browserCaps) && !this.Opera5to9Process(headers, browserCaps) && !this.OperapsionProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.OperaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x0006A33A File Offset: 0x0006933A
		protected virtual void OperamobileProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x0006A33C File Offset: 0x0006933C
		protected virtual void OperamobileProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x0006A340 File Offset: 0x00069340
		private bool OperamobileProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Linux \\S+embedix"))
			{
				return false;
			}
			capabilities["isMobileDevice"] = "true";
			this.OperamobileProcessGateways(headers, browserCaps);
			bool flag = false;
			this.OperamobileProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x0006A39F File Offset: 0x0006939F
		protected virtual void Opera1to3betaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x0006A3A1 File Offset: 0x000693A1
		protected virtual void Opera1to3betaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x0006A3A4 File Offset: 0x000693A4
		private bool Opera1to3betaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["letters"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^b"))
			{
				return false;
			}
			text = (string)capabilities["majorversion"];
			if (!regexWorker.ProcessRegex(text, "[1-3]"))
			{
				return false;
			}
			capabilities["beta"] = "true";
			browserCaps.AddBrowser("Opera1to3beta");
			this.Opera1to3betaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Opera1to3betaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x0006A436 File Offset: 0x00069436
		protected virtual void Opera4ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x0006A438 File Offset: 0x00069438
		protected virtual void Opera4ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x0006A43C File Offset: 0x0006943C
		private bool Opera4Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "4"))
			{
				return false;
			}
			capabilities["ecmascriptversion"] = "1.3";
			capabilities["supportsFileUpload"] = "true";
			capabilities["xml"] = "true";
			browserCaps.AddBrowser("Opera4");
			this.Opera4ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Opera4betaProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Opera4ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x0006A4D8 File Offset: 0x000694D8
		protected virtual void Opera4betaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x0006A4DA File Offset: 0x000694DA
		protected virtual void Opera4betaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x0006A4DC File Offset: 0x000694DC
		private bool Opera4betaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["letters"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^b"))
			{
				return false;
			}
			capabilities["beta"] = "true";
			browserCaps.AddBrowser("Opera4beta");
			this.Opera4betaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Opera4betaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x0006A54B File Offset: 0x0006954B
		protected virtual void Opera5to9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x0006A54D File Offset: 0x0006954D
		protected virtual void Opera5to9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0006A550 File Offset: 0x00069550
		private bool Opera5to9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "[5-9]"))
			{
				return false;
			}
			capabilities["ecmascriptversion"] = "1.3";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "7000";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["screenBitDepth"] = "24";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsItalic"] = "false";
			capabilities["w3cdomversion"] = "1.0";
			capabilities["xml"] = "true";
			browserCaps.AddBrowser("Opera5to9");
			this.Opera5to9ProcessGateways(headers, browserCaps);
			this.Opera5to9betaProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Opera6to9Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Opera5to9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x0006A675 File Offset: 0x00069675
		protected virtual void Opera6to9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x0006A677 File Offset: 0x00069677
		protected virtual void Opera6to9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x0006A67C File Offset: 0x0006967C
		private bool Opera6to9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "[6-9]"))
			{
				return false;
			}
			capabilities["backgroundsounds"] = "true";
			capabilities["css1"] = "true";
			capabilities["css2"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["javaapplets"] = "true";
			capabilities["maximumRenderedPageSize"] = "2000000";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["screenBitDepth"] = "32";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["xml"] = "false";
			browserCaps.Adapters["System.Web.UI.WebControls.CheckBox, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "";
			browserCaps.Adapters["System.Web.UI.WebControls.RadioButton, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "";
			browserCaps.Adapters["System.Web.UI.WebControls.TextBox, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "";
			browserCaps.HtmlTextWriter = "";
			browserCaps.AddBrowser("Opera6to9");
			this.Opera6to9ProcessGateways(headers, browserCaps);
			this.OperamobilebrowserProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Opera7to9Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Opera6to9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x0006A810 File Offset: 0x00069810
		protected virtual void Opera7to9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x0006A812 File Offset: 0x00069812
		protected virtual void Opera7to9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x0006A814 File Offset: 0x00069814
		private bool Opera7to9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "[7-9]"))
			{
				return false;
			}
			capabilities["ecmascriptversion"] = "1.4";
			capabilities["supportsMaintainScrollPositionOnPostback"] = "true";
			browserCaps.AddBrowser("Opera7to9");
			this.Opera7to9ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Opera8to9Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Opera7to9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x0006A8A0 File Offset: 0x000698A0
		protected virtual void Opera8to9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x0006A8A2 File Offset: 0x000698A2
		protected virtual void Opera8to9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x0006A8A4 File Offset: 0x000698A4
		private bool Opera8to9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "[8-9]"))
			{
				return false;
			}
			capabilities["supportsCallback"] = "true";
			browserCaps.AddBrowser("Opera8to9");
			this.Opera8to9ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Opera8to9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x0006A913 File Offset: 0x00069913
		protected virtual void OperamobilebrowserProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x0006A915 File Offset: 0x00069915
		protected virtual void OperamobilebrowserProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x0006A918 File Offset: 0x00069918
		private bool OperamobilebrowserProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "; (?'screenWidth'\\d+)x(?'screenHeight'\\d+)\\)"))
			{
				return false;
			}
			capabilities["backgroundSounds"] = "false";
			capabilities["canSendMail"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["JavaApplets"] = "false";
			capabilities["maximumRenderedPageSize"] = "200000";
			capabilities["msDomVersion"] = "0.0";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["requiredMetaTagNameValue"] = "HandheldFriendly";
			capabilities["requiresPragmaNoCacheHeader"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "24";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsItalic"] = "true";
			capabilities["tagwriter"] = "System.Web.UI.Html32TextWriter";
			this.OperamobilebrowserProcessGateways(headers, browserCaps);
			bool flag = false;
			this.OperamobilebrowserProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x0006AA57 File Offset: 0x00069A57
		protected virtual void Opera5to9betaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x0006AA59 File Offset: 0x00069A59
		protected virtual void Opera5to9betaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x0006AA5C File Offset: 0x00069A5C
		private bool Opera5to9betaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["letters"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^b"))
			{
				return false;
			}
			capabilities["beta"] = "true";
			this.Opera5to9betaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Opera5to9betaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x0006AAC0 File Offset: 0x00069AC0
		protected virtual void OperapsionProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x0006AAC2 File Offset: 0x00069AC2
		protected virtual void OperapsionProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x0006AAC4 File Offset: 0x00069AC4
		private bool OperapsionProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Mozilla/.* \\(compatible; Opera .*; EPOC; (?'screenWidth'\\d*)x(?'screenHeight'\\d*)\\)"))
			{
				return false;
			}
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["canRenderEmptySelects"] = "false";
			capabilities["canSendMail"] = "false";
			capabilities["css1"] = "false";
			capabilities["css2"] = "false";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["maximumRenderedPageSize"] = "2560";
			capabilities["mobileDeviceManufacturer"] = "Psion";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "50";
			capabilities["screenPixelsHeight"] = regexWorker["${screenHeight}"];
			capabilities["screenPixelsWidth"] = regexWorker["${screenWidth}"];
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsSelectMultiple"] = "false";
			capabilities["tagwriter"] = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("OperaPsion");
			this.OperapsionProcessGateways(headers, browserCaps);
			bool flag = false;
			this.OperapsionProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x0006ACAA File Offset: 0x00069CAA
		protected virtual void MypalmProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x0006ACAC File Offset: 0x00069CAC
		protected virtual void MypalmProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x0006ACB0 File Offset: 0x00069CB0
		private bool MypalmProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Mozilla/2\\.0 \\(compatible; Elaine/(?'gatewayMajorVersion'\\w*)(?'gatewayMinorVersion'\\.\\w*)\\)"))
			{
				return false;
			}
			capabilities["browser"] = "MyPalm";
			capabilities["cookies"] = "false";
			capabilities["defaultScreenCharactersHeight"] = "6";
			capabilities["defaultScreenCharactersWidth"] = "12";
			capabilities["defaultScreenPixelsHeight"] = "72";
			capabilities["defaultScreenPixelsWidth"] = "96";
			capabilities["ecmascriptversion"] = "1.1";
			capabilities["frames"] = "true";
			capabilities["gatewayMajorVersion"] = regexWorker["${gatewayMajorVersion}"];
			capabilities["gatewayMinorVersion"] = regexWorker["${gatewayMinorVersion}"];
			capabilities["gatewayVersion"] = regexWorker["${gatewayMajorVersion}${gatewayMinorVersion}"];
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["javaapplets"] = "false";
			capabilities["javascript"] = "false";
			capabilities["mobileDeviceManufacturer"] = "PalmOS-licensee";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["requiredMetaTagNameValue"] = "PalmComputingPlatform";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["screenBitDepth"] = "2";
			capabilities["screenCharactersHeight"] = "12";
			capabilities["screenCharactersWidth"] = "36";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "160";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsEmptyStringInCookieValue"] = "false";
			capabilities["supportsFontColor"] = "false";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsImageSubmit"] = "false";
			capabilities["tables"] = "false";
			capabilities["type"] = "MyPalm";
			capabilities["vbscript"] = "false";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("Mypalm");
			this.MypalmProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Mypalm1Process(headers, browserCaps))
			{
				flag = false;
			}
			this.MypalmProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x0006AFB9 File Offset: 0x00069FB9
		protected virtual void Mypalm1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x0006AFBB File Offset: 0x00069FBB
		protected virtual void Mypalm1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x0006AFC0 File Offset: 0x00069FC0
		private bool Mypalm1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["gatewayMajorVersion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^1"))
			{
				return false;
			}
			text = (string)capabilities["gatewayMinorVersion"];
			if (!regexWorker.ProcessRegex(text, "\\.0$"))
			{
				return false;
			}
			capabilities["browser"] = "EarthLink";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "true";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "false";
			capabilities["maximumRenderedPageSize"] = "7000";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["rendersBreaksAfterHtmlLists"] = "false";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["requiresUniqueHtmlCheckboxNames"] = "true";
			capabilities["requiresUniqueHtmlInputNames"] = "true";
			capabilities["screenBitDepth"] = "4";
			capabilities["screenCharactersHeight"] = "13";
			capabilities["screenCharactersWidth"] = "30";
			capabilities["supportsFontSize"] = "false";
			capabilities["tables"] = "true";
			capabilities["type"] = "EarthLink";
			browserCaps.AddBrowser("MyPalm1");
			this.Mypalm1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mypalm1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x0006B142 File Offset: 0x0006A142
		protected virtual void PalmwebproProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x0006B144 File Offset: 0x0006A144
		protected virtual void PalmwebproProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x0006B148 File Offset: 0x0006A148
		private bool PalmwebproProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "PalmOS.*WebPro"))
			{
				return false;
			}
			capabilities["browser"] = "Palm WebPro";
			capabilities["canSendMail"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["javascript"] = "false";
			capabilities["maximumRenderedPageSize"] = "7000";
			capabilities["mobileDeviceManufacturer"] = "Palm";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiredMetaTagNameValue"] = "PalmComputingPlatform";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOutputOptimization"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "12";
			capabilities["screenCharactersWidth"] = "30";
			capabilities["screenPixelsHeight"] = "320";
			capabilities["screenPixelsWidth"] = "320";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivAlign"] = "false";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFileUpload"] = "false";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsFontSize"] = "false";
			capabilities["tables"] = "false";
			capabilities["type"] = "Palm WebPro";
			browserCaps.AddBrowser("PalmWebPro");
			this.PalmwebproProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Palmwebpro3Process(headers, browserCaps))
			{
				flag = false;
			}
			this.PalmwebproProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x0006B35F File Offset: 0x0006A35F
		protected virtual void Palmwebpro3ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x0006B361 File Offset: 0x0006A361
		protected virtual void Palmwebpro3ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x0006B364 File Offset: 0x0006A364
		private bool Palmwebpro3Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "WebPro3\\."))
			{
				return false;
			}
			capabilities["CanSendMail"] = "true";
			capabilities["InputType"] = "keyboard";
			capabilities["JavaScript"] = "true";
			capabilities["MaximumRenderedPageSize"] = "520000";
			capabilities["maximumHrefLength"] = "2000";
			capabilities["preferredRequestEncoding"] = "ISO-8859-1";
			capabilities["preferredResponseEncoding"] = "ISO-8859-1";
			capabilities["RequiresControlStateInSession"] = "false";
			capabilities["RequiresOutputOptimization"] = "false";
			capabilities["RequiresPragmaNoCacheHeader"] = "false";
			capabilities["RequiresUniqueFilePathSuffix"] = "false";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["Tables"] = "true";
			browserCaps.AddBrowser("PalmWebPro3");
			this.Palmwebpro3ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Palmwebpro3ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x0006B48E File Offset: 0x0006A48E
		protected virtual void EudorawebProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x0006B490 File Offset: 0x0006A490
		protected virtual void EudorawebProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x0006B494 File Offset: 0x0006A494
		private bool EudorawebProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "EudoraWeb (?'browserMajorVersion'\\d+)(?'browserMinorVersion'\\.\\d+)"))
			{
				return false;
			}
			capabilities["browser"] = "EudoraWeb";
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["cookies"] = "true";
			capabilities["isColor"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isMobileDevice"] = "true";
			capabilities["javaapplets"] = "false";
			capabilities["javascript"] = "false";
			capabilities["maximumRenderedPageSize"] = "30000";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceManufacturer"] = "PalmOS-licensee";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenCharactersHeight"] = "12";
			capabilities["screenCharactersWidth"] = "36";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "160";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsImageSubmit"] = "false";
			capabilities["supportsItalic"] = "true";
			capabilities["tables"] = "false";
			capabilities["type"] = "EudoraWeb";
			capabilities["vbscript"] = "false";
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("Eudoraweb");
			this.EudorawebProcessGateways(headers, browserCaps);
			this.EudorawebmsieProcess(headers, browserCaps);
			bool flag = true;
			if (!this.PdqbrowserProcess(headers, browserCaps) && !this.Eudoraweb21plusProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.EudorawebProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x0006B700 File Offset: 0x0006A700
		protected virtual void EudorawebmsieProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x0006B702 File Offset: 0x0006A702
		protected virtual void EudorawebmsieProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x0006B704 File Offset: 0x0006A704
		private bool EudorawebmsieProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MSIE (?'version'(?'msMajorVersion'\\d+)(?'msMinorVersion'\\.\\d+)(?'letters'\\w*))(?'extra'[^)]*)"))
			{
				return false;
			}
			capabilities["activexcontrols"] = "true";
			capabilities["backgroundsounds"] = "true";
			capabilities["ecmaScriptVersion"] = "1.2";
			capabilities["frames"] = "true";
			capabilities["msdomversion"] = regexWorker["${msMajorVersion}${msMinorVersion}"];
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["w3cdomversion"] = "1.0";
			this.EudorawebmsieProcessGateways(headers, browserCaps);
			bool flag = false;
			this.EudorawebmsieProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x0006B7C9 File Offset: 0x0006A7C9
		protected virtual void PdqbrowserProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x0006B7CB File Offset: 0x0006A7CB
		protected virtual void PdqbrowserProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x0006B7D0 File Offset: 0x0006A7D0
		private bool PdqbrowserProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "pdQbrowser"))
			{
				return false;
			}
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Kyocera";
			capabilities["mobileDeviceModel"] = "QCP 6035";
			capabilities["supportsFontSize"] = "false";
			browserCaps.AddBrowser("PdQbrowser");
			this.PdqbrowserProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PdqbrowserProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x0006B86A File Offset: 0x0006A86A
		protected virtual void Eudoraweb21plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x0006B86C File Offset: 0x0006A86C
		protected virtual void Eudoraweb21plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x0006B870 File Offset: 0x0006A870
		private bool Eudoraweb21plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["version"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "([3-9]\\.\\d+)|(2\\.[1-9]\\d*)"))
			{
				return false;
			}
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["requiresUniqueHtmlCheckboxNames"] = "true";
			capabilities["screenCharactersHeight"] = "11";
			capabilities["screenCharactersWidth"] = "30";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsFontColor"] = "false";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("Eudoraweb21Plus");
			this.Eudoraweb21plusProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Eudoraweb21plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x0006B97F File Offset: 0x0006A97F
		protected virtual void PalmscapeProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x0006B981 File Offset: 0x0006A981
		protected virtual void PalmscapeProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x0006B984 File Offset: 0x0006A984
		private bool PalmscapeProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Palmscape/.*\\(v. (?'version'[^;]+);"))
			{
				return false;
			}
			capabilities["browser"] = "Palmscape";
			capabilities["cookies"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isMobileDevice"] = "true";
			capabilities["mobileDeviceManufacturer"] = "PalmOS-licensee";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["screenCharactersHeight"] = "12";
			capabilities["screenCharactersWidth"] = "36";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "160";
			capabilities["supportsCharacterEntityEncodign"] = "false";
			capabilities["type"] = "Palmscape";
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("Palmscape");
			this.PalmscapeProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.PalmscapeversionProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.PalmscapeProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x0006BAE1 File Offset: 0x0006AAE1
		protected virtual void PalmscapeversionProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x0006BAE3 File Offset: 0x0006AAE3
		protected virtual void PalmscapeversionProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x0006BAE8 File Offset: 0x0006AAE8
		private bool PalmscapeversionProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["version"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'browserMajorVersion'\\d+)(?'browserMinorVersion'\\.\\d+)"))
			{
				return false;
			}
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			browserCaps.AddBrowser("PalmscapeVersion");
			this.PalmscapeversionProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PalmscapeversionProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x0006BB73 File Offset: 0x0006AB73
		protected virtual void AuspalmProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x0006BB75 File Offset: 0x0006AB75
		protected virtual void AuspalmProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x0006BB78 File Offset: 0x0006AB78
		private bool AuspalmProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "AUS PALM WAPPER"))
			{
				return false;
			}
			capabilities["browser"] = "AU-System Demo WAP Browser";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isMobileDevice"] = "true";
			capabilities["mobileDeviceManufacturer"] = "PalmOS-licensee";
			capabilities["optimumPageWeight"] = "900";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml11";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenCharactersHeight"] = "12";
			capabilities["screenCharactersWidth"] = "36";
			capabilities["screenPixelsHeight"] = "160";
			capabilities["screenPixelsWidth"] = "160";
			capabilities["supportsCharacterEntityEncoding"] = "true";
			capabilities["type"] = "AU-System";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("AusPalm");
			this.AuspalmProcessGateways(headers, browserCaps);
			bool flag = false;
			this.AuspalmProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x0006BD07 File Offset: 0x0006AD07
		protected virtual void SharppdaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x0006BD09 File Offset: 0x0006AD09
		protected virtual void SharppdaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x0006BD0C File Offset: 0x0006AD0C
		private bool SharppdaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "sharp pda browser/(?'browserMajorVersion'\\d+)(?'browserMinorVersion'\\.\\d+)"))
			{
				return false;
			}
			capabilities["browser"] = "Sharp PDA Browser";
			capabilities["isMobileDevice"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["mobileDeviceManufacturer"] = "Sharp";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["supportsCharacterEntityEncoding"] = "false";
			capabilities["type"] = "Sharp PDA Browser";
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("SharpPda");
			this.SharppdaProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Zaurusmie1Process(headers, browserCaps) && !this.Zaurusmie21Process(headers, browserCaps) && !this.Zaurusmie25Process(headers, browserCaps))
			{
				flag = false;
			}
			this.SharppdaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x0006BE49 File Offset: 0x0006AE49
		protected virtual void Zaurusmie1ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0006BE4B File Offset: 0x0006AE4B
		protected virtual void Zaurusmie1ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0006BE50 File Offset: 0x0006AE50
		private bool Zaurusmie1Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MI-E1"))
			{
				return false;
			}
			capabilities["defaultCharacterHeight"] = "18";
			capabilities["defaultCharacterWidth"] = "7";
			capabilities["frames"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["javascript"] = "false";
			capabilities["mobileDeviceModel"] = "Zaurus MI-E1";
			capabilities["requiresDBCSCharacter"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenPixelsHeight"] = "240";
			capabilities["screenPixelsWidth"] = "320";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("ZaurusMiE1");
			this.Zaurusmie1ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Zaurusmie1ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x0006BF8A File Offset: 0x0006AF8A
		protected virtual void Zaurusmie21ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x0006BF8C File Offset: 0x0006AF8C
		protected virtual void Zaurusmie21ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x0006BF90 File Offset: 0x0006AF90
		private bool Zaurusmie21Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MI-E21"))
			{
				return false;
			}
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["canRenderEmptySelects"] = "false";
			capabilities["cookies"] = "true";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "60000";
			capabilities["mobileDeviceModel"] = "Zaurus MI-E21";
			capabilities["requiresAttributeColonSubstitution"] = "true";
			capabilities["requiresDBCSCharacter"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "18";
			capabilities["screenCharactersWidth"] = "40";
			capabilities["screenPixelsHeight"] = "320";
			capabilities["screenPixelsWidth"] = "240";
			capabilities["supportsFontSize"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("ZaurusMiE21");
			this.Zaurusmie21ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Zaurusmie21ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x0006C0FA File Offset: 0x0006B0FA
		protected virtual void Zaurusmie25ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x0006C0FC File Offset: 0x0006B0FC
		protected virtual void Zaurusmie25ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x0006C100 File Offset: 0x0006B100
		private bool Zaurusmie25Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MI-E25"))
			{
				return false;
			}
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["cookies"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "60000";
			capabilities["mobileDeviceModel"] = "Zaurus MI-E25DC";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresContentTypeMetaTag"] = "false";
			capabilities["requiresDBCSCharacter"] = "true";
			capabilities["requiresOutputOptimization"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "11";
			capabilities["screenCharactersWidth"] = "50";
			capabilities["screenPixelsHeight"] = "240";
			capabilities["screenPixelsWidth"] = "320";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("ZaurusMiE25");
			this.Zaurusmie25ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Zaurusmie25ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x0006C27A File Offset: 0x0006B27A
		protected virtual void PanasonicProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x0006C27C File Offset: 0x0006B27C
		protected virtual void PanasonicProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x0006C280 File Offset: 0x0006B280
		private bool PanasonicProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Panasonic-(?'deviceModel'.*)"))
			{
				return false;
			}
			capabilities["browser"] = "Panasonic";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["maximumSoftkeyLabelLength"] = "16";
			capabilities["mobileDeviceManufacturer"] = "Panasonic";
			capabilities["mobileDeviceModel"] = regexWorker["${deviceModel}"];
			capabilities["numberOfSoftkeys"] = "2";
			capabilities["preferredImageMime"] = "image/vnd.wap.wbmp";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersWmlDoAcceptsInline"] = "false";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "16";
			capabilities["screenPixelsHeight"] = "130";
			capabilities["screenPixelsWidth"] = "100";
			capabilities["supportsCacheControlMetaTag"] = "false";
			capabilities["tables"] = "false";
			capabilities["type"] = "Panasonic";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("Panasonic");
			this.PanasonicProcessGateways(headers, browserCaps);
			this.PanasonicexchangesupporteddeviceProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Panasonicgad95Process(headers, browserCaps) && !this.Panasonicgad87Process(headers, browserCaps))
			{
				flag = false;
			}
			this.PanasonicProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x0006C475 File Offset: 0x0006B475
		protected virtual void Panasonicgad95ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x0006C477 File Offset: 0x0006B477
		protected virtual void Panasonicgad95ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x0006C47C File Offset: 0x0006B47C
		private bool Panasonicgad95Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "GAD95"))
			{
				return false;
			}
			browserCaps.AddBrowser("PanasonicGAD95");
			this.Panasonicgad95ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Panasonicgad95ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0006C4DB File Offset: 0x0006B4DB
		protected virtual void Panasonicgad87ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x0006C4DD File Offset: 0x0006B4DD
		protected virtual void Panasonicgad87ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x0006C4E0 File Offset: 0x0006B4E0
		private bool Panasonicgad87Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "GAD87"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "Panasonic";
			capabilities["canSendMail"] = "true";
			capabilities["cookies"] = "true";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "12000";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-basic";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenPixelsHeight"] = "176";
			capabilities["screenPixelsWidth"] = "132";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsNoWrapStyle"] = "false";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "false";
			capabilities["type"] = "Panasonic";
			browserCaps.AddBrowser("PanasonicGAD87");
			this.Panasonicgad87ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Panasonicgad87a39Process(headers, browserCaps) && !this.Panasonicgad87a38Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Panasonicgad87ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x0006C736 File Offset: 0x0006B736
		protected virtual void PanasonicexchangesupporteddeviceProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x0006C738 File Offset: 0x0006B738
		protected virtual void PanasonicexchangesupporteddeviceProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x0006C73C File Offset: 0x0006B73C
		private bool PanasonicexchangesupporteddeviceProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "\\/(A3[89]|A[4-9]\\d|A\\d{3,})"))
			{
				return false;
			}
			capabilities["ExchangeOmaSupported"] = "true";
			this.PanasonicexchangesupporteddeviceProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PanasonicexchangesupporteddeviceProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x0006C7A0 File Offset: 0x0006B7A0
		protected virtual void Panasonicgad87a39ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0006C7A2 File Offset: 0x0006B7A2
		protected virtual void Panasonicgad87a39ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x0006C7A4 File Offset: 0x0006B7A4
		private bool Panasonicgad87a39Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "GAD87\\/A39"))
			{
				return false;
			}
			capabilities["mobileDeviceModel"] = "GD87";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "7";
			capabilities["screenCharactersWidth"] = "14";
			capabilities["tables"] = "false";
			browserCaps.AddBrowser("PanasonicGAD87A39");
			this.Panasonicgad87a39ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Panasonicgad87a39ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x0006C853 File Offset: 0x0006B853
		protected virtual void Panasonicgad87a38ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x0006C855 File Offset: 0x0006B855
		protected virtual void Panasonicgad87a38ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x0006C858 File Offset: 0x0006B858
		private bool Panasonicgad87a38Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["mobileDeviceModel"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "GAD87\\/A38"))
			{
				return false;
			}
			capabilities["maximumRenderedPageSize"] = "12155";
			capabilities["mobileDeviceModel"] = "GU87";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "20";
			browserCaps.AddBrowser("PanasonicGAD87A38");
			this.Panasonicgad87a38ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Panasonicgad87a38ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x0006C907 File Offset: 0x0006B907
		protected virtual void WinceProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x0006C909 File Offset: 0x0006B909
		protected virtual void WinceProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x0006C90C File Offset: 0x0006B90C
		private bool WinceProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Mozilla/\\S* \\(compatible; MSIE (?'majorVersion'\\d*)(?'minorVersion'\\.\\d*);\\D* Windows CE(;(?'deviceID' \\D\\w*))?(; (?'screenWidth'\\d+)x(?'screenHeight'\\d+))?"))
			{
				return false;
			}
			capabilities["activexcontrols"] = "true";
			capabilities["backgroundsounds"] = "true";
			capabilities["browser"] = "WinCE";
			capabilities["cookies"] = "true";
			capabilities["css1"] = "true";
			capabilities["defaultScreenCharactersHeight"] = "6";
			capabilities["defaultScreenCharactersWidth"] = "12";
			capabilities["defaultScreenPixelsHeight"] = "72";
			capabilities["defaultScreenPixelsWidth"] = "96";
			capabilities["deviceID"] = regexWorker["${deviceID}"];
			capabilities["ecmascriptversion"] = "1.0";
			capabilities["frames"] = "true";
			capabilities["inputType"] = "telephoneKeypad";
			capabilities["isColor"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["javascript"] = "true";
			capabilities["jscriptversion"] = "1.0";
			capabilities["majorVersion"] = regexWorker["${majorVersion}"];
			capabilities["minorVersion"] = regexWorker["${minorVersion}"];
			capabilities["platform"] = "WinCE";
			capabilities["screenBitDepth"] = "1";
			capabilities["screenPixelsHeight"] = regexWorker["${screenHeight}"];
			capabilities["screenPixelsWidth"] = regexWorker["${screenWidth}"];
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["tables"] = "true";
			capabilities["version"] = regexWorker["${majorVersion}${minorVersion}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("WinCE");
			this.WinceProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.PieProcess(headers, browserCaps) && !this.Pie4Process(headers, browserCaps) && !this.Pie5plusProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.WinceProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x0006CB6B File Offset: 0x0006BB6B
		protected virtual void PieProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x0006CB6D File Offset: 0x0006BB6D
		protected virtual void PieProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x0006CB70 File Offset: 0x0006BB70
		private bool PieProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["version"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^3\\.02$"))
			{
				return false;
			}
			capabilities["browser"] = "Pocket IE";
			capabilities["defaultCharacterHeight"] = "18";
			capabilities["defaultCharacterWidth"] = "7";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["javascript"] = "true";
			capabilities["majorVersion"] = "4";
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["minorVersion"] = ".0";
			capabilities["mobileDeviceModel"] = "Pocket PC";
			capabilities["optimumPageWeight"] = "4000";
			capabilities["requiresContentTypeMetaTag"] = "true";
			capabilities["requiresLeadingPageBreak"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["supportsBodyColor"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivAlign"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFontColor"] = "true";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["type"] = "Pocket IE";
			capabilities["version"] = "4.0";
			browserCaps.AddBrowser("PIE");
			this.PieProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.PieppcProcess(headers, browserCaps) && !this.PienodeviceidProcess(headers, browserCaps) && !this.PiesmartphoneProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.PieProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x0006CDA0 File Offset: 0x0006BDA0
		protected virtual void PieppcProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x0006CDA2 File Offset: 0x0006BDA2
		protected virtual void PieppcProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x0006CDA4 File Offset: 0x0006BDA4
		private bool PieppcProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, " PPC;"))
			{
				return false;
			}
			capabilities["minorVersion"] = ".1";
			capabilities["version"] = "4.1";
			browserCaps.AddBrowser("PIEPPC");
			this.PieppcProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PieppcProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x0006CE1E File Offset: 0x0006BE1E
		protected virtual void Pie4ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x0006CE20 File Offset: 0x0006BE20
		protected virtual void Pie4ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x0006CE24 File Offset: 0x0006BE24
		private bool Pie4Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MSIE 4(\\.\\d*)"))
			{
				return false;
			}
			capabilities["activexcontrols"] = "true";
			capabilities["browser"] = "Pocket IE";
			capabilities["cdf"] = "true";
			capabilities["defaultScreenCharactersHeight"] = "6";
			capabilities["defaultScreenCharactersWidth"] = "12";
			capabilities["defaultScreenPixelsHeight"] = "72";
			capabilities["defaultScreenPixelsWidth"] = "96";
			capabilities["ecmascriptversion"] = "1.2";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["maximumRenderedPageSize"] = "7000";
			capabilities["msdomversion"] = "4.0";
			capabilities["platform"] = "WinCE";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "9";
			capabilities["screenCharactersWidth"] = "50";
			capabilities["screenPixelsHeight"] = "240";
			capabilities["screenPixelsWidth"] = "640";
			capabilities["supportsCss"] = "true";
			capabilities["supportsDivNoWrap"] = "true";
			capabilities["supportsFileUpload"] = "false";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["type"] = "Pocket IE";
			capabilities["vbscript"] = "true";
			browserCaps.AddBrowser("PIE4");
			this.Pie4ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Pie4ppcProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Pie4ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x0006D03B File Offset: 0x0006C03B
		protected virtual void Pie5plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x0006D03D File Offset: 0x0006C03D
		protected virtual void Pie5plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x0006D040 File Offset: 0x0006C040
		private bool Pie5plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MSIE 5(\\.\\d*)"))
			{
				return false;
			}
			capabilities["browser"] = "IE";
			capabilities["ecmascriptversion"] = "1.2";
			capabilities["javaapplets"] = "true";
			capabilities["msdomversion"] = "5.5";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["type"] = "Pocket IE";
			capabilities["vbscript"] = "true";
			capabilities["w3cdomversion"] = "1.0";
			capabilities["xml"] = "true";
			browserCaps.AddBrowser("PIE5Plus");
			this.Pie5plusProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Sigmarion3Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Pie5plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x0006D137 File Offset: 0x0006C137
		protected virtual void PienodeviceidProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x0006D139 File Offset: 0x0006C139
		protected virtual void PienodeviceidProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x0006D13C File Offset: 0x0006C13C
		private bool PienodeviceidProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^$"))
			{
				return false;
			}
			capabilities["supportsQueryStringInFormAction"] = "false";
			browserCaps.AddBrowser("PIEnoDeviceID");
			this.PienodeviceidProcessGateways(headers, browserCaps);
			this.PiescreenbitdepthProcess(headers, browserCaps);
			bool flag = false;
			this.PienodeviceidProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x0006D1B4 File Offset: 0x0006C1B4
		protected virtual void PiescreenbitdepthProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x0006D1B6 File Offset: 0x0006C1B6
		protected virtual void PiescreenbitdepthProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x0006D1B8 File Offset: 0x0006C1B8
		private bool PiescreenbitdepthProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["UA-COLOR"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "color32"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["screenBitDepth"] = "32";
			this.PiescreenbitdepthProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PiescreenbitdepthProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x0006D21D File Offset: 0x0006C21D
		protected virtual void NetfrontProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0006D21F File Offset: 0x0006C21F
		protected virtual void NetfrontProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x0006D224 File Offset: 0x0006C224
		private bool NetfrontProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "\\((?'deviceID'.*)\\) NetFront\\/(?'browserMajorVersion'\\d*)(?'browserMinorVersion'\\.\\d*).*"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "Compact NetFront";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["canSendMail"] = "false";
			capabilities["inputType"] = "telephoneKeypad";
			capabilities["isMobileDevice"] = "true";
			capabilities["javascript"] = "false";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresFullyQualifiedRedirectUrl"] = "true";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFileUpload"] = "false";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsImageSubmit"] = "false";
			capabilities["supportsItalic"] = "false";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["type"] = regexWorker["Compact NetFront ${browserMajorVersion}"];
			browserCaps.AddBrowser("NetFront");
			this.NetfrontProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Slb500Process(headers, browserCaps) && !this.VrnaProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.NetfrontProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x0006D49D File Offset: 0x0006C49D
		protected virtual void Slb500ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x0006D49F File Offset: 0x0006C49F
		protected virtual void Slb500ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x0006D4A4 File Offset: 0x0006C4A4
		private bool Slb500Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SL-B500"))
			{
				return false;
			}
			capabilities["browser"] = "NetFront";
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "60000";
			capabilities["mobileDeviceManufacturer"] = "Sharp";
			capabilities["mobileDeviceModel"] = "SL-B500";
			capabilities["preferredImageMime"] = "image/gif";
			capabilities["preferredRenderingMime"] = "text/html";
			capabilities["preferredRenderingType"] = "html32";
			capabilities["requiresContentTypeMetaTag"] = "false";
			capabilities["requiresNoBreakInFormatting"] = "true";
			capabilities["requiresOutputOptimization"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "21";
			capabilities["screenCharactersWidth"] = "47";
			capabilities["screenPixelsHeight"] = "240";
			capabilities["screenPixelsWidth"] = "320";
			capabilities["supportsAccessKeyAttribute"] = "false";
			capabilities["supportsBold"] = "false";
			capabilities["supportsFontSize"] = "false";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsMultilineTextboxDisplay"] = "true";
			browserCaps.AddBrowser("SLB500");
			this.Slb500ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Slb500ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0006D68E File Offset: 0x0006C68E
		protected virtual void VrnaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0006D690 File Offset: 0x0006C690
		protected virtual void VrnaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0006D694 File Offset: 0x0006C694
		private bool VrnaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "sony/model vrna"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Sony";
			capabilities["mobileDeviceModel"] = "CLIE PEG-TG50";
			capabilities["canInitiateVoiceCall"] = "false";
			capabilities["canSendMail"] = "true";
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["javascript"] = "true";
			capabilities["maximumRenderedPageSize"] = "65000";
			capabilities["preferredRenderingMime"] = "text/html";
			capabilities["preferredRenderingType"] = "html32";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOutputOptimization"] = "true";
			capabilities["requiresPragmaNoCacheHeader"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenCharactersWidth"] = "31";
			capabilities["screenPixelsHeight"] = "320";
			capabilities["screenPixelsWidth"] = "320";
			capabilities["supportsAccessKeyAttribute"] = "false";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsMultilineTextboxDisplay"] = "true";
			browserCaps.AddBrowser("VRNA");
			this.VrnaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.VrnaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x0006D84E File Offset: 0x0006C84E
		protected virtual void Pie4ppcProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x0006D850 File Offset: 0x0006C850
		protected virtual void Pie4ppcProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x0006D854 File Offset: 0x0006C854
		private bool Pie4ppcProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "PPC(; (?'screenWidth'\\d+)x(?'screenHeight'\\d+))?"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "MSIE";
			capabilities["ecmascriptversion"] = "1.0";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["maximumRenderedPageSize"] = "800000";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingType"] = "html32";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "16";
			capabilities["screenCharactersWidth"] = "32";
			capabilities["screenPixelsHeight"] = regexWorker["${screenHeight}"];
			capabilities["screenPixelsWidth"] = regexWorker["${screenWidth}"];
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsNoWrapStyle"] = "false";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsTitleElement"] = "false";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["type"] = regexWorker["MSIE ${majorVersion}"];
			browserCaps.AddBrowser("PIE4PPC");
			this.Pie4ppcProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Pie4ppcProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x0006DA70 File Offset: 0x0006CA70
		protected virtual void PiesmartphoneProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x0006DA72 File Offset: 0x0006CA72
		protected virtual void PiesmartphoneProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x0006DA74 File Offset: 0x0006CA74
		private bool PiesmartphoneProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Smartphone"))
			{
				return false;
			}
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["frames"] = "false";
			capabilities["inputType"] = "telephoneKeypad";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["minorVersion"] = ".1";
			capabilities["mobileDeviceModel"] = "Smartphone";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["requiresContentTypeMetaTag"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["screenCharactersHeight"] = "13";
			capabilities["screenCharactersWidth"] = "28";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsFontName"] = "false";
			capabilities["version"] = "4.1";
			browserCaps.AddBrowser("PIESmartphone");
			this.PiesmartphoneProcessGateways(headers, browserCaps);
			bool flag = false;
			this.PiesmartphoneProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x0006DBD3 File Offset: 0x0006CBD3
		protected virtual void Sigmarion3ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x0006DBD5 File Offset: 0x0006CBD5
		protected virtual void Sigmarion3ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x0006DBD8 File Offset: 0x0006CBD8
		private bool Sigmarion3Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "sigmarion3"))
			{
				return false;
			}
			capabilities["cachesAllResponsesWithExpires"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["maximumRenderedPageSize"] = "64000";
			capabilities["mobileDeviceManufacturer"] = "NTT DoCoMo";
			capabilities["MobileDeviceModel"] = "Sigmarion III";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRequestEncoding"] = "shift_jis";
			capabilities["preferredResponseEncoding"] = "shift_jis";
			capabilities["requiresOutputOptimization"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["screenCharactersHeight"] = "19";
			capabilities["screenCharactersWidth"] = "94";
			capabilities["screenPixelsHeight"] = "480";
			capabilities["screenPixelsWidth"] = "800";
			browserCaps.AddBrowser("sigmarion3");
			this.Sigmarion3ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sigmarion3ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x0006DD27 File Offset: 0x0006CD27
		protected virtual void Mspie06ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0006DD29 File Offset: 0x0006CD29
		protected virtual void Mspie06ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x0006DD2C File Offset: 0x0006CD2C
		private bool Mspie06Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Microsoft Pocket Internet Explorer/0\\.6"))
			{
				return false;
			}
			capabilities["backgroundsounds"] = "true";
			capabilities["browser"] = "PIE";
			capabilities["cookies"] = "false";
			capabilities["isMobileDevice"] = "true";
			capabilities["majorversion"] = "1";
			capabilities["minorversion"] = "0";
			capabilities["platform"] = "WinCE";
			capabilities["tables"] = "true";
			capabilities["type"] = "PIE";
			capabilities["version"] = "1.0";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("MSPIE06");
			this.Mspie06ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mspie06ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x0006DE46 File Offset: 0x0006CE46
		protected virtual void MspieProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x0006DE48 File Offset: 0x0006CE48
		protected virtual void MspieProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x0006DE4C File Offset: 0x0006CE4C
		private bool MspieProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^Mozilla[^(]*\\(compatible; MSPIE (?'version'(?'major'\\d+)(?'minor'\\.\\d+)(?'letters'\\w*))(?'extra'.*)"))
			{
				return false;
			}
			capabilities["backgroundsounds"] = "true";
			capabilities["browser"] = "PIE";
			capabilities["cookies"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["tables"] = "true";
			capabilities["type"] = regexWorker["PIE${major}"];
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("MSPIE");
			this.MspieProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Mspie2Process(headers, browserCaps))
			{
				flag = false;
			}
			this.MspieProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x0006DF7B File Offset: 0x0006CF7B
		protected virtual void Mspie2ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x0006DF7D File Offset: 0x0006CF7D
		protected virtual void Mspie2ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x0006DF80 File Offset: 0x0006CF80
		private bool Mspie2Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["version"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "2\\."))
			{
				return false;
			}
			capabilities["frames"] = "true";
			browserCaps.AddBrowser("MSPIE2");
			this.Mspie2ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Mspie2ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x0006DFEF File Offset: 0x0006CFEF
		protected virtual void SktdevicesProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x0006DFF1 File Offset: 0x0006CFF1
		protected virtual void SktdevicesProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x0006DFF4 File Offset: 0x0006CFF4
		private bool SktdevicesProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^(?'carrier'\\S{3})(?'serviceType'\\d)(?'deviceType'\\d)(?'deviceManufacturer'\\S{2})(?'deviceID'\\S{2})(?'browserType'\\d{2})(?'majorVersion'\\d)(?'minorVersion'\\d)(?'screenWidth'\\d{3})(?'screenHeight'\\d{3})(?'screenColumn'\\d{2})(?'screenRow'\\d{2})(?'colorDepth'\\d{2})(?'MINNumber'\\d{8})"))
			{
				return false;
			}
			capabilities["browserType"] = regexWorker["${browserType}"];
			capabilities["colorDepth"] = regexWorker["${colorDepth}"];
			capabilities["cookies"] = "false";
			capabilities["deviceManufacturer"] = regexWorker["${deviceManufacturer}"];
			capabilities["deviceID"] = regexWorker["${deviceID}"];
			capabilities["deviceType"] = regexWorker["${deviceType}"];
			capabilities["isMobileDevice"] = "true";
			capabilities["majorVersion"] = regexWorker["${majorVersion}"];
			capabilities["minorVersion"] = regexWorker["${minorVersion}"];
			capabilities["screenColumn"] = regexWorker["${screenColumn}"];
			capabilities["screenHeight"] = regexWorker["${screenHeight}"];
			capabilities["screenWidth"] = regexWorker["${screenWidth}"];
			capabilities["screenRow"] = regexWorker["${screenRow}"];
			capabilities["version"] = regexWorker["${majorVersion}.${minorVersion}"];
			browserCaps.AddBrowser("SKTDevices");
			this.SktdevicesProcessGateways(headers, browserCaps);
			this.SktdevicescolordepthProcess(headers, browserCaps);
			this.SktdevicesscreenrowProcess(headers, browserCaps);
			this.SktdevicesscreencolumnProcess(headers, browserCaps);
			this.SktdevicesscreenheightProcess(headers, browserCaps);
			this.SktdevicesscreenwidthProcess(headers, browserCaps);
			bool flag = true;
			if (!this.SktdeviceshyundaiProcess(headers, browserCaps) && !this.SktdeviceshanhwaProcess(headers, browserCaps) && !this.SktdevicesjtelProcess(headers, browserCaps) && !this.SktdeviceslgProcess(headers, browserCaps) && !this.SktdevicesmotorolaProcess(headers, browserCaps) && !this.SktdevicesnokiaProcess(headers, browserCaps) && !this.SktdevicesskttProcess(headers, browserCaps) && !this.SktdevicessamsungProcess(headers, browserCaps) && !this.SktdevicesericssonProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.SktdevicesProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x0006E200 File Offset: 0x0006D200
		protected virtual void SktdevicescolordepthProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x0006E202 File Offset: 0x0006D202
		protected virtual void SktdevicescolordepthProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x0006E204 File Offset: 0x0006D204
		private bool SktdevicescolordepthProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["colorDepth"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^(\\d[1-9])|([1-9]0)$"))
			{
				return false;
			}
			capabilities["screenBitDepth"] = regexWorker["${colorDepth}"];
			this.SktdevicescolordepthProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.SktdevicesiscolorProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.SktdevicescolordepthProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0006E27B File Offset: 0x0006D27B
		protected virtual void SktdevicesiscolorProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x0006E27D File Offset: 0x0006D27D
		protected virtual void SktdevicesiscolorProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x0006E280 File Offset: 0x0006D280
		private bool SktdevicesiscolorProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["colorDepth"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^(0[5-9])|([1-9]\\d)$"))
			{
				return false;
			}
			capabilities["isColor"] = "true";
			this.SktdevicesiscolorProcessGateways(headers, browserCaps);
			bool flag = false;
			this.SktdevicesiscolorProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x0006E2E4 File Offset: 0x0006D2E4
		protected virtual void SktdevicesscreenrowProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x0006E2E6 File Offset: 0x0006D2E6
		protected virtual void SktdevicesscreenrowProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x0006E2E8 File Offset: 0x0006D2E8
		private bool SktdevicesscreenrowProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["screenRow"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^(\\d[1-9])|([1-9]0)$"))
			{
				return false;
			}
			capabilities["screenCharactersHeight"] = regexWorker["${screenRow}"];
			this.SktdevicesscreenrowProcessGateways(headers, browserCaps);
			bool flag = false;
			this.SktdevicesscreenrowProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0006E352 File Offset: 0x0006D352
		protected virtual void SktdevicesscreencolumnProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x0006E354 File Offset: 0x0006D354
		protected virtual void SktdevicesscreencolumnProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0006E358 File Offset: 0x0006D358
		private bool SktdevicesscreencolumnProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["screenColumn"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^(\\d[1-9])|([1-9]0)$"))
			{
				return false;
			}
			capabilities["screenCharactersWidth"] = regexWorker["${screenColumn}"];
			this.SktdevicesscreencolumnProcessGateways(headers, browserCaps);
			bool flag = false;
			this.SktdevicesscreencolumnProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0006E3C2 File Offset: 0x0006D3C2
		protected virtual void SktdevicesscreenheightProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0006E3C4 File Offset: 0x0006D3C4
		protected virtual void SktdevicesscreenheightProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x0006E3C8 File Offset: 0x0006D3C8
		private bool SktdevicesscreenheightProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["screenHeight"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^(\\d\\d[1-9])|(((\\d[1-9])|([1-9]0))0)$"))
			{
				return false;
			}
			capabilities["screenPixelsHeight"] = regexWorker["${screenHeight}"];
			this.SktdevicesscreenheightProcessGateways(headers, browserCaps);
			bool flag = false;
			this.SktdevicesscreenheightProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x0006E432 File Offset: 0x0006D432
		protected virtual void SktdevicesscreenwidthProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x0006E434 File Offset: 0x0006D434
		protected virtual void SktdevicesscreenwidthProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x0006E438 File Offset: 0x0006D438
		private bool SktdevicesscreenwidthProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["screenWidth"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^(\\d\\d[1-9])|(((\\d[1-9])|([1-9]0))0)$"))
			{
				return false;
			}
			capabilities["screenPixelsWidth"] = regexWorker["${screenWidth}"];
			this.SktdevicesscreenwidthProcessGateways(headers, browserCaps);
			bool flag = false;
			this.SktdevicesscreenwidthProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x0006E4A2 File Offset: 0x0006D4A2
		protected virtual void SktdeviceshyundaiProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x0006E4A4 File Offset: 0x0006D4A4
		protected virtual void SktdeviceshyundaiProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0006E4A8 File Offset: 0x0006D4A8
		private bool SktdeviceshyundaiProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceManufacturer"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "HD"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Hyundai";
			browserCaps.AddBrowser("SKTDevicesHyundai");
			this.SktdeviceshyundaiProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Pse200Process(headers, browserCaps))
			{
				flag = false;
			}
			this.SktdeviceshyundaiProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0006E524 File Offset: 0x0006D524
		protected virtual void Pse200ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x0006E526 File Offset: 0x0006D526
		protected virtual void Pse200ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0006E528 File Offset: 0x0006D528
		private bool Pse200Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "10"))
			{
				return false;
			}
			text = (string)capabilities["browserType"];
			if (!regexWorker.ProcessRegex(text, "15"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "Infraware";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "true";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["mobileDeviceManufacturer"] = "Pantech&Curitel";
			capabilities["mobileDeviceModel"] = "PS-E200";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresOnEnterForwardForCheckboxLists"] = "true";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "10";
			capabilities["screenPixelsHeight"] = "80";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsNoWrapStyle"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = regexWorker["${browser} ${majorVersion}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.XhtmlTextWriter";
			browserCaps.AddBrowser("PSE200");
			this.Pse200ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Pse200ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x0006E7D0 File Offset: 0x0006D7D0
		protected virtual void SktdeviceshanhwaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0006E7D2 File Offset: 0x0006D7D2
		protected virtual void SktdeviceshanhwaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0006E7D4 File Offset: 0x0006D7D4
		private bool SktdeviceshanhwaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceManufacturer"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "HH"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Hanhwa";
			browserCaps.AddBrowser("SKTDevicesHanhwa");
			this.SktdeviceshanhwaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.SktdeviceshanhwaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x0006E843 File Offset: 0x0006D843
		protected virtual void SktdevicesjtelProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x0006E845 File Offset: 0x0006D845
		protected virtual void SktdevicesjtelProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0006E848 File Offset: 0x0006D848
		private bool SktdevicesjtelProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceManufacturer"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "JT"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "JTEL";
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.HtmlTextWriter = "System.Web.UI.ChtmlTextWriter";
			browserCaps.AddBrowser("SKTDevicesJTEL");
			this.SktdevicesjtelProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Jtel01Process(headers, browserCaps))
			{
				flag = false;
			}
			this.SktdevicesjtelProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0006E8E4 File Offset: 0x0006D8E4
		protected virtual void Jtel01ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x0006E8E6 File Offset: 0x0006D8E6
		protected virtual void Jtel01ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0006E8E8 File Offset: 0x0006D8E8
		private bool Jtel01Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "01"))
			{
				return false;
			}
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["mobileDeviceModel"] = "Cellvic XG";
			capabilities["screenCharactersHeight"] = "6";
			capabilities["screenCharactersWidth"] = "12";
			browserCaps.AddBrowser("JTEL01");
			this.Jtel01ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.JtelnateProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Jtel01ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0006E994 File Offset: 0x0006D994
		protected virtual void JtelnateProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x0006E996 File Offset: 0x0006D996
		protected virtual void JtelnateProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x0006E998 File Offset: 0x0006D998
		private bool JtelnateProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["browserType"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "03"))
			{
				return false;
			}
			capabilities["browser"] = "NATE";
			capabilities["cookies"] = "true";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "true";
			capabilities["requiresLeadingPageBreak"] = "true";
			capabilities["supportsBodyColor"] = "false";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontColor"] = "false";
			capabilities["tables"] = "true";
			capabilities["type"] = "NATE 1";
			browserCaps.AddBrowser("JTELNate");
			this.JtelnateProcessGateways(headers, browserCaps);
			bool flag = false;
			this.JtelnateProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x0006EA87 File Offset: 0x0006DA87
		protected virtual void SktdeviceslgProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x0006EA89 File Offset: 0x0006DA89
		protected virtual void SktdeviceslgProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x0006EA8C File Offset: 0x0006DA8C
		private bool SktdeviceslgProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceManufacturer"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "LG"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "LG";
			browserCaps.AddBrowser("SKTDevicesLG");
			this.SktdeviceslgProcessGateways(headers, browserCaps);
			bool flag = false;
			this.SktdeviceslgProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x0006EAFB File Offset: 0x0006DAFB
		protected virtual void SktdevicesmotorolaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x0006EAFD File Offset: 0x0006DAFD
		protected virtual void SktdevicesmotorolaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x0006EB00 File Offset: 0x0006DB00
		private bool SktdevicesmotorolaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceManufacturer"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MT"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Motorola";
			browserCaps.AddBrowser("SKTDevicesMotorola");
			this.SktdevicesmotorolaProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Sktdevicesv730Process(headers, browserCaps))
			{
				flag = false;
			}
			this.SktdevicesmotorolaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x0006EB7C File Offset: 0x0006DB7C
		protected virtual void Sktdevicesv730ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x0006EB7E File Offset: 0x0006DB7E
		protected virtual void Sktdevicesv730ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0006EB80 File Offset: 0x0006DB80
		private bool Sktdevicesv730Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceID"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "07"))
			{
				return false;
			}
			text = (string)capabilities["browserType"];
			if (!regexWorker.ProcessRegex(text, "00"))
			{
				return false;
			}
			capabilities["browser"] = "AU-System";
			capabilities["canInitiateVoiceCall"] = "true";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "false";
			capabilities["maximumRenderedPageSize"] = "2900";
			capabilities["mobileDeviceModel"] = "V730";
			capabilities["numberOfSoftkeys"] = "2";
			capabilities["preferredImageMime"] = "image/bmp";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml11";
			capabilities["rendersBreakBeforeWmlSelectAndInput"] = "true";
			capabilities["rendersBreaksAfterWmlInput"] = "true";
			capabilities["rendersWmlSelectsAsMenuCards"] = "true";
			capabilities["requiresNoSoftkeyLabels"] = "true";
			capabilities["requiresUniqueFilePathSuffix"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["type"] = regexWorker["${browser} ${majorVersion}"];
			browserCaps.AddBrowser("SKTDevicesV730");
			this.Sktdevicesv730ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sktdevicesv730ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x0006ED18 File Offset: 0x0006DD18
		protected virtual void SktdevicesnokiaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x0006ED1A File Offset: 0x0006DD1A
		protected virtual void SktdevicesnokiaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x0006ED1C File Offset: 0x0006DD1C
		private bool SktdevicesnokiaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceManufacturer"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "NO"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Nokia";
			browserCaps.AddBrowser("SKTDevicesNokia");
			this.SktdevicesnokiaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.SktdevicesnokiaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x0006ED8B File Offset: 0x0006DD8B
		protected virtual void SktdevicesskttProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x0006ED8D File Offset: 0x0006DD8D
		protected virtual void SktdevicesskttProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0006ED90 File Offset: 0x0006DD90
		private bool SktdevicesskttProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceManufacturer"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SK"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "SKTT";
			browserCaps.AddBrowser("SKTDevicesSKTT");
			this.SktdevicesskttProcessGateways(headers, browserCaps);
			bool flag = false;
			this.SktdevicesskttProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0006EDFF File Offset: 0x0006DDFF
		protected virtual void SktdevicessamsungProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0006EE01 File Offset: 0x0006DE01
		protected virtual void SktdevicessamsungProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0006EE04 File Offset: 0x0006DE04
		private bool SktdevicessamsungProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["deviceManufacturer"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "SS"))
			{
				return false;
			}
			capabilities["mobileDeviceManufacturer"] = "Samsung";
			browserCaps.AddBrowser("SKTDevicesSamSung");
			this.SktdevicessamsungProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Sche150Process(headers, browserCaps))
			{
				flag = false;
			}
			this.SktdevicessamsungProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0006EE80 File Offset: 0x0006DE80
		protected virtual void Sche150ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0006EE82 File Offset: 0x0006DE82
		protected virtual void Sche150ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0006EE84 File Offset: 0x0006DE84
		private bool Sche150Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["browserType"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "15"))
			{
				return false;
			}
			text = (string)capabilities["deviceID"];
			if (!regexWorker.ProcessRegex(text, "50"))
			{
				return false;
			}
			capabilities["breaksOnInlineElements"] = "false";
			capabilities["browser"] = "Infraware";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "true";
			capabilities["maximumRenderedPageSize"] = "10000";
			capabilities["mobileDeviceModel"] = "SCH-E150";
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "application/xhtml+xml";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresCommentInStyleElement"] = "false";
			capabilities["requiresHiddenFieldValues"] = "false";
			capabilities["requiresHtmlAdaptiveErrorReporting"] = "true";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenPixelsHeight"] = "80";
			capabilities["screenPixelsWidth"] = "101";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["supportsBodyClassAttribute"] = "true";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsNoWrapStyle"] = "true";
			capabilities["supportsSelectFollowingTable"] = "true";
			capabilities["supportsStyleElement"] = "true";
			capabilities["supportsTitleElement"] = "true";
			capabilities["supportsUrlAttributeEncoding"] = "true";
			capabilities["tables"] = "true";
			capabilities["type"] = regexWorker["${browser} ${majorVersion}"];
			browserCaps.AddBrowser("SCHE150");
			this.Sche150ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Sche150ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0006F0DC File Offset: 0x0006E0DC
		protected virtual void SktdevicesericssonProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0006F0DE File Offset: 0x0006E0DE
		protected virtual void SktdevicesericssonProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x0006F0E0 File Offset: 0x0006E0E0
		private bool SktdevicesericssonProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["browserType"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "01"))
			{
				return false;
			}
			capabilities["browser"] = "Ericsson";
			browserCaps.AddBrowser("SKTDevicesEricsson");
			this.SktdevicesericssonProcessGateways(headers, browserCaps);
			bool flag = false;
			this.SktdevicesericssonProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0006F14F File Offset: 0x0006E14F
		protected virtual void WebtvProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x0006F151 File Offset: 0x0006E151
		protected virtual void WebtvProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0006F154 File Offset: 0x0006E154
		private bool WebtvProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "WebTV/(?'version'(?'major'\\d+)(?'minor'\\.\\d+)(?'letters'\\w*))"))
			{
				return false;
			}
			capabilities["backgroundsounds"] = "true";
			capabilities["browser"] = "WebTV";
			capabilities["cookies"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["letters"] = regexWorker["${letters}"];
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["tables"] = "true";
			capabilities["type"] = regexWorker["WebTV${major}"];
			capabilities["version"] = regexWorker["${version}"];
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("WebTV");
			this.WebtvProcessGateways(headers, browserCaps);
			this.WebtvbetaProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Webtv2Process(headers, browserCaps))
			{
				flag = false;
			}
			this.WebtvProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x0006F28D File Offset: 0x0006E28D
		protected virtual void Webtv2ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x0006F28F File Offset: 0x0006E28F
		protected virtual void Webtv2ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0006F294 File Offset: 0x0006E294
		private bool Webtv2Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["minorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "2"))
			{
				return false;
			}
			capabilities["css1"] = "true";
			capabilities["ecmascriptversion"] = "1.0";
			capabilities["isMobileDevice"] = "false";
			capabilities["javascript"] = "true";
			capabilities["supportsBold"] = "false";
			capabilities["supportsCss"] = "false";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFontName"] = "false";
			capabilities["supportsFontSize"] = "false";
			capabilities["supportsImageSubmit"] = "false";
			capabilities["supportsItalic"] = "false";
			browserCaps.AddBrowser("WebTV2");
			this.Webtv2ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Webtv2ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0006F3A3 File Offset: 0x0006E3A3
		protected virtual void WebtvbetaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x0006F3A5 File Offset: 0x0006E3A5
		protected virtual void WebtvbetaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0006F3A8 File Offset: 0x0006E3A8
		private bool WebtvbetaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["letters"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^b"))
			{
				return false;
			}
			capabilities["beta"] = "true";
			this.WebtvbetaProcessGateways(headers, browserCaps);
			bool flag = false;
			this.WebtvbetaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x0006F40C File Offset: 0x0006E40C
		protected virtual void WinwapProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0006F40E File Offset: 0x0006E40E
		protected virtual void WinwapProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x0006F410 File Offset: 0x0006E410
		private bool WinwapProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^WinWAP-(?'platform'\\w*)/(?'browserMajorVersion'\\w*)(?'browserMinorVersion'\\.\\w*)"))
			{
				return false;
			}
			capabilities["browser"] = "WinWAP";
			capabilities["canRenderAfterInputOrSelectElement"] = "false";
			capabilities["canSendMail"] = "false";
			capabilities["cookies"] = "false";
			capabilities["hasBackButton"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "3500";
			capabilities["maximumSoftkeyLabelLength"] = "21";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["platform"] = regexWorker["${platform}"];
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml12";
			capabilities["rendersBreaksAfterWmlInput"] = "false";
			capabilities["rendersWmlSelectsAsMenuCards"] = "false";
			capabilities["requiresSpecialViewStateEncoding"] = "true";
			capabilities["requiresUniqueFilepathSuffix"] = "true";
			capabilities["screenBitDepth"] = "24";
			capabilities["screenCharactersHeight"] = "16";
			capabilities["screenCharactersWidth"] = "43";
			capabilities["screenPixelsHeight"] = "320";
			capabilities["screenPixelsWidth"] = "240";
			capabilities["supportsBold"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsRedirectWithCookie"] = "false";
			capabilities["type"] = regexWorker["WinWAP ${browserMajorVersion}${browserMinorVersion}"];
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("WinWap");
			this.WinwapProcessGateways(headers, browserCaps);
			bool flag = false;
			this.WinwapProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0006F68D File Offset: 0x0006E68D
		protected virtual void XiinoProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x0006F68F File Offset: 0x0006E68F
		protected virtual void XiinoProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x0006F694 File Offset: 0x0006E694
		private bool XiinoProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Xiino/(?'browserMajorVersion'\\d+)(?'browserMinorVersion'\\.\\d+).* (?'screenWidth'\\d+)x(?'screenHeight'\\d+);"))
			{
				return false;
			}
			capabilities["browser"] = "Xiino";
			capabilities["canRenderEmptySelects"] = "true";
			capabilities["canSendMail"] = "false";
			capabilities["hidesRightAlignedMultiselectScrollbars"] = "false";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "true";
			capabilities["majorVersion"] = regexWorker["${browserMajorVersion}"];
			capabilities["maximumRenderedPageSize"] = "65000";
			capabilities["minorVersion"] = regexWorker["${browserMinorVersion}"];
			capabilities["requiresAdaptiveErrorReporting"] = "true";
			capabilities["requiresAttributeColonSubstitution"] = "false";
			capabilities["screenBitDepth"] = "8";
			capabilities["screenCharactersHeight"] = "12";
			capabilities["screenCharactersWidth"] = "30";
			capabilities["screenPixelsHeight"] = regexWorker["${screenHeight}"];
			capabilities["screenPixelsWidth"] = regexWorker["${screenWidth}"];
			capabilities["supportsBold"] = "true";
			capabilities["supportsCharacterEntityEncoding"] = "false";
			capabilities["supportsFontSize"] = "true";
			capabilities["type"] = "Xiino";
			capabilities["version"] = regexWorker["${browserMajorVersion}${browserMinorVersion}"];
			browserCaps.HtmlTextWriter = "System.Web.UI.Html32TextWriter";
			browserCaps.AddBrowser("Xiino");
			this.XiinoProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Xiinov2Process(headers, browserCaps))
			{
				flag = false;
			}
			this.XiinoProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x0006F884 File Offset: 0x0006E884
		protected virtual void Xiinov2ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0006F886 File Offset: 0x0006E886
		protected virtual void Xiinov2ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0006F888 File Offset: 0x0006E888
		private bool Xiinov2Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["version"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^2\\.0$"))
			{
				return false;
			}
			capabilities["preferredImageMime"] = "image/jpeg";
			capabilities["requiresOutputOptimization"] = "true";
			capabilities["screenBitDepth"] = "16";
			capabilities["supportsItalic"] = "true";
			browserCaps.AddBrowser("XiinoV2");
			this.Xiinov2ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Xiinov2ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x0006F927 File Offset: 0x0006E927
		protected virtual void DefaultDefaultProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x0006F92C File Offset: 0x0006E92C
		private bool DefaultDefaultProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			capabilities["ecmascriptversion"] = "0.0";
			capabilities["javascript"] = "false";
			capabilities["jscriptversion"] = "0.0";
			bool flag = true;
			if (!this.DefaultWmlProcess(headers, browserCaps) && !this.DefaultXhtmlmpProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.DefaultDefaultProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x0006F992 File Offset: 0x0006E992
		protected virtual void DefaultWmlProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x0006F994 File Offset: 0x0006E994
		private bool DefaultWmlProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["Accept"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "text/vnd\\.wap\\.wml|text/hdml"))
			{
				return false;
			}
			text = headers["Accept"];
			bool flag = regexWorker.ProcessRegex(text, "application/xhtml\\+xml; profile|application/vnd\\.wap\\.xhtml\\+xml");
			if (flag)
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["preferredRenderingMime"] = "text/vnd.wap.wml";
			capabilities["preferredRenderingType"] = "wml11";
			bool flag2 = false;
			this.DefaultWmlProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x0006FA1F File Offset: 0x0006EA1F
		protected virtual void DefaultXhtmlmpProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x0006FA24 File Offset: 0x0006EA24
		private bool DefaultXhtmlmpProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["Accept"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "application/xhtml\\+xml; profile|application/vnd\\.wap\\.xhtml\\+xml"))
			{
				return false;
			}
			text = headers["Accept"];
			bool flag = regexWorker.ProcessRegex(text, "text/hdml");
			if (flag)
			{
				return false;
			}
			text = headers["Accept"];
			flag = regexWorker.ProcessRegex(text, "text/vnd\\.wap\\.wml");
			if (flag)
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["preferredRenderingMime"] = "text/html";
			capabilities["preferredRenderingType"] = "xhtml-mp";
			browserCaps.HtmlTextWriter = "System.Web.UI.XhtmlTextWriter";
			bool flag2 = false;
			this.DefaultXhtmlmpProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x0006FAD8 File Offset: 0x0006EAD8
		protected override void PopulateMatchedHeaders(IDictionary dictionary)
		{
			base.PopulateMatchedHeaders(dictionary);
			dictionary["X-UP-DEVCAP-NUMSOFTKEYS"] = null;
			dictionary["UA-COLOR"] = null;
			dictionary["UA-PIXELS"] = null;
			dictionary["HTTP_X_WAP_PROFILE"] = null;
			dictionary["X-UP-DEVCAP-SCREENDEPTH"] = null;
			dictionary["X-UP-DEVCAP-MSIZE"] = null;
			dictionary["X-UP-DEVCAP-MAX-PDU"] = null;
			dictionary["X-UP-DEVCAP-CHARSET"] = null;
			dictionary["X-GA-TABLES"] = null;
			dictionary["X-AVANTGO-VERSION"] = null;
			dictionary["X-UP-DEVCAP-SOFTKEYSIZE"] = null;
			dictionary["UA-VOICE"] = null;
			dictionary["X-UP-DEVCAP-ISCOLOR"] = null;
			dictionary["X-JPHONE-DISPLAY"] = null;
			dictionary["X-UP-DEVCAP-SCREENPIXELS"] = null;
			dictionary["X-JPHONE-COLOR"] = null;
			dictionary["X-UP-DEVCAP-SCREENCHARS"] = null;
			dictionary["Accept"] = null;
			dictionary[""] = null;
			dictionary["VIA"] = null;
			dictionary["X-GA-MAX-TRANSFER"] = null;
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x0006FBE8 File Offset: 0x0006EBE8
		protected override void PopulateBrowserElements(IDictionary dictionary)
		{
			base.PopulateBrowserElements(dictionary);
			dictionary["Default"] = new Triplet(null, string.Empty, 0);
			dictionary["Mozilla"] = new Triplet("Default", string.Empty, 1);
			dictionary["IE"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["IE5to9"] = new Triplet("Ie", string.Empty, 3);
			dictionary["IE6to9"] = new Triplet("Ie5to9", string.Empty, 4);
			dictionary["Treo600"] = new Triplet("Ie6to9", string.Empty, 5);
			dictionary["IE5"] = new Triplet("Ie5to9", string.Empty, 4);
			dictionary["IE50"] = new Triplet("Ie5", string.Empty, 5);
			dictionary["IE55"] = new Triplet("Ie5", string.Empty, 5);
			dictionary["IE5to9Mac"] = new Triplet("Ie5to9", string.Empty, 4);
			dictionary["IE4"] = new Triplet("Ie", string.Empty, 3);
			dictionary["IE3"] = new Triplet("Ie", string.Empty, 3);
			dictionary["IE3win16"] = new Triplet("Ie3", string.Empty, 4);
			dictionary["IE3win16a"] = new Triplet("Ie3win16", string.Empty, 5);
			dictionary["IE3Mac"] = new Triplet("Ie3", string.Empty, 4);
			dictionary["IE2"] = new Triplet("Ie", string.Empty, 3);
			dictionary["WebTV"] = new Triplet("Ie2", string.Empty, 4);
			dictionary["WebTV2"] = new Triplet("Webtv", string.Empty, 5);
			dictionary["IE1minor5"] = new Triplet("Ie", string.Empty, 3);
			dictionary["PowerBrowser"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["Gecko"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["MozillaRV"] = new Triplet("Gecko", string.Empty, 3);
			dictionary["MozillaFirebird"] = new Triplet("Mozillarv", string.Empty, 4);
			dictionary["MozillaFirefox"] = new Triplet("Mozillarv", string.Empty, 4);
			dictionary["Safari"] = new Triplet("Gecko", string.Empty, 3);
			dictionary["Safari60"] = new Triplet("Safari", string.Empty, 4);
			dictionary["Safari85"] = new Triplet("Safari", string.Empty, 4);
			dictionary["Safari1Plus"] = new Triplet("Safari", string.Empty, 4);
			dictionary["Netscape5"] = new Triplet("Gecko", string.Empty, 3);
			dictionary["Netscape6to9"] = new Triplet("Netscape5", string.Empty, 4);
			dictionary["Netscape6to9Beta"] = new Triplet("Netscape6to9", string.Empty, 5);
			dictionary["NetscapeBeta"] = new Triplet("Netscape5", string.Empty, 4);
			dictionary["AvantGo"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["TMobileSidekick"] = new Triplet("Avantgo", string.Empty, 3);
			dictionary["GoAmerica"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["GoAmericaWinCE"] = new Triplet("Goamerica", string.Empty, 3);
			dictionary["GoAmericaPalm"] = new Triplet("Goamerica", string.Empty, 3);
			dictionary["GoAmericaRIM"] = new Triplet("Goamerica", string.Empty, 3);
			dictionary["GoAmericaRIM950"] = new Triplet("Goamericarim", string.Empty, 4);
			dictionary["GoAmericaRIM850"] = new Triplet("Goamericarim", string.Empty, 4);
			dictionary["GoAmericaRIM957"] = new Triplet("Goamericarim", string.Empty, 4);
			dictionary["GoAmericaRIM957major6minor2"] = new Triplet("Goamericarim957", string.Empty, 5);
			dictionary["GoAmericaRIM857"] = new Triplet("Goamericarim", string.Empty, 4);
			dictionary["GoAmericaRIM857major6"] = new Triplet("Goamericarim857", string.Empty, 5);
			dictionary["GoAmericaRIM857major6minor2to9"] = new Triplet("Goamericarim857major6", string.Empty, 6);
			dictionary["GoAmerica7to9"] = new Triplet("Goamericarim857", string.Empty, 5);
			dictionary["Netscape3"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["Netscape4"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["Casiopeia"] = new Triplet("Netscape4", string.Empty, 3);
			dictionary["PalmWebPro"] = new Triplet("Netscape4", string.Empty, 3);
			dictionary["PalmWebPro3"] = new Triplet("Palmwebpro", string.Empty, 4);
			dictionary["NetFront"] = new Triplet("Netscape4", string.Empty, 3);
			dictionary["SLB500"] = new Triplet("Netfront", string.Empty, 4);
			dictionary["VRNA"] = new Triplet("Netfront", string.Empty, 4);
			dictionary["Mypalm"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["MyPalm1"] = new Triplet("Mypalm", string.Empty, 3);
			dictionary["Eudoraweb"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["PdQbrowser"] = new Triplet("Eudoraweb", string.Empty, 3);
			dictionary["Eudoraweb21Plus"] = new Triplet("Eudoraweb", string.Empty, 3);
			dictionary["WinCE"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["PIE"] = new Triplet("Wince", string.Empty, 3);
			dictionary["PIEPPC"] = new Triplet("Pie", string.Empty, 4);
			dictionary["PIEnoDeviceID"] = new Triplet("Pie", string.Empty, 4);
			dictionary["PIESmartphone"] = new Triplet("Pie", string.Empty, 4);
			dictionary["PIE4"] = new Triplet("Wince", string.Empty, 3);
			dictionary["PIE4PPC"] = new Triplet("Pie4", string.Empty, 4);
			dictionary["PIE5Plus"] = new Triplet("Wince", string.Empty, 3);
			dictionary["sigmarion3"] = new Triplet("Pie5plus", string.Empty, 4);
			dictionary["MSPIE"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["MSPIE2"] = new Triplet("Mspie", string.Empty, 3);
			dictionary["Docomo"] = new Triplet("Default", string.Empty, 1);
			dictionary["DocomoSH251i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoSH251iS"] = new Triplet("Docomosh251i", string.Empty, 3);
			dictionary["DocomoN251i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN251iS"] = new Triplet("Docomon251i", string.Empty, 3);
			dictionary["DocomoP211i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoF212i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoD501i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoF501i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN501i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP501i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoD502i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoF502i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN502i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP502i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoNm502i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoSo502i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoF502it"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN502it"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoSo502iwm"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoF504i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN504i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP504i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN821i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP821i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoD209i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoEr209i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoF209i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoKo209i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN209i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP209i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP209is"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoR209i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoR691i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoF503i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoF503is"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoD503i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoD503is"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoD210i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoF210i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN210i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN2001"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoD211i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN211i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP210i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoKo210i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP2101v"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP2102v"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoF211i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoF671i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN503is"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN503i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoSo503i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP503is"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP503i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoSo210i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoSo503is"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoSh821i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN2002"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoSo505i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoP505i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoN505i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoD505i"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["DocomoISIM60"] = new Triplet("Docomo", string.Empty, 2);
			dictionary["EricssonR380"] = new Triplet("Default", string.Empty, 1);
			dictionary["Ericsson"] = new Triplet("Default", string.Empty, 1);
			dictionary["EricssonR320"] = new Triplet("Ericsson", string.Empty, 2);
			dictionary["EricssonT20"] = new Triplet("Ericsson", string.Empty, 2);
			dictionary["EricssonT65"] = new Triplet("Ericsson", string.Empty, 2);
			dictionary["EricssonT68"] = new Triplet("Ericsson", string.Empty, 2);
			dictionary["Ericsson301A"] = new Triplet("Ericssont68", string.Empty, 3);
			dictionary["EricssonT68R1A"] = new Triplet("Ericssont68", string.Empty, 3);
			dictionary["EricssonT68R101"] = new Triplet("Ericssont68", string.Empty, 3);
			dictionary["EricssonT68R201A"] = new Triplet("Ericssont68", string.Empty, 3);
			dictionary["EricssonT300"] = new Triplet("Ericsson", string.Empty, 2);
			dictionary["EricssonP800"] = new Triplet("Ericsson", string.Empty, 2);
			dictionary["EricssonP800R101"] = new Triplet("Ericssonp800", string.Empty, 3);
			dictionary["EricssonT61"] = new Triplet("Ericsson", string.Empty, 2);
			dictionary["EricssonT31"] = new Triplet("Ericsson", string.Empty, 2);
			dictionary["EricssonR520"] = new Triplet("Ericsson", string.Empty, 2);
			dictionary["EricssonA2628"] = new Triplet("Ericsson", string.Empty, 2);
			dictionary["EricssonT39"] = new Triplet("Ericsson", string.Empty, 2);
			dictionary["EzWAP"] = new Triplet("Default", string.Empty, 1);
			dictionary["GenericDownlevel"] = new Triplet("Default", string.Empty, 1);
			dictionary["Jataayu"] = new Triplet("Default", string.Empty, 1);
			dictionary["JataayuPPC"] = new Triplet("Jataayu", string.Empty, 2);
			dictionary["Jphone"] = new Triplet("Default", string.Empty, 1);
			dictionary["JphoneMitsubishi"] = new Triplet("Jphone", string.Empty, 2);
			dictionary["JphoneDenso"] = new Triplet("Jphone", string.Empty, 2);
			dictionary["JphoneKenwood"] = new Triplet("Jphone", string.Empty, 2);
			dictionary["JphoneNec"] = new Triplet("Jphone", string.Empty, 2);
			dictionary["JphoneNecN51"] = new Triplet("Jphonenec", string.Empty, 3);
			dictionary["JphonePanasonic"] = new Triplet("Jphone", string.Empty, 2);
			dictionary["JphonePioneer"] = new Triplet("Jphone", string.Empty, 2);
			dictionary["JphoneSanyo"] = new Triplet("Jphone", string.Empty, 2);
			dictionary["JphoneSA51"] = new Triplet("Jphonesanyo", string.Empty, 3);
			dictionary["JphoneSharp"] = new Triplet("Jphone", string.Empty, 2);
			dictionary["JphoneSharpSh53"] = new Triplet("Jphonesharp", string.Empty, 3);
			dictionary["JphoneSharpSh07"] = new Triplet("Jphonesharp", string.Empty, 3);
			dictionary["JphoneSharpSh08"] = new Triplet("Jphonesharp", string.Empty, 3);
			dictionary["JphoneSharpSh51"] = new Triplet("Jphonesharp", string.Empty, 3);
			dictionary["JphoneSharpSh52"] = new Triplet("Jphonesharp", string.Empty, 3);
			dictionary["JphoneToshiba"] = new Triplet("Jphone", string.Empty, 2);
			dictionary["JphoneToshibaT06a"] = new Triplet("Jphonetoshiba", string.Empty, 3);
			dictionary["JphoneToshibaT08"] = new Triplet("Jphonetoshiba", string.Empty, 3);
			dictionary["JphoneToshibaT51"] = new Triplet("Jphonetoshiba", string.Empty, 3);
			dictionary["Legend"] = new Triplet("Default", string.Empty, 1);
			dictionary["LGG5200"] = new Triplet("Legend", string.Empty, 2);
			dictionary["MME"] = new Triplet("Default", string.Empty, 1);
			dictionary["MMEF20"] = new Triplet("Mme", string.Empty, 2);
			dictionary["MMECellphone"] = new Triplet("Mmef20", string.Empty, 3);
			dictionary["MMEBenefonQ"] = new Triplet("Mmecellphone", string.Empty, 4);
			dictionary["MMESonyCMDZ5"] = new Triplet("Mmecellphone", string.Empty, 4);
			dictionary["MMESonyCMDZ5Pj020e"] = new Triplet("Mmesonycmdz5", string.Empty, 5);
			dictionary["MMESonyCMDJ5"] = new Triplet("Mmecellphone", string.Empty, 4);
			dictionary["MMESonyCMDJ7"] = new Triplet("Mmecellphone", string.Empty, 4);
			dictionary["MMEGenericSmall"] = new Triplet("Mmecellphone", string.Empty, 4);
			dictionary["MMEGenericLarge"] = new Triplet("Mmecellphone", string.Empty, 4);
			dictionary["MMEGenericFlip"] = new Triplet("Mmecellphone", string.Empty, 4);
			dictionary["MMEGeneric3D"] = new Triplet("Mmecellphone", string.Empty, 4);
			dictionary["MMEMobileExplorer"] = new Triplet("Mme", string.Empty, 2);
			dictionary["Nokia"] = new Triplet("Default", string.Empty, 1);
			dictionary["NokiaBlueprint"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["NokiaWapSimulator"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["NokiaMobileBrowser"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia7110"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia6220"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia6250"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia6310"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia6510"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia8310"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia9110i"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia9110"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia3330"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia9210"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia9210HTML"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia3590"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia3590V1"] = new Triplet("Nokia3590", string.Empty, 3);
			dictionary["Nokia3595"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia3560"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia3650"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia3650P12Plus"] = new Triplet("Nokia3650", string.Empty, 3);
			dictionary["Nokia5100"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia6200"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia6590"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia6800"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["Nokia7650"] = new Triplet("Nokia", string.Empty, 2);
			dictionary["NokiaMobileBrowserRainbow"] = new Triplet("Default", string.Empty, 1);
			dictionary["NokiaEpoc32wtl"] = new Triplet("Default", string.Empty, 1);
			dictionary["NokiaEpoc32wtl20"] = new Triplet("Nokiaepoc32wtl", string.Empty, 2);
			dictionary["Up"] = new Triplet("Default", string.Empty, 1);
			dictionary["AuMic"] = new Triplet("Up", string.Empty, 2);
			dictionary["AuMicV2"] = new Triplet("Aumic", string.Empty, 3);
			dictionary["a500"] = new Triplet("Aumic", string.Empty, 3);
			dictionary["n400"] = new Triplet("Aumic", string.Empty, 3);
			dictionary["AlcatelBe4"] = new Triplet("Up", string.Empty, 2);
			dictionary["AlcatelBe5"] = new Triplet("Up", string.Empty, 2);
			dictionary["AlcatelBe5v2"] = new Triplet("Alcatelbe5", string.Empty, 3);
			dictionary["AlcatelBe3"] = new Triplet("Up", string.Empty, 2);
			dictionary["AlcatelBf3"] = new Triplet("Up", string.Empty, 2);
			dictionary["AlcatelBf4"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotCb"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotF5"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotD8"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotCf"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotF6"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotBc"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotDc"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotPanC"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotC4"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mcca"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mot2000"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotP2kC"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotAf"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotAf418"] = new Triplet("Motaf", string.Empty, 3);
			dictionary["MotC2"] = new Triplet("Up", string.Empty, 2);
			dictionary["Xenium"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sagem959"] = new Triplet("Up", string.Empty, 2);
			dictionary["SghA300"] = new Triplet("Up", string.Empty, 2);
			dictionary["SghN100"] = new Triplet("Up", string.Empty, 2);
			dictionary["C304sa"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sy11"] = new Triplet("Up", string.Empty, 2);
			dictionary["St12"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sy14"] = new Triplet("Up", string.Empty, 2);
			dictionary["SieS40"] = new Triplet("Up", string.Empty, 2);
			dictionary["SieSl45"] = new Triplet("Up", string.Empty, 2);
			dictionary["SieS35"] = new Triplet("Up", string.Empty, 2);
			dictionary["SieMe45"] = new Triplet("Up", string.Empty, 2);
			dictionary["SieS45"] = new Triplet("Up", string.Empty, 2);
			dictionary["Gm832"] = new Triplet("Up", string.Empty, 2);
			dictionary["Gm910i"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mot32"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mot28"] = new Triplet("Up", string.Empty, 2);
			dictionary["D2"] = new Triplet("Up", string.Empty, 2);
			dictionary["PPat"] = new Triplet("Up", string.Empty, 2);
			dictionary["Alaz"] = new Triplet("Up", string.Empty, 2);
			dictionary["Cdm9100"] = new Triplet("Up", string.Empty, 2);
			dictionary["Cdm135"] = new Triplet("Up", string.Empty, 2);
			dictionary["Cdm9000"] = new Triplet("Up", string.Empty, 2);
			dictionary["C303ca"] = new Triplet("Up", string.Empty, 2);
			dictionary["C311ca"] = new Triplet("Up", string.Empty, 2);
			dictionary["C202de"] = new Triplet("Up", string.Empty, 2);
			dictionary["C409ca"] = new Triplet("Up", string.Empty, 2);
			dictionary["C402de"] = new Triplet("Up", string.Empty, 2);
			dictionary["Ds15"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tp2200"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tp120"] = new Triplet("Up", string.Empty, 2);
			dictionary["Ds10"] = new Triplet("Up", string.Empty, 2);
			dictionary["R280"] = new Triplet("Up", string.Empty, 2);
			dictionary["C201h"] = new Triplet("Up", string.Empty, 2);
			dictionary["S71"] = new Triplet("Up", string.Empty, 2);
			dictionary["C302h"] = new Triplet("Up", string.Empty, 2);
			dictionary["C309h"] = new Triplet("Up", string.Empty, 2);
			dictionary["C407h"] = new Triplet("Up", string.Empty, 2);
			dictionary["C451h"] = new Triplet("Up", string.Empty, 2);
			dictionary["R201"] = new Triplet("Up", string.Empty, 2);
			dictionary["P21"] = new Triplet("Up", string.Empty, 2);
			dictionary["Kyocera702g"] = new Triplet("Up", string.Empty, 2);
			dictionary["Kyocera703g"] = new Triplet("Up", string.Empty, 2);
			dictionary["KyoceraC307k"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tk01"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tk02"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tk03"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tk04"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tk05"] = new Triplet("Up", string.Empty, 2);
			dictionary["D303k"] = new Triplet("Up", string.Empty, 2);
			dictionary["D304k"] = new Triplet("Up", string.Empty, 2);
			dictionary["Qcp2035"] = new Triplet("Up", string.Empty, 2);
			dictionary["Qcp3035"] = new Triplet("Up", string.Empty, 2);
			dictionary["D512"] = new Triplet("Up", string.Empty, 2);
			dictionary["Dm110"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tm510"] = new Triplet("Up", string.Empty, 2);
			dictionary["Lg13"] = new Triplet("Up", string.Empty, 2);
			dictionary["P100"] = new Triplet("Up", string.Empty, 2);
			dictionary["Lgc875f"] = new Triplet("Up", string.Empty, 2);
			dictionary["Lgp680f"] = new Triplet("Up", string.Empty, 2);
			dictionary["Lgp7800f"] = new Triplet("Up", string.Empty, 2);
			dictionary["Lgc840f"] = new Triplet("Up", string.Empty, 2);
			dictionary["Lgi2100"] = new Triplet("Up", string.Empty, 2);
			dictionary["Lgp7300f"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sd500"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tp1100"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tp3000"] = new Triplet("Up", string.Empty, 2);
			dictionary["T250"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mo01"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mo02"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mc01"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mccc"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mcc9"] = new Triplet("Up", string.Empty, 2);
			dictionary["Nk00"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mai12"] = new Triplet("Up", string.Empty, 2);
			dictionary["Ma112"] = new Triplet("Up", string.Empty, 2);
			dictionary["Ma13"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mac1"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mat1"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sc01"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sc03"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sc02"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sc04"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sg08"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sc13"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sc11"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sec01"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sc10"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sy12"] = new Triplet("Up", string.Empty, 2);
			dictionary["St11"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sy13"] = new Triplet("Up", string.Empty, 2);
			dictionary["Syc1"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sy01"] = new Triplet("Up", string.Empty, 2);
			dictionary["Syt1"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sty2"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sy02"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sy03"] = new Triplet("Up", string.Empty, 2);
			dictionary["Si01"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sni1"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sn11"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sn12"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sn134"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sn156"] = new Triplet("Up", string.Empty, 2);
			dictionary["Snc1"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tsc1"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tsi1"] = new Triplet("Up", string.Empty, 2);
			dictionary["Ts11"] = new Triplet("Up", string.Empty, 2);
			dictionary["Ts12"] = new Triplet("Up", string.Empty, 2);
			dictionary["Ts13"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tst1"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tst2"] = new Triplet("Up", string.Empty, 2);
			dictionary["Tst3"] = new Triplet("Up", string.Empty, 2);
			dictionary["Ig01"] = new Triplet("Up", string.Empty, 2);
			dictionary["Ig02"] = new Triplet("Up", string.Empty, 2);
			dictionary["Ig03"] = new Triplet("Up", string.Empty, 2);
			dictionary["Qc31"] = new Triplet("Up", string.Empty, 2);
			dictionary["Qc12"] = new Triplet("Up", string.Empty, 2);
			dictionary["Qc32"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sp01"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sh"] = new Triplet("Up", string.Empty, 2);
			dictionary["Upg1"] = new Triplet("Up", string.Empty, 2);
			dictionary["Opwv1"] = new Triplet("Up", string.Empty, 2);
			dictionary["Alav"] = new Triplet("Up", string.Empty, 2);
			dictionary["Im1k"] = new Triplet("Up", string.Empty, 2);
			dictionary["Nt95"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mot2001"] = new Triplet("Up", string.Empty, 2);
			dictionary["Motv200"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mot72"] = new Triplet("Up", string.Empty, 2);
			dictionary["Mot76"] = new Triplet("Up", string.Empty, 2);
			dictionary["Scp6000"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotD5"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotF0"] = new Triplet("Up", string.Empty, 2);
			dictionary["SghA400"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sec03"] = new Triplet("Up", string.Empty, 2);
			dictionary["SieC3i"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sn17"] = new Triplet("Up", string.Empty, 2);
			dictionary["Scp4700"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sec02"] = new Triplet("Up", string.Empty, 2);
			dictionary["Sy15"] = new Triplet("Up", string.Empty, 2);
			dictionary["Db520"] = new Triplet("Up", string.Empty, 2);
			dictionary["L430V03J02"] = new Triplet("Up", string.Empty, 2);
			dictionary["OPWVSDK"] = new Triplet("Up", string.Empty, 2);
			dictionary["OPWVSDK6"] = new Triplet("Opwvsdk", string.Empty, 3);
			dictionary["OPWVSDK6Plus"] = new Triplet("Opwvsdk", string.Empty, 3);
			dictionary["KDDICA21"] = new Triplet("Up", string.Empty, 2);
			dictionary["KDDITS21"] = new Triplet("Up", string.Empty, 2);
			dictionary["KDDISA21"] = new Triplet("Up", string.Empty, 2);
			dictionary["KM100"] = new Triplet("Up", string.Empty, 2);
			dictionary["LGELX5350"] = new Triplet("Up", string.Empty, 2);
			dictionary["HitachiP300"] = new Triplet("Up", string.Empty, 2);
			dictionary["SIES46"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotorolaV60G"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotorolaV708"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotorolaV708A"] = new Triplet("Up", string.Empty, 2);
			dictionary["MotorolaE360"] = new Triplet("Up", string.Empty, 2);
			dictionary["SonyericssonA1101S"] = new Triplet("Up", string.Empty, 2);
			dictionary["PhilipsFisio820"] = new Triplet("Up", string.Empty, 2);
			dictionary["CasioA5302"] = new Triplet("Up", string.Empty, 2);
			dictionary["TCLL668"] = new Triplet("Up", string.Empty, 2);
			dictionary["KDDITS24"] = new Triplet("Up", string.Empty, 2);
			dictionary["SIES55"] = new Triplet("Up", string.Empty, 2);
			dictionary["SHARPGx10"] = new Triplet("Up", string.Empty, 2);
			dictionary["BenQAthena"] = new Triplet("Up", string.Empty, 2);
			dictionary["Opera"] = new Triplet("Default", string.Empty, 1);
			dictionary["Opera1to3beta"] = new Triplet("Opera", string.Empty, 2);
			dictionary["Opera4"] = new Triplet("Opera", string.Empty, 2);
			dictionary["Opera4beta"] = new Triplet("Opera4", string.Empty, 3);
			dictionary["Opera5to9"] = new Triplet("Opera", string.Empty, 2);
			dictionary["Opera6to9"] = new Triplet("Opera5to9", string.Empty, 3);
			dictionary["Opera7to9"] = new Triplet("Opera6to9", string.Empty, 4);
			dictionary["Opera8to9"] = new Triplet("Opera7to9", string.Empty, 5);
			dictionary["OperaPsion"] = new Triplet("Opera", string.Empty, 2);
			dictionary["Palmscape"] = new Triplet("Default", string.Empty, 1);
			dictionary["PalmscapeVersion"] = new Triplet("Palmscape", string.Empty, 2);
			dictionary["AusPalm"] = new Triplet("Default", string.Empty, 1);
			dictionary["SharpPda"] = new Triplet("Default", string.Empty, 1);
			dictionary["ZaurusMiE1"] = new Triplet("Sharppda", string.Empty, 2);
			dictionary["ZaurusMiE21"] = new Triplet("Sharppda", string.Empty, 2);
			dictionary["ZaurusMiE25"] = new Triplet("Sharppda", string.Empty, 2);
			dictionary["Panasonic"] = new Triplet("Default", string.Empty, 1);
			dictionary["PanasonicGAD95"] = new Triplet("Panasonic", string.Empty, 2);
			dictionary["PanasonicGAD87"] = new Triplet("Panasonic", string.Empty, 2);
			dictionary["PanasonicGAD87A39"] = new Triplet("Panasonicgad87", string.Empty, 3);
			dictionary["PanasonicGAD87A38"] = new Triplet("Panasonicgad87", string.Empty, 3);
			dictionary["MSPIE06"] = new Triplet("Default", string.Empty, 1);
			dictionary["SKTDevices"] = new Triplet("Default", string.Empty, 1);
			dictionary["SKTDevicesHyundai"] = new Triplet("Sktdevices", string.Empty, 2);
			dictionary["PSE200"] = new Triplet("Sktdeviceshyundai", string.Empty, 3);
			dictionary["SKTDevicesHanhwa"] = new Triplet("Sktdevices", string.Empty, 2);
			dictionary["SKTDevicesJTEL"] = new Triplet("Sktdevices", string.Empty, 2);
			dictionary["JTEL01"] = new Triplet("Sktdevicesjtel", string.Empty, 3);
			dictionary["JTELNate"] = new Triplet("Jtel01", string.Empty, 4);
			dictionary["SKTDevicesLG"] = new Triplet("Sktdevices", string.Empty, 2);
			dictionary["SKTDevicesMotorola"] = new Triplet("Sktdevices", string.Empty, 2);
			dictionary["SKTDevicesV730"] = new Triplet("Sktdevicesmotorola", string.Empty, 3);
			dictionary["SKTDevicesNokia"] = new Triplet("Sktdevices", string.Empty, 2);
			dictionary["SKTDevicesSKTT"] = new Triplet("Sktdevices", string.Empty, 2);
			dictionary["SKTDevicesSamSung"] = new Triplet("Sktdevices", string.Empty, 2);
			dictionary["SCHE150"] = new Triplet("Sktdevicessamsung", string.Empty, 3);
			dictionary["SKTDevicesEricsson"] = new Triplet("Sktdevices", string.Empty, 2);
			dictionary["WinWap"] = new Triplet("Default", string.Empty, 1);
			dictionary["Xiino"] = new Triplet("Default", string.Empty, 1);
			dictionary["XiinoV2"] = new Triplet("Xiino", string.Empty, 2);
		}
	}
}
