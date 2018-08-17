using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MVCWEF.Models;

namespace MVCWEF.Controllers
{
    public class ProductController : Controller
    {
        //Connection
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=MvcCrudDB;Integrated Security=True";
        // GET: Product
        [HttpGet]
        public ActionResult Index()
        {
            DataTable dtbProduct = new DataTable();
            using (SqlConnection sqlCon= new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM PRODUCT",sqlCon);
                sqlDa.Fill(dtbProduct);
            }
            return View(dtbProduct);
        }


        // GET: Product/Create
        [HttpGet]
        //create view
        public ActionResult Create()
        {
            var Product = new ProductModel();
            return View(Product);
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductModel productModel)
        {
            using (SqlConnection sqlCon= new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO Product Values(@ProductName,@Price,@Count)";
                SqlCommand sqlCmd = new SqlCommand(query,sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProductName",productModel.ProductName);
                sqlCmd.Parameters.AddWithValue("@Price",productModel.Price);
                sqlCmd.Parameters.AddWithValue("@Count",productModel.Count);
                sqlCmd.ExecuteNonQuery();
            }
                return RedirectToAction("Index");
            
        }

        // GET: Product/Edit/5
        //create view

        public ActionResult Edit(int id)
        {
          
            ProductModel productModel = new ProductModel();
            DataTable dtbProduct = new DataTable();
            using (SqlConnection sqlCon= new SqlConnection(connectionString))
            {
                sqlCon.Open();
                String query = "SELECT * FROM Product Where ProductID=@productID ";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query,sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("@productID",id);
                sqlDa.Fill(dtbProduct);
            }
            if (dtbProduct.Rows.Count == 1)
            {
                productModel.ProductID = Convert.ToInt32(dtbProduct.Rows[0][0].ToString());
                productModel.ProductName = dtbProduct.Rows[0][1].ToString();
                productModel.Price = Convert.ToDecimal(dtbProduct.Rows[0][2].ToString());
                productModel.Count = Convert.ToInt32(dtbProduct.Rows[0][3].ToString());
                return View(productModel);
            }
            else
                return RedirectToAction("Index");
           
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(ProductModel productModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE Product SET ProductName=@ProductName, Price=@Price,Count=@count where ProductID=@productID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@ProductID", productModel.ProductID);
                sqlCmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                sqlCmd.Parameters.AddWithValue("@Price", productModel.Price);
                sqlCmd.Parameters.AddWithValue("@Count", productModel.Count);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
           
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM Product WHERE ProductID=@productID";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);

                sqlCmd.Parameters.AddWithValue("@ProductID", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

      
    }
}
