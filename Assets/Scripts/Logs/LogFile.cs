using System;
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
        ftpClient.createDirectory("/test");
        ftpClient.upload("/Test.txt", @"D:\Test.txt");

        // WebClient client = new System.Net.WebClient();
        // Uri uri =
        //     new Uri("ftp://lewin-of-greenwich-naval-history-forum.co.uk" +
        //         "/" +
        //         new FileInfo(fileName).Name);

        // client.Credentials =
        //     new System.Net.NetworkCredential("lewin-of-greenwich-naval-history-forum.co.uk",
        //         "YdFDyYkUjKyjmseVmGkhipAB");

        // client.UploadFileAsync(uri, "STOR", fileName);
    }
}
