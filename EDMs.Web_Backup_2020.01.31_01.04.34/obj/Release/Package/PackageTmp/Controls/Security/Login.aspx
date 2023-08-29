<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EDMs.Web.Controls.Security.Login" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/login.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #login {
margin: 50px auto;
width: 300px;
}
.textboxUserName {
outline: none;
display: block;
width: 100%;
border: 1px solid #d9d9d9;
margin: 0 0 20px;
padding: 10px 15px;
box-sizing: border-box;
font-wieght: 400;
-webkit-transition: 0.3s ease;
transition: 0.3s ease;
}
.btnLogin {
background-color: #008dde;
border: none;
border-radius: 3px;
-moz-border-radius: 3px;
-webkit-border-radius: 3px;
color: #f4f4f4;
cursor: pointer;
font-family: 'Open Sans', Arial, Helvetica, sans-serif;
height: 50px;
text-transform: uppercase;
width: 300px;
-webkit-appearance:none;
}
form fieldset a {
color: #5a5656;
font-size: 10px;

}form fieldset {
 border: 0;
}
form fieldset a:hover { text-decoration: underline; }
.btn-round {
background-color: #5a5656;
border-radius: 50%;
-moz-border-radius: 50%;
-webkit-border-radius: 50%;
color: #f4f4f4;
display: block;
font-size: 12px;
height: 50px;
line-height: 50px;
margin: 30px 125px;
text-align: center;
text-transform: uppercase;
width: 50px;
}


    </style>
   
</head>
<body style="overflow:hidden;">
    <div id="login-wrapper" class="png_bg">
        <div id="login-top">
            <img src="../../Images/pecc2logo.png" style="height: 130px; padding-top: 0px !important; padding-left: 0px"/>
        </div>
        <div id="login-content" style="padding-top: 0px">
              
            <form id="frmLogin" runat="server">
                <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
            <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel2" />
                <div class="module form-module">
                      <div class="form">
                        <h1 style="color: #33b5e5;">DM/DC</h1>
                        <h2>Login to your account</h2>
                <fieldset>
                    

                        <p>
                            <asp:TextBox ID="txtUsername"  runat="server" CssClass="textboxUserName" onBlur="if(this.value=='')this.value='User name'" onFocus="if(this.value=='User name')this.value='' "></asp:TextBox>
                        </p>
                        <p>
                    
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="textboxUserName" onBlur="if(this.value=='')this.value='Password'" onFocus="if(this.value=='Password')this.value='' "></asp:TextBox>
                        </p>
                
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUsername" ErrorMessage="Please enter user name." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" ErrorMessage="Please enter password." />
                                <asp:Label ID="lblMessage" runat="server" CssClass="login-error"></asp:Label>
                            </div>
                        <p>
                            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btnLogin"  OnClick="btnLogin_Click" OnClientClick="GetInfor()"/>
                        </p>
                </fieldset>
        </div>
        </div>
    <asp:HiddenField runat="server" ID="Browser"/>
    <asp:HiddenField runat="server" ID="TimeZone"/>
    <asp:HiddenField runat="server" ID="LocalTime"/>
    <asp:HiddenField runat="server" ID="LocalDate"/>
    <asp:HiddenField runat="server" ID="Domain"/>
    <asp:HiddenField runat="server" ID="Hostname"/>
    <asp:HiddenField runat="server" ID="Memory"/>
    <asp:HiddenField runat="server" ID="Os"/>
    <asp:HiddenField runat="server" ID="IpAddress"/>
     <telerik:RadScriptBlock runat="server">
           <script src="../../Scripts/jquery-1.7.1.js" type="text/javascript"></script>  
          <script type="text/javascript">
                function pageLoad() {
                    GetInfor();
                }

                function GetInfor() {
                    var getbrowser = "";
                    var osystem = "";
                  
                    var offset = new Date().getTimezoneOffset(), o = Math.abs(offset);
                    offset = (offset < 0 ? "+" : "-") + ("00" + Math.floor(o / 60)).slice(-2) + ":" + ("00" + (o % 60)).slice(-2);
                    document.getElementById("<%= TimeZone.ClientID %>").value = offset;
                    var year = new Date().getFullYear();
                    var month = new Date().getMonth()+1;
                    var day = new Date().getDate();
                    var hh = new Date().getHours();
                    var m = new Date().getMinutes();
                    var s = new Date().getSeconds();
                    var ss = new Date().getMilliseconds();
                    document.getElementById("<%= LocalTime.ClientID %>").value = hh+ ":" +m+ ":" + s+ ":" +ss ;
                    document.getElementById("<%= LocalDate.ClientID %>").value = day + "/" + month + "/" + year ;
                    var unknown = '-';
                    var nVer = navigator.appVersion;
                    var nAgt = navigator.userAgent;
                    var browserName = navigator.appName;
                    var fullVersion = '' + parseFloat(navigator.appVersion);
                    var majorVersion = parseInt(navigator.appVersion, 10);
                    var nameOffset, verOffset, ix;

                    // In Opera, the true version is after "Opera" or after "Version"
                    if ((verOffset = nAgt.indexOf("Opera")) != -1) {
                        browserName = "Opera";
                        fullVersion = nAgt.substring(verOffset + 6);
                        if ((verOffset = nAgt.indexOf("Version")) != -1)
                            fullVersion = nAgt.substring(verOffset + 8);
                    }
                        // In MSIE, the true version is after "MSIE" in userAgent
                    else if ((verOffset = nAgt.indexOf("MSIE")) != -1) {
                        browserName = "Microsoft Internet Explorer";
                        fullVersion = nAgt.substring(verOffset + 5);
                    }
                    else if ((verOffset = nAgt.indexOf("coc_coc_browser")) != -1) {
                        browserName = "coc_coc";
                        fullVersion = nAgt.substring(verOffset + 16);
                    }
                        // In Chrome, the true version is after "Chrome" 
                    else if ((verOffset = nAgt.indexOf("Chrome")) != -1) {
                        browserName = "Chrome";
                        fullVersion = nAgt.substring(verOffset + 7);
                    }
                        // In Safari, the true version is after "Safari" or after "Version" 
                    else if ((verOffset = nAgt.indexOf("Safari")) != -1) {
                        browserName = "Safari";
                        fullVersion = nAgt.substring(verOffset + 7);
                        if ((verOffset = nAgt.indexOf("Version")) != -1)
                            fullVersion = nAgt.substring(verOffset + 8);
                    }
                        // In Firefox, the true version is after "Firefox" 
                    else if ((verOffset = nAgt.indexOf("Firefox")) != -1) {
                        browserName = "Firefox";
                        fullVersion = nAgt.substring(verOffset + 8);
                    }
                        // In most other browsers, "name/version" is at the end of userAgent 
                    else if ((nameOffset = nAgt.lastIndexOf(' ') + 1) < (verOffset = nAgt.lastIndexOf('/'))) {
                        browserName = nAgt.substring(nameOffset, verOffset);
                        fullVersion = nAgt.substring(verOffset + 1);
                        if (browserName.toLowerCase() == browserName.toUpperCase()) {
                            browserName = navigator.appName;
                        }
                    }
                    // trim the fullVersion string at semicolon/space if present
                    if ((ix = fullVersion.indexOf(";")) != -1)
                        fullVersion = fullVersion.substring(0, ix);
                    if ((ix = fullVersion.indexOf(" ")) != -1)
                        fullVersion = fullVersion.substring(0, ix);

                    majorVersion = parseInt('' + fullVersion, 10);
                    if (isNaN(majorVersion)) {
                        fullVersion = '' + parseFloat(navigator.appVersion);
                        majorVersion = parseInt(navigator.appVersion, 10);
                    }
                    getbrowser = browserName + " (" + fullVersion + ")";


                    var os = unknown;
                    var clientStrings = [
                        { s: 'Windows 10', r: /(Windows 10.0|Windows NT 10.0)/ },
                        { s: 'Windows 8.1', r: /(Windows 8.1|Windows NT 6.3)/ },
                        { s: 'Windows 8', r: /(Windows 8|Windows NT 6.2)/ },
                        { s: 'Windows 7', r: /(Windows 7|Windows NT 6.1)/ },
                        { s: 'Windows Vista', r: /Windows NT 6.0/ },
                        { s: 'Windows Server 2003', r: /Windows NT 5.2/ },
                        { s: 'Windows XP', r: /(Windows NT 5.1|Windows XP)/ },
                        { s: 'Windows 2000', r: /(Windows NT 5.0|Windows 2000)/ },
                        { s: 'Windows ME', r: /(Win 9x 4.90|Windows ME)/ },
                        { s: 'Windows 98', r: /(Windows 98|Win98)/ },
                        { s: 'Windows 95', r: /(Windows 95|Win95|Windows_95)/ },
                        { s: 'Windows NT 4.0', r: /(Windows NT 4.0|WinNT4.0|WinNT|Windows NT)/ },
                        { s: 'Windows CE', r: /Windows CE/ },
                        { s: 'Windows 3.11', r: /Win16/ },
                        { s: 'Android', r: /Android/ },
                        { s: 'Open BSD', r: /OpenBSD/ },
                        { s: 'Sun OS', r: /SunOS/ },
                        { s: 'Linux', r: /(Linux|X11)/ },
                        { s: 'iOS', r: /(iPhone|iPad|iPod)/ },
                        { s: 'Mac OS X', r: /Mac OS X/ },
                        { s: 'Mac OS', r: /(MacPPC|MacIntel|Mac_PowerPC|Macintosh)/ },
                        { s: 'QNX', r: /QNX/ },
                        { s: 'UNIX', r: /UNIX/ },
                        { s: 'BeOS', r: /BeOS/ },
                        { s: 'OS/2', r: /OS\/2/ },
                        { s: 'Search Bot', r: /(nuhk|Googlebot|Yammybot|Openbot|Slurp|MSNBot|Ask Jeeves\/Teoma|ia_archiver)/ }
                    ];
                    for (var id in clientStrings) {
                        var cs = clientStrings[id];
                        if (cs.r.test(nAgt)) {
                            os = cs.s;
                            break;
                        }
                    }

                    var osVersion = unknown;

                    if (/Windows/.test(os)) {
                        osVersion = /Windows (.*)/.exec(os)[1];
                        os = 'Windows';
                    }

                    switch (os) {
                        case 'Mac OS X':
                            osVersion = /Mac OS X (10[\.\_\d]+)/.exec(nAgt)[1];
                            break;

                        case 'Android':
                            osVersion = /Android ([\.\_\d]+)/.exec(nAgt)[1];
                            break;

                        case 'iOS':
                            osVersion = /OS (\d+)_(\d+)_?(\d+)?/.exec(nVer);
                            osVersion = osVersion[1] + '.' + osVersion[2] + '.' + (osVersion[3] | 0);
                            break;
                    }

                    osystem = os + ' ' + osVersion;

                    document.getElementById("<%= Os.ClientID %>").value = osystem;
                    document.getElementById("<%= Browser.ClientID %>").value = getbrowser;
                   <%-- document.getElementById("<%= Hostname.ClientID %>").value = Environment.MachineNam;--%>
                    
                  
                    getIPAddress();
                    }

              var getIPAddress = function () {
                  $.getJSON("https://jsonip.com?callback=?", function (data) {
                      document.getElementById("<%= IpAddress.ClientID %>").value = data.ip;   
                  });
              };
            
                 </script>
        </telerik:RadScriptBlock>
            </form>
        </div>
    </div>
    <div id="dummy"></div>
</body>
</html>
