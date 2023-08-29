namespace EDMs.Data.Entities
{
    public class PerformanceReviewObj
    {

        public int ID { get; set; }
        public string Title { get; set; }

        public int TotalObj { get; set; }
        public int OverdueObj { get; set; }
        public int CompletedObj { get; set; }
        public int IncompleteObj { get; set; }
    }
}
