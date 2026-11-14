using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components.Views
{
    public partial class InventoryView : GameView
    {
        protected override string ScreenFile => "Views\\InventoryView.hjson";

        [UIRequired("inventory.background")]
        private void BindBackground(Component part) => Background = part;

        [UIRequired("inventory.playerStats")]
        private void BindPlayerStats(PlayerStats part) => PlayerStats = part;

        [UIRequired("inventory.grid")]
        private void BindItemGrid(ItemGrid part)
        {
            part.SelectionChanged = stack =>
            {
                _detailName.Text(stack.Item.Name);
                _detailType.Text(stack.Item.GetType().Name);
                _detailDescription.Text(stack.Item.TooltipText);
            };
        }

        [UIRequired("inventory.detail.name")]
        private void BindDetailName(Label part) => _detailName = part;

        [UIRequired("inventory.detail.type")]
        private void BindDetailType(Label part) => _detailType = part;

        [UIRequired("inventory.detail.description")]
        private void BindDetailDescription(Label part) => _detailDescription = part;

        private Label _detailName = null!;

        private Label _detailType = null!;

        private Label _detailDescription = null!;

        [UIEvent("inventory.back")]
        private void Back() => MonoCoC.UI.Components.Views.InventoryView.Exit();
    }
}
