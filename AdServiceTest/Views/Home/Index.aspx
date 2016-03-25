<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Home</title>

    <script type="text/javascript" src="../../Scripts/jquery-1.8.2.min.js"></script>
</head>
<body>
    <div id="data">
        Test
    </div>

    vccv 
<script>
    $(document).ready(function () {
        var dateUrl = "/Home/GetData";
        $.get(dateUrl, null, function (data) {
            var adData = jQuery.parseJSON(data);
            $("#data").html(data);
        })
    });

</script>
</body>
</html>
