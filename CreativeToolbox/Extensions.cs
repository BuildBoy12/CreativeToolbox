namespace CreativeToolbox
{
    using Exiled.API.Features;

    public static class Extensions
    {
        public static void SetWeaponAmmo(this Player ply, int amount)
        {
            ply.Inventory.items.ModifyDuration(
                ply.Inventory.items.IndexOf(ply.Inventory.GetItemInHand()),
                amount);
        }

        public static bool ItemInHandIsKeycard(this Player ply)
        {
            ItemCategory itemCat = ply.Inventory.GetItemByID(ply.Inventory.curItem)?.itemCategory ?? ItemCategory.None;
            return itemCat == ItemCategory.Keycard;
        }
    }
}