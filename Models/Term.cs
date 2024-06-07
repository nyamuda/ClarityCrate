namespace Clarity_Crate.Models
{
    public class Term
    {

        public int Id { get; set; }

        public string? Name { get; set; }


        public Definition? Definition { get; set; }

        public IEnumerable<Level> Levels { get; set; } = new List<Level>();


    }


}
