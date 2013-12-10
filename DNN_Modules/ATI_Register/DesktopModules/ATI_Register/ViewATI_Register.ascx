<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Register.ViewATI_Register" CodeFile="ViewATI_Register.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="UserName" Src="~/DesktopModules/ATI_Base/controls/ATI_UserNameRegister.ascx" %>
<%@ Register TagPrefix="ati" TagName="Slim" Src="~/DesktopModules/ATI_Base/controls/ATI_SlimControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="Password" Src="~/DesktopModules/ATI_Base/controls/ATI_Password.ascx" %>
<%@ Register TagPrefix="ati" TagName="BodyComp" Src="~/DesktopModules/ATI_Base/controls/ATI_BodyComposition.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="WebLinks" Src="~/DesktopModules/ATI_Base/controls/ATI_WebLinks.ascx" %>
<%@ Register TagPrefix="ati" TagName="Address" Src="~/DesktopModules/ATI_Base/controls/ATI_AddressControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="WorkoutTypes" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutTypes.ascx" %>
<%@ Register TagPrefix="ati" TagName="ThemeEditor" Src="~/DesktopModules/ATI_Base/controls/ATI_ThemeEditor.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<style type="text/css">

#atiRegisterPanel .ui-widget input.atiTxtBox, .ui-widget textarea.atiTxtArea, .ui-widget  input.atiTxtBoxSmall, .ui-widget select.atiTxtBox
{
	width: 200px;	
}

.formEdit
{
	float: right;
	width: 700px;
}

dt
{
	padding-top: 6px;
}

div.innerFlat
{
	background-color: #FFF !important; 
}
div.atiRegister
{
	border: 1px solid #ccc;
	padding: 10px 20px;
}
div.atiRegister h3
{
	text-decoration: underline;
}

div.atiRegisterFB
{
	width: 650px;
	border: 1px solid #ccc;
	padding: 10px 20px;
	margin: 40px auto;
}
.dull
{
	color: #ccc !important;
}


</style>

<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>

    <script type="text/javascript" >
        Affine.ServiceResonse = {       
            OnResponseEnd: function (sender, args){
                $('#atiStatusWidget').fadeIn('slow'); 
                $('#<%=bUpdate.ClientID %>').button( { icons: { primary: 'ui-icon-disk'} }  );
                setTimeout(function(){ $('#atiStatusWidget').fadeOut('slow');   } , Aqufit.Const.TIMEOUT );                 
            },        
            OnRequestStart: function(sender, args){                
                $('#atiStatusWidget').hide();        
            }                     
        };

        Aqufit.Page.Tabs = {
            SwitchTab: function (ind) {
                $('#tabs').tabs('select', ind);
            }       
        };        

        Aqufit.Windows.ErrorDialog = {
            win: null,
            open: function (json) {
                var oJson = eval('(' + json + ')');
                $('#errorStatusDiv').empty().html(oJson['html']);
                Aqufit.Windows.ErrorDialog.win = $find('<%=ErrorDialog.ClientID %>');
                Aqufit.Windows.ErrorDialog.win.show();
            },
            close: function () {
                Aqufit.Windows.ErrorDialog.win.close();
            }
        };

        Aqufit.Windows.SuccessDialog = {
            win: null,
            open: function (json) {
                var oJson = eval('(' + json + ')');
                $('#successStatusDiv').empty().html(oJson['html']);
                Aqufit.Windows.SuccessDialog.win = $find('<%=SuccessDialog.ClientID %>');
                Aqufit.Windows.SuccessDialog.win.show();
            },
            close: function () {
                Aqufit.Windows.SuccessDialog.win.close();
            }
        }
        
        
        $(function () {
            <%if(this.UserId > 0){ %>   // We are editing so no fancy "dull" help
                var passHtml = $('#passwordHack').children().remove();
                $('#pageViewPassword').append(passHtml);
                $('.dull').removeClass('dull');    
                //$('#<%=bUpdatePass.ClientID %>').button( { icons: { primary: 'ui-icon-key'} } );      
                $('#<%=bUpdate.ClientID %>').button( { icons: { primary: 'ui-icon-disk'} }  );   
                $('fieldset > div:first').addClass('profilePic')                                         
                                         .next('div')
                                         .addClass('formEdit');
            <%}else{ %>
                $('.dull').focus(function () {
                    if ($(this).hasClass('dull')) {
                        $(this).removeClass('dull').val('');
                    }
                });
            <%} %>
            $('#<%=bUpdatePass.ClientID %>').button();
            $('#bSuccessDialogClose').button().click(function(event){
                Aqufit.Windows.SuccessDialog.close();
                event.stopPropagation();
                return false;
            }); 
           $('#atiStatusWidget').hide();
           $('#dnn_ctr420_Login_pnlLogin').remove();
            // IE - screws up button rendering (they look like crap anyway in IE)
            if (navigator.appName.toLowerCase().indexOf("internet explorer") == -1) {
                $('#<%=bRegister.ClientID %>').button();   
                $('#<%=bUserSetup.ClientID %>').button();   
                $('#<%=bLogin.ClientID %>').button();      
            }
            $('#tabs').tabs().bind('tabsselect', function(event, ui) {

                // Objects available in the function context:
                //ui.tab     // anchor element of the selected (clicked) tab
                //ui.panel   // element, that contains the selected/clicked tab contents
                //ui.index   // zero-based index of the selected (clicked) tab
                if(ui.index == 1){  // the password change tab ..
                    $('#<%=bUpdate.ClientID %>').hide();
                }else{
                    $('#<%=bUpdate.ClientID %>').show();
                }

            });                 
            
     
        });
      
    </script>            
</telerik:RadCodeBlock>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="Affine.ServiceResonse.OnRequestStart" ClientEvents-OnResponseEnd="Affine.ServiceResonse.OnResponseEnd">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="bRegister">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="RadCaptcha1"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="ValidationSummary1"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="bRegister" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting> 
        <telerik:AjaxSetting AjaxControlID="bUpdatePass">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="bUpdatePass" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>   
        <telerik:AjaxSetting AjaxControlID="bUpdate">
        <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="bUpdate" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>  
        <telerik:AjaxSetting AjaxControlID="bUserSetup">
        <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="bUserSetup" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting> 
        <telerik:AjaxSetting AjaxControlID="bLogin">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>               
            </UpdatedControls>
        </telerik:AjaxSetting>     
        <telerik:AjaxSetting AjaxControlID="bCreateGroup">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="atiGroupRegistrationPanel" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>               
            </UpdatedControls>
        </telerik:AjaxSetting>            
    </AjaxSettings>        
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />   

<telerik:RadWindow ID="ErrorDialog" runat="server" Skin="Black" Title="Error" Width="450" Height="250" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
        <ContentTemplate>
            <div style="width: 100%; height: 100%; background-color: white; position: relative; overflow: hidden;">
               <div style="position: absolute; top: 50px; left: 40px; color: Black; font-size: 12px;">                   
                   <h3 style="color: Red; font-size: 16px;">                   
                       <img id="imgError" runat="server" style="float: left; clear: left; padding-right: 10px;" />
                       We have encountered a problem.
                   </h3>  
                   <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="groups" />         
                   <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="aqufit" />
                   <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="updatepassword" />                         
                   <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="oauth" />      
                   <div id="errorStatusDiv" style="padding: 10px;">Unknown</div>     
               </div>       
            </div>            
        </ContentTemplate>
</telerik:RadWindow>

<telerik:RadWindow ID="SuccessDialog" runat="server" Skin="Black" Title="Success" Width="450" Height="200" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
        <ContentTemplate>
            <div style="width: 100%; height: 100%; background-color: white; position: relative; overflow: hidden;">
               <div style="position: absolute; top: 25px; left: 40px; color: Black; font-size: 12px; width: 100%;">                   
                   <img id="imgCheck" runat="server" style="float: left; clear: left; padding-right: 10px;" />                       
                   <div id="successStatusDiv" style="padding: 10px; width: 100%;"></div>  
                   <button id="bSuccessDialogClose">Close</button>   
               </div>       
            </div>            
        </ContentTemplate>
</telerik:RadWindow>

<div id="atiStatusWidget" class="ui-widget">
	<div class="ui-state-highlight ui-corner-all" style="margin-bottom: 20px; padding: 18px;"> 
		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		<asp:Literal ID="litStatus" runat="server" />
	</div>
</div>
<asp:Panel ID="atiRegistrationPanel" runat="server">



<div id="atiRegisterPanel" style="display: block;" class="ui-widget">  
    <div id="tabs">
        <ul>
            <li id="tabRegistration" runat="server"><a href="#pageViewRegistration"><asp:Literal ID="litRegistration" runat="server" Text="Register" /></a></li>
            <li id="tabPassword" runat="server" visible="false"><a href="#pageViewPassword">Password</a></li>
            <li id="tabAppearance" runat="server" visible="false"><a href="#pageViewAppearance">Appearance</a></li>
            <li id="tabWorkoutSettings" runat="server" visible="false"><a href="#pageViewWorkoutSettings">Athlete Information</a></li>
        </ul>
        <div id="pageViewRegistration" class="innerFlat">
            <asp:Panel ID="panelBeta" runat="server" Visible="false">
              <h2>** IMPORTANT.  Site is in BETA mode.  This means you will only be allowed to register if you are on the beta list.</h2>
              <p>If you would like to get on the list please <a href="/ContactUs" style="color: #f9a01b;">Contact Us</a>.</p>
            </asp:Panel>
            <div style="float: left; width: 370px;" id="atiLeftPanel" runat="server">            
                <div class="grad-FFF-EEE rounded atiRegister">
                <h3>Register using FaceBook</h3>            
                <center>
                    <img id="imgfbConnect" runat="server" style="border: 1px solid #ccc; margin-bottom: 20px;" />
                    <asp:Literal id="litFBConnect" runat="server" />    
                    <br /><br />
                </center>                  
                <h1 style="position: absolute; top: 150px; left: 400px; width: 60px;">- OR -</h1>
                </div>
            </div>
            <div runat="server" visible="false" id="panelProfilePic" style="float: left; width: 120px;" >
                <ati:ProfileImage ID="atiProfileImage" runat="server" Width="192px" />   
            </div>
            <div style="float: right; width: 450px;" id="atiRightPanel" runat="server">
                <div class="grad-FFF-EEE rounded atiRegister">
                <fieldset>                         
                    <h3 runat="server" id="h2Register">Create a New Account</h3>          
                    <ati:UserName Id="atiUserName" runat="server" ValidationGroupName="aqufit" />
                    <asp:Literal ID="litUserName" runat="server" Visible="false" />
                    <ati:Slim ID="atiSlimControl" runat="server" ValidationGroupName="aqufit" />        
                    <ati:BodyComp id="atiBodyComp" runat="server" ValidationGroupName="aqufit" FitnessLevelVisible="false" WeightVisible="false" HeightVisible="false" />
                    <div id="passwordHack">
                        <ati:Password ID="atiPassword" runat="server" ValidationGroupName="aqufit" />
                        <asp:Button id="bUpdatePass" runat="server" Text="Update Password" OnClick="bUpdatePass_Click" Visible="false" ValidationGroup="updatepassword"  /> 
                    </div>
                    

                    <telerik:RadCaptcha  ID="RadCaptcha1" runat="server" ErrorMessage="The captcha code you entered is not valid." ValidationGroup="aqufit" CaptchaImage-TextChars="Numbers" CaptchaTextBoxCssClass="ui-corner-all ui-widget-content atiTxtBox">
                            <CaptchaImage ImageCssClass="imageClass" />
                    </telerik:RadCaptcha>
                    <span class="instruction" id="spanTerms" runat="server">By clicking "Register" you are agreeing to the <a class="username" href="/terms">terms of use</a>.</span><br />
                    <asp:HiddenField ID="hiddenGroupKey" runat="server" Value="0" />
                    <asp:Button ID="bRegister" runat="server" Text="Register" OnClick="bRegister_Click" Visible="true" ValidationGroup="aqufit" />  
                    <asp:ValidationSummary ID="ValidationSummary6" CssClass="validateSumm" runat="server" ValidationGroup="aqufit" />                                
                </fieldset>    
                </div>
            </div>
            <br style="clear: both;" />
        </div>
        <div id="pageViewPassword" class="innerFlat">
            
        </div>
        <div id="pageViewAppearance" class="innerFlat">
            <ati:WebLinks id="atiWebLinks" runat="server" ValidationGroupName="aqufit" Visible="false" />
                    
            <ati:ThemeEditor id="atiThemeEditor" runat="server" Visible="false" />
            <asp:Button ID="bUpload" runat="server" Text="Upload Image" OnClick="bUpload_Click" Visible="false" />
        </div>
        <div id="pageViewWorkoutSettings" class="innerFlat">
            <asp:Panel ID="panelAthleteSettings" runat="server" Visible="false">
            <fieldset>
                <dl>
                    <dt>Home Group: </dt>
                    <dd><asp:DropDownList ID="ddlHomeGroup" runat="server" Width="400px" CssClass="ui-corner-all ui-widget-content atiTxtBox" /></dd>

                    <dt>Allow Home Group email notification:</dt>
                    <dd><asp:CheckBox ID="cbAllowGroupEmails" runat="server" Checked="true" /></dd>

                    <dt>I want to log: </dt>
                    <dd><ati:WorkoutTypes ID="atiWorkoutTypes" runat="server" /></dd>

                    <ati:BodyComp ID="atiHeightWeight" runat="server" GenderVisible="false" BirthDateVisible="false" FitnessLevelVisible="false" />

                    <dt>Bio (1024 characters max)</dt>
                    <dd><asp:TextBox ID="txtBio" runat="server" MaxLength="1024" Width="400px" Height="100px" TextMode="MultiLine" CssClass="ui-corner-all ui-widget-content atiTxtBox" /></dd>

                    <dt>How long have you been training? (512 characters max)</dt>
                    <dd><asp:TextBox ID="txtTraining" runat="server" MaxLength="512" Width="400px" Height="100px" TextMode="MultiLine" CssClass="ui-corner-all ui-widget-content atiTxtBox" /></dd>
                    
                </dl>
            </fieldset>
            </asp:Panel>
        </div>
        <asp:ValidationSummary ID="ValidationSummary5" CssClass="validateSumm" runat="server" ValidationGroup="aqufit" />
        <asp:Button id="bUpdate" runat="server" Text="Update" OnClick="bUpdate_Click" Visible="false" ValidationGroup="aqufit"  />  
        
    </div>
</div>       
</asp:Panel>     
<asp:Panel ID="atiRpxRecievePanel" CssClass="atiRpxUserNameSetup" runat="server" Visible="false">
    <div class="grad-FFF-EEE rounded atiRegisterFB">
        <h2>Almost Done.  You just need to select a UserName for the site.</h2>
        <asp:Literal ID="litDebug" runat="server" />
        <fieldset>
            <asp:HiddenField ID="hiddenGivenName" runat="server" />
            <asp:HiddenField ID="hiddenFamilyName" runat="server" />
            <asp:HiddenField ID="hiddenEmail" runat="server" />
            <asp:HiddenField id="hiddenGender" runat="server" />
            <asp:HiddenField ID="hiddenBirthday" runat="server" />
            <asp:HiddenField ID="hiddenIdentifier" runat="server" />
            <ati:UserName Id="atiUserNameSetup" runat="server" ValidationGroupName="oauth" />
            <ati:Password ID="atiPasswordSetup" runat="server" ValidationGroupName="oauth" />
            <ati:Slim ID="atiSlimFBSetup" runat="server" ValidationGroupName="oauth" ShowEmail="false" ShowFullName="false" /> 
            <asp:Button ID="bUserSetup" runat="server" Text="Done" ValidationGroup="oauth" OnClick="bUserSetup_Click" style="margin-top: 7px;" />
        </fieldset>
    </div>
</asp:Panel>

<asp:Panel ID="atiLoginPanel" runat="server" Visible="false">
<script type="text/javascript">
    Aqufit.EnterKeyHandler = function (event) {
        $('#<%=bLogin.ClientID%>').trigger('click');
        event.stopPropagation();
        return false;
    };
    $(function () {
        $('#dnn_ctr420_Login_DNN').unbind('keydown').hide();       
    });
</script>
<div id="atiLoginPanel" style="display: block;" class="ui-widget">  
    <div style="float: left; width: 500px;">       
        <h3>Login using FaceBook.</h3>
        <div style="padding: 20px 100px;"><asp:Literal ID="fbConnect" runat="server" /></div>      
        <h1 style="position: relative; left: 400px; top: -50px; width: 100px;">- OR -</h1>
    </div>
    <div style="float: right; width: 400px;">
    
    <fieldset>  
        <dl>
            <dt><asp:Label id="plUserNameEmail" runat="server" controlname="txtUserNameEmail" text="Username/Email:" /></dt>
            <dd><asp:TextBox id="txtUserNameEmail" runat="server" MaxLength="128" CssClass="ui-corner-all ui-widget-content atiTxtBox" /></dd>
        
            <dt><asp:Label id="plLoginPassword" runat="server" controlname="txtLoginPassword" text="Password:" /></dt>
            <dd><asp:TextBox id="txtLoginPassword" TextMode="Password" runat="server" MaxLength="128" CssClass="ui-corner-all ui-widget-content atiTxtBox" /></dd>
            
            <dt></dt>
            <dd><asp:Label id="plRememberMe" runat="server" controlname="cbRememberMe" text="Remember me:" /> <asp:CheckBox id="cbRememberMe" runat="server" /></dd>
        </dl>
        <asp:Button ID="bLogin" runat="server" Text="Sign In" OnClick="bLogin_Click" style="margin-top: 7px;" /><br /><br />
        <a href="<%=ResolveUrl("~/Home/tabid/64/ctl/SendPassword/Default.aspx")%>?returnurl=/SiteLogin.aspx">Forgot password?</a>
    </fieldset>
    </div>
</asp:Panel>   


    

         
    
               



