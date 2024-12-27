using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Security.Principal;
using System.Security.Util;
using System.Threading;

namespace System.Security.Permissions
{
	// Token: 0x02000639 RID: 1593
	[ComVisible(true)]
	[Serializable]
	public sealed class PrincipalPermission : IPermission, IUnrestrictedPermission, ISecurityEncodable, IBuiltInPermission
	{
		// Token: 0x060039DB RID: 14811 RVA: 0x000C2E74 File Offset: 0x000C1E74
		public PrincipalPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.m_array = new IDRole[1];
				this.m_array[0] = new IDRole();
				this.m_array[0].m_authenticated = true;
				this.m_array[0].m_id = null;
				this.m_array[0].m_role = null;
				return;
			}
			if (state == PermissionState.None)
			{
				this.m_array = new IDRole[1];
				this.m_array[0] = new IDRole();
				this.m_array[0].m_authenticated = false;
				this.m_array[0].m_id = "";
				this.m_array[0].m_role = "";
				return;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPermissionState"));
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x000C2F30 File Offset: 0x000C1F30
		public PrincipalPermission(string name, string role)
		{
			this.m_array = new IDRole[1];
			this.m_array[0] = new IDRole();
			this.m_array[0].m_authenticated = true;
			this.m_array[0].m_id = name;
			this.m_array[0].m_role = role;
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x000C2F88 File Offset: 0x000C1F88
		public PrincipalPermission(string name, string role, bool isAuthenticated)
		{
			this.m_array = new IDRole[1];
			this.m_array[0] = new IDRole();
			this.m_array[0].m_authenticated = isAuthenticated;
			this.m_array[0].m_id = name;
			this.m_array[0].m_role = role;
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x000C2FDE File Offset: 0x000C1FDE
		private PrincipalPermission(IDRole[] array)
		{
			this.m_array = array;
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x000C2FF0 File Offset: 0x000C1FF0
		private bool IsEmpty()
		{
			for (int i = 0; i < this.m_array.Length; i++)
			{
				if (this.m_array[i].m_id == null || !this.m_array[i].m_id.Equals("") || this.m_array[i].m_role == null || !this.m_array[i].m_role.Equals("") || this.m_array[i].m_authenticated)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x000C3072 File Offset: 0x000C2072
		private bool VerifyType(IPermission perm)
		{
			return perm != null && perm.GetType() == base.GetType();
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x000C3088 File Offset: 0x000C2088
		public bool IsUnrestricted()
		{
			for (int i = 0; i < this.m_array.Length; i++)
			{
				if (this.m_array[i].m_id != null || this.m_array[i].m_role != null || !this.m_array[i].m_authenticated)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x000C30D8 File Offset: 0x000C20D8
		public bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.IsEmpty();
			}
			bool flag;
			try
			{
				PrincipalPermission principalPermission = (PrincipalPermission)target;
				if (principalPermission.IsUnrestricted())
				{
					flag = true;
				}
				else if (this.IsUnrestricted())
				{
					flag = false;
				}
				else
				{
					for (int i = 0; i < this.m_array.Length; i++)
					{
						bool flag2 = false;
						for (int j = 0; j < principalPermission.m_array.Length; j++)
						{
							if (principalPermission.m_array[j].m_authenticated == this.m_array[i].m_authenticated && (principalPermission.m_array[j].m_id == null || (this.m_array[i].m_id != null && this.m_array[i].m_id.Equals(principalPermission.m_array[j].m_id))) && (principalPermission.m_array[j].m_role == null || (this.m_array[i].m_role != null && this.m_array[i].m_role.Equals(principalPermission.m_array[j].m_role))))
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							return false;
						}
					}
					flag = true;
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return flag;
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x000C324C File Offset: 0x000C224C
		public IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (!this.VerifyType(target))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			if (this.IsUnrestricted())
			{
				return target.Copy();
			}
			PrincipalPermission principalPermission = (PrincipalPermission)target;
			if (principalPermission.IsUnrestricted())
			{
				return this.Copy();
			}
			ArrayList arrayList = null;
			for (int i = 0; i < this.m_array.Length; i++)
			{
				for (int j = 0; j < principalPermission.m_array.Length; j++)
				{
					if (principalPermission.m_array[j].m_authenticated == this.m_array[i].m_authenticated)
					{
						if (principalPermission.m_array[j].m_id == null || this.m_array[i].m_id == null || this.m_array[i].m_id.Equals(principalPermission.m_array[j].m_id))
						{
							if (arrayList == null)
							{
								arrayList = new ArrayList();
							}
							IDRole idrole = new IDRole();
							idrole.m_id = ((principalPermission.m_array[j].m_id == null) ? this.m_array[i].m_id : principalPermission.m_array[j].m_id);
							if (principalPermission.m_array[j].m_role == null || this.m_array[i].m_role == null || this.m_array[i].m_role.Equals(principalPermission.m_array[j].m_role))
							{
								idrole.m_role = ((principalPermission.m_array[j].m_role == null) ? this.m_array[i].m_role : principalPermission.m_array[j].m_role);
							}
							else
							{
								idrole.m_role = "";
							}
							idrole.m_authenticated = principalPermission.m_array[j].m_authenticated;
							arrayList.Add(idrole);
						}
						else if (principalPermission.m_array[j].m_role == null || this.m_array[i].m_role == null || this.m_array[i].m_role.Equals(principalPermission.m_array[j].m_role))
						{
							if (arrayList == null)
							{
								arrayList = new ArrayList();
							}
							arrayList.Add(new IDRole
							{
								m_id = "",
								m_role = ((principalPermission.m_array[j].m_role == null) ? this.m_array[i].m_role : principalPermission.m_array[j].m_role),
								m_authenticated = principalPermission.m_array[j].m_authenticated
							});
						}
					}
				}
			}
			if (arrayList == null)
			{
				return null;
			}
			IDRole[] array = new IDRole[arrayList.Count];
			IEnumerator enumerator = arrayList.GetEnumerator();
			int num = 0;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				array[num++] = (IDRole)obj;
			}
			return new PrincipalPermission(array);
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x000C3528 File Offset: 0x000C2528
		public IPermission Union(IPermission other)
		{
			if (other == null)
			{
				return this.Copy();
			}
			if (!this.VerifyType(other))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			PrincipalPermission principalPermission = (PrincipalPermission)other;
			if (this.IsUnrestricted() || principalPermission.IsUnrestricted())
			{
				return new PrincipalPermission(PermissionState.Unrestricted);
			}
			int num = this.m_array.Length + principalPermission.m_array.Length;
			IDRole[] array = new IDRole[num];
			int i;
			for (i = 0; i < this.m_array.Length; i++)
			{
				array[i] = this.m_array[i];
			}
			for (int j = 0; j < principalPermission.m_array.Length; j++)
			{
				array[i + j] = principalPermission.m_array[j];
			}
			return new PrincipalPermission(array);
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x000C35FC File Offset: 0x000C25FC
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			IPermission permission = obj as IPermission;
			return (obj == null || permission != null) && this.IsSubsetOf(permission) && (permission == null || permission.IsSubsetOf(this));
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x000C3634 File Offset: 0x000C2634
		[ComVisible(false)]
		public override int GetHashCode()
		{
			int num = 0;
			for (int i = 0; i < this.m_array.Length; i++)
			{
				num += this.m_array[i].GetHashCode();
			}
			return num;
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x000C3667 File Offset: 0x000C2667
		public IPermission Copy()
		{
			return new PrincipalPermission(this.m_array);
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x000C3674 File Offset: 0x000C2674
		private void ThrowSecurityException()
		{
			AssemblyName assemblyName = null;
			Evidence evidence = null;
			PermissionSet.s_fullTrust.Assert();
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				assemblyName = callingAssembly.GetName();
				if (callingAssembly != Assembly.GetExecutingAssembly())
				{
					evidence = callingAssembly.Evidence;
				}
			}
			catch
			{
			}
			PermissionSet.RevertAssert();
			throw new SecurityException(Environment.GetResourceString("Security_PrincipalPermission"), assemblyName, null, null, null, SecurityAction.Demand, this, this, evidence);
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x000C36DC File Offset: 0x000C26DC
		public void Demand()
		{
			new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Assert();
			IPrincipal currentPrincipal = Thread.CurrentPrincipal;
			if (currentPrincipal == null)
			{
				this.ThrowSecurityException();
			}
			if (this.m_array == null)
			{
				return;
			}
			int num = this.m_array.Length;
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				if (!this.m_array[i].m_authenticated)
				{
					flag = true;
					break;
				}
				IIdentity identity = currentPrincipal.Identity;
				if (identity.IsAuthenticated && (this.m_array[i].m_id == null || string.Compare(identity.Name, this.m_array[i].m_id, StringComparison.OrdinalIgnoreCase) == 0))
				{
					if (this.m_array[i].m_role == null)
					{
						flag = true;
					}
					else
					{
						WindowsPrincipal windowsPrincipal = currentPrincipal as WindowsPrincipal;
						if (windowsPrincipal != null && this.m_array[i].Sid != null)
						{
							flag = windowsPrincipal.IsInRole(this.m_array[i].Sid);
						}
						else
						{
							flag = currentPrincipal.IsInRole(this.m_array[i].m_role);
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				this.ThrowSecurityException();
			}
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x000C37F0 File Offset: 0x000C27F0
		public SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Permissions.PrincipalPermission");
			securityElement.AddAttribute("version", "1");
			int num = this.m_array.Length;
			for (int i = 0; i < num; i++)
			{
				securityElement.AddChild(this.m_array[i].ToXml());
			}
			return securityElement;
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x000C3854 File Offset: 0x000C2854
		public void FromXml(SecurityElement elem)
		{
			CodeAccessPermission.ValidateElement(elem, this);
			if (elem.InternalChildren != null && elem.InternalChildren.Count != 0)
			{
				int count = elem.InternalChildren.Count;
				int num = 0;
				this.m_array = new IDRole[count];
				IEnumerator enumerator = elem.Children.GetEnumerator();
				while (enumerator.MoveNext())
				{
					IDRole idrole = new IDRole();
					idrole.FromXml((SecurityElement)enumerator.Current);
					this.m_array[num++] = idrole;
				}
				return;
			}
			this.m_array = new IDRole[0];
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x000C38DE File Offset: 0x000C28DE
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x000C38EB File Offset: 0x000C28EB
		int IBuiltInPermission.GetTokenIndex()
		{
			return PrincipalPermission.GetTokenIndex();
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x000C38F2 File Offset: 0x000C28F2
		internal static int GetTokenIndex()
		{
			return 8;
		}

		// Token: 0x04001DE2 RID: 7650
		private IDRole[] m_array;
	}
}
