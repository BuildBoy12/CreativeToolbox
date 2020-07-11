using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreativeToolbox
{
    public static class Extensions
    {
        public static void SetWeaponAmmo(this ReferenceHub rh, int amount)
        {
            rh.inventory.items.ModifyDuration(
            rh.inventory.items.IndexOf(rh.inventory.GetItemInHand()),
            amount);
        }

        public static bool IsGun(this ItemType item)
        {
            switch (item)
            {
                case ItemType.GunCOM15:
                case ItemType.GunE11SR:
                case ItemType.GunLogicer:
                case ItemType.GunMP7:
                case ItemType.GunProject90:
                case ItemType.GunUSP:
                case ItemType.MicroHID:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsSCP(this RoleType role)
        {
            switch (role)
            {
                case RoleType.Scp049:
                case RoleType.Scp0492:
                case RoleType.Scp079:
                case RoleType.Scp096:
                case RoleType.Scp106:
                case RoleType.Scp173:
                case RoleType.Scp93953:
                case RoleType.Scp93989:
                    return true;
                default:
                    return false;
            }
        }

        public static bool ItemInHandIsKeycard(this ReferenceHub rh)
        {
            ItemCategory ItemCat = rh.inventory.GetItemByID(rh.inventory.curItem)?.itemCategory ?? ItemCategory.None;
            return ItemCat == ItemCategory.Keycard;
        }
    }
}
