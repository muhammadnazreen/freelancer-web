using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace freelancer.Pages.Users
{
    public class RegisterModel : PageModel
    {
        public UserInfo user = new UserInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
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
