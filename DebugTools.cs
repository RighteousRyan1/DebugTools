using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace DebugTools
{
    public class DebugTools : Mod
    {
        public class HurtSelf : ModCommand
        {
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "hurt";

            public override string Usage
                => "/hurt <damage>";

            public override string Description
                => "Hurt yourself for any amount of damage";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                Player player = Main.player[Main.myPlayer];
                var damageAmount = args[0];
                if (!int.TryParse(args[0], out int type))
                {
                    if (type == 0)
                    {
                        throw new UsageException($"{damageAmount} is not a valid integer.");
                    }
                }

                int damage = 1;
                if (args.Length >= 2)
                {
                    damage = int.Parse(args[1]);
                }
                caller.Player.Hurt(PlayerDeathReason.ByCustomReason($"{player.name} hurt themself a bit too much."), type, 0, false, false, false, -1);
            }
        }
        public class PlayerVelocity : ModCommand
        {
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "velocity";

            public override string Usage
                => "/velocity <velocityX> <velocityY>"; // <time>

            public override string Description
                => "Set your player's velocity to <velocityX, velocityY> for 1 tick"; // X seconds

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                Player player = Main.player[Main.myPlayer];
                var velX = args[0];
                var velY = args[1];
                // var time = args[2];
                if (!int.TryParse(args[0], out int type))
                {
                    if (type == 0)
                    {
                        // Main.NewText($"{velX} is not a valid integer.");
                        return;
                    }
                }
                if (!int.TryParse(args[1], out int type1))
                {
                    if (type1 == 0)
                    {
                        // Main.NewText($"{velY} is not a valid integer.");
                        return;
                    }
                }
                caller.Player.velocity = new Vector2(type, type1);
                
            }
        }
        public class FrameCommand : ModCommand
        {
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "disableiframes";

            public override string Usage
                => "/disableiframes";

            public override string Description
                => "Disable iFrames for the player";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                Player player = Main.player[Main.myPlayer];
                if (!LePlayer.iFrameDisabled)
                {
                    LePlayer.iFrameDisabled = true;
                    // Main.NewText("iFrames have been disabled.", Color.Red);
                }
                else if (LePlayer.iFrameDisabled)
                {
                    LePlayer.iFrameDisabled = false;
                    // Main.NewText("iFrames have been enabled.", Color.Green);
                }
            }
        }
        public class ProjInfo : ModCommand
        {
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "projInfo";

            public override string Usage
                => "/projInfo";

            public override string Description
                => "| display projectile info ";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                Player player = Main.player[Main.myPlayer];
                if (!LePlayer.extraProjectileInformation)
                {
                    LePlayer.extraProjectileInformation = true;
                    //Main.NewText("Projectile info is now being displayed.", Color.Green);
                }
                else if (LePlayer.extraProjectileInformation)
                {
                    LePlayer.extraProjectileInformation = false;
                    //Main.NewText("Projectile info is no longer being displayed.", Color.Red);
                }
            }
        }
        public class PInfo : ModCommand
        {
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "playerInfo";

            public override string Usage
                => "/playerInfo";

            public override string Description
                => "| display player info ";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                Player player = Main.player[Main.myPlayer];
                if (!LePlayer.playerInfo)
                {
                    LePlayer.playerInfo = true;
                    //Main.NewText("Player info is now being displayed.", Color.Green);
                }
                else if (LePlayer.playerInfo)
                {
                    LePlayer.playerInfo = false;
                    //Main.NewText("Player info is no longer being displayed.", Color.Red);
                }
            }
        }
        public class SetMaxLife : ModCommand
        {
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "lifeSet";

            public override string Usage
                => "/lifeSet <life>";

            public override string Description
                => "Set your max life";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                Player player = Main.player[Main.myPlayer];
                var lifeAmt = args[0];
                if (!int.TryParse(args[0], out int type))
                {
                    if (type == 0)
                    {
                        throw new UsageException($"{lifeAmt} is not a valid integer.");
                    }
                }
                int lifeInt = 1;
                if (args.Length >= 2)
                {
                    lifeInt = int.Parse(args[1]);
                }
                player.statLifeMax = type;
            }
        }
        public class PlaySound : ModCommand
        {
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "playSound";

            public override string Usage
                => "/playSound <legacy (true | false)> <type>";

            public override string Description
                => "Play a sound";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                Player player = Main.player[Main.myPlayer];
                if (args[0] != "true" && args[0] != "false")
                {
                    Main.NewText("Please provide a valid boolean value (true, false (Case sensitive)).", Color.Red);
                    return;
                }
                int x = int.Parse(args[1]);
                if (!int.TryParse(args[1], out int result) || x > 125 || x < 1)
                {
                    Main.NewText("Invalid integer.");
                    return;
                }
                if (args[0] == "true")
                {
                    SoundEngine.PlaySound(typeof(Main).Assembly.GetType("Terraria.ID.SoundID").GetField("Item" + x).GetValue(null) as Terraria.Audio.LegacySoundStyle);
                }
                if (args[0] == "false")
                {
                    SoundEngine.PlaySound(x);
                }
            }
        }
        /// <summary>
        /// Rename yourself!
        /// </summary>
        public class Rename : ModCommand
        {
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "rename";

            public override string Usage
                => "/rename <string>";

            public override string Description
                => "change your name"
                    + "\n Use $ to represent spaces.";
            public override void Action(CommandCaller caller, string input, string[] args)
            {
                Player player = Main.player[Main.myPlayer];
                if (args[0].Length < 21)
                {
                    var name = args[0].Replace("$", " ");
                    Main.NewText($"Your old name was {player.name}.", Color.LightBlue);
                    player.name = name;
                    Main.NewText($"Now your name has been changed to {name}.", Color.Aqua);
                }
                else 
                { 
                    Main.NewText("Please keep your name under 21 characters.", Color.Red); 
                }
                /*ModPacket NamePack = mod.GetPacket();
                NamePack.Write(player.name);
                NamePack.Send();*/
            }
        }
        public class Heal : ModCommand
        {
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "heal";

            public override string Usage
                => "/heal";

            public override string Description
                => "Heals you back to full health";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                Player player = Main.player[Main.myPlayer];
                    caller.Player.statLife = player.statLifeMax2;
                    Main.NewText("You have been healed.", Color.Coral);
            }
        }
        public class DisableSpawns : ModCommand
        {
            public override CommandType Type
               => CommandType.Chat;

            public override string Command
                => "spawnDisable";

            public override string Usage
                => "/spawnDisable";

            public override string Description
                => "disables all spawns";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                spawnsStopped = !spawnsStopped;
            }
        }
        public class NPCSummon : ModCommand
        {
			
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "summon";

            public override string Usage
                => "/summon <npcname>";

            public override string Description
                => "Spawn an NPC";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                if (int.TryParse(args[0], out int type))
                {
                    return;
                }
                var name = args[0];// args[0].Replace("_", " ");

                var canID = NPCID.Search.TryGetId(name, out int id);

                if (!canID)
                {
                    Main.NewText("Unknown NPC: " + name);
                }

                int amount = 1;
                if (args.Length >= 2)
                {
                    amount = int.Parse(args[1]);
                }

                for (int i = 0; i < amount; i++)
                    NPC.NewNPC(
				(int)(Main.MouseWorld.X + Main.rand.NextFloat(-10 * amount, 10 * amount)), 
				(int)(Main.MouseWorld.Y + Main.rand.NextFloat(0, -10 * amount)), 
				id);
            }
        }
		public class ItemCommand : ModCommand
		{
			public override CommandType Type
				=> CommandType.Chat;

			public override string Command
				=> "item";
	
			public override string Usage
				=> "/item <type|name> [stack]" +
				"\nReplace spaces in item name with underscores";

			public override string Description
				=> "Spawn an item";
	
			public override void Action(CommandCaller caller, string input, string[] args) {
				if (!int.TryParse(args[0], out int type)) {
					var name = args[0].Replace("_", " ");
					var item = new Item();
					for (var k = 0; k < ItemLoader.ItemCount; k++) {
						item.SetDefaults(k, true);
						if (name != Lang.GetItemNameValue(k)) {
							continue;
						}

						type = k;
						break;
					}

					if (type == 0) {
						throw new UsageException("Unknown item: " + name);
					}
				}

				int stack = 1;
				if (args.Length >= 2) {
					stack = int.Parse(args[1]);
				}

				caller.Player.QuickSpawnItem(type, stack);
			}
		}
        public static bool spawnsStopped;
        public class StopSpawnsNPC : GlobalNPC
        {
            public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
            {
                if (spawnsStopped)
                    pool.Clear();
            }
        }
        public class Butcher : ModCommand
        {
            public override CommandType Type
               => CommandType.Chat;

            public override string Command
                => "butcher";

            public override string Usage
                => "/butcher";

            public override string Description
                => "kill all NPCs";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                foreach (var npc in Main.npc)
                {
                    if (npc.active)
                    {
                        npc.StrikeNPC(npc.lifeMax + 50, 0, 0);
                    }
                }
            }
        }
        public class ScroogeMcDuck : ModCommand
        {
            public override CommandType Type
               => CommandType.Chat;

            public override string Command
                => "scrooge";

            public override string Usage
                => "/scrooge";

            public override string Description
                => "here comes the money";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                caller.Player.QuickSpawnItem(ItemID.PlatinumCoin, 999);
            }
        }
        public class ModifyBodyFrames : ModCommand
        {
            public override CommandType Type
               => CommandType.Chat;

            public override string Command
                => "modifyBodyFrames";

            public override string Usage
                => "/modifyBodyFrames";

            public override string Description
                => "modify body frames";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                if (args[0].ToLower() == "reset")
                    canModify = false;
                else if(args[0].ToLower() == "begin")
                    canModify = true;
                if (args.Length >= 2)
                {
                    if (int.TryParse(args[1], out int headFrame) && int.TryParse(args[2], out int bodyFrame) && int.TryParse(args[3], out int legFrame))
                    {
                        newHeadFrame = headFrame;
                        newBodyFrame = bodyFrame;
                        newLegFrame = legFrame;
                    }
                }
            }
        }
        public static int newHeadFrame;
        public static int newBodyFrame;
        public static int newLegFrame;
        public static bool canModify;
    }
    public class ModifyBodyFramePlayer : ModPlayer
    {
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (DebugTools.canModify)
            {
                drawInfo.drawPlayer.headFrame.Y = drawInfo.drawPlayer.headFrame.Height * DebugTools.newHeadFrame;
                drawInfo.drawPlayer.legFrame.Y = drawInfo.drawPlayer.legFrame.Height * DebugTools.newLegFrame;
                drawInfo.drawPlayer.bodyFrame.Y = drawInfo.drawPlayer.bodyFrame.Height * DebugTools.newBodyFrame;
            }
        }
    }
}