namespace CoreCodeCamp.Camps
{
    using System;

    public class CampModel
    {
        public string Name { get; set; }
        public string Moniker { get; set; }
        public DateTime EventDate { get; set; }
        public int Length { get; set; } = 1;
    }
}
