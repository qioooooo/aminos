using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Compilation;
using System.Web.Management;

namespace System.Web.Configuration
{
	// Token: 0x020001EC RID: 492
	internal class HealthMonitoringSectionHelper
	{
		// Token: 0x06001B1C RID: 6940 RVA: 0x0007D44C File Offset: 0x0007C44C
		internal static HealthMonitoringSectionHelper GetHelper()
		{
			if (HealthMonitoringSectionHelper.s_helper == null)
			{
				HealthMonitoringSectionHelper.s_helper = new HealthMonitoringSectionHelper();
			}
			return HealthMonitoringSectionHelper.s_helper;
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x0007D464 File Offset: 0x0007C464
		private HealthMonitoringSectionHelper()
		{
			try
			{
				this._section = RuntimeConfig.GetAppConfig().HealthMonitoring;
			}
			catch (Exception ex)
			{
				if (HttpRuntime.InitializationException == null)
				{
					HttpRuntime.InitializationException = ex;
				}
				this._section = RuntimeConfig.GetAppLKGConfig().HealthMonitoring;
				if (this._section == null)
				{
					throw;
				}
			}
			this._enabled = this._section.Enabled;
			if (!this._enabled)
			{
				return;
			}
			this.BasicSanityCheck();
			this._ruleInfos = new ArrayList();
			this._customEvaluatorInstances = new Hashtable();
			this._providerInstances = new HealthMonitoringSectionHelper.ProviderInstances(this._section);
			this._cachedMatchedRulesForCustomEvents = new Hashtable(new WebBaseEventKeyComparer());
			HealthMonitoringSectionHelper._cachedMatchedRules = new ArrayList[WebEventCodes.GetEventArrayDimensionSize(0), WebEventCodes.GetEventArrayDimensionSize(1)];
			this.BuildRuleInfos();
			this._providerInstances.CleanupUninitProviders();
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001B1E RID: 6942 RVA: 0x0007D544 File Offset: 0x0007C544
		internal bool Enabled
		{
			get
			{
				return this._enabled;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001B1F RID: 6943 RVA: 0x0007D54C File Offset: 0x0007C54C
		internal HealthMonitoringSection HealthMonitoringSection
		{
			get
			{
				return this._section;
			}
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x0007D554 File Offset: 0x0007C554
		private void BasicSanityCheck()
		{
			foreach (object obj in this._section.Providers)
			{
				ProviderSettings providerSettings = (ProviderSettings)obj;
				Type type = ConfigUtil.GetType(providerSettings.Type, "type", providerSettings);
				HandlerBase.CheckAssignableType(providerSettings.ElementInformation.Properties["type"].Source, providerSettings.ElementInformation.Properties["type"].LineNumber, typeof(WebEventProvider), type);
			}
			foreach (object obj2 in this._section.EventMappings)
			{
				EventMappingSettings eventMappingSettings = (EventMappingSettings)obj2;
				Type type = ConfigUtil.GetType(eventMappingSettings.Type, "type", eventMappingSettings);
				if (eventMappingSettings.StartEventCode > eventMappingSettings.EndEventCode)
				{
					string text = "startEventCode";
					if (eventMappingSettings.ElementInformation.Properties[text].LineNumber == 0)
					{
						text = "endEventCode";
					}
					throw new ConfigurationErrorsException(SR.GetString("Event_name_invalid_code_range"), eventMappingSettings.ElementInformation.Properties[text].Source, eventMappingSettings.ElementInformation.Properties[text].LineNumber);
				}
				HandlerBase.CheckAssignableType(eventMappingSettings.ElementInformation.Properties["type"].Source, eventMappingSettings.ElementInformation.Properties["type"].LineNumber, typeof(WebBaseEvent), type);
				eventMappingSettings.RealType = type;
			}
			foreach (object obj3 in this._section.Rules)
			{
				RuleSettings ruleSettings = (RuleSettings)obj3;
				string provider = ruleSettings.Provider;
				if (!string.IsNullOrEmpty(provider) && this._section.Providers[provider] == null)
				{
					throw new ConfigurationErrorsException(SR.GetString("Health_mon_provider_not_found", new object[] { provider }), ruleSettings.ElementInformation.Properties["provider"].Source, ruleSettings.ElementInformation.Properties["provider"].LineNumber);
				}
				string profile = ruleSettings.Profile;
				if (!string.IsNullOrEmpty(profile) && this._section.Profiles[profile] == null)
				{
					throw new ConfigurationErrorsException(SR.GetString("Health_mon_profile_not_found", new object[] { profile }), ruleSettings.ElementInformation.Properties["profile"].Source, ruleSettings.ElementInformation.Properties["profile"].LineNumber);
				}
				if (this._section.EventMappings[ruleSettings.EventName] == null)
				{
					throw new ConfigurationErrorsException(SR.GetString("Event_name_not_found", new object[] { ruleSettings.EventName }), ruleSettings.ElementInformation.Properties["eventName"].Source, ruleSettings.ElementInformation.Properties["eventName"].LineNumber);
				}
			}
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0007D910 File Offset: 0x0007C910
		private void DisplayRuleInfo(HealthMonitoringSectionHelper.RuleInfo ruleInfo)
		{
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0007D914 File Offset: 0x0007C914
		private void BuildRuleInfos()
		{
			foreach (object obj in this._section.Rules)
			{
				RuleSettings ruleSettings = (RuleSettings)obj;
				HealthMonitoringSectionHelper.RuleInfo ruleInfo = this.CreateRuleInfo(ruleSettings);
				this.DisplayRuleInfo(ruleInfo);
				this._ruleInfos.Add(ruleInfo);
			}
			this._ruleInfos.Sort(HealthMonitoringSectionHelper.s_ruleInfoComparer);
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x0007D998 File Offset: 0x0007C998
		private HealthMonitoringSectionHelper.RuleInfo CreateRuleInfo(RuleSettings ruleSettings)
		{
			HealthMonitoringSectionHelper.RuleInfo ruleInfo = new HealthMonitoringSectionHelper.RuleInfo(ruleSettings, this._section);
			this.MergeValuesWithProfile(ruleInfo);
			this.InitReferencedProvider(ruleInfo);
			this.InitCustomEvaluator(ruleInfo);
			return ruleInfo;
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0007D9C8 File Offset: 0x0007C9C8
		private void InitReferencedProvider(HealthMonitoringSectionHelper.RuleInfo ruleInfo)
		{
			string provider = ruleInfo._ruleSettings.Provider;
			if (string.IsNullOrEmpty(provider))
			{
				return;
			}
			WebEventProvider webEventProvider = this._providerInstances[provider];
			ruleInfo._referencedProvider = webEventProvider;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0007DA00 File Offset: 0x0007CA00
		private void MergeValuesWithProfile(HealthMonitoringSectionHelper.RuleInfo ruleInfo)
		{
			ProfileSettings profileSettings = null;
			if (ruleInfo._ruleSettings.ElementInformation.Properties["profile"].ValueOrigin != PropertyValueOrigin.Default)
			{
				profileSettings = this._section.Profiles[ruleInfo._ruleSettings.Profile];
			}
			if (profileSettings != null && ruleInfo._ruleSettings.ElementInformation.Properties["minInstances"].ValueOrigin == PropertyValueOrigin.Default)
			{
				ruleInfo._minInstances = profileSettings.MinInstances;
			}
			else
			{
				ruleInfo._minInstances = ruleInfo._ruleSettings.MinInstances;
			}
			if (profileSettings != null && ruleInfo._ruleSettings.ElementInformation.Properties["maxLimit"].ValueOrigin == PropertyValueOrigin.Default)
			{
				ruleInfo._maxLimit = profileSettings.MaxLimit;
			}
			else
			{
				ruleInfo._maxLimit = ruleInfo._ruleSettings.MaxLimit;
			}
			if (profileSettings != null && ruleInfo._ruleSettings.ElementInformation.Properties["minInterval"].ValueOrigin == PropertyValueOrigin.Default)
			{
				ruleInfo._minInterval = profileSettings.MinInterval;
			}
			else
			{
				ruleInfo._minInterval = ruleInfo._ruleSettings.MinInterval;
			}
			if (profileSettings != null && ruleInfo._ruleSettings.ElementInformation.Properties["custom"].ValueOrigin == PropertyValueOrigin.Default)
			{
				ruleInfo._customEvaluator = profileSettings.Custom;
				ruleInfo._customEvaluatorConfig = profileSettings;
				return;
			}
			ruleInfo._customEvaluator = ruleInfo._ruleSettings.Custom;
			ruleInfo._customEvaluatorConfig = ruleInfo._ruleSettings;
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0007DB6C File Offset: 0x0007CB6C
		private void InitCustomEvaluator(HealthMonitoringSectionHelper.RuleInfo ruleInfo)
		{
			string customEvaluator = ruleInfo._customEvaluator;
			if (customEvaluator == null || customEvaluator.Trim().Length == 0)
			{
				ruleInfo._customEvaluatorType = null;
				return;
			}
			ruleInfo._customEvaluatorType = ConfigUtil.GetType(ruleInfo._customEvaluator, "custom", ruleInfo._customEvaluatorConfig);
			HandlerBase.CheckAssignableType(ruleInfo._customEvaluatorConfig.ElementInformation.Properties["custom"].Source, ruleInfo._customEvaluatorConfig.ElementInformation.Properties["custom"].LineNumber, typeof(IWebEventCustomEvaluator), ruleInfo._customEvaluatorType);
			if (this._customEvaluatorInstances[ruleInfo._customEvaluatorType] == null)
			{
				this._customEvaluatorInstances[ruleInfo._customEvaluatorType] = HttpRuntime.CreatePublicInstance(ruleInfo._customEvaluatorType);
			}
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0007DC38 File Offset: 0x0007CC38
		internal ArrayList FindFiringRuleInfos(Type eventType, int eventCode)
		{
			bool flag = eventCode < 100000;
			CustomWebEventKey customWebEventKey = null;
			int num = 0;
			int num2 = 0;
			ArrayList arrayList;
			if (flag)
			{
				WebEventCodes.GetEventArrayIndexsFromEventCode(eventCode, out num, out num2);
				arrayList = HealthMonitoringSectionHelper._cachedMatchedRules[num, num2];
			}
			else
			{
				customWebEventKey = new CustomWebEventKey(eventType, eventCode);
				arrayList = (ArrayList)this._cachedMatchedRulesForCustomEvents[customWebEventKey];
			}
			if (arrayList != null)
			{
				return arrayList;
			}
			object obj;
			if (flag)
			{
				obj = HealthMonitoringSectionHelper._cachedMatchedRules;
			}
			else
			{
				obj = this._cachedMatchedRulesForCustomEvents;
			}
			ArrayList arrayList2;
			lock (obj)
			{
				if (flag)
				{
					arrayList = HealthMonitoringSectionHelper._cachedMatchedRules[num, num2];
				}
				else
				{
					arrayList = (ArrayList)this._cachedMatchedRulesForCustomEvents[customWebEventKey];
				}
				if (arrayList != null)
				{
					arrayList2 = arrayList;
				}
				else
				{
					ArrayList arrayList3 = new ArrayList();
					for (int i = this._ruleInfos.Count - 1; i >= 0; i--)
					{
						HealthMonitoringSectionHelper.RuleInfo ruleInfo = (HealthMonitoringSectionHelper.RuleInfo)this._ruleInfos[i];
						if (ruleInfo.Match(eventType, eventCode))
						{
							arrayList3.Add(new HealthMonitoringSectionHelper.FiringRuleInfo(ruleInfo));
						}
					}
					int count = arrayList3.Count;
					for (int j = 0; j < count; j++)
					{
						HealthMonitoringSectionHelper.FiringRuleInfo firingRuleInfo = (HealthMonitoringSectionHelper.FiringRuleInfo)arrayList3[j];
						if (firingRuleInfo._ruleInfo._referencedProvider != null)
						{
							for (int k = j + 1; k < count; k++)
							{
								HealthMonitoringSectionHelper.FiringRuleInfo firingRuleInfo2 = (HealthMonitoringSectionHelper.FiringRuleInfo)arrayList3[k];
								if (firingRuleInfo2._ruleInfo._referencedProvider != null && firingRuleInfo2._indexOfFirstRuleInfoWithSameProvider == -1 && firingRuleInfo._ruleInfo._referencedProvider == firingRuleInfo2._ruleInfo._referencedProvider)
								{
									if (firingRuleInfo._indexOfFirstRuleInfoWithSameProvider == -1)
									{
										firingRuleInfo._indexOfFirstRuleInfoWithSameProvider = j;
									}
									firingRuleInfo2._indexOfFirstRuleInfoWithSameProvider = j;
								}
							}
						}
					}
					if (flag)
					{
						HealthMonitoringSectionHelper._cachedMatchedRules[num, num2] = arrayList3;
					}
					else
					{
						this._cachedMatchedRulesForCustomEvents[customWebEventKey] = arrayList3;
					}
					arrayList2 = arrayList3;
				}
			}
			return arrayList2;
		}

		// Token: 0x04001824 RID: 6180
		private static HealthMonitoringSectionHelper s_helper;

		// Token: 0x04001825 RID: 6181
		private static RuleInfoComparer s_ruleInfoComparer = new RuleInfoComparer();

		// Token: 0x04001826 RID: 6182
		private HealthMonitoringSection _section;

		// Token: 0x04001827 RID: 6183
		internal HealthMonitoringSectionHelper.ProviderInstances _providerInstances;

		// Token: 0x04001828 RID: 6184
		internal Hashtable _customEvaluatorInstances;

		// Token: 0x04001829 RID: 6185
		internal ArrayList _ruleInfos;

		// Token: 0x0400182A RID: 6186
		private bool _enabled;

		// Token: 0x0400182B RID: 6187
		private static ArrayList[,] _cachedMatchedRules;

		// Token: 0x0400182C RID: 6188
		private Hashtable _cachedMatchedRulesForCustomEvents;

		// Token: 0x020001ED RID: 493
		internal class RuleInfo
		{
			// Token: 0x06001B29 RID: 6953 RVA: 0x0007DE3C File Offset: 0x0007CE3C
			internal RuleInfo(RuleSettings ruleSettings, HealthMonitoringSection section)
			{
				this._eventMappingSettings = section.EventMappings[ruleSettings.EventName];
				this._ruleSettings = ruleSettings;
				this._ruleFiringRecord = new RuleFiringRecord(this);
			}

			// Token: 0x06001B2A RID: 6954 RVA: 0x0007DE70 File Offset: 0x0007CE70
			internal bool Match(Type eventType, int eventCode)
			{
				return (eventType.Equals(this._eventMappingSettings.RealType) || eventType.IsSubclassOf(this._eventMappingSettings.RealType)) && this._eventMappingSettings.StartEventCode <= eventCode && eventCode <= this._eventMappingSettings.EndEventCode;
			}

			// Token: 0x0400182D RID: 6189
			internal string _customEvaluator;

			// Token: 0x0400182E RID: 6190
			internal ConfigurationElement _customEvaluatorConfig;

			// Token: 0x0400182F RID: 6191
			internal int _minInstances;

			// Token: 0x04001830 RID: 6192
			internal int _maxLimit;

			// Token: 0x04001831 RID: 6193
			internal TimeSpan _minInterval;

			// Token: 0x04001832 RID: 6194
			internal RuleSettings _ruleSettings;

			// Token: 0x04001833 RID: 6195
			internal WebEventProvider _referencedProvider;

			// Token: 0x04001834 RID: 6196
			internal Type _customEvaluatorType;

			// Token: 0x04001835 RID: 6197
			internal EventMappingSettings _eventMappingSettings;

			// Token: 0x04001836 RID: 6198
			internal RuleFiringRecord _ruleFiringRecord;
		}

		// Token: 0x020001EE RID: 494
		internal class FiringRuleInfo
		{
			// Token: 0x06001B2B RID: 6955 RVA: 0x0007DEC4 File Offset: 0x0007CEC4
			internal FiringRuleInfo(HealthMonitoringSectionHelper.RuleInfo ruleInfo)
			{
				this._ruleInfo = ruleInfo;
				this._indexOfFirstRuleInfoWithSameProvider = -1;
			}

			// Token: 0x04001837 RID: 6199
			internal HealthMonitoringSectionHelper.RuleInfo _ruleInfo;

			// Token: 0x04001838 RID: 6200
			internal int _indexOfFirstRuleInfoWithSameProvider;
		}

		// Token: 0x020001EF RID: 495
		internal class ProviderInstances
		{
			// Token: 0x06001B2C RID: 6956 RVA: 0x0007DEDC File Offset: 0x0007CEDC
			internal ProviderInstances(HealthMonitoringSection section)
			{
				this._instances = CollectionsUtil.CreateCaseInsensitiveHashtable(section.Providers.Count);
				foreach (object obj in section.Providers)
				{
					ProviderSettings providerSettings = (ProviderSettings)obj;
					this._instances.Add(providerSettings.Name, providerSettings);
				}
			}

			// Token: 0x06001B2D RID: 6957 RVA: 0x0007DF60 File Offset: 0x0007CF60
			private WebEventProvider GetProviderInstance(string providerName)
			{
				object obj = this._instances[providerName];
				if (obj == null)
				{
					return null;
				}
				ProviderSettings providerSettings = obj as ProviderSettings;
				WebEventProvider webEventProvider;
				if (providerSettings != null)
				{
					string type = providerSettings.Type;
					Type type2 = BuildManager.GetType(type, false);
					if (typeof(IInternalWebEventProvider).IsAssignableFrom(type2))
					{
						webEventProvider = (WebEventProvider)HttpRuntime.CreateNonPublicInstance(type2);
					}
					else
					{
						webEventProvider = (WebEventProvider)HttpRuntime.CreatePublicInstance(type2);
					}
					using (new ProcessImpersonationContext())
					{
						try
						{
							webEventProvider.Initialize(providerSettings.Name, providerSettings.Parameters);
						}
						catch (ConfigurationErrorsException)
						{
							throw;
						}
						catch (ConfigurationException ex)
						{
							throw new ConfigurationErrorsException(ex.Message, providerSettings.ElementInformation.Properties["type"].Source, providerSettings.ElementInformation.Properties["type"].LineNumber);
						}
						catch
						{
							throw;
						}
					}
					this._instances[providerName] = webEventProvider;
				}
				else
				{
					webEventProvider = obj as WebEventProvider;
				}
				return webEventProvider;
			}

			// Token: 0x1700053C RID: 1340
			internal WebEventProvider this[string name]
			{
				get
				{
					return this.GetProviderInstance(name);
				}
			}

			// Token: 0x06001B2F RID: 6959 RVA: 0x0007E094 File Offset: 0x0007D094
			internal void CleanupUninitProviders()
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in this._instances)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (dictionaryEntry.Value is ProviderSettings)
					{
						arrayList.Add(dictionaryEntry.Key);
					}
				}
				foreach (object obj2 in arrayList)
				{
					this._instances.Remove(obj2);
				}
			}

			// Token: 0x06001B30 RID: 6960 RVA: 0x0007E158 File Offset: 0x0007D158
			internal bool ContainsKey(string name)
			{
				return this._instances.ContainsKey(name);
			}

			// Token: 0x06001B31 RID: 6961 RVA: 0x0007E166 File Offset: 0x0007D166
			public IDictionaryEnumerator GetEnumerator()
			{
				return this._instances.GetEnumerator();
			}

			// Token: 0x04001839 RID: 6201
			internal Hashtable _instances;
		}
	}
}
