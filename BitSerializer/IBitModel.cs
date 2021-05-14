namespace NVentimiglia
{
    /// <summary>
    ///     Datamodel interface
    /// </summary>
    public interface IBitModel
    {
        void Parse(BitSerializer stream);
    }
}
