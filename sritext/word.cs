using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace sritext
{
    public partial class trie
    {
        public class word : IComparable
        {
            static public word NewLoad(BinaryReader br,trie tr)
            {
                return (new word(tr)).Load(br);
            }
            public word Load(BinaryReader br)
            {
                m_index=(int)br.BaseStream.Position;
                m_word=br.ReadString();
                count = br.ReadDouble();// +trie.him[(int)trie.hime.nin];
                xCount = br.ReadDouble();// +trie.him[(int)trie.hime.nin];
                children.Clear();
                for (int i = br.ReadInt32(); i != 0; i--)
                    children[br.ReadInt32()] = br.ReadInt32();
                if (grandChildren == null) grandChildren = new List<int>();
                grandChildren.Clear();
                for (int i = br.ReadInt32(); i != 0; i--)
                    grandChildren.Add(br.ReadInt32());
                return this;
            }
            
            public int Save(BinaryWriter bw, StreamWriter csvfile,bool NewWordsOnly)
            {
                if(csvfile!=null)
                if (count != 0&&((!NewWordsOnly)||0>m_index))
                    csvfile.WriteLine(ToString() + ","+count.ToString());
                foreach (KeyValuePair<int, int> c in children)
                    Trie.GetSavedIndex(c.Value, bw,csvfile,NewWordsOnly);
                int r = (int)bw.BaseStream.Position;
                bw.Write(m_word);
                bw.Write(count);
                bw.Write(xCount);
                bw.Write(children.Count);
                foreach (KeyValuePair<int, int> c in children)
                {
                    bw.Write(c.Key);
                    bw.Write(Trie.GetSavedIndex(c.Value, null,null,false));
                }
                List<int> grand = GetAllChildrenIndexes();
                bw.Write(grand.Count);
                foreach (int g in grand)
                    if(m_index==g)
                        bw.Write(r);
                    else
                        bw.Write(Trie.GetSavedIndex(g, null,null,false));
                bw.Flush();
                return r;
            }

            static public word DummyWord(string s)
            {
                word r=new word();
                r.m_word=s;
                return r;
            }
            static public word DummyWord(string s,int count)
            {
                word r = DummyWord(s);
                r.count = count;
                return r;
            }
            word()
            {}
            public word(trie tr)
            {
                Trie = tr;
            }
            public word(int index, trie tr)
            {
                m_index = index;
                Trie = tr;
            }
            public word(string s_word, double p_count, double xcnt, trie tr)
            {
                m_word = s_word;
                count = p_count;
                xCount = xcnt;
                Trie = tr;
                TravelProbability = double.NegativeInfinity;
            }
            public double count = 0;
            public double xCount = Double.NegativeInfinity;
            public double TravelProbability=double.NegativeInfinity;
            string m_word="";
            public Dictionary<int, int> children = new Dictionary<int, int>();

            public List<int> grandChildren = null;
            public int m_index=0;
            private trie Trie;
            public word Add(List<int> s, int c, string complete_word)
            {
                grandChildren = null;
                if (s.Count == 0)
                {
                    count = c;
                    m_word = complete_word;
                    return this;
                }
                int ky = s[0];
                if (!children.ContainsKey(ky))
                {
                    word w = new word(Trie);
                    children[ky] = Trie.Add2List(w);
                    w.m_index = children[ky];
                }
                return Trie.GetIndexedWord(children[ky]).Add(s.GetRange(1,s.Count-1), c, complete_word);
            }
            public List<int> GetAllChildrenIndexes()
            {
                if (grandChildren == null)
                {
                    List<int> r = new List<int>();
                    List<word> wr = new List<word>();
                    grandChildren = new List<int>();
                    if (count != 0)
                        r.Add(m_index);
                    foreach (KeyValuePair<int, int> c in children)
                        r.AddRange(Trie.GetIndexedWord(c.Value).GetAllChildrenIndexes());
                    foreach (int i in r)
                        wr.Add(Trie.GetIndexedWord(i));
                    wr.Sort();
                    for (int i = 0; i <= Trie.MaxItems && i < wr.Count; i++)
                        grandChildren.Add(wr[i].m_index);
                }
                return grandChildren;
            }
            public List<word> GetAllChildren()
            {
                List<word> r = new List<word>();
                List<int> ri = GetAllChildrenIndexes();
                ri = ri.GetRange(0, ri.Count > Trie.MaxItems ? Trie.MaxItems : ri.Count);
                foreach (int i in ri)
                    r.Add(Trie.GetIndexedWord(i));
                return r;
            }
            public override string ToString() { return m_word; }
            public int CompareTo(object o) { return (int)(10000 * (((word)o).Probability - Probability)); }

            public double OriginalProbability
            {
                get
                {
                    return Math.Log(count) - Math.Log(TRIE.totalWordCount);
                }
            }
            public double Probability
            {
                get
                {
                    if (TravelProbability == double.NegativeInfinity)
                        return OriginalProbability;
                    return OriginalProbability + xCount + TravelProbability;
                }
            }
        }
    }
}