using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class GoblinSharpshooterCombatHandlers
    {

        [Register("coc:goblin_sharpshooter_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (GoblinSharpshooterEnemy)context.Enemy;

            enemy.Ammo = false;

            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        [Register("coc:goblin_sharpshooter_push_back")]
        public static void PushBack(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (GoblinSharpshooterEnemy)context.Enemy;

            var player = context.Player;

            enemy.Ammo = false;

            context.Append("Scenes.Goblinsharpshooter.PushBack.LoadsRedCartridge".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false,

                DoFatigue = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
                return;

            context.Append("Scenes.Goblinsharpshooter.PushBack.MuzzleChestShot".Localized());

            context.Append("Scenes.Goblinsharpshooter.PushBack.LaunchedBackwards".Localized());

            CombatRangeData.MoveDistantSafe(enemy);

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.WeaponAttack - 50);

            player.HealthChange(-damage);
        }

        [Register("coc:goblin_sharpshooter_skeet_shot")]
        public static void SkeetShot(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (GoblinSharpshooterEnemy)context.Enemy;

            var player = context.Player;

            enemy.Ammo = false;

            context.Append("Scenes.Goblinsharpshooter.SkeetShot.ThrowsPinkFlask".Localized());

            if (enemy.ChanceToHit(player) - 30 > context.Random.Next(100))
            {
                context.Append("Scenes.Goblinsharpshooter.SkeetShot.FlaskDetonatesMidair".Localized());

                player.Teased(30);

                return;
            }

            context.Append("Scenes.Goblinsharpshooter.SkeetShot.ShotGoesWide".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false,

                DoFatigue = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
                return;

            context.Append("Scenes.Goblinsharpshooter.SkeetShot.GrenadeDetonatesEarly".Localized());

            player.Teased(20);
        }

        [Register("coc:goblin_sharpshooter_reload")]
        public static void Reload(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (GoblinSharpshooterEnemy)context.Enemy;

            enemy.Ammo = true;

            context.Append("Scenes.Goblinsharpshooter.Reload.ReloadsBlunderbuss".Localized());
        }

        [Register("coc:goblin_sharpshooter_take_aim")]
        public static void TakeAim(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Scenes.Goblinsharpshooter.TakeAim.TakesAimBlunderbuss".Localized());

            context.Enemy.CreateStatusEffect(StatusEffectIds.TargetMarked, duration: 6);
        }

        [Register("coc:goblin_sharpshooter_bugger_out")]
        public static void BuggerOut(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (GoblinSharpshooterEnemy)context.Enemy;

            var scene = SceneInstance<GoblinSharpshooterScene>.Get();

            enemy.ItsADraw = true;

            context.Append("Scenes.Goblinsharpshooter.BuggerOut.StaggersCrikey".Localized());

            if (!scene.SawSmokeGrenade)
            {
                context.Append("Scenes.Goblinsharpshooter.BuggerOut.GrabsSmokeGrenade".Localized());

                context.Append("Scenes.Goblinsharpshooter.BuggerOut.SmokeFlashBlind".Localized());

                context.EndOfRoundOverride = () => scene.GoblinEscapes();
            }
            else
            {
                context.Append("Scenes.Goblinsharpshooter.BuggerOut.PreparedSmoke".Localized());

                context.EndOfRoundOverride = () => enemy.TryToGrabGobbo(scene);
            }
        }
    }
}