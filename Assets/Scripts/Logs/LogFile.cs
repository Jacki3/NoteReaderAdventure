using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;

public static class LogFile
{
    public static void WriteCSV(string data)
    {
        string fileName =
            CoreGameElements.i.saveLocation +
            "/" +
            CoreGameElements.i.saveFileName;

        StreamWriter tw = new StreamWriter(fileName, true);

        string newData = "";
        newData += data;

        tw.Write (newData);

        tw.Close();

        ftp ftpClient =
            new ftp(@"ftp://ftp.lewin-of-greenwich-naval-history-forum.co.uk",
                "lewin-of-greenwich-naval-history-forum.co.uk",
                "YdFDyYkUjKyjmseVmGkhipAB");
        string file = CoreGameElements.i.saveFileName;
        ftpClient.upload("/" + file, @fileName);
    }
}
