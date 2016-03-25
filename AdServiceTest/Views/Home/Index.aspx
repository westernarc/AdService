<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<!--
Objectives:
    Show 4 views of the data:
    Full list of all data that is sortable and paged.  Default sort is brand name, alphabetical
    List of ads on Cover position with 50%+ coverage.  Sortable, paged, sorted by brand name alphabetical
    Top 5 ads by coverage amount, distinct by brand.  Sort descending by coverage, then brand name alphabetical
    Top 5 brands by page coverage amount
-->
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Home</title>

    <link rel="stylesheet" type="text/css" href="/DataTables/datatables.css">

    <script type="text/javascript" src="../../Scripts/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" charset="utf8" src="//cdn.datatables.net/1.10.11/js/jquery.dataTables.js"></script>
</head>
<body>
    <table id="data">
        <thead>
            <tr>
                <th>AdId</th>
                <th>BrandId</th>
                <th>BrandName</th>
                <th>NumPages</th>
                <th>Position</th>
            </tr>
        </thead>
    </table>
    <div id="test">
    </div>
    <script>
        $(document).ready(function () {
            var allAdUrl = "/Home/GetAllAdData";

            $.get(allAdUrl, null, function (data) {
                for (var i = 0; i < data[0].length; i++) {
                    $("#test").append(data[0][i][0]);
                }

                var adData = jQuery.parseJSON(data);
                $("#data").DataTable({
                    data: adData,
                    columns: [
                        { data: "AdId" },
                        { data: "BrandId" },
                        { data: "BrandName" },
                        { data: "NumPages" },
                        { data: "Position" }
                    ]
                });
            })

        });

    </script>
</body>
</html>
