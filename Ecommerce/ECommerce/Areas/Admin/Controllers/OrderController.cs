using ClosedXML.Excel;
using Ecommerce.Data.Entities;
using Ecommerce.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<Order> orders = await  _orderRepository.GetAllOrders();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            Order order = await _orderRepository.GetOrder(id);
            if (order is null) return NotFound();
            return View(order); 
        }

        public async Task<IActionResult> ChangeStatus(int id,int status)
        {
            try
            {
                await _orderRepository.ChangeStatus(id, status);
                return Json(new
                {
                    Message = "Orders status has been changed successfully",
                    Status = true,
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = false,
                    Message = "Something went wrong",
                });
            }
        }

        public async Task<IActionResult> Export()
        {
            List<Order> orders = await _orderRepository.GetAllOrders();
            using (var workbook = new XLWorkbook())
            {
                var workSheet = workbook.Worksheets.Add("Orders");
                int row = 1;
                workSheet.Cell(1, 1).Value = "Id";
                workSheet.Cell(1, 2).Value = "User";
                workSheet.Cell(1, 3).Value = "Product Name";
                workSheet.Cell(1, 4).Value = "Price";
                workSheet.Cell(1, 5).Value = "Color";
                workSheet.Cell(1, 6).Value = "Size";
                workSheet.Cell(1, 7).Value = "Count";
                workSheet.Cell(1, 8).Value = "Datetime";
                workSheet.Cell(1, 9).Value = "Status";

                foreach (Order order in orders)
                {
                    foreach (OrderProduct item in order.OrderProducts)
                    {
                        row++;
                        workSheet.Cell(row, 1).Value = item.Id;
                        workSheet.Cell(row, 2).Value = order.AppUser.Firstname + " " + order.AppUser.Lastname;
                        workSheet.Cell(row, 3).Value = item.ProductColorSize.ProductColor.Product.Name;
                        workSheet.Cell(row, 4).Value = item.ProductPrice;
                        workSheet.Cell(row, 5).Value = item.ProductColorSize.ProductColor.Color.Name;
                        workSheet.Cell(row, 6).Value = item.ProductColorSize.Size.Name;
                        workSheet.Cell(row, 7).Value = item.Count;
                        workSheet.Cell(row, 8).Value = order.CreatedAt.ToString("dd-MM-yyyy HH:mm");
                        workSheet.Cell(row, 9).Value = order.OrderStatus.ToString();
                    }
                }


                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "orders.xlsx");
                }
            }
        }
    }
}
