using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Clinic
{

    public partial class home : System.Web.UI.Page
    {
        public string message;
        public string inputUpdate = "";
        public string inputDelete = "";
        public string inputsAdd = "";//הוספה
        public string tableView = "";//נתוני טבלה
        protected void Page_Load(object sender, EventArgs e)
        {
            string tableName = "Patient";//שם טבלת המטופלים בקופת החולים
            string dbName = "DB_Clinic.mdf";//שם מסד הנתונים
            
            string sqlSelect = "SELECT * FROM " + tableName;//שאילתה להצגת נתוני לקוחות
            DataTable table = Helper.ExecuteDataTable(dbName, sqlSelect);//ייצוא נתוני הטבלה לתוך המשתנה
            int length = table.Rows.Count;
            if (length == 0)
            {
                message = "לא נמצאו מטופלים";
            }
            else
            {
                tableView += "<tr><th>Name</th><th>Id</th><th>Address" +
                    "</th><th>Birth Date</th><th>Phone</th><th>Cell Phone</th></tr>";
                

                for(int i = 0; i < length; i++)
                {
                    tableView += "<tr><td>" + table.Rows[i]["Name"] + "</td><td>" + table.Rows[i]["Id"] + "</td><td>" + table.Rows[i]["Address"] + "</td><td>" + table.Rows[i]["Birth Date"] + "</td><td>"+table.Rows[i]["Phone"]+ "</td><td>"+ table.Rows[i]["Cell Phone"]+"</td></tr>";

                }
            }
            if (Request.Form["add"] != null)//אם כפתור הוספת מטופל
            {

                inputsAdd += "<input type ='text' name='namePat'/><label> Name </label><br/>" +
                    "<input type ='text' name='idPat'/><label> Id </label><br/>+" +
                    "<input type='date'/><label> Birth date </label><br/>"+
                    "<input type ='text'/><label> Phone </label><br/>"+
                    "<input type ='text'/><label> Cell Phone </label><br/>"+
                    "<input type ='submit' value='submit' name='submitButton'/>";              
                //שאילתת INSERT יש לתת שם לכל אחד מהאינפוטים
                string sqlAdd = "INSERT INTO " + tableName + "VALUES(" + Request.Form["namePat"] + ","+ Request.Form["namePat"]+","+")";
               // Helper.DoQuery(dbName, sqlAdd);
            }
            if (Request.Form["update"] != null)
            {
                inputUpdate += "<input type = 'text' name = 'idPat' />< label > Id </ label >< br />";
                string id = Request.Form["idPat"];
                message = "you want to update user with id: " + id;
                inputDelete += "<input type ='text' name='namePat'/><label> Name </label><br/>" +
                    "<input type ='text' name='idPat'/><label> Id </label><br/>+" +
                    "<input type='date'/><label> Birth date </label><br/>" +
                    "<input type ='text'/><label> Phone </label><br/>" +
                    "<input type ='text'/><label> Cell Phone </label><br/>" +
                    "<input type ='submit' value='submit'/>";
               string sqlUpdate="UPDATE "+tableName +"SET Name=" + Request.Form["namePat"]+"AND Id=" + Request.Form["namePat"]
                    //לסיים
                    +"where id="+id;
                //שליחה לעדכון המסד נתונים
              Helper.DoQuery(dbName,sqlUpdate);
                //מה יקרה כשנלחץ על הכפתור עדכון 
            }
            if (Request.Form["delete"] != null)
            {
                inputDelete += "<input type = 'text' name = 'idPat' />< label > Id </ label >< br />";
                string sqlDelete = "DELETE FROM " + tableName + "WHERE Id=" + Request.Form["idPat"];
               //שליחה למחיקה מהמסד נתונים
                Helper.DoQuery(dbName,sqlDelete);
                //מה יקרה כשנלחץ על הכפתור מחיקה
            }
            if (Request.Form["reset"]!=null)
            {
                inputUpdate = "";
                inputsAdd = "";
                inputDelete = "";
            }
        }
    }
}