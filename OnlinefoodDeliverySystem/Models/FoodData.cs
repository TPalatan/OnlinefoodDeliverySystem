using System.ComponentModel.DataAnnotations;

namespace OnlinefoodDeliverySystem.Models
{
    public class FoodData
    {

        [Key]
        public string orderId { get; set; } 

        public string customerName { get; set; }    
        
        public string restaurantName { get; set; }  

        public string foodItem { get; set; }    

        public int totalPrice { get; set; } 

        public string status { get; set; }  
    }
}
