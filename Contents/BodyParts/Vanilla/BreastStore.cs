using MonoCoC.Contents.Flags;
using System;
using System.Globalization;

namespace MonoCoC.Contents.BodyParts.Vanilla
{

    /// <summary>
    /// 原版 classes/BreastStore.hx —— NPC 专属的乳房状态存储。
    /// 数据以 "<![CDATA[1^rows^cupSize^lactationLevel^nippleLength^fullness^timesMilked^preventInc^preventDec]]>"
    /// 的形式保存在一个 Flag&lt;string&gt; 里，格式与原版一致，可直接读取原版存档。
    /// </summary>
    public class BreastStore
    {
        public const int LactationDisabled = 0;

        public const int LactationNone = 1;

        public const int LactationLight = 2;

        public const int LactationModerate = 3;

        public const int LactationStrong = 4;

        public const int LactationHeavy = 5;

        public const int LactationEpic = 6;

        /// <summary>原版 BreastStore.hx:10 LACTATION_BOOST —— 泌乳等级越高回充越快。</summary>
        public static readonly int[] LactationBoost = [0, 0, 2, 3, 6, 9, 17];

        private const string StoreVersion = "1";

        private readonly Flag<string> _breastFlag;

        private int _rows = 2;

        private int _cupSize = 0;

        private int _lactation = LactationNone;

        private float _nippleLength = 0.25f;

        private int _fullness;

        private int _timesMilked;

        /// <summary>原版 BreastStore.hx:29 —— 泌乳增长的上限转折点。</summary>
        public int PreventLactationIncrease;

        /// <summary>原版 BreastStore.hx:30 —— 泌乳下降的下限转折点。</summary>
        public int PreventLactationDecrease;

        public BreastStore(Flag<string> breastFlag)
        {
            _breastFlag = breastFlag;

            Load();
        }

        /// <summary>原版 BreastStore.hx:38 updateAfterLoad —— 从 flag 字符串解析。</summary>
        public virtual void Load()
        {
            var raw = CoC.Instance.Flags.Get(_breastFlag);

            if (string.IsNullOrEmpty(raw))
                return;

            var parts = raw.Split('^');

            if (parts.Length < 9)
                return;

            Rows = ParseInt(parts[1], 2);

            CupSize = ParseInt(parts[2], 0);

            LactationLevel = ParseInt(parts[3], LactationNone);

            _nippleLength = float.TryParse(parts[4], NumberStyles.Float, CultureInfo.InvariantCulture, out var nl) ? nl : 0.25f;

            _fullness = ParseInt(parts[5], 0);

            _timesMilked = ParseInt(parts[6], 0);

            PreventLactationIncrease = ParseInt(parts[7], 0);

            PreventLactationDecrease = ParseInt(parts[8], 0);
        }

        /// <summary>原版 BreastStore.hx:56 updateBeforeSave —— 写回 flag 字符串。</summary>
        public virtual void Save()
        {
            CoC.Instance.Flags.Set(
                _breastFlag,

                string.Join(
                    '^',

                    StoreVersion,

                    Rows,

                    CupSize,

                    LactationLevel,

                    _nippleLength.ToString(CultureInfo.InvariantCulture),

                    _fullness,

                    _timesMilked,

                    PreventLactationIncrease,

                    PreventLactationDecrease));
        }

        /// <summary>原版 BreastStore.hx:332 rows。</summary>
        public virtual int Rows
        {
            get => _rows;

            set { _rows = Math.Clamp(value, 1, 10); Save(); }
        }

        /// <summary>原版 BreastStore.hx:109 cupSize。</summary>
        public virtual int CupSize
        {
            get => _cupSize;

            set { _cupSize = Math.Clamp(value, 0, 100); Save(); }
        }

        /// <summary>原版 BreastStore.hx:124 lactationLevel。</summary>
        public virtual int LactationLevel
        {
            get => _lactation;

            set { _lactation = Math.Clamp(value, LactationDisabled, LactationEpic); Save(); }
        }

        /// <summary>原版 BreastStore.hx:319 nippleLength。</summary>
        public virtual float NippleLength
        {
            get => _nippleLength;

            set { _nippleLength = Math.Clamp(value, 0f, 4f); Save(); }
        }

        // ---- 便捷别名：与 C# 侧 KatherineScene 既有调用名保持一致 ----

        /// <summary>等价于原版 lactationLevel。</summary>
        public virtual int Lactation
        {
            get => LactationLevel;

            set => LactationLevel = value;
        }

        /// <summary>等价于原版 cupSize。</summary>
        public virtual int BreastRating
        {
            get => CupSize;

            set => CupSize = value;
        }

        /// <summary>等价于原版 _fullness。</summary>
        public virtual int MilkFullness
        {
            get => _fullness;

            set { _fullness = Math.Max(0, value); Save(); }
        }

        /// <summary>原版 BreastStore.hx:142 advanceTime —— 每小时回充。</summary>
        public virtual void AdvanceTime()
        {
            if (_lactation <= LactationNone)
                return;

            _fullness += LactationBoost[_lactation];

            if (_fullness > 60 + 20 * LactationBoost[_lactation])
            {
                _fullness = 50;

                if (_timesMilked >= 5)
                    _timesMilked -= 5;
                else if (PreventLactationDecrease != _lactation)
                    _lactation--;
            }

            Save();
        }

        /// <summary>原版 BreastStore.hx:244 milked —— 返回是否因此提升了泌乳等级。</summary>
        public virtual bool Milked()
        {
            _fullness = 0;

            _timesMilked++;

            if (PreventLactationIncrease == _lactation)
            {
                Save();

                return false;
            }

            var threshold = _lactation switch
            {
                LactationNone => 12,

                LactationLight => 10,

                LactationModerate => 12,

                LactationHeavy => 15,

                LactationStrong => 20,

                _ => -1,
            };

            if (threshold < 0 || _timesMilked < threshold)
            {
                Save();

                return false;
            }

            _timesMilked = 5;

            _lactation++;

            Save();

            return true;
        }

        /// <summary>原版 BreastStore.hx:240 lactating。</summary>
        public virtual bool Lactating() => _lactation > LactationLight;

        /// <summary>原版 BreastStore.hx:285 milkIsFull。</summary>
        public virtual bool MilkIsFull() => _lactation > LactationNone && _fullness >= 50;

        /// <summary>原版 BreastStore.hx:289 milkIsOverflowing。</summary>
        public virtual bool MilkIsOverflowing() => _lactation > LactationNone && _fullness >= 60 + 5 * LactationBoost[_lactation];

        /// <summary>原版 BreastStore.hx:295 milkQuantity。</summary>
        public virtual float MilkQuantity() =>

            _lactation <= LactationNone

                ? 0

                : 0.01f * Math.Max(100, 2 * _fullness) * 20 * _rows * _cupSize * (_lactation - 1);

        /// <summary>原版 BreastStore.hx:236 hasBreasts。</summary>
        public virtual bool HasBreasts() => _cupSize > 0;

        /// <summary>原版 BreastStore.hx:217 canTitFuck。</summary>
        public virtual bool CanTitFuck() => _cupSize >= 2;

        /// <summary>原版 BreastStore.hx:221 cup。</summary>
        public virtual string Cup() => Creatures.Appearance.BreastCup(CupSize);

        /// <summary>原版 BreastStore.hx:158 adj。</summary>
        public virtual string Adj()
        {
            return CupSize switch
            {
                0 => "non-existent",

                >= 1 and <= 1 => "small",

                >= 2 and <= 3 => "palmable",

                >= 4 and <= 6 => "sizeable",

                >= 7 and <= 9 => "substantial",

                >= 10 and <= 12 => "large",

                >= 13 and <= 15 => "massive",

                >= 16 and <= 19 => "gigantic",

                _ => "titanic",
            };
        }

        private static int ParseInt(string text, int fallback) =>

            int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : fallback;
    }
}