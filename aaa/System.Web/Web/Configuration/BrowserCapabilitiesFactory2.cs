using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;

namespace System.Web.Configuration
{
	// Token: 0x020001A8 RID: 424
	internal class BrowserCapabilitiesFactory2 : BrowserCapabilitiesFactoryBase
	{
		// Token: 0x06001802 RID: 6146 RVA: 0x00073420 File Offset: 0x00072420
		public override void ConfigureBrowserCapabilities(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			this.DefaultProcess(headers, browserCaps);
			if (!base.IsBrowserUnknown(browserCaps))
			{
				return;
			}
			this.DefaultDefaultProcess(headers, browserCaps);
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0007343E File Offset: 0x0007243E
		protected virtual void IeProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00073440 File Offset: 0x00072440
		protected virtual void IeProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00073444 File Offset: 0x00072444
		private bool IeProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "MSIE (?'version'(?'major'\\d+)(\\.(?'minor'\\d+)?)(?'letters'\\w*))(?'extra'[^)]*)"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			bool flag = regexWorker.ProcessRegex(text, "IEMobile");
			if (flag)
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "Trident/(?'layoutVersion'\\d+)");
			capabilities["browser"] = "IE";
			capabilities["layoutEngine"] = "Trident";
			capabilities["layoutEngineVersion"] = regexWorker["${layoutVersion}"];
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
			this.IebetaProcess(headers, browserCaps);
			bool flag2 = true;
			if (!this.Ie6plusProcess(headers, browserCaps))
			{
				flag2 = false;
			}
			this.IeProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x000735C3 File Offset: 0x000725C3
		protected virtual void Ie6plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x000735C5 File Offset: 0x000725C5
		protected virtual void Ie6plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x000735C8 File Offset: 0x000725C8
		private bool Ie6plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^[6-9]|\\d{2,}$"))
			{
				return false;
			}
			capabilities["jscriptversion"] = "5.6";
			capabilities["msdomversion"] = regexWorker["${majorversion}.${minorversion}"];
			capabilities["ExchangeOmaSupported"] = "true";
			capabilities["activexcontrols"] = "true";
			capabilities["backgroundsounds"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["supportsVCard"] = "true";
			capabilities["supportsAccessKeyAttribute"] = "true";
			capabilities["vbscript"] = "true";
			browserCaps.AddBrowser("IE6Plus");
			this.Ie6plusProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Ie6to9Process(headers, browserCaps) && !this.Ie10plusProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.Ie6plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x000736D4 File Offset: 0x000726D4
		protected virtual void Ie6to9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x000736D6 File Offset: 0x000726D6
		protected virtual void Ie6to9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x000736D8 File Offset: 0x000726D8
		private bool Ie6to9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^[6-9]$"))
			{
				return false;
			}
			browserCaps.AddBrowser("IE6to9");
			this.Ie6to9ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Ie7Process(headers, browserCaps) && !this.Ie8Process(headers, browserCaps) && !this.Ie9Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Ie6to9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x00073758 File Offset: 0x00072758
		protected virtual void Ie7ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0007375A File Offset: 0x0007275A
		protected virtual void Ie7ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0007375C File Offset: 0x0007275C
		private bool Ie7Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^7$"))
			{
				return false;
			}
			capabilities["jscriptversion"] = "5.7";
			browserCaps.AddBrowser("IE7");
			this.Ie7ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie7ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x000737CB File Offset: 0x000727CB
		protected virtual void Ie8ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x000737CD File Offset: 0x000727CD
		protected virtual void Ie8ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x000737D0 File Offset: 0x000727D0
		private bool Ie8Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^8$"))
			{
				return false;
			}
			capabilities["jscriptversion"] = "6.0";
			browserCaps.AddBrowser("IE8");
			this.Ie8ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie8ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x0007383F File Offset: 0x0007283F
		protected virtual void Ie9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00073841 File Offset: 0x00072841
		protected virtual void Ie9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00073844 File Offset: 0x00072844
		private bool Ie9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^9$"))
			{
				return false;
			}
			capabilities["jscriptversion"] = "6.0";
			browserCaps.AddBrowser("IE9");
			this.Ie9ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie9ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x000738B3 File Offset: 0x000728B3
		protected virtual void Ie10plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x000738B5 File Offset: 0x000728B5
		protected virtual void Ie10plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x000738B8 File Offset: 0x000728B8
		private bool Ie10plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "\\d{2,}"))
			{
				return false;
			}
			capabilities["jscriptversion"] = "6.0";
			browserCaps.AddBrowser("IE10Plus");
			this.Ie10plusProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Ie10plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x00073927 File Offset: 0x00072927
		protected virtual void IebetaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x00073929 File Offset: 0x00072929
		protected virtual void IebetaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0007392C File Offset: 0x0007292C
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

		// Token: 0x0600181B RID: 6171 RVA: 0x00073990 File Offset: 0x00072990
		protected virtual void InternetexplorerProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x00073992 File Offset: 0x00072992
		protected virtual void InternetexplorerProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x00073994 File Offset: 0x00072994
		private bool InternetexplorerProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Trident/(?'layoutVersion'[7-9]|0*[1-9]\\d+)(\\.\\d+)?;(.*;)?\\s*rv:(?'version'(?'major'\\d+)(\\.(?'minor'\\d+)))"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			bool flag = regexWorker.ProcessRegex(text, "IEMobile");
			if (flag)
			{
				return false;
			}
			flag = regexWorker.ProcessRegex(text, "MSIE ");
			if (flag)
			{
				return false;
			}
			capabilities["browser"] = "InternetExplorer";
			capabilities["version"] = regexWorker["${version}"];
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["layoutEngine"] = "Trident";
			capabilities["layoutEngineVersion"] = regexWorker["${layoutVersion}"];
			capabilities["type"] = regexWorker["InternetExplorer${major}"];
			browserCaps.AddBrowser("InternetExplorer");
			this.InternetexplorerProcessGateways(headers, browserCaps);
			bool flag2 = false;
			this.InternetexplorerProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x00073AAC File Offset: 0x00072AAC
		protected virtual void BlackberryProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x00073AAE File Offset: 0x00072AAE
		protected virtual void BlackberryProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x00073AB0 File Offset: 0x00072AB0
		private bool BlackberryProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "BlackBerry(?'deviceName'\\w+)/(?'version'(?'major'\\d+)(\\.(?'minor'\\d+)?)\\w*)"))
			{
				return false;
			}
			capabilities["layoutEngine"] = "BlackBerry";
			capabilities["browser"] = "BlackBerry";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["type"] = regexWorker["BlackBerry${major}"];
			capabilities["mobileDeviceModel"] = regexWorker["${deviceName}"];
			capabilities["isMobileDevice"] = "true";
			capabilities["version"] = regexWorker["${version}"];
			capabilities["ecmascriptversion"] = "3.0";
			capabilities["javascript"] = "true";
			capabilities["javascriptversion"] = "1.3";
			capabilities["w3cdomversion"] = "1.0";
			capabilities["supportsAccesskeyAttribute"] = "true";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["cookies"] = "true";
			capabilities["frames"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["supportsCallback"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["supportsXmlHttp"] = "true";
			capabilities["tables"] = "true";
			capabilities["canInitiateVoiceCall"] = "true";
			browserCaps.AddBrowser("BlackBerry");
			this.BlackberryProcessGateways(headers, browserCaps);
			bool flag = false;
			this.BlackberryProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x00073CA8 File Offset: 0x00072CA8
		protected virtual void OperaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x00073CAA File Offset: 0x00072CAA
		protected virtual void OperaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x00073CAC File Offset: 0x00072CAC
		private bool OperaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Opera[ /](?'version'(?'major'\\d+)(\\.(?'minor'\\d+)?)(?'letters'\\w*))"))
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "Presto/(?'layoutVersion'\\d+)");
			capabilities["browser"] = "Opera";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["type"] = regexWorker["Opera${major}"];
			capabilities["version"] = regexWorker["${version}"];
			capabilities["layoutEngine"] = "Presto";
			capabilities["layoutEngineVersion"] = regexWorker["${layoutVersion}"];
			capabilities["ecmascriptversion"] = "3.0";
			capabilities["javascript"] = "true";
			capabilities["javascriptversion"] = "1.5";
			capabilities["letters"] = regexWorker["${letters}"];
			capabilities["w3cdomversion"] = "1.0";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["cookies"] = "true";
			capabilities["frames"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["supportsAccesskeyAttribute"] = "true";
			capabilities["supportsCallback"] = "true";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["supportsXmlHttp"] = "true";
			capabilities["tables"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "false";
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["screenBitDepth"] = "8";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsDivNoWrap"] = "true";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsItalic"] = "true";
			browserCaps.AddBrowser("Opera");
			this.OperaProcessGateways(headers, browserCaps);
			this.OperaminiProcess(headers, browserCaps);
			this.OperamobileProcess(headers, browserCaps);
			bool flag = true;
			if (!this.Opera8plusProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.OperaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x00073F80 File Offset: 0x00072F80
		protected virtual void OperaminiProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x00073F82 File Offset: 0x00072F82
		protected virtual void OperaminiProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x00073F84 File Offset: 0x00072F84
		private bool OperaminiProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Opera Mini"))
			{
				return false;
			}
			capabilities["isMobileDevice"] = "true";
			this.OperaminiProcessGateways(headers, browserCaps);
			bool flag = false;
			this.OperaminiProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x00073FE3 File Offset: 0x00072FE3
		protected virtual void OperamobileProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x00073FE5 File Offset: 0x00072FE5
		protected virtual void OperamobileProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x00073FE8 File Offset: 0x00072FE8
		private bool OperamobileProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Opera Mobi"))
			{
				return false;
			}
			capabilities["isMobileDevice"] = "true";
			this.OperamobileProcessGateways(headers, browserCaps);
			bool flag = false;
			this.OperamobileProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x00074047 File Offset: 0x00073047
		protected virtual void Opera8plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x00074049 File Offset: 0x00073049
		protected virtual void Opera8plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0007404C File Offset: 0x0007304C
		private bool Opera8plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^[8-9]|\\d{2,}$"))
			{
				return false;
			}
			capabilities["supportsMaintainScrollPositionOnPostback"] = "true";
			browserCaps.AddBrowser("Opera8Plus");
			this.Opera8plusProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Opera8to9Process(headers, browserCaps) && !this.Opera10Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Opera8plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x000740D2 File Offset: 0x000730D2
		protected virtual void Opera8to9ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x000740D4 File Offset: 0x000730D4
		protected virtual void Opera8to9ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x000740D8 File Offset: 0x000730D8
		private bool Opera8to9Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^[8-9]$"))
			{
				return false;
			}
			text = (string)capabilities["Version"];
			bool flag = regexWorker.ProcessRegex(text, "^9.80$");
			if (flag)
			{
				return false;
			}
			browserCaps.AddBrowser("Opera8to9");
			this.Opera8to9ProcessGateways(headers, browserCaps);
			bool flag2 = false;
			this.Opera8to9ProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0007415A File Offset: 0x0007315A
		protected virtual void Opera10ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0007415C File Offset: 0x0007315C
		protected virtual void Opera10ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x00074160 File Offset: 0x00073160
		private bool Opera10Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Opera/10\\.|Version/10\\."))
			{
				return false;
			}
			capabilities["version"] = "10.00";
			capabilities["majorversion"] = "10";
			capabilities["minorversion"] = "00";
			browserCaps.AddBrowser("Opera10");
			this.Opera10ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Opera10ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x000741EA File Offset: 0x000731EA
		protected virtual void ChromeProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x000741EC File Offset: 0x000731EC
		protected virtual void ChromeProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x000741F0 File Offset: 0x000731F0
		private bool ChromeProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Chrome/(?'version'(?'major'\\d+)(\\.(?'minor'\\d+)?)\\w*)"))
			{
				return false;
			}
			capabilities["browser"] = "Chrome";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["type"] = regexWorker["Chrome${major}"];
			capabilities["version"] = regexWorker["${version}"];
			capabilities["ecmascriptversion"] = "3.0";
			capabilities["javascript"] = "true";
			capabilities["javascriptversion"] = "1.7";
			capabilities["w3cdomversion"] = "1.0";
			capabilities["supportsAccesskeyAttribute"] = "true";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["cookies"] = "true";
			capabilities["frames"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["supportsCallback"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsMaintainScrollPositionOnPostback"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["supportsXmlHttp"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("Chrome");
			this.ChromeProcessGateways(headers, browserCaps);
			bool flag = false;
			this.ChromeProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x000743B2 File Offset: 0x000733B2
		protected virtual void DefaultProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x000743B4 File Offset: 0x000733B4
		protected virtual void DefaultProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x000743B8 File Offset: 0x000733B8
		private bool DefaultProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			capabilities["activexcontrols"] = "false";
			capabilities["aol"] = "false";
			capabilities["backgroundsounds"] = "false";
			capabilities["beta"] = "false";
			capabilities["browser"] = "Unknown";
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
			capabilities["defaultSubmitButtonLimit"] = "1";
			capabilities["ecmascriptversion"] = "0.0";
			capabilities["frames"] = "false";
			capabilities["gatewayMajorVersion"] = "0";
			capabilities["gatewayMinorVersion"] = "0";
			capabilities["gatewayVersion"] = "None";
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
			capabilities["requiresAbsolutePostbackUrl"] = "false";
			capabilities["requiresAdaptiveErrorReporting"] = "false";
			capabilities["requiresAttributeColonSubstitution"] = "false";
			capabilities["requiresContentTypeMetaTag"] = "false";
			capabilities["requiresControlStateInSession"] = "false";
			capabilities["requiresDBCSCharacter"] = "false";
			capabilities["requiresFullyQualifiedRedirectUrl"] = "false";
			capabilities["requiresLeadingPageBreak"] = "false";
			capabilities["requiresNoBreakInFormatting"] = "false";
			capabilities["requiresOutputOptimization"] = "false";
			capabilities["requiresPhoneNumbersAsPlainText"] = "false";
			capabilities["requiresPostRedirectionHandling"] = "false";
			capabilities["requiresSpecialViewStateEncoding"] = "false";
			capabilities["requiresUniqueFilePathSuffix"] = "false";
			capabilities["requiresUniqueHtmlCheckboxNames"] = "false";
			capabilities["requiresUniqueHtmlInputNames"] = "false";
			capabilities["requiresUrlEncodedPostfieldValues"] = "false";
			capabilities["requiresXhtmlCssSuppression"] = "false";
			capabilities["screenBitDepth"] = "1";
			capabilities["supportsAccesskeyAttribute"] = "false";
			capabilities["supportsBodyColor"] = "true";
			capabilities["supportsBold"] = "false";
			capabilities["supportsCallback"] = "false";
			capabilities["supportsCacheControlMetaTag"] = "true";
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
			capabilities["SupportsMaintainScrollPositionOnPostback"] = "false";
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
			browserCaps.AddBrowser("Default");
			this.DefaultProcessGateways(headers, browserCaps);
			this.CrawlerProcess(headers, browserCaps);
			this.PlatformProcess(headers, browserCaps);
			this.WinProcess(headers, browserCaps);
			bool flag = true;
			if (!this.BlackberryProcess(headers, browserCaps) && !this.OperaProcess(headers, browserCaps) && !this.GenericdownlevelProcess(headers, browserCaps) && !this.MozillaProcess(headers, browserCaps) && !this.UcbrowserProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.DefaultProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x00074ADA File Offset: 0x00073ADA
		protected virtual void FirefoxProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x00074ADC File Offset: 0x00073ADC
		protected virtual void FirefoxProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00074AE0 File Offset: 0x00073AE0
		private bool FirefoxProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Firefox\\/(?'version'(?'major'\\d+)(\\.(?'minor'\\d+)?)\\w*)"))
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "Gecko/(?'layoutVersion'\\d+)");
			capabilities["browser"] = "Firefox";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["version"] = regexWorker["${version}"];
			capabilities["type"] = regexWorker["Firefox${major}"];
			capabilities["layoutEngine"] = "Gecko";
			capabilities["layoutEngineVersion"] = regexWorker["${layoutVersion}"];
			capabilities["supportsAccesskeyAttribute"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			browserCaps.AddBrowser("Firefox");
			this.FirefoxProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Firefox3plusProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.FirefoxProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x00074C1C File Offset: 0x00073C1C
		protected virtual void Firefox3plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x00074C1E File Offset: 0x00073C1E
		protected virtual void Firefox3plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x00074C20 File Offset: 0x00073C20
		private bool Firefox3plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "[3-9]|\\d{2,}"))
			{
				return false;
			}
			capabilities["javascriptversion"] = "1.8";
			browserCaps.AddBrowser("Firefox3Plus");
			this.Firefox3plusProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Firefox3Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Firefox3plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x00074C9C File Offset: 0x00073C9C
		protected virtual void Firefox3ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x00074C9E File Offset: 0x00073C9E
		protected virtual void Firefox3ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00074CA0 File Offset: 0x00073CA0
		private bool Firefox3Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^3$"))
			{
				return false;
			}
			browserCaps.AddBrowser("Firefox3");
			this.Firefox3ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Firefox35Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Firefox3ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x00074D0C File Offset: 0x00073D0C
		protected virtual void Firefox35ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x00074D0E File Offset: 0x00073D0E
		protected virtual void Firefox35ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x00074D10 File Offset: 0x00073D10
		private bool Firefox35Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["minorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^[5-9]"))
			{
				return false;
			}
			browserCaps.AddBrowser("Firefox35");
			this.Firefox35ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Firefox35ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x00074D6F File Offset: 0x00073D6F
		protected virtual void CrawlerProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x00074D71 File Offset: 0x00073D71
		protected virtual void CrawlerProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x00074D74 File Offset: 0x00073D74
		private bool CrawlerProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "crawler|Crawler|Googlebot|bingbot"))
			{
				return false;
			}
			capabilities["crawler"] = "true";
			this.CrawlerProcessGateways(headers, browserCaps);
			bool flag = false;
			this.CrawlerProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x00074DD3 File Offset: 0x00073DD3
		protected virtual void PlatformProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x00074DD5 File Offset: 0x00073DD5
		protected virtual void PlatformProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x00074DD8 File Offset: 0x00073DD8
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

		// Token: 0x0600184B RID: 6219 RVA: 0x00074E7C File Offset: 0x00073E7C
		protected virtual void PlatformwinntProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x00074E7E File Offset: 0x00073E7E
		protected virtual void PlatformwinntProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x00074E80 File Offset: 0x00073E80
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

		// Token: 0x0600184E RID: 6222 RVA: 0x00074F14 File Offset: 0x00073F14
		protected virtual void PlatformwinxpProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x00074F16 File Offset: 0x00073F16
		protected virtual void PlatformwinxpProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x00074F18 File Offset: 0x00073F18
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

		// Token: 0x06001851 RID: 6225 RVA: 0x00074F77 File Offset: 0x00073F77
		protected virtual void Platformwin2000aProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x00074F79 File Offset: 0x00073F79
		protected virtual void Platformwin2000aProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x00074F7C File Offset: 0x00073F7C
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

		// Token: 0x06001854 RID: 6228 RVA: 0x00074FDB File Offset: 0x00073FDB
		protected virtual void Platformwin2000bProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x00074FDD File Offset: 0x00073FDD
		protected virtual void Platformwin2000bProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x00074FE0 File Offset: 0x00073FE0
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

		// Token: 0x06001857 RID: 6231 RVA: 0x0007503F File Offset: 0x0007403F
		protected virtual void Platformwin95ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x00075041 File Offset: 0x00074041
		protected virtual void Platformwin95ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x00075044 File Offset: 0x00074044
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

		// Token: 0x0600185A RID: 6234 RVA: 0x000750A3 File Offset: 0x000740A3
		protected virtual void Platformwin98ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x000750A5 File Offset: 0x000740A5
		protected virtual void Platformwin98ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x000750A8 File Offset: 0x000740A8
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

		// Token: 0x0600185D RID: 6237 RVA: 0x00075107 File Offset: 0x00074107
		protected virtual void Platformwin16ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x00075109 File Offset: 0x00074109
		protected virtual void Platformwin16ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0007510C File Offset: 0x0007410C
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

		// Token: 0x06001860 RID: 6240 RVA: 0x0007516B File Offset: 0x0007416B
		protected virtual void PlatformwinceProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0007516D File Offset: 0x0007416D
		protected virtual void PlatformwinceProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x00075170 File Offset: 0x00074170
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

		// Token: 0x06001863 RID: 6243 RVA: 0x000751CF File Offset: 0x000741CF
		protected virtual void Platformmac68kProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x000751D1 File Offset: 0x000741D1
		protected virtual void Platformmac68kProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x000751D4 File Offset: 0x000741D4
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

		// Token: 0x06001866 RID: 6246 RVA: 0x00075233 File Offset: 0x00074233
		protected virtual void PlatformmacppcProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00075235 File Offset: 0x00074235
		protected virtual void PlatformmacppcProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00075238 File Offset: 0x00074238
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

		// Token: 0x06001869 RID: 6249 RVA: 0x00075297 File Offset: 0x00074297
		protected virtual void PlatformunixProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00075299 File Offset: 0x00074299
		protected virtual void PlatformunixProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x0007529C File Offset: 0x0007429C
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

		// Token: 0x0600186C RID: 6252 RVA: 0x000752FB File Offset: 0x000742FB
		protected virtual void PlatformwebtvProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x000752FD File Offset: 0x000742FD
		protected virtual void PlatformwebtvProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x00075300 File Offset: 0x00074300
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

		// Token: 0x0600186F RID: 6255 RVA: 0x0007535F File Offset: 0x0007435F
		protected virtual void WinProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x00075361 File Offset: 0x00074361
		protected virtual void WinProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x00075364 File Offset: 0x00074364
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

		// Token: 0x06001872 RID: 6258 RVA: 0x000753B8 File Offset: 0x000743B8
		protected virtual void Win32ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x000753BA File Offset: 0x000743BA
		protected virtual void Win32ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x000753BC File Offset: 0x000743BC
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

		// Token: 0x06001875 RID: 6261 RVA: 0x0007541B File Offset: 0x0007441B
		protected virtual void Win16ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0007541D File Offset: 0x0007441D
		protected virtual void Win16ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00075420 File Offset: 0x00074420
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

		// Token: 0x06001878 RID: 6264 RVA: 0x0007547F File Offset: 0x0007447F
		protected virtual void GenericdownlevelProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00075481 File Offset: 0x00074481
		protected virtual void GenericdownlevelProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00075484 File Offset: 0x00074484
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
			browserCaps.Adapters["System.Web.UI.WebControls.Menu, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"] = "System.Web.UI.WebControls.Adapters.MenuAdapter";
			browserCaps.AddBrowser("GenericDownlevel");
			this.GenericdownlevelProcessGateways(headers, browserCaps);
			bool flag = false;
			this.GenericdownlevelProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00075533 File Offset: 0x00074533
		protected virtual void MozillaProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00075535 File Offset: 0x00074535
		protected virtual void MozillaProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00075538 File Offset: 0x00074538
		private bool MozillaProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Mozilla"))
			{
				return false;
			}
			capabilities["browser"] = "Mozilla";
			capabilities["cookies"] = "true";
			capabilities["ecmascriptversion"] = "3.0";
			capabilities["frames"] = "true";
			capabilities["inputType"] = "keyboard";
			capabilities["isColor"] = "true";
			capabilities["isMobileDevice"] = "false";
			capabilities["javascript"] = "true";
			capabilities["javascriptversion"] = "1.5";
			capabilities["maximumRenderedPageSize"] = "300000";
			capabilities["screenBitDepth"] = "8";
			capabilities["supportsBold"] = "true";
			capabilities["supportsCallback"] = "true";
			capabilities["supportsCss"] = "true";
			capabilities["supportsDivNoWrap"] = "true";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsFontName"] = "true";
			capabilities["supportsFontSize"] = "true";
			capabilities["supportsImageSubmit"] = "true";
			capabilities["supportsItalic"] = "true";
			capabilities["supportsMaintainScrollPositionOnPostback"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["supportsXmlHttp"] = "true";
			capabilities["tables"] = "true";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["type"] = "Mozilla";
			capabilities["w3cdomversion"] = "1.0";
			browserCaps.AddBrowser("Mozilla");
			this.MozillaProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.IeProcess(headers, browserCaps) && !this.InternetexplorerProcess(headers, browserCaps) && !this.FirefoxProcess(headers, browserCaps) && !this.WebkitProcess(headers, browserCaps) && !this.IemobileProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.MozillaProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x00075777 File Offset: 0x00074777
		protected virtual void WebkitProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x00075779 File Offset: 0x00074779
		protected virtual void WebkitProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0007577C File Offset: 0x0007477C
		private bool WebkitProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "AppleWebKit"))
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "AppleWebKit/(?'layoutVersion'\\d+)");
			capabilities["layoutEngine"] = "WebKit";
			capabilities["layoutEngineVersion"] = regexWorker["${layoutVersion}"];
			browserCaps.AddBrowser("WebKit");
			this.WebkitProcessGateways(headers, browserCaps);
			this.WebkitmobileProcess(headers, browserCaps);
			bool flag = true;
			if (!this.ChromeProcess(headers, browserCaps) && !this.SafariProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.WebkitProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00075833 File Offset: 0x00074833
		protected virtual void WebkitmobileProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x00075835 File Offset: 0x00074835
		protected virtual void WebkitmobileProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00075838 File Offset: 0x00074838
		private bool WebkitmobileProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Mobile( Safari)?/(?'iOSVersion'[^ ]+)"))
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "Mozilla/5.0 \\((?'deviceName'[^;]+)");
			capabilities["mobileDeviceModel"] = regexWorker["${deviceName}"];
			capabilities["isMobileDevice"] = "true";
			capabilities["ecmascriptversion"] = "3.0";
			capabilities["javascript"] = "true";
			capabilities["javascriptversion"] = "1.6";
			capabilities["w3cdomversion"] = "1.0";
			capabilities["supportsAccesskeyAttribute"] = "true";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["cookies"] = "true";
			capabilities["frames"] = "true";
			capabilities["supportsCallback"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsMaintainScrollPositionOnPostback"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["supportsXmlHttp"] = "true";
			capabilities["tables"] = "true";
			this.WebkitmobileProcessGateways(headers, browserCaps);
			bool flag = false;
			this.WebkitmobileProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x000759B4 File Offset: 0x000749B4
		protected virtual void IemobileProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x000759B6 File Offset: 0x000749B6
		protected virtual void IemobileProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x000759B8 File Offset: 0x000749B8
		private bool IemobileProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "IEMobile.(?'version'(?'major'\\d+)(\\.(?'minor'\\d+)?)\\w*)"))
			{
				return false;
			}
			regexWorker.ProcessRegex(browserCaps[string.Empty], "MSIE (?'msieMajorVersion'\\d+)");
			capabilities["layoutEngine"] = "Trident";
			capabilities["browser"] = "IEMobile";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["type"] = regexWorker["IEMobile${msieMajorVersion}"];
			capabilities["isMobileDevice"] = "true";
			capabilities["version"] = regexWorker["${version}"];
			capabilities["jscriptversion"] = "5.6";
			capabilities["msdomversion"] = regexWorker["${majorversion}.${minorversion}"];
			capabilities["supportsAccesskeyAttribute"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["vbscript"] = "true";
			capabilities["inputType"] = "virtualKeyboard";
			capabilities["numberOfSoftkeys"] = "2";
			browserCaps.AddBrowser("IEMobile");
			this.IemobileProcessGateways(headers, browserCaps);
			this.MonoProcess(headers, browserCaps);
			this.PixelsProcess(headers, browserCaps);
			this.OsProcess(headers, browserCaps);
			this.CpuProcess(headers, browserCaps);
			this.VoiceProcess(headers, browserCaps);
			bool flag = true;
			if (!this.WindowsphoneProcess(headers, browserCaps))
			{
				flag = false;
			}
			this.IemobileProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00075B71 File Offset: 0x00074B71
		protected virtual void WindowsphoneProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x00075B73 File Offset: 0x00074B73
		protected virtual void WindowsphoneProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x00075B78 File Offset: 0x00074B78
		private bool WindowsphoneProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Windows Phone OS"))
			{
				return false;
			}
			capabilities["javaapplets"] = "false";
			capabilities["jscriptversion"] = "5.7";
			browserCaps.AddBrowser("WindowsPhone");
			this.WindowsphoneProcessGateways(headers, browserCaps);
			bool flag = false;
			this.WindowsphoneProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x00075BF2 File Offset: 0x00074BF2
		protected virtual void MonoProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x00075BF4 File Offset: 0x00074BF4
		protected virtual void MonoProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00075BF8 File Offset: 0x00074BF8
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

		// Token: 0x0600188D RID: 6285 RVA: 0x00075C89 File Offset: 0x00074C89
		protected virtual void PixelsProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x00075C8B File Offset: 0x00074C8B
		protected virtual void PixelsProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x00075C90 File Offset: 0x00074C90
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

		// Token: 0x06001890 RID: 6288 RVA: 0x00075D27 File Offset: 0x00074D27
		protected virtual void OSProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x00075D29 File Offset: 0x00074D29
		protected virtual void OSProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x00075D2C File Offset: 0x00074D2C
		private bool OsProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["UA-OS"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'os'.+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["platform"] = regexWorker["${os}"];
			this.OSProcessGateways(headers, browserCaps);
			bool flag = false;
			this.OSProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x00075D97 File Offset: 0x00074D97
		protected virtual void CpuProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x00075D99 File Offset: 0x00074D99
		protected virtual void CpuProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x00075D9C File Offset: 0x00074D9C
		private bool CpuProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = headers["UA-CPU"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(?'cpu'.+)"))
			{
				return false;
			}
			browserCaps.DisableOptimizedCacheKey();
			capabilities["cpu"] = regexWorker["${cpu}"];
			this.CpuProcessGateways(headers, browserCaps);
			bool flag = false;
			this.CpuProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x00075E07 File Offset: 0x00074E07
		protected virtual void VoiceProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00075E09 File Offset: 0x00074E09
		protected virtual void VoiceProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00075E0C File Offset: 0x00074E0C
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

		// Token: 0x06001899 RID: 6297 RVA: 0x00075E87 File Offset: 0x00074E87
		protected virtual void IphoneProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x00075E89 File Offset: 0x00074E89
		protected virtual void IphoneProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x00075E8C File Offset: 0x00074E8C
		private bool IphoneProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "iPhone"))
			{
				return false;
			}
			capabilities["isMobileDevice"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Apple";
			capabilities["mobileDeviceModel"] = "IPhone";
			capabilities["canInitiateVoiceCall"] = "true";
			this.IphoneProcessGateways(headers, browserCaps);
			bool flag = false;
			this.IphoneProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x00075F1B File Offset: 0x00074F1B
		protected virtual void IpodProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x00075F1D File Offset: 0x00074F1D
		protected virtual void IpodProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x00075F20 File Offset: 0x00074F20
		private bool IpodProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "iPod"))
			{
				return false;
			}
			capabilities["isMobileDevice"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Apple";
			capabilities["mobileDeviceModel"] = "IPod";
			this.IpodProcessGateways(headers, browserCaps);
			bool flag = false;
			this.IpodProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00075F9F File Offset: 0x00074F9F
		protected virtual void IpadProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x00075FA1 File Offset: 0x00074FA1
		protected virtual void IpadProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00075FA4 File Offset: 0x00074FA4
		private bool IpadProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "iPad"))
			{
				return false;
			}
			capabilities["isMobileDevice"] = "true";
			capabilities["mobileDeviceManufacturer"] = "Apple";
			capabilities["mobileDeviceModel"] = "IPad";
			this.IpadProcessGateways(headers, browserCaps);
			bool flag = false;
			this.IpadProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x00076023 File Offset: 0x00075023
		protected virtual void SafariProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x00076025 File Offset: 0x00075025
		protected virtual void SafariProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x00076028 File Offset: 0x00075028
		private bool SafariProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Safari"))
			{
				return false;
			}
			text = browserCaps[string.Empty];
			bool flag = regexWorker.ProcessRegex(text, "Chrome");
			if (flag)
			{
				return false;
			}
			text = browserCaps[string.Empty];
			flag = regexWorker.ProcessRegex(text, "Android");
			if (flag)
			{
				return false;
			}
			capabilities["browser"] = "Safari";
			capabilities["type"] = "Safari";
			browserCaps.AddBrowser("Safari");
			this.SafariProcessGateways(headers, browserCaps);
			this.IphoneProcess(headers, browserCaps);
			this.IpodProcess(headers, browserCaps);
			this.IpadProcess(headers, browserCaps);
			bool flag2 = true;
			if (!this.Safari3plusProcess(headers, browserCaps))
			{
				flag2 = false;
			}
			this.SafariProcessBrowsers(flag2, headers, browserCaps);
			return true;
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x00076106 File Offset: 0x00075106
		protected virtual void Safari3plusProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x00076108 File Offset: 0x00075108
		protected virtual void Safari3plusProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0007610C File Offset: 0x0007510C
		private bool Safari3plusProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "Version/(?'version'(?'major'[3-9]|\\d{2,})(\\.(?'minor'\\d+)?)\\w*)"))
			{
				return false;
			}
			capabilities["version"] = regexWorker["${version}"];
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["type"] = regexWorker["Safari${major}"];
			capabilities["ecmascriptversion"] = "3.0";
			capabilities["javascript"] = "true";
			capabilities["javascriptversion"] = "1.6";
			capabilities["w3cdomversion"] = "1.0";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["cookies"] = "true";
			capabilities["frames"] = "true";
			capabilities["javaapplets"] = "true";
			capabilities["supportsAccesskeyAttribute"] = "true";
			capabilities["supportsCallback"] = "true";
			capabilities["supportsDivNoWrap"] = "false";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsMaintainScrollPositionOnPostback"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["supportsXmlHttp"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("Safari3Plus");
			this.Safari3plusProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Safari3to4Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Safari3plusProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x000762CB File Offset: 0x000752CB
		protected virtual void Safari3to4ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x000762CD File Offset: 0x000752CD
		protected virtual void Safari3to4ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x000762D0 File Offset: 0x000752D0
		private bool Safari3to4Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^[3-4]$"))
			{
				return false;
			}
			browserCaps.AddBrowser("Safari3to4");
			this.Safari3to4ProcessGateways(headers, browserCaps);
			bool flag = true;
			if (!this.Safari4Process(headers, browserCaps))
			{
				flag = false;
			}
			this.Safari3to4ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0007633C File Offset: 0x0007533C
		protected virtual void Safari4ProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x0007633E File Offset: 0x0007533E
		protected virtual void Safari4ProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x00076340 File Offset: 0x00075340
		private bool Safari4Process(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = (string)capabilities["majorversion"];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "^4$"))
			{
				return false;
			}
			capabilities["javascriptversion"] = "1.7";
			browserCaps.AddBrowser("Safari4");
			this.Safari4ProcessGateways(headers, browserCaps);
			bool flag = false;
			this.Safari4ProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x000763AF File Offset: 0x000753AF
		protected virtual void UcbrowserProcessGateways(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x000763B1 File Offset: 0x000753B1
		protected virtual void UcbrowserProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x000763B4 File Offset: 0x000753B4
		private bool UcbrowserProcess(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
			IDictionary capabilities = browserCaps.Capabilities;
			string text = browserCaps[string.Empty];
			RegexWorker regexWorker = new RegexWorker(browserCaps);
			if (!regexWorker.ProcessRegex(text, "(UC Browser |UCWEB)(?'version'(?'major'\\d+)(\\.(?'minor'[\\d\\.]+)?)\\w*)"))
			{
				return false;
			}
			capabilities["browser"] = "UCBrowser";
			capabilities["majorversion"] = regexWorker["${major}"];
			capabilities["minorversion"] = regexWorker["${minor}"];
			capabilities["isMobileDevice"] = "true";
			capabilities["version"] = regexWorker["${version}"];
			capabilities["ecmascriptversion"] = "3.0";
			capabilities["javascript"] = "true";
			capabilities["javascriptversion"] = "1.5";
			capabilities["tagwriter"] = "System.Web.UI.HtmlTextWriter";
			capabilities["cookies"] = "true";
			capabilities["frames"] = "true";
			capabilities["supportsCallback"] = "true";
			capabilities["supportsFileUpload"] = "true";
			capabilities["supportsMultilineTextBoxDisplay"] = "true";
			capabilities["supportsXmlHttp"] = "true";
			capabilities["tables"] = "true";
			browserCaps.AddBrowser("UCBrowser");
			this.UcbrowserProcessGateways(headers, browserCaps);
			bool flag = false;
			this.UcbrowserProcessBrowsers(flag, headers, browserCaps);
			return true;
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x00076520 File Offset: 0x00075520
		protected virtual void DefaultDefaultProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x00076524 File Offset: 0x00075524
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

		// Token: 0x060018B3 RID: 6323 RVA: 0x0007658A File Offset: 0x0007558A
		protected virtual void DefaultWmlProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x0007658C File Offset: 0x0007558C
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

		// Token: 0x060018B5 RID: 6325 RVA: 0x00076617 File Offset: 0x00075617
		protected virtual void DefaultXhtmlmpProcessBrowsers(bool ignoreApplicationBrowsers, NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x0007661C File Offset: 0x0007561C
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

		// Token: 0x060018B7 RID: 6327 RVA: 0x000766D0 File Offset: 0x000756D0
		protected override void PopulateMatchedHeaders(IDictionary dictionary)
		{
			base.PopulateMatchedHeaders(dictionary);
			dictionary[""] = null;
			dictionary["UA-COLOR"] = null;
			dictionary["UA-PIXELS"] = null;
			dictionary["UA-OS"] = null;
			dictionary["UA-CPU"] = null;
			dictionary["UA-VOICE"] = null;
			dictionary["Accept"] = null;
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x00076738 File Offset: 0x00075738
		protected override void PopulateBrowserElements(IDictionary dictionary)
		{
			base.PopulateBrowserElements(dictionary);
			dictionary["Default"] = new Triplet(null, string.Empty, 0);
			dictionary["BlackBerry"] = new Triplet("Default", string.Empty, 1);
			dictionary["Opera"] = new Triplet("Default", string.Empty, 1);
			dictionary["Opera8Plus"] = new Triplet("Opera", string.Empty, 2);
			dictionary["Opera8to9"] = new Triplet("Opera8plus", string.Empty, 3);
			dictionary["Opera10"] = new Triplet("Opera8plus", string.Empty, 3);
			dictionary["GenericDownlevel"] = new Triplet("Default", string.Empty, 1);
			dictionary["Mozilla"] = new Triplet("Default", string.Empty, 1);
			dictionary["IE"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["IE6Plus"] = new Triplet("Ie", string.Empty, 3);
			dictionary["IE6to9"] = new Triplet("Ie6plus", string.Empty, 4);
			dictionary["IE7"] = new Triplet("Ie6to9", string.Empty, 5);
			dictionary["IE8"] = new Triplet("Ie6to9", string.Empty, 5);
			dictionary["IE9"] = new Triplet("Ie6to9", string.Empty, 5);
			dictionary["IE10Plus"] = new Triplet("Ie6plus", string.Empty, 4);
			dictionary["InternetExplorer"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["Firefox"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["Firefox3Plus"] = new Triplet("Firefox", string.Empty, 3);
			dictionary["Firefox3"] = new Triplet("Firefox3plus", string.Empty, 4);
			dictionary["Firefox35"] = new Triplet("Firefox3", string.Empty, 5);
			dictionary["WebKit"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["Chrome"] = new Triplet("Webkit", string.Empty, 3);
			dictionary["Safari"] = new Triplet("Webkit", string.Empty, 3);
			dictionary["Safari3Plus"] = new Triplet("Safari", string.Empty, 4);
			dictionary["Safari3to4"] = new Triplet("Safari3plus", string.Empty, 5);
			dictionary["Safari4"] = new Triplet("Safari3to4", string.Empty, 6);
			dictionary["IEMobile"] = new Triplet("Mozilla", string.Empty, 2);
			dictionary["WindowsPhone"] = new Triplet("Iemobile", string.Empty, 3);
			dictionary["UCBrowser"] = new Triplet("Default", string.Empty, 1);
		}
	}
}
