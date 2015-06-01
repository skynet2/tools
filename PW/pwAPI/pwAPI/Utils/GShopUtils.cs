using System;
using System.Collections.Generic;
using System.Linq;
using Type = pwApi.Enums.Style.Type;
using Gender = pwApi.Enums.GShop.Gender;
using WeaponSub = pwApi.Enums.GShop.WeaponCat;
using pwApi.Readers;
using pwApi.StructuresElement;
using pwApi.StructuresGShop;

namespace pwApi.Utils
{
    public class GShopUtils
    {
        public static void CleanUp(GShopReader shop, ElementReader element, bool removeStyles = true, bool removeWeapon = true)
        {
            List<ShopItem> removeItems = new List<ShopItem>();
            foreach (var item in shop.Items)
            {
                foreach (var i in element.GetListById(84))
                {
                    if (!removeStyles && i.GetByKey("id_major_type") != (int)Type.Weapon)
                        continue;
                    if (!removeWeapon && i.GetByKey("id_major_type") == (int)Type.Weapon)
                        continue;
                    if (i.GetByKey("ID") != item.ItemId) continue;
                    removeItems.Add(item);
                    break;
                }
            }
            foreach (var rr in removeItems)
                shop.RemoveItem(rr);
        }


        public static void AddStyles(GShopReader shop, ElementReader element)
        {
            foreach (var ssin in RemoveStylesDuplicates(element))
            {
                var cat = ssin.GetByKey("gender") == 0 ? 6 : 7;
                int subcat = ssin.GetByKey("id_major_type");
                if (subcat == (int)Type.Weapon) continue;
                switch (subcat)
                {
                    case (int)Type.Up:
                        subcat = 0;
                        break;
                    case (int)Type.Down:
                        subcat = 1;
                        break;
                    case (int)Type.Boots:
                        subcat = 2;
                        break;
                    case (int)Type.Bracers:
                        subcat = 3;
                        break;
                    case (int)Type.Hat:
                        subcat = 4;
                        break;
                }
                shop.AddItem(ssin.GetByKey("ID"), cat, subcat, ssin.GetByKey("Name"), ssin.GetByKey("file_icon"), true);
            }
        }
        public static void AddWeapons(GShopReader shop, ElementReader element)
        {
            foreach (var ss in RemoveWeaponsDuplicates(element))
            {
                var cat = ss.GetByKey("gender") == 0 ? (int)Gender.Man : (int)Gender.Women;
                int subcat = ss.GetByKey("character_combo_id");
                switch (subcat)
                {
                    case 32:
                        subcat = (int)WeaponSub.Sin;
                        break;
                    case 4:
                        subcat = (int)WeaponSub.Sham;
                        break;
                    case 273:
                        subcat = (int)WeaponSub.Swords;
                        break;
                    case 64:
                        subcat = (int)WeaponSub.Archer;
                        break;
                    case 97:
                        subcat = (int)WeaponSub.Archer;
                        break;
                    case 17:
                        subcat = (int)WeaponSub.Bylav;
                        break;
                    case 650:
                        subcat = (int)WeaponSub.Posox;
                        break;
                    case 89:
                        subcat = (int)WeaponSub.Kastet;
                        break;
                    default:
                        subcat = (int)WeaponSub.Diff;
                        break;
                }
                shop.AddItem(ss.GetByKey("ID"), cat, subcat, ss.GetByKey("Name"), ss.GetByKey("file_icon"), true);
            }
        }

        private static List<Item> RemoveWeaponsDuplicates(ElementReader element)
        {
            var result = new List<Item>();
            var weapons = element.GetListById(84).Where(i => i.GetByKey("id_major_type") == (int)Type.Weapon).ToList();
            Console.WriteLine("Total weapons : " + weapons.Count);
            const string temp =
                "\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000\u0000";
            // VALUES //
            int GENDER = weapons[0].GetPos("gender");
            int FILE_MODEL_RIGHT = weapons[0].GetPos("file_model_right");
            int FILE_MODEL_LEFT = weapons[0].GetPos("file_model_left");
            // VALUES //
            foreach (Item current in weapons)
            {
                foreach (Item next in weapons)
                {
                    if (result.Contains(current) || result.Contains(next) || current.Equals(next) ||
                        current.GetByPos(GENDER).Equals(next.GetByPos(GENDER))) continue;
                    if (!current.GetByPos(FILE_MODEL_RIGHT).Equals(temp) &&
                        current.GetByPos(FILE_MODEL_RIGHT).Equals(next.GetByPos(FILE_MODEL_RIGHT)))
                    {
                        result.AddRange(new[] { next, current });
                        break;
                    }
                    if (!current.GetByPos(FILE_MODEL_LEFT).Equals(temp) &&
                        current.GetByPos(FILE_MODEL_LEFT).Equals(next.GetByPos(FILE_MODEL_LEFT)))
                    {
                        result.AddRange(new[] { next, current });
                    }
                }
            }
            Console.WriteLine("Size after : " + result.Count);
            return result;
        }

        private static List<Item> RemoveStylesDuplicates(ElementReader element)
        {
            var index = new List<Item>();
            var elementStyles = element.GetListById(84).ToList();
            Console.WriteLine("Size before " + elementStyles.Count);

            // VALUES //
            int REALNAME = elementStyles[0].GetPos("realname");
            int GENDER = elementStyles[0].GetPos("gender");
            int ID_MAJOR_TYPE = elementStyles[0].GetPos("id_major_type");
            int REQUIRE_DYE_COUNT = elementStyles[0].GetPos("require_dye_count");
            // VALUES //

            for (int i = 0; i < elementStyles.Count; i++)
            {
                for (int j = i + 1; j < elementStyles.Count; j++)
                {
                    var now = elementStyles[i];
                    var next = elementStyles[j];
                    if (now.GetByPos(REALNAME) != next.GetByPos(REALNAME) || now.GetByPos(GENDER) != next.GetByPos(GENDER) ||
                        now.GetByPos(ID_MAJOR_TYPE) != (int)Type.Weapon || next.GetByPos(ID_MAJOR_TYPE) != (int)Type.Weapon) continue;
                    if ((now.GetByPos(REQUIRE_DYE_COUNT) > 0 && next.GetByPos(REQUIRE_DYE_COUNT) == 0))
                        index.Add(next);
                    else if (now.GetByPos(REQUIRE_DYE_COUNT) == 0 && next.GetByPos(REQUIRE_DYE_COUNT) > 0)
                        index.Add(now);
                    else if (now.GetByPos(REQUIRE_DYE_COUNT) == next.GetByPos(REQUIRE_DYE_COUNT))
                        index.Add(next);
                }
            }
            foreach (var style in index)
                elementStyles.Remove(style);
            Console.WriteLine("Size after " + elementStyles.Count);
            return elementStyles;
        }
    }
}
