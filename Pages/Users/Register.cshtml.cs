using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace freelancer.Pages.Users
{
    public class RegisterModel : PageModel
    {
        public UserInfo user = new UserInfo();
        public String errorMessage = "";
        public String successMessage = "";
        
        public void onGet()
        {

        }
        public bool validate(UserInfo x)
        {
            if (!Regex.IsMatch(x.mail, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
            {
                errorMessage = "Invalid email address. Please enter a valid email address in the format \"example@example.com\"";
                return false;
            }

            if (!Regex.IsMatch(x.phone, @"^(01[0-46-9]-\d{7,8}|011-\d{7,8}|01[2-9]-\d{7}|02\d-\d{7,8})$"))
            {
                errorMessage = "Invalid phone number. Please enter a valid Malaysian phone number in the format \"012-3456789\"";
                return false;
            }

            return true;
        }

        public void OnPost() {
            user.name = Request.Form["name"];
            user.mail = Request.Form["mail"];
            user.phone = Request.Form["phone"];
            user.skillsets = Request.Form["skillsets"];
            user.hobby = Request.Form["hobby"];

            if(user.name.Length == 0 || user.mail.Length == 0 || user.phone.Length == 0 || user.skillsets.Length == 0 || user.hobby.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            if (!validate(user)) return;

            // save the new user info into the database
            try
            {
                String con = "Data Source=.;Initial Catalog=tempdb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    String sql = "Insert Into users (name, mail, phone, skillsets, hobby) values (@name, @mail, @phone, @skillsets, @hobby);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("name", user.name);
                        command.Parameters.AddWithValue("mail", user.mail);
                        command.Parameters.AddWithValue("phone", user.phone);
                        command.Parameters.AddWithValue("skillsets", user.skillsets);
                        command.Parameters.AddWithValue("hobby", user.hobby);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            user.name = "";
            user.mail = "";
            user.phone = "";
            user.skillsets = "";
            user.hobby = "";
            successMessage = "Successfully register user";

            Response.Redirect("/Users/Index");
        }
    }
}
