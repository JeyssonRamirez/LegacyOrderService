using Application.Definition.IServices;
using Core.DataTransferObject.Order;
using LegacyOrderService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LegacyOrderService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var serviceProvider = Startup.ConfigureServices();

                var appservice = serviceProvider.GetService<IOrderAppService>();

                if (appservice == null)
                {

                }
                else
                {
                    Console.WriteLine("Welcome to Order Processor!");

                    Product product = null;
                    string customerName = "";
                    int qty = 0;
                    var validCustomer = false;
                    while (!validCustomer)
                    {
                        Console.WriteLine("Enter customer name:");
                        customerName = Console.ReadLine();

                        if (!string.IsNullOrEmpty(customerName))
                        {
                            validCustomer = true;
                        }
                        else
                        {
                            Console.WriteLine("The customer name is required");
                        }
                    }
                    var validProduct = false;
                    while (!validProduct)
                    {
                        Console.WriteLine("Enter product name:");
                        string productName = Console.ReadLine();

                        var productSearch = await appservice.GetProductByName(productName);

                        if (productSearch.Success)
                        {
                            validProduct = true;
                            product = productSearch.Data;
                        }
                        else
                        {
                            Console.WriteLine("The product entered doesnt exist");
                        }
                    }


                    var validquantity = false;
                    while (!validquantity)
                    {
                        Console.WriteLine("Enter quantity:");
                        var quantity = Console.ReadLine();
                        validquantity = int.TryParse(quantity, out qty);
                        if (!validquantity)
                        {
                            Console.WriteLine("Invalid Quantity");
                        }
                        if (qty < 1)
                        {
                            validquantity = false;
                            Console.WriteLine("The Quantity  should be more that 0 ");
                        }
                    }


                    Console.WriteLine("Processing order...");

                    var order = new ModifyOrderDTO
                    {
                        ActionType = ActionType.Add
                        ,
                        CustomerName = customerName,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = qty,
                        NewPrice = product.Price,
                        Total = qty * product.Price,
                    };




                    Console.WriteLine("Customer: " + order.CustomerName);
                    Console.WriteLine("Product: " + order.ProductName);
                    Console.WriteLine("Quantity: " + order.Quantity);
                    Console.WriteLine("Total: $" + order.Total);

                    Console.WriteLine("Saving order to database...");

                    var resultSaveOrder = await appservice.ModifyOrder(order);
                    if (resultSaveOrder.Success)
                    {
                        Console.WriteLine("Order complete!");
                    }
                    else
                    {
                        Console.WriteLine(resultSaveOrder.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }




        }
    }
}
