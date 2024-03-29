using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace sritext
{
    public delegate string dele_converter(string s);

    public enum Method
    {
        Sri_Text,
        SinGlish,
        kaputadotcom,
        unicode,
        SriShell_Guess
    }
    public partial class Converter
    {
        public static Dictionary<Method, char[]> alpha = new Dictionary<Method, char[]>();
        public static Dictionary<Method, dele_converter> readers = new Dictionary<Method, dele_converter>();
        public static Dictionary<Method, dele_converter> getters = new Dictionary<Method, dele_converter>();
    

        static Converter()
        {
            alpha[Method.SriShell_Guess] = new char[] {
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','/','-','+','\''};
            alpha[Method.Sri_Text] = new char[] {
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','/','-','+','\''};
            alpha[Method.SinGlish] = new char[] { 
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '\\',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
            alpha[Method.kaputadotcom] = new char[] { 
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '\\',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                '`','~','@','#','$','^','&','_','{','}','[',']',';',':','|'
            };
            alpha[Method.unicode] = new char[] { };
            readers[Method.SriShell_Guess] = new dele_converter(readSriShell_Guess);
            readers[Method.Sri_Text] = new dele_converter(readSritext);
            readers[Method.SinGlish] = new dele_converter(readSinglish);
            readers[Method.kaputadotcom] = new dele_converter(readKaputa);
            readers[Method.unicode] = new dele_converter(readSinhalaUnicode);
            getters[Method.Sri_Text] = new dele_converter(getSritext);
            //getters[Method.SinGlish     ] = new dele_converter(getSinglish);
            getters[Method.kaputadotcom ] = new dele_converter(getKaputa);
            getters[Method.unicode      ] = new dele_converter(getSinhalaUnicode);
            
            
        }
        //alpha.Add();
        static Queue KnowenConversions = new  Queue();
        public static string readSriShell_Guess(string s)
        {
            while (KnowenConversions.Count > 10000)
            {
                KnowenConversions.Dequeue();
                System.Windows.Forms.MessageBox.Show(KnowenConversions.Count.ToString());
            }
            string r="";
            while (s != "")
            {
                Match m = Regex.Match(s, @"^([^a-z/+\-]*)([a-z/+\-]*[0-9]?)(.*)",RegexOptions.Singleline);
                r += m.Groups[1].ToString();
                string c=m.Groups[2].ToString();
                bool found=false;
                foreach(KeyValuePair<string,string> keyval in KnowenConversions)
                {
                    if(keyval.Key==c)
                    {
                        r += keyval.Value;
                        found=true;
                    }
                }
                if(!found)
                {
                    KeyValuePair<string, string> keyval=new KeyValuePair<string,string>("","");
                    keyval = new KeyValuePair<string, string>(c, trie.TRIE.findAll(c,false)[0].ToString().Replace("|", ""));
                    if(c.Length>0)
                        KnowenConversions.Enqueue(keyval);
                    r+=keyval.Value;
                }
                s = m.Groups[3].ToString();
            }
            return r;
        }
        public static string readSinglish(string r)
        {
            //r = (new Regex(@"(.&?Y?)(@+)")).Replace(r, "$2$1");
            r = (new Regex(@"ie|ee")).Replace(r, "ii");
            r = r.Replace("ae", "aee");
            r = r.Replace("\\a", "ae");
            r = r.Replace("ei", "ee");
            r = r.Replace("oo", "uu");
            r = r.Replace("oe", "oo");
            r = r.Replace("A", "ae");
            r = (new Regex(@"(.)\)")).Replace(r, "$1$1");
            r = (new Regex(@"([ae])a")).Replace(r, "$1$1");
            r = r.Replace("ai", "ayi");
            r = r.Replace("I", "ai");

            r = r.Replace("GN", "cx");
            r = r.Replace("KN", "/c");
            r = r.Replace("th", "tx");
            r = r.Replace("dh", "dx");
            r = r.Replace("ch", "c");
            r = r.Replace("N", "nx");
            r = r.Replace("L", "lx");
            r = r.Replace("Th", "txh");
            r = r.Replace("Dh", "dxh");
            r = r.Replace("K", "kh");
            r = r.Replace("G", "gh");
            r = r.Replace("T", "th");
            r = r.Replace("D", "dh");
            r = r.Replace("P", "ph");
            r = r.Replace("Ch", "ch");
            r = r.Replace("B", "/b");
            r = r.Replace("sh", "sx");
            r = r.Replace("Sh", "sh");
            r = r.Replace("q", "jh");
            r = r.Replace("\\n", "/n");
            r = r.Replace("\\h", "hx");
            r = r.Replace("\\N", "/k");
            r = r.Replace("\\R", "rx");

            r = (new Regex(@"R|\\r")).Replace(r, "r");
            r = (new Regex(@"Y|\\y")).Replace(r, "y");

            r = (new Regex(@"([^aeiouA])ruu")).Replace(r, "$1rr");
            r = (new Regex(@"([^aeiouA])ru")).Replace(r, "$1r");

            r = (new Regex(@"nn([gd])")).Replace(r, "/$1");

            //r = r.Replace("", "");
            //r = r.Replace("(", "");

            //r = r.Replace("", "/$1");

            return r;
        }
        public static string getKaputa(string r)
        {
            r = r.Replace( "aa", "a`");
            r = r.Replace( "sx", "z");
            r = r.Replace("ooo", "/ au");
            r = r.Replace("cx", "Z~");
            r = r.Replace("hx", ":");

            r = r.Replace("/k", "V|");
            r = r.Replace("kh", "K|");
            r = r.Replace("k", "k~");
            r = r.Replace("gh", "G~");
            r = r.Replace("/g", "M~");
            r = r.Replace("g", "g~");


            r = r.Replace("z", "X~");
            r = r.Replace("/c", "z~");
            r = r.Replace("ch", "C~");
            r = r.Replace("c", "c|");


            r = r.Replace("jh", "J|");
            r = r.Replace("/j", "j~");
            r = r.Replace("j", "j~");


            r = r.Replace("w", "v");

            r = r.Replace("txh", "}~");
            r = r.Replace("w", "v|");
            r = r.Replace("tx", "w~");
            r = r.Replace("dxh", "{|");
            r = r.Replace("/dx", "[~");
            r = r.Replace("dx", "q~");
            r = r.Replace("/n", "A");
            r = r.Replace("nx", "N~");
            r = r.Replace("n", "n~");

            r = r.Replace("th", "T~");
            r = r.Replace("t", "t|");
            r = r.Replace("dh", "D~");
            r = r.Replace("/d", "V|");
            r = r.Replace("d", "d|");

            r = r.Replace("ph", "P~");
            r = r.Replace("p", "p~");

            r = r.Replace("rxx", "G^^");
            r = r.Replace("rx", "G^");
            r = r.Replace("r", "r\\");

            r = r.Replace("bh", "x~");
            r = r.Replace("/b", "B|");
            r = r.Replace("b", "b|");
            r = r.Replace("m", "m|");

            r = r.Replace("y", "y~");

            r = r.Replace("lx", "L~");
            r = r.Replace("l", "l~");
            r = r.Replace("v", "v|");
            r = r.Replace("sh", ";~");
            r = r.Replace("s", "s~");
            r = r.Replace("h", "h~");

            r = r.Replace("~r\\", "Y~");
            r = r.Replace("|r\\", "Y|");
            r = r.Replace("~y~", "&~");
            r = r.Replace("|y~", "&|");
            r = r.Replace("f", "f~");

            r = r.Replace("r\\aee", "rH");
            r = r.Replace("r\\ae", "rF");
            r = r.Replace("~aee", "$");
            r = r.Replace("\\aee", "$");
            r = r.Replace("|aee", "$");
            r = r.Replace("aee", "a$");
            r = r.Replace("~ae", "#");
            r = r.Replace("\\ae", "#");
            r = r.Replace("|ae", "#");
            r = r.Replace("ae", "a#");
            r = r.Replace("~aa", "`");
            r = r.Replace("\\aa", "`");
            r = r.Replace("|aa", "`");
            r = r.Replace("aa", "a`");
            r = r.Replace("~ai", "@@");
            r = r.Replace("\\ai", "@@");
            r = r.Replace("|ai", "@@");
            r = r.Replace("~au", "@_");
            r = r.Replace("\\au", "@_");
            r = r.Replace("|au", "@_");
            r = r.Replace("au", "o_");
            r = r.Replace("~a", "");
            r = r.Replace("\\a", "");
            r = r.Replace("|a", "");

            r = r.Replace("~ii", "W");
            r = r.Replace("\\ii", "W");
            r = r.Replace("|ii", "W");
            r = r.Replace("ii", "I");
            r = r.Replace("~i", "Q");
            r = r.Replace("\\i", "Q");
            r = r.Replace("|i", "Q");

            r = r.Replace("k~uu", "kS");
            r = r.Replace("k~u", "kO");
            r = r.Replace("g~uu", "gS");
            r = r.Replace("g~u", "gO");
            r = r.Replace("M~uu", "MS");
            r = r.Replace("M~u", "MO");
            r = r.Replace("w~uu", "wS");
            r = r.Replace("w~u", "wO");
            r = r.Replace("X~uu", "XS");
            r = r.Replace("X~u", "XO");
            r = r.Replace("x~uu", "xS");
            r = r.Replace("x~u", "xO");
            r = r.Replace("L~uu", "U$");
            r = r.Replace("L~u", "U");
            r = r.Replace("Y~uu", "Y$");
            r = r.Replace("Y~u", "Y#");
            r = r.Replace("Y|uu", "Y$");
            r = r.Replace("Y|u", "Y#");
            r = r.Replace("r\\uu", "r$");
            r = r.Replace("r\\u", "r#");
            r = r.Replace("~uu", "R");
            r = r.Replace("\\uu", "R");
            r = r.Replace("|uu", "R");
            r = r.Replace("uu", "u_");
            r = r.Replace("~u", "E");
            r = r.Replace("\\u", "E");
            r = r.Replace("|u", "E");

            r = r.Replace("~ee", "@~");
            r = r.Replace("\\ee", "@\\");
            r = r.Replace("|ee", "@|");
            r = r.Replace("ee", "e~");
            r = r.Replace("~e", "@");
            r = r.Replace("\\e", "@");
            r = r.Replace("|e", "@");

            r = r.Replace("~oo", "@`~");
            r = r.Replace("~o", "@`");
            r = r.Replace("\\oo", "@`~");
            r = r.Replace("\\o", "@`");
            r = r.Replace("|oo", "@`~");
            r = r.Replace("|o", "@`");
            r = r.Replace("oo", "o|");

            r = r.Replace("Y~", "^");
            r = r.Replace("Y|", "^");
            r = r.Replace("^r\\", "^^");

            r = r.Replace("&~", "~y~");
            r = r.Replace("&|", "|y~");

            Regex rgx = new Regex(@"(.&?Y?)(@+)");
            r = rgx.Replace(r, "$2$1");

            r = r.Replace("ai", "@e");
            r = r.Replace("/ ", "");
            r = r.Replace("/-", "");
            r = r.Replace("k~/+;", "]");
            r = r.Replace("/+", "");
            return r;
        }
        public static string readKaputa(string r)
        {
            r = r.Replace("WY", "YW");
            r = new Regex(@"\\|\|").Replace(r, "~");
            r = r.Replace("]", "k~;");

            r = new Regex(@"(@@?)(.&?Y?)").Replace(r, "$2$1");
            r = r.Replace("U$", "Uu");
            r = r.Replace("r#", "rE");
            r = r.Replace("r$", "rR");
            r = new Regex(@"([[bcdfghjklmnpqrstvwxyzBCDGJKLMNPTVXYZ;{}])").Replace(r, "$1a");
            r = r.Replace("&", "~ya");
            r = r.Replace("Y", "~r");
            r = new Regex("#|F").Replace(r, "~ae");
            r = new Regex(@"\$|H").Replace(r, "~aee");
            r = r.Replace("@@", "~ai");
            r = r.Replace("@~", "~ee");
            r = r.Replace("@`~", "~oo");
            r = r.Replace("@`", "~o");
            r = r.Replace("@_", "~au");
            r = r.Replace("@", "~e");
            r = r.Replace("Q", "~i");
            r = r.Replace("W", "~ii");
            r = new Regex("[EO]").Replace(r, "~u");
            r = new Regex("[RS]").Replace(r, "~uu");
            r = r.Replace("`", "a");
            r = r.Replace("^^", "~rr");
            r = r.Replace("^", "~r");
            r = new Regex(@"a[~|]").Replace(r, "");

            r = r.Replace("x", "bh");
            r = r.Replace("w", "tx");
            r = r.Replace("q", "dx");
            r = r.Replace("e~", "ee");
            r = r.Replace("o~", "oo");
            r = r.Replace("o_", "au");
            r = r.Replace("u_", "uu");
            r = r.Replace("A", "/n");
            r = r.Replace("B", "/b");
            r = r.Replace("C", "ch");
            r = r.Replace("D", "dh");
            r = r.Replace("G", "gh");
            r = r.Replace("I", "ii");
            r = r.Replace("J", "jh");
            r = r.Replace("K", "kh");
            r = r.Replace("L", "lx");
            r = r.Replace("M", "/g");
            r = r.Replace("N", "nx");
            r = r.Replace("P", "ph");
            r = r.Replace("T", "th");
            r = r.Replace("U", "lxu");
            r = r.Replace("V", "/d");
            r = r.Replace("X", "sx");
            r = r.Replace("Z", "cx");
            r = r.Replace("{", "dxh");
            r = r.Replace("}", "txh");
            r = r.Replace("[", "/dx");
            r = r.Replace(";", "sh");

            r = new Regex("[sG]r~r").Replace(r, "rxx");
            r = new Regex("[sG]r").Replace(r, "rx");
            r = new Regex(@"\/d([^aeiouxh])").Replace(r, "/k$1");
            return r;
        }

        public static string getSinhalaUnicode(string r)
        {
            r = r.Replace("hx", "ඃ");//"&#x0D83;"
            r = r.Replace("/n", "ං");
            r = r.Replace("/k", "ඞ්");
            r = r.Replace("kh", "ඛ්");
            r = r.Replace("k", "ක්");
            r = r.Replace("/g", "ඟ්");
            r = r.Replace("gh", "ඝ්");
            r = r.Replace("g", "ග්");
            r = r.Replace("cx", "ඥ්");
            r = r.Replace("/c", "ඤ්");
            r = r.Replace("ch", "ඡ්");
            r = r.Replace("c", "ච්");
            r = r.Replace("/j", "ඦ්");
            r = r.Replace("jh", "ඣ්");
            r = r.Replace("j", "ජ්");
            r = r.Replace("/dx", "ඳ්");
            r = r.Replace("/d", "ඬ්");
            r = r.Replace("txh", "ථ්");
            r = r.Replace("tx", "ත්");
            r = r.Replace("dxh", "ධ්");
            r = r.Replace("dx", "ද්");
            r = r.Replace("th", "ඨ්");
            r = r.Replace("t", "ට්");
            r = r.Replace("dh", "ඪ්");
            r = r.Replace("d", "ඩ්");
            r = r.Replace("nx", "ණ්");
            r = r.Replace("n", "න්");
            r = r.Replace("ph", "ඵ්");
            r = r.Replace("p", "ප්");
            r = r.Replace("bh", "භ්");
            r = r.Replace("/b", "ඹ්");
            r = r.Replace("b", "බ්");
            r = r.Replace("m", "ම්");
            r = r.Replace("y", "ය්");
            r = r.Replace("rxx", "ඎ");
            r = r.Replace("rx", "ඍ");
            r = r.Replace("r", "ර්");
            r = r.Replace("lxxx", "ඐ");
            r = r.Replace("lxx", "ඏ");
            r = r.Replace("lx", "ළ්");
            r = r.Replace("l", "ල්");
            r = r.Replace("v", "ව්");
            r = r.Replace("w", "ව්");
            r = r.Replace("z", "ශ්");
            r = r.Replace("sx", "ශ්");
            r = r.Replace("sh", "ෂ්");
            r = r.Replace("s", "ස්");
            r = r.Replace("h", "හ්");
            r = r.Replace("f", "‍ෆ්");

            r = r.Replace("්ai", "ෛ");
            r = r.Replace("්au", "ෞ");
            r = r.Replace("්aee", "ෑ");
            r = r.Replace("්ae", "ැ");
            r = r.Replace("්aa", "ා");
            r = r.Replace("්a", "");
            r = r.Replace("්ii", "ී");
            r = r.Replace("්i", "ි");
            r = r.Replace("්uu", "ූ");
            r = r.Replace("්u", "ු");
            r = r.Replace("්ee", "ේ");
            r = r.Replace("්e", "ෙ");
            r = r.Replace("්oo", "ෝ");
            r = r.Replace("්o", "ො");

            r = r.Replace("ai", "ඓ");
            r = r.Replace("au", "ඖ");
            r = r.Replace("aee", "ඈ");
            r = r.Replace("ae", "ඇ");
            r = r.Replace("aa", "ආ");
            r = r.Replace("a", "අ");
            r = r.Replace("ii", "ඊ");
            r = r.Replace("i", "ඉ");
            r = r.Replace("uu", "ඌ");
            r = r.Replace("u", "උ");
            r = r.Replace("ee", "ඒ");
            r = r.Replace("e", "එ");
            r = r.Replace("oo", "ඕ");
            r = r.Replace("o", "ඔ");

            r = r.Replace("්ර්ර්", "ෲ");//gaetapili-2
            r = r.Replace("්ර්", "ෘ");//gaetapili
            r = r.Replace("්ර", "්‍ර");//rakaaraansa
            r = r.Replace("්ය", "්‍ය");//yansa

            r = r.Replace("/-", "");
            r = r.Replace("/+", "‍");
            return r;
        }
        public static string readSinhalaUnicode(string r)
        {
            string s = ((char)0x200D).ToString(); 

            r = r.Replace(s, "/+");

            r = r.Replace("්ය", "්/-ය");
            r = r.Replace("්ර", "්/-ර");

            r = r.Replace("ෛ", "්ai");
            r = r.Replace("ෞ", "්au");
            r = r.Replace("ෑ", "්aee");
            r = r.Replace("ැ", "්ae");
            r = r.Replace("ා", "්aa");
            r = r.Replace("ී", "්ii");
            r = r.Replace("ි", "්i");
            r = r.Replace("ූ", "්uu");
            r = r.Replace("ු", "්u");
            r = r.Replace("ේ", "්ee");
            r = r.Replace("ෙ", "්e");
            r = r.Replace("ෝ", "්oo");
            r = r.Replace("ො", "්o");
            r = r.Replace("ෲ", "්rr");//gaetapili-2
            r = r.Replace("ෘ", "්r");//gaetapili


            r = r.Replace( "ඃ","hx");//"&#x0D83;"
            r = r.Replace( "ං","/n");
            r = r.Replace( "ඞ","/ka");
            r = r.Replace( "ඛ","kha");
            r = r.Replace( "ක","ka");
            r = r.Replace( "ඟ","/ga");
            r = r.Replace( "ඝ","gha");
            r = r.Replace( "ග","ga");
            r = r.Replace( "ඥ","cxa");
            r = r.Replace( "ඤ","/ca");
            r = r.Replace( "ඡ","cha");
            r = r.Replace( "ච","ca");
            r = r.Replace( "ඦ","/ja");
            r = r.Replace( "ඣ","jha");
            r = r.Replace( "ජ","ja");
            r = r.Replace( "ඳ","/dxa");
            r = r.Replace( "ඬ","/da");
            r = r.Replace( "ථ","txha");
            r = r.Replace( "ත","txa");
            r = r.Replace( "ධ","dxha");
            r = r.Replace( "ද","dxa");
            r = r.Replace( "ඨ","tha");
            r = r.Replace( "ට","ta");
            r = r.Replace( "ඪ","dha");
            r = r.Replace( "ඩ","da");
            r = r.Replace( "ණ","nxa");
            r = r.Replace( "න","na");
            r = r.Replace( "ඵ","pha");
            r = r.Replace( "ප","pa");
            r = r.Replace( "භ","bha");
            r = r.Replace( "ඹ","/ba");
            r = r.Replace( "බ","ba");
            r = r.Replace( "ම","ma");
            r = r.Replace( "ය","ya");
            r = r.Replace( "ඎ","rxx");
            r = r.Replace( "ඍ","rx");
            r = r.Replace( "ර","ra");
            r = r.Replace( "ඐ","lxxx");
            r = r.Replace( "ඏ","lxx");
            r = r.Replace( "ළ","lxa");
            r = r.Replace( "ල","la");
            r = r.Replace( "ව","va");
            r = r.Replace( "ශ","sxa");
            r = r.Replace( "ෂ","sha");
            r = r.Replace( "ස","sa");
            r = r.Replace( "හ","ha");
            r = r.Replace( "ෆ","fa");
            r = r.Replace("a්", "");


            r = r.Replace( "ඓ","/-ai");
            r = r.Replace("ඖ", "/-au");
            r = r.Replace("ඈ", "/-aee");
            r = r.Replace("ඇ", "/-ae");
            r = r.Replace("ආ", "/-aa");
            r = r.Replace("අ", "/-a");
            r = r.Replace("ඊ", "/-ii");
            r = r.Replace("ඉ", "/-i");
            r = r.Replace("ඌ", "/-uu");
            r = r.Replace("උ", "/-u");
            r = r.Replace("ඒ", "/-ee");
            r = r.Replace( "එ","/-e");
            r = r.Replace( "ඕ","/-oo");
            r = r.Replace( "ඔ","/-o");

            
            //r = r.Replace( "්‍ර","්ර");//rakaaraansa
            //r = r.Replace( "්‍ය","්ය");//yansa
            //r = r.Replace("/+y", "y");
            //r = r.Replace("/+r", "r");
            
            r = new Regex(@"[^a-z]/\-").Replace(r, "");
            r = new Regex(@"^/\-").Replace(r, "");
            r = new Regex(@"/\+([yr])").Replace(r, "$1");
            
            return r;
        } 
    }
}
