//#define  LOADFROMWORDLIST

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace sritext
{
    public partial class trie
    {
        public static int[] him;
        enum hime { nin, pra, itm };

        public static trie TRIE;
        static trie()
        {
            //him = new int[10];
            TRIE = new trie();
        }
        
        public int BaseWord;

        public int MaxItems { get { return 10;} }//him[(int)hime.itm];

        public Dictionary<int, word> allWords = new Dictionary<int, word>();

        
        BinaryReader trieReader;
        public word GetIndexedWord(int index)
        {
            if (!allWords.ContainsKey(index))
            {
                    //if (trieReader == null) trieReader = new BinaryReader(File.OpenRead("trie.dat"));
                    trieReader.BaseStream.Position = index;
                    allWords[index]=word.NewLoad(trieReader,this);
            }
            return allWords[index];
        }
        
        public List<word> findAll( string s,bool NoTrvelProb)
        {
            if (thread != null&&!thread.IsAlive)
            {
                thread.Abort();
                thread = null;
                trieReader.Close();
                File.Delete("trieold.dat"); File.Move("trie.dat", "trieold.dat");
                File.Delete("trie.dat"); File.Move("out.dat", "trie.dat");
                File.Delete("modified.csv"); File.Move("modified.tmp", "modified.csv");
                trieReader = new BinaryReader(File.OpenRead("trie.dat"));
                totalWordCount = trieReader.ReadInt32();
                BaseWord = trieReader.ReadInt32();
                allWords.Clear();
            }
            find_return found = new find_return();
//            if(variations.qList.ContainsKey("^"+s+"$"))
//                foreach (String w in variations.qList["^" + s + "$"].Keys)
//                {
//                    //found.Completes.Add(word.DummyWord(w.Substring(1,w.Length-2)));
//                }
            find(GetIndexedWord(BaseWord), s, NoTrvelProb?double.NegativeInfinity: 0, found);
            found.Completes.Sort();
            if (found.Completes.Count < MaxItems - 1)
            {
                List<word> PartialChildren = new List<word>();
                foreach (word w in found.Partials)
                    PartialChildren.AddRange(w.GetAllChildren());
                foreach (word w in PartialChildren)
                    if(!found.Completes.Contains(w))
                        w.TravelProbability = double.NegativeInfinity;
                PartialChildren.Sort();
                found.Add2Completes(PartialChildren);
            }
            if (found.Completes.Count < MaxItems - 1)
            {
                List<word> bi = new List<word>();
                foreach (string sx in found.Bigrams.Keys)
                {
                    List<word> w1s=found.Bigrams[sx];
                    w1s.Sort(); 
                    if(w1s.Count>MaxItems)
                        w1s =w1s.GetRange(0,MaxItems);
                    find_return bfound = find(GetIndexedWord(BaseWord), sx,NoTrvelProb?double.NegativeInfinity: 0, new find_return());
                    bfound.Completes.Sort();
                    if(bfound.Completes.Count>MaxItems)
                        bfound.Completes=bfound.Completes.GetRange(0,MaxItems);
                    foreach (word w in w1s)
                        foreach (word w2 in bfound.Completes)
                        {
                            word xw = new word(w.ToString() + " |" + w2.ToString(), w.count * w2.count/totalWordCount, w.xCount + w2.xCount, null);
                            xw.TravelProbability = NoTrvelProb ? double.NegativeInfinity :
                                found.TravelProbabilies[w.m_index.ToString() + sx]
                                + w2.TravelProbability;
                            bi.Add(xw);
                        }
                }
                bi.Sort();
                if(bi.Count>0)
                    found.Add2Completes(bi.GetRange(0,Math.Min(MaxItems,bi.Count)));
            }
            List<word> r=found.Completes.GetRange(0,Math.Min(found.Completes.Count,MaxItems));
            foreach (word w in found.Completes)
                if (w.ToString().Replace("|", "") == s)
                {
                    if (!r.Contains(w))
                        r.Add(w);
                    return r;
                }
            r.Add(word.DummyWord(s));
            return r;
        }
        class find_return
        {
            //public Dictionary<string,double> TravelProbabilities;
            public List<word> Completes,Partials;
            public Dictionary<string,List<word>> Bigrams;
            public Dictionary<string, double> TravelProbabilies;
            public find_return()
            {
                Completes = new List<word>();
                Partials = new List<word>();
                Bigrams = new Dictionary<string, List<word>>();
                TravelProbabilies = new Dictionary<string, double>();
            }
            public void Add2CompletesPartials(word w, double TravelProbability) 
            {
                if (w.count !=0)
                {
                    if (Completes.Contains(w))
                        w.TravelProbability = Math.Max(w.TravelProbability, TravelProbability);
                    else
                    {
                        Completes.Add(w);
                        w.TravelProbability = TravelProbability;
                        //TravelProbabilities[w.ToString()+":"+u] = TravelProbability;
                    }
                    //Console.WriteLine("{0} {1}",w, TravelProbability);
                }
                Add2Partials(w);                 
            }
            public void Add2Completes(List<word> wl) { Add2Set(Completes, wl); }
            public void Add2Partials(word w) { Add2Set(Partials, w); }
            public void Add2Bigrams(string s, word w, double TravelProbability)
            {
                if (w.count == 0)
                    return;
                if (!Bigrams.ContainsKey(s))
                    Bigrams[s] = new List<word>();
                if (!Completes.Contains(w))
                {
                    if (Bigrams[s].Contains(w))
                        w.TravelProbability = Math.Max(w.TravelProbability, TravelProbability);
                    else
                        w.TravelProbability = TravelProbability;
                }
                Add2Set(Bigrams[s], w);
            }
            void Add2Set(List<word> l, word w)
            {
                if (!l.Contains(w))
                    l.Add(w);
            }
            void Add2Set(List<word> l,List<word> wl)
            {
                foreach(word w in wl)
                    if (!l.Contains(w))
                        l.Add(w);
            }
        }
        find_return find(word w, string s, double p, find_return r /*bool NoTrvelProb*/)
        {
            if (w == null) return r;
            string key = w.m_index.ToString() + s;
            if (r.TravelProbabilies.ContainsKey(key) && 
                p<r.TravelProbabilies[key])
                return r;
            r.TravelProbabilies[key] = p;
            if (s == "")
                r.Add2CompletesPartials(w, p);
            else
                r.Add2Bigrams(s, w,  p);
            Dictionary<int, List<List<int>>> consumes = variations.consume(s);
            foreach (int i in consumes.Keys)
                foreach (List<int> candidate in consumes[i])
                    {
                        word tmp = w;
                        double pv = variations.GetProbability(variations.MergeChars(candidate), s.Substring(0, i));
                        foreach (int c in candidate)
                        {
                            if (tmp.children.ContainsKey(c))
                                tmp = GetIndexedWord(tmp.children[c]);
                            else
                            {
                                tmp = null;
                                break;
                            }
                        }
                        find(tmp, s.Substring(i), p + pv,r);                       
                    }
                return r;
        }
        int totalWordCount;
        Dictionary<int, double> CharCounts;
        public void List2Trie(string filename)
        {
            CharCounts = new Dictionary<int, double>();
            double totalCharCount = 0;
            for (int i = 0; i < variations.Chars.Count; i++)
                CharCounts[i] = 1;
            allWords = new Dictionary<int, word>();
            allWords[-1] = new word(-1, this);
            BaseWord = -1;

            
            string[] a_splited_words = File.ReadAllLines("splited.csv");
            List<string> splited_words = new List<string>(a_splited_words);
            int c = 0;
            totalWordCount = 0;
            foreach (string s in splited_words)
            {
                c++;
                Match m = Regex.Match(s, "(.*?),([0-9]+)");
                if (m.Groups.Count == 3)
                {
                    string str = m.Groups[1].ToString();
                    List<int> chrs = variations.SplitIntoChars(str);
                    int cnt = Convert.ToInt32(m.Groups[2].ToString());
                    totalWordCount += cnt;
                    foreach (int i in chrs)
                        CharCounts[i] += cnt;
                    totalCharCount += chrs.Count * cnt;
                    GetIndexedWord(BaseWord).Add(chrs,cnt, str);
                }
            }
            BinaryWriter bw=new BinaryWriter(File.OpenWrite("charprobabilities.dat"));
            string charprobabilities = "";
            for (int i = 0; i < variations.Chars.Count; i++)
            {
                CharCounts[i] = Math.Log(CharCounts[i]) - Math.Log(totalCharCount);
                charprobabilities += variations.Chars[i] + "," + CharCounts[i].ToString() + "\n";
                bw.Write(CharCounts[i]);
            }
            bw.Close();
            File.WriteAllText("charprobabilities.csv", charprobabilities);
            xCountCalcAll(BaseWord, 0);

            
            Save(filename,"modified.csv");
        }
        
            public Dictionary<int, int> SavedAt;
            public int GetSavedIndex(int index, BinaryWriter bw, StreamWriter csvfile,bool NewWordsOnly)
            {
                if (!SavedAt.ContainsKey(index))
                    SavedAt[index] = GetIndexedWord(index).Save(bw, csvfile,NewWordsOnly);
                return SavedAt[index];
            }
            public void Save(string filename, string csvfilename)
            {
                SavedAt = new Dictionary<int, int>();
                FileStream fs = File.Create(filename);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(totalWordCount);
                bw.Write((int)0);
                bw.Flush();
                Console.WriteLine(bw.BaseStream.Position);
                StreamWriter csvfile= File.CreateText(csvfilename);
                int x = GetSavedIndex(BaseWord, bw, csvfile,false);
                csvfile.Close();
                bw.BaseStream.Position = 4;
                bw.Write(x);
                bw.Flush();
                bw.Close();
                fs.Close();
            }
        

        void xCountCalcAll(int w, double p)
        {
            word x=GetIndexedWord(w);
            x.xCount = - p;
            foreach (KeyValuePair<int, int> c in x.children)
                xCountCalcAll(c.Value, CharCounts[c.Key] + p);
        }
        void xCountCalc(word w)
        {
            w.xCount = 0;
            foreach(int chr in variations.SplitIntoChars(w.ToString()))
                w.xCount-=CharCounts[chr];
        }
        //public Sec sec;
        trie()
        {
            //sec = new Sec();
            /*if (trieReader == null)*/ trieReader = new BinaryReader(File.OpenRead("trie.dat")); 
            totalWordCount = trieReader.ReadInt32();
            BaseWord = trieReader.ReadInt32();
            if (File.Exists("charprobabilities.dat"))
            {
                CharCounts = new Dictionary<int, double>();
                BinaryReader bw = new BinaryReader(File.OpenRead("charprobabilities.dat"));
                for (int i = 0; i < variations.Chars.Count; i++)
                    CharCounts[i] = bw.ReadDouble();
                bw.Close();
            }
        }
        /*~trie()
        {
            TRIE.List2Trie("20090613.dat");
        }*/
        public int Add2List(word w)
        {
            allWords[-allWords.Count-1]=w;
            return -allWords.Count;
        }
        BinaryReader ThreadReader;
        StreamWriter csvfile;
        BinaryWriter bw;
        void ThreadFunction()
        {
            File.Copy("trie.dat", "out.tmp", true);
            //File.Copy("modified.csv", "modified.tmp", true);
            csvfile = File.CreateText("modified.tmp");// AppendText("modified.tmp");
            ThreadReader = new BinaryReader(File.OpenRead("out.tmp"));
            ThreadReader.BaseStream.Position = 8;
            bw = new BinaryWriter(File.OpenWrite("out.dat"));
            bw.Write(totalWordCount);
            bw.Write((int)0);
            SavedAt = new Dictionary<int, int>();
            word tmpWord = new word(this);
            tmpWord.grandChildren = new List<int>();
            while (ThreadReader.BaseStream.Position < ThreadReader.BaseStream.Length)
            {                
                tmpWord.Load(ThreadReader);
                if (!ThreadParameters.Contains(tmpWord.m_index))
                {
                    SavedAt[tmpWord.m_index] = (int)bw.BaseStream.Position;
                    tmpWord.Save(bw, csvfile,false);
                }
            }
            int x = GetSavedIndex(BaseWord, bw, csvfile,true);
            csvfile.Close();
            bw.BaseStream.Position = 4;
            bw.Write(x);
            bw.Close();
            ThreadReader.Close();
            ThreadParameters.Clear();
        }
        
        Thread thread = null;
        List<int> ThreadParameters=new List<int>();
        
        public void FeedBack(string s, word w)
        {
            if(w.ToString().IndexOf(" ")!=-1)
                return;
            if (allWords.ContainsValue(w))
            {
                if (w.m_index > 0 && thread == null)
                {
                    if (trieReader != null)
                    {
                        trieReader.Close();
                        trieReader = null;
                    }
                    variations.FeedBack(s, w.ToString());
                    totalWordCount++;
                    w.count ++;
                    FileStream fs = File.OpenWrite("trie.dat");
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(totalWordCount);
                    bw.Flush();
                    fs.Position = w.m_index;
                    fs.Position += w.ToString().Length + 1;
                    bw.Write(w.count);
                    bw.Write(w.xCount);
                    bw.Close();
                    fs.Close();
                    trieReader = new BinaryReader(File.OpenRead("trie.dat"));
                }
            }
            else
            {
                List<int> chrs=variations.SplitIntoChars(w.ToString());
                if (chrs == null) return;
                word nw=GetIndexedWord(BaseWord).Add(chrs,1,variations.MergeChars(chrs)+"|");
                totalWordCount++;
                xCountCalc(nw);
                word now=GetIndexedWord(BaseWord);
                ThreadParameters.Add(now.m_index);
                foreach (int chr in chrs)
                {
                    now = GetIndexedWord(now.children[chr]);
                    Console.WriteLine("{0} {1}",variations.Chars[chr],now.m_index);
                    if (now.m_index > 0)
                        ThreadParameters.Add(now.m_index);
                }
                if (thread != null)
                {
                    thread.Abort();
                    ThreadReader.Close();
                    bw.Close();
                    csvfile.Close();
                }
                thread = new Thread(ThreadFunction);
                thread.Start();
            }
        }
    }
}


