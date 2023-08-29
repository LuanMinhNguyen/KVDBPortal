namespace EDMs.Data.Entities
{
    public class ToDoListTracking
    {
        public bool IsComplete { get; set; }
        public int ObjectTypeId { get; set; }
        public string ObjectType { get; set; }
        public string ObjectCode { get; set; }
        public string ObjectTitle { get; set; }
        public string Deadline { get; set; }
        public string ObjId { get; set; }

        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        
    }
}
