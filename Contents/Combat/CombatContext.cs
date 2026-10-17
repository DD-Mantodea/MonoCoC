using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Combat
{
    public class CombatContext
    {
        public Player Player = null!;

        public bool EnemyFirst;

        public Enemy _enemy = null!;

        public Enemy Enemy
        {
            get => _enemy;

            set
            {
                _enemy = value;

                if (value == null)
                    return;

                if (Enemies.Count == 0)
                {
                    Enemies.Add(value);

                    CurrTarget = 0;

                    return;
                }

                if (!Enemies.Contains(value))
                    Enemies[Math.Clamp(CurrTarget, 0, Enemies.Count - 1)] = value;
            }
        }

        public int CurrTarget;

        public bool EndCombatRequested;

        public virtual void ResetFor(params Enemy[] reinstate)
        {
            Enemies.Clear();

            CurrTarget = 0;

            EndCombatRequested = false;

            PendingNameRestore = null;

            foreach (var enemy in reinstate)
                AddEnemy(enemy);
        }

        public string? PendingNameRestore;

        public List<Enemy> Enemies = [];

        public Identifier MonsterDamageType;

        public virtual void DealDamageToPlayer(float damage)
        {
            Player.HealthChange(-damage);

            var type = MonsterDamageType;

            if (type.Path == null)
                return;

            var definition = Assets.ManagerInstance<Assets.ContentManagers.DamageTypeManager>.Instance[type.ToString()];

            if (definition != null && definition.Handler.Path != null)
                DamageTypeHandlerRegistry.Invoke(definition.Handler, this, damage);
        }

        public virtual void AddEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);

            if (Enemy == null!)
                Enemy = enemy;
        }

        public IEnumerable<Enemy> LivingEnemies => Enemies.Where(e => e.Health > 0);

        public bool AllEnemiesDefeated => Enemies.Count > 0 && Enemies.All(e => e.Health <= 0);

        public virtual bool CanTarget(int index) =>

            index >= 0 && index < Enemies.Count

            && Enemies[index].Health > 0

            && (Enemies[index].Lust < Enemies[index].MaxLust);

        public List<Enemy> ActiveEnemies =>

            Enemies.Where(e => e.Health > 0 && e.Lust < e.MaxLust).ToList();

        public virtual int LowestLivingTarget()
        {
            for (var i = 0; i < Enemies.Count; i++)
                if (CanTarget(i))
                    return i;

            return Enemies.Count;
        }

        public virtual void SetTarget(int index)
        {
            if (Enemies.Count > 1 && CanTarget(index))
            {
                CurrTarget = index;

                Enemy = Enemies[index];
            }
        }

        public virtual T? EnemyAs<T>() where T : Enemy => Enemy as T;

        public Random Random = Random.Shared;

        public bool PlayerWaited;

        /// <summary>
        /// 原版 Combat.damage 的等价物：敌人可在 WhenAttacked 钩子里放大本次受到的伤害
        /// （例：SandTrap.hx:100 陷在沙坑里时伤害 ×1.75）。结算后必须重置为 1。
        /// </summary>
        public float EnemyDamageTakenMultiplier = 1f;

        public Action<string> Write = _ => { };

        public Action<string> Append = _ => { };

    /// <summary>原版 startCombatMultiple(..., desc) 的一次性战斗介绍文本。</summary>
    public string IntroDescription = "";

        public Action<string> WriteAction = _ => { };

        public List<string> Log = [];

        public Action<int, string, Action> AddButton = (_, _, _) => { };

        public Action<int, string, string> AddButtonDisabled = (_, _, _) => { };

        public Action Clear = () => { };

        public Action ClearButtons = () => { };

        public Action ReturnToCamp = () => { };

        public bool CanFlee = true;

        public Action? EndOfRoundOverride;
    }

    public class CombatState
    {
        public int Round;

        /// <summary>原版 CombatAbilities.hx:67 fireMagicLastTurn —— 上次施放火焰法术的回合（初值 -100）。</summary>
        public int FireMagicLastTurn = -100;

        /// <summary>原版 CombatAbilities.hx:68 fireMagicCumulated —— 连续施放火焰法术的累计次数。</summary>
        public int FireMagicCumulated;

        /// <summary>原版 CombatAbilities.hx:5652 flurryAmount —— 本场战斗 Endless Flurry 的连击数（初值 1，上限 5）。</summary>
        public int FlurryAmount = 1;

        public bool PlayerTurn = true;

        public CombatOutcome? Outcome;

        public readonly Dictionary<Identifier, int> Cooldowns = [];

        public virtual bool IsOnCooldown(Identifier ability) =>

            Cooldowns.TryGetValue(ability, out var remaining) && remaining > 0;

        public virtual int CooldownOf(Identifier ability) =>

            Cooldowns.TryGetValue(ability, out var remaining) ? remaining : 0;

        public virtual void SetCooldown(Identifier ability, int rounds) =>

            Cooldowns[ability] = Math.Max(0, rounds);

        public virtual void TickCooldowns()
        {
            foreach (var id in Cooldowns.Keys.ToList())
            {
                var remaining = Cooldowns[id] - 1;

                if (remaining <= 0)
                    Cooldowns.Remove(id);
                else
                    Cooldowns[id] = remaining;
            }
        }
    }

    public enum CombatOutcome
    {
        HpVictory,

        LustVictory,

        HpDefeat,

        LustDefeat,

        Fled
    }
}