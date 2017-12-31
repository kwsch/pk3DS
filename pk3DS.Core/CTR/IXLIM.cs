namespace pk3DS.Core.CTR
{
    /// <summary>
    /// <see cref="BCLIM"/> and <see cref="BFLIM"/> header interface.
    /// </summary>
    public interface IXLIM
    {
        uint Magic { get; set; }
        ushort Width { get; set; }
        ushort Height { get; set; }
    }
}