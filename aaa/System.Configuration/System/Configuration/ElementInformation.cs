using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x02000060 RID: 96
	public sealed class ElementInformation
	{
		// Token: 0x060003AB RID: 939 RVA: 0x00013099 File Offset: 0x00012099
		internal ElementInformation(ConfigurationElement thisElement)
		{
			this._thisElement = thisElement;
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003AC RID: 940 RVA: 0x000130A8 File Offset: 0x000120A8
		public PropertyInformationCollection Properties
		{
			get
			{
				if (this._internalProperties == null)
				{
					this._internalProperties = new PropertyInformationCollection(this._thisElement);
				}
				return this._internalProperties;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003AD RID: 941 RVA: 0x000130C9 File Offset: 0x000120C9
		public bool IsPresent
		{
			get
			{
				return this._thisElement.ElementPresent;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060003AE RID: 942 RVA: 0x000130D6 File Offset: 0x000120D6
		public bool IsLocked
		{
			get
			{
				return (this._thisElement.ItemLocked & ConfigurationValueFlags.Locked) != ConfigurationValueFlags.Default && (this._thisElement.ItemLocked & ConfigurationValueFlags.Inherited) != ConfigurationValueFlags.Default;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060003AF RID: 943 RVA: 0x000130FC File Offset: 0x000120FC
		public bool IsCollection
		{
			get
			{
				ConfigurationElementCollection configurationElementCollection = this._thisElement as ConfigurationElementCollection;
				if (configurationElementCollection == null && this._thisElement.Properties.DefaultCollectionProperty != null)
				{
					configurationElementCollection = this._thisElement[this._thisElement.Properties.DefaultCollectionProperty] as ConfigurationElementCollection;
				}
				return configurationElementCollection != null;
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00013152 File Offset: 0x00012152
		internal PropertySourceInfo PropertyInfoInternal()
		{
			return this._thisElement.PropertyInfoInternal(this._thisElement.ElementTagName);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001316A File Offset: 0x0001216A
		internal void ChangeSourceAndLineNumber(PropertySourceInfo sourceInformation)
		{
			this._thisElement.Values.ChangeSourceInfo(this._thisElement.ElementTagName, sourceInformation);
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00013188 File Offset: 0x00012188
		public string Source
		{
			get
			{
				PropertySourceInfo sourceInfo = this._thisElement.Values.GetSourceInfo(this._thisElement.ElementTagName);
				if (sourceInfo == null)
				{
					return null;
				}
				return sourceInfo.FileName;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x000131BC File Offset: 0x000121BC
		public int LineNumber
		{
			get
			{
				PropertySourceInfo sourceInfo = this._thisElement.Values.GetSourceInfo(this._thisElement.ElementTagName);
				if (sourceInfo == null)
				{
					return 0;
				}
				return sourceInfo.LineNumber;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x000131F0 File Offset: 0x000121F0
		public Type Type
		{
			get
			{
				return this._thisElement.GetType();
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x000131FD File Offset: 0x000121FD
		public ConfigurationValidatorBase Validator
		{
			get
			{
				return this._thisElement.ElementProperty.Validator;
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00013210 File Offset: 0x00012210
		private ConfigurationException[] GetReadOnlyErrorsList()
		{
			ArrayList errorsList = this._thisElement.GetErrorsList();
			int count = errorsList.Count;
			ConfigurationException[] array = new ConfigurationException[errorsList.Count];
			if (count != 0)
			{
				errorsList.CopyTo(array, 0);
			}
			return array;
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00013248 File Offset: 0x00012248
		public ICollection Errors
		{
			get
			{
				if (this._errors == null)
				{
					this._errors = this.GetReadOnlyErrorsList();
				}
				return this._errors;
			}
		}

		// Token: 0x040002EE RID: 750
		private ConfigurationElement _thisElement;

		// Token: 0x040002EF RID: 751
		private PropertyInformationCollection _internalProperties;

		// Token: 0x040002F0 RID: 752
		private ConfigurationException[] _errors;
	}
}
