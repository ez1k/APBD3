namespace Exercise4.models
{
    

    public class Animal
    {
        public int id { get; set; }
        public string name { get; set; } = String.Empty;
        public string description { get; set; }
        public string category { get; set; } = String.Empty;
        public string area { get; set; } = String.Empty;
        public Animal() { }
        public Animal(int id, string name, string description, string category, string area)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.category = category;
            this.area = area;
        }
    }
}
