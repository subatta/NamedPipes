namespace Client
{
  using System;
  using System.IO.Pipes;
  using System.Threading;

  using Common;

  using ProtoBuf;

  public class Client
  {
    #region -------------------- Public Methods --------------------
    public static void Main(string[] Args)
    {
      var t = new Thread(Receive);
      t.Start();

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
        using (var pipe = new NamedPipeClientStream(".", "S2C.Pipe", PipeDirection.In, PipeOptions.None))
        {
          pipe.Connect();

          var message = Serializer.Deserialize<Message>(pipe);
          Console.WriteLine($"Message Text: {message.Value}");
        }
      }
    }

    static void Send(string message)
    {
      using (var pipe = new NamedPipeServerStream("C2S.Pipe", PipeDirection.Out))
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