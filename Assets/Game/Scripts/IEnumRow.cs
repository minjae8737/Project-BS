public interface IEnumRow<TEnum>
{
    public TEnum Type { get; }
    public void RefreshName();
}