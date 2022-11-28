public interface ICrafter 
{
    public CraftingRecipieData[] Recipies { get; }
    public CraftingRecipieData ActiveCraftData { get; }
    public int CraftTimeLeft { get; }

    public void OnCraftStarted(CraftingRecipieData craftData);
    public void OnCraftEnded();
    public void DecreaseCraftTimeLeft();
}
