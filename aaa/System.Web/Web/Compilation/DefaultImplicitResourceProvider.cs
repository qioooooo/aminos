using System;
using System.Collections;
using System.Globalization;
using System.Resources;

namespace System.Web.Compilation
{
	// Token: 0x02000175 RID: 373
	internal class DefaultImplicitResourceProvider : IImplicitResourceProvider
	{
		// Token: 0x06001072 RID: 4210 RVA: 0x00048FFB File Offset: 0x00047FFB
		internal DefaultImplicitResourceProvider(IResourceProvider resourceProvider)
		{
			this._resourceProvider = resourceProvider;
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x0004900C File Offset: 0x0004800C
		public virtual object GetObject(ImplicitResourceKey entry, CultureInfo culture)
		{
			string text = DefaultImplicitResourceProvider.ConstructFullKey(entry);
			return this._resourceProvider.GetObject(text, culture);
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0004902D File Offset: 0x0004802D
		public virtual ICollection GetImplicitResourceKeys(string keyPrefix)
		{
			this.EnsureGetPageResources();
			if (this._implicitResources == null)
			{
				return null;
			}
			return (ICollection)this._implicitResources[keyPrefix];
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00049050 File Offset: 0x00048050
		internal void EnsureGetPageResources()
		{
			if (this._attemptedGetPageResources)
			{
				return;
			}
			this._attemptedGetPageResources = true;
			IResourceReader resourceReader = this._resourceProvider.ResourceReader;
			if (resourceReader == null)
			{
				return;
			}
			this._implicitResources = new Hashtable(StringComparer.OrdinalIgnoreCase);
			foreach (object obj in resourceReader)
			{
				ImplicitResourceKey implicitResourceKey = DefaultImplicitResourceProvider.ParseFullKey((string)((DictionaryEntry)obj).Key);
				if (implicitResourceKey != null)
				{
					ArrayList arrayList = (ArrayList)this._implicitResources[implicitResourceKey.KeyPrefix];
					if (arrayList == null)
					{
						arrayList = new ArrayList();
						this._implicitResources[implicitResourceKey.KeyPrefix] = arrayList;
					}
					arrayList.Add(implicitResourceKey);
				}
			}
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00049124 File Offset: 0x00048124
		private static ImplicitResourceKey ParseFullKey(string key)
		{
			string text = string.Empty;
			if (key.IndexOf(':') > 0)
			{
				string[] array = key.Split(new char[] { ':' });
				if (array.Length > 2)
				{
					return null;
				}
				text = array[0];
				key = array[1];
			}
			int num = key.IndexOf('.');
			if (num <= 0)
			{
				return null;
			}
			string text2 = key.Substring(0, num);
			string text3 = key.Substring(num + 1);
			return new ImplicitResourceKey
			{
				Filter = text,
				KeyPrefix = text2,
				Property = text3
			};
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x000491B0 File Offset: 0x000481B0
		private static string ConstructFullKey(ImplicitResourceKey entry)
		{
			string text = entry.KeyPrefix + "." + entry.Property;
			if (entry.Filter.Length > 0)
			{
				text = entry.Filter + ":" + text;
			}
			return text;
		}

		// Token: 0x04001655 RID: 5717
		private IResourceProvider _resourceProvider;

		// Token: 0x04001656 RID: 5718
		private IDictionary _implicitResources;

		// Token: 0x04001657 RID: 5719
		private bool _attemptedGetPageResources;
	}
}
