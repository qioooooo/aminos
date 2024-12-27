using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200029D RID: 669
	internal class ComponentManagerProxy : MarshalByRefObject, UnsafeNativeMethods.IMsoComponentManager, UnsafeNativeMethods.IMsoComponent
	{
		// Token: 0x06002400 RID: 9216 RVA: 0x0005287F File Offset: 0x0005187F
		internal ComponentManagerProxy(ComponentManagerBroker broker, UnsafeNativeMethods.IMsoComponentManager original)
		{
			this._broker = broker;
			this._original = original;
			this._creationThread = SafeNativeMethods.GetCurrentThreadId();
			this._refCount = 0;
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x000528A7 File Offset: 0x000518A7
		private void Dispose()
		{
			if (this._original != null)
			{
				Marshal.ReleaseComObject(this._original);
				this._original = null;
				this._components = null;
				this._componentId = 0;
				this._refCount = 0;
				this._broker.ClearComponentManager();
			}
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000528E4 File Offset: 0x000518E4
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000528E7 File Offset: 0x000518E7
		private bool RevokeComponent()
		{
			return this._original.FRevokeComponent(this._componentId);
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06002404 RID: 9220 RVA: 0x000528FA File Offset: 0x000518FA
		private UnsafeNativeMethods.IMsoComponent Component
		{
			get
			{
				if (this._trackingComponent != null)
				{
					return this._trackingComponent;
				}
				if (this._activeComponent != null)
				{
					return this._activeComponent;
				}
				return null;
			}
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x0005291C File Offset: 0x0005191C
		bool UnsafeNativeMethods.IMsoComponent.FDebugMessage(IntPtr hInst, int msg, IntPtr wparam, IntPtr lparam)
		{
			UnsafeNativeMethods.IMsoComponent component = this.Component;
			return component != null && component.FDebugMessage(hInst, msg, wparam, lparam);
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x00052940 File Offset: 0x00051940
		bool UnsafeNativeMethods.IMsoComponent.FPreTranslateMessage(ref NativeMethods.MSG msg)
		{
			UnsafeNativeMethods.IMsoComponent component = this.Component;
			return component != null && component.FPreTranslateMessage(ref msg);
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x00052960 File Offset: 0x00051960
		void UnsafeNativeMethods.IMsoComponent.OnEnterState(int uStateID, bool fEnter)
		{
			if (this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					msoComponent.OnEnterState(uStateID, fEnter);
				}
			}
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000529C4 File Offset: 0x000519C4
		void UnsafeNativeMethods.IMsoComponent.OnAppActivate(bool fActive, int dwOtherThreadID)
		{
			if (this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					msoComponent.OnAppActivate(fActive, dwOtherThreadID);
				}
			}
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x00052A28 File Offset: 0x00051A28
		void UnsafeNativeMethods.IMsoComponent.OnLoseActivation()
		{
			if (this._activeComponent != null)
			{
				this._activeComponent.OnLoseActivation();
			}
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x00052A40 File Offset: 0x00051A40
		void UnsafeNativeMethods.IMsoComponent.OnActivationChange(UnsafeNativeMethods.IMsoComponent component, bool fSameComponent, int pcrinfo, bool fHostIsActivating, int pchostinfo, int dwReserved)
		{
			if (this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					msoComponent.OnActivationChange(component, fSameComponent, pcrinfo, fHostIsActivating, pchostinfo, dwReserved);
				}
			}
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x00052AA8 File Offset: 0x00051AA8
		bool UnsafeNativeMethods.IMsoComponent.FDoIdle(int grfidlef)
		{
			bool flag = false;
			if (this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					flag |= msoComponent.FDoIdle(grfidlef);
				}
			}
			return flag;
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x00052B10 File Offset: 0x00051B10
		bool UnsafeNativeMethods.IMsoComponent.FContinueMessageLoop(int reason, int pvLoopData, NativeMethods.MSG[] msgPeeked)
		{
			bool flag = false;
			if (this._refCount == 0 && this._componentId != 0 && this.RevokeComponent())
			{
				this._components.Clear();
				this._componentId = 0;
			}
			if (this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					flag |= msoComponent.FContinueMessageLoop(reason, pvLoopData, msgPeeked);
				}
			}
			return flag;
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x00052BA4 File Offset: 0x00051BA4
		bool UnsafeNativeMethods.IMsoComponent.FQueryTerminate(bool fPromptUser)
		{
			return true;
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x00052BA8 File Offset: 0x00051BA8
		void UnsafeNativeMethods.IMsoComponent.Terminate()
		{
			if (this._components != null && this._components.Values.Count > 0)
			{
				UnsafeNativeMethods.IMsoComponent[] array = new UnsafeNativeMethods.IMsoComponent[this._components.Values.Count];
				this._components.Values.CopyTo(array, 0);
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in array)
				{
					msoComponent.Terminate();
				}
			}
			if (this._original != null)
			{
				this.RevokeComponent();
			}
			this.Dispose();
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x00052C28 File Offset: 0x00051C28
		IntPtr UnsafeNativeMethods.IMsoComponent.HwndGetWindow(int dwWhich, int dwReserved)
		{
			UnsafeNativeMethods.IMsoComponent component = this.Component;
			if (component != null)
			{
				return component.HwndGetWindow(dwWhich, dwReserved);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x00052C4D File Offset: 0x00051C4D
		int UnsafeNativeMethods.IMsoComponentManager.QueryService(ref Guid guidService, ref Guid iid, out object ppvObj)
		{
			return this._original.QueryService(ref guidService, ref iid, out ppvObj);
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x00052C5D File Offset: 0x00051C5D
		bool UnsafeNativeMethods.IMsoComponentManager.FDebugMessage(IntPtr hInst, int msg, IntPtr wparam, IntPtr lparam)
		{
			return this._original.FDebugMessage(hInst, msg, wparam, lparam);
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x00052C70 File Offset: 0x00051C70
		bool UnsafeNativeMethods.IMsoComponentManager.FRegisterComponent(UnsafeNativeMethods.IMsoComponent component, NativeMethods.MSOCRINFOSTRUCT pcrinfo, out int dwComponentID)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			dwComponentID = 0;
			if (this._refCount == 0 && !this._original.FRegisterComponent(this, pcrinfo, out this._componentId))
			{
				return false;
			}
			this._refCount++;
			if (this._components == null)
			{
				this._components = new Dictionary<int, UnsafeNativeMethods.IMsoComponent>();
			}
			this._nextComponentId++;
			if (this._nextComponentId == 2147483647)
			{
				this._nextComponentId = 1;
			}
			bool flag = false;
			while (this._components.ContainsKey(this._nextComponentId))
			{
				this._nextComponentId++;
				if (this._nextComponentId == 2147483647)
				{
					if (flag)
					{
						throw new InvalidOperationException(SR.GetString("ComponentManagerProxyOutOfMemory"));
					}
					flag = true;
					this._nextComponentId = 1;
				}
			}
			this._components.Add(this._nextComponentId, component);
			dwComponentID = this._nextComponentId;
			return true;
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x00052D5C File Offset: 0x00051D5C
		bool UnsafeNativeMethods.IMsoComponentManager.FRevokeComponent(int dwComponentID)
		{
			if (this._original == null)
			{
				return false;
			}
			if (this._components == null || dwComponentID <= 0 || !this._components.ContainsKey(dwComponentID))
			{
				return false;
			}
			if (this._refCount == 1 && SafeNativeMethods.GetCurrentThreadId() == this._creationThread && !this.RevokeComponent())
			{
				return false;
			}
			this._refCount--;
			this._components.Remove(dwComponentID);
			if (this._refCount <= 0)
			{
				this.Dispose();
			}
			if (dwComponentID == this._activeComponentId)
			{
				this._activeComponent = null;
				this._activeComponentId = 0;
			}
			if (dwComponentID == this._trackingComponentId)
			{
				this._trackingComponent = null;
				this._trackingComponentId = 0;
			}
			return true;
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x00052E08 File Offset: 0x00051E08
		bool UnsafeNativeMethods.IMsoComponentManager.FUpdateComponentRegistration(int dwComponentID, NativeMethods.MSOCRINFOSTRUCT info)
		{
			return this._original != null && this._original.FUpdateComponentRegistration(this._componentId, info);
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x00052E28 File Offset: 0x00051E28
		bool UnsafeNativeMethods.IMsoComponentManager.FOnComponentActivate(int dwComponentID)
		{
			if (this._original == null)
			{
				return false;
			}
			if (this._components == null || dwComponentID <= 0 || !this._components.ContainsKey(dwComponentID))
			{
				return false;
			}
			if (!this._original.FOnComponentActivate(this._componentId))
			{
				return false;
			}
			this._activeComponent = this._components[dwComponentID];
			this._activeComponentId = dwComponentID;
			return true;
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x00052E8C File Offset: 0x00051E8C
		bool UnsafeNativeMethods.IMsoComponentManager.FSetTrackingComponent(int dwComponentID, bool fTrack)
		{
			if (this._original == null)
			{
				return false;
			}
			if (this._components == null || dwComponentID <= 0 || !this._components.ContainsKey(dwComponentID))
			{
				return false;
			}
			if (!this._original.FSetTrackingComponent(this._componentId, fTrack))
			{
				return false;
			}
			if (fTrack)
			{
				this._trackingComponent = this._components[dwComponentID];
				this._trackingComponentId = dwComponentID;
			}
			else
			{
				this._trackingComponent = null;
				this._trackingComponentId = 0;
			}
			return true;
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x00052F04 File Offset: 0x00051F04
		void UnsafeNativeMethods.IMsoComponentManager.OnComponentEnterState(int dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude, int dwReserved)
		{
			if (this._original == null)
			{
				return;
			}
			if ((uContext == 0 || uContext == 1) && this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					msoComponent.OnEnterState(uStateID, true);
				}
			}
			this._original.OnComponentEnterState(this._componentId, uStateID, uContext, cpicmExclude, rgpicmExclude, dwReserved);
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x00052F90 File Offset: 0x00051F90
		bool UnsafeNativeMethods.IMsoComponentManager.FOnComponentExitState(int dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude)
		{
			if (this._original == null)
			{
				return false;
			}
			if ((uContext == 0 || uContext == 1) && this._components != null)
			{
				foreach (UnsafeNativeMethods.IMsoComponent msoComponent in this._components.Values)
				{
					msoComponent.OnEnterState(uStateID, false);
				}
			}
			return this._original.FOnComponentExitState(this._componentId, uStateID, uContext, cpicmExclude, rgpicmExclude);
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x0005301C File Offset: 0x0005201C
		bool UnsafeNativeMethods.IMsoComponentManager.FInState(int uStateID, IntPtr pvoid)
		{
			return this._original != null && this._original.FInState(uStateID, pvoid);
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x00053035 File Offset: 0x00052035
		bool UnsafeNativeMethods.IMsoComponentManager.FContinueIdle()
		{
			return this._original != null && this._original.FContinueIdle();
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x0005304C File Offset: 0x0005204C
		bool UnsafeNativeMethods.IMsoComponentManager.FPushMessageLoop(int dwComponentID, int reason, int pvLoopData)
		{
			return this._original != null && this._original.FPushMessageLoop(this._componentId, reason, pvLoopData);
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x0005306B File Offset: 0x0005206B
		bool UnsafeNativeMethods.IMsoComponentManager.FCreateSubComponentManager(object punkOuter, object punkServProv, ref Guid riid, out IntPtr ppvObj)
		{
			if (this._original == null)
			{
				ppvObj = IntPtr.Zero;
				return false;
			}
			return this._original.FCreateSubComponentManager(punkOuter, punkServProv, ref riid, out ppvObj);
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x00053093 File Offset: 0x00052093
		bool UnsafeNativeMethods.IMsoComponentManager.FGetParentComponentManager(out UnsafeNativeMethods.IMsoComponentManager ppicm)
		{
			if (this._original == null)
			{
				ppicm = null;
				return false;
			}
			return this._original.FGetParentComponentManager(out ppicm);
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x000530B0 File Offset: 0x000520B0
		bool UnsafeNativeMethods.IMsoComponentManager.FGetActiveComponent(int dwgac, UnsafeNativeMethods.IMsoComponent[] ppic, NativeMethods.MSOCRINFOSTRUCT info, int dwReserved)
		{
			if (this._original == null)
			{
				return false;
			}
			if (this._original.FGetActiveComponent(dwgac, ppic, info, dwReserved))
			{
				if (ppic[0] == this)
				{
					if (dwgac == 0)
					{
						ppic[0] = this._activeComponent;
					}
					else if (dwgac == 1)
					{
						ppic[0] = this._trackingComponent;
					}
					else if (dwgac == 2 && this._trackingComponent != null)
					{
						ppic[0] = this._trackingComponent;
					}
				}
				return ppic[0] != null;
			}
			return false;
		}

		// Token: 0x04001598 RID: 5528
		private ComponentManagerBroker _broker;

		// Token: 0x04001599 RID: 5529
		private UnsafeNativeMethods.IMsoComponentManager _original;

		// Token: 0x0400159A RID: 5530
		private int _refCount;

		// Token: 0x0400159B RID: 5531
		private int _creationThread;

		// Token: 0x0400159C RID: 5532
		private int _componentId;

		// Token: 0x0400159D RID: 5533
		private int _nextComponentId;

		// Token: 0x0400159E RID: 5534
		private Dictionary<int, UnsafeNativeMethods.IMsoComponent> _components;

		// Token: 0x0400159F RID: 5535
		private UnsafeNativeMethods.IMsoComponent _activeComponent;

		// Token: 0x040015A0 RID: 5536
		private int _activeComponentId;

		// Token: 0x040015A1 RID: 5537
		private UnsafeNativeMethods.IMsoComponent _trackingComponent;

		// Token: 0x040015A2 RID: 5538
		private int _trackingComponentId;
	}
}
