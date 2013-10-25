<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_UserNameRegister.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_UserNameRegister" %>

<script type="text/javascript">  

    Aqufit.Page.Controls.atiUserNameRegister = function (id) {
        this.id = id;
        this.arguments = null;
    }

    Aqufit.Page.Controls.atiUserNameRegister.prototype = {
        atiUserNameValidate: function (source, arguments) {
            $('#availableTrue').hide();
            $('#availableFalse').hide();
            $('#availablLoad').show();
            this.arguments = arguments;
            //_arguments = arguments;   
            var $lAvailability = $('#<%=lAvailabilityResult.ClientID %>');
            //   var lAvailability = document.getElementById("");
            $lAvailability.html('');
            var unameRegEx = /^([a-zA-Z0-9_\.\-])+$/;
            var str = arguments.Value;
            if (unameRegEx.test(str)) {
                $lAvailability.css('color', '#666666').html('Checking ... ');
                this.CheckUserName(arguments.Value); // perform the user name check
            }
        },
        atiGroupNameValidate: function(source, arguments) {
            $('#availableTrue').hide();
            $('#availableFalse').hide();
            $('#availablLoad').show();
            this.arguments = arguments;
            //_arguments = arguments;   
            var $lAvailability = $('#<%=lAvailabilityResult.ClientID %>');
            //   var lAvailability = document.getElementById("");
            $lAvailability.html('');
            var unameRegEx = /^([a-zA-Z0-9_\.\-])+$/;
            var str = arguments.Value;
            if (unameRegEx.test(str)) {
                $lAvailability.css('color', '#666666').html('Checking ... ');
                this.CheckUserName(arguments.Value); // perform the user name check
            }
        },
        CheckUserName: function (userName) {
            var $lAvailability = $('#<%=lAvailabilityResult.ClientID %>');
            if( Aqufit.Page.UserName == userName && userName != '' ){
                $('#availablLoad').hide();
                $lAvailability.css('color', 'orange').html('THIS IS YOU');
                this.arguments.IsValid = true;
            }else{
                var that = this;
                Affine.WebService.RegisterService.UserNameExists(userName, Aqufit.Page.PortalId, function (result) {
                    // true == the user already exists
                    // false == the user does not exist.
                    $('#availablLoad').hide();
                    if (result) {
                        $lAvailability.css('color', 'red').html('TAKEN');
                        $('#availableTrue').hide();
                        $('#availableFalse').show();
                        that.arguments.IsValid = false;
                    } else {
                        $lAvailability.css('color', 'green').html('AVAILABLE');
                        $('#availableFalse').hide();
                        $('#availableTrue').show();
                        that.arguments.IsValid = true;
                    }
                }, function (err) { });
            }
        }       
    };

    //Aqufit.UserNameValidate = function(source, arguments){
    //    Aqufit.Page.<%=this.ID %>.atiUserNameValidate(source, arguments);
    //}

    $(function () {       
        
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiUserNameRegister('<%=this.ID %>');   
        <%if( this.IsEditMode ){ %>        
            $('#<%=atiTxtUsername.ClientID %>').removeClass('dull');
        <% } %>               
    });       
    
</script>

<dl>
    <dt>Availability</dt>
    <dd><em><asp:Label ID="lAvailabilityResult" runat="server" Text="" /></em></dd>
    <dt><asp:Label id="plUserName" runat="server" controlname="atiTxtUsername" text="Username:" /></dt>        
    <dd>
        <asp:TextBox ID="atiTxtUsername" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" MaxLength="63" Text="eg: Popeye" />        
        <asp:Label ID="aitLabelUsername" runat="server" Visible="false" CssClass="UserNameStone" />
        <img id="availableTrue" src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png") %>" style="display: none;" />
        <img id="availableFalse" src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png") %>" style="display: none;" />	
        <img id="availablLoad" src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/loading.gif") %>" style="display: none;" />		 
        <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtUsername" ErrorMessage="User Name is required!" Text="*" /> 
        <asp:RegularExpressionValidator ID="revUserName" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtUsername" ValidationExpression="^([a-zA-Z0-9_\-])+" ErrorMessage="User name may only contain (letters, numbers, and '_-')" Text="User name may only contain (letters, numbers, and '_-')" />
        <asp:CustomValidator id="cvUserName" ControlToValidate="atiTxtUsername" ErrorMessage="User Name Taken" Text="*" runat="server"/>
        <asp:Literal ID="litHelp" runat="server" Visible="false" />
    </dd>
</dl>  