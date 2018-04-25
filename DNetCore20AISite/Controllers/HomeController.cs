using DNetCore20AISite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace DNetCore20AISite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var req = WebRequest.Create("http://bing.com");
            var response = req.GetResponse();
            using (var s = response.GetResponseStream())
            {
                byte[] content = new byte[512];
                s.Read(content, 0, content.Length);
                var contentS = System.Text.Encoding.UTF8.GetString(content);
                Console.WriteLine(contentS);
                ViewBag.Content = contentS;
            }

            ConnectDB();
            SqlError();

            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page." + Assembly.GetExecutingAssembly().FullName;

            ViewBag.Assemblies = AppDomain.CurrentDomain.GetAssemblies();

            ViewBag.Modules = Process.GetCurrentProcess().Modules;

            ViewBag.EnvironmentVariables = Environment.GetEnvironmentVariables();

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult ConnectDB()
        {
            string sqlstring = "Server=tcp:n604ivwf6k.database.windows.net,1433;Database=aivendortest;User ID=aivendor@n604ivwf6k;Password=AIRocks!;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
            SqlConnection con = new SqlConnection(sqlstring);
            con.Open();
            string command = "Select * From T1";
            SqlCommand com = new SqlCommand(command, con);
            SqlDataAdapter sda = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                con.Close();
                return Content(@"<script>alert('" + dt.Rows.Count + "Rows');window.location.href='Index';</script>");
            }
            else
            {
                con.Close();
                return Content(@"<script>alert('An Error');window.location.href='Index';</script>");
            }
        }

        public ActionResult SqlError()
        {
            try
            {
                string sqlstring = "Server=tcp:n604ivwf6k.database.windows.net,1433;Database=aivendortest;User ID=aivendor@n604ivwf6k;Password=AIRocks!;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
                SqlConnection con = new SqlConnection(sqlstring);
                con.Open();
                string command = "INSERT INTO T1 VALUES (1,'ERROR')";
                SqlCommand com = new SqlCommand(command, con);
                com.ExecuteNonQuery();
                return Content(@"<script>alert('No error, try another PK value');window.location.href='Index';</script>");
            }
            catch (Exception ex)
            {
                return Content(@"<script>alert('" + ex.GetType() + "');window.location.href='Index';</script>");
            }
        }
    }
}
