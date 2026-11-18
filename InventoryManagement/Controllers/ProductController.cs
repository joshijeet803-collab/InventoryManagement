using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Products";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = (int)dr["ProductId"],
                        ProductName = dr["ProductName"].ToString(),
                        Category = dr["Category"].ToString(),
                        Quantity = (int)dr["Quantity"],
                        Price = (decimal)dr["Price"],
                        Description = dr["Description"].ToString()
                    });
                }
            }

            return View(products);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = @"INSERT INTO Products
                (ProductName, Category, Quantity, Price, Description)
                VALUES (@Name, @Category, @Quantity, @Price, @Description)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", product.ProductName);
                cmd.Parameters.AddWithValue("@Category", product.Category);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Description",
                    (object)product.Description ?? System.DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Product product = null;

            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM Products WHERE ProductId=@Id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    product = new Product
                    {
                        ProductId = (int)dr["ProductId"],
                        ProductName = dr["ProductName"].ToString(),
                        Category = dr["Category"].ToString(),
                        Quantity = (int)dr["Quantity"],
                        Price = (decimal)dr["Price"],
                        Description = dr["Description"].ToString()
                    };
                }
            }

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = @"UPDATE Products SET
                ProductName=@Name, Category=@Category,
                Quantity=@Quantity, Price=@Price,
                Description=@Description
                WHERE ProductId=@Id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", product.ProductId);
                cmd.Parameters.AddWithValue("@Name", product.ProductName);
                cmd.Parameters.AddWithValue("@Category", product.Category);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Description",
                    (object)product.Description ?? System.DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = "DELETE FROM Products WHERE ProductId=@Id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}