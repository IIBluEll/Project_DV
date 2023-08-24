using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAction : MonoBehaviour
{
   [SerializeField] private PlayerInputManager inputMgr;
   [SerializeField] private WeaponFunction weaponFunc;

   private void Update()
   {
      if (inputMgr.MouseButton0_Down && inputMgr.MouseButton1_Down)
      {
         weaponFunc.StartWeaponFire();
      }
      else if (inputMgr.MouseButton0_Up)
      {
          weaponFunc.StopWeaponFire();
      }

      if (inputMgr.IsReload)
      {
         // 재장전
      }
   }
}
