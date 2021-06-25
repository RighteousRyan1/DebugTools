using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria.GameContent;
using ReLogic.Content;

namespace DebugTools
{
    public class LePlayer : ModPlayer
    {
        public static bool extraProjectileInformation;
        public static bool iFrameDisabled;
        public static bool playerInfo;
        public int lastTakenDamage;
        public string recName = "N/A";
        public override void PreUpdate()
        {
            if (iFrameDisabled)
            {
                Player.immuneTime = 0;
            }
        }
    }
    public class HitNPC : GlobalNPC
    {
        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            target.GetModPlayer<LePlayer>().lastTakenDamage = damage;
            target.GetModPlayer<LePlayer>().recName = npc.FullName + " (NPC)";
        }
    }
    public class DebugGlobalProj : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            Player player = Main.player[Main.myPlayer];
            if (projectile.type == ProjectileID.UFOLaser)
            {
                projectile.penetrate = -1;
            }
            if (projectile.type == ProjectileID.MagnetSphereBall)
            {
                player.ownedProjectileCounts[ProjectileID.MagnetSphereBall] = 0;
            }
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            target.GetModPlayer<LePlayer>().lastTakenDamage = damage;
            target.GetModPlayer<LePlayer>().recName = projectile.Name + " (Projectile)";
        }
        public override void ModifyHitPvp(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            target.GetModPlayer<LePlayer>().lastTakenDamage = damage;
            target.GetModPlayer<LePlayer>().recName = projectile.Name + " (Projectile)";
        }
        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            bool leftAlt = Main.keyState.IsKeyDown(Keys.LeftAlt);
            float distance = projectile.Distance(Main.player[projectile.owner].Center);
            if (LePlayer.extraProjectileInformation)
            {
                string info = !leftAlt ? $"Name: {projectile.Name}\nAI: (ai[0]: {projectile.ai[0]}) (ai[1]: {projectile.ai[1]})\nLocalAI: (localAI[0] - {projectile.ai[0]}) (localAI[1] - {projectile.ai[1]})"
                + $"\nVelocity: (X: {projectile.velocity.X:0.##} | Y: {projectile.velocity.Y:0.##}\nPosition: (X: {projectile.position.X:0.##} | Y: {projectile.position.Y:0.##})" :

                $"Name: {projectile.Name}\nAI: (ai[0]: {projectile.ai[0]}) (ai[1]: {projectile.ai[1]})\nLocalAI: (localAI[0] - {projectile.ai[0]}) (localAI[1] - {projectile.ai[1]})"
                + $"\nVelocity: (X: {projectile.velocity.X:0.##} | Y: {projectile.velocity.Y:0.##}\nPosition: (X: {projectile.position.X:0.##} | Y: {projectile.position.Y:0.##})"
                + $"\nExtra Info:\n"
                + $"Distance from player: {distance}";

                float scale = 0.2f * Main.UIScale;
                Vector2 measurement = FontAssets.DeathText.Value.MeasureString(info) * scale;
                Main.spriteBatch.Draw((Texture2D)Mod.GetTexture("Assets/WhitePixel"), projectile.Center + new Vector2(10, 15) - Main.screenPosition, null, new Color(0, 165, 255) * 0.4f, 0f, Vector2.Zero, measurement + new Vector2(10, 5), SpriteEffects.None, 1f);
                Main.spriteBatch.DrawString(FontAssets.DeathText.Value, info, projectile.Center + new Vector2(20, 20) - Main.screenPosition, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
                Main.spriteBatch.DrawString(FontAssets.DeathText.Value, "Hold down LeftAlt for extra information.", projectile.Center + new Vector2(20, 0) - Main.screenPosition, Color.Green, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
            }
        }
    }
    public class DrawInfoWorld : ModSystem
    {
        string NA;
        public static bool hasFrames;
        public override void PostDrawInterface(SpriteBatch spriteBatch  )
        {
            bool leftAlt = Main.keyState.IsKeyDown(Keys.LeftAlt);
            Player player = Main.player[Main.myPlayer];

            Vector2 itemTexDrawPos = new Vector2(player.Center.X + 14, player.Center.Y + 235);

            Item item = player.HeldItem;

            Rectangle? itemTextureRect = null;

            Texture2D itemTex = (Texture2D)TextureAssets.Item[player.HeldItem.type];

            if (Main.itemAnimations[item.type] != null)
            {
                hasFrames = true;
                itemTextureRect = Main.itemAnimations[item.type].GetFrame(itemTex);
            }
            else
            {
                hasFrames = false;
            }

            NA = player.HeldItem.type > ItemID.None ? player.HeldItem.Name : "None";
            string info = !leftAlt ? $"Name: {player.name}\nHead Frame: {player.headFrame.Y / 56}\nBody Frame: {player.bodyFrame.Y / 56}\nLeg Frame: {player.legFrame.Y / 56}"
                + $"\nVelocity: (X: {player.velocity.X:0.##} | Y: {player.velocity.Y:0.##})\nPosition: (X: {player.position.X:0.##} | Y: {player.position.Y:0.##})" :

                $"Name: {player.name}\nHead Frame: {player.headFrame.Y / 56}\nBody Frame: {player.bodyFrame.Y / 56}\nLeg Frame: {player.legFrame.Y / 56}"
                + $"\nVelocity: (X: {player.velocity.X:0.##} | Y: {player.velocity.Y:0.##})\nPosition (Tile Coordinates): (X: {System.Math.Round(player.position.X / 16)} | Y: {System.Math.Round(player.position.Y / 16)})"
                + $"\nExtra Info:\n"
                + $"Last Taken Damage: {player.GetModPlayer<LePlayer>().lastTakenDamage}\nLast Taken Damage Cause: {player.GetModPlayer<LePlayer>().recName} (True Damage)"
                + $"\nUseItem Data: (Item Animation: {player.itemAnimation})"
                + $"\nHeld Item Data: (Name: {NA} | NetID: {player.HeldItem.netID} | useAnimation: {item.useAnimation} | useTime: {item.useTime}"
                + $"\nHeld Item Texture: ";
            if (LePlayer.playerInfo)
            {
                float scale = 0.2f * Main.UIScale;
                Vector2 measurement = FontAssets.DeathText.Value.MeasureString(info) * scale;
                Main.spriteBatch.Draw((Texture2D)Mod.GetTexture("Assets/WhitePixel"), player.Center + new Vector2(-80, 45) - Main.screenPosition, null, player.hairColor * 0.4f, 0f, Vector2.Zero, measurement + new Vector2(10, 5), SpriteEffects.None, 1f);
                Main.spriteBatch.DrawString(FontAssets.DeathText.Value, info, player.Center + new Vector2(-75, 50) - Main.screenPosition, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
                Main.spriteBatch.DrawString(FontAssets.DeathText.Value, "Hold down LeftAlt for extra information.", player.Center + new Vector2(-80, 35) - Main.screenPosition, Color.Chartreuse, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
            }
            if (leftAlt && LePlayer.playerInfo)
            {
                Main.spriteBatch.Draw(itemTex, itemTexDrawPos - Main.screenPosition, itemTextureRect, Color.White, 0f, !hasFrames ? new Vector2(0, itemTex.Height / 2) : new Vector2(0, 15), 0.35f, SpriteEffects.None, 1f);
            }
        }
    }
}
