/*
    This source file is created by @Chion82
    for the purpose of exporting real-time game data.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Diagnostics;

namespace Grabacr07.KanColleWrapper
{
    class DataExportUtils
    {
        static public void WriteStringToFile(String filePath, String stringToWrite) 
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(stringToWrite);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at DataExportUtils.WriteStringToFile()");
                Console.WriteLine(e.ToString());
            }
        }

        static public long GetUnixTimeStramp(DateTime datetime)
        {
            return (long)((datetime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
        }
    }
}
