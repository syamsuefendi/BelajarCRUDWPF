﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarCRUDWPF.Model
{
    [Table("TB_M_Item")]
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }

        public Supplier Supplier { get; set; } //Foreign Key automatic

        //constructor
        public Item()
        {
        }

        //constructor with parameter
        public Item(string name, int price, int stock, Supplier supplier)
        {
            this.Name = name;
            this.Price = price;
            this.Stock = stock;
            this.Supplier = supplier;
        }

    }
}
