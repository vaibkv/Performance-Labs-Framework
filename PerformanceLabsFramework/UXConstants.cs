// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerformanceLabsFramework
{
    public class UXConstants
    {
        public const Int32 LengthBreak = 30;
        public const String StartingHTML = @"
<!DOCTYPE html>
<html lang='en'><head><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
    <meta charset='utf-8'>
    <title>Performance Report, NUnit Project</title>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
";

        public const String ScriptTag = "<script type='text/javascript'>";
        public const String ScriptTagClose = "</script>";
        public const String StyleTag = "<style type='text/css'>";
        public const String StyleTagClose = "</style>";
        public const String HeadTag = "<head>";
        public const String HeadTagClose = "</head>";
        public const String BodyTag = "<body>";
        public const String BodyTagClose = "</body>";
        public const String HtmlTag = "<html>";
        public const String HtmlTagClose = "</html>";
        public const String DivTagClose = "</div>";
        public const String Header2Tag = "<h2>";
        public const String Header2TagClose = "</h2>";
        public const String Header4Tag = "<h4>";
        public const String Header4TagClose = "</h4>";
        public const String ParaTag = "<p>";
        public const String ParaTagClose = "</p>";
        public const String TableTagStyled = "<table class='table table-bordered table-striped'>";
        public const String THeaderTag = "<thead>";
        public const String THeaderTagClose = "</thead>";
        public const String TBodyTag = "<tbody>";
        public const String TBodyTagClose = "</tbody>";
        public const String BreakTag = "<br>";
        public const String TableTag = "<table>";
        public const String TableTagClose = "</table>";
        public const String RowTag = "<tr>";
        public const String RowTagClose = "</tr>";
        public const String ColumnTag = "<td>";
        public const String ColumnTagClose = "</td>";
        public const String ColumnHeaderTag = "<th>";
        public const String ColumnHeaderTagClose = "</th>";
        public const String ColumnHeaderTagDisplayNone = "<th style='display:none'>";
        public const String ColumnTagDisplayNone = "<td style='display:none'>";
        public const String AnchorTagClose = "</a>";
        public const String AnchorTagHref = "<a href='#{0}' onclick='javascript:var x = document.getElementsByName(\"{0}\")[0];$(x).effect(\"highlight\", {color:\"#00FFFF\"}, 6000);'>";
        public const String AnchorTagLanding = "<a name='{0}'>{1}</a>";
        public const String AnchorTagModal = "<a data-mydata='{0}' style='cursor:pointer;' onclick='javascript:var div=document.getElementById(\"modalContent\");div.innerHTML = this.dataset.mydata;$(\"#myModal\").modal(\"show\");' >";
        public const String AnchorTagModalRegression = "<a data-mydata1='{0}' style='cursor:pointer;' onclick='javascript:var div=document.getElementById(\"modalContent1\");div.innerHTML = this.dataset.mydata1;$(\"#myModalRegression\").modal(\"show\");' >";
        public const String StyleFollowingBootstrp = @"
<style>
      body {
        padding-top: 60px;
      }
    </style>
";

        public const String DivModal = @"
<div class='modal hide' id='myModal'>
  <div class='modal-header'>
    <button type='button' class='close' data-dismiss='modal'>×</button>
    <h3>Complete Listing</h3>
  </div>
  <div class='modal-body' id='modalContent'>
    
  </div>
  <div class='modal-footer'>
    <a href='#' class='btn btn-primary' data-dismiss='modal'>Close</a>
  </div>
</div>
";

        public const String DivModalRegression = @"
<div class='modal hide' id='myModalRegression'>
  <div class='modal-header'>
    <button type='button' class='close' data-dismiss='modal'>×</button>
    <h3>Explanation For Regression</h3>
  </div>
  <div class='modal-body' id='modalContent1'>
    
  </div>
  <div class='modal-footer'>
    <a href='#' class='btn btn-primary' data-dismiss='modal'>Close</a>
  </div>
</div>
";

        public static String DivHeader = String.Format(@"
<div class='navbar navbar-fixed-top'>
      <div class='navbar-inner'>
        <div class='container'>
          <a class='brand' >Performance Report | {1}</a>
          <a class='brand' style='float:right;'>Start Date : {0}</a>
          <!--/.nav-collapse -->
        </div>
      </div>
    </div>
", DateTime.UtcNow.ToShortDateString(), PerformanceLabsConfigurations.ProductName);

        public const String DivTagContainer = @"
<div class='container'>
";

        public const String JQueryCDN = @"<script src='https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js'></script>";

        public const String JQueryUICssCDN = @"<link href='http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css' rel='stylesheet' type='text/css'/>";

        public const String JQueryUICDN = @"<script src='http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js'></script>";

        public const String EvaluationEnvironmentHeader = "Performance Evaluation Environment";
        public const String RegressionsDBOperations = "Db Operations Regressions";
        public const String RegressionsCodeOps = "Operation Regressions";
        public const String SlowCode = "Slow Operations";
        public const String SlowDbOps = "Slow Db Operations";
        public const String SqlParams = "Sql Parameters Information";
        public const String DbOpsDrillDown = "Db Operations Drilldown";
        public const String ExplanationForRegression = "Explanation For Regression";
        public const String RegressionConditionExplanation = "Regression Condition : High Z-Score OR Spike in Response time";
        public const String PreviousResponseTimes = "Previous Response Times";
        public const String CurrentResponseTime = "Current Response Time (ms) : ";
        public const String CurrentZScore = "Current Z-Score : ";
        public const String ZScoreThreshold = "Threshold Z-Score : ";
        public const String CurrentMultiplicationFactor = "Current Multiplication Factor(Current Response Time/Last Response Time) : ";
        public const String ThresholdMultiplicationFactor = "Threshold Multiplication Factor : ";
        public const String ThresholdTrivialServerCodeTime = "Operations with response times > {0} ms";
    }
}
