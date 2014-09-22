using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Management;

namespace sritext
{
    public class Sec1
    {
        //public int [] him=new int [6];
        static string serial;
        public static bool LoadSearial()
        {
            if(File.Exists("serial.ini"))
                serial=File.ReadAllText("serial.ini");
            return File.Exists("serial.ini");
        }
        //public enum hime { nin, pra, itm };
        public Sec1()
        {
            string hd = inherent();
            LoadSearial();
            byte[] crbuf = Convert.FromBase64String(serial);
            byte[] buf, himbuf = new byte[24];
            //int[] him = new int[6];
            int[] x = new int[6];
            int v = int.MaxValue;
            for (int j = 0; j < 10; j++)
            {
                hd = char25(hd);
                buf = Convert.FromBase64String(hd);
                for (int i = 0; i < buf.Length; i++)
                    buf[i] ^= crbuf[i];
                Buffer.BlockCopy(buf, 0, x, 0, 24);
                int vx = 0;
                foreach (int xi in x) vx += xi;
                vx = Math.Abs(vx);
                if (vx < v)
                {
                    v = vx;
                    //Buffer.BlockCopy(x, 0, him, 0, 24);
                }
            }
        }
        
        public static string char25(string inp)
        {
            byte[] DesKey ={166,119,161,4,185,2,56,164,11,121,108,79,156,35,182,52,201,
                208,210,113,113,188,198,84};
            byte[] DesIV ={ 72, 71, 73, 179, 224, 15, 163, 143 };
            byte[] cryptData;
            char[] r = new char[25];
            byte[] source = Encoding.Unicode.GetBytes(inp);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(DesKey, DesIV),CryptoStreamMode.Write);
            cs.Write(source, 0, source.Length);
            cs.Close();
            cryptData = ms.ToArray();
            ms.Close();
            for (int i = 24; i < cryptData.Length; i++)
                cryptData[i % 24] ^= cryptData[i];
            return Convert.ToBase64String(cryptData,0,24);
        }

        static public string inherent()
        {
            string s = "";
            ManagementClass oMClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection colMObj = oMClass.GetInstances();
            foreach (ManagementObject objMO in colMObj)
                if ((bool)objMO["IPEnabled"])
                    if(s.Length<800)
                        s += /*objMO["Caption"].ToString() +*/ objMO["MacAddress"].ToString()+"\n";
            oMClass = new ManagementClass("Win32_DiskDrive");
            colMObj = oMClass.GetInstances();
            foreach (ManagementObject objMO in colMObj)
            {
                if(objMO["InterfaceType"].ToString()=="IDE")
                    if (s.Length < 800)
                        s += objMO["Caption"] + "\n";
            }
            return s;
        }
    }
}
