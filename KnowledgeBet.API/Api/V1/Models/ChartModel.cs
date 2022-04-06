namespace KnowledgeBet.API.Api.V1.Models
{
    public class ChartModel
    {
        public List<int> Data { get; set; }
        public string Label { get; set; }

        public ChartModel(List<int> data, string label="")
        {
            Data = data;
            Label = label;
        }
    }
}
