using System;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x0200034F RID: 847
	internal sealed class ObjectHolder
	{
		// Token: 0x060021EA RID: 8682 RVA: 0x0005637A File Offset: 0x0005537A
		internal ObjectHolder(long objID)
			: this(null, objID, null, null, 0L, null, null)
		{
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x0005638C File Offset: 0x0005538C
		internal ObjectHolder(object obj, long objID, SerializationInfo info, ISerializationSurrogate surrogate, long idOfContainingObj, FieldInfo field, int[] arrayIndex)
		{
			this.m_object = obj;
			this.m_id = objID;
			this.m_flags = 0;
			this.m_missingElementsRemaining = 0;
			this.m_missingDecendents = 0;
			this.m_dependentObjects = null;
			this.m_next = null;
			this.m_serInfo = info;
			this.m_surrogate = surrogate;
			this.m_markForFixupWhenAvailable = false;
			if (obj is TypeLoadExceptionHolder)
			{
				this.m_typeLoad = (TypeLoadExceptionHolder)obj;
			}
			if (idOfContainingObj != 0L && ((field != null && field.FieldType.IsValueType) || arrayIndex != null))
			{
				if (idOfContainingObj == objID)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_ParentChildIdentical"));
				}
				this.m_valueFixup = new ValueTypeFixupInfo(idOfContainingObj, field, arrayIndex);
			}
			this.SetFlags();
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x00056444 File Offset: 0x00055444
		internal ObjectHolder(string obj, long objID, SerializationInfo info, ISerializationSurrogate surrogate, long idOfContainingObj, FieldInfo field, int[] arrayIndex)
		{
			this.m_object = obj;
			this.m_id = objID;
			this.m_flags = 0;
			this.m_missingElementsRemaining = 0;
			this.m_missingDecendents = 0;
			this.m_dependentObjects = null;
			this.m_next = null;
			this.m_serInfo = info;
			this.m_surrogate = surrogate;
			this.m_markForFixupWhenAvailable = false;
			if (idOfContainingObj != 0L && arrayIndex != null)
			{
				this.m_valueFixup = new ValueTypeFixupInfo(idOfContainingObj, field, arrayIndex);
			}
			if (this.m_valueFixup != null)
			{
				this.m_flags |= 8;
			}
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x000564CF File Offset: 0x000554CF
		private void IncrementDescendentFixups(int amount)
		{
			this.m_missingDecendents += amount;
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x000564DF File Offset: 0x000554DF
		internal void DecrementFixupsRemaining(ObjectManager manager)
		{
			this.m_missingElementsRemaining--;
			if (this.RequiresValueTypeFixup)
			{
				this.UpdateDescendentDependencyChain(-1, manager);
			}
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000564FF File Offset: 0x000554FF
		internal void RemoveDependency(long id)
		{
			this.m_dependentObjects.RemoveElement(id);
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x0005650E File Offset: 0x0005550E
		internal void AddFixup(FixupHolder fixup, ObjectManager manager)
		{
			if (this.m_missingElements == null)
			{
				this.m_missingElements = new FixupHolderList();
			}
			this.m_missingElements.Add(fixup);
			this.m_missingElementsRemaining++;
			if (this.RequiresValueTypeFixup)
			{
				this.UpdateDescendentDependencyChain(1, manager);
			}
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x00056550 File Offset: 0x00055550
		private void UpdateDescendentDependencyChain(int amount, ObjectManager manager)
		{
			ObjectHolder objectHolder = this;
			do
			{
				objectHolder = manager.FindOrCreateObjectHolder(objectHolder.ContainerID);
				objectHolder.IncrementDescendentFixups(amount);
			}
			while (objectHolder.RequiresValueTypeFixup);
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x0005657B File Offset: 0x0005557B
		internal void AddDependency(long dependentObject)
		{
			if (this.m_dependentObjects == null)
			{
				this.m_dependentObjects = new LongList();
			}
			this.m_dependentObjects.Add(dependentObject);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x0005659C File Offset: 0x0005559C
		internal void UpdateData(object obj, SerializationInfo info, ISerializationSurrogate surrogate, long idOfContainer, FieldInfo field, int[] arrayIndex, ObjectManager manager)
		{
			this.SetObjectValue(obj, manager);
			this.m_serInfo = info;
			this.m_surrogate = surrogate;
			if (idOfContainer != 0L && ((field != null && field.FieldType.IsValueType) || arrayIndex != null))
			{
				if (idOfContainer == this.m_id)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_ParentChildIdentical"));
				}
				this.m_valueFixup = new ValueTypeFixupInfo(idOfContainer, field, arrayIndex);
			}
			this.SetFlags();
			if (this.RequiresValueTypeFixup)
			{
				this.UpdateDescendentDependencyChain(this.m_missingElementsRemaining, manager);
			}
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00056623 File Offset: 0x00055623
		internal void MarkForCompletionWhenAvailable()
		{
			this.m_markForFixupWhenAvailable = true;
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x0005662C File Offset: 0x0005562C
		internal void SetFlags()
		{
			if (this.m_object is IObjectReference)
			{
				this.m_flags |= 1;
			}
			this.m_flags &= -7;
			if (this.m_surrogate != null)
			{
				this.m_flags |= 4;
			}
			else if (this.m_object is ISerializable)
			{
				this.m_flags |= 2;
			}
			if (this.m_valueFixup != null)
			{
				this.m_flags |= 8;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x060021F6 RID: 8694 RVA: 0x000566AC File Offset: 0x000556AC
		// (set) Token: 0x060021F7 RID: 8695 RVA: 0x000566BC File Offset: 0x000556BC
		internal bool IsIncompleteObjectReference
		{
			get
			{
				return (this.m_flags & 1) != 0;
			}
			set
			{
				if (value)
				{
					this.m_flags |= 1;
					return;
				}
				this.m_flags &= -2;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060021F8 RID: 8696 RVA: 0x000566DF File Offset: 0x000556DF
		internal bool RequiresDelayedFixup
		{
			get
			{
				return (this.m_flags & 7) != 0;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060021F9 RID: 8697 RVA: 0x000566EF File Offset: 0x000556EF
		internal bool RequiresValueTypeFixup
		{
			get
			{
				return (this.m_flags & 8) != 0;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060021FA RID: 8698 RVA: 0x000566FF File Offset: 0x000556FF
		// (set) Token: 0x060021FB RID: 8699 RVA: 0x00056733 File Offset: 0x00055733
		internal bool ValueTypeFixupPerformed
		{
			get
			{
				return (this.m_flags & 32768) != 0 || (this.m_object != null && (this.m_dependentObjects == null || this.m_dependentObjects.Count == 0));
			}
			set
			{
				if (value)
				{
					this.m_flags |= 32768;
				}
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060021FC RID: 8700 RVA: 0x0005674A File Offset: 0x0005574A
		internal bool HasISerializable
		{
			get
			{
				return (this.m_flags & 2) != 0;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060021FD RID: 8701 RVA: 0x0005675A File Offset: 0x0005575A
		internal bool HasSurrogate
		{
			get
			{
				return (this.m_flags & 4) != 0;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060021FE RID: 8702 RVA: 0x0005676A File Offset: 0x0005576A
		internal bool CanSurrogatedObjectValueChange
		{
			get
			{
				return this.m_surrogate == null || this.m_surrogate.GetType() != typeof(SurrogateForCyclicalReference);
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x00056790 File Offset: 0x00055790
		internal bool CanObjectValueChange
		{
			get
			{
				return this.IsIncompleteObjectReference || (this.HasSurrogate && this.CanSurrogatedObjectValueChange);
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06002200 RID: 8704 RVA: 0x000567AC File Offset: 0x000557AC
		internal int DirectlyDependentObjects
		{
			get
			{
				return this.m_missingElementsRemaining;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06002201 RID: 8705 RVA: 0x000567B4 File Offset: 0x000557B4
		internal int TotalDependentObjects
		{
			get
			{
				return this.m_missingElementsRemaining + this.m_missingDecendents;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06002202 RID: 8706 RVA: 0x000567C3 File Offset: 0x000557C3
		// (set) Token: 0x06002203 RID: 8707 RVA: 0x000567CB File Offset: 0x000557CB
		internal bool Reachable
		{
			get
			{
				return this.m_reachable;
			}
			set
			{
				this.m_reachable = value;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06002204 RID: 8708 RVA: 0x000567D4 File Offset: 0x000557D4
		internal bool TypeLoadExceptionReachable
		{
			get
			{
				return this.m_typeLoad != null;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06002205 RID: 8709 RVA: 0x000567E2 File Offset: 0x000557E2
		// (set) Token: 0x06002206 RID: 8710 RVA: 0x000567EA File Offset: 0x000557EA
		internal TypeLoadExceptionHolder TypeLoadException
		{
			get
			{
				return this.m_typeLoad;
			}
			set
			{
				this.m_typeLoad = value;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06002207 RID: 8711 RVA: 0x000567F3 File Offset: 0x000557F3
		internal object ObjectValue
		{
			get
			{
				return this.m_object;
			}
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000567FB File Offset: 0x000557FB
		internal void SetObjectValue(object obj, ObjectManager manager)
		{
			this.m_object = obj;
			if (obj == manager.TopObject)
			{
				this.m_reachable = true;
			}
			if (obj is TypeLoadExceptionHolder)
			{
				this.m_typeLoad = (TypeLoadExceptionHolder)obj;
			}
			if (this.m_markForFixupWhenAvailable)
			{
				manager.CompleteObject(this, true);
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x00056838 File Offset: 0x00055838
		// (set) Token: 0x0600220A RID: 8714 RVA: 0x00056840 File Offset: 0x00055840
		internal SerializationInfo SerializationInfo
		{
			get
			{
				return this.m_serInfo;
			}
			set
			{
				this.m_serInfo = value;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x00056849 File Offset: 0x00055849
		internal ISerializationSurrogate Surrogate
		{
			get
			{
				return this.m_surrogate;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600220C RID: 8716 RVA: 0x00056851 File Offset: 0x00055851
		// (set) Token: 0x0600220D RID: 8717 RVA: 0x00056859 File Offset: 0x00055859
		internal LongList DependentObjects
		{
			get
			{
				return this.m_dependentObjects;
			}
			set
			{
				this.m_dependentObjects = value;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x0600220E RID: 8718 RVA: 0x00056862 File Offset: 0x00055862
		// (set) Token: 0x0600220F RID: 8719 RVA: 0x00056889 File Offset: 0x00055889
		internal bool RequiresSerInfoFixup
		{
			get
			{
				return ((this.m_flags & 4) != 0 || (this.m_flags & 2) != 0) && (this.m_flags & 16384) == 0;
			}
			set
			{
				if (!value)
				{
					this.m_flags |= 16384;
					return;
				}
				this.m_flags &= -16385;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06002210 RID: 8720 RVA: 0x000568B3 File Offset: 0x000558B3
		internal ValueTypeFixupInfo ValueFixup
		{
			get
			{
				return this.m_valueFixup;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06002211 RID: 8721 RVA: 0x000568BB File Offset: 0x000558BB
		internal bool CompletelyFixed
		{
			get
			{
				return !this.RequiresSerInfoFixup && !this.IsIncompleteObjectReference;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002212 RID: 8722 RVA: 0x000568D0 File Offset: 0x000558D0
		internal long ContainerID
		{
			get
			{
				if (this.m_valueFixup != null)
				{
					return this.m_valueFixup.ContainerID;
				}
				return 0L;
			}
		}

		// Token: 0x04000E0C RID: 3596
		internal const int INCOMPLETE_OBJECT_REFERENCE = 1;

		// Token: 0x04000E0D RID: 3597
		internal const int HAS_ISERIALIZABLE = 2;

		// Token: 0x04000E0E RID: 3598
		internal const int HAS_SURROGATE = 4;

		// Token: 0x04000E0F RID: 3599
		internal const int REQUIRES_VALUETYPE_FIXUP = 8;

		// Token: 0x04000E10 RID: 3600
		internal const int REQUIRES_DELAYED_FIXUP = 7;

		// Token: 0x04000E11 RID: 3601
		internal const int SER_INFO_FIXED = 16384;

		// Token: 0x04000E12 RID: 3602
		internal const int VALUETYPE_FIXUP_PERFORMED = 32768;

		// Token: 0x04000E13 RID: 3603
		private object m_object;

		// Token: 0x04000E14 RID: 3604
		internal long m_id;

		// Token: 0x04000E15 RID: 3605
		private int m_missingElementsRemaining;

		// Token: 0x04000E16 RID: 3606
		private int m_missingDecendents;

		// Token: 0x04000E17 RID: 3607
		internal SerializationInfo m_serInfo;

		// Token: 0x04000E18 RID: 3608
		internal ISerializationSurrogate m_surrogate;

		// Token: 0x04000E19 RID: 3609
		internal FixupHolderList m_missingElements;

		// Token: 0x04000E1A RID: 3610
		internal LongList m_dependentObjects;

		// Token: 0x04000E1B RID: 3611
		internal ObjectHolder m_next;

		// Token: 0x04000E1C RID: 3612
		internal int m_flags;

		// Token: 0x04000E1D RID: 3613
		private bool m_markForFixupWhenAvailable;

		// Token: 0x04000E1E RID: 3614
		private ValueTypeFixupInfo m_valueFixup;

		// Token: 0x04000E1F RID: 3615
		private TypeLoadExceptionHolder m_typeLoad;

		// Token: 0x04000E20 RID: 3616
		private bool m_reachable;
	}
}
