using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Monsters
{
    // StareMonster.hx:10 class StareMonster extends Monster，:11 static function speedReduce(player, amount)
      // 原版 StareMonster.hx 全文仅 16 行：一个空壳基类 + speedReduce() 静态方法，此处已完整对应。
    public static class StareMonster
    {
        // 原版 StareMonster.hx:11 speedReduce()：叠加层数并把减速效果挂到 CombatBuff 上
        public static void SpeedReduce(Player player, float amount = 0)
        {
            var bse = player.CreateOrFindStatusEffect(StatusEffectIds.BasiliskSlow);

            var stacks = bse.Values[0] + (amount <= 0 ? 1 : amount);

            bse.Values[0] = stacks;

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, -stacks, "BasiliskSlow", StatusEffectIds.BasiliskSlow);
        }
    }
}