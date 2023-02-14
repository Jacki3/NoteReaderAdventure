using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using UnityEngine;

public class LogFile : MonoBehaviour
{
    static string fileName;

    static string wholeFilePath;

    public static void WriteCSV(string data)
    {
        fileName = CoreGameElements.i.gameSave.userIndex + "_GAMEDATA" + ".csv";
        wholeFilePath = Application.dataPath + "/" + fileName;

        StreamWriter tw = new StreamWriter(wholeFilePath, true);

        string newData = "";
        newData += data;

        tw.Write (newData);

        tw.Close();

        UploadResults();
    }

    public static void UploadResults()
    {
        ftp ftpClient =
            new ftp(@"ftp://ftp.lewin-of-greenwich-naval-history-forum.co.uk",
                "lewin-of-greenwich-naval-history-forum.co.uk",
                "YdFDyYkUjKyjmseVmGkhipAB");

        // ftpClient
        //     .createDirectory("Study4/GameData/" +
        //     CoreGameElements.i.gameSave.userIndex);
        ftpClient
            .upload("Study4/GameData/" +
            CoreGameElements.i.gameSave.userIndex +
            "/" +
            fileName,
            @wholeFilePath);
    }
}
