using System;
using System.Globalization;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006C3 RID: 1731
	internal class DynamicPropertyHolder
	{
		// Token: 0x06003EC3 RID: 16067 RVA: 0x000D7C40 File Offset: 0x000D6C40
		internal virtual bool AddDynamicProperty(IDynamicProperty prop)
		{
			bool flag2;
			lock (this)
			{
				DynamicPropertyHolder.CheckPropertyNameClash(prop.Name, this._props, this._numProps);
				bool flag = false;
				if (this._props == null || this._numProps == this._props.Length)
				{
					this._props = DynamicPropertyHolder.GrowPropertiesArray(this._props);
					flag = true;
				}
				this._props[this._numProps++] = prop;
				if (flag)
				{
					this._sinks = DynamicPropertyHolder.GrowDynamicSinksArray(this._sinks);
				}
				if (this._sinks == null)
				{
					this._sinks = new IDynamicMessageSink[this._props.Length];
					for (int i = 0; i < this._numProps; i++)
					{
						this._sinks[i] = ((IContributeDynamicSink)this._props[i]).GetDynamicSink();
					}
				}
				else
				{
					this._sinks[this._numProps - 1] = ((IContributeDynamicSink)prop).GetDynamicSink();
				}
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x000D7D48 File Offset: 0x000D6D48
		internal virtual bool RemoveDynamicProperty(string name)
		{
			lock (this)
			{
				for (int i = 0; i < this._numProps; i++)
				{
					if (this._props[i].Name.Equals(name))
					{
						this._props[i] = this._props[this._numProps - 1];
						this._numProps--;
						this._sinks = null;
						return true;
					}
				}
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Contexts_NoProperty"), new object[] { name }));
			}
			bool flag;
			return flag;
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06003EC5 RID: 16069 RVA: 0x000D7DF4 File Offset: 0x000D6DF4
		internal virtual IDynamicProperty[] DynamicProperties
		{
			get
			{
				if (this._props == null)
				{
					return null;
				}
				IDynamicProperty[] array2;
				lock (this)
				{
					IDynamicProperty[] array = new IDynamicProperty[this._numProps];
					Array.Copy(this._props, array, this._numProps);
					array2 = array;
				}
				return array2;
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06003EC6 RID: 16070 RVA: 0x000D7E50 File Offset: 0x000D6E50
		internal virtual ArrayWithSize DynamicSinks
		{
			get
			{
				if (this._numProps == 0)
				{
					return null;
				}
				lock (this)
				{
					if (this._sinks == null)
					{
						this._sinks = new IDynamicMessageSink[this._numProps + 8];
						for (int i = 0; i < this._numProps; i++)
						{
							this._sinks[i] = ((IContributeDynamicSink)this._props[i]).GetDynamicSink();
						}
					}
				}
				return new ArrayWithSize(this._sinks, this._numProps);
			}
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x000D7EE0 File Offset: 0x000D6EE0
		private static IDynamicMessageSink[] GrowDynamicSinksArray(IDynamicMessageSink[] sinks)
		{
			int num = ((sinks != null) ? sinks.Length : 0) + 8;
			IDynamicMessageSink[] array = new IDynamicMessageSink[num];
			if (sinks != null)
			{
				Array.Copy(sinks, array, sinks.Length);
			}
			return array;
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x000D7F10 File Offset: 0x000D6F10
		internal static void NotifyDynamicSinks(IMessage msg, ArrayWithSize dynSinks, bool bCliSide, bool bStart, bool bAsync)
		{
			for (int i = 0; i < dynSinks.Count; i++)
			{
				if (bStart)
				{
					dynSinks.Sinks[i].ProcessMessageStart(msg, bCliSide, bAsync);
				}
				else
				{
					dynSinks.Sinks[i].ProcessMessageFinish(msg, bCliSide, bAsync);
				}
			}
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x000D7F58 File Offset: 0x000D6F58
		internal static void CheckPropertyNameClash(string name, IDynamicProperty[] props, int count)
		{
			for (int i = 0; i < count; i++)
			{
				if (props[i].Name.Equals(name))
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DuplicatePropertyName"));
				}
			}
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x000D7F94 File Offset: 0x000D6F94
		internal static IDynamicProperty[] GrowPropertiesArray(IDynamicProperty[] props)
		{
			int num = ((props != null) ? props.Length : 0) + 8;
			IDynamicProperty[] array = new IDynamicProperty[num];
			if (props != null)
			{
				Array.Copy(props, array, props.Length);
			}
			return array;
		}

		// Token: 0x04001FB4 RID: 8116
		private const int GROW_BY = 8;

		// Token: 0x04001FB5 RID: 8117
		private IDynamicProperty[] _props;

		// Token: 0x04001FB6 RID: 8118
		private int _numProps;

		// Token: 0x04001FB7 RID: 8119
		private IDynamicMessageSink[] _sinks;
	}
}
