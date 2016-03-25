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
    <title>Ad Service Data Viewer</title>
    <link rel="stylesheet" type="text/css" href="../../Content/Site.css" />

    <script type="text/javascript" src="../../Scripts/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui-1.8.24.min.js"></script>
    <script type="text/javascript" charset="utf8" src="../../Scripts/DataTables-1.10.11/media/js/jquery.dataTables.js"></script>

</head>
<body>
    <div id="sidetag">
        <div>
            Written by Adrian Micayabas
        </div>
    </div>
    <div id="content">
        <div id="header">WCF Ad Data Service</div>
        <div id="nav">
            <button id="btnAllAds">
                All Ads</button><button id="btnHighCoverAds">
                High Coverage Ads</button><button id="btnTop5Ads">
                Top 5 Ads</button><button id="btnTop5Brands">
                Top 5 Brands</button>
            <div id="filter">
                From
                <input type="text" id="datFrom" />
                To
                <input type="text" id="datTo" />
            </div>
        </div>
        <div id="sections">
            <div id="secAllAds">
                <div class="sectionHeader">All Ad Data</div>
                <table id="tblAllAds">
                    <thead>
                        <tr>
                            <th>Ad ID</th>
                            <th>Brand ID</th>
                            <th>Brand Name</th>
                            <th>Coverage</th>
                            <th>Position</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div id="secHighCoverAds">
                <div class="sectionHeader">Cover Ads with over 50% in Page Coverage</div>
                <table id="tblHighCoverAds">
                    <thead>
                        <tr>
                            <th>Ad ID</th>
                            <th>Brand ID</th>
                            <th>Brand Name</th>
                            <th>Coverage</th>
                            <th>Position</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div id="secTop5Ads">
                <div class="sectionHeader">Top 5 Ads (Grouped by Brand) in Page Coverage</div>
                <table id="tblTop5Ads">
                    <thead>
                        <tr>
                            <th>Ad ID</th>
                            <th>Brand ID</th>
                            <th>Brand Name</th>
                            <th>Coverage</th>
                            <th>Position</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div id="secTop5Brands">
                <div class="sectionHeader">Top 5 Brands in Page Coverage</div>
                <table id="tblTop5Brands">
                    <thead>
                        <tr>
                            <th style="width: 50%;">Brand Name</th>
                            <th>Coverage</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
    <script>
        function getAllAds() {
            $.get("/Home/GetAllAdData", { fromDate: $("#datFrom").val(), toDate: $("#datTo").val() }, function (data) {
                var adData = jQuery.parseJSON(data);
                setMode(0);
                $("#tblAllAds").DataTable({
                    destroy: true,
                    pageLength: 25,
                    data: adData,
                    scrollY: "500px",
                    columns: [
                    { data: "AdId", width: "10%" },
                    { data: "BrandId", width: "10%" },
                    { data: "BrandName", width: "60%" },
                    { data: "NumPages", width: "10%" },
                    { data: "Position", width: "10%" }
                    ],
                    order: [2, "asc"]
                });

            });
        }
        function getHighCoverAds() {
            $.get("/Home/GetHighCoverAdData", { fromDate: $("#datFrom").val(), toDate: $("#datTo").val() }, function (data) {
                var adData = jQuery.parseJSON(data);
                setMode(1);
                $("#tblHighCoverAds").DataTable({
                    destroy: true,
                    pageLength: 25,
                    data: adData,
                    scrollY: "500px",
                    columns: [
                    { data: "AdId", width: "10%" },
                    { data: "BrandId", width: "10%" },
                    { data: "BrandName", width: "60%" },
                    { data: "NumPages", width: "10%" },
                    { data: "Position", width: "10%" }
                    ],
                    order: [2, "asc"]
                });

            });
        }
        function getTop5Brands() {
            $.get("/Home/GetTop5BrandData", { fromDate: $("#datFrom").val(), toDate: $("#datTo").val() }, function (data) {
                var adData = jQuery.parseJSON(data);
                setMode(3);
                $("#tblTop5Brands").DataTable({
                    destroy: true,
                    pageLength: 5,
                    data: adData,
                    bFilter: false,
                    bInfo: false,
                    sDom: "t",
                    columns: [
                    { data: "BrandName", bSortable: false, width: "200px" },
                    { data: "NumPages", bSortable: false, width: "200px" },
                    ],
                    order: [[1, "desc"], [0, "asc"]]
                });

            });
        }
        function getTop5Ads() {
            $.get("/Home/GetTop5AdData", { fromDate: $("#datFrom").val(), toDate: $("#datTo").val() }, function (data) {
                var adData = jQuery.parseJSON(data);
                setMode(2);
                $("#tblTop5Ads").DataTable({
                    destroy: true,
                    pageLength: 5,
                    data: adData,
                    bFilter: false,
                    bInfo: false,
                    sDom: "t",
                    columns: [
                    { data: "AdId", width: "10%" },
                    { data: "BrandId", width: "10%" },
                    { data: "BrandName", width: "60%" },
                    { data: "NumPages", width: "10%" },
                    { data: "Position", width: "10%" }
                    ],
                    order: [[3, "desc"], [2, "asc"]]
                }); 1

            });
        }
        function setMode(mode) {
            switch (mode) {
                case -1:
                    $("#sections>[id^='sec']").hide();
                case 0:
                    $("#secTop5Brands").fadeOut(400);
                    $("#secTop5Ads").fadeOut(400);
                    $("#secHighCoverAds").fadeOut(400);
                    $("#secAllAds").fadeIn(400);
                    $("[id^='btn']").removeClass("active");
                    $("#btnAllAds").addClass("active");
                    break;
                case 1:
                    $("#secTop5Brands").fadeOut(400);
                    $("#secTop5Ads").fadeOut(400);
                    $("#secHighCoverAds").fadeIn(400);
                    $("#secAllAds").fadeOut(400);
                    $("[id^='btn']").removeClass("active");
                    $("#btnHighCoverAds").addClass("active");
                    break;
                case 2:
                    $("#secTop5Brands").fadeOut(400);
                    $("#secTop5Ads").fadeIn(400);
                    $("#secHighCoverAds").fadeOut(400);
                    $("#secAllAds").fadeOut(400);
                    $("[id^='btn']").removeClass("active");
                    $("#btnTop5Ads").addClass("active");
                    break;
                case 3:
                    $("#secTop5Brands").fadeIn(400);
                    $("#secTop5Ads").fadeOut(400);
                    $("#secHighCoverAds").fadeOut(400);
                    $("#secAllAds").fadeOut(400);
                    $("[id^='btn']").removeClass("active");
                    $("#btnTop5Brands").addClass("active");
                    break;
            }
        }
        function validateDatepickers() {
            var fromDate = new Date($("#datFrom").val());
            var toDate = new Date($("#datTo").val());
            if (fromDate >= toDate) {
                alert("From Date must be earlier than To Date");
                $("#datFrom").focus();
                return false;
            } else {
                return true;
            }
        }
        $(document).ready(function () {
            $("#datFrom").val("01/01/2011");
            $("#datTo").val("04/01/2011");
            $("#datFrom").datepicker();
            $("#datTo").datepicker();

            setMode(-1);
            getAllAds();

            $("#btnAllAds").click(function () {
                if (validateDatepickers()) { getAllAds(); }
            });
            $("#btnHighCoverAds").click(function () {
                if (validateDatepickers()) { getHighCoverAds(); }
            });
            $("#btnTop5Ads").click(function () {
                if (validateDatepickers()) { getTop5Ads(); }
            });
            $("#btnTop5Brands").click(function () {
                if (validateDatepickers()) { getTop5Brands(); }
            });
        });
    </script>
</body>
</html>
