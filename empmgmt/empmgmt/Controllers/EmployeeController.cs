using ClosedXML.Excel;
using empmgmt.Data;
using empmgmt.Extension;
using empmgmt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.OleDb;

namespace empmgmt.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IConfiguration configuration;
        bool IsSessionExpired = false;
        public EmployeeController(AppDBContext context, IConfiguration configuration)
        {
            this._context = context;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> FileImport(IFormFile execlFile)
        {
            if (execlFile != null)
            {
                string path = Path.Combine(AppContext.BaseDirectory, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = Path.GetFileName(execlFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    execlFile.CopyTo(stream);
                }
                string ExcelConnString = configuration.GetConnectionString("ExcelConString");

                DataTable dt = new DataTable();
                ExcelConnString = string.Format(ExcelConnString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(ExcelConnString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            connExcel.Open();
                            DataTable? dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                            string? sheetName = String.Empty;
                            if (dtExcelSchema != null && dtExcelSchema.Rows.Count > 0)
                                if (dtExcelSchema.Columns.Contains("TABLE_NAME"))
                                    sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            if (!string.IsNullOrEmpty(sheetName))
                            {
                                connExcel.Open();
                                cmdExcel.CommandText = "Select [Employee Id] as emp_id, [Employee Name] as emp_name,[Date of Birth] as date_of_birth,[Joining Date] as joining_date,[Skills] as skills,[Salary] as salary,[Designation] as designation,[Email] as email_id From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();
                            }
                        }
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    SqlParameter p1 = new SqlParameter("@emp", dt);
                    p1.TypeName = "dbo.tt_employee";
                    p1.SqlDbType = SqlDbType.Structured;

                    SqlParameter[] sqlParams = new SqlParameter[] { p1 };
                    string strParams = string.Join(',', sqlParams.Select(x => x.ParameterName).ToArray());
                    await _context.Database.ExecuteSqlRawAsync($"sp_employee_ins_upd_bulk {strParams}", sqlParams);
                }
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IsSessionExpired = string.IsNullOrEmpty(HttpContext?.Session?.GetString("username"));
            if (IsSessionExpired)
                return RedirectToAction("Index", "Home");

            var emp = await _context.view_employee.ToListAsync();
            return View(emp);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var emp = await _context.employee.FirstOrDefaultAsync(x => x.emp_id == id);
            if (Equals(emp, null))
                return RedirectToAction("Index");

            _context.employee.Remove(emp);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddUpdate(int id = 0)
        {
            if (Equals(id, 0))
                return View();

            var emp = await _context.view_employee.FirstOrDefaultAsync(x => x.emp_id == id);
            if (Equals(emp, null))
                return RedirectToAction("Index");

            EmployeeViewModel employee = new EmployeeViewModel
            {
                emp_id = emp.emp_id,
                emp_name = emp.emp_name,
                date_of_birth = emp.date_of_birth,
                joining_date = emp.joining_date,
                designation = emp.designation,
                salary = emp.salary,
                skills = emp.skills,
                email_id = emp.email_id
            };

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdate(EmployeeViewModel employeeRequest)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@emp_id", employeeRequest.emp_id),
                new SqlParameter("@emp_name",employeeRequest.emp_name),
                new SqlParameter("@date_of_birth",employeeRequest.date_of_birth),
                new SqlParameter("@joining_date",employeeRequest.joining_date),
                new SqlParameter("@designation",employeeRequest.designation),
                new SqlParameter("@salary",employeeRequest.salary),
                new SqlParameter("@email_id",employeeRequest.email_id),
                new SqlParameter("@skills",employeeRequest.skills)
            };
            string strParams = string.Join(',', sqlParams.Select(x => x.ParameterName).ToArray());
            await _context.Database.ExecuteSqlRawAsync($"sp_employee_ins_upd {strParams}", sqlParams);

            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> Export()
        {
            DataTable dt = new DataTable("emp");
            var emp = await _context.employee.ToListAsync();
            if (emp != null)
                dt = emp.ToDataTable();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employee.xlsx");
                }
            }
        }
    }
}

