namespace Server
{
  using System;
  using System.IO.Pipes;
  using System.Threading;

  using Common;

  using ProtoBuf;

  public class Server
  {
    #region -------------------- Public Methods --------------------
    public static void Main()
    {
      var ts = new Thread(Receive);
      ts.Start();

      var i = 0;
      while (i++ < 10)
      {
        var s = Console.ReadLine();
        Send(s);
      }
    }
    #endregion

    #region -------------------- Private Methods --------------------
    static void Receive()
    {
      while (true)
      {
        using (var pipe = new NamedPipeClientStream(".", "C2S.Pipe", PipeDirection.In, PipeOptions.None))
        {
          pipe.Connect();

          var message = Serializer.Deserialize<Message>(pipe);
          Console.WriteLine($"Message Text: {message.Value}");
        }
      }
    }

    static void Send(string message)
    {
      using (var pipe = new NamedPipeServerStream("S2C.Pipe", PipeDirection.Out))
      {
        pipe.WaitForConnection();

        Serializer.Serialize(
          pipe,
          new Message
          {
            Value = message
          });
      }
    }
    #endregion
  }
}