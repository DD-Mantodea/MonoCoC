using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Scripting;

namespace MonoCoC.Macros.Providers
{

    [Register<MonsterMacroProvider>("coc:monster")]
    public class MonsterMacroProvider : IMacroProvider
    {
        public bool TryResolve(string name, out ScriptValue value)
        {
            var resolved = Resolve(name);

            if (resolved != null)
            {
                value = resolved.Value;

                return true;
            }

            value = default;

            return false;
        }

        public static ScriptValue? Resolve(string name) => name.ToLowerInvariant() switch
        {
            "monster.Short" => Scene.MonsterTag(m => m.ShortName),

            "monster.a" => Scene.MonsterTag(m => m.Article),

            "themonster" => Scene.MonsterTag(m => (char.IsUpper(name[0]) ? Cap(m.Article + m.ShortName) : m.Article + m.ShortName)),

            "monster.possessive" => Scene.MonsterTag(m => m.Possessive),

            "themonster's" => Scene.MonsterTag(m => (char.IsUpper(name[0]) ? Cap(m.Article + m.ShortName) : m.Article + m.ShortName) + m.Possessive),

            "monster.he" => Scene.MonsterTag(m => m.Pronoun1),

            "monster.him" => Scene.MonsterTag(m => m.Pronoun2),

            "monster.his" => Scene.MonsterTag(m => m.Pronoun3),

            "monster.himself" => Scene.MonsterTag(m => m.Pronoun2 + "self"),

            "monster.hair" => Scene.MonsterTag(m => m.Body.Get<HairPart>().Color),

            "monster.skin" => Scene.MonsterTag(m => m.Body.Get<SkinPart>().Tone),

            "monster.Weapon" => Scene.MonsterTag(m => m.WeaponName),

            "monster.armor" => Scene.MonsterTag(m => m.ArmorName),

            "monster.s" => Scene.MonsterTag(_ => "s"),

            "monster.is" => Scene.MonsterTag(_ => "is"),

            _ => null
        };

        public static string Cap(string text) =>

            string.IsNullOrEmpty(text) ? text : char.ToUpperInvariant(text[0]) + text[1..];
    }
}