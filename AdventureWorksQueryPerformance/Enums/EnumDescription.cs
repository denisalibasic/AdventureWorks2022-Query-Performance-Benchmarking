namespace AdventureWorksQueryPerformance.Enums
{
    public class EnumDescription : Attribute
    {
        public string Description { get; }

        public EnumDescription(string description)
        {
            Description = description;
        }
    }
}
