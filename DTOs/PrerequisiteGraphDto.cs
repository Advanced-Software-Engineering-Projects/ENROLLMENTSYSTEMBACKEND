namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class PrerequisiteGraphDto
    {
        public Dictionary<int, List<int>> Graph { get; set; }
    }

    public class PrerequisiteNodeDto
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public int Year { get; set; }
        public bool IsMet { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }

    public class PrerequisiteEdgeDto
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public bool Animated { get; set; }
    }
}