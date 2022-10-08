// Decompiled with JetBrains decompiler
// Type: Eco.Util.FileSignature
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using System.IO;

namespace Eco.Util
{
  public static class FileSignature
  {
    private static byte[][] access_signatures = new byte[2][]
    {
      new byte[19]
      {
        (byte) 0,
        (byte) 1,
        (byte) 0,
        (byte) 0,
        (byte) 83,
        (byte) 116,
        (byte) 97,
        (byte) 110,
        (byte) 100,
        (byte) 97,
        (byte) 114,
        (byte) 100,
        (byte) 32,
        (byte) 74,
        (byte) 101,
        (byte) 116,
        (byte) 32,
        (byte) 68,
        (byte) 66
      },
      new byte[19]
      {
        (byte) 0,
        (byte) 1,
        (byte) 0,
        (byte) 0,
        (byte) 83,
        (byte) 116,
        (byte) 97,
        (byte) 110,
        (byte) 100,
        (byte) 97,
        (byte) 114,
        (byte) 100,
        (byte) 32,
        (byte) 65,
        (byte) 67,
        (byte) 69,
        (byte) 32,
        (byte) 68,
        (byte) 66
      }
    };
    private static byte[][] sqlite_signatures = new byte[1][]
    {
      new byte[16]
      {
        (byte) 83,
        (byte) 81,
        (byte) 76,
        (byte) 105,
        (byte) 116,
        (byte) 101,
        (byte) 32,
        (byte) 102,
        (byte) 111,
        (byte) 114,
        (byte) 109,
        (byte) 97,
        (byte) 116,
        (byte) 32,
        (byte) 51,
        (byte) 0
      }
    };

    public static bool IsAccessDatabase(string file)
    {
      FileStream input = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      BinaryReader binaryReader = new BinaryReader((Stream) input);
      bool flag = false;
      byte[] buffer = new byte[19];
      if (binaryReader.Read(buffer, 0, buffer.Length) == buffer.Length)
      {
        for (int index1 = 0; index1 < FileSignature.access_signatures.Length; ++index1)
        {
          byte[] accessSignature = FileSignature.access_signatures[index1];
          if (accessSignature.Length <= buffer.Length)
          {
            flag = true;
            for (int index2 = 0; index2 < FileSignature.access_signatures[index1].Length & flag; ++index2)
              flag = (int) accessSignature[index1] == (int) buffer[index1];
            if (flag)
              break;
          }
        }
      }
      binaryReader.Close();
      input.Close();
      return flag;
    }

    public static bool IsSqliteDatabase(string file)
    {
      FileStream input = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      BinaryReader binaryReader = new BinaryReader((Stream) input);
      bool flag = false;
      byte[] buffer = new byte[16];
      if (binaryReader.Read(buffer, 0, buffer.Length) == buffer.Length)
      {
        for (int index1 = 0; index1 < FileSignature.sqlite_signatures.Length; ++index1)
        {
          byte[] sqliteSignature = FileSignature.sqlite_signatures[index1];
          if (sqliteSignature.Length <= buffer.Length)
          {
            flag = true;
            for (int index2 = 0; index2 < FileSignature.sqlite_signatures[index1].Length & flag; ++index2)
              flag = (int) sqliteSignature[index1] == (int) buffer[index1];
            if (flag)
              break;
          }
        }
      }
      binaryReader.Close();
      input.Close();
      return flag;
    }
  }
}
