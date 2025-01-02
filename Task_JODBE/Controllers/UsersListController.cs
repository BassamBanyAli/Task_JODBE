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
                // Handle image upload if there is a file
                if (userImage != null && userImage.ContentLength > 0)
                {
                    // Generate a unique file name for the image (optional)
                    var fileName = Path.GetFileName(userImage.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    System.Diagnostics.Debug.WriteLine("File path: " + filePath);  // Add this line to see the full path

                    // Ensure the directory exists
                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));  // Create the folder if it doesn't exist
                    }
                    try
                    {

                        userImage.SaveAs(filePath);
                    }
                    catch (Exception ex)
                    {
                        // Log the error or return the error message
                        System.Diagnostics.Debug.WriteLine("Error saving image: " + ex.Message);
                        return Json(new { success = false, message = "Error in saving image" });
                    }
                    // Save the image path in the database (only the relative path, not full file path)
                    user.Photo = "/Content/Images/" + fileName;
                }

                // Password hashing and other logic
                user.passwordHash = HashPassword(user.password);
                user.passwordSalt = GenerateSalt();
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;

                // Save user to the database
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return Json(new { success = true, message = "User added successfully" });
            }

            return Json(new { success = false, message = "Error in adding user" });
        }


        private byte[] HashPassword(string password)
        {
            return new byte[0];  // Placeholder, implement your hashing logic here
        }

        private byte[] GenerateSalt()
        {
            return new byte[0];  // Placeholder, implement your salt generation logic here
        }

        public ActionResult ImportExcel()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ImportExcel(HttpPostedFileBase file)
        {
            var stopwatch = new Stopwatch(); // Initialize stopwatch to measure time
            stopwatch.Start(); // Start measuring time

            if (file != null && file.ContentLength > 0)
            {
                var users = new List<User>();

                try
                {
                    // Make sure the file is of type .xlsx
                    if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        return Json(new { success = false, message = "Invalid file format. Please upload an Excel (.xlsx) file." });
                    }

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet != null)
                        {
                            var startRow = 2; // Assuming row 1 is the header
                            var endRow = worksheet.Dimension.End.Row;

                            // Process rows
                            for (int row = startRow; row <= endRow; row++)
                            {
                                // Create a new user from the current row
                                var user = new User
                                {
                                    Name = worksheet.Cells[row, 2].GetValue<string>(), // Name in column 2
                                    Email = worksheet.Cells[row, 3].GetValue<string>(), // Email in column 3
                                    MobileNumber = worksheet.Cells[row, 4].GetValue<string>(), // MobileNo in column 4
                                    password = "123456" // Set default password to "123456"
                                };

                                // Add user to the list
                                users.Add(user);
                            }

                            // Perform bulk insert using Z.EntityFramework.Extensions
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
