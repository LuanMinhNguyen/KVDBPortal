<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PdfControl.ascx.cs" Inherits="WebAppPdf.PdfControl" %>
<script type="text/javascript" src="WebViewer/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="WebViewer/lib/WebViewer.min.js"></script>
	<script type="text/javascript">
	    $(function () {
	        var viewerElement = document.getElementById('viewer');

	        function initStreaming() {
	            myWebViewer = new PDFTron.WebViewer({
	                path: "WebViewer/lib",
	                initialDoc: "ConvertAndStream.aspx?file=HĐ_PD.pdf",
	                showLocalFilePicker: true,
	                enableAnnotations: true,
	                streaming: true
	            }, viewerElement);
	        }

	        function initPreconverted() {
	            myWebViewer = new PDFTron.WebViewer({
	                type: "html5",
	                path: "WebViewer/lib",
	                initialDoc: "<%=WebAppPdf.EditPDFControlPara.PDFFilePath%>",
	                documentType: "pdf",
	                showLocalFilePicker: true,
	                enableAnnotations: true
	            }, viewerElement);
	        }

	        function radioValueChanged() {
	            $(viewerElement).empty();
	        
	                initPreconverted();
	           
	        }

	       // $("input[name='RadioGroup']").change(radioValueChanged);
	        initPreconverted();
	    });
	</script>
 

 <div id="viewer" style="height:1000px;overflow: hidden;"></div>

