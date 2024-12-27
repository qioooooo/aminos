using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008F1 RID: 2289
	public abstract class CommonAcl : GenericAcl
	{
		// Token: 0x06005345 RID: 21317 RVA: 0x0012E214 File Offset: 0x0012D214
		static CommonAcl()
		{
			for (int i = 0; i < CommonAcl.AFtoPM.Length; i++)
			{
				CommonAcl.AFtoPM[i] = CommonAcl.PM.GO;
			}
			CommonAcl.AFtoPM[0] = CommonAcl.PM.F;
			CommonAcl.AFtoPM[4] = CommonAcl.PM.F | CommonAcl.PM.CO | CommonAcl.PM.GO;
			CommonAcl.AFtoPM[5] = CommonAcl.PM.F | CommonAcl.PM.CO;
			CommonAcl.AFtoPM[6] = CommonAcl.PM.CO | CommonAcl.PM.GO;
			CommonAcl.AFtoPM[7] = CommonAcl.PM.CO;
			CommonAcl.AFtoPM[8] = CommonAcl.PM.F | CommonAcl.PM.CF | CommonAcl.PM.GF;
			CommonAcl.AFtoPM[9] = CommonAcl.PM.F | CommonAcl.PM.CF;
			CommonAcl.AFtoPM[10] = CommonAcl.PM.CF | CommonAcl.PM.GF;
			CommonAcl.AFtoPM[11] = CommonAcl.PM.CF;
			CommonAcl.AFtoPM[12] = CommonAcl.PM.F | CommonAcl.PM.CF | CommonAcl.PM.CO | CommonAcl.PM.GF | CommonAcl.PM.GO;
			CommonAcl.AFtoPM[13] = CommonAcl.PM.F | CommonAcl.PM.CF | CommonAcl.PM.CO;
			CommonAcl.AFtoPM[14] = CommonAcl.PM.CF | CommonAcl.PM.CO | CommonAcl.PM.GF | CommonAcl.PM.GO;
			CommonAcl.AFtoPM[15] = CommonAcl.PM.CF | CommonAcl.PM.CO;
			CommonAcl.PMtoAF = new CommonAcl.AF[32];
			for (int j = 0; j < CommonAcl.PMtoAF.Length; j++)
			{
				CommonAcl.PMtoAF[j] = CommonAcl.AF.NP;
			}
			CommonAcl.PMtoAF[16] = (CommonAcl.AF)0;
			CommonAcl.PMtoAF[21] = CommonAcl.AF.OI;
			CommonAcl.PMtoAF[20] = CommonAcl.AF.OI | CommonAcl.AF.NP;
			CommonAcl.PMtoAF[5] = CommonAcl.AF.OI | CommonAcl.AF.IO;
			CommonAcl.PMtoAF[4] = CommonAcl.AF.OI | CommonAcl.AF.IO | CommonAcl.AF.NP;
			CommonAcl.PMtoAF[26] = CommonAcl.AF.CI;
			CommonAcl.PMtoAF[24] = CommonAcl.AF.CI | CommonAcl.AF.NP;
			CommonAcl.PMtoAF[10] = CommonAcl.AF.CI | CommonAcl.AF.IO;
			CommonAcl.PMtoAF[8] = CommonAcl.AF.CI | CommonAcl.AF.IO | CommonAcl.AF.NP;
			CommonAcl.PMtoAF[31] = CommonAcl.AF.CI | CommonAcl.AF.OI;
			CommonAcl.PMtoAF[28] = CommonAcl.AF.CI | CommonAcl.AF.OI | CommonAcl.AF.NP;
			CommonAcl.PMtoAF[15] = CommonAcl.AF.CI | CommonAcl.AF.OI | CommonAcl.AF.IO;
			CommonAcl.PMtoAF[12] = CommonAcl.AF.CI | CommonAcl.AF.OI | CommonAcl.AF.IO | CommonAcl.AF.NP;
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x0012E360 File Offset: 0x0012D360
		private static CommonAcl.AF AFFromAceFlags(AceFlags aceFlags, bool isDS)
		{
			CommonAcl.AF af = (CommonAcl.AF)0;
			if ((byte)(aceFlags & AceFlags.ContainerInherit) != 0)
			{
				af |= CommonAcl.AF.CI;
			}
			if (!isDS && (byte)(aceFlags & AceFlags.ObjectInherit) != 0)
			{
				af |= CommonAcl.AF.OI;
			}
			if ((byte)(aceFlags & AceFlags.InheritOnly) != 0)
			{
				af |= CommonAcl.AF.IO;
			}
			if ((byte)(aceFlags & AceFlags.NoPropagateInherit) != 0)
			{
				af |= CommonAcl.AF.NP;
			}
			return af;
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x0012E39C File Offset: 0x0012D39C
		private static AceFlags AceFlagsFromAF(CommonAcl.AF af, bool isDS)
		{
			AceFlags aceFlags = AceFlags.None;
			if ((af & CommonAcl.AF.CI) != (CommonAcl.AF)0)
			{
				aceFlags |= AceFlags.ContainerInherit;
			}
			if (!isDS && (af & CommonAcl.AF.OI) != (CommonAcl.AF)0)
			{
				aceFlags |= AceFlags.ObjectInherit;
			}
			if ((af & CommonAcl.AF.IO) != (CommonAcl.AF)0)
			{
				aceFlags |= AceFlags.InheritOnly;
			}
			if ((af & CommonAcl.AF.NP) != (CommonAcl.AF)0)
			{
				aceFlags |= AceFlags.NoPropagateInherit;
			}
			return aceFlags;
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x0012E3D8 File Offset: 0x0012D3D8
		private static bool MergeInheritanceBits(AceFlags left, AceFlags right, bool isDS, out AceFlags result)
		{
			result = AceFlags.None;
			CommonAcl.AF af = CommonAcl.AFFromAceFlags(left, isDS);
			CommonAcl.AF af2 = CommonAcl.AFFromAceFlags(right, isDS);
			CommonAcl.PM pm = CommonAcl.AFtoPM[(int)af];
			CommonAcl.PM pm2 = CommonAcl.AFtoPM[(int)af2];
			if (pm == CommonAcl.PM.GO || pm2 == CommonAcl.PM.GO)
			{
				return false;
			}
			CommonAcl.PM pm3 = pm | pm2;
			CommonAcl.AF af3 = CommonAcl.PMtoAF[(int)pm3];
			if (af3 == CommonAcl.AF.NP)
			{
				return false;
			}
			result = CommonAcl.AceFlagsFromAF(af3, isDS);
			return true;
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x0012E434 File Offset: 0x0012D434
		private static bool RemoveInheritanceBits(AceFlags existing, AceFlags remove, bool isDS, out AceFlags result, out bool total)
		{
			result = AceFlags.None;
			total = false;
			CommonAcl.AF af = CommonAcl.AFFromAceFlags(existing, isDS);
			CommonAcl.AF af2 = CommonAcl.AFFromAceFlags(remove, isDS);
			CommonAcl.PM pm = CommonAcl.AFtoPM[(int)af];
			CommonAcl.PM pm2 = CommonAcl.AFtoPM[(int)af2];
			if (pm == CommonAcl.PM.GO || pm2 == CommonAcl.PM.GO)
			{
				return false;
			}
			CommonAcl.PM pm3 = pm & ~pm2;
			if (pm3 == (CommonAcl.PM)0)
			{
				total = true;
				return true;
			}
			CommonAcl.AF af3 = CommonAcl.PMtoAF[(int)pm3];
			if (af3 == CommonAcl.AF.NP)
			{
				return false;
			}
			result = CommonAcl.AceFlagsFromAF(af3, isDS);
			return true;
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x0012E49E File Offset: 0x0012D49E
		private void CanonicalizeIfNecessary()
		{
			if (this._isDirty)
			{
				this.Canonicalize(false, this is DiscretionaryAcl);
				this._isDirty = false;
			}
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x0012E4C0 File Offset: 0x0012D4C0
		private static int DaclAcePriority(GenericAce ace)
		{
			AceType aceType = ace.AceType;
			int num;
			if ((byte)(ace.AceFlags & AceFlags.Inherited) != 0)
			{
				num = 131070 + (int)ace._indexInAcl;
			}
			else if (aceType == AceType.AccessDenied || aceType == AceType.AccessDeniedCallback)
			{
				num = 0;
			}
			else if (aceType == AceType.AccessDeniedObject || aceType == AceType.AccessDeniedCallbackObject)
			{
				num = 1;
			}
			else if (aceType == AceType.AccessAllowed || aceType == AceType.AccessAllowedCallback)
			{
				num = 2;
			}
			else if (aceType == AceType.AccessAllowedObject || aceType == AceType.AccessAllowedCallbackObject)
			{
				num = 3;
			}
			else
			{
				num = (int)(ushort.MaxValue + ace._indexInAcl);
			}
			return num;
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x0012E530 File Offset: 0x0012D530
		private static int SaclAcePriority(GenericAce ace)
		{
			AceType aceType = ace.AceType;
			int num;
			if ((byte)(ace.AceFlags & AceFlags.Inherited) != 0)
			{
				num = 131070 + (int)ace._indexInAcl;
			}
			else if (aceType == AceType.SystemAudit || aceType == AceType.SystemAlarm || aceType == AceType.SystemAuditCallback || aceType == AceType.SystemAlarmCallback)
			{
				num = 0;
			}
			else if (aceType == AceType.SystemAuditObject || aceType == AceType.SystemAlarmObject || aceType == AceType.SystemAuditCallbackObject || aceType == AceType.SystemAlarmCallbackObject)
			{
				num = 1;
			}
			else
			{
				num = (int)(ushort.MaxValue + ace._indexInAcl);
			}
			return num;
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x0012E59C File Offset: 0x0012D59C
		private static CommonAcl.ComparisonResult CompareAces(GenericAce ace1, GenericAce ace2, bool isDacl)
		{
			int num = (isDacl ? CommonAcl.DaclAcePriority(ace1) : CommonAcl.SaclAcePriority(ace1));
			int num2 = (isDacl ? CommonAcl.DaclAcePriority(ace2) : CommonAcl.SaclAcePriority(ace2));
			if (num < num2)
			{
				return CommonAcl.ComparisonResult.LessThan;
			}
			if (num > num2)
			{
				return CommonAcl.ComparisonResult.GreaterThan;
			}
			KnownAce knownAce = ace1 as KnownAce;
			KnownAce knownAce2 = ace2 as KnownAce;
			if (knownAce != null && knownAce2 != null)
			{
				int num3 = knownAce.SecurityIdentifier.CompareTo(knownAce2.SecurityIdentifier);
				if (num3 < 0)
				{
					return CommonAcl.ComparisonResult.LessThan;
				}
				if (num3 > 0)
				{
					return CommonAcl.ComparisonResult.GreaterThan;
				}
			}
			return CommonAcl.ComparisonResult.EqualTo;
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x0012E61C File Offset: 0x0012D61C
		private void QuickSort(int left, int right, bool isDacl)
		{
			if (left >= right)
			{
				return;
			}
			int num = left;
			int num2 = right;
			GenericAce genericAce = this._acl[left];
			while (left < right)
			{
				while (CommonAcl.CompareAces(this._acl[right], genericAce, isDacl) != CommonAcl.ComparisonResult.LessThan && left < right)
				{
					right--;
				}
				if (left != right)
				{
					this._acl[left] = this._acl[right];
					left++;
				}
				while (CommonAcl.ComparisonResult.GreaterThan != CommonAcl.CompareAces(this._acl[left], genericAce, isDacl) && left < right)
				{
					left++;
				}
				if (left != right)
				{
					this._acl[right] = this._acl[left];
					right--;
				}
			}
			this._acl[left] = genericAce;
			int num3 = left;
			left = num;
			right = num2;
			if (left < num3)
			{
				this.QuickSort(left, num3 - 1, isDacl);
			}
			if (right > num3)
			{
				this.QuickSort(num3 + 1, right, isDacl);
			}
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x0012E700 File Offset: 0x0012D700
		private bool InspectAce(ref GenericAce ace, bool isDacl)
		{
			KnownAce knownAce = ace as KnownAce;
			if (knownAce != null && knownAce.AccessMask == 0)
			{
				return false;
			}
			if (!this.IsContainer)
			{
				if ((byte)(ace.AceFlags & AceFlags.InheritOnly) != 0)
				{
					return false;
				}
				if ((byte)(ace.AceFlags & AceFlags.InheritanceFlags) != 0)
				{
					GenericAce genericAce = ace;
					genericAce.AceFlags &= ~(AceFlags.ObjectInherit | AceFlags.ContainerInherit | AceFlags.NoPropagateInherit | AceFlags.InheritOnly);
				}
			}
			else
			{
				if ((byte)(ace.AceFlags & AceFlags.InheritOnly) != 0 && (byte)(ace.AceFlags & AceFlags.ContainerInherit) == 0 && (byte)(ace.AceFlags & AceFlags.ObjectInherit) == 0)
				{
					return false;
				}
				if ((byte)(ace.AceFlags & AceFlags.NoPropagateInherit) != 0 && (byte)(ace.AceFlags & AceFlags.ContainerInherit) == 0 && (byte)(ace.AceFlags & AceFlags.ObjectInherit) == 0)
				{
					GenericAce genericAce2 = ace;
					genericAce2.AceFlags &= ~AceFlags.NoPropagateInherit;
				}
			}
			QualifiedAce qualifiedAce = knownAce as QualifiedAce;
			if (isDacl)
			{
				GenericAce genericAce3 = ace;
				genericAce3.AceFlags &= ~(AceFlags.SuccessfulAccess | AceFlags.FailedAccess);
				if (qualifiedAce != null && qualifiedAce.AceQualifier != AceQualifier.AccessAllowed && qualifiedAce.AceQualifier != AceQualifier.AccessDenied)
				{
					return false;
				}
			}
			else
			{
				if ((byte)(ace.AceFlags & AceFlags.AuditFlags) == 0)
				{
					return false;
				}
				if (qualifiedAce != null && qualifiedAce.AceQualifier != AceQualifier.SystemAudit)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x0012E820 File Offset: 0x0012D820
		private void RemoveMeaninglessAcesAndFlags(bool isDacl)
		{
			for (int i = this._acl.Count - 1; i >= 0; i--)
			{
				GenericAce genericAce = this._acl[i];
				if (!this.InspectAce(ref genericAce, isDacl))
				{
					this._acl.RemoveAce(i);
				}
			}
		}

		// Token: 0x06005351 RID: 21329 RVA: 0x0012E86C File Offset: 0x0012D86C
		private void Canonicalize(bool compact, bool isDacl)
		{
			ushort num = 0;
			while ((int)num < this._acl.Count)
			{
				this._acl[(int)num]._indexInAcl = num;
				num += 1;
			}
			this.QuickSort(0, this._acl.Count - 1, isDacl);
			if (compact)
			{
				for (int i = 0; i < this.Count - 1; i++)
				{
					QualifiedAce qualifiedAce = this._acl[i] as QualifiedAce;
					if (!(qualifiedAce == null))
					{
						QualifiedAce qualifiedAce2 = this._acl[i + 1] as QualifiedAce;
						if (!(qualifiedAce2 == null) && this.MergeAces(ref qualifiedAce, qualifiedAce2))
						{
							this._acl.RemoveAce(i + 1);
						}
					}
				}
			}
		}

		// Token: 0x06005352 RID: 21330 RVA: 0x0012E920 File Offset: 0x0012D920
		private void GetObjectTypesForSplit(ObjectAce originalAce, int accessMask, AceFlags aceFlags, out ObjectAceFlags objectFlags, out Guid objectType, out Guid inheritedObjectType)
		{
			objectFlags = ObjectAceFlags.None;
			objectType = Guid.Empty;
			inheritedObjectType = Guid.Empty;
			if ((accessMask & ObjectAce.AccessMaskWithObjectType) != 0)
			{
				objectType = originalAce.ObjectAceType;
				objectFlags |= originalAce.ObjectAceFlags & ObjectAceFlags.ObjectAceTypePresent;
			}
			if ((byte)(aceFlags & AceFlags.ContainerInherit) != 0)
			{
				inheritedObjectType = originalAce.InheritedObjectAceType;
				objectFlags |= originalAce.ObjectAceFlags & ObjectAceFlags.InheritedObjectAceTypePresent;
			}
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x0012E990 File Offset: 0x0012D990
		private bool ObjectTypesMatch(QualifiedAce ace, QualifiedAce newAce)
		{
			Guid guid = ((ace is ObjectAce) ? ((ObjectAce)ace).ObjectAceType : Guid.Empty);
			Guid guid2 = ((newAce is ObjectAce) ? ((ObjectAce)newAce).ObjectAceType : Guid.Empty);
			return guid.Equals(guid2);
		}

		// Token: 0x06005354 RID: 21332 RVA: 0x0012E9DC File Offset: 0x0012D9DC
		private bool InheritedObjectTypesMatch(QualifiedAce ace, QualifiedAce newAce)
		{
			Guid guid = ((ace is ObjectAce) ? ((ObjectAce)ace).InheritedObjectAceType : Guid.Empty);
			Guid guid2 = ((newAce is ObjectAce) ? ((ObjectAce)newAce).InheritedObjectAceType : Guid.Empty);
			return guid.Equals(guid2);
		}

		// Token: 0x06005355 RID: 21333 RVA: 0x0012EA28 File Offset: 0x0012DA28
		private bool AccessMasksAreMergeable(QualifiedAce ace, QualifiedAce newAce)
		{
			if (this.ObjectTypesMatch(ace, newAce))
			{
				return true;
			}
			ObjectAceFlags objectAceFlags = ((ace is ObjectAce) ? ((ObjectAce)ace).ObjectAceFlags : ObjectAceFlags.None);
			return (ace.AccessMask & newAce.AccessMask & ObjectAce.AccessMaskWithObjectType) == (newAce.AccessMask & ObjectAce.AccessMaskWithObjectType) && (objectAceFlags & ObjectAceFlags.ObjectAceTypePresent) == ObjectAceFlags.None;
		}

		// Token: 0x06005356 RID: 21334 RVA: 0x0012EA84 File Offset: 0x0012DA84
		private bool AceFlagsAreMergeable(QualifiedAce ace, QualifiedAce newAce)
		{
			if (this.InheritedObjectTypesMatch(ace, newAce))
			{
				return true;
			}
			ObjectAceFlags objectAceFlags = ((ace is ObjectAce) ? ((ObjectAce)ace).ObjectAceFlags : ObjectAceFlags.None);
			return (objectAceFlags & ObjectAceFlags.InheritedObjectAceTypePresent) == ObjectAceFlags.None;
		}

		// Token: 0x06005357 RID: 21335 RVA: 0x0012EABC File Offset: 0x0012DABC
		private bool GetAccessMaskForRemoval(QualifiedAce ace, ObjectAceFlags objectFlags, Guid objectType, ref int accessMask)
		{
			if ((ace.AccessMask & accessMask & ObjectAce.AccessMaskWithObjectType) != 0)
			{
				if (ace is ObjectAce)
				{
					ObjectAce objectAce = ace as ObjectAce;
					if ((objectFlags & ObjectAceFlags.ObjectAceTypePresent) != ObjectAceFlags.None && (objectAce.ObjectAceFlags & ObjectAceFlags.ObjectAceTypePresent) == ObjectAceFlags.None)
					{
						return false;
					}
					if ((objectFlags & ObjectAceFlags.ObjectAceTypePresent) != ObjectAceFlags.None && !objectAce.ObjectTypesMatch(objectFlags, objectType))
					{
						accessMask &= ~ObjectAce.AccessMaskWithObjectType;
					}
				}
				else if ((objectFlags & ObjectAceFlags.ObjectAceTypePresent) != ObjectAceFlags.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x0012EB28 File Offset: 0x0012DB28
		private bool GetInheritanceFlagsForRemoval(QualifiedAce ace, ObjectAceFlags objectFlags, Guid inheritedObjectType, ref AceFlags aceFlags)
		{
			if ((byte)(ace.AceFlags & AceFlags.ContainerInherit) != 0 && (byte)(aceFlags & AceFlags.ContainerInherit) != 0)
			{
				if (ace is ObjectAce)
				{
					ObjectAce objectAce = ace as ObjectAce;
					if ((objectFlags & ObjectAceFlags.InheritedObjectAceTypePresent) != ObjectAceFlags.None && (objectAce.ObjectAceFlags & ObjectAceFlags.InheritedObjectAceTypePresent) == ObjectAceFlags.None)
					{
						return false;
					}
					if ((objectFlags & ObjectAceFlags.InheritedObjectAceTypePresent) != ObjectAceFlags.None && !objectAce.InheritedObjectTypesMatch(objectFlags, inheritedObjectType))
					{
						aceFlags &= ~(AceFlags.ObjectInherit | AceFlags.ContainerInherit | AceFlags.NoPropagateInherit | AceFlags.InheritOnly);
					}
				}
				else if ((objectFlags & ObjectAceFlags.InheritedObjectAceTypePresent) != ObjectAceFlags.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x0012EB94 File Offset: 0x0012DB94
		private bool MergeAces(ref QualifiedAce ace, QualifiedAce newAce)
		{
			if ((byte)(ace.AceFlags & AceFlags.Inherited) != 0)
			{
				return false;
			}
			if ((byte)(newAce.AceFlags & AceFlags.Inherited) != 0)
			{
				return false;
			}
			if (ace.AceQualifier != newAce.AceQualifier)
			{
				return false;
			}
			if (ace.SecurityIdentifier != newAce.SecurityIdentifier)
			{
				return false;
			}
			if (ace.AceFlags == newAce.AceFlags)
			{
				if (!(ace is ObjectAce) && !(newAce is ObjectAce))
				{
					ace.AccessMask |= newAce.AccessMask;
					return true;
				}
				if (this.InheritedObjectTypesMatch(ace, newAce) && this.AccessMasksAreMergeable(ace, newAce))
				{
					ace.AccessMask |= newAce.AccessMask;
					return true;
				}
			}
			if ((byte)(ace.AceFlags & AceFlags.InheritanceFlags) == (byte)(newAce.AceFlags & AceFlags.InheritanceFlags) && ace.AccessMask == newAce.AccessMask)
			{
				if (!(ace is ObjectAce) && !(newAce is ObjectAce))
				{
					QualifiedAce qualifiedAce = ace;
					qualifiedAce.AceFlags |= newAce.AceFlags & AceFlags.AuditFlags;
					return true;
				}
				if (this.InheritedObjectTypesMatch(ace, newAce) && this.ObjectTypesMatch(ace, newAce))
				{
					QualifiedAce qualifiedAce2 = ace;
					qualifiedAce2.AceFlags |= newAce.AceFlags & AceFlags.AuditFlags;
					return true;
				}
			}
			if ((byte)(ace.AceFlags & AceFlags.AuditFlags) == (byte)(newAce.AceFlags & AceFlags.AuditFlags) && ace.AccessMask == newAce.AccessMask)
			{
				AceFlags aceFlags;
				if (ace is ObjectAce || newAce is ObjectAce)
				{
					if (this.ObjectTypesMatch(ace, newAce) && this.AceFlagsAreMergeable(ace, newAce) && CommonAcl.MergeInheritanceBits(ace.AceFlags, newAce.AceFlags, this.IsDS, out aceFlags))
					{
						ace.AceFlags = aceFlags | (ace.AceFlags & AceFlags.AuditFlags);
						return true;
					}
				}
				else if (CommonAcl.MergeInheritanceBits(ace.AceFlags, newAce.AceFlags, this.IsDS, out aceFlags))
				{
					ace.AceFlags = aceFlags | (ace.AceFlags & AceFlags.AuditFlags);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x0012ED98 File Offset: 0x0012DD98
		private bool CanonicalCheck(bool isDacl)
		{
			if (isDacl)
			{
				int num = 0;
				for (int i = 0; i < this._acl.Count; i++)
				{
					GenericAce genericAce = this._acl[i];
					int num2;
					if ((byte)(genericAce.AceFlags & AceFlags.Inherited) != 0)
					{
						num2 = 2;
					}
					else
					{
						QualifiedAce qualifiedAce = genericAce as QualifiedAce;
						if (qualifiedAce == null)
						{
							return false;
						}
						if (qualifiedAce.AceQualifier == AceQualifier.AccessAllowed)
						{
							num2 = 1;
						}
						else
						{
							if (qualifiedAce.AceQualifier != AceQualifier.AccessDenied)
							{
								return false;
							}
							num2 = 0;
						}
					}
					if (num2 != 3)
					{
						if (num2 > num)
						{
							num = num2;
						}
						else if (num2 < num)
						{
							return false;
						}
					}
				}
			}
			else
			{
				int num3 = 0;
				for (int j = 0; j < this._acl.Count; j++)
				{
					GenericAce genericAce2 = this._acl[j];
					if (!(genericAce2 == null))
					{
						int num4;
						if ((byte)(genericAce2.AceFlags & AceFlags.Inherited) != 0)
						{
							num4 = 1;
						}
						else
						{
							QualifiedAce qualifiedAce2 = genericAce2 as QualifiedAce;
							if (qualifiedAce2 == null)
							{
								return false;
							}
							if (qualifiedAce2.AceQualifier != AceQualifier.SystemAudit && qualifiedAce2.AceQualifier != AceQualifier.SystemAlarm)
							{
								return false;
							}
							num4 = 0;
						}
						if (num4 > num3)
						{
							num3 = num4;
						}
						else if (num4 < num3)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x0012EEBA File Offset: 0x0012DEBA
		private void ThrowIfNotCanonical()
		{
			if (!this._isCanonical)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ModificationOfNonCanonicalAcl"));
			}
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x0012EED4 File Offset: 0x0012DED4
		internal CommonAcl(bool isContainer, bool isDS, byte revision, int capacity)
		{
			this._isContainer = isContainer;
			this._isDS = isDS;
			this._acl = new RawAcl(revision, capacity);
			this._isCanonical = true;
		}

		// Token: 0x0600535D RID: 21341 RVA: 0x0012EF00 File Offset: 0x0012DF00
		internal CommonAcl(bool isContainer, bool isDS, RawAcl rawAcl, bool trusted, bool isDacl)
		{
			this._isContainer = isContainer;
			this._isDS = isDS;
			if (rawAcl == null)
			{
				throw new ArgumentNullException("rawAcl");
			}
			if (trusted)
			{
				this._acl = rawAcl;
				this.RemoveMeaninglessAcesAndFlags(isDacl);
			}
			else
			{
				this._acl = new RawAcl(rawAcl.Revision, rawAcl.Count);
				for (int i = 0; i < rawAcl.Count; i++)
				{
					GenericAce genericAce = rawAcl[i].Copy();
					if (this.InspectAce(ref genericAce, isDacl))
					{
						this._acl.InsertAce(this._acl.Count, genericAce);
					}
				}
			}
			if (this.CanonicalCheck(isDacl))
			{
				this.Canonicalize(true, isDacl);
				this._isCanonical = true;
				return;
			}
			this._isCanonical = false;
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x0600535E RID: 21342 RVA: 0x0012EFBE File Offset: 0x0012DFBE
		internal RawAcl RawAcl
		{
			get
			{
				return this._acl;
			}
		}

		// Token: 0x0600535F RID: 21343 RVA: 0x0012EFC6 File Offset: 0x0012DFC6
		internal void CheckAccessType(AccessControlType accessType)
		{
			if (accessType != AccessControlType.Allow && accessType != AccessControlType.Deny)
			{
				throw new ArgumentOutOfRangeException("accessType", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
		}

		// Token: 0x06005360 RID: 21344 RVA: 0x0012EFE4 File Offset: 0x0012DFE4
		internal void CheckFlags(InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			if (this.IsContainer)
			{
				if (inheritanceFlags == InheritanceFlags.None && propagationFlags != PropagationFlags.None)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidAnyFlag"), "propagationFlags");
				}
			}
			else
			{
				if (inheritanceFlags != InheritanceFlags.None)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidAnyFlag"), "inheritanceFlags");
				}
				if (propagationFlags != PropagationFlags.None)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidAnyFlag"), "propagationFlags");
				}
			}
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x0012F044 File Offset: 0x0012E044
		internal void AddQualifiedAce(SecurityIdentifier sid, AceQualifier qualifier, int accessMask, AceFlags flags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			this.ThrowIfNotCanonical();
			bool flag = false;
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			if (qualifier == AceQualifier.SystemAudit && (byte)(flags & AceFlags.AuditFlags) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "flags");
			}
			if (accessMask == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ArgumentZero"), "accessMask");
			}
			GenericAce genericAce;
			if (!this.IsDS || objectFlags == ObjectAceFlags.None)
			{
				genericAce = new CommonAce(flags, qualifier, accessMask, sid, false, null);
			}
			else
			{
				genericAce = new ObjectAce(flags, qualifier, accessMask, sid, objectFlags, objectType, inheritedObjectType, false, null);
			}
			if (!this.InspectAce(ref genericAce, this is DiscretionaryAcl))
			{
				return;
			}
			for (int i = 0; i < this.Count; i++)
			{
				QualifiedAce qualifiedAce = this._acl[i] as QualifiedAce;
				if (!(qualifiedAce == null) && this.MergeAces(ref qualifiedAce, genericAce as QualifiedAce))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this._acl.InsertAce(this._acl.Count, genericAce);
				this._isDirty = true;
			}
			this.OnAclModificationTried();
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x0012F154 File Offset: 0x0012E154
		internal void SetQualifiedAce(SecurityIdentifier sid, AceQualifier qualifier, int accessMask, AceFlags flags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			this.ThrowIfNotCanonical();
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			if (qualifier == AceQualifier.SystemAudit && (byte)(flags & AceFlags.AuditFlags) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "flags");
			}
			if (accessMask == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ArgumentZero"), "accessMask");
			}
			GenericAce genericAce;
			if (!this.IsDS || objectFlags == ObjectAceFlags.None)
			{
				genericAce = new CommonAce(flags, qualifier, accessMask, sid, false, null);
			}
			else
			{
				genericAce = new ObjectAce(flags, qualifier, accessMask, sid, objectFlags, objectType, inheritedObjectType, false, null);
			}
			if (!this.InspectAce(ref genericAce, this is DiscretionaryAcl))
			{
				return;
			}
			for (int i = 0; i < this.Count; i++)
			{
				QualifiedAce qualifiedAce = this._acl[i] as QualifiedAce;
				if (!(qualifiedAce == null) && (byte)(qualifiedAce.AceFlags & AceFlags.Inherited) == 0 && qualifiedAce.AceQualifier == qualifier && !(qualifiedAce.SecurityIdentifier != sid))
				{
					this._acl.RemoveAce(i);
					i--;
				}
			}
			this._acl.InsertAce(this._acl.Count, genericAce);
			this._isDirty = true;
			this.OnAclModificationTried();
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x0012F27C File Offset: 0x0012E27C
		internal bool RemoveQualifiedAces(SecurityIdentifier sid, AceQualifier qualifier, int accessMask, AceFlags flags, bool saclSemantics, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			this.ThrowIfNotCanonical();
			if (accessMask == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ArgumentZero"), "accessMask");
			}
			if (qualifier == AceQualifier.SystemAudit && (byte)(flags & AceFlags.AuditFlags) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "flags");
			}
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			bool flag = true;
			bool flag2 = true;
			int num = accessMask;
			AceFlags aceFlags = flags;
			byte[] array = new byte[this.BinaryLength];
			this.GetBinaryForm(array, 0);
			for (;;)
			{
				try
				{
					for (int i = 0; i < this.Count; i++)
					{
						QualifiedAce qualifiedAce = this._acl[i] as QualifiedAce;
						if (!(qualifiedAce == null) && (byte)(qualifiedAce.AceFlags & AceFlags.Inherited) == 0 && qualifiedAce.AceQualifier == qualifier && !(qualifiedAce.SecurityIdentifier != sid))
						{
							if (this.IsDS)
							{
								accessMask = num;
								bool flag3 = !this.GetAccessMaskForRemoval(qualifiedAce, objectFlags, objectType, ref accessMask);
								if ((qualifiedAce.AccessMask & accessMask) == 0)
								{
									goto IL_045A;
								}
								flags = aceFlags;
								bool flag4 = !this.GetInheritanceFlagsForRemoval(qualifiedAce, objectFlags, inheritedObjectType, ref flags);
								if (((byte)(qualifiedAce.AceFlags & AceFlags.ContainerInherit) == 0 && (byte)(flags & AceFlags.ContainerInherit) != 0 && (byte)(flags & AceFlags.InheritOnly) != 0) || ((byte)(flags & AceFlags.ContainerInherit) == 0 && (byte)(qualifiedAce.AceFlags & AceFlags.ContainerInherit) != 0 && (byte)(qualifiedAce.AceFlags & AceFlags.InheritOnly) != 0) || ((byte)(aceFlags & AceFlags.ContainerInherit) != 0 && (byte)(aceFlags & AceFlags.InheritOnly) != 0 && (byte)(flags & AceFlags.ContainerInherit) == 0))
								{
									goto IL_045A;
								}
								if (flag3 || flag4)
								{
									flag2 = false;
									break;
								}
							}
							else if ((qualifiedAce.AccessMask & accessMask) == 0)
							{
								goto IL_045A;
							}
							if (!saclSemantics || ((byte)(qualifiedAce.AceFlags & flags) & 192) != 0)
							{
								ObjectAceFlags objectAceFlags = ObjectAceFlags.None;
								Guid empty = Guid.Empty;
								Guid empty2 = Guid.Empty;
								AceFlags aceFlags2 = AceFlags.None;
								int num2 = 0;
								ObjectAceFlags objectAceFlags2 = ObjectAceFlags.None;
								Guid empty3 = Guid.Empty;
								Guid empty4 = Guid.Empty;
								ObjectAceFlags objectAceFlags3 = ObjectAceFlags.None;
								Guid empty5 = Guid.Empty;
								Guid empty6 = Guid.Empty;
								AceFlags aceFlags3 = AceFlags.None;
								bool flag5 = false;
								AceFlags aceFlags4 = qualifiedAce.AceFlags;
								int num3 = qualifiedAce.AccessMask & ~accessMask;
								if (qualifiedAce is ObjectAce)
								{
									this.GetObjectTypesForSplit(qualifiedAce as ObjectAce, num3, aceFlags4, out objectAceFlags, out empty, out empty2);
								}
								if (saclSemantics)
								{
									aceFlags2 = qualifiedAce.AceFlags & ~(flags & AceFlags.AuditFlags);
									num2 = qualifiedAce.AccessMask & accessMask;
									if (qualifiedAce is ObjectAce)
									{
										this.GetObjectTypesForSplit(qualifiedAce as ObjectAce, num2, aceFlags2, out objectAceFlags2, out empty3, out empty4);
									}
								}
								AceFlags aceFlags5 = (AceFlags)((byte)(qualifiedAce.AceFlags & AceFlags.InheritanceFlags) | ((byte)(flags & qualifiedAce.AceFlags) & 192));
								int num4 = qualifiedAce.AccessMask & accessMask;
								if (!saclSemantics || (byte)(aceFlags5 & AceFlags.AuditFlags) != 0)
								{
									if (!CommonAcl.RemoveInheritanceBits(aceFlags5, flags, this.IsDS, out aceFlags3, out flag5))
									{
										flag2 = false;
										break;
									}
									if (!flag5)
									{
										aceFlags3 |= aceFlags5 & AceFlags.AuditFlags;
										if (qualifiedAce is ObjectAce)
										{
											this.GetObjectTypesForSplit(qualifiedAce as ObjectAce, num4, aceFlags3, out objectAceFlags3, out empty5, out empty6);
										}
									}
								}
								if (!flag)
								{
									if (num3 != 0)
									{
										if (qualifiedAce is ObjectAce && (((ObjectAce)qualifiedAce).ObjectAceFlags & ObjectAceFlags.ObjectAceTypePresent) != ObjectAceFlags.None && (objectAceFlags & ObjectAceFlags.ObjectAceTypePresent) == ObjectAceFlags.None)
										{
											this._acl.RemoveAce(i);
											ObjectAce objectAce = new ObjectAce(aceFlags4, qualifier, num3, qualifiedAce.SecurityIdentifier, objectAceFlags, empty, empty2, false, null);
											this._acl.InsertAce(i, objectAce);
										}
										else
										{
											qualifiedAce.AceFlags = aceFlags4;
											qualifiedAce.AccessMask = num3;
											if (qualifiedAce is ObjectAce)
											{
												ObjectAce objectAce2 = qualifiedAce as ObjectAce;
												objectAce2.ObjectAceFlags = objectAceFlags;
												objectAce2.ObjectAceType = empty;
												objectAce2.InheritedObjectAceType = empty2;
											}
										}
									}
									else
									{
										this._acl.RemoveAce(i);
										i--;
									}
									if (saclSemantics && (byte)(aceFlags2 & AceFlags.AuditFlags) != 0)
									{
										QualifiedAce qualifiedAce2;
										if (qualifiedAce is CommonAce)
										{
											qualifiedAce2 = new CommonAce(aceFlags2, qualifier, num2, qualifiedAce.SecurityIdentifier, false, null);
										}
										else
										{
											qualifiedAce2 = new ObjectAce(aceFlags2, qualifier, num2, qualifiedAce.SecurityIdentifier, objectAceFlags2, empty3, empty4, false, null);
										}
										i++;
										this._acl.InsertAce(i, qualifiedAce2);
									}
									if (!flag5)
									{
										QualifiedAce qualifiedAce2;
										if (qualifiedAce is CommonAce)
										{
											qualifiedAce2 = new CommonAce(aceFlags3, qualifier, num4, qualifiedAce.SecurityIdentifier, false, null);
										}
										else
										{
											qualifiedAce2 = new ObjectAce(aceFlags3, qualifier, num4, qualifiedAce.SecurityIdentifier, objectAceFlags3, empty5, empty6, false, null);
										}
										i++;
										this._acl.InsertAce(i, qualifiedAce2);
									}
								}
							}
						}
						IL_045A:;
					}
				}
				catch (OverflowException)
				{
					this._acl.SetBinaryForm(array, 0);
					return false;
				}
				if (!flag || !flag2)
				{
					break;
				}
				flag = false;
			}
			this.OnAclModificationTried();
			return flag2;
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x0012F740 File Offset: 0x0012E740
		internal void RemoveQualifiedAcesSpecific(SecurityIdentifier sid, AceQualifier qualifier, int accessMask, AceFlags flags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			this.ThrowIfNotCanonical();
			if (accessMask == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ArgumentZero"), "accessMask");
			}
			if (qualifier == AceQualifier.SystemAudit && (byte)(flags & AceFlags.AuditFlags) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "flags");
			}
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			for (int i = 0; i < this.Count; i++)
			{
				QualifiedAce qualifiedAce = this._acl[i] as QualifiedAce;
				if (!(qualifiedAce == null) && (byte)(qualifiedAce.AceFlags & AceFlags.Inherited) == 0 && qualifiedAce.AceQualifier == qualifier && !(qualifiedAce.SecurityIdentifier != sid) && qualifiedAce.AceFlags == flags && qualifiedAce.AccessMask == accessMask)
				{
					if (this.IsDS)
					{
						if (qualifiedAce is ObjectAce && objectFlags != ObjectAceFlags.None)
						{
							ObjectAce objectAce = qualifiedAce as ObjectAce;
							if (!objectAce.ObjectTypesMatch(objectFlags, objectType))
							{
								goto IL_0102;
							}
							if (!objectAce.InheritedObjectTypesMatch(objectFlags, inheritedObjectType))
							{
								goto IL_0102;
							}
						}
						else if (qualifiedAce is ObjectAce || objectFlags != ObjectAceFlags.None)
						{
							goto IL_0102;
						}
					}
					this._acl.RemoveAce(i);
					i--;
				}
				IL_0102:;
			}
			this.OnAclModificationTried();
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x0012F865 File Offset: 0x0012E865
		internal virtual void OnAclModificationTried()
		{
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06005366 RID: 21350 RVA: 0x0012F867 File Offset: 0x0012E867
		public sealed override byte Revision
		{
			get
			{
				return this._acl.Revision;
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06005367 RID: 21351 RVA: 0x0012F874 File Offset: 0x0012E874
		public sealed override int Count
		{
			get
			{
				this.CanonicalizeIfNecessary();
				return this._acl.Count;
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x06005368 RID: 21352 RVA: 0x0012F887 File Offset: 0x0012E887
		public sealed override int BinaryLength
		{
			get
			{
				this.CanonicalizeIfNecessary();
				return this._acl.BinaryLength;
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06005369 RID: 21353 RVA: 0x0012F89A File Offset: 0x0012E89A
		public bool IsCanonical
		{
			get
			{
				return this._isCanonical;
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x0600536A RID: 21354 RVA: 0x0012F8A2 File Offset: 0x0012E8A2
		public bool IsContainer
		{
			get
			{
				return this._isContainer;
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x0600536B RID: 21355 RVA: 0x0012F8AA File Offset: 0x0012E8AA
		public bool IsDS
		{
			get
			{
				return this._isDS;
			}
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x0012F8B2 File Offset: 0x0012E8B2
		public sealed override void GetBinaryForm(byte[] binaryForm, int offset)
		{
			this.CanonicalizeIfNecessary();
			this._acl.GetBinaryForm(binaryForm, offset);
		}

		// Token: 0x17000E77 RID: 3703
		public sealed override GenericAce this[int index]
		{
			get
			{
				this.CanonicalizeIfNecessary();
				return this._acl[index].Copy();
			}
			set
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SetMethod"));
			}
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x0012F8F4 File Offset: 0x0012E8F4
		public void RemoveInheritedAces()
		{
			this.ThrowIfNotCanonical();
			for (int i = this._acl.Count - 1; i >= 0; i--)
			{
				GenericAce genericAce = this._acl[i];
				if ((byte)(genericAce.AceFlags & AceFlags.Inherited) != 0)
				{
					this._acl.RemoveAce(i);
				}
			}
			this.OnAclModificationTried();
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x0012F94C File Offset: 0x0012E94C
		public void Purge(SecurityIdentifier sid)
		{
			this.ThrowIfNotCanonical();
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			for (int i = this.Count - 1; i >= 0; i--)
			{
				KnownAce knownAce = this._acl[i] as KnownAce;
				if (!(knownAce == null) && (byte)(knownAce.AceFlags & AceFlags.Inherited) == 0 && knownAce.SecurityIdentifier == sid)
				{
					this._acl.RemoveAce(i);
				}
			}
			this.OnAclModificationTried();
		}

		// Token: 0x04002B08 RID: 11016
		private static CommonAcl.PM[] AFtoPM = new CommonAcl.PM[16];

		// Token: 0x04002B09 RID: 11017
		private static CommonAcl.AF[] PMtoAF;

		// Token: 0x04002B0A RID: 11018
		private RawAcl _acl;

		// Token: 0x04002B0B RID: 11019
		private bool _isDirty;

		// Token: 0x04002B0C RID: 11020
		private readonly bool _isCanonical;

		// Token: 0x04002B0D RID: 11021
		private readonly bool _isContainer;

		// Token: 0x04002B0E RID: 11022
		private readonly bool _isDS;

		// Token: 0x020008F2 RID: 2290
		[Flags]
		private enum AF
		{
			// Token: 0x04002B10 RID: 11024
			CI = 8,
			// Token: 0x04002B11 RID: 11025
			OI = 4,
			// Token: 0x04002B12 RID: 11026
			IO = 2,
			// Token: 0x04002B13 RID: 11027
			NP = 1,
			// Token: 0x04002B14 RID: 11028
			Invalid = 1
		}

		// Token: 0x020008F3 RID: 2291
		[Flags]
		private enum PM
		{
			// Token: 0x04002B16 RID: 11030
			F = 16,
			// Token: 0x04002B17 RID: 11031
			CF = 8,
			// Token: 0x04002B18 RID: 11032
			CO = 4,
			// Token: 0x04002B19 RID: 11033
			GF = 2,
			// Token: 0x04002B1A RID: 11034
			GO = 1,
			// Token: 0x04002B1B RID: 11035
			Invalid = 1
		}

		// Token: 0x020008F4 RID: 2292
		private enum ComparisonResult
		{
			// Token: 0x04002B1D RID: 11037
			LessThan,
			// Token: 0x04002B1E RID: 11038
			EqualTo,
			// Token: 0x04002B1F RID: 11039
			GreaterThan
		}
	}
}
