using Application.Definition.IServices;
using Core.DataTransferObject;
using Core.DataTransferObject.Order;
using Crosscutting.Util.Extension;
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

                _=await appservice.UpdateDatabase();
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("Welcome to Order Processor!");

                Product product = null;
                string customerName = "";
                int qty = 0;
                var validCustomer = false;
                while (!validCustomer)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Enter customer name:");
                    customerName = Console.ReadLine();

                    if (!string.IsNullOrEmpty(customerName))
                    {
                        validCustomer = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The customer name is required");
                    }
                }
                var validProduct = false;
                while (!validProduct)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The product entered doesnt exist");
                    }
                }


                var validquantity = false;
                while (!validquantity)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Enter quantity:");
                    var quantity = Console.ReadLine();
                    validquantity = int.TryParse(quantity, out qty);
                    if (!validquantity)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Quantity");
                    }
                    if (qty < 1)
                    {
                        validquantity = false;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The Quantity  should be more that 0 ");
                    }
                }

                Console.ForegroundColor = ConsoleColor.Blue;
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Customer: " + order.CustomerName);
                Console.WriteLine("Product: " + order.ProductName);
                Console.WriteLine("Quantity: " + order.Quantity);
                Console.WriteLine("Total: $" + order.Total);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Saving order to database...");

                var resultSaveOrder = await appservice.ModifyOrder(order);
                if (resultSaveOrder.Success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Order complete!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(resultSaveOrder.Message);
                }


            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                var message = "";
#if Production
				message= "An unknown error occurred.";
#else
                message = ex.GetDevExceptionMessage();
#endif               

                Console.WriteLine($"Error: {message}");
            }
        }
    }
}
