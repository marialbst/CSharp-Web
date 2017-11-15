﻿using System.Text;

namespace WebServer.ByTheCakeApp.Models
{
    public class Cake
    {
        public Cake(int id, string name, string price)
        {
            this.Id = id;
            this.Name = name;
            this.Price = decimal.Parse(price);
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine($"<div>name: {this.Name}</div>");
            result.Append($"<div>price: {this.Price:f2}</div>");

            return result.ToString();
        }
    }
}
