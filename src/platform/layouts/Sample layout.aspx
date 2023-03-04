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
            cookie_domain: 'xmcloudcm.local', // Optional
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
  
	<script>
	function GetDatasource(experienceId,experienceValue){
		var currentUrl = encodeURIComponent(window.location.href);
		var apiUrl = "/PersonalizeConnect?pageurl="+currentUrl+"&experienceId="+experienceId+"&experienceValue="+experienceValue;
		
		var xhttp = new XMLHttpRequest();
		xhttp.open("GET", apiUrl, false);
		xhttp.send();
		var result = JSON.parse(xhttp.responseText);
		
		return result;
	}
	
	function PopulateBlock(datasource){
		var containerId = datasource.container;		
		var container = document.querySelector('[cdp-container="'+containerId+'"]');
		if (container==undefined)
			return;
			
		Object.keys(datasource.fields).forEach(key => {
			var subcontainer = container.querySelector('[cdp-field="'+key+'"]');
			if (subcontainer!=undefined){
				var value = datasource.fields[key];
				
				if (subcontainer.tagName.toLowerCase()=="img"){
					if (value.src!="")
						subcontainer.src = value.src;
					if (value.alt!="")
						subcontainer.alt = value.alt;
				}
				else if (subcontainer.tagName.toLowerCase()=="a"){
				
					if (value.href!="")
						subcontainer.href = value.href;
					if (value.target!="")
						subcontainer.target = value.target;
					if (value.text!="")
						subcontainer.innerHTML = value.text;
					if (value.title!="")
						subcontainer.title = value.title;
					if (value.class!="")
						subcontainer.className = value.class;
				}
				else {
					subcontainer.innerHTML = value;				
				}				
			}
			
		});
	}	
	</script>
  
 </head>
<body> 
  <form id="mainform" method="post" runat="server">
    <div id="MainPanel">
      <sc:placeholder key="main" runat="server" /> 
    </div>
  </form>
 </body>
</html>
