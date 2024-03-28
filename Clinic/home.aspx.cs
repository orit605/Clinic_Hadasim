using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Clinic
{
    public partial class home : Page
    {
        protected string inputsAdd = "";
        protected string tableView = "";
        protected string message = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPatients();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ProcessRequestActions();
            LoadPatients(); // Refresh patient list to reflect any changes
        }

        private void ProcessRequestActions()
        {
            var action = Request.Form["action"];
            if (string.IsNullOrEmpty(action)) return;

            string id = action.Split('_').Length > 1 ? action.Split('_')[1] : string.Empty;

            switch (action)
            {
                case "tryAdd":
                    ShowAddFields();
                    break;
                case "submitAdd":
                    AddPatient();
                    break;
                case "submitUpdate":
                    UpdatePatient();
                    break;
                case "reset":
                    ResetForm();
                    break;
                default:
                    if (action.StartsWith("showDetails_"))
                        ShowPatientDetails(id);
                    else if (action.StartsWith("selectUpdate_"))
                        ShowUpdateFields(id);
                    else if (action.StartsWith("delete_"))
                        DeletePatient(id);
                    break;
            }
        }

        private void LoadPatients()
        {
            DataTable patients = Helper.GetAllPatients();

            // Define table headers
            tableView = "<table><tr><th>Name</th><th>ID</th><th>Address</th><th>Birth Date</th><th>Phone</th><th>Cell Phone</th><th>Actions</th></tr>";

            if (patients.Rows.Count > 0)
            {
                foreach (DataRow row in patients.Rows)
                {
                    tableView += FormatPatientRow(row);
                }
            }
            else
            {
                message = "No patients found.";
            }

            tableView += "</table>";
        }

        private string FormatPatientRow(DataRow row)
        {
            string detailButton = $"<button type='submit' name='action' value='showDetails_{row["Id"]}'>Show Details</button>";
            string updateButton = $"<button type='submit' name='action' value='selectUpdate_{row["Id"]}'>Update</button>";
            string deleteButton = $"<button type='submit' name='action' value='delete_{row["Id"]}'>Delete</button>";

            return $"<tr><td>{row["Name"]}</td><td>{row["Id"]}</td><td>{row["Address"]}</td><td>{row["BirthDate"]}</td><td>{row["Phone"]}</td><td>{row["CellPhone"]}</td><td>{updateButton} {deleteButton} {detailButton}</td></tr>";
        }

        private void ShowPatientDetails(string patientId)
        {
            // Reset previous inputs or messages
            inputsAdd = $"<h2>Patient Id: {patientId}</h2>";
            inputsAdd += FetchCoronaPatientDetails(patientId);
            inputsAdd += FetchVaccinatedDetails(patientId);
        }

        private string FetchCoronaPatientDetails(string patientId)
        {
            DataRow coronaRow =Helper.GetCoronaPatientData(patientId);
            if (coronaRow != null)
            {
                return $"<h3>Corona Patient Details:</h3><p>Date Of Illness: {coronaRow["Date_Of_Illness"]}<br/>Date Of Recovery: {coronaRow["Date_Of_Recovery"]}</p>";
            }

            return "<p>No Corona Patient details found.</p>";
        }

        private string FetchVaccinatedDetails(string patientId)
        {
            DataRow vaccinatedRow = Helper.GetVaccinatedData(patientId);
            if (vaccinatedRow != null)
            {
                return "<h3>Vaccination Details:</h3>" +
                       $"<p>Code: {vaccinatedRow["Code_V"]}, Date: {vaccinatedRow["Date_V"]}, Number: {vaccinatedRow["V_Number"]}</p>";
            }

            return "<p>No Vaccination details found.</p>";
        }
        
        private void ShowUpdateFields(string id)
        {
            // Assume helper methods GetPatientData, GetCoronaPatientData, and GetVaccinatedData exist
            DataRow patientRow =Helper.GetPatientData(id);
            DataRow coronaRow =Helper.GetCoronaPatientData(id);
            DataRow vaccinatedRow = Helper.GetVaccinatedData(id);

                inputsAdd = $"<input type='hidden' name='updateId' value='{id}'/>" +
                             $"<label>Name:</label><input type='text' name='updateName' value='{patientRow["Name"]}'/><br/>" +
                             $"<label>Address:</label><input type='text' name='updateAddress' value='{patientRow["Address"]}'/><br/>" +
                             $"<label>Birth Date:</label><input type='date' name='updateBirthDate' value='{patientRow["BirthDate"].ToString().Split(' ')[0]}'/><br/>" +
                             $"<label>Phone:</label><input type='text' name='updatePhone' value='{patientRow["Phone"]}'/><br/>" +
                             $"<label>Cell Phone:</label><input type='text' name='updateCellPhone' value='{patientRow["CellPhone"]}'/><br/>";
                inputsAdd += "<h3>Corona Details:</h3>" +
                 $"<label>Date of Illness:</label><input type='date' name='coronaDateOfIllness' value='{coronaRow?["Date_Of_Illness"]}'/><br/>" +
                 $"<label>Date of Recovery:</label><input type='date' name='coronaDateOfRecovery' value='{coronaRow?["Date_Of_Recovery"]}'/><br/>";
                inputsAdd += "<h3>Vaccination Details:</h3>" +
                     $"<label>Code:</label><input type='text' name='vaccineCode' value='{vaccinatedRow?["Code_V"]}'/><br/>" +
                     $"<label>Date:</label><input type='date' name='vaccineDate' value='{vaccinatedRow?["Date_V"]}'/><br/>" +
                     $"<label>Number:</label><input type='number' name='vaccineNumber' value='{vaccinatedRow?["V_Number"]}'/><br/>";
                inputsAdd += "<input type='submit' name='action' value='submitUpdate'/>";

        }
       

       
        
        private void AddPatient()
        {
            Helper.AddPatient(Request.Form["name"],
                Request.Form["id"],
                Request.Form["address"],
                Request.Form["birthDate"],
                Request.Form["phone"],
                Request.Form["cellPhone"],
                Request.Form["dateOfIllness"],
                 Request.Form["dateOfRecovery"],
                 Request.Form["codeV"],
                 Request.Form["dateV"],
                 Request.Form["vNumber"]


                );
        }


        private void UpdatePatient()
        {
            // Retrieve form data
            string id = Request.Form["updateId"];
            string name = Request.Form["updateName"];
            string address = Request.Form["updateAddress"];
            string birthDate = Request.Form["updateBirthDate"];
            string phone = Request.Form["updatePhone"];
            string cellPhone = Request.Form["updateCellPhone"];
            string dateOfIllness = Request.Form["coronaDateOfIllness"];
            string dateOfRecovery = Request.Form["coronaDateOfRecovery"];
            string codeV = Request.Form["vaccineCode"];
            string dateV = Request.Form["vaccineDate"];
            string vNumber = Request.Form["vaccineNumber"];

            // Call the updated method in the Helper class
            Helper.UpdatePatient(id, name, address, birthDate, phone, cellPhone, dateOfIllness, dateOfRecovery, codeV, dateV, vNumber);
        }


        private void DeletePatient(string id)
        {
            Helper.DeletePatient(id);
        }
        private void ResetForm()
        {
            // Reset form fields or any state as necessary
        }

        private void ShowAddFields()
        {
            inputsAdd = "<label>Name:</label><input type='text' name='name' /><br/>" +
                        "<label>ID:</label><input type='text' name='id' /><br/>" +
                        "<label>Address:</label><input type='text' name='address' /><br/>" +
                        "<label>Birth Date:</label><input type='date' name='birthDate' /><br/>" +
                        "<label>Phone:</label><input type='text' name='phone' /><br/>" +
                        "<label>Cell Phone:</label><input type='text' name='cellPhone' /><br/>" +

               "<label>Date of Illness:</label><input type='date' name='dateOfIllness' /><br/>" +
               "<label>Date of Recovery:</label><input type='date' name='dateOfRecovery' /><br/>" +
               "<label>Vaccine Code:</label><input type='text' name='codeV' /><br/>" +
               "<label>Vaccine Date:</label><input type='date' name='dateV' /><br/>" +
               "<label>Vaccine Number:</label><input type='number' name='vNumber' /><br/>" +
               "<input type='submit' name='action' value='submitAdd' />";
        }

        // Additional methods for update and delete operations, similar to AddPatient, would go here
    }
}


