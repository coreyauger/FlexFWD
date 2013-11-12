<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Feedback.ViewATI_Feedback" CodeFile="ViewATI_Feedback.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!-- TODO: make this modules configurable so we can use it in our other projects -->

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script type="text/javascript">
    var uservoiceJsHost = ("https:" == document.location.protocol) ? "https://uservoice.com" : "http://cdn.uservoice.com";
    document.write(unescape("%3Cscript src='" + uservoiceJsHost + "/javascripts/widgets/tab.js' type='text/javascript'%3E%3C/script%3E"))
</script>

<script type="text/javascript">
 /* See Single Sign-On Setup */
  //UserVoice.User.setToken('aLEm%2FdggP8gqCHETZU0EgzMbctX2mUFqjVI3YLqIklZOljMP9JyspwyVCNdpiUohQJwX%2BCA1%2BIQniC4%2BNuk9mxdLwWLcaVSaHHTLuReA%2BflRzMsnJEFSLCr4sY%2FfWbz4u03FvCEd5JmMl3aTphKBLiaMdgUw5b1RPHN3Zkv1LQoRH5jAXT6Uc00q%2Bk5Qm69w9ohq9BPK7i5QJdh%2BoE11eknomXmP9DGDQA1Yo6dIqGf3T279BZ6z7KEzfoSBkZoxNLUdmLSs8H8J8NJgpaAggh9RSeGLYgw07vvVxLCqZaoo9Mfupa7WnHIzxuCNwef3Uqrak7GLtApRsuzfCgwnUQ%3D%3D'); 

  <%if (Settings["ShowFeedback"] == null || Convert.ToBoolean( Settings["ShowFeedback"] )){ %>
    UserVoice.Tab.show({ 
      /* required */
      key: 'aqufit',
      host: 'aqufit.uservoice.com', 
      forum: '32012', 
      /* optional */
      alignment: 'right',
      background_color:'#e47526', 
      text_color: 'white',
      hover_color: '#000',
      lang: 'en'
    });        
  <%} %> /* optional */
      
</script>
</telerik:RadCodeBlock>
    

    

         
    
               



