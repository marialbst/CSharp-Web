namespace WebServer.ByTheCake.Models
{
    using System.Text;

    public class Cake
    {
        public Cake(string name, string price)
        {
            this.Name = name;
            this.Price = decimal.Parse(price);
        }
        
        public string Name { get; set; }

        public decimal Price { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine($"<div>name: {this.Name}</div>");
            result.Append($"<div>price: ${this.Price:f2}</div>");

            return result.ToString();
        }
    }
}
