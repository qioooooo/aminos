using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006A3 RID: 1699
	internal sealed class BlobPersonalizationState : PersonalizationState
	{
		// Token: 0x0600530E RID: 21262 RVA: 0x001504FB File Offset: 0x0014F4FB
		public BlobPersonalizationState(WebPartManager webPartManager)
			: base(webPartManager)
		{
			this._isPostRequest = webPartManager.Page.Request.HttpVerb == HttpVerb.POST;
		}

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x0600530F RID: 21263 RVA: 0x0015051D File Offset: 0x0014F51D
		public override bool IsEmpty
		{
			get
			{
				return this._extractedState == null || this._extractedState.Count == 0;
			}
		}

		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x06005310 RID: 21264 RVA: 0x00150537 File Offset: 0x0014F537
		private bool IsPostRequest
		{
			get
			{
				return this._isPostRequest;
			}
		}

		// Token: 0x17001525 RID: 5413
		// (get) Token: 0x06005311 RID: 21265 RVA: 0x0015053F File Offset: 0x0014F53F
		private PersonalizationScope PersonalizationScope
		{
			get
			{
				return base.WebPartManager.Personalization.Scope;
			}
		}

		// Token: 0x17001526 RID: 5414
		// (get) Token: 0x06005312 RID: 21266 RVA: 0x00150551 File Offset: 0x0014F551
		private IDictionary SharedState
		{
			get
			{
				return this._sharedState;
			}
		}

		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x06005313 RID: 21267 RVA: 0x00150559 File Offset: 0x0014F559
		private IDictionary UserState
		{
			get
			{
				if (this._rawUserData != null)
				{
					this._userState = BlobPersonalizationState.DeserializeData(this._rawUserData);
					this._rawUserData = null;
				}
				if (this._userState == null)
				{
					this._userState = new HybridDictionary(false);
				}
				return this._userState;
			}
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x00150598 File Offset: 0x0014F598
		private void ApplyPersonalization(Control control, string personalizationID, bool isWebPartManager, PersonalizationScope extractScope, GenericWebPart genericWebPart)
		{
			if (this._personalizedControls == null)
			{
				this._personalizedControls = new HybridDictionary(false);
			}
			else if (this._personalizedControls.Contains(personalizationID))
			{
				throw new InvalidOperationException(SR.GetString("BlobPersonalizationState_CantApply", new object[] { personalizationID }));
			}
			IDictionary personalizablePropertyEntries = PersonalizableAttribute.GetPersonalizablePropertyEntries(control.GetType());
			if (this.SharedState == null)
			{
				throw new InvalidOperationException(SR.GetString("BlobPersonalizationState_NotLoaded"));
			}
			BlobPersonalizationState.PersonalizationInfo personalizationInfo = (BlobPersonalizationState.PersonalizationInfo)this.SharedState[personalizationID];
			BlobPersonalizationState.PersonalizationInfo personalizationInfo2 = null;
			IDictionary dictionary = null;
			IDictionary dictionary2 = null;
			PersonalizationDictionary personalizationDictionary = null;
			BlobPersonalizationState.ControlInfo controlInfo = new BlobPersonalizationState.ControlInfo();
			controlInfo._allowSetDirty = false;
			this._personalizedControls[personalizationID] = controlInfo;
			if (personalizationInfo != null && personalizationInfo._isStatic && !personalizationInfo.IsMatchingControlType(control))
			{
				personalizationInfo = null;
				if (this.PersonalizationScope == PersonalizationScope.Shared)
				{
					this.SetControlDirty(control, personalizationID, isWebPartManager, true);
				}
			}
			IPersonalizable personalizable = control as IPersonalizable;
			ITrackingPersonalizable trackingPersonalizable = control as ITrackingPersonalizable;
			WebPart webPart = null;
			if (!isWebPartManager)
			{
				if (genericWebPart != null)
				{
					webPart = genericWebPart;
				}
				else
				{
					webPart = (WebPart)control;
				}
			}
			try
			{
				if (trackingPersonalizable != null)
				{
					trackingPersonalizable.BeginLoad();
				}
				if (this.PersonalizationScope == PersonalizationScope.User)
				{
					if (this.UserState == null)
					{
						throw new InvalidOperationException(SR.GetString("BlobPersonalizationState_NotLoaded"));
					}
					personalizationInfo2 = (BlobPersonalizationState.PersonalizationInfo)this.UserState[personalizationID];
					if (personalizationInfo2 != null && personalizationInfo2._isStatic && !personalizationInfo2.IsMatchingControlType(control))
					{
						personalizationInfo2 = null;
						this.SetControlDirty(control, personalizationID, isWebPartManager, true);
					}
					if (personalizable != null)
					{
						PersonalizationDictionary personalizationDictionary2 = this.MergeCustomProperties(personalizationInfo, personalizationInfo2, isWebPartManager, webPart, ref personalizationDictionary);
						if (personalizationDictionary2 != null)
						{
							controlInfo._allowSetDirty = true;
							personalizable.Load(personalizationDictionary2);
							controlInfo._allowSetDirty = false;
						}
					}
					if (!isWebPartManager)
					{
						IDictionary dictionary3 = null;
						IDictionary dictionary4 = null;
						if (personalizationInfo != null)
						{
							IDictionary properties = personalizationInfo._properties;
							if (properties != null && properties.Count != 0)
							{
								webPart.SetHasSharedData(true);
								dictionary3 = BlobPersonalizationState.SetPersonalizedProperties(control, personalizablePropertyEntries, properties, PersonalizationScope.Shared);
							}
						}
						dictionary = BlobPersonalizationState.GetPersonalizedProperties(control, personalizablePropertyEntries, null, null, extractScope);
						if (personalizationInfo2 != null)
						{
							IDictionary properties2 = personalizationInfo2._properties;
							if (properties2 != null && properties2.Count != 0)
							{
								webPart.SetHasUserData(true);
								dictionary4 = BlobPersonalizationState.SetPersonalizedProperties(control, personalizablePropertyEntries, properties2, extractScope);
							}
							if (trackingPersonalizable == null || !trackingPersonalizable.TracksChanges)
							{
								dictionary2 = properties2;
							}
						}
						bool flag = dictionary3 != null || dictionary4 != null;
						if (flag)
						{
							IVersioningPersonalizable versioningPersonalizable = control as IVersioningPersonalizable;
							if (versioningPersonalizable != null)
							{
								IDictionary dictionary5 = null;
								if (dictionary3 != null)
								{
									dictionary5 = dictionary3;
									if (dictionary4 == null)
									{
										goto IL_0288;
									}
									using (IDictionaryEnumerator enumerator = dictionary4.GetEnumerator())
									{
										while (enumerator.MoveNext())
										{
											object obj = enumerator.Current;
											DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
											dictionary5[dictionaryEntry.Key] = dictionaryEntry.Value;
										}
										goto IL_0288;
									}
								}
								dictionary5 = dictionary4;
								IL_0288:
								controlInfo._allowSetDirty = true;
								versioningPersonalizable.Load(dictionary5);
								controlInfo._allowSetDirty = false;
							}
							else
							{
								this.SetControlDirty(control, personalizationID, isWebPartManager, true);
							}
						}
					}
				}
				else
				{
					if (personalizable != null)
					{
						PersonalizationDictionary personalizationDictionary3 = this.MergeCustomProperties(personalizationInfo, personalizationInfo2, isWebPartManager, webPart, ref personalizationDictionary);
						if (personalizationDictionary3 != null)
						{
							controlInfo._allowSetDirty = true;
							personalizable.Load(personalizationDictionary3);
							controlInfo._allowSetDirty = false;
						}
					}
					if (!isWebPartManager)
					{
						IDictionary dictionary6 = null;
						dictionary = BlobPersonalizationState.GetPersonalizedProperties(control, personalizablePropertyEntries, null, null, extractScope);
						if (personalizationInfo != null)
						{
							IDictionary properties3 = personalizationInfo._properties;
							if (properties3 != null && properties3.Count != 0)
							{
								webPart.SetHasSharedData(true);
								dictionary6 = BlobPersonalizationState.SetPersonalizedProperties(control, personalizablePropertyEntries, properties3, PersonalizationScope.Shared);
							}
							if (trackingPersonalizable == null || !trackingPersonalizable.TracksChanges)
							{
								dictionary2 = properties3;
							}
						}
						if (dictionary6 != null)
						{
							IVersioningPersonalizable versioningPersonalizable2 = control as IVersioningPersonalizable;
							if (versioningPersonalizable2 != null)
							{
								controlInfo._allowSetDirty = true;
								versioningPersonalizable2.Load(dictionary6);
								controlInfo._allowSetDirty = false;
							}
							else
							{
								this.SetControlDirty(control, personalizationID, isWebPartManager, true);
							}
						}
					}
				}
			}
			finally
			{
				controlInfo._allowSetDirty = true;
				if (trackingPersonalizable != null)
				{
					trackingPersonalizable.EndLoad();
				}
			}
			controlInfo._control = control;
			controlInfo._personalizableProperties = personalizablePropertyEntries;
			controlInfo._defaultProperties = dictionary;
			controlInfo._initialProperties = dictionary2;
			controlInfo._customInitialProperties = personalizationDictionary;
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x00150988 File Offset: 0x0014F988
		public override void ApplyWebPartPersonalization(WebPart webPart)
		{
			base.ValidateWebPart(webPart);
			if (webPart is UnauthorizedWebPart)
			{
				return;
			}
			string text = this.CreatePersonalizationID(webPart, null);
			PersonalizationScope personalizationScope = this.PersonalizationScope;
			if (personalizationScope == PersonalizationScope.User && !webPart.IsShared)
			{
				personalizationScope = PersonalizationScope.Shared;
			}
			this.ApplyPersonalization(webPart, text, false, personalizationScope, null);
			GenericWebPart genericWebPart = webPart as GenericWebPart;
			if (genericWebPart != null)
			{
				Control childControl = genericWebPart.ChildControl;
				text = this.CreatePersonalizationID(childControl, genericWebPart);
				this.ApplyPersonalization(childControl, text, false, personalizationScope, genericWebPart);
			}
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x001509F2 File Offset: 0x0014F9F2
		public override void ApplyWebPartManagerPersonalization()
		{
			this.ApplyPersonalization(base.WebPartManager, "__wpm", true, this.PersonalizationScope, null);
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x00150A10 File Offset: 0x0014FA10
		private bool CompareProperties(IDictionary newProperties, IDictionary oldProperties)
		{
			int num = 0;
			int num2 = 0;
			if (newProperties != null)
			{
				num = newProperties.Count;
			}
			if (oldProperties != null)
			{
				num2 = oldProperties.Count;
			}
			if (num != num2)
			{
				return true;
			}
			if (num != 0)
			{
				foreach (object obj in newProperties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					object key = dictionaryEntry.Key;
					object value = dictionaryEntry.Value;
					if (!oldProperties.Contains(key))
					{
						return true;
					}
					object obj2 = oldProperties[key];
					if (!object.Equals(value, obj2))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x00150AC4 File Offset: 0x0014FAC4
		private string CreatePersonalizationID(string ID, string genericWebPartID)
		{
			if (!string.IsNullOrEmpty(genericWebPartID))
			{
				return ID + '$' + genericWebPartID;
			}
			return ID;
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x00150ADE File Offset: 0x0014FADE
		private string CreatePersonalizationID(Control control, WebPart associatedGenericWebPart)
		{
			if (associatedGenericWebPart != null)
			{
				return this.CreatePersonalizationID(control.ID, associatedGenericWebPart.ID);
			}
			return this.CreatePersonalizationID(control.ID, null);
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x00150B04 File Offset: 0x0014FB04
		private static IDictionary DeserializeData(byte[] data)
		{
			IDictionary dictionary = null;
			if (data != null && data.Length > 0)
			{
				Exception ex = null;
				int num = -1;
				object[] array = null;
				int num2 = 0;
				try
				{
					ObjectStateFormatter objectStateFormatter = new ObjectStateFormatter(null, false);
					if (HttpRuntime.NamedPermissionSet != null && HttpRuntime.ProcessRequestInApplicationTrust)
					{
						HttpRuntime.NamedPermissionSet.PermitOnly();
					}
					array = (object[])objectStateFormatter.DeserializeWithAssert(new MemoryStream(data));
					if (array != null && array.Length != 0)
					{
						num = (int)array[num2++];
					}
				}
				catch (Exception ex2)
				{
					ex = ex2;
				}
				if (num != 1)
				{
					if (num != 2)
					{
						goto IL_0256;
					}
				}
				try
				{
					int num3 = (int)array[num2++];
					if (num3 > 0)
					{
						dictionary = new HybridDictionary(num3, false);
					}
					for (int i = 0; i < num3; i++)
					{
						Type type = null;
						VirtualPath virtualPath = null;
						object obj = array[num2++];
						string text;
						bool flag;
						if (obj is string)
						{
							text = (string)obj;
							flag = false;
						}
						else
						{
							type = (Type)obj;
							if (type == typeof(UserControl))
							{
								virtualPath = VirtualPath.CreateNonRelativeAllowNull((string)array[num2++]);
							}
							text = (string)array[num2++];
							flag = true;
						}
						IDictionary dictionary2 = null;
						int num4 = (int)array[num2++];
						if (num4 > 0)
						{
							dictionary2 = new HybridDictionary(num4, false);
							for (int j = 0; j < num4; j++)
							{
								string value = ((IndexedString)array[num2++]).Value;
								object obj2 = array[num2++];
								dictionary2[value] = obj2;
							}
						}
						PersonalizationDictionary personalizationDictionary = null;
						int num5 = (int)array[num2++];
						if (num5 > 0)
						{
							personalizationDictionary = new PersonalizationDictionary(num5);
							for (int k = 0; k < num5; k++)
							{
								string value2 = ((IndexedString)array[num2++]).Value;
								object obj3 = array[num2++];
								PersonalizationScope personalizationScope = (((bool)array[num2++]) ? PersonalizationScope.Shared : PersonalizationScope.User);
								bool flag2 = false;
								if (num == 2)
								{
									flag2 = (bool)array[num2++];
								}
								personalizationDictionary[value2] = new PersonalizationEntry(obj3, personalizationScope, flag2);
							}
						}
						dictionary[text] = new BlobPersonalizationState.PersonalizationInfo
						{
							_controlID = text,
							_controlType = type,
							_controlVPath = virtualPath,
							_isStatic = flag,
							_properties = dictionary2,
							_customProperties = personalizationDictionary
						};
					}
				}
				catch (Exception ex3)
				{
					ex = ex3;
				}
				IL_0256:
				if (ex != null || (num != 1 && num != 2))
				{
					throw new ArgumentException(SR.GetString("BlobPersonalizationState_DeserializeError"), "data", ex);
				}
			}
			if (dictionary == null)
			{
				dictionary = new HybridDictionary(false);
			}
			return dictionary;
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x00150DC8 File Offset: 0x0014FDC8
		private void ExtractPersonalization(Control control, string personalizationID, bool isWebPartManager, PersonalizationScope scope, bool isStatic, GenericWebPart genericWebPart)
		{
			if (this._extractedState == null)
			{
				this._extractedState = new HybridDictionary(false);
			}
			if (this._personalizedControls == null)
			{
				throw new InvalidOperationException(SR.GetString("BlobPersonalizationState_NotApplied"));
			}
			BlobPersonalizationState.ControlInfo controlInfo = (BlobPersonalizationState.ControlInfo)this._personalizedControls[personalizationID];
			if (controlInfo == null)
			{
				throw new InvalidOperationException(SR.GetString("BlobPersonalizationState_CantExtract", new object[] { personalizationID }));
			}
			ITrackingPersonalizable trackingPersonalizable = control as ITrackingPersonalizable;
			IPersonalizable personalizable = control as IPersonalizable;
			IDictionary dictionary = controlInfo._initialProperties;
			PersonalizationDictionary personalizationDictionary = controlInfo._customInitialProperties;
			bool flag = false;
			try
			{
				if (trackingPersonalizable != null)
				{
					trackingPersonalizable.BeginSave();
				}
				if (!this.IsPostRequest)
				{
					if (controlInfo._dirty)
					{
						if (personalizable != null)
						{
							PersonalizationDictionary personalizationDictionary2 = new PersonalizationDictionary();
							personalizable.Save(personalizationDictionary2);
							if (personalizationDictionary2.Count != 0)
							{
								if (scope == PersonalizationScope.User)
								{
									personalizationDictionary2.RemoveSharedProperties();
								}
								personalizationDictionary = personalizationDictionary2;
							}
						}
						if (!isWebPartManager)
						{
							dictionary = BlobPersonalizationState.GetPersonalizedProperties(control, controlInfo._personalizableProperties, controlInfo._defaultProperties, controlInfo._initialProperties, scope);
						}
						flag = true;
					}
				}
				else
				{
					bool flag2 = true;
					bool flag3 = true;
					if (controlInfo._dirty)
					{
						flag3 = false;
					}
					else if (trackingPersonalizable != null && trackingPersonalizable.TracksChanges && !controlInfo._dirty)
					{
						flag2 = false;
					}
					if (flag2)
					{
						if (personalizable != null && (controlInfo._dirty || personalizable.IsDirty))
						{
							PersonalizationDictionary personalizationDictionary3 = new PersonalizationDictionary();
							personalizable.Save(personalizationDictionary3);
							if (personalizationDictionary3.Count != 0 || (personalizationDictionary != null && personalizationDictionary.Count != 0))
							{
								if (personalizationDictionary3.Count != 0)
								{
									if (scope == PersonalizationScope.User)
									{
										personalizationDictionary3.RemoveSharedProperties();
									}
									personalizationDictionary = personalizationDictionary3;
								}
								else
								{
									personalizationDictionary = null;
								}
								flag3 = false;
								flag = true;
							}
						}
						if (!isWebPartManager)
						{
							IDictionary personalizedProperties = BlobPersonalizationState.GetPersonalizedProperties(control, controlInfo._personalizableProperties, controlInfo._defaultProperties, controlInfo._initialProperties, scope);
							if (flag3 && !this.CompareProperties(personalizedProperties, controlInfo._initialProperties))
							{
								flag2 = false;
							}
							if (flag2)
							{
								dictionary = personalizedProperties;
								flag = true;
							}
						}
					}
				}
			}
			finally
			{
				if (trackingPersonalizable != null)
				{
					trackingPersonalizable.EndSave();
				}
			}
			BlobPersonalizationState.PersonalizationInfo personalizationInfo = new BlobPersonalizationState.PersonalizationInfo();
			personalizationInfo._controlID = personalizationID;
			if (isStatic)
			{
				UserControl userControl = control as UserControl;
				if (userControl != null)
				{
					personalizationInfo._controlType = typeof(UserControl);
					personalizationInfo._controlVPath = userControl.TemplateControlVirtualPath;
				}
				else
				{
					personalizationInfo._controlType = control.GetType();
				}
			}
			personalizationInfo._isStatic = isStatic;
			personalizationInfo._properties = dictionary;
			personalizationInfo._customProperties = personalizationDictionary;
			this._extractedState[personalizationID] = personalizationInfo;
			if (flag)
			{
				base.SetDirty();
			}
			if ((dictionary != null && dictionary.Count > 0) || (personalizationDictionary != null && personalizationDictionary.Count > 0))
			{
				WebPart webPart = null;
				if (!isWebPartManager)
				{
					if (genericWebPart != null)
					{
						webPart = genericWebPart;
					}
					else
					{
						webPart = (WebPart)control;
					}
				}
				if (webPart != null)
				{
					if (this.PersonalizationScope == PersonalizationScope.Shared)
					{
						webPart.SetHasSharedData(true);
						return;
					}
					webPart.SetHasUserData(true);
				}
			}
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x00151088 File Offset: 0x00150088
		public override void ExtractWebPartPersonalization(WebPart webPart)
		{
			base.ValidateWebPart(webPart);
			ProxyWebPart proxyWebPart = webPart as ProxyWebPart;
			if (proxyWebPart != null)
			{
				this.RoundTripWebPartPersonalization(proxyWebPart.OriginalID, proxyWebPart.GenericWebPartID);
				return;
			}
			PersonalizationScope personalizationScope = this.PersonalizationScope;
			if (personalizationScope == PersonalizationScope.User && !webPart.IsShared)
			{
				personalizationScope = PersonalizationScope.Shared;
			}
			bool isStatic = webPart.IsStatic;
			string text = this.CreatePersonalizationID(webPart, null);
			this.ExtractPersonalization(webPart, text, false, personalizationScope, isStatic, null);
			GenericWebPart genericWebPart = webPart as GenericWebPart;
			if (genericWebPart != null)
			{
				Control childControl = genericWebPart.ChildControl;
				text = this.CreatePersonalizationID(childControl, genericWebPart);
				this.ExtractPersonalization(childControl, text, false, personalizationScope, isStatic, genericWebPart);
			}
		}

		// Token: 0x0600531D RID: 21277 RVA: 0x00151117 File Offset: 0x00150117
		public override void ExtractWebPartManagerPersonalization()
		{
			this.ExtractPersonalization(base.WebPartManager, "__wpm", true, this.PersonalizationScope, true, null);
		}

		// Token: 0x0600531E RID: 21278 RVA: 0x00151133 File Offset: 0x00150133
		public override string GetAuthorizationFilter(string webPartID)
		{
			if (string.IsNullOrEmpty(webPartID))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("webPartID");
			}
			return this.GetPersonalizedValue(webPartID, "AuthorizationFilter") as string;
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x0015115C File Offset: 0x0015015C
		internal static IDictionary GetPersonalizedProperties(Control control, PersonalizationScope scope)
		{
			IDictionary personalizablePropertyEntries = PersonalizableAttribute.GetPersonalizablePropertyEntries(control.GetType());
			return BlobPersonalizationState.GetPersonalizedProperties(control, personalizablePropertyEntries, null, null, scope);
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x00151180 File Offset: 0x00150180
		private static IDictionary GetPersonalizedProperties(Control control, IDictionary personalizableProperties, IDictionary defaultPropertyState, IDictionary initialPropertyState, PersonalizationScope scope)
		{
			if (personalizableProperties.Count == 0)
			{
				return null;
			}
			bool flag = scope == PersonalizationScope.User;
			IDictionary dictionary = null;
			foreach (object obj in personalizableProperties)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				PersonalizablePropertyEntry personalizablePropertyEntry = (PersonalizablePropertyEntry)dictionaryEntry.Value;
				if (!flag || personalizablePropertyEntry.Scope != PersonalizationScope.Shared)
				{
					PropertyInfo propertyInfo = personalizablePropertyEntry.PropertyInfo;
					string text = (string)dictionaryEntry.Key;
					object property = FastPropertyAccessor.GetProperty(control, text, control.DesignMode);
					bool flag2 = true;
					if ((initialPropertyState == null || !initialPropertyState.Contains(text)) && defaultPropertyState != null)
					{
						object obj2 = defaultPropertyState[text];
						if (object.Equals(property, obj2))
						{
							flag2 = false;
						}
					}
					if (flag2)
					{
						if (dictionary == null)
						{
							dictionary = new HybridDictionary(personalizableProperties.Count, false);
						}
						dictionary[text] = property;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x00151274 File Offset: 0x00150274
		private object GetPersonalizedValue(string personalizationID, string propertyName)
		{
			if (this.SharedState == null)
			{
				throw new InvalidOperationException(SR.GetString("BlobPersonalizationState_NotLoaded"));
			}
			BlobPersonalizationState.PersonalizationInfo personalizationInfo = (BlobPersonalizationState.PersonalizationInfo)this.SharedState[personalizationID];
			IDictionary dictionary = ((personalizationInfo != null) ? personalizationInfo._properties : null);
			if (this.PersonalizationScope == PersonalizationScope.Shared)
			{
				if (dictionary != null)
				{
					return dictionary[propertyName];
				}
			}
			else
			{
				if (this.UserState == null)
				{
					throw new InvalidOperationException(SR.GetString("BlobPersonalizationState_NotLoaded"));
				}
				BlobPersonalizationState.PersonalizationInfo personalizationInfo2 = (BlobPersonalizationState.PersonalizationInfo)this.UserState[personalizationID];
				IDictionary dictionary2 = ((personalizationInfo2 != null) ? personalizationInfo2._properties : null);
				if (dictionary2 != null && dictionary2.Contains(propertyName))
				{
					return dictionary2[propertyName];
				}
				if (dictionary != null)
				{
					return dictionary[propertyName];
				}
			}
			return null;
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x00151323 File Offset: 0x00150323
		public void LoadDataBlobs(byte[] sharedData, byte[] userData)
		{
			this._sharedState = BlobPersonalizationState.DeserializeData(sharedData);
			this._rawUserData = userData;
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x00151338 File Offset: 0x00150338
		private PersonalizationDictionary MergeCustomProperties(BlobPersonalizationState.PersonalizationInfo sharedInfo, BlobPersonalizationState.PersonalizationInfo userInfo, bool isWebPartManager, WebPart hasDataWebPart, ref PersonalizationDictionary customInitialProperties)
		{
			PersonalizationDictionary personalizationDictionary = null;
			bool flag = sharedInfo != null && sharedInfo._customProperties != null;
			bool flag2 = userInfo != null && userInfo._customProperties != null;
			if (flag && flag2)
			{
				personalizationDictionary = new PersonalizationDictionary();
				foreach (object obj in sharedInfo._customProperties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					personalizationDictionary[(string)dictionaryEntry.Key] = (PersonalizationEntry)dictionaryEntry.Value;
				}
				using (IDictionaryEnumerator enumerator2 = userInfo._customProperties.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
						personalizationDictionary[(string)dictionaryEntry2.Key] = (PersonalizationEntry)dictionaryEntry2.Value;
					}
					goto IL_0105;
				}
			}
			if (flag)
			{
				personalizationDictionary = sharedInfo._customProperties;
			}
			else if (flag2)
			{
				personalizationDictionary = userInfo._customProperties;
			}
			IL_0105:
			if (this.PersonalizationScope == PersonalizationScope.Shared && flag)
			{
				customInitialProperties = sharedInfo._customProperties;
			}
			else if (this.PersonalizationScope == PersonalizationScope.User && flag2)
			{
				customInitialProperties = userInfo._customProperties;
			}
			if (flag && !isWebPartManager)
			{
				hasDataWebPart.SetHasSharedData(true);
			}
			if (flag2 && !isWebPartManager)
			{
				hasDataWebPart.SetHasUserData(true);
			}
			return personalizationDictionary;
		}

		// Token: 0x06005324 RID: 21284 RVA: 0x001514B0 File Offset: 0x001504B0
		private void RoundTripWebPartPersonalization(string ID, string genericWebPartID)
		{
			if (string.IsNullOrEmpty(ID))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("ID");
			}
			string text = this.CreatePersonalizationID(ID, genericWebPartID);
			this.RoundTripWebPartPersonalization(text);
			if (!string.IsNullOrEmpty(genericWebPartID))
			{
				string text2 = this.CreatePersonalizationID(genericWebPartID, null);
				this.RoundTripWebPartPersonalization(text2);
			}
		}

		// Token: 0x06005325 RID: 21285 RVA: 0x001514F8 File Offset: 0x001504F8
		private void RoundTripWebPartPersonalization(string personalizationID)
		{
			if (this.PersonalizationScope == PersonalizationScope.Shared)
			{
				if (this.SharedState == null)
				{
					throw new InvalidOperationException(SR.GetString("BlobPersonalizationState_NotLoaded"));
				}
				if (this.SharedState.Contains(personalizationID))
				{
					this._extractedState[personalizationID] = (BlobPersonalizationState.PersonalizationInfo)this.SharedState[personalizationID];
					return;
				}
			}
			else
			{
				if (this.UserState == null)
				{
					throw new InvalidOperationException(SR.GetString("BlobPersonalizationState_NotLoaded"));
				}
				if (this.UserState.Contains(personalizationID))
				{
					this._extractedState[personalizationID] = (BlobPersonalizationState.PersonalizationInfo)this.UserState[personalizationID];
				}
			}
		}

		// Token: 0x06005326 RID: 21286 RVA: 0x00151595 File Offset: 0x00150595
		public byte[] SaveDataBlob()
		{
			return BlobPersonalizationState.SerializeData(this._extractedState);
		}

		// Token: 0x06005327 RID: 21287 RVA: 0x001515A4 File Offset: 0x001505A4
		private static byte[] SerializeData(IDictionary data)
		{
			byte[] array = null;
			if (data == null || data.Count == 0)
			{
				return array;
			}
			ArrayList arrayList = new ArrayList();
			foreach (object obj in data)
			{
				BlobPersonalizationState.PersonalizationInfo personalizationInfo = (BlobPersonalizationState.PersonalizationInfo)((DictionaryEntry)obj).Value;
				if ((personalizationInfo._properties != null && personalizationInfo._properties.Count != 0) || (personalizationInfo._customProperties != null && personalizationInfo._customProperties.Count != 0))
				{
					arrayList.Add(personalizationInfo);
				}
			}
			if (arrayList.Count != 0)
			{
				ArrayList arrayList2 = new ArrayList();
				arrayList2.Add(2);
				arrayList2.Add(arrayList.Count);
				foreach (object obj2 in arrayList)
				{
					BlobPersonalizationState.PersonalizationInfo personalizationInfo2 = (BlobPersonalizationState.PersonalizationInfo)obj2;
					if (personalizationInfo2._isStatic)
					{
						arrayList2.Add(personalizationInfo2._controlType);
						if (personalizationInfo2._controlVPath != null)
						{
							arrayList2.Add(personalizationInfo2._controlVPath.AppRelativeVirtualPathString);
						}
					}
					arrayList2.Add(personalizationInfo2._controlID);
					int num = 0;
					if (personalizationInfo2._properties != null)
					{
						num = personalizationInfo2._properties.Count;
					}
					arrayList2.Add(num);
					if (num != 0)
					{
						foreach (object obj3 in personalizationInfo2._properties)
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
							arrayList2.Add(new IndexedString((string)dictionaryEntry.Key));
							arrayList2.Add(dictionaryEntry.Value);
						}
					}
					int num2 = 0;
					if (personalizationInfo2._customProperties != null)
					{
						num2 = personalizationInfo2._customProperties.Count;
					}
					arrayList2.Add(num2);
					if (num2 != 0)
					{
						foreach (object obj4 in personalizationInfo2._customProperties)
						{
							DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj4;
							arrayList2.Add(new IndexedString((string)dictionaryEntry2.Key));
							PersonalizationEntry personalizationEntry = (PersonalizationEntry)dictionaryEntry2.Value;
							arrayList2.Add(personalizationEntry.Value);
							arrayList2.Add(personalizationEntry.Scope == PersonalizationScope.Shared);
							arrayList2.Add(personalizationEntry.IsSensitive);
						}
					}
				}
				if (arrayList2.Count != 0)
				{
					ObjectStateFormatter objectStateFormatter = new ObjectStateFormatter(null, false);
					MemoryStream memoryStream = new MemoryStream(1024);
					object[] array2 = arrayList2.ToArray();
					if (HttpRuntime.NamedPermissionSet != null && HttpRuntime.ProcessRequestInApplicationTrust)
					{
						HttpRuntime.NamedPermissionSet.PermitOnly();
					}
					objectStateFormatter.SerializeWithAssert(memoryStream, array2);
					array = memoryStream.ToArray();
				}
			}
			return array;
		}

		// Token: 0x06005328 RID: 21288 RVA: 0x00151910 File Offset: 0x00150910
		private void SetControlDirty(Control control, string personalizationID, bool isWebPartManager, bool forceSetDirty)
		{
			if (this._personalizedControls == null)
			{
				throw new InvalidOperationException(SR.GetString("BlobPersonalizationState_NotApplied"));
			}
			BlobPersonalizationState.ControlInfo controlInfo = (BlobPersonalizationState.ControlInfo)this._personalizedControls[personalizationID];
			if (controlInfo != null && (forceSetDirty || controlInfo._allowSetDirty))
			{
				controlInfo._dirty = true;
			}
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x00151960 File Offset: 0x00150960
		internal static IDictionary SetPersonalizedProperties(Control control, IDictionary propertyState)
		{
			IDictionary personalizablePropertyEntries = PersonalizableAttribute.GetPersonalizablePropertyEntries(control.GetType());
			return BlobPersonalizationState.SetPersonalizedProperties(control, personalizablePropertyEntries, propertyState, PersonalizationScope.Shared);
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x00151984 File Offset: 0x00150984
		private static IDictionary SetPersonalizedProperties(Control control, IDictionary personalizableProperties, IDictionary propertyState, PersonalizationScope scope)
		{
			if (personalizableProperties.Count == 0)
			{
				return propertyState;
			}
			if (propertyState == null || propertyState.Count == 0)
			{
				return null;
			}
			IDictionary dictionary = null;
			foreach (object obj in propertyState)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = (string)dictionaryEntry.Key;
				object value = dictionaryEntry.Value;
				PersonalizablePropertyEntry personalizablePropertyEntry = (PersonalizablePropertyEntry)personalizableProperties[text];
				bool flag = false;
				if (personalizablePropertyEntry != null && (scope == PersonalizationScope.Shared || personalizablePropertyEntry.Scope == PersonalizationScope.User))
				{
					PropertyInfo propertyInfo = personalizablePropertyEntry.PropertyInfo;
					try
					{
						FastPropertyAccessor.SetProperty(control, text, value, control.DesignMode);
						flag = true;
					}
					catch
					{
					}
				}
				if (!flag)
				{
					if (dictionary == null)
					{
						dictionary = new HybridDictionary(propertyState.Count, false);
					}
					dictionary[text] = value;
				}
			}
			return dictionary;
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x00151A74 File Offset: 0x00150A74
		public override void SetWebPartDirty(WebPart webPart)
		{
			base.ValidateWebPart(webPart);
			string text = this.CreatePersonalizationID(webPart, null);
			this.SetControlDirty(webPart, text, false, false);
			GenericWebPart genericWebPart = webPart as GenericWebPart;
			if (genericWebPart != null)
			{
				Control childControl = genericWebPart.ChildControl;
				text = this.CreatePersonalizationID(childControl, genericWebPart);
				this.SetControlDirty(childControl, text, false, false);
			}
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x00151ABF File Offset: 0x00150ABF
		public override void SetWebPartManagerDirty()
		{
			this.SetControlDirty(base.WebPartManager, "__wpm", true, false);
		}

		// Token: 0x04002E3D RID: 11837
		private const int PersonalizationVersion = 2;

		// Token: 0x04002E3E RID: 11838
		private const string WebPartManagerPersonalizationID = "__wpm";

		// Token: 0x04002E3F RID: 11839
		private bool _isPostRequest;

		// Token: 0x04002E40 RID: 11840
		private IDictionary _personalizedControls;

		// Token: 0x04002E41 RID: 11841
		private IDictionary _sharedState;

		// Token: 0x04002E42 RID: 11842
		private IDictionary _userState;

		// Token: 0x04002E43 RID: 11843
		private byte[] _rawUserData;

		// Token: 0x04002E44 RID: 11844
		private IDictionary _extractedState;

		// Token: 0x020006A4 RID: 1700
		private sealed class PersonalizationInfo
		{
			// Token: 0x0600532D RID: 21293 RVA: 0x00151AD4 File Offset: 0x00150AD4
			public bool IsMatchingControlType(Control c)
			{
				if (c is ProxyWebPart)
				{
					return true;
				}
				if (this._controlType == null)
				{
					return false;
				}
				if (this._controlType == typeof(UserControl))
				{
					UserControl userControl = c as UserControl;
					return userControl != null && userControl.TemplateControlVirtualPath == this._controlVPath;
				}
				return this._controlType.IsAssignableFrom(c.GetType());
			}

			// Token: 0x04002E45 RID: 11845
			public Type _controlType;

			// Token: 0x04002E46 RID: 11846
			public VirtualPath _controlVPath;

			// Token: 0x04002E47 RID: 11847
			public string _controlID;

			// Token: 0x04002E48 RID: 11848
			public bool _isStatic;

			// Token: 0x04002E49 RID: 11849
			public IDictionary _properties;

			// Token: 0x04002E4A RID: 11850
			public PersonalizationDictionary _customProperties;
		}

		// Token: 0x020006A5 RID: 1701
		private sealed class ControlInfo
		{
			// Token: 0x04002E4B RID: 11851
			public Control _control;

			// Token: 0x04002E4C RID: 11852
			public IDictionary _personalizableProperties;

			// Token: 0x04002E4D RID: 11853
			public bool _dirty;

			// Token: 0x04002E4E RID: 11854
			public bool _allowSetDirty;

			// Token: 0x04002E4F RID: 11855
			public IDictionary _defaultProperties;

			// Token: 0x04002E50 RID: 11856
			public IDictionary _initialProperties;

			// Token: 0x04002E51 RID: 11857
			public PersonalizationDictionary _customInitialProperties;
		}

		// Token: 0x020006A6 RID: 1702
		private enum PersonalizationVersions
		{
			// Token: 0x04002E53 RID: 11859
			WhidbeyBeta2 = 1,
			// Token: 0x04002E54 RID: 11860
			WhidbeyRTM
		}
	}
}
