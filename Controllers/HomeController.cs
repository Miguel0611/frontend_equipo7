using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = "Data Source=imsnor.database.windows.net;Initial Catalog=db;User ID=dbUserN;Password=Npas$w0rd;";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var data = new List<Carreras>();
            SqlConnection equipo7 = new SqlConnection(connectionString);
            SqlCommand norte = new SqlCommand("SELECT [Id], [Titulo], [Creditos], [Campus] FROM [Carreras]", equipo7);
            try 
            {
                equipo7.Open();
                SqlDataReader dr = norte.ExecuteReader();

                while (dr.Read())
                {
                    data.Add(new Carreras
                    {
                        Id = (Guid)dr["Id"],
                        Titulo = (string)dr["Titulo"],
                        Creditos = (int)dr["Creditos"],
                        Campus = (string)dr["Campus"]
                    });
                }
                return View(data);
            }
            catch (Exception) { throw; }
            finally
            {
                equipo7.Close();
            }
        }
        public IActionResult Details(Guid id)
        {
            var data = new Carreras();
            SqlConnection equipo7 = new SqlConnection(connectionString);
            SqlCommand norte = new SqlCommand("SELECT [Id], [Titulo], [Creditos], [Campus] FROM [Carreras] WHERE [Id] = @i", equipo7);
            norte.Parameters.Add("@i", SqlDbType.UniqueIdentifier).Value = id;

            try
            {
                equipo7.Open();
                SqlDataReader dr = norte.ExecuteReader();
                if (dr.Read())
                {
                    data.Id = (Guid)dr["Id"];
                    data.Titulo = (string)dr["Titulo"];
                    data.Creditos = (int)dr["Creditos"];
                    data.Campus = (string)dr["Campus"];
                }
                return View(data);
            }
            catch (Exception) { throw; }
            finally
            {
                equipo7.Close();
            }

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Carreras data)
        {
            SqlConnection equipo7 = new SqlConnection(connectionString);
            SqlCommand norte = new SqlCommand(@"INSERT INTO [Carreras] ([Id], [Titulo], [Creditos], [Campus]) VALUES (NEWID(), @t, @c, @a);", equipo7);

            norte.Parameters.Add("@t", SqlDbType.NVarChar, 50).Value = data.Titulo;
            norte.Parameters.Add("@c", SqlDbType.Int).Value = data.Creditos;
            norte.Parameters.Add("@a", SqlDbType.NVarChar, 100).Value = data.Campus;
            
            try
            {
                equipo7.Open();
                norte.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                equipo7.Close();
            }
        }

        public IActionResult Edit(Guid id)
        {
            var data = new Carreras();
            SqlConnection equipo7 = new SqlConnection(connectionString);
            SqlCommand norte = new SqlCommand("SELECT [Id], [Titulo], [Creditos], [Campus] FROM [Carreras] WHERE [Id] = @i", equipo7);
            norte.Parameters.Add("@i", SqlDbType.UniqueIdentifier).Value = id;
            try
            {
                equipo7.Open();
                SqlDataReader dr = norte.ExecuteReader();
                if(dr.Read())
                {
                    data.Id = (Guid)dr["Id"];
                    data.Titulo = (string)dr["Titulo"];
                    data.Creditos = (int)dr["Creditos"];
                    data.Campus = (string)dr["Campus"];
                }
                return View(data);
            }
            catch (Exception) { throw; }
            finally
            {
                equipo7.Close();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Carreras data)
        {
            SqlConnection equipo7 = new SqlConnection(connectionString);
            SqlCommand norte = new SqlCommand(@"UPDATE [Carreras] SET [Titulo] = @t, [creditos] = @c, [Campus] = @a WHERE [Id] = @i;", equipo7);
            norte.Parameters.Add("@i", SqlDbType.UniqueIdentifier).Value = data.Id;
            norte.Parameters.Add("@t", SqlDbType.NVarChar, 50).Value = data.Titulo;
            norte.Parameters.Add("@c", SqlDbType.Int).Value = data.Creditos;
            norte.Parameters.Add("@a", SqlDbType.NVarChar, 100).Value = data.Campus;

            try
            {
                equipo7.Open();
                norte.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception) { throw; }
            finally
            {
                equipo7.Close();
            }
        }

        public IActionResult Delete(Guid id)
        {
            SqlConnection equipo7 = new SqlConnection(connectionString);
            SqlCommand norte = new SqlCommand("DELETE FROM [Carreras] WHERE [Id] = @i", equipo7);

            norte.Parameters.Add("@i", SqlDbType.UniqueIdentifier).Value = id;

            try
            {
                equipo7.Open();
                norte.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception) { throw; }
            finally
            {
                equipo7.Close();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
