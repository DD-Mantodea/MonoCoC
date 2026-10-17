using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Saving.Legacy
{
    public class HaxeDeserializer(string data)
    {
        public static Dictionary<char, int> CODES = [];

        public string Data = data;

        public int index = 0;

        public List<string> StrCache = [];

        public List<HaxeObj?> DynCache = [];

        public char ReadChar() => Data[index++];

        public char ReadCharAt(int index) => Data[index];

        public void Skip(int length) => index += length;

        public void Back(int length) => index -= length;

        public int ReadInt()
        {
            var k = 0;

            var s = false;

            var fpos = index;

            while (true)
            {
                var c = ReadCharAt(index);

                if (c == '-')
                {
                    if (fpos != index)
                        break;

                    s = true;

                    index++;

                    continue;
                }

                if (c < '0' || c > '9')
                    break;

                k = k * 10 + c - '0';

                index++;
            }

            if (s)
                k *= -1;

            return k;
        }

        public double ReadDouble()
        {
            var p = index;

            while (true)
            {
                var c = ReadCharAt(index);

                if ((c >= 43 && c < 58) || c == 'e' || c == 'E')
                    index++;
                else
                    break;
            }

            return double.Parse(Data[p..index]);
        }

        public HaxeDynamicObj ReadEnum(HaxeDynamicObj o, HaxeObj name, HaxeObj tag)
        {
            if (ReadChar() != ':')
                throw new Exception();

            var nargs = ReadInt();

            var count = nargs;

            o["ENUMNAME"] = name;

            o["ENUMVALUE"] = tag;

            if (nargs == 0)
                return o;

            while (nargs-- > 0)

                o[count - nargs] = Deserialize();

            return o;
        }

        public void ReadObject(HaxeDynamicObj o)
        {
            while (true)
            {
                if (index >= Data.Length)
                    throw new Exception();

                if (ReadCharAt(index) == 'g')
                    break;

                var k = Deserialize();

                if (k?.Value is not string)
                    throw new Exception();

                var v = Deserialize();

                o[k.Value] = v;
            }

            index++;
        }

        public HaxeObj? Deserialize()
        {
            switch (ReadChar())
            {
                case 'n':
                    return null;

                case 't':
                    return true;

                case 'f':
                    return false;

                case 'z':
                    return 0;

                case 'i':
                    return ReadInt();

                case 'd':
                    return ReadDouble();

                case 'y':
                    var len = ReadInt();

                    if (ReadChar() != ':' || Data.Length - index < len)
                        throw new Exception();

                    var s = Data[index..(index + len)];

                    index += len;

                    s = Uri.UnescapeDataString(s);

                    StrCache.Add(s);

                    return s;

                case 'k':
                    return float.NaN;

                case 'm':
                    return float.NegativeInfinity;

                case 'p':
                    return float.PositiveInfinity;

                case 'a':
                    var buf = Data;

                    var a = new HaxeArray();

                    DynCache.Add(a);

                    while (true)
                    {
                        var c = ReadCharAt(index);

                        if (c == 'h')
                        {
                            index++;

                            break;
                        }

                        if (c == 'u')
                        {
                            index++;

                            var n = ReadInt();

                            a[a.Count + n - 1] = null;
                        }
                        else
                            a.Data.Add(Deserialize());
                    }

                    return a;

                case 'o':
                    var o = new HaxeDynamicObj();

                    DynCache.Add(o);

                    ReadObject(o);

                    return o;

                case 'r':
                    var n1 = ReadInt();

                    if (n1 < 0 || n1 > DynCache.Count)
                        throw new Exception();

                    return DynCache[n1];

                case 'R':
                    var n2 = ReadInt();

                    if (n2 < 0 || n2 > StrCache.Count)
                        throw new Exception();

                    return StrCache[n2];

                case 'x':
                    throw new Exception();

                case 'c':
                    var name = Deserialize();

                    var o2 = new HaxeClass
                    {
                        HaxeClassName = name
                    };

                    DynCache.Add(o2);

                    ReadObject(o2);

                    return o2;

                case 'w':
                    var name2 = Deserialize();

                    if (name2?.Value is not string)
                        throw new Exception();

                    var o3 = new HaxeDynamicObj();

                    var tag = Deserialize();

                    if (tag?.Value is not string)
                        throw new Exception();

                    o3 = ReadEnum(o3, name2, tag);

                    DynCache.Add(o3);

                    return o3;

                case 'j':
                    var name3 = Deserialize();

                    if (name3?.Value is not string)
                        throw new Exception();

                    index++;

                    var i = ReadInt();

                    var o4 = new HaxeDynamicObj();

                    o4["ENUMNAME"] = name3;

                    o4["ENUMVALUE"] = i;

                    DynCache.Add(o4);

                    return o4;

                case 'l':
                    var l = new HaxeArray();

                    DynCache.Add(l);

                    while (ReadCharAt(index) != 'h')

                        l.Data.Add(Deserialize());

                    index++;

                    return l;

                case 'b':
                    var h = new HaxeDynamicObj();

                    DynCache.Add(h);

                    while (ReadCharAt(index) != 'h')
                    {
                        var s2 = Deserialize();

                        h.Data.Add(s2, Deserialize());
                    }

                    index++;

                    return h;

                case 'q':
                    var h2 = new HaxeDynamicObj();

                    DynCache.Add(h2);

                    var c2 = ReadChar();

                    while (c2 == ':')
                    {
                        var i3 = ReadInt();

                        h2.Data.Add(i3, Deserialize());

                        c2 = ReadChar();
                    }

                    if (c2 != 'h')
                        throw new Exception();

                    return h2;

                case 'M':
                    var h3 = new HaxeDynamicObj();

                    DynCache.Add(h3);

                    while (ReadCharAt(index) != 'h')
                    {
                        var s2 = Deserialize();

                        h3.Data.Add(s2, Deserialize());
                    }

                    index++;

                    return h3;

                case 'v':
                    if (ReadCharAt(index) >= '0' && ReadCharAt(index) <= '9' && ReadCharAt(index + 1) >= '0' && ReadCharAt(index + 1) <= '9' && ReadCharAt(index + 2) >= '0'
                        && ReadCharAt(index + 2) <= '9' && ReadCharAt(index + 3) >= '0' && ReadCharAt(index + 3) <= '9' && ReadCharAt(index + 4) == '-')

                        index += 19;
                    else
                        ReadDouble();

                    HaxeObj o5 = new();

                    DynCache.Add(o5);

                    return o5;

                case 's':
                    var len2 = ReadInt();

                    if (ReadChar() != ':' || Data.Length - index < len2)
                        throw new Exception();

                    if (CODES.Count == 0)
                    {
                        var b64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789%:";

                        var strs = b64.Split();

                        CODES = strs.Select(s => new KeyValuePair<char, int>(s[0], b64.IndexOf(s))).ToDictionary();
                    }

                    var codes = CODES;

                    var i2 = index;

                    var rest = len2 & 3;

                    var size = (len2 >> 2) * 3 + ((rest >= 2) ? rest - 1 : 0);

                    var max = i2 + len2 - rest;

                    var bytes = new byte[size];

                    var bpos = 0;

                    while (i2 < max)
                    {
                        var c01 = codes[Data[i2++]];

                        var c02 = codes[Data[i2++]];

                        bytes[bpos++] = (byte)((c01 << 2) | (c02 >> 4));

                        var c03 = codes[Data[i2++]];

                        bytes[bpos++] = (byte)((c02 << 4) | (c03 >> 2));

                        var c04 = codes[Data[i2++]];

                        bytes[bpos++] = (byte)((c03 << 6) | c04);
                    }

                    if (rest >= 2)
                    {
                        var c01 = codes[Data[i2++]];

                        var c02 = codes[Data[i2++]];

                        bytes[bpos++] = (byte)((c01 << 2) | (c02 >> 4));

                        if (rest == 3)
                        {
                            var c03 = codes[Data[i2++]];

                            bytes[bpos++] = (byte)((c02 << 4) | (c03 >> 2));
                        }
                    }

                    index += len2;

                    DynCache.Add(bytes);

                    return bytes;

                case 'C':
                    return null;

                case 'A':
                    var name5 = Deserialize();

                    if (name5.Value is not string)
                        throw new Exception();

                    return "CLASS:" + name5;

                case 'B':
                    var name6 = Deserialize();

                    if (name6.Value is not string)
                        throw new Exception();

                    return "ENUM:" + name6;

                default:
                    break;
            }

            index--;

            throw new Exception();
        }
    }
}