using System;

namespace MonoCoC.Attributes
{
    // 统一的注册标记。一个类只会属于一个注册表（敌人 / 场景 / 招式 ……），
    // 所以不需要 RegisterEnemy / RegisterScene 这种按种类分的 Attribute：
    // 「谁认领」由类型继承关系决定，Attribute 只负责 id（和 mod 覆盖用的 Replace）。
    //
    // 用法约定（已统一）：
    //   类上   → 一律 [Register<T>("coc:xxx")]
    //   方法上 → 一律 [Register("coc:xxx")]，标记所在的方法就是 handler
    //
    // 例外：极少数类需要带参构造（11 个 AreaScene 与 AliceEnemy / NumbingEffect），
    // 无法满足泛型版要求的 new()，因此基类仍允许打在类上，仅供这些类使用；
    // 是否有例外由 Tools/PortKit/audit-register.mjs 报告。
    //
    // 故意不 sealed：以后要派生便利子类（比如 ReplaceEnemyAttribute）还能继承。
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RegisterAttribute(string id) : Attribute
    {
        // 注册表里的 id（敌人就是 Enemies.hjson 的 type 字段）
        public string Id { get; } = id;

        // true 表示允许覆盖同 id 的已有注册；false 时重复 id 会直接抛异常
        public bool Replace { get; set; }

        // 工厂类型；非泛型版为 null，表示「标记所在的那个类」
        public virtual Type? Factory => null;
    }

    // 泛型版：
    //   new()  —— 「工厂类必须有 public 无参构造」在编译期就有保证。
    // 注：原先还要求 T : IIdentifiable，但注册表一律按 attribute.Id 建索引、
    // 用无参构造实例化，从不读取实例的 Id 属性；而宏 provider、跟随者定义等
    // 合法的注册目标并不实现 IIdentifiable，该约束会把它们挡在泛型写法之外，
    // 导致「类上一律用 Register<T>」无法统一。故这里只保留 new()。
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterAttribute<T> : RegisterAttribute where T : new()
    {
        public RegisterAttribute(string id) : base(id) { }

        public override Type Factory => typeof(T);
    }
}
