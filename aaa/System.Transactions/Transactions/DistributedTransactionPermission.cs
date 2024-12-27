using System;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Transactions
{
	// Token: 0x0200006F RID: 111
	[Serializable]
	public sealed class DistributedTransactionPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x06000317 RID: 791 RVA: 0x000326E8 File Offset: 0x00031AE8
		public DistributedTransactionPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.unrestricted = true;
				return;
			}
			this.unrestricted = false;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00032710 File Offset: 0x00031B10
		public bool IsUnrestricted()
		{
			return this.unrestricted;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00032724 File Offset: 0x00031B24
		public override IPermission Copy()
		{
			DistributedTransactionPermission distributedTransactionPermission = new DistributedTransactionPermission(PermissionState.None);
			if (this.IsUnrestricted())
			{
				distributedTransactionPermission.unrestricted = true;
			}
			else
			{
				distributedTransactionPermission.unrestricted = false;
			}
			return distributedTransactionPermission;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00032754 File Offset: 0x00031B54
		public override IPermission Intersect(IPermission target)
		{
			IPermission permission;
			try
			{
				if (target == null)
				{
					permission = null;
				}
				else
				{
					DistributedTransactionPermission distributedTransactionPermission = (DistributedTransactionPermission)target;
					if (!distributedTransactionPermission.IsUnrestricted())
					{
						permission = distributedTransactionPermission;
					}
					else
					{
						permission = this.Copy();
					}
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(SR.GetString("ArgumentWrongType"), "target");
			}
			return permission;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000327BC File Offset: 0x00031BBC
		public override IPermission Union(IPermission target)
		{
			IPermission permission;
			try
			{
				if (target == null)
				{
					permission = this.Copy();
				}
				else
				{
					DistributedTransactionPermission distributedTransactionPermission = (DistributedTransactionPermission)target;
					if (distributedTransactionPermission.IsUnrestricted())
					{
						permission = distributedTransactionPermission;
					}
					else
					{
						permission = this.Copy();
					}
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(SR.GetString("ArgumentWrongType"), "target");
			}
			return permission;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00032828 File Offset: 0x00031C28
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return !this.unrestricted;
			}
			bool flag;
			try
			{
				DistributedTransactionPermission distributedTransactionPermission = (DistributedTransactionPermission)target;
				if (!this.unrestricted)
				{
					flag = true;
				}
				else if (distributedTransactionPermission.unrestricted)
				{
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(SR.GetString("ArgumentWrongType"), "target");
			}
			return flag;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0003289C File Offset: 0x00031C9C
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			Type type = base.GetType();
			StringBuilder stringBuilder = new StringBuilder(type.Assembly.ToString());
			stringBuilder.Replace('"', '\'');
			securityElement.AddAttribute("class", type.FullName + ", " + stringBuilder);
			securityElement.AddAttribute("version", "1");
			securityElement.AddAttribute("Unrestricted", this.unrestricted.ToString());
			return securityElement;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0003291C File Offset: 0x00031D1C
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			if (!securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("ArgumentWrongType"), "securityElement");
			}
			string text = securityElement.Attribute("Unrestricted");
			if (text != null)
			{
				this.unrestricted = Convert.ToBoolean(text, CultureInfo.InvariantCulture);
				return;
			}
			this.unrestricted = false;
		}

		// Token: 0x04000145 RID: 325
		private bool unrestricted;
	}
}
