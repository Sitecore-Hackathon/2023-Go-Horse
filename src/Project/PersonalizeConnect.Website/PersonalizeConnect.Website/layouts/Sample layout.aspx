<%@ Page Language="c#" Inherits="System.Web.UI.Page" CodePage="65001" %>
<%@ OutputCache Location="None" VaryByParam="none" %>
<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
  <title>Welcome to Sitecore</title>
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
  <meta name="CODE_LANGUAGE" content="C#" />
  <meta name="vs_defaultClientScript" content="JavaScript" />
  <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
  <sc:PlatformFontStylesLink runat="server"/>
  <link href="/default.css" rel="stylesheet" />
  
  
	<script type="text/javascript">
		var _boxeverq = _boxeverq || [];
		var _boxever_settings = {
			client_key: 'psfu6uh05hsr9c34rptlr06dn864cqrx', 
			target: 'https://api.boxever.com/v1.3', 
            cookie_domain: 'xmcloud.local', // Optional
			pointOfSale: 'gohorse-2023', 
			web_flow_target: 'https://d35vb5cccm4xzp.cloudfront.net', 
			//web_flow_config: { async: false, defer: false },
			javascriptLibraryVersion: '1.4.9', 
		};
		// Import the Boxever library asynchronously 
		(function() {
			 var s = document.createElement("script");
			 s.type = "text/javascript";
			 s.async = true;  
			 s.src = "https://d1mj578wat5n4o.cloudfront.net/boxever-" + window._boxever_settings.javascriptLibraryVersion + ".min.js";
			 var x = document.getElementsByTagName("script")[0]; x.parentNode.insertBefore(s, x);
		})();		
    </script>  
	<script type="text/javascript" src="/js/personalizeconnect.js"></script>

 </head>
<body> 
  <form id="mainform" method="post" runat="server">
    <div id="MainPanel">
      <sc:placeholder key="main" runat="server" /> 
    </div>
  </form>
 </body>
</html>
