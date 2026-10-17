using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 scenes/dungeons/manor/LethiciteCrystal.hx 的招式。
    // 文本 key 来自 Tools/PortKit/out/LethiciteCrystal.texts.json（落在 Monsters/LethiciteCrystal.hjson）。
    public static class LethiciteCrystalCombatHandlers
    {
        // 原版 LethiciteCrystal.hx:26 pulse：
        //   开场一段 → Utils.rand(3) 三选一回忆（case 3 走不到）→ 收尾一段 →
        //   player.takeLustDamage(20 + rand(20), true, true); player.takeLustDamage(5 + rand(3), true, false);
        [Register("coc:lethicite_crystal_pulse")]
        private static void Pulse(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.LethiciteCrystal.Pulse.TheCrystalPulses".Localized());

            switch (context.Random.Next(3))
            {
                case 0:
                    context.Append("Combat.LethiciteCrystal.Pulse.ABeautifulGirl".Localized());
                    context.Append("Combat.LethiciteCrystal.Pulse.SheGrindsHard".Localized());
                    context.Append("Combat.LethiciteCrystal.Pulse.YouCumToo".Localized());
                    break;
                case 1:
                    context.Append("Combat.LethiciteCrystal.Pulse.YouReSurprised".Localized());
                    context.Append("Combat.LethiciteCrystal.Pulse.MinutesLaterShe".Localized());
                    context.Append("Combat.LethiciteCrystal.Pulse.YouHaveNo".Localized());
                    break;
                case 2:
                    context.Append("Combat.LethiciteCrystal.Pulse.YouSmellSomething".Localized());
                    context.Append("Combat.LethiciteCrystal.Pulse.YouEjaculateFor".Localized());
                    break;
                default:
                    // 原版 case 3（LethiciteCrystal.hx:44）：Utils.rand(3) 只给 0..2，永远走不到，保留以对齐原文。
                    context.Append("Combat.LethiciteCrystal.Pulse.YouLeaveThe".Localized());
                    context.Append("Combat.LethiciteCrystal.Pulse.SuddenlyYouNotice".Localized());
                    context.Append("Combat.LethiciteCrystal.Pulse.YouStartTurning".Localized());
                    break;
            }

            context.Append("Combat.LethiciteCrystal.Pulse.YouShakeYour".Localized());

            context.Player.TakeLustDamage(20 + context.Random.Next(20), true, true);
            context.Player.TakeLustDamage(5 + context.Random.Next(3), true, false);
        }
    }
}
