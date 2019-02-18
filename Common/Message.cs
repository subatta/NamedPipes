namespace Common
{
  using ProtoBuf;

  [ProtoContract]
  public class Message
  {
    #region -------------------- Public Properties --------------------
    [ProtoMember(1)]
    public string Value { get; set; }
    #endregion
  }
}