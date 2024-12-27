using System;
using System.Collections;
using System.Diagnostics;

namespace System
{
	// Token: 0x02000084 RID: 132
	internal class ConfigNode
	{
		// Token: 0x0600075B RID: 1883 RVA: 0x0001814F File Offset: 0x0001714F
		internal ConfigNode(string name, ConfigNode parent)
		{
			this.m_name = name;
			this.m_parent = parent;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0001817D File Offset: 0x0001717D
		internal string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x00018185 File Offset: 0x00017185
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x0001818D File Offset: 0x0001718D
		internal string Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x00018196 File Offset: 0x00017196
		internal ConfigNode Parent
		{
			get
			{
				return this.m_parent;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0001819E File Offset: 0x0001719E
		internal ArrayList Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x000181A6 File Offset: 0x000171A6
		internal ArrayList Attributes
		{
			get
			{
				return this.m_attributes;
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x000181AE File Offset: 0x000171AE
		internal void AddChild(ConfigNode child)
		{
			child.m_parent = this;
			this.m_children.Add(child);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000181C4 File Offset: 0x000171C4
		internal int AddAttribute(string key, string value)
		{
			this.m_attributes.Add(new DictionaryEntry(key, value));
			return this.m_attributes.Count - 1;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x000181EB File Offset: 0x000171EB
		internal void ReplaceAttribute(int index, string key, string value)
		{
			this.m_attributes[index] = new DictionaryEntry(key, value);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00018208 File Offset: 0x00017208
		[Conditional("_LOGGING")]
		internal void Trace()
		{
			string value = this.m_value;
			ConfigNode parent = this.m_parent;
			for (int i = 0; i < this.m_attributes.Count; i++)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)this.m_attributes[i];
			}
			for (int j = 0; j < this.m_children.Count; j++)
			{
			}
		}

		// Token: 0x04000282 RID: 642
		private string m_name;

		// Token: 0x04000283 RID: 643
		private string m_value;

		// Token: 0x04000284 RID: 644
		private ConfigNode m_parent;

		// Token: 0x04000285 RID: 645
		private ArrayList m_children = new ArrayList(5);

		// Token: 0x04000286 RID: 646
		private ArrayList m_attributes = new ArrayList(5);
	}
}
