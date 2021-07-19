using DebugTools.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Graphics;
using System.Text.RegularExpressions;
using System.Text;

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
                        Main.NewText($"{damageAmount} is not a valid integer.");
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
                        Main.NewText($"{velX} is not a valid integer.");
                        return;
                    }
                }
                if (!int.TryParse(args[1], out int type1))
                {
                    if (type1 == 0)
                    {
                        Main.NewText($"{velY} is not a valid integer.");
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
                    Main.NewText("iFrames have been disabled.", Color.Red);
                }
                else if (LePlayer.iFrameDisabled)
                {
                    LePlayer.iFrameDisabled = false;
                    Main.NewText("iFrames have been enabled.", Color.Green);
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
                    Main.NewText("Projectile info is now being displayed.", Color.Green);
                }
                else if (LePlayer.extraProjectileInformation)
                {
                    LePlayer.extraProjectileInformation = false;
                    Main.NewText("Projectile info is no longer being displayed.", Color.Red);
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
                    Main.NewText("Player info is now being displayed.", Color.Green);
                }
                else if (LePlayer.playerInfo)
                {
                    LePlayer.playerInfo = false;
                    Main.NewText("Player info is no longer being displayed.", Color.Red);
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
                        Main.NewText($"{lifeAmt} is not a valid integer.");
                    }
                }
                int lifeInt = 1;
                if (args.Length >= 2)
                {
					Main.NewText($"Life has been set to {args[1]}");
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
				Main.NewText(spawnsStopped ? "Spawns have been stopped." : "Spawns have been turned back on.");
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
                    Main.NewText($"Unknown NPC: {name}");
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
				Main.NewText(amount == 1 ? $"Spawned {name} {amount} time." : $"Spawned {name} {amount} times.");
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
				int butcherAmount = 0;
                foreach (var npc in Main.npc)
                {
                    if (npc.active)
                    {
						butcherAmount++;
                        npc.StrikeNPC(npc.lifeMax + 50, 0, 0);
                    }
                }
				Main.NewText($"Butchered {butcherAmount} NPCs.");
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
        public class TimeCommand : ModCommand
        {
            public override CommandType Type
               => CommandType.Chat;

            public override string Command
                => "time";

            public override string Usage
                => "/time <day/night> <ticks>";

            public override string Description
                => "set the time (dawn, latemorning, noon, afternoon, dusk, midnight can be alternatives for <day/night>, then <ticks> is irrelevant)";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                if (args.Length >= 3)
                {
                    Main.NewText($"Too many arguments.", Color.Red);
                    return;
                }
                if (args[0] == "day")
                    Main.dayTime = true;
                else if (args[0] == "night")
                    Main.dayTime = false;

                string timePass = args[0];

                if (timePass.ToLower() == "dawn")
                {
                    Main.dayTime = true;
                    Main.time = 0;
					Main.NewText($"Changed the time to {Main.time}.");
                    return;
                }
                if (timePass.ToLower() == "latemorning")
                {
                    Main.dayTime = true;
                    Main.time = 9000;
					Main.NewText($"Changed the time to {Main.time}.");
                    return;
                }
                if (timePass.ToLower() == "noon")
                {
                    Main.dayTime = true;
                    Main.time = Main.dayLength / 2;
					Main.NewText($"Changed the time to {Main.time}.");
                    return;
                }
                if (timePass.ToLower() == "afternoon")
                {
                    Main.dayTime = true;
                    Main.time = (int)(Main.dayLength * 0.75f);
					Main.NewText($"Changed the time to {Main.time}.");
                    return;
                }
                if (timePass.ToLower() == "dusk")
                {
                    Main.dayTime = false;
                    Main.time = 0;
					Main.NewText($"Changed the time to {Main.time}.");
                    return;
                }
                if (timePass.ToLower() == "midnight")
                {
                    Main.dayTime = false;
                    Main.time = Main.nightLength / 2;
					Main.NewText($"Changed the time to {Main.time}.");
                    return;
                }
                bool canParse = int.TryParse(args[1], out int time);

                if (canParse)
                {
                    Main.time = time;
					Main.NewText($"Changed the time to {Main.time}.");
                    return;
                }
            }
        }
        public class UseTimeCommand : ModCommand
        {
            public override CommandType Type
               => CommandType.Chat;

            public override string Command
                => "useTime";

            public override string Usage
                => "/useTime <ticks>";

            public override string Description
                => "change your held item's useTime and useAnimation";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                if (int.TryParse(args[0], out int usetime))
                {
                    caller.Player.HeldItem.useTime = usetime;
                    caller.Player.HeldItem.useAnimation = usetime;
					
					Main.NewText($"Changed your held item's useTime and useAnimation to {usetime} ticks.");
                }
                else if (args[0].ToLower() == "setdefaults")
				{
                    caller.Player.HeldItem.SetDefaults(caller.Player.HeldItem.type);
					Main.NewText($"Changed your held item's useTime and useAnimation back to their defaults.");
				}
            }
        }
        public class ModifyWeather : ModCommand
        {
            public override CommandType Type
               => CommandType.Chat;

            public override string Command
                => "weather";

            public override string Usage
                => "/weather <clear/sun, rain, storm, sandstorm>";

            public override string Description
                => "begin/end weather";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                string weather = args[0];

                if (weather.ToLower() == "sun" || weather.ToLower() == "clear")
                {
                    Main.maxRaining = 0f;
                    Main.rainTime = 0;
                    Main.raining = false;
                    Sandstorm.Happening = false;
					Main.NewText($"Weather has been cleared.");
					return;
                }
                if (weather.ToLower() == "rain")
                {
                    Main.maxRaining = Main.rand.NextFloat(0.01f, 0.2f);
                    Main.rainTime = Main.rand.Next(10000, 20000);
                    Main.raining = true;
					Main.NewText($"Weather has been set to rain.");
					return;
                }
                if (weather.ToLower() == "storm")
                {
                    Main.maxRaining = Main.rand.NextFloat(0.5f, 1f);
                    Main.rainTime = Main.rand.Next(10000, 20000);
                    Main.raining = true;
					Main.NewText($"Weather has been set to thunderstorm.");
					return;
                }
                if (weather.ToLower() == "sandstorm")
                {
                    Sandstorm.Happening = true;
                    Sandstorm.IntendedSeverity = Main.rand.NextFloat();
					Main.NewText($"A sandstorm has been started.");
					return;
                }
            }
        }
        public class SpawnHellevator : ModCommand
        {
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "spawnHellevator";

            public override string Usage
                => "/spawnHellevator <width>";

            public override string Description
                => "spawn a hellevator";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                if (int.TryParse(args[0], out int width))
                {
                    for (int i = (int)caller.Player.Center.X / 16 - width; i < (int)caller.Player.Center.X / 16 + width; i++)
                    {
                        for (int j = (int)caller.Player.Center.Y / 16; j < Main.maxTilesY - 175; j++)
                        {
                            var tile = Main.tile[i, j];

                            tile.IsActive = false;
                            WorldGen.SquareTileFrame(i, j);

                            tile.LiquidAmount = 0;
                            tile.LiquidType = 0;
                        }
                    }
					Main.NewText($"Spawned a hellevator with a width of {width}.");
                }
                else
                    Main.NewText($"'{args[0]}' is not a valid argument.", Color.Red);
            }
        }
        public class TPCommand : ModCommand
        {
            public override CommandType Type
                => CommandType.Chat;

            public override string Command
                => "tp";

            public override string Usage
                => "/tp";

            public override string Description
                => "teleport to a location <x, y>, or <hell, dungeon, spawn, space, caverns, beachleft, beachright>\nDo note that these should be TILE COORDINATES";

            public override void Action(CommandCaller caller, string input, string[] args)
            {
                var player = caller.Player;

                if (args.Length == 0)
                    Main.NewText($"Please provide arguments.");
                if (int.TryParse(args[0], out int x))
                {
                    if (int.TryParse(args[1], out int y))
                    {
                        player.Center = new Vector2(x, y) * 16;
                    }
                }
                else
                {
                    var tpLoc = args[0];
                    if (tpLoc == "hell")
                    {
                        player.Bottom = new Vector2(Main.rand.Next(0, Main.maxTilesX), Main.maxTilesY - 150) * 16;
						Main.NewText($"Teleported to hell.");
                    }
                    else if (tpLoc == "dungeon")
                    {
                        player.Bottom = new Vector2(Main.dungeonX, Main.dungeonY) * 16;
						Main.NewText($"Teleported to the dungeon.");
                    }
                    else if (tpLoc == "spawn")
                    {
                        player.Bottom = new Vector2(Main.spawnTileX, Main.spawnTileY) * 16;
						Main.NewText($"Teleported to spawn.");
                    }
                    else if (tpLoc == "space")
                    {
                        player.Bottom = new Vector2(Main.rand.Next(0, Main.maxTilesX), 100) * 16;
						Main.NewText($"Teleported to space.");
                    }
                    else if (tpLoc == "caverns")
                    {
                        player.Bottom = new Vector2(Main.rand.Next(0, Main.maxTilesX), (float)Main.rockLayer) * 16;
						Main.NewText($"Teleported to the caverns.");
                    }
                    else if (tpLoc == "beachleft")
                    {
                        player.Bottom = new Vector2(110, player.Center.Y) * 16;
						Main.NewText($"Teleported to the left-most beach.");
                    }
                    else if (tpLoc == "beachright")
                    {
                        player.Bottom = new Vector2(Main.maxTilesX - 110, player.Center.Y) * 16;
						Main.NewText($"Teleported to the right-most beach.");
                    }
                }
            }
        }
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

    public class Hooking : ModSystem
    {
        public override void OnModLoad()
        {
            On.Terraria.Main.DrawInterface_0_InterfaceLogic1 += DrawPickRect;
        }

        private void DrawPickRect(On.Terraria.Main.orig_DrawInterface_0_InterfaceLogic1 orig)
        {
            var sb = Main.spriteBatch;
            sineWaveVal += 0.025f;
            bool eitherNeg = WorldEditPickaxe.blockBreakRect.Width < 0 || WorldEditPickaxe.blockBreakRect.Height < 0;
            var rectScreen = new Rectangle(WorldEditPickaxe.blockBreakRect.X - (int)Main.screenPosition.X, WorldEditPickaxe.blockBreakRect.Y - (int)Main.screenPosition.Y, WorldEditPickaxe.blockBreakRect.Width, WorldEditPickaxe.blockBreakRect.Height);
            sb.Draw(TextureAssets.MagicPixel.Value, WorldEditPickaxe.blockBreakRect, null, Color.White * (float)Math.Abs(Math.Sin(sineWaveVal)), 0f, Vector2.Zero, default, 0f);
            sb.Draw(TextureAssets.MagicPixel.Value, eitherNeg ? new Rectangle(-500, -500, 0, 0) : rectScreen, null, Color.White * (float)(Math.Abs(Math.Sin(sineWaveVal)) + 0.3f) * 0.5f, 0f, Vector2.Zero, default, 0f);
            string SplitCamelCase(string input)
            {
                return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            }
            if (Main.LocalPlayer.HeldItem.ModItem is WorldEditPickaxe wePick)
            {
                sb.DrawString(FontAssets.MouseText.Value, $"{SplitCamelCase(wePick.Mode.ToString())}", Main.MouseScreen + new Vector2(20), Color.White);
            }
            orig();
        }

        float sineWaveVal;
    }
}