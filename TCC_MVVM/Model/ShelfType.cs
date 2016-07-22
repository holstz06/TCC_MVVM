namespace TCC_MVVM.Model
{
    public class ShelfType
    {
        public string Name { get; set; }
        public string CamPostColor { get; set; }
        public int CamPostQuantity { get; set; }
        public decimal CamPostPrice { get; set; }
        public bool HasFence { get; set; }
        public string FenceColor { get; set; }
        public decimal FencePrice { get; set; }
        public bool HasTopConnector { get; set; }
        public string TopConnectorColor { get; set; }
        public decimal TopConnectorPrice { get; set; }
        public bool IsToeKick { get; set; }
    }
}
