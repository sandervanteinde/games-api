using Microsoft.AspNetCore.Components;

namespace GameApis.Examples.Components.Grid;

public partial class GameApiGrid<TItem>
{
    private IEnumerable<IEnumerable<TItem>> _itemsGroupedByMaxSize = Enumerable.Empty<IEnumerable<TItem>>();

    [Parameter] public IEnumerable<TItem>? Items { get; set; }

    [Parameter] public int MaxItemsPerRow { get; set; } = 4;

    [Parameter] public string Gap { get; set; } = "10px";

    [Parameter] public RenderFragment<TItem> GridItem { get; set; }

    protected override void OnParametersSet()
    {
        _itemsGroupedByMaxSize = Items?.Chunk(MaxItemsPerRow) ?? Enumerable.Empty<IEnumerable<TItem>>();
    }
}