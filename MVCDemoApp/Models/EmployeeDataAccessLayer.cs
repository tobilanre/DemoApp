using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MVCDemoApp.Models
{
    public class EmployeeDataAccessLayer
    {
        // string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=myTestDB;Data Source=ANKIT-HP\\SQLEXPRESS";

        string connectionString = @"URI=file:C:\Users\Akanbiit\Documents\Sqlite\TestDB.db";


        //To View all employees details  
        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> lstemployee = new List<Employee>();

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                string stm = "select * from tblEmployee";

               var cmd = new SQLiteCommand(stm, con);
               SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Employee employee = new Employee();

                    employee.ID = Convert.ToInt32(rdr["EmployeeID"]);
                    employee.Name = rdr["Name"].ToString();
                    employee.Gender = rdr["Gender"].ToString();
                    employee.Department = rdr["Department"].ToString();
                    employee.City = rdr["City"].ToString();

                    lstemployee.Add(employee);
                }
                con.Close();
            }
            return lstemployee;
        }

        //To Add new employee record  
        public void AddEmployee(Employee employee)
        {
            employee.ID = Convert.ToInt32(GenerateRndNumber(3));

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();

                string stm = "Insert into tblEmployee (EmployeeId, Name,City,Department, Gender) Values(@EmployeeId, @Name, @City, @Department, @Gender)";

                var cmd = new SQLiteCommand(stm, con);
                cmd.Parameters.AddWithValue("@EmployeeId", employee.ID);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@Department", employee.Department);
                cmd.Parameters.AddWithValue("@City", employee.City);


                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //To Update the records of a particluar employee
        public void UpdateEmployee(Employee employee)
        {
           
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {

                string stm = "Update tblEmployee set Name = @Name, City = @City, Department = @Department,Gender = @Gender where EmployeeId = @EmpId";

                var cmd = new SQLiteCommand(stm, con);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@Department", employee.Department);
                cmd.Parameters.AddWithValue("@City", employee.City);


                cmd.ExecuteNonQuery();
                con.Close();


            }
        }

        //Get the details of a particular employee
        public Employee GetEmployeeData(int? id)
        {
            Employee employee = new Employee();

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {

                string stm = "SELECT * FROM tblEmployee WHERE EmployeeID= " + id;

                var cmd = new SQLiteCommand(stm, con);
                con.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    employee.ID = Convert.ToInt32(rdr["EmployeeID"]);
                    employee.Name = rdr["Name"].ToString();
                    employee.Gender = rdr["Gender"].ToString();
                    employee.Department = rdr["Department"].ToString();
                    employee.City = rdr["City"].ToString();

                }

            }
            return employee;
        }

        //To Delete the record on a particular employee
        public void DeleteEmployee(int? id)
        {

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                string stm = "Delete from tblEmployee where EmployeeId=@EmpId";

                var cmd = new SQLiteCommand(stm, con);
                con.Open();

                cmd.Parameters.AddWithValue("@EmpId", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }     
        }


        public  string GenerateRndNumber(int cnt)
        {
            var result = string.Empty;

            Guid Item = Guid.NewGuid();

            var guidArray = Item.ToString().ToCharArray();

            int n;
            foreach (var item in guidArray)
            {
                if (int.TryParse(item.ToString(), out n) == true)
                {
                    result += item.ToString();
                }
            }

            result = result.Substring(0, cnt);

            return result;
        }
    }

}