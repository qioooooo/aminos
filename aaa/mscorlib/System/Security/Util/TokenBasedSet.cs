using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security.Util
{
	// Token: 0x02000472 RID: 1138
	[Serializable]
	internal class TokenBasedSet
	{
		// Token: 0x06002DBD RID: 11709 RVA: 0x0009A2BB File Offset: 0x000992BB
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this.OnDeserializedInternal();
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x0009A2C3 File Offset: 0x000992C3
		private void OnDeserializedInternal()
		{
			if (this.m_objSet != null)
			{
				if (this.m_cElt == 1)
				{
					this.m_Obj = this.m_objSet[this.m_maxIndex];
				}
				else
				{
					this.m_Set = this.m_objSet;
				}
				this.m_objSet = null;
			}
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x0009A300 File Offset: 0x00099300
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			if ((ctx.State & ~(StreamingContextStates.Clone | StreamingContextStates.CrossAppDomain)) != (StreamingContextStates)0)
			{
				if (this.m_cElt == 1)
				{
					this.m_objSet = new object[this.m_maxIndex + 1];
					this.m_objSet[this.m_maxIndex] = this.m_Obj;
					return;
				}
				if (this.m_cElt > 0)
				{
					this.m_objSet = this.m_Set;
				}
			}
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x0009A361 File Offset: 0x00099361
		[OnSerialized]
		private void OnSerialized(StreamingContext ctx)
		{
			if ((ctx.State & ~(StreamingContextStates.Clone | StreamingContextStates.CrossAppDomain)) != (StreamingContextStates)0)
			{
				this.m_objSet = null;
			}
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x0009A37C File Offset: 0x0009937C
		internal bool MoveNext(ref TokenBasedSetEnumerator e)
		{
			switch (this.m_cElt)
			{
			case 0:
				return false;
			case 1:
				if (e.Index == -1)
				{
					e.Index = this.m_maxIndex;
					e.Current = this.m_Obj;
					return true;
				}
				e.Index = (int)((short)(this.m_maxIndex + 1));
				e.Current = null;
				return false;
			default:
				while (++e.Index <= this.m_maxIndex)
				{
					e.Current = this.m_Set[e.Index];
					if (e.Current != null)
					{
						return true;
					}
				}
				e.Current = null;
				return false;
			}
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x0009A41B File Offset: 0x0009941B
		internal TokenBasedSet()
		{
			this.Reset();
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x0009A438 File Offset: 0x00099438
		internal TokenBasedSet(TokenBasedSet tbSet)
		{
			if (tbSet == null)
			{
				this.Reset();
				return;
			}
			if (tbSet.m_cElt > 1)
			{
				object[] set = tbSet.m_Set;
				int num = set.Length;
				object[] array = new object[num];
				Array.Copy(set, 0, array, 0, num);
				this.m_Set = array;
			}
			else
			{
				this.m_Obj = tbSet.m_Obj;
			}
			this.m_cElt = tbSet.m_cElt;
			this.m_maxIndex = tbSet.m_maxIndex;
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x0009A4B6 File Offset: 0x000994B6
		internal void Reset()
		{
			this.m_Obj = null;
			this.m_Set = null;
			this.m_cElt = 0;
			this.m_maxIndex = -1;
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x0009A4D4 File Offset: 0x000994D4
		internal void SetItem(int index, object item)
		{
			if (item == null)
			{
				this.RemoveItem(index);
				return;
			}
			switch (this.m_cElt)
			{
			case 0:
				this.m_cElt = 1;
				this.m_maxIndex = (int)((short)index);
				this.m_Obj = item;
				return;
			case 1:
			{
				if (index == this.m_maxIndex)
				{
					this.m_Obj = item;
					return;
				}
				object obj = this.m_Obj;
				int num = Math.Max(this.m_maxIndex, index);
				object[] array = new object[num + 1];
				array[this.m_maxIndex] = obj;
				array[index] = item;
				this.m_maxIndex = (int)((short)num);
				this.m_cElt = 2;
				this.m_Set = array;
				this.m_Obj = null;
				return;
			}
			default:
			{
				object[] array = this.m_Set;
				if (index >= array.Length)
				{
					object[] array2 = new object[index + 1];
					Array.Copy(array, 0, array2, 0, this.m_maxIndex + 1);
					this.m_maxIndex = (int)((short)index);
					array2[index] = item;
					this.m_Set = array2;
					this.m_cElt++;
					return;
				}
				if (array[index] == null)
				{
					this.m_cElt++;
				}
				array[index] = item;
				if (index > this.m_maxIndex)
				{
					this.m_maxIndex = (int)((short)index);
				}
				return;
			}
			}
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x0009A5EC File Offset: 0x000995EC
		internal object GetItem(int index)
		{
			switch (this.m_cElt)
			{
			case 0:
				return null;
			case 1:
				if (index == this.m_maxIndex)
				{
					return this.m_Obj;
				}
				return null;
			default:
				if (index < this.m_Set.Length)
				{
					return this.m_Set[index];
				}
				return null;
			}
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x0009A63C File Offset: 0x0009963C
		internal object RemoveItem(int index)
		{
			object obj = null;
			switch (this.m_cElt)
			{
			case 0:
				obj = null;
				break;
			case 1:
				if (index != this.m_maxIndex)
				{
					obj = null;
				}
				else
				{
					obj = this.m_Obj;
					this.Reset();
				}
				break;
			default:
				if (index < this.m_Set.Length && this.m_Set[index] != null)
				{
					obj = this.m_Set[index];
					this.m_Set[index] = null;
					this.m_cElt--;
					if (index == this.m_maxIndex)
					{
						this.ResetMaxIndex(this.m_Set);
					}
					if (this.m_cElt == 1)
					{
						this.m_Obj = this.m_Set[this.m_maxIndex];
						this.m_Set = null;
					}
				}
				break;
			}
			return obj;
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x0009A6F4 File Offset: 0x000996F4
		private void ResetMaxIndex(object[] aObj)
		{
			for (int i = aObj.Length - 1; i >= 0; i--)
			{
				if (aObj[i] != null)
				{
					this.m_maxIndex = (int)((short)i);
					return;
				}
			}
			this.m_maxIndex = -1;
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x0009A726 File Offset: 0x00099726
		internal int GetStartingIndex()
		{
			if (this.m_cElt <= 1)
			{
				return this.m_maxIndex;
			}
			return 0;
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x0009A739 File Offset: 0x00099739
		internal int GetCount()
		{
			return this.m_cElt;
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x0009A741 File Offset: 0x00099741
		internal int GetMaxUsedIndex()
		{
			return this.m_maxIndex;
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x0009A749 File Offset: 0x00099749
		internal bool FastIsEmpty()
		{
			return this.m_cElt == 0;
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x0009A754 File Offset: 0x00099754
		internal TokenBasedSet SpecialUnion(TokenBasedSet other, ref bool canUnrestrictedOverride)
		{
			this.OnDeserializedInternal();
			TokenBasedSet tokenBasedSet = new TokenBasedSet();
			int num;
			if (other != null)
			{
				other.OnDeserializedInternal();
				num = ((this.GetMaxUsedIndex() > other.GetMaxUsedIndex()) ? this.GetMaxUsedIndex() : other.GetMaxUsedIndex());
			}
			else
			{
				num = this.GetMaxUsedIndex();
			}
			for (int i = 0; i <= num; i++)
			{
				object item = this.GetItem(i);
				IPermission permission = item as IPermission;
				ISecurityElementFactory securityElementFactory = item as ISecurityElementFactory;
				object obj = ((other != null) ? other.GetItem(i) : null);
				IPermission permission2 = obj as IPermission;
				ISecurityElementFactory securityElementFactory2 = obj as ISecurityElementFactory;
				if (item != null || obj != null)
				{
					if (item == null)
					{
						if (securityElementFactory2 != null)
						{
							permission2 = PermissionSet.CreatePerm(securityElementFactory2, false);
						}
						PermissionToken token = PermissionToken.GetToken(permission2);
						if (token == null)
						{
							throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
						}
						tokenBasedSet.SetItem(token.m_index, permission2);
						if (!CodeAccessPermission.CanUnrestrictedOverride(permission2))
						{
							canUnrestrictedOverride = false;
						}
					}
					else if (obj == null)
					{
						if (securityElementFactory != null)
						{
							permission = PermissionSet.CreatePerm(securityElementFactory, false);
						}
						PermissionToken token2 = PermissionToken.GetToken(permission);
						if (token2 == null)
						{
							throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
						}
						tokenBasedSet.SetItem(token2.m_index, permission);
						if (!CodeAccessPermission.CanUnrestrictedOverride(permission))
						{
							canUnrestrictedOverride = false;
						}
					}
				}
			}
			return tokenBasedSet;
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x0009A888 File Offset: 0x00099888
		internal void SpecialSplit(ref TokenBasedSet unrestrictedPermSet, ref TokenBasedSet normalPermSet, bool ignoreTypeLoadFailures)
		{
			int maxUsedIndex = this.GetMaxUsedIndex();
			for (int i = this.GetStartingIndex(); i <= maxUsedIndex; i++)
			{
				object item = this.GetItem(i);
				if (item != null)
				{
					IPermission permission = item as IPermission;
					if (permission == null)
					{
						permission = PermissionSet.CreatePerm(item, ignoreTypeLoadFailures);
					}
					PermissionToken token = PermissionToken.GetToken(permission);
					if (permission != null && token != null)
					{
						if (permission is IUnrestrictedPermission)
						{
							if (unrestrictedPermSet == null)
							{
								unrestrictedPermSet = new TokenBasedSet();
							}
							unrestrictedPermSet.SetItem(token.m_index, permission);
						}
						else
						{
							if (normalPermSet == null)
							{
								normalPermSet = new TokenBasedSet();
							}
							normalPermSet.SetItem(token.m_index, permission);
						}
					}
				}
			}
		}

		// Token: 0x0400175F RID: 5983
		private int m_initSize = 24;

		// Token: 0x04001760 RID: 5984
		private int m_increment = 8;

		// Token: 0x04001761 RID: 5985
		private object[] m_objSet;

		// Token: 0x04001762 RID: 5986
		[OptionalField(VersionAdded = 2)]
		private object m_Obj;

		// Token: 0x04001763 RID: 5987
		[OptionalField(VersionAdded = 2)]
		private object[] m_Set;

		// Token: 0x04001764 RID: 5988
		private int m_cElt;

		// Token: 0x04001765 RID: 5989
		private int m_maxIndex;
	}
}
