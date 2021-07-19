using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameInput;

namespace DebugTools.Items
{
    public class GodlyRarity : ModRarity
    {
        private static Color Switch(Color firstColor, Color secondColor, float transitionTime) => Color.Lerp(firstColor, secondColor, (float)(Math.Sin(Main.GameUpdateCount / transitionTime) + 1f) / 2f);
        public override string Name => "Godly";
        public override Color RarityColor => Switch(Color.Black, Color.White, 20f);
    }
    public class WorldEditPickaxe : ModItem
    {
        public static Rectangle blockBreakRect;
        public static Vector2 lastMousePos;
        public bool wasUsingThisItem;

        public enum DestructionMode
        {
            TilesOnly,
            WallsOnly,
            TilesAndWalls,
            CollisionOnly,
            WaterOnly,
            Everything
        }
        public bool usePickPower;
        public int pickPower;

        public DestructionMode Mode { get; set; }
        private Action ActUponTiles { get; set; }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("World Edit Pickaxe");
            Tooltip.SetDefault("Cut out rectangles of the world with some SIMPLE drag and drop!\nBe sure to start with the top left and drag until the bottom right");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 3;
            Item.useAnimation = 3;
            Item.channel = true;
            Item.damage = 5;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.autoReuse = true;
            Item.rare = ModContent.RarityType<GodlyRarity>();
        }
        public override void HoldItem(Player player)
        {
            bool keyPress(Microsoft.Xna.Framework.Input.Keys key) => Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
            if (keyPress(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                if ((int)Mode < Enum.GetNames(typeof(DestructionMode)).Length - 1)
                    Mode++;
            }
            else if (keyPress(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                if ((int)Mode > 0)
                    Mode--;
            }

            if (!player.controlUseItem && wasUsingThisItem)
            {
                for (int x = blockBreakRect.X; x < blockBreakRect.X + blockBreakRect.Width; x++)
                {
                    for (int y = blockBreakRect.Y; y < blockBreakRect.Y + blockBreakRect.Height; y++)
                    {
                        switch (Mode)
                        {
                            case DestructionMode.TilesOnly:
                                ActUponTiles = delegate
                                {
                                    if (WorldGen.InWorld(x / 16, y / 16))
                                        WorldGen.KillTile(x / 16, y / 16);
                                };
                                break;
                            case DestructionMode.WallsOnly:
                                ActUponTiles = delegate
                                {
                                    if (WorldGen.InWorld(x / 16, y / 16))
                                        WorldGen.KillWall(x / 16, y / 16);
                                };
                                break;
                            case DestructionMode.TilesAndWalls:
                                ActUponTiles = delegate
                                {
                                    if (WorldGen.InWorld(x / 16, y / 16))
                                    {
                                        WorldGen.KillTile(x / 16, y / 16);
                                        WorldGen.KillWall(x / 16, y / 16);
                                    }
                                };
                                break;
                            case DestructionMode.WaterOnly:
                                ActUponTiles = delegate
                                {
                                    if (WorldGen.InWorld(x / 16, y / 16))
                                        Framing.GetTileSafely(x / 16, y / 16).LiquidAmount = 0;
                                };
                                break;
                            case DestructionMode.CollisionOnly:
                                ActUponTiles = delegate
                                {
                                    if (WorldGen.InWorld(x / 16, y / 16))
                                    {
                                        var t = Framing.GetTileSafely(x / 16, y / 16);

                                        if (t.CollisionType == 1)
                                            WorldGen.KillTile(x / 16, y / 16);
                                    }
                                };
                                break;
                            case DestructionMode.Everything:
                                ActUponTiles = delegate
                                {
                                    if (WorldGen.InWorld(x / 16, y / 16))
                                    {
                                        var t = Framing.GetTileSafely(x / 16, y / 16);
                                        t.LiquidAmount = 0;
                                        WorldGen.KillTile(x / 16, y / 16);
                                        WorldGen.KillWall(x / 16, y / 16);
                                    }
                                };
                                break;
                        }
                        ActUponTiles?.Invoke();
                    }
                    NetMessage.SendData(MessageID.TileManipulation);
                }
            }
            if (!player.controlUseItem)
            {
                lastMousePos = new Vector2(Player.tileTargetX, Player.tileTargetY) * 16;
                blockBreakRect = new();
            }
            wasUsingThisItem = player.controlUseItem;
        }
        public override bool? UseItem(Player player)
        {
            int x = (int)lastMousePos.X;
            int y = (int)lastMousePos.Y;

            int xDiff = Player.tileTargetX * 16 - x;
            int yDiff = Player.tileTargetY * 16 - y;

            blockBreakRect = new(x, y, 1, 1);

            blockBreakRect.Width = xDiff;
            blockBreakRect.Height = yDiff;

            return base.UseItem(player);
        }
    }
}