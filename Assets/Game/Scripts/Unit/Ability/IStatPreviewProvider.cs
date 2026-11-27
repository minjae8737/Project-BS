public interface IStatPreviewProvider
{
    public bool TryGetModifierPreview(StatModifier modifier, out float currentValue, out float resultValue);
}