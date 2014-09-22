using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace sritext
{
    static public class variations
    {
        static int variations_keys_max_length = 0;

        public static List<string> Chars = new List<string>();
        static Dictionary<string, List<List<int>>> m_variations = new Dictionary<string, List<List<int>>>();
        static Dictionary<string, Dictionary<string, int>> Counts,OriginalCounts;
        static int totalCounts;
        static variations()
        {
            Counts = new Dictionary<string, Dictionary<string, int>>();
            OriginalCounts = new Dictionary<string, Dictionary<string, int>>();
            totalCounts = 0;
            string[] lines = File.ReadAllLines("vars.csv");
            for (int i = 0; i < lines.Length; i++)
                if (lines[i] != "")
                {
                    string[] lst = lines[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string c in lst[0].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                        if (!Chars.Contains(c))
                            Chars.Add(c);
                    List<int> SplitedChars = SplitIntoChars(lst[0]);
                    Counts[lst[0]] = new Dictionary<string, int>();
                    OriginalCounts[lst[0]] = new Dictionary<string, int>();
                    for (int j = 1; j < lst.Length; j+=2)
                        if (lst[j] != "")
                        {
                            string s = lst[j] == "<null>" ? "" : lst[j];
                            if (!m_variations.ContainsKey(s))
                                m_variations[s] = new List<List<int>>();
                            m_variations[s].Add(SplitedChars);
                            variations_keys_max_length = Math.Max(s.Length, variations_keys_max_length);
                            //if (!Counts[lst[0]].ContainsKey(s))
                            Counts[lst[0]][s] = Convert.ToInt32(lst[j + 1]);
                            OriginalCounts[lst[0]][s] = Counts[lst[0]][s];
                            totalCounts +=Math.Abs(Counts[lst[0]][s]);
                        }
                } 
            Save(); 
        }
        static void Save()
        {
            string s = "";
            foreach (string l in Counts.Keys)
            {
                s+=l + ",";
                double S=0, So=0, P=0, Po=0;
                foreach (string k in Counts[l].Keys)
                {
                    S += Math.Abs(Counts[l][k]);
                    So += Math.Abs(OriginalCounts[l][k]);
                    P += Counts[l][k] > 0 ? Counts[l][k] : 0;
                    Po += OriginalCounts[l][k] > 0 ? OriginalCounts[l][k] : 0;
                }
                foreach (string k in Counts[l].Keys)
                {
                    double v = Counts[l][k]>0?Counts[l][k]:OriginalCounts[l][k];
                    v *= (v>0?Po/P:1)*S / So;
                    s += k == "" ? "<null>" : k;
                    s += "," +Math.Round(v).ToString() + ",";
                }
                //s+=string.Format("S=,{0},So=,{1},P=,{2},Po=,{3}:\r\n",S,So,P,Po);
                s += "\r\n";
            }
            File.WriteAllText("vars.csv", s);
        }
        static public Dictionary<int, List<List<int>>> consume(string s)
        {
            Dictionary<int, List<List<int>>> r = new Dictionary<int, List<List<int>>>();
            for (int i = Math.Min(s.Length, variations_keys_max_length); i >= 0; i--)
                if (m_variations.ContainsKey(s.Substring(0, i)))
                    r[i] = m_variations[s.Substring(0, i)];
            return r;
        }
        static public List<int> SplitIntoChars(string s)
        {
            List<int> r = new List<int>();
            if (-1 == s.IndexOf('|'))
            {
                while ("" != s)
                {
                    int x = -1;
                    for (int i = 0; i < Chars.Count; i++)
                        if (s.StartsWith(Chars[i]) && (x == -1 || Chars[i].Length > Chars[x].Length))
                            x = i;
                    if (-1 == x)
                    {
                        Console.WriteLine("Split Error");
                        return null;
                    }
                    r.Add(x);
                    s = s.Substring(Chars[x].Length);
                }
                return r;
            }
            foreach (string c in s.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int x = Chars.IndexOf(c);
                if (x < 0)
                {
                    Console.WriteLine("Split Error");
                    return null;
                }
                r.Add(x);
            }
            return r;
        }
        static public string MergeChars(List<int> l)
        {
            string r = "";
            foreach (int i in l)
                r += Chars[i]+"|";
            r=r.Length>0?r.Substring(0,r.Length-1):r;
            return r;
        }

        static public Dictionary<string, Dictionary<string, int>> Parse(string s, List<int> chars)
        {
            Dictionary<string, Dictionary<string, int>> r =
                new Dictionary<string, Dictionary<string, int>>();
            string x;
            ParsePvt(s, chars, out x);
            Match m=Regex.Match(x,@"(([^(]*)\(([^)]*)\)\|)*");
            //m.Groups[0].Captures;
            for (int i = 0; i < m.Groups[2].Captures.Count; i++)
            {
                string b = m.Groups[2].Captures[i].ToString();
                string a = m.Groups[3].Captures[i].ToString();
                Console.WriteLine("{0} {1}", a,b);
                if (Counts[a].ContainsKey(b))
                {
                    if (!r.ContainsKey(a)) r[a] = new Dictionary<string, int>();
                    if (!r[a].ContainsKey(b)) r[a][b] = 0;
                    r[a][b]++;
                }
            }
            return r;
        }
        static public string Parse2String(string s, List<int> chars)
        {
            string x;
            ParsePvt(s, chars, out x);
            return x;
        }
        static double ParsePvt(string s, List<int> chars, out string result)
        {
            result = "";
            if (chars.Count == 0)
                if (s.Length == 0)
                    return 0;
                else
                    return -1000;

            double p = double.NegativeInfinity; ;
            Dictionary<int, List<List<int>>> consumes = variations.consume(s);
            foreach (int i in consumes.Keys)
                foreach (List<int> candidate in consumes[i])
                {
                    if (candidate.Count > chars.Count) continue;
                    bool ok = true;
                    for (int j = 0; j < candidate.Count; j++)
                        if (candidate[j] != chars[j])
                            ok = false;
                    if (ok)
                    {
                        string a = s.Substring(0, i);
                        string cands = variations.MergeChars(candidate);
                        string rtemp;
                        //Console.WriteLine("\t{0}=>{1}", a, cands);
                        double ptemp =
                            ParsePvt(s.Substring(i), chars.GetRange(candidate.Count, chars.Count - candidate.Count),out rtemp) +
                            GetProbability(cands, a);
                        //Console.WriteLine("\t{0}=>{1} {2:N2} {3:N2} {4}", a, cands, ptemp, p,result);
                        if (ptemp >= p)
                        {
                            p = ptemp;
                            result = a+"("+cands+")|"+rtemp;
                            //Console.WriteLine("OK: {0}",result);
                        }
                    }
                }
            if (result == "" && s=="")
            {
                foreach (int x in chars)
                    result += string.Format("({0})|", Chars[x]);
                p = -1000;
            }
            //Console.WriteLine("{0} {1} {2:N2} {3}", s, variations.MergeChars(chars),p,result);
            return p;
        }
        static public void FeedBack(string s,string w)
        {
            List<int> l= variations.SplitIntoChars(w.ToString());
            if (l == null) return;
            Dictionary<string, Dictionary<string, int>> p = variations.Parse(s,l);
            if (p == null) return;
            foreach (string chrs in p.Keys)
                foreach (string inp in p[chrs].Keys)
                    if(Counts[chrs][inp]!=-1)
                    {
                        Counts[chrs][inp] += Math.Sign(Counts[chrs][inp]) * p[chrs][inp];
                        totalCounts +=Math.Abs(p[chrs][inp]);
                    }
            Save();    
        }
        static public double GetProbability(string chrs, string s)
        {
            return Math.Log(Math.Abs(Counts[chrs][s])) - Math.Log(totalCounts); 
        }
    }
}
