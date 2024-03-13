using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace freelancer.Pages.Users
{
    public class IndexModel : PageModel
    {
        public List<UserInfo> listUser = new List<UserInfo>();
        public void OnGet()
        {
            try
            {
                String con = "Data Source=.;Initial Catalog=tempdb;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();
                    String sql = "Select * from users";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                UserInfo user = new UserInfo();
                                user.id = "" + reader.GetInt32(0);
                                user.name = reader.GetString(1);
                                user.mail = reader.GetString(2);
                                user.phone = reader.GetString(3);
                                user.skillsets = reader.GetString(4);
                                user.hobby = reader.GetString(5);
                                //user.udt = reader.GetDateTime(6).ToString();

                                listUser.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class UserInfo
    {
        public String id;
        public String name;
        public String mail;
        public String phone;
        public String skillsets;
        public String hobby;
        //public String udt;
    }
}
