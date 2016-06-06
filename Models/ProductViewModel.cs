using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CanBuyWeb.Models
{
 
    public class ProductCreate
    {
        [Required]
        [Display(Name = "品名")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "最低價格")]
        public decimal MinPrice { get; set; }

        [Required]
        [Display(Name = "最高價格")]
        public decimal MaxPrice { get; set; }

        [Required]
        [Display(Name = "商品說明")]
        public string description { get; set; }
    }

    public class ProductIndex
    {
        public int ID { get; set; }
       
        public string Name { get; set; }
       
        public decimal MinPrice { get; set; }

       
        public decimal MaxPrice { get; set; }

        public byte[] Image { get; set; }
        public string ImagePath { get; set; }

        public int ImageId { get; set; }
    }

    public class ProductEdit
    {
        
        public int ID { get; set; }

        [Required]
        [Display(Name = "品名")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "最低價格")]
        public decimal MinPrice { get; set; }

        [Required]
        [Display(Name = "最高價格")]
        public decimal MaxPrice { get; set; }

        [Required]
        [Display(Name = "商品說明")]
        public string description { get; set; }

        
        public List<Product_Images> Images { get; set; }
    }

    public class ProductDetail
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "品名")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "最低價格")]
        public decimal MinPrice { get; set; }

        [Required]
        [Display(Name = "最高價格")]
        public decimal MaxPrice { get; set; }

        [Required]
        [Display(Name = "商品說明")]
        public string description { get; set; }


        public List<Product_Images> Images { get; set; }
    }
}