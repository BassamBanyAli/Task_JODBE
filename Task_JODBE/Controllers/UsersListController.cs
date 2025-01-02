using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using Task_JODBE.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using EF6.BulkInsert.Extensions;

namespace Task_JODBE.Controllers
{
    [Authorize]
    public class UsersListController : Controller
    {
        private readonly UserManagementDBEntities _dbContext;

        public UsersListController()
        {
            _dbContext = new UserManagementDBEntities();
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUsers()
        {
            try
            {
                var users = _dbContext.Users.Select(u => new
                {
                    u.UserID,
                    u.Name,
                    u.Email,
                    Photo = u.Photo ?? "https://via.placeholder.com/45",
                    u.password,
                    u.MobileNumber
                }).ToList();

                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred while fetching users." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddUser(User user, HttpPostedFileBase userImage)
        {
            if (ModelState.IsValid)
            {

                if (userImage != null && userImage.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(userImage.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    System.Diagnostics.Debug.WriteLine("File path: " + filePath);  


                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath)); 
                    }
                    try
                    {

                        userImage.SaveAs(filePath);
                    }
                    catch (Exception ex)
                    {

                        System.Diagnostics.Debug.WriteLine("Error saving image: " + ex.Message);
                        return Json(new { success = false, message = "Error in saving image" });
                    }

                    user.Photo = "/Content/Images/" + fileName;
                }


                user.passwordHash = HashPassword(user.password);
                user.passwordSalt = GenerateSalt();
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;


                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return Json(new { success = true, message = "User added successfully" });
            }

            return Json(new { success = false, message = "Error in adding user" });
        }


        private byte[] HashPassword(string password)
        {
            return new byte[0];  
        }

        private byte[] GenerateSalt()
        {
            return new byte[0];  
        }

        public ActionResult ImportExcel()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ImportExcel(HttpPostedFileBase file)
        {
            var stopwatch = new Stopwatch(); 
            stopwatch.Start(); 

            if (file != null && file.ContentLength > 0)
            {
                var users = new List<User>();

                try
                {

                    if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        return Json(new { success = false, message = "Invalid file format. Please upload an Excel (.xlsx) file." });
                    }

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet != null)
                        {
                            var startRow = 2; 
                            var endRow = worksheet.Dimension.End.Row;


                            for (int row = startRow; row <= endRow; row++)
                            {

                                var user = new User
                                {
                                    Name = worksheet.Cells[row, 2].GetValue<string>(),
                                    Email = worksheet.Cells[row, 3].GetValue<string>(), 
                                    MobileNumber = worksheet.Cells[row, 4].GetValue<string>(), 
                                    password = "123456" 
                                };


                                users.Add(user);
                            }


                            await Task.Run(() =>
                            {
                                _dbContext.BulkInsert(users);
                            });
                        }
                    }

                    stopwatch.Stop();
                    double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

                    return Json(new { success = true, message = $"File uploaded and data imported successfully in {elapsedSeconds:F2} seconds." });
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
                }
            }

            return Json(new { success = false, message = "Please upload a valid Excel file." });
        }


    }
}
