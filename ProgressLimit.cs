using Terraria;
using Terraria.ID;
using Terraria.GameContent.Events;
using TShockAPI;
using ProcessBag;

namespace ProcessBag
{
    public static class ProgressLimit
    {
        private static bool IsDefeated(int type)
        {
            var unlockState = Main.BestiaryDB.FindEntryByNPCID(type).UIInfoProvider.GetEntryUICollectionInfo().UnlockState;
            return unlockState == Terraria.GameContent.Bestiary.BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
        }

        public static bool Query(this TSPlayer ply, IEnumerable<string> progress)
        {
            TSPlayer ply2 = ply;
            bool response = true;
            progress.ForEach(delegate (string x)
            {
                if (!Get(x, ply2.Index))
                {
                    response = false;
                }
            });
            return response;
        }

        public static bool Get(string s, int plrIndex)
        {
            bool result = true;
            switch (s)
            {
                case "史莱姆王":
                case "史王":
                    if(!(IsDefeated(NPCID.KingSlime)))
                    {
                        result = false;
                    }
                    break;
                case "克眼":
                case "克苏鲁之眼":
                    if(!(IsDefeated(NPCID.EyeofCthulhu)))
                    {
                        result = false;
                    }
                    break;
                case "巨鹿":
                case "鹿角怪":
                    if(!(IsDefeated(NPCID.Deerclops)))
                    {
                        result = false;
                    }
                    break;
                case "世吞":
                case "世界吞噬者":
                case "世界吞噬怪":
                    if(!(IsDefeated(NPCID.EaterofWorldsHead)))
                    {
                        result = false;
                    }
                    break;
                case "克脑":
                case "克苏鲁之脑":
                    if(!(IsDefeated(NPCID.BrainofCthulhu)))
                    {
                        result = false;
                    }
                    break;
                case "蜂王":
                    if (!(IsDefeated(NPCID.QueenBee)))
                    {
                        result = false;
                    }
                    break;
                case "骷髅王":
                    if (!(IsDefeated(NPCID.SkeletronHead)))
                    {
                        result = false;
                    }
                    break;
                case "困难模式":
                case "肉后":
                case "血肉墙":
                    if (!(IsDefeated(NPCID.WallofFlesh)))
                    {
                        result = false;
                    }
                    break;
                case "毁灭者":
                    if (!(IsDefeated(NPCID.TheDestroyer)))
                    {
                        result = false;
                    }
                    break;
                case "双子魔眼":
                    if (!(IsDefeated(NPCID.Retinazer)))
                    {
                        result = false;
                    }
                    break;
                case "机械骷髅王":
                    if (!(IsDefeated(NPCID.SkeletronPrime)))
                    {
                        result = false;
                    }
                    break;
                case "世纪之花":
                case "花后":
                case "世花":
                    if (!(IsDefeated(NPCID.Plantera)))
                    {
                        result = false;
                    }
                    break;
                case "石后":
                case "石巨人":
                    if (!(IsDefeated(NPCID.Golem)))
                    {
                        result = false;
                    }
                    break;
                case "史后":
                case "史莱姆皇后":
                    if (!(IsDefeated(NPCID.QueenSlimeBoss)))
                    {
                        result = false;
                    }
                    break;
                case "光之女皇":
                case "光女":
                    if (!(IsDefeated(NPCID.HallowBoss)))
                    {
                        result = false;
                    }
                    break;
                case "猪鲨":
                case "猪龙鱼公爵":
                    if (!(IsDefeated(NPCID.DukeFishron)))
                    {
                        result = false;
                    }
                    break;
                case "教徒":
                case "拜月教邪教徒":
                    if (!(IsDefeated(NPCID.CultistBoss)))
                    {
                        result = false;
                    }
                    break;
                case "月亮领主":
                    if (!(IsDefeated(NPCID.MoonLordCore)))
                    {
                        result = false;
                    }
                    break;
                case "哀木":
                    if (!NPC.downedHalloweenTree)
                    {
                        result = false;
                    }
                    break;
                case "南瓜王":
                    if (!NPC.downedHalloweenKing)
                    {
                        result = false;
                    }
                    break;
                case "常绿尖叫怪":
                    if (!NPC.downedChristmasTree)
                    {
                        result = false;
                    }
                    break;
                case "冰雪女王":
                    if (!NPC.downedChristmasIceQueen)
                    {
                        result = false;
                    }
                    break;
                case "圣诞坦克":
                    if (!NPC.downedChristmasSantank)
                    {
                        result = false;
                    }
                    break;
                case "火星飞碟":
                    if (!NPC.downedMartians)
                    {
                        result = false;
                    }
                    break;
                case "小丑":
                    if (!NPC.downedClown)
                    {
                        result = false;
                    }
                    break;
                case "日耀柱":
                    if (!NPC.downedTowerSolar)
                    {
                        result = false;
                    }
                    break;
                case "星旋柱":
                    if (!NPC.downedTowerVortex)
                    {
                        result = false;
                    }
                    break;
                case "星云柱":
                    if (!NPC.downedTowerNebula)
                    {
                        result = false;
                    }
                    break;
                case "星尘柱":
                    if (!NPC.downedTowerStardust)
                    {
                        result = false;
                    }
                    break;
                case "一王后":
                    if (!NPC.downedMechBossAny)
                    {
                        result = false;
                    }
                    break;
                case "三王后":
                    if (!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3)
                    {
                        result = false;
                    }
                    break;
                case "一柱后":
                    result = NPC.downedTowerNebula || NPC.downedTowerSolar || NPC.downedTowerStardust || NPC.downedTowerVortex;
                    break;
                case "四柱后":
                    if (!NPC.downedTowerNebula || !NPC.downedTowerSolar || !NPC.downedTowerStardust || !NPC.downedTowerVortex)
                    {
                        result = false;
                    }
                    break;
                case "哥布林入侵":
                    if (!NPC.downedGoblins)
                    {
                        result = false;
                    }
                    break;
                case "海盗入侵":
                    if (!NPC.downedPirates)
                    {
                        result = false;
                    }
                    break;
                case "霜月":
                    if (!NPC.downedFrost)
                    {
                        result = false;
                    }
                    break;
                case "血月":
                    if (!Main.bloodMoon)
                    {
                        result = false;
                    }
                    break;
                case "雨天":
                    if (!Main.raining)
                    {
                        result = false;
                    }
                    break;
                case "白天":
                    if (!Main.dayTime)
                    {
                        result = false;
                    }
                    break;
                case "晚上":
                    if (Main.dayTime)
                    {
                        result = false;
                    }
                    break;
                case "大风天":
                    if (!Main.IsItAHappyWindyDay)
                    {
                        result = false;
                    }
                    break;
                case "万圣节":
                    if (!Main.halloween)
                    {
                        result = false;
                    }
                    break;
                case "圣诞节":
                    if (!Main.xMas)
                    {
                        result = false;
                    }
                    break;
                case "派对":
                    if (!BirthdayParty.PartyIsUp)
                    {
                        result = false;
                    }
                    break;
                case "2020":
                    if (!Main.drunkWorld)
                    {
                        result = false;
                    }
                    break;
                case "2021":
                    if (!Main.tenthAnniversaryWorld)
                    {
                        result = false;
                    }
                    break;
                case "ftw":
                    if (!Main.getGoodWorld)
                    {
                        result = false;
                    }
                    break;
                case "ntb":
                    if (!Main.notTheBeesWorld)
                    {
                        result = false;
                    }
                    break;
                case "dst":
                    if (!Main.dontStarveWorld)
                    {
                        result = false;
                    }
                    break;
                case "森林":
                    if (!Main.player[plrIndex].ShoppingZone_Forest)
                    {
                        result = false;
                    }
                    break;
                case "丛林":
                    if (!Main.player[plrIndex].ZoneJungle)
                    {
                        result = false;
                    }
                    break;
                case "沙漠":
                    if (!Main.player[plrIndex].ZoneDesert)
                    {
                        result = false;
                    }
                    break;
                case "雪原":
                    if (!Main.player[plrIndex].ZoneSnow)
                    {
                        result = false;
                    }
                    break;
                case "洞穴":
                    if (!Main.player[plrIndex].ZoneUnderworldHeight)
                    {
                        result = false;
                    }
                    break;
                case "海洋":
                    if (!Main.player[plrIndex].ZoneBeach)
                    {
                        result = false;
                    }
                    break;
                case "神圣":
                    if (!Main.player[plrIndex].ZoneHallow)
                    {
                        result = false;
                    }
                    break;
                case "蘑菇":
                    if (!Main.player[plrIndex].ZoneGlowshroom)
                    {
                        result = false;
                    }
                    break;
                case "腐化之地":
                    if (!Main.player[plrIndex].ZoneCorrupt)
                    {
                        result = false;
                    }
                    break;
                case "猩红之地":
                    if (!Main.player[plrIndex].ZoneCrimson)
                    {
                        result = false;
                    }
                    break;
                case "地牢":
                    if (!Main.player[plrIndex].ZoneDungeon)
                    {
                        result = false;
                    }
                    break;
                case "墓地":
                    if (!Main.player[plrIndex].ZoneGraveyard)
                    {
                        result = false;
                    }
                    break;
                case "蜂巢":
                    if (!Main.player[plrIndex].ZoneHive)
                    {
                        result = false;
                    }
                    break;
                case "神庙":
                    if (!Main.player[plrIndex].ZoneLihzhardTemple)
                    {
                        result = false;
                    }
                    break;
                case "沙尘暴":
                    if (!Main.player[plrIndex].ZoneSandstorm)
                    {
                        result = false;
                    }
                    break;
                case "天空":
                    if (!Main.player[plrIndex].ZoneSkyHeight)
                    {
                        result = false;
                    }
                    break;
                case "满月":
                    if (Main.moonPhase != 0)
                    {
                        result = false;
                    }
                    break;
                case "亏凸月":
                    if (Main.moonPhase != 1)
                    {
                        result = false;
                    }
                    break;
                case "下弦月":
                    if (Main.moonPhase != 2)
                    {
                        result = false;
                    }
                    break;
                case "残月":
                    if (Main.moonPhase != 3)
                    {
                        result = false;
                    }
                    break;
                case "新月":
                    if (Main.moonPhase != 4)
                    {
                        result = false;
                    }
                    break;
                case "娥眉月":
                    if (Main.moonPhase != 5)
                    {
                        result = false;
                    }
                    break;
                case "上弦月":
                    if (Main.moonPhase != 6)
                    {
                        result = false;
                    }
                    break;
                case "盈凸月":
                    if (Main.moonPhase != 7)
                    {
                        result = false;
                    }
                    break;
            }
            return result;
        }
    }
}
