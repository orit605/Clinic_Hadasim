using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public static class Helper
{
    private static string GetConnectionString()
    {
        // Retrieves connection string from Web.config file.
        return "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\me\\Downloads\\ClinicZIP\\ClinicZIP\\Clinic\\Clinic\\App_Data\\DB_Clinic.mdf; Integrated Security = True; Connect Timeout = 30";
    }
    public static DataTable ExecuteDataTable(string sql, SqlParameter[] parameters = null)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    public static void ExecuteNonQuery(string sql, SqlParameter[] parameters = null)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static DataTable ExecuteSelectCommand(string sql, SqlParameter[] parameters = null)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
    public static object ExecuteScalar(string sql, SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteScalar();
            }
        }
    }
    public static bool ExecuteExistCommand(string sql, SqlParameter[] parameters = null)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.Read(); // Returns true if at least one row is found
                }
            }
        }
    }
    public static DataTable GetAllPatients()
    {
        string sqlSelect = "SELECT * FROM Patient";
        return ExecuteDataTable(sqlSelect);
    }
    public static DataRow GetPatientData(string patientId)
    {
        string sqlSelect = "SELECT * FROM Patient WHERE Id = @Id";
        DataTable dt = ExecuteDataTable(sqlSelect, new SqlParameter[] { new SqlParameter("@Id", patientId) });
        return dt.Rows.Count > 0 ? dt.Rows[0] : null;
    }

    public static DataRow GetVaccinatedData(string patientId)
    {
        string sqlSelect = "SELECT * FROM Vaccinated WHERE Id = @Id";
        SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@Id", patientId) };
        DataTable dt = ExecuteDataTable(sqlSelect, parameters);
        return dt.Rows.Count > 0 ? dt.Rows[0] : null;
    }

    public static DataRow GetCoronaPatientData(string patientId)
    {
        string sqlSelect = "SELECT * FROM Corona_Patients WHERE Id = @Id";
        SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@Id", patientId) };
        DataTable dt = ExecuteDataTable(sqlSelect, parameters);
        return dt.Rows.Count > 0 ? dt.Rows[0] : null;
    }
    public static void AddPatient(string name, string id, string address, string birthDate, string phone, string cellPhone, string dateOfIllness, string dateOfRecovery, string codeV, string dateV, string vNumber)
    {
          // Insert into Patient table as before
            string sqlPatient = "INSERT INTO Patient (Name, Id, Address, BirthDate, Phone, CellPhone) VALUES (@Name, @Id, @Address, @BirthDate, @Phone, @CellPhone)";
            SqlParameter[] patientParameters = {
        new SqlParameter("@Name",name),
        new SqlParameter("@Id", id),
        new SqlParameter("@Address", address),
        new SqlParameter("@BirthDate",birthDate),
        new SqlParameter("@Phone",phone),
        new SqlParameter("@CellPhone", cellPhone),
    };
            Helper.ExecuteNonQuery(sqlPatient, patientParameters);

            // Conditional insert into Corona_Patients
            if (!string.IsNullOrEmpty(dateOfIllness) && !string.IsNullOrEmpty(dateOfRecovery))
            {
                string sqlCorona = "INSERT INTO Corona_Patients (Id, Date_Of_Illness, Date_Of_Recovery) VALUES (@Id, @DateOfIllness, @DateOfRecovery)";
                SqlParameter[] coronaParameters = {
            new SqlParameter("@Id", id),
            new SqlParameter("@DateOfIllness", dateOfIllness),
            new SqlParameter("@DateOfRecovery", dateOfRecovery),
        };
                Helper.ExecuteNonQuery(sqlCorona, coronaParameters);
            }

            // Conditional insert into Vaccinated
            if (!string.IsNullOrEmpty(codeV) && !string.IsNullOrEmpty(dateV) && !string.IsNullOrEmpty(vNumber))
            {
                string sqlVaccinated = "INSERT INTO Vaccinated (Id, Code_V, Date_V, V_Number) VALUES (@Id, @CodeV, @DateV, @VNumber)";
                SqlParameter[] vaccinatedParameters = {
            new SqlParameter("@Id", id),
            new SqlParameter("@CodeV", codeV),
            new SqlParameter("@DateV", dateV),
            new SqlParameter("@VNumber", vNumber),
        };
                Helper.ExecuteNonQuery(sqlVaccinated, vaccinatedParameters);
            }
    }

    public static void UpdatePatient(string id, string name, string address, string birthDate, string phone, string cellPhone, string dateOfIllness, string dateOfRecovery, string codeV, string dateV, string vNumber)
    {
        // Update the Patient table
        string sqlPatient = "UPDATE Patient SET Name = @Name, Address = @Address, BirthDate = @BirthDate, Phone = @Phone, CellPhone = @CellPhone WHERE Id = @Id";
        SqlParameter[] patientParameters = {
            new SqlParameter("@Name", name),
            new SqlParameter("@Id", id),
            new SqlParameter("@Address", address),
            new SqlParameter("@BirthDate", birthDate),
            new SqlParameter("@Phone", phone),
            new SqlParameter("@CellPhone", cellPhone),
        };
        ExecuteNonQuery(sqlPatient, patientParameters);

        // Update or Insert into Corona_Patients
        UpdateOrInsertCoronaPatients(id, dateOfIllness, dateOfRecovery);

        // Update or Insert into Vaccinated
        UpdateOrInsertVaccinated(id, codeV, dateV, vNumber);
    }

    private static void UpdateOrInsertCoronaPatients(string patientId, string dateOfIllness, string dateOfRecovery)
    {
        // First, check if a record exists for this patient
        string checkSql = "SELECT COUNT(*) FROM Corona_Patients WHERE Id = @Id";
        int count = (int)ExecuteScalar(checkSql, new SqlParameter[] { new SqlParameter("@Id", patientId) });

        if (count > 0)
        {
            // A record exists, so update it
            string updateSql = "UPDATE Corona_Patients SET Date_Of_Illness = @DateOfIllness, Date_Of_Recovery = @DateOfRecovery WHERE Id = @Id";
            SqlParameter[] updateParams = new SqlParameter[] {
            new SqlParameter("@Id", patientId),
            new SqlParameter("@DateOfIllness", dateOfIllness),
            new SqlParameter("@DateOfRecovery", dateOfRecovery),
        };
            ExecuteNonQuery(updateSql, updateParams);
        }
        else
        {
            // No record exists, insert a new one
            string insertSql = "INSERT INTO Corona_Patients (Id, Date_Of_Illness, Date_Of_Recovery) VALUES (@Id, @DateOfIllness, @DateOfRecovery)";
            SqlParameter[] insertParams = new SqlParameter[] {
            new SqlParameter("@Id", patientId),
            new SqlParameter("@DateOfIllness", dateOfIllness),
            new SqlParameter("@DateOfRecovery", dateOfRecovery),
        };
            ExecuteNonQuery(insertSql, insertParams);
        }
    }

    private static void UpdateOrInsertVaccinated(string patientId, string codeV, string dateV, string vNumber)
    {
        // Check if a record exists for this patient
        string checkSql = "SELECT COUNT(*) FROM Vaccinated WHERE Id = @Id";
        int count = (int)ExecuteScalar(checkSql, new SqlParameter[] { new SqlParameter("@Id", patientId) });

        if (count > 0)
        {
            // A record exists, so update it
            string updateSql = "UPDATE Vaccinated SET Code_V = @CodeV, Date_V = @DateV, V_Number = @VNumber WHERE Id = @Id";
            SqlParameter[] updateParams = new SqlParameter[] {
            new SqlParameter("@Id", patientId),
            new SqlParameter("@CodeV", codeV),
            new SqlParameter("@DateV", dateV),
            new SqlParameter("@VNumber", vNumber),
        };
            ExecuteNonQuery(updateSql, updateParams);
        }
        else
        {
            // No record exists, so insert a new one
            string insertSql = "INSERT INTO Vaccinated (Id, Code_V, Date_V, V_Number) VALUES (@Id, @CodeV, @DateV, @VNumber)";
            SqlParameter[] insertParams = new SqlParameter[] {
            new SqlParameter("@Id", patientId),
            new SqlParameter("@CodeV", codeV),
            new SqlParameter("@DateV", dateV),
            new SqlParameter("@VNumber", vNumber),
        };
            ExecuteNonQuery(insertSql, insertParams);
        }
    }

    public static void DeletePatient(string id)
    {
        // Delete related records from the Corona_Patients table
        string sqlDeleteCoronaPatients = "DELETE FROM Corona_Patients WHERE Id = @Id";
        ExecuteNonQuery(sqlDeleteCoronaPatients, new SqlParameter[] { new SqlParameter("@Id", id) });

        // Delete related records from the Vaccinated table
        string sqlDeleteVaccinated = "DELETE FROM Vaccinated WHERE Id = @Id";
        ExecuteNonQuery(sqlDeleteVaccinated, new SqlParameter[] { new SqlParameter("@Id", id) });

        // Delete the patient from the Patient table
        string sqlDeletePatient = "DELETE FROM Patient WHERE Id = @Id";
        ExecuteNonQuery(sqlDeletePatient, new SqlParameter[] { new SqlParameter("@Id", id) });
    }


}
