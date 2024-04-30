using System.Diagnostics;
using Terraria;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;



// 主插件类应始终使用ApiVersion属性进行修饰。当前API版本为2.1
namespace ProcessBag
{
    [ApiVersion(2, 1)]
    public class MainPlugin : TerrariaPlugin
    {
        public static Config config = new Config();

        public override string Author => "少司命 羽学";

        public override string Description => "进度礼包";

        public override string Name => "进度礼包";

        public override Version Version => new Version(1, 0, 0, 7);

        internal static string LConfigPath => Path.Combine(TShock.SavePath, "进度礼包.json");

        public MainPlugin(Main game)
            : base(game)
        {
            this.Order = 3;
        }

        #region 注册与释放钩子
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Commands.ChatCommands.RemoveAll((Command f) => f.Name == "礼包");
            }
            base.Dispose(disposing);
        }

        public override void Initialize()
        {
            RC();
            Commands.ChatCommands.Add(new Command("bag.use", GiftBag, "礼包"));
        } 
        #endregion

        private void GiftBag(CommandArgs args)
        {
            CommandArgs args2 = args;
            if (args2.Parameters.Count > 0 && args2.Parameters[0].ToLower() == "help")
            {
                args2.Player.SendInfoMessage("/礼包 list");
                args2.Player.SendInfoMessage("/礼包 领取全部");
                args2.Player.SendInfoMessage("/礼包 领取 <礼包名称>");
                args2.Player.SendInfoMessage("/礼包 reload");
                args2.Player.SendInfoMessage("/礼包 重置");
            }
            else if (args2.Parameters.Count >= 1 && args2.Parameters[0] == "list")
            {
                List<string> list = new List<string>();
                foreach (Bag item in config.礼包)
                {
                    if (item.已领取玩家.Contains(args2.Player.Name))
                    {
                        list.Add("[" + item.礼包名称 + "] => " + "已领取".Color("AAFFAA"));
                    }
                    else
                    {
                        list.Add("[" + item.礼包名称 + "] => " + "未领取".Color("AAAAFF"));
                    }
                }
                ShowBag(list);
            }
            else if (args2.Parameters.Count == 1 && args2.Parameters[0] == "reload")
            {
                if (!args2.Player.HasPermission("bag.admin"))
                {
                    args2.Player.SendErrorMessage("没有足够权限执行此命令");
                    return;
                }
                RC();
                args2.Player.SendSuccessMessage("reload成功!");
            }
            else if (args2.Parameters.Count == 1 && args2.Parameters[0] == "重置")
            {
                if (!args2.Player.HasPermission("bag.admin"))
                {
                    args2.Player.SendErrorMessage("没有足够权限执行此命令");
                    return;
                }
                foreach (Bag item2 in config.礼包)
                {
                    item2.已领取玩家.Clear();
                }
                config.Write(LConfigPath);
                RC();
                args2.Player.SendSuccessMessage("礼包领取重置成功!");
            }
            else if (args2.Parameters.Count == 1 && args2.Parameters[0] == "领取全部")
            {
                int num = 0;
                foreach (Bag item3 in config.礼包)
                {
                    if ((item3.可领取组.Count > 0 && !item3.可领取组.Contains(args2.Player.Group.Name)) || !args2.Player.Query(item3.进度限制) || item3.已领取玩家.Contains(args2.Player.Name))
                    {
                        continue;
                    }
                    foreach (Award item4 in item3.礼包奖励)
                    {
                        args2.Player.GiveItem(item4.netID, item4.stack, item4.prefix);
                    }
                    foreach (string item5 in item3.执行命令)
                    {
                        ExecCommand.HandleCommand(args2.Player, item5);
                    }
                    num++;
                    TShock.Log.Write("[进度礼包]: " + args2.Player.Name + " 领取了 " + item3.礼包名称, TraceLevel.Info);
                    item3.已领取玩家.Add(args2.Player.Name);
                }
                config.Write(LConfigPath);
                RC();
                if (num > 0)
                {
                    args2.Player.SendSuccessMessage($"成功领取{num}个进度礼包!");
                }
                else
                {
                    args2.Player.SendErrorMessage("没有进度礼包可以领取!");
                }
            }
            else if (args2.Parameters.Count == 2 && args2.Parameters[0] == "领取")
            {
                foreach (Bag item6 in config.礼包)
                {
                    if (!(item6.礼包名称 == args2.Parameters[1]))
                    {
                        continue;
                    }
                    if (!args2.Player.Query(item6.进度限制))
                    {
                        args2.Player.SendErrorMessage("当前进度无法领取该礼包!");
                        return;
                    }
                    if (item6.可领取组.Count > 0 && !item6.可领取组.Contains(args2.Player.Group.Name))
                    {
                        args2.Player.SendErrorMessage("你当前所在的组无法领取该礼包!");
                        return;
                    }
                    if (!item6.已领取玩家.Contains(args2.Player.Name))
                    {
                        foreach (Award item7 in item6.礼包奖励)
                        {
                            args2.Player.GiveItem(item7.netID, item7.stack, item7.prefix);
                        }
                        foreach (string item8 in item6.执行命令)
                        {
                            ExecCommand.HandleCommand(args2.Player, item8);
                        }
                        TShock.Log.Write("[进度礼包]: " + args2.Player.Name + " 领取了 " + item6.礼包名称, TraceLevel.Info);
                        args2.Player.SendSuccessMessage("领取成功 [{0}] 礼包", item6.礼包名称);
                        item6.已领取玩家.Add(args2.Player.Name);
                        config.Write(LConfigPath);
                        RC();
                        return;
                    }
                    args2.Player.SendErrorMessage("[{0}] 礼包已经领取过了，不能重复领取", item6.礼包名称);
                }
                args2.Player.SendErrorMessage("没有此礼包");
            }
            else
            {
                args2.Player.SendInfoMessage("输入/礼包 help");
            }
            void ShowBag(List<string> line)
            {
                if (PaginationTools.TryParsePageNumber(args2.Parameters, 1, args2.Player, out var pageNumber))
                {
                    PaginationTools.SendPage(args2.Player, pageNumber, line, new PaginationTools.Settings
                    {
                        MaxLinesPerPage = 6,
                        HeaderFormat = "礼包列表 ({0}/{1})：",
                        FooterFormat = "输入 {0}礼包 list {{0}} 查看更多".SFormat(Commands.Specifier)
                    });
                }
            }
        }

        //判断怪物图鉴方法
        private bool IsDefeated(int type)
        {
            var unlockState = Main.BestiaryDB.FindEntryByNPCID(type).UIInfoProvider.GetEntryUICollectionInfo().UnlockState;
            return unlockState == Terraria.GameContent.Bestiary.BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
        }

        private Dictionary<string, bool> BossProcess()
        {
            return new Dictionary<string, bool>
        {
            { "无限制", true },
            {
                "史莱姆王",
                IsDefeated(NPCID.KingSlime)
            },
            {
                "克苏鲁之眼",
                IsDefeated(NPCID.EyeofCthulhu)
            },
            {
                "世界吞噬怪",
               IsDefeated(NPCID.EaterofWorldsHead)
            },
            {
                "克苏鲁之脑",
                IsDefeated(NPCID.BrainofCthulhu)
            },
            {
                "骷髅王",
                 IsDefeated(NPCID.SkeletronHead)
            },
            {
                "鹿角怪",
                 IsDefeated(NPCID.Deerclops)
            },
            {
                "蜂王",
                (IsDefeated(NPCID.QueenBee))
            },
            {
                "血肉墙",
                (IsDefeated(NPCID.WallofFlesh))
            },
            {
                "史莱姆皇后",
                (IsDefeated(NPCID.QueenSlimeBoss))
            },
            {
                "双子魔眼",
                (IsDefeated(NPCID.Retinazer))
            },
            {
                "毁灭者",
                IsDefeated(NPCID.TheDestroyer)
            },
            {
                "机械骷髅王",
                IsDefeated(NPCID.SkeletronPrime)
            },
            {
                "世纪之花",
                IsDefeated(NPCID.Plantera)
            },
            {
                "石巨人",
                IsDefeated(NPCID.Golem)
            },
            {
                "猪鲨",
                IsDefeated(NPCID.DukeFishron)
            },
            {
                "光之女皇",
                IsDefeated(NPCID.HallowBoss)
            },
            {
                "拜月教邪教徒",
                IsDefeated(NPCID.CultistBoss)
            },
            {
                "日耀柱",
                IsDefeated(NPCID.LunarTowerSolar)
            },
            {
                "星云柱",
                IsDefeated(NPCID.LunarTowerNebula)
            },
            {
                "星旋柱",
                IsDefeated(NPCID.LunarTowerVortex)
            },
            {
                "星尘柱",
                IsDefeated(NPCID.LunarTowerStardust)
            },
            {
                "月球领主",
                IsDefeated(NPCID.MoonLordCore)
            },
            {
                "雪人军团",
                NPC.downedFrost
            },
            {
                "哥布林军队",
                NPC.downedGoblins
            },
            {
                "海盗入侵",
                NPC.downedPirates
            },
            {
                "南瓜月",
                NPC.downedHalloweenKing
            },
            {
                "火星暴乱",
                NPC.downedMartians
            },
            {
                "霜月",
                NPC.downedChristmasIceQueen
            }
        };
        }

        private void RC()
        {
            try
            {
                if (File.Exists(LConfigPath))
                {
                    config = Config.Read(LConfigPath);
                }
                else
                {
                    foreach (KeyValuePair<string, bool> item in BossProcess())
                    {
                        Bag bag = new Bag();
                        bag.进度限制.Add(item.Key);
                        bag.礼包名称 = item.Key + "礼包";
                        bag.礼包奖励.Add(new Award());
                        config.礼包.Add(bag);
                    }
                    TShock.Log.ConsoleError("未找到进度礼包配置文件，已为您创建！");
                }
                config.Write(LConfigPath);
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleError("进度礼包配置读取错误:" + ex.ToString());
            }
        }
    }
}