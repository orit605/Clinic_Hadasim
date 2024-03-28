<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="Clinic.home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>home</title>
    <link rel="stylesheet" href="basic.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1 name="head">קופת חולים</h1>
            <button type="submit" name="action" value="tryAdd">Add</button>
          
            <button type="submit" name="action" value="reset">Reset</button>
            <br />
            <div id="add_patient">
            <%=inputsAdd%>
            </div>
            <br />
            <table>
                <%=tableView%>
            </table>
        </div>
    </form>
</body>
</html>
